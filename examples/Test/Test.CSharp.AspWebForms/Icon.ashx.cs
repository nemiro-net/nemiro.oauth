using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.CSharp.AspWebForms
{
  /// <summary>
  /// Сводное описание для Icon
  /// </summary>
  public class Icon : IHttpHandler
  {

    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "image/png";
      byte[] buffer = null;
      try
      {
        buffer = (byte[])Test.Resources.Images.ResourceManager.GetObject(context.Request.QueryString["id"].ToLower().Replace(".", ""));
      }
      catch
      {
        buffer = (byte[])Test.Resources.Images.ResourceManager.GetObject("error");
      }
      context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    public bool IsReusable
    {
      get
      {
        return false;
      }
    }
  }
}