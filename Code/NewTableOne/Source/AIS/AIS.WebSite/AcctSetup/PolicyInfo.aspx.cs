/*-----	Page:	PolicyInfo
-----
-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	This page is used to create/Modify policies for a particular program period.
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 2/16/09 (AS part of Retro Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               06/16/09	Zakir Hussain
-----				Code modified in Policy Info to add a new Text Amount NY Premium discount.

*/
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
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using SSG = SpreadsheetGear; // AWS Cloud Migration

namespace ZurichNA.AIS.WebSite.AcctSetup
{
    public partial class PolicyInfo : AISBasePage
    {
        // AWS Cloud Migration
        string sSpreadsheetGearExcelFileFormat = ConfigurationManager.AppSettings["SpreadsheetGearExcelFileFormat"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ppProgramPeriod.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ppProgramPeriod_ProgramPeriodRowClicked);
            if (!Page.IsPostBack)
            {
                this.Master.Page.Title = "Policy Information";
                //PolicyInfoTransWrapper = new AISBusinessTransaction();
                SortBy = "";
                SortDir = "";
            }

            //Checks Exiting without Save
            CheckExitingSave();

        }

        private void CheckExitingSave()
        {
            //Checks Exiting without Save
            ArrayList list = new ArrayList();
            list.Add(txtALAECAPPED);
            list.Add(txtDedPolLimAmt);
            list.Add(txtDedPolLimAmt);
            list.Add(txtDepMaxAmt);
            list.Add(txtIBNR);
            list.Add(txtLDF);
            list.Add(txtNonConv);
            list.Add(txtOthAmt);
            list.Add(txtNYPremiumDiscAmt);
            list.Add(txtOvrDedLimAmt);
            list.Add(txtPolicyEff);
            list.Add(txtPolicyExp);
            list.Add(txtPolicyMod);
            list.Add(txtPolicyNumber);
            list.Add(txtPolicySymbol);
            list.Add(txtProgramPeriodEffDate);
            list.Add(txtProgramPeriodExpDate);
            list.Add(CalendarExtender1);
            list.Add(CalendarExtender12);
            list.Add(chkLDFIBNRInclLim);
            list.Add(chkLDFIBNRStepped);
            list.Add(chkPEOPolicy);
            list.Add(chkTPA);
            list.Add(chkTPADirect);
            list.Add(chkUnLimDedPolLim);
            list.Add(chkUnLimOvrDedLim);
            list.Add(btnAdd);
            list.Add(btnCancel);
            list.Add(btnCopy);
            list.Add(btnSave);
            ProcessExitFlag(list);
        }
        //protected AISBusinessTransaction PolicyInfoTransWrapper
        //{
        //    get
        //    {
        //        if ((AISBusinessTransaction)Session["POLICYINFOTRANSACTION"] == null)
        //            Session["POLICYINFOTRANSACTION"] = new AISBusinessTransaction();
        //        return (AISBusinessTransaction)Session["POLICYINFOTRANSACTION"];
        //    }
        //    set
        //    {
        //        Session["POLICYINFOTRANSACTION"] = value;
        //    }
        //}
        #region Program Period
        protected int SelectedProgramPeriodID
        {
            get
            {
                if (hidSelProgramPeriod.Value != null)
                    return Convert.ToInt32(hidSelProgramPeriod.Value);
                else
                    return -1;
            }
            set { hidSelProgramPeriod.Value = value.ToString(); }
        }
        protected string proPerStartDate
        {
            get { return (string)ViewState["PROPERSTARTDATE"]; }
            set { ViewState["PROPERSTARTDATE"] = value; }
        }
        protected string proPerEndDate
        {
            get { return (string)ViewState["PROPERENDDATE"]; }
            set { ViewState["PROPERENDDATE"] = value; }
        }
        protected bool isProPerDEP
        {
            get { return (bool)ViewState["ISPROPERDEP"]; }
            set { ViewState["ISPROPERDEP"] = value; }
        }



        /// <summary>
        /// Invoked when Program Period row is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ppProgramPeriod_ProgramPeriodRowClicked(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                isProPerDEP = false;
                SelectedProgramPeriodID = Convert.ToInt32(e.CommandArgument);
                ProgramPeriodsBS proPreBS = new ProgramPeriodsBS();
                ProgramPeriodBE proPreBE = new ProgramPeriodBE();
                proPreBE = proPreBS.getProgramPeriodInfo(SelectedProgramPeriodID);
                proPerStartDate = Convert.ToDateTime(proPreBE.STRT_DT).ToShortDateString();
                proPerEndDate = Convert.ToDateTime(proPreBE.PLAN_END_DT).ToShortDateString();
                if (proPreBE.PROGRAMTYPENAME.StartsWith("DEP")) isProPerDEP = true;
                //0623 for vera code 
                //lblDateRange.Text = "[" + proPerStartDate + " - " + proPerEndDate + "]";
                //lblDateRange.Text = "[" + Server.HtmlDecode(Server.HtmlEncode(proPerStartDate)) + " - " + Server.HtmlDecode(Server.HtmlEncode(proPerEndDate)) + "]";
                lblDateRange.Text = "[" + (Server.HtmlEncode(Convert.ToString(proPerStartDate))) + " - " + (Server.HtmlEncode(Convert.ToString(proPerEndDate))) + "]";
                BindPolicyInformation();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, ex);
            }
        }
        private string SortBy
        {
            get
            {
                //return (string)Session["SORTBY"]; 
                return (string)RetrieveObjectFromSessionUsingWindowName("SORTBY");
            }
            set
            {
                //Session["SORTBY"] = value; 
                SaveObjectToSessionUsingWindowName("SORTBY", value);
            }
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
        /// <summary>
        /// Binding Policy Info List View
        /// </summary>
        private void BindPolicyInformation()
        {
            //lstPolicyInfo.DataSource = PolicyBizService.getPolicyData(SelectedProgramPeriodID);
            lstPolicyInfo.DataSource = GetSortedPolicyData(SelectedProgramPeriodID);
            lstPolicyInfo.DataBind();
            lblPolicyInfo.Visible = true;
            pnlPolicyInfo.Visible = true;
            lstPolicyInfo.Enabled = true;

            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.AdjSpecialist || WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Manager || WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
            {
                btnUpload.Visible = true;
                btnSFUpload.Visible = true;
            }

            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnAdd.Enabled = true;


            lblPolicyDetails.Visible = false;
            lbClosePolicyDetails.Visible = false;
            pnlDetails.Visible = false;

            lblSteppedFactor.Visible = false;
            lbCloseSteppedFactor.Visible = false;
            pnlSteppedFactor.Visible = false;


        }
        private IList<PolicyBE> GetSortedPolicyData(int SelectedProgramPeriodID)
        {
            IList<PolicyBE> PolicyList = new List<PolicyBE>();
            PolicyList = PolicyBizService.getPolicyData(SelectedProgramPeriodID);
            switch (SortBy)
            {
                case "PolicyEffDt":
                    if (SortDir == "ASC")
                        PolicyList = (PolicyList.OrderBy(o => o.PolicyEffectiveDate)).ToList();
                    else if (SortDir == "DESC")
                        PolicyList = (PolicyList.OrderByDescending(o => o.PolicyEffectiveDate)).ToList();

                    break;

                case "AdjType":
                    if (SortDir == "ASC")
                        PolicyList = (PolicyList.OrderBy(o => o.AdjustmentTypeName)).ToList();
                    else if (SortDir == "DESC")
                        PolicyList = (PolicyList.OrderByDescending(o => o.AdjustmentTypeName)).ToList();

                    break;
            }
            return PolicyList;


        }
        protected void lstPolicyInfo_Sorting(object sender, ListViewSortEventArgs e)
        {
            Image imgSortByPolicyEffDt = (Image)lstPolicyInfo.FindControl("imgSortByPolicyEffDt");
            Image imgSortByAdjType = (Image)lstPolicyInfo.FindControl("imgSortByAdjType");
            Image img = new Image();
            switch (e.SortExpression)
            {
                case "PolicyEffDt":
                    SortBy = "PolicyEffDt";
                    imgSortByPolicyEffDt.Visible = true;
                    imgSortByAdjType.Visible = false;
                    img = imgSortByPolicyEffDt;
                    break;

                case "AdjType":
                    SortBy = "AdjType";
                    imgSortByPolicyEffDt.Visible = false;
                    imgSortByAdjType.Visible = true;
                    img = imgSortByAdjType;
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
            BindPolicyInformation();
        }
        #endregion

        #region Policy Info Details
        protected bool isNew
        {
            get
            {
                if (ViewState["ISNEW"] != null)
                { return (bool)ViewState["ISNEW"]; }
                else
                {
                    ViewState["ISNEW"] = false;
                    return false;
                }
            }
            set { ViewState["ISNEW"] = value; }
        }
        protected bool isCopy
        {
            get
            {
                if (ViewState["ISCOPY"] != null)
                { return (bool)ViewState["ISCOPY"]; }
                else
                {
                    ViewState["ISCOPY"] = false;
                    return false;
                }
            }
            set { ViewState["ISCOPY"] = value; }
        }
        private PolicyBS policyBizService;
        private PolicyBS PolicyBizService
        {
            get
            {
                if (policyBizService == null)
                {
                    policyBizService = new PolicyBS();
                    // policyBizService.AppTransactionWrapper = PolicyInfoTransWrapper;
                }
                return policyBizService;
            }
        }
        private ApplWebPageAudtBS auditBizService;
        private ApplWebPageAudtBS AuditBizService
        {
            get
            {
                if (auditBizService == null)
                {
                    auditBizService = new ApplWebPageAudtBS();
                    //   auditBizService.AppTransactionWrapper = PolicyInfoTransWrapper;
                }
                return auditBizService;
            }
        }
        private PolicyBE polBE
        {
            get
            {
                //return (PolicyBE)Session["POLICYBE"]; 
                return (PolicyBE)RetrieveObjectFromSessionUsingWindowName("POLICYBE");
            }
            set
            {
                //Session["POLICYBE"] = value; 
                SaveObjectToSessionUsingWindowName("POLICYBE", value);
            }
        }

        protected int SelectedPolicyID
        {
            get
            {
                if (hidSelValue.Value != null)
                    return Convert.ToInt32(hidSelValue.Value);
                else
                    return -1;
            }
            set { hidSelValue.Value = value.ToString(); }
        }
        /// <summary>
        /// Invoked when user clicks on PolicyInfo listview. Below panel will be displayed with filled selected policy info. Can edit
        /// </summary>
        /// <param name="policyID"></param>
        private void DisplayDetails(int policyID)
        {
            if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager && CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
            {
                txtOthAmt.Enabled = false;
            }
            //Loding PolicyBE using Framwork load. and keeping it in session
            polBE = PolicyBizService.getPolicyInfo(policyID);
            txtPolicySymbol.Text = polBE.PolicySymbol;
            txtPolicyNumber.Text = polBE.PolicyNumber;
            txtPolicyMod.Text = polBE.PolicyModulus;
            txtPolicyEff.Text = Convert.ToDateTime(polBE.PolicyEffectiveDate).ToShortDateString();
            txtPolicyExp.Text = Convert.ToDateTime(polBE.PlanEndDate).ToShortDateString();
            txtProgramPeriodEffDate.Text = Convert.ToDateTime(proPerStartDate).ToShortDateString();
            txtProgramPeriodExpDate.Text = Convert.ToDateTime(proPerEndDate).ToShortDateString();
            if (polBE.CoverageTypeID != null)
            {
                ddlCoverageType.DataBind();
                //                ddlCoverageType.Items.FindByValue(polBE.CoverageTypeID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlCoverageType, polBE.CoverageTypeID.Value);
            }
            else
            {
                ddlCoverageType.DataBind();
                ddlCoverageType.Items.FindByValue("0").Selected = true;
            }

            if (polBE.AdjusmentTypeID != null)
            {
                ddlAdjustmentType.DataBind();
                //                ddlAdjustmentType.Items.FindByValue(polBE.AdjusmentTypeID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlAdjustmentType, polBE.AdjusmentTypeID.Value);
            }
            else
            {
                ddlAdjustmentType.DataBind();
                ddlAdjustmentType.Items.FindByValue("0").Selected = true;
            }

            if (polBE.ALAETypeID != null)
            {
                ddlALAE.DataBind();
                //                ddlALAE.Items.FindByValue(polBE.ALAETypeID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlALAE, polBE.ALAETypeID.Value);
            }
            else
            {
                ddlALAE.DataBind();
                ddlALAE.Items.FindByValue("0").Selected = true;
            }
            //The system will allow entry of ALAE Cap amount digits when Policy Information associated 
            //with a Program Period if ALAE Handling Type = "Capped".
            if (ddlALAE.SelectedItem.Text == "Capped Amount")
                txtALAECAPPED.Enabled = true;
            else
                txtALAECAPPED.Enabled = false;
            if (polBE.ALAECappedAmount != null)
                txtALAECAPPED.Text = Math.Round(Convert.ToDecimal(polBE.ALAECappedAmount), 0).ToString("#,##0");
            else
                txtALAECAPPED.Text = "";

            if (polBE.UnlimDedtblPolLimitIndicator != null)
            {
                if (polBE.UnlimDedtblPolLimitIndicator == true)
                { chkUnLimDedPolLim.Checked = true; }
                else
                { chkUnLimDedPolLim.Checked = false; }
            }
            if (chkUnLimDedPolLim.Checked)
                txtDedPolLimAmt.Enabled = false;
            else
                txtDedPolLimAmt.Enabled = true;
            if (polBE.DedTblPolicyLimitAmount != null)
                txtDedPolLimAmt.Text = Math.Round(Convert.ToDecimal(polBE.DedTblPolicyLimitAmount), 0).ToString("#,##0");
            else
                txtDedPolLimAmt.Text = "";
            if (polBE.UnlimOverrideDedtblLimitIndicator != null)
            {
                if (polBE.UnlimOverrideDedtblLimitIndicator == true)
                { chkUnLimOvrDedLim.Checked = true; }
                else
                { chkUnLimOvrDedLim.Checked = false; }
            }
            if (chkUnLimOvrDedLim.Checked)
                txtOvrDedLimAmt.Enabled = false;
            else
                txtOvrDedLimAmt.Enabled = true;
            if (polBE.OverrideDedtblLimitAmount != null)
                txtOvrDedLimAmt.Text = Math.Round(Convert.ToDecimal(polBE.OverrideDedtblLimitAmount), 0).ToString("#,##0");
            else
                txtOvrDedLimAmt.Text = "";
            if (polBE.LDFIncurredNotReport != null)
            {
                if (polBE.LDFIncurredNotReport == true)
                { chkLDFIBNRInclLim.Checked = true; }
                else
                { chkLDFIBNRInclLim.Checked = false; }
            }
            if (polBE.LDFIncurredNO63740 != null)
            {
                if (polBE.LDFIncurredNO63740 == true) { chkLDFIBNRStepped.Checked = true; }
                else { chkLDFIBNRStepped.Checked = false; }
            }
            if (polBE.LDFFactor != null)
            {
                txtLDF.Text = Convert.ToDecimal(polBE.LDFFactor).ToString("0.000000");
                txtIBNR.Enabled = false;
            }
            else
            {
                txtLDF.Text = "";
                txtIBNR.Enabled = true;
            }
            if (polBE.IBNRFactor != null)
            {
                txtIBNR.Text = Convert.ToDecimal(polBE.IBNRFactor).ToString("0.000000");
                txtLDF.Enabled = false;
            }
            else
            {
                txtIBNR.Text = "";
                txtLDF.Enabled = true;
            }
            if (chkLDFIBNRStepped.Checked)
            {
                txtLDF.Enabled = false;
                txtIBNR.Enabled = false;
            }
            else
            {
                txtLDF.Enabled = true;
                txtIBNR.Enabled = true;
                if (polBE.LDFFactor != null)
                {
                    txtIBNR.Enabled = false;
                }
                else
                {
                    txtIBNR.Enabled = true;
                }
                if (polBE.IBNRFactor != null)
                {
                    txtLDF.Enabled = false;
                }
                else
                {
                    txtLDF.Enabled = true;
                }
            }
            if (polBE.DedtblProtPolicyStID != null)
            {
                ddlDepState.DataBind();
                //                ddlDepState.Items.FindByValue(polBE.DedtblProtPolicyStID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlDepState, polBE.DedtblProtPolicyStID.Value);
            }
            else
            {
                ddlDepState.DataBind();
                ddlDepState.Items.FindByValue("0").Selected = true;
            }


            if (polBE.TPAIndicator != null)
            {
                if (polBE.TPAIndicator == true) { chkTPA.Checked = true; }
                else { chkTPA.Checked = false; chkTPADirect.InputAttributes.Add("disabled", "disabled"); }
            }
            if (polBE.TPADirectIndicator != null)
            {
                if (polBE.TPADirectIndicator == true) { chkTPADirect.Checked = true; }
                else { chkTPADirect.Checked = false; }
            }
            if (polBE.LossSystemSourceID != null)
            {
                ddlLossSource.DataBind();
                //                ddlLossSource.Items.FindByValue(polBE.LossSystemSourceID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlLossSource, polBE.LossSystemSourceID.Value);
            }
            else
            {
                ddlLossSource.DataBind();
                ddlLossSource.Items.FindByValue("0").Selected = true;
            }

            if (polBE.NonConversionAmount != null)
                txtNonConv.Text = Math.Round(Convert.ToDecimal(polBE.NonConversionAmount), 0).ToString("#,##0");
            else
                txtNonConv.Text = "";

            if (polBE.OtherPolicyAdjustmentAmount != null)
                txtOthAmt.Text = Math.Round(Convert.ToDecimal(polBE.OtherPolicyAdjustmentAmount), 0).ToString("#,##0");
            else
                txtOthAmt.Text = "";

            //Display the NY Premium Discount amount when a particulat policy is selected.
            if (polBE.NYPremiumDiscAmount != null)
                txtNYPremiumDiscAmt.Text = Math.Round(Convert.ToDecimal(polBE.NYPremiumDiscAmount), 0).ToString("#,##0");
            else
                txtNYPremiumDiscAmt.Text = "";

            if (!isProPerDEP)
            {
                ddlDepState.Enabled = false;

            }
            else
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)

                    ddlDepState.Enabled = true;

            }
            if (polBE.ISMasterPEOPolicy != null)
            {
                if (polBE.ISMasterPEOPolicy == true) { chkPEOPolicy.Checked = true; }
                else { chkPEOPolicy.Checked = false; }
            }
            btnSave.Text = "Update";
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                pnlDetails.Enabled = Convert.ToBoolean(polBE.IsActive);
        }
        /// <summary>
        /// Will clear all fields when Add new is clicked
        /// </summary>
        public void ClearFileds()
        {
            btnSave.Text = "Save";
            txtPolicySymbol.Text = "";
            txtPolicyNumber.Text = "";
            txtPolicyMod.Text = "";
            txtPolicyEff.Text = proPerStartDate;
            txtPolicyExp.Text = proPerEndDate;
            txtProgramPeriodEffDate.Text = proPerStartDate;
            txtProgramPeriodExpDate.Text = proPerEndDate;
            ddlCoverageType.DataBind();
            ddlCoverageType.Items.FindByValue("0").Selected = true;
            ddlAdjustmentType.DataBind();
            ddlAdjustmentType.Items.FindByValue("0").Selected = true;
            ddlALAE.DataBind();
            ddlALAE.Items.FindByValue("0").Selected = true;
            txtALAECAPPED.Text = "";
            chkUnLimDedPolLim.Checked = false;
            txtDedPolLimAmt.Text = "";
            chkUnLimOvrDedLim.Checked = false;
            txtOvrDedLimAmt.Text = "";
            chkLDFIBNRInclLim.Checked = false;
            chkLDFIBNRStepped.Checked = false;
            txtLDF.Text = "";
            txtIBNR.Text = "";
            txtLDF.Enabled = true;
            txtIBNR.Enabled = true;
            ddlDepState.DataBind();
            txtNYPremiumDiscAmt.Text = "";
            ddlDepState.Items.FindByValue("0").Selected = true;
            chkPEOPolicy.Checked = false;

            if (!isProPerDEP)
            {
                ddlDepState.Enabled = false;
            }
            else
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    ddlDepState.Enabled = true;
            }
            chkTPA.Checked = false;
            chkTPADirect.Checked = false;
            ddlLossSource.DataBind();
            ddlLossSource.Items.FindByValue("0").Selected = true;
            txtNonConv.Text = "";
            txtOthAmt.Text = "";
            txtOthAmt.Enabled = true;
            pnlDetails.Enabled = true;
        }
        protected void lstPolicyInfo_RowDataBound(object sender, EventArgs e)
        {

        }
        protected void lstPolicyInfo_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            HtmlTableRow tr;
            if (ViewState["SelectedIndex"] != null)
            {
                int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                if (lstPolicyInfo.Items.Count > index)
                {
                    tr = (HtmlTableRow)lstPolicyInfo.Items[index].FindControl("trItemTemplate");
                    if ((index % 2) == 0)
                        tr.Attributes["class"] = "ItemTemplate";
                    else
                        tr.Attributes["class"] = "AlternatingItemTemplate";
                }
            }
            tr = (HtmlTableRow)lstPolicyInfo.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
            tr.Attributes["class"] = "SelectedItemTemplate";
            ViewState["SelectedIndex"] = e.NewSelectedIndex;
        }
        protected void lstPolicyInfo_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ImageButton imgPolicyEnableDisable = (ImageButton)e.Item.FindControl("imgPolicyEnableDisable");
            Label lblPolicyActive = (Label)e.Item.FindControl("lblPolicyActive");
            if (imgPolicyEnableDisable != null)
            {

                if (lblPolicyActive.Text == "True")
                {
                    //Part of role based security
                    if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager && CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
                    {
                        imgPolicyEnableDisable.Enabled = false;
                    }
                    imgPolicyEnableDisable.ImageUrl = "~/images/disabled.GIF";
                    imgPolicyEnableDisable.CommandName = "DISABLE";
                    imgPolicyEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable?');");
                    imgPolicyEnableDisable.ToolTip = "Click here to Disable this Policy";
                }
                else
                {
                    imgPolicyEnableDisable.ImageUrl = "~/images/enabled.GIF";
                    imgPolicyEnableDisable.CommandName = "ENABLE";
                    imgPolicyEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable?');");
                    imgPolicyEnableDisable.ToolTip = "Click here to Enable this Policy";
                }
            }
            //Stepped Factor LinkButton will be active only when LDF or IBNR is captured as a stepped factor
            LinkButton lbSteppedFactor = (LinkButton)e.Item.FindControl("lbSteppedFactor");
            if (lbSteppedFactor != null)
            {
                Label lblSteppedcheck = (Label)e.Item.FindControl("lblSteppedcheck");
                if ((lblSteppedcheck.Text == "True") && (lblPolicyActive.Text == "True"))
                {
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        lbSteppedFactor.Enabled = true;
                }
                else
                    lbSteppedFactor.Enabled = false;
            }
        }
        /// <summary>
        /// Event will be raised when user want to save a new record or edit a record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SAVE_Click(object sender, EventArgs e)
        {
            if (ddlLossSource.SelectedItem.Text.ToUpper() == "ARMIS/TPA" && chkTPA.Checked == false && chkTPADirect.Checked == false)
            {
                ShowError("The TPA indicator box or TPA Direct indicator box must be checked when loss source option ARMIS/TPA is selected");
            }
            else
            {
                int OldPolicyID = 0;
                bool isDataChanged = false;
                //try
                //{
                //Checking if it is New

                if (isNew == false)
                {    //Loading the BE using the FrameWork

                    polBE.UPDATE_DATE = DateTime.Now;
                    polBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                    isDataChanged = IsDataChanged(polBE);
                }
                else
                {
                    if (isCopy) OldPolicyID = polBE.PolicyID; //will be used if copy is selected

                    polBE = new PolicyBE();
                    polBE.cstmrid = AISMasterEntities.AccountNumber;
                    polBE.ProgramPeriodID = SelectedProgramPeriodID;
                    polBE.CREATE_DATE = DateTime.Now;
                    polBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                    polBE.IsActive = true;
                }
                polBE.PolicySymbol = txtPolicySymbol.Text.ToUpper().ToString();
                //Prefix '0' if Policy Number lengthe is 7
                if (txtPolicyNumber.Text.Length == 7) txtPolicyNumber.Text = "0" + txtPolicyNumber.Text;
                polBE.PolicyNumber = txtPolicyNumber.Text.ToString();
                polBE.PolicyModulus = txtPolicyMod.Text.ToString();
                if (txtPolicyEff.Text != "")
                    polBE.PolicyEffectiveDate = DateTime.Parse(txtPolicyEff.Text);
                else
                    polBE.PolicyEffectiveDate = null;

                if (txtPolicyExp.Text != "")
                    polBE.PlanEndDate = DateTime.Parse(txtPolicyExp.Text);

                if (Convert.ToInt32(ddlCoverageType.SelectedValue) != 0)
                    polBE.CoverageTypeID = Convert.ToInt32(ddlCoverageType.SelectedValue);
                else
                    polBE.CoverageTypeID = null;

                if (Convert.ToInt32(ddlALAE.SelectedValue) != 0)
                    polBE.ALAETypeID = Convert.ToInt32(ddlALAE.SelectedValue);
                else
                    polBE.ALAETypeID = null;

                if (Convert.ToInt32(ddlAdjustmentType.SelectedValue) != 0)
                    polBE.AdjusmentTypeID = Convert.ToInt32(ddlAdjustmentType.SelectedValue);
                else
                    polBE.AdjusmentTypeID = null;

                if (txtALAECAPPED.Text != "")
                    polBE.ALAECappedAmount = Convert.ToDecimal(txtALAECAPPED.Text.Replace(",", ""));
                else
                    polBE.ALAECappedAmount = 0;

                polBE.UnlimDedtblPolLimitIndicator = chkUnLimDedPolLim.Checked;

                if (txtDedPolLimAmt.Text != "")
                    polBE.DedTblPolicyLimitAmount = Convert.ToDecimal(txtDedPolLimAmt.Text.Replace(",", ""));
                else
                    polBE.DedTblPolicyLimitAmount = 0;

                polBE.UnlimOverrideDedtblLimitIndicator = chkUnLimOvrDedLim.Checked;
                if (txtOvrDedLimAmt.Text != "")
                    polBE.OverrideDedtblLimitAmount = Convert.ToDecimal(txtOvrDedLimAmt.Text.Replace(",", ""));
                else
                    polBE.OverrideDedtblLimitAmount = 0;

                polBE.LDFIncurredNotReport = chkLDFIBNRInclLim.Checked;
                polBE.LDFIncurredNO63740 = chkLDFIBNRStepped.Checked;
                if (txtLDF.Text != "")
                    polBE.LDFFactor = Convert.ToDecimal(txtLDF.Text);
                else
                    polBE.LDFFactor = null;
                if (txtIBNR.Text != "")
                    polBE.IBNRFactor = Convert.ToDecimal(txtIBNR.Text);
                else
                    polBE.IBNRFactor = null;

                if (Convert.ToInt32(ddlDepState.SelectedValue) != 0)
                    polBE.DedtblProtPolicyStID = Convert.ToInt32(ddlDepState.SelectedValue);
                else
                    polBE.DedtblProtPolicyStID = null;

                polBE.TPAIndicator = chkTPA.Checked;
                polBE.TPADirectIndicator = chkTPADirect.Checked;
                if (Convert.ToInt32(ddlLossSource.SelectedValue) != 0)
                    polBE.LossSystemSourceID = Convert.ToInt32(ddlLossSource.SelectedValue);
                else
                    polBE.LossSystemSourceID = null;
                if (txtNonConv.Text != "")
                    polBE.NonConversionAmount = Convert.ToDecimal(txtNonConv.Text.Replace(",", ""));
                else
                    polBE.NonConversionAmount = null;
                if (txtOthAmt.Text != "")
                    polBE.OtherPolicyAdjustmentAmount = Convert.ToDecimal(txtOthAmt.Text.Replace(",", ""));
                else
                    polBE.OtherPolicyAdjustmentAmount = null;
                if (txtNYPremiumDiscAmt.Text != "")
                    polBE.NYPremiumDiscAmount = Convert.ToDecimal(txtNYPremiumDiscAmt.Text);
                else
                    polBE.NYPremiumDiscAmount = null;
                polBE.ISMasterPEOPolicy = chkPEOPolicy != null ? (chkPEOPolicy.Checked) : false;
                //checking for An identical record if Policy Number and EffDate is same for a Program Period
                bool isPolExists = PolicyBizService.isPolicyAlreadyExist(polBE.PolicyID, polBE.PolicyNumber, Convert.ToDateTime(polBE.PolicyEffectiveDate), SelectedProgramPeriodID);
                //A policy number with an identical Symbol, Policy Number, Modulus, Effective Date, Expiration Date cannot span two Accounts
                bool isPolExistsInAnyAcct = PolicyBizService.isPolExistsInAnyAcct(polBE.PolicyID, polBE.PolicySymbol, polBE.PolicyNumber, polBE.PolicyModulus, Convert.ToDateTime(polBE.PolicyEffectiveDate), Convert.ToDateTime(polBE.PlanEndDate), AISMasterEntities.AccountNumber);
                if ((isPolExists == false) && (isPolExistsInAnyAcct == false))
                {
                    bool Result = false;
                    if (isNew == true)
                        Result = PolicyBizService.Update(polBE);
                    else if ((isNew == false))
                    {
                        Result = PolicyBizService.Update(polBE);
                        ShowConcurrentConflict(Result, polBE.ErrorMessage);

                        if (Result == true && isDataChanged == true) AuditBizService.Save(AISMasterEntities.AccountNumber, SelectedProgramPeriodID, GlobalConstants.AuditingWebPage.PolicyInfo, CurrentAISUser.PersonID);
                    }
                    if (isCopy == true)
                    {
                        //The system will copy all Policy information, including stepped LDF or IBNR factors,
                        //when Policy Information is copied to a new Policy.
                        CopyDependencies(OldPolicyID, polBE.PolicyID);
                    }
                    isNew = false;
                    isCopy = false;
                    BindPolicyInformation();
                    SelectedPolicyID = polBE.PolicyID;
                    DisplayDetails(SelectedPolicyID);

                    btnSave.Enabled = true;
                    btnCopy.Enabled = true;
                    btnCancel.Enabled = true;
                    //to show selected list view row to highlite
                    if (lstPolicyInfo.Items.Count > 0)
                    {
                        //to show the Listview row selected
                        LinkButton lnkSelect;
                        HtmlTableRow tr;
                        int selectedindex = -1;
                        for (int i = 0; i < lstPolicyInfo.Items.Count; i++)
                        {
                            lnkSelect = (LinkButton)lstPolicyInfo.Items[i].FindControl("lbSelect");
                            if (Convert.ToInt32(lnkSelect.CommandArgument) == SelectedPolicyID)
                            {
                                selectedindex = i;
                                break;
                            }
                        }
                        if (selectedindex > -1)
                        {
                            tr = (HtmlTableRow)lstPolicyInfo.Items[selectedindex].FindControl("trItemTemplate");
                            tr.Attributes["class"] = "SelectedItemTemplate";
                            ViewState["SelectedIndex"] = selectedindex;
                        }
                    }

                    lstPolicyInfo.Enabled = false;
                    PnlProgramperiod.Enabled = false;

                    lblPolicyDetails.Visible = true;
                    lbClosePolicyDetails.Visible = true;
                    pnlDetails.Visible = true;
                }
                else
                {
                    if (isPolExists == true)
                    {
                        bool isPolExistsAndDisabled = PolicyBizService.isPolicyAlreadyExistAndDisabled(polBE.PolicyID, polBE.PolicyNumber, Convert.ToDateTime(polBE.PolicyEffectiveDate), SelectedProgramPeriodID);
                        if (isPolExistsAndDisabled == true) ShowMessage("The record cannot be saved. An identical record already exists. Please enable existing policy");
                        else ShowMessage("The record cannot be saved. An identical record already exists.");
                    }
                    else if (isPolExistsInAnyAcct == true)
                        ShowMessage("The record cannot be saved. A policy number with an identical Symbol, Policy Number, Modulus, Effective Date, Expiration Date cannot span two Accounts");

                    BindPolicyInformation();
                    lblPolicyDetails.Visible = true;
                    lbClosePolicyDetails.Visible = true;
                    pnlDetails.Visible = true;

                }

                //lblPolicyDetails.Visible = false;
                //lstPolicyInfo.Enabled = true;
                //PnlProgramperiod.Enabled = true;
                //pnlDetails.Visible = false;
                //BindPolicyInformation();

                //}
                //catch (Exception ex)
                //{
                //    ShowError(ex.Message);
                //}
            }
        }
        /// <summary>
        /// This is to check any data got changed when user clicks on Save button for updation
        /// If changed then only an entry will go to Audit Trial
        /// </summary>
        /// <param name="policyBE"></param>
        /// <returns></returns>
        private bool IsDataChanged(PolicyBE polBE)
        {
            bool isDataChanged = false;
            isDataChanged = (polBE.PolicySymbol.Trim() != txtPolicySymbol.Text.Trim()) ? true : false;
            isDataChanged = (polBE.PolicyNumber.Trim() != txtPolicyNumber.Text.Trim()) ? true : isDataChanged;
            isDataChanged = (polBE.PolicyModulus.Trim() != txtPolicyMod.Text.Trim()) ? true : isDataChanged;
            isDataChanged = (polBE.PolicyEffectiveDate != DateTime.Parse(txtPolicyEff.Text)) ? true : isDataChanged;
            isDataChanged = (polBE.PlanEndDate != DateTime.Parse(txtPolicyExp.Text)) ? true : isDataChanged;
            isDataChanged = (polBE.CoverageTypeID != Convert.ToInt32(ddlCoverageType.SelectedValue)) ? true : isDataChanged;
            isDataChanged = (polBE.AdjusmentTypeID != Convert.ToInt32(ddlAdjustmentType.SelectedValue)) ? true : isDataChanged;
            isDataChanged = (polBE.ALAETypeID != Convert.ToInt32(ddlALAE.SelectedValue)) ? true : isDataChanged;
            if (txtALAECAPPED.Text != "")
                isDataChanged = (polBE.ALAECappedAmount != Convert.ToDecimal(txtALAECAPPED.Text)) ? true : isDataChanged;
            else if (txtALAECAPPED.Text == "")
                isDataChanged = (polBE.ALAECappedAmount != null) ? true : isDataChanged;
            isDataChanged = (polBE.UnlimDedtblPolLimitIndicator != chkUnLimDedPolLim.Checked) ? true : isDataChanged;
            if (txtDedPolLimAmt.Text != "")
                isDataChanged = (polBE.DedTblPolicyLimitAmount != Convert.ToDecimal(txtDedPolLimAmt.Text)) ? true : isDataChanged;
            else if (txtDedPolLimAmt.Text == "")
                isDataChanged = (polBE.DedTblPolicyLimitAmount != null) ? true : isDataChanged;
            isDataChanged = (polBE.UnlimOverrideDedtblLimitIndicator != chkUnLimOvrDedLim.Checked) ? true : isDataChanged;
            if (txtOvrDedLimAmt.Text != "")
                isDataChanged = (polBE.OverrideDedtblLimitAmount != Convert.ToDecimal(txtOvrDedLimAmt.Text)) ? true : isDataChanged;
            else if (txtOvrDedLimAmt.Text == "")
                isDataChanged = (polBE.OverrideDedtblLimitAmount != null) ? true : isDataChanged;
            isDataChanged = (polBE.LDFIncurredNotReport != chkLDFIBNRInclLim.Checked) ? true : isDataChanged;
            isDataChanged = (polBE.LDFIncurredNO63740 != chkLDFIBNRStepped.Checked) ? true : isDataChanged;

            if (txtLDF.Text != "")
                isDataChanged = (polBE.LDFFactor != Convert.ToDecimal(txtLDF.Text)) ? true : isDataChanged;
            else if (txtLDF.Text == "")
                isDataChanged = (polBE.LDFFactor != null) ? true : isDataChanged;

            if (txtIBNR.Text != "")
                isDataChanged = (polBE.IBNRFactor != Convert.ToDecimal(txtIBNR.Text)) ? true : isDataChanged;
            else if (txtIBNR.Text == "")
                isDataChanged = (polBE.IBNRFactor != null) ? true : isDataChanged;

            if (Convert.ToInt32(ddlDepState.SelectedValue) != 0)
                isDataChanged = (polBE.DedtblProtPolicyStID != Convert.ToInt32(ddlDepState.SelectedValue)) ? true : isDataChanged;
            else if (Convert.ToInt32(ddlDepState.SelectedValue) == 0)
                isDataChanged = (polBE.DedtblProtPolicyStID != null) ? true : isDataChanged;

            isDataChanged = (polBE.TPAIndicator != chkTPA.Checked) ? true : isDataChanged;
            isDataChanged = (polBE.TPADirectIndicator != chkTPADirect.Checked) ? true : isDataChanged;

            if (Convert.ToInt32(ddlLossSource.SelectedValue) != 0)
                isDataChanged = (polBE.LossSystemSourceID != Convert.ToInt32(ddlLossSource.SelectedValue)) ? true : isDataChanged;
            else if (Convert.ToInt32(ddlLossSource.SelectedValue) == 0)
                isDataChanged = (polBE.LossSystemSourceID != null) ? true : isDataChanged;
            if (txtNonConv.Text != "")
                isDataChanged = (polBE.NonConversionAmount != Convert.ToDecimal(txtNonConv.Text)) ? true : isDataChanged;
            else if (txtNonConv.Text == "")
                isDataChanged = (polBE.NonConversionAmount != null) ? true : isDataChanged;

            if (txtOthAmt.Text != "")
                isDataChanged = (polBE.OtherPolicyAdjustmentAmount != Convert.ToDecimal(txtOthAmt.Text)) ? true : isDataChanged;
            else if (txtOthAmt.Text == "")
                isDataChanged = (polBE.OtherPolicyAdjustmentAmount != null) ? true : isDataChanged;

            // Update functionality for NY Premium Discount amount.
            if (txtNYPremiumDiscAmt.Text != "")
                isDataChanged = (polBE.NYPremiumDiscAmount != Convert.ToDecimal(txtNYPremiumDiscAmt.Text)) ? true : isDataChanged;
            else if (txtNYPremiumDiscAmt.Text == "")
                isDataChanged = (polBE.NYPremiumDiscAmount != null) ? true : isDataChanged;

            isDataChanged = (polBE.ISMasterPEOPolicy != chkPEOPolicy.Checked) ? true : isDataChanged;
            return isDataChanged;
        }
        private bool CopyDependencies(int oldPolicyID, int newPolicyID)
        {
            bool isCopySuccess = false;
            IList<SteppedFactorBE> SteppedFactorList = new List<SteppedFactorBE>();
            SteppedFactorList = SteppedFactorBizService.GetSteppedFactorData(oldPolicyID);
            foreach (SteppedFactorBE stepFactorBE in SteppedFactorList)
            {
                stepFactorBE.STEPPED_FACTOR_ID = 0;
                stepFactorBE.POLICY_ID = newPolicyID;
                stepFactorBE.PREM_ADJ_PGM_ID = SelectedProgramPeriodID;
                stepFactorBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                stepFactorBE.UPDATE_DATE = null;
                stepFactorBE.UPDATE_USER_ID = null;
                stepFactorBE.CREATE_DATE = DateTime.Now;
                stepFactorBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                isCopySuccess = ((!SteppedFactorBizService.Update(stepFactorBE)) ? false : isCopySuccess);
            }

            return true;
        }


        protected void btnSFErrorLog_Click(object sender, EventArgs e)
        {
            // btnErrorLog_Policy.Visible = false;
            try
            {
                string FileName = string.Empty;
                string FilePath = string.Empty;
                if (Session["strMessageErrorLog_SFUpload"] != null && Session["strMessageErrorLog_SFUpload"] != string.Empty)
                {
                    string[] SessionValue = Session["strMessageErrorLog_SFUpload"].ToString().Split('|');
                    FileName = SessionValue[0];
                    FilePath = SessionValue[1];

                }

                FileInfo filepolicyuploadSFerror = new FileInfo(FilePath);
                //FileInfo filepolicyuploaderror = new FileInfo(FilePath + "\\" + FileName);

                // Checking if file exists
                if (filepolicyuploadSFerror.Exists)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats - officedocument.spreadsheetml.sheet";
                    // Response.ContentType = "application/xlsx";
                    //EAISA-5 Veracode flaw fix 12072017
                    Response.AddHeader("content-disposition", "attachment;filename=" + filepolicyuploadSFerror.Name);

                    Response.AddHeader("Content-Length", filepolicyuploadSFerror.Length.ToString());

                    //modalPolDetails.Show();

                    //EAISA-5 Veracode flaw fix 12072017
                    Response.TransmitFile(filepolicyuploadSFerror.FullName);
                    //Response.TransmitFile(Server.MapPath("PolicyErrorLogTempPath\\"+filepolicyuploaderror.Name));
                    Response.Flush();
                    //Session.Abandon();
                    //Response.End();
                    // Clear the content of the response
                    //Response.ClearContent();

                    //// LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                    //Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);
                    ////Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                    //// Add the file size into the response header
                    //Response.AddHeader("Content-Length", file.Length.ToString());

                    //// Set the ContentType
                    //Response.ContentType = "application/vnd.ms-excel";

                    //// Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                    //Response.TransmitFile(file.FullName);

                    ////Response.BufferOutput = true;
                    //Response.Flush();
                    ////Response.Close();
                    //// End the response
                    //Response.End();
                }
                else
                {
                    ShowError("No Error Log to be Downloaded");
                }
            }
            catch (Exception ex)
            {
                //ProPrgTransWrapper.RollbackChanges();
                ShowError(ex.Message, ex);
            }

        }

        protected void btnErrorLog_PolicyClick(object sender, EventArgs e)
        {
            // btnErrorLog_Policy.Visible = false;
            try
            {
                string FileName = string.Empty;
                string FilePath = string.Empty;
                if (Session["strMessageErrorLog_PolicyUpload"] != null && Session["strMessageErrorLog_PolicyUpload"] != string.Empty)
                {
                    string[] SessionValue = Session["strMessageErrorLog_PolicyUpload"].ToString().Split('|');
                    FileName = SessionValue[0];
                    FilePath = SessionValue[1];

                }
                //if (ddlRoleMass.Visible == true)
                //{
                //    FilePath = ConfigurationManager.AppSettings["MassReassignTemplatePath"] + "\\" + ConfigurationManager.AppSettings["MultipleUserUploadTemplateName"];
                //}
                //else
                //    FilePath = ConfigurationManager.AppSettings["MassReassignTemplatePath"] + "\\" + ConfigurationManager.AppSettings["MassReassignTemplateName"];

                FileInfo filepolicyuploaderror = new FileInfo(FilePath + "\\" + FileName);
                //FileInfo filepolicyuploaderror = new FileInfo(FilePath + "\\" + FileName);

                // Checking if file exists
                if (filepolicyuploaderror.Exists)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    //Response.ContentType = "application/vnd.ms-excel";
                    Response.ContentType = "application/vnd.openxmlformats - officedocument.spreadsheetml.sheet";
                    // Response.ContentType = "application/xlsx";
                    //EAISA-5 Veracode flaw fix 12072017
                    Response.AddHeader("content-disposition", "attachment;filename=" + filepolicyuploaderror.Name);

                    Response.AddHeader("Content-Length", filepolicyuploaderror.Length.ToString());

                    //modalPolDetails.Show();

                    //EAISA-5 Veracode flaw fix 12072017
                    Response.TransmitFile(filepolicyuploaderror.FullName);
                    //Response.TransmitFile(Server.MapPath("PolicyErrorLogTempPath\\"+filepolicyuploaderror.Name));
                    Response.Flush();
                    //Response.End();
                    // Clear the content of the response
                    //Response.ClearContent();

                    //// LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                    //Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);
                    ////Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                    //// Add the file size into the response header
                    //Response.AddHeader("Content-Length", file.Length.ToString());

                    //// Set the ContentType
                    //Response.ContentType = "application/vnd.ms-excel";

                    //// Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                    //Response.TransmitFile(file.FullName);

                    ////Response.BufferOutput = true;
                    //Response.Flush();
                    ////Response.Close();
                    //// End the response
                    //Response.End();
                }
                else
                {
                    ShowError("No Error Log to be Downloaded");
                }
            }
            catch (Exception ex)
            {
                //ProPrgTransWrapper.RollbackChanges();
                ShowError(ex.Message, ex);
            }

        }
        protected void btnPopCancel_Click(object sender, EventArgs e)
        {
            if (Session["strMessageErrorLog_PolicyUpload"] != null && Session["strMessageErrorLog_PolicyUpload"] != string.Empty)
            {
                Session["strMessageErrorLog_PolicyUpload"] = null;
                btnErrorLog_Policy.Visible = false;
            }
            modalPolDetails.Hide();
        }
        /// <summary>
        /// User when click s on Copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //User wants to copy the existing record and make a new record.
            //making the CheckNew boolean to true
            isNew = true;
            isCopy = true;
            btnSave.Text = "Save";
            btnCopy.Enabled = false;
            //increment Policy Mod, Policy Effective Year and Policy Expiration Year of the target Policy Information by one
            // following lines are commented by naresh as a part of new requirement 
            //txtPolicyMod.Text = (Convert.ToInt32(txtPolicyMod.Text) + 1).ToString();
            //txtPolicyEff.Text = (DateTime.Parse(txtPolicyEff.Text).AddYears(1).ToShortDateString());
            //txtPolicyExp.Text = (DateTime.Parse(txtPolicyExp.Text).AddYears(1).ToShortDateString());
        }
        /// <summary>
        /// When user clicks on New
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ViewState["SelectedIndex"] != null)
            {
                int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                if (lstPolicyInfo.Items.Count > index)
                {
                    HtmlTableRow tr = (HtmlTableRow)lstPolicyInfo.Items[index].FindControl("trItemTemplate");
                    tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
                }
            }
            ViewState["SelectedIndex"] = null;

            chkTPADirect.InputAttributes.Add("disabled", "disabled");

            chkTPA.Attributes.Add("onclick", "EnablechkTPADirect()");
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                btnSave.Enabled = true;
                btnCopy.Enabled = false;
                //btnCancel.Enabled = false;
                btnCancel.Enabled = true;
            }
            isNew = true;
            isCopy = false;
            ClearFileds();

            lstPolicyInfo.Enabled = false;
            PnlProgramperiod.Enabled = false;
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnAdd.Enabled = false;


            lblPolicyDetails.Visible = true;
            lbClosePolicyDetails.Visible = true;
            pnlDetails.Visible = true;
            btnSave.Text = "Save";
        }
        protected void lstPolicyInfo_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                isNew = false;
                //btnSFUpload.Visible = true;
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                {
                    btnSave.Enabled = true;
                    btnCopy.Enabled = true;
                    btnCancel.Enabled = true;
                }
                int intPolicyID = Convert.ToInt32(e.CommandArgument);
                //int intPolicyID = int.Parse(((Label)e.FindControl("lblPolicy")).Text);

                SelectedPolicyID = intPolicyID;
                //SelectedIndex = e.NewSelectedIndex;
                DisplayDetails(intPolicyID);

                lstPolicyInfo.Enabled = false;
                PnlProgramperiod.Enabled = false;

                lblPolicyDetails.Visible = true;
                lbClosePolicyDetails.Visible = true;
                pnlDetails.Visible = true;

                lblSteppedFactor.Visible = false;
                lbCloseSteppedFactor.Visible = false;
                pnlSteppedFactor.Visible = false;


                //LinkButton lbSteppedFactor = (LinkButton)e.Item.FindControl("lbSteppedFactor");
                //Label lblPolicyActive = (Label)e.Item.FindControl("lblPolicyActive");
                //if (lbSteppedFactor != null)
                //{
                //    Label lblSteppedcheck = (Label)e.Item.FindControl("lblSteppedcheck");
                //    if ((lblSteppedcheck.Text == "True") && (lblPolicyActive.Text == "True"))
                //    {
                //        //if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                //            //btnSFUpload.Visible = true;
                //    }
                //    else
                //        btnSFUpload.Visible = false;
                //}

            }
            try
            {
                if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
                {
                    int policyID = Convert.ToInt32(e.CommandArgument);
                    polBE = PolicyBizService.getPolicyInfo(policyID);
                    if (e.CommandName.ToUpper() == "DISABLE")
                    {
                        polBE.IsActive = false;
                        polBE.UPDATE_DATE = DateTime.Now;
                        polBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                        bool boolIsPolicySaved = PolicyBizService.Update(polBE);
                        //if (boolIsPolicySaved) 
                        AuditBizService.Save(AISMasterEntities.AccountNumber, SelectedProgramPeriodID, GlobalConstants.AuditingWebPage.PolicyInfo, CurrentAISUser.PersonID);
                        //ProPrgTransWrapper.SubmitTransactionChanges();
                    }
                    else if (e.CommandName.ToUpper() == "ENABLE")
                    {
                        bool isPolExistsInAnyAcct = PolicyBizService.isPolExistsInAnyAcct(polBE.PolicyID, polBE.PolicySymbol, polBE.PolicyNumber, polBE.PolicyModulus, Convert.ToDateTime(polBE.PolicyEffectiveDate), Convert.ToDateTime(polBE.PlanEndDate), AISMasterEntities.AccountNumber);
                        if (isPolExistsInAnyAcct == false)
                        {
                            polBE.IsActive = true;
                            polBE.UPDATE_DATE = DateTime.Now;
                            polBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                            bool boolIsPolicySaved = PolicyBizService.Update(polBE);
                            //if (boolIsPolicySaved) 
                            AuditBizService.Save(AISMasterEntities.AccountNumber, SelectedProgramPeriodID, GlobalConstants.AuditingWebPage.PolicyInfo, CurrentAISUser.PersonID);
                            //ProPrgTransWrapper.SubmitTransactionChanges();
                        }
                        else
                            ShowMessage("The record cannot be enabled. A policy number with an identical Symbol, Policy Number, Modulus, Effective Date, Expiration Date cannot span two Accounts");
                    }


                    BindPolicyInformation();
                    UpdateRetroInfo();
                }
            }
            catch (Exception ex)
            {
                //ProPrgTransWrapper.RollbackChanges();
                ShowError(ex.Message, ex);
            }
        }
        protected void btnCancel_Click(Object sender, EventArgs e)
        {
            if (isNew)
            {
                if (isCopy)
                {
                    isNew = false;
                    isCopy = false;
                    btnSave.Text = "Update";
                    ShowMessage("Policy Information Copy process has been Cancelled");
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        btnCopy.Enabled = true;
                }
                else
                {
                    { ClearFileds(); }
                }

            }
            else
            {
                DisplayDetails(SelectedPolicyID);
            }

        }
        protected void lbClosePolicyDetails_Click(object sender, EventArgs e)
        {
            lstPolicyInfo.Enabled = true;
            PnlProgramperiod.Enabled = true;
            lblPolicyDetails.Visible = false;
            lbClosePolicyDetails.Visible = false;
            pnlDetails.Visible = false;

            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnAdd.Enabled = true;

            //btnSFUpload.Visible = false;
        }

        #endregion Policy Details

        #region Stepped Factor

        private SteppedFactorBS steppedFactorBizService;
        private SteppedFactorBS SteppedFactorBizService
        {
            get
            {
                if (steppedFactorBizService == null)
                {
                    steppedFactorBizService = new SteppedFactorBS();
                    // steppedFactorBizService.AppTransactionWrapper = PolicyInfoTransWrapper;
                }
                return steppedFactorBizService;
            }
        }
        /// <summary>
        /// Invokes when user clicks on SteppedFactor LinkButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SteppedFactor(object sender, CommandEventArgs e)
        {
            SelectedPolicyID = Convert.ToInt32(e.CommandArgument);

            BindSteppedFactorListView();

            lstPolicyInfo.Enabled = false;
            PnlProgramperiod.Enabled = false;
            btnAdd.Enabled = false;

            lblPolicyDetails.Visible = false;
            lbClosePolicyDetails.Visible = false;
            pnlDetails.Visible = false;

            lblSteppedFactor.Visible = true;
            lbCloseSteppedFactor.Visible = true;
            pnlSteppedFactor.Visible = true;


            //btnSFUpload.Visible = true;
        }
        /// <summary>
        /// Binds the data to Stepped  Factor List view
        /// </summary>
        private void BindSteppedFactorListView()
        {

            lstSteppedFactor.DataSource = SteppedFactorBizService.GetSteppedFactorData(SelectedPolicyID);
            lstSteppedFactor.DataBind();
            polBE = PolicyBizService.getPolicyInfo(SelectedPolicyID);
            //veracode fix
            lblSteppedFactor.Text = ("Stepped Factors for Policy # " + Server.HtmlEncode(polBE.PolicySymbol.Trim()) + ' ' + Server.HtmlEncode(polBE.PolicyNumber.Trim()) + ' ' + Server.HtmlEncode(polBE.PolicyModulus.Trim()));
            if (lstSteppedFactor.InsertItemPosition != InsertItemPosition.None)
            {
                TextBox txtLDFFactor = (TextBox)lstSteppedFactor.InsertItem.FindControl("txtLDFFactor");
                TextBox txtIBNRFactor = (TextBox)lstSteppedFactor.InsertItem.FindControl("txtIBNRFactor");
                if ((txtLDFFactor != null) && (txtIBNRFactor != null))
                {
                    txtLDFFactor.Attributes.Add("onblur", "DisableOtherFactor(" + txtLDFFactor.ClientID + ", " + txtIBNRFactor.ClientID + ")");
                    txtIBNRFactor.Attributes.Add("onblur", "DisableOtherFactor(" + txtLDFFactor.ClientID + ", " + txtIBNRFactor.ClientID + ")");
                }
            }
        }

        protected void lstSteppedFactor_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            if (e.CancelMode == ListViewCancelMode.CancelingEdit)
            {
                lstSteppedFactor.EditIndex = -1;
                lstSteppedFactor.InsertItemPosition = InsertItemPosition.FirstItem;
                BindSteppedFactorListView();
            }
            else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
            {
                //Back to normal mode.            
                lstSteppedFactor.InsertItemPosition = InsertItemPosition.None;
                BindSteppedFactorListView();
            }
        }

        protected void lstSteppedFactor_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lstSteppedFactor.EditIndex = e.NewEditIndex;
            lstSteppedFactor.InsertItemPosition = InsertItemPosition.None;
            BindSteppedFactorListView();
        }

        protected void lstSteppedFactor_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "UPDATE"))
                {
                    TextBox txtMonthsToVal = (TextBox)e.Item.FindControl("txtMonthsToVal");
                    TextBox txtLDFFactor = (TextBox)e.Item.FindControl("txtLDFFactor");
                    TextBox txtIBNRFactor = (TextBox)e.Item.FindControl("txtIBNRFactor");
                    CheckBox chkSteppedFactor = (CheckBox)e.Item.FindControl("chkSteppedFactor");

                    SteppedFactorBE stepBE = new SteppedFactorBE();

                    if (e.CommandName.ToUpper() == "UPDATE")
                    {
                        Label lblSTEPPED_FACTOR_ID = (Label)e.Item.FindControl("lblSTEPPED_FACTOR_ID");
                        int intSTEPPED_FACTOR_ID = int.Parse(lblSTEPPED_FACTOR_ID.Text);
                        stepBE = SteppedFactorBizService.getGetSteppedFactor(intSTEPPED_FACTOR_ID);

                        stepBE.MONTHS_TO_VAL = Convert.ToInt32(txtMonthsToVal.Text);
                        stepBE.MONTHS_TO_VAL = Convert.ToInt32(txtMonthsToVal.Text);
                        if (txtLDFFactor.Text == "")
                            stepBE.LDF_FACTOR = null;
                        else
                            stepBE.LDF_FACTOR = Convert.ToDecimal(txtLDFFactor.Text);
                        if (txtIBNRFactor.Text == "")
                            stepBE.IBNR_FACTOR = null;
                        else
                            stepBE.IBNR_FACTOR = Convert.ToDecimal(txtIBNRFactor.Text);

                        stepBE.UPDATE_DATE = DateTime.Now;
                        stepBE.UPDATE_USER_ID = CurrentAISUser.PersonID;

                        bool Flag = SteppedFactorBizService.Update(stepBE);
                        ShowConcurrentConflict(Flag, stepBE.ErrorMessage);

                        // get out of the edit mode
                        lstSteppedFactor.InsertItemPosition = InsertItemPosition.FirstItem;
                        lstSteppedFactor.EditIndex = -1;
                    }
                    else if (e.CommandName.ToUpper() == "SAVE")
                    {
                        stepBE.POLICY_ID = SelectedPolicyID;
                        stepBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                        stepBE.PREM_ADJ_PGM_ID = SelectedProgramPeriodID;

                        stepBE.MONTHS_TO_VAL = Convert.ToInt32(txtMonthsToVal.Text);
                        if (txtLDFFactor.Text == "")
                            stepBE.LDF_FACTOR = null;
                        else
                            stepBE.LDF_FACTOR = Convert.ToDecimal(txtLDFFactor.Text);
                        if (txtIBNRFactor.Text == "")
                            stepBE.IBNR_FACTOR = null;
                        else
                            stepBE.IBNR_FACTOR = Convert.ToDecimal(txtIBNRFactor.Text);
                        stepBE.CREATE_DATE = DateTime.Now;
                        stepBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        stepBE.ISACTIVE = true;

                        SteppedFactorBizService.Update(stepBE);
                    }

                    BindSteppedFactorListView();
                }
                if ((e.CommandName.ToUpper() == "DISABLESTEPPED") || (e.CommandName.ToUpper() == "ENABLESTEPPED"))
                {
                    SteppedFactorBE stepBE = new SteppedFactorBE();
                    int intSTEPPED_FACTOR_ID = Convert.ToInt32(e.CommandArgument);
                    stepBE = SteppedFactorBizService.getGetSteppedFactor(intSTEPPED_FACTOR_ID);
                    if (e.CommandName.ToUpper() == "DISABLESTEPPED")
                        stepBE.ISACTIVE = false;
                    else
                        stepBE.ISACTIVE = true;

                    stepBE.UPDATE_DATE = DateTime.Now;
                    stepBE.UPDATE_USER_ID = CurrentAISUser.PersonID;

                    bool boolIsSteepSaved = SteppedFactorBizService.Update(stepBE);

                    //if (boolIsPolicySaved) AuditBizService.Save(AISMasterEntities.AccountNumber, prpPreID, GlobalConstants.AuditingWebPage.ProgramPeriodSetup, CurrentAISUser.PersonID);

                    //ProPrgTransWrapper.SubmitTransactionChanges();
                    BindSteppedFactorListView();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, ex);
            }
        }
        protected void lbCloseSteppedFactor_Click(object sender, EventArgs e)
        {
            lstPolicyInfo.Enabled = true;
            PnlProgramperiod.Enabled = true;
            PnlProgramperiod.Enabled = true;

            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnAdd.Enabled = true;


            lblSteppedFactor.Visible = false;
            lbCloseSteppedFactor.Visible = false;
            pnlSteppedFactor.Visible = false;
        }
        protected void lstSteppedFactor_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgSteppedFactorDisable = (ImageButton)e.Item.FindControl("imgSteppedFactorDisable");

                if (imgSteppedFactorDisable != null)
                {
                    Label lblSPActive = (Label)e.Item.FindControl("lblSPActive");
                    if (lblSPActive.Text == "True")
                    {
                        imgSteppedFactorDisable.ImageUrl = "~/images/disabled.GIF";
                        imgSteppedFactorDisable.CommandName = "DISABLESTEPPED";
                        imgSteppedFactorDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable?');");
                        imgSteppedFactorDisable.ToolTip = "Click here to Disable this Stepped Factor";
                    }
                    else
                    {
                        imgSteppedFactorDisable.ImageUrl = "~/images/enabled.GIF";
                        imgSteppedFactorDisable.CommandName = "ENABLESTEPPED";
                        imgSteppedFactorDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable?');");
                        imgSteppedFactorDisable.ToolTip = "Click here to Enable this Stepped Factor";
                    }
                }

                TextBox txtLDFFactor = (TextBox)e.Item.FindControl("txtLDFFactor");
                TextBox txtIBNRFactor = (TextBox)e.Item.FindControl("txtIBNRFactor");
                if ((txtLDFFactor != null) && (txtIBNRFactor != null))
                {
                    if (txtLDFFactor.Text != "") txtLDFFactor.Text = Convert.ToDecimal(txtLDFFactor.Text).ToString("0.000000");
                    if (txtIBNRFactor.Text != "") txtIBNRFactor.Text = Convert.ToDecimal(txtIBNRFactor.Text).ToString("0.000000");

                    txtLDFFactor.Attributes.Add("onblur", "DisableOtherFactor(" + txtLDFFactor.ClientID + ", " + txtIBNRFactor.ClientID + ")");
                    txtIBNRFactor.Attributes.Add("onblur", "DisableOtherFactor(" + txtLDFFactor.ClientID + ", " + txtIBNRFactor.ClientID + ")");

                    if (txtLDFFactor.Text != "") txtIBNRFactor.Enabled = false;
                    if (txtIBNRFactor.Text != "") txtLDFFactor.Enabled = false;

                }

                Label lblLDFFactor = (Label)e.Item.FindControl("lblLDFFactor");
                Label lblIBNRFactor = (Label)e.Item.FindControl("lblIBNRFactor");
                if ((lblLDFFactor != null) && (lblIBNRFactor != null))
                {
                    if (lblLDFFactor.Text != "") lblLDFFactor.Text = Convert.ToDecimal(lblLDFFactor.Text).ToString("0.000000");
                    if (lblIBNRFactor.Text != "") lblIBNRFactor.Text = Convert.ToDecimal(lblIBNRFactor.Text).ToString("0.000000");
                }


            }


        }
        protected void lstSteppedFactor_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        { }
        #endregion Stepped Factor

        public void UpdateRetroInfo()
        {
            int prmPerdID = SelectedProgramPeriodID;
            IList<RetroInfoBE> retroBE = (new RetroInfoBS()).GetRetroInfo(prmPerdID);
            bool Flag = false;
            decimal? dclMaxAmt = 0;
            for (int i = 0; i < retroBE.Count; i++)
            {
                if (retroBE[i].ADJ_RETRO_INFO_ID <= 0)
                {
                    return;
                }
                RetroInfoBE retroinfoBE = (new RetroInfoBS()).LoadData(retroBE[i].ADJ_RETRO_INFO_ID);


                if (retroinfoBE.EXPO_TYP_ID == null || retroinfoBE.EXPO_TYP_ID <= 0)//if nothing is there in case of N/A checkbox
                {
                    retroinfoBE.AUDT_EXPO_AMT = 0;
                }
                else if (retroinfoBE.EXPO_TYP_ID == 127 && i != 0)//Basic Premium
                {
                    retroinfoBE.AUDT_EXPO_AMT = dclMaxAmt;
                }
                else if (retroinfoBE.EXPO_TYP_ID == 131)//payroll
                {
                    retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetPayrollAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
                }
                else if (retroinfoBE.EXPO_TYP_ID == 128)//combined elements
                {
                    retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetCombinedAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
                }
                else if (retroinfoBE.EXPO_TYP_ID == 135)//standard premium
                {
                    retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetStandardAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
                }
                else if (retroinfoBE.EXPO_TYP_ID != 130)// Manual Premium
                {
                    retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetOtherAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
                }
                if (retroinfoBE.EXPO_TYP_INCREMNT_NBR_ID != null && retroinfoBE.EXPO_TYP_INCREMNT_NBR_ID > 0)
                {
                    retroinfoBE.TOT_AUDT_AMT = Math.Round(decimal.Parse(((retroinfoBE.AUDT_EXPO_AMT * retroinfoBE.AGGR_FCTR_PCT) / decimal.Parse(retroBE[i].EXP_BASIS)).ToString()));
                }
                else
                {
                    retroinfoBE.TOT_AUDT_AMT = null;
                }
                if (i == 0)
                {
                    if (retroinfoBE.TOT_AGMT_AMT == null && retroinfoBE.TOT_AUDT_AMT == null)
                        dclMaxAmt = null;
                    else
                    {
                        // This will ensure that a null value will not be used in calculating the dclMaxAmt (that was previously causing errors)
                        decimal? TOT_AGMT_AMT = retroinfoBE.TOT_AGMT_AMT != null ? retroinfoBE.TOT_AGMT_AMT : 0;
                        decimal? TOT_AUDT_AMT = retroinfoBE.TOT_AUDT_AMT != null ? retroinfoBE.TOT_AUDT_AMT : 0;

                        dclMaxAmt = TOT_AGMT_AMT > TOT_AUDT_AMT ? Math.Round(decimal.Parse(TOT_AGMT_AMT.ToString())) : Math.Round(decimal.Parse(TOT_AUDT_AMT.ToString()));
                    }
                }
                retroinfoBE.UPDT_DT = DateTime.Now;
                retroinfoBE.UPDT_USER_ID = CurrentAISUser.PersonID;
                Flag = (new RetroInfoBS()).SaveRetroData(retroinfoBE);
            }
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblErrorLog.Visible = false;
            lblErrorLog.Text = "";
            modalPolDetails.Show();
        }


        protected void btnSFUpload_Click(object sender, EventArgs e)
        {
            lblSFErrorMessage.Visible = false;
            lblSFErrorMessage.Text = "";
            mpeStepped.Show();
        }
        //Policy Upload button

        protected void PolicyUpload_Click(object sender, EventArgs e)
        {
            if (flUpload.HasFile)
            {
                if (Path.GetExtension(flUpload.PostedFile.FileName) == sSpreadsheetGearExcelFileFormat)   // AWS Cloud Migration
                {
                    //string strDate = DateTime.Now.ToString().Replace(":", "-");
                    //strDate = strDate.Replace("/", "-");
                    string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                    string strTime = DateTime.Now.ToString("HH-mm");

                    string fileName = "PEOPolicyUpload_" + CurrentAISUser.PersonID.ToString() + "_" + SelectedProgramPeriodID + "_" + strDate + "-" + strTime + sSpreadsheetGearExcelFileFormat; // AWS Cloud Migration

                    if (!(Directory.Exists(ConfigurationManager.AppSettings["PolicyFilesTempPath"])))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["PolicyFilesTempPath"]);
                    }

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    flUpload.SaveAs(ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + fileName);

                    //full path name
                    string filename = ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + fileName;
                    policyBizService = new PolicyBS();
                    DataTable excelSheetDT = new DataTable();

                    try
                    {
                        excelSheetDT = GetDataTableExcelWithOutOledb(filename);


                        DataTable errorLogDT = excelSheetDT.Clone();
                        errorLogDT.Columns.Add("Comments", typeof(System.String));

                        string strError = "";

                        int success = 0;


                        for (int i = 0; i < excelSheetDT.Rows.Count; i++)
                        {
                            bool isErrorRowImported = false;
                            bool isErrorCommentAdded = false;
                            polBE = new PolicyBE();

                            polBE.cstmrid = AISMasterEntities.AccountNumber;
                            polBE.ProgramPeriodID = SelectedProgramPeriodID;
                            polBE.CREATE_DATE = DateTime.Now;
                            polBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                            polBE.IsActive = true;

                            string strPolicySymbol = Convert.ToString(excelSheetDT.Rows[i][0]);
                            if (ValidatePolicySymbol(strPolicySymbol.Trim(), out strError))
                            {
                                polBE.PolicySymbol = strPolicySymbol.Trim();
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

                            string strPolicyNumber = Convert.ToString(excelSheetDT.Rows[i][1]);
                            if (ValidatePolicyNumber(strPolicyNumber.Trim(), out strError))
                            {
                                if (strPolicyNumber.Length == 7)
                                    strPolicyNumber = "0" + strPolicyNumber;
                                polBE.PolicyNumber = strPolicyNumber.Trim();
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


                            string strPolicyModule = Convert.ToString(excelSheetDT.Rows[i][2]);
                            if (ValidatePolicyModule(strPolicyModule.Trim(), out strError))
                            {
                                if (strPolicyModule.Length == 1)
                                    strPolicyModule = "0" + strPolicyModule;
                                polBE.PolicyModulus = strPolicyModule.Trim();
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

                            string strPolicyEffDate = Convert.ToString(excelSheetDT.Rows[i][3]);
                            strPolicyEffDate = strPolicyEffDate.Trim();
                            if (ValidatePolicyEffDate(strPolicyEffDate, out strError))
                            {
                                polBE.PolicyEffectiveDate = Convert.ToDateTime(strPolicyEffDate);
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

                            string strPolicyExpDate = Convert.ToString(excelSheetDT.Rows[i][4]);
                            strPolicyExpDate = strPolicyExpDate.Trim();
                            if (ValidatePolicyExpDate(strPolicyExpDate, out strError, Convert.ToDateTime(polBE.PolicyEffectiveDate)))
                            {
                                polBE.PlanEndDate = Convert.ToDateTime(strPolicyExpDate);
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

                            int lookupID = 0;
                            string strCoverageTypeID = Convert.ToString(excelSheetDT.Rows[i][5]);
                            if (!string.IsNullOrWhiteSpace(strCoverageTypeID))
                            {
                                if (ValidateLookUp(strCoverageTypeID.Trim(), "LOB COVERAGE", out strError, out lookupID))
                                {
                                    polBE.CoverageTypeID = lookupID;
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
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "Coverage type should not be blank";

                                }
                                else
                                {
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Coverage type should not be blank";
                                    isErrorCommentAdded = true;
                                }
                                //continue;
                            }

                            string strAdjustmentType = Convert.ToString(excelSheetDT.Rows[i][6]);
                            if (!string.IsNullOrWhiteSpace(strAdjustmentType))
                            {
                                if (ValidateLookUp(strAdjustmentType.Trim(), "ADJUSTMENT TYPE", out strError, out lookupID))
                                {
                                    polBE.AdjusmentTypeID = lookupID;
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
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "Adjustment type should not be blank";

                                }
                                else
                                {
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Adjustment type should not be blank";
                                    isErrorCommentAdded = true;
                                }
                                //continue;
                            }

                            string strLossSource = Convert.ToString(excelSheetDT.Rows[i][7]);
                            if (!string.IsNullOrWhiteSpace(strLossSource))
                            {
                                if (ValidateLookUp(strLossSource.Trim(), "LOSS SOURCE", out strError, out lookupID))
                                {
                                    polBE.LossSystemSourceID = lookupID;
                                    polBE.LossSourceName = strLossSource.Trim();
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
                            }

                            string strUnlimDedtblPolLimitIndicator = Convert.ToString(excelSheetDT.Rows[i][8]);
                            if (!string.IsNullOrEmpty(strUnlimDedtblPolLimitIndicator))
                            {
                                strUnlimDedtblPolLimitIndicator = strUnlimDedtblPolLimitIndicator.Trim();
                                if (ValidateTrueFalse(strUnlimDedtblPolLimitIndicator, "Unlimited Pol Ded Limit", out strError))
                                {
                                    polBE.UnlimDedtblPolLimitIndicator = Convert.ToBoolean(strUnlimDedtblPolLimitIndicator);
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
                            }

                            //here need to add Deductible Pol Limit Amt logic - column 9

                            string strDedTblPolicyLimitAmount = Convert.ToString(excelSheetDT.Rows[i][9]);
                            if (!string.IsNullOrWhiteSpace(strDedTblPolicyLimitAmount))
                            {
                                strDedTblPolicyLimitAmount = strDedTblPolicyLimitAmount.Trim();
                                if (ValidateAmount(strDedTblPolicyLimitAmount, "Deductible Pol Limit Amt", out strError))
                                {

                                    strDedTblPolicyLimitAmount = Convert.ToDecimal(strDedTblPolicyLimitAmount).ToString("0.00");
                                    polBE.DedTblPolicyLimitAmount = Convert.ToDecimal(strDedTblPolicyLimitAmount);
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
                            }


                            string strALAETypeID = Convert.ToString(excelSheetDT.Rows[i][10]);
                            if (!string.IsNullOrWhiteSpace(strALAETypeID))
                            {
                                if (ValidateLookUp(strALAETypeID.Trim(), "ALAE TYPE", out strError, out lookupID))
                                {
                                    polBE.ALAETypeID = lookupID;
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
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "ALAE Type should not be blank";

                                }
                                else
                                {
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "ALAE Type should not be blank";
                                    isErrorCommentAdded = true;
                                }

                                //continue;
                            }

                            //here need to add ALAE Capped Amt logic - column 11


                            if (!string.IsNullOrWhiteSpace(strALAETypeID) && strALAETypeID.ToLower() == "capped amount")
                            {
                                string strALAECappedAmount = Convert.ToString(excelSheetDT.Rows[i][11]);
                                if (!string.IsNullOrWhiteSpace(strALAECappedAmount))
                                {
                                    strALAECappedAmount = strALAECappedAmount.Trim();
                                    if (ValidateAmount(strALAECappedAmount, "ALAE Capped Amt", out strError))
                                    {
                                        strALAECappedAmount = Convert.ToDecimal(strALAECappedAmount).ToString("0.00");
                                        polBE.ALAECappedAmount = Convert.ToDecimal(strALAECappedAmount);
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
                                }
                            }
                            else
                            {
                                polBE.ALAECappedAmount = 0;
                            }

                            string strUnlimOverrideDedtblLimitIndicator = Convert.ToString(excelSheetDT.Rows[i][12]);
                            if (!string.IsNullOrWhiteSpace(strUnlimOverrideDedtblLimitIndicator))
                            {
                                strUnlimOverrideDedtblLimitIndicator = strUnlimOverrideDedtblLimitIndicator.Trim();
                                if (ValidateTrueFalse(strUnlimOverrideDedtblLimitIndicator, "Unlimited Override Ded Limit", out strError))
                                {
                                    polBE.UnlimOverrideDedtblLimitIndicator = Convert.ToBoolean(strUnlimOverrideDedtblLimitIndicator);
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
                            }

                            //here need to add Override Ded Limit Amtlogic - column 13
                            string strOverrideDedtblLimitAmount = Convert.ToString(excelSheetDT.Rows[i][13]);
                            if (!string.IsNullOrWhiteSpace(strOverrideDedtblLimitAmount))
                            {
                                strOverrideDedtblLimitAmount = strOverrideDedtblLimitAmount.Trim();
                                if (ValidateAmount(strOverrideDedtblLimitAmount, "Override Ded Limit Amt", out strError))
                                {
                                    strOverrideDedtblLimitAmount = Convert.ToDecimal(strOverrideDedtblLimitAmount).ToString("0.00");
                                    polBE.OverrideDedtblLimitAmount = Convert.ToDecimal(strOverrideDedtblLimitAmount);
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
                            }

                            string strLDFIncurredNotReport = Convert.ToString(excelSheetDT.Rows[i][14]);
                            if (!string.IsNullOrWhiteSpace(strLDFIncurredNotReport))
                            {
                                strLDFIncurredNotReport = strLDFIncurredNotReport.Trim();
                                if (ValidateTrueFalse(strLDFIncurredNotReport, "LDF/IBNR Incl Limit", out strError))
                                {
                                    polBE.LDFIncurredNotReport = Convert.ToBoolean(strLDFIncurredNotReport);
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
                            }

                            string strLDFIncurredNO63740 = Convert.ToString(excelSheetDT.Rows[i][15]);
                            if (!string.IsNullOrWhiteSpace(strLDFIncurredNO63740))
                            {
                                strLDFIncurredNO63740 = strLDFIncurredNO63740.Trim();
                                if (ValidateTrueFalse(strLDFIncurredNO63740, "LDF/IBNR Stepped", out strError))
                                {
                                    polBE.LDFIncurredNO63740 = Convert.ToBoolean(strLDFIncurredNO63740);
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
                            }

                            string strLDFFactor = Convert.ToString(excelSheetDT.Rows[i][16]);
                            if (!string.IsNullOrWhiteSpace(strLDFFactor))
                            {
                                strLDFFactor = strLDFFactor.Trim();
                                if (ValidateDecimals(strLDFFactor, "LDF", out strError))
                                {
                                    polBE.LDFFactor = Convert.ToDecimal(strLDFFactor);
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
                            }

                            string strIBNRFactor = Convert.ToString(excelSheetDT.Rows[i][17]);
                            if (!string.IsNullOrWhiteSpace(strIBNRFactor))
                            {
                                strIBNRFactor = strIBNRFactor.Trim();
                                if (ValidateDecimals(strIBNRFactor, "IBNR", out strError))
                                {
                                    polBE.IBNRFactor = Convert.ToDecimal(strIBNRFactor);
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
                            }

                            //need to add DEP State logic here - column 18

                            if (strAdjustmentType.ToUpper().StartsWith("DEP"))
                            {
                                string strDedtblProtPolicyStID = Convert.ToString(excelSheetDT.Rows[i][18]);

                                if (!string.IsNullOrWhiteSpace(strDedtblProtPolicyStID))
                                {
                                    strDedtblProtPolicyStID = strDedtblProtPolicyStID.Trim();
                                    if (ValidateLookUp(strDedtblProtPolicyStID, "STATE", out strError, out lookupID))
                                    {
                                        polBE.DedtblProtPolicyStID = lookupID;
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
                                }
                            }


                            string strTPAIndicator = Convert.ToString(excelSheetDT.Rows[i][19]);
                            if (!string.IsNullOrWhiteSpace(strTPAIndicator))
                            {
                                strTPAIndicator = strTPAIndicator.Trim();

                                if (ValidateTrueFalse(strTPAIndicator, "TPA", out strError))
                                {
                                    polBE.TPAIndicator = Convert.ToBoolean(strTPAIndicator);
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
                            }
                            else
                            {
                                polBE.TPAIndicator = false;
                            }

                            if (polBE.TPAIndicator != null)
                            {
                                if (polBE.TPAIndicator == true)
                                {
                                    string strTPADirectIndicator = Convert.ToString(excelSheetDT.Rows[i][20]);
                                    if (!string.IsNullOrWhiteSpace(strTPADirectIndicator))
                                    {
                                        strTPADirectIndicator = strTPADirectIndicator.Trim();
                                        if (ValidateTrueFalse(strTPADirectIndicator, "TPA Direct", out strError))
                                        {
                                            polBE.TPADirectIndicator = Convert.ToBoolean(strTPADirectIndicator);
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
                                    }
                                    else
                                    {
                                        polBE.TPADirectIndicator = false;
                                    }
                                }
                                else
                                {
                                    polBE.TPADirectIndicator = false;
                                }
                            }
                            else
                            {
                                polBE.TPADirectIndicator = false;
                            }

                            string strISMasterPEOPolicy = excelSheetDT.Rows[i][21].ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(strISMasterPEOPolicy))
                            {
                                strISMasterPEOPolicy = strISMasterPEOPolicy.Trim();
                                if (ValidateTrueFalse(strISMasterPEOPolicy, "Master PEO Policy", out strError))
                                {
                                    polBE.ISMasterPEOPolicy = Convert.ToBoolean(strISMasterPEOPolicy);
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
                            }

                            // Non Conv.
                            //---------

                            //AISAmount validation textbox  - column 22

                            string strNonConversionAmount = Convert.ToString(excelSheetDT.Rows[i][22]);

                            if (!string.IsNullOrWhiteSpace(strNonConversionAmount))
                            {
                                strNonConversionAmount = strNonConversionAmount.Trim();
                                if (ValidateAmount(strNonConversionAmount, "Non Conv", out strError))
                                {
                                    strNonConversionAmount = Convert.ToDecimal(strNonConversionAmount).ToString("0.00");
                                    polBE.NonConversionAmount = Convert.ToDecimal(strNonConversionAmount);
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
                            }
                            //else
                            //{
                            //    polBE.NonConversionAmount = 0;
                            //}



                            //Other Amt
                            //---------

                            //AISAmount validation textbox  - column 23

                            string strOtherPolicyAdjustmentAmount = Convert.ToString(excelSheetDT.Rows[i][23]);
                            if (!string.IsNullOrWhiteSpace(strOtherPolicyAdjustmentAmount))
                            {
                                strOtherPolicyAdjustmentAmount = strOtherPolicyAdjustmentAmount.Trim();
                                if (ValidateAmount(strOtherPolicyAdjustmentAmount, "Other Amt", out strError))
                                {
                                    strOtherPolicyAdjustmentAmount = Convert.ToDecimal(strOtherPolicyAdjustmentAmount).ToString("0.00");
                                    polBE.OtherPolicyAdjustmentAmount = Convert.ToDecimal(strOtherPolicyAdjustmentAmount);
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
                            }

                            //NY Premium Discount
                            //--------------------

                            //AISAmount validation textbox - column 24
                            string strNYPremiumDiscAmount = Convert.ToString(excelSheetDT.Rows[i][24]);
                            if (!string.IsNullOrWhiteSpace(strNYPremiumDiscAmount))
                            {
                                strNYPremiumDiscAmount = strNYPremiumDiscAmount.Trim();
                                if (ValidateAmount(strNYPremiumDiscAmount, "NY Premium Discount", out strError))
                                {
                                    strNYPremiumDiscAmount = Convert.ToDecimal(strNYPremiumDiscAmount).ToString("0.00");
                                    polBE.NYPremiumDiscAmount = Convert.ToDecimal(strNYPremiumDiscAmount);
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
                            }

                            if (polBE.LossSourceName == "ARMIS/TPA" && polBE.TPAIndicator == false && polBE.TPADirectIndicator == false)
                            {
                                if (!isErrorRowImported)
                                {
                                    errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                    isErrorRowImported = true;
                                }


                                if (isErrorCommentAdded)
                                {
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "The TPA indicator or TPA Direct indicator must be TRUE when loss source option ARMIS/TPA is entered";

                                }
                                else
                                {
                                    errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "The TPA indicator or TPA Direct indicator must be TRUE when loss source option ARMIS/TPA is entered";
                                    isErrorCommentAdded = true;
                                }


                                //continue;
                            }


                            if (!isErrorCommentAdded)
                            //if (string.IsNullOrWhiteSpace(Convert.ToString(errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"])))
                            {

                                bool isPolExists = PolicyBizService.isPolicyAlreadyExist(polBE.PolicyID, polBE.PolicyNumber, Convert.ToDateTime(polBE.PolicyEffectiveDate), SelectedProgramPeriodID);
                                //A policy number with an identical Symbol, Policy Number, Modulus, Effective Date, Expiration Date cannot span two Accounts
                                bool isPolExistsInAnyAcct = PolicyBizService.isPolExistsInAnyAcct(polBE.PolicyID, polBE.PolicySymbol, polBE.PolicyNumber, polBE.PolicyModulus, Convert.ToDateTime(polBE.PolicyEffectiveDate), Convert.ToDateTime(polBE.PlanEndDate), AISMasterEntities.AccountNumber);

                                if ((isPolExists == false) && (isPolExistsInAnyAcct == false))
                                {
                                    bool Result = false;
                                    Result = PolicyBizService.Update(polBE);

                                    if (Result == true)
                                    {
                                        AuditBizService.Save(AISMasterEntities.AccountNumber, SelectedProgramPeriodID, GlobalConstants.AuditingWebPage.PolicyInfo, CurrentAISUser.PersonID);
                                        success += 1;
                                    }
                                    BindPolicyInformation();
                                    //SelectedPolicyID = polBE.PolicyID;
                                    //DisplayDetails(SelectedPolicyID);
                                }
                                else
                                {
                                    if (isPolExists == true)
                                    {
                                        bool isPolExistsAndDisabled = PolicyBizService.isPolicyAlreadyExistAndDisabled(polBE.PolicyID, polBE.PolicyNumber, Convert.ToDateTime(polBE.PolicyEffectiveDate), SelectedProgramPeriodID);
                                        if (isPolExistsAndDisabled == true)
                                        {
                                            errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                            errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "The record cannot be saved. An identical record already exists. Please enable existing policy";
                                            continue;
                                        }
                                        else
                                        {
                                            //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                            //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "The record cannot be saved. An identical record already exists.";
                                            //continue;

                                            int policyID = PolicyBizService.getPolicyID(polBE.PolicyNumber, Convert.ToDateTime(polBE.PolicyEffectiveDate), SelectedProgramPeriodID);
                                            bool Result = false;
                                            PolicyBE polBE1 = PolicyBizService.getPolicyInfo(policyID);

                                            polBE1.cstmrid = polBE.cstmrid;

                                            polBE1.ProgramPeriodID = polBE.ProgramPeriodID;

                                            polBE1.IsActive = polBE.IsActive;

                                            polBE1.PolicySymbol = polBE.PolicySymbol;

                                            polBE1.PolicyModulus = polBE.PolicyModulus;

                                            polBE1.PolicyNumber = polBE.PolicyNumber;

                                            polBE1.PolicyEffectiveDate = polBE.PolicyEffectiveDate;

                                            polBE1.PlanEndDate = polBE.PlanEndDate;

                                            polBE1.CoverageTypeID = polBE.CoverageTypeID;

                                            polBE1.AdjusmentTypeID = polBE.AdjusmentTypeID;

                                            polBE1.LossSystemSourceID = polBE.LossSystemSourceID;

                                            polBE1.LossSourceName = polBE.LossSourceName;

                                            polBE1.UnlimDedtblPolLimitIndicator = polBE.UnlimDedtblPolLimitIndicator;

                                            polBE1.DedTblPolicyLimitAmount = polBE.DedTblPolicyLimitAmount;

                                            polBE1.ALAETypeID = polBE.ALAETypeID;

                                            polBE1.ALAECappedAmount = polBE.ALAECappedAmount;

                                            polBE1.UnlimOverrideDedtblLimitIndicator = polBE.UnlimOverrideDedtblLimitIndicator;

                                            polBE1.OverrideDedtblLimitAmount = polBE.OverrideDedtblLimitAmount;

                                            polBE1.LDFIncurredNotReport = polBE.LDFIncurredNotReport;

                                            polBE1.LDFIncurredNO63740 = polBE.LDFIncurredNO63740;

                                            polBE1.LDFFactor = polBE.LDFFactor;

                                            polBE1.IBNRFactor = polBE.IBNRFactor;

                                            polBE1.DedtblProtPolicyStID = polBE.DedtblProtPolicyStID;

                                            polBE1.TPAIndicator = polBE.TPAIndicator;

                                            polBE1.TPADirectIndicator = polBE.TPADirectIndicator;

                                            polBE1.ISMasterPEOPolicy = polBE.ISMasterPEOPolicy;

                                            polBE1.NonConversionAmount = polBE.NonConversionAmount;

                                            polBE1.OtherPolicyAdjustmentAmount = polBE.OtherPolicyAdjustmentAmount;

                                            polBE1.NYPremiumDiscAmount = polBE.NYPremiumDiscAmount;

                                            polBE1.PolicyID = policyID;
                                            polBE1.UPDATE_DATE = DateTime.Now;
                                            polBE1.UPDATE_USER_ID = CurrentAISUser.PersonID;

                                            Result = PolicyBizService.Update(polBE1);
                                            if (Result == true)
                                            {
                                                AuditBizService.Save(AISMasterEntities.AccountNumber, SelectedProgramPeriodID, GlobalConstants.AuditingWebPage.PolicyInfo, CurrentAISUser.PersonID);
                                                success += 1;
                                            }
                                            BindPolicyInformation();
                                        }
                                    }
                                    else if (isPolExistsInAnyAcct == true)
                                    {
                                        string custName = PolicyBizService.isPolExistsInAnyAcctCustName(polBE.PolicyID, polBE.PolicySymbol, polBE.PolicyNumber, polBE.PolicyModulus, Convert.ToDateTime(polBE.PolicyEffectiveDate), Convert.ToDateTime(polBE.PlanEndDate), AISMasterEntities.AccountNumber);
                                        if (!isErrorRowImported)
                                        {
                                            errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                            isErrorRowImported = true;
                                        }
                                        errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "The record cannot be saved. A policy number with an identical Symbol, Policy Number, Modulus, Effective Date, Expiration Date cannot span two Accounts (Account Name : " + custName + ")";
                                        continue;
                                    }

                                }
                            }
                        }

                        modalPolDetails.Show();
                        if (errorLogDT.Rows.Count > 0)
                        {
                            string strErrLogPathName = String.Empty;

                            string strErrFileName = "PEOPolicyUpload_ERROR_" + CurrentAISUser.PersonID.ToString() + "_" + SelectedProgramPeriodID + "_" + strDate + "-" + strTime + sSpreadsheetGearExcelFileFormat; // AWS Cloud Migration
                            string strErrLogPath = ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + strErrFileName;
                            strErrLogPathName = ConfigurationManager.AppSettings["PolicyFilesTempPath"];
                            //ExportToExcel(errorLogDT, strErrLogPath, strErrFileName); // AWS Cloud Migration
                            // AWS Cloud Migration
                            ExportToExcelusingSpreadsheetGear(errorLogDT, strErrLogPath, strErrFileName);
                            lblErrorLog.Visible = true;
                            lblErrorLog.Text = "Uploaded " + success.ToString() + " policies out of " + excelSheetDT.Rows.Count.ToString() + " policies. ";
                            lblErrorLog.Text += "Please Click Error Button";
                            lblErrorLog.Text += " for Error log";
                            Session["strMessageErrorLog_PolicyUpload"] = strErrFileName + "|" + strErrLogPathName;
                            if (Session["strMessageErrorLog_PolicyUpload"] != null && Session["strMessageErrorLog_PolicyUpload"] != string.Empty)
                            {

                                btnErrorLog_Policy.Visible = true;
                            }
                            else
                            {
                                btnErrorLog_Policy.Visible = false;
                            }
                        }
                        else
                        {
                            lblErrorLog.Visible = true;
                            lblErrorLog.Text = "Policies uploaded successfully.";
                        }

                    }
                    catch (OleDbException ex)
                    {
                        modalPolDetails.Show();
                        lblErrorLog.Visible = true;
                        lblErrorLog.Text = "Uploaded file is not in correct format. Please upload the data using latest template.";
                        //break;
                    }

                }
                else
                {
                    lblErrorLog.Visible = true;
                    //lblErrorLog.Text = "Please Upload .xls format files only."; // commented AWS Cloud Migration
                    lblErrorLog.Text = "Please Upload " + sSpreadsheetGearExcelFileFormat + " format files only.";
                    //mpeStepped.Show();
                    modalPolDetails.Show();
                }

            }

            //modalPolDetails.Show();
            //lblErrorLog.Text = " Working";
        }


        // Commented for AWS Cloud Migration changes

        //private void ExportToExcel(DataTable Tbl, string ExcelFilePath, string strErrFileName)
        //{
        //    try
        //    {
        //        if (Tbl == null || Tbl.Columns.Count == 0)
        //            throw new Exception("ExportToExcel: Null or empty input table!\n");

        //        // load excel, and create a new workbook
        //         Excel.Application excelApp = new Excel.Application();
        //        excelApp.Workbooks.Add();                                                                 

        //        // single worksheet
        //        Excel._Worksheet workSheet = excelApp.ActiveSheet as Excel.Worksheet;

        //        workSheet.Cells.NumberFormat = "@";
        //        // column headings
        //        for (int i = 0; i < Tbl.Columns.Count; i++)
        //        {
        //            workSheet.Cells[1, (i + 1)] = Tbl.Columns[i].ColumnName;

        //        }

        //        // rows
        //        for (int i = 0; i < Tbl.Rows.Count; i++)
        //        {
        //            // to do: format datetime values before printing
        //            for (int j = 0; j < Tbl.Columns.Count; j++)
        //            {
        //                workSheet.Cells[(i + 2), (j + 1)] = Tbl.Rows[i][j];
        //            }

        //        }



        //        // check fielpath
        //        if (ExcelFilePath != null && ExcelFilePath != "")
        //        {
        //            try
        //            {
        //                //workSheet.Name = "abcdef";
        //                workSheet.SaveAs(ExcelFilePath);
        //                excelApp.Quit();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
        //                    + ex.Message);
        //            }
        //        }
        //        else    // no filepath is given
        //        {
        //            excelApp.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("ExportToExcel: \n" + ex.Message);
        //    }
        //}

        // AWS Cloud migration - SpreadsheetGear changes 
        private void ExportToExcelusingSpreadsheetGear(DataTable Tbl, string ExcelFilePath, string strErrFileName)
        {
            try
            {
                if (Tbl == null || Tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");
                // Create a new workbook and worksheet.
                SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
                int sheetIndex = 0;
                int cellIndex = 0;

                if (Tbl.Columns.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    // column headings
                    SSG.IWorksheet worksheetPolicyUpload = workbook.Worksheets["Sheet" + sheetIndex.ToString()];

                    // Get the worksheet cells reference. 
                    SSG.IRange cellsPolicyUpload = worksheetPolicyUpload.Cells;
                    cellIndex = 1;

                    for (int i = 0; i < Tbl.Columns.Count; i++)
                    {
                        String columname = string.Empty;
                        int columnindexvalue = i + 1;
                        columname = getColumnNamefromIndex(columnindexvalue);
                        cellsPolicyUpload[columname + cellIndex.ToString()].Value = Tbl.Columns[i].ColumnName;
                    }
                    cellIndex = cellIndex + 1;
                    //rows
                    for (int i = 0; i < Tbl.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (int j = 0; j < Tbl.Columns.Count; j++)
                        {

                            String columname = string.Empty;
                            int columnindexvalue = j + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);
                            cellsPolicyUpload[columname + cellIndex.ToString()].Value = Tbl.Rows[i][j];

                        }

                        cellIndex = cellIndex + 1;
                    }

                }

                //Delete original blank empty sheet from report workbook.
                workbook.ActiveWorksheet.Delete();

                if (sheetIndex == 0)
                {
                    workbook.Worksheets.Add();
                }

                if (ExcelFilePath != null && ExcelFilePath != "")
                {

                    try
                    {
                        workbook.Worksheets[0].Select();
                        string strFileName = ExcelFilePath;
                        string strFileNameDecd = Server.HtmlDecode(Server.HtmlEncode(strFileName));
                        workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
                        workbook.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }


                }
                else
                {
                    throw new Exception("ExportToExcel: File path is not given!\n");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        // AWS Cloud migration - SpreadsheetGear changes 
        public static void FormatHeaderTypeCellViewPolicyAndSteppFact(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, SSG.VAlign valign = SSG.VAlign.Center, bool isBold = true)
        {
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Borders.Color = SSG.Colors.Black;
            cell.VerticalAlignment = valign;

        }

        // AWS Cloud migration - SpreadsheetGear changes 
        public static void FormatTypeCellViewPolicyAndSteppFact(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, SSG.VAlign valign = SSG.VAlign.Center, bool isBold = false)
        {
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Borders.Color = SSG.Colors.Black;
            cell.VerticalAlignment = valign;

        }

        // AWS Cloud migration - SpreadsheetGear changes 
        private void ViewPolicyandSteppedFactExportToExcelusingSpreadsheetGear(DataTable Tbl, string ExcelFilePath, string strFileName, string SheetName)
        {
            try
            {
                if (Tbl == null || Tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");
                // Create a new workbook and worksheet.
                SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
                int sheetIndex = 0;
                int cellIndex = 0;

                if (Tbl.Columns.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    // column headings
                    SSG.IWorksheet worksheetViewPolicyAndSteppFactExport = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetViewPolicyAndSteppFactExport.Name = SheetName;

                    // Get the worksheet cells reference. 
                    SSG.IRange cellsviewPolicyAndSteppFactExport = worksheetViewPolicyAndSteppFactExport.Cells;
                    cellIndex = 1;

                    for (int i = 0; i < Tbl.Columns.Count; i++)
                    {
                        String columname = string.Empty;
                        int columnindexvalue = i + 1;
                        columname = getColumnNamefromIndex(columnindexvalue);
                        cellsviewPolicyAndSteppFactExport[columname + cellIndex.ToString()].Value = Tbl.Columns[i].ColumnName;
                        FormatHeaderTypeCellViewPolicyAndSteppFact(cellsviewPolicyAndSteppFactExport[columname + cellIndex.ToString()]);
                    }
                    cellIndex = cellIndex + 1;
                    //rows
                    for (int i = 0; i < Tbl.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (int j = 0; j < Tbl.Columns.Count; j++)
                        {

                            String columname = string.Empty;
                            int columnindexvalue = j + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);
                            cellsviewPolicyAndSteppFactExport[columname + cellIndex.ToString()].Value = Tbl.Rows[i][j];
                            FormatTypeCellViewPolicyAndSteppFact(cellsviewPolicyAndSteppFactExport[columname + cellIndex.ToString()]);
                        }

                        cellIndex = cellIndex + 1;
                    }

                    worksheetViewPolicyAndSteppFactExport.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetViewPolicyAndSteppFactExport.UsedRange.ColumnCount; col++)
                    {
                        worksheetViewPolicyAndSteppFactExport.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                //Delete original blank empty sheet from report workbook.
                workbook.ActiveWorksheet.Delete();

                if (sheetIndex == 0)
                {
                    workbook.Worksheets.Add();
                }

                if (ExcelFilePath != null && ExcelFilePath != "")
                {

                    try
                    {
                        workbook.Worksheets[0].Select();
                        string strFileNamePath = ExcelFilePath;
                        string strFileNameDecd = Server.HtmlDecode(Server.HtmlEncode(strFileNamePath));
                        workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
                        workbook.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }


                }
                else
                {
                    throw new Exception("ExportToExcel: File path is not given!\n");
                }



                MemoryStream memStream = new MemoryStream();

                try
                {
                    using (FileStream fileStream = File.OpenRead(ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + strFileName))
                    {

                        memStream.SetLength(fileStream.Length);
                        fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                        byte[] byteArray = memStream.ToArray();
                        Response.Clear();
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                        Response.AddHeader("content-length", memStream.Length.ToString());
                        Response.BinaryWrite(byteArray);
                    }

                }
                catch (Exception ex)
                {
                    ShowError("Unable to Preview the Report. Please contact Application Support Team");
                    return;
                }
                finally
                {
                    memStream.Close();
                    Response.Flush();
                    Response.End();
                    //HttpContext.Current.Response.End();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        // AWS Cloud migration - SpreadsheetGear changes 
        public static String getColumnNamefromIndex(int column)
        {
            column--;
            String col = Convert.ToString((char)('A' + (column % 26)));
            while (column >= 26)
            {
                column = (column / 26) - 1;
                col = Convert.ToString((char)('A' + (column % 26))) + col;
            }
            return col;
        }

        private void ExportToExcel(string fileName, Table tbl)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter strWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(strWriter);

            tbl.Style[HtmlTextWriterStyle.BorderColor] = "Black";
            tbl.Style[HtmlTextWriterStyle.BorderStyle] = BorderStyle.Solid.ToString();
            tbl.Style[HtmlTextWriterStyle.BorderWidth] = "1";


            foreach (TableRow row in tbl.Rows)
            {
                row.Attributes.Add("class", "textmode");

                foreach (TableCell cell in row.Cells)
                {
                    cell.Attributes.Add("class", "textmode");
                }
            }

            //tbl.Attributes.Add("class", "textmode");

            tbl.RenderControl(htmlWriter);
            //string style = @"<style>.textmode{mso-number-format:\@;}</style>";
            //string style = @"<style> .textmode { mso-number-format:'\@'; } </style>";

            string style = @"<style> .textmode { mso-number-format:'\@';}
                                  </style>";

            Response.Write(style);
            Response.Output.Write(strWriter.ToString());
            Response.Flush();
            Response.End();
        }

        //Validations for PEO Policy Automation
        private bool ValidatePolicySymbol(string strSymbol, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            if (!string.IsNullOrWhiteSpace(strSymbol))
            {
                if (Regex.IsMatch(strSymbol, @"^[a-zA-Z]+$"))
                {
                    if (strSymbol.Length < 2 || strSymbol.Length > 3)
                    {
                        strError = "Policy Symbol Should Contain 2 or 3 Alphabets";
                        ValidStatus = false;
                    }
                    else
                    {
                        strError = "";
                        ValidStatus = true;
                    }
                }
                else
                {
                    strError = "Policy Symbol Should Contain only Alphabets";
                    ValidStatus = false;
                }
            }
            else
            {
                strError = "Policy Symbol Should not be a blank";
                ValidStatus = false;
            }

            return ValidStatus;
        }

        private bool ValidatePolicyNumber(string strNumber, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            strError = "";
            if (!string.IsNullOrWhiteSpace(strNumber))
            {
                if (Regex.IsMatch(strNumber, @"^[a-zA-Z0-9]+$"))
                {
                    if (strNumber.Length < 7 || strNumber.Length > 8)
                    {
                        strError = "Policy Number Should Contain 7 to 8 Characters of Length";
                        ValidStatus = false;
                    }
                    else
                    {
                        strError = "";
                        ValidStatus = true;
                    }
                }
                else
                {
                    strError = "Policy Number Should Contain only Alphanumerics";
                    ValidStatus = false;
                }

            }
            else
            {
                strError = "Policy number Should not be a blank";
                ValidStatus = false;
            }
            return ValidStatus;
        }

        private bool ValidatePolicyModule(string strModule, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            if (!string.IsNullOrEmpty(strModule))
            {
                if (Regex.IsMatch(strModule, @"^[0-9]+$"))
                {
                    if (strModule.Length > 2)
                    {
                        strError = "Policy Module Should Contain 2 Digits of Numeric Value";
                        ValidStatus = false;
                    }
                    else
                    {
                        strError = "";
                        ValidStatus = true;
                    }
                }
                else
                {
                    strError = "Policy Module Should Contain only Numbers";
                    ValidStatus = false;
                }
            }
            else
            {
                strError = "Policy Module Should not be a blank";
                ValidStatus = false;
            }
            return ValidStatus;
        }

        private bool ValidatePolicyEffDate(string strEffDate, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            DateTime EffectiveDate;
            if (!string.IsNullOrEmpty(strEffDate))
            {
                if (DateTime.TryParse(strEffDate, out EffectiveDate))
                {

                    if (EffectiveDate < Convert.ToDateTime(proPerStartDate))
                    {
                        strError = "Policy effective date must be greater than or equal to program period effective date";
                        ValidStatus = false;
                    }
                    else if (EffectiveDate > Convert.ToDateTime(proPerEndDate))
                    {
                        strError = "Policy effective date must be less than or equal to program period effective date";
                        ValidStatus = false;
                    }
                    else
                    {
                        strError = "";
                        ValidStatus = true;
                    }
                }
                else
                {
                    strError = "Policy Effective Date is Not Valid. It Should be mm/dd/yyyy format";
                    ValidStatus = false;
                }
            }
            else
            {
                strError = "Policy Effective Date should not be blank";
                ValidStatus = false;
            }

            return ValidStatus;
        }

        private bool ValidatePolicyExpDate(string strExpDate, out string strError, DateTime PolEffDate)
        {
            strError = "";
            bool ValidStatus = false;
            DateTime ExpiryDate;
            if (!string.IsNullOrEmpty(strExpDate))
            {
                if (DateTime.TryParse(strExpDate, out ExpiryDate))
                {

                    if (PolEffDate > ExpiryDate)
                    {
                        strError = "Policy expiration date must be greater than policy effective date";
                        ValidStatus = false;
                    }
                    else if (ExpiryDate < Convert.ToDateTime(proPerStartDate))
                    {
                        strError = "Policy Expiration Date must be greater than or equal to Program Period Effective Date";
                        ValidStatus = false;
                    }
                    else if (ExpiryDate > Convert.ToDateTime(proPerEndDate))
                    {
                        strError = "Policy Expiration Date must be less than or equal Program Period Expiration Date";
                        ValidStatus = false;
                    }
                    else
                    {
                        strError = "";
                        ValidStatus = true;
                    }
                }
                else
                {
                    strError = "Policy Effective Date is Not Valid. It Should be mm/dd/yyyy format";
                    ValidStatus = false;
                }
            }

            else
            {
                strError = "Policy Effective Date should not be blank";
                ValidStatus = false;
            }


            return ValidStatus;
        }

        private bool ValidateLookUp(string strMatch, string strLookupType, out string strError, out int LookupID)
        {
            bool ValidStatus = false;
            IList<LookupBE> lkups = new List<LookupBE>();
            lkups = (new ZurichNA.AIS.Business.Logic.BLAccess()).GetLookUpActiveData(strLookupType);
            if (lkups.Any(x => x.LookUpName.ToLower() == strMatch.ToLower()))
            {
                ValidStatus = true;
                strError = "";
                LookupBE a = lkups.SingleOrDefault(x => x.LookUpName.ToLower() == strMatch.ToLower());
                LookupID = a.LookUpID;
            }
            else
            {
                ValidStatus = false;
                strError = "Given " + strLookupType + " does not exist";
                LookupID = 0;
            }
            return ValidStatus;

        }

        private bool ValidateTrueFalse(string strValue, string strFiledName, out string strError)
        {
            bool ValidStatus = false;
            bool status = false;
            if (bool.TryParse(strValue, out status))
            {
                ValidStatus = true;
                strError = "";
            }
            else
            {
                ValidStatus = false;
                strError = "Given " + strFiledName + " value is not valid,Please enter True/False";
            }
            return ValidStatus;
        }

        private bool ValidateDecimals(string strValue, string strFieldName, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            decimal result;
            //if (Regex.IsMatch(strValue, @"^\$[0-9]+(\.[0-9][0-9])?$"))
            if (Decimal.TryParse(strValue, out result))
            {
                ValidStatus = true;
                strError = "";
            }
            else
            {
                strError = "Given " + strFieldName + " value is not valid,Please enter value in x.xxxxxx format";
                ValidStatus = false;
            }

            return ValidStatus;

        }

        private bool ValidateAmount(string strValue, string strFieldName, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            decimal result;

            if (Decimal.TryParse(strValue, out result))
            {
                ValidStatus = true;
                strError = "";
            }
            else
            {
                strError = "Given " + strFieldName + " value is not valid,Please enter valid amount";
                ValidStatus = false;
            }
            return ValidStatus;
        }

        private bool ValidateFactor(string strValue, string strFieldName, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            decimal result;

            if (Decimal.TryParse(strValue, out result))
            {
                string[] strArr = strValue.Split('.');
                if (strArr[0].Length > 1)
                {
                    strError = "Given " + strFieldName + " value is not valid,Please enter factor in x.xxxxxx format";
                    ValidStatus = false;
                }
                else
                {
                    ValidStatus = true;
                    strError = "";
                }
            }
            else
            {
                strError = "Given " + strFieldName + " value is not valid,Please enter factor in x.xxxxxx format";
                ValidStatus = false;
            }
            return ValidStatus;
        }

        private bool ValidateMonths(string strValue, out string strError)
        {
            strError = "";
            bool ValidStatus = false;
            int result;

            if (strValue.Length <= 3)
            {
                if (int.TryParse(strValue, out result))
                {
                    ValidStatus = true;
                    strError = "";
                }
            }
            else
            {
                strError = "Given Value for Months to Validate is not valid, Please enter maximum 3 digit number";
                ValidStatus = false;
            }
            return ValidStatus;
        }

        protected void btnSFUpdate_Click(object sender, EventArgs e)
        {
            if (fleStepped.HasFile)
            {
                if (Path.GetExtension(fleStepped.PostedFile.FileName) == sSpreadsheetGearExcelFileFormat) // AWS Cloud Migration
                {


                    string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                    string strTime = DateTime.Now.ToString("HH-mm");

                    string fileName = "SteppedUpload_" + CurrentAISUser.PersonID.ToString() + "_" + SelectedProgramPeriodID + "_" + strDate + "-" + strTime + sSpreadsheetGearExcelFileFormat; // AWS Cloud Migration
                    if (!(Directory.Exists(ConfigurationManager.AppSettings["PolicyFilesTempPath"])))
                    {
                        Directory.CreateDirectory(ConfigurationManager.AppSettings["PolicyFilesTempPath"]);
                    }

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    fleStepped.SaveAs(ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + fileName);

                    //full path name
                    string filename = ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + fileName;
                    policyBizService = new PolicyBS();
                    try
                    {
                        DataTable excelSheetDT = policyBizService.GetDataTableExcelSF(filename);
                        //DataTable excelSheetDT = policyBizService.GetDataTableFromCSVFile(filename);

                        DataTable errorLogDT = excelSheetDT.Clone();
                        errorLogDT.Columns.Add("Comments", typeof(System.String));

                        string strError = "";

                        int success = 0;

                        SteppedFactorBE stepBE = new SteppedFactorBE();
                        //stepBE.POLICY_ID = SelectedPolicyID;
                        //stepBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                        //stepBE.PREM_ADJ_PGM_ID = SelectedProgramPeriodID;
                        //stepBE.PREM_ADJ_PGM_ID = policyBizService.getProgramPeriodID(
                        //
                        List<SteppedFactorBE> stepBeLst = new List<SteppedFactorBE>();
                        for (int i = 0; i < excelSheetDT.Rows.Count; i++)
                        {
                            bool isErrorRowImported = false;
                            bool isErrorCommentAdded = false;

                            stepBE = new SteppedFactorBE();
                            stepBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                            stepBE.PREM_ADJ_PGM_ID = SelectedProgramPeriodID;

                            string strPolicySymbol = Convert.ToString(excelSheetDT.Rows[i][0]);
                            if (ValidatePolicySymbol(strPolicySymbol.Trim(), out strError))
                            {
                                stepBE.PolicySymbol = strPolicySymbol.Trim();
                            }
                            else
                            {
                                //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = strError;
                                //continue;

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
                            }

                            string strPolicyNumber = Convert.ToString(excelSheetDT.Rows[i][1]);
                            if (ValidatePolicyNumber(strPolicyNumber.Trim(), out strError))
                            {
                                if (strPolicyNumber.Length == 7)
                                    strPolicyNumber = "0" + strPolicyNumber;
                                stepBE.PolicyNumber = strPolicyNumber.Trim();
                            }
                            else
                            {
                                //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = strError;
                                //continue;

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
                            }


                            string strPolicyModule = Convert.ToString(excelSheetDT.Rows[i][2]);
                            if (ValidatePolicyModule(strPolicyModule.Trim(), out strError))
                            {
                                if (strPolicyModule.Length == 1)
                                    strPolicyModule = "0" + strPolicyModule;
                                stepBE.PolicyModulus = strPolicyModule.Trim();
                            }
                            else
                            {
                                //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = strError;
                                //continue;

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
                            }



                            string strPolicyEffDate = Convert.ToString(excelSheetDT.Rows[i][3]);
                            strPolicyEffDate = strPolicyEffDate.Trim();
                            if (ValidatePolicyEffDate(strPolicyEffDate, out strError))
                            {
                                stepBE.PolicyEffectiveDate = Convert.ToDateTime(strPolicyEffDate);
                            }
                            else
                            {
                                //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = strError;
                                //continue;

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
                            }

                            string strPolicyExpDate = Convert.ToString(excelSheetDT.Rows[i][4]);
                            strPolicyExpDate = strPolicyExpDate.Trim();
                            if (ValidatePolicyExpDate(strPolicyExpDate, out strError, Convert.ToDateTime(stepBE.PolicyEffectiveDate)))
                            {
                                stepBE.PlanEndDate = Convert.ToDateTime(strPolicyExpDate);
                            }
                            else
                            {
                                //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = strError;
                                //continue;

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
                            }



                            policyBizService = new PolicyBS();
                            int POLICY_ID = 0;
                            if (isErrorCommentAdded == false)
                            {
                                //POLICY_ID = policyBizService.getPolicyID(stepBE.PolicyNumber, Convert.ToDateTime(stepBE.PolicyEffectiveDate), stepBE.PREM_ADJ_PGM_ID);
                                POLICY_ID = policyBizService.getPolicyID(stepBE.PolicySymbol, stepBE.PolicyNumber, stepBE.PolicyModulus, Convert.ToDateTime(stepBE.PolicyEffectiveDate), Convert.ToDateTime(stepBE.PlanEndDate), stepBE.CUSTMR_ID);

                                if (POLICY_ID <= 0)
                                {
                                    //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                    //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Policy not existing for the selected account. Please check the given data.";
                                    //continue;

                                    if (!isErrorRowImported)
                                    {
                                        errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                        isErrorRowImported = true;
                                    }

                                    if (isErrorCommentAdded)
                                    {
                                        errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "Policy not existing for the selected account. Please check the given data.";

                                    }
                                    else
                                    {
                                        errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Policy not existing for the selected account. Please check the given data.";
                                        isErrorCommentAdded = true;
                                    }
                                }


                                if (POLICY_ID > 0)
                                {

                                    PolicyBE polBeEnableDisable = policyBizService.getPolicyInfo(POLICY_ID);

                                    if (polBeEnableDisable.IsActive == false)
                                    {
                                        //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                        //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Given Policy is disabled.Please enable existing policy";
                                        //continue;

                                        if (!isErrorRowImported)
                                        {
                                            errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                            isErrorRowImported = true;
                                        }

                                        if (isErrorCommentAdded)
                                        {
                                            errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "Given Policy is disabled.Please enable existing policy";

                                        }
                                        else
                                        {
                                            errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Given Policy is disabled.Please enable existing policy";
                                            isErrorCommentAdded = true;
                                        }
                                    }


                                    if (polBeEnableDisable.LDFIncurredNO63740 == false)
                                    {
                                        //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                        //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Given Policy is disabled.Please enable existing policy";
                                        //continue;

                                        if (!isErrorRowImported)
                                        {
                                            errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                            isErrorRowImported = true;
                                        }

                                        if (isErrorCommentAdded)
                                        {
                                            errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "LDF/IBNR Stepped is not enabled for the given policy";

                                        }
                                        else
                                        {
                                            errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "LDF/IBNR Stepped is not enabled for the given policy";
                                            isErrorCommentAdded = true;
                                        }
                                    }



                                    string strMonths = Convert.ToString(excelSheetDT.Rows[i][5]);
                                    if (!string.IsNullOrWhiteSpace(strMonths))
                                    {
                                        strMonths = strMonths.Trim();
                                        if (ValidateMonths(strMonths, out strError))
                                        {
                                            stepBE.MONTHS_TO_VAL = Convert.ToInt32(strMonths);
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
                                        }
                                    }
                                    else
                                    {
                                        //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                        //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Months to val should not be blank";
                                        //continue;

                                        if (!isErrorRowImported)
                                        {
                                            errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                            isErrorRowImported = true;
                                        }

                                        if (isErrorCommentAdded)
                                        {
                                            errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "Months to val should not be blank";

                                        }
                                        else
                                        {
                                            errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "Months to val should not be blank";
                                            isErrorCommentAdded = true;
                                        }
                                    }

                                    string strLDFFactor = Convert.ToString(excelSheetDT.Rows[i][6]);
                                    if (!string.IsNullOrWhiteSpace(strLDFFactor))
                                    {
                                        strLDFFactor = strLDFFactor.Trim();
                                        if (ValidateFactor(strLDFFactor, "LDF Factor", out strError))
                                        {
                                            stepBE.LDF_FACTOR = Convert.ToDecimal(strLDFFactor);
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
                                        }
                                    }

                                    string strIBNRFactor = Convert.ToString(excelSheetDT.Rows[i][7]);
                                    if (!string.IsNullOrWhiteSpace(strIBNRFactor))
                                    {
                                        if (!string.IsNullOrWhiteSpace(strLDFFactor))
                                        {
                                            //errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                            //errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "LDF and IBNR factors are exists. Enter only LDF or IBNR factors";
                                            //continue;

                                            if (!isErrorRowImported)
                                            {
                                                errorLogDT.ImportRow(excelSheetDT.Rows[i]);
                                                isErrorRowImported = true;
                                            }

                                            if (isErrorCommentAdded)
                                            {
                                                errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] += ", " + "LDF and IBNR factors are exists. Enter only LDF or IBNR factors";

                                            }
                                            else
                                            {
                                                errorLogDT.Rows[errorLogDT.Rows.Count - 1]["Comments"] = "LDF and IBNR factors are exists. Enter only LDF or IBNR factors";
                                                isErrorCommentAdded = true;
                                            }
                                        }

                                        strIBNRFactor = strIBNRFactor.Trim();
                                        if (ValidateFactor(strIBNRFactor, "IBNR Factor", out strError))
                                        {
                                            stepBE.IBNR_FACTOR = Convert.ToDecimal(strIBNRFactor);
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
                                        }
                                    }


                                    if (!isErrorCommentAdded)
                                    {
                                        stepBeLst.Add(stepBE);
                                    }
                                }
                            }
                        }

                        if (stepBeLst.Count > 0)
                        {
                            bool deleteStepped = SteppedFactorBizService.deleteSteppedFactors(AISMasterEntities.AccountNumber, SelectedProgramPeriodID);
                            foreach (SteppedFactorBE stepBe1 in stepBeLst)
                            {
                                policyBizService = new PolicyBS();
                                int POLICY_ID = policyBizService.getPolicyID(stepBe1.PolicyNumber, Convert.ToDateTime(stepBe1.PolicyEffectiveDate), stepBe1.PREM_ADJ_PGM_ID);
                                stepBe1.POLICY_ID = POLICY_ID;
                                //if (POLICY_ID > 0)
                                //{
                                if (deleteStepped)
                                {
                                    stepBe1.CREATE_DATE = DateTime.Now;
                                    stepBe1.CREATE_USER_ID = CurrentAISUser.PersonID;
                                    stepBe1.ISACTIVE = true;

                                    bool Result = SteppedFactorBizService.Update(stepBe1);
                                    if (Result)
                                        success += 1;
                                }
                                else
                                {
                                    lblSFErrorMessage.Visible = true;
                                    lblSFErrorMessage.Text = "Error while deleting existing stepped factors. Please try again";
                                }
                                //}
                                //else
                                //{
                                //    lblSFErrorMessage.Visible = true;
                                //    lblSFErrorMessage.Text = "Policy not found in the selected account. Please check the given data";
                                //}

                                if (success > 0)
                                {
                                    //lstPolicyInfo.Enabled = true;
                                    //PnlProgramperiod.Enabled = true;
                                    //lblPolicyDetails.Visible = false;
                                    //lbClosePolicyDetails.Visible = false;
                                    //pnlDetails.Visible = false;
                                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                        btnAdd.Enabled = true;

                                    //btnSFUpload.Visible = true;

                                    //lstPolicyInfo.Enabled = false;
                                    //PnlProgramperiod.Enabled = false;
                                    //btnAdd.Enabled = false;

                                    //lblPolicyDetails.Visible = false;
                                    //lbClosePolicyDetails.Visible = false;
                                    //pnlDetails.Visible = false;

                                    //lblSteppedFactor.Visible = true;
                                    //lbCloseSteppedFactor.Visible = true;
                                    //pnlSteppedFactor.Visible = true;
                                    //BindSteppedFactorListView();

                                    //BindPolicyInformation();
                                    //SelectedPolicyID = POLICY_ID;
                                    //BindSteppedFactorListView();
                                }
                            }
                        }
                        mpeStepped.Show();
                        if (errorLogDT.Rows.Count > 0)
                        {
                            //string strErrFileName = "SteppedUpload_ERROR_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + ".xls";
                            string strErrFileName = "SteppedUpload_ERROR_" + CurrentAISUser.PersonID.ToString() + "_" + SelectedProgramPeriodID + "_" + strDate + "-" + strTime + sSpreadsheetGearExcelFileFormat; // AWS Cloud Migration
                            string strErrLogPath = ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + strErrFileName;
                            //ExportToExcel(errorLogDT, strErrLogPath, strErrFileName); // AWS Cloud Migration
                            ExportToExcelusingSpreadsheetGear(errorLogDT, strErrLogPath, strErrFileName);
                            lblSFErrorMessage.Visible = true;
                            lblSFErrorMessage.Text = "Uploaded " + success.ToString() + " Stepped Factors out of " + excelSheetDT.Rows.Count.ToString() + " Stepped Factors. ";
                            //lblSFErrorMessage.Text += "Please <a href=@" + Server.MapPath(strErrLogPath) + ">Click Here</a>";
                            lblSFErrorMessage.Text += "Please Click Error Button";
                            lblSFErrorMessage.Text += " for Error log";
                            Session["strMessageErrorLog_SFUpload"] = strErrFileName + "|" + strErrLogPath;
                            if (Session["strMessageErrorLog_SFUpload"] != null && Session["strMessageErrorLog_SFUpload"] != string.Empty)
                            {

                                btnSFErrorLog.Visible = true;
                            }
                            else
                            {
                                btnSFErrorLog.Visible = false;
                            }
                        }
                        else
                        {
                            lblSFErrorMessage.Visible = true;
                            lblSFErrorMessage.Text = "Stepped Factors uploaded successfully.";
                        }
                    }

                    catch (OleDbException ex)
                    {
                        mpeStepped.Show();
                        lblSFErrorMessage.Visible = true;
                        lblSFErrorMessage.Text = "Uploaded file is not in correct format. Please upload the data using latest template.";
                        //break;
                    }
                }
                else
                {
                    lblSFErrorMessage.Visible = true;
                    // commented for AWS Cloud Migration changes
                    //lblSFErrorMessage.Text = "Please Upload .xls format files only.";
                    lblSFErrorMessage.Text = "Please Upload " + sSpreadsheetGearExcelFileFormat + " format files only.";
                    mpeStepped.Show();
                }
            }



        }

        protected void btnViewPolicies_Click(object sender, EventArgs e)
        {
            IList<PolicyBE> lstPolicyBE = GetSortedPolicyData(SelectedProgramPeriodID);
            string strDate = DateTime.Now.ToString().Replace(":", "-");
            strDate = strDate.Replace("/", "-");
            string strFileName = "Policy Details" + "_" + CurrentAISUser.PersonID + "_" + strDate + sSpreadsheetGearExcelFileFormat; //AWS Cloud Migration

            // commented for AWS Cloud Migration changes

            //Table tbl = new Table();
            //TableRow tblRow = new TableRow();
            //TableCell[] cell = new TableCell[25];

            //cell[0] = new TableCell();
            //cell[0].Text = "Policy Symbol";
            //cell[1] = new TableCell();
            //cell[1].Text = "Policy Number";
            //cell[2] = new TableCell();
            //cell[2].Text = "Policy Mod";
            //cell[3] = new TableCell();
            //cell[3].Text = "Policy Effective";
            //cell[4] = new TableCell();
            //cell[4].Text = "Policy Expire";
            //cell[5] = new TableCell();
            //cell[5].Text = "Coverage Type";
            //cell[6] = new TableCell();
            //cell[6].Text = "Adjustment Type";
            //cell[7] = new TableCell();
            //cell[7].Text = "Loss Source";
            //cell[8] = new TableCell();
            //cell[8].Text = "Unlimited Pol Ded Limit";
            //cell[9] = new TableCell();
            //cell[9].Text = "Deductible Pol Limit Amt";
            //cell[10] = new TableCell();
            //cell[10].Text = "ALAE Handling Type";
            //cell[11] = new TableCell();
            //cell[11].Text = "ALAE Capped Amt";
            //cell[12] = new TableCell();
            //cell[12].Text = "Unlimited Override Ded Limit";
            //cell[13] = new TableCell();
            //cell[13].Text = "Override Ded Limit Amt";
            //cell[14] = new TableCell();
            //cell[14].Text = "LDF/IBNR Incl Limit";
            //cell[15] = new TableCell();
            //cell[15].Text = "LDF/IBNR Stepped";
            //cell[16] = new TableCell();
            //cell[16].Text = "LDF";
            //cell[17] = new TableCell();
            //cell[17].Text = "IBNR";
            //cell[18] = new TableCell();
            //cell[18].Text = "DEP State";
            //cell[19] = new TableCell();
            //cell[19].Text = "TPA";
            //cell[20] = new TableCell();
            //cell[20].Text = "TPA Direct";
            //cell[21] = new TableCell();
            //cell[21].Text = "Master PEO Policy";
            //cell[22] = new TableCell();
            //cell[22].Text = "Non Conv";
            //cell[23] = new TableCell();
            //cell[23].Text = "Other Amt";
            //cell[24] = new TableCell();
            //cell[24].Text = "NY Premium Discount";

            //AddStyle(cell, true);
            //tblRow.Cells.AddRange(cell);
            //tbl.Rows.Add(tblRow);

            DataTable dt = new DataTable();

            dt.Columns.Add("Policy Symbol");
            dt.Columns.Add("Policy Number");
            dt.Columns.Add("Policy Mod");
            dt.Columns.Add("Policy Effective");
            dt.Columns.Add("Policy Expire");
            dt.Columns.Add("Coverage Type");
            dt.Columns.Add("Adjustment Type");
            dt.Columns.Add("Loss Source");
            dt.Columns.Add("Unlimited Pol Ded Limit");
            dt.Columns.Add("Deductible Pol Limit Amt");
            dt.Columns.Add("ALAE Handling Type");
            dt.Columns.Add("ALAE Capped Amt");
            dt.Columns.Add("Unlimited Override Ded Limit");
            dt.Columns.Add("Override Ded Limit Amt");
            dt.Columns.Add("LDF/IBNR Incl Limit");
            dt.Columns.Add("LDF/IBNR Stepped");
            dt.Columns.Add("LDF");
            dt.Columns.Add("IBNR");
            dt.Columns.Add("DEP State");
            dt.Columns.Add("TPA");
            dt.Columns.Add("TPA Direct");
            dt.Columns.Add("Master PEO Policy");
            dt.Columns.Add("Non Conv");
            dt.Columns.Add("Other Amt");
            dt.Columns.Add("NY Premium Discount");

            int i = 0;

            foreach (PolicyBE polBE in lstPolicyBE)
            {

                if (dt.Rows.Count == 0)
                {
                    i = 0;
                }
                else
                {
                    i = i + 1;
                }


                dt.Rows.Add();

                dt.Rows[i][0] = HttpUtility.HtmlEncode(Convert.ToString(polBE.PolicySymbol));
                dt.Rows[i][1] = HttpUtility.HtmlEncode(Convert.ToString(polBE.PolicyNumber));
                dt.Rows[i][2] = HttpUtility.HtmlEncode(Convert.ToString(polBE.PolicyModulus));
                if (polBE.PolicyEffectiveDate.HasValue)
                    dt.Rows[i][3] = polBE.PolicyEffectiveDate.GetValueOrDefault().ToString("MM/dd/yyy");
                dt.Rows[i][4] = polBE.PlanEndDate.ToString("MM/dd/yyyy");
                dt.Rows[i][5] = (polBE.CoverageTypeID.HasValue) ? GetLookupNameByID(polBE.CoverageTypeID.Value) : "";
                dt.Rows[i][6] = (polBE.CoverageTypeID.HasValue) ? GetLookupNameByID(polBE.AdjusmentTypeID.Value) : "";
                dt.Rows[i][7] = (polBE.LossSystemSourceID.HasValue) ? GetLookupNameByID(polBE.LossSystemSourceID.Value) : "";
                dt.Rows[i][8] = Convert.ToString(polBE.UnlimDedtblPolLimitIndicator);
                dt.Rows[i][9] = Convert.ToString((int?)polBE.DedTblPolicyLimitAmount);
                dt.Rows[i][10] = HttpUtility.HtmlEncode(Convert.ToString(polBE.ALAETypeName));
                dt.Rows[i][11] = Convert.ToString((int?)polBE.ALAECappedAmount);
                dt.Rows[i][12] = Convert.ToString(polBE.UnlimOverrideDedtblLimitIndicator);
                dt.Rows[i][13] = Convert.ToString((int?)polBE.OverrideDedtblLimitAmount);
                dt.Rows[i][14] = Convert.ToString(polBE.LDFIncurredNotReport);
                dt.Rows[i][15] = Convert.ToString(polBE.LDFIncurredNO63740);
                dt.Rows[i][16] = Convert.ToString(polBE.LDFFactor);
                dt.Rows[i][17] = Convert.ToString(polBE.IBNRFactor);
                dt.Rows[i][18] = Convert.ToString(polBE.DedtblProtPolicyStID);
                dt.Rows[i][19] = Convert.ToString(polBE.TPAIndicator);
                dt.Rows[i][20] = Convert.ToString(polBE.TPADirectIndicator);
                dt.Rows[i][21] = Convert.ToString(polBE.ISMasterPEOPolicy);
                dt.Rows[i][22] = Convert.ToString((int?)polBE.NonConversionAmount);
                dt.Rows[i][23] = Convert.ToString((int?)polBE.OtherPolicyAdjustmentAmount);
                dt.Rows[i][24] = Convert.ToString((int?)polBE.NYPremiumDiscAmount);


                //commented for AWS Cloud Migration changes

                //tblRow = new TableRow();

                //cell[0] = new TableCell();
                ////06/23 for veracode
                ////cell[0].Text = Convert.ToString(polBE.PolicySymbol);
                //cell[0].Text = HttpUtility.HtmlEncode(Convert.ToString(polBE.PolicySymbol));
                //cell[1] = new TableCell();
                ////06/23 for veracode
                ////cell[1].Text = Convert.ToString(polBE.PolicyNumber);
                //cell[1].Text = HttpUtility.HtmlEncode(Convert.ToString(polBE.PolicyNumber));
                //cell[2] = new TableCell();
                ////06/23 for veracode
                ////cell[2].Text = Convert.ToString(polBE.PolicyModulus);
                //cell[2].Text = HttpUtility.HtmlEncode(Convert.ToString(polBE.PolicyModulus));
                //cell[3] = new TableCell();
                //if (polBE.PolicyEffectiveDate.HasValue)
                //    cell[3].Text = polBE.PolicyEffectiveDate.GetValueOrDefault().ToString("MM/dd/yyy");
                //cell[4] = new TableCell();
                //cell[4].Text = polBE.PlanEndDate.ToString("MM/dd/yyyy");

                //cell[5] = new TableCell();
                //cell[5].Text = (polBE.CoverageTypeID.HasValue) ? GetLookupNameByID(polBE.CoverageTypeID.Value) : "";
                //cell[6] = new TableCell();
                //cell[6].Text = (polBE.CoverageTypeID.HasValue) ? GetLookupNameByID(polBE.AdjusmentTypeID.Value) : "";

                //cell[7] = new TableCell();
                //cell[7].Text = (polBE.LossSystemSourceID.HasValue) ? GetLookupNameByID(polBE.LossSystemSourceID.Value) : "";

                //cell[8] = new TableCell();
                //cell[8].Text = Convert.ToString(polBE.UnlimDedtblPolLimitIndicator);
                //cell[9] = new TableCell();
                //cell[9].Text = Convert.ToString((int?)polBE.DedTblPolicyLimitAmount);
                //cell[10] = new TableCell();
                ////06/23 for veracode
                ////cell[10].Text = Convert.ToString(polBE.ALAETypeName);
                //cell[10].Text = HttpUtility.HtmlEncode(Convert.ToString(polBE.ALAETypeName));
                //cell[11] = new TableCell();
                //cell[11].Text = Convert.ToString((int?)polBE.ALAECappedAmount);
                //cell[12] = new TableCell();
                //cell[12].Text = Convert.ToString(polBE.UnlimOverrideDedtblLimitIndicator);
                //cell[13] = new TableCell();
                //cell[13].Text = Convert.ToString((int?)polBE.OverrideDedtblLimitAmount);
                //cell[14] = new TableCell();
                //cell[14].Text = Convert.ToString(polBE.LDFIncurredNotReport);
                //cell[15] = new TableCell();
                //cell[15].Text = Convert.ToString(polBE.LDFIncurredNO63740);
                //cell[16] = new TableCell();
                //cell[16].Text = Convert.ToString(polBE.LDFFactor);
                //cell[17] = new TableCell();
                //cell[17].Text = Convert.ToString(polBE.IBNRFactor);
                //cell[18] = new TableCell();
                //cell[18].Text = Convert.ToString(polBE.DedtblProtPolicyStID);


                //cell[19] = new TableCell();
                //cell[19].Text = Convert.ToString(polBE.TPAIndicator);
                //cell[20] = new TableCell();
                //cell[20].Text = Convert.ToString(polBE.TPADirectIndicator);
                //cell[21] = new TableCell();
                //cell[21].Text = Convert.ToString(polBE.ISMasterPEOPolicy);
                //cell[22] = new TableCell();
                //cell[22].Text = Convert.ToString((int?)polBE.NonConversionAmount);
                //cell[23] = new TableCell();
                //cell[23].Text = Convert.ToString((int?)polBE.OtherPolicyAdjustmentAmount);
                //cell[24] = new TableCell();
                //cell[24].Text = Convert.ToString((int?)polBE.NYPremiumDiscAmount);


                //AddStyle(cell, false);
                //tblRow.Cells.AddRange(cell);
                //tbl.Rows.Add(tblRow);
            }

            // AWS Cloud Migration changes
            string strFilePath = ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + strFileName;
            string SheetName = "Policy Details";
            //ExportToExcel(strFileName, tbl); // commented for AWS Cloud Migration changes
            //modalPolDetails.Show();
            ViewPolicyandSteppedFactExportToExcelusingSpreadsheetGear(dt, strFilePath, strFileName, SheetName);
        }

        protected void btnViewFactors_Click(object sender, EventArgs e)
        {
            steppedFactorBizService = new SteppedFactorBS();
            IList<SteppedFactorBE> lstSteppedBE = steppedFactorBizService.GetSteppedFactorDataByPgmID(SelectedProgramPeriodID);
            string strDate = DateTime.Now.ToString().Replace(":", "-");
            strDate = strDate.Replace("/", "-");
            string strFileName = "Stepped Factor Details" + "_" + CurrentAISUser.PersonID + "_" + strDate + sSpreadsheetGearExcelFileFormat; // AWS Cloud Migration changes

            // commented for AWS Cloud Migration changes

            //Table tbl = new Table();
            //TableRow tblRow = new TableRow();
            //TableCell[] cell = new TableCell[8];

            //cell[0] = new TableCell();
            //cell[0].Text = "Policy Symbol";
            //cell[1] = new TableCell();
            //cell[1].Text = "Policy Number";
            //cell[2] = new TableCell();
            //cell[2].Text = "Policy Mod";
            //cell[3] = new TableCell();
            //cell[3].Text = "Policy Effective";
            //cell[4] = new TableCell();
            //cell[4].Text = "Policy Expire";
            //cell[5] = new TableCell();
            //cell[5].Text = "Months To Val";
            //cell[6] = new TableCell();
            //cell[6].Text = "LDF Factor";
            //cell[7] = new TableCell();
            //cell[7].Text = "IBNR Factor";

            //AddStyle(cell, true);
            //tblRow.Cells.AddRange(cell);
            //tbl.Rows.Add(tblRow);


            DataTable dt = new DataTable();

            dt.Columns.Add("Policy Symbol");
            dt.Columns.Add("Policy Number");
            dt.Columns.Add("Policy Mod");
            dt.Columns.Add("Policy Effective");
            dt.Columns.Add("Policy Expire");
            dt.Columns.Add("Months To Val");
            dt.Columns.Add("LDF Factor");
            dt.Columns.Add("IBNR Factor");

            int i = 0;

            foreach (SteppedFactorBE steppedBE in lstSteppedBE)
            {

                // commented for AWS Cloud Migration changes

                //tblRow = new TableRow();

                //cell[0] = new TableCell();
                ////06/23 for veracode
                ////cell[0].Text = Convert.ToString(steppedBE.PolicySymbol);
                //cell[0].Text =  HttpUtility.HtmlEncode(Convert.ToString(steppedBE.PolicySymbol));// EAISA-5 veracode fix
                //cell[1] = new TableCell();
                ////06/23 for veracode
                ////cell[1].Text = Convert.ToString(steppedBE.PolicyNumber);
                //cell[1].Text =  HttpUtility.HtmlEncode(Convert.ToString(steppedBE.PolicyNumber));// EAISA-5 veracode fix
                //cell[2] = new TableCell();
                ////06/23 for veracode
                ////cell[2].Text = Convert.ToString(steppedBE.PolicyModulus);
                //cell[2].Text =  HttpUtility.HtmlEncode(Convert.ToString(steppedBE.PolicyModulus));// EAISA-5 veracode fix
                //cell[3] = new TableCell();
                //if (steppedBE.PolicyEffectiveDate.HasValue)
                //{
                //    cell[3].Text = steppedBE.PolicyEffectiveDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                //}
                //cell[4] = new TableCell();
                //cell[4].Text = steppedBE.PlanEndDate.ToString("MM/dd/yyyy");
                //cell[5] = new TableCell();
                //cell[5].Text = Convert.ToString(steppedBE.MONTHS_TO_VAL);
                //cell[6] = new TableCell();
                //cell[6].Text = Convert.ToString(steppedBE.LDF_FACTOR);
                //cell[7] = new TableCell();
                //cell[7].Text = Convert.ToString(steppedBE.IBNR_FACTOR);

                //AddStyle(cell, false);
                //tblRow.Cells.AddRange(cell);
                //tbl.Rows.Add(tblRow);

                if (dt.Rows.Count == 0)
                {
                    i = 0;
                }
                else
                {
                    i = i + 1;
                }
                dt.Rows.Add();

                dt.Rows[i][0] = HttpUtility.HtmlEncode(Convert.ToString(steppedBE.PolicySymbol));
                dt.Rows[i][1] = HttpUtility.HtmlEncode(Convert.ToString(steppedBE.PolicyNumber));
                dt.Rows[i][2] = HttpUtility.HtmlEncode(Convert.ToString(steppedBE.PolicyModulus));
                if (steppedBE.PolicyEffectiveDate.HasValue)
                {
                    dt.Rows[i][3] = steppedBE.PolicyEffectiveDate.GetValueOrDefault().ToString("MM/dd/yyyy");
                }
                dt.Rows[i][4] = steppedBE.PlanEndDate.ToString("MM/dd/yyyy");
                dt.Rows[i][5] = Convert.ToString(steppedBE.MONTHS_TO_VAL);
                dt.Rows[i][6] = Convert.ToString(steppedBE.LDF_FACTOR);
                dt.Rows[i][7] = Convert.ToString(steppedBE.IBNR_FACTOR);


            }

            //ExportToExcel(strFileName, tbl); // commented for AWS Cloud Migaratin changes
            //modalPolDetails.Show();

            string strFilePath = ConfigurationManager.AppSettings["PolicyFilesTempPath"] + "\\" + strFileName;
            string SheetName = "Stepped Factor Details";
            ViewPolicyandSteppedFactExportToExcelusingSpreadsheetGear(dt, strFilePath, strFileName, SheetName);
        }

        private void AddStyle(TableCell[] cells, bool isHeader)
        {
            foreach (TableCell cell in cells)
            {
                cell.Style[HtmlTextWriterStyle.BorderColor] = "Black";
                cell.Style[HtmlTextWriterStyle.BorderStyle] = BorderStyle.Solid.ToString();
                cell.Style[HtmlTextWriterStyle.BorderWidth] = "1";

                if (isHeader)
                    cell.Style[HtmlTextWriterStyle.FontStyle] = "bold";
            }
        }

        private string GetLookupNameByID(int lookupID)
        {
            LookupBS lkupBS = new LookupBS();
            LookupBE lkupBE = lkupBS.getLkupRow(lookupID);
            //06/23 for veracode
            //return lkupBE.LookUpName;
            return HttpUtility.HtmlEncode(Convert.ToString(lkupBE.LookUpName));
        }

        protected void btnSFCancel_Click(object sender, EventArgs e)
        {
            if (Session["strMessageErrorLog_SFUpload"] != null && Session["strMessageErrorLog_SFUpload"] != string.Empty)
            {
                Session["strMessageErrorLog_SFUpload"] = null;
                btnSFErrorLog.Visible = false;
            }
            mpeStepped.Hide();
        }
        // Gets data from excel sheet
        public DataTable GetDataTableExcelWithOutOledb(string strFileName)
        {
            try
            {
                //Getting workbook from given path
                SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(strFileName);
                //Getting active worksheet from workbook
                SpreadsheetGear.IWorksheet worksheet = workbook.ActiveWorksheet;
                //Getting cells from worksheet
                SpreadsheetGear.IRange cells = worksheet.Cells;

                //Getting used range of row count & column count from worksheet
                int rows = worksheet.UsedRange.RowCount;
                int cols = worksheet.UsedRange.ColumnCount;

                //Data table initialization
                System.Data.DataTable datatable = new DataTable();

                //For Column Headers
                for (int c = 0; c < cols; c++)
                {
                    datatable.Columns.Add(cells[0, c].Text);
                }

                //For rows and columns
                for (int r = 1; r < rows; r++)
                {
                    DataRow dr = datatable.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[c] = cells[r, c].Text;
                    }
                    datatable.Rows.Add(dr);
                }

                bool isEmpty = true;

                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    isEmpty = true;

                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        if (string.IsNullOrEmpty(datatable.Rows[i][j].ToString()) == false)
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty == true)
                    {
                        datatable.Rows.RemoveAt(i);
                        i--;
                    }
                }

                return datatable;
            }

            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

