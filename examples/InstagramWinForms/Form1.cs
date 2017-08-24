using System;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using Nemiro.OAuth;
using Nemiro.OAuth.LoginForms;

namespace InstagramWinForms
{

  public partial class Form1 : Form
  {

    private const string API_BASE_URL = "https://api.instagram.com/v1";

    private ImageList ImageList = new ImageList();

    private ListView ListView1 = new ListView();

    private string AccessToken
    {
      get
      {
        return Properties.Settings.Default.AccessToken;
      }
    }

    public Form1()
    {
      InitializeComponent();

      this.ImageList.ImageSize = new Size(150, 150);
      this.ImageList.ColorDepth = ColorDepth.Depth16Bit;

      this.ListView1.View = View.LargeIcon;
      this.ListView1.LargeImageList = this.ImageList;

      this.ListView1.Dock = DockStyle.Fill;

      this.Controls.Add(this.ListView1);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
      {
        this.GetAccessToken();
      }
      else
      {
        this.Text = Properties.Settings.Default.Username;
        this.GetRecentMedia();
      }
    }

    private void GetAccessToken()
    {
      var login = new InstagramLogin
      (
        "9fcad1f7740b4b66ba9a0357eb9b7dda", 
        "3f04cbf48f194739a10d4911c93dcece", 
        "https://oauthproxy.nemiro.net/",
        scope: "basic public_content",
        loadUserInfo: true
      );

      login.Owner = this;
      login.ShowDialog();

      if (login.IsSuccessfully)
      {
        Properties.Settings.Default.Username = this.Text = (login.UserInfo.DisplayName ?? login.UserInfo.UserName);
        Properties.Settings.Default.AccessToken = login.AccessToken.Value;
        Properties.Settings.Default.Save();

        this.GetRecentMedia();
      }
      else
      {
        MessageBox.Show("Error...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void GetRecentMedia()
    {
      OAuthUtility.GetAsync
      (
        String.Format("{0}/users/self/media/recent?access_token={1}", API_BASE_URL, this.AccessToken),
        callback: GetRecentMedia_Result
      );
    }

    private async void GetRecentMedia_Result(RequestResult result)
    {
      if (result.StatusCode == 200)
      {
        foreach (UniValue item in result["data"])
        {
          using (var client = new HttpClient())
          {
            var s = await client.GetStreamAsync(item["images"]["thumbnail"]["url"].ToString());
            Invoke(new Action(() => this.ImageList.Images.Add(Image.FromStream(s))));
          }

          var image = new ListViewItem();
          image.Text = item["caption"]["text"].ToString();
          image.ImageIndex = this.ImageList.Images.Count - 1;

          Invoke(new Action(() => this.ListView1.Items.Add(image)));
        }
      }
      else
      {
        this.ShowError(result);
      }
    }

    private void ShowError(RequestResult result)
    {
      if (result["meta"]["error_message"].HasValue)
      {
        MessageBox.Show(result["meta"]["error_message"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      else
      {
        MessageBox.Show(result.ToString(), "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.GetRecentMedia();
    }

  }

}