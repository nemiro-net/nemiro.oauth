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
  /// The access token class for OAuth 1.0.
  /// </summary>
  [Serializable]
  public class OAuthAccessToken : AccessToken
  {

    #region ..fields & properties..

    // NOTE: Value - for usability

    /// <summary>
    /// The access token issued by the authorization server.
    /// </summary>
    public new string Value
    {
      get
      {
        return base.Value;
      }
      protected set
      {
        base.Value = value;
      }
    }

    /// <summary>
    /// The access token issued by the authorization server.
    /// </summary>
    public string TokenSecret { get; protected set; }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAccessToken"/>.
    /// </summary>
    protected OAuthAccessToken() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAccessToken"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected OAuthAccessToken(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }

      this.TokenSecret = info.GetString("TokenSecret");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAccessToken"/> class.
    /// </summary>
    /// <param name="result">Result of request to the OAuth server.</param>
    public OAuthAccessToken(RequestResult result) : base(result)
    {
      this.Value = result["oauth_token"].ToString();
      this.TokenSecret = result["oauth_token_secret"].ToString();
    }

    #endregion
    #region ..methods..

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

      info.AddValue("TokenSecret", this.TokenSecret);

      base.GetObjectData(info, context);
    }

    /// <summary>
    /// Returns the <see cref="Value"/>.
    /// </summary>
    public override string ToString()
    {
      return this.Value;
    }

    /// <summary>
    /// Converts the specified string to an <see cref="OAuthAccessToken"/>.
    /// </summary>
    /// <param name="value">A string containing an access token to parse.</param>
    /// <returns>A new <see cref="OAuthAccessToken"/> instance.</returns>
    public new static OAuthAccessToken Parse(string value)
    {
      return AccessToken.Parse<OAuthAccessToken>(value);
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Converts the <see cref="OAuthAccessToken"/> to <see cref="System.String"/>.
    /// </summary>
    /// <param name="v">The <see cref="OAuthAccessToken"/> instance.</param>
    public static implicit operator string(OAuthAccessToken v)
    {
      return v.Value;
    }

    /// <summary>
    /// Creates a new <see cref="OAuthAccessToken"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="OAuthAccessToken"/>.</param>
    public static implicit operator OAuthAccessToken(string value)
    {
      return OAuthAccessToken.Parse(value);
    }

    #endregion


  }

}