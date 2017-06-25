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
  /// OAuth client for <b>Assembla</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure an Assembla Application</h1>
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
  /// <para><b><see href="https://app.assembla.com/user/edit/manage_clients">Register a new OAuth application</see></b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Application ID</b> and <b>Application Secret</b>.
  /// Use this for creating an instance of the <see cref="AssemblaClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new AssemblaClient
  ///   (
  ///     "bOS4QkXnmr5jhdacwqjQXA", 
  ///     "701ee6dedf74fc4ad75bfa7476666a2f"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New AssemblaClient _
  ///   (
  ///     "bOS4QkXnmr5jhdacwqjQXA", 
  ///     "701ee6dedf74fc4ad75bfa7476666a2f"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="http://api-docs.assembla.cc/">Assembla API Documentation Site</see>.
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
  public class AssemblaClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Assembla</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Assembla";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblaClient"/>.
    /// </summary>
    /// <param name="clientId">The Application ID obtained from the <see href="https://www.assembla.com/user/edit/manage_clients">Assembla Applications Manager</see>.</param>
    /// <param name="clientSecret">The Application Secret obtained from the <see href="https://www.assembla.com/user/edit/manage_clients">Assembla Applications Manager</see>.</param>
    public AssemblaClient(string clientId, string clientSecret) : base
    (
      "https://api.assembla.com/authorization",
      "https://api.assembla.com/token",
      clientId,
      clientSecret
    )
    {
      base.SupportRefreshToken = true;
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

      // execute the request
      var result = OAuthUtility.Get
      (
        "https://api.assembla.com/v1/user.json",
        accessToken: accessToken
      );

      // help: http://api-docs.assembla.cc/content/ref/user_show.html
      // http://api-docs.assembla.cc/content/ref/user_fields.html

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id", "UserId", typeof(string));
      map.Add("login", "UserName", typeof(string));
      map.Add("name", "DisplayName");
      map.Add("email", "Email");
      map.Add("phone", "Phone");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}