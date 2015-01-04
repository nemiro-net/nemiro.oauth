Imports Nemiro.OAuth
Imports Nemiro.OAuth.Clients

Public Class MvcApplication
  Inherits System.Web.HttpApplication

  Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
    filters.Add(New HandleErrorAttribute())
  End Sub

  Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
    routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

    routes.MapRoute( _
        "Default", _
        "{controller}/{action}/{id}", _
        New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional} _
    )

  End Sub

  Sub Application_Start()

    AreaRegistration.RegisterAllAreas()

    RegisterGlobalFilters(GlobalFilters.Filters)
    RegisterRoutes(RouteTable.Routes)

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
        "c37bece01b744a1daf501cb2643862eb",
        "189bc8c6fe8742fc9d6595cc8aeb1318"
      )
    )

    OAuthManager.RegisterClient _
    (
      New GitHubClient _
      (
        "e14122695d88f5c95bce",
        "cde23ec001c5180e01e865f4efb57cb0bc848c16"
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
      New DropboxClient _
      (
        "0le6wsyp3y085wy",
        "48afwq9yth83y7u"
      )
    )

    OAuthManager.RegisterClient _
    (
      New FoursquareClient _
      (
        "LHYZN1KUXN50L141QCQFNNVOYBGUE3G3FCWFZ3EEZTOZHY5Q",
        "HWXYFLLSS2IUQ0H4XNCDAZEFZKIU3MZRP5G55TNBDHRPNOQT"
      )
    )

    OAuthManager.RegisterClient _
    (
      New SoundCloudClient _
      (
        "42b58d31e399664a3fb8503bfcaaa9ba",
        "f9d85648da59fb95ec131b40c7645c31"
      )
    )

    OAuthManager.RegisterClient _
    (
      New LinkedInClient _
      (
        "75vufylz829iim",
        "VOf14z4T1jie4ezS"
      )
    )
  End Sub

End Class
