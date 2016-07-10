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
  /// The exception that is thrown when adding a <see cref="HttpFile"/> to the <see cref="HttpParameterCollection"/> and collection has one a <see cref="HttpRequestBody"/>.
  /// </summary>
  [Serializable]
  public class IncompatibleHttpParametersException : RequestException
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="IncompatibleHttpParametersException"/>.
    /// </summary>
    public IncompatibleHttpParametersException() : base("Parameters contain request body. Cannot add file. Remove request body and try again.") { }

  }

}