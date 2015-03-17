// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014-2015. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Nemiro.OAuth;

namespace AspNetWebFormsCustomClient
{

  public class MyFacebookClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>MyFacebook</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "MyFacebook";
      }
    }

    /// <summary>
    /// Return URL.
    /// </summary>
    public override string ReturnUrl
    {
      get
      {
        if (String.IsNullOrEmpty(base.ReturnUrl))
        {
          // default return url
          return "https://www.facebook.com/connect/login_success.html";
        }
        return base.ReturnUrl;
      }
      set
      {
        base.ReturnUrl = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyFacebookClient"/>.
    /// </summary>
    /// <param name="clientId">The App ID obtained from the <see href="https://developers.facebook.com/apps/">Facebook Developers</see>.</param>
    /// <param name="clientSecret">The App Secret obtained from the <see href="https://developers.facebook.com/apps/">Facebook Developers</see>.</param>
    public MyFacebookClient(string clientId, string clientSecret) : base
    (
      "https://www.facebook.com/dialog/oauth",
      "https://graph.facebook.com/oauth/access_token",
      clientId,
      clientSecret
    )
    {
      // scope list
      base.ScopeSeparator = ",";
      base.DefaultScope = "public_profile,email"; //,user_website,user_birthday
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <remarks>
    /// <para>
    /// For more details, please see <see href="https://developers.facebook.com/docs/graph-api/reference/v2.0/user">User</see> method in <b>Guide of Facebook Graph API</b>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // query parameters
      var parameters = new NameValueCollection
      { 
        { "access_token" , this.AccessToken["access_token"].ToString() }
      };

      // execute the request
      var result = OAuthUtility.Get("https://graph.facebook.com/me", parameters);

      // field mapping
      var map = new ApiDataMapping();
      map.Add("id", "UserId", typeof(string));
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");
      map.Add("name", "DisplayName");
      map.Add("email", "Email");
      map.Add("link", "Url"); // website
      map.Add("birthday", "Birthday", typeof(DateTime), @"MM\/dd\/yyyy");
      map.Add
      (
        "gender", "Sex",
        delegate(UniValue value)
        {
          if (value.HasValue)
          {
            if (value.Equals("male", StringComparison.OrdinalIgnoreCase))
            {
              return Sex.Male;
            }
            else if (value.Equals("female", StringComparison.OrdinalIgnoreCase))
            {
              return Sex.Female;
            }
          }
          return Sex.None;
        }
      );

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}