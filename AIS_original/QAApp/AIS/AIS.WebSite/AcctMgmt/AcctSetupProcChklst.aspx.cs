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
namespace AIS.WebSite.AcctMgmt
{
    public partial class AcctSetup_AcctSetupProcChklst : AISBasePage
    {
        private AcctSetupProcChklstBS acctSetupChklstItemService;
        private AcctSetupQCBS acctSetupQCService;

        private Qtly_Cntrl_ChklistBS qltyCntrlListService;

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
        /// a property for AccountSetupProcessingChecklist Service Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>AcctSetupProcChklstBS</returns>
        private AcctSetupProcChklstBS AcctSetupChklstItemService
        {
            get
            {
                if (acctSetupChklstItemService == null)
                {
                    acctSetupChklstItemService = new AcctSetupProcChklstBS();

                }
                return acctSetupChklstItemService;
            }
            set
            {
                acctSetupChklstItemService = value;
            }
        }
        /// <summary>
        /// a property for AccountSetupQC Service Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>AcctSetupQCBS</returns>
        private AcctSetupQCBS AcctSetupQCService
        {
            get
            {
                if (acctSetupQCService == null)
                {
                    acctSetupQCService = new AcctSetupQCBS();

                }
                return acctSetupQCService;
            }
            set
            {
                acctSetupQCService = value;
            }
        }/// <summary>
        /// a property for QualityControlChecklist Entity Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>Qtly_Cntrl_ChklistBE</returns>
        private Qtly_Cntrl_ChklistBE qltychklstBE
        {
            get
            {
                return ((Session["QLTYCHKLSTBE"] == null) ?
                    (new Qtly_Cntrl_ChklistBE()) : (Qtly_Cntrl_ChklistBE)Session["QLTYCHKLSTBE"]);
            }
            set
            {
                Session["QLTYCHKLSTBE"] = value;
            }

        }
        /// <summary>
        /// a property for Premium Adjustment Program Service Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>ProgramPeriodBE</returns>
        private ProgramPeriodBE premAdjPgmBE
        {
            get
            {
                return ((Session["PREMADJPGMBE"] == null) ?
                    (new ProgramPeriodBE()) : (ProgramPeriodBE)Session["PREMADJPGMBE"]);
            }

            set { Session["PREMADJPGMBE"] = value; }
        }

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentAISUser.PersonID <= 0)
            {
                pnlAccProc.Enabled = false;
                pnlAcctSetupQC.Enabled = false;
                pnlAcctSetupQCDetails.Enabled = false;
                ShowError("UserID/AZCorpID Not Registered. Please Register using Internal Contacts.");
                return;
            }
            this.ucSaveCancel.OperationsButtonClicked += new EventHandler(btnSaveCancel_OperationsButtonClicked);

            if (!IsPostBack)
            {
                premAdjPgmBE = new ProgramPeriodBE();
                this.Master.Page.Title = "AccountSetupProcChecklist";
                lblAcctSetupQCDetails.Visible = true;
                BindAcctSetupProcChklstListView();
                BindAcctSetupQC();
               
                this.ucSaveCancel.Controls[0].FindControl("cmdCancel").Visible= false;

            }
            if (hidTabindex.Value == "1")
            {
                lblAcctSetupProcChklst.Text = "Account Setup QC";
                UCSave.Style.Add(HtmlTextWriterStyle.Display, "none");
                
                
            }
            else
            {
                lblAcctSetupProcChklst.Text = "Account Setup Processing Check List";
                UCSave.Style.Add(HtmlTextWriterStyle.Display, "block");
            }

            //Checks Exiting without Save
            ArrayList list = new ArrayList();
            list.Add(txtComment);
            list.Add(txtQCDate);
            list.Add(btnSave);
            ProcessExitFlag(list);
        }



        /// <summary>
        /// Funtion to Bind the lstAcctSetupProcChklst ListView 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        private void BindAcctSetupProcChklstListView()
        {
            //New code
            Label lblchklstcd = new Label();
            DropDownList drplist = new DropDownList();
            //End
           
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == "Account Setup Processing Checklist").ToList();
            IList<Qtly_Cntrl_ChklistBE> accProcChklstItem = new List<Qtly_Cntrl_ChklistBE>();
            accProcChklstItem = AcctSetupChklstItemService.getRelatedChklstItems(lookups[0].LookUpID, Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID), AISMasterEntities.AccountNumber);
            if (accProcChklstItem.Count == 0)
            {

                this.lstAcctSetupProcChklst.DataSource = AcctSetupChklstItemService.getAllChklstItems();
                lstAcctSetupProcChklst.DataBind();
            }
            else
            {
                ViewState["PrmAdjPrdID"] = accProcChklstItem[0].PremumAdj_Pgm_ID.ToString();
                ViewState["QltyCntrlChklstID"] = accProcChklstItem[0].QualityControlChklst_ID.ToString();
                this.lstAcctSetupProcChklst.DataSource = accProcChklstItem;
                lstAcctSetupProcChklst.DataBind();
                IList<Qtly_Cntrl_ChklistBE> lstAcctSetPC = accProcChklstItem;
                if (lstAcctSetPC.Count > 0)
                {

                    ArrayList alQltyCntrlLstIDs = new ArrayList();
                    for (int row = 0; row < lstAcctSetPC.Count; row++)
                    {
                        //New code
                        lblchklstcd = (Label)lstAcctSetupProcChklst.Items[row].FindControl("lblchklistcd");
                        drplist = (DropDownList)lstAcctSetupProcChklst.Items[row].FindControl("ddlSelectAcctProc");
                        drplist.Text = (lblchklstcd.Text.Trim().Length == 0) ? "No" : lblchklstcd.Text;
                        //End
                        alQltyCntrlLstIDs.Add(lstAcctSetPC[row].QualityControlChklst_ID);
                    }

                    this.QltyCntrlLstIDlist = alQltyCntrlLstIDs;

                }
            }
        }
        /// <summary>
        /// Funtion for saving  new records(AcctSetupProcChklist Items) into  QualityControlChklst Table
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        void AcctSetupProcChklstSave()
        {
            AcctSetupChklstItemService = new AcctSetupProcChklstBS();
            if (ViewState["QltyCntrlChklstID"] == null)
            {
                Qtly_Cntrl_ChklistBE qltychklstBE;
                for (int i = 0; i < lstAcctSetupProcChklst.Items.Count; i++)
                {
                    qltychklstBE = new Qtly_Cntrl_ChklistBE();
                    //CheckBox chk = new CheckBox();
                    //old code need to remove
                    //chk = (CheckBox)lstAcctSetupProcChklst.Items[i].FindControl("chkSelectAcctProc");
                    //qltychklstBE.ACTIVE = chk.Checked;
                    //End
                    //New code
                    DropDownList drplist = new DropDownList();
                    drplist = (DropDownList)lstAcctSetupProcChklst.Items[i].FindControl("ddlSelectAcctProc");

                    if (drplist.Text.Trim().ToUpper() == "(SELECT)" || drplist.Text == "0")
                        qltychklstBE.CHKLIST_STS_CD = "No";
                    else
                        qltychklstBE.CHKLIST_STS_CD = drplist.Text;

                    //Need to remove below code once implemented in reports
                    if (drplist.Text == "Yes")
                        qltychklstBE.ACTIVE = true;
                    else
                        qltychklstBE.ACTIVE = false;
                    //End
                    Label lblActIndx = (Label)lstAcctSetupProcChklst.Items[i].FindControl("lblhidQualitycntrl");
                    qltychklstBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);

                    qltychklstBE.PremumAdj_Pgm_ID = Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID);
                    qltychklstBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;

                    qltychklstBE.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    qltychklstBE.CreatedUser_ID = CurrentAISUser.PersonID;
                    bool Flag = AcctSetupChklstItemService.Update(qltychklstBE);


                }
            }
            else
            {
                UpdateQltyCntrlList();

            }

            BindAcctSetupProcChklstListView();
        }
        /// <summary>
        /// Funtion for Updating ChecklistItems into  QualityControlChklst Table
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>

        private void UpdateQltyCntrlList()
        {
            ArrayList alQltyCntrlLstIDs = this.QltyCntrlLstIDlist;
            int intQltyCntrlListID = 0;

            if (alQltyCntrlLstIDs != null)
            {

                Qtly_Cntrl_ChklistBE qltychklstBE = null;
                for (int i = 0; i < lstAcctSetupProcChklst.Items.Count; i++)
                {
                    intQltyCntrlListID = Convert.ToInt32(alQltyCntrlLstIDs[i].ToString());
                    qltychklstBE = AcctSetupChklstItemService.getQltyCntrlRow(intQltyCntrlListID);
                    if (qltychklstBE.IsNull())
                    {
                        qltychklstBE = new Qtly_Cntrl_ChklistBE();
                        DropDownList drplist = new DropDownList();
                        drplist = (DropDownList)lstAcctSetupProcChklst.Items[i].FindControl("ddlSelectAcctProc");
                        if (drplist.Text == "0")
                        {
                            qltychklstBE.CHKLIST_STS_CD = "No ";
                        }
                        else
                        {
                            qltychklstBE.CHKLIST_STS_CD = drplist.Text;
                        }
                        //Need to remove below code once implemented in reports
                        if (drplist.Text == "Yes")
                            qltychklstBE.ACTIVE = true;
                        else
                            qltychklstBE.ACTIVE = false;
                        //End
                        Label lblActIndx = (Label)lstAcctSetupProcChklst.Items[i].FindControl("lblhidQualitycntrl");
                        qltychklstBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);

                        qltychklstBE.PremumAdj_Pgm_ID = Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID);
                        qltychklstBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;

                        qltychklstBE.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                        qltychklstBE.CreatedUser_ID = CurrentAISUser.PersonID;
                        
                        bool Flag = (new AcctSetupProcChklstBS()).Update(qltychklstBE);
                    }
                    else
                    {
                        //old code
                        //CheckBox chk = new CheckBox();
                        //chk = (CheckBox)lstAcctSetupProcChklst.Items[i].FindControl("chkSelectAcctProc");
                        //qltychklstBE.ACTIVE = chk.Checked;
                        //New code
                        DropDownList drplist = new DropDownList();
                        drplist = (DropDownList)lstAcctSetupProcChklst.Items[i].FindControl("ddlSelectAcctProc");
                        if (drplist.Text == "0")
                        {
                            qltychklstBE.CHKLIST_STS_CD = "No ";
                        }
                        else
                        {
                            qltychklstBE.CHKLIST_STS_CD = drplist.Text;
                        }
                        //Need to remove below code once implemented in reports
                        if (drplist.Text == "Yes")
                            qltychklstBE.ACTIVE = true;
                        else
                            qltychklstBE.ACTIVE = false;
                        //End
                        Label lblActIndx = (Label)lstAcctSetupProcChklst.Items[i].FindControl("lblhidQualitycntrl");

                        qltychklstBE.PremumAdj_Pgm_ID = Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID);
                        qltychklstBE.CHECKLISTITEM_ID = Convert.ToInt32(lblActIndx.Text);


                        qltychklstBE.UpdatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                        qltychklstBE.UpdatedUserID = CurrentAISUser.PersonID;
                        bool Flag = AcctSetupChklstItemService.Update(qltychklstBE);
                        ShowConcurrentConflict(Flag, qltychklstBE.ErrorMessage);
                    }
                }

            }
        }

        /// <summary>
        /// Funtion to Bind the all the Fields of AccountSetupQC page
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        private void BindAcctSetupQC()
        {
            lblQCBy.Text = "";
            txtComment.Text = "";
            txtQCDate.Text = "";
            IList<ProgramPeriodBE> premList = new List<ProgramPeriodBE>();

            premList = AcctSetupQCService.getRelatedPrmPrdInfo(Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID));
            /// Check if Program Premium Record exists
            if (premList.Count > 0)
            {
                // Check if QC was already performed
                if (premList[0].QUALITYCONTROL_PERSON_ID != null)
                {
                    PersonBE persBE = (new PersonBS()).getPersonRow(Convert.ToInt32(premList[0].QUALITYCONTROL_PERSON_ID));

                    if (persBE.PERSON_ID > 0)
                    {
                        lblQCBy.Text = persBE.SURNAME + "," + persBE.FORENAME;
                    }
                }
                else
                {
                    lblQCBy.Text = CurrentAISUser.FullName;
                }

                if (premList[0].QLTY_CMMNT_TXT != null)
                {
                    txtComment.Text = premList[0].QLTY_CMMNT_TXT.ToString();
                }

                if (premList[0].QLTY_CNTRL_DT.HasValue)
                {
                    txtQCDate.Text = premList[0].QLTY_CNTRL_DT.ToString();
                }

                if (premList[0].PREM_ADJ_PGM_ID > 0)
                {
                    ViewState["PremiumAdjustmentProgramID"] = premList[0].PREM_ADJ_PGM_ID.ToString();
                }
                BindAcctQCDetailsList();
            }
            else
            {
                lblQCBy.Text = CurrentAISUser.FullName;
            }


        }
        /// <summary>
        /// Funtion to Bind the lstAcctSetupQCDetails ListView 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        public void BindAcctQCDetailsList()
        {

            IList<LookupBE> lookups;
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == "Account Setup QC Issue").ToList();
            lstAcctSetupQCDetails.DataSource = QltyCntrlListService.getAccQtlychklistList(lookups[0].LookUpID, Convert.ToInt32(ViewState["PremiumAdjustmentProgramID"]), AISMasterEntities.AccountNumber);
            lstAcctSetupQCDetails.DataBind();
            if (lstAcctSetupQCDetails.InsertItemPosition != InsertItemPosition.None)
            {
                DropDownList ddl = (DropDownList)lstAcctSetupQCDetails.InsertItem.FindControl("ddlQCIssues");
                QCMasterIssueListBS issues = new QCMasterIssueListBS();
                ddl.DataSource = issues.getIssuesList(lookups[0].LookUpID);
                ddl.DataValueField = "QualityCntrlMstrIsslstID";
                ddl.DataTextField = "IssueText";
                ddl.DataBind();
                pnlAcctSetupQCDetails.Visible = true;
                ListItem li = new ListItem("Select", "0");
                ddl.Items.Insert(0, li);
            }

        }
        /// <summary>
        /// Function for saving new record into PremiumAdjustmentProgram Table
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // premAdjPgmBE = new ProgramPeriodBE();

            Saveqcdetails();
           
        }

        public void Saveqcdetails()
        {

            premAdjPgmBE = AcctSetupQCService.getPreAdjPgmRow(Convert.ToInt32(ViewState["PremiumAdjustmentProgramID"]));
            premAdjPgmBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            if (txtQCDate.Text != "")
            {
                premAdjPgmBE.QLTY_CNTRL_DT = DateTime.Parse(txtQCDate.Text);
            }

            premAdjPgmBE.QUALITYCONTROL_PERSON_ID = CurrentAISUser.PersonID;
            premAdjPgmBE.QLTY_CMMNT_TXT = txtComment.Text.ToString();
            premAdjPgmBE.UPDATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            premAdjPgmBE.UPDATE_USER_ID = CurrentAISUser.PersonID;



            bool Flag = AcctSetupQCService.Update(premAdjPgmBE);
            ShowConcurrentConflict(Flag, premAdjPgmBE.ErrorMessage);
            BindAcctSetupQC();
            lstAcctSetupQCDetails.Enabled = true;
        
        
        }
        /// <summary>
        /// Funtion For saving operation of ListView 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        void btnSaveCancel_OperationsButtonClicked(object sender, EventArgs e)
        {

            string strResult = ucSaveCancel.Operation.ToString();

            if (TabConAcctSetupProcChklst.ActiveTabIndex == 0)
            {

                if (strResult.ToUpper() == "SAVE")
                {
                    AcctSetupProcChklstSave();
                }
                else
                {
                    BindAcctSetupProcChklstListView();
                }
            }
            if (TabConAcctSetupProcChklst.ActiveTabIndex == 1)
            {
                // AcctSetupQCDetailSave();
            }
        }

        /// <summary>
        /// Invoked when any  command button is clicked for  ListView 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        protected void CommandList(Object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SAVE")
            {
               
                AcctSetupQCDetailsSaveList(e.Item);
                 Saveqcdetails();

            }

            else if (e.CommandName.ToUpper() == "DISABLE" || e.CommandName.ToUpper() == "ENABLE")
            {
                //Function call to Enable or Disable the Record
                
                DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true);
                Saveqcdetails();
            }


        }
        /// <summary>
        /// Funtion to Disable one rows of lstAcctSetupQCDetails ListView 
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        protected void DisableRow(int intQltyID, bool Flag)
        {
            try
            {

                qltychklstBE = new Qtly_Cntrl_ChklistBE();
                qltychklstBE = QltyCntrlListService.getQualityControlRow(intQltyID);
                qltychklstBE.ACTIVE = Flag;
                Flag = QltyCntrlListService.Update(qltychklstBE);
                ShowConcurrentConflict(Flag, qltychklstBE.ErrorMessage);
                if (Flag)
                {
                    //Function call to bind lstReviewDetails
                    BindAcctQCDetailsList();
                }

            }
            catch (RetroBaseException ee)
            {
                ShowError(ee.Message, ee);
            }
        }

        protected void InsertList(Object sender, ListViewInsertEventArgs e)
        {
            //lstLookupType.InsertItemPosition = InsertItemPosition.None;
        }



        /// <summary>
        /// Funtion for inserting new AcctSetupQC issues records into Qlty_Cntrl_List Table
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        protected void AcctSetupQCDetailsSaveList(ListViewItem e)
        {


            qltychklstBE = new Qtly_Cntrl_ChklistBE();
            qltychklstBE.PremumAdj_Pgm_ID = Convert.ToInt32(ViewState["PremiumAdjustmentProgramID"].ToString());
            qltychklstBE.CHECKLISTITEM_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlQCIssues")).SelectedItem.Value);


            qltychklstBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
            qltychklstBE.PREMIUMADJUSTMENT_ID = AISMasterEntities.AdjusmentNumber;
            qltychklstBE.ACTIVE = true;

            qltychklstBE.CreatedUser_ID = CurrentAISUser.PersonID;
            qltychklstBE.CreatedDate = DateTime.Now;
            bool Result = QltyCntrlListService.IsExistsAcctQCIssue(qltychklstBE.PremumAdj_Pgm_ID, qltychklstBE.CHECKLISTITEM_ID, qltychklstBE.CUSTOMER_ID, qltychklstBE.QualityControlChklst_ID);
            if (!Result)
            {
                bool Flag = QltyCntrlListService.Update(qltychklstBE);
                BindAcctQCDetailsList();
            }
            else
            {
                ShowMessage("The record cannot be saved. An identical record already exists.");
            }

        }

        /// <summary>
        /// Funtion for databound event  of lstAccountSetupQCDetails listview
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
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
            }
        }




    }
}
