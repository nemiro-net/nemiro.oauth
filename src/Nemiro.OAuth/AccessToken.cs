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
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents base properties and method for access token results.
  /// </summary>
  [Serializable]
  public class AccessToken : RequestResult
  {

    #region ..fields & properties..

    /// <summary>
    /// The access token issued by the authorization server.
    /// </summary>
    public string Value { get; protected set; }

    /// <summary>
    /// Represents the empty <see cref="AccessToken"/>.
    /// </summary>
    public static new AccessToken Empty
    {
      get
      {
        return new AccessToken() { Value = null };
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="AccessToken"/> is empty or not.
    /// </summary>
    public new bool IsEmpty
    {
      get
      {
        return String.IsNullOrEmpty(this.Value);
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessToken"/> class.
    /// </summary>
    protected AccessToken() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessToken"/> class.
    /// </summary>
    /// <param name="result">Result of request to the OAuth server.</param>
    internal AccessToken(RequestResult result) : base(result) { }

    #endregion
    #region ..methods..

    /// <summary>
    /// Returns the <see cref="AccessToken.Value"/>.
    /// </summary>
    public override string ToString()
    {
      return this.Value;
    }

    /// <summary>
    /// Converts the specified string to an <see cref="AccessToken"/>.
    /// </summary>
    /// <param name="value">A string containing an access token to parse.</param>
    /// <returns>A new <see cref="AccessToken"/> instance.</returns>
    public static AccessToken Parse(string value)
    {
      return AccessToken.Parse<AccessToken>(value);
    }

    /// <summary>
    /// Converts the specified string to an <see cref="AccessToken"/>.
    /// </summary>
    /// <typeparam name="T">Type Inherited from the <see cref="AccessToken"/> that should be returned.</typeparam>
    /// <param name="value">A string containing an access token to parse.</param>
    /// <returns>A new <typeparamref name="T"/> instance.</returns>
    public static T Parse<T>(string value) where T : AccessToken
    {
      if (String.IsNullOrEmpty(value)) { return (T)AccessToken.Empty; }

      var result = new RequestResult("text/plain", value);

      if (result["oauth_token"].HasValue && result["oauth_token_secret"].HasValue)
      {
        /*if (typeof(T) != typeof(OAuthAccessToken))
        {
          throw new AccessTokenException("Found access token for protocol version 1.0, which is incompatible with the version 2.0");
        }*/
        return (T)(AccessToken)new OAuthAccessToken(result);
      }
      else if (result["access_token"].HasValue)
      {
        /*if (typeof(T) != typeof(OAuth2AccessToken))
        {
          throw new AccessTokenException("Found access token for protocol version 2.0, which is incompatible with the version 1.0.");
        }*/
        return (T)(AccessToken)new OAuth2AccessToken(result);
      }

      // value
      var r = (T)Activator.CreateInstance(typeof(T), true);

      r.Value = value;

      return r;
    }

    /// <summary>
    /// Indicates whether the specified value is null or an <see cref="AccessToken.Empty"/>.
    /// </summary>
    /// <param name="value">The <see cref="AccessToken"/> instance to test.</param>
    /// <returns><b>true</b> if the <paramref name="value"/> parameter is <b>null</b> or <see cref="AccessToken.IsEmpty"/> is <b>false</b>; otherwise, <b>false</b>.</returns>
    public static bool IsNullOrEmpty(AccessToken value)
    {
      try
      {
        return value.IsEmpty;
      }
      catch
      {
        return true;
      }
    }

    #endregion
    #region ..iserializable..

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessToken"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected AccessToken(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      this.Value = info.GetString("Value");
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

      info.AddValue("Value", this.Value);

      base.GetObjectData(info, context);
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Converts the <see cref="AccessToken"/> to <see cref="System.String"/>.
    /// </summary>
    /// <param name="v">The <see cref="AccessToken"/> instance.</param>
    public static implicit operator string(AccessToken v)
    {
      return v.Value;
    }

    /// <summary>
    /// Creates a new <see cref="AccessToken"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="AccessToken"/>.</param>
    public static implicit operator AccessToken(string value)
    {
      return AccessToken.Parse(value);
    }

    #endregion

  }

}