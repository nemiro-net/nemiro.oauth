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

      /*OAuthManager.RegisterClient
      (
        new TwitterClient
        (
          "cXzSHLUy57C4gTBgMGRDuqQtr",
          "3SSldiSb5H4XeEMOIIF4osPWxOy19jrveDcPHaWtHDQqgDYP9P"
        )
      );*/

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

      /*OAuthManager.RegisterClient
      (
        new GoogleClient
        (
          "1058655871432-fscjqht7ou30a75gjkde1eu1brsvbqkn.apps.googleusercontent.com",
          "SI5bIZkrSB5rO03YF-CdsCJC"
        )
        {
          ReturnUrl = "http://localhost/"
        }
      );*/

      OAuthManager.RegisterClient
      (
        new YandexClient
        (
          "0ee5f0bf2cd141a1b194a2b71b0332ce",
          "59d76f7c09b54ad38e6b15f792da7a9a"
        )
      );

      OAuthManager.RegisterClient
      (
        new OdnoklassnikiClient
        (
          "1094959360",
          "E45991423E8C5AE249B44E84",
          "CBACMEECEBABABABA"
        ) 
        {
          ReturnUrl = "http://localhost"  // return url - it's important
        }
      );

      /*OAuthManager.RegisterClient
      (
        new LiveClient
        (
          "0000000040124265",
          "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
        )
      );*/

      /*OAuthManager.RegisterClient
      (
        new AmazonClient
        (
          "amzn1.application-oa2-client.f0ffe4edc256488dae00dcaf96d75d1b",
          "764dcefe49b441c8c6244c93e5d5d04de54fda6dfdc83da9693bf346f4dc4515"
        )
      );*/

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