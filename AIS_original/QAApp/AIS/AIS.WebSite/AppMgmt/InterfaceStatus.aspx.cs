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
using System.Collections.Generic;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

public partial class AppMgmt_InterfaceStatus : AISBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Code executed when Search button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindListView();
    }
    /// <summary>
    /// 
    /// </summary>
    public void BindListView()
    {
        int AcctNumber = 0;

        string selectedType = string.Empty;

        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddlAccountlist.SelectedValue != "0" && ddlAccountlist.SelectedValue != "")
        {
            AcctNumber = Convert.ToInt32(ddlAccountlist.SelectedValue);
        }

        ApplicationStatusLogBS list = new ApplicationStatusLogBS();
        if (ddlInterfaceType.SelectedValue == "0")
        { selectedType = ""; }
        else
        { selectedType = ddlInterfaceType.SelectedItem.Text; }

        string FromDate = txtFromDate.Text;
        string ToDate = txtToDate.Text;
        IList<ApplicationStatusLogBE> list2 = list.getLogData(AcctNumber, selectedType, FromDate, ToDate);



        lstInternalMasters.DataSource = list2;
        lstInternalMasters.DataBind();
    }
}
