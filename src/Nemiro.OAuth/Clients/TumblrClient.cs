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
  /// OAuth client for <b>Tumblr</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Tumblr Application</h1>
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
  /// <para>Open the <b><see href="https://www.tumblr.com/oauth/apps">Tumblr Dashboard</see></b>, and <b>Register an application</b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Consumer Key</b> and <b>Consumer Secret</b>.
  /// Use this for creating an instance of the <see cref="TumblrClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new TumblrClient
  ///   (
  ///     "2EZbsj2oF8OAouPlDWSVnESetAchImzPLV4q0IcQH7DGKECuzJ", 
  ///     "4WZ3HBDwNuz5ZDZY8qyK1qA5QFHEJY7gkPK6ooYFCN4yw6crKd"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New TumblrClient _
  ///   (
  ///     "2EZbsj2oF8OAouPlDWSVnESetAchImzPLV4q0IcQH7DGKECuzJ", 
  ///     "4WZ3HBDwNuz5ZDZY8qyK1qA5QFHEJY7gkPK6ooYFCN4yw6crKd"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://www.tumblr.com/docs/en/api/v2">Tumblr API Documentation</see>.
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
  public class TumblrClient : OAuthClient
  {

    /// <summary>
    /// Unique provider name: <b>Tumblr</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Tumblr";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TumblrClient"/>.
    /// </summary>
    /// <param name="consumerKey">The <b>Consumer Key</b> obtained from the <see href="https://www.tumblr.com/oauth/apps">Tumblr Dashboard</see>.</param>
    /// <param name="consumerSecret">The <b>Consumer Secret</b> obtained from the <see href="https://www.tumblr.com/oauth/apps">Tumblr Dashboard</see>.</param>
    public TumblrClient(string consumerKey, string consumerSecret) : base
    (
      "http://www.tumblr.com/oauth/request_token",
      "http://www.tumblr.com/oauth/authorize",
      "http://www.tumblr.com/oauth/access_token",
      consumerKey,
      consumerSecret,
      SignatureMethods.HMACSHA1
    ) { }

    /// <summary>
    /// Gets an user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      // api documentation: 
      // https://www.tumblr.com/docs/en/api/v2#user-methods

      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      this.Authorization["oauth_token"] = accessToken["oauth_token"].ToString();
      this.Authorization.TokenSecret = accessToken["oauth_token_secret"].ToString();
      // required new stamp
      this.Authorization.Timestamp = OAuthUtility.GetTimeStamp();
      this.Authorization.Nonce = OAuthUtility.GetRandomKey();

      // execute the request
      var result = OAuthUtility.Get
      (
        endpoint: "https://api.tumblr.com/v2/user/info",
        parameters: new HttpParameterCollection { new HttpUrlParameter("api_key", this.ApplicationId) },
        authorization: this.Authorization
      );

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id", "UserId", typeof(string));
      map.Add("name", "DisplayName");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result["response"]["user"], map);
    }

  }

}