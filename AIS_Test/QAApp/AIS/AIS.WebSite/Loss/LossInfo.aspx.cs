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


public partial class LossInfo : AISBasePage
{
    #region PageLoad
    string strAdjstatus = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Loss Information";
        //Fill Policy effective date and Expiration date based on Account No. and Premium Adjustment Program Id
        if (!IsPostBack)
        {
            btnDisplayLossesByLob.Enabled = true;
            btnDisplayLossesByPol.Enabled = true;
            //Need to uncomment the below code once intergarted with application
            //strAdjstatus = Request.QueryString[0];
            if (AISMasterEntities.AccountNumber == 0)
            {
                ShowError("Account Number Can't be 0");
                return;
            }
            if (AISMasterEntities.PremiumAdjProgramID.ToString() == "")
            {
                ShowError("Premium Adjustment Program Id Can't be null");
                return;
            }
            int custmrID = AISMasterEntities.AccountNumber;
            int prgID = 0;
            if (AISMasterEntities.PremiumAdjProgramID != "")
                prgID = Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID.ToString());
            DateTime dtValDate = DateTime.Parse(AISMasterEntities.ValuationDate.ToString());
            //Filling Program period dropdown Based on Customer id
            ddlProgramPeriod.DataSource = new ProgramPeriodsBS().GetProgramPeriodsByCustmrID(custmrID);
            ddlProgramPeriod.DataBind();
            IList<ProgramPeriodSearchListBE> prgprdBE = new List<ProgramPeriodSearchListBE>();
            prgprdBE = new ProgramPeriodsBS().GetProgramPeriodsList(prgID.ToString());
            string startEndDate = (prgprdBE[0].STARTDATE_ENDDATE_PGMTYP);
            //if (AISMasterEntities.ExcessLoss.PremiumAdjID != 0)
            //{
            //    ddlProgramPeriod.DataSource = new ProgramPeriodsBS().GetProgramPeriodsByPremAdjID(AISMasterEntities.ExcessLoss.PremiumAdjID.ToString(), dtValDate);
            //    ddlProgramPeriod.DataBind();
            //}
            //else
            //{
            //    ddlProgramPeriod.DataSource = new ProgramPeriodsBS().GetProgramPeriodsList(prgID.ToString());
            //    ddlProgramPeriod.DataBind();
            //}
            if (Session["refProgramPeriod"] != null && Session["refPremAdjPgmID"] != null)
            {
                ddlProgramPeriod.Items.FindByValue(Session["refPremAdjPgmID"].ToString()).Selected = true;
                Session["refProgramPeriod"] = null;
                Session["refPremAdjPgmID"] = null;
                searchMethod();

            }
            else if (prgID > 0)
            {
                //if PrgID >0 then select the value from Program Period Dropdown
                ListItem li = new ListItem(startEndDate.ToString(), prgID.ToString());
                if (ddlProgramPeriod.Items.Contains(li))
                    ddlProgramPeriod.Items.FindByValue(prgID.ToString()).Selected = true;
                if (ddlProgramPeriod.Items.Count > 1 && ddlProgramPeriod.SelectedIndex == 0)
                {
                    ddlProgramPeriod.SelectedIndex = -1;
                    ddlProgramPeriod.Items[1].Selected = true;
                }
                //Calling the Search Method      
                searchMethod();
            }
        }
    }
    #endregion
    protected void ddlProgramPeriod_DataBound(object sender, EventArgs e)
    {
        //Insert Select at index 0 of Program Period Dropdown
        ListItem li = new ListItem("(Select)", "0");
        ddlProgramPeriod.Items.Insert(0, li);
    }
    //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber
    #region BindListview Data
    public void BindListView()
    {
        itemCount = 0;
        DateTime ValDate = DateTime.Parse(AISMasterEntities.ValuationDate.ToString());
        int custmrID = AISMasterEntities.AccountNumber;

        int prgID = Convert.ToInt32(ViewState["PremAdjPgmID"].ToString());
        LossInfoBS lInfoBS = new LossInfoBS();
        //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber!= null for Adjustment
        // Details and Invoice Tab
        if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
        {
            losInfoBEList = lInfoBS.getLossInfoDataAdjNo(ValDate, custmrID, prgID, AISMasterEntities.AdjusmentNumber).Where(obj => obj.PREM_ADJ_ID != null).ToList();
            lsvLossInfo.DataSource = losInfoBEList;
            lsvLossInfo.DataBind();
        }
        //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber= null for UpcomingValuation Tab
        else if (((string)Session["Adjdtls"]) == "LossInfo")
        {
            losInfoBEList = lInfoBS.getLossInfoData(ValDate, custmrID, prgID).Where(obj => obj.PREM_ADJ_ID == null).ToList();
            losInfoBEList = losInfoBEList.Where(loss => loss.VALN_DATE.Value == ValDate).ToList();
            lsvLossInfo.DataSource = losInfoBEList;
            lsvLossInfo.DataBind();
        }
        if (lsvLossInfo.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddl = (DropDownList)lsvLossInfo.InsertItem.FindControl("ddlSavePolicy");
            ListItem li = new ListItem("(Select)", "0");
            ddl.Items.Insert(0, li);
            strAdjstatus = AISMasterEntities.AdjustmentStatus;
            //Applying conditions on lnkSave when adjustment status is  Calc for Adjustment Details and Invoice Tab
            if (Session["Adjdtls"] != null)
            {
                if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
                {
                    strAdjstatus = AISMasterEntities.AdjustmentStatus;
                    if (strAdjstatus != "CALC")
                    {
                        LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                        lnkSelect.Enabled = false;

                    }
                }
                if (((string)Session["Adjdtls"]) == "Invoice")
                {

                    LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                    lnkSelect.Enabled = false;

                }
                //if (((string)Session["Adjdtls"]) == "LossInfo")
                //{
                //    LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                //    lnkSelect.Enabled = false;


                //}
            }
        }

    }
    //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber for Hide Disable Lines.
    public void BindListViewHideDisLin()
    {
        bool Flag = chkHideDisLines.Checked;
        DateTime ValDate = DateTime.Parse(AISMasterEntities.ValuationDate.ToString());
        int custmrID = AISMasterEntities.AccountNumber;

        int prgID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
        LossInfoBS lInfoBS = new LossInfoBS();
        //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber!= null for Adjustment
        // Details and Invoice Tab
        if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
        {
            losInfoBEList = lInfoBS.getLossInfoDataHideDisLinesAdjNo(ValDate, custmrID, prgID, Flag, AISMasterEntities.AdjusmentNumber).Where(obj => obj.PREM_ADJ_ID != null).ToList();
            lsvLossInfo.DataSource = losInfoBEList;
            lsvLossInfo.DataBind();
        }
        //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber= null for UpcomingValuation Tab
        else if (((string)Session["Adjdtls"]) == "LossInfo")
        {
            losInfoBEList = lInfoBS.getLossInfoDataHideDisLines(ValDate, custmrID, prgID, Flag).Where(obj => obj.PREM_ADJ_ID == null).ToList();
            losInfoBEList = losInfoBEList.Where(loss => losInfoBE.VALN_DATE.Value == ValDate).ToList();
            lsvLossInfo.DataSource = losInfoBEList;
            lsvLossInfo.DataBind();
        }


        if (lsvLossInfo.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddl = (DropDownList)lsvLossInfo.InsertItem.FindControl("ddlSavePolicy");
            ListItem li = new ListItem("(Select)", "0");
            ddl.Items.Insert(0, li);
            strAdjstatus = AISMasterEntities.AdjustmentStatus;
            //Applying conditions on lnkSave when adjustment status is  Calc for Adjustment Details and Invoice Tab
            if (Session["Adjdtls"] != null)
            {
                if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
                {
                    strAdjstatus = AISMasterEntities.AdjustmentStatus;
                    if (strAdjstatus != "CALC")
                    {
                        LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                        lnkSelect.Enabled = false;

                    }
                }
                if (((string)Session["Adjdtls"]) == "Invoice")
                {

                    LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                    lnkSelect.Enabled = false;

                }
                //if (((string)Session["Adjdtls"]) == "LossInfo")
                //{
                //    LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                //    lnkSelect.Enabled = false;


                //}
            }


        }
    }

    #endregion
    // Invoked when the Search Button is clicked
    // Bind the Listview Data
    #region Search Button
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        searchMethod();

    }

    private void searchMethod()
    {

        //Traige #67 Part A
             Session["ValnDate"] = String.Empty;
            int prgID = Convert.ToInt32(ddlProgramPeriod.SelectedValue);
            ProgramPeriodBE prgprdBE = new ProgramPeriodBE();
            ProgramPeriodsBS prgprdBS = new ProgramPeriodsBS();
            if (prgID > 0)
            {
                prgprdBE = prgprdBS.getProgramPeriodRow(prgID);
                DateTime NextValDatePrem = Convert.ToDateTime(prgprdBE.NXT_VALN_DT_PREM);
                DateTime NextValDateNonPrem = Convert.ToDateTime(prgprdBE.NXT_VALN_DT_NON_PREM_DT);
                if (NextValDatePrem <= NextValDateNonPrem)
                {
                    Session["ValnDate"] = DateTime.Parse(NextValDatePrem.ToString());
                }
                else
                {
                    Session["ValnDate"] = DateTime.Parse(NextValDateNonPrem.ToString());
                }
            }
       
        //End
        btnDisplayLossesByLob.Enabled = true;
        btnDisplayLossesByPol.Enabled = true;
        bool Flag = chkHideDisLines.Checked;
        ViewState["PremAdjPgmID"] = ddlProgramPeriod.SelectedValue;
        ViewState["ProgramPeriod"] = ddlProgramPeriod.SelectedItem.Text;
        //Calling Bindlistview or BindListViewHideDisLin based on Checkbox status.
        if (!Flag)
            BindListView();
        else
            BindListViewHideDisLin();
    }
    #endregion
    // Invoked when the Edit Link is clicked
    // Set the Listview to Editmode
    #region Edit Listview
    protected void EditList(Object sender, ListViewEditEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lsvLossInfo")
        {
            lsvLossInfo.EditIndex = e.NewEditIndex;
            BindListView();
            //Selecting the values in the dropdowns.
            HiddenField hdlob = ((HiddenField)lsvLossInfo.Items[e.NewEditIndex].FindControl("hidLOB"));
            string strlob = hdlob.Value.ToString();
            //DropDownList ddl=((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlLOB"));
            //    ddl.Items.Insert(0,"(Select)");
            //(((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlLOB")).SelectedItem.Text) = strlob;
            DropDownList ddlLOB = ((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlLOB"));
            //            ddlLOB.Items.FindByValue(strlob.Trim()).Selected = true;
            AddInActiveLookupData(ref ddlLOB, strlob);
            PolicyBS polBS = new PolicyBS();
            DropDownList ddl = ((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlPolicy"));
            int prgID = Convert.ToInt32(Convert.ToInt32(ViewState["PremAdjPgmID"]));
            ddl.DataSource = polBS.getLOBPolData(strlob, prgID);
            ddl.DataBind();

            HiddenField hdpol = ((HiddenField)lsvLossInfo.Items[e.NewEditIndex].FindControl("hidComlAgmtID"));
            string strpol = hdpol.Value.ToString();
            //            ((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlPolicy")).Items.FindByText(strpol).Selected = true;
            AddInActivePolicyData(ref ddl, strpol);
            //(((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlPolicy")).SelectedItem.Text) = strpol;
            HiddenField hdstate = ((HiddenField)lsvLossInfo.Items[e.NewEditIndex].FindControl("hidState"));
            string strstate = hdstate.Value.ToString();

            //(((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlState")).SelectedItem.Text) = strstate;
            DropDownList ddlState = ((DropDownList)lsvLossInfo.Items[e.NewEditIndex].FindControl("ddlState"));
            //            ddlState.Items.FindByText(strstate).Selected = true;
            AddInActiveLookupDataByText(ref ddlState, strstate);
        }

    }
    #endregion

    // Invoked when the Cancel Link is clicked
    #region Cancel Link
    protected void CancelList(Object sender, ListViewCancelEventArgs e)
    {

        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            ListView lstView = (ListView)sender;
            if (lstView.ID.ToString() == "lsvLossInfo")
            {
                lsvLossInfo.EditIndex = -1;
                BindListView();
            }
        }

    }
    #endregion

    protected void InsertList(Object sender, ListViewInsertEventArgs e)
    {
        lsvLossInfo.InsertItemPosition = InsertItemPosition.None;

    }

    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {

        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
            //Fix for bug 7865
            strAdjstatus = AISMasterEntities.AdjustmentStatus;
            //Applying conditions on lnkSave when adjustment status is  Calc for Adjustment Details and Invoice Tab
            if (Session["Adjdtls"] != null)
            {
                if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
                {
                    strAdjstatus = AISMasterEntities.AdjustmentStatus;
                    if (strAdjstatus != "CALC")
                    {
                        LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                        lnkSelect.Enabled = false;

                    }
                }
                if (((string)Session["Adjdtls"]) == "Invoice")
                {

                    LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                    lnkSelect.Enabled = false;

                }
                //if (((string)Session["Adjdtls"]) == "LossInfo")
                //{
                //     LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                //     lnkSelect.Enabled = false;


                //}
            }

        }


    }

    #region Command List for Save,Enable and disable
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            ListView lv = (ListView)sender;
            if (lv.ID.ToString() == "lsvLossInfo")
            {
                SaveList(e.Item);
            }

        }
        else if (e.CommandName == "DISABLE")
        {
            DisableRow(Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName == "ENABLE")
        {
            DisableRow(Convert.ToInt32(e.CommandArgument), true);
        }
        else if (e.CommandName == "SelectExcRow")
        {
            HiddenField hid = (HiddenField)e.Item.FindControl("hidComlAgmtID");
            int intComlAgmtID = Convert.ToInt32(hid.Value);
            HiddenField hidPremAdJID = (HiddenField)e.Item.FindControl("hidPremAdJID");
            HiddenField hidAdjStatus = (HiddenField)e.Item.FindControl("hidAdjStatus");
            string strAdjStatus = string.Empty;
            if (hidPremAdJID.Value == "")
            {
                strAdjStatus = "true";
            }
            else
            {
                if (hidAdjStatus.Value == "")
                {
                    strAdjStatus = "false";
                }
                else { strAdjStatus = "true"; }
            }
            SelectExcRow(Convert.ToInt32(e.CommandArgument), intComlAgmtID, strAdjStatus);

        }
        else if (e.CommandName == "SelectRow")
        {
            ShowLossesByState(Convert.ToInt32(e.CommandArgument));
        }

    }
    #endregion
    /// <summary>
    /// Function to Call LossesByState Report when user clicks on Details Link
    /// </summary>
    /// <param name="intArmisID"></param>
    #region ShowLossesByState Function
    private void ShowLossesByState(int intArmisID)
    {
        Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        AISMasterEntities.ExcessLoss.PremiumAdjPgmID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
        AISMasterEntities.ExcessLoss.ProgramPeriod = ViewState["ProgramPeriod"].ToString();
        AISMasterEntities.ExcessLoss.ARMISLossID = intArmisID;
        Response.Redirect("LossesReport.aspx?mode=STATE");
    }
    #endregion
    #region SelectExc Row
    protected void SelectExcRow(int intARMISLOSID, int intComlAgmtID, string strAdjStatus)
    {

        LossInfoBS losInfoBS = new LossInfoBS();
        LossInfoBE losInfo = losInfoBEList.Single(li => li.ARMIS_LOS_ID == intARMISLOSID);
        if (!losInfo.IsNull())
        {
            AISMasterEntities.ExcessLoss.AdjStatus = strAdjStatus;
            AISMasterEntities.ExcessLoss.ARMISLossID = intARMISLOSID;
            AISMasterEntities.ExcessLoss.ComlAgmtID = intComlAgmtID;
            AISMasterEntities.ExcessLoss.LOB = losInfo.POLICYSYMBOL;
            AISMasterEntities.ExcessLoss.PolicyNumber = losInfo.POLICY;
            AISMasterEntities.ExcessLoss.State = losInfo.STATETYPE;
            AISMasterEntities.ExcessLoss.PremiumAdjPgmID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
            AISMasterEntities.ExcessLoss.ProgramPeriod = ViewState["ProgramPeriod"].ToString();



        }

        Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        Response.Redirect("ExcessNonbillable.aspx", false);

    }
    #endregion

    // Invoked when the Disable Link is clicked
    #region Disable Row
    protected void DisableRow(int intARMISLOSID, bool Flag)
    {

        LossInfoBS losInfoBS = new LossInfoBS();
        LossInfoBE losInfoBE = losInfoBS.getLossInfoRow(intARMISLOSID);
        //Concurrency Issue
        LossInfoBE losinfoBEold = (losInfoBEList.Where(o => o.ARMIS_LOS_ID.Equals(intARMISLOSID))).First();
        bool con = ShowConcurrentConflict(Convert.ToDateTime(losinfoBEold.UPDATEDDATE), Convert.ToDateTime(losInfoBE.UPDATEDDATE));
        if (!con)
            return;
        //End
        losInfoBE.ACTV_IND = Flag;
        losInfoBE.UPDATEDUSER = CurrentAISUser.PersonID;
        losInfoBE.UPDATEDDATE = DateTime.Now;
        Flag = losInfoBS.Update(losInfoBE);
        //ShowConcurrentConflict(Flag, losInfoBE.ErrorMessage);
        //Perform Limiting on Disable
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
        bool ExcNonBil = ExcNonBilBS.PerformLimiting(Convert.ToInt32(AISMasterEntities.AccountNumber), Convert.ToInt32(ViewState["PremAdjPgmID"]));
        //ShowConcurrentConflict(ExcNonBil, "Busy");
        //End
        bool FlagHid = chkHideDisLines.Checked;
        if (!FlagHid)
            BindListView();
        else
            BindListViewHideDisLin();

    }
    #endregion
    // Invoked when the Save Link is clicked
    #region Save Listview Data
    protected void SaveList(ListViewItem e)
    {

        Decimal dTotalPaidIndem = decimal.Parse(((TextBox)e.FindControl("txtSaveTotalPaidIndem")).Text.Replace(",", ""));
        Decimal dTotalPaidExp = decimal.Parse(((TextBox)e.FindControl("txtSaveTotalPaidExp")).Text.Replace(",", ""));
        Decimal dTotalResrvIndem = decimal.Parse(((TextBox)e.FindControl("txtSaveTotalResrvIndem")).Text.Replace(",", ""));
        Decimal dTotalResrvExp = decimal.Parse(((TextBox)e.FindControl("txtSaveTotalResrvExp")).Text.Replace(",", ""));
        LossInfoDA losInfoDA = new LossInfoDA();
        LossInfoBE losInfoBE = new LossInfoBE();
        LossInfoBS losInfoBS = new LossInfoBS();
        losInfoBE.ST_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlSaveState")).SelectedValue);
        losInfoBE.COML_AGMT_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlSavePolicy")).SelectedValue);
        losInfoBE.PAID_IDNMTY_AMT = dTotalPaidIndem;
        losInfoBE.PAID_EXPS_AMT = dTotalPaidExp;
        losInfoBE.RESRV_IDNMTY_AMT = dTotalResrvIndem;
        losInfoBE.RESRV_EXPS_AMT = dTotalResrvExp;
        losInfoBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
        //Traige #67
        //losInfoBE.VALN_DATE = AISMasterEntities.ValuationDate;
        losInfoBE.VALN_DATE = DateTime.Parse(Session["ValnDate"].ToString());
        //End
        losInfoBE.PREM_ADJ_PGM_ID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
        losInfoBE.SUPRT_SERV_CUSTMR_GP_ID = AISMasterEntities.SSCGID;
        //Adjustment ID is updated by Caculation Engine as per Dan
        if (AISMasterEntities.AdjusmentNumber != 0)//&& Session["adjStatus"].ToString() == "CALC")
        {
            losInfoBE.PREM_ADJ_ID = AISMasterEntities.AdjusmentNumber;
        }
        else if ((((string)Session["Adjdtls"]) == "Adjustmentdetails" ||
            ((string)Session["Adjdtls"]) == "Invoice") && AISMasterEntities.AdjusmentNumber == 0)
        {
            LossInfoBS lossInfo = new LossInfoBS();
            losInfoBE.PREM_ADJ_ID =
                lossInfo.GetAdjustmentNumber(AISMasterEntities.AccountNumber, losInfoBE.VALN_DATE.Value);
        }

        losInfoBE.ACTV_IND = true;
        losInfoBE.SYS_GENRT_IND = false;
        losInfoBE.CREATEDATE = DateTime.Now;
        losInfoBE.CREATEUSER = CurrentAISUser.PersonID;
        losInfoBS.Update(losInfoBE);
        ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
        ExcNonBilBS.PerformLimiting(Convert.ToInt32(AISMasterEntities.AccountNumber), Convert.ToInt32(ViewState["PremAdjPgmID"]));
        bool Flag = chkHideDisLines.Checked;
        if (!Flag)
            BindListView();
        else
            BindListViewHideDisLin();

    }
    #endregion

    //Invoke when selected index changed for LOB dropdownlist in Edit mode
    #region Dropdownlist Policy databind
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

        PolicyBS polBS = new PolicyBS();
        DropDownList ddl = (DropDownList)lsvLossInfo.Items[lsvLossInfo.EditIndex].FindControl("ddlPolicy");
        // DropDownList ddl = (DropDownList)lsvLossInfo.EditItem.FindControl("ddlPolicy");
        string LOB = (((DropDownList)lsvLossInfo.Items[lsvLossInfo.EditIndex].FindControl("ddlLOB")).SelectedValue);
        int prgID = Convert.ToInt32(Convert.ToInt32(ViewState["PremAdjPgmID"]));
        ddl.DataSource = polBS.getLOBPolData(LOB, prgID);
        ddl.DataBind();
    }

    //Invoke when selected index changed for LOB dropdownlist in Insert Mode

    protected void ddlSaveLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

        PolicyBS polBS = new PolicyBS();
        DropDownList ddl = (DropDownList)lsvLossInfo.InsertItem.FindControl("ddlSavePolicy");
        string LOB = (((DropDownList)lsvLossInfo.InsertItem.FindControl("ddlSaveLOB")).SelectedValue);
        if (LOB == "(Select)")
        {
            ddl.Enabled = false;
        }
        else
        {
            ddl.Enabled = true;
        }
        int prgID = Convert.ToInt32(Convert.ToInt32(ViewState["PremAdjPgmID"]));
        ddl.DataSource = polBS.getLOBPolData(LOB, prgID);
        //ddl.DataSourceID = ("objDataSourcePolicy");
        ddl.DataBind();
    }
    #endregion

    #region Maintaining Session for LossinfoBE
    private IList<LossInfoBE> losInfoBEList
    {
        get
        {
            if (Session["losInfoBEList"] == null)
                Session["losInfoBEList"] = new List<LossInfoBE>();
            return (IList<LossInfoBE>)Session["losInfoBEList"];            
        }
        set { Session["losInfoBEList"] = value; }
    }

    private LossInfoBE losInfoBE
    {
        get { return (LossInfoBE)Session["LosInfoBE"]; }
        set { Session["LosInfoBE"] = value; }
    }
    #endregion

    // Invoked when the Update Link is clicked
    #region Update Listview Data
    protected void UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lsvLossInfo")
        {
            ListViewItem myItem = lsvLossInfo.Items[e.ItemIndex];
            int intARMISLOSID = int.Parse(((HiddenField)myItem.FindControl("hidArmisLosID")).Value.ToString());
            Decimal dTotalPaidIndem = decimal.Parse(((TextBox)myItem.FindControl("txtTotalPaidIndem")).Text.Replace(",", ""));
            Decimal dTotalPaidExp = decimal.Parse(((TextBox)myItem.FindControl("txtTotalPaidExp")).Text.Replace(",", ""));
            Decimal dTotalResrvIndem = decimal.Parse(((TextBox)myItem.FindControl("txtTotalResrvIndem")).Text.Replace(",", ""));
            Decimal dTotalResrvExp = decimal.Parse(((TextBox)myItem.FindControl("txtTotalResrvExp")).Text.Replace(",", ""));
            //LossInfoDA losInfoDA = new LossInfoDA();
            LossInfoBS losInfoBS = new LossInfoBS();
            LossInfoBE losInfoBE = losInfoBS.getLossInfoRow(intARMISLOSID);
            //Concurrency Issue
            LossInfoBE losinfoBEold = (losInfoBEList.Where(o => o.ARMIS_LOS_ID.Equals(intARMISLOSID))).First();
            bool con=ShowConcurrentConflict(Convert.ToDateTime(losinfoBEold.UPDATEDDATE), Convert.ToDateTime(losInfoBE.UPDATEDDATE));
            if (!con)
                return;
            //End
            losInfoBE.ST_ID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlState")).SelectedValue);
            int intComlID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlPolicy")).SelectedValue);
            int intPremAdjPgmID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
            losInfoBE.COML_AGMT_ID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlPolicy")).SelectedValue);
            losInfoBE.PREM_ADJ_PGM_ID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
            losInfoBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            losInfoBE.PAID_IDNMTY_AMT = dTotalPaidIndem;
            losInfoBE.PAID_EXPS_AMT = dTotalPaidExp;
            losInfoBE.RESRV_IDNMTY_AMT = dTotalResrvIndem;
            losInfoBE.RESRV_EXPS_AMT = dTotalResrvExp;
            //losInfoBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            //losInfoBE.VALN_DATE = AISMasterEntities.ValuationDate;
            //losInfoBE.PREM_ADJ_PGM_ID = Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID.ToString());
            //losInfoBE.SUPRT_SERV_CUSTMR_GP_ID = AISMasterEntities.SSCGID;
            //losInfoBE.PREM_ADJ_ID = AISMasterEntities.AdjusmentNumber;
            losInfoBE.ACTV_IND = true;
            losInfoBE.SYS_GENRT_IND = false;
            losInfoBE.UPDATEDDATE = DateTime.Now;
            losInfoBE.UPDATEDUSER = CurrentAISUser.PersonID;
            //New code
            ExcessNonBillableBS excnonBS = new ExcessNonBillableBS();
            IList<ExcessNonBillableBE> excnonBEList = excnonBS.getExcNonBillableDataLoss(intARMISLOSID);
            for (int i = 0; i < excnonBEList.Count; i++)
            {
                ExcessNonBillableBE excnonBE = excnonBS.getExcessNonBilRow(excnonBEList[i].ARMIS_LOS_EXC_ID);
                excnonBE.ARMIS_LOS_ID = intARMISLOSID;
                excnonBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
                excnonBE.PREM_ADJ_PGM_ID = intPremAdjPgmID;
                excnonBE.COML_AGMT_ID = intComlID;
                excnonBE.UPDATEDDATE = DateTime.Now;
                excnonBE.UPDATEDUSER = CurrentAISUser.PersonID;
                bool Flagexc = excnonBS.Update(excnonBE);
                ShowConcurrentConflict(Flagexc, excnonBE.ErrorMessage);
            }
            //End
            bool Losinfo = losInfoBS.Update(losInfoBE);
            //ShowConcurrentConflict(Losinfo, losInfoBE.ErrorMessage);
            lsvLossInfo.EditIndex = -1;
            ExcessNonBillableBS ExcNonBilBS = new ExcessNonBillableBS();
            ExcNonBilBS.PerformLimiting(Convert.ToInt32(AISMasterEntities.AccountNumber), Convert.ToInt32(Convert.ToInt32(ViewState["PremAdjPgmID"])));
            bool Flag = chkHideDisLines.Checked;
            if (!Flag)
                BindListView();
            else
                BindListViewHideDisLin();

        }

    }
    #endregion
    // Invoke when Check box Hide Disable Lines gets checked and unchecked
    #region Hide Disable Lines
    protected void chkHideDisLines_CheckedChanged(object sender, EventArgs e)
    {
        bool Flag = chkHideDisLines.Checked;
        if (Flag)
        {
            if ((ddlProgramPeriod.SelectedItem.Text.ToString()) == "(Select)" || (ddlProgramPeriod.SelectedItem.Text.ToString()) == "(Select)")
            {
                ShowError("Please select the Date.");
                chkHideDisLines.Checked = false;
                return;
            }
            else
            {
                BindListViewHideDisLin();
            }
        }
        else
        {
            if ((ddlProgramPeriod.SelectedItem.Text.ToString()) == "(Select)" || (ddlProgramPeriod.SelectedItem.Text.ToString()) == "(Select)")
            {
                ShowError("Please select the Program Period");
                chkHideDisLines.Checked = true;
                return;
            }
            else
            {
                BindListView();
            }
        }

    }
    #endregion

    // Invoke when Item created for ListviewItem
    // To check condition for Adjustment status<>Calc and Active indicator
    #region Item created for ListviewItem
    int itemCount = 0;
    protected void lsvLossInfo_ItemCreated(object sender, ListViewItemEventArgs e)
    {
        if (lsvLossInfo.Items.Count == 0)
            itemCount = 0;

     
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
            if (lnkEdit != null)
            {
                if (itemCount < losInfoBEList.Count)
                {
                    string adjStatus = ((List<LossInfoBE>)losInfoBEList)[itemCount].ADJ_STATUS;
                    //Session["adjStatus"] = adjStatus;
                    bool actInd = ((List<LossInfoBE>)losInfoBEList)[itemCount].ACTV_IND.Value;
                    bool sysGen = ((List<LossInfoBE>)losInfoBEList)[itemCount].SYS_GENRT_IND.Value;
                    if ((!((List<LossInfoBE>)losInfoBEList)[itemCount].PREM_ADJ_ID.HasValue) ||
                        (adjStatus == "CALC"))
                    {
                        adjStatus = "true";
                    }
                    else adjStatus = "false";

                    lnkEdit.Enabled = (adjStatus == "true" && actInd && !sysGen);
                    if (((string)Session["Adjdtls"]) == "Invoice")
                    {
                        lnkEdit.Enabled = false;
                    }
                    itemCount++;
                }
            }
        }
        else
        {
            itemCount = 0;
        }

    }
    #endregion
    /// <summary>
    /// DispalyLossesByPolicy Button Click Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region DipalyLossesByPolicy Click Event
    protected void btnDisplayLossesByPol_Click(object sender, EventArgs e)
    {
        AISMasterEntities.ExcessLoss.PremiumAdjPgmID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
        AISMasterEntities.ExcessLoss.ProgramPeriod = ViewState["ProgramPeriod"].ToString();
        Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        Response.Redirect("LossesReport.aspx?mode=Policy");
    }
    #endregion

    /// <summary>
    /// DispalyLossesByLOB Button Click Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region DiaplayLossesByLOB Click Event
    protected void btnDisplayLossesByLob_Click(object sender, EventArgs e)
    {
        AISMasterEntities.ExcessLoss.PremiumAdjPgmID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
        AISMasterEntities.ExcessLoss.ProgramPeriod = ViewState["ProgramPeriod"].ToString();
        Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        Response.Redirect("LossesReport.aspx?mode=LOB");
    }
    #endregion

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
    protected void lsvLossInfo_Sorting(object sender, ListViewSortEventArgs e)
    {
        Image imgSortByExcNonBil = (Image)lsvLossInfo.FindControl("imgSortByExcNonBil");
        Image imgSortByPolicySort = (Image)lsvLossInfo.FindControl("imgSortByPolicySort");
        Image img = new Image();
        switch (e.SortExpression)
        {
            case "EXC_NON_BIL":
                SortBy = "EXC_NON_BIL";
                imgSortByExcNonBil.Visible = true;
                imgSortByPolicySort.Visible = false;
                img = imgSortByExcNonBil;
                break;

            case "POLICY":
                SortBy = "POLICY";
                imgSortByExcNonBil.Visible = false;
                imgSortByPolicySort.Visible = true;
                img = imgSortByPolicySort;
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
        BindLossInformation();
    }


    private void BindLossInformation()
    {
        lsvLossInfo.DataSource = GetSortedPolicyData();
        lsvLossInfo.DataBind();
    }

    private IList<LossInfoBE> GetSortedPolicyData()
    {

        switch (SortBy)
        {
            case "EXC_NON_BIL":
                if (SortDir == "ASC")
                    losInfoBEList = (losInfoBEList.OrderBy(o => o.EXC_NON_BIL)).ToList();
                else if (SortDir == "DESC")
                    losInfoBEList = (losInfoBEList.OrderByDescending(o => o.EXC_NON_BIL)).ToList();

                break;

            case "POLICY":
                if (SortDir == "ASC")
                    losInfoBEList = (losInfoBEList.OrderBy(o => o.POLICY)).ToList();
                else if (SortDir == "DESC")
                    losInfoBEList = (losInfoBEList.OrderByDescending(o => o.POLICY)).ToList();

                break;
        }
        return losInfoBEList;


    }
    #endregion

}

