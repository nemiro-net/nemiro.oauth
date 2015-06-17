using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nemiro.OAuth;

namespace DropBoxWebForms
{

  public partial class Default : System.Web.UI.Page
  {

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        if (Session["AccessToken"] == null)
        {
          pnlLogin.Visible = true;
          pnlUpload.Visible = false;
        }
        else
        {
          pnlLogin.Visible = false;
          pnlUpload.Visible = true;
        }
      }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
      var btn = (Button)sender;

      // build callback url
      string returnUrl = new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri;

      // redirect to authorization page
      OAuthWeb.RedirectToAuthorization("dropbox", returnUrl);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
      if (Session["AccessToken"] == null)
      {
        Response.Write("Error. Access token not found.<br /><a href=\"/\">Try again</a>.");
        pnlUpload.Visible = false;
        return;
      }

      var token = Session["AccessToken"].ToString();

      string serverPath = System.IO.Path.GetFileName(FileUpload1.FileName);

      // you can upload to any folder:
      // serverPath = "/folder_name/" + System.IO.Path.GetFileName(FileUpload1.FileName);
      // folder_name - should exist in dropbox

      var result = OAuthUtility.Put
      (
        "https://api-content.dropbox.com/1/files_put/auto/",
        new HttpParameterCollection
        { 
          { "access_token", token },
          { "overwrite", "true" },
          { "path", serverPath },
          { FileUpload1.PostedFile } 
        }
      );

      if (result.StatusCode != 200)
      {
        // error
        Response.Write(result["error"].ToString());
      }
      else
      {
        // ok
        hlResult.NavigateUrl = String.Format("https://api-content.dropbox.com/1/files/auto{0}?access_token={1}", result["path"], token);
        hlResult.Text = hlResult.NavigateUrl;

        pnlSuccess.Visible = true;
        pnlUpload.Visible = false;
      }
    }

  }

}