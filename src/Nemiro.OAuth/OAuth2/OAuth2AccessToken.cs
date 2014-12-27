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
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Nemiro.OAuth
{

  /// <summary>
  /// The access token class for OAuth 2.0.
  /// </summary>
	[Serializable]
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
    /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
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

  }

}
