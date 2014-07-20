// ----------------------------------------------------------------------------
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
  /// OAuth client for <b>Microsoft Live</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Live Application</h1>
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
  /// <para>Open the <b><see href="https://account.live.com/developers/applications/index">Live Connect App Management</see></b> and <b>Create a New Application</b>.</para>
  /// <para>
  /// <img src="../img/live001.png" alt="Create a New Application" />
  /// </para>
  /// <para>Specify the application name, read terms of use and click the <b>I accept</b>.</para>
  /// <para>
  /// <img src="../img/live002.png" alt="Create a New Application form" />
  /// </para>
  /// <para>Open the <b>App Settings</b> and add return URLs. You can't use localhost.</para>
  /// <para>
  /// <img src="../img/live003.png" alt="Redirect URLs" />
  /// </para>
  /// <para>On the <b>App Settings</b> page, you can found <b>Client ID</b> and <b>Client Secret</b>.</para>
  /// <para>
  /// Use this for creating an instance of the <see cref="LiveClient"/>.
  /// </para>
  /// <para>
  /// <img src="../img/live004.png" alt="Cleint ID and Client Secret" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new LiveClient
  ///   (
  ///     "0000000040124265", 
  ///     "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New LiveClient _
  ///   (
  ///     "0000000040124265", 
  ///     "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit to the <see href="http://msdn.microsoft.com/en-us/library/hh826541.aspx">MSDN</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <para>The following example shows how to add the <b>Microsoft Live OAuth Client</b> to <b>ASP .NET MVC Application</b>.</para>
  /// <para>In the <c>Application_Start</c> event handler (<c>Global.asax</c> file) is registered the <see cref="LiveClient"/>.</para>
  /// <code lang="C#">
  /// protected void Application_Start()
  /// {
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new LiveClient
  ///     (
  ///       "0000000040124265", 
  ///       "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
  ///     )
  ///   );
  /// }
  /// </code>
  /// <code lang="VB">
  /// Sub Application_Start()
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New LiveClient _
  ///     (
  ///       "0000000040124265", 
  ///       "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
  ///     )
  ///   )
  /// End Sub
  /// </code>
  /// <para>The <c>LiveLoginResult</c> method will handle authorization result.</para>
  /// <code lang="C#">
  /// public ActionResult LiveLoginResult()
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
  /// Public Function LiveLoginResult() As ActionResult
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
  /// <para>Add action method for redirection to the <b>Microsoft Live</b>.</para>
  /// <code lang="C#">
  /// public ActionResult LiveLogin()
  /// {
  ///   string authUrl = OAuthWeb.GetAuthorizationUrl("Live", Url.Action("LiveLoginResult", "Home", null, null, Request.Url.Host));
  ///   return Redirect(authUrl);
  /// }
  /// </code>
  /// <code lang="VB">
  /// Public Function LiveLogin() As ActionResult
  ///   Dim authUrl As String = OAuthWeb.GetAuthorizationUrl("Live", Url.Action("LiveLoginResult", "Home", Nothing, Nothing, Request.Url.Host))
  ///   Return Redirect(authUrl)
  /// End Function
  /// </code>
  /// <para>On a page add link to the <c>LiveLogin</c> method.</para>
  /// <code lang="html">
  /// @Html.ActionLink("Log in with Microsoft Live", "LiveLogin")
  /// </code>
  /// <para>Result shown in the images below.</para>
  /// <para><img src="../img/live005.png" alt="Microsoft Live Sign in" /></para>
  /// <para><img src="../img/live006.png" alt="User Info" /></para>
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
  public class LiveClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Live</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Live";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveClient"/>.
    /// </summary>
    /// <param name="clientId">The Client ID obtained from the <see href="https://account.live.com/developers/applications/index">Live Connect App Management</see>.</param>
    /// <param name="clientSecret">The Client Secret obtained from the <see href="https://account.live.com/developers/applications/index">Live Connect App Management</see>.</param>
    public LiveClient(string clientId, string clientSecret) : base
    (
      "https://login.live.com/oauth20_authorize.srf",
      "https://login.live.com/oauth20_token.srf", 
      clientId,
      clientSecret
    ) 
    {
      // http://msdn.microsoft.com/en-us/library/hh243646.aspx
      this.Scope = "wl.basic,wl.emails,wl.birthday,wl.phone_numbers"; 
    }


    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    public override UserInfo GetUserInfo()
    {
      // http://msdn.microsoft.com/en-us/library/hh243648.aspx

      var parameters = new NameValueCollection
      { 
        { "access_token" , this.AccessToken["access_token"].ToString() }
      };

      var result = Helpers.ExecuteRequest
      (
        "GET",
        "https://apis.live.net/v5.0/me",
        parameters,
        null
      );

      var map = new ApiDataMapping();
      map.Add("id", "UserId", typeof(string));
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");
      map.Add("name", "DisplayName");
      map.Add("link", "Url");

      map.Add
      (
        "birth_day",
        "Birthday",
        delegate(object value)
        {
          if (value == null) { return null; }
          return new DateTime
          (
            Convert.ToInt32(result["birth_year"]),
            Convert.ToInt32(result["birth_month"]),
            Convert.ToInt32(result["birth_day"])
          );
        }
      );

      map.Add
      (
        "emails",
        "Email",
        delegate(object value)
        {
          if (value == null || value.GetType() != typeof(Dictionary<string, object>) || !((Dictionary<string, object>)value).ContainsKey("preferred")) { return null; }
          return ((Dictionary<string, object>)value)["preferred"];
        }
      );

      map.Add
      (
        "phones",
        "Phone",
        delegate(object value)
        {
          if (value == null || value.GetType() != typeof(Dictionary<string, object>) || !((Dictionary<string, object>)value).ContainsKey("mobile")) { return null; }
          return ((Dictionary<string, object>)value)["mobile"];
        }
      );

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

      return new UserInfo(result.Result as Dictionary<string, object>, map);
    }

  }
}
