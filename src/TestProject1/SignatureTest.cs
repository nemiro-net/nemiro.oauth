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

  }

}