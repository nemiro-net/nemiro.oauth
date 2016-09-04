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
  /// Represents the request item to OAuth server.
  /// </summary>
  public class OAuthRequest
  {

    /// <summary>
    /// Gets name of the client.
    /// </summary>
    public ClientName ClientName { get; private set; }

    /// <summary>
    /// Gets instance of the OAuth client.
    /// </summary>
    public OAuthBase Client { get; private set; }

    /// <summary>
    /// Gets date and time creation of the request.
    /// </summary>
    public DateTime DateCreated { get; private set; }

    /// <summary>
    /// Gets custom state.
    /// </summary>
    public object State { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthRequest"/> class.
    /// </summary>
    /// <param name="client">The instance of the OAuth client.</param>
    public OAuthRequest(OAuthBase client) : this (null, client, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthRequest"/> class.
    /// </summary>
    /// <param name="client">The instance of the OAuth client.</param>
    /// <param name="clientName">The client name.</param>
    public OAuthRequest(ClientName clientName, OAuthBase client) : this (clientName, client, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthRequest"/> class.
    /// </summary>
    /// <param name="client">The instance of the OAuth client.</param>
    /// <param name="clientName">The client name.</param>
    /// <param name="state">Custom state associated with the request.</param>
    public OAuthRequest(ClientName clientName, OAuthBase client, object state)
    {
      if (client == null)
      {
        throw new ArgumentNullException("client");
      }

      if (String.IsNullOrEmpty(clientName))
      {
        clientName = client.ProviderName;
      }

      this.ClientName = clientName;
      this.Client = client;
      this.DateCreated = DateTime.Now;
      this.State = state;
    }

  }

}