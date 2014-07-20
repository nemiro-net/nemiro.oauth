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

namespace Nemiro.OAuth
{

  /// <summary>
  /// The access token class for OAuth 2.0.
  /// </summary>
  public class OAuth2AccessToken : AccessToken
  {

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
      if (result.ContainsKey("refresh_token") && result["refresh_token"] != null)
      {
        this.RefreshToken = result["refresh_token"].ToString();
      }
      if (result.ContainsKey("scope") && result["scope"] != null)
      {
        this.Scope = result["scope"].ToString();
      }
      if (result.ContainsKey("token_type") && result["token_type"] != null)
      {
        this.TokenType = result["token_type"].ToString();
      }
    }

  }

}
