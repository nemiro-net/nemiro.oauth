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
using System.Collections.Specialized;

// Thanks rkn.gov.ru! You make our life easier :)

namespace Nemiro.OAuth.Clients
{

  /// <summary>
  /// OAuth client for <b>LinkedIn</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a LinkedIn Application</h1>
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
  /// <para>Open and sigin to the <b><see href="https://developer.linkedin.com/">LinkedIn for Developers</see></b>, and <b><see href="https://www.linkedin.com/secure/developer?newapp=">Add new app</see></b>.</para>
  /// <para>
  /// In the application settings  you can found <b>Api Key</b> and <b>Secret Key</b>.
  /// Use this for creating an instance of the <see cref="LinkedInClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new LinkedInClient
  ///   (
  ///     "75vufylz829iim", 
  ///     "VOf14z4T1jie4ezS"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New LinkedInClient _
  ///   (
  ///     "75vufylz829iim", 
  ///     "VOf14z4T1jie4ezS"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://developer.linkedin.com/">LinkedIn for Developers</see>.
  /// </para>
  /// </remarks>
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
  public class LinkedInClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>LinkedIn</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "LinkedIn";
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkedInClient"/>.
    /// </summary>
    /// <param name="clientId">The <b>Api Key</b> obtained from the <see href="https://www.linkedin.com/secure/developer">LinkedIn Dashboard</see>.</param>
    /// <param name="clientSecret">The <b>Secret Key</b> obtained from the <see href="https://www.linkedin.com/secure/developer">LinkedIn Dashboard</see>.</param>
    public LinkedInClient(string clientId, string clientSecret) : base
    (
      "https://www.linkedin.com/oauth/v2/authorization",
      "https://www.linkedin.com/oauth/v2/accessToken",
      clientId,
      clientSecret
    )
    {
      // default scope
      base.ScopeSeparator = " ";
      base.DefaultScope = "r_basicprofile r_emailaddress";
      // for more details please see 
      // https://developer.linkedin.com/docs/oauth2
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      // https://developer.linkedin.com/docs/fields/basic-profile

      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // fix: server does not return token type, but requires Bearer
      accessToken = new OAuth2AccessToken
      (
        accessToken.Value, 
        ((OAuth2AccessToken)accessToken).RefreshToken, 
        AccessTokenType.Bearer
      );

      // execute the request
      var result = OAuthUtility.Get
      (
        endpoint: "https://api.linkedin.com/v1/people/~:(id,first-name,last-name,formatted-name,picture-url,email-address,public-profile-url)",
        accessToken: accessToken,
        headers: new NameValueCollection { { "x-li-format", "json" } }
      );

      // field mapping
      var map = new ApiDataMapping();

      map.Add("id", "UserId", typeof(string));
      map.Add("firstName", "FirstName");
      map.Add("lastName", "LastName");
      map.Add("formattedName", "DisplayName");
      map.Add("pictureUrl", "Userpic");
      map.Add("emailAddress", "Email");
      map.Add("publicProfileUrl", "Url");

            // parse the server response and returns the UserInfo instance
      return new UserInfo(result, map);
    }

  }

}