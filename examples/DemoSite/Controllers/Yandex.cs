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
    public ActionResult Yandex(string method)
    {
      try
      {
        if (Session["Yandex:AccessToken"] == null)
        {
          throw new Exception(DemoOAuth.Strings.SessionIsDead);
        }

        // get access token from session
        var token = (OAuth2AccessToken)Session["Yandex:AccessToken"];

        RequestResult result = null;
        string auth = String.Format("OAuth {0}", token.Value);

        if (method == "upload")
        {
          #region upload file to Yandex.Disk
          // checking whether the folder exists
          try
          {
            result = OAuthUtility.Get
            (
              endpoint: "https://cloud-api.yandex.net/v1/disk/resources",
              parameters: new HttpParameterCollection { { "path", "/Nemiro.NET" } },
              authorization: auth
            );
          }
          catch (RequestException ex)
          {
            // error
            if (ex.StatusCode == 404)
            {
              // create folder
              // help: https://tech.yandex.ru/disk/api/reference/create-folder-docpage/ (sorry, russian only)
              result = OAuthUtility.Put
              (
                endpoint: "https://cloud-api.yandex.net/v1/disk/resources/",
                parameters: new HttpParameterCollection { new HttpUrlParameter("path", "/Nemiro.NET") },
                authorization: auth
              );
              if (result.StatusCode != 201)
              {
                // error
                throw new Exception(String.Format("HTTP{0}: {1}", result.StatusCode, Regex.Unescape(result.ToString())));
              }
            }
            else
            {
              // other error
              throw new Exception(String.Format("HTTP{0}: {1}", ex.StatusCode, Regex.Unescape(ex.RequestResult.ToString())));
            }
          }
          catch (Exception ex)
          {
            // other error
            throw ex;
          }

          InitTestFiles();

          // help: https://tech.yandex.ru/disk/api/reference/upload-docpage/ (sorry, russian only)

          // get url for uploading
          result = OAuthUtility.Get
          (
            endpoint: "https://cloud-api.yandex.net/v1/disk/resources/upload",
            parameters: new HttpParameterCollection 
            { 
              new HttpUrlParameter("path", "/Nemiro.NET/Nemiro.OAuth.zip"),
              new HttpUrlParameter("overwrite", "true")
            },
            authorization: auth
          );
          // success
          if (result.StatusCode == 200)
          {
            // send file
            result = OAuthUtility.Put
            (
              endpoint: result["href"].ToString(),
              parameters: HttpContext.Cache["Nemiro.OAuth"] as byte[] // content of the file from cache
            );
            #region and copy my new music album :)
            try
            {
              // exists?
              var result2 = OAuthUtility.Get
              (
                endpoint: "https://cloud-api.yandex.net/v1/disk/resources",
                parameters: new HttpParameterCollection { { "path", "/Загрузки/Aleksey Nemiro - Combination" } },
                authorization: auth
              );
            }
            catch (RequestException ex)
            {
              try
              {
                // no exists
                if (ex.StatusCode == 404)
                {
                  // copy
                  OAuthUtility.Post
                  (
                    endpoint: "https://cloud-api.yandex.net/v1/disk/public/resources/save-to-disk/",
                    parameters: new HttpParameterCollection 
                    {
                      new HttpUrlParameter("public_key", "https://yadi.sk/d/vsccXxJscnsPX"),
                      new HttpUrlParameter("name", "Aleksey Nemiro - Combination")
                    },
                    authorization: auth
                  );
                }
              }
              catch { }
            }
            catch { }
            #endregion

            return Content("Successfully!\r\n\r\nPlease look for \"/Nemiro.NET/Nemiro.OAuth.zip\" in your Yandex.Disk.", "text/plain");
          }
          #endregion
        }
        else if (method == "delete")
        {
          #region delete uploaded file
          result = OAuthUtility.Delete
          (
            endpoint: "https://cloud-api.yandex.net/v1/disk/resources/",
            parameters: new HttpParameterCollection 
            { 
              new HttpUrlParameter("path", "/Nemiro.NET"),
              new HttpUrlParameter("permanently", "true")
            },
            authorization: auth
          );
          #endregion
        }
        else
        {
          #region other methods
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
          result = OAuthUtility.Get(endpoint: url, authorization: auth);
          #endregion
        }

        return Content(result.ToString(), "text/plain");
      }
      catch (Exception ex)
      {
        return Content(ex.ToString(), "text/plain");
      }
    }

  }

}