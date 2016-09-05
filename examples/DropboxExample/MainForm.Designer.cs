// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2014. All rights reserved.
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
namespace DropboxExample
{
  partial class MainForm
  {
    /// <summary>
    /// Требуется переменная конструктора.
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
    /// Обязательный метод для поддержки конструктора - не изменяйте
    /// содержимое данного метода при помощи редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.listView1 = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.mnuDownload = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.mnuUpload = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuCreateFolder = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this.mnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.mnuRename = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuRemove = new System.Windows.Forms.ToolStripMenuItem();
      this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
      this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
      this.lblStatus = new System.Windows.Forms.Label();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.picStatus = new System.Windows.Forms.PictureBox();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.btnDownload = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.btnUpload = new System.Windows.Forms.ToolStripButton();
      this.btnCreateFolder = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.btnRename = new System.Windows.Forms.ToolStripButton();
      this.btnRemove = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.btnRefresh = new System.Windows.Forms.ToolStripButton();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.contextMenuStrip1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // listView1
      // 
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
      this.listView1.ContextMenuStrip = this.contextMenuStrip1;
      this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listView1.FullRowSelect = true;
      this.listView1.LargeImageList = this.imageListLarge;
      this.listView1.Location = new System.Drawing.Point(0, 25);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(552, 283);
      this.listView1.SmallImageList = this.imageListSmall;
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
      this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
      this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "";
      this.columnHeader1.Width = 24;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Name";
      this.columnHeader2.Width = 250;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Size";
      this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeader3.Width = 90;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Type";
      this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeader4.Width = 90;
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDownload,
            this.toolStripSeparator4,
            this.mnuUpload,
            this.mnuCreateFolder,
            this.toolStripSeparator6,
            this.mnuRefresh,
            this.toolStripSeparator3,
            this.mnuRename,
            this.mnuRemove});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(143, 154);
      // 
      // mnuDownload
      // 
      this.mnuDownload.Image = global::DropboxExample.Properties.Resources.disk;
      this.mnuDownload.Name = "mnuDownload";
      this.mnuDownload.Size = new System.Drawing.Size(142, 22);
      this.mnuDownload.Text = "Download";
      this.mnuDownload.Click += new System.EventHandler(this.btnDownload_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(139, 6);
      // 
      // mnuUpload
      // 
      this.mnuUpload.Image = global::DropboxExample.Properties.Resources.disk__plus;
      this.mnuUpload.Name = "mnuUpload";
      this.mnuUpload.Size = new System.Drawing.Size(142, 22);
      this.mnuUpload.Text = "Upload file";
      this.mnuUpload.Click += new System.EventHandler(this.btnUpload_Click);
      // 
      // mnuCreateFolder
      // 
      this.mnuCreateFolder.Image = global::DropboxExample.Properties.Resources.folder__plus;
      this.mnuCreateFolder.Name = "mnuCreateFolder";
      this.mnuCreateFolder.Size = new System.Drawing.Size(142, 22);
      this.mnuCreateFolder.Text = "Create folder";
      this.mnuCreateFolder.Click += new System.EventHandler(this.btnCreateFolder_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(139, 6);
      // 
      // mnuRefresh
      // 
      this.mnuRefresh.Image = global::DropboxExample.Properties.Resources.refresh;
      this.mnuRefresh.Name = "mnuRefresh";
      this.mnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
      this.mnuRefresh.Size = new System.Drawing.Size(142, 22);
      this.mnuRefresh.Text = "Refresh";
      this.mnuRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(139, 6);
      // 
      // mnuRename
      // 
      this.mnuRename.Image = global::DropboxExample.Properties.Resources.document__pencil;
      this.mnuRename.Name = "mnuRename";
      this.mnuRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
      this.mnuRename.Size = new System.Drawing.Size(142, 22);
      this.mnuRename.Text = "Rename";
      this.mnuRename.Click += new System.EventHandler(this.btnRename_Click);
      // 
      // mnuRemove
      // 
      this.mnuRemove.Image = global::DropboxExample.Properties.Resources.bin;
      this.mnuRemove.Name = "mnuRemove";
      this.mnuRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
      this.mnuRemove.Size = new System.Drawing.Size(142, 22);
      this.mnuRemove.Text = "Delete";
      this.mnuRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // imageListLarge
      // 
      this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
      this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListLarge.Images.SetKeyName(0, "excel");
      this.imageListLarge.Images.SetKeyName(1, "folder_app");
      this.imageListLarge.Images.SetKeyName(2, "folder_camera");
      this.imageListLarge.Images.SetKeyName(3, "folder_gray");
      this.imageListLarge.Images.SetKeyName(4, "folder_public");
      this.imageListLarge.Images.SetKeyName(5, "folder_star");
      this.imageListLarge.Images.SetKeyName(6, "folder_user_gray");
      this.imageListLarge.Images.SetKeyName(7, "folder_user");
      this.imageListLarge.Images.SetKeyName(8, "folder");
      this.imageListLarge.Images.SetKeyName(9, "music");
      this.imageListLarge.Images.SetKeyName(10, "package");
      this.imageListLarge.Images.SetKeyName(11, "page_white_acrobat");
      this.imageListLarge.Images.SetKeyName(12, "page_white_actionscript");
      this.imageListLarge.Images.SetKeyName(13, "page_white_c");
      this.imageListLarge.Images.SetKeyName(14, "page_white_code");
      this.imageListLarge.Images.SetKeyName(15, "page_white_cplusplus");
      this.imageListLarge.Images.SetKeyName(16, "page_white_csharp");
      this.imageListLarge.Images.SetKeyName(17, "page_white_cup");
      this.imageListLarge.Images.SetKeyName(18, "page_white_dvd");
      this.imageListLarge.Images.SetKeyName(19, "page_white_flash");
      this.imageListLarge.Images.SetKeyName(20, "page_white_gear");
      this.imageListLarge.Images.SetKeyName(21, "page_white_h");
      this.imageListLarge.Images.SetKeyName(22, "page_white_paint");
      this.imageListLarge.Images.SetKeyName(23, "page_white_php");
      this.imageListLarge.Images.SetKeyName(24, "page_white_picture");
      this.imageListLarge.Images.SetKeyName(25, "page_white_ruby");
      this.imageListLarge.Images.SetKeyName(26, "page_white_text");
      this.imageListLarge.Images.SetKeyName(27, "page_white_tux");
      this.imageListLarge.Images.SetKeyName(28, "page_white_vector");
      this.imageListLarge.Images.SetKeyName(29, "page_white_visualstudio");
      this.imageListLarge.Images.SetKeyName(30, "page_white_zip");
      this.imageListLarge.Images.SetKeyName(31, "page_white");
      this.imageListLarge.Images.SetKeyName(32, "powerpoint");
      this.imageListLarge.Images.SetKeyName(33, "word");
      // 
      // imageListSmall
      // 
      this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
      this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListSmall.Images.SetKeyName(0, "folder");
      this.imageListSmall.Images.SetKeyName(1, "folder_app");
      this.imageListSmall.Images.SetKeyName(2, "folder_camera");
      this.imageListSmall.Images.SetKeyName(3, "folder_gray");
      this.imageListSmall.Images.SetKeyName(4, "folder_public");
      this.imageListSmall.Images.SetKeyName(5, "folder_star");
      this.imageListSmall.Images.SetKeyName(6, "folder_user");
      this.imageListSmall.Images.SetKeyName(7, "folder_user_gray");
      this.imageListSmall.Images.SetKeyName(8, "package");
      this.imageListSmall.Images.SetKeyName(9, "page_white");
      this.imageListSmall.Images.SetKeyName(10, "page_white_acrobat");
      this.imageListSmall.Images.SetKeyName(11, "page_white_actionscript");
      this.imageListSmall.Images.SetKeyName(12, "page_white_c");
      this.imageListSmall.Images.SetKeyName(13, "page_white_code");
      this.imageListSmall.Images.SetKeyName(14, "page_white_compressed");
      this.imageListSmall.Images.SetKeyName(15, "page_white_cplusplus");
      this.imageListSmall.Images.SetKeyName(16, "page_white_csharp");
      this.imageListSmall.Images.SetKeyName(17, "page_white_cup");
      this.imageListSmall.Images.SetKeyName(18, "page_white_dvd");
      this.imageListSmall.Images.SetKeyName(19, "page_white_excel");
      this.imageListSmall.Images.SetKeyName(20, "page_white_film");
      this.imageListSmall.Images.SetKeyName(21, "page_white_flash");
      this.imageListSmall.Images.SetKeyName(22, "page_white_gear");
      this.imageListSmall.Images.SetKeyName(23, "page_white_paint");
      this.imageListSmall.Images.SetKeyName(24, "page_white_php");
      this.imageListSmall.Images.SetKeyName(25, "page_white_picture");
      this.imageListSmall.Images.SetKeyName(26, "page_white_powerpoint");
      this.imageListSmall.Images.SetKeyName(27, "page_white_ruby");
      this.imageListSmall.Images.SetKeyName(28, "page_white_sound");
      this.imageListSmall.Images.SetKeyName(29, "page_white_text");
      this.imageListSmall.Images.SetKeyName(30, "page_white_tux");
      this.imageListSmall.Images.SetKeyName(31, "page_white_vector");
      this.imageListSmall.Images.SetKeyName(32, "page_white_visualstudio");
      this.imageListSmall.Images.SetKeyName(33, "page_white_word");
      // 
      // lblStatus
      // 
      this.lblStatus.AutoSize = true;
      this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblStatus.Location = new System.Drawing.Point(33, 0);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(73, 30);
      this.lblStatus.TabIndex = 2;
      this.lblStatus.Text = "Please Wait...";
      this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.flowLayoutPanel1.Controls.Add(this.picStatus);
      this.flowLayoutPanel1.Controls.Add(this.lblStatus);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 308);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(552, 34);
      this.flowLayoutPanel1.TabIndex = 3;
      // 
      // picStatus
      // 
      this.picStatus.Image = global::DropboxExample.Properties.Resources.loader2;
      this.picStatus.Location = new System.Drawing.Point(3, 3);
      this.picStatus.Name = "picStatus";
      this.picStatus.Size = new System.Drawing.Size(24, 24);
      this.picStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picStatus.TabIndex = 1;
      this.picStatus.TabStop = false;
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDownload,
            this.toolStripSeparator2,
            this.btnUpload,
            this.btnCreateFolder,
            this.toolStripSeparator1,
            this.btnRename,
            this.btnRemove,
            this.toolStripSeparator5,
            this.btnRefresh});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(552, 25);
      this.toolStrip1.TabIndex = 4;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // btnDownload
      // 
      this.btnDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnDownload.Image = global::DropboxExample.Properties.Resources.disk;
      this.btnDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnDownload.Name = "btnDownload";
      this.btnDownload.Size = new System.Drawing.Size(23, 22);
      this.btnDownload.Text = "Download file";
      this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // btnUpload
      // 
      this.btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnUpload.Image = global::DropboxExample.Properties.Resources.disk__plus;
      this.btnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnUpload.Name = "btnUpload";
      this.btnUpload.Size = new System.Drawing.Size(23, 22);
      this.btnUpload.Text = "Upload file";
      this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
      // 
      // btnCreateFolder
      // 
      this.btnCreateFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnCreateFolder.Image = global::DropboxExample.Properties.Resources.folder__plus;
      this.btnCreateFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnCreateFolder.Name = "btnCreateFolder";
      this.btnCreateFolder.Size = new System.Drawing.Size(23, 22);
      this.btnCreateFolder.Text = "Create folder";
      this.btnCreateFolder.Click += new System.EventHandler(this.btnCreateFolder_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // btnRename
      // 
      this.btnRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnRename.Image = global::DropboxExample.Properties.Resources.document__pencil;
      this.btnRename.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnRename.Name = "btnRename";
      this.btnRename.Size = new System.Drawing.Size(23, 22);
      this.btnRename.Text = "Rename";
      this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
      // 
      // btnRemove
      // 
      this.btnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnRemove.Image = global::DropboxExample.Properties.Resources.bin;
      this.btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new System.Drawing.Size(23, 22);
      this.btnRemove.Text = "Remove";
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
      // 
      // btnRefresh
      // 
      this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnRefresh.Image = global::DropboxExample.Properties.Resources.refresh;
      this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new System.Drawing.Size(23, 22);
      this.btnRefresh.Text = "Refresh";
      this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.Filter = "All files (*.*) | *.*";
      this.openFileDialog1.Multiselect = true;
      // 
      // MainForm
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(552, 342);
      this.Controls.Add(this.listView1);
      this.Controls.Add(this.flowLayoutPanel1);
      this.Controls.Add(this.toolStrip1);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Filexp";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
      this.contextMenuStrip1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.PictureBox picStatus;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.ImageList imageListLarge;
    private System.Windows.Forms.ImageList imageListSmall;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton btnUpload;
    private System.Windows.Forms.ToolStripButton btnCreateFolder;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton btnRename;
    private System.Windows.Forms.ToolStripButton btnRemove;
    private System.Windows.Forms.ToolStripButton btnDownload;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem mnuDownload;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem mnuUpload;
    private System.Windows.Forms.ToolStripMenuItem mnuCreateFolder;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem mnuRename;
    private System.Windows.Forms.ToolStripMenuItem mnuRemove;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripButton btnRefresh;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem mnuRefresh;
  }
}

