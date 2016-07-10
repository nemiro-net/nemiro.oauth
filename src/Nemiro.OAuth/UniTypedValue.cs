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

namespace Nemiro.OAuth
{

  /// <summary>
  /// Additional type for references.
  /// </summary>
  [Serializable]
  internal class UniTypedValue : UniValue
  {

    public UniTypedValue(object value, NameValueCollection attributes, UniValue parent)
    {
      base.Key = "value";
      base.Data = value;
      base.Parent = parent;
      base.Attributes = attributes;
    }

    protected UniTypedValue(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
    }

  }

}