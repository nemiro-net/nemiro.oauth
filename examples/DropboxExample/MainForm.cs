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

    // https://www.dropbox.com/developers/documentation/http/documentation

    private string _CurrentPath = "";

    private HttpAuthorization Authorization = null;

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
        btnRename.Enabled = mnuRename.Enabled = false;
        btnRemove.Enabled = mnuRemove.Enabled = false;
        btnDownload.Enabled = mnuDownload.Enabled = false;
      }
      else
      {
        btnRename.Enabled = mnuRename.Enabled = true;
        btnRemove.Enabled = mnuRemove.Enabled = true;
        btnDownload.Enabled = mnuDownload.Enabled = !((UniValue)listView1.FocusedItem.Tag)[".tag"].Equals("folder");
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

        if (itm[".tag"].Equals("folder"))
        {
          this.CurrentPath = itm["path_lower"].ToString();
        }
        else
        {
          new Download(itm["path_lower"]) { Owner = this }.Show();
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
      var name = file["name"].ToString();
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

      this.Rename(listView1.FocusedItem, Path.Combine(Path.GetDirectoryName(file["path_display"].ToString()), ib.Value));
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (listView1.FocusedItem == null || listView1.FocusedItem.Tag == null)
      {
        btnRemove.Enabled = mnuRemove.Enabled = false;
        return;
      }

      var name = ((UniValue)listView1.FocusedItem.Tag)["name"].ToString();

      if (MessageBox.Show(String.Format("Are you want delete \"{0}\"?", name), "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
      {
        return;
      }

      this.Delete(listView1.FocusedItem);
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      this.UpdateList();
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

    private string GetIconByFileExtension(string value)
    {
      value = " " + value + " ";

      if (" .wav .mp3 .ogg .mid .midi ".IndexOf(value) != -1)
      {
        return "page_white_sound";
      }
      else if (" .zip .7z .rar .gzip .tar .bz ".IndexOf(value) != -1)
      {
        return "page_white_compressed";
      }
      else if (" .txt .log ".IndexOf(value) != -1)
      {
        return "page_white_text";
      }
      else if (" .bmp .jpg .jpeg .gif .png ".IndexOf(value) != -1)
      {
        return "page_white_picture";
      }
      else if (" .doc .docx .rtf ".IndexOf(value) != -1)
      {
        return "page_white_word";
      }
      else if (" .xls .xlsx ".IndexOf(value) != -1)
      {
        return "page_white_excel";
      }

      return "page_white";
    }

    private string GetSizeInfo(double size)
    {
      string[] sizes = { "B", "KB", "MB", "GB" };

      int order = 0;

      while (size >= 1024 && ++order < sizes.Length)
      {
        size = size / 1024.0;
      }

      return String.Format("{0:0.##} {1}", size, sizes[order]);
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
      var login = new DropboxLogin("5nkunr8uscwfoba", "n7x9icfwoe6dehq", "https://oauthproxy.nemiro.net/", false, false) { Owner = this };

      // show login form
      login.ShowDialog();

      // authorization is success
      if (login.IsSuccessfully)
      {
        // save the access token to the application settings
        Properties.Settings.Default.AccessToken = login.AccessToken.Value;
        Properties.Settings.Default.Save();

        this.Authorization = new HttpAuthorization(AuthorizationType.Bearer, Properties.Settings.Default.AccessToken);

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

      this.Authorization = new HttpAuthorization(AuthorizationType.Bearer, Properties.Settings.Default.AccessToken);

      OAuthUtility.PostAsync
      (
        "https://api.dropboxapi.com/2/users/get_current_account",
        authorization: this.Authorization,
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
        this.UpdateList();
      }
    }

    /// <summary>
    /// Gets list of folders and files from Dropbox.
    /// </summary>
    public void UpdateList()
    {
      this.RequestStart();

      OAuthUtility.PostAsync
      (
        "https://api.dropboxapi.com/2/files/list_folder",
        parameters: new HttpParameterCollection
        {
          new {
            path = this.CurrentPath.Replace("\\", "/"),
            include_media_info = true
          }
        },
        contentType: "application/json",
        authorization: this.Authorization,
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
        var fileList = result["entries"].OrderBy(itm => itm["name"].ToString()).OrderByDescending(itm => itm[".tag"].Equals("folder"));
        foreach (UniValue file in fileList)
        {
          this.AddItem(file);
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

    /// <summary>
    /// Adds file or folder to list.
    /// </summary>
    /// <param name="file">Dropbox file or folder.</param>
    private void AddItem(UniValue file)
    {
      var item = new ListViewItem();
      item.SubItems.Add(file["name"].ToString());

      if (!file[".tag"].Equals("folder"))
      {
        item.ImageKey = this.GetIconByFileExtension(Path.GetExtension(file["name"].ToString()));
        item.SubItems.Add(this.GetSizeInfo(Convert.ToDouble(file["size"])));
      }
      else
      {
        item.ImageKey = "folder";
      }

      item.SubItems.Add(file["mime_type"].ToString());
      item.Tag = file;

      listView1.Items.Add(item);
    }

    private void Delete(ListViewItem item)
    {
      this.RequestStart();

      var file = (UniValue)item.Tag;

      OAuthUtility.PostAsync
      (
        "https://api.dropboxapi.com/2/files/delete",
        new HttpParameterCollection 
        { 
          new { path = file["path_display"].ToString() }
        },
        contentType: "application/json",
        authorization: this.Authorization,
        callback: (result) =>
        {
          this.Delete_Result(result, item);
        }
      );
    }

    private void Delete_Result(RequestResult result, ListViewItem item)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult, ListViewItem>(Delete_Result), result, item);
        return;
      }

      if (result.StatusCode == 200)
      {
        if (item != null)
        {
          this.listView1.Items.Remove(item);
          this.RequestEnd(result);
        }
        else
        {
          this.UpdateList();
        }
      }
      else
      {
        this.RequestEnd(result);
      }
    }

    private void Rename(ListViewItem item, string newPath)
    {
      this.RequestStart();

      var file = (UniValue)item.Tag;

      OAuthUtility.PostAsync
      (
        "https://api.dropboxapi.com/2/files/move",
        new HttpParameterCollection 
        { 
          new
          {
            from_path = file["path_display"].ToString(),
            to_path = newPath.Replace("\\", "/")
          }
        },
        contentType: "application/json",
        authorization: this.Authorization,
        callback: (result) => 
        {
          this.Rename_Result(result, item);
        }
      );
    }

    private void Rename_Result(RequestResult result, ListViewItem item)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult, ListViewItem>(Rename_Result), result, item);
        return;
      }

      if (result.StatusCode == 200)
      {
        if (item != null)
        {
          item.Tag = result;
          item.SubItems[1].Text = result["name"].ToString();
          this.RequestEnd(result);
        }
        else
        {
          this.UpdateList();
        }
      }
      else
      {
        this.RequestEnd(result);
      }
    }

    private void CreateFolder(string name)
    {
      this.RequestStart();

      OAuthUtility.PostAsync
      (
        "https://api.dropboxapi.com/2/files/create_folder",
        new HttpParameterCollection
        { 
          new
          {
            path = ((String.IsNullOrEmpty(this.CurrentPath) ? "/" : "") + Path.Combine(this.CurrentPath, name)).Replace("\\", "/")
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
        this.Invoke(new Action<RequestResult>(CreateFolder_Result), result);
        return;
      }

      if (result.StatusCode == 200)
      {
        result[".tag"] = "folder";
        this.AddItem(result);
        this.RequestEnd(result);
      }
      else
      {
        this.RequestEnd(result);
      }
    }

    private void RequestStart()
    {
      btnRefresh.Enabled = mnuRefresh.Enabled = false;
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

      btnRefresh.Enabled = mnuRefresh.Enabled = true;

      if (result.StatusCode < 200 || result.StatusCode > 299)
      {
        if (result["error_summary"].HasValue)
        {
          lblStatus.Text = result["error_summary"].ToString();
        }
        else
        {
          lblStatus.Text = result.ToString();
        }

        MessageBox.Show(lblStatus.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
