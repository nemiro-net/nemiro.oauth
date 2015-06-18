using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nemiro.OAuth;

namespace GoogleDriveWebForms
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
      OAuthWeb.RedirectToAuthorization("google", returnUrl);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
      if (Session["AccessToken"] == null)
      {
        Response.Write("Error. Access token not found.<br /><a href=\"/\">Try again</a>.");
        pnlUpload.Visible = false;
        return;
      }

      // help: https://developers.google.com/drive/v2/reference/files/insert
      var token = Session["AccessToken"].ToString();

      try
      {
        var result = OAuthUtility.Post
        (
          "https://www.googleapis.com/upload/drive/v2/files",
          new HttpParameterCollection
          { 
            { "uploadType", "media" },
            { FileUpload1.PostedFile } 
          },
          new HttpAuthorization(AuthorizationType.Bearer, token)
        );

        // ok
        hlResult.NavigateUrl = result["webContentLink"].ToString();
        hlResult.Text = hlResult.NavigateUrl;
          
        pnlSuccess.Visible = true;
        pnlUpload.Visible = false;
      }
      catch (Exception ex)
      {
        Response.Write(ex.Message);
      }
    }

  }

}