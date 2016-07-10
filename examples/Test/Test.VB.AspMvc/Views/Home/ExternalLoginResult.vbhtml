@Code
    ViewData("Title") = "Login Result"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Login Result</h2>
<pre>
  @If Model.IsSuccessfully Then
    Dim user As Nemiro.OAuth.UserInfo = Model.UserInfo
    @:Provider: @Model.ProviderName
    @:User ID:  @user.UserId
    @:Name:     @user.DisplayName
    @:Email:    @user.Email
  Else
    @Model.ErrorInfo.Message
  End If
</pre>

<hr />

@Html.ActionLink("Try again", "Index")