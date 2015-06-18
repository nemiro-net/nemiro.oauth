using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;

namespace GoogleDriveWebForms
{

  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      OAuthManager.RegisterClient
      (
        "google",
        "546998793431-lngma9lbhbv89hnh7uemtj6htbdjoei3.apps.googleusercontent.com",
        "YCPcNvhtQPIWVgw2JAY0S6n2",
        "https://www.googleapis.com/auth/drive" 
        // for details please visit https://developers.google.com/drive/web/scopes
      );
    }

  }

}