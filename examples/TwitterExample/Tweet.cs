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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nemiro.OAuth;
using System.IO;
using System.Net;

namespace TwitterExample
{

  public partial class Tweet : UserControl
  {

    private static string CachePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "cache");

    private static List<string> LoadingPics = new List<string>();

    private string UserpicPath = "";

    public Tweet(UniValue data)
    {
      InitializeComponent();

      Dock = DockStyle.Top;
      Username.Text = String.Format("@{0}", data["user"]["screen_name"]);
      Nickname.Text = data["user"]["name"].ToString();
      TweetText.Text = data["text"].ToString();

      var created = System.DateTime.ParseExact(data["created_at"].ToString(), "ddd MMM dd HH:mm:ss zzzz yyyy", System.Globalization.CultureInfo.InvariantCulture);

      if (DateTime.Now.Day == created.Day && DateTime.Now.Month == created.Month && DateTime.Now.Year == created.Year)
      {
        DateCreated.Text= created.ToString("HH:mm");
      }
      else if (DateTime.Now.Year != created.Year)
      {
        DateCreated.Text = created.ToString("dd MMMM yyyy");
      }
      else
      {
        DateCreated.Text = created.ToString("dd MMMM");
      }

      if (data["user"]["profile_image_url"].HasValue && !String.IsNullOrEmpty(data["user"]["profile_image_url"].ToString()))
      {
        UserpicPath = Path.Combine(Tweet.CachePath, String.Format("{0}.jpg", this.GetMD5Hash(data["user"]["profile_image_url"].ToString())));

        if (File.Exists(this.UserpicPath) && Tweet.LoadingPics.IndexOf(this.UserpicPath) == -1)
        {
          Userpic.Image = Image.FromFile(this.UserpicPath);
          Userpic.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        else if (File.Exists(this.UserpicPath) && Tweet.LoadingPics.IndexOf(this.UserpicPath) != -1)
        {
          timer1.Enabled = true;
        }
        else
        {
          DownloadUserpic(data["user"]["profile_image_url"].ToString(), this.UserpicPath);
          timer1.Enabled = true;
        }
      }
    }

    private void DownloadUserpic(string url, string userpicPath)
    {
      Tweet.LoadingPics.Add(userpicPath);

      if (!Directory.Exists(Tweet.CachePath))
      {
        Directory.CreateDirectory(Tweet.CachePath);
      }
      
      var client = new WebClient();

      client.DownloadFileCompleted += (object s, AsyncCompletedEventArgs e) =>
      {
        Tweet.LoadingPics.Remove(userpicPath);
      };

      client.DownloadFileAsync(new Uri(url), userpicPath);
    }

    private string GetMD5Hash(string value)
    {
      var md5 = System.Security.Cryptography.MD5.Create();

      byte[] inputBytes = Encoding.UTF8.GetBytes(value);
      byte[] hashBytes = md5.ComputeHash(inputBytes);
            
      return String.Join("", hashBytes.Select(b => b.ToString("x2")));
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (Tweet.LoadingPics.IndexOf(UserpicPath) == -1)
      {
        Userpic.Image = Image.FromFile(UserpicPath);
        Userpic.SizeMode = PictureBoxSizeMode.StretchImage;
        timer1.Enabled = false;
      }
    }

  }

}
