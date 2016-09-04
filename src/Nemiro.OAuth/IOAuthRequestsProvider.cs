// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2016. All rights reserved.
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
using System.Collections;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Defines a requests provider.
  /// </summary>
  public interface IOAuthRequestsProvider : IEnumerable
  {

    /// <summary>
    /// Adds the specified request to the collection.
    /// </summary>
    /// <param name="key">The unique request key.</param>
    /// <param name="clientName">The client name.</param>
    /// <param name="client">The client instance.</param>
    /// <param name="state">Custom state associated with authorization request.</param>
    void Add(string key, ClientName clientName, OAuthBase client, object state);

    /// <summary>
    /// Determines whether the storage contains an entry with the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the storage.</param>
    bool ContainsKey(string key);

    /// <summary>
    /// Gets the specified request by key.
    /// </summary>
    /// <typeparam name="T">The type based <see cref="OAuthRequest"/>.</typeparam>
    /// <param name="key">The unique request key.</param>
    T Get<T>(string key) where T : OAuthRequest;

    /// <summary>
    /// Gets the specified request by key.
    /// </summary>
    /// <param name="key">The unique request key.</param>
    OAuthRequest Get(string key);

    /// <summary>
    /// Removes the request from collection.
    /// </summary>
    /// <param name="key">The unique request key to remove.</param>
    void Remove(string key);

    /// <summary>
    /// Removes all requests from collection.
    /// </summary>
    void Clear();

  }

}