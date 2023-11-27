using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZurichNA.AIS.WebSite.App_Shared
{
    public partial class AlertSessionTimeOut : System.Web.UI.UserControl
    {
        public string WindowName
        {
            get
            {
                if (HttpContext.Current.Request["wID"] != null && HttpContext.Current.Request["wID"] != "")
                {
                    return HttpContext.Current.Request["wID"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
    }
}