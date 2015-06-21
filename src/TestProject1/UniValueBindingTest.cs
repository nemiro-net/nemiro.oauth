using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Nemiro.OAuth;

namespace TestProject1
{

  [TestClass]
  public class UniValueBindingTest
  {

    [TestMethod]
    public void UniValue_Binding()
    {
      var list = new ListBox();
      var v = UniValue.ParseJson("[{text: 'test', value: 1, any: 'aaaa'},{text: '123', value: 2, any: 'bbbb'},{text: 'xyz', value: 3, any: 'cccc'}]");

      list.DisplayMember = "text";
      list.ValueMember = "value";

      list.DataSource = v;

      var form = new Form();
      form.Controls.Add(list);

      var list2 = new ListBox() { Left = list.Left + list.Width };
      list2.DisplayMember = "text";
      list2.ValueMember = "value";

      foreach (UniValue item in v)
      {
        list2.Items.Add(item);
      }

      form.Controls.Add(list2);


      var list3 = new ListBox() { Left = list2.Left + list2.Width };
      list3.DisplayMember = "text";
      list3.ValueMember = "value";
      v = UniValue.ParseJson("{items: [{text: 'test', value: 1, any: 'aaaa'},{text: '123', value: 2, any: 'bbbb'},{text: 'xyz', value: 3, any: 'cccc'}]}");

      foreach (UniValue item in v["items"])
      {
        list3.Items.Add(item);
      }

      form.Controls.Add(list3);

      form.Show();

      list.SelectedIndex = 0;
      Assert.IsTrue(list.Text == "test");

      form.Close();
      
      list.SelectedIndex = 1;
      if (list.Text != "123")
      {
        Assert.Fail();
      }
      if (list.SelectedValue.ToString() != "2")
      {
        Assert.Fail();
      }
      if (((UniValue)list.SelectedItem)["any"] != "bbbb")
      {
        Assert.Fail();
      }

      list2.SelectedIndex = 1;
      if (list2.Text != "123")
      {
        Assert.Fail();
      }
      if (((UniValue)list2.SelectedItem)["value"] != "2")
      {
        Assert.Fail();
      }

      list3.SelectedIndex = 2;
      if (list3.Text != "xyz")
      {
        Assert.Fail();
      }
      if (((UniValue)list3.SelectedItem)["value"] != "3")
      {
        Assert.Fail();
      }

      form.Close();

    }

  }

}