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

public partial class AdjParameters_CombinedElements : AISBasePage
{
    public decimal Val;
    private int PgmPrdID
    {
        get
        {
            if (ViewState["PgmPrd"] == null) return 0;
            return Convert.ToInt32(ViewState["PgmPrd"]);
        }
        set
        {
            ViewState["PgmPrd"] = value;
        }

    }

    private CombinedElementsBE CombElemsBE
    {
        get
        {
            return (CombinedElementsBE)Session["CombElemsInfo-combelemsBE"];
        }
        set { Session["CombElemsInfo-combelemsBE"] = value; }

    }
    private CombinedElementsBS CombElemesService
    {
        get
        {
            return (CombinedElementsBS)Session["CombElemsService"];
        }
        set { Session["CombElemsService"] = value; }
    }
    private ProgramPeriodsBS ProgramperiodsService
    {
        get
        {
            return (ProgramPeriodsBS)Session["ProgramPeriodService"];
        }
        set { Session["ProgramPeriodService"] = value; }

    }
    IList<CombinedElementsBE> lst
    {
        get
        {
            if (Session["CombinedElementsBEList"] == null)
                Session["CombinedElementsBEList"] = new List<CombinedElementsBE>();
            return (IList<CombinedElementsBE>)Session["CombinedElementsBE"];
        }
        set { Session["CombinedElementsBE"] = value; }
    
    }
    private ProgramPeriodBE ProgramperiodsBE
    {
        get { return (ProgramPeriodBE)Session["ProgamInfo-ProgramBE"]; }
        set { Session["ProgamInfo-ProgramBE"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.UcProgramperiods.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(UcProgramperiods_OnItemCommand);
        if (!IsPostBack)
        {
            CombElemsBE = new CombinedElementsBE();
            CombElemesService = new CombinedElementsBS();
            ProgramperiodsBE = new ProgramPeriodBE();
            ProgramperiodsService = new ProgramPeriodsBS();
       
            if (Request.QueryString["ProgPerdID"] != null)
            {
                SelectProgPeriod(Convert.ToInt32(Request.QueryString["ProgPerdID"].ToString()));
                ProgramPeriodBE ProgPerdBE = ((new ProgramPeriodsBS()).getProgramPeriodRow(Convert.ToInt32(Request.QueryString["ProgPerdID"].ToString())));
                lblEffdt.Text += DateTime.Parse(ProgPerdBE.STRT_DT.ToString()).ToShortDateString();
                lblExpdt.Text = " - " + DateTime.Parse(ProgPerdBE.PLAN_END_DT.ToString()).ToShortDateString();
                //Logic to highlighted  Select the ProgramPeriod Line for cinsistancy setting the Public property of ProgramPeriod User control
                UcProgramperiods.SelectedProgramID = Convert.ToInt32(Request.QueryString["ProgPerdID"].ToString());
                
            }
        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(chkAlagreement);
        list.Add(chkLBAagreement);
        list.Add(chkLBALSI);
        list.Add(chkLsialae);
        list.Add(chkPaidagreement);
        list.Add(chkUlaeagreement);
        list.Add(chkUlaeLSIIncluded);
        list.Add(lnkClose);
        ProcessExitFlag(list);
    }


    void UcProgramperiods_OnItemCommand(object sender, ListViewCommandEventArgs e)
    {
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
    public void SelectProgPeriod(int ProgramPeriodID)
    {
        ViewState["PgmPrd"] = ProgramPeriodID;
        hidProgPerdID.Value = ProgramPeriodID.ToString();
        DropDownList ddlExpTyp = new DropDownList();
        this.divContent.Visible = true;
        this.PolicyDataSource.SelectParameters[0].DefaultValue = ProgramPeriodID.ToString();
        BindCombinedElements(ProgramPeriodID);
        BindLsiElements(ProgramPeriodID);
        this.tabCombinedElements.ActiveTabIndex = 0;
        Label lblTotAmount = (Label)this.lstCombinedelements.FindControl("lblTotalAmount");
        lblTotAmount.Text = (decimal.Parse(Val.ToString())).ToString("#,##0");
        //((ListView)ProgramPeriod.FindControl("lstProgramPeriod")).Enabled = false;
        ((ListView)UcProgramperiods.FindControl("lstProgramPeriod")).Enabled = false;
        lblRetroInfo.Visible = true;
        //PopulateDropDownList(GlobalConstants.LookUpType.EXPOSURE_TYPE, ref ddlExpTyp);
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        btnSaveCancel.OperationsButtonClicked += new EventHandler(btnSaveCancel_OperationsButtonClicked);
    }

    private void btnSaveCancel_OperationsButtonClicked(object sender, EventArgs e)
    {
        if (btnSaveCancel.Operation.Trim().ToUpper() == "SAVE")
        {
            ProgramperiodsBE = ProgramperiodsService.getProgramPeriodInfo(PgmPrdID);
            ProgramperiodsBE.AGMT_ALOC_LOS_ADJ_EXPS_IND = this.chkAlagreement.Checked;
            ProgramperiodsBE.AGMT_LOS_BASE_ASSES_IND = this.chkLBAagreement.Checked;
            ProgramperiodsBE.AGMT_PAID_INCUR_IND = this.chkPaidagreement.Checked;
            ProgramperiodsBE.AGMT_UNALOCTD_LOS_ADJ_IND = this.chkUlaeagreement.Checked;
            ProgramperiodsBE.LSI_ALOC_LOS_ADJ_EXPS_IND = this.chkLsialae.Checked;
            //ProgramperiodsBE.LSI_UNALOCTD_LOS_ADJ_IND = this.chkUlaeagreement.Checked;
            ProgramperiodsBE.LSI_UNALOCTD_LOS_ADJ_IND = this.chkUlaeLSIIncluded.Checked;
            ProgramperiodsBE.LSI_LOS_BASE_ASSES_IND = this.chkLBALSI.Checked;

            bool flag = ProgramperiodsService.Update(ProgramperiodsBE);
            ShowConcurrentConflict(flag, ProgramperiodsBE.ErrorMessage);
            BindLsiElements(PgmPrdID);

        }
        if (btnSaveCancel.Operation.Trim().ToUpper() == "CANCEL")
        {
            this.chkAlagreement.Checked=false;
            this.chkPaidagreement.Checked=false;
            this.chkLBAagreement.Checked=false;
            this.chkUlaeagreement.Checked = false;
            this.chkLsialae.Checked=false;
                this.chkUlaeLSIIncluded.Checked=false;
                this.chkLBALSI.Checked = false;
        }

    }
    /// <summary>
    /// This method is used to update the combined elements information.
    /// </summary>
    /// <param name="e"></param>
    protected void lstCombinedelements_ItemUpdate(ListViewItem e)
    {


        Label lblCombelemid = (Label)e.FindControl("lblID");
        decimal per;
        string strCombelemid = lblCombelemid.Text;
        DropDownList ddlExposureType = (DropDownList)e.FindControl("ddlExptype");
        DropDownList ddlPER = (DropDownList)e.FindControl("ddlPEr");
        DropDownList ddlPolicy = (DropDownList)e.FindControl("ddlPolicy");
        TextBox txtRate = (TextBox)e.FindControl("txtRate");
        TextBox txtExposure = (TextBox)e.FindControl("txtauditexp");
        CombElemsBE = CombElemesService.getCombelemID(Convert.ToInt32(strCombelemid));
        CombinedElementsBE CombElemsBEBEOld = (lst.Where(o => o.COMB_ELEMTS_SETUP_ID.Equals(Convert.ToInt32(strCombelemid)))).First();
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(CombElemsBEBEOld.UPDT_DATE), Convert.ToDateTime(CombElemsBE.UPDT_DATE));
        if (!Concurrency)
            return;
        CheckBox chkActind = (CheckBox)e.FindControl("chkActive");

        if (ddlExposureType.SelectedItem != null)
        {


            CombElemsBE.PREM_ADJ_PGM_ID = Convert.ToInt32(ViewState["PgmPrd"]); 
            CombElemsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            CombElemsBE.EXPO_TYP_ID = Convert.ToInt32(ddlExposureType.SelectedValue);
            CombElemsBE.DVSR_NBR_ID = Convert.ToInt32(ddlPER.SelectedValue);
            CombElemsBE.AUDIT_EXPO_AMT = Convert.ToDecimal(txtExposure.Text);
            CombElemsBE.ADJ_RT = Convert.ToDecimal(txtRate.Text);
            per = Convert.ToDecimal(ddlPER.SelectedItem.Text);
            CombElemsBE.TOT_AMT = (Convert.ToDecimal(((CombElemsBE.AUDIT_EXPO_AMT / per) * CombElemsBE.ADJ_RT)));
            CombElemsBE.CRTE_DT = DateTime.Now;
            CombElemsBE.UPDT_DATE = System.DateTime.Now;
            if (chkActind.Checked == false)
                CombElemsBE.ACTV_IND = true;
            CombElemsBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            CombElemsBE.CRTE_USR_ID = 1;

        }
       bool flag= CombElemesService.Update(CombElemsBE);
      // ShowConcurrentConflict(flag, CombElemsBE.ErrorMessage);
        this.lstCombinedelements.EditIndex = -1;
        BindCombinedElements(PgmPrdID);
        Label lblTotAmount = (Label)this.lstCombinedelements.FindControl("lblTotalAmount");
        lblTotAmount.Text = decimal.Parse(Val.ToString()).ToString("#,##0");


    }
    /// <summary>
    /// This method is used to save the combined elements information.
    /// </summary>
    /// <param name="e"></param>
    protected void lstRelatedAccounts_Saving(ListViewItem e)
    {
        DropDownList ddlExposureType = (DropDownList)e.FindControl("ddlExptype");
        DropDownList ddlPER = (DropDownList)e.FindControl("ddlPEr");
        DropDownList ddlPolicy = (DropDownList)e.FindControl("ddlPolicy");
        TextBox txtRate = (TextBox)e.FindControl("txtRate");
        TextBox txtExposure = (TextBox)e.FindControl("txtauditexp");
        decimal per;
        CombinedElementsBE CombinedElemsBE = new CombinedElementsBE();
        if (ddlExposureType.SelectedItem != null)
        {

            //AccountService.Update(accountBE);
           
            CombinedElemsBE.COML_AGMT_ID = Convert.ToInt32(ddlPolicy.SelectedValue);
            CombinedElemsBE.PREM_ADJ_PGM_ID = Convert.ToInt32(ViewState["PgmPrd"]); ;
            CombinedElemsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            CombinedElemsBE.EXPO_TYP_ID = Convert.ToInt32(ddlExposureType.SelectedValue);
            CombinedElemsBE.DVSR_NBR_ID = Convert.ToInt32(ddlPER.SelectedValue);
            CombinedElemsBE.AUDIT_EXPO_AMT = Convert.ToDecimal(txtExposure.Text);
            CombinedElemsBE.ADJ_RT = Convert.ToDecimal(txtRate.Text);
            per = Convert.ToDecimal(ddlPER.SelectedItem.Text);
            CombinedElemsBE.TOT_AMT = (Convert.ToDecimal((CombinedElemsBE.AUDIT_EXPO_AMT/per) * CombinedElemsBE.ADJ_RT));
            CombinedElemsBE.CRTE_DT = DateTime.Now;
            CombinedElemsBE.UPDT_DATE = (Nullable<DateTime>)null;
            CombinedElemsBE.ACTV_IND = true;
            CombinedElemsBE.UPDT_USER_ID = (Nullable<int>)null;
            CombinedElemsBE.CRTE_USR_ID = 1;

        }
        (new CombinedElementsBS()).Update(CombinedElemsBE);
        BindCombinedElements(PgmPrdID);
        Label lblTotAmount = (Label)this.lstCombinedelements.FindControl("lblTotalAmount");
        lblTotAmount.Text = decimal.Parse(Val.ToString()).ToString("#,##0");


        //
        // bind the listview
        // BindRetrosListView();
    }
    /// <summary>
    /// This function is used to Bind the combined elements data to the Listview.
    /// </summary>
    /// <param name="PgmPrdID">PgmPrdID</param>
    public void BindCombinedElements(int PgmPrdID)
    {
        CombinedElementsBS objCE = new CombinedElementsBS();
       
      lst = objCE.GetCombinedElements(PgmPrdID);
        string strval = string.Empty;
        Val = 0;
        for (int intcount = 0; intcount < lst.Count; intcount++)
        {
          
           if((lst[intcount].TOT_AMT.HasValue)&&(lst[intcount].ACTV_IND==true))
            Val = Val + lst[intcount].TOT_AMT.Value;
           
        }
       // Val = Val.ToString("#,##0.00");
       
        this.lstCombinedelements.DataSource = lst;
        this.lstCombinedelements.DataBind();
        Label lblTotAmount = (Label)this.lstCombinedelements.FindControl("lblTotalAmount");
        
        lblTotAmount.Text = (decimal.Parse(Val.ToString())).ToString("#,##0");
        
    }
    /// <summary>
    /// This function is used to bind the LST elements list view.
    /// </summary>
    /// <param name="PgmPrid"></param>
    public void BindLsiElements(int PgmPrid)
    {

        ProgramperiodsBE = ProgramperiodsService.getProgramPeriodInfo(PgmPrid);
       if (ProgramperiodsBE.AGMT_ALOC_LOS_ADJ_EXPS_IND != null)
            this.chkAlagreement.Checked = ProgramperiodsBE.AGMT_ALOC_LOS_ADJ_EXPS_IND.Value;

        if (ProgramperiodsBE.AGMT_LOS_BASE_ASSES_IND != null)
            this.chkLBAagreement.Checked = ProgramperiodsBE.AGMT_LOS_BASE_ASSES_IND.Value;
        if (ProgramperiodsBE.AGMT_UNALOCTD_LOS_ADJ_IND != null)
            this.chkUlaeagreement.Checked = ProgramperiodsBE.AGMT_UNALOCTD_LOS_ADJ_IND.Value;
        if (ProgramperiodsBE.AGMT_PAID_INCUR_IND != null)
            this.chkPaidagreement.Checked = ProgramperiodsBE.AGMT_PAID_INCUR_IND.Value;
        if (ProgramperiodsBE.LSI_LOS_BASE_ASSES_IND != null)
            this.chkLBALSI.Checked = ProgramperiodsBE.LSI_LOS_BASE_ASSES_IND.Value;
        if (ProgramperiodsBE.LSI_UNALOCTD_LOS_ADJ_IND != null)
            //this.chkUlaeagreement.Checked = ProgramperiodsBE.LSI_UNALOCTD_LOS_ADJ_IND.Value;
            this.chkUlaeLSIIncluded.Checked = ProgramperiodsBE.LSI_UNALOCTD_LOS_ADJ_IND.Value;
        if (ProgramperiodsBE.LSI_ALOC_LOS_ADJ_EXPS_IND != null)
            this.chkLsialae.Checked = ProgramperiodsBE.LSI_ALOC_LOS_ADJ_EXPS_IND.Value;


       

    }
    protected void lstCombinedelements_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstCombinedelements.EditIndex = e.NewEditIndex;
        // This function is used to bind the combined elements list view.
        BindCombinedElements(PgmPrdID);
    }
    //public void BindExposureType(DropDownList ddlExpTYp)
    //{


    //    PopulateDropDownList(GlobalConstants.LookUpType.EXPOSURE_TYPE, ref ddlExpTYp);

    //}

    protected void PolicyInfoDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["ProgramPeriodID"] = this.PgmPrdID;
    }

    protected void lstCombinedelements_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {

    }

    protected void lstCombinedelements_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstCombinedelements.EditIndex = -1;
            //  This function is used to bind the combined elements list view.
            BindCombinedElements(PgmPrdID);
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            //Back to normal mode.
            CancelUpdateMode();
        }
    }

    protected void CancelUpdateMode()
    {
        lstCombinedelements.InsertItemPosition = InsertItemPosition.None;
        //  This function is used to bind the combined elements list view.
        BindCombinedElements(PgmPrdID);
    }

    protected void lstCombinedelements_ItemEditing(object sender, ListViewEditEventArgs e)
    {


    }

    protected void lstCombinedelements_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {

    }

    protected void lstCombinedelements_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
    {

    }

    protected void lstCombinedelements_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            // This method is used to save the combined elements information.
            lstRelatedAccounts_Saving(e.Item);
        }
        if (e.CommandName == "Update")
        {
            // This method is used to update the combined elements information.
            lstCombinedelements_ItemUpdate(e.Item);

        }

        if (e.CommandName == "DISABLE")
        {
            // This method is used to daiable the row.
            DisableRow(Convert.ToInt32(e.CommandArgument), false);

        }
        if (e.CommandName == "ENABLE")
        {
            // This method is used to enable the row.
            DisableRow(Convert.ToInt32(e.CommandArgument), true);
        }
    }
    protected void lnkClose_Click(Object Sender, EventArgs E)
    {
        this.divContent.Visible = false;
        ((ListView)UcProgramperiods.FindControl("lstProgramPeriod")).Enabled = true;
    }
    /// <summary>
    /// This method is used to bind the combined elements.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            DropDownList ddlExposureType = (DropDownList)e.Item.FindControl("ddlExptype");
            DropDownList ddlPerType = (DropDownList)e.Item.FindControl("ddlPer");
            Label lblAdtexp = (Label)e.Item.FindControl("lblAuditexp");
            Label lblExposureTypeID = (Label)e.Item.FindControl("lblExpTyp");
            Label lblPerTypeId = (Label)e.Item.FindControl("lblPER");
            Label lblRate = (Label)e.Item.FindControl("lblRate");
            if (lblRate != null)
            {
                if (lblRate.Text.EndsWith("00"))
                {
                    lblRate.Text = lblRate.Text.Substring(0, lblRate.Text.Length - 2);
                }
            }

            //if (lblAdtexp != null)
            //{
            //    if (lblAdtexp.Text.EndsWith("00"))
            //    {
            //        lblAdtexp.Text = lblAdtexp.Text.Substring(0, lblAdtexp.Text.Length - 3);
            //    }
            //}
            if ((ddlExposureType != null) & (lblExposureTypeID != null))
            {
//                ddlExposureType.SelectedIndex = ddlExposureType.Items.IndexOf(ddlExposureType.Items.FindByText(lblExposureTypeID.Text.ToString()));
                AddInActiveLookupDataByText(ref ddlExposureType, lblExposureTypeID.Text);
            }

            if ((ddlPerType != null) & (lblPerTypeId != null))
            {
                ddlPerType.DataBind();
//                ddlPerType.SelectedIndex = ddlPerType.Items.IndexOf(ddlPerType.Items.FindByText(lblPerTypeId.Text.ToString()));
                AddInActiveLookupDataByText(ref ddlPerType, lblPerTypeId.Text);
            }


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
    /// 
    /// </summary>
    /// <param name="Combinedelementid"></param>
    /// <param name="Flag"></param>
    protected void DisableRow(int Combinedelementid, bool Flag)
    {
        // This method is used to populate the combined elementsBE for a particular id.
        CombElemsBE = CombElemesService.getCombelemID(Combinedelementid);
        CombElemsBE.ACTV_IND = Flag;
        CombElemsBE.UPDT_USER_ID = CurrentAISUser.PersonID;
        CombElemsBE.UPDT_DATE = DateTime.Now;
        //This method is used to the update the changes to the combined elements table.
        Flag = CombElemesService.Update(CombElemsBE);
        if (Flag)
        {
            BindCombinedElements(PgmPrdID);

        }

    }


}
