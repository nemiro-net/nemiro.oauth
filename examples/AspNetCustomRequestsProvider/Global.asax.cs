using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Nemiro.OAuth;

namespace AspNetCustomRequestsProvider
{

  public class MvcApplication : System.Web.HttpApplication
  {

    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);

      // set custom requests provider
      OAuthManager.SetAuthRequestsProvider(typeof(MemCacheOAuthRequestsProvider));

      // OAuth clients registration
      // NOTE: Specify their own client IDs and secret keys
      OAuthManager.RegisterClient
      (
        "facebook",
        "1435890426686808",
        "c6057dfae399beee9e8dc46a4182e8fd"
      );

      OAuthManager.RegisterClient
      (
        "mail.ru",
        "722701",
        "d0622d3d9c9efc69e4ca42aa173b938a"
      );

      OAuthManager.RegisterClient
      (
        "google",
        "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com",
        "AeEbEGQqoKgOZb41JUVLvEJL"
      );

      OAuthManager.RegisterClient
      (
        "live",
        "0000000040124265",
        "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
      );
    }
    
  }

}
