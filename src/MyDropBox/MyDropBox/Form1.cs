using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nemiro.OAuth;
using Nemiro.OAuth.LoginForms;
using System.IO;
using System.Net;

// http://www.youtube.com/watch?v=fcT-Jt8rcdY

namespace MyDropBox
{
  public partial class Form1 : Form
  {

    private string CurrentPath = "/";

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
        this.GetFiles();
      }
    }

    private void GetAccessToken()
    {
      var login = new DropboxLogin("4mlpoeq657vuif8", "1whj6c5mxtkns7m");
      login.Owner = this;
      login.ShowDialog();

      if (login.IsSuccessfully)
      {
        Properties.Settings.Default.AccessToken = login.AccessToken.Value;
        Properties.Settings.Default.Save();
        this.GetFiles();
      }
      else
      {
        MessageBox.Show("error...");
      }
    }

    private void GetFiles()
    {
      OAuthUtility.GetAsync
      (
        "https://api.dropbox.com/1/metadata/auto/",
        new HttpParameterCollection
        {
          { "path", this.CurrentPath },
          { "access_token", Properties.Settings.Default.AccessToken }
        },
        callback: GetFiles_Result
      );
    }

    private void GetFiles_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(GetFiles_Result), result);
        return;
      }

      if (result.StatusCode == 200)
      {

        listBox1.Items.Clear();

        listBox1.DisplayMember = "path";

        foreach (UniValue file in result["contents"])
        {
          listBox1.Items.Add(file);
        }

        if (this.CurrentPath != "/")
        {
          listBox1.Items.Insert(0, UniValue.ParseJson("{path: '..'}"));
        }

      }
      else
      {
        MessageBox.Show("Error...");
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      OAuthUtility.PostAsync
      (
        "https://api.dropbox.com/1/fileops/create_folder",
        new HttpParameterCollection
        {
          {"access_token", Properties.Settings.Default.AccessToken},
          {"root", "auto"},
          {"path", Path.Combine(this.CurrentPath, textBox1.Text).Replace("\\", "/")}
        },
        callback: CreateFolder_Result
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
      UniValue file = (UniValue)listBox1.SelectedItem;

      if (file["path"] == "..")
      {
        if (this.CurrentPath != "/")
        {
          this.CurrentPath = Path.GetDirectoryName(this.CurrentPath).Replace("\\", "/");
        }
      }
      else
      {
        if (file["is_dir"] == 1)
        {
          this.CurrentPath = file["path"].ToString();
        }
        else
        {
          saveFileDialog1.FileName = Path.GetFileName(file["path"].ToString());
          if (saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
          {
            return;
          }
          var web = new WebClient();
          web.DownloadProgressChanged += DownloadProgressChanged;
          web.DownloadFileAsync(new Uri(String.Format("https://api-content.dropbox.com/1/files/auto{0}?access_token={1}", file["path"], Properties.Settings.Default.AccessToken)), saveFileDialog1.FileName);
        }
      }
      this.GetFiles();
    }

    private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
      progressBar1.Value = e.ProgressPercentage;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) { return; }

      OAuthUtility.PutAsync
      (
        "https://api-content.dropbox.com/1/files_put/auto/",
        new HttpParameterCollection
        {
          {"access_token", Properties.Settings.Default.AccessToken},
          {"path", Path.Combine(this.CurrentPath, Path.GetFileName(openFileDialog1.FileName)).Replace("\\", "/")},
          {"overwrite", "true"},
          {"autorename","true"},
          {openFileDialog1.OpenFile()}
        },
        callback: Upload_Result
      );

    }

    private void Upload_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(Upload_Result), result);
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
          MessageBox.Show(result["error"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          MessageBox.Show(result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }
     
  }
}
