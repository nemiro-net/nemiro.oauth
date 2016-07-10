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
using System.Text;
using System.Security.Cryptography;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the signature of the request. This is a helper class to simplify debugging.
  /// </summary>
  public class OAuthSignature
  {

    #region ..fields & properties..

    private string _Key = null;

    /// <summary>
    /// Gets the secret key for encryption. 
    /// </summary>
    public string Key
    {
      get
      {
        return _Key;
      }
    }

    private string _SignatureMethod = null;

    /// <summary>
    /// Gets the name of hashing algorithm to calculate the signature: HMAC-SHA1 or PLAINTEXT.
    /// </summary>
    public string SignatureMethod
    {
      get
      {
        return _SignatureMethod;
      }
    }

    private string _BaseString = null;

    /// <summary>
    /// Get base string of the signature.
    /// </summary>
    public string BaseString
    {
      get
      {
        return _BaseString;
      }
    }

    private string _Value = null;

    /// <summary>
    /// Gets the signature.
    /// </summary>
    public string Value
    {
      get
      {
        return _Value;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthSignature"/> class.
    /// </summary>
    /// <param name="signatureMethod">The name of hashing algorithm to calculate the signature: HMAC-SHA1 (default) or PLAINTEXT.</param>
    /// <param name="key">The secret key for encryption.</param>
    /// <param name="baseString">Base string of the signature.</param>
    /// <exception cref="NotSupportedException">
    /// <para><paramref name="signatureMethod"/> is not suppored.</para>
    /// </exception>
    public OAuthSignature(string signatureMethod, string key, string baseString)
    {
      _BaseString = baseString;
      _Key = key;
      _SignatureMethod = signatureMethod;

      switch (signatureMethod)
      {
        case SignatureMethods.PLAINTEXT:
          _Value = key;
          break;

        case SignatureMethods.HMACSHA1:
          _Value = Convert.ToBase64String
          (
            new HMACSHA1(Encoding.ASCII.GetBytes(key)).ComputeHash
            (
              Encoding.ASCII.GetBytes(baseString)
            )
          );
          break;

        case SignatureMethods.RSASHA1:
          var formatter = new RSAPKCS1SignatureFormatter(new RSACryptoServiceProvider());
          formatter.SetHashAlgorithm("SHA1");
          _Value = Convert.ToBase64String
          (
            formatter.CreateSignature
            (
              Encoding.ASCII.GetBytes(baseString)
            )
          );
          break;

        default:
          throw new NotSupportedException(String.Format("Signature method \"{0}\" not suppored.", signatureMethod));
      }
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Returns the <see cref="OAuthSignature.Value"/> of the current object.
    /// </summary>
    public override string ToString()
    {
      return this.Value;
    }

    #endregion

  }

}