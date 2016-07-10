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
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Nemiro.OAuth;

// If it works, no need to change the code. 
// Just use it! ;-)

namespace AspNetWebFormsCustomClient
{

  public class MyTwitterClient : OAuthClient
  {

    /// <summary>
    /// Unique provider name: <b>Twitter</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        // The name is the same as in the library.
        // If in the application is registered client of the library,
        // then you will have to use a different name.
        return "Twitter";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyTwitterClient"/>.
    /// </summary>
    /// <param name="consumerKey">The API Key obtained from the <see href="https://apps.twitter.com">Twitter Application Management</see>.</param>
    /// <param name="consumerSecret">The API Secret obtained from the <see href="https://apps.twitter.com">Twitter Application Management</see>.</param>
    public MyTwitterClient(string consumerKey, string consumerSecret) : base
    (
      "https://api.twitter.com/oauth/request_token",
      "https://api.twitter.com/oauth/authorize",
      "https://api.twitter.com/oauth/access_token",
      consumerKey,
      consumerSecret,
      SignatureMethods.HMACSHA1 // only HMAC-SHA1
    ) { }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // help: https://dev.twitter.com/docs/api/1/get/users/show

      string url = "https://api.twitter.com/1.1/users/show.json";

      // query parameters
      var parameters = new HttpParameterCollection();
      parameters.AddUrlParameter("user_id", this.AccessToken["user_id"].ToString());
      parameters.AddUrlParameter("screen_name", this.AccessToken["screen_name"].ToString());
      parameters.AddUrlParameter("include_entities", "false");

      this.Authorization["oauth_token"] = this.AccessToken["oauth_token"];
      this.Authorization.TokenSecret = this.AccessToken["oauth_token_secret"].ToString();

      // execute the request
      var result = OAuthUtility.Get(url, parameters, this.Authorization);

      // field mapping
      var map = new ApiDataMapping();
      map.Add("id_str", "UserId", typeof(string));
      map.Add("name", "DisplayName");
      map.Add("screen_name", "UserName");
      map.Add("profile_image_url", "Userpic");
      map.Add("url", "Url");
      map.Add("birthday", "Birthday", typeof(DateTime), @"dd\.MM\.yyyy");
      //map.Add("verified", "Url");
      //map.Add("location", "Url");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}