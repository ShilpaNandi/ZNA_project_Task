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
            if ((AISBusinessTransaction)Session["MasterERPTransaction"] == null)
                Session["MasterERPTransaction"] = new AISBusinessTransaction();
            return (AISBusinessTransaction)Session["MasterERPTransaction"];
        }
        set
        {
            Session["MasterERPTransaction"] = value;
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
        get { return (MasterERPFormulaBE)Session["MASRTER_ERP_FORMULA_BE"]; }
        set { Session["MASRTER_ERP_FORMULA_BE"] = value; }
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

            ViewState["ERPFORMULAID"] = -1;
            masterERPFormulaBE = new MasterERPFormulaBE();
            MasterERPTransactionWrapper = new AISBusinessTransaction();
            //Function to Bind the Master Erp Formulas
            BindFormulaListView();
            BindKyorData();

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
          bool Flag= koBS.SaveSetupData(setupBE);
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
    protected void LstERP_DataBoundList(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label lblFormula = (Label)e.Item.FindControl("lblFormula");
            if (lblFormula != null)
                lblFormula.Attributes.Add("onclick", "javascript:ERPFormula('" + lblFormula.Text + "')");
        }

    }


}


