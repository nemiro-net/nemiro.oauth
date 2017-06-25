using System;
using Xunit;

namespace Nemiro.OAuth.Test
{

  public class AccessTokenTest
  {

    [Fact]
    public void Common()
    {
      AccessToken token = AccessToken.Empty;
      Assert.True(token.IsEmpty);

      var token2 = new OAuthAccessToken(new RequestResult("text/html; charset=utf-8", "oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test"));

      Assert.Equal<string>("123456789", token2.Value);
      Assert.Equal<string>("abcdef", token2.TokenSecret);

      var token3 = new OAuth2AccessToken(new RequestResult("text/plain; charset=UTF-8", "access_token=123abc&expires=5180667"));

      Assert.Equal<string>("123abc", token3.Value);

      token = "123";

      Assert.Equal<string>("123", token.Value);

      token2 = "123";

      Assert.Equal<string>("123", token2.Value);

      token3 = "123";

      Assert.Equal<string>("123", token3.Value);

      var token4 = AccessToken.Parse("oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test");

      Assert.Equal(typeof(OAuthAccessToken), token4.GetType());

      var token5 = AccessToken.Parse("access_token=123abc&expires=5180667");

      Assert.Equal(typeof(OAuth2AccessToken), token5.GetType());

      var token6 = AccessToken.Parse<OAuthAccessToken>("oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test");

      Assert.Equal<string>("123456789", token6.Value);
      Assert.Equal<string>("abcdef", token6.TokenSecret);

      var token7 = AccessToken.Parse<OAuth2AccessToken>("access_token=123abc&expires=5180667");

      Assert.Equal<string>("123abc", token7.Value);

      Assert.Throws<InvalidCastException>(() => AccessToken.Parse<OAuth2AccessToken>("oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test"));
      Assert.Throws<InvalidCastException>(() => AccessToken.Parse<OAuthAccessToken>("access_token=123abc&expires=5180667"));

      AccessToken token10 = null;
      Assert.Null(token10);

      AccessToken token11 = "";
      Assert.NotNull(token11);
      
      var token13 = new OAuth2AccessToken("123", "4567");

      Assert.Equal<string>("123", token13.Value);
      Assert.Equal<string>("4567", token13.RefreshToken);
    }

  }

}