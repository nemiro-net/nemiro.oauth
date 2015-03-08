Imports Nemiro.OAuth

Public Class ExternalLoginResult
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    preResult.InnerHtml = ""
    Dim result As AuthorizationResult = OAuthWeb.VerifyAuthorization()
    preResult.InnerHtml &= String.Format("Client:   {0}<br />", result.ClientName.Key)
    preResult.InnerHtml &= String.Format("Provider: {0}<br />", result.ProviderName)
    If result.IsSuccessfully Then
      Dim user As UserInfo = result.UserInfo
      preResult.InnerHtml &= String.Format("User ID:  {0}<br />", user.UserId)
      preResult.InnerHtml &= String.Format("Name:     {0}<br />", user.DisplayName)
      preResult.InnerHtml &= String.Format("Email:    {0}", user.Email)
    Else
      preResult.InnerHtml &= result.ErrorInfo.Message
    End If
  End Sub

End Class