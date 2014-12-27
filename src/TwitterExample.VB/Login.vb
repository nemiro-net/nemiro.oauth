' ----------------------------------------------------------------------------
' Copyright (c) Aleksey Nemiro, 2014. All rights reserved.
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
Imports Nemiro.OAuth
Imports Nemiro.OAuth.Clients
Imports System.Threading.Tasks

Public Class Login

  Public Twitter = New TwitterClient(My.Settings.ConsumerKey, My.Settings.ConsumerSecret)

  Private Sub Login_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    ' open login page
    Me.WebBrowser1.Navigate(Me.Twitter.AuthorizationUrl)
  End Sub

  Private Sub WebBrowser1_DocumentCompleted(sender As System.Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
    'waiting for results
    If e.Url.ToString().Equals("https://api.twitter.com/oauth/authorize", StringComparison.OrdinalIgnoreCase) Then
      If Me.WebBrowser1.Document.GetElementsByTagName("code").Count > 0 Then
        ' found authorization code
        Try
          ' verify code
          Me.Twitter.AuthorizationCode = Me.WebBrowser1.Document.GetElementsByTagName("code")(0).InnerText
          ' show progress
          Me.WebBrowser1.Visible = False
          Me.PictureBox1.Visible = True
          ' save access token to application settings
          Task.Factory.StartNew _
          ( _
            Sub()
              My.Settings.AccessToken = CType(Me.Twitter.AccessToken, OAuthAccessToken).Value
              My.Settings.TokenSecret = CType(Me.Twitter.AccessToken, OAuthAccessToken).TokenSecret
              My.Settings.UserId = Me.Twitter.AccessToken("user_id").ToString()
              My.Settings.Save()
              Me.Complete()
            End Sub
          )
        Catch ex As Exception
          ' show error message
          MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
          Me.Complete()
        End Try
      Else
        ' the user has refused to give permission 
        Me.Complete()
      End If
    End If
  End Sub

  Private Sub Complete()
    If Me.InvokeRequired Then
      Me.Invoke(New Action(AddressOf Complete))
      Return
    End If
    Me.Close()
  End Sub

End Class