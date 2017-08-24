# Nemiro.OAuth

**Nemiro.OAuth** is a class library for authorization via **OAuth** protocol in **.NET Framework**.

The library provides mechanisms for implementing **OAuth** clients, and also contains a ready-to-use clients for popular websites.

**Nemiro.OAuth** is distributed under **Apache License Version 2.0**.

To install **Nemiro.OAuth**, run the following command in the **Package Manager Console**:

`PM> Install-Package Nemiro.OAuth`

## Online Demo

[demo-oauth.nemiro.net](http://demo-oauth.nemiro.net/)

## Features

* Support OAuth 1.0 and 2.0; 
* Obtaining basic information about users: ID, name, sex, date of birth, email address and telephone number; 
* Ready-to-Use OAuth clients for:
  Amazon, Assembla, CodeProject, 
  Dropbox, Facebook, Foursquare, 
  GitHub, Google, Instagram, 
  LinkedIn, Microsoft Live, Mail.Ru, 
  Odnoklassniki (odnoklassniki.ru), SoundCloud, SourceForge, 
  Tumblr, Twitter, VK (vkontakte, vk.com), 
  Yahoo!, Yandex (yandex.ru);
* Base classes to create additional clients; 
* Basic principles of operation with API of different providers;
* Unified mechanisms to facilitate integration with a variety of API.

Less code, more functionality!

## System Requirements

* .NET Framework 3.5, 4.0, 4.5, 4.6 or 4.7

## License

**Nemiro.OAuth** is distributed under **Apache License Version 2.0**.

## How to use

1\. Create an application at the **OAuth** provider site.

2\. Use these credentials for registration of an **OAuth** client in your project.

For example, **Facebook**:

**C#**
```C#
OAuthManager.RegisterClient
(
  "facebook", 
  "1435890426686808", 
  "c6057dfae399beee9e8dc46a4182e8fd"
);
```

**Visual Basic .NET**
```VBNet
OAuthManager.RegisterClient _
(
  "facebook", 
  "1435890426686808", 
  "c6057dfae399beee9e8dc46a4182e8fd"
)
```

3\. Create a page to handle the callback. And add code to obtain user data with external server.

For example:

**C#**
```C#
public partial class ExternalLoginResult : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    var result = OAuthWeb.VerifyAuthorization();
    Response.Write(String.Format("Provider: {0}<br />", result.ProviderName));
    if (result.IsSuccessfully)
    {
      var user = result.UserInfo;
      Response.Write(String.Format("User ID:  {0}<br />", user.UserId));
      Response.Write(String.Format("Name:     {0}<br />", user.DisplayName));
      Response.Write(String.Format("Email:    {0}", user.Email));
    }
    else
    {
      Response.Write(result.ErrorInfo.Message);
    }
  }
}
```

**Visual Basic .NET**
```VBNet
Public Class ExternalLoginResult
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim result As AuthorizationResult = OAuthWeb.VerifyAuthorization()
    Response.Write(String.Format("Provider: {0}<br />", result.ProviderName))
    If result.IsSuccessfully Then
      Dim user As UserInfo = result.UserInfo
      Response.Write(String.Format("User ID:  {0}<br />", user.UserId))
      Response.Write(String.Format("Name:     {0}<br />", user.DisplayName))
      Response.Write(String.Format("Email:    {0}", user.Email))
    Else
      Response.Write(result.ErrorInfo.ToString())
    End If
  End Sub

End Class
```

4\. Get the address for authentication and redirect the user to it.

**C#**
```C#
string returnUrl =  new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri;
OAuthWeb.RedirectToAuthorization("facebook", returnUrl);
```

**Visual Basic .NET**
```VBNet
Dim returnUrl As String = New Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri
OAuthWeb.RedirectToAuthorization("facebook", returnUrl)
```

5\. Enjoy!

### See Also

* [Guide Nemiro.OAuth](http://oauth.nemiro.net)
* [Online Demo](http://demo-oauth.nemiro.net/)
* [Forms for Windows Applications](https://github.com/alekseynemiro/Nemiro.OAuth.LoginForms)
* [Other projects](http://nemiro.net)
