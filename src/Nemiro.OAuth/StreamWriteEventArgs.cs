// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014-2016. All rights reserved.
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
  /// Provides data for the <see cref="HttpWriteRequestStream"/>.
  /// </summary>
  public class StreamWriteEventArgs
  {

    /// <summary>
    /// The event, which occurs when the state of the current instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This event occurs at change the property values:
    /// <see cref="StreamWriteEventArgs.BytesWritten"/>, 
    /// <see cref="StreamWriteEventArgs.TotalBytesWritten"/> and
    /// <see cref="StreamWriteEventArgs.IsCompleted"/>.
    /// </para>
    /// <para>
    /// For <see cref="StreamWriteEventArgs.BytesWritten"/> and <see cref="StreamWriteEventArgs.TotalBytesWritten"/> comes only one call of events.
    /// </para>
    /// </remarks>
    public event EventHandler Changed;

    private long _BytesWritten = 0;

    /// <summary>
    /// Gets the number of bytes written.
    /// </summary>
    /// <remarks>
    /// <para>This value does not exceed the buffer size.</para>
    /// </remarks>
    public long BytesWritten
    {
      get
      {
        return _BytesWritten;
      }
      internal set
      {
        _BytesWritten = value;
        this.TotalBytesWritten += value;
      }
    }

    private long _TotalBytesWritten = 0;

    /// <summary>
    /// Get the total number of bytes witten.
    /// </summary>
    public long TotalBytesWritten
    {
      get
      {
        return _TotalBytesWritten;
      }
      protected set
      {
        if (_TotalBytesWritten != value)
        {
          _TotalBytesWritten = value;
          this.OnChanged();
        }
        else
        {
          _TotalBytesWritten = value;
        }
      }
    }

    private bool _IsCompleted = false;

    /// <summary>
    /// Gets a value indicating the status of completion write to the stream.
    /// </summary>
    public bool IsCompleted
    {
      get
      {
        return _IsCompleted;
      }
      internal set
      {
        if (_IsCompleted != value)
        {
          _IsCompleted = value;
          this.OnChanged();
        }
        else
        {
          _IsCompleted = value;
        }
      }
    }

    /// <summary>
    /// Gets a unique user state.
    /// </summary>
    public object State { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamWriteEventArgs"/> class.
    /// </summary>
    public StreamWriteEventArgs() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamWriteEventArgs"/> class with a specified value unique user state.
    /// </summary>
    /// <param name="state">The user state.</param>
    public StreamWriteEventArgs(object state)
    {
      this.State = state;
    }

    /// <summary>
    /// Raises the <see cref="StreamWriteEventArgs.Changed"/> event.
    /// </summary>
    protected virtual void OnChanged()
    {
      if (this.Changed == null) { return; }
      this.Changed(this, default(EventArgs));
    }

  }

}