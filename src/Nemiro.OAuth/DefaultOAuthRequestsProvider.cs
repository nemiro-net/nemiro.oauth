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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the default provider requests.
  /// </summary>
  /// <remarks>
  /// <para>Requests are stored in the memory, in the current instance of provider.</para>
  /// <para>Used requests are automatically deleted.</para>
  /// <para>Maximum storage time of unused requests - 20 minutes.</para>
  /// </remarks>
  internal class DefaultOAuthRequestsProvider : IOAuthRequestsProvider
  {

    private Timer Timer = new Timer(60000);

    /// <summary>
    /// Gets the list of active requests.
    /// </summary>
    internal Dictionary<string, OAuthRequest> Requests { get; private set; }

    public DefaultOAuthRequestsProvider()
    {
      this.Requests = new Dictionary<string, OAuthRequest>();
      this.Timer.Elapsed += Timer_Elapsed;
    }

    /// <summary>
    /// The method is called when the interval elapsed.
    /// </summary>
    /// <param name="sender">Instance of the object that raised the event.</param>
    /// <param name="e">The event data.</param>
    private void Timer_Elapsed(object sender, EventArgs e)
    {
      if (this.Requests.Count <= 0)
      {
        // no active requests, stop the time
        this.Timer.Stop();
        return;
      }

      // lifetime request - 20 minutes
      // remove old requests
      var now = DateTime.Now;
      var toRemove = this.Requests.Where(itm2 => now.Subtract(itm2.Value.DateCreated).TotalMinutes >= 20).ToList();

      foreach (var itm in toRemove)
      {
        if (this.Requests.ContainsKey(itm.Key))
        {
          OAuthManager.RemoveRequest(itm.Key);
        }
      }

      // change the status of the timer
      this.Timer.Enabled = (this.Requests.Count > 0);
    }

    /// <summary>
    /// Adds the specified request to the collection.
    /// </summary>
    /// <param name="key">The unique key of request.</param>
    /// <param name="clientName">The client name.</param>
    /// <param name="client">The client instance.</param>
    /// <param name="state">Custom state associated with authorization request.</param>
    public void Add(string key, ClientName clientName, OAuthBase client, object state)
    {
      if (String.IsNullOrEmpty(key))
      {
        throw new ArgumentNullException("key");
      }

      this.Requests.Add(key, new OAuthRequest(clientName, client, state));

      this.Timer.Start();
    }

    /// <summary>
    /// Determines whether the storage contains an entry with the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the storage.</param>
    public bool ContainsKey(string key)
    {
      return this.Requests.ContainsKey(key);
    }

    /// <summary>
    /// Gets the specified request by key.
    /// </summary>
    /// <param name="key">The unique key of request.</param>
    public OAuthRequest Get(string key)
    {
      return this.Get<OAuthRequest>(key);
    }

    /// <summary>
    /// Gets the specified request by key.
    /// </summary>
    /// <typeparam name="T">The type based <see cref="OAuthRequest"/>.</typeparam>
    /// <param name="key">The unique key of request.</param>
    public T Get<T>(string key) where T : OAuthRequest
    {
      if (String.IsNullOrEmpty(key))
      {
        throw new ArgumentNullException("key");
      }

      return (T)this.Requests[key];
    }

    /// <summary>
    /// Removes the request from collection.
    /// </summary>
    /// <param name="key">The key of request to remove.</param>
    public void Remove(string key)
    {
      if (String.IsNullOrEmpty(key))
      {
        throw new ArgumentNullException("key");
      }

      if (this.Requests.ContainsKey(key))
      {
        this.Requests.Remove(key);
      }

      this.Timer.Enabled = (this.Requests.Count > 0);
    }
    
    /// <summary>
    /// Removes all requests from collection.
    /// </summary>
    public void Clear()
    {
      this.Requests.Clear();
      this.Timer.Enabled = false;
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    public IEnumerator GetEnumerator()
    {
      return this.Requests.GetEnumerator();
    }

  }

}