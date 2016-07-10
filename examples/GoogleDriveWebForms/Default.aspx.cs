using System;
using System.Collections.Generic;
using System.IO;
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
      var filePath = Server.MapPath(String.Format("~/Temp/{0}.tmp", Guid.NewGuid()));

      try
      {
        /*
        // simple upload
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
        */

        // save file
        if (!Directory.Exists(Server.MapPath("~/Temp")))
        {
          Directory.CreateDirectory(Server.MapPath("~/Temp"));
        }

        using (var file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Inheritable))
        using (var writer = new BinaryWriter(file))
        {
          using (var reader = new BinaryReader(FileUpload1.PostedFile.InputStream))
          {
            byte[] buffer = new byte[4096];
            int readBytes = 0;

            while ((readBytes = reader.Read(buffer, 0, buffer.Length)) != 0)
            {
              writer.Write(buffer, 0, readBytes);
            }
          }
        }

        var fileToSend = File.OpenRead(filePath);

        // send saved file
        var parameters = new HttpParameterCollection();
        parameters.Add("uploadType", "multipart");
        parameters.AddContent("application/json", new { title = FileUpload1.FileName });
        parameters.AddContent
        (
          FileUpload1.PostedFile.ContentType ?? "application/octet-stream",
          fileToSend
        );

        var result = OAuthUtility.Post
        (
          "https://www.googleapis.com/upload/drive/v2/files",
          parameters,
          authorization: new HttpAuthorization(AuthorizationType.Bearer, token),
          contentType: "multipart/related"
        );

        fileToSend.Close();

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
      finally
      {
        // remove temp file
        if (File.Exists(filePath))
        {
          File.Delete(filePath);
        }
      }
    }

  }

}