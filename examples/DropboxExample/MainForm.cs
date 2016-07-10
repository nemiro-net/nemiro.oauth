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
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nemiro.OAuth;
using System.IO;
using Nemiro.OAuth.LoginForms;

namespace DropboxExample
{

  public partial class MainForm : Form
  {

    // https://www.dropbox.com/developers/core/docs

    private string _CurrentPath = "";

    public string CurrentPath
    {
      get
      {
        return _CurrentPath;
      }
      set
      {
        _CurrentPath = value;
        this.UpdateList();
      }
    }

    public MainForm()
    {
      InitializeComponent();
    }
    
    private void Form1_Load(object sender, EventArgs e)
    {
      listView1_SelectedIndexChanged(listView1, null);

      if (String.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
      {
        // access token is empty
        // get access token
        this.GetAccessToken();
      }
      else
      {
        // check access token
        this.CheckAccessToken();
      }
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      btnDownload_Click(sender, e);
    }

    private void listView1_ItemActivate(object sender, EventArgs e)
    {
      listView1_SelectedIndexChanged(sender, e);
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnCreateFolder.Enabled = true;
      btnUpload.Enabled = true;

      if (listView1.FocusedItem == null || listView1.FocusedItem.Tag == null)
      {
        btnRename.Enabled = btnRemove.Enabled = false;
        mnuRename.Enabled = mnuRemove.Enabled = false;
        btnDownload.Enabled = false;
        mnuDownload.Enabled = false;
      }
      else
      {
        btnRename.Enabled = btnRemove.Enabled = true;
        mnuRename.Enabled = mnuRemove.Enabled = true;
        btnDownload.Enabled = mnuDownload.Enabled = !Convert.ToBoolean(((UniValue)listView1.FocusedItem.Tag)["is_dir"]);
      }
    }

    private void btnDownload_Click(object sender, EventArgs e)
    {
      if (listView1.FocusedItem == null)
      {
        btnDownload.Enabled = mnuDownload.Enabled = false;
        return;
      }

      if (listView1.FocusedItem.Tag == null)
      {
        this.CurrentPath = (Path.GetDirectoryName(this.CurrentPath) == "\\" ? "" : Path.GetDirectoryName(this.CurrentPath));
      }
      else
      {
        UniValue itm = (UniValue)listView1.FocusedItem.Tag;

        if (Convert.ToBoolean(itm["is_dir"]))
        {
          this.CurrentPath = itm["path"].ToString();
        }
        else
        {
          new Download
          (
            String.Format("https://api-content.dropbox.com/1/files/auto{0}", itm["path"])
          ) { Owner = this }.Show();
        }
      }
    }

    private void btnUpload_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
      {
        return;
      }

      new Upload(this.openFileDialog1.FileNames, this.CurrentPath) { Owner = this }.ShowDialog();
    }

    private void btnCreateFolder_Click(object sender, EventArgs e)
    {
      InputBox ib = null;

      if (btnCreateFolder.Tag != null)
      {
        ib = btnCreateFolder.Tag as InputBox;
      }
      else
      {
        ib = new InputBox("Create folder", "Please enter a folder name:", "") { Owner = this };
      }

      if (ib.ShowDialog() != System.Windows.Forms.DialogResult.OK)
      {
        return;
      }

      if (ib.Value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
      {
        MessageBox.Show("The name contains invalid characters.\r\nPlease check the name and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        btnCreateFolder.Tag = ib;
        btnCreateFolder_Click(sender, e);
        return;
      }

      btnCreateFolder.Tag = null;

      this.CreateFolder(ib.Value);    
    }

    private void btnRename_Click(object sender, EventArgs e)
    {
      if (listView1.FocusedItem == null || listView1.FocusedItem.Tag == null)
      {
        btnRename.Enabled = mnuRename.Enabled = false;
        return;
      }

      var file = ((UniValue)listView1.FocusedItem.Tag);
      var name = Path.GetFileName(file["path"].ToString());
      InputBox ib = null;

      if (btnRename.Tag != null)
      {
        ib = btnRename.Tag as InputBox;
      }
      else
      {
        ib = new InputBox("Rename", "Please enter a new name:", name) { Owner = this };
      }

      if (ib.ShowDialog() != System.Windows.Forms.DialogResult.OK || ib.Value.Equals(name, StringComparison.OrdinalIgnoreCase))
      {
        return;
      }

      if (ib.Value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
      {
        MessageBox.Show("The name contains invalid characters.\r\nPlease check the name and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        btnRename.Tag = ib;
        btnRename_Click(sender, e);
        return;
      }

      btnRename.Tag = null;

      this.Rename(file["path"].ToString(), Path.Combine(Path.GetDirectoryName(file["path"].ToString()), ib.Value));
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (listView1.FocusedItem == null || listView1.FocusedItem.Tag == null)
      {
        btnRemove.Enabled = mnuRemove.Enabled = false;
        return;
      }

      var name = Path.GetFileName(((UniValue)listView1.FocusedItem.Tag)["path"].ToString());

      if (MessageBox.Show(String.Format("Are you want delete \"{0}\"?", name), "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
      {
        return;
      }

      this.Delete(((UniValue)listView1.FocusedItem.Tag)["path"].ToString());
    }

    private void MainForm_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        e.Effect = DragDropEffects.Copy;
      }
    }

    private void MainForm_DragDrop(object sender, DragEventArgs e)
    {
      string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

      if (files.Length <= 0) { return; }

      new Upload(files, this.CurrentPath) { Owner = this }.ShowDialog();
    }

    #region dropbox api
    
    private void GetAccessToken()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(GetAccessToken));
        return;
      }

      // create login form
      var login = new DropboxLogin("5nkunr8uscwfoba", "n7x9icfwoe6dehq") { Owner = this };

      // show login form
      login.ShowDialog();

      // authorization is success
      if (login.IsSuccessfully)
      {
        // save the access token to the application settings
        Properties.Settings.Default.AccessToken = login.AccessToken.Value;
        Properties.Settings.Default.Save();
        // update the list of files
        this.UpdateList();
      }
      // is fails
      else
      {
        if (MessageBox.Show("Please Click OK to login on Dropbox or CANCEL for exit from the program.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
        {
          this.Close();
        }
        else
        {
          this.GetAccessToken();
        }
      }
    }

    /// <summary>
    /// Checks the validity of the access token.
    /// </summary>
    private void CheckAccessToken()
    {
      this.RequestStart();

      OAuthUtility.GetAsync
      (
        "https://api.dropbox.com/1/account/info",
        new HttpParameterCollection 
        { 
          { "access_token", Properties.Settings.Default.AccessToken }
        },
        callback: CheckAccessToken_Result
      );
    }
    private void CheckAccessToken_Result(RequestResult result)
    {
      if (result.StatusCode == 401)
      {
        // error, show login form
        this.GetAccessToken();
        // hide progress
        this.RequestEnd(new RequestResult(statusCode: 200));
      }
      else
      {
        // success
        //this.RequestEnd(result);
        this.UpdateList();
      }
    }

    /// <summary>
    /// Gets list of folders and files from Dropbox.
    /// </summary>
    public void UpdateList()
    {
      this.RequestStart();

      OAuthUtility.GetAsync
      (
        "https://api.dropbox.com/1/metadata/auto/",
        new HttpParameterCollection 
        { 
          { "path", this.CurrentPath.Replace("\\", "/") },
          { "access_token", Properties.Settings.Default.AccessToken }
        },
        callback: UpdateList_Result
      );
    }
    private void UpdateList_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(UpdateList_Result), result);
        return;
      }

      listView1.Items.Clear();

      if (result.StatusCode == 200)
      {
        var fileList = result["contents"].OrderBy(itm => Path.GetFileName(itm["path"].ToString())).OrderByDescending(itm => Convert.ToBoolean(itm["is_dir"]));
        foreach (UniValue file in fileList)
        {
          var item = new ListViewItem();
          item.ImageKey = file["icon"].ToString();
          item.SubItems.Add(Path.GetFileName(file["path"].ToString()));
          item.SubItems.Add(file["size"].ToString());
          item.SubItems.Add(file["mime_type"].ToString());
          item.Tag = file;
          listView1.Items.Add(item);
        }

        if (!String.IsNullOrEmpty(this.CurrentPath))
        {
          var root = new ListViewItem();
          root.ImageKey = "folder";
          root.SubItems.Add("..");
          listView1.Items.Insert(0, root);
        }
      }

      listView1_SelectedIndexChanged(listView1, null);

      this.RequestEnd(result);
    }

    private void Delete(string path)
    {
      this.RequestStart();

      OAuthUtility.PostAsync
      (
        "https://api.dropbox.com/1/fileops/delete",
        new HttpParameterCollection 
        { 
          { "access_token", Properties.Settings.Default.AccessToken },
          { "root", "auto" },
          { "path", path.Replace("\\", "/") }
        },
        callback: FileOperation_Result
      );
    }
    
    private void Rename(string oldPath, string newPath)
    {
      this.RequestStart();
      
      OAuthUtility.PostAsync
      (
        "https://api.dropbox.com/1/fileops/move",
        new HttpParameterCollection 
        { 
          { "access_token", Properties.Settings.Default.AccessToken },
          { "root", "auto" },
          { "from_path", oldPath.Replace("\\", "/") },
          { "to_path", newPath.Replace("\\", "/") }
        },
        callback: FileOperation_Result
      );
    }

    private void CreateFolder(string name)
    {
      this.RequestStart();

      OAuthUtility.PostAsync
      (
        "https://api.dropbox.com/1/fileops/create_folder",
        new HttpParameterCollection
        { 
          { "access_token" , Properties.Settings.Default.AccessToken },
          { "root", "auto" },
          { "path", Path.Combine(this.CurrentPath, name).Replace("\\", "/") },
        },
        callback: FileOperation_Result
      );
    }

    private void FileOperation_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(FileOperation_Result), result);
        return;
      }

      if (result.StatusCode == 200)
      {
        this.UpdateList();
      }
      else
      {
        this.RequestEnd(result);
      }
    }

    private void RequestStart()
    {
      lblStatus.Text = "Please Wait...";
      picStatus.Image = Properties.Resources.loader2;
    }

    private void RequestEnd(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(RequestEnd), result);
        return;
      }

      if (result.StatusCode < 200 || result.StatusCode > 299)
      {
        if (result["error"].HasValue)
        {
          lblStatus.Text = result["error"].ToString();
          MessageBox.Show(result["error"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          lblStatus.Text = result.ToString();
          MessageBox.Show(result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        picStatus.Image = Properties.Resources.error;
      }
      else
      {
        lblStatus.Text = "OK";
        picStatus.Image = Properties.Resources.success;
      }
    }

    #endregion

  }

}
