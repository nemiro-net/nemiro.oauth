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
using System.IO;
using System.Net;
using System.Threading;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Collection of HTTP parameters.
  /// </summary>
  public class HttpParameterCollection : List<HttpParameter>
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets a value indicating whether the current collection has a files.
    /// </summary>
    public bool HasFiles { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether the current collection has a <see cref="HttpRequestBody"/>.
    /// </summary>
    public bool IsRequestBody { get; protected set; }

    /// <summary>
    /// Gets a boundary for a request.
    /// </summary>
    public string Boundary { get; protected set; }

    /// <summary>
    /// Gets or sets code page for parameters encoding.
    /// </summary>
    public Encoding Encoding { get; set; }

    //public UrlEncodingType UrlEncodingType { get; set; }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterCollection"/> class.
    /// </summary>
    public HttpParameterCollection()
    {
      //this.UrlEncodingType = OAuth.UrlEncodingType.Auto;
      this.Encoding = Encoding.UTF8;
      this.Boundary = "----------" + DateTime.Now.Ticks.ToString("x");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterCollection"/> class.
    /// </summary>
    /// <param name="source">The collection of parameters.</param>
    /// <remarks>
    /// <para>If the <paramref name="source"/> parameter has a value of <b>null</b> (<b>Nothing</b> for VB), it will not be added to the collection.</para>
    /// </remarks>
    public HttpParameterCollection(NameValueCollection source) : this()
    {
      if (source == null) { return; }
      for (int i = 0; i <= source.Count - 1; i++)
      {
        this.Add(new HttpParameter(source.Keys[i], source[i]));
      }
    }

    /*
    /// <summary>
    /// Initializes a new instance of the <see cref="Dictionary&lt;TKey, TVvalue&gt;"/> class.
    /// </summary>
    /// <param name="source">The collection of parameters.</param>
    /// <remarks>
    /// <para>If the <paramref name="source"/> parameter has a value of <b>null</b> (<b>Nothing</b> for VB), it will not be added to the collection.</para>
    /// </remarks>
    public HttpParameterCollection(Dictionary<string, object> source) : this()
    {
      if (source == null) { return; }
      foreach (string key in source.Keys)
      {
        this.Add(new HttpParameter(key, (source[key] != null ? source[key].ToString() : "")));
      }
    }
    */

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterCollection"/> class.
    /// </summary>
    /// <param name="source">The body of a request.</param>
    /// <remarks>
    /// <para>If the <paramref name="source"/> parameter has a value of <b>null</b> (<b>Nothing</b> for VB), it will not be added to the collection.</para>
    /// </remarks>
    public HttpParameterCollection(byte[] source) : this()
    {
      if (source == null) { return; }
      this.Add(new HttpRequestBody(source));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterCollection"/> class.
    /// </summary>
    /// <param name="source">The body of a request.</param>
    /// <remarks>
    /// <para>If the <paramref name="source"/> parameter has a value of <b>null</b> (<b>Nothing</b> for VB), it will not be added to the collection.</para>
    /// </remarks>
    public HttpParameterCollection(Stream source) : this()
    {
      if (source == null) { return; }
      this.Add(new HttpRequestBody(source));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterCollection"/> class.
    /// </summary>
    /// <param name="source">The array of parameters.</param>
    /// <remarks>
    /// <para>If the <paramref name="source"/> parameter has a value of <b>null</b> (<b>Nothing</b> for VB), it will not be added to the collection.</para>
    /// </remarks>
    public HttpParameterCollection(HttpParameter[] source) : this()
    {
      if (source == null) { return; }
      this.AddRange(source);
    }

    #endregion
    #region ..methods..

    #region Add()

    /// <summary>
    /// Adds a parameter to the end of the collection.
    /// </summary>
    /// <param name="item">The parameter to be added to the collection.</param>
    public new void Add(HttpParameter item)
    {
      this.CheckType(item.GetType());
      base.Add(item);
      this.UpdateCollectionStatus();
    }

    /// <summary>
    /// Adds a parameter to the end of the collection.
    /// </summary>
    /// <param name="name">The name of parameter.</param>
    /// <param name="value">The parameter value.</param>
    public void Add(string name, object value)
    {
      if (value != null)
      {
        var t = value.GetType();
        if (t == typeof(System.Web.HttpPostedFile))
        {
          this.Add(name, (System.Web.HttpPostedFile)value);
        }
        else if (t == typeof(Stream) || (t.BaseType != null && t.BaseType == typeof(Stream)))
        {
          this.Add(name, (Stream)value);
        }
        else if (t == typeof(FileInfo) || (t.BaseType != null && t.BaseType == typeof(FileInfo)))
        {
          this.Add(name, ((FileInfo)value).OpenRead());
        }
        else if (t == typeof(byte[]))
        {
          this.Add(name, new MemoryStream((byte[])value));
        }
        else if (t == typeof(HttpParameterValue))
        {
          this.Add(name, ((HttpParameterValue)value).Value);
        }
        else
        {
          this.Add(name, Convert.ToString(value));
        }
      }
      else
      {
        this.Add(name, "");
      }
    }

    /// <summary>
    /// Adds a <see cref="HttpParameter"/> to the end of the collection.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The parameter value.</param>
    public void Add(string name, string value)
    {
      this.Add(new HttpParameter(name, value));
    }

    /// <summary>
    /// Adds a <see cref="HttpParameter"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterValue">The value of the parameter.</param>
    /// <param name="parameterType">The type of the parameter.</param>
    public void Add(HttpParameterType parameterType, string parameterName, string parameterValue)
    {
      if (parameterType == HttpParameterType.Form)
      {
        this.Add(new HttpFormParameter(parameterName, parameterValue));
      }
      else if (parameterType == HttpParameterType.Url)
      {
        this.Add(new HttpUrlParameter(parameterName, parameterValue));
      }
      else if (parameterType == HttpParameterType.RequestBody)
      {
        this.Add(new HttpRequestBody(this.Encoding.GetBytes(parameterValue)));
      }
      else if (parameterType == HttpParameterType.File)
      {
        this.Add(new HttpFile(parameterName, "file.dat", this.Encoding.GetBytes(parameterValue)));
      }
      else
      {
        this.Add(new HttpParameter(parameterName, parameterValue));
      }
    }

    /// <summary>
    /// Adds a <see cref="HttpFile"/> to the end of the collection.
    /// </summary>
    /// <param name="name">The name of parameter.</param>
    /// <param name="file">The posted file.</param>
    [Obsolete("Use overload.", false)]
    public void Add(string name, System.Web.HttpPostedFile file)
    {
      if (file == null)
      {
        throw new ArgumentNullException("file");
      }
      this.Add(new HttpFile(name, file));
    }

    /// <summary>
    /// Adds a <see cref="HttpRequestBody"/> to the end of the collection.
    /// </summary>
    /// <param name="file">The posted file.</param>
    [Obsolete("Use overload.", false)]
    public void Add(System.Web.HttpPostedFile file)
    {
      if (file == null)
      {
        throw new ArgumentNullException("file");
      }
      this.Add(new HttpRequestBody(file.InputStream));
    }

    /// <summary>
    /// Adds a <see cref="HttpFile"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileContent">The content of the file.</param>
    public void Add(string parameterName, string fileName, string contentType, byte[] fileContent)
    {
      this.Add(new HttpFile(parameterName, fileName, contentType, fileContent));
    }

    /// <summary>
    /// Adds a <see cref="HttpFile"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="fileContent">The content of the file.</param>
    public void Add(string parameterName, string fileName, byte[] fileContent)
    {
      this.Add(new HttpFile(parameterName, fileName, fileContent));
    }

    /// <summary>
    /// Adds a <see cref="HttpFile"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="inputStream">The file stream.</param>
    public void Add(string parameterName, string fileName, string contentType, Stream inputStream)
    {
      this.Add(new HttpFile(parameterName, fileName, contentType, inputStream));
    }

    /// <summary>
    /// Adds a <see cref="HttpFile"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="inputStream">The file stream.</param>
    public void Add(string parameterName, string fileName, Stream inputStream)
    {
      this.Add(new HttpFile(parameterName, fileName, inputStream));
    }

    /// <summary>
    /// Adds a <see cref="HttpRequestBody"/> to the end of the collection.
    /// </summary>
    /// <param name="inputStream">The content of the request.</param>
    public void Add(Stream inputStream)
    {
      if (inputStream == null)
      {
        throw new ArgumentNullException("inputStream");
      }
      this.Add(new HttpRequestBody(inputStream));
    }

    /// <summary>
    /// Adds a <see cref="HttpRequestBody"/> to the end of the collection.
    /// </summary>
    /// <param name="requestBody">The content of the request.</param>
    public void Add(byte[] requestBody)
    {
      if (requestBody == null)
      {
        throw new ArgumentNullException("requestBody");
      }
      this.Add(new HttpRequestBody(requestBody));
    }

    /// <summary>
    /// Adds a <see cref="HttpRequestBody"/> to the end of the collection.
    /// </summary>
    /// <param name="requestBody">The content of the request.</param>
    public void Add(object requestBody)
    {
      if (requestBody == null)
      {
        throw new ArgumentNullException("requestBody");
      }
      if (requestBody.GetType() == typeof(System.Web.HttpPostedFile))
      {
        this.Add((System.Web.HttpPostedFile)requestBody);
      }
      else if (requestBody.GetType() == typeof(Stream) || requestBody.GetType().IsSubclassOf(typeof(Stream))) // (requestBody.GetType().BaseType != null && requestBody.GetType().BaseType == typeof(Stream))
      {
        this.Add((Stream)requestBody);
      }
      else if (requestBody.GetType() == typeof(byte[]))
      {
        this.Add(new HttpRequestBody((byte[])requestBody));
      }
      else
      {
        // this.Add(this.Encoding.GetBytes(Convert.ToString(requestBody)));
        this.Add(new HttpRequestBody(requestBody));
      }
    }

    /// <summary>
    /// Adds the items of the specified collection to the end of the current instance of the <see cref="HttpParameterCollection"/>.
    /// </summary>
    /// <param name="collection">The collection whose items should be added to the end of the current instance of the <see cref="HttpParameterCollection"/>.</param>
    public new void AddRange(IEnumerable<HttpParameter> collection)
    {
      if (collection == null)
      {
        throw new ArgumentNullException("collection");
      }
      foreach (var itm in collection)
      {
        this.Add(itm);
      }
    }

    /// <summary>
    /// Adds a <see cref="HttpUrlParameter"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="parameterValue">The parameter value.</param>
    public void AddUrlParameter(string parameterName, string parameterValue)
    {
      this.Add(HttpParameterType.Url, parameterName, parameterValue);
    }

    /// <summary>
    /// Adds a <see cref="HttpUrlParameter"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="parameterValue">The parameter value.</param>
    public void AddUrlParameter(string parameterName, object parameterValue)
    {
      this.Add(HttpParameterType.Url, parameterName, parameterValue.ToString());
    }

    /// <summary>
    /// Adds a <see cref="HttpFormParameter"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="parameterValue">The parameter value.</param>
    public void AddFormParameter(string parameterName, string parameterValue)
    {
      this.Add(HttpParameterType.Form, parameterName, parameterValue);
    }

    /// <summary>
    /// Adds a <see cref="HttpFormParameter"/> to the end of the collection.
    /// </summary>
    /// <param name="parameterName">The name of parameter.</param>
    /// <param name="parameterValue">The parameter value.</param>
    public void AddFormParameter(string parameterName, object parameterValue)
    {
      this.Add(HttpParameterType.Form, parameterName, parameterValue.ToString());
    }

    /// <summary>
    /// Adds file as content to the end of the collection.
    /// </summary>
    /// <param name="file">The posted file.</param>
    [Obsolete("Use overload.", false)]
    public void AddContent(System.Web.HttpPostedFile file)
    {
      this.AddContent(null, file);
    }

    /// <summary>
    /// Adds content to the end of the collection.
    /// </summary>
    /// <param name="contentType">The Content-Type of the <paramref name="value"/>.</param>
    /// <param name="value">The content value.</param>
    public void AddContent(string contentType, object value)
    {
      if (String.IsNullOrEmpty(contentType) && value != null && value.GetType() == typeof(System.Web.HttpPostedFile))
      {
        contentType = ((System.Web.HttpPostedFile)value).ContentType;
      }

      if (String.IsNullOrEmpty(contentType))
      {
        throw new ArgumentNullException("contentType");
      }

      contentType = contentType.ToLower();

      if (value == null)
      {
        this.Add(new HttpParameter(null, null, contentType));
        return;
      }

      var t = value.GetType();

      if (t == typeof(System.Web.HttpPostedFile))
      {
        this.Add(new HttpParameter(null, (System.Web.HttpPostedFile)value, contentType));
      }
      else if (t == typeof(Stream) || (t.BaseType != null && t.BaseType == typeof(Stream)))
      {
        this.Add(new HttpParameter(null, (Stream)value, contentType));
      }
      else if (t == typeof(FileInfo) || (t.BaseType != null && t.BaseType == typeof(FileInfo)))
      {
        this.Add(new HttpParameter(null, ((FileInfo)value).OpenRead(), contentType));
      }
      else if (t == typeof(byte[]))
      {
        this.Add(new HttpParameter(null, (byte[])value, contentType));
      }
      else if (t == typeof(HttpParameterValue))
      {
        this.Add(new HttpParameter(null, (HttpParameterValue)value, contentType));
      }
      else
      {
        if (t.IsClass && Array.IndexOf(OAuthUtility.ExcludedTypeOfClasses, t) == -1 || (t.IsArray && t != typeof(byte[])))
        {
          if (contentType.Contains("json"))
          {
            string json = new System.Web.Script.Serialization.JavaScriptSerializer()
            {
              MaxJsonLength = Int32.MaxValue,
              RecursionLimit = Int32.MaxValue
            }.Serialize(value);
            this.Add(new HttpParameter(null, new HttpParameterValue(json), contentType));
          }
          else if (contentType.Contains("xml"))
          {
            using (var m = new MemoryStream())
            {
              new System.Xml.Serialization.XmlSerializer(t).Serialize(m, value);
              this.Add(new HttpParameter(null, new HttpParameterValue(this.Encoding.GetString(m.ToArray())), contentType));
            }
          }
          else
          {
            throw new NotSupportedException("Use the content type application/json or text/xml, or specify the value as a string or byte array.");
          }
        }
        else
        {
          this.Add(new HttpParameter(null, new HttpParameterValue(value), contentType));
        }
      }
    }

    #endregion

    /// <summary>
    /// Inserts an element into the instance of the <see cref="HttpParameterCollection"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which item should be inserted.</param>
    /// <param name="item">The <see cref="HttpParameter"/> to insert.</param>
    public new void Insert(int index, HttpParameter item)
    {
      this.CheckType(item.GetType());
      base.Insert(index, item);
      this.UpdateCollectionStatus();
    }

    /// <summary>
    /// Inserts the elements of a collection into the instance of the <see cref="HttpParameterCollection"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="collection">The collection whose elements should be inserted into the instance of the <see cref="HttpParameterCollection"/>.</param>
    public new void InsertRange(int index, IEnumerable<HttpParameter> collection)
    {
      foreach (var itm in collection)
      {
        this.Insert(index, itm);
      }
    }

    /// <summary>
    /// Removes the first occurrence of a specific parameter from the <see cref="HttpParameterCollection"/>.
    /// </summary>
    /// <param name="item">The parameter to remove from the <see cref="HttpParameterCollection"/>. </param>
    /// <returns>
    /// <b>true</b> if <paramref name="item"/> is successfully removed; otherwise, <b>false</b>. 
    /// This method also returns <b>false</b> if <paramref name="item"/> was not found in the <see cref="HttpParameterCollection"/>.
    /// </returns>
    public new bool Remove(HttpParameter item)
    {
      var result = base.Remove(item);
      this.UpdateCollectionStatus();
      return result;
    }

    /// <summary>
    /// Removes all the elements that match the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="math">The <see cref="Predicate&lt;T&gt;"/> delegate that defines the conditions of the elements to remove.</param>
    /// <returns>The number of elements removed from the <see cref="HttpParameterCollection"/>.</returns>
    public new int RemoveAll(Predicate<HttpParameter> math)
    {
      var result = base.RemoveAll(math);
      this.UpdateCollectionStatus();
      return result;
    }

    /// <summary>
    /// Removes the element at the specified index of the <see cref="HttpParameterCollection"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    public new void RemoveAt(int index)
    {
      base.RemoveAt(index);
      this.UpdateCollectionStatus();
    }

    /// <summary>
    /// Removes a range of elements from the <see cref="HttpParameterCollection"/>.
    /// </summary>
    /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    public new void RemoveRange(int index, int count)
    {
      base.RemoveRange(index, count);
      this.UpdateCollectionStatus();
    }

    /// <summary>
    /// Removes all elements from the <see cref="HttpParameterCollection"/>.
    /// </summary>
    public new void Clear()
    {
      base.Clear();
      this.UpdateCollectionStatus();
    }

    /// <summary>
    /// Updates status of the collection.
    /// </summary>
    private void UpdateCollectionStatus()
    {
      this.IsRequestBody = this.Any(itm => itm.GetType() == typeof(HttpRequestBody));
      this.HasFiles = this.Any(itm => itm.GetType() == typeof(HttpFile));
    }

    /// <summary>
    /// Checks parameters types.
    /// </summary>
    /// <param name="t">The type for checking.</param>
    private void CheckType(Type t)
    {
      if (t == typeof(HttpFile) && this.IsRequestBody)
      {
        throw new IncompatibleHttpParametersException();
      }
      if (t == typeof(HttpRequestBody) && this.IsRequestBody)
      {
        throw new MultipleRequestBodyException();
      }
    }

    #region ToStringParameters()

    /// <summary>
    /// Returns a string query parameters encoded by default method (<see cref="System.Web.HttpUtility.UrlEncode(string)"/>).
    /// </summary>
    public string ToStringParameters()
    {
      return this.ToStringParameters(false);
    }

    /// <summary>
    /// Returns a string query parameters encoded by default method (<see cref="System.Web.HttpUtility.UrlEncode(string)"/>).
    /// </summary>
    /// <param name="donotEncodeKeys">Allows forbid to encode a parameters names.</param>
    public string ToStringParameters(bool donotEncodeKeys)
    {
      if (donotEncodeKeys)
      {
        return this.ToStringParameters("&", UrlEncodingType.Default, HttpParameterType.Unformed | HttpParameterType.Url | HttpParameterType.OptDonotEncodeKeys);
      }
      else
      {
        return this.ToStringParameters("&", UrlEncodingType.Default, HttpParameterType.Unformed | HttpParameterType.Url);
      }
    }

    /// <summary>
    /// Returns a string of query parameters with a specified separator.
    /// </summary>
    /// <param name="separator">The separator of query parameters. For example: &amp;</param>
    public string ToStringParameters(string separator)
    {
      return this.ToStringParameters(separator, false);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified separator.
    /// </summary>
    /// <param name="separator">The separator of query parameters. For example: &amp;</param>
    /// <param name="donotEncodeKeys">Allows forbid to encode a parameters names.</param>
    public string ToStringParameters(string separator, bool donotEncodeKeys)
    {
      if (donotEncodeKeys)
      {
        return this.ToStringParameters(separator, UrlEncodingType.Default, HttpParameterType.Unformed | HttpParameterType.Url | HttpParameterType.OptDonotEncodeKeys);
      }
      else
      {
        return this.ToStringParameters(separator, UrlEncodingType.Default, HttpParameterType.Unformed | HttpParameterType.Url);
      }
    }

    /// <summary>
    /// Returns a string of query parameters with a specified encoding parameters.
    /// </summary>
    /// <param name="separator">The separator of query parameters. For example: &amp;</param>
    /// <param name="donotEncodeKeys">Allows forbid to encode a parameters names.</param>
    /// <param name="encodingType">The type of url encoding.</param>
    public string ToStringParameters(string separator, UrlEncodingType encodingType, bool donotEncodeKeys)
    {
      if (donotEncodeKeys)
      {
        return this.ToStringParameters(separator, encodingType, HttpParameterType.Unformed | HttpParameterType.Url | HttpParameterType.OptDonotEncodeKeys);
      }
      else
      {
        return this.ToStringParameters(separator, encodingType, HttpParameterType.Unformed | HttpParameterType.Url);
      }
    }

    /// <summary>
    /// Returns a string of query parameters with a specified encoding parameters.
    /// </summary>
    /// <param name="encodingType">The type of url encoding.</param>
    public string ToStringParameters(UrlEncodingType encodingType)
    {
      return this.ToStringParameters(encodingType, false);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified encoding parameters.
    /// </summary>
    /// <param name="donotEncodeKeys">Allows forbid to encode a parameters names.</param>
    /// <param name="encodingType">The type of url encoding.</param>
    public string ToStringParameters(UrlEncodingType encodingType, bool donotEncodeKeys)
    {
      if (donotEncodeKeys)
      {
        return this.ToStringParameters("&", encodingType, HttpParameterType.Unformed | HttpParameterType.Url | HttpParameterType.OptDonotEncodeKeys);
      }
      else
      {
        return this.ToStringParameters("&", encodingType, HttpParameterType.Unformed | HttpParameterType.Url);
      }
    }

    /// <summary>
    /// Returns a string of query parameters with a specified encoding parameters.
    /// </summary>
    /// <param name="parametersType">The parameter type.</param>
    public string ToStringParameters(HttpParameterType parametersType)
    {
      return this.ToStringParameters("&", UrlEncodingType.Default, parametersType);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified encoding parameters.
    /// </summary>
    /// <param name="encodingType">The type of url encoding.</param>
    /// <param name="parametersType">The parameter type.</param>
    public string ToStringParameters(UrlEncodingType encodingType, HttpParameterType parametersType)
    {
      return this.ToStringParameters("&", encodingType, parametersType);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified encoding parameters.
    /// </summary>
    /// <param name="separator">The separator of query parameters. For example: &amp;</param>
    /// <param name="parametersType">The parameter type.</param>
    public string ToStringParameters(string separator, HttpParameterType parametersType)
    {
      return this.ToStringParameters(separator, UrlEncodingType.Default, parametersType);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified separator and encoding parameters.
    /// </summary>
    /// <param name="separator">The separator of query parameters.</param>
    /// <param name="encodingType">The type of the encoder.</param>
    /// <param name="parametersType">The types of parameters to be used.</param>
    public string ToStringParameters(string separator, UrlEncodingType encodingType, HttpParameterType parametersType)
    {
      if ((parametersType & HttpParameterType.Form) == HttpParameterType.Form || (parametersType & HttpParameterType.RequestBody) == HttpParameterType.RequestBody || (parametersType & HttpParameterType.File) == HttpParameterType.File)
      {
        throw new NotSupportedException("Only url parameters allowed.");
      }

      string result = "";

      foreach (var itm in this)
      {
        // ignore files and request body
        if (itm.GetType() == typeof(HttpFile) || itm.GetType() == typeof(HttpRequestBody)) { continue; }

        // ignore common parameters, if specified in the rule
        // note: !parametersType.HasFlag(HttpParameterType.Unformed) is not supported in the .net framework 3.5
        if (itm.GetType() != typeof(HttpUrlParameter) && (parametersType & HttpParameterType.Unformed) != HttpParameterType.Unformed) { continue; }

        if (result.Length > 0) { result += "&"; }

        var value = "";

        if (itm.Value != null)
        {
          value = itm.Value.ToEncodedString(encodingType);
        }

        if ((parametersType & HttpParameterType.OptDonotEncodeKeys) == HttpParameterType.OptDonotEncodeKeys)
        {
          result += String.Format("{0}={1}", itm.Name, value);
        }
        else
        {
          result += String.Format("{0}={1}", OAuthUtility.UrlEncode(itm.Name, encodingType), value);
        }
      }

      return result;
    }

    #endregion

    /// <summary>
    /// Copies elements of the <see cref="HttpParameterCollection"/> to a new <see cref="NameValueCollection" /> instance.
    /// </summary>
    /// <param name="onlyUrlParameters">The <b>true</b> value included into the results only the <see cref="HttpUrlParameter"/> parameters. The default value is <b>false</b> - all parameters.</param>
    public NameValueCollection ToNameValueCollection(bool onlyUrlParameters = false)
    {
      var result = new NameValueCollection();
      foreach (var itm in this)
      {
        if (itm.GetType() == typeof(HttpFile) || itm.GetType() == typeof(HttpRequestBody)) { continue; }
        if (onlyUrlParameters && itm.GetType() != typeof(HttpUrlParameter)) { continue; }
        result.Add(itm.Name, itm.Value.ToString());
      }
      return result;
    }

    /// <summary>
    /// Gets a body for the request.
    /// </summary>
    /// <param name="contentType">Content-Type</param>
    [Obsolete("Please use WriteRequestBody. // v1.11", true)]
    public byte[] ToRequestBody(string contentType = null)
    {
      return this.GetRequestBody(contentType, 4096);
    }

    /// <summary>
    /// Writes a body to stream.
    /// </summary>
    /// <param name="output">Output stream.</param>
    /// <param name="contentType">Content-Type</param>
    /// <param name="bufferSize">Buffer size. Default: <c>4096</c>.</param>
    /// <param name="outputStatus">To parameter is passed information about the state of the writes to stream.</param>
    public void WriteRequestBody(Stream output, string contentType = null, int bufferSize = 4096, StreamWriteEventArgs outputStatus = null)
    {
      if (output == null)
      {
        throw new ArgumentNullException("stream");
      }

      if (outputStatus == null)
      {
        outputStatus = new StreamWriteEventArgs();
      }

      if (contentType == null) { contentType = ""; }

      contentType = contentType.ToLower();

      bool isMultipart = contentType.Contains("multipart/"); // multipart/form-data
      bool isFormData = contentType.Contains("multipart/form-data");

      if (this.IsRequestBody && !isMultipart)
      {
        this.First(itm => itm.GetType() == typeof(HttpRequestBody)).Value.WriteToStream(output, bufferSize, outputStatus, contentType, this.Encoding);
      }
      else if (this.HasFiles || isMultipart)
      {
        foreach (var itm in this)
        {
          if (itm.GetType() == typeof(HttpUrlParameter)) { continue; } // ignore url parameters
          if (itm.GetType() == typeof(HttpFile))
          {
            var file = itm as HttpFile;
            if (isFormData)
            {
              // add form-data parameter
              this.WriteFormDataFile(itm.Name, file.FileName, file.ContentType, file.Value, output, bufferSize, outputStatus);
            }
            else
            {
              // add other type
              this.WriteParameter(itm.ContentType, file.Value, output, bufferSize, outputStatus);
            }
          }
          else
          {
            if (isFormData)
            {
              // add form-data parameter
              this.WriteFormDataParameter(itm.Name, itm.Value.ToString(), output);
            }
            else
            {
              // add other type
              this.WriteParameter(itm.ContentType, itm.Value, output, bufferSize, outputStatus);
            }
          }
        }

        var end = this.Encoding.GetBytes(String.Format("\r\n--{0}--\r\n", this.Boundary));
        output.Write(end, 0, end.Length);
        output.Flush();

        outputStatus.BytesWritten = end.Length;
      }
      else
      {
        var buffer = this.Encoding.GetBytes(((HttpParameterCollection)this.Where(itm => itm.GetType() != typeof(HttpUrlParameter)).ToArray()).ToStringParameters(UrlEncodingType.PercentEncoding));
        output.Write(buffer, 0, buffer.Length);
        outputStatus.BytesWritten = buffer.Length;
      }
    }

    /// <summary>
    /// Writes the parameters to the request.
    /// </summary>
    /// <param name="req">The instance of the request.</param>
    public void WriteToRequestStream(HttpWebRequest req)
    {
      this.WriteToRequestStream(req, 4096, null);
    }

    /// <summary>
    /// Writes the parameters to the request.
    /// </summary>
    /// <param name="req">The instance of the request.</param>
    /// <param name="callback">A delegate that, if provided, is called when writing a data block to the stream.</param>
    public void WriteToRequestStream(HttpWebRequest req, HttpWriteRequestStream callback)
    {
      this.WriteToRequestStream(req, 4096, callback);
    }

    /// <summary>
    /// Writes the parameters to the request.
    /// </summary>
    /// <param name="req">The instance of the request.</param>
    /// <param name="bufferSize">Buffer size. Default: 4096.</param>
    /// <param name="callback">A delegate that, if provided, is called when writing a data block to the stream.</param>
    public void WriteToRequestStream(HttpWebRequest req, int bufferSize, HttpWriteRequestStream callback)
    {
      // content-type
      if (String.IsNullOrEmpty(req.ContentType))
      {
        if (this.IsRequestBody)
        {
          req.ContentType = "application/octet-stream";
        }
        else if (this.HasFiles)
        {
          req.ContentType = String.Format("multipart/form-data; boundary={0}", this.Boundary);
        }
        else
        {
          req.ContentType = "application/x-www-form-urlencoded";
        }
      }

      if (bufferSize <= 0)
      {
        bufferSize = 4096;
      }

      var writerArgs = new StreamWriteEventArgs();

      writerArgs.Changed += (s, e) =>
      {
        if (callback != null)
        {
          callback(req, writerArgs);
        }
      };

      // set boundary
      if (req.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) != -1 && req.ContentType.IndexOf("boundary", StringComparison.OrdinalIgnoreCase) == -1)
      {
        if (!req.ContentType.EndsWith(";")) { req.ContentType += "; "; }
        req.ContentType += String.Format("boundary={0}", this.Boundary);
      }

      if ((this.HasFiles || this.IsRequestBody || req.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) != -1) && !req.AllowWriteStreamBuffering)
      {
        #region for large data (without buffering)

        // not as bad as it could be :)
        // backwards compatibility with earlier versions of fully preserved

        bool beginGetRequestStream = false;

        // write async
        var asyncResult = req.BeginGetRequestStream((IAsyncResult r) =>
        {
          using (var s = req.EndGetRequestStream(r))
          {
            this.WriteRequestBody(s, req.ContentType, bufferSize, writerArgs);
          }
          beginGetRequestStream = true;
        }, null);

        // waiting result
        while (!beginGetRequestStream)
        {
          Thread.Sleep(250);
        }

        writerArgs.IsCompleted = true;

        #endregion
      }
      else
      {
        #region small data (old/original code)

        // get request body 
        byte[] b = this.GetRequestBody(req.ContentType, bufferSize);

#if NET35
        // fix for .NET Framework 2.0/3.0/3.5
        req.ContentLength = b.Length;
#endif

        req.GetRequestStream().Write(b, 0, b.Length);

        writerArgs.BytesWritten = b.Length;
        writerArgs.IsCompleted = true;

        #endregion
      }
    }

    /// <summary>
    /// Gets a body for the request.
    /// </summary>
    /// <param name="contentType">Content-Type</param>
    /// <param name="bufferSize">Buffer size. Default: 4096.</param>
    private byte[] GetRequestBody(string contentType, int bufferSize)
    {
      if (contentType == null) { contentType = ""; }
      if (bufferSize <= 0) { bufferSize = 4096; }

      contentType = contentType.ToLower();

      bool isMultipart = contentType.Contains("multipart/"); // multipart/form-data
      bool isFormData = contentType.Contains("multipart/form-data");

      if (this.IsRequestBody && !isMultipart)
      {
        using (var m = new MemoryStream())
        {
          this.First(itm => itm.GetType() == typeof(HttpRequestBody)).Value.WriteToStream(m, bufferSize, null, contentType, this.Encoding);

          return m.ToArray();
        }
      }
      else if (this.HasFiles || isMultipart)
      {
        using (var m = new MemoryStream())
        {
          foreach (var itm in this)
          {
            if (itm.GetType() == typeof(HttpUrlParameter)) { continue; } // url paramters ignore
            if (itm.GetType() == typeof(HttpFile))
            {
              var file = itm as HttpFile;
              if (isFormData)
              {
                // add form-data parameter
                this.WriteFormDataFile(itm.Name, file.FileName, file.ContentType, file.Value.ToByteArray(this.Encoding), m);
              }
              else
              {
                // add other type
                this.WriteParameter(itm.ContentType, file.Value.ToByteArray(this.Encoding), m);
              }
            }
            else
            {
              if (isFormData)
              {
                // add form-data parameter
                this.WriteFormDataParameter(itm.Name, itm.Value.ToString(), m);
              }
              else
              {
                // add other type
                this.WriteParameter(itm.ContentType, itm.Value.ToByteArray(this.Encoding), m);
              }
            }
          }

          var end = this.Encoding.GetBytes(String.Format("\r\n--{0}--\r\n", this.Boundary));

          m.Write(end, 0, end.Length);
          m.Flush();

          return m.ToArray();
        }
      }
      else
      {
        return this.Encoding.GetBytes(((HttpParameterCollection)this.Where(itm => itm.GetType() != typeof(HttpUrlParameter)).ToArray()).ToStringParameters(UrlEncodingType.PercentEncoding));
      }
    }

    /// <summary>
    /// Writes a file to the request.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="fileName">The filename.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="body">The content of the file.</param>
    /// <param name="output">The output stream instance.</param>
    private void WriteFormDataFile(string parameterName, string fileName, string contentType, byte[] body, Stream output)
    {
      string headerTemplate = String.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"{{0}}\"; filename=\"{{1}}\"\r\nContent-Type: {{2}}\r\n\r\n", this.Boundary);
      byte[] buffer = this.Encoding.GetBytes(String.Format(headerTemplate, parameterName, fileName, contentType));
      output.Write(buffer, 0, buffer.Length);
      output.Write(body, 0, body.Length);
    }

    /// <summary>
    /// Writes a file to the request.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="fileName">The filename.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="output">The output stream instance.</param>
    /// <param name="outputStatus">To parameter is passed information about the state of the writes to stream.</param>
    /// <param name="bufferSize">Buffer size.</param>
    /// <param name="value">The parameter value to write.</param>
    private void WriteFormDataFile(string parameterName, string fileName, string contentType, HttpParameterValue value, Stream output, int bufferSize, StreamWriteEventArgs outputStatus)
    {
      string headerTemplate = String.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"{{0}}\"; filename=\"{{1}}\"\r\nContent-Type: {{2}}\r\n\r\n", this.Boundary);
      byte[] buffer = this.Encoding.GetBytes(String.Format(headerTemplate, parameterName, fileName, contentType));
      output.Write(buffer, 0, buffer.Length);

      if (outputStatus != null)
      {
        outputStatus.BytesWritten = buffer.Length;
      }

      value.WriteToStream(output, bufferSize, outputStatus);
    }

    /// <summary>
    /// Writes a form-data parameter to the request.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="output">The output stream instance.</param>
    private void WriteFormDataParameter(string parameterName, string value, Stream output)
    {
      string formDataTemplate = String.Format("\r\n--{0}\r\nContent-Disposition: form-data; name=\"{{0}}\";\r\n\r\n{{1}}", this.Boundary);
      byte[] buffer = this.Encoding.GetBytes(String.Format(formDataTemplate, parameterName, value));
      output.Write(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Writes any parameter to the request.
    /// </summary>
    /// <param name="contentType">The Content-Type of the <paramref name="content"/>.</param>
    /// <param name="content">The content value.</param>
    /// <param name="output">The instance of the output stream.</param>
    private void WriteParameter(string contentType, byte[] content, Stream output)
    {
      byte[] buffer = this.Encoding.GetBytes(String.Format("\r\n--{0}\r\nContent-Type: {1}\r\n\r\n", this.Boundary, contentType));
      output.Write(buffer, 0, buffer.Length);
      output.Write(content, 0, content.Length);
    }

    /// <summary>
    /// Writes any parameter to the request.
    /// </summary>
    /// <param name="contentType">The Content-Type of the <paramref name="value"/>.</param>
    /// <param name="output">The instance of the output stream.</param>
    /// <param name="outputStatus">To parameter is passed information about the state of the writes to stream.</param>
    /// <param name="value">The parameter value to write.</param>
    /// <param name="bufferSize">Buffer size.</param>
    private void WriteParameter(string contentType, HttpParameterValue value, Stream output, int bufferSize, StreamWriteEventArgs outputStatus)
    {
      byte[] buffer = this.Encoding.GetBytes(String.Format("\r\n--{0}\r\nContent-Type: {1}\r\n\r\n", this.Boundary, contentType));
      output.Write(buffer, 0, buffer.Length);

      if (outputStatus != null)
      {
        outputStatus.BytesWritten = buffer.Length;
      }

      value.WriteToStream(output, bufferSize, outputStatus);
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// The assignment operator for array of the <see cref="HttpParameter"/>.
    /// </summary>
    /// <param name="value">The array that will be used as the <see cref="HttpParameterCollection"/>.</param>
    /// <returns>New instance of the <see cref="HttpParameterCollection"/>.</returns>
    public static implicit operator HttpParameterCollection(HttpParameter[] value)
    {
      return new HttpParameterCollection(value);
    }

    /// <summary>
    /// The assignment operator for the <see cref="NameValueCollection"/>.
    /// </summary>
    /// <param name="value">The collection that will be used as the <see cref="HttpParameterCollection"/>.</param>
    /// <returns>New instance of the <see cref="HttpParameterCollection"/>.</returns>
    public static implicit operator HttpParameterCollection(NameValueCollection value)
    {
      return new HttpParameterCollection(value);
    }

    /*
    /// <summary>
    /// The assignment operator for the <see cref="Dictionary&lt;TKey, TVvalue&gt;"/>.
    /// </summary>
    /// <param name="value">The collection that will be used as the <see cref="HttpParameterCollection"/>.</param>
    /// <returns>New instance of the <see cref="HttpParameterCollection"/>.</returns>
    public static implicit operator HttpParameterCollection(Dictionary<string, object> value)
    {
      return new HttpParameterCollection(value);
    }
    */

    /// <summary>
    /// The assignment operator for the byte array.
    /// </summary>
    /// <param name="value">The byte array of the request body.</param>
    /// <returns>New instance of the <see cref="HttpParameterCollection"/>.</returns>
    public static implicit operator HttpParameterCollection(byte[] value)
    {
      return new HttpParameterCollection(value);
    }

    /// <summary>
    /// The assignment operator for the <see cref="System.IO.Stream"/>.
    /// </summary>
    /// <param name="value">The instance of the <see cref="System.IO.Stream"/> of the request body.</param>
    /// <returns>New instance of the <see cref="HttpParameterCollection"/>.</returns>
    public static implicit operator HttpParameterCollection(Stream value)
    {
      return new HttpParameterCollection(value);
    }

    #endregion

  }

}