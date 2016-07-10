<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForms35._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title></title>
	</head>
	<body>
		<form id="form1" runat="server">
			<ul style="width:600px;margin-left:auto;margin-right:auto;">
				<asp:Repeater ID="RepeaterOAuth" runat="server" EnableViewState="false" onitemdatabound="RepeaterOAuth_ItemDataBound">
					<ItemTemplate>
						<asp:LinkButton ID="lnk" runat="server" />
					</ItemTemplate>
				</asp:Repeater>
			</ul>
		</form>
	</body>
</html>
