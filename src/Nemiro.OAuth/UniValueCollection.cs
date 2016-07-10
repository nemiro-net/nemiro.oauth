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
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections;
using System.Security.Permissions;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents the collection of the <see cref="UniValue"/>.
  /// </summary>
  [Serializable]
  public class UniValueCollection :
  IDictionary<string, UniValue>,
  ICollection<KeyValuePair<string, UniValue>>,
  IEnumerable<KeyValuePair<string, UniValue>>,
  IEnumerable, ISerializable
  {

    #region ..fields & properties..

    [EditorBrowsable(EditorBrowsableState.Never)]
    private Dictionary<string, UniValue> _Items = new Dictionary<string, UniValue>();

    /// <summary>
    /// Gets or sets items of the collection.
    /// </summary>
    public Dictionary<string, UniValue> Items
    {
      get
      {
        return _Items;
      }
      protected set
      {
        _Items = value;
      }
    }

    /// <summary>
    /// The reference to parent.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public UniValue Parent { get; internal set; }

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //internal bool Unreferenced { get; set; }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    public UniValue this[string key]
    {
      get
      {
        return this.Items[key];
      }
      set
      {
        this.Items[key] = value;
      }
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    public UniValue this[int index]
    {
      get
      {
        if (this.ContainsKey(index.ToString()))
        {
          return this[index.ToString()];
        }
        if (index < 0 || index > this.Count - 1) { return UniValue.Empty; }
        return this[this.Keys.ToArray()[index]];
      }
    }

    /// <summary>
    /// Gets a collection containing the keys in the collection.
    /// </summary>
    public Dictionary<string, UniValue>.KeyCollection Keys
    {
      get
      {
        return this.Items.Keys;
      }
    }

    /// <summary>
    /// Gets a collection containing the values in the collection.
    /// </summary>
    public Dictionary<string, UniValue>.ValueCollection Values
    {
      get
      {
        return this.Items.Values;
      }
    }

    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    public int Count
    {
      get
      {
        return this.Items.Count;
      }
    }

    #endregion
    #region ..constructor..

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValueCollection"/>.
    /// </summary>
    public UniValueCollection()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValueCollection"/> with a specified reference to parent.
    /// </summary>
    public UniValueCollection(UniValue parent)
    {
      this.Parent = parent;
      //this.Unreferenced = (parent != null ? parent.Unreferenced : false);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValueCollection"/> by other an instance of the <see cref="UniValueCollection"/>.
    /// </summary>
    public UniValueCollection(UniValueCollection source)
    {
      if (source == null) { throw new ArgumentNullException("source"); }
      //this.Unreferenced = (source.Parent != null ? source.Parent.Unreferenced : false);
      foreach (KeyValuePair<string, UniValue> itm in source)
      {
        this.Add(itm.Key, itm.Value);
      }
    }

    #endregion
    #region ..methods..

    /// <summary>
    /// Adds the specified key and value to the collection.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    public void Add(string key, UniValue value)
    {
      if (String.IsNullOrEmpty(key))
      {
        key = "____";
      }
      if (!this.ContainsKey(key))
      {
        // first key
        value.Key = key;
        value.Parent = this.Parent;
        this.Items.Add(key, value);
        if (this.Parent != null && this.Parent.IsArraySubtype && !OAuthUtility.IsNumeric(key))
        {
          this.Parent.IsArraySubtype = false;
        }
      }
      else
      {
        // key exists
        if (this[key].IsArraySubtype)
        {
          // is array, push item
          this[key].Add(this[key].Count.ToString(), value);
        }
        else
        {
          // is not array, create it
          this[key] = new UniValue(new UniValue[] { new UniValue(this[key]), value });
          this[key].Parent = this.Parent;
        }
        this[key].Key = key;
      }
    }

    /// <summary>
    /// Determines whether the collection contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is <b>null</b>.</exception>
    public bool ContainsKey(string key)
    {
      return this.Items.ContainsKey(key);
    }

    /// <summary>
    /// Removes the value with the specified key from the collection.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>
    /// <b>true</b> if the element is successfully found and removed; otherwise, <b>false</b>. 
    /// </returns>
    public bool Remove(string key)
    {
      return this.Items.Remove(key);
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="UniValueCollection"/>.
    /// </summary>
    public override string ToString()
    {
      if (this.Parent != null && this.Parent.IsArraySubtype) //  && !this.Any(itm => itm.Value.HasAttributes)
      {
        return String.Format
        (
          "[{0}]",
          String.Join
          (
            ", ",
            this.Select
            (
              itm => this.ItemToString(itm)
            ).ToArray()
          )
        );
      }
      else
      {
        return String.Format
        (
          "{{ {0} }}",
          String.Join
          (
            ", ",
            this.Select
            (
              itm => this.ItemToString(itm)
            ).ToArray()
          )
        );
      }
    }

    /// <summary>
    /// Determines whether two object instances are equal.
    /// </summary>
    /// <param name="o">The object to compare with the current instance of the <see cref="UniValueCollection"/>.</param>
    /// <returns><b>true</b> if the specified object is equal to the current <see cref="UniValueCollection"/>; otherwise, <b>false</b>.</returns>
    public new bool Equals(object o)
    {
      if (o == null) { return false; }
      if (o.GetType() == typeof(UniValueCollection))
      {
        return base.Equals(o);
      }
      else if (o.GetType() == typeof(UniValue) && ((UniValue)o).IsCollection)
      {
        return ((UniValue)o).CollectionItems.Equals(this);
      }
      else if (o.GetType() == typeof(String) || ((o.GetType() == typeof(UniValue) && ((UniValue)o).IsString)))
      {
        return o.ToString().Equals(this.ToString());
      }
      else
      {
        return o.Equals(this);
      }
    }

    /// <summary>
    /// Returns a string that represents the specified item of the <see cref="UniValueCollection"/>.
    /// </summary>
    /// <param name="itm">The item to converted.</param>
    private string ItemToString(KeyValuePair<string, UniValue> itm)
    {
      if ((itm.Value.Parent != null && itm.Value.Parent.IsArraySubtype) && !itm.Value.HasAttributes)
      {
        return this.ValueToString(itm.Value);
      }
      else if ((itm.Value.Parent != null && itm.Value.Parent.IsArraySubtype) && itm.Value.HasAttributes)
      {
        if (itm.Value.HasValue && ((itm.Value.IsString && !String.IsNullOrEmpty(itm.Value.ToString())) || !itm.Value.IsString))
        {
          return String.Format
          (
            "{{ \"value\": {0}, {1} }}",
            this.ValueToString(itm.Value),
            String.Join(", ", itm.Value.Attributes.AllKeys.Select(k => String.Format("\"@{0}\": \"{1}\"", k, OAuthUtility.JavaScriptStringEncode(itm.Value.Attributes[k]))).ToArray())
          );
        }
        else
        {
          return String.Format
          (
            "{{ {0} }}",
            String.Join(", ", itm.Value.Attributes.AllKeys.Select(k => String.Format("\"@{0}\": \"{1}\"", k, OAuthUtility.JavaScriptStringEncode(itm.Value.Attributes[k]))).ToArray())
          );
        }
      }

      if (itm.Value.IsArraySubtype && !itm.Value.HasAttributes)
      {
        return String.Format
        (
          "\"{0}\": {1}",
          itm.Key,
          this.ValueToString(itm.Value)
        );
      }

      if (itm.Value.HasAttributes)
      {
        if (itm.Value.HasValue && ((itm.Value.IsString && !String.IsNullOrEmpty(itm.Value.ToString())) || !itm.Value.IsString))
        {
          return String.Format
          (
            "\"{0}\": {{ value: {1}, {2} }}",
            itm.Key,
            this.ValueToString(itm.Value),
            String.Join(", ", itm.Value.Attributes.AllKeys.Select(k => String.Format("\"@{0}\": \"{1}\"", k, OAuthUtility.JavaScriptStringEncode(itm.Value.Attributes[k]))).ToArray())
          );
        }
        else
        {
          return String.Format
          (
            "\"{0}\": {{ {1} }}",
            itm.Key,
            String.Join(", ", itm.Value.Attributes.AllKeys.Select(k => String.Format("\"@{0}\": \"{1}\"", k, OAuthUtility.JavaScriptStringEncode(itm.Value.Attributes[k]))).ToArray())
          );
        }
      }

      return String.Format
      (
        "\"{0}\": {1}",
        itm.Key,
        this.ValueToString(itm.Value)
      );
    }

    /// <summary>
    /// Returns a string that represents the specified <see cref="UniValue"/> instance.
    /// </summary>
    /// <param name="value">The value to converted.</param>
    private string ValueToString(UniValue value)
    {
      if (value.IsString)
      {
        return String.Format("\"{0}\"", OAuthUtility.JavaScriptStringEncode(value.ToString()));
      }
      else if (value.IsBoolean)
      {
        return String.Format("{0}", value.ToString().ToLower());
      }
      else if (value.IsDateTime)
      {
        return String.Format("\"{0}\"", ((DateTime)value).ToString("r"));
      }
      else if (value.IsNumeric)
      {
        return String.Format("{0}", value.ToString().Replace(",", "."));
      }
      else
      {
        return !value.HasValue ? "null" : value.ToString();
      }
    }

    #endregion
    #region ..icollection..

    /// <summary>
    /// Gets a collection containing the keys in the collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    ICollection<string> IDictionary<string, UniValue>.Keys
    {
      get
      {
        return this.Items.Keys;
      }
    }

    /// <summary>
    /// Gets a collection containing the values in the collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    ICollection<UniValue> IDictionary<string, UniValue>.Values
    {
      get
      {
        return this.Items.Values;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the collection is read-only. Always <b>false</b>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Adds the specified item to the <see cref="UniValueCollection"/>.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(KeyValuePair<string, UniValue> item)
    {
      this.Add(item.Key, item.Value);
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    public void Clear()
    {
      this.Items.Clear();
    }

    /// <summary>
    /// Determines whether the collection contains a specific value.
    /// </summary>
    /// <param name="item">The object to locate in the collection.</param>
    public bool Contains(KeyValuePair<string, UniValue> item)
    {
      return this.ContainsKey(item.Key);
    }

    /// <summary>
    /// Removes the first occurrence of a specific object from the collection. 
    /// </summary>
    /// <param name="item">The object to remove from the collection.</param>
    public bool Remove(KeyValuePair<string, UniValue> item)
    {
      return this.Items.Remove(item.Key);
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key whose value to get.</param>
    /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the <see cref="UniValue.Empty"/>.</param>
    public bool TryGetValue(string key, out UniValue value)
    {
      if (this.ContainsKey(key))
      {
        value = this[key];
        return true;
      }
      else
      {
        value = UniValue.Empty;
        return false;
      }
    }

    /// <summary>
    /// [Is not implemented] Copies the elements of the collection to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from collection. The <see cref="System.Array"/> must have zero-based indexing. </param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void CopyTo(KeyValuePair<string, UniValue>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    #endregion
    #region ..ienumerable..

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    public IEnumerator GetEnumerator()
    {
      return this.Items.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    IEnumerator<KeyValuePair<string, UniValue>> IEnumerable<KeyValuePair<string, UniValue>>.GetEnumerator()
    {
      return this.Items.GetEnumerator();
    }

    #endregion
    #region ..iserializable..

    /// <summary>
    /// Initializes a new instance of the <see cref="UniValueCollection"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected UniValueCollection(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
      this.Parent = (UniValue)info.GetValue("Parent", typeof(UniValue));
      //this.Unreferenced = (bool)info.GetValue("Unreferenced", typeof(bool));
      int count = info.GetInt32("ItemsCount");
      for (int i = 0; i < count; i++)
      {
        string key = (string)info.GetValue(String.Format("ItemKey{0}", i), typeof(string));
        UniValue value = (UniValue)info.GetValue(String.Format("ItemValue{0}", i), typeof(UniValue));
        this.Add(key, (UniValue.IsNullOrEmpty(value) ? UniValue.Empty : value));
      }
    }

    /// <summary>
    /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
      info.AddValue("Parent", this.Parent, typeof(UniValue));
      //info.AddValue("Unreferenced", this.Unreferenced, typeof(bool));

      int i = 0;
      foreach (KeyValuePair<string, UniValue> item in this)
      {
        info.AddValue(String.Format("ItemKey{0}", i), item.Key, typeof(string));
        info.AddValue(String.Format("ItemValue{0}", i), item.Value, typeof(UniValue));
        i++;
      }

      info.AddValue("ItemsCount", this.Count);
    }

    #endregion
    #region ..operators..

    /// <summary>
    /// Returns <see cref="UniValueCollection"/> instance from <see cref="UniValue"/>.
    /// </summary>
    /// <param name="value">The value from which will be returned the <see cref="UniValueCollection"/>.</param>
    public static implicit operator UniValueCollection(UniValue value)
    {
      return value.CollectionItems;
    }

    #endregion

  }

}