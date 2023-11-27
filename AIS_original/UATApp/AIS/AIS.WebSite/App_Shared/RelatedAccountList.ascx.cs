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

public partial class App_Shared_RelAccountUserDropdown : System.Web.UI.UserControl
{
    private IList<AccountBE> dtDDLSource;

    public int SelectedIndex
    {
        get
        {
            return ddlAccountlist.SelectedIndex;
        }
        set
        {
            ddlAccountlist.SelectedIndex = value;
        }
    }


    public string SelectedValue
    {
        get
        {
            return ddlAccountlist.SelectedValue;
        }
        set
        {
            ddlAccountlist.SelectedValue = value;
        }
    }

    public string SelectedItem
    {
        get
        {
            return Convert.ToString(ddlAccountlist.SelectedItem);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //////this.dtDDLSource = (new AccountBS()).getNonMasterAccounts();
        //////if (!IsPostBack)
        //////{

        //////    this.ddlAccountlist.DataSource = dtDDLSource;
        //////    this.ddlAccountlist.DataTextField = "FULL_NM";
        //////    this.ddlAccountlist.DataValueField = "CUSTMR_ID";
        //////    this.ddlAccountlist.DataBind();
        //////}
    }

    protected void LButton_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        this.dtDDLSource = (new AccountBS()).getNonMasterAccounts();
        IList<AccountBE> result = new List<AccountBE>();

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

            IEnumerable<AccountBE> query = (from acct in dtDDLSource
                                           where acct.FULL_NM.StartsWith(fst, StringComparison.InvariantCultureIgnoreCase) ||
                                                 acct.FULL_NM.StartsWith(snd, StringComparison.InvariantCultureIgnoreCase) ||
                                                 acct.FULL_NM.StartsWith(trd, StringComparison.InvariantCultureIgnoreCase
                                                 )
                                           orderby acct.FULL_NM
                                           select new AccountBE
                                           {
                                               FULL_NM = acct.FULL_NM,
                                               FINC_PTY_ID = acct.FINC_PTY_ID,
                                               CUSTMR_ID = acct.CUSTMR_ID,
                                               SUPRT_SERV_CUSTMR_GP_ID = acct.SUPRT_SERV_CUSTMR_GP_ID,
                                               MSTR_ACCT_IND = acct.MSTR_ACCT_IND,
                                               ACTV_IND = acct.ACTV_IND
                                           });

            query = query.Where(acct => acct.MSTR_ACCT_IND != true && acct.ACTV_IND == true);

            result = query.ToList();
            this.ddlAccountlist.DataSource = result;
            this.ddlAccountlist.DataTextField = "FULL_NM";
            this.ddlAccountlist.DataValueField = "CUSTMR_ID";
            this.ddlAccountlist.DataBind();
        }
    }
}
