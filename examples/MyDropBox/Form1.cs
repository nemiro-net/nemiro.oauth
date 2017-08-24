using System;
using System.Text;
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
      var login = new DropboxLogin("4mlpoeq657vuif8", "1whj6c5mxtkns7m", "https://oauthproxy.nemiro.net/", false, false);
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

      using(var file = openFileDialog1.OpenFile())
      using (var reader = new BinaryReader(file))
      {

        //reader.ReadBytes(fi
        var client = new WebClient();

        // handlers
        client.UploadProgressChanged += (s, ce) =>
        {
          // you can create progress here
          Console.WriteLine("Uploading: {0}", ce.ProgressPercentage);
        };

        client.UploadFileCompleted += (s, ce) =>
        {
          // exception
          if (ce.Error != null)
          {
            MessageBox.Show(ce.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
          }

          // parse success result
          var result = UniValue.ParseJson(Encoding.UTF8.GetString(ce.Result));
          if (result["error"].HasValue)
          {
            MessageBox.Show(result["error"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
            MessageBox.Show(result.ToString(), "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        };

        var url = new UriBuilder("https://api-content.dropbox.com/1/files_put/auto/");
        url.Query = String.Format("access_token={0}", Properties.Settings.Default.AccessToken) + // your access token here
                    String.Format("&path={0}", Path.Combine(this.CurrentPath, Path.GetFileName(openFileDialog1.FileName)).Replace("\\", "/")) +
                    String.Format("&overwrite={0}", "true") +
                    String.Format("&autorename={0}", "true");

        client.UploadFileAsync
        (
          url.Uri,
          "PUT",
          openFileDialog1.FileName, 
          null
        );
      }

      // small files
      if (false)
      {
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
          MessageBox.Show(result.ToString(), "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }
     
  }
}
