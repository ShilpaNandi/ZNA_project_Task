using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using System.Threading;
using ZurichNA.AIS.WebSite.Reports;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using ZurichNA.LSP.Framework;
using System.Text;
using System.Text.RegularExpressions;
using ZurichNA.LSP.Framework.Business;
using System.Web.SessionState;
using ZurichNA.AIS.WebSite.ZDWJavaWS;
using Microsoft.Web.Services3;
using System.Web.Services;
using Microsoft.Web.Services3.Security.Tokens;
using SSG = SpreadsheetGear;




namespace ZurichNA.AIS.WebSite.AdjCalc
{
    public partial class PreviewInvoice : System.Web.UI.Page
    {
        protected static Common common = null;
        Array _VCarrMasterParamsInt = Array.CreateInstance(typeof(String), 10);
        Array _VCarrMasterParamsExt = Array.CreateInstance(typeof(String), 10);
        StringBuilder _VCsbSubParamsExt = new StringBuilder();
        StringBuilder _VCsbMasterParamsExt = new StringBuilder();
        Array _VCarrSubParamsExt = Array.CreateInstance(typeof(String), 31);
        Array _VCarrSubParamsInt = Array.CreateInstance(typeof(String), 31);
        StringBuilder _VCsbSubParamsInt = new StringBuilder();
        StringBuilder _VCsbMasterParamsInt = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strDocName = Request.QueryString["DocName"];
                string strAdjNo=Request.QueryString["AdjNo"];
                if (strDocName.Trim().ToUpper() == "WA")
                {
                    ShowError("Report is in progress for " + strDocName);
                    return;
                }
                PremiumAdjustmentBE premAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(Convert.ToInt32(strAdjNo));
                string strInvNo = string.Empty;
                //if (premAdjBE.FNL_INVC_NBR_TXT != "")
                //{
                //    strInvNo = premAdjBE.FNL_INVC_NBR_TXT;
                //}
                if(premAdjBE.INVC_NBR_TXT != "" && premAdjBE.INVC_NBR_TXT!=null)
                {
                    strInvNo = premAdjBE.INVC_NBR_TXT;
                }
                bool HistFlag = false;
                if (premAdjBE.HISTORICAL_ADJ_IND.HasValue)
                {
                    HistFlag = Convert.ToBoolean(premAdjBE.HISTORICAL_ADJ_IND);
                }
                Preview(Convert.ToInt32(strAdjNo),strInvNo,HistFlag,strDocName);
                

            }
        }
        public void Preview(int iAdjNo, string strInvNo, bool HistFlag, string strDocumentName)
        {

            switch (strDocumentName.Trim().ToUpper())
            {
                case "LOSS FUND":
                    //Escrow Fund Adjustment
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptEscrowMasterReport.rpt", "srptEscrowFundAdjustment.rpt", "SuppressEscrow", strDocumentName);
                    break;
                case "ILRFINTERNAL":
                    //Loss Reimbursement Fund Adjustment-Internal
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptILRFInternalMasterReport.rpt", "srptLRFInternal.rpt", "SuppressLRFInternal", strDocumentName);
                    break;
                case "ILRFEXTERNAL":
                    //Loss Reimbursement Fund Adjustment-External
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptILRFExternalMasterReport.rpt", "srptLRFExternal.rpt", "SuppressLRFExternal", strDocumentName);
                    break;
                case "INVOICE":
                    //Adjustment Invoice Exhibit
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptInvoiceMasterReport.rpt", "srptAdjusInvoice.rpt", "SuppressSectionAdjInv", strDocumentName);
                    break;
                case "LBA":
                    //Loss Based Assessment Exhibit
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptLBAMasterReport.rpt", "srptLossBasedAssessmentsCalculation.rpt", "SuppressLBA", strDocumentName);
                    break;
                case "CHF":
                    //Claim Handling Fee
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCHFMasterReport.rpt", "srptClaimHandlingFeeSummary.rpt", "", strDocumentName);
                    break;
                case "COMBINED ELEMENTS":
                    //Combined Elements Exhibit - Internal
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCombinedElementsMasterReport.rpt", "srptCombinedElements.rpt", "SuppressCombinedElements", strDocumentName);
                    break;
                //case "NY-SIF":
                //    //Adjustment of NY Second Injury Fund
                //    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptNYSIFMasterReport.rpt", "srptAdjNYSecInjFund.rpt", "SuppressAdjNY", strDocumentName);
                //    break;
                case "RML":
                    //Residual Market Subsidy Charge
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptRMLMasterReport.rpt", "srptResidualMarkSubCharge.rpt", "SuppressResidualMarkSubChange", strDocumentName);
                    break;
                case "EXCESS LOSS":
                    //Excess Loss
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptExcessMasterReport.rpt", "srptExcessLoss.rpt", "SuppressExcessLoss", strDocumentName);
                    break;
                case "WA":
                    //need clarifications
                    break;
                //Retrospective Premium Adjustment
                case "RETROSPECTIVE PREMIUM ADJUSTMENT":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptRetroMasterReport.rpt", "srptRetroPremAdjustmentMain.rpt", "SuppressRetroPremAdj", strDocumentName);
                    break;
                //Retrospective Adjustment Legend
                case "RETROSPECTIVE ADJUSTMENT LEGEND":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptRetroLegendMasterReport.rpt", "srptRetroPremAdjLegend.rpt", "SuppressRetroLegend", strDocumentName);
                    break;
                //Broker Letter
                case "BROKER LETTER":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptBrokerMasterReport.rpt", "srptBrokerLetter.rpt", "SuppressBrokerLetter", strDocumentName);
                    break;
                //Surchareges and Assessments
                case "SURCHARGES ASSESSMENTS":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptSurchargesMasterReport.rpt", "srptSurchargesAssessments.rpt", "SuppressSurcharges", strDocumentName);
                    break;
                //Processing Checklist
                case "PROCESSING CHECKLIST":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptProcessChecklistRetroMasterReport.rpt", "srptProcessingCheckList.rpt", "SuppressProcessingCheckList", strDocumentName);
                    break;
                //Aries Posting Details
                case "ARIES POSTING DETAILS":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptARiESMasterReport.rpt", "srptARiEsPostingDetails.rpt", "SuppressAries", strDocumentName);
                    break;
                //Cesar coding Worksheet
                case "CESAR CODING WORKSHEET":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCesarCodingMasterReport.rpt", "srptCesarCodingWorksheet.rpt", "SuppressCesar", strDocumentName);
                    break;
                //Cumulative Totals
                case "PAID LOSS BILLINGS FOR CURRENT ADJUSTMENT PERIOD":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCumulativeMasterReport.rpt", "srptCumTotalsWorksheet.rpt", "SuppressCumTotals", strDocumentName);
                    break;
                //retrospective tax exhibit (external):
                case "STATE SALES (EXTERNAL)":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptILRFExternalTaxMasterReport.rpt", "srptLRFExternaTax.rpt", "SuppressLRFTaxExternal", strDocumentName);
                    break;
                //Retrospective Tax Exhibit (Internal):
                case "STATE SALES (INTERNAL)":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptILRFInternalTaxMasterReport.rpt", "srptLRFInternaTax.rpt", "SuppressLRFTaxInternal", strDocumentName);
                    break;
                case "REMITTANCE ADVICE":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptRemittanceMasterReport.rpt", "srptRemittance.rpt", "SuppressRemittance", strDocumentName);
                    break;
               //Internal PDF
                case "INTERNAL PDF":
                    GenerateInternalPDFReportWithTOC(iAdjNo, HistFlag);
                    break;
               //External PDF
                case "EXTERNAL PDF":
                    GenerateExternalPDFReportWithTOC(iAdjNo, HistFlag);
                    break;
                //External Spreadsheet
                case "EXTERNAL SPREADSHEET":
                    DownLaodFileSpreadSheetGear(iAdjNo, strInvNo, 0, strDocumentName);
                    break;
                //Internal Spreadsheet
                case "INTERNAL SPREADSHEET":
                    DownLaodFileSpreadSheetGear(iAdjNo, strInvNo, 1, strDocumentName);
                    break;
                // Phase 3 - Adding new Exhibit -( DMR 147050 AIS Exhibit Redesign)
                case "PROGRAM SUMMARY SPREADSHEET":
                    DownloadFileProgramSummaySpreadsheetGear(iAdjNo, strInvNo, strDocumentName);
                    break; 
               //Cesar Coding PDF
                case "CESAR CODING PDF":
                    PremAdjustmentBS objInvBS = new PremAdjustmentBS();
                    PremiumAdjustmentBE objInvBE = new PremiumAdjustmentBE();
                    PremiumAdjustmentBE objInvPreviousBE = new PremiumAdjustmentBE();
                    objInvBE = objInvBS.getPremiumAdjustmentRow(iAdjNo);
                    if (objInvBE.REL_PREM_ADJ_ID.HasValue)
                    {
                        objInvPreviousBE = objInvBS.getPremiumAdjustmentRow(Convert.ToInt32(objInvBE.REL_PREM_ADJ_ID));
                        if (objInvPreviousBE.ADJ_RRSN_IND == true)
                            GenerateRevisedCesarCodingPDFReport(iAdjNo, objInvPreviousBE.PREMIUM_ADJ_ID, HistFlag);
                        else
                            GenerateCesarCodingPDFReport(iAdjNo, HistFlag);  
                    }
                    else
                    {
                        GenerateCesarCodingPDFReport(iAdjNo, HistFlag);  

                    }
                    
                    break;
               
            }


        }

       

        public void GenerateReport(int iAdjNo, string strInvNo, bool HistFlag, string strMasterReport, string strSubReport, string strSubReportParameter, string strDocName)
        {
            ReportDocument objMainPreview = new ReportDocument();
            //rptMasterReport objMainPreview = new rptMasterReport();
            objMainPreview.Load(Server.MapPath("\\Reports\\" + strMasterReport));
            //Pdf Connections
            GenerateReportConnections(objMainPreview);

            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmERPInd = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            prmFlag.Value = 1;
            prmERPInd.Value = false;//This is dummy variable
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo);
            objMainPreview.SetParameterValue("@FLAG", prmFlag);
            if (strSubReportParameter != "")
                objMainPreview.SetParameterValue(strSubReportParameter, "View");
            /*****************Setting Master Report Parameters Value Begin******************/

            /*****************Setting Sub Reports Parameters Value Begin******************/
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, strSubReport);
            objMainPreview.SetParameterValue("@FLAG", prmFlag, strSubReport);
            objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, strSubReport);
            if (strDocName.ToUpper() != "BROKER LETTER" && strDocName.ToUpper() != "RETROSPECTIVE ADJUSTMENT LEGEND")
            {
                objMainPreview.SetParameterValue("@INVNO", prmInvNo, strSubReport);
            }
            if (strDocName.ToUpper() == "SURCHARGES ASSESSMENTS")
            {
                objMainPreview.SetParameterValue("@REVFLAGPREV", prmRevFlag, strSubReport);
            }
            if (strDocName.ToUpper() == "CESAR CODING WORKSHEET" || strDocName.ToUpper() == "RML")
            {
                objMainPreview.SetParameterValue("IsFlipSigns", prmRevFlag, strSubReport);
            }
            if (strDocName.ToUpper() == "RML")
            {
                objMainPreview.SetParameterValue("@REVFLAGPREV", prmRevFlag, strSubReport);
            }
            if (strDocName.ToUpper() == "ILRFINTERNAL")
            {
                objMainPreview.SetParameterValue("@CMTCATGID", 318, strSubReport);
            }
            if (strDocName.ToUpper() == "ILRFEXTERNAL")
            {
                objMainPreview.SetParameterValue("@CMTCATGID", 375, strSubReport);
            }
            if (strDocName.ToUpper() == "BROKER LETTER")
            {
                objMainPreview.SetParameterValue("@CMTCATGID", 339, strSubReport);
            }
            if (strDocName.ToUpper() == "INVOICE")
            {
                objMainPreview.SetParameterValue("@CMTCATGID", 319, strSubReport);
            }
            if (strDocName.ToUpper() == "STATE SALES (EXTERNAL)")
            {
                objMainPreview.SetParameterValue("@CMTCATGID", 533, strSubReport);
            }
            if (strDocName.ToUpper() == "STATE SALES (INTERNAL)")
            {
                objMainPreview.SetParameterValue("@CMTCATGID", 534, strSubReport);
            }
            if (strDocName.ToUpper() == "COMBINED ELEMENTS")
            {
                objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptadjinvCombinedElements.rpt");
                objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptadjinvCombinedElements.rpt");
                objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptadjinvCombinedElements.rpt");
            }
            if (strDocName.ToUpper() == "RETROSPECTIVE PREMIUM ADJUSTMENT")
            {
                objMainPreview.SetParameterValue("SuppressRetroPremAdj", "View");
                objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjustmentMain.rpt");
                objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjustmentMain.rpt");
                objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjustmentMain.rpt");
                objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjustmentMain.rpt");
            }
            if (strDocName.ToUpper() == "RETROSPECTIVE ADJUSTMENT LEGEND")
            {
                objMainPreview.SetParameterValue("SuppressRetroLegend", "View");
                objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjLegend.rpt");
                objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjLegend.rpt");
                objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjLegend.rpt");
                objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjLegend.rpt");
                objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptCalcInfoLegend.rpt");
                objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptCalcInfoLegend.rpt");
                objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCalcInfoLegend.rpt");
                objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptCalcInfoLegend.rpt");
                objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptLegendSecond.rpt");
                objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptLegendSecond.rpt");
                objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLegendSecond.rpt");
                objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptLegendSecond.rpt");
            }
            if (strDocName.ToUpper() == "STATE SALES (INTERNAL)")
            {
                objMainPreview.SetParameterValue("SuppressLRFTaxInternal", "View");
                objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternaTax.rpt");
                objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptLRFExternaTax.rpt");
                objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternaTax.rpt");
                objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternaTax.rpt");
                objMainPreview.SetParameterValue("@CMTCATGID", 534, "srptLRFExternaTax.rpt");
            
            
            }
            if (strDocName.ToUpper() == "REMITTANCE ADVICE")
            {
                objMainPreview.SetParameterValue("SuppressRemittance", "View");
                objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptRemittance.rpt");
                objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptRemittance.rpt");
                objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptRemittance.rpt");
                objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRemittance.rpt");
                objMainPreview.SetParameterValue("@CMTCATGID", 318, "srptRemittance.rpt");


            }
            
            /*****************Setting Sub Reports Parameters Value End******************/
            Stream memStream;
            memStream = (Stream)objMainPreview.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //MemoryStream memStream; - WinServer Changes for crystal Reports
            //memStream = (MemoryStream)objMainPreview.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            try
            {
                byte[] byteArray = new byte[memStream.Length];
                memStream.Read(byteArray, 0, (int)memStream.Length);
                //byte[] byteArray = memStream.ToArray();
                string strFileName = string.Empty;
                if (strDocName == "RETROSPECTIVE ADJUSTMENT LEGEND")
                {
                    strFileName = "RETRO ADJUSTMENT LEGEND" + ".pdf";
                }
                else if (strDocName == "RETROSPECTIVE PREMIUM ADJUSTMENT")
                {
                    strFileName = "RETRO PREMIUM ADJUSTMENT" + ".pdf";
                }
                else
                {
                    strFileName = strDocName + ".pdf";
                }
                Response.Clear();
                Response.ContentType = "application/pdf";
                //Veracode Fix
                Response.AddHeader("content-disposition", "attachment;filename=" + Server.UrlDecode(Server.UrlEncode(strFileName)));//EAISA-5 Veracode flaw fix updated
                Response.AddHeader("content-length", memStream.Length.ToString());
                Response.BinaryWrite(byteArray);
                memStream.Close();
            }
            catch (Exception ex)
            {
                ShowError("Unable to Preview the Report. Please contact Application Support Team");
                return;
            }
            Response.Flush();
            Response.End();
            objMainPreview.Close();
            objMainPreview.Dispose();

        }

        public void GenerateReportSaveOnDisk(int iAdjNo, string strInvNo, bool HistFlag, string strMasterReport, string strSubReport, string strSubReportParameter, string strDocName,string companyCode)
        {
            ReportDocument objMainPreview = new ReportDocument();
           // objMainPreview.Load(@"C:\My Projects\TFS2010\AIS\Development\ AIS C to Z Enhancement\AIS\AIS.WebSite\Reports\" + strMasterReport);


            objMainPreview.Load(ConfigurationManager.AppSettings["ReportPath"].ToString() + strMasterReport);

            //Pdf Connections
            GenerateReportConnections(objMainPreview);

            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmERPInd = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            prmFlag.Value = 2;
            prmERPInd.Value = false;//This is dummy variable
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo);
            objMainPreview.SetParameterValue("@FLAG", prmFlag);
            if (strSubReportParameter != "")
                objMainPreview.SetParameterValue(strSubReportParameter, "View");
            /*****************Setting Master Report Parameters Value Begin******************/

            /*****************Setting Sub Reports Parameters Value Begin******************/
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, strSubReport);
            objMainPreview.SetParameterValue("@FLAG", prmFlag, strSubReport);
            objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, strSubReport);
            objMainPreview.SetParameterValue("@CMTCATGID", 319, strSubReport);
            objMainPreview.SetParameterValue("@INVNO", prmInvNo, strSubReport);

            /*****************Setting Sub Reports Parameters Value End******************/
            //MemoryStream memStream;  - WinServer Changes for crystal Reports
            //memStream = (MemoryStream)objMainPreview.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream memStream;
            memStream = (Stream)objMainPreview.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            //byte[] byteArray = memStream.ToArray();
            byte[] byteArray = new byte[memStream.Length];
            memStream.Read(byteArray, 0, (int)memStream.Length);
            string strFileName = string.Empty;
            strFileName = strDocName + ".pdf";
            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            //Response.AddHeader("content-length", memStream.Length.ToString());
            //Response.BinaryWrite(byteArray);
            //memStream.Close();
            //Response.Flush();
            //Response.End();

            objMainPreview.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            objMainPreview.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
            //objDiskOpt.DiskFileName = @"C:\Bookmarks\" + strFileName;
            if (companyCode == "Z01")
                objDiskOpt.DiskFileName = ConfigurationManager.AppSettings["ZDW_USA_SharedFolder"] + strFileName;
            else if (companyCode == "ZC2")
                objDiskOpt.DiskFileName = ConfigurationManager.AppSettings["ZDW_CAD_SharedFolder"] + strFileName;
            objMainPreview.ExportOptions.DestinationOptions = objDiskOpt;
            objMainPreview.Export();
            
            objMainPreview.Close();
            objMainPreview.Dispose();
        }

        public void GenerateInternalPDFReport(int iAdjNo, bool HistFlag)
        {
            //Internal PDF Generation
            ReportDocument objMainDraftInternal = new ReportDocument();
            objMainDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));
            //Pdf Connections
            GenerateReportConnections(objMainDraftInternal);
            string strInvNo = string.Empty;
            PremiumAdjustmentBE PremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iAdjNo);
            if (PremAdjBE.INVC_NBR_TXT != null && PremAdjBE.INVC_NBR_TXT != "")
                strInvNo = PremAdjBE.INVC_NBR_TXT;
            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Draft Invoice
            prmFlag.Value = 1;
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainDraftInternal.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/

            //Draft Invoice Internal PDF
            IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
            for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainDraftInternal, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            /*****************Setting Sub Reports Parameters Value Begin******************/
            setInternalSubReportParameters(objMainDraftInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            
            Stream st;
            //MemoryStream st; - WinServer Changes for crystal Reports
            try
            {
                //st = (MemoryStream)objMainDraftInternal.ExportToStream(ExportFormatType.PortableDocFormat); - WinServer Changes for crystal Reports
                st = (Stream)objMainDraftInternal.ExportToStream(ExportFormatType.PortableDocFormat);
                //byte[] arr = st.ToArray();                
                byte[] arr = new byte[st.Length];
                st.Read(arr, 0, (int)st.Length);
                string strFileName = "Internal PDF.pdf";
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.AddHeader("content-length", st.Length.ToString());
                Response.BinaryWrite(arr);
                st.Close();
            }
            catch (Exception ex)
            {
                ShowError("Unable to Preview the Report. Please contact Application Support Team");
                return;
            }
            Response.Flush();
            Response.End();
            objMainDraftInternal.Close();
            objMainDraftInternal.Dispose();
        }
        public void GenerateInternalPDFReportWithTOC(int iAdjNo, bool HistFlag)
        {
            //Internal PDF Generation
            //ReportDocument objMainDraftInternal = new ReportDocument();
            //objMainDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));
            //Pdf Connections
            //GenerateReportConnections(objMainDraftInternal);
            string strInvNo = string.Empty;
            PremiumAdjustmentBE PremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iAdjNo);
            if (PremAdjBE.INVC_NBR_TXT != null && PremAdjBE.INVC_NBR_TXT != "")
                strInvNo = PremAdjBE.INVC_NBR_TXT;
            //ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            //ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            //ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            //ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            //ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            //ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            object objAdjNo = iAdjNo;
            //Draft Invoice
            object objprmFlag = 1;
            object objprmFlipSigns = false;
            object objprmRevFlag = false;
            object objprmInvNo = strInvNo;
            object objprmHistFlag = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            _VCarrMasterParamsInt.SetValue("\"Parm1:" + objAdjNo.ToString() + "\"", 1);
            _VCarrMasterParamsInt.SetValue("\"Parm2:" + objprmFlag.ToString() + "\"", 2);
            //objMainDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
            //objMainDraftInternal.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/

            //Draft Invoice Internal PDF
            IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
            for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
            {

                VCsetMasterReportParameter(null, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);
            }
            /*****************Setting Sub Reports Parameters Value Begin******************/
            VCsetInternalSubReportParameters(objAdjNo, objprmFlag, objprmFlipSigns, objprmInvNo, objprmRevFlag, objprmHistFlag);
            //setInternalSubReportParameters(objMainDraftInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            try
            {
                //st = (MemoryStream)objMainDraftInternal.ExportToStream(ExportFormatType.PortableDocFormat); - WinServer Changes for crystal Reports
                //byte[] arr = st.ToArray();
                string[] strArray = VCshellScriptExecution('I', objAdjNo);
                if (strArray != null)
                {
                    if (strArray[0] == "I")
                    {
                        FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                        byte[] bc = new byte[fs.Length];
                        fs.Read(bc, 0, (Int32)fs.Length);
                        fs.Close();
                        //byte[] arr = new byte[st.Length];
                        //st.Read(arr, 0, (int)st.Length);
                        string strFileName = "Internal PDF.pdf";
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                        Response.AddHeader("content-length", bc.Length.ToString());
                        Response.BinaryWrite(bc);
                        //st.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                ShowError("Unable to Preview the Report. Please contact Application Support Team");
                return;
            }
            Response.Flush();
            Response.End();
            //objMainDraftInternal.Close();
        }
        /// <summary>
        /// Method to set the parameters to Subreports to External for Visual Cut
        /// </summary>
        /// <param name="objMain"></param>
        /// <param name="prmAdjNo"></param>
        /// <param name="prmFlag"></param>
        public void VCsetExternalSubReportParameters(object prmAdjNo, object prmFlag, object prmFlipSigns, object prmInvNo, object prmRevFlag, object prmHistFlag)
        {
            //  _VCarrSubParamsExt.SetValue("[Parm_12]:View", 12);

            string IntExtFlag = "E";

            _VCarrSubParamsExt.SetValue("[Parm_22]:" + prmInvNo.ToString(), 22);
            _VCarrSubParamsExt.SetValue("[Parm_23]:" + prmHistFlag.ToString(), 23);
            _VCarrSubParamsExt.SetValue("[Parm_24]:" + prmRevFlag.ToString(), 24);
            _VCarrSubParamsExt.SetValue("[Parm_25]:339", 25);
            _VCarrSubParamsExt.SetValue("[Parm_26]:319", 26);
            _VCarrSubParamsExt.SetValue("[Parm_27]:375", 27);
            _VCarrSubParamsExt.SetValue("[Parm_28]:318", 28);
            _VCarrSubParamsExt.SetValue("[Parm_29]:608", 29);
            _VCarrSubParamsExt.SetValue("[Parm_32]:" + IntExtFlag, 30);
            // _VCarrSubParamsExt.SetValue("[Parm_30]:View", 30);
            //return _VCarrSubParamsExt.ToString();

        }
        public void VCsetInternalSubReportParameters(object prmAdjNo,
                object prmFlag,
                object prmFlipSigns,
                object prmInvNo,
                object prmRevFlag,
                object prmHistFlag)
        {
            // _VCarrSubParamsInt.SetValue("[Parm_30]:View", 30);
            if (string.IsNullOrEmpty(prmInvNo.ToString()))
                prmInvNo = "  ";

            string IntExtFlag = "I";
            _VCarrSubParamsInt.SetValue("[Parm_22]:" + prmInvNo.ToString(), 22);
            _VCarrSubParamsInt.SetValue("[Parm_23]:" + prmHistFlag.ToString(), 23);
            _VCarrSubParamsInt.SetValue("[Parm_24]:" + prmRevFlag.ToString(), 24);
            _VCarrSubParamsInt.SetValue("[Parm_25]:339", 25);
            _VCarrSubParamsInt.SetValue("[Parm_26]:319", 26);
            _VCarrSubParamsInt.SetValue("[Parm_27]:375", 27);

            _VCarrSubParamsInt.SetValue("[Parm_28]:318", 28);
            _VCarrSubParamsInt.SetValue("[Parm_29]:608", 29);
            _VCarrSubParamsInt.SetValue("[Parm_32]:" + IntExtFlag, 30);
            //return _VCarrSubParamsInt.ToString();

        }
        public void GenerateExternalPDFReport(int iAdjNo, bool HistFlag)
        {
            //External PDF Generation
            ReportDocument objMainDraftExternal = new ReportDocument();
            objMainDraftExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //Pdf Connections
            GenerateReportConnections(objMainDraftExternal);
            string strInvNo = string.Empty;
            PremiumAdjustmentBE PremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iAdjNo);
            if (PremAdjBE.INVC_NBR_TXT != null && PremAdjBE.INVC_NBR_TXT != "")
                strInvNo = PremAdjBE.INVC_NBR_TXT;
            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Draft Invoice
            prmFlag.Value = 1;
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainDraftExternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainDraftExternal.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/
            //Draft Invoice External PDF
            IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
            for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainDraftExternal, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
          
            /*****************Setting Sub Reports Parameters Value Begin******************/
            setExternalSubReportParameters(objMainDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/

            //MemoryStream st; - WinServer Changes for crystal Reports
            //st = (MemoryStream)objMainDraftExternal.ExportToStream(ExportFormatType.PortableDocFormat);
            Stream st;
            st = (Stream)objMainDraftExternal.ExportToStream(ExportFormatType.PortableDocFormat);

            try
            {
                //byte[] arr = st.ToArray(); 
                byte[] arr = new byte[st.Length];
                st.Read(arr, 0, (int)st.Length);
                string strFileName = "External PDF.pdf";
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.AddHeader("content-length", st.Length.ToString());
                Response.BinaryWrite(arr);
                st.Close();
            }
            catch (Exception ex)
            {
                ShowError("Unable to Preview the Report. Please contact Application Support Team");
                return;
            }
            Response.Flush();
            Response.End();
            objMainDraftExternal.Close();
            objMainDraftExternal.Dispose();
        }
        public void VCsetMasterReportParameter(ReportDocument objMain, string strAttachCode,
          bool blStatus, int intFlag, char strInternalFlag, int iAdjNo)
        {
            AdjustmentReviewCommentsBS objReportAvailable = new AdjustmentReviewCommentsBS();

            switch (strAttachCode)
            {
                //Broker Letter Exhibit
                case "INV1":
                    #region INV1
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "BROKER LETTER"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                //Set SuppressBrokerLetter 
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    _VCarrMasterParamsInt.SetValue("\"Parm7:View\"", 7);
                                }
                                else
                                {
                                    _VCarrMasterParamsInt.SetValue("\"Parm7:Suppress\"", 7);
                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrMasterParamsExt.SetValue("\"Parm7:View\"", 7);
                                else
                                    _VCarrMasterParamsExt.SetValue("\"Parm7:Suppress\"", 7);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                        }
                        else
                        {
                            if (_VCarrMasterParamsInt.GetValue(7) == null)
                                _VCarrMasterParamsInt.SetValue("\"Parm7:Suppress\"", 7);
                            if (_VCarrMasterParamsExt.GetValue(7) == null)
                                _VCarrMasterParamsExt.SetValue("\"Parm7:Suppress\"", 7);
                        }
                    }
                    else
                    {
                        if (_VCarrMasterParamsInt.GetValue(7) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm7:Suppress\"", 7);
                        if (_VCarrMasterParamsExt.GetValue(7) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm7:Suppress\"", 7);

                    }
                    #endregion
                    break;

                //Adjustment Invoice Exhibit
                case "INV2":
                    #region Inv2
                    //Set SuppressSectionAdjInv
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "SUMMARY INVOICE"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    _VCarrMasterParamsInt.SetValue("\"Parm3:View\"", 3);
                                }

                                else
                                {
                                    _VCarrMasterParamsInt.SetValue("\"Parm3:Suppress\"", 3);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrMasterParamsExt.SetValue("\"Parm3:View\"", 3);
                                else
                                    _VCarrMasterParamsExt.SetValue("\"Parm3:Suppress\"", 3);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                        }
                        else
                        {
                            if (_VCarrMasterParamsInt.GetValue(3) == null)
                                _VCarrMasterParamsInt.SetValue("\"Parm3:Suppress\"", 3);
                            if (_VCarrMasterParamsExt.GetValue(3) == null)
                                _VCarrMasterParamsExt.SetValue("\"Parm3:Suppress\"", 3);
                        }

                    }
                    else
                    {
                        if (_VCarrMasterParamsInt.GetValue(3) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm3:Suppress\"", 3);
                        if (_VCarrMasterParamsExt.GetValue(3) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm3:Suppress\"", 3);

                    }
                    #endregion
                    break;

                //Retrospective Premium Adjustment Exhibit
                case "INV3":
                    #region Inv3
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE PREMIUM ADJUSTMENT"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_16]:View", 16);
                                }
                                else
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_16]:Suppress", 16);
                                }

                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_16]:View", 16);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_16]:Suppress", 16);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                        }
                        else
                        {
                            if (_VCarrSubParamsInt.GetValue(16) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_16]:Suppress", 16);
                            if (_VCarrSubParamsExt.GetValue(16) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_16]:Suppress", 16);
                        }

                    }
                    else
                    {
                        if (_VCarrSubParamsInt.GetValue(16) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_16]:Suppress", 16);
                        if (_VCarrSubParamsExt.GetValue(16) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_16]:Suppress", 16);

                    }
                    #endregion
                    break;


                //Retrospective Premium Adjustment Legend Exhibit
                case "INV4":
                    #region Inv4
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE ADJUSTMENT LEGEND"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_10]:View", 10);

                                }
                                //objMain.SetParameterValue("SuppressRetroLegend", "View");
                                else
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_10]:Suppress", 10);

                                }
                            //objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_10]:View", 10);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_10]:Suppress", 10);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                        }
                        else
                        {
                            if (_VCarrSubParamsInt.GetValue(10) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_10]:Suppress", 10);
                            if (_VCarrSubParamsExt.GetValue(10) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_10]:Suppress", 10);

                        }
                        //objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                    }
                    else
                    {
                        if (_VCarrSubParamsInt.GetValue(10) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_10]:Suppress", 10);
                        if (_VCarrSubParamsExt.GetValue(10) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_10]:Suppress", 10);

                    }
                    //objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                    #endregion
                    break;

                //Loss Based Assessment Exhibit
                case "INV5":
                    #region Inv5
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LBA"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    _VCarrMasterParamsInt.SetValue("\"Parm4:View\"", 4);
                                }
                                //objMain.SetParameterValue("SuppressLBA", "View");
                                else
                                {
                                    _VCarrMasterParamsInt.SetValue("\"Parm4:Suppress\"", 4);
                                }
                            //objMain.SetParameterValue("SuppressLBA", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrMasterParamsExt.SetValue("\"Parm4:View\"", 4);
                                else
                                    _VCarrMasterParamsExt.SetValue("\"Parm4:Suppress\"", 4);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLBA", "Suppress");
                        }
                        else
                        {
                            if (_VCarrMasterParamsInt.GetValue(4) == null)
                                _VCarrMasterParamsInt.SetValue("\"Parm4:Suppress\"", 4);
                            if (_VCarrMasterParamsExt.GetValue(4) == null)
                                _VCarrMasterParamsExt.SetValue("\"Parm4:Suppress\"", 4);

                        }
                        //objMain.SetParameterValue("SuppressLBA", "Suppress");
                    }
                    else
                    {
                        if (_VCarrMasterParamsInt.GetValue(4) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm4:Suppress\"", 4);
                        if (_VCarrMasterParamsExt.GetValue(4) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm4:Suppress\"", 4);

                    }
                    //objMain.SetParameterValue("SuppressLBA", "Suppress");
                    #endregion
                    break;

                //Claims Handling Fee
                case "INV6":
                    #region Inv6
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CHF"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_18]:View", 18);

                                }
                                //objMain.SetParameterValue("SuppressCHF", "View");
                                else
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_18]:Suppress", 18);

                                }
                            //objMain.SetParameterValue("SuppressCHF", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_18]:View", 18);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_18]:Suppress", 18);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCHF", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressCHF", "Suppress");

                            if (_VCarrSubParamsInt.GetValue(18) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_18]:Suppress", 18);
                            if (_VCarrSubParamsExt.GetValue(18) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_18]:Suppress", 18);
                        }
                    }
                    else
                    {
                        if (_VCarrSubParamsInt.GetValue(18) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_18]:Suppress", 18);
                        if (_VCarrSubParamsExt.GetValue(18) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_18]:Suppress", 18);
                    }
                    //objMain.SetParameterValue("SuppressCHF", "Suppress");
                    #endregion
                    break;


                //Excess Losses
                case "INV7":
                    #region Inv7
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "EXCESS LOSS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressExcessLoss", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_11]:View", 11);

                                }
                                else
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_11]:Suppress", 11);

                                }
                            //objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_11]:View", 11);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_11]:Suppress", 11);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(11) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_11]:Suppress", 11);
                            if (_VCarrSubParamsExt.GetValue(11) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_11]:Suppress", 11);
                        }

                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(11) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_11]:Suppress", 11);
                        if (_VCarrSubParamsExt.GetValue(11) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_11]:Suppress", 11);
                    }
                    #endregion
                    break;

                //Cumulative Totals Worksheet
                case "INV8":
                    #region Inv8
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PAID LOSS BILLINGS FOR CURRENT ADJUSTMENT PERIOD"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressCumTotals", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_8]:View", 8);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_8]:Suppress", 8);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(8) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_8]:Suppress", 8);
                            if (_VCarrSubParamsExt.GetValue(8) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(8) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_8]:Suppress", 8);
                        if (_VCarrSubParamsExt.GetValue(8) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                    }
                    #endregion
                    break;

                //Residual Market Subsidy Charge
                case "INV9":
                    #region Inv9
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RML"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    _VCarrSubParamsInt.SetValue("[Parm_14]:View", 14);

                                }
                                //objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                                else
                                {
                                    //objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_14]:Suppress", 14);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_14]:View", 14);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_14]:Suppress", 14);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                        }
                        else
                        {
                            // objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(14) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_14]:Suppress", 14);
                            if (_VCarrSubParamsExt.GetValue(14) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_14]:Suppress", 14);
                        }
                    }
                    else
                    {
                        // objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(14) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_14]:Suppress", 14);
                        if (_VCarrSubParamsExt.GetValue(14) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_14]:Suppress", 14);
                    }
                    #endregion
                    break;


                //Surcharges & Assesments
                case "INV10":
                    #region Inv10
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETRO PREMIUM BASED SURCHARGES & ASSESSMENTS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    _VCarrSubParamsInt.SetValue("[Parm_12]:View", 12);
                                else
                                    _VCarrSubParamsInt.SetValue("[Parm_12]:Suppress", 12);
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_12]:View", 12);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_12]:Suppress", 12);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressSurcharges", "View");
                        }
                        else
                        {
                            _VCarrSubParamsInt.SetValue("[Parm_12]:Suppress", 12);
                            _VCarrSubParamsExt.SetValue("[Parm_12]:Suppress", 12);
                        }
                    }
                    else
                    {
                        _VCarrSubParamsInt.SetValue("[Parm_12]:Suppress", 12);
                        _VCarrSubParamsExt.SetValue("[Parm_12]:Suppress", 12);
                    }
                    #endregion
                    break;




                //Loss Reimbursement Fund Adjustment -External
                case "INV11":
                    #region Inv12
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (EXTERNAL)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressLRFExternal", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_9]:View", 9);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_9]:Suppress", 9);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_9]:View", 9);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_9]:Suppress", 9);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(9) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_9]:Suppress", 9);
                            if (_VCarrSubParamsExt.GetValue(9) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_9]:Suppress", 9);
                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(9) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_9]:Suppress", 9);
                        if (_VCarrSubParamsExt.GetValue(9) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_9]:Suppress", 9);
                    }
                    #endregion
                    break;

                //Escrow Fund Adjustment
                case "INV12":
                    #region Inv13
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LOSS FUND"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressEscrow", "View");
                                    _VCarrMasterParamsInt.SetValue("\"Parm5:View\"", 5);

                                }
                                else
                                    //objMain.SetParameterValue("SuppressEscrow", "Suppress");
                                    _VCarrMasterParamsInt.SetValue("\"Parm5:Suppress\"", 5);
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrMasterParamsExt.SetValue("\"Parm5:View\"", 5);
                                else
                                    _VCarrMasterParamsExt.SetValue("\"Parm5:Suppress\"", 5);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressEscrow", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressEscrow", "Suppress");
                            if (_VCarrMasterParamsInt.GetValue(5) == null)
                                _VCarrMasterParamsInt.SetValue("\"Parm5:Suppress\"", 5);
                            if (_VCarrMasterParamsExt.GetValue(5) == null)
                                _VCarrMasterParamsExt.SetValue("\"Parm5:Suppress\"", 5);

                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressEscrow", "Suppress");
                        if (_VCarrMasterParamsInt.GetValue(5) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm5:Suppress\"", 5);
                        if (_VCarrMasterParamsExt.GetValue(5) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm5:Suppress\"", 5);

                    }
                    #endregion
                    break;

                //Loss Reimbursement Fund Adjustment-Internal
                case "INV13":
                    #region Inv14
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (INTERNAL)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                            {
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    //objMain.SetParameterValue("SuppressLRFInternal", "View");
                                    _VCarrMasterParamsInt.SetValue("\"Parm6:View\"", 6);

                            }
                            else
                                //objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                                _VCarrMasterParamsInt.SetValue("\"Parm6:Suppress\"", 6);
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrMasterParamsExt.SetValue("\"Parm6:View\"", 6);
                                else
                                    _VCarrMasterParamsExt.SetValue("\"Parm6:Suppress\"", 6);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                        }
                        else
                        {
                            // objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                            if (_VCarrMasterParamsInt.GetValue(6) == null)
                                _VCarrMasterParamsInt.SetValue("\"Parm6:Suppress\"", 6);
                            if (_VCarrMasterParamsExt.GetValue(6) == null)
                                _VCarrMasterParamsExt.SetValue("\"Parm6:Suppress\"", 6);

                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                        if (_VCarrMasterParamsInt.GetValue(6) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm6:Suppress\"", 6);
                        if (_VCarrMasterParamsExt.GetValue(6) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm6:Suppress\"", 6);

                    }
                    #endregion
                    break;

                //Aries Posting Details 
                case "INV14":
                    #region Inv15
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ARIES POSTING DETAILS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressAries", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_17]:View", 17);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressAries", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_17]:Suppress", 17);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_17]:View", 17);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_17]:Suppress", 17);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressAries", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressAries", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(17) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_17]:Suppress", 17);
                            if (_VCarrSubParamsExt.GetValue(17) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_17]:Suppress", 17);
                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressAries", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(17) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_17]:Suppress", 17);
                        if (_VCarrSubParamsExt.GetValue(17) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_17]:Suppress", 17);
                    }
                    #endregion
                    break;


                //Combined Elements Exhibit - Internal
                case "INV15":
                    #region Inv16
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "COMBINED ELEMENTS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressCombinedElements", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_15]:View", 15);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_15]:Suppress", 15);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_15]:View", 15);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_15]:Suppress", 15);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(15) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_15]:Suppress", 15);
                            if (_VCarrSubParamsExt.GetValue(15) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_15]:Suppress", 15);
                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(15) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_15]:Suppress", 15);
                        if (_VCarrSubParamsExt.GetValue(15) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_15]:Suppress", 15);
                    }
                    #endregion
                    break;

                //Processing and Distribution Checklist
                case "INV16":
                    #region Inv17
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PROCESSING CHECKLIST"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressProcessingCheckList", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_13]:View", 13);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_13]:Suppress", 13);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_13]:View", 13);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_13]:Suppress", 13);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(13) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_13]:Suppress", 13);
                            if (_VCarrSubParamsExt.GetValue(13) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_13]:Suppress", 13);

                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(13) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_13]:Suppress", 13);
                        if (_VCarrSubParamsExt.GetValue(13) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_13]:Suppress", 13);
                    }
                    #endregion
                    break;

                //Cesar Coding WorkSheet
                case "INV17":
                    #region Inv18
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CESAR CODING WORKSHEET"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressCesar", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_19]:View", 19);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressCesar", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_19]:Suppress", 19);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_19]:View", 19);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_19]:Suppress", 19);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCesar", "View");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressCesar", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(19) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_19]:Suppress", 19);
                            if (_VCarrSubParamsExt.GetValue(19) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_19]:Suppress", 19);
                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(19) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_19]:Suppress", 19);
                        if (_VCarrSubParamsExt.GetValue(19) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_19]:Suppress", 19);


                    }
                    #endregion
                    break;
                case "INV18":
                    #region Inv19
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "State Sales & Service Tax Exhibit (External)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressCesar", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_20]:View", 20);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressCesar", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_20]:Suppress", 20);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_20]:View", 20);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_20]:Suppress", 20);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFTaxExternal", "View");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressCesar", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(20) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_20]:Suppress", 20);
                            if (_VCarrSubParamsExt.GetValue(20) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_20]:Suppress", 20);
                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(20) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_20]:Suppress", 20);
                        if (_VCarrSubParamsExt.GetValue(20) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_20]:Suppress", 20);


                    }
                    #endregion
                    break;
                case "INV19":
                    #region Inv19
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "State Sales & Service Tax Exhibit (Internal)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                {
                                    //objMain.SetParameterValue("SuppressCesar", "View");
                                    _VCarrSubParamsInt.SetValue("[Parm_21]:View", 21);

                                }
                                else
                                {
                                    //objMain.SetParameterValue("SuppressCesar", "Suppress");
                                    _VCarrSubParamsInt.SetValue("[Parm_21]:Suppress", 21);

                                }
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    _VCarrSubParamsExt.SetValue("[Parm_21]:View", 21);
                                else
                                    _VCarrSubParamsExt.SetValue("[Parm_21]:Suppress", 21);
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFTaxInternal", "View");
                        }
                        else
                        {
                            //objMain.SetParameterValue("SuppressCesar", "Suppress");
                            if (_VCarrSubParamsInt.GetValue(21) == null)
                                _VCarrSubParamsInt.SetValue("[Parm_21]:Suppress", 21);
                            if (_VCarrSubParamsExt.GetValue(21) == null)
                                _VCarrSubParamsExt.SetValue("[Parm_21]:Suppress", 21);
                        }
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(21) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_21]:Suppress", 21);
                        if (_VCarrSubParamsExt.GetValue(21) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_21]:Suppress", 21);


                    }
                    #endregion
                    break;
            }


        }
      
        public void GenerateExternalPDFReportWithTOC(int iAdjNo, bool HistFlag)
        {

            string strInvNo = string.Empty;
            PremiumAdjustmentBE PremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iAdjNo);
            if (PremAdjBE.INVC_NBR_TXT != null && PremAdjBE.INVC_NBR_TXT != "")
                strInvNo = PremAdjBE.INVC_NBR_TXT;

            object objAdjNo = iAdjNo;
            object objprmFlag = 1;
            object objprmFlipSigns = false;
            object objprmRevFlag = true;
            object objprmInvNo = strInvNo;
            object objprmHistFlag = HistFlag;

            /*****************Setting Master Report Parameters Value Begin******************/

            _VCarrMasterParamsExt.SetValue("\"Parm1:" + objAdjNo.ToString() + "\"", 1);
            _VCarrMasterParamsExt.SetValue("\"Parm2:" + objprmFlag.ToString() + "\"", 2);
            /*****************Setting Master Report Parameters Value Begin******************/
            //Draft Invoice External PDF
            IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
            for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
            {

                VCsetMasterReportParameter(null, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(),
                    Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2,
                    Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            /*****************Setting Sub Reports Parameters Value Begin******************/
            VCsetExternalSubReportParameters(objAdjNo, objprmFlag, objprmFlipSigns, objprmInvNo, objprmRevFlag, objprmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            try
            {
                //st = (MemoryStream)objMainDraftInternal.ExportToStream(ExportFormatType.PortableDocFormat);
                //byte[] arr = st.ToArray();
                string[] strArray = VCshellScriptExecution('E', objAdjNo);
                if (strArray != null)
                {
                    if (strArray[0] == "E")
                    {
                        FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                        byte[] bc = new byte[fs.Length];
                        fs.Read(bc, 0, (Int32)fs.Length);
                        fs.Close();
                        //byte[] arr = new byte[st.Length];
                        //st.Read(arr, 0, (int)st.Length);
                        string strFileName = "External PDF.pdf";
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                        Response.AddHeader("content-length", bc.Length.ToString());
                        Response.BinaryWrite(bc);
                        //st.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Unable to Preview the Report. Please contact Application Support Team");
                return;
            }
            Response.Flush();
            Response.End();
            //objMainDraftExternal.Close();
        }

        /// <summary>
        /// This method is used for to generate internal/external report in spread sheet. 
        /// </summary>
        /// <param name="AdjNo"></param>
        /// <param name="strInvNo"></param>
        /// <param name="extIntFlag">0 is for external and 1 is for internal spreadsheet</param>
        /// <param name="strDocumentName"></param>
        public void DownLaodFileSpreadSheetGear(int AdjNo, string strInvNo, int extIntFlag, string strDocumentName)
        {
            try
            {
                // Create a new workbook and worksheet.
                SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
                int sheetIndex = 0;
                int cellIndex = 0;

                // int debugindex = 0; //for break point

                #region Sheet:Adjustment Invoice

                DataTable dtAdj = (new ProgramPeriodsBS()).GetAdjReport(AdjNo, 1, false, 0);

                if (dtAdj != null && dtAdj.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetAdj = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetAdj.Name = "Adjustment Invoice";

                    // Get the worksheet cells reference. 
                    SSG.IRange cellsAdj = worksheetAdj.Cells;
                    DataRow dr = dtAdj.Rows[0];
                    cellIndex = 1;

                    //MergeAndFormatCellsWithColor(cellsAdj, "A", ":D", "Invoice Reference Number", SSG.HAlign.Left, cellIndex);
                    cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Reference Number";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":D", strInvNo, SSG.HAlign.Left, cellIndex);

                    cellIndex = cellIndex + 2;

                    cellsAdj["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsAdj["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsAdj["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "E", ":F", Convert.ToString(dr["BROKER NAME"]), SSG.HAlign.Left, cellIndex);

                    //{GetAdjustmentInvoice;1.CUSTOMER CITY}+", "+{GetAdjustmentInvoice;1.CUSTOMER STATE}+" "+{GetAdjustmentInvoice;1.CUSTOMER ZIP}
                    cellIndex = cellIndex + 2;

                    MergeAndFormatCellsWithColor(cellsAdj, "A", ":B", "Adjustment Summary :", SSG.HAlign.Left, cellIndex);
                    cellIndex++;

                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);

                    cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Date";
                    cellsAdj["B" + cellIndex.ToString()].Value = "Loss Valuation Date";
                    cellsAdj["C" + cellIndex.ToString()].Value = "Due Date";
                    cellsAdj["D" + cellIndex.ToString()].Value = "Total Amount Billed";
                    cellIndex++;

                    var item = (from items in dtAdj.AsEnumerable()
                                where (items.Field<Int32>("PREM ADJ ID") == AdjNo)
                                      && (!string.IsNullOrEmpty(items.Field<string>("VALUATION DATE")))
                                select
                                new { VALUATIONDATE = items.Field<string>("VALUATION DATE"), TOTALAMOUNTBILLED = items.Field<Decimal>("TOTAL AMOUNT BILLED") }
                                ).First();

                    cellsAdj["A" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                    cellsAdj["B" + cellIndex.ToString()].Value = Convert.ToString(item.VALUATIONDATE);
                    //cellsAdj["C" + cellIndex.ToString()].Value = DateTime.Now.AddDays(20).ToShortDateString();
                    cellsAdj["D" + cellIndex.ToString()].Value = Convert.ToString(item.TOTALAMOUNTBILLED);
                    SetCurrencyFormat(cellsAdj["D" + cellIndex.ToString()]);
                    cellIndex = cellIndex + 2;

                    MergeAndFormatCellsWithColor(cellsAdj, "A", ":B", "Program Details :", SSG.HAlign.Left, cellIndex);
                    cellIndex++;

                    bool isDisplayA = false;
                    bool isDisplayB = false;
                    //(Convert.ToBoolean(Convert.ToString(dr["CUSTOMER REL ID"])) == false || Convert.ToBoolean(Convert.ToString(dr["CUSTOMER REL ID"])) == true)
                    if (dr["CUSTOMER REL ID"] != DBNull.Value && (dr["REL ACCT IND"] != DBNull.Value && Convert.ToInt32(dr["REL ACCT IND"]) == 0))
                    {
                        isDisplayB = true;
                    }
                    else if ((dr["CUSTOMER REL ID"] == DBNull.Value || Convert.ToBoolean(Convert.ToString(dr["CUSTOMER REL ID"])) != false) && (dr["REL ACCT IND"] != DBNull.Value && Convert.ToInt32(dr["REL ACCT IND"]) == 1))
                    {
                        isDisplayA = true;
                    }

                    //{GetAdjustmentInvoice;1.} = false and {GetAdjustmentInvoice;1.REL ACCT IND} = 0
                    //    or
                    //    {GetAdjustmentInvoice;1.CUSTOMER REL ID} = true and {GetAdjustmentInvoice;1.REL ACCT IND} = 0

                    //        {GetAdjustmentInvoice;1.CUSTOMER REL ID} <> false And {GetAdjustmentInvoice;1.REL ACCT IND} = 1

                    if (isDisplayA)
                    {
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);

                        cellsAdj["A" + cellIndex.ToString()].Value = "Insured Name";
                        cellsAdj["B" + cellIndex.ToString()].Value = "Adjustment Type";
                        cellsAdj["C" + cellIndex.ToString()].Value = "Program Period";
                        cellsAdj["D" + cellIndex.ToString()].Value = "Adjustment Number";
                        cellsAdj["E" + cellIndex.ToString()].Value = "Amount Billed";
                        cellIndex++;

                        foreach (DataRow drRows in dtAdj.Rows)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["INSURED NAME"]);
                            cellsAdj["B" + cellIndex.ToString()].Value = Convert.ToString(drRows["ADJUSTMENT TYPE"]);
                            cellsAdj["C" + cellIndex.ToString()].Value = Convert.ToString(drRows["PROGRAM PERIOD"]);
                            cellsAdj["D" + cellIndex.ToString()].Value = Convert.ToString(drRows["ADJUSTMENT NUMBER"]);
                            cellsAdj["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "AMOUNT BILLED");
                            SetCurrencyFormat(cellsAdj["E" + cellIndex.ToString()]);
                            cellIndex++;
                        }
                    }
                    else if (isDisplayB)
                    {
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);

                        cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Type";
                        cellsAdj["B" + cellIndex.ToString()].Value = "Program Period";
                        cellsAdj["C" + cellIndex.ToString()].Value = "Adjustment Number";
                        cellsAdj["D" + cellIndex.ToString()].Value = "Amount Billed";
                        cellIndex++;

                        foreach (DataRow drRows in dtAdj.Rows)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["ADJUSTMENT TYPE"]);
                            cellsAdj["B" + cellIndex.ToString()].Value = Convert.ToString(drRows["PROGRAM PERIOD"]);
                            cellsAdj["C" + cellIndex.ToString()].Value = Convert.ToString(drRows["ADJUSTMENT NUMBER"]);
                            cellsAdj["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "AMOUNT BILLED");
                            SetCurrencyFormat(cellsAdj["D" + cellIndex.ToString()]);
                            cellIndex++;
                        }
                    }

                    worksheetAdj.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetAdj.UsedRange.ColumnCount; col++)
                    {
                        worksheetAdj.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:RPA

                DataTable dtRPAdj = (new ProgramPeriodsBS()).GetRPAReport(AdjNo, 1, false);
                DataTable dtRPSummary = (new ProgramPeriodsBS()).GetRPASummaryReport(AdjNo, 1, false);

                if (dtRPAdj != null && dtRPAdj.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetRPA = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetRPA.Name = "RPA";

                    SSG.IRange cellsRPA = worksheetRPA.Cells;
                    DataRow dr = dtRPAdj.Rows[0];
                    cellIndex = 1;

                    // Add column headers. 
                    cellsRPA["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsRPA["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsRPA, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsRPA["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsRPA["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsRPA, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 2;

                    FormatHeaderTypeCell(cellsRPA["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                    cellsRPA["A" + cellIndex.ToString()].Value = "Program Period";
                    cellsRPA["B" + cellIndex.ToString()].Value = "Adjustment Number";
                    cellsRPA["C" + cellIndex.ToString()].Value = "Loss Type";
                    cellsRPA["D" + cellIndex.ToString()].Value = "Loss Val Date";
                    cellsRPA["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                    cellsRPA["F" + cellIndex.ToString()].Value = "Adjustment Date";
                    cellIndex++;

                    List<DataTable> details1 = dtRPAdj.AsEnumerable()
                                         .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();

                    Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(details1, "PREM ADJ PGM ID");

                    //Sorting fix
                    List<DataTable> details = new List<DataTable>();
                    foreach (var item in prgPrdList)
                    {
                        DataTable dtPrgPrd = (from dts in dtRPAdj.AsEnumerable()
                                              where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                              select dts).CopyToDataTable();
                        details.Add(dtPrgPrd);
                    }

                    for (int i = 0; i < details.Count; i++)
                    {
                        DataTable detail = details[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsRPA["A" + cellIndex.ToString()].Value = prgPrdAndType;
                        cellsRPA["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                        cellsRPA["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                        cellsRPA["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                        cellsRPA["E" + cellIndex.ToString()].Value = strInvNo;
                        cellsRPA["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                        cellIndex++;
                    }
                    cellIndex++;

                    if (dtRPSummary != null && dtRPSummary.Rows.Count > 0)
                    {
                        List<DataTable> dtSummaryPrgPrd1 = dtRPSummary.AsEnumerable()
                                         .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Where(item => (from items in dtRPAdj.AsEnumerable() select items.Field<int>("PREM ADJ PGM ID")).Contains(item.Field<int>("PREM ADJ PGM ID")))
                                         .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();

                        Dictionary<int, string> prgPrdList1 = GetPrgPrdsWithType(dtSummaryPrgPrd1, "PREM ADJ PGM ID");

                        //Sorting fix
                        List<DataTable> dtSummaryPrgPrd = new List<DataTable>();
                        foreach (var item in prgPrdList1)
                        {
                            DataTable dtPrgPrd = (from dts in dtRPSummary.AsEnumerable()
                                                  where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            dtSummaryPrgPrd.Add(dtPrgPrd);
                        }

                        for (int i = 0; i < dtSummaryPrgPrd.Count; i++)
                        {
                            DataTable result = dtSummaryPrgPrd[i];
                            var programPeriod = (from item in dtRPAdj.AsEnumerable()
                                                 where (item.Field<int>("PREM ADJ PGM ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]))
                                                 select
                                                 new { ProgramPeriod = item.Field<string>("PROGRAM PERIOD"), ConvIncrAt = item.Field<string>("CONV DATE") }
                                                 ).First();

                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();

                            cellsRPA["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsRPA["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsRPA, "B", ":G", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                            cellIndex++;
                            cellsRPA["A" + cellIndex.ToString()].Value = "Converts to Incurred At:";
                            FormatHeaderTypeCell(cellsRPA["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsRPA, "B", ":G", Convert.ToString(programPeriod.ConvIncrAt), SSG.HAlign.Left, cellIndex);
                            cellIndex = cellIndex + 2;

                            List<DataTable> result1 = result.AsEnumerable()
                                // .Where(row => (!string.IsNullOrEmpty(row.Field<string>("MIN MAX CODE")) && row.Field<string>("MIN MAX CODE") != "0")
                                // && (row.Field<string>("NUMERICVALUE") != "35" && row.Field<string>("NUMERICVALUE") != "42" && (row.Field<string>("NUMERICVALUE") != "45")))
                                         .GroupBy(row => row.Field<string>("GRP NAME"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();
                            //decimal erpValue = Convert.ToDecimal(result.AsEnumerable()
                            //                              .Where(row => row.Field<string>("NUMERICVALUE") == "12")
                            //                              .Select(erp => erp.Field<decimal?>("FINAL VALUE")));
                            //bool minERPFlag=false;
                            //bool maxERPFlag=false;
                            for (int j = 0; j < result1.Count; j++)
                            {
                                DataTable dtDetails = result1[j];
                                string RetroFinalFormula = string.Empty;
                                if (Convert.ToString(dtDetails.Rows[0]["GRP NAME"]) == "Earned Retrospective Premium Calculation")
                                {
                                    cellsRPA["A" + cellIndex.ToString()].Value = "Earned Retrospective Premium Calculation";
                                    FormatHeaderTypeCell(cellsRPA["A" + cellIndex.ToString()]);

                                    for(int r=0;r<details1.Count;r++)
                                    {
                                        if (details1[r].Rows[0]["PREM ADJ PGM ID"].ToString() == dtDetails.Rows[0]["PREM ADJ PGM ID"].ToString())
                                        {
                                             RetroFinalFormula = details1[r].Rows[0]["Final_Formula_Description"].ToString();
                                        }
                                    }
                                    
                                    MergeAndFormatCellsWithColor(cellsRPA, "B", ":G", Convert.ToString(RetroFinalFormula), SSG.HAlign.Left, cellIndex);
                                    
                                }
                                else
                                {
                                    MergeAndFormatCellsWithColor(cellsRPA, "A", ":G", Convert.ToString(dtDetails.Rows[0]["GRP NAME"]), SSG.HAlign.Left, cellIndex);
                                }
                                cellIndex++;

                                foreach (DataRow drRows in dtDetails.Rows)
                                {
                                    #region comment code
                                    //Description
                                    // (ISNULL({GetRetroSummary;1.MIN MAX CODE}) OR {GetRetroSummary;1.MIN MAX CODE} = '0') AND {GetRetroSummary;1.NUMERICVALUE} = '35'
                                    //OR
                                    //(ISNULL({GetRetroSummary;1.MIN MAX CODE}) OR {GetRetroSummary;1.MIN MAX CODE} = '0') AND {GetRetroSummary;1.NUMERICVALUE} = '42'
                                    //OR
                                    //(ISNULL({GetRetroSummary;1.MIN MAX CODE}) OR {GetRetroSummary;1.MIN MAX CODE} = '0') AND {GetRetroSummary;1.NUMERICVALUE} = '45'
                                    //NA :
                                    //IF({GetRetroSummary;1.NA} = 1 AND ({GetRetroSummary;1.NUMERICVALUE} = '19' OR {GetRetroSummary;1.NUMERICVALUE} = '18' OR {GetRetroSummary;1.NUMERICVALUE} = '20' OR {GetRetroSummary;1.NUMERICVALUE} = '10'))
                                    //    THEN
                                    //    FALSE
                                    //    ELSE
                                    //    TRUE

                                    //Unlimited
                                    //IF(({GetRetroSummary;1.UNLIM FLAG} = 1 AND {GetRetroSummary;1.NUMERICVALUE} = '19') OR ({GetRetroSummary;1.UNLIM FLAG} = 1 AND {GetRetroSummary;1.NUMERICVALUE} = '18'))
                                    //THEN
                                    //FALSE
                                    //ELSE
                                    //TRUE
                                    //final value
                                    //({GetRetroSummary;1.NUMERICVALUE} = '20' AND {GetRetroSummary;1.MIN MAX CODE} <> 'MIN') OR
                                    //({GetRetroSummary;1.NUMERICVALUE} = '21' AND {GetRetroSummary;1.MIN MAX CODE} <> 'MAX') OR
                                    //({GetRetroSummary;1.NUMERICVALUE} = '22' AND {GetRetroSummary;1.MIN MAX CODE} <> 'ERP')

                                    //IF({GetRetroSummary;1.MIN MAX CODE} = 'MIN' AND {GetRetroSummary;1.NUMERICVALUE} = '20')
                                    //THEN
                                    //TRUE
                                    //ELSE IF({GetRetroSummary;1.MIN MAX CODE} = 'MAX' AND {GetRetroSummary;1.NUMERICVALUE} = '21')
                                    //THEN 
                                    //TRUE
                                    //ELSE IF({GetRetroSummary;1.MIN MAX CODE} = 'ERP' AND {GetRetroSummary;1.NUMERICVALUE} = '22')
                                    //THEN 
                                    //TRUE
                                    //ELSE IF({GetRetroSummary;1.NUMERICVALUE} <> '20' AND {GetRetroSummary;1.NUMERICVALUE} <> '21' AND {GetRetroSummary;1.NUMERICVALUE} <> '22')

                                    //if ((Convert.ToString(drRows["MIN MAX CODE"]) == "MIN" && Convert.ToString(drRows["NUMERICVALUE"]) == "20") ||
                                    //            (Convert.ToString(drRows["MIN MAX CODE"]) == "MAX" && Convert.ToString(drRows["NUMERICVALUE"]) == "21") ||
                                    //            (Convert.ToString(drRows["MIN MAX CODE"]) == "ERP" && Convert.ToString(drRows["NUMERICVALUE"]) == "22")
                                    //    //|| (Convert.ToString(drRows["NUMERICVALUE"]) != "20" && Convert.ToString(drRows["NUMERICVALUE"]) != "21" && Convert.ToString(drRows["NUMERICVALUE"]) != "22")
                                    //            )
                                    //{
                                    //    cellsRPA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "FINAL VALUE");
                                    //    SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                    //}
                                    //else if (Convert.ToString(drRows["NA"]) == "1" && Convert.ToString(drRows["UNLIM FLAG"]) != "1")
                                    //{
                                    //    cellsRPA["G" + cellIndex.ToString()].Value = "N/A";
                                    //}
                                    //else if (Convert.ToString(drRows["NA"]) != "1" && Convert.ToString(drRows["UNLIM FLAG"]) == "1")
                                    //{
                                    //    cellsRPA["G" + cellIndex.ToString()].Value = "UNLIMITED";
                                    //}
                                    //if ((Convert.ToString(drRows["NUMERICVALUE"]) == "20" || Convert.ToString(drRows["NUMERICVALUE"]) == "21") 
                                    //    && (minERPFlag != true && maxERPFlag != true))
                                    //{
                                    //    cellsRPA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "FINAL VALUE");
                                    //    SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                    //}
                                    //else if ((Convert.ToString(drRows["NUMERICVALUE"]) == "20" || Convert.ToString(drRows["NUMERICVALUE"]) == "21") 
                                    //    && (minERPFlag == true && maxERPFlag == true))
                                    //{
                                    //    cellsRPA["G" + cellIndex.ToString()].Value = "N/A";
                                    //}                                           
                                    //else
                                    //{
                                    //    cellsRPA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "FINAL VALUE");
                                    //    SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                    //}       

                                    #endregion

                                    //bool isDisplayA = false;
                                    //bool isDisplayB = false;

                                    //if ((string.IsNullOrEmpty(Convert.ToString(drRows["MIN MAX CODE"])) || Convert.ToString(drRows["MIN MAX CODE"]) == "0")
                                    //    && Convert.ToString(drRows["NUMERICVALUE"]) == "45")
                                    //{
                                    //    isDisplayB = true;
                                    //}
                                    //else
                                    //{
                                    //    isDisplayA = true;
                                    //}

                                    if (Convert.ToString(drRows["NUMERICVALUE"]) != "42" && Convert.ToString(drRows["NUMERICVALUE"]) != "45" )
                                    {
                                        

                                        if((Convert.ToString(drRows["NA"]) == "1" && Convert.ToString(drRows["NUMERICVALUE"]) == "19") || (Convert.ToString(drRows["NA"]) == "1" && Convert.ToString(drRows["NUMERICVALUE"]) == "20") ||(Convert.ToString(drRows["NA"]) == "1" && Convert.ToString(drRows["NUMERICVALUE"]) == "10"))
                                        {
                                            MergeAndFormatCellsWithoutColor(cellsRPA, "A", ":F", Convert.ToString(drRows["DESCRIPTION"]), SSG.HAlign.Left, cellIndex, false);
                                            cellsRPA["G" + cellIndex.ToString()].Value = "NA";
                                            SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                        }

                                        else if ((Convert.ToString(drRows["UNLIM FLAG"]) == "1" && Convert.ToString(drRows["NUMERICVALUE"]) == "19") || (Convert.ToString(drRows["UNLIM FLAG"]) == "1" && Convert.ToString(drRows["NUMERICVALUE"]) == "20"))
                                        {
                                            MergeAndFormatCellsWithoutColor(cellsRPA, "A", ":F", Convert.ToString(drRows["DESCRIPTION"]), SSG.HAlign.Left, cellIndex, false);
                                            cellsRPA["G" + cellIndex.ToString()].Value = "UNLIMITED";
                                            SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                        }
                                        else
                                        {
                                            MergeAndFormatCellsWithoutColor(cellsRPA, "A", ":F", Convert.ToString(drRows["DESCRIPTION"]), SSG.HAlign.Left, cellIndex, false);
                                            cellsRPA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "FINAL VALUE");
                                            SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                        }
                                        cellIndex++;
                                    }

                                    else if (Convert.ToString(drRows["NUMERICVALUE"]) == "42" || Convert.ToString(drRows["NUMERICVALUE"]) == "45")
                                    {
                                        cellIndex++;
                                        MergeAndFormatCellsWithColor(cellsRPA, "A", ":F", Convert.ToString(dtDetails.Rows[0]["GRP NAME"]), SSG.HAlign.Left, cellIndex);
                                        cellsRPA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "FINAL VALUE");
                                        SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                        FormatHeaderTypeCellRetro(cellsRPA["G" + cellIndex.ToString()]);
                                        cellIndex = cellIndex + 2;
                                    }
                                    //if (isDisplayA)
                                    //{
                                    //    MergeAndFormatCellsWithoutColor(cellsRPA, "A", ":F", Convert.ToString(drRows["DESCRIPTION"]), SSG.HAlign.Left, cellIndex, false);

                                    //    if (Convert.ToString(drRows["NA"]) == "1"
                                    //        && (Convert.ToString(drRows["NUMERICVALUE"]) == "10" || Convert.ToString(drRows["NUMERICVALUE"]) == "18" || Convert.ToString(drRows["NUMERICVALUE"]) == "19")
                                    //        )
                                    //    {
                                    //        cellsRPA["G" + cellIndex.ToString()].Value = "N/A";
                                    //        //if (Convert.ToString(drRows["NA"]) == "1" && (Convert.ToString(drRows["NUMERICVALUE"]) == "18" || Convert.ToString(drRows["NUMERICVALUE"]) == "19" ))
                                    //        //{
                                    //        //    minERPFlag=true;
                                    //        //}

                                    //    }
                                    //    else if (Convert.ToString(drRows["UNLIM FLAG"]) == "1" &&
                                    //        (Convert.ToString(drRows["NUMERICVALUE"]) == "18" || Convert.ToString(drRows["NUMERICVALUE"]) == "19")
                                    //        )
                                    //    {
                                    //        cellsRPA["G" + cellIndex.ToString()].Value = "UNLIMITED";
                                    //        //maxERPFlag=true;
                                    //    }
                                    //    else
                                    //    {
                                    //        if ((Convert.ToString(drRows["NUMERICVALUE"]) == "20" && Convert.ToString(drRows["MIN MAX CODE"]).ToUpper() != "MIN")
                                    //            || (Convert.ToString(drRows["NUMERICVALUE"]) == "21" && Convert.ToString(drRows["MIN MAX CODE"]).ToUpper() != "MAX")
                                    //            || (Convert.ToString(drRows["NUMERICVALUE"]) == "22" && Convert.ToString(drRows["MIN MAX CODE"]).ToUpper() != "ERP")
                                    //            )
                                    //        {
                                    //            //cellsRPA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "FINAL VALUE");
                                    //            //SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                    //            cellsRPA["G" + cellIndex.ToString()].Value = "N/A";
                                    //        }
                                    //        else
                                    //        {
                                    //            cellsRPA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "FINAL VALUE");
                                    //            SetCurrencyFormat(cellsRPA["G" + cellIndex.ToString()]);
                                    //        }
                                    //    }
                                    //    cellIndex++;
                                    //}
                                }
                            }
                            cellIndex++;
                        }
                    }

                    worksheetRPA.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetRPA.UsedRange.ColumnCount; col++)
                    {
                        worksheetRPA.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:RPA Audit

                DataTable dtRPAAudit = (new ProgramPeriodsBS()).GetRPAAuditReport(AdjNo, 1, false);

                DataTable dtRPAAuditSum = (new ProgramPeriodsBS()).GetRPAAuditReportSum(AdjNo, 1, false);

                if (extIntFlag == 1)
                {

                    if (dtRPAAudit != null && dtRPAAudit.Rows.Count > 0)
                    {
                        workbook.Worksheets.Add();
                        sheetIndex++;
                        SSG.IWorksheet worksheetRPAAudit = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                        worksheetRPAAudit.Name = "RPA - Audit";

                        SSG.IRange cellsRPAAudit = worksheetRPAAudit.Cells;
                        DataRow dr = dtRPAAudit.Rows[0];
                        cellIndex = 1;

                        // Add column headers. 
                        cellsRPAAudit["A" + cellIndex.ToString()].Value = "Insured Name:";
                        FormatHeaderTypeCell(cellsRPAAudit["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsRPAAudit, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                        cellsRPAAudit["D" + cellIndex.ToString()].Value = "Broker:";
                        FormatHeaderTypeCell(cellsRPAAudit["D" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsRPAAudit, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                        cellIndex = cellIndex + 2;

                        FormatHeaderTypeCell(cellsRPAAudit["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                        cellsRPAAudit["A" + cellIndex.ToString()].Value = "Program Period";
                        cellsRPAAudit["B" + cellIndex.ToString()].Value = "Adjustment Number";
                        cellsRPAAudit["C" + cellIndex.ToString()].Value = "Loss Type";
                        cellsRPAAudit["D" + cellIndex.ToString()].Value = "Loss Val Date";
                        cellsRPAAudit["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                        cellsRPAAudit["F" + cellIndex.ToString()].Value = "Adjustment Date";
                        cellIndex++;

                        List<DataTable> programPeriods1 = dtRPAAudit.AsEnumerable()
                                             .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .Select(g => g.CopyToDataTable())
                                             .ToList();

                        Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                        //Sorting fix
                        List<DataTable> programPeriods = new List<DataTable>();
                        foreach (var item in prgPrdList)
                        {
                            DataTable dtPrgPrd = (from dts in dtRPAAudit.AsEnumerable()
                                                  where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            programPeriods.Add(dtPrgPrd);
                        }

                        for (int i = 0; i < programPeriods.Count; i++)
                        {
                            DataTable detail = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsRPAAudit["A" + cellIndex.ToString()].Value = prgPrdAndType;
                            cellsRPAAudit["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                            cellsRPAAudit["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                            cellsRPAAudit["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                            cellsRPAAudit["E" + cellIndex.ToString()].Value = strInvNo;
                            cellsRPAAudit["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                            cellIndex++;
                        }
                        cellIndex = cellIndex + 2;
                        MergeAndFormatCellsWithColor(cellsRPAAudit, "A", ":H", "Audit Information", SSG.HAlign.Center, cellIndex);
                        cellIndex = cellIndex + 2;

                        MergeAndFormatCellsWithColor(cellsRPAAudit, "A", ":B", "", SSG.HAlign.Center, cellIndex);
                        MergeAndFormatCellsWithColor(cellsRPAAudit, "C", ":H", "Premium Audit Information", SSG.HAlign.Center, cellIndex);
                        MergeAndFormatCellsWithColor(cellsRPAAudit, "I", ":L", "Policy Deposit Premium", SSG.HAlign.Center, cellIndex);
                        cellIndex++;

                        FormatHeaderTypeCell(cellsRPAAudit["A" + cellIndex.ToString() + ":L" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithColor(cellsRPAAudit, "A", ":B", "LOB", SSG.HAlign.Left, cellIndex);
                        cellsRPAAudit["C" + cellIndex.ToString()].Value = "Audited Exposure for Inclusion in Adjustment";
                        cellsRPAAudit["D" + cellIndex.ToString()].Value = "Audited Standard(Subject) Premium";
                        cellsRPAAudit["E" + cellIndex.ToString()].Value = "Audited Non-Subject PremiumDescription";
                        cellsRPAAudit["F" + cellIndex.ToString()].Value = "AuditedNon-SubjectPremium";
                        cellsRPAAudit["G" + cellIndex.ToString()].Value = "AuditedEarnedPremiumTotal";
                        cellsRPAAudit["H" + cellIndex.ToString()].Value = "AuditResult";
                        cellsRPAAudit["I" + cellIndex.ToString()].Value = "Standard(Subject)PremiumDeposit";
                        cellsRPAAudit["J" + cellIndex.ToString()].Value = "Standard(Subject)PremiumDeferred";
                        cellsRPAAudit["K" + cellIndex.ToString()].Value = "NonSubjectPremiumDeposit";
                        cellsRPAAudit["L" + cellIndex.ToString()].Value = "TotalEstimatedPolicyPremium";

                        cellIndex = cellIndex + 2;


                        for (int i = 0; i < programPeriods.Count; i++)
                        {
                            DataTable result = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsRPAAudit["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsRPAAudit["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsRPAAudit, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);

                            cellsRPAAudit["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsRPAAudit["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsRPAAudit, "B", ":G", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                            cellIndex++;


                            DataTable GetRPAAuditSumbyPGMID = (from dts in dtRPAAuditSum.AsEnumerable()
                                                               where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]))
                                                               select dts).CopyToDataTable();


                            for (int j = 0; j < GetRPAAuditSumbyPGMID.Rows.Count; j++)
                            {
                                DataRow drRows = GetRPAAuditSumbyPGMID.Rows[j];
                                MergeAndFormatCellsWithoutColor(cellsRPAAudit, "A", ":B", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "LOB")), SSG.HAlign.Left, cellIndex);
                                cellsRPAAudit["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_AUD_EXPO_INCL_ADJ"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["C" + cellIndex.ToString()]);
                                cellsRPAAudit["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_AUD_STD_SUB_PREM"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["D" + cellIndex.ToString()]);
                                cellsRPAAudit["E" + cellIndex.ToString()].Value = ""; //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);

                                cellsRPAAudit["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_AUD_NON_SUBJ_PREM"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["F" + cellIndex.ToString()]);
                                cellsRPAAudit["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_AUD_EAR_PREM_TOT"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["G" + cellIndex.ToString()]);
                                cellsRPAAudit["H" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_AUDIT_RESULT"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["H" + cellIndex.ToString()]);
                                cellsRPAAudit["I" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_STD_SUBJ_PREM_DEP"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["I" + cellIndex.ToString()]);
                                cellsRPAAudit["J" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_STD_SUBJ_PREM_DEFERRED"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["J" + cellIndex.ToString()]);
                                cellsRPAAudit["K" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_NON_SUB_PREM_DEPOS"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["K" + cellIndex.ToString()]);
                                cellsRPAAudit["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Sum_TOTAL_ESTIMATED_POL_PREM"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                SetCurrencyFormat(cellsRPAAudit["L" + cellIndex.ToString()]);
                                cellIndex++;
                            }


                            MergeAndFormatCellsWithColor(cellsRPAAudit, "A", ":B", "Total of Audit and DepositPremium for All LOB", SSG.HAlign.Left, cellIndex);

                            cellsRPAAudit["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T AUD EXPO INCL ADJ");
                            SetCurrencyFormat(cellsRPAAudit["C" + cellIndex.ToString()]);
                            cellsRPAAudit["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T AUD STD SUB PREM");
                            SetCurrencyFormat(cellsRPAAudit["D" + cellIndex.ToString()]);
                            cellsRPAAudit["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T AUD NON SUBJ PREM");
                            SetCurrencyFormat(cellsRPAAudit["F" + cellIndex.ToString()]);
                            cellsRPAAudit["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T AUD EAR PREM TOT");
                            SetCurrencyFormat(cellsRPAAudit["G" + cellIndex.ToString()]);
                            cellsRPAAudit["H" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T AUDIT RESULT");
                            SetCurrencyFormat(cellsRPAAudit["H" + cellIndex.ToString()]);
                            cellsRPAAudit["I" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T STD SUBJ PREM DEP");
                            SetCurrencyFormat(cellsRPAAudit["I" + cellIndex.ToString()]);
                            cellsRPAAudit["J" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T STD SUBJ PREM DEFERRED");
                            SetCurrencyFormat(cellsRPAAudit["J" + cellIndex.ToString()]);
                            cellsRPAAudit["K" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T NON SUB PREM DEPOS");
                            SetCurrencyFormat(cellsRPAAudit["K" + cellIndex.ToString()]);
                            cellsRPAAudit["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "T TOTAL ESTIMATED POL PREM");
                            SetCurrencyFormat(cellsRPAAudit["L" + cellIndex.ToString()]);

                            cellIndex = cellIndex + 2;
                        }

                        worksheetRPAAudit.UsedRange.Columns.AutoFit();
                        for (int col = 0; col < worksheetRPAAudit.UsedRange.ColumnCount; col++)
                        {
                            worksheetRPAAudit.Cells[1, col].ColumnWidth *= 1.15;
                        }
                    }
                }

                #endregion

                #region Sheet:RPA Legend Summarization

                DataTable dtRPLegSummary = (new ProgramPeriodsBS()).GetRPALegendSummaryReport(AdjNo, 1, false);

                if (dtRPAdj != null && dtRPAdj.Rows.Count > 0 && dtRPLegSummary != null && dtRPLegSummary.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetRPALegSummary = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetRPALegSummary.Name = "RPA Legend - Summarization";

                    SSG.IRange cellsRPALegSummary = worksheetRPALegSummary.Cells;
                    DataRow dr = dtRPAdj.Rows[0];
                    cellIndex = 1;

                    // Add column headers. 
                    cellsRPALegSummary["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsRPALegSummary["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsRPALegSummary["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsRPALegSummary["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 2;

                    FormatHeaderTypeCell(cellsRPALegSummary["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                    cellsRPALegSummary["A" + cellIndex.ToString()].Value = "Program Period";
                    cellsRPALegSummary["B" + cellIndex.ToString()].Value = "Adjustment Number";
                    cellsRPALegSummary["C" + cellIndex.ToString()].Value = "Loss Type";
                    cellsRPALegSummary["D" + cellIndex.ToString()].Value = "Loss Val Date";
                    cellsRPALegSummary["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                    cellsRPALegSummary["F" + cellIndex.ToString()].Value = "Adjustment Date";
                    cellIndex++;

                    List<DataTable> details1 = dtRPAdj.AsEnumerable()
                                         .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();

                    Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(details1, "PREM ADJ PGM ID");

                    //Sorting fix
                    List<DataTable> details = new List<DataTable>();
                    foreach (var item in prgPrdList)
                    {
                        DataTable dtPrgPrd = (from dts in dtRPAdj.AsEnumerable()
                                              where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                              select dts).CopyToDataTable();
                        details.Add(dtPrgPrd);
                    }

                    for (int i = 0; i < details.Count; i++)
                    {
                        DataTable detail = details[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();

                        cellsRPALegSummary["A" + cellIndex.ToString()].Value = prgPrdAndType;
                        cellsRPALegSummary["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                        cellsRPALegSummary["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                        cellsRPALegSummary["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                        cellsRPALegSummary["E" + cellIndex.ToString()].Value = strInvNo;
                        cellsRPALegSummary["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                        cellIndex++;
                    }
                    cellIndex++;

                    if (dtRPLegSummary != null && dtRPLegSummary.Rows.Count > 0)
                    {
                        List<DataTable> dtSummaryPrgPrd1 = dtRPLegSummary.AsEnumerable()
                                         .OrderByDescending(row => row.Field<int>("PREM_ADJ_PGM_ID"))
                                         .Where(item => (from items in dtRPAdj.AsEnumerable() select items.Field<int>("PREM ADJ PGM ID")).Contains(item.Field<int>("PREM_ADJ_PGM_ID")))
                                         .GroupBy(row => row.Field<int>("PREM_ADJ_PGM_ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();

                        Dictionary<int, string> prgPrdList1 = GetPrgPrdsWithType(dtSummaryPrgPrd1, "PREM_ADJ_PGM_ID");

                        //Sorting fix
                        List<DataTable> dtSummaryPrgPrd = new List<DataTable>();
                        foreach (var item in prgPrdList1)
                        {
                            DataTable dtPrgPrd = (from dts in dtRPLegSummary.AsEnumerable()
                                                  where (dts.Field<int>("PREM_ADJ_PGM_ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            dtSummaryPrgPrd.Add(dtPrgPrd);
                        }
                        for (int i = 0; i < dtSummaryPrgPrd.Count; i++)
                        {
                            

                            DataTable result = dtSummaryPrgPrd[i];

                            string PaidorIncurredType = string.Empty;
                            for (int r = 0; r < details1.Count; r++)
                            {
                                if (details1[r].Rows[0]["PREM ADJ PGM ID"].ToString() == result.Rows[0]["PREM_ADJ_PGM_ID"].ToString())
                                {
                                    PaidorIncurredType = details1[r].Rows[0]["ADJUSTMENT TYPE"].ToString();
                                }
                            }
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM_ADJ_PGM_ID"])).Select(p => p.Value).First();

                            cellsRPALegSummary["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsRPALegSummary["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "B", ":G", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                            cellIndex = cellIndex + 2;

                            FormatHeaderTypeCell(cellsRPALegSummary["A" + cellIndex.ToString() + ":H" + cellIndex.ToString()], SSG.HAlign.Center);
                            cellsRPALegSummary["A" + cellIndex.ToString()].Value = "LOB";         
                            cellsRPALegSummary["B" + cellIndex.ToString()].Value = "Category";
                            if (PaidorIncurredType == "Paid")
                            {
                                MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", "Net Paid", SSG.HAlign.Center, cellIndex);
                            }
                            else
                            {
                                MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", "Net Incurred", SSG.HAlign.Center, cellIndex);
                            }
                            MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", "Excess Amounts", SSG.HAlign.Center, cellIndex);
                            if (PaidorIncurredType == "Paid")
                            {
                                MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", "Limited Paid", SSG.HAlign.Center, cellIndex);
                            }
                            else
                            {
                                MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", "Limited Incurred", SSG.HAlign.Center, cellIndex);
                            }
                            cellIndex++;

                            //FormatHeaderTypeCell(cellsRPALegSummary["C" + cellIndex.ToString() + ":L" + cellIndex.ToString()]);
                            //cellsRPALegSummary["C" + cellIndex.ToString()].Value = "Paid Loss";
                            //cellsRPALegSummary["D" + cellIndex.ToString()].Value = "Paid ALAE";
                            //cellsRPALegSummary["E" + cellIndex.ToString()].Value = "Reserve Loss";
                            //cellsRPALegSummary["F" + cellIndex.ToString()].Value = "Reserve ALAE";
                            //cellsRPALegSummary["G" + cellIndex.ToString()].Value = "Paid";
                            //cellsRPALegSummary["H" + cellIndex.ToString()].Value = "Reserve";
                            //cellsRPALegSummary["I" + cellIndex.ToString()].Value = "Paid Loss";
                            //cellsRPALegSummary["J" + cellIndex.ToString()].Value = "Paid ALAE";
                            //cellsRPALegSummary["K" + cellIndex.ToString()].Value = "Reserve Loss";
                            //cellsRPALegSummary["L" + cellIndex.ToString()].Value = "Reserve ALAE";
                            //cellIndex++;

                            foreach (DataRow drRows in result.Rows)
                            {

                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "WC";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "Losses";
                                int WC_Limited_PaidorIncurred_Losses = 0;
                                int WC_SUM_PaidLoss_ResLoss = 0;
                                if (PaidorIncurredType == "Paid")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                                    string WCCURRPAIDLOSS = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSS"));
                                    int WCCURRPAIDLOSS_int = Convert.ToInt32(Convert.ToDouble(WCCURRPAIDLOSS));

                                    string WCCURRPAIDLOSSEXCESS = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSSEXCESS"));
                                    int WCCURRPAIDLOSSEXCESS_INT = Convert.ToInt32(Convert.ToDouble(WCCURRPAIDLOSSEXCESS));
                                    WC_Limited_PaidorIncurred_Losses = WCCURRPAIDLOSS_int - WCCURRPAIDLOSSEXCESS_INT;
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    WC_SUM_PaidLoss_ResLoss = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRRESERLOSS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(WC_SUM_PaidLoss_ResLoss), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    WC_Limited_PaidorIncurred_Losses = WC_SUM_PaidLoss_ResLoss - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;


                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "WC";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "ALAE";
                                int WC_Limited_PaidorIncurred_ALAE = 0;
                                int WC_SUM_PaidALAE_ResALAE = 0;
                                if (PaidorIncurredType == "Paid")
                                {

                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAE")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    WC_Limited_PaidorIncurred_ALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAE"))) - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    WC_SUM_PaidALAE_ResALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRRESERALAE")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(WC_SUM_PaidALAE_ResALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    WC_Limited_PaidorIncurred_ALAE = WC_SUM_PaidALAE_ResALAE - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;



                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "WC Adj.Ded";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "Losses";
                                int WC_ADJ_Limited_PaidorIncurred_Losses = 0;
                                int WC_ADJ_SUM_PaidLoss_ResLoss = 0;
                                if (PaidorIncurredType == "Paid")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    WC_ADJ_Limited_PaidorIncurred_Losses = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSS"))) - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_ADJ_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    WC_ADJ_SUM_PaidLoss_ResLoss = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRRESERLOSS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(WC_ADJ_SUM_PaidLoss_ResLoss), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    WC_ADJ_Limited_PaidorIncurred_Losses = WC_ADJ_SUM_PaidLoss_ResLoss - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_ADJ_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;


                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "WC Adj.Ded";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "ALAE";
                                int WC_ADJ_Limited_PaidorIncurred_ALAE = 0;
                                int WC_ADJ_SUM_PaidALAE_ResALAE = 0;
                                if (PaidorIncurredType == "Paid")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAE")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    WC_ADJ_Limited_PaidorIncurred_ALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAE"))) - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_ADJ_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    WC_ADJ_SUM_PaidALAE_ResALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRRESERALAE")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(WC_ADJ_SUM_PaidALAE_ResALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    WC_ADJ_Limited_PaidorIncurred_ALAE = WC_ADJ_SUM_PaidALAE_ResALAE - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(WC_ADJ_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;


                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "GL";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "Losses";
                                int GL_Limited_PaidorIncurred_Losses = 0;
                                int GL_SUM_PaidLoss_ResLoss = 0;
                                if (PaidorIncurredType == "Paid")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    GL_Limited_PaidorIncurred_Losses = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSS"))) - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(GL_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    GL_SUM_PaidLoss_ResLoss = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRRESERLOSS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(GL_SUM_PaidLoss_ResLoss), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    GL_Limited_PaidorIncurred_Losses = GL_SUM_PaidLoss_ResLoss - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(GL_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;



                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "GL";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "ALAE";
                                int GL_Limited_PaidorIncurred_ALAE = 0;
                                int GL_SUM_PaidAlae_ResAlae = 0;
                                if (PaidorIncurredType == "Paid")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAE")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    GL_Limited_PaidorIncurred_ALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAE"))) - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(GL_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    GL_SUM_PaidAlae_ResAlae = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRRESERALAE")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(GL_SUM_PaidAlae_ResAlae), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    GL_Limited_PaidorIncurred_ALAE = GL_SUM_PaidAlae_ResAlae - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(GL_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;


                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "AUTO";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "Losses";
                                int AUTO_Limited_PaidorIncurred_Losses = 0;
                                int AUTO_SUM_PaidLoss_ResLoss = 0;
                                if (PaidorIncurredType == "Paid")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    AUTO_Limited_PaidorIncurred_Losses = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSS"))) - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(AUTO_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    AUTO_SUM_PaidLoss_ResLoss = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRRESERLOSS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(AUTO_SUM_PaidLoss_ResLoss), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSSEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    AUTO_Limited_PaidorIncurred_Losses = AUTO_SUM_PaidLoss_ResLoss - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(AUTO_Limited_PaidorIncurred_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;


                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "AUTO";
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "ALAE";
                                int AUTO_Limited_PaidorIncurred_ALAE = 0;
                                int AUTO_SUM_PaidALAE_ResALAE = 0;
                                if (PaidorIncurredType == "Paid")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAE")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    AUTO_Limited_PaidorIncurred_ALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAE"))) - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(AUTO_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    AUTO_SUM_PaidALAE_ResALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRRESERALAE")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "C", ":D", Convert.ToString(AUTO_SUM_PaidALAE_ResALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "E", ":F", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAEEXCESS")), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    AUTO_Limited_PaidorIncurred_ALAE = AUTO_SUM_PaidALAE_ResALAE - Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithoutColor(cellsRPALegSummary, "G", ":H", Convert.ToString(AUTO_Limited_PaidorIncurred_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;
                                

                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "Total";
                                FormatHeaderTypeCell(cellsRPALegSummary["A" + cellIndex.ToString()]);
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "Losses";
                                FormatHeaderTypeCell(cellsRPALegSummary["B" + cellIndex.ToString()]);
                                if (PaidorIncurredType == "Paid")
                                {
                                    int Total_Net_Losses = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSS")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "C", ":D", Convert.ToString(Total_Net_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    int Total_Excess_Losses = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSSEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSSEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSSEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "E", ":F", Convert.ToString(Total_Excess_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    int Total_Limited_Losses = WC_Limited_PaidorIncurred_Losses + WC_ADJ_Limited_PaidorIncurred_Losses + GL_Limited_PaidorIncurred_Losses + AUTO_Limited_PaidorIncurred_Losses;
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "G", ":H", Convert.ToString(Total_Limited_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {

                                    int Total_Net_Losses = WC_SUM_PaidLoss_ResLoss + WC_ADJ_SUM_PaidLoss_ResLoss + GL_SUM_PaidLoss_ResLoss + AUTO_SUM_PaidLoss_ResLoss; //Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSS")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "C", ":D", Convert.ToString(Total_Net_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    int Total_Excess_Losses = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDLOSSEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDLOSSEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDLOSSEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDLOSSEXCESS")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "E", ":F", Convert.ToString(Total_Excess_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    int Total_Limited_Losses = WC_Limited_PaidorIncurred_Losses + WC_ADJ_Limited_PaidorIncurred_Losses + GL_Limited_PaidorIncurred_Losses + AUTO_Limited_PaidorIncurred_Losses;
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "G", ":H", Convert.ToString(Total_Limited_Losses), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;

                                cellsRPALegSummary["A" + cellIndex.ToString()].Value = "Total";
                                FormatHeaderTypeCell(cellsRPALegSummary["A" + cellIndex.ToString()]);
                                cellsRPALegSummary["B" + cellIndex.ToString()].Value = "ALAE";
                                FormatHeaderTypeCell(cellsRPALegSummary["B" + cellIndex.ToString()]);
                                if (PaidorIncurredType == "Paid")
                                {
                                    int Total_Net_ALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAE")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "C", ":D", Convert.ToString(Total_Net_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    int Total_Excess_ALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAEEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAEEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAEEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "E", ":F", Convert.ToString(Total_Excess_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    int Total_Limited_ALAE = WC_Limited_PaidorIncurred_ALAE + WC_ADJ_Limited_PaidorIncurred_ALAE + GL_Limited_PaidorIncurred_ALAE + AUTO_Limited_PaidorIncurred_ALAE;
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "G", ":H", Convert.ToString(Total_Limited_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    int Total_Net_ALAE = WC_SUM_PaidALAE_ResALAE + WC_ADJ_SUM_PaidALAE_ResALAE + GL_SUM_PaidAlae_ResAlae + AUTO_SUM_PaidALAE_ResALAE;//Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAE"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAE")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "C", ":D", Convert.ToString(Total_Net_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["C" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);
                                    int Total_Excess_ALAE = Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCCURRPAIDALAEEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "WCADJCURRPAIDALAEEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "GLCURRPAIDALAEEXCESS"))) + Convert.ToInt32(Convert.ToDouble(CheckNullOrEmptyReturnValue(drRows, "ALCURRPAIDALAEEXCESS")));
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "E", ":F", Convert.ToString(Total_Excess_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["E" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);
                                    int Total_Limited_ALAE = WC_Limited_PaidorIncurred_ALAE + WC_ADJ_Limited_PaidorIncurred_ALAE + GL_Limited_PaidorIncurred_ALAE + AUTO_Limited_PaidorIncurred_ALAE;
                                    MergeAndFormatCellsWithColor(cellsRPALegSummary, "G", ":H", Convert.ToString(Total_Limited_ALAE), SSG.HAlign.Left, cellIndex);
                                    SetCurrencyFormat(cellsRPALegSummary["G" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                                }
                                cellIndex++;
                                
                                cellIndex++;
                            }
                            cellIndex++;
                        }
                    }

                    worksheetRPALegSummary.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetRPALegSummary.UsedRange.ColumnCount; col++)
                    {
                        worksheetRPALegSummary.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:RPA Calc Info

                DataTable dtRPALegCalc = (new ProgramPeriodsBS()).GetRPALegendCalcInfoReport(AdjNo, 1, false);

                DataTable dtRPALegendAuditedExposure = (new ProgramPeriodsBS()).GetRPALegendAuditedExposure(AdjNo);

                if (dtRPAdj != null && dtRPAdj.Rows.Count > 0 && dtRPALegCalc != null && dtRPALegCalc.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetRPALegCalc = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetRPALegCalc.Name = "RPA Legend -  CalcInfo";

                    SSG.IRange cellsRPALegCalc = worksheetRPALegCalc.Cells;
                    DataRow dr = dtRPAdj.Rows[0];
                    cellIndex = 1;

                    // Add column headers. 
                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsRPALegCalc["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsRPALegCalc["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsRPALegCalc["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 2;

                    FormatHeaderTypeCell(cellsRPALegCalc["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = "Program Period";
                    cellsRPALegCalc["B" + cellIndex.ToString()].Value = "Adjustment Number";
                    cellsRPALegCalc["C" + cellIndex.ToString()].Value = "Loss Type";
                    cellsRPALegCalc["D" + cellIndex.ToString()].Value = "Loss Val Date";
                    cellsRPALegCalc["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                    cellsRPALegCalc["F" + cellIndex.ToString()].Value = "Adjustment Date";
                    cellIndex++;

                    List<DataTable> details1 = dtRPAdj.AsEnumerable()
                                         .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();

                    Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(details1, "PREM ADJ PGM ID");

                    //Sorting fix
                    List<DataTable> details = new List<DataTable>();
                    foreach (var item in prgPrdList)
                    {
                        DataTable dtPrgPrd = (from dts in dtRPAdj.AsEnumerable()
                                              where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                              select dts).CopyToDataTable();
                        details.Add(dtPrgPrd);
                    }

                    for (int i = 0; i < details.Count; i++)
                    {
                        DataTable detail = details[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsRPALegCalc["A" + cellIndex.ToString()].Value = prgPrdAndType;
                        cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                        cellsRPALegCalc["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                        cellsRPALegCalc["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                        cellsRPALegCalc["E" + cellIndex.ToString()].Value = strInvNo;
                        cellsRPALegCalc["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                        cellIndex++;
                    }
                    cellIndex++;

                    if (dtRPALegCalc != null && dtRPALegCalc.Rows.Count > 0)
                    {
                        
                        

                        for(int i=0;i<dtRPALegCalc.Rows.Count; i++)
                        {
                            Boolean PGMIDEXIST = false;
                            for (int a = 0; a < dtRPALegendAuditedExposure.Rows.Count; a++)
                            {
                                if (Convert.ToInt32(dtRPALegendAuditedExposure.Rows[a]["PREM_ADJ_PGM_ID"]) == Convert.ToInt32(dtRPALegCalc.Rows[i]["PREM_ADJ_PGM_ID"]))
                                {
                                    PGMIDEXIST = true;
                                }
                            }

                            if (PGMIDEXIST == false)
                            {
                                dtRPALegCalc.Rows[i].Delete();
                            }
                        }

                        dtRPALegCalc.AcceptChanges();

                        List<DataTable> dtSummaryPrgPrd1 = dtRPALegCalc.AsEnumerable()
                                         .OrderByDescending(row => row.Field<int>("PREM_ADJ_PGM_ID"))
                                         .Where(item => (from items in dtRPAdj.AsEnumerable() select items.Field<int>("PREM ADJ PGM ID")).Contains(item.Field<int>("PREM_ADJ_PGM_ID")))
                                         .GroupBy(row => row.Field<int>("PREM_ADJ_PGM_ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();
                        Dictionary<int, string> prgPrdList1 = GetPrgPrdsWithType(dtSummaryPrgPrd1, "PREM_ADJ_PGM_ID");

                        //Sorting fix
                        List<DataTable> dtSummaryPrgPrd = new List<DataTable>();
                        foreach (var item in prgPrdList1)
                        {
                            DataTable dtPrgPrd = (from dts in dtRPALegCalc.AsEnumerable()
                                                  where (dts.Field<int>("PREM_ADJ_PGM_ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            dtSummaryPrgPrd.Add(dtPrgPrd);
                        }

                        for (int i = 0; i < dtSummaryPrgPrd.Count; i++)
                        {
                            DataTable result = dtSummaryPrgPrd[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM_ADJ_PGM_ID"])).Select(p => p.Value).First();
                            var PaidorIncurredType = (from items in dtRPAdj.AsEnumerable()
                                               where items.Field<Int32>("PREM ADJ PGM ID") == Convert.ToInt32(result.Rows[0]["PREM_ADJ_PGM_ID"])
                                                      select new {PaidorIncurredType = items.Field<string>("ADJUSTMENT TYPE")}).First();

                            var companyCode = (from items in dtRPAdj.AsEnumerable()
                                               where items.Field<Int32>("PREM ADJ PGM ID") == Convert.ToInt32(result.Rows[0]["PREM_ADJ_PGM_ID"])
                                               select new { CompanyCode = items.Field<Int32>("CompanyCode") }).First();

                            var RetroFormula = (from items in dtRPAdj.AsEnumerable()
                                               where items.Field<Int32>("PREM ADJ PGM ID") == Convert.ToInt32(result.Rows[0]["PREM_ADJ_PGM_ID"])
                                                select new {RetroFormula=items.Field<string>("Final_Formula")}).First();

                            DataTable GetRPAAuditExposure = (from dts in dtRPALegendAuditedExposure.AsEnumerable()
                                                             where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM_ADJ_PGM_ID"]))
                                                               select dts).CopyToDataTable();


                            

                            cellsRPALegCalc["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsRPALegCalc["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "B", ":G", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                            cellIndex++;
                            cellsRPALegCalc["A" + cellIndex.ToString()].Value = "LOB";
                            FormatHeaderTypeCell(cellsRPALegCalc["A" + cellIndex.ToString()]);
                            cellsRPALegCalc["B" + cellIndex.ToString()].Value = "Program Type";
                            FormatHeaderTypeCell(cellsRPALegCalc["B" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithColor(cellsRPALegCalc, "C", ":E", "Audited Exposure for inclusion in Adjustment", SSG.HAlign.Center, cellIndex);
                            cellsRPALegCalc["F" + cellIndex.ToString()].Value = "Retro Limit";
                            FormatHeaderTypeCell(cellsRPALegCalc["F" + cellIndex.ToString()]);
                            cellsRPALegCalc["G" + cellIndex.ToString()].Value = "ALAE Treatment";
                            FormatHeaderTypeCell(cellsRPALegCalc["G" + cellIndex.ToString()]);
                            cellIndex++;

                            for (int j = 0; j < GetRPAAuditExposure.Rows.Count; j++)
                            {
                                DataRow drRows = GetRPAAuditExposure.Rows[j];
                                string WC_INCUorPAID_LOSS_RETRO_LOB =Convert.ToString(GetRPAAuditExposure.Rows[j]["WC_INCUorPAID_LOSS_RETRO_LOB"]);
                                string WC_INCUorPAID_UNDERLAYER_LOB = Convert.ToString(GetRPAAuditExposure.Rows[j]["WC_INCUorPAID_UNDERLAYER_LOB"]);
                                string GL_INCUorPAID_LOSS_RETRO_LOB = Convert.ToString(GetRPAAuditExposure.Rows[j]["GL_INCUorPAID_LOSS_RETRO_LOB"]);
                                string GL_INCUorPAID_UNDERLAYER_LOB = Convert.ToString(GetRPAAuditExposure.Rows[j]["GL_INCUorPAID_UNDERLAYER_LOB"]);
                                string AUTO_INCUorPAID_LOSS_RETRO_LOB = Convert.ToString(GetRPAAuditExposure.Rows[j]["AUTO_INCUorPAID_LOSS_RETRO_LOB"]);
                                string AUTO_INCUorPAID_UNDERLAYER_LOB = Convert.ToString(GetRPAAuditExposure.Rows[j]["AUTO_INCUorPAID_UNDERLAYER_LOB"]);
                                if (WC_INCUorPAID_LOSS_RETRO_LOB != null && WC_INCUorPAID_LOSS_RETRO_LOB != string.Empty)
                                {
                                    
                                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_LOSS_RETRO_LOB"));
                                    cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_LOSS_RETRO_ADJUSTMENT_TYPE"));
                                    MergeAndFormatCellsWithoutColorandBold(cellsRPALegCalc, "C", ":E", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_LOSS_RETRO_ADUITED_EXPOSURE")), SSG.HAlign.Center, cellIndex);
                                    SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);
                                    cellsRPALegCalc["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_LOSS_RETRO_RETRO_LIMIT"); 
                                    SetCurrencyFormat(cellsRPALegCalc["F" + cellIndex.ToString()]);
                                    cellsRPALegCalc["G" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_LOSS_RETRO_ALAE_TREATMENT"));
                                    cellIndex++;
                                }
                                if (WC_INCUorPAID_UNDERLAYER_LOB != null && WC_INCUorPAID_UNDERLAYER_LOB != string.Empty)
                                {
                                    
                                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_UNDERLAYER_LOB"));
                                    cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_UNDERLAYER_ADJUSTMENT_TYPE"));
                                    //cellsRPALegCalc["C" + cellIndex.ToString() + ":E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_UNDERLAYER_ADUITED_EXPOSURE");
                                    MergeAndFormatCellsWithoutColorandBold(cellsRPALegCalc, "C", ":E", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_UNDERLAYER_ADUITED_EXPOSURE")), SSG.HAlign.Center, cellIndex);
                                    SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);
                                    cellsRPALegCalc["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_UNDERLAYER_RETRO_LIMIT");
                                    SetCurrencyFormat(cellsRPALegCalc["F" + cellIndex.ToString()]);
                                    cellsRPALegCalc["G" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "WC_INCUorPAID_UNDERLAYER_ALAE_TREATMENT"));
                                    cellIndex++;
                                }
                                if (GL_INCUorPAID_LOSS_RETRO_LOB != null && GL_INCUorPAID_LOSS_RETRO_LOB != string.Empty)
                                {
                                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_LOSS_RETRO_LOB"));
                                    cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_LOSS_RETRO_ADJUSTMENT_TYPE"));
                                    MergeAndFormatCellsWithoutColorandBold(cellsRPALegCalc, "C", ":E", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_LOSS_RETRO_ADUITED_EXPOSURE")), SSG.HAlign.Center, cellIndex);
                                    SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);
                                    cellsRPALegCalc["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_LOSS_RETRO_RETRO_LIMIT");
                                    SetCurrencyFormat(cellsRPALegCalc["F" + cellIndex.ToString()]);
                                    cellsRPALegCalc["G" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_LOSS_RETRO_ALAE_TREATMENT"));
                                    cellIndex++;
                                }
                                if (GL_INCUorPAID_UNDERLAYER_LOB != null && GL_INCUorPAID_UNDERLAYER_LOB != string.Empty)
                                {
                                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_UNDERLAYER_LOB"));
                                    cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_UNDERLAYER_ADJUSTMENT_TYPE"));
                                    MergeAndFormatCellsWithoutColorandBold(cellsRPALegCalc, "C", ":E", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_UNDERLAYER_ADUITED_EXPOSURE")), SSG.HAlign.Center, cellIndex);
                                    SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);
                                    cellsRPALegCalc["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_UNDERLAYER_RETRO_LIMIT");
                                    SetCurrencyFormat(cellsRPALegCalc["F" + cellIndex.ToString()]);
                                    cellsRPALegCalc["G" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "GL_INCUorPAID_UNDERLAYER_ALAE_TREATMENT"));
                                    cellIndex++;
                                }
                                if (AUTO_INCUorPAID_LOSS_RETRO_LOB != null && AUTO_INCUorPAID_LOSS_RETRO_LOB != string.Empty)
                                {
                                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_LOSS_RETRO_LOB"));
                                    cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_LOSS_RETRO_ADJUSTMENT_TYPE"));
                                    MergeAndFormatCellsWithoutColorandBold(cellsRPALegCalc, "C", ":E", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_LOSS_RETRO_ADUITED_EXPOSURE")), SSG.HAlign.Center, cellIndex);
                                    SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);
                                    cellsRPALegCalc["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_LOSS_RETRO_RETRO_LIMIT");
                                    SetCurrencyFormat(cellsRPALegCalc["F" + cellIndex.ToString()]);
                                    cellsRPALegCalc["G" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_LOSS_RETRO_ALAE_TREATMENT"));
                                    cellIndex++;
                                }
                                if (AUTO_INCUorPAID_UNDERLAYER_LOB != null && AUTO_INCUorPAID_UNDERLAYER_LOB != string.Empty)
                                {
                                    cellsRPALegCalc["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_UNDERLAYER_LOB"));
                                    cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_UNDERLAYER_ADJUSTMENT_TYPE"));
                                    MergeAndFormatCellsWithoutColorandBold(cellsRPALegCalc, "C", ":E", Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_UNDERLAYER_ADUITED_EXPOSURE")), SSG.HAlign.Center, cellIndex);
                                    SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);
                                    cellsRPALegCalc["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_UNDERLAYER_RETRO_LIMIT");
                                    SetCurrencyFormat(cellsRPALegCalc["F" + cellIndex.ToString()]);
                                    cellsRPALegCalc["G" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "AUTO_INCUorPAID_UNDERLAYER_ALAE_TREATMENT"));
                                    cellIndex++;
                                }
                            }
                            //cellIndex++;
                            MergeAndFormatCellsWithColor(cellsRPALegCalc, "A", ":E", "Incurred Limited Losses and ALAE", SSG.HAlign.Center, cellIndex);
                            cellIndex++;
                            cellsRPALegCalc["A" + cellIndex.ToString()].Value = "Loss/ALAE";
                            FormatHeaderTypeCell(cellsRPALegCalc["A" + cellIndex.ToString()]);
                            cellsRPALegCalc["B" + cellIndex.ToString()].Value = "Paid";
                            FormatHeaderTypeCell(cellsRPALegCalc["B" + cellIndex.ToString()]);
                            cellsRPALegCalc["C" + cellIndex.ToString()].Value = "Reserve";
                            FormatHeaderTypeCell(cellsRPALegCalc["C" + cellIndex.ToString()]);
                            cellsRPALegCalc["D" + cellIndex.ToString()].Value = "Incurred";
                            FormatHeaderTypeCell(cellsRPALegCalc["D" + cellIndex.ToString()]);
                            cellsRPALegCalc["E" + cellIndex.ToString()].Value = "Subject to Adjustment";
                            FormatHeaderTypeCell(cellsRPALegCalc["E" + cellIndex.ToString()]);
                            cellIndex++;
                            decimal currPLoss = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Paid Loss"));
                            decimal currRLoss = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Reserve Loss"));
                            decimal totalLoss = currPLoss + currRLoss;
                            decimal currPALAE = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Paid ALAE"));
                            decimal currRALAE = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Reserve ALAE"));
                            decimal totalALAE = currPALAE + currRALAE;
                            decimal TotPLossandPALAE = currPLoss + currPALAE;
                            decimal TotRLossandRALAE = currRLoss + currRALAE;
                            decimal TotalIncurred = TotPLossandPALAE + TotRLossandRALAE;
                            cellsRPALegCalc["A" + cellIndex.ToString()].Value = "Losses";
                            cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(currPLoss); ;
                            SetCurrencyFormat(cellsRPALegCalc["B" + cellIndex.ToString()]);
                            cellsRPALegCalc["C" + cellIndex.ToString()].Value = Convert.ToString(currRLoss); ;
                            SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString()]);
                            cellsRPALegCalc["D" + cellIndex.ToString()].Value = Convert.ToString(totalLoss);
                            SetCurrencyFormat(cellsRPALegCalc["D" + cellIndex.ToString()]);
                            if (Convert.ToString(PaidorIncurredType).Split('=')[1].Split('}')[0].Trim() == "Paid")
                            {
                                cellsRPALegCalc["E" + cellIndex.ToString()].Value = Convert.ToString(currPLoss);
                                SetCurrencyFormat(cellsRPALegCalc["E" + cellIndex.ToString()]);
                            }
                            else
                            {
                                cellsRPALegCalc["E" + cellIndex.ToString()].Value = Convert.ToString(totalLoss);
                                SetCurrencyFormat(cellsRPALegCalc["E" + cellIndex.ToString()]);
                            }
                            cellIndex++;
                            cellsRPALegCalc["A" + cellIndex.ToString()].Value = "ALAE";
                            cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(currPALAE); ;
                            SetCurrencyFormat(cellsRPALegCalc["B" + cellIndex.ToString()]);
                            cellsRPALegCalc["C" + cellIndex.ToString()].Value = Convert.ToString(currRALAE); ;
                            SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString()]);
                            cellsRPALegCalc["D" + cellIndex.ToString()].Value = Convert.ToString(totalALAE);
                            SetCurrencyFormat(cellsRPALegCalc["D" + cellIndex.ToString()]);
                            if (Convert.ToString(PaidorIncurredType).Split('=')[1].Split('}')[0].Trim() == "Paid")
                            {
                                cellsRPALegCalc["E" + cellIndex.ToString()].Value = Convert.ToString(currPALAE);
                                SetCurrencyFormat(cellsRPALegCalc["E" + cellIndex.ToString()]);
                            }
                            else
                            {
                                cellsRPALegCalc["E" + cellIndex.ToString()].Value = Convert.ToString(totalALAE);
                                SetCurrencyFormat(cellsRPALegCalc["E" + cellIndex.ToString()]);
                            }
                            cellIndex++;
                            cellsRPALegCalc["A" + cellIndex.ToString()].Value = "Total";
                            if (Convert.ToString(PaidorIncurredType).Split('=')[1].Split('}')[0].Trim() == "Paid")
                            {
                                cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(TotPLossandPALAE);
                                SetCurrencyFormat(cellsRPALegCalc["B" + cellIndex.ToString()]);
                                cellsRPALegCalc["C" + cellIndex.ToString()].Value = Convert.ToString(TotRLossandRALAE); ;
                                SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString()]);
                                cellsRPALegCalc["D" + cellIndex.ToString()].Value = Convert.ToString(TotalIncurred);
                                SetCurrencyFormat(cellsRPALegCalc["D" + cellIndex.ToString()]);
                                cellsRPALegCalc["E" + cellIndex.ToString()].Value = Convert.ToString(TotPLossandPALAE);
                                SetCurrencyFormat(cellsRPALegCalc["E" + cellIndex.ToString()]);
                            }
                            else
                            {
                                cellsRPALegCalc["B" + cellIndex.ToString()].Value = Convert.ToString(TotPLossandPALAE); ;
                                SetCurrencyFormat(cellsRPALegCalc["B" + cellIndex.ToString()]);
                                cellsRPALegCalc["C" + cellIndex.ToString()].Value = Convert.ToString(TotRLossandRALAE); ;
                                SetCurrencyFormat(cellsRPALegCalc["C" + cellIndex.ToString()]);
                                cellsRPALegCalc["D" + cellIndex.ToString()].Value = Convert.ToString(TotalIncurred);
                                SetCurrencyFormat(cellsRPALegCalc["D" + cellIndex.ToString()]);
                                cellsRPALegCalc["E" + cellIndex.ToString()].Value = Convert.ToString(TotalIncurred);
                                SetCurrencyFormat(cellsRPALegCalc["E" + cellIndex.ToString()]);
                            }
                            //cellIndex = cellIndex + 2;
                            cellIndex++;

                            int subIndex = cellIndex;
                            int lastCellIndex = 0;

                            //MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "A", ":F", "Incurred Limited Losses and ALAE", SSG.HAlign.Center, subIndex);
                            //subIndex++;
                            //cellsRPALegCalc["A" + subIndex.ToString()].Value = "Current Paid Loss";
                            //cellsRPALegCalc["B" + subIndex.ToString()].Value = "Current Reserve Loss";
                            //cellsRPALegCalc["C" + subIndex.ToString()].Value = "Total Losses";
                            //cellsRPALegCalc["D" + subIndex.ToString()].Value = "Current Paid ALAE";
                            //cellsRPALegCalc["E" + subIndex.ToString()].Value = "Current Reserve ALAE";
                            //cellsRPALegCalc["F" + subIndex.ToString()].Value = "Total ALAE";
                            //subIndex++;
                            ////decimal currPLoss = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Paid Loss"));
                            ////decimal currRLoss = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Reserve Loss"));
                            ////decimal totalLoss = currPLoss + currRLoss;
                            ////decimal currPALAE = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Paid ALAE"));
                            ////decimal currRALAE = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Current Reserve ALAE"));
                            ////decimal totalALAE = currPALAE + currRALAE;
                            //cellsRPALegCalc["A" + subIndex.ToString()].Value = Convert.ToString(currPLoss); ;
                            //SetCurrencyFormat(cellsRPALegCalc["A" + subIndex.ToString()]);
                            //cellsRPALegCalc["B" + subIndex.ToString()].Value = Convert.ToString(currRLoss); ;
                            //SetCurrencyFormat(cellsRPALegCalc["B" + subIndex.ToString()]);
                            //cellsRPALegCalc["C" + subIndex.ToString()].Value = Convert.ToString(totalLoss);
                            //SetCurrencyFormat(cellsRPALegCalc["C" + subIndex.ToString()]);
                            //cellsRPALegCalc["D" + subIndex.ToString()].Value = Convert.ToString(currPALAE);
                            //SetCurrencyFormat(cellsRPALegCalc["D" + subIndex.ToString()]);
                            //cellsRPALegCalc["E" + subIndex.ToString()].Value = Convert.ToString(currRALAE);
                            //SetCurrencyFormat(cellsRPALegCalc["E" + subIndex.ToString()]);
                            //cellsRPALegCalc["F" + subIndex.ToString()].Value = Convert.ToString(totalALAE);
                            //SetCurrencyFormat(cellsRPALegCalc["F" + subIndex.ToString()]);
                            //subIndex = cellIndex;

                            MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "A", ":D", "Unallocated Loss Adjustment Expense (ULAE)", SSG.HAlign.Center, subIndex);
                            subIndex++;
                            cellsRPALegCalc["A" + subIndex.ToString()].Value = "LOB/State";
                            cellsRPALegCalc["B" + subIndex.ToString()].Value = "Limited Loss and ALAE";
                            cellsRPALegCalc["C" + subIndex.ToString()].Value = "LCF Factor";
                            cellsRPALegCalc["D" + subIndex.ToString()].Value = "LCF Amount";
                            subIndex++;
                            foreach (DataRow drRows in result.Rows)
                            {
                                cellsRPALegCalc["A" + subIndex.ToString()].Value = Convert.ToString(drRows["LOB"]) + "/" + Convert.ToString(drRows["STATE"]);
                                cellsRPALegCalc["B" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Lim Loss and ALAE");
                                SetCurrencyFormat(cellsRPALegCalc["B" + subIndex.ToString()]);
                                cellsRPALegCalc["C" + subIndex.ToString()].Value = (CheckNullOrEmptyReturnValue(drRows, "LCF Rate") == "0") ? "1" : Convert.ToString(drRows["LCF Rate"]);
                                cellsRPALegCalc["D" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LCF Amount");
                                SetCurrencyFormat(cellsRPALegCalc["D" + subIndex.ToString()]);
                                subIndex++;
                            }
                            cellsRPALegCalc["A" + subIndex.ToString()].Value = "Total ULAE";
                            cellsRPALegCalc["B" + subIndex.ToString()].Value = Convert.ToString(GetSumForDecimalField(result, "Lim Loss and ALAE"));
                            SetCurrencyFormat(cellsRPALegCalc["B" + subIndex.ToString()]);
                            cellsRPALegCalc["C" + subIndex.ToString()].Value = Convert.ToString(GetSumForDecimalField(result, "LCF Amount"));
                            SetCurrencyFormat(cellsRPALegCalc["D" + subIndex.ToString()]);
                            lastCellIndex = subIndex;
                            subIndex = cellIndex;

                            cellsRPALegCalc["E" + subIndex.ToString()].Value = "Loss Development Reserve";
                            subIndex++;
                            cellsRPALegCalc["E" + subIndex.ToString()].Value = "Factor";
                            subIndex++;
                            //{GetCalcInfoSecond;1.LDR Count} = 0
                            if (Convert.ToString(result.Rows[0]["LDR Count"]) != "0")
                            {
                                cellsRPALegCalc["E" + subIndex.ToString()].Value = Convert.ToString(result.Rows[0]["LDF IBNR Factors"]);
                            }
                            else
                            {
                                cellsRPALegCalc["E" + subIndex.ToString()].Value = "N/A";
                            }
                            subIndex = cellIndex;

                            MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "F", ":H", "Excess Loss Premium Calculation", SSG.HAlign.Center, subIndex);
                            subIndex++;
                            cellsRPALegCalc["F" + subIndex.ToString()].Value = "Excess Loss Premium Per Agreement";
                            cellsRPALegCalc["G" + subIndex.ToString()].Value = "Rate Info";
                            cellsRPALegCalc["H" + subIndex.ToString()].Value = "Total";
                            subIndex++;
                            //{GetCalcInfoSecond;1.Not applicable Excess} = true

                            if (result.Rows[0]["Not applicable Excess"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["Not applicable Excess"]) == true)
                            {
                                MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "F", ":H", "N/A", SSG.HAlign.Center, subIndex);
                            }
                            else
                            {
                                cellsRPALegCalc["F" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Tot Agr Excess");
                                SetCurrencyFormat(cellsRPALegCalc["F" + subIndex.ToString()]);
                                cellsRPALegCalc["G" + subIndex.ToString()].Value = Convert.ToString(result.Rows[0]["Left lbl Excess"]) + Convert.ToString(result.Rows[0]["Right lbl Excess"]);
                                cellsRPALegCalc["H" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Result Excess");
                                SetCurrencyFormat(cellsRPALegCalc["H" + subIndex.ToString()]);
                            }
                            subIndex = cellIndex;

                            MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "I", ":K", "Basic Premium", SSG.HAlign.Center, subIndex);
                            subIndex++;
                            cellsRPALegCalc["I" + subIndex.ToString()].Value = "Basic Min Amount";
                            cellsRPALegCalc["J" + subIndex.ToString()].Value = "Rate Info";
                            cellsRPALegCalc["K" + subIndex.ToString()].Value = "Total";
                            subIndex++;
                            //GetCalcInfoSecond;1.Tot Agr Basic
                            //GetCalcInfoSecond;1.Right lbl Basic
                            //GetCalcInfoSecond;1.Result Basic
                            //    Not applicable Basic
                            //Convert.ToString(result.Rows[0]["Left lbl Basic"]);
                            if (result.Rows[0]["Not applicable Basic"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["Not applicable Basic"]) == true)
                            {
                                cellsRPALegCalc["I" + subIndex.ToString()].Value = "N/A";
                                cellsRPALegCalc["J" + subIndex.ToString()].Value = "N/A";
                                cellsRPALegCalc["K" + subIndex.ToString()].Value = "N/A";
                            }
                            else
                            {
                                cellsRPALegCalc["I" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Tot Agr Basic");
                                SetCurrencyFormat(cellsRPALegCalc["I" + subIndex.ToString()]);
                                cellsRPALegCalc["J" + subIndex.ToString()].Value = Convert.ToString(result.Rows[0]["Left lbl Basic"]) + Convert.ToString(result.Rows[0]["Right lbl Basic"]);
                                cellsRPALegCalc["K" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Result Basic");
                                SetCurrencyFormat(cellsRPALegCalc["K" + subIndex.ToString()]);
                            }
                            subIndex = cellIndex;

                            MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "L", ":M", "Tax Multiplier", SSG.HAlign.Center, subIndex);
                            //N/A - NOT({GetCalcInfoSecond;1.Factor rate} = 1.0000000000 AND {GetRetroPremAdj;1.CompanyCode} = 734)
                            // -/- ({GetCalcInfoSecond;1.Factor rate} = 1.0 and {GetRetroPremAdj;1.CompanyCode} = 734) 
                            //Variable TM- ({GetCalcInfoSecond;1.Pgm Typ Id}<>451) or ({GetCalcInfoSecond;1.Factor rate} = 1.0 and {GetRetroPremAdj;1.CompanyCode} = 734)
                            //GetCalcInfoSecond;1.Tax Multiplier Amount- ({GetCalcInfoSecond;1.Factor rate} = 1.0000000000 and {GetRetroPremAdj;1.CompanyCode} = 734)
                            //Factor Rate : ({GetCalcInfoSecond;1.Pgm Typ Id}=451) or ({GetCalcInfoSecond;1.Factor rate} = 1.0 and {GetRetroPremAdj;1.CompanyCode} = 734)
                            subIndex++;
                            cellsRPALegCalc["L" + subIndex.ToString()].Value = "TX Rate";
                            cellsRPALegCalc["M" + subIndex.ToString()].Value = "Amount";
                            subIndex++;
                            if (Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Factor rate")) == 1 && Convert.ToString(companyCode.CompanyCode) == "734")
                            {
                                cellsRPALegCalc["L" + subIndex.ToString()].Value = "N/A";
                                cellsRPALegCalc["M" + subIndex.ToString()].Value = "N/A";

                            }
                            else
                            {
                                cellsRPALegCalc["L" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Factor rate");
                                if (Convert.ToString(result.Rows[0]["Pgm Typ Id"]) == "451")
                                {
                                    cellsRPALegCalc["L" + subIndex.ToString()].Value = "N/A";
                                }
                                cellsRPALegCalc["M" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Tax Multiplier Amount");
                                SetCurrencyFormat(cellsRPALegCalc["M" + subIndex.ToString()]);
                            }
                            //if (Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "Factor rate")) != 1 && Convert.ToString(companyCode.CompanyCode) != "734")                                
                            //{
                            //    cellsRPALegCalc["R" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Factor rate");
                            //    if (Convert.ToString(result.Rows[0]["Pgm Typ Id"]) == "451")
                            //    {
                            //    cellsRPALegCalc["R" + subIndex.ToString()].Value = "N/A";                                    
                            //    }
                            //    cellsRPALegCalc["S" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Tax Multiplier Amount");
                            //    SetCurrencyFormat(cellsRPALegCalc["S" + subIndex.ToString()]);
                            //}
                            //else
                            //{
                            //    if (Convert.ToString(result.Rows[0]["Pgm Typ Id"]) != "451")
                            //    {
                            //        cellsRPALegCalc["R" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Factor rate");
                            //    }
                            //    else
                            //    {
                            //        cellsRPALegCalc["R" + subIndex.ToString()].Value = "N/A";
                            //    }
                            //    cellsRPALegCalc["S" + subIndex.ToString()].Value = "N/A";
                            //}
                            subIndex = cellIndex;


                            cellsRPALegCalc["N" + subIndex.ToString()].Value = "Retro Formula";
                            subIndex = subIndex + 2;
                            cellsRPALegCalc["N" + subIndex.ToString()].Value = Convert.ToString(RetroFormula).Split('=')[1].Split('}')[0].Trim();
                            subIndex = cellIndex;

                            MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "O", ":Q", "Minimum Retrospective Premium", SSG.HAlign.Center, subIndex);
                            subIndex++;
                            //N/A-{GetCalcInfoSecond;1.Not applicable Minimum} = false
                            //Unlimited-{GetCalcInfoSecond;1.no limit ind Minimum} = false
                            //Total Agr Min-{GetCalcInfoSecond;1.Not applicable Minimum} = true
                            //ResultMin-{GetCalcInfoSecond;1.no limit ind Minimum} = true or {GetCalcInfoSecond;1.Not applicable Minimum} = true
                            //Rightlbl-{GetCalcInfoSecond;1.Not applicable Minimum} = true OR {GetCalcInfoSecond;1.no limit ind Minimum} = true
                            cellsRPALegCalc["O" + subIndex.ToString()].Value = "Minimum Per Agreement";
                            cellsRPALegCalc["P" + subIndex.ToString()].Value = "Rate Info";
                            cellsRPALegCalc["Q" + subIndex.ToString()].Value = "Total";
                            subIndex++;

                            if ((result.Rows[0]["Not applicable Minimum"] == DBNull.Value || Convert.ToBoolean(result.Rows[0]["Not applicable Minimum"]) != true)
                                && (result.Rows[0]["no limit ind Minimum"] == DBNull.Value || Convert.ToBoolean(result.Rows[0]["no limit ind Minimum"]) != true))
                            {
                                cellsRPALegCalc["O" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Tot Agr Minimum");
                                SetCurrencyFormat(cellsRPALegCalc["U" + subIndex.ToString()]);
                                cellsRPALegCalc["P" + subIndex.ToString()].Value = Convert.ToString(result.Rows[0]["Left lbl Minimum"]) + Convert.ToString(result.Rows[0]["Right lbl Minimum"]);
                                cellsRPALegCalc["Q" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Result Minimum");
                                SetCurrencyFormat(cellsRPALegCalc["W" + subIndex.ToString()]);
                            }
                            else if (Convert.ToBoolean(result.Rows[0]["Not applicable Minimum"]) == true || Convert.ToBoolean(result.Rows[0]["no limit ind Minimum"]) == true)
                            {
                                cellsRPALegCalc["O" + subIndex.ToString()].Value = "N/A";
                                cellsRPALegCalc["P" + subIndex.ToString()].Value = "N/A";
                                cellsRPALegCalc["Q" + subIndex.ToString()].Value = "N/A";
                                if (Convert.ToBoolean(result.Rows[0]["no limit ind Minimum"]) == true)
                                {
                                    cellsRPALegCalc["Q" + subIndex.ToString()].Value = "UNLIMITED";
                                }
                            }
                            subIndex = cellIndex;

                            if (Convert.ToString(result.Rows[0]["Expo Typ Id"]) != "128")
                            {
                                MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "R", ":T", "Maximum Retrospective Premium", SSG.HAlign.Center, subIndex);
                                subIndex++;
                                //{GetCalcInfoSecond;1.Expo Typ Id} = 128 
                                //N/A-{GetCalcInfoSecond;1.Not applicable Maximum} = false
                                //Unlimited-{GetCalcInfoSecond;1.no limit ind Maximum} = false
                                //Total Agr Max-{GetCalcInfoSecond;1.Not applicable Maximum} = true
                                //ResultMax-{GetCalcInfoSecond;1.no limit ind Maximum} = true OR {GetCalcInfoSecond;1.Not applicable Maximum} = true
                                //Rightlbl-{GetCalcInfoSecond;1.no limit ind Maximum} = true OR {GetCalcInfoSecond;1.Not applicable Maximum} = true
                                cellsRPALegCalc["R" + subIndex.ToString()].Value = "Maximum Per Agreement";
                                cellsRPALegCalc["S" + subIndex.ToString()].Value = "Rate Info";
                                cellsRPALegCalc["T" + subIndex.ToString()].Value = "Total";
                                subIndex++;
                                if (result.Rows[0]["Not applicable Maximum"] == DBNull.Value || Convert.ToBoolean(result.Rows[0]["Not applicable Maximum"]) != true)
                                {
                                    cellsRPALegCalc["R" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Tot Agr Maximum");
                                    SetCurrencyFormat(cellsRPALegCalc["R" + subIndex.ToString()]);
                                    if (result.Rows[0]["no limit ind Maximum"] == DBNull.Value || Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) != true)
                                    {
                                        cellsRPALegCalc["S" + subIndex.ToString()].Value = Convert.ToString(result.Rows[0]["Left lbl Maximum"]) + Convert.ToString(result.Rows[0]["Right lbl Maximum"]);
                                        cellsRPALegCalc["T" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Result Maximum");
                                        SetCurrencyFormat(cellsRPALegCalc["T" + subIndex.ToString()]);
                                    }
                                    else if (Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) == true)
                                    {
                                        cellsRPALegCalc["S" + subIndex.ToString()].Value = "N/A";
                                        cellsRPALegCalc["T" + subIndex.ToString()].Value = "UNLIMITED";
                                    }
                                }
                                else if (Convert.ToBoolean(result.Rows[0]["Not applicable Maximum"]) == true || Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) == true)
                                {
                                    cellsRPALegCalc["R" + subIndex.ToString()].Value = "N/A";
                                    cellsRPALegCalc["S" + subIndex.ToString()].Value = "N/A";
                                    cellsRPALegCalc["T" + subIndex.ToString()].Value = "N/A";
                                    if (Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) == true)
                                    {
                                        cellsRPALegCalc["T" + subIndex.ToString()].Value = "UNLIMITED";
                                    }
                                }
                                subIndex = cellIndex;
                            }
                            else if (Convert.ToString(result.Rows[0]["Expo Typ Id"]) == "128")
                            {
                                MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "R", ":T", "Combined Elements", SSG.HAlign.Center, subIndex);
                                subIndex++;
                                //{GetCalcInfoSecond;1.Expo Typ Id} <> 128 
                                //N/A-{GetCalcInfoSecond;1.Not applicable Maximum} = false
                                //Unlimited-{GetCalcInfoSecond;1.no limit ind Maximum} = false
                                //Total Agr Max-{GetCalcInfoSecond;1.Not applicable Maximum} = true
                                //ResultMax-{GetCalcInfoSecond;1.no limit ind Maximum} = true OR {GetCalcInfoSecond;1.Not applicable Maximum} = true
                                //Rightlbl-{GetCalcInfoSecond;1.no limit ind Maximum} = true OR {GetCalcInfoSecond;1.Not applicable Maximum} = true
                                cellsRPALegCalc["R" + subIndex.ToString()].Value = "Combined Elements Per Agreement";
                                cellsRPALegCalc["S" + subIndex.ToString()].Value = "Rate Info";
                                cellsRPALegCalc["T" + subIndex.ToString()].Value = "Total";
                                subIndex++;
                                if (result.Rows[0]["Not applicable Maximum"] == DBNull.Value || Convert.ToBoolean(result.Rows[0]["Not applicable Maximum"]) != true)
                                {
                                    cellsRPALegCalc["R" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Tot Agr Maximum");
                                    SetCurrencyFormat(cellsRPALegCalc["R" + subIndex.ToString()]);
                                    if (result.Rows[0]["no limit ind Maximum"] == DBNull.Value || Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) != true)
                                    {
                                        cellsRPALegCalc["S" + subIndex.ToString()].Value = Convert.ToString(result.Rows[0]["Left lbl Maximum"]) + Convert.ToString(result.Rows[0]["Right lbl Maximum"]);
                                        cellsRPALegCalc["T" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Result Maximum");
                                        SetCurrencyFormat(cellsRPALegCalc["T" + subIndex.ToString()]);
                                    }
                                    else if (Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) == true)
                                    {
                                        cellsRPALegCalc["S" + subIndex.ToString()].Value = "N/A";
                                        cellsRPALegCalc["T" + subIndex.ToString()].Value = "UNLIMITED";
                                    }
                                }
                                else if (Convert.ToBoolean(result.Rows[0]["Not applicable Maximum"]) == true ||
                                    (result.Rows[0]["no limit ind Maximum"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) == true))
                                {
                                    cellsRPALegCalc["R" + subIndex.ToString()].Value = "N/A";
                                    cellsRPALegCalc["S" + subIndex.ToString()].Value = "N/A";
                                    cellsRPALegCalc["T" + subIndex.ToString()].Value = "N/A";
                                    if (Convert.ToBoolean(result.Rows[0]["no limit ind Maximum"]) == true)
                                    {
                                        cellsRPALegCalc["T" + subIndex.ToString()].Value = "UNLIMITED";
                                    }
                                }
                                subIndex = cellIndex;
                            }

                            //MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "U", ":V", "Cash Flow Benefit", SSG.HAlign.Center, subIndex);
                            //subIndex++;

                            //if (Convert.ToString(result.Rows[0]["Paid Incurr Typ"]) != "297")
                            //{
                            //    //{GetCalcInfoSecond;1.Paid Incurr Typ}= 297
                            //    //GetCalcInfoSecond;1.CFB Formula GetCalcInfoSecond;1.Cash Flow Benefit
                            //    //If Minimum/Maximum applies, cash flow benefit adjusted to:
                            //    //GetCalcInfoSecond;1.Adj Cash Flow Benefit
                            //    cellsRPALegCalc["U" + subIndex.ToString()].Value = "Reserves times Applicable Factors (" + Convert.ToString(result.Rows[0]["CFB Formula"]) + ")";
                            //    cellsRPALegCalc["V" + subIndex.ToString()].Value = "If Minimum/Maximum applies, cash flow benefit adjusted to";
                            //    subIndex++;
                            //    cellsRPALegCalc["U" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Cash Flow Benefit");
                            //    SetCurrencyFormat(cellsRPALegCalc["U" + subIndex.ToString()]);
                            //    cellsRPALegCalc["V" + subIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "Adj Cash Flow Benefit");
                            //    SetCurrencyFormat(cellsRPALegCalc["V" + subIndex.ToString()]);
                            //    subIndex = cellIndex;
                            //}
                            //else
                            //{
                            //    subIndex++;
                            //    MergeAndFormatCellsWithoutColor(cellsRPALegCalc, "U", ":V", "N/A", SSG.HAlign.Center, subIndex);
                            //    subIndex = cellIndex;
                            //}
                            subIndex++;
                            FormatHeaderTypeCell(cellsRPALegCalc["A" + cellIndex.ToString() + ":T" + cellIndex.ToString()], SSG.HAlign.Center);
                            FormatHeaderTypeCell(cellsRPALegCalc["A" + subIndex.ToString() + ":T" + subIndex.ToString()], SSG.HAlign.Left);

                            cellIndex = lastCellIndex;
                            cellIndex = cellIndex + 3;
                        }
                    }

                    worksheetRPALegCalc.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetRPALegCalc.UsedRange.ColumnCount; col++)
                    {
                        worksheetRPALegCalc.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:LBA

                DataTable dtLBA = (new ProgramPeriodsBS()).GetLBAReport(AdjNo, 1, false);

                if (dtLBA != null && dtLBA.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetLBA = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetLBA.Name = "LBA";

                    SSG.IRange cellsLBA = worksheetLBA.Cells;
                    DataRow dr = dtLBA.Rows[0];
                    cellIndex = 1;

                    // Add column headers. 
                    cellsLBA["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsLBA["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsLBA, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsLBA["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsLBA["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsLBA, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);

                    cellIndex = cellIndex + 1;

                    FormatHeaderTypeCell(cellsLBA["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                    cellsLBA["A" + cellIndex.ToString()].Value = "Program Period";
                    cellsLBA["B" + cellIndex.ToString()].Value = "Adjustment Number";
                    cellsLBA["C" + cellIndex.ToString()].Value = "Loss Type";
                    cellsLBA["D" + cellIndex.ToString()].Value = "Loss Val Date";
                    cellsLBA["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                    cellsLBA["F" + cellIndex.ToString()].Value = "Adjustment Date";
                    cellIndex++;

                    List<DataTable> programPeriods1 = dtLBA.AsEnumerable()
                                          .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                          .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                          .Select(g => g.CopyToDataTable())
                                          .ToList();

                    Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                    //Sorting fix
                    List<DataTable> programPeriods = new List<DataTable>();
                    foreach (var item in prgPrdList)
                    {
                        DataTable dtPrgPrd = (from dts in dtLBA.AsEnumerable()
                                              where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                              select dts).CopyToDataTable();
                        programPeriods.Add(dtPrgPrd);
                    }

                    for (int i = 0; i < programPeriods.Count; i++)
                    {
                        DataTable detail = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsLBA["A" + cellIndex.ToString()].Value = prgPrdAndType;
                        cellsLBA["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                        cellsLBA["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                        cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                        cellsLBA["E" + cellIndex.ToString()].Value = strInvNo;
                        cellsLBA["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                        cellIndex++;
                    }

                    cellIndex++;

                    FormatHeaderTypeCell(cellsLBA["A" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);

                    cellsLBA["A" + cellIndex.ToString()].Value = "State";
                    cellsLBA["B" + cellIndex.ToString()].Value = "Incurred Loss";
                    cellsLBA["C" + cellIndex.ToString()].Value = "Rate";
                    cellsLBA["D" + cellIndex.ToString()].Value = "Assessment Amount";
                    cellIndex++;

                    for (int i = 0; i < programPeriods.Count; i++)
                    {
                        DataTable result = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsLBA["A" + cellIndex.ToString()].Value = "Program Period:";
                        FormatHeaderTypeCell(cellsLBA["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsLBA, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                        cellIndex = cellIndex + 1;

                        //Split tables as per number of policies under a programperiod
                        List<DataTable> policyDetails = result.AsEnumerable()
                                           .OrderBy(ord => ord.Field<string>("POLICY NUMBER"))
                                           .GroupBy(row => row.Field<string>("POLICY NUMBER"))
                                           .Select(g => g.CopyToDataTable())
                                           .ToList();

                        for (int j = 0; j < policyDetails.Count; j++)
                        {
                            DataTable result1 = policyDetails[j];
                            decimal policyTotal = GetSumForDecimalField(result1, "ASSESSMENT AMOUNT");
                            if (policyTotal != 0)
                            {
                                string policyNumber = Convert.ToString(result1.Rows[0]["POLICY NUMBER"]);
                                cellsLBA["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                                FormatHeaderTypeCell(cellsLBA["A" + cellIndex.ToString()]);
                                MergeAndFormatCellsWithoutColor(cellsLBA, "B", ":C", policyNumber, SSG.HAlign.Left, cellIndex);
                                cellIndex++;
                                var details1 = from item in result1.AsEnumerable()
                                               orderby item.Field<String>("STATE")
                                               group item by item.Field<String>("STATE") into g
                                               select new
                                               {
                                                   State = g.Key,
                                                   IncurredLoss = g.Sum(item => item.Field<decimal?>("PAID/INCURRED LOSS")),
                                                   Rate = g.Sum(item => item.Field<decimal?>("RATE")),
                                                   AssesmentAmount = g.Sum(item => item.Field<decimal?>("ASSESSMENT AMOUNT")),
                                               };

                                for (int r = 0; r < result1.Rows.Count; r++)
                                {
                                    DataRow drRows = result1.Rows[r];
                                    cellsLBA["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["STATE"]);
                                    cellsLBA["B" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "PAID/INCURRED LOSS"));
                                    SetCurrencyFormat(cellsLBA["B" + cellIndex.ToString()]);
                                    cellsLBA["C" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "RATE"));
                                    cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "ASSESSMENT AMOUNT"));
                                    SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                                    cellIndex++;
                                }


                                //foreach (var detail in details1)
                                //{
                                //    if (Convert.ToDecimal(detail.AssesmentAmount) != 0)
                                //    {
                                //        cellsLBA["A" + cellIndex.ToString()].Value = Convert.ToString(detail.State);
                                //        cellsLBA["B" + cellIndex.ToString()].Value = Convert.ToString(detail.IncurredLoss);
                                //        SetCurrencyFormat(cellsLBA["B" + cellIndex.ToString()]);
                                //        cellsLBA["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rate);
                                //        cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(detail.AssesmentAmount);
                                //        SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                                //        cellIndex++;
                                //    }

                                //}

                                cellsLBA["C" + cellIndex.ToString()].Value = "Total LBA Amount:";
                                cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(policyTotal);
                                SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                                cellIndex++;
                            }
                        }

                        if (Convert.ToString(result.Rows[0]["ADJUSTMENT TYPE"]) != string.Empty && Convert.ToString(result.Rows[0]["ADJUSTMENT TYPE"]) != "")
                        {
                            MergeAndFormatCellsWithoutColor(cellsLBA, "A", ":C", "Loss Based Assessments Total :", SSG.HAlign.Left, cellIndex);
                            cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(result.Rows[0]["LOSS BASED ASSESSMENTS TOTAL"]);
                            SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                            cellIndex++;

                            if (result.Rows[0]["INCLUDED ERP"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["INCLUDED ERP"]) == false)
                            {
                                MergeAndFormatCellsWithoutColor(cellsLBA, "A", ":C", "Previous Loss Based Assesments :", SSG.HAlign.Left, cellIndex);
                                cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(result.Rows[0]["PREVIOUS LOSS BASED ASSESSMENTS"]);
                                SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                                cellIndex++;
                                MergeAndFormatCellsWithoutColor(cellsLBA, "A", ":C", "Current Loss Based Assesments :", SSG.HAlign.Left, cellIndex);
                                cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(result.Rows[0]["CURRENT LOSS BASED ASSESSMENTS"]);
                                SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                                cellIndex++;
                                MergeAndFormatCellsWithoutColor(cellsLBA, "A", ":C", "LBA Deposit :", SSG.HAlign.Left, cellIndex);
                                cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(result.Rows[0]["LBA DEPOSIT"]);
                                SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                                cellIndex++;
                            }

                            MergeAndFormatCellsWithoutColor(cellsLBA, "A", ":C", "Amount Invoiced :", SSG.HAlign.Left, cellIndex);
                            cellsLBA["D" + cellIndex.ToString()].Value = Convert.ToString(result.Rows[0]["AMOUNT INVOICED"]);
                            SetCurrencyFormat(cellsLBA["D" + cellIndex.ToString()]);
                        }
                        cellIndex=cellIndex+2;
                    }

                    cellIndex++;
                    worksheetLBA.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetLBA.UsedRange.ColumnCount; col++)
                    {
                        worksheetLBA.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:Excess Loss

                DataTable dtExLoss = (new ProgramPeriodsBS()).GetExcessLossReport(AdjNo, 1, false);

                //DataTable dtCheckLDF_IBNR_Incl_Limit_Excess = (new ProgramPeriodsBS()).CheckLDF_IBNR_Incl_Limit(AdjNo);

                if (dtExLoss != null && dtExLoss.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetExLoss = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetExLoss.Name = "Excess Loss";

                    SSG.IRange cellsExLoss = worksheetExLoss.Cells;
                    DataRow dr = dtExLoss.Rows[0];
                    cellIndex = 1;

                    // Add column headers. 
                    cellsExLoss["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsExLoss["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsExLoss, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsExLoss["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsExLoss["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsExLoss, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 2;

                    FormatHeaderTypeCell(cellsExLoss["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                    cellsExLoss["A" + cellIndex.ToString()].Value = "Program Period";
                    cellsExLoss["B" + cellIndex.ToString()].Value = "Adjustment Number";
                    cellsExLoss["C" + cellIndex.ToString()].Value = "Loss Type";
                    cellsExLoss["D" + cellIndex.ToString()].Value = "Loss Val Date";
                    cellsExLoss["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                    cellsExLoss["F" + cellIndex.ToString()].Value = "Adjustment Date";
                    cellIndex++;

                    List<DataTable> programPeriods1 = dtExLoss.AsEnumerable()
                                         .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();

                    Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                    //Sorting fix
                    List<DataTable> programPeriods = new List<DataTable>();
                    foreach (var item in prgPrdList)
                    {
                        DataTable dtPrgPrd = (from dts in dtExLoss.AsEnumerable()
                                              where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                              select dts).CopyToDataTable();
                        programPeriods.Add(dtPrgPrd);
                    }

                    for (int i = 0; i < programPeriods.Count; i++)
                    {
                        DataTable detail = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsExLoss["A" + cellIndex.ToString()].Value = prgPrdAndType;
                        cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                        cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                        cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                        cellsExLoss["E" + cellIndex.ToString()].Value = strInvNo;
                        cellsExLoss["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();

                        cellIndex++;
                    }
                    cellIndex++;

                    for (int i = 0; i < programPeriods.Count; i++)
                    {

                        DataTable result = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsExLoss["A" + cellIndex.ToString()].Value = "Program Period:";
                        FormatHeaderTypeCell(cellsExLoss["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsExLoss, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                        cellIndex = cellIndex + 2;

                        List<DataTable> lobDetails = result.AsEnumerable()
                                          .OrderBy(ord => ord.Field<string>("LOB"))
                                          .GroupBy(row => row.Field<string>("LOB"))
                                          .Select(g => g.CopyToDataTable())
                                          .ToList();
                        int LDForIBNRHeaderInd = 0;
                        //if (dtCheckLDF_IBNR_Incl_Limit_Excess != null && dtCheckLDF_IBNR_Incl_Limit_Excess.Rows.Count > 0)
                        //{
                        //    DataTable CheckLDF_IBNR_Incl_Limit = (from dts in dtCheckLDF_IBNR_Incl_Limit_Excess.AsEnumerable()
                        //                                          where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]))
                        //                                          select dts).CopyToDataTable();
                        //    GetLDF_IBNR_Incl_Limit = Convert.ToInt32(CheckNullOrEmptyReturnValue(CheckLDF_IBNR_Incl_Limit.Rows[0], "GetLDF_IBNR_Incl_Limit"));
                        //}
                        //else
                        //{
                        //    GetLDF_IBNR_Incl_Limit = 0;
                        //}

                        for (int j = 0; j < lobDetails.Count; j++)
                        {
                            DataTable result1 = lobDetails[j];

                            cellsExLoss["A" + cellIndex.ToString()].Value = Convert.ToString(result1.Rows[0]["LOB"]);
                            FormatHeaderTypeCell(cellsExLoss["A" + cellIndex.ToString()]);
                            cellsExLoss["B" + cellIndex.ToString()].Value = "ADJUSTMENT TYPE";
                            FormatHeaderTypeCell(cellsExLoss["B" + cellIndex.ToString()]); 
                                                                                        
                            MergeAndFormatCellsWithColor(cellsExLoss, "C", ":D", "PAID", SSG.HAlign.Center, cellIndex);
                            MergeAndFormatCellsWithColor(cellsExLoss, "E", ":F", "RESERVES", SSG.HAlign.Center, cellIndex);
                            cellIndex = cellIndex + 1;
                            LDForIBNRHeaderInd = Convert.ToInt32(result1.Rows[0]["LDForIBNRHeaderInd"]);

                            if (LDForIBNRHeaderInd == 1)
                            {
                                FormatHeaderTypeCell(cellsExLoss["C" + cellIndex.ToString() + ":H" + cellIndex.ToString()]);
                            }
                            else
                            {
                                FormatHeaderTypeCell(cellsExLoss["C" + cellIndex.ToString() + ":G" + cellIndex.ToString()]);
                            }
                            cellsExLoss["C" + cellIndex.ToString()].Value = "LOSS";
                            cellsExLoss["D" + cellIndex.ToString()].Value = "ALAE";
                            cellsExLoss["E" + cellIndex.ToString()].Value = "LOSS";
                            cellsExLoss["F" + cellIndex.ToString()].Value = "ALAE";
                            if (LDForIBNRHeaderInd == 1)
                            {
                                cellsExLoss["G" + cellIndex.ToString()].Value = "LDF/IBNR";
                                cellsExLoss["H" + cellIndex.ToString()].Value = "GRAND TOTAL";
                            }
                            else
                            {
                                cellsExLoss["G" + cellIndex.ToString()].Value = "GRAND TOTAL";
                            }
                            cellIndex = cellIndex + 2;

                            #region State Section

                            foreach (DataRow drRow in result1.Rows)
                            {
                                string state = "State : " + Convert.ToString(drRow["STATE"]);
                                string claim = "Claim # " + Convert.ToString(drRow["CLAIM"]);
                                cellsExLoss["A" + cellIndex.ToString()].Value = state;
                                FormatHeaderTypeCell(cellsExLoss["A" + cellIndex.ToString()]);
                                cellsExLoss["B" + cellIndex.ToString()].Value = claim;
                                FormatHeaderTypeCell(cellsExLoss["B" + cellIndex.ToString()]);
                                //MergeAndFormatCellsWithColor(cellsExLoss, "A", ":A", state, SSG.HAlign.Left, cellIndex);
                                //MergeAndFormatCellsWithoutColor(cellsExLoss, "B", ":B", claim, SSG.HAlign.Left, cellIndex);


                                cellIndex++;
                                decimal piaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "PIA"));
                                decimal peaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "PEA"));
                                decimal totalPaidClaimAmt = piaAmt + peaAmt;
                                decimal riaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "RIA"));
                                decimal reaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "REA"));
                                decimal totalReserClaimAmt = riaAmt + reaAmt;
                                decimal grandClaimAmt = totalPaidClaimAmt + totalReserClaimAmt;

                                cellsExLoss["A" + cellIndex.ToString()].Value = "Total Claim Amount";
                                //String AdjustmentTypebyPolicynumner = Convert.ToString(result1.Rows[0]["AdjustmentTypeByPolicyNumber"]);
                                cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(result1.Rows[0]["AdjustmentTypeByPolicyNumber"]); //Convert.ToString(CheckNullOrEmptyReturnValue(drRow, "AdjustmentTypeByPolicyNumber"));
                                //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                                cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(piaAmt);
                                SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                                cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(peaAmt);
                                SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                                cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(riaAmt);
                                SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                                cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(reaAmt);
                                SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);

                                int GetLDF_IBNR_Incl_Limit = 0;
                                int GetLDF_IBNR_Step_Fact = 0;
                                int LDFInd = 0;
                                int IBNRInd = 0;
                                GetLDF_IBNR_Incl_Limit = Convert.ToInt32(result1.Rows[0]["LDForIBNR_INC_LIM"]);
                                GetLDF_IBNR_Step_Fact = Convert.ToInt32(result1.Rows[0]["LDForIBNR_Step_Fact"]);
                                LDFInd = Convert.ToInt32(result1.Rows[0]["LDFInd"]);
                                IBNRInd = Convert.ToInt32(result1.Rows[0]["IBNRInd"]);

                                //cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                                if (GetLDF_IBNR_Incl_Limit == 1 || GetLDF_IBNR_Step_Fact == 1 || LDFInd == 1 || IBNRInd==1)
                                {
                                    cellsExLoss["G" + cellIndex.ToString()].Value = "";
                                    cellsExLoss["H" + cellIndex.ToString()].Value = Convert.ToString(grandClaimAmt);
                                    SetCurrencyFormat(cellsExLoss["H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(grandClaimAmt);
                                    SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                                }

                                cellIndex++;
                                //decimal npiaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "NPIA"));
                                //decimal npeaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "NPEA"));
                                //decimal totalPaidNonPayKind = npiaAmt + npeaAmt;
                                //decimal nriaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "NRIA"));
                                //decimal nreaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "NREA"));
                                //decimal totalReserNonPayKind = nriaAmt + nreaAmt;
                                //decimal grandPayKind = totalPaidNonPayKind + totalReserNonPayKind;

                                //cellsExLoss["A" + cellIndex.ToString()].Value = "Non Billable Pay-Kind Codes";
                                //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(npiaAmt);
                                //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                                //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(npeaAmt);
                                //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                                //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalPaidNonPayKind);
                                //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                                //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(nriaAmt);
                                //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                                //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(nreaAmt);
                                //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                                //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalReserNonPayKind);
                                //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                                ////cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                                //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandPayKind);
                                //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);

                                //cellIndex++;
                                decimal spiaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "SPIA"));
                                decimal speaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "SPEA"));
                                decimal totalPaidSubjClaims = spiaAmt + speaAmt;
                                decimal sriaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "SRIA"));
                                decimal sreaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "SREA"));
                                decimal totalReservSubjClaims = sriaAmt + sreaAmt;
                                decimal grandSubjClaims = totalPaidSubjClaims + totalReservSubjClaims;

                                cellsExLoss["A" + cellIndex.ToString()].Value = "Subject Claims";
                                cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(result1.Rows[0]["AdjustmentTypeByPolicyNumber"]);
                                //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                                cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(spiaAmt);
                                SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);

                                cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(speaAmt);
                                SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                                cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(sriaAmt);
                                SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                                cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(sreaAmt);
                                SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                                if (GetLDF_IBNR_Incl_Limit == 1 || GetLDF_IBNR_Step_Fact == 1 || LDFInd == 1 || IBNRInd == 1 )
                                {
                                    cellsExLoss["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRow, "LDF");
                                    SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                                    cellsExLoss["H" + cellIndex.ToString()].Value = Convert.ToString(grandSubjClaims);
                                    SetCurrencyFormat(cellsExLoss["H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(grandSubjClaims);
                                    SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                                }

                                cellIndex++;
                                decimal epiaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "EPIA"));
                                decimal epeaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "EPEA"));
                                decimal totalpaidExcessClaims = epiaAmt + epeaAmt;
                                decimal eriaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "ERIA"));
                                decimal ereaAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRow, "EREA"));
                                decimal totalreservExcessClaims = eriaAmt + ereaAmt;
                                decimal grandExcessClaims = totalpaidExcessClaims + totalreservExcessClaims;

                                cellsExLoss["A" + cellIndex.ToString()].Value = "Excess Claims";
                                cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(result1.Rows[0]["AdjustmentTypeByPolicyNumber"]);
                                cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(epiaAmt);
                                SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                                cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(epeaAmt);
                                SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);

                                cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(eriaAmt);
                                SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                                cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(ereaAmt);
                                SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                                if (GetLDF_IBNR_Incl_Limit == 1 || GetLDF_IBNR_Step_Fact == 1 || LDFInd == 1 || IBNRInd == 1)
                                {
                                    cellsExLoss["G" + cellIndex.ToString()].Value = "";
                                    cellsExLoss["H" + cellIndex.ToString()].Value = Convert.ToString(grandExcessClaims);
                                    SetCurrencyFormat(cellsExLoss["H" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(grandExcessClaims);
                                    SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                                }
                                //cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                                
                                cellIndex++;
                            }
                            #endregion

                            //#region Policy Claim Total Section

                      //MergeAndFormatCellsWithoutColor(cellsExLoss, "A", ":B", "Policy Claim Subtotal", SSG.HAlign.Left, cellIndex);

                            //cellIndex++;
                            //decimal piaAmtCTotal = GetSumForDecimalField(result1, "PIA");
                            //decimal peaAmtCTotal = GetSumForDecimalField(result1, "PEA");
                            //decimal totalPaidClaimAmtCTotal = piaAmtCTotal + peaAmtCTotal;
                            //decimal riaAmtCTotal = GetSumForDecimalField(result1, "RIA");
                            //decimal reaAmtCTotal = GetSumForDecimalField(result1, "REA");
                            //decimal totalReserClaimAmtCTotal = riaAmtCTotal + reaAmtCTotal;
                            //decimal grandClaimAmtCTotal = totalPaidClaimAmtCTotal + totalReserClaimAmtCTotal;

                            //cellsExLoss["A" + cellIndex.ToString()].Value = "Total Claim Amount";
                            //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(piaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                            //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(peaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                            //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalPaidClaimAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                            //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(riaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                            //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(reaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                            //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalReserClaimAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                            ////cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                            //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandClaimAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);

                            //cellIndex++;
                            //decimal npiaAmtCTotal = GetSumForDecimalField(result1, "NPIA");
                            //decimal npeaAmtCTotal = GetSumForDecimalField(result1, "NPEA");
                            //decimal totalPaidNonPayKindCTotal = npiaAmtCTotal + npeaAmtCTotal;
                            //decimal nriaAmtCTotal = GetSumForDecimalField(result1, "NRIA");
                            //decimal nreaAmtCTotal = GetSumForDecimalField(result1, "NREA");
                            //decimal totalReserNonPayKindCTotal = nriaAmtCTotal + nreaAmtCTotal;
                            //decimal grandPayKindCTotal = totalPaidNonPayKindCTotal + totalReserNonPayKindCTotal;

                            //cellsExLoss["A" + cellIndex.ToString()].Value = "Total Non Billable Pay -Kind Codes";
                            //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(npiaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                            //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(npeaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                            //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalPaidNonPayKindCTotal);
                            //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                            //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(nriaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                            //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(nreaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                            //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalReserNonPayKindCTotal);
                            //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                            ////cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                            //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandPayKindCTotal);
                            //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);

                            //cellIndex++;
                            //decimal spiaAmtCTotal = GetSumForDecimalField(result1, "SPIA");
                            //decimal speaAmtCTotal = GetSumForDecimalField(result1, "SPEA");
                            //decimal totalPaidSubjClaimsCTotal = spiaAmtCTotal + speaAmtCTotal;
                            //decimal sriaAmtCTotal = GetSumForDecimalField(result1, "SRIA");
                            //decimal sreaAmtCTotal = GetSumForDecimalField(result1, "SREA");
                            //decimal totalReservSubjClaimsCTotal = sriaAmtCTotal + sreaAmtCTotal;
                            //decimal grandSubjClaimsCTotal = totalPaidSubjClaimsCTotal + totalReservSubjClaimsCTotal;

                            //cellsExLoss["A" + cellIndex.ToString()].Value = "Total Subject Claims";
                            //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(spiaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                            //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(speaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                            //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalPaidSubjClaimsCTotal);
                            //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                            //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(sriaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                            //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(sreaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                            //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalReservSubjClaimsCTotal);
                            //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                            //cellsExLoss["H" + cellIndex.ToString()].Value = Convert.ToString(GetSumForDecimalField(result1, "LDF"));
                            //SetCurrencyFormat(cellsExLoss["H" + cellIndex.ToString()]);
                            //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandSubjClaimsCTotal);
                            //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);

                            //cellIndex++;
                            //decimal epiaAmtCTotal = GetSumForDecimalField(result1, "EPIA");
                            //decimal epeaAmtCTotal = GetSumForDecimalField(result1, "EPEA");
                            //decimal totalpaidExcessClaimsCTotal = epiaAmtCTotal + epeaAmtCTotal;
                            //decimal eriaAmtCTotal = GetSumForDecimalField(result1, "ERIA");
                            //decimal ereaAmtCTotal = GetSumForDecimalField(result1, "EREA");
                            //decimal totalreservExcessClaimsCTotal = eriaAmtCTotal + ereaAmtCTotal;
                            //decimal grandExcessClaimsCTotal = totalpaidExcessClaimsCTotal + totalreservExcessClaimsCTotal;

                            //cellsExLoss["A" + cellIndex.ToString()].Value = "Total Excess Claims";
                            //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(epiaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                            //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(epeaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                            //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalpaidExcessClaimsCTotal);
                            //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                            //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(eriaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                            //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(ereaAmtCTotal);
                            //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                            //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalreservExcessClaimsCTotal);
                            //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                            ////cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                            //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandExcessClaimsCTotal);
                            //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);
                            //cellIndex++;

                            //#endregion
                        }
                        //cellIndex = cellIndex + 1;
                        #region Grand Total Section

                        MergeAndFormatCellsWithoutColor(cellsExLoss, "A", ":B", "Claim Grand Total", SSG.HAlign.Left, cellIndex);

                        cellIndex++;
                        //decimal piaAmtGTotal = GetSumForDecimalField(result, "PIA");
                        //decimal peaAmtGTotal = GetSumForDecimalField(result, "PEA");
                        //decimal totalPaidClaimAmtGTotal = piaAmtGTotal + peaAmtGTotal;
                        //decimal riaAmtGTotal = GetSumForDecimalField(result, "RIA");
                        //decimal reaAmtGTotal = GetSumForDecimalField(result, "REA");
                        //decimal totalReserClaimAmtGTotal = riaAmtGTotal + reaAmtGTotal;
                        //decimal grandClaimAmtGTotal = totalPaidClaimAmtGTotal + totalReserClaimAmtGTotal;

                        //cellsExLoss["A" + cellIndex.ToString()].Value = "Total Claim Amount";
                        //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(piaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                        //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(peaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                        //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalPaidClaimAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                        //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(riaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                        //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(reaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                        //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalReserClaimAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                        ////cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                        //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandClaimAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);

                        //cellIndex++;
                        //decimal npiaAmtGTotal = GetSumForDecimalField(result, "NPIA");
                        //decimal npeaAmtGTotal = GetSumForDecimalField(result, "NPEA");
                        //decimal totalPaidNonPayKindGTotal = npiaAmtGTotal + npeaAmtGTotal;
                        //decimal nriaAmtGTotal = GetSumForDecimalField(result, "NRIA");
                        //decimal nreaAmtGTotal = GetSumForDecimalField(result, "NREA");
                        //decimal totalReserNonPayKindGTotal = nriaAmtGTotal + nreaAmtGTotal;
                        //decimal grandPayKindGTotal = totalPaidNonPayKindGTotal + totalReserNonPayKindGTotal;

                        //cellsExLoss["A" + cellIndex.ToString()].Value = "Total Non Billable Pay -Kind Codes";
                        //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(npiaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                        //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(npeaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                        //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalPaidNonPayKindGTotal);
                        //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                        //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(nriaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                        //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(nreaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                        //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalReserNonPayKindGTotal);
                        //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                        ////cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                        //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandPayKindGTotal);
                        //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);

                        //cellIndex++;
                        //decimal spiaAmtGTotal = GetSumForDecimalField(result, "SPIA");
                        //decimal speaAmtGTotal = GetSumForDecimalField(result, "SPEA");
                        //decimal totalPaidSubjClaimSSGTotal = spiaAmtGTotal + speaAmtGTotal;
                        //decimal sriaAmtGTotal = GetSumForDecimalField(result, "SRIA");
                        //decimal sreaAmtGTotal = GetSumForDecimalField(result, "SREA");
                        //decimal totalReservSubjClaimSSGTotal = sriaAmtGTotal + sreaAmtGTotal;
                        //decimal grandSubjClaimSSGTotal = totalPaidSubjClaimSSGTotal + totalReservSubjClaimSSGTotal;

                        //cellsExLoss["A" + cellIndex.ToString()].Value = "Total Subject Claims";
                        //cellsExLoss["B" + cellIndex.ToString()].Value = Convert.ToString(spiaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["B" + cellIndex.ToString()]);
                        //cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(speaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                        //cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(totalPaidSubjClaimSSGTotal);
                        //SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                        //cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(sriaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                        //cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(sreaAmtGTotal);
                        //SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                        //cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(totalReservSubjClaimSSGTotal);
                        //SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                        //cellsExLoss["H" + cellIndex.ToString()].Value = Convert.ToString(GetSumForDecimalField(result, "LDF"));
                        //SetCurrencyFormat(cellsExLoss["H" + cellIndex.ToString()]);
                        //cellsExLoss["I" + cellIndex.ToString()].Value = Convert.ToString(grandSubjClaimSSGTotal);
                        //SetCurrencyFormat(cellsExLoss["I" + cellIndex.ToString()]);

                        //cellIndex++;
                        decimal epiaAmtGTotal = GetSumForDecimalField(result, "EPIA");
                        decimal epeaAmtGTotal = GetSumForDecimalField(result, "EPEA");
                        decimal totalpaidExcessClaimSSGTotal = epiaAmtGTotal + epeaAmtGTotal;
                        decimal eriaAmtGTotal = GetSumForDecimalField(result, "ERIA");
                        decimal ereaAmtGTotal = GetSumForDecimalField(result, "EREA");
                        decimal totalreservExcessClaimSSGTotal = eriaAmtGTotal + ereaAmtGTotal;
                        decimal grandExcessClaimSSGTotal = totalpaidExcessClaimSSGTotal + totalreservExcessClaimSSGTotal;

                        decimal LDFTotal = GetSumForDecimalField(result, "LDF");

                        cellsExLoss["A" + cellIndex.ToString()].Value = "Total Excess Claims";
                        FormatHeaderTypeCellExcessLoss(cellsExLoss["A" + cellIndex.ToString()]);
                        cellsExLoss["B" + cellIndex.ToString()].Value = "";
                        FormatHeaderTypeCellRetro(cellsExLoss["B" + cellIndex.ToString()]);
                        cellsExLoss["C" + cellIndex.ToString()].Value = Convert.ToString(epiaAmtGTotal);
                        SetCurrencyFormat(cellsExLoss["C" + cellIndex.ToString()]);
                        FormatHeaderTypeCellRetro(cellsExLoss["C" + cellIndex.ToString()]);
                        cellsExLoss["D" + cellIndex.ToString()].Value = Convert.ToString(epeaAmtGTotal);
                        SetCurrencyFormat(cellsExLoss["D" + cellIndex.ToString()]);
                        FormatHeaderTypeCellRetro(cellsExLoss["D" + cellIndex.ToString()]);
                        cellsExLoss["E" + cellIndex.ToString()].Value = Convert.ToString(eriaAmtGTotal);
                        SetCurrencyFormat(cellsExLoss["E" + cellIndex.ToString()]);
                        FormatHeaderTypeCellRetro(cellsExLoss["E" + cellIndex.ToString()]);
                        cellsExLoss["F" + cellIndex.ToString()].Value = Convert.ToString(ereaAmtGTotal);
                        SetCurrencyFormat(cellsExLoss["F" + cellIndex.ToString()]);
                        FormatHeaderTypeCellRetro(cellsExLoss["F" + cellIndex.ToString()]);
                        //cellsExLoss["H" + cellIndex.ToString()].Value = string.Empty;
                        if (LDForIBNRHeaderInd == 1)
                        {
                            cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(LDFTotal);
                            FormatHeaderTypeCellRetro(cellsExLoss["G" + cellIndex.ToString()]);
                            SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                            cellsExLoss["H" + cellIndex.ToString()].Value = Convert.ToString(grandExcessClaimSSGTotal);
                            SetCurrencyFormat(cellsExLoss["H" + cellIndex.ToString()]);
                            FormatHeaderTypeCellRetro(cellsExLoss["H" + cellIndex.ToString()]);
                        }
                        else
                        {
                            cellsExLoss["G" + cellIndex.ToString()].Value = Convert.ToString(grandExcessClaimSSGTotal);
                            SetCurrencyFormat(cellsExLoss["G" + cellIndex.ToString()]);
                            FormatHeaderTypeCellRetro(cellsExLoss["G" + cellIndex.ToString()]);
                        }
                        
                        cellIndex++;
                        #endregion

                        cellIndex = cellIndex + 2;
                    }

                    worksheetExLoss.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetExLoss.UsedRange.ColumnCount; col++)
                    {
                        worksheetExLoss.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:Paid Loss Billing

                DataTable dtPaidLoss = (new ProgramPeriodsBS()).GetPaidLossBillingReport(AdjNo, 1, false);

                if (extIntFlag == 1)
                {

                    if (dtPaidLoss != null && dtPaidLoss.Rows.Count > 0)
                    {
                        workbook.Worksheets.Add();
                        sheetIndex++;
                        SSG.IWorksheet worksheetPaidLoss = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                        worksheetPaidLoss.Name = "Paid Loss Billing";

                        SSG.IRange cellsPaidLoss = worksheetPaidLoss.Cells;
                        DataRow dr = dtPaidLoss.Rows[0];
                        cellIndex = 1;

                        // Add column headers. 
                        cellsPaidLoss["A" + cellIndex.ToString()].Value = "Insured Name:";
                        FormatHeaderTypeCell(cellsPaidLoss["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsPaidLoss, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                        cellsPaidLoss["D" + cellIndex.ToString()].Value = "Broker:";
                        FormatHeaderTypeCell(cellsPaidLoss["D" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsPaidLoss, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                        cellIndex = cellIndex + 2;

                        FormatHeaderTypeCell(cellsPaidLoss["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                        cellsPaidLoss["A" + cellIndex.ToString()].Value = "Program Period";
                        cellsPaidLoss["B" + cellIndex.ToString()].Value = "Adjustment Number";
                        cellsPaidLoss["C" + cellIndex.ToString()].Value = "Loss Type";
                        cellsPaidLoss["D" + cellIndex.ToString()].Value = "Loss Val Date";
                        cellsPaidLoss["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                        cellsPaidLoss["F" + cellIndex.ToString()].Value = "Adjustment Date";
                        cellIndex++;

                        List<DataTable> programPeriods1 = dtPaidLoss.AsEnumerable()
                                              .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .Select(g => g.CopyToDataTable())
                                             .ToList();
                        Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                        //Sorting fix
                        List<DataTable> programPeriods = new List<DataTable>();
                        foreach (var item in prgPrdList)
                        {
                            DataTable dtPrgPrd = (from dts in dtPaidLoss.AsEnumerable()
                                                  where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            programPeriods.Add(dtPrgPrd);
                        }

                        for (int i = 0; i < programPeriods.Count; i++)
                        {
                            DataTable detail = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsPaidLoss["A" + cellIndex.ToString()].Value = prgPrdAndType;
                            cellsPaidLoss["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                            cellsPaidLoss["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                            cellsPaidLoss["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                            cellsPaidLoss["E" + cellIndex.ToString()].Value = strInvNo;
                            cellsPaidLoss["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();

                            cellIndex++;
                        }

                        cellIndex++;

                        for (int i = 0; i < programPeriods.Count; i++)
                        {

                            DataTable result = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsPaidLoss["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsPaidLoss["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsPaidLoss, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                            cellIndex = cellIndex + 2;

                            //Split tables as per LSI VALUATION DATE under a programperiod
                            List<DataTable> lossDetailsByDate = result.AsEnumerable()
                                               .OrderByDescending(ord => ord.Field<DateTime>("LSI VALUATION DATE"))
                                               .GroupBy(row => row.Field<DateTime>("LSI VALUATION DATE"))
                                               .Select(g => g.CopyToDataTable())
                                               .ToList();

                            List<string> lobColNames = result.AsEnumerable()
                                                      .Select(row => row.Field<string>("LOB")).Distinct()
                                                      .ToList();


                            Dictionary<string, string> cellXLobList = new Dictionary<string, string>();
                            string cellNxtCol = string.Empty;
                            //to find next column
                            int nxtASCICode = (int)Convert.ToChar("A") + 1;

                            cellsPaidLoss["A" + cellIndex.ToString()].Value = "Valuation Date";
                            foreach (var item in lobColNames)
                            {
                                cellNxtCol = char.ConvertFromUtf32(nxtASCICode);
                                cellsPaidLoss[cellNxtCol + cellIndex.ToString()].Value = item;
                                cellXLobList.Add(cellNxtCol, item);
                                nxtASCICode++;
                            }
                            cellNxtCol = char.ConvertFromUtf32(nxtASCICode);
                            cellsPaidLoss[cellNxtCol + cellIndex.ToString()].Value = "TOTALS";
                            cellXLobList.Add(cellNxtCol, "TOTALS");

                            FormatHeaderTypeCell(cellsPaidLoss["A" + cellIndex.ToString() + ":" + cellNxtCol + cellIndex.ToString()]);

                            cellIndex++;

                            int nxtCellIdx = cellIndex;

                            for (int k = 0; k < lossDetailsByDate.Count; k++)
                            {
                                DataTable resultk = lossDetailsByDate[k];

                                var details = from item in resultk.AsEnumerable()
                                              orderby item.Field<String>("LOB")
                                              group item by item.Field<String>("LOB") into g
                                              select new
                                              {
                                                  LOB = g.Key,
                                                  AMOUNT = g.Sum(item => item.Field<decimal?>("AMOUNT"))
                                              };
                                cellsPaidLoss["A" + nxtCellIdx.ToString()].Value = resultk.Rows[0]["LSI VALUATION DATE"];

                                foreach (var itemL in cellXLobList)
                                {
                                    var lobD = (from lobN in details
                                                where lobN.LOB == itemL.Value
                                                select new { name = lobN.LOB, Amount = lobN.AMOUNT }).FirstOrDefault();

                                    if (lobD != null && !String.IsNullOrEmpty(lobD.name) && itemL.Value == lobD.name)
                                    {
                                        cellsPaidLoss[itemL.Key + nxtCellIdx.ToString()].Value = Convert.ToString(lobD.Amount);
                                    }
                                    else
                                    {
                                        cellsPaidLoss[itemL.Key + nxtCellIdx.ToString()].Value = "0";
                                    }
                                    SetCurrencyFormatForDecimal(cellsPaidLoss[itemL.Key + nxtCellIdx.ToString()]);
                                }

                                cellsPaidLoss[cellXLobList.Last().Key + nxtCellIdx.ToString()].Value = GetSumForDecimalField(resultk, "AMOUNT");
                                SetCurrencyFormatForDecimal(cellsPaidLoss[cellXLobList.Last().Key + nxtCellIdx.ToString()]);

                                nxtCellIdx++;
                            }

                            cellsPaidLoss["A" + nxtCellIdx.ToString()].Value = "TOTAL";

                            var groupAmounts = from item in result.AsEnumerable()
                                               orderby item.Field<String>("LOB")
                                               group item by item.Field<String>("LOB") into g
                                               select new
                                               {
                                                   LOB = g.Key,
                                                   AMOUNT = g.Sum(item => item.Field<decimal?>("AMOUNT"))
                                               };
                            foreach (var itemL in cellXLobList)
                            {
                                var lobD = (from lobN in groupAmounts
                                            where lobN.LOB == itemL.Value
                                            select new { name = lobN.LOB, Amount = lobN.AMOUNT }).FirstOrDefault();

                                if (lobD != null && !String.IsNullOrEmpty(lobD.name) && itemL.Value == lobD.name)
                                {
                                    cellsPaidLoss[itemL.Key + nxtCellIdx.ToString()].Value = Convert.ToString(lobD.Amount);
                                }
                                else
                                {
                                    cellsPaidLoss[itemL.Key + nxtCellIdx.ToString()].Value = "0";
                                }
                                SetCurrencyFormat(cellsPaidLoss[itemL.Key + nxtCellIdx.ToString()]);
                            }
                            cellsPaidLoss[cellXLobList.Last().Key + nxtCellIdx.ToString()].Value = GetSumForDecimalField(result, "AMOUNT");
                            SetCurrencyFormat(cellsPaidLoss[cellXLobList.Last().Key + nxtCellIdx.ToString()]);

                            nxtCellIdx++;
                            cellIndex = nxtCellIdx;
                            cellIndex = cellIndex + 2;
                        }

                        worksheetPaidLoss.UsedRange.Columns.AutoFit();
                        for (int col = 0; col < worksheetPaidLoss.UsedRange.ColumnCount; col++)
                        {
                            worksheetPaidLoss.Cells[1, col].ColumnWidth *= 1.15;
                        }
                    }
                }

                #endregion

                #region Sheet:Surcharge And Assessment

                DataTable dtSAMain = (new ProgramPeriodsBS()).GetSurchargesAssessmentReport(AdjNo, 1, false, false);

                if (dtSAMain != null && dtSAMain.Rows.Count > 0)
                {
                    List<DataTable> dtSASub = dtSAMain.AsEnumerable()
                                            .GroupBy(row => row.Field<string>("ISRevised"))
                                            .Select(g => g.CopyToDataTable())
                                            .ToList();


                    for (int k = 0; k < dtSASub.Count; k++)
                    {
                        DataTable dtSA = dtSASub[k];
                        if (extIntFlag == 0 && Convert.ToString(dtSA.Rows[0]["ISRevised"]) == "True")
                        {
                            continue;
                        }
                        workbook.Worksheets.Add();
                        sheetIndex++;
                        SSG.IWorksheet worksheetSA = workbook.Worksheets["Sheet" + sheetIndex.ToString()];

                        if (Convert.ToString(dtSA.Rows[0]["ISRevised"]) == "True")
                        {
                            worksheetSA.Name = "Surcharge And Assessment - RTI";
                        }
                        else
                        {
                            worksheetSA.Name = "Surcharge And Assessment";
                        }

                        SSG.IRange cellsSA = worksheetSA.Cells;
                        DataRow dr = dtSA.Rows[0];
                        cellIndex = 1;

                        // Add column headers. 
                        cellsSA["A" + cellIndex.ToString()].Value = "Insured Name:";
                        FormatHeaderTypeCell(cellsSA["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsSA, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                        cellsSA["D" + cellIndex.ToString()].Value = "Broker:";
                        FormatHeaderTypeCell(cellsSA["D" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsSA, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                        cellIndex = cellIndex + 2;

                        FormatHeaderTypeCell(cellsSA["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                        cellsSA["A" + cellIndex.ToString()].Value = "Program Period";
                        cellsSA["B" + cellIndex.ToString()].Value = "Adjustment Number";
                        cellsSA["C" + cellIndex.ToString()].Value = "Loss Type";
                        cellsSA["D" + cellIndex.ToString()].Value = "Loss Val Date";
                        cellsSA["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                        cellsSA["F" + cellIndex.ToString()].Value = "Adjustment Date";
                        cellIndex++;

                        List<DataTable> programPeriods1 = dtSA.AsEnumerable()
                                                .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                                .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                                .Select(g => g.CopyToDataTable())
                                                .ToList();


                        Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                        //Sorting fix
                        List<DataTable> programPeriods = new List<DataTable>();
                        foreach (var item in prgPrdList)
                        {
                            DataTable dtPrgPrd = (from dts in dtSA.AsEnumerable()
                                                  where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            programPeriods.Add(dtPrgPrd);
                        }

                        for (int i = 0; i < programPeriods.Count; i++)
                        {
                            DataTable detail = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsSA["A" + cellIndex.ToString()].Value = prgPrdAndType;
                            cellsSA["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                            cellsSA["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                            cellsSA["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                            if (Convert.ToString(detail.Rows[0]["ISRevised"]) == "True")
                            {
                                cellsSA["E" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["INVOICE NUMBER"]);
                                cellsSA["F" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["INVOICE DATE"]);
                            }
                            else
                            {
                                cellsSA["E" + cellIndex.ToString()].Value = strInvNo;
                                cellsSA["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                            }
                            cellIndex++;
                        }

                        //foreach (DataRow drRows in dtSA.Rows)
                        //{
                        //    cellsSA["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["PROGRAM PERIOD"]);
                        //    cellsSA["B" + cellIndex.ToString()].Value = Convert.ToString(drRows["ADJUSTMENT NUMBER"]);
                        //    cellsSA["C" + cellIndex.ToString()].Value = Convert.ToString(drRows["ADJUSTMENT TYPE"]);
                        //    cellsSA["D" + cellIndex.ToString()].Value = Convert.ToString(drRows["VALUATION DATE"]);
                        //    if (Convert.ToString(drRows["ISRevised"]) == "True")
                        //    {
                        //        cellsSA["E" + cellIndex.ToString()].Value = Convert.ToString(drRows["INVOICE NUMBER"]);
                        //        cellsSA["F" + cellIndex.ToString()].Value = Convert.ToString(drRows["INVOICE DATE"]);
                        //    }
                        //    else
                        //    {
                        //        cellsSA["E" + cellIndex.ToString()].Value = strInvNo;
                        //        cellsSA["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                        //    }
                        //    cellIndex++;                        
                        //}
                        cellIndex++;

                        FormatHeaderTypeCell(cellsSA["A" + cellIndex.ToString() + ":L" + cellIndex.ToString()]);

                        cellsSA["A" + cellIndex.ToString()].Value = "Surcharge/Assessment Code";
                        cellsSA["B" + cellIndex.ToString()].Value = "Description";
                        cellsSA["C" + cellIndex.ToString()].Value = "Basic Premium";
                        cellsSA["D" + cellIndex.ToString()].Value = "Incurred Losses";
                        cellsSA["E" + cellIndex.ToString()].Value = "Current ERP";
                        cellsSA["F" + cellIndex.ToString()].Value = "Prior ERP/ Standard Subject Premium";
                        cellsSA["G" + cellIndex.ToString()].Value = "Retro Result";
                        cellsSA["H" + cellIndex.ToString()].Value = "Additional Surcharge/Assessment Components";
                        cellsSA["I" + cellIndex.ToString()].Value = "Total Surcharge / Assessment Base";
                        cellsSA["J" + cellIndex.ToString()].Value = "Factor";
                        cellsSA["K" + cellIndex.ToString()].Value = "Other Surcharges and Credits";
                        cellsSA["L" + cellIndex.ToString()].Value = "Total additional/ Return";
                        cellIndex++;


                        for (int i = 0; i < programPeriods.Count; i++)
                        {
                            DataTable result = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();

                            cellsSA["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsSA["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsSA, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                            cellIndex = cellIndex + 1;

                            //Split tables as per number of policies under a programperiod 
                            List<DataTable> policyDetails = result.AsEnumerable()
                                               .OrderBy(ord => ord.Field<string>("POLICY NUMBER"))
                                               .GroupBy(row => row.Field<string>("POLICY NUMBER"))
                                               .Select(g => g.CopyToDataTable())
                                               .ToList();

                            for (int j = 0; j < policyDetails.Count; j++)
                            {
                                DataTable result1 = policyDetails[j];
                                string policyNumber = Convert.ToString(result1.Rows[0]["POLICY NUMBER"]);
                                cellsSA["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                                FormatHeaderTypeCell(cellsSA["A" + cellIndex.ToString()]);
                                MergeAndFormatCellsWithoutColor(cellsSA, "B", ":C", policyNumber, SSG.HAlign.Left, cellIndex);
                                cellIndex++;

                                foreach (DataRow drRows in result1.Rows)
                                {
                                    cellsSA["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["STATE CD"]);
                                    cellsSA["B" + cellIndex.ToString()].Value = Convert.ToString(drRows["ABBREV"]);
                                    cellsSA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "BASIC AMT");
                                    SetCurrencyFormat(cellsSA["C" + cellIndex.ToString()]);
                                    cellsSA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "INCURRED LOSSES");
                                    SetCurrencyFormat(cellsSA["D" + cellIndex.ToString()]);
                                    cellsSA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "CURR ERP");
                                    SetCurrencyFormat(cellsSA["E" + cellIndex.ToString()]);
                                    cellsSA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "PREV ERP");
                                    SetCurrencyFormat(cellsSA["F" + cellIndex.ToString()]);
                                    cellsSA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "RETRO RESULT");
                                    SetCurrencyFormat(cellsSA["G" + cellIndex.ToString()]);
                                    cellsSA["H" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "ADDN SURCHARGE ASSESSMENT COMP");
                                    SetCurrencyFormat(cellsSA["H" + cellIndex.ToString()]);
                                    cellsSA["I" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL SURCHARGE ASSESSMENT BASE");
                                    SetCurrencyFormat(cellsSA["I" + cellIndex.ToString()]);
                                    cellsSA["J" + cellIndex.ToString()].Value = Convert.ToString(drRows["FACTOR"]);
                                    cellsSA["K" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "OTHER SURCHARGE AMOUNT");
                                    SetCurrencyFormat(cellsSA["K" + cellIndex.ToString()]);
                                    cellsSA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL ADDITIONAL RETURN");
                                    SetCurrencyFormat(cellsSA["L" + cellIndex.ToString()]);
                                    cellIndex++;
                                }
                                decimal policyTotal = GetSumForDecimalField(result1, "TOTAL ADDITIONAL RETURN");
                                cellsSA["K" + cellIndex.ToString()].Value = "Policy Total";
                                FormatHeaderTypeCell(cellsSA["K" + cellIndex.ToString()]);
                                cellsSA["L" + cellIndex.ToString()].Value = Convert.ToString(policyTotal);
                                SetCurrencyFormat(cellsSA["L" + cellIndex.ToString()]);
                                cellIndex++;
                            }
                            decimal prgPeriodTotal = GetSumForDecimalField(result, "TOTAL ADDITIONAL RETURN");
                            cellsSA["K" + cellIndex.ToString()].Value = "Total";
                            FormatHeaderTypeCell(cellsSA["K" + cellIndex.ToString()]);
                            cellsSA["L" + cellIndex.ToString()].Value = Convert.ToString(prgPeriodTotal);
                            SetCurrencyFormat(cellsSA["L" + cellIndex.ToString()]);
                            cellIndex = cellIndex + 2;
                        }

                        worksheetSA.UsedRange.Columns.AutoFit();
                        for (int col = 0; col < worksheetSA.UsedRange.ColumnCount; col++)
                        {
                            worksheetSA.Cells[1, col].ColumnWidth *= 1.15;
                        }

                    }

                }

                #endregion

                #region Sheet:LRFA

                DataTable dtLRFA = (new ProgramPeriodsBS()).GetILRFReport(AdjNo, 1, false, 0);

                

                if (extIntFlag == 1)
                {

                    if (dtLRFA != null && dtLRFA.Rows.Count > 0)
                    {
                        workbook.Worksheets.Add();
                        sheetIndex++;
                        SSG.IWorksheet worksheetLRFA = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                        worksheetLRFA.Name = "LRFA";

                        SSG.IRange cellsLRFA = worksheetLRFA.Cells;
                        DataRow dr = dtLRFA.Rows[0];
                        cellIndex = 1;

                        // Add column headers. 
                        cellsLRFA["A" + cellIndex.ToString()].Value = "Insured Name:";
                        FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsLRFA, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                        cellsLRFA["D" + cellIndex.ToString()].Value = "Broker:";
                        FormatHeaderTypeCell(cellsLRFA["D" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsLRFA, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                        cellIndex = cellIndex + 2;

                        FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":G" + cellIndex.ToString()]);

                        cellsLRFA["A" + cellIndex.ToString()].Value = "Program Period";
                        cellsLRFA["B" + cellIndex.ToString()].Value = "Adjustment Number";
                        cellsLRFA["C" + cellIndex.ToString()].Value = "Loss Type";
                        cellsLRFA["D" + cellIndex.ToString()].Value = "Loss Val Date";
                        cellsLRFA["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                        cellsLRFA["F" + cellIndex.ToString()].Value = "Adjustment Date";
                        cellsLRFA["G" + cellIndex.ToString()].Value = "LDF/IBNR";
                        cellIndex++;

                        List<DataTable> programPeriods1 = dtLRFA.AsEnumerable()
                                             .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .Select(g => g.CopyToDataTable())
                                             .ToList();

                        Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                        //Sorting fix
                        List<DataTable> programPeriods = new List<DataTable>();
                        foreach (var item in prgPrdList)
                        {
                            DataTable dtPrgPrd = (from dts in dtLRFA.AsEnumerable()
                                                  where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            programPeriods.Add(dtPrgPrd);
                        }

                        for (int i = 0; i < programPeriods.Count; i++)
                        {
                            DataTable detail = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsLRFA["A" + cellIndex.ToString()].Value = prgPrdAndType;
                            cellsLRFA["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                            cellsLRFA["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                            cellsLRFA["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                            cellsLRFA["E" + cellIndex.ToString()].Value = strInvNo;
                            cellsLRFA["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                            cellsLRFA["G" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["LDF/IBNR"]);
                            cellIndex++;
                        }

                        cellIndex++;

                        for (int i = 0; i < programPeriods.Count; i++)
                        {

                            DataTable result = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsLRFA["A" + cellIndex.ToString()].Value = "Program Period:";
                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsLRFA, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                            cellIndex++;
                            cellsLRFA["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsLRFA, "B", ":G", Convert.ToString(result.Rows[0]["POLICIES"]), SSG.HAlign.Left, cellIndex);
                            cellIndex++;
                            cellsLRFA["A" + cellIndex.ToString()].Value = "Aggregate Limit:";
                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                            if (result.Rows[0]["AGMT LIM IND"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["AGMT LIM IND"]) == true)
                            {
                                cellsLRFA["B" + cellIndex.ToString()].Value = "UNLIMITED";

                            }
                            else
                            {
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AGGR LIMIT");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                            }

                            cellsLRFA["C" + cellIndex.ToString()].Value = "Minimum required:";
                            FormatHeaderTypeCell(cellsLRFA["C" + cellIndex.ToString()]);
                            if (result.Rows[0]["MIN LIM IND"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["MIN LIM IND"]) == true)
                            {
                                cellsLRFA["D" + cellIndex.ToString()].Value = "UNLIMITED";
                            }
                            else
                            {
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "MIN REQ");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                            }

                            cellIndex = cellIndex + 1;

                            //Split tables as per LOB/State under a programperiod
                            List<DataTable> policyDetails = result.AsEnumerable()
                                               .OrderBy(ord => ord.Field<string>("LOB"))
                                               .GroupBy(row => row.Field<string>("LOB"))
                                               .Select(g => g.CopyToDataTable())
                                               .ToList();

                            for (int j = 0; j < policyDetails.Count; j++)
                            {
                                FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":L" + cellIndex.ToString()]);

                                cellsLRFA["A" + cellIndex.ToString()].Value = "LOB/State";
                                cellsLRFA["B" + cellIndex.ToString()].Value = "Paid Losses";
                                cellsLRFA["C" + cellIndex.ToString()].Value = "Paid ALAE";
                                cellsLRFA["D" + cellIndex.ToString()].Value = "Reserve Losses";
                                cellsLRFA["E" + cellIndex.ToString()].Value = "Reserve ALAE";
                                cellsLRFA["F" + cellIndex.ToString()].Value = "LBA Factor";
                                cellsLRFA["G" + cellIndex.ToString()].Value = "LBA Result";
                                cellsLRFA["H" + cellIndex.ToString()].Value = "LCF Factor";
                                cellsLRFA["I" + cellIndex.ToString()].Value = "LCF Result";
                                cellsLRFA["J" + cellIndex.ToString()].Value = "LDF/IBNR Factor";
                                cellsLRFA["K" + cellIndex.ToString()].Value = "LDF/IBNR Result";
                                cellsLRFA["L" + cellIndex.ToString()].Value = "Total Amount";
                                cellIndex++;

                                foreach (DataRow drRows in policyDetails[j].Rows)
                                {
                                    cellsLRFA["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["LOB"]) + "  " + Convert.ToString(drRows["STATE"]);
                                    cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "PAID LOSSES");
                                    SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                    cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "PAID ALAE");
                                    SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                    cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "RESERVE LOSSES");
                                    SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                    cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "RESERVE ALAE");
                                    SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                    cellsLRFA["F" + cellIndex.ToString()].Value = Convert.ToString(drRows["LBA FACTOR"]);
                                    cellsLRFA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LBA RESULT");
                                    SetCurrencyFormat(cellsLRFA["G" + cellIndex.ToString()]);
                                    cellsLRFA["H" + cellIndex.ToString()].Value = Convert.ToString(drRows["LCF FACTOR"]);
                                    cellsLRFA["I" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LCF RESULT");
                                    SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                                    cellsLRFA["J" + cellIndex.ToString()].Value = Convert.ToString(drRows["LDF FACTOR"]);
                                    cellsLRFA["K" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LDF RESULT");
                                    SetCurrencyFormat(cellsLRFA["K" + cellIndex.ToString()]);
                                    cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL AMOUNT");
                                    SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                                    cellIndex++;
                                }

                                cellsLRFA["A" + cellIndex.ToString()].Value = "Sub Total(LOB)";
                                cellsLRFA["B" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "PAID LOSSES");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "PAID ALAE");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "RESERVE LOSSES");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "RESERVE ALAE");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["G" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "LBA RESULT");
                                SetCurrencyFormat(cellsLRFA["G" + cellIndex.ToString()]);
                                cellsLRFA["I" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "LCF RESULT");
                                SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                                cellsLRFA["K" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "LDF RESULT");
                                SetCurrencyFormat(cellsLRFA["K" + cellIndex.ToString()]);
                                cellsLRFA["L" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "TOTAL AMOUNT");
                                SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);

                                cellIndex = cellIndex + 2;
                            }

                            MergeAndFormatCellsWithoutColor(cellsLRFA, "G", ":K", "Claim Handling Fees", SSG.HAlign.Left, cellIndex);
                            cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CLAIM HANDLING FEES");
                            SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                            cellIndex = cellIndex + 2;

                            cellsLRFA["A" + cellIndex.ToString()].Value = "Total :";
                            cellsLRFA["B" + cellIndex.ToString()].Value = GetSumForDecimalField(result, "PAID LOSSES");
                            SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                            cellsLRFA["C" + cellIndex.ToString()].Value = GetSumForDecimalField(result, "PAID ALAE");
                            SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                            cellsLRFA["D" + cellIndex.ToString()].Value = GetSumForDecimalField(result, "RESERVE LOSSES");
                            SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                            cellsLRFA["E" + cellIndex.ToString()].Value = GetSumForDecimalField(result, "RESERVE ALAE");
                            SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                            cellsLRFA["G" + cellIndex.ToString()].Value = GetSumForDecimalField(result, "LBA RESULT");
                            SetCurrencyFormat(cellsLRFA["G" + cellIndex.ToString()]);
                            cellsLRFA["I" + cellIndex.ToString()].Value = GetSumForDecimalField(result, "LCF RESULT");
                            SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                            cellsLRFA["K" + cellIndex.ToString()].Value = GetSumForDecimalField(result, "LDF RESULT");
                            SetCurrencyFormat(cellsLRFA["K" + cellIndex.ToString()]);
                            cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CURRENT LOSS REIMBURSEMENT FUND");
                            SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                            cellIndex = cellIndex + 2;

                            //This section(if block) only available to internal spread sheet
                            if (extIntFlag == 1)
                            {
                                FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                                cellsLRFA["A" + cellIndex.ToString()].Value = "Descr.";
                                cellsLRFA["B" + cellIndex.ToString()].Value = "Current";
                                cellsLRFA["C" + cellIndex.ToString()].Value = "Aggr.";
                                cellsLRFA["D" + cellIndex.ToString()].Value = "Limited";
                                cellsLRFA["E" + cellIndex.ToString()].Value = "Prior";
                                cellsLRFA["F" + cellIndex.ToString()].Value = "Posting";
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "WC TPD";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCTPDCURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCTPDAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCTPDLIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCTPDPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCTPDPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "Auto TPD";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOTPDCURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOTPDAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOTPDLIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOTPDPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOTPDPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "GL TPD";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLTPDCURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLTPDAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLTPDLIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLTPDPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLTPDPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "WC LCF";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCLCFCURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCLCFAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCLCFLIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCLCFPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "WCLCFPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "Auto LCF";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOLCFCURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOLCFAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOLCFLIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOLCFPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AUTOLCFPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "GL LCF";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLLCFCURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLLCFAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLLCFLIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLLCFPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "GLLCFPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "Reserve";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "RESERVECURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "RESERVEAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "RESERVELIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "RESERVEPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "RESERVEPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "CHF";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CHFCURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CHFAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CHFLIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CHFPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CHFPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "LBA";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LBACURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LBAAGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LBALIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LBAPRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LBAPOST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex++;
                                cellsLRFA["A" + cellIndex.ToString()].Value = "TOTAL";
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_CURR");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_AGGR");
                                SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_LIM");
                                SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_PRIOR");
                                SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_POST");
                                SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                cellIndex = cellIndex + 2;
                            }

                            MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "Summary :", SSG.HAlign.Left, cellIndex);
                            cellIndex++;
                            MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "Current Loss Reimbursement Fund Required :", SSG.HAlign.Left, cellIndex);
                            cellsLRFA["H" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CURRENT LOSS REIMBURSEMENT FUND");
                            SetCurrencyFormat(cellsLRFA["H" + cellIndex.ToString()]);
                            cellIndex++;
                            MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "Limited Loss Reimbursement Fund Required :", SSG.HAlign.Left, cellIndex);
                            cellsLRFA["H" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_LIM");
                            SetCurrencyFormat(cellsLRFA["H" + cellIndex.ToString()]);
                            cellIndex++;
                            MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "Loss Reimbursement Fund Previously Required :", SSG.HAlign.Left, cellIndex);
                            cellsLRFA["H" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LOSS REIMBUR FUND PREV");
                            SetCurrencyFormat(cellsLRFA["H" + cellIndex.ToString()]);
                            cellIndex++;
                            MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "Amount Due to Zurich (Due Insured) :", SSG.HAlign.Left, cellIndex);
                            decimal totLIM = !string.IsNullOrEmpty(Convert.ToString(result.Rows[0]["TOT_LIM"])) ? Convert.ToDecimal(result.Rows[0]["TOT_LIM"]) : 0;
                            decimal lrfPrev = !string.IsNullOrEmpty(Convert.ToString(result.Rows[0]["LOSS REIMBUR FUND PREV"])) ? Convert.ToDecimal(result.Rows[0]["LOSS REIMBUR FUND PREV"]) : 0;
                            decimal curResult = totLIM - lrfPrev;
                            cellsLRFA["H" + cellIndex.ToString()].Value = Convert.ToString(curResult);
                            SetCurrencyFormat(cellsLRFA["H" + cellIndex.ToString()]);
                            cellIndex = cellIndex + 2;
                        }

                        worksheetLRFA.UsedRange.Columns.AutoFit();
                        for (int col = 0; col < worksheetLRFA.UsedRange.ColumnCount; col++)
                        {
                            worksheetLRFA.Cells[1, col].ColumnWidth *= 1.15;
                        }
                    }
                }
                else if (extIntFlag == 0)
                {
                    if (dtLRFA != null && dtLRFA.Rows.Count > 0)
                    {
                        // DMR
                        DataTable dtLRFAExternal = (new ProgramPeriodsBS()).GetILRFReportExternal(AdjNo, 1, false, 0);

                        DataTable dtExcessLossExhibitExternal = (new ProgramPeriodsBS()).GetExcessLossExhibitExternalSummary(AdjNo, 1, false, 0);

                        DataTable dtILRFFormula = (new ProgramPeriodsBS()).GetILRFFormula(AdjNo);

                        DataTable dtCheckLDF_IBNR_Incl_Limit = (new ProgramPeriodsBS()).CheckLDF_IBNR_Incl_Limit(AdjNo);

                    

                        workbook.Worksheets.Add();
                        sheetIndex++;
                        SSG.IWorksheet worksheetLRFA = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                        worksheetLRFA.Name = "LRFA";

                        SSG.IRange cellsLRFA = worksheetLRFA.Cells;
                        DataRow dr = dtLRFA.Rows[0];
                        cellIndex = 1;

                        // Add column headers. 
                        cellsLRFA["A" + cellIndex.ToString()].Value = "Insured Name:";
                        FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsLRFA, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                        cellsLRFA["D" + cellIndex.ToString()].Value = "Broker:";
                        FormatHeaderTypeCell(cellsLRFA["D" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsLRFA, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                        cellIndex = cellIndex + 2;

                        FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":G" + cellIndex.ToString()]);

                        cellsLRFA["A" + cellIndex.ToString()].Value = "Program Period";
                        cellsLRFA["B" + cellIndex.ToString()].Value = "Adjustment Number";
                        cellsLRFA["C" + cellIndex.ToString()].Value = "Loss Type";
                        cellsLRFA["D" + cellIndex.ToString()].Value = "Loss Val Date";
                        cellsLRFA["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                        cellsLRFA["F" + cellIndex.ToString()].Value = "Adjustment Date";
                        cellsLRFA["G" + cellIndex.ToString()].Value = "LDF/IBNR";
                        cellIndex++;

                        List<DataTable> programPeriods1 = dtLRFA.AsEnumerable()
                                             .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                             .Select(g => g.CopyToDataTable())
                                             .ToList();

                        Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                        //Sorting fix
                        List<DataTable> programPeriods = new List<DataTable>();
                        foreach (var item in prgPrdList)
                        {
                            DataTable dtPrgPrd = (from dts in dtLRFA.AsEnumerable()
                                                  where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                                  select dts).CopyToDataTable();
                            programPeriods.Add(dtPrgPrd);
                        }

                        for (int i = 0; i < programPeriods.Count; i++)
                        {
                            DataTable detail = programPeriods[i];
                            var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            cellsLRFA["A" + cellIndex.ToString()].Value = prgPrdAndType;
                            cellsLRFA["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                            cellsLRFA["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                            cellsLRFA["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                            cellsLRFA["E" + cellIndex.ToString()].Value = strInvNo;
                            cellsLRFA["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                            cellsLRFA["G" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["LDF/IBNR"]);
                            cellIndex++;
                        }

                        cellIndex++;

                        for (int i = 0; i < programPeriods.Count; i++)
                        {

                            DataTable result = programPeriods[i];

                            //DataTable policyDetailsExternal = dtLRFAExternal.AsEnumerable().Where(p => p.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]));

                            DataTable policyDetailsExternal = (from dts in dtLRFAExternal.AsEnumerable()
                                                               where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]))
                                                  select dts).CopyToDataTable();

                            DataTable ExcessLossExhibitExternal = (from dts in dtExcessLossExhibitExternal.AsEnumerable()
                                                               where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]))
                                                               select dts).CopyToDataTable();

                            DataTable GetILRFFormula = (from dts in dtILRFFormula.AsEnumerable()
                                                                   where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]))
                                                                   select dts).CopyToDataTable();

                            DataTable CheckLDF_IBNR_Incl_Limit = (from dts in dtCheckLDF_IBNR_Incl_Limit.AsEnumerable()
                                                                  where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"]))
                                                                  select dts).CopyToDataTable();  
  
  

                            //var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                            //cellsLRFA["A" + cellIndex.ToString()].Value = "Program Period:";
                            //FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                            //MergeAndFormatCellsWithoutColor(cellsLRFA, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);

                            cellIndex = cellIndex + 2;

                            cellsLRFA["A" + cellIndex.ToString()].Value = "Minimum:";
                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                            if (result.Rows[0]["MIN LIM IND"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["MIN LIM IND"]) == true)
                            {
                                cellsLRFA["B" + cellIndex.ToString()].Value = "UNLIMITED";
                            }
                            else
                            {
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "MIN REQ");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                            }
                            cellIndex++;
                            //cellsLRFA["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                            //FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                            //MergeAndFormatCellsWithoutColor(cellsLRFA, "B", ":G", Convert.ToString(result.Rows[0]["POLICIES"]), SSG.HAlign.Left, cellIndex);
                            //cellIndex++;
                            cellsLRFA["A" + cellIndex.ToString()].Value = "Aggregate Limit:";
                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);
                            if (result.Rows[0]["AGMT LIM IND"] != DBNull.Value && Convert.ToBoolean(result.Rows[0]["AGMT LIM IND"]) == true)
                            {
                                cellsLRFA["B" + cellIndex.ToString()].Value = "UNLIMITED";

                            }
                            else
                            {
                                cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AGGR LIMIT");
                                SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                            }
                            cellIndex++;
                           // MergeAndFormatCellsWithoutColor(cellsLRFA, "A","LRF Calculation:", SSG.HAlign.Left, cellIndex);
                            cellsLRFA["A" + cellIndex.ToString()].Value = "LRF Calculation:";


                            MergeAndFormatCellsWithoutColor(cellsLRFA, "B", ":K", CheckNullOrEmptyReturnValue(GetILRFFormula.Rows[0], "FinalFormula"), SSG.HAlign.Left, cellIndex);

                            //cellsLRFA["B" + cellIndex.ToString() + ":K" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(GetILRFFormula.Rows[0], "FinalFormula");
                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);

                           
                            cellIndex = cellIndex + 1;

                            //Split tables as per LOB/State under a programperiod
                            List<DataTable> policyDetailsLob = policyDetailsExternal.AsEnumerable()
                                               .OrderBy(ord => ord.Field<string>("LOB"))
                                               .GroupBy(row => row.Field<string>("LOB"))
                                               .Select(g => g.CopyToDataTable())
                                               .ToList();

                            //for (int j = 0; j < policyDetails.Count; j++)


                            int GetLDF_IBNR_Incl_Limit = Convert.ToInt32(CheckNullOrEmptyReturnValue(CheckLDF_IBNR_Incl_Limit.Rows[0], "GetLDF_IBNR_Incl_Limit"));
                            //{
                            if (GetLDF_IBNR_Incl_Limit == 1)
                            {
                                FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":L" + cellIndex.ToString()]);
                            }
                            else
                            {
                                FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":J" + cellIndex.ToString()]);
                            }

                                cellsLRFA["A" + cellIndex.ToString()].Value = "LOB/STATE";
                                cellsLRFA["B" + cellIndex.ToString()].Value = "Incurred Loss within Limit (1 & 3)";
                                cellsLRFA["C" + cellIndex.ToString()].Value = "Incurred ALAE within Limit (2 & 4)";
                                cellsLRFA["D" + cellIndex.ToString()].Value = "Total Limited Losses";
                                cellsLRFA["E" + cellIndex.ToString()].Value = "LCF Factor";
                                cellsLRFA["F" + cellIndex.ToString()].Value = "LCF Result";
                                cellsLRFA["G" + cellIndex.ToString()].Value = "LBA Result";
                                if (GetLDF_IBNR_Incl_Limit == 1)
                                {
                                    cellsLRFA["H" + cellIndex.ToString()].Value = "Incurred Limited Developed Losses";
                                    cellsLRFA["I" + cellIndex.ToString()].Value = "Incurred Limited Developed ALAE";
                                    cellsLRFA["J" + cellIndex.ToString()].Value = "LDF or IBNR Factor";
                                    cellsLRFA["K" + cellIndex.ToString()].Value = "LDF or IBNR Result";
                                    cellsLRFA["L" + cellIndex.ToString()].Value = "Total Amount";
                                }
                                else
                                {
                                    cellsLRFA["H" + cellIndex.ToString()].Value = "LDF or IBNR Factor";
                                    cellsLRFA["I" + cellIndex.ToString()].Value = "LDF or IBNR Result";
                                    cellsLRFA["J" + cellIndex.ToString()].Value = "Total Amount";
                                }

                                
                                cellIndex++;

                                //foreach (DataRow drRows in policyDetailsLob[j].Rows)
                                //{
                                for (int j = 0; j < policyDetailsLob.Count; j++)
                                {
                                    foreach (DataRow drRows in policyDetailsLob[j].Rows)
                                    {
                                        cellsLRFA["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "LOB/STATE"));
                                        cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Limited_Incurred_Loss"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                        SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                        cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Limited_Incurred_ALAE");
                                        SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                        cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Total_Limited_Losses");
                                        SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                        cellsLRFA["E" + cellIndex.ToString()].Value = Convert.ToString(drRows["LCF_FACTOR"]);
                                        cellsLRFA["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LCF_RESULT");
                                        SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                                        cellsLRFA["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LBA_RESULT");
                                        SetCurrencyFormat(cellsLRFA["G" + cellIndex.ToString()]);
                                        if (GetLDF_IBNR_Incl_Limit == 1)
                                        {
                                            cellsLRFA["H" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Incurred_Limited_Developed_Loss");
                                            SetCurrencyFormat(cellsLRFA["H" + cellIndex.ToString()]);
                                            cellsLRFA["I" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "Incurred_Limited_Developed_ALAE");
                                            SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                                            cellsLRFA["J" + cellIndex.ToString()].Value = Convert.ToString(drRows["LDF_FACTOR"]);
                                            cellsLRFA["K" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LDF_RESULT");
                                            SetCurrencyFormat(cellsLRFA["K" + cellIndex.ToString()]);
                                            cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL_AMOUNT_OF_TLL_LDF_LCF");
                                            SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                                        }
                                        else
                                        {
                                            cellsLRFA["H" + cellIndex.ToString()].Value = Convert.ToString(drRows["LDF_FACTOR"]);
                                            cellsLRFA["I" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "LDF_RESULT");
                                            SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                                            cellsLRFA["J" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL_AMOUNT_OF_TLL_LDF_LCF");
                                            SetCurrencyFormat(cellsLRFA["J" + cellIndex.ToString()]);
                                        }
                                        cellIndex++;
                                    }
                                }

                                //cellsLRFA["A" + cellIndex.ToString()].Value = "Sub Total(LOB)";
                                //cellsLRFA["B" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "PAID LOSSES");
                                //SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                //cellsLRFA["C" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "PAID ALAE");
                                //SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                //cellsLRFA["D" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "RESERVE LOSSES");
                                //SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                //cellsLRFA["E" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "RESERVE ALAE");
                                //SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                //cellsLRFA["G" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "LBA RESULT");
                                //SetCurrencyFormat(cellsLRFA["G" + cellIndex.ToString()]);
                                //cellsLRFA["I" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "LCF RESULT");
                                //SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                                //cellsLRFA["K" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "LDF RESULT");
                                //SetCurrencyFormat(cellsLRFA["K" + cellIndex.ToString()]);
                                //cellsLRFA["L" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetails[j], "TOTAL AMOUNT");
                                //SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);

                                cellIndex = cellIndex + 1;
                            //}

                                //MergeAndFormatCellsWithoutColor(cellsLRFA, "I", ":J", "Loss Based Assessments", SSG.HAlign.Left, cellIndex);
                                //cellsLRFA["K" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "LBA_RESULT");
                                //SetCurrencyFormat(cellsLRFA["K" + cellIndex.ToString()]);
                               

                                //cellIndex++;
                                if (GetLDF_IBNR_Incl_Limit == 1)
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "J", ":K", "Claim Handling Fees", SSG.HAlign.Left, cellIndex);
                                    cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CLAIM HANDLING FEES");
                                    SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "H", ":I", "Claim Handling Fees", SSG.HAlign.Left, cellIndex);
                                    cellsLRFA["J" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CLAIM HANDLING FEES");
                                    SetCurrencyFormat(cellsLRFA["J" + cellIndex.ToString()]);
                                }
                            cellIndex++;

                            if (GetLDF_IBNR_Incl_Limit == 1)
                            {
                                FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":L" + cellIndex.ToString()]);
                            }
                            else
                            {
                                FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":J" + cellIndex.ToString()]);
                            }

                            cellsLRFA["A" + cellIndex.ToString()].Value = "Summary :";
                            cellsLRFA["B" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "Limited_Incurred_Loss");
                            SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                            cellsLRFA["C" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "Limited_Incurred_ALAE");
                            SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                            cellsLRFA["D" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "Total_Limited_Losses");
                            SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                            cellsLRFA["F" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "LCF_RESULT");
                            SetCurrencyFormat(cellsLRFA["F" + cellIndex.ToString()]);
                            cellsLRFA["G" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "LBA_RESULT");
                            SetCurrencyFormat(cellsLRFA["G" + cellIndex.ToString()]);

                            if (GetLDF_IBNR_Incl_Limit == 1)
                            {
                                cellsLRFA["H" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "Incurred_Limited_Developed_Loss");
                                SetCurrencyFormat(cellsLRFA["H" + cellIndex.ToString()]);
                                cellsLRFA["I" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "Incurred_Limited_Developed_ALAE");
                                SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                                cellsLRFA["K" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "LDF_RESULT");
                                SetCurrencyFormat(cellsLRFA["K" + cellIndex.ToString()]);
                                decimal CHFValue = decimal.Parse(CheckNullOrEmptyReturnValue(result.Rows[0], "CLAIM HANDLING FEES"));
                                cellsLRFA["L" + cellIndex.ToString()].Value = Convert.ToInt32(GetSumForDecimalField(policyDetailsExternal, "TOTAL_AMOUNT_OF_TLL_LDF_LCF")) + Convert.ToInt32(CHFValue); //+GetSumForDecimalField(policyDetailsExternal, "LBA_RESULT") + CheckNullOrEmptyReturnValue(result.Rows[0], "CLAIM HANDLING FEES");
                                SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                            }
                            else
                            {
                                cellsLRFA["I" + cellIndex.ToString()].Value = GetSumForDecimalField(policyDetailsExternal, "LDF_RESULT");
                                SetCurrencyFormat(cellsLRFA["I" + cellIndex.ToString()]);
                                decimal CHFValue = decimal.Parse(CheckNullOrEmptyReturnValue(result.Rows[0], "CLAIM HANDLING FEES"));
                                cellsLRFA["J" + cellIndex.ToString()].Value = Convert.ToInt32(GetSumForDecimalField(policyDetailsExternal, "TOTAL_AMOUNT_OF_TLL_LDF_LCF")) + Convert.ToInt32(CHFValue); //+GetSumForDecimalField(policyDetailsExternal, "LBA_RESULT") + CheckNullOrEmptyReturnValue(result.Rows[0], "CLAIM HANDLING FEES");
                                SetCurrencyFormat(cellsLRFA["J" + cellIndex.ToString()]);
                            }

                            
                            //cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CURRENT LOSS REIMBURSEMENT FUND");
                            //SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                            cellIndex = cellIndex + 2;

                            

                            //MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "Summary :", SSG.HAlign.Left, cellIndex);
                            //cellIndex++;

                            MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":B", "Notes:", SSG.HAlign.Left, cellIndex);


                            if (GetLDF_IBNR_Incl_Limit == 1)
                            {
                                MergeAndFormatCellsWithoutColor(cellsLRFA, "I", ":K", "Current Loss Reimbursement Fund Required :", SSG.HAlign.Left, cellIndex);
                                cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CURRENT LOSS REIMBURSEMENT FUND");
                                SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                            }
                            else
                            {

                                MergeAndFormatCellsWithoutColor(cellsLRFA, "H", ":I", "Current Loss Reimbursement Fund Required :", SSG.HAlign.Left, cellIndex);
                                cellsLRFA["J" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CURRENT LOSS REIMBURSEMENT FUND");
                                SetCurrencyFormat(cellsLRFA["J" + cellIndex.ToString()]);
                            }
                            cellIndex++;

                            int CURRENT_LOSS_REIMBURSEMENT_FUN = Convert.ToInt32(CheckNullOrEmptyReturnValue(result.Rows[0], "CURRENT LOSS REIMBURSEMENT FUND").Split('.')[0]);

                            int TOT_LIM = Convert.ToInt32(CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_LIM").Split('.')[0]);

                            string LDFORIBNR= CheckNullOrEmptyReturnValue(dtLRFA.Rows[0], "LDF/IBNR");

                            
                            if (CURRENT_LOSS_REIMBURSEMENT_FUN != TOT_LIM)
                            {
                                MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "If program is paid, only Paid Loss and ALAE are considered.", SSG.HAlign.Left, cellIndex);

                                if (GetLDF_IBNR_Incl_Limit == 1)
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "I", ":K", "Limited Loss Reimbursement Fund Required :", SSG.HAlign.Left, cellIndex);
                                    cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_LIM");
                                    SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                                }
                                else
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "H", ":I", "Limited Loss Reimbursement Fund Required :", SSG.HAlign.Left, cellIndex);
                                    cellsLRFA["J" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "TOT_LIM");
                                    SetCurrencyFormat(cellsLRFA["J" + cellIndex.ToString()]);
                                }
                                
                                cellIndex++;
                            }

                            if (CURRENT_LOSS_REIMBURSEMENT_FUN == TOT_LIM)
                            {
                                MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "If program is paid, only Paid Loss and ALAE are considered.", SSG.HAlign.Left, cellIndex);
                            }
                            else
                            {
                                if (GetLDF_IBNR_Incl_Limit == 1 && LDFORIBNR == "LDF")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "LDF is included within the deductible limit - no claims developed past the deductible.", SSG.HAlign.Left, cellIndex);
                                }
                                else if (GetLDF_IBNR_Incl_Limit == 1 && LDFORIBNR == "IBNR")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "IBNR is included within the deductible limit - no claims developed past the deductible.", SSG.HAlign.Left, cellIndex);
                                }
                            }
                            if (GetLDF_IBNR_Incl_Limit == 1)
                            {
                                MergeAndFormatCellsWithoutColor(cellsLRFA, "I", ":K", "Loss Reimbursement Fund Previously Required :", SSG.HAlign.Left, cellIndex);
                                cellsLRFA["L" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LOSS REIMBUR FUND PREV");
                                SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                            }
                            else
                            {
                                MergeAndFormatCellsWithoutColor(cellsLRFA, "H", ":I", "Loss Reimbursement Fund Previously Required :", SSG.HAlign.Left, cellIndex);
                                cellsLRFA["J" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "LOSS REIMBUR FUND PREV");
                                SetCurrencyFormat(cellsLRFA["J" + cellIndex.ToString()]);
                            }
                            
                            cellIndex++;
                            if (CURRENT_LOSS_REIMBURSEMENT_FUN == TOT_LIM)
                            {
                                if (GetLDF_IBNR_Incl_Limit == 1 && LDFORIBNR == "LDF")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "LDF is included within the deductible limit - no claims developed past the deductible.", SSG.HAlign.Left, cellIndex);
                                }
                                else if (GetLDF_IBNR_Incl_Limit == 1 && LDFORIBNR == "IBNR")
                                {
                                    MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":G", "IBNR is included within the deductible limit - no claims developed past the deductible.", SSG.HAlign.Left, cellIndex);
                                }
                            }

                            if (GetLDF_IBNR_Incl_Limit == 1)
                            {
                                MergeAndFormatCellsWithoutColor(cellsLRFA, "I", ":K", "Amount Due to Zurich (Due Insured) :", SSG.HAlign.Left, cellIndex);
                                decimal totLIM = !string.IsNullOrEmpty(Convert.ToString(result.Rows[0]["TOT_LIM"])) ? Convert.ToDecimal(result.Rows[0]["TOT_LIM"]) : 0;
                                decimal lrfPrev = !string.IsNullOrEmpty(Convert.ToString(result.Rows[0]["LOSS REIMBUR FUND PREV"])) ? Convert.ToDecimal(result.Rows[0]["LOSS REIMBUR FUND PREV"]) : 0;
                                decimal curResult = totLIM - lrfPrev;
                                cellsLRFA["L" + cellIndex.ToString()].Value = Convert.ToString(curResult);
                                SetCurrencyFormat(cellsLRFA["L" + cellIndex.ToString()]);
                            }
                            else
                            {
                                MergeAndFormatCellsWithoutColor(cellsLRFA, "H", ":I", "Amount Due to Zurich (Due Insured) :", SSG.HAlign.Left, cellIndex);
                                decimal totLIM = !string.IsNullOrEmpty(Convert.ToString(result.Rows[0]["TOT_LIM"])) ? Convert.ToDecimal(result.Rows[0]["TOT_LIM"]) : 0;
                                decimal lrfPrev = !string.IsNullOrEmpty(Convert.ToString(result.Rows[0]["LOSS REIMBUR FUND PREV"])) ? Convert.ToDecimal(result.Rows[0]["LOSS REIMBUR FUND PREV"]) : 0;
                                decimal curResult = totLIM - lrfPrev;
                                cellsLRFA["J" + cellIndex.ToString()].Value = Convert.ToString(curResult);
                                SetCurrencyFormat(cellsLRFA["J" + cellIndex.ToString()]);
                            }
                            cellIndex=cellIndex+2;

                            MergeAndFormatCellsWithoutColor(cellsLRFA, "A", ":B", "Loss Detail:", SSG.HAlign.Left, cellIndex);
                            cellIndex++;

                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);

                            cellsLRFA["A" + cellIndex.ToString()].Value = "LOB";
                            cellsLRFA["B" + cellIndex.ToString()].Value = "Paid Loss (1)";
                            cellsLRFA["C" + cellIndex.ToString()].Value = "Paid ALAE (2)";
                            cellsLRFA["D" + cellIndex.ToString()].Value = "Reserve Loss (3)";
                            cellsLRFA["E" + cellIndex.ToString()].Value = "Reserve ALAE (4)";

                            cellIndex++;

                            for (int j = 0; j < policyDetailsLob.Count; j++)
                            {
                                if (policyDetailsLob[j].Rows.Count == 1)
                                {
                                    foreach (DataRow drRows in policyDetailsLob[j].Rows)
                                    {
                                        cellsLRFA["A" + cellIndex.ToString()].Value = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "LOB"));
                                        cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "PAID_LOSSES"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                        SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                        cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "PAID_ALAE");
                                        SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                        cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "RESERVE_LOSSES");
                                        SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                        cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "RESERVE_ALAE");
                                        SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                        cellIndex++;
                                    }
                                }
                                else
                                {
                                    string LOB = string.Empty;
                                    decimal PL = 0;
                                    decimal PA = 0;
                                    decimal RL = 0;
                                    decimal RA = 0;
                                    foreach (DataRow drRows in policyDetailsLob[j].Rows)
                                    {
                                        if (LOB == string.Empty || LOB == "")
                                        {
                                            LOB = Convert.ToString(CheckNullOrEmptyReturnValue(drRows, "LOB"));
                                            PL = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "PAID_LOSSES"));
                                            PA = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "PAID_ALAE"));
                                            RL = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "RESERVE_LOSSES"));
                                            RA = Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "RESERVE_ALAE"));
                                        }
                                        else
                                        {
                                            PL = PL + Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "PAID_LOSSES"));
                                            PA = PA + Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "PAID_ALAE"));
                                            RL = RL + Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "RESERVE_LOSSES"));
                                            RA = RA + Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "RESERVE_ALAE"));

                                        }
                                    }
                                    cellsLRFA["A" + cellIndex.ToString()].Value = Convert.ToString(LOB);
                                    cellsLRFA["B" + cellIndex.ToString()].Value = PL; //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                                    SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                                    cellsLRFA["C" + cellIndex.ToString()].Value = PA;
                                    SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                                    cellsLRFA["D" + cellIndex.ToString()].Value = RL;
                                    SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);
                                    cellsLRFA["E" + cellIndex.ToString()].Value = RA;
                                    SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);
                                    cellIndex++;

                                }
                            }

                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);

                            cellsLRFA["A" + cellIndex.ToString()].Value = "Minus Total Excess Losses";
                            cellsLRFA["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0],"PaidLossessExhibit"); //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                            SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);
                            cellsLRFA["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0], "PaidALAEExhibit");
                            SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);
                            cellsLRFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0], "ReserveLossesExhibit");
                            SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);

                            cellsLRFA["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0], "ReserveALAEExhibit");
                            SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);

                            cellIndex++;

                            FormatHeaderTypeCell(cellsLRFA["A" + cellIndex.ToString()]);

                            cellsLRFA["A" + cellIndex.ToString()].Value = "Total Limited Losses";

                            string PaidLossessExhibit = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0], "PaidLossessExhibit");

                            decimal PAIDLOSSEXHIBITCONVET = decimal.Parse(PaidLossessExhibit);


                            cellsLRFA["B" + cellIndex.ToString()].Value = Convert.ToInt32(GetSumForDecimalField(policyDetailsExternal, "PAID_LOSSES")) - PAIDLOSSEXHIBITCONVET; //CheckNullOrEmptyReturnValue(policyDetailsLob[j].Columns["Limited_Incurred_Loss"]);
                            SetCurrencyFormat(cellsLRFA["B" + cellIndex.ToString()]);

                            string PaidALAEExhibit = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0], "PaidALAEExhibit");

                            decimal PaidALAEExhibitCONVERT = decimal.Parse(PaidALAEExhibit);


                            cellsLRFA["C" + cellIndex.ToString()].Value = Convert.ToInt32(GetSumForDecimalField(policyDetailsExternal, "PAID_ALAE")) - PaidALAEExhibitCONVERT;
                            SetCurrencyFormat(cellsLRFA["C" + cellIndex.ToString()]);

                            string ReserveLossesExhibit = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0], "ReserveLossesExhibit");

                            decimal ReserveLossesExhibitCONVERT = decimal.Parse(ReserveLossesExhibit);

                            cellsLRFA["D" + cellIndex.ToString()].Value = Convert.ToInt32(GetSumForDecimalField(policyDetailsExternal, "RESERVE_LOSSES")) - ReserveLossesExhibitCONVERT;
                            SetCurrencyFormat(cellsLRFA["D" + cellIndex.ToString()]);

                            string ReserveALAEExhibit = CheckNullOrEmptyReturnValue(ExcessLossExhibitExternal.Rows[0], "ReserveALAEExhibit");

                            decimal ReserveALAEExhibitCONVERT = decimal.Parse(ReserveALAEExhibit);

                            cellsLRFA["E" + cellIndex.ToString()].Value = Convert.ToInt32(GetSumForDecimalField(policyDetailsExternal, "RESERVE_ALAE")) - ReserveALAEExhibitCONVERT;
                            SetCurrencyFormat(cellsLRFA["E" + cellIndex.ToString()]);

                            cellIndex=cellIndex+1;
                           

                        }

                        worksheetLRFA.UsedRange.Columns.AutoFit();
                        for (int col = 0; col < worksheetLRFA.UsedRange.ColumnCount; col++)
                        {
                            worksheetLRFA.Cells[1, col].ColumnWidth *= 1.15;
                        }
                    }
                }

                #endregion

                #region Sheet:State Sales Service Tax

                DataTable dtTexasTax = (new ProgramPeriodsBS()).GetTexasTaxExternalReport(AdjNo, 1, false, 0);
                DataTable dtTexasTaxInternal = null;
                if (extIntFlag == 1)
                {
                    dtTexasTaxInternal = (new ProgramPeriodsBS()).GetTexasTaxInternalReport(AdjNo, 1, false, 0);
                }

                if (dtTexasTax != null && dtTexasTax.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetTexasTax = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetTexasTax.Name = "State Sales Service Tax";

                    SSG.IRange cellsTexasTax = worksheetTexasTax.Cells;
                    DataRow dr = dtTexasTax.Rows[0];
                    cellIndex = 1;

                    // Add column headers. 
                    cellsTexasTax["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsTexasTax["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsTexasTax, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsTexasTax["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsTexasTax["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsTexasTax, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 2;

                    FormatHeaderTypeCell(cellsTexasTax["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                    cellsTexasTax["A" + cellIndex.ToString()].Value = "Program Period";
                    cellsTexasTax["B" + cellIndex.ToString()].Value = "Adjustment Number";
                    cellsTexasTax["C" + cellIndex.ToString()].Value = "Loss Type";
                    cellsTexasTax["D" + cellIndex.ToString()].Value = "Loss Val Date";
                    cellsTexasTax["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                    cellsTexasTax["F" + cellIndex.ToString()].Value = "Adjustment Date";
                    cellIndex++;

                    List<DataTable> programPeriods1 = dtTexasTax.AsEnumerable()
                                          .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();
                    Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                    //Sorting fix
                    List<DataTable> programPeriods = new List<DataTable>();
                    foreach (var item in prgPrdList)
                    {
                        DataTable dtPrgPrd = (from dts in dtTexasTax.AsEnumerable()
                                              where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                              select dts).CopyToDataTable();
                        programPeriods.Add(dtPrgPrd);
                    }

                    for (int i = 0; i < programPeriods.Count; i++)
                    {
                        DataTable detail = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsTexasTax["A" + cellIndex.ToString()].Value = prgPrdAndType;
                        cellsTexasTax["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                        cellsTexasTax["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                        cellsTexasTax["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                        cellsTexasTax["E" + cellIndex.ToString()].Value = strInvNo;
                        cellsTexasTax["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();

                        cellIndex++;
                    }

                    cellIndex++;

                    for (int i = 0; i < programPeriods.Count; i++)
                    {

                        DataTable result = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsTexasTax["A" + cellIndex.ToString()].Value = "Program Period:";
                        FormatHeaderTypeCell(cellsTexasTax["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsTexasTax, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                        cellIndex++;
                        cellsTexasTax["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                        FormatHeaderTypeCell(cellsTexasTax["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsTexasTax, "B", ":G", Convert.ToString(result.Rows[0]["POLICIES"]), SSG.HAlign.Left, cellIndex);
                        cellIndex++;

                        //Split tables as per State under a programperiod
                        List<DataTable> policyDetails = result.AsEnumerable()
                                           .OrderBy(ord => ord.Field<string>("STATE"))
                                           .GroupBy(row => row.Field<string>("STATE"))
                                           .Select(g => g.CopyToDataTable())
                                           .ToList();

                        for (int j = 0; j < policyDetails.Count; j++)
                        {

                            FormatHeaderTypeCell(cellsTexasTax["A" + cellIndex.ToString() + ":G" + cellIndex.ToString()]);

                            cellsTexasTax["A" + cellIndex.ToString()].Value = "State";
                            cellsTexasTax["B" + cellIndex.ToString()].Value = "LOB";
                            cellsTexasTax["C" + cellIndex.ToString()].Value = "Description";
                            cellsTexasTax["D" + cellIndex.ToString()].Value = "Tax Component";
                            cellsTexasTax["E" + cellIndex.ToString()].Value = "Tax Base";
                            cellsTexasTax["F" + cellIndex.ToString()].Value = "Tax Rate";
                            cellsTexasTax["G" + cellIndex.ToString()].Value = "Tax Additional/Return";
                            cellIndex++;

                            foreach (DataRow drRows in policyDetails[j].Rows)
                            {
                                bool isDisplayA = false;
                                bool isDisplayB = false;
                                if (Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "IND STATE")) != 0)
                                {
                                    if (Convert.ToString(drRows["INDICATOR"]) != "1")
                                    {
                                        if (Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "TAX COMPONENT AMT")) != 0)
                                        {
                                            if (Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "TAX RATE")) != 0)
                                            {
                                                isDisplayA = true;
                                            }
                                        }
                                    }
                                    if (Convert.ToString(drRows["INDICATOR"]) == "1")
                                    {
                                        isDisplayB = true;
                                    }
                                }
                                if (isDisplayA)
                                {
                                    cellsTexasTax["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["STATE"]);
                                    cellsTexasTax["B" + cellIndex.ToString()].Value = Convert.ToString(drRows["LOB"]);
                                    cellsTexasTax["C" + cellIndex.ToString()].Value = Convert.ToString(drRows["DESCRIPTION"]);
                                    cellsTexasTax["D" + cellIndex.ToString()].Value = Convert.ToString(drRows["TAX COMPONENT"]);
                                    cellsTexasTax["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TAX COMPONENT AMT");
                                    SetCurrencyFormat(cellsTexasTax["E" + cellIndex.ToString()]);
                                    cellsTexasTax["F" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TAX RATE");
                                    cellsTexasTax["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL ADDIT RETURN");
                                    SetCurrencyFormat(cellsTexasTax["G" + cellIndex.ToString()]);
                                    cellIndex++;
                                }
                                else if (isDisplayB)
                                {
                                    cellsTexasTax["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["STATE"]);
                                    cellsTexasTax["B" + cellIndex.ToString()].Value = Convert.ToString(drRows["LOB"]);
                                    cellsTexasTax["C" + cellIndex.ToString()].Value = Convert.ToString(drRows["DESCRIPTION"]);
                                    cellsTexasTax["D" + cellIndex.ToString()].Value = Convert.ToString(drRows["TAX COMPONENT"]);
                                    MergeAndFormatCellsWithoutColor(cellsTexasTax, "E", ":F", "See Claim Exhibit for Claims Taxed", SSG.HAlign.Left, cellIndex, false);
                                    cellsTexasTax["G" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL ADDIT RETURN");
                                    SetCurrencyFormat(cellsTexasTax["G" + cellIndex.ToString()]);
                                    cellIndex++;
                                }


                            }

                            string state = Convert.ToString(policyDetails[j].Rows[0]["STATE"]);
                            decimal sumForState = GetSumForDecimalField(policyDetails[j], "TOTAL ADDIT RETURN");

                            cellsTexasTax["A" + cellIndex.ToString()].Value = "Total Tax for " + state;
                            cellsTexasTax["G" + cellIndex.ToString()].Value = Convert.ToString(sumForState);
                            SetCurrencyFormat(cellsTexasTax["G" + cellIndex.ToString()]);
                            cellIndex = cellIndex + 2;
                        }

                        decimal sumForProgPeriod = GetSumForDecimalField(result, "TOTAL ADDIT RETURN");

                        decimal prevAmt = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "PREV REQ AMT"));
                        decimal amtBilled = sumForProgPeriod - prevAmt;

                        cellsTexasTax["A" + cellIndex.ToString()].Value = "Total Tax";
                        cellsTexasTax["G" + cellIndex.ToString()].Value = Convert.ToString(sumForProgPeriod);
                        SetCurrencyFormat(cellsTexasTax["G" + cellIndex.ToString()]);
                        cellIndex = cellIndex + 2;

                        MergeAndFormatCellsWithoutColor(cellsTexasTax, "A", ":E", "Summary :", SSG.HAlign.Left, cellIndex);
                        cellIndex++;
                        MergeAndFormatCellsWithoutColor(cellsTexasTax, "A", ":E", "Current Tax Required :", SSG.HAlign.Left, cellIndex);
                        cellsTexasTax["F" + cellIndex.ToString()].Value = Convert.ToString(sumForProgPeriod);
                        SetCurrencyFormat(cellsTexasTax["F" + cellIndex.ToString()]);
                        cellIndex++;
                        MergeAndFormatCellsWithoutColor(cellsTexasTax, "A", ":E", "Tax Previously Required :", SSG.HAlign.Left, cellIndex);
                        cellsTexasTax["F" + cellIndex.ToString()].Value = Convert.ToString(prevAmt);
                        SetCurrencyFormat(cellsTexasTax["F" + cellIndex.ToString()]);
                        cellIndex++;
                        MergeAndFormatCellsWithoutColor(cellsTexasTax, "A", ":E", "Amount Billed (Due Insured):", SSG.HAlign.Left, cellIndex);
                        cellsTexasTax["F" + cellIndex.ToString()].Value = Convert.ToString(amtBilled);
                        SetCurrencyFormat(cellsTexasTax["F" + cellIndex.ToString()]);
                        cellIndex = cellIndex + 2;

                        //This section only available to internal spread sheet
                        if (extIntFlag == 1)
                        {
                            DataTable resultInternal = (from items in dtTexasTaxInternal.AsEnumerable()
                                                        where items.Field<int>("PREM ADJ PGM ID") == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])
                                                        select items).CopyToDataTable();

                            FormatHeaderTypeCell(cellsTexasTax["A" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);

                            cellsTexasTax["A" + cellIndex.ToString()].Value = "Tax Type";
                            cellsTexasTax["B" + cellIndex.ToString()].Value = "Current";
                            cellsTexasTax["C" + cellIndex.ToString()].Value = "Prior Year";
                            cellsTexasTax["D" + cellIndex.ToString()].Value = "Adj. Prior Year";
                            cellsTexasTax["E" + cellIndex.ToString()].Value = "Posting";
                            cellIndex++;

                            foreach (DataRow drRows in resultInternal.Rows)
                            {
                                //{GetTexasTaxInternal;1.CURRENT AMT} = 0 AND {GetTexasTaxInternal;1.PRIOR AMT} = 0 AND {GetTexasTaxInternal;1.ADJ PRIOR AMT} = 0 then 
                                if (Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "CURRENT AMT")) != 0
                                    || Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "PRIOR AMT")) != 0
                                    || Convert.ToDecimal(CheckNullOrEmptyReturnValue(drRows, "ADJ PRIOR AMT")) != 0
                                    )
                                {
                                    cellsTexasTax["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["TAX TYPE"]);
                                    cellsTexasTax["B" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "CURRENT AMT");
                                    SetCurrencyFormat(cellsTexasTax["B" + cellIndex.ToString()]);
                                    cellsTexasTax["C" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "PRIOR AMT");
                                    SetCurrencyFormat(cellsTexasTax["C" + cellIndex.ToString()]);
                                    cellsTexasTax["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "ADJ PRIOR AMT");
                                    SetCurrencyFormat(cellsTexasTax["D" + cellIndex.ToString()]);
                                    cellsTexasTax["E" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "POST AMT");
                                    SetCurrencyFormat(cellsTexasTax["E" + cellIndex.ToString()]);
                                    cellIndex++;
                                }
                            }
                            cellsTexasTax["A" + cellIndex.ToString()].Value = "Tax Total";
                            cellsTexasTax["B" + cellIndex.ToString()].Value = GetSumForDecimalField(resultInternal, "CURRENT AMT");
                            SetCurrencyFormat(cellsTexasTax["B" + cellIndex.ToString()]);
                            cellsTexasTax["C" + cellIndex.ToString()].Value = GetSumForDecimalField(resultInternal, "PRIOR AMT");
                            SetCurrencyFormat(cellsTexasTax["C" + cellIndex.ToString()]);
                            cellsTexasTax["D" + cellIndex.ToString()].Value = GetSumForDecimalField(resultInternal, "ADJ PRIOR AMT");
                            SetCurrencyFormat(cellsTexasTax["D" + cellIndex.ToString()]);
                            cellsTexasTax["E" + cellIndex.ToString()].Value = GetSumForDecimalField(resultInternal, "POST AMT");
                            SetCurrencyFormat(cellsTexasTax["E" + cellIndex.ToString()]);
                            cellIndex = cellIndex + 2;
                        }
                    }

                    worksheetTexasTax.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetTexasTax.UsedRange.ColumnCount; col++)
                    {
                        worksheetTexasTax.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:Loss Fund Adjustment

                DataTable dtLFA = (new ProgramPeriodsBS()).GetLossFundAdjustmentReport(AdjNo, 1, false);

                if (dtLFA != null && dtLFA.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetLFA = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetLFA.Name = "Loss Fund Adjustment";

                    SSG.IRange cellsLFA = worksheetLFA.Cells;
                    DataRow dr = dtLFA.Rows[0];
                    cellIndex = 1;

                    // Add column headers. 
                    cellsLFA["A" + cellIndex.ToString()].Value = "Insured Name:";
                    FormatHeaderTypeCell(cellsLFA["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsLFA, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellsLFA["D" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsLFA["D" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsLFA, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 2;

                    FormatHeaderTypeCell(cellsLFA["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                    cellsLFA["A" + cellIndex.ToString()].Value = "Program Period";
                    cellsLFA["B" + cellIndex.ToString()].Value = "Adjustment Number";
                    cellsLFA["C" + cellIndex.ToString()].Value = "Loss Type";
                    cellsLFA["D" + cellIndex.ToString()].Value = "Loss Val Date";
                    cellsLFA["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                    cellsLFA["F" + cellIndex.ToString()].Value = "Adjustment Date";
                    cellIndex++;

                    List<DataTable> programPeriods1 = dtLFA.AsEnumerable()
                                          .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                         .Select(g => g.CopyToDataTable())
                                         .ToList();
                    Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                    //Sorting fix
                    List<DataTable> programPeriods = new List<DataTable>();
                    foreach (var item in prgPrdList)
                    {
                        DataTable dtPrgPrd = (from dts in dtLFA.AsEnumerable()
                                              where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                              select dts).CopyToDataTable();
                        programPeriods.Add(dtPrgPrd);
                    }

                    for (int i = 0; i < programPeriods.Count; i++)
                    {
                        DataTable detail = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsLFA["A" + cellIndex.ToString()].Value = prgPrdAndType;
                        cellsLFA["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                        cellsLFA["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                        cellsLFA["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                        cellsLFA["E" + cellIndex.ToString()].Value = strInvNo;
                        cellsLFA["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();

                        cellIndex++;
                    }

                    cellIndex++;

                    for (int i = 0; i < programPeriods.Count; i++)
                    {

                        DataTable result = programPeriods[i];
                        var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                        cellsLFA["A" + cellIndex.ToString()].Value = "Program Period:";
                        FormatHeaderTypeCell(cellsLFA["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsLFA, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                        cellIndex++;
                        cellsLFA["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                        FormatHeaderTypeCell(cellsLFA["A" + cellIndex.ToString()]);
                        MergeAndFormatCellsWithoutColor(cellsLFA, "B", ":G", Convert.ToString(result.Rows[0]["POLICIES"]), SSG.HAlign.Left, cellIndex);
                        cellIndex++;

                        FormatHeaderTypeCell(cellsLFA["A" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);

                        string monthPrior = CheckNullOrEmptyReturnValue(result.Rows[0], "MONTHS PRIOR");
                        string billDivision = CheckNullOrEmptyReturnValue(result.Rows[0], "DIVISION");
                        string monthHeldFac = CheckNullOrEmptyReturnValue(result.Rows[0], "MONTHS HELD FACTOR");
                        cellsLFA["A" + cellIndex.ToString()].Value = "Paid Loss Billings " + monthPrior + " Months Prior";
                        cellsLFA["B" + cellIndex.ToString()].Value = "Paid Loss Billings / " + billDivision;
                        cellsLFA["C" + cellIndex.ToString()].Value = "Paid Loss Billings / " + monthPrior + " * " + monthHeldFac;
                        cellsLFA["D" + cellIndex.ToString()].Value = "Adjusted Loss Fund";
                        cellIndex++;

                        //                        //paid division
                        //                        IF {GetEscrow;1.DIVISION} = 0 THEN 0
                        //ELSE 
                        //({GetEscrow;1.PAID LOSS BILLING}/{GetEscrow;1.DIVISION})

                        //                    //paid multiply
                        //                    IF {GetEscrow;1.DIVISION} = 0 OR {GetEscrow;1.MONTHS HELD FACTOR} = 0 OR {GetEscrow;1.PAID LOSS BILLING} = 0 THEN
                        //0
                        //ELSE 
                        //({GetEscrow;1.PAID LOSS BILLING}/{GetEscrow;1.DIVISION})*{GetEscrow;1.MONTHS HELD FACTOR}

                        decimal paidLoss = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "PAID LOSS BILLING"));
                        decimal bDivision = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "DIVISION"));
                        decimal mhFactor = Convert.ToDecimal(CheckNullOrEmptyReturnValue(result.Rows[0], "MONTHS HELD FACTOR"));
                        string paidDivision = string.Empty;
                        string paidMultiply = string.Empty;
                        if (paidLoss == 0)
                        {
                            paidDivision = "0";
                        }
                        else
                        {
                            paidDivision = Convert.ToString(paidLoss / bDivision);
                        }

                        if (paidLoss == 0 || bDivision == 0 || mhFactor == 0)
                        {
                            paidMultiply = "0";
                        }
                        else
                        {
                            paidMultiply = Convert.ToString((paidLoss / bDivision) * mhFactor);
                        }

                        cellsLFA["A" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "PAID LOSS BILLING");
                        SetCurrencyFormat(cellsLFA["A" + cellIndex.ToString()]);
                        cellsLFA["B" + cellIndex.ToString()].Value = paidDivision;
                        SetCurrencyFormat(cellsLFA["B" + cellIndex.ToString()]);
                        cellsLFA["C" + cellIndex.ToString()].Value = paidMultiply;
                        SetCurrencyFormat(cellsLFA["C" + cellIndex.ToString()]);
                        cellsLFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "ADJUSTED ESCROW FUND");
                        SetCurrencyFormat(cellsLFA["D" + cellIndex.ToString()]);
                        cellIndex = cellIndex + 2;

                        MergeAndFormatCellsWithoutColor(cellsLFA, "A", ":D", "Summary :", SSG.HAlign.Left, cellIndex);
                        cellIndex++;
                        MergeAndFormatCellsWithoutColor(cellsLFA, "A", ":C", "Current Loss Fund Required :", SSG.HAlign.Left, cellIndex);
                        cellsLFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "CURRENT ESCROW FUND");
                        SetCurrencyFormat(cellsLFA["D" + cellIndex.ToString()]);
                        cellIndex++;
                        MergeAndFormatCellsWithoutColor(cellsLFA, "A", ":C", "Loss Fund Previously Required :", SSG.HAlign.Left, cellIndex);
                        cellsLFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "ESCR FUND PREV REQ");
                        SetCurrencyFormat(cellsLFA["D" + cellIndex.ToString()]);
                        cellIndex++;
                        MergeAndFormatCellsWithoutColor(cellsLFA, "A", ":C", "Amount Billed (Due Insured) :", SSG.HAlign.Left, cellIndex);
                        cellsLFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "AMOUNT BILLED");
                        SetCurrencyFormat(cellsLFA["D" + cellIndex.ToString()]);
                        cellIndex++;
                        MergeAndFormatCellsWithoutColor(cellsLFA, "A", ":C", "Invoice Amount :", SSG.HAlign.Left, cellIndex);
                        cellsLFA["D" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(result.Rows[0], "INVOICED AMT");
                        SetCurrencyFormat(cellsLFA["D" + cellIndex.ToString()]);
                        cellIndex = cellIndex + 2;
                    }

                    worksheetLFA.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetLFA.UsedRange.ColumnCount; col++)
                    {
                        worksheetLFA.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:Cesar Coding Worksheet
                //Only for internal spread sheet.
                if (extIntFlag == 1)
                {
                    DataTable dtCCWMain = (new ProgramPeriodsBS()).GetCesarCodingReport(AdjNo, 1, false);

                    if (dtCCWMain != null && dtCCWMain.Rows.Count > 0)
                    {
                        List<DataTable> dtCCWSub = dtCCWMain.AsEnumerable()
                                                .GroupBy(row => row.Field<string>("ISRevised"))
                                                .Select(g => g.CopyToDataTable())
                                                .ToList();

                        for (int k = 0; k < dtCCWSub.Count; k++)
                        {
                            DataTable dtCCW = dtCCWSub[k];
                            workbook.Worksheets.Add();
                            sheetIndex++;
                            SSG.IWorksheet worksheetCCW = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                            if (Convert.ToString(dtCCW.Rows[0]["ISRevised"]) == "True")
                            {
                                worksheetCCW.Name = "Cesar Coding Worksheet - RTI";
                            }
                            else
                            {
                                worksheetCCW.Name = "Cesar Coding Worksheet";
                            }

                            SSG.IRange cellsCCW = worksheetCCW.Cells;
                            DataRow dr = dtCCW.Rows[0];
                            cellIndex = 1;

                            // Add column headers. 
                            cellsCCW["A" + cellIndex.ToString()].Value = "Insured Name:";
                            FormatHeaderTypeCell(cellsCCW["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsCCW, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                            cellsCCW["D" + cellIndex.ToString()].Value = "Broker:";
                            FormatHeaderTypeCell(cellsCCW["D" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsCCW, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                            cellsCCW["G" + cellIndex.ToString()].Value = "Co:";
                            FormatHeaderTypeCell(cellsCCW["G" + cellIndex.ToString()]);
                            cellsCCW["H" + cellIndex.ToString()].Value = Convert.ToString(dr["CompanyCode"]);
                            cellsCCW["I" + cellIndex.ToString()].Value = "CCY:";
                            FormatHeaderTypeCell(cellsCCW["I" + cellIndex.ToString()]);
                            cellsCCW["J" + cellIndex.ToString()].Value = Convert.ToString(dr["CurrencyCode"]);
                            cellIndex = cellIndex + 2;

                            FormatHeaderTypeCell(cellsCCW["A" + cellIndex.ToString() + ":F" + cellIndex.ToString()]);

                            cellsCCW["A" + cellIndex.ToString()].Value = "Program Period";
                            cellsCCW["B" + cellIndex.ToString()].Value = "Adjustment Number";
                            cellsCCW["C" + cellIndex.ToString()].Value = "Loss Type";
                            cellsCCW["D" + cellIndex.ToString()].Value = "Loss Val Date";
                            cellsCCW["E" + cellIndex.ToString()].Value = "Invoice Ref Number";
                            cellsCCW["F" + cellIndex.ToString()].Value = "Adjustment Date";
                            cellIndex++;

                            List<DataTable> programPeriods1 = dtCCW.AsEnumerable()
                                          .OrderByDescending(row => row.Field<int>("PREM ADJ PGM ID"))
                                                  .GroupBy(row => row.Field<int>("PREM ADJ PGM ID"))
                                                  .Select(g => g.CopyToDataTable())
                                                  .ToList();

                            Dictionary<int, string> prgPrdList = GetPrgPrdsWithType(programPeriods1, "PREM ADJ PGM ID");

                            //Sorting fix
                            List<DataTable> programPeriods = new List<DataTable>();
                            foreach (var item in prgPrdList)
                            {
                                DataTable dtPrgPrd = (from dts in dtCCW.AsEnumerable()
                                                      where (dts.Field<int>("PREM ADJ PGM ID") == item.Key)
                                                      select dts).CopyToDataTable();
                                programPeriods.Add(dtPrgPrd);
                            }

                            for (int i = 0; i < programPeriods.Count; i++)
                            {
                                DataTable detail = programPeriods[i];
                                var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(detail.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();
                                cellsCCW["A" + cellIndex.ToString()].Value = prgPrdAndType;
                                cellsCCW["B" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT NUMBER"]);
                                cellsCCW["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["ADJUSTMENT TYPE"]);
                                cellsCCW["D" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["VALUATION DATE"]);
                                if (Convert.ToString(detail.Rows[0]["ISRevised"]) == "True")
                                {
                                    cellsCCW["E" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["INVOICE NUMBER"]);
                                    cellsCCW["F" + cellIndex.ToString()].Value = Convert.ToString(detail.Rows[0]["INVOICE DATE"]);
                                }
                                else
                                {
                                    cellsCCW["E" + cellIndex.ToString()].Value = strInvNo;
                                    cellsCCW["F" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                                }
                                cellIndex++;
                            }

                            cellIndex++;

                            FormatHeaderTypeCell(cellsCCW["A" + cellIndex.ToString() + ":D" + cellIndex.ToString()]);

                            cellsCCW["A" + cellIndex.ToString()].Value = "State";
                            cellsCCW["B" + cellIndex.ToString()].Value = "Retro Result";
                            cellsCCW["C" + cellIndex.ToString()].Value = "Previous";
                            cellsCCW["D" + cellIndex.ToString()].Value = "Current Result";
                            cellIndex++;

                            for (int i = 0; i < programPeriods.Count; i++)
                            {
                                DataTable result = programPeriods[i];
                                var prgPrdAndType = prgPrdList.Where(p => p.Key == Convert.ToInt32(result.Rows[0]["PREM ADJ PGM ID"])).Select(p => p.Value).First();

                                cellsCCW["A" + cellIndex.ToString()].Value = "Program Period:";
                                FormatHeaderTypeCell(cellsCCW["A" + cellIndex.ToString()]);
                                MergeAndFormatCellsWithoutColor(cellsCCW, "B", ":C", prgPrdAndType, SSG.HAlign.Left, cellIndex);
                                cellIndex = cellIndex + 1;

                                //Split tables as per number of policies under a programperiod
                                List<DataTable> policyDetails = result.AsEnumerable()
                                                   .OrderBy(ord => ord.Field<string>("POLICY NUMBER"))
                                                   .GroupBy(row => row.Field<string>("POLICY NUMBER"))
                                                   .Select(g => g.CopyToDataTable())
                                                   .ToList();

                                for (int j = 0; j < policyDetails.Count; j++)
                                {
                                    DataTable result1 = policyDetails[j];
                                    string policyNumber = Convert.ToString(result1.Rows[0]["POLICY NUMBER"]);
                                    cellsCCW["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                                    FormatHeaderTypeCell(cellsCCW["A" + cellIndex.ToString()]);
                                    MergeAndFormatCellsWithoutColor(cellsCCW, "B", ":C", policyNumber, SSG.HAlign.Left, cellIndex);
                                    cellIndex++;
                                    var details1 = from item in result1.AsEnumerable()
                                                   orderby item.Field<String>("State")
                                                   group item by item.Field<String>("State") into g
                                                   select new
                                                   {
                                                       State = g.Key,
                                                       RetroResult = g.Sum(item => item.Field<decimal?>("Retro Result")),
                                                       Previous = g.Sum(item => item.Field<decimal?>("Previous")),
                                                   };

                                    foreach (var detail in details1)
                                    {
                                        cellsCCW["A" + cellIndex.ToString()].Value = Convert.ToString(detail.State);
                                        cellsCCW["B" + cellIndex.ToString()].Value = Convert.ToString(detail.RetroResult);
                                        SetCurrencyFormat(cellsCCW["B" + cellIndex.ToString()]);
                                        cellsCCW["C" + cellIndex.ToString()].Value = Convert.ToString(detail.Previous);
                                        SetCurrencyFormat(cellsCCW["C" + cellIndex.ToString()]);
                                        decimal retroResult = !string.IsNullOrEmpty(Convert.ToString(detail.RetroResult)) ? Convert.ToDecimal(detail.RetroResult) : 0;
                                        decimal prevResult = !string.IsNullOrEmpty(Convert.ToString(detail.Previous)) ? Convert.ToDecimal(detail.Previous) : 0;
                                        decimal curResult = retroResult - prevResult;
                                        cellsCCW["D" + cellIndex.ToString()].Value = Convert.ToString(curResult);
                                        SetCurrencyFormat(cellsCCW["D" + cellIndex.ToString()]);
                                        cellIndex++;
                                    }

                                    decimal retroResultTotal = GetSumForDecimalField(result1, "Retro Result");
                                    decimal previousTotal = GetSumForDecimalField(result1, "Previous");
                                    decimal currentTotal = retroResultTotal - previousTotal;

                                    cellsCCW["A" + cellIndex.ToString()].Value = "Total :";
                                    cellsCCW["B" + cellIndex.ToString()].Value = Convert.ToString(retroResultTotal);
                                    SetCurrencyFormat(cellsCCW["B" + cellIndex.ToString()]);
                                    cellsCCW["C" + cellIndex.ToString()].Value = Convert.ToString(previousTotal);
                                    SetCurrencyFormat(cellsCCW["C" + cellIndex.ToString()]);
                                    cellsCCW["D" + cellIndex.ToString()].Value = Convert.ToString(currentTotal);
                                    SetCurrencyFormat(cellsCCW["D" + cellIndex.ToString()]);
                                    cellIndex++;
                                }
                            }

                            cellIndex++;
                            worksheetCCW.UsedRange.Columns.AutoFit();
                            for (int col = 0; col < worksheetCCW.UsedRange.ColumnCount; col++)
                            {
                                worksheetCCW.Cells[1, col].ColumnWidth *= 1.15;
                            }
                        }
                    }
                }

                #endregion

                #region Sheet: ARiES Posting Details
                //Only for internal spread sheet.
                if (extIntFlag == 1)
                {
                    DataTable dtARIESMain = (new ProgramPeriodsBS()).GetARiESPostingReport(AdjNo, 1, false);

                    if (dtARIESMain != null && dtARIESMain.Rows.Count > 0)
                    {
                        List<DataTable> dtARIESSub = dtARIESMain.AsEnumerable()
                                                .GroupBy(row => row.Field<string>("ISRevised"))
                                                .Select(g => g.CopyToDataTable())
                                                .ToList();

                        for (int k = 0; k < dtARIESSub.Count; k++)
                        {
                            DataTable dtARIES = dtARIESSub[k];
                            workbook.Worksheets.Add();
                            sheetIndex++;
                            SSG.IWorksheet worksheetARIES = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                            if (Convert.ToString(dtARIES.Rows[0]["ISRevised"]) == "True")
                            {
                                worksheetARIES.Name = "ARiES Posting Details - RTI";
                            }
                            else
                            {
                                worksheetARIES.Name = "ARiES Posting Details";
                            }

                            SSG.IRange cellsARIES = worksheetARIES.Cells;
                            DataRow dr = dtARIES.Rows[0];
                            cellIndex = 1;

                            // Add column headers. 
                            cellsARIES["A" + cellIndex.ToString()].Value = "Insured Name:";
                            FormatHeaderTypeCell(cellsARIES["A" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsARIES, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                            cellsARIES["D" + cellIndex.ToString()].Value = "Broker:";
                            FormatHeaderTypeCell(cellsARIES["D" + cellIndex.ToString()]);
                            MergeAndFormatCellsWithoutColor(cellsARIES, "E", ":F", Convert.ToString(dr["BROKER"]), SSG.HAlign.Left, cellIndex);
                            cellIndex = cellIndex + 2;

                            FormatHeaderTypeCell(cellsARIES["A" + cellIndex.ToString() + ":C" + cellIndex.ToString()]);

                            cellsARIES["A" + cellIndex.ToString()].Value = "Loss Val Date";
                            cellsARIES["B" + cellIndex.ToString()].Value = "Invoice Ref Number";
                            cellsARIES["C" + cellIndex.ToString()].Value = "Adjustment Date";
                            cellIndex++;
                            cellsARIES["A" + cellIndex.ToString()].Value = Convert.ToString(dr["VALUATION DATE"]);
                            if (Convert.ToString(dr["ISRevised"]) == "True")
                            {
                                cellsARIES["B" + cellIndex.ToString()].Value = Convert.ToString(dr["INVOICE NUMBER"]);
                                cellsARIES["C" + cellIndex.ToString()].Value = Convert.ToString(dr["INVOICE DATE"]);
                            }
                            else
                            {
                                cellsARIES["B" + cellIndex.ToString()].Value = strInvNo;
                                cellsARIES["C" + cellIndex.ToString()].Value = DateTime.Today.ToShortDateString();
                            }

                            cellIndex = cellIndex + 2;

                            FormatHeaderTypeCell(cellsARIES["A" + cellIndex.ToString() + ":I" + cellIndex.ToString()]);

                            cellsARIES["A" + cellIndex.ToString()].Value = "Adjustment Type";
                            cellsARIES["B" + cellIndex.ToString()].Value = "Program Period";
                            cellsARIES["C" + cellIndex.ToString()].Value = "Main";
                            cellsARIES["D" + cellIndex.ToString()].Value = "Sub";
                            cellsARIES["E" + cellIndex.ToString()].Value = "Policy#";
                            cellsARIES["F" + cellIndex.ToString()].Value = "BP#";
                            cellsARIES["G" + cellIndex.ToString()].Value = "LSI#";
                            cellsARIES["H" + cellIndex.ToString()].Value = "Transaction Description";
                            cellsARIES["I" + cellIndex.ToString()].Value = "Total";
                            cellIndex++;

                            foreach (DataRow drRows in dtARIES.Rows)
                            {
                                cellsARIES["A" + cellIndex.ToString()].Value = Convert.ToString(drRows["ADJUSTMENT TYPE"]);
                                cellsARIES["B" + cellIndex.ToString()].Value = Convert.ToString(drRows["PROGRAM PERIOD"]);
                                cellsARIES["C" + cellIndex.ToString()].Value = Convert.ToString(drRows["Main"]);
                                cellsARIES["D" + cellIndex.ToString()].Value = Convert.ToString(drRows["Sub"]);
                                cellsARIES["E" + cellIndex.ToString()].Value = Convert.ToString(drRows["Policy#"]);
                                cellsARIES["F" + cellIndex.ToString()].Value = Convert.ToString(drRows["BP#"]);
                                cellsARIES["G" + cellIndex.ToString()].Value = Convert.ToString(drRows["LSI#"]);
                                cellsARIES["H" + cellIndex.ToString()].Value = Convert.ToString(drRows["TRANSACTION DESCRIPTION"]);
                                cellsARIES["I" + cellIndex.ToString()].Value = CheckNullOrEmptyReturnValue(drRows, "TOTAL");
                                SetCurrencyFormat(cellsARIES["I" + cellIndex.ToString()]);
                                cellIndex++;
                            }

                            MergeAndFormatCellsWithoutColor(cellsARIES, "A", ":H", "Total", SSG.HAlign.Right, cellIndex);
                            cellsARIES["I" + cellIndex.ToString()].Value = Convert.ToString(GetSumForDecimalField(dtARIES, "TOTAL"));
                            SetCurrencyFormat(cellsARIES["I" + cellIndex.ToString()]);

                            worksheetARIES.UsedRange.Columns.AutoFit();
                            for (int col = 0; col < worksheetARIES.UsedRange.ColumnCount; col++)
                            {
                                worksheetARIES.Cells[1, col].ColumnWidth *= 1.15;
                            }
                        }
                    }
                }

                #endregion

                if (sheetIndex == 0)
                {
                    workbook.Worksheets.Add();
                }

                string vcwDirectory = ConfigurationManager.AppSettings["Directory"].ToString();
                string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
                sFormat = sFormat.Replace(":", "");
                sFormat = sFormat.Replace("-", "");
                char cFlag = 'I';
                if (extIntFlag == 0)
                {
                    cFlag = 'E';
                }

                workbook.Worksheets[0].Select();
                string strFileName = vcwDirectory + "\\" + cFlag + "_" + sFormat + "_" + Request["AdjNo"] + ".xlsx";
                //EAISA-5 Veracode flaw fix 12072017
                string strFileNameDecd = Server.HtmlDecode(Server.HtmlEncode(strFileName));
                workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
                workbook.Close();

                FileInfo file = new FileInfo(strFileNameDecd);
                Response.Clear();
                Response.ContentType = "application/xlsx";
                //EAISA-5 Veracode flaw fix 12072017
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileNameDecd);
                
                Response.AddHeader("Content-Length", file.Length.ToString());
                //EAISA-5 Veracode flaw fix 12072017
                Response.TransmitFile(strFileNameDecd);
                Response.Flush();
                Response.End();  
                //EAISA - 7 fix to avoid exception  14-5-2018
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception err)
            {
                common.Logger.Error(err);
            }
        }


        /// <summary>
        /// This method is used for to generate Program Summary report in spread sheet. 
        /// Phase 3 - Adding new Exhibit -( DMR 147050 AIS Exhibit Redesign)
        /// </summary>
        /// <param name="AdjNo"></param>
        /// <param name="strInvNo"></param>      
        /// <param name="strDocumentName"></param>
         public void DownloadFileProgramSummaySpreadsheetGear(int AdjNo, string strInvNo, string strDocumentName)
        {
            try
            {
                // Create a new workbook and worksheet.
                SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
                int sheetIndex = 0;
                int cellIndex = 0;

                // int debugindex = 0; //for break point

                #region Sheet:Summary Detail

                DataTable dtAdj = (new ProgramPeriodsBS()).GetAdjReport(AdjNo, 1, false, 0);

                if (dtAdj != null && dtAdj.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetAdj = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetAdj.Name = "Summary Detail";

                    // Get the worksheet cells reference. 
                    SSG.IRange cellsAdj = worksheetAdj.Cells;
                    DataRow dr = dtAdj.Rows[0];
                    cellIndex = 1;


                    //worksheetAdj.Range["E2:E3"].Merge();

                    //string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                    //worksheetAdj.Shapes.AddPicture(path, 250, 13, 40, 30);


                    var item = (from items in dtAdj.AsEnumerable()
                                where (items.Field<Int32>("PREM ADJ ID") == AdjNo)
                                      && (!string.IsNullOrEmpty(items.Field<string>("VALUATION DATE")))
                                select
                                new { VALUATIONDATE = items.Field<string>("VALUATION DATE"), TOTALAMOUNTBILLED = items.Field<Decimal>("TOTAL AMOUNT BILLED") }
                                ).First();

                    cellsAdj["A" + cellIndex.ToString()].Value = "Insured:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["BROKER NAME"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Ref Number:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", strInvNo, SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Loss Val Date:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.VALUATIONDATE), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Date:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", DateTime.Today.ToShortDateString(), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Total Amount Billed:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.TOTALAMOUNTBILLED), SSG.HAlign.Left, cellIndex);
                    SetCurrencyFormatProgramSummaryTotalAmountBilled(cellsAdj["B" + cellIndex.ToString() + ":C" + cellIndex.ToString()]);
                    cellIndex = cellIndex + 2;

                    DataTable dtPeriodDet = (new ProgramPeriodsBS()).GetProgramSummaryPeriodDetails(AdjNo);

                    if (dtPeriodDet.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Number";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                        //worksheetAdj.Range["E2:E3"].Merge();


                        string path = Server.MapPath("~") + "\\images\\" + "zurich_logo.JPG";
                        //if (dtPeriodDet.Rows.Count <= 3)
                        //{
                        //    worksheetAdj.Shapes.AddPicture(path, 215, 13, 40, 30);
                        //}
                        //else
                        //{
                        //    worksheetAdj.Shapes.AddPicture(path, 250, 13, 40, 30);
                        //}
                        if (dtPeriodDet.Rows.Count <= 3)
                        {
                            worksheetAdj.Shapes.AddPicture(path, 215, 13, 100, 60);
                        }
                        else
                        {
                            worksheetAdj.Shapes.AddPicture(path, 250, 13, 100, 60);
                        }


                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            int AdjustNum = 0;
                            AdjustNum = dtPeriodDet.Rows[i - 1].Field<int>("adj_nbr");
                            cellsAdj[columname + cellIndex.ToString()].Value = AdjustNum.ToString();
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;
                        cellsAdj["A" + cellIndex.ToString()].Value = "Program period";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                        for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                        {
                            int columnindexvalue;
                            String columname = string.Empty;

                            if (i == 0)
                            {
                                columnindexvalue = i + 2;
                            }
                            else
                            {
                                columnindexvalue = i + 2;
                            }
                            columname = getColumnNamefromIndex(columnindexvalue);

                            string Programperiod = string.Empty;
                            Programperiod = dtPeriodDet.Rows[i].Field<string>("Program Period");

                            cellsAdj[columname + cellIndex.ToString()].Value = Programperiod.ToString();
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 2;

                    }

                    #region Sheet:RPA

                    DataTable dtProgramSummaryRetroReport = (new ProgramPeriodsBS()).GetProgramSummaryRetroDetails(AdjNo);
                    if (dtProgramSummaryRetroReport != null)
                    {
                        if (dtProgramSummaryRetroReport.Rows.Count > 0)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = "Retrospective Premium Adjustment";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);

                                cellsAdj[columname + cellIndex.ToString()].Value = "";
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;
                            cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);
                                string AdjsutmentType = string.Empty;
                                AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                                cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;


                            for (int i = 1; i <= dtProgramSummaryRetroReport.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i;
                                columname = getColumnNamefromIndex(columnindexvalue);

                                String RetroDescription = String.Empty;

                                RetroDescription = dtProgramSummaryRetroReport.Rows[i - 1].Field<string>("DESCR");
                                cellsAdj["A" + cellIndex.ToString()].Value = RetroDescription.ToString();

                                if (RetroDescription == "Adjustment Result:  AP  / (RP) Due Zurich")
                                {
                                    SetFontBold(cellsAdj["A" + cellIndex.ToString()]);
                                }

                                //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                                for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                                {
                                    String columnamePGMID = string.Empty;
                                    int columnindexvaluePGMID = J + 1;
                                    columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                    int RetroPGMID = 0;
                                    RetroPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");


                                    int Fieldpostion;
                                    if (J == 1)
                                        Fieldpostion = 3;
                                    else
                                        Fieldpostion = J + 2;

                                    decimal RetroFinalValue;
                                    RetroFinalValue = dtProgramSummaryRetroReport.Rows[i - 1].Field<decimal>(Fieldpostion);
                                    cellsAdj[columnamePGMID + cellIndex.ToString()].Value = RetroFinalValue.ToString();
                                    SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                    if (RetroDescription == "Adjustment Result:  AP  / (RP) Due Zurich")
                                    {
                                        SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);
                                    }
                                }

                                cellIndex = cellIndex + 1;

                            }

                        }
                        cellIndex = cellIndex + 2;
                    }

                    #endregion

                    #region Sheet:ILRF

                    DataTable dtLRFA = (new ProgramPeriodsBS()).GetProgramSummaryILRFDetails(AdjNo, 1, false, 0);


                    if (dtLRFA != null)
                    {
                        if (dtLRFA.Rows.Count > 0)
                        {

                            cellsAdj["A" + cellIndex.ToString()].Value = "Loss Reimbursement Fund";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);

                                cellsAdj[columname + cellIndex.ToString()].Value = "";
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;


                            if (dtProgramSummaryRetroReport== null)
                            {
                                cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                                for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                                {
                                    String columname = string.Empty;
                                    int columnindexvalue = i + 1;
                                    columname = getColumnNamefromIndex(columnindexvalue);
                                    string AdjsutmentType = string.Empty;
                                    AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                                    cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                                    FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                                }
                                cellIndex = cellIndex + 1;
                            }


                            for (int i = 1; i <= dtLRFA.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i;
                                columname = getColumnNamefromIndex(columnindexvalue);

                                String ILRFDescription = String.Empty;

                                ILRFDescription = dtLRFA.Rows[i - 1].Field<string>("DESCR");
                                cellsAdj["A" + cellIndex.ToString()].Value = ILRFDescription.ToString();
                                //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                                if (ILRFDescription == "AP  / (RP)   Due Zurich")
                                {
                                    SetFontBold(cellsAdj["A" + cellIndex.ToString()]);
                                }


                                for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                                {
                                    String columnamePGMID = string.Empty;
                                    int columnindexvaluePGMID = J + 1;
                                    columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                    int ILRFPGMID = 0;
                                    ILRFPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                                    int Fieldpostion;
                                    if (J == 1)
                                        Fieldpostion = 4;
                                    else
                                        Fieldpostion = J + 3;

                                    decimal ILRFFinalValue;
                                    ILRFFinalValue = dtLRFA.Rows[i - 1].Field<decimal>(Fieldpostion);
                                    cellsAdj[columnamePGMID + cellIndex.ToString()].Value = ILRFFinalValue.ToString();
                                    SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                    if (ILRFDescription == "AP  / (RP)   Due Zurich")
                                    {
                                        SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);
                                    }

                                }

                                cellIndex = cellIndex + 1;

                            }

                        }
                        cellIndex = cellIndex + 2;
                    }


                    #endregion

                    #region Sheet:OtherAdjustments

                    DataTable dtProgramSummaryOtherAdjsutmentDetails = (new ProgramPeriodsBS()).GetProgramSummaryOtherAdjsutmentDetails(AdjNo, 1, false, 0);

                    bool checkifAdditionalInvoice = false;
                    if (dtProgramSummaryOtherAdjsutmentDetails != null)
                    {
                        String AdditionalInvoice = String.Empty;
                        for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                        {
                            AdditionalInvoice = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<string>("DESCR");

                            if (AdditionalInvoice != "Retro" && AdditionalInvoice != "WC Deductible" && AdditionalInvoice != "Loss Reimbursement Fund" && AdditionalInvoice != "DEP" && AdditionalInvoice != "DEP 2")
                            {
                                checkifAdditionalInvoice = true;
                                break;
                            }

                        }
                    }

                    if (checkifAdditionalInvoice == true)
                    {
                        if (dtProgramSummaryOtherAdjsutmentDetails.Rows.Count > 0)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = "Additional Invoiced Amounts";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);

                                cellsAdj[columname + cellIndex.ToString()].Value = "";
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;


                            if (dtProgramSummaryRetroReport == null && dtLRFA == null)
                            {
                                cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                                for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                                {
                                    String columname = string.Empty;
                                    int columnindexvalue = i + 1;
                                    columname = getColumnNamefromIndex(columnindexvalue);
                                    string AdjsutmentType = string.Empty;
                                    AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                                    cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                                    FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                                }
                                cellIndex = cellIndex + 1;
                            }
                            for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i;
                                columname = getColumnNamefromIndex(columnindexvalue);

                                String OtherAdjustmentDescription = String.Empty;

                                OtherAdjustmentDescription = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<string>("DESCR");

                                if (OtherAdjustmentDescription != "Retro" && OtherAdjustmentDescription != "WC Deductible" && OtherAdjustmentDescription != "Loss Reimbursement Fund" && OtherAdjustmentDescription != "DEP" && OtherAdjustmentDescription != "DEP 2")
                                {
                                    cellsAdj["A" + cellIndex.ToString()].Value = OtherAdjustmentDescription.ToString();
                                    //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                                    SetFontBold(cellsAdj["A" + cellIndex.ToString()]);

                                    for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                                    {
                                        String columnamePGMID = string.Empty;
                                        int columnindexvaluePGMID = J + 1;
                                        columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                        int AddInvAmtPGMID = 0;
                                        AddInvAmtPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                                        int Fieldpostion;
                                        if (J == 1)
                                            Fieldpostion = 3;
                                        else
                                            Fieldpostion = J + 2;

                                        decimal AdditionInvoiceFinalValue;
                                        AdditionInvoiceFinalValue = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                                        cellsAdj[columnamePGMID + cellIndex.ToString()].Value = AdditionInvoiceFinalValue.ToString();
                                        SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                        SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);

                                    }

                                    cellIndex = cellIndex + 1;
                                }


                            }
                            cellIndex = cellIndex + 2;
                        }
                    }

                   
                    #endregion


                    #region Sheet:Period Result

                    if (dtProgramSummaryOtherAdjsutmentDetails.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Overall Result by Period";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);



                        for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                        {
                            String columnamePGMID = string.Empty;
                            int columnindexvaluePGMID = J + 1;
                            columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                            //int RetroPGMID = 0;
                            //RetroPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                            int Fieldpostion;
                            if (J == 1)
                                Fieldpostion = 3;
                            else
                                Fieldpostion = J + 2;

                            decimal PeriodTotal = 0;


                            for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                            {
                                if (i == 1)
                                {
                                    PeriodTotal = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                                }
                                else
                                {
                                    PeriodTotal = PeriodTotal + dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                                }


                            }
                            cellsAdj[columnamePGMID + cellIndex.ToString()].Value = PeriodTotal.ToString();
                            SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                            FormatHeaderTypeCellRetro(cellsAdj[columnamePGMID + cellIndex.ToString()]);

                        }
                    }

                    #endregion


                    worksheetAdj.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetAdj.UsedRange.ColumnCount; col++)
                    {
                        worksheetAdj.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                #region Sheet:Policy Number


                if (dtAdj != null && dtAdj.Rows.Count > 0)
                {
                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetPolicyNumber = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetPolicyNumber.Name = "Policy Number";

                    // Get the worksheet cells reference. 
                    SSG.IRange cellsAdj = worksheetPolicyNumber.Cells;
                    DataRow dr = dtAdj.Rows[0];
                    cellIndex = 1;



                   // worksheetPolicyNumber.Range["E2:E3"].Merge();
                   // string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                   // worksheetPolicyNumber.Shapes.AddPicture(path, 230, 13, 40, 30);

                    string path = Server.MapPath("~") + "\\images\\" + "zurich_logo.JPG";
                    worksheetPolicyNumber.Shapes.AddPicture(path, 230, 13,100, 60);
      


                    var item = (from items in dtAdj.AsEnumerable()
                                where (items.Field<Int32>("PREM ADJ ID") == AdjNo)
                                      && (!string.IsNullOrEmpty(items.Field<string>("VALUATION DATE")))
                                select
                                new { VALUATIONDATE = items.Field<string>("VALUATION DATE"), TOTALAMOUNTBILLED = items.Field<Decimal>("TOTAL AMOUNT BILLED") }
                                ).First();

                    cellsAdj["A" + cellIndex.ToString()].Value = "Insured:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Broker:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["BROKER NAME"]), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Ref Number:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", strInvNo, SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Loss Val Date:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.VALUATIONDATE), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Date:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", DateTime.Today.ToShortDateString(), SSG.HAlign.Left, cellIndex);
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Total Amount Billed:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                    MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.TOTALAMOUNTBILLED), SSG.HAlign.Left, cellIndex);
                    SetCurrencyFormatProgramSummaryTotalAmountBilled(cellsAdj["B" + cellIndex.ToString() + ":C" + cellIndex.ToString()]);
                    cellIndex = cellIndex + 2;

                    DataTable dtPeriodDet = (new ProgramPeriodsBS()).GetProgramSummaryPeriodDetails(AdjNo);

                    if (dtPeriodDet.Rows.Count > 0)
                    {

                        cellsAdj["A" + cellIndex.ToString()].Value = "Program period / Adjustment Type:";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                        for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                        {
                            int columnindexvalue = 0;
                            int columnindexvalue1 = 0;


                            String columname = string.Empty;
                            String columname1 = string.Empty;
                            if (i == 0)
                            {
                                columnindexvalue = i + 2;
                                columnindexvalue1 = columnindexvalue + 1;
                                ViewState["ColumnIndex1"] = columnindexvalue1.ToString();

                            }
                            else
                            {
                                if (ViewState["ColumnIndex1"] != null)
                                {
                                    columnindexvalue = Convert.ToInt32(ViewState["ColumnIndex1"]) + 1;
                                    columnindexvalue1 = columnindexvalue + 1;
                                    ViewState["ColumnIndex1"] = columnindexvalue1.ToString();
                                }
                            }
                            columname = getColumnNamefromIndex(columnindexvalue);
                            columname1 = getColumnNamefromIndex(columnindexvalue1);
                            columname1 = ":" + columname1.ToString();

                            string Programperiod = string.Empty;
                            Programperiod = dtPeriodDet.Rows[i].Field<string>("Program Period");

                            //cellsAdj[columname + cellIndex.ToString()].Value = Programperiod.ToString();

                            MergeAndFormatCellsWithColor(cellsAdj, columname, columname1, Convert.ToString(Programperiod), SSG.HAlign.Left, cellIndex);
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString() + columname1 + cellIndex.ToString()]);

                            //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;

                    }

                    #region Sheet:Policy Numbers

                    DataTable GetProgramSummaryPolicyInfo = (new ProgramPeriodsBS()).GetProgramSummaryPolicyInfo(AdjNo);
                    if (GetProgramSummaryPolicyInfo != null)
                    {
                        if (GetProgramSummaryPolicyInfo.Rows.Count > 0)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                            for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                            {

                                int columnindexvalue = 0;
                                int columnindexvalue1 = 0;


                                String columnamePolicy = string.Empty;
                                String columname1Policy = string.Empty;

                                if (i == 0)
                                {
                                    columnindexvalue = i + 2;
                                    columnindexvalue1 = columnindexvalue + 1;
                                    ViewState["ColumnIndex1Policy"] = columnindexvalue1.ToString();

                                }
                                else
                                {
                                    if (ViewState["ColumnIndex1Policy"] != null)
                                    {
                                        columnindexvalue = Convert.ToInt32(ViewState["ColumnIndex1Policy"]) + 1;
                                        columnindexvalue1 = columnindexvalue + 1;
                                        ViewState["ColumnIndex1Policy"] = columnindexvalue1.ToString();
                                    }
                                }
                                columnamePolicy = getColumnNamefromIndex(columnindexvalue);
                                columname1Policy = getColumnNamefromIndex(columnindexvalue1);


                                DataTable GetPolicyInfoByPGMID = (from dts in GetProgramSummaryPolicyInfo.AsEnumerable()
                                                                  where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(dtPeriodDet.Rows[i]["PREM_ADJ_PGM_ID"]))
                                                                  select dts).CopyToDataTable();
                                cellIndex = 9;
                                for (int k = 0; k < GetPolicyInfoByPGMID.Rows.Count; k++)
                                {
                                    if (k == 0)
                                    {
                                        String PolicyNumbervalue = string.Empty;
                                        String Policy_Type = string.Empty;
                                        PolicyNumbervalue = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Number");
                                        cellsAdj[columnamePolicy + cellIndex.ToString()].Value = PolicyNumbervalue.ToString();

                                        Policy_Type = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Type");
                                        cellsAdj[columname1Policy + cellIndex.ToString()].Value = Policy_Type.ToString();
                                    }
                                    else
                                    {

                                        String PolicyNumbervalue = string.Empty;
                                        String Policy_Type = string.Empty;
                                        PolicyNumbervalue = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Number");
                                        cellsAdj[columnamePolicy + cellIndex.ToString()].Value = PolicyNumbervalue.ToString();

                                        Policy_Type = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Type");
                                        cellsAdj[columname1Policy + cellIndex.ToString()].Value = Policy_Type.ToString();
                                    }
                                    cellIndex = cellIndex + 1;
                                }

                            }
                        }
                    }
                    #endregion

                    worksheetPolicyNumber.UsedRange.Columns.AutoFit();
                    for (int col = 0; col < worksheetPolicyNumber.UsedRange.ColumnCount; col++)
                    {
                        worksheetPolicyNumber.Cells[1, col].ColumnWidth *= 1.15;
                    }
                }

                #endregion

                if (sheetIndex == 0)
                {
                    workbook.Worksheets.Add();
                }

                string vcwDirectory = ConfigurationManager.AppSettings["Directory"].ToString();
                string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
                sFormat = sFormat.Replace(":", "");
                sFormat = sFormat.Replace("-", "");
                char cFlag = 'P';
                //if (extIntFlag == 0)
                //{
                //    cFlag = 'E';
                //}

                workbook.Worksheets[0].Select();
                string strFileName = vcwDirectory + "\\" + cFlag + "_" + sFormat + "_" + Request["AdjNo"] + ".xlsx";
                //EAISA-5 Veracode flaw fix 12072017
                string strFileNameDecd = Server.HtmlDecode(Server.HtmlEncode(strFileName));
                workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
                workbook.Close();

                FileInfo file = new FileInfo(strFileNameDecd);
                Response.Clear();
                Response.ContentType = "application/xlsx";
                //EAISA-5 Veracode flaw fix 12072017
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileNameDecd);

                Response.AddHeader("Content-Length", file.Length.ToString());
                //EAISA-5 Veracode flaw fix 12072017
                Response.TransmitFile(strFileNameDecd);
                Response.Flush();

                //EAISA - 7 fix to avoid exception  14-5-2018
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception err)
            {
                common.Logger.Error(err);
            }
            finally
            {
                Response.End();
            }
        }
        // Phase -3
         public static String getColumnNamefromIndex(int column)
         {
             column--;
             String col = Convert.ToString((char)('A' + (column % 26)));
             while (column >= 26)
             {
                 column = (column / 26) - 1;
                 col = Convert.ToString((char)('A' + (column % 26))) + col;
             }
             return col;
         }

        private Dictionary<int, string> GetPrgPrdsWithType(List<DataTable> dtResult, string colName)
        {
            List<int> prgPrdIDList = new List<int>();

            for (int i = 0; i < dtResult.Count; i++)
            {
                prgPrdIDList.Add(Convert.ToInt32(dtResult[i].Rows[0][colName]));
            }
            Dictionary<int, string> prgPrdList = new ProgramPeriodsBS().GetProgramPeriodTypes(prgPrdIDList);
            return prgPrdList;
        }

        //This Method checks NullOrEmpty for decimal field.
        private string CheckNullOrEmptyReturnValue(DataRow row, string colName)
        {
            string result = !string.IsNullOrEmpty(Convert.ToString(row[colName])) ? Convert.ToString(row[colName]) : "0";
            return result;
        }

        //This Method sets currency format in excel cell for amount type 
        private void SetCurrencyFormat(SSG.IRange cell)
        {
            cell.HorizontalAlignment = SSG.HAlign.Right;
            //cell.NumberFormat = "$#,###";
           // cell.NumberFormat = "$#,##0";
            cell.NumberFormat = "$#,##0_);($#,##0)";
            
  
        }


        // Phase -3
        private void SetFontBold(SSG.IRange cell)
        {
            cell.Font.Bold = true;

        }

        // Phase -3
        //This Method sets currency format in excel cell for amount type 
        private void SetCurrencyFormatProgramSummaryTotalAmountBilled(SSG.IRange cell)
        {
            cell.HorizontalAlignment = SSG.HAlign.Left;
            //cell.NumberFormat = "$#,###";
         //   cell.NumberFormat = "$#,##0";
            cell.NumberFormat = "$#,##0_);($#,##0)";
            
  
          
        }

        //This Method sets currency format in excel cell for amount type with 2-decimal
        private void SetCurrencyFormatForDecimal(SSG.IRange cell)
        {
            cell.HorizontalAlignment = SSG.HAlign.Right;
            //cell.NumberFormat = "$#,###";
            //cell.NumberFormat = "$#,##0.00";
            cell.NumberFormat = "$#,##0.00_);($#,##0.00)";

                           
        }

        private decimal GetSumForDecimalField(DataTable dtResult, string colName)
        {
            var sumTotal = dtResult.AsEnumerable()
                                .Where(row => row.Field<decimal?>(colName) != null)
                                .Sum(row => row.Field<decimal>(colName));
            return Convert.ToDecimal(sumTotal);
        }

        private void MergeAndFormatCellsWithoutColor(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = true)
        {
            SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
            cell.Merge();
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Value = text;
        }
        private void MergeAndFormatCellsWithoutColorandBold(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = false)
        {
            SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
            cell.Merge();
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Value = text;
        }

        private void MergeAndFormatCellsWithColor(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = true, SSG.Color color = default(SSG.Color))
        {
            SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
            cell.Merge();
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Value = text;
            //FormatHeaderTypeCell(cell,align,isBold);
            SetColorToCell(cell, color);

        }

        private void FormatHeaderTypeCell(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, bool isBold = true, SSG.Color color = default(SSG.Color))
        {
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Borders.Color = SSG.Colors.SteelBlue;
            SetColorToCell(cell, color);
            //if (color == default(SSG.Color))
            //{
            //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
            //}
            //else
            //{
            //    cell.Interior.Color = color;
            //}
            //cell.Interior.Color = SSG.Colors.SteelBlue;
            //SSG.Color.FromArgb(192,192,192)
        }

        private void FormatHeaderTypeCellProgramSumary(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Center, bool isBold = true, SSG.Color color = default(SSG.Color))
        {
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Borders.Color = SSG.Colors.SteelBlue;
            SetColorToCell(cell, color);
            //if (color == default(SSG.Color))
            //{
            //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
            //}
            //else
            //{
            //    cell.Interior.Color = color;
            //}
            //cell.Interior.Color = SSG.Colors.SteelBlue;
            //SSG.Color.FromArgb(192,192,192)
        }

        private void FormatHeaderTypeCellRetro(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Right, bool isBold = true, SSG.Color color = default(SSG.Color))
        {
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Borders.Color = SSG.Colors.SteelBlue;
            SetColorToCell(cell, color);
            //if (color == default(SSG.Color))
            //{
            //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
            //}
            //else
            //{
            //    cell.Interior.Color = color;
            //}
            //cell.Interior.Color = SSG.Colors.SteelBlue;
            //SSG.Color.FromArgb(192,192,192)
        }
        private void FormatHeaderTypeCellExcessLoss(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, bool isBold = true, SSG.Color color = default(SSG.Color))
        {
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;
            cell.Borders.Color = SSG.Colors.SteelBlue;
            SetColorToCell(cell, color);
            //if (color == default(SSG.Color))
            //{
            //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
            //}
            //else
            //{
            //    cell.Interior.Color = color;
            //}
            //cell.Interior.Color = SSG.Colors.SteelBlue;
            //SSG.Color.FromArgb(192,192,192)
        }

        private void SetColorToCell(SSG.IRange cell, SSG.Color color = default(SSG.Color))
        {
            if (color == default(SSG.Color))
            {
                cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
            }
            else
            {
                cell.Interior.Color = color;
            }
        }

        public void GenerateCesarCodingPDFReport(int iAdjNo, bool HistFlag)
        {
            //Coding Work Sheet PDF Generation
            ReportDocument objMainDraftCDWSheet = new ReportDocument();
            objMainDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptCodingMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Connections
            GenerateReportConnections(objMainDraftCDWSheet);
            string strInvNo=string.Empty;
            PremiumAdjustmentBE PremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iAdjNo);
            if(PremAdjBE.INVC_NBR_TXT!=null && PremAdjBE.INVC_NBR_TXT!="")
                strInvNo = PremAdjBE.INVC_NBR_TXT;

            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Draft Invoice
            prmFlag.Value = 1;
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/
            //Draft Coding Work Sheet PDF 
            IList<InvoiceExhibitBE> objDrftCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
            for (int iCount = 0; iCount < objDrftCDWorksheetIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainDraftCDWSheet, objDrftCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objDrftCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            /*****************Setting Sub Reports Parameters Value Begin******************/
            setCDWSubReportParameters(objMainDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            //MemoryStream st; - WinServer Changes for crystal Reports - WinServer Changes for crystal Reports
            //st = (MemoryStream)objMainDraftCDWSheet.ExportToStream(ExportFormatType.PortableDocFormat);
            Stream st;
            st = (Stream)objMainDraftCDWSheet.ExportToStream(ExportFormatType.PortableDocFormat);
            try
            {
                //byte[] arr = st.ToArray(); 
                byte[] arr = new byte[st.Length];
                st.Read(arr, 0, (int)st.Length);
                string strFileName = "Cesar Coding PDF.pdf";
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.AddHeader("content-length", st.Length.ToString());
                Response.BinaryWrite(arr);
                st.Close();
            }
            catch (Exception ex)
            {
                ShowError("Unable to Preview the Report. Please contact Application Support Team");
                return;
            }
            Response.Flush();
            Response.End();
            objMainDraftCDWSheet.Close();
            objMainDraftCDWSheet.Dispose();
        }

        private void GenerateRevisedCesarCodingPDFReport(int iAdjNo, int iPreviousAdjNo, bool HistFlag)
        {
            //Coding Work Sheet PDF Generation
            ReportDocument objMainRevDraftCDWSheet = new ReportDocument();
            objMainRevDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Connections
            GenerateReportConnections(objMainRevDraftCDWSheet);
            string strInvNo = string.Empty;
            string strPrevInvNo = string.Empty;
            PremiumAdjustmentBE PremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iAdjNo);
            if (PremAdjBE.INVC_NBR_TXT != null && PremAdjBE.INVC_NBR_TXT != "")
                strInvNo = PremAdjBE.INVC_NBR_TXT;
            PremiumAdjustmentBE prevPremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iPreviousAdjNo);
            if (prevPremAdjBE.FNL_INVC_NBR_TXT != null && prevPremAdjBE.FNL_INVC_NBR_TXT != "")
                strPrevInvNo = prevPremAdjBE.FNL_INVC_NBR_TXT;

            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmPreviousAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmPreviousInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Draft Invoice
            prmFlag.Value = 1;
            prmPreviousAdjNo.Value = iPreviousAdjNo;
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmPreviousInvNo.Value = strPrevInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainRevDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/
            //Draft Coding Work Sheet PDF 
            IList<InvoiceExhibitBE> objDrftCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
            for (int iCount = 0; iCount < objDrftCDWorksheetIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainRevDraftCDWSheet, objDrftCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objDrftCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            /*****************Setting Sub Reports Parameters Value Begin******************/
                setRevisionCDWSubReportParameters(objMainRevDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmPreviousAdjNo, prmPreviousInvNo, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
                //MemoryStream st; - WinServer Changes for crystal Reports
                //st = (MemoryStream)objMainRevDraftCDWSheet.ExportToStream(ExportFormatType.PortableDocFormat);
                Stream st;
                st = (Stream)objMainRevDraftCDWSheet.ExportToStream(ExportFormatType.PortableDocFormat);

                try
                {
                    //byte[] arr = st.ToArray();
                    byte[] arr = new byte[st.Length];
                    st.Read(arr, 0, (int)st.Length);
                    string strFileName = "Cesar Coding PDF.pdf";
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.AddHeader("content-length", st.Length.ToString());
                    Response.BinaryWrite(arr);
                    st.Close();
                }
                catch (Exception ex)
                {
                    ShowError("Unable to Preview the Report. Please contact Application Support Team");
                    return;
                }
                Response.Flush();
                Response.End();
                objMainRevDraftCDWSheet.Close();
                objMainRevDraftCDWSheet.Dispose();
        }
        private void GenerateReportConnections(ReportDocument objMain)
        {
            // Set the Database Connection String Properties for the report.
            common = new Common(this.GetType());
            AISBasePage objAISPage = new AISBasePage();
            ConnectionInfo conn = objAISPage.GetReportConnection();

            TableLogOnInfo tblLogonInfo;
            Tables tbls = objMain.Database.Tables;

            // Set the Database Login Info to all the tables used in the main report.
            foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in tbls)
            {
                tblLogonInfo = tbl.LogOnInfo;
                tblLogonInfo.ConnectionInfo = conn;
                tbl.ApplyLogOnInfo(tblLogonInfo);
            }


            // Loop through all the sections and its objects to do the same for the subreports
            foreach (CrystalDecisions.CrystalReports.Engine.Section section in objMain.ReportDefinition.Sections)
            {

                // In each section we need to loop through all the reporting objects
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subReport = (SubreportObject)reportObject;
                        ReportDocument subDocument = objMain.OpenSubreport(subReport.SubreportName);

                        foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                        {
                            tblLogonInfo = table.LogOnInfo;
                            tblLogonInfo.ConnectionInfo = conn;
                            table.ApplyLogOnInfo(tblLogonInfo);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Method to set the parameters to Subreports to Internal
        /// </summary>
        /// <param name="objMain"></param>
        /// <param name="prmAdjNo"></param>
        /// <param name="prmFlag"></param>
        #region setSubReportParameters Method
        private void setInternalSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
        {
            /*****************Setting Sub Reports Parameters Value Begin******************/
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjusInvoice.rpt");
            //319-for AdjusInvoice
            objMain.SetParameterValue("@CMTCATGID", 319, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLossBasedAssessmentsCalculation.rpt");
            //objMain.SetParameterValue("@INCLERPLBA", false, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFInternal.rpt");
            //Newly added parameter
            objMain.SetParameterValue("@CMTCATGID", 318, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptBrokerLetter.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptBrokerLetter.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptBrokerLetter.rpt");
            //339-for Broker letter
            objMain.SetParameterValue("@CMTCATGID", 339, "srptBrokerLetter.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptEscrowFundAdjustment.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptEscrowFundAdjustment.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptEscrowFundAdjustment.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptEscrowFundAdjustment.rpt");
            //objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternal.rpt");
            //Newly adde parameter
            objMain.SetParameterValue("@CMTCATGID", 375, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLegendSecond.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLegendSecond.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLegendSecond.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLegendSecond.rpt");
            //objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            //objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptSurchargesAssessments.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjInvCombinedElements.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjInvCombinedElements.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjInvCombinedElements.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");

            //Texas Tax
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@CMTCATGID", 534, "srptLRFInternaTax.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@CMTCATGID", 533, "srptLRFExternaTax.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@CMTCATGID", 534, "srptLRFExternaTax.rpt - 01");


            /*****************Setting Sub Reports Parameters Value End******************/
        }
        #endregion

        /// <summary>
        /// Method to set the parameters to Subreports to External
        /// </summary>
        /// <param name="objMain"></param>
        /// <param name="prmAdjNo"></param>
        /// <param name="prmFlag"></param>
        #region setSubReportParameters Method
        private void setExternalSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
        {
            /*****************Setting Sub Reports Parameters Value Begin******************/
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCumTotalsWorksheet.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjusInvoice.rpt");
            //319-for AdjusInvoice
            objMain.SetParameterValue("@CMTCATGID", 319, "srptAdjusInvoice.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLossBasedAssessmentsCalculation.rpt");
            //objMain.SetParameterValue("@INCLERPLBA", false, "srptLossBasedAssessmentsCalculation.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFInternal.rpt");
            //Newly added parameter
            objMain.SetParameterValue("@CMTCATGID", 318, "srptLRFInternal.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptBrokerLetter.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptBrokerLetter.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptBrokerLetter.rpt");
            //339-for Broker letter
            objMain.SetParameterValue("@CMTCATGID", 339, "srptBrokerLetter.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptEscrowFundAdjustment.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptEscrowFundAdjustment.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptEscrowFundAdjustment.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptEscrowFundAdjustment.rpt");
            //objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", true, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternal.rpt");
            //Newly adde parameter
            objMain.SetParameterValue("@CMTCATGID", 375, "srptLRFExternal.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjLegend.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCalcInfoLegend.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLegendSecond.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLegendSecond.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLegendSecond.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLegendSecond.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptExcessLoss.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptProcessingCheckList.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", true, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCombinedElements.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjInvCombinedElements.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjInvCombinedElements.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjInvCombinedElements.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjustmentMain.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptARiEsPostingDetails.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptClaimHandlingFeeSummary.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");

            //Texas Tax
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFInternaTax.rpt");
            objMain.SetParameterValue("@CMTCATGID", 534, "srptLRFInternaTax.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternaTax.rpt");
            objMain.SetParameterValue("@CMTCATGID", 533, "srptLRFExternaTax.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternaTax.rpt - 01");
            objMain.SetParameterValue("@CMTCATGID", 534, "srptLRFExternaTax.rpt - 01");


            /*****************Setting Sub Reports Parameters Value End******************/
        }
        #endregion
        /// <summary>
        /// Mthod to Set the Parameters to Master Report
        /// </summary>
        /// <param name="objMain"></param>
        /// <param name="strAttachCode"></param>
        /// <param name="blStatus"></param>
        /// <param name="intFlag"></param>
        #region setMasterReportParameters Method
        public void setMasterReportParameter(ReportDocument objMain, string strAttachCode, bool blStatus, int intFlag, char strInternalFlag, int iAdjNo)
        {
            AdjustmentReviewCommentsBS objReportAvailable = new AdjustmentReviewCommentsBS();

            switch (strAttachCode)
            {
                //Broker Letter Exhibit
                case "INV1":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "BROKER LETTER"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressBrokerLetter", "View");
                                else
                                    objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressBrokerLetter", "View");
                                else
                                    objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                    break;

                //Adjustment Invoice Exhibit
                case "INV2":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "SUMMARY INVOICE"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressSectionAdjInv", "View");
                                else
                                    objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressSectionAdjInv", "View");
                                else
                                    objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                    break;

                //Retrospective Premium Adjustment Exhibit
                case "INV3":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE PREMIUM ADJUSTMENT"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressRetroPremAdj", "View");
                                else
                                    objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressRetroPremAdj", "View");
                                else
                                    objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                    break;


                //Retrospective Premium Adjustment Legend Exhibit
                case "INV4":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE ADJUSTMENT LEGEND"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressRetroLegend", "View");
                                else
                                    objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressRetroLegend", "View");
                                else
                                    objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                    break;

                //Loss Based Assessment Exhibit
                case "INV5":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LBA"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLBA", "View");
                                else
                                    objMain.SetParameterValue("SuppressLBA", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLBA", "View");
                                else
                                    objMain.SetParameterValue("SuppressLBA", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLBA", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressLBA", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLBA", "Suppress");
                    break;

                //Claims Handling Fee
                case "INV6":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CHF"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCHF", "View");
                                else
                                    objMain.SetParameterValue("SuppressCHF", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCHF", "View");
                                else
                                    objMain.SetParameterValue("SuppressCHF", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCHF", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressCHF", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressCHF", "Suppress");
                    break;


                //Excess Losses
                case "INV7":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "EXCESS LOSS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressExcessLoss", "View");
                                else
                                    objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressExcessLoss", "View");
                                else
                                    objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                    break;

                //Cumulative Totals Worksheet
                case "INV8":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PAID LOSS BILLINGS FOR CURRENT ADJUSTMENT PERIOD"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCumTotals", "View");
                                else
                                    objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                                else
                                    objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                    break;

                //Residual Market Subsidy Charge
                case "INV9":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RML"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                                else
                                    objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                                else
                                    objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                        }
                        else
                            objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                    break;


                //Surcharges and Assessments
                case "INV10":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETRO PREMIUM BASED SURCHARGES & ASSESSMENTS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressSurcharges", "View");
                                else
                                    objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressSurcharges", "View");
                                else
                                    objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressSurcharges", "View");
                        }
                        else
                            objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                    break;


                //Adjustment of NY Second Injury Fund
                //case "INV11":
                //    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "NY-SIF"))
                //    {
                //        if (blStatus == true)
                //        {
                //            if (intFlag == 1)
                //                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                //                    objMain.SetParameterValue("SuppressAdjNY", "View");
                //                else
                //                    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                //            if (intFlag == 2)
                //                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                //                    objMain.SetParameterValue("SuppressAdjNY", "View");
                //                else
                //                    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                //            if (intFlag == 3)
                //                objMain.SetParameterValue("SuppressAdjNY", "View");
                //        }
                //        else
                //            objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                //    }
                //    else
                //        objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                //    break;

                //Loss Reimbursement Fund Adjustment -External
                case "INV11":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (EXTERNAL)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFExternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFExternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                    break;

                //Escrow Fund Adjustment
                case "INV12":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LOSS FUND"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressEscrow", "View");
                                else
                                    objMain.SetParameterValue("SuppressEscrow", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressEscrow", "View");
                                else
                                    objMain.SetParameterValue("SuppressEscrow", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressEscrow", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressEscrow", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressEscrow", "Suppress");
                    break;

                //Loss Reimbursement Fund Adjustment-Internal
                case "INV13":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (INTERNAL)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFInternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFInternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                    break;

                //Aries Posting Details 
                case "INV14":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ARIES POSTING DETAILS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressAries", "View");
                                else
                                    objMain.SetParameterValue("SuppressAries", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressAries", "View");
                                else
                                    objMain.SetParameterValue("SuppressAries", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressAries", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressAries", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressAries", "Suppress");
                    break;


                //Combined Elements Exhibit - Internal
                case "INV15":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "COMBINED ELEMENTS"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCombinedElements", "View");
                                else
                                    objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCombinedElements", "View");
                                else
                                    objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                    break;

                //Processing and Distribution Checklist
                case "INV16":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PROCESSING CHECKLIST"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressProcessingCheckList", "View");
                                else
                                    objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressProcessingCheckList", "View");
                                else
                                    objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                    break;

                //Cesar Coding WorkSheet
                case "INV17":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CESAR CODING WORKSHEET"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCesar", "View");
                                else
                                    objMain.SetParameterValue("SuppressCesar", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressCesar", "View");
                                else
                                    objMain.SetParameterValue("SuppressCesar", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressCesar", "View");
                        }
                        else
                            objMain.SetParameterValue("SuppressCesar", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressCesar", "Suppress");
                    break;

                //Retrospective Tax Exhibit-External
                case "INV18":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "STATE SALES & SERVICE TAX EXHIBIT (EXTERNAL)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFTaxExternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFTaxExternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                    break;

                //Retrospective Tax Exhibit-Internal
                case "INV19":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "STATE SALES & SERVICE TAX EXHIBIT (INTERNAL)"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFTaxInternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressLRFTaxInternal", "View");
                                else
                                    objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                        }
                        else
                            objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                    break;


            }


        }
        #endregion

        /// <summary>
        /// Method to set the parameters to Subreports
        /// </summary>
        /// <param name="objMain"></param>
        /// <param name="prmAdjNo"></param>
        /// <param name="prmFlag"></param>
        #region setSubReportParameters Method
        private void setCDWSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
        {
            /*****************Setting Sub Reports Parameters Value Begin******************/

            //objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");

            /*****************Setting Sub Reports Parameters Value End******************/
        }
        #endregion

        /// <summary>
        /// Method to set the parameters to Subreports
        /// </summary>
        /// <param name="objMain"></param>
        /// <param name="prmAdjNo"></param>
        /// <param name="prmFlag"></param>
        #region setSubReportParameters Method
        private void setRevisionCDWSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmPreviousAdjNo, ParameterDiscreteValue prmPreviousInvNo, ParameterDiscreteValue prmHistFlag)
        {
            /*****************Setting Sub Reports Parameters Value Begin******************/

            //objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");

            //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", true, "srptAdjNYSIFRevision.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptSurchargesAssessments.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptSurchargesAssessments.rpt");

            //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptKYORRevision.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptKYORRevision.rpt");
            //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptKYORRevision.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptKYORRevision.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", true, "srptKYORRevision.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("IsFlipSigns", false, "srptResidualMarkSubCharge.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptResidualMarkSubCharge.rpt");

            //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptResidualMarkSubChargeRevision.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubChargeRevision.rpt");
            //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptResidualMarkSubChargeRevision.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubChargeRevision.rpt");
            //objMain.SetParameterValue("IsFlipSigns", true, "srptResidualMarkSubChargeRevision.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("IsFlipSigns", false, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");

            //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptCesarCodingWorksheetRevision.rpt");
            //objMain.SetParameterValue("IsFlipSigns", true, "srptCesarCodingWorksheetRevision.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheetRevision.rpt");
            //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptCesarCodingWorksheetRevision.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheetRevision.rpt");

            /*****************Setting Sub Reports Parameters Value End******************/
        }
        #endregion


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


        public void ShowError(string errorMessage)
        {
            try
            {   //06/23 for vera code
                //AISErrorValidator.ErrorMessage = errorMessage;
                AISErrorValidator.ErrorMessage = HttpUtility.HtmlEncode(Convert.ToString(errorMessage));//veracode fix 07162018
                AISSummary.CssClass = "ValidationSummary";
                //summary.Font.Size = FontUnit.Small;
                //summary.ForeColor = System.Drawing.Color.Red;
                AISErrorValidator.Validate();
                AISSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        private string[] VCshellScriptExecution(char cFlag, object intAdjNo)
        {

            #region StartShelll

            ProcessStartInfo startInfo;
            Process batchExecute;
            string[] strArray = new string[2];
            string setSubParamsINT = string.Empty;
            string setSubParamsEXT = string.Empty;
            string vcExePath = string.Empty;
            string vcwDirectory = string.Empty;
            string tocUSImagePath = string.Empty;
            string tocCanadaImagePath = string.Empty;
            try
            {
                vcExePath = ConfigurationManager.AppSettings["EXEPath"].ToString();
                vcwDirectory = ConfigurationManager.AppSettings["Directory"].ToString();
                tocUSImagePath = ConfigurationManager.AppSettings["TOCUSImagePath"].ToString();
                tocCanadaImagePath = ConfigurationManager.AppSettings["TOCCanadaImagePath"].ToString();
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(vcExePath) || string.IsNullOrEmpty(vcwDirectory))
                {
                    new ApplicationStatusLogBS().WriteLog((int)intAdjNo, "Configuration Error", "ERR", "Path not Found", ex.Message, 1);
                    return null;
                }
            }
            try
            {
                string reportPath = Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt").ToString();
                string zLogo = Server.MapPath("images\\" + "zurich_logo_1650_121.jpg").ToString();
                //string reportPath = ConfigurationSettings.AppSettings["ReportPath"].ToString();
                string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
                sFormat = sFormat.Replace(":", "");
                sFormat = sFormat.Replace("-", "");
                string CurrentFile = vcwDirectory + "\\" + cFlag + "-" + sFormat + "_" + intAdjNo.ToString() + ".pdf";
                if (cFlag == 'I')
                {
                    foreach (string sParam in _VCarrMasterParamsInt)
                    {
                        if (!string.IsNullOrEmpty(sParam))
                        {
                            _VCsbMasterParamsInt.Append(sParam);
                            _VCsbMasterParamsInt.Append(" ");
                        }
                    }
                    foreach (string sParam in _VCarrSubParamsInt)
                    {
                        //string strParam = Convert.ToString(sParam);
                        if (!string.IsNullOrEmpty(sParam))
                        {
                            _VCsbSubParamsInt.Append(sParam);
                            _VCsbSubParamsInt.Append("-----");
                        }
                    }
                    setSubParamsINT = _VCsbSubParamsInt.ToString().Substring(0, _VCsbSubParamsInt.ToString().LastIndexOf("-----"));
                }
                else
                {
                    foreach (string sParam in _VCarrMasterParamsExt)
                    {
                        if (!string.IsNullOrEmpty(sParam))
                        {
                            _VCsbMasterParamsExt.Append(sParam);
                            _VCsbMasterParamsExt.Append(" ");
                        }
                    }
                    foreach (string sParam in _VCarrSubParamsExt)
                    {
                        //string strParam = Convert.ToString(sParam);
                        if (!string.IsNullOrEmpty(sParam))
                        {
                            _VCsbSubParamsExt.Append(sParam);
                            _VCsbSubParamsExt.Append("-----");
                        }
                    }
                    setSubParamsEXT = _VCsbSubParamsExt.ToString().Substring(0, _VCsbSubParamsExt.ToString().LastIndexOf("-----"));
                }

                System.Text.StringBuilder stApp = new System.Text.StringBuilder();
                stApp.Append("\"" + vcExePath + "\"");
                stApp.Append(" ");
                stApp.Append("-e");
                stApp.Append(" ");
                stApp.Append("\"" + reportPath + "\"");
                stApp.Append(" ");
                if (cFlag == 'I')
                {
                    stApp.Append(_VCsbMasterParamsInt.ToString());
                    setSubParamsINT = "Parm8:" + setSubParamsINT;
                    stApp.Append("\"" + setSubParamsINT + "\"");
                }
                if (cFlag == 'E')
                {
                    stApp.Append(_VCsbMasterParamsExt.ToString());
                    setSubParamsEXT = "Parm8:" + setSubParamsEXT;
                    stApp.Append("\"" + setSubParamsEXT + "\"");
                }

                // TOC
                stApp.Append(" ");
                tocUSImagePath = "Parm30:" + tocUSImagePath;
                stApp.Append("\"" + tocUSImagePath + "\"");
                stApp.Append(" ");
                tocCanadaImagePath = "Parm31:" + tocCanadaImagePath;
                stApp.Append("\"" + tocCanadaImagePath + "\"");

                //stApp.Append(" ");
                stApp.Append(" ");
                stApp.Append("\"Export_Format:Adobe Acrobat (pdf)\"");
                stApp.Append(" ");
                stApp.Append("\"Export_File:" + CurrentFile + "\"");
                stApp.Append(" ");
                if (cFlag == 'I')
                    stApp.Append("\"PDF_TOC:2>40>11>6>5>2>" + CurrentFile + "\"");
                if (cFlag == 'E')
                    stApp.Append("\"PDF_TOC:2>40>11>6>5>2>" + CurrentFile + "\"");
                stApp.Append(" ");
                stApp.Append("\"PDF_Bookmark_Tags:" + CurrentFile + "\"");
                stApp.Append(" ");
                //stApp.Append("\"PDF_PAGE_N:1>10>10>10>Bottom>Right>Page_NofM>" + CurrentFile + ">Arial\"");
                stApp.Append("\"PDF_PAGE_N:1>10>10>10>Bottom>Right>Page_NofM>" + CurrentFile + ">TT||Arial||Bold||0;0;0\"");
                //stApp.Append(" ");
                //stApp.Append("\"PDF_AddImage:" + CurrentFile + ">1>1>40>60>50>10>http://www.zurichna.com>" + zLogo + ">0\"");
                AISBasePage objAISPage = new AISBasePage();
                ConnectionInfo conn = objAISPage.GetReportConnection();
                stApp.Append(" \"Connect_To_SQLOLEDB:" + conn.ServerName + ">>" + conn.DatabaseName + ">>" + 
                    conn.IntegratedSecurity.ToString().ToUpper()+"\"");
                stApp.Append(" "+"\"user_id:" + conn.UserID + "\""+" " + "\"password:" + conn.Password + "\"");
                if (File.Exists(vcExePath))
                {
                    startInfo = new ProcessStartInfo(vcExePath);
                    startInfo.WorkingDirectory = vcwDirectory;
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = false;
                    //startInfo.RedirectStandardOutput = true;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    batchExecute = new Process();
                    batchExecute.StartInfo = startInfo;
                    batchExecute.StartInfo.Arguments = stApp.ToString();
                    try
                    {

                        // Start the process with the info we specified.
                        // Call WaitForExit and then the using statement will close.
                        //this.hWND = (int)WindowShowStyle.Hide;
                        // In your code some where: show a form, without making it active
                        //ShowWindow(hWND, WindowShowStyle.Hide);
                        batchExecute.Start();
                        batchExecute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        batchExecute.WaitForInputIdle();
                        //IntPtr hWnd = batchExecute.MainWindowHandle;
                        //ShowWindow(hWnd, SW_HIDE);
                        batchExecute.WaitForExit();
                        //batchExecute.Dispose();
                        // string sStream = batchExecute.StandardOutput.ReadToEnd();
                        //batchExecute.Close();
                        //batchExecute.CloseMainWindow();
                        //batchExecute.Kill();
                    }
                    catch (Exception ex)
                    {
                        new ApplicationStatusLogBS().WriteLog((int)intAdjNo, "ShellExecution", "ERR", "Processor Execution Error", ex.Message, 2);
                        return null;
                        // Log error.
                    }
                    strArray[0] = cFlag.ToString();
                    strArray[1] = CurrentFile;
                }
            }
            catch (Exception ex)
            {
                new ApplicationStatusLogBS().WriteLog((int)intAdjNo, "ShellExecution", "ERR", "VisualCut Error", ex.Message, 3);
                strArray = null;
                throw;
            }
            return strArray;
            #endregion
        }

    }
}
