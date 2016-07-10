Imports Nemiro.OAuth

' For more details, please visit http://oauth.nemiro.net

Public Class Form2

  Public Sub New(providerName As String)
    InitializeComponent()
    WebBrowser1.ScriptErrorsSuppressed = True
    WebBrowser1.Navigate(OAuthWeb.GetAuthorizationUrl(providerName))
  End Sub

  Private Sub WebBrowser1_DocumentCompleted(sender As System.Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
    ' waiting for results
    If Not e.Url.Query.IndexOf("code=") = -1 OrElse Not e.Url.Fragment.IndexOf("code=") = -1 OrElse Not e.Url.Query.IndexOf("oauth_verifier=") = -1 Then
      ' is the result, verify
      Dim result = OAuthWeb.VerifyAuthorization(e.Url.ToString())
      If result.IsSuccessfully Then
        ' show user info
        MessageBox.Show _
        (
          String.Format _
          (
            "User ID: {0}{4}Username: {1}{4}Display Name: {2}{4}E-Mail: {3}",
            result.UserInfo.UserId,
            result.UserInfo.UserName,
            If(Not String.IsNullOrEmpty(result.UserInfo.DisplayName), result.UserInfo.DisplayName, result.UserInfo.FullName),
            result.UserInfo.Email,
            vbNewLine
          ),
          "Successfully",
          MessageBoxButtons.OK,
          MessageBoxIcon.Information
        )

        ' using access token for API request
        ' (this small example for vkontakte only)
        If result.ProviderName.Equals("vk", StringComparison.OrdinalIgnoreCase) Then
          Call New VkontakteFriends(result.AccessTokenValue).ShowDialog()
        End If
      Else
        ' show error message
        MessageBox.Show(result.ErrorInfo.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
      End If
      Me.Close()
    End If
  End Sub

End Class