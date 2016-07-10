using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nemiro.OAuth;

namespace Test.CSharp.AspWebForms
{
  public partial class ExternalLoginResult : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      preResult.InnerHtml = "";
      var result = OAuthWeb.VerifyAuthorization();
      preResult.InnerHtml += String.Format("Provider: {0}<br />", result.ProviderName);
      if (result.IsSuccessfully)
      {
        var user = result.UserInfo;
        preResult.InnerHtml += String.Format("User ID:  {0}<br />", user.UserId);
        preResult.InnerHtml += String.Format("Name:     {0}<br />", user.DisplayName);
        preResult.InnerHtml += String.Format("Email:    {0}", user.Email);
      }
      else
      {
        preResult.InnerHtml += result.ErrorInfo.Message;
      }
    }
  }
}