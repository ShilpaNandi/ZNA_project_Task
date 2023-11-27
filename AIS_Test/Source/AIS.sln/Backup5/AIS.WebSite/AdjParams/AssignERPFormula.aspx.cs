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

namespace AIS.WebSite.AdjParams
{
    public partial class AssignERPFormula : AISBasePage
    {
        int? formulaID;
        AssignERPFormulaBS eRPformulaBS;
        /// <summary>
        /// property to hold an instance for Business Transaction Wrapper
        /// </summary>
        /// <param name=""></param>
        /// <returns>AISBusinessTransaction property</returns>
        protected AISBusinessTransaction ERPTransactionWrapper
        {
            get
            {
                //if ((AISBusinessTransaction)Session["ERPTransaction"] == null)
                //    Session["ERPTransaction"] = new AISBusinessTransaction();

                //return (AISBusinessTransaction)Session["ERPTransaction"];
                if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("ERPTransaction") == null)
                    SaveObjectToSessionUsingWindowName("ERPTransaction", new AISBusinessTransaction());

                return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("ERPTransaction");
            }
            set
            {
                //Session["ERPTransaction"] = value;
                SaveObjectToSessionUsingWindowName("ERPTransaction", value);
            }
        }


        /// <summary>
        /// a property for ERPFormula Service Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>ERPformulaBS</returns>
        private AssignERPFormulaBS ERPformulaBS
        {
            get
            {
                if (eRPformulaBS == null)
                {
                    eRPformulaBS = new AssignERPFormulaBS();
                    eRPformulaBS.AppTransactionWrapper = ERPTransactionWrapper;
                }
                return eRPformulaBS;
            }
            set
            {
                eRPformulaBS = value;
            }
        }

        /// <summary>
        /// a property for ErpFormula Entity Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>ERPformulaBE</returns>
        ProgramPeriodBE ERPformulaBE
        {
            get
            {
                //return ((Session["ERPFormulaBE"] == null) ?
                //    (new ProgramPeriodBE()) : (ProgramPeriodBE)Session["ERPFormulaBE"]);
                return ((RetrieveObjectFromSessionUsingWindowName("ERPFormulaBE") == null) ?
                   (new ProgramPeriodBE()) : (ProgramPeriodBE)RetrieveObjectFromSessionUsingWindowName("ERPFormulaBE"));
            }
            set
            {
                //Session["ERPFormulaBE"] = value;
                SaveObjectToSessionUsingWindowName("ERPFormulaBE", value);
            }
        }
        private ProgramPeriodBE ERPformulaBEOld
        {
            get 
            { 
                //return (ProgramPeriodBE)Session["ERPformulaBEOld"]; 
                return (ProgramPeriodBE)RetrieveObjectFromSessionUsingWindowName("ERPformulaBEOld"); 
            }
            set 
            { 
                //Session["ERPformulaBEOld"] = value; 
                SaveObjectToSessionUsingWindowName("ERPformulaBEOld", value);
            }
        }
        #region Properties


        #endregion
        protected override object SaveControlState()
        {

            int stateIdx = 0;
            object[] ctlState = new object[2];
            ctlState[stateIdx++] = base.SaveControlState();
            //ctlState[stateIdx++] = this.PrmPerdID;

            return ctlState;
        }
        #region Events
        /// <summary>
        /// PageInit Event code 
        /// </summary>
        /// <param name=""></param>
        ///<returns></returns>
        protected void Page_Init(object sender, EventArgs e)
        {
            this.ucProgramPeriod.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ucProgramPeriod_ProgramPeriodRowClicked);
            this.ucSaveCancel.OperationsButtonClicked += new EventHandler(btnSaveCancel_OperationsButtonClicked);

        }
        /// <summary>
        /// PageLoad Event code 
        /// </summary>
        /// <param name=""></param>
        ///<returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ucSaveCancel.Controls[0].FindControl("cmdCancel").Visible = false;

            if (!Page.IsPostBack)
            {
                //To display the title of the page
                this.Master.Page.Title = "Assign ERP Formula";
                this.ucSaveCancel.Controls[0].FindControl("cmdSave").Visible = false;
                try
                {
                    if (Request.QueryString["Flag"] != null)
                    {
                        AjaxControlToolkit.CollapsiblePanelExtender collaps = (AjaxControlToolkit.CollapsiblePanelExtender)UcMastervalues.FindControl("CollapseAccountHeader");
                        collaps.Collapsed = bool.Parse(Request.QueryString["Flag"].ToString());
                    }
                    ERPTransactionWrapper = new AISBusinessTransaction();
                    if (Request.QueryString["ProgPerdID"] != null)
                    {
                        hidProgPerdID.Value = Request.QueryString["ProgPerdID"].ToString();

                        SelectProgPeriod(Convert.ToInt32(hidProgPerdID.Value.ToString()));
                    }
                    //Logic to highlighted  Select the ProgramPeriod Line for cinsistancy setting the Public property of ProgramPeriod User control
                    ucProgramPeriod.SelectedProgramID = Convert.ToInt32(hidProgPerdID.Value);
                }
                catch (Exception ee)
                {
                }
            }

            //Checks Exiting Without Save
            ArrayList list = new ArrayList();
            list.Add(lnkClose);
            ProcessExitFlag(list);
            
        }
        /// <summary>
        ///  Databound Event for  lstErpFormula listview 
        /// </summary>
        /// <param name=""></param>
        ///<returns></returns>
        protected void lstErpFormula_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem index = (ListViewDataItem)e.Item;
                //CheckBox cb = (CheckBox)e.Item.FindControl("cbSelect");
                //cb.Attributes.Add("onclick", "javascript:getrowno('" + index.DataItemIndex + "','" + hidindex.ClientID + "')");

                string strScript = "uncheckOthers(" + ((CheckBox)e.Item.FindControl("cbSelect")).ClientID + ",'" + index.DataItemIndex + "','" + hidindex.ClientID + "');";
                ((CheckBox)e.Item.FindControl("cbSelect")).Attributes.Add("onclick", strScript);
            }

        }

        /// <summary>
        /// Function for Radio button checked changed event
        /// </summary>
        /// <param name=""></param>
        ///<returns></returns>
        //protected void rdbSelect_CheckedChanged(object sender, EventArgs e)
        //{
        //    lblAssignERPFormula.Visible = true;
        //    lnkClose.Visible = true;
        //    //ucSaveCancel.Visible = true;
        //    if (ViewState["INDEX"] != null)
        //    {
        //        RadioButton rdbFormula = (RadioButton)lstErpFormula.Items[int.Parse(ViewState["INDEX"].ToString())].FindControl("rdbSelect");
        //        rdbFormula.Checked = false;
        //    }
        //    ViewState["INDEX"] = hidindex.Value.ToString();

        //    string strCtlDirtyBit =
        //     "Javascript:document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value=1;";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "regDirtyBit", strCtlDirtyBit);

        //}
        protected void lnkClose_Click(object sender, EventArgs e)
        {
            lblAssignERPFormula.Visible = false;
            this.ucSaveCancel.Controls[0].FindControl("cmdSave").Visible = false;
            lnkClose.Visible = false;
            pnlAssignERPFormula.Visible = false;
            hidProgPerdID.Value = "0";
            ((ListView)ucProgramPeriod.FindControl("lstProgramPeriod")).Enabled = true;
        }



        #endregion
        #region  Methods

        /// <summary>
        /// Functions for Program period User control Row click event
        /// </summary>
        /// <param name=""></param>
        ///<returns></returns>
        void ucProgramPeriod_ProgramPeriodRowClicked(object sender, ListViewCommandEventArgs e)
        {
            hidProgPerdID.Value = e.CommandArgument.ToString();
            SelectProgPeriod(Convert.ToInt32(e.CommandArgument));

        }
        /// <summary>
        /// Function to Display when Program Period is selected
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        public void SelectProgPeriod(int ProgramPeriodID)
        {
            lblAssignERPFormula.Text = "Assign ERP Formula -- ";
            this.ucSaveCancel.Controls[0].FindControl("cmdSave").Visible = true;
            ProgramPeriodBE ProgPerdBE = ((new ProgramPeriodsBS()).getProgramPeriodRow(ProgramPeriodID));
            lblAssignERPFormula.Text += ProgPerdBE.PROGRAMPERIOD;
            ((ListView)ucProgramPeriod.FindControl("lstProgramPeriod")).Enabled = false;
            pnlAssignERPFormula.Visible = true;
            lnkClose.Visible = true;
            lblAssignERPFormula.Visible = true;
            int prgPrdID = ProgramPeriodID;
            //Session["PrmPrdID"] = prgPrdID.ToString();
            SaveObjectToSessionUsingWindowName("PrmPrdID", prgPrdID);
            ViewState["INDEX"] = null;
            //Function for binding the Assign ERP Formula listview
            BindFormulas(prgPrdID);
        }
        /// <summary>
        /// Function for binding the ERP Formulas to Assign ERP Formula listview
        /// </summary>
        /// <param name="progmID">Program period ID of selected row of the program period user Control</param>
        /// <returns>void</returns>
        private void BindFormulas(int progmID)
        {
            ERPformulaBE = new ProgramPeriodBE();
            ERPformulaBS = new AssignERPFormulaBS();
            ERPformulaBE = ERPformulaBS.getAssignERPRow(progmID);
            ERPformulaBEOld = ERPformulaBE;
            formulaID = ERPformulaBE.MSTR_ERND_RETRO_PREM_FRMLA_ID;
            ViewState["PROGPPID"] = progmID;
            IList<MasterERPFormulaBE> lstERPFormulas = new List<MasterERPFormulaBE>();

            MasterERPFormulaBS mefBS = new MasterERPFormulaBS();
            lstERPFormulas = mefBS.getERPFormulas();
            this.lstErpFormula.DataSource = lstERPFormulas;
            this.lstErpFormula.DataBind();


            Label lblactid;
            for (int i = 0; i < lstErpFormula.Items.Count; i++)
            {
                lblactid = (Label)lstErpFormula.Items[i].FindControl("lblhidFormulaID");
                if (lblactid.Text == formulaID.ToString())
                {
                    //RadioButton rdbselect = (RadioButton)lstErpFormula.Items[i].FindControl("rdbSelect");
                    CheckBox cbselect = (CheckBox)lstErpFormula.Items[i].FindControl("cbSelect");
                    cbselect.Checked = true;
                    ViewState["INDEX"] = i.ToString();
                    break;
                }
            }

        }
        /// <summary>
        /// Function for saving the ERP Formulas to selected Program period
        /// </summary>
        /// <param name=""></param>
        ///<returns></returns>
        void btnSaveCancel_OperationsButtonClicked(object sender, EventArgs e)
        {
            string strResult = ucSaveCancel.Operation.ToString();


            if (strResult.ToUpper() == "SAVE")
            {
                ERPformulaBE = ERPformulaBS.getAssignERPRow(int.Parse(ViewState["PROGPPID"].ToString()));
                bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(ERPformulaBEOld.UPDATE_DATE), Convert.ToDateTime(ERPformulaBE.UPDATE_DATE));
                    if (!Concurrency)
                        return;
                    
                //if (ViewState["INDEX"] != null)
                //{
                //    Label lblactid = (Label)lstErpFormula.Items[int.Parse(ViewState["INDEX"].ToString())].FindControl("lblhidFormulaID");
                //}

                Label lblhidFID = new Label();
                lblhidFID.Text = "";
                for (int i = 0; i < lstErpFormula.Items.Count; i++)
                {
                    CheckBox cbselect = (CheckBox)lstErpFormula.Items[i].FindControl("cbSelect");
                    if (cbselect.Checked)
                    {
                        lblhidFID = (Label)lstErpFormula.Items[i].FindControl("lblhidFormulaID");
                        ViewState["INDEX"] = i.ToString();
                        break;
                    }

                }
                if (lblhidFID.Text != "")
                {
                    ERPformulaBE.MSTR_ERND_RETRO_PREM_FRMLA_ID = Convert.ToInt32(lblhidFID.Text);
                }
                else
                {
                    ERPformulaBE.MSTR_ERND_RETRO_PREM_FRMLA_ID = null;
                }
                ERPformulaBE.UPDATE_DATE = DateTime.Now;
                ERPformulaBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                //Function for Audit Trail
                // bool ISUpdate = (ERPformulaBE.MSTR_ERND_RETRO_PREM_FRMLA_ID > 0);
                //bool auditNeeded = CheckAuditTrail(ERPformulaBE);

                //Function for Updating to Database
                bool Flag = ERPformulaBS.Update(ERPformulaBE);
                //ShowConcurrentConflict(Flag, ERPformulaBE.ErrorMessage);
                ERPformulaBEOld = ERPformulaBE;
                if (Flag)
                {
                    //if (auditNeeded && ISUpdate)
                    //{
                    try
                    {
                        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                        audtBS.AppTransactionWrapper = ERPTransactionWrapper;

                        audtBS.Save(AISMasterEntities.AccountNumber, ERPformulaBE.PREM_ADJ_PGM_ID,
                           GlobalConstants.AuditingWebPage.AssignERPFormula, CurrentAISUser.PersonID);
                        ERPTransactionWrapper.SubmitTransactionChanges();
                    }
                    catch (Exception ex)
                    {
                        ERPTransactionWrapper.RollbackChanges();
                        throw new RetroBaseException(ex.Message);
                    }
                    //}
                    //else
                    //{
                    //    ERPTransactionWrapper.SubmitTransactionChanges();
                    //}
                }
                else
                {
                    ERPTransactionWrapper.RollbackChanges();
                }
            }
            else
            {
                //BindFormulas(Convert.ToInt32(Session["PrmPrdID"].ToString()));
                BindFormulas(Convert.ToInt32(RetrieveObjectFromSessionUsingWindowName("PrmPrdID").ToString()));
            }



        }

        /// <summary>
        /// Function for checking the Audit Trail
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>

        private bool CheckAuditTrail(ProgramPeriodBE papBE)
        {
            bool auditNeeded = false;
            if (ViewState["INDEX"] != null)
            {
                Label lblactid =
                    (Label)lstErpFormula.Items[int.Parse(ViewState["INDEX"].ToString())].FindControl("lblhidFormulaID");

                auditNeeded = ERPformulaBE.MSTR_ERND_RETRO_PREM_FRMLA_ID != int.Parse(lblactid.Text);

                ERPformulaBE.MSTR_ERND_RETRO_PREM_FRMLA_ID = int.Parse(lblactid.Text);
            }
            return auditNeeded;

        }



        #endregion

    }
}