using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nemiro.OAuth;

// For more details, please visit http://oauth.nemiro.net

namespace Test.CSharp.WinForms
{

  public partial class Form2 : Form
  {

    public Form2()
    {
      InitializeComponent();
    }

    public Form2(string providerName)
    {
      InitializeComponent();
      webBrowser1.ScriptErrorsSuppressed = true;
      webBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl(providerName));
    }

    private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
      // waiting for results
      if (e.Url.Query.IndexOf("code=") != -1 || e.Url.Fragment.IndexOf("code=") != -1 || e.Url.Query.IndexOf("oauth_verifier=") != -1)
      {
        // is the result, verify
        var result = OAuthWeb.VerifyAuthorization(e.Url.ToString());
        if (result.IsSuccessfully)
        {
          // show user info
          MessageBox.Show
          (
            String.Format
            (
              "User ID: {0}\r\nUsername: {1}\r\nDisplay Name: {2}\r\nE-Mail: {3}", 
              result.UserInfo.UserId,
              result.UserInfo.UserName,
              result.UserInfo.DisplayName ?? result.UserInfo.FullName,
              result.UserInfo.Email
            ), 
            "Successfully", 
            MessageBoxButtons.OK, 
            MessageBoxIcon.Information
          );

          // using access token for API request
          // (this small example for vkontakte only)
          if (result.ProviderName.Equals("vk", StringComparison.OrdinalIgnoreCase))
          {
            new VkontakteFriends(result.AccessTokenValue).ShowDialog();
          }
        }
        else
        {
          // show error message
          MessageBox.Show(result.ErrorInfo.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        this.Close();
      }
    }

  }

}