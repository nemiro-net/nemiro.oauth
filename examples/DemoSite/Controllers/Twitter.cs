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

namespace DemoOAuth.Controllers
{

  public partial class ApiController
  {

    [HttpPost]
    public ActionResult Twitter(string method)
    {
      try
      {
        if (Session["Twitter:AccessToken"] == null)
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        var token = (OAuthAccessToken)Session["Twitter:AccessToken"];

        string url = "https://api.twitter.com/1.1/followers/ids.json";

        if (method == "user_timeline")
        {
          url = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        }

        var auth = new OAuthAuthorization();
        auth.ConsumerKey = ConfigurationManager.AppSettings["oauth:twitter:id"];
        auth.ConsumerSecret = ConfigurationManager.AppSettings["oauth:twitter:key"];
        auth.TokenSecret = token.TokenSecret;
        auth.SignatureMethod = SignatureMethods.HMACSHA1;
        auth.Nonce = OAuthUtility.GetRandomKey();
        auth.Timestamp = OAuthUtility.GetTimeStamp();
        auth.Token = token.Value;
        auth.Version = "1.0";

        var result = OAuthUtility.ExecuteRequest("GET", url, authorization: auth);

        return Content(result.ToString(), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

  }

}