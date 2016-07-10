// (c) Aleksey Nemiro, 2014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nemiro.OAuth;
using System.Collections.Specialized;
using Nemiro.OAuth.Clients;
using System.Configuration;
using System.Text.RegularExpressions;
using Nemiro.OAuth.Extensions;
using System.Text;

namespace DemoOAuth.Controllers
{

  [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.None)]
  public partial class ApiController : Controller
  {
    
    /// <summary>
    /// Checks the test files for upload.
    /// </summary>
    private void InitTestFiles()
    {
      if (HttpContext.Cache["Nemiro.OAuth"] == null)
      {
        // cache is empty
        // get file for tests
        var r = OAuthUtility.Get("https://github.com/alekseynemiro/nemiro.oauth.dll/archive/master.zip");
        if (r.IsFile)
        {
          HttpContext.Cache.Add("Nemiro.OAuth", (byte[])r, null, DateTime.Now.AddDays(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
        }
      }
    }

    [HttpPost]
    public ActionResult GetUserInfo(string provider)
    {
        AccessToken accessToken = AccessToken.Empty;
      try
      {
        if (Session[String.Format("{0}:AccessToken", provider)] != null)
        {
          accessToken = (AccessToken)Session[String.Format("{0}:AccessToken", provider)];
        }

        if (AccessToken.IsNullOrEmpty(accessToken))
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        var result = OAuthManager.RegisteredClients[provider].GetUserInfo(accessToken);

        return Content(result.Items.ToString(), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

    [HttpPost]
    public ActionResult Token(string provider)
    {
      try
      {
        if (Session[String.Format("{0}:AccessToken", provider)] == null)
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        return Content(RequestResult.Create((RequestResult)Session[String.Format("{0}:AccessToken", provider)]).ToString(), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

    [HttpPost]
    public ActionResult Refresh(string provider)
    {
      try
      {
        AccessToken accessToken = AccessToken.Empty;
        if (Session[String.Format("{0}:AccessToken", provider)] != null)
        {
          accessToken = (AccessToken)Session[String.Format("{0}:AccessToken", provider)];
        }

        if (AccessToken.IsNullOrEmpty(accessToken))
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        if (provider.Equals("yahoo", StringComparison.OrdinalIgnoreCase))
        {
          // #!@#%^
          Session[String.Format("{0}:AccessToken", provider)] = ((YahooClient)OAuthManager.RegisteredClients[provider]).RefreshToken(accessToken, Session[String.Format("{0}:ReturnUrl", provider)].ToString());
        }
        else
        {
          Session[String.Format("{0}:AccessToken", provider)] = OAuthManager.RegisteredClients[provider].RefreshToken(accessToken);
        }

        return Content
        (
          "{ msgbox: 'ok' }",
          "text/plain"
        );
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

    [HttpPost]
    public ActionResult Revoke(string provider)
    {
      try
      {
        AccessToken accessToken = AccessToken.Empty;
        if (Session[String.Format("{0}:AccessToken", provider)] != null)
        {
          accessToken = (AccessToken)Session[String.Format("{0}:AccessToken", provider)];
        }

        if (AccessToken.IsNullOrEmpty(accessToken))
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        var result = OAuthManager.RegisteredClients[provider].RevokeToken(accessToken);
        
        if (result.IsSuccessfully)
        {
          Session[String.Format("{0}:AccessToken", provider)] = null;
        }

        return Content
        (
          (result.IsSuccessfully ? "{ msgbox: 'ok' }" : result.ToString()),
          "text/plain"
        );
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

  }

}