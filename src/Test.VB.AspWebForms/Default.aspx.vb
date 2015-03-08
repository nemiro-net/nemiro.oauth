Imports Nemiro.OAuth

Public Class _Default
  Inherits System.Web.UI.Page

  Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    ' output ouath providers list
    RepeaterOAuth.DataSource = Nemiro.OAuth.OAuthManager.RegisteredClients
    RepeaterOAuth.DataBind()
  End Sub

  Private Sub RepeaterOAuth_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RepeaterOAuth.ItemDataBound
    If Not (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then Return

    ' add provider name and click handler
    Dim lnk As LinkButton = CType(e.Item.FindControl("lnk"), LinkButton)
    lnk.Attributes.Add("data-provider", CType(e.Item.DataItem, KeyValuePair(Of ClientName, OAuthBase)).Key)
    AddHandler lnk.Click, AddressOf RedirectToLogin_Click

    ' add icon
    Dim img As Image = CType(e.Item.FindControl("img"), Image)
    img.ImageUrl = String.Format("~/Icon.ashx?id={0}", lnk.Attributes("data-provider"))
  End Sub

  Private Sub RedirectToLogin_Click(sender As Object, e As System.EventArgs)
    ' get provider name
    Dim provider As String = CType(sender, LinkButton).Attributes("data-provider")

    ' build callback url
    Dim returnUrl As String = New Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri

    ' not suppored localhost (it is only for localhost)
    Dim notSupportedLocalhost() As String = {"live", "mail.ru", "github"}
    If notSupportedLocalhost.Any(Function(itm) itm.Equals(provider, StringComparison.OrdinalIgnoreCase)) Then
      returnUrl = String.Format("http://oauth.nemiro.net/oauth_redirect.html?returnUrl={0}", Server.UrlEncode(returnUrl))
    End If

    ' redirect to authorization page of the specified provider
    OAuthWeb.RedirectToAuthorization(provider, returnUrl)
  End Sub

End Class