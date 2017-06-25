using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;
using System.Collections.Specialized;

namespace Test.CSharp.AspMvc
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

      routes.MapRoute(
          "Default", 
          "{controller}/{action}/{id}",
          new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );

    }

    protected void Application_Start()
    {
      #region mvc
      AreaRegistration.RegisterAllAreas();

      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);
      #endregion

      // OAuth clients registration
      // NOTE: Specify their own client IDs and secret keys
      OAuthManager.RegisterClient
      (
        new FacebookClient
        (
          "1435890426686808",
          "c6057dfae399beee9e8dc46a4182e8fd"
        )
        {
          Scope = "public_profile,email"
          //Parameters = new NameValueCollection { { "display", "popup" } }
        }
      );

      OAuthManager.RegisterClient
      (
        new TwitterClient
        (
          "1Ayh2ZM2l9chloiFsmxNpi7Gg",
          "dbVXKWVIlH5fRuVI0FvE2ZDsZAbBg74UrGFYwW1kLSwc0ceJnJ"
        )
      );

      OAuthManager.RegisterClient
      (
        new VkontakteClient
        (
          "2419779",
          "31nnASa9T1eO150VCFgr"
        )
      );

      OAuthManager.RegisterClient
      (
        new MailRuClient
        (
          "722701",
          "d0622d3d9c9efc69e4ca42aa173b938a"
        )
      );

      OAuthManager.RegisterClient
      (
        new GoogleClient
        (
          "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com",
          "AeEbEGQqoKgOZb41JUVLvEJL"
        )
        // { Parameters = new NameValueCollection { { "approval_prompt", "force" } } }
      );

      OAuthManager.RegisterClient
      (
        new YandexClient
        (
          "7b0a53d23e384033be3a414739be255f",
          "3c85e4a1148640b2a490cbd8c75384df"
        )
      );

      OAuthManager.RegisterClient
      (
        new OdnoklassnikiClient
        (
          "1094959360",
          "E45991423E8C5AE249B44E84",
          "CBACMEECEBABABABA"
        )
      );

      OAuthManager.RegisterClient
      (
        new GitHubClient
        (
          "e14122695d88f5c95bce",
          "cde23ec001c5180e01e865f4efb57cb0bc848c16"
        )
      );

      OAuthManager.RegisterClient
      (
        new LiveClient
        (
          "0000000040124265",
          "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
        )
      );

      OAuthManager.RegisterClient
      (
        new AmazonClient
        (
          "amzn1.application-oa2-client.f0ffe4edc256488dae00dcaf96d75d1b",
          "764dcefe49b441c8c6244c93e5d5d04de54fda6dfdc83da9693bf346f4dc4515"
        ) { ReturnUrl = "http://localhost" }
      );

      OAuthManager.RegisterClient
      (
        "foursquare",
        "LHYZN1KUXN50L141QCQFNNVOYBGUE3G3FCWFZ3EEZTOZHY5Q",
        "HWXYFLLSS2IUQ0H4XNCDAZEFZKIU3MZRP5G55TNBDHRPNOQT"
        // parameters: new NameValueCollection { { "display", "webpopup" } } // webpopup
      );

      OAuthManager.RegisterClient
      (
        new TumblrClient
        (
          "2EZbsj2oF8OAouPlDWSVnESetAchImzPLV4q0IcQH7DGKECuzJ",
          "4WZ3HBDwNuz5ZDZY8qyK1qA5QFHEJY7gkPK6ooYFCN4yw6crKd"
        )
      );

      OAuthManager.RegisterClient
      (
        new InstagramClient
        (
          "215a1941ebed4e4fa74e94dd84762836",
          "ba53a710e1624870bc066e7a9ae38601"
        )
      );

      OAuthManager.RegisterClient
      (
        new AssemblaClient
        (
          "bOS4QkXnmr5jhdacwqjQXA",
          "701ee6dedf74fc4ad75bfa7476666a2f"
        )
      );
    }
  }
}