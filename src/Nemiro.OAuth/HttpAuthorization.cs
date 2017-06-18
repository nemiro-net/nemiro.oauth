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
using System.Text.RegularExpressions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents a HTTP authorization header.
  /// </summary>
  public class HttpAuthorization
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets or sets authorization method.
    /// </summary>
    public AuthorizationType AuthorizationType { get; set; }

    /// <summary>
    /// Gets or sets parameters of the authorization.
    /// </summary>
    public UniValue Value { get; protected set; }

    /// <summary>
    /// Authorization parameters.
    /// </summary>
    [Obsolete("Do not use this property. Use Value. // v1.5", true)]
    public Dictionary<string, object> Parameters
    {
      get
      {
        return this.Value.ToDictionary();
      }
      protected set
      {
        this.Value = value;
      }
    }

    /// <summary>
    /// Sorted authorization parameters.
    /// </summary>
    protected Dictionary<string, object> ParametersSorted
    {
      get
      {
        // todo: replace ToDictionary
        return this.Value.ToDictionary().OrderBy(itm => itm.Key).ToDictionary(itm => itm.Key, itm => itm.Value);
      }
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    public UniValue this[string key]
    {
      get
      {
        return this.Value[key];
      }
      set
      {
        this.Value[key] = value;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthorization"/> class.
    /// </summary>
    public HttpAuthorization()
    {
      this.Value = new UniValue();
      this.AuthorizationType = OAuth.AuthorizationType.OAuth;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthorization"/> class with specific authorization type and value.
    /// </summary>
    public HttpAuthorization(AuthorizationType type, UniValue value)
    {
      this.AuthorizationType = type;
      this.Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthorization"/> class from specific source.
    /// </summary>
    public HttpAuthorization(string source) : this()
    {
      if (Regex.IsMatch(source, "^OAuth", RegexOptions.IgnoreCase))
      {
        this.AuthorizationType = OAuth.AuthorizationType.OAuth;
      }
      else if (Regex.IsMatch(source, "^Bearer", RegexOptions.IgnoreCase))
      {
        this.AuthorizationType = OAuth.AuthorizationType.Bearer;
      }
      else if (Regex.IsMatch(source, "^Digest", RegexOptions.IgnoreCase))
      {
        this.AuthorizationType = OAuth.AuthorizationType.Digest;
      }
      else
      {
        this.AuthorizationType = OAuth.AuthorizationType.Basic;
      }

      var regex = new Regex(@"\b(?<key>[^\x3d]*)=""(?<value>[^\x22]*)""", RegexOptions.Singleline);

      if (source.IndexOf(" ") != -1)
      {
        source = source.Substring(source.IndexOf(" ") + 1);
      }

      if (regex.IsMatch(source))
      {
        var mc = regex.Matches(source);

        foreach (Match m in mc)
        {
          this.Value.Add(m.Groups["key"].Value.Trim(), System.Web.HttpUtility.UrlDecode(m.Groups["value"].Value.Trim()));
        }
      }
      else
      {
        this.Value = source;
      }
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Removes the value with the specified key from the <see cref="Value"/>.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>
    /// <b>true</b> if the element is successfully found and removed; otherwise, <b>false</b>. 
    /// This method returns <b>false</b> if key is not found in the <see cref="Value"/> or is not collection.
    /// </returns>
    public bool Remove(string key)
    {
      return this.Value.Remove(key);
    }

    /// <summary>
    /// Returns OAuth string of the current object for Authorization header. 
    /// </summary>
    public override string ToString()
    {
      StringBuilder result = new StringBuilder();
      if (this.Value.HasValue)
      {
        if (this.Value.Count > 0)
        {
          foreach (var itm in this.ParametersSorted)
          {
            if (result.Length > 0) { result.Append(", "); }
            result.AppendFormat("{0}=\"{1}\"", itm.Key, OAuthUtility.UrlEncode(itm.Value.ToString()));
          }
        }
        else
        {
          result.Append(this.Value.ToString());
        }
      }
      result.Insert(0, this.AuthorizationType.ToString() + " ");
      return result.ToString();
    }

    /// <summary>
    /// Invoked before sending a web request.
    /// </summary>
    /// <param name="httpMethod">HTTP Method of the request: <b>POST</b>, <b>PUT</b>, <b>GET</b> or <b>DELETE</b>.</param>
    /// <param name="url">URL of the web request.</param>
    /// <param name="contentType">The value of the <b>Content-Type</b> HTTP header.</param>
    /// <param name="parameters">>Parameters of the web request.</param>
    internal virtual void Build(string httpMethod, string url, string contentType, HttpParameterCollection parameters) { }

    #endregion
    #region ..operators..

    /// <summary>
    /// Creates a new <see cref="HttpAuthorization"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="HttpAuthorization"/>.</param>
    public static implicit operator HttpAuthorization(string value)
    {
      return new HttpAuthorization(value);
    }

    #endregion

  }

}