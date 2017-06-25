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

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Instagram</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Instagram Application</h1>
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
  /// <para><b><see href="http://www.instagram.com/developer/register/">Register as Developer</see></b> and <b><see href="http://www.instagram.com/developer/clients/register/">Register new Client ID</see></b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Client ID</b> and <b>Client Key</b>.
  /// Use this for creating an instance of the <see cref="InstagramClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new InstagramClient
  ///   (
  ///     "215a1941ebed4e4fa74e94dd84762836", 
  ///     "ba53a710e1624870bc066e7a9ae38601"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New InstagramClient _
  ///   (
  ///     "215a1941ebed4e4fa74e94dd84762836", 
  ///     "ba53a710e1624870bc066e7a9ae38601"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://www.instagram.com/developer/">Instagram Developer Documentation</see>.
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
  public class InstagramClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Instagram</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Instagram";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstagramClient"/>.
    /// </summary>
    /// <param name="clientId">The <b>Client ID</b> obtained from the <see href="http://instagram.com/developer/clients/manage/">Instagram Manage Clients</see>.</param>
    /// <param name="clientSecret">The <b>Client Secret</b> obtained from the <see href="http://instagram.com/developer/clients/manage/">Instagram Manage Clients</see>.</param>
    public InstagramClient(string clientId, string clientSecret) : base
    (
      "https://api.instagram.com/oauth/authorize/",
      "https://api.instagram.com/oauth/access_token",
      clientId,
      clientSecret
    )
    {
      // default scope
      base.ScopeSeparator = " ";
      base.DefaultScope = "basic";
      // for more details please see 
      // https://www.instagram.com/developer/authorization/
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

      var result = OAuthUtility.Get
      (
        "https://api.instagram.com/v1/users/self",
        accessToken: accessToken
      );

      // field mapping
      var map = new ApiDataMapping();
      map.Add("id", "UserId", typeof(string));
      map.Add("username", "UserName");
      map.Add("website", "Url");
      map.Add("profile_picture", "Userpic");
      map.Add("full_name", "DisplayName");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result["data"], map);
    }

  }

}