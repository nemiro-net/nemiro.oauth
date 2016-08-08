<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoogleDriveWebForms.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
  <form id="form1" runat="server">
		<asp:Panel ID="pnlLogin" runat="server">
			<h3>Sign in to get an access token:</h3>
			<asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" />
		</asp:Panel>

		<asp:Panel ID="pnlUpload" runat="server">
			<h3>The access token is found in the Session!</h3>
			<h4>You can upload a file to Google Drive:</h4>
			<asp:FileUpload ID="FileUpload1" runat="server" /><br /><br />
			<asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />
		</asp:Panel>

		<asp:Panel ID="pnlSuccess" runat="server" Visible="false">
			<h3>File successfully sent!</h3>
			<asp:HyperLink ID="hlResult" runat="server" Target="_blank" />
			<br /><br />
			<a href="/">Upload another file</a>
		</asp:Panel>
  </form>
</body>
</html>