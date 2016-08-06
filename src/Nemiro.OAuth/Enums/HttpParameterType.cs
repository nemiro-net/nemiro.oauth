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
    /// Unformed parameter.
    /// </summary>
    Unformed = 0,
    /// <summary>
    /// Parameter of the query string.
    /// </summary>
    Url = 1,
    /// <summary>
    /// Parameter of the form.
    /// </summary>
    Form = 2,
    /// <summary>
    /// File.
    /// </summary>
    File = 4,
    /// <summary>
    /// Body of the request.
    /// </summary>
    RequestBody = 8,
    /// <summary>
    /// Do not encode parameter names.
    /// </summary>
    OptDonotEncodeKeys = 1024
  }

}