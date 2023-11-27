using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using ZurichNA.AIS.ExceptionHandling;

using ZurichNA.LSP.Framework;
/// <summary>
/// <para>
/// GeneralError
/// </para>
/// This page is called when page displays a standard error message to the user.
/// The page displays a header, suggestion to avoid the error, and detailed technical information.
/// </summary>
public partial class GeneralError : System.Web.UI.Page
{
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

    private Common common = null;
    private string genError = "General Error";
    private string suggestedAction = "General Error <br /> " +
                                  "Please call the Global IT Service Desk for assistance.";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        // Clear the menu tab session variables to prevent error when hitting back in browser
        Master.Page.Title = "Error Page";
        Session["MenuIdTier1"] = null;
        Session["MenuIdTier2"] = null;

        if (Session["RetroException"] != null)
        {
            Exception ex = (Exception)Session["RetroException"];
            common = new Common(this.GetType());
            if (ex is RetroBaseException)
            {
                RetroBaseException myex = (RetroBaseException)ex;
                //Logging
                if (User.Identity.Name != null)
                {
                    common.Logger.Error(ex.Message + " error occurred for user:" + User.Identity.Name.ToString(), myex);
                }
                else
                {
                    common.Logger.Error(ex.Message, myex);
                }
                setLocalVariables(myex);
            }
            else
            {
                //Logging
                if (User.Identity.Name != null)
                {
                    common.Logger.Error(ex.Message + " error occurred for user:" + User.Identity.Name.ToString(), ex);
                }
                else
                {
                    common.Logger.Error(ex.Message, ex);
                }
                setLocalVariables(ex);
            }

            Session["RetroException"] = null;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ex"></param>
    private void setLocalVariables(RetroBaseException ex)
    {
        lblError.Text = ex.GeneralError.ToString();
        lblComment.Text = ex.SuggestedAction.ToString();
        if (ex.InnerException != null)
            lblErrorMsg.Text = ex.InnerException.Message.ToString();
        else
            lblErrorMsg.Text = "Unknown Error. Please retry navigating to current working screen.";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ex"></param>
    private void setLocalVariables(Exception ex)
    {
        lblError.Text = this.genError;
        lblComment.Text = this.suggestedAction;
        lblErrorMsg.Text = ex.Message.ToString();
    }
}
