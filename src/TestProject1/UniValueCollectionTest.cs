using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nemiro.OAuth;

namespace TestProject1
{

  [TestClass]
  public class UniValueCollectionTest
  {

    [TestMethod]
    public void UniValueCollection_Common()
    {
      var c = new UniValueCollection();
      c.Add("test", "123");
      if (!c.ContainsKey("test"))
      {
        Assert.Fail();
      }
      var k = new KeyValuePair<string, UniValue>("test2", "234");
      c.Add(k);
      if (!c.ContainsKey("test2"))
      {
        Assert.Fail();
      }
      if (!c.Contains(k))
      {
        Assert.Fail();
      }

      if (c.ContainsKey("abc"))
      {
        Assert.Fail();
      }

      if (!c.Remove("test"))
      {
        Assert.Fail();
      }
      if (c.ContainsKey("test"))
      {
        Assert.Fail();
      }

      if (!c.Remove(k))
      {
        Assert.Fail();
      }
      if (c.ContainsKey("test2"))
      {
        Assert.Fail();
      }

      c.Add("test", "123");
      c.Add("test", "234");
      c.Add("test", "345");
      if (!c["test"].IsCollection)
      {
        Assert.Fail();
      }

      Console.WriteLine(c);
      if (c.ToString() != "{ \"test\": [\"123\", \"234\", \"345\"] }")
      {
        Assert.Fail();
      }
      if (c["test"] != "[\"123\", \"234\", \"345\"]")
      {
        Assert.Fail();
      }

      Console.WriteLine(c.Count);
      if (c.Count != 1)
      {
        Assert.Fail();
      }

      Console.WriteLine(c.Count);
      if (c["test"].Count != 3)
      {
        Assert.Fail();
      }

      Console.WriteLine(c.Keys.Count);
      if (c.Keys.Count != 1)
      {
        Assert.Fail();
      }

      Console.WriteLine(c.Values.Count);
      if (c.Keys.Count != 1)
      {
        Assert.Fail();
      }

      c.Clear();

      if (c.Count > 0)
      {
        Assert.Fail();
      }

      for (int i = 0; i <= 9; i++)
      {
        c.Add(String.Format("test-{0}", i), Guid.NewGuid().ToString());
      }

      foreach (string key in c.Keys)
      {
        Console.WriteLine("{0} = {1}", key, c[key]);
      }

      Console.WriteLine("OK");

      UniValue vv = UniValue.Empty;
      foreach (UniValue itm in vv["data"])
      {
      }
    }

  }

}