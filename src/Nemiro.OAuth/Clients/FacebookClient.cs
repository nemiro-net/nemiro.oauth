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
  /// OAuth client for <b>Facebook</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Facebook Application</h1>
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
  /// You need to <b><see href="https://developers.facebook.com">register as developer</see></b>.
  /// </para>
  /// <para>Open the <b><see href="https://developers.facebook.com">Facebook Developers</see></b> and <b>Create a New App</b>.</para>
  /// <para>
  /// <img src="../img/facebook001.png" alt="Create new application menu" />
  /// </para>
  /// <para>Specify the application name and click the <b>Create App</b>.</para>
  /// <para>
  /// <img src="../img/facebook002.png" alt="Create new application form" />
  /// </para>
  /// <para>
  /// In the application dashboard you can found <b>App ID</b> and <b>App Secret</b>, this is <b>Client ID</b> and <b>Client Secret</b>.
  /// Use this for creating an instance of the <see cref="FacebookClient"/> class.
  /// </para>
  /// <para>
  /// <img src="../img/facebook003.png" alt="App ID and App Secret" />
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new FacebookClient
  ///   (
  ///     "1435890426686808", 
  ///     "c6057dfae399beee9e8dc46a4182e8fd"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New FacebookClient _
  ///   (
  ///     "1435890426686808", 
  ///     "c6057dfae399beee9e8dc46a4182e8fd"
  ///   )
  /// )
  /// </code>
  /// <para>You can use the <b>App ID</b> and <b>App Secret</b> for desktop, mobile and web projects.</para>
  /// <h2>The application availability</h2>
  /// <para>
  /// To manage the status of the application, you must provide contact information.
  /// </para>
  /// <para>
  /// Enter a contact email on the <b>Settings</b> page.
  /// </para>
  /// <para>
  /// <img src="../img/facebook004.png" alt="Contact Email" />
  /// </para>
  /// <para>
  /// And now, you can change availability status of the application on the <b>Status &amp; Review</b> page.
  /// </para>
  /// <para>
  /// <img src="../img/facebook005.png" alt="Make App Public" />
  /// </para>
  /// <h2>Configure application for web projects</h2>
  /// <para>For web projects, configure <b>return URLs</b>.</para>
  /// <para>
  /// Open the application <b>Settings</b> and click <b>Advanced</b> tab.
  /// </para>
  /// <para>
  /// <img src="../img/facebook006.png" alt="Advanced tab" />
  /// </para>
  /// <para>
  /// You must add the return URLs to the <b>Valid OAuth redirect URIs</b> field of the <b>Security</b> section.
  /// </para>
  /// <para>Do not forget to save your changes.</para>
  /// <para><b>NOTE:</b> If the application will be used for web and desktop, then also add URL: <c>https://www.facebook.com/connect/login_success.html</c>.</para>
  /// <para><b>NOTE:</b> Enable <b>Client OAuth Login</b> if it's disabled.</para>
  /// <para>
  /// <img src="../img/facebook007.png" alt="Valid OAuth redirect URIs" />
  /// </para>
  /// <para>
  /// For more details, please see <see href="https://developers.facebook.com/docs/">Facebook Developer Documentation</see>.
  /// </para>
  /// </remarks>
  /// <example>
  /// <para>The following example shows how to use the <see cref="FacebookClient"/> in desktop applications.</para>
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
  ///   // facebook client registration
  ///   OAuthManager.RegisterClient
  ///   (
  ///     new FacebookClient
  ///     (
  ///       // app id
  ///       "1435890426686808", 
  ///       // app secret
  ///       "c6057dfae399beee9e8dc46a4182e8fd"
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
  ///   ' facebook client registration
  ///   OAuthManager.RegisterClient _
  ///   (
  ///     New FacebookClient _
  ///     (
  ///       "1435890426686808",
  ///       "c6057dfae399beee9e8dc46a4182e8fd"
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
  /// <para>Insert a <c>WebBrowser</c> to the <c>Form2</c>.</para>
  /// <code lang="C#">
  /// public Form2()
  /// {
  ///   InitializeComponent();
  ///   webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
  ///   webBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("facebook"));
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
  ///   WebBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl("facebook"))
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
  /// <para><img src="../img/facebook008.png" alt="Log in with Facebook" /></para>
  /// <para><img src="../img/facebook009.png" alt="Facebook User Info" /></para>
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
  public class FacebookClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Facebook</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Facebook";
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
          return "https://www.facebook.com/connect/login_success.html";
        }

        return base.ReturnUrl;
      }
      set
      {
        base.ReturnUrl = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FacebookClient"/>.
    /// </summary>
    /// <param name="clientId">The App ID obtained from the <see href="https://developers.facebook.com/apps/">Facebook Developers</see>.</param>
    /// <param name="clientSecret">The App Secret obtained from the <see href="https://developers.facebook.com/apps/">Facebook Developers</see>.</param>
    public FacebookClient(string clientId, string clientSecret) : base
    (
      "https://www.facebook.com/dialog/oauth",
      "https://graph.facebook.com/oauth/access_token",
      clientId,
      clientSecret
    )
    {
      // scope list
      base.ScopeSeparator = ",";
      base.DefaultScope = "public_profile,email"; //,user_website,user_birthday
      // features for access token
      base.SupportRefreshToken = true;
      base.SupportRevokeToken = true;
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <remarks>
    /// <para>
    /// For more details, please see <see href="https://developers.facebook.com/docs/graph-api/reference/v2.7/user">User</see> method in <b>Guide of Facebook Graph API</b>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // execute the request
      var result = OAuthUtility.Get("https://graph.facebook.com/v2.9/me?fields=id,name,first_name,last_name,email,birthday,link,gender,languages", accessToken: accessToken);

      // field mapping
      var map = new ApiDataMapping();
      map.Add("id", "UserId", typeof(string));
      map.Add("first_name", "FirstName");
      map.Add("last_name", "LastName");
      map.Add("name", "DisplayName");
      map.Add("email", "Email");
      map.Add("link", "Url"); // website
      map.Add("birthday", "Birthday", typeof(DateTime), @"MM\/dd\/yyyy");
      map.Add
      (
        "gender", "Sex",
        delegate(UniValue value)
        {
          if (value.HasValue)
          {
            if (value.Equals("male", StringComparison.OrdinalIgnoreCase))
            {
              return Sex.Male;
            }
            else if (value.Equals("female", StringComparison.OrdinalIgnoreCase))
            {
              return Sex.Female;
            }
          }
          return Sex.None;
        }
      );
      map.Add
      (
        "languages", "Language",
        delegate(UniValue value)
        {
          if (value.HasValue && value.Count > 0)
          {
            return value[0]["name"].ToString();
          }

          return null;
        }
      );

      // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }
    
    /// <summary>
    /// Sends a request to revoke the access token.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which should be revoked.</param>
    /// <exception cref="NotSupportedException">
    /// <para>Provider does not support revoking the access token, or the method is not implemented.</para>
    /// <para>Use the property <see cref="OAuthBase.SupportRevokeToken"/>, to check the possibility of calling this method.</para>
    /// </exception>
    /// <remarks>
    /// <para>If <paramref name="accessToken"/> parameter is not specified, it will use the current access token from the same property of the current class instance.</para>
    /// </remarks>
    public override RequestResult RevokeToken(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      return OAuthUtility.Delete
      (
        "https://graph.facebook.com/v2.9/me/permissions", 
        new NameValueCollection
        { 
          { "access_token", accessToken.Value }
        }
      );
    }

    /// <summary>
    /// Sends a request to refresh the access token.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which should be refreshed.</param>
    /// <exception cref="NotSupportedException">
    /// <para>Provider does not support refreshing the access token, or the method is not implemented.</para>
    /// <para>Use the property <see cref="OAuthBase.SupportRefreshToken"/>, to check the possibility of calling this method.</para>
    /// </exception>
    /// <remarks>
    /// <para>If <paramref name="accessToken"/> parameter is not specified, it will use the current access token from the same property of the current class instance.</para>
    /// </remarks>
    public override AccessToken RefreshToken(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken, refreshTokenRequired: false);

      var result = OAuthUtility.Post
      (
        "https://graph.facebook.com/oauth/access_token",
        new NameValueCollection
        { 
          { "client_id", this.ApplicationId },
          { "client_secret", this.ApplicationSecret },
          { "grant_type", "fb_exchange_token" },
          { "fb_exchange_token", accessToken.Value }
        }
      );

      return new OAuth2AccessToken(result);
    }
    
  }

}