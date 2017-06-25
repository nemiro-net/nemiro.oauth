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

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Twitter</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Twitter Application</h1>
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
  /// <para>Open the <b><see href="https://apps.twitter.com">Twitter Application Management</see></b> and <b>Create a New App</b>.</para>
  /// <para>
  /// <img src="../img/twitter001.png" alt="Create a New App button" />
  /// </para>
  /// <para>Fill out the form and click the <b>Create your Twitter application</b>.</para>
  /// <para>For web project, set a <b>Callback URL</b>.</para>
  /// <para>
  /// <img src="../img/twitter002.png" alt="Create a New App form" />
  /// </para>
  /// <para>
  /// Open the application page and click to the <b>API Keys</b>.
  /// </para>
  /// <para>
  /// <img src="../img/twitter003.png" alt="API Keys link" />
  /// </para>
  /// <para>
  /// You can see <b>API Key</b> and <b>API secret</b>, this is <b>Consumer Key</b> and <b>Consumer Secret</b>.
  /// Use this for creating an instance of the <see cref="TwitterClient"/>.
  /// </para>
  /// <para>
  /// <img src="../img/twitter004.png" alt="Client ID and Client Secret" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new TwitterClient
  ///   (
  ///     "cXzSHLUy57C4gTBgMGRDuqQtr", 
  ///     "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New TwitterClient _
  ///   (
  ///     "cXzSHLUy57C4gTBgMGRDuqQtr", 
  ///     "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://dev.twitter.com/docs">Twitter Developers Documentation</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <h2>Console Applications</h2>
  /// <para>The following example shows how to use the <see cref="TwitterClient"/> in <b>Console Applications</b>.</para>
  /// <para>For desktop applications, the user will need to manually enter authorization code.</para>
  /// <code lang="C#">
  /// class Program
  /// {
  ///   static void Main(string[] args)
  ///   {
  ///     try
  ///     {
  ///       var twitter = new TwitterClient
  ///       (
  ///         "cXzSHLUy57C4gTBgMGRDuqQtr",
  ///         "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
  ///       );
  ///       
  ///       // open the login page in browser
  ///       System.Diagnostics.Process.Start(twitter.AuthorizationUrl);
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
  ///       twitter.AuthorizationCode = code;
  ///       // get user info
  ///       var user = twitter.GetUserInfo();
  ///       Console.WriteLine("User ID: {0}", user.UserId);
  ///       Console.WriteLine("Name:    {0}", user.DisplayName);
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
  ///       Dim twitter As New TwitterClient _
  ///       (
  ///         "cXzSHLUy57C4gTBgMGRDuqQtr",
  ///         "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
  ///       )
  ///       ' open the login page in browser
  ///       System.Diagnostics.Process.Start(twitter.AuthorizationUrl)
  /// 
  ///       ' waiting of entering the access code
  ///       Dim code As String = ""
  ///       Do While String.IsNullOrEmpty(code)
  ///         Console.WriteLine("Enter access code:")
  ///         code = Console.ReadLine()
  ///       Loop
  /// 
  ///       ' set authorization code
  ///       twitter.AuthorizationCode = code
  /// 
  ///       ' get user info
  ///       Dim user As UserInfo = twitter.GetUserInfo()
  ///       Console.WriteLine("User ID: {0}", user.UserId)
  ///       Console.WriteLine("Name:    {0}", user.DisplayName)
  ///     Catch ex As Exception
  ///       Console.WriteLine(ex.Message)
  ///     End Try
  ///     Console.ReadKey()
  ///   End Sub
  /// 
  /// End Module
  /// </code>
  /// <para>Result of the program is shown in the images below.</para>
  /// <para><img src="../img/twitter005.png" alt="Log in with Twitter" /></para>
  /// <para><img src="../img/twitter006.png" alt="Access code" /></para>
  /// <para><img src="../img/twitter007.png" alt="User info" /></para>
  /// <h2>ASP .NET WebForms</h2>
  /// <para>In a web projects you can use the <see cref="OAuthManager"/> and <see cref="OAuthWeb"/>.</para>
  /// <para>The following example shows how to use the <see cref="TwitterClient"/> in <b>ASP .NET WebForms</b>.</para>
  /// <para>To test the example, create a new <b>ASP .NET WebForms</b> (empty) project. Add <b>Global.asax</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="TwitterClient"/>.</para>
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
  /// <para><b>NOTE:</b> Do not forget to adjust the <b>Callback URL</b> in the <b>Twitter Application Settings</b>.</para>
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
  public class TwitterClient : OAuthClient
  {

    /// <summary>
    /// Unique provider name: <b>Twitter</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Twitter";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterClient"/>.
    /// </summary>
    /// <param name="consumerKey">The API Key obtained from the <see href="https://apps.twitter.com">Twitter Application Management</see>.</param>
    /// <param name="consumerSecret">The API Secret obtained from the <see href="https://apps.twitter.com">Twitter Application Management</see>.</param>
    public TwitterClient(string consumerKey, string consumerSecret) : base
    (
      "https://api.twitter.com/oauth/request_token",
      "https://api.twitter.com/oauth/authorize",
      "https://api.twitter.com/oauth/access_token",
      consumerKey,
      consumerSecret,
      SignatureMethods.HMACSHA1 // only HMAC-SHA1
    ) { }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      // help: https://dev.twitter.com/rest/reference/get/users/show

      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      string url = "https://api.twitter.com/1.1/users/show.json";

      // query parameters
      var parameters = new HttpParameterCollection();

      parameters.AddUrlParameter("user_id", accessToken["user_id"].ToString());
      parameters.AddUrlParameter("screen_name", accessToken["screen_name"].ToString());
      parameters.AddUrlParameter("include_entities", "false");

      this.Authorization["oauth_token"] = accessToken["oauth_token"].ToString();
      this.Authorization.TokenSecret = accessToken["oauth_token_secret"].ToString();

      // execute the request
      var result = OAuthUtility.Get(url, parameters, this.Authorization);

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id_str", "UserId", typeof(string));
      map.Add("name", "DisplayName");
      map.Add("screen_name", "UserName");
      map.Add("profile_image_url", "Userpic");
      map.Add("url", "Url");
      map.Add("birthday", "Birthday", typeof(DateTime), @"dd\.MM\.yyyy");
      map.Add("lang", "Language");
      //map.Add("verified", "Url");
      //map.Add("location", "Url");

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}