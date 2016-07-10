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
  /// The exception that is thrown when a resource owner or authorization server denied the request.
  /// </summary>
  [Serializable]
  public class AccessDeniedException : AuthorizationException
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessDeniedException"/> class with a specified error message and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public AccessDeniedException(string message) : base(message ?? "The resource owner or authorization server denied the request.") { }

  }

}