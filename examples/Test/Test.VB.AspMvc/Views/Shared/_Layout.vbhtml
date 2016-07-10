<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8" />
    <title>@ViewBag.Title</title>
    <link href="@Url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
  </head>

  <body>
    <div id="page">
      @RenderBody()
    </div>
  </body>
</html>