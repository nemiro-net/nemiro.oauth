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

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the request item to OAuth server.
  /// </summary>
  internal class OAuthRequest
  {

    private ClientName _ClientName = null;

    /// <summary>
    /// Gets name of the client.
    /// </summary>
    public ClientName ClientName
    {
      get
      {
        return _ClientName;
      }
    }

    private OAuthBase _Client = null;

    /// <summary>
    /// Gets instance of the OAuth client.
    /// </summary>
    public OAuthBase Client
    {
      get
      {
        return _Client;
      }
    }

    private DateTime _DateCreated = DateTime.Now;

    /// <summary>
    /// Gets date and time creation of the request.
    /// </summary>
    public DateTime DateCreated
    {
      get
      {
        return _DateCreated;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthRequest"/> class.
    /// </summary>
    /// <param name="client">The instance of the OAuth client.</param>
    /// <param name="clientName">The client name.</param>
    public OAuthRequest(ClientName clientName, OAuthBase client)
    {
      /* is is not possible
      if (client == null)
      {
        throw new ArgumentNullException("client");
      }
      */

      if (String.IsNullOrEmpty(clientName))
      {
        clientName = client.ProviderName;
      }

      _ClientName = clientName;
      _Client = client;
    }

  }

}