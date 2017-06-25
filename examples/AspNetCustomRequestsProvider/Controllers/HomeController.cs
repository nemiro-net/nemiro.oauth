using System;
using System.Linq;
using System.Web.Mvc;
using Nemiro.OAuth;

namespace AspNetCustomRequestsProvider.Controllers
{

  public class HomeController : Controller
  {

    string[] NotSupportedLocalhost = { "live", "mail.ru" };

    public ActionResult Index()
    {
      return View();
    }

    /// <summary>
    /// Redirect to login
    /// </summary>
    /// <param name="provider">Provider name. For example: facebook, twitter, google etc.</param>
    public ActionResult ExternalLogin(string provider)
    {
      // build callback url
      string returnUrl = Url.Action("ExternalLoginResult", "Home", null, null, Request.Url.Host);

      // not suppored localhost (it is only for localhost)
      if (this.NotSupportedLocalhost.Any(itm => itm.Equals(provider, StringComparison.OrdinalIgnoreCase)))
      {
        returnUrl = String.Format("http://oauth.nemiro.net/oauth_redirect.html?returnUrl={0}", Server.UrlEncode(returnUrl));
      }
      // --

      // redirect to authorization page of the specified provider
      return Redirect
      (
        OAuthWeb.GetAuthorizationUrl
        (
          provider, 
          returnUrl, 
          new
          {
            text = "custom state",
            text2 = "you can use any type with any data",
            aaa = 123,
            bbb = "abc"
          }
        )
      );

      // return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl, "custom state"));
      // return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl, 123));

      // without state
      // return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl));
    }

    /// <summary>
    /// Login result
    /// </summary>
    public ActionResult ExternalLoginResult()
    {
      return View(OAuthWeb.VerifyAuthorization());
    }

  }

}