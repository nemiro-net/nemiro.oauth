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
  /// The exception that is thrown when <b>HttpContext.Current</b> is <b>null</b> (<b>Nothing</b> in Visual Basic).
  /// </summary>
  /// <remarks>
  /// <para>The exception that is thrown when you try to access methods that are designed for web projects.</para>
  /// </remarks>
  [Serializable]
  public class NullHttpContextException : Exception
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="NullHttpContextException"/> class.
    /// </summary>
    public NullHttpContextException() : base() { }

  }

}