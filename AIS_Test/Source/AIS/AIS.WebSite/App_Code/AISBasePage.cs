// Copyright (c) Microsoft Corporation. All rights reserved.
using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;
using System.IO;
using System.Collections.Specialized;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using ZurichNA.LSP.Framework;
using CrystalDecisions.Shared;
using System.Collections;
using ZurichNA.AIS.ExceptionHandling;
using System.Text.RegularExpressions;


/// <summary>
/// Base Page class for Retro ASP.NET application
/// </summary>
/// 
public partial class AISBasePage : System.Web.UI.Page
{
    private Common common = null;
    protected override void OnLoad(EventArgs e)
    {

        if (!this.IsPostBack)
        {


            string currentWebPage = RemoveWIDinQueryString(Request.Url.PathAndQuery.Remove(0, 1));
            string AbsoluteWebPage = Request.Url.AbsolutePath.Substring(1, Request.Url.AbsolutePath.Length - 1);
            if (!currentWebPage.Contains("tab"))
            {
                currentWebPage = AbsoluteWebPage;
            }

            currentWebPage = IsAISTabPage(currentWebPage.ToUpper());
            //Checks URL Authorization
            bool FlagAuthorized = CurrentAISUser.IsAuthorized(currentWebPage);
            if (!FlagAuthorized)
            {
                Exception exAp = new Exception("You are not an authorized user of the AIS application.");
                Exception ex = new Exception("You are not authorized to view " + currentWebPage + " web page.");
                //if (CurrentAISUser.Role.Length > 1)
                //    Session["RetroException"] = ex;
                //else
                //    Session["RetroException"] = exAp;
                if (CurrentAISUser.Role.Length > 1)
                    SaveObjectToSessionUsingWindowName("RetroException", ex);
                else
                    SaveObjectToSessionUsingWindowName("RetroException", exAp);

                //Response.Redirect("~/AppError/GeneralError.aspx");
                ResponseRedirect("~/AppError/GeneralError.aspx");
            }

            if (CurrentAISUser.ISAccountDependPage(currentWebPage))
            {
                if (AISMasterEntities == null)
                    //Response.Redirect("~/AcctSearch.aspx");
                    ResponseRedirect("~/AcctSearch.aspx");
            }

            //Checks Role based Security
            char acPerm = CurrentAISUser.AccessPermission(currentWebPage);
            if (acPerm == 'X')
            {
                Exception ex = new Exception("You are not authorized to view " + currentWebPage + " web page.");
                //Session["RetroException"] = ex;
                SaveObjectToSessionUsingWindowName("RetroException", ex);
                //Response.Redirect("~/AppError/GeneralError.aspx");
                ResponseRedirect("~/AppError/GeneralError.aspx");
            }
            else if (acPerm == 'I' || CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                WEBPAGEROLE = GlobalConstants.ApplicationSecurityGroup.Inquiry;
                ROLENAME = CurrentAISUser.Role;
                //DisableControls(this, IsAISSearchPage(AbsoluteWebPage.ToUpper()));
                DisableControls(this);
            }
            else
            {
                WEBPAGEROLE = null;
                ROLENAME = CurrentAISUser.Role;
                //if(!IsAISPureSearchPage(AbsoluteWebPage.ToUpper()))
                //    this.RegisterOnChangeScript(this);
            }

            HttpContext.Current.Session.Add("ProcessType" + WindowName,
                (currentWebPage.ToUpper().Contains("INVOICINGDASHBOARD.ASPX")) ? "calc" : "app");
         
        }
             
        this.RegisterOnChangeScript(this);
        base.OnLoad(e);
    }

    protected override void OnPreRender(EventArgs e)
    {

        base.OnPreRender(e);
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Commented Temporarily 
        //if (!CurrentAISUser.IsAuthorized(currentWebPage))
        //  {
        //        Exception ex = new Exception("Unauthorizd User! Access is Denied...");
        //        Session["RetroException"] = ex;
        //        Response.Redirect("~/AppError/GeneralError.aspx");
        //  }
    }

    //protected void Page_Error(object sender, EventArgs e)
    //{
    //    Exception ex = Server.GetLastError();
    //    //Session["RetroException"] = ex;
    //    SaveObjectToSessionUsingWindowName("RetroException", ex);
    //    //Response.Redirect("~/AppError/GeneralError.aspx");
    //    ResponseRedirect("~/AppError/GeneralError.aspx");
    //}

    //protected override void OnError(EventArgs e)
    //{
    //    base.OnError(e);
    //}

    public void AssignDirtyBit(Control ct)
    {
        string strCtlDirtyBit =
         "Javascript:document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value=1;";

        string strDirtyBit =
         "Javascript:document.getElementById('" + Page.Master.FindControl("hdnDirtyBit").ClientID + "').value=1;";

        string strResetDirtyBit =
         "Javascript:document.getElementById('" + Page.Master.FindControl("hdnDirtyBit").ClientID + "').value=0;";

        string strResetCtlDirtyBit =
         "Javascript:document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value=0;";

        if (ct != null)
        {
            if (ct.GetType() == typeof(CheckBox))
            {
                if (((CheckBox)ct).Attributes["onChange"] == null)
                    ((CheckBox)ct).Attributes["onChange"] = strCtlDirtyBit;
                else
                    ((CheckBox)ct).Attributes["onChange"] = strCtlDirtyBit + ((CheckBox)ct).Attributes["onChange"];

                if (((CheckBox)ct).Attributes["onClick"] == null)
                    ((CheckBox)ct).Attributes["onClick"] = strCtlDirtyBit;
                else
                    ((CheckBox)ct).Attributes["onClick"] = strCtlDirtyBit + ((CheckBox)ct).Attributes["onClick"];

            }
            if (ct.GetType() == typeof(TextBox))
            {
                if (((TextBox)ct).Attributes["onEnter"] == null)
                    ((TextBox)ct).Attributes["onEnter"] = strCtlDirtyBit;
                else
                    ((TextBox)ct).Attributes["onEnter"] += strCtlDirtyBit;

                if (((TextBox)ct).Attributes["onKeyUp"] == null)
                    ((TextBox)ct).Attributes["onKeyUp"] = strCtlDirtyBit;
                else
                    ((TextBox)ct).Attributes["onKeyUp"] = strCtlDirtyBit + ((TextBox)ct).Attributes["onKeyUp"];

                //if (((TextBox)ct).Attributes["onKeyPress"] == null)
                //    ((TextBox)ct).Attributes["onKeyPress"] = strCtlDirtyBit;
                //else
                //    ((TextBox)ct).Attributes["onKeyPress"] = strCtlDirtyBit + ((TextBox)ct).Attributes["onKeyPress"];

            }

            if (ct.GetType() == typeof(AjaxControlToolkit.CalendarExtender))
            {
                ((AjaxControlToolkit.CalendarExtender)ct).OnClientDateSelectionChanged =
                    "function() { " + strCtlDirtyBit + " }";
            }

            if (ct.GetType() == typeof(DropDownList))
            {
                if (((DropDownList)ct).Attributes["onchange"] == null)
                    ((DropDownList)ct).Attributes["onchange"] = strCtlDirtyBit;
                else
                    ((DropDownList)ct).Attributes["onchange"] = strCtlDirtyBit + ((DropDownList)ct).Attributes["onchange"];

            }
            if (ct.GetType() == typeof(Button))
            {
                string strValue = (((Button)ct).Text.Trim().ToUpper()=="SAVE"||
                    ((Button)ct).Text.ToUpper() == "UPDATE" || ((Button)ct).Text.ToUpper() == "ADD" ||
                    ((Button)ct).Text.ToUpper() == "FINALIZE" || ((Button)ct).Text.ToUpper() == "COPY")?
                    "submitted = 1;" : " submitted = 0;";
                if (((Button)ct).Attributes["onClick"] == null)
                    ((Button)ct).Attributes["onClick"] =
                        strResetDirtyBit + strResetCtlDirtyBit + strValue;
                else
                    ((Button)ct).Attributes["onClick"] =
                        strResetDirtyBit + strResetCtlDirtyBit + strValue +
                        ((Button)ct).Attributes["onClick"];
            }

            if (ct.GetType() == typeof(HyperLink))
            {
                if (((HyperLink)ct).Text.ToUpper() == "CLOSE")
                {
                    if (((HyperLink)ct).Attributes["onClick"] == null)
                        ((HyperLink)ct).Attributes["onClick"] = strResetDirtyBit + strResetCtlDirtyBit
                            + " submitted = 0;";
                    else
                        ((HyperLink)ct).Attributes["onClick"] =
                            strResetDirtyBit + strResetCtlDirtyBit + " submitted = 0;"+
                            ((HyperLink)ct).Attributes["onClick"];
                }
                else
                    ((HyperLink)ct).NavigateUrl =
                        "Javascript:CheckWithoutSave('" + ((HyperLink)ct).NavigateUrl.Replace("~", "..") + "');";
            }
            if (ct.GetType() == typeof(LinkButton))
            {
                if (((LinkButton)ct).Attributes["onClick"] == null)
                    ((LinkButton)ct).Attributes["onClick"] = strResetDirtyBit + strResetCtlDirtyBit;
                else
                    ((LinkButton)ct).Attributes["onClick"] =
                        strResetDirtyBit + strResetCtlDirtyBit + ((LinkButton)ct).Attributes["onClick"];
            }
        }
    }

    public void AssignDirtyBit(Page mypage, Control ct)
    {
        string strCtlDirtyBit =
         "Javascript:document.getElementById('" + mypage.Master.FindControl("hdnControlDirty").ClientID + "').value=1;";

        string strDirtyBit =
         "Javascript:document.getElementById('" + mypage.Master.FindControl("hdnDirtyBit").ClientID + "').value=1;";

        string strResetDirtyBit =
         "Javascript:document.getElementById('" + mypage.Master.FindControl("hdnDirtyBit").ClientID + "').value=0;";

        string strResetCtlDirtyBit =
         "Javascript:document.getElementById('" + mypage.Master.FindControl("hdnControlDirty").ClientID + "').value=0;";

        if (ct != null)
        {
            if (ct.GetType() == typeof(CheckBox))
            {
                if (((CheckBox)ct).Attributes["onClick"] == null)
                    ((CheckBox)ct).Attributes["onClick"] = strCtlDirtyBit;
                else
                    ((CheckBox)ct).Attributes["onClick"] = strCtlDirtyBit + ((CheckBox)ct).Attributes["onClick"];
            }

            if (ct.GetType() == typeof(RadioButton))
            {
                if (((RadioButton)ct).Attributes["onClick"] == null)
                    ((RadioButton)ct).Attributes["onClick"] = strCtlDirtyBit;
                else
                    ((RadioButton)ct).Attributes["onClick"] = strCtlDirtyBit + ((RadioButton)ct).Attributes["onClick"];
            }

            if (ct.GetType() == typeof(TextBox))
            {
                if (((TextBox)ct).Attributes["onEnter"] == null)
                    ((TextBox)ct).Attributes["onEnter"] = strCtlDirtyBit;
                else
 
                    ((TextBox)ct).Attributes["onEnter"] += strCtlDirtyBit;

                if (((TextBox)ct).Attributes["onKeyUp"] == null)
                    ((TextBox)ct).Attributes["onKeyUp"] = strCtlDirtyBit;
                else
                    ((TextBox)ct).Attributes["onKeyUp"] = strCtlDirtyBit + ((TextBox)ct).Attributes["onKeyUp"];

                //if (((TextBox)ct).Attributes["onKeyPress"] == null)
                //    ((TextBox)ct).Attributes["onKeyPress"] = strCtlDirtyBit;
                //else
                //    ((TextBox)ct).Attributes["onKeyPress"] = strCtlDirtyBit + ((TextBox)ct).Attributes["onKeyPress"];

            }
            
            if (ct.GetType() == typeof(AjaxControlToolkit.CalendarExtender))
            {
                ((AjaxControlToolkit.CalendarExtender)ct).OnClientDateSelectionChanged =
                    "function() { " + strCtlDirtyBit + " }";
            } 
            
            if (ct.GetType() == typeof(DropDownList))
            {
                if (((DropDownList)ct).Attributes["onChange"] == null)
                    ((DropDownList)ct).Attributes["onChange"] = strCtlDirtyBit;
                else
                    ((DropDownList)ct).Attributes["onChange"] = strCtlDirtyBit + ((DropDownList)ct).Attributes["onChange"];

            }
            if (ct.GetType() == typeof(Button))
            {
                if (((Button)ct).Attributes["onClick"] == null)
                    ((Button)ct).Attributes["onClick"] = strResetDirtyBit + strResetCtlDirtyBit;
                else
                    ((Button)ct).Attributes["onClick"] = strResetDirtyBit + strResetCtlDirtyBit + ((Button)ct).Attributes["onClick"];
            }

            if (ct.GetType() == typeof(HyperLink))
            {
                if (((HyperLink)ct).Text.Trim().ToUpper() == "EDIT")
                {
                    ((HyperLink)ct).Attributes["onClick"] = strCtlDirtyBit + ((HyperLink)ct).Attributes["onClick"];
                }
                //else if (((HyperLink)ct).Text.Trim().ToUpper() == "UPDATE" || ((HyperLink)ct).Text.Trim().ToUpper() == "SAVE")
                //{
                //    ((HyperLink)ct).Attributes["onClick"] = strResetCtlDirtyBit + " submitted = 1;" + ((HyperLink)ct).Attributes["onClick"];
                //}
                else if (((HyperLink)ct).Text.Trim().ToUpper() == "CANCEL")
                {
                    ((HyperLink)ct).Attributes["onClick"] = strResetCtlDirtyBit +((HyperLink)ct).Attributes["onClick"];
                }
            }
            if (ct.GetType() == typeof(LinkButton))
            {
                if (((LinkButton)ct).Text.Trim().ToUpper() == "EDIT")
                {
                    ((LinkButton)ct).Attributes["onClick"] = strCtlDirtyBit + ((LinkButton)ct).Attributes["onClick"];
                }
                //else if (((LinkButton)ct).Text.Trim().ToUpper() == "UPDATE" || ((LinkButton)ct).Text.Trim().ToUpper() == "SAVE")
                //{
                //    ((LinkButton)ct).Attributes["onClick"] = strResetCtlDirtyBit + " submitted = 1;" + ((LinkButton)ct).Attributes["onClick"];
                //}
                else if (((LinkButton)ct).Text.Trim().ToUpper() == "CANCEL")
                {
                    ((LinkButton)ct).Attributes["onClick"] = strResetCtlDirtyBit + ((LinkButton)ct).Attributes["onClick"];
                }
                else
                ((LinkButton)ct).Attributes["onClick"] +=
                        strResetDirtyBit + strResetCtlDirtyBit + ((LinkButton)ct).Attributes["onClick"];
            }
        }
    }
    public void ProcessExitFlag(ArrayList ctlc)
    {
        if (!IsPostBack)
        {
            Control ct = null;
            foreach (object oct in ctlc)
            {
                ct = (Control)oct;
                AssignDirtyBit(ct);
            }
        }
    }

    public void PopupOnSurchargeCode()
    {
        ClientScriptManager cs = ((Page)this.Page).ClientScript;
        string scriptString = "  window.onbeforeunload = AlertUser; ";
        scriptString += "  function AlertUser( ) ";
        scriptString += "   { ";
        scriptString += "     if((keyCodeValue == 8) ||(window.event.clientX<0) || (window.event.clientY<0)";
        scriptString += "        ||(window.event.clientX>32500) || (window.event.clientY>32600))";
        scriptString += "     { ";
        scriptString += "       keyCodeValue = 0;";
        scriptString += "       var ex_msg = \"To avoid possible loss of data, " +
                                "click Cancel and use the AIS Application Menu to Exit or navigate to another page. \"; ";
        scriptString += "       return ex_msg; ";
        scriptString += "     } ";
        scriptString += "   } ";

        cs.RegisterClientScriptBlock(this.Page.GetType(), "OnChangeFunction", scriptString, true);
    }

    /// <summary>
    /// <para>
    /// PFCUtility:RegisterOnChangeScript( )
    /// </para>
    /// Register two javaScript functions used in performing background database saves.
    /// </summary>
    /// <param name="mypage">Calling page identifier</param>
    public void RegisterOnChangeScript(Page mypage)
    {
        string currentWebPage =
           Request.Url.AbsolutePath.Split(new char[] { '/' })
           [Request.Url.AbsolutePath.Split(new char[] { '/' }).Length - 1];
        ClientScriptManager cs = ((Page)mypage).ClientScript;

        string strResetCtlDirtyBit =
         "document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value=0;";
        string strResetDirtyBit =
         "document.getElementById('" + Page.Master.FindControl("hdnDirtyBit").ClientID + "').value=0;";
        //        string strResetRealBit =
        //         "document.getElementById('" + Page.Master.FindControl("hdnRealBit").ClientID + "').value=0;";


        string strCtlDirtyValue = "document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value";
        string strDirtyValue = "document.getElementById('" + Page.Master.FindControl("hdnDirtyBit").ClientID + "').value";

        string scriptString = "  window.onbeforeunload = AlertUser; ";
        scriptString += "  function AlertUser( ) ";
        scriptString += "   { ";
        scriptString += "     if((keyCodeValue == 8) ||(window.event.clientX<0) || (window.event.clientY<0)";
        scriptString += "        ||(window.event.clientX>32500) || (window.event.clientY>32600))";
        scriptString += "     { ";
        scriptString += "       keyCodeValue = 0;";
        scriptString += "       var ex_msg = \"To avoid possible loss of data, " +
                                "click Cancel and use the AIS Application Menu to Exit or navigate to another page. \"; ";
        scriptString += "       return ex_msg; ";
        scriptString += "     } ";
        scriptString += "   } ";

        cs.RegisterClientScriptBlock(mypage.GetType(), "OnChangeFunction", scriptString, true);
    }

    #region Public Methods for Displaying Errors

    public void ShowServerError(ValidationErrorCollection Errors)
    {
        string errorMessage = String.Empty;
        errorMessage = "<br /><ul>";
        foreach (ValidationError e in Errors)
        {
            errorMessage += "<li>" + e.Message + "</li>";
        }
        errorMessage += "</ul>";

        ValidationSummary summary = null;
        CustomValidator validator = null;

        try
        {
            validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
            summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");

            //summary.HeaderText = "<br>The following errors have occurred:";
            validator.ErrorMessage = errorMessage;
            //summary.ForeColor = System.Drawing.Color.Red;
            //summary.Font.Size = FontUnit.Small;
            summary.CssClass = "ValidationSummary";
            validator.Validate();
            summary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
        }
        catch (Exception exp)
        {
            throw exp;
        }

    }

    /// <summary>
    /// Shows Error(s) with Error Message Parameter
    /// </summary>
    /// <param name="errorMessage"></param>
    public void ShowError(string errorMessage)
    {
        ValidationSummary summary = null;
        CustomValidator validator = null;


        try
        {
            ClearError();
            validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
            summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");

            //                summary.HeaderText = "<br>The following errors have occurred:";
            validator.ErrorMessage = errorMessage;
            summary.CssClass = "ValidationSummary";

            //summary.Font.Size = FontUnit.Small;
            //summary.ForeColor = System.Drawing.Color.Red;
            validator.Validate();
            summary.DisplayMode = ValidationSummaryDisplayMode.BulletList;

            string strCtlDirtyBit =
             "Javascript:document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value=1;";
            ClientScript.RegisterStartupScript(this.GetType(), "setvalue", strCtlDirtyBit, true);
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }

    /// <summary>
    /// Shows Error(s) with Error Message Parameter
    /// </summary>
    /// <param name="errorMessage"></param>
    public void ShowError(string errorMessage, Exception ex)
    {
        ValidationSummary summary = null;
        CustomValidator validator = null;
       

        try
        {
            common = new Common(this.GetType());
            ClearError();
            validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
            summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");

            //                summary.HeaderText = "<br>The following errors have occurred:";
            validator.ErrorMessage = errorMessage;
            summary.CssClass = "ValidationSummary";

            //summary.Font.Size = FontUnit.Small;
            //summary.ForeColor = System.Drawing.Color.Red;
            validator.Validate();
            summary.DisplayMode = ValidationSummaryDisplayMode.BulletList;

            string strCtlDirtyBit =
             "Javascript:document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value=1;";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "setvalue", strCtlDirtyBit, true);
            if (User.Identity.Name != null)
            {
                common.Logger.Error(ex.Message + " error occurred for user:" + User.Identity.Name.ToString(), ex);
            }
            else
            {
                common.Logger.Error(ex.Message, ex);
            }
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }
    /// <summary>
    /// Clears Error(s) from validator
    /// </summary>
    /// <param name="errorMessage"></param>
    public void ClearError()
    {
        ValidationSummary summary = null;
        CustomValidator validator = null;

        try
        {
            validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
            summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");

            summary.HeaderText = "";
            validator.ErrorMessage = "";
            //summary.ForeColor = System.Drawing.Color.Red;
            //validator.Validate();
            //summary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }


    /// <summary>
    /// Shows Error(s) with error message and Display Mode
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="displayMode"></param>
    public void ShowError(string errorMessage, ValidationSummaryDisplayMode displayMode)
    {
        ValidationSummary summary = null;
        CustomValidator validator = null;
        try
        {
            validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
            summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");

            //                summary.HeaderText = "<br>The following errors have occurred:";

            if (displayMode == ValidationSummaryDisplayMode.List)
            {
                errorMessage += "<ul>" + errorMessage + "</ul>";
            }

            validator.ErrorMessage = errorMessage;
            //summary.ForeColor = System.Drawing.Color.Red;
            //summary.Font.Size = FontUnit.Small;
            summary.CssClass = "ValidationSummary";
            validator.Validate();
            summary.DisplayMode = displayMode;
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }

    /// <summary>
    /// Shows error messages with display Mode and Header Text
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="displayMode"></param>
    /// <param name="headerText"></param>
    public void ShowError(string errorMessage, ValidationSummaryDisplayMode displayMode,
                            string headerText)
    {
        ValidationSummary summary = null;
        CustomValidator validator = null;
        try
        {
            validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
            summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");
            //                summary.HeaderText = "<br/>" + headerText;

            if (displayMode == ValidationSummaryDisplayMode.List)
            {
                errorMessage += "<ul>" + errorMessage + "</ul>";
            }

            validator.ErrorMessage = errorMessage;
            //summary.ForeColor = System.Drawing.Color.Red;
            //summary.Font.Size = FontUnit.Small;
            summary.CssClass = "ValidationSummary";
            validator.Validate();
            summary.DisplayMode = displayMode;
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }

    /// <summary>
    /// Shows error messages with display Mode and Header Text
    /// </summary>
    /// <param name="message"></param>
    public void ShowMessage(string message)
    {
        ValidationSummary summary = null;
        CustomValidator validator = null;
        try
        {
            validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
            summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");

            message = "<ul><li>" + message + "</li></ul>";
            summary.HeaderText = message;
            summary.CssClass = "ValidationSummary";
            //summary.ForeColor = System.Drawing.Color.Blue;
            //summary.Font.Size = new FontUnit(11);
            validator.Validate();
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }

    public void ShowSurchargeMessage()
    {
        StringBuilder aConfMessage = new StringBuilder();
        TextBox aTxt = new TextBox();
        Response.Write(@"<SCRIPT>if(confirm(" + '"' + "In order for changes to take effect, the adjustment must be recalculated. There has been a change to the Other Surcharges & Credits field.A comment is required.  Would you like enter comments now?" + '"' + ")){window.showModalDialog('/AdjCalc/SurchargeReviewComments.aspx?wID=" + WindowName + "','name','dialogWidth:300x;dialogHeight:300px');}</SCRIPT>");
        // Response.Write("<SCRIPT>window.showModalDialog('/AdjCalc/SurchargeReviewComments.aspx','name','dialogWidth:300x;dialogHeight:300px');</SCRIPT>");

        //if (Session["CommentsShown"] != null && String.IsNullOrEmpty(Session["CommentsShown"].ToString()))
        //{
        //    Session["PreviousPage"] = null;

        //}
        //else
        //{
        //    Session["PreviousPageRepeated"] = "SurchargeAssesment";
        //   // Response.Redirect("http://localhost:3665/AdjCalc/SurchargeAssesmentReview.aspx?SelectedValues=156;3/31/2009;2562;1080");
        //}
        if (RetrieveObjectFromSessionUsingWindowName("CommentsShown") != null && String.IsNullOrEmpty(RetrieveObjectFromSessionUsingWindowName("CommentsShown").ToString()))
        {
            SaveObjectToSessionUsingWindowName("PreviousPage", null);
        }
        else
        {
            SaveObjectToSessionUsingWindowName("PreviousPageRepeated", "SurchargeAssesment");
        }
    }


    #endregion

    #region Public Properties
    /// <summary>
    /// It is used to hold variable Master Variables such as 
    /// Account Number, Account Name
    /// Program Period Details with ID
    /// SSCGID
    /// 
    /// </summary>
    public MasterEntities AISMasterEntities
    {
        //get { return (MasterEntities)Session["MasterEntities"]; }
        //set { Session["MasterEntities"] = value; }
        get { return (MasterEntities)RetrieveObjectFromSessionUsingWindowName("MasterEntities"); }
        set { SaveObjectToSessionUsingWindowName("MasterEntities", value); }
    }

    /// <summary>
    /// Retrieves the LookUp Data for all LookUp Types
    /// </summary>
    public IList<LookupBE> LookUpData
    {
        get
        {
            return (IList<LookupBE>)Application["LookUpData"];
        }
    }

    /// <summary>
    /// Retrieves Current Logged-In AIS User
    /// </summary>
    public AISUser CurrentAISUser
    {
        get
        {
            return (AISUser)System.Web.HttpContext.Current.Items["CurrentUser"];
        }
    }
    //Security Testing
    public string WEBPAGEROLE
    {
        //get { return (string)Session["WEBPAGEROLE"]; }
        //set { Session["WEBPAGEROLE"] = value; }
        get { return (string)RetrieveObjectFromSessionUsingWindowName("WEBPAGEROLE"); }
        set { SaveObjectToSessionUsingWindowName("WEBPAGEROLE", value); }
    }
    //Security Testing
    public string ROLENAME
    {
        //get { return (string)Session["ROLENAME"]; }
        //set { Session["ROLENAME"] = value; }
        get { return (string)RetrieveObjectFromSessionUsingWindowName("ROLENAME"); }
        set { SaveObjectToSessionUsingWindowName("ROLENAME", value); }
    }

    /// <summary>
    /// Returns the Window Name
    /// </summary>
    public string WindowName
    {
        get
        {
            if (this.Page != null && this.Page.Master != null && ((HiddenField)this.Page.Master.FindControl("form1").FindControl("tsttt").Controls[0].FindControl("hdnWindowName")) != null
                && ((HiddenField)this.Page.Master.FindControl("form1").FindControl("tsttt").Controls[0].FindControl("hdnWindowName")).Value != null
                && ((HiddenField)this.Page.Master.FindControl("form1").FindControl("tsttt").Controls[0].FindControl("hdnWindowName")).Value != "")
            {
                return ((HiddenField)this.Page.Master.FindControl("form1").FindControl("tsttt").Controls[0].FindControl("hdnWindowName")).Value;
            }
            else if (HttpContext.Current.Request["wID"] != null && HttpContext.Current.Request["wID"] != "")
            {
                //EAISA-5 Veracode flaw fix 12072017                
                return System.Web.HttpUtility.HtmlEncode( HttpContext.Current.Request["wID"].ToString());
            }
            else
            {
                return "";
            }
        }
    }
    #endregion

    #region public Methods
    /// <summary>
    /// Populates the Lookup Values for a given DropDown List
    /// </summary>
    /// <param name="lookupTypeName">Look Type Name</param>
    /// <param name="ddList">DropDown List Reference</param>
    public void PopulateDropDownList(string lookupTypeName, ref DropDownList ddList)
    {
        IList<LookupBE> lkups = new List<LookupBE>();
        lkups = (new ZurichNA.AIS.Business.Logic.BLAccess()).GetLookUpActiveData(lookupTypeName);
        ddList.DataSource = lkups;
        ddList.DataTextField = "LookUpName";
        ddList.DataValueField = "LookUpID";
        ddList.DataBind();
        // ListItem item = new ListItem("(Select)", "0");
        //ddList.Items.Insert(0, item);
    }

    /// <summary>
    /// Populates the Lookup Values for a given DropDown List
    /// </summary>
    /// <param name="lookupTypeName">Lookup Type Name</param>
    /// <param name="ddList">DropDown List</param>
    /// <param name="selectedValue">Selected Value</param>
    public void PopulateDropDownList(string lookupTypeName, ref DropDownList ddList, string selectedValue)
    {
        PopulateDropDownList(lookupTypeName, ref ddList);
        ddList.SelectedIndex = ddList.Items.IndexOf(ddList.Items.FindByValue(selectedValue));
    }
    # endregion

    # region Database Connection Information
    /// <summary>
    /// Method to return database connection information for the Reports.
    /// This works with SQL Server Authentication or Integrated Security,
    /// depending on what the web.config is using.
    /// </summary>
    /// <returns></returns>
    public ConnectionInfo GetReportConnection()
    {
        ConnectionInfo conn = new ConnectionInfo();
        if (IntegratedSecuritySet())
        {
            conn.ServerName = RetrieveDBServer();
            conn.DatabaseName = RetrieveDBName();
            conn.IntegratedSecurity = true;
        }
        else
        {
            conn.ServerName = RetrieveDBServer();
            conn.DatabaseName = RetrieveDBName();
            conn.UserID = RetrieveUserID();
            conn.Password = RetrievePassword();
        }
        return conn;
    }

    // Retrieves whether or not Integrated Security is being used from the Web.config
    // Parse the <DBConnectionLINQ> - return true if "Integrated Security=SSPI" is being used
    public bool IntegratedSecuritySet()
    {
        string connection = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ConnectionString;
        return connection.Contains("Integrated Security=SSPI");
    }
    // Retrieves the DB Server from the Web.config
    // Parse the <DBConnectionLINQ> - return the Data Source value
    public string RetrieveDBServer()
    {
        string DBServer = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ConnectionString;
        int startIndex, endIndex;
        startIndex = DBServer.IndexOf("Data Source=") + 12;
        DBServer = DBServer.Remove(0, startIndex);
        endIndex = DBServer.IndexOf(";");
        if (endIndex != -1)
            DBServer = DBServer.Remove(endIndex, DBServer.Length - endIndex);
        return DBServer;
    }
    // Retrieves the DB Name from the Web.config
    // Parse the <DBConnectionLINQ> - return the Initial Catalog value
    public string RetrieveDBName()
    {
        string DBName = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ConnectionString;
        int startIndex, endIndex;
        startIndex = DBName.IndexOf("Initial Catalog=") + 16;
        DBName = DBName.Remove(0, startIndex);
        endIndex = DBName.IndexOf(";");
        if (endIndex != -1)
            DBName = DBName.Remove(endIndex, DBName.Length - endIndex);
        return DBName;
    }
    // Retrieves the User ID from the Web.config
    // Parse the <DBConnectionLINQ> - return the User ID value
    public string RetrieveUserID()
    {
        string UserID = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ConnectionString;
        int startIndex, endIndex;
        startIndex = UserID.IndexOf("User ID=") + 8;
        UserID = UserID.Remove(0, startIndex);
        endIndex = UserID.IndexOf(";");
        if (endIndex != -1)
            UserID = UserID.Remove(endIndex, UserID.Length - endIndex);
        return UserID;
    }
    // Retrieves the Password from the Web.config
    // Parse the <DBConnectionLINQ> - return the Password value
    public string RetrievePassword()
    {
        string Password = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ConnectionString;
        int startIndex, endIndex;
        startIndex = Password.IndexOf("Password=") + 9;
        Password = Password.Remove(0, startIndex);
        endIndex = Password.IndexOf(";");
        if (endIndex != -1)
            Password = Password.Remove(endIndex, Password.Length - endIndex);
        return Password;
    }

    /// <summary>
    /// Set the given value into given session
    /// </summary>
    /// <param name="SessionVariable"></param>
    /// <param name="EntityObject"></param>
    public void SaveObjectToSession(string SessionVariable, object EntityObject)
    {
        Session[SessionVariable] = EntityObject;
    }

    /// <summary>
    /// Set the given value into given session for a particular window
    /// </summary>
    /// <param name="SessionVariable"></param>
    /// <param name="EntityObject"></param>
    public void SaveObjectToSessionUsingWindowName(string SessionVariable, object EntityObject)
    {
        SaveObjectToSession(WindowName + SessionVariable, EntityObject);
    }

    /// <summary>
    /// Retrives the given Session for a particular window.
    /// </summary>
    /// <param name="SessionVariable"></param>
    /// <returns></returns>
    public object RetrieveObjectFromSession(string SessionVariable)
    {
        return Session[SessionVariable];
    }

    /// <summary>
    /// Retrives the given Session
    /// </summary>
    /// <param name="SessionVariable"></param>
    /// <returns></returns>
    public object RetrieveObjectFromSessionUsingWindowName(string SessionVariable)
    {
        return RetrieveObjectFromSession(WindowName + SessionVariable);
    }
    // veracode fix 06192018

    public void ResponseRedirect(string url)
    {       
            if (!url.Contains("?wID") && !url.Contains("&wID"))
            {
                Regex validateWinName = new Regex("^[a-zA-Z0-9-]+$");
                if (url.Contains('?'))
                {                                      
                    if (validateWinName.IsMatch(WindowName))
                    {
                        //EAISA-5 Veracode flaw fix 12072017 
                        //string url1=(string.Format("{0}&wID={1}", url, WindowName));  
                        Response.Redirect(string.Format("{0}&wID={1}", System.Web.HttpUtility.HtmlEncode(url), WindowName));
                    }
                }
                else
                {
                    if (validateWinName.IsMatch(WindowName))
                    {
                        //EAISA-5 Veracode flaw fix 12072017
                        //string url2 = (string.Format("{0}?wID={1}", url, WindowName));
                        Response.Redirect(string.Format("{0}?wID={1}", System.Web.HttpUtility.HtmlEncode(url), WindowName));
                    }
                }
            }
    }

    #endregion


    public void DisableControls(Page mypage)
    {
        //this.FindControls(mypage.Page.Form.FindControl("MainPlaceHolder").Controls, IsSearchPage);
        //this.FindControls(mypage.Page.Form.FindControl("HeaderPlaceHolder").Controls, IsSearchPage);

        this.FindControls(mypage.Page.Form.FindControl("MainPlaceHolder").Controls);
        this.FindControls(mypage.Page.Form.FindControl("HeaderPlaceHolder").Controls);

    }
    public void FindControls(ControlCollection d)
    {
        foreach (Control c in d)
        {
            if (c.HasControls())
            {
                this.FindControls(c.Controls);
            }
            if (c is Button)
            {
                Button btn = (Button)c;
                if (!btn.Text.ToUpper().Contains("SEARCH") && !btn.Text.ToUpper().Contains("PREVIEW") && !btn.Text.ToUpper().Contains("EXPORT") && !btn.Text.ToUpper().Contains("CLEAR") && !btn.Text.ToUpper().Contains("QUERY INVOICE") && !btn.Text.ToUpper().Contains("BACK") && !btn.Text.ToUpper().Contains("CLOSE"))
                {
                    ((WebControl)c).Enabled = false;
                }
            }
            else if (c is LinkButton)
            {
                LinkButton lnk = (LinkButton)c;
                if (lnk.Text.ToUpper().Contains("SAVE") || lnk.Text.ToUpper().Contains("CANCEL") || lnk.Text.ToUpper().Contains("VOID") || lnk.Text.ToUpper().Contains("UPDATE") || lnk.Text.ToUpper().Contains("REVISION"))
                {
                    ((WebControl)c).Enabled = false;
                    ((WebControl)c).Style.Add(HtmlTextWriterStyle.Cursor, "Text");
                }
            }
            else if (c is Image)
            {
                ((WebControl)c).Enabled = false;
                ((WebControl)c).Style.Add(HtmlTextWriterStyle.Cursor, "Text");
            }
        }
    }
    /// Takes an inventory of all controls in the page and disables buttons and hyperlinks
    /// </summary>
    /// <param name="d">Control Container</param>
    //public void FindControls(ControlCollection d, bool IsSearchPage)
    //{
    //    foreach (Control c in d)
    //    {
    //        if (c.HasControls())
    //        {
    //            this.FindControls(c.Controls, IsSearchPage);
    //        }
    //        if (c is Button)
    //        {
    //            Button btn = (Button)c;
    //            if (!btn.Text.Contains("Search") && !btn.Text.Contains("Clear") && !btn.Text.Contains("Query Invoice") && !btn.Text.Contains("Back"))
    //            {
    //                ((WebControl)c).Enabled = false;
    //            }
    //        }
    //        if (!IsSearchPage)
    //        {
    //            if (c is CheckBoxList)
    //            {
    //                //((WebControl)c).Enabled = false;
    //                ((WebControl)c).Attributes.Add("onclick", "return false;");
    //            }
    //            else if (c is DropDownList)
    //            {
    //                ((WebControl)c).Enabled = false;

    //            }
    //            else if (c is CheckBox || c is Image || c is TextBox  || c is RadioButton)
    //            {
    //                //((WebControl)c).Enabled = false;
    //                ((WebControl)c).Attributes.Add("onclick", "return false;");
    //                if (c is Image)
    //                    ((WebControl)c).Style.Add(HtmlTextWriterStyle.Cursor, "Text");
    //            }
    //            else if (c is LinkButton)
    //            {
    //                LinkButton lnk = (LinkButton)c;
    //                if (lnk.Text.Contains("Save") || lnk.Text.Contains("Cancel") || lnk.Text.Contains("Void") || lnk.Text.Contains("Update") || lnk.Text.Contains("Revision"))
    //                {
    //                    ((WebControl)c).Enabled = false;
    //                    ((WebControl)c).Style.Add(HtmlTextWriterStyle.Cursor, "Text");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (c is Panel)
    //            {
    //                Panel pnl = (Panel)c;
    //                if (pnl.ID == "pnlDetails")
    //                    pnl.Enabled = false;


    //            }
    //        }
    //    }
    //}
    //public bool IsAISSearchPage(string WebPageName)
    //{
    //    ArrayList AISSearchPageList;
    //    if (Session["AISSEARCHPAGELIST"] == null)
    //    {
    //        AISSearchPageList = new ArrayList();
    //        AISSearchPageList.Add("ACCTSEARCH.ASPX");
    //        AISSearchPageList.Add("ACCTSETUP/EXTCONTACTS.ASPX");
    //        AISSearchPageList.Add("ACCTSETUP/PROGRAMPERIOD.ASPX");
    //        AISSearchPageList.Add("TPAMANUAL/TPAPOSTINGMGMT.ASPX");
    //        AISSearchPageList.Add("INVOICE/ADJUSTMENTMGMT.ASPX");
    //        AISSearchPageList.Add("INVOICE/INVOICEMAILINGDTLS.ASPX");
    //        AISSearchPageList.Add("INVOICE/INVOICEINQUIRY.ASPX");
    //        AISSearchPageList.Add("ACCTMGMT/ACCOUNTASSIGNMENT.ASPX");
    //        AISSearchPageList.Add("ADJCALC/INVOICINGDASHBOARD.ASPX");
    //        AISSearchPageList.Add("ADJCALC/ADJUSTMENTREVIEW.ASPX");
    //        AISSearchPageList.Add("ADJCALC/COMBINEDELEMENTS.ASPX");
    //        AISSearchPageList.Add("ADJCALC/ESCROWADJUSTMENT.ASPX");
    //        AISSearchPageList.Add("ADJCALC/MISCINVOICING.ASPX");
    //        AISSearchPageList.Add("ADJCALC/NY-SIF.ASPX");
    //        AISSearchPageList.Add("ADJCALC/ADJUSTPROCESSINGCHKLST.ASPX");
    //        AISSearchPageList.Add("ADJCALC/PAIDLOSSBILLING.ASPX");
    //        AISSearchPageList.Add("ADJCALC/LRFPOSTINGDETAILS.ASPX");
    //        AISSearchPageList.Add("ACCTMGMT/DOCUMENTTRACKING.ASPX");
    //        AISSearchPageList.Add("ACCTMGMT/ACCOUNTDASHBOARD.ASPX");
    //        AISSearchPageList.Add("APPMGMT/INTERFACESTATUS.ASPX");
    //        AISSearchPageList.Add("LOSS/LOSSINFO.ASPX");
    //        Session["AISSEARCHPAGELIST"] = AISSearchPageList;
    //    }
    //    else
    //    {
    //        AISSearchPageList = (ArrayList)Session["AISSEARCHPAGELIST"];
    //    }
    //    return AISSearchPageList.Contains(WebPageName);

    //}

    //public bool IsAISPureSearchPage(string WebPageName)
    //{
    //        ArrayList AISPureSearchPageList = new ArrayList();
    //        AISPureSearchPageList.Add("ACCTSEARCH.ASPX");
    //        AISPureSearchPageList.Add("ADJCALC/INVOICINGDASHBOARD.ASPX");
    //        AISPureSearchPageList.Add("ACCTMGMT/ACCOUNTDASHBOARD.ASPX");
    //        AISPureSearchPageList.Add("APPMGMT/INTERFACESTATUS.ASPX");

    //        return (AISPureSearchPageList.Contains(WebPageName));
    //}


    public string IsAISTabPage(string WebPageName)
    {
        Hashtable AISTabPageList;
        AISTabPageList = (Hashtable)Application["AISTABPAGELIST"];
        if (AISTabPageList.Contains(WebPageName))
        {
            WebPageName = AISTabPageList[WebPageName].ToString();
        }
        return WebPageName;
    }

    /// <summary>
    /// Function to show Concurrennt Error
    /// </summary>
    /// <param name="success"></param>
    public void ShowConcurrentConflict(bool success, string errorMessage)
    {
        if (!success)
        {
            if (errorMessage.ToUpper().Contains("CHANGED"))
                ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
            else
                ShowError(GlobalConstants.ErrorMessage.ServerTooBusy);
        }
    }

    /// <summary>
    /// Function to show Concurrennt Error
    /// </summary>
    /// <param name="success"></param>
    public bool ShowConcurrentConflict(DateTime oldUpdatedDate, DateTime curUpdatedDate)
    {
        // If a null value was converted to DateTime, it returns 1/1/0001 12:00:00 AM (aka new DateTime())
        // If either date was null, we don't want to test for a concurrent conflicts b/c we'll get false errors
        if (oldUpdatedDate != new DateTime() && curUpdatedDate != new DateTime())
        {
            if (!(oldUpdatedDate.Day == curUpdatedDate.Day && oldUpdatedDate.Month == curUpdatedDate.Month
                && oldUpdatedDate.Year == curUpdatedDate.Year && oldUpdatedDate.Hour == curUpdatedDate.Hour
                && oldUpdatedDate.Minute == curUpdatedDate.Minute && oldUpdatedDate.Second == curUpdatedDate.Second))
            {
                ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                return false;
            }
        }
        else if ((oldUpdatedDate.ToString() == "1/1/0001 12:00:00 AM"
                    && curUpdatedDate.ToString() != "1/1/0001 12:00:00 AM") || (oldUpdatedDate.ToString() != "1/1/0001 12:00:00 AM"
                    && curUpdatedDate.ToString() == "1/1/0001 12:00:00 AM"))
        {
            ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Adds Inactive Lookup Data to the specified dropdown list control
    /// </summary>
    /// <param name="ddlList"></param>
    /// <param name="lookupID"></param>
    public void AddInActiveLookupData(ref DropDownList ddlList, int? lookupID)
    {
        ListItem item = new ListItem();
        if (lookupID.HasValue)
        {
            if (ddlList.Items.FindByValue(lookupID.ToString()) == null)
            {
                //string lkUpName = String.Empty;
                //lkUpName = (new ZurichNA.AIS.Business.Logic.BLAccess()).GetLookupName(lookupID);
                //if (lkUpName.Trim().Length > 0)
                //{
                //    item.Text = lkUpName;
                //    item.Value = lookupID.ToString();
                //    ddlList.Items.Add(item);
                //    ddlList.Items.FindByValue(lookupID.ToString()).Selected = true;
                //}
            }
            else
            {
                ddlList.SelectedIndex = -1;
                ddlList.Items.FindByValue(lookupID.ToString()).Selected = true;
            }
        }
    }

    /// <summary>
    /// Adds Inactive Lookup Data to the specified dropdown list control
    /// </summary>
    /// <param name="ddlList"> Dropdown List</param>
    /// <param name="lookupID">String LookupID</param>
    public void AddInActiveLookupData(ref DropDownList ddlList, string lookupID)
    {
        ListItem item = new ListItem();
        int intLookupID = 0;

        Int32.TryParse(lookupID, out intLookupID);

        if (ddlList.Items.FindByValue(lookupID.ToString()) == null)
        {
            //string lkUpName = String.Empty;
            //lkUpName = (new ZurichNA.AIS.Business.Logic.BLAccess()).GetLookupName(intLookupID);
            //if (lkUpName.Trim().Length > 0)
            //{
            //    item.Text = lkUpName;
            //    item.Value = lookupID.ToString();
            //    ddlList.Items.Add(item);
            //    ddlList.Items.FindByValue(lookupID.ToString()).Selected = true;
            //}
        }
        else
            ddlList.Items.FindByValue(lookupID.ToString()).Selected = true;
    }

    /// <summary>
    /// Adds Inactive Lookup Data to the specified dropdown list control
    /// </summary>
    /// <param name="ddlList"> Dropdown List</param>
    /// <param name="lookupID">String LookupID</param>
    public void AddInActiveLookupDataByText(ref DropDownList ddlList, string lookupName)
    {
        ListItem item = new ListItem();

        if (ddlList.Items.FindByText(lookupName) == null)
        {
            //int lkUpID = 0;
            //lkUpID = (new ZurichNA.AIS.Business.Logic.BLAccess()).GetLookUpID(lookupName);
            //if ( lkUpID > 0)
            //{
            //    item.Text = lookupName;
            //    item.Value = lkUpID.ToString();
            //    ddlList.Items.Add(item);
            //    ddlList.Items.FindByValue(lkUpID.ToString()).Selected = true;
            //}
        }
        else
            ddlList.Items.FindByText(lookupName).Selected = true;
    }

    /// <summary>
    /// Adds Inactive Policy Data to the specified CheckBox list control
    /// </summary>
    /// <param name="chkList">CheckBox List</param>
    /// <param name="policyID">Policy ID</param>
    public void AddInActiveLookupData(ref CheckBoxList chkList, int policyID)
    {
        ListItem item = new ListItem();
        if (policyID >= 0)
        {
            if (chkList.Items.FindByValue(policyID.ToString()) == null)
            {
                //string polName = String.Empty;
                //polName = (new ZurichNA.AIS.Business.Logic.PolicyBS()).getPolicyInfo(policyID).PolicyPerfectNumber;
                //if (polName.Trim().Length > 0)
                //{
                //    item.Text = polName;
                //    item.Value = policyID.ToString();
                //    chkList.Items.Add(item);
                //    chkList.Items.FindByValue(policyID.ToString()).Selected = true;
                //}
            }
            else
                chkList.Items.FindByValue(policyID.ToString()).Selected = true;
        }
    }

    /// <summary>
    /// Adds Inactive Policy Data to the specified CheckBox list control
    /// </summary>
    /// <param name="chkList"> Dropdown List</param>
    /// <param name="policyID">String Policy ID</param>
    public void AddInActiveLookupData(ref CheckBoxList chkList, string policyID)
    {
        ListItem item = new ListItem();
        int intPolicyID = 0;

        Int32.TryParse(policyID, out intPolicyID);

        if (intPolicyID >= 0)
        {
            if (chkList.Items.FindByValue(intPolicyID.ToString()) == null)
            {
                //string polName = String.Empty;
                //polName = (new ZurichNA.AIS.Business.Logic.PolicyBS()).getPolicyInfo(intPolicyID).PolicyPerfectNumber;
                //if (polName.Trim().Length > 0)
                //{
                //    item.Text = polName;
                //    item.Value = intPolicyID.ToString();
                //    chkList.Items.Add(item);
                //    chkList.Items.FindByValue(intPolicyID.ToString()).Selected = true;
                //}
            }
            else
                chkList.Items.FindByValue(intPolicyID.ToString()).Selected = true;
        }
    }

    /// <summary>
    /// Adds Inactive Lookup Data to the specified dropdown list control
    /// </summary>
    /// <param name="ddlList"> Dropdown List</param>
    /// <param name="lookupID">String LookupID</param>
    public void AddInActiveContactData(ref DropDownList ddlList, int personID)
    {
        ListItem item = new ListItem();

        if (ddlList.Items.FindByValue(personID.ToString()) == null)
        {
            //string lkUpName = String.Empty;
            //lkUpName = (new ZurichNA.AIS.Business.Logic.PersonBS()).getPersonRow(personID).FULLNAME;
            //if (lkUpName.Trim().Length > 0)
            //{
            //    item.Text = lkUpName;
            //    item.Value = personID.ToString();
            //    ddlList.Items.Add(item);
            //    ddlList.Items.FindByValue(personID.ToString()).Selected = true;
            //}
        }
        else
            ddlList.Items.FindByValue(personID.ToString()).Selected = true;
    }

    /// <summary>
    /// Adds Inactive Lookup Data to the specified dropdown list control
    /// </summary>
    /// <param name="ddlList"> Dropdown List</param>
    /// <param name="lookupID">String LookupID</param>
    public void AddInActiveExternalOrgData(ref DropDownList ddlList, int externOrgID)
    {
        ListItem item = new ListItem();

        if (ddlList.Items.FindByValue(externOrgID.ToString()) == null)
        {
            //string lkUpName = String.Empty;
            //lkUpName = (new ZurichNA.AIS.Business.Logic.BrokerBS()).Retrieve(externOrgID).FULL_NAME;
            //if (lkUpName.Trim().Length > 0)
            //{
            //    item.Text = lkUpName;
            //    item.Value = externOrgID.ToString();
            //    ddlList.Items.Add(item);
            //    ddlList.Items.FindByValue(externOrgID.ToString()).Selected = true;
            //}
        }
        else
            ddlList.Items.FindByValue(externOrgID.ToString()).Selected = true;
    }
    /// <summary>
    /// Adds Inactive Lookup Data to the specified dropdown list control
    /// </summary>
    /// <param name="ddlList"> Dropdown List</param>
    /// <param name="lookupID">String LookupID</param>
    public void AddInActivePostTransData(ref DropDownList ddlList, int PostTransID)
    {
        ListItem item = new ListItem();

        if (ddlList.Items.FindByValue(PostTransID.ToString()) == null)
        {
            //string lkUpName = String.Empty;
            //lkUpName = (new ZurichNA.AIS.Business.Logic.PostingTransactionTypeBS()).Retrieve(PostTransID).TRANS_TXT;
            //if (lkUpName.Trim().Length > 0)
            //{
            //    item.Text = lkUpName;
            //    item.Value = PostTransID.ToString();
            //    ddlList.Items.Add(item);
            //    ddlList.Items.FindByValue(PostTransID.ToString()).Selected = true;
            //}
        }
        else
            ddlList.Items.FindByValue(PostTransID.ToString()).Selected = true;
    }

    /// <summary>
    /// Adds Inactive Policy Data to the specified CheckBox list control
    /// </summary>
    /// <param name="chkList"> Dropdown List</param>
    /// <param name="policyID">String Policy ID</param>
    public void AddInActivePolicyData(ref DropDownList ddlList, string policyID)
    {
        ListItem item = new ListItem();
        int intPolicyID = 0;

        Int32.TryParse(policyID, out intPolicyID);

        if (intPolicyID >= 0)
        {
            if (ddlList.Items.FindByValue(intPolicyID.ToString()) == null)
            {
                //string polName = String.Empty;
                //polName = (new ZurichNA.AIS.Business.Logic.PolicyBS()).getPolicyInfo(intPolicyID).PolicyPerfectNumber;
                //if (polName.Trim().Length > 0)
                //{
                //    item.Text = polName;
                //    item.Value = intPolicyID.ToString();
                //    ddlList.Items.Add(item);
                //    ddlList.Items.FindByValue(intPolicyID.ToString()).Selected = true;
                //}
            }
            else
                ddlList.Items.FindByValue(intPolicyID.ToString()).Selected = true;
        }
    }

    /// <summary>
    /// Waits randomly for given Milli Seconds
    /// </summary>
    /// <param name="milliSecs">Value in Milli Seconds</param>
    public void RandomWait(int milliSecs)
    {
        int mSecs = 0;
        int maxMS = milliSecs;
        Random rand = new Random();
        mSecs = rand.Next(maxMS);
        System.Threading.Thread.Sleep(mSecs);
    }
//    public static class EnumerableExtension
//{
//public static T PickRandom<T>(this IEnumerable<T> source)
//{
//return source.PickRandom(1).Single();
//}
 
//public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
//{
//    return source.Shuffle.Take(count);
//}
 
//public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
//{
//return source.OrderBy(x =>Guid.NewGuid());
//}
//}

    /// <summary>
    /// Function for check value is negitive or not.
    /// </summary>
    /// <param name="strAmount">Value in double/int</param>
    public string isNegitive(string strAmount)
    {
        double Num;
        bool isNum = double.TryParse(strAmount, out Num);

        if (isNum)
        {
            int intConvetedValue = Convert.ToInt32(Num);
            if (intConvetedValue < 0)
                return "$(" + Math.Abs(intConvetedValue).ToString() + ")";
            else
                return "$" + intConvetedValue.ToString();
        }
        else
        {
            return "";
        }
    }
    private string RemoveWIDinQueryString(string URL)
    {
        string returnURL;
        int end;
        int appresendStart = URL.IndexOf("&wID");
        int questStart = URL.IndexOf("?wID");
        if (appresendStart > -1)
        {
            end = URL.IndexOf("&", appresendStart + 1);
            if (end > -1)
            {
                returnURL = URL.Remove(appresendStart, end - appresendStart);
            }
            else 
            {
                returnURL = URL.Remove(appresendStart);
            }
        }
        else if (questStart > -1)
        {
            end = URL.IndexOf("&", questStart);
            if (end > -1)
            {
                returnURL = URL.Remove(questStart + 1, end - questStart);
            }
            else
            {
                returnURL = URL.Remove(questStart);
            }
        }
        else
        {
            returnURL = URL;
        }
        return returnURL;

    }
}
