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
  /// OAuth client for <b>Dropbox</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Dropbox Application</h1>
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
  /// <para>Open the <b><see href="https://www.dropbox.com/developers/apps">Dropbox App Console</see></b> and <b>Create app</b>.</para>
  /// <para>Choose the <b>Dropbox API app</b> type.</para>
  /// <para>
  /// In the application settings  you can found <b>App key</b> and <b>App secret</b>.
  /// Use this for creating an instance of the <see cref="DropboxClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new DropboxClient
  ///   (
  ///     "0le6wsyp3y085wy", 
  ///     "48afwq9yth83y7u"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New DropboxClient _
  ///   (
  ///     "0le6wsyp3y085wy", 
  ///     "48afwq9yth83y7u"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://www.dropbox.com/developers">Dropbox Platform Help</see>.
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
  public class DropboxClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Dropbox</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Dropbox";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DropboxClient"/>.
    /// </summary>
    /// <param name="clientId">The <b>App key</b> obtained from the <see href="https://www.dropbox.com/developers/apps">Dropbox App Console</see>.</param>
    /// <param name="clientSecret">The <b>App secret</b> obtained from the <see href="https://www.dropbox.com/developers/apps">Dropbox App Console</see>.</param>
    public DropboxClient(string clientId, string clientSecret) : base
    (
      "https://www.dropbox.com/1/oauth2/authorize",
      "https://api.dropbox.com/1/oauth2/token", 
      clientId,
      clientSecret
    ) 
    { }
    
    /// <summary>
    /// Gets the user details.
    /// </summary>
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
        "https://api.dropbox.com/1/account/info",
        parameters,
        null
      );

      // field mapping
      var map = new ApiDataMapping();
      map.Add("uid", "UserId", typeof(string));
      map.Add("display_name", "DisplayName");
      map.Add("email", "Email");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result.Result as Dictionary<string, object>, map);
    }

  }

}