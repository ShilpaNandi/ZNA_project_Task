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

namespace ZurichNA.AIS.WebSite.AcctSetup
{
    public partial class PolicyInfo : AISBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ppProgramPeriod.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ppProgramPeriod_ProgramPeriodRowClicked);
            if (!Page.IsPostBack)
            {
                this.Master.Page.Title = "Policy Information";
                //PolicyInfoTransWrapper = new AISBusinessTransaction();
                  SortBy="";
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
                lblDateRange.Text = "[" + proPerStartDate + " - " + proPerEndDate + "]";
                BindPolicyInformation();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, ex);
            }
        }
        private string SortBy
        {
            get { return (string)Session["SORTBY"]; }
            set { Session["SORTBY"] = value; }
        }
        private string SortDir
        {
            get { return (string)Session["SORTDIR"]; }
            set { Session["SORTDIR"] = value; }
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
            get { return (PolicyBE)Session["POLICYBE"]; }
            set { Session["POLICYBE"] = value; }
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
            if (ddlALAE.SelectedItem.Text=="Capped Amount")
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
                polBE.ISMasterPEOPolicy = chkPEOPolicy!=null?(chkPEOPolicy.Checked):false;
                //checking for An identical record if Policy Number and EffDate is same for a Program Period
                bool isPolExists = PolicyBizService.isPolicyAlreadyExist(polBE.PolicyID, polBE.PolicyNumber, Convert.ToDateTime(polBE.PolicyEffectiveDate), SelectedProgramPeriodID);
                //A policy number with an identical Symbol, Policy Number, Modulus, Effective Date, Expiration Date cannot span two Accounts
                bool isPolExistsInAnyAcct = PolicyBizService.isPolExistsInAnyAcct(polBE.PolicyID, polBE.PolicySymbol, polBE.PolicyNumber, polBE.PolicyModulus, Convert.ToDateTime(polBE.PolicyEffectiveDate), Convert.ToDateTime(polBE.PlanEndDate),AISMasterEntities.AccountNumber);
                if ((isPolExists == false) && (isPolExistsInAnyAcct == false))
                {   bool Result=false;
                    if (isNew==true)
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
                    isCopy =false;                    
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
                    if (isPolExists==true)                        
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

            if (Convert.ToInt32(ddlLossSource.SelectedValue)!=0)
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
        }
        /// <summary>
        /// Binds the data to Stepped  Factor List view
        /// </summary>
        private void BindSteppedFactorListView()
        {

            lstSteppedFactor.DataSource = SteppedFactorBizService.GetSteppedFactorData(SelectedPolicyID);
            lstSteppedFactor.DataBind();
            polBE = PolicyBizService.getPolicyInfo(SelectedPolicyID);
            lblSteppedFactor.Text = "Stepped Factors for Policy # " + polBE.PolicySymbol.Trim() + ' ' + polBE.PolicyNumber.Trim() + ' ' + polBE.PolicyModulus.Trim();
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

                       bool Flag= SteppedFactorBizService.Update(stepBE);
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
    }
}
