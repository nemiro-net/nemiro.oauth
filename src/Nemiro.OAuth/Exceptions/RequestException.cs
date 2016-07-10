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
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;

namespace Nemiro.OAuth
{

  /// <summary>
  /// The exception that is thrown when an error occurs while accessing the network.
  /// </summary>
  [Serializable]
  public class RequestException : Exception
  {

    #region ..fields & properties..

    /// <summary>
    /// Instance of the <see cref="RequestResult"/>.
    /// </summary>
    public RequestResult RequestResult { get; protected set; }

    /// <summary>
    /// Gets the HTTP status code of the output returned to the client.
    /// </summary>
    public int StatusCode
    {
      get
      {
        return this.RequestResult.StatusCode;
      }
    }

    /// <summary>
    /// Gets the content type of the response.
    /// </summary>
    public string ContentType
    {
      get
      {
        return this.RequestResult.ContentType;
      }
    }

    /// <summary>
    /// Gets the http headers of the response.
    /// </summary>
    public NameValueCollection HttpHeaders
    {
      get
      {
        return this.RequestResult.HttpHeaders;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestException"/> class with a specified server request result, content type and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <b>null</b> reference (<b>Nothing</b> in Visual Basic) if no inner exception is specified.</param>
    [Obsolete("Please use an overloads. // v1.5", false)]
    public RequestException(string contentType, string result, Exception innerException) : base(innerException != null ? innerException.Message : "Request exception.", innerException)
    {
      this.RequestResult = new RequestResult(contentType, result);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestException"/> class with a specified server request result, content type and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="result">The result of the request.</param>
    /// <param name="contentType">The content type of the server request result.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a <b>null</b> reference (<b>Nothing</b> in Visual Basic) if no inner exception is specified.</param>
    /// <param name="headers">The HTTP headers of the output.</param>
    /// <param name="statusCode">The HTTP status code of the output.</param>
    public RequestException
    (
      string contentType = null,
      byte[] result = null,
      Exception innerException = null,
      NameValueCollection headers = null,
      int statusCode = 0
    ) : base(innerException != null ? innerException.Message : "Request exception.", innerException)
    {
      this.RequestResult = new RequestResult(contentType, result, headers, statusCode);
    }

    #endregion
    #region ..serialization..

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestException"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected RequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
      this.RequestResult = (RequestResult)info.GetValue("RequestResult", typeof(RequestResult));
    }

    /// <summary>
    /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
      info.AddValue("RequestResult", this.RequestResult);
      base.GetObjectData(info, context);
    }

    #endregion

  }

}