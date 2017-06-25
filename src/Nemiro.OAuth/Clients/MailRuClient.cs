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
using System.Linq;
using Nemiro.OAuth.Extensions;
using System.Collections.Specialized;

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Mail.Ru</b>.
  /// </summary>
  /// <remarks>
  /// <para><b>Mail.Ru</b> is a popular email service, search engine, social network, photo &amp; video hosting, blogs and another services in Russia and CIS.</para>
  /// <h1>Register and Configure a Mail.Ru Site</h1>
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
  /// <para>Unfortunately, the interface is only in Russian.</para>
  /// <para>You must have a confirmed account in Mail.Ru.</para>
  /// <para>Open <see href="http://api.mail.ru/sites/my/">Sites</see> page and click <see href="http://api.mail.ru/sites/my/add/">Connect a New Site</see>.</para>
  /// <para>
  /// <img src="../img/mailru001.png" alt="Create a New Site button" />
  /// </para>
  /// <para>Accept the terms of the agreement.</para>
  /// <para>
  /// <img src="../img/mailru002.png" alt="Terms" />
  /// </para>
  /// <para>Enter the site name, URL and click the <b>Next</b> button.</para>
  /// <para>
  /// <img src="../img/mailru003.png" alt="Site name and URL" />
  /// </para>
  /// <para>Download the <b>receiver.html</b> file and place it in the root directory of your site.</para>
  /// <para>This step can be <i>skipped</i> (<i>Пропустить</i>).</para>
  /// <para>
  /// <img src="../img/mailru004.png" alt="receiver.html file" />
  /// </para>
  /// <para>The last step you will find the <b>Client ID</b> and <b>Client Secret</b>.</para>
  /// <para>
  /// Use this for creating an instance of the <see cref="MailRuClient"/>.
  /// </para>
  /// <para>
  /// <img src="../img/mailru005.png" alt="Cleint ID and Client Secret" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new MailRuClient
  ///   (
  ///     "722701", 
  ///     "d0622d3d9c9efc69e4ca42aa173b938a"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New MailRuClient _
  ///   (
  ///     "722701", 
  ///     "d0622d3d9c9efc69e4ca42aa173b938a"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit to the <see href="http://api.mail.ru/docs/">Mail.Ru API Documentation</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <h2>Windows Forms</h2>
  /// <para>The following example shows how to use the <see cref="MailRuClient"/> in desktop applications.</para>
  /// <para>To test the example, create a new <b>Windows Forms</b> project with two forms. Add a <c>Button</c> to the <c>Form1</c>.</para>
  /// <code lang="C#">
  /// public Form1()
  /// {
  ///   InitializeComponent();
  ///   button1.Click += new EventHandler(button1_Click);
  /// }
  /// 
  /// private void Form1_Load(object sender, EventArgs e)
  /// {
  ///   // mail.ru client registration
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new MailRuClient
  ///     (
  ///       "722701", 
  ///       "d0622d3d9c9efc69e4ca42aa173b938a"
  ///     )
  ///   );
  /// }
  /// 
  /// private void button1_Click(object sender, EventArgs e)
  /// {
  ///   var frm = new Form2();
  ///   frm.ShowDialog();
  /// }
  /// </code>
  /// <code lang="VB">
  /// Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
  ///   ' mail.ru client registration
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New MailRuClient _
  ///     (
  ///       "722701",
  ///       "d0622d3d9c9efc69e4ca42aa173b938a"
  ///     )
  ///   )
  /// End Sub
  /// 
  /// Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
  ///   Call New Form2().ShowDialog()
  /// End Sub
  /// </code>
  /// <para>Add a <c>WebBrowser</c> to the <c>Form2</c>.</para>
  /// <code lang="C#">
  /// public Form2()
  /// {
  ///   InitializeComponent();
  ///   webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
  ///   webBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("Mail.Ru"));
  /// }
  /// 
  /// private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
  /// {
  ///   // waiting for results
  ///   if (e.Url.Query.IndexOf("code=") != -1 || e.Url.Fragment.IndexOf("code=") != -1 || e.Url.Query.IndexOf("oauth_verifier=") != -1)
  ///   {
  ///     // is the result, verify
  ///     var result = OAuthWeb.VerifyAuthorization(e.Url.ToString());
  ///     if (result.IsSuccessfully)
  ///     {
  ///       // show user info
  ///       MessageBox.Show
  ///       (
  ///         String.Format
  ///         (
  ///           "User ID: {0}\r\nUsername: {1}\r\nDisplay Name: {2}\r\nE-Mail: {3}", 
  ///           result.UserInfo.UserId,
  ///           result.UserInfo.UserName,
  ///           result.UserInfo.DisplayName ?? result.UserInfo.FullName,
  ///           result.UserInfo.Email
  ///         ), 
  ///         "Successfully", 
  ///         MessageBoxButtons.OK, 
  ///         MessageBoxIcon.Information
  ///       );
  ///     }
  ///     else
  ///     {
  ///       // show error message
  ///       MessageBox.Show(result.ErrorInfo.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
  ///     }
  ///     this.Close();
  ///   }
  /// }
  /// </code>
  /// <code lang="VB">
  /// Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
  ///   WebBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("Mail.Ru"))
  /// End Sub
  ///
  /// Private Sub WebBrowser1_DocumentCompleted(sender As System.Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
  ///   ' waiting for results
  ///   If Not e.Url.Query.IndexOf("code=") = -1 OrElse Not e.Url.Fragment.IndexOf("code=") = -1 OrElse Not e.Url.Query.IndexOf("oauth_verifier=") = -1 Then
  ///     ' is the result, verify
  ///     Dim result = OAuthWeb.VerifyAuthorization(e.Url.ToString())
  ///     If result.IsSuccessfully Then
  ///       ' show user info
  ///       MessageBox.Show _
  ///       (
  ///         String.Format _
  ///         (
  ///           "User ID: {0}{4}Username: {1}{4}Display Name: {2}{4}E-Mail: {3}",
  ///           result.UserInfo.UserId,
  ///           result.UserInfo.UserName,
  ///           If(Not String.IsNullOrEmpty(result.UserInfo.DisplayName), result.UserInfo.DisplayName, result.UserInfo.FullName),
  ///           result.UserInfo.Email,
  ///           vbNewLine
  ///         ),
  ///         "Successfully",
  ///         MessageBoxButtons.OK,
  ///         MessageBoxIcon.Information
  ///       )
  ///     Else
  ///       ' show error message
  ///       MessageBox.Show(result.ErrorInfo.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
  ///     End If
  ///     Me.Close()
  ///   End If
  /// End Sub
  /// </code>
  /// <para>Result of the program is shown in the images below.</para>
  /// <para><img src="../img/mailru006.png" alt="Log in with Mail.Ru" /></para>
  /// <para><img src="../img/mailru007.png" alt="Mail.Ru User Info" /></para>
  /// <h2>ASP .NET MVC</h2>
  /// <para>The following example shows how to use the <see cref="MailRuClient"/> in <b>ASP .NET MVC Application</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="MailRuClient"/>.</para>
  /// <code lang="C#">
  /// protected void Application_Start()
  /// {
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new MailRuClient
  ///     (
  ///       "722701", 
  ///       "d0622d3d9c9efc69e4ca42aa173b938a"
  ///     )
  ///   );
  /// }
  /// </code>
  /// <code lang="VB">
  /// Sub Application_Start()
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New MailRuClient _
  ///     (
  ///       "722701", 
  ///       "d0622d3d9c9efc69e4ca42aa173b938a"
  ///     )
  ///   )
  /// End Sub
  /// </code>
  /// <para>The <c>MailRuLoginResult</c> method will handle authorization result.</para>
  /// <code lang="C#">
  /// public ActionResult MailRuLoginResult()
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
  /// Public Function MailRuLoginResult() As ActionResult
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
  /// <para>Add action method for redirection to the <b>Mail.Ru</b>.</para>
  /// <code lang="C#">
  /// public ActionResult MailRuLogin()
  /// {
  ///   string authUrl = OAuthWeb.GetAuthorizationUrl("Mail.Ru", Url.Action("MailRuLoginResult", "Home", null, null, Request.Url.Host));
  ///   return Redirect(authUrl);
  /// }
  /// </code>
  /// <code lang="VB">
  /// Public Function MailRuLogin() As ActionResult
  ///   Dim authUrl As String = OAuthWeb.GetAuthorizationUrl("Mail.Ru", Url.Action("MailRuLoginResult", "Home", Nothing, Nothing, Request.Url.Host))
  ///   Return Redirect(authUrl)
  /// End Function
  /// </code>
  /// <para>On a page add link to the <c>MailRuLogin</c> method.</para>
  /// <code lang="html">
  /// @Html.ActionLink("Log in with Mail.Ru", "MailRuLogin")
  /// </code>
  /// <para><b>NOTE:</b> For proper processing, you will need to download and put on your site a <b>receiver.html</b> file.</para>
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
  public class MailRuClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Mail.Ru</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Mail.Ru";
      }
    }

    /// <summary>
    /// Return URL.
    /// </summary>
    public override string ReturnUrl
    {
      get
      {
        if (String.IsNullOrEmpty(base.ReturnUrl))
        {
          // default return url
          return "http://connect.mail.ru/oauth/success.html";
        }

        return base.ReturnUrl;
      }
      set
      {
        base.ReturnUrl = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MailRuClient"/>.
    /// </summary>
    /// <param name="clientId">The Client ID.</param>
    /// <param name="clientSecret">The Client Secret.</param>
    public MailRuClient(string clientId, string clientSecret) : base
    (
      "https://connect.mail.ru/oauth/authorize",
      "https://connect.mail.ru/oauth/token",
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
    /// <remarks>
    /// <para>The access token must contain the user ID in the parameter <b>x_mailru_vid</b>.</para>
    /// </remarks>
    /// <exception cref="ApiException"/>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      // http://api.mail.ru/docs/reference/rest/users.getInfo/

      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      var parameters = new NameValueCollection
      { 
        { "method", "users.getInfo" },
        { "app_id", this.ApplicationId },
        { "secure", "1" },
        { "uid", accessToken["x_mailru_vid"].ToString() },
        { "format", "json" }
      };

      string signatureBaseString = parameters.Sort().ToParametersString();

      parameters["sig"] = OAuthUtility.GetMD5Hash(signatureBaseString + this.ApplicationSecret);

      var result = OAuthUtility.Post("http://www.appsmail.ru/platform/api", parameters);

      if (!result.IsCollection)
      {
        throw new ApiException(result, "Expected one-dimensional array."); // expected one-dimensional array.
      }

      var map = new ApiDataMapping();

      map.Add("uid", "UserId", typeof(string));
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");
      map.Add("nick", "DisplayName");
      map.Add("email", "Email");
      map.Add("email", "UserName");
      map.Add("pic", "Userpic");
      map.Add("link", "Url");
      map.Add("birthday", "Birthday", typeof(DateTime), @"dd\.MM\.yyyy");
      map.Add
      (
        "sex", "Sex",
        delegate(UniValue value)
        {
          if (Convert.ToInt32(value) == 0)
          {
            return Sex.Male;
          }
          else if (Convert.ToInt32(value) == 1)
          {
            return Sex.Female;
          }
          return Sex.None;
        }
      );

      return new UserInfo(result.First(), map);
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
      var token = (OAuth2AccessToken)base.GetSpecifiedTokenOrCurrent(accessToken, refreshTokenRequired: true);

      // fix: требуют bearer, но в этом запросе bearer не поддерживают
      token = new OAuth2AccessToken(token.Value, token.RefreshToken, "");

      var parameters = new NameValueCollection
      { 
        { "client_id", this.ApplicationId },
        { "client_secret", this.ApplicationSecret },
        { "grant_type", GrantType.RefreshToken },
        { "refresh_token", token.RefreshToken }
      };

      var result = OAuthUtility.Post
      (
        "https://appsmail.ru/oauth/token",
        parameters: parameters,
        accessToken: token
      );

      return new OAuth2AccessToken(result);
    }

  }

}