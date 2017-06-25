// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2016. All rights reserved.
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
using Nemiro.OAuth.Extensions;
using System.Collections.Specialized;

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Odnoklassniki</b>.
  /// </summary>
  /// <remarks>
  /// <para><b>Odnoklassniki</b> is a social network service for classmates and old friends. It is popular in Russia and CIS.</para>
  /// <h1>Register and Configure a Odnoklassniki Site</h1>
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
  /// <para>Open <b>My Games</b> page and click <b>My Uploaded Games</b>.</para>
  /// <para>
  /// <img src="../img/ok001.png" alt="My Games in the footer menu" width="650" />
  /// </para>
  /// <para>
  /// <img src="../img/ok002.png" alt="My Uploaded Games" width="650" />
  /// </para>
  /// <para>Click <b>Add App</b>.</para>
  /// <para>
  /// <img src="../img/ok003.png" alt="Add App button" width="650" />
  /// </para>
  /// <para>Enter the Title, Shortname, Description, Image link and App link. Select Application type (<b>External</b> type) and Permission.</para>
  /// <list type="table">
  /// <item>
  /// <term><img src="../img/warning.png" alt="(!)" title="" /></term>
  /// <term>
  /// <b>It is important to provide a link to the application (App link).</b><br />
  /// You must use this link as the return URL. Even for desktop applications.<br />
  /// You can use <b>localhost</b> for desktop applications.<br />
  /// <b>It is important to specify the type of application: External.</b>
  /// </term>
  /// </item>
  /// </list>
  /// <para>
  /// <img src="../img/ok004.png" alt="App form" width="650" />
  /// </para>
  /// <para>In your email box you will find email message with the <b>Client ID</b>, <b>Client Secret</b> and <b>Public key</b>.</para>
  /// <para>
  /// <img src="../img/ok005.png" alt="Email message" width="650" />
  /// </para>
  /// <para>
  /// Use this for creating an instance of the <see cref="OdnoklassnikiClient"/>.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new OdnoklassnikiClient
  ///   (
  ///     "1094959360", 
  ///     "E45991423E8C5AE249B44E84",
  ///     "CBACMEECEBABABABA"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New OdnoklassnikiClient _
  ///   (
  ///     "1094959360", 
  ///     "E45991423E8C5AE249B44E84",
  ///     "CBACMEECEBABABABA"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit to the <see href="https://apiok.ru/en">Odnoklassniki API Documentation</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <h2>Windows Forms</h2>
  /// <para>The following example shows how to use the <see cref="OdnoklassnikiClient"/> in desktop applications.</para>
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
  ///   // odnoklassniki client registration
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new OdnoklassnikiClient
  ///     (
  ///       // application ID
  ///       "1094959360", 
  ///       // sectet key
  ///       "E45991423E8C5AE249B44E84",
  ///       // public key
  ///       "CBACMEECEBABABABA"
  ///     ) { ReturnUrl = "http://localhost" } // return url - it's important
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
  ///   ' odnoklassniki client registration
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New OdnoklassnikiClient _
  ///     (
  ///       "1094959360", 
  ///       "E45991423E8C5AE249B44E84",
  ///       "CBACMEECEBABABABA"
  ///     ) With { .ReturnUrl = "http://localhost" }
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
  ///   webBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("Odnoklassniki"));
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
  ///   WebBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("Odnoklassniki"))
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
  /// <h2>ASP .NET MVC</h2>
  /// <para>The following example shows how to use the <see cref="OdnoklassnikiClient"/> in <b>ASP .NET MVC Application</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="OdnoklassnikiClient"/>.</para>
  /// <code lang="C#">
  /// protected void Application_Start()
  /// {
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new OdnoklassnikiClient
  ///     (
  ///       // application ID
  ///       "1094959360", 
  ///       // sectet key
  ///       "E45991423E8C5AE249B44E84",
  ///       // public key
  ///       "CBACMEECEBABABABA"
  ///     )
  ///   );
  /// }
  /// </code>
  /// <code lang="VB">
  /// Sub Application_Start()
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New OdnoklassnikiClient _
  ///     (
  ///       "1094959360", 
  ///       "E45991423E8C5AE249B44E84",
  ///       "CBACMEECEBABABABA"  
  ///     )
  ///   )
  /// End Sub
  /// </code>
  /// <para>The <c>OdnoklassnikiLoginResult</c> method will handle authorization result.</para>
  /// <code lang="C#">
  /// public ActionResult OdnoklassnikiLoginResult()
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
  /// Public Function OdnoklassnikiLoginResult() As ActionResult
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
  /// <para>Add action method for redirection to the <b>Odnoklassniki</b>.</para>
  /// <code lang="C#">
  /// public ActionResult OdnoklassnikiLogin()
  /// {
  ///   string authUrl = OAuthWeb.GetAuthorizationUrl("Odnoklassniki", Url.Action("OdnoklassnikiLoginResult", "Home", null, null, Request.Url.Host));
  ///   return Redirect(authUrl);
  /// }
  /// </code>
  /// <code lang="VB">
  /// Public Function OdnoklassnikiLogin() As ActionResult
  ///   Dim authUrl As String = OAuthWeb.GetAuthorizationUrl("Odnoklassniki", Url.Action("OdnoklassnikiLoginResult", "Home", Nothing, Nothing, Request.Url.Host))
  ///   Return Redirect(authUrl)
  /// End Function
  /// </code>
  /// <para>On a page add link to the <c>OdnoklassnikiLogin</c> method.</para>
  /// <code lang="html">
  /// @Html.ActionLink("Log in with Odnoklassniki", "OdnoklassnikiLogin")
  /// </code>
  /// <h2>ASP .NET WebForms</h2>
  /// <para>The following example shows how to use the <see cref="OdnoklassnikiClient"/> in <b>ASP .NET WebForms</b>.</para>
  /// <para>To test the example, create a new <b>ASP .NET WebForms</b> (empty) project. Add <b>Global.asax</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="OdnoklassnikiClient"/>.</para>
  /// <code lang="C#">
  /// using System;
  /// using System.Collections.Generic;
  /// using System.Linq;
  /// using System.Web;
  /// using System.Web.Security;
  /// using System.Web.SessionState;
  /// using Nemiro.OAuth;
  /// using Nemiro.OAuth.Clients;
  /// 
  /// namespace Test.CSharp.AspWebForms
  /// {
  ///   public class Global : System.Web.HttpApplication
  ///   {
  ///     protected void Application_Start(object sender, EventArgs e)
  ///     {
  ///       OAuthManager.RegisterClient
  ///       (
  ///         new OdnoklassnikiClient
  ///         (
  ///           // application ID 
  ///           "1094959360",
  ///           // sectet key 
  ///           "E45991423E8C5AE249B44E84",
  ///           // public key 
  ///           "CBACMEECEBABABABA"
  ///         )
  ///       );
  ///     }
  ///   }
  /// }
  /// </code>
  /// <code lang="VB">
  /// Imports Nemiro.OAuth
  /// Imports Nemiro.OAuth.Clients
  /// 
  /// Public Class Global_asax
  ///   Inherits System.Web.HttpApplication
  /// 
  ///   Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
  ///     OAuthManager.RegisterClient _
  ///     (
  ///       New OdnoklassnikiClient _
  ///       (
  ///         "1094959360",
  ///         "E45991423E8C5AE249B44E84",
  ///         "CBACMEECEBABABABA"
  ///       )
  ///     )
  ///   End Sub
  /// 
  /// End Class
  /// </code>
  /// <para>Add <b>ExternalLoginResult.aspx</b>.</para>
  /// <code lang="html">&lt;%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalLoginResult.aspx.cs" Inherits="Test.CSharp.AspWebForms.ExternalLoginResult" %gt;</code>
  /// <para>And add the following code.</para>
  /// <code lang="C#">
  /// using System;
  /// using System.Collections.Generic;
  /// using System.Linq;
  /// using System.Web;
  /// using System.Web.UI;
  /// using System.Web.UI.WebControls;
  /// using Nemiro.OAuth;
  /// 
  /// namespace Test.CSharp.AspWebForms
  /// {
  ///   public partial class ExternalLoginResult : System.Web.UI.Page
  ///   {
  ///     protected void Page_Load(object sender, EventArgs e)
  ///     {
  ///       Response.Write("&lt;pre&gt;");
  ///       var result = OAuthWeb.VerifyAuthorization();
  ///       if (result.IsSuccessfully)
  ///       {
  ///         var user = result.UserInfo;
  ///         Response.Write(String.Format("User ID: {0}&lt;br /&gt;", user.UserId));
  ///         Response.Write(String.Format("Name:    {0}&lt;br /&gt;", user.DisplayName));
  ///         Response.Write(String.Format("Email:   {0}", user.Email));
  ///       }
  ///       else
  ///       {
  ///         Response.Write(result.ErrorInfo.Message);
  ///       }
  ///       Response.Write("&lt;/pre&gt;");
  ///     }
  ///   }
  /// }
  /// </code>
  /// <code lang="VB">
  /// Imports Nemiro.OAuth
  /// 
  /// Public Class ExternalLoginResult
  ///   Inherits System.Web.UI.Page
  /// 
  ///   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  ///     Response.Write("&lt;pre&gt;")
  ///     Dim result As AuthorizationResult = OAuthWeb.VerifyAuthorization()
  ///     If result.IsSuccessfully Then
  ///       Dim user As UserInfo = result.UserInfo
  ///       Response.Write(String.Format("User ID: {0}&lt;br /&gt;", user.UserId))
  ///       Response.Write(String.Format("Name:    {0}&lt;br /&gt;", user.DisplayName))
  ///       Response.Write(String.Format("Email:   {0}", user.Email))
  ///     Else
  ///       Response.Write(result.ErrorInfo.Message)
  ///     End If
  ///     Response.Write("&lt;/pre&gt;")
  ///   End Sub
  /// 
  /// End Class
  /// </code>
  /// <para>Add <b>Default.aspx</b> and insert one <b>LinkButton</b> to the page.</para>
  /// <code lang="html">
  /// &lt;%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Test.CSharp.AspWebForms.Default" %&gt;
  /// 
  /// &lt;!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"&gt;
  /// 
  /// &lt;html xmlns="http://www.w3.org/1999/xhtml"&gt;
  /// &lt;head runat="server"&gt;
  ///     &lt;title&gt;&lt;/title&gt;
  /// &lt;/head&gt;
  /// &lt;body&gt;
  ///     &lt;form id="form1" runat="server"&gt;
  ///     &lt;div&gt;
  ///     &lt;asp:LinkButton ID="lnkOdnoklassniki" runat="server" 
  ///         Text="Log in with Odnoklassniki" onclick="lnkOdnoklassniki_Click" /&gt;
  ///     &lt;/div&gt;
  ///     &lt;/form&gt;
  /// &lt;/body&gt;
  /// &lt;/html&gt;
  /// </code>
  /// <para>Add a handler for a click on the link.</para>
  /// <code lang="C#">
  /// using System;
  /// using System.Collections.Generic;
  /// using System.Linq;
  /// using System.Web;
  /// using System.Web.UI;
  /// using System.Web.UI.WebControls;
  /// using Nemiro.OAuth;
  /// 
  /// namespace Test.CSharp.AspWebForms
  /// {
  ///   public partial class Default : System.Web.UI.Page
  ///   {
  ///     protected void lnkOdnoklassniki_Click(object sender, EventArgs e)
  ///     {
  ///       OAuthWeb.RedirectToAuthorization("Odnoklassniki", new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri);
  ///     }
  ///   }
  /// }
  /// </code>
  /// <code lang="VB">
  /// Imports Nemiro.OAuth
  /// 
  /// Public Class _Default
  ///   Inherits System.Web.UI.Page
  /// 
  ///   Protected Sub lnkOdnoklassniki_Click(sender As Object, e As EventArgs) Handles lnkOdnoklassniki.Click
  ///     OAuthWeb.RedirectToAuthorization("Odnoklassniki", New Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri)
  ///   End Sub
  /// 
  /// End Class
  /// </code>
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
  public class OdnoklassnikiClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Odnoklassniki</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Odnoklassniki";
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
          // default url
          return "https://api.ok.ru/blank.html";
        }

        return base.ReturnUrl;
      }
      set
      {
        base.ReturnUrl = value;
      }
    }

    /// <summary>
    /// Public Key for access to API.
    /// </summary>
    public string ApplicationKey { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OdnoklassnikiClient"/>.
    /// </summary>
    /// <param name="clientId">The Client ID.</param>
    /// <param name="clientSecret">The Client Secret.</param>
    /// <param name="publickKey">The Public Key.</param>
    public OdnoklassnikiClient(string clientId, string clientSecret, string publickKey) : base
    (
      "https://connect.ok.ru/oauth/authorize",
      "https://api.ok.ru/oauth/token.do",
      clientId,
      clientSecret
    )
    {
      this.ApplicationKey = publickKey; // required only for API
      base.SupportRefreshToken = true;
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <exception cref="ApiException"/>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      // https://apiok.ru/dev/methods/rest/users/users.getCurrentUser

      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // query parameters
      var parameters = new NameValueCollection
      { 
        { "method", "users.getCurrentUser" },
        { "application_key", this.ApplicationKey },
        { "format", "json" },
        { "fields", "uid,first_name,last_name,name,gender,birthday,location,pic_full,email,locale" }
      };

      // signature base string
      string signatureBaseString = parameters.Sort().ToParametersString(true);

      signatureBaseString += OAuthUtility.GetMD5Hash(accessToken.Value + this.ApplicationSecret);

      // calculate the signature
      parameters["sig"] = OAuthUtility.GetMD5Hash(signatureBaseString);
      parameters["access_token"] = accessToken.Value;

      // execute the request
      var result = OAuthUtility.Post("https://api.ok.ru/fb.do", parameters);

      // error result
      if (result["error_code"].HasValue)
      {
        throw new ApiException
        (
          result,
          result.ContainsKey("error_msg") ? result["error_msg"].ToString() : null
        );
      }

      // field mapping
      var map = new ApiDataMapping();

      map.Add("uid", "UserId", typeof(string));
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");
      map.Add("name", "DisplayName");
      map.Add("email", "Email");
      map.Add("email", "UserName");
      map.Add("pic_full", "Userpic");
      map.Add("link", "Url");
      map.Add("locale", "Language");
      map.Add("birthday", "Birthday", typeof(DateTime), @"yyyy\-MM\-dd");
      map.Add
      (
        "gender", "Sex",
        delegate(UniValue value)
        {
          if (value.Equals("male", StringComparison.OrdinalIgnoreCase))
          {
            return Sex.Male;
          }
          else if (value.Equals("female", StringComparison.OrdinalIgnoreCase))
          {
            return Sex.Female;
          }
          return Sex.None;
        }
      );

      return new UserInfo(result, map);
    }

  }

}