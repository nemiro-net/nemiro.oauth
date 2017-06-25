# Nemiro.OAuth

**Nemiro.OAuth** – это библиотека классов для реализации авторизации по протоколу **OAuth** в проектах **.NET Framework**.

Библиотека предоставляет полностью готовые клиенты **OAuth** для популярных веб-сайтов с реализованными функциями получения базовой информации о пользователях.

Библиотека в первую очередь создавалась для использования в веб-проектах (**ASP.NET MVC**, **WebForms**), но она также может быть использована в консольных приложениях, проектах **Windows Forms** и, теоретически, в других типах проектов.

Исходный код **Nemiro.OAuth** поставляется на условиях лицензии **Apache License Version 2.0**.

Для установки **Nemiro.OAuth** выполните следующую команду в консоли диспетчера пакетов (**Package Manager Console**):

`PM> Install-Package Nemiro.OAuth`

## Демонстрация

Посмотреть на работу библиотеки **Nemiro.OAuth** можно на демонстрационном сайте:

[demo-oauth.nemiro.net](http://demo-oauth.nemiro.net/)

## Особенности

* Поддержка протоколов **OAuth 1.0** и **2.0**;
* Получение базовой информации о пользователях (если поставщик поддерживает и достаточно разрешений): ID, полное имя, пол, дата рождения, email и номер телефона;
* **OAuth**-клиенты «из коробки» для: 
  **Amazon**, **Assembla**, **CodeProject**, 
  **Dropbox**, **Facebook**, **Foursquare**, 
  **GitHub**, **Google**, **Instagram**, 
  **LinkedIn**, **Microsoft Live**, **Mail.Ru**, 
  **Odnoklassniki**, **SoundCloud**, **SourceForge**, 
  **Tumblr**, **Twitter**, **VK**, 
  **Yahoo!**, **Yandex**;
* Классы для создания собственных клиентов;
* Унифицированные методы работы с разнообразными **API**.

Меньше кода, больше возможностей!

### Системные требования

* .NET Framework 3.5, 4.0, 4.5, 4.6 или 4.7

### Лицензия

**Nemiro.OAuth** поставляется на условиях лицензии **Apache License Version 2.0**.

### Использование

1\. На сайте поставщика **OAuth** зарегистрируйте своё приложение.

2\. Поставщик выдаст идентификатор и секретный ключ вашего приложения. 
Используйте их при регистрации поставщика в своём проекте.
В веб-проектах это лучше всего делать в файле `Global.asax`, в обработчике события `Application_Start`.

Например, **Facebook**:

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

3\. Создайте страницу обратного вызова и добавьте код получения информации из профиля пользователя с сайта поставщика.

Например:

**C#**
```C#
public partial class ExternalLoginResult : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    var result = OAuthWeb.VerifyAuthorization();
    Response.Write(String.Format("Поставщик: {0}<br />", result.ProviderName));
    if (result.IsSuccessfully)
    {
      var user = result.UserInfo;
      Response.Write(String.Format("Идентификатор:     {0}<br />", user.UserId));
      Response.Write(String.Format("Отображаемое имя:  {0}<br />", user.DisplayName));
      Response.Write(String.Format("Электронная почта: {0}", user.Email));
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
      Response.Write(String.Format("Идентификатор:     {0}<br />", user.UserId))
      Response.Write(String.Format("Отображаемое имя:  {0}<br />", user.DisplayName))
      Response.Write(String.Format("Электронная почта: {0}", user.Email))
    Else
      Response.Write(result.ErrorInfo.ToString())
    End If
  End Sub

End Class
```

4\. Создайте ссылку для перенаправления пользователя на сайт поставщика **OAuth** для авторизации.

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

5\. Наслаждайтесь!

### См. также

* [Справочник](http://oauth.nemiro.net)
* [Online-демонстрация](http://demo-oauth.nemiro.net/)
* [Готовые формы для Windows Forms](https://github.com/alekseynemiro/Nemiro.OAuth.LoginForms)
* [Другие проекты](http://nemiro.net)
