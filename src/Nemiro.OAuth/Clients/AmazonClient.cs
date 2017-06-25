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
using System;

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Amazon</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure Amazon Application</h1>
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
  /// You need to register as developer in the <b><see href="http://login.amazon.com/manageApps">App Console</see></b>.
  /// </para>
  /// <para>
  /// <img src="../img/amazon001.png" alt="Sign in to the App Console" />
  /// </para>
  /// <para>
  /// In the <b>App Console </b> <see href="https://sellercentral.amazon.com/hz/iba/app/new">register new application</see>.
  /// </para>
  /// <para>
  /// <img src="../img/amazon002.png" alt="Register new application" />
  /// </para>
  /// <para>
  /// Specify one or more return URLs. Access to any other address will be denied.
  /// </para>
  /// <para>
  /// <b>NOTE: Amazon supports only addresses over HTTP<font color="red">S</font> (excluding localhost).</b>
  /// </para>
  /// <para>
  /// For example:
  /// <list type="bullet">
  /// <item><description>https://hamster.example.org/Home/ExternalLoginResult</description></item>
  /// <item><description>http://localhost:59962/Home/ExternalLoginResult</description></item>
  /// <item><description>http://localhost/</description></item>
  /// </list>
  /// </para>
  /// <para>
  /// <img src="../img/amazon003.png" alt="Allowed Return URLs" />
  /// </para>
  /// <para>Do not forget to save your changes.</para>
  /// <para>Use application <b>Client ID</b> and <b>Client Secret</b> when creating an instance of the <see cref="AmazonClient"/> class.</para>
  /// <para>
  /// <img src="../img/amazon004.png" alt="Allowed Return URLs" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new AmazonClient
  ///   (
  ///     "amzn1.application-oa2-client.f0ffe4edc256488dae00dcaf96d75d1b", 
  ///     "764dcefe49b441c8c6244c93e5d5d04de54fda6dfdc83da9693bf346f4dc4515"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New AmazonClient _
  ///   (
  ///     "amzn1.application-oa2-client.f0ffe4edc256488dae00dcaf96d75d1b", 
  ///     "764dcefe49b441c8c6244c93e5d5d04de54fda6dfdc83da9693bf346f4dc4515"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please see <see href="http://login.amazon.com/documentation">Amazon Developer Documentation</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <para>The following example shows how to add the <b>Amazon OAuth Client</b> to <b>ASP .NET MVC Application</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="AmazonClient"/>.</para>
  /// <code lang="C#">
  /// protected void Application_Start()
  /// {
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new AmazonClient
  ///     (
  ///       "amzn1.application-oa2-client.f0ffe4edc256488dae00dcaf96d75d1b", 
  ///       "764dcefe49b441c8c6244c93e5d5d04de54fda6dfdc83da9693bf346f4dc4515"
  ///     )
  ///   );
  /// }
  /// </code>
  /// <code lang="VB">
  /// Sub Application_Start()
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New AmazonClient _
  ///     (
  ///       "amzn1.application-oa2-client.f0ffe4edc256488dae00dcaf96d75d1b", 
  ///       "764dcefe49b441c8c6244c93e5d5d04de54fda6dfdc83da9693bf346f4dc4515"
  ///     )
  ///   )
  /// End Sub
  /// </code>
  /// <para>The <c>ExternalLoginResult</c> method will handle authorization result.</para>
  /// <code lang="C#">
  /// public ActionResult ExternalLoginResult()
  /// {
  ///   var result = OAuthWeb.VerifyAuthorization();
  ///   if (result.IsSuccessfully)
  ///   {
  ///     var user = result.UserInfo;    
  ///     // NOTE: For StringBuilder import the System.Text
  ///     StringBuilder r = new StringBuilder();
  ///     r.AppendFormat("User ID: {0}\r\n", user.UserId);
  ///     r.AppendFormat("Name:    {0}\r\n", user.DisplayName);
  ///     r.AppendFormat("Email:   {0}", user.Email);
  ///     return new ContentResult { Content = r.ToString(), ContentType = "text/plain" }; 
  ///   }
  ///   
  ///   return new ContentResult 
  ///   { 
  ///     Content = "Error: " + result.ErrorInfo.Message, 
  ///     ContentType = "text/plain" 
  ///   };
  /// }
  /// </code>
  /// <code lang="VB">
  /// Public Function ExternalLoginResult() As ActionResult
  ///   Dim result As AuthorizationResult = OAuthWeb.VerifyAuthorization()
  ///   If result.IsSuccessfully Then 
  ///     Dim user As UserInfo = result.UserInfo   
  ///     ' NOTE: For StringBuilder import the System.Text
  ///     Dim r As New StringBuilder()
  ///     r.AppendFormat("User ID: {0}", user.UserId)
  ///     r.AppendLine()
  ///     r.AppendFormat("Name:    {0}", user.DisplayName)
  ///     r.AppendLine()
  ///     r.AppendFormat("Email:   {0}", user.Email)
  ///     Return New ContentResult With { .Content = r.ToString(), .ContentType = "text/plain" } 
  ///   End If
  ///   
  ///   Return New ContentResult With _
  ///   { 
  ///     .Content = "Error: " + result.ErrorInfo.Message, 
  ///     .ContentType = "text/plain" 
  ///   }
  /// End Function
  /// </code>
  /// <para>Now you can easily redirect user to login page.</para>
  /// <code lang="C#">
  /// // NOTE: use httpS scheme for real websites
  /// string authUrl = OAuthWeb.GetAuthorizationUrl("Amazon", Url.Action("ExternalLoginResult", "Home", null, null, Request.Url.Host));
  /// // for example, MVC redirection from Action:
  /// // return Redirect(authUrl);
  /// </code>
  /// <code lang="VB">
  /// ' NOTE: use httpS scheme for real websites
  /// Dim authUrl As String = OAuthWeb.GetAuthorizationUrl("Amazon", Url.Action("ExternalLoginResult", "Home", Nothing, Nothing, Request.Url.Host))
  /// ' for example, MVC redirection from Action
  /// ' Return Redirect(authUrl)
  /// </code>
  /// <para>Result shown in the images below.</para>
  /// <para><img src="../img/amazon005.png" alt="Amazon Sign in" /></para>
  /// <para><img src="../img/amazon006.png" alt="Amazon User Info" /></para>
  /// </example>
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
  public class AmazonClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Amazon</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Amazon";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AmazonClient"/> class.
    /// </summary>
    /// <param name="clientId">The client ID obtained from the <see href="http://login.amazon.com/manageApps">App Console</see>.</param>
    /// <param name="clientSecret">The client secret obtained from the <see href="http://login.amazon.com/manageApps">App Console</see>.</param>
    public AmazonClient(string clientId, string clientSecret) : base
    (
      "https://www.amazon.com/ap/oa",
      "https://api.amazon.com/auth/o2/token",
      clientId,
      clientSecret
    )
    {
      base.DefaultScope = "profile";
      base.SupportRefreshToken = true;
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // execute the request
      var result = OAuthUtility.Get("https://api.amazon.com/user/profile", accessToken: accessToken);

      // field mapping
      var map = new ApiDataMapping();

      map.Add("user_id", "UserId");
      map.Add("name", "DisplayName");
      map.Add("email", "Email");

      // parse the server response and returns user info
      return new UserInfo(result, map);
    }

    /// <summary>
    /// Sends a request to refresh the access token.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which should be refreshed.</param>
    /// <remarks>
    /// <para>If <paramref name="accessToken"/> parameter is not specified, it will use the current access token from the same property of the current class instance.</para>
    /// <para>Token must contain the <b>refresh_token</b>, which was received together with the access token.</para>
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
    public override AccessToken RefreshToken(AccessToken accessToken = null)
    {
      if (!this.SupportRefreshToken)
      {
        throw new NotSupportedException();
      }

      var token = (OAuth2AccessToken)base.GetSpecifiedTokenOrCurrent(accessToken, refreshTokenRequired: true);

      var parameters = new HttpParameterCollection();

      parameters.AddFormParameter("access_token", token.Value);
      parameters.AddFormParameter("client_id", this.ApplicationId);
      parameters.AddFormParameter("client_secret", this.ApplicationSecret);
      parameters.AddFormParameter("grant_type", GrantType.RefreshToken);
      parameters.AddFormParameter("refresh_token",  token.RefreshToken);

      var result = OAuthUtility.Post
      (
        this.AccessTokenUrl,
        parameters: parameters
      );

      return new OAuth2AccessToken(result);
    }

  }

}