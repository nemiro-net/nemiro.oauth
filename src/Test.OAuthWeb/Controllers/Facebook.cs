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

namespace Test.OAuthWeb.Controllers
{

  public partial class ApiController
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

  }

}