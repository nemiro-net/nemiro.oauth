// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014. All rights reserved.
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

// If it works, no need to change the code. 
// Just use it! ;-)

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>SourceForge</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Twitter Application</h1>
  /// <list type="table">
  /// <item>
  /// <term><img src="../img/warning.png" alt="(!)" title="" /></term>
  /// <term>
  /// <b>Web Management Interface may change over time. Applications registration shown below may differ.</b><br />
  /// If the interface is changed, you need to register the application and get <b>Consumer ID</b> and <b>Consumer Secret</b>. For web projects, configure <b>return URLs</b>.<br />
  /// If you have any problems with this, please <see href="https://github.com/alekseynemiro/nemiro.oauth.dll/issues">visit issues</see>. If you do not find a solution to your problem, you can <see href="https://github.com/alekseynemiro/nemiro.oauth.dll/issues/new">create a new question</see>.
  /// </term>
  /// </item>
  /// </list>
  /// <para>Open the <b>SourceForge Authorized Applications</b> and <b><see href="https://sourceforge.net/auth/oauth/">Register New Application</see></b>.</para>
  /// <para>
  /// You can see <b>Consumer Key</b> and <b>Consumer Secret</b>, use this for creating an instance of the <see cref="SourceForgeClient"/>.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new SourceForgeClient
  ///   (
  ///     "cXzSHLUy57C4gTBgMGRDuqQtr", 
  ///     "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New SourceForgeClient _
  ///   (
  ///     "cXzSHLUy57C4gTBgMGRDuqQtr", 
  ///     "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://sourceforge.net/p/forge/documentation/Allura%20API">Allura API</see>.
  /// </para>
  /// </remarks>
  /// <seealso cref="AmazonClient"/>
  /// <seealso cref="DropboxClient"/>
  /// <seealso cref="FacebookClient"/>
  /// <seealso cref="FoursquareClient"/>
  /// <seealso cref="GitHubClient"/>
  /// <seealso cref="GoogleClient"/>
  /// <seealso cref="LinkedInClient"/>
  /// <seealso cref="LiveClient"/>
  /// <seealso cref="MailRuClient"/>
  /// <seealso cref="OdnoklassnikiClient"/>
  /// <seealso cref="SoundCloudClient"/>
  /// <seealso cref="TwitterClient"/>
  /// <seealso cref="VkontakteClient"/>
  /// <seealso cref="YahooClient"/>
  /// <seealso cref="YandexClient"/>
  public class SourceForgeClient : OAuthClient
  {

    /// <summary>
    /// Unique provider name: <b>SourceForge</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "SourceForge";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceForgeClient"/>.
    /// </summary>
    /// <param name="consumerKey">The <b>Consumer Key</b> obtained from the <see href="https://sourceforge.net/auth/oauth/">SourceForge OAuth applications</see>.</param>
    /// <param name="consumerSecret">The <b>Consumer Secret</b> obtained from the <see href="https://sourceforge.net/auth/oauth/">SourceForge OAuth applications</see>.</param>
    public SourceForgeClient(string consumerKey, string consumerSecret) : base
    (
      "https://sourceforge.net/rest/oauth/request_token",
      "https://sourceforge.net/rest/oauth/authorize",
      "https://sourceforge.net/rest/oauth/access_token",
      consumerKey, 
      consumerSecret,
      SignatureMethods.HMACSHA1
    ) { }


    /// <summary>
    /// Gets the user details.
    /// </summary>
    public override UserInfo GetUserInfo()
    {
      // help: https://sourceforge.net/p/forge/documentation/Allura%20API/#user

      string url = String.Format("https://sourceforge.net/rest/u/{0}/profile", "");

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
    
    /// <summary>
    /// Gets the request token from the remote server.
    /// </summary>
    public override void GetRequestToken()
    {
      base.Authorization.PrepareForRequestToken();

      if (!String.IsNullOrEmpty(base.ReturnUrl))
      {
        base.Authorization.Callback = String.Format
        (
          "{0}{1}state={2}",
          this.ReturnUrl,
          (this.ReturnUrl.Contains("?") ? "&" : "?"),
          this.State
        );
      }

      base.RequestToken = new OAuthRequestToken
      (
        OAuthUtility.ExecuteRequest
        (
          "GET",
          base.RequestTokenUrl,
          null,
          base.Authorization
        ),
        base.AuthorizeUrl,
        base.Parameters
      );
    }

  }

}
