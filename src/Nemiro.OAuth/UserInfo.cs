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
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the user profile info.
  /// </summary>
  public class UserInfo
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets or sets a collection containing the API request results.
    /// </summary>
    public UniValue Items { get; protected internal set; }

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public string UserId { get; protected internal set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string UserName { get; protected internal set; }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; protected internal set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; protected internal set; }

    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    public string DisplayName { get; protected internal set; }

    /// <summary>
    /// Gets or sets user email address.
    /// </summary>
    public string Email { get; protected internal set; }

    /// <summary>
    /// Gets or sets user phone.
    /// </summary>
    public string Phone { get; protected internal set; }

    /// <summary>
    /// Gets or sets user birthday.
    /// </summary>
    public DateTime? Birthday { get; protected internal set; }

    /// <summary>
    /// Gets or sets user url.
    /// </summary>
    public string Url { get; protected internal set; }

    /// <summary>
    /// Gets or sets user image url.
    /// </summary>
    public string Userpic { get; protected internal set; }

    /// <summary>
    /// Gets or sets user language name or code. For example: en, ru, en-us, ru-ru.
    /// </summary>
    public string Language { get; protected internal set; }

    /// <summary>
    /// Gets or sets gender of the user.
    /// </summary>
    public Sex Sex { get; protected internal set; }

    /// <summary>
    /// Gets the first and last name.
    /// </summary>
    public string FullName
    {
      get
      {
        return String.Format("{0} {1}", this.FirstName, this.LastName).Trim();
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="UserInfo"/> class.
    /// </summary>
    /// <param name="source">The data source.</param>
    /// <param name="mapping">The mapping rules.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public UserInfo(Dictionary<string, object> source, ApiDataMapping mapping) : this(new UniValue(source), mapping) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserInfo"/> class.
    /// </summary>
    /// <param name="source">The data source.</param>
    /// <param name="mapping">The mapping rules.</param>
    public UserInfo(UniValue source, ApiDataMapping mapping)
    {
      if (mapping == null || !source.HasValue) { return; }

      this.Items = source;

      var t = typeof(UserInfo);

      foreach (var p in t.GetProperties())
      {
        var item = mapping.FirstOrDefault(itm => itm.DestinationName.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
        if (item != null && source.ContainsKey(item.SourceName))
        {
          object vr = null;
          UniValue vs = source[item.SourceName];
          if (item.Parse != null)
          {
            vr = item.Parse(vs);
          }
          else
          {
            if (item.Type == typeof(DateTime))
            {
              var f = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name, true);
              var formatDateTime = "dd.MM.yyyy HH:mm:ss";
              if (!String.IsNullOrEmpty(item.Format))
              {
                formatDateTime = item.Format;
              }
              f.DateTimeFormat.FullDateTimePattern = formatDateTime;
              f.DateTimeFormat.ShortDatePattern = formatDateTime;
              DateTime dateValue;
              if (DateTime.TryParse(vs.ToString(), f, DateTimeStyles.NoCurrentDateDefault, out dateValue))
              {
                vr = dateValue;
              }
              else
              {
                vr = null;
              }
            }
            else if (item.Type == typeof(bool))
            {
              vr = Convert.ToBoolean(vs);
            }
            else if (item.Type == typeof(Int16))
            {
              vr = Convert.ToInt16(vs);
            }
            else if (item.Type == typeof(Int32))
            {
              vr = Convert.ToInt32(vs);
            }
            else if (item.Type == typeof(Int64))
            {
              vr = Convert.ToInt64(vs);
            }
            else if (item.Type == typeof(UInt16))
            {
              vr = Convert.ToUInt16(vs);
            }
            else if (item.Type == typeof(UInt32))
            {
              vr = Convert.ToUInt32(vs);
            }
            else if (item.Type == typeof(UInt64))
            {
              vr = Convert.ToUInt64(vs);
            }
            else if (item.Type == typeof(double))
            {
              vr = Convert.ToDouble(vs);
            }
            else if (item.Type == typeof(Single))
            {
              vr = Convert.ToSingle(vs);
            }
            else if (item.Type == typeof(decimal))
            {
              vr = Convert.ToDecimal(vs);
            }
            else if (item.Type == typeof(byte))
            {
              vr = Convert.ToByte(vs);
            }
            else if (item.Type == typeof(char))
            {
              vr = Convert.ToChar(vs);
            }
            else if (item.Type == typeof(string))
            {
              vr = Convert.ToString(vs);
            }
            else
            {
              vr = Convert.ToString(vs);
            }
          }
          p.SetValue(this, vr, null);
        }
      }
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Returns the <see cref="UserName"/> or <see cref="UserId"/>.
    /// </summary>
    public override string ToString()
    {
      return this.UserName ?? this.UserId ?? this.GetType().Name;
    }

    #endregion

  }

}