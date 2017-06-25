using System;
using System.Collections.Specialized;
using Xunit;

namespace Nemiro.OAuth.Test
{

  public class SignatureTest
  {

    [Fact]
    public void Common()
    {
      NameValueCollection param = null;
      string httpMethod = "GET";
      string url = "https://localhost/test";

      var auth = new OAuthAuthorization();
      auth.ConsumerKey = "12345";
      auth.ConsumerSecret = "1234567890";
      auth.TokenSecret = "abc";
      auth.Token = "xyz";
      auth.Nonce = "000000";
      auth.SignatureMethod = "HMAC-SHA1";
      auth.Timestamp = "1111111111";
      auth.Version = "1.0";

      string b = OAuthAuthorization.GetSignatureBaseString(httpMethod, url, param, auth);

      var singn = new OAuthSignature
      (
        auth.SignatureMethod,
        String.Format("{0}&{1}", auth.ConsumerSecret, auth.TokenSecret),
        OAuthAuthorization.GetSignatureBaseString(httpMethod, url, param, auth)
      ).ToString();

      Assert.Equal("vYE8cEP5ynznQRDqTxx307kc6rY=", singn);
    }

    [Fact]
    public void BodyHash()
    {
      NameValueCollection param = null;
      string httpMethod = "GET";
      string url = "https://localhost/test";

      var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();

      var auth = new OAuthAuthorization();
      auth.ConsumerKey = "123123123";
      auth.ConsumerSecret = "111111111111";
      auth.SignatureMethod = "HMAC-SHA1";
      auth.Nonce = "10098421";
      auth.Timestamp = "1423300052";
      auth.Version = "1.0";
      auth["oauth_body_hash"] = Convert.ToBase64String(sha1.ComputeHash(new byte[] { }));

      string b = OAuthAuthorization.GetSignatureBaseString(httpMethod, url, param, auth);

      var singn = new OAuthSignature
      (
        auth.SignatureMethod,
        String.Format("{0}&{1}", auth.ConsumerSecret, auth.TokenSecret),
        OAuthAuthorization.GetSignatureBaseString(httpMethod, url, param, auth)
      ).ToString();

      Assert.Equal("0dMQJB8HJSDse2/P4C0icvIbHfU=", singn);
    }

  }

}