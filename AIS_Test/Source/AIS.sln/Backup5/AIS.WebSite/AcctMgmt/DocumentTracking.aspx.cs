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
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using System.Collections.Generic;

public partial class AcctMgmt_DcoumentTracking : AISBasePage
{

    private string HidFormValue
    {
        get
        {
            string val = String.Empty;
            if (ViewState["HidFormValue"] != null)
                val = ViewState["HidFormValue"].ToString();
            return val;
        }
        set
        {
            ViewState["HidFormValue"] = value;
        }
    }

    private string DocumentID
    {
        get
        {
            string val = String.Empty;
            if (ViewState["DocumentID"] != null)
                val = ViewState["DocumentID"].ToString();
            return val;
        }
        set
        {
            ViewState["DocumentID"] = value;
        }
    }
    private bool IsQcCompleted
    {
        get
        {
            bool val = false;
            if (ViewState["IsQcCompleted"] != null)
                val = Convert.ToBoolean(ViewState["IsQcCompleted"]);
            return val;
        }
        set
        {
            ViewState["IsQcCompleted"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Page.Title = "Document-Tracking";
        //txtAmount.Enabled = false;
        this.ucAccountList.OperationsButtonClicked += new EventHandler(ucAccountList_OperationsButtonClicked);
        this.ucNonaisacctlst.OnSelectedIndexChanged += new EventHandler(ucNonaisacctlst_OnSelectedIndexChanged);
        this.ucAccountList.OnSelectedIndexChanged += new EventHandler(ucAccountList_OnSelectedIndexChanged);
        if (!IsPostBack)
        {
            this.txtdtvalidate.Text = System.DateTime.Now.AddDays(-1).ToShortDateString();
            //BindAccounts();
            int intCRMid = new BLAccess().GetLookUpID(GlobalConstants.InternalContacts.Crm);
            ContactNamesDataSource.SelectParameters["ContTypId"].DefaultValue = intCRMid.ToString();
            ContactNamesDataSource.Select();
            ContactNamesDataSource.DataBind();
            CustomerDocBE = new CustomerDocumentBE();
            NonaiscustmrBE = new NonAisCustomerBE();

            CheckNew = false;
            DocTransactionWrapper = new AISBusinessTransaction();
        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(txtAmount);
        list.Add(txtComments);
        list.Add(txtDateentered);
        list.Add(txtDtrecieved);
        list.Add(txtNonais);
        list.Add(txtPrgmeffdate);
        list.Add(txtPrgmexpdate);
        list.Add(txtQcdate);
        list.Add(txtValuationdate);
        list.Add(ddlBUoffice);
        list.Add(ddlCfs);
        list.Add(ddlFormsRecieved);
        list.Add(ddlNonais);
        list.Add(ddlQCBy);
        list.Add(chklstTrackingissues);
        list.Add(chkQC);
        list.Add(txtDtentered_CalendarExtender);
        list.Add(txtDtreciev_CalendarExtender);
        list.Add(txtPrgmdt_CalendarExtender);
        list.Add(txtPrgmexp_CalendarExtender);
        list.Add(txtQCDt_CalendarExtender);
        list.Add(txtVlndt_CalendarExtender);
        list.Add(lnkClose);
        list.Add(btnSave);
        list.Add(btnCancel);
        list.Add(btnAdd);
        ProcessExitFlag(list);

    }

    protected AISBusinessTransaction DocTransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["DocTransactionWrapper"] == null)
            //    Session["DocTransactionWrapper"] = new AISBusinessTransaction();
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("DocTransactionWrapper") == null)
                SaveObjectToSessionUsingWindowName("DocTransactionWrapper", new AISBusinessTransaction());

            //return (AISBusinessTransaction)Session["DocTransactionWrapper"];
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("DocTransactionWrapper");
        }
        set
        {
            //Session["DocTransactionWrapper"] = value;
            SaveObjectToSessionUsingWindowName("DocTransactionWrapper", value);
        }
    }
    protected void ucAccountList_OperationsButtonClicked(object sender, EventArgs e)
    {
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
        { }
        //this.btnAdd.Enabled = true;
        //this.btnSearch.Enabled = true;

    }
    protected void ucNonaisacctlst_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlNonaislist = (DropDownList)this.ucNonaisacctlst.FindControl("ddlAccountlist");
        int nonaisid = Convert.ToInt32(ddlNonaislist.SelectedItem.Value);
        if (nonaisid == 0)
        {
            this.txtNonais.Enabled = true;
            // this.pnlNonaissave.Visible = true;

        }
        else
        {
            this.txtNonais.Text = "";
            this.txtNonais.Enabled = false;
            this.pnlNonaissave.Visible = false;
            this.lstDocumentTracking.Visible = false;
            this.lblDocuments.Visible = false;


            this.pnlDetails.Visible = false;
            this.pnlNonaissave.Visible = false;
            this.divHeading.Visible = false;
        }
    }

    protected void ucAccountList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlNonaislist = (DropDownList)this.ucAccountList.FindControl("ddlAccountlist");
        int nonaisid = Convert.ToInt32(ddlNonaislist.SelectedItem.Value);
        if (nonaisid == 0)
        {
            //this.txtNonais.Enabled = true;
            // this.pnlNonaissave.Visible = true;

        }
        else
        {
            //this.txtNonais.Text = "";
            //.txtNonais.Enabled = false;
            //this.pnlNonaissave.Visible = false;
            this.lstDocumentTracking.Visible = false;
            this.lblDocuments.Visible = false;

            this.pnlDetails.Visible = false;
            this.pnlNonaissave.Visible = false;
            this.divHeading.Visible = false;
        }
    }

    private CustomerDocumentBE CustomerDocBE
    {
        get
        {
            //return (CustomerDocumentBE)Session["CustdocInfo-custdocBE"];
            return (CustomerDocumentBE)RetrieveObjectFromSessionUsingWindowName("CustdocInfo-custdocBE");
        }

        set 
        { 
            //Session["CustdocInfo-custdocBE"] = value; 
            SaveObjectToSessionUsingWindowName("CustdocInfo-custdocBE", value);
        }

    }
    private CustomerDocumentBS _CustomerDocService;
    private CustomerDocumentBS CustomerDocService
    {
        get
        {
            if (_CustomerDocService == null)
            {
                _CustomerDocService = new CustomerDocumentBS();
                _CustomerDocService.AppTransactionWrapper = DocTransactionWrapper;
            }
            return _CustomerDocService;
        }
        set { _CustomerDocService = value; }
    }

    private CustomerDocumentIssuesBS _CustomerdocissueService;
    private CustomerDocumentIssuesBS CustomerdocissueService
    {
        get
        {
            if (_CustomerdocissueService == null)
            {
                _CustomerdocissueService = new CustomerDocumentIssuesBS();
                _CustomerdocissueService.AppTransactionWrapper = DocTransactionWrapper;
            }
            return _CustomerdocissueService;
        }
        set { _CustomerdocissueService = value; }
    }
    private NonAisCustomerBE NonaiscustmrBE
    {
        get 
        { 
            //return (NonAisCustomerBE)Session["NonaiscustmrBE"]; 
            return (NonAisCustomerBE)RetrieveObjectFromSessionUsingWindowName("NonaiscustmrBE");
        }
        set 
        { 
            //Session["NonaiscustmrBE"] = value; 
            SaveObjectToSessionUsingWindowName("NonaiscustmrBE", value);
        }
    }
    private IList<CustomerDocumentBE> lstDocuments
    {
        get
        {
            //if (Session["DocumentsList"] == null)
            //    Session["DocumentsList"] = new List<CustomerDocumentBE>();
            //return (IList<CustomerDocumentBE>)Session["DocumentsList"];
            if (RetrieveObjectFromSessionUsingWindowName("DocumentsList") == null)
                SaveObjectToSessionUsingWindowName("DocumentsList", new List<CustomerDocumentBE>());
            return (IList<CustomerDocumentBE>)RetrieveObjectFromSessionUsingWindowName("DocumentsList");
        }
        set 
        { 
            //Session["DocumentsList"] = value; 
            SaveObjectToSessionUsingWindowName("DocumentsList", value);
        }

    }

    protected bool CheckNew
    {
        get { return (bool)ViewState["CheckNew"]; }
        set { ViewState["CheckNew"] = value; }
    }
    protected int SelectedValue
    {
        get
        {
            if (hidindex.Value != null)
            {
                return Convert.ToInt32(hidindex.Value);
            }
            else
            {
                return -1;
            }
        }
        set { hidindex.Value = value.ToString(); }
    }
    private string SortDir
    {
        get 
        { 
            //return (string)Session["SORTDIR"]; 
            return (string)RetrieveObjectFromSessionUsingWindowName("SORTDIR"); 
        }
        set 
        { 
            //Session["SORTDIR"] = value; 
            SaveObjectToSessionUsingWindowName("SORTDIR", value);
        }
    }
    protected void lstDocuments_Sorting(object sender, ListViewSortEventArgs e)
    {
        // Check which field is being sorted
        // to set the visibility of the image controls.
        Image imgSortByName = (Image)lstDocumentTracking.FindControl("imgSortByName");
        imgSortByName.Visible = true;
        if (imgSortByName.ToolTip == "Ascending")
        {
            imgSortByName.ToolTip = "Descending";
            imgSortByName.ImageUrl = "~/images/descending.gif";
            SortDir = "DESC";
        }
        else
        {
            imgSortByName.ToolTip = "Ascending";
            imgSortByName.ImageUrl = "~/images/Ascending.gif";
            SortDir = "ASC";
        }
        if (this.radRetro.Checked)
            BindDocuments(Convert.ToInt32(ViewState["customerid"]));
        else
            BindNonaisDocuments(Convert.ToInt32(ViewState["customerid"]));
    }
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SELECT")
        {
            // this.lblDocumentTracking.Visible = true;
            // this.lnkClose.Visible = true;
            this.pnlDetails.Visible = true;
            this.pnlNonaissave.Visible = true;
            this.divHeading.Visible = true;
            CheckNew = false;
            HiddenField hidforms = (HiddenField)e.Item.FindControl("hdforms");
            int intDocelemid = Convert.ToInt32(e.CommandArgument);
            SelectedValue = intDocelemid;

            HidFormValue = hidforms.Value;
            DocumentID = intDocelemid.ToString();

            //this.pnlDetails.Visible = true;
            this.lstDocumentTracking.Enabled = false;
            DisplayDetails(intDocelemid, hidforms.Value);


        }
        if (e.CommandName == "DISABLE")
        {
            DisableRow(Convert.ToInt32(e.CommandArgument), false);
        }
        if (e.CommandName == "ENABLE")
        {
            DisableRow(Convert.ToInt32(e.CommandArgument), true);
        }
    }

    protected void DisableRow(int Combinedelementid, bool Flag)
    {
        // This method is used to populate the combined elementsBE for a particular id.
        CustomerDocBE = CustomerDocService.getCustomerDocid(SelectedValue);
        CustomerDocBE.ACTV_IND = Flag;

        CustomerDocBE.UPDATED_USR_ID = CurrentAISUser.PersonID;
        CustomerDocBE.UPDATED_DATE = DateTime.Now;
        //This method is used to disable the row.
        Flag = CustomerDocService.save(CustomerDocBE);
        if (Flag)
        {
            DropDownList ddlAccnts = (DropDownList)this.ucAccountList.FindControl("ddlAccountlist");
            int intCustomerid = Convert.ToInt32(ddlAccnts.SelectedItem.Value);
            BindDocuments(intCustomerid);

        }

    }
    /// <summary>
    /// This method is used for hightlighting the row selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
        }
    }
    /// <summary>
    /// This method is used to populate the controls with the data.
    /// </summary>
    /// <param name="Documnetid"></param>
    public void DisplayDetails(int Documnetid, string strformname)
    {

        //if (strformname.Trim() == "Maryland Retro Only" || strformname.Trim() == "Par Plan Only")
        //{
        //    this.txtAmount.Enabled = true;


        //}
        //else
        //{
        //    this.txtAmount.Enabled = false;

        //}

        CustomerDocBE = CustomerDocService.getCustomerDocid(SelectedValue);

        ListItem li = new ListItem(CustomerDocBE.RESPONSIBLE_PERS_ID.ToString(), CustomerDocBE.RESPONSIBLE_PERS_ID.ToString());
        //(decimal.Parse(Val.ToString())).ToString("#,##0.00");
        if (CustomerDocBE.RETRO_ADJ_AMOUNT != null)
            this.txtAmount.Text = decimal.Parse(CustomerDocBE.RETRO_ADJ_AMOUNT.ToString()).ToString("#,##0");
        else
            this.txtAmount.Text = String.Empty;
        // this.txtAmount.Text = CustomerDocBE.RETRO_ADJ_AMOUNT.ToString();
        //this.txtAmount.Text = Math.Round(Convert.ToDecimal(CustomerDocBE.RETRO_ADJ_AMOUNT),0).ToString();
        this.txtComments.Text = CustomerDocBE.COMMENTS;
        if (CustomerDocBE.ENTRY_DATE != null)
            this.txtDateentered.Text = CustomerDocBE.ENTRY_DATE.Value.ToShortDateString();
        else
            this.txtDateentered.Text = String.Empty;
        if (CustomerDocBE.RECEVD_DATE != null)
            this.txtDtrecieved.Text = CustomerDocBE.RECEVD_DATE.Value.ToShortDateString();
        else
            this.txtDtrecieved.Text = String.Empty;

        if (CustomerDocBE.PROGM_EFF_DATE != null)
            this.txtPrgmeffdate.Text = CustomerDocBE.PROGM_EFF_DATE.Value.ToShortDateString();
        else
            this.txtPrgmeffdate.Text = String.Empty;
        if (CustomerDocBE.PROG_EXP_DATE != null)
            this.txtPrgmexpdate.Text = CustomerDocBE.PROG_EXP_DATE.Value.ToShortDateString();
        else
            this.txtPrgmexpdate.Text = String.Empty;
        ViewState["DateEntered"] = CustomerDocBE.ENTRY_DATE;
        //Code to add Message  "The QC Date should not be removed once the QC has been performed on this item."
        if (CustomerDocBE.QUALITY_CNTRL_DATE != null)
        {
            this.txtQcdate.Text = CustomerDocBE.QUALITY_CNTRL_DATE.Value.ToShortDateString();
            this.IsQcCompleted = true;
        }
        else
        {
            this.txtQcdate.Text = String.Empty;
            this.IsQcCompleted = false;
        }
        if (CustomerDocBE.VALUATION_DATE != null)
            this.txtValuationdate.Text = CustomerDocBE.VALUATION_DATE.Value.ToShortDateString();
        else
            this.txtValuationdate.Text = String.Empty;

        ddlFormsRecieved.DataBind();
        ddlFormsRecieved.SelectedIndex = -1;

        //        this.ddlFormsRecieved.Items.FindByValue(CustomerDocBE.FORM_ID.ToString()).Selected = true;
        int? formID;
        formID = CustomerDocBE.FORM_ID;
        AddInActiveLookupData(ref ddlFormsRecieved, formID);

        PersonBE PersBE = new PersonBE();
        PersonBS persBS = new PersonBS();
        PersBE = persBS.getPersonRow(CustomerDocBE.RESPONSIBLE_PERS_ID);

        //per.surname + ", " + per.forename
        this.ddlQCBy.DataBind();
        this.ddlQCBy.SelectedIndex = -1;
        this.ddlCfs.DataBind();
        this.ddlCfs.SelectedIndex = -1;
        if (!PersBE.IsNull())
        {
            String strfullname = PersBE.SURNAME + ", " + PersBE.FORENAME;
            ListItem liperson = new ListItem();
            liperson.Value = CustomerDocBE.RESPONSIBLE_PERS_ID.ToString();
            liperson.Text = strfullname;
            if (this.ddlQCBy.Items.Contains(liperson))
                //            this.ddlQCBy.Items.FindByValue(CustomerDocBE.RESPONSIBLE_PERS_ID.ToString()).Selected = true;
                AddInActiveContactData(ref ddlQCBy, CustomerDocBE.RESPONSIBLE_PERS_ID);
            if (this.ddlCfs.Items.Contains(liperson))
                //                this.ddlCfs.Items.FindByValue(CustomerDocBE.cash_flw_spl_id.ToString()).Selected = true;
                AddInActiveContactData(ref ddlCfs, CustomerDocBE.cash_flw_spl_id);
        }


        this.ddlBUoffice.DataBind();
        if (CustomerDocBE.bu_off_id != null)
            //this.ddlBUoffice.Items.FindByValue(CustomerDocBE.bu_off_id.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlBUoffice, CustomerDocBE.bu_off_id);
        this.chkQC.Checked = CustomerDocBE.TWENTY_PER_QC.Value;
        this.chklstTrackingissues.DataBind();
        for (int count = 0; count < chklstTrackingissues.Items.Count; count++)
        {
            if (chklstTrackingissues.Items[count].Selected == true)
                chklstTrackingissues.Items[count].Selected = false;
        }

        IList<CustomerDocumentIssuesBE> lstissues = CustomerdocissueService.getIssuedetails(CustomerDocBE.CUSTOMER_DOCUMENT_ID);
        string itemText = string.Empty; // veracode fix added by Murugappan 06262018
        for (int icount = 0; icount < lstissues.Count; icount++)
        {

            foreach (CustomerDocumentIssuesBE issues in lstissues)
            {
                //               this.chklstTrackingissues.Items.FindByValue(issues.tracking_issue_id.ToString()).Selected = true;
                if (this.chklstTrackingissues.Items.FindByValue(issues.tracking_issue_id.ToString()) != null)
                    this.chklstTrackingissues.Items.FindByValue(issues.tracking_issue_id.ToString()).Selected = true;
                else
                {
                    ListItem item = new ListItem();
                    itemText = (new BLAccess()).GetLookupName(issues.tracking_issue_id);// veracode fix modified by Murugappan 06262018
                    if (itemText.Trim().Length > 0)
                    {  
                        //06/23 for veracode fix 
                        //item.Text = Convert.ToString(itemText);                   // Veracode fix 07162018
                        item.Text = HttpUtility.HtmlEncode(Convert.ToString(itemText)) ; // veracode fix 07162018
                        //item.Value =  issues.tracking_issue_id.ToString();              
                        item.Value = Convert.ToString(issues.tracking_issue_id);
                        item.Selected = true;
                        this.chklstTrackingissues.Items.Add(item);
                    }
                }
            }


        }
    }


    /// <summary>
    /// This method is used to save the document details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {


        if (this.txtNonais.Text.Length > 0)
        {

            NonAisCustomerBS nonaiscustmrbs = new NonAisCustomerBS();
            // nonaiscustmrbs.AppTransactionWrapper = DocTransactionWrapper;
            NonaiscustmrBE = new NonAisCustomerBE();
            NonaiscustmrBE.fullname = this.txtNonais.Text;
            NonaiscustmrBE.Createddate = System.DateTime.Now;
            NonaiscustmrBE.Creteduserid = CurrentAISUser.PersonID;
            NonaiscustmrBE.Updateddate = System.DateTime.Now;
            NonaiscustmrBE.Updateduserid = CurrentAISUser.PersonID;
            bool saved = nonaiscustmrbs.save(NonaiscustmrBE);
            //DocTransactionWrapper.SubmitTransactionChanges();
            if (!saved)
            {
                ShowError("Non ais customer could not be saved");
                this.txtNonais.Text = "";
                this.pnlNonaissave.Visible = false;
                return;
            }
            else
            {
                this.txtNonais.Text = "";
                this.pnlNonaissave.Visible = false;
                return;
            }

        }
        if (this.IsQcCompleted == true)
        {
            if (this.txtQcdate.Text == string.Empty)
            {
                ShowError("The QC Date should not be removed once the QC has been performed on this item.");
                return;

            }
        }
        DateTime dtreceived = DateTime.Now, dtentered = DateTime.Now;
        if (this.txtDateentered.Text != string.Empty)
            dtentered = DateTime.Parse(this.txtDateentered.Text);
        if (this.txtDtrecieved.Text != string.Empty)
            dtreceived = DateTime.Parse(this.txtDtrecieved.Text);
        if (this.ddlFormsRecieved.SelectedItem.Text == "Maryland Retro Only")
        {
            if ((this.txtDateentered.Text != string.Empty) && (this.txtDtrecieved.Text != string.Empty))
            {
                if (dtentered < dtreceived)
                {
                    ShowError("Date entered cannot be less than Date recieved");
                    return;
                }
            }
        }
        else
        {
            if (this.txtDateentered.Text != string.Empty)
            {
                if (dtentered < System.DateTime.Today && this.txtQcdate.Text == string.Empty)
                {
                    if (ViewState["DateEntered"] == null || ViewState["DateEntered"].ToString() == "" || dtentered != DateTime.Parse(ViewState["DateEntered"].ToString()))
                    {
                        ShowError("Date entered cannot be less than current date");
                        return;
                    }
                }
            }
            if (this.txtDtrecieved.Text != string.Empty)
            {
                if (dtreceived > System.DateTime.Today)
                {
                    ShowError("Date received cannot be greater than current date");
                    return;
                }
            }
        }

        Boolean success, Custmrsuccess;
        ArrayList arrissues = new ArrayList();
        success = false;
        if (CheckNew == false)
        {
            CustomerDocumentBE CustomerDocBE = new CustomerDocumentBE();
            CustomerDocBE = CustomerDocService.getCustomerDocid(SelectedValue);

            //Concurrency Issue
            // CustomerDocumentBE CustmrDocsBEold = (lstDocuments.Where(o => o.CUSTOMER_DOCUMENT_ID.Equals(SelectedValue))).First();
            // bool con = ShowConcurrentConflict(Convert.ToDateTime(CustmrDocsBEold.UPDATED_DATE), Convert.ToDateTime(CustomerDocBE.UPDATED_DATE));
            //if (!con)
            //    return;
            //End

        }

        else
        {

            CustomerDocBE = new CustomerDocumentBE();
            CustomerDocBE.CREATED_USR_ID = CurrentAISUser.PersonID;
        }
        try
        {



            if (this.radLSI.Checked)
            //get the selected non ais customer id.
            {
                //CustomerDocBE.Non_Ais_id = NonaiscustmrBE.Nonaiscustmrid;
                DropDownList ddllsiAccnts = (DropDownList)ucNonaisacctlst.FindControl("ddlAccountlist");
                CustomerDocBE.Non_Ais_id = Convert.ToInt32(ddllsiAccnts.SelectedItem.Value);
            }

            if (this.radRetro.Checked)
            {
                DropDownList ddlAccnts = (DropDownList)this.ucAccountList.FindControl("ddlAccountlist");
                CustomerDocBE.CUSTMR_ID = Convert.ToInt32(ddlAccnts.SelectedItem.Value);
            }


            CustomerDocBE.RESPONSIBLE_PERS_ID = Convert.ToInt32(this.ddlQCBy.SelectedItem.Value);


            //if (this.ddlFormsRecieved.SelectedItem.Text == "Maryland Retro Only" || this.ddlFormsRecieved.SelectedItem.Text == "Par Plan Only")
            //{
            //    txtAmount.Enabled = true;
            //}
            //else { txtAmount.Enabled = false; }
            CustomerDocBE.FORM_ID = Convert.ToInt32(this.ddlFormsRecieved.SelectedItem.Value);
            if (Convert.ToInt32(this.ddlBUoffice.SelectedItem.Value) != 0)
                CustomerDocBE.bu_off_id = Convert.ToInt32(this.ddlBUoffice.SelectedItem.Value);
            else
                CustomerDocBE.bu_off_id = null;

            CustomerDocBE.cash_flw_spl_id = Convert.ToInt32(this.ddlCfs.SelectedItem.Value);

            string str = this.txtDtrecieved.Text;
            if (this.txtDtrecieved.Text != string.Empty)
                CustomerDocBE.RECEVD_DATE = DateTime.Parse(this.txtDtrecieved.Text);
            else
            {
                CustomerDocBE.RECEVD_DATE = null;
            }
            if (this.txtPrgmeffdate.Text != string.Empty)
                CustomerDocBE.PROGM_EFF_DATE = DateTime.Parse(this.txtPrgmeffdate.Text);
            else
            {
                CustomerDocBE.PROGM_EFF_DATE = null;
            }
            if (this.txtPrgmexpdate.Text != string.Empty)
                CustomerDocBE.PROG_EXP_DATE = DateTime.Parse(this.txtPrgmexpdate.Text);
            else
            {
                CustomerDocBE.PROG_EXP_DATE = null;
            }
            if (this.txtDateentered.Text != string.Empty)
                CustomerDocBE.ENTRY_DATE = DateTime.Parse(this.txtDateentered.Text);
            else
            {
                CustomerDocBE.ENTRY_DATE = null;
            }
            ViewState["DateEntered"] = txtDateentered.Text;
            if (this.txtQcdate.Text != string.Empty)
                CustomerDocBE.QUALITY_CNTRL_DATE = DateTime.Parse(this.txtQcdate.Text);
            else
            {
                CustomerDocBE.QUALITY_CNTRL_DATE = null;
            }
            if (this.txtValuationdate.Text != string.Empty)
                CustomerDocBE.VALUATION_DATE = DateTime.Parse(this.txtValuationdate.Text);
            else
            {
                CustomerDocBE.VALUATION_DATE = null;
            }
            if (this.txtAmount.Text != string.Empty)
                CustomerDocBE.RETRO_ADJ_AMOUNT = Convert.ToDecimal(this.txtAmount.Text);
            CustomerDocBE.TWENTY_PER_QC = this.chkQC.Checked;
            CustomerDocBE.COMMENTS = this.txtComments.Text;
            CustomerDocBE.UPDATED_DATE = DateTime.Now;

            CustomerDocBE.UPDATED_USR_ID = CurrentAISUser.PersonID;

            CustomerDocBE.ACTV_IND = true;
            //  This method is used to save the csutomerdocumene data.
            Custmrsuccess = CustomerDocService.save(CustomerDocBE);


            bool flag = DocTransactionWrapper.SubmitTransactionChanges();

            ShowConcurrentConflict(flag, DocTransactionWrapper.ErrorMessage);

            if (Custmrsuccess)
            {
                Custmrsuccess = CustomerdocissueService.deleteCustmrissues(CustomerDocBE.CUSTOMER_DOCUMENT_ID);
            }
            for (int count = 0; count < this.chklstTrackingissues.Items.Count; count++)
            {

                // Try cach should come here and transaction --24/10/2008.
                if (this.chklstTrackingissues.Items[count].Selected)
                {
                    CustomerDocumentIssuesBE custmrdocissues = new CustomerDocumentIssuesBE();
                    custmrdocissues.custmr_doc_id = CustomerDocBE.CUSTOMER_DOCUMENT_ID;
                    custmrdocissues.createduserid = CurrentAISUser.PersonID;
                    custmrdocissues.updateduserid = CurrentAISUser.PersonID;
                    custmrdocissues.tracking_issue_id = int.Parse(chklstTrackingissues.Items[count].Value);
                    custmrdocissues.cretateddate = System.DateTime.Now;
                    custmrdocissues.updateddate = System.DateTime.Now;
                    //  This method is used to save the csutomerdocumene data.
                    CustomerdocissueService.save(custmrdocissues);
                }
            }
            bool bsuccess = DocTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(bsuccess, DocTransactionWrapper.ErrorMessage);

            SelectedValue = CustomerDocBE.CUSTOMER_DOCUMENT_ID;
            this.btnSearch.Enabled = true;
            this.lstDocumentTracking.Enabled = false;
            this.lstDocumentTracking.Visible = true;
            this.lblDocuments.Visible = true;
            this.pnlDetails.Visible = true;
            this.lnkClose.Visible = true;
            SortDir = "";
            if (this.radRetro.Checked)
                BindDocuments(Convert.ToInt32(CustomerDocBE.CUSTMR_ID));
            if (this.radLSI.Checked)
                BindNonaisDocuments(Convert.ToInt32(CustomerDocBE.Non_Ais_id));

            this.divDocumenttracking.Visible = true;
            this.lstDocumentTracking.Visible = true;
            this.divHeading.Visible = true;
            Image imgSortByName = (Image)lstDocumentTracking.FindControl("imgSortByName");
            if (imgSortByName != null)
            {
                imgSortByName.Visible = false;
            }
            //Code to add Message  "The QC Date should not be removed once the QC has been performed on this item."
            if (CustomerDocBE.QUALITY_CNTRL_DATE != null)
            {
                this.IsQcCompleted = true;
            }
            else
            {
                this.IsQcCompleted = false;
            }
            //this.pnlDetails.Visible = false;

            CheckNew = false;

            DocumentID = CustomerDocBE.CUSTOMER_DOCUMENT_ID.ToString();
        }
        catch
        {
            DocTransactionWrapper.RollbackChanges();
            ShowError("The record cannot be saved");

        }

    }
    /// <summary>
    /// This method is used to the disable the selected record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        // This method returns the CustomerDocumentBE.
        CustomerDocBE = CustomerDocService.getCustomerDocid(SelectedValue);
        CustomerDocBE.ACTV_IND = false;
        bool Flag = CustomerDocService.save(CustomerDocBE);
        this.btnSearch.Enabled = true;

    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        int intCustomerid, intnonaiscustomer;

        if (this.radLSI.Checked)
        {

            DropDownList ddllsiAccnts = (DropDownList)ucNonaisacctlst.FindControl("ddlAccountlist");
            intnonaiscustomer = Convert.ToInt32(ddllsiAccnts.SelectedItem.Value);
            if (intnonaiscustomer == 0)
            {
                ShowError("Please select a NON-AIS account");
                this.txtNonais.Text = "";
                return;

            }
            else
            {
                this.pnlDetails.Visible = false;
                this.pnlNonaissave.Visible = false;
                this.divHeading.Visible = false;

                this.lstDocumentTracking.Enabled = true;
                this.lstDocumentTracking.Visible = true;
                this.lblDocuments.Visible = true;
                // this.pnlDetails.Visible = true;
                this.lnkClose.Visible = true;
                this.lblDocumentTracking.Visible = true;
                BindNonaisDocuments(intnonaiscustomer);
                lblDocuments.Visible = true;
                this.divDocumenttracking.Visible = true;
                this.divHeading.Visible = false;
                //this.pnlDetails.Visible = false;
                lstDocumentTracking.Visible = true;
                //this.pnlNonaissave.Visible = false;
                ViewState["customerid"] = intnonaiscustomer;
                SortDir = "";

                Image imgSortByName = (Image)lstDocumentTracking.FindControl("imgSortByName");
                if (imgSortByName != null)
                    imgSortByName.Visible = false;
            }

        }
        if (this.radRetro.Checked)
        {
            DropDownList ddlAccnts = (DropDownList)this.ucAccountList.FindControl("ddlAccountlist");
            intCustomerid = Convert.ToInt32(ddlAccnts.SelectedItem.Value);
            if (intCustomerid == 0)
            {
                ShowError("Please select the account");
                return;

            }
            else
            {
                this.lstDocumentTracking.Enabled = true;
                this.lstDocumentTracking.Visible = true;
                this.lblDocuments.Visible = true;
                this.pnlDetails.Visible = true;
                this.lnkClose.Visible = true;
                this.lblDocumentTracking.Visible = true;
                BindDocuments(intCustomerid);
                lblDocuments.Visible = true;
                this.divDocumenttracking.Visible = true;
                this.divHeading.Visible = false;
                this.pnlDetails.Visible = false;
                this.pnlNonaissave.Visible = false;
                lstDocumentTracking.Visible = true;
                ViewState["customerid"] = intCustomerid;
                SortDir = "";

                Image imglsiSortByName = (Image)lstDocumentTracking.FindControl("imgSortByName");
                if (imglsiSortByName != null)
                    imglsiSortByName.Visible = false;
            }
        }


    }
    /// <summary>
    /// This method is used to populate the documnts list view based on the account selected.
    /// </summary>
    /// <param name="intCustomerid"></param>
    public void BindDocuments(int intCustomerid)
    {
        // This method returns the customer docuuments list.
        lstDocuments = CustomerDocService.getcustomerdocuments(intCustomerid);
        if (SortDir == "ASC")
            lstDocumentTracking.DataSource = lstDocuments.OrderBy(o => o.FORM_ID);
        else if (SortDir == "DESC")
            lstDocumentTracking.DataSource = lstDocuments.OrderByDescending(o => o.FORM_ID);
        else

            lstDocumentTracking.DataSource = lstDocuments;
        this.lstDocumentTracking.DataBind();

    }
    /// <summary>
    /// This method is used to bind the non ais customer documents.
    /// </summary>
    /// <param name="intnonaiscustomer"></param>
    public void BindNonaisDocuments(int intnonaiscustomer)
    {
        // This method returns the customer docuuments list.

        //IList<CustomerDocumentBE> lstNonaisDocuments = CustomerDocService.getNonaiscustomerdocuments(intnonaiscustomer);
        lstDocuments = CustomerDocService.getNonaiscustomerdocuments(intnonaiscustomer);
        this.lstDocumentTracking.DataSource = null;
        if (SortDir == "ASC")
            lstDocumentTracking.DataSource = lstDocuments.OrderBy(o => o.FORM_ID);
        else if (SortDir == "DESC")
            lstDocumentTracking.DataSource = lstDocuments.OrderByDescending(o => o.FORM_ID);
        else

            lstDocumentTracking.DataSource = lstDocuments;
        this.lstDocumentTracking.DataBind();

    }


    /// <summary>
    /// This method is used to add  a new record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.IsQcCompleted = false;
        ViewState["DateEntered"] = null;
        DropDownList ddlAccnts = (DropDownList)this.ucAccountList.FindControl("ddlAccountlist");
        int intCustomerid = Convert.ToInt32(ddlAccnts.SelectedItem.Value);
        DropDownList ddlNonaislist = (DropDownList)this.ucNonaisacctlst.FindControl("ddlAccountlist");
        int intnonaiscustomer = Convert.ToInt32(ddlNonaislist.SelectedItem.Value);

        if (this.radLSI.Checked)
        {
            if (intnonaiscustomer == 0)
            {

                if ((this.txtNonais.Text.Trim().Length) == 0)
                {
                    ShowError("Please select a Non-Ais account / Enter a new NonAis account");
                    return;
                }

            }

        }
        if (this.radRetro.Checked)
        {

            if (intCustomerid == 0)
            {
                ShowError("Please select the account");
                return;

            }
        }

        if ((this.txtNonais.Text.Trim().Length) > 0)
        {
            this.pnlDetails.Visible = false;
            this.pnlNonaissave.Visible = true;
            this.divDocumenttracking.Visible = false;
            this.divHeading.Visible = false;
            this.lblDocuments.Visible = false;
        }
        else
        {
            this.lblDocuments.Visible = false;
            this.lblDocumentTracking.Visible = true;
            // this.divDocumenttracking.Visible = false;
            this.pnlNonaissave.Visible = true;
            CheckNew = true;
            this.pnlDetails.Visible = true;
            this.divHeading.Visible = true;

            DocumentID = String.Empty;

            ClearDetails();
            Defaultdetails();
        }
    }
    /// <summary>
    /// This method is used to clear the controls.
    /// </summary>
    public void ClearDetails()
    {
        this.txtPrgmexpdate.Text = string.Empty;
        this.txtQcdate.Text = string.Empty;
        this.txtPrgmeffdate.Text = string.Empty;
        this.txtValuationdate.Text = string.Empty;
        this.txtDtrecieved.Text = string.Empty;
        this.txtDateentered.Text = string.Empty;
        this.txtComments.Text = string.Empty;
        this.txtAmount.Text = string.Empty;
        this.ddlQCBy.SelectedIndex = -1;
        this.ddlFormsRecieved.SelectedIndex = -1;
        this.ddlCfs.SelectedIndex = -1;
        this.ddlBUoffice.SelectedIndex = -1;
        for (int count = 0; count < this.chklstTrackingissues.Items.Count; count++)
        {

            if (this.chklstTrackingissues.Items[count].Selected)
            {
                this.chklstTrackingissues.Items[count].Selected = false;
            }
        }

        this.chkQC.Checked = false;


    }
    /// <summary>
    /// This method is used to bind the Default details
    /// </summary>
    public void Defaultdetails()
    {
        // this.txtQcdate.Text = System.DateTime.Now.ToShortDateString();
        // this.txtDtrecieved.Text = System.DateTime.Now.ToShortDateString();
        // this.txtDateentered.Text = System.DateTime.Now.ToShortDateString();

    }

    protected void lnkClose_Click(Object Sender, EventArgs E)
    {

        pnlDetails.Visible = false;
        this.pnlNonaissave.Visible = false;
        this.divHeading.Visible = false;
        this.lstDocumentTracking.Enabled = true;

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (this.txtNonais.Text.Length > 0)
            this.txtNonais.Text = String.Empty;
        ClearDetails();
        // this.pnlDetails.Visible = false;
        this.btnSearch.Enabled = true;
        // this.pnlNonaissave.Visible = false;
        try
        {
            if (DocumentID.Length > 0)
                DisplayDetails(int.Parse(DocumentID), HidFormValue);
        }
        catch { }
    }

    protected void lstDocumentTracking_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        if (lstDocumentTracking.Items.Count > 0)
        {
            HtmlTableRow tr;
            //code for changing the previous selected row color to its original Color
            if (ViewState["SelectedIndex"] != null && ViewState["SelectedIndex"] != "")
            {
                int count = lstDocumentTracking.Items.Count;
                int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                if (lstDocumentTracking.Items.Count > index)
                {
                    if (count > 1)
                    {
                        tr = (HtmlTableRow)lstDocumentTracking.Items[index].FindControl("trItemTemplate");
                        tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
                    }
                    else
                    {
                        tr = (HtmlTableRow)lstDocumentTracking.Items[0].FindControl("trItemTemplate");
                        tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
                    }
                }
            }
            tr = (HtmlTableRow)lstDocumentTracking.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
            LinkButton lnk = (LinkButton)lstDocumentTracking.Items[e.NewSelectedIndex].FindControl("lnkSelect");
            ViewState["SelectedIndex"] = e.NewSelectedIndex;
            //code for changing the selected row style to highlighted
            tr.Attributes["class"] = "SelectedItemTemplate";
        }
    }

    protected void chklstTrackingissues_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlFormsRecieved_SelectedIndexChanged(object sender, EventArgs e)
    {
        /* if (this.ddlFormsRecieved.SelectedItem.Text == "Maryland Retro Only")
              this.compareDtentered.EnableClientScript = false;
          else
              this.compareDtentered.EnableClientScript = true; */
        //if (this.ddlFormsRecieved.SelectedItem.Text == "Maryland Retro Only" || this.ddlFormsRecieved.SelectedItem.Text == "Par Plan Only")
        //{
        //    txtAmount.Enabled = true;
        //}
        //else { txtAmount.Enabled = false; }
    }

    protected void radRetro_CheckedChanged(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ucAccountList.FindControl("ddlAccountlist");
        DropDownList ddlNonaislist = (DropDownList)this.ucNonaisacctlst.FindControl("ddlAccountlist");
        if (radRetro.Checked)
        {
            ddlAccnts.Enabled = true;
            ddlNonaislist.SelectedIndex = -1;
            ddlNonaislist.Enabled = false;
            this.txtNonais.Enabled = false;
            this.btnSearch.Enabled = true;
            this.divnonais.Disabled = true;
            this.divaactlst.Disabled = false;
            this.pnlNonaissave.Visible = false;
            this.lstDocumentTracking.Visible = false;
            this.lblDocumentTracking.Visible = false;
            this.lblDocuments.Visible = false;
            this.pnlDetails.Visible = false;
            this.lnkClose.Visible = false;
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                this.btnAdd.Enabled = true;
            else
                this.btnAdd.Enabled = false;

        }
    }

    protected void radLSI_CheckedChanged(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ucAccountList.FindControl("ddlAccountlist");
        DropDownList ddlNonaislist = (DropDownList)this.ucNonaisacctlst.FindControl("ddlAccountlist");
        if (radLSI.Checked)
        {
            ddlAccnts.SelectedIndex = -1;
            ddlAccnts.Enabled = false;
            ddlNonaislist.Enabled = true;
            this.txtNonais.Enabled = true;
            this.btnSearch.Enabled = true;
            this.divaactlst.Disabled = true;
            this.divnonais.Disabled = false;
            this.pnlNonaissave.Visible = false;
            this.lstDocumentTracking.Visible = false;
            this.lblDocumentTracking.Visible = false;
            this.lblDocuments.Visible = false;
            this.lnkClose.Visible = false;
            this.pnlDetails.Visible = false;

        }
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            this.btnAdd.Enabled = true;
        else
            this.btnAdd.Enabled = false;

    }
}