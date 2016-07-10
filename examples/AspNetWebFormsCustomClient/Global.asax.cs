using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Nemiro.OAuth;

namespace AspNetWebFormsCustomClient
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      // IMPORTANT: You can not register clients by provider name. 
      // Registration should be carried out through the creation of instances of the class.
      
      // custom facebook client
      OAuthManager.RegisterClient
      (
        new MyFacebookClient
        (
          "1435890426686808", 
          "c6057dfae399beee9e8dc46a4182e8fd"
        )
      );

      // standard twitter client
      OAuthManager.RegisterClient
      (
        new Nemiro.OAuth.Clients.TwitterClient
        (
          "1Ayh2ZM2l9chloiFsmxNpi7Gg",
          "dbVXKWVIlH5fRuVI0FvE2ZDsZAbBg74UrGFYwW1kLSwc0ceJnJ"
        )
      );

      // custom twitter client
      OAuthManager.RegisterClient
      (
        // the client has the same name as the original
        "Custom", // <---- you must add the difference
        new MyTwitterClient
        (
          "1Ayh2ZM2l9chloiFsmxNpi7Gg",
          "dbVXKWVIlH5fRuVI0FvE2ZDsZAbBg74UrGFYwW1kLSwc0ceJnJ"
        )
      );
    }

  }
}