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
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using System.Xml.Linq;
using System.Collections.Generic;
using ZurichNA.AIS.ExceptionHandling;
public partial class AcctMgmt_AccountAssignment : AISBasePage
{

    private ProgramPeriodsBS ProgramperiodsService
    {
        get
        {
            return (ProgramPeriodsBS)Session["ProgramPeriodService"];
        }
        set { Session["ProgramPeriodService"] = value; }

    }

    private ProgramPeriodBE ProgramperiodsBE
    {
        get { return (ProgramPeriodBE)Session["ProgamInfo-ProgramBE"]; }
        set { Session["ProgamInfo-ProgramBE"] = value; }
    }

    private CustomerContactBE CustomercontactsBE
    {
        get { return (CustomerContactBE)Session["CustomerInfo-CustomerBE"]; }
        set { Session["CustomerInfo-CustomerBE"] = value; }

    }
    private CustomerContactBS CustomercontactsBS
    {
        get { return (CustomerContactBS)Session["CustomerInfo-CustomerBS"]; }
        set { Session["CustomerInfo-CustomerBS"] = value; }

    }

    private IList<CustomerContactBE> lstAcctresponsibilities
    {
        get { return (IList<CustomerContactBE>)Session["AA-lstAcctresponsibilities"]; }
        set { Session["AA-lstAcctresponsibilities"] = value; }
 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "AccountAssignment";


        if (!IsPostBack)
        {

            ProgramperiodsBE = new ProgramPeriodBE();
            ProgramperiodsService = new ProgramPeriodsBS();
            CustomercontactsBE = new CustomerContactBE();
            CustomercontactsBS = new CustomerContactBS();
            BindDropdownlist();
            //BindListViews();

        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(btnSearch);
        list.Add(btnClear);
        list.Add(btnSave);
        ProcessExitFlag(list);
    }


    protected void chkRangeSearch_CheckedChanged(object sender, EventArgs e)
    {
        tdAccountsList.Visible = false;
        tdResponsibilities.Visible = false;
        if (!chkRangeSearch.Checked)
        {
            tblRange.Visible = false;

            //this.tdAcctlist.Disabled = false;
            Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            pnlAccountlist.Enabled = true;

        }
        else
        {
            //DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
            Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            pnlAccountlist.Enabled = false;


            tblRange.Visible = true;

        }
    }
/// <summary>
/// This method is used to populate the dropdown list with the numbers and alphabets.
/// </summary>
    public void BindDropdownlist()
    {
        int icounter;
        ListItem lstitem;
        for (icounter = 48; icounter <= 57; icounter++)
        {

            lstitem = new ListItem();
            lstitem.Text = Convert.ToChar(icounter).ToString();

            lstitem.Value = Convert.ToChar(icounter).ToString();

            ddlSearchstart.Items.Add(lstitem);
            ddlSearchend.Items.Add(lstitem);

        
        }
        for (icounter = 65; icounter <= 90; icounter++)
        {

            lstitem = new ListItem();
            lstitem.Text = Convert.ToChar(icounter).ToString();

            lstitem.Value = Convert.ToChar(icounter).ToString();

            ddlSearchstart.Items.Add(lstitem);
            ddlSearchend.Items.Add(lstitem);


        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        
        int intBrokerid = Convert.ToInt32(ddlBroker.SelectedValue);
        int intBUOfficeid = Convert.ToInt32(this.ddlOffice.SelectedValue);
        char stVal = ((ddlSearchstart.SelectedIndex >= 0) ? this.ddlSearchstart.SelectedItem.Text[0] : ' ');
        char edVal = ((ddlSearchend.SelectedIndex >= 0) ? this.ddlSearchend.SelectedItem.Text[0] : ' ');
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");

        int intCustomerid = 0;
        if (ddlAccnts.SelectedIndex == -1 && ddlOffice.SelectedIndex == 0 && ddlBroker.SelectedIndex == 0)
        {
            string strMessage = "Please select at least one of the dropdown prior to clicking Search button";
            ShowError(strMessage);
            return;
        }

        for (int icount = 0; icount <= lstAssignResponsibilities.Items.Count - 1; icount++)
        {
            DropDownList ddlPerson = (DropDownList)lstAssignResponsibilities.Items[icount].FindControl("ddlName");
           // ListItem lstitem = new ListItem();
            ddlPerson.SelectedIndex = -1;
            ddlPerson.DataSourceID = "PersonDatasource";
            ddlPerson.DataTextField="FULLNAME";
            ddlPerson.DataValueField ="PERSON_ID";
            ddlPerson.DataBind();
        }
        IList<ProgramPeriodBE> Customerlist;

        if (this.chkRangeSearch.Checked == true)
            //This method is used to get details based on the range.
            Customerlist = ProgramperiodsService.getrangeprogramperiods(intBUOfficeid, intBrokerid, stVal, edVal);
        else
        {if(ddlAccnts.SelectedValue!="")
            intCustomerid = Convert.ToInt32(ddlAccnts.SelectedValue);
            // This method is used to get the customerslist.
            Customerlist = ProgramperiodsService.GetCustomerList(intCustomerid, intBUOfficeid, intBrokerid);
        }
        this.lstAccountList.DataSource = Customerlist;

        this.lstAccountList.DataBind();
        this.tdAccountsList.Visible = true;
        if (Customerlist.Count > 0)
            this.tdResponsibilities.Visible = true;
        else
            this.tdResponsibilities.Visible = false;

        this.lstViewAccountLevel.Visible = false;
        this.lblAccountLevel.Visible = false;
        if(intCustomerid>0)
            lstAcctresponsibilities = CustomercontactsBS.getAccountResponsibilities(intCustomerid);
    }

    protected void lstAccountList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void lstAccountList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        // this.tdResponsibilities.Visible = true;
        Label lblcustmr_name = new Label();
        string str = String.Empty;
        int intCustomerid = 0;
        if (e.CommandName == "Details")
        {
            intCustomerid = Convert.ToInt32(e.CommandArgument);
            Label lblname = (Label)e.Item.FindControl("lblCustmrname");
            str = "Responsibilities for " + lblname.Text;
        }
        // This method is used to assign responsibilities
        IList<CustomerContactBE> lstAccountresponsibilities = CustomercontactsBS.getAccountResponsibilities(intCustomerid);
        // if (lstAccountresponsibilities.Count == 0)
        //    this.lblAccountLevel.Visible = false;
        //tdResponsibilities.Visible = false;
        //this.lblResponsibilities.Visible = false;
       
        this.lstViewAccountLevel.Visible = true;
        this.lblAccountLevel.Text = str;
        this.lblAccountLevel.Visible = true;
        this.lstViewAccountLevel.DataSource = lstAccountresponsibilities;
        this.lstViewAccountLevel.DataBind();
        BindResponsibilities(intCustomerid);
        this.lstViewAccountLevel.Visible = true;
        this.lblAccountLevel.Visible = true;

    }

    private void BindResponsibilities(int intCustomerid)
    {
        HiddenField hfPersonid;
        DropDownList ddlPersons;
       
        lstAcctresponsibilities = CustomercontactsBS.getAccountResponsibilities(intCustomerid);
        if (lstAcctresponsibilities.Count > 0)
        {
            for (int icount = 0; icount <= lstAssignResponsibilities.Items.Count - 1; icount++)
            {
                DropDownList ddlPerson = (DropDownList)lstAssignResponsibilities.Items[icount].FindControl("ddlName");
                // ListItem lstitem = new ListItem();
                ddlPerson.SelectedIndex = -1;
                ddlPerson.DataSourceID = "PersonDatasource";
                ddlPerson.DataTextField = "FULLNAME";
                ddlPerson.DataValueField = "PERSON_ID";
                ddlPerson.DataBind();
            }

            for (int acctrespcount = 0; acctrespcount < lstAcctresponsibilities.Count; acctrespcount++)
            {
                for (int respcount = 0; respcount < lstAssignResponsibilities.Items.Count; respcount++)
                {
                    hfPersonid = (HiddenField)lstAssignResponsibilities.Items[respcount].FindControl("hfResponseid");
                    if (lstAcctresponsibilities[acctrespcount].ROLE_ID == Convert.ToInt32(hfPersonid.Value))
                    {

                        string strname = lstAcctresponsibilities[acctrespcount].PER.SURNAME + "," + lstAcctresponsibilities[acctrespcount].PER.FORENAME;
                        ListItem ddlPerson = new ListItem(strname, lstAcctresponsibilities[acctrespcount].PERSON_ID.ToString());


                        ddlPersons = (DropDownList)lstAssignResponsibilities.Items[respcount].FindControl("ddlName");
                        ddlPersons.Items.Clear();
                        ddlPersons.DataSourceID = "PersonDatasource";
                        ddlPersons.DataTextField = "FULLNAME";
                        ddlPersons.DataValueField = "PERSON_ID";
                       
                        ddlPersons.DataBind();
                        ddlPersons.SelectedIndex = -1;
                        if (ddlPersons.Items.Contains(ddlPerson))

                            ddlPersons.Items.FindByValue(lstAcctresponsibilities[acctrespcount].PERSON_ID.ToString()).Selected = true;
                    }
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        HiddenField hfCustmrid, hfPersonid;
        CheckBox chkSelected;
        DropDownList ddlPersons;
        int intCustomerid,intpersonid;
        //int intpersonid;
        //lstAssignResponsibilities
        for (int custcount = 0; custcount < lstAccountList.Items.Count; custcount++)
        {
            hfCustmrid = (HiddenField)lstAccountList.Items[custcount].FindControl("hidCustmrid");
            chkSelected = (CheckBox)lstAccountList.Items[custcount].FindControl("chkSelect");
            intCustomerid = Convert.ToInt32(hfCustmrid.Value);
            if (chkSelected.Checked == true)
            {
                ArrayList arrlstResponsibilities = new ArrayList();
                ArrayList arrlstPersons = new ArrayList();
                for (int respcount = 0; respcount < lstAssignResponsibilities.Items.Count; respcount++)
                {
                    hfPersonid = (HiddenField)lstAssignResponsibilities.Items[respcount].FindControl("hfResponseid");
                    ddlPersons = (DropDownList)lstAssignResponsibilities.Items[respcount].FindControl("ddlName");
                    //CustomercontactsBE=
                    //intpersonid = Convert.ToInt32(ddlPersons.SelectedValue);
                    //if (intpersonid > 0)
                    //{
                        arrlstResponsibilities.Add(Convert.ToInt32(hfPersonid.Value));
                        arrlstPersons.Add(Convert.ToInt32(ddlPersons.SelectedValue));
                    //}
                    //else 
                    //{
                    
                    //}
                }
                // This method is used to assign responsibilities.
                
                string errorMessage = String.Empty;
                bool handleConcurrency = (lstAccountList.Items.Count == 1 && chkSelected.Checked);
                bool success = CustomercontactsBS.AssignResponsibilities(intCustomerid,
                    arrlstResponsibilities, arrlstPersons, out errorMessage, lstAcctresponsibilities, handleConcurrency, CurrentAISUser.PersonID);
               //Shows Concurrent Conflict Error
               ShowConcurrentConflict(success, errorMessage);
               lstAcctresponsibilities = CustomercontactsBS.getAccountResponsibilities(intCustomerid);
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        ddlAccnts.SelectedIndex = -1;
        this.ddlOffice.SelectedIndex = -1;
        this.ddlBroker.SelectedIndex = -1;
        this.tdResponsibilities.Visible = false;
        lstViewAccountLevel.Visible = false;
        lblAccountLevel.Visible = false;
        tdAccountsList.Visible = false;

    }

}
