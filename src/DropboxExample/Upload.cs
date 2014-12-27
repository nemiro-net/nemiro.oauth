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
using System.IO;

namespace DropboxExample
{

  public partial class Upload : Form
  {

    public Dictionary<string, string> Files { get; protected set; }

    public Upload()
    {
      InitializeComponent();
    }

    public Upload(string[] files, string serverPath) : this()
    {
      this.Files = new Dictionary<string, string>();
      foreach (string path in files)
      {
        if (!this.Files.ContainsKey(path) && (File.GetAttributes(path) != FileAttributes.Directory || (File.GetAttributes(path) == FileAttributes.Directory && Directory.GetFiles(path).Length <= 0 && Directory.GetDirectories(path).Length <= 0)))
        {
          this.Files.Add(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
        if (File.GetAttributes(path) == FileAttributes.Directory)
        {
          this.GetAllFiles(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
      }
    }

    private void Upload_Load(object sender, EventArgs e)
    {
      progressBar1.Maximum = this.Files.Count;

      foreach (var file in this.Files)
      {
        this.UploadFile(file.Key, file.Value);
      }
    }

    private void GetAllFiles(string localPath, string serverPath)
    {
      // add subfolders
      foreach (string path in Directory.GetDirectories(localPath))
      {
        if (Directory.GetFiles(path).Length <= 0 && Directory.GetDirectories(path).Length <= 0)
        {
          // add empty folder
          this.Files.Add(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
        else
        {
          // add all files
          this.GetAllFiles(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
      }
      // add files
      foreach (string path in Directory.GetFiles(localPath))
      {
        if (!this.Files.ContainsKey(path))
        {
          this.Files.Add(path, Path.Combine(serverPath, Path.GetFileName(path)));
        }
      }
    }

    private void UploadFile(string localPath, string serverPath)
    {
      serverPath = serverPath.Replace("\\", "/");

      if (File.GetAttributes(localPath) == FileAttributes.Directory)
      {
        // create folder
        OAuthUtility.PostAsync
        (
          "https://api.dropbox.com/1/fileops/create_folder",
          new HttpParameterCollection
          { 
            { "access_token" , Properties.Settings.Default.AccessToken },
            { "root", "auto" },
            { "path", serverPath },
          },
          callback: UploadFile_Result
        );
      }
      else
      {
        // send file
        var fs = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        OAuthUtility.PutAsync
        (
          "https://api-content.dropbox.com/1/files_put/auto/",
          new HttpParameterCollection
          { 
            { "access_token" , Properties.Settings.Default.AccessToken },
            { "overwrite", "false" },
            { "autorename", "false" },
            { "path", serverPath },
            { fs } // content of the file
          },
          callback: UploadFile_Result
        );
      }
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
        if (result["error"].HasValue)
        {
          MessageBox.Show(result["error"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
        
  }

}
