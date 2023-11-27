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
using System.IO;
using System.Collections.Generic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using System.Threading;
using ZurichNA.AIS.WebSite.Reports;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using ZurichNA.LSP.Framework;
using System.Text;
using System.Text.RegularExpressions;
using ZurichNA.LSP.Framework.Business;
using System.Web.SessionState;
using ZurichNA.AIS.WebSite.ZDWJavaWS;
using Microsoft.Web.Services3;
using System.Web.Services;
using Microsoft.Web.Services3.Security.Tokens;
using System.Security.Principal;

namespace ZurichNA.AIS.WebSite.Invoice
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IIdentity Iden = HttpContext.Current.User.Identity;
                WindowsIdentity winIden = (WindowsIdentity)Iden;
                WindowsImpersonationContext wic = winIden.Impersonate();
                try
                {
                    string strZDWKey = Request.QueryString["ZDWKey"];
                    Preview(strZDWKey);
                }
                catch { }
                finally
                {
                    wic.Undo();
                }
            }
        }

        private void Preview(string strZDWKey)
        {
            CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();
            ElectronicDocument objDocument = new ElectronicDocument();
            try
            {

                UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                objDocument = objJavaWS.retrieveDocument(strZDWKey, "D");
            }

            catch (System.Web.Services.Protocols.SoapException ex)
            {
                new ApplicationStatusLogBS().WriteLog(0, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, 9999999);
                //ShowError("ZDW Interface is not responding. Please try this action later.");
                lblZDWerror.Text = "ZDW Interface is not responding. Please try this action later.";
                return;
            }
            
            DocumentContentElement objDocContentElement = new DocumentContentElement();
            objDocContentElement = objDocument.documentContentElement[0];
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(objDocContentElement.theBinaryValue.ToArray());
        }

        //public void ShowError(string errorMessage)
        //{
        //    try
        //    {
        //        AISErrorValidator.ErrorMessage = errorMessage;
        //        AISSummary.CssClass = "ValidationSummary";

        //        //summary.Font.Size = FontUnit.Small;
        //        //summary.ForeColor = System.Drawing.Color.Red;
        //        AISErrorValidator.Validate();
        //        AISSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
        //}


        ///// <summary>
        ///// This function handles the validate event of the Custom Validator
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void RaiseError(object sender, ServerValidateEventArgs e)
        //{
        //    try
        //    {
        //        //set the isvalid property to false to trigger Validator to the error
        //        e.IsValid = false;
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
        //}

    }
}
