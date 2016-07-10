// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014. All rights reserved.
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
using System.Net;
using System.IO;

namespace DropboxExample
{

  public partial class Download : Form
  {

    public Uri Url { get; protected set; }

    public string LoadingFileName { get; protected set; }

    private WebClient Client = new WebClient();

    public Download()
    {
      InitializeComponent();
    }

    public Download(string url) : this()
    {
      this.Url = new Uri(String.Format("{0}?access_token={1}", url, Properties.Settings.Default.AccessToken));
      this.saveFileDialog1.FileName = this.LoadingFileName = Path.GetFileName(url); // .Substring(0, url.IndexOf("?"))
      this.Text = String.Format("Loading '{0}'", this.LoadingFileName);
    }

    private void Download_Load(object sender, EventArgs e)
    {
      if (this.saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
      {
        this.Close();
        return;
      }
      this.Client.DownloadProgressChanged += DownloadProgressChanged;
      this.Client.DownloadFileCompleted += DownloadFileCompleted;
      this.Client.DownloadFileAsync(this.Url, this.saveFileDialog1.FileName);
    }

    public void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
      progressBar1.Value = e.ProgressPercentage;
      this.Text = String.Format("Loading '{0}' ({1}%)", this.LoadingFileName, e.ProgressPercentage);
    }

    public void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      this.Close();
    }

    private void Download_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        this.Client.CancelAsync();
      }
    }

  }

}
