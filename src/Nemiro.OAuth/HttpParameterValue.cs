// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014-2015. All rights reserved.
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
using System.IO;
using System.Web;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Implements a value of the HTTP paramter.
  /// </summary>
  public class HttpParameterValue
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets or sets value.
    /// </summary>
    public object Value { get; set; }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterValue"/> class.
    /// </summary>
    public HttpParameterValue()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterValue"/> class with a specified value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public HttpParameterValue(object value)
    {
      this.Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterValue"/> class with a specified value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public HttpParameterValue(byte[] value)
    {
      this.Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameterValue"/> class with a specified value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public HttpParameterValue(Stream value)
    {
      this.Value = value;
      // TODO: Think...
      /*List<byte> result = new List<byte>();
      using (BinaryReader br = new BinaryReader(value))
      {
        int bytesRead = 0;
        byte[] buffer = new byte[1023];
        while ((bytesRead = br.Read(buffer, 0, buffer.Length)) != 0)
        {
          byte[] b = null;
          Array.Copy(buffer, b, bytesRead);
          result.AddRange(b);
          Array.Clear(buffer, 0, buffer.Length);
        }
      }
      this.Value = result.ToArray();*/
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Returns an encoded string of the value.
    /// </summary>
    public string ToEncodedString()
    {
      return this.ToEncodedString(UrlEncodingType.Default);
    }

    /// <summary>
    /// Returns an encoded string of the value.
    /// </summary>
    public string ToEncodedString(UrlEncodingType encodingType)
    {
      if (this.Value == null) { return null; }
      return OAuthUtility.UrlEncode(this.ToString(), encodingType);
    }

    /// <summary>
    /// Returns a string that represents the current value.
    /// </summary>
    public override string ToString()
    {
      if (this.Value == null) { return null; }
      if (this.Value.GetType() == typeof(byte[]))
      {
        return Convert.ToBase64String((byte[])this.Value);
      }
      return this.Value.ToString();
    }

    /// <summary>
    /// Returns a byte array that represents the current value.
    /// </summary>
    public byte[] ToByteArray()
    {
      if (this.Value == null) { return null; }
      if (this.Value.GetType() == typeof(byte[]))
      {
        return (byte[])this.Value;
      }
      else if (this.Value.GetType() == typeof(Stream) || this.Value.GetType().IsSubclassOf(typeof(Stream)))
      {
        if (((Stream)this.Value).CanSeek)
        {
          ((Stream)this.Value).Position = 0;
        }
        using (BinaryReader br = new BinaryReader((Stream)this.Value))
        {
          List<byte> result = new List<byte>();
          int bytesRead = 0;
          byte[] buffer = new byte[4095];
          while ((bytesRead = br.Read(buffer, 0, buffer.Length)) != 0)
          {
            byte[] b = new byte[bytesRead];
            Array.Copy(buffer, b, bytesRead);
            result.AddRange(b);
            Array.Clear(b, 0, b.Length);
          }
          return result.ToArray();
        }
      }
      return Encoding.Default.GetBytes(this.Value.ToString());
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Int16"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Int16 value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Int32"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Int32 value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Int64"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Int64 value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.UInt16"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(UInt16 value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.UInt32"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(UInt32 value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.UInt64"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(UInt64 value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Decimal"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Decimal value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Double"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Double value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Single"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Single value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.DateTime"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(DateTime value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(String value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Byte"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Byte value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the array of the <see cref="System.Byte"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Byte[] value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.IO.Stream"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(Stream value)
    {
      return new HttpParameterValue(value);
    }

    /// <summary>
    /// Implements the assignment operator for the <see cref="System.Web.HttpPostedFile"/>.
    /// </summary>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>New instance of the <see cref="HttpParameterValue"/>.</returns>
    public static implicit operator HttpParameterValue(HttpPostedFile value)
    {
      return new HttpParameterValue(value.InputStream);
    }

    #endregion

  }

}