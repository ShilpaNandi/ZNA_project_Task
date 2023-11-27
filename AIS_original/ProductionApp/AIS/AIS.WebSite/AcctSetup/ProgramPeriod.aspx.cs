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

namespace ZurichNA.AIS.WebSite.AcctSetup
{
    public partial class ProgramPeriod : AISBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Page.Title = "Program Periods";
            chkUpdate.Checked = false;
            CheckBox chkList = (CheckBox)lstProgramPeriod.FindControl("chkSelectAll");
            if (chkList != null) chkList.Checked = false;

            if (!Page.IsPostBack)
            {                
                btnUpdate.Attributes.Add("disabled", "disabled");
                SortBy = "";
                SortDir = "";                   
                //Checking if the selected Account is PEO or not
                AccountBS accBS = new AccountBS();
                bool isAccountSetUPasPEO = accBS.isAccountSetUPasPEO(AISMasterEntities.AccountNumber);
                txtPEOPayIn.Enabled = isAccountSetUPasPEO;
                //Binding the List view with Program Periods Data   
                BindProgramPeriodData();
                //ProPrgTransWrapper = new AISBusinessTransaction();               

            }

            //if (((txtSubmitStsID.Text != "") && (txtSetupStsID.Text != "")) || ((chkSubmitForSetupQC.Checked) && (chkSetupQCComplete.Checked)))
            //    chkActive.Enabled = true;
            //else
            //    chkActive.InputAttributes.Add("disabled", "disabled");

            //Checks Exiting without Save
            CheckExitWithoutSave();
        }

        private void CheckExitWithoutSave()
        {
            ArrayList list = new ArrayList();
            list.Add(txtActiveOldsts);
            list.Add(txtActiveStsID);
            list.Add(txtBKTCYBUYOUTEffDate);
            list.Add(txtCombEleMax);
            list.Add(txtConvertsAT);
            list.Add(txtEffDtHidden);
            list.Add(txtEffectiveDate);
            list.Add(txtExpirationDate);
            list.Add(txtFinalAdj);
            list.Add(txtFinalAdjNONPREM);
            list.Add(txtFirstAdj);
            list.Add(txtFirstAdjDt);
            list.Add(txtFirstAdjDtNP);
            list.Add(txtFirstAdjNONPREM);
            list.Add(txtFreq);
            list.Add(txtFreqNonPrem);
            list.Add(txtFstAdjHidden);
            list.Add(txtHidenValDt);
            list.Add(txtInitialOldSts);
            list.Add(txtInitialStsID);
            list.Add(txtLSIEndDate);
            list.Add(txtLSIRetriveFromDt);
            list.Add(txtLSIStartDate);
            list.Add(txtNextValDt);
            list.Add(txtNextValDtNP);
            list.Add(txtPEOPayIn);
            list.Add(txtPrevValDt);
            list.Add(txtPrevValDtNP);
            list.Add(txtSetupOldSts);
            list.Add(txtSetupStsID);
            list.Add(txtSubmitOldSts);
            list.Add(txtSubmitStsID);
            list.Add(txtTMFactor);
            list.Add(txtValDt);
            list.Add(CalendarExtender1);
            list.Add(CalendarExtender12);
            list.Add(CalendarExtender2);
            list.Add(CalendarExtender3);
            list.Add(CalendarExtender4);
            list.Add(CalendarExtender5);
            list.Add(CalendarExtender6);
            list.Add(CalendarExtender7);
            list.Add(chkActive);
            list.Add(chkAvgTM);
            list.Add(chkCHFZSC);
            list.Add(chkInclCptvPKCodes);
            list.Add(chkInitial);
            list.Add(chkSetupQCComplete);
            list.Add(chkSubmitForSetupQC);
            list.Add(chkUpdate);
            list.Add(ddlBKTCYBUYOUT);
            list.Add(ddlBroker);
            list.Add(ddlBrokerContact);
            list.Add(ddlBrokerDetail);
            list.Add(ddlBU);
            list.Add(ddlBUDetails);
            list.Add(ddlPaidIncurred);
            list.Add(ddlProgramType);
            list.Add(btnAdd);
            list.Add(btnCancel);
            list.Add(btnCopy);
            list.Add(btnSave);
            list.Add(btnUpdate);
            list.Add(lbCloseDetails);
            ProcessExitFlag(list);
        }

        public string GetProgPerCheckBoxes(string id,string active)
        {
            if (active=="True")
                return ("<input type='checkbox' id='chkSelProgPer' name='chkSelProgPer' value='" + id.ToString() + "'  />");
            else
                return ("<input type='checkbox' id='chkSelProgPer' name='chkSelProgPer' disabled='True' value='" + id.ToString() + "'  />");
            
        }

        //protected AISBusinessTransaction ProPrgTransWrapper
        //{
        //    get
        //    {
        //        if ((AISBusinessTransaction)Session["PrpPrgTransaction"] == null)
        //            Session["PrpPrgTransaction"] = new AISBusinessTransaction();
        //        return (AISBusinessTransaction)Session["PrpPrgTransaction"];
        //    }
        //    set
        //    {
        //        Session["PrpPrgTransaction"] = value;
        //    }
        //}

        private ProgramPeriodsBS programPeriodsBizService;
        private ProgramPeriodsBS ProgramPeriodsBizService
        {
            get
            {
                if (programPeriodsBizService == null)
                {
                    programPeriodsBizService = new ProgramPeriodsBS();
                    // programPeriodsBizService.AppTransactionWrapper = ProPrgTransWrapper;
                }
                return programPeriodsBizService;
            }
        }

        private PremiumAdjustmentProgramStatusBS programStatusBizService;
        private PremiumAdjustmentProgramStatusBS ProgramStatusBizService
        {
            get
            {
                if (programStatusBizService == null)
                {
                    programStatusBizService = new PremiumAdjustmentProgramStatusBS();
                    // programStatusBizService.AppTransactionWrapper = ProPrgTransWrapper;
                }
                return programStatusBizService;
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
                    //   auditBizService.AppTransactionWrapper = ProPrgTransWrapper;
                }
                return auditBizService;
            }
        }

        private ProgramPeriodBE progPerBE
        {
            get { return (ProgramPeriodBE)Session["PROGRAMPERIODBE"]; }
            set { Session["PROGRAMPERIODBE"] = value; }
        }

        private IList<PremiumAdjustmentProgramStatusBE> programStatusList
        {
            get { return (IList<PremiumAdjustmentProgramStatusBE>)Session["PROGRAMSTATUSLIST"]; }
            set { Session["PROGRAMSTATUSLIST"] = value; }
        }

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
        protected int SelectedValue
        {
            get
            {
                if (hidSelValue.Value != null) return Convert.ToInt32(hidSelValue.Value);
                else
                    return -1;
            }
            set { hidSelValue.Value = value.ToString(); }
        }
        /// <summary>
        /// Binding the Program Period List view
        /// </summary>
        private void BindProgramPeriodData()
        {

            lstProgramPeriod.DataSource = GetSortedProgramPeriodData();

            lstProgramPeriod.DataBind();
        }
        private IList<ProgramPeriodListBE> GetSortedProgramPeriodData()
        {
            IList<ProgramPeriodListBE> ProgramPeriodsList = new List<ProgramPeriodListBE>();
            ProgramPeriodsList = ProgramPeriodsBizService.GetActiveInActiveProgramPeriods(AISMasterEntities.AccountNumber);
            //If user click on any column sort
            switch (SortBy)
            {
                case "PROGRAMTYPENAME":
                    if (SortDir == "ASC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderBy(o => o.PROGRAMTYPENAME)).ToList();
                    else if (SortDir == "DESC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderByDescending(o => o.PROGRAMTYPENAME)).ToList();
                    break;

                case "BROKERNAME":
                    if (SortDir == "ASC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderBy(o => o.BROKERNAME)).ToList();
                    else if (SortDir == "DESC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderByDescending(o => o.BROKERNAME)).ToList();

                    break;
                case "BUSINESSUNITNAME":
                    if (SortDir == "ASC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderBy(o => o.BUSINESSUNITNAME)).ToList();
                    else if (SortDir == "DESC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderByDescending(o => o.BUSINESSUNITNAME)).ToList();

                    break;
                case "VALN_MM_DT":
                    if (SortDir == "ASC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderBy(o => o.VALN_MM_DT)).ToList();
                    else if (SortDir == "DESC")
                        ProgramPeriodsList = (ProgramPeriodsList.OrderByDescending(o => o.VALN_MM_DT)).ToList();

                    break;
            }
            return ProgramPeriodsList;


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
        protected void lstProgramPeriod_Sorting(object sender, ListViewSortEventArgs e)
        {
            Image imgPROGRAMTYPENAME = (Image)lstProgramPeriod.FindControl("imgPROGRAMTYPENAME");
            Image imgBROKERNAME = (Image)lstProgramPeriod.FindControl("imgBROKERNAME");
            Image imgBUSINESSUNITNAME = (Image)lstProgramPeriod.FindControl("imgBUSINESSUNITNAME");
            Image imgVALN_MM_DT = (Image)lstProgramPeriod.FindControl("imgVALN_MM_DT");

            Image img = new Image();
            switch (e.SortExpression)
            {
                case "PROGRAMTYPENAME":
                    SortBy = "PROGRAMTYPENAME";
                    imgPROGRAMTYPENAME.Visible = true;
                    imgBROKERNAME.Visible = false;
                    imgBUSINESSUNITNAME.Visible = false;
                    imgVALN_MM_DT.Visible = false;
                    img = imgPROGRAMTYPENAME;
                    break;

                case "BROKERNAME":
                    SortBy = "BROKERNAME";
                    imgPROGRAMTYPENAME.Visible = false;
                    imgBROKERNAME.Visible = true;
                    imgBUSINESSUNITNAME.Visible = false;
                    imgVALN_MM_DT.Visible = false;
                    img = imgBROKERNAME;
                    break;
                case "BUSINESSUNITNAME":
                    SortBy = "BUSINESSUNITNAME";
                    imgPROGRAMTYPENAME.Visible = false;
                    imgBROKERNAME.Visible = false;
                    imgBUSINESSUNITNAME.Visible = true;
                    imgVALN_MM_DT.Visible = false;
                    img = imgBUSINESSUNITNAME;
                    break;
                case "VALN_MM_DT":
                    SortBy = "VALN_MM_DT";
                    imgPROGRAMTYPENAME.Visible = false;
                    imgBROKERNAME.Visible = false;
                    imgBUSINESSUNITNAME.Visible = false;
                    imgVALN_MM_DT.Visible = true;
                    img = imgVALN_MM_DT;
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
            BindProgramPeriodData();
        }

        /// <summary>
        /// Invoked when User Checks/Uncecks the Update checkbox
        /// If it is checked then the Update panel with Broker and BU/Office dropdown will be enabled
        /// If it is Unchecked then the Update panel with Broker and BU/Office dropdown will be Disabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void chkUpdate_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (chkUpdate.Checked)
        //            pnlUpdate.Enabled = true;
        //        else if (!(chkUpdate.Checked))
        //        { pnlUpdate.Enabled = false; }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowError(ex.Message);
        //    }
        //}
        /// <summary>
        /// This event fires when user selects on any button of Program period List view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstProgramPeriod_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //User selects a Program Period by clicking on Select Link
            if (e.CommandName.ToUpper() == "SELECT")
            {
                isNew = false;
                isCopy = false;
                //if User is with Inquiry roll, No edit,create,Active/Inactive will be given
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                {
                    btnSave.Enabled = true;
                    btnCopy.Enabled = true;
                }
                ClearFileds();

                int intProgramPeriodID = Convert.ToInt32(e.CommandArgument);
                SelectedValue = intProgramPeriodID;
                //SelectedIndex = e.NewSelectedIndex;
                DisplayDetails(intProgramPeriodID);
                pnlDetails.Visible = true;
                lblProgramPeriodDetails.Visible = true;
                lbCloseDetails.Visible = true;
                pnlProgramPeriod.Height = Unit.Point(80);

                lstProgramPeriod.Enabled = false;
                pnlUpdateSelectedPeriod.Enabled = false;
                //btnAdd.Enabled = false;
                //if User is with Inquiry roll, No edit,create,Active/Inactive will be given
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                {
                    btnCancel.Enabled = true;
                    btnCopy.Enabled = true;
                }
                btnSave.Text = "Update";
            }

            try
            {
                //User selects on Active/Inactive Image
                if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
                {
                    int prpPreID = Convert.ToInt32(e.CommandArgument);
                    progPerBE = ProgramPeriodsBizService.getProgramPeriodInfo(prpPreID);
                    if (e.CommandName.ToUpper() == "DISABLE")
                        progPerBE.ACTV_IND = false;
                    else
                        progPerBE.ACTV_IND = true;

                    progPerBE.UPDATE_DATE = DateTime.Now;
                    progPerBE.UPDATE_USER_ID = CurrentAISUser.PersonID;

                    bool boolIsProgPerSaved = ProgramPeriodsBizService.Update(progPerBE);

                    if (boolIsProgPerSaved) AuditBizService.Save(AISMasterEntities.AccountNumber, prpPreID, GlobalConstants.AuditingWebPage.ProgramPeriodSetup, CurrentAISUser.PersonID);

                    //ProPrgTransWrapper.SubmitTransactionChanges();
                    BindProgramPeriodData();
                }
            }
            catch (Exception ex)
            {
                //ProPrgTransWrapper.RollbackChanges();
                ShowError(ex.Message, ex);
            }

        }
        //Highlite the selected row
        protected void lstProgramPeriod_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            HtmlTableRow tr;
            if (ViewState["SelectedIndex"] != null)
            {
                int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                tr = (HtmlTableRow)lstProgramPeriod.Items[index].FindControl("trItemTemplate");
                if ((index % 2) == 0)
                    tr.Attributes["class"] = "ItemTemplate";
                else
                    tr.Attributes["class"] = "AlternatingItemTemplate";
            }
            tr = (HtmlTableRow)lstProgramPeriod.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
            tr.Attributes["class"] = "SelectedItemTemplate";
            ViewState["SelectedIndex"] = e.NewSelectedIndex;
        }
        protected void lstProgramPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstProgramPeriod.SelectedIndex = -1;

        }
        protected void lstProgramPeriod_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";                

                ImageButton imgProPreEnableDisable = (ImageButton)e.Item.FindControl("imgProPreEnableDisable");

                if (imgProPreEnableDisable != null)
                {

                    Label lblProPreActive = (Label)e.Item.FindControl("lblProPreActive");
                    if (lblProPreActive.Text == "True")
                    {
                        //Part of role based security
                        if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager && CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
                        {
                            imgProPreEnableDisable.Enabled = false;
                        }
                        imgProPreEnableDisable.ImageUrl = "~/images/disabled.GIF";
                        imgProPreEnableDisable.CommandName = "DISABLE";
                        imgProPreEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable?');");
                        imgProPreEnableDisable.ToolTip = "Click here to Disable this Program Period";
                    }
                    else
                    {
                        imgProPreEnableDisable.ImageUrl = "~/images/enabled.GIF";
                        imgProPreEnableDisable.CommandName = "ENABLE";
                        imgProPreEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable?');");
                        imgProPreEnableDisable.ToolTip = "Click here to Enable this Program Period";
                    }
                }
                //  ((CheckBox)e.Item.FindControl("chkSelectAll")).Checked = false;
            }
        }
        /// <summary>
        /// Event when use clicks on 'Add new' Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ViewState["SelectedIndex"] != null)
            {
                int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                HtmlTableRow tr = (HtmlTableRow)lstProgramPeriod.Items[index].FindControl("trItemTemplate");
                tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
            }
            ViewState["SelectedIndex"] = null;
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                btnSave.Enabled = true;
            }
            isNew = true;
            isCopy = false;
            ClearFileds();
            lblProgramPeriodDetails.Visible = true;
            lbCloseDetails.Visible = true;
            pnlDetails.Visible = true;
            pnlProgramPeriod.Height = Unit.Point(80);

            lstProgramPeriod.Enabled = false;
            pnlUpdateSelectedPeriod.Enabled = false;
            btnAdd.Enabled = false;
            btnCopy.Enabled = false;
            ddlProgramType.Enabled = true;
            btnSave.Text = "Save";
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
                    ShowMessage("Program Period Copy process has been Cancelled");
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        btnCopy.Enabled = true;
                    DisplayDetails(SelectedValue);
                }
                else
                {
                    ClearFileds();
                }
            }
            else
            {
                DisplayDetails(SelectedValue);
            }
        }
        protected void lbCloseDetails_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = false;
            lblProgramPeriodDetails.Visible = false;
            lbCloseDetails.Visible = false;
            pnlProgramPeriod.Height = Unit.Point(300);

            lstProgramPeriod.Enabled = true;
            pnlUpdateSelectedPeriod.Enabled = true;
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnAdd.Enabled = true;

        }
        /// <summary>
        /// Event when users clicks on Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SAVE_Click(Object sender, EventArgs e)
        {
            bool isDataChanged = false;
            int OldProgramPeriodID = 0;
            try
            {

                //Checking if it is New
                //If it is existing then loading the BE using Framework and editing the same BE
                if (isNew == false)
                {
                    progPerBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                    progPerBE.UPDATE_DATE = DateTime.Now;
                    isDataChanged = IsDataChanged(progPerBE);
                    //isDataChanged = true;

                }
                else
                {
                    if (isCopy) OldProgramPeriodID = progPerBE.PREM_ADJ_PGM_ID; //will be used if copy is selected
                    progPerBE = new ProgramPeriodBE();
                    progPerBE.AGMT_ALOC_LOS_ADJ_EXPS_IND = false;
                    progPerBE.AGMT_UNALOCTD_LOS_ADJ_IND = false;
                    progPerBE.AGMT_LOS_BASE_ASSES_IND = false;
                    progPerBE.LSI_ALOC_LOS_ADJ_EXPS_IND = false;
                    progPerBE.LSI_UNALOCTD_LOS_ADJ_IND = false;
                    progPerBE.LSI_LOS_BASE_ASSES_IND = false;
                    progPerBE.AGMT_PAID_INCUR_IND = false;
                    progPerBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    progPerBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                    progPerBE.CREATE_DATE = DateTime.Now;
                    progPerBE.ACTV_IND = true;
                }
                if (txtEffectiveDate.Text != "")
                    progPerBE.STRT_DT = DateTime.Parse(txtEffectiveDate.Text);
                else
                    progPerBE.STRT_DT = null;

                if (txtExpirationDate.Text != "")
                    progPerBE.PLAN_END_DT = DateTime.Parse(txtExpirationDate.Text);
                else
                    progPerBE.PLAN_END_DT = null;

                if (txtLSIEndDate.Text != "")
                    progPerBE.LOS_SENS_INFO_END_DT = DateTime.Parse(txtLSIEndDate.Text);
                else
                    progPerBE.LOS_SENS_INFO_END_DT = DateTime.Parse(txtExpirationDate.Text);

                if (txtLSIStartDate.Text != "")
                    progPerBE.LOS_SENS_INFO_STRT_DT = DateTime.Parse(txtLSIStartDate.Text);
                else
                    progPerBE.LOS_SENS_INFO_STRT_DT = DateTime.Parse(txtEffectiveDate.Text);

                if (Convert.ToInt32(ddlBrokerDetail.SelectedValue) != 0)
                    progPerBE.BRKR_ID = Convert.ToInt32(ddlBrokerDetail.SelectedValue);
                else
                    progPerBE.BRKR_ID = null;

                if (Convert.ToInt32(ddlBrokerContact.SelectedValue) != 0)
                    progPerBE.BRKR_CONCTC_ID = Convert.ToInt32(ddlBrokerContact.SelectedValue);
                else
                    progPerBE.BRKR_CONCTC_ID = null;

                if (Convert.ToInt32(ddlProgramType.SelectedValue) != 0)
                    progPerBE.PGM_TYP_ID = Convert.ToInt32(ddlProgramType.SelectedValue);
                else
                    progPerBE.PGM_TYP_ID = null;

                if (Convert.ToInt32(ddlBUDetails.SelectedValue) != 0)
                    progPerBE.BSN_UNT_OFC_ID = Convert.ToInt32(ddlBUDetails.SelectedValue);
                else
                    progPerBE.BSN_UNT_OFC_ID = null;
                if (Convert.ToInt32(ddlPaidIncurred.SelectedValue) != 0)
                    progPerBE.PAID_INCUR_TYP_ID = Convert.ToInt32(ddlPaidIncurred.SelectedValue);
                else
                    progPerBE.PAID_INCUR_TYP_ID = null;

                if (txtConvertsAT.Text != "")
                    progPerBE.INCUR_CONV_MMS_CNT = Convert.ToInt32(txtConvertsAT.Text);
                else
                    progPerBE.INCUR_CONV_MMS_CNT = null;

                if (txtFirstAdj.Text != "")
                    progPerBE.FST_ADJ_MMS_FROM_INCP_CNT = Convert.ToInt32(txtFirstAdj.Text);
                else
                    progPerBE.FST_ADJ_MMS_FROM_INCP_CNT = null;

                if (txtHidenValDt.Text != "")
                {
                    //progPerBE.VALN_MM_DT = DateTime.Parse(txtValDt.Text);
                    //to get the full date along with the YEAR. storing and rettiving from hidden text box
                    progPerBE.VALN_MM_DT = DateTime.Parse(txtHidenValDt.Text);
                }
                //If the program period effective date + FirstAdj (months) is  from the 16th -31st will produce a 
                //valuation date of the calculated current month end.  Effective dates of 1st – 15th will produce a 
                //valuation date for the prior month end.
                else if (txtHidenValDt.Text == "")
                {
                    DateTime myDate = DateTime.Parse(txtEffectiveDate.Text).AddMonths(Convert.ToInt32(txtFirstAdj.Text));
                    if (myDate.Day > 15)
                    {
                        myDate = myDate.AddMonths(1);  //Add a month to get the Last date of the month
                        // remove all of the days in the next month to get bumped down to the last day of the previous month
                        myDate = myDate.AddDays(-(myDate.Day));
                    }
                    else if (myDate.Day < 16)
                    {
                        //newdt = newdt.AddMonths(1);    //Add a month to get the Last date of the month
                        //newdt = newdt.AddMonths(-1);   //if the Date is Less then 16th previous months lastdate                 
                        myDate = myDate.AddDays(-(myDate.Day));
                    }
                    progPerBE.VALN_MM_DT = myDate;
                }
                else
                    progPerBE.VALN_MM_DT = null;


                if (txtFreq.Text != "")
                    progPerBE.ADJ_FREQ_MMS_INTVRL_CNT = Convert.ToInt32(txtFreq.Text);
                else
                    progPerBE.ADJ_FREQ_MMS_INTVRL_CNT = null;

                if (Convert.ToInt32(ddlBKTCYBUYOUT.SelectedValue) != 0)
                    progPerBE.BNKRPT_BUYOUT_ID = Convert.ToInt32(ddlBKTCYBUYOUT.SelectedValue);
                else
                    progPerBE.BNKRPT_BUYOUT_ID = null;

                if (txtBKTCYBUYOUTEffDate.Text != "")
                    progPerBE.BNKRPT_BUYOUT_EFF_DT = DateTime.Parse(txtBKTCYBUYOUTEffDate.Text);
                else
                    progPerBE.BNKRPT_BUYOUT_EFF_DT = null;

                if (txtFinalAdj.Text != "")
                    progPerBE.FNL_ADJ_DT = DateTime.Parse(txtFinalAdj.Text);
                else
                    progPerBE.FNL_ADJ_DT = null;

                progPerBE.INCLD_CAPTV_PAYKND_IND = chkInclCptvPKCodes.Checked;
                progPerBE.AVG_TAX_MULTI_IND = chkAvgTM.Checked;

                if (txtCombEleMax.Text != "")
                    progPerBE.COMB_ELEMTS_MAX_AMT = Convert.ToDecimal(txtCombEleMax.Text);
                else
                    progPerBE.COMB_ELEMTS_MAX_AMT = null;

                progPerBE.ZNA_SERV_COMP_CLM_HNDL_FEE_IND = chkCHFZSC.Checked;

                if (txtPEOPayIn.Text != "")
                    progPerBE.PEO_PAY_IN_AMT = Convert.ToDecimal(txtPEOPayIn.Text.Replace(",", ""));
                else
                    progPerBE.PEO_PAY_IN_AMT = null;

                if (txtFirstAdjNONPREM.Text != "")
                    progPerBE.FST_ADJ_NON_PREM_MMS_CNT = Convert.ToInt32(txtFirstAdjNONPREM.Text);
                else
                    //If user did not enter First Adj Non Premium, Default will be FirstAdj
                    progPerBE.FST_ADJ_NON_PREM_MMS_CNT = Convert.ToInt32(txtFirstAdj.Text);

                if (txtFreqNonPrem.Text != "")
                    progPerBE.FREQ_NON_PREM_MMS_CNT = Convert.ToInt32(txtFreqNonPrem.Text);
                else
                    //IF User did not enter any value for Freq Non Prem, Default will be Adjustment Frequency
                    progPerBE.FREQ_NON_PREM_MMS_CNT = Convert.ToInt32(txtFreq.Text);

                if (txtFinalAdjNONPREM.Text != "")
                    progPerBE.FNL_ADJ_NON_PREM_DT = DateTime.Parse(txtFinalAdjNONPREM.Text);
                else
                    progPerBE.FNL_ADJ_NON_PREM_DT = null;
                //string strTMFactor = (txtTMFactor.Text.Replace("_", "0"));
                if (txtTMFactor.Text != "")
                    progPerBE.TAX_MULTI_FCTR_RT = Convert.ToDecimal(txtTMFactor.Text);
                else
                    progPerBE.TAX_MULTI_FCTR_RT = null;


                if (txtFirstAdjDt.Text != "")
                    progPerBE.FST_ADJ_DT = DateTime.Parse(txtFirstAdjDt.Text);
                else
                    progPerBE.FST_ADJ_DT = null;
                //1. The LSI Retrieve From Date must be set to the "Program Period Start Date" when the "Prev Val Date" is null. 
                //2. The LSI Retrieve From Date should be equal to "Prev Val Date" when "Prev Val Date" is not null. 
                if (txtLSIRetriveFromDt.Text != "")
                    progPerBE.LSI_RETRIEVE_FROM_DT = DateTime.Parse(txtLSIRetriveFromDt.Text);
                else if (txtLSIRetriveFromDt.Text == "")
                {
                    if (txtPrevValDt.Text != "")
                        progPerBE.LSI_RETRIEVE_FROM_DT = DateTime.Parse(txtPrevValDt.Text);
                    else
                        progPerBE.LSI_RETRIEVE_FROM_DT = DateTime.Parse(txtEffectiveDate.Text);
                }

                if (txtNextValDt.Text != "")
                    progPerBE.NXT_VALN_DT = Convert.ToDateTime(txtNextValDt.Text);
                else
                    progPerBE.NXT_VALN_DT = null;
                if (txtNextValDtNP.Text != "")
                    progPerBE.NXT_VALN_DT_NON_PREM_DT = Convert.ToDateTime(txtNextValDtNP.Text);
                else
                    progPerBE.NXT_VALN_DT_NON_PREM_DT = null;

                if (txtFirstAdjDtNP.Text != "")
                    progPerBE.FST_ADJ_NON_PREM_DT = Convert.ToDateTime(txtFirstAdjDtNP.Text);
                else
                    progPerBE.FST_ADJ_NON_PREM_DT = null;

                //Checking for not to have Duplicate Records
                bool boolIsProgramPeriodExits = ProgramPeriodsBizService.IsProgramPeriodExits(progPerBE.PREM_ADJ_PGM_ID, Convert.ToInt32(ddlBrokerDetail.SelectedValue), Convert.ToInt32(ddlBUDetails.SelectedValue), Convert.ToDateTime(txtEffectiveDate.Text), Convert.ToDateTime(txtExpirationDate.Text), AISMasterEntities.AccountNumber, Convert.ToDateTime(txtHidenValDt.Text), Convert.ToInt32(ddlProgramType.SelectedValue));
                if (!boolIsProgramPeriodExits)
                {
                    if (isNew)
                    {
                        bool boolIsProgPerSaved = ProgramPeriodsBizService.Update(progPerBE);
                        SaveProgramStatus(progPerBE.PREM_ADJ_PGM_ID);
                        //ProPrgTransWrapper.SubmitTransactionChanges();
                    }
                    else if ((!isNew))
                    {
                        bool boolIsProgPerSaved = ProgramPeriodsBizService.Update(progPerBE);
                        ShowConcurrentConflict(boolIsProgPerSaved, progPerBE.ErrorMessage);
                        bool flag=SaveProgramStatus(progPerBE.PREM_ADJ_PGM_ID);
                        ShowConcurrentConflict(flag, progPerBE.ErrorMessage);
                        if (boolIsProgPerSaved && isDataChanged) AuditBizService.Save(AISMasterEntities.AccountNumber, SelectedValue, GlobalConstants.AuditingWebPage.ProgramPeriodSetup, CurrentAISUser.PersonID);
                        //ProPrgTransWrapper.SubmitTransactionChanges();
                    }
                    if (isCopy)
                    {
                        //The system will copy all Program Period Setup information, including Retro Information, 
                        //Policy Information, Audit Information, LBA, LCF, TM, CHF, RML, ILRF, Combined Elements and Escrow, 
                        //for a Program Period when an Active Program Period is copied to a new Program Period.
                        CopyDependencies(OldProgramPeriodID, progPerBE.PREM_ADJ_PGM_ID);

                        //ProPrgTransWrapper.RollbackChanges();
                        //else
                        //ProPrgTransWrapper.SubmitTransactionChanges();
                    }
                    isNew = false;
                    isCopy = false;

                    btnSave.Enabled = true;
                    btnCopy.Enabled = true;
                    btnCancel.Enabled = true;
                    BindProgramPeriodData();
                    SelectedValue = progPerBE.PREM_ADJ_PGM_ID;
                    DisplayDetails(SelectedValue);
                    //to show the Listview row selected
                    LinkButton lbSelect;
                    HtmlTableRow tr;
                    int selectedindex = 0;
                    for (int i = 0; i < lstProgramPeriod.Items.Count; i++)
                    {
                        lbSelect = (LinkButton)lstProgramPeriod.Items[i].FindControl("lbSelect");
                        if (Convert.ToInt32(lbSelect.CommandArgument) == SelectedValue)
                        {
                            selectedindex = i;
                            break;
                        }
                    }
                    tr = (HtmlTableRow)lstProgramPeriod.Items[selectedindex].FindControl("trItemTemplate");
                    tr.Attributes["class"] = "SelectedItemTemplate";
                    ViewState["SelectedIndex"] = selectedindex;
                    //pnlProgramPeriod.Height = Unit.Point(300);
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        btnAdd.Enabled = true;
                    //lstProgramPeriod.Enabled = true;
                    pnlUpdateSelectedPeriod.Enabled = true;

                }
                else
                {
                    bool boolIsProgramPeriodExitsAndDisabled = ProgramPeriodsBizService.IsProgramPeriodExitsAndDisabled(progPerBE.PREM_ADJ_PGM_ID, Convert.ToInt32(ddlBrokerDetail.SelectedValue), Convert.ToInt32(ddlBUDetails.SelectedValue), Convert.ToDateTime(txtEffectiveDate.Text), Convert.ToDateTime(txtExpirationDate.Text), AISMasterEntities.AccountNumber, Convert.ToDateTime(txtHidenValDt.Text), Convert.ToInt32(ddlProgramType.SelectedValue));
                    if (boolIsProgramPeriodExitsAndDisabled == true) ShowMessage("The record cannot be saved. An identical record already exists. Please enable existing program period. ");
                    else ShowMessage("The record cannot be saved. An identical record already exists.");
                }
            }
            catch (Exception ex)
            {
                //ProPrgTransWrapper.RollbackChanges();
                ShowError(ex.Message, ex);
            }
            //lblProgramPeriodDetails.Visible = false;
            //pnlDetails.Visible = false;
            //lbCloseDetails.Visible = false;

        }

        private bool SaveProgramStatus(int ProgramPeriodID)
        {
            BLAccess BLAcc = new BLAccess();
            int initialLookUpID = BLAcc.GetLookUpID("INITIAL");
            int submitLookUPID = BLAcc.GetLookUpID("SUBMIT - SETUP QC");
            int setupLookUpID = BLAcc.GetLookUpID("SETUP QC - COMPLETE");
            int activeLookUpID = BLAcc.GetLookUpID("ACTIVE");

            if (isNew)
            {
                if (chkInitial.Checked)
                {
                    PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                    prgStsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    prgStsBE.PREM_ADJ_PGM_ID = ProgramPeriodID;
                    prgStsBE.PGM_STATUS_ID = initialLookUpID;
                    prgStsBE.STS_CHK_IND = true;
                    prgStsBE.CREATEDDATE = DateTime.Now;
                    prgStsBE.CREATEDUSER_ID = CurrentAISUser.PersonID;

                    ProgramStatusBizService.Update(prgStsBE);
                }
                if (chkSubmitForSetupQC.Checked)
                {
                    PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                    prgStsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    prgStsBE.PREM_ADJ_PGM_ID = ProgramPeriodID;
                    prgStsBE.PGM_STATUS_ID = submitLookUPID;
                    prgStsBE.STS_CHK_IND = true;
                    prgStsBE.CREATEDDATE = DateTime.Now;
                    prgStsBE.CREATEDUSER_ID = CurrentAISUser.PersonID;

                    ProgramStatusBizService.Update(prgStsBE);
                }
                if (chkSetupQCComplete.Checked)
                {
                    PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                    prgStsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    prgStsBE.PREM_ADJ_PGM_ID = ProgramPeriodID;
                    prgStsBE.PGM_STATUS_ID = setupLookUpID;
                    prgStsBE.STS_CHK_IND = true;
                    prgStsBE.CREATEDDATE = DateTime.Now;
                    prgStsBE.CREATEDUSER_ID = CurrentAISUser.PersonID;

                    ProgramStatusBizService.Update(prgStsBE);
                }
                if (chkActive.Checked)
                {
                    PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                    prgStsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    prgStsBE.PREM_ADJ_PGM_ID = ProgramPeriodID;
                    prgStsBE.PGM_STATUS_ID = activeLookUpID;
                    prgStsBE.STS_CHK_IND = true;
                    prgStsBE.CREATEDDATE = DateTime.Now;
                    prgStsBE.CREATEDUSER_ID = CurrentAISUser.PersonID;

                    ProgramStatusBizService.Update(prgStsBE);
                }
            }
            else if (!isNew)
            {
                if ((chkSubmitForSetupQC.Checked) && (txtSubmitStsID.Text == ""))
                {
                    PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                    prgStsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    prgStsBE.PREM_ADJ_PGM_ID = ProgramPeriodID;
                    prgStsBE.PGM_STATUS_ID = submitLookUPID;
                    prgStsBE.STS_CHK_IND = true;
                    prgStsBE.CREATEDDATE = DateTime.Now;
                    prgStsBE.CREATEDUSER_ID = CurrentAISUser.PersonID;

                    ProgramStatusBizService.Update(prgStsBE);
                }
                else if (txtSubmitStsID.Text != "")
                {
                    //comapring with old status. If any change then only update
                    if (Convert.ToBoolean(txtSubmitOldSts.Text) != chkSubmitForSetupQC.Checked)
                    {
                        PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                        prgStsBE = ProgramStatusBizService.getPremAdjProgSt(Convert.ToInt32(txtSubmitStsID.Text));
                        prgStsBE.UPDATEDATE = DateTime.Now;
                        prgStsBE.UPDATEUSER_ID = CurrentAISUser.PersonID;
                        prgStsBE.STS_CHK_IND = chkSubmitForSetupQC.Checked;
                        ProgramStatusBizService.Update(prgStsBE);
                    }
                }

                if ((chkSetupQCComplete.Checked) && (txtSetupStsID.Text == ""))
                {
                    PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                    prgStsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    prgStsBE.PREM_ADJ_PGM_ID = ProgramPeriodID;
                    prgStsBE.PGM_STATUS_ID = setupLookUpID;
                    prgStsBE.STS_CHK_IND = true;
                    prgStsBE.CREATEDDATE = DateTime.Now;
                    prgStsBE.CREATEDUSER_ID = CurrentAISUser.PersonID;

                    ProgramStatusBizService.Update(prgStsBE);
                }
                else if (txtSetupStsID.Text != "")
                {
                    //comapring with old status. If any change then only update
                    if (Convert.ToBoolean(txtSetupOldSts.Text) != chkSetupQCComplete.Checked)
                    {
                        PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                        prgStsBE = ProgramStatusBizService.getPremAdjProgSt(Convert.ToInt32(txtSetupStsID.Text));
                        prgStsBE.UPDATEDATE = DateTime.Now;
                        prgStsBE.UPDATEUSER_ID = CurrentAISUser.PersonID;
                        prgStsBE.STS_CHK_IND = chkSetupQCComplete.Checked;
                        ProgramStatusBizService.Update(prgStsBE);
                    }
                }

                if ((chkActive.Checked) && (txtActiveStsID.Text == ""))
                {
                    PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                    prgStsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    prgStsBE.PREM_ADJ_PGM_ID = ProgramPeriodID;
                    prgStsBE.PGM_STATUS_ID = activeLookUpID;
                    prgStsBE.STS_CHK_IND = true;
                    prgStsBE.CREATEDDATE = DateTime.Now;
                    prgStsBE.CREATEDUSER_ID = CurrentAISUser.PersonID;
                    ProgramStatusBizService.Update(prgStsBE);
                    //all status checkboxes has to disble ones user selects Active status
                    //this is for normal users. For manager those will be enabled
                    EnableDisableStatusChechkBoxes();

                }
                else if (txtActiveStsID.Text != "")
                {
                    //comapring with old status. If any change then only update
                    if (Convert.ToBoolean(txtActiveOldsts.Text) != chkActive.Checked)
                    {
                        PremiumAdjustmentProgramStatusBE prgStsBE = new PremiumAdjustmentProgramStatusBE();
                        prgStsBE = ProgramStatusBizService.getPremAdjProgSt(Convert.ToInt32(txtActiveStsID.Text));
                        prgStsBE.UPDATEDATE = DateTime.Now;
                        prgStsBE.UPDATEUSER_ID = CurrentAISUser.PersonID;
                        prgStsBE.STS_CHK_IND = chkActive.Checked;
                        ProgramStatusBizService.Update(prgStsBE);
                    }
                }


            }
            return true;
        }

        private bool CopyDependencies(int oldProgPerID, int newProgPerID)
        {
            DataTable TempPolicyDataTable = new DataTable();
            TempPolicyDataTable.Columns.Add("NewPolID", System.Type.GetType("System.Int32"));
            TempPolicyDataTable.Columns.Add("OldPolID", System.Type.GetType("System.Int32"));

            //-----Retro Info -----------------------------
            bool isRetroInfoCopied = false;
            RetroInfoBS retroInoBS = new RetroInfoBS();
            //retroBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<RetroInfoBE> retroInfoList = new List<RetroInfoBE>();
            retroInfoList = retroInoBS.getRetroInfoForCopy(oldProgPerID);
            foreach (RetroInfoBE retroInfoBE in retroInfoList)
            {
                retroInfoBE.ADJ_RETRO_INFO_ID = 0;
                retroInfoBE.PREM_ADJ_PGM_ID = newProgPerID;
                retroInfoBE.UPDT_USER_ID = null;
                retroInfoBE.UPDT_DT = null;
                retroInfoBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                retroInfoBE.CRTE_DT = DateTime.Now;
                isRetroInfoCopied = (retroInoBS.SaveRetroData(retroInfoBE)) ? true : false;
                if (isRetroInfoCopied == false) break;
            }
            //----- Policy Information-----------------------
            bool isPolicyCopied = false;
            PolicyBS policyBS = new PolicyBS();
            //policyBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<PolicyBE> policyList = new List<PolicyBE>();

            SteppedFactorBS steppedFactorBS = new SteppedFactorBS();
            // steppedFactorBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<SteppedFactorBE> StepFactorList = new List<SteppedFactorBE>();
            //if (this.ddlProgramType.SelectedItem.Text == "NON-DEP")
            //   policyList = policyBS.getPolicyData(oldProgPerID);  //This will retive only Main Policies. i.e ParentPolicyID=null
            //else if (this.ddlProgramType.SelectedItem.Text == "DEP")
            policyList = policyBS.getAllPolicies(oldProgPerID); //This will retrive all policies along with with underlaying policies. 

            foreach (PolicyBE policyBE in policyList)
            {
                DataRow TempPolicyDataRow = TempPolicyDataTable.NewRow();
                int OldPolicyID = policyBE.PolicyID;
                TempPolicyDataRow["OldPolID"] = OldPolicyID; //old Policy ID
                policyBE.PolicyID = 0;
                policyBE.ProgramPeriodID = newProgPerID;
                if (policyBE.ParentPolicyID != null)
                    policyBE.ParentPolicyID = Convert.ToInt32(TempPolicyDataTable.Select("OldPolID=" + policyBE.ParentPolicyID)[0]["NewPolID"]);
                policyBE.PolicyEffectiveDate = (Convert.ToDateTime(policyBE.PolicyEffectiveDate).AddYears(1));
                policyBE.PlanEndDate = (Convert.ToDateTime(policyBE.PlanEndDate).AddYears(1));
                if (policyBE.PolicyModulus != "99")
                    policyBE.PolicyModulus = (Convert.ToInt32(policyBE.PolicyModulus) + 1).ToString("00");
                policyBE.UPDATE_DATE = null;
                policyBE.UPDATE_USER_ID = null;
                policyBE.CREATE_DATE = DateTime.Now;
                policyBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                isPolicyCopied = ((policyBS.Update(policyBE)) ? true : false);
                if (isPolicyCopied == false) break;
                TempPolicyDataRow["NewPolID"] = policyBE.PolicyID; ; //New Policy ID
                TempPolicyDataTable.Rows.Add(TempPolicyDataRow); // Adding to TempPolicyDataTable

                StepFactorList = steppedFactorBS.GetSteppedFactorData(OldPolicyID);
                foreach (SteppedFactorBE stepFactorBE in StepFactorList)
                {
                    stepFactorBE.STEPPED_FACTOR_ID = 0;
                    stepFactorBE.POLICY_ID = policyBE.PolicyID;
                    stepFactorBE.PREM_ADJ_PGM_ID = newProgPerID;
                    stepFactorBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                    stepFactorBE.UPDATE_DATE = null;
                    stepFactorBE.UPDATE_USER_ID = null;
                    stepFactorBE.CREATE_DATE = DateTime.Now;
                    stepFactorBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                    isPolicyCopied = ((steppedFactorBS.Update(stepFactorBE)) ? true : false);
                    if (isPolicyCopied == false) break;
                }
            }

            //-----Audit Information----------------------------
            bool isAuditInfoCopied = false;
            Coml_Agmt_AuDtBS comAgmtAudtBS = new Coml_Agmt_AuDtBS();
            // comAgmtAudtBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<Coml_Agmt_AuDtBE> comAgmtAudtList = new List<Coml_Agmt_AuDtBE>();
            comAgmtAudtList = comAgmtAudtBS.getCommAgrAuditList(oldProgPerID);
            foreach (Coml_Agmt_AuDtBE comAgmtAudtBE in comAgmtAudtList)
            {
                int oldComAgmtAudtID = comAgmtAudtBE.Comm_Agr_Audit_ID;
                int OldPolicyID = comAgmtAudtBE.Comm_Agr_ID;
                comAgmtAudtBE.Comm_Agr_Audit_ID = 0;
                comAgmtAudtBE.Prem_Adj_Prg_ID = newProgPerID;
                comAgmtAudtBE.Comm_Agr_ID = Convert.ToInt32(TempPolicyDataTable.Select("OldPolID=" + comAgmtAudtBE.Comm_Agr_ID)[0]["NewPolID"]);
                comAgmtAudtBE.Customer_ID = AISMasterEntities.AccountNumber;
                comAgmtAudtBE.AdjustmentIndicator = false;
                comAgmtAudtBE.Aud_Rev_Status = false;
                comAgmtAudtBE.UpdatedDate = null;
                comAgmtAudtBE.UpdatedUser_ID = null;
                comAgmtAudtBE.CreatedDate = DateTime.Now;
                comAgmtAudtBE.CreatedUser_ID = CurrentAISUser.PersonID;
                isAuditInfoCopied = ((comAgmtAudtBS.Update(comAgmtAudtBE)) ? true : false);
                if (isAuditInfoCopied == false) break;

                Sub_Audt_PremBS subAudtPremBS = new Sub_Audt_PremBS();
                //subAudtPremBS.AppTransactionWrapper = ProPrgTransWrapper;
                IList<SubjectAuditPremiumBE> subAudtPremList = new List<SubjectAuditPremiumBE>();
                subAudtPremList = subAudtPremBS.getsubPremAudList(oldComAgmtAudtID);
                foreach (SubjectAuditPremiumBE subAudtPremBE in subAudtPremList)
                {
                    subAudtPremBE.Sub_Prem_Aud_ID = 0;
                    subAudtPremBE.Coml_Agmt_Audt_ID = comAgmtAudtBE.Comm_Agr_Audit_ID;
                    subAudtPremBE.Coml_Agmt_ID = comAgmtAudtBE.Comm_Agr_ID;
                    subAudtPremBE.Prem_Adj_Pgm_ID = newProgPerID;
                    subAudtPremBE.Custmr_ID = AISMasterEntities.AccountNumber;
                    subAudtPremBE.UpdatedDate = null;
                    subAudtPremBE.UpdatedUser_ID = null;
                    subAudtPremBE.CreatedDate = DateTime.Now;
                    subAudtPremBE.CreatedUser_ID = CurrentAISUser.PersonID;
                    isAuditInfoCopied = ((subAudtPremBS.Update(subAudtPremBE)) ? true : false);
                    if (isAuditInfoCopied == false) break;
                }
                if (isAuditInfoCopied == false) break;
                Non_Sub_Audt_PremBS nonSubAudtPremBS = new Non_Sub_Audt_PremBS();
                //nonSubAudtPremBS.AppTransactionWrapper = ProPrgTransWrapper;
                IList<NonSubjectAuditPremiumBE> nonSubAudtPremList = new List<NonSubjectAuditPremiumBE>();
                nonSubAudtPremList = nonSubAudtPremBS.getNonsubPremAudList(oldComAgmtAudtID);
                foreach (NonSubjectAuditPremiumBE nonSubAudtPremBE in nonSubAudtPremList)
                {
                    nonSubAudtPremBE.N_Sub_Prem_Aud_ID = 0;
                    nonSubAudtPremBE.Coml_Agmt_Audt_ID = comAgmtAudtBE.Comm_Agr_Audit_ID;
                    nonSubAudtPremBE.Coml_Agmt_ID = comAgmtAudtBE.Comm_Agr_ID;
                    nonSubAudtPremBE.Prem_Adj_Pgm_ID = newProgPerID;
                    nonSubAudtPremBE.Custmr_ID = AISMasterEntities.AccountNumber;
                    nonSubAudtPremBE.UpdatedDate = null;
                    nonSubAudtPremBE.UpdatedUser_ID = null;
                    nonSubAudtPremBE.CreatedDate = DateTime.Now;
                    nonSubAudtPremBE.CreatedUser_ID = CurrentAISUser.PersonID;
                    isAuditInfoCopied = ((nonSubAudtPremBS.Update(nonSubAudtPremBE)) ? true : false);
                    if (isAuditInfoCopied == false) break;
                }
            }

            //--------LBA,LCF,TM,CHF,RML,ILRF,ESCROW----------------
            bool isParametersCopied = false;
            Adj_Parameter_SetupBS adjParamSetupBS = new Adj_Parameter_SetupBS();
            // adjParamSetupBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<AdjustmentParameterSetupBE> adjParamSetupList = new List<AdjustmentParameterSetupBE>();

            Adj_Paramet_PolBS adjParamPolBS = new Adj_Paramet_PolBS();
            // adjParamPolBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<AdjustmentParameterPolicyBE> adjParamPolList = new List<AdjustmentParameterPolicyBE>();

            Adj_paramet_DtlBS adjParamDetailBS = new Adj_paramet_DtlBS();
            // adjParamDtlBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<AdjustmentParameterDetailBE> adjParamDetaillList = new List<AdjustmentParameterDetailBE>();

            ILRFFormulaBS iLRFFormulaBS = new ILRFFormulaBS();
            // iLRFFormulaBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<ILRFFormulaBE> ILRFFormulaList = new List<ILRFFormulaBE>();

            adjParamSetupList = adjParamSetupBS.getAdjParamtr(oldProgPerID, AISMasterEntities.AccountNumber);
            foreach (AdjustmentParameterSetupBE adjParamSetupBE in adjParamSetupList)
            {
                int oldAdjparamSetupID = adjParamSetupBE.adj_paramet_setup_id;
                int oldIBNRLDFID = Convert.ToInt32((adjParamSetupBE.incur_but_not_rptd_los_dev_fctr_id != null) ? adjParamSetupBE.incur_but_not_rptd_los_dev_fctr_id : -1);
                adjParamSetupBE.adj_paramet_setup_id = 0;
                adjParamSetupBE.prem_adj_pgm_id = newProgPerID;
                adjParamSetupBE.UPDATE_DATE = null;
                adjParamSetupBE.UPDATE_USER_ID = null;
                adjParamSetupBE.CREATE_DATE = DateTime.Now;
                adjParamSetupBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                isParametersCopied = ((adjParamSetupBS.Update(adjParamSetupBE)) ? true : false);
                if (isParametersCopied == false) break;

                adjParamPolList = adjParamSetupBE.AdjParametPolBEs;
                foreach (AdjustmentParameterPolicyBE adjParamPolBE in adjParamPolList)
                {
                    adjParamPolBE.adj_paramet_pol_id = 0;
                    adjParamPolBE.adj_paramet_setup_id = adjParamSetupBE.adj_paramet_setup_id;
                    adjParamPolBE.PrmadjPRgmID = newProgPerID;
                    adjParamPolBE.coml_agmt_id = Convert.ToInt32(TempPolicyDataTable.Select("OldPolID=" + adjParamPolBE.coml_agmt_id)[0]["NewPolID"]);
                    adjParamPolBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                    adjParamPolBE.CREATE_DATE = DateTime.Now;
                    isParametersCopied = ((adjParamPolBS.Update(adjParamPolBE)) ? true : false);
                    if (isParametersCopied == false) break;
                }
                if (isParametersCopied == false) break;
                adjParamDetaillList = adjParamDetailBS.getLBAAdjParamtrDtls(oldProgPerID, oldAdjparamSetupID, AISMasterEntities.AccountNumber);
                foreach (AdjustmentParameterDetailBE adjParamDetailBE in adjParamDetaillList)
                {
                    adjParamDetailBE.prem_adj_pgm_dtl_id = 0;
                    adjParamDetailBE.PrgmPerodID = newProgPerID;
                    adjParamDetailBE.adj_paramet_id = adjParamSetupBE.adj_paramet_setup_id;
                    adjParamDetailBE.AccountID = AISMasterEntities.AccountNumber;
                    adjParamDetailBE.UPDTE_DATE = null;
                    adjParamDetailBE.UPDTE_USER_ID = null;
                    adjParamDetailBE.CRTE_DATE = DateTime.Now;
                    adjParamDetailBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                    isParametersCopied = ((adjParamDetailBS.Update(adjParamDetailBE)) ? true : false);
                    if (isParametersCopied == false) break;
                }

                if (oldIBNRLDFID > -1)
                {
                    string IBNRLDF = (new LookupBS()).getLkupRow(oldIBNRLDFID).LookUpName;
                    ILRFFormulaList = iLRFFormulaBS.getILRFFormulasForProgPerCopy(oldProgPerID, AISMasterEntities.AccountNumber, IBNRLDF);
                    foreach (ILRFFormulaBE iLRFFormulaBE in ILRFFormulaList)
                    {
                        iLRFFormulaBE.INCURRED_LOSS_REIM_FUND_FRMLA_ID = 0;
                        iLRFFormulaBE.PROGRAMPERIOD_ID = newProgPerID;
                        iLRFFormulaBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
                        iLRFFormulaBE.UPDATE_DATE = null;
                        iLRFFormulaBE.UPDATE_USER_ID = null;
                        iLRFFormulaBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        iLRFFormulaBE.CREATE_DATE = DateTime.Now;
                        isParametersCopied = ((iLRFFormulaBS.Update(iLRFFormulaBE)) ? true : false);
                        if (isParametersCopied == false) break;
                    }
                }

            }
            //-------------Assign ERP Formula----------------------
            bool isAssignERPCopied = false;
            ProgramPeriodBE oldprogPerBE = new ProgramPeriodBE();
            oldprogPerBE = ProgramPeriodsBizService.getProgramPeriodInfo(oldProgPerID);

            progPerBE.MSTR_ERND_RETRO_PREM_FRMLA_ID = oldprogPerBE.MSTR_ERND_RETRO_PREM_FRMLA_ID;
            isAssignERPCopied = ((ProgramPeriodsBizService.Update(progPerBE)) ? true : false);

            //--------------Combined Elements---------------------
            bool isCominCopied = false;
            CombinedElementsBS combElemtBS = new CombinedElementsBS();
            //combElemtBS.AppTransactionWrapper = ProPrgTransWrapper;
            IList<CombinedElementsBE> combElemtList = new List<CombinedElementsBE>();
            combElemtList = combElemtBS.GetCombinedElements(oldProgPerID);
            foreach (CombinedElementsBE combElemtBE in combElemtList)
            {
                combElemtBE.COMB_ELEMTS_SETUP_ID = 0;
                combElemtBE.PREM_ADJ_PGM_ID = newProgPerID;
                combElemtBE.COML_AGMT_ID = Convert.ToInt32(TempPolicyDataTable.Select("OldPolID=" + combElemtBE.COML_AGMT_ID)[0]["NewPolID"]);
                combElemtBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                combElemtBE.UPDT_DATE = null;
                combElemtBE.UPDT_USER_ID = null;
                combElemtBE.CRTE_DT = DateTime.Now;
                combElemtBE.CRTE_USR_ID = CurrentAISUser.PersonID;
                isCominCopied = ((combElemtBS.Update(combElemtBE)) ? true : false);
                if (isCominCopied == false) break;
            }

            if (isCominCopied == true)
            {
                //ProgramPeriodBE oldprogPerBE = new ProgramPeriodBE();
                //oldprogPerBE = ProgramPeriodsBizService.getProgramPeriodInfo(oldProgPerID);

                progPerBE.AGMT_ALOC_LOS_ADJ_EXPS_IND = oldprogPerBE.AGMT_ALOC_LOS_ADJ_EXPS_IND;
                progPerBE.AGMT_LOS_BASE_ASSES_IND = oldprogPerBE.AGMT_LOS_BASE_ASSES_IND;
                progPerBE.AGMT_PAID_INCUR_IND = oldprogPerBE.AGMT_PAID_INCUR_IND;
                progPerBE.AGMT_UNALOCTD_LOS_ADJ_IND = oldprogPerBE.AGMT_UNALOCTD_LOS_ADJ_IND;
                progPerBE.LSI_ALOC_LOS_ADJ_EXPS_IND = oldprogPerBE.LSI_ALOC_LOS_ADJ_EXPS_IND;
                progPerBE.LSI_UNALOCTD_LOS_ADJ_IND = oldprogPerBE.LSI_UNALOCTD_LOS_ADJ_IND;
                progPerBE.LSI_LOS_BASE_ASSES_IND = oldprogPerBE.LSI_LOS_BASE_ASSES_IND;
                isCominCopied = ((ProgramPeriodsBizService.Update(progPerBE)) ? true : false);
            }
            if ((isRetroInfoCopied) && (isPolicyCopied) && (isAuditInfoCopied) && (isParametersCopied) && isAssignERPCopied && (isCominCopied))
                return true;
            else
                return false;

        }



        private void EnableDisableStatusChechkBoxes()
        {
            bool flag = false;
            //Part of role based security
            if ((CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.Manager) || (CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.SystemAdmin))
            {
                flag = true;
            }
            chkSubmitForSetupQC.Enabled = flag;

            chkSetupQCComplete.Enabled = flag;

            chkActive.Enabled = flag;

        }

        /// <summary>
        /// This is to check any data got changed when user clicks on Save button for updation
        /// If changed then only an entry will go to Audit Trial
        /// </summary>
        /// <param name="progPerBE"></param>
        /// <returns></returns>
        private bool IsDataChanged(ProgramPeriodBE progPerBE)
        {
            bool isDataChanged = false;

            isDataChanged = (progPerBE.STRT_DT != DateTime.Parse(txtEffectiveDate.Text)) ? true : false;

            isDataChanged = (progPerBE.PLAN_END_DT != DateTime.Parse(txtExpirationDate.Text)) ? true : isDataChanged;

            if (txtLSIEndDate.Text != "")
                isDataChanged = (progPerBE.LOS_SENS_INFO_END_DT != DateTime.Parse(txtLSIEndDate.Text)) ? true : isDataChanged;
            else if (txtLSIEndDate.Text == "")
                isDataChanged = (progPerBE.LOS_SENS_INFO_END_DT != null) ? true : isDataChanged;

            if (txtLSIStartDate.Text != "")
                isDataChanged = (progPerBE.LOS_SENS_INFO_STRT_DT != DateTime.Parse(txtLSIStartDate.Text)) ? true : isDataChanged;
            else if (txtLSIStartDate.Text == "")
                isDataChanged = (progPerBE.LOS_SENS_INFO_STRT_DT != null) ? true : isDataChanged;

            isDataChanged = (progPerBE.BRKR_ID != Convert.ToInt32(ddlBrokerDetail.SelectedValue)) ? true : isDataChanged;

            isDataChanged = (progPerBE.BRKR_CONCTC_ID != Convert.ToInt32(ddlBrokerContact.SelectedValue)) ? true : isDataChanged;

            isDataChanged = (progPerBE.PGM_TYP_ID != Convert.ToInt32(ddlProgramType.SelectedValue)) ? true : isDataChanged;

            isDataChanged = (progPerBE.BSN_UNT_OFC_ID != Convert.ToInt32(ddlBUDetails.SelectedValue)) ? true : isDataChanged;

            isDataChanged = (progPerBE.PAID_INCUR_TYP_ID != Convert.ToInt32(ddlPaidIncurred.SelectedValue)) ? true : isDataChanged;

            if (txtConvertsAT.Text != "")
                isDataChanged = (progPerBE.INCUR_CONV_MMS_CNT != Convert.ToInt32(txtConvertsAT.Text)) ? true : isDataChanged;
            else if (txtConvertsAT.Text == "")
                isDataChanged = (progPerBE.INCUR_CONV_MMS_CNT != null) ? true : isDataChanged;

            isDataChanged = (progPerBE.FST_ADJ_MMS_FROM_INCP_CNT != Convert.ToInt32(txtFirstAdj.Text)) ? true : isDataChanged;

            if (txtHidenValDt.Text != "")
                isDataChanged = (progPerBE.VALN_MM_DT != DateTime.Parse(txtHidenValDt.Text)) ? true : isDataChanged;
            else if (txtHidenValDt.Text == "")
                isDataChanged = (progPerBE.VALN_MM_DT != null) ? true : isDataChanged;

            if (txtNextValDt.Text != "")
                isDataChanged = (progPerBE.NXT_VALN_DT != DateTime.Parse(txtNextValDt.Text)) ? true : isDataChanged;
            else if (txtNextValDt.Text == "")
                isDataChanged = (progPerBE.NXT_VALN_DT != null) ? true : isDataChanged;

            if (txtPrevValDt.Text != "")
                isDataChanged = (progPerBE.PREV_VALN_DT != DateTime.Parse(txtPrevValDt.Text)) ? true : isDataChanged;
            else if (txtPrevValDt.Text == "")
                isDataChanged = (progPerBE.PREV_VALN_DT != null) ? true : isDataChanged;

            isDataChanged = (progPerBE.ADJ_FREQ_MMS_INTVRL_CNT != Convert.ToInt32(txtFreq.Text)) ? true : isDataChanged;

            if (Convert.ToInt32(ddlBKTCYBUYOUT.SelectedValue) != 0)
                isDataChanged = (progPerBE.BNKRPT_BUYOUT_ID != Convert.ToInt32(ddlBKTCYBUYOUT.SelectedValue)) ? true : isDataChanged;
            else if (Convert.ToInt32(ddlBKTCYBUYOUT.SelectedValue) != 0)
                isDataChanged = (progPerBE.BNKRPT_BUYOUT_ID != null) ? true : isDataChanged;

            if (txtBKTCYBUYOUTEffDate.Text != "")
                isDataChanged = (progPerBE.BNKRPT_BUYOUT_EFF_DT != DateTime.Parse(txtBKTCYBUYOUTEffDate.Text)) ? true : isDataChanged;
            else if (txtBKTCYBUYOUTEffDate.Text == "")
                isDataChanged = (progPerBE.BNKRPT_BUYOUT_EFF_DT != null) ? true : isDataChanged;

            if (txtFinalAdj.Text != "")
                isDataChanged = (progPerBE.FNL_ADJ_DT != DateTime.Parse(txtFinalAdj.Text)) ? true : isDataChanged;
            else if (txtFinalAdj.Text == "")
                isDataChanged = (progPerBE.FNL_ADJ_DT != null) ? true : isDataChanged;

            isDataChanged = (progPerBE.INCLD_CAPTV_PAYKND_IND != chkInclCptvPKCodes.Checked) ? true : isDataChanged;

            isDataChanged = (progPerBE.AVG_TAX_MULTI_IND != chkAvgTM.Checked) ? true : isDataChanged;

            if (txtCombEleMax.Text != "")
                isDataChanged = (progPerBE.COMB_ELEMTS_MAX_AMT != Convert.ToDecimal(txtCombEleMax.Text)) ? true : isDataChanged;
            else if (txtCombEleMax.Text == "")
                isDataChanged = (progPerBE.COMB_ELEMTS_MAX_AMT != null) ? true : isDataChanged;

            isDataChanged = (progPerBE.ZNA_SERV_COMP_CLM_HNDL_FEE_IND != chkCHFZSC.Checked) ? true : isDataChanged;

            if (txtPEOPayIn.Text != "")
                isDataChanged = (progPerBE.PEO_PAY_IN_AMT != Convert.ToDecimal(txtPEOPayIn.Text)) ? true : isDataChanged;
            else if (txtPEOPayIn.Text == "")
                isDataChanged = (progPerBE.PEO_PAY_IN_AMT != null) ? true : isDataChanged;

            if (txtFirstAdjNONPREM.Text != "")
                isDataChanged = (progPerBE.FST_ADJ_NON_PREM_MMS_CNT != Convert.ToInt32(txtFirstAdjNONPREM.Text)) ? true : isDataChanged;
            else if (txtFirstAdjNONPREM.Text != "")
                isDataChanged = (progPerBE.FST_ADJ_NON_PREM_MMS_CNT != null) ? true : isDataChanged;

            if (txtFreqNonPrem.Text != "")
                isDataChanged = (progPerBE.FREQ_NON_PREM_MMS_CNT != Convert.ToInt32(txtFreqNonPrem.Text)) ? true : isDataChanged;
            else if (txtFreqNonPrem.Text != "")
                isDataChanged = (progPerBE.FREQ_NON_PREM_MMS_CNT != null) ? true : isDataChanged;

            if (txtFinalAdjNONPREM.Text != "")
                isDataChanged = (progPerBE.FNL_ADJ_NON_PREM_DT != DateTime.Parse(txtFinalAdjNONPREM.Text)) ? true : isDataChanged;
            else if (txtFinalAdjNONPREM.Text == "")
                isDataChanged = (progPerBE.FNL_ADJ_NON_PREM_DT != null) ? true : isDataChanged;

            if (txtNextValDtNP.Text != "")
                isDataChanged = (progPerBE.NXT_VALN_DT_NON_PREM_DT != DateTime.Parse(txtNextValDtNP.Text)) ? true : isDataChanged;
            else if (txtNextValDtNP.Text != "")
                isDataChanged = (progPerBE.NXT_VALN_DT_NON_PREM_DT != null) ? true : isDataChanged;

            if (txtPrevValDtNP.Text != "")
                isDataChanged = (progPerBE.PREV_VALN_DT_NON_PREM_DT != DateTime.Parse(txtPrevValDtNP.Text)) ? true : isDataChanged;
            else if (txtPrevValDtNP.Text != "")
                isDataChanged = (progPerBE.PREV_VALN_DT_NON_PREM_DT != null) ? true : isDataChanged;

            if (txtTMFactor.Text != "")
                isDataChanged = (progPerBE.TAX_MULTI_FCTR_RT != Convert.ToDecimal(txtTMFactor.Text)) ? true : isDataChanged;
            else if (txtTMFactor.Text == "")
                isDataChanged = (progPerBE.TAX_MULTI_FCTR_RT != null) ? true : isDataChanged;

            if (txtFirstAdjDt.Text != "")
                isDataChanged = (progPerBE.FST_ADJ_DT != DateTime.Parse(txtFirstAdjDt.Text)) ? true : isDataChanged;
            else if (txtFirstAdjDt.Text == "")
                isDataChanged = (progPerBE.FST_ADJ_DT != null) ? true : isDataChanged;

            if (txtLSIRetriveFromDt.Text != "")
                isDataChanged = (progPerBE.LSI_RETRIEVE_FROM_DT != DateTime.Parse(txtLSIRetriveFromDt.Text)) ? true : isDataChanged;
            else if (txtFirstAdjDt.Text == "")
                isDataChanged = (progPerBE.LSI_RETRIEVE_FROM_DT != null) ? true : isDataChanged;

            //Checking for ProgramPeriodStatus changes
            programStatusList = ProgramStatusBizService.GetProgramStatusList(AISMasterEntities.AccountNumber, progPerBE.PREM_ADJ_PGM_ID);
            BLAccess BLAcc = new BLAccess();

            int initialLookUpID = BLAcc.GetLookUpID("INITIAL");
            int submitLookUPID = BLAcc.GetLookUpID("SUBMIT - SETUP QC");
            int setupLookUpID = BLAcc.GetLookUpID("SETUP QC - COMPLETE");
            int activeLookUpID = BLAcc.GetLookUpID("ACTIVE");

            foreach (PremiumAdjustmentProgramStatusBE sts in programStatusList)
            {
                if (sts.PGM_STATUS_ID == initialLookUpID)
                    isDataChanged = (sts.STS_CHK_IND != chkInitial.Checked) ? true : isDataChanged;

                else if (sts.PGM_STATUS_ID == submitLookUPID)
                    isDataChanged = (sts.STS_CHK_IND != chkSubmitForSetupQC.Checked) ? true : isDataChanged;

                else if (sts.PGM_STATUS_ID == setupLookUpID)
                    isDataChanged = (sts.STS_CHK_IND != chkSetupQCComplete.Checked) ? true : isDataChanged;

                else if (sts.PGM_STATUS_ID == activeLookUpID)
                    isDataChanged = (sts.STS_CHK_IND != chkActive.Checked) ? true : isDataChanged;
            }
            isDataChanged = ((chkSubmitForSetupQC.Checked) && (txtSubmitStsID.Text == "")) ? true : isDataChanged;
            isDataChanged = ((chkSetupQCComplete.Checked) && (txtSetupStsID.Text == "")) ? true : isDataChanged;
            isDataChanged = ((chkActive.Checked) && (txtActiveStsID.Text == "")) ? true : isDataChanged;

            return isDataChanged;
        }

        /// <summary>
        /// Invokes when user clicks on Copy button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //User wants to copy the existing record and make a new record.
            //making the CheckNew boolean to true
            //if (progPerBE.PGM_TYP_ID != Convert.ToInt32(ddlProgramType.SelectedValue))
            //{
            //    if (ddlProgramType.SelectedItem.Text == "DEP")
            //        ShowMessage("Sorry, Copy Program Period can not proceed. Program Type has been changed from 'NONDEP' to 'DEP'");
            //    else if (ddlProgramType.SelectedItem.Text == "NONDEP")
            //        ShowMessage("Sorry, Copy Program Period can not  proceed. Program Type has been changed from 'DEP' to 'NONDEP'");
            //}
            //else
            //{
            isNew = true;
            isCopy = true;
            //The system will increment a copied Program Period and Mod by one.
            txtEffectiveDate.Text = Convert.ToDateTime(txtEffectiveDate.Text).AddYears(1).ToShortDateString();
            txtEffDtHidden.Text = Convert.ToDateTime(txtEffectiveDate.Text).AddYears(1).ToShortDateString();
            txtExpirationDate.Text = Convert.ToDateTime(txtExpirationDate.Text).AddYears(1).ToShortDateString();
            txtLSIStartDate.Text = Convert.ToDateTime(txtLSIStartDate.Text).AddYears(1).ToShortDateString();
            txtLSIEndDate.Text = Convert.ToDateTime(txtLSIEndDate.Text).AddYears(1).ToShortDateString();
            txtLSIRetriveFromDt.Text = Convert.ToDateTime(txtEffectiveDate.Text).ToShortDateString();
            
            if (txtFirstAdj.Text != "")
                {
                    //If the program period effective date + FirstAdj (months) is  from the 16th -31st will produce a 
                    //valuation date of the calculated current month end.  Effective dates of 1st – 15th will produce a 
                    //valuation date for the prior month end.
                    DateTime myDate = DateTime.Parse(txtEffectiveDate.Text).AddMonths(Convert.ToInt32(txtFirstAdj.Text));
                    if (myDate.Day > 15)
                    {
                        myDate = myDate.AddMonths(1);  //Add a month to get the Last date of the month
                        // remove all of the days in the next month to get bumped down to the last day of the previous month
                        myDate = myDate.AddDays(-(myDate.Day));
                    }
                    else if (myDate.Day < 16)
                    {
                        //newdt = newdt.AddMonths(1);    //Add a month to get the Last date of the month
                        //newdt = newdt.AddMonths(-1);   //if the Date is Less then 16th previous months lastdate                 
                        myDate = myDate.AddDays(-(myDate.Day));
                    }
                    txtValDt.Text = myDate.ToString("MM/dd"); ;
                    txtHidenValDt.Text = myDate.ToShortDateString();
                    txtFirstAdjDt.Text = myDate.ToShortDateString();
                    txtNextValDt.Text = myDate.ToShortDateString();
                }
            //If the program period effective date + FirstAdjNP (months) is  from the 16th -31st will produce a 
            //valuation date of the calculated current month end.  Effective dates of 1st – 15th will produce a 
            //valuation date for the prior month end.
            if (txtFirstAdjNONPREM.Text != "")
                {
                    DateTime myDate = DateTime.Parse(txtEffectiveDate.Text).AddMonths(Convert.ToInt32(txtFirstAdjNONPREM.Text));
                    if (myDate.Day > 15)
                    {
                        myDate = myDate.AddMonths(1);  //Add a month to get the Last date of the month
                        // remove all of the days in the next month to get bumped down to the last day of the previous month
                        myDate = myDate.AddDays(-(myDate.Day));
                    }
                    else if (myDate.Day < 16)
                    {
                        //newdt = newdt.AddMonths(1);    //Add a month to get the Last date of the month
                        //newdt = newdt.AddMonths(-1);   //if the Date is Less then 16th previous months lastdate                 
                        myDate = myDate.AddDays(-(myDate.Day));
                    }
                   
                    txtFirstAdjDtNP.Text = myDate.ToShortDateString();
                    txtNextValDtNP.Text = myDate.ToShortDateString();
                }            
            
            txtPrevValDt.Text = "";
            txtPrevValDtNP.Text = "";

            txtFirstAdj.Enabled = true;
            txtFirstAdjNONPREM.Enabled = true;

            ClearStatusControls();

            btnSave.Text = "Save";
            btnCopy.Enabled = false;
            //}

        }

        /// <summary>
        /// Even when user clicks on QC Link on Program period List view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GotoQC(object sender, CommandEventArgs e)
        {
            int ProgramPeriodID = Convert.ToInt32(e.CommandArgument);
            AISMasterEntities.PremiumAdjProgramID = ProgramPeriodID.ToString();
            ProgramPeriodsBS proPreBS = new ProgramPeriodsBS();
            ProgramPeriodBE proPreBE = new ProgramPeriodBE();
            proPreBE = proPreBS.getProgramPeriodInfo(ProgramPeriodID);
            AISMasterEntities.PremAdjProgramStartDate = Convert.ToDateTime(proPreBE.STRT_DT);
            AISMasterEntities.PremAdjProgramEndState = Convert.ToDateTime(proPreBE.PLAN_END_DT);
            AISMasterEntities.PremiumAdjProgramID = ProgramPeriodID.ToString();

            Response.Redirect("~/AcctMgmt/AcctSetupProcChklst.aspx");
        }
        /// <summary>
        /// Clears all controls when users wants to create a brand new record
        /// </summary>
        public void ClearFileds()
        {
            txtEffectiveDate.Text = "";
            txtExpirationDate.Text = "";
            txtLSIStartDate.Text = "";
            txtLSIEndDate.Text = "";
            txtConvertsAT.Text = "";
            txtFirstAdj.Text = "";
            txtValDt.Text = "";
            txtHidenValDt.Text = "";
            txtNextValDt.Text = "";
            txtPrevValDt.Text = "";
            txtFreq.Text = "";
            txtFinalAdj.Text = "";
            chkInclCptvPKCodes.Checked = false;
            //chkAvgTM.Checked = false;
            txtCombEleMax.Text = "";
            chkCHFZSC.Checked = false;
            txtPEOPayIn.Text = "";
            txtFirstAdjNONPREM.Text = "";
            txtFreqNonPrem.Text = "";
            txtFinalAdjNONPREM.Text = "";
            txtNextValDtNP.Text = "";
            txtPrevValDtNP.Text = "";
            txtFirstAdjDtNP.Text = "";
            txtBKTCYBUYOUTEffDate.Text = "";
            txtTMFactor.Text = "";
            //txtTMFactor.Enabled = false;
            txtFirstAdjDt.Text = "";
            txtLSIRetriveFromDt.Text = "";

            txtBKTCYBUYOUTEffDate.Enabled = false;
            imgBKTCYBUYOUTEffDate.Enabled = false;

            txtFirstAdj.Enabled = true;
            txtFirstAdjNONPREM.Enabled = true;

            txtFreq.Enabled = true;
            txtFreqNonPrem.Enabled = true;

            txtFinalAdj.Enabled = true;
            imgFinalAdj.Enabled = true;

            txtFinalAdjNONPREM.Enabled = true;
            imgFinalAdjNONPREM.Enabled = true;
            txtConvertsAT.Enabled = true;

            ddlProgramType.DataBind();
            ddlProgramType.Items.FindByText("NON-DEP").Selected = true;

            ddlBUDetails.DataBind();
            ddlBUDetails.Items.FindByValue("0").Selected = true;

            ddlPaidIncurred.DataBind();
            ddlPaidIncurred.Items.FindByValue("0").Selected = true;

            ddlBKTCYBUYOUT.DataBind();
            ddlBKTCYBUYOUT.Items.FindByValue("0").Selected = true;

            ddlBrokerDetail.DataBind();
            ddlBrokerDetail.Items.FindByValue("0").Selected = true;

            ddlBrokerContact.Items.Clear();
            ListItem LI = new ListItem("(Select)", "0");
            ddlBrokerContact.Items.Add(LI);
            ListItem LI1 = new ListItem("Not Applicable", "1000000");
            ddlBrokerContact.Items.Add(LI1);

            pnlDetails.Enabled = true;

            ClearStatusControls();
        }

        private void ClearStatusControls()
        {
            txtInitialStsID.Text = "";
            chkInitial.Checked = true;
            lblInitialPeriod.Text = "";
            txtInitialOldSts.Text = "";

            txtSubmitStsID.Text = "";
            chkSubmitForSetupQC.Checked = false;
            lblSubmitQCPeriod.Text = "";
            chkSubmitForSetupQC.Enabled = true;
            txtSubmitOldSts.Text = "";

            txtSetupStsID.Text = "";
            chkSetupQCComplete.Checked = false;
            lblSetupQCPeriod.Text = "";
            chkSetupQCComplete.Enabled = true;
            txtSetupOldSts.Text = "";

            txtActiveStsID.Text = "";
            chkActive.Checked = false;
            lblActivePeriod.Text = "";
            chkActive.Enabled = true;
            txtActiveOldsts.Text = "";
        }
        /// <summary>
        /// Fills all Selected Program periods details into below detail panel
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        public void DisplayDetails(int ProgramPeriodID)
        {
            try
            {
               
                //--------
                //Loading the BE using the FrameWork
                progPerBE = ProgramPeriodsBizService.getProgramPeriodInfo(ProgramPeriodID);

                //ProgramperiodsBE = PrPeBE();
                if (progPerBE.STRT_DT != null)
                {
                    txtEffectiveDate.Text = Convert.ToDateTime(progPerBE.STRT_DT).ToShortDateString();
                    txtEffDtHidden.Text = Convert.ToDateTime(progPerBE.STRT_DT).ToString("MM/dd/yyyy");
                }
                else
                {
                    txtEffectiveDate.Text = "";
                    txtEffDtHidden.Text = "";
                }

                if (progPerBE.PLAN_END_DT != null)
                    txtExpirationDate.Text = Convert.ToDateTime(progPerBE.PLAN_END_DT).ToShortDateString();
                else
                    txtExpirationDate.Text = null;

                if (progPerBE.LOS_SENS_INFO_STRT_DT != null)
                    txtLSIStartDate.Text = Convert.ToDateTime(progPerBE.LOS_SENS_INFO_STRT_DT).ToShortDateString();
                else
                    txtLSIStartDate.Text = "";

                if (progPerBE.LOS_SENS_INFO_END_DT != null)
                    txtLSIEndDate.Text = Convert.ToDateTime(progPerBE.LOS_SENS_INFO_END_DT).ToShortDateString();
                else
                    txtLSIEndDate.Text = "";
                ddlBrokerDetail.DataBind();
                if ((progPerBE.BRKR_ID != null) && (ddlBrokerDetail.Items.FindByValue(progPerBE.BRKR_ID.ToString()) != null))
                {
                    ddlBrokerDetail.Items.FindByValue(progPerBE.BRKR_ID.ToString()).Selected = true;
                }
                else
                {
                    ddlBrokerDetail.Items.FindByValue("0").Selected = true;
                }
                BindDDLBrokerContacts(int.Parse(ddlBrokerDetail.SelectedValue));
                ddlBrokerContact.DataBind();
                if ((progPerBE.BRKR_CONCTC_ID != null) && (ddlBrokerContact.Items.FindByValue(progPerBE.BRKR_CONCTC_ID.ToString()) != null))
                {
                    ddlBrokerContact.Items.FindByValue(progPerBE.BRKR_CONCTC_ID.ToString()).Selected = true;
                }
                else
                {
                    ddlBrokerContact.Items.FindByValue("0").Selected = true;
                }
                if (progPerBE.PGM_TYP_ID != null)
                {
                    ddlProgramType.DataBind();
//                    ddlProgramType.Items.FindByValue(progPerBE.PGM_TYP_ID.ToString()).Selected = true;
                    AddInActiveLookupData(ref ddlProgramType, progPerBE.PGM_TYP_ID.Value);
                }
                else
                {
                    ddlProgramType.DataBind();
                    ddlProgramType.Items.FindByValue("0").Selected = true;
                }
                ddlBUDetails.DataBind();
                if ((progPerBE.BSN_UNT_OFC_ID != null) && (ddlBUDetails.Items.FindByValue(progPerBE.BSN_UNT_OFC_ID.ToString()) != null))
                {
//                    ddlBUDetails.Items.FindByValue(progPerBE.BSN_UNT_OFC_ID.ToString()).Selected = true;
                    AddInActiveLookupData(ref ddlBUDetails, progPerBE.BSN_UNT_OFC_ID.Value);
                }
                else
                {
                    ddlBUDetails.Items.FindByValue("0").Selected = true;
                }
                if (progPerBE.PAID_INCUR_TYP_ID != null)
                {
                    ddlPaidIncurred.DataBind();
//                    ddlPaidIncurred.Items.FindByValue(progPerBE.PAID_INCUR_TYP_ID.ToString()).Selected = true;
                    AddInActiveLookupData(ref ddlPaidIncurred, progPerBE.PAID_INCUR_TYP_ID.Value);
                }
                else
                {
                    ddlPaidIncurred.DataBind();
                    ddlPaidIncurred.Items.FindByValue("0").Selected = true;
                }
                if (progPerBE.INCUR_CONV_MMS_CNT != null)
                    txtConvertsAT.Text = progPerBE.INCUR_CONV_MMS_CNT.ToString();
                else
                    txtConvertsAT.Text = "";

                if (progPerBE.FST_ADJ_MMS_FROM_INCP_CNT != null)
                {
                    txtFirstAdj.Text = progPerBE.FST_ADJ_MMS_FROM_INCP_CNT.ToString();
                    txtFstAdjHidden.Text = progPerBE.FST_ADJ_MMS_FROM_INCP_CNT.ToString();
                }
                else
                {
                    txtFirstAdj.Text = "";
                    txtFstAdjHidden.Text = "";
                }

                if (progPerBE.VALN_MM_DT != null)
                {
                    DateTime MyDate = DateTime.Parse(progPerBE.VALN_MM_DT.ToString());
                    //txtValDt.Text = MyDate.Month.ToString() + "/" + MyDate.Day.ToString();
                    txtValDt.Text = MyDate.ToString("MM/dd");
                    //to get the full date along with the YEAR. storing and rettiving from hidden text box
                    txtHidenValDt.Text = progPerBE.VALN_MM_DT.ToString();
                }
                else
                {
                    txtValDt.Text = "";
                    txtHidenValDt.Text = "";
                }

                if (progPerBE.NXT_VALN_DT != null)
                    txtNextValDt.Text = Convert.ToDateTime(progPerBE.NXT_VALN_DT).ToShortDateString();
                else
                    txtNextValDt.Text = "";

                if (progPerBE.PREV_VALN_DT != null)
                    txtPrevValDt.Text = Convert.ToDateTime(progPerBE.PREV_VALN_DT).ToShortDateString();
                else
                    txtPrevValDt.Text = "";

                if (progPerBE.ADJ_FREQ_MMS_INTVRL_CNT != null)
                    txtFreq.Text = progPerBE.ADJ_FREQ_MMS_INTVRL_CNT.ToString();
                else
                    txtFreq.Text = "";

                if (progPerBE.FNL_ADJ_DT != null)
                    txtFinalAdj.Text = Convert.ToDateTime(progPerBE.FNL_ADJ_DT).ToShortDateString();
                else
                    txtFinalAdj.Text = "";

                if (progPerBE.BNKRPT_BUYOUT_ID != null)
                {
                    ddlBKTCYBUYOUT.DataBind();
//                    ddlBKTCYBUYOUT.Items.FindByValue(progPerBE.BNKRPT_BUYOUT_ID.ToString()).Selected = true;
                    AddInActiveLookupData(ref ddlBKTCYBUYOUT, progPerBE.BNKRPT_BUYOUT_ID.Value);

                    txtBKTCYBUYOUTEffDate.Enabled = true;
                    imgBKTCYBUYOUTEffDate.Enabled = true;
                }
                else
                {
                    ddlBKTCYBUYOUT.DataBind();
                    ddlBKTCYBUYOUT.Items.FindByValue("0").Selected = true;
                    txtBKTCYBUYOUTEffDate.Enabled = false;
                    imgBKTCYBUYOUTEffDate.Enabled = false;
                }

                if (progPerBE.BNKRPT_BUYOUT_EFF_DT != null)
                    txtBKTCYBUYOUTEffDate.Text = Convert.ToDateTime(progPerBE.BNKRPT_BUYOUT_EFF_DT).ToShortDateString();
                else
                    txtBKTCYBUYOUTEffDate.Text = "";

                chkInclCptvPKCodes.Checked = Convert.ToBoolean(progPerBE.INCLD_CAPTV_PAYKND_IND);
                //chkAvgTM.Checked = Convert.ToBoolean(progPerBE.AVG_TAX_MULTI_IND);
                if (chkAvgTM.Checked == false)
                    txtTMFactor.Enabled = false;
                else
                    txtTMFactor.Enabled = true;

                if (progPerBE.TAX_MULTI_FCTR_RT != null)
                    txtTMFactor.Text = Convert.ToDouble(progPerBE.TAX_MULTI_FCTR_RT).ToString("0.000000");
                //txtTMFactor.Text =  Math.Round(Convert.ToDouble(progPerBE.TAX_MULTI_FCTR_RT), 2).ToString(); 

                else
                    txtTMFactor.Text = "";

                if (progPerBE.COMB_ELEMTS_MAX_AMT != null)
                    txtCombEleMax.Text = progPerBE.COMB_ELEMTS_MAX_AMT.Value.ToString("#,##0");
                else
                    txtCombEleMax.Text = "";

                chkCHFZSC.Checked = Convert.ToBoolean(progPerBE.ZNA_SERV_COMP_CLM_HNDL_FEE_IND);
                if (progPerBE.PEO_PAY_IN_AMT != null)
                    txtPEOPayIn.Text = progPerBE.PEO_PAY_IN_AMT.Value.ToString("#,##0");
                else
                    txtPEOPayIn.Text = "";

                if (progPerBE.FST_ADJ_NON_PREM_MMS_CNT != null)
                    txtFirstAdjNONPREM.Text = progPerBE.FST_ADJ_NON_PREM_MMS_CNT.ToString();
                else
                    txtFirstAdjNONPREM.Text = "";
                if (progPerBE.FREQ_NON_PREM_MMS_CNT != null)
                    txtFreqNonPrem.Text = progPerBE.FREQ_NON_PREM_MMS_CNT.ToString();
                else
                    txtFreqNonPrem.Text = "";

                if (progPerBE.FNL_ADJ_NON_PREM_DT != null)
                    txtFinalAdjNONPREM.Text = Convert.ToDateTime(progPerBE.FNL_ADJ_NON_PREM_DT).ToShortDateString();
                else
                    txtFinalAdjNONPREM.Text = "";
                if (progPerBE.NXT_VALN_DT_NON_PREM_DT != null)
                    txtNextValDtNP.Text = Convert.ToDateTime(progPerBE.NXT_VALN_DT_NON_PREM_DT).ToShortDateString();
                else
                    txtNextValDtNP.Text = "";
                if (progPerBE.PREV_VALN_DT_NON_PREM_DT != null)
                    txtPrevValDtNP.Text = Convert.ToDateTime(progPerBE.PREV_VALN_DT_NON_PREM_DT).ToShortDateString();
                else
                    txtPrevValDtNP.Text = "";

                if (progPerBE.FST_ADJ_DT != null)
                    txtFirstAdjDt.Text = Convert.ToDateTime(progPerBE.FST_ADJ_DT).ToShortDateString();
                else
                    txtFirstAdjDt.Text = "";

                if (progPerBE.FST_ADJ_NON_PREM_DT != null)
                    txtFirstAdjDtNP.Text = Convert.ToDateTime(progPerBE.FST_ADJ_NON_PREM_DT).ToShortDateString();
                else
                    txtFirstAdjDtNP.Text = "";

                if (progPerBE.LSI_RETRIEVE_FROM_DT != null)
                    txtLSIRetriveFromDt.Text = Convert.ToDateTime(progPerBE.LSI_RETRIEVE_FROM_DT).ToShortDateString();
                else
                    txtLSIRetriveFromDt.Text = "";
                               
                //if (txtPrevValDt.Text != "")
                //    txtLSIRetriveFromDt.Text = Convert.ToDateTime(txtPrevValDt.Text).ToShortDateString();

                if (progPerBE.PREV_VALN_DT != null) txtFirstAdj.Enabled = false;
                if (progPerBE.PREV_VALN_DT_NON_PREM_DT != null) txtFirstAdjNONPREM.Enabled = false;
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    pnlDetails.Enabled = Convert.ToBoolean(progPerBE.ACTV_IND);
                //Calling function to Display Program Status Details
                DisplayStatusDetails(ProgramPeriodID);
                btnSave.Text = "Update";
                //Part of Role based security
                if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager && CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.SystemAdmin && chkActive.Checked)
                {
                    txtFreq.Enabled = false;
                    txtFreqNonPrem.Enabled = false;
                    txtFirstAdj.Enabled = false;
                    txtFirstAdjNONPREM.Enabled = false;
                    txtFinalAdj.Enabled = false;
                    imgFinalAdj.Enabled = false;
                    txtFinalAdjNONPREM.Enabled = false;
                    imgFinalAdjNONPREM.Enabled = false;
                    txtConvertsAT.Enabled = false;

                }
                
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, ex);
            }

        }
        protected void DisplayStatusDetails(int ProgramPeriodID)
        {
            ClearStatusControls();

            //IList<PremiumAdjustmentProgramStatusBE> programStatusList = new List<PremiumAdjustmentProgramStatusBE>();

            programStatusList = ProgramStatusBizService.GetProgramStatusList(AISMasterEntities.AccountNumber, ProgramPeriodID);

            BLAccess BLAcc = new BLAccess();

            int initialLookUpID = BLAcc.GetLookUpID("INITIAL");
            int submitLookUPID = BLAcc.GetLookUpID("SUBMIT - SETUP QC");
            int setupLookUpID = BLAcc.GetLookUpID("SETUP QC - COMPLETE");
            int activeLookUpID = BLAcc.GetLookUpID("ACTIVE");

            foreach (PremiumAdjustmentProgramStatusBE sts in programStatusList)
            {
                if (sts.PGM_STATUS_ID == initialLookUpID)
                {
                    chkInitial.Checked = (sts.STS_CHK_IND == true) ? true : false;
                    lblInitialPeriod.Text = "[" + Convert.ToDateTime(sts.CREATEDDATE).ToShortDateString() + "]";
                    txtInitialStsID.Text = sts.PREMUMADJPROG_STS_ID.ToString();
                    txtActiveOldsts.Text = sts.STS_CHK_IND.ToString();
                }

                else if (sts.PGM_STATUS_ID == submitLookUPID)
                {
                    chkSubmitForSetupQC.Checked = (sts.STS_CHK_IND == true) ? true : false;
                    string strlable = Convert.ToDateTime(sts.CREATEDDATE).ToShortDateString();
                    if (sts.UPDATEDATE != null) strlable = strlable + "-" + Convert.ToDateTime(sts.UPDATEDATE).ToShortDateString();
                    lblSubmitQCPeriod.Text = "[" + strlable + "]";
                    txtSubmitStsID.Text = sts.PREMUMADJPROG_STS_ID.ToString();
                    txtSubmitOldSts.Text = sts.STS_CHK_IND.ToString();
                }
                else if (sts.PGM_STATUS_ID == setupLookUpID)
                {
                    chkSetupQCComplete.Checked = (sts.STS_CHK_IND == true) ? true : false;
                    string strlable = Convert.ToDateTime(sts.CREATEDDATE).ToShortDateString();
                    if (sts.UPDATEDATE != null) strlable = strlable + "-" + Convert.ToDateTime(sts.UPDATEDATE).ToShortDateString();
                    lblSetupQCPeriod.Text = "[" + strlable + "]";
                    txtSetupStsID.Text = sts.PREMUMADJPROG_STS_ID.ToString();
                    txtSetupOldSts.Text = sts.STS_CHK_IND.ToString();
                }
                else if (sts.PGM_STATUS_ID == activeLookUpID)
                {
                    chkActive.Checked = (sts.STS_CHK_IND == true) ? true : false;
                    string strlable = Convert.ToDateTime(sts.CREATEDDATE).ToShortDateString();
                    if (sts.UPDATEDATE != null) strlable = strlable + "-" + Convert.ToDateTime(sts.UPDATEDATE).ToShortDateString();
                    lblActivePeriod.Text = "[" + strlable + "]";
                    txtActiveStsID.Text = sts.PREMUMADJPROG_STS_ID.ToString();
                    txtActiveOldsts.Text = sts.STS_CHK_IND.ToString();
                }
                //all status checkboxes has to disble ones user selects Active status
                //this is for normal users. For manager those will be enabled

            }
            if (chkActive.Checked)
                EnableDisableStatusChechkBoxes();
            else
            {
                //if ((txtSubmitStsID.Text != "") && (txtSetupStsID.Text != ""))
                //    chkActive.Enabled = true;
                //else
                //    chkActive.InputAttributes.Add("disabled", "disabled");
            }

        }

        /// <summary>
        /// event fires when User selected Broker in dropdown        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBrokerDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(ddlBrokerDetail.SelectedValue) != 0)
                // Filling Broker Contact Dropdown according to Broker dropdown selection
                BindDDLBrokerContacts(int.Parse(ddlBrokerDetail.SelectedValue));

        }
        /// <summary>
        /// Fill Broker Contact Dropdown according to Broker dropdown selection
        /// </summary>
        /// <param name="ExtOrgID"></param>
        private void BindDDLBrokerContacts(int ExtOrgID)
        {
            PersonBS person = new PersonBS();

            IList<LookupBE> ExtOrg = new List<LookupBE>();
            ExtOrg = person.getContactsByExtOrg(ExtOrgID);

            ddlBrokerContact.DataSource = ExtOrg;
            ddlBrokerContact.DataValueField = "LookUpID";
            ddlBrokerContact.DataTextField = "LookUpName";
            ddlBrokerContact.DataBind();
            ddlBrokerContact.Items.FindByValue("0").Selected = true;
        }
        /// <summary>
        /// Event Fires when User Clicks on Update Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Checking is Update Checkbox is selected or not
                //Checking eighter Broker or BU/Office dropdown is selected
                if (((Convert.ToInt32(ddlBroker.SelectedValue) != 0) || (Convert.ToInt32(ddlBU.SelectedValue) != 0)))
                {
                    string strCheckList = Request.Form["chkSelProgPer"];
                    string[] sampleString;
                    sampleString = strCheckList.Split(',');
                    for (int i = 0; i < sampleString.Length; i++)
                    {
                        int programPeriodID = Convert.ToInt32(sampleString[i].ToString());
                        ProgramPeriodBE PrPeBE = new ProgramPeriodBE();
                        //Loading the BE using Framework
                        PrPeBE = ProgramPeriodsBizService.getProgramPeriodInfo(programPeriodID);
                        //Checking if Broker dropdown is selected any thing
                        //Checking if Existing Broker and selected Broker is same 
                        //If both are same doing nothing ,If not updating

                        if ((Convert.ToInt32(ddlBroker.SelectedValue) != 0) && (Convert.ToInt32(ddlBroker.SelectedValue) != PrPeBE.BRKR_ID))
                        {
                            PrPeBE.BRKR_ID = Convert.ToInt32(ddlBroker.SelectedValue);
                            PrPeBE.BRKR_CONCTC_ID = null;
                        }
                        //Checking if BU/Office dropdown is selected any thing
                        //Checking if Existing BU/Office and selected BU/Office is same 
                        //If both are same, doing nothing ,If not updating
                        if ((Convert.ToInt32(ddlBU.SelectedValue) != 0) && (Convert.ToInt32(ddlBU.SelectedValue) != PrPeBE.BSN_UNT_OFC_ID))
                        {
                            PrPeBE.BSN_UNT_OFC_ID = Convert.ToInt32(ddlBU.SelectedValue);
                        }
                        //Saving the BE using Frame work                           

                        PrPeBE.UPDATE_DATE = DateTime.Now;
                        PrPeBE.UPDATE_USER_ID = CurrentAISUser.PersonID;

                        bool boolIsProgPerSaved = ProgramPeriodsBizService.Update(PrPeBE);

                        if (boolIsProgPerSaved) AuditBizService.Save(AISMasterEntities.AccountNumber, programPeriodID, GlobalConstants.AuditingWebPage.ProgramPeriodSetup, CurrentAISUser.PersonID);
                    }
                    //ProPrgTransWrapper.SubmitTransactionChanges();
                    chkUpdate.Checked = false;
                    ddlBU.DataBind();
                    ddlBU.Items.FindByValue("0").Selected = true;
                    ddlBroker.DataBind();
                    ddlBroker.Items.FindByValue("0").Selected = true;

                    BindProgramPeriodData();
                    CheckBox chk = (CheckBox)lstProgramPeriod.FindControl("chkSelectAll");
                    chk.Checked = false;

                    //}              
                }
            }
            catch (Exception ex)
            {
                //ProPrgTransWrapper.RollbackChanges();
                ShowError(ex.Message, ex);
            }
        }

    }
}


