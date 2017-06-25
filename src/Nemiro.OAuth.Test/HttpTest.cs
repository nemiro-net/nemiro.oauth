using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Xunit;

namespace Nemiro.OAuth.Test
{

  public class HttpTest
  {

    [Fact]
    public void HttpParametersTest()
    {
      object obj = null;
      string str = null;
      int? @int = null;

      var parameters = new HttpParameterCollection
      {
        { "test", obj },
        { "test2", str },
        { "test3", @int }
      };

      foreach (var p in parameters)
      {
        Assert.Equal(HttpParameterType.Unformed, p.ParameterType);
      }

      string r = parameters.ToStringParameters();

      Assert.Equal("test=&test2=&test3=", r);

      parameters = new HttpParameterCollection
      {
        { "n", 1 },
        { "n", 2 },
        { "n", 3 }
      };

      r = parameters.ToStringParameters();

      Assert.Equal("n=1&n=2&n=3", r);

      parameters = new HttpParameterCollection
      {
        { "n[]", 1 },
        { "n[]", 2 },
        { "n[]", 3 }
      };

      r = parameters.ToStringParameters();

      Assert.Equal("n%5b%5d=1&n%5b%5d=2&n%5b%5d=3", r);

      var constructorInfo = typeof(System.Web.HttpPostedFile).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();
      var f = (System.Web.HttpPostedFile)constructorInfo.Invoke(new object[] { "oauth.png", "image/png", null });

      parameters = new HttpParameterCollection
      {
        { "file", "test.dat", "text/plain", new byte[] {1, 2, 3, 4, 5} },
        { "file2", "test2.dat", new byte[] {1, 2, 3, 4, 5} },
        { "file3", "test3.dat",  new MemoryStream(Properties.Resources.oauth) },
        { "file4", f },
        { "x", 123 },
        { "y", 456 }
      };

      for (int i = 0; i < parameters.Count; i++)
      {
        var p = parameters[i];

        if (i < 3)
        {
          Assert.Equal(HttpParameterType.File, p.ParameterType);
        }
      }

      r = parameters.ToStringParameters();

      Assert.Equal("x=123&y=456", r);

      parameters = new HttpParameterCollection
      {
        { new byte[] {1, 2, 3, 4, 5} },
        { "x", 123 },
        { "y", 456 }
      };

      for (int i = 0; i < parameters.Count; i++)
      {
        var p = parameters[i];

        if (i == 0)
        {
          Assert.Equal(HttpParameterType.RequestBody, p.ParameterType);
        }
      }

      r = parameters.ToStringParameters();

      Assert.Equal("x=123&y=456", r);

      parameters = new HttpParameterCollection
      {
        { f },
        { "x", 123 },
        { "y", 456 }
      };

      for (int i = 0; i < parameters.Count; i++)
      {
        var p = parameters[i];

        if (i == 0)
        {
          Assert.Equal(HttpParameterType.RequestBody, p.ParameterType);
        }
      }

      r = parameters.ToStringParameters();

      Assert.Equal("x=123&y=456", r);

      parameters = new HttpParameterCollection
      {
        { Properties.Resources.oauth },
        { "x", 123 },
        { "x", 789 },
        { "y", 456 }
      };

      Assert.Equal(HttpParameterType.RequestBody, parameters[0].ParameterType);
      Assert.Equal(HttpParameterType.Unformed, parameters[1].ParameterType);
      Assert.Equal(HttpParameterType.Unformed, parameters[2].ParameterType);
      Assert.Equal(HttpParameterType.Unformed, parameters[3].ParameterType);
      
      r = parameters.ToStringParameters();

      Assert.Equal("x=123&x=789&y=456", r);

      object obj2 = Properties.Resources.oauth;
      parameters = new HttpParameterCollection
      {
        { obj2 }
      };


      for (int i = 0; i < parameters.Count; i++)
      {
        var p = parameters[i];

        if (i == 0)
        {
          Assert.Equal(HttpParameterType.RequestBody, p.ParameterType);
        }
      }

      var parameters2 = new NameValueCollection
      {
        { "a", "123" },
        { "a", "245" },
        { "b", "abc" }
      };

      r = ((HttpParameterCollection)parameters2).ToStringParameters();

      Assert.Equal("a=123%2c245&b=abc", r);
    }

    [Fact]
    public void HttpParametersWriteTest()
    {
      object obj = null;
      string str = null;
      int? @int = null;

      var parameters = new HttpParameterCollection
      {
        { "test", obj },
        { "test2", str },
        { "test3", @int }
      };

      foreach (var p in parameters)
      {
        Assert.Equal(HttpParameterType.Unformed, p.ParameterType);
      }

      var req = (HttpWebRequest)WebRequest.Create("http://api.foxtools.ru/v2/hash");

      req.Method = "POST";

      parameters.WriteToRequestStream(req);

      string r = parameters.ToStringParameters();

      Assert.Equal("test=&test2=&test3=", r);

      parameters = new HttpParameterCollection
      {
        new HttpUrlParameter("test", "123"),
        new HttpFormParameter("test2", null),
        new HttpFormParameter("text", "test")
      };

      for (int i = 0; i < parameters.Count; i++)
      {
        var p = parameters[i];

        if (i == 0)
        {
          Assert.Equal(HttpParameterType.Url, p.ParameterType);
        }
        else
        {
          Assert.Equal(HttpParameterType.Form, p.ParameterType);
        }
      }

      req = (HttpWebRequest)WebRequest.Create("http://api.foxtools.ru/v2/hash");
      req.Method = "POST";

      parameters.WriteToRequestStream(req);

      r = parameters.ToStringParameters(HttpParameterType.Url);

      Assert.Equal("test=123", r);

      parameters = new HttpParameterCollection
      {
        new { a = 1, b = 2, c = "abc" }
      };

      for (int i = 0; i < parameters.Count; i++)
      {
        var p = parameters[i];

        Assert.Equal(HttpParameterType.RequestBody, p.ParameterType);
      }

      req = (HttpWebRequest)WebRequest.Create("http://api.foxtools.ru/v2/hash");
      req.Method = "POST";
      req.ContentType = "application/json";

      parameters.WriteToRequestStream(req);

      req.GetResponse();

      req = (HttpWebRequest)WebRequest.Create("http://api.foxtools.ru/v2/hash");
      req.Method = "POST";

      parameters.WriteToRequestStream(req);

      req.GetResponse();
    }

    [Fact]
    public void HttpHeadersTest()
    {
      var result = OAuthUtility.Post
      (
        "http://api.foxtools.ru/v2/Hash",
        headers: new NameValueCollection
        {
          { "Accept", "application/xml" },
          { "User-Agent", "FireBox" },
          { "X-XXX", "XXX" },
          { "Content-Type", "custom/type" }
        }
      );
    }

    [Fact]
    public void ExecuteRequestTest()
    {
      var parameters = new HttpParameterCollection
      {
        { "content", "hello world!" },
        { "contentType", "text/plain" }
      };

      var result = OAuthUtility.Get("http://api.foxtools.ru/v2/Http", parameters);

      Assert.True(result.Contains("hello world!"));

      parameters = new HttpParameterCollection
      {
        { Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAFUAAABVCAYAAAA49ahaAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsIAAA7CARUoSoAAAAR/SURBVHhe7ZDRbiMxDAPv/3+6px62gDomzGVsb+4hA0wNiJSS5k/x9aSO3X3mp70+U4endOzuMz/t9Zk6PKVjd5/5aa/P1OEpHbv7zE97feY43Im7n+ZO4vJV1P16x+FO3P00dxKXr6Lu1zsOd+Lup7mTuHwVdb/ecbgTdz/NncTlq6j79Y7DDnMnSXNKVKdL0txJVF7vOOwwd5I0p0R1uiTNnUTl9Y7DDnMnSXNKVKdL0txJVF7vOOwwd5I0p0R1uiTNnUTl9Y7DDnMnSfNUh+szdxKV1zsOO8ydJM1THa7P3ElUXu847DB3kjRPdbg+cydReb3jsMPcSdI81eH6zJ1E5fWOww5zJ3E5cX3mlKS5k6i83nHYYe4kLieuz5ySNHcSldc7DjvMncTlxPWZU5LmTqLyesdhh7mTuJy4PnNK0txJVF7vONzJu++7fBV1v95xuJN333f5Kup+veNwJ+++7/JV1P16x+FO3n3f5auo+/X+Hp6WPJ2f9vpMHZ6SPJ2f9vpMHZ6SPJ2f9vpMHZ6SPJ2f9t9n/vv7RtSXmsE+/R/4/KgH+PyoB/j8qAeo73H2S7n7zKlD7cx0qJ2Zipr70gruPnPqUDszHWpnpqLmvrSCu8+cOtTOTIfamamouS+t4O4zpw61M9OhdmYq7KeoQzMdaqebom50HWpn5h0+P6rYmXmHz48qdmbe4fOjip2Zd6ieXv5xFXWzm+L2mVOH2pmpqLku/7iKutlNcfvMqUPtzFTUXJd/XEXd7Ka4febUoXZmKmquyz+uom52U9w+c+pQOzMVy7/anQ+ZwX2aku6z7yQqz781UEcTuE9T0n32nUTl+bcG6mgC92lKus++k6g8/9ZAHU3gPk1J99l3EpXn3xqoox2XO9J99qkj7Ste22q4L+FyR7rPPnWkfcVrWw33JVzuSPfZp460r3htq+G+hMsd6T771JH2Fa9tbYT/hJO43MF9+gqfHxX79BU+Pyr26St8flTs01eoPX3slClunzklae5U1FyXT5ni9plTkuZORc11+ZQpbp85JWnuVNRcl0+Z4vaZU5LmTkXNfWmFp++nEtXpEpXXOw538vT9VKI6XaLyesfhTp6+n0pUp0tUXu843MnT91OJ6nSJyusdhx3mTuJywj51qJ0ucTlhn16dcdhh7iQuJ+xTh9rpEpcT9unVGYcd5k7icsI+daidLnE5YZ9enXHYYe4kLifsU4fa6RKXE/bp1RmHHeZO4nJHun+6f4e6Mz/K3Elc7kj3T/fvUHfmR5k7icsd6f7p/h3qzvwocydxuSPdP92/Q92ZH2XuJC53uP2nc+e1Mw47zJ3E5Q63/3TuvHbGYYe5k7jc4fafzp3XzjjsMHcSlzvc/tO589oZhztJ77NPiep0HWpn5h2qly8lpPfZp0R1ug61M/MO1cuXEtL77FOiOl2H2pl5h+rlSwnpffYpUZ2uQ+3MvEP19PIpSZrvlqzm39T8d+m0JM13S1bzb2r+u3Rakua7Jav5NzX/XTotSfPdktX8Gz39sMDX11/4Q2CGtGAk5wAAAABJRU5ErkJggg==") },
        { "test", 123 },
        { new HttpFormParameter("x", "y") },
        { new HttpFormParameter("a", "bc") },
        { "contentType", "text/plain" }
      };

      result = OAuthUtility.Post("http://api.foxtools.ru/v2/Http", parameters);

      Assert.True(result.ToString().IndexOf("PNG") != -1);

      parameters = new HttpParameterCollection
      {
        { new HttpFile("file1", "test.png", "image/png", Convert.FromBase64String("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")) },
        { "dd", 20 },
        { "id", 13 },
        { new HttpFormParameter("key", "e91268b7-1162-4044-9236-7191c8c3b5d2") },
        { "contentType", "text/plain" }
      };

      result = OAuthUtility.Post("http://api.foxtools.ru/v2/Http", parameters);

      Assert.True(result.ToString().Contains("test.png"));
      Assert.True(result.ToString().Contains("e91268b7-1162-4044-9236-7191c8c3b5d2"));

      using (var fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "oauth.dat"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        parameters = new HttpParameterCollection
        {
          { new HttpFile("file1", "123.png", "image/png", fs) },
          { "dd", 20 },
          { "id", 13 },
          { "key", "e91268b7-1162-4044-9236-7191c8c3b5d2" },
          { "contentType", "text/plain" }
        };

        result = OAuthUtility.ExecuteRequest("POST", "http://api.foxtools.ru/v2/Http", parameters);

        Assert.True(result.ToString().Contains("123.png"));
      }

      result = OAuthUtility.Get("https://github.com/alekseynemiro/nemiro.oauth.dll/archive/master.zip");

      Assert.True(result.IsFile);
      Assert.Contains("master.zip", result.FileName);
    }

  }
}