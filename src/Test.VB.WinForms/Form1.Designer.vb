<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
    Me.flowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
    Me.SuspendLayout()
    '
    'flowLayoutPanel1
    '
    Me.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.flowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
    Me.flowLayoutPanel1.Name = "flowLayoutPanel1"
    Me.flowLayoutPanel1.Padding = New System.Windows.Forms.Padding(24)
    Me.flowLayoutPanel1.Size = New System.Drawing.Size(259, 176)
    Me.flowLayoutPanel1.TabIndex = 1
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(259, 176)
    Me.Controls.Add(Me.flowLayoutPanel1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.MaximizeBox = False
    Me.Name = "Form1"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Select provider"
    Me.ResumeLayout(False)

  End Sub
  Private WithEvents flowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel

End Class
