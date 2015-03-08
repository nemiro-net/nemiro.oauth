using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nemiro.OAuth;

namespace AspNetWebFormsCustomClient
{
  public partial class Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!this.IsPostBack)
      {
        this.Repeater1.DataSource = OAuthManager.RegisteredClients.Keys;
        this.Repeater1.DataBind();
      }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
      var btn = (Button)sender;

      // build callback url
      string returnUrl = new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri;

      // redirect to authorization page
      OAuthWeb.RedirectToAuthorization(btn.Attributes["data-provider"], returnUrl);
    }
  }
}