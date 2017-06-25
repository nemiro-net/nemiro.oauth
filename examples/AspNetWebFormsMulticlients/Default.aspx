<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AspNetWebFormsMulticlients.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

			<ul>
				<%foreach (var name in Nemiro.OAuth.OAuthManager.RegisteredClients.Keys) {%>
					<li>
						<a href="<%=ResolveUrl(String.Format("~/RedirectToAuth.aspx?provider={0}", name.Hash))%>" onclick="<%=name.Key == "popup" ? "return Popup(this.href);" : "return true;"%>">
							<%=name.ProviderName%> (<%=name.Key%>)
						</a>
					</li>
				<%}%>
			</ul>

            <hr />
            <small>Nemiro.OAuth v<%=System.Reflection.Assembly.GetAssembly(typeof(Nemiro.OAuth.OAuth2Client)).GetName().Version%></small>

			<script type="text/javascript">
				function Popup(url, width, height) {
					if(width == undefined || !width) { width = 550; }
					if(height == undefined || !height) { height = 450; }
					var x = (screen.width - width) / 2;
					var y = (screen.height - height) / 2;
					url += '&popup=true';
					window.open(url, '_blank', 'toolbar=no,status=no,resizable=yes,scrollbars=yes,width=' + width + ',height=' + height + ',left=' + x + ',top=' + y);
					return false;
				}
			</script>

    </div>
    </form>
</body>
</html>
