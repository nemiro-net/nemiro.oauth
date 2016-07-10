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
    public ActionResult CodeProject(string method)
    {
      try
      {
        if (Session["CodeProject:AccessToken"] == null)
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        var token = (OAuth2AccessToken)Session["CodeProject:AccessToken"];

        // execute the request
        var result = OAuthUtility.Get
        (
          "https://api.codeproject.com/v1/Articles",
          new HttpParameterCollection
          { 
            { "tags" , "asp.net,c#,vb.net" }
          },
          authorization: new HttpAuthorization(AuthorizationType.Bearer, token.Value)
        );

        return Content(result.ToString(), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

  }

}