// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014-2015. All rights reserved.
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
using System.Web;
using System.Collections.Specialized;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents base class for OAuth client classes.
  /// </summary>
  public abstract class OAuthBase : ICloneable
  {

    #region ..fields & properties..

    /// <summary>
    /// Unique provider name.
    /// </summary>
    /// <remarks>
    /// <para>Client classes are required to implement this property.</para>
    /// <para><b>The provider name must be unique.</b></para>
    /// </remarks>
    /// <example>
    /// <code lang="C#">
    /// public override string ProviderName
    /// {
    ///   get
    ///   {
    ///     return "KGB";
    ///   }
    /// }
    /// </code>
    /// </example>
    public abstract string ProviderName { get; }

    /// <summary>
    /// Gets the endpoint of the authorization.
    /// </summary>
    /// <remarks>
    /// <para>This property is implemented at the protocol level (<see cref="OAuthClient"/> &amp; <see cref="OAuth2Client"/>).</para>
    /// </remarks>
    public virtual string AuthorizationUrl
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    private RequestResult _AccessToken = null;

    /// <summary>
    /// Gets or sets an access token.
    /// </summary>
    public virtual RequestResult AccessToken
    {
      get
      {
        if (_AccessToken == null)
        {
          this.GetAccessToken();
        }
        return _AccessToken;
      }
      protected set
      {
        _AccessToken = value;
      }
    }

    /// <summary>
    /// Gets an access token value.
    /// </summary>
    public virtual string AccessTokenValue
    {
      get
      {
        if (this.AccessToken.GetType().IsSubclassOf(typeof(AccessToken)))
        {
          return ((AccessToken)this.AccessToken).Value;
        }
        return null;
      }
    }

    private string _AuthorizationCode = null;

    /// <summary>
    /// Gets or sets access code for access token requests.
    /// </summary>
    public virtual string AuthorizationCode
    {
      get
      {
        return _AuthorizationCode;
      }
      set
      {
        if (_AuthorizationCode != value)
        {
          _AccessToken = null;
        }
        _AuthorizationCode = value;
      }
    }

    /// <summary>
    /// Gets or sets unique request identifier.
    /// For clients the value sets is automatically.
    /// </summary>
    public virtual string State { get; protected set; }

    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    public string ApplicationId { get; protected set; }

    /// <summary>
    /// Gets or sets the application secret key.
    /// </summary>
    public string ApplicationSecret { get; protected set; }

    /// <summary>
    /// Gets or sets the base address for login.
    /// </summary>
    protected string AuthorizeUrl { get; set; }

    /// <summary>
    /// Gets or sets the address for the access token.
    /// </summary>
    protected string AccessTokenUrl { get; set; }

    /// <summary>
    /// Gets the version of the OAuth protocol.
    /// </summary>
    public string Version
    {
      get
      {
        if (this.GetType().IsSubclassOf(typeof(OAuthClient)))
        {
          return "1.0";
        }
        else if (this.GetType().IsSubclassOf(typeof(OAuth2Client)))
        {
          return "2.0";
        }
        else
        {
          return "0.0";
        }
      }
    }

    private string _ReturnUrl = "";

    /// <summary>
    /// Gets or sets return URL.
    /// </summary>
    /// <remarks>
    /// <para>At this address provider will return the user authorization results.</para>
    /// <para>For many providers are needed configuration of the application on the provider website.</para>
    /// </remarks>
    public virtual string ReturnUrl
    {
      get
      {
        return _ReturnUrl;
      }
      set
      {
        if (_ReturnUrl != value)
        {
          this.AuthorizationCode = "";
        }
        _ReturnUrl = value;
      }
    }

    private NameValueCollection _Parameters = new NameValueCollection();

    /// <summary>
    /// Gets or sets additional query parameters.
    /// </summary>
    /// <remarks>
    /// <para>These parameters will be transferred to the provider.</para>
    /// </remarks>
    public NameValueCollection Parameters
    {
      get
      {
        return _Parameters;
      }
      set
      {
        _Parameters = value;
      }
    }

    // Is a revoke token supported for this client.
    public virtual bool SupportRevokeToken
    {
      get
      {
        return false;
      }
    }

    public virtual bool SupportRefreshToken
    {
      get
      {
        return false;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthBase"/> class.
    /// </summary>
    /// <param name="authorizeUrl">The address for login.</param>
    /// <param name="accessTokenUrl">The address for the access token.</param>
    /// <param name="applicationId">The application identifier obtained from the provider website.</param>
    /// <param name="applicationSecret">The application secret key obtained from the provider website.</param>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="authorizeUrl"/> is <b>null</b> or <b>empty</b>.</para>
    /// <para>-or-</para>
    /// <para><paramref name="accessTokenUrl"/> is <b>null</b> or <b>empty</b>.</para>
    /// <para>-or-</para>
    /// <para><paramref name="applicationId"/> is <b>null</b> or <b>empty</b>.</para>
    /// <para>-or-</para>
    /// <para><paramref name="applicationSecret"/> is <b>null</b> or <b>empty</b>.</para>
    /// </exception>
    public OAuthBase(string authorizeUrl, string accessTokenUrl, string applicationId, string applicationSecret)
    {
      if (String.IsNullOrEmpty(authorizeUrl)) { throw new ArgumentNullException("authorizeUrl"); }
      if (String.IsNullOrEmpty(accessTokenUrl)) { throw new ArgumentNullException("accessTokenUrl"); }
      if (String.IsNullOrEmpty(applicationId)) { throw new ArgumentNullException("applicationId"); }
      if (String.IsNullOrEmpty(applicationSecret)) { throw new ArgumentNullException("applicationSecret"); }

      this.AuthorizeUrl = authorizeUrl;
      this.AccessTokenUrl = accessTokenUrl;
      this.ApplicationId = applicationId;
      this.ApplicationSecret = applicationSecret;

      // set unique identifier to the instance
      this.State = OAuthUtility.GetRandomKey();

      // I do not remember, why. I'll try to change it. // v1.8 
      // add the instance to the clients collection
      // OAuthManager.AddRequest(this.State, this.ProviderName, this);
      // --
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Redirects a client to the Authorization URL.
    /// </summary>
    /// <remarks>
    /// <para>Use this method only for web applications (<b>ASP .NET</b>).</para>
    /// </remarks>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public virtual void RedirectToAuthorization()
    {
      if (HttpContext.Current == null)
      {
        throw new NullHttpContextException();
      }
      HttpContext.Current.Response.Redirect(this.AuthorizationUrl);
    }

    /// <summary>
    /// Gets the access token from the remote server.
    /// </summary>
    /// <remarks>
    /// <para>This is method is implemented at the protocol level (<see cref="OAuthClient"/> &amp; <see cref="OAuth2Client"/>).</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">The <see cref="AuthorizationCode"/> is <b>null</b> or <b>empty</b>.</exception>
    protected virtual void GetAccessToken() { }

    public virtual RequestResult RevokeToken(string accessToken = null) 
    {
      throw new NotSupportedException();
    }

    public virtual AccessToken RefreshToken(string accessToken = null)
    {
      throw new NotSupportedException();
    }

    /// <summary>
    /// Gets the user details via API of the provider.
    /// </summary>
    /// <remarks>
    /// <para>This is method is implemented at the <see cref="Nemiro.OAuth.Clients">client</see> level.</para>
    /// </remarks>
    public abstract UserInfo GetUserInfo();

    /// <summary>
    /// Creates a shallow copy of the current object.
    /// </summary>
    /// <param name="parameters">The query parameters for new copy object.</param>
    /// <param name="returnUrl">The new return URL for new copy object.</param>
    /// <returns>A shallow copy of the current object.</returns>
    /// <remarks>
    /// <para>Method creates a copy of the current object, removes tokens, change the return address, query parameters and state.</para>
    /// <para>Unfortunately, I made a mistake in architecture, so I had to make this method.</para>
    /// </remarks>
    /// <seealso cref="Clone()"/>
    public OAuthBase Clone(NameValueCollection parameters, string returnUrl)
    {
      OAuthBase result = this.Clone() as OAuthBase;
      if (returnUrl != null) { result.ReturnUrl = returnUrl; }
      if (parameters != null) { result.Parameters = parameters; }
      return result;
    }

    /// <summary>
    /// Creates a shallow copy of the current object.
    /// </summary>
    /// <returns>A shallow copy of the current object.</returns>
    /// <remarks>
    /// <para>Method creates a copy of the current object, removes tokens, change the return address, query parameters and state.</para>
    /// <para>Unfortunately, I made a mistake in architecture, so I had to make this method.</para>
    /// </remarks>
    /// <seealso cref="Clone(NameValueCollection, string)"/>
    public object Clone()
    {
      OAuthBase result = this.MemberwiseClone() as OAuthBase;

      result.State = OAuthUtility.GetRandomKey();
      result.AccessToken = null;
      result.AuthorizationCode = null;

      if (result.GetType().IsSubclassOf(typeof(OAuthClient)))
      {
        ((OAuthClient)result).RequestToken = null;
      }

      // I do not remember, why. I'll try to change it. // v1.8 
      // OAuthManager.AddRequest(result.State, result.ProviderName, result);
      // --

      return result;
    }

    internal protected string GetSpecifiedTokenOrCurrent(string accessToken)
    {
      if (!String.IsNullOrEmpty(accessToken))
      {
        return accessToken;
      }

      if (this.AccessToken.GetType() == typeof(AccessToken))
      {
        return ((AccessToken)this.AccessToken).Value;
      }

      // todo: other exception
      throw new ArgumentNullException("accessToken");
    }

    #endregion

  }

}