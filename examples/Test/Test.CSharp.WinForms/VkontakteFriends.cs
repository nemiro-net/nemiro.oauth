using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using Nemiro.OAuth;

namespace Test.CSharp.WinForms
{
  public partial class VkontakteFriends : Form
  {
    public VkontakteFriends()
    {
      InitializeComponent();
    }

    public VkontakteFriends(string accessToken)
    {
      InitializeComponent();

      // query parameters
      var parameters = new NameValueCollection
      { 
        { "access_token", accessToken },
        { "count", "10" }, // first 10 friends
        { "fields", "nickname" }
      };

      // execute the request
      var result = Nemiro.OAuth.OAuthUtility.ExecuteRequest
      (
        "GET",
        "https://api.vk.com/method/friends.get",
        parameters,
        null
      );

      foreach (UniValue itm in result["response"])
      {
        string friendName = "";
        if (itm.ContainsKey("first_name") && itm.ContainsKey("last_name"))
        {
          friendName = String.Format("{0} {1}", itm["first_name"], itm["last_name"]);
        }
        else
        {
          friendName = itm["nickname"].ToString();
        }
        listBox1.Items.Add(friendName);
      }
      
    }
  }
}
