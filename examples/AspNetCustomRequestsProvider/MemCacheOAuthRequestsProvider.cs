using System;
using System.Collections;
using System.Linq;
using System.Runtime.Caching;
using Nemiro.OAuth;

namespace AspNetCustomRequestsProvider
{

  public class MemCacheOAuthRequestsProvider : IOAuthRequestsProvider
  {

    private MemoryCache Cache = null;

    public MemCacheOAuthRequestsProvider()
    {
      this.Init();
    }

    private void Init()
    {
      this.Cache = new MemoryCache("MemCacheOAuthRequestsProvider");
    }

    public void Add(string key, ClientName clientName, OAuthBase client, object state)
    {
      this.Cache.Add(String.Format("oauth-{0}", key), new OAuthRequest(clientName, client, state), DateTime.Now.AddMinutes(20));
    }

    public void Clear()
    {
      this.Cache.Dispose();
      this.Init();
    }

    public bool ContainsKey(string key)
    {
      return this.Cache.Contains(String.Format("oauth-{0}", key));
    }

    public OAuthRequest Get(string key)
    {
      return this.Get<OAuthRequest>(key);
    }

    public T Get<T>(string key) where T : OAuthRequest
    {
      return (T)this.Cache[String.Format("oauth-{0}", key)];
    }

    public IEnumerator GetEnumerator()
    {
      return this.Cache.ToDictionary(k => k.Key, v => v.Value).GetEnumerator();
    }

    public void Remove(string key)
    {
      this.Cache.Remove(String.Format("oauth-{0}", key));
    }

  }

}