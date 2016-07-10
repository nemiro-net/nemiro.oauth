@Code
  ViewBag.Title = "Index"
  Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Index</h2>

@Html.Raw(Test.Resources.Strings.IndexAbout)

@* !!!USE JAVASCRIPT LINKS TO PREVENT THEM FROM INDEXING!!! *@
<ul style="width:600px;margin-left:auto;margin-right:auto;">
  @For Each client As String In Nemiro.OAuth.OAuthManager.RegisteredClients.Keys
    @:<li class="@(If(client = "Yandex", "clear", ""))">
    @:  <a href="#" onclick="window.location.href='@(Url.Action("ExternalLogin", New With {.provider = client}))';return false;">
    @:    <img src="@(Url.Action("Icon", New With {.id = client}))" alt="@client" />
    @:  </a>
    @:</li>
  Next
	@If Not (Nemiro.OAuth.OAuthManager.RegisteredClients.Count Mod 5) = 0 Then
	 For i As Integer = 0 To 5 - (Nemiro.OAuth.OAuthManager.RegisteredClients.Count Mod 5) - 1
		@:<li><img src="@Url.Action("Icon", New With { .id = "empty" })" alt="empty" title="" width="100" height="100" /></li>
	 Next
	End If
</ul>

<div class="clear"></div>

<div class="message warning">
  <div class="title">@Test.Resources.Strings.Warning</div>
  <div class="text">@Test.Resources.Strings.UseJavaScriptLinks</div>
</div>

<div class="message warning">
  <div class="title">@Test.Resources.Strings.Warning</div>
  <div class="text">@Html.Raw(String.Format(Test.Resources.Strings.Port59962, 48179))</div>
</div>

<div class="message note">
  <div class="title">@Test.Resources.Strings.Note</div>
  <div class="text">@Html.Raw(Test.Resources.Strings.SeeAlsoDemo)</div>
</div>