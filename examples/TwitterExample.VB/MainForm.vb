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
Imports Nemiro.OAuth
Imports System.Collections.Specialized
Imports Nemiro.OAuth.LoginForms

Public Class MainForm

  ' https://dev.twitter.com/docs

  Public Property LastTweetId As String = Nothing

  Private Sub MainForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    If String.IsNullOrEmpty(My.Settings.AccessToken) OrElse String.IsNullOrEmpty(My.Settings.TokenSecret) OrElse String.IsNullOrEmpty(My.Settings.UserId) Then
      ' access token is empty
      ' get access token
      Me.GetAccessToken()
    Else
      Me.CheckAccessToken()
    End If
  End Sub

  Private Sub btnMore_Click(sender As System.Object, e As System.EventArgs) Handles btnMore.Click
    Me.GetTweets()
  End Sub

  Private Sub tbMessage_Leave(sender As System.Object, e As System.EventArgs) Handles tbMessage.Leave
    If String.IsNullOrEmpty(tbMessage.Text) Then
      tbMessage.Text = tbMessage.Tag.ToString()
    End If
  End Sub

  Private Sub tbMessage_Enter(sender As System.Object, e As System.EventArgs) Handles tbMessage.Enter
    If tbMessage.Text.Equals(tbMessage.Tag.ToString()) Then
      tbMessage.Text = ""
    End If
  End Sub

  Private Sub tbMessage_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles tbMessage.KeyPress
    If tbMessage.Text.Length > tbMessage.MaxLength AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = False
  End Sub

  Private Sub btnTweet_Click(sender As System.Object, e As System.EventArgs) Handles btnTweet.Click
    If String.IsNullOrEmpty(tbMessage.Text) OrElse tbMessage.Text.Equals(tbMessage.Tag.ToString()) Then
      MessageBox.Show("Text is required!", "Ouch!", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return
    End If

    Me.SendTweet()
  End Sub

#Region "..Twitter API.."

  Private Sub CheckAccessToken()
    Me.RequestStart()

    btnMore.Enabled = False
    btnTweet.Enabled = False

    Dim parameters As New HttpParameterCollection From _
    {
      New HttpUrlParameter("user_id", My.Settings.UserId),
      New HttpUrlParameter("include_entities", "false")
    }

    OAuthUtility.GetAsync _
    (
      "https://api.twitter.com/1.1/users/show.json",
      parameters:=parameters,
      authorization:=Me.GetAuth(),
      callback:=AddressOf CheckAccessToken_Result
    )
  End Sub
  Private Sub CheckAccessToken_Result(result As RequestResult)
    If Me.InvokeRequired Then
      Me.Invoke(New Action(Of RequestResult)(AddressOf CheckAccessToken_Result), result)
      Return
    End If

    If Not result.IsSuccessfully Then
      My.Settings.AccessToken = ""
      My.Settings.TokenSecret = ""
      My.Settings.Save()
      Me.GetAccessToken()
    Else
      Me.GetTweets()
    End If
  End Sub

  Private Sub GetAccessToken()
    If Me.InvokeRequired Then
      Me.Invoke(New Action(AddressOf GetAccessToken))
      Return
    End If

    Dim login As New TwitterLogin(My.Settings.ConsumerKey, My.Settings.ConsumerSecret) With {.Owner = Me}
    login.ShowDialog()

    If login.IsSuccessfully Then
      ' save access token to application settings
      My.Settings.AccessToken = CType(login.AccessToken, OAuthAccessToken).Value
      My.Settings.TokenSecret = CType(login.AccessToken, OAuthAccessToken).TokenSecret
      My.Settings.UserId = login.AccessToken("user_id").ToString()
      My.Settings.Save()
      ' get tweets
      Me.GetTweets()
    Else
      If MessageBox.Show("Please Click OK to login on Twitter or CANCEL for exit from the program.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = System.Windows.Forms.DialogResult.Cancel Then
        Me.Close()
      Else
        Me.GetAccessToken()
      End If
    End If
  End Sub

  ''' <summary>
  ''' 
  ''' </summary>
  ''' <remarks>https://dev.twitter.com/rest/reference/get/statuses/user_timeline</remarks>
  Private Sub GetTweets()
    Me.RequestStart()

    btnMore.Enabled = False
    btnTweet.Enabled = False

		Dim parameters As HttpParameterCollection = Nothing

		If Not String.IsNullOrEmpty(Me.LastTweetId) Then
			parameters = New HttpParameterCollection From {New HttpUrlParameter("max_id", Me.LastTweetId)}
		End If

		OAuthUtility.GetAsync _
    (
      "https://api.twitter.com/1.1/statuses/user_timeline.json",
      parameters:=parameters,
      authorization:=Me.GetAuth(),
      callback:=AddressOf GetTweets_Result
    )
  End Sub
  Private Sub GetTweets_Result(result As RequestResult)
    If Me.InvokeRequired Then
      Me.Invoke(New Action(Of RequestResult)(AddressOf GetTweets_Result), result)
      Return
    End If

    If result.IsSuccessfully Then
      Dim first As TwitterItem = Nothing
      For i As Integer = 0 To result.Count - 1
        If i = 0 AndAlso Not String.IsNullOrEmpty(Me.LastTweetId) Then Continue For
        Dim itm As UniValue = result(i)
        Dim tweet As New TwitterItem With _
        {
          .Dock = DockStyle.Top,
          .Username = String.Format("@{0}", itm("user")("screen_name")),
          .Nickname = itm("user")("name"),
          .DateCreated = Date.ParseExact(itm("created_at"), "ddd MMM dd HH:mm:ss zzzz yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString(),
          .Text = itm("text")
        }
        '.Top = If(pnlList.Controls.Count = 0, 0, pnlList.Controls(pnlList.Controls.Count - 1).Top + pnlList.Controls(pnlList.Controls.Count - 1).Height),
        pnlList.Controls.Add(tweet)
        pnlList.Controls.SetChildIndex(tweet, 0)
        Me.LastTweetId = itm("id_str")
        If i = 0 AndAlso String.IsNullOrEmpty(Me.LastTweetId) Then first = pnlList.Controls(0)
        If i = 1 AndAlso Not String.IsNullOrEmpty(Me.LastTweetId) Then first = pnlList.Controls(0)
      Next
      pnlList.ScrollControlIntoView(first)
    End If

    btnMore.Enabled = True
    btnTweet.Enabled = True

    RequestEnd(result)
  End Sub

  ''' <summary>
  ''' 
  ''' </summary>
  ''' <remarks>https://dev.twitter.com/rest/reference/post/statuses/update</remarks>
  Private Sub SendTweet()
    Me.RequestStart()

    tbMessage.Enabled = False
    btnTweet.Enabled = False

    Dim parameters As New HttpParameterCollection()
    parameters.AddFormParameter("status", Me.tbMessage.Text)

		OAuthUtility.PostAsync _
		(
			"https://api.twitter.com/1.1/statuses/update.json",
			parameters:=parameters,
			authorization:=Me.GetAuth(),
			callback:=AddressOf SendTweet_Result,
			contentType:="multipart/form-data"
		)
	End Sub
  Private Sub SendTweet_Result(result As RequestResult)
    If Me.InvokeRequired Then
      Me.Invoke(New Action(Of RequestResult)(AddressOf SendTweet_Result), result)
      Return
    End If

    If result.IsSuccessfully Then
      Dim tweet As New TwitterItem With _
      {
        .Dock = DockStyle.Top,
        .Username = String.Format("@{0}", result("user")("screen_name")),
        .Nickname = result("user")("name"),
        .DateCreated = Date.ParseExact(result("created_at"), "ddd MMM dd HH:mm:ss zzzz yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString(),
        .Text = result("text")
      }
      pnlList.Controls.Add(tweet)
      pnlList.Controls.SetChildIndex(tweet, 0)
      pnlList.ScrollControlIntoView(pnlList.Controls(0))
      tbMessage.Text = tbMessage.Tag.ToString()
    End If

    RequestEnd(result)

    tbMessage.Enabled = True
    btnTweet.Enabled = True
  End Sub

  Private Sub RequestStart()
    lblStatus.Text = "Please Wait..."
    picStatus.Image = My.Resources.loader2
  End Sub

  Private Sub RequestEnd(result As RequestResult)
    If Me.InvokeRequired Then
      Me.Invoke(New Action(Of RequestResult)(AddressOf RequestEnd), result)
      Return
    End If
    If Not result.IsSuccessfully Then
      If result("errors").HasValue Then
        lblStatus.Text = String.Join(vbCrLf, result("errors").Select(Function(itm) itm("message").ToString()).ToArray())
        MessageBox.Show(lblStatus.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
      ElseIf result("error").HasValue Then
        lblStatus.Text = result("error")
        MessageBox.Show(lblStatus.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
      Else
        lblStatus.Text = result.ToString()
        MessageBox.Show(result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
      End If
      picStatus.Image = My.Resources.err
    Else
      lblStatus.Text = "OK"
      picStatus.Image = My.Resources.success
    End If
  End Sub

  Private Function GetAuth() As OAuthAuthorization
    Dim auth As New OAuthAuthorization()
    auth.ConsumerKey = My.Settings.ConsumerKey
    auth.ConsumerSecret = My.Settings.ConsumerSecret
    auth.SignatureMethod = SignatureMethods.HMACSHA1
    auth.Token = My.Settings.AccessToken
    auth.TokenSecret = My.Settings.TokenSecret
    Return auth
  End Function

#End Region

End Class