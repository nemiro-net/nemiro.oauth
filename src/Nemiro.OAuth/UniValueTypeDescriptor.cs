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
using System.Linq;
using System.ComponentModel;

namespace Nemiro.OAuth
{

  /// <summary>
  /// The <see cref="CustomTypeDescriptor"/> for <see cref="UniValue"/>.
  /// </summary>
  internal sealed class UniValueTypeDescriptor : CustomTypeDescriptor
  {

    /// <summary>
    /// The properties collection.
    /// </summary>
    private UniValueCollection Properties { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValueTypeDescriptor"/> class.
    /// </summary>
    public UniValueTypeDescriptor(UniValueCollection properties)
    {
      this.Properties = properties;
    }

    /// <summary>
    /// Returns a collection of property descriptors for the object represented by this type descriptor.
    /// </summary>
    public override PropertyDescriptorCollection GetProperties()
    {
      var orig = base.GetProperties();

      var result = this.Properties.Select
      (
        p => new UniValuePropertyDescriptor
        (
          p.Key,
          itm =>
          {
            return ((UniValue)itm)[p.Key].ToString();
          }
        )
      );

      return new PropertyDescriptorCollection(orig.Cast<PropertyDescriptor>().Concat(result.ToArray()).ToArray());
    }

  }

}
