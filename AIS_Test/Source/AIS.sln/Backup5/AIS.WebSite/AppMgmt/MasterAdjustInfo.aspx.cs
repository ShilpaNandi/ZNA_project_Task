/*-----	Page:	MasterAdjustmentInfo
-----
-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	This page is used by the Admin.Only an Admin can save/edit records on this page.
-----                   The page consistes of KY-OR Setup tab,Deductible Taxes Tab,Master ERP formulas used in the AIS application.
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 2/27/09 (AS part of Retro Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               05/27/09	Zakir Hussain
-----				Code modified in Deductible Setup so that a record cannot be saved if either the Main/Sub values are missing in Posting Transaction Setup.

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
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.LSP.Framework;
public partial class AppMgmt_MasterAdjustInfo : AISBasePage
{
    private MasterERPFormulaBS ERPService;
    /// <summary>
    /// property to hold an instance for Business Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISBusinessTransaction property</returns>
    protected AISBusinessTransaction MasterERPTransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["MasterERPTransaction"] == null)
            //    Session["MasterERPTransaction"] = new AISBusinessTransaction();
            //return (AISBusinessTransaction)Session["MasterERPTransaction"];
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("MasterERPTransaction") == null)
                SaveObjectToSessionUsingWindowName("MasterERPTransaction", new AISBusinessTransaction());
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("MasterERPTransaction");
        }
        set
        {
            //Session["MasterERPTransaction"] = value;
            SaveObjectToSessionUsingWindowName("MasterERPTransaction", value);
        }
    }
    /// <summary>
    /// a property for MasterERPFormula Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>MasterERPFormulaBS</returns>
    private MasterERPFormulaBS MasterERPService
    {
        get
        {
            if (ERPService == null)
            {
                ERPService = new MasterERPFormulaBS();
                ERPService.AppTransactionWrapper = MasterERPTransactionWrapper;
            }
            return ERPService;
        }
    }
    /// <summary>
    /// a property for MasterERPFormula Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>MasterERPFormulaBE</returns>
    private MasterERPFormulaBE masterERPFormulaBE
    {
        //get { return (MasterERPFormulaBE)Session["MASRTER_ERP_FORMULA_BE"]; }
        //set { Session["MASRTER_ERP_FORMULA_BE"] = value; }
        get { return (MasterERPFormulaBE)RetrieveObjectFromSessionUsingWindowName("MASRTER_ERP_FORMULA_BE"); }
        set { SaveObjectToSessionUsingWindowName("MASRTER_ERP_FORMULA_BE", value); }
    }

    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        string s = TabContainer1.ActiveTab.HeaderText;

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //switch (CurrentAISUser.Role)
            //{
            //    case GlobalConstants.ApplicationSecurityGroup.Inquiry:
            //        hidSecurity.Value = "1";
            //        tblpnlERP.Visible = false;
            //        lstKOSetup.Enabled = false;
            //        this.Master.Page.Title = "KY OR Setup";
            //        btnAdd.Visible = false;
            //        break;
            //    case GlobalConstants.ApplicationSecurityGroup.SetupRecon:
            //        tblpnlERP.Visible = false;
            //        lstKOSetup.Enabled = false;
            //        this.Master.Page.Title = "KY OR Setup";
            //        btnAdd.Visible = false;
            //        break;
            //    case GlobalConstants.ApplicationSecurityGroup.AdjSpecialist:
            //        tblpnlERP.Visible = false;
            //        Panel2.Enabled = true;
            //        this.Master.Page.Title = "KY OR Setup";
            //        btnAdd.Visible = false;
            //        break;
            //    case GlobalConstants.ApplicationSecurityGroup.Manager:
            //        lstMasterERPFormula.Enabled = false;
            //        Panel2.Enabled = true;
            //        this.Master.Page.Title = "Master ERP Formula";
            //        btnAdd.Enabled = false;
            //        break;
            //    case GlobalConstants.ApplicationSecurityGroup.SystemAdmin:
            //        lstMasterERPFormula.Enabled = true;
            //        Panel2.Enabled = true;
            //        this.Master.Page.Title = "Master ERP Formula";
            //        break;
            //}
            if (TabContainer1.ActiveTab.HeaderText == "Deductible Taxes")
            {
                ContentPlaceHolder cp = (ContentPlaceHolder)this.Page.FindControl("Content1");
                ((Button)cp.FindControl("btnAdd")).Enabled = false;
            }
            ViewState["ERPFORMULAID"] = -1;
            masterERPFormulaBE = new MasterERPFormulaBE();
            MasterERPTransactionWrapper = new AISBusinessTransaction();
            //Function to Bind the Master Erp Formulas
            BindFormulaListView();
            BindKyorData();
            BindDeductibleTaxes();
            BindSurcharges();
        }

        //Checks Exiting without Save
        CheckExitWithoutSave();
    }

    private void CheckExitWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtDescription);
        list.Add(txtFormulaOne);
        list.Add(txtFormulaTwo);
        list.Add(btnAdd);
        list.Add(btnCancel);
        list.Add(btnSave);
        ProcessExitFlag(list);
    }
    /// <summary>
    /// Function to bind the MasterERPFormulas to the ListView
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void BindFormulaListView()
    {
        lstMasterERPFormula.DataSource = MasterERPService.getERPFormulasWithOrder();
        lstMasterERPFormula.DataBind();

    }
    protected void ERPCommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName != "Select")
        {
            //Function to Disable the Master ERP Formula
            DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true);
        }
    }
    /// <summary>
    /// Function to Disable the MasterERPFormula
    /// </summary>
    /// <param name="FormulaID"></param>
    /// <param name="Flag"></param>
    /// <returns></returns>
    /// Invoked when the Disable Link is clicked
    protected void DisableRow(int FormulaID, bool Flag)
    {
        masterERPFormulaBE = MasterERPService.getERPFormulaRow(FormulaID);
        masterERPFormulaBE.IsActive = Flag;
        masterERPFormulaBE.UPDATED_USERID = CurrentAISUser.PersonID;
        masterERPFormulaBE.UPDATED_DATE = DateTime.Now;
        Flag = MasterERPService.UpdateFormula(masterERPFormulaBE);
        ShowConcurrentConflict(Flag, masterERPFormulaBE.ErrorMessage);
        if (Flag)
        {
            MasterERPTransactionWrapper.SubmitTransactionChanges();
            //Function to Bind the Master Erp Formulas
            BindFormulaListView();
        }
        else
        {
            MasterERPTransactionWrapper.RollbackChanges();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool update;
        if (Convert.ToInt32(ViewState["ERPFORMULAID"]) != -1)
        {
            masterERPFormulaBE = MasterERPService.getERPFormulaRow(Convert.ToInt32(ViewState["ERPFORMULAID"]));
            masterERPFormulaBE.UPDATED_DATE = DateTime.Now;
            masterERPFormulaBE.UPDATED_USERID = CurrentAISUser.PersonID;
            update = true;
        }
        else
        {
            //QA 4982 Fix was discovered after fixing the 4982
            masterERPFormulaBE.IsActive = true;
            //QA 4982 Fix was discovered after fixing the 4982
            masterERPFormulaBE.CREATED_DATE = DateTime.Now;
            masterERPFormulaBE.CREATED_USERID = CurrentAISUser.PersonID;
            update = false;
        }
        masterERPFormulaBE.FormulaDescription = txtDescription.Text;
        masterERPFormulaBE.FormulaOneText = txtFormulaOne.Text;
        masterERPFormulaBE.FormulaTwoText = txtFormulaTwo.Text;
        bool Flag = MasterERPService.UpdateFormula(masterERPFormulaBE);

        if (update == true)
        {
            ShowConcurrentConflict(Flag, masterERPFormulaBE.ErrorMessage);

        }
        //Logging
        if (update)
        {
            (new Common(this.GetType())).Logger.Info("User -  " + CurrentAISUser.FullName + "[AZCORP:"
              + CurrentAISUser.UserID + ", Role: " + CurrentAISUser.Role + "] tried to update ERP Formula Info the AIS application");

        }
        if (Flag)
        {
            MasterERPTransactionWrapper.SubmitTransactionChanges();
            //Function to Bind the Master Erp Formulas
            BindFormulaListView();
            lnkClose.Visible = false;
            lblDetails.Visible = false;
            lstMasterERPFormula.Enabled = true;
            pnlDetails.Visible = false;
            pnlDetails.Enabled = false;
        }
        else
        {
            MasterERPTransactionWrapper.RollbackChanges();
        }

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        pnlDetails.Enabled = true;
        ViewState["ERPFORMULAID"] = -1;
        lstMasterERPFormula.Enabled = false;
        txtDescription.Text = "";
        txtFormulaOne.Text = "";
        txtFormulaTwo.Text = "";
        hidText.Value = "";
        lnkClose.Visible = true;
        lblDetails.Visible = true;
        pnlDetails.Visible = true;
        /// Function to Bind ERP List with LookupData
        BindERPList();
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            pnlDetails.Enabled = true;

        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            HtmlTableRow tr = (HtmlTableRow)lstMasterERPFormula.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["ERPFORMULAID"]) != -1)
        {
            txtFormulaOne.Text = ((Label)lstMasterERPFormula.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("lblFormulaTwo")).Text;
            //txtFormulaTwo.Text = ((HiddenField)lstMasterERPFormula.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("hidFormulaTwo")).Value;
            txtFormulaTwo.Text = ((Label)lstMasterERPFormula.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("lblFormulaOne")).Text;
            txtDescription.Text = ((Label)lstMasterERPFormula.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("lblDesc")).Text;
            hidText.Value = ((HiddenField)lstMasterERPFormula.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("hidFormulaTwo")).Value;
        }
        else
        {
            txtDescription.Text = "";
            txtFormulaOne.Text = "";
            txtFormulaTwo.Text = "";
            hidText.Value = "";
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        lnkClose.Visible = false;
        lblDetails.Visible = false;
        lstMasterERPFormula.Enabled = true;
        pnlDetails.Visible = false;
        HtmlTableRow tr;
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lstMasterERPFormula.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
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
        }
    }
    #region ERP
    /// <summary>
    /// Function to Bind ERP List with LookupData
    /// </summary>
    public void BindERPList()
    {
        IList<LookupBE> lookups = new List<LookupBE>();
        lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper() == "FORMULA COMPONENTS" && lk.ACTIVE == true).ToList();
        lookups = lookups.OrderByDescending(lk => lk.LookUpName).ToList();
        LstERP.DataSource = lookups;
        LstERP.DataBind();
    }
    protected void lstMasterERPFormula_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        HtmlTableRow tr;
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lstMasterERPFormula.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        lstMasterERPFormula.Enabled = false;
        pnlDetails.Visible = true;
        /// Function to Bind ERP List with LookupData
        BindERPList();
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            pnlDetails.Enabled = true;
        lnkClose.Visible = true;
        lblDetails.Visible = true;
        tr = (HtmlTableRow)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        LinkButton lnk = (LinkButton)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("lnkerpSelect");
        ViewState["SelectedIndex"] = e.NewSelectedIndex;
        tr.Attributes["class"] = "SelectedItemTemplate";
        ViewState["ERPFORMULAID"] = Convert.ToInt32(lnk.CommandArgument);
        if (((HiddenField)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("hidActive")).Value == "False")
        {
            pnlDetails.Enabled = false;
        }
        else
        {
            pnlDetails.Enabled = true;
        }
        txtFormulaOne.Text = ((Label)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("lblFormulaTwo")).Text;
        //txtFormulaTwo.Text = ((HiddenField)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("hidFormulaTwo")).Value;
        txtFormulaTwo.Text = ((Label)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("lblFormulaOne")).Text;
        txtDescription.Text = ((Label)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("lblDesc")).Text;
        hidText.Value = ((HiddenField)lstMasterERPFormula.Items[e.NewSelectedIndex].FindControl("hidFormulaTwo")).Value;
    }
    protected void LstERP_DataBoundList(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label lblFormula = (Label)e.Item.FindControl("lblFormula");
            if (lblFormula != null)
                lblFormula.Attributes.Add("onclick", "javascript:ERPFormula('" + lblFormula.Text + "')");
        }

    }
    #endregion
    #region KY &OR
    protected void lstKOSetup_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstKOSetup.EditIndex = e.NewEditIndex;
        BindKyorData();
    }

    protected void lstKOSetup_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if ((e.CommandName.ToUpper() == "SAVE"))
        {
            TextBox txtEffDt = (TextBox)e.Item.FindControl("txtEffDt");
            TextBox txtKentucky = (TextBox)e.Item.FindControl("txtKentucky");
            TextBox txtOregon = (TextBox)e.Item.FindControl("txtOregon");

            KYORSetupBE setupBE = new KYORSetupBE();

            setupBE.EFF_DT = Convert.ToDateTime(txtEffDt.Text);
            if (txtKentucky.Text != string.Empty)
                setupBE.KY_FCTR_RT = Convert.ToDecimal(txtKentucky.Text);
            if (txtOregon.Text != string.Empty)
                setupBE.OR_FCTR_RT = Convert.ToDecimal(txtOregon.Text);
            setupBE.CRTE_DT = System.DateTime.Now;
            setupBE.CRTE_USER_ID = 1;
            //setupBE.CRTE_USER_ID = ((AISUser)User).Userid
            //setupBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            KYORSetupBS koBS = new KYORSetupBS();
            koBS.SaveSetupData(setupBE);
            lstKOSetup.EditIndex = -1;


        }
        if ((e.CommandName.ToUpper() == "UPDATE"))
        {
            TextBox txtEffDt = (TextBox)e.Item.FindControl("txtEffDt");
            TextBox txtKentucky = (TextBox)e.Item.FindControl("txtKentucky");
            TextBox txtOregon = (TextBox)e.Item.FindControl("txtOregon");
            Label lblKOSetupId = (Label)e.Item.FindControl("hdKyOrSetupId");
            KYORSetupBE setupBE = new KYORSetupBE();
            KYORSetupBS koBS = new KYORSetupBS();
            setupBE = koBS.LoadData(Convert.ToInt32(lblKOSetupId.Text));
            setupBE.KY_OR_SETUP_ID = Convert.ToInt32(lblKOSetupId.Text);
            setupBE.EFF_DT = Convert.ToDateTime(txtEffDt.Text);
            if (txtKentucky.Text != string.Empty)
                setupBE.KY_FCTR_RT = Convert.ToDecimal(txtKentucky.Text);
            if (txtOregon.Text != string.Empty)
                setupBE.OR_FCTR_RT = Convert.ToDecimal(txtOregon.Text);
            setupBE.CRTE_DT = System.DateTime.Now;
            setupBE.CRTE_USER_ID = 1;
            //setupBE.CRTE_USER_ID = ((AISUser)User).Userid
            //setupBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            bool Flag = koBS.SaveSetupData(setupBE);
            ShowConcurrentConflict(Flag, setupBE.ErrorMessage);
            lstKOSetup.EditIndex = -1;

        }
        BindKyorData();
    }
    protected void lstKOSetup_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstKOSetup.EditIndex = -1;
            BindKyorData();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            lstKOSetup.InsertItemPosition = InsertItemPosition.None;
            BindKyorData();
        }
    }

    protected void lstKOSetup_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        lstKOSetup.InsertItemPosition = InsertItemPosition.None;
    }

    protected void lstKOSetup_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
    {

    }

    /// <summary>
    /// Function to bind the KY & OR SetUp Data to the ListView
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void BindKyorData()
    {
        IList<KYORSetupBE> list = new List<KYORSetupBE>();
        KYORSetupBS koBS = new KYORSetupBS();
        list = koBS.SelectData();
        this.lstKOSetup.DataSource = list;
        lstKOSetup.DataBind();
    }

    protected void lstKOSetup_DataBoundList(object sender, ListViewItemEventArgs e)
    {
        Label lblKentucky = (Label)e.Item.FindControl("lblKentucky");
        Label lblOregon = (Label)e.Item.FindControl("lblOregon");
        if (lblKentucky != null && lblKentucky.Text != string.Empty)
            lblKentucky.Text = Convert.ToDecimal(lblKentucky.Text).ToString("0000.000000");
        //lblKentucky.Text = Math.Round(Convert.ToDecimal(lblKentucky.Text), 6).ToString();
        if (lblOregon != null && lblOregon.Text != string.Empty)
            //lblOregon.Text = Math.Round(Convert.ToDecimal(lblOregon.Text), 6).ToString();
            lblOregon.Text = Convert.ToDecimal(lblOregon.Text).ToString("0000.000000");
    }
    #endregion

    #region Deductible Taxes
    /// <summary>
    /// Function to bind the Deductible Taxes to the ListView
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void BindDeductibleTaxes()
    {
        IList<DeductibleTaxesBE> list = new List<DeductibleTaxesBE>();
        DeductibleTaxesBS dTaxesBS = new DeductibleTaxesBS();
        list = dTaxesBS.SelectData();
        lstDeductibleTaxes.DataSource = list;
        lstDeductibleTaxes.DataBind();
    }
    protected void lstDeductibleTaxes_DataBoundList(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label lblTaxRate = (Label)e.Item.FindControl("lblTaxRate");
            if (lblTaxRate != null && lblTaxRate.Text != string.Empty)
                lblTaxRate.Text = Convert.ToDecimal(lblTaxRate.Text).ToString("0000.000000");
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
            //For Edit of Deductible Taxes
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (dataItem.DisplayIndex == lstDeductibleTaxes.EditIndex)
            {
                ////odsProgramPeriod.SelectParameters[0].DefaultValue = ddlAdjNumber.SelectedValue;
                Label lbldTaxesSetupId = (Label)e.Item.FindControl("lbldTaxesSetupId");
                DeductibleTaxesBE dTaxesBE = new DeductibleTaxesBE();
                DeductibleTaxesBS dTaxesBS = new DeductibleTaxesBS();
                dTaxesBE = dTaxesBS.SelectDataOnSetupId(Convert.ToInt32(lbldTaxesSetupId.Text));
                DropDownList ddlLOB = (DropDownList)e.Item.FindControl("ddlLOB");
                Label lblLOB = (Label)e.Item.FindControl("lblLOB");
                if ((ddlLOB != null) && (dTaxesBE != null))
                {
                    ddlLOB.Items.FindByText(dTaxesBE.LN_OF_BSN_TXT).Selected = true;
                }
                DropDownList ddlState = (DropDownList)e.Item.FindControl("ddlState");
                string strStateID = dTaxesBE.ST_ID.ToString();


                if ((ddlState != null) && (dTaxesBE != null))
                {
                    ListItem liState = ddlState.Items.FindByValue(strStateID);
                    liState.Selected = true;
                    DropDownList ddlDescription = (DropDownList)(e.Item.FindControl("ddlDescription"));
                    Label lblDescriptionId = (Label)(e.Item.FindControl("lblDescriptionId"));
                    //ddlDescription.DataSource = dTaxesBS.GetDescription(ddlState.SelectedItem.Text, GlobalConstants.LookUpType.TAX_TYPE);
                    ddlDescription.DataSource = dTaxesBS.GetDescriptionForEdit(Convert.ToInt32(lblDescriptionId.Text), ddlState.SelectedItem.Text, GlobalConstants.LookUpType.TAX_TYPE);
                    ddlDescription.DataBind();
                    Label lblDescription = (Label)e.Item.FindControl("lblDescription");
                    if ((ddlDescription != null) && (dTaxesBE != null))
                    {
                        ddlDescription.Items.FindByText(lblDescription.Text).Selected = true;
                    }
                }

                TextBox txtTaxRate = (TextBox)e.Item.FindControl("txtTaxRate");
                txtTaxRate.Text = Convert.ToDecimal(dTaxesBE.TAX_RATE).ToString("0000.000000");
                TextBox txtPolEffDate = (TextBox)e.Item.FindControl("txtPolEffDate");
                txtPolEffDate.Text = Convert.ToString(dTaxesBE.POL_EFF_DT);
                string ComponentId = ((Label)e.Item.FindControl("lblComponentAppliesTo")).Text;
                DropDownList ddlComponentAppliesTo = (DropDownList)(e.Item.FindControl("ddlComponentAppliesTo"));
                odsCompDescriptionEdit.SelectParameters[0].DefaultValue = strStateID;
                ddlComponentAppliesTo.DataSourceID = "odsCompDescriptionEdit";
                ddlComponentAppliesTo.DataTextField = "LookUpName";
                ddlComponentAppliesTo.DataValueField = "LookUpID";
                ddlComponentAppliesTo.DataBind();
                ddlComponentAppliesTo.Items.FindByValue(ComponentId.ToString()).Selected = true;
                if (dTaxesBE.TAX_END_DT != null)
                {
                    TextBox txtTaxEndDate = (TextBox)e.Item.FindControl("txtTaxEndDate");
                    txtTaxEndDate.Text = Convert.ToString(dTaxesBE.TAX_END_DT);
                }
            }

        }
    }
    protected void lstDeductibleTaxes_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstDeductibleTaxes.EditIndex = e.NewEditIndex;
        BindDeductibleTaxes();
    }
    protected void lstDeductibleTaxes_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstDeductibleTaxes.EditIndex = -1;
            BindDeductibleTaxes();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            lstDeductibleTaxes.InsertItemPosition = InsertItemPosition.None;
            BindDeductibleTaxes();
        }
    }
    protected void lstDeductibleTaxes_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        ClearError();
        if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "UPDATE"))
        {
            
            //Get the state value
            DropDownList ddlState = (DropDownList)(e.Item.FindControl("ddlState"));
            //Get the state value
            DropDownList ddlLOB = (DropDownList)(e.Item.FindControl("ddlLOB"));
            //Get the ComponentAppliesTo
            DropDownList ddlComponentAppliesTo = (DropDownList)(e.Item.FindControl("ddlComponentAppliesTo"));
            //Get the Tax Rate
            TextBox txtTaxRate = (TextBox)(e.Item.FindControl("txtTaxRate"));
            //Get the Policy Eff Date
            TextBox txtPolEffDate = (TextBox)(e.Item.FindControl("txtPolEffDate"));
            //Get the Tax Type ID
            DropDownList ddlDescription = (DropDownList)(e.Item.FindControl("ddlDescription"));
            //Get the tax end date
            TextBox txtTaxEndDate = (TextBox)(e.Item.FindControl("txtTaxEndDate"));
            //Get the Main and Sub
            Label lblMainSub = (Label)(e.Item.FindControl("lblMainSub"));
            if (lblMainSub.Text == null || lblMainSub.Text == "" )
            {
                ShowError("Main/Sub can not be empty.Please update the corresponding Main/Sub values in the Posting Transaction Setup.");
                return;
            } 
                // Code modified so that a record cannot be saved if either the Main/Sub values are missing in Posting Transaction Setup.
                string StrMain = string.Empty;
                StrMain = lblMainSub.Text.Substring(0, lblMainSub.Text.IndexOf("/")).ToString();
                string StrSub = string.Empty;
                StrSub = lblMainSub.Text.Substring(lblMainSub.Text.IndexOf("/") + 1).ToString();
                if (lblMainSub.Text == null || lblMainSub.Text == "" || lblMainSub.Text == "/" || StrMain == null || StrMain == "" || StrSub == "" || StrSub == null)
                {
                    ShowError("Main/Sub can not be empty.Please update the corresponding Main/Sub values in the Posting Transaction Setup.");
                    return;
                }
            
            if ((e.CommandName.ToUpper() == "SAVE"))
            {
                DeductibleTaxesBE dTaxesBE = new DeductibleTaxesBE();
                DeductibleTaxesBS dTaxesBS = new DeductibleTaxesBS();
                dTaxesBE.ST_ID = Convert.ToInt32(ddlState.SelectedValue);
                dTaxesBE.LN_OF_BSN_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                dTaxesBE.DED_TAX_COMPONENT_ID = Convert.ToInt32(ddlComponentAppliesTo.SelectedValue);
                dTaxesBE.TAX_RATE = Convert.ToDecimal(txtTaxRate.Text);
                dTaxesBE.POL_EFF_DT = DateTime.Parse(txtPolEffDate.Text);
                dTaxesBE.TAX_TYP_ID = Convert.ToInt32(ddlDescription.SelectedValue);
                if (txtTaxEndDate.Text != null && txtTaxEndDate.Text != string.Empty && txtTaxEndDate.Text != "")
                    dTaxesBE.TAX_END_DT = DateTime.Parse(txtTaxEndDate.Text);
                else
                    dTaxesBE.TAX_END_DT = null;
                dTaxesBE.MAIN_NBR_TXT = lblMainSub.Text.Substring(0, lblMainSub.Text.IndexOf("/"));
                dTaxesBE.SUB_NBR_TXT = lblMainSub.Text.Substring(lblMainSub.Text.IndexOf("/") + 1);
                //Get the Created user ID
                dTaxesBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                dTaxesBE.CRTE_DT = DateTime.Now;
                dTaxesBE.ACTV_IND = true;
                int intDuplicateCount = (new DeductibleTaxesBS()).isTaxTypeAlreadyExist(0,Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlDescription.SelectedValue), Convert.ToInt32(ddlComponentAppliesTo.SelectedValue), Convert.ToDateTime(txtPolEffDate.Text));
                //int intDuplicateCount = (new DeductibleTaxesBS()).isTaxTypeAlreadyExist(0, Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlDescription.SelectedValue), Convert.ToInt32(ddlComponentAppliesTo.SelectedValue)); 
                if (intDuplicateCount == 0)
                {
                    bool i = dTaxesBS.SaveDeductibleTaxes(dTaxesBE);
                    BindDeductibleTaxes();
                    ShowError("The entry has been saved.");
                }
                else if (intDuplicateCount == 1)
                {
                    ShowError("Duplicate record cannot be saved.");
                }
                else if (intDuplicateCount == 2)
                {
                    ShowError("Duplicate record cannot be saved.");
                }

            }
            if ((e.CommandName.ToUpper() == "UPDATE"))
            {
                DeductibleTaxesBE dTaxesBE = new DeductibleTaxesBE();
                DeductibleTaxesBS dTaxesBS = new DeductibleTaxesBS();
                Label lbldTaxesSetupId = (Label)e.Item.FindControl("lbldTaxesSetupId");
                dTaxesBE = dTaxesBS.LoadData(Convert.ToInt32(lbldTaxesSetupId.Text));
                dTaxesBE.ST_ID = Convert.ToInt32(ddlState.SelectedValue);
                dTaxesBE.LN_OF_BSN_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                dTaxesBE.DED_TAX_COMPONENT_ID = Convert.ToInt32(ddlComponentAppliesTo.SelectedValue);
                dTaxesBE.TAX_RATE = Convert.ToDecimal(txtTaxRate.Text);
                dTaxesBE.POL_EFF_DT = DateTime.Parse(txtPolEffDate.Text);
                dTaxesBE.TAX_TYP_ID = Convert.ToInt32(ddlDescription.SelectedValue);
                if (txtTaxEndDate.Text != null && txtTaxEndDate.Text != string.Empty && txtTaxEndDate.Text != "")
                    dTaxesBE.TAX_END_DT = DateTime.Parse(txtTaxEndDate.Text);
                else
                    dTaxesBE.TAX_END_DT = null;
                dTaxesBE.MAIN_NBR_TXT = lblMainSub.Text.Substring(0, lblMainSub.Text.IndexOf("/"));
                dTaxesBE.SUB_NBR_TXT = lblMainSub.Text.Substring(lblMainSub.Text.IndexOf("/") + 1);
                //Get the Created user ID
                dTaxesBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                dTaxesBE.CRTE_DT = DateTime.Now;

                dTaxesBE.ACTV_IND = true;
                int intDuplicateCount = (new DeductibleTaxesBS()).isTaxTypeAlreadyExist(dTaxesBE.DED_TAXES_SETUP_ID, Convert.ToInt32(ddlLOB.SelectedValue),Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlDescription.SelectedValue), Convert.ToInt32(ddlComponentAppliesTo.SelectedValue), Convert.ToDateTime(txtPolEffDate.Text));
                if (intDuplicateCount == 0)
                {
                    bool i = dTaxesBS.SaveDeductibleTaxes(dTaxesBE);
                    ShowError("The entry has been saved.");
                }
                else if (intDuplicateCount == 1)
                {
                    ShowError("Duplicate record cannot be saved.");
                }
                else if (intDuplicateCount == 2)
                {
                    ShowError("Duplicate record cannot be saved.");
                }
            }
            lstDeductibleTaxes.EditIndex = -1;
            BindDeductibleTaxes();
        }
        else if (e.CommandName == "DISABLE")
        {
            //Function to make Disable/Enable the MiscInvoice
            DisableDTaxesRow(Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName == "ENABLE")
        {
            //Function to make Disable/Enable the MiscInvoice
            DisableDTaxesRow(Convert.ToInt32(e.CommandArgument), true);
        }
    }
    protected void lstDeductibleTaxes_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        lstDeductibleTaxes.InsertItemPosition = InsertItemPosition.None;
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        DeductibleTaxesBS dTaxesBS = new DeductibleTaxesBS();
        DropDownList ddlState;
        DropDownList ddlDescription;
        if (lstDeductibleTaxes.EditIndex < 0)
        {
            ddlState = (DropDownList)(lstDeductibleTaxes.InsertItem.FindControl("ddlState"));
            ddlDescription = (DropDownList)(lstDeductibleTaxes.InsertItem.FindControl("ddlDescription"));
            ddlDescription.DataSource = dTaxesBS.GetDescription(ddlState.SelectedItem.Text, GlobalConstants.LookUpType.TAX_TYPE);
        }
        else
        {
            ddlState = (DropDownList)(lstDeductibleTaxes.EditItem.FindControl("ddlState"));
            ddlDescription = (DropDownList)(lstDeductibleTaxes.EditItem.FindControl("ddlDescription"));
            Label lblDescriptionId = (Label)(lstDeductibleTaxes.EditItem.FindControl("lblDescriptionId"));
            ddlDescription.DataSource = dTaxesBS.GetDescriptionForEdit(Convert.ToInt32(lblDescriptionId.Text), ddlState.SelectedItem.Text, GlobalConstants.LookUpType.TAX_TYPE);
        }
        ddlDescription.DataTextField = "LookUpTypeName";
        ddlDescription.DataValueField = "LookUpID";
        ddlDescription.DataBind();
    }
    protected void ddlDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        DeductibleTaxesBS dTaxesBS = new DeductibleTaxesBS();
        DropDownList ddlDescription;
        Label lblMainSub;
        if (lstDeductibleTaxes.EditIndex < 0)
        {
            ddlDescription = (DropDownList)(lstDeductibleTaxes.InsertItem.FindControl("ddlDescription"));
            lblMainSub = (Label)(lstDeductibleTaxes.InsertItem.FindControl("lblMainSub"));
        }
        else
        {
            ddlDescription = (DropDownList)(lstDeductibleTaxes.EditItem.FindControl("ddlDescription"));
            lblMainSub = (Label)(lstDeductibleTaxes.EditItem.FindControl("lblMainSub"));
        }
        if (ddlDescription.SelectedIndex > 0)
        {
            string ddlDesc = dTaxesBS.GetMainSub(ddlDescription.SelectedItem.Text.ToString());
            //06/23 for veracode 
            //lblMainSub.Text = dTaxesBS.GetMainSub(ddlDescription.SelectedItem.Text);
            lblMainSub.Text = Server.HtmlDecode(Server.HtmlEncode(ddlDesc));

        }
        else
            lblMainSub.Text = "";
    }
    protected void lstDeductibleTaxes_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
    {

    }
    protected void DisableDTaxesRow(int intDeductibleTaxSetupID, bool Flag)
    {
        try
        {
            DeductibleTaxesBE DTaxSetupBE = new DeductibleTaxesBE();
            DTaxSetupBE = (new DeductibleTaxesBS()).LoadData(intDeductibleTaxSetupID);
            DTaxSetupBE.ACTV_IND = Flag;
            DTaxSetupBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            DTaxSetupBE.UPDT_DT = DateTime.Now;
            bool i = (new DeductibleTaxesBS()).SaveDeductibleTaxes(DTaxSetupBE);
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message, ee);
        }
        BindDeductibleTaxes();

    }
    #endregion

    #region Surcharges Setup
    /// <summary>
    /// Function to bind the Surcharges to the ListView
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void BindSurcharges()
    {
        IList<SurchargesBE> list = new List<SurchargesBE>();
        SurchargesBS sTaxesBS = new SurchargesBS();
        list = sTaxesBS.SelectData();
        lstSurcharges.DataSource = list;
        lstSurcharges.DataBind();
    }
    protected void lstSurcharges_DataBoundList(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label lblSurRate = (Label)e.Item.FindControl("lblSurRate");
            if (lblSurRate != null && lblSurRate.Text != string.Empty)
                lblSurRate.Text = Convert.ToDecimal(lblSurRate.Text).ToString("0000.00000000");
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
            //For Edit of Surcharges Data
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (dataItem.DisplayIndex == lstSurcharges.EditIndex)
            {
                Label lblSurSetupId = (Label)e.Item.FindControl("lblSurSetupId");
                SurchargesBE sTaxesBE = new SurchargesBE();
                SurchargesBS sTaxesBS = new SurchargesBS();
                sTaxesBE = sTaxesBS.SelectDataOnSetupId(Convert.ToInt32(lblSurSetupId.Text));
                DropDownList ddlLOB = (DropDownList)e.Item.FindControl("ddlLOB");
                Label lblLOB = (Label)e.Item.FindControl("lblLOB");
                if ((ddlLOB != null) && (sTaxesBE != null))
                {
                    ddlLOB.Items.FindByText(sTaxesBE.LN_OF_BSN_TXT).Selected = true;
                }
                DropDownList ddlState = (DropDownList)e.Item.FindControl("ddlState");
                string strStateID = sTaxesBE.ST_ID.ToString();


                if ((ddlState != null) && (sTaxesBE != null))
                {
                    ListItem liState = ddlState.Items.FindByValue(strStateID);
                    liState.Selected = true;
                    DropDownList ddlSurCode = (DropDownList)(e.Item.FindControl("ddlSurCode"));
                    Label lblSurCodeId = (Label)(e.Item.FindControl("lblSurCodeId"));
                    ddlSurCode.DataSource = sTaxesBS.GetCodeForEdit(Convert.ToInt32(lblSurCodeId.Text), ddlState.SelectedItem.Text, GlobalConstants.LookUpType.SURCHARGE_ASSESSMENT_CODE);
                    ddlSurCode.DataBind();
                    Label lblSurCode = (Label)e.Item.FindControl("lblSurCode");
                    if ((ddlSurCode != null) && (sTaxesBE != null))
                    {
                        ddlSurCode.Items.FindByText(lblSurCode.Text).Selected = true;
                    }
                }
                TextBox txtSurEffDate = (TextBox)e.Item.FindControl("txtSurEffDate");
                txtSurEffDate.Text = Convert.ToString(sTaxesBE.SURCHARGE_EFF_DT);
                TextBox txtSurRate = (TextBox)e.Item.FindControl("txtSurRate");
                txtSurRate.Text = Convert.ToDecimal(sTaxesBE.SURCHARGE_RATE).ToString("0000.00000000");
                string FactorId = ((Label)e.Item.FindControl("lblChargedDateInd")).Text;
                DropDownList ddlChargedDateInd = (DropDownList)(e.Item.FindControl("ddlChargedDateInd"));

                //FactorDescriptionForEdit.SelectParameters[0].DefaultValue = strStateID;
                ddlChargedDateInd.DataSourceID = "odsFactorDescriptionEdit";
                ddlChargedDateInd.DataTextField = "LookUpName";
                ddlChargedDateInd.DataValueField = "LookUpID";
                ddlChargedDateInd.DataBind();
                ddlChargedDateInd.Items.FindByValue(FactorId.ToString()).Selected = true;

            }

        }
    }
    protected void lstSurcharges_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstSurcharges.EditIndex = e.NewEditIndex;
        BindSurcharges();
    }
    protected void lstSurcharges_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstSurcharges.EditIndex = -1;
            BindSurcharges();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            lstSurcharges.InsertItemPosition = InsertItemPosition.None;
            BindSurcharges();
        }
    }
    protected void lstSurcharges_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        lstSurcharges.InsertItemPosition = InsertItemPosition.None;
    }
    protected void lstSurcharges_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
    {

    }
    protected void ddlState_SelectedIndexChangedSur(object sender, EventArgs e)
    {
        SurchargesBS sTaxesBS = new SurchargesBS();
        DropDownList ddlState;
        DropDownList ddlSurCode;
        HiddenField hidSurDescriptionID;
        Label lblSurDescription;
        if (lstSurcharges.EditIndex < 0)
        {
            ddlState = (DropDownList)(lstSurcharges.InsertItem.FindControl("ddlState"));
            ddlSurCode = (DropDownList)(lstSurcharges.InsertItem.FindControl("ddlSurCode"));
            lblSurDescription = (Label)(lstSurcharges.InsertItem.FindControl("lblSurDescription"));
            hidSurDescriptionID = (HiddenField)(lstSurcharges.InsertItem.FindControl("hidSurDescriptionID"));
            lblSurDescription.Text = "";
            hidSurDescriptionID.Value = "";
            ddlSurCode.DataSource = sTaxesBS.GetCode(ddlState.SelectedItem.Text, GlobalConstants.LookUpType.SURCHARGE_ASSESSMENT_CODE);


        }
        else
        {
            ddlState = (DropDownList)(lstSurcharges.EditItem.FindControl("ddlState"));
            ddlSurCode = (DropDownList)(lstSurcharges.EditItem.FindControl("ddlSurCode"));
            Label lblCodeId = (Label)(lstSurcharges.EditItem.FindControl("lblSurCodeId"));
            lblSurDescription = (Label)(lstSurcharges.EditItem.FindControl("lblSurDescription"));
            hidSurDescriptionID = (HiddenField)(lstSurcharges.EditItem.FindControl("hidSurDescriptionIDEdit"));
            lblSurDescription.Text = "";
            hidSurDescriptionID.Value = "";
            ddlSurCode.DataSource = sTaxesBS.GetCodeForEdit(Convert.ToInt32(lblCodeId.Text), ddlState.SelectedItem.Text, GlobalConstants.LookUpType.SURCHARGE_ASSESSMENT_CODE);

        }
        ddlSurCode.DataTextField = "LookUpTypeName";
        ddlSurCode.DataValueField = "LookUpID";
        ddlSurCode.DataBind();
    }
    protected void ddlCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        SurchargesBS sTaxesBS = new SurchargesBS();
        LookupBE SurchargeDescriptionBE;
        DropDownList ddlSurCode;
        Label lblSurDescription;
        HiddenField hidSurDescriptionID;
        if (lstSurcharges.EditIndex < 0)
        {
            ddlSurCode = (DropDownList)(lstSurcharges.InsertItem.FindControl("ddlSurCode"));
            lblSurDescription = (Label)(lstSurcharges.InsertItem.FindControl("lblSurDescription"));
            hidSurDescriptionID = (HiddenField)(lstSurcharges.InsertItem.FindControl("hidSurDescriptionID"));
        }
        else
        {
            ddlSurCode = (DropDownList)(lstSurcharges.EditItem.FindControl("ddlSurCode"));
            lblSurDescription = (Label)(lstSurcharges.EditItem.FindControl("lblSurDescription"));
            hidSurDescriptionID = (HiddenField)(lstSurcharges.EditItem.FindControl("hidSurDescriptionIDEdit"));
        }
        if (ddlSurCode.SelectedIndex > 0)
        {
            SurchargeDescriptionBE = sTaxesBS.GetDescription(ddlSurCode.SelectedItem.Text);
            //06/23 for veracode
            //lblSurDescription.Text = SurchargeDescriptionBE.LookUpName;
            lblSurDescription.Text = Server.HtmlDecode(Server.HtmlEncode(SurchargeDescriptionBE.LookUpName));
            hidSurDescriptionID.Value = SurchargeDescriptionBE.LookUpID.ToString();

        }
        else
        {
            lblSurDescription.Text = "";
            hidSurDescriptionID.Value = "";
        }
    }
    protected void lstSurcharges_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        ClearError();
        if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "UPDATE"))
        {

            //Get the state value
            DropDownList ddlState = (DropDownList)(e.Item.FindControl("ddlState"));
            //Get the state value
            DropDownList ddlLOB = (DropDownList)(e.Item.FindControl("ddlLOB"));
            //Get the Surcharge Code
            DropDownList ddlSurCode = (DropDownList)(e.Item.FindControl("ddlSurCode"));
            //Get the Surcharge Factor
            TextBox txtSurRate = (TextBox)(e.Item.FindControl("txtSurRate"));
            //Get the Surcharge Eff Date
            TextBox txtSurEffDate = (TextBox)(e.Item.FindControl("txtSurEffDate"));
            //Get the Surcharge Date Indicator
            DropDownList ddlChargedDateInd = (DropDownList)(e.Item.FindControl("ddlChargedDateInd"));
            //Get the Surcharge Description
            Label lblSurDescription = (Label)(e.Item.FindControl("lblSurDescription"));
            //
            HiddenField hidSurDescriptionID = (HiddenField)(lstSurcharges.InsertItem.FindControl("hidSurDescriptionID"));
            if (lblSurDescription.Text == null || lblSurDescription.Text == "")
            {
                ShowError("Surcharge/Assessment Description cannot be empty.Please update the Surcharge/Assessment Description for the relative Surcharge/Assessment Code.");
                return;
            }
            PostingTransactionTypeBS PostBS = new PostingTransactionTypeBS();
            bool blMainSub = PostBS.IsMainSubExits(lblSurDescription.Text.Trim(), "SURCHARGES AND ASSESSMENTS");
            if (!blMainSub)
            {
                ShowError("Please enter the Main/Sub values in the Posting Transaction Setup.");
                return;
            }
            if (ddlChargedDateInd.SelectedItem.Text == "D" && !(Convert.ToDecimal(txtSurRate.Text) == 0))
            {
                ShowError("For the discontinued surcharges, surcharge rate other than Zero is not allowed.Please enter surcharge rate as zero.");
                return;
            }
            if ((e.CommandName.ToUpper() == "SAVE"))
            {
                SurchargesBE sTaxesBE = new SurchargesBE();
                SurchargesBS sTaxesBS = new SurchargesBS();
                sTaxesBE.ST_ID = Convert.ToInt32(ddlState.SelectedValue);
                sTaxesBE.LN_OF_BSN_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                sTaxesBE.SURCHARGE_CODE_ID = Convert.ToInt32(ddlSurCode.SelectedValue);
                sTaxesBE.SURCHARGE_RATE = Convert.ToDecimal(txtSurRate.Text);
                sTaxesBE.SURCHARGE_EFF_DT = DateTime.Parse(txtSurEffDate.Text);
                sTaxesBE.SURCHARGE_FACTOR_ID = Convert.ToInt32(ddlChargedDateInd.SelectedValue);

                sTaxesBE.SURCHARGE_TYPE_ID = Convert.ToInt32(hidSurDescriptionID.Value);
                //Get the Created user ID
                sTaxesBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                sTaxesBE.CRTE_DT = DateTime.Now;
                sTaxesBE.ACTV_IND = true;
                int intDuplicateCount = (new SurchargesBS()).isSurAlreadyExist(0, Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlSurCode.SelectedValue), Convert.ToDateTime(txtSurEffDate.Text));
                //int intDuplicateCount = (new DeductibleTaxesBS()).isTaxTypeAlreadyExist(0, Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlDescription.SelectedValue), Convert.ToInt32(ddlComponentAppliesTo.SelectedValue)); 
                if (intDuplicateCount == 0)
                {
                    bool i = sTaxesBS.SaveSurcharges(sTaxesBE);
                    BindSurcharges();
                    ShowError("The entry has been saved.");
                }
                else if (intDuplicateCount == 1)
                {
                    ShowError("Duplicate record cannot be saved.");
                }
                else if (intDuplicateCount == 2)
                {
                    ShowError("Duplicate record cannot be saved.");
                }

            }
            if ((e.CommandName.ToUpper() == "UPDATE"))
            {
                SurchargesBE sTaxesBE = new SurchargesBE();
                SurchargesBS sTaxesBS = new SurchargesBS();
                Label lblSurSetupId = (Label)e.Item.FindControl("lblSurSetupId");
                HiddenField hidSurDescriptionIDEdit = (HiddenField)e.Item.FindControl("hidSurDescriptionIDEdit");
                sTaxesBE = sTaxesBS.LoadData(Convert.ToInt32(lblSurSetupId.Text));
                sTaxesBE.LN_OF_BSN_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                sTaxesBE.ST_ID = Convert.ToInt32(ddlState.SelectedValue);
                sTaxesBE.SURCHARGE_CODE_ID = Convert.ToInt32(ddlSurCode.SelectedValue);
                sTaxesBE.SURCHARGE_RATE = Convert.ToDecimal(txtSurRate.Text);
                sTaxesBE.SURCHARGE_EFF_DT = DateTime.Parse(txtSurEffDate.Text);
                sTaxesBE.SURCHARGE_FACTOR_ID = Convert.ToInt32(ddlChargedDateInd.SelectedValue);

                sTaxesBE.SURCHARGE_TYPE_ID = Convert.ToInt32(hidSurDescriptionIDEdit.Value);
                //Get the Created user ID
                sTaxesBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                sTaxesBE.CRTE_DT = DateTime.Now;
                sTaxesBE.ACTV_IND = true;

                sTaxesBE.ACTV_IND = true;
                int intDuplicateCount = (new SurchargesBS()).isSurAlreadyExist(sTaxesBE.SURCHARGE_ASSESS_SETUP_ID, Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlSurCode.SelectedValue), Convert.ToDateTime(txtSurEffDate.Text));
                if (intDuplicateCount == 0)
                {
                    bool i = sTaxesBS.SaveSurcharges(sTaxesBE);
                    ShowError("The entry has been Updated.");
                }
                else if (intDuplicateCount == 1)
                {
                    ShowError("Duplicate record cannot be saved.");
                }
                else if (intDuplicateCount == 2)
                {
                    ShowError("Duplicate record cannot be saved.");
                }
            }
            lstSurcharges.EditIndex = -1;
            BindSurcharges();
        }
        else if (e.CommandName == "DISABLE")
        {
            //Function to make Disable/Enable the record
            DisableSurTaxesRow(Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName == "ENABLE")
        {
            //Function to make Disable/Enable the record
            DisableSurTaxesRow(Convert.ToInt32(e.CommandArgument), true);
        }
    }
    protected void DisableSurTaxesRow(int intSurSetupID, bool Flag)
    {
        try
        {
            SurchargesBE sTaxesBE = new SurchargesBE();
            sTaxesBE = (new SurchargesBS()).LoadData(intSurSetupID);
            sTaxesBE.ACTV_IND = Flag;
            sTaxesBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            sTaxesBE.UPDT_DT = DateTime.Now;
            bool i = (new SurchargesBS()).SaveSurcharges(sTaxesBE);
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message, ee);
        }
        BindSurcharges();

    }


    #endregion

}


