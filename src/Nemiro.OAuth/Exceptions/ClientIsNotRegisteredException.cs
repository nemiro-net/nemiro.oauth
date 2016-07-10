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
  /// The exception that is thrown when trying to access an unregistered OAuth client.
  /// </summary>
  /// <remarks>
  /// <para>Use the <see cref="OAuthManager.RegisterClient(OAuthBase)" /> for OAuth clients registration.</para>
  /// </remarks>
  /// <example>
  /// <para>The following example illustrates a situation in which the <see cref="ClientIsNotRegisteredException"/> is thrown.</para>
  /// <code lang="C#">
  /// string url = OAuthWeb.GetAuthorizationUrl("facebook");
  /// // ...
  /// </code>
  /// <code lang="VB">
  /// Dim url As String = OAuthWeb.GetAuthorizationUrl("facebook");
  /// ' ...
  /// </code>
  /// <para><term><img src="../img/ex001.png" alt="ClientIsNotRegisteredException" /></term></para>
  /// <para>To solve the problem enough to register the client.</para>
  /// <code lang="C#">
  /// // facebook client registration
  /// OAuthManager.RegisterClient
  /// (
  ///   new FacebookClient
  ///   (
  ///     "1435890426686808", 
  ///     "c6057dfae399beee9e8dc46a4182e8fd"
  ///   )
  /// );
  /// </code>
  /// <code lang="VB">
  /// ' facebook client registration
  /// OAuthManager.RegisterClient _
  /// (
  ///   New FacebookClient _
  ///   (
  ///     "1435890426686808", 
  ///     "c6057dfae399beee9e8dc46a4182e8fd"
  ///   )
  /// )
  /// </code>
  /// <para>Enjoy!</para>
  /// <code lang="C#">
  /// string url = OAuthWeb.GetAuthorizationUrl("facebook");
  /// // ...
  /// </code>
  /// <code lang="VB">
  /// Dim url As String = OAuthWeb.GetAuthorizationUrl("facebook");
  /// ' ...
  /// </code>
  /// </example>
  [Serializable]
  public class ClientIsNotRegisteredException : Exception
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientIsNotRegisteredException"/> class.
    /// </summary>
    public ClientIsNotRegisteredException() : base() { }

  }

}