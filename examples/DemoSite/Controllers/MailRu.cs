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
    public ActionResult MailRu(string method)
    {
      try
      {
        if (Session["Mail.Ru:AccessToken"] == null)
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        // get access token from session
        var token = (OAuth2AccessToken)Session["Mail.Ru:AccessToken"];

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

        return Content(result.ToString(), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

  }

}