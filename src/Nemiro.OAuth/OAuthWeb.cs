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
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents helper class for sessions management of OAuth.
  /// </summary>
  /// <remarks>
  /// <para>Mainly the class is intended for web projects.</para>
  /// <para>But you can use some methods of the class in desktop applications together with the <b>WebBrowser</b> control.</para>
  /// <para>
  /// The methods <see cref="RedirectToAuthorization(string)"/>, 
  /// <see cref="RedirectToAuthorization(string, string)"/>, 
  /// <see cref="RedirectToAuthorization(string, NameValueCollection, string)"/> and
  /// <see cref="VerifyAuthorization()"/>
  /// will not work in desktop applications.
  /// </para>
  /// </remarks>
  /// <example>
  /// <h2>ASP .NET WebForms</h2>
  /// <para>The following example shows how to use the <see cref="Nemiro.OAuth.Clients.TwitterClient"/> in <b>ASP .NET WebForms</b>.</para>
  /// <para>To redirect the user to the login page is used the <see cref="RedirectToAuthorization(string, string)"/> method.</para>
  /// <para>Processing of the authorization results is performed by <see cref="VerifyAuthorization()"/>.</para>
  /// <para>To test the example, create a new <b>ASP .NET WebForms</b> (empty) project. Add <b>Global.asax</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="Nemiro.OAuth.Clients.TwitterClient"/>.</para>
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
  ///         new TwitterClient
  ///         (
  ///           "cXzSHLUy57C4gTBgMGRDuqQtr",
  ///           "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
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
  ///       New TwitterClient _
  ///       (
  ///         "cXzSHLUy57C4gTBgMGRDuqQtr",
  ///         "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
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
  ///     &lt;asp:LinkButton ID="lnkTwitter" runat="server" 
  ///         Text="Log in with Twitter" onclick="lnkTwitter_Click" /&gt;
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
  ///     protected void lnkTwitter_Click(object sender, EventArgs e)
  ///     {
  ///       OAuthWeb.RedirectToAuthorization("Twitter", new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri);
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
  ///   Protected Sub lnkTwitter_Click(sender As Object, e As EventArgs) Handles lnkTwitter.Click
  ///     OAuthWeb.RedirectToAuthorization("Twitter", New Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri)
  ///   End Sub
  /// 
  /// End Class
  /// </code>
  /// <h2>Windows Forms</h2>
  /// <para>The following example shows how to use the <see cref="Nemiro.OAuth.Clients.MailRuClient"/> in desktop applications.</para>
  /// <para>Methods redirection in Windows Forms applications do not work. To get the address of the authorization <see cref="GetAuthorizationUrl(string)"/> is used.</para>
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
  /// </example>
  public static class OAuthWeb
  {

    #region RedirectToAuthorization

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="clientName"/> is unregistered. Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string)"/>
    public static void RedirectToAuthorization(string clientName)
    {
      OAuthWeb.RedirectToAuthorization(ClientName.Parse(clientName), null, null, null);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider with specified parameters.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization query.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="clientName"/> is unregistered. Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, NameValueCollection)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string, NameValueCollection)"/>
    public static void RedirectToAuthorization(string clientName, NameValueCollection parameters)
    {
      OAuthWeb.RedirectToAuthorization(ClientName.Parse(clientName), parameters, null, null);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="clientName"/> is unregistered. Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string, string)"/>
    public static void RedirectToAuthorization(string clientName, string returnUrl)
    {
      OAuthWeb.RedirectToAuthorization(ClientName.Parse(clientName), null, returnUrl, null);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <param name="state">Custom state associated with authorization request.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="clientName"/> is unregistered. Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string, string, object)"/>
    public static void RedirectToAuthorization(string clientName, string returnUrl, object state)
    {
      OAuthWeb.RedirectToAuthorization(ClientName.Parse(clientName), null, returnUrl, state);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization query.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="clientName"/> is unregistered. Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, NameValueCollection, string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string, NameValueCollection, string)"/>
    public static void RedirectToAuthorization(string clientName, NameValueCollection parameters, string returnUrl)
    {
      OAuthWeb.RedirectToAuthorization(ClientName.Parse(clientName), parameters, returnUrl, null);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization query.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="clientName"/> is unregistered. Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, NameValueCollection, string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(ClientName, NameValueCollection, string)"/>
    public static void RedirectToAuthorization(ClientName clientName, NameValueCollection parameters, string returnUrl)
    {
      OAuthWeb.RedirectToAuthorization(clientName, parameters, returnUrl, null);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization query.</param>
    /// <param name="state">Custom state associated with authorization request.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="clientName"/> is unregistered. Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, NameValueCollection, string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(ClientName, NameValueCollection, string, object)"/>
    public static void RedirectToAuthorization(ClientName clientName, NameValueCollection parameters, string returnUrl, object state)
    {
      if (!OAuthManager.RegisteredClients.ContainsKey(clientName))
      {
        throw new ClientIsNotRegisteredException();
      }

      // get normal client name
      clientName = OAuthManager.RegisteredClients.Keys.First(k => k == clientName);

      // create new instance of the client
      var client = OAuthManager.RegisteredClients[clientName].Clone(parameters, returnUrl);

      // add request
      OAuthManager.AddRequest(client.State, clientName, client, state);

      // redirect
      client.RedirectToAuthorization();
    }

    #endregion
    #region GetAuthorizationUrl

    /// <summary>
    /// Returns the authorization URL of the specified provider.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string clientName)
    {
      return OAuthWeb.GetAuthorizationUrl(ClientName.Parse(clientName), null, null, null);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider with specified parameters.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization URL.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string clientName, NameValueCollection parameters)
    {
      return OAuthWeb.GetAuthorizationUrl(ClientName.Parse(clientName), parameters, null, null);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string clientName, string returnUrl)
    {
      return OAuthWeb.GetAuthorizationUrl(ClientName.Parse(clientName), null, returnUrl, null);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <param name="state">Custom state associated with authorization request.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string clientName, string returnUrl, object state)
    {
      return OAuthWeb.GetAuthorizationUrl(ClientName.Parse(clientName), null, returnUrl, state);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="clientName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization URL.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string clientName, NameValueCollection parameters, string returnUrl)
    {
      return OAuthWeb.GetAuthorizationUrl(ClientName.Parse(clientName), parameters, returnUrl, null);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="clientName">
    /// The provider name, through which it is necessary to authorize the current user; or the name of the registered client.
    /// </param>
    /// <param name="parameters">Additional parameters to be passed to the authorization URL.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(ClientName clientName, NameValueCollection parameters, string returnUrl)
    {
      return OAuthWeb.GetAuthorizationUrl(clientName, parameters, returnUrl, null);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="clientName">
    /// The provider name, through which it is necessary to authorize the current user; or the name of the registered client.
    /// </param>
    /// <param name="parameters">Additional parameters to be passed to the authorization URL.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <param name="state">Custom state associated with authorization request.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(ClientName clientName, NameValueCollection parameters, string returnUrl, object state)
    {
      if (!OAuthManager.RegisteredClients.ContainsKey(clientName))
      {
        throw new ClientIsNotRegisteredException();
      }

      // get normal client name
      clientName = OAuthManager.RegisteredClients.Keys.First(k => k == clientName);

      // create new instance of the client
      var client = OAuthManager.RegisteredClients[clientName].Clone(parameters, returnUrl);

      // add request
      OAuthManager.AddRequest(client.State, clientName, client, state);

      // return url
      return client.AuthorizationUrl;
    }

    #endregion
    #region VerifyAuthorization

    /// <summary>
    /// Verifies the authorization results for the current URL.
    /// </summary>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use the overloads <see cref="VerifyAuthorization(string)"/> or <see cref="VerifyAuthorization(string, string)"/>.</para>
    /// </remarks>
    /// <returns>
    /// <para>Returns the verification results.</para>
    /// </returns>
    public static AuthorizationResult VerifyAuthorization()
    {
      if (HttpContext.Current == null)
      {
        return new AuthorizationResult { ErrorInfo = new NullHttpContextException() };
      }
      return OAuthWeb.VerifyAuthorization(HttpContext.Current.Request.Url.ToString());
    }

    /// <summary>
    /// Verifies the authorization results for the specified URL.
    /// </summary>
    /// <param name="url">Address at which to perform the verification.</param>
    /// <returns>
    /// <para>Returns the verification results.</para>
    /// </returns>
    public static AuthorizationResult VerifyAuthorization(string url)
    {
      var result = new AuthorizationResult();

      try
      {
        // HtmlDecode - small fix for wrong data from provider. 
        // Thanks to @Nacer ( https://github.com/Nacer- ) // v1.8
        UriBuilder u = new UriBuilder(HttpUtility.HtmlDecode(url));
        NameValueCollection qs = null;

        if (!String.IsNullOrEmpty(u.Query))
        {
          qs = HttpUtility.ParseQueryString(u.Query);
        }
        else if (String.IsNullOrEmpty(u.Query) && !String.IsNullOrEmpty(u.Fragment))
        {
          qs = HttpUtility.ParseQueryString(u.Fragment.Substring(1));
        }

        if (qs == null)
        {
          throw new AuthorizationException("Invalid URL. Verification code is not found.");
        }

        if (!String.IsNullOrEmpty(qs["denied"]))
        {
          throw new AccessDeniedException(qs["error_description"]);
        }

        if (!String.IsNullOrEmpty(qs["error"]))
        {
          switch (qs["error"].ToLower())
          {
            case "access_denied":
              throw new AccessDeniedException(qs["error_description"]);

            default:
              throw new AuthorizationException(qs["error_description"] ?? qs["error"]);
          }
        }

        result = OAuthWeb.VerifyAuthorization(qs["state"], qs["oauth_verifier"] ?? qs["code"]);
      }
      catch (Exception ex)
      {
        result.ErrorInfo = ex;
      }

      return result;
    }

    /// <summary>
    /// Verifies the authorization results for the specified request identifier and the code of the authorization.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="code">The authorization code received from the provider server.</param>
    /// <returns>
    /// <para>Returns the verification results.</para>
    /// </returns>
    /// <remarks>
    /// <para>This method is intended for internal use. It is recommended to use the overload <see cref="VerifyAuthorization()"/> or <see cref="VerifyAuthorization(string)"/>.</para>
    /// </remarks>
    public static AuthorizationResult VerifyAuthorization(string requestId, string code)
    {
      var result = new AuthorizationResult();

      try
      {
        if (String.IsNullOrEmpty(requestId))
        {
          throw new ArgumentNullException("requestId");
        }

        if (String.IsNullOrEmpty(code))
        {
          throw new ArgumentNullException("code");
        }

        if (!OAuthManager.RequestsProvider.ContainsKey(requestId))
        {
          throw new AuthorizationException("Sorry, request key not found. Please try again authorization.");
        }

        var request = OAuthManager.RequestsProvider.Get(requestId);
        var client = request.Client;

        client.AuthorizationCode = code;
        result.RequestId = requestId;
        result.ClientName = request.ClientName;
        result.ProtocolVersion = client.Version.ToString(2);
        result.AccessToken = client.AccessToken;
        result.State = request.State;

        // is not empty and not error
        if (!result.AccessToken.IsEmpty && result.AccessToken.IsSuccessfully)
        {
          // get the user profile details
          result.UserInfo = client.GetUserInfo(client.AccessToken);
        }
      }
      catch (Exception ex)
      {
        // error
        result.ErrorInfo = ex;
      }

      return result;
    }

    #endregion
    #region VerifyAuthorizationAndRemoveRequest

    /// <summary>
    /// Verifies the authorization results for the current URL and removes the request from memory.
    /// </summary>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use the overloads <see cref="VerifyAuthorization(string)"/> or <see cref="VerifyAuthorization(string, string)"/>.</para>
    /// </remarks>
    /// <returns>
    /// <para>Returns the verification results.</para>
    /// </returns>
    public static AuthorizationResult VerifyAuthorizationAndRemoveRequest()
    {
      return OAuthWeb.VerifyAuthorizationAndRemoveRequest(HttpContext.Current.Request.Url.ToString());
    }

    /// <summary>
    /// Verifies the authorization results for the specified URL, and removes the request from memory.
    /// </summary>
    /// <param name="url">Address at which to perform the verification.</param>
    /// <returns>
    /// <para>Returns the verification results.</para>
    /// </returns>
    public static AuthorizationResult VerifyAuthorizationAndRemoveRequest(string url)
    {
      var result = OAuthWeb.VerifyAuthorization(url);
      OAuthManager.RemoveRequest(result.RequestId);
      return result;
    }

    /// <summary>
    /// Verifies the authorization results for the specified request identifier and the code of the authorization, and removes the request from memory.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="code">The authorization code received from the provider server.</param>
    /// <returns>
    /// <para>Returns the verification results.</para>
    /// </returns>
    /// <remarks>
    /// <para>This method is intended for internal use. It is recommended to use the overload <see cref="VerifyAuthorization()"/> or <see cref="VerifyAuthorization(string)"/>.</para>
    /// </remarks>
    public static AuthorizationResult VerifyAuthorizationAndRemoveRequest(string requestId, string code)
    {
      var result = OAuthWeb.VerifyAuthorization(requestId, code);
      OAuthManager.RemoveRequest(requestId);
      return result;
    }

    #endregion

    /*
    public static AuthorizationResult Authorize(string providerName, NameValueCollection parameters, string returnUrl, string username, string password)
    {
      if (!OAuthManager.RegisteredClients.ContainsKey(providerName))
      {
        throw new ClientIsNotRegisteredException();
      }

      if (OAuthManager.RegisteredClients[providerName].Version != "2.0")
      {
        throw new ClientIsNotRegisteredException();//todo
      }

      var result = new AuthorizationResult();

      try
      {
        var client = (OAuth2Client)OAuthManager.RegisteredClients[providerName].Clone(parameters, returnUrl);
        // set username and password
        client.Username = username;
        client.Password = password;

        // preparing results
        result.ProviderName = client.ProviderName;
        result.ProtocolVersion = client.Version;
        result.AccessToken = client.AccessToken;

        // is not empty and not error
        if (result.AccessToken.GetType() != typeof(EmptyResult) && result.AccessToken.GetType() != typeof(ErrorResult))
        {
          // get the user profile details
          result.UserInfo = client.GetUserInfo();
        }
      }
      catch (Exception ex)
      {
        // error
        result.ErrorInfo = ex;
      }
      return result;
    }
    */

  }

}