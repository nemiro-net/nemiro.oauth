using System;
using System.Linq;
using System.Web.Mvc;
using Nemiro.OAuth;

namespace Test.CSharp.AspMvc.Controllers
{

  public class HomeController : Controller
  {

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Icon(string id)
    {
      // image from resource
      try
      {
        return File((byte[])Test.Resources.Images.ResourceManager.GetObject(id.ToLower().Replace(".", "")), "image/png");
      }
      catch
      {
        return File((byte[])Test.Resources.Images.ResourceManager.GetObject("error"), "image/png");
      }
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
      string[] notSupportedLocalhost = { "live", "mail.ru" };

      if (notSupportedLocalhost.Any(itm => itm.Equals(provider, StringComparison.OrdinalIgnoreCase)))
      {
        returnUrl = String.Format("http://oauth.nemiro.net/oauth_redirect.html?returnUrl={0}", Server.UrlEncode(returnUrl));
      }
      else if (provider.Equals("github", StringComparison.OrdinalIgnoreCase))
      {
        returnUrl = String.Format("https://oauthproxy.nemiro.net/?returnUrl={0}", Server.UrlEncode(returnUrl));
      }
      // --

      // redirect to authorization page of the specified provider
      return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl));
    }

    /// <summary>
    /// Login result
    /// </summary>
    public ActionResult ExternalLoginResult()
    {
      return View(OAuthWeb.VerifyAuthorization());
    }

    public ActionResult Refresh(string clientName, string accessToken, string refreshToken)
    {
      AccessToken token = accessToken;
      if (!String.IsNullOrEmpty(refreshToken))
      {
        token = new OAuth2AccessToken(accessToken, refreshToken);
      }
      var result = OAuthManager.RegisteredClients[clientName].RefreshToken(token);
      return new ContentResult { Content = result.ToString(), ContentType = "text/plain" };
    }

    public ActionResult Revoke(string clientName, string accessToken)
    {
      var result = OAuthManager.RegisteredClients[clientName].RevokeToken(accessToken);
      return new ContentResult { Content = result.ToString(), ContentType = "text/plain" };
    }

  }

}
