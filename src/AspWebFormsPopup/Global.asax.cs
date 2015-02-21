using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Nemiro.OAuth;
using System.Collections.Specialized;

namespace AspWebFormsPopup
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      OAuthManager.RegisterClient
      (
        "facebook",
        "1435890426686808",
        "c6057dfae399beee9e8dc46a4182e8fd",
        parameters: new NameValueCollection { { "display", "popup" } }
      );

      OAuthManager.RegisterClient
      (
        "foursquare",
        "LHYZN1KUXN50L141QCQFNNVOYBGUE3G3FCWFZ3EEZTOZHY5Q",
        "HWXYFLLSS2IUQ0H4XNCDAZEFZKIU3MZRP5G55TNBDHRPNOQT",
        parameters: new NameValueCollection { { "display", "touch" } }
      );
    }

  }
}