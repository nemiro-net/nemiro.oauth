Imports Nemiro.OAuth
Imports Nemiro.OAuth.Clients

Public Class Global_asax
  Inherits System.Web.HttpApplication

  Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    ' OAuth clients registration
    ' NOTE: Specify their own client IDs and secret keys
    OAuthManager.RegisterClient _
    (
      New FacebookClient _
      (
        "1435890426686808",
        "c6057dfae399beee9e8dc46a4182e8fd"
      )
    )

    OAuthManager.RegisterClient _
    (
      New TwitterClient _
      (
        "1Ayh2ZM2l9chloiFsmxNpi7Gg",
        "dbVXKWVIlH5fRuVI0FvE2ZDsZAbBg74UrGFYwW1kLSwc0ceJnJ"
      )
    )

    OAuthManager.RegisterClient _
    (
      New VkontakteClient _
      (
        "2419779",
        "31nnASa9T1eO150VCFgr"
      )
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
      New GoogleClient _
      (
        "1058655871432-83b9micke7cll89jfmcno5nftha3e95o.apps.googleusercontent.com",
        "AeEbEGQqoKgOZb41JUVLvEJL"
      )
    )

    OAuthManager.RegisterClient _
    (
      New YandexClient _
      (
        "4a8ae8ecb56a4e51841d24480d0964d6",
        "6f97151aa65c4c3a865f981c1fa8ca85"
      )
    )

    OAuthManager.RegisterClient _
    (
      New LiveClient _
      (
        "0000000040124265",
        "6ViSGIbw9N59s5Ndsfz-zaeezlBt62Ep"
      )
    )

    OAuthManager.RegisterClient _
    (
      New AmazonClient _
      (
        "amzn1.application-oa2-client.f0ffe4edc256488dae00dcaf96d75d1b",
        "764dcefe49b441c8c6244c93e5d5d04de54fda6dfdc83da9693bf346f4dc4515"
      ) With {.ReturnUrl = "http://localhost"}
    )

    OAuthManager.RegisterClient _
    (
      New FoursquareClient _
      (
        "LHYZN1KUXN50L141QCQFNNVOYBGUE3G3FCWFZ3EEZTOZHY5Q",
        "HWXYFLLSS2IUQ0H4XNCDAZEFZKIU3MZRP5G55TNBDHRPNOQT"
      )
    )
  End Sub

End Class