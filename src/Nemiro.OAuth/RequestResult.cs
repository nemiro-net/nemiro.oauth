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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the base class for results of remote requests.
  /// </summary>
  public class RequestResult
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets or sets the content type of the response.
    /// </summary>
    protected internal string ContentType { get; protected set; }

    /// <summary>
    /// Gets or sets the source of the response.
    /// </summary>
    public string Source { get; protected set; }

    /// <summary>
    /// Gets or sets the processed result of the response.
    /// </summary>
    public object Result { get; protected set; }

    /// <summary>
    /// Gets the value associated with the specified key of the <see cref="Result"/>.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    public object this[string key]
    {
      get
      {
        if (this.IsNameValueCollection)
        {
          return ((NameValueCollection)this.Result)[key];
        }
        else if (this.IsDictionary)
        {
          return ((Dictionary<string, object>)this.Result)[key];
        }
        else if (this.IsArray)
        {
          return ((Array)this.Result).GetValue(Convert.ToInt32(key));
        }
        return this.Result.GetType().GetProperty(key).GetValue(this, null);
      }
    }

    /// <summary>
    /// Gets the value associated with the specified index of the <see cref="Result"/>.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    public object this[int index]
    {
      get
      {
        if (this.IsNameValueCollection)
        {
          return ((NameValueCollection)this.Result)[index];
        }
        else if (this.IsDictionary)
        {
          return ((Dictionary<string, object>)this.Result)[index.ToString()];
        }
        else if (this.IsArray)
        {
          return ((Array)this.Result).GetValue(index);
        }
        return this.Result.GetType().GetProperties()[index].GetValue(this, null);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Result"/> is array or not.
    /// </summary>
    public bool IsArray
    {
      get
      {
        return this.Result.GetType().IsArray;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Result"/> is <see cref="Dictionary&lt;TKey, TValue&gt;"/> or not.
    /// </summary>
    public bool IsDictionary
    {
      get
      {
        return this.Result.GetType() == typeof(Dictionary<string, object>);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Result"/> is <see cref="NameValueCollection"/> or not.
    /// </summary>
    public bool IsNameValueCollection
    {
      get
      {
        return this.Result.GetType() == typeof(NameValueCollection);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="NameValueCollection"/> is XML or not.
    /// </summary>
    public bool IsXml
    {
      get
      {
        return this.Result.GetType() == typeof(XDocument);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="NameValueCollection"/> is empty or not.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        return this.Result == null;
      }
    }
    
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
      this.Result = result.Result;
      this.ContentType = result.ContentType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResult"/> class.
    /// </summary>
    /// <param name="contentType">The content type of the response.</param>
    /// <param name="source">The source  of the response.</param>
    public RequestResult(string contentType, string source)
    {
      if (String.IsNullOrEmpty(contentType))
      {
        contentType = "text/plain";
        //throw new ArgumentNullException("contentType");
      }

      if (contentType.IndexOf(";") != -1)
      {
        contentType = contentType.Substring(0, contentType.IndexOf(";"));
      }

      this.Source = source;
      this.ContentType = contentType;

      if (!String.IsNullOrEmpty(source))
      {
        this.ParseSource(contentType, source);
      }
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Parses the source to the <see cref="Result"/>.
    /// </summary>
    /// <param name="contentType">The content type of the response.</param>
    /// <param name="source">The source  of the response.</param>
    private void ParseSource(string contentType, string source)
    {
      switch (contentType.ToLower())
      {
        case "application/json":
        case "text/json":
        case "application/javascript":
        case "text/javascript":
          this.Result = new JavaScriptSerializer().DeserializeObject(source);
          break;

        case "text/xml":
          this.Result = XDocument.Parse(source);
          break;

        case "text/html":
        case "text/plain":
          if (Regex.IsMatch(source, @"([^\x3D]+)=([^\x26]*)", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase))
          {
            NameValueCollection items = new NameValueCollection();
            foreach (string q in source.Split('&'))
            {
              string[] p = q.Split('=');
              string key = p.First(), value = (p.Length > 1 ? p.Last() : "");
              items.Add(key, value);
            }
            this.Result = items;
          }
          else
          {
            this.Result = source;
          }
          break;

        default:
          throw new NotSupportedException(String.Format("Content-Type \"{0}\" not suppored.", contentType));
      }
    }

    /// <summary>
    /// Determines whether the <see cref="Result"/> contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the <see cref="Result"/>.</param>
    /// <returns><b>true</b> if the <see cref="Result"/> contains an element with the specified key; otherwise, <b>false</b>.</returns>
    public bool ContainsKey(string key)
    {
      if (this.IsNameValueCollection)
      {
        return ((NameValueCollection)this.Result).AllKeys.Any(itm => itm.Equals(key, StringComparison.OrdinalIgnoreCase));
      }
      else if (this.IsDictionary)
      {
        return ((Dictionary<string, object>)this.Result).ContainsKey(key);
      }
      else if (this.IsArray)
      {
        return ((Array)this.Result).Length <= Convert.ToInt32(key);
      }
      return this.Result.GetType().GetProperty(key) != null;
    }

    /// <summary>
    /// Returs the <see cref="Source"/>.
    /// </summary>
    public override string ToString()
    {
      return this.Source;
    }

    #endregion

  }

}
