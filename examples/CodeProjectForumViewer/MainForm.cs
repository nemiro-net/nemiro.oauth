// ----------------------------------------------------------------------------
// Copyright (c) Aleksey Nemiro, 2015. All rights reserved.
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Nemiro.OAuth.LoginForms;
using Nemiro.OAuth;

namespace CodeProjectForumViewer
{

  public partial class MainForm : Form
  {

    private bool AccessTokenIsCheked = false;

    /// <summary>
    /// Current page
    /// </summary>
    private int CurrentPage = 1;

    public MainForm()
    {
      InitializeComponent();
    }

    private void Main_Load(object sender, EventArgs e)
    {
      // set style for columns of the dataGridView
      dataGridView1.AutoGenerateColumns = false;
      dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

      // forums list
      toolStripComboBox1.ComboBox.DisplayMember = "Text";
      toolStripComboBox1.ComboBox.ValueMember = "Value";
      toolStripComboBox1.Items.Add(new { Text = "Uncategorised posts", Value = 1712597 });
      toolStripComboBox1.Items.Add(new { Text = "General Programming", Value = 0 });
      toolStripComboBox1.Items.Add(new { Text = "-- .NET Framework", Value = 1650 });
      toolStripComboBox1.Items.Add(new { Text = "-- Algorithms", Value = 326859 });
      toolStripComboBox1.Items.Add(new { Text = "-- C#", Value = 1649 });
      toolStripComboBox1.Items.Add(new { Text = "-- Graphics", Value = 387159 });
      toolStripComboBox1.Items.Add(new { Text = "-- Hardware & Devices", Value = 186301 });
      toolStripComboBox1.Items.Add(new { Text = "-- LINQ", Value = 1004117 });
      toolStripComboBox1.Items.Add(new { Text = "Web Development", Value = 0 });
      toolStripComboBox1.Items.Add(new { Text = "-- ASP.NET", Value = 12076 });
      toolStripComboBox1.Items.Add(new { Text = "-- JavaScript", Value = 1580226 });
      toolStripComboBox1.Items.Add(new { Text = "Mobile Development", Value = 0 });
      toolStripComboBox1.Items.Add(new { Text = "-- Android", Value = 1848626 });
      toolStripComboBox1.Items.Add(new { Text = "-- iOS", Value = 1876716 });
      toolStripComboBox1.Items.Add(new { Text = "-- Mobile", Value = 13695 });

      // check access token
      if (String.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
      {
        // access token is empty
        // get access token
        this.GetAccessToken();
      }
      else
      {
        // get messages list
        toolStripComboBox1.SelectedIndex = 0;
        //this.UpdateList();
      }
    }

    private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      // forum index is changed, 
      // update list of messages
      this.UpdateList();
    }

    private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      // show thread
      long threadId = Convert.ToInt64(((dynamic)dataGridView1.Rows[e.RowIndex].DataBoundItem).id);
      new ThreadViewer(threadId).Show();
    }

    private void GetAccessToken()
    {
      this.AccessTokenIsCheked = true;

      // create login form
      var login = new CodeProjectLogin
      (
        "92mWWELc2DjcL-6tu7L1Py6yllleqSCt", 
        "YJXrk_Vzz4Ps02GqmaUY-aSLucxh4kfLq6oq0CtiukPfvbzb9yQG69NeDr2yiV9M", 
        "https://oauthproxy.nemiro.net/"
      );
      // set owner
      login.Owner = this;
      // show login form
      login.ShowDialog();
      // authorization is success
      if (login.IsSuccessfully)
      {
        // save the access token to the application settings
        Properties.Settings.Default.AccessToken = login.AccessTokenValue;
        Properties.Settings.Default.Save();
        // update the list of messages
        toolStripComboBox1.SelectedIndex = 0;
        toolStripComboBox1_SelectedIndexChanged(toolStripComboBox1, null);
      }
      // is fails
      else
      {
        if (MessageBox.Show("Please Click OK to login on CodeProject or CANCEL for exit from the program.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
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
    /// Updates the list of messages.
    /// </summary>
    /// <remarks>
    /// Help: https://api.codeproject.com/Help/Api/GET-v1-Forum-forumId-mode_page
    /// </remarks>
    private void UpdateList()
    {
      int forumId = ((dynamic)toolStripComboBox1.SelectedItem).Value;
      
      if (forumId <= 0) { return; }

      this.RequestStart();

      OAuthUtility.GetAsync
      (
        String.Format("https://api.codeproject.com/v1/Forum/{0}/Threads", forumId),
        new HttpParameterCollection 
        { 
          { "page", this.CurrentPage }
        },
        authorization: new HttpAuthorization(AuthorizationType.Bearer, Properties.Settings.Default.AccessToken),
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

      if (!result.IsSuccessfully)
      {
        if (!this.AccessTokenIsCheked)
        {
          this.GetAccessToken();
        }
        else
        {
          MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          this.RequestEnd();
        }
        return;
      }

      if (!this.AccessTokenIsCheked)
      {
        this.AccessTokenIsCheked = true;
      }

      // set list of messages to the dataGridView
      dataGridView1.DataSource = result["items"].Select
      (
        item => new 
        {
          id = Convert.ToInt64(item["id"]),
          title = item["title"].ToString(),
          author = item["authors"].First()["name"].ToString(),
          createdDate = Convert.ToDateTime(item["createdDate"])
        }
      ).ToArray();

      // update list of pages
      flowLayoutPanel1.Controls.Clear();

      int start = (int)result["pagination"]["page"];
      int end = (int)result["pagination"]["totalPages"];

      if (this.CurrentPage > 1)
      {
        if (this.CurrentPage - 4 > 0)
        {
          start = this.CurrentPage - 4;
        }
        else
        {
          start = start - this.CurrentPage + 1;
        }
      }

      if (end - start > 10)
      {
        end = start + 10;
      }
      else
      {
        end = start + (end - start);
      }

      if (start - 1 > 0)
      {
        this.AddPage(this.CurrentPage - 1, "<<");
      } 

      for (int i = start; i < end; i++)
      {
        this.AddPage(i);
      }

      if (end < (int)result["pagination"]["totalPages"])
      {
        this.AddPage(this.CurrentPage + 1, ">>");
      }

      // hide progress
      this.RequestEnd();
    }

    /// <summary>
    /// Adds page item to list
    /// </summary>
    /// <param name="pageIndex">The index of page</param>
    /// <param name="title">Teh title</param>
    private void AddPage(int pageIndex, string title = null)
    {
      var page = new LinkLabel();
      page.Text = title ?? pageIndex.ToString();
      page.Tag = pageIndex;
      page.AutoSize = true;
      if (this.CurrentPage == pageIndex)
      {
        page.Font = new Font(page.Font, FontStyle.Bold);
      }
      page.Click += Page_Click;
      flowLayoutPanel1.Controls.Add(page);
    }

    /// <summary>
    /// Click handler for the page
    /// </summary>
    private void Page_Click(object sender, EventArgs e)
    {
      this.CurrentPage = Convert.ToInt32(((LinkLabel)sender).Tag);
      this.UpdateList();
    }

    private void RequestStart()
    {
      // get screen of the dataGridView1
      var bmp = new System.Drawing.Bitmap(dataGridView1.Width, dataGridView1.Height);
      dataGridView1.DrawToBitmap(bmp, dataGridView1.ClientRectangle);
      // set image to background
      pictureBox1.BackgroundImage = bmp;
      // show progress
      pictureBox1.Visible = true;
      // disable controls
      dataGridView1.Enabled = toolStrip1.Enabled = flowLayoutPanel1.Enabled = false;
    }

    private void RequestEnd()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action(RequestEnd));
        return;
      }

      dataGridView1.Enabled = toolStrip1.Enabled = flowLayoutPanel1.Enabled = true;
      pictureBox1.Visible = false;
    }

  }

}