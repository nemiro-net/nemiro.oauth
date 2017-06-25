// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2015, 2017. All rights reserved.
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
using System.Collections.Specialized;
using Nemiro.OAuth.Extensions;
using System.Text;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents base properties and method for OAuth 2.0 client.
  /// </summary>
  /// <remarks>
  /// <para>For more details, please visit <see href="http://tools.ietf.org/html/draft-ietf-oauth-v2-31"/>.</para>
  /// </remarks>
  public abstract class OAuth2Client : OAuthBase
  {

    #region ..fields & properties..

    private readonly Version _Version = new Version(2, 0);

    /// <summary>
    /// Gets version number of the OAuth protocol.
    /// </summary>
    public override Version Version
    {
      get
      {
        return this._Version;
      }
    }

    /// <summary>
    /// The scope of the access request.
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    /// The deault scope.
    /// </summary>
    public string DefaultScope { get; set; }

    /// <summary>
    /// The separator in the scope list.
    /// </summary>
    public string ScopeSeparator { get; set; }

    /// <summary>
    /// Gets or sets grant type.
    /// </summary>
    public GrantType GrantType { get; set; }

    /// <summary>
    /// Gets or sets response type.
    /// </summary>
    public ResponseType ResponseType { get; set; }

    /// <summary>
    /// Gets or sets <b>username</b> if <see cref="GrantType"/> is <b>password</b> or <b>client_credentials</b>.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets <b>password</b> if <see cref="GrantType"/> is <b>password</b> or <b>client_credentials</b>.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets the endpoint of the authorization.
    /// </summary>
    public override string AuthorizationUrl
    {
      get
      {
        var result = new StringBuilder();

        result.Append(this.AuthorizeUrl);
        result.Append(this.AuthorizeUrl.Contains("?") ? "&" : "?");
        result.AppendFormat("client_id={0}&response_type={1}", OAuthUtility.UrlEncode(this.ApplicationId), this.ResponseType);
        result.AppendFormat("&state={0}", OAuthUtility.UrlEncode(this.State));

        // add default scope
        string scope = this.DefaultScope;

        // add custom scope
        if (!String.IsNullOrEmpty(this.Scope))
        {
          var scopeToAdd = new List<string>();

          if (!String.IsNullOrEmpty(scope))
          {
            var scope1 = scope.Split(this.ScopeSeparator.ToCharArray());
            var scope2 = this.Scope.Split(this.ScopeSeparator.ToCharArray());

            foreach (var s in scope2)
            {
              if (!scope1.Any(itm => itm.Equals(s, StringComparison.OrdinalIgnoreCase)))
              {
                scopeToAdd.Add(s);
              }
            }

            if (scopeToAdd.Count > 0) { scope += this.ScopeSeparator; }
          }
          else
          {
            // Fix. Thanks to @codexboise ( https://github.com/codexboise ) // v1.8
            scope = this.Scope;
          }

          scope += String.Join(this.ScopeSeparator, scopeToAdd.ToArray());
        }

        // add scope to url
        if (!String.IsNullOrEmpty(scope))
        {
          result.AppendFormat("&scope={0}", OAuthUtility.UrlEncode(scope));
        }

        // add return url to url
        if (!String.IsNullOrEmpty(this.ReturnUrl))
        {
          result.AppendFormat("&redirect_uri={0}", OAuthUtility.UrlEncode(this.ReturnUrl));
        }

        // other parameters
        if (this.Parameters != null && this.Parameters.Count > 0)
        {
          result.Append("&").Append(this.Parameters.ToParametersString("&"));
        }

        return result.ToString();
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2Client"/> class.
    /// </summary>
    /// <param name="authorizeUrl">The address for login.</param>
    /// <param name="accessTokenUrl">The address for the access token.</param>
    /// <param name="clientId">The application identifier.</param>
    /// <param name="clientSecret">The application secret key.</param>
    public OAuth2Client(string authorizeUrl, string accessTokenUrl, string clientId, string clientSecret) : base(authorizeUrl, accessTokenUrl, clientId, clientSecret) 
    {
      this.GrantType = GrantType.AuthorizationCode;
      this.ResponseType = ResponseType.Code;
    }

    #endregion
    #region ..methods..

    // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

    /// <summary>
    /// Gets the access token from the remote server.
    /// </summary>
    protected override void GetAccessToken()
    {
      // authorization code is required for request
      if (this.GrantType.IsAuthorizationCode && String.IsNullOrEmpty(this.AuthorizationCode))
      {
        throw new ArgumentNullException("AuthorizationCode");
      }
      else if (this.GrantType.IsPassword || this.GrantType.IsClientCredentials)
      {
        if (String.IsNullOrEmpty(this.Username))
        {
          throw new ArgumentNullException("username");
        }

        if (String.IsNullOrEmpty(this.Password))
        {
          throw new ArgumentNullException("password");
        }
      }
      
      // set default access token value
      this.AccessToken = Nemiro.OAuth.AccessToken.Empty;
      
      // set request data
      HttpAuthorization auth = null;
      NameValueCollection parameters = new NameValueCollection();

      if (this.GrantType.IsAuthorizationCode)
      {
        // http://tools.ietf.org/html/rfc6749#section-4.1.3
        parameters.Add("code", this.AuthorizationCode);
      }
      else if (this.GrantType.IsPassword)
      {
        // http://tools.ietf.org/html/rfc6749#section-4.3.2
        parameters.Add("username", this.Username);
        parameters.Add("password", this.Password);
      }
      else if (this.GrantType.IsClientCredentials)
      {
        // http://tools.ietf.org/html/rfc6749#section-4.4.2
        auth = new HttpAuthorization(AuthorizationType.Basic, OAuthUtility.ToBase64String("{0}:{1}", this.Username, this.Password));
      }
      else
      {
        throw new NotSupportedException(String.Format("GrantType '{0}' is not supported. Please write the code ;)", this.GrantType));
      }
      
      parameters.Add("client_id", this.ApplicationId);
      parameters.Add("client_secret", this.ApplicationSecret);
      parameters.Add("grant_type", this.GrantType);
      
      if (!String.IsNullOrEmpty(this.ReturnUrl))
      {
        parameters.Add("redirect_uri", this.ReturnUrl);
      }

      var result = OAuthUtility.Post
      (
        this.AccessTokenUrl,
        parameters,
        auth
      );

      if (result.ContainsKey("error"))
      {
        this.AccessToken = new AccessToken(new ErrorResult(result));
      }
      else
      {
        this.AccessToken = new OAuth2AccessToken(result);
      }
    }

    /// <summary>
    /// Sends a request to refresh the access token.
    /// </summary>
    /// <param name="accessToken">May contain an access token, which should be refreshed.</param>
    /// <remarks>
    /// <para>If <paramref name="accessToken"/> parameter is not specified, it will use the current access token from the same property of the current class instance.</para>
    /// <para>Token must contain the <b>refresh_token</b>, which was received together with the access token.</para>
    /// </remarks>
    /// <exception cref="NotSupportedException">
    /// <para>Provider does not support refreshing the access token, or the method is not implemented.</para>
    /// <para>Use the property <see cref="OAuthBase.SupportRefreshToken"/>, to check the possibility of calling this method.</para>
    /// </exception>
    /// <exception cref="AccessTokenException">
    /// <para>Access token is not found or is not specified.</para>
    /// <para>-or-</para>
    /// <para><b>refresh_token</b> value is empty.</para>
    /// </exception>
    /// <exception cref="RequestException">Error during execution of a web request.</exception>
    public override AccessToken RefreshToken(AccessToken accessToken = null)
    {
      if (!this.SupportRefreshToken)
      {
        throw new NotSupportedException();
      }

      var token = (OAuth2AccessToken)base.GetSpecifiedTokenOrCurrent(accessToken, refreshTokenRequired: true);
   
      //HttpAuthorization authorization = null;
      var parameters = new NameValueCollection
      { 
        { "client_id", this.ApplicationId },
        { "client_secret", this.ApplicationSecret },
        { "grant_type", GrantType.RefreshToken },
        { "refresh_token", token.RefreshToken }
      };

      /*if (!String.IsNullOrEmpty(token.TokenType) && token.TokenType.Equals(AccessTokenType.Bearer, StringComparison.OrdinalIgnoreCase))
      {
        authorization = new HttpAuthorization(AuthorizationType.Bearer, accessToken.Value);
      }
      else
      {
        parameters.Add("access_token", accessToken.Value);
      }*/

      var result = OAuthUtility.Post
      (
        this.AccessTokenUrl,
        parameters: parameters,
        accessToken: token
        //authorization: authorization
      );

      return new OAuth2AccessToken(result);
    }

    #endregion

  }

}