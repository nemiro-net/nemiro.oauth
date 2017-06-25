// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2015. All rights reserved.
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
using System.Collections.Specialized;

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>SoundCloud</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a SoundCloud Application</h1>
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
  /// <para>Open the <b><see href="https://developers.soundcloud.com/">SoundCloud for Developers</see></b> and <b><see href="http://soundcloud.com/you/apps/new">Register a new app</see></b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Client ID</b> and <b>Client Secret</b>.
  /// Use this for creating an instance of the <see cref="SoundCloudClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new SoundCloudClient
  ///   (
  ///     "42b58d31e399664a3fb8503bfcaaa9ba", 
  ///     "f9d85648da59fb95ec131b40c7645c31"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New SoundCloudClient _
  ///   (
  ///     "42b58d31e399664a3fb8503bfcaaa9ba", 
  ///     "f9d85648da59fb95ec131b40c7645c31"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://developers.soundcloud.com/">SoundCloud for Developers</see>.
  /// </para>
  /// </remarks>
  /// <seealso cref="AmazonClient"/>
  /// <seealso cref="AssemblaClient"/>
  /// <seealso cref="CodeProjectClient"/>
  /// <seealso cref="DropboxClient"/>
  /// <seealso cref="FacebookClient"/>
  /// <seealso cref="FoursquareClient"/>
  /// <seealso cref="GitHubClient"/>
  /// <seealso cref="GoogleClient"/>
  /// <seealso cref="InstagramClient"/>
  /// <seealso cref="LinkedInClient"/>
  /// <seealso cref="LiveClient"/>
  /// <seealso cref="MailRuClient"/>
  /// <seealso cref="OdnoklassnikiClient"/>
  /// <seealso cref="SoundCloudClient"/>
  /// <seealso cref="TumblrClient"/>
  /// <seealso cref="TwitterClient"/>
  /// <seealso cref="VkontakteClient"/>
  /// <seealso cref="YahooClient"/>
  /// <seealso cref="YandexClient"/>
  public class SoundCloudClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>SoundCloud</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "SoundCloud";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundCloudClient"/>.
    /// </summary>
    /// <param name="clientId">The <b>Client ID</b> obtained from the <see href="http://soundcloud.com/you/apps">SoundCloud Applications</see>.</param>
    /// <param name="clientSecret">The <b>Client Secret</b> obtained from the <see href="http://soundcloud.com/you/apps">SoundCloud Applications</see>.</param>
    public SoundCloudClient(string clientId, string clientSecret) : base
    (
      "https://soundcloud.com/connect",
      "https://api.soundcloud.com/oauth2/token",
      clientId,
      clientSecret
    )
    {
      // https://developers.soundcloud.com/docs/api/reference
      base.ScopeSeparator = " "; // don't know...
      base.DefaultScope = "non-expiring";
      // features for access token
      // https://developers.soundcloud.com/docs/api/guide#authentication
      base.SupportRefreshToken = false; // scope = "*" not working and refresh_token too...
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // query parameters
      var parameters = new NameValueCollection
      { 
        { "oauth_token", accessToken.Value }
      };

      // execute the request
      var result = OAuthUtility.Get("https://api.soundcloud.com/me.json", parameters);

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id", "UserId", typeof(string));
      map.Add("username", "DisplayName");
      map.Add("permalink_url", "Url"); // website
      map.Add("avatar_url", "Userpic");
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}