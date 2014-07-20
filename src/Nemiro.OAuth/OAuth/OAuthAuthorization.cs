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

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents authorization parameters for OAuth.
  /// </summary>
  public class OAuthAuthorization
  {

    #region ..fields & properties..

    private Dictionary<string, object> _Parameters = new Dictionary<string, object>();

    /// <summary>
    /// Authorization parameters.
    /// </summary>
    public Dictionary<string, object> Parameters
    {
      get
      {
        return _Parameters;
      }
      protected set
      {
        _Parameters = value;
      }
    }

    /// <summary>
    /// Sorted authorization parameters.
    /// </summary>
    protected Dictionary<string, object> ParametersSorted
    {
      get
      {
        return _Parameters.OrderBy(itm => itm.Key).ToDictionary(itm => itm.Key, itm => itm.Value);
      }
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    public object this[string key]
    {
      get
      {
        if (!this.Parameters.ContainsKey(key)) { return null; }
        return this.Parameters[key];
      }
      set
      {
        if (!this.Parameters.ContainsKey(key))
        {
          this.Parameters.Add(key, null);
        }
        this.Parameters[key] = value;
      }
    }
    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthorization"/> class.
    /// </summary>
    public OAuthAuthorization() { }

    #endregion
    #region ..methods..

    /// <summary>
    /// Returns OAuth string of the current object for Authorization header. 
    /// </summary>
    public override string ToString()
    {
      StringBuilder result = new StringBuilder();
      foreach (var itm in this.ParametersSorted)
      {
        if (result.Length > 0) { result.Append(", "); }
        result.AppendFormat("{0}=\"{1}\"", itm.Key, Helpers.UrlEncode(itm.Value.ToString()));
      }
      result.Insert(0, "OAuth ");
      return result.ToString();
    }

    #endregion

  }

}
