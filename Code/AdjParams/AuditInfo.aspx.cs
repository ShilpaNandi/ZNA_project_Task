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
using System.Text.RegularExpressions;

public partial class AdjParameters_AuditInfo : AISBasePage
{
    private Coml_Agmt_AuDtBS comAgrBS;
    private Sub_Audt_PremBS subaudprem;
    private Non_Sub_Audt_PremBS nonsubaudprem;
    /// <summary>
    /// property to hold an instance for Business Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISBusinessTransaction property</returns>
    protected AISBusinessTransaction AuditTransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["PersonTransaction"] == null)
            //    Session["PersonTransaction"] = new AISBusinessTransaction();
            //return (AISBusinessTransaction)Session["PersonTransaction"];
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("PersonTransaction") == null)
                SaveObjectToSessionUsingWindowName("PersonTransaction", new AISBusinessTransaction());
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("PersonTransaction");
        }
        set
        {
            //Session["PersonTransaction"] = value;
            SaveObjectToSessionUsingWindowName("PersonTransaction", value);
        }
    }
    /// <summary>
    /// a property for Commercial Agreement Audit Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Coml_Agmt_AuDtBS</returns>
    private Coml_Agmt_AuDtBS CommAgrAudService
    {
        get
        {
            if (comAgrBS == null)
            {
                comAgrBS = new Coml_Agmt_AuDtBS();
                //comAgrBS.AppTransactionWrapper = AuditTransactionWrapper;
            }
            return comAgrBS;
        }
    }
    /// <summary>
    /// a property for Subject Audit Premium Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Sub_Audt_PremBS</returns>
    private Sub_Audt_PremBS SubAudPremService
    {
        get
        {
            if (subaudprem == null)
            {
                subaudprem = new Sub_Audt_PremBS();
                //subaudprem.AppTransactionWrapper = AuditTransactionWrapper;
            }
            return subaudprem;
        }
    }
    /// <summary>
    /// a property for Non Subject Audit Premium Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Non_Sub_Audt_PremBS</returns>
    private Non_Sub_Audt_PremBS NonSubAudPremService
    {
        get
        {
            if (nonsubaudprem == null)
            {
                nonsubaudprem = new Non_Sub_Audt_PremBS();
                //nonsubaudprem.AppTransactionWrapper = AuditTransactionWrapper;
            }
            return nonsubaudprem;
        }
    }
    /// <summary>
    /// a property for Commercial Agreement Audit Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Coml_Agmt_AuDtBE</returns>
    private Coml_Agmt_AuDtBE ComAgrAudBE
    {
        //get { return (Coml_Agmt_AuDtBE)Session["COML_AGMT_AUDTBE"]; }
        //set { Session["COML_AGMT_AUDTBE"] = value; }
        get { return (Coml_Agmt_AuDtBE)RetrieveObjectFromSessionUsingWindowName("COML_AGMT_AUDTBE"); }
        set { SaveObjectToSessionUsingWindowName("COML_AGMT_AUDTBE", value); }
    }
    /// <summary>
    /// a property for Subject Audit Premium Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>SubjectAuditPremiumBE</returns>
    private SubjectAuditPremiumBE SubAudPremBE
    {
        //get { return (SubjectAuditPremiumBE)Session["SUBJECTAUDITPREMIUMBE"]; }
        //set { Session["SUBJECTAUDITPREMIUMBE"] = value; }
        get { return (SubjectAuditPremiumBE)RetrieveObjectFromSessionUsingWindowName("SUBJECTAUDITPREMIUMBE"); }
        set { SaveObjectToSessionUsingWindowName("SUBJECTAUDITPREMIUMBE", value); }
    }
    /// <summary>
    /// a property for Non Commercial Agreement Audit Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>NonSubjectAuditPremiumBE</returns>
    private NonSubjectAuditPremiumBE NonComAgrPrmBE
    {
        //get { return (NonSubjectAuditPremiumBE)Session["NONSUBJECTAUDITPREMIUMBE"]; }
        //set { Session["NONSUBJECTAUDITPREMIUMBE"] = value; }
        get { return (NonSubjectAuditPremiumBE)RetrieveObjectFromSessionUsingWindowName("NONSUBJECTAUDITPREMIUMBE"); }
        set { SaveObjectToSessionUsingWindowName("NONSUBJECTAUDITPREMIUMBE", value); }
    }
    IList<Coml_Agmt_AuDtBE> ComAgrAudBEList
    {
        get
        {
            //if (Session["ComAgrAudBEList"] == null)
            //    Session["ComAgrAudBEList"] = new List<Coml_Agmt_AuDtBE>();
            //return (IList<Coml_Agmt_AuDtBE>)Session["ComAgrAudBEList"];
            if (RetrieveObjectFromSessionUsingWindowName("ComAgrAudBEList") == null)
                SaveObjectToSessionUsingWindowName("ComAgrAudBEList", new List<Coml_Agmt_AuDtBE>());
            return (IList<Coml_Agmt_AuDtBE>)RetrieveObjectFromSessionUsingWindowName("ComAgrAudBEList");
        }
        set
        {
            //Session["ComAgrAudBEList"] = value;
            SaveObjectToSessionUsingWindowName("ComAgrAudBEList", value);
        }
    }
    IList<SubjectAuditPremiumBE> SubAudPremBEList
    {
        get
        {
            //if (Session["SubAudPremBEList"] == null)
            //    Session["SubAudPremBEList"] = new List<SubjectAuditPremiumBE>();
            //return (IList<SubjectAuditPremiumBE>)Session["SubAudPremBEList"];
            if (RetrieveObjectFromSessionUsingWindowName("SubAudPremBEList") == null)
                SaveObjectToSessionUsingWindowName("SubAudPremBEList", new List<SubjectAuditPremiumBE>());
            return (IList<SubjectAuditPremiumBE>)RetrieveObjectFromSessionUsingWindowName("SubAudPremBEList");
        }
        set
        {
            //Session["SubAudPremBEList"] = value;
            SaveObjectToSessionUsingWindowName("SubAudPremBEList", value);
        }
    }
    IList<NonSubjectAuditPremiumBE> NonComAgrPrmBEList
    {
        get
        {
            //if (Session["NonComAgrPrmBEList"] == null)
            //    Session["NonComAgrPrmBEList"] = new List<NonSubjectAuditPremiumBE>();
            //return (IList<NonSubjectAuditPremiumBE>)Session["NonComAgrPrmBEList"];
            if (RetrieveObjectFromSessionUsingWindowName("NonComAgrPrmBEList") == null)
                SaveObjectToSessionUsingWindowName("NonComAgrPrmBEList", new List<NonSubjectAuditPremiumBE>());
            return (IList<NonSubjectAuditPremiumBE>)RetrieveObjectFromSessionUsingWindowName("NonComAgrPrmBEList");
        }
        set
        {
            //Session["NonComAgrPrmBEList"] = value;
            SaveObjectToSessionUsingWindowName("NonComAgrPrmBEList", value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucProgramPeriod.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ucProgramPeriod_ProgramPeriodRowClicked);
        if (!Page.IsPostBack)
        {
            this.Master.Page.Title = "Audit Information";
            ComAgrAudBE = new Coml_Agmt_AuDtBE();
            SubAudPremBE = new SubjectAuditPremiumBE();
            NonComAgrPrmBE = new NonSubjectAuditPremiumBE();
            try
            {
                if (Request.QueryString["Flag"] != null)
                {
                    AjaxControlToolkit.CollapsiblePanelExtender collaps = (AjaxControlToolkit.CollapsiblePanelExtender)UcMastervalues.FindControl("CollapseAccountHeader");
                    collaps.Collapsed = bool.Parse(Request.QueryString["Flag"].ToString());
                }
                if (Request.QueryString["ProgPerdID"] != null)
                {
                    hidProgPerdID.Value = Request.QueryString["ProgPerdID"].ToString();
                    SelectProgPeriod(Convert.ToInt32(Request.QueryString["ProgPerdID"].ToString()));
                    lblAuditinfoDate.Text = "Audit Information -- ";
                    ProgramPeriodBE ProgPerdBE = ((new ProgramPeriodsBS()).getProgramPeriodRow(Convert.ToInt32(hidProgPerdID.Value)));
                    lblAuditinfoDate.Text += ProgPerdBE.PROGRAMPERIOD;
                    //Logic to highlighted  Select the ProgramPeriod Line for cinsistancy setting the Public property of ProgramPeriod User control
                    ucProgramPeriod.SelectedProgramID = Convert.ToInt32(hidProgPerdID.Value);
                }
            }
            catch (Exception ee)
            {
            }

        }

        //Checks Exiting Without Save
        ArrayList list = new ArrayList();
        list.Add(lnkAuditinfoClose);
        ProcessExitFlag(list);

    }
    void ucProgramPeriod_ProgramPeriodRowClicked(object sender, ListViewCommandEventArgs e)
    {
        hidProgPerdID.Value = e.CommandArgument.ToString();
        SelectProgPeriod(Convert.ToInt32(e.CommandArgument));
        lblAuditinfoDate.Text = "Audit Information -- ";
        lblAuditinfoDate.Text += ((Label)e.Item.FindControl("lblstartDate")).Text + " - ";
        lblAuditinfoDate.Text += ((Label)e.Item.FindControl("lblendDate")).Text;

    }
    /// <summary>
    /// Function to Display when Program Period is selected
    /// </summary>
    /// <param name="ProgramPeriodID"></param>
    /// </summary>
    public void SelectProgPeriod(int prgPrdID)
    {
        ((ListView)ucProgramPeriod.FindControl("lstProgramPeriod")).Enabled = false;
        ViewState["ProgramPeriodID"] = prgPrdID;
        lnkAuditinfoClose.Visible = true;
        pnlAuditInfo.Visible = true;
        /// Function to Bind the lstAuditInfo ListView 
        BindAuditinfoList(prgPrdID);
    }
    protected void CloseAuditinfo(object sender, EventArgs e)
    {
        hidProgPerdID.Value = "0";
        LinkButton lnk = (LinkButton)sender;
        if (lnk.ID.ToString() == "lnkAuditinfoClose")
        {
            pnlAuditInfo.Visible = false;
            lnkAuditinfoClose.Visible = false;
            lblAuditinfoDate.Text = "";
            ((ListView)ucProgramPeriod.FindControl("lstProgramPeriod")).Enabled = true;
        }
        else
        {
            tblTotal.Visible = false;
            lblTotalSubPremAmt.Text = "0";
            lnkAuditinfoClose.Enabled = true;
            //if (lstSubject.Controls.Count > 0)
            //{
            //    ((Label)lstSubject.FindControl("lblTotalSubPremAmt")).Text = "";
            //}
            //if (lstNonSubject.Controls.Count > 0)
            //{
            //    ((Label)lstNonSubject.FindControl("lblTotalSubPremAmt")).Text = "";
            //}
            lstAuditInfo.Enabled = true;
            pnlDetails.Visible = false;
            lnkAuditPremium.Visible = false;
            lblAuditPremium.Text = "";

        }
    }
    /// <summary>
    /// Function to Bind the lstAuditInfo ListView 
    /// </summary>
    /// <param name="prgPrdID"></param>
    public void BindAuditinfoList(int prgPrdID)
    {
        ComAgrAudBEList = ((CommAgrAudService.getCommAgrAuditList(prgPrdID)).Where(com => com.Customer_ID == AISMasterEntities.AccountNumber)).ToList();
        lstAuditInfo.DataSource = ComAgrAudBEList;
        lstAuditInfo.DataBind();
        if (lstAuditInfo.InsertItemPosition != InsertItemPosition.None)
        {

            DropDownList ddl = (DropDownList)lstAuditInfo.InsertItem.FindControl("ddlPolicy");
            Coml_AgmtBS Coml_agmt = new Coml_AgmtBS();
            ddl.DataSource = Coml_agmt.getCommercialAgreement(prgPrdID, AISMasterEntities.AccountNumber);
            ddl.DataTextField = "POLICY";
            ddl.DataValueField = "Comm_Agr_ID";
            ddl.DataBind();
            ListItem li = new ListItem("(Select)", "0");
            ddl.Items.Insert(0, li);
        }

    }
    /// <summary>
    /// Function to Bind the lblAuditPremium ListView
    /// </summary>
    /// <param name="strSubject"></param>
    public void BindSubPrem(string strSubject)
    {
        lnkAuditinfoClose.Enabled = false;
        lstAuditInfo.Enabled = false;
        lnkAuditPremium.Visible = true;
        pnlDetails.Visible = true;
        tblTotal.Visible = true;
        if (strSubject == "SUBJECT")
        {
            lblAuditPremium.Text = "Subject Premium";
            lstNonSubject.Visible = false;
            lstSubject.Visible = true;
            SubAudPremBEList = (SubAudPremService.getsubPremAudList(int.Parse(ViewState["COM_AGR_AUD_ID"].ToString()))).ToList();
            lstSubject.DataSource = SubAudPremBEList;
            lstSubject.DataBind();
            DropDownList ddlSubject = (DropDownList)lstSubject.InsertItem.FindControl("ddlState");
            ddlSubject.DataSource = SubAudPremService.getStateNames(int.Parse(ViewState["COM_AGR_AUD_ID"].ToString()), -1);
            ddlSubject.DataTextField = "LookUpName";
            ddlSubject.DataValueField = "LookUpID";
            ddlSubject.DataBind();
        }
        else
        {
            lblAuditPremium.Text = "Non-Subject Premium";
            lstNonSubject.Visible = true;
            lstSubject.Visible = false;
            NonComAgrPrmBEList = (NonSubAudPremService.getNonsubPremAudList(int.Parse(ViewState["COM_AGR_AUD_ID"].ToString()))).ToList();
            lstNonSubject.DataSource = NonComAgrPrmBEList;
            lstNonSubject.DataBind();
            DropDownList ddlSubject = (DropDownList)lstNonSubject.InsertItem.FindControl("ddlNonSub");
            ddlSubject.DataSource = nonsubaudprem.getNONSubjectPrem(int.Parse(ViewState["COM_AGR_AUD_ID"].ToString()), -1);
            ddlSubject.DataTextField = "LookUpName";
            ddlSubject.DataValueField = "LookUpID";
            ddlSubject.DataBind();
        }
    }
    protected void InsertCategory(object sender, ListViewInsertEventArgs e)
    {
        TextBox txtDepPrem = (TextBox)e.Item.FindControl("txtdefdepprem");
        TextBox txtsubPrem = (TextBox)e.Item.FindControl("txtsubdepPrem");
        TextBox txtNsubPrem = (TextBox)e.Item.FindControl("txtNSubDepPrem");
        Label lblAudit = (Label)e.Item.FindControl("lblAuditResult");
        if (txtNsubPrem != null)
        {
            txtDepPrem.Attributes["onchange"] = "javascript:CalculateAuditresult(0," + txtDepPrem.ClientID + "," + txtsubPrem.ClientID + "," + txtNsubPrem.ClientID + "," + lblAudit.ClientID + ");";
            txtsubPrem.Attributes["onchange"] = "javascript:CalculateAuditresult(0," + txtDepPrem.ClientID + "," + txtsubPrem.ClientID + "," + txtNsubPrem.ClientID + "," + lblAudit.ClientID + ");";
            txtNsubPrem.Attributes["onchange"] = "javascript:CalculateAuditresult(0," + txtDepPrem.ClientID + "," + txtsubPrem.ClientID + "," + txtNsubPrem.ClientID + "," + lblAudit.ClientID + ");";
        }
    }
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        DropDownList ddl;
        ListView lst = (ListView)sender;
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            if (lst.ID == "lstAuditInfo")
            {
                //QA4869 fix Which provides user to edit the policy number - Rolled back later
                Label lblEditPolicy = (Label)e.Item.FindControl("lblComAgrIDEdt");
                Label lblPolicy = (Label)e.Item.FindControl("lblComlPolicy");
                DropDownList ddlPlcy = (DropDownList)e.Item.FindControl("ddlPolicyEdt");
                HiddenField AuditIndicator = (HiddenField)e.Item.FindControl("hidAdjustIndicator");
                Label lblreviestatus = (Label)e.Item.FindControl("lblRevise");
                LinkButton lnkBtnEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                if (lblreviestatus != null && AuditIndicator != null)
                {
                    if ((AuditIndicator.Value == "False" && lblreviestatus.Text == "False") || (AuditIndicator.Value == "" && lblreviestatus.Text == ""))
                    {
                        lblPolicy.Visible = false;
                        ddlPlcy.Visible = true;
                        if (lblEditPolicy != null && ddlPlcy != null)
                        {
                            Coml_AgmtBS Coml_agmt = new Coml_AgmtBS();
                            ddlPlcy.DataSource = Coml_agmt.getCommercialAgreement(int.Parse(ViewState["ProgramPeriodID"].ToString()), AISMasterEntities.AccountNumber);
                            ddlPlcy.DataTextField = "POLICY";
                            ddlPlcy.DataValueField = "Comm_Agr_ID";
                            ddlPlcy.DataBind();
                            ListItem li = new ListItem("(Select)", "0");
                            ddlPlcy.Items.Insert(0, li);
                            //ddlPlcy.Items.FindByValue(lblEditPolicy.Text).Selected = true;
                            AddInActivePolicyData(ref ddlPlcy, lblEditPolicy.Text);
                            //ddlPlcy.SelectedIndex = ddlPlcy.Items.IndexOf(ddlPlcy.Items.FindByText(lblEditPolicy.Text.ToString()));
                        }

                    }
                    else
                    {
                        lblPolicy.Visible = true;
                        ddlPlcy.Visible = false;
                    }
                }
                //QA4869 fix 
                Label lblSubAudit = (Label)e.Item.FindControl("lblSubAudit");
                Label lblNonSubAudit = (Label)e.Item.FindControl("lblNonSubAudit");
                TextBox txtDepPrem = (TextBox)e.Item.FindControl("txtdefdepprem");
                TextBox txtsubPrem = (TextBox)e.Item.FindControl("txtsubdepPrem");
                TextBox txtNsubPrem = (TextBox)e.Item.FindControl("txtNSubDepPrem");
                Label lblAudit = (Label)e.Item.FindControl("lblAuditResult");
                if (txtNsubPrem != null)
                {
                    double intValue = 0 + Convert.ToDouble(lblSubAudit.Text) + Convert.ToDouble(lblNonSubAudit.Text);
                    txtDepPrem.Attributes["onchange"] = "javascript:CalculateAuditresult(" + intValue + "," + txtDepPrem.ClientID + "," + txtsubPrem.ClientID + "," + txtNsubPrem.ClientID + "," + lblAudit.ClientID + ");";
                    txtsubPrem.Attributes["onchange"] = "javascript:CalculateAuditresult(" + intValue + "," + txtDepPrem.ClientID + "," + txtsubPrem.ClientID + "," + txtNsubPrem.ClientID + "," + lblAudit.ClientID + ");";
                    txtNsubPrem.Attributes["onchange"] = "javascript:CalculateAuditresult(" + intValue + "," + txtDepPrem.ClientID + "," + txtsubPrem.ClientID + "," + txtNsubPrem.ClientID + "," + lblAudit.ClientID + ");";
                }
            }
            else
            {
                HiddenField hidStateID = (HiddenField)e.Item.FindControl("hidStateID");
                HiddenField hidNSA = (HiddenField)e.Item.FindControl("hidNSATypID");
                HiddenField hidState = (HiddenField)e.Item.FindControl("hidState");
                HiddenField hidNSATYPE = (HiddenField)e.Item.FindControl("hidNSATYPE");
                ListItem li;

                if (hidStateID != null)
                {
                    ddl = (DropDownList)e.Item.FindControl("ddlState");
                    ddl.DataSource = SubAudPremService.getStateNames(int.Parse(ViewState["COM_AGR_AUD_ID"].ToString()), int.Parse(hidStateID.Value));
                    ddl.DataTextField = "LookUpName";
                    ddl.DataValueField = "LookUpID";
                    ddl.DataBind();
                    li = new ListItem(hidState.Value, hidStateID.Value);
                    if (ddl.Items.Contains(li))
                        ddl.Items.FindByValue(hidStateID.Value).Selected = true;
                }
                if (hidNSA != null)
                {
                    ddl = (DropDownList)e.Item.FindControl("ddlNonSub");
                    ddl.DataSource = nonsubaudprem.getNONSubjectPrem(int.Parse(ViewState["COM_AGR_AUD_ID"].ToString()), int.Parse(hidNSA.Value));
                    ddl.DataTextField = "LookUpName";
                    ddl.DataValueField = "LookUpID";
                    ddl.DataBind();
                    li = new ListItem(hidNSATYPE.Value, hidNSA.Value);
                    if (ddl.Items.Contains(li))
                        ddl.Items.FindByValue(hidNSA.Value).Selected = true;
                }

                ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
                if (imgDelete != null)
                {
                    HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                    if (hid.Value == "True")
                    {
                        LinkButton lblEDit = (LinkButton)e.Item.FindControl("lnkEdit");
                        lblEDit.Enabled = true;
                        imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable?');");
                    }
                    else
                    {
                        LinkButton lblEDit = (LinkButton)e.Item.FindControl("lnkEdit");
                        lblEDit.Enabled = false;
                        imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable?');");
                    }
                }
            }
        }
        else if (e.Item.ItemType == ListViewItemType.InsertItem)
        {

        }
    }
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        ListView lst = (ListView)sender;
        if (e.CommandName.ToUpper() == "SAVE")
        {
            if (lst.ID.ToString() == "lstAuditInfo")
            {
                SaveAuditList(e.Item);
            }
            else if (lst.ID.ToString() == "lstSubject")
            {
                SaveSubjectList(e.Item);
            }
            else if (lst.ID.ToString() == "lstNonSubject")
            {
                SaveNonSubjectList(e.Item);
            }
        }
        else if (e.CommandName.ToUpper() == "SUBJECT")
        {
            ViewState["COM_AGR_AUD_ID"] = e.CommandArgument;
            ViewState["COM_AGR_ID"] = ((Label)e.Item.FindControl("lblComAgrID")).Text;
            // Function to Bind the lblAuditPremium ListView With Subject Data
            BindSubPrem("SUBJECT");
            lstSubject.Enabled = ((LinkButton)e.Item.FindControl("lnkEdit")).Enabled;
        }
        else if (e.CommandName.ToUpper() == "NONSUBJECT")
        {
            ViewState["COM_AGR_AUD_ID"] = e.CommandArgument;
            ViewState["COM_AGR_ID"] = ((Label)e.Item.FindControl("lblComAgrID")).Text;
            // Function to Bind the lblAuditPremium ListView with NonSubject Data
            BindSubPrem("NONSUBJECT");
            lstNonSubject.Enabled = ((LinkButton)e.Item.FindControl("lnkEdit")).Enabled;
        }
        else if (e.CommandName.ToUpper() == "ISACTIVE")
        {
            bool Flag = bool.Parse(((HiddenField)e.Item.FindControl("hidActive")).Value);
            if (Flag)
            {
                // Function to Disable or Enable the Record
                DisableorEnableRow(e.Item, Convert.ToInt32(e.CommandArgument), lst, false);
            }
            else
            {
                // Function to Disable or Enable the Record
                DisableorEnableRow(e.Item, Convert.ToInt32(e.CommandArgument), lst, true);
            }
        }
        else if (e.CommandName.ToUpper() == "REVISE")
        {
            // Function to Revise the Audit Record
            AuditRevise(e.Item, Convert.ToInt32(e.CommandArgument));
        }
    }
    /// <summary>
    /// Function to Revise the Audit Record
    /// </summary>
    /// <param name="e"></param>
    /// <param name="CommAgrAudID"></param>
    public void AuditRevise(ListViewItem e, int CommAgrAudID)
    {
        Label lblStartDate = (Label)e.FindControl("lblStartDate");
        Label lblDefdepAmt = (Label)e.FindControl("lblDefdepAmt");
        Label lblSubAmt = (Label)e.FindControl("lblSubAmt");
        Label lblNonsubAmt = (Label)e.FindControl("lblNonsubAmt");
        Label lblExpAmt = (Label)e.FindControl("lblExpAmt");
        Label lblComAgrID = (Label)e.FindControl("lblComAgrID");
        LinkButton lnkSubPrmAmt = (LinkButton)e.FindControl("lnkSubPrmAmt");
        LinkButton lnkNonSubPrmAmt = (LinkButton)e.FindControl("lnkNonSubPrmAmt");

        ComAgrAudBE = CommAgrAudService.getCommAgrRow(CommAgrAudID);
        ComAgrAudBE.Aud_Rev_Status = true;
        ComAgrAudBE.UpdatedDate = DateTime.Now;
        ComAgrAudBE.UpdatedUser_ID = CurrentAISUser.PersonID;
        bool Flag = CommAgrAudService.Update(ComAgrAudBE);
        ShowConcurrentConflict(Flag, ComAgrAudBE.ErrorMessage);
        //----------code to add new record with the revised record values
        ComAgrAudBE = new Coml_Agmt_AuDtBE();
        ComAgrAudBE.Comm_Agr_ID = Convert.ToInt32(lblComAgrID.Text);
        ComAgrAudBE.Prem_Adj_Prg_ID = int.Parse(ViewState["ProgramPeriodID"].ToString());
        ComAgrAudBE.Customer_ID = AISMasterEntities.AccountNumber;
        ComAgrAudBE.StartDate = DateTime.Parse(lblStartDate.Text);
        ComAgrAudBE.Sub_Aud_Prm_Amt = decimal.Parse(lnkSubPrmAmt.Text);
        ComAgrAudBE.AdjustmentIndicator = false;
        ComAgrAudBE.Aud_Rev_Status = false;
        ComAgrAudBE.Non_Sub_Aud_Prm_Amt = decimal.Parse(lnkNonSubPrmAmt.Text);
        ComAgrAudBE.Def_Dep_prm_Amt = decimal.Parse(lblDefdepAmt.Text);
        ComAgrAudBE.Sub_Dep_Prm_Amt = decimal.Parse(lblSubAmt.Text);
        ComAgrAudBE.Non_Sub_Dep_Prm_Amt = decimal.Parse(lblNonsubAmt.Text);
        ComAgrAudBE.Audit_Reslt_Amt = (ComAgrAudBE.Sub_Aud_Prm_Amt + ComAgrAudBE.Non_Sub_Aud_Prm_Amt) - (ComAgrAudBE.Def_Dep_prm_Amt + ComAgrAudBE.Sub_Dep_Prm_Amt + ComAgrAudBE.Non_Sub_Dep_Prm_Amt);
        ComAgrAudBE.ExposureAmt = decimal.Parse(lblExpAmt.Text);
        ComAgrAudBE.CreatedDate = DateTime.Now;
        ComAgrAudBE.CreatedUser_ID = CurrentAISUser.PersonID;
        Flag = CommAgrAudService.Update(ComAgrAudBE);
        if (Flag)
        {
            ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
            audtBS.Save(AISMasterEntities.AccountNumber, int.Parse(ViewState["ProgramPeriodID"].ToString()), GlobalConstants.AuditingWebPage.AuditInormation, CurrentAISUser.PersonID);
            //code to copy the subaudit premium records
            IList<SubjectAuditPremiumBE> SubAuditBE = SubAudPremService.getsubPremAudList(CommAgrAudID);
            SubjectAuditPremiumBE newSubjAuditBE;
            for (int i = 0; i < SubAuditBE.Count; i++)
            {
                newSubjAuditBE = new SubjectAuditPremiumBE();
                newSubjAuditBE.Coml_Agmt_Audt_ID = ComAgrAudBE.Comm_Agr_Audit_ID;
                newSubjAuditBE.Coml_Agmt_ID = SubAuditBE[i].Coml_Agmt_ID;
                newSubjAuditBE.Prem_Adj_Pgm_ID = SubAuditBE[i].Prem_Adj_Pgm_ID;
                newSubjAuditBE.Custmr_ID = SubAuditBE[i].Custmr_ID;
                newSubjAuditBE.StateID = SubAuditBE[i].StateID;
                newSubjAuditBE.Prem_Amt = SubAuditBE[i].Prem_Amt;
                newSubjAuditBE.Active = true;
                newSubjAuditBE.CreatedDate = DateTime.Now;
                newSubjAuditBE.CreatedUser_ID = CurrentAISUser.PersonID;
                Flag = SubAudPremService.Update(newSubjAuditBE);

            }
            //code to copy the Nonsubaudit premium records
            IList<NonSubjectAuditPremiumBE> NonSubAuditBE = NonSubAudPremService.getNonsubPremAudList(CommAgrAudID);
            NonSubjectAuditPremiumBE newNonSubjAuditBE;
            for (int i = 0; i < NonSubAuditBE.Count; i++)
            {
                newNonSubjAuditBE = new NonSubjectAuditPremiumBE();
                newNonSubjAuditBE.Coml_Agmt_Audt_ID = ComAgrAudBE.Comm_Agr_Audit_ID;
                newNonSubjAuditBE.Coml_Agmt_ID = NonSubAuditBE[i].Coml_Agmt_ID;
                newNonSubjAuditBE.Prem_Adj_Pgm_ID = NonSubAuditBE[i].Prem_Adj_Pgm_ID;
                newNonSubjAuditBE.Custmr_ID = NonSubAuditBE[i].Custmr_ID;
                newNonSubjAuditBE.Nsa_Typ_ID = NonSubAuditBE[i].Nsa_Typ_ID;
                newNonSubjAuditBE.Non_Subj_Audt_Prem_Amt = NonSubAuditBE[i].Non_Subj_Audt_Prem_Amt;
                newNonSubjAuditBE.Active = true;
                newNonSubjAuditBE.CreatedDate = DateTime.Now;
                newNonSubjAuditBE.CreatedUser_ID = CurrentAISUser.PersonID;
                Flag = NonSubAudPremService.Update(newNonSubjAuditBE);
            }
            /// Function to Update Retroinfo AuditExpAmt values for the selected Program Period
            UpdateRetroInfo();
            /// Function to Bind the lstAuditInfo ListView 
            BindAuditinfoList(int.Parse(ViewState["ProgramPeriodID"].ToString()));
        }
    }
    /// <summary>
    /// Function to Disable or Enable the Record
    /// </summary>
    /// <param name="e"></param>
    /// <param name="CommAgrAudID"></param>
    /// <param name="lst"></param>
    /// <param name="Status"></param>
    protected void DisableorEnableRow(ListViewItem e, int CommAgrAudID, ListView lst, bool Status)
    {
        if (lst.ID.ToString() == "lstSubject")
        {
            SubAudPremBE = SubAudPremService.getsubPremAudRow(CommAgrAudID);
            SubAudPremBE.Active = Status;
            bool Flag = SubAudPremService.Update(SubAudPremBE);
            lstSubject.EditIndex = -1;
            //lblTotalSubPremAmt.Text = "0";
            if (Flag)
            {
                // Function to Bind the lblAuditPremium ListView with Subject Data
                BindSubPrem("SUBJECT");
            }
            // Code for Calculating Total of SubjectAuditPremium,NonSubjectAuditPremium and Audit Result
            CommercialAgreementTotal("SUBJECT");




        }
        else
        {
            NonComAgrPrmBE = NonSubAudPremService.getsubPremAudRow(CommAgrAudID);
            NonComAgrPrmBE.Active = Status;
            bool Flag = NonSubAudPremService.Update(NonComAgrPrmBE);
            lstNonSubject.EditIndex = -1;
            //Label LblTotSubPermAmt = ((Label)lstSubject.FindControl("lblTotalSubPremAmt"));
            //if (LblTotSubPermAmt != null)
            //{
            //    //((Label)lstNonSubject.FindControl("lblTotalSubPremAmt")).Text = "0";
            //    lblTotalSubPremAmt.Text = "0";
            //}
            if (Flag)
            {
                // Function to Bind the lblAuditPremium ListView with Non Subject Data

                BindSubPrem("NONSUBJECT");
            }
            // Code for Calculating Total of SubjectAuditPremium,NonSubjectAuditPremium and Audit Result
            CommercialAgreementTotal("NONSUBJECT");

        }

    }
    /// <summary>
    /// Invoked when the Edit Link is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void EditList(Object sender, ListViewEditEventArgs e)
    {
        ListView lst = (ListView)sender;
        if (lst.ID.ToString() == "lstAuditInfo")
        {
            lstAuditInfo.EditIndex = e.NewEditIndex;
            /// Function to Bind the lstAuditInfo ListView 
            BindAuditinfoList(int.Parse(ViewState["ProgramPeriodID"].ToString()));
            ((LinkButton)lstAuditInfo.InsertItem.FindControl("lnkSave")).Enabled = false;

        }
        else if (lst.ID.ToString() == "lstSubject")
        {
            lstSubject.EditIndex = e.NewEditIndex;
            // Function to Bind the lblAuditPremium ListView with Subject Data

            BindSubPrem("SUBJECT");
            ((LinkButton)lstSubject.InsertItem.FindControl("lnkSave")).Enabled = false;

        }
        else if (lst.ID.ToString() == "lstNonSubject")
        {
            lstNonSubject.EditIndex = e.NewEditIndex;
            // Function to Bind the lblAuditPremium ListView with Non Subject Data

            BindSubPrem("NONSUBJECT");
            ((LinkButton)lstNonSubject.InsertItem.FindControl("lnkSave")).Enabled = false;
        }

    }
    /// <summary>
    /// Invoked when the Cancel Link is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CancelList(Object sender, ListViewCancelEventArgs e)
    {
        ListView lst = (ListView)sender;
        if (lst.ID.ToString() == "lstAuditInfo")
        {
            lstAuditInfo.EditIndex = -1;
            /// Function to Bind the lstAuditInfo ListView 
            BindAuditinfoList(int.Parse(ViewState["ProgramPeriodID"].ToString()));
        }
        else if (lst.ID.ToString() == "lstSubject")
        {
            lstSubject.EditIndex = -1;
            // Function to Bind the lblAuditPremium ListView with Subject Data

            BindSubPrem("SUBJECT");
        }
        else if (lst.ID.ToString() == "lstNonSubject")
        {
            lstNonSubject.EditIndex = -1;
            // Function to Bind the lblAuditPremium ListView with Non Subject Data

            BindSubPrem("NONSUBJECT");
        }
    }

    /// <summary>
    /// Compare and check if the policy information you are trying to save already exists or not
    /// </summary>
    /// <param name="AccountNumber"></param>
    /// <param name="PrgmPeriod"></param>
    /// <returns></returns>
    protected bool CompareValues(int AccountNumber, int PrgmPeriod, int policyid)
    {
        bool retValue = false;
        Coml_Agmt_AuDtBS ComlAudtBS = new Coml_Agmt_AuDtBS();
        IList<Coml_Agmt_AuDtBE> ComlAudtBE = ComlAudtBS.getCommAgrAuditList(AccountNumber, PrgmPeriod);
        foreach (Coml_Agmt_AuDtBE ComlAudtRowBE in ComlAudtBE)
            if (ComlAudtRowBE.Comm_Agr_ID == policyid)
            {
                retValue = true;
                break;
            }
        return retValue;
    }

    /// <summary>
    /// Invoked when the Update Link is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateAuditList(Object sender, ListViewUpdateEventArgs e)
    {
        ListViewItem myItem = lstAuditInfo.Items[e.ItemIndex];
        bool isAuditRequired = false;
        bool PolicyChanges = true;
        int intComlAgrAudtID = int.Parse(((HiddenField)myItem.FindControl("hidCommAgrAuditID")).Value.ToString());
        ComAgrAudBE = CommAgrAudService.getCommAgrRow(intComlAgrAudtID);
        // Concurrency Code
        Coml_Agmt_AuDtBE comlAgrAudtBEOld = (ComAgrAudBEList.Where(o => o.Comm_Agr_Audit_ID.Equals(intComlAgrAudtID))).First();
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(comlAgrAudtBEOld.UpdatedDate), Convert.ToDateTime(ComAgrAudBE.UpdatedDate));
        if (!Concurrency)
            return;
        bool changePolicy = false;
        //QA4869 fix Check if the policy information you are trying to save already exists or not - Rolled Back
        if (((DropDownList)myItem.FindControl("ddlPolicyEdt")).Visible == true)
        {
            if (ComAgrAudBE.Comm_Agr_ID != Convert.ToInt32(((DropDownList)myItem.FindControl("ddlPolicyEdt")).SelectedItem.Value))
            {
                if (CompareValues(AISMasterEntities.AccountNumber, int.Parse(ViewState["ProgramPeriodID"].ToString()), Convert.ToInt32(((DropDownList)myItem.FindControl("ddlPolicyEdt")).SelectedItem.Value)))
                {
                    PolicyChanges = false;

                }
                else
                {
                    PolicyChanges = true;
                    isAuditRequired = true;
                    changePolicy = true;
                }
            }
        }
        //QA4869 fix 
        if (PolicyChanges == true)
        {
            if (ComAgrAudBE.StartDate.ToShortDateString() != DateTime.Parse(((TextBox)myItem.FindControl("txtAuditDate")).Text).ToShortDateString())
            {
                isAuditRequired = true;
            }
            ComAgrAudBE.StartDate = DateTime.Parse(((TextBox)myItem.FindControl("txtAuditDate")).Text);
            //ComAgrAudBE.Sub_Aud_Prm_Amt = 0;
            //ComAgrAudBE.Non_Sub_Aud_Prm_Amt = 0;
            //QA4869 fix 
            if (((DropDownList)myItem.FindControl("ddlPolicyEdt")).Visible == true)
                ComAgrAudBE.Comm_Agr_ID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlPolicyEdt")).SelectedItem.Value);
            //QA4869 fix 
            //QA4871 fix 
            string StrValueDepprem = (((TextBox)myItem.FindControl("txtdefdepprem")).Text.Replace(",", "")).Trim();
            if (StrValueDepprem.Length > 0)
            {
                if (!isAuditRequired && ComAgrAudBE.Def_Dep_prm_Amt != decimal.Parse(((TextBox)myItem.FindControl("txtdefdepprem")).Text.Replace(",", "")))
                {
                    isAuditRequired = true;
                }
            }
            else
            {
                if (!isAuditRequired && ComAgrAudBE.Def_Dep_prm_Amt != 0)
                {
                    isAuditRequired = true;
                }
            }
            string StrValueDefprem = (((TextBox)myItem.FindControl("txtsubdepPrem")).Text.Replace(",", "")).Trim();
            if (StrValueDefprem.Length > 0)
            {
                if (!isAuditRequired && ComAgrAudBE.Sub_Dep_Prm_Amt != decimal.Parse(((TextBox)myItem.FindControl("txtsubdepPrem")).Text.Replace(",", "")))
                {
                    isAuditRequired = true;
                }
            }
            else
            {
                if (!isAuditRequired && ComAgrAudBE.Sub_Dep_Prm_Amt != 0)
                {
                    isAuditRequired = true;
                }

            }
            string StrValueNonDefprem = (((TextBox)myItem.FindControl("txtNSubDepPrem")).Text.Replace(",", "")).Trim();
            if (StrValueNonDefprem.Length > 0)
            {
                if (!isAuditRequired && ComAgrAudBE.Non_Sub_Dep_Prm_Amt != decimal.Parse(((TextBox)myItem.FindControl("txtNSubDepPrem")).Text.Replace(",", "")))
                {
                    isAuditRequired = true;
                }
            }
            else
            {
                if (!isAuditRequired && ComAgrAudBE.Non_Sub_Dep_Prm_Amt != 0)
                {
                    isAuditRequired = true;
                }
            }
            if (!isAuditRequired && ComAgrAudBE.ExposureAmt != decimal.Parse(((TextBox)myItem.FindControl("txtExpAmt")).Text.Replace(",", "")))
            {
                isAuditRequired = true;
            }
            if (isAuditRequired)
            {
                if (((TextBox)myItem.FindControl("txtdefdepprem")).Text.Replace(",", "") != "")
                {
                    ComAgrAudBE.Def_Dep_prm_Amt = decimal.Parse(((TextBox)myItem.FindControl("txtdefdepprem")).Text.Replace(",", ""));
                }
                else
                {
                    ComAgrAudBE.Def_Dep_prm_Amt = 0;
                }
                if (((TextBox)myItem.FindControl("txtsubdepPrem")).Text.Replace(",", "") != "")
                {
                    ComAgrAudBE.Sub_Dep_Prm_Amt = decimal.Parse(((TextBox)myItem.FindControl("txtsubdepPrem")).Text.Replace(",", ""));
                }
                else
                {
                    ComAgrAudBE.Sub_Dep_Prm_Amt = 0;
                }
                if (((TextBox)myItem.FindControl("txtNSubDepPrem")).Text.Replace(",", "") != "")
                {
                    ComAgrAudBE.Non_Sub_Dep_Prm_Amt = decimal.Parse(((TextBox)myItem.FindControl("txtNSubDepPrem")).Text.Replace(",", ""));
                }
                else
                {
                    ComAgrAudBE.Non_Sub_Dep_Prm_Amt = 0;
                }
                ComAgrAudBE.Audit_Reslt_Amt = (ComAgrAudBE.Sub_Aud_Prm_Amt + ComAgrAudBE.Non_Sub_Aud_Prm_Amt) - (ComAgrAudBE.Def_Dep_prm_Amt + ComAgrAudBE.Sub_Dep_Prm_Amt + ComAgrAudBE.Non_Sub_Dep_Prm_Amt);
                ComAgrAudBE.ExposureAmt = decimal.Parse(((TextBox)myItem.FindControl("txtExpAmt")).Text.Replace(",", ""));
                ComAgrAudBE.UpdatedDate = DateTime.Now;
                ComAgrAudBE.UpdatedUser_ID = CurrentAISUser.PersonID;

                bool Flag = CommAgrAudService.Update(ComAgrAudBE);
                ShowConcurrentConflict(Flag, ComAgrAudBE.ErrorMessage);
                if (Flag)
                {
                    //Code for updating SUbject Premium and Non Subject Premium PolicyId when Commercial Agreement Audit Policy ID is changed.
                    if (changePolicy)
                    {
                        IList<SubjectAuditPremiumBE> SubjectBE = (new Sub_Audt_PremBS()).getsubPremAudList(ComAgrAudBE.Comm_Agr_Audit_ID);
                        for (int i = 0; i < SubjectBE.Count; i++)
                        {
                            SubjectAuditPremiumBE SubBE = (new Sub_Audt_PremBS()).getsubPremAudRow(SubjectBE[i].Sub_Prem_Aud_ID);
                            SubBE.Coml_Agmt_ID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlPolicyEdt")).SelectedItem.Value);
                            SubBE.UpdatedDate = DateTime.Now;
                            SubBE.UpdatedUser_ID = CurrentAISUser.PersonID;
                            (new Sub_Audt_PremBS()).Update(SubBE);
                        }
                        IList<NonSubjectAuditPremiumBE> NonSubjectBE = (new Non_Sub_Audt_PremBS()).getNonsubPremAudList(ComAgrAudBE.Comm_Agr_Audit_ID);
                        for (int i = 0; i < NonSubjectBE.Count; i++)
                        {
                            NonSubjectAuditPremiumBE NonSubBE = (new Non_Sub_Audt_PremBS()).getsubPremAudRow(NonSubjectBE[i].N_Sub_Prem_Aud_ID);
                            NonSubBE.Coml_Agmt_ID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlPolicyEdt")).SelectedItem.Value);
                            NonSubBE.UpdatedDate = DateTime.Now;
                            NonSubBE.UpdatedUser_ID = CurrentAISUser.PersonID;
                            (new Non_Sub_Audt_PremBS()).Update(NonSubBE);
                        }
                    }
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, int.Parse(ViewState["ProgramPeriodID"].ToString()), GlobalConstants.AuditingWebPage.AuditInormation, CurrentAISUser.PersonID);
                    /// Function to Bind the lstAuditInfo ListView 
                    /// Function to Update Retroinfo AuditExpAmt values for the selected Program Period
                    UpdateRetroInfo();
                }
            }
            else
            {
                //sShowMessage("No information has been changed to Save");
            }
            lstAuditInfo.EditIndex = -1;
            BindAuditinfoList(int.Parse(ViewState["ProgramPeriodID"].ToString()));
        }
        else
        {
            ShowMessage("The Audit information for this Policy number you are trying to save already exists");
        }

    }
    /// <summary>
    /// Invoked when the Update Link of lstSubject Listview is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateSubjectList(Object sender, ListViewUpdateEventArgs e)
    {
        ListViewItem myItem = lstSubject.Items[e.ItemIndex];
        int intSubID = int.Parse(((HiddenField)myItem.FindControl("hidSub")).Value.ToString());
        SubAudPremBE = SubAudPremService.getsubPremAudRow(intSubID);
        // Concurrency Code
        SubjectAuditPremiumBE subPremBEOld = (SubAudPremBEList.Where(o => o.Sub_Prem_Aud_ID.Equals(intSubID))).First();
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(subPremBEOld.UpdatedDate), Convert.ToDateTime(SubAudPremBE.UpdatedDate));
        if (!Concurrency)
            return;
        
        SubAudPremBE.StateID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlState")).SelectedItem.Value);
        SubAudPremBE.Prem_Amt = decimal.Parse(((TextBox)myItem.FindControl("txtPremiumAmount")).Text.Replace(",", ""));
        SubAudPremBE.UpdatedDate = DateTime.Now;
        SubAudPremBE.UpdatedUser_ID = CurrentAISUser.PersonID;
        bool Flag = SubAudPremService.Update(SubAudPremBE);
        ShowConcurrentConflict(Flag, SubAudPremBE.ErrorMessage);
        lstSubject.EditIndex = -1;
        if (Flag)
        {
            // Function to Bind the lblAuditPremium ListView with Subject Data

            BindSubPrem("SUBJECT");
        }
        // Code for Calculating Total of SubjectAuditPremium,NonSubjectAuditPremium and Audit Result
        CommercialAgreementTotal("SUBJECT");
    }
    /// <summary>
    /// Invoked when the Update Link of ListView lstNonSubject is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateNonSubjectList(Object sender, ListViewUpdateEventArgs e)
    {
        ListViewItem myItem = lstNonSubject.Items[e.ItemIndex];
        int intNSubID = int.Parse(((HiddenField)myItem.FindControl("hidNonSub")).Value.ToString());
        NonComAgrPrmBE = NonSubAudPremService.getsubPremAudRow(intNSubID);
        NonSubjectAuditPremiumBE NonsubPremBEOld = (NonComAgrPrmBEList.Where(o => o.N_Sub_Prem_Aud_ID.Equals(intNSubID))).First();
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(NonsubPremBEOld.UpdatedDate), Convert.ToDateTime(NonComAgrPrmBE.UpdatedDate));
        if (!Concurrency)
            return;
        NonComAgrPrmBE.Nsa_Typ_ID = Convert.ToInt32(((DropDownList)myItem.FindControl("ddlNonSub")).SelectedItem.Value);
        NonComAgrPrmBE.Non_Subj_Audt_Prem_Amt = decimal.Parse(((TextBox)myItem.FindControl("txtPremiumAmount")).Text.Replace(",", ""));
        NonComAgrPrmBE.UpdatedDate = DateTime.Now;
        NonComAgrPrmBE.UpdatedUser_ID = CurrentAISUser.PersonID;
        bool Flag = NonSubAudPremService.Update(NonComAgrPrmBE);
        ShowConcurrentConflict(Flag, NonComAgrPrmBE.ErrorMessage);
        lstNonSubject.EditIndex = -1;
        if (Flag)
        {
            // Function to Bind the lblAuditPremium ListView with Non Subject Data

            BindSubPrem("NONSUBJECT");
        }
        // Code for Calculating Total of SubjectAuditPremium,NonSubjectAuditPremium and Audit Result
        CommercialAgreementTotal("NONSUBJECT");
    }
    /// <summary>
    /// Invoked when the Save Link of Listview lstAuditInfo is clicked
    /// </summary>
    /// <param name="e"></param>
    protected void SaveAuditList(ListViewItem e)
    {
        //QA4869 fix Check if the policy information you are trying to save already exists or not
        bool PolicyChanges;
        if (CompareValues(AISMasterEntities.AccountNumber, int.Parse(ViewState["ProgramPeriodID"].ToString()), Convert.ToInt32(((DropDownList)e.FindControl("ddlPolicy")).SelectedItem.Value)))
        {
            PolicyChanges = false;

        }
        else
        {
            PolicyChanges = true;
        }
        if (PolicyChanges == true)
        {
            //QA4869 fix
            ComAgrAudBE = new Coml_Agmt_AuDtBE();
            ComAgrAudBE.Comm_Agr_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlPolicy")).SelectedItem.Value);
            ComAgrAudBE.Prem_Adj_Prg_ID = int.Parse(ViewState["ProgramPeriodID"].ToString());
            ComAgrAudBE.Customer_ID = AISMasterEntities.AccountNumber;
            ComAgrAudBE.StartDate = DateTime.Parse(((TextBox)e.FindControl("txtAuditDate")).Text);
            ComAgrAudBE.Sub_Aud_Prm_Amt = 0;
            ComAgrAudBE.Aud_Rev_Status = false;
            ComAgrAudBE.AdjustmentIndicator = false;
            ComAgrAudBE.Non_Sub_Aud_Prm_Amt = 0;
            if (((TextBox)e.FindControl("txtdefdepprem")).Text.Replace(",", "") != "")
            {
                ComAgrAudBE.Def_Dep_prm_Amt = decimal.Parse(((TextBox)e.FindControl("txtdefdepprem")).Text.Replace(",", ""));
            }
            else
            {
                ComAgrAudBE.Def_Dep_prm_Amt = 0;
            }
            if (((TextBox)e.FindControl("txtsubdepPrem")).Text.Replace(",", "") != "")
            {
                ComAgrAudBE.Sub_Dep_Prm_Amt = decimal.Parse(((TextBox)e.FindControl("txtsubdepPrem")).Text.Replace(",", ""));
            }
            else
            {
                ComAgrAudBE.Sub_Dep_Prm_Amt = 0;
            }
            if (((TextBox)e.FindControl("txtNSubDepPrem")).Text.Replace(",", "") != "")
            {
                ComAgrAudBE.Non_Sub_Dep_Prm_Amt = decimal.Parse(((TextBox)e.FindControl("txtNSubDepPrem")).Text.Replace(",", ""));
            }
            else
            {
                ComAgrAudBE.Non_Sub_Dep_Prm_Amt = 0;
            }
            ComAgrAudBE.Audit_Reslt_Amt = (ComAgrAudBE.Sub_Aud_Prm_Amt + ComAgrAudBE.Non_Sub_Aud_Prm_Amt) - (ComAgrAudBE.Def_Dep_prm_Amt + ComAgrAudBE.Sub_Dep_Prm_Amt + ComAgrAudBE.Non_Sub_Dep_Prm_Amt);
            ComAgrAudBE.ExposureAmt = decimal.Parse(((TextBox)e.FindControl("txtExpAmt")).Text.Replace(",", ""));
            ComAgrAudBE.CreatedDate = DateTime.Now;
            ComAgrAudBE.CreatedUser_ID = CurrentAISUser.PersonID;
            bool Flag = CommAgrAudService.Update(ComAgrAudBE);
            if (Flag)
            {
                /// Function to Update Retroinfo AuditExpAmt values for the selected Program Period
                UpdateRetroInfo();
            }
            lstAuditInfo.EditIndex = -1;
            BindAuditinfoList(int.Parse(ViewState["ProgramPeriodID"].ToString()));
        }
        else
        {
            ShowMessage("The Audit information for this Policy number you are trying to save already exists");
        }

    }
    /// <summary>
    /// Function to Update Retroinfo AuditExpAmt values for the selected Program Period
    /// </summary>
    public void UpdateRetroInfo()
    {
        int prmPerdID = int.Parse(ViewState["ProgramPeriodID"].ToString());
        IList<RetroInfoBE> retroBE = (new RetroInfoBS()).GetRetroInfo(prmPerdID);
        bool Flag = false;
        decimal? dclMaxAmt = 0;
        for (int i = 0; i < retroBE.Count; i++)
        {
            if (retroBE[i].ADJ_RETRO_INFO_ID <= 0)
            {
                return;
            }
            RetroInfoBE retroinfoBE = (new RetroInfoBS()).LoadData(retroBE[i].ADJ_RETRO_INFO_ID);


            if (retroinfoBE.EXPO_TYP_ID == null || retroinfoBE.EXPO_TYP_ID <= 0)//if nothing is there in case of N/A checkbox
            {
                retroinfoBE.AUDT_EXPO_AMT = 0;
            }
            else if (retroinfoBE.EXPO_TYP_ID == 127 && i != 0)//Basic Premium
            {
                retroinfoBE.AUDT_EXPO_AMT = dclMaxAmt;
            }
            else if (retroinfoBE.EXPO_TYP_ID == 131)//payroll
            {
                retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetPayrollAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
            }
            else if (retroinfoBE.EXPO_TYP_ID == 128)//combined elements
            {
                retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetCombinedAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
            }
            else if (retroinfoBE.EXPO_TYP_ID == 135)//standard premium
            {
                retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetStandardAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
            }
            else if (retroinfoBE.EXPO_TYP_ID != 130)// Manual Premium
            {
                retroinfoBE.AUDT_EXPO_AMT = decimal.Parse((new RetroInfoBS()).GetOtherAuditExp(prmPerdID, AISMasterEntities.AccountNumber).ToString());
            }
            if (retroinfoBE.EXPO_TYP_INCREMNT_NBR_ID != null && retroinfoBE.EXPO_TYP_INCREMNT_NBR_ID > 0)
            {
                retroinfoBE.TOT_AUDT_AMT = Math.Round(decimal.Parse(((retroinfoBE.AUDT_EXPO_AMT * retroinfoBE.AGGR_FCTR_PCT) / decimal.Parse(retroBE[i].EXP_BASIS)).ToString()));
            }
            else
            {
                retroinfoBE.TOT_AUDT_AMT = null;
            }
            if (i == 0)
            {
                if (retroinfoBE.TOT_AGMT_AMT == null && retroinfoBE.TOT_AUDT_AMT == null)
                    dclMaxAmt = null;
                else
                {
                    // This will ensure that a null value will not be used in calculating the dclMaxAmt (that was previously causing errors)
                    decimal? TOT_AGMT_AMT = retroinfoBE.TOT_AGMT_AMT != null ? retroinfoBE.TOT_AGMT_AMT : 0;
                    decimal? TOT_AUDT_AMT = retroinfoBE.TOT_AUDT_AMT != null ? retroinfoBE.TOT_AUDT_AMT : 0;

                    dclMaxAmt = TOT_AGMT_AMT > TOT_AUDT_AMT ? Math.Round(decimal.Parse(TOT_AGMT_AMT.ToString())) : Math.Round(decimal.Parse(TOT_AUDT_AMT.ToString()));
                }
            }
            retroinfoBE.UPDT_DT = DateTime.Now;
            retroinfoBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            Flag = (new RetroInfoBS()).SaveRetroData(retroinfoBE);
        }
    }
    /// <summary>
    /// Invoked when the Save Link of ListView lstSubject is clicked
    /// </summary>
    /// <param name="e"></param>
    protected void SaveSubjectList(ListViewItem e)
    {
        int intpremAdjStsID = int.Parse(ViewState["COM_AGR_AUD_ID"].ToString());

        SubAudPremBE = new SubjectAuditPremiumBE();
        SubAudPremBE.Coml_Agmt_Audt_ID = intpremAdjStsID;
        SubAudPremBE.Coml_Agmt_ID = int.Parse(ViewState["COM_AGR_ID"].ToString());
        SubAudPremBE.Prem_Adj_Pgm_ID = int.Parse(ViewState["ProgramPeriodID"].ToString());
        SubAudPremBE.Custmr_ID = AISMasterEntities.AccountNumber;
        SubAudPremBE.StateID = Convert.ToInt32(((DropDownList)e.FindControl("ddlState")).SelectedItem.Value);
        SubAudPremBE.Prem_Amt = decimal.Parse(((TextBox)e.FindControl("txtPremiumAmount")).Text.Replace(",", ""));
        SubAudPremBE.Active = true;
        SubAudPremBE.CreatedDate = DateTime.Now;
        SubAudPremBE.CreatedUser_ID = CurrentAISUser.PersonID;
        bool Flag = SubAudPremService.Update(SubAudPremBE);
        if (Flag)
        {

            // Function to Bind the lblAuditPremium ListView with Subject Data
            //ComAgrAudBE = CommAgrAudService.getCommAgrRow(intpremAdjStsID);
            //ComAgrAudBE.AdjustmentIndicator = true;
            //Flag = CommAgrAudService.Update(ComAgrAudBE);
            BindSubPrem("SUBJECT");
        }
        // Code for Calculating Total of SubjectAuditPremium,NonSubjectAuditPremium and Audit Result
        CommercialAgreementTotal("SUBJECT");

    }
    /// <summary>
    /// Invoked when the Save Link of ListView lstNonSubject is clicked
    /// </summary>
    /// <param name="e"></param>
    protected void SaveNonSubjectList(ListViewItem e)
    {
        int intpremAdjStsID = int.Parse(ViewState["COM_AGR_AUD_ID"].ToString());

        NonComAgrPrmBE = new NonSubjectAuditPremiumBE();
        NonComAgrPrmBE.Coml_Agmt_Audt_ID = intpremAdjStsID;
        NonComAgrPrmBE.Coml_Agmt_ID = int.Parse(ViewState["COM_AGR_ID"].ToString());
        NonComAgrPrmBE.Prem_Adj_Pgm_ID = int.Parse(ViewState["ProgramPeriodID"].ToString());
        NonComAgrPrmBE.Custmr_ID = AISMasterEntities.AccountNumber;
        NonComAgrPrmBE.Nsa_Typ_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlNonSub")).SelectedItem.Value);
        NonComAgrPrmBE.Non_Subj_Audt_Prem_Amt = decimal.Parse(((TextBox)e.FindControl("txtPremiumAmount")).Text.Replace(",", ""));
        NonComAgrPrmBE.Active = true;
        NonComAgrPrmBE.CreatedDate = DateTime.Now;
        NonComAgrPrmBE.CreatedUser_ID = CurrentAISUser.PersonID;
        bool Flag = NonSubAudPremService.Update(NonComAgrPrmBE);
        if (Flag)
        {
            // Function to Bind the lblAuditPremium ListView with Non Subject Data
            //ComAgrAudBE = CommAgrAudService.getCommAgrRow(intpremAdjStsID);
            //ComAgrAudBE.AdjustmentIndicator = true;
            //Flag = CommAgrAudService.Update(ComAgrAudBE);
            BindSubPrem("NONSUBJECT");
        }
        // Code for Calculating Total of SubjectAuditPremium,NonSubjectAuditPremium and Audit Result
        CommercialAgreementTotal("NONSUBJECT");
    }
    /// <summary>
    /// Code for Calculating Total of SubjectAuditPremium,NonSubjectAuditPremium and Audit Result
    /// </summary>
    /// <param name="strCommercial"></param>
    protected void CommercialAgreementTotal(string strCommercial)
    {

        ComAgrAudBE = CommAgrAudService.getCommAgrRow(int.Parse(ViewState["COM_AGR_AUD_ID"].ToString()));
        if (strCommercial == "SUBJECT")
        {
            ComAgrAudBE.Sub_Aud_Prm_Amt = decimal.Parse(lblTotalSubPremAmt.Text);
        }
        else
        {
            //ComAgrAudBE.Non_Sub_Aud_Prm_Amt = decimal.Parse(((Label)lstNonSubject.FindControl("lblTotalSubPremAmt")).Text);
            ComAgrAudBE.Non_Sub_Aud_Prm_Amt = decimal.Parse(lblTotalSubPremAmt.Text);
        }
        ComAgrAudBE.Audit_Reslt_Amt = (ComAgrAudBE.Sub_Aud_Prm_Amt + ComAgrAudBE.Non_Sub_Aud_Prm_Amt) - (ComAgrAudBE.Def_Dep_prm_Amt + ComAgrAudBE.Sub_Dep_Prm_Amt + ComAgrAudBE.Non_Sub_Dep_Prm_Amt);
        bool Flag = CommAgrAudService.Update(ComAgrAudBE);
        /// Function to Update Retroinfo AuditExpAmt values for the selected Program Period
        UpdateRetroInfo();
        /// Function to Bind the lstAuditInfo ListView 
        BindAuditinfoList(int.Parse(ViewState["ProgramPeriodID"].ToString()));
    }
}
