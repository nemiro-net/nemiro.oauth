using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Nemiro.OAuth
{
    /// <summary>
    /// A request cache implementation using MemoryCache
    /// </summary>
    public class RequestMemoryCache : IRequestCache
    {
        private MemoryCache _cache;
        private bool _initialized;

        public void Initialise(string storeName)
        {
            if (!_initialized)
            {
                _cache = new MemoryCache(storeName);
                _initialized = true;
            }
        }

        public void Set(string key, object value, TimeSpan validFor)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.UtcNow.Add(validFor);
            _cache.Set(key, value, policy);
        }

        public T Get<T>(string key)
        {
            return (T)_cache[key];
        }

        public void Remove(string key)
        {
            if (Exists(key))
            {
                _cache.Remove(key);
            }
        }

        public bool Exists(string key)
        {
            return _cache.Contains(key);
        }
    }
}
