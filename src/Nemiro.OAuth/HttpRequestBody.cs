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
using System.IO;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Implements a request body.
  /// </summary>
  public class HttpRequestBody : HttpParameter
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestBody"/> class with a specified value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public HttpRequestBody(byte[] value) : base(HttpParameterType.RequestBody, "RequestBody", value, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestBody"/> class with a specified value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public HttpRequestBody(string value) : base(HttpParameterType.RequestBody, "RequestBody", value, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestBody"/> class with a specified value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public HttpRequestBody(Stream value) : base(HttpParameterType.RequestBody, "RequestBody", value, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestBody"/> class with a specified value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public HttpRequestBody(object value) : base(HttpParameterType.RequestBody, "RequestBody", new HttpParameterValue(value), null) { }

  }

}