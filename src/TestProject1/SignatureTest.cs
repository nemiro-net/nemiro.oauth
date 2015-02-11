using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nemiro.OAuth;
using System.Collections.Specialized;

namespace TestProject1
{

  [TestClass]
  public class SignatureTest
  {

    [TestMethod]
    public void Signature_Common()
    {
      string httpMethod = "GET";
      string url = "https://localhost/test";
      NameValueCollection param = null;

      var auth = new OAuthAuthorization();
      auth.ConsumerKey= "12345";
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

      if (singn != "vYE8cEP5ynznQRDqTxx307kc6rY=")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

    }

    [TestMethod]
    public void Signature_BodyHash()
    {
      string httpMethod = "GET";
      string url = "https://localhost/test";
      NameValueCollection param = null;

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

      if (singn != "0dMQJB8HJSDse2/P4C0icvIbHfU=")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

    }

  }

}