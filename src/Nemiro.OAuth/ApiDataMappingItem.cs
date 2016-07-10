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
  /// Represents data mapping item for API results.
  /// </summary>
  public class ApiDataMappingItem
  {

    /// <summary>
    /// Gets or sets the key name in the data source.
    /// </summary>
    public string SourceName { get; set; }

    /// <summary>
    /// Gets or set the property name in the destination object.
    /// </summary>
    public string DestinationName { get; set; }

    /// <summary>
    /// Gets or sets the data type of the property in the destination object.
    /// </summary>
    public Type Type { get; set; }

    /// <summary>
    /// Gets or sets the data format.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Gets or sets a custom parser of the data.
    /// </summary>
    public CustomParse Parse { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiDataMappingItem"/> class.
    /// </summary>
    public ApiDataMappingItem() { }

  }

}