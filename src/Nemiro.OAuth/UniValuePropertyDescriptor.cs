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
using System.ComponentModel;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents property description of a class.
  /// </summary>
  internal sealed class UniValuePropertyDescriptor : PropertyDescriptor
  {

    #region ..fields & properties..

    /// <summary>
    /// The data reader.
    /// </summary>
    private Func<object, object> Reader;
    
    /// <summary>
    /// Gets a value indicating whether this property is read-only.
    /// </summary>
    public override bool IsReadOnly
    {
      get 
      { 
        return true; 
      }
    }

    /// <summary>
    /// Gets the type of the property.
    /// </summary>
    public override System.Type PropertyType
    {
      get 
      { 
        return typeof(object); 
      }
    }
    
    /// <summary>
    /// Gets the type of the component this property is bound to.
    /// </summary>
    public override System.Type ComponentType
    {
      get 
      { 
        return typeof(object); 
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValuePropertyDescriptor"/> class.
    /// </summary>
    public UniValuePropertyDescriptor(string name, Func<object, object> reader) : base(name, new Attribute[] {})
    {
      this.Reader = reader;
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Returns whether resetting an object changes its value.
    /// </summary>
    /// <param name="component">The component to test for reset capability.</param>
    public override bool CanResetValue(object component)
    {
      return false;
    }

    /// <summary>
    /// Gets the current value of the property on a component.
    /// </summary>
    /// <param name="component">The component with the property for which to retrieve the value.</param>
    public override object GetValue(object component)
    {
      return this.Reader(component);
    }

    /// <summary>
    /// Resets the value for this property of the component to the default value.
    /// </summary>
    /// <param name="component">The component with the property value that is to be reset to the default value.</param>
    public override void ResetValue(object component)
    {
      throw new NotSupportedException();
    }

    /// <summary>
    /// Sets the value of the component to a different value.
    /// </summary>
    /// <param name="component">The component with the property value that is to be set.</param>
    /// <param name="value">The new value. </param>
    public override void SetValue(object component, object value)
    {
      throw new NotSupportedException();
    }

    /// <summary>
    /// Determines a value indicating whether the value of this property needs to be persisted.
    /// </summary>
    /// <param name="component">The component with the property to be examined for persistence. </param>
    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }

    #endregion

  }

}