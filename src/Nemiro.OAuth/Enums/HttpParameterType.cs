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
  /// The list of the types a HTTP parameters.
  /// </summary>
  [Flags]
  public enum HttpParameterType
  {
    /// <summary>
    /// Not specified.
    /// </summary>
    None = 0,
    /// <summary>
    /// Unformed parameter.
    /// </summary>
    Unformed = 1,
    /// <summary>
    /// Parameter of the query string.
    /// </summary>
    Url = 2,
    /// <summary>
    /// Parameter of the form.
    /// </summary>
    Form = 4,
    /// <summary>
    /// File.
    /// </summary>
    File = 8,
    /// <summary>
    /// Body of the request.
    /// </summary>
    RequestBody = 16,
    /// <summary>
    /// Do not encode parameter names.
    /// </summary>
    OptDonotEncodeKeys = 1024
  }

}