Imports System.Collections.Specialized
Imports Nemiro.OAuth

Public Class VkontakteFriends

  Public Sub New(accessToken As String)
    InitializeComponent()

    ' get user friends
    ' query parameters
    Dim parameters As New NameValueCollection() From _
    {
      {"access_token", accessToken},
      {"count", "10"},
      {"fields", "nickname"}
    }

    ' execute the request
    Dim result As RequestResult = OAuthUtility.ExecuteRequest _
    (
      "GET",
      "https://api.vk.com/method/friends.get",
      parameters,
      Nothing
    )

    For Each itm As UniValue In result("response")
      Dim friendName As String = ""
      If itm.ContainsKey("first_name") AndAlso itm.ContainsKey("last_name") Then
        friendName = String.Format("{0} {1}", itm("first_name"), itm("last_name"))
      Else
        friendName = itm("nickname").ToString()
      End If
      listBox1.Items.Add(friendName)
    Next
  End Sub

End Class