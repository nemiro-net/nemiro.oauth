<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalLoginResult.aspx.cs" Inherits="Test.CSharp.AspWebForms.ExternalLoginResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1" runat="server">
    <title>Login Result</title>
    <link href="Site.css" rel="stylesheet" type="text/css" />
  </head>
  <body>
    <form id="form1" runat="server">
      <div id="page">
        <h2>Login Result</h2>
        <pre id="preResult" runat="server" />
        <hr />
        <asp:HyperLink ID="hlTryAgain" runat="server" Text="Try again" NavigateUrl="~/" />
      </div>
    </form>
  </body>
</html>