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

namespace Test.OAuthWeb.Controllers
{

  [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.None)]
  public class ApiController : Controller
  {

    [HttpPost]
    public ActionResult Facebook(string method)
    {
      try
      {
        if (Session["Facebook:AccessToken"] == null)
        {
          throw new Exception(Test.Resources.Strings.SessionIsDead);
        }

        var parameters = new NameValueCollection
        { 
          { "access_token", Session["Facebook:AccessToken"].ToString() }
        };

        // execute the request
        var result = OAuthUtility.ExecuteRequest
        (
          "GET",
          String.Format("https://graph.facebook.com/v2.2/me/{0}", method),
          parameters
        );

        return Content(Regex.Unescape(result.ToString()), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

    [HttpPost]
    public ActionResult Twitter(string method)
    {
      try
      {
        if (Session["Twitter:AccessToken"] == null)
        {
          throw new Exception(Test.Resources.Strings.SessionIsDead);
        }

        string url = "https://api.twitter.com/1.1/followers/ids.json";

        if (method == "user_timeline")
        {
          url = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        }

        var auth = new OAuthAuthorization();
        auth["oauth_consumer_key"] = ConfigurationManager.AppSettings["oauth:twitter:id"];
        auth["oauth_signature_method"] = SignatureMethods.HMACSHA1;
        auth["oauth_nonce"] = OAuthUtility.GetRandomKey();
        auth["oauth_timestamp"] = OAuthUtility.GetTimeStamp();
        auth["oauth_token_secret"] = Session["Twitter:TokenSecret"].ToString();
        auth["oauth_token"] = Session["Twitter:AccessToken"].ToString();
        auth["oauth_version"] = "1.0";
        auth.SetSignature("GET", new Uri(url), ConfigurationManager.AppSettings["oauth:twitter:key"], Session["Twitter:TokenSecret"].ToString(), null);

        var result = OAuthUtility.ExecuteRequest("GET", url, authorization: auth.ToString());

        return Content(Regex.Unescape(result.ToString()), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

    [HttpPost]
    public ActionResult VK(string method)
    {
      try
      {
        if (Session["VK:AccessToken"] == null)
        {
          throw new Exception(Test.Resources.Strings.SessionIsDead);
        }

        string fields = "sex,bdate,city,country,photo_max_orig,domain,contacts,site";
        string url = "https://api.vk.com/method/friends.get";

        // query parameters
        var parameters = new NameValueCollection
        { 
          { "access_token" , Session["VK:AccessToken"].ToString() }
        };

        if (method == "status.get")
        {
          url = "https://api.vk.com/method/status.get";
        }
        else if (method == "audio.get")
        {
          url = "https://api.vk.com/method/audio.get";
        }
        else
        {
          parameters.Add("fields", fields);
        }

        // execute the request
        var result = OAuthUtility.ExecuteRequest("GET", url, parameters);

        return Content(Regex.Unescape(result.ToString()), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }
    
    [HttpPost]
    public ActionResult MailRu(string method)
    {
      try
      {
        if (Session["Mail.Ru:AccessToken"] == null)
        {
          throw new Exception(Test.Resources.Strings.SessionIsDead);
        }

        // query parameters
        var parameters = new NameValueCollection
        { 
          { "method", method },
          { "app_id", ConfigurationManager.AppSettings["oauth:mail.ru:id"] },
          { "uid" , Session["Mail.Ru:UserId"].ToString() },
          { "secure", "1" },
          { "format", "json" }
        };

        string signatureBaseString = parameters.Sort().ToParametersString();
        parameters["sig"] = OAuthUtility.GetMD5Hash(signatureBaseString + ConfigurationManager.AppSettings["oauth:mail.ru:key"]);

        // execute the request
        var result = OAuthUtility.ExecuteRequest("POST", "http://www.appsmail.ru/platform/api", parameters);

        return Content(Regex.Unescape(result.ToString()), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

    [HttpPost]
    public ActionResult Google(string method)
    {
      try
      {
        if (Session["Google:AccessToken"] == null)
        {
          throw new Exception(Test.Resources.Strings.SessionIsDead);
        }

        var parameters = new NameValueCollection
        { 
          { "access_token", Session["Google:AccessToken"].ToString() }
        };

        string url = String.Format("https://www.google.com/m8/feeds/contacts/{0}/full", HttpUtility.UrlEncode(Session["Google:Email"].ToString()));

        // execute the request
        var result = OAuthUtility.ExecuteRequest("GET", url, parameters);

        return Content(Regex.Unescape(result.ToString()), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

    [HttpPost]
    public ActionResult Yandex(string method)
    {
      try
      {
        if (Session["Yandex:AccessToken"] == null)
        {
          throw new Exception(Test.Resources.Strings.SessionIsDead);
        }

        string url = "https://cloud-api.yandex.net/v1/disk/";

        if (method == "disk-resources")
        {
          url = "https://cloud-api.yandex.net/v1/disk/resources?path=/";
        }
        else if (method == "albums")
        {
          url = String.Format("http://api-fotki.yandex.ru/api/users/{0}/albums/", Session["Yandex:UserName"]);
        }

        // execute the request
        var result = OAuthUtility.ExecuteRequest("GET", url, null, String.Format("OAuth {0}", Session["Yandex:AccessToken"]));

        return Content(Regex.Unescape(result.ToString()), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

  }

}