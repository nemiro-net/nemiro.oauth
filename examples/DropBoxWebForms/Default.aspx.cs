using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

      string serverPath = "/" + System.IO.Path.GetFileName(FileUpload1.FileName);

      // you can upload to any folder:
      // serverPath = "/folder_name/" + System.IO.Path.GetFileName(FileUpload1.FileName);
      // folder_name - should exist in dropbox

      var fileInfo = UniValue.Empty;
      fileInfo["path"] = serverPath;
      fileInfo["mode"] = "add";
      fileInfo["autorename"] = true;
      fileInfo["mute"] = false;

      var result = OAuthUtility.Post
      (
        "https://content.dropboxapi.com/2/files/upload",
        new HttpParameterCollection
        { 
          { FileUpload1.PostedFile } 
        },
        headers: new NameValueCollection { { "Dropbox-API-Arg", fileInfo.ToString() } },
        contentType: "application/octet-stream",
        authorization: String.Format("Bearer {0}", token)
      );

      if (result.StatusCode != 200)
      {
        // error
        Response.Write(result["error"].ToString());
      }
      else
      {
        // get shared link
        result = OAuthUtility.Post
        (
          "https://api.dropboxapi.com/2/sharing/create_shared_link_with_settings",
          parameters: new HttpParameterCollection
          {
            new
            {
              path = serverPath,
              settings = new
              {
                requested_visibility = "public"
              }
            }
          },
          authorization: String.Format("Bearer {0}", token),
          contentType: "application/json"
        );

        if (result.StatusCode != 200)
        {
          Response.Write(result["error"].ToString());
        }
        else
        {
          hlResult.NavigateUrl = result["url"].ToString();
          hlResult.Text = hlResult.NavigateUrl;

          pnlSuccess.Visible = true;
          pnlUpload.Visible = false;
        }
      }
    }

  }

}