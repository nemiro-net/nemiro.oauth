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
  /// The exception occurs when you try to access a provider by provider name. If the name is incorrect, or does not exist.
  /// </summary>
  [Serializable]
  public class UnknownProviderException : Exception
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownProviderException"/> class.
    /// </summary>
    public UnknownProviderException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownProviderException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public UnknownProviderException(string message, params object[] args) : base(String.Format(message, args)) { }

  }

}