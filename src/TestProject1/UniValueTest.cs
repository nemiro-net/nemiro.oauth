using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nemiro.OAuth;

namespace TestProject1
{

  [TestClass]
  public class UniValueTest
  {

    [TestMethod]
    public void UniValue_Common()
    {
      Console.WriteLine("Test 1: Types");
      var test = new Dictionary<string, UniValue>();
      test.Add("a", 123);
      test.Add("b", "test");
      test.Add("c", DateTime.Now);
      test.Add("d", 4.2);
      test.Add("e", new byte[] { 1, 2, 3 });
      test.Add("f", UniValue.Create(new Uri("http://localhost")));
      test.Add("g", UniValue.Create(new { id = 123, name = "tester", date = DateTime.Now, obj = new { a = 1, b = 2, c = 3, d = new int[] { 1, 2, 3 } } }));
      test.Add("h", new int[] { 1, 2, 3 });
      test.Add("i", new string[] { "a", "b", "c" });
      StringBuilder sb = new StringBuilder(); sb.Append("xyz");
      test.Add("j", sb);

      /*ResultValue a = 123;
      ResultValue b = "test";
      ResultValue c = DateTime.Now;
      ResultValue d = 4.2;
      ResultValue e = new byte[] { 1, 2, 3 };
      ResultValue f = new Uri("http://localhost");*/

      foreach (var key in test.Keys)
      {
        Console.WriteLine("{0} = {1}", key, test[key]);
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 2: Parse");

      Console.WriteLine("JSON");
      try
      {
        var r = UniValue.ParseJson("test,1,#2,$3");
        Assert.Fail("Invalid parse: ERROR");
      }
      catch
      {
        Console.WriteLine("Invalid parse: OK");
      }
      try
      {
        UniValue r = UniValue.Empty;
        if (!UniValue.TryParseJson("test,1,#2,$3", out r))
        {
          Console.WriteLine("Try parse #1: OK");
        }
        else
        {
          Assert.Fail("Try parse #1: ERROR");
        }
        if (UniValue.TryParseJson("[1,2,3]", out r))
        {
          Console.WriteLine("Try parse #2: OK / {0}", r);
        }
        else
        {
          Assert.Fail("Try parse #2: ERROR");
        }
      }
      catch
      {
        Assert.Fail("Try parse: ERROR");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("XML");
      try
      {
        var r = UniValue.ParseXml("test,1,#2,$3");
        Assert.Fail("Invalid parse: ERROR");
      }
      catch
      {
        Console.WriteLine("Invalid parse: OK");
      }
      try
      {
        UniValue r = UniValue.Empty;
        if (!UniValue.TryParseXml("test,1,#2,$3", out r))
        {
          Console.WriteLine("Try parse #1: OK");
        }
        else
        {
          Assert.Fail("Try parse #1: ERROR");
        }
        if (UniValue.TryParseXml("<items><item>1</item><item>2</item><item>3</item></items>", out r))
        {
          Console.WriteLine("Try parse #2: OK / {0}", r);
        }
        else
        {
          Assert.Fail("Try parse #2: ERROR");
        }
      }
      catch
      {
        Assert.Fail("Try parse: ERROR");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("PARAMETERS");
      try
      {
        var r = UniValue.ParseParameters("test,1,#2,$3\r\n");
        Assert.Fail("Invalid parse: ERROR");
      }
      catch
      {
        Console.WriteLine("Invalid parse: OK");
      }
      try
      {
        UniValue r = UniValue.Empty;
        if (!UniValue.TryParseParameters("test,1,#2,$3\r\n", out r))
        {
          Console.WriteLine("Try parse #1: OK");
        }
        else
        {
          Assert.Fail("Try parse #1: ERROR");
        }
        if (UniValue.TryParseParameters("a=1&b=2&c=3", out r))
        {
          Console.WriteLine("Try parse #2: OK / {0}", r);
        }
        else
        {
          Assert.Fail("Try parse #2: ERROR");
        }
      }
      catch
      {
        Assert.Fail("Try parse: ERROR");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 3");

      Console.WriteLine("ToString");

      UniValue r2 = "world!";
      string ttt = "Hello, " + r2;
      Console.WriteLine("IsString: {0}", r2.IsString);
      if (!r2.IsString)
      {
        Assert.Fail();
      }
      Console.WriteLine(ttt);
      Console.WriteLine("OK");

      Console.WriteLine("ToInt32");

      r2 = 123;
      int num = (int)r2;
      num = Convert.ToInt32(r2);

      r2 = "ff";
      num = Convert.ToInt32(r2.ToString(), 16);

      r2 = "123";
      num = (int)r2;

      r2 = 123.123;
      num = (int)r2;
      
      r2 = "123.123";
      num = (int)r2;

      r2 = "123,123";
      num = (int)r2;

      try
      {
        r2 = "abc";
        num = (int)r2;
        Assert.Fail();
      }
      catch
      {
      }
      Console.WriteLine("OK");

      Console.WriteLine("ToDouble");
      r2 = 123;
      double num2 = (double)r2;
      r2 = "123";
      num2 = (double)r2;

      r2 = "123.123";
      num2 = (double)r2;

      r2 = "123,123";
      num2 = (double)r2;

      try
      {
        r2 = "abc";
        num2 = (double)r2;
        Assert.Fail();
      }
      catch
      {
      }
      Console.WriteLine("OK");

      Console.WriteLine("ToDateTime");

      r2 = "Thu Dec 04, 2014";
      Convert.ToDateTime(r2);

      Console.WriteLine("OK");

    }

    [TestMethod]
    public void UniValue_Json()
    {
      int i = 0; int j = 0;
      var testValue = new string[] { "1", "2", "3" };
      var testValue2 = new string[] { "test", "test2", "test3" };
      var testValue3 = new string[] { "1", "2" };
      var testValue4 = new string[] { "hi", "hello" };
      Console.WriteLine("Test 1");
      UniValue value = UniValue.ParseJson("{data: [{id: 1, name: 'test'}, {id: 2, name: 'test2'}, {id: 3, name: 'test3', list: [{x:1,y:'hi'}, {x:2, y:'hello'}] }] }");
      foreach (UniValue itm in value["data"])
      {
        Console.WriteLine("-- {0} = {1}", itm["id"], itm["name"]);
        if (itm["list"].HasValue)
        {
          foreach (UniValue itm2 in itm["list"])
          {
            Console.WriteLine("---- {0} = {1}", itm2["x"], itm2["y"]);
            if (testValue3[j] != itm2["x"] && testValue4[j] != itm2["y"])
            {
              Assert.Fail();
            }
            j++;
          }
        }
        else
        {
          Console.WriteLine("---- no list");
        }
        if (testValue[i] != itm["id"] && testValue2[i] != itm["name"])
        {
          Assert.Fail();
        }
        i++;
      }
      Console.WriteLine("-------------------------------------");
      Console.WriteLine("Test 2");
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
      Console.WriteLine("-- Array");
      var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      i = j = 0;
      foreach (UniValue itm in value["array"])
      {
        Console.WriteLine("---- {0}", itm);
        if (array[i] != itm)
        {
          Assert.Fail();
        }
        i++;
      }
      Console.WriteLine("-- Array2");
      var array2 = new object[] { "a", "b", 1, 2, 3 };
      i = j = 0;
      foreach (UniValue itm in value["array2"])
      {
        Console.WriteLine("---- {0}", itm);
        if (!itm.Equals(array2[i]))
        {
          Assert.Fail();
        }
        i++;
      }
      Console.WriteLine("-- Data");
      foreach (UniValue itm in value["data"])
      {
        Console.WriteLine("---- {0} = {1}", itm["id"], itm["name"]);
        if (itm["list"].HasValue)
        {
          foreach (UniValue itm2 in itm["list"])
          {
            Console.WriteLine("------ {0} = {1}", itm2["x"], itm2["y"]);
            if (itm2["array"].HasValue)
            {
              foreach (UniValue itm3 in itm2["array"])
              {
                Console.WriteLine("-------- {0}", itm3);
              }
            }
            if (itm2["sub"].HasValue)
            {
              foreach (UniValue itm3 in itm2["sub"])
              {
                Console.WriteLine("-------- {0} = {1}", itm3["id"], itm3["text"]);
                if (itm3["arr"].HasValue)
                {
                  foreach (UniValue itm4 in itm3["arr"])
                  {
                    Console.WriteLine("---------- {0}", itm4);
                  }
                }
                if (itm3["list"].HasValue)
                {
                  foreach (UniValue itm4 in itm3["list"])
                  {
                    Console.WriteLine("---------- {0}", itm4["sid"]);
                  }
                }
              }
            }
          }
        }
        else
        {
          Console.WriteLine("------ no list");
        }
      }
      Console.WriteLine("-------------------------------------");

    }

    [TestMethod]
    public void UniValue_Xml()
    {
      bool ok = true;
      Console.WriteLine("Test 1: Simple items");
      var testValue0 = new string[] { "1", "2", "3" };
      int j = 0;
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");
      Console.WriteLine(value);
      foreach (UniValue itm in value["response"]["item"])
      {
        Console.WriteLine("-- {0}", itm);
        if (ok)
        {
          ok = itm == testValue0[j];
        }
        j++;
      }
      if (ok)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("-------------------------------------");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");
      ok = true;
      Console.WriteLine("Test 2: Hard items");
      value = UniValue.ParseXml("<response><item><id>1</id><name>test</name></item><item><id>2</id><name>test2</name></item><item><id>3</id><name>test3</name></item></response>");
      Console.WriteLine(value);
      foreach (UniValue itm in value["response"]["item"])
      {
        Console.WriteLine("-- {0} = {1}", itm["id"], itm["name"]);
      }
      var testValue = new string[] { "1", "2", "3" };
      var testValue2 = new string[] { "test", "test2", "test3" };
      for (int i = 0; i <= value["response"]["item"].Count - 1; i++)
      {
        var itm = value["response"]["item"][i];
        //Console.WriteLine("-- {0} = {1}", itm["id"], itm["name"]);
        if (ok)
        {
          ok = (testValue[i] == itm["id"]) && (itm["name"] == testValue2[i]);
        }
      }
      if (ok)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("-------------------------------------");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");
      ok = true;
      Console.WriteLine("Test 3: Attributes");
      value = UniValue.ParseXml("<response><item type=\"4\">1</item><item type=\"5\">2</item><item type=\"6\">3</item></response>");
      Console.WriteLine(value);
      testValue = new string[] { "4", "5", "6" };
      testValue2 = new string[] { "1", "2", "3" };
      j = 0;
      foreach (UniValue itm in value["response"]["item"])
      {
        Console.WriteLine("-- {0}/{1}", itm, itm["@type"]);
        if (ok)
        {
          ok = (testValue[j] == itm["@type"]) && (itm == testValue2[j]);
        }
        j++;
      }
      if (ok)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("-------------------------------------");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");
      ok = true;
      Console.WriteLine("Test 4: Attributes 2");
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
      Console.WriteLine(value);
      foreach (UniValue itm in value["response"]["item"])
      {
        Console.WriteLine("-- {0}/{1} = {2}/{3}/{4}", itm["id"], itm["@type"], itm["name"], itm["name"]["@case"], itm["name"]["@style"]);
        if (ok)
        {
          ok = (testValue[j] == itm["@type"]) && (itm["id"] == testValue2[j]) && (itm["name"]["@case"] == testValue3[j]) && (testValue4[j] == itm["name"]["@style"]) && (testValue5[j] == itm["name"]);
        }
        j++;
      }
      if (ok)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("-------------------------------------");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");
      ok = true;
      testValue = new string[] { "text", "date", "num" };
      testValue2 = new string[] { "1", "2", "num" };
      j = 0;
      Console.WriteLine("Test 5: Attributes 3");
      value = UniValue.ParseXml
      (
        @"<response>
            <item id=""text""><id>1</id></item>
            <item id=""date""><id>2</id></item>
            <item id=""num"" />
          </response>"
      );
      Console.WriteLine(value);
      foreach (UniValue itm in value["response"]["item"])
      {
        Console.WriteLine("-- {0}/{1}", itm["id"], itm["@id"]);
        if (ok)
        {
          ok = (testValue[j] == itm["@id"]) && (itm["id"] == testValue2[j]);
        }
        j++;
      }
      if (ok)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("-------------------------------------");
      Console.WriteLine(value);
    }

    [TestMethod]
    public void UniValue_Index()
    {
      bool ok = true;
      Console.WriteLine("Test 1");
      UniValue r = UniValue.Empty;

      Console.WriteLine("Key: {0}", r["test"]);
      if (r["test"] == null)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine(r);

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Index: {0}", r[10]);
      if (r[10] == null)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine(r);

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Key + Index: {0}", r["test"][12]);
      if (r["test"][12] == null)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      if (r["test"].First() == r["test"][12])
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine(r);

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("All: {0}", r["xyz"][3]["qwer"][1]["abc"]);
      if (r["xyz"][3]["qwer"][1]["abc"] == null)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }

      Console.WriteLine(r);

      Console.WriteLine("-------------------------------------");

      ok = true;
      Console.WriteLine("Test 2");
      r = UniValue.ParseJson("[1, 2, 3]");
      for (int i = 0; i < 5; i++)
      {
        Console.WriteLine("{0}: {1}", i, r[i]);
        if (ok)
        {
          if (i + 1 <= 3)
          {
            ok = (r[i] == i + 1);
          }
          else
          {
            ok = r[i] == null;
          }
        }
      }
      if (ok)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }

      Console.WriteLine("-------------------------------------");
      Console.WriteLine("Test 3");
      ok = true;
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");
      if (ok)
      {
        ok = value[0].Key == "response";
      }
      if (ok)
      {
        ok = value["response"].Key == "response";
      }
      if (ok)
      {
        ok = value[0][0].Key == "item";
      }
      if (ok)
      {
        ok = value["response"][0].Key == "item";
      }
      if (ok)
      {
        ok = value["response"]["item"][2] == 3;
      }
      if (ok)
      {
        ok = value[0][0][1] == 2;
      }
      if (ok)
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }

      Console.WriteLine("-------------------------------------");
      // UniValue.Create().Add("test", 123);
      Console.WriteLine("Test 4");

      r = UniValue.Empty;
      r["test"] = "123";
      if (r["test"] != "123")
      {
        Assert.Fail();
      }

      r = UniValue.Empty;
      r["test"]["a"] = "abc";
      if (r["test"]["a"] != "abc")
      {
        Assert.Fail();
      }

      r = UniValue.Empty;
      r["a"][0]["test"][2] = "aaa";
      if (r["a"][0]["test"][2] != "aaa")
      {
        Assert.Fail();
      }

      r = UniValue.Empty;
      r.Add("test", "123");
      if (r["test"] != "123")
      {
        Assert.Fail();
      }

      r["test"].Add("a", "abc");
      if (r["test"]["a"] != "abc")
      {
        Assert.Fail();
      }

      //value["response"]["item"][2]["test"] = "hello, world!";
      //var rr = value["response"]["item"][2];
      //rr["test"] = "123";

      r[0][1][2][3][4][5][6][7]["a"]["b"]["c"] = 123;
      if (r[0][1][2][3][4][5][6][7]["a"]["b"]["c"] != 123)
      {
        Assert.Fail();
      }
    }

    [TestMethod]
    public void UniValue_Clone()
    {
      Console.WriteLine("Test 1: String");
      UniValue v = UniValue.Create("test");
      UniValue v2 = v.Clone() as UniValue;
      v2 = "123";
      if (v.Equals(v2))
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }
      Console.WriteLine("Test 2: Object");
      var o = new { id = 123, text = "test" };
      v = UniValue.Create(o);
      v2 = v.Clone() as UniValue;
      v2 = "test";
      if (v.Equals(v2))
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }
      Console.WriteLine("Test 3: Reference");
      v = UniValue.Create(o);
      v2 = v;
      v["id"] = "555";
      if (v.Equals(v2))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
    }

    [TestMethod]
    public void UniValue_IsNullOrEmpty()
    {
      UniValue v = null;
      Console.WriteLine("null = {0}", UniValue.IsNullOrEmpty(v));
      if (UniValue.IsNullOrEmpty(v))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      v = "123";
      Console.WriteLine("123 = {0}", UniValue.IsNullOrEmpty(v));
      if (!UniValue.IsNullOrEmpty(v))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("1 = {0}", UniValue.IsNullOrEmpty(1));
      if (!UniValue.IsNullOrEmpty(1))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("test = {0}", UniValue.IsNullOrEmpty("test"));
      if (!UniValue.IsNullOrEmpty("test"))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("empty = {0}", UniValue.IsNullOrEmpty(UniValue.Empty));
      if (UniValue.IsNullOrEmpty(UniValue.Empty))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("create = {0}", UniValue.IsNullOrEmpty(UniValue.Create(DateTime.Now)));
      if (!UniValue.IsNullOrEmpty(UniValue.Create(DateTime.Now)))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
      Console.WriteLine("create empty = {0}", UniValue.IsNullOrEmpty(UniValue.Create()));
      if (UniValue.IsNullOrEmpty(UniValue.Create()))
      {
        Console.WriteLine("OK");
      }
      else
      {
        Assert.Fail();
      }
    }

    [TestMethod]
    public void UniValue_Parents()
    {
      Console.WriteLine("Test 1");
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");

      Console.WriteLine("[response] = {0}", value["response"].Parent.Key);
      if (value["response"].Parent.Key != "root")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[response][item] = {0}", value["response"]["item"].Parent.Key);
      if (value["response"]["item"].Parent != value["response"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 2");
      value = UniValue.ParseXml("<response><item><id>1</id></item><item><id>2</id></item><item><id>3</id></item></response>");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");

      Console.WriteLine("[response][item][id] = {0}", value["response"]["item"][0]["id"].Parent.Key);

      if (value["response"]["item"][0]["id"].Parent != value["response"]["item"][0])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      if (value["response"]["item"][0].Parent != value["response"]["item"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 3");
      value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");

      Console.WriteLine("[response][item][1] = {0}", value["response"]["item"][1].Parent.Key);

      if (value["response"]["item"].First().Parent != value["response"]["item"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      if (value["response"]["item"].Last().Parent != value["response"]["item"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      if (value["response"]["item"]["0"].Parent != value["response"]["item"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      if (value["response"]["item"][1].Parent != value["response"]["item"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[response][0][1] = {0}", value["response"][0][1].Parent.Key);
      if (value["response"][0][1].Parent != value["response"]["item"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[response][item][2][test] = {0}", value["response"]["item"][2]["test"].Parent);
      if (value["response"]["item"][2]["test"].Parent.Key != value["response"]["item"][2].Key)
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[response][item][2][test] = {0}", value["response"]["item"][2]["test"].Parent);
      if (value["response"]["item"][2]["test"].Parent != value["response"]["item"][2])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[response][item][2][test][a] = {0}", value["response"]["item"][2]["test"]["a"].Parent);
      if (value["response"]["item"][2]["test"]["a"].Parent.Key != value["response"]["item"][2]["test"].Key)
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[response][item][2][5] = {0}", value["response"]["item"][2][5].Parent);
      if (value["response"]["item"][2][5].Parent != value["response"]["item"][2])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 2");
      value = UniValue.ParseJson("{data: [{id: 1, name: 'test'}, {id: 2, name: 'test2'}, {id: 3, name: 'test3', list: [{x:1,y:'hi'}, {x:2, y:'hello'}] }] }");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");

      Console.WriteLine("[data] = {0}", value["data"].Parent.Key);
      if (value["data"].Parent.Key != "root")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[data][0] = {0}", value["data"][0].Parent.Key);
      if (value["data"][0].Parent != value["data"])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[data][1][name] = {0}", value["data"][1]["name"].Parent.Key);
      if (value["data"][1]["name"].Parent != value["data"][1])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[data][0][1] = {0}", value["data"][0][1].Parent.Key);
      if (value["data"][0][1].Parent != value["data"][0])
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }
    }

    [TestMethod]
    public void UniValue_ToString()
    {
      Console.WriteLine("Test 1");
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");
      Console.WriteLine(value);
      Console.WriteLine("-------------------------------------");

      if (value != "{ \"response\": { \"item\": [\"1\", \"2\", \"3\"] } }")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      value["response"]["item"]["test"] = "123";
      Console.WriteLine(value);

      if (value != "{ \"response\": { \"item\": { \"0\": \"1\", \"1\": \"2\", \"2\": \"3\", \"test\": \"123\" } } }")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 2");
      value = UniValue.ParseJson("{\"data\": [{\"id\": 1, \"name\": \"test\"}, {\"id\": 2, \"name\": \"test2\"}, {\"id\": 3, \"name\": \"test3\", \"list\": [{\"x\":1,\"y\":\"hi\"}, {\"x\":2, \"y\":\"hello\"}] }] }");
      Console.WriteLine("-------------------------------------");

      Console.WriteLine("[] = {0}", value);
      if (value != "{ \"data\": [{ \"id\": 1, \"name\": \"test\" }, { \"id\": 2, \"name\": \"test2\" }, { \"id\": 3, \"name\": \"test3\", \"list\": [{ \"x\": 1, \"y\": \"hi\" }, { \"x\": 2, \"y\": \"hello\" }] }] }")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[data] = {0}", value["data"]);
      if (value["data"] != "[{ \"id\": 1, \"name\": \"test\" }, { \"id\": 2, \"name\": \"test2\" }, { \"id\": 3, \"name\": \"test3\", \"list\": [{ \"x\": 1, \"y\": \"hi\" }, { \"x\": 2, \"y\": \"hello\" }] }]")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("[data][1][name] = {0}", value["data"][1]["name"]);
      if (value["data"][1]["name"] != "test2")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 3");
      value = UniValue.ParseXml("<response><item id='1' style='bold' color='white' /><item id='2' style='italic' color='red'>123</item></response>");

      Console.WriteLine("[]");
      Console.WriteLine(value);
      Console.WriteLine();

      Console.WriteLine("[response]");
      Console.WriteLine(value["response"]);
      Console.WriteLine();

      Console.WriteLine("[response][item]");
      Console.WriteLine(value["response"]["item"]);
      Console.WriteLine();

      Console.WriteLine("[response][item][0]");
      Console.WriteLine(value["response"]["item"][0]);
      Console.WriteLine();

      Console.WriteLine("[response][item][1]");
      Console.WriteLine(value["response"]["item"][1]);
      Console.WriteLine();

      Console.WriteLine("[response][item][0][@id]");
      Console.WriteLine(value["response"]["item"][0]["@id"]);
      Console.WriteLine();

      Console.WriteLine("[response][item][0][@style]");
      Console.WriteLine(value["response"]["item"][0]["style"]);
      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 4");
      value = UniValue.ParseXml("<response><item id='1' style='bold' color='white'><vvalue abc='test' avalue='1234567'></vvalue></item><item id='2' style='italic' color='red'><v>456</v></item></response>");
      Console.WriteLine("[]");
      Console.WriteLine(value);
      Console.WriteLine();

      Console.WriteLine("[response]");
      Console.WriteLine(value["response"]);
      Console.WriteLine();

      Console.WriteLine("[response][item]");
      Console.WriteLine(value["response"]["item"]);

      if (value["response"]["item"] != "[{ \"value\": { \"vvalue\": { \"@abc\": \"test\", \"@avalue\": \"1234567\" } }, \"@id\": \"1\", \"@style\": \"bold\", \"@color\": \"white\" }, { \"value\": { \"v\": \"456\" }, \"@id\": \"2\", \"@style\": \"italic\", \"@color\": \"red\" }]")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine();

      Console.WriteLine("[response][item][0]");
      Console.WriteLine(value["response"]["item"][0]);

      if (value["response"]["item"][0] != "{ \"vvalue\": { \"@abc\": \"test\", \"@avalue\": \"1234567\" } }")
      {
        Assert.Fail();
      }
      else
      {
        Console.WriteLine("OK");
      }

      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 5");
      value = UniValue.ParseJson("{data: [{id: 1, name: null}, {id: 2, name: 'test2\r\n123'}, {id: 3, name: 'test3', list: [{x:1,y:'hi'}, {x:2, y:'hello'}] }] }");
      Console.WriteLine(value);
      Assert.AreEqual(value.ToString(), "{ \"data\": [{ \"id\": 1, \"name\": null }, { \"id\": 2, \"name\": \"test2\\r\\n123\" }, { \"id\": 3, \"name\": \"test3\", \"list\": [{ \"x\": 1, \"y\": \"hi\" }, { \"x\": 2, \"y\": \"hello\" }] }] }");

      value = "test\r\ntest";
      Console.WriteLine(value);
      Assert.AreEqual(value.ToString(), "test\r\ntest");
      Console.WriteLine("-------------------------------------");
    }

    [TestMethod]
    public void UniValue_Convert()
    {
      UniValue ok = "OK";
      
      Console.WriteLine("Test 1: Numeric to Int32");
      var l = new List<UniValue>();
      l.Add((byte)0);
      l.Add((sbyte)1);
      l.Add((bool)true);
      l.Add((Int16)123);
      l.Add((Int32)123);
      l.Add((Int64)123);
      l.Add((UInt16)123);
      l.Add((UInt32)123);
      l.Add((UInt64)123);
      l.Add((double)123.22);
      l.Add((decimal)123.33);
      l.Add((float)123.55);

      foreach (UniValue item in l)
      {
        Console.WriteLine("{0} = {1}", item, (int)item);
      }

      Console.WriteLine(ok);

      Console.WriteLine();
      Console.WriteLine("-------------------------------------");
      Console.WriteLine();

      Console.WriteLine("Test 2: Numeric to Double");

      foreach (UniValue item in l)
      {
        Console.WriteLine("{0} = {1}", item, (double)item);
      }

      Console.WriteLine(ok);

      Console.WriteLine();
      Console.WriteLine("-------------------------------------");
      Console.WriteLine();

      Console.WriteLine("Test 3: Numeric to string");

      foreach (UniValue item in l)
      {
        string str = item.ToString();
      }

      Console.WriteLine(ok);

      Console.WriteLine();
      Console.WriteLine("-------------------------------------");
      Console.WriteLine();

      Console.WriteLine("Test 4: Numeric to Boolean");

      foreach (var item in l)
      {
        Console.WriteLine("{0} = {1}", item, (bool)item);
      }

      Console.WriteLine(ok);

      Console.WriteLine();
      Console.WriteLine("-------------------------------------");
      Console.WriteLine();

      Console.WriteLine("Test 5: Convert class");

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

      Console.WriteLine(ok);
    }

    [TestMethod]
    public void UniValue_Enumerator()
    {
      Console.WriteLine("Test 1");
      UniValue value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");
      Console.WriteLine(value);
      foreach (UniValue itm in value.First()["item"])
      {
        Console.WriteLine(itm);
        if (!(itm == "1" || itm == 2 || itm == "3"))
        {
          Assert.Fail();
        }
      }
      Console.WriteLine("-------------------------------------");
      Console.WriteLine("Test 2");
      value = UniValue.ParseXml("<response><item>1</item><item>2</item><item>3</item></response>");
      Console.WriteLine(value);
      foreach (UniValue itm in value["response"]["item"].First())
      {
        Console.WriteLine(itm);
        if (itm != "1")
        {
          Assert.Fail();
        }
      }
    }

    [TestMethod]
    public void UniValue_Compatibility()
    {
      Console.WriteLine("Test 1");
      UniValue r = UniValue.ParseJson("{ response: [{ uid: 489, first_name: 'Aleksandr', last_name: '', nickname: '', online: 1, user_id: 484, lists: [1] }, { uid: 546, first_name: 'Oksana', last_name: '', deactivated: 'deleted', online: 0, user_id: 546 }, { uid: 845, first_name: 'Sergey', last_name: '', nickname: '', online: 0, user_id: 845 }] }");
      Console.WriteLine(@"foreach (Dictionary<string, object> itm in (Array)r[""response""])
Replace Dictionary<string, object> to ResultValue");
      foreach (UniValue itm in (Array)r["response"])
      {
        Console.WriteLine("{0} = {1}", itm["uid"], itm["first_name"]);
      }
      Console.WriteLine("OK");
      Console.WriteLine("-------------------------------------");

      Console.WriteLine("Test 2: Binary");
      r = UniValue.Create(new byte[] { 1, 2, 3, 4, 5 });
      var b = (byte[])r;
      Console.WriteLine(Convert.ToBase64String(b));
      Console.WriteLine("OK");
    }

  }

}