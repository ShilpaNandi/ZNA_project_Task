using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
namespace ZurichNA.AIS.WebSite.App_Code
{
    public class AISAmountTextbox : TextBox
    {
        #region Properties
        public bool AllowNegetive { get; set; }
        public bool AllowDecimal { get; set; }
        #endregion
        protected override void OnPreRender(EventArgs e)
        {
            string str = ResolveUrl("~/JavaScript/AISAmount.js").ToString();
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("AISAmountTextbox"))
            {
                Page.ClientScript.RegisterClientScriptInclude("AISAmountTextbox", "../JavaScript/AISAmount.js");
            }

            if (this.Attributes["onkeyup"] != null && !this.Attributes["onkeyup"].ToString().EndsWith(";"))
                this.Attributes["onkeyup"] += ";";
            this.Attributes["onkeyup"] += "onkeyupAmount(event,this,'" + AllowDecimal.ToString() + "')";

            if (this.Attributes["onblur"] != null && !this.Attributes["onblur"].ToString().EndsWith(";"))
                this.Attributes["onblur"] += ";";
            this.Attributes["onblur"] += "onblurAmount(this,'" + AllowDecimal.ToString() + "');";

            if (this.Attributes["onkeypress"] != null && !this.Attributes["onkeypress"].ToString().EndsWith(";"))
                this.Attributes["onkeypress"] += ";";
            this.Attributes["onkeypress"] += "return onkeypressAmount(event,this,'" + AllowDecimal.ToString() + "','" + AllowNegetive.ToString() + "')";

            if (this.Attributes["onfocus"] != null && !this.Attributes["onfocus"].ToString().EndsWith(";"))
                this.Attributes["onfocus"] += ";";
            this.Attributes["onfocus"] += "onfocusAmount(this)";

            base.OnPreRender(e);

        }
    }
}
