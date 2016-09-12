// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2016. All rights reserved.
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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Nemiro.OAuth;
using Nemiro.OAuth.LoginForms;

namespace TwitterExample
{

  public partial class MainForm : Form
  {

    private Properties.Settings Settings = Properties.Settings.Default;

    private string LastTweetId;

    private long CurrentUploadingSize = 0;

    public MainForm()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      Message_Leave(Message, null);

      if (String.IsNullOrEmpty(Settings.AccessToken) || String.IsNullOrEmpty(Settings.TokenSecret) || String.IsNullOrEmpty(Settings.UserId))
      {
        // access token is empty
        // get access token
        GetAccessToken();
      }
      else
      {
        // check token
        CheckAccessToken();
      }
    }

    private async void btnTweet_Click(object sender, EventArgs e)
    {
      if (Message.Tag == null)
      {
        MessageBox.Show("Text is required!", "Ouch!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      List<string> media_ids = null;

      // images
      if (UploadedImages.Controls.Count > 0)
      {
        media_ids = new List<string>();

        foreach (PictureBox image in UploadedImages.Controls)
        {
          media_ids.Add(image.Tag.ToString());
        }
      }

      SetEnabledStatus(false);
      SetStatus("Sending tweet...", Properties.Resources.loader2);

      var result = await TwitterApi.SendTweet(Message.Text, media_ids);

      if (result.IsSuccessfully)
      {
        var tweet = new Tweet(result);
        Tweets.Controls.Add(tweet);
        Tweets.ScrollControlIntoView(tweet);
        Message.Text = null;
        Message_Leave(Message, null);
        UploadedImages.Controls.Clear();

        SetEnabledStatus(true);
        SetStatus("Tweet successfully published!", Properties.Resources.success);
      }
      else
      {
        ErrorResult(result);
      }
    }

    private async void btnAddPhoto_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      SetEnabledStatus(false);
      SetStatus("Uploading photo...");

      // get file size for pgoresss
      CurrentUploadingSize = new FileInfo(openFileDialog1.FileName).Length;

      var result = await TwitterApi.UploadMedia(openFileDialog1.FileName, UploadMedia_Processing);

      if (result.IsSuccessfully)
      {
        // add photo to uploaded list
        var image = new PictureBox();
        image.Height = UploadedImages.Height;
        image.Width = UploadedImages.Height;
        image.SizeMode = PictureBoxSizeMode.StretchImage;

        // save media_id to tag
        image.Tag = result["media_id"].ToString();

        using (var file = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.Inheritable))
        {
          image.Image = Image.FromStream(file);
        }

        var delete = new Button();
        delete.BackColor = Color.Red;
        delete.Text = "X";
        delete.ForeColor = Color.White;
        delete.Font = new Font(delete.Font, FontStyle.Bold);
        delete.Padding = new Padding(0);
        delete.Margin = new Padding(0);
        delete.Width = 24;
        delete.Height = 24;
        delete.Top = 0;
        delete.Left = image.Width - delete.Width;
        delete.Visible = false;

        delete.Click += (sss, eee) =>
        {
          UploadedImages.Controls.Remove(((Button)sss).Parent);
          btnAddPhoto.Enabled = (UploadedImages.Controls.Count < 4);
        };

        image.MouseEnter += (sss, eee) =>
        {
          delete.Visible = true;
        };

        image.MouseLeave += (sss, eee) =>
        {
          if (image.GetChildAtPoint(image.PointToClient(Cursor.Position)) == null)
          {
            delete.Visible = false;
          }
        };

        image.Controls.Add(delete);

        UploadedImages.Controls.Add(image);

        SetEnabledStatus(true);
        SetStatus("Photo uploaded successfully!", Properties.Resources.success);

        btnAddPhoto.Enabled = (UploadedImages.Controls.Count < 4);
      }
      else
      {
        ErrorResult(result);
      }
    }

    private void UploadMedia_Processing(object sender, ProgressChangedEventArgs e)
    {
      SetStatus(String.Format("Uploading photo ({0}%)...", e.ProgressPercentage));
    }

    private void GetAccessToken()
    {
      if (InvokeRequired)
      {
        Invoke(new Action(GetAccessToken));
        return;
      }

      var login = new TwitterLogin
      (
        Settings.ConsumerKey, 
        Settings.ConsumerSecret
        , true // auto logout
      );

      login.Owner = this;
      
      login.ShowDialog();

      if (login.IsSuccessfully)
      {
        // save access token to application settings
        Settings.AccessToken = ((OAuthAccessToken)login.AccessToken).Value;
        Settings.TokenSecret = ((OAuthAccessToken)login.AccessToken).TokenSecret;
        Settings.UserId = login.AccessToken["user_id"].ToString();
        Settings.Save();

        // get tweets
        GetTweets();
      }
      else
      {
        if (MessageBox.Show("Please Click OK to login on Twitter or CANCEL for exit from the program.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
        {
          Close();
        }
        else
        {
          GetAccessToken();
        }
      }
    }

    private async void CheckAccessToken()
    {
      try
      {
        SetEnabledStatus(false);
        SetStatus("Checking token...");

        var success = await TwitterApi.CheckAccessToken();

        if (success)
        {
          SetStatus("Token is valid!", Properties.Resources.success);

          GetTweets();
        }
        else
        {
          // reset token
          Settings.AccessToken = "";
          Settings.TokenSecret = "";
          Settings.Save();

          SetStatus("Token is not valid.", Properties.Resources.error);

          // get new token
          GetAccessToken();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.Close();
      }
    }

    private async void GetTweets()
    {
      SetEnabledStatus(false);
      SetStatus("Loading tweets...");

      var result = await TwitterApi.GetTweets(LastTweetId);

      if (result.IsSuccessfully)
      {
        SetEnabledStatus(true);
        SetStatus("Tweets successfully received!", Properties.Resources.success);

        Tweet first = null;

        for (int i = 0; i <= result.Count - 1; i++)
        {
          if (i == 0 && !String.IsNullOrEmpty(LastTweetId))
          {
            continue;
          }

          var tweet = new Tweet(result[i]);

          Tweets.Controls.Add(tweet);
          Tweets.Controls.SetChildIndex(tweet, 0);

          LastTweetId = result[i]["id_str"].ToString();

          if (first == null)
          {
            first = tweet;
          }
        }

        Tweets.ScrollControlIntoView(first);
      }
      else
      {
        ErrorResult(result);
      }
    }

    /// <summary>
    /// Disable or enable buttons.
    /// </summary>
    /// <param name="enabled"></param>
    private void SetEnabledStatus(bool enabled)
    {
      btnAddPhoto.Enabled = btnTweet.Enabled = btnMore.Enabled = Message.Enabled = enabled;
    }

    /// <summary>
    /// Sets status to status bar.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="image"></param>
    private void SetStatus(string text, Image image = null)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<string, Image>(SetStatus), text, image);
        return;
      }

      lblStatus.Text = text;
      picStatus.Image = image ?? Properties.Resources.loader2;
    }

    /// <summary>
    /// Shows error message.
    /// </summary>
    private void ErrorResult(RequestResult result)
    {
      if (InvokeRequired)
      {
        Invoke(new Action<RequestResult>(ErrorResult), result);
        return;
      }

      string errorMessage = "";

      if (result["errors"].HasValue)
      {
        errorMessage = String.Join("\r\n", result["errors"].Select(itm => itm["message"].ToString()));
      }
      else
      {
        errorMessage = result.ToString();
      }

      MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      SetStatus(errorMessage, Properties.Resources.error);
      SetEnabledStatus(true);
    }

    private void Message_TextChanged(object sender, EventArgs e)
    {
      if (!String.IsNullOrEmpty(Message.Text) && !Message.Equals("What's happening?"))
      {
        Message.Tag = true;
      }
      else
      {
        Message.Tag = null;
      }
    }

    private void Message_Enter(object sender, EventArgs e)
    {
      if (Message.Tag == null)
      {
        Message.Text = "";
        Message.ForeColor = Color.Black;
      }
    }

    private void Message_Leave(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(Message.Text))
      {
        Message.Text = "What's happening?";
        Message.ForeColor = Color.Gray;
        Message.Tag = null;
      }
    }

    private void btnMore_Click(object sender, EventArgs e)
    {
      GetTweets();
    }
  }

}
