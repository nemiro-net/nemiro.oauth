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

namespace Nemiro.OAuth
{

  /// <summary>
  /// The access token class for OAuth 1.0.
  /// </summary>
  public class OAuthAccessToken : AccessToken
  {

    /// <summary>
    /// The access token issued by the authorization server.
    /// </summary>
    public string TokenSecret { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAccessToken"/> class.
    /// </summary>
    /// <param name="result">Result of request to the OAuth server.</param>
    public OAuthAccessToken(RequestResult result) : base(result) 
    {
      this.Value = result["oauth_token"].ToString();
      this.TokenSecret = result["oauth_token_secret"].ToString();
    }

  }

}
