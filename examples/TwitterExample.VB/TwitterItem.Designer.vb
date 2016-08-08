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
Partial Class TwitterItem
  Inherits System.Windows.Forms.UserControl

  'Пользовательский элемент управления (UserControl) переопределяет метод Dispose для очистки списка компонентов.
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
		Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
		Me.lblNickname = New System.Windows.Forms.Label()
		Me.lblUsername = New System.Windows.Forms.Label()
		Me.lblDate = New System.Windows.Forms.Label()
		Me.lblMessage = New System.Windows.Forms.Label()
		Me.FlowLayoutPanel1.SuspendLayout()
		Me.SuspendLayout()
		'
		'FlowLayoutPanel1
		'
		Me.FlowLayoutPanel1.Controls.Add(Me.lblNickname)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblUsername)
		Me.FlowLayoutPanel1.Controls.Add(Me.lblDate)
		Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
		Me.FlowLayoutPanel1.Location = New System.Drawing.Point(2, 4)
		Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
		Me.FlowLayoutPanel1.Size = New System.Drawing.Size(479, 23)
		Me.FlowLayoutPanel1.TabIndex = 0
		'
		'lblNickname
		'
		Me.lblNickname.AutoSize = True
		Me.lblNickname.Dock = System.Windows.Forms.DockStyle.Left
		Me.lblNickname.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
		Me.lblNickname.Location = New System.Drawing.Point(3, 0)
		Me.lblNickname.Name = "lblNickname"
		Me.lblNickname.Size = New System.Drawing.Size(118, 16)
		Me.lblNickname.TabIndex = 0
		Me.lblNickname.Text = "Aleksey Nemiro"
		Me.lblNickname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblUsername
		'
		Me.lblUsername.AutoSize = True
		Me.lblUsername.Dock = System.Windows.Forms.DockStyle.Left
		Me.lblUsername.ForeColor = System.Drawing.SystemColors.ButtonShadow
		Me.lblUsername.Location = New System.Drawing.Point(127, 0)
		Me.lblUsername.Name = "lblUsername"
		Me.lblUsername.Size = New System.Drawing.Size(91, 16)
		Me.lblUsername.TabIndex = 1
		Me.lblUsername.Text = "@aleksey_nemiro"
		Me.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblDate
		'
		Me.lblDate.AutoSize = True
		Me.lblDate.Dock = System.Windows.Forms.DockStyle.Left
		Me.lblDate.Location = New System.Drawing.Point(224, 0)
		Me.lblDate.Name = "lblDate"
		Me.lblDate.Size = New System.Drawing.Size(91, 16)
		Me.lblDate.TabIndex = 2
		Me.lblDate.Text = "09.12.2014 14:34"
		Me.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'lblMessage
		'
		Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
		Me.lblMessage.Location = New System.Drawing.Point(5, 30)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.Size = New System.Drawing.Size(89, 18)
		Me.lblMessage.TabIndex = 1
		Me.lblMessage.Text = "Hello, world!"
		'
		'TwitterItem
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoSize = True
		Me.Controls.Add(Me.lblMessage)
		Me.Controls.Add(Me.FlowLayoutPanel1)
		Me.Name = "TwitterItem"
		Me.Padding = New System.Windows.Forms.Padding(2, 4, 2, 4)
		Me.Size = New System.Drawing.Size(483, 100)
		Me.FlowLayoutPanel1.ResumeLayout(False)
		Me.FlowLayoutPanel1.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
  Friend WithEvents lblNickname As System.Windows.Forms.Label
  Friend WithEvents lblUsername As System.Windows.Forms.Label
  Friend WithEvents lblDate As System.Windows.Forms.Label
  Friend WithEvents lblMessage As System.Windows.Forms.Label

End Class
