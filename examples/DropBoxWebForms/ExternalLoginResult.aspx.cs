using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nemiro.OAuth;

namespace DropBoxWebForms
{

  public partial class ExternalLoginResult : System.Web.UI.Page
  {

    protected void Page_Load(object sender, EventArgs e)
    {
      var result = OAuthWeb.VerifyAuthorization();
      if (result.IsSuccessfully)
      {
        Session["AccessToken"] = result.AccessTokenValue;
        Response.Redirect("~/", true); // to home
      }
      else
      {
        Response.Write("Error: " + result.ErrorInfo.Message);
      }
    }

  }

}