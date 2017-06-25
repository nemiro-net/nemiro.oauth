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
  /// OAuth client for <b>CodeProject</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a CodeProject Application</h1>
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
  /// <para><b><see href="https://www.codeproject.com/script/webapi/userclientregistrations.aspx">Register a new client</see></b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Client ID</b> and <b>Client Secret</b>.
  /// Use this for creating an instance of the <see cref="CodeProjectClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new CodeProjectClient
  ///   (
  ///     "92mWWELc2DjcL-6tu7L1Py6yllleqSCt", 
  ///     "YJXrk_Vzz4Ps02GqmaUY-aSLucxh4kfLq6oq0CtiukPfvbzb9yQG69NeDr2yiV9M"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New CodeProjectClient _
  ///   (
  ///     "92mWWELc2DjcL-6tu7L1Py6yllleqSCt", 
  ///     "YJXrk_Vzz4Ps02GqmaUY-aSLucxh4kfLq6oq0CtiukPfvbzb9yQG69NeDr2yiV9M"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please see <see href="https://api.codeproject.com/Help">CodeProject API Documentation</see>.
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
  public class CodeProjectClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>CodeProject</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "CodeProject";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeProjectClient"/>.
    /// </summary>
    /// <param name="clientId">The Client ID obtained from the <see href="https://www.codeproject.com/script/webapi/userclientregistrations.aspx">CodeProject Web API Clients</see>.</param>
    /// <param name="clientSecret">The Client Secret obtained from the <see href="https://www.codeproject.com/script/webapi/userclientregistrations.aspx">CodeProject Web API Clients</see>.</param>
    public CodeProjectClient(string clientId, string clientSecret) : base
    (
      "https://api.codeproject.com/Account/Authorize",
      "https://api.codeproject.com/token",
      clientId,
      clientSecret
    )
    {
      //base.GrantType = GrantType.ClientCredentials;
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
        "https://api.codeproject.com/v1/my/profile",
        accessToken: accessToken
      );

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id", "UserId", typeof(string));
      map.Add("userName", "UserName", typeof(string));
      map.Add("displayName", "DisplayName");
      map.Add("email", "Email");
      map.Add("homePage", "Url");
      map.Add("avatar", "Userpic");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}