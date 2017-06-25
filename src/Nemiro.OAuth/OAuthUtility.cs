// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2015, 2017. All rights reserved.
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
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Provides helpers methods for OAuth.
  /// </summary>
  public static class OAuthUtility
  {

    #region ..properties and fields..

    /// <summary>
    /// Unreserved characters for the <see cref="UrlEncode(string)"/> method.
    /// </summary>
    /// <remarks>
    /// http://tools.ietf.org/html/rfc3986#page-13
    /// </remarks>
    internal static readonly string UnreservedCharacters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.~";

    internal static readonly Type[] NumericType = new Type[] 
    { 
      typeof(byte), typeof(sbyte), 
      typeof(short), typeof(int), typeof(long), 
      typeof(ushort), typeof(uint), typeof(ulong),
      typeof(decimal), typeof(double), typeof(float)
    };

    internal static readonly Type[] ExcludedTypeOfClasses = new Type[] { typeof(string), typeof(byte[]) };

    internal static readonly Dictionary<char, string> JavaScriptChars = new Dictionary<char, string>
    {
      { '\'', "\\\'" }, // \u0027
      { '\"', "\\\"" },
      { '\\', "\\\\" },
      { '\b', "\\b" },
      { '\f', "\\f" },
      { '\n', "\\n" },
      { '\r', "\\r" },
      { '\t', "\\t" },
      { '<', "\u003c" },
      { '&', "\u0026" }
    };

    private static readonly string[] HttpHeadersToProperty = 
    {
      "Accept",
      "Connection",
      "Expect",
      "Transfer-Encoding",
      "User-Agent",
      "Connection-Group-Name",
      "Keep-Alive",
      "Content-Type"
    };

    /// <summary>
    /// Version number of Nemiro.OAuth.
    /// </summary>
    private static readonly string LibraryVersion;

    #endregion
    #region ..constructor..

    /// <summary>
    /// This is main helper class.
    /// </summary>
    static OAuthUtility()
    {
      var assembly = System.Reflection.Assembly.GetExecutingAssembly();
      if (assembly != null)
      {
        OAuthUtility.LibraryVersion = assembly.GetName().Version.ToString();
      }
    }

    #endregion
    #region ..methods..

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
        return System.Web.HttpUtility.UrlEncode(value, codePage);
      }
      else
      {
        return value;
      }
    }

    /// <summary>
    /// Encodes a string into JavaScript string.
    /// </summary>
    /// <param name="value">The string to encode.</param>
    public static string JavaScriptStringEncode(string value)
    {
      if (String.IsNullOrEmpty(value)) { return value; }
      // return value.ToString().Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");

      var result = new StringBuilder();

      foreach (char c in value)
      {
        if (OAuthUtility.JavaScriptChars.ContainsKey(c))
        {
          result.Append(OAuthUtility.JavaScriptChars[c]);
        }
        else
        {
          int code = (int)c;
          if (code < 32 || code > 127)
          {
            result.AppendFormat("\\u{0:x4}", code);
          }
          else
          {
            result.Append(c);
          }
        }
      }

      return result.ToString();
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
    /// Performs a request using a <b>GET</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for request.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static RequestResult Get(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, AccessToken accessToken = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      return OAuthUtility.ExecuteRequest("GET", endpoint, parameters, authorization, headers, null, accessToken, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs a request using a <b>POST</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static RequestResult Post(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, AccessToken accessToken = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      return OAuthUtility.ExecuteRequest("POST", endpoint, parameters, authorization, headers, contentType, accessToken, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs a request using a <b>PUT</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to the request.</param>
    /// <param name="parameters">Parameters to be passed to the request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for the request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static RequestResult Put(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, AccessToken accessToken = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      return OAuthUtility.ExecuteRequest("PUT", endpoint, parameters, authorization, headers, contentType, accessToken, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs a request using a <b>DELETE</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to the request.</param>
    /// <param name="parameters">Parameters to be passed to the request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for the request.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static RequestResult Delete(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, AccessToken accessToken = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      return OAuthUtility.ExecuteRequest("DELETE", endpoint, parameters, authorization, headers, null, accessToken, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs a request.
    /// </summary>
    /// <param name="method">HTTP Method: <b>POST</b> (default), <b>PUT</b>, <b>GET</b> or <b>DELETE</b>.</param>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <returns>Returns an instance of the <see cref="RequestResult"/> class, which contains the result of the request.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static RequestResult ExecuteRequest(string method = "POST", string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, AccessToken accessToken = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      // checking
      if (String.IsNullOrEmpty(endpoint)) { throw new ArgumentNullException("endpoint"); }

      if (!AccessToken.IsNullOrEmpty(accessToken) && authorization != null)
      {
        throw new ArgumentException("The request can not contain both authorization headers and access token.");
      }

      // set default values
      if (!String.IsNullOrEmpty(method)) { method = method.ToUpper(); }

      string[] post = { "POST", "PUT" };

      if (String.IsNullOrEmpty(method) || (parameters != null && (parameters.HasFiles || parameters.IsRequestBody) && Array.IndexOf(post, method) == -1))
      {
        method = "POST";
      }

      bool isPost = Array.IndexOf(post, method) != -1;

      // set protocols
      var securityProtocol = ServicePointManager.SecurityProtocol;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

      // ignore errors
      var serverCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback;
      ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
      // --

      // access token
      if (!AccessToken.IsNullOrEmpty(accessToken))
      {
        if (accessToken.GetType() == typeof(OAuth2AccessToken) || accessToken.GetType().IsSubclassOf(typeof(OAuth2AccessToken)))
        {
          // is oauth 2.0
          var token = (OAuth2AccessToken)accessToken;
          if (!String.IsNullOrEmpty(token.TokenType) && (token.TokenType.Equals(AccessTokenType.Bearer, StringComparison.OrdinalIgnoreCase))) //  || token.TokenType.Equals(AccessTokenType.OAuth, StringComparison.OrdinalIgnoreCase)
          {
            // bearer
            authorization = new HttpAuthorization(AuthorizationType.Bearer, accessToken.Value);
          }
          else
          {
            // other
            if (parameters == null) { parameters = new HttpParameterCollection(); }
            parameters.AddUrlParameter("access_token", accessToken.Value);
          }
        }
        else if (accessToken.GetType() == typeof(OAuthAccessToken) || accessToken.GetType().IsSubclassOf(typeof(OAuthAccessToken)))
        {
          // is oauth 1.0
          authorization = new OAuthAuthorization(accessToken);
        }
        else
        {
          // I do not know that. But it's definitely need to consider.
          if (parameters == null) { parameters = new HttpParameterCollection(); }
          parameters.AddUrlParameter("access_token", accessToken.Value);
        }
      }
      // --

      string requestUrl = endpoint; // need source endpoint for signature

      if (!isPost && parameters != null && parameters.Count > 0)
      {
        // set parameters to the URL if the request is executed using the GET method
        requestUrl += (requestUrl.Contains("?") ? "&" : "?");
        requestUrl += parameters.ToStringParameters(donotEncodeKeys);
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
            requestUrl += p.ToStringParameters(donotEncodeKeys);
          }
        }
        else
        {
          // url parameters to endpoint
          HttpParameterCollection p = parameters.Where(itm => itm.ParameterType == HttpParameterType.Url).ToArray();
          if (p.Count > 0)
          {
            requestUrl += (requestUrl.Contains("?") ? "&" : "?");
            requestUrl += p.ToStringParameters(donotEncodeKeys);
          }
        }
      }

      // create request
      var req = (HttpWebRequest)HttpWebRequest.Create(requestUrl);

      // http method
      req.Method = method;

      // req.ProtocolVersion = HttpVersion.Version10;

      // user-agent (required for some providers)
      req.UserAgent = String.Format("Nemiro.OAuth v{0}", OAuthUtility.LibraryVersion);

      // json format acceptable for the response
      req.Accept = "application/json";

      if (isPost)
      {
        req.ContentType = contentType;
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
        for (int i = 0; i < headers.Count; i++)
        {
          if (!OAuthUtility.HttpHeadersToProperty.Contains(headers.Keys[i], StringComparer.OrdinalIgnoreCase))
          {
            continue;
          }

          OAuthUtility.SetHeader(headers.Keys[i], headers, req);
          i--;
        }

        if (headers.Count > 0)
        {
          req.Headers.Add(headers);
        }
      }

      // set parameters to the body if the request is executed using the POST method
      if (isPost)
      {
        if (parameters != null && parameters.Count > 0)
        {
          //req.ContentType = contentType;

          if (contentLength > 0)
          {
            req.ContentLength = contentLength;
          }

          if (parameters.HasFiles || parameters.IsRequestBody || (!String.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) != -1))
          {
            req.AllowWriteStreamBuffering = allowWriteStreamBuffering;
            req.SendChunked = allowSendChunked;
          }

          parameters.WriteToRequestStream(req, writeBufferSize, streamWriteCallback);
        }
        else
        {
          // for some servers
          req.ContentLength = 0;
        }
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
          result = OAuthUtility.ReadResponseStream(resp, readBufferSize);
        }
      }
      catch (WebException ex)
      { 
        // web exception, 
        if (ex.Response != null)
        { 
          // reading contents of the response
          using (var resp = (HttpWebResponse)ex.Response)
          {
            ct = resp.ContentType;
            rh = resp.Headers;
            status = (int)resp.StatusCode;
            result = OAuthUtility.ReadResponseStream(resp, readBufferSize);
          }
        }
        else
        { 
          // no response, get error message
          result = Encoding.UTF8.GetBytes(ex.Message);
        }

        exception = ex;
      }
      catch (Exception ex)
      { 
        // other exception
        result = Encoding.UTF8.GetBytes(ex.Message);
        exception = ex;
      }

      // restore ServicePoint
      ServicePointManager.SecurityProtocol = securityProtocol;
      ServicePointManager.ServerCertificateValidationCallback = serverCertificateValidationCallback;

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
    /// <param name="bufferSize">Size of read buffer. Default: <c>4096</c>.</param>
    private static byte[] ReadResponseStream(HttpWebResponse resp, int bufferSize = 4096)
    {
      if (bufferSize <= 0)
      {
        throw new ArgumentOutOfRangeException("The value of the bufferSize must be greater than zero.");
      }

      using (var s = resp.GetResponseStream())
      using (var sr = new BinaryReader(s, Encoding.UTF8))
      {
        using (var result = new MemoryStream())
        {
          int readed = 0; byte[] buffer = new byte[bufferSize];

          while ((readed = sr.Read(buffer, 0, buffer.Length)) != 0)
          {
            result.Write(buffer, 0, readed);
          }

          return result.ToArray();
        }
      }
    }

    #endregion
    #region async web methods

    /// <summary>
    /// Performs an async request using a <b>GET</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for request.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async request is completed.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static void GetAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, AccessToken accessToken = null, ExecuteRequestAsyncCallback callback = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      OAuthUtility.ExecuteRequestAsync("GET", endpoint, parameters, authorization, headers, null, accessToken, callback, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs an async request using a <b>POST</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async request is completed.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static void PostAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, AccessToken accessToken = null, ExecuteRequestAsyncCallback callback = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      OAuthUtility.ExecuteRequestAsync("POST", endpoint, parameters, authorization, headers, contentType, accessToken, callback, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs an async request using a <b>PUT</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async request is completed.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static void PutAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, AccessToken accessToken = null, ExecuteRequestAsyncCallback callback = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      OAuthUtility.ExecuteRequestAsync("PUT", endpoint, parameters, authorization, headers, contentType, accessToken, callback, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs an async request using a <b>DELETE</b> method.
    /// </summary>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for request.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async request is completed.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static void DeleteAsync(string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, AccessToken accessToken = null, ExecuteRequestAsyncCallback callback = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      OAuthUtility.ExecuteRequestAsync("DELETE", endpoint, parameters, authorization, headers, null, accessToken, callback, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
    }

    /// <summary>
    /// Performs an async request.
    /// </summary>
    /// <param name="method">HTTP Method: <b>POST</b> (default), <b>PUT</b>, <b>GET</b> or <b>DELETE</b>.</param>
    /// <param name="endpoint">URL to which will be sent to request.</param>
    /// <param name="parameters">Parameters to be passed to request.</param>
    /// <param name="authorization">Authorization header value.</param>
    /// <param name="headers">HTTP headers for request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="callback">A delegate that, if provided, is called when an async web request is completed.</param>
    /// <param name="accessToken">Access token to be used in the request.</param>
    /// <param name="streamWriteCallback">A delegate that, if provided, is called when writing a data block to the stream of the request.</param>
    /// <param name="allowSendChunked">The value that indicates whether to send data in segments to the <paramref name="endpoint"/> (for files and request body only). Default: <c>true</c>.</param>
    /// <param name="allowWriteStreamBuffering">The value that indicates whether to buffer the data sent to the <paramref name="endpoint"/> (for files and request body only). Default: <c>false</c>.</param>
    /// <param name="contentLength">The size of the content. Only for files and request body. The size of the content is required if the value of the parameter <paramref name="allowSendChunked"/> equals <c>false</c>.</param>
    /// <param name="readBufferSize">Read buffer size. Default: <c>4096</c>.</param>
    /// <param name="writeBufferSize">Write buffer size. Default: <c>4096</c>.</param>
    /// <param name="donotEncodeKeys">This option allows to disable the encoding of a parameters names. Default: <c>false</c> (to encode names).</param>
    /// <remarks>
    /// <para>Can not be used simultaneously <paramref name="accessToken"/> and <paramref name="authorization"/>. Use only one of these parameters.</para>
    /// <para>
    /// Sending big data is performed in parts. This allows to reduce the amount of consumed memory.
    /// But not all servers support receiving chunked data.
    /// If you have problems, set the parameter <paramref name="allowSendChunked"/> to <c>false</c>.
    /// In this case you have to manually specify the size of the content (<paramref name="contentLength"/>), or enable buffering (<paramref name="allowWriteStreamBuffering"/> = <c>true</c>).
    /// </para>
    /// <para>
    /// The <paramref name="streamWriteCallback"/> call may be from a separate thread. 
    /// In Windows Forms applications should be returned to the main thread, to have access to the form elements.
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="RequestException"></exception>
    /// <exception cref="ArgumentException">
    /// <para>The exception occurs when the query parameters are specified at the same time <paramref name="authorization"/> and <paramref name="accessToken"/>.</para>
    /// </exception>
    public static void ExecuteRequestAsync(string method = "POST", string endpoint = null, HttpParameterCollection parameters = null, HttpAuthorization authorization = null, NameValueCollection headers = null, string contentType = null, AccessToken accessToken = null, ExecuteRequestAsyncCallback callback = null, bool allowWriteStreamBuffering = false, bool allowSendChunked = true, long contentLength = -1, HttpWriteRequestStream streamWriteCallback = null, int writeBufferSize = 4096, int readBufferSize = 4096, bool donotEncodeKeys = false)
    {
      var t = new Thread
      (() =>
        {
          RequestResult result = null;
          try
          {
            result = OAuthUtility.ExecuteRequest(method, endpoint, parameters, authorization, headers, contentType, accessToken, allowWriteStreamBuffering, allowSendChunked, contentLength, streamWriteCallback, writeBufferSize, readBufferSize, donotEncodeKeys);
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
    /// Sets header to request and remove from collections.
    /// </summary>
    /// <param name="name">Header to set.</param>
    /// <param name="headers">Headers collection.</param>
    /// <param name="req">Request.</param>
    private static void SetHeader(string name, NameValueCollection headers, HttpWebRequest req)
    {
      string propertyName = String.Join("", name.ToLower().Split('-').Select(itm => itm.Substring(0, 1).ToUpper() + itm.Substring(1)).ToArray());

      if (propertyName.Equals("KeepAlive"))
      {
        req.GetType().GetProperty(propertyName).SetValue(req, Convert.ToBoolean(headers[name]), null);
      }
      else
      {
        req.GetType().GetProperty(propertyName).SetValue(req, headers[name], null);
      }

      headers.Remove(name);
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
    /// Returns a string containing a number.
    /// </summary>
    /// <param name="value">The value for processing.</param>
    internal static object GetNumber(object value)
    {
      if (OAuthUtility.IsEmpty(value)) { return 0; }
      if (value.GetType() == typeof(UniValue) || value.GetType().IsSubclassOf(typeof(UniValue)))
      {
        if (((UniValue)value).IsBoolean)
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

    #endregion
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