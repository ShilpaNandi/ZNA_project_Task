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
using System.Text.RegularExpressions;

namespace AIS.ZNAWNDICInterface
{
    class ZNAWInterface
    {
        string sSQLServerConnectString = ConfigurationManager.ConnectionStrings["SQLServerConnectString"].ToString();
        string sZNAWOutputFolder = ConfigurationManager.AppSettings["ZNAWOutputFolder"].ToString() + "\\";
        string sZNAWOutputArchiveFolder = ConfigurationManager.AppSettings["ZNAWOutputArchiveFolder"].ToString() + DateTime.Now.ToString("MM_dd_yyyy") + "(" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ")" + "\\";
        string sNDICPgmPrdFileName = ConfigurationManager.AppSettings["NDICPgmPrdFileName"].ToString();
        string sNDICPgmPrdControlBalanceFileName = ConfigurationManager.AppSettings["NDICpgmPrdControlBalanceFileName"].ToString();
        string sNDICPolicyFileName = ConfigurationManager.AppSettings["NDICPolicyFileName"].ToString();
        string sNDICPolicyControlBalanceFileName = ConfigurationManager.AppSettings["NDICPolicyControlBalanceFileName"].ToString();
        string sZNAWLogFolder = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\";
        string ZNAWEmailNotification = ConfigurationManager.AppSettings["EmailNotificationStatus"].ToString();
        string sControlBalanceDataEmailPrint = string.Empty;
        string sControlBalancePolicyDataEmailPrint = string.Empty;


        public bool WriteZNAWData()
        {
            bool returnValue = true;
            try
            {
                if (WriteProgramPeriodNDICFile() == true && WritePolicyNDICFile() == true)
                {
                  if (WriteProgramPeriodControlBalanceFile() == true && WritePolicyControlBalanceFile() == true)
                  {

                    if (UpdateStatus() == true)
                    {
                        WriteFileLogFile("Control Balance and NDIC Data successfully loaded!");
                        if (Archivefiles() == true)
                        {
                            WriteFileLogFile("Archive File Creation successfull!");
                            WriteFileLogFile("Job Completed Successfully..");
                            if (ZNAWEmailNotification == "True")
                            {
                                SendStatusMail("", true);
                            }
                            returnValue = true;
                        }
                        else
                        {
                            WriteFileLogFile("Archive File Creation unsuccessfull!");
                            if (ZNAWEmailNotification == "True")
                            {
                                SendStatusMail("Archive File Creation unsuccessfull", false);
                            }
                            returnValue = false;
                        }
                    }
                    else
                    {
                        WriteFileLogFile("Control Balance and NDIC Data successfully loaded!");
                        WriteFileLogFile("NDIC Tramsmittal Status update unsuccessful!");
                        if (ZNAWEmailNotification == "True")
                        {
                            SendStatusMail("NDIC Tramsmittal Status update unsuccessful!", false);
                        }
                        returnValue = false;
                    }
                  }
                  else
                   {
                    WriteFileLogFile("NDIC Program Period Control Balance File Creation Unsuccessfull!");
                    WriteFileLogFile("NDIC Policy Control Balance File Creation Unsuccessfull!");

                    //WriteFileLogFile("NDIC Program Period Control Balance File Creation Unsuccessfull!");

                    if (ZNAWEmailNotification == "True")
                    {
                        SendStatusMail("NDIC Program Period File and NDIC Policy File Creation Unsuccessfull!", false);
                    }
                    returnValue = false;
                }
              
                }
                else
                {
                    WriteFileLogFile("NDIC Program Period File Creation Unsuccessfull!");
                    WriteFileLogFile("NDIC Policy File Creation Unsuccessfull!");

                    //WriteFileLogFile("NDIC Program Period Control Balance File Creation Unsuccessfull!");

                    if (ZNAWEmailNotification == "True")
                    {
                        SendStatusMail("NDIC Program Period File and NDIC Policy File Creation Unsuccessfull!", false);
                    }
                    returnValue = false;
                }
            
            }
            catch (Exception ex)
            {
                WriteFileLogFile(ex.Message);
                returnValue = false;
            }
            return returnValue;
        }

        private bool WriteProgramPeriodNDICFile()
        {
            bool blnNDICPgmPrdFileStatus = true;
            FileStream fstr = null;

            WriteFileLogFile("NDIC Program Period File Creation has been Started..");
            try
            {
                //*****************************************************
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    //SqlCommand sqlCmd = new SqlCommand("dbo.usp_SFI_Transmittal", conn);
                    SqlCommand sqlCmd = new SqlCommand("dbo.NDIC_PgmPrd_Transmittal", conn);
                    sqlCmd.CommandTimeout = 7200;
                    //conn.ConnectionString = sSQLServerConnectString;
                    conn.Open();
                    //sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = sqlCmd.ExecuteReader();
                    sqlCmd.Dispose();



                    //*****************************************************
                    //Create Archive folder, overwrite if exists already
                    Directory.CreateDirectory(sZNAWOutputFolder);

                    fstr = new FileStream(sZNAWOutputFolder + sNDICPgmPrdFileName, System.IO.FileMode.Create);

                    StreamWriter wOut = new StreamWriter(fstr, System.Text.Encoding.ASCII);

                    int ColumnCount = rdr.FieldCount;
                    string ListOfColumns = string.Empty;
                    while (rdr.Read())
                    {
                        wOut.WriteLine(rdr["Result"].ToString());
                    }

                    wOut.Close();
                    fstr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                blnNDICPgmPrdFileStatus = false;
                //Write to log File

                WriteFileLogFile("Exception in NDIC Program Period File Creation!");
                WriteFileLogFile(ex.Message);


            }

            if (blnNDICPgmPrdFileStatus == true)
            {
                WriteFileLogFile("NDIC Program Period File Creation '" + fstr.Name.ToString() + "' Completed Successfully");
            }

            return blnNDICPgmPrdFileStatus;
        }

        private bool WritePolicyNDICFile()
        {
            bool blnNDICPolicyFileStatus = true;
            FileStream fstr = null;

            WriteFileLogFile("NDIC Policy File Creation has been Started..");
            try
            {
                //*****************************************************
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    
                    SqlCommand sqlCmd = new SqlCommand("dbo.NDIC_Policy_Transmittal", conn);
                    sqlCmd.CommandTimeout = 7200;
                    //conn.ConnectionString = sSQLServerConnectString;
                    conn.Open();
                    //sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = sqlCmd.ExecuteReader();
                    sqlCmd.Dispose();



                    //*****************************************************
                    //Create Archive folder, overwrite if exists already
                    //Directory.CreateDirectory(sZNAWOutputFolder);

                    fstr = new FileStream(sZNAWOutputFolder + sNDICPolicyFileName, System.IO.FileMode.Create);

                    StreamWriter wOut = new StreamWriter(fstr, System.Text.Encoding.ASCII);

                    int ColumnCount = rdr.FieldCount;
                    string ListOfColumns = string.Empty;
                    while (rdr.Read())
                    {
                        wOut.WriteLine(rdr["Result"].ToString());
                    }

                    wOut.Close();
                    fstr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                blnNDICPolicyFileStatus = false;
                //Write to log File

                WriteFileLogFile("Exception in NDIC Policy File Creation!");
                WriteFileLogFile(ex.Message);


            }

            if (blnNDICPolicyFileStatus == true)
            {
                WriteFileLogFile("NDIC Policy File Creation '" + fstr.Name.ToString() + "' Completed Successfully");
            }

            return blnNDICPolicyFileStatus;
        }

        private bool WriteProgramPeriodControlBalanceFile()
        {
            bool blnControlBalanceFileStatus = true;
            FileStream fstr = null;

            WriteFileLogFile("Control Balance File Creation for Program Period has been Started..");
            try
            {
                //*****************************************************
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    SqlCommand sqlCmd = new SqlCommand("dbo.NDIC_PgmPrd_Transmittal_ControlBalance", conn);
                    sqlCmd.CommandTimeout = 7200;
                    //conn.ConnectionString = sSQLServerConnectString;
                    conn.Open();
                    //sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = sqlCmd.ExecuteReader();
                    sqlCmd.Dispose();
                    //*****************************************************

                    //Create folder, overwrite if exists already
                    Directory.CreateDirectory(sZNAWOutputFolder);


                    fstr = new FileStream(sZNAWOutputFolder + sNDICPgmPrdControlBalanceFileName, System.IO.FileMode.Create);

                    StreamWriter wOut = new StreamWriter(fstr, System.Text.Encoding.ASCII);

                    StringBuilder strControlBalanceResult = new StringBuilder();

                    int count = 0;

                    while (rdr.Read())
                    {
                        wOut.WriteLine(rdr["Result"].ToString());

                        if (count == 0)
                        {
                            //strControlBalanceResult.Append("********* Control Balance File Data **********");
                            strControlBalanceResult.Append(Environment.NewLine);
                            strControlBalanceResult.Append(rdr["Result"].ToString());
                            strControlBalanceResult.Append(Environment.NewLine);
                        }
                        else
                        {
                            strControlBalanceResult.Append(rdr["Result"].ToString());
                            strControlBalanceResult.Append(Environment.NewLine);
                        }

                        count = count + 1;
                    }

                    WriteFileLogFile(strControlBalanceResult.ToString());
                    if (strControlBalanceResult != null)
                    {
                        sControlBalanceDataEmailPrint = strControlBalanceResult.ToString();
                    }

                    wOut.Close();
                    fstr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                blnControlBalanceFileStatus = false;
                //Write to log File

                WriteFileLogFile("Exception in Control Balance File Creation!");
                WriteFileLogFile(ex.Message);

            }

            if (blnControlBalanceFileStatus == true)
            {
                WriteFileLogFile("Program Period Control Balance File Creation '" + fstr.Name.ToString() + "' Completed Successfully");
            }
            return blnControlBalanceFileStatus;
        }

        private bool WritePolicyControlBalanceFile()
        {
            bool blnControlBalancePolicyFileStatus = true;
            FileStream fstr = null;

            WriteFileLogFile("Control Balance File Creation for Policy has been Started..");
            try
            {
                //*****************************************************
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    SqlCommand sqlCmd = new SqlCommand("dbo.NDIC_Policy_Transmittal_ControlBalance", conn);
                    sqlCmd.CommandTimeout = 7200;
                    //conn.ConnectionString = sSQLServerConnectString;
                    conn.Open();
                    //sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader rdr = sqlCmd.ExecuteReader();
                    sqlCmd.Dispose();
                    //*****************************************************

                    //Create folder, overwrite if exists already
                    //Directory.CreateDirectory(sZNAWOutputFolder);

                    fstr = new FileStream(sZNAWOutputFolder + sNDICPolicyControlBalanceFileName, System.IO.FileMode.Create);

                    StreamWriter wOut = new StreamWriter(fstr, System.Text.Encoding.ASCII);

                    StringBuilder strPolicyControlBalanceResult = new StringBuilder();

                    int count = 0;

                    while (rdr.Read())
                    {
                        wOut.WriteLine(rdr["Result"].ToString());

                        if (count == 0)
                        {
                            //strControlBalanceResult.Append("********* Control Balance File Data **********");
                            strPolicyControlBalanceResult.Append(Environment.NewLine);
                            strPolicyControlBalanceResult.Append(rdr["Result"].ToString());
                            strPolicyControlBalanceResult.Append(Environment.NewLine);
                        }
                        else
                        {
                            strPolicyControlBalanceResult.Append(rdr["Result"].ToString());
                            strPolicyControlBalanceResult.Append(Environment.NewLine);
                        }

                        count = count + 1;
                    }

                    WriteFileLogFile(strPolicyControlBalanceResult.ToString());
                    if (strPolicyControlBalanceResult != null)
                    {
                        sControlBalancePolicyDataEmailPrint = strPolicyControlBalanceResult.ToString();
                    }

                    wOut.Close();
                    fstr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                blnControlBalancePolicyFileStatus = false;
                //Write to log File

                WriteFileLogFile("Exception in Policy Control Balance File Creation!");
                WriteFileLogFile(ex.Message);

            }

            if (blnControlBalancePolicyFileStatus == true)
            {
                WriteFileLogFile("Control Balance File Creation '" + fstr.Name.ToString() + "' Completed Successfully");
            }
            return blnControlBalancePolicyFileStatus;
        }

        private bool UpdateStatus()
        {
            bool blnUpdateStatus = true;
            try
            {
                #region fnSFITramsmittalStatusupdate_comment
                //This function will update various statuses
                #endregion

                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandTimeout = 7200;
                    //conn.ConnectionString = sSQLServerConnectString;
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    WriteFileLogFile("Stored Procedure call '" + "dbo.NDIC_update_Transmittal_Status" + "'.....");
                    sqlCmd.CommandText = "dbo.NDIC_update_Transmittal_Status";
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                blnUpdateStatus = false;

                WriteFileLogFile("Exception in NDIC Tramsmittal Status update!");
                WriteFileLogFile(ex.Message);

            }
            if (blnUpdateStatus == true)
            {
                WriteFileLogFile("Stored Procedure call '" + "dbo.NDIC_update_Transmittal_Status" + "' Successful");
            }
            return blnUpdateStatus;

        }
        public bool Archivefiles()
        {
            bool ZNAWArchiveFile = true;

            try
            {
                System.IO.Directory.CreateDirectory(sZNAWOutputArchiveFolder);
                string[] filePaths = Directory.GetFiles(sZNAWOutputFolder);
                foreach (var filename in filePaths)
                {
                    string file = filename.ToString();

                    string destinationFile = string.Empty;
                    if (file.Contains(sNDICPgmPrdControlBalanceFileName))
                    {
                        destinationFile = sZNAWOutputArchiveFolder + sNDICPgmPrdControlBalanceFileName;
                        File.Copy(file, destinationFile, true);
                        WriteFileLogFile("Archive File Creation path for Program Period Control Balance Data '" + destinationFile.ToString() + "'");
                    }
                    if (file.Contains(sNDICPgmPrdFileName))
                    {
                        destinationFile = sZNAWOutputArchiveFolder + sNDICPgmPrdFileName;
                        File.Copy(file, destinationFile, true);
                        WriteFileLogFile("Archive File Creation path for NDIC Program Period Data '" + destinationFile.ToString() + "'");
                    }
                    if (file.Contains(sNDICPolicyControlBalanceFileName))
                    {
                        destinationFile = sZNAWOutputArchiveFolder + sNDICPolicyControlBalanceFileName;
                        File.Copy(file, destinationFile, true);
                        WriteFileLogFile("Archive File Creation path for Policy Control Balance Data '" + destinationFile.ToString() + "'");
                    }
                    if (file.Contains(sNDICPolicyFileName))
                    {
                        destinationFile = sZNAWOutputArchiveFolder + sNDICPolicyFileName;
                        File.Copy(file, destinationFile, true);
                        WriteFileLogFile("Archive File Creation path for NDIC Policy Data '" + destinationFile.ToString() + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                ZNAWArchiveFile = false;
                WriteFileLogFile("An error occurred on Archive files Creation!");
                WriteFileLogFile(ex.Message);
            }
            return ZNAWArchiveFile;
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
                if (strEmailList[i].ToString() != string.Empty)
                {
                    if (EmailIsValid(strEmailList[i].ToString()))
                    {
                        objMailList.Add(strEmailList[i].ToString());
                    }
                    else
                    {
                        WriteFileLogFile("Invalid Email Address found :" + strEmailList[i].ToString());
                    }
                }
            }

            if (status)
                dd.Subject = ConfigurationManager.AppSettings["AISNDICTransmittalSuccessSubject"].ToString();
            else
                dd.Subject = ConfigurationManager.AppSettings["AISNDICTransmittalFailureSubject"].ToString();

            StringBuilder body = new StringBuilder();

            body.AppendLine("");

            if (status)
            {
                body.AppendLine(ConfigurationManager.AppSettings["AISNDICTransmittalSuccessBody"].ToString());
                if (sControlBalanceDataEmailPrint != string.Empty)
                {
                    body.Append(Environment.NewLine);
                    body.AppendLine("*************NDIC Program Period Control Balance File Data **************");

                    body.AppendLine(sControlBalanceDataEmailPrint);
                }

                if (sControlBalancePolicyDataEmailPrint != string.Empty)
                {
                    body.Append(Environment.NewLine);
                    body.AppendLine("*************NDIC Policy Control Balance File Data **************");

                    body.AppendLine(sControlBalancePolicyDataEmailPrint);
                }


            }
            else
            {
                body.AppendLine(ConfigurationManager.AppSettings["AISNDICTransmittalFailureBody"].ToString());
                body.AppendLine(ErrorMsg.ToString());
                if (sControlBalanceDataEmailPrint != string.Empty)
                {
                    body.Append(Environment.NewLine);
                    body.AppendLine("************* *NDIC Program Period Control Balance File Data **************");

                    body.AppendLine(sControlBalanceDataEmailPrint);
                }
                if (sControlBalancePolicyDataEmailPrint != string.Empty)
                {
                    body.Append(Environment.NewLine);
                    body.AppendLine("*************NDIC Policy Control Balance File Data **************");

                    body.AppendLine(sControlBalancePolicyDataEmailPrint);
                }
            }

            dd.Body = body.ToString();

            dd.SendMail();

        }
        #endregion
        public void WriteFileLogFile(string entry)
        {
            string FilePath;
            if (ConfigurationManager.AppSettings["LogFile"] == null)
            {
                FilePath = "c:\\AIS\\ZNWLogFile";
            }
            else
            {
                FilePath = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\ZNAWLog.txt";
            }
            Directory.CreateDirectory(sZNAWLogFolder);
            string fullentry = DateTime.Now.ToShortDateString().ToString() + " " +
                                DateTime.Now.ToLongTimeString().ToString() +
                                ":   " + entry;
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
        public bool EmailIsValid(string email)
        {
            try
            {
                string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

                if (Regex.IsMatch(email, expression))
                {
                    if (Regex.Replace(email, expression, string.Empty).Length == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteFileLogFile(ex.Message);
                return false;
            }
        }
    }
}
