// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2017. All rights reserved.
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
using System.Collections.Specialized;

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Yahoo</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Yahoo Application</h1>
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
  /// <para>Open <b><see href="https://developer.yahoo.com/">Yahoo Developer Network</see></b> and <b><see href="https://developer.apps.yahoo.com/dashboard/createKey.html">Create a Project</see></b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Consumer Key</b> and <b>Consumer Secret</b>.
  /// Use this for creating an instance of the <see cref="YahooClient"/> class.
  /// </para>
  /// <para>Note that <b>Yahoo!</b> does not work with the localhost. Use only a real servers. Make sure that your application on the Yahoo! dashboard configured correctly.</para>
  /// <para><b>Yahoo!</b> has a pretty flimsy OAuth interface. If something is done or configured incorrectly, the work will be nothing. But in general, the client is tested and works.</para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new YahooClient
  ///   (
  ///     "dj0yJmk9Qm1vZ3p2TmtQUm4zJmQ9WVdrOU4wbGlkWGxJTkc4bWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD0xZQ--", 
  ///     "a55738627652db0acfe464de2d9be13963b0ba1f"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New YahooClient _
  ///   (
  ///     "dj0yJmk9Qm1vZ3p2TmtQUm4zJmQ9WVdrOU4wbGlkWGxJTkc4bWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD0xZQ--", 
  ///     "a55738627652db0acfe464de2d9be13963b0ba1f"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://developer.yahoo.com/">Yahoo Developer Network</see>.
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
  public class YahooClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Yahoo</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Yahoo";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YahooClient"/>.
    /// </summary>
    /// <param name="clientId">The <b>Consumer Key</b> obtained from the <see href="https://developer.apps.yahoo.com/projects">Yahoo Developer Dashboard</see>.</param>
    /// <param name="clientSecret">The <b>Consumer Secret</b> obtained from the <see href="https://developer.apps.yahoo.com/projects">Yahoo Developer Dashboard</see>.</param>
    public YahooClient(string clientId, string clientSecret) : base
    (
      "https://api.login.yahoo.com/oauth2/request_auth",
      "https://api.login.yahoo.com/oauth2/get_token",
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
    /// <remarks>
    /// <para>Access token must contain the user ID in the parameter <b>xoauth_yahoo_guid</b>.</para>
    /// </remarks>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      RequestResult result;
      string guid = null;

      if (!UniValue.IsNullOrEmpty(accessToken["xoauth_yahoo_guid"]))
      {
        guid = accessToken["xoauth_yahoo_guid"].ToString();
      }
      else
      {
        result = OAuthUtility.Get("https://social.yahooapis.com/v1/me/guid", accessToken: accessToken);
        guid = result["guid"]["value"].ToString();
      }

      string url = String.Format("https://social.yahooapis.com/v1/user/{0}/profile?format=json", guid);

      result = OAuthUtility.Get
      (
        endpoint: url,
        accessToken: accessToken
      );

      var map = new ApiDataMapping();

      map.Add("guid", "UserId", typeof(string));
      map.Add("givenName", "FirstName");
      map.Add("familyName", "LastName");
      map.Add("nickname", "DisplayName");
      map.Add("profileUrl", "Url");
      map.Add("lang", "Language");
      map.Add("birthdate", "Birthday", typeof(DateTime), @"MM\/dd\/yyyy");
      map.Add
      (
        "gender", "Sex",
        delegate(UniValue value)
        {
          if (!value.HasValue) { return Sex.None; }
          if (value.Equals("M", StringComparison.OrdinalIgnoreCase))
          {
            return Sex.Male;
          }
          else if (value.Equals("F", StringComparison.OrdinalIgnoreCase))
          {
            return Sex.Female;
          }
          return Sex.None;
        }
      );
      map.Add
      (
        "image", "Userpic",
        delegate(UniValue value)
        {
          return Convert.ToString(value["imageUrl"]);
        }
      );
      map.Add
      (
        "phones", "Phone",
        delegate(UniValue value)
        {
          return Convert.ToString(value["number"]);
        }
      );

      return new UserInfo(result["profile"], map);
    }

    /// <summary>
    /// Gets the access token from the remote server.
    /// </summary>
    protected override void GetAccessToken()
    {
      if (String.IsNullOrEmpty(this.AuthorizationCode))
      {
        throw new ArgumentNullException("AuthorizationCode");
      }

      var parameters = new NameValueCollection
      {
        { "client_id", this.ApplicationId },
        { "client_secret", this.ApplicationSecret },
      };

      if (!String.IsNullOrEmpty(this.ReturnUrl))
      {
        parameters.Add("redirect_uri", this.ReturnUrl);
      }

      parameters.Add("code", this.AuthorizationCode);
      parameters.Add("grant_type", GrantType.AuthorizationCode);

      var result = OAuthUtility.Post
      (
        endpoint: this.AccessTokenUrl,
        parameters: parameters,
        authorization: new HttpAuthorization(AuthorizationType.Basic, OAuthUtility.ToBase64String("{0}:{1}", this.ApplicationId, this.ApplicationSecret))
      );

      if (result.ContainsKey("error"))
      {
        this.AccessToken = new AccessToken(new ErrorResult(result));
      }
      else
      {
        this.AccessToken = new OAuth2AccessToken(result);
      }
    }

    /// <summary>
    /// Sends a request to refresh the access token.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which should be refreshed.</param>
    /// <remarks>
    /// <para>If <paramref name="accessToken"/> parameter is not specified, it will use the current access token from the same property of the current class instance.</para>
    /// <para>Token must contain the <b>refresh_token</b>, which was received together with the access token.</para>
    /// <list type="table">
    /// <item>
    /// <term><img src="../img/warning.png" alt="(!)" title="" /></term>
    /// <term><b>To update the access token, you must specify the return address that was used in obtaining the access token.</b></term>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// <para>Provider does not support refreshing the access token, or the method is not implemented.</para>
    /// <para>Use the property <see cref="OAuthBase.SupportRefreshToken"/>, to check the possibility of calling this method.</para>
    /// </exception>
    /// <exception cref="AccessTokenException">
    /// <para>Access token is not found or is not specified.</para>
    /// <para>-or-</para>
    /// <para><b>refresh_token</b> value is empty.</para>
    /// </exception>
    /// <exception cref="RequestException">Error during execution of a web request.</exception>
    /// <exception cref="ArgumentNullException">An exception occurs if there is no authorization code.</exception>
    public override AccessToken RefreshToken(AccessToken accessToken = null)
    {
      return this.RefreshToken(accessToken, null);
    }

    /// <summary>
    /// Sends a request to refresh the access token.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which should be refreshed.</param>
    /// <param name="returnUrl">Callback address that was used in obtaining the access token.</param>
    /// <remarks>
    /// <para>If <paramref name="accessToken"/> parameter is not specified, it will use the current access token from the same property of the current class instance.</para>
    /// <para>Token must contain the <b>refresh_token</b>, which was received together with the access token.</para>
    /// <list type="table">
    /// <item>
    /// <term><img src="../img/warning.png" alt="(!)" title="" /></term>
    /// <term><b>To update the access token, you must specify the return address that was used in obtaining the access token.</b></term>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// <para>Provider does not support refreshing the access token, or the method is not implemented.</para>
    /// <para>Use the property <see cref="OAuthBase.SupportRefreshToken"/>, to check the possibility of calling this method.</para>
    /// </exception>
    /// <exception cref="AccessTokenException">
    /// <para>Access token is not found or is not specified.</para>
    /// <para>-or-</para>
    /// <para><b>refresh_token</b> value is empty.</para>
    /// </exception>
    /// <exception cref="RequestException">Error during execution of a web request.</exception>
    /// <exception cref="ArgumentNullException">An exception occurs if there is no authorization code.</exception>
    public AccessToken RefreshToken(AccessToken accessToken, string returnUrl)
    {
      var token = (OAuth2AccessToken)base.GetSpecifiedTokenOrCurrent(accessToken, refreshTokenRequired: true);

      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("client_id", this.ApplicationId);
      parameters.AddFormParameter("client_secret", this.ApplicationSecret);
      parameters.AddFormParameter("redirect_uri", returnUrl);
      parameters.AddFormParameter("grant_type", GrantType.RefreshToken);
      parameters.AddFormParameter("refresh_token", token.RefreshToken);

      var result = OAuthUtility.Post
      (
        this.AccessTokenUrl,
        parameters: parameters,
        authorization: new HttpAuthorization(AuthorizationType.Basic, OAuthUtility.ToBase64String("{0}:{1}", this.ApplicationId, this.ApplicationSecret))
      );

      return new OAuth2AccessToken(result);
    }
    
  
  }

}