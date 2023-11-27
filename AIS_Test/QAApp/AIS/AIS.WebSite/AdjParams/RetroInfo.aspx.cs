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
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Logic;
using System.Collections.Generic;
using ZurichNA.AIS.ExceptionHandling;

public partial class AdjParameters_RetroInfo : AISBasePage
{
    protected AISBusinessTransaction RetroInfoTransWrapper
    {
        get
        {
            if ((AISBusinessTransaction)Session["RetroTransaction"] == null)
                Session["RetroTransaction"] = new AISBusinessTransaction();
            return (AISBusinessTransaction)Session["RetroTransaction"];
        }
        set
        {
            Session["RetroTransaction"] = value;
        }
    }

    private RetroInfoBS retroBS;
    private RetroInfoBS RetroBS
    {
        get
        {
            if (retroBS == null)
            {
                retroBS = new RetroInfoBS();
                retroBS.AppTransactionWrapper = RetroInfoTransWrapper;
            }
            return retroBS;
        }
    }
    private ApplWebPageAudtBS retroAuditService;
    private ApplWebPageAudtBS RetroAuditService
    {
        get
        {
            if (retroAuditService == null)
            {
                retroAuditService = new ApplWebPageAudtBS();
                retroAuditService.AppTransactionWrapper = RetroInfoTransWrapper;
            }
            return retroAuditService;
        }
    }
    private IList<RetroInfoBE> RetroListOld
    {
        get { return (IList<RetroInfoBE>)Session["RetroListOld"]; }
        set { Session["RetroListOld"] = value; }
    }
    private int prmPerdID
    {
        get { return ((ViewState["PrmPerdID"] == null) ? 0 : Convert.ToInt32(ViewState["PrmPerdID"])); }
        set { ViewState["PrmPerdID"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        // this.Page.Form.DefaultButton = "btnSave";
        this.Master.Page.Title = "Retro Information";
        if (!Page.IsPostBack)
        {
            RetroInfoTransWrapper = new AISBusinessTransaction();
            btnSave.Visible = false;
            btnCancel.Visible = false;
            try
            {
                if (Request.QueryString["Flag"] != null)
                {
                    AjaxControlToolkit.CollapsiblePanelExtender collaps = (AjaxControlToolkit.CollapsiblePanelExtender)UcMastervalues.FindControl("CollapseAccountHeader");
                    collaps.Collapsed = bool.Parse(Request.QueryString["Flag"].ToString());
                }
                if (Request.QueryString["ProgPerdID"] != null)
                {
                    prmPerdID = Convert.ToInt32(Request.QueryString["ProgPerdID"].ToString());
                    hidProgPerdID.Value = Request.QueryString["ProgPerdID"].ToString();
                    SelectProgPeriod(Convert.ToInt32(Request.QueryString["ProgPerdID"].ToString()));
                    ProgramPeriodBE ProgPerdBE = ((new ProgramPeriodsBS()).getProgramPeriodRow(Convert.ToInt32(hidProgPerdID.Value)));
                    lblEffdt.Text += DateTime.Parse(ProgPerdBE.STRT_DT.ToString()).ToShortDateString();
                    lblExpdt.Text = " - " + DateTime.Parse(ProgPerdBE.PLAN_END_DT.ToString()).ToShortDateString();
                    //Logic to highlighted  Select the ProgramPeriod Line for cinsistancy setting the Public property of ProgramPeriod User control
                    ProgramPeriod.SelectedProgramID = prmPerdID;
                }
            }
            catch (Exception ee)
            {
                lblEffdt.Text = string.Empty;
                lblExpdt.Text = string.Empty;
                lbClose.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                lblRetroInfo.Visible = false;
                ShowError(ee.Message, ee);

            }
        }

        //Checks Exit without Save
        CheckWithoutSave();
    }

    private void CheckWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(btnCancel);
        list.Add(btnSave);
        list.Add(lbClose);
        ProcessExitFlag(list);
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //btnSaveCancel.OperationsButtonClicked += new EventHandler(btnSaveCancel_OperationsButtonClicked);
    }
    public void ProgramPeriod_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        prmPerdID = Convert.ToInt32(e.CommandArgument);
        hidProgPerdID.Value = e.CommandArgument.ToString();
        SelectProgPeriod(Convert.ToInt32(e.CommandArgument));
        Label EffPeriod = (Label)e.Item.FindControl("lblstartDate");
        Label ExpPeriod = (Label)e.Item.FindControl("lblendDate");
        lblEffdt.Text = EffPeriod.Text;
        lblExpdt.Text = " - " + ExpPeriod.Text;
    }
    /// <summary>
    /// Function to Display when Program Period is selected
    /// </summary>
    /// <param name="ProgramPeriodID"></param>
    /// </summary>
    public void SelectProgPeriod(int prmPerdID)
    {

        Label lblPrgmPerId = (Label)this.panRetroInfo.FindControl("hdPgmPerId");
        lblPrgmPerId.Text = prmPerdID.ToString();
        lblRetroInfo.Visible = true;
        lbClose.Visible = true;
        btnSave.Visible = true;
        btnCancel.Visible = true;
        pnlSaveClose.Visible = true;
        panRetroInfo.Visible = true;
        ((ListView)ProgramPeriod.FindControl("lstProgramPeriod")).Enabled = false;
        hdtxtAudExpStandard.Value = RetroBS.GetStandardAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString();
        hdtxtAudExpPayroll.Value = RetroBS.GetPayrollAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString();
        hdtxtAudExpCombined.Value = RetroBS.GetCombinedAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString();
        hdtxtAudExpOther.Value = RetroBS.GetOtherAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString();
        hdtxtAudExpStandardText.Value = GlobalConstants.AuditExposureType.STANDARD_PREMIUM;
        hdtxtAudExpPayrollText.Value = GlobalConstants.AuditExposureType.PAYROLL;
        hdtxtAudExpCombinedText.Value = GlobalConstants.AuditExposureType.COMBINED_AGGREGATE;
        hdtxtAudExpManualText.Value = GlobalConstants.AuditExposureType.MANUAL_PREMIUM;
        GetRetroInfo(prmPerdID);
    }
    protected void LstRetroInfo_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        Label lblhdExpType = (Label)e.Item.FindControl("lblhdExpType");
        Label lblhdPerType = (Label)e.Item.FindControl("lblhdPerType");

        CheckBox checkNoLimit = (CheckBox)e.Item.FindControl("chkNoLimit");

        Label element = (Label)e.Item.FindControl("lblRetroElements");
        if (element.Text.ToUpper() == "BASIC" || element.Text.ToUpper() == "EXCESS LOSS PREMIUM")
        {
            checkNoLimit.Visible = false;
        }
        if (element.Text.ToUpper() == "MINIMUM" || element.Text.ToUpper() == "MAXIMUM")
        {
            checkNoLimit.Visible = true;
        }

        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            CheckBox chk = (CheckBox)e.Item.FindControl("chkDntApply");
            CheckBox chkNoLimit = (CheckBox)e.Item.FindControl("chkNoLimit");
            CompareValidator cmpExpo = (CompareValidator)e.Item.FindControl("valExpType");
            //CompareValidator cmpPer = (CompareValidator)e.Item.FindControl("valPerType");
            TextBox txtExposureAgr = (TextBox)e.Item.FindControl("txtExposureAgr");
            TextBox txtFactor = (TextBox)e.Item.FindControl("txtFactor");
            TextBox txtFactorAgr = (TextBox)e.Item.FindControl("txtFactorAgr");
            TextBox txtFactorAgrAct = (TextBox)e.Item.FindControl("txtFactorAgrAct");
            Label txtTotalAgr = (Label)e.Item.FindControl("txtTotalAgr");
            Label txtTotalAudit = (Label)e.Item.FindControl("txtTotalAudit");
            Label txtAudExp = (Label)e.Item.FindControl("txtAudExp");
            TextBox hidtxtTotalAgr = (TextBox)e.Item.FindControl("hidtxtTotalAgr");
            TextBox hidtxtTotalAudit = (TextBox)e.Item.FindControl("hidtxtTotalAudit");
            TextBox hidtxtAudExp = (TextBox)e.Item.FindControl("hidtxtAudExp");
            DropDownList ddlPerType = (DropDownList)e.Item.FindControl("ddlPerList");
            DropDownList ddlExposureType = (DropDownList)e.Item.FindControl("ddlExpTypelist");
            Label lblRetroElements = (Label)e.Item.FindControl("lblRetroElements");

            //            ddlExposureType.Items.FindByValue(lblhdExpType.Text.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlExposureType, lblhdExpType.Text);
            //            ddlPerType.Items.FindByValue(lblhdPerType.Text.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlPerType, lblhdPerType.Text);

            //------------------------------------
            int intEXPO_TYP_ID = 0;
            if (lblhdExpType.Text != "")
                intEXPO_TYP_ID = Convert.ToInt32(lblhdExpType.Text);
            if (intEXPO_TYP_ID <= 0)//if nothing is there in case of N/A checkbox
            {
                txtAudExp.Text = "0";
                hidtxtAudExp.Text = "0";
            }
            else if (intEXPO_TYP_ID == 131)//payroll
            {
                txtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpPayroll.Value)).ToString("#,##0");
                hidtxtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpPayroll.Value)).ToString("#,##0");
            }
            else if (intEXPO_TYP_ID == 127 && lblRetroElements.Text != "Basic")//Basic Premium
            {
                txtAudExp.Text = Math.Round(decimal.Parse(hidBasicMax.Value)).ToString("#,##0");
                hidtxtAudExp.Text = Math.Round(decimal.Parse(hidBasicMax.Value)).ToString("#,##0");
            }
            else if (intEXPO_TYP_ID == 128)//combined elements
            {
                txtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpCombined.Value)).ToString("#,##0");
                hidtxtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpCombined.Value)).ToString("#,##0");
            }
            else if (intEXPO_TYP_ID == 135)//standard premium
            {
                txtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpStandard.Value)).ToString("#,##0");
                hidtxtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpStandard.Value)).ToString("#,##0");
            }
            else if (intEXPO_TYP_ID != 130)// Manual Premium
            {
                txtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpOther.Value)).ToString("#,##0");
                hidtxtAudExp.Text = Math.Round(decimal.Parse(hdtxtAudExpOther.Value)).ToString("#,##0");
            }
            if (ddlPerType.SelectedIndex > 0)
            {
                txtTotalAudit.Text = Math.Round(((decimal.Parse(txtAudExp.Text.Trim().Length == 0 ? "0.0" : txtAudExp.Text) *
                    decimal.Parse(txtFactorAgr.Text.Trim().Length == 0 ? "0.0" : txtFactorAgr.Text)) /
                    decimal.Parse(ddlPerType.SelectedItem.Text))).ToString("#,##0");
                hidtxtTotalAudit.Text = Math.Round(((decimal.Parse(txtAudExp.Text.Trim().Length == 0 ? "0.0" : txtAudExp.Text) *
                    decimal.Parse(txtFactorAgr.Text.Trim().Length == 0 ? "0.0" : txtFactorAgr.Text)) / 
                    decimal.Parse(ddlPerType.SelectedItem.Text))).ToString("#,##0");
            }
            //-------------------------------------------

            ImageButton ibtnEnableDisable = (ImageButton)e.Item.FindControl("ibtnEnableDisable");
            chk.Attributes["onclick"] = "javascript:ChkNAClick('" + cmpExpo.ClientID + "','" + chk.ClientID + "','" + txtExposureAgr.ClientID + "','" + txtFactor.ClientID + "','" + txtFactorAgr.ClientID + "','" + ddlPerType.ClientID + "','" + ddlExposureType.ClientID + "')";
            chkNoLimit.Attributes["onclick"] = "javascript:ChkNoLimitClick('" + cmpExpo.ClientID + "','" + chkNoLimit.ClientID + "')";
            if (chk.Checked)
            {
                txtExposureAgr.Enabled = false;
                txtFactor.Enabled = false;
                txtFactorAgr.Enabled = false;
                ddlPerType.Enabled = false;
                ddlExposureType.Enabled = false;
                cmpExpo.Enabled = false;
                //cmpPer.Enabled = false;
            }
            //txtExposureAgr.Text = Convert.ToDecimal(txtExposureAgr.Text).ToString("00000000000");
            if (txtExposureAgr.Text != string.Empty)
                txtExposureAgr.Text = Math.Round(Convert.ToDecimal(txtExposureAgr.Text), 0).ToString("#,##0");
            if (ibtnEnableDisable != null)
            {
                Label lblActive = (Label)e.Item.FindControl("lblActive");
                if (lblActive.Text.ToUpper() == "TRUE")
                {
                    ibtnEnableDisable.Visible = true;
                    ibtnEnableDisable.ImageUrl = "~/images/disabled.GIF";
                    ibtnEnableDisable.CommandName = "DISABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                    ibtnEnableDisable.ToolTip = "Click here to Disable this Contact";
                }
                //if (lblActive.Text.ToUpper() == "FALSE")
                else if (lblActive.Text.ToUpper() == "FALSE")
                {
                    ibtnEnableDisable.Visible = true;
                    ibtnEnableDisable.ImageUrl = "~/images/enabled.GIF";
                    ibtnEnableDisable.CommandName = "ENABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                    ibtnEnableDisable.ToolTip = "Click here to Enable this Contact";
                    txtExposureAgr.Enabled = false;
                    txtFactor.Enabled = false;
                    txtFactorAgr.Enabled = false;
                    ddlExposureType.Enabled = false;
                    ddlPerType.Enabled = false;

                }
            }
            if (txtFactorAgrAct.Text != "") txtFactorAgrAct.Text = Convert.ToDecimal(txtFactorAgrAct.Text).ToString("00.000000");
            txtExposureAgr.Attributes["OnChange"] =
                "ExpoChanged('" + txtExposureAgr.ClientID + "','" + txtFactor.ClientID + "','" + txtTotalAgr.ClientID + "','" + txtTotalAudit.ClientID + "','" + txtFactorAgr.ClientID + "','" + txtFactorAgrAct.ClientID + "', '" + ddlExposureType.ClientID + "','" + txtAudExp.ClientID + "','" + ddlPerType.ClientID + "','" + hidtxtTotalAgr.ClientID + "','" + hidtxtAudExp.ClientID + "','" + hidtxtTotalAudit.ClientID + "')";
            txtFactor.Attributes["OnChange"] =
                "ExpoChanged('" + txtExposureAgr.ClientID + "','" + txtFactor.ClientID + "','" + txtTotalAgr.ClientID + "','" + txtTotalAudit.ClientID + "','" + txtFactorAgr.ClientID + "','" + txtFactorAgrAct.ClientID + "','" + ddlExposureType.ClientID + "','" + txtAudExp.ClientID + "','" + ddlPerType.ClientID + "','" + hidtxtTotalAgr.ClientID + "','" + hidtxtAudExp.ClientID + "','" + hidtxtTotalAudit.ClientID + "')";
            txtFactorAgr.Attributes["OnChange"] =
                 "ExpoChanged('" + txtExposureAgr.ClientID + "','" + txtFactor.ClientID + "','" + txtTotalAgr.ClientID + "','" + txtTotalAudit.ClientID + "','" + txtFactorAgr.ClientID + "','" + txtFactorAgrAct.ClientID + "','" + ddlExposureType.ClientID + "','" + txtAudExp.ClientID + "','" + ddlPerType.ClientID + "','" + hidtxtTotalAgr.ClientID + "','" + hidtxtAudExp.ClientID + "','" + hidtxtTotalAudit.ClientID + "')";
            ddlPerType.Attributes["OnChange"] =
                 "ExpoChanged('" + txtExposureAgr.ClientID + "','" + txtFactor.ClientID + "','" + txtTotalAgr.ClientID + "','" + txtTotalAudit.ClientID + "','" + txtFactorAgr.ClientID + "','" + txtFactorAgrAct.ClientID + "','" + ddlExposureType.ClientID + "','" + txtAudExp.ClientID + "','" + ddlPerType.ClientID + "','" + hidtxtTotalAgr.ClientID + "','" + hidtxtAudExp.ClientID + "','" + hidtxtTotalAudit.ClientID + "')";
            ddlExposureType.Attributes["OnChange"] =
                "ExpoTypChanged('" + txtExposureAgr.ClientID + "','" + txtFactor.ClientID + "','" + ddlExposureType.ClientID + "','" + txtAudExp.ClientID + "','" + txtTotalAudit.ClientID + "','" + txtFactorAgr.ClientID + "','" + txtFactorAgrAct.Text + "','" + ddlPerType.ClientID + "','" + hidtxtAudExp.ClientID + "','" + hidtxtTotalAudit.ClientID + "')";

            if (txtFactor.Text != "") txtFactor.Text = Convert.ToDecimal(txtFactor.Text).ToString("00.000000");
            if (txtFactorAgr.Text != "") txtFactorAgr.Text = Convert.ToDecimal(txtFactorAgr.Text).ToString("00.000000");




        }

    }
    protected void LstRetroInfo_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
        {
            try
            {
                bool succeed = false;
                int ADJ_RETRO_INFO_ID = Convert.ToInt32(e.CommandArgument);
                RetroInfoBE retroBE;
                retroBE = RetroBS.LoadData(ADJ_RETRO_INFO_ID);
                if (e.CommandName.ToUpper() == "DISABLE")
                    retroBE.ACTV_IND = false;
                else
                    retroBE.ACTV_IND = true;
                retroBE.UPDT_USER_ID = CurrentAISUser.PersonID;
                retroBE.UPDT_DT = DateTime.Now;
                succeed = RetroBS.SaveRetroData(retroBE);
                if (succeed)
                {
                    RetroAuditService.Save(AISMasterEntities.AccountNumber, prmPerdID, GlobalConstants.AuditingWebPage.RetroInfo, CurrentAISUser.PersonID);
                    RetroInfoTransWrapper.SubmitTransactionChanges();
                }
            }
            catch (System.Exception ex)
            {
                RetroInfoTransWrapper.RollbackChanges();
                ShowError(ex.Message, ex);
            }

        }
        GetRetroInfo(prmPerdID);
    }

    /// <summary>
    /// To get the Retro Information for the given Program Period and to bind the ListView
    /// </summary>
    /// <param name="ProgramPeriodId"></param>
    public void GetRetroInfo(int programPeriodId)
    {
        IList<RetroInfoBE> RetroBEList = RetroBS.GetRetroInfo(programPeriodId);
        RetroListOld = RetroBEList;
        //Basic Row(Triage 61)
        //if (RetroBEList[0].TOT_AGMT_AMT > RetroBEList[0].TOT_AUDT_AMT)
        //{
        //if (RetroBEList[0].TOT_AGMT_AMT == null) RetroBEList[0].TOT_AGMT_AMT = 0;
        //if (RetroBEList[0].TOT_AUDT_AMT == null) RetroBEList[0].TOT_AUDT_AMT = 0;
        if (RetroBEList[0].TOT_AGMT_AMT != null && RetroBEList[0].TOT_AUDT_AMT != null)
            hidBasicMax.Value = RetroBEList[0].TOT_AGMT_AMT > RetroBEList[0].TOT_AUDT_AMT ? RetroBEList[0].TOT_AGMT_AMT.ToString() : RetroBEList[0].TOT_AUDT_AMT.ToString();
        else if (RetroBEList[0].TOT_AGMT_AMT == null && RetroBEList[0].TOT_AUDT_AMT != null)
            hidBasicMax.Value = RetroBEList[0].TOT_AUDT_AMT.ToString();
        else if (RetroBEList[0].TOT_AGMT_AMT != null && RetroBEList[0].TOT_AUDT_AMT == null)
            hidBasicMax.Value = RetroBEList[0].TOT_AGMT_AMT.ToString();
        else
            hidBasicMax.Value = "0";
        hidBasicMax.Value = hidBasicMax.Value == "" ? "0" : hidBasicMax.Value;
        //}
        this.LstRetroInfo.DataSource = RetroBEList;
        LstRetroInfo.DataBind();


        CheckBox chkNoLimit;
        CompareValidator valExpType;
        for (int i = 0; i < LstRetroInfo.Items.Count; i++)
        {
            chkNoLimit = (CheckBox)LstRetroInfo.Items[i].FindControl("chkNoLimit");
            valExpType = (CompareValidator)LstRetroInfo.Items[i].FindControl("valExpType");
            if (chkNoLimit.Checked)
                valExpType.Enabled = false;

        }
        this.pnlSaveClose.Visible = true;
    }
    /// <summary>
    /// To Save the entered/updated Retro Information
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Boolean isNew = true;
        Boolean isDataUpdated = false;
        Boolean isDataUpdate = false;
        try
        {
            foreach (ListViewItem control in LstRetroInfo.Items)
            {
                isNew = true;

                Label hdAdjRetroInfoId = (Label)control.FindControl("hdAdjRetroInfoId");
                // Label hdCustId = (Label)control.FindControl("hdCustId");
                CheckBox chkDntApply = (CheckBox)control.FindControl("chkDntApply");
                CheckBox chkNoLimit = (CheckBox)control.FindControl("chkNoLimit");
                Label lblRetroElements = (Label)control.FindControl("lblRetroElements");
                Label hdRetroElemTypId = (Label)control.FindControl("hdRetroElemTypId");
                TextBox txtExposureAgr = (TextBox)control.FindControl("txtExposureAgr");
                TextBox txtFactor = (TextBox)control.FindControl("txtFactor");
                Label txtTotalAgr = (Label)control.FindControl("txtTotalAgr");
                Label txtAudExp = (Label)control.FindControl("txtAudExp");
                Label txtTotalAudit = (Label)control.FindControl("txtTotalAudit");
                TextBox hidtxtTotalAgr = (TextBox)control.FindControl("hidtxtTotalAgr");
                TextBox hidtxtAudExp = (TextBox)control.FindControl("hidtxtAudExp");
                TextBox hidtxtTotalAudit = (TextBox)control.FindControl("hidtxtTotalAudit");
                DropDownList ddlExpTypelist = (DropDownList)control.FindControl("ddlExpTypelist");

                TextBox txtFactorAgr = (TextBox)control.FindControl("txtFactorAgr");
                DropDownList ddlPerList = (DropDownList)control.FindControl("ddlPerList");

                Boolean success;


                RetroInfoBE retroInfoBE = new RetroInfoBE();
                if (Convert.ToInt64(hdAdjRetroInfoId.Text) > 0)
                {
                    isNew = false;
                    retroInfoBE = RetroBS.LoadData(Convert.ToInt32(hdAdjRetroInfoId.Text));
                    RetroInfoBE retroInfoBEConcurrent = (RetroListOld.Where(o => o.ADJ_RETRO_INFO_ID.Equals(Convert.ToInt32(hdAdjRetroInfoId.Text)))).First();
                    bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(retroInfoBEConcurrent.UPDT_DT), Convert.ToDateTime(retroInfoBE.UPDT_DT));
                    if (!Concurrency)
                        return;
                    retroInfoBE.ADJ_RETRO_INFO_ID = Convert.ToInt32(hdAdjRetroInfoId.Text);
                    isDataUpdated = ((retroInfoBE.EXPO_TYP_ID == Convert.ToInt64(ddlExpTypelist.SelectedValue)) ? false : true);
                    isDataUpdated = ((retroInfoBE.RETRO_ELEMT_TYP_ID == Convert.ToInt64(hdRetroElemTypId.Text)) ? false : true);
                    isDataUpdated = ((retroInfoBE.RETRO_ADJ_FCTR_APLCBL_IND == chkDntApply.Checked) ? false : true);
                    isDataUpdated = ((retroInfoBE.NO_LIM_IND == chkNoLimit.Checked) ? false : true);
                    isDataUpdated = ((retroInfoBE.EXPO_AGMT_AMT.ToString() == txtExposureAgr.Text) ? false : true);
                    isDataUpdated = ((retroInfoBE.RETRO_ADJ_FCTR_RT.ToString() == txtFactor.Text) ? false : true);

                    isDataUpdated = ((retroInfoBE.TOT_AGMT_AMT.ToString() == hidtxtTotalAgr.Text) ? false : true);
                    isDataUpdated = ((retroInfoBE.AUDT_EXPO_AMT.ToString() == hidtxtAudExp.Text) ? false : true);
                    isDataUpdated = ((retroInfoBE.AGGR_FCTR_PCT.ToString() == txtFactorAgr.Text) ? false : true);
                    isDataUpdated = ((retroInfoBE.EXPO_TYP_INCREMNT_NBR_ID == Convert.ToInt64(ddlPerList.SelectedValue)) ? false : true);
                    isDataUpdated = ((retroInfoBE.TOT_AUDT_AMT.ToString() == hidtxtTotalAudit.Text) ? false : true);
                    if (isDataUpdated == true)
                    {
                        isDataUpdate = true;
                    }
                    retroInfoBE.UPDT_DT = DateTime.Now;
                    retroInfoBE.UPDT_USER_ID = CurrentAISUser.PersonID;
                }
                retroInfoBE.PREM_ADJ_PGM_ID = prmPerdID;
                retroInfoBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                if (Convert.ToInt64(ddlExpTypelist.SelectedValue) > 0)
                    retroInfoBE.EXPO_TYP_ID = Convert.ToInt32(ddlExpTypelist.SelectedValue);
                else
                    retroInfoBE.EXPO_TYP_ID = null;
                retroInfoBE.RETRO_ELEMT_TYP_ID = Convert.ToInt32(hdRetroElemTypId.Text);
                if (Convert.ToInt64(ddlPerList.SelectedValue) > 0)
                    retroInfoBE.EXPO_TYP_INCREMNT_NBR_ID = Convert.ToInt32(ddlPerList.SelectedValue);
                else
                    retroInfoBE.EXPO_TYP_INCREMNT_NBR_ID = null;
                if (hidtxtTotalAudit.Text == "")
                    retroInfoBE.TOT_AUDT_AMT = null;
                else
                    retroInfoBE.TOT_AUDT_AMT = Convert.ToDecimal(hidtxtTotalAudit.Text);
                retroInfoBE.NO_LIM_IND = chkNoLimit.Checked;
                retroInfoBE.RETRO_ADJ_FCTR_APLCBL_IND = chkDntApply.Checked;
                if (txtExposureAgr.Text == "")
                    retroInfoBE.EXPO_AGMT_AMT = 0;
                else
                    retroInfoBE.EXPO_AGMT_AMT = Convert.ToDecimal(txtExposureAgr.Text);
                if (txtFactor.Text == "")
                    retroInfoBE.RETRO_ADJ_FCTR_RT = 0;
                else
                    retroInfoBE.RETRO_ADJ_FCTR_RT = Convert.ToDecimal(txtFactor.Text);
                if (hidtxtTotalAgr.Text == "")
                    retroInfoBE.TOT_AGMT_AMT = 0;
                else
                    retroInfoBE.TOT_AGMT_AMT = Convert.ToDecimal(hidtxtTotalAgr.Text);
                if (hidtxtAudExp.Text == "")
                    retroInfoBE.AUDT_EXPO_AMT = 0;
                else
                    retroInfoBE.AUDT_EXPO_AMT = Convert.ToDecimal(hidtxtAudExp.Text);
                if (txtFactorAgr.Text == "")
                    retroInfoBE.AGGR_FCTR_PCT = 0;
                else
                    retroInfoBE.AGGR_FCTR_PCT = Convert.ToDecimal(txtFactorAgr.Text);
                //retroInfoBE.ACTV_IND = true;
                if (isNew)
                {
                    retroInfoBE.CRTE_DT = DateTime.Now;
                    retroInfoBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                    retroInfoBE.ACTV_IND = true;
                    (new RetroInfoBS()).SaveRetroData(retroInfoBE);

                }
                else
                {
                    if (isDataUpdated)
                    {
                        success = retroBS.SaveRetroData(retroInfoBE);
                        ShowConcurrentConflict(success, retroInfoBE.ErrorMessage);
                    }
                }
            }

            if (!isNew && isDataUpdate)
                //Saves Audit Trail for this Page
                RetroAuditService.Save(AISMasterEntities.AccountNumber, prmPerdID,
                    GlobalConstants.AuditingWebPage.RetroInfo, CurrentAISUser.PersonID);

            RetroInfoTransWrapper.SubmitTransactionChanges();
        }
        catch (Exception ex)
        {
            RetroInfoTransWrapper.RollbackChanges();
            ShowError(ex.Message, ex);
        }
        GetRetroInfo(prmPerdID);


    }

    /// <summary>
    /// To cancel the changes to the Retro Information
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        GetRetroInfo(prmPerdID);
    }

    /// <summary>
    /// To close the list view and to activate the Program Period list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CloseContactInfo(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        panRetroInfo.Visible = false;
        lbClose.Visible = false;
        lblRetroInfo.Visible = false;
        pnlSaveClose.Visible = false;
        lblEffdt.Text = String.Empty;
        lblExpdt.Text = String.Empty;
        ((ListView)ProgramPeriod.FindControl("lstProgramPeriod")).Enabled = true;
        hidProgPerdID.Value = "0";


    }
}
