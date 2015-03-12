Imports Nemiro.OAuth

Public Class MyFacebookClient
  Inherits OAuth2Client

  ''' <summary>
  ''' Unique provider name: <b>MyFacebook</b>.
  ''' </summary>
  Public Overrides ReadOnly Property ProviderName As String
    Get
      Return "MyFacebook"
    End Get
  End Property

  ''' <summary>
  ''' Return URL.
  ''' </summary>
  Public Overrides Property ReturnUrl As String
    Get
      If String.IsNullOrEmpty(MyBase.ReturnUrl) Then
        ' default return url
        Return "https://www.facebook.com/connect/login_success.html"
      End If
      Return MyBase.ReturnUrl
    End Get
    Set(value As String)
      MyBase.ReturnUrl = value
    End Set
  End Property

  ''' <summary>
  ''' Initializes a new instance of the <see cref="MyFacebookClient"/>.
  ''' </summary>
  ''' <param name="clientId">The App ID obtained from the <see href="https://developers.facebook.com/apps/">Facebook Developers</see>.</param>
  ''' <param name="clientSecret">The App Secret obtained from the <see href="https://developers.facebook.com/apps/">Facebook Developers</see>.</param>
  Public Sub New(clientId As String, clientSecret As String)
    MyBase.New("https://www.facebook.com/dialog/oauth", "https://graph.facebook.com/oauth/access_token", clientId, clientSecret)
    ' scope list
    MyBase.ScopeSeparator = ","
    MyBase.DefaultScope = "public_profile,email"
  End Sub

  ''' <summary>
  ''' Gets the user details.
  ''' </summary>
  ''' <remarks>
  ''' <para>
  ''' For more details, please see <see href="https://developers.facebook.com/docs/graph-api/reference/v2.0/user">User</see> method in <b>Guide of Facebook Graph API</b>.
  ''' </para>
  ''' </remarks>
  ''' <returns>
  ''' <para>Returns an instance of the <see cref="UserInfo"/> class, containing information about the user.</para>
  ''' </returns>
  Public Overrides Function GetUserInfo(Optional ByVal accessToken As AccessToken = Nothing) As UserInfo
    accessToken = MyBase.GetSpecifiedTokenOrCurrent(accessToken)

    Dim parameters As New NameValueCollection() From {{"access_token", accessToken.ToString()}}
    Dim result As RequestResult = OAuthUtility.[Get]("https://graph.facebook.com/me", parameters)

    Dim map As New ApiDataMapping()
    map.Add("id", "UserId", GetType(String))
    map.Add("first_name", "FirstName")
    map.Add("last_name", "LastName")
    map.Add("name", "DisplayName")
    map.Add("email", "Email")
    map.Add("link", "Url")
    map.Add("birthday", "Birthday", GetType(DateTime), "MM\/dd\/yyyy")
    map.Add _
    (
      "gender", "Sex",
      Function(value As UniValue)
        If value.HasValue Then
          If value.Equals("male", StringComparison.OrdinalIgnoreCase) Then
            Return Sex.Male
          ElseIf value.Equals("female", StringComparison.OrdinalIgnoreCase) Then
            Return Sex.Female
          End If
        End If
        Return Sex.None
      End Function
    )

    Return New UserInfo(result, map)
  End Function


End Class
