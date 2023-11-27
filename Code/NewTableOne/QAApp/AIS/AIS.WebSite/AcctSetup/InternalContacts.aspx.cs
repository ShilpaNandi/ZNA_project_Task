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

public partial class AppMgmt_InternalContacts : AISBasePage
{
    private PersonBS personService;
    private PostAddressBS postAddressService;
    /// <summary>
    /// property to hold an instance for Business Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISBusinessTransaction property</returns>
    protected AISBusinessTransaction PersonTransactionWrapper
    {
        get
        {
            if ((AISBusinessTransaction)Session["PersonTransaction"] == null)
                Session["PersonTransaction"] = new AISBusinessTransaction();
            return (AISBusinessTransaction)Session["PersonTransaction"];
        }
        set
        {
            Session["PersonTransaction"] = value;
        }
    }
    /// <summary>
    /// a property for Person Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PersonBS</returns>
    private PersonBS PersonService
    {
        get
        {
            if (personService == null)
            {
                personService = new PersonBS();
                personService.AppTransactionWrapper = PersonTransactionWrapper;
            }
            return personService;
        }
    }
    /// <summary>
    /// a property for Postal Address Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostAddressBS</returns>
    private PostAddressBS PostAddressService
    {
        get
        {
            if (postAddressService == null)
            {
                postAddressService = new PostAddressBS();
                postAddressService.AppTransactionWrapper = PersonTransactionWrapper;
            }
            return postAddressService;
        }
    }
    /// <summary>
    /// a property for Person Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PersonBE</returns>
    private PersonBE personBE
    {
        get { return (PersonBE)Session["PERSONBE"]; }
        set { Session["PERSONBE"] = value; }
    }
    /// <summary>
    /// a property for Postal Address Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostalAddressBE</returns>
    private PostalAddressBE postAddressBE
    {
        get { return (PostalAddressBE)Session["POSTADDRESSBE"]; }
        set { Session["POSTADDRESSBE"] = value; }
    }
    /// <summary>
    /// PageLoad Event code appears here
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    protected void Page_Load(object sender, EventArgs e)
    {
        // To display the title of the page
        this.Master.Page.Title = "Internal Masters";
        if (!IsPostBack)
        {
            ddlSearchContact.Enabled = true;
            personBE = new PersonBE();
            postAddressBE = new PostalAddressBE();
            PersonTransactionWrapper = new AISBusinessTransaction();
        }

        //Checks Exiting without Save
        CheckWithoutSave();
    }

    private void CheckWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtAddress1);
        list.Add(txtAddress2);
        list.Add(txtCity);
        list.Add(txtEmail);
        list.Add(txtFax);
        list.Add(txtFirstName);
        list.Add(txtLastName);
        list.Add(txtTelephone);
        list.Add(txtUserID);
        list.Add(txtValidFrom);
        list.Add(txtValidTo);
        list.Add(txtZipCode);
        list.Add(calValidFrom);
        list.Add(calValidTO);
        list.Add(ddlActSetup);
        list.Add(ddlAdj);
        list.Add(ddlAries);
        list.Add(ddlContactType);
        list.Add(ddlManager);
        list.Add(ddlState);
        list.Add(ddlTitle);
        list.Add(btnAdd);
        list.Add(btnCancel);
        list.Add(btnCopy);
        list.Add(btnSave);
        list.Add(btnSearch);
        list.Add(lnkClose);
        ProcessExitFlag(list);
    }
    /// <summary>
    /// bool Function to check the record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNew
    {
        get { return (bool)ViewState["CheckNew"]; }
        set { ViewState["CheckNew"] = value; }
    }
    /// <summary>
    /// Protected Property for storing Selectedvalue of Internal contacts list
    /// </summary>
    /// <param name=""></param>
    /// <returns>integer Selected value</returns>
    protected int SelectedValue
    {
        get
        {
            return (hidindex.Value != null ? Convert.ToInt32(hidindex.Value) : -1);
        }
        set { hidindex.Value = value.ToString(); }
    }
    protected void btnSearch_Click(object obj, EventArgs e)
    {
        // Function call to bind the Internal Contacts list
        BindListView();
        ViewState["SelectedIndex"] = null;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        pnlDetails.Enabled = true;
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            if (lstInternalMasters.Items.Count > index)
            {
                HtmlTableRow tr = (HtmlTableRow)lstInternalMasters.Items[index].FindControl("trItemTemplate");
                tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
                ViewState["SelectedIndex"] = null;
            }
        }
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            btnSave.Enabled = true;
        btnSave.Text = "Save";
        btnAdd.Enabled = false;
        btnSearch.Enabled = false;
        lnkClose.Visible = true;
        lstInternalMasters.Enabled = false;
        ddlContactType.SelectedIndex = -1;
        ddlManager.SelectedIndex = -1;
        ddlState.SelectedIndex = -1;
        ddlTitle.SelectedIndex = -1;
        ddlActSetup.SelectedIndex = -1;
        ddlAdj.SelectedIndex = -1;
        ddlAries.SelectedIndex = -1;
        CheckNew = true;
        //Function call to clear all the control values
        ClearFileds();
        //Function call to Bind the Manager DropDown List
        FillManagers();
        pnlDetails.Visible = true;
        lblInternalContactsDetails.Visible = true;
    }
    protected void btnSave_Click(Object sender, EventArgs e)
    {
        try
        {
            // Remove symbols (dash, space and parentheses)
            string zipLength = Regex.Replace(txtZipCode.Text, @"[- ()_]", String.Empty);
            if ((zipLength.Length != 5) && (zipLength.Length != 9))
            {
                ShowMessage("Please enter 5 or 9 digits for Zip Code  in the format of 99999-9999 ");
                return;
            }
            else
            {
                if (CheckNew == true)
                {
                    personBE = new PersonBE();
                    personBE.CREATEDDATE = DateTime.Now;
                    personBE.CREATEDUSER_ID = CurrentAISUser.PersonID;
                    personBE.ACTIVE = true;

                    postAddressBE = new PostalAddressBE();
                    postAddressBE.CREATEDDATE = DateTime.Now;
                    postAddressBE.CREATEDUSER = CurrentAISUser.PersonID;
                }
                else
                {
                    personBE.UPDATEDDATE = DateTime.Now;
                    personBE.UPDATEDUSER_ID = CurrentAISUser.PersonID;

                    postAddressBE.UPDATEDDATE = DateTime.Now;
                    postAddressBE.UPDATEDUSER = CurrentAISUser.PersonID;
                }
                //Function to Check wether audit is needed or not
                bool auditneeded = false;
                if (!CheckNew)
                {
                    auditneeded = CheckEntity(personBE, postAddressBE);
                    if (!auditneeded)
                    {
                        valSummInternal.Style["display"] = "none";
                        return;
                    }
                }
                //Function to assign Textbox values to PersonBE and  PostaddrBE
                AssignEntity(personBE, postAddressBE);
                //Function to Check wether duplicate entry exists or not
                bool Result = false;
                    //PersonService.IsExistsInternalContact(personBE.FORENAME, personBE.SURNAME, personBE.TELEPHONE1, personBE.EMAIL, personBE.PERSON_ID);
                if (!Result)
                {

                    Result = PersonService.Update(personBE);
                    if (Result)
                    {
                        bool chkflg = PersonTransactionWrapper.SubmitTransactionChanges();
                        ShowConcurrentConflict(chkflg, PersonTransactionWrapper.ErrorMessage);
                        Result = PostAddressService.Update(postAddressBE, personBE.PERSON_ID);
                    }
                    if (Result)
                    {
                        if (auditneeded)
                        {
                            //Code for logging into Audit Transaction Table 
                            ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                            audtBS.Save(GlobalConstants.AuditingWebPage.InternalMasters, CurrentAISUser.PersonID);
                        }
                        bool pchkflg = PersonTransactionWrapper.SubmitTransactionChanges();
                        ShowConcurrentConflict(pchkflg, PersonTransactionWrapper.ErrorMessage);
                        //Function to bind the Listview with Internal Contacts
                        BindListView();
                        //valSummInternal.BorderWidth = Unit.Pixel(0);
                        valSummInternal.Style["display"] = "none";
                        btnSave.Text = "Update";
                    }
                    else
                    {
                        PersonTransactionWrapper.RollbackChanges();
                    }
                }
                //else
                //{
                //    ShowMessage("The record cannot be saved. An identical record already exists.");
                //    return;
                //}
                if(CheckNew)
                {
                    LinkButton lnk;
                    int index = -1;
                    for (int i = 0; i < lstInternalMasters.Items.Count; i++)
                    {
                        lnk = (LinkButton)lstInternalMasters.Items[i].FindControl("lnkSelect");
                        if (lnk.CommandArgument == personBE.PERSON_ID.ToString())
                        {
                            ViewState["SelectedIndex"] = i.ToString();
                            index = i;
                            break;
                        }
                    }
                    if (index >= 0)
                    {
                        HtmlTableRow tr = (HtmlTableRow)lstInternalMasters.Items[index].FindControl("trItemTemplate");
                        tr.Attributes["class"] = "SelectedItemTemplate";
                    }
                }
                else 
                {
                    int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                    if (lstInternalMasters.Items.Count > index)
                    {
                        HtmlTableRow tr = (HtmlTableRow)lstInternalMasters.Items[index].FindControl("trItemTemplate");
                        tr.Attributes["class"] = "SelectedItemTemplate";
                    }
                }

                CheckNew = false;
            }
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message,ee);
        }
    }
    /// <summary>
    /// Function to check audit entry is required or not and also assigning new values to personBE and PostalAddressBE
    /// </summary>
    /// <param name="personBE"></param>
    /// <param name="postAddressBE"></param>
    /// <returns>bool True/False</returns>
    private bool CheckEntity(PersonBE personBE, PostalAddressBE postAddressBE)
    {
        bool isAuditNeeded = false;
        isAuditNeeded = (personBE.CONCTACT_TYPE_ID != Convert.ToInt32(ddlContactType.SelectedItem.Value)) ? true : isAuditNeeded;

        isAuditNeeded = (personBE.TITLE_ID != Convert.ToInt32(ddlTitle.SelectedItem.Value)) ? true : isAuditNeeded;

        isAuditNeeded = (personBE.SURNAME != txtLastName.Text) ? true : isAuditNeeded;

        isAuditNeeded = (personBE.FORENAME != txtFirstName.Text) ? true : isAuditNeeded;

        isAuditNeeded = (personBE.USERID != txtUserID.Text.ToUpper()) ? true : isAuditNeeded;

        isAuditNeeded = (personBE.TELEPHONE1 != Regex.Replace(txtTelephone.Text, @"[- ()_]", String.Empty)) ? true : isAuditNeeded;

        isAuditNeeded = (personBE.FAX != Regex.Replace(txtFax.Text, @"[- ()_]", String.Empty)) ? true : isAuditNeeded;

        isAuditNeeded = (personBE.EMAIL != txtEmail.Text) ? true : isAuditNeeded;

        if (txtValidFrom.Text != "")
        {
            isAuditNeeded = (personBE.EFFECTIVEDATE != DateTime.Parse(txtValidFrom.Text)) ? true : isAuditNeeded;
        }

        if (txtValidTo.Text != "")
        {
            isAuditNeeded = (personBE.EXPIRYDATE != DateTime.Parse(txtValidTo.Text)) ? true : isAuditNeeded;
        }
        if (ddlActSetup.SelectedIndex >= 0)
        {
            isAuditNeeded = (personBE.ACCTSETUP_QC != Convert.ToInt32(ddlActSetup.SelectedItem.Value)) ? true : isAuditNeeded;
            personBE.ACCTSETUP_QC = Convert.ToInt32(ddlActSetup.SelectedItem.Value);
        }
        if (ddlAdj.SelectedIndex >= 0)
        {
            isAuditNeeded = (personBE.ADJUSTMENT_QC != Convert.ToInt32(ddlAdj.SelectedItem.Value)) ? true : isAuditNeeded;
        }
        if (ddlManager.SelectedIndex > 0)
        {
            isAuditNeeded = (personBE.MANAGERID != Convert.ToInt32(ddlManager.SelectedItem.Value)) ? true : isAuditNeeded;
        }
        if (ddlAries.SelectedIndex >= 0)
        {
            isAuditNeeded = (personBE.ARIES_QC != Convert.ToInt32(ddlAries.SelectedItem.Value)) ? true : isAuditNeeded;
        }

        isAuditNeeded = (postAddressBE.ADDRESS1 != txtAddress1.Text) ? true : isAuditNeeded;

        isAuditNeeded = (postAddressBE.ADDRESS2 != txtAddress2.Text) ? true : isAuditNeeded;

        isAuditNeeded = (postAddressBE.CITY != txtCity.Text) ? true : isAuditNeeded;

        isAuditNeeded = (postAddressBE.STATE_ID != Convert.ToInt32(ddlState.SelectedItem.Value)) ? true : isAuditNeeded;

        isAuditNeeded = (postAddressBE.ZIP_CODE != Regex.Replace(txtZipCode.Text, @"[- ()_]", String.Empty)) ? true : isAuditNeeded;


        return isAuditNeeded;
    }
    /// <summary>
    /// Function to assign the Textbox  values to PersonBE and PostalAddressBE
    /// </summary>
    /// <param name="personBE"></param>
    /// <param name="postAddressBE"></param>
    private void AssignEntity(PersonBE personBE, PostalAddressBE postAddressBE)
    {
        personBE.CONCTACT_TYPE_ID = Convert.ToInt32(ddlContactType.SelectedItem.Value);
        //personBE.TITLE_ID = Convert.ToInt32(ddlTitle.SelectedItem.Value);
        if (ddlTitle.SelectedIndex > 0)
        {
            personBE.TITLE_ID = Convert.ToInt32(ddlTitle.SelectedItem.Value);
        }
        else
        {
            personBE.TITLE_ID = null;
        }
        personBE.SURNAME = txtLastName.Text;
        personBE.FORENAME = txtFirstName.Text;
        personBE.USERID = txtUserID.Text.ToUpper();
        // Remove symbols (dash, space and parentheses)
        string str = Regex.Replace(txtTelephone.Text, @"[- ()_]", String.Empty);
        personBE.TELEPHONE1 = str.Trim();
        str = Regex.Replace(txtFax.Text, @"[- ()_]", String.Empty);
        personBE.FAX = str.Trim();
        personBE.EMAIL = txtEmail.Text;
        if (txtValidFrom.Text != "")
        {
            personBE.EFFECTIVEDATE = DateTime.Parse(txtValidFrom.Text);
        }
        else
        {
            personBE.EFFECTIVEDATE = null;
        }
        if (txtValidTo.Text != "")
        {
            personBE.EXPIRYDATE = DateTime.Parse(txtValidTo.Text);
        }
        else
        {
            personBE.EXPIRYDATE = null;
        }
        if (ddlActSetup.SelectedIndex > 0)
        {
            personBE.ACCTSETUP_QC = Convert.ToInt32(ddlActSetup.SelectedItem.Value);
        }
        else
        {
            personBE.ACCTSETUP_QC = null;
        }
        if (ddlAdj.SelectedIndex > 0)
        {
            personBE.ADJUSTMENT_QC = Convert.ToInt32(ddlAdj.SelectedItem.Value);
        }
        else
        {
            personBE.ADJUSTMENT_QC = null;
        }
        if (ddlAries.SelectedIndex > 0)
        {
            personBE.ARIES_QC = Convert.ToInt32(ddlAries.SelectedItem.Value);
        }
        else
        {
            personBE.ARIES_QC = null;
        }
        if (ddlManager.SelectedIndex > 0)
        {
            personBE.MANAGERID = Convert.ToInt32(ddlManager.SelectedItem.Value);
        }
        else
        {
            personBE.MANAGERID = null;
        }
        postAddressBE.ADDRESS1 = txtAddress1.Text;
        postAddressBE.ADDRESS2 = txtAddress2.Text;
        postAddressBE.CITY = txtCity.Text;
        postAddressBE.STATE_ID = Convert.ToInt32(ddlState.SelectedItem.Value);
        postAddressBE.ZIP_CODE = Regex.Replace(txtZipCode.Text, @"[- ()_]", String.Empty);
    }
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        CheckNew = true;
        btnSave.Text = "Save";
        ShowMessage("Data has been copied. Please click on Save button to save the Record.");
    }
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable this record?');" : "return confirm('Are you sure you want to Enable this record?');");
            }
        }
    }
    protected void lstInternalMasters_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        HtmlTableRow tr;
        //code for changing the previous selected row color to its original Color
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            if (lstInternalMasters.Items.Count > index)
            {
                tr = (HtmlTableRow)lstInternalMasters.Items[index].FindControl("trItemTemplate");
                tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
            }
        }
        tr = (HtmlTableRow)lstInternalMasters.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        LinkButton lnk = (LinkButton)lstInternalMasters.Items[e.NewSelectedIndex].FindControl("lnkSelect");
        ViewState["SelectedIndex"] = e.NewSelectedIndex;
        //code for changing the selected row style to highlighted
        tr.Attributes["class"] = "SelectedItemTemplate";
        CheckNew = false;
        btnAdd.Enabled = false;
        btnSearch.Enabled = false;
        lnkClose.Visible = true;
        lstInternalMasters.Enabled = false;
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
        {
            btnSave.Enabled = true;
            btnCopy.Enabled = true;
        }
        btnSave.Text = "Update";

        int intPersonID = Convert.ToInt32(lnk.CommandArgument);
        SelectedValue = intPersonID;
        //function call to display the selected internal contact Details
        HiddenField hid = (HiddenField)lstInternalMasters.Items[e.NewSelectedIndex].FindControl("hidActive");
        HiddenField hidPost = (HiddenField)lstInternalMasters.Items[e.NewSelectedIndex].FindControl("HidPostID");
        //Funtion to Display the details of the selected Internal Contact
        DisplayDetails(intPersonID, Convert.ToInt32(hidPost.Value));
        pnlDetails.Enabled = bool.Parse(hid.Value);
        pnlDetails.Visible = true;
        lblInternalContactsDetails.Visible = true;

    }
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        //Function call to Enable or Disable the Internal Contact Record
        if (e.CommandName != "Select")
            DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true);
    }
    /// <summary>
    /// function for Person record to make enable or disable
    /// </summary>
    /// <param name="personID"></param>
    /// <param name="Flag">True/False boolean value</param>
    /// <returns></returns>
    protected void DisableRow(int personID, bool Flag)
    {
        try
        {
            personBE = PersonService.getPersonRow(personID);
            personBE.ACTIVE = Flag;
            personBE.UPDATEDUSER_ID = CurrentAISUser.PersonID;
            personBE.UPDATEDDATE = DateTime.Now;
            Flag = PersonService.Update(personBE);
            ShowConcurrentConflict(Flag, personBE.ErrorMessage);
            if (Flag)
            {
                btnCopy.Enabled = false;
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.Save(GlobalConstants.AuditingWebPage.InternalMasters, CurrentAISUser.PersonID, Flag.ToString());
                PersonTransactionWrapper.SubmitTransactionChanges();
                //Function to bind the Listview with Internal Contacts
                BindListView();
            }
            else
            {
                PersonTransactionWrapper.RollbackChanges();
            }
            pnlDetails.Visible = false;
            lblInternalContactsDetails.Visible = false;
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message,ee);
        }
    }
    /// <summary>
    /// Function to bind the Listview with Internal Contacts data
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void BindListView()
    {
        if (int.Parse(ddlSearchContact.SelectedItem.Value.ToString()) != 0)
        {
            IList<PersonBE> list = PersonService.getPersonsList(ddlSearchContact.SelectedItem.Text);
            lstInternalMasters.DataSource = list;
            lstInternalMasters.DataBind();
        }
    }
    /// <summary>
    /// Function to display the selected internal contact details
    /// </summary>
    /// <param name="PersonID">Person ID of the selected person in the listview</param>
    /// <param name="PostID">Postal Address ID of the selected person in the listview</param>
    /// <returns></returns>
    public void DisplayDetails(int PersonID, int PostID)
    {
        /// Clears all control values
        ClearFileds();
        personBE = PersonService.getPersonRow(PersonID);
        postAddressBE = PostAddressService.getPostAddrRow(PostID);
        txtFirstName.Text = personBE.FORENAME;
        txtLastName.Text = personBE.SURNAME;
        txtAddress1.Text = postAddressBE.ADDRESS1;
        if (txtAddress1.Text != string.Empty)
        {
            ViewState["PostID"] = postAddressBE.POSTALADDRESSID;
        }
        txtAddress2.Text = postAddressBE.ADDRESS2;
        txtCity.Text = postAddressBE.CITY;
        txtZipCode.Text = postAddressBE.ZIP_CODE;
        // here empty space is added before because of mask appearance Telephone Textbox
        txtTelephone.Text = " " + personBE.TELEPHONE1;
        // here empty space is added before because of mask appearance for Fax Textbox
        txtFax.Text = " " + personBE.FAX;
        txtEmail.Text = personBE.EMAIL;
        if (personBE.EFFECTIVEDATE != null)
        {
            txtValidFrom.Text = DateTime.Parse(personBE.EFFECTIVEDATE.ToString()).ToShortDateString();
        }

        if (personBE.EXPIRYDATE != null)
        {
            txtValidTo.Text = DateTime.Parse(personBE.EXPIRYDATE.ToString()).ToShortDateString();
        }


        txtUserID.Text = personBE.USERID;
        //Function call to Bind the Manager DropDown List
        FillManagers();
        if (personBE.MANAGERID != null)
            ddlManager.Items.FindByValue(personBE.MANAGERID.ToString()).Selected = true;
        if (personBE.CONCTACT_TYPE_ID != 0)
        {
            ddlContactType.DataBind();
            ddlContactType.Items.FindByValue(personBE.CONCTACT_TYPE_ID.ToString()).Selected = true;
        }
        if (personBE.TITLE_ID != null)
        {
            ddlTitle.DataBind();
//            ddlTitle.Items.FindByValue(personBE.TITLE_ID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlTitle, personBE.TITLE_ID.Value);
        }
        if (personBE.ACCTSETUP_QC != null)
        {
            ddlActSetup.DataBind();
//            ddlActSetup.Items.FindByValue(personBE.ACCTSETUP_QC.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlActSetup, personBE.ACCTSETUP_QC.Value);
        }
        if (personBE.ADJUSTMENT_QC != null)
        {
            ddlAdj.DataBind();
//            ddlAdj.Items.FindByValue(personBE.ADJUSTMENT_QC.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlAdj, personBE.ADJUSTMENT_QC.Value);
        }
        if (personBE.ARIES_QC != null)
        {
            ddlAries.DataBind();
//            ddlAries.Items.FindByValue(personBE.ARIES_QC.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlAries, personBE.ARIES_QC.Value);
        }
        if (postAddressBE.STATE_ID != 0)
        {
            ddlState.DataBind();
//            ddlState.Items.FindByValue(postAddressBE.STATE_ID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlState, postAddressBE.STATE_ID);
        }
    }
    /// <summary>
    /// Binds the Manager Dropdown with persons data
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void FillManagers()
    {
        ddlManager.DataSource = PersonService.getPersonsList();
        ddlManager.DataValueField = "PERSON_ID";
        ddlManager.DataTextField = "FULLNAME";
        ddlManager.DataBind();
        ddlManager.Items.Insert(0, "(Select)");
    }
    /// <summary>
    /// Clears all control values
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void ClearFileds()
    {
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtAddress1.Text = string.Empty;
        txtAddress2.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtZipCode.Text = string.Empty;
        txtTelephone.Text = string.Empty;
        txtFax.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtUserID.Text = string.Empty;
        txtValidTo.Text = string.Empty;
        txtValidFrom.Text = string.Empty;
        ddlActSetup.SelectedIndex = -1;
        ddlAdj.SelectedIndex = -1;
        ddlAries.SelectedIndex = -1;
        ddlContactType.SelectedIndex = -1;
        ddlManager.SelectedIndex = -1;
        ddlState.SelectedIndex = -1;
        ddlTitle.SelectedIndex = -1;
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        // This condition is part of Application Security
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            btnAdd.Enabled = true;
        lnkClose.Visible = false;
        btnSearch.Enabled = true;
        pnlDetails.Visible = false;
        lblInternalContactsDetails.Visible = false;
        lstInternalMasters.Enabled = true;
        HtmlTableRow tr;
        //code for changing the previous selected row color to its original Color
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            if (lstInternalMasters.Items.Count > index)
            {
                tr = (HtmlTableRow)lstInternalMasters.Items[index].FindControl("trItemTemplate");
                tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
            }
        }
        SelectedValue = -1;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (SelectedValue != -1)
        {
            HiddenField hidPost = (HiddenField)lstInternalMasters.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("HidPostID");
            DisplayDetails(SelectedValue, Convert.ToInt32(hidPost.Value));
        }
        else
        {
            /// Clears all control values
            ClearFileds();
        }
    }
}
