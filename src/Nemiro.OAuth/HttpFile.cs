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
  /// Implements a file to transfer in a HTTP request.
  /// </summary>
  public class HttpFile : HttpParameter
  {

    /// <summary>
    /// Gets or sets the filename.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpFile"/> class.
    /// </summary>
    /// <param name="fileContent">Content of the file.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    public HttpFile(string parameterName, string fileName, HttpParameterValue fileContent) : this(parameterName, fileName, null, fileContent)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpFile"/> class.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="file">The posted file.</param>
    [Obsolete("Use overload.", false)]
    public HttpFile(string parameterName, System.Web.HttpPostedFile file) : this(parameterName, file.FileName, file.ContentType, file.InputStream)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpFile"/> class.
    /// </summary>
    /// <param name="fileContent">Content of the file.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="contentType">MIME type of the file.</param>
    public HttpFile(string parameterName, string fileName, string contentType, HttpParameterValue fileContent) : base(HttpParameterType.File, parameterName, fileContent, contentType)
    {
      if (String.IsNullOrEmpty(fileName)) // String.IsNullOrWhiteSpace
      {
        throw new ArgumentNullException("fileName");
      }
      if (String.IsNullOrEmpty(contentType)) // String.IsNullOrWhiteSpace
      {
        contentType = "application/octet-stream";
      }
      this.FileName = fileName;
    }

  }

}