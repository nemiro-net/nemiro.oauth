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
using System.Text;

namespace Nemiro.OAuth
{

  /// <summary>
  /// The exception that is thrown when server of API returns error.
  /// </summary>
  [Serializable]
  public class ApiException : RequestException
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified <see cref="RequestResult"/> and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <b>null</b> reference (<b>Nothing</b> in Visual Basic) if no inner exception is specified.</param>
    public ApiException(RequestResult result, Exception innerException) : base(result.ContentType, result.Source, innerException, result.HttpHeaders, result.StatusCode) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified <see cref="RequestResult"/> and error message.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public ApiException(RequestResult result, string message) : base(result.ContentType, result.Source, new Exception(message), result.HttpHeaders, result.StatusCode) { }

    #region [obsolete]

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public ApiException(string message) : base("text/plain", Encoding.UTF8.GetBytes(message), null, null, 0) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified error message and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <b>null</b> reference (<b>Nothing</b> in Visual Basic) if no inner exception is specified.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public ApiException(string message, Exception innerException) : base("text/plain", Encoding.UTF8.GetBytes(message ?? "API error. Can't execute request."), innerException, null, 0) { }


    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified server request result, content type and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <b>null</b> reference (<b>Nothing</b> in Visual Basic) if no inner exception is specified.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public ApiException(string contentType, string result, Exception innerException) : base(contentType, Encoding.UTF8.GetBytes(result), innerException, null, 0) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified server request result, content type and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <b>null</b> reference (<b>Nothing</b> in Visual Basic) if no inner exception is specified.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public ApiException(string contentType, byte[] result, Exception innerException) : base(contentType, result, innerException, null, 0) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified server request result, content type and a error message.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public ApiException(string contentType, string result, string message) : base(contentType, Encoding.UTF8.GetBytes(result), new ApiException(message), null, 0) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified server request result, content type and a error message.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public ApiException(string contentType, byte[] result, string message) : base(contentType, result, new ApiException(message), null) { }

    #endregion

  }

}