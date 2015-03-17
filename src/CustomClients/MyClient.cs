using System;
using Nemiro.OAuth;

public class MyClient : OAuth2Client
{

  /// <summary>
  /// Unique provider name: <b>MyClient</b>.
  /// </summary>
  public override string ProviderName
  {
    get
    {
      return "MyClient";
    }
  }

  /// <summary>
  /// Initializes a new instance of the class.
  /// </summary>
  /// <param name="clientId">The Client ID obtained from the provider website.</param>
  /// <param name="clientSecret">The Client Secret obtained from the provider website.</param>
  public MyClient(string clientId, string clientSecret) : base
  (
    "https://example.org/oauth",
    "https://example.org/oauth/access_token",
    clientId,
    clientSecret
  ) { }

  /// <summary>
  /// Gets the user details.
  /// </summary>
  /// <param name="accessToken">May contain an access token, which will have to be used in obtaining information about the user.</param>
  public override UserInfo GetUserInfo(AccessToken accessToken = null)
  {
    return new UserInfo(UniValue.Empty, null);
  }

}