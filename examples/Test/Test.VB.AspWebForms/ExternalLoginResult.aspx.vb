Imports Nemiro.OAuth

Public Class ExternalLoginResult
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    preResult.InnerHtml = ""
    Dim result As AuthorizationResult = OAuthWeb.VerifyAuthorization()
    preResult.InnerHtml += String.Format("Provider: {0}<br />", result.ProviderName)
    If result.IsSuccessfully Then
      'preResult.InnerHtml += "Успешно!<br /><br /><textarea style=""width:500px;"" rows=""15"">" + New JavaScriptSerializer().Serialize(result.UserInfo) + "</textarea><br /><br />"
      Dim user As UserInfo = result.UserInfo
      preResult.InnerHtml += String.Format("User ID:  {0}<br />", user.UserId)
      preResult.InnerHtml += String.Format("Name:     {0}<br />", user.DisplayName)
      preResult.InnerHtml += String.Format("Email:    {0}", user.Email)
    Else
      preResult.InnerHtml += result.ErrorInfo.ToString()
      If result.ErrorInfo.InnerException IsNot Nothing Then
        If result.ErrorInfo.GetType() Is GetType(RequestException) Then
          preResult.InnerHtml += CType(result.ErrorInfo, RequestException).RequestResult.ToString() & "<br /><br />"
          preResult.InnerHtml += result.ErrorInfo.InnerException.ToString()
        Else
          preResult.InnerHtml += result.ErrorInfo.InnerException.ToString()
        End If
      End If
    End If
  End Sub

End Class