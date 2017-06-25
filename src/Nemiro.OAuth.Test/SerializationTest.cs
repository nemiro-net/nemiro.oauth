using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Xunit;

namespace Nemiro.OAuth.Test
{

  public class SerializationTest
  {

    [Fact]
    public void UniValueTest()
    {
      // Serialize
      var value = UniValue.Create(123);
      var bf = new BinaryFormatter();
      var m = new MemoryStream();

      bf.Serialize(m, value);

      // Deserialize
      m.Position = 0;

      var value2 = (UniValue)bf.Deserialize(m);

      Assert.Equal(value2, value);

      var value3 = UniValue.ParseXml("<items><item>1</item><item>2</item><item>3</item></items>");//UniValue.Create(new { id = 123, a = new HttpUrlParameter("test", null), text = "hello", list = new object[] { 1, 2, 3, 4, 5, 6, 7 } });

      var bf2 = new BinaryFormatter();
      var m2 = new MemoryStream();

      bf.Serialize(m2, value3.First());

      // Deserialize
      m2.Position = 0;

      var value4 = (UniValue)bf.Deserialize(m2);

      Assert.Equal(value4.ToString(), value3.First().ToString());

      value3 = UniValue.ParseXml("<items><item>1</item><item>2</item><item>3</item></items>");//UniValue.Create(new { id = 123, a = new HttpUrlParameter("test", null), text = "hello", list = new object[] { 1, 2, 3, 4, 5, 6, 7 } });

      bf2 = new BinaryFormatter();
      m2 = new MemoryStream();

      bf.Serialize(m2, value3);

      // Deserialize
      m2.Position = 0;

      value4 = (UniValue)bf.Deserialize(m2);

      Assert.Equal(value4.ToString(), value3.ToString());

      value3 = UniValue.Create(new { id = 123, a = new HttpUrlParameter("test", null), text = "hello", list = new object[] { 1, 2, 3, 4, 5, 6, 7 } });

      m2 = new MemoryStream();

      bf.Serialize(m2, value3);

      m2.Position = 0;

      value4 = (UniValue)bf.Deserialize(m2);

      Assert.Equal(value4.ToString(), value3.ToString());
    }

    [Fact]
    public void RequestResultTest()
    {
      // Serialize
      var r = new RequestResult
      (
        contentType: "text/xml",
        httpHeaders: new NameValueCollection { { "X-Test", "123" } },
        statusCode: 200,
        source: Encoding.UTF8.GetBytes("<items><item>1</item><item>2</item><item>3</item></items>")
      );

      var bf = new BinaryFormatter();
      var m = new MemoryStream();

      bf.Serialize(m, r);

      // Deserialize
      m.Position = 0;

      var r2 = (RequestResult)bf.Deserialize(m);

      Assert.Equal(r2.ContentType, "text/xml");
      Assert.Equal(r2.StatusCode, 200);
      Assert.Equal(r2.HttpHeaders["X-Test"], "123");
      Assert.Equal(r2.Source.Length, r.Source.Length);
      Assert.Equal((int)r2["items"]["item"][0], 1);
    }

    [Fact]
    public void RequestExceptionTest()
    {
      // Serialize
      var e = new RequestException
      (
        contentType: "text/plain",
        headers: new NameValueCollection { { "X-Test", "123" } },
        statusCode: 200
      );

      var bf = new BinaryFormatter();
      var m = new MemoryStream();

      bf.Serialize(m, e);

      // Deserialize
      m.Position = 0;

      var e2 = (RequestException)bf.Deserialize(m);

      Assert.Equal(e2.ContentType, "text/plain");
      Assert.Equal(e2.StatusCode, 200);
      Assert.Equal(e2.HttpHeaders["X-Test"], "123");
    }

  }

}