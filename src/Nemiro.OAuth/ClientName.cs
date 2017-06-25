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
using System.Text.RegularExpressions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the name of the client.
  /// </summary>
  public struct ClientName
  {

    #region ..properties..

    private string _Key;

    /// <summary>
    /// Gets the client name.
    /// </summary>
    public string Key
    {
      get
      {
        return _Key;
      }
    }

    private string _ProviderName;

    /// <summary>
    /// Gets the provider name.
    /// </summary>
    public string ProviderName
    {
      get
      {
        return _ProviderName;
      }
    }

    private string _Hash;

    /// <summary>
    /// Gets a md5 hash code for the current instance of the <see cref="ClientName"/>.
    /// </summary>
    public string Hash 
    {
      get
      {
        return _Hash;
      }
      private set
      {
        _Hash = value;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientName"/>.
    /// </summary>
    /// <param name="providerName">The provider name.</param>
    internal ClientName(string providerName) : this(null, providerName) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientName"/>.
    /// </summary>
    /// <param name="key">The client name. Any string.</param>
    /// <param name="providerName">The provider name.</param>
    internal ClientName(string key, string providerName)
    {
      _Key = key;
      _ProviderName = providerName;
      _Hash = OAuthUtility.GetMD5Hash(String.Format("{0}/{1}", key, providerName).ToLower());
    }

    #endregion
    #region ..static methods..

    /// <summary>
    /// Returns a new <see cref="ClientName"/> instance with a specified <paramref name="providerName"/>.
    /// </summary>
    public static ClientName Create(string providerName)
    {
      return new ClientName(null, providerName);
    }

    /// <summary>
    /// Returns a new <see cref="ClientName"/> instance with a specified <paramref name="key"/> and <paramref name="providerName"/>.
    /// </summary>
    public static ClientName Create(string key, string providerName)
    {
      return new ClientName(key, providerName);
    }

    /// <summary>
    /// Converts a specified string to <see cref="ClientName"/>.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    public static ClientName Parse(string value)
    {
      if (String.IsNullOrEmpty(value))
      {
        return new ClientName(null, null);
      }

      // value is md5 hash
      if (Regex.IsMatch(value, "[0-9a-f]{32}"))
      {
        return new ClientName(null, null) { Hash = value };
      }

      // escaping slash
      value = ClientName.Encode(value);

      // value contains slash
      int slash = value.IndexOf("/");

      if (slash == -1)
      {
        // does not contain a slash, is provider name
        return new ClientName(null, ClientName.Decode(value));
      }

      // contain a slash, is client name and provider name
      return new ClientName
      (
        ClientName.Decode(value.Substring(0, slash)), 
        ClientName.Decode(value.Substring(slash + 1, value.Length - slash - 1))
      );
    }

    /// <summary>
    /// Encodes a string.
    /// </summary>
    /// <param name="value">The string to encode. </param>
    private static string Encode(string value)
    {
      return value.Replace("//", ((char)0).ToString());
    }

    /// <summary>
    /// Decodes a string.
    /// </summary>
    /// <param name="value">The string to decode.</param>
    private static string Decode(string value)
    {
      return value.Replace(((char)0).ToString(), "/");
    }

    /// <summary>
    /// Escapes a special characters in a string.
    /// </summary>
    /// <param name="value">The string to escape.</param>
    internal static string Escape(string value)
    {
      if (String.IsNullOrEmpty(value)) { return value; }

      return value.Replace("/", "//");
    }

    /// <summary>
    /// Converts any escaped characters in the input string.
    /// </summary>
    /// <param name="value">The input string containing the value to convert.</param>
    internal static string Unescape(string value)
    {
      if (String.IsNullOrEmpty(value)) { return value; }

      return value.Replace("//", "/");
    }

    #endregion
    #region ..overrides..

    /// <summary>
    /// Returns a string that represents the current <see cref="ClientName"/>.
    /// </summary>
    public override string ToString()
    {
      if (String.IsNullOrEmpty(this.Key))
      {
        return this.ProviderName;
      }
      else
      {
        return String.Format("{0}/{1}", ClientName.Escape(this.Key), ClientName.Escape(this.ProviderName));
      }
    }

    /// <summary>
    /// Determines whether two object instances are equal.
    /// </summary>
    /// <param name="o">The object to compare with the current instance of the <see cref="ClientName"/>.</param>
    /// <returns><b>true</b> if the specified object is equal to the current <see cref="ClientName"/>; otherwise, <b>false</b>.</returns>
    public override bool Equals(object o)
    {
      if (o == null || (o.GetType() == typeof(string) && String.IsNullOrEmpty(o.ToString()))) { return false; }
      if (o.GetType() == typeof(ClientName))
      {
        return this.Equals((ClientName)o);
      }
      else
      {
        return this.Equals(ClientName.Create(o.ToString()));
      }
    }

    /// <summary>
    /// Determines whether two object instances are equal.
    /// </summary>
    /// <param name="value">The instance of <see cref="ClientName"/> to compare with the current instance of the <see cref="ClientName"/>.</param>
    /// <returns><b>true</b> if the specified <see cref="ClientName"/> instance is equal to the current <see cref="ClientName"/>; otherwise, <b>false</b>.</returns>
    public bool Equals(ClientName value)
    {
      return this.Hash.Equals(value.Hash, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>A hash code for the current <see cref="ClientName"/>.</returns>
    public override int GetHashCode()
    {
      return this.Hash.GetHashCode();
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Converts the <see cref="ClientName"/> to <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The <see cref="ClientName"/> instance.</param>
    public static implicit operator string(ClientName value)
    {
      return value.ToString();
    }

    /// <summary>
    /// Creates a new <see cref="ClientName"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="ClientName"/>.</param>
    public static implicit operator ClientName(string value)
    {
      return ClientName.Parse(value);
    }

    #endregion
    #region ..conditional operators..

    /// <summary>
    /// Indicate whether two <see cref="ClientName"/> are not equal.
    /// </summary>
    /// <param name="x">The first <see cref="ClientName"/> instance.</param>
    /// <param name="y">The second <see cref="ClientName"/> instance.</param>
    public static bool operator !=(ClientName x, ClientName y)
    {
      return !x.Equals(y);
    }

    /// <summary>
    /// Indicate whether two <see cref="ClientName"/> are equal.
    /// </summary>
    /// <param name="x">The first <see cref="ClientName"/> instance.</param>
    /// <param name="y">The second <see cref="ClientName"/> instance.</param>
    public static bool operator ==(ClientName x, ClientName y)
    {
      return x.Equals(y);
    }

    #endregion

  }

}