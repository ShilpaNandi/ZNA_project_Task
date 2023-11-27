using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Principal;
namespace ZurichNA.AIS.WebSite
{
    public partial class SecurityTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AISUser user = (AISUser)System.Web.HttpContext.Current.Items["CurrentUser"];
            Response.Write("Role: " + user.Role + "<br/>");
            IPrincipal lPrincipal;
            string userdomain = user.Identity.Name.Split(new char[] { '\\' })[0];
            string actualdomain = string.Empty;
            if (userdomain == ConfigurationManager.AppSettings["GITDIRDomain"].ToString())
            {
                actualdomain = ConfigurationManager.AppSettings["Inquiry_GITDIR"].ToString().Split(new char[] { '\\' })[0];
            }
            else
            {
                actualdomain = ConfigurationManager.AppSettings["Inquiry"].ToString().Split(new char[] { '\\' })[0];
            }
            string actualdomainTest = ConfigurationManager.AppSettings["Inquiry_Test"].ToString().Split(new char[] { '\\' })[0];
            //return (this.Role.Length > 0 && (userdomain == actualdomain || userdomain == actualdomainTest));
            Response.Write("UserDomain: " + userdomain + "<br/>");
            Response.Write("Actual Domain:" + actualdomain + "<br/>");
            Response.Write("Actual Domain_Test:" + actualdomainTest + "<br/>");
            //06/23 for veracode
            //Response.Write("Current WebPage:"+Request.Url.AbsolutePath.Substring(1, Request.Url.AbsolutePath.Length - 1)+"<br/>");
            Response.Write("Current WebPage:" + (Server.HtmlEncode(Convert.ToString(Request.Url.AbsolutePath.Substring(1, Request.Url.AbsolutePath.Length - 1)) + "<br/>")));
            bool flag = user.IsAuthorized("AcctSearch.aspx");
            Response.Write("IsAuthorized: " + flag.ToString());
        }
    }
}   
