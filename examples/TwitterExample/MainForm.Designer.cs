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
  partial class MainForm
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

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.btnTweet = new System.Windows.Forms.Button();
      this.Message = new System.Windows.Forms.TextBox();
      this.btnAddPhoto = new System.Windows.Forms.Button();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.btnMore = new System.Windows.Forms.Button();
      this.UploadedImages = new System.Windows.Forms.FlowLayoutPanel();
      this.FlowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.picStatus = new System.Windows.Forms.PictureBox();
      this.lblStatus = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.Tweets = new System.Windows.Forms.Panel();
      this.FlowLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnTweet
      // 
      this.btnTweet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(205)))), ((int)(((byte)(242)))));
      this.btnTweet.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnTweet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.btnTweet.ForeColor = System.Drawing.Color.White;
      this.btnTweet.Location = new System.Drawing.Point(468, 3);
      this.btnTweet.Name = "btnTweet";
      this.btnTweet.Size = new System.Drawing.Size(75, 36);
      this.btnTweet.TabIndex = 0;
      this.btnTweet.Text = "Tweet";
      this.btnTweet.UseVisualStyleBackColor = false;
      this.btnTweet.Click += new System.EventHandler(this.btnTweet_Click);
      // 
      // Message
      // 
      this.Message.BackColor = System.Drawing.Color.White;
      this.Message.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Message.ForeColor = System.Drawing.Color.Black;
      this.Message.Location = new System.Drawing.Point(3, 3);
      this.Message.Multiline = true;
      this.Message.Name = "Message";
      this.Message.Size = new System.Drawing.Size(540, 69);
      this.Message.TabIndex = 1;
      this.Message.Tag = "";
      this.Message.TextChanged += new System.EventHandler(this.Message_TextChanged);
      this.Message.Enter += new System.EventHandler(this.Message_Enter);
      this.Message.Leave += new System.EventHandler(this.Message_Leave);
      // 
      // btnAddPhoto
      // 
      this.btnAddPhoto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(205)))), ((int)(((byte)(242)))));
      this.btnAddPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnAddPhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.btnAddPhoto.ForeColor = System.Drawing.Color.White;
      this.btnAddPhoto.Location = new System.Drawing.Point(3, 3);
      this.btnAddPhoto.Name = "btnAddPhoto";
      this.btnAddPhoto.Size = new System.Drawing.Size(87, 36);
      this.btnAddPhoto.TabIndex = 2;
      this.btnAddPhoto.Text = "Add photo";
      this.btnAddPhoto.UseVisualStyleBackColor = false;
      this.btnAddPhoto.Click += new System.EventHandler(this.btnAddPhoto_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.Filter = "Images (*.jpg;*.png;*.gif;*.bmp) | *.jpg;*.png;*.gif;*.bmp | All files (*.*) | *." +
    "*";
      // 
      // btnMore
      // 
      this.btnMore.BackColor = System.Drawing.Color.White;
      this.btnMore.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.btnMore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(205)))), ((int)(((byte)(242)))));
      this.btnMore.Location = new System.Drawing.Point(3, 314);
      this.btnMore.Name = "btnMore";
      this.btnMore.Size = new System.Drawing.Size(540, 26);
      this.btnMore.TabIndex = 3;
      this.btnMore.Text = "Load more tweets";
      this.btnMore.UseVisualStyleBackColor = false;
      this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
      // 
      // UploadedImages
      // 
      this.UploadedImages.Location = new System.Drawing.Point(93, 0);
      this.UploadedImages.Margin = new System.Windows.Forms.Padding(0);
      this.UploadedImages.Name = "UploadedImages";
      this.UploadedImages.Size = new System.Drawing.Size(352, 39);
      this.UploadedImages.TabIndex = 4;
      // 
      // FlowLayoutPanel1
      // 
      this.FlowLayoutPanel1.BackColor = System.Drawing.Color.White;
      this.FlowLayoutPanel1.Controls.Add(this.picStatus);
      this.FlowLayoutPanel1.Controls.Add(this.lblStatus);
      this.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.FlowLayoutPanel1.Location = new System.Drawing.Point(3, 463);
      this.FlowLayoutPanel1.Name = "FlowLayoutPanel1";
      this.FlowLayoutPanel1.Size = new System.Drawing.Size(540, 26);
      this.FlowLayoutPanel1.TabIndex = 5;
      // 
      // picStatus
      // 
      this.picStatus.Image = global::TwitterExample.Properties.Resources.loader2;
      this.picStatus.Location = new System.Drawing.Point(0, 0);
      this.picStatus.Margin = new System.Windows.Forms.Padding(0);
      this.picStatus.Name = "picStatus";
      this.picStatus.Size = new System.Drawing.Size(24, 24);
      this.picStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picStatus.TabIndex = 0;
      this.picStatus.TabStop = false;
      // 
      // lblStatus
      // 
      this.lblStatus.AutoSize = true;
      this.lblStatus.Dock = System.Windows.Forms.DockStyle.Left;
      this.lblStatus.ForeColor = System.Drawing.Color.Black;
      this.lblStatus.Location = new System.Drawing.Point(27, 0);
      this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(16, 24);
      this.lblStatus.TabIndex = 1;
      this.lblStatus.Text = "...";
      this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.FlowLayoutPanel1, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.btnMore, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.Tweets, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 5;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(546, 492);
      this.tableLayoutPanel1.TabIndex = 6;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(248)))), ((int)(((byte)(253)))));
      this.tableLayoutPanel2.ColumnCount = 3;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel2.Controls.Add(this.UploadedImages, 1, 0);
      this.tableLayoutPanel2.Controls.Add(this.btnAddPhoto, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.btnTweet, 2, 0);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 418);
      this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 1;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(546, 42);
      this.tableLayoutPanel2.TabIndex = 7;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(248)))), ((int)(((byte)(253)))));
      this.panel1.Controls.Add(this.Message);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 343);
      this.panel1.Margin = new System.Windows.Forms.Padding(0);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new System.Windows.Forms.Padding(3);
      this.panel1.Size = new System.Drawing.Size(546, 75);
      this.panel1.TabIndex = 8;
      // 
      // Tweets
      // 
      this.Tweets.AutoScroll = true;
      this.Tweets.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Tweets.Location = new System.Drawing.Point(3, 3);
      this.Tweets.Name = "Tweets";
      this.Tweets.Size = new System.Drawing.Size(540, 305);
      this.Tweets.TabIndex = 9;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(546, 492);
      this.Controls.Add(this.tableLayoutPanel1);
      this.ForeColor = System.Drawing.Color.Black;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(326, 356);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Twitter";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.FlowLayoutPanel1.ResumeLayout(false);
      this.FlowLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnTweet;
    private System.Windows.Forms.TextBox Message;
    private System.Windows.Forms.Button btnAddPhoto;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.Button btnMore;
    private System.Windows.Forms.FlowLayoutPanel UploadedImages;
    internal System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel1;
    internal System.Windows.Forms.PictureBox picStatus;
    internal System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel Tweets;
  }
}

