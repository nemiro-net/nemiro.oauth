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
    public ActionResult Dropbox(string method)
    {
      try
      {
        if (Session["Dropbox:AccessToken"] == null)
        {
          throw new Exception(Test.Resources.Strings.SessionIsDead);
        }

        InitTestFiles();

        var token = (OAuth2AccessToken)Session["Dropbox:AccessToken"];

        // execute the request
        var result = OAuthUtility.ExecuteRequest
        (
          "PUT",
          "https://api-content.dropbox.com/1/files_put/auto/Nemiro.OAuth.zip",
          new HttpParameterCollection
          { 
            { "access_token" , token.Value },
            { "overwrite", "true" },
            { HttpContext.Cache["Nemiro.OAuth"] } // content of the file from cache
          }
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