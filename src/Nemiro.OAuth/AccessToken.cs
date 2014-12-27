// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014. All rights reserved.
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
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Nemiro.OAuth
{

  /// <summary>
  /// Represents base properties and method for access token results.
  /// </summary>
	[Serializable]
  public abstract class AccessToken : RequestResult
	{

		#region ..fields & properties..

		/// <summary>
    /// The access token issued by the authorization server.
    /// </summary>
    public string Value { get; protected set; }

		#endregion
		#region ..constructor..

		/// <summary>
    /// Initializes a new instance of the <see cref="AccessToken"/> class.
    /// </summary>
    /// <param name="result">Result of request to the OAuth server.</param>
    public AccessToken(RequestResult result) : base(result) { }

		#endregion
		#region ..methods..

		/// <summary>
    /// Returns the <see cref="AccessToken.Value"/>.
    /// </summary>
    public override string ToString()
    {
      return this.Value;
    }

		#endregion
		#region ..iserializable..

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessToken"/>.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> with data.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
		protected AccessToken(SerializationInfo info, StreamingContext context) : base(info, context)
    {
			this.Value = info.GetString("Value");
    }

    /// <summary>
    /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException("info");
      }
			info.AddValue("Value", this.Value);
      base.GetObjectData(info, context);
    }

    #endregion

  }
  
}