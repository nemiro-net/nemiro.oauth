// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014, 2016. All rights reserved.
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
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Nemiro.OAuth;

namespace DropboxExample
{

  public partial class Download : Form
  {

    public string LoadingFileName { get; protected set; }

    private string FilePath { get; set; }

    private byte[] ReadBuffer = new byte[4096];

    private Stream Reader = null;

    private FileStream FileStream = null;

    private BinaryWriter Writer = null;

    private bool IsClosed = false;

    public Download()
    {
      InitializeComponent();
    }

    public Download(UniValue path) : this()
    {
      this.FilePath = path.ToString();
      this.saveFileDialog1.FileName = this.LoadingFileName = Path.GetFileName(this.FilePath);
      this.Text = String.Format("Loading '{0}'", this.LoadingFileName);
    }

    private void Download_Load(object sender, EventArgs e)
    {
      if (this.saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
      {
        this.Close();
        return;
      }

      this.FileStream = new FileStream(this.saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
      this.Writer = new BinaryWriter(this.FileStream);

      var req = WebRequest.Create("https://content.dropboxapi.com/2/files/download");

      req.Method = "POST";

      req.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", Properties.Settings.Default.AccessToken));
      req.Headers.Add("Dropbox-API-Arg", UniValue.Create(new { path = this.FilePath }).ToString());

      req.BeginGetResponse(result =>
      {
        var resp = req.EndGetResponse(result);

        this.SafeInvoke(() =>
        {
          this.progressBar1.Maximum = (int)resp.ContentLength;
          this.Text = String.Format("Loading '{0}' ({1}%)", this.LoadingFileName, 0);
        });

        // get response stream
        this.Reader = resp.GetResponseStream();

        // read async
        this.Reader.BeginRead(this.ReadBuffer, 0, this.ReadBuffer.Length, this.ReadCallback, null);
      }, null);
    }

    private void ReadCallback(IAsyncResult result)
    {
      if (this.IsClosed) { return; }

      var bytesRead = this.Reader.EndRead(result);

      if (bytesRead > 0)
      {
        this.SafeInvoke(() =>
        {
          this.progressBar1.Increment(bytesRead);
          this.Text = String.Format("Loading '{0}' ({1}%)", this.LoadingFileName, (this.progressBar1.Value * 100) / this.progressBar1.Maximum);
        });

        if (this.FileStream.CanWrite)
        {
          // write to file
          this.Writer.Write(this.ReadBuffer.Take(bytesRead).ToArray());

          // read next part
          this.Reader.BeginRead(this.ReadBuffer, 0, this.ReadBuffer.Length, ReadCallback, null);
        }
      }
      else
      {
        this.FileStream.Close();

        this.SafeInvoke(() =>
        {
          this.progressBar1.Value = this.progressBar1.Maximum;
          this.Text = String.Format("Loading '{0}' ({1}%)", this.LoadingFileName, 100);

          this.Close();
        });
      }
    }

    private void Download_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.Reader != null && this.FileStream != null)
      {
        this.IsClosed = true;

        this.Reader.Close();
        this.FileStream.Close();

        if (this.Reader.CanRead || this.FileStream.CanWrite)
        {
          e.Cancel = true;
          this.Visible = false;
          timer1.Enabled = true;
        }
      }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (!this.Reader.CanRead && !this.FileStream.CanWrite)
      {
        this.Close();
      }
    }

    private void SafeInvoke(Action action)
    {
      try
      {
        if (this.IsDisposed || this.IsClosed)
        {
          return;
        }

        if (this.InvokeRequired)
        {
          this.Invoke(new Action<Action>(SafeInvoke), action);
          return;
        }

        action();
      }
      catch (ObjectDisposedException ex)
      {
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

  }

}
