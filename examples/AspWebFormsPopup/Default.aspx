<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AspWebFormsPopup.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

			<asp:HyperLink NavigateUrl="~/RedirectToAuth.aspx?provider=facebook" onclick="return Popup(this.href);" ID="btnFacebook" runat="server" Text="[Connexion FaceBook]" EnableViewState="False" />
			<asp:HyperLink NavigateUrl="~/RedirectToAuth.aspx?provider=foursquare" onclick="return Popup(this.href);" ID="btnFoursquare" runat="server" Text="[Connexion Foursquare]" EnableViewState="False" />

			<script type="text/javascript">
				function Popup(url, width, height) {
					if(width == undefined || !width) { width = 550; }
					if(height == undefined || !height) { height = 450; }
					var x = (screen.width - width) / 2; 
					var y = (screen.height - height) / 2; 
					window.open(url, '_blank', 'toolbar=no,status=no,resizable=yes,scrollbars=yes,width=' + width + ',height=' + height + ',left=' + x + ',top=' + y);
					return false;
				}
			</script>

    </div>

    <hr />
    <small>Nemiro.OAuth v<%=System.Reflection.Assembly.GetAssembly(typeof(Nemiro.OAuth.OAuth2Client)).GetName().Version%></small>
    </form>
</body>
</html>
