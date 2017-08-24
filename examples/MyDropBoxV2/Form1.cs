using System;
using System.Text;
using System.Windows.Forms;
using Nemiro.OAuth;
using Nemiro.OAuth.LoginForms;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Linq;

namespace MyDropBox
{
  public partial class Form1 : Form
  {

    private HttpAuthorization Authorization = null;

    private string CurrentPath = "";

    private long UploadingFileLength = 0;

    private Stream DownloadReader = null;
    private FileStream DownloadFileStream = null;
    private BinaryWriter DownloadWriter = null;
    private byte[] DownloadReadBuffer = new byte[4096];

    public Form1()
    {
      InitializeComponent();
    }
     
    private void Form1_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
      {
        this.GetAccessToken();
      }
      else
      {
        // create authorization header
        this.Authorization = new HttpAuthorization(AuthorizationType.Bearer, Properties.Settings.Default.AccessToken);

        // get files
        this.GetFiles();
      }
    }

    private void GetAccessToken()
    {
      var login = new DropboxLogin("4mlpoeq657vuif8", "1whj6c5mxtkns7m", "https://oauthproxy.nemiro.net/", false, false);
      login.Owner = this;
      login.ShowDialog();

      if (login.IsSuccessfully)
      {
        Properties.Settings.Default.AccessToken = login.AccessTokenValue;
        Properties.Settings.Default.Save();

        this.Authorization = new HttpAuthorization(AuthorizationType.Bearer, login.AccessTokenValue);

        this.GetFiles();
      }
      else
      {
        MessageBox.Show("error...");
      }
    }

    private void GetFiles()
    {
      OAuthUtility.PostAsync
      (
        "https://api.dropboxapi.com/2/files/list_folder",
        new HttpParameterCollection
        {
          new 
          { 
            path = this.CurrentPath,
            include_media_info = true
          }
        },
        contentType: "application/json",
        authorization: this.Authorization,
        callback: this.GetFiles_Result
      );
    }

    private void GetFiles_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(this.GetFiles_Result), result);
        return;
      }

      if (result.StatusCode == 200)
      {

        this.listBox1.Items.Clear();

        this.listBox1.DisplayMember = "path_display";

        foreach (UniValue file in result["entries"])
        {
          listBox1.Items.Add(file);
        }

        if (!String.IsNullOrEmpty(this.CurrentPath))
        {
          this.listBox1.Items.Insert(0, UniValue.ParseJson("{path_display: '..'}"));
        }
      }
      else
      {
        MessageBox.Show(result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      OAuthUtility.PostAsync
      (
        "https://api.dropboxapi.com/2/files/create_folder",
        new HttpParameterCollection
        {
          new
          {
            path = ((String.IsNullOrEmpty(this.CurrentPath) ? "/" : "") + Path.Combine(this.CurrentPath, this.textBox1.Text).Replace("\\", "/"))
          }
        },
        contentType: "application/json",
        authorization: this.Authorization,
        callback: this.CreateFolder_Result
      );
    }

    private void CreateFolder_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(this.CreateFolder_Result), result);
        return;
      }

      if (result.StatusCode == 200)
      {
        this.GetFiles();
      }
      else
      {
        if (result["error"].HasValue)
        {
          MessageBox.Show(result["error"].ToString());
        }
        else 
        {
          MessageBox.Show(result.ToString());
        }
      }
    }

    private void listBox1_DoubleClick(object sender, EventArgs e)
    {
      if (listBox1.SelectedItem == null) { return; }

      var file = (UniValue)this.listBox1.SelectedItem;

      if (file["path_display"] == "..")
      {
        if (!String.IsNullOrEmpty(this.CurrentPath))
        {
          this.CurrentPath = Path.GetDirectoryName(this.CurrentPath).Replace("\\", "/");
          if (this.CurrentPath == "/") { this.CurrentPath = ""; }
        }
      }
      else
      {
        if (file[".tag"].Equals("folder"))
        {
          this.CurrentPath = file["path_display"].ToString();
        }
        else
        {
          this.saveFileDialog1.FileName = Path.GetFileName(file["path_display"].ToString());

          if (this.saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
          {
            return;
          }

          this.progressBar1.Value = 0;

          this.DownloadFileStream = new FileStream(this.saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
          this.DownloadWriter = new BinaryWriter(this.DownloadFileStream);

          // download file
          var req = WebRequest.Create("https://content.dropboxapi.com/2/files/download");

          req.Method = "POST";

          req.Headers.Add(HttpRequestHeader.Authorization, this.Authorization.ToString());
          req.Headers.Add("Dropbox-API-Arg", UniValue.Create(new { path = file["path_display"].ToString() }).ToString());

          req.BeginGetResponse(result =>
          {
            var resp = req.EndGetResponse(result);

            this.SafeInvoke(() =>
            {
              this.progressBar1.Maximum = (int)resp.ContentLength;
            });

            // get response stream
            this.DownloadReader = resp.GetResponseStream();

            // read async
            this.DownloadReader.BeginRead(this.DownloadReadBuffer, 0, this.DownloadReadBuffer.Length, this.DownloadReadCallback, null);
          }, null);
        }
      }
      this.GetFiles();
    }

    private void DownloadReadCallback(IAsyncResult result)
    {
      var bytesRead = this.DownloadReader.EndRead(result);

      if (bytesRead > 0)
      {
        this.SafeInvoke(() =>
        {
          this.progressBar1.Increment(bytesRead);
        });

        if (this.DownloadFileStream.CanWrite)
        {
          // write to file
          this.DownloadWriter.Write(this.DownloadReadBuffer.Take(bytesRead).ToArray());

          // read next part
          this.DownloadReader.BeginRead(this.DownloadReadBuffer, 0, this.DownloadReadBuffer.Length, DownloadReadCallback, null);
        }
      }
      else
      {
        this.DownloadFileStream.Close();

        this.SafeInvoke(() =>
        {
          this.progressBar1.Value = this.progressBar1.Maximum;
        });
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) { return; }

      // send file
      var fs = new FileStream(this.openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

      // get file length for progressbar
      this.UploadingFileLength = fs.Length;
      this.progressBar1.Value = 0;
      this.progressBar1.Maximum = 100;

      var fileInfo = UniValue.Empty;
      fileInfo["path"] = (String.IsNullOrEmpty(this.CurrentPath) ? "/" : "") + Path.Combine(this.CurrentPath, Path.GetFileName(this.openFileDialog1.FileName)).Replace("\\", "/");
      fileInfo["mode"] = "add";
      fileInfo["autorename"] = true;
      fileInfo["mute"] = false;

      OAuthUtility.PostAsync
      (
        "https://content.dropboxapi.com/2/files/upload",
        new HttpParameterCollection
        { 
          { fs } // content of the file
        },
        headers: new NameValueCollection { { "Dropbox-API-Arg", fileInfo.ToString() } },
        contentType: "application/octet-stream",
        authorization: this.Authorization,
        // handler of result
        callback: this.Upload_Result,
        // handler of uploading
        streamWriteCallback: this.Upload_Processing
      );
    }

    private void Upload_Processing(object sender, StreamWriteEventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<object, StreamWriteEventArgs>(this.Upload_Processing), sender, e);
        return;
      }

      progressBar1.Value = Math.Min(Convert.ToInt32(Math.Round((e.TotalBytesWritten * 100.0) / this.UploadingFileLength)), 100);
    }

    private void Upload_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(this.Upload_Result), result);
        return;
      }

      if (result.StatusCode == 200)
      {
        this.GetFiles();
      }
      else
      {
        if (result["error_summary"].HasValue)
        {
          MessageBox.Show(result["error_summary"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          MessageBox.Show(result.ToString(), "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }

    private void SafeInvoke(Action action)
    {
      try
      {
        if (this.IsDisposed)
        {
          return;
        }

        if (this.InvokeRequired)
        {
          this.Invoke(new Action<Action>(this.SafeInvoke), action);
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
