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
namespace Nemiro.OAuth
{

  /// <summary>
  /// Variants of the signature encryption.
  /// </summary>
  public static class SignatureMethods
  {

    /// <summary>
    /// HMAC-SHA1
    /// </summary>
    public const string HMACSHA1 = "HMAC-SHA1";

    /// <summary>
    /// RSA-SHA1
    /// </summary>
    public const string RSASHA1 = "RSA-SHA1";

    /// <summary>
    /// PLAINTEXT
    /// </summary>
    public const string PLAINTEXT = "PLAINTEXT";

  }

}