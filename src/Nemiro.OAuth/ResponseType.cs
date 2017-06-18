// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2017. All rights reserved.
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

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the response type.
  /// </summary>
  public struct ResponseType
  {

    #region ..constants..

    /// <summary>
    /// The server must return an authorization code.
    /// </summary>
    public const string Code = "code";

    /// <summary>
    /// The server must return an access token.
    /// </summary>
    public const string Token = "token";

    #endregion
    #region ..properties..

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    internal string Value { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current value is <b>code</b> or not.
    /// </summary>
    public bool IsCode
    {
      get
      {
        return this.Value.Equals(ResponseType.Code, StringComparison.OrdinalIgnoreCase);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the current value is <b>token</b> or not.
    /// </summary>
    public bool IsToken
    {
      get
      {
        return this.Value.Equals(ResponseType.Token, StringComparison.OrdinalIgnoreCase);
      }
    }

    #endregion
    #region ..static methods..

    /// <summary>
    /// Initializes a new <see cref="ResponseType"/> instance with a specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value of response type.</param>
    public static ResponseType Create(string value)
    {
      return new ResponseType { Value = value };
    }

    #endregion
    #region ..overrides..

    /// <summary>
    /// Returns a string that represents the current <see cref="ResponseType"/>.
    /// </summary>
    public override string ToString()
    {
      return this.Value;
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Converts the <see cref="ResponseType"/> to <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The <see cref="ResponseType"/> instance.</param>
    public static implicit operator string(ResponseType value)
    {
      return value.ToString();
    }

    /// <summary>
    /// Creates a new <see cref="ResponseType"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="ResponseType"/>.</param>
    public static implicit operator ResponseType(string value)
    {
      return ResponseType.Create(value);
    }

    #endregion

  }

}