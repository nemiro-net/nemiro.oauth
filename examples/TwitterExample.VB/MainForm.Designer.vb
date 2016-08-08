' ----------------------------------------------------------------------------
' Copyright © Aleksey Nemiro, 2014. All rights reserved.
' 
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
' 
' http:'www.apache.org/licenses/LICENSE-2.0
' 
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
' ----------------------------------------------------------------------------
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
  Inherits System.Windows.Forms.Form

  'Форма переопределяет dispose для очистки списка компонентов.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Является обязательной для конструктора форм Windows Forms
  Private components As System.ComponentModel.IContainer

  'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
  'Для ее изменения используйте конструктор форм Windows Form.  
  'Не изменяйте ее в редакторе исходного кода.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
		Me.tbMessage = New System.Windows.Forms.TextBox()
		Me.btnTweet = New System.Windows.Forms.Button()
		Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
		Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
		Me.picStatus = New System.Windows.Forms.PictureBox()
		Me.lblStatus = New System.Windows.Forms.Label()
		Me.btnMore = New System.Windows.Forms.Button()
		Me.pnlList = New System.Windows.Forms.Panel()
		Me.TableLayoutPanel1.SuspendLayout()
		Me.FlowLayoutPanel1.SuspendLayout()
		CType(Me.picStatus, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'tbMessage
		'
		Me.tbMessage.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tbMessage.Location = New System.Drawing.Point(3, 3)
		Me.tbMessage.MaxLength = 140
		Me.tbMessage.Multiline = True
		Me.tbMessage.Name = "tbMessage"
		Me.tbMessage.Size = New System.Drawing.Size(545, 69)
		Me.tbMessage.TabIndex = 0
		Me.tbMessage.Tag = "What's on your mind?"
		Me.tbMessage.Text = "What's on your mind?"
		'
		'btnTweet
		'
		Me.btnTweet.Location = New System.Drawing.Point(3, 78)
		Me.btnTweet.Name = "btnTweet"
		Me.btnTweet.Size = New System.Drawing.Size(75, 23)
		Me.btnTweet.TabIndex = 1
		Me.btnTweet.Text = "Tweet"
		Me.btnTweet.UseVisualStyleBackColor = True
		'
		'TableLayoutPanel1
		'
		Me.TableLayoutPanel1.ColumnCount = 1
		Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
		Me.TableLayoutPanel1.Controls.Add(Me.tbMessage, 0, 0)
		Me.TableLayoutPanel1.Controls.Add(Me.btnTweet, 0, 1)
		Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 4)
		Me.TableLayoutPanel1.Controls.Add(Me.btnMore, 0, 3)
		Me.TableLayoutPanel1.Controls.Add(Me.pnlList, 0, 2)
		Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
		Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
		Me.TableLayoutPanel1.RowCount = 5
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
		Me.TableLayoutPanel1.Size = New System.Drawing.Size(551, 485)
		Me.TableLayoutPanel1.TabIndex = 3
		'
		'FlowLayoutPanel1
		'
		Me.FlowLayoutPanel1.Controls.Add(Me.picStatus)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblStatus)
		Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 456)
		Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
		Me.FlowLayoutPanel1.Size = New System.Drawing.Size(545, 26)
		Me.FlowLayoutPanel1.TabIndex = 3
		'
		'picStatus
		'
		Me.picStatus.Image = Global.TwitterExample.VB.My.Resources.Resources.loader2
		Me.picStatus.Location = New System.Drawing.Point(0, 0)
		Me.picStatus.Margin = New System.Windows.Forms.Padding(0)
		Me.picStatus.Name = "picStatus"
		Me.picStatus.Size = New System.Drawing.Size(24, 24)
		Me.picStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picStatus.TabIndex = 0
		Me.picStatus.TabStop = False
		'
		'lblStatus
		'
		Me.lblStatus.AutoSize = True
		Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Left
		Me.lblStatus.Location = New System.Drawing.Point(27, 0)
		Me.lblStatus.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.Size = New System.Drawing.Size(16, 24)
		Me.lblStatus.TabIndex = 1
		Me.lblStatus.Text = "..."
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'btnMore
		'
		Me.btnMore.Dock = System.Windows.Forms.DockStyle.Fill
		Me.btnMore.Location = New System.Drawing.Point(3, 424)
		Me.btnMore.Name = "btnMore"
		Me.btnMore.Size = New System.Drawing.Size(545, 26)
		Me.btnMore.TabIndex = 4
		Me.btnMore.Text = "More"
		Me.btnMore.UseVisualStyleBackColor = True
		'
		'pnlList
		'
		Me.pnlList.AutoScroll = True
		Me.pnlList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pnlList.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlList.Location = New System.Drawing.Point(3, 108)
		Me.pnlList.Name = "pnlList"
		Me.pnlList.Size = New System.Drawing.Size(545, 310)
		Me.pnlList.TabIndex = 5
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(551, 485)
		Me.Controls.Add(Me.TableLayoutPanel1)
		Me.Name = "MainForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Twitter"
		Me.TableLayoutPanel1.ResumeLayout(False)
		Me.TableLayoutPanel1.PerformLayout()
		Me.FlowLayoutPanel1.ResumeLayout(False)
		Me.FlowLayoutPanel1.PerformLayout()
		CType(Me.picStatus, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents tbMessage As System.Windows.Forms.TextBox
  Friend WithEvents btnTweet As System.Windows.Forms.Button
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
  Friend WithEvents picStatus As System.Windows.Forms.PictureBox
  Friend WithEvents lblStatus As System.Windows.Forms.Label
  Friend WithEvents btnMore As System.Windows.Forms.Button
  Friend WithEvents pnlList As System.Windows.Forms.Panel

End Class
