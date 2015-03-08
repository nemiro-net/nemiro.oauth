<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Test.CSharp.AspWebForms.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Index</title>
    <link href="Site.css" rel="stylesheet" type="text/css" />
  </head>
  <body>
    <form id="form1" runat="server">
      <div id="page">
        <h2>Index</h2>

        <%=Test.Resources.Strings.DefaultAbout%>

        <%-- !!!USE JAVASCRIPT LINKS TO PREVENT THEM FROM INDEXING!!! --%>
        <ul style="width:600px;margin-left:auto;margin-right:auto;">
          <asp:Repeater ID="RepeaterOAuth" runat="server" EnableViewState="false"
            onitemdatabound="RepeaterOAuth_ItemDataBound"
					>
            <ItemTemplate>
              <asp:LinkButton ID="lnk" runat="server">
                <asp:Image ID="img" runat="server" />
              </asp:LinkButton>
            </ItemTemplate>
          </asp:Repeater>
        </ul>

        <div class="clear"></div>

        <div class="message warning">
          <div class="title"><%=Test.Resources.Strings.Warning%></div>
          <div class="text"><%=Test.Resources.Strings.UseJavaScriptLinks%></div>
        </div>

        <div class="message warning">
          <div class="title"><%=Test.Resources.Strings.Warning%></div>
          <div class="text"><%=String.Format(Test.Resources.Strings.Port59962, 56027)%></div>
        </div>

        <div class="message note">
          <div class="title"><%=Test.Resources.Strings.Note%></div>
          <div class="text"><%=Test.Resources.Strings.SeeAlsoDemo%></div>
        </div>
      </div>

    </form>
  </body>
</html>