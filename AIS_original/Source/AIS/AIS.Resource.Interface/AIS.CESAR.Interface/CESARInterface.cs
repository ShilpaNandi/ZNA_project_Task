using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Diagnostics;

namespace ZurichNA.AIS.CESAR.Interface
{
    class CESARInterface
    {
        string sSQLServerConnectString = ConfigurationManager.ConnectionStrings["SQLServerConnectString"].ToString();
        string sCESAROutputFolder = ConfigurationManager.AppSettings["CESAROutputFolder"].ToString() + DateTime.Today.ToString("MM-dd-yyyy") + "\\";
        string sCESARFileName = ConfigurationManager.AppSettings["CESARFileName"].ToString() + "_" + DateTime.Today.ToString("MMddyyyy") + ".txt";
        string sCESARFTPFileName = ConfigurationManager.AppSettings["CESARFTPFileName"].ToString();
        string sCESARFTPServer = ConfigurationManager.AppSettings["CESARFTPServer"].ToString();
        string sCESARFTPUserName = ConfigurationManager.AppSettings["CESARFTPUserName"].ToString();
        string sCESARFTPPassword = ConfigurationManager.AppSettings["CESARFTPPassword"].ToString();
        string sCESARLiteral = ConfigurationManager.AppSettings["CESARLiteral"].ToString();



        public void WriteCESARData()
        {
            CheckFileExists();

            if (WriteCESARFile() == true)//  if (WriteCESARFile() == True)
            {
                //On Successful load, upload to FTP server.
                if (UploadCESARFile(sCESAROutputFolder + "\\" + sCESARFileName) == true)
                {
                    //On Successful upload to FTP server, update statuses
                    if (UpdateStatus() == true)
                    {
                        //Write to log table
                        WriteLog("CESAR Interface", "Inf", "CESAR data successfully loaded!", "CESAR files successfully loaded!", 1);
                        SendStatusMail("", true);
                    }
                }
            }


        }

        private void CheckFileExists()
        {
            
            string strReadfile = this.PerformFTPProcess();
            string msg="";
            if (strReadfile.Contains("List completed successfully"))
            {
                msg = "File already exists on the mainframe or FTP server, " + sCESARFTPFileName + "";
                //WriteFileLogFile(msg);
                WriteLog("CESAR Interface", "Inf", "File Already Exists", msg, 1);
                throw (new Exception(msg));
            }                      
        }

        private string PerformFTPProcess()
        {
            string InstructionFileName = "";
            //string FileName = "";
            string FileFullPath = "";
            string batchFileName = "";
            string batchFileFullPath = "";
            string LogFilePath = "";
            string strReadFile = "";
            FileInfo fileinfo;
            StreamWriter w;
            FileStream fs;
            TextReader tr;
            WriteFileLogFile("Checking if Destination (FTP Server) file exists '" + sCESARFTPFileName + "'...");
            InstructionFileName = "CheckExists_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss")
                + ".ftp.Instructions.txt";
            //ConfigurationManager.AppSettings["RMDP_Transmission0_FilePath"].ToString() +
            FileFullPath = sCESAROutputFolder + InstructionFileName;
            fs = null;

            if (!Directory.Exists(sCESAROutputFolder))
                Directory.CreateDirectory(sCESAROutputFolder);

            using (fs = File.Create(FileFullPath))
            {
            }
            fs.Close();
            fileinfo = new FileInfo(FileFullPath);
            w = fileinfo.CreateText();
            w.WriteLine("Open " + ConfigurationManager.AppSettings["CESARFTPServer"].ToString());
            w.WriteLine("User " + ConfigurationManager.AppSettings["CESARFTPUserName"].ToString() + " " +
                ConfigurationManager.AppSettings["CESARFTPPassword"].ToString());
            w.WriteLine("dir '" + sCESARFTPFileName + "'");
            w.WriteLine("quit");
            w.Close();

            batchFileName = "CheckExists_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".bat";
            batchFileFullPath = sCESAROutputFolder + batchFileName;
            using (fs = File.Create(batchFileFullPath))
            {
            }
            fs.Close();
            fileinfo = new FileInfo(batchFileFullPath);
            w = fileinfo.CreateText();
            w.WriteLine("ftp -n -i -s:\"" + FileFullPath + "\" > \"" + FileFullPath.Replace("ftp.Instructions.txt", "ftp.log") + "\"");
            w.Close();



            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(batchFileFullPath);

                //p.StartInfo.UseShellExecute = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();
            }
            catch (Exception ex)
            {
                //WriteLog("CESAR Interface", "Inf", "Error executing FTP Process shell script", ex.ToString(), 1);
                WriteFileLogFile("Error executing FTP Process shell script, " + ex.ToString());
                throw ex;
            }

            bool blnFTPComplete = false;
            LogFilePath = FileFullPath.Replace("Instructions.txt", "log");
            while (blnFTPComplete == false)
            {
                try
                {
                    using (tr = new StreamReader(LogFilePath))
                    {
                        strReadFile = tr.ReadToEnd();
                        blnFTPComplete = true;
                    }
                }
                catch //(Exception ex)
                {
                    blnFTPComplete = false;
                }
            }
            return strReadFile;
        }
       
        private bool GetCodingToCesar()
        {
            bool blnCodingToCesar = true;
            try
            {
                SqlConnection conn = new SqlConnection();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = 0;
                conn.ConnectionString = sSQLServerConnectString;
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ModAIS_CodingToCesar";
                sqlCmd.Parameters.AddWithValue("@create_user_id", 99999);
                SqlParameter parmErr = new SqlParameter("@err_msg_output", SqlDbType.VarChar);
                parmErr.Direction = ParameterDirection.Output;
                parmErr.Size = 50;
                sqlCmd.Parameters.Add(parmErr);
                string strErr = string.Empty;
                sqlCmd.ExecuteNonQuery();
                strErr = Convert.ToString(sqlCmd.Parameters["@err_msg_output"].Value);
                sqlCmd.Dispose();
                conn.Close();
                if (!string.IsNullOrWhiteSpace(strErr))
                {
                    blnCodingToCesar = false;
                    WriteLog("CESAR Interface", "Inf", "Getting data for Coding to CESAR unsuccessful!", strErr, 1);
                }
            }
            catch(Exception ex)
            {
                blnCodingToCesar = false;
                //Write to log table
                WriteLog("CESAR Interface", "Inf", "Getting data for Coding to CESAR unsuccessful!", ex.InnerException.ToString(), 1);
                throw ex;
            }
            return blnCodingToCesar;

        }

        private bool WriteCESARFile()
        {
            bool blnCESARFileError = true;

            //*****************************************************
            SqlConnection conn = new SqlConnection(sSQLServerConnectString);
            SqlCommand sqlCmd = new SqlCommand("ModAIS_SendCodingToCesar", conn);
            sqlCmd.CommandTimeout = 0;
            //conn.ConnectionString = sSQLServerConnectString;
            conn.Open();
            //sqlCmd.Connection = conn;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = sqlCmd.ExecuteReader();
            sqlCmd.Dispose();
            //*****************************************************

            try
            {
                //Create Archive folder, overwrite if exists already
                Directory.CreateDirectory(sCESAROutputFolder);
                FileStream fstr = null;
                fstr = new FileStream(sCESAROutputFolder + sCESARFileName, System.IO.FileMode.Create);
                StreamWriter wOut = new StreamWriter(fstr, System.Text.Encoding.ASCII);

                while (rdr.Read())
                {
                    wOut.WriteLine(rdr["AISCESARRecord"].ToString());
                }

                wOut.Close();
                fstr.Close();
                conn.Close();
            }
            catch(Exception ex)
            {
                blnCESARFileError = false;
                //Write to log table
                WriteLog("CESAR Interface", "Inf", "CESAR File Creation Unsuccessfull!", ex.ToString(), 1);
                throw ex;
            }

            return blnCESARFileError;
        }

        private bool UpdateStatus()
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
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ModAIS_ConfirmSentToCESAR";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                conn.Close();
            }
            catch(Exception ex)
            {
                blnUpdateStatus = false;
                //Write to log table
                WriteLog("CESAR Interface", "Inf", "CESAR Status update unsuccessful!",ex.ToString(), 1);
                throw ex;
            }
            return blnUpdateStatus;

        }

        private bool UploadCESARFile(string filename)
        {
            bool blnUploadCESARFileError = true;
            string sCESARFileFullFilePath = "";
            sCESARFileFullFilePath = sCESAROutputFolder + sCESARFileName;
            string sbatchFileName = "";
            string sbatchFileFullPath;
            FileStream fCESARFileStream;
            FileInfo fCESARFileinfo;
            StreamWriter wCESARStreamWriter;
            string sscriptFileName = "";
            sscriptFileName = "CESARCodingScript.TXT";
            string sscriptFileFullPath = sCESAROutputFolder + sscriptFileName;

            try
            {
                using (fCESARFileStream = File.Create(sscriptFileFullPath))
                {
                }
                fCESARFileStream.Close();

                fCESARFileinfo = new FileInfo(sscriptFileFullPath);

                if (fCESARFileinfo.Exists)
                {

                    wCESARStreamWriter = fCESARFileinfo.CreateText();
                    wCESARStreamWriter.WriteLine("Open " + sCESARFTPServer);
                    wCESARStreamWriter.WriteLine("User " + sCESARFTPUserName + " " + sCESARFTPPassword);
                    wCESARStreamWriter.WriteLine(sCESARLiteral);
                    wCESARStreamWriter.WriteLine("put " + sCESARFileFullFilePath + " '" + sCESARFTPFileName + "'");
                    //wCESARStreamWriter.WriteLine("pause");
                    wCESARStreamWriter.WriteLine("quit");
                    wCESARStreamWriter.Close();

                    sbatchFileName = "AISCESARData" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() +
                     DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".bat";

                    sbatchFileFullPath = sCESAROutputFolder + sbatchFileName;

                    using (fCESARFileStream = File.Create(sbatchFileFullPath))
                    {
                    }
                    fCESARFileStream.Close();

                    fCESARFileinfo = new FileInfo(sbatchFileFullPath);
                    wCESARStreamWriter = fCESARFileinfo.CreateText();
                    wCESARStreamWriter.WriteLine("ftp -n -i -s:" + sscriptFileFullPath);
                    wCESARStreamWriter.Close();
                    Process pCESAR = new Process();

                    pCESAR.StartInfo = new ProcessStartInfo(sbatchFileFullPath);
                    pCESAR.StartInfo.UseShellExecute = true;
                    pCESAR.Start();
                }
                else
                {
                    blnUploadCESARFileError = false;
                    //Write to log table
                    WriteLog("CESAR Interface", "Inf", "CESAR file does not exist - Upload Unsuccessfull!", "CESAR file does not exist - Upload Unsuccessfull!", 1);
                }
            }
            catch(Exception ex)
            {
                blnUploadCESARFileError = false;
                //Write to log table
                WriteLog("CESAR Interface", "Inf", "CESAR FTP File Upload Unsuccessfull!", ex.ToString(), 1);
                throw ex;

            }
            return blnUploadCESARFileError;
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
                FilePath = "d:\\CESARLog.txt";
            }
            else
            {
                FilePath = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\CESARLog.txt";
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
                //using (StreamWriter sw = new StreamWriter("d:\\ARMISLog.txt", true))
                //{
                //    sw.WriteLine("Failed writing to the default directory path");
                //    sw.WriteLine(fullentry);
                //    sw.Close();
                //}
            }
        }

        #region EMailNotification
        public void SendStatusMail(string ErrorMsg, bool status)
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
                dd.Subject = ConfigurationManager.AppSettings["CESARSuccessSubject"].ToString();
            else
                dd.Subject = ConfigurationManager.AppSettings["CESARFailureSubject"].ToString();

            StringBuilder body = new StringBuilder();

            body.AppendLine("");

            if (status)
                body.AppendLine(ConfigurationManager.AppSettings["CESARSuccessBody"].ToString());
            else
            {
                body.AppendLine(ConfigurationManager.AppSettings["CESARFailureBody"].ToString());
                body.AppendLine("Please see the below error details");
                body.AppendLine(ErrorMsg);
            }

            dd.Body = body.ToString();

            dd.SendMail();

        }
        #endregion
    }
}
