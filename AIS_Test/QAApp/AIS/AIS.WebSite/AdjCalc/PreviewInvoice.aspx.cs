using System;
using System.Collections;
using System.Collections.Generic;
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

namespace ZurichNA.AIS.WebSite.AdjCalc
{
    public partial class PreviewInvoice : System.Web.UI.Page
    {
        protected static Common common = null;
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
                case "ESCROW":
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
                case "NY-SIF":
                    //Adjustment of NY Second Injury Fund
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptNYSIFMasterReport.rpt", "srptAdjNYSecInjFund.rpt", "SuppressAdjNY", strDocumentName);
                    break;
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
                //KY & OR TAXES
                case "KYOR":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptKYORMasterReport.rpt", "srptWorkerCompTaxAssessKentOreg.rpt", "SuppressKYOR", strDocumentName);
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
                case "CUMULATIVE TOTALS":
                    GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCumulativeMasterReport.rpt", "srptCumTotalsWorksheet.rpt", "SuppressCumTotals", strDocumentName);
                    break;
               //Internal PDF
                case "INTERNAL PDF":
                    GenerateInternalPDFReport(iAdjNo, HistFlag);
                    break;
               //External PDF
                case "EXTERNAL PDF":
                    GenerateExternalPDFReport(iAdjNo, HistFlag);
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

       

        private void GenerateReport(int iAdjNo, string strInvNo, bool HistFlag, string strMasterReport, string strSubReport, string strSubReportParameter, string strDocName)
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
            if (strDocName.ToUpper() == "NY-SIF" || strDocName.ToUpper() == "KYOR")
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
            /*****************Setting Sub Reports Parameters Value End******************/
            MemoryStream memStream;
            memStream = (MemoryStream)objMainPreview.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            try
            {
                byte[] byteArray = memStream.ToArray();
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
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
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

            try
            {
                st = objMainDraftInternal.ExportToStream(ExportFormatType.PortableDocFormat);
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

            Stream st;
            st = objMainDraftExternal.ExportToStream(ExportFormatType.PortableDocFormat);

            try
            {
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
            Stream st;
            st = objMainDraftCDWSheet.ExportToStream(ExportFormatType.PortableDocFormat);

            try
            {
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
                Stream st;
                st = objMainRevDraftCDWSheet.ExportToStream(ExportFormatType.PortableDocFormat);

                try
                {
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
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
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
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
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
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", true, "srptAdjNYSecInjFund.rpt");
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
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", true, "srptWorkerCompTaxAssessKentOreg.rpt");
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
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CUMULATIVE TOTALS"))
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
                                    objMain.SetParameterValue("SuppressCumTotals", "View");
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


                //Workers Compensation Tax and Assessment Kentucky
                case "INV10":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "KY & OR TAXES"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressKYOR", "View");
                                else
                                    objMain.SetParameterValue("SuppressKYOR", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressKYOR", "View");
                                else
                                    objMain.SetParameterValue("SuppressKYOR", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressKYOR", "View");
                        }
                        else
                            objMain.SetParameterValue("SuppressKYOR", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressKYOR", "Suppress");
                    break;


                //Adjustment of NY Second Injury Fund
                case "INV11":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "NY-SIF"))
                    {
                        if (blStatus == true)
                        {
                            if (intFlag == 1)
                                if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressAdjNY", "View");
                                else
                                    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                            if (intFlag == 2)
                                if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                    objMain.SetParameterValue("SuppressAdjNY", "View");
                                else
                                    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                            if (intFlag == 3)
                                objMain.SetParameterValue("SuppressAdjNY", "View");
                        }
                        else
                            objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                    break;

                //Loss Reimbursement Fund Adjustment -External
                case "INV12":
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
                case "INV13":
                    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ESCROW"))
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
                case "INV14":
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
                case "INV15":
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
                case "INV16":
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
                case "INV17":
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
                case "INV18":
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

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
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

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");

            //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSIFRevision.rpt");
            //objMain.SetParameterValue("@REVFLAGPREV", true, "srptAdjNYSIFRevision.rpt");

            objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
            objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");

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
            {
                AISErrorValidator.ErrorMessage = errorMessage;
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

    }
}
