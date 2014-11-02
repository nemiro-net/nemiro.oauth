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
  /// Provides helpers methods for OAuth.
  /// </summary>
  public class OAuthUtility
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
    static OAuthUtility()
    {
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
    /// Converts the string value to its equivalent string representation that is encoded with base-64 digits.
    /// </summary>
    /// <param name="value">A composite format string for encoding to Base64.</param>
    /// <param name="args">An object array that contains zero or more objects to format. </param>
    /// <returns>The string representation, in base 64.</returns>
    public static string ToBase64String(string value, params object[] args)
    {
      return Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format(value, args)));
    }

    /// <summary>
    /// Performs a web request.
    /// </summary>
    /// <param name="method">HTTP Method: <b>POST</b> (default) or <b>GET</b>.</param>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static RequestResult ExecuteRequest(string method = "POST", string endpoint = null, NameValueCollection parameters = null, string authorization = null, NameValueCollection headers = null)
    {
      if (String.IsNullOrEmpty(endpoint)) { throw new ArgumentNullException("endpoint"); }
      if (String.IsNullOrEmpty(method)) { method = "POST"; }

      // set protocols
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
      // ignore errors
      ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
      // --

      // set parameters to the URL if the request is executed using the GET method
      if (method.Equals("GET", StringComparison.OrdinalIgnoreCase) && parameters != null && parameters.Count > 0)
      {
        endpoint += (endpoint.Contains("?") ? "&" : "?");
        endpoint += parameters.ToParametersString("&");
      }

      // create request
      var req = (HttpWebRequest)HttpWebRequest.Create(endpoint);
      
      // http method
      req.Method = method.ToUpper();
      
      // req.ProtocolVersion = HttpVersion.Version10;

      // user-agent (required for some providers)
      req.UserAgent = "Nemiro.OAuth";

      // json format acceptable for the response
      req.Accept = "application/json";

      // set parameters to the body if the request is executed using the POST method
      if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
      {
        req.ContentType = "application/x-www-form-urlencoded";
        if (parameters != null && parameters.Count > 0)
        {
          byte[] b = System.Text.Encoding.ASCII.GetBytes(parameters.ToParametersString("&"));
          // for .NET Framework 2.0/3.0/3.5
          if (Environment.Version.Major < 4)
          {
            req.ContentLength = b.Length;
          }
          // --
          req.GetRequestStream().Write(b, 0, b.Length);
        }
        else
        {
          // for some servers
          req.ContentLength = 0;
        }
      }

      // set authorization header
      if (!String.IsNullOrEmpty(authorization))
      {
        if (headers == null) { headers = new NameValueCollection(); }
        if (String.IsNullOrEmpty(headers["Authorization"]))
        {
          headers.Add("Authorization", authorization);
        }
        else
        {
          headers["Authorization"] = authorization;
        }
      }

      // headers
      if (headers != null)
      {
        req.Headers.Add(headers);
      }

      Exception exception = null;
      string result = "", contentType = "";

      try
      {
        // executes the request
        using (var resp = (HttpWebResponse)req.GetResponse())
        {
          contentType = resp.ContentType;
          result = OAuthUtility.ReadResponseStream(resp);
        }
      }
      catch (WebException ex)
      { // web exception, 
        if (ex.Response != null)
        { // reading contents of the response
          using (var resp = (HttpWebResponse)ex.Response)
          {
            contentType = resp.ContentType;
            result = OAuthUtility.ReadResponseStream(resp);
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

      // exception
      if (exception != null)
      {
        throw new RequestException(contentType, result, exception);
      }

      // result
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

    /// <summary>
    /// Gets the value of the specified <paramref name="key"/>, if the <paramref name="source"/> is a <see cref="System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;"/>.
    /// </summary>
    /// <param name="source">Source of data.</param>
    /// <param name="key">The key is to be obtained.</param>
    public static object GetDictionaryValueOrNull(object source, string key)
    {
      if (source == null || source.GetType() != typeof(Dictionary<string, object>))
      {
        return null;
      }
      var v = source as Dictionary<string, object>;
      if (!v.ContainsKey(key)) { return null; }
      return v[key].ToString();
    }

    /// <summary>
    /// Gets signature for the current request.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="tokenSecret">The token secret.</param>
    /// <param name="parameters">The query parameters.</param>
    /// <param name="applicationSecret">The application secret key obtained from the provider website.</param>
    /// <param name="authorization">The authorization parameters.</param>
    public static OAuthSignature GetSignature(string httpMethod, string url, string applicationSecret, string tokenSecret, NameValueCollection parameters, OAuthAuthorization authorization)
    {
      return OAuthUtility.GetSignature(httpMethod, new Uri(url), applicationSecret, tokenSecret, parameters, authorization);
    }
    /// <summary>
    /// Gets signature for the current request.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="tokenSecret">The token secret.</param>
    /// <param name="parameters">The query parameters.</param>
    /// <param name="applicationSecret">The application secret key obtained from the provider website.</param>
    /// <param name="authorization">The authorization parameters.</param>
    public static OAuthSignature GetSignature(string httpMethod, Uri url, string applicationSecret, string tokenSecret, NameValueCollection parameters, OAuthAuthorization authorization)
    {
      if (authorization == null) { throw new ArgumentNullException("authorization"); }
      return new OAuthSignature
      (
        authorization["oauth_signature_method"].ToString(),
        String.Format("{0}&{1}", applicationSecret, tokenSecret),
        OAuthUtility.GetSignatureBaseString(httpMethod, url, parameters, authorization)
      );
    }

    /// <summary>
    /// Gets base string of the signature for current request (OAuth 1.0).
    /// </summary>
    /// <remarks><para>For more details, please visit <see href="http://tools.ietf.org/html/rfc5849#section-3.4.1.1"/></para></remarks>
    public static string GetSignatureBaseString(string httpMethod, string url, NameValueCollection parameters, OAuthAuthorization authorization)
    {
      return OAuthUtility.GetSignatureBaseString(httpMethod, new Uri(url), parameters, authorization);
    }
    /// <summary>
    /// Gets base string of the signature for current request (OAuth 1.0).
    /// </summary>
    /// <remarks><para>For more details, please visit <see href="http://tools.ietf.org/html/rfc5849#section-3.4.1.1"/></para></remarks>
    public static string GetSignatureBaseString(string httpMethod, Uri url, NameValueCollection parameters, OAuthAuthorization authorization)
    {
      if (String.IsNullOrEmpty(httpMethod)) { throw new ArgumentNullException("httpMethod"); }
      if (authorization == null) { throw new ArgumentNullException("authorization"); }
      if (url == null) { throw new ArgumentNullException("url"); }

      if (parameters == null) { parameters = new NameValueCollection(); }

      // append the authorization headers
      foreach (var itm in authorization.Parameters)
      {
        if (itm.Key.Equals("oauth_signature", StringComparison.OrdinalIgnoreCase)) { continue; }
        parameters.Add(itm.Key, itm.Value.ToString());
      }

      // append the query parameters
      string queryString = url.GetComponents(UriComponents.Query, UriFormat.Unescaped);
      if (!String.IsNullOrEmpty(queryString))
      {
        foreach (string q in queryString.Split('&'))
        {
          string[] p = q.Split('=');
          string key = p.First(), value = (p.Length > 1 ? p.Last() : "");
          parameters.Add(key, value);
        }
      }

      // sorting and build base string of the signature
      StringBuilder signBaseString = new StringBuilder();
      foreach (var itm in parameters.Sort().ToKeyValuePairCollection())
      {
        //if (itm.Key.Equals("oauth_verifier", StringComparison.OrdinalIgnoreCase)) { continue; }
        if (signBaseString.Length > 0) { signBaseString.Append(OAuthUtility.UrlEncode("&")); }
        signBaseString.Append(OAuthUtility.UrlEncode(String.Format("{0}={1}", itm.Key, OAuthUtility.UrlEncode(itm.Value))));
      }

      signBaseString.Insert(0, String.Format("{0}&{1}&", httpMethod.ToUpper(), OAuthUtility.UrlEncode(url.ToString())));

      return signBaseString.ToString();
    }

  }

}