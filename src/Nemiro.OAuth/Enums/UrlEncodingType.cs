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
namespace Nemiro.OAuth
{

  /// <summary>
  /// The list of url encding methods.
  /// </summary>
  public enum UrlEncodingType
  {
    /// <summary>
    /// Without encoding.
    /// </summary>
    None,
    /// <summary>
    /// <see cref="UrlEncodingType.PercentEncoding"/> for POST requests when a conetent-type is x-www-form-urlencoded.
    /// And <see cref="UrlEncodingType.Default"/> for other requests.
    /// </summary>
    Auto,
    /// <summary>
    /// x-www-form-urlencoded (spaces encoded as plus (+) signs).
    /// </summary>
    Default,
    /// <summary>
    /// RFC 3986 (spaces encoded as %20).
    /// </summary>
    PercentEncoding
  }

}