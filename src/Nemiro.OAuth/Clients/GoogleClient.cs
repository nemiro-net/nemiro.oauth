﻿// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

// If it works, no need to change the code. 
// Just use it! ;-)

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>Google</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Google Application</h1>
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
  /// <para>Open the <b><see href="https://console.developers.google.com">Google Developers Console</see></b> and <b>Create Project</b>.</para>
  /// <para>
  /// <img src="../img/google001.png" alt="Create new project button" />
  /// </para>
  /// <para>Enter the project name and click the <b>Create</b>.</para>
  /// <para>
  /// <img src="../img/google002.png" alt="Create new project form" />
  /// </para>
  /// <para>
  /// Click to the <b>Credential</b> menu in the <b>APIs &amp; OAuth</b>.
  /// </para>
  /// <para>
  /// <img src="../img/google003.png" alt="Credential menu" />
  /// </para>
  /// <para>
  /// For <b>desktop application</b>, click the <b>Create new Client ID</b>, select <b>Installed application</b> and <b>Other</b>.
  /// </para>
  /// <para>
  /// Click the <b>Create Client ID</b> to complete.
  /// </para>
  /// <para>
  /// <img src="../img/google004.png" alt="Create new Client ID for desktop application" />
  /// </para>
  /// <para>
  /// You will get the <b>Client ID</b> and <b>Client Secret</b>.
  /// Use this for creating an instance of the <see cref="GoogleClient"/>.
  /// </para>
  /// <para>
  /// <img src="../img/google005.png" alt="Create new Client ID for desktop application" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new GoogleClient
  ///   (
  ///     "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com", 
  ///     "AeEbEGQqoKgOZb41JUVLvEJL"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New GoogleClient _
  ///   (
  ///     "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com", 
  ///     "AeEbEGQqoKgOZb41JUVLvEJL"
  ///   )
  /// )
  /// </code>
  /// <para>For <b>web projects</b> create another <b>Client ID</b>. In the form select the <b>Web application</b> and specify return addresses.</para>
  /// <para>
  /// <img src="../img/google006.png" alt="Create new Client ID for web application" />
  /// </para>
  /// <para>
  /// For more details, please visit <see href="https://developers.google.com/console/help/new/">Google Developers Console Help</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <para>The following example shows how to use the <see cref="GoogleClient"/> in <b>Console Applications</b>.</para>
  /// <para>For desktop applications, the user will need to manually enter authorization code.</para>
  /// <code lang="C#">
  /// class Program
  /// {
  ///   static void Main(string[] args)
  ///   {
  ///     try
  ///     {
  ///       var google = new GoogleClient
  ///       (
  ///         "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com",
  ///         "AeEbEGQqoKgOZb41JUVLvEJL"
  ///       );
  ///       
  ///       // open the login page in browser
  ///       System.Diagnostics.Process.Start(google.AuthorizationUrl);
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
  ///       google.AuthorizationCode = code;
  ///       // get user info
  ///       var user = google.GetUserInfo();
  ///       Console.WriteLine("User ID: {0}", user.UserId);
  ///       Console.WriteLine("Name:    {0}", user.DisplayName);
  ///       Console.WriteLine("Email:   {0}", user.Email);
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
  /// Module Module1
  /// 
  ///   Sub Main()
  ///     Try
  ///       Dim google As New GoogleClient _
  ///       (
  ///         "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com",
  ///         "AeEbEGQqoKgOZb41JUVLvEJL"
  ///       )
  ///       ' open the login page in browser
  ///       System.Diagnostics.Process.Start(google.AuthorizationUrl)
  /// 
  ///       ' waiting of entering the access code
  ///       Dim code As String = ""
  ///       Do While String.IsNullOrEmpty(code)
  ///         Console.WriteLine("Enter access code:")
  ///         code = Console.ReadLine()
  ///       Loop
  /// 
  ///       ' set authorization code
  ///       google.AuthorizationCode = code
  /// 
  ///       ' get user info
  ///       Dim user As UserInfo = google.GetUserInfo()
  ///       Console.WriteLine("User ID: {0}", user.UserId)
  ///       Console.WriteLine("Name:    {0}", user.DisplayName)
  ///       Console.WriteLine("Email:   {0}", user.Email)
  ///     Catch ex As Exception
  ///       Console.WriteLine(ex.Message)
  ///     End Try
  ///     Console.ReadKey()
  ///   End Sub
  /// 
  /// End Module
  /// </code>
  /// <para>Result of the program is shown in the images below.</para>
  /// <para><img src="../img/google007.png" alt="Log in with Google" /></para>
  /// <para><img src="../img/google008.png" alt="Access code" /></para>
  /// <para><img src="../img/google009.png" alt="User info" /></para>
  /// <para>In a web projects you can use the <see cref="OAuthManager"/> and <see cref="OAuthWeb"/>.</para>
  /// <para>The following example shows how use the <see cref="GoogleClient"/> in <b>ASP .NET MVC Application</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="GoogleClient"/>.</para>
  /// <code lang="C#">
  /// protected void Application_Start()
  /// {
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new GoogleClient
  ///     (
  ///       "1058655871432-fscjqht7ou30a75gjkde1eu1brsvbqkn.apps.googleusercontent.com",
  ///       "SI5bIZkrSB5rO03YF-CdsCJC"
  ///     )
  ///   );
  /// }
  /// </code>
  /// <code lang="VB">
  /// Sub Application_Start()
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New GoogleClient _
  ///     (
  ///       "1058655871432-fscjqht7ou30a75gjkde1eu1brsvbqkn.apps.googleusercontent.com",
  ///       "SI5bIZkrSB5rO03YF-CdsCJC"
  ///     )
  ///   )
  /// End Sub
  /// </code>
  /// <para>The <c>GoogleLoginResult</c> method will handle authorization result.</para>
  /// <code lang="C#">
  /// public ActionResult GoogleLoginResult()
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
  /// Public Function GoogleLoginResult() As ActionResult
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
  /// <para>Add action method for redirection to the <b>Google</b>.</para>
  /// <code lang="C#">
  /// public ActionResult GoogleLogin()
  /// {
  ///   string authUrl = OAuthWeb.GetAuthorizationUrl("Google", Url.Action("GoogleLoginResult", "Home", null, null, Request.Url.Host));
  ///   return Redirect(authUrl);
  /// }
  /// </code>
  /// <code lang="VB">
  /// Public Function GoogleLogin() As ActionResult
  ///   Dim authUrl As String = OAuthWeb.GetAuthorizationUrl("Google", Url.Action("GoogleLoginResult", "Home", Nothing, Nothing, Request.Url.Host))
  ///   Return Redirect(authUrl)
  /// End Function
  /// </code>
  /// <para>On a page add link to the <c>GoogleLogin</c> method.</para>
  /// <code lang="html">
  /// @Html.ActionLink("Log in with Google", "GoogleLogin")
  /// </code>
  /// </example>
  /// <seealso cref="AmazonClient"/>
  /// <seealso cref="FacebookClient"/>
  /// <seealso cref="GitHubClient"/>
  /// <seealso cref="GoogleClient"/>
  /// <seealso cref="LiveClient"/>
  /// <seealso cref="MailRuClient"/>
  /// <seealso cref="OdnoklassnikiClient"/>
  /// <seealso cref="TwitterClient"/>
  /// <seealso cref="VkontakteClient"/>
  /// <seealso cref="YandexClient"/>
  public class GoogleClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Google</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Google";
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
          return "urn:ietf:wg:oauth:2.0:oob";
        }
        return base.ReturnUrl;
      }
      set
      {
        base.ReturnUrl = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleClient"/>.
    /// </summary>
    /// <param name="clientId">The Client ID obtained from the <see href="https://console.developers.google.com/">Google Developers Console</see>.</param>
    /// <param name="clientSecret">The Client Secret obtained from the <see href="https://console.developers.google.com/">Google Developers Console</see>.</param>
    public GoogleClient(string clientId, string clientSecret) : base
    (
      "https://accounts.google.com/o/oauth2/auth",
      "https://accounts.google.com/o/oauth2/token", 
      clientId,
      clientSecret
    ) 
    {
      // visit: https://developers.google.com/+/api/oauth?#login-scopes
      this.Scope = "https://www.googleapis.com/auth/userinfo.email";
    }

      /// <summary>
      /// Gets the user details.
      /// </summary>
      /// <returns>
      /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
      /// </returns>
      public override UserInfo GetUserInfo()
      {
          return GetUserInfo(this.AccessToken);
      }

      /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
      public override UserInfo GetUserInfo(RequestResult accessToken)
    {
      // query parameters
      var parameters = new NameValueCollection
      { 
        { "access_token" , accessToken["access_token"].ToString() }
      };

      // execute the request
      var result = Helpers.ExecuteRequest
      (
        "GET",
        "https://www.googleapis.com/oauth2/v1/userinfo?alt=json",
        parameters,
        null
      );

      // field mapping
      var map = new ApiDataMapping();
      map.Add("id", "UserId", typeof(string));
      map.Add("given_name", "FirstName");
      map.Add("family_name", "LastName");
      map.Add("name", "DisplayName");
      map.Add("email", "Email");
      map.Add("link", "Url");
      map.Add("picture", "Userpic");
      map.Add("birthday", "Birthday", typeof(DateTime), @"MM\/dd\/yyyy");
      map.Add
      (
        "gender", "Sex",
        delegate(object value)
        {
          if (value != null)
          {
            if (value.ToString().Equals("male", StringComparison.OrdinalIgnoreCase))
            {
              return Sex.Male;
            }
            else if (value.ToString().Equals("female", StringComparison.OrdinalIgnoreCase))
            {
              return Sex.Female;
            }
          }
          return Sex.None;
        }
      );

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result.Result as Dictionary<string, object>, map);
    }

  }

}