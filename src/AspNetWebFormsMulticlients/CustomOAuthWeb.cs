using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using Nemiro.OAuth;

namespace AspNetWebFormsMulticlients
{
  public class CustomOAuthWeb
  {

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="providerName"/> is unregistered. Use the <see cref="CustomOAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string)"/>
    public static void RedirectToAuthorization(string groupName, string providerName)
    {
      CustomOAuthWeb.RedirectToAuthorization(groupName, providerName, null, null);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider with specified parameters.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization query.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="providerName"/> is unregistered. Use the <see cref="CustomOAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, NameValueCollection)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string, NameValueCollection)"/>
    public static void RedirectToAuthorization(string groupName, string providerName, NameValueCollection parameters)
    {
      CustomOAuthWeb.RedirectToAuthorization(groupName, providerName, parameters, null);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider and return URL.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="providerName"/> is unregistered. Use the <see cref="CustomOAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string, string)"/>
    public static void RedirectToAuthorization(string groupName, string providerName, string returnUrl)
    {
      CustomOAuthWeb.RedirectToAuthorization(groupName, providerName, null, returnUrl);
    }

    /// <summary>
    /// Redirects current client to the authorization page of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization query.</param>
    /// <exception cref="ClientIsNotRegisteredException">
    /// <paramref name="providerName"/> is unregistered. Use the <see cref="CustomOAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.
    /// </exception>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    /// <remarks>
    /// <para>The method will not work in desktop applications. For desktop applications you can use <see cref="GetAuthorizationUrl(string, NameValueCollection, string)"/>.</para>
    /// </remarks>
    /// <seealso cref="GetAuthorizationUrl(string, NameValueCollection, string)"/>
    public static void RedirectToAuthorization(string groupName, string providerName, NameValueCollection parameters, string returnUrl)
    {
      if (!CustomOAuthManager.RegisteredClients.ContainsKey(String.Format("{0}/{1}", groupName, providerName)))
      {
        throw new ClientIsNotRegisteredException();
      }

      CustomOAuthManager.RegisteredClients[String.Format("{0}/{1}", groupName, providerName)].Clone(parameters, returnUrl).RedirectToAuthorization();
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string groupName, string providerName)
    {
      return CustomOAuthWeb.GetAuthorizationUrl(groupName, providerName, null, null);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider with specified parameters.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization URL.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string groupName, string providerName, NameValueCollection parameters)
    {
      return CustomOAuthWeb.GetAuthorizationUrl(groupName, providerName, parameters, null);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider and return URL.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string groupName, string providerName, string returnUrl)
    {
      return CustomOAuthWeb.GetAuthorizationUrl(groupName, providerName, null, returnUrl);
    }

    /// <summary>
    /// Returns the authorization URL of the specified provider, query parameters and return URL.
    /// </summary>
    /// <param name="providerName">Provider name, through which it is necessary to authorize the current user.</param>
    /// <param name="parameters">Additional parameters to be passed to the authorization URL.</param>
    /// <param name="returnUrl">The address to which the user is redirected after the authorization.</param>
    /// <exception cref="NullHttpContextException">
    /// The exception that is thrown when you try to access methods that are designed for web projects.
    /// </exception>
    public static string GetAuthorizationUrl(string groupName, string providerName, NameValueCollection parameters, string returnUrl)
    {
      if (!CustomOAuthManager.RegisteredClients.ContainsKey(String.Format("{0}/{1}", groupName, providerName)))
      {
        throw new ClientIsNotRegisteredException();
      }

      return CustomOAuthManager.RegisteredClients[String.Format("{0}/{1}", groupName, providerName)].Clone(parameters, returnUrl).AuthorizationUrl;
    }

  }
}