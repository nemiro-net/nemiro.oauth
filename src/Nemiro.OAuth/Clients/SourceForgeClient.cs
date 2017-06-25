// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014, 2016. All rights reserved.
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

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>SourceForge</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register a SourceForge Application</h1>
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
  [Obsolete("It is difficult to support. Bad documentation. Does not work stably. Over time, it will be deleted. // 2017-06-25")]
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
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      return new UserInfo(UniValue.Empty, null);
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <param name="usersname">Name of the user whose data should be obtained.</param>
    public UserInfo GetUserInfo(AccessToken accessToken = null, string usersname = "")
    {
      if (String.IsNullOrEmpty("usersname"))
      {
        throw new ArgumentNullException("usersname");
      }

      // help: https://sourceforge.net/p/forge/documentation/Allura%20API/#user

      string url = String.Format("https://sourceforge.net/rest/u/{0}/profile", usersname);

      this.Authorization["oauth_token"] = this.AccessToken["oauth_token"];
      this.Authorization.TokenSecret = this.AccessToken["oauth_token_secret"].ToString();

      base.Authorization["oauth_body_hash"] = "2jmj7l5rSw0yVb/vlWAYkK/YBwk=";
      base.Authorization.Build("GET", url, null, null);

      // execute the request
      var result = OAuthUtility.Get(url, base.Authorization.Value.ToNameValueCollection());

      // field mapping
      var map = new ApiDataMapping();
      map.Add("username", "UserName");
      map.Add("name", "DisplayName");
      map.Add("url", "Url");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result["developers"][0], map);
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

      // var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
      base.Authorization["oauth_body_hash"] = "2jmj7l5rSw0yVb/vlWAYkK/YBwk="; // Convert.ToBase64String(sha1.ComputeHash(new byte[] { }));

      // debug
      // base.Authorization.Nonce = "13020680";
      // base.Authorization.Timestamp = "1423301309";

      base.Authorization.Build("GET", base.RequestTokenUrl, null, null);

      base.RequestToken = new OAuthRequestToken
      (
        OAuthUtility.Get(base.RequestTokenUrl, base.Authorization.Value.ToNameValueCollection()),
        base.AuthorizeUrl,
        base.Parameters
      );
    }

    /// <summary>
    /// Gets the access token from the remote server.
    /// </summary>
    protected override void GetAccessToken()
    {
      // authorization code is required for request
      if (String.IsNullOrEmpty(this.AuthorizationCode))
      {
        throw new ArgumentNullException("AuthorizationCode");
      }

      // set default access tken value
      this.AccessToken = Nemiro.OAuth.AccessToken.Empty;

      // prepare
      this.Authorization.PrepareForAccessToken();

      // set request data
      this.Authorization.Verifier = this.AuthorizationCode;
      this.Authorization.Token = this.RequestToken.OAuthToken;
      this.Authorization.TokenSecret = this.RequestToken.OAuthTokenSecret;

      base.Authorization["oauth_body_hash"] = "2jmj7l5rSw0yVb/vlWAYkK/YBwk=";

      base.Authorization.Build("GET", base.AccessTokenUrl, null, null);

      // send request
      base.AccessToken = new OAuthAccessToken
      (
        OAuthUtility.Get
        (
          this.AccessTokenUrl,
          this.Authorization.Value.ToNameValueCollection()
        )
      );

      // remove oauth_verifier from headers
      this.Authorization.Remove("oauth_verifier");
    }

  }

}