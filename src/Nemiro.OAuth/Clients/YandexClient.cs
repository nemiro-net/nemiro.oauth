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
using System.Collections.Specialized;

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Yandex</b>.
  /// </summary>
  /// <remarks>
  /// <para><b>Yandex</b> is a popular search engine in Russia and CIS.</para>
  /// <h1>Register and Configure a Yandex Application</h1>
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
  /// <para>Open the <b><see href="https://oauth.yandex.com/client/new">register new application</see></b> page, fill out the form and click <b>Save</b>.</para>
  /// <para><b>NOTE:</b> Russian language is available on the <b>yandex<font color="red">.ru</font></b></para>
  /// <para>Specify the application name and set permissions.</para>
  /// <para>
  /// To access a users profile, select <b>Yandex.Username</b>: Date of birth; Email address; User name, surname and gender. 
  /// This minimum permissions that are required to work.
  /// </para>
  /// <para>For web project, set a <b>Callback URI</b>.</para>
  /// <para><b>NOTE: For desktop applications set Callback URI to <b>https://oauth.yandex.ru/verification_code</b>.</b></para>
  /// <para>
  /// <img src="../img/yandex001.png" alt="Register new application" />
  /// </para>
  /// <para>
  /// In the next step you will see an <b>Application ID</b> and <b>Application password</b>, this is <b>Client ID</b> and <b>Client Secret</b>.
  /// Use this for creating an instance of the <see cref="YandexClient"/>.
  /// </para>
  /// <para>
  /// <img src="../img/yandex002.png" alt="Client ID and Client Secret" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new YandexClient
  ///   (
  ///     "0ee5f0bf2cd141a1b194a2b71b0332ce", 
  ///     "59d76f7c09b54ad38e6b15f792da7a9a"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New YandexClient _
  ///   (
  ///     "0ee5f0bf2cd141a1b194a2b71b0332ce", 
  ///     "59d76f7c09b54ad38e6b15f792da7a9a"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="http://api.yandex.com/oauth/doc/dg/concepts/About.xml">Yandex OAuth Documentation</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <h2>Console Applications</h2>
  /// <para>The following example shows how to use the <see cref="YandexClient"/> in <b>Console Applications</b>.</para>
  /// <para>For desktop applications, the user will need to manually enter authorization code.</para>
  /// <code lang="C#">
  /// class Program
  /// {
  ///   static void Main(string[] args)
  ///   {
  ///     try
  ///     {
  ///       var yandex = new YandexClient
  ///       (
  ///         "0ee5f0bf2cd141a1b194a2b71b0332ce",
  ///         "59d76f7c09b54ad38e6b15f792da7a9a"
  ///       );
  ///       
  ///       // open the login page in browser
  ///       System.Diagnostics.Process.Start(yandex.AuthorizationUrl);
  ///       
  ///       // waiting of entering the access code
  ///       string code = "";
  ///       while (String.IsNullOrEmpty(code))
  ///       {
  ///         Console.WriteLine("Enter access code:");
  ///         code = Console.ReadLine();
  ///       }
  /// 
  ///       Console.WriteLine();
  /// 
  ///       // set authorization code
  ///       yandex.AuthorizationCode = code;
  ///       // get user info
  ///       var user = yandex.GetUserInfo();
  ///       Console.WriteLine("User ID:   {0}", user.UserId);
  ///       Console.WriteLine("E-Mail:    {0}", user.Email);
  ///       Console.WriteLine("Name:      {0}", user.DisplayName);
  ///       Console.WriteLine("Birthday:  {0}", user.Birthday);
  ///       Console.WriteLine("Sex:       {0}", user.Sex);
  ///     }
  ///     catch (Exception ex)
  ///     {
  ///       Console.WriteLine(ex.Message);
  ///     }
  ///     Console.ReadKey();
  ///   }
  /// }
  /// </code>
  /// <code lang="VB">
  /// Imports Nemiro.OAuth
  /// Imports Nemiro.OAuth.Clients
  /// 
  /// Module Module1
  /// 
  ///   Sub Main()
  ///     Try
  ///       Dim yandex As New YandexClient _
  ///       (
  ///         "0ee5f0bf2cd141a1b194a2b71b0332ce",
  ///         "59d76f7c09b54ad38e6b15f792da7a9a"
  ///       )
  ///       ' open the login page in browser
  ///       System.Diagnostics.Process.Start(yandex.AuthorizationUrl)
  /// 
  ///       ' waiting of entering the access code
  ///       Dim code As String = ""
  ///       Do While String.IsNullOrEmpty(code)
  ///         Console.WriteLine("Enter access code:")
  ///         code = Console.ReadLine()
  ///       Loop
  /// 
  ///       ' set authorization code
  ///       yandex.AuthorizationCode = code
  /// 
  ///       ' get user info
  ///       Dim user As UserInfo = yandex.GetUserInfo()
  ///       Console.WriteLine("User ID:   {0}", user.UserId)
  ///       Console.WriteLine("E-Mail:    {0}", user.Email)
  ///       Console.WriteLine("Name:      {0}", user.DisplayName)
  ///       Console.WriteLine("Birthday:  {0}", user.Birthday)
  ///       Console.WriteLine("Sex:       {0}", user.Sex)
  ///     Catch ex As Exception
  ///       Console.WriteLine(ex.Message)
  ///     End Try
  ///     Console.ReadKey()
  ///   End Sub
  /// 
  /// End Module
  /// </code>
  /// <para>Result of the program is shown in the images below.</para>
  /// <para><img src="../img/yandex003.png" alt="Log in with Yandex" /></para>
  /// <para><img src="../img/yandex004.png" alt="Access code" /></para>
  /// <para><img src="../img/yandex005.png" alt="User info" /></para>
  /// <h2>ASP .NET WebForms</h2>
  /// <para>In a web projects you can use the <see cref="OAuthManager"/> and <see cref="OAuthWeb"/>.</para>
  /// <para>The following example shows how to use the <see cref="YandexClient"/> in <b>ASP .NET WebForms</b>.</para>
  /// <para>To test the example, create a new <b>ASP .NET WebForms</b> (empty) project. Add <b>Global.asax</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="YandexClient"/>.</para>
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
  ///         new YandexClient
  ///         (
  ///           "0ee5f0bf2cd141a1b194a2b71b0332ce",
  ///           "59d76f7c09b54ad38e6b15f792da7a9a"
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
  ///       New YandexClient _
  ///       (
  ///         "0ee5f0bf2cd141a1b194a2b71b0332ce",
  ///         "59d76f7c09b54ad38e6b15f792da7a9a"
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
  ///         Response.Write(String.Format("E-Mail:  {0}&lt;br /&gt;", user.Email));
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
  ///       Response.Write(String.Format("E-Mail:  {0}&lt;br /&gt;", user.Email))
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
  ///     &lt;asp:LinkButton ID="lnkYandex" runat="server" 
  ///         Text="Log in with Yandex" onclick="lnkYandex_Click" /&gt;
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
  ///     protected void lnkYandex_Click(object sender, EventArgs e)
  ///     {
  ///       OAuthWeb.RedirectToAuthorization("Yandex", new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri);
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
  ///   Protected Sub lnkYandex_Click(sender As Object, e As EventArgs) Handles lnkYandex.Click
  ///     OAuthWeb.RedirectToAuthorization("Yandex", New Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri)
  ///   End Sub
  /// 
  /// End Class
  /// </code>
  /// <para><b>NOTE:</b> Do not forget to adjust the <b>Callback URI</b> in the <b>Yandex Application Settings</b>.</para>
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
  public class YandexClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Yandex</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Yandex";
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
          return "https://oauth.yandex.ru/verification_code";
        }

        return base.ReturnUrl;
      }
      set
      {
        base.ReturnUrl = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YandexClient"/>.
    /// </summary>
    /// <param name="clientId">The Application ID.</param>
    /// <param name="clientSecret">The Application Password.</param>
    public YandexClient(string clientId, string clientSecret) : base
    (
      "https://oauth.yandex.ru/authorize",
      "https://oauth.yandex.ru/token",
      clientId,
      clientSecret
    ) 
    { }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // query parameters
      var parameters = new NameValueCollection
      { 
        { "format", "json" }
      };

      // execute the request
      var result = OAuthUtility.Get
      (
        "https://login.yandex.ru/info", 
        parameters: parameters, 
        authorization: new HttpAuthorization(AuthorizationType.OAuth, accessToken.Value)
      );

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id", "UserId", typeof(string));
      map.Add("login", "UserName");
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");
      map.Add("display_name", "DisplayName");
      map.Add("birthday", "Birthday", typeof(DateTime), @"yyyy\-MM\-dd");
      map.Add("default_email", "Email");
      /*map.Add
      (
        "emails", "Email",
        delegate(object value)
        {
          if (value != null && value.GetType().IsArray)
          {
            return ((Array)value).GetValue(0);
          }
          return null;
        }
      );*/
      map.Add
      (
        "sex", "Sex",
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

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}