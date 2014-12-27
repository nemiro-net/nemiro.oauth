// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2014. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;
using System.Threading.Tasks;

namespace DropboxExample
{

  public partial class Login : Form
  {

    private DropboxClient Dropbox = new DropboxClient("5nkunr8uscwfoba", "n7x9icfwoe6dehq");

    public Login()
    {
      InitializeComponent();
      // open login page
      this.webBrowser1.Navigate(this.Dropbox.AuthorizationUrl);
    }

    private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
      // waiting for results
      if (e.Url.ToString().Equals("about:blank", StringComparison.OrdinalIgnoreCase))
      {
        // the user has refused to give permission 
        this.Complete();
      }
      else
      {
        if (webBrowser1.Document.GetElementById("auth-code") != null)
        {
          // found authorization code
          try
          {
            // verify code
            this.Dropbox.AuthorizationCode = webBrowser1.Document.GetElementById("auth-code").InnerText;
            // show progress
            this.webBrowser1.Visible = false;
            this.pictureBox1.Visible = true;
            // save access token to application settings
            var t = Task.Factory.StartNew(() =>
            {
              // this.Dropbox.AccessToken["access_token"].ToString()
              Properties.Settings.Default.AccessToken = ((OAuth2AccessToken)this.Dropbox.AccessToken).Value;
              Properties.Settings.Default.Save();
              this.Complete();
            });
          }
          catch (Exception ex)
          {
            // show error message
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Complete();
          }
        }
      }
    }

    private void Complete()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(Complete));
        return;
      }
      this.Close();
    }

  }

}
