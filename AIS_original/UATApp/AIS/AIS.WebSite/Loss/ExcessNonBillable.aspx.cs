using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;
using System.Collections.Generic;


public partial class ExcessNonBillable : AISBasePage
{
    /// <summary>
    /// PageLoad Event code appears here
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Excess Non Billable";
        lblLOBText.Text = AISMasterEntities.ExcessLoss.LOB;
        lblPolicyNumberText.Text = AISMasterEntities.ExcessLoss.PolicyNumber;
        lblStateText.Text = AISMasterEntities.ExcessLoss.State;
        lblStartDateText.Text = AISMasterEntities.ExcessLoss.ProgramPeriod;
        if (AISMasterEntities.ExcessLoss.AdjStatus == "false")
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            btnCopy.Enabled = false;
        }
        //Applying conditions on btnSave and btnAdd when adjustment status is  Calc for Adjustment Details .
        if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
        {
            string strAdjstatus = AISMasterEntities.AdjustmentStatus;
            if (strAdjstatus == "CALC")
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                {
                    btnAdd.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            else
            {
                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }
        //Applying conditions on btnSave and btnAdd when adjustment status is  Calc for Upcoming Valuations.
        else if (((string)Session["Adjdtls"]) == "LossInfo")
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                btnAdd.Enabled = true;
                btnSave.Enabled = true;
            }
        }
        else
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
        }
        if (!IsPostBack)
        {
            // Function call to bind the EXC NON Billable Details
            BindListView();
            

        }

        //Checks Exiting Without Save
        CheckWithoutSave();
        txtAdditionalClaims.Enabled = false;
    }

    private void CheckWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtAdditionalClaims);
        list.Add(txtClaimantName);
        list.Add(txtClaimNumber);
        list.Add(TxtCovgTriggerDt);
        list.Add(txtLimit);
        list.Add(txtNonBilPaidExp);
        list.Add(txtNonBilPaidIdmnt);
        list.Add(txtNonBilResrvExp);
        list.Add(txtNonBilResrvIdmnt);
        list.Add(txtTotPaidExp);
        list.Add(txtTotPaidIdmnt);
        list.Add(txtTotResrvExp);
        list.Add(chkAddClaims);
        list.Add(txtTotResrvIdmnt);
        list.Add(CalendarCovgTriggerDt);
        list.Add(ddlClaimsStatus);
        list.Add(btnAdd);
        list.Add(btnCancel);
        list.Add(btnCopy);
        list.Add(btnSave);
        list.Add(lnkClose);
        ProcessExitFlag(list);
 
    }
    #endregion
    /// <summary>
    /// Function to bind the Listview with EXC NON Billable Details data
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    #region BindListview
    public void BindListView()
    {
        //Binding Listviw Data based on Armis Los Id,ComlAgmtId,Customer Id and PrgID.
        int armsid = AISMasterEntities.ExcessLoss.ARMISLossID;
        int comAmgtID = AISMasterEntities.ExcessLoss.ComlAgmtID;
        int custmrID = AISMasterEntities.AccountNumber;
        int prgID = Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString());
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
        ExcNonBilBEList = ExcNonBilBS.getExcNonBillableData(armsid, comAmgtID, custmrID, prgID);
        lsvExcessNonBillable.DataSource = ExcNonBilBEList;
        lsvExcessNonBillable.DataBind();
       
    }
    public void BindListViewHideDisLin()
    {
        //Binding Listviw Data based on Armis Los Id,ComlAgmtId,Customer Id and PrgID Based on Checkbox Status.
        bool Flag = chkHideDisLines.Checked;
        int armsid = AISMasterEntities.ExcessLoss.ARMISLossID;
        int comAmgtID = AISMasterEntities.ExcessLoss.ComlAgmtID;
        int custmrID = AISMasterEntities.AccountNumber;
        int prgID = Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString());
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
        ExcNonBilBEList = ExcNonBilBS.getExcNonBillableDataHideDisLines(armsid, comAmgtID, custmrID, prgID, Flag);
        lsvExcessNonBillable.DataSource = ExcNonBilBEList;
        lsvExcessNonBillable.DataBind();
        
    }
    #endregion


    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            //HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            //tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            //tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
        }
    }
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        //Function call to Enable or Disable the Record
        if (e.CommandName != "Sort")
        {
            if (e.CommandName == "DISABLE")
                DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true);
            else if (e.CommandName == "ENABLE")
                DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "ENABLE" ? true : false);
        }
        if (e.CommandName == "DETAILS")
        {
            ShowNonBillableLoss(Convert.ToInt32(e.CommandArgument));
        }

    }
    /// <summary>
    /// Function to be called when user Clicks on Details Link
    /// </summary>
    /// <param name="intARMISLOSSEXCID"></param>
    #region ShowNonBillableLoss Function
    private void ShowNonBillableLoss(int intARMISLOSSEXCID)
    {
        AISMasterEntities.ExcessLoss.ARMISLOssExcID = intARMISLOSSEXCID;
        Response.Redirect("LossesReport.aspx?mode=NonBillable");

    }
    #endregion
    /// <summary>
    /// function for record to make enable or disable
    /// </summary>
    /// <param name="personID"></param>
    /// <param name="Flag">True/False boolean value</param>
    /// <returns></returns>
    #region DisableRow
    protected void DisableRow(int armsExcID, bool Flag)
    {
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
        ExcessNonBillableBE ExcNonBilBE = ExcNonBilBS.getExcessNonBilRow(armsExcID);
        ExcNonBilBE.ACTV_IND = Flag;
        ExcNonBilBE.UPDATEDUSER = CurrentAISUser.PersonID;
        ExcNonBilBE.UPDATEDDATE = DateTime.Now;
        Flag = ExcNonBilBS.Update(ExcNonBilBE);
        ShowConcurrentConflict(Flag, ExcNonBilBE.ErrorMessage);
        bool FlagHid = chkHideDisLines.Checked;
        ExcNonBilBS.PerformLimiting(Convert.ToInt32(AISMasterEntities.AccountNumber), Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString()));
        if (!FlagHid)
            BindListView();
        else
            BindListViewHideDisLin();


    }
    #endregion
    // Invoke when Check box Hide Disable Lines gets checked and unchecked

    #region Hide Disable Lines
    protected void chkHideDisLines_CheckedChanged(object sender, EventArgs e)
    {
        bool Flag = chkHideDisLines.Checked;
        if (Flag)
        {
            BindListViewHideDisLin();

        }
        else
        {
            BindListView();

        }

    }
    #endregion
    // Invoke when Check box Add Claims gets checked and unchecked

    #region Add Claims
    //protected void chkAdditionalClaims_CheckedChanged(object sender, EventArgs e)
    //{
    //    bool Flag = chkAdditionalClaims.Checked;
    //    if (Flag)
    //    {
    //        txtAdditionalClaims.Enabled = true;
    //    }
    //    else
    //    {
    //        txtAdditionalClaims.Enabled = false;
    //        txtAdditionalClaims.Text = "";
            
    //    }

    //}
    #endregion

    /// <summary>
    /// Binds the Claims Status Dropdown 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void FillClaimsStatus()
    {
        //ddlClaimsStatus.Items.Clear();
        //ddlClaimsStatus.Items.Insert(0, "(Select)");
        //ddlClaimsStatus.Items.Insert(1, "Open");
        //ddlClaimsStatus.Items.Insert(2, "Closed");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lsvExcessNonBillable.Enabled = false;
        pnlDetails.Enabled = true;
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            HtmlTableRow tr = (HtmlTableRow)lsvExcessNonBillable.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry )
        btnSave.Enabled = true;
        ddlClaimsStatus.SelectedIndex = -1;
        CheckNew = true;
        //Function call to clear all the control values
        ClearFileds();
        //Function call to Bind the Claims DropDown List 
        //ddlClaimsStatus.DataBind();
       // FillClaimsStatus();
        pnlDetails.Visible = true;
        lnkClose.Visible = true;
        lblExcNonBilDetails.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();

        if (CheckNew == true)
        {
            //Inserting the records.
            ExcNonBilBE = new ExcessNonBillableBE();
            ExcNonBilBE.ARMIS_LOS_ID = AISMasterEntities.ExcessLoss.ARMISLossID;
            ExcNonBilBE.PREM_ADJ_PGM_ID = Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString());
            ExcNonBilBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            ExcNonBilBE.COML_AGMT_ID = AISMasterEntities.ExcessLoss.ComlAgmtID;
            ExcNonBilBE.CLAIM_NBR_TXT = txtClaimNumber.Text;
            ExcNonBilBE.CLAIMANT_NM = txtClaimantName.Text;
            ExcNonBilBE.COVG_TRIGGER_DATE = DateTime.Parse(TxtCovgTriggerDt.Text);
            ExcNonBilBE.PAID_IDNMTY_AMT = decimal.Parse(txtTotPaidIdmnt.Text.Replace(",", ""));
            ExcNonBilBE.PAID_EXPS_AMT = decimal.Parse(txtTotPaidExp.Text.Replace(",", ""));
            ExcNonBilBE.RESRV_IDNMTY_AMT = decimal.Parse(txtTotResrvIdmnt.Text.Replace(",", ""));
            ExcNonBilBE.RESRV_EXPS_AMT = decimal.Parse(txtTotResrvExp.Text.Replace(",", ""));

            ExcNonBilBE.NON_BILABL_PAID_IDNMTY_AMT = (txtNonBilPaidIdmnt.Text == "" ? 0 : decimal.Parse(txtNonBilPaidIdmnt.Text.Replace(",", "")));
            ExcNonBilBE.NON_BILABL_PAID_EXPS_AMT = (txtNonBilPaidExp.Text == "" ? 0 : decimal.Parse(txtNonBilPaidExp.Text.Replace(",", "")));
            ExcNonBilBE.NON_BILABL_RESRV_IDNMTY_AMT = (txtNonBilResrvIdmnt.Text == "" ? 0 : decimal.Parse(txtNonBilResrvIdmnt.Text.Replace(",", "")));
            ExcNonBilBE.NON_BILABL_RESRV_EXPS_AMT = (txtNonBilResrvExp.Text == "" ? 0 : decimal.Parse(txtNonBilResrvExp.Text.Replace(",", "")));
            //ExcNonBilBE.ADD_CLAIM_TXT = chkAdditionalClaims.Checked.ToString();
            ExcNonBilBE.ADD_CLAIM_TXT = txtAdditionalClaims.Text;
            ExcNonBilBE.LIMIT2_AMT = (txtLimit.Text == "" ? 0 : decimal.Parse(txtLimit.Text.Replace(",", "")));
            if (ddlClaimsStatus.SelectedValue == "(Select)")
            {
                ExcNonBilBE.CLAIM_STS_ID = null;
            }
            else
            {
                ExcNonBilBE.CLAIM_STS_ID = Convert.ToInt32(ddlClaimsStatus.SelectedValue);
            }
            ExcNonBilBE.SYS_GENRT_IND = false;
            ExcNonBilBE.ACTV_IND = true;
            ExcNonBilBE.ADDN_CLAIMS = chkAddClaims.Checked;     
            ExcNonBilBE.CREATEDATE = DateTime.Now;
            ExcNonBilBE.CREATEUSER = CurrentAISUser.PersonID;
            ExcNonBilBS.Update(ExcNonBilBE);
            Session["ArmisExcID"] = ExcNonBilBE.ARMIS_LOS_EXC_ID.ToString();
            Session["SysGen"] = "false";
            BindListView();
            CheckNew = false;
            //ClearFileds();
            //pnlDetails.Visible = false;
            //lnkClose.Visible = false;
            //lblExcNonBilDetails.Visible = false;
            //lsvExcessNonBillable.Enabled = true;
            ExcNonBilBS.PerformLimiting(Convert.ToInt32(AISMasterEntities.AccountNumber), Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString()));
        }
        else
        {
            //Updating the records.
            String strvalue = HttpContext.Current.Session["ArmisExcID"].ToString();
            ExcNonBilBE = ExcNonBilBS.getExcessNonBilRow(int.Parse(strvalue));
            //Concurrency Issue
            ExcessNonBillableBE ExcNonBilBEold = (ExcNonBilBEList.Where(o => o.ARMIS_LOS_EXC_ID.Equals(Convert.ToInt32(strvalue)))).First();
            bool con=ShowConcurrentConflict(Convert.ToDateTime(ExcNonBilBEold.UPDATEDDATE), Convert.ToDateTime(ExcNonBilBE.UPDATEDDATE));
            if (!con)
                return;
            //End
            // ExcNonBilBE.ARMIS_LOS_EXC_ID = Int32.Parse(strvalue);
            ExcNonBilBE.CLAIM_NBR_TXT = txtClaimNumber.Text;
            ExcNonBilBE.CLAIMANT_NM = txtClaimantName.Text;
            ExcNonBilBE.COVG_TRIGGER_DATE = DateTime.Parse(TxtCovgTriggerDt.Text);
            ExcNonBilBE.PAID_IDNMTY_AMT = decimal.Parse(txtTotPaidIdmnt.Text.Replace(",", ""));
            ExcNonBilBE.PAID_EXPS_AMT = decimal.Parse(txtTotPaidExp.Text.Replace(",", ""));
            ExcNonBilBE.RESRV_IDNMTY_AMT = decimal.Parse(txtTotResrvIdmnt.Text.Replace(",", ""));
            ExcNonBilBE.RESRV_EXPS_AMT = decimal.Parse(txtTotResrvExp.Text.Replace(",", ""));
            ExcNonBilBE.NON_BILABL_PAID_IDNMTY_AMT = (txtNonBilPaidIdmnt.Text == "" ? 0 : decimal.Parse(txtNonBilPaidIdmnt.Text.Replace(",", "")));
            ExcNonBilBE.NON_BILABL_PAID_EXPS_AMT = (txtNonBilPaidExp.Text == "" ? 0 : decimal.Parse(txtNonBilPaidExp.Text.Replace(",", "")));
            ExcNonBilBE.NON_BILABL_RESRV_IDNMTY_AMT = (txtNonBilResrvIdmnt.Text == "" ? 0 : decimal.Parse(txtNonBilResrvIdmnt.Text.Replace(",", "")));
            ExcNonBilBE.NON_BILABL_RESRV_EXPS_AMT = (txtNonBilResrvExp.Text == "" ? 0 : decimal.Parse(txtNonBilResrvExp.Text.Replace(",", "")));
            //ExcNonBilBE.ADD_CLAIM_TXT = chkAdditionalClaims.Checked.ToString();
            ExcNonBilBE.ADDN_CLAIMS = chkAddClaims.Checked;
            ExcNonBilBE.ADD_CLAIM_TXT = txtAdditionalClaims.Text;
            ExcNonBilBE.LIMIT2_AMT = (txtLimit.Text == "" ? 0 : decimal.Parse(txtLimit.Text.Replace(",", "")));
            if (ddlClaimsStatus.SelectedValue == "(Select)")
            {
                ExcNonBilBE.CLAIM_STS_ID = null;
            }
            else
            {
                ExcNonBilBE.CLAIM_STS_ID = Convert.ToInt32(ddlClaimsStatus.SelectedValue);
            }
            if (Session["SysGen"].ToString() == "True")
            {
                ExcNonBilBE.SYS_GENRT_IND = true;
            }
            else { ExcNonBilBE.SYS_GENRT_IND = false; }
            ExcNonBilBE.ACTV_IND = true;
            ExcNonBilBE.UPDATEDDATE = DateTime.Now;
            ExcNonBilBE.UPDATEDUSER = CurrentAISUser.PersonID;
            bool Flag=ExcNonBilBS.Update(ExcNonBilBE);
            //ShowConcurrentConflict(Flag, ExcNonBilBE.ErrorMessage);
            BindListView();
            //ClearFileds();
            //pnlDetails.Visible = false;
            //lnkClose.Visible = false;
            //lblExcNonBilDetails.Visible = false;
            //lsvExcessNonBillable.Enabled = true;
            ExcNonBilBS.PerformLimiting(Convert.ToInt32(AISMasterEntities.AccountNumber), Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString()));
        }
    }

    /// <summary>
    /// Clears all control values
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void ClearFileds()
    {
        txtLimit.Text = string.Empty;
        txtClaimantName.Text = string.Empty;
        txtClaimNumber.Text = string.Empty;
        txtNonBilPaidExp.Text = string.Empty;
        txtNonBilPaidIdmnt.Text = string.Empty;
        txtNonBilResrvExp.Text = string.Empty;
        txtNonBilResrvIdmnt.Text = string.Empty;
        txtTotPaidExp.Text = string.Empty;
        txtTotPaidIdmnt.Text = string.Empty;
        txtTotResrvExp.Text = string.Empty;
        txtTotResrvIdmnt.Text = string.Empty;
        ddlClaimsStatus.SelectedIndex = -1;
        //chkAdditionalClaims.Checked = false;
        chkAddClaims.Checked = false;
        txtAdditionalClaims.Text = string.Empty;
        TxtCovgTriggerDt.Text = string.Empty;
    }

    public void EnableFieds()
    {
        //Setting the controls to enable status.
        txtNonBilPaidExp.Enabled = true;
        txtNonBilPaidIdmnt.Enabled = true;
        txtNonBilResrvExp.Enabled = true;
        txtNonBilResrvIdmnt.Enabled = true;
        txtClaimantName.Enabled = true;
        txtClaimNumber.Enabled = true;
        TxtCovgTriggerDt.Enabled = true;
        txtLimit.Enabled = true;
        txtTotPaidExp.Enabled = true;
        txtTotPaidIdmnt.Enabled = true;
        txtTotResrvExp.Enabled = true;
        txtTotResrvIdmnt.Enabled = true;
        //chkAdditionalClaims.Enabled = true;
        chkAddClaims.Enabled = true;
        txtAdditionalClaims.Enabled = true;
    }

    protected void lsvExcessNonBillable__SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        ClearFileds();
        HtmlTableRow tr;
        //code for changing the previous selected row color to its original Color
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lsvExcessNonBillable.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        tr = (HtmlTableRow)lsvExcessNonBillable.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        LinkButton lnk = (LinkButton)lsvExcessNonBillable.Items[e.NewSelectedIndex].FindControl("lnkSelect");
        ViewState["SelectedIndex"] = e.NewSelectedIndex;
        //code for changing the selected row style to highlighted
        tr.Attributes["class"] = "SelectedItemTemplate";
        CheckNew = false;
        if (AISMasterEntities.ExcessLoss.AdjStatus == "false")
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            btnCopy.Enabled = false;
        }
        else
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry )
            {
                btnAdd.Enabled = true;
                btnSave.Enabled = true;
                btnCopy.Enabled = true;
            }
        }
        int intArmisExcID = Convert.ToInt32(lnk.CommandArgument);
        SelectedValue = intArmisExcID;
        //function call to display the selected Exc Non Billable Details
        HiddenField hid = (HiddenField)lsvExcessNonBillable.Items[e.NewSelectedIndex].FindControl("hidActive");
        HiddenField hidArmisId = (HiddenField)lsvExcessNonBillable.Items[e.NewSelectedIndex].FindControl("HidArmisExcID");
        Session["ArmisExcID"] = intArmisExcID;
        HiddenField hidsts = (HiddenField)lsvExcessNonBillable.Items[e.NewSelectedIndex].FindControl("hidStatus");
        Session["ClaimStatus"] = hidsts.Value;
        //Funtion to Display the details of the selected Exc Non Billable
        DisplayDetails(intArmisExcID);
        pnlDetails.Enabled = bool.Parse(hid.Value);
        pnlDetails.Visible = true;
        lnkClose.Visible = true;
        lblExcNonBilDetails.Visible = true;
        lsvExcessNonBillable.Enabled = false;
    }
    /// <summary>
    /// Protected Property for storing Selectedvalue of Internal contacts list
    /// </summary>
    /// <param name=""></param>
    /// <returns>integer Selected value</returns>
    protected int SelectedValue
    {
        get
        {
            return (hidindex.Value != null ? Convert.ToInt32(hidindex.Value) : -1);
        }
        set { hidindex.Value = value.ToString(); }
    }

    /// <summary>
    /// bool Function to check the record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNew
    {
        get { return (bool)ViewState["CheckNew"]; }
        set { ViewState["CheckNew"] = value; }
    }

    /// <summary>
    /// Function to display the selected Excess Loss details
    /// </summary>
    /// <param name="intArmisExcID">intArmisExcID ID of the selected record in the listview</param>
    /// <returns></returns>
    public void DisplayDetails(int intArmisExcID)
    {
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
        ExcNonBilBE = ExcNonBilBS.getExcessNonBilRow(intArmisExcID);
        txtClaimantName.Text = ExcNonBilBE.CLAIMANT_NM;
        txtClaimNumber.Text = ExcNonBilBE.CLAIM_NBR_TXT;
        txtClaimantName.Text = ExcNonBilBE.CLAIMANT_NM;
        TxtCovgTriggerDt.Text = ExcNonBilBE.COVG_TRIGGER_DATE.ToString();
        txtTotPaidIdmnt.Text = Convert.ToInt64(ExcNonBilBE.PAID_IDNMTY_AMT).ToString("#,##0");
        txtTotPaidExp.Text = Convert.ToInt64(ExcNonBilBE.PAID_EXPS_AMT).ToString("#,##0");
        txtTotResrvIdmnt.Text = Convert.ToInt64(ExcNonBilBE.RESRV_IDNMTY_AMT).ToString("#,##0");
        txtTotResrvExp.Text = Convert.ToInt64(ExcNonBilBE.RESRV_EXPS_AMT).ToString("#,##0");
        txtNonBilPaidIdmnt.Text = Convert.ToInt64(ExcNonBilBE.NON_BILABL_PAID_IDNMTY_AMT).ToString("#,##0");
        txtNonBilPaidExp.Text = Convert.ToInt64(ExcNonBilBE.NON_BILABL_PAID_EXPS_AMT).ToString("#,##0");
        txtNonBilResrvIdmnt.Text = Convert.ToInt64(ExcNonBilBE.NON_BILABL_RESRV_IDNMTY_AMT).ToString("#,##0");
        txtNonBilResrvExp.Text = Convert.ToInt64(ExcNonBilBE.NON_BILABL_RESRV_EXPS_AMT).ToString("#,##0");
       // chkAdditionalClaims.Checked = Convert.ToBoolean(ExcNonBilBE.ADD_CLAIM_TXT);
        chkAddClaims.Checked = Convert.ToBoolean(ExcNonBilBE.ADDN_CLAIMS);
        txtAdditionalClaims.Text = ExcNonBilBE.ADD_CLAIM_TXT;
        txtLimit.Text = Convert.ToInt64(ExcNonBilBE.LIMIT2_AMT).ToString("#,##0");
        string strvalue = Session["ClaimStatus"].ToString();
        //Function call to Bind the Claim DropDown List
        
        ddlClaimsStatus.DataBind();
        //FillClaimsStatus();

        if (strvalue != "")
        {
//        ddlClaimsStatus.Items.FindByText(strvalue).Selected = true;
            AddInActiveLookupDataByText(ref ddlClaimsStatus, strvalue);
        }
        
        bool sysGen = ExcNonBilBE.SYS_GENRT_IND.Value;
        Session["SysGen"] = sysGen.ToString();
        if (sysGen)
        {
            txtClaimantName.Enabled = false;
            txtClaimNumber.Enabled = false;
            TxtCovgTriggerDt.Enabled = false;
            txtLimit.Enabled = false;
            txtTotPaidExp.Enabled = false;
            txtTotPaidIdmnt.Enabled = false;
            txtTotResrvExp.Enabled = false;
            txtTotResrvIdmnt.Enabled = false;
            //chkAdditionalClaims.Enabled = false;
            chkAddClaims.Enabled = false;
            txtAdditionalClaims.Enabled = false;
        }
        else
        {
            EnableFieds();

        }
        if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
        {
            string strAdjstatus = AISMasterEntities.AdjustmentStatus;
            if (strAdjstatus == "CALC")
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                {
                    btnAdd.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            else
            {
                btnAdd.Enabled = false;
                btnSave.Enabled = false;
            }
        }
        else if (((string)Session["Adjdtls"]) == "LossInfo")
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                btnAdd.Enabled = true;
                btnSave.Enabled = true;
            }
        }
        else
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
        }

    }
    /// <summary>
    /// a property for ExcessNonBillable Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>ExcessNonBillableBE</returns>
    private ExcessNonBillableBE ExcNonBilBE
    {
        get { return (ExcessNonBillableBE)Session["ExcNonBilBE"]; }
        set { Session["ExcNonBilBE"] = value; }
    }
    private IList<ExcessNonBillableBE> ExcNonBilBEList
    {
        get
        {
            if (Session["ExcNonBilBEList"] == null)
                Session["ExcNonBilBEList"] = new List<ExcessNonBillableBE>();
            return (IList<ExcessNonBillableBE>)Session["ExcNonBilBEList"];
        }
        set { Session["ExcNonBilBEList"] = value; }
    }
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        CheckNew = true;
        EnableFieds();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CheckNew == true)
        {
            ClearFileds();
        }
        else
        {
            DisplayDetails(SelectedValue);
        }
    }



    #region SortBy Property
    private string SortBy
    {
        get { return (string)Session["SORTBY"]; }
        set { Session["SORTBY"] = value; }
    }
    #endregion

    #region SortDir Property
    private string SortDir
    {
        get { return (string)Session["SORTDIR"]; }
        set { Session["SORTDIR"] = value; }
    }
    #endregion

    #region Listview Sorting
    protected void lsvExcessNonBillable_Sorting(object sender, ListViewSortEventArgs e)
    {
        Image imgSortByCovgDt = (Image)lsvExcessNonBillable.FindControl("imgSortByCovgDt");
        Image imgSortByALAE = (Image)lsvExcessNonBillable.FindControl("imgSortByALAE");
        Image img = new Image();
        switch (e.SortExpression)
        {
            case "COVG_TRIGGER_DATE":
                SortBy = "COVG_TRIGGER_DATE";
                imgSortByCovgDt.Visible = true;
                imgSortByALAE.Visible = false;
                img = imgSortByCovgDt;
                break;

            case "ALAE_TYP":
                SortBy = "ALAE_TYP";
                imgSortByCovgDt.Visible = false;
                imgSortByALAE.Visible = true;
                img = imgSortByALAE;
                break;
        }
        if (img.ToolTip == "Ascending")
        {
            img.ToolTip = "Descending";
            img.ImageUrl = "~/images/descending.gif";
            SortDir = "DESC";
        }
        else
        {
            img.ToolTip = "Ascending";
            img.ImageUrl = "~/images/Ascending.gif";
            SortDir = "ASC";
        }
        BindExcInformation();
    }


    private void BindExcInformation()
    {
        int armsid = AISMasterEntities.ExcessLoss.ARMISLossID;
        int comAmgtID = AISMasterEntities.ExcessLoss.ComlAgmtID;
        int custmrID = AISMasterEntities.AccountNumber;
        int prgID = Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString());
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
        ExcNonBilBEList = ExcNonBilBS.getExcNonBillableData(armsid, comAmgtID, custmrID, prgID);
        lsvExcessNonBillable.DataSource = GetSortedExcData();
        lsvExcessNonBillable.DataBind();
    }

    private IList<ExcessNonBillableBE> GetSortedExcData()
    {

        switch (SortBy)
        {
            case "COVG_TRIGGER_DATE":
                if (SortDir == "ASC")
                    ExcNonBilBEList = (ExcNonBilBEList.OrderBy(o => o.COVG_TRIGGER_DATE)).ToList();
                else if (SortDir == "DESC")
                    ExcNonBilBEList = (ExcNonBilBEList.OrderByDescending(o => o.COVG_TRIGGER_DATE)).ToList();

                break;

            case "ALAE_TYP":
                if (SortDir == "ASC")
                    ExcNonBilBEList = (ExcNonBilBEList.OrderBy(o => o.ALAE_TYP)).ToList();
                else if (SortDir == "DESC")
                    ExcNonBilBEList = (ExcNonBilBEList.OrderByDescending(o => o.ALAE_TYP)).ToList();

                break;
        }
        return ExcNonBilBEList;


    }
    #endregion

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        
        //On clicking close link its hiding the panel and Changing the colour of the selected record.
        lsvExcessNonBillable.Enabled = true;
        pnlDetails.Visible = false;
        lblExcNonBilDetails.Visible = false;
        lnkClose.Visible = false;
        HtmlTableRow tr;
        //code for changing the previous selected row color to its original Color
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lsvExcessNonBillable.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        SelectedValue = -1;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        //On clicking Back button its redirecting back to Loss Info screen.
        Response.Redirect("~/Loss/LossInfo.aspx");
    }

}

