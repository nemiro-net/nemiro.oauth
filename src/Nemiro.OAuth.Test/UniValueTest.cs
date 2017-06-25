using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Xunit;

namespace Nemiro.OAuth.Test
{

  public class UniValueTest
  {

    [Fact]
    public void Common()
    {
      var test = new Dictionary<string, UniValue>();
      var now = DateTime.Now;

      test.Add("a", 123);
      test.Add("b", "test");
      test.Add("c", now);
      test.Add("d", 4.2);
      test.Add("e", new byte[] { 1, 2, 3 });
      test.Add("f", UniValue.Create(new Uri("http://localhost")));
      test.Add("g", UniValue.Create(new { id = 123, name = "tester", date = now, obj = new { a = 1, b = 2, c = 3, d = new int[] { 1, 2, 3 } } }));
      test.Add("h", new int[] { 1, 2, 3 });
      test.Add("i", new string[] { "a", "b", "c" });
      test.Add("j", new StringBuilder("xyz"));

      Assert.Equal("123", test["a"].ToString());
      Assert.Equal("test", test["b"].ToString());
      Assert.Equal(now, (DateTime)test["c"]);
      Assert.Equal(4.2, (double)test["d"]);
      Assert.Equal("http://localhost", test["f"]["OriginalString"]);
      Assert.Equal(123, test["g"]["id"]);
      Assert.Equal(now, test["g"]["date"]);
      Assert.Equal(1, test["g"]["obj"]["a"]);
      Assert.Equal(2, test["g"]["obj"]["b"]);
      Assert.Equal(3, test["g"]["obj"]["c"]);
      Assert.Equal(1, test["h"][0]);
      Assert.Equal(2, test["h"][1]);
      Assert.Equal(3, test["h"][2]);
      Assert.Equal("a", test["i"][0]);
      Assert.Equal("b", test["i"][1]);
      Assert.Equal("c", test["i"][2]);
      Assert.Equal("xyz", test["j"].ToString());
      Assert.True(test["j"].Equals("xyz"));
      // TODO: Assert.Equal("xyz", test["j"]);

      UniValue r2 = "world!";
      string ttt = "Hello, " + r2;

      Assert.True(r2.IsString);
      Assert.Equal("Hello, world!", ttt);

      r2 = 123;
      Assert.Equal(123, (int)r2);
      Assert.Equal(123, Convert.ToInt32(r2));

      r2 = "ff";
      Assert.Equal(255, Convert.ToInt32(r2.ToString(), 16));

      r2 = "123";
      Assert.Equal(123, (int)r2);

      r2 = 123.123;
      Assert.Equal(123, (int)r2);
      
      r2 = "123.123";
      Assert.Equal(123, (int)r2);

      r2 = "123,123";
      Assert.Equal(123, (int)r2);

      r2 = "abc";

#if NET35
      Assert.Throws<FormatException>(() => (int)r2);
#elif NET40
      Assert.Throws<FormatException>(() => (int)r2);
#else
      Assert.ThrowsAny<FormatException>(() => (int)r2);
#endif

      r2 = 123;
      Assert.Equal(123, (double)r2);

      r2 = "123";
      Assert.Equal(123, (double)r2);

      r2 = "123.123";
      Assert.Equal(123.123, (double)r2);

      r2 = "123,123";
      Assert.Equal(123.123, (double)r2);

      r2 = "abc";

#if NET35
      Assert.Throws<FormatException>(() => (double)r2);
#elif NET40
      Assert.Throws<FormatException>(() => (double)r2);
#else
      Assert.ThrowsAny<FormatException>(() => (double)r2);
#endif

      r2 = "Thu Dec 04, 2014";
      Assert.Equal(new DateTime(2014, 12, 4), Convert.ToDateTime(r2));
    }

    [Fact]
    public void Json()
    {

#if NET35
      Assert.Throws<ArgumentException>(() => UniValue.ParseJson("test,1,#2,$3"));
#elif NET40
      Assert.Throws<ArgumentException>(() => UniValue.ParseJson("test,1,#2,$3"));
#else
      Assert.ThrowsAny<ArgumentException>(() => UniValue.ParseJson("test,1,#2,$3"));
#endif

      UniValue r = UniValue.Empty;

      Assert.False(UniValue.TryParseJson("test,1,#2,$3", out r));
      Assert.True(UniValue.TryParseJson("[1,2,3]", out r));

      int i = 0;
      int j = 0;

      var testValue = new string[] { "1", "2", "3" };
      var testValue2 = new string[] { "test", "test2", "test3" };
      var testValue3 = new string[] { "1", "2" };
      var testValue4 = new string[] { "hi", "hello" };

      UniValue value = UniValue.ParseJson("{data: [{id: 1, name: 'test'}, {id: 2, name: 'test2'}, {id: 3, name: 'test3', list: [{x:1,y:'hi'}, {x:2, y:'hello'}] }] }");

      foreach (UniValue itm in value["data"])
      {
        if (itm["list"].HasValue)
        {
          foreach (UniValue itm2 in itm["list"])
          {
           Assert.True(testValue3[j] == itm2["x"] && testValue4[j] == itm2["y"]);

            j++;
          }
        }
        
        Assert.True(testValue[i] == itm["id"] && testValue2[i] == itm["name"]);

        i++;
      }

      value = UniValue.ParseJson
      (
        @"{
            array: [1, 2, 3, 4, 5, 6, 7, 8, 9],
            array2: ['a', 'b', 1, 2, 3],
            data: 
            [
              {id: 1, name: 'test'}, 
              {id: 2, name: 'test2'}, 
              {
                id: 3, name: 'test3', list: 
                [
                  {x:1,y:'hi'}, 
                  {x:2, y:'hello'},
                  {
                    x:3, y:'test', 
                    array: [1,2,3], 
                    sub: 
                    [
                      {id: 123, text: 'tesssst'},
                      {id: 124, text: 'xyz'},
                      {id: 125, text: 'aaaa', list: [{sid: 1}, {sid: 2}]},
                      {id: 127, text: 'zyx', arr: ['a', 'b', 'c']}
                    ]
                  }
                ] 
              }
            ] 
          }"
      );

      var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

      i = j = 0;

      foreach (UniValue itm in value["array"])
      {
        Assert.True(itm == array[i]);
        Assert.True(itm.Equals(array[i]));
        Assert.Equal(array[i], (int)itm);

        i++;
      }

      var array2 = new object[] { "a", "b", 1, 2, 3 };

      i = j = 0;

      foreach (UniValue itm in value["array2"])
      {
        Assert.True(itm.Equals(array2[i]));
        Assert.Equal(array2[i].ToString(), itm.ToString());

        i++;
      }

      var names = new string[] { "test", "test2", "test3" };

      i = j = 0;

      foreach (UniValue itm in value["data"])
      {
        Assert.Equal(names[i], itm["name"].ToString());

        if (itm["list"].HasValue)
        {
          foreach (UniValue itm2 in itm["list"])
          {
            if (itm2["array"].HasValue)
            {
              j = 0;

              foreach (UniValue itm3 in itm2["array"])
              {
                Assert.Equal<int>(j + 1, (int)itm3);
                Assert.True(itm3 == j + 1);
                Assert.True(itm3.Equals(j + 1));

                j++;
              }
            }

            if (itm2["sub"].HasValue)
            {
              foreach (UniValue itm3 in itm2["sub"])
              {
                if (itm3["arr"].HasValue)
                {
                  var arr = new string[] { "a", "b", "c" };

                  j = 0;

                  foreach (UniValue itm4 in itm3["arr"])
                  {
                    Assert.Equal(arr[j], itm4);
                    Assert.True(itm4 == arr[j]);
                    Assert.True(itm4.Equals(arr[j]));

                    j++;
                  }
                }

                if (itm3["list"].HasValue)
                {
                  j = 0;

                  foreach (UniValue itm4 in itm3["list"])
                  {
                    Assert.Equal(j + 1, (int)itm4["sid"]);
                    Assert.True(itm4["sid"] == j + 1);
                    Assert.True(itm4["sid"].Equals(j + 1));

                    j++;
                  }
                }
              }
            }
          }
        }

        i++;
      }
    }

    [Fact]
    public void Xml()
    {
#if NET35
      Assert.Throws<XmlException>(() => UniValue.ParseXml("test,1,#2,$3"));
#elif NET40
      Assert.Throws<XmlException>(() => UniValue.ParseXml("test,1,#2,$3"));
#else
      Assert.ThrowsAny<XmlException>(() => UniValue.ParseXml("test,1,#2,$3"));
#endif

      UniValue r = UniValue.Empty;

      Assert.False(UniValue.TryParseXml("test,1,#2,$3", out r));
      Assert.True(UniValue.TryParseXml("<items><item>1</item><item>2</item><item>3</item></items>", out r));

      var testValue0 = new string[] { "1", "2", "3" };

      int j = 0;

      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");

      foreach (UniValue itm in value["response"]["item"])
      {
        Assert.Equal(itm, testValue0[j]);
        Assert.True(itm == testValue0[j]);
        Assert.True(itm.Equals(testValue0[j]));

        j++;
      }

      value = UniValue.ParseXml("<response><item><id>1</id><name>test</name></item><item><id>2</id><name>test2</name></item><item><id>3</id><name>test3</name></item></response>");

      var testValue = new string[] { "1", "2", "3" };
      var testValue2 = new string[] { "test", "test2", "test3" };

      for (int i = 0; i <= value["response"]["item"].Count - 1; i++)
      {
        var itm = value["response"]["item"][i];

        Assert.True((testValue[i] == itm["id"]) && (itm["name"] == testValue2[i]));
      }

      value = UniValue.ParseXml("<response><item type=\"4\">1</item><item type=\"5\">2</item><item type=\"6\">3</item></response>");

      testValue = new string[] { "4", "5", "6" };
      testValue2 = new string[] { "1", "2", "3" };

      j = 0;

      foreach (UniValue itm in value["response"]["item"])
      {
        Assert.True((testValue[j] == itm["@type"]) && (itm == testValue2[j]));

        j++;
      }

      testValue = new string[] { "text", "date", "num" };
      testValue2 = new string[] { "1", "2", "3" };

      var testValue3 = new string[] { "10", "11", "12" };
      var testValue4 = new string[] { "normal", null, "bold" };
      var testValue5 = new string[] { "test", "test2", "test3" };

      j = 0;

      value = UniValue.ParseXml
      (
        @"<response>
            <item type=""text""><id>1</id><name case=""10"" style=""normal"">test</name></item>
            <item type=""date""><id>2</id><name case=""11"">test2</name></item>
            <item type=""num""><id>3</id><name case=""12"" style=""bold"">test3</name></item>
          </response>"
      );

      foreach (UniValue itm in value["response"]["item"])
      {
        Assert.True((testValue[j] == itm["@type"]) && (itm["id"] == testValue2[j]) && (itm["name"]["@case"] == testValue3[j]) && (testValue4[j] == itm["name"]["@style"]) && (testValue5[j] == itm["name"]));

        j++;
      }

      testValue = new string[] { "text", "date", "num" };
      testValue2 = new string[] { "1", "2", "num" };

      j = 0;

      value = UniValue.ParseXml
      (
        @"<response>
            <item id=""text""><id>1</id></item>
            <item id=""date""><id>2</id></item>
            <item id=""num"" />
          </response>"
      );

      foreach (UniValue itm in value["response"]["item"])
      {
        Assert.True((testValue[j] == itm["@id"]) && (itm["id"] == testValue2[j]));

        j++;
      }
    }

    [Fact]
    public void ParseParameters()
    {
#if NET35
      Assert.Throws<InvalidDataException>(() => UniValue.ParseParameters("test,1,#2,$3\r\n"));
#elif NET40
      Assert.Throws<InvalidDataException>(() => UniValue.ParseParameters("test,1,#2,$3\r\n"));
#else
      Assert.ThrowsAny<InvalidDataException>(() => UniValue.ParseParameters("test,1,#2,$3\r\n"));
#endif

      UniValue r = UniValue.Empty;

      Assert.False(UniValue.TryParseParameters("test,1,#2,$3\r\n", out r));
      Assert.True(UniValue.TryParseParameters("a=1&b=2&c=3", out r));
    }

    [Fact]
    public void Index()
    {
      UniValue r = UniValue.Empty;

      Assert.True(r["test"] == null);
      Assert.True(r[10] == null);
      Assert.True(r["test"][12] == null);
      Assert.True(r["test"].First() == r["test"][12]);
      Assert.Equal(r["test"].First(), r["test"][12]);
      Assert.True(r["xyz"][3]["qwer"][1]["abc"] == null);

      r = UniValue.ParseJson("[1, 2, 3]");

      for (int i = 0; i < 5; i++)
      {
        if (i + 1 <= 3)
        {
          Assert.True(r[i] == i + 1);
        }
        else
        {
          Assert.True(r[i] == null);
        }
      }

      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");

      Assert.True(value[0].Key == "response");
      Assert.Equal("response", value[0].Key);

      Assert.True(value["response"].Key == "response");
      Assert.Equal("response", value["response"].Key);

      Assert.True(value[0][0].Key == "item");
      Assert.Equal("item", value[0][0].Key);

      Assert.True(value["response"][0].Key == "item");
      Assert.Equal("item", value["response"][0].Key);

      Assert.True(value["response"]["item"][2] == 3);
      Assert.Equal(3, (int)value["response"]["item"][2]);

      Assert.True(value[0][0][1] == 2);
      Assert.Equal(2, (int)value[0][0][1]);

      r = UniValue.Empty;

      r["test"] = "123";

      Assert.Equal("123", r["test"]);

      r = UniValue.Empty;

      r["test"]["a"] = "abc";

      Assert.Equal("abc", r["test"]["a"]);

      r = UniValue.Empty;
      r["a"][0]["test"][2] = "aaa";

      Assert.Equal("aaa", r["a"][0]["test"][2]);

      r = UniValue.Empty;
      r.Add("test", "123");

      Assert.Equal("123", r["test"]);
      
      r["test"].Add("a", "abc");

      Assert.Equal("abc", r["test"]["a"]);

      r[0][1][2][3][4][5][6][7]["a"]["b"]["c"] = 123;

      Assert.Equal(123, (int)r[0][1][2][3][4][5][6][7]["a"]["b"]["c"]);
    }

    [Fact]
    public void Clone()
    {
      UniValue v = UniValue.Create("test");
      UniValue v2 = v.Clone() as UniValue;
      v2 = "123";

#if !NET35 && !NET40
      Assert.NotEqual(v2, v);
#endif

      Assert.False(v2.Equals(v));

      var o = new { id = 123, text = "test" };

      v = UniValue.Create(o);
      v2 = v.Clone() as UniValue;
      v2 = "test";

#if !NET35 && !NET40
      Assert.NotEqual(v2, v);
#endif

      Assert.False(v2.Equals(v));

      v = UniValue.Create(o);
      v2 = v;
      v["id"] = "555";

#if !NET35 && !NET40
      Assert.Equal(v, v2);
#endif

      Assert.True(v2.Equals(v));
    }

    [Fact]
    public void IsNullOrEmpty()
    {
      UniValue v = null;

      Assert.True(UniValue.IsNullOrEmpty(v));
      
      v = "123";

      Assert.False(UniValue.IsNullOrEmpty(v));
      Assert.False(UniValue.IsNullOrEmpty(1));
      Assert.False(UniValue.IsNullOrEmpty("test"));
      Assert.True(UniValue.IsNullOrEmpty(UniValue.Empty));
      Assert.False(UniValue.IsNullOrEmpty(UniValue.Create(DateTime.Now)));
      Assert.True(UniValue.IsNullOrEmpty(UniValue.Create()));
    }

    [Fact]
    public void Parents()
    {
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");

      Assert.Equal("root", value["response"].Parent.Key);

      // ???

#if NET35 || NET40
      Assert.True(value["response"]["item"].Parent.Equals(value["response"]));
#else
      Assert.Equal(value["response"]["item"].Parent, value["response"]);
#endif

      value = UniValue.ParseXml("<response><item><id>1</id></item><item><id>2</id></item><item><id>3</id></item></response>");

#if NET35 || NET40
      Assert.True(value["response"]["item"][0]["id"].Parent.Equals(value["response"]["item"][0]));
      Assert.True(value["response"]["item"][0].Parent.Equals(value["response"]["item"]));
#else
      Assert.Equal(value["response"]["item"][0]["id"].Parent, value["response"]["item"][0]);
      Assert.Equal(value["response"]["item"][0].Parent, value["response"]["item"]);
#endif

      value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");

#if NET35 || NET40
      Assert.True(value["response"]["item"].First().Parent.Equals(value["response"]["item"]));
      Assert.True(value["response"]["item"].Last().Parent.Equals(value["response"]["item"]));
      Assert.True(value["response"]["item"]["0"].Parent.Equals(value["response"]["item"]));
      Assert.True(value["response"]["item"][1].Parent.Equals(value["response"]["item"]));
      Assert.True(value["response"][0][1].Parent.Equals(value["response"]["item"]));
      Assert.True(value["response"]["item"][2]["test"].Parent.Key.Equals(value["response"]["item"][2].Key));
      Assert.True(value["response"]["item"][2]["test"].Parent.Equals(value["response"]["item"][2]));
      Assert.True(value["response"]["item"][2]["test"]["a"].Parent.Key.Equals(value["response"]["item"][2]["test"].Key));
      Assert.True(value["response"]["item"][2][5].Parent.Equals(value["response"]["item"][2]));
#else
      Assert.Equal(value["response"]["item"].First().Parent, value["response"]["item"]);
      Assert.Equal(value["response"]["item"].Last().Parent, value["response"]["item"]);
      Assert.Equal(value["response"]["item"]["0"].Parent, value["response"]["item"]);
      Assert.Equal(value["response"]["item"][1].Parent, value["response"]["item"]);
      Assert.Equal(value["response"][0][1].Parent, value["response"]["item"]);
      Assert.Equal(value["response"]["item"][2]["test"].Parent.Key, value["response"]["item"][2].Key);
      Assert.Equal(value["response"]["item"][2]["test"].Parent, value["response"]["item"][2]);
      Assert.Equal(value["response"]["item"][2]["test"]["a"].Parent.Key, value["response"]["item"][2]["test"].Key);
      Assert.Equal(value["response"]["item"][2][5].Parent, value["response"]["item"][2]);
#endif

      value = UniValue.ParseJson("{data: [{id: 1, name: 'test'}, {id: 2, name: 'test2'}, {id: 3, name: 'test3', list: [{x:1,y:'hi'}, {x:2, y:'hello'}] }] }");


#if NET35 || NET40
      Assert.True("root".Equals(value["data"].Parent.Key));
      Assert.True(value["data"][0].Parent.Equals(value["data"]));
      Assert.True(value["data"][1]["name"].Parent.Equals(value["data"][1]));
      Assert.True(value["data"][0][1].Parent.Equals(value["data"][0]));
#else
      Assert.Equal("root", value["data"].Parent.Key);
      Assert.Equal(value["data"][0].Parent, value["data"]);
      Assert.Equal(value["data"][1]["name"].Parent, value["data"][1]);
      Assert.Equal(value["data"][0][1].Parent, value["data"][0]);
#endif
    }

    [Fact]
    public void ToStringTest()
    {
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");

      Assert.Equal("{ \"response\": { \"item\": [\"1\", \"2\", \"3\"] } }", value.ToString());

      value["response"]["item"]["test"] = "123";

      Assert.Equal("{ \"response\": { \"item\": { \"0\": \"1\", \"1\": \"2\", \"2\": \"3\", \"test\": \"123\" } } }", value.ToString());

      value = UniValue.ParseJson("{\"data\": [{\"id\": 1, \"name\": \"test\"}, {\"id\": 2, \"name\": \"test2\"}, {\"id\": 3, \"name\": \"test3\", \"list\": [{\"x\":1,\"y\":\"hi\"}, {\"x\":2, \"y\":\"hello\"}] }] }");

      Assert.Equal("{ \"data\": [{ \"id\": 1, \"name\": \"test\" }, { \"id\": 2, \"name\": \"test2\" }, { \"id\": 3, \"name\": \"test3\", \"list\": [{ \"x\": 1, \"y\": \"hi\" }, { \"x\": 2, \"y\": \"hello\" }] }] }", value.ToString());
      Assert.Equal("[{ \"id\": 1, \"name\": \"test\" }, { \"id\": 2, \"name\": \"test2\" }, { \"id\": 3, \"name\": \"test3\", \"list\": [{ \"x\": 1, \"y\": \"hi\" }, { \"x\": 2, \"y\": \"hello\" }] }]", value["data"].ToString());
      Assert.Equal("test2", value["data"][1]["name"].ToString());

      value = UniValue.ParseXml("<response><item id='1' style='bold' color='white' /><item id='2' style='italic' color='red'>123</item></response>");

      Assert.True(value == "{ \"response\": { \"item\": [{ \"@id\": \"1\", \"@style\": \"bold\", \"@color\": \"white\" }, { \"value\": \"123\", \"@id\": \"2\", \"@style\": \"italic\", \"@color\": \"red\" }] } }");
      Assert.True(value["response"] == "{ \"item\": [{ \"@id\": \"1\", \"@style\": \"bold\", \"@color\": \"white\" }, { \"value\": \"123\", \"@id\": \"2\", \"@style\": \"italic\", \"@color\": \"red\" }] }");
      Assert.True(value["response"]["item"] == "[{ \"@id\": \"1\", \"@style\": \"bold\", \"@color\": \"white\" }, { \"value\": \"123\", \"@id\": \"2\", \"@style\": \"italic\", \"@color\": \"red\" }]");
      Assert.True(String.IsNullOrEmpty(value["response"]["item"][0].ToString()));
      Assert.True(value["response"]["item"][0].ToString() == "");
      Assert.False(UniValue.IsNullOrEmpty(value["response"]["item"][0]));
      Assert.True(value["response"]["item"][1] == "123");
      Assert.True(value["response"]["item"][1] == 123);
      Assert.True(value["response"]["item"][0]["@id"] == 1);
      Assert.True(value["response"]["item"][0]["style"] == "bold");

      value = UniValue.ParseXml("<response><item id='1' style='bold' color='white'><vvalue abc='test' avalue='1234567'></vvalue></item><item id='2' style='italic' color='red'><v>456</v></item></response>");

      Assert.Equal("{ \"response\": { \"item\": [{ \"value\": { \"vvalue\": { \"@abc\": \"test\", \"@avalue\": \"1234567\" } }, \"@id\": \"1\", \"@style\": \"bold\", \"@color\": \"white\" }, { \"value\": { \"v\": \"456\" }, \"@id\": \"2\", \"@style\": \"italic\", \"@color\": \"red\" }] } }", value.ToString());
      Assert.Equal("[{ \"value\": { \"vvalue\": { \"@abc\": \"test\", \"@avalue\": \"1234567\" } }, \"@id\": \"1\", \"@style\": \"bold\", \"@color\": \"white\" }, { \"value\": { \"v\": \"456\" }, \"@id\": \"2\", \"@style\": \"italic\", \"@color\": \"red\" }]", value["response"]["item"].ToString());
      Assert.Equal("{ \"vvalue\": { \"@abc\": \"test\", \"@avalue\": \"1234567\" } }", value["response"]["item"][0].ToString());

      value = UniValue.ParseJson("{data: [{id: 1, name: null}, {id: 2, name: 'test2\r\n123'}, {id: 3, name: 'test3', list: [{x:1,y:'hi'}, {x:2, y:'hello'}] }] }");

      Assert.Equal("{ \"data\": [{ \"id\": 1, \"name\": null }, { \"id\": 2, \"name\": \"test2\\r\\n123\" }, { \"id\": 3, \"name\": \"test3\", \"list\": [{ \"x\": 1, \"y\": \"hi\" }, { \"x\": 2, \"y\": \"hello\" }] }] }", value.ToString());

      value = "test\r\ntest";

      Assert.Equal("test\r\ntest", value.ToString());
    }

    [Fact]
    public void ConvertType()
    {
      UniValue ok = "OK";
      Assert.Equal("OK", ok);

      var l = new List<UniValue>();

      l.Add((byte)0);
      l.Add((sbyte)1);
      l.Add((bool)true);
      l.Add((short)123);
      l.Add((int)123);
      l.Add((long)123);
      l.Add((ushort)123);
      l.Add((uint)123);
      l.Add((ulong)123);
      l.Add((double)123.22);
      l.Add((decimal)123.33);
      l.Add((float)123.55);

      foreach (UniValue item in l)
      {
        var r = (int)item;
      }

      foreach (UniValue item in l)
      {
        var r = (double)item;
      }

      foreach (UniValue item in l)
      {
        string r = item.ToString();
      }

      foreach (var item in l)
      {
        var r = (bool)item;
      }

      UniValue value = (byte)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (sbyte)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (Int16)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (Int32)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (Int64)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (UInt16)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (UInt32)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (UInt64)0;

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (Double)0;

      Convert.ToByte(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (Decimal)0;

      Convert.ToByte(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = (Single)0;

      Convert.ToByte(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = '0';

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      value = "0";

      Convert.ToByte(value);
      Convert.ToChar(value);
      Convert.ToDecimal(value);
      Convert.ToDouble(value);
      Convert.ToInt16(value);
      Convert.ToInt32(value);
      Convert.ToInt64(value);
      Convert.ToSByte(value);
      Convert.ToSingle(value);
      Convert.ToString(value);
      Convert.ToUInt16(value);
      Convert.ToUInt32(value);
      Convert.ToUInt64(value);

      Convert.ChangeType(value, typeof(int));
      Convert.ChangeType(value, typeof(double));

      value = "true";

      Convert.ToBoolean(value);
      Convert.ChangeType(value, typeof(bool));
    }

    [Fact]
    public void Enumerator()
    {
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");

      foreach (UniValue itm in value.First()["item"])
      {
        Assert.True(itm == "1" || itm == 2 || itm == "3");
      }

      value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");

      foreach (UniValue itm in value["response"]["item"].First())
      {
        Assert.Equal("1", itm.ToString());
      }
    }

    [Fact]
    public void Compatibility()
    {
      int i = 0;
      var uid = new int[] { 489, 546, 845 };
      var firstName = new string[] { "Aleksandr", "Oksana", "Sergey" };

      UniValue r = UniValue.ParseJson("{ response: [{ uid: 489, first_name: 'Aleksandr', last_name: '', nickname: '', online: 1, user_id: 484, lists: [1] }, { uid: 546, first_name: 'Oksana', last_name: '', deactivated: 'deleted', online: 0, user_id: 546 }, { uid: 845, first_name: 'Sergey', last_name: '', nickname: '', online: 0, user_id: 845 }] }");
      
      foreach (UniValue itm in (Array)r["response"])
      {
        Assert.Equal(uid[i], itm["uid"]);
        Assert.Equal(firstName[i], itm["first_name"]);

        i++;
      }

      var bytes = new byte[] { 1, 2, 3, 4, 5 };

      r = UniValue.Create(bytes);

      Assert.Equal(bytes, (byte[])r);
    }

    [Fact]
    public void UniValueCollection()
    {
      var c = new UniValueCollection();

      c.Add("test", "123");

      Assert.True(c.ContainsKey("test"));

      var k = new KeyValuePair<string, UniValue>("test2", "234");

      c.Add(k);

      Assert.True(c.ContainsKey("test2"));
      Assert.True(c.Contains(k));
      Assert.False(c.ContainsKey("abc"));
      Assert.True(c.Remove("test"));
      Assert.False(c.ContainsKey("test"));
      Assert.True(c.Remove(k));
      Assert.False(c.ContainsKey("test2"));

      c.Add("test", "123");
      c.Add("test", "234");
      c.Add("test", "345");

      Assert.True(c["test"].IsCollection);
      Assert.Equal("{ \"test\": [\"123\", \"234\", \"345\"] }", c.ToString());
      Assert.Equal("[\"123\", \"234\", \"345\"]", c["test"].ToString());
      Assert.False(c.Count != 1);
      Assert.False(c["test"].Count != 3);
      Assert.False(c.Keys.Count != 1);
      Assert.False(c.Values.Count != 1);

      c.Clear();

      Assert.False(c.Count > 0);

      for (int i = 0; i <= 9; i++)
      {
        c.Add(String.Format("test-{0}", i), Guid.NewGuid().ToString());
      }

      Assert.True(c.Count == 10);

      for (int i = 0; i <= 9; i++)
      {
        Assert.True(c[String.Format("test-{0}", i)] != null);
      }
    }

  }

}