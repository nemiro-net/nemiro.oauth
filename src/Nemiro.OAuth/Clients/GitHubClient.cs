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
using System.Collections.Specialized;

// If it works, no need to change the code. 
// Just use it! ;-)

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>GitHub</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a GitHub Application</h1>
  /// <list type="table">
  /// <item>
  /// <term><img src="../img/warning.png" alt="(!)" title="" /></term>
  /// <term>
  /// <b>Web Management Interface may change over time. Applications registration shown below may differ.</b><br />
  /// If the interface is changed, you need to register the application and get <b>Client ID</b> and <b>Client Secret</b>. For web projects, configure <b>return URLs</b>.<br />
  /// If you have any problems with this, please <see href="https://github.com/alekseynemiro/nemiro.oauth.dll/issues">visit issues</see>. If you do not find a solution to your problem, you can <see href="https://github.com/alekseynemiro/nemiro.oauth.dll/issues/new">create a new question</see>.
  /// </term>
  /// </item>
  /// </list>
  /// <para>
  /// </para>
  /// <para><b><see href="https://github.com/settings/applications/new">Register a new OAuth application</see></b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Client ID</b> and <b>Client Secret</b>.
  /// Use this for creating an instance of the <see cref="GitHubClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new GitHubClient
  ///   (
  ///     "e14122695d88f5c95bce", 
  ///     "cde23ec001c5180e01e865f4efb57cb0bc848c16"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New GitHubClient _
  ///   (
  ///     "e14122695d88f5c95bce", 
  ///     "cde23ec001c5180e01e865f4efb57cb0bc848c16"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please see <see href="https://developer.github.com/guides/">GitHub Development Guides</see>.
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
  public class GitHubClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>GitHub</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "GitHub";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubClient"/>.
    /// </summary>
    /// <param name="clientId">The Client ID obtained from the <see href="https://github.com/settings/applications">GitHub Applications</see>.</param>
    /// <param name="clientSecret">The Client Secret obtained from the <see href="https://github.com/settings/applications">GitHub Applications</see>.</param>
    public GitHubClient(string clientId, string clientSecret) : base
    (
      "https://github.com/login/oauth/authorize",
      "https://github.com/login/oauth/access_token", 
      clientId,
      clientSecret
    ) 
    {
      // default scope list
      this.Scope = "user"; // https://developer.github.com/v3/oauth/#scopes
    }
    
    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For more details, please see <see href="https://developers.facebook.com/docs/graph-api/reference/v2.0/user">User</see> method in <b>Guide of Facebook Graph API</b>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    public override UserInfo GetUserInfo()
    {
      // query parameters
      var parameters = new NameValueCollection
      { 
        { "access_token" , this.AccessToken["access_token"].ToString() }
      };

      // execute the request
      var result = OAuthUtility.ExecuteRequest
      (
        "GET",
        "https://api.github.com/user",
        parameters,
        null
      );

      // help: https://developer.github.com/v3/users/#get-the-authenticated-user

      // field mapping
      var map = new ApiDataMapping();
      map.Add("id", "UserId", typeof(string));
      map.Add("login", "UserName", typeof(string));
      map.Add("name", "DisplayName");
      map.Add("email", "Email");
      map.Add("html_url", "Url"); //blog
      map.Add("avatar_url", "Userpic", typeof(DateTime), @"MM\/dd\/yyyy");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result.Result as Dictionary<string, object>, map);
    }

  }

}