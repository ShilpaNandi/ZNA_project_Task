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
using ZurichNA.AIS.DAL.LINQ;


public partial class TPAorManual_TPAManualPosting : AISBasePage
{
    System.Data.Common.DbTransaction trans = null;
    AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
    private TPAManualPostingsBS TPAbs;
    private TPAManualPostingsDetailBS TPSdtlbs;
    protected AISBusinessTransaction TPATransactionWrapper
    {
        get
        {
            if ((AISBusinessTransaction)Session["TPATransaction"] == null)
                Session["TPATransaction"] = new AISBusinessTransaction();
            return (AISBusinessTransaction)Session["TPATransaction"];
        }
        set
        {
            Session["TPATransaction"] = value;
        }
    }
    /// <summary>
    /// a property for TPAManualPostingsBS Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Coml_Agmt_AuDtBS</returns>
    private TPAManualPostingsBS TPAService
    {
        get
        {
            if (TPAbs == null)
            {
                TPAbs = new TPAManualPostingsBS();
                //TPAManualPostingsBS.AppTransactionWrapper = TPATransactionWrapper;
            }
            return TPAbs;
        }
    }
    /// <summary>
    /// a property for TPAManualPostingsDetailBS Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Sub_Audt_PremBS</returns>
    private TPAManualPostingsDetailBS TPAdtlService
    {
        get
        {
            if (TPSdtlbs == null)
            {
                TPSdtlbs = new TPAManualPostingsDetailBS();
                //TPSdtlbs.AppTransactionWrapper = TPATransactionWrapper;
            }
            return TPSdtlbs;
        }
    }
    /// <summary>
    /// a property for TPAManualPostingsBE Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Coml_Agmt_AuDtBE</returns>
    private TPAManualPostingsBE TPAPostBE
    {
        get { return (TPAManualPostingsBE)Session["TPAMANUALPOSTINGSBE"]; }
        set { Session["TPAMANUALPOSTINGSBE"] = value; }
    }
    private TPAManualPostingsBE TPAPostBEOld
    {
        get { return (TPAManualPostingsBE)Session["TPAPostBEOld"]; }
        set { Session["TPAPostBEOld"] = value; }
    }
    /// <summary>
    /// a property for TPAManualPostingsDetailBE Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>TPAManualPostingsDetailBE</returns>
    private TPAManualPostingsDetailBE TPAPostDtlBE
    {
        get { return (TPAManualPostingsDetailBE)Session["TPAMANUALPOSTINGSDETAILBE"]; }
        set { Session["TPAMANUALPOSTINGSDETAILBE"] = value; }
    }
    private IList<TPAManualPostingsDetailBE> TPAPostDtlBEList
    {
        get { return (IList<TPAManualPostingsDetailBE>)Session["TPAMANUALPOSTINGSDETAILBELIST"]; }
        set { Session["TPAMANUALPOSTINGSDETAILBELIST"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {



        if (!Page.IsPostBack)
        {
            this.Master.Page.Title = "TPA Manual Posting";
            TPAPostBE = new TPAManualPostingsBE();
            TPAPostDtlBE = new TPAManualPostingsDetailBE();
            AccountBE acct = (new AccountBS()).getAccount(AISMasterEntities.AccountNumber);
            if (acct.TPA_FUNDED_IND != true)
            {
                txtTPAFund.Text = "YES";
            }
            /// Function to bind the TPA/Manual Invoice information
            if (Request.QueryString.Count > 0)
            {
                ViewState["TPAID"] = Request.QueryString["TPAID"].ToString();
            }
            BindTPAList();

        }
        //Checks Exiting without Save
        CheckWithoutSave();
    }

    private void CheckWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtAmount);
        list.Add(txtComments);
        list.Add(txtDueDate);
        list.Add(txtEndDate);
        list.Add(txtInvoiceDate);
        list.Add(txtInvoiceNbr);
        list.Add(txtPolicyYears);
        list.Add(txtValuationDate);
        list.Add(calDuedate);
        list.Add(calEndDate);
        list.Add(calInvoiceDate);
        list.Add(calValDate);
        list.Add(ddlBillingCycle);
        list.Add(ddlBuOffice);
        list.Add(ddlInvoiceType);
        list.Add(ddlSourceofLoss);
        list.Add(ddlTPAName);
        list.Add(btnFinalize);
        list.Add(btnSave);
        ProcessExitFlag(list);
    }


    /// <summary>
    /// Function to bind the TPA/Manual Invoice information
    /// </summary>
    public void BindTPAList()
    {
        ddlTPAName.DataSource = (new BrokerBS()).GetBrokerData().Where(br => br.CONTACT_TYPE_ID == 237);
        ddlTPAName.DataTextField = "FULL_NAME";
        ddlTPAName.DataValueField = "EXTRNL_ORG_ID";
        ddlTPAName.DataBind();
        ListItem li = new ListItem("(Select)", "0");
        ddlTPAName.Items.Insert(0, li);

        if (ViewState["TPAID"] != null)
        {
            TPAPostBE = TPAService.getTPAPostRow(Convert.ToInt32(ViewState["TPAID"].ToString()));
            TPAPostBEOld = TPAPostBE;
            if (TPAPostBE.FinalizedIndicator == true || TPAPostBE.CancelIndicator == true)
            {
                btnSave.Enabled = false;
                btnFinalize.Enabled = false;
                lstTPADtl.Enabled = false;
            }
        }
        else
        {
            TPAPostBE = TPAService.getTPASubsequent(AISMasterEntities.AccountNumber);
            TPAPostBEOld = TPAPostBE;
        }
        TPAManualPostingsBE TPASubsequent = (new TPAManualPostingsBS()).getTPASubsequent(AISMasterEntities.AccountNumber);
        if (TPAPostBE.CustomerID == 0 || (ViewState["TPAID"] == null && TPASubsequent.CustomerID == 0))
        {
            txtInvoiceDate.Text = DateTime.Now.ToShortDateString();
            AccountBE acctBE = (new AccountBS()).getAccount(AISMasterEntities.AccountNumber);
            txtValuationDate.Text = acctBE.THIRD_PARTY_ADMIN_FUNDED_DATE == null ? "" : acctBE.THIRD_PARTY_ADMIN_FUNDED_DATE.Value.ToShortDateString();
            btnFinalize.Enabled = false;
            return;
        }
        else
        {
            if (TPAPostBE.FinalizedIndicator == true || TPAPostBE.CancelIndicator == true)
            {
                btnSave.Enabled = false;
                btnFinalize.Enabled = false;
                lstTPADtl.Enabled = false;
            }
        }
        ViewState["TPAINVOICEID"] = TPAPostBE.ThirdPartyAdminManualInvoiceID;
        txtInvoiceNbr.Text = TPAPostBE.InvoiceNumber;
        if (TPAPostBE.InvoiceDate != null)
        {
            txtInvoiceDate.Text = DateTime.Parse(TPAPostBE.InvoiceDate.ToString()).ToShortDateString();
        }
        if (TPAPostBE.ThirdPartyAdminInvoiceTypID != null)
        {
            ddlInvoiceType.DataBind();
            //            ddlInvoiceType.Items.FindByValue(TPAPostBE.ThirdPartyAdminInvoiceTypID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlInvoiceType, TPAPostBE.ThirdPartyAdminInvoiceTypID.Value);
        }
        //if (txtTPAFund.Text == "YES" && ddlInvoiceType.SelectedItem.Text == "MANUAL")
        //{
        //    compddlTPAName.Enabled = false;
        //}
        //else
        //{
        //    compddlTPAName.Enabled = false;
        //}
        if (TPAPostBE.ValuationDate != null)
        {
            txtValuationDate.Text = DateTime.Parse(TPAPostBE.ValuationDate.ToString()).ToShortDateString();
        }
        if (TPAPostBE.EndDate != null)
        {
            txtEndDate.Text = DateTime.Parse(TPAPostBE.EndDate.ToString()).ToShortDateString();
        }
        if (TPAPostBE.ThirdPartyAdminLossSrcID != null)
        {
            ddlSourceofLoss.DataBind();
            //            ddlSourceofLoss.Items.FindByValue(TPAPostBE.ThirdPartyAdminLossSrcID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlSourceofLoss, TPAPostBE.ThirdPartyAdminLossSrcID.Value);
        }
        if (TPAPostBE.BusinessUnitOfficeID != null)
        {
            ddlBuOffice.DataBind();
            //ddlBuOffice.Items.FindByValue(TPAPostBE.BusinessUnitOfficeID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlBuOffice, TPAPostBE.BusinessUnitOfficeID);
        }
        if (TPAPostBE.ThirdPartyAdminID != null)
        {
            //            ddlTPAName.Items.FindByValue(TPAPostBE.ThirdPartyAdminID.ToString()).Selected = true;
            AddInActiveExternalOrgData(ref ddlTPAName, TPAPostBE.ThirdPartyAdminID.Value);
        }
        if (TPAPostBE.InoiceAmt != null)
        {
            txtAmount.Text = String.Format("{0:N}", double.Parse(TPAPostBE.InoiceAmt.ToString()));

        }
        if (TPAPostBE.PolicyYearNumber != null)
        {
            txtPolicyYears.Text = TPAPostBE.PolicyYearNumber.ToString();
        }
        if (TPAPostBE.BillingCycleID != null)
        {
            ddlBillingCycle.DataBind();
            //            ddlBillingCycle.Items.FindByValue(TPAPostBE.BillingCycleID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlBillingCycle, TPAPostBE.BillingCycleID.Value);
        }
        txtDueDate.Text = TPAPostBE.DueDate.ToString();
        if (TPAPostBE.EndDate != null)
            txtEndDate.Text = DateTime.Parse(TPAPostBE.EndDate.ToString()).ToShortDateString();
        txtComments.Text = TPAPostBE.CommentText;
        if (TPAPostBE.FinalizedIndicator == true)
        {
            btnFinalize.Enabled = false;
            btnSave.Enabled = false;
        }
        // Function to bind the TPADetails
        BindTPADetails();


    }
    /// <summary>
    /// Function to bind the TPADetails
    /// </summary>
    private void BindTPADetails()
    {
        if (ViewState["TPAID"] != null)
            TPAPostDtlBEList = TPAdtlService.getTPAPostDltsList(Convert.ToInt32(ViewState["TPAID"].ToString()));
        else
            TPAPostDtlBEList = TPAdtlService.getTPAPostDltsList(Convert.ToInt32(ViewState["TPAINVOICEID"]));

        lstTPADtl.DataSource = TPAPostDtlBEList;
        lstTPADtl.DataBind();
        lblDetails.Visible = true;

        if (lstTPADtl.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddl = (DropDownList)lstTPADtl.InsertItem.FindControl("ddlTransaction");
            PostingTransactionTypeBS PostTrnsBS = new PostingTransactionTypeBS();
            ddl.DataValueField = "POST_TRANS_TYP_ID";
            ddl.DataTextField = "TRANS_TXT";
            ddl.DataSource = PostTrnsBS.getTPAList();
            ddl.DataBind();
            ListItem li = new ListItem("Select", "0");
            ddl.Items.Insert(0, li);
            LinkButton lnkSave = (LinkButton)lstTPADtl.InsertItem.FindControl("lnkSave");
            Label lblMain = (Label)lstTPADtl.InsertItem.FindControl("lblMain");
            Label lblSub = (Label)lstTPADtl.InsertItem.FindControl("lblSub");
            Label lblCompany = (Label)lstTPADtl.InsertItem.FindControl("lblCompany");
            HiddenField hidIndicator = (HiddenField)lstTPADtl.InsertItem.FindControl("hidIndicator");
            RequiredFieldValidator reqPolSym = (RequiredFieldValidator)lstTPADtl.InsertItem.FindControl("RequiredPolicySymbol");
            RequiredFieldValidator reqPolNbr = (RequiredFieldValidator)lstTPADtl.InsertItem.FindControl("requiredPolicyNumber");
            RequiredFieldValidator reqPolMod = (RequiredFieldValidator)lstTPADtl.InsertItem.FindControl("requiredPolicyModulus");
            lnkSave.Attributes.Add("onclick", "javascript:EnableDisable('" + reqPolSym.ClientID + "','" + reqPolNbr.ClientID + "','" + reqPolMod.ClientID + "','" + hidIndicator.ClientID + "')");
        }
        if (TPAPostBE.FinalizedIndicator != true && TPAPostBE.CancelIndicator != true)
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                btnFinalize.Enabled = true;
                btnSave.Enabled = true;
            }
        }
    }
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {

    }
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            // Function to Save the TPADetails
            SaveTPADetails(e.Item);
        }
    }
    /// <summary>
    /// Function to Save the TPADetails
    /// </summary>
    /// <param name="e"></param>
    private void SaveTPADetails(ListViewItem e)
    {
        TPAPostDtlBE = new TPAManualPostingsDetailBE();
        TPAPostDtlBE.ThirdPartyAdminManualInvoiceID = Convert.ToInt32(ViewState["TPAINVOICEID"]);
        TPAPostDtlBE.AriesMainNbr = ((Label)e.FindControl("lblMain")).Text;
        TPAPostDtlBE.AriesSubNbr = ((Label)e.FindControl("lblSub")).Text;
        TPAPostDtlBE.CompanyCode = ((Label)e.FindControl("lblCompany")).Text;
        if (((TextBox)e.FindControl("txtEffectiveDate")).Text != "")
            TPAPostDtlBE.EffectiveDate = DateTime.Parse(((TextBox)e.FindControl("txtEffectiveDate")).Text);
        if (((TextBox)e.FindControl("txtExpiryDate")).Text != "")
            TPAPostDtlBE.ExpiryDate = DateTime.Parse(((TextBox)e.FindControl("txtExpiryDate")).Text);
        if (((TextBox)e.FindControl("txtAmount")).Text != "")
            TPAPostDtlBE.ThirdPartyAdminAmt = Convert.ToDecimal(((TextBox)e.FindControl("txtAmount")).Text.Replace(",", ""));
        TPAPostDtlBE.PolicySymbolText = ((TextBox)e.FindControl("txtpolSymbol")).Text;
        if (((TextBox)e.FindControl("txtpolNumber")).Text.Length != 0)
            TPAPostDtlBE.PolicyNbrText = (((TextBox)e.FindControl("txtpolNumber")).Text.Length == 8 ? ((TextBox)e.FindControl("txtpolNumber")).Text : "0" + ((TextBox)e.FindControl("txtpolNumber")).Text);
        else
            TPAPostDtlBE.PolicyNbrText = "";
        TPAPostDtlBE.PolicyModText = ((TextBox)e.FindControl("txtpolmodules")).Text;
        TPAPostDtlBE.CustomerID = AISMasterEntities.AccountNumber;
        DropDownList ddl = (DropDownList)e.FindControl("ddlTransaction");
        if (ddl.SelectedIndex != 0)
        {
            TPAPostDtlBE.PostingTrnsTypID = Convert.ToInt32(ddl.SelectedValue);
        }
        TPAPostDtlBE.CreatedDate = DateTime.Now;
        TPAPostDtlBE.CreatedUserID = CurrentAISUser.PersonID;
        bool Flag = TPAdtlService.Update(TPAPostDtlBE);
        if (Flag)
            // Function to bind the TPADetails
            BindTPADetails();
    }
    protected void EditList(Object sender, ListViewEditEventArgs e)
    {
        lstTPADtl.EditIndex = e.NewEditIndex;
        // Function to bind the TPADetails
        BindTPADetails();
        LinkButton lnkUpdate = (LinkButton)lstTPADtl.Items[e.NewEditIndex].FindControl("lnkUpdate");
        Label lblMain = (Label)lstTPADtl.Items[e.NewEditIndex].FindControl("lblMain");
        Label lblSub = (Label)lstTPADtl.Items[e.NewEditIndex].FindControl("lblSub");
        Label lblCompany = (Label)lstTPADtl.Items[e.NewEditIndex].FindControl("lblCompany");
        HiddenField hidIndicator = (HiddenField)lstTPADtl.Items[e.NewEditIndex].FindControl("hidIndicator");
        RequiredFieldValidator reqPolSym = (RequiredFieldValidator)lstTPADtl.Items[e.NewEditIndex].FindControl("RequiredPolicySymbol");
        RequiredFieldValidator reqPolNbr = (RequiredFieldValidator)lstTPADtl.Items[e.NewEditIndex].FindControl("requiredPolicyNumber");
        RequiredFieldValidator reqPolMod = (RequiredFieldValidator)lstTPADtl.Items[e.NewEditIndex].FindControl("requiredPolicyModulus");
        DropDownList ddl = (DropDownList)lstTPADtl.Items[e.NewEditIndex].FindControl("ddlTransaction");
        PostingTransactionTypeBS PostTrnsBS = new PostingTransactionTypeBS();
        ddl.DataValueField = "POST_TRANS_TYP_ID";
        ddl.DataTextField = "TRANS_TXT";
        ddl.DataSource = PostTrnsBS.getTPAList();
        ddl.DataBind();
        ListItem li = new ListItem("Select", "0");
        ddl.Items.Insert(0, li);
        HiddenField hidval = (HiddenField)lstTPADtl.Items[e.NewEditIndex].FindControl("hidTransID");
        HiddenField hidTransText = (HiddenField)lstTPADtl.Items[e.NewEditIndex].FindControl("hidTransText");

        if (hidval.Value != "")
        {
            //li = new ListItem(hidTransText.Value, hidval.Value);
            //if (ddl.Items.Contains(li))
            //{
            //ddl.Items.FindByValue(hidval.Value.ToString()).Selected = true;
            AddInActivePostTransData(ref ddl, int.Parse(hidval.Value));
            PostingTransactionTypeBE PostTrnsBE = (new PostingTransactionTypeBS()).LoadData(int.Parse(hidval.Value.ToString()));
            hidIndicator.Value = PostTrnsBE.POL_REQR_IND == null ? "False" : PostTrnsBE.POL_REQR_IND.ToString();
            //((TextBox)lstTPADtl.Items[e.NewEditIndex].FindControl("txtpolSymbol")).Enabled = bool.Parse(hidIndicator.Value);
            //((TextBox)lstTPADtl.Items[e.NewEditIndex].FindControl("txtpolNumber")).Enabled = bool.Parse(hidIndicator.Value);
            //((TextBox)lstTPADtl.Items[e.NewEditIndex].FindControl("txtpolmodules")).Enabled = bool.Parse(hidIndicator.Value);

            //}
        }
        lnkUpdate.Attributes.Add("onclick", "javascript:EnableDisable('" + reqPolSym.ClientID + "','" + reqPolNbr.ClientID + "','" + reqPolMod.ClientID + "','" + hidIndicator.ClientID + "')");
        ((LinkButton)lstTPADtl.InsertItem.FindControl("lnkSave")).Enabled = false;
        btnSave.Enabled = false;
        btnFinalize.Enabled = false;
    }
    protected void CancelList(Object sender, ListViewCancelEventArgs e)
    {
        lstTPADtl.EditIndex = -1;
        // Function to bind the TPADetails
        BindTPADetails();
    }
    protected void UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        ListViewItem myItem = lstTPADtl.Items[e.ItemIndex];
        int TPAdtlID = Convert.ToInt32(((LinkButton)myItem.FindControl("lnkUpdate")).CommandArgument);
        TPAPostDtlBE = TPAdtlService.getTPAPostDtlRow(TPAdtlID);
        // Concurrency Code
        TPAManualPostingsDetailBE TPAdtlOld = (TPAPostDtlBEList.Where(o => o.ThirdPartyAdminManualInvoiceDtlID.Equals(TPAdtlID))).First();
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(TPAdtlOld.UpdatedDate), Convert.ToDateTime(TPAPostDtlBE.UpdatedDate));
        if (!Concurrency)
            return;
        TPAPostDtlBE.ThirdPartyAdminManualInvoiceID = Convert.ToInt32(ViewState["TPAINVOICEID"]);
        if (((Label)myItem.FindControl("lblMain")).Text != "")
        {
            TPAPostDtlBE.AriesMainNbr = ((Label)myItem.FindControl("lblMain")).Text;
        }
        if (((Label)myItem.FindControl("lblSub")).Text != "")
        {
            TPAPostDtlBE.AriesSubNbr = ((Label)myItem.FindControl("lblSub")).Text;
        }
        TPAPostDtlBE.CompanyCode = ((Label)myItem.FindControl("lblCompany")).Text;
        TPAPostDtlBE.PolicySymbolText = ((TextBox)myItem.FindControl("txtpolSymbol")).Text;
        if (((TextBox)myItem.FindControl("txtpolNumber")).Text.Length != 0)
            TPAPostDtlBE.PolicyNbrText = (((TextBox)myItem.FindControl("txtpolNumber")).Text.Length == 8 ? ((TextBox)myItem.FindControl("txtpolNumber")).Text : "0" + ((TextBox)myItem.FindControl("txtpolNumber")).Text);
        else
            TPAPostDtlBE.PolicyNbrText = "";
        TPAPostDtlBE.PolicyModText = ((TextBox)myItem.FindControl("txtpolmodules")).Text;
        if (((TextBox)myItem.FindControl("txtEffectiveDate")).Text != "")
        {
            TPAPostDtlBE.EffectiveDate = DateTime.Parse(((TextBox)myItem.FindControl("txtEffectiveDate")).Text);
        }
        if (((TextBox)myItem.FindControl("txtExpiryDate")).Text != "")
        {
            TPAPostDtlBE.ExpiryDate = DateTime.Parse(((TextBox)myItem.FindControl("txtExpiryDate")).Text);
        }
        if (((TextBox)myItem.FindControl("txtAmount")).Text != "")
        {
            TPAPostDtlBE.ThirdPartyAdminAmt = Convert.ToDecimal(((TextBox)myItem.FindControl("txtAmount")).Text.Replace(",", ""));
        }
        else
            TPAPostDtlBE.ThirdPartyAdminAmt = null;
        TPAPostDtlBE.CustomerID = AISMasterEntities.AccountNumber;
        DropDownList ddl = (DropDownList)myItem.FindControl("ddlTransaction");
        if (ddl.SelectedIndex != 0)
        {
            TPAPostDtlBE.PostingTrnsTypID = Convert.ToInt32(ddl.SelectedValue);
        }
        TPAPostDtlBE.UpdatedDate = DateTime.Now;
        TPAPostDtlBE.UpdatedUserID = 1;
        bool Flag = TPAdtlService.Update(TPAPostDtlBE);
        ShowConcurrentConflict(Flag, TPAPostDtlBE.ErrorMessage);
        if (Flag)
            lstTPADtl.EditIndex = -1;
        // Function to bind the TPADetails
        BindTPADetails();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (ViewState["TPAINVOICEID"] == null)
        {
            TPAPostBE = TPAService.getTPASubsequent(AISMasterEntities.AccountNumber);
            if (TPAPostBE.CustomerID > 0)
            {
                ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                return ;
            }
            TPAPostBE = new TPAManualPostingsBE();
            TPAPostBE.CreatedDate = DateTime.Now;
            TPAPostBE.CreatedUserID = CurrentAISUser.PersonID;
            TPAPostBE.Active = true;
        }
        else
        {
            TPAPostBE = TPAService.getTPAPostRow(Convert.ToInt32(ViewState["TPAINVOICEID"]));
            // Concurrency Code
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(TPAPostBEOld.UpdatedDate), Convert.ToDateTime(TPAPostBE.UpdatedDate));
            if (!Concurrency)
                return;

            TPAPostBE.UpdatedDate = DateTime.Now;
            TPAPostBE.UpdatedUserID = CurrentAISUser.PersonID;
        }
        TPAPostBE = AssignValues(TPAPostBE);
        if (ViewState["TPAID"] != null)
        {
            // TPAPostBE.RelatedInvoiceID = Convert.ToInt32(ViewState["TPAID"].ToString());
            //TPAPostBE.FinalizedIndicator = true;
        }
        // Code for save the TPAEnty
        bool Flag = TPAService.Update(TPAPostBE);
        if (Flag)
        {
            TPAPostBEOld = TPAPostBE;
            if (ViewState["TPAID"] != null)
            {
                //Response.Redirect("TPAManualPosting.aspx");
                ViewState["TPAID"] = null;
                //BindTPADetails();
                BindTPAList();
            }
            else
            {
                ViewState["TPAINVOICEID"] = TPAPostBE.ThirdPartyAdminManualInvoiceID;
                BindTPADetails();
            }
            modalSave.Show();

        }
    }
    /// <summary>
    /// To assign page values TPAManual Postings BE for Saving
    /// </summary>
    /// <param name="TPAPostBE"></param>
    /// <returns></returns>
    public TPAManualPostingsBE AssignValues(TPAManualPostingsBE TPAPostBE)
    {

        TPAPostBE.CustomerID = AISMasterEntities.AccountNumber;
        if (ddlTPAName.SelectedIndex != 0)
        {
            TPAPostBE.ThirdPartyAdminID = Convert.ToInt32(ddlTPAName.SelectedItem.Value);
        }
        else
            TPAPostBE.ThirdPartyAdminID = null;
        if (ddlInvoiceType.SelectedIndex != 0)
            TPAPostBE.ThirdPartyAdminInvoiceTypID = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);
        else
            TPAPostBE.ThirdPartyAdminInvoiceTypID = null;
        if (ddlBillingCycle.SelectedIndex != 0)
            TPAPostBE.BillingCycleID = Convert.ToInt32(ddlBillingCycle.SelectedItem.Value);
        else
            TPAPostBE.BillingCycleID = null;
        if (ddlSourceofLoss.SelectedIndex != 0)
            TPAPostBE.ThirdPartyAdminLossSrcID = Convert.ToInt32(ddlSourceofLoss.SelectedItem.Value);
        else
            TPAPostBE.ThirdPartyAdminLossSrcID = null;
        TPAPostBE.InvoiceNumber = txtInvoiceNbr.Text;
        if (txtInvoiceDate.Text != "")
            TPAPostBE.InvoiceDate = DateTime.Parse(txtInvoiceDate.Text);
        if (txtDueDate.Text != "")
            TPAPostBE.DueDate = DateTime.Parse(txtDueDate.Text);
        if (txtEndDate.Text != "")
            TPAPostBE.EndDate = DateTime.Parse(txtEndDate.Text);
        if (txtValuationDate.Text != "")
            TPAPostBE.ValuationDate = DateTime.Parse(txtValuationDate.Text);
        if (txtAmount.Text != "")
            //TPAPostBE.InoiceAmt = Decimal.Parse(txtAmount.Text.Replace(",", ""));
            TPAPostBE.InoiceAmt = Decimal.Parse(txtAmount.Text);
        else
            TPAPostBE.InoiceAmt = null;
        if (txtPolicyYears.Text != "")
            TPAPostBE.PolicyYearNumber = Convert.ToInt32(txtPolicyYears.Text);
        TPAPostBE.CommentText = txtComments.Text;
        if (ddlBuOffice.SelectedIndex != 0)
            TPAPostBE.BusinessUnitOfficeID = Convert.ToInt32(ddlBuOffice.SelectedItem.Value);
        else
            TPAPostBE.BusinessUnitOfficeID = null;

        return TPAPostBE;
    }
    protected void ddlInsertTransaction(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)lstTPADtl.InsertItem.FindControl("ddlTransaction");
        Label lblMain = (Label)lstTPADtl.InsertItem.FindControl("lblMain");
        Label lblSub = (Label)lstTPADtl.InsertItem.FindControl("lblSub");
        Label lblCompany = (Label)lstTPADtl.InsertItem.FindControl("lblCompany");
        HiddenField hidIndicator = (HiddenField)lstTPADtl.InsertItem.FindControl("hidIndicator");
        if (ddl.SelectedValue != "0")
        {
            int intPKID = Convert.ToInt32(ddl.SelectedValue);
            PostingTransactionTypeBE PostTrnsBE = (new PostingTransactionTypeBS()).LoadData(intPKID);
            lblMain.Text = PostTrnsBE.MAIN_NBR;
            lblSub.Text = PostTrnsBE.SUB_NBR;
            lblCompany.Text = PostTrnsBE.COMP_TXT;
            hidIndicator.Value = PostTrnsBE.POL_REQR_IND == null ? "False" : PostTrnsBE.POL_REQR_IND.ToString();
            //((TextBox)lstTPADtl.InsertItem.FindControl("txtpolSymbol")).Enabled = bool.Parse(hidIndicator.Value);
            //((TextBox)lstTPADtl.InsertItem.FindControl("txtpolNumber")).Enabled = bool.Parse(hidIndicator.Value);
            //((TextBox)lstTPADtl.InsertItem.FindControl("txtpolmodules")).Enabled = bool.Parse(hidIndicator.Value);
        }
        else
        {
            lblMain.Text = "";
            lblSub.Text = "";
            lblCompany.Text = "";
            hidIndicator.Value = "";
            //((TextBox)lstTPADtl.InsertItem.FindControl("txtpolSymbol")).Enabled = false;
            //((TextBox)lstTPADtl.InsertItem.FindControl("txtpolNumber")).Enabled = false;
            //((TextBox)lstTPADtl.InsertItem.FindControl("txtpolmodules")).Enabled = false;
        }

    }
    protected void ddlEditTransaction(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("ddlTransaction");
        Label lblMain = (Label)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("lblMain");
        Label lblSub = (Label)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("lblSub");
        Label lblCompany = (Label)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("lblCompany");
        HiddenField hidIndicator = (HiddenField)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("hidIndicator");
        if (ddl.SelectedValue != "0")
        {
            int intPKID = Convert.ToInt32(ddl.SelectedValue);
            PostingTransactionTypeBE PostTrnsBE = (new PostingTransactionTypeBS()).LoadData(intPKID);
            lblMain.Text = PostTrnsBE.MAIN_NBR;
            lblSub.Text = PostTrnsBE.SUB_NBR;
            lblCompany.Text = PostTrnsBE.COMP_TXT;
            hidIndicator.Value = PostTrnsBE.POL_REQR_IND == null ? "False" : PostTrnsBE.POL_REQR_IND.ToString();
            //((TextBox)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("txtpolSymbol")).Enabled = bool.Parse(hidIndicator.Value);
            //((TextBox)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("txtpolNumber")).Enabled = bool.Parse(hidIndicator.Value);
            //((TextBox)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("txtpolmodules")).Enabled = bool.Parse(hidIndicator.Value);
        }
        else
        {
            lblMain.Text = "";
            lblSub.Text = "";
            lblCompany.Text = "";
            hidIndicator.Value = "";
            // ((TextBox)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("txtpolSymbol")).Enabled = false;
            // ((TextBox)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("txtpolNumber")).Enabled = false;
            // ((TextBox)lstTPADtl.Items[lstTPADtl.EditIndex].FindControl("txtpolmodules")).Enabled = false;
        }
    }
    protected void btnFinalize_Click(object sender, EventArgs e)
    {
        if (AISMasterEntities.Bpnumber.Trim() == "")
        {
            ShowError("BP Number must be enterd using AccountInfo web page");
        }
        else
        {
            ClearError();
            string strError = string.Empty;
            bool Flag = false;
            Label lblAmount;
            decimal Amount = 0;
            for (int i = 0; i < lstTPADtl.Items.Count; i++)
            {
                lblAmount = (Label)lstTPADtl.Items[i].FindControl("lblAmount");
                if (lblAmount.Text.Trim() != "")
                {
                    Amount += decimal.Parse(lblAmount.Text);
                }
            }
            //commented as part of 10464 bug Fix.(if the invoice amounts are not matching then display validation error though details are not added)
            //if (lstTPADtl.Items.Count > 0)
            //{
            decimal ActualAmt = txtAmount.Text == "" ? 0 : decimal.Parse(txtAmount.Text);
            if (ActualAmt != Amount)
            {
                strError += "The sum of amounts for all entries for the Account should be equal to entered Invoice Amount";
                Flag = true;
            }
            //}
            if (Flag)
            {
                ShowError(strError);
                return;
            }
            modalFinalze.Show();
        }
    }
    protected void btnFinalizePopup_Click(object sender, EventArgs e)
    {
        modalFinalze.Hide();
        if (AISMasterEntities.Bpnumber == "")
        {
            ShowError("BP Number must be enterd in AccountInfo web page");
        }
        else
        {
            try
            {
                ClearError();
                string strError = string.Empty;
                //TPAPostBE = TPAService.getTPAPostRow(TPAPostBE.ThirdPartyAdminManualInvoiceID);
                objDC.Connection.Open();
                trans = objDC.Connection.BeginTransaction();
                objDC.Transaction = trans;
                var TPAPost = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVCs where cdd.thrd_pty_admin_mnl_invc_id == TPAPostBE.ThirdPartyAdminManualInvoiceID select cdd).First();
                //TPAPostBE = AssignValues(TPAPostBE);
                // Concurrency Code
                bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(TPAPostBEOld.UpdatedDate), Convert.ToDateTime(TPAPost.updt_dt));
                if (!Concurrency)
                {
                    trans.Rollback();
                    if (objDC.Connection.State == ConnectionState.Open)
                        objDC.Connection.Close();
                    return;
                }
                TPAPost.custmr_id = AISMasterEntities.AccountNumber;
                if (ddlTPAName.SelectedIndex != 0)
                {
                    TPAPost.thrd_pty_admin_id = Convert.ToInt32(ddlTPAName.SelectedItem.Value);
                }
                else
                    TPAPost.thrd_pty_admin_id = null;
                if (ddlInvoiceType.SelectedIndex != 0)
                    TPAPost.thrd_pty_admin_invc_typ_id = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);
                else
                    TPAPost.thrd_pty_admin_invc_typ_id = null;
                if (ddlBillingCycle.SelectedIndex != 0)
                    TPAPost.bil_cycl_id = Convert.ToInt32(ddlBillingCycle.SelectedItem.Value);
                else
                    TPAPost.bil_cycl_id = null;
                if (ddlSourceofLoss.SelectedIndex != 0)
                    TPAPost.thrd_pty_admin_los_src_id = Convert.ToInt32(ddlSourceofLoss.SelectedItem.Value);
                else
                    TPAPost.thrd_pty_admin_los_src_id = null;
                TPAPost.invc_nbr_txt = txtInvoiceNbr.Text;
                if (txtInvoiceDate.Text != "")
                    TPAPost.invc_dt = DateTime.Parse(txtInvoiceDate.Text);
                if (txtDueDate.Text != "")
                    TPAPost.due_dt = DateTime.Parse(txtDueDate.Text);
                if (txtEndDate.Text != "")
                    TPAPost.end_dt = DateTime.Parse(txtEndDate.Text);
                if (txtValuationDate.Text != "")
                    TPAPost.valn_dt = DateTime.Parse(txtValuationDate.Text);
                if (txtAmount.Text != "")
                    TPAPost.invc_amt = Decimal.Parse(txtAmount.Text);
                if (txtPolicyYears.Text != "")
                    TPAPost.pol_yy_nbr = Convert.ToInt32(txtPolicyYears.Text);
                TPAPost.cmmnt_txt = txtComments.Text;
                if (ddlBuOffice.SelectedIndex != 0)
                    TPAPost.bsn_unt_ofc_id = Convert.ToInt32(ddlBuOffice.SelectedItem.Value);
                else
                    TPAPost.bsn_unt_ofc_id = null;

                //--------------------------
                DateTime dtValDt;
                if (ddlBillingCycle.SelectedItem.Text.ToUpper() == "MONTHLY")
                {
                    dtValDt = DateTime.Parse(TPAPost.valn_dt.ToString()).AddMonths(1);
                }
                else if (ddlBillingCycle.SelectedItem.Text.ToUpper() == "ANNUAL")
                {
                    dtValDt = DateTime.Parse(TPAPost.valn_dt.ToString()).AddYears(1);
                }
                else if (ddlBillingCycle.SelectedItem.Text.ToUpper() == "QUARTERLY")
                {
                    dtValDt = DateTime.Parse(TPAPost.valn_dt.ToString()).AddMonths(3);
                }
                else if (ddlBillingCycle.SelectedItem.Text.ToUpper() == "SEMI-ANNUAL")
                {
                    dtValDt = DateTime.Parse(TPAPost.valn_dt.ToString()).AddMonths(6);
                }
                else
                {
                    dtValDt = DateTime.Parse(txtValuationDate.Text);
                }

                TPAPost.fnl_ind = true;
                if (TPAPost.related_invoice_id == null)
                {
                    TPAPost.invc_nbr_txt = GenenrateInvoiceNumber(TPAPost.thrd_pty_admin_mnl_invc_id.ToString().Length, TPAPost.thrd_pty_admin_mnl_invc_id.ToString(), "RMI01");
                }
                else
                {
                    //---------------------------
                    TPAManualPostingsBE TPAMasterBE = (new TPAManualPostingsBS()).getTPAPostRow(int.Parse(TPAPost.related_invoice_id.ToString()));
                    if (TPAMasterBE.InvoiceNumber.StartsWith("RMI"))
                    {
                        TPAPost.invc_nbr_txt = GenenrateInvoiceNumber(TPAPostBE.RelatedInvoiceID.ToString().Length, TPAPostBE.RelatedInvoiceID.ToString(), "RMR01");
                    }
                    else
                    {
                        string strnumber = TPAMasterBE.InvoiceNumber;
                        if (strnumber != "")
                        {
                            int num = int.Parse(strnumber.Substring(3, 2)) + 1;
                            string str = num > 9 ? num.ToString() : "0" + num.ToString();
                            if (strnumber.Length > 5)
                                TPAPost.invc_nbr_txt = "RMR" + str + strnumber.Remove(0, 5);
                        }
                    }
                    //-----------------------
                }
                TPAPost.updt_user_id = CurrentAISUser.PersonID;
                TPAPost.updt_dt = DateTime.Now;

                //Save = TPAService.Update(TPAPostBE);
                //--------------------------------------------------------------------
                try
                {
                    var TPAPostdel = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVCs where cdd.custmr_id == AISMasterEntities.AccountNumber && cdd.fnl_ind == null && cdd.can_ind == null && cdd.thrd_pty_admin_mnl_invc_id != TPAPost.thrd_pty_admin_mnl_invc_id select cdd).First();
                    for (int i = 0; i < TPAPostdel.THRD_PTY_ADMIN_MNL_INVC_DTLs.Count(); i++)
                    {
                        objDC.THRD_PTY_ADMIN_MNL_INVC_DTLs.DeleteOnSubmit(TPAPostdel.THRD_PTY_ADMIN_MNL_INVC_DTLs[i]);
                    }
                    objDC.SubmitChanges();
                    objDC.THRD_PTY_ADMIN_MNL_INVCs.DeleteOnSubmit(TPAPostdel);
                    objDC.SubmitChanges();
                }
                catch (Exception)
                {

                }
                //--------------------------------------------------------------------
                /// New Valuation date is less than End Date - make a copy
                bool copy = true;
                if (txtEndDate.Text != "")
                {
                    if (dtValDt > DateTime.Parse(txtEndDate.Text))
                    {
                        copy = false;
                    }
                }
                if (copy)
                {
                    THRD_PTY_ADMIN_MNL_INVC TPANew = new THRD_PTY_ADMIN_MNL_INVC()
                    {
                        valn_dt = dtValDt,
                        crte_dt = DateTime.Now,
                        crte_user_id = CurrentAISUser.PersonID,
                        actv_ind = true,
                        custmr_id = TPAPost.custmr_id,
                        thrd_pty_admin_id = TPAPost.thrd_pty_admin_id,
                        thrd_pty_admin_invc_typ_id = TPAPost.thrd_pty_admin_invc_typ_id,
                        bil_cycl_id = TPAPost.bil_cycl_id,
                        thrd_pty_admin_los_src_id = TPAPost.thrd_pty_admin_los_src_id,
                        end_dt = TPAPost.end_dt,
                        invc_amt = null,
                        pol_yy_nbr = TPAPost.pol_yy_nbr,
                        bsn_unt_ofc_id = TPAPost.bsn_unt_ofc_id,
                    };
                    objDC.THRD_PTY_ADMIN_MNL_INVCs.InsertOnSubmit(TPANew);
                    objDC.SubmitChanges();
                    //Flag = (new TPAManualPostingsBS()).Update(TPAPostNewBE);
                    //IList<TPAManualPostingsDetailBE> TPAdtlBE = (new TPAManualPostingsDetailBS()).getTPAPostDltsList(Convert.ToInt32(ViewState["TPAINVOICEID"]));
                    var TPAdtlBE = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVC_DTLs where cdd.thrd_pty_admin_mnl_invc_id == Convert.ToInt32(ViewState["TPAINVOICEID"]) select cdd).ToList();
                    for (int i = 0; i < TPAdtlBE.Count; i++)
                    {
                        THRD_PTY_ADMIN_MNL_INVC_DTL TPAnewDtl = new THRD_PTY_ADMIN_MNL_INVC_DTL()
                        {
                            thrd_pty_admin_mnl_invc_id = TPANew.thrd_pty_admin_mnl_invc_id,
                            custmr_id = TPANew.custmr_id,
                            thrd_pty_admin_amt = null,
                            post_trns_typ_id = TPAdtlBE[i].post_trns_typ_id,
                            crte_dt = DateTime.Now,
                            crte_user_id = CurrentAISUser.PersonID,
                            updt_dt = null,
                            updt_user_id = null,
                            aries_main_nbr_txt = TPAdtlBE[i].aries_main_nbr_txt,
                            aries_sub_nbr_txt = TPAdtlBE[i].aries_sub_nbr_txt,
                            comp_cd_txt = TPAdtlBE[i].comp_cd_txt,
                            eff_dt = TPAdtlBE[i].eff_dt,
                            expi_dt = TPAdtlBE[i].expi_dt,
                            pol_nbr_txt = TPAdtlBE[i].pol_nbr_txt,
                            pol_modulus_txt = TPAdtlBE[i].pol_modulus_txt,
                            pol_sym_txt = TPAdtlBE[i].pol_sym_txt,
                        };

                        objDC.THRD_PTY_ADMIN_MNL_INVC_DTLs.InsertOnSubmit(TPAnewDtl);
                        objDC.SubmitChanges();
                        //Flag = (new TPAManualPostingsDetailBS()).Update(TPAdtlBE[i]);
                    }
                }
                else
                {
                    ShowError("The new Valuation Date is greater than End Date. So copy is not performed. ");
                }
                objDC.SubmitChanges();

                //calling Aries PRocedure for FInalize
                //bool Flag = (new TPAManualPostingsBS()).TPATransmittalToARiES(TPAPost.thrd_pty_admin_mnl_invc_id, AISMasterEntities.AccountNumber, 1);
                string ErroMsg;
                ErroMsg = string.Empty;
                objDC.ModAIS_TPATransmittalToARiES(TPAPost.thrd_pty_admin_mnl_invc_id, AISMasterEntities.AccountNumber, ref ErroMsg, 1);
                if (ErroMsg == "")
                {
                    trans.Commit();
                    txtInvoiceNbr.Text = TPAPost.invc_nbr_txt;
                    btnFinalize.Enabled = false;
                    btnSave.Enabled = false;
                    lstTPADtl.Enabled = false;
                }
                else
                {
                    trans.Rollback();
                    (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Finalize error", ErroMsg, AISMasterEntities.AccountNumber, CurrentAISUser.PersonID);
                    ShowError("TPA/Manual Invoice could not be finalized due to an error. Please check the error log for additional details");
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
                (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Finalize error", ee.Message, AISMasterEntities.AccountNumber, CurrentAISUser.PersonID);
                ShowError("TPA/Manual Invoice could not be finalized due to an error. Please check the error log for additional details");
            }
            finally
            {
                if (objDC.Connection.State == ConnectionState.Open)
                    objDC.Connection.Close();
            }


        }
    }
    /// <summary>
    /// Function to generate Invoice Number
    /// </summary>
    /// <param name="i"></param>
    /// <param name="strID"></param>
    /// <returns>Invoice Number</returns>
    private string GenenrateInvoiceNumber(int i, string strID, string str)
    {
        //string str = "RMI01";
        if (i == 1)
        {
            str += "000000000" + strID;
        }
        else if (i == 2)
        {
            str += "00000000" + strID;
        }
        else if (i == 3)
        {
            str += "0000000" + strID;
        }
        else if (i == 4)
        {
            str += "000000" + strID;
        }
        else if (i == 5)
        {
            str += "00000" + strID;
        }
        else if (i == 6)
        {
            str += "0000" + strID;
        }
        else if (i == 7)
        {
            str += "000" + strID;
        }
        return str;
    }
    protected void lstTPADtl_Sorting(object sender, ListViewSortEventArgs e)
    {
        Image imgPolicy = (Image)lstTPADtl.FindControl("imgPolicy");
        Image imgEffctDate = (Image)lstTPADtl.FindControl("imgEffctDate");
        Image imgTransaction = (Image)lstTPADtl.FindControl("imgTransaction");

        imgPolicy.Visible = false;
        imgEffctDate.Visible = false;
        imgTransaction.Visible = false;
        IList<TPAManualPostingsDetailBE> TPADtlBE = TPAdtlService.getTPAPostDltsList(Convert.ToInt32(ViewState["TPAINVOICEID"]));
        switch (e.SortExpression)
        {
            case "PostTransactionText":
                imgTransaction.Visible = true;
                if (imgTransaction.ToolTip == "Ascending")
                {
                    TPADtlBE = (TPADtlBE.OrderBy(o => o.PostTransactionText)).ToList();
                    imgTransaction.ToolTip = "Descending";
                    imgTransaction.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    TPADtlBE = (TPADtlBE.OrderByDescending(o => o.PostTransactionText)).ToList();
                    imgTransaction.ToolTip = "Ascending";
                    imgTransaction.ImageUrl = "~/images/ascending.gif";
                }
                break;

            case "PolicyNbrText":
                imgPolicy.Visible = true;
                if (imgPolicy.ToolTip == "Ascending")
                {
                    TPADtlBE = (TPADtlBE.OrderBy(o => o.PolicyNbrText)).ToList();
                    imgPolicy.ToolTip = "Descending";
                    imgPolicy.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    TPADtlBE = (TPADtlBE.OrderByDescending(o => o.PolicyNbrText)).ToList();
                    imgPolicy.ToolTip = "Ascending";
                    imgPolicy.ImageUrl = "~/images/ascending.gif";
                }
                break;
            case "EffectiveDate":
                imgEffctDate.Visible = true;
                if (imgEffctDate.ToolTip == "Ascending")
                {
                    TPADtlBE = (TPADtlBE.OrderBy(o => o.EffectiveDate)).ToList();
                    imgEffctDate.ToolTip = "Descending";
                    imgEffctDate.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    TPADtlBE = (TPADtlBE.OrderByDescending(o => o.EffectiveDate)).ToList();
                    imgEffctDate.ToolTip = "Ascending";
                    imgEffctDate.ImageUrl = "~/images/ascending.gif";
                }
                break;
        }
        lstTPADtl.DataSource = TPADtlBE;
        lstTPADtl.DataBind();
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
        {
            DropDownList ddl = (DropDownList)lstTPADtl.InsertItem.FindControl("ddlTransaction");
            PostingTransactionTypeBS PostTrnsBS = new PostingTransactionTypeBS();
            ddl.DataValueField = "POST_TRANS_TYP_ID";
            ddl.DataTextField = "TRANS_TXT";
            ddl.DataSource = PostTrnsBS.getList();
            ddl.DataBind();
            ListItem li = new ListItem("Select", "0");
            ddl.Items.Insert(0, li);
        }
    }
}
