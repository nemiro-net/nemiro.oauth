using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;

namespace TestProject1
{

  [TestClass]
  public class AccessTokenTest
  {

    [TestMethod]
    public void TestMethod1()
    {
      AccessToken token = AccessToken.Empty;
      Assert.IsTrue(token.IsEmpty);

      var token2 = new OAuthAccessToken(new RequestResult("text/html; charset=utf-8", "oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test"));
      Assert.AreEqual(token2.Value, "123456789");
      Assert.AreEqual(token2.TokenSecret, "abcdef");

      var token3 = new OAuth2AccessToken(new RequestResult("text/plain; charset=UTF-8", "access_token=123abc&expires=5180667"));
      Assert.AreEqual(token3.Value, "123abc");

      token = "123";
      Console.WriteLine("123 = {0}", token);
      Assert.AreEqual(token.Value, "123");

      token2 = "123";
      Console.WriteLine("123 = {0}", token2);
      Assert.AreEqual(token2.Value, "123");

      token3 = "123";
      Console.WriteLine("123 = {0}", token3);
      Assert.AreEqual(token3.Value, "123");

      var token4 = AccessToken.Parse("oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test");
      Console.WriteLine(token4.GetType());
      Assert.AreEqual(token4.GetType(), typeof(OAuthAccessToken));

      var token5 = AccessToken.Parse("access_token=123abc&expires=5180667");
      Console.WriteLine(token5.GetType());
      Assert.AreEqual(token5.GetType(), typeof(OAuth2AccessToken));

      var token6 = AccessToken.Parse<OAuthAccessToken>("oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test");
      Console.WriteLine(token6.GetType());
      Assert.AreEqual(token6.Value, "123456789");
      Assert.AreEqual(token6.TokenSecret, "abcdef");

      var token7 = AccessToken.Parse<OAuth2AccessToken>("access_token=123abc&expires=5180667");
      Console.WriteLine(token7.GetType());
      Assert.AreEqual(token7.Value, "123abc");

      try
      {
        var token8 = AccessToken.Parse<OAuth2AccessToken>("oauth_token=123456789&oauth_token_secret=abcdef&user_id=111&screen_name=test");
        Assert.Fail();
      }
      catch (InvalidCastException ex)
      {
      }
      catch (Exception ex)
      {
        Assert.Fail();
      }

      try
      {
        var token9 = AccessToken.Parse<OAuthAccessToken>("access_token=123abc&expires=5180667");
        Assert.Fail();
      }
      catch (InvalidCastException ex)
      {
      }
      catch (Exception ex)
      {
        Assert.Fail();
      }

      AccessToken token10 = null;
      Assert.IsNull(token10);

      AccessToken token11 = "";
      Assert.IsNotNull(token11);

      // AccessToken token11 = new OAuthAccessToken(new RequestResult("text/plain", null));
      // Assert.IsNull(token11);

      
      /*var client = new FacebookClient
      (
        "1435890426686808",
        "c6057dfae399beee9e8dc46a4182e8fd"
      );

      var token12 = client.GetSpecifiedTokenOrCurrent(null);
      */

      var token13 = new OAuth2AccessToken("123", "4567");
      Assert.AreEqual(token13.Value, "123");
      Assert.AreEqual(token13.RefreshToken, "4567");
    }

  }

}
