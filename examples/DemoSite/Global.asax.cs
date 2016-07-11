using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Collections.Specialized;

namespace DemoOAuth
{

  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
      routes.AppendTrailingSlash = true;

      routes.MapRoute
      (
        "Api",
        "Api/{action}/{method}",
        new { controller = "Api", action = "Index", method = UrlParameter.Optional }
      );

      routes.MapRoute
      (
        "Default", 
        "{action}/{id}",
        new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }

    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();

      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);

      this.RegistrationOAuthClients();
    }

    protected void Application_AcquireRequestState()
    {
      this.SetCulture();
    }

    /// <summary>
    /// Registartion OAuth clients.
    /// </summary>
    private void RegistrationOAuthClients()
    {
      foreach (string clientName in ConfigurationManager.AppSettings["oauth:clients"].Split(','))
      {
        if (String.IsNullOrEmpty(clientName)) { continue; }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings[String.Format("oauth:{0}:id", clientName)]))
        {
          throw new ArgumentNullException(String.Format(DemoOAuth.Strings.ClientIdIsRequired, clientName));
        }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings[String.Format("oauth:{0}:key", clientName)]))
        {
          throw new ArgumentNullException(String.Format(DemoOAuth.Strings.ClientKeyIsRequired, clientName));
        }
         
        // public key for odnoklassniki.ru
        ArrayList args = null;
        if (clientName.Equals("odnoklassniki", StringComparison.OrdinalIgnoreCase))
        {
          if (String.IsNullOrEmpty(ConfigurationManager.AppSettings[String.Format("oauth:{0}:publicKey", clientName)]))
          {
            throw new ArgumentNullException(String.Format(DemoOAuth.Strings.PublicKeyIsRequired, clientName));
          }
          args = new ArrayList();
          args.Add(ConfigurationManager.AppSettings[String.Format("oauth:{0}:publicKey", clientName)]);
        }
        // --

        // necessary permissions
        string scope = null;
        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[String.Format("oauth:{0}:scope", clientName)]))
        {
          scope = ConfigurationManager.AppSettings[String.Format("oauth:{0}:scope", clientName)];
        }
        // --
        
        // other parameters
        NameValueCollection parameters = null;
        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[String.Format("oauth:{0}:parameters", clientName)]))
        {
          parameters = HttpUtility.ParseQueryString(ConfigurationManager.AppSettings[String.Format("oauth:{0}:parameters", clientName)]);
        }
        // --

        OAuthManager.RegisterClient
        (
          clientName,
          ConfigurationManager.AppSettings[String.Format("oauth:{0}:id", clientName)], 
          ConfigurationManager.AppSettings[String.Format("oauth:{0}:key", clientName)], 
          initArgs: (args != null ? args.ToArray() : null),
          scope: scope,
          parameters: parameters
        );
      }
    }

    /// <summary>
    /// Set language.
    /// </summary>
    private void SetCulture()
    {
      try
      {
        if (Request.Url.ToString().IndexOf("Icon") != -1 || Request.Url.ToString().EndsWith(".css") || Request.Url.ToString().EndsWith(".js")) { return; }
        string lang = null;
        if (Request.Cookies["lang"] != null && !String.IsNullOrEmpty(Request.Cookies["lang"]["code"]))
        {
          lang = Request.Cookies["lang"]["code"];
        }
        if (!String.IsNullOrEmpty(Request.QueryString["lang"]))
        {
          lang = Request.QueryString["lang"];
        }
        if (String.IsNullOrEmpty(lang) && Request.UserLanguages.Length > 0)
        {
          lang = Request.UserLanguages.First().Substring(2).ToLower();
          string[] russian = { "ru", "be", "ab", "uk", "ba", "uz", "tg", "ky", "kk" };
          if (Array.IndexOf(russian, lang) != -1)
          {
            lang = "ru";
          }
        }
        if (!String.IsNullOrEmpty(lang))
        {
          var ci = new CultureInfo(lang);
          Thread.CurrentThread.CurrentUICulture = ci;
          Thread.CurrentThread.CurrentCulture = ci;
          // set lang to cookies
          if (Request.Cookies["lang"] == null || String.IsNullOrEmpty(Request.Cookies["lang"]["code"]) || !lang.Equals(Request.Cookies["lang"]["code"], StringComparison.OrdinalIgnoreCase))
          {
            Response.Cookies["lang"]["code"] = lang;
            Response.Cookies["lang"].Expires = DateTime.Now.AddYears(1);
          }
        }
        if (!String.IsNullOrEmpty(Request.QueryString["lang"]))
        {
          Response.Redirect("~/");
        }
      }
      catch { }
    }

  
  }
}