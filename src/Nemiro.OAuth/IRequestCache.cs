using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nemiro.OAuth
{
    /// <summary>
    /// Interface for request cache
    /// </summary>
    public interface IRequestCache
    {
        /// <summary>
        /// Performs initialisation tasks required for the cache implementation
        /// </summary>
        void Initialise(string storeName);

        /// <summary>
        /// Insert or update a cache value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="validFor"></param>
        void Set(string key, object value, TimeSpan validFor);

        /// <summary>
        /// Retrieve a typed value from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Removes the value for the given key from the cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Returns whether the cache contains a value for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);
    }
}
