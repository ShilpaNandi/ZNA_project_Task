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
using ZurichNA.AIS.WebSite.ZDWJavaWS_CAD;
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
                //IIdentity Iden = HttpContext.Current.User.Identity;
                //WindowsIdentity winIden = (WindowsIdentity)Iden;
                //WindowsImpersonationContext wic = winIden.Impersonate();
                try
                {
                    string strZDWKey = Request.QueryString["ZDWKey"];
                    Preview(strZDWKey);
                }
                catch(Exception ex)
                {
                    new ApplicationStatusLogBS().WriteLog(0, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, 99999);
                    lblZDWerror.Text = "Unable to open PDF documents. Please contact support team.";
                    return;
                }
                //finally
                //{
                //    wic.Undo();
                //}
            }
        }

        private void Preview(string strZDWKey)
        {

            bool IsCanadaAcct = false;
            InvoiceDriverBS invDriverBS = new InvoiceDriverBS();
            DataTable dtProgrSummZDWKey = invDriverBS.GetProgramSummary_Info_ByZdwKey(strZDWKey);
            if (dtProgrSummZDWKey != null)
            {
                if (dtProgrSummZDWKey.Rows.Count > 0)
                {
                    IsCanadaAcct = (new AccountBS()).IsCanadaAccount(dtProgrSummZDWKey.Rows[0].Field<int>("prem_adj_id"));
                }
            }
            else
            {

                IsCanadaAcct = (new AccountBS()).IsCanadaAccountbyZDWKey(strZDWKey);
            }

            if (IsCanadaAcct)
            {
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService objJavaWS = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService();
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument();
                try
                {

                    UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                    objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                    objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                    objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                    objDocument = objJavaWS.retrieveDocument("ZDW_DOC04." + strZDWKey, "D");
                }

                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    new ApplicationStatusLogBS().WriteLog(0, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, 9999999);
                    //ShowError("ZDW Interface is not responding. Please try this action later.");
                    lblZDWerror.Text = "ZDW Interface is not responding. Please try this action later.";
                    return;
                }

                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                objDocContentElement = objDocument.documentContentElement[0];

                //Phase -3
                string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
                sFormat = sFormat.Replace(":", "");
                sFormat = sFormat.Replace("-", "");
                string filename = objDocument.typeName.ToString().Split('_')[0];

                Response.ClearContent();
                Response.ClearHeaders();
                Response.Clear();                
                //Response.ContentType = "application/pdf";
                if (objDocument.typeName.ToString().Contains(".xlsx"))
                {
                    Response.AddHeader("content-type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename + "_" + sFormat + ".xlsx");
                }
                else
                {
                    Response.AddHeader("content-type", "application/pdf");
                }
                Response.AddHeader("X-Content-Type-Options", "nosniff");                
                Response.BinaryWrite(objDocContentElement.theBinaryValue.ToArray());
                Response.End(); 
                //HttpContext.Current.ApplicationInstance.CompleteRequest(); // reverted veracode fix
            }
            else
            {
                ZurichNA.AIS.WebSite.ZDWJavaWS.CommunicationManagerServiceWse objJavaWS = new ZurichNA.AIS.WebSite.ZDWJavaWS.CommunicationManagerServiceWse();
                ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument();
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

                ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                objDocContentElement = objDocument.documentContentElement[0];

                //Phase -3
                string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
                sFormat = sFormat.Replace(":", "");
                sFormat = sFormat.Replace("-", "");
                string filename = objDocument.typeName.ToString().Split('_')[0];

                Response.ClearContent();
                Response.ClearHeaders();
                Response.Clear();
                if (objDocument.typeName.ToString().Contains(".xlsx"))
                {
                    Response.AddHeader("content-type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename + "_" + sFormat + ".xlsx");
                }
                else
                {
                    Response.AddHeader("content-type", "application/pdf");
                }
                Response.AddHeader("X-Content-Type-Options", "nosniff");
                Response.BinaryWrite(objDocContentElement.theBinaryValue.ToArray());
                Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest(); // reverted veracode fix
            }
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


        /// <summary>
        /// This function handles the validate event of the Custom Validator
        /// </summary>
        /// <param name="sender"></param>
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
