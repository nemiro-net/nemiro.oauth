using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nemiro.OAuth;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Specialized;

namespace TestProject1
{

  [TestClass]
  public class SerializationTest
  {

    [TestMethod]
    public void Serialization_UniValue()
    {
      Console.WriteLine("Test 1 : Single");

      // Serialize
      var value = UniValue.Create(123);

      var bf = new BinaryFormatter();
      var m = new MemoryStream();
      bf.Serialize(m, value);

      // Deserialize
      m.Position = 0;
      var value2 = (UniValue)bf.Deserialize(m);
      Console.WriteLine(value);
      Console.WriteLine(value2);
      if (value2 == value)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }

      Console.WriteLine();
      Console.WriteLine("-------------------------------------");
      Console.WriteLine();

      Console.WriteLine("Test 2 : Collection");

      var value3 = UniValue.ParseXml("<items><item>1</item><item>2</item><item>3</item></items>");//UniValue.Create(new { id = 123, a = new HttpUrlParameter("test", null), text = "hello", list = new object[] { 1, 2, 3, 4, 5, 6, 7 } });

      var bf2 = new BinaryFormatter();
      var m2 = new MemoryStream();
      bf.Serialize(m2, value3.First());

      // Deserialize
      m2.Position = 0;
      var value4 = (UniValue)bf.Deserialize(m2);
      Console.WriteLine(value3.First());
      Console.WriteLine(value4);
      if (value4.ToString() == value3.First().ToString())
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }

      Console.WriteLine();
      Console.WriteLine("-------------------------------------");
      Console.WriteLine();

      Console.WriteLine("Test 3 : Collection #2");

      value3 = UniValue.ParseXml("<items><item>1</item><item>2</item><item>3</item></items>");//UniValue.Create(new { id = 123, a = new HttpUrlParameter("test", null), text = "hello", list = new object[] { 1, 2, 3, 4, 5, 6, 7 } });

      bf2 = new BinaryFormatter();
      m2 = new MemoryStream();
      bf.Serialize(m2, value3);

      // Deserialize
      m2.Position = 0;
      value4 = (UniValue)bf.Deserialize(m2);
      Console.WriteLine(value3);
      Console.WriteLine(value4);
      if (value4.ToString() == value3.ToString())
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }

      Console.WriteLine();
      Console.WriteLine("-------------------------------------");
      Console.WriteLine();

      Console.WriteLine("Test 4 : Collection #3");

      value3 = UniValue.Create(new { id = 123, a = new HttpUrlParameter("test", null), text = "hello", list = new object[] { 1, 2, 3, 4, 5, 6, 7 } });

      m2 = new MemoryStream();
      bf.Serialize(m2, value3);

      // Deserialize
      m2.Position = 0;
      value4 = (UniValue)bf.Deserialize(m2);
      Console.WriteLine(value3);
      Console.WriteLine(value4);
      if (value4.ToString() == value3.ToString())
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
    }

    [TestMethod]
    public void Serialization_RequestResult()
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
      if (r2.ContentType == "text/xml" && r2.StatusCode == 200 && r2.HttpHeaders["X-Test"] == "123" && r2.Source.Length == r.Source.Length && r2["items"]["item"][0] == 1)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
    }

    [TestMethod]
    public void Serialization_RequestException()
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
      if (e2.ContentType == "text/plain" && e2.StatusCode == 200 && e2.HttpHeaders["X-Test"] == "123")
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
    }

  }

}
