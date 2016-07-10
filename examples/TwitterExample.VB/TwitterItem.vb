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
Public Class TwitterItem

  Public Property Username As String
    Get
      Return lblUsername.Text
    End Get
    Set(value As String)
      lblUsername.Text = value
    End Set
  End Property

  Public Property Nickname As String
    Get
      Return lblNickname.Text
    End Get
    Set(value As String)
      lblNickname.Text = value
    End Set
  End Property

  Public Property DateCreated As String
    Get
      Return lblDate.Text
    End Get
    Set(value As String)
      lblDate.Text = value
    End Set
  End Property

  Public Overrides Property Text As String
    Get
      Return lblMessage.Text
    End Get
    Set(value As String)
      lblMessage.Text = value
    End Set
  End Property

  Private Sub lblMessage_TextChanged(sender As System.Object, e As System.EventArgs) Handles lblMessage.TextChanged
    TwitterItem_SizeChanged(Me, Nothing)
  End Sub

  Private Sub TwitterItem_SizeChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.SizeChanged
    Dim sz As New Size(Me.Width - Me.Padding.Horizontal - lblMessage.Margin.Horizontal, Int32.MaxValue)
    sz = TextRenderer.MeasureText(lblMessage.Text, lblMessage.Font, sz, TextFormatFlags.WordBreak Or TextFormatFlags.TextBoxControl)
    sz.Height += Me.Margin.Vertical
    If lblMessage.Height < sz.Height Then lblMessage.Height = sz.Height
    lblMessage.Width = Me.Width - lblMessage.Margin.Horizontal - Me.Padding.Horizontal
  End Sub

End Class