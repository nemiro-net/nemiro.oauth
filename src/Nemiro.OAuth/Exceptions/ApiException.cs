// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014. All rights reserved.
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

namespace Nemiro.OAuth
{

  /// <summary>
  /// The exception that is thrown when server of API returns error.
  /// </summary>
  public class ApiException : RequestException
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public ApiException(string message) : this(message, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified error message and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.</param>
    public ApiException(string message, Exception innerException) : this("text/plain", message ?? "API error. Can't execute request.", innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified <see cref="RequestResult"/> and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.</param>
    public ApiException(RequestResult result, Exception innerException) : this(result.ContentType, result.Source, innerException) { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified <see cref="RequestResult"/> and error message.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public ApiException(RequestResult result, string message) : this(result.ContentType, result.Source, message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified server request result, content type and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    public ApiException(string contentType, string result, Exception innerException) : base(contentType, result, innerException) { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified server request result, content type and a error message.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    public ApiException(string contentType, string result, string message) : base(contentType, result, new ApiException(message)) { }

  }

}
