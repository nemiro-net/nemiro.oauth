Imports Nemiro.OAuth
Imports Nemiro.OAuth.Clients
Imports System.Collections.Specialized

' For more details, please visit http://oauth.nemiro.net

Public Class Form1

  Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    ' OAuth clients registration
    ' NOTE: Specify their own client IDs and secret keys
    OAuthManager.RegisterClient _
    (
      New FacebookClient _
      (
        "1435890426686808",
        "c6057dfae399beee9e8dc46a4182e8fd"
      ) _
      With
      {
        .Parameters = New NameValueCollection() From {{"display", "popup"}}
      }
    )

    OAuthManager.RegisterClient _
    (
      New VkontakteClient _
      (
        "4457505",
        "wW5lFMVbsw0XwYFgCGG0"
      ) _
      With
      {
        .Parameters = New NameValueCollection() From {{"display", "popup"}},
        .Scope = "status,friends,email"
      }
    )

    OAuthManager.RegisterClient _
    (
      New MailRuClient _
      (
        "722701",
        "d0622d3d9c9efc69e4ca42aa173b938a"
      )
    )

    OAuthManager.RegisterClient _
    (
      New YandexClient _
      (
        "0ee5f0bf2cd141a1b194a2b71b0332ce",
        "59d76f7c09b54ad38e6b15f792da7a9a"
      )
    )

    ' add buttons to the form
    For Each providerName As String In OAuthManager.RegisteredClients.Keys
      Dim btn As New Button()
      btn.Text = providerName
      AddHandler btn.Click, AddressOf btn_Click
      flowLayoutPanel1.Controls.Add(btn)
    Next
  End Sub

  Private Sub btn_Click(sender As System.Object, e As System.EventArgs)
    Call New Form2(CType(sender, Button).Text).ShowDialog()
  End Sub

End Class
