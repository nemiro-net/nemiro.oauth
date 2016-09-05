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
using System.Collections.Generic;
using System.Windows.Forms;
using Nemiro.OAuth;
using System.IO;
using System.Collections.Specialized;

namespace DropboxExample
{

  public partial class Upload : Form
  {

    private long CurrentFileLength = 0;
    private int TotalFiles = 0;
    private Dictionary<string, string>.Enumerator Files;

    private HttpAuthorization Authorization = new HttpAuthorization(AuthorizationType.Bearer, Properties.Settings.Default.AccessToken);

    public Upload()
    {
      InitializeComponent();
    }

    public Upload(string[] files, string serverPath) : this()
    {
      var list = new Dictionary<string, string>();

      foreach (string path in files)
      {
        if (!list.ContainsKey(path) && (!File.GetAttributes(path).HasFlag(FileAttributes.Directory) || (File.GetAttributes(path).HasFlag(FileAttributes.Directory) && Directory.GetFiles(path).Length <= 0 && Directory.GetDirectories(path).Length <= 0)))
        {
          list.Add(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }

        if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
        {
          this.GetAllFiles(list, path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
      }

      this.TotalFiles = list.Count;
      this.Files = list.GetEnumerator();
    }

    private void Upload_Load(object sender, EventArgs e)
    {
      progressBar1.Maximum = this.TotalFiles;
      this.UploadNextFile();
    }

    private void UploadNextFile()
    {
      this.Text = String.Format("Sending {0} of {1}...", progressBar1.Value + 1, progressBar1.Maximum);
      this.Files.MoveNext();
      this.UploadFile(this.Files.Current.Key, this.Files.Current.Value);
    }

    private void GetAllFiles(Dictionary<string, string> list, string localPath, string serverPath)
    {
      // add subfolders
      foreach (string path in Directory.GetDirectories(localPath))
      {
        if (Directory.GetFiles(path).Length <= 0 && Directory.GetDirectories(path).Length <= 0)
        {
          // add empty folder
          list.Add(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
        else
        {
          // add all files
          this.GetAllFiles(list, path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
      }

      // add files
      foreach (string path in Directory.GetFiles(localPath))
      {
        if (!list.ContainsKey(path))
        {
          list.Add(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
      }
    }

    private void UploadFile(string localPath, string serverPath)
    {
      progressBar2.Value = 0;

      serverPath = serverPath.Replace("\\", "/");

      if (!serverPath.StartsWith("/"))
      {
        serverPath = serverPath.Insert(0, "/");
      }

      if (File.GetAttributes(localPath) == FileAttributes.Directory)
      {
        // create folder
        OAuthUtility.PostAsync
        (
          "https://api.dropboxapi.com/2/files/create_folder",
          new HttpParameterCollection
          { 
            new { path = serverPath },
          },
          contentType: "application/json",
          authorization: this.Authorization,
          callback: UploadFile_Result
        );
      }
      else
      {
        // send file
        var fs = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        // get file length for progressbar
        this.CurrentFileLength = fs.Length;

        var fileInfo = UniValue.Empty;
        fileInfo["path"] = serverPath;
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
          callback: UploadFile_Result,
          // handler of uploading
          streamWriteCallback: UploadFile_Processing
        );
      }
    }

    private void UploadFile_Processing(object sender, StreamWriteEventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<object, StreamWriteEventArgs>(this.UploadFile_Processing), sender, e);
        return;
      }

      progressBar2.Value = Math.Min(Convert.ToInt32(Math.Round((e.TotalBytesWritten * 100.0) / this.CurrentFileLength)), 100);
    }

    private void UploadFile_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(UploadFile_Result), result);
        return;
      }

      if (result.StatusCode != 200)
      {
        if (result["error_summary"].HasValue)
        {
          MessageBox.Show(result["error_summary"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          MessageBox.Show(result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      progressBar1.Value++;

      if (progressBar1.Value == progressBar1.Maximum)
      {
        ((MainForm)this.Owner).UpdateList();
        this.Close();
      }
      else
      {
        this.UploadNextFile();
      }
    }
        
  }

}
