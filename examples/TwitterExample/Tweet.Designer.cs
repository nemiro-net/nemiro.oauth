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
namespace TwitterExample
{
  partial class Tweet
  {
    /// <summary> 
    /// Обязательная переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором компонентов

    /// <summary> 
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.Nickname = new System.Windows.Forms.Label();
      this.Username = new System.Windows.Forms.Label();
      this.TweetText = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.label1 = new System.Windows.Forms.Label();
      this.DateCreated = new System.Windows.Forms.Label();
      this.Userpic = new System.Windows.Forms.PictureBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.Userpic)).BeginInit();
      this.SuspendLayout();
      // 
      // Nickname
      // 
      this.Nickname.AutoSize = true;
      this.Nickname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Nickname.Location = new System.Drawing.Point(3, 0);
      this.Nickname.Name = "Nickname";
      this.Nickname.Size = new System.Drawing.Size(63, 13);
      this.Nickname.TabIndex = 1;
      this.Nickname.Text = "Nickname";
      // 
      // Username
      // 
      this.Username.AutoSize = true;
      this.Username.ForeColor = System.Drawing.Color.Gray;
      this.Username.Location = new System.Drawing.Point(72, 0);
      this.Username.Name = "Username";
      this.Username.Size = new System.Drawing.Size(64, 13);
      this.Username.TabIndex = 2;
      this.Username.Text = "@username";
      // 
      // TweetText
      // 
      this.TweetText.AutoSize = true;
      this.TweetText.Location = new System.Drawing.Point(3, 24);
      this.TweetText.Name = "TweetText";
      this.TweetText.Size = new System.Drawing.Size(52, 13);
      this.TweetText.TabIndex = 3;
      this.TweetText.Text = "Text here";
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.Userpic, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(463, 94);
      this.tableLayoutPanel1.TabIndex = 4;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.TweetText, 0, 1);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(51, 3);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(409, 88);
      this.tableLayoutPanel2.TabIndex = 1;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.Nickname);
      this.flowLayoutPanel1.Controls.Add(this.Username);
      this.flowLayoutPanel1.Controls.Add(this.label1);
      this.flowLayoutPanel1.Controls.Add(this.DateCreated);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(409, 24);
      this.flowLayoutPanel1.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.ForeColor = System.Drawing.Color.Gray;
      this.label1.Location = new System.Drawing.Point(139, 0);
      this.label1.Margin = new System.Windows.Forms.Padding(0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(13, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "‏::";
      // 
      // DateCreated
      // 
      this.DateCreated.AutoSize = true;
      this.DateCreated.ForeColor = System.Drawing.Color.Gray;
      this.DateCreated.Location = new System.Drawing.Point(155, 0);
      this.DateCreated.Name = "DateCreated";
      this.DateCreated.Size = new System.Drawing.Size(26, 13);
      this.DateCreated.TabIndex = 4;
      this.DateCreated.Text = "time";
      // 
      // Userpic
      // 
      this.Userpic.BackColor = System.Drawing.Color.White;
      this.Userpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Userpic.Image = global::TwitterExample.Properties.Resources.loader;
      this.Userpic.Location = new System.Drawing.Point(3, 3);
      this.Userpic.Name = "Userpic";
      this.Userpic.Size = new System.Drawing.Size(42, 42);
      this.Userpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.Userpic.TabIndex = 0;
      this.Userpic.TabStop = false;
      // 
      // timer1
      // 
      this.timer1.Interval = 250;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // Tweet
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this.tableLayoutPanel1);
      this.ForeColor = System.Drawing.Color.Black;
      this.Name = "Tweet";
      this.Size = new System.Drawing.Size(463, 94);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.Userpic)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox Userpic;
    private System.Windows.Forms.Label Nickname;
    private System.Windows.Forms.Label Username;
    private System.Windows.Forms.Label TweetText;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label DateCreated;
    private System.Windows.Forms.Timer timer1;
  }
}
