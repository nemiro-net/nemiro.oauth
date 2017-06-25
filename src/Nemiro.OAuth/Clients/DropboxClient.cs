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
  /// OAuth client for <b>Dropbox</b>.
  /// </summary>
  /// <remarks>
  /// <h1>Register and Configure a Dropbox Application</h1>
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
  /// <para>Open the <b><see href="https://www.dropbox.com/developers/apps">Dropbox App Console</see></b> and <b>Create app</b>.</para>
  /// <para>Choose the <b>Dropbox API app</b> type.</para>
  /// <para>
  /// In the application settings  you can found <b>App key</b> and <b>App secret</b>.
  /// Use this for creating an instance of the <see cref="DropboxClient"/> class.
  /// </para>
  /// <code lang="C#">
  /// OAuthManager.RegisterClient
  /// (
  ///   new DropboxClient
  ///   (
  ///     "0le6wsyp3y085wy", 
  ///     "48afwq9yth83y7u"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// OAuthManager.RegisterClient _
  /// (
  ///   New DropboxClient _
  ///   (
  ///     "0le6wsyp3y085wy", 
  ///     "48afwq9yth83y7u"
  ///   )
  /// )
  /// </code>
  /// <para>
  /// For more details, please visit <see href="https://www.dropbox.com/developers">Dropbox Platform Help</see>.
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
  public class DropboxClient : OAuth2Client
  {

    /// <summary>
    /// Unique provider name: <b>Dropbox</b>.
    /// </summary>
    public override string ProviderName
    {
      get
      {
        return "Dropbox";
      }
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DropboxClient"/>.
    /// </summary>
    /// <param name="clientId">The <b>App key</b> obtained from the <see href="https://www.dropbox.com/developers/apps">Dropbox App Console</see>.</param>
    /// <param name="clientSecret">The <b>App secret</b> obtained from the <see href="https://www.dropbox.com/developers/apps">Dropbox App Console</see>.</param>
    public DropboxClient(string clientId, string clientSecret) : base
    (
      "https://www.dropbox.com/oauth2/authorize",
      "https://api.dropboxapi.com/oauth2/token", 
      clientId,
      clientSecret
    ) 
    {
      base.SupportRevokeToken = true;
    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
    /// <returns>
    /// <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
    /// </returns>
    /// <remarks>
    /// <para>For more information, please visit to <see href="https://www.dropbox.com/developers/documentation/http/documentation#users-get_current_account"/>.</para>
    /// </remarks>
    public override UserInfo GetUserInfo(AccessToken accessToken = null)
    {
      accessToken = base.GetSpecifiedTokenOrCurrent(accessToken);

      // execute the request
      var result = OAuthUtility.Post("https://api.dropboxapi.com/2/users/get_current_account", accessToken: accessToken);

      // field mapping
      var map = new ApiDataMapping();

      map.Add("account_id", "UserId", typeof(string));
      map.Add("email", "Email");
      map.Add("profile_photo_url", "Userpic");
      map.Add("locale", "Language");

      map.Add
      (
        "name", "DisplayName",
        delegate (UniValue value)
        {
          if (!value.HasValue) { return null; }
          return value["display_name"].ToString();
        }
      );

      map.Add
      (
        "name", "FirstName",
        delegate (UniValue value)
        {
          if (!value.HasValue) { return null; }
          return value["given_name"].ToString();
        }
      );
      
      map.Add
      (
        "name", "LastName",
        delegate (UniValue value)
        {
          if (!value.HasValue) { return null; }
          return value["surname"].ToString();
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

      return OAuthUtility.Post
      (
        "https://api.dropboxapi.com/2/auth/token/revoke",
        accessToken: accessToken
      );
    }

  }

}