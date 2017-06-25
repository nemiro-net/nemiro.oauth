// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2015, 2017. All rights reserved.
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
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// The universal type that represents a value.
  /// </summary>
  /// <remarks>
  /// <para><see cref="UniValue"/> represents any type of data.</para>
  /// </remarks>
  [Serializable]
  [DefaultBindingProperty("CollectionItems")]
  [ComplexBindingProperties("CollectionItems", "Values")]
  public class UniValue : IEnumerable, IEnumerable<UniValue>, IConvertible, ICloneable, ISerializable, IListSource, ICustomTypeDescriptor
  {

    #region ..fields & properties..

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    internal object Data { get; set; }

    /// <summary>
    /// Gets a collection of string keys and <see cref="UniValue"/> values of the current <see cref="UniValue"/>.
    /// </summary>
    /// <remarks>
    /// <para>Has a <b>null</b> value, if the <see cref="IsCollection"/> property is <b>false</b>.</para>
    /// </remarks>
    public UniValueCollection CollectionItems
    {
      get
      {
        if (!this.HasValue) { return null; } // new ResultValueCollection();
        if (!this.IsCollection)
        {
          throw null; // new InvalidCastException(String.Format("Cannot cast the {0} to the ResultValueCollection.", this.Data.GetType().Name));
        }
        return (UniValueCollection)this.Data;
      }
    }

    /// <summary>
    /// Gets or sets an attributes of the XML item (only for XML). 
    /// </summary>
    public NameValueCollection Attributes { get; protected set; }

    /// <summary>
    /// Gets the value associated with the specified key of the <see cref="CollectionItems"/>.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    public UniValue this[string key]
    {
      get
      {
        if (String.IsNullOrEmpty(key))
        {
          throw new ArgumentNullException("key");
        }
        /*if (!this.HasValue || key == null)
        {
          return this.Add(key, UniValue.Empty, this);
        }*/

        if (key.StartsWith("@")) //  && this.GetAttributeValue(key).HasValue
        {
          return this.GetAttributeValue(key);
        }

        if (this.IsValue && ((UniValue)this.Data).ContainsKey(key))
        {
          return ((UniValue)this.Data)[key];
        }
        else if (this.IsCollection && this.ContainsKey(key))
        {
          var result = this.CollectionItems[key]; //((UniValueCollection)this.Data)[key];
          if (result.IsValue)
          {
            //if (this.Unreferenced) // if (result.Parent != null && result.Parent.Unreferenced)//
            //{
            //  return result;
            //}
            //else
            //{
            return (UniValue)result.Data;
            //}
          }
          else
          {
            /*if (result.Parent != null && !result.Parent.Unreferenced)
            {
              result.Parent = new UniValue(result.Parent, result.Parent.Attributes, result.Parent.Parent) { Key = result.Parent.Key, IsArraySubtype = result.Parent.IsArraySubtype, Unreferenced = true };
            }*/
            return result;
            //new UniValue(result, result.Attributes, result.Parent) { Key = result.Key, IsArraySubtype = result.IsArraySubtype, Unreferenced = true };
          }
        }
        else
        {
          if (this.ContainsAttribute(key))
          {
            return this.GetAttributeValue(key);
          }
          else
          {
            return this.Add(key, UniValue.Empty, this);
          }
        }
      }
      set
      {
        if (!this.ContainsKey(key))
        {
          this.Add(key, value);
        }
        else
        {
          this.CollectionItems[key] = value;
        }
      }
    }

    /// <summary>
    /// Gets the value associated with the specified index of the <see cref="CollectionItems"/>.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    public UniValue this[int index]
    {
      get
      {
        /*if (!this.HasValue || index < 0)
        {
          return this.Add(index.ToString(), UniValue.Empty, this);
        }*/

        if (this.IsValue && ((UniValue)this.Data).ContainsKey(index.ToString()))
        {
          return ((UniValue)this.Data)[index];
        }
        else if (this.IsCollection && ((index >= 0 && index <= this.CollectionItems.Keys.Count - 1) || this.ContainsKey(index.ToString())))
        {
          var result = this.CollectionItems[index]; // ((UniValueCollection)this.Data)[index];
          if (result.IsValue)
          {
            // if (this.Unreferenced) // if (result.Parent != null && result.Parent.Unreferenced) //
            //{
            //  return result;
            //}
            //else
            //{
            return (UniValue)result.Data;
            //}
          }
          else
          {
            /*if (result.Parent != null && !result.Parent.Unreferenced)
            {
              result.Parent = new UniValue(result.Parent, result.Parent.Attributes, result.Parent.Parent) { Key = result.Parent.Key, IsArraySubtype = result.Parent.IsArraySubtype, Unreferenced = true };
            }*/
            return result;
            // new UniValue(result, result.Attributes, result.Parent) { Key = result.Key, IsArraySubtype = result.IsArraySubtype, Unreferenced = true };
          }
        }
        else
        {
          return this.Add(index.ToString(), UniValue.Empty, this);
        }
      }
      set
      {
        if (!this.ContainsKey(index.ToString()))
        {
          this.Add(index.ToString(), value);
        }
        else
        {
          this.CollectionItems[index.ToString()] = value;
        }
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to <see cref="UniValue"/>.
    /// </summary>
    public bool IsValue
    {
      get
      {
        if (!this.HasValue) { return false; }
        return this.Data.GetType() == typeof(UniValue); //  || this.Data.GetType().IsSubclassOf(typeof(UniValue))
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to <see cref="UniTypedValue"/>.
    /// </summary>
    internal bool IsTypedValue
    {
      get
      {
        if (!this.HasValue) { return false; }
        return this.Data.GetType() == typeof(UniTypedValue);
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to <see cref="UniValueCollection"/>.
    /// </summary>
    public bool IsCollection
    {
      get
      {
        if (!this.HasValue) { return false; }
        return this.Data.GetType() == typeof(UniValueCollection);
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to <see cref="System.Byte"/> array.
    /// </summary>
    public bool IsBinary
    {
      get
      {
        if (!this.HasValue) { return false; }
        if (this.IsTypedValue)
        {
          return ((UniTypedValue)this.Data).IsBinary;
        }
        else
        {
          return this.Data.GetType() == typeof(byte[]);
        }
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to <see cref="System.String"/>.
    /// </summary>
    public bool IsString
    {
      get
      {
        if (!this.HasValue) { return false; }
        if (this.IsTypedValue)
        {
          return ((UniTypedValue)this.Data).IsString;
        }
        else
        {
          return this.Data.GetType() == typeof(string) || this.Data.GetType() == typeof(char);
        }
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to <see cref="System.DateTime"/>.
    /// </summary>
    public bool IsDateTime
    {
      get
      {
        if (!this.HasValue) { return false; }
        if (this.IsTypedValue)
        {
          return ((UniTypedValue)this.Data).IsDateTime;
        }
        else
        {
          return this.Data.GetType() == typeof(DateTime);
        }
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to <see cref="System.Boolean"/>.
    /// </summary>
    public bool IsBoolean
    {
      get
      {
        if (!this.HasValue) { return false; }
        if (this.IsTypedValue)
        {
          return ((UniTypedValue)this.Data).IsBoolean;
        }
        else
        {
          return (this.Data.GetType() == typeof(bool));
        }
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the data type of the <see cref="UniValue"/> is equal to numeric type.
    /// </summary>
    public bool IsNumeric
    {
      get
      {
        if (!this.HasValue) { return false; }
        if (this.IsTypedValue)
        {
          return ((UniTypedValue)this.Data).IsNumeric;
        }
        else
        {
          return Array.IndexOf(OAuthUtility.NumericType, this.Data.GetType()) != -1;
        }
      }
    }

    /// <summary>
    /// Gets a value indicating whether the current <see cref="UniValue"/> object has a value.
    /// </summary>
    public bool HasValue
    {
      get
      {
        return this.Data != null;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the current <see cref="UniValue"/> object has an attributes (only for xml data type).
    /// </summary>
    public bool HasAttributes
    {
      get
      {
        return this.Attributes != null && this.Attributes.Count > 0;
      }
    }

    /// <summary>
    /// Gets the number of elements actually contained in the <see cref="CollectionItems"/>.
    /// </summary>
    public int Count
    {
      get
      {
        if (this.IsCollection)
        {
          return ((UniValueCollection)this.Data).Count;
        }
        /*else if (this.IsString)
        {
          return this.Value.ToString().Length;
        }*/
        return 0;
      }
    }

    /// <summary>
    /// Gets or sets the key for the current item, if the current item included into the collection.
    /// </summary>
    /// <remarks>
    /// <para>Assigned automatically when parsing data of <b>JSON</b>, <b>XML</b> or <b>query string</b>.</para>
    /// <para><b>root</b> for root elements.</para>
    /// <para><b>value</b> for <see cref="UniTypedValue"/>.</para>
    /// <para><b>____</b> for new collections created by the developer manually.</para>
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string Key { get; internal set; }

    /// <summary>
    /// The parent of the current item, if the current item included into the collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public UniValue Parent { get; internal set; }

#if DEBUG

    /// <summary>
    /// The <see cref="Key"/> of the <see cref="Parent"/>.
    /// </summary>
    /// <remarks><para>This property for debugging.</para></remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal string ParentKey
    {
      get
      {
        if (this.Parent == null)
        {
          return null;
        }
        else
        {
          return this.Parent.Key;
        }
      }
    }

    /// <summary>
    /// Retrieves the hash code of the object.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal int Hash
    {
      get
      {
        return this.GetHashCode();
      }
    }

#endif

    /// <summary>
    /// Gets or sets a value that indicates whether the data type of the <see cref="UniValue"/> is array.
    /// </summary>
    /// <remarks>
    /// <para>This affects the representation of the object as a string. For arrays in a <b>JSON</b> is not use the <see cref="Key"/>.</para>
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal bool IsArraySubtype { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the type is unreferenced.
    /// </summary>
    [Obsolete("Technical properties. Appropriateness of the use is under consideration. // v1.5", false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal bool Unreferenced { get; set; }

    /// <summary>
    /// Represents the empty <see cref="UniValue"/>.
    /// </summary>
    public static UniValue Empty
    {
      get
      {
        return new UniValue(null, null, null);
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValue"/>.
    /// </summary>
    internal UniValue() : this(null, null, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValue"/> with a specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    internal UniValue(object value) : this(value, null, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValue"/> with a specified value and attributes.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="attributes">The collection of an attributes.</param>
    internal UniValue(object value, NameValueCollection attributes) : this(value, attributes, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValue"/> with a specified value and reference to parent.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="parent">The instance of the <see cref="UniValue"/>.</param>
    internal UniValue(object value, UniValue parent) : this(value, null, parent) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValue"/> with a specified value, attributes and reference to parent.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="attributes">The collection of an attributes.</param>
    /// <param name="parent">The instance of the <see cref="UniValue"/>.</param>
    internal UniValue(object value, NameValueCollection attributes, UniValue parent)
    {
      this.Key = "root";
      this.Parent = parent; //(parent == null ? UniValue.Empty : parent);

      if (value == null || value == DBNull.Value)
      {
        this.Data = this.Attributes = null;
        return;
      }

      //if (parent != null && value.GetType() == typeof(UniTypedValue)) { this.IsDynamic = parent.IsDynamic; }
      //if (parent != null) { this.Unreferenced = parent.Unreferenced; }

      // attributes (only for xml)
      this.Attributes = attributes;
      // --

      // value
      if (value.GetType().IsArray && value.GetType() != typeof(byte[]) && value.GetType() != typeof(XElement[]))
      {
        #region array

        var result = new UniValueCollection(this);
        var data = value as Array;

        for (int i = 0; i <= data.Length - 1; i++)
        {
          result.Add(i.ToString(), UniValue.Create(data.GetValue(i), this.GetParent()));
        }

        this.Data = result;
        this.IsArraySubtype = true;

        #endregion
      }
      else if (value.GetType() == typeof(Dictionary<string, object>))
      {
        #region dictionary

        var result = new UniValueCollection(this);
        var data = value as Dictionary<string, object>;

        foreach (var itm in data)
        {
          var v = UniValue.Create(itm.Value, this.GetParent());
          if (v.IsValue)
          {
            result.Add(itm.Key, (UniValue)v.Data);
          }
          else if (v.IsCollection)
          {
            result.Add(itm.Key, v);
          }
          else
          {
            result.Add(itm.Key, v);
          }
        }

        this.Data = result;

        #endregion
      }
      else if (value.GetType() == typeof(NameValueCollection))
      {
        #region nameValueCollection

        var result = new UniValueCollection(this);
        var data = value as NameValueCollection;

        foreach (var key in data.Keys)
        {
          if (data[key.ToString()].IndexOf(",") != -1)
          {
            result.Add(key.ToString(), UniValue.Create(data[key.ToString()].Split(','), this.GetParent()));
          }
          else
          {
            result.Add(key.ToString(), UniValue.Create(data[key.ToString()], this.GetParent()));
          }
        }

        this.Data = result;

        #endregion
      }
      else if (value.GetType() == typeof(XDocument))
      {
        #region xdocument

        this.Data = null;
        var result = new UniValueCollection(this);
        var data = ((XDocument)value).Elements();

        foreach (var itm in data)
        {
          this.ParseXElement(result, itm, this.GetParent());
        }

        this.Data = result;

        #endregion
      }
      else if (value.GetType() == typeof(XElement[]))
      {
        #region xelement[]

        this.Data = null;
        var result = new UniValueCollection(this);
        var data = (XElement[])value;

        foreach (var itm in data)
        {
          this.ParseXElement(result, itm, this.GetParent());
        }

        this.Data = result;

        #endregion
      }
      else if (value.GetType() == typeof(XElement))
      {
        #region xelement

        this.Data = null;
        var result = new UniValueCollection(this);
        var itm = (XElement)value;

        this.ParseXElement(result, itm, this.GetParent());

        this.Data = result;

        #endregion
      }
      else if (value.GetType() == typeof(StringBuilder))
      {
        #region stringBuilder

        this.Data = value.ToString();

        #endregion
      }
      else if (value.GetType() == typeof(UniValueCollection))
      {
        #region uniValueCollection

        this.Data = value;

        #endregion
      }
      else if (value.GetType() == typeof(UniValue) || value.GetType().IsSubclassOf(typeof(UniValue)))
      {
        #region uniValue

        this.Parent = ((UniValue)value).Parent;
        this.Data = ((UniValue)value).Data;
        this.Attributes = ((UniValue)value).Attributes;

        #endregion
      }
      else
      {
        #region other

        if (value.GetType().IsClass && Array.IndexOf(OAuthUtility.ExcludedTypeOfClasses, value.GetType()) == -1)
        {
          // is class
          var result = new UniValueCollection(this);

          foreach (var itm in value.GetType().GetProperties())
          {
            result.Add(itm.Name, UniValue.Create((itm.CanRead ? itm.GetValue(value, null) : null), this.GetParent()));
          }

          this.Data = result;
        }
        else
        {
          // other type
          this.Data = new UniTypedValue(value, attributes, this.GetParent());
          //this.Data = new UniTypedValue(value, attributes, ((object)parent != null && (object)parent.Parent != null ? ((object)(parent.Parent) ?? ((object)parent ?? this)) : ((object)parent ?? this)));
          // (parent != null && parent.Parent != null ? (parent.Parent ?? (parent ?? this)) : (parent ?? this))
          // parent ?? this
        }

        #endregion
      }
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Copies items of the <see cref="UniValue"/> to a new <see cref="Dictionary&lt;TKey, TValue&gt;">Dictionary&lt;string, object&gt;</see>.
    /// </summary>
    /// <returns>
    /// <para>A <see cref="Dictionary&lt;TKey, TValue&gt;">Dictionary&lt;string, object&gt;</see> containing copies of the elements of the <see cref="UniValue"/>.</para>
    /// <para>A <b>null</b> value, if the <see cref="IsCollection"/> property is <b>false</b> or the <see cref="UniValue"/> is empty.</para>
    /// </returns>
    public Dictionary<string, object> ToDictionary()
    {
      if (!this.HasValue) { return null; }
      if (this.IsCollection)
      {
        var result = new Dictionary<string, object>();
        foreach (KeyValuePair<string, UniValue> itm in this.CollectionItems)
        {
          result.Add(itm.Key, itm.Value);
        }
        return result;
      }
      else
      {
        return null;
        //throw new InvalidCastException(String.Format("Cannot cast the {0} to the Dictionary<string, ResultValue>.", this.Data.GetType().Name));
      }
    }

    /// <summary>
    /// Copies items of the <see cref="UniValue"/> to a new <see cref="NameValueCollection" />.
    /// </summary>
    /// <returns>
    /// <para>A <see cref="NameValueCollection" /> containing copies of the elements of the <see cref="UniValue"/>.</para>
    /// <para>A <b>null</b> value, if the <see cref="IsCollection"/> property is <b>false</b> or the <see cref="UniValue"/> is empty.</para>
    /// </returns>
    public NameValueCollection ToNameValueCollection()
    {
      if (!this.HasValue) { return null; }
      if (this.IsCollection)
      {
        var result = new NameValueCollection();
        foreach (KeyValuePair<string, UniValue> itm in this.CollectionItems)
        {
          foreach (var value in itm.Value) //.ToArray()
          {
            result.Add(itm.Key, value.ToString());
          }
        }
        return result;
      }
      else
      {
        return null;
        //throw new InvalidCastException(String.Format("Cannot cast the {0} to the NameValueCollection.", this.Data.GetType().Name));
      }
    }

    /// <summary>
    /// Returns a <see cref="System.Byte"/> array that represents the current <see cref="UniValue"/>.
    /// </summary>
    /// <returns>
    /// <para>A <see cref="System.Byte"/> array containing the current <see cref="UniValue"/>.</para>
    /// <para>A <b>null</b> value, if the property <see cref="IsBinary"/> and <see cref="IsString"/> is <b>false</b> or the <see cref="UniValue"/> is empty.</para>
    /// </returns>
    public byte[] ToBinary()
    {
      if (!this.HasValue) { return null; }
      if (this.IsBinary)
      {
        if (this.IsTypedValue)
        {
          return (byte[])((UniTypedValue)this.Data).Data;
        }
        else
        {
          return (byte[])this.Data;
        }
      }
      else if (this.IsString)
      {
        return Encoding.UTF8.GetBytes(this.Data.ToString());
      }
      else
      {
        return null;
        //throw new InvalidCastException(String.Format("Cannot cast the {0} to the byte array.", this.Data.GetType().Name));
      }
    }

    /// <summary>
    /// Determines whether the <see cref="CollectionItems"/> contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the <see cref="CollectionItems"/>.</param>
    /// <returns><b>true</b> if the <see cref="CollectionItems"/> is not <b>null</b> and contains an element with the specified <paramref name="key"/>; otherwise, <b>false</b>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is <b>null</b>.</exception>
    public bool ContainsKey(string key)
    {
      if (String.IsNullOrEmpty(key))
      {
        throw new ArgumentNullException("key");
      }
      if (this.IsCollection)
      {
        return this.CollectionItems.ContainsKey(key);
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Determines whether the <see cref="Attributes"/> contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the <see cref="Attributes"/>.</param>
    /// <returns><b>true</b> if the <see cref="Attributes"/> is not <b>null</b> and contains an element with the specified <paramref name="key"/>; otherwise, <b>false</b>.</returns>
    public bool ContainsAttribute(string key)
    {
      if (!this.HasAttributes) { return false; }
      if (this.Attributes[key] == null)
      {
        return this.Attributes.AllKeys.Contains(key);
      }
      return true;
    }

    /// <summary>
    /// Adds the specified key and value to the <see cref="UniValue"/>.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <returns>
    /// <para>Returns the added element.</para>
    /// </returns>
    /// <remarks>
    /// <para>
    /// If the current <see cref="UniValue"/> is not a collection (<see cref="IsCollection"/> is <b>false</b>), it will automatically be converted to the collection.
    /// If the current <see cref="UniValue"/> is not empty, then it will be assigned a key <c>____</c> in a new collection.
    /// </para>
    /// </remarks>
    public UniValue Add(string key, UniValue value)
    {
      return this.Add(key, value, this.Parent);
    }

    /// <summary>
    /// Adds the specified key and value to the <see cref="UniValue"/>.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <param name="parent">The reference to parent of the elemet to add.</param>
    /// <returns>
    /// <para>Returns the added element.</para>
    /// </returns>
    /// <remarks>
    /// <para>
    /// If the current <see cref="UniValue"/> is not a collection (<see cref="IsCollection"/> is <b>false</b>), it will automatically be converted to the collection.
    /// If the current <see cref="UniValue"/> is not empty, then it will be assigned a key <c>____</c> in a new collection.
    /// </para>
    /// </remarks>
    public UniValue Add(string key, UniValue value, UniValue parent)
    {
      if (!this.IsCollection)
      {
        var c = new UniValueCollection(parent);
        if (this.HasValue && !this.Equals(value))
        {
          c.Add("", UniValue.Create(this));
        }
        this.Data = c;
      }
      this.CollectionItems.Add(key, value);
      return this.CollectionItems.Last().Value;
    }

    /// <summary>
    /// Removes the value with the specified key from the <see cref="UniValue"/>.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>
    /// <b>true</b> if the element is successfully found and removed; otherwise, <b>false</b>. 
    /// This method returns <b>false</b> if key is not found in the <see cref="UniValue"/> or is not collection (<see cref="IsCollection"/> is <b>false</b>).
    /// </returns>
    public bool Remove(string key)
    {
      if (!this.IsCollection) { return false; }
      return this.CollectionItems.Remove(key);
    }

    /// <summary>
    /// Parses the <see cref="XElement"/> and converts to <see cref="UniValueCollection"/>.
    /// </summary>
    /// <param name="result">The reference to the collection, which will be placed the result of parsing <paramref name="itm"/>.</param>
    /// <param name="itm">The <see cref="XElement"/> for parsing.</param>
    /// <param name="parent">The reference to parent.</param>
    private void ParseXElement(UniValueCollection result, XElement itm, UniValue parent)
    {
      NameValueCollection attr = null;
      if (itm.HasAttributes)
      {
        attr = new NameValueCollection();
        foreach (var a in itm.Attributes())
        {
          attr.Add(a.Name.LocalName, a.Value);
        }
      }
      if (itm.HasElements)
      {
        result.Add(itm.Name.LocalName, new UniValue(itm.Elements().ToArray(), attr, parent));
      }
      else
      {
        result.Add(itm.Name.LocalName, new UniValue(itm.Value, attr, this));
      }
    }

    /// <summary>
    /// Returns the <see cref="UniValue"/> of the specified attribute.
    /// </summary>
    /// <param name="attributeName">The name of the attribute whose value you want to get.</param>
    private UniValue GetAttributeValue(string attributeName)
    {
      if (this.Attributes == null) { return UniValue.Empty; }
      if (attributeName.StartsWith("@"))
      {
        attributeName = attributeName.Substring(1, attributeName.Length - 1);
      }
      return new UniValue(this.Attributes[attributeName], this);
    }

    /// <summary>
    /// Returns parent for new instance of <see cref="UniValue"/>.
    /// </summary>
    private UniValue GetParent()
    {
      if ((object)this.Parent != null)
      {
        return this.Parent;
      }
      else
      {
        return this;
      }
    }

    #endregion
    #region ..static methods..

    /// <summary>
    /// Initializes a new <see cref="UniValue"/> instance.
    /// </summary>
    public static UniValue Create()
    {
      return UniValue.Create(null, null, null);
    }

    /// <summary>
    /// Initializes a new <see cref="UniValue"/> instance with a specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    public static UniValue Create(object value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Initializes a new <see cref="UniValue"/> instance with a specified <paramref name="value" /> and <paramref name="attributes" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="attributes">The collection of an attributes.</param>
    public static UniValue Create(object value, NameValueCollection attributes)
    {
      return new UniValue(value, attributes, null);
    }

    /// <summary>
    /// Initializes a new <see cref="UniValue"/> instance with a specified <paramref name="value" /> and reference to <paramref name="parent" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="parent">The instance of the <see cref="UniValue"/>.</param>
    internal static UniValue Create(object value, UniValue parent)
    {
      return new UniValue(value, null, parent);
    }

    /// <summary>
    /// Initializes a new <see cref="UniValue"/> instance with a specified <paramref name="value" />, <paramref name="attributes" /> and reference to <paramref name="parent" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="parent">The instance of the <see cref="UniValue"/>.</param>
    /// <param name="attributes">The collection of an attributes.</param>
    internal static UniValue Create(object value, NameValueCollection attributes, UniValue parent)
    {
      return new UniValue(value, attributes, parent);
    }

    /// <summary>
    /// Initializes an empty <see cref="UniValue"/> instance with a specified <paramref name="attributes" /> and reference to <paramref name="parent" />.
    /// </summary>
    /// <param name="attributes">The collection of an attributes.</param>
    /// <param name="parent">The instance of the <see cref="UniValue"/>.</param>
    internal static UniValue CreateEmpty(NameValueCollection attributes, UniValue parent)
    {
      return new UniValue(null, attributes, parent);
    }

    /// <summary>
    /// Converts the specified <b>JSON</b> string to an <see cref="UniValue"/>.
    /// </summary>
    /// <param name="text">A string containing a <b>JSON</b> data to parse.</param>
    /// <returns>A new <see cref="UniValue"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <b>null</b>.</exception>
    /// <exception cref="ArgumentException">The <paramref name="text"/> length exceeds the value of <see cref="Int32.MaxValue"/>.</exception>
    /// <exception cref="ArgumentException">The recursion limit defined by <see cref="Int32.MaxValue"/> was exceeded.</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> contains an unexpected character sequence.</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> is a dictionary type and a non-string key value was encountered.</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> includes member definitions that are not available on the target type.</exception>
    /// <exception cref="InvalidOperationException">It is not possible to convert <paramref name="text"/> to the target type.</exception>
    public static UniValue ParseJson(string text)
    {
      return UniValue.Create
      (
        new System.Web.Script.Serialization.JavaScriptSerializer()
        {
          MaxJsonLength = Int32.MaxValue,
          RecursionLimit = Int32.MaxValue
        }.DeserializeObject(text), null, null
      );
    }

    /// <summary>
    /// Converts the specified <b>XML</b> string to an <see cref="UniValue"/>.
    /// </summary>
    /// <param name="text">A string containing a <b>XML</b> data to parse.</param>
    /// <returns>A new <see cref="UniValue"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <b>null</b>.</exception>
    public static UniValue ParseXml(string text)
    {
      return UniValue.Create(XDocument.Parse(text), null);
    }

    /// <summary>
    /// Converts the specified parameters string to an <see cref="UniValue"/>.
    /// </summary>
    /// <param name="text">A string containing an url parameters to parse.</param>
    /// <returns>A new <see cref="UniValue"/> instance.</returns>
    /// <exception cref="InvalidDataException"><paramref name="text"/> contains an <b>CR</b> or <b>LF</b> characters.</exception>
    /// <remarks>
    /// <para>If <paramref name="text"/> is <b>null</b> or empty, the function returns an <see cref="UniValue.Empty"/> instance.</para>
    /// </remarks>
    public static UniValue ParseParameters(string text)
    {
      if (String.IsNullOrEmpty(text)) { return UniValue.Empty; }

      if (text.IndexOf("\r") != -1 || text.IndexOf("\n") != -1)
      {
        throw new InvalidDataException("CR and LF are not allowed in the Parameters string.");
      }

      if (Regex.IsMatch(text, @"([^\x3D]+)=([^\x26]*)", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase))
      {
        var items = new NameValueCollection();

        foreach (string q in text.Split('&'))
        {
          string[] p = q.Split('=');
          string key = p.First(), value = (p.Length > 1 ? p.Last() : "");

          items.Add(key, System.Web.HttpUtility.UrlDecode(value));
        }

        return UniValue.Create(items, null);
      }
      else
      {
        return UniValue.Create(text, null);
      }
    }

    /// <summary>
    /// Converts the specified <b>JSON</b> string to an <see cref="UniValue"/>. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="text">A string containing a <b>JSON</b> data to parse.</param>
    /// <param name="result">If successful, this parameter takes the result of parsing data.</param>
    /// <returns><b>true</b> if s was converted successfully; otherwise, <b>false</b>.</returns>
    public static bool TryParseJson(string text, out UniValue result)
    {
      result = UniValue.Empty;
      try
      {
        result = UniValue.ParseJson(text);
        return true;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Converts the specified <b>XML</b> string to an <see cref="UniValue"/>. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="text">A string containing a <b>XML</b> data to parse.</param>
    /// <param name="result">If successful, this parameter takes the result of parsing data.</param>
    /// <returns><b>true</b> if s was converted successfully; otherwise, <b>false</b>.</returns>
    public static bool TryParseXml(string text, out UniValue result)
    {
      result = UniValue.Empty;
      try
      {
        result = UniValue.ParseXml(text);
        return true;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Converts the specified url parameters string to an <see cref="UniValue"/>. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="text">A string containing an url parameters to parse.</param>
    /// <param name="result">If successful, this parameter takes the result of parsing data.</param>
    /// <returns><b>true</b> if s was converted successfully; otherwise, <b>false</b>.</returns>
    public static bool TryParseParameters(string text, out UniValue result)
    {
      result = UniValue.Empty;
      try
      {
        result = UniValue.ParseParameters(text);
        return true;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Indicates whether the specified value is null or an <see cref="UniValue.Empty"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance to test.</param>
    /// <returns><b>true</b> if the <paramref name="value"/> parameter is <b>null</b> or <see cref="UniValue.HasValue"/> is <b>false</b>; otherwise, <b>false</b>.</returns>
    public static bool IsNullOrEmpty(UniValue value)
    {
      try
      {
        return !value.HasValue;
      }
      catch
      {
        return true;
      }
    }

    #endregion
    #region ..iconvertible..

    /// <summary>
    /// Gets the underlying type code of the <see cref="Data"/>.
    /// </summary>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public TypeCode GetTypeCode()
    {
      if (!this.HasValue) { return TypeCode.Empty; }
      return Type.GetTypeCode(this.Data.GetType());
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent <see cref="System.Boolean"/> value using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information. </param>
    /// <returns>A <see cref="System.Boolean"/> value equivalent to the value of this instance.</returns>
    /// <exception cref="FormatException" />
    /// <exception cref="InvalidCastException" />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ToBoolean(IFormatProvider provider)
    {
      if (!this.HasValue) { return false; }
      return Convert.ToBoolean(this.Data);
    }

    /*bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      if (!this.HasValue) { return false; }
      return Convert.ToBoolean(this.Data);
    }*/

    /// <summary>
    /// Converts the value this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 8-bit unsigned integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public byte ToByte(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToByte(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>A Unicode character equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public char ToChar(IFormatProvider provider)
    {
      if (!this.HasValue) { return new char(); }
      return Convert.ToChar(this.Data);
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent <see cref="System.DateTime"/> using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>A <see cref="System.DateTime"/> instance equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime ToDateTime(IFormatProvider provider)
    {
      if (!this.HasValue) { return new DateTime(1, 1, 1); }
      return Convert.ToDateTime(this.Data);
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent <see cref="System.Decimal"/> number using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>A <see cref="System.Decimal"/> number equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public decimal ToDecimal(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToDecimal(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>A double-precision floating-point number equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public double ToDouble(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToDouble(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 16-bit signed integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public short ToInt16(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToInt16(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 32-bit signed integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int ToInt32(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToInt32(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 64-bit signed integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public long ToInt64(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToInt64(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 8-bit signed integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sbyte ToSByte(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToSByte(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>A single-precision floating-point number equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public float ToSingle(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToSingle(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent <see cref="System.String"/> using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>A <see cref="System.String"/> instance equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string ToString(IFormatProvider provider)
    {
      if (!this.HasValue) { return null; }
      return Convert.ToString(this.Data);
    }

    /// <summary>
    /// Converts the value of this instance to an <see cref="System.Object"/> of the specified <see cref="System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="conversionType">The <see cref="System.Type"/> to which the value of this instance is converted. </param>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An <see cref="System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object ToType(Type conversionType, IFormatProvider provider)
    {
      if (!this.HasValue) { return null; }
      return Convert.ChangeType(this.Data, conversionType);
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 16-bit unsigned integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ushort ToUInt16(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToUInt16(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 32-bit unsigned integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public uint ToUInt32(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToUInt32(OAuthUtility.GetNumber(this.Data));
    }

    /// <summary>
    /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">An <see cref="System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
    /// <returns>An 64-bit unsigned integer equivalent to the value of this instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ulong ToUInt64(IFormatProvider provider)
    {
      if (!this.HasValue) { return 0; }
      return Convert.ToUInt64(OAuthUtility.GetNumber(this.Data));
    }

    #endregion
    #region ..icloneable..

    /// <summary>
    /// Creates a new object that is a copy of the current <see cref="UniValue"/> instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public object Clone()
    {
      NameValueCollection attributes = null;
      if (this.HasAttributes)
      {
        attributes = new NameValueCollection();
        attributes.Add(this.Attributes);
      }

      if (this.HasValue)
      {
        if (this.Data.GetType().IsAssignableFrom(typeof(ICloneable)))
        {
          return new UniValue(((ICloneable)this.Data).Clone(), attributes, this.Parent);
        }
      }

      return new UniValue(this.Data, attributes, this.Parent);
    }

    #endregion
    #region ..ienumerator..

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator()
    {
      if (!this.HasValue)
      {
        return new List<UniValue>().GetEnumerator();
      }

      if (this.IsValue)
      {
        return new List<UniValue>() { (UniValue)this.Data }.GetEnumerator();
      }
      else if (this.IsCollection)
      {
        return this.CollectionItems.Values.ToArray().GetEnumerator();
      }
      else
      {
        return new ArrayList() { this.Data }.GetEnumerator();
      }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="System.Collections.IEnumerator">System.Collections.IEnumerator&lt;UniValue&gt;</see> object that can be used to iterate through the collection.</returns>
    IEnumerator<UniValue> IEnumerable<UniValue>.GetEnumerator()
    {
      if (!this.HasValue)
      {
        return new List<UniValue>().GetEnumerator();
      }

      var result = new List<UniValue>();

      if (this.IsValue)
      {
        result.Add((UniValue)this.Data);
      }
      else if (this.IsCollection)
      {
        result.AddRange(this.CollectionItems.Values.ToArray());
      }
      else
      {
        result.Add(new UniValue(this.Data));
      }

      return result.GetEnumerator();
    }

    #endregion
    #region ..iserializable..

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValue"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected UniValue(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
      this.Data = info.GetValue("Data", typeof(object));
      this.Attributes = (NameValueCollection)info.GetValue("Attributes", typeof(NameValueCollection));
      this.Key = (string)info.GetValue("Key", typeof(string));
      this.Parent = (UniValue)info.GetValue("Parent", typeof(UniValue));
      this.IsArraySubtype = (bool)info.GetValue("IsArraySubtype", typeof(bool));
      //this.Unreferenced = (bool)info.GetValue("Unreferenced", typeof(bool));
    }

    //[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
    /// <summary>
    /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    //[System.Security.SecurityCritical]
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
      info.AddValue("Data", this.Data);
      info.AddValue("Attributes", this.Attributes);
      info.AddValue("Key", this.Key);
      info.AddValue("Parent", this.Parent);
      info.AddValue("IsArraySubtype", this.IsArraySubtype);
      //info.AddValue("Unreferenced", this.Unreferenced);
    }

    #endregion
    #region ..overrides..

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>A hash code for the current <see cref="UniValue"/>.</returns>
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="UniValue"/>.
    /// </summary>
    public override string ToString()
    {
      if (!this.HasValue)
      {
        return null;
      }
      else
      {
        //if (OAuthUtility.IsNumeric(this.Data))
        //{
        //  return this.Data.ToString().Replace(",", ".");
        //}
        //else
        //{
        return this.Data.ToString();
        //}
      }
    }

    /// <summary>
    /// Determines whether two object instances are equal.
    /// </summary>
    /// <param name="o">The object to compare with the current instance of the <see cref="UniValue"/>.</param>
    /// <returns><b>true</b> if the specified object is equal to the current <see cref="UniValue"/>; otherwise, <b>false</b>.</returns>
    public override bool Equals(object o)
    {
      return this.Equals(UniValue.Create(o));
    }

    /// <summary>
    /// Determines whether two <see cref="UniValue"/> instances are equal.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> to compare with the current instance of the <see cref="UniValue"/>.</param>
    /// <returns><b>true</b> if the specified <see cref="UniValue"/> is equal to the current <see cref="UniValue"/>; otherwise, <b>false</b>.</returns>
    public bool Equals(UniValue value)
    {
      if (UniValue.IsNullOrEmpty(this) != UniValue.IsNullOrEmpty(value)) { return false; }
      if (UniValue.IsNullOrEmpty(this) && UniValue.IsNullOrEmpty(value)) { return true; }
      if (this.Data.GetType() == typeof(UniTypedValue))
      {
        return ((UniValue)this.Data).Equals(value);
      }
      else if (this.Data.GetType() == typeof(UniValueCollection))
      {
        return this.CollectionItems.Equals(value);
      }
      else if (this.Data.GetType() == typeof(Boolean))
      {
        return Convert.ToBoolean(this.Data) == Convert.ToBoolean(value);
      }
      else if (this.Data.GetType() == typeof(Byte))
      {
        return Convert.ToByte(this.Data) == Convert.ToByte(value);
      }
      else if (this.Data.GetType() == typeof(Char))
      {
        return Convert.ToChar(this.Data) == Convert.ToChar(value);
      }
      else if (this.Data.GetType() == typeof(DateTime))
      {
        return Convert.ToDateTime(this.Data) == Convert.ToDateTime(value);
      }
      else if (this.Data.GetType() == typeof(Decimal))
      {
        return Convert.ToDecimal(this.Data) == Convert.ToDecimal(value);
      }
      else if (this.Data.GetType() == typeof(Double))
      {
        return Convert.ToDouble(this.Data) == Convert.ToDouble(value);
      }
      else if (this.Data.GetType() == typeof(Int16))
      {
        return Convert.ToInt16(this.Data) == Convert.ToInt16(value);
      }
      else if (this.Data.GetType() == typeof(Int32))
      {
        return Convert.ToInt32(this.Data) == Convert.ToInt32(value);
      }
      else if (this.Data.GetType() == typeof(Int64))
      {
        return Convert.ToInt64(this.Data) == Convert.ToInt64(value);
      }
      else if (this.Data.GetType() == typeof(SByte))
      {
        return Convert.ToSByte(this.Data) == Convert.ToSByte(value);
      }
      else if (this.Data.GetType() == typeof(Single))
      {
        return Convert.ToSingle(this.Data) == Convert.ToSingle(value);
      }
      else if (this.Data.GetType() == typeof(String))
      {
        return Convert.ToString(this.Data) == Convert.ToString(value);
      }
      else if (this.Data.GetType() == typeof(UInt16))
      {
        return Convert.ToUInt16(this.Data) == Convert.ToUInt16(value);
      }
      else if (this.Data.GetType() == typeof(UInt32))
      {
        return Convert.ToUInt32(this.Data) == Convert.ToUInt32(value);
      }
      else if (this.Data.GetType() == typeof(UInt64))
      {
        return Convert.ToUInt64(this.Data) == Convert.ToUInt64(value);
      }
      else
      {
        return this.Data.Equals(value.Data);
      }
    }

    /// <summary>
    /// Determines whether this instance and another specified <see cref="System.String"/> object have the same value.
    /// </summary>
    /// <param name="value">The string to compare to this instance of the <see cref="UniValue"/>.</param>
    /// <returns><b>true</b> if the value of the <paramref name="value"/> parameter is the same as this instance; otherwise, <b>false</b>.</returns>
    public bool Equals(string value)
    {
      return this.Equals(value, StringComparison.CurrentCulture);
    }

    /// <summary>
    /// Determines whether this string and a specified <see cref="System.String"/> object have the same value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="value">The string to compare to this instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared. </param>
    /// <returns><b>true</b> if the value of the <paramref name="value"/> parameter is the same as this instance; otherwise, <b>false</b>.</returns>
    public bool Equals(string value, StringComparison comparisonType)
    {
      if (this.HasValue == String.IsNullOrEmpty(value)) { return false; }
      if (!this.HasValue && String.IsNullOrEmpty(value)) { return true; }
      return this.Data.ToString().Equals(value, comparisonType);
    }

    #endregion
    #region ..operators explicit..

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="NameValueCollection"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator NameValueCollection(UniValue value)
    {
      return value.ToNameValueCollection();
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="Dictionary&lt;TKey, TValue&gt;">Dictionary&lt;string, object&gt;</see>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator Dictionary<string, object>(UniValue value)
    {
      return value.ToDictionary();
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an <see cref="UniValue"/> array.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator UniValue[](UniValue value)
    {
      return value.ToArray();
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an array.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator Array(UniValue value)
    {
      return value.ToArray();
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.Byte"/> array.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator byte[](UniValue value)
    {
      return value.ToBinary();
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.IO.Stream"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator Stream(UniValue value)
    {
      return new MemoryStream(value.ToBinary());
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.Byte"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator byte(UniValue value)
    {
      return value.ToByte(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.SByte"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator sbyte(UniValue value)
    {
      return value.ToSByte(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.Char"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator char(UniValue value)
    {
      return value.ToChar(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.Boolean"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator bool(UniValue value)
    {
      return value.ToBoolean(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.DateTime"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator DateTime(UniValue value)
    {
      return value.ToDateTime(DateTimeFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an <see cref="System.Int16"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator Int16(UniValue value)
    {
      return value.ToInt16(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an <see cref="System.Int32"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator Int32(UniValue value)
    {
      return value.ToInt32(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an <see cref="System.Int64"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator Int64(UniValue value)
    {
      return value.ToInt64(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an <see cref="System.UInt16"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator UInt16(UniValue value)
    {
      return value.ToUInt16(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an <see cref="System.UInt32"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator UInt32(UniValue value)
    {
      return value.ToUInt32(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as an <see cref="System.UInt64"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator UInt64(UniValue value)
    {
      return value.ToUInt64(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.Double"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator double(UniValue value)
    {
      return value.ToDouble(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.Decimal"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator decimal(UniValue value)
    {
      return value.ToDecimal(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.Single"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator float(UniValue value)
    {
      return value.ToSingle(NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Converts the <see cref="UniValue"/> as a <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The <see cref="UniValue"/> instance.</param>
    public static explicit operator string(UniValue value)
    {
      return value.ToString();
    }

    #endregion
    #region ..operators implicit..

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="NameValueCollection"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(NameValueCollection value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="Dictionary&lt;TKey, TValue&gt;">Dictionary&lt;string, object&gt;</see>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(Dictionary<string, object> value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from array.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(Array value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Byte"/> array.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(byte[] value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.String"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(string value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Int16"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(Int16 value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Int32"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(Int32 value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Int64"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(Int64 value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.UInt16"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(UInt16 value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.UInt32"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(UInt32 value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.UInt64"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(UInt64 value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Boolean"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(bool value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.DateTime"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(DateTime value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Byte"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(byte value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.SByte"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(sbyte value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Char"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(char value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Decimal"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(decimal value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Double"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(double value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Single"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(float value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="System.Text.StringBuilder"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(StringBuilder value)
    {
      return UniValue.Create(value, null, null);
    }

    /// <summary>
    /// Creates a new <see cref="UniValue"/> instance from <see cref="UniValueCollection"/>.
    /// </summary>
    /// <param name="value">The value from which will be created a new instance of the <see cref="UniValue"/>.</param>
    public static implicit operator UniValue(UniValueCollection value)
    {
      return UniValue.Create(value, null, null);
    }

    #endregion
    #region ..conditional operators..

    /// <summary>
    /// Indicate whether two <see cref="UniValue"/> are not equal.
    /// </summary>
    /// <param name="x">The first <see cref="UniValue"/> instance.</param>
    /// <param name="y">The second <see cref="UniValue"/> instance.</param>
    public static bool operator !=(UniValue x, UniValue y)
    {
      if (UniValue.IsNullOrEmpty(x))
      {
        return !UniValue.Empty.Equals(y);
      }
      else
      {
        return !x.Equals(y);
      }
    }

    /// <summary>
    /// Indicate whether two <see cref="UniValue"/> are equal.
    /// </summary>
    /// <param name="x">The first <see cref="UniValue"/> instance.</param>
    /// <param name="y">The second <see cref="UniValue"/> instance.</param>
    public static bool operator ==(UniValue x, UniValue y)
    {
      if (UniValue.IsNullOrEmpty(x))
      {
        return UniValue.Empty.Equals(y);
      }
      else
      {
        return x.Equals(y);
      }
    }

    /// <summary>
    /// Indicate whether <see cref="UniValue"/> and <see cref="System.String"/> are not equal.
    /// </summary>
    /// <param name="x">The <see cref="UniValue"/> instance.</param>
    /// <param name="y">The string.</param>
    public static bool operator !=(UniValue x, string y)
    {
      if (UniValue.IsNullOrEmpty(x))
      {
        return !UniValue.Empty.Equals(y);
      }
      else
      {
        return !x.Equals(y);
      }
    }

    /// <summary>
    /// Indicate whether <see cref="UniValue"/> and <see cref="System.String"/> are equal.
    /// </summary>
    /// <param name="x">The <see cref="UniValue"/> instance.</param>
    /// <param name="y">The string.</param>
    public static bool operator ==(UniValue x, string y)
    {
      if (UniValue.IsNullOrEmpty(x))
      {
        return UniValue.Empty.Equals(y);
      }
      else
      {
        return x.Equals(y);
      }
    }

    #endregion
    #region ..ilistSource..

    /// <summary>
    /// Gets a value indicating whether the collection is a collection of <see cref="IList"/> objects.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ContainsListCollection
    {
      get 
      {
        return true; 
      }
    }

    /// <summary>
    /// Returns an <see cref="IList"/> that can be bound to a data source from an object that does not implement an <see cref="IList"/> itself.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IList GetList()
    {
      if (!this.HasValue)
      {
        return null; //new List<UniValue>();
      }

      var result = new List<UniValue>();

      if (this.IsValue)
      {
        result.Add((UniValue)this.Data);
      }
      else if (this.IsCollection)
      {
        result.AddRange(this.CollectionItems.Values.ToArray());
      }
      else
      {
        result.Add(new UniValue(this.Data));
      }

      return result;
    }

    #endregion
    #region ..icustomTypeDescriptor..

    /// <summary>
    /// Returns a collection of custom attributes for this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public AttributeCollection GetAttributes()
    {
      return new AttributeCollection();
    }

    /// <summary>
    /// Returns the class name of this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string GetClassName()
    {
      return null;
    }

    /// <summary>
    /// Returns the name of this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)] 
    public string GetComponentName()
    {
      return null;
    }

    /// <summary>
    /// Returns a type converter for this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public TypeConverter GetConverter()
    {
      return null;
    }

    /// <summary>
    /// Returns the default event for this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public EventDescriptor GetDefaultEvent()
    {
      return null;
    }

    /// <summary>
    /// Returns the default property for this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public PropertyDescriptor GetDefaultProperty()
    {
      return null;
    }

    /// <summary>
    /// Returns an editor of the specified type for this instance of a component.
    /// </summary>
    /// <param name="editorBaseType">A <see cref="System.Type"/> that represents the editor for this object.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object GetEditor(Type editorBaseType)
    {
      return null;
    }

    /// <summary>
    /// Returns the events for this instance of a component using the specified attribute array as a filter.
    /// </summary>
    /// <param name="attributes">An array of type <see cref="Attribute"/> that is used as a filter.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
      return EventDescriptorCollection.Empty;
    }

    /// <summary>
    /// Returns the events for this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public EventDescriptorCollection GetEvents()
    {
      return this.GetEvents(null);
    }

    /// <summary>
    /// Returns the properties for this instance of a component using the attribute array as a filter.
    /// </summary>
    /// <param name="attributes">An array of type <see cref="Attribute"/> that is used as a filter.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
      if (!this.HasValue)
      {
        return PropertyDescriptorCollection.Empty;
      }

      if (this.IsValue)
      {
        return new UniValueTypeDescriptor(new UniValueCollection { { this.Key, this } }).GetProperties();
      }
      else if (this.IsCollection)
      {
        return new UniValueTypeDescriptor(this.CollectionItems).GetProperties();
      }
      else
      {
        return new UniValueTypeDescriptor(new UniValueCollection { { this.Key, new UniValue(this.Data) } }).GetProperties();
      }
    }

    /// <summary>
    /// Returns the properties for this instance of a component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public PropertyDescriptorCollection GetProperties()
    {
      return this.GetProperties(null);
    }

    /// <summary>
    /// Returns an object that contains the property described by the specified property descriptor.
    /// </summary>
    /// <param name="pd">A <see cref="PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object GetPropertyOwner(PropertyDescriptor pd)
    {
      return new UniValue();
    }

    #endregion

  }

}