using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;

namespace DropBoxWebForms
{

  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      OAuthManager.RegisterClient
      (
        new DropboxClient
        (
          "0le6wsyp3y085wy",
          "48afwq9yth83y7u"
        )
      );
    }

  }

}