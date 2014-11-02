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
using System.Collections.Specialized;

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

    private static Timer _Timer = new Timer(60000);

    private static Dictionary<string, Type> _AllClients = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
    /// <summary>
    /// Gets the list of all clients.
    /// </summary>
    internal static Dictionary<string, Type> AllClients
    {
      get
      {
        return _AllClients;
      }
    } 
   
    private static Dictionary<string, OAuthRequest> _Requets = new Dictionary<string, OAuthRequest>();
    /// <summary>
    /// Gets the list of active requests.
    /// </summary>
    internal static Dictionary<string, OAuthRequest> Requets
    {
      get
      {
        return _Requets;
      }
    }

    private static Dictionary<string, OAuthBase> _RegisteredClients = new Dictionary<string, OAuthBase>(StringComparer.OrdinalIgnoreCase);
    /// <summary>
    /// Gets the list of registered clients.
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
      // get all clients
      var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where
      (
        itm =>
        itm.BaseType != null &&
        (itm.BaseType == typeof(OAuthClient) || itm.BaseType == typeof(OAuth2Client))
      );
      // creating clients list
      foreach (var t in types)
      {
        var param = new ArrayList();
        foreach (var p in t.GetConstructors().First().GetParameters())
        {
          if (p.ParameterType == typeof(string))
          {
            param.Add("123");
          }
          else
          {
            throw new NotSupportedException(String.Format("Supported only string data types for parameters constructors of providers class. Please, check {0} class.", t.Name));
          }
        }
        var client = Activator.CreateInstance(t, param.ToArray()) as OAuthBase;
        _AllClients.Add(client.ProviderName, t);
      }
      // --
      _Timer.Elapsed += Timer_Elapsed;
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// The method is called when the interval elapsed.
    /// </summary>
    /// <param name="sender">Instance of the object that raised the event.</param>
    /// <param name="e">The event data.</param>
    private static void Timer_Elapsed(object sender, EventArgs e)
    {
      if (_Requets.Count <= 0) 
      {
        // no active requests, stop the time
        _Timer.Stop();
        return; 
      }

      // lifetime request - 20 minutes
      // remove old requests
      var now = DateTime.Now;
      var toRemove = _Requets.Where(itm2 => now.Subtract(itm2.Value.DateCreated).TotalMinutes >= 20).ToList();

      foreach (var itm in toRemove)
      {
        if (_Requets.ContainsKey(itm.Key))
        {
          OAuthManager.RemoveRequet(itm.Key);
        }
      }

      // change the status of the timer
      _Timer.Enabled = (_Requets.Count > 0);
    }

    /// <summary>
    /// Adds the specified request to the collection.
    /// </summary>
    /// <param name="key">The unique request key.</param>
    /// <param name="client">The client instance.</param>
    internal static void AddRequet(string key, OAuthBase client)
    {
      OAuthManager.Requets.Add(key, new OAuthRequest(client));
      _Timer.Start();
    }

    /// <summary>
    /// Removes the request from collection.
    /// </summary>
    /// <param name="key">The unique request key to remove..</param>
    internal static void RemoveRequet(string key)
    {
      if (_Requets.ContainsKey(key))
      {
        _Requets.Remove(key);
      }
      _Timer.Enabled = (_Requets.Count > 0);
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
    /// Registers the specified client in the application.
    /// </summary>
    /// <param name="providerName">The provider name.</param>
    /// <param name="clientId">The application identifier obtained from the provider website.</param>
    /// <param name="clientSecret">The application secret key obtained from the provider website.</param>
    /// <param name="initArgs">Additional parameters to be passed to the constructor of the client class.</param>
    /// <param name="scope">List of scope that will be requested from the provider. Only for OAuth 2.0.</param>
    /// <param name="parameters">Additional parameters that will be transferred to the provider website.</param>
    /// <exception cref="ArgumentNullException"><paramref name="providerName"/>, <paramref name="clientId"/> or <paramref name="clientSecret"/> is <b>null</b> or <b>empty</b>.</exception>
    /// <exception cref="UnknownProviderException">Provider not found by <paramref name="providerName"/>.</exception>
    /// <exception cref="NotSupportedException">The <paramref name="providerName"/> not suppored <paramref name="scope"/>.</exception>
    /// <example>
    /// <code lang="C#">
    /// OAuthManager.RegisterClient
    /// (
    ///   "google", 
    ///   "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com", 
    ///   "AeEbEGQqoKgOZb41JUVLvEJL"
    /// );
    /// 
    /// OAuthManager.RegisterClient
    /// (
    ///   "facebook"
    ///   "1435890426686808", 
    ///   "c6057dfae399beee9e8dc46a4182e8fd"
    /// );
    /// </code>
    /// <code lang="VB">
    /// OAuthManager.RegisterClient _
    /// (
    ///   "google",
    ///   "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com", 
    ///   "AeEbEGQqoKgOZb41JUVLvEJL"
    /// )
    /// 
    /// OAuthManager.RegisterClient _
    /// (
    ///   "facebook",
    ///   "1435890426686808", 
    ///   "c6057dfae399beee9e8dc46a4182e8fd"
    /// )
    /// </code>
    /// </example>
    public static void RegisterClient(string providerName, string clientId, string clientSecret, string scope = null, NameValueCollection parameters = null, object[] initArgs = null)
    {
      if (String.IsNullOrEmpty(providerName)) { throw new ArgumentNullException("providerName"); }
      if (String.IsNullOrEmpty(clientId)) { throw new ArgumentNullException("clientId"); }
      if (String.IsNullOrEmpty(clientSecret)) { throw new ArgumentNullException("clientSecret"); }
      // searching provider by name
      if (!OAuthManager.AllClients.ContainsKey(providerName))
      {
        throw new UnknownProviderException("Provider [{0}] not found. Please, check provider name.", providerName);
      }
      // init parameters
      var parm = new ArrayList();
      parm.Add(clientId);
      parm.Add(clientSecret);
      if (initArgs != null && initArgs.Length > 0)
      {
        parm.AddRange(initArgs);
      } 
      // creating client instance
      OAuthBase client = Activator.CreateInstance(OAuthManager.AllClients[providerName], parm.ToArray()) as OAuthBase;
      if (!String.IsNullOrEmpty(scope))
      {
        if (client.GetType().BaseType != typeof(OAuth2Client))
        {
          throw new NotSupportedException("The scope supported only for OAuth 2.0 clients.");
        }
        ((OAuth2Client)client).Scope = scope;
      }
      if (parameters != null)
      {
        client.Parameters = parameters;
      }
      // add client
      OAuthManager.RegisterClient(client);
    }
    #endregion

  }

}
