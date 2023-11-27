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

public partial class App_Shared_LSIUserDropdown : System.Web.UI.UserControl
{
    private IList<LSIAllCustomersBE> dtDDLSource;

    public int SelectedIndex
    {
        get
        {
            return ddlLSIAccountlist.SelectedIndex;
        }
        set
        {
            ddlLSIAccountlist.SelectedIndex = value;
        }
    }


    public string SelectedValue
    {
        get
        {
            return ddlLSIAccountlist.SelectedValue;
        }
        set
        {
            ddlLSIAccountlist.SelectedValue = value;
        }
    }

    public string SelectedItem
    {
        get
        {
            return Convert.ToString(ddlLSIAccountlist.SelectedItem);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ////this.dtDDLSource = (new LSICustomersBS()).getLSIAccounts();
        ////if (!IsPostBack)
        ////{

        ////    this.ddlLSIAccountlist.DataSource = dtDDLSource;
        ////    this.ddlLSIAccountlist.DataTextField = "FULL_NM";
        ////    this.ddlLSIAccountlist.DataValueField = "CUSTMR_ID";
        ////    this.ddlLSIAccountlist.DataBind();
        ////}
    }

    protected void LButton_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        this.dtDDLSource = (new LSICustomersBS()).getLSIAccounts();
        string fst = null, snd = null, trd = null;
        if (dtDDLSource != null)
        {
            switch (lb.Text.Trim())
            {
                case "A": fst = "A";
                    snd = "B";
                    trd = "C";
                    break;
                case "D": fst = "D";
                    snd = "E";
                    trd = "F";
                    break;
                case "G": fst = "G";
                    snd = "H";
                    trd = "I";
                    break;
                case "J": fst = "J";
                    snd = "K";
                    trd = "L";
                    break;
                case "M": fst = "M";
                    snd = "N";
                    trd = "O";
                    break;
                case "P": fst = "P";
                    snd = "Q";
                    trd = "R";
                    break;
                case "S": fst = "S";
                    snd = "T";
                    trd = "U";
                    break;
                case "V": fst = "V";
                    snd = "W";
                    trd = "X";
                    break;
                case "Y": fst = "Y";
                    snd = "Z";
                    trd = "Z";
                    break;

            }

            var result = (from acct in dtDDLSource
                          where acct.FULL_NAME.StartsWith(fst, StringComparison.InvariantCultureIgnoreCase) ||
                                acct.FULL_NAME.StartsWith(snd, StringComparison.InvariantCultureIgnoreCase) ||
                                acct.FULL_NAME.StartsWith(trd, StringComparison.InvariantCultureIgnoreCase)
                          orderby acct.FULL_NAME
                          select new LSIAllCustomersBE
                          {
                              FULL_NAME = acct.FULL_NAME,
                              LSI_ACCT_ID = acct.LSI_ACCT_ID,
                              NAME_ACCOUNTNO = acct.NAME_ACCOUNTNO
                          }).ToList();

            this.ddlLSIAccountlist.DataSource = result;
            this.ddlLSIAccountlist.DataTextField = "NAME_ACCOUNTNO";
            this.ddlLSIAccountlist.DataValueField = "LSI_ACCT_ID";
            this.ddlLSIAccountlist.DataBind();
        }
    }
}
