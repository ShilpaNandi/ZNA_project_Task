using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Diagnostics;

namespace ZurichNA.AIS.ARiES.Interface
{
    class ARiESInterface
    {
        //1. Read the Retro Configuration file for the File Locations
        //Move to the main Config manager class...
        string sSQLServerConnectString = ConfigurationManager.ConnectionStrings["SQLServerConnectString"].ToString();
        string sARiESOutputFolder = ConfigurationManager.AppSettings["ARiESOutputFolder"].ToString() + DateTime.Today.ToString("MM-dd-yyyy") + "\\";
        string sARiESFileName = ConfigurationManager.AppSettings["ARiESFileName"].ToString() + "_" + DateTime.Today.ToString("MMddyyyy") + ".txt";
        string sARiESFTPFileName = ConfigurationManager.AppSettings["ARiESFTPFileName"].ToString();
        string sARiESFTPServer = ConfigurationManager.AppSettings["ARiESFTPServer"].ToString();
        string sARiESFTPUserName = ConfigurationManager.AppSettings["ARiESFTPUserName"].ToString();
        string sARiESFTPPassword = ConfigurationManager.AppSettings["ARiESFTPPassword"].ToString();
        string sARiESLiteral = ConfigurationManager.AppSettings["ARiESLiteral"].ToString();

        public void WriteARiESData()
        {
            //If the process of loading the ARMIS files into the holding tables does not error out,
            //then proceed to load them in the 2 LOSS tables...a false returned value indicates that no errors were encountered
            if (WriteARiESFile() == true)//  if (WriteARiESFile() == True)
            {
                //On Successful load, upload to FTP server.
                if (UploadARiESFile(sARiESOutputFolder + "\\" + sARiESFileName) == true)
                {
                     //On Successful upload to FTP server, update statuses
                    if (UpdateStatus() == true)
                    {
                        //Write to log table
                        WriteLog("ARiES Interface", "Inf", "ARiES data successfully loaded!", "ARiES files successfully loaded!", 1);
                    }
                }
            }
        }

       private bool WriteARiESFile()
        {
            bool blnARiESFileError = true;

            //*****************************************************
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandTimeout = 7200;
            conn.ConnectionString = sSQLServerConnectString;
            conn.Open();
            sqlCmd.Connection = conn;
            sqlCmd.CommandText = "ModAIS_SendTransmittalToARiES";
            SqlDataReader rdr = sqlCmd.ExecuteReader();
            sqlCmd.Dispose();
            //*****************************************************

            try
            {
                //Create Archive folder, overwrite if exists already
                Directory.CreateDirectory(sARiESOutputFolder);

                FileStream fstr = new FileStream(sARiESOutputFolder + sARiESFileName, System.IO.FileMode.Create);
                StreamWriter wOut = new StreamWriter(fstr, System.Text.Encoding.ASCII);

                while (rdr.Read())
                {
                    wOut.WriteLine(rdr["AISRMDPRecord"].ToString());
                }

                wOut.Close();
                fstr.Close();
                conn.Close();
            }
            catch
            {
                blnARiESFileError = false;
                //Write to log table
                WriteLog("ARiES Interface", "Inf", "ARiES File Creation Unsuccessfull!", "ARiES File Creation Unsuccessfull!", 1);
            }

            return blnARiESFileError;
        }

       private bool UpdateStatus()
       {
           bool blnUpdateStatus = true;
           try
                {
              #region fnLoadARMISData_comment
               //This function will update various statuses
              #endregion

               SqlConnection conn = new SqlConnection();
               SqlCommand sqlCmd = new SqlCommand();
               sqlCmd.CommandTimeout = 7200;
               conn.ConnectionString = sSQLServerConnectString;
               conn.Open();
               sqlCmd.Connection = conn;

               sqlCmd.CommandText = "ModAIS_ConfirmTransmittalSentToARiES";
               sqlCmd.ExecuteNonQuery();
               sqlCmd.Dispose();
                }
            catch
             {
                 blnUpdateStatus = false;
                //Write to log table
                 WriteLog("ARiES Interface", "Inf", "ARiES Status update unsuccessful!", "ARiES Status update unsuccessful!", 1);
             }
           return blnUpdateStatus;

       }

       private bool UploadARiESFile(string filename)
       {
           bool blnUploadARiESFileError = true;

           string sARiESFileFullFilePath = sARiESOutputFolder + sARiESFileName;
           string sbatchFileName = "";
           string sbatchFileFullPath;
           FileStream fARiESFileStream;
           FileInfo fARiESFileinfo;
           StreamWriter wARiESStreamWriter;
           string sscriptFileName = "ARiESTransmittalScript.TXT";
           string sscriptFileFullPath = sARiESOutputFolder + sscriptFileName;

           try
           {
               using (fARiESFileStream = File.Create(sscriptFileFullPath))
               {
               }
               fARiESFileStream.Close();

               fARiESFileinfo = new FileInfo(sscriptFileFullPath);

               if (fARiESFileinfo.Exists)
               {

                   wARiESStreamWriter = fARiESFileinfo.CreateText();
                   wARiESStreamWriter.WriteLine("Open " + sARiESFTPServer);
                   wARiESStreamWriter.WriteLine("User " + sARiESFTPUserName + " " + sARiESFTPPassword);
                   wARiESStreamWriter.WriteLine(sARiESLiteral);
                   wARiESStreamWriter.WriteLine("put " + sARiESFileFullFilePath + " '" + sARiESFTPFileName + "'");
                   //wARiESStreamWriter.WriteLine("pause");
                   wARiESStreamWriter.WriteLine("quit");
                   wARiESStreamWriter.Close();

                   sbatchFileName = "AISTransmittalData" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() +
                    DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".bat";

                   sbatchFileFullPath = sARiESOutputFolder + sbatchFileName;

                   using (fARiESFileStream = File.Create(sbatchFileFullPath))
                   {
                   }
                   fARiESFileStream.Close();

                   fARiESFileinfo = new FileInfo(sbatchFileFullPath);
                   wARiESStreamWriter = fARiESFileinfo.CreateText();
                   wARiESStreamWriter.WriteLine("ftp -n -i -s:" + sscriptFileFullPath);
                   wARiESStreamWriter.Close();
                   Process pARiES = new Process();

                   pARiES.StartInfo = new ProcessStartInfo(sbatchFileFullPath);
                   pARiES.StartInfo.UseShellExecute = true;
                   pARiES.Start();
               }
               else
               {
                   blnUploadARiESFileError = false;
                   //Write to log table
                   WriteLog("ARiES Interface", "Inf", "ARiES file does not exist - Upload Unsuccessfull!", "ARiES file does not exist - Upload Unsuccessfull!", 1);
               }
           }
           catch
           {
               blnUploadARiESFileError = false;
               //Write to log table
               WriteLog("ARiES Interface", "Inf", "ARiES FTP File Upload Unsuccessfull!", "ARiES FTP File Upload Unsuccessfull!", 1);
           }
           return blnUploadARiESFileError;
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
            sqlCmd.CommandTimeout = 7200;
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
                FilePath = "d:\\ARIESLog.txt";
            }
            else
            {
                FilePath = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\ARIESLog.txt";
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
    }
}
