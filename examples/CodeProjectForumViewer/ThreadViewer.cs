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
using System.Linq;
using System.Windows.Forms;
using Nemiro.OAuth;

namespace CodeProjectForumViewer
{

  public partial class ThreadViewer : Form
  {

    private long ThreadId = 0;
    private int CurrentPage = 1;

    public ThreadViewer()
    {
      InitializeComponent();
    }

    public ThreadViewer(long threadId) : this()
    {
      this.ThreadId = threadId;
    }

    private void ThreadViewer_Load(object sender, EventArgs e)
    {
      this.GetThread();
    }

    /// <summary>
    /// Gets the messages of the Thread
    /// </summary>
    /// <remarks>
    /// Help: https://api.codeproject.com/Help/Api/GET-v1-MessageThread-threadId-messages_page
    /// </remarks>
    private void GetThread()
    {
      OAuthUtility.GetAsync
      (
        String.Format("https://api.codeproject.com/v1/MessageThread/{0}/messages", this.ThreadId),
        new HttpParameterCollection 
        { 
          { "page", this.CurrentPage }
        },
        authorization: new HttpAuthorization(AuthorizationType.Bearer, Properties.Settings.Default.AccessToken),
        callback: GetThread_Result
      );
    }

    private void GetThread_Result(RequestResult result)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new Action<RequestResult>(GetThread_Result), result);
        return;
      }

      if (!result.IsSuccessfully)
      {
        webBrowser1.DocumentText = result.ToString();
        return;
      }

      string messages = "";

      foreach (UniValue item in result["items"])
      {
        string message = Properties.Resources.MessageTemplate;

        if (item.Key == "0")
        {
          messages = String.Format("<h3>{0}</h3>", item["title"]);
          this.Text = item["title"].ToString();
        }

        message = message.Replace("%author%", String.Join(", ", item["authors"].Select(author=>author["name"].ToString())));
        message = message.Replace("%date%", Convert.ToDateTime(item["createdDate"]).ToString("d MMM yyyy, HH:mm:ss"));
        message = message.Replace("%text%", item["summary"].ToString());
        messages += message;
      }

      webBrowser1.DocumentText = Properties.Resources.ThreadTemplate.Replace("%body%", messages);
    }

  }

}