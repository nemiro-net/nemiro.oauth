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
using System.Net;
using System.IO;
using System.Web;
using Nemiro.OAuth.Extensions;

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

    /// <summary>
    /// The scope of the access request.
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    /// Gets the endpoint of the authorization.
    /// </summary>
    public override string AuthorizationUrl
    {
      get
      {
        string result = this.AuthorizeUrl;
        result += result.Contains("?") ? "&" : "?";
        result += String.Format("client_id={0}&response_type=code", OAuthUtility.UrlEncode(this.ApplicationId));
        result += String.Format("&state={0}", OAuthUtility.UrlEncode(this.State.ToString()));
        if (!String.IsNullOrEmpty(this.Scope))
        {
          result += String.Format("&scope={0}", OAuthUtility.UrlEncode(this.Scope));
        }
        if (!String.IsNullOrEmpty(this.ReturnUrl))
        {
          result += String.Format("&redirect_uri={0}", OAuthUtility.UrlEncode(this.ReturnUrl));
        }
        if (this.Parameters != null && this.Parameters.Count > 0)
        {
          result += "&" + this.Parameters.ToParametersString("&");
        }
        return result;
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
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Gets the access token from the remote server.
    /// </summary>
    protected override void GetAccessToken()
    {
      base.GetAccessToken();

      var parameters = new NameValueCollection
      {
        { "client_id", this.ApplicationId },
        { "client_secret", this.ApplicationSecret },
        { "grant_type", "authorization_code" },
        { "code", this.AuthorizationCode }
      };

      if (!String.IsNullOrEmpty(this.ReturnUrl))
      {
        parameters.Add("redirect_uri", this.ReturnUrl);
      }

      var result = OAuthUtility.ExecuteRequest
      (
        "POST",
        this.AccessTokenUrl,
        parameters
      );

      if (result.ContainsKey("error"))
      {
        this.AccessToken = new ErrorResult(result);
      }
      else
      {
        this.AccessToken = new OAuth2AccessToken(result);
      }
    }

    #endregion

  }

}