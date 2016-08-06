using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nemiro.OAuth;
using Nemiro.OAuth.LoginForms;

namespace TwitterExample
{

  public partial class Form1 : Form
  {

    private Properties.Settings Settings = Properties.Settings.Default;

    private int MediaUploadChunkSize = (3 * 1024 * 1024); // 3 MB

    public string LastTweetId { get; set; }

    public Form1()
    {
      InitializeComponent();
    }

    private OAuthAuthorization GetAuth()
    {
      OAuthAuthorization auth = new OAuthAuthorization();
      auth.ConsumerKey = this.Settings.ConsumerKey;
      auth.ConsumerSecret = this.Settings.ConsumerSecret;
      auth.SignatureMethod = SignatureMethods.HMACSHA1;
      auth.Token = this.Settings.AccessToken;
      auth.TokenSecret = this.Settings.TokenSecret;

      return auth;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(this.Settings.AccessToken) || String.IsNullOrEmpty(this.Settings.TokenSecret) || String.IsNullOrEmpty(this.Settings.UserId))
      {
        // access token is empty
        // get access token
        this.GetAccessToken();
      }
      else
      {
        // check token
        this.CheckAccessToken();
      }
    }

    private void btnAddPhoto_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      this.MediaUploadInit(openFileDialog1.FileName);
    }

    private void GetAccessToken()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(GetAccessToken));
        return;
      }

      var login = new TwitterLogin
      (
        this.Settings.ConsumerKey, 
        this.Settings.ConsumerSecret
      );

      login.Owner = this;

      login.ShowDialog();

      if (login.IsSuccessfully)
      {
        // save access token to application settings
        this.Settings.AccessToken = ((OAuthAccessToken)login.AccessToken).Value;
        this.Settings.TokenSecret = ((OAuthAccessToken)login.AccessToken).TokenSecret;
        this.Settings.UserId = login.AccessToken["user_id"].ToString();
        this.Settings.Save();

        // get tweets
        //this.GetTweets();
      }
      else
      {
        if (MessageBox.Show("Please Click OK to login on Twitter or CANCEL for exit from the program.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
        {
          this.Close();
        }
        else
        {
          this.GetAccessToken();
        }
      }
    }

    private void CheckAccessToken()
    {
      //this.RequestStart();

      //btnMore.Enabled = false;
      //btnTweet.Enabled = false;

      var parameters = new HttpParameterCollection
      {
        new HttpUrlParameter("user_id", this.Settings.UserId),
        new HttpUrlParameter("include_entities", "false")
      };

      OAuthUtility.GetAsync
      (
        "https://api.twitter.com/1.1/users/show.json",
        parameters: parameters, 
        authorization: this.GetAuth(),
        callback: CheckAccessToken_Result
      );
    }

    private void CheckAccessToken_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(CheckAccessToken_Result), result);
        return;
      }

      if (!result.IsSuccessfully)
      {
        this.Settings.AccessToken = "";
        this.Settings.TokenSecret = "";
        this.Settings.Save();

        this.GetAccessToken();
      }
      else
      {
        //this.GetTweets();
      }
    }

    private void btnTweet_Click(object sender, EventArgs e)
    {
      HttpParameterCollection parameters = new HttpParameterCollection();
      parameters.AddFormParameter("status", this.Message.Text);

      // images
      if (UploadedImages.Controls.Count > 0)
      {
        var media_ids = new List<string>();

        foreach (PictureBox image in UploadedImages.Controls)
        {
          media_ids.Add(image.Tag.ToString());
        }

        // add to query
        parameters.AddFormParameter("media_ids", String.Join(",", media_ids));
      }

      OAuthUtility.PostAsync
      (
        "https://api.twitter.com/1.1/statuses/update.json",
        parameters: parameters, 
        authorization: this.GetAuth(),
        callback: SendTweet_Result,
        contentType: "application/x-www-form-urlencoded"
      );
    }

    private void SendTweet_Result(RequestResult result)
    {
      /*if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(SendTweet_Result), result);
        return;
      }*/

      if (result.IsSuccessfully)
      {
        /*TwitterItem tweet = new TwitterItem();
        //{
        //    Dock = DockStyle.Top,
        //    Username = string.Format("@{0}", result["user"]["screen_name"]),
        //    Nickname = result["user"]["name"],
        //    DateCreated = System.DateTime.ParseExact(result["created_at"], "ddd MMM dd HH:mm:ss zzzz yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString(),
        //    Text = result["text"]
        //};
        pnlList.Controls.Add(tweet);
        pnlList.Controls.SetChildIndex(tweet, 0);
        pnlList.ScrollControlIntoView(pnlList.Controls[0]);
        tbMessage.Text = tbMessage.Tag.ToString();*/
      }

      //RequestEnd(result);

      //tbMessage.Enabled = true;
      //btnTweet.Enabled = true;
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/media/upload-init
    /// </summary>
    /// <param name="path">File path.</param>
    private void MediaUploadInit(string path)
    {
      var file = new FileInfo(path);

      string media_type = "image/jpeg";

      switch (file.Extension.ToLower())
      {
        case ".png":
          media_type = "image/png";
          break;
        case ".gif":
          media_type = "image/gif";
          break;
        case ".bmp":
          media_type = "image/bmp";
          break;
      }

      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("command", "INIT");
      parameters.AddFormParameter("total_bytes", file.Length.ToString());
      parameters.AddFormParameter("media_type", media_type);
      parameters.AddFormParameter("media_category", "tweet_image");

      OAuthUtility.PostAsync
      (
        "https://upload.twitter.com/1.1/media/upload.json",
        parameters: parameters, authorization: this.GetAuth(),
        contentType: "multipart/form-data",
        callback: (RequestResult result) =>
        {
          if (result.IsSuccessfully)
          {
            this.MeadiaUploadAppend(path, result["media_id"].ToString());
          }
          else
          {
            this.ErrorResult(result);
          }
        }
      );
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/media/upload-append
    /// </summary>
    /// <param name="path">File path.</param>
    /// <param name="media_id">Media id.</param>
    /// <param name="chunk">Chunk. Default: 0.</param>
    private void MeadiaUploadAppend(string path, string media_id, int chunk = 0)
    {
      var file = new FileInfo(path);
      bool isUploded = false;
      byte[] media = null;

      if (chunk > 0)
      {
        // multiple chunks
        using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Inheritable))
        using (var reader = new BinaryReader(stream))
        {
          stream.Position = (this.MediaUploadChunkSize * chunk); // + 1;
          media = reader.ReadBytes(this.MediaUploadChunkSize);
          isUploded = (stream.Position == stream.Length);
        }
      }
      else
      {
        if (file.Length <= this.MediaUploadChunkSize)
        {
          // one chunk
          using (var reader = new BinaryReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Inheritable)))
          {
            media = reader.ReadBytes(Convert.ToInt32(file.Length));
            isUploded = true;
          }
        }
        else
        {
          // multiple chunks
          using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Inheritable))
          using (var reader = new BinaryReader(stream))
          {
            media = reader.ReadBytes(this.MediaUploadChunkSize);
            isUploded = (stream.Position == stream.Length);
          }
        }
      }

      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("command", "APPEND");
      parameters.AddFormParameter("media_id", media_id);
      parameters.AddFormParameter("segment_index", chunk.ToString());
      parameters.Add("media", Path.GetFileName(path), media);
      
      OAuthUtility.PostAsync
      (
        "https://upload.twitter.com/1.1/media/upload.json",
        parameters: parameters, authorization: this.GetAuth(),
        contentType: "multipart/form-data",
        callback: (RequestResult result) =>
        {
          if (!result.IsSuccessfully)
          {
            // error
            this.ErrorResult(result);
            return;
          }

          if (file.Length > this.MediaUploadChunkSize && !isUploded)
          {
            // next chunk
            this.MeadiaUploadAppend(path, media_id, chunk + 1);
          }
          else
          {
            // finalize
            this.MeadiaUploadFinalize(path, media_id);
          }
        }
      );
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/media/upload-finalize
    /// </summary>
    /// <param name="media_id">Media id.</param>
    private void MeadiaUploadFinalize(string path, string media_id)
    {
      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("command", "FINALIZE");
      parameters.AddFormParameter("media_id", media_id);

      OAuthUtility.PostAsync
      (
        "https://upload.twitter.com/1.1/media/upload.json",
        parameters: parameters, authorization: this.GetAuth(),
        contentType: "multipart/form-data",
        callback: (RequestResult result) =>
        {
          if (result.IsSuccessfully)
          {
            // ok
            this.MeadiaUploadFinalize_Result(path, result);
          }
          else
          {
            // error
            this.ErrorResult(result);
          }
        }
      );
    }

    private void MeadiaUploadFinalize_Result(string path, RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<string, RequestResult>(MeadiaUploadFinalize_Result), path, result);
        return;
      }

      // add photo to uploaded list
      var image = new PictureBox();
      image.Height = UploadedImages.Height;
      image.Width = UploadedImages.Height;
      image.SizeMode = PictureBoxSizeMode.StretchImage;

      // save media_id to tag
      image.Tag = result["media_id"].ToString();

      using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Inheritable))
      {
        image.Image = Image.FromStream(file);
      }

      UploadedImages.Controls.Add(image);
    }

    private void ErrorResult(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(ErrorResult), result);
        return;
      }

      if (result["errors"].HasValue)
      {
        MessageBox.Show(String.Join("\r\n", result["errors"].Select(itm => itm["message"].ToString())), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      else
      {
        MessageBox.Show(result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }

}
