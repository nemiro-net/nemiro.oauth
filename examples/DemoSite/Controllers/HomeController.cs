// (c) Aleksey Nemiro, 2014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nemiro.OAuth;
using System.Text;
using System.Collections.Specialized;

namespace DemoOAuth.Controllers
{
  public class HomeController : Controller
  {

    public ActionResult Index()
    {
      try
      {
        if (HttpContext.Cache["LatestRelease"] == null)
        {
          var result = OAuthUtility.Get("https://www.nuget.org/api/v2/Search()?$orderby=Id&$skip=0&$top=30&searchTerm=%27Nemiro.OAuth%27&targetFramework=%27%27&includePrerelease=false");
          foreach (UniValue item in result["feed"]["entry"])
          {
            if (item["properties"]["Id"].ToString().Equals("Nemiro.OAuth", StringComparison.OrdinalIgnoreCase))
            {
              ViewBag.Version = item["properties"]["Version"].ToString();
              break;
            }
          }
          HttpContext.Cache.Add("LatestRelease", ViewBag.Version, null, DateTime.Now.AddHours(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
        }
        else
        {
          ViewBag.Version = HttpContext.Cache["LatestRelease"].ToString();
        }
      }
      catch(Exception ex)
      {
        ViewBag.Version = "ERR: " + ex.Message;
      }
      return View();
    }

    [OutputCache(Duration = 1800, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "id")]
    public ActionResult Icon(string id)
    {
      try
      {
        // image from resource
        return File((byte[])DemoOAuth.Images.ResourceManager.GetObject(id.ToLower().Replace(".", "")), "image/png");
      }
      catch
      {
        return File((byte[])DemoOAuth.Images.ResourceManager.GetObject("error"), "image/png");
      }
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
      string[] notSupportedHttp = { "dropbox", "amazon", "codeproject", "odnoklassniki" };

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
        Session[String.Format("{0}:AccessToken", result.ProviderName)] = result.AccessToken;
        Session[String.Format("{0}:UserId", result.ProviderName)] = result.UserInfo.UserId;
        Session[String.Format("{0}:UserName", result.ProviderName)] = result.UserInfo.UserName;
        Session[String.Format("{0}:Email", result.ProviderName)] = result.UserInfo.Email;
        
        if (result.ProviderName.Equals("yahoo", StringComparison.OrdinalIgnoreCase))
        {
          // for refreshing token
          Session[String.Format("{0}:ReturnUrl", result.ProviderName)] = Request.Url.ToString();
        }
      }

      TempData["ExternalLoginResult"] = result;

      return RedirectToAction("Result");
    }

    /// <summary>
    /// Login result view
    /// </summary>
    [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.None)]
    public ActionResult Result()
    {
      if (TempData["ExternalLoginResult"] == null)
      {
        return RedirectToAction("Index");
      }

      Response.StatusCode = 403;

      return View(TempData["ExternalLoginResult"]);
    }

  }
}