<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VkontakteFriends
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
    Me.listBox1 = New System.Windows.Forms.ListBox()
    Me.SuspendLayout()
    '
    'listBox1
    '
    Me.listBox1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.listBox1.FormattingEnabled = True
    Me.listBox1.Location = New System.Drawing.Point(0, 0)
    Me.listBox1.Name = "listBox1"
    Me.listBox1.Size = New System.Drawing.Size(245, 134)
    Me.listBox1.TabIndex = 1
    '
    'VkontakteFriends
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(245, 134)
    Me.Controls.Add(Me.listBox1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "VkontakteFriends"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Vkontakte friends"
    Me.ResumeLayout(False)

  End Sub
  Private WithEvents listBox1 As System.Windows.Forms.ListBox
End Class
