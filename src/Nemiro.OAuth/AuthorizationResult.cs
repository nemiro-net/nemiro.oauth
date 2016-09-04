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

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents authorization results.
  /// </summary>
  public class AuthorizationResult
  {

    /// <summary>
    /// The ID of the authorization request.
    /// </summary>
    internal string RequestId { get; set; }

    /// <summary>
    /// OAuth version. For example: 1.0, 2.0.
    /// </summary>
    public string ProtocolVersion { get; protected internal set; }

    /// <summary>
    /// Provider and custom client name.
    /// </summary>
    public ClientName ClientName { get; protected internal set; }

    /// <summary>
    /// Provider name. For example: facebook, twitter, google.
    /// </summary>
    public string ProviderName
    {
      get
      {
        return this.ClientName.ProviderName;
      }
    }

    /// <summary>
    /// The access token which is used to query the provider.
    /// </summary>
    public AccessToken AccessToken { get; protected internal set; }

    /// <summary>
    /// The user profile details that is returned from the provider.
    /// </summary>
    public UserInfo UserInfo { get; protected internal set; }

    /// <summary>
    /// Gets a value indicating whether the authorization is successful.
    /// </summary>
    public bool IsSuccessfully
    {
      get
      {
        return this.ErrorInfo == null;
      }
    }

    /// <summary>
    /// Gets error info when the authorization is not successful.
    /// </summary>
    public Exception ErrorInfo { get; protected internal set; }

    /// <summary>
    /// Gets the user ID that is returned from the provider.
    /// </summary>
    public string UserId
    {
      get
      {
        if (this.UserInfo == null) { return null; }
        return this.UserInfo.UserId;
      }
    }

    /// <summary>
    /// Gets the username that is returned from the provider.
    /// </summary>
    public string UserName
    {
      get
      {
        if (this.UserInfo == null) { return null; }
        return this.UserInfo.UserName ?? this.UserId;
      }
    }

    /// <summary>
    /// Gets the access token value.
    /// </summary>
    public string AccessTokenValue
    {
      get
      {
        if (this.AccessToken.GetType() == typeof(OAuthAccessToken))
        {
          return ((OAuthAccessToken)this.AccessToken).Value;
        }
        else if (this.AccessToken.GetType() == typeof(OAuth2AccessToken))
        {
          return ((OAuth2AccessToken)this.AccessToken).Value;
        }
        else
        {
          return null;
        }
      }
    }

    /// <summary>
    /// Gets custom state.
    /// </summary>
    public object State { get; protected internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationResult"/> class.
    /// </summary>
    public AuthorizationResult() { }

  }
}