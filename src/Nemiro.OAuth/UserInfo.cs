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
    public Dictionary<string, object> Items { get; protected internal set; }

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
    public UserInfo(Dictionary<string, object> source, ApiDataMapping mapping)
    {
      this.Items = source;
      var t = typeof(UserInfo);
      foreach (var p in t.GetProperties())
      {
        var item = mapping.FirstOrDefault(itm => itm.DestinationName == p.Name);
        if (item != null && source.ContainsKey(item.SourceName))
        {
          object value = source[item.SourceName];
          if (item.Parse != null)
          {
            value = item.Parse(value);
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
              if (DateTime.TryParse(value.ToString(), f, DateTimeStyles.NoCurrentDateDefault, out dateValue))
              {
                value = dateValue;
              }
              else
              {
                value = null;
              }
            }
            else if (item.Type == typeof(bool))
            {
              value = Convert.ToBoolean(value);
            }
            else if (item.Type == typeof(Int16))
            {
              value = Convert.ToInt16(value);
            }
            else if (item.Type == typeof(Int32))
            {
              value = Convert.ToInt32(value);
            }
            else if (item.Type == typeof(Int64))
            {
              value = Convert.ToInt64(value);
            }
            else if (item.Type == typeof(UInt16))
            {
              value = Convert.ToUInt16(value);
            }
            else if (item.Type == typeof(UInt32))
            {
              value = Convert.ToUInt32(value);
            }
            else if (item.Type == typeof(UInt64))
            {
              value = Convert.ToUInt64(value);
            }
            else if (item.Type == typeof(double))
            {
              value = Convert.ToDouble(value);
            }
            else if (item.Type == typeof(Single))
            {
              value = Convert.ToSingle(value);
            }
            else if (item.Type == typeof(decimal))
            {
              value = Convert.ToDecimal(value);
            }
            else if (item.Type == typeof(byte))
            {
              value = Convert.ToByte(value);
            }
            else if (item.Type == typeof(char))
            {
              value = Convert.ToChar(value);
            }
            else if (item.Type == typeof(string))
            {
              value = Convert.ToString(value);
            }
          }
          p.SetValue(this, value, null);
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
      return this.UserName ?? this.UserId;
    }

    #endregion

  }

}