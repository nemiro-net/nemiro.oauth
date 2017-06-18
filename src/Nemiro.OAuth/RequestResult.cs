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
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Security.Permissions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the base class for results of remote requests.
  /// </summary>
  [Serializable]
  public class RequestResult : UniValue
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets or sets the HTTP status code of the output returned to the client.
    /// </summary>
    public int StatusCode { get; protected internal set; }

    /// <summary>
    /// Gets a value indicating whether the current request result is successful or not.
    /// </summary>
    /// <remarks>
    /// <para>Successful result - is a response code from <b>200</b> to <b>299</b>.</para>
    /// </remarks>
    public bool IsSuccessfully
    {
      get
      {
        return this.StatusCode >= 200 && this.StatusCode <= 299;
      }
    }

    /// <summary>
    /// Gets or sets the content type of the response.
    /// </summary>
    public string ContentType { get; protected set; }

    /// <summary>
    /// Gets or sets the http headers of the response.
    /// </summary>
    public NameValueCollection HttpHeaders { get; protected set; }

    /// <summary>
    /// Gets the <c>Content-Disposition</c> header of the response.
    /// </summary>
    public string ContentDisposition
    {
      get
      {
        return this.HttpHeaders["Content-Disposition"];
      }
    }

    /// <summary>
    /// Gets the file name, if <see cref="Result"/> is file.
    /// </summary>
    public string FileName
    {
      get
      {
        if (String.IsNullOrEmpty(this.ContentDisposition)) { return null; }
        return Regex.Match(this.ContentDisposition, @"filename=(?<fn>[^\;]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["fn"].Value;
      }
    }

    /// <summary>
    /// Gets or sets the source of the response.
    /// </summary>
    public byte[] Source { get; protected set; }

    /// <summary>
    /// Gets a value indicating the <see cref="Result"/> is file or not.
    /// </summary>
    public bool IsFile
    {
      get
      {
        return base.IsBinary && !String.IsNullOrEmpty(this.ContentDisposition);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Result"/> is empty or not.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        return !base.HasValue;
      }
    }

    #region [obsolete]

    /// <summary>
    /// Gets or sets the processed result of the response.
    /// </summary>
    [Obsolete("No need. Use current class instance. // v1.5", false)]
    public UniValue Result
    {
      get
      {
        return this;
      }
      protected set
      {
        this.Data = value;
      }
    }

    /// <summary>
    /// Gets a value indicating the <see cref="Result"/> is <see cref="NameValueCollection"/> or not.
    /// </summary>
    [Obsolete("Please use IsCollection. // v1.5", false)]
    public bool IsNameValueCollection
    {
      get
      {
        return base.IsCollection;
      }
    }

    /// <summary>
    /// Gets a value indicating the <see cref="Result"/> is <see cref="Dictionary&lt;TKey, TValue&gt;"/> or not.
    /// </summary>
    [Obsolete("Please use IsCollection. // v1.5", false)]
    public bool IsDictionary
    {
      get
      {
        return base.IsCollection;
      }
    }

    /// <summary>
    /// Gets a value indicating the <see cref="Result"/> is XML or not.
    /// </summary>
    [Obsolete("Please use IsCollection. // v1.5", false)]
    public bool IsXml
    {
      get
      {
        return base.IsCollection;
      }
    }

    /// <summary>
    /// Gets a value indicating the <see cref="Result"/> is array or not.
    /// </summary>
    [Obsolete("Please use IsCollection. // v1.5", false)]
    public bool IsArray
    {
      get
      {
        return base.IsCollection;
      }
    }

    #endregion
    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResult"/> class.
    /// </summary>
    /// <param name="result">The request results.</param>
    public RequestResult(RequestResult result)
    {
      if (result == null)
      {
        throw new ArgumentNullException("result");
      }

      this.Source = result.Source;
      this.Data = result.Data;
      this.ContentType = result.ContentType;
      this.HttpHeaders = result.HttpHeaders;
      this.StatusCode = result.StatusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResult"/> class.
    /// </summary>
    /// <param name="contentType">The content type of the response.</param>
    /// <param name="source">The source of the response.</param>
    public RequestResult(string contentType, string source) : this(contentType, Encoding.UTF8.GetBytes(source ?? ""), null, 0) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResult"/> class.
    /// </summary>
    /// <param name="contentType">The content type of the response.</param>
    /// <param name="source">The source of the response.</param>
    /// <param name="httpHeaders">The HTTP headers of the response.</param>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    public RequestResult(string contentType = null, byte[] source = null, NameValueCollection httpHeaders = null, int statusCode = 0)
    {
      if (String.IsNullOrEmpty(contentType))
      {
        contentType = "text/plain";
      }

      if (contentType.IndexOf(";") != -1)
      {
        contentType = contentType.Substring(0, contentType.IndexOf(";"));
      }

      this.StatusCode = statusCode;
      this.HttpHeaders = httpHeaders;
      this.Source = source;
      this.ContentType = contentType;

      this.ParseSource();
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Parses the source to the <see cref="Result"/>.
    /// </summary>
    private void ParseSource()
    {
      if (this.Source == null || this.Source.Length <= 0) { return; }
      switch (this.ContentType.ToLower())
      {
        case "text/json":
        case "text/javascript":
        case "application/json":
        case "application/javascript":
          this.Data = UniValue.ParseJson(Encoding.UTF8.GetString(this.Source)).Data;
          break;

        case "text/xml":
        case "application/xml":
        case "application/atom+xml":
        case "application/atomsvc+xml":
          this.Data = UniValue.ParseXml(Encoding.UTF8.GetString(this.Source)).Data;
          break;

        case "text/html":
        case "text/plain":
        case "application/x-www-form-urlencoded": // for some cases
          UniValue r = UniValue.Empty;
          if (UniValue.TryParseParameters(Encoding.UTF8.GetString(this.Source), out r))
          {
            this.Data = r.Data;
          }
          else
          {
            this.Data = UniValue.Create(Encoding.UTF8.GetString(this.Source)).Data;
          }
          break;

        default:
          this.Data = UniValue.Create(this.Source).Data;
          break;
      }
    }

    #endregion
    #region ..iserializable..

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResult"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected RequestResult(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      this.StatusCode = info.GetInt32("StatusCode");
      this.ContentType = info.GetString("ContentType");
      this.HttpHeaders = (NameValueCollection)info.GetValue("HttpHeaders", typeof(NameValueCollection));
      this.Source = (byte[])info.GetValue("Source", typeof(byte[]));
    }

    /// <summary>
    /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
      info.AddValue("StatusCode", this.StatusCode);
      info.AddValue("ContentType", this.ContentType);
      info.AddValue("HttpHeaders", this.HttpHeaders);
      info.AddValue("Source", this.Source);
      base.GetObjectData(info, context);
    }

    #endregion
    #region ..operators..
    /*
    public static explicit operator byte[](RequestResult value)
    {
      return value.Source;
    }

    public static explicit operator Stream(RequestResult value)
    {
      return new MemoryStream(value.Source);
    }
    */
    #endregion

  }

}