using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using ZurichNA.LSP.Framework;

/// <summary>
/// <para>
/// Retromaster
/// </para>
/// This is Master page for the web application
/// </summary>
public partial class Retromaster : System.Web.UI.MasterPage, ICallbackEventHandler
{
    private Common common = null;
    /// <summary>
    /// Set the theme to "RetroTheme" for every page in the site
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["Theme"] == null)
        {
            // No theme has been chosen. Choose a default
            // (or set a blank string to make sure no theme
            // is used).
            Session["Theme"] = "RetroTheme";
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //// Emit JavaScript to the page for the timeout code.  3 hours
        //StringBuilder strBuild = new StringBuilder();
        //strBuild.Append("\n<script language=\"JavaScript\">");
        //strBuild.Append("\n setTimeout(\"ShowTimeOutDiv()\",((1)*60000),\"javascript\");");
        //strBuild.Append("\n");
        //strBuild.Append("\n</script>");
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "TimeoutCode", strBuild.ToString());

        //To Remove Session off while exiting application
        string callBackReference =
            Page.ClientScript.GetCallbackEventReference(this, "arg", "LogOffUser", "");
        string logOffUserCallBackScrpt =
            "function LogOffUserCallBack(arg, context) { " + callBackReference + "; }";

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LogOffUserCallBack",
                                                    logOffUserCallBackScrpt, true);
        //*****************************************************

        if (Page.Request.Browser.Browser != "IE")
        {
            BrowserCheck.Text = "You are viewing this site in an unsupported browser. Please upgrade your browser to Internet Explorer 6.";
        }

        AppSettingsReader appSettings = new AppSettingsReader();
        AISUser currentUser = (AISUser)System.Web.HttpContext.Current.Items["CurrentUser"];

        lblwelcome.Text = " Welcome, [" + currentUser.Role + "] " + currentUser.UserID;

        // Display appropriate menu items in the Left Navigation Menu
        lblVersion.Text = (string)appSettings.GetValue("Footer", typeof(string));
        lblVersion.Text += (string)appSettings.GetValue("Version", typeof(string));
        lblVersion.Text += " - " + HttpContext.Current.Server.MachineName;

        //Binds the Retro Menu
        AISMenu.CreateMenu(currentUser.Role, ref AISMenuData);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        TopNav.Dispose();

    }

    /// <summary>
    /// <para>
    /// Retromaster:TopNav_MenuItemClick( )
    /// </para>
    /// Method is executed when a user clicks a menu item in Top Navigation Menu
    /// It directs the user to the requested page
    /// </summary>
    protected void TopNav_MenuItemClick(object sender, MenuEventArgs e)
    {
        Session["MenuIdTier1"] = TopNav.SelectedValue;

        //The FindItem method can not be passed a string containing a "/", it will mistake it for a path name
        try
        {
            if (Session["MenuIdTier1"] != null && Session["MenuIdTier1"].ToString().Contains("/") == false)
            {
                TopNav.FindItem(Session["MenuIdTier1"].ToString()).Selected = true;
            }
        }
        catch
        {
            Session["MenuIdTier1"] = null;
        }

        TopNav.Enabled = false;

    }


    protected void TopNav_MenuItemDataBound(object sender, MenuEventArgs e)
    {
        string strDirtyValue = "document.getElementById('" + Page.Master.FindControl("hdnDirtyBit").ClientID + "').value";
        string strRealValue = "document.getElementById('" + Page.Master.FindControl("hdnRealBit").ClientID + "').value";

        if (e.Item.Text.ToUpper() != "EXIT")
            e.Item.NavigateUrl = "Javascript:CheckWithoutSave('../" + e.Item.NavigateUrl + "');";
        //            e.Item.NavigateUrl = "Javascript:" + strDirtyValue + "=1;window.location.href='../" + e.Item.NavigateUrl + "';";
        else
            e.Item.NavigateUrl = "Javascript:" + strDirtyValue + "=1;" + e.Item.NavigateUrl.Replace("JavaScript:", "");
    }

    protected void TopNav_PreRender(object sender, EventArgs e)
    {

    }


    /// <summary>
    /// This function handles the validate event of the Custom Validator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RaiseError(object sender, ServerValidateEventArgs e)
    {
        try
        {
            //set the isvalid property to false to trigger Validator to the error
            e.IsValid = false;
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }




    #region ICallbackEventHandler Members

    private string strCallbackStatus = String.Empty;
    public string GetCallbackResult()
    {
        return strCallbackStatus;
    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        /// Adding a try to ensure this logic does not disturb
        /// the normal flow of the loggoff or session ending process
        try
        {
            common = new Common(this.GetType());
            if (System.Web.HttpContext.Current.Items["CurrentUser"] != null)
            {
                string userName = ((AISUser)System.Web.HttpContext.Current.Items["CurrentUser"]).UserID;
                string strInfo = "User: " + userName + " logged out the AIS Application";
                common.Logger.Info(strInfo);
            }
        }
        catch (Exception ex)
        {

        }
        Session.RemoveAll();
        Session.Abandon();
        strCallbackStatus = "Success";
    }

    #endregion

}
