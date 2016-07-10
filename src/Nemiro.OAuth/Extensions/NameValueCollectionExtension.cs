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
using System.Collections.Specialized;
using System.Web;

namespace Nemiro.OAuth.Extensions
{

  /// <summary>
  /// Represents a class that extends the <see cref="System.Collections.Specialized.NameValueCollection"/> class by adding methods for use with query parameters.
  /// </summary>
  public static class NameValueCollectionExtension
  {

    /// <summary>
    /// Convert the <see cref="System.Collections.Specialized.NameValueCollection"/> to list of the <see cref="System.Collections.Generic.KeyValuePair&lt;TKey, TValue&gt;"/>.
    /// </summary>
    /// <param name="source">The <see cref="System.Collections.Specialized.NameValueCollection"/>.</param>
    public static List<KeyValuePair<string, string>> ToKeyValuePairCollection(this NameValueCollection source)
    {
      return source.AllKeys.SelectMany
      (
        source.GetValues, (k, v) => new KeyValuePair<string, string>(k, v)
      ).ToList();
    }

    /// <summary>
    /// Returns a string of query parameters without a separator.
    /// </summary>
    /// <param name="source">The <see cref="System.Collections.Specialized.NameValueCollection"/>.</param>
    public static string ToParametersString(this NameValueCollection source)
    {
      return ToParametersString(source, "", false);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified separator.
    /// </summary>
    /// <param name="source">The <see cref="System.Collections.Specialized.NameValueCollection"/>.</param>
    /// <param name="separator">The separator of query parameters. For example: &amp;</param>
    public static string ToParametersString(this NameValueCollection source, string separator)
    {
      return ToParametersString(source, separator, false);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified encoding parameters.
    /// </summary>
    /// <param name="source">The <see cref="System.Collections.Specialized.NameValueCollection"/>.</param>
    /// <param name="noencoding">Disables parameters encoding.</param>
    public static string ToParametersString(this NameValueCollection source, bool noencoding)
    {
      return ToParametersString(source, "", noencoding);
    }

    /// <summary>
    /// Returns a string of query parameters with a specified separator and encoding parameters.
    /// </summary>
    /// <param name="source">The <see cref="System.Collections.Specialized.NameValueCollection"/>.</param>
    /// <param name="separator">The separator of query parameters.</param>
    /// <param name="noencoding">Disables parameters encoding.</param>
    public static string ToParametersString(this NameValueCollection source, string separator, bool noencoding)
    {
      return String.Join
      (
        separator,
        source.ToKeyValuePairCollection().Select
        (
          itm => String.Format
          (
            "{0}={1}",
            itm.Key,
            (noencoding && !String.IsNullOrEmpty(itm.Value) ? itm.Value : HttpUtility.UrlEncode(itm.Value))
          )
        ).ToArray()
      );
    }

    /// <summary>
    /// Sorts the <see cref="System.Collections.Specialized.NameValueCollection"/> by alphabetically and returns a new <see cref="System.Collections.Specialized.NameValueCollection"/>.
    /// </summary>
    /// <param name="source">The <see cref="System.Collections.Specialized.NameValueCollection"/>.</param>
    public static NameValueCollection Sort(this NameValueCollection source)
    {
      return source.ToKeyValuePairCollection().OrderBy
      ( // сортируем по алфавиту
        itm => itm.Key
      ).Aggregate
      ( // поебразуем обратно в NameValueCollection
        new NameValueCollection(),
        (r, itm) =>
        {
          r.Add(itm.Key, itm.Value);
          return r;
        }
      );
    }

    /// <summary>
    /// Removes the value with the specified key from the <see cref="System.Collections.Specialized.NameValueCollection"/>.
    /// </summary>
    /// <param name="source">The <see cref="System.Collections.Specialized.NameValueCollection"/>.</param>
    /// <param name="key">The key of the element to remove.</param>
    public static NameValueCollection RemoveKey(this NameValueCollection source, string key)
    {
      if (String.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
      source.Remove(key);
      return source;
    }

  }

}