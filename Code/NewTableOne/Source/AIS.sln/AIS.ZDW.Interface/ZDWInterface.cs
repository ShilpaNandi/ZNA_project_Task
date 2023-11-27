using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.WebSite;


namespace ZurichNA.AIS.ZDW.Interface
{
    class ZDWInterface
    {
        string sSQLServerConnectString = ConfigurationManager.ConnectionStrings["SQLServerConnectString"].ToString();
        string sLogFileName = "ZDWLog_" + DateTime.Now.ToString("MMddyyyyhhmm") + ".txt";

        public void ZDWSummaryInvoiceMain()
        {
            try
            {
                //For ZNA invoices
                GetSummaryInvocieDetails("Z01");
                //For Canada invoices
                GetSummaryInvocieDetails("ZC2");
                WriteLog("ZDW Interface", "Inf", "ZDW job successfully completed!", "ZDW job successfully completed!", 1);
                WriteFileLogFile("ZDW Job completed successfully");
                SendStatusMail("", true);
            }
            catch (Exception ex)
            {
                SendStatusMail(ex.Message, false);
                throw ex;
            }
        }


        public void GetSummaryInvocieDetails(string companyCode)
        {
                SqlConnection conn = new SqlConnection();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = 0;
                conn.ConnectionString = sSQLServerConnectString;
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "dbo.ModAIS_SendSummaryInvoiceToZDW";
                sqlCmd.Parameters.Add("@company_cd", SqlDbType.VarChar).Value = companyCode;
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlCmd;

                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    GenerateSummaryInvoice((int)dr["prem_adj_id"], dr["fnl_invc_nbr_txt"].ToString(), companyCode);
                    UpdateSummaryInvoiceStatus((int)dr["prem_adj_id"]);
                }

                

                dt.Dispose();
        }

        public void GenerateSummaryInvoice(int adjID, string invoiceNumber, string companyCode)
        {
            try
            {
                WriteFileLogFile("Summary Invoice Generation Started Successfully" + "\t" + "Adjustment ID : " + adjID);
               
                ZurichNA.AIS.WebSite.AdjCalc.PreviewInvoice objPre = new ZurichNA.AIS.WebSite.AdjCalc.PreviewInvoice();
                objPre.GenerateReportSaveOnDisk(adjID, invoiceNumber, false, "rptInvoiceMasterReport.rpt", "srptAdjusInvoice.rpt", "SuppressSectionAdjInv", "50_AISInvoice_48_" + invoiceNumber + "_Adjustment Summary", companyCode);
                WriteFileLogFile("Summary Invoice Generation Completed Successfully" + "\t" + "Adjustment ID : " + adjID);
                
            }
            catch (Exception ex)
            {
                WriteLog("ZDW Interface", "Inf", "Error during ZDW GenerateSummaryInvoice Function", ex.ToString(), 1);
                WriteFileLogFile("ZDW Summary Invoice Generation Failed" + "\t" + "Error : " + ex.Message);
                throw ex;
            }
        }

        private bool UpdateSummaryInvoiceStatus(int adjID)
        {
            bool blnUpdateStatus = true;
            try
            {
                #region UpdateStatus
                //This function will update various statuses
                #endregion

                SqlConnection conn = new SqlConnection();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = 0;
                conn.ConnectionString = sSQLServerConnectString;
                sqlCmd.Connection = conn;
                conn.Open();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "update prem_adj set zdw_sent_status=1,updt_dt=getdate() where prem_adj_id = " + adjID + "";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
            }
            catch (Exception ex)
            {
                blnUpdateStatus = false;
                //Write to log table
                WriteLog("ZDW Interface", "Inf", "ZDW Status update failed for adj id : " + adjID, ex.ToString(), 1);
            }
            return blnUpdateStatus;

        }

        public void WriteLog(string src_txt, string sev_cd, string shrt_desc_txt, string full_desc_txt, int crte_user_id)
        {

            //src_txt - Source of error
            //sev_cd - Severity Code 
            //* Err: Error
            //* Sev: Severe 
            //* War: Warning
            //* Inf: Information
            //shrt_desc_txt - To store brief descriprion
            //full_desc_txt - To store detailed description.
            //crte_user_id - Created by
            //crte_dt   - Created on

            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandTimeout = 0;
            conn.ConnectionString = sSQLServerConnectString;
            conn.Open();
            sqlCmd.Connection = conn;

            string sErrDataLine = "'" + src_txt + "','" + sev_cd;
            sErrDataLine = sErrDataLine + "','" + shrt_desc_txt + "','" + full_desc_txt + "'," + crte_user_id;
            string sInsertData = "Insert into APLCTN_STS_LOG (src_txt,sev_cd,shrt_desc_txt,full_desc_txt,crte_user_id) values ( " + sErrDataLine + ")";

            //Error Write to DB
            sqlCmd.CommandText = sInsertData;
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="newentry"></param>
        public void WriteFileLogFile(string entry)
        {
            string FilePath;
            if (ConfigurationManager.AppSettings["LogFile"] == null)
            {
                FilePath = "C:\\" + sLogFileName;
            }
            else
            {
                FilePath = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\" + sLogFileName;
            }

            string fullentry = DateTime.Now.ToShortDateString().ToString() + " " +
                                DateTime.Now.ToLongTimeString().ToString() +
                                ":" + entry;
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true))
                {

                    sw.WriteLine(fullentry);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        

        #region EMailNotification
        private void SendStatusMail(string ErrorMsg, bool status)
        {
            SMTPMailer dd = new SMTPMailer();
            dd.HasAttachment = true;
            System.Net.Mail.MailAddressCollection objMailList = new System.Net.Mail.MailAddressCollection();
            dd.lstTomailAddress = objMailList;
            string strEmailIDs = ConfigurationManager.AppSettings["NotificationEmailIDs"].ToString();
            string[] strEmailList = strEmailIDs.Split(';');
            for (int i = 0; i < strEmailList.Count(); i++)
            {
                objMailList.Add(strEmailList[i].ToString());
            }

            if (status)
                dd.Subject = ConfigurationManager.AppSettings["SummaryInvoiceSuccessSubject"].ToString();
            else
                dd.Subject = ConfigurationManager.AppSettings["SummaryInvoiceFailureSubject"].ToString();

            StringBuilder body = new StringBuilder();

            body.AppendLine("");

            if (status)
            {
                body.AppendLine(ConfigurationManager.AppSettings["SummaryInvoiceSuccessBody"].ToString());
                body.AppendLine("See attached log for further details");
            }
            else
            {
                body.AppendLine(ConfigurationManager.AppSettings["SummaryInvoiceFailureBody"].ToString());
                body.AppendLine("See attached log for further details");
                
            }

            dd.Body = body.ToString();
            string FilePath = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\" + sLogFileName;
            ZAAttachment zdwLog = new ZAAttachment();
            zdwLog.AttachmentName = FilePath;
            zdwLog.AttachmentType = SMTPMailer.Text;
            zdwLog.FullAttachmentPath = FilePath;
            dd.AttachmentList.Add(zdwLog);

            dd.SendMail();

        }


        #endregion

    }
}
