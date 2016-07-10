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
  /// The exception that is thrown when you attempt to register the already registered client.
  /// </summary>
  [Serializable]
  public class DuplicateProviderException : Exception
  {

    private string _Message = "";

    /// <summary>
    /// Gets an error message.
    /// </summary>
    public override string Message
    {
      get
      {
        return _Message;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateProviderException"/> class.
    /// </summary>
    /// <param name="providerName">The name of the provider.</param>
    [Obsolete("Please use an overload. // v1.8", true)]
    public DuplicateProviderException(string providerName) : base(String.Format("Provider \"{0}\" already registered.", providerName)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateProviderException"/> class.
    /// </summary>
    /// <param name="clientName">The name of the provider and client.</param>
    public DuplicateProviderException(ClientName clientName)
    {
      if (!String.IsNullOrEmpty(clientName.Key))
      {
        _Message = String.Format("Provider \"{0}\" with name \"{1}\" already registered.", clientName.ProviderName, clientName.Key);
      }
      else
      {
        _Message = String.Format("Provider \"{0}\" already registered. You can use the ClientName, to register multiple clients for the same provider.", clientName);
      }
    }

  }

}