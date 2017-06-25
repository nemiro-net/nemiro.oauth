Imports Nemiro.OAuth
Imports System.Web.Mvc
Imports Test.Resources

Namespace Test.VB.AspMvc

  Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
      Return View()
    End Function

    Function Icon(id As String) As ActionResult
      ' image from resource
      Try
        Return File(CType(Images.ResourceManager.GetObject(id.ToLower().Replace(".", "")), Byte()), "image/png")
      Catch ex As Exception
        Return File(CType(Images.ResourceManager.GetObject("error"), Byte()), "image/png")
      End Try
    End Function

    ''' <summary>
    ''' Redirect to login
    ''' </summary>
    Public Function ExternalLogin(provider As String) As ActionResult
      ' build callback url
      Dim returnUrl As String = Url.Action("ExternalLoginResult", "Home", Nothing, Nothing, Request.Url.Host)

      ' not suppored localhost (it is only for localhost)
      Dim notSupportedLocalhost() As String = {"live", "mail.ru", "yahoo"}

      If notSupportedLocalhost.Any(Function(itm) itm.Equals(provider, StringComparison.OrdinalIgnoreCase)) Then
        returnUrl = String.Format("http://oauth.nemiro.net/oauth_redirect.html?returnUrl={0}", Server.UrlEncode(returnUrl))
      ElseIf provider.Equals("github", StringComparison.OrdinalIgnoreCase) Then
        returnUrl = String.Format("https://oauthproxy.nemiro.net/?returnUrl={0}", Server.UrlEncode(returnUrl))
      End If

      ' redirect to authorization page of the specified provider
      Try
        Return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl))
      Catch ex As Threading.ThreadAbortException
        Return Nothing
      Catch ex As Exception
        Return Content(ex.Message)
      End Try
    End Function

    ''' <summary>
    ''' Login result
    ''' </summary>
    Public Function ExternalLoginResult() As ActionResult
      Return View(OAuthWeb.VerifyAuthorization())
    End Function

  End Class

End Namespace