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
using System.Collections;
using System.Timers;

namespace Nemiro.OAuth
{

    /// <summary>
    /// Represents helper class for management of OAuth clients.
    /// </summary>
    /// <example>
    /// <para>You can use this class to register clients in your project.</para>
    /// <code lang="C#">
    /// OAuthManager.RegisterClient
    /// (
    ///   new FacebookClient
    ///   (
    ///     "1435890426686808", 
    ///     "c6057dfae399beee9e8dc46a4182e8fd"
    ///   )
    /// );
    /// </code>
    /// <code lang="VB">
    /// OAuthManager.RegisterClient _
    /// (
    ///   New FacebookClient _
    ///   (
    ///     "1435890426686808", 
    ///     "c6057dfae399beee9e8dc46a4182e8fd"
    ///   )
    /// )
    /// </code>
    /// </example>
    public static class OAuthManager
    {

        #region ..fields & properties..
        
        private static IRequestCache _requestCache;

        private static Dictionary<string, OAuthBase> _RegisteredClients = new Dictionary<string, OAuthBase>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the list of the registered clients.
        /// </summary>
        public static Dictionary<string, OAuthBase> RegisteredClients
        {
            get
            {
                return _RegisteredClients;
            }
        }

        #endregion
        #region ..constructor..

        /// <summary>
        /// Initializes the <see cref="OAuthManager"/>.
        /// </summary>
        static OAuthManager()
        {
            _requestCache = new RequestMemoryCache();
            _requestCache.Initialise("Nemiro.OAuth");
        }

        #endregion
        #region ..methods..
        
        /// <summary>
        /// Adds the specified request to the collection.
        /// </summary>
        /// <param name="key">The unique request key.</param>
        /// <param name="client">The client instance.</param>
        internal static void AddRequet(string key, OAuthBase client)
        {
            _requestCache.Set(key, new OAuthRequest(client), TimeSpan.FromMinutes(20));
        }

        /// <summary>
        /// Removes the request from collection.
        /// </summary>
        /// <param name="key">The unique request key to remove..</param>
        internal static void RemoveRequet(string key)
        {
            _requestCache.Remove(key);
        }

        /// <summary>
        /// Gets a request from collection
        /// </summary>
        /// <param name="key">The unique request key to get</param>
        /// <returns></returns>
        internal static OAuthRequest GetRequest(string key)
        {
            if (_requestCache.Exists(key))
            {
                return _requestCache.Get<OAuthRequest>(key);
            }
            return null;
        }

        /// <summary>
        /// Registers the specified client in the application.
        /// </summary>
        /// <param name="client">The client instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="client"/> is <b>null</b> or <b>empty</b>.</exception>
        /// <exception cref="DuplicateProviderException">If you attempt to register the already registered client.</exception>
        /// <example>
        /// <code lang="C#">
        /// OAuthManager.RegisterClient
        /// (
        ///   new GoogleClient
        ///   (
        ///     "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com", 
        ///     "AeEbEGQqoKgOZb41JUVLvEJL"
        ///   )
        /// );
        /// 
        /// OAuthManager.RegisterClient
        /// (
        ///   new FacebookClient
        ///   (
        ///     "1435890426686808", 
        ///     "c6057dfae399beee9e8dc46a4182e8fd"
        ///   )
        /// );
        /// </code>
        /// <code lang="VB">
        /// OAuthManager.RegisterClient _
        /// (
        ///   New GoogleClient _
        ///   (
        ///     "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com", 
        ///     "AeEbEGQqoKgOZb41JUVLvEJL"
        ///   )
        /// )
        /// 
        /// OAuthManager.RegisterClient _
        /// (
        ///   New FacebookClient _
        ///   (
        ///     "1435890426686808", 
        ///     "c6057dfae399beee9e8dc46a4182e8fd"
        ///   )
        /// )
        /// </code>
        /// </example>
        public static void RegisterClient(OAuthBase client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            if (_RegisteredClients.ContainsKey(client.ProviderName))
            {
                throw new DuplicateProviderException(client.ProviderName);
            }
            // add client
            _RegisteredClients.Add(client.ProviderName, client);
            // remove from watching
            OAuthManager.RemoveRequet(client.State.ToString());
        }

        /// <summary>
        /// Register a cache to use to store the requests
        /// </summary>
        /// <param name="cache"><see cref="IRequestCache"/> to use to store the requests</param>
        /// <remarks>Use this if you need to store the requests in a shared key value store
        /// I.e. if you are using a web farm
        /// </remarks>
        public static void RegisterCache(IRequestCache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }
            cache.Initialise("Nemiro.OAuth");
            _requestCache = cache;
        }

        #endregion

    }

}
