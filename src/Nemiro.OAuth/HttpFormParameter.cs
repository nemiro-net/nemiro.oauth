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
namespace Nemiro.OAuth
{

  /// <summary>
  /// Implements a form parameter.
  /// </summary>
  public class HttpFormParameter : HttpParameter
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpFormParameter"/> class with a specified parameter name and value.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    /// <param name="name">The parameter name.</param>
    public HttpFormParameter(string name, HttpParameterValue value) : base(HttpParameterType.Form, name, value, null) { }

  }

}