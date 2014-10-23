// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------
using System;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Collections.Generic;
using Nemiro.OAuth.Extensions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// The main helper class.
  /// </summary>
  public class Helpers
  {

    /// <summary>
    /// Unreserved characters for the <see cref="UrlEncode(string)"/> method.
    /// </summary>
    /// <remarks>
    /// http://tools.ietf.org/html/rfc3986#page-13
    /// </remarks>
    protected const string UnreservedCharacters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.~";

    /// <summary>
    /// This is helper class.
    /// </summary>
    static Helpers()
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
      // ignore errors
      ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; 
    }

    /// <summary>
    /// Percent encoding (only for OAuth 1.0).
    /// </summary>
    /// <param name="value">Value for encoding.</param>
    /// <remarks>
    /// <para>For more details, please see:</para>
    /// <list type="bullet">
    /// <item><description><see href="http://en.wikipedia.org/wiki/Percent-encoding"/></description></item>
    /// <item><description><see href="http://tools.ietf.org/html/rfc3986"/></description></item>
    /// </list>
    /// </remarks>
    public static string UrlEncode(string value)
    {
      if (String.IsNullOrEmpty(value)) { return String.Empty; }
      return String.Join
      (
        "",
        value.Select
        (
          ch => UnreservedCharacters.IndexOf(ch) == -1 ? String.Format("%{0:X2}", (int)ch) : ch.ToString()
        ).ToArray()
      );
    }

    /// <summary>
    /// Generate timestamp for a signature (only for OAuth 1.0).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The timestamp value MUST be a positive integer.  Unless otherwise
    /// specified by the server's documentation, the timestamp is expressed
    /// in the number of seconds since January 1, 1970 00:00:00 GMT.
    /// </para>
    /// <para>For more details, please see: <see href="http://tools.ietf.org/html/rfc5849#section-3.3"/></para>
    /// </remarks>
    public static string GetTimeStamp()
    {
      return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString("0");
    }

    /// <summary>
    /// Generate random key.
    /// </summary>
    public static string GetRandomKey()
    {
      return Guid.NewGuid().ToString().Replace("-", "");
    }
    
    /// <summary>
    /// Compute MD5 hash.
    /// </summary>
    /// <param name="value">Value that must be processed.</param>
    public static string GetMD5Hash(string value)
    {
      return String.Join
      (
        "",
        new MD5CryptoServiceProvider().ComputeHash
        (
          Encoding.UTF8.GetBytes(value)
        ).Select
        (
          b => b.ToString("x2")
        ).ToArray()
      );
    }

    /// <summary>
    /// Performs a web request.
    /// </summary>
    /// <param name="httpMethod">HTTP Method: <b>POST</b> (default) or <b>GET</b>.</param>
    /// <param name="url">URL to which will be sent to the web request.</param>
    /// <param name="parameters">Parameters to be passed to the web request.</param>
    /// <param name="authorization">Authorization header value (only for OAuth 1.0).</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static RequestResult ExecuteRequest(string httpMethod, string url, NameValueCollection parameters, string authorization = null)
    {
      if (String.IsNullOrEmpty(url)) { throw new ArgumentNullException("url"); }
      if (String.IsNullOrEmpty(httpMethod)) { httpMethod = "POST"; }

      // set parameters to the URL if the request is executed using the GET method
      if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase) && parameters != null && parameters.Count > 0)
      {
        url += (url.Contains("?") ? "&" : "?");
        url += parameters.ToParametersString("&");
      }

      // create request
      var req = (HttpWebRequest)HttpWebRequest.Create(url);

      // http method
      req.Method = httpMethod.ToUpper();

      // req.ProtocolVersion = HttpVersion.Version10;

      // user-agent (required for some providers)
      req.UserAgent = "Nemiro.OAuth";

      // json format acceptable for the response
      req.Accept = "application/json";

      // set parameters to the body if the request is executed using the POST method
      if (httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
      {
        req.ContentType = "application/x-www-form-urlencoded";
        if (parameters != null && parameters.Count > 0)
        {
          byte[] b = System.Text.Encoding.ASCII.GetBytes(parameters.ToParametersString("&"));
          // Patch for .NET Framework 2.0/3.0/3.5
          if (Environment.Version.Major < 4)
          {
            req.ContentLength = b.Length;
          }
          // --
          req.GetRequestStream().Write(b, 0, b.Length);
        }
      }

      // set authorization header
      if (!String.IsNullOrEmpty(authorization))
      {
        req.Headers.Add("Authorization", authorization);
      }

      Exception exception = null;
      string result = "", contentType = "";

      try
      {
        // execute request
        using (var resp = (HttpWebResponse)req.GetResponse())
        {
          contentType = resp.ContentType;
          result = Helpers.ReadResponseStream(resp);
        }
      }
      catch (WebException ex)
      { // web exception, 
        if (ex.Response != null)
        { // reading contents of the response
          using (var resp = (HttpWebResponse)ex.Response)
          {
            contentType = resp.ContentType;
            result = Helpers.ReadResponseStream(resp);
          }
        }
        else
        { // no response, get error message
          result = ex.Message;
        }
        exception = ex;
      }
      catch (Exception ex)
      { // other exception
        result = ex.Message;
        exception = ex;
      }

      if (exception != null)
      {
        throw new RequestException(contentType, result, exception);
      }

      return new RequestResult(contentType, result);
    }

    /// <summary>
    /// Reads results of the web request to the string.
    /// </summary>
    /// <param name="resp"><see cref="System.Net.HttpWebResponse"/> instance.</param>
    private static string ReadResponseStream(HttpWebResponse resp)
    {
      using (var s = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
      {
        return s.ReadToEnd();
      }
    }

  }

}