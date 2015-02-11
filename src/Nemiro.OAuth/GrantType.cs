using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the authorization grant type.
  /// </summary>
  public struct GrantType
  {

    #region ..constants..

    /// <summary>
    /// Using an authorization code to confirm the identity (grant type is <b>authorization_code</b>).
    /// </summary>
    public const string AuthorizationCode = "authorization_code";

    /// <summary>
    /// Using username and password (grant type is <b>password</b>).
    /// </summary>
    public const string Password = "password";

    /// <summary>
    /// Using basic authorization with username and password (grant type is <b>client_credentials</b>). 
    /// </summary>
    public const string ClientCredentials = "client_credentials";

    /// <summary>
    /// refresh_token (not used).
    /// </summary>
    public const string RefreshToken = "refresh_token";

    #endregion
    #region ..properties..

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    internal string Value { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current value is <b>authorization_code</b> or not.
    /// </summary>
    public bool IsAuthorizationCode
    {
      get
      {
        return this.Value.Equals(GrantType.AuthorizationCode, StringComparison.OrdinalIgnoreCase);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the current value is <b>password</b> or not.
    /// </summary>
    public bool IsPassword
    {
      get
      {
        return this.Value.Equals(GrantType.Password, StringComparison.OrdinalIgnoreCase);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the current value is <b>client_credentials</b> or not.
    /// </summary>
    public bool IsClientCredentials
    {
      get
      {
        return this.Value.Equals(GrantType.ClientCredentials, StringComparison.OrdinalIgnoreCase);
      }
    }

    #endregion
    #region ..static methods..

    /// <summary>
    /// Initializes a new <see cref="GrantType"/> instance with a specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value of grant type.</param>
    public static GrantType Create(string value)
    {
      return new GrantType { Value = value };
    }

    #endregion
    #region ..overrides..

    /// <summary>
    /// Returns a string that represents the current <see cref="GrantType"/>.
    /// </summary>
    public override string ToString()
    {
      return this.Value;
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Converts the <see cref="GrantType"/> to <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The <see cref="GrantType"/> instance.</param>
    public static implicit operator string(GrantType value)
    {
      return value.ToString();
    }

    /// <summary>
    /// Creates a new <see cref="GrantType"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="GrantType"/>.</param>
    public static implicit operator GrantType(string value)
    {
      return GrantType.Create(value);
    }

    #endregion

  }

}