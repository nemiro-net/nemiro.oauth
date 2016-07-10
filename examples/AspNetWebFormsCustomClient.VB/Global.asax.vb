Imports System.Web.SessionState
Imports Nemiro.OAuth

Public Class Global_asax
  Inherits System.Web.HttpApplication

  Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    ' IMPORTANT: You can not register clients by provider name. 
    ' Registration should be carried out through the creation of instances of the class.

    ' custom facebook client
    OAuthManager.RegisterClient _
    (
      New MyFacebookClient _
      (
        "1435890426686808",
        "c6057dfae399beee9e8dc46a4182e8fd"
      )
    )

    ' standard twitter client
    OAuthManager.RegisterClient _
    (
      New Nemiro.OAuth.Clients.TwitterClient _
      (
        "1Ayh2ZM2l9chloiFsmxNpi7Gg",
        "dbVXKWVIlH5fRuVI0FvE2ZDsZAbBg74UrGFYwW1kLSwc0ceJnJ"
      )
    )

    ' custom twitter client
    OAuthManager.RegisterClient _
    (
      "Custom",
      New MyTwitterClient _
      (
        "1Ayh2ZM2l9chloiFsmxNpi7Gg",
        "dbVXKWVIlH5fRuVI0FvE2ZDsZAbBg74UrGFYwW1kLSwc0ceJnJ"
      )
    )
  End Sub

End Class