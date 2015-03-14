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
        
        // get access token from session
        var token = (OAuth2AccessToken)Session["VK:AccessToken"];

        // query parameters
        var parameters = new NameValueCollection
        { 
          { "access_token" , token.Value }
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

  }

}