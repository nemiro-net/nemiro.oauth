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
using System.Collections.Generic;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents data mapping collection for API results.
  /// </summary>
  public class ApiDataMapping : List<ApiDataMappingItem>
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiDataMapping"/> class.
    /// </summary>
    public ApiDataMapping() { }

    /// <summary>
    /// Adds the specified data mapping to the collection.
    /// </summary>
    /// <param name="sourceName">The key name in the data source.</param>
    /// <param name="destinationName">The property name in the destination object.</param>
    /// <param name="type">The data type.</param>
    /// <param name="format">The data format. For example: "dd.MM.yyyy" for dates, or: "00" for numerics, etc.</param>
    /// <param name="parse">Custom parser of the data.</param>
    public void Add(string sourceName, string destinationName, Type type, string format, CustomParse parse)
    {
      this.Add
      (
        new ApiDataMappingItem
        {
          SourceName = sourceName,
          DestinationName = destinationName,
          Type = type,
          Format = format,
          Parse = parse
        }
      );
    }

    /// <summary>
    /// Adds the specified data mapping to the collection.
    /// </summary>
    /// <param name="sourceName">The key name in the data source.</param>
    /// <param name="destinationName">The property name in the destination object.</param>
    public void Add(string sourceName, string destinationName)
    {
      this.Add(sourceName, destinationName, null, null, null);
    }

    /// <summary>
    /// Adds the specified data mapping to the collection.
    /// </summary>
    /// <param name="sourceName">The key name in the data source.</param>
    /// <param name="destinationName">The property name in the destination object.</param>
    /// <param name="type">The data type.</param>
    public void Add(string sourceName, string destinationName, Type type)
    {
      this.Add(sourceName, destinationName, type, null, null);
    }

    /// <summary>
    /// Adds the specified data mapping to the collection.
    /// </summary>
    /// <param name="sourceName">The key name in the data source.</param>
    /// <param name="destinationName">The property name in the destination object.</param>
    /// <param name="type">The data type.</param>
    /// <param name="format">The data format. For example: "dd.MM.yyyy" for dates, or: "00" for numerics, etc.</param>
    public void Add(string sourceName, string destinationName, Type type, string format)
    {
      this.Add(sourceName, destinationName, type, format, null);
    }

    /// <summary>
    /// Adds the specified data mapping to the collection.
    /// </summary>
    /// <param name="sourceName">The key name in the data source.</param>
    /// <param name="destinationName">The property name in the destination object.</param>
    /// <param name="parse">Custom parser of the data.</param>
    public void Add(string sourceName, string destinationName, CustomParse parse)
    {
      this.Add(sourceName, destinationName, null, null, parse);
    }

  }

}