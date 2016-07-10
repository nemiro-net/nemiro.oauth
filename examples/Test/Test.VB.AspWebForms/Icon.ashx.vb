Imports System.Web
Imports System.Web.Services

Public Class Icon
  Implements System.Web.IHttpHandler

  Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
    context.Response.ContentType = "image/png"
    Dim buffer() As Byte = Nothing
    Try
      buffer = CType(Test.Resources.Images.ResourceManager.GetObject(context.Request.QueryString("id").ToLower().Replace(".", "")), Byte())
    Catch ex As Exception
      buffer = CType(Test.Resources.Images.ResourceManager.GetObject("error"), Byte())
    End Try
    context.Response.OutputStream.Write(buffer, 0, buffer.Length)
  End Sub

  ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
    Get
      Return False
    End Get
  End Property

End Class