using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nemiro.OAuth.Clients;
using Nemiro.OAuth;

namespace AspWebFormsPopup
{
  public partial class RedirectToAuth : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(Request.QueryString["provider"]))
      {
        throw new ArgumentNullException("provider");
      }

      // build callback url
      string returnUrl = new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri;
      // pop-up indicator for JavaScript code
      if (returnUrl.IndexOf("?") != -1)
      {
        returnUrl += "&";
      }
      else
      {
        returnUrl += "?";
      }
      returnUrl += "popup=true";

      // redirect to authorization page of the specified provider
      OAuthWeb.RedirectToAuthorization(Request.QueryString["provider"], returnUrl);
    }
  }
}