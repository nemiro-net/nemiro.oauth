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
using System.Collections.Specialized;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents base properties and method for OAuth 1.0 client.
  /// </summary>
  /// <remarks>
  /// <para>For more details, please visit <see href="http://tools.ietf.org/html/rfc5849"/>.</para>
  /// </remarks>
  public abstract class OAuthClient : OAuthBase
  {

    #region ..fields & properties..

    private readonly Version _Version = new Version(1, 0);

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
    /// Gets or sets the address for the request token.
    /// </summary>
    protected string RequestTokenUrl { get; set; }

    private OAuthAuthorization _Authorization = new OAuthAuthorization();

    /// <summary>
    /// Get the authorization parameters.
    /// </summary>
    public OAuthAuthorization Authorization
    {
      get
      {
        return _Authorization;
      }
    }

    /// <summary>
    /// Gets the endpoint of the authorization.
    /// </summary>
    public override string AuthorizationUrl
    {
      get
      {
        return this.RequestToken.AuthorizationUrl;
      }
    }

    private OAuthRequestToken _RequestToken = null;

    /// <summary>
    /// Gets or sets the request token.
    /// </summary>
    public OAuthRequestToken RequestToken
    {
      get
      {
        if (_RequestToken == null)
        {
          this.GetRequestToken();
        }

        return _RequestToken;
      }
      protected internal set
      {
        _RequestToken = value;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthClient"/> class.
    /// </summary>
    /// <param name="requestTokenUrl">The address for the request token.</param>
    /// <param name="authorizeUrl">The address for login.</param>
    /// <param name="accessTokenUrl">The address for the access token.</param>
    /// <param name="consumerKey">The application identifier.</param>
    /// <param name="consumerSecret">The application secret key.</param>
    /// <param name="signatureMethod">The name of hashing algorithm to calculate the signature: HMAC-SHA1 (default) or PLAINTEXT.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="requestTokenUrl"/> is null or empty.</exception>
    public OAuthClient(string requestTokenUrl, string authorizeUrl, string accessTokenUrl, string consumerKey, string consumerSecret, string signatureMethod = SignatureMethods.HMACSHA1) : base(authorizeUrl, accessTokenUrl, consumerKey, consumerSecret)
    {
      if (String.IsNullOrEmpty(requestTokenUrl)) { throw new ArgumentNullException("requestTokenUrl"); }

      this.RequestTokenUrl = requestTokenUrl;

      this.Authorization.ConsumerSecret = consumerSecret;
      this.Authorization.ConsumerKey = consumerKey;
      this.Authorization.SignatureMethod = signatureMethod;
      this.Authorization.Signature = "";
      this.Authorization.Token = "";
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Gets base string of the signature for current request.
    /// </summary>
    /// <remarks><para>For more details, please visit <see href="http://tools.ietf.org/html/rfc5849#section-3.4.1.1"/></para></remarks>
    [Obsolete("Use OAuthUtility. // v1.4", false)]
    private string GetSignatureBaseString(string httpMethod, Uri url, NameValueCollection parameters)
    {
      return OAuthUtility.GetSignatureBaseString(httpMethod, url, parameters, this.Authorization);
    }

    /// <summary>
    /// Gets signature for the current request.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="tokenSecret">The token secret.</param>
    /// <param name="parameters">The query parameters.</param>
    [Obsolete("Use OAuthUtility. // v1.4", false)]
    public OAuthSignature GetSignature(string httpMethod, Uri url, string tokenSecret, NameValueCollection parameters)
    {
      return OAuthUtility.GetSignature(httpMethod, url, this.ApplicationSecret, tokenSecret, parameters, this.Authorization);
    }

    /// <summary>
    /// Gets the request token from the remote server.
    /// </summary>
    public virtual void GetRequestToken()
    {
      this.Authorization.PrepareForRequestToken();

      if (!String.IsNullOrEmpty(this.ReturnUrl))
      {
        this.Authorization.Callback = String.Format
        (
          "{0}{1}state={2}",
          this.ReturnUrl,
          (this.ReturnUrl.Contains("?") ? "&" : "?"),
          this.State
        );
      }

      _RequestToken = new OAuthRequestToken
      (
        OAuthUtility.Post
        (
          this.RequestTokenUrl,
          authorization: this.Authorization,
          headers: new NameValueCollection { { "Accept", "text/plain" } } // application/x-www-form-urlencoded ???
        ),
        this.AuthorizeUrl,
        this.Parameters
      );
    }

    /// <summary>
    /// Gets the access token from the remote server.
    /// </summary>
    protected override void GetAccessToken()
    {
      // authorization code is required for request
      if (String.IsNullOrEmpty(this.AuthorizationCode))
      {
        throw new ArgumentNullException("AuthorizationCode");
      }

      // set default access tken value
      this.AccessToken = Nemiro.OAuth.AccessToken.Empty;

      // prepare
      this.Authorization.PrepareForAccessToken();

      // set request data
      this.Authorization.Verifier = this.AuthorizationCode;
      this.Authorization.Token = this.RequestToken.OAuthToken;
      this.Authorization.TokenSecret = this.RequestToken.OAuthTokenSecret;

      // send request
      base.AccessToken = new OAuthAccessToken
      (
        OAuthUtility.Post
        (
          this.AccessTokenUrl,
          authorization: this.Authorization,
          headers: new NameValueCollection { { "Accept", "text/plain" } } // application/x-www-form-urlencoded ???
        )
      );

      // remove oauth_verifier from headers
      this.Authorization.Remove("oauth_verifier");
    }

    #endregion

  }

}