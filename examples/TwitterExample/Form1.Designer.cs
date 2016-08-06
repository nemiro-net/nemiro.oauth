namespace TwitterExample
{
  partial class Form1
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
      this.btnTweet = new System.Windows.Forms.Button();
      this.Message = new System.Windows.Forms.TextBox();
      this.btnAddPhoto = new System.Windows.Forms.Button();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.btnMore = new System.Windows.Forms.Button();
      this.UploadedImages = new System.Windows.Forms.FlowLayoutPanel();
      this.SuspendLayout();
      // 
      // btnTweet
      // 
      this.btnTweet.Location = new System.Drawing.Point(367, 66);
      this.btnTweet.Name = "btnTweet";
      this.btnTweet.Size = new System.Drawing.Size(75, 23);
      this.btnTweet.TabIndex = 0;
      this.btnTweet.Text = "Tweet";
      this.btnTweet.UseVisualStyleBackColor = true;
      this.btnTweet.Click += new System.EventHandler(this.btnTweet_Click);
      // 
      // Message
      // 
      this.Message.Location = new System.Drawing.Point(12, 12);
      this.Message.Multiline = true;
      this.Message.Name = "Message";
      this.Message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.Message.Size = new System.Drawing.Size(430, 48);
      this.Message.TabIndex = 1;
      // 
      // btnAddPhoto
      // 
      this.btnAddPhoto.Location = new System.Drawing.Point(12, 66);
      this.btnAddPhoto.Name = "btnAddPhoto";
      this.btnAddPhoto.Size = new System.Drawing.Size(75, 23);
      this.btnAddPhoto.TabIndex = 2;
      this.btnAddPhoto.Text = "Add photo";
      this.btnAddPhoto.UseVisualStyleBackColor = true;
      this.btnAddPhoto.Click += new System.EventHandler(this.btnAddPhoto_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.Filter = "Images (*.jpg;*.png;*.gif;*.bmp) | *.jpg;*.png;*.gif;*.bmp | All files (*.*) | *." +
    "*";
      // 
      // btnMore
      // 
      this.btnMore.Location = new System.Drawing.Point(322, 281);
      this.btnMore.Name = "btnMore";
      this.btnMore.Size = new System.Drawing.Size(75, 23);
      this.btnMore.TabIndex = 3;
      this.btnMore.UseVisualStyleBackColor = true;
      // 
      // UploadedImages
      // 
      this.UploadedImages.Location = new System.Drawing.Point(12, 95);
      this.UploadedImages.Name = "UploadedImages";
      this.UploadedImages.Size = new System.Drawing.Size(430, 129);
      this.UploadedImages.TabIndex = 4;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(634, 363);
      this.Controls.Add(this.UploadedImages);
      this.Controls.Add(this.btnMore);
      this.Controls.Add(this.btnAddPhoto);
      this.Controls.Add(this.Message);
      this.Controls.Add(this.btnTweet);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnTweet;
    private System.Windows.Forms.TextBox Message;
    private System.Windows.Forms.Button btnAddPhoto;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.Button btnMore;
    private System.Windows.Forms.FlowLayoutPanel UploadedImages;
  }
}

