//Default namespaces in AdjustProcessingChklst screen
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
//Importing different AIS framework namespaces for AdjustProcessingChklst screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;
namespace ZurichNA.AIS.WebSite.AdjCalc
{
    /// <summary>
    /// This is the class which is used to give the information about the AdjustProcessingChklst and it 
    /// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
    /// the Basepage
    /// </summary>
    #region AdjustProcessingChklst Class
    
    public partial class AdjustProcessingChklst : AISBasePage
    {
       
        #region Properties Declaration
        /// <summary>
        /// A Property for Holding CustomerID in ViewState
        /// </summary>
        protected int AccountID
        {
            get
            {
                if (ViewState["AccountID"] != null)
                {
                    return int.Parse(ViewState["AccountID"].ToString());
                }
                else
                {
                    ViewState["AccountID"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["AccountID"] = value;
            }
        }
        /// <summary>
        /// A Property for Holding PremAdjID in ViewState
        /// </summary>
        protected int PreAdjID
        {
            get
            {
                if (ViewState["PreAdjID"] != null)
                {
                    return int.Parse(ViewState["PreAdjID"].ToString());
                }
                else
                {
                    ViewState["PreAdjID"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["PreAdjID"] = value;
            }
        }
        /// <summary>
        /// A Property for Holding PremAdjProgramID in ViewState
        /// </summary>
        protected int PrgPeriodID
        {
            get
            {
                if (ViewState["PrgPeriodID"] != null)
                {
                    return int.Parse(ViewState["PrgPeriodID"].ToString());
                }
                else
                {
                    ViewState["PrgPeriodID"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["PrgPeriodID"] = value;
            }
        }

        /// <summary>
        /// A Property for Holding PremAdjPeriodID in ViewState
        /// </summary>
        protected int PremAdjPerdID
        {
            get
            {
                if (ViewState["PremAdjPerdID"] != null)
                {
                    return int.Parse(ViewState["PremAdjPerdID"].ToString());
                }
                else
                {
                    ViewState["PremAdjPerdID"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["PremAdjPerdID"] = value;
            }
        }
        /// <summary>
        /// A Property for Holding AdjStatusTypeID in ViewState
        /// </summary>
        protected int AdjStatusTypeID
        {
            get
            {
                if (ViewState["AdjStatusTypeID"] != null)
                {
                    return int.Parse(ViewState["AdjStatusTypeID"].ToString());
                }
                else
                {
                    ViewState["AdjStatusTypeID"] = 0;
                    return 0;
                }
            }
            set
            {
                ViewState["AdjStatusTypeID"] = value;
            }
        }

        /// <summary>
        /// a property for PremiumAdjustmentPeriod Business Service Class
        /// </summary>
        /// <returns>PremiumAdjustmentPeriodBS</returns>

        private PremiumAdjustmentPeriodBS prmAdjPerd;
        private PremiumAdjustmentPeriodBS PrmAdjPerd
        {
            get
            {
                if (prmAdjPerd == null)
                {
                    prmAdjPerd = new PremiumAdjustmentPeriodBS();
                }
                return prmAdjPerd;
            }
        }
        private AdjProcChklstBS adjSetupChklstItemService;
        private Qtly_Cntrl_ChklistBS qltyCntrlListService;
        /// <summary>
        /// a property for AdjustmentProcessing Checklist Service Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>AdjProcChklstBS</returns>
        private AdjProcChklstBS AdjSetupChklstItemService
        {
            get
            {
                if (adjSetupChklstItemService == null)
                {
                    adjSetupChklstItemService = new AdjProcChklstBS();

                }
                return adjSetupChklstItemService;
            }
            set
            {
                adjSetupChklstItemService = value;
            }
        }
        /// <summary>
        /// a property for Quality control Checklist Service Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>Qtly_Cntrl_ChklistBS</returns>
        private Qtly_Cntrl_ChklistBS QltyCntrlListService
        {
            get
            {
                if (qltyCntrlListService == null)
                {
                    qltyCntrlListService = new Qtly_Cntrl_ChklistBS();

                }
                return qltyCntrlListService;
            }
            set
            {
                qltyCntrlListService = value;
            }
        }
        /// <summary>
        /// a property for Quality control Checklist Enity Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>Qtly_Cntrl_ChklistBE</returns>
        private Qtly_Cntrl_ChklistBE QltyCntrlListBE
        {
            get
            {
                return ((Session["qltyCntrlListBE"] == null) ?
                    (new Qtly_Cntrl_ChklistBE()) : (Qtly_Cntrl_ChklistBE)Session["qltyCntrlListBE"]);
            }
            set
            {
                Session["qltyCntrlListBE"] = value;
            }

        }
        private IList<Qtly_Cntrl_ChklistBE> QltyCntrlListBEList
        {
            get
            {
                if (Session["QltyCntrlListBEList"] == null)
                    Session["QltyCntrlListBEList"] = new List<Qtly_Cntrl_ChklistBE>();
                return (IList<Qtly_Cntrl_ChklistBE>)Session["QltyCntrlListBEList"];
            }
            set { Session["QltyCntrlListBEList"] = value; }
        }
        private IList<Qtly_Cntrl_ChklistBE> QltyCntrlListBEListApp
        {
            get
            {
                if (Session["QltyCntrlListBEListApp"] == null)
                    Session["QltyCntrlListBEListApp"] = new List<Qtly_Cntrl_ChklistBE>();
                return (IList<Qtly_Cntrl_ChklistBE>)Session["QltyCntrlListBEListApp"];
            }
            set { Session["QltyCntrlListBEListApp"] = value; }
        }
        /// <summary>
        /// a property for Quality control Approved Invoice Checklist Enity Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>Qtly_Cntrl_ChklistBE</returns>
        private Qtly_Cntrl_ChklistBE QltyCntrlApprovedInvListBE
        {
            get
            {
                return ((Session["qltyCntrlApprovedInvListBE"] == null) ?
                    (new Qtly_Cntrl_ChklistBE()) : (Qtly_Cntrl_ChklistBE)Session["qltyCntrlApprovedInvListBE"]);
            }
            set
            {
                Session["qltyCntrlApprovedInvListBE"] = value;
            }

        }
        protected ArrayList QltyCntrlLstIDlist
        {
            get
            {
                if (ViewState["QltyCntrlChklstID"] != null)
                {
                    return (ArrayList)ViewState["QltyCntrlChklstID"];
                }
                else
                {
                    ViewState["QltyCntrlChklstID"] = null;
                    return null;
                }
            }
            set
            {
                ViewState["QltyCntrlChklstID"] = value;
            }
        }

        protected ArrayList QltyCntrlApprovedInvLstIDlist
        {
            get
            {
                if (ViewState["QltyCntrlApprovedInvChklstID"] != null)
                {
                    return (ArrayList)ViewState["QltyCntrlApprovedInvChklstID"];
                }
                else
                {
                    ViewState["QltyCntrlApprovedInvChklstID"] = null;
                    return null;
                }
            }
            set
            {
                ViewState["QltyCntrlApprovedInvChklstID"] = value;
            }
        }
        #endregion
        public  bool ChkBind = true;
        public bool ChkBindApp = true;
        /// <summary>
        /// Page load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ChkBind = true;
            ChkBindApp = true;
            this.ucSaveCancel.OperationsButtonClicked += new EventHandler(btnSaveCancel_OperationsButtonClicked);
            //User control event
            this.ARS.AdjustmentReviewSearchButtonClicked += new EventHandler(btnSearch_AdjustmentReviewSearchButtonClicked);
            this.ARS.ARProgramPeriodSelectedIndexChanged += new EventHandler(ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged);
            this.Master.Page.Title = "AdjustProcessingChklst";
            if (!IsPostBack)
            {
                this.ucSaveCancel.Controls[0].FindControl("cmdCancel").Visible = false;

                //BindAdjProcChklstListView();
                //BindLRFPosting();
                //prmAdjLRFBS = new PremiumAdjLRFPostingBS();
                try
                {
                    //verification for QueryString to implement persistance
                    if (Request.QueryString["SelectedValues"] != null && Request.QueryString["SelectedValues"] != "")
                    {
                        string[] strSelectedValues = Request.QueryString["SelectedValues"].ToString().Split(';');
                        string strSelectedAccountID = strSelectedValues[0].ToString();
                        string strSelectedValDate = strSelectedValues[1].ToString();
                        string strSelectedPremAdjID = strSelectedValues[2].ToString();
                        string strSelectedProgramPeriod = strSelectedValues[3].ToString();
                        if (strSelectedAccountID != "" && strSelectedValDate != "" && strSelectedPremAdjID != "" && strSelectedProgramPeriod != "")
                            fillSelectedValues(strSelectedAccountID, strSelectedValDate, strSelectedProgramPeriod, strSelectedPremAdjID);
                        else
                            hidSelectedValues.Value = strSelectedAccountID + ";" + strSelectedValDate + ";" + strSelectedPremAdjID + ";" + strSelectedProgramPeriod;
                    }
                }
                catch (Exception ee)
                {
                    ShowError(ee.Message,ee);
                    return;
                }

            }

        }
        #endregion

        /// <summary>
        /// Method to Load the selected search Criteria while navingating from other tab
        /// </summary>
        /// <param name="strSelectedAccountID"></param>
        /// <param name="strSelectedValDate"></param>
        /// <param name="strSelectedProgramPeriod"></param>
        /// <param name="strSelectedPremAdjID"></param>
        #region fillSelectedValues
        private void fillSelectedValues(string strSelectedAccountID, string strSelectedValDate, string strSelectedProgramPeriod, string strSelectedPremAdjID)
        {


            int intAccountID = 0;
            if (strSelectedAccountID != "")
                intAccountID = Convert.ToInt32(strSelectedAccountID);
            int intPremAdjID = 0;
            if (strSelectedPremAdjID != "")
                intPremAdjID = Convert.ToInt32(strSelectedPremAdjID);
            DateTime dtValDate = new DateTime();
            if (strSelectedValDate != "")
                dtValDate = Convert.ToDateTime(strSelectedValDate);
            string strProgramPeriod = strSelectedProgramPeriod;
            int intPremAdjPerdID = 0;
            int intPrgPeriodID = 0;
            if (strSelectedProgramPeriod != "")
                intPrgPeriodID = Convert.ToInt32(strSelectedProgramPeriod);
            
            IList<PremiumAdjustmentPeriodBE> premAdjPerdBE = new List<PremiumAdjustmentPeriodBE>();
            premAdjPerdBE = PrmAdjPerd.getPremAdjPerdID(intAccountID, intPremAdjID, intPrgPeriodID);
            if (premAdjPerdBE.Count > 0)
            {
                intPremAdjPerdID = premAdjPerdBE[premAdjPerdBE.Count - 1].PREM_ADJ_PERD_ID;
            }
            //Retrieving the Status Type ID of the adjustment Selected
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPremAdjID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            this.AccountID = intAccountID;
            this.PreAdjID = intPremAdjID;
            this.PrgPeriodID = intPrgPeriodID;
            this.PremAdjPerdID = intPremAdjPerdID;
            this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";" + strProgramPeriod;
            //Restricting User to Add or Update if Adjustment Status is not CALC
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            Button btnSave = (Button)ucSaveCancel.FindControl("cmdSave");
            if (intAdjCalStatusID != AdjStatusTypeID)
            {

                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            //lblAdjProcChklst.Visible = true;
//            if (TabContainerAdjChecklist.ActiveTabIndex == 0)
//            {
                pnlAdjChklst.Visible = true;
                BindAdjProcChklstListView();
//            }
//            else if (TabContainerAdjChecklist.ActiveTabIndex == 1)
//            {
                PnlApprovedInvChklist.Visible = true;
                BindApprovedInvChklstListView();
//            }


        }
        #endregion

        /// <summary>
        /// Listview Data Binding
        /// </summary>
        #region BindApprovedInvChklstListView
        private void BindApprovedInvChklstListView()
        {
            //New code
             //DropDownList drplist = new DropDownList();
            //End
            adjSetupChklstItemService = new AdjProcChklstBS();
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == "Adjustment Approved Invoice to Underwriting Checklist").ToList();

            IList<Qtly_Cntrl_ChklistBE> adjProcChklstItem = new List<Qtly_Cntrl_ChklistBE>();
            //adjProcChklstItem = adjSetupChklstItemService.getRelatedChklstItems(lookups[0].LookUpID, Convert.ToInt32(AISMasterEntities.AdjusmentNumber), AISMasterEntities.AccountNumber);
            adjProcChklstItem = adjSetupChklstItemService.getRelatedChklstItems(lookups[0].LookUpID, this.PreAdjID, this.AccountID, this.PrgPeriodID);
            if (adjProcChklstItem.Count == 0)
            {
                //Concurrency
                QltyCntrlListBEListApp = adjSetupChklstItemService.getAllApprovedInvChklstItems();
                //End
                this.lstApprovedInvChklist.DataSource = QltyCntrlListBEListApp;// adjSetupChklstItemService.getAllApprovedInvChklstItems();
                lstApprovedInvChklist.DataBind();
            }
            else
            {
                //Concurrency
                QltyCntrlListBEListApp = adjProcChklstItem;
                //End
                ViewState["PrmAdjID"] = adjProcChklstItem[0].PREMIUMADJUSTMENT_ID.ToString();
                ViewState["PrmAdjPrdID"] = adjProcChklstItem[0].PremumAdj_Pgm_ID.ToString();
                ViewState["QltyCntrlApprovedInvChklstID"] = adjProcChklstItem[0].QualityControlChklst_ID.ToString();
                this.lstApprovedInvChklist.DataSource = QltyCntrlListBEListApp;// adjProcChklstItem;
                lstApprovedInvChklist.DataBind();
                IList<Qtly_Cntrl_ChklistBE> lstAdjSetProc = QltyCntrlListBEListApp;// adjProcChklstItem;
                if (lstAdjSetProc.Count > 0)
                {

                    ArrayList alQltyCntrlLstIDs = new ArrayList();
                    for (int row = 0; row < lstAdjSetProc.Count; row++)
                    {
                        //New code
                        //drplist = (DropDownList)lstApprovedInvChklist.Items[row].FindControl("ddlSelectAcctProc");
                        //drplist.Text = lstAdjSetProc[row].CHKLIST_STS_CD;
                        //End
                        alQltyCntrlLstIDs.Add(lstAdjSetProc[row].QualityControlChklst_ID);
                    }

                    this.QltyCntrlApprovedInvLstIDlist = alQltyCntrlLstIDs;

                }
            }
        }
        #endregion

        /// <summary>
        /// Tab changed Event
        /// Fires when tab changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region TabContainer1_ActiveTabChanged
        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainerAdjChecklist.ActiveTabIndex == 0)
            {
                if (this.PreAdjID != 0)
                {
                    pnlAdjChklst.Visible = true;
                    BindAdjProcChklstListView();
                }
            }
            else if (TabContainerAdjChecklist.ActiveTabIndex == 1)
            {
                if (this.PreAdjID != 0)
                {
                    PnlApprovedInvChklist.Visible = true;
                    BindApprovedInvChklstListView();
                }
            }
        }
        #endregion

        /// <summary>
        /// Onselectedindexchanged event for the user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged
        void ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
            DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
            DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
            DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
            if (ddlAccnts.SelectedIndex != 0)
            {
                hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ";" + ";";

            }
            else
            {
                hidSelectedValues.Value = "";
            }
            if (ddlValDates.SelectedIndex != 0)
            {
                hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ";";

            }
            else if (ddlAccnts.SelectedIndex != 0)
            {
                hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ";" + ";";

            }
            if (ddlAdjNumber.SelectedIndex != 0)
            {
                hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
            }
            else if (ddlValDates.SelectedIndex != 0)
            {
                hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ";";
            }
            if (ddlPrgPeriod.SelectedIndex != 0)
            {
                hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";" + ddlPrgPeriod.SelectedValue;
            }
            else if (ddlAdjNumber.SelectedIndex != 0)
            {
                hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
            }

            pnlAdjChklst.Visible = false;
            PnlApprovedInvChklist.Visible = false;
        }
        #endregion
        /// <summary>
        /// Search Button Click event for the user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Search Button Click Event
        void btnSearch_AdjustmentReviewSearchButtonClicked(object sender, EventArgs e)
        {
            ChkBind = true;
            ChkBindApp = true;
            ViewState["QltyCntrlChklstID"] = null;
            ViewState["QltyCntrlApprovedInvChklstID"] = null;
            DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
            DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
            DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
            DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
            if (ddlAccnts.SelectedIndex == 0)
            {
                string strMessage = "Please select Account name";
                ShowError(strMessage);
                return;
            }
            if (ddlValDates.SelectedIndex == 0)
            {
                string strMessage = "Please select Valuation Date";
                ShowError(strMessage);
                return;
            }
            if (ddlAdjNumber.SelectedIndex == 0)
            {
                string strMessage = "Please select Adjustment Number";
                ShowError(strMessage);
                return;
            }
            if (ddlPrgPeriod.SelectedIndex == 0)
            {
                string strMessage = "Please select Program Period";
                ShowError(strMessage);
                return;
            }
            
            int intAccountID = Convert.ToInt32(ddlAccnts.SelectedValue);
            int intPremAdjID = Convert.ToInt32(ddlAdjNumber.SelectedValue);
            DateTime dtValDate = Convert.ToDateTime(ddlValDates.SelectedValue.ToString());
            string strProgramPeriod = ddlPrgPeriod.SelectedValue.ToString();
            
            int intPremAdjPerdID = 0;
            int intPrgPeriodID = Convert.ToInt32(strProgramPeriod);
            
            IList<PremiumAdjustmentPeriodBE> premAdjPerdBE = new List<PremiumAdjustmentPeriodBE>();
            premAdjPerdBE = PrmAdjPerd.getPremAdjPerdID(intAccountID, intPremAdjID, intPrgPeriodID);
            if (premAdjPerdBE.Count > 0)
            {
                intPremAdjPerdID = premAdjPerdBE[premAdjPerdBE.Count - 1].PREM_ADJ_PERD_ID;
            }
            //Retrieving the Status Type ID of the adjustment Selected
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPremAdjID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            this.AccountID = intAccountID;
            this.PreAdjID = intPremAdjID;
            this.PrgPeriodID = intPrgPeriodID;
            this.PremAdjPerdID = intPremAdjPerdID;
            this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";" + strProgramPeriod;
            //Restricting User to Add or Update if Adjustment Status is not CALC
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            Button btnSave = (Button)ucSaveCancel.FindControl("cmdSave");
            if (intAdjCalStatusID != AdjStatusTypeID)
            {
               
               btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            //lblAdjProcChklst.Visible = true;
//             if (TabContainerAdjChecklist.ActiveTabIndex == 0)
//            {
                pnlAdjChklst.Visible = true;
                BindAdjProcChklstListView();
//            }
//            else if (TabContainerAdjChecklist.ActiveTabIndex == 1)
//            {
                PnlApprovedInvChklist.Visible = true;
                BindApprovedInvChklstListView();
//            }

        }
        #endregion
        /// <summary>
        /// Funtion to Bind the lstAdjustProcChklst ListView 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        #region BindAdjProcChklstListView
        private void BindAdjProcChklstListView()
        {
            //New code
            DropDownList drplist = new DropDownList();
            //End
            adjSetupChklstItemService = new AdjProcChklstBS();
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == "Adjustment Processing Checklist").ToList();

            IList<Qtly_Cntrl_ChklistBE> adjProcChklstItem = new List<Qtly_Cntrl_ChklistBE>();
            //adjProcChklstItem = adjSetupChklstItemService.getRelatedChklstItems(lookups[0].LookUpID, Convert.ToInt32(AISMasterEntities.AdjusmentNumber), AISMasterEntities.AccountNumber);
            adjProcChklstItem = adjSetupChklstItemService.getRelatedChklstItems(lookups[0].LookUpID, this.PreAdjID, this.AccountID, this.PrgPeriodID);
            
            if (adjProcChklstItem.Count == 0)
            {
                //Concurrency
                QltyCntrlListBEList = adjSetupChklstItemService.getAllAdjChklstItems();
                //End
                this.lstAdjProcChklst.DataSource = QltyCntrlListBEList;// adjSetupChklstItemService.getAllAdjChklstItems();
                lstAdjProcChklst.DataBind();
            }
            else
            {
                //Concurrency
                QltyCntrlListBEList = adjProcChklstItem;
                //End
                ViewState["PrmAdjID"] = adjProcChklstItem[0].PREMIUMADJUSTMENT_ID.ToString();
                ViewState["PrmAdjPrdID"] = adjProcChklstItem[0].PremumAdj_Pgm_ID.ToString();
                ViewState["QltyCntrlChklstID"] = adjProcChklstItem[0].QualityControlChklst_ID.ToString();
                this.lstAdjProcChklst.DataSource = QltyCntrlListBEList;// adjProcChklstItem;
                lstAdjProcChklst.DataBind();
                IList<Qtly_Cntrl_ChklistBE> lstAdjSetProc = QltyCntrlListBEList;// adjProcChklstItem;
                if (lstAdjSetProc.Count > 0)
                {

                    ArrayList alQltyCntrlLstIDs = new ArrayList();
                    for (int row = 0; row < lstAdjSetProc.Count; row++)
                    {
                        //New code
                       drplist = (DropDownList)lstAdjProcChklst.Items[row].FindControl("ddlSelectAcctProc");
                       if (lstAdjSetProc[row].CHKLIST_STS_CD == null ||
                           lstAdjSetProc[row].CHKLIST_STS_CD.Trim().Length == 0)
                           drplist.Text = "No";

                        drplist.Text = lstAdjSetProc[row].CHKLIST_STS_CD;
                        //End
                        alQltyCntrlLstIDs.Add(lstAdjSetProc[row].QualityControlChklst_ID);
                    }

                    this.QltyCntrlLstIDlist = alQltyCntrlLstIDs;

                }
            }



        }
        #endregion

        /// <summary>
        /// Funtion for updating the values into QltyCntrlList Table 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        #region UpdateQltyCntrlList
        private void UpdateQltyCntrlList()
        {
           
            ArrayList alQltyCntrlLstIDs = this.QltyCntrlLstIDlist;
            int intQltyCntrlListID = 0;
            //Concurrency chk
            adjSetupChklstItemService = new AdjProcChklstBS();
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == "Adjustment Processing Checklist").ToList();
            IList<Qtly_Cntrl_ChklistBE> adjProcChklstItem = new List<Qtly_Cntrl_ChklistBE>();
            adjProcChklstItem = adjSetupChklstItemService.getRelatedChklstItems(lookups[0].LookUpID, this.PreAdjID, this.AccountID, this.PrgPeriodID);

            if (adjProcChklstItem.Count > 0)
            {
                if (adjProcChklstItem[0].CHKLIST_STS_CD != null && this.QltyCntrlLstIDlist[0].ToString() == "0")
                {
                    ChkBind = false;
                    ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                    return;
                }
            }
            //End
            if (alQltyCntrlLstIDs != null)
            {
                Qtly_Cntrl_ChklistBE QltyCntrlListBE = null;
                for (int i = 0; i < lstAdjProcChklst.Items.Count; i++)
                {
                    intQltyCntrlListID = Convert.ToInt32(alQltyCntrlLstIDs[i].ToString());
                                      
                    QltyCntrlListBE = adjSetupChklstItemService.getQltyCntrlRow(intQltyCntrlListID);
                    if (QltyCntrlListBE.IsNull())
                    {
                        
                        QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
                        DropDownList drplist = new DropDownList();
                        drplist = (DropDownList)lstAdjProcChklst.Items[i].FindControl("ddlSelectAcctProc");

                        if (drplist.Text == "0")
                        {
                            QltyCntrlListBE.CHKLIST_STS_CD = "No ";
                        }
                        else
                        {
                            QltyCntrlListBE.CHKLIST_STS_CD = drplist.Text;
                        }
                        //Need to remove below code once implemented in reports
                        if (drplist.Text == "Yes")
                            QltyCntrlListBE.ACTIVE = true;
                        else
                            QltyCntrlListBE.ACTIVE = false;
                        //End
                        Label lblActIndx = (Label)lstAdjProcChklst.Items[i].FindControl("lblhidQualitycntrl");
                        QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);

                        QltyCntrlListBE.PremumAdj_Pgm_ID = PrgPeriodID;
                        QltyCntrlListBE.PREMIUMADJUSTMENT_ID = this.PreAdjID;
                        QltyCntrlListBE.CUSTOMER_ID = this.AccountID;
                        QltyCntrlListBE.PremumAdj_Pgm_ID = this.PrgPeriodID;
                        QltyCntrlListBE.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                        QltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
                        bool Flag = (new AdjProcChklstBS()).Update(QltyCntrlListBE);
                    }
                    else
                    {
                        //Concurrency Issue
                        if (QltyCntrlListBEList.Count > 0)
                        {
                            Qtly_Cntrl_ChklistBE QltyCntrlListBEOld = QltyCntrlListBEList[i];
                            bool con = ShowConcurrentConflict(Convert.ToDateTime(QltyCntrlListBEOld.UpdatedDate), Convert.ToDateTime(QltyCntrlListBE.UpdatedDate));
                            if (!con)
                            {
                                ChkBind = false;
                                break;
                            }
                        }
                        //End
                        //old code
                        //CheckBox chk = new CheckBox();
                        //chk = (CheckBox)lstAdjProcChklst.Items[i].FindControl("chkSelectAdjProc");
                        //QltyCntrlListBE.ACTIVE = chk.Checked;
                        //End

                        //New code
                        DropDownList drplist = new DropDownList();
                        drplist = (DropDownList)lstAdjProcChklst.Items[i].FindControl("ddlSelectAcctProc");

                        if (drplist.Text == "0")
                        {
                            QltyCntrlListBE.CHKLIST_STS_CD = "No ";
                        }
                        else
                        {
                            QltyCntrlListBE.CHKLIST_STS_CD = drplist.Text;
                        }
                        //Need to remove below code once implemented in reports
                        if (drplist.Text == "Yes")
                            QltyCntrlListBE.ACTIVE = true;
                        else
                            QltyCntrlListBE.ACTIVE = false;
                        //End
                        Label lblActIndx = (Label)lstAdjProcChklst.Items[i].FindControl("lblhidQualitycntrl");

                        QltyCntrlListBE.PremumAdj_Pgm_ID = PrgPeriodID;
                        QltyCntrlListBE.PREMIUMADJUSTMENT_ID = Convert.ToInt32(ViewState["PrmAdjID"]);
                        QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);

                        QltyCntrlListBE.UpdatedDate = DateTime.Now;
                        QltyCntrlListBE.UpdatedUserID = CurrentAISUser.PersonID;
                        bool Flag = adjSetupChklstItemService.Update(QltyCntrlListBE);
                        ShowConcurrentConflict(Flag, QltyCntrlListBE.ErrorMessage);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Funtion to save AdjustProcessing Chklist Items into Qlty_Cntrl_List Table
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        #region AdjProcChklstSave
        void AdjProcChklstSave()
        {
            adjSetupChklstItemService = new AdjProcChklstBS();

            if (ViewState["QltyCntrlChklstID"] == null)
            {
                Qtly_Cntrl_ChklistBE QltyCntrlListBE;
                for (int i = 0; i < lstAdjProcChklst.Items.Count; i++)
                {
                    QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
                    //Old Code
                    //CheckBox chk = new CheckBox();
                    //chk = (CheckBox)lstAdjProcChklst.Items[i].FindControl("chkSelectAdjProc");
                    //QltyCntrlListBE.ACTIVE = chk.Checked;
                    //End
                    //New code
                    DropDownList drplist = new DropDownList();
                    drplist = (DropDownList)lstAdjProcChklst.Items[i].FindControl("ddlSelectAcctProc");
                    if(drplist.Text.Trim().ToUpper() == "(SELECT)" || drplist.Text == "0")
                        QltyCntrlListBE.CHKLIST_STS_CD = "No";
                    else
                        QltyCntrlListBE.CHKLIST_STS_CD = drplist.Text;
                   
                    //Need to remove below code once implemented in reports
                    if (drplist.Text == "Yes")
                        QltyCntrlListBE.ACTIVE = true;
                    else
                        QltyCntrlListBE.ACTIVE = false;
                    //End
                    Label lblActIndx = (Label)lstAdjProcChklst.Items[i].FindControl("lblhidQualitycntrl");
                    QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);

                    QltyCntrlListBE.PremumAdj_Pgm_ID = PrgPeriodID;
                    QltyCntrlListBE.PREMIUMADJUSTMENT_ID = this.PreAdjID;
                    QltyCntrlListBE.CUSTOMER_ID = this.AccountID;
                    QltyCntrlListBE.PremumAdj_Pgm_ID = this.PrgPeriodID;
                    QltyCntrlListBE.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    QltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
                    bool Flag = adjSetupChklstItemService.Update(QltyCntrlListBE);
                }

            }
            else
            {
                UpdateQltyCntrlList();
            }
            if(ChkBind)
            BindAdjProcChklstListView();
        }
        #endregion

        /// <summary>
        /// Funtion for updating the values into QltyCntrlList Table 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        #region UpdateApprovedInvQltyCntrlList
        private void UpdateApprovedInvQltyCntrlList()
        {
            ArrayList alQltyCntrlLstIDs = this.QltyCntrlApprovedInvLstIDlist;
            int intQltyCntrlListID = 0;
            //Concurrency chk
            adjSetupChklstItemService = new AdjProcChklstBS();
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == "Adjustment Approved Invoice to Underwriting Checklist").ToList();
            IList<Qtly_Cntrl_ChklistBE> adjProcChklstItem = new List<Qtly_Cntrl_ChklistBE>();
            adjProcChklstItem = adjSetupChklstItemService.getRelatedChklstItems(lookups[0].LookUpID, this.PreAdjID, this.AccountID, this.PrgPeriodID);

            if (adjProcChklstItem.Count > 0)
            {
                if (adjProcChklstItem[0].CHKLIST_STS_CD != null && this.QltyCntrlApprovedInvLstIDlist[0].ToString() == "0")
                {
                    ChkBindApp = false;
                    ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                    return;
                }
            }
            //End
            if (alQltyCntrlLstIDs != null)
            {

                Qtly_Cntrl_ChklistBE QltyCntrlListBE=null;
                for (int i = 0; i < lstApprovedInvChklist.Items.Count; i++)
                {
                    intQltyCntrlListID = Convert.ToInt32(alQltyCntrlLstIDs[i].ToString());

                    QltyCntrlListBE = adjSetupChklstItemService.getQltyCntrlRow(intQltyCntrlListID);
                    if (QltyCntrlListBE.IsNull())
                    {
                        QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
                        //old Code
                        CheckBox chk = new CheckBox();
                        chk = (CheckBox)lstApprovedInvChklist.Items[i].FindControl("chkSelectAdjProc");
                        QltyCntrlListBE.ACTIVE = chk.Checked;
                        if (chk.Checked)
                        {
                            QltyCntrlListBE.CHKLIST_STS_CD = "Yes";
                        }
                        else
                        {
                            QltyCntrlListBE.CHKLIST_STS_CD = "No";
                        }
                        //End
                        //New code
                        //DropDownList drplist = new DropDownList();
                        //drplist = (DropDownList)lstApprovedInvChklist.Items[i].FindControl("ddlSelectAcctProc");
                        //QltyCntrlListBE.CHKLIST_STS_CD = drplist.Text;
                        ////Need to remove below code once implemented in reports
                        //if (drplist.Text == "Yes")
                        //    QltyCntrlListBE.ACTIVE = true;
                        //else
                        //    QltyCntrlListBE.ACTIVE = false;
                        //End
                        Label lblActIndx = (Label)lstApprovedInvChklist.Items[i].FindControl("lblhidQualitycntrl");
                        QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);

                        QltyCntrlListBE.PremumAdj_Pgm_ID = PrgPeriodID;
                        QltyCntrlListBE.PREMIUMADJUSTMENT_ID = this.PreAdjID;
                        QltyCntrlListBE.CUSTOMER_ID = this.AccountID;
                        QltyCntrlListBE.PremumAdj_Pgm_ID = this.PrgPeriodID;
                        QltyCntrlListBE.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                        QltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
                        bool Flag = (new AdjProcChklstBS()).Update(QltyCntrlListBE);
                    }
                    else
                    {
                        //Concurrency Issue
                        if (QltyCntrlListBEListApp.Count > 0)
                        {
                            Qtly_Cntrl_ChklistBE QltyCntrlListBEOld = QltyCntrlListBEListApp[i];
                            bool con = ShowConcurrentConflict(Convert.ToDateTime(QltyCntrlListBEOld.UpdatedDate), Convert.ToDateTime(QltyCntrlListBE.UpdatedDate));
                            if (!con)
                            {
                                ChkBindApp = false;
                                break;
                            }
                        }
                        //End
                        //Old Code
                        CheckBox chk = new CheckBox();
                        chk = (CheckBox)lstApprovedInvChklist.Items[i].FindControl("chkSelectAdjProc");
                        QltyCntrlListBE.ACTIVE = chk.Checked;
                        //End
                        //New code
                        //DropDownList drplist = new DropDownList();
                        //drplist = (DropDownList)lstApprovedInvChklist.Items[i].FindControl("ddlSelectAcctProc");
                        //QltyCntrlListBE.CHKLIST_STS_CD = drplist.Text;
                        ////Need to remove below code once implemented in reports
                        //if (drplist.Text == "Yes")
                        //    QltyCntrlListBE.ACTIVE = true;
                        //else
                        //    QltyCntrlListBE.ACTIVE = false;
                        //End
                        Label lblActIndx = (Label)lstApprovedInvChklist.Items[i].FindControl("lblhidQualitycntrl");

                    QltyCntrlListBE.PremumAdj_Pgm_ID = PrgPeriodID;
                    QltyCntrlListBE.PREMIUMADJUSTMENT_ID = Convert.ToInt32(ViewState["PrmAdjID"]);
                    QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);
                    
                    QltyCntrlListBE.UpdatedDate = DateTime.Now;
                    QltyCntrlListBE.UpdatedUserID = CurrentAISUser.PersonID;
                    bool Flag = adjSetupChklstItemService.Update(QltyCntrlListBE);
                    ShowConcurrentConflict(Flag, QltyCntrlListBE.ErrorMessage);
                   
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Funtion to save Approved Invoice Chklist Items into Qlty_Cntrl_List Table
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        #region ApprovedInvChklstSave
        void ApprovedInvChklstSave()
        {
            adjSetupChklstItemService = new AdjProcChklstBS();

            if (ViewState["QltyCntrlApprovedInvChklstID"] == null)
            {
                Qtly_Cntrl_ChklistBE QltyCntrlListBE;
                for (int i = 0; i < lstApprovedInvChklist.Items.Count; i++)
                {
                    QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
                    //old Code
                    CheckBox chk = new CheckBox();
                    chk = (CheckBox)lstApprovedInvChklist.Items[i].FindControl("chkSelectAdjProc");
                    QltyCntrlListBE.ACTIVE = chk.Checked;
                    //End
                    //New code
                    //DropDownList drplist = new DropDownList();
                    //drplist = (DropDownList)lstApprovedInvChklist.Items[i].FindControl("ddlSelectAcctProc");
                    //QltyCntrlListBE.CHKLIST_STS_CD = drplist.Text;
                    ////Need to remove below code once implemented in reports
                    //if (drplist.Text == "Yes")
                    //    QltyCntrlListBE.ACTIVE = true;
                    //else
                    //    QltyCntrlListBE.ACTIVE = false;
                    //End
                    Label lblActIndx = (Label)lstApprovedInvChklist.Items[i].FindControl("lblhidQualitycntrl");
                    QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);

                    QltyCntrlListBE.PremumAdj_Pgm_ID = PrgPeriodID;
                    QltyCntrlListBE.PREMIUMADJUSTMENT_ID = this.PreAdjID;
                    QltyCntrlListBE.CUSTOMER_ID = this.AccountID;
                    QltyCntrlListBE.PremumAdj_Pgm_ID = this.PrgPeriodID;
                    QltyCntrlListBE.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    QltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
                    bool Flag = adjSetupChklstItemService.Update(QltyCntrlListBE);
                }

            }
            else
            {
                UpdateApprovedInvQltyCntrlList();
            }
            if (ChkBindApp)
            BindApprovedInvChklstListView();
        }
        #endregion

        /// <summary>
        /// SaveCancel User control Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region btnSaveCancel_OperationsButtonClicked
        void btnSaveCancel_OperationsButtonClicked(object sender, EventArgs e)
        {
            string strResult = ucSaveCancel.Operation.ToString();

            if (strResult.ToUpper() == "SAVE")
            {
                if (TabContainerAdjChecklist.ActiveTabIndex == 0)
                {
                    AdjProcChklstSave();
                }
                else
                {
                    ApprovedInvChklstSave();
                }
            }
            else
            {
                if (TabContainerAdjChecklist.ActiveTabIndex == 0)
                {
                    BindAdjProcChklstListView();
                }
                else
                {
                    BindApprovedInvChklstListView();
                }
            }


        }
        #endregion



    }
    #endregion
}
