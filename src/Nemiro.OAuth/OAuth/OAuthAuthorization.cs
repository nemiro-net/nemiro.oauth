// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2015. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Nemiro.OAuth.Extensions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents authorization parameters for OAuth 1.0.
  /// </summary>
  public class OAuthAuthorization : HttpAuthorization
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets or sets the consumer key.
    /// </summary>
    public string ConsumerKey
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_consumer_key")) { return null; }

        return base["oauth_consumer_key"].ToString();
      }
      set
      {
        base["oauth_consumer_key"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the consumer secret.
    /// </summary>
    public string ConsumerSecret { get; set; }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    public string Token
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_token")) { return null; }

        return base["oauth_token"].ToString();
      }
      set
      {
        base["oauth_token"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the secret token.
    /// </summary>
    public string TokenSecret { get; set; }

    /// <summary>
    /// Gets or sets the signature method.
    /// </summary>
    public string SignatureMethod
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_signature_method")) { return null; }

        return base["oauth_signature_method"].ToString();
      }
      set
      {
        if (value.Equals(SignatureMethods.PLAINTEXT, StringComparison.OrdinalIgnoreCase))
        {
          base["oauth_signature_method"] = SignatureMethods.PLAINTEXT;
        }
        else if (value.Equals(SignatureMethods.HMACSHA1, StringComparison.OrdinalIgnoreCase))
        {
          base["oauth_signature_method"] = SignatureMethods.HMACSHA1;
        }
        else if (value.Equals(SignatureMethods.RSASHA1, StringComparison.OrdinalIgnoreCase))
        {
          base["oauth_signature_method"] = SignatureMethods.RSASHA1;
        }
        else
        {
          throw new NotSupportedException(String.Format("The {0} signature method is not supported.", value));
        }
      }
    }

    /// <summary>
    /// Gets or sets the nonce.
    /// </summary>
    public string Nonce
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_nonce"))
        {
          base["oauth_nonce"] = OAuthUtility.GetRandomKey();
        }

        return base["oauth_nonce"].ToString();
      }
      set
      {
        base["oauth_nonce"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public string Timestamp
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_timestamp"))
        {
          base["oauth_timestamp"] = OAuthUtility.GetRandomKey();
        }

        return base["oauth_timestamp"].ToString();
      }
      set
      {
        base["oauth_timestamp"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the version of the OAuth.
    /// </summary>
    public string Version
    {
      get
      {
        return base["oauth_version"].ToString();
      }
      set
      {
        base["oauth_version"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the signature.
    /// </summary>
    public string Signature
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_signature")) { return null; }

        return base["oauth_signature"].ToString();
      }
      internal set
      {
        base["oauth_signature"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the callback address.
    /// </summary>
    public string Callback
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_callback")) { return null; }

        return base["oauth_callback"].ToString();
      }
      set
      {
        base["oauth_callback"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the verifier code.
    /// </summary>
    public string Verifier
    {
      get
      {
        if (!base.Value.ContainsKey("oauth_verifier")) { return null; }

        return base["oauth_verifier"].ToString();
      }
      internal set
      {
        base["oauth_verifier"] = value;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthorization"/> class.
    /// </summary>
    public OAuthAuthorization() : base()
    {
      this.AuthorizationType = AuthorizationType.OAuth;
      this.DefaultInit();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthorization"/> class with specific value.
    /// </summary>
    public OAuthAuthorization(UniValue value) : base(AuthorizationType.OAuth, value)
    {
      this.DefaultInit();
    }

    #endregion
    #region ..methods..

    internal void PrepareForRequestToken()
    {
      this.UpdateStamp();
      this.TokenSecret = null;
      base.Remove("oauth_verifier");
      base.Remove("oauth_signature");
      base.Remove("oauth_token");
      base.Remove("oauth_callback");
    }

    internal void PrepareForAccessToken()
    {
      this.UpdateStamp();
      base.Remove("oauth_verifier");
      base.Remove("oauth_signature");
      base.Remove("oauth_token");
    }

    internal override void Build(string httpMethod, string url, string contentType, HttpParameterCollection parameters)
    {
      NameValueCollection param = null;

      if (parameters != null)
      {
        if (!String.IsNullOrEmpty(contentType) && contentType.ToLower().Contains("multipart/form-data"))
        {
          param = ((HttpParameterCollection)parameters.Where(itm => itm.ParameterType == HttpParameterType.Url).ToArray()).ToNameValueCollection();
        }
        else
        {
          param = parameters.ToNameValueCollection();
        }
      }

      this.Signature = new OAuthSignature
      (
        this.SignatureMethod.ToString(),
        String.Format("{0}&{1}", this.ConsumerSecret, this.TokenSecret),
        OAuthAuthorization.GetSignatureBaseString(httpMethod, url, param, this)
      ).ToString();
    }

    /// <summary>
    /// Updates the nonce and the timestamp.
    /// </summary>
    internal void UpdateStamp()
    {
      this.Nonce = OAuthUtility.GetRandomKey();
      this.Timestamp = OAuthUtility.GetTimeStamp();
    }

    private void DefaultInit()
    {
      this.SignatureMethod = SignatureMethods.HMACSHA1;
      this.Timestamp = OAuthUtility.GetTimeStamp();
      this.Nonce = OAuthUtility.GetRandomKey();
      this.Version = "1.0";
    }

    #endregion
    #region ..static methods..

    /// <summary>
    /// Gets base string of the signature for current request (OAuth 1.0).
    /// </summary>
    /// <remarks><para>For more details, please visit <see href="http://tools.ietf.org/html/rfc5849#section-3.4.1.1"/></para></remarks>
    public static string GetSignatureBaseString(string httpMethod, string url, NameValueCollection parameters, OAuthAuthorization authorization)
    {
      return OAuthAuthorization.GetSignatureBaseString(httpMethod, new Uri(url), parameters, authorization);
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

      var param = new NameValueCollection();

      if (parameters != null) { param.Add(parameters); }

      // append the authorization headers
      foreach (KeyValuePair<string, UniValue> itm in authorization.Value.CollectionItems)
      {
        if (itm.Key.Equals("oauth_signature", StringComparison.OrdinalIgnoreCase)) { continue; }
        param.Add(itm.Key, itm.Value.ToString());
      }

      // append the query parameters
      string queryString = url.GetComponents(UriComponents.Query, UriFormat.Unescaped);

      if (!String.IsNullOrEmpty(queryString))
      {
        foreach (string q in queryString.Split('&'))
        {
          string[] p = q.Split('=');
          string key = p.First(), value = (p.Length > 1 ? p.Last() : "");

          param.Add(key, value);
        }
      }

      // sorting and build base string of the signature
      StringBuilder signBaseString = new StringBuilder();

      foreach (var itm in param.Sort().ToKeyValuePairCollection())
      {
        //if (itm.Key.Equals("oauth_verifier", StringComparison.OrdinalIgnoreCase)) { continue; }
        if (signBaseString.Length > 0) { signBaseString.Append(OAuthUtility.UrlEncode("&")); }
        signBaseString.Append(OAuthUtility.UrlEncode(String.Format("{0}={1}", OAuthUtility.UrlEncode(itm.Key), OAuthUtility.UrlEncode(itm.Value))));
      }

      signBaseString.Insert(0, String.Format("{0}&{1}&", httpMethod.ToUpper(), OAuthUtility.UrlEncode(url.ToString())));

      return signBaseString.ToString();
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Creates a new <see cref="OAuthAuthorization"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="OAuthAuthorization"/>.</param>
    public static implicit operator OAuthAuthorization(string value)
    {
      return new OAuthAuthorization(value);
    }

    #endregion
    #region ..obsolete..

    /// <summary>
    /// Sets signature.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="tokenSecret">The token secret.</param>
    /// <param name="parameters">The query parameters.</param>
    /// <param name="applicationSecret">The application secret key obtained from the provider website.</param>
    [Obsolete("No need to call this method. Signature is calculated automatically when execute a request. // v1.5", true)]
    public void SetSignature(string httpMethod, string url, string applicationSecret, string tokenSecret, HttpParameterCollection parameters)
    {
      this.SetSignature(httpMethod, url, applicationSecret, tokenSecret, parameters);
    }

    /// <summary>
    /// Sets signature.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="tokenSecret">The token secret.</param>
    /// <param name="parameters">The query parameters.</param>
    /// <param name="applicationSecret">The application secret key obtained from the provider website.</param>
    [Obsolete("No need to call this method. Signature is calculated automatically when execute a request. // v1.5", true)]
    public void SetSignature(string httpMethod, Uri url, string applicationSecret, string tokenSecret, NameValueCollection parameters)
    {
      this["oauth_signature"] = OAuthUtility.GetSignature(httpMethod, url, applicationSecret, tokenSecret, parameters, this).ToString();
    }

    /// <summary>
    /// Sets signature.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="parameters">The query parameters.</param>
    [Obsolete("No need to call this method. Signature is calculated automatically when execute a request. // v1.5", true)]
    public void SetSignature(string httpMethod, Uri url, NameValueCollection parameters)
    {
      this["oauth_signature"] = OAuthUtility.GetSignature(httpMethod, url, this.ConsumerSecret, this.TokenSecret, parameters, this).ToString();
    }

    /// <summary>
    /// Sets signature.
    /// </summary>
    /// <param name="httpMethod">The HTTP method: <b>GET</b> or <b>POST</b>. Default is <b>POST</b>.</param>
    /// <param name="url">The request URI.</param>
    /// <param name="parameters">The query parameters.</param>
    [Obsolete("No need to call this method. Signature is calculated automatically when execute a request. // v1.5", true)]
    public void SetSignature(string httpMethod, string url, NameValueCollection parameters)
    {
      this.SetSignature(httpMethod, new Uri(url), parameters);
    }

    #endregion

  }

}