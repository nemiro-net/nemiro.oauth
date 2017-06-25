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
using System.Linq;
using System.Collections.Specialized;

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>VK</b> (<b>VKontakte</b>).
  /// </summary>
  /// <remarks>
  /// <para>VK (VKontakte) is "Russian Facebook". :-)</para>
  /// <h1>Register and Configure a VKontakte Application</h1>
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
  /// <para>Open the <b><see href="http://vk.com/dev">VK App development</see></b> and click <b>Create an Application</b>.</para>
  /// <para>
  /// <img src="../img/vk001.png" alt="Create an Application button" />
  /// </para>
  /// <para>Specify the application name and type, and click the <b>Connect Application</b>.</para>
  /// <para>
  /// <img src="../img/vk002.png" alt="Creating an application" />
  /// </para>
  /// <para>Confirm by SMS.</para>
  /// <para>
  /// <img src="../img/vk003.png" alt="Confirmation" />
  /// </para>
  /// <para>
  /// In the <b>Application Settings</b> you can found <b>Application ID</b> and <b>Secure key</b>, this is <b>Client ID</b> and <b>Client Secret</b>.
  /// Use this for creating an instance of the <see cref="VkontakteClient"/> class.
  /// </para>
  /// <para><b>NOTE:</b> Change application status to <b>Application ON and visible to all</b>.</para>
  /// <para>
  /// <img src="../img/vk004.png" alt="Application settings" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new VkontakteClient
  ///   (
  ///     "4457505", 
  ///     "wW5lFMVbsw0XwYFgCGG0"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New VkontakteClient _
  ///   (
  ///     "4457505", 
  ///     "wW5lFMVbsw0XwYFgCGG0"
  ///   )
  /// )
  /// </code>
  /// <para>For web projects, enable <b>Open API</b>, set <b>Site address</b> and configure <b>Base domain</b> in the <b>Open API</b> section.</para>
  /// <para>
  /// <img src="../img/vk005.png" alt="Base domains" />
  /// </para>
  /// <para>
  /// For more details, please see <see href="http://vk.com/dev">VK App development</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <h2>Windows Forms</h2>
  /// <para>The following example shows how to use the <see cref="VkontakteClient"/> in desktop applications.</para>
  /// <para>To test the example, create a new <b>Windows Forms</b> project with two forms. Insert a <c>Button</c> to the <c>Form1</c>.</para>
  /// <code lang="C#">
  /// public Form1()
  /// {
  ///   InitializeComponent();
  ///   button1.Click += new EventHandler(button1_Click);
  /// }
  /// 
  /// private void Form1_Load(object sender, EventArgs e)
  /// {
  ///   // vk(ontakte) client registration
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new VkontakteClient
  ///     (
  ///       // application id
  ///       "4457505", 
  ///       // secure secret
  ///       "wW5lFMVbsw0XwYFgCGG0"
  ///     ) 
  ///     { 
  ///       // display=popup - to open a popup window
  ///       Parameters = new NameValueCollection { { "display", "popup" } } 
  ///     }
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
  ///   ' vk(ontakte) client registration
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New VkontakteClient _
  ///     (
  ///       "4457505",
  ///       "wW5lFMVbsw0XwYFgCGG0"
  ///     ) With _
  ///     {
  ///       .Parameters = New NameValueCollection() From {{"display", "popup"}}
  ///     }
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
  ///   webBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("vk"));
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
  ///   WebBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("vk"))
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
  /// <para><img src="../img/vk006.png" alt="Log in with VK(ontakte)" /></para>
  /// <para><img src="../img/vk007.png" alt="VK User Info" /></para>
  /// <h2>ASP .NET MVC</h2>
  /// <para>The following example shows how to use the <see cref="VkontakteClient"/> in <b>ASP .NET MVC Application</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="VkontakteClient"/>.</para>
  /// <code lang="C#">
  /// protected void Application_Start()
  /// {
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new VkontakteClient
  ///     (
  ///       "4457505", 
  ///       "wW5lFMVbsw0XwYFgCGG0"
  ///     )
  ///   );
  /// }
  /// </code>
  /// <code lang="VB">
  /// Sub Application_Start()
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New VkontakteClient _
  ///     (
  ///       "4457505", 
  ///       "wW5lFMVbsw0XwYFgCGG0"  
  ///     )
  ///   )
  /// End Sub
  /// </code>
  /// <para>The <c>VkontakteLoginResult</c> method will handle authorization result.</para>
  /// <code lang="C#">
  /// public ActionResult VkontakteLoginResult()
  /// {
  ///   var result = OAuthWeb.VerifyAuthorization();
  ///   if (result.IsSuccessfully)
  ///   {
  ///     var user = result.UserInfo;    
  ///     // NOTE: For StringBuilder import the System.Text
  ///     StringBuilder r = new StringBuilder();
  ///     r.AppendFormat("User ID: {0}\r\n", user.UserId);
  ///     r.AppendFormat("Name:    {0}\r\n", user.DisplayName);
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
  /// Public Function VkontakteLoginResult() As ActionResult
  ///   Dim result As AuthorizationResult = OAuthWeb.VerifyAuthorization()
  ///   If result.IsSuccessfully Then 
  ///     Dim user As UserInfo = result.UserInfo   
  ///     ' NOTE: For StringBuilder import the System.Text
  ///     Dim r As New StringBuilder()
  ///     r.AppendFormat("User ID: {0}", user.UserId)
  ///     r.AppendLine()
  ///     r.AppendFormat("Name:    {0}", user.DisplayName)
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
  /// <para>Add action method for redirection to the <b>Vkontakte</b>.</para>
  /// <code lang="C#">
  /// public ActionResult VkontakteLogin()
  /// {
  ///   string authUrl = OAuthWeb.GetAuthorizationUrl("vk", Url.Action("VkontakteLoginResult", "Home", null, null, Request.Url.Host));
  ///   return Redirect(authUrl);
  /// }
  /// </code>
  /// <code lang="VB">
  /// Public Function VkontakteLogin() As ActionResult
  ///   Dim authUrl As String = OAuthWeb.GetAuthorizationUrl("vk", Url.Action("VkontakteLoginResult", "Home", Nothing, Nothing, Request.Url.Host))
  ///   Return Redirect(authUrl)
  /// End Function
  /// </code>
  /// <para>On a page add link to the <c>VkontakteLogin</c> method.</para>
  /// <code lang="html">
  /// @Html.ActionLink("Log in with VK(ontakte)", "VkontakteLogin")
  /// </code>
  /// <h2>ASP .NET WebForms</h2>
  /// <para>The following example shows how to use the <see cref="VkontakteClient"/> in <b>ASP .NET WebForms</b>.</para>
  /// <para>To test the example, create a new <b>ASP .NET WebForms</b> (empty) project. Add <b>Global.asax</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="VkontakteClient"/>.</para>
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
  ///         new VkontakteClient
  ///         (
  ///           "4457505", 
  ///           "wW5lFMVbsw0XwYFgCGG0"
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
  ///       New VkontakteClient _
  ///       (
  ///         "4457505", 
  ///         "wW5lFMVbsw0XwYFgCGG0"
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
  ///     &lt;asp:LinkButton ID="lnkVkontakte" runat="server" 
  ///         Text="Log in with VK(ontakte)" onclick="lnkVkontakte_Click" /&gt;
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
  ///     protected void lnkVkontakte_Click(object sender, EventArgs e)
  ///     {
  ///       OAuthWeb.RedirectToAuthorization("vk", new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri);
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
  ///   Protected Sub lnkVkontakte_Click(sender As Object, e As EventArgs) Handles lnkVkontakte.Click
  ///     OAuthWeb.RedirectToAuthorization("vk", New Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri)
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
  public class VkontakteClient : OAuth2Client
  {

    // VK has problem with OAuth:
    // http://vk.com/bugs?act=show&id=1022459_1

    /// <summary>
    /// Unique provider name: <b>VK</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "VK";
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
          return "https://oauth.vk.com/blank.html";
        }

        return base.ReturnUrl;
      }
      set
      {
        base.ReturnUrl = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VkontakteClient"/>.
    /// </summary>
    /// <param name="clientId">The Application ID obtained from the <see href="http://vk.com/dev">VK App development</see>.</param>
    /// <param name="clientSecret">The Secure Key obtained from the <see href="http://vk.com/dev">VK App development</see>.</param>
    public VkontakteClient(string clientId, string clientSecret) : base
    (
      "https://oauth.vk.com/authorize",
      "https://oauth.vk.com/access_token",
      clientId,
      clientSecret
    )
    {
      base.DefaultScope = "status,email";
      base.ScopeSeparator = ",";
    }


    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <exception cref="ApiException"/>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      // help: http://vk.com/dev/users.get

      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // query parameters
      var parameters = new NameValueCollection
      { 
        { "user_ids", accessToken["user_id"].ToString() },
        { "fields", "sex,bdate,city,country,photo_max_orig,domain,contacts,site" },
        { "v", "5.65" }
      };

      // execute the request
      var result = OAuthUtility.Get
      (
        "https://api.vk.com/method/users.get", 
        parameters: parameters, 
        accessToken: accessToken
      );

      if (result.ContainsKey("error"))
      {
        throw new ApiException(result, result["error"]["error_msg"].ToString());
      }

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id", "UserId", typeof(string));
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");
      map.Add("domain", "UserName");
      map.Add("photo_max_orig", "Userpic");
      map.Add("mobile_phone", "Phone");
      map.Add("bdate", "Birthday", typeof(DateTime), @"dd\.MM\.yyyy");
      map.Add
      (
        "site", "Url",
        delegate(UniValue value)
        {
          if (value.HasValue || String.IsNullOrEmpty(value.ToString())) { return null; }

          return value.ToString().Split(' ').First();
        }
      );
      map.Add
      (
        "sex", "Sex",
        delegate(UniValue value)
        {
          if (Convert.ToInt32(value) == 2)
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

      // email, thanks to Aleksander (KamAz) Kryatov (http://vk.com/acid_rock) for idea
      if (accessToken.ContainsKey("email"))
      {
        result["response"].First().Add("at_email", accessToken["email"]);
        map.Add("at_email", "Email", typeof(string));
      }
      // --

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result["response"].First(), map);
    }

  }

}