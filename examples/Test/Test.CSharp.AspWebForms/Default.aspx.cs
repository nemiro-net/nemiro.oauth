using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nemiro.OAuth;

namespace Test.CSharp.AspWebForms
{
  public partial class Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // output ouath providers list
      RepeaterOAuth.DataSource = Nemiro.OAuth.OAuthManager.RegisteredClients;
      RepeaterOAuth.DataBind();
    }

    protected void RepeaterOAuth_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      if (!(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
      {
        return;
      }

      // add provider name and click handler
      LinkButton lnk = e.Item.FindControl("lnk") as LinkButton;
      lnk.Attributes.Add("data-provider", ((KeyValuePair<ClientName, OAuthBase>)e.Item.DataItem).Key);
      lnk.Click += RedirectToLogin_Click;

      // add icon
      Image img = e.Item.FindControl("img") as Image;
      img.ImageUrl = String.Format("~/Icon.ashx?id={0}", lnk.Attributes["data-provider"]);
    }

    protected void RedirectToLogin_Click(object sender, EventArgs e)
    {
      // get provider name
      string provider = ((LinkButton)sender).Attributes["data-provider"];

      // build callback url
      string returnUrl =  new Uri(Request.Url, "ExternalLoginResult.aspx").AbsoluteUri;
      // not suppored localhost (it is only for localhost)
      string[] notSupportedLocalhost = { "live", "mail.ru" };
      if (notSupportedLocalhost.Any(itm => itm.Equals(provider, StringComparison.OrdinalIgnoreCase)))
      {
        returnUrl = String.Format("http://oauth.nemiro.net/oauth_redirect.html?returnUrl={0}", Server.UrlEncode(returnUrl));
      }
      // --

      // redirect to authorization page of the specified provider
      OAuthWeb.RedirectToAuthorization(provider, returnUrl);
    }

  }
}