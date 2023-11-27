using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ZurichNA.AIS.WebSite.App_Code
{
    public class AISHMenu:Menu
    {

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            // output some script to override the default menu flyout behaviour; this helps to avoid
            // intermittent "Operation Aborted" errors
            Page.ClientScript.RegisterStartupScript(
                typeof(AISHMenu),
                "overrideMenu_HoverStatic",
                "if (typeof(overrideMenu_HoverStatic) == 'function' && typeof(Menu_HoverStatic) == 'function')\n" +
                "{\n" +
                    "Menu_HoverStatic = overrideMenu_HoverStatic;\n" +
                "}\n",
                true);

        }
    }
}
