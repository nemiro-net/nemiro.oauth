Imports Nemiro.OAuth

Public Class MyTwitterClient
  Inherits OAuthClient

  ''' <summary>
  ''' Unique provider name: <b>Twitter</b>.
  ''' </summary>
  Public Overrides ReadOnly Property ProviderName As String
    Get
      ' The name is the same as in the library.
      ' If in the application is registered client of the library,
      ' then you will have to use a different name.
      Return "Twitter"
    End Get
  End Property

  ''' <summary>
  ''' Initializes a new instance of the class.
  ''' </summary>
  Public Sub New(consumerKey As String, consumerSecret As String)
    MyBase.New _
    (
      "https://api.twitter.com/oauth/request_token",
      "https://api.twitter.com/oauth/authorize",
      "https://api.twitter.com/oauth/access_token",
      consumerKey,
      consumerSecret,
      SignatureMethods.HMACSHA1
    )
  End Sub

  Public Overrides Function GetUserInfo(Optional ByVal accessToken As AccessToken = Nothing) As Nemiro.OAuth.UserInfo
    accessToken = MyBase.GetSpecifiedTokenOrCurrent(accessToken)

    Dim url As String = "https://api.twitter.com/1.1/users/show.json"

    Dim parameters As New HttpParameterCollection()
    parameters.AddUrlParameter("user_id", Me.AccessToken("user_id").ToString())
    parameters.AddUrlParameter("screen_name", Me.AccessToken("screen_name").ToString())
    parameters.AddUrlParameter("include_entities", "false")
    Me.Authorization("oauth_token") = Me.AccessToken("oauth_token")
    Me.Authorization.TokenSecret = Me.AccessToken("oauth_token_secret").ToString()
    Dim result As RequestResult = OAuthUtility.[Get](url, parameters, Me.Authorization)

    Dim map As New ApiDataMapping()
    map.Add("id_str", "UserId", GetType(String))
    map.Add("name", "DisplayName")
    map.Add("screen_name", "UserName")
    map.Add("profile_image_url", "Userpic")
    map.Add("url", "Url")
    map.Add("birthday", "Birthday", GetType(DateTime), "dd\.MM\.yyyy")

    Return New UserInfo(result, map)
  End Function

End Class
