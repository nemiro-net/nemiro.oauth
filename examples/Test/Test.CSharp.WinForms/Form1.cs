using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nemiro.OAuth;
using System.Collections.Specialized;
using Nemiro.OAuth.Clients;

// For more details, please visit http://oauth.nemiro.net

namespace Test.CSharp.WinForms
{

  public partial class Form1 : Form
  {

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      // OAuth clients registration
      // NOTE: Specify their own client IDs and secret keys
      OAuthManager.RegisterClient
      (
        new FacebookClient
        (
          "1435890426686808",
          "c6057dfae399beee9e8dc46a4182e8fd"
        ) 
        { 
          Parameters = new NameValueCollection { { "display", "popup" } }
        }
      );

      OAuthManager.RegisterClient
      (
        new VkontakteClient
        (
          "4457505",
          "wW5lFMVbsw0XwYFgCGG0"
        ) 
        { 
          Parameters = new NameValueCollection { { "display", "popup" } },
          Scope = "status,friends,email"
        }
      );

      OAuthManager.RegisterClient
      (
        new MailRuClient
        (
          "722701",
          "d0622d3d9c9efc69e4ca42aa173b938a"
        )
      );

      OAuthManager.RegisterClient
      (
        new YandexClient
        (
          "0ee5f0bf2cd141a1b194a2b71b0332ce",
          "59d76f7c09b54ad38e6b15f792da7a9a"
        )
      );

      // add buttons to the form
      foreach (string providerName in OAuthManager.RegisteredClients.Keys)
      {
        var btn = new Button();
        btn.Text = providerName;
        btn.Click += new EventHandler(btn_Click);
        flowLayoutPanel1.Controls.Add(btn);
      }
    }

    private void btn_Click(object sender, EventArgs e)
    {
      // show login form
      var frm = new Form2(((Button)sender).Text);
      frm.ShowDialog();
    }

  }

}