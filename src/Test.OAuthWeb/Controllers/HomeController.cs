using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test.OAuthWeb.Controllers
{
  public class HomeController : Controller
  {

    public ActionResult Index()
    {
      return View();
    }

    [OutputCache(Duration = 1800, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "id")]
    public ActionResult Icon(string id)
    {
      // image from resource
      return File((byte[])Test.Resources.Images.ResourceManager.GetObject(id.ToLower().Replace(".", "")), "image/png");
    }

    /// <summary>
    /// Redirect to login
    /// </summary>
    /// <param name="provider">Provider name. For example: facebook, twitter, google etc.</param>
    [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.None)]
    public ActionResult ExternalLogin(string provider)
    {
      string protocol = null;
      // I have not normal SSL
      // but some providers require https
      string[] notSupportedHttp = { "dropbox", "amazon" };
      if (notSupportedHttp.Any(itm => itm.Equals(provider, StringComparison.OrdinalIgnoreCase)))
      {
        protocol = "https";
      }
      // redirect to authorization page of the specified provider
      return Redirect(Nemiro.OAuth.OAuthWeb.GetAuthorizationUrl(provider, Url.Action("ExternalLoginResult", "Home", null, protocol, Request.Url.Host)));
    }

    /// <summary>
    /// Login result
    /// </summary>
    [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.None)]
    public ActionResult ExternalLoginResult()
    {
      Response.StatusCode = 403;
      var result = Nemiro.OAuth.OAuthWeb.VerifyAuthorization();
      if (result.IsSuccessfully)
      {
        Session[String.Format("{0}:AccessToken", result.ProviderName)] = result.AccessTokenValue;
        Session[String.Format("{0}:UserId", result.ProviderName)] = result.UserInfo.UserId;
        Session[String.Format("{0}:UserName", result.ProviderName)] = result.UserInfo.UserName;
        Session[String.Format("{0}:Email", result.ProviderName)] = result.UserInfo.Email;
        if (result.ProtocolVersion == "1.0")
        {
          Session[String.Format("{0}:TokenSecret", result.ProviderName)] = ((Nemiro.OAuth.OAuthAccessToken)result.AccessToken).TokenSecret;
        }
      }
      return View(result);
    }

  }
}