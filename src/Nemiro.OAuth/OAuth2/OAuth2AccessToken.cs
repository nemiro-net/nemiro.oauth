// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2015. All rights reserved.
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
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Security.Permissions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// The access token class for OAuth 2.0.
  /// </summary>
  [Serializable]
  public class OAuth2AccessToken : AccessToken
  {

    #region ..fields & properties..

    // NOTE: Value - for usability

    /// <summary>
    /// The access token issued by the authorization server.
    /// </summary>
    public new string Value
    {
      get
      {
        return base.Value;
      }
      protected set
      {
        base.Value = value;
      }
    }

    /// <summary>
    /// The lifetime in seconds of the access token.
    /// </summary>
    public long ExpiresIn { get; protected set; }

    /// <summary>
    /// The refresh token, which can be used to obtain new
    /// access tokens using the same authorization grant.
    /// </summary>
    public string RefreshToken { get; protected set; }

    /// <summary>
    /// The scope of the access token.
    /// </summary>
    public string Scope { get; protected set; }

    /// <summary>
    /// The type of the token issued. Value is case insensitive.
    /// </summary>
    public string TokenType { get; protected set; }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2AccessToken"/>.
    /// </summary>
    protected OAuth2AccessToken() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2AccessToken"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected OAuth2AccessToken(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }

      this.ExpiresIn = info.GetInt64("ExpiresIn");
      this.RefreshToken = info.GetString("RefreshToken");
      this.Scope = info.GetString("Scope");
      this.TokenType = info.GetString("TokenType");
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2AccessToken"/> class.
    /// </summary>
    /// <param name="result">Result of request to the OAuth server.</param>
    public OAuth2AccessToken(RequestResult result) : base(result)
    {
      this.Value = result["access_token"].ToString();

      if (result.ContainsKey("expires_in"))
      {
        this.ExpiresIn = Convert.ToInt64(result["expires_in"]);
      }

      // todo: think
      /*if (result.ContainsKey("expires"))
      {
        this.ExpiresIn = Convert.ToInt64(result["expires"]);
      }*/

      if (result.ContainsKey("refresh_token") && result["refresh_token"].HasValue)
      {
        this.RefreshToken = result["refresh_token"].ToString();
      }

      if (result.ContainsKey("scope") && result["scope"].HasValue)
      {
        this.Scope = result["scope"].ToString();
      }

      if (result.ContainsKey("token_type") && result["token_type"].HasValue)
      {
        this.TokenType = result["token_type"].ToString();
      }
    }
        /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2AccessToken"/> class with a specified access token and refresh token.
    /// </summary>
    /// <param name="accessTolen">The access token.</param>
    /// <param name="refreshToken">The refresh token.</param>
    public OAuth2AccessToken(string accessTolen, string refreshToken) : this(accessTolen, refreshToken, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2AccessToken"/> class with a specified access token and refresh token.
    /// </summary>
    /// <param name="accessTolen">The access token.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="tokenType">The token type. For example: bearer. Default: null.</param>
    public OAuth2AccessToken(string accessTolen, string refreshToken, string tokenType) : base()
    {
      this["access_token"] = this.Value = accessTolen;

      if (!String.IsNullOrEmpty(refreshToken))
      {
        this["refresh_token"] = this.RefreshToken = refreshToken;
      }

      if (!String.IsNullOrEmpty(tokenType))
      {
        this["token_type"] = this.TokenType = tokenType;
      }
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }

      info.AddValue("ExpiresIn", this.ExpiresIn);
      info.AddValue("RefreshToken", this.RefreshToken);
      info.AddValue("Scope", this.Scope);
      info.AddValue("TokenType", this.TokenType);

      base.GetObjectData(info, context);
    }

    /// <summary>
    /// Returns the <see cref="Value"/>.
    /// </summary>
    public override string ToString()
    {
      return this.Value;
    }

    /// <summary>
    /// Converts the specified string to an <see cref="OAuth2AccessToken"/>.
    /// </summary>
    /// <param name="value">A string containing an access token to parse.</param>
    /// <returns>A new <see cref="OAuth2AccessToken"/> instance.</returns>
    public new static OAuth2AccessToken Parse(string value)
    {
      return AccessToken.Parse<OAuth2AccessToken>(value);
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Converts the <see cref="OAuth2AccessToken"/> to <see cref="System.String"/>.
    /// </summary>
    /// <param name="v">The <see cref="OAuth2AccessToken"/> instance.</param>
    public static implicit operator string(OAuth2AccessToken v)
    {
      return v.Value;
    }

    /// <summary>
    /// Creates a new <see cref="OAuthAccessToken"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="OAuth2AccessToken"/>.</param>
    public static implicit operator OAuth2AccessToken(string value)
    {
      return new OAuth2AccessToken(new RequestResult("text/plain", Encoding.UTF8.GetBytes(String.Format("access_token={0}", value))));
    }

    #endregion

  }

}