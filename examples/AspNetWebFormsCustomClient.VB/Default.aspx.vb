Imports Nemiro.OAuth

Public Class _Default
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not Me.IsPostBack Then
      Me.Repeater1.DataSource = OAuthManager.RegisteredClients.Keys
      Me.Repeater1.DataBind()
    End If
  End Sub

  Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
    Dim btn As Button = CType(sender, Button)

    ' build callback url
    Dim returnUrl As String = New Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri

    ' redirect to authorization page
    OAuthWeb.RedirectToAuthorization(btn.Attributes("data-provider"), returnUrl)
  End Sub

End Class