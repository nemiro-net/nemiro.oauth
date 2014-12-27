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
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

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
    internal static readonly string UnreservedCharacters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.~";

    internal static readonly Type[] NumericType = new Type[] 
    { 
      typeof(Byte), typeof(SByte), 
      typeof(Int16), typeof(Int32), typeof(Int64), 
      typeof(UInt16), typeof(UInt32), typeof(UInt64),
      typeof(Decimal), typeof(Double), typeof(Single)
    };

    internal static readonly Type[] ExcludedTypeOfClasses = new Type[] { typeof(string), typeof(byte[]) };

    /// <summary>
    /// This is main helper class.
    /// </summary>
    static OAuthUtility()
    {
    }

    /// <summary>
    /// Percent encoding.
    /// </summary>
    /// <param name="value">The text to encode.</param>
    /// <param name="codePage">The <see cref="System.Text.Encoding"/> object that specifies the encoding scheme. </param>
    /// <remarks>
    /// <para>For more details, please see:</para>
    /// <list type="bullet">
    /// <item><description><see href="http://en.wikipedia.org/wiki/Percent-encoding"/></description></item>
    /// <item><description><see href="http://tools.ietf.org/html/rfc3986"/></description></item>
    /// </list>
    /// </remarks>
    public static string PercentEncode(string value, Encoding codePage)
    {
      if (String.IsNullOrEmpty(value)) { return String.Empty; }
      return String.Join
      (
        "",
        value.Select
        (
          ch => 
          {
            if (UnreservedCharacters.IndexOf(ch) != -1) { return ch.ToString(); }
            if (codePage.IsSingleByte)
            {
              return String.Format("%{0:X2}", (int)ch);
            }
            else
            {
              return String.Join
              (
                "", 
                codePage.GetBytes(new char[] { ch }).Select(b => String.Format("%{0:X2}", b)).ToArray()
              );
            }
          }
        ).ToArray()
      );
    }

    /// <summary>
    /// Percent encoding.
    /// </summary>
    /// <param name="value">The text to encode.</param>
    public static string UrlEncode(string value)
    {
      return OAuthUtility.UrlEncode(value, UrlEncodingType.PercentEncoding, Encoding.UTF8);
    }

    /// <summary>
    /// Encodes a <b>URL</b> string using the specified encoding object.
    /// </summary>
    /// <param name="value">The text to encode.</param>
    /// <param name="encodingType">The type of the encoder.</param>
    public static string UrlEncode(string value, UrlEncodingType encodingType)
    {
      return OAuthUtility.UrlEncode(value, encodingType, Encoding.UTF8);
    }
    
    /// <summary>
    /// Encodes a <b>URL</b> string using the specified encoding object.
    /// </summary>
    /// <param name="value">The text to encode.</param>
    /// <param name="codePage">The <see cref="System.Text.Encoding"/> object that specifies the encoding scheme. </param>
    /// <param name="encodingType">The type of the encoder.</param>
    public static string UrlEncode(string value, UrlEncodingType encodingType, Encoding codePage)
    {
      if (String.IsNullOrEmpty(value)) { return String.Empty; }
      if (encodingType == UrlEncodingType.PercentEncoding)
      {
        return OAuthUtility.PercentEncode(value, codePage);
      }
      else if (encodingType == UrlEncodingType.Default)
      {
        return HttpUtility.UrlEncode(value, codePage);
      }
      else
      {
        return value;
      }
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

    #region web methods

    /// <summary>
    /// Performs a web request using a <b>GET</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static RequestResult Get(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null)
    {
      return OAuthUtility.ExecuteRequest("GET", endpoint, parameters, authorization, headers, null);
    }

    /// <summary>
    /// Performs a web request using a <b>POST</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static RequestResult Post(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null)
    {
      return OAuthUtility.ExecuteRequest("POST", endpoint, parameters, authorization, headers, contentType);
    }

    /// <summary>
    /// Performs a web request using a <b>PUT</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static RequestResult Put(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null)
    {
      return OAuthUtility.ExecuteRequest("PUT", endpoint, parameters, authorization, headers, contentType);
    }

    /// <summary>
    /// Performs a web request using a <b>DELETE</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static RequestResult Delete(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null)
    {
      return OAuthUtility.ExecuteRequest("DELETE", endpoint, parameters, authorization, headers, null);
    }

    /// <summary>
    /// Performs a web request.
    /// </summary>
    /// <param name="method">HTTP Method: <b>POST</b> (default), <b>PUT</b>, <b>GET</b> or <b>DELETE</b>.</param>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static RequestResult ExecuteRequest(string method = "POST", string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null)
    {
      if (String.IsNullOrEmpty(endpoint)) { throw new ArgumentNullException("endpoint"); }
      if (!String.IsNullOrEmpty(method)) { method = method.ToUpper(); }
      string[] post = { "POST", "PUT" };
      if (String.IsNullOrEmpty(method) || (parameters != null &&  (parameters.HasFiles || parameters.IsRequestBody) && Array.IndexOf(post, method) == -1))
      {
        method = "POST";
      }
      bool isPost = Array.IndexOf(post, method) != -1;

      // set protocols
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; // | SecurityProtocolType.Ssl3;
      // ignore errors
      ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
      // --

      string requestUrl = endpoint; // need source endpoint for signature

      if (!isPost && parameters != null && parameters.Count > 0)
      {
        // set parameters to the URL if the request is executed using the GET method
        requestUrl += (requestUrl.Contains("?") ? "&" : "?");
        requestUrl += parameters.ToStringParameters();
      }
      else if (isPost && parameters != null && parameters.Count > 0)
      {
        // is POST or PUT method
        if (parameters.IsRequestBody)
        {
          // all parameters to url
          HttpParameterCollection p = parameters.Where(itm => itm.ParameterType != HttpParameterType.RequestBody).ToArray();
          if (p.Count > 0)
          {
            requestUrl += (requestUrl.Contains("?") ? "&" : "?");
            requestUrl += p.ToStringParameters();
          }
        }
        else
        {
          // url parameters to endpoint
          HttpParameterCollection p = parameters.Where(itm => itm.ParameterType == HttpParameterType.Url).ToArray();
          if (p.Count > 0)
          {
            requestUrl += (requestUrl.Contains("?") ? "&" : "?");
            requestUrl += p.ToStringParameters();
          }
        }
      }

      // create request
      var req = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
      
      // http method
      req.Method = method;
      
      // req.ProtocolVersion = HttpVersion.Version10;

      // user-agent (required for some providers)
      req.UserAgent = "Nemiro.OAuth";

      // json format acceptable for the response
      req.Accept = "application/json";
      
      // set parameters to the body if the request is executed using the POST method
      if (isPost)
      {
        if (parameters != null && parameters.Count > 0)
        {
          req.ContentType = contentType;
          parameters.WriteToRequestStream(req);
        }
        else
        {
          // for some servers
          req.ContentLength = 0;
        }
      }

      // set authorization header
      if (authorization != null)
      {
        // build authorization
        authorization.Build(method, endpoint, req.ContentType, parameters);
        // add authorization to headers
        if (headers == null) { headers = new NameValueCollection(); }
        if (String.IsNullOrEmpty(headers["Authorization"]))
        {
          headers.Add("Authorization", authorization.ToString());
        }
        else
        {
          headers["Authorization"] = authorization.ToString();
        }
      }

      // headers
      if (headers != null)
      {
        req.Headers.Add(headers);
      }

      WebHeaderCollection rh = null;
      Exception exception = null;
      string ct = "";
      int status = 0;
      byte[] result;
      
      try
      {
        // executes the request
        using (var resp = (HttpWebResponse)req.GetResponse())
        {
          ct = resp.ContentType;
          rh = resp.Headers;
          status = (int)resp.StatusCode;
          result = OAuthUtility.ReadResponseStream(resp);
        }
      }
      catch (WebException ex)
      { // web exception, 
        if (ex.Response != null)
        { // reading contents of the response
          using (var resp = (HttpWebResponse)ex.Response)
          {
            ct = resp.ContentType;
            rh = resp.Headers;
            status = (int)resp.StatusCode;
            result = OAuthUtility.ReadResponseStream(resp);
          }
        }
        else
        { // no response, get error message
          result = Encoding.UTF8.GetBytes(ex.Message);
        }
        exception = ex;
      }
      catch (Exception ex)
      { // other exception
        result = Encoding.UTF8.GetBytes(ex.Message);
        exception = ex;
      }

      // exception
      if (exception != null)
      {
        throw new RequestException(ct, result, exception, rh, status);
      }
       
      // result
      return new RequestResult(ct, result, rh, status);
    }

    /// <summary>
    /// Reads results of the web request to the string.
    /// </summary>
    /// <param name="resp"><see cref="System.Net.HttpWebResponse"/> instance.</param>
    private static byte[] ReadResponseStream(HttpWebResponse resp)
    {
      using (Stream s = resp.GetResponseStream())
      {
        using (BinaryReader sr = new BinaryReader(s, Encoding.UTF8))
        {
          using (MemoryStream result = new MemoryStream())
          {
            int readed = 0; byte[] buffer = new byte[4095];
            while ((readed = sr.Read(buffer, 0, buffer.Length)) != 0)
            {
              result.Write(buffer, 0, readed);
            }
            return result.ToArray();
          }
        }
      }
    }
    
    #endregion
    #region async web methods

    /// <summary>
    /// Performs an async web request using a <b>GET</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async web request is completed.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static void GetAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, ExecuteRequestAsyncCallback callback = null)
    {
      OAuthUtility.ExecuteRequestAsync("GET", endpoint, parameters, authorization, headers, null, callback);
    }

    /// <summary>
    /// Performs an async web request using a <b>POST</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async web request is completed.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static void PostAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, ExecuteRequestAsyncCallback callback = null)
    {
      OAuthUtility.ExecuteRequestAsync("POST", endpoint, parameters, authorization, headers, contentType, callback);
    }

    /// <summary>
    /// Performs an async web request using a <b>PUT</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async web request is completed.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static void PutAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, ExecuteRequestAsyncCallback callback = null)
    {
      OAuthUtility.ExecuteRequestAsync("PUT", endpoint, parameters, authorization, headers, contentType, callback);
    }

    /// <summary>
    /// Performs an async web request using a <b>DELETE</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async web request is completed.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static void DeleteAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, ExecuteRequestAsyncCallback callback = null)
    {
      OAuthUtility.ExecuteRequestAsync("DELETE", endpoint, parameters, authorization, headers, null, callback);
    }

    /// <summary>
    /// Performs an async web request.
    /// </summary>
    /// <param name="method">HTTP Method: <b>POST</b> (default), <b>PUT</b>, <b>GET</b> or <b>DELETE</b>.</param>
    /// <param name="endpoint">URL to which will be sent to web request.</param>
    /// <param name="parameters">Parameters to be passed to web request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async web request is completed.</param>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the web request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    public static void ExecuteRequestAsync(string method = "POST", string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, ExecuteRequestAsyncCallback callback = null)
    {
      var t = new Thread
      (() =>
      {
        RequestResult result = null;
        try
        {
          result = OAuthUtility.ExecuteRequest(method, endpoint, parameters, authorization, headers, contentType);
        }
        catch (RequestException ex)
        {
          result = ex.RequestResult;
        }
        if (callback != null)
        {
          callback(result);
        }
      }
      );
      t.IsBackground = true;
      t.Start();
    }

    #endregion

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
    /// Returns a string containing a number.
    /// </summary>
    /// <param name="value">The value for processing.</param>
    internal static object GetNumber(object value)
    {
      if (OAuthUtility.IsEmpty(value)) { return 0; }
      if (value.GetType() == typeof(UniValue) || value.GetType().IsSubclassOf(typeof(UniValue)))
      {
        if(((UniValue)value).IsBoolean)
        {
          return Convert.ToBoolean(value);
        }
      }
      string str = value.ToString();
      if (Regex.IsMatch(value.ToString(), @",|\."))
      {
        return Convert.ToDecimal(Regex.Replace(Regex.Replace(str, @",|\.", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator), @"\s+", ""));
      }
      else
      {
        int int32;
        if (Int32.TryParse(str, out int32))
        {
          return int32;
        }
        long int64;
        if (Int64.TryParse(str, out int64))
        {
          return int64;
        }
        return Regex.Replace(str, @"\s+", "");
      }
    }

    internal static bool IsNumeric(object value)
    {
      if (OAuthUtility.IsEmpty(value)) { return false; }
      if (value.GetType() == typeof(UniValue) || value.GetType().IsSubclassOf(typeof(UniValue))) { return ((UniValue)value).IsNumeric; }
      return Regex.IsMatch(value.ToString(), "^[0-9]+$", RegexOptions.Singleline);
    }

    internal static bool IsEmpty(object value)
    {
      if (value == DBNull.Value || value == null) { return true; }
      if (value.GetType() == typeof(string)) { return String.IsNullOrEmpty(value.ToString()); }
      if (value.GetType() == typeof(UniValue) || value.GetType().IsSubclassOf(typeof(UniValue))) { return UniValue.IsNullOrEmpty((UniValue)value); }
      return false;
    }

    #region [obsolete]

    /// <summary>
    /// Gets signature for the current request.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="tokenSecret">The token secret.</param>
    /// <param name="parameters">The query parameters.</param>
    /// <param name="applicationSecret">The application secret key obtained from the provider website.</param>
    /// <param name="authorization">The authorization parameters.</param>
    [Obsolete("Is no longer used. // v1.5", false)]
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
    [Obsolete("Is no longer used. // v1.5", false)]
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
    [Obsolete("Use a similar method in the OAuthAuthorization class. // v1.5", false)]
    public static string GetSignatureBaseString(string httpMethod, string url, NameValueCollection parameters, OAuthAuthorization authorization)
    {
      return OAuthAuthorization.GetSignatureBaseString(httpMethod, new Uri(url), parameters, authorization);
    }
    /// <summary>
    /// Gets base string of the signature for current request (OAuth 1.0).
    /// </summary>
    /// <remarks><para>For more details, please visit <see href="http://tools.ietf.org/html/rfc5849#section-3.4.1.1"/></para></remarks>
    [Obsolete("Use a similar method in the OAuthAuthorization class. // v1.5", false)]
    public static string GetSignatureBaseString(string httpMethod, Uri url, NameValueCollection parameters, OAuthAuthorization authorization)
    {
      return OAuthAuthorization.GetSignatureBaseString(httpMethod, url, parameters, authorization);
    }

    #endregion

  }

}