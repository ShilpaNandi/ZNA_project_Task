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
using System.IO;
using System.ComponentModel;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;


public partial class LossInfo : AISBasePage
{

    #region PageLoad
    string strAdjstatus = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Loss Information";
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.btnLossDownload);
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
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
            //if (Session["refProgramPeriod"] != null && Session["refPremAdjPgmID"] != null)
            if (RetrieveObjectFromSessionUsingWindowName("refProgramPeriod") != null && RetrieveObjectFromSessionUsingWindowName("refPremAdjPgmID") != null)
            {
                //ddlProgramPeriod.Items.FindByValue(Session["refPremAdjPgmID"].ToString()).Selected = true;
                ddlProgramPeriod.Items.FindByValue(RetrieveObjectFromSessionUsingWindowName("refPremAdjPgmID").ToString()).Selected = true;
                //Session["refProgramPeriod"] = null;
                //Session["refPremAdjPgmID"] = null;
                SaveObjectToSessionUsingWindowName("refProgramPeriod", null);
                SaveObjectToSessionUsingWindowName("refPremAdjPgmID", null);
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

    protected void Page_Prerender(object sender, EventArgs e)
    {
        if (WEBPAGEROLE == GlobalConstants.ApplicationSecurityGroup.Inquiry)
            btnLossinfo.Visible = false;
        else
            btnLossinfo.Visible = true;
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
        //if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
        if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Adjustmentdetails" || ((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Invoice")
        {
            losInfoBEList = lInfoBS.getLossInfoDataAdjNo(ValDate, custmrID, prgID, AISMasterEntities.AdjusmentNumber).Where(obj => obj.PREM_ADJ_ID != null).ToList();
            lsvLossInfo.DataSource = losInfoBEList;
            lsvLossInfo.DataBind();
        }
        //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber= null for UpcomingValuation Tab
        //else if (((string)Session["Adjdtls"]) == "LossInfo")
        else if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "LossInfo")
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
            //if (Session["Adjdtls"] != null)
            if (RetrieveObjectFromSessionUsingWindowName("Adjdtls") != null)
            {
                // if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
                if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Adjustmentdetails")
                {
                    strAdjstatus = AISMasterEntities.AdjustmentStatus;
                    if (strAdjstatus != "CALC")
                    {
                        LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                        lnkSelect.Enabled = false;

                    }
                }
                //if (((string)Session["Adjdtls"]) == "Invoice")
                if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Invoice")
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
        //if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
        if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Adjustmentdetails" || ((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Invoice")
        {
            losInfoBEList = lInfoBS.getLossInfoDataHideDisLinesAdjNo(ValDate, custmrID, prgID, Flag, AISMasterEntities.AdjusmentNumber).Where(obj => obj.PREM_ADJ_ID != null).ToList();
            lsvLossInfo.DataSource = losInfoBEList;
            lsvLossInfo.DataBind();
        }
        //Bind Listview Data based on Account number,Prem. Adj. Program Id,ValDate,AdjusmentNumber= null for UpcomingValuation Tab
        //else if (((string)Session["Adjdtls"]) == "LossInfo")
        else if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "LossInfo")
        {
            //As per general error bug modified the losInfoBE variable in lamda expression to loss
            losInfoBEList = lInfoBS.getLossInfoDataHideDisLines(ValDate, custmrID, prgID, Flag).Where(obj => obj.PREM_ADJ_ID == null).ToList();
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
            //if (Session["Adjdtls"] != null)
            if (RetrieveObjectFromSessionUsingWindowName("Adjdtls") != null)
            {
                // if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
                if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Adjustmentdetails")
                {
                    strAdjstatus = AISMasterEntities.AdjustmentStatus;
                    if (strAdjstatus != "CALC")
                    {
                        LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                        lnkSelect.Enabled = false;

                    }
                }
                //if (((string)Session["Adjdtls"]) == "Invoice")
                if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Invoice")
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
             //Session["ValnDate"] = String.Empty;
        SaveObjectToSessionUsingWindowName("ValnDate", String.Empty);
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
                    //Session["ValnDate"] = DateTime.Parse(NextValDatePrem.ToString());
                    SaveObjectToSessionUsingWindowName("ValnDate", DateTime.Parse(NextValDatePrem.ToString()));
                }
                else
                {
                    //Session["ValnDate"] = DateTime.Parse(NextValDateNonPrem.ToString());
                    SaveObjectToSessionUsingWindowName("ValnDate", DateTime.Parse(NextValDateNonPrem.ToString()));
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
            //if (Session["Adjdtls"] != null)
            if (RetrieveObjectFromSessionUsingWindowName("Adjdtls") != null)
            {
                //if (((string)Session["Adjdtls"]) == "Adjustmentdetails")
                if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Adjustmentdetails")
                {
                    strAdjstatus = AISMasterEntities.AdjustmentStatus;
                    if (strAdjstatus != "CALC")
                    {
                        LinkButton lnkSelect = (LinkButton)lsvLossInfo.InsertItem.FindControl("lnkSave");
                        lnkSelect.Enabled = false;

                    }
                }
                //if (((string)Session["Adjdtls"]) == "Invoice")
                if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Invoice")
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
        //Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        //Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        SaveObjectToSessionUsingWindowName("refProgramPeriod", ViewState["ProgramPeriod"].ToString());
        SaveObjectToSessionUsingWindowName("refPremAdjPgmID", ViewState["PremAdjPgmID"].ToString());
        AISMasterEntities.ExcessLoss.PremiumAdjPgmID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
        AISMasterEntities.ExcessLoss.ProgramPeriod = ViewState["ProgramPeriod"].ToString();
        AISMasterEntities.ExcessLoss.ARMISLossID = intArmisID;
        //Response.Redirect("LossesReport.aspx?mode=STATE");
        ResponseRedirect("LossesReport.aspx?mode=STATE");
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

        //Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        //Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        SaveObjectToSessionUsingWindowName("refProgramPeriod", ViewState["ProgramPeriod"].ToString());
        SaveObjectToSessionUsingWindowName("refPremAdjPgmID", ViewState["PremAdjPgmID"].ToString());
        //Response.Redirect("ExcessNonbillable.aspx", false);
        ResponseRedirect("ExcessNonbillable.aspx");

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
        //losInfoBE.VALN_DATE = DateTime.Parse(Session["ValnDate"].ToString());
        losInfoBE.VALN_DATE = DateTime.Parse(RetrieveObjectFromSessionUsingWindowName("ValnDate").ToString());
        //End
        losInfoBE.PREM_ADJ_PGM_ID = Convert.ToInt32(ViewState["PremAdjPgmID"]);
        losInfoBE.SUPRT_SERV_CUSTMR_GP_ID = AISMasterEntities.SSCGID;
        //Adjustment ID is updated by Caculation Engine as per Dan
        if (AISMasterEntities.AdjusmentNumber != 0)//&& Session["adjStatus"].ToString() == "CALC")
        {
            losInfoBE.PREM_ADJ_ID = AISMasterEntities.AdjusmentNumber;
        }
            //else if ((((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice") && AISMasterEntities.AdjusmentNumber == 0)
        else if ((((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Adjustmentdetails" ||
            ((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Invoice") && AISMasterEntities.AdjusmentNumber == 0)
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
            //if (Session["losInfoBEList"] == null)
            //    Session["losInfoBEList"] = new List<LossInfoBE>();
            //return (IList<LossInfoBE>)Session["losInfoBEList"];            
            if (RetrieveObjectFromSessionUsingWindowName("losInfoBEList") == null)
                SaveObjectToSessionUsingWindowName("losInfoBEList", new List<LossInfoBE>());
            return (IList<LossInfoBE>)RetrieveObjectFromSessionUsingWindowName("losInfoBEList");
        }
        //set { Session["losInfoBEList"] = value; }
        set { SaveObjectToSessionUsingWindowName("losInfoBEList", value); }
    }

    private LossInfoBE losInfoBE
    {
        //get { return (LossInfoBE)Session["LosInfoBE"]; }
        //set { Session["LosInfoBE"] = value; }
        get { return (LossInfoBE)RetrieveObjectFromSessionUsingWindowName("LosInfoBE"); }
        set { SaveObjectToSessionUsingWindowName("LosInfoBE", value); }
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
                    //if (((string)Session["Adjdtls"]) == "Invoice")
                    if (((string)RetrieveObjectFromSessionUsingWindowName("Adjdtls")) == "Invoice")
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
        //Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        //Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        SaveObjectToSessionUsingWindowName("refProgramPeriod", ViewState["ProgramPeriod"].ToString());
        SaveObjectToSessionUsingWindowName("refPremAdjPgmID", ViewState["PremAdjPgmID"].ToString());
        //Response.Redirect("LossesReport.aspx?mode=Policy");
        ResponseRedirect("LossesReport.aspx?mode=Policy");
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
        //Session["refProgramPeriod"] = ViewState["ProgramPeriod"].ToString();
        //Session["refPremAdjPgmID"] = ViewState["PremAdjPgmID"].ToString();
        SaveObjectToSessionUsingWindowName("refProgramPeriod", ViewState["ProgramPeriod"].ToString());
        SaveObjectToSessionUsingWindowName("refPremAdjPgmID", ViewState["PremAdjPgmID"].ToString());
        //Response.Redirect("LossesReport.aspx?mode=LOB");
        ResponseRedirect("LossesReport.aspx?mode=LOB");
    }
    #endregion

    #region SortBy Property
    private string SortBy
    {
        //get { return (string)Session["SORTBY"]; }
        //set { Session["SORTBY"] = value; }
        get { return (string)RetrieveObjectFromSessionUsingWindowName("SORTBY"); }
        set { SaveObjectToSessionUsingWindowName("SORTBY", value); }
    }
    #endregion

    #region SortDir Property
    private string SortDir
    {
        //get { return (string)Session["SORTDIR"]; }
        //set { Session["SORTDIR"] = value; }
        get { return (string)RetrieveObjectFromSessionUsingWindowName("SORTDIR"); }
        set { SaveObjectToSessionUsingWindowName("SORTDIR", value); }
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


    /* Loss Info Work order Changes */

    protected void btnLossinfo_Click(object sender, EventArgs e)
    {
        //Session["Bulkupload"] = null;
        //Session["Bulkupload_Error"] = null;
        //Session["BulkuploadExc"] = null;
        //Session["Bulkupload_ErrorExc"] = null;
        SaveObjectToSessionUsingWindowName("Bulkupload", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_Error", null);
        SaveObjectToSessionUsingWindowName("BulkuploadExc", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_ErrorExc", null);

        modalLossInfoDetails.Show();

        lblErrorLog.Text = "";
        rbSpreadSheetCopy.Checked = false;
        pnlDirectCopy.Visible = false;
        pnlSpreadSheetCopy.Visible = false;

        //ClearPopupCintrols(true);
        ClearPopupCintrols(false);

        rbDirectCopy.Checked = true;
        rbDirectCopy_CheckedChanged(null,null);

        //Bind Valuation Date
        ProgramPeriodsBS objPGMBS = new ProgramPeriodsBS();
        ProgramPeriodBE pgmBE = objPGMBS.GetPrevValnDate(Convert.ToInt32(ddlProgramPeriod.SelectedValue));
        ddlValuationDate.Items.Clear();
        ddlValuationDateDC.Items.Clear();
        ddlValuationDate.Items.Insert(0,new ListItem("(select)","0"));
        ddlValuationDateDC.Items.Insert(0, new ListItem("(select)", "0"));
        if (pgmBE != null && pgmBE.PREV_VALN_DT != null)
        {            
            string strPrevValDate = pgmBE.PREV_VALN_DT.HasValue ? pgmBE.PREV_VALN_DT.Value.ToString("MM/dd/yyyy") : string.Empty;
            ddlValuationDate.Items.Insert(1, new ListItem(strPrevValDate, strPrevValDate));
            ddlValuationDateDC.Items.Insert(1, new ListItem(strPrevValDate, strPrevValDate));
            ViewState["NextValDate"] = pgmBE.NXT_VALN_DT.HasValue ? pgmBE.NXT_VALN_DT.Value.ToString("MM/dd/yyyy") : string.Empty;
            txtFutureValDate.Text = Convert.ToString(ViewState["NextValDate"]);
        }

        //Bind Adjusmment Number
        PremAdjustmentBS objAdjBS = new PremAdjustmentBS();
        PremiumAdjustmentBE premiumAdjustmentBE1 = objAdjBS.GetRecentAdjID(AISMasterEntities.AccountNumber);
        ddlAdjNo.Items.Clear();
        ddlAdjNoDC.Items.Clear();
        ddlAdjNo.Items.Insert(0, new ListItem("(select)", "0"));
        ddlAdjNoDC.Items.Insert(0, new ListItem("(select)", "0"));
        if (premiumAdjustmentBE1 != null && premiumAdjustmentBE1.PREMIUM_ADJ_ID > 0)
        {
            ddlAdjNo.Items.Insert(1, new ListItem(premiumAdjustmentBE1.PREMIUM_ADJ_ID.ToString(), premiumAdjustmentBE1.VALUATIONDATE.ToString()));
            ddlAdjNoDC.Items.Insert(1, new ListItem(premiumAdjustmentBE1.PREMIUM_ADJ_ID.ToString(), premiumAdjustmentBE1.VALUATIONDATE.ToString()));
        }
    }

    protected void ddlAdjNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ValidateFutureValidate())
        {
            lblErrorLog.Text = string.Empty;
            chkPGMList.Items.Clear();
            chkpollist.Items.Clear();
            btnPGMSelectAll.Visible = false;
            btnSelectAll.Visible = false;
            if (ddlAdjNo.SelectedIndex > 0)
            {
                ddlValuationDate.Enabled = false;
                btnSelectAll.Visible = false;
                btnPGMSelectAll.Visible = false;
                BindProgramPeriod();
            }
            else
                ddlValuationDate.Enabled = true;
        }
        else
        {
            if (ddlAdjNo.SelectedIndex > 0)
            {
                ddlValuationDate.Enabled = false;                
            }
            else
                ddlValuationDate.Enabled = true;
        }

        modalLossInfoDetails.Show();
    }

    protected void ddlValuationDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ValidateFutureValidate())
        {
            lblErrorLog.Text = string.Empty;
            chkPGMList.Items.Clear();
            chkpollist.Items.Clear();
            btnPGMSelectAll.Visible = false;
            btnSelectAll.Visible = false;
            if (ddlValuationDate.SelectedIndex > 0)
            {
                ddlAdjNo.Enabled = false;
                btnSelectAll.Visible = false;
                btnPGMSelectAll.Visible = false;
                BindProgramPeriod();
            }
            else
                ddlAdjNo.Enabled = true;
        }
        else
        {
            if (ddlValuationDate.SelectedIndex > 0)
            {
                ddlAdjNo.Enabled = false;                
            }
            else
                ddlAdjNo.Enabled = true;
        }

        modalLossInfoDetails.Show();
    }

    protected void ddlValuationDateDC_SelectedIndexChanged(object sender, EventArgs e)
    {       
            lblErrorLog.Text = string.Empty;           
            if (ddlValuationDateDC.SelectedIndex > 0)
            {
                ddlAdjNoDC.Enabled = false;               
            }
            else
                ddlAdjNoDC.Enabled = true;       

        modalLossInfoDetails.Show();
    }

    protected void ddlAdjNoDC_SelectedIndexChanged(object sender, EventArgs e)
    {        
            lblErrorLog.Text = string.Empty;
            
            if (ddlAdjNoDC.SelectedIndex > 0)
            {
                ddlValuationDateDC.Enabled = false;               
            }
            else
                ddlValuationDateDC.Enabled = true;       

        modalLossInfoDetails.Show();
    }

    protected void chkPGMList_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorLog.Text = string.Empty;
        chkpollist.Items.Clear();

        BindPolicyList();

        int selectCount = 0;
        foreach (ListItem li in chkPGMList.Items)
        {
            if (li.Selected)
            {
                selectCount++;
            }
        }

        if (chkPGMList.Items.Count == selectCount)
        {
            btnPGMSelectAll.Text = "Deselect All";
        }
        else
        {
            btnPGMSelectAll.Text = "Select All";
        }
        
        modalLossInfoDetails.Show();


    }

    protected void chkpollist_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectCount = 0;
        foreach (ListItem li in chkpollist.Items)
        {
            if (li.Selected)
            {
                selectCount++;
            }
        }

        if (chkpollist.Items.Count == selectCount)
        {
            btnSelectAll.Text = "Deselect All";
        }
        else
        {
            btnSelectAll.Text = "Select All";
        }
        modalLossInfoDetails.Show();
    }

    void BindPolicyList()
    {
        chkpollist.Items.Clear();
        string pgmIDS = "";
        foreach (ListItem li in chkPGMList.Items)
        {
            if (li.Selected)
            {
                if (!string.IsNullOrWhiteSpace(pgmIDS))
                    pgmIDS += "," + li.Value;
                else
                    pgmIDS = li.Value;
            }
        }

        if (!string.IsNullOrWhiteSpace(pgmIDS))
        {
            PolicyBS policyBS = new PolicyBS();
            chkpollist.DataSource = policyBS.getPolicyDataforLookups(pgmIDS, AISMasterEntities.AccountNumber);
            chkpollist.DataValueField = "LookUpID";
            chkpollist.DataTextField = "LookUpName";
            chkpollist.DataBind();

        }
        if (chkpollist.Items.Count > 0)
            btnSelectAll.Visible = true;
        else
            btnSelectAll.Visible = false;
        btnSelectAll.Text = "Select All";
    }

    void BindProgramPeriod()
    {
        chkPGMList.Items.Clear();
        //Bind Program Periods
        ProgramPeriodSearchListBE programPeriodSearchListBE = new ProgramPeriodSearchListBE();
        DateTime valDate = DateTime.Now;
        int intAdjNo = 0;
        if (ddlAdjNo.SelectedIndex > 0)
            valDate = Convert.ToDateTime(ddlAdjNo.SelectedValue);
        if (ddlValuationDate.SelectedIndex > 0)
            valDate = Convert.ToDateTime(ddlValuationDate.SelectedItem.Text);
        ProgramPeriodsBS objPgmBS = new ProgramPeriodsBS();
        chkPGMList.DataSource = objPgmBS.GetPGMByAdjIDValDate(intAdjNo, AISMasterEntities.AccountNumber, valDate);
        chkPGMList.DataValueField = "PREM_ADJ_PGM_ID";
        //chkPGMList.DataTextField = "STARTDATE_ENDDATE";
        chkPGMList.DataTextField = "STARTDATE_ENDDATE_PGMTYP";
        chkPGMList.DataBind();

        btnPGMSelectAll.Visible = (chkPGMList.Items.Count > 0) ? true : false;        
        btnPGMSelectAll.Text = "Select All";
        chkpollist.Items.Clear();
        modalLossInfoDetails.Show();
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


    DataTable ConvertToDatatable(IList<LossInfoBE> list)
    {
        DataTable dt = new DataTable();        
        dt.Columns.Add("Valuation Date");
        dt.Columns.Add("LOB");
        dt.Columns.Add("Policy No");
        dt.Columns.Add("Customer ID");
        dt.Columns.Add("Program Period Eff Date");
        dt.Columns.Add("Program Period Exp Date");
        dt.Columns.Add("Program Type");
        dt.Columns.Add("State");
        dt.Columns.Add("Policy Eff Date");
        dt.Columns.Add("Policy Exp Date");
        dt.Columns.Add("SCGID");
        dt.Columns.Add("Claim Status");
        dt.Columns.Add("System Generated");
        dt.Columns.Add("Total Paid Indemnity");
        dt.Columns.Add("Total Paid Expense");
        dt.Columns.Add("Total Reserved Indemnity");
        dt.Columns.Add("Total Reserved Expense");
        
        foreach (var item in list)
        {
            var row = dt.NewRow();
            row["LOB"] = item.POLICYSYMBOL;
            row["Valuation Date"] = item.VALN_DATE != null ? item.VALN_DATE.Value.ToString("MM/dd/yyyy") : "";
            row["Policy No"] = Convert.ToString(item.POLICY);
            row["Customer ID"] = item.CUSTMR_ID.ToString();
            row["Program Period Eff Date"] = item.PGM_PRD_STRT_DT != null ? item.PGM_PRD_STRT_DT.Value.ToString("MM/dd/yyyy") : "";
            row["Program Period Exp Date"] = item.PGM_PRD_END_DT != null ? item.PGM_PRD_END_DT.Value.ToString("MM/dd/yyyy") : "";
            row["Program Type"] = item.PGMTYPE;
            row["State"] = item.STATETYPE;
            row["Policy Eff Date"] = item.POL_STRT_DT != null ? item.POL_STRT_DT.Value.ToString("MM/dd/yyyy") : "";
            row["Policy Exp Date"] = item.POL_END_DT != null ? item.POL_END_DT.Value.ToString("MM/dd/yyyy") : "";
            row["SCGID"] = "";
            row["System Generated"] = item.SYS_GENRT_IND.ToString();
            row["Total Paid Indemnity"] = item.PAID_IDNMTY_AMT.ToString();
            row["Total Paid Expense"] = item.PAID_EXPS_AMT.ToString();
            row["Total Reserved Indemnity"] = item.RESRV_IDNMTY_AMT.ToString();
            row["Total Reserved Expense"] = item.RESRV_EXPS_AMT.ToString();
            dt.Rows.Add(row);
        }

        return dt;
    }

    protected void btnLossDownload_Click(object sender, EventArgs e)
    {
        bool ispgmPrdChecked = false;
        foreach (ListItem li in chkPGMList.Items)
        {
            if (li.Selected)
            {
                ispgmPrdChecked = true;
                break;
            }
        }
        if ((ddlAdjNo.SelectedIndex < 1 && ddlValuationDate.SelectedIndex < 1))
        {
            lblErrorLog.Text = "Please select Valuation Date/Adjusment Number and at least one program period.";
            modalLossInfoDetails.Show();
        }
        else if (!ispgmPrdChecked)
        {
            lblErrorLog.Text = "Please select at least one program period.";
            modalLossInfoDetails.Show();
        }
        else
        {
            LossInfoBS objLossBS = new LossInfoBS();
            string valDate = "";
            int adjNo = 0;
            int prgID = 0;
            string comlIDs = null;
            int sysGen = 2;

            string strvalDate = null;
            string stradjNo = null;
            string strprgID = null;

            string strsysGen = null;
            string strCustmrID = null;

            foreach (ListItem li in chkpollist.Items)
            {
                if (li.Selected)
                {
                    if (!string.IsNullOrWhiteSpace(comlIDs))
                        comlIDs += "," + li.Value;
                    else
                        comlIDs = li.Value;
                }
            }

            if (ddlLossType.SelectedIndex > 0)
            {
                sysGen = Convert.ToInt32(ddlLossType.SelectedValue);

                strsysGen = ddlLossType.SelectedValue;
            }

            foreach (ListItem li in chkPGMList.Items)
            {
                if (li.Selected)
                {
                    if (!string.IsNullOrWhiteSpace(strprgID))
                        strprgID += "," + li.Value;
                    else
                        strprgID = li.Value;
                }
            }

            if (ddlAdjNo.SelectedIndex > 0)
            {
                valDate = ddlAdjNo.SelectedValue;
                strvalDate = ddlAdjNo.SelectedValue;
            }
            else if (ddlValuationDate.SelectedIndex > 0)
            {
                valDate = ddlValuationDate.SelectedItem.Text;
                strvalDate = ddlValuationDate.SelectedItem.Text;
            }

            //ExportToExcel(dt, strFileName);
            DataTable dt = objLossBS.GetArmisLossDetails(strvalDate, stradjNo, strprgID, comlIDs, strsysGen, strCustmrID);
            DataTable dt2 = objLossBS.GetExcessLossDetails(strvalDate, stradjNo, strprgID, comlIDs, strsysGen, strCustmrID);

            if (dt.Columns["STRT_DT"] != null)
            {
                dt.Columns.Remove("STRT_DT");
            }

            if (dt2.Columns["STRT_DT"] != null)
            {
                dt2.Columns.Remove("STRT_DT");
            }

            if ((dt != null && dt.Rows.Count > 0) || (dt2 != null && dt2.Rows.Count > 0))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt);
                ds.Tables[1].TableName = "Losses";
                ds.Tables[0].TableName = "Excess Losses";

                string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                string strTime = DateTime.Now.ToString("HH-mm-ss");
                string fileName = "CopyLosses_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";
                DataSetsToExcel(ds, fileName);
            }
            else if ((dt == null || dt.Rows.Count == 0) && (dt2 == null || dt2.Rows.Count == 0))
            {
                modalLossInfoDetails.Show();
                lblErrorLog.Text = "There is no loss to be downloaded.";
            }            
        }
    }

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        int selectCount = 0;
        foreach (ListItem li in chkpollist.Items)
        {
            if (li.Selected)
            {
                selectCount++;
            }
        }

        if (chkpollist.Items.Count == selectCount)
        {
            foreach (ListItem li in chkpollist.Items)
            {
                li.Selected = false;
            }
        }
        else
        {
            foreach (ListItem li in chkpollist.Items)
            {
                if (!li.Selected)
                {
                    li.Selected = true;
                }
            }
        }

        selectCount = 0;
        foreach (ListItem li in chkpollist.Items)
        {
            if (li.Selected)
            {
                selectCount++;
            }
        }

        if (chkpollist.Items.Count == selectCount)
        {
            btnSelectAll.Text = "Deselect All";
        }
        else
        {
            btnSelectAll.Text = "Select All";
        }
        modalLossInfoDetails.Show();
    }

    protected void btnPGMSelectAll_Click(object sender, EventArgs e)
    {
        int selectCount = 0;
        foreach (ListItem li in chkPGMList.Items)
        {
            if (li.Selected)
            {
                selectCount++;
            }
        }

        if (chkPGMList.Items.Count == selectCount)
        {
            foreach (ListItem li in chkPGMList.Items)
            {
                li.Selected = false;
            }
        }
        else
        {

            foreach (ListItem li in chkPGMList.Items)
            {
                if (!li.Selected)
                {
                    li.Selected = true;
                }
            }
        }

        selectCount = 0;
        foreach (ListItem li in chkPGMList.Items)
        {
            if (li.Selected)
            {
                selectCount++;
            }
        }

        if (chkPGMList.Items.Count == selectCount)
        {
            btnPGMSelectAll.Text = "Deselect All";
        }
        else
        {
            btnPGMSelectAll.Text = "Select All";
        }
        BindPolicyList();
        modalLossInfoDetails.Show();
    }

    private DataTable GetDataTableExcel(string strFileName,string sheetName)
    {
        System.Data.OleDb.OleDbConnection conn = null;

        try
        {
            conn =
                new System.Data.OleDb.OleDbConnection("Provider=" + ConfigurationManager.AppSettings["oledbProvider"] + "; Data Source = "
                                                + strFileName + "; Extended Properties = \"" + ConfigurationManager.AppSettings["oledbProperties"] + "\";");

            conn.Open();

           
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM ["+ sheetName +"$]", conn);
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

    void BulkUploadValidate()
    {

        //Session["Bulkupload"] = null;
        //Session["Bulkupload_Error"] = null;
        //Session["BulkuploadExc"] = null;
        //Session["Bulkupload_ErrorExc"] = null;
        SaveObjectToSessionUsingWindowName("Bulkupload", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_Error", null);
        SaveObjectToSessionUsingWindowName("BulkuploadExc", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_ErrorExc", null);
        if (flUpload.HasFile)
        {
            if (Path.GetExtension(flUpload.PostedFile.FileName) == ".xls")
            {
                string strDate = DateTime.Now.ToString("MM-dd-yyyy");
                string strTime = DateTime.Now.ToString("HH-mm-ss");
                DateTime dtUploadDateTime = System.DateTime.Now;

                string fileName = "CopyLosses_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";

                if (!(Directory.Exists(ConfigurationManager.AppSettings["CopyLossFilesPath"])))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["CopyLossFilesPath"]);
                }

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                flUpload.SaveAs(ConfigurationManager.AppSettings["CopyLossFilesPath"] + "\\" + fileName);

                //full path name
                string filename = ConfigurationManager.AppSettings["CopyLossFilesPath"] + "\\" + fileName;

                string sheetName = "Losses";
                DataTable excelSheetDT = GetDataTableExcel(filename, sheetName);

                //DataTable errorLogDT = excelSheetDT.Clone();
                //errorLogDT.Columns.Add("Comments", typeof(System.String));

                string sheetNameExc = "Excess Losses";
                DataTable excelSheetDTExc = GetDataTableExcel(filename, sheetNameExc);
                //DataTable errorLogDTExc = excelSheetDTExc.Clone();
                //errorLogDTExc.Columns.Add("Comments", typeof(System.String));

                //Session["Bulkupload"] = excelSheetDT;
                //Session["BulkuploadExc"] = excelSheetDTExc;
                SaveObjectToSessionUsingWindowName("Bulkupload", excelSheetDT);
                SaveObjectToSessionUsingWindowName("BulkuploadExc", excelSheetDTExc);             

                DataColumn crteUserId = new DataColumn("crte_usr_id", typeof(System.String));
                crteUserId.DefaultValue = CurrentAISUser.UserID;
                excelSheetDT.Columns.Add(crteUserId);

                DataColumn crteUserIdExc = new DataColumn("crte_usr_id", typeof(System.String));
                crteUserIdExc.DefaultValue = CurrentAISUser.UserID;
                excelSheetDTExc.Columns.Add(crteUserIdExc);

                DataColumn crte_dt = new DataColumn("crte_dt", typeof(System.DateTime));
                crte_dt.DefaultValue = dtUploadDateTime;
                excelSheetDT.Columns.Add(crte_dt);

                DataColumn crte_dtExc = new DataColumn("crte_dt", typeof(System.DateTime));
                crte_dtExc.DefaultValue = dtUploadDateTime;
                excelSheetDTExc.Columns.Add(crte_dtExc);

                DataColumn validate = new DataColumn("validate", typeof(System.Int32));
                validate.DefaultValue = 1;
                excelSheetDT.Columns.Add(validate);

                DataColumn validateExc = new DataColumn("validate", typeof(System.Int32));
                validateExc.DefaultValue = 1;
                excelSheetDTExc.Columns.Add(validateExc);

                LossInfoBS lInfoBS = new LossInfoBS();
                bool status = lInfoBS.LossesUploadStage(excelSheetDT);
                bool statusExc = lInfoBS.ExcessLossesUploadStage(excelSheetDTExc);

                
                string errmsg = lInfoBS.ModAISLossInfoCopyStageUploads(CurrentAISUser.UserID, dtUploadDateTime);
                DataTable errorLogDT = lInfoBS.LossesUploadsError(CurrentAISUser.UserID, dtUploadDateTime);
                
                //Session["Bulkupload_Error"] = errorLogDT;
                SaveObjectToSessionUsingWindowName("Bulkupload_Error", errorLogDT);

                string errmsgExc = lInfoBS.ModAISLossInfoCopyStageExcessUploads(CurrentAISUser.UserID, dtUploadDateTime);
                DataTable errorLogDTExc = lInfoBS.ExcessLossesUploadsError(CurrentAISUser.UserID, dtUploadDateTime);

                //Session["Bulkupload_ErrorExc"] = errorLogDTExc;
                SaveObjectToSessionUsingWindowName("Bulkupload_ErrorExc", errorLogDTExc);

                if ((excelSheetDT.Rows.Count - errorLogDT.Rows.Count) == 0 && (excelSheetDTExc.Rows.Count - errorLogDTExc.Rows.Count) == 0)
                {
                    btnUploadLoss.Enabled = false;
                }
                else
                {
                    btnUploadLoss.Enabled = true;
                }


                if (errorLogDT.Rows.Count > 0 || errorLogDTExc.Rows.Count > 0)
                {
                    string strErrFileName = "CopyLosses_ERROR_" + CurrentAISUser.PersonID.ToString() + "_" + strDate + "-" + strTime + ".xls";
                    string strErrLogPath = ConfigurationManager.AppSettings["CopyLossFilesPath"] + "\\" + strErrFileName;
                    //ExportToExcel(errorLogDT, strErrLogPath, strErrFileName);
                    DataSet ds = new DataSet();
                    
                    ds.Tables.Add(errorLogDTExc);
                    ds.Tables.Add(errorLogDT);
                    ds.Tables[1].TableName = "Losses";
                    ds.Tables[0].TableName = "Excess Losses";
                    //ExcelHelper.ToExcelPath(ds, strErrLogPath);  
                    DataSetsToExcelSaveOnly(ds, strErrFileName);
                    string strMessage = @"Validated the uploaded records and found errros."+
                                        "Please <a href='" + strErrLogPath + "' target='_blank'>Click Here</a> for Error log.";

                    lblErrorLog.Text = strMessage;
                    modalLossInfoDetails.Show();
                }
                else
                {
                    string strMessage = "Validated successfully.";
                    lblErrorLog.Text = strMessage;
                    modalLossInfoDetails.Show();
                }

            }
            else
            {
                lblErrorLog.Text = "Please Upload .xls format files only.";
            }
        }
        else
        {
            lblErrorLog.Text = "Upload excel sheet";
        }
    }

    void BulkUploadSave()
    {
        lblErrorLog.Text = "";
        //if (Session["Bulkupload"] != null && Session["BulkuploadExc"] != null)
        if (RetrieveObjectFromSessionUsingWindowName("Bulkupload") != null && RetrieveObjectFromSessionUsingWindowName("BulkuploadExc") != null)
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


            //DataTable excelSheetDTExc = (DataTable)Session["BulkuploadExc"];
            DataTable excelSheetDTExc = (DataTable)RetrieveObjectFromSessionUsingWindowName("BulkuploadExc");

            excelSheetDTExc.Columns.Remove("validate");
            excelSheetDTExc.Columns.Remove("crte_usr_id");
            excelSheetDTExc.Columns.Remove("crte_dt");


            DataColumn crteUserIdExc = new DataColumn("crte_usr_id", typeof(System.String));
            crteUserIdExc.DefaultValue = CurrentAISUser.UserID;
            excelSheetDTExc.Columns.Add(crteUserIdExc);

            DataColumn crte_dtExc = new DataColumn("crte_dt", typeof(System.DateTime));
            crte_dtExc.DefaultValue = dtUploadDateTime;
            excelSheetDTExc.Columns.Add(crte_dtExc);

            DataColumn validateExc = new DataColumn("validate", typeof(System.Int32));

            validateExc.DefaultValue = 0;

            excelSheetDTExc.Columns.Add(validateExc);


            LossInfoBS lInfoBS = new LossInfoBS();
            bool status = lInfoBS.LossesUploadStage(excelSheetDT);
            bool statusExc = lInfoBS.ExcessLossesUploadStage(excelSheetDTExc);


            string errmsg = lInfoBS.ModAISLossInfoCopyStageUploads(CurrentAISUser.UserID, dtUploadDateTime);
            DataTable errorLogDT = lInfoBS.LossesUploadsError(CurrentAISUser.UserID, dtUploadDateTime);
            

            string errmsgExc = lInfoBS.ModAISLossInfoCopyStageExcessUploads(CurrentAISUser.UserID, dtUploadDateTime);
            DataTable errorLogDTExc = lInfoBS.ExcessLossesUploadsError(CurrentAISUser.UserID, dtUploadDateTime);
            
            lblErrorLog.Text = "Uploaded successfully.";                      

            //try
           
            //Session["Bulkupload"] = null;
            //Session["Bulkupload_Error"] = null;

            //Session["BulkuploadExc"] = null;
            //Session["Bulkupload_ErrorExc"] = null;
            SaveObjectToSessionUsingWindowName("Bulkupload", null);
            SaveObjectToSessionUsingWindowName("Bulkupload_Error", null);
            SaveObjectToSessionUsingWindowName("BulkuploadExc", null);
            SaveObjectToSessionUsingWindowName("Bulkupload_ErrorExc", null);
        }

        else
        {
            lblErrorLog.Text = "Please validate the records prior to Upload.";
        }
    }

    protected void btnValidateLoss_Click(object sender, EventArgs e)
    {
        BulkUploadValidate();
        
        modalLossInfoDetails.Show();
    }

    protected void btnUploadLoss_Click(object sender, EventArgs e)
    {
        BulkUploadSave();
       
        modalLossInfoDetails.Show();

        btnUploadLoss.Enabled = false;
    }

    protected void btnPopCancel_Click(object sender, EventArgs e)
    {
        //Session["Bulkupload"] = null;
        //Session["Bulkupload_Error"] = null;
        //Session["BulkuploadExc"] = null;
        //Session["Bulkupload_ErrorExc"] = null;
        SaveObjectToSessionUsingWindowName("Bulkupload", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_Error", null);
        SaveObjectToSessionUsingWindowName("BulkuploadExc", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_ErrorExc", null);

        modalLossInfoDetails.Show();
        lblErrorLog.Text = string.Empty;
        if (rbDirectCopy.Checked)
        {
            ClearPopupCintrols(true);
            rbDirectCopy.Checked = true;
            pnlDirectCopy.Visible = true;
        }
        if (rbSpreadSheetCopy.Checked)
        {
            ClearPopupCintrols(false);
            rbSpreadSheetCopy.Checked = true;
            pnlSpreadSheetCopy.Visible = true;
        }        
    }

    private void ClearPopupCintrols(bool isDirectCopy)
    {
        if (isDirectCopy)
        {
            //Direct Copy Section clear
            ddlValuationDateDC.Enabled = true;
            ddlAdjNoDC.Enabled = true;
            btnDirectCopy.Enabled = true;
            txtFutureValDate.Text = Convert.ToString(ViewState["NextValDate"]);

            if (ddlValuationDateDC.Items.FindByValue("0") != null)
            {
                ddlValuationDateDC.ClearSelection();
                ddlValuationDateDC.Items.FindByValue("0").Selected = true;
            }

            if (ddlAdjNoDC.Items.FindByValue("0") != null)
            {
                ddlAdjNoDC.ClearSelection();
                ddlAdjNoDC.Items.FindByValue("0").Selected = true;
            }
        }
        else if (!isDirectCopy)
        {
            //Spreadsheet Copy Section clear
            chkPGMList.Items.Clear();
            chkpollist.Items.Clear();
            ddlValuationDate.Enabled = true;
            ddlAdjNo.Enabled = true;
            btnValidateLoss.Enabled = true;
            btnUploadLoss.Enabled = false;
            btnSelectAll.Text = "Select All";
            btnPGMSelectAll.Text = "Select All";
            btnPGMSelectAll.Visible = false;
            btnSelectAll.Visible = false;
            if (ddlValuationDate.Items.FindByValue("0") != null)
            {
                ddlValuationDate.ClearSelection();
                ddlValuationDate.Items.FindByValue("0").Selected = true;
            }
            if (ddlAdjNo.Items.FindByValue("0") != null)
            {
                ddlAdjNo.ClearSelection();
                ddlAdjNo.Items.FindByValue("0").Selected = true;
            }
        }         

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        //Session["Bulkupload"] = null;
        //Session["Bulkupload_Error"] = null;
        //Session["BulkuploadExc"] = null;
        //Session["Bulkupload_ErrorExc"] = null;
        SaveObjectToSessionUsingWindowName("Bulkupload", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_Error", null);
        SaveObjectToSessionUsingWindowName("BulkuploadExc", null);
        SaveObjectToSessionUsingWindowName("Bulkupload_ErrorExc", null);

        if (rbDirectCopy.Checked)
        {
            ClearPopupCintrols(true);
            rbDirectCopy.Checked = false;
            pnlDirectCopy.Visible = false;
        }
        if (rbSpreadSheetCopy.Checked)
        {
            ClearPopupCintrols(false);
            rbSpreadSheetCopy.Checked = false;
            pnlSpreadSheetCopy.Visible = false;
        }  
       
        modalLossInfoDetails.Hide();
        searchMethod();
    }

    protected void btnDirectCopy_Click(object sender, EventArgs e)
    {
        if (txtFutureValDate.Text.Trim() != "" && (ddlAdjNoDC.SelectedIndex > 0 || ddlValuationDateDC.SelectedIndex > 0))
        {
            LossInfoBS objLossBS = new LossInfoBS();
            string comlIDs = null;
            string strvalDate = null;
            string stradjNo = null;
            string strprgID = null;
            string strsysGen = null;
            string strCustmrID = null;
            string valDate = "";
            strCustmrID = AISMasterEntities.AccountNumber.ToString();
            if (ddlAdjNoDC.SelectedIndex > 0)
            {
                valDate = ddlAdjNoDC.SelectedValue;
                strvalDate = ddlAdjNoDC.SelectedValue;
            }
            else if (ddlValuationDateDC.SelectedIndex > 0)
            {
                valDate = ddlValuationDateDC.SelectedItem.Text;
                strvalDate = ddlValuationDateDC.SelectedItem.Text;
            }

            DataTable dt = objLossBS.GetArmisLossDetails(strvalDate, stradjNo, strprgID, comlIDs, strsysGen, strCustmrID);
            DataTable dt2 = objLossBS.GetExcessLossDetails(strvalDate, stradjNo, strprgID, comlIDs, strsysGen, strCustmrID);
            DateTime dtUploadDateTime = System.DateTime.Now;
            DataTable excelSheetDT = dt;

            if ((dt != null && dt.Rows.Count > 0) || (dt2 != null && dt2.Rows.Count > 0))
            {
                DataColumn crteUserId = new DataColumn("crte_usr_id", typeof(System.String));
                crteUserId.DefaultValue = CurrentAISUser.UserID;
                excelSheetDT.Columns.Add(crteUserId);

                DataColumn crte_dt = new DataColumn("crte_dt", typeof(System.DateTime));
                crte_dt.DefaultValue = dtUploadDateTime;
                excelSheetDT.Columns.Add(crte_dt);

                DataColumn validate = new DataColumn("validate", typeof(System.Int32));
                validate.DefaultValue = 0;
                excelSheetDT.Columns.Add(validate);
                excelSheetDT.Columns.Remove("VALUATION DATE");

                DataColumn valuationDate = new DataColumn("VALUATION DATE", typeof(System.String));
                valuationDate.DefaultValue = txtFutureValDate.Text.Trim();
                excelSheetDT.Columns.Add(valuationDate);

                DataTable excelSheetDTExc = dt2;
                DataColumn crteUserIdExc = new DataColumn("crte_usr_id", typeof(System.String));
                crteUserIdExc.DefaultValue = CurrentAISUser.UserID;
                excelSheetDTExc.Columns.Add(crteUserIdExc);

                DataColumn crte_dtExc = new DataColumn("crte_dt", typeof(System.DateTime));
                crte_dtExc.DefaultValue = dtUploadDateTime;
                excelSheetDTExc.Columns.Add(crte_dtExc);

                DataColumn validateExc = new DataColumn("validate", typeof(System.Int32));
                validateExc.DefaultValue = 0;
                excelSheetDTExc.Columns.Add(validateExc);
                excelSheetDTExc.Columns.Remove("VALUATION DATE");

                DataColumn valuationDate1 = new DataColumn("VALUATION DATE", typeof(System.String));
                valuationDate1.DefaultValue = txtFutureValDate.Text.Trim();
                excelSheetDTExc.Columns.Add(valuationDate1);

                LossInfoBS lInfoBS = new LossInfoBS();
                bool status = lInfoBS.LossesUploadStage(excelSheetDT);
                bool statusExc = lInfoBS.ExcessLossesUploadStage(excelSheetDTExc);
                string errmsg = lInfoBS.ModAISLossInfoCopyStageUploads(CurrentAISUser.UserID, dtUploadDateTime);
                DataTable errorLogDT = lInfoBS.LossesUploadsError(CurrentAISUser.UserID, dtUploadDateTime);
                string errmsgExc = lInfoBS.ModAISLossInfoCopyStageExcessUploads(CurrentAISUser.UserID, dtUploadDateTime);
                DataTable errorLogDTExc = lInfoBS.ExcessLossesUploadsError(CurrentAISUser.UserID, dtUploadDateTime);

                if (errorLogDT.Rows.Count > 0 || errorLogDTExc.Rows.Count > 0)
                {
                    string strMessage = string.Empty;
                    if (errorLogDT.Rows.Count > 0 && errorLogDTExc.Rows.Count > 0)
                    {
                        strMessage = "Error(Loss | Excess Loss) :" + Convert.ToString(errorLogDT.Rows[0]["Comments"]) + " | " + Convert.ToString(errorLogDTExc.Rows[0]["Comments"]);
                    }
                    else if (errorLogDT.Rows.Count > 0)
                    {
                        strMessage = "Error Loss :" + Convert.ToString(errorLogDT.Rows[0]["Comments"]);
                    }
                    else if (errorLogDTExc.Rows.Count > 0)
                    {
                        strMessage = "Error Excess Loss :" + Convert.ToString(errorLogDTExc.Rows[0]["Comments"]);
                    }
                    //06/23 for veracode
                    //lblErrorLog.Text = strMessage;
                    //lblErrorLog.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(strMessage)); // EAISA-7 fix 06142018
                    lblErrorLog.Text = string.IsNullOrEmpty(strMessage.Trim()) ? "" : HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(strMessage)); // veracode 06252018

                }
                else
                {
                    lblErrorLog.Text = "Losses Copied successfully.";
                }
            }
            else if ((dt == null || dt.Rows.Count == 0) && (dt2 == null || dt2.Rows.Count == 0))
            {
                lblErrorLog.Text = "There is no loss to be copied.";
            }            
        }
        else
        {
            lblErrorLog.Text = "Please provide current valuation date and previous Valuation Date/Adjustment Number";
        }

        modalLossInfoDetails.Show();
    }

    protected void rbDirectCopy_CheckedChanged(object sender, EventArgs e)
    {
        modalLossInfoDetails.Show();
        lblErrorLog.Text = string.Empty;

        if (rbDirectCopy.Checked)
        {            
            pnlDirectCopy.Visible  = true;
            pnlSpreadSheetCopy.Visible = false;

            ClearPopupCintrols(true);     
        }
        else if(!rbDirectCopy.Checked)
        {
            pnlDirectCopy.Visible = false;
            pnlSpreadSheetCopy.Visible = false;
        }
    }

    protected void rbSpreadSheetCopy_CheckedChanged(object sender, EventArgs e)
    {
        modalLossInfoDetails.Show();
        lblErrorLog.Text = string.Empty;

        if (rbSpreadSheetCopy.Checked)
        {
            pnlDirectCopy.Visible  = false;
            pnlSpreadSheetCopy.Visible = true;
            btnDirectCopy.Enabled = false;

            ClearPopupCintrols(false);            
        }
        else if (!rbDirectCopy.Checked)
        {
            rbSpreadSheetCopy.Visible = false;
            pnlSpreadSheetCopy.Visible = false;
        }
    }

    bool ValidateFutureValidate()
    {
        bool status = false;

        if ( ddlValuationDate.SelectedIndex > 0 || ddlAdjNo.SelectedIndex >0)
        {
            //btnDirectCopy.Visible = true;
            btnValidateLoss.Enabled = true;
            status = true;
        }
        else
        {
           // btnDirectCopy.Visible = false;
            btnValidateLoss.Enabled = false;
        }

        return status;
    }

    private void ExportToExcel(DataSet ds, string ExcelFilePath, string strErrFileName)
    {
        try
        {
            if (ds == null || ds.Tables.Count == 0)
                throw new Exception("ExportToExcel: Null or empty input table!\n");

            // load excel, and create a new workbook
            Excel.Application excelApp = new Excel.Application();
            excelApp.Workbooks.Add();

            // single worksheet
            Excel._Worksheet workSheet = excelApp.ActiveSheet as Excel.Worksheet;

            workSheet.Cells.NumberFormat = "@";

            DataTable Tbl = ds.Tables[0];
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

    private void FormatLossesExcel(Excel.Worksheet xlWorksheet)
    {
        Excel.Range validationDateRange = xlWorksheet.get_Range("A2").EntireColumn;
        validationDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range programEffectiveDateRange = xlWorksheet.get_Range("E2").EntireColumn;
        programEffectiveDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range programExpirationDateRange = xlWorksheet.get_Range("F2").EntireColumn;
        programExpirationDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range policyEffectiveDateRange = xlWorksheet.get_Range("I2").EntireColumn;
        policyEffectiveDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range policyExpirationDateRange = xlWorksheet.get_Range("J2").EntireColumn;
        policyExpirationDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range scgidRange = xlWorksheet.get_Range("K2").EntireColumn;
        scgidRange.NumberFormat = "@";
    }

    private void FormatExcessLossesExcel(Excel.Worksheet xlWorksheet)
    {
        Excel.Range validationDateRange = xlWorksheet.get_Range("A1").EntireColumn;
        validationDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range programEffectiveDateRange = xlWorksheet.get_Range("E1").EntireColumn;
        programEffectiveDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range programExpirationDateRange = xlWorksheet.get_Range("F1").EntireColumn;
        programExpirationDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range policyEffectiveDateRange = xlWorksheet.get_Range("I1").EntireColumn;
        policyEffectiveDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range policyExpirationDateRange = xlWorksheet.get_Range("J1").EntireColumn;
        policyExpirationDateRange.NumberFormat = "MM/DD/YYYY";

        Excel.Range scgidRange = xlWorksheet.get_Range("K2").EntireColumn;
        scgidRange.NumberFormat = "@";

        Excel.Range coverageDateRange = xlWorksheet.get_Range("P1").EntireColumn;
        coverageDateRange.NumberFormat = "MM/DD/YYYY";

    }

    public void DataSetsToExcel(DataSet dataSet, string fileName)
    {
        Microsoft.Office.Interop.Excel.Application xlApp =
                  new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
        foreach (DataTable dataTable in dataSet.Tables)
        {

            Excel.Sheets xlSheets = null;
            Excel.Worksheet xlWorksheet = null;

            //foreach (DataSet dataSet in dataSets)
            //{
            //System.Data.DataTable dataTable = dataSet.Tables[0];
            int rowNo = dataTable.Rows.Count;
            int columnNo = dataTable.Columns.Count;
            int colIndex = 0;

            //Create Excel Sheets
            xlSheets = xlWorkbook.Sheets;
            xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                           Type.Missing, Type.Missing, Type.Missing);
            xlWorksheet.Name = dataTable.TableName;

            //Generate Field Names
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                colIndex++;
                xlApp.Cells[1, colIndex] = dataColumn.ColumnName;                
            }

            object[,] objData = new object[rowNo, columnNo];

            //Convert DataSet to Cell Data
            for (int row = 0; row < rowNo; row++)
            {
                for (int col = 0; col < columnNo; col++)
                {
                    objData[row, col] = dataTable.Rows[row][col];
                }
            }

            //Add the Data
            Excel.Range range = xlWorksheet.Range[xlApp.Cells[2, 1], xlApp.Cells[rowNo + 1, columnNo]];
            range.Value2 = objData;

            Excel.Range firstRow = xlWorksheet.Range[xlApp.Cells[1, 1], xlApp.Cells[1, columnNo]];
            firstRow.Font.Bold = true;
            firstRow.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            if (xlWorksheet.Name == "Losses")
                FormatLossesExcel(xlWorksheet);
            if (xlWorksheet.Name == "Excess Losses")
                FormatExcessLossesExcel(xlWorksheet);

            xlWorksheet.Columns.AutoFit();
        }
        //}

        //Remove the Default Worksheet
        ((Excel.Worksheet)xlApp.ActiveWorkbook.Sheets[xlApp.ActiveWorkbook.Sheets.Count]).Delete();

        //Save
        xlWorkbook.SaveAs(ConfigurationManager.AppSettings["CopyLossFilesPath"] + "\\" + fileName,
            Excel.XlFileFormat.xlExcel8,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            Excel.XlSaveAsAccessMode.xlNoChange,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value);

        xlWorkbook.Close();
        xlApp.Quit();
        GC.Collect();

       MemoryStream memStream = new MemoryStream();
        
        try
        {
            using (FileStream fileStream = File.OpenRead(ConfigurationManager.AppSettings["CopyLossFilesPath"] + "\\" + fileName))
            {

                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                byte[] byteArray = memStream.ToArray();
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
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
        }
    }

    public void DataSetsToExcelSaveOnly(DataSet dataSet, string fileName)
    {
        Microsoft.Office.Interop.Excel.Application xlApp =
                  new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
        foreach (DataTable dataTable in dataSet.Tables)
        {

            Excel.Sheets xlSheets = null;
            Excel.Worksheet xlWorksheet = null;

            //foreach (DataSet dataSet in dataSets)
            //{
            //System.Data.DataTable dataTable = dataSet.Tables[0];
            int rowNo = dataTable.Rows.Count;
            int columnNo = dataTable.Columns.Count;
            int colIndex = 0;

            //Create Excel Sheets
            xlSheets = xlWorkbook.Sheets;
            xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1],
                           Type.Missing, Type.Missing, Type.Missing);
            xlWorksheet.Name = dataTable.TableName;

            //Generate Field Names
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                colIndex++;
                xlApp.Cells[1, colIndex] = dataColumn.ColumnName;
            }

            object[,] objData = new object[rowNo, columnNo];

            //Convert DataSet to Cell Data
            for (int row = 0; row < rowNo; row++)
            {
                for (int col = 0; col < columnNo; col++)
                {
                    objData[row, col] = dataTable.Rows[row][col];
                }
            }

            //Add the Data
            Excel.Range range = xlWorksheet.Range[xlApp.Cells[2, 1], xlApp.Cells[rowNo + 1, columnNo]];
            range.Value2 = objData;

            Excel.Range firstRow = xlWorksheet.Range[xlApp.Cells[1, 1], xlApp.Cells[1, columnNo]];
            firstRow.Font.Bold = true;
            firstRow.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            xlWorksheet.Columns.AutoFit();
        }
        //}

        //Remove the Default Worksheet
        ((Excel.Worksheet)xlApp.ActiveWorkbook.Sheets[xlApp.ActiveWorkbook.Sheets.Count]).Delete();

        //Save
        xlWorkbook.SaveAs(ConfigurationManager.AppSettings["CopyLossFilesPath"] + "\\" + fileName,
            Excel.XlFileFormat.xlExcel8,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            Excel.XlSaveAsAccessMode.xlNoChange,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value,
            System.Reflection.Missing.Value);

        xlWorkbook.Close();
        xlApp.Quit();
        GC.Collect();

        //MemoryStream memStream = new MemoryStream();
        //try
        //{
        //    using (FileStream fileStream = File.OpenRead(ConfigurationManager.AppSettings["CopyLossFilesPath"] + "\\" + fileName))
        //    {

        //        memStream.SetLength(fileStream.Length);
        //        fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

        //        byte[] byteArray = memStream.ToArray();
        //        Response.Clear();
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        //        Response.AddHeader("content-length", memStream.Length.ToString());
        //        Response.BinaryWrite(byteArray);
        //    }

        //}
        //catch (Exception ex)
        //{
        //    ShowError("Unable to Preview the Report. Please contact Application Support Team");
        //    return;
        //}
        //finally
        //{
        //    memStream.Close();
        //    Response.Flush();
        //    Response.End();
        //}
    }
}

//ExcelHelper.cs

//public class ExcelHelper
//{
//    //Row limits older Excel version per sheet
//    const int rowLimit = 65000;

//    private static string getWorkbookTemplate()
//    {
//        var sb = new System.Text.StringBuilder();
//        sb.Append("<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n");
//        sb.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n		xmlns:x=\"urn:schemas- microsoft-com:office:excel\"\r\n		xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n");
//        sb.Append(" <Styles>\r\n		<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n		<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>");
//        sb.Append("\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n		<Protection/>\r\n </Style>\r\n		<Style ss:ID=\"BoldColumn\">\r\n <Font ");
//        sb.Append("x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n		<Style ss:ID=\"s62\">\r\n <NumberFormat");
//        sb.Append(" ss:Format=\"@\"/>\r\n </Style>\r\n 		<Style ss:ID=\"Decimal\">\r\n		<NumberFormat ss:Format=\"0.0000\"/>\r\n </Style>\r\n ");
//        sb.Append("<Style ss:ID=\"Integer\">\r\n		<NumberFormat ss:Format=\"0\"/>\r\n </Style>\r\n		<Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ");
//        sb.Append("ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n		<Style ss:ID=\"s28\">\r\n");
//        sb.Append("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Top\"		ss:ReadingOrder=\"LeftToRight\" ss:WrapText=\"1\"/>\r\n");
//        sb.Append("<Font x:CharSet=\"1\" ss:Size=\"9\"		ss:Color=\"#808080\" ss:Underline=\"Single\"/>\r\n");
//        sb.Append("<Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/>		</Style>\r\n</Styles>\r\n {0}</Workbook>");
//        return sb.ToString();
//    }

//    private static string replaceXmlChar(string input)
//    {
//        input = input.Replace("&", "&");
//        input = input.Replace("<", "<");
//        input = input.Replace(">", ">");
//        input = input.Replace("\"", "");
//        input = input.Replace("'", "&apos;");
//        return input;
//    }

//    private static string getWorksheets(DataSet source)
//    {
//        var sw = new StringWriter();
//        if (source == null || source.Tables.Count == 0)
//        {
//            sw.Write("<Worksheet ss:Name=\"Sheet1\"><Table><Row>		<Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data>		</Cell></Row></Table></Worksheet>");
//            return sw.ToString();
//        }
//        foreach (DataTable dt in source.Tables)
//        {
//            if (dt.Rows.Count == 0)
//            {
//                sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + "\"><Table><Row>");
//                foreach (DataColumn dc in dt.Columns)
//                    sw.Write(
//                        string.Format("<Cell ss:StyleID=\"BoldColumn\">	<Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dc.ColumnName)));
//                sw.Write("</Row>\r\n");

//                sw.Write("</Table></Worksheet>");
//            }
//            else
//            {
//                //write each row data
//                var sheetCount = 0;
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {
//                    if ((i % rowLimit) == 0)
//                    {
//                        //add close tags for previous sheet of the same data table
//                        if ((i / rowLimit) > sheetCount)
//                        {
//                            sw.Write("</Table></Worksheet>");
//                            sheetCount = (i / rowLimit);
//                        }
//                        sw.Write("<Worksheet ss:Name=\"" +
//            replaceXmlChar(dt.TableName) +
//                                 (((i / rowLimit) == 0) ? "" :
//            Convert.ToString(i / rowLimit)) + "\"><Table>");
//                        //write column name row
//                        sw.Write("<Row>");
//                        foreach (DataColumn dc in dt.Columns)
//                            sw.Write(
//                                string.Format("<Cell ss:StyleID=\"BoldColumn\">	<Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dc.ColumnName)));
//                        sw.Write("</Row>\r\n");
//                    }
//                    sw.Write("<Row>\r\n");
//                    foreach (DataColumn dc in dt.Columns)
//                        sw.Write(
//                            string.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dt.Rows[i][dc.ColumnName].ToString())));
//                    sw.Write("</Row>\r\n");
//                }
//                sw.Write("</Table></Worksheet>");
//            }
//        }

//        return sw.ToString();
//    }
//    public static string GetExcelXml(DataTable dtInput, string filename)
//    {
//        var excelTemplate = getWorkbookTemplate();
//        var ds = new DataSet();
//        ds.Tables.Add(dtInput.Copy());
//        var worksheets = getWorksheets(ds);
//        var excelXml = string.Format(excelTemplate, worksheets);
//        return excelXml;
//    }

//    public static string GetExcelXml(DataSet dsInput, string filename)
//    {
//        var excelTemplate = getWorkbookTemplate();
//        var worksheets = getWorksheets(dsInput);
//        var excelXml = string.Format(excelTemplate, worksheets);
//        return excelXml;
//    }

//    public static void ToExcel(DataSet dsInput, string filename, HttpResponse response)
//    {
//        var excelXml = GetExcelXml(dsInput, filename);
//        response.Clear();
//        //response.AppendHeader("Content-Type", "application/vnd.xls");
//        //response.AppendHeader("Content-Type", "application/vnd.ms-excel");
//        response.AppendHeader("Content-disposition", "attachment; filename=" + filename);
//        response.Write(excelXml);
//        response.Flush();
//        response.End();
//    }


//    public static void ToExcelPath(DataSet dsInput, string path)
//    {
//        string excelXml = GetExcelXml(dsInput, "copylosses.xls");
//        System.IO.File.WriteAllText(path, excelXml);

//    }

//    public static void ToExcel(DataTable dtInput, string filename, HttpResponse response)
//    {
//        var ds = new DataSet();
//        ds.Tables.Add(dtInput.Copy());
//        ToExcel(ds, filename, response);
//    }

   
//}
