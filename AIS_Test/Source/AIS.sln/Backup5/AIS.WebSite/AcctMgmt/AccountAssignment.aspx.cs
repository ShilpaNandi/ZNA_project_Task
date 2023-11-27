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
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;

public partial class AcctMgmt_AccountAssignment : AISBasePage
{

    private ProgramPeriodsBS ProgramperiodsService
    {
        get
        {
            //return (ProgramPeriodsBS)Session["ProgramPeriodService"];
            return (ProgramPeriodsBS)RetrieveObjectFromSession("ProgramPeriodService");
        }
        set 
        { 
            //Session["ProgramPeriodService"] = value;
            SaveObjectToSession("ProgramPeriodService", value);
        }

    }

    private ProgramPeriodBE ProgramperiodsBE
    {
        get 
        { 
            //return (ProgramPeriodBE)Session["ProgamInfo-ProgramBE"]; 
            return (ProgramPeriodBE)RetrieveObjectFromSessionUsingWindowName("ProgamInfo-ProgramBE");
        }
        set 
        { 
            //Session["ProgamInfo-ProgramBE"] = value; 
            SaveObjectToSessionUsingWindowName("ProgamInfo-ProgramBE", value);
        }
    }

    private CustomerContactBE CustomercontactsBE
    {
        get 
        { 
            //return (CustomerContactBE)Session["CustomerInfo-CustomerBE"]; 
            return (CustomerContactBE)RetrieveObjectFromSessionUsingWindowName("CustomerInfo-CustomerBE");
        }
        set 
        { 
            //Session["CustomerInfo-CustomerBE"] = value; 
            SaveObjectToSessionUsingWindowName("CustomerInfo-CustomerBE", value);
        }

    }
    private CustomerContactBS CustomercontactsBS
    {
        get 
        { 
            //return (CustomerContactBS)Session["CustomerInfo-CustomerBS"]; 
            return (CustomerContactBS)RetrieveObjectFromSession("CustomerInfo-CustomerBS");
        }
        set 
        { 
            //Session["CustomerInfo-CustomerBS"] = value; 
            SaveObjectToSession("CustomerInfo-CustomerBS", value);
        }

    }

    private IList<CustomerContactBE> lstAcctresponsibilities
    {
        get 
        { 
            //return (IList<CustomerContactBE>)Session["AA-lstAcctresponsibilities"]; 
            return (IList<CustomerContactBE>)RetrieveObjectFromSessionUsingWindowName("AA-lstAcctresponsibilities");
        }
        set 
        { 
            //Session["AA-lstAcctresponsibilities"] = value; 
            SaveObjectToSessionUsingWindowName("AA-lstAcctresponsibilities", value);
        }
 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "AccountAssignment";


        if (!IsPostBack)
        {
            //btnSave.Attributes.Add("onclick", "javascript:return confirm('Are You Sure To Save These Details')");
            ProgramperiodsBE = new ProgramPeriodBE();
            ProgramperiodsService = new ProgramPeriodsBS();
            CustomercontactsBE = new CustomerContactBE();
            CustomercontactsBS = new CustomerContactBS();
            BindDropdownlist();
            //BindListViews();
            BindRole();
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
            tblAccountRange.Visible = false;

            //this.tdAcctlist.Disabled = false;
            Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            pnlAccountlist.Enabled = true;

            chkBURangeSearch.Enabled = true;
            ChkBrokerRangeSearch.Enabled = true;

            ddlOffice.Enabled = true;
            ddlBroker.Enabled = true;
        }
        else if (chkRangeSearch.Checked)
        {
            Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            pnlAccountlist.Enabled = false;
            ddlOffice.Enabled = true;
            ddlBroker.Enabled = true;

            tblAccountRange.Visible = true;

            chkBURangeSearch.Enabled = true;
            ChkBrokerRangeSearch.Enabled = true;

            ddlSearchend.SelectedIndex = 0;
            ddlSearchstart.SelectedIndex = 0;
            ddlAcctlist.SelectedIndex = 0;
        }
    }

    protected void chkBURangeSearch_CheckedChanged(object sender, EventArgs e)
    {
        tdAccountsList.Visible = false;
        tdResponsibilities.Visible = false;
       if (!chkBURangeSearch.Checked)
        {
            tblBURange.Visible = false;            
            //Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            //pnlAccountlist.Enabled = true;
            ddlOffice.Enabled = true;
            ddlOfficeOnly.Enabled = true;
            ddlBU.Enabled = true;
            ddlBroker.Enabled = true;
            chkRangeSearch.Enabled = true;
            ChkBrokerRangeSearch.Enabled = true;
        }
        else if (chkBURangeSearch.Checked)
        {            
            //Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            //pnlAccountlist.Enabled = false;
            tblBURange.Visible = true;
            chkRangeSearch.Enabled = true;
            ChkBrokerRangeSearch.Enabled = true;
            ddlOffice.Enabled = false;
            ddlBroker.Enabled = true;
            ddlOfficeOnly.Enabled = false;
            ddlBU.Enabled = true;
            ddlBUstart.SelectedIndex = 0;
            ddlBUend.SelectedIndex = 0;
            ddlOffice.SelectedIndex = 0;
            ddlOfficeOnly.SelectedIndex = 0;
            ddlBU.SelectedIndex = 0;
        } 
    }

    protected void ChkBrokerRangeSearch_CheckedChanged(object sender, EventArgs e)
    {
        tdAccountsList.Visible = false;
        tdResponsibilities.Visible = false;
       if (!ChkBrokerRangeSearch.Checked)
        {

            tblBrokerRange.Visible = false;            
            //Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            //pnlAccountlist.Enabled = true;

            //ddlOffice.Enabled = true;
            ddlBroker.Enabled = true;

            chkRangeSearch.Enabled = true;
            chkBURangeSearch.Enabled = true;
        }
        else if (ChkBrokerRangeSearch.Checked)
        {
            //lblRangeSearch.Text = "Broker Range Search";            
            //Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
            //pnlAccountlist.Enabled = false;
            tblBrokerRange.Visible = true;
            chkRangeSearch.Enabled = true;
            chkBURangeSearch.Enabled = true;
            //ddlOffice.Enabled = false;
            ddlBroker.Enabled = false;
            ddlBrokerend.SelectedIndex = 0;
            ddlBrokerstart.SelectedIndex = 0;
            ddlBroker.SelectedIndex = 0;
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

            ddlBUstart.Items.Add(lstitem);
            ddlBUend.Items.Add(lstitem);

            ddlBrokerstart.Items.Add(lstitem);
            ddlBrokerend.Items.Add(lstitem);
        }
        for (icounter = 65; icounter <= 90; icounter++)
        {

            lstitem = new ListItem();
            lstitem.Text = Convert.ToChar(icounter).ToString();

            lstitem.Value = Convert.ToChar(icounter).ToString();

            ddlSearchstart.Items.Add(lstitem);
            ddlSearchend.Items.Add(lstitem);

            ddlBUstart.Items.Add(lstitem);
            ddlBUend.Items.Add(lstitem);

            ddlBrokerstart.Items.Add(lstitem);
            ddlBrokerend.Items.Add(lstitem);
        }

    }

    public void BindRole()
    {
        BLAccess bla = new BLAccess();
        ddlRole.DataSource = bla.GetLookUpActiveDataWithoutSelect("RESPONSIBILITY");
        ddlRole.DataBind();
        ddlRole.Items.Insert(0, new ListItem("(select)", "0"));

    }

    public void BindRoleMass()
    {
        BLAccess bla = new BLAccess();
        //ddlRoleMass.DataSource 

        IList<LookupBE> lookups = bla.GetLookUpActiveDataWithoutSelect("RESPONSIBILITY");

        //foreach (var item in lookups)
        //{
        //    if (item.LookUpID == 359 | item.LookUpID == 362 | item.LookUpID == 363)
        //    {
        //        lookups.Remove(item);
        //    }
        //}
        ddlRoleMass.DataSource = lookups;
        ddlRoleMass.DataBind();
        //foreach (ListItem item in ddlRoleMass.Items)
        //{

        //    if(item.Value=="359"|item.Value=="362"|item.Value=="363")
        //        ddlRoleMass.Items.Remove(item);
        //}

        //ddlRoleMass.DataBind();
        ddlRoleMass.Items.Insert(0, new ListItem("(select)", "0"));

        ddlRoleMass.Items.Remove(ddlRoleMass.Items.FindByValue("359"));
        ddlRoleMass.Items.Remove(ddlRoleMass.Items.FindByValue("362"));
        ddlRoleMass.Items.Remove(ddlRoleMass.Items.FindByValue("363"));

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindSearchAccountList(0, null, true);
        
    }

    private void BindSearchAccountList(int assignType,string ids,bool validation)
    {
        //int intBrokerid = Convert.ToInt32(ddlBroker.SelectedValue);
        //int intBUOfficeid = Convert.ToInt32(this.ddlOffice.SelectedValue);
        //char stVal = ((ddlSearchstart.SelectedIndex >= 0) ? this.ddlSearchstart.SelectedItem.Text[0] : ' ');
        //char edVal = ((ddlSearchend.SelectedIndex >= 0) ? this.ddlSearchend.SelectedItem.Text[0] : ' ');

        //Session["AccountList"] = null;
        //Session["Search"] = null;
        SaveObjectToSessionUsingWindowName("Search", null);
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");


        //int intCustomerid = 0;
        if (validation == true)
        {
            if (ddlAccnts.SelectedIndex <= 0 && ddlOffice.SelectedIndex == 0 && ddlBroker.SelectedIndex == 0 && ddlBU.SelectedIndex == 0 && ddlOfficeOnly.SelectedIndex == 0 && ddlRole.SelectedIndex == 0 && ddlUser.SelectedIndex == 0 && txtBPNumber.Text == string.Empty && chkRangeSearch.Checked == false && chkBURangeSearch.Checked == false && ChkBrokerRangeSearch.Checked == false && fleAssign.HasFile == false)
            {
                string strMessage = "Please select at least one of the search criteria prior to clicking Search button";
                ShowError(strMessage);
                return;
            }
        }
        
        IList<ProgramPeriodBE> Customerlist;

        //if (this.chkRangeSearch.Checked == true)
        //    //This method is used to get details based on the range.
        //    Customerlist = ProgramperiodsService.getrangeprogramperiods(intBUOfficeid, intBrokerid, stVal, edVal);
        //else
        //{if(ddlAccnts.SelectedValue!="")
        //    intCustomerid = Convert.ToInt32(ddlAccnts.SelectedValue);
        //    // This method is used to get the customerslist.
        //    Customerlist = ProgramperiodsService.GetCustomerList(intCustomerid, intBUOfficeid, intBrokerid);
        //}

        int? custmr_id = (ddlAccnts.SelectedIndex > 0) ? Convert.ToInt32(ddlAccnts.SelectedValue) : (int?)null;

        int? buofficeid = (ddlOffice.SelectedIndex > 0) ? Convert.ToInt32(ddlOffice.SelectedValue) : (int?)null;

        string buname = (ddlBU.SelectedIndex > 0) ? ddlBU.SelectedItem.Text : null;

        string buoffice = (ddlOfficeOnly.SelectedIndex > 0) ? ddlOfficeOnly.SelectedItem.Text : null;

        int? brokerid = (ddlBroker.SelectedIndex > 0) ? Convert.ToInt32(ddlBroker.SelectedValue) : (int?)null;

        int? roleid = (ddlRole.SelectedIndex > 0) ? Convert.ToInt32(ddlRole.SelectedValue) : (int?)null;

        int? userid = (ddlUser.SelectedIndex > 0) ? Convert.ToInt32(ddlUser.SelectedValue) : (int?)null;

        string bpnumber = (txtBPNumber.Text != string.Empty) ? txtBPNumber.Text : null;

        string acct_range = (chkRangeSearch.Checked) ? "[" + ddlSearchstart.SelectedItem.Text + "-" + ddlSearchend.Text + "]%" : null;

        string buoffice_range = (chkBURangeSearch.Checked) ? "[" + ddlBUstart.SelectedItem.Text + "-" + ddlBUend.Text + "]%" : null;

        string broker_range = (ChkBrokerRangeSearch.Checked) ? "[" + ddlBrokerstart.SelectedItem.Text + "-" + ddlBrokerend.Text + "]%" : null;    

        Hashtable ht = new Hashtable();
        ht.Add("custmr_id", custmr_id);
        ht.Add("buofficeid", buofficeid);
        ht.Add("buname", buname);
        ht.Add("buoffice", buoffice);
        ht.Add("brokerid", brokerid);
        ht.Add("roleid", roleid);
        ht.Add("userid", userid);
        ht.Add("bpnumber", bpnumber);
        ht.Add("acct_range", acct_range);
        ht.Add("buoffice_range", buoffice_range);
        ht.Add("broker_range", broker_range);
        if (assignType == 1)
        {
            ht.Add("custmr_ids", ids);
            ht.Add("bp_ids", null);
        }
        else if (assignType == 2)
        {
            ht.Add("bp_ids", ids);
            ht.Add("custmr_ids", null);
        }
        //if (assignType == 1 && !string.IsNullOrWhiteSpace(ids))
        //    Customerlist = ProgramperiodsService.GetCusmrDetails_AccountAssgiment(custmr_id, ids, buofficeid, null, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);
        //else if (assignType == 2 && !string.IsNullOrWhiteSpace(ids))
        //    Customerlist = ProgramperiodsService.GetCusmrDetails_AccountAssgiment(custmr_id, null, buofficeid, ids, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);
        //else
        //    Customerlist = ProgramperiodsService.GetCusmrDetails_AccountAssgiment(custmr_id, null, buofficeid, null, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);

        //Session["Search"] = ht;
        SaveObjectToSessionUsingWindowName("Search", ht);

        DataTable dt;
        if (assignType == 1 && !string.IsNullOrWhiteSpace(ids))
            dt = ProgramperiodsService.GetCustmrDetails_WithRoles(custmr_id, ids, buofficeid, null, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);
        else if (assignType == 2 && !string.IsNullOrWhiteSpace(ids))
            dt = ProgramperiodsService.GetCustmrDetails_WithRoles(custmr_id, null, buofficeid, ids, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);
        else
            dt = ProgramperiodsService.GetCustmrDetails_WithRoles(custmr_id, null, buofficeid, null, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);
        
        this.lstAccountList.Items.Clear();
        this.lstAccountList.DataSource = dt;

        this.lstAccountList.DataBind();
        this.tdAccountsList.Visible = true;
        //Session["AccountList"] = dt;
        if (dt.Rows.Count > 0)
        {
            ((CheckBox)lstAccountList.FindControl("chkSelectAll")).Checked = true;

            this.tdResponsibilities.Visible = true;
            this.lstAssignResponsibilities.DataBind();
            for (int icount = 0; icount <= lstAssignResponsibilities.Items.Count - 1; icount++)
            {
                HiddenField hdn = (HiddenField)lstAssignResponsibilities.Items[icount].FindControl("hfResponseid");
                DropDownList ddlPerson = (DropDownList)lstAssignResponsibilities.Items[icount].FindControl("ddlName");
                if ((new[] { "359", "362", "363" }).Contains(hdn.Value))
                {
                    ddlPerson.Enabled = false;
                    ddlPerson.SelectedIndex = -1;
                }
                ddlPerson.Items.Insert(1, new ListItem("", ""));
                //else
                //{

                //    // ListItem lstitem = new ListItem();
                //    ddlPerson.SelectedIndex = -1;
                //    ddlPerson.DataSourceID = "PersonDatasource";
                //    ddlPerson.DataTextField = "FULLNAME";
                //    ddlPerson.DataValueField = "PERSON_ID";
                //    ddlPerson.DataBind();
                //}
            }
            lblRecords.Text = "Total " + dt.Rows.Count + " Record(s) Found";
            btnExport.Visible = true;
            
        }
        else
        {
            this.tdResponsibilities.Visible = false;
            lblRecords.Text = "";
            btnExport.Visible = false;
        }

        this.lstViewAccountLevel.Visible = false;
        this.lblAccountLevel.Visible = false;
        //if(intCustomerid>0)
        //    lstAcctresponsibilities = CustomercontactsBS.getAccountResponsibilities(intCustomerid);
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

        modalAcctDetails.Show();

    }

    private void BindResponsibilities(int intCustomerid)
    {
        HiddenField hfPersonid;
        DropDownList ddlPersons;
       
        lstAcctresponsibilities = CustomercontactsBS.getAccountResponsibilities(intCustomerid);
        if (lstAcctresponsibilities.Count > 0)
        {
            //for (int icount = 0; icount <= lstAssignResponsibilities.Items.Count - 1; icount++)
            //{
            //    DropDownList ddlPerson = (DropDownList)lstAssignResponsibilities.Items[icount].FindControl("ddlName");
            //    // ListItem lstitem = new ListItem();
            //    ddlPerson.SelectedIndex = -1;
            //    ddlPerson.DataSourceID = "PersonDatasource";
            //    ddlPerson.DataTextField = "FULLNAME";
            //    ddlPerson.DataValueField = "PERSON_ID";
            //    ddlPerson.DataBind();
            //}

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
                        //ddlPersons.Items.Clear();
                        //ddlPersons.DataSourceID = "PersonDatasource";
                        //ddlPersons.DataTextField = "FULLNAME";
                        //ddlPersons.DataValueField = "PERSON_ID";
                       
                        //ddlPersons.DataBind();
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
        
        CheckBox chkSelected;
        
        int count = 0;
        for (int custcount = 0; custcount < lstAccountList.Items.Count; custcount++)
        {
            
            chkSelected = (CheckBox)lstAccountList.Items[custcount].FindControl("chkSelect");
            if (chkSelected.Checked == true)
            {
                count++;

                if (count > 1)
                    break;

            }
        }

        if (count > 1)
            modalConfirm.Show();
        else
            SaveDetails_New();
    }

    void SaveDetails()
    {
        HiddenField hfCustmrid, hfRoleid;
        CheckBox chkSelected;
        DropDownList ddlPersons;
        int intCustomerid, intpersonid;
        //int intpersonid;
        //lstAssignResponsibilities
        string strCustmrids = "";
        string strRoleids = "";
        string strPersids = "";
        int selectCount = 0;
        for (int custcount = 0; custcount < lstAccountList.Items.Count; custcount++)
        {
            hfCustmrid = (HiddenField)lstAccountList.Items[custcount].FindControl("hidCustmrid");
            chkSelected = (CheckBox)lstAccountList.Items[custcount].FindControl("chkSelect");
            intCustomerid = Convert.ToInt32(hfCustmrid.Value);


            if (chkSelected.Checked == true)
            {
                selectCount++;
                //ArrayList arrlstResponsibilities = new ArrayList();
                //ArrayList arrlstPersons = new ArrayList();
                strRoleids = "";
                strPersids = "";
                strCustmrids += intCustomerid.ToString() + ",";
                for (int respcount = 0; respcount < lstAssignResponsibilities.Items.Count; respcount++)
                {
                    hfRoleid = (HiddenField)lstAssignResponsibilities.Items[respcount].FindControl("hfResponseid");
                    ddlPersons = (DropDownList)lstAssignResponsibilities.Items[respcount].FindControl("ddlName");


                    if (ddlPersons.SelectedIndex > 0)
                    {
                        strRoleids += hfRoleid.Value + ",";

                        if (ddlPersons.SelectedValue == string.Empty)
                        {
                            strPersids += "0" + ",";
                        }
                        else
                        {
                            strPersids += ddlPersons.SelectedValue + ",";
                        }
                    }


                    //CustomercontactsBE=
                    //intpersonid = Convert.ToInt32(ddlPersons.SelectedValue);
                    //if (intpersonid > 0)
                    //{
                    //    arrlstResponsibilities.Add(Convert.ToInt32(hfPersonid.Value));
                    //    arrlstPersons.Add(Convert.ToInt32(ddlPersons.SelectedValue));
                    //}
                    //else 
                    //{

                    //}
                }
                // This method is used to assign responsibilities.

                //string errorMessage = String.Empty;
                //bool handleConcurrency = (lstAccountList.Items.Count == 1 && chkSelected.Checked);
                //bool success = CustomercontactsBS.AssignResponsibilities(intCustomerid,
                //  arrlstResponsibilities, arrlstPersons, out errorMessage, lstAcctresponsibilities, handleConcurrency, CurrentAISUser.PersonID);
                //Shows Concurrent Conflict Error
                //ShowConcurrentConflict(success, errorMessage);
                //lstAcctresponsibilities = CustomercontactsBS.getAccountResponsibilities(intCustomerid);

                //if (success)
                //    ShowError("Data entry saved");
                //else
                //    ShowError(errorMessage);



            }
        }

        if (!string.IsNullOrWhiteSpace(strCustmrids) && !string.IsNullOrWhiteSpace(strPersids))
        {
            //DataTable dtBefore = ProgramperiodsService.GetCustmrDetails_WithRoles(strCustmrids.TrimEnd(','));

            int count = ProgramperiodsService.AddCustPersRelAccountAssignment(strCustmrids.TrimEnd(','), strRoleids.TrimEnd(','), strPersids.TrimEnd(','), CurrentAISUser.PersonID);
            if (count > 0)
            {
                //DataTable dtAfter = ProgramperiodsService.GetCustmrDetails_WithRoles(strCustmrids.TrimEnd(','));
                //string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                //string strTime = DateTime.Now.ToString("HH-mm-ss");
                //string strErrFileName = "AccountAssingment_FinalView_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";
                //string strErrLogPath = ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + strErrFileName;
                //ExportToExcelAfterResults(dtBefore, dtAfter, strErrLogPath, strErrFileName);
                //string strMessage = @"Data entry saved" +
                //                    "Please <a href='" + strErrLogPath + "' target='_blank'>Click Here</a> to verify the updated records.";
                string strMessage = "Data entry saved";
                ShowError(strMessage);
            }
        }
        else if (string.IsNullOrWhiteSpace(strCustmrids))
        {
            ShowError("Please select atleast one account from account list");
        }
        else if (string.IsNullOrWhiteSpace(strPersids))
        {
            ShowError("Please select atleast one responsibilty to save details");
        }
    }

    void SaveDetails_New()
    {
        HiddenField hfCustmrid, hfRoleid;
        CheckBox chkSelected;
        DropDownList ddlPersons;
        int intCustomerid, intpersonid;        
        string strCustmrids = "";
        string strRoleids = "";
        string strPersids = "";
        int selectCount = 0;

        DateTime dtUploadDateTime = System.DateTime.Now;

        DataTable dt = new DataTable();

        dt.Columns.Add("Account_ID",typeof(System.Int32));
        dt.Columns.Add("ACCOUNT SETUP QC", typeof(System.String));
        dt.Columns.Add("ADJUSTMENT QC 100%", typeof(System.String));
        dt.Columns.Add("ADJUSTMENT QC 20%", typeof(System.String));
        dt.Columns.Add("ARiES QC", typeof(System.String));
        dt.Columns.Add("C&RM ADMIN ANALYST", typeof(System.String));
        dt.Columns.Add("C&RM COLLECTION SPECIALIST", typeof(System.String));
        dt.Columns.Add("CFS1", typeof(System.String));
        dt.Columns.Add("CFS2", typeof(System.String));
        dt.Columns.Add("LSS Admin", typeof(System.String));
        dt.Columns.Add("RECONCILER", typeof(System.String));
        dt.Columns.Add("CreatedBy", typeof(System.String));
        dt.Columns.Add("CreatedDate", typeof(System.DateTime));

        for (int custcount = 0; custcount < lstAccountList.Items.Count; custcount++)
        {
            hfCustmrid = (HiddenField)lstAccountList.Items[custcount].FindControl("hidCustmrid");
            chkSelected = (CheckBox)lstAccountList.Items[custcount].FindControl("chkSelect");
            intCustomerid = Convert.ToInt32(hfCustmrid.Value);


            if (chkSelected.Checked == true)
            {

                DataRow dr = dt.NewRow();
                dr["Account_ID"] = intCustomerid.ToString();
                dr["CreatedBy"] = CurrentAISUser.UserID;
                dr["CreatedDate"] = dtUploadDateTime;
                for (int respcount = 0; respcount < lstAssignResponsibilities.Items.Count; respcount++)
                {
                    ddlPersons = (DropDownList)lstAssignResponsibilities.Items[respcount].FindControl("ddlName");
                    if (respcount == 0)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["ACCOUNT SETUP QC"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["ACCOUNT SETUP QC"] = null;
                        }
                        else
                        {
                            dr["ACCOUNT SETUP QC"] = ddlPersons.SelectedValue;
                        }

                    }
                    else if (respcount == 1)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["ADJUSTMENT QC 100%"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["ADJUSTMENT QC 100%"] = null;
                        }
                        else
                        {
                            dr["ADJUSTMENT QC 100%"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 2)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["ADJUSTMENT QC 20%"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["ADJUSTMENT QC 20%"] = null;
                        }
                        else
                        {
                            dr["ADJUSTMENT QC 20%"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 3)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["ARiES QC"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["ARiES QC"] = null;
                        }
                        else
                        {
                            dr["ARiES QC"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 4)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["C&RM ADMIN ANALYST"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["C&RM ADMIN ANALYST"] = null;
                        }
                        else
                        {
                            dr["C&RM ADMIN ANALYST"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 5)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["C&RM COLLECTION SPECIALIST"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["C&RM COLLECTION SPECIALIST"] = null;
                        }
                        else
                        {
                            dr["C&RM COLLECTION SPECIALIST"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 6)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["CFS1"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["CFS1"] = null;
                        }
                        else
                        {
                            dr["CFS1"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 7)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["CFS2"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["CFS2"] = null;
                        }
                        else
                        {
                            dr["CFS2"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 8)
                    {
                        if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["LSS Admin"] = "-1";
                        }
                        else if (ddlPersons.SelectedIndex == 1)
                        {
                            dr["LSS Admin"] = null;
                        }
                        else
                        {
                            dr["LSS Admin"] = ddlPersons.SelectedValue;
                        }
                    }
                    else if (respcount == 9)
                    {
                         if (ddlPersons.SelectedIndex == 0)
                        {
                            dr["RECONCILER"] = "-1";
                        }
                         else if (ddlPersons.SelectedIndex == 1)
                         {
                             dr["RECONCILER"] = null;
                         }
                         else
                         {
                             dr["RECONCILER"] = ddlPersons.SelectedValue;
                         }
                    }
                }
                dt.Rows.Add(dr);
            }
        }

        bool status = ProgramperiodsService.MassReassignStage(dt);

        string strErr = ProgramperiodsService.ModAISProcessMassReassignments(CurrentAISUser.UserID, dtUploadDateTime);

        if(!string.IsNullOrWhiteSpace(strErr))
            ShowError("Data entry failed to save, Please try again");
        else
            ShowError("Data entry saved");

        //DataTable dt1 = dt;
        //if (!string.IsNullOrWhiteSpace(strCustmrids) && !string.IsNullOrWhiteSpace(strPersids))
        //{
        //    int count = ProgramperiodsService.AddCustPersRelAccountAssignment(strCustmrids.TrimEnd(','), strRoleids.TrimEnd(','), strPersids.TrimEnd(','), CurrentAISUser.PersonID);
        //    if (count > 0)
        //    {
        //        string strMessage = "Data entry saved";
        //        ShowError(strMessage);
        //    }
        //}
        //else if (string.IsNullOrWhiteSpace(strCustmrids))
        //{
        //    ShowError("Please select atleast one account from account list");
        //}
        //else if (string.IsNullOrWhiteSpace(strPersids))
        //{
        //    ShowError("Please select atleast one responsibilty to save details");
        //}
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Panel pnlAccountlist = (Panel)this.ddlAcctlist.FindControl("pnlAccntlist");
        pnlAccountlist.Enabled = true;
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        ddlOffice.Enabled = true;
        ddlBroker.Enabled = true;
        ddlBU.Enabled = true;
        ddlOfficeOnly.Enabled = true;
        ddlAccnts.SelectedIndex = -1;
        this.ddlOffice.SelectedIndex = -1;
        this.ddlBroker.SelectedIndex = -1;
        this.ddlBU.SelectedIndex = -1;
        this.ddlOfficeOnly.SelectedIndex = -1;
        this.ddlRole.SelectedIndex = -1;
        this.ddlUser.SelectedIndex = -1;
        txtBPNumber.Text = string.Empty;
        chkRangeSearch.Checked = false;
        chkBURangeSearch.Checked = false;
        ChkBrokerRangeSearch.Checked = false;
        tblAccountRange.Visible = false;
        tblBrokerRange.Visible = false;
        tblBURange.Visible = false;
        tblMassReassign.Visible = false;
        this.tdResponsibilities.Visible = false;
        lstViewAccountLevel.Visible = false;
        lblAccountLevel.Visible = false;
        tdAccountsList.Visible = false;
        ddlRole.Enabled = false;
    }


    protected void btnMassAssign_Click(object sender, EventArgs e)
    {
        tblMassReassign.Visible = true;
        lblHeader.Text = "Account Match";
        lblErrorLog.Text = "";
        ddlAssignType.SelectedIndex = 0;
        trRolesMass.Visible = false;
        ddlRoleMass.Items.Clear();
        btnProcess.Text = "Process";
        btnValidate.Visible = false;
    }

    protected void btnBulkUpload_Click(object sender, EventArgs e)
    {
        tblMassReassign.Visible = true;
        lblHeader.Text = "Multiple User Upload";
        lblErrorLog.Text = "";
        ddlAssignType.SelectedIndex = 0;
        trRolesMass.Visible = true;
        BindRoleMass();
        btnProcess.Text = "Upload";
        btnValidate.Visible = true;
        btnProcess.Enabled = false;
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        tblMassReassign.Visible = false;
        lblErrorLog.Text = "";
        ddlAssignType.SelectedIndex = 0;
    }


    void BulkUpload(string vflag)
    {
        //Session["custmr_ids"] = "";
        //Session["user_ids"] = "";
        //Session["roleid"] = "";
        //Session["AssignType"] = "";
        SaveObjectToSessionUsingWindowName("custmr_ids", "");
        SaveObjectToSessionUsingWindowName("user_ids", "");
        SaveObjectToSessionUsingWindowName("roleid", "");
        SaveObjectToSessionUsingWindowName("AssignType", "");

        if (fleAssign.HasFile && ddlAssignType.SelectedIndex > 0 && ddlRoleMass.SelectedIndex > 0)
        {
            if (Path.GetExtension(fleAssign.PostedFile.FileName) == ".xls")
            {
                //string strDate = DateTime.Now.ToString().Replace(":", "-");
                //strDate = strDate.Replace("/", "-");
                string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                string strTime = DateTime.Now.ToString("HH-mm-ss");

                string fileName = "MassReassignmentBulkUpload_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";

                if (!(Directory.Exists(ConfigurationManager.AppSettings["MassReassignFilesPath"])))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["MassReassignFilesPath"]);
                }

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                fleAssign.SaveAs(ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + fileName);

                //full path name
                string filename = ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + fileName;

                DataTable excelSheetDT = GetDataTableExcel(filename);

                string strCustmrids = "", strUserIds = "";
                for (int i = 0; i < excelSheetDT.Rows.Count; i++)
                {
                    strCustmrids += excelSheetDT.Rows[i][0].ToString() + ",";
                    strUserIds += excelSheetDT.Rows[i][1].ToString() + ",";
                }
                DataTable errorLogDT = ProgramperiodsService.AddCustPersRelAccountAssignmentWithUserName(strCustmrids.TrimEnd(','), strUserIds.TrimEnd(','), ddlRoleMass.SelectedValue, CurrentAISUser.PersonID, Convert.ToInt32(ddlAssignType.SelectedValue), vflag);

                //Session["custmr_ids"] = strCustmrids.TrimEnd(',');
                //Session["user_ids"] = strUserIds.TrimEnd(',');
                //Session["roleid"] = ddlRoleMass.SelectedValue;
                //Session["AssignType"] = Convert.ToInt32(ddlAssignType.SelectedValue);
                SaveObjectToSessionUsingWindowName("custmr_ids", strCustmrids.TrimEnd(','));
                SaveObjectToSessionUsingWindowName("user_ids", strUserIds.TrimEnd(','));
                SaveObjectToSessionUsingWindowName("roleid", ddlRoleMass.SelectedValue);
                SaveObjectToSessionUsingWindowName("AssignType", Convert.ToInt32(ddlAssignType.SelectedValue));

                if (errorLogDT.Rows.Count > 0)
                {

                    string strErrFileName = "AccountBulkAssingment_ERROR_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";
                    string strErrLogPath = ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + strErrFileName;
                    ExportToExcel(errorLogDT, strErrLogPath, strErrFileName);
                    string strMessage = @"Processed " + (excelSheetDT.Rows.Count - errorLogDT.Rows.Count).ToString() + " accounts out of " + excelSheetDT.Rows.Count.ToString() + " accounts." +
                                        "Please <a href='" + strErrLogPath + "' target='_blank'>Click Here</a> for Error log.";

                    ShowError(strMessage);
                    tblMassReassign.Visible = false;
                }
                else
                {
                    string strMessage = "Processed successfully. Please verify the records before assigning the responsibilities";
                    ShowError(strMessage);
                    tblMassReassign.Visible = false;
                }

            }
            else
            {
                ShowError("Please Upload .xls format files only.");
            }
        }
        else
        {
            ShowError("Select assign type,Role and upload excel sheet");
        }
    }

    void BulkUploadValidate()
    {
        //Session["Bulkupload"] = null;
        //Session["Bulkupload_Error"] = null;
        SaveObjectToSessionUsingWindowName("Bulkupload", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_Error", null);
        if (fleAssign.HasFile && ddlAssignType.SelectedIndex > 0 && ddlRoleMass.SelectedIndex > 0)
        {
            if (Path.GetExtension(fleAssign.PostedFile.FileName) == ".xls")
            {
                string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                string strTime = DateTime.Now.ToString("HH-mm-ss");
                DateTime dtUploadDateTime = System.DateTime.Now;

                string fileName = "MultipleUserUpload_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";

                if (!(Directory.Exists(ConfigurationManager.AppSettings["MassReassignFilesPath"])))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["MassReassignFilesPath"]);
                }

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                fleAssign.SaveAs(ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + fileName);

                //full path name
                string filename = ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + fileName;

                DataTable excelSheetDT = GetDataTableExcel(filename);
                

                //Session["Bulkupload"] = excelSheetDT;
                SaveObjectToSessionUsingWindowName("Bulkupload", excelSheetDT);

                DataColumn RoleId = new DataColumn("rol_id", typeof(System.String));
                RoleId.DefaultValue = Convert.ToInt32(ddlRoleMass.SelectedValue);
                excelSheetDT.Columns.Add(RoleId);

                DataColumn crteUserId = new DataColumn("crte_usr_id", typeof(System.String));
                crteUserId.DefaultValue = CurrentAISUser.UserID;
                excelSheetDT.Columns.Add(crteUserId);

                DataColumn crte_dt = new DataColumn("crte_dt", typeof(System.DateTime));
                crte_dt.DefaultValue = dtUploadDateTime;
                excelSheetDT.Columns.Add(crte_dt);

                DataColumn validate = new DataColumn("validate", typeof(System.Int32));

                validate.DefaultValue = 1;

                excelSheetDT.Columns.Add(validate);

                bool status = ProgramperiodsService.MultipleUserUploadStage(excelSheetDT, Convert.ToInt32(ddlAssignType.SelectedValue));

                string errmsg = ProgramperiodsService.ModAISProcessMassReassignmentsUploads(CurrentAISUser.UserID, dtUploadDateTime, Convert.ToInt32(ddlAssignType.SelectedValue));


                DataTable errorLogDT = ProgramperiodsService.MassReassignmentsUploadsError(CurrentAISUser.UserID, dtUploadDateTime, Convert.ToInt32(ddlAssignType.SelectedValue));
                //Session["Bulkupload_Error"] = errorLogDT;
                SaveObjectToSessionUsingWindowName("Bulkupload_Error", errorLogDT);

                if ((excelSheetDT.Rows.Count - errorLogDT.Rows.Count) == 0)
                {
                    btnProcess.Enabled = false;
                }
                else
                {
                    btnProcess.Enabled = true;
                }
                

                if (errorLogDT.Rows.Count > 0)
                {
                    string strErrFileName = "MultipleUserUpload_ERROR_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";
                    string strErrLogPath = ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + strErrFileName;
                    ExportToExcel(errorLogDT, strErrLogPath, strErrFileName);
                    string strMessage = @"Validated " + (excelSheetDT.Rows.Count - errorLogDT.Rows.Count).ToString() + " records out of " + excelSheetDT.Rows.Count.ToString() + " records." +
                                        "Please <a href='" + strErrLogPath + "' target='_blank'>Click Here</a> for Error log.";

                    ShowError(strMessage);
                    tblMassReassign.Visible = false;
                }
                else
                {
                    string strMessage = "Processed successfully. Please verify the records before assigning the responsibilities";
                    ShowError(strMessage);
                    tblMassReassign.Visible = false;
                }

            }
            else
            {
                ShowError("Please Upload .xls format files only.");
            }
        }
        else
        {
            ShowError("Select assign type,Role and upload excel sheet");
        }
    }

    void BulkUploadSave()
    {
        //if (ddlAssignType.SelectedIndex > 0 && ddlRoleMass.SelectedIndex > 0 && Session["Bulkupload"] != null)
        if (ddlAssignType.SelectedIndex > 0 && ddlRoleMass.SelectedIndex > 0 && RetrieveObjectFromSessionUsingWindowName("Bulkupload") != null)
        {

            DateTime dtUploadDateTime = System.DateTime.Now;

            //DataTable excelSheetDT = (DataTable)Session["Bulkupload"];
            DataTable excelSheetDT = (DataTable)RetrieveObjectFromSessionUsingWindowName("Bulkupload");

            excelSheetDT.Columns.Remove("validate");
            excelSheetDT.Columns.Remove("crte_usr_id");
            excelSheetDT.Columns.Remove("crte_dt");


            DataColumn crteUserId = new DataColumn("crte_usr_id", typeof(System.String));
            crteUserId.DefaultValue = CurrentAISUser.UserID;
            excelSheetDT.Columns.Add(crteUserId);

            DataColumn crte_dt = new DataColumn("crte_dt", typeof(System.DateTime));
            crte_dt.DefaultValue = dtUploadDateTime;
            excelSheetDT.Columns.Add(crte_dt);


            DataColumn validate = new DataColumn("validate", typeof(System.Int32));

            validate.DefaultValue = 0;

            excelSheetDT.Columns.Add(validate);

            bool status = ProgramperiodsService.MultipleUserUploadStage(excelSheetDT, Convert.ToInt32(ddlAssignType.SelectedValue));

            string errmsg = ProgramperiodsService.ModAISProcessMassReassignmentsUploads(CurrentAISUser.UserID, dtUploadDateTime, Convert.ToInt32(ddlAssignType.SelectedValue));

            string strMessage = "Processed successfully.";
            ShowError(strMessage);
            tblMassReassign.Visible = false;

            excelSheetDT.Columns.Remove("rol_id");
            excelSheetDT.Columns.Remove("crte_usr_id");
            excelSheetDT.Columns.Remove("crte_dt");
            excelSheetDT.Columns.Remove("validate");

            //try
            //{
            //DataTable errorLogDT = (DataTable)Session["Bulkupload_Error"];
            DataTable errorLogDT = (DataTable)RetrieveObjectFromSessionUsingWindowName("Bulkupload_Error");
            DataTable tblResults;
            if (errorLogDT.Rows.Count > 0)
            {
                errorLogDT.Columns.Remove("Error");
                for (int i = 0; i < excelSheetDT.Rows.Count; ++i)
                {
                    for (int j = 0; j < errorLogDT.Rows.Count; j++)
                    {
                        if (excelSheetDT.Rows[i].RowState != DataRowState.Deleted)
                        {
                            var arr1 = excelSheetDT.Rows[i].ItemArray;
                            var arr2 = errorLogDT.Rows[j].ItemArray;
                            if (arr2.SequenceEqual(arr1))
                            {
                                excelSheetDT.Rows[i].Delete();
                                //--i;
                                if (i > 0)
                                {
                                    --i;
                                }
                                continue;
                            }
                        }
                    }
                }
                excelSheetDT.AcceptChanges();
            }

            string id = "";
            for (int i = 0; i < excelSheetDT.Rows.Count; i++)
            {
                string strID = Convert.ToString(excelSheetDT.Rows[i][0]);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    id += "," + strID;
                }
                else
                {
                    id = strID;
                }
            }
            BindSearchAccountList(Convert.ToInt32(ddlAssignType.SelectedValue), id, false);

            //}
            //catch (Exception ex)
            //{
            //    ShowError("Records saved successfully, But unable to get the records to view");
            //}

            //Session["Bulkupload"] = null;
            //Session["Bulkupload_Error"] = null;
            SaveObjectToSessionUsingWindowName("Bulkupload", null);
            SaveObjectToSessionUsingWindowName("Bulkupload_Error", null);
        }

        else
        {
            ShowError("Please validate the records prior to Upload.");
        }
    }

    

    protected void btnProcess_Click(object sender, EventArgs e)
    {

        if (ddlRoleMass.Visible == true)
        {
            ////BulkUpload("IU"); 
            //if (Session["custmr_ids"] != null && Session["user_ids"] != null && Session["roleid"] != null && Session["AssignType"] != null)
            //{
            //    string strCustmrids = Session["custmr_ids"].ToString();
            //    string strUserIds = Session["user_ids"].ToString();
            //    string strRole = Session["roleid"].ToString();
            //    int assignType = Convert.ToInt32(Session["AssignType"].ToString());

            //    DataTable errorLogDT = ProgramperiodsService.AddCustPersRelAccountAssignmentWithUserName(strCustmrids, strUserIds, strRole, CurrentAISUser.PersonID, assignType, "IU");
            //    string strMessage = "Uploaded successfully.";
            //    ShowError(strMessage);
            //    tblMassReassign.Visible = false;
            //}
            //else
            //{
            //    string strMessage = "Upload Failed, Please try again.";
            //    ShowError(strMessage);
            //    tblMassReassign.Visible = false;
            //}

            BulkUploadSave();
        }
        else
        {
            if (fleAssign.HasFile && ddlAssignType.SelectedIndex > 0)
            {
                if (Path.GetExtension(fleAssign.PostedFile.FileName) == ".xls")
                {
                    //string strDate = DateTime.Now.ToString().Replace(":", "-");
                    //strDate = strDate.Replace("/", "-");
                    string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                    string strTime = DateTime.Now.ToString("HH-mm-ss");

                    string fileName = "MassReassignmentUpload_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";

                    if (!(Directory.Exists(ConfigurationManager.AppSettings["MassReassignFilesPath"])))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["MassReassignFilesPath"]);
                    }

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    fleAssign.SaveAs(ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + fileName);

                    //full path name
                    string filename = ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + fileName;

                    DataTable excelSheetDT = GetDataTableExcel(filename);

                    DataTable errorLogDT = excelSheetDT.Clone();
                    errorLogDT.Columns.Add("Comments", typeof(System.String));

                    string strError = "";

                    int success = 0;
                    string id = "";

                    for (int i = 0; i < excelSheetDT.Rows.Count; i++)
                    {
                        bool isErrorRowImported = false;
                        bool isErrorCommentAdded = false;

                        string strID = Convert.ToString(excelSheetDT.Rows[i][0]);
                        if (ValidateNumber(strID.Trim(), out strError))
                        {
                            if (Convert.ToInt32(ddlAssignType.SelectedValue) == 1)
                            {
                                if (ProgramperiodsService.CheckCustmrIDExist(Convert.ToInt32(strID)))
                                {
                                    if (!string.IsNullOrWhiteSpace(id))
                                    {
                                        id += "," + strID;
                                    }
                                    else
                                    {
                                        id = strID;
                                    }
                                }

                                else
                                {
                                    if (!isErrorRowImported)
                                    {
                                        errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                        isErrorRowImported = true;
                                    }

                                    if (isErrorCommentAdded)
                                    {
                                        errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "Account Number Doesnot Exist";
                                    }
                                    else
                                    {
                                        errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Account Number Doesnot Exist";
                                        isErrorCommentAdded = true;
                                    }
                                }
                            }
                            if (Convert.ToInt32(ddlAssignType.SelectedValue) == 2)
                            {
                                if (ProgramperiodsService.CheckBPNumExist(strID))
                                {
                                    if (!string.IsNullOrWhiteSpace(id))
                                    {
                                        id += "," + strID;
                                    }
                                    else
                                    {
                                        id = strID;
                                    }
                                }
                                else
                                {
                                    if (!isErrorRowImported)
                                    {
                                        errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                        isErrorRowImported = true;
                                    }

                                    if (isErrorCommentAdded)
                                    {
                                        errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "BP Number Doesnot Exist";
                                    }
                                    else
                                    {
                                        errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "BP Number Doesnot Exist";
                                        isErrorCommentAdded = true;
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (!isErrorRowImported)
                            {
                                errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                isErrorRowImported = true;
                            }

                            if (isErrorCommentAdded)
                            {
                                errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + strError;

                            }
                            else
                            {
                                errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = strError;
                                isErrorCommentAdded = true;
                            }
                            //continue;
                        }

                        if (!isErrorCommentAdded)
                        {
                            success++;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        BindSearchAccountList(Convert.ToInt32(ddlAssignType.SelectedValue), id, true);
                    }

                    if (errorLogDT.Rows.Count > 0)
                    {

                        string strErrFileName = "AccountAssingment_ERROR_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";
                        string strErrLogPath = ConfigurationManager.AppSettings["MassReassignFilesPath"] + "\\" + strErrFileName;
                        ExportToExcel(errorLogDT, strErrLogPath, strErrFileName);
                        //lblErrorLog.Visible = true;
                        //lblErrorLog.Text = "Processed " + success.ToString() + " accounts out of " + excelSheetDT.Rows.Count.ToString() + " accounts. ";
                        //lblErrorLog.Text += "Please <a href='" + strErrLogPath + "' target='_blank'>Click Here</a>";
                        //lblErrorLog.Text += " for Error log";

                        string strMessage = @"Processed " + success.ToString() + " accounts out of " + excelSheetDT.Rows.Count.ToString() + " accounts." +
                                            "Please <a href='" + strErrLogPath + "' target='_blank'>Click Here</a> for Error log.<br/>" +
                                            "Please verify the records before assigning the responsibilities";

                        ShowError(strMessage);
                        tblMassReassign.Visible = false;
                    }
                    else
                    {
                        string strMessage = "Processed successfully. Please verify the records before assigning the responsibilities";
                        ShowError(strMessage);
                        tblMassReassign.Visible = false;
                    }

                }
                else
                {
                    ShowError("Please Upload .xls format files only.");
                }
            }
            else
            {
                ShowError("Select assign type and upload excel sheet with Account Number/BP Number");
            }
        }
    }

    protected void btnValidate_Click(object sender, EventArgs e)
    {
        BulkUploadValidate();
        tblMassReassign.Visible = true;
        lblHeader.Text = "Multiple User Upload";
        //btnProcess.Enabled = true;
    }
    
    private bool ValidateNumber(string strNumber, out string strError)
    {
        strError = "";
        bool ValidStatus = false;
        strError = "";
        if (!string.IsNullOrWhiteSpace(strNumber))
        {
            int num;
            if (int.TryParse(strNumber, out num))
            {
                strError = "";
                ValidStatus = true;
            }
            //if (Regex.IsMatch(strNumber, @"^[0-9]+$"))
            //{
            //    strError = "";
            //    ValidStatus = true;
            //}
            else
            {
                strError = "Account Number / BP Number  Should Contain only numerics";
                ValidStatus = false;
            }

        }
        else
        {
            strError = "Account Number / BP Number Should not be a blank";
            ValidStatus = false;
        }
        return ValidStatus;
    }

    private DataTable GetDataTableExcel(string strFileName)
    {
        System.Data.OleDb.OleDbConnection conn = null;

        try
        {
            conn =
                new System.Data.OleDb.OleDbConnection("Provider=" + ConfigurationManager.AppSettings["oledbProvider"] + "; Data Source = "
                                                + strFileName + "; Extended Properties = \"" + ConfigurationManager.AppSettings["oledbProperties"] + "\";");
            
            conn.Open();

            //DataTable Schemadt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //string sheetName = Schemadt.Rows[0]["TABLE_NAME"].ToString();

            //string strQuery = @"SELECT * FROM [@sheetName]";
            //OleDbCommand cmd = new OleDbCommand(strQuery, conn);
            //cmd.Parameters.Add("@sheetName", sheetName);
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", conn);
            System.Data.OleDb.OleDbDataAdapter adapter =
                new System.Data.OleDb.OleDbDataAdapter(cmd);

            
            DataTable dt = new DataTable();
            adapter.FillSchema(dt, SchemaType.Source);
            dt.Columns[0].DataType = typeof(string);
            adapter.Fill(dt);

            bool isEmpty = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                isEmpty = true;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.IsNullOrEmpty(dt.Rows[i][j].ToString()) == false)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty == true)
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }

            return dt;
        }

        catch (Exception ex)
        {
            throw;
        }
        finally { conn.Close(); }


    }

    private void ExportToExcel(DataTable Tbl, string ExcelFilePath, string strErrFileName)
    {
        try
        {
            if (Tbl == null || Tbl.Columns.Count == 0)
                throw new Exception("ExportToExcel: Null or empty input table!\n");

            // load excel, and create a new workbook
            Excel.Application excelApp = new Excel.Application();
            excelApp.Workbooks.Add();

            // single worksheet
            Excel._Worksheet workSheet = excelApp.ActiveSheet as Excel.Worksheet;

            workSheet.Cells.NumberFormat = "@";
            
            // column headings
            for (int i = 0; i < Tbl.Columns.Count; i++)
            {
                workSheet.Cells[1, (i + 1)] = Tbl.Columns[i].ColumnName;
                
            }

            // rows
            for (int i = 0; i < Tbl.Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < Tbl.Columns.Count; j++)
                {
                    workSheet.Cells[(i + 2), (j + 1)] = Tbl.Rows[i][j];
                }

            }



            // check fielpath
            if (ExcelFilePath != null && ExcelFilePath != "")
            {
                try
                {
                    //workSheet.Name = "abcdef";
                    workSheet.SaveAs(ExcelFilePath);
                    excelApp.Quit();
                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                        + ex.Message);
                }
            }
            else    // no filepath is given
            {
                excelApp.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("ExportToExcel: \n" + ex.Message);
        }
    }

    private void ExportToExcelAfterResults(DataTable tblBefore,DataTable tblAfter, string ExcelFilePath, string strErrFileName)
    {
        try
        {
            if (tblBefore == null || tblBefore.Columns.Count == 0)
                throw new Exception("ExportToExcel: Null or empty input table!\n");

            // load excel, and create a new workbook
            Excel.Application excelApp = new Excel.Application();
            excelApp.Workbooks.Add();

            // single worksheet
            Excel._Worksheet workSheet = excelApp.ActiveSheet as Excel.Worksheet;
            workSheet.Name = "Before Results";
            workSheet.Cells.NumberFormat = "@";

            // column headings
            for (int i = 0; i < tblBefore.Columns.Count; i++)
            {
                workSheet.Cells[1, (i + 1)] = tblBefore.Columns[i].ColumnName;

            }

            // rows
            for (int i = 0; i < tblBefore.Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < tblBefore.Columns.Count; j++)
                {
                    workSheet.Cells[(i + 2), (j + 1)] = tblBefore.Rows[i][j];
                }

            }



            // single worksheet
            Excel._Worksheet workSheetAfter = excelApp.Worksheets[2] as Excel.Worksheet;
            workSheetAfter.Activate();
            workSheetAfter.Name = "After Results";
            workSheetAfter.Cells.NumberFormat = "@";

            // column headings
            for (int i = 0; i < tblAfter.Columns.Count; i++)
            {
                workSheetAfter.Cells[1, (i + 1)] = tblAfter.Columns[i].ColumnName;

            }

            // rows
            for (int i = 0; i < tblAfter.Rows.Count; i++)
            {
                // to do: format datetime values before printing
                for (int j = 0; j < tblAfter.Columns.Count; j++)
                {
                    workSheetAfter.Cells[(i + 2), (j + 1)] = tblAfter.Rows[i][j];
                }

            }



            // check fielpath
            if (ExcelFilePath != null && ExcelFilePath != "")
            {
                try
                {
                    //workSheet.Name = "abcdef";
                    workSheet.SaveAs(ExcelFilePath);
                    //excelApp.s
                    excelApp.Quit();
                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                        + ex.Message);
                }
            }
            else    // no filepath is given
            {
                excelApp.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("ExportToExcel: \n" + ex.Message);
        }
    }

    public void ExportToExcel(DataTable dt, string fileName)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        StringWriter strWriter = new System.IO.StringWriter();
        HtmlTextWriter htmlWriter = new HtmlTextWriter(strWriter);

        DataGrid dg = new DataGrid();
        dg.HeaderStyle.Font.Bold = true;
        dg.DataSource = dt;
        dg.DataBind();

        

        dg.RenderControl(htmlWriter);
        Response.Write(strWriter.ToString());
        Response.End();
        
    }

    protected void btnResClear_Click(object sender, EventArgs e)
    {
        for (int icount = 0; icount <= lstAssignResponsibilities.Items.Count - 1; icount++)
        {
            DropDownList ddlPerson = (DropDownList)lstAssignResponsibilities.Items[icount].FindControl("ddlName");
            ddlPerson.SelectedIndex = 0;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        HiddenField hfCustmrid;
        CheckBox chkSelected;
        int intCustomerid;
        //string strCustmrids = "";
        //int assignType = 1;
        DataTable dt = new DataTable();
        //if (Session["AccountList"] != null)
        //{
        //    dt = (DataTable)Session["AccountList"];

        //    for (int custcount = 0; custcount < lstAccountList.Items.Count; custcount++)
        //    {
        //        hfCustmrid = (HiddenField)lstAccountList.Items[custcount].FindControl("hidCustmrid");
        //        chkSelected = (CheckBox)lstAccountList.Items[custcount].FindControl("chkSelect");
        //        intCustomerid = Convert.ToInt32(hfCustmrid.Value);


        //        //if (chkSelected.Checked == false)
        //        //{
        //        //    strCustmrids += intCustomerid.ToString() + ",";
        //        //}
        //        if (chkSelected.Checked == false)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                if (dr["Account No"].ToString() == intCustomerid.ToString())
        //                    dr.Delete();
        //            }
        //            dt.AcceptChanges();
        //        }
        //    }
        //if (Session["Search"] != null)
            if (RetrieveObjectFromSessionUsingWindowName("Search") != null)
            {
                //Hashtable ht = (Hashtable)Session["Search"];
                Hashtable ht = (Hashtable)RetrieveObjectFromSessionUsingWindowName("Search");
                int? custmr_id = (ht["custmr_id"] != null) ? Convert.ToInt32(ht["custmr_id"]) : (int?)null;

                int? buofficeid = (ht["buofficeid"] != null) ? Convert.ToInt32(ht["buofficeid"]) : (int?)null;

                string buname = (ht["buname"] != null) ? Convert.ToString(ht["buname"]) : null;

                string buoffice = (ht["buoffice"] != null) ? Convert.ToString(ht["buoffice"]) : null;

                int? brokerid = (ht["brokerid"] != null) ? Convert.ToInt32(ht["brokerid"]) : (int?)null;

                int? roleid = (ht["roleid"] != null) ? Convert.ToInt32(ht["roleid"]) : (int?)null;

                int? userid = (ht["userid"] != null) ? Convert.ToInt32(ht["userid"]) : (int?)null;

                string bpnumber = (ht["bpnumber"] != null) ? Convert.ToString(ht["bpnumber"]) : null;

                string acct_range = (ht["acct_range"] != null) ? Convert.ToString(ht["acct_range"]) : null;

                string buoffice_range = (ht["buoffice_range"] != null) ? Convert.ToString(ht["buoffice_range"]) : null;

                string broker_range = (ht["broker_range"] != null) ? Convert.ToString(ht["broker_range"]) : null;

                string custmr_ids = (ht["custmr_ids"] != null) ? Convert.ToString(ht["custmr_ids"]) : null;

                string bp_ids = (ht["bp_ids"] != null) ? Convert.ToString(ht["bp_ids"]) : null;

                dt = ProgramperiodsService.GetCustmrDetails_WithRoles(custmr_id, custmr_ids, buofficeid, bp_ids, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);


                for (int custcount = 0; custcount < lstAccountList.Items.Count; custcount++)
                {
                    hfCustmrid = (HiddenField)lstAccountList.Items[custcount].FindControl("hidCustmrid");
                    chkSelected = (CheckBox)lstAccountList.Items[custcount].FindControl("chkSelect");
                    intCustomerid = Convert.ToInt32(hfCustmrid.Value);


                    //if (chkSelected.Checked == false)
                    //{
                    //    strCustmrids += intCustomerid.ToString() + ",";
                    //}
                    if (chkSelected.Checked == false)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["Account No"].ToString() == intCustomerid.ToString())
                                dr.Delete();
                        }
                        dt.AcceptChanges();
                    }
                }
            //if (!string.IsNullOrWhiteSpace(strCustmrids))
            //{
            //    strCustmrids = strCustmrids.TrimEnd(',');
            //    DataTable dt = ProgramperiodsService.GetCustmrDetails_WithRoles(strCustmrids);
            //    //string strDate = DateTime.Now.ToString().Replace(":", "-");
            //    // strDate = strDate.Replace("/", "-");
            //    string strFileName = "CustomerDetails.xls";
            //    ExportToExcel(dt, strFileName);
            //}

            //DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");

            //if (ddlAccnts.SelectedIndex <= 0 && ddlOffice.SelectedIndex == 0 && ddlBroker.SelectedIndex == 0 && ddlBU.SelectedIndex == 0 && ddlOfficeOnly.SelectedIndex == 0 && ddlRole.SelectedIndex == 0 && ddlUser.SelectedIndex == 0 && txtBPNumber.Text == string.Empty && chkRangeSearch.Checked == false && chkBURangeSearch.Checked == false && ChkBrokerRangeSearch.Checked == false && fleAssign.HasFile == false)
            //{
            //    string strMessage = "Please select at least one of the search criteria prior to export to excel";
            //    ShowError(strMessage);
            //    return;
            //}

            ////IList<ProgramPeriodBE> Customerlist;

            //int? custmr_id = (ddlAccnts.SelectedIndex > 0) ? Convert.ToInt32(ddlAccnts.SelectedValue) : (int?)null;

            //int? buofficeid = (ddlOffice.SelectedIndex > 0) ? Convert.ToInt32(ddlOffice.SelectedValue) : (int?)null;

            //string buname = (ddlBU.SelectedIndex > 0) ? ddlBU.SelectedItem.Text : null;

            //string buoffice = (ddlOfficeOnly.SelectedIndex > 0) ? ddlOfficeOnly.SelectedItem.Text : null;

            //int? brokerid = (ddlBroker.SelectedIndex > 0) ? Convert.ToInt32(ddlBroker.SelectedValue) : (int?)null;

            //int? roleid = (ddlRole.SelectedIndex > 0) ? Convert.ToInt32(ddlRole.SelectedValue) : (int?)null;

            //int? userid = (ddlUser.SelectedIndex > 0) ? Convert.ToInt32(ddlUser.SelectedValue) : (int?)null;

            //string bpnumber = (txtBPNumber.Text != string.Empty) ? txtBPNumber.Text : null;

            //string acct_range = (chkRangeSearch.Checked) ? "[" + ddlSearchstart.SelectedItem.Text + "-" + ddlSearchend.Text + "]%" : null;

            //string buoffice_range = (chkBURangeSearch.Checked) ? "[" + ddlBUstart.SelectedItem.Text + "-" + ddlBUend.Text + "]%" : null;

            //string broker_range = (ChkBrokerRangeSearch.Checked) ? "[" + ddlBrokerstart.SelectedItem.Text + "-" + ddlBrokerend.Text + "]%" : null;
            //DataTable dt;
            //if (assignType == 0 && !string.IsNullOrWhiteSpace(strCustmrids))
            //    dt = ProgramperiodsService.GetCustmrDetails_WithRoles(custmr_id, strCustmrids, buofficeid, null, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);
            ////else if (assignType == 2 && !string.IsNullOrWhiteSpace(ids))
            ////    Customerlist = ProgramperiodsService.GetCusmrDetails_AccountAssgiment(custmr_id, null, buofficeid, ids, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);
            //else
            //    dt = ProgramperiodsService.GetCustmrDetails_WithRoles(custmr_id, null, buofficeid, null, buname, buoffice, brokerid, roleid, userid, bpnumber, acct_range, buoffice_range, broker_range);

            //string strDate = DateTime.Now.ToString().Replace(":", "-");
            // strDate = strDate.Replace("/", "-");
            string strFileName = "CustomerDetails.xls";
            ExportToExcel(dt, strFileName);
        }
    }

    protected void btnOKpopup_Click(object sender, EventArgs e)
    {
        modalConfirm.Hide();
        SaveDetails_New();
    }
    protected void btnCancelpopup_Click(object sender, EventArgs e)
    {
        modalConfirm.Hide();
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string FilePath;
        if (ddlRoleMass.Visible == true)
        {
          FilePath = ConfigurationManager.AppSettings["MassReassignTemplatePath"] + "\\" + ConfigurationManager.AppSettings["MultipleUserUploadTemplateName"];
        }
        else
           FilePath = ConfigurationManager.AppSettings["MassReassignTemplatePath"] + "\\" + ConfigurationManager.AppSettings["MassReassignTemplateName"];


        FileInfo file = new FileInfo(FilePath);

        // Checking if file exists
        if (file.Exists)
        {
            // Clear the content of the response
            Response.ClearContent();

            // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
            Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

            // Add the file size into the response header
            Response.AddHeader("Content-Length", file.Length.ToString());

            // Set the ContentType
            Response.ContentType = "application/vnd.ms-excel"; 

            // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
            Response.TransmitFile(file.FullName);

            //Response.BufferOutput = true;
            Response.Flush();
            //Response.Close();
            // End the response
            Response.End();
        }
        else
        {
            ShowError("Mass Reassignment Upload Template File not found for download");
        }
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUser.SelectedIndex > 0)
        {
            ddlRole.Enabled = true;
            ddlRole.SelectedIndex=0;
        }
        else
        {
            ddlRole.Enabled = false;
            ddlRole.SelectedIndex = 0;
        }

    }

    protected void ddlOfficeOnly_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOfficeOnly.SelectedIndex > 0 || ddlBU.SelectedIndex > 0)
        {
            ddlOffice.Enabled = false;
            ddlOffice.SelectedIndex = 0;
        }
        else
        {
            ddlOffice.Enabled = true;
            ddlOffice.SelectedIndex = 0;
        }
    }

    protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOffice.SelectedIndex > 0)
        {
            ddlOfficeOnly.Enabled = false;
            ddlBU.Enabled = false;
            ddlOfficeOnly.SelectedIndex = 0;
            ddlBU.SelectedIndex = 0;
        }
        else
        {
            ddlOfficeOnly.Enabled = true;
            ddlBU.Enabled = true;
            ddlOfficeOnly.SelectedIndex = 0;
            ddlBU.SelectedIndex = 0;
        }
    }
}
