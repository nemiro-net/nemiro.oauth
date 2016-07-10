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

namespace Nemiro.OAuth
{

  /// <summary>
  /// Implements a HTTP paramter.
  /// </summary>
  public class HttpParameter
  {

    /// <summary>
    /// Gets or sets parameter name.
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Gets or sets parameter value.
    /// </summary>
    public HttpParameterValue Value { get; protected set; }

    /// <summary>
    /// Gets or sets Content-Type.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets type of the parameter.
    /// </summary>
    public HttpParameterType ParameterType { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameter"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="contentType">The content-type of the parameter.</param>
    internal HttpParameter(string name, HttpParameterValue value, string contentType = null) : this(HttpParameterType.Unformed, name, value, contentType) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpParameter"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="parameterType">The type of the parameter.</param>
    /// <param name="contentType">The content-type of the parameter.</param>
    protected HttpParameter(HttpParameterType parameterType, string name, HttpParameterValue value, string contentType)
    {
      this.ParameterType = parameterType;
      this.Name = name;
      this.Value = value;
      this.ContentType = contentType;
    }

    /// <summary>
    /// Returns a string that represents the current parameter.
    /// </summary>
    public override string ToString()
    {
      if (!String.IsNullOrEmpty(this.Name))
      {
        return String.Format("{0}={1}", this.Name, (this.Value != null ? this.Value.ToEncodedString() : null));
      }
      else
      {
        return (this.Value != null ? this.Value.ToEncodedString() : base.ToString());
      }
    }

  }

}