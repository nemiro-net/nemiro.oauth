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
    public ActionResult Google(string method)
    {
      try
      {
        if (Session["Google:AccessToken"] == null)
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        // get access token from session
        var token = (OAuth2AccessToken)Session["Google:AccessToken"];

        // set parameters
        var parameters = new NameValueCollection
        { 
          { "access_token", token.Value }
        };

        string url = String.Format("https://www.google.com/m8/feeds/contacts/{0}/full", HttpUtility.UrlEncode(Session["Google:Email"].ToString()));

        // execute the request
        var result = OAuthUtility.ExecuteRequest("GET", url, parameters);

        return Content(result.ToString(), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

  }

}