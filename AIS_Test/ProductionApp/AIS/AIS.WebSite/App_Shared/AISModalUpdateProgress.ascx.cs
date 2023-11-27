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

namespace ZurichNA.AIS.WebSite.App_Shared
{
    public partial class AISModalUpdateProgress : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string script = string.Empty;

            script = "function ModalUpdProgInitialize(sender, args) {\n" +
                "    var upp = $find('" +UpdateProgress1.ClientID + "');\n" +
                "    upp._pageRequestManager.remove_endRequest(upp._endRequestHandlerDelegate);\n" +
                "    upp._endRequestHandlerDelegate = Function.createDelegate(upp, ModalUpdProgEndRequest);\n" +
                "    upp._startDelegate = Function.createDelegate(upp, ModalUpdProgStartRequest);\n" +
                "    upp._pageRequestManager.add_endRequest(upp._endRequestHandlerDelegate);\n" +
                "}\n" +
                "function ModalUpdProgStartRequest() {\n" +
                "    if (this._pageRequestManager.get_isInAsyncPostBack()) {\n" +
                "        $find('" + mdlPopUpExt.ClientID + "').show();\n" +
                "    }\n" +
                "    this._timerCookie = null;\n" +
                "}" +
                "function ModalUpdProgEndRequest(sender, arg) {\n" +
                "    $find('" + mdlPopUpExt.ClientID + "').hide();\n" +
                "    if (this._timerCookie) {\n" +
                "        window.clearTimeout(this._timerCookie);\n" +
                "        this._timerCookie = null;\n" +
                "    }\n" +
                "}\n" +
                " Sys.Application.add_load(ModalUpdProgInitialize);";

             Page.ClientScript.RegisterStartupScript(this.GetType(),"Modal",script,true);
        }
    }
}