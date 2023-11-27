using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace AIS.NDICExtract
{
    class NDICExtractInterface
    {
        string sSQLServerConnectString = ConfigurationManager.ConnectionStrings["SQLServerConnectString"].ToString();
        string sAISNDICLogFolder = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\";
        string sJobSuccessCode = ConfigurationManager.AppSettings["JobSuccessCode"].ToString();

        public bool GetNDICExtract()
        {
            bool blnUpdateStatus = false;
            int NDICGenID = 0;

            try
            {
                NDICGenID = GetNDICGenID();
                WriteFileLogFile("Stored Procedure call 'NDIC_AIS_Extract' has been started.....");
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandTimeout = 7200;
                    //conn.ConnectionString = sSQLServerConnectString;
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "NDIC_AIS_Extract";
                    sqlCmd.Parameters.AddWithValue("@NDICGenID", NDICGenID);
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                blnUpdateStatus = false;
                WriteFileLogFile(ex.Message);

            }
            finally
            {
                int fkNDICGenStatusID = GetStatusForNDICGenID(NDICGenID);
                if (fkNDICGenStatusID == Convert.ToInt32(sJobSuccessCode))
                {
                    blnUpdateStatus = true;
                    WriteFileLogFile("NDIC AIS Extract has been Completed..");
                }
                else
                {
                    blnUpdateStatus = false;
                    WriteFileLogFile("Job failed..");
                    WriteFileLogFile("Job failure status code :" + fkNDICGenStatusID);

                    string GetErrormessageAndDescription = GetErrorDescriptionAndMessageForNDICGenID(NDICGenID);
                    if (GetErrormessageAndDescription != string.Empty)
                    {
                        string[] strErrormessageAndDescription = GetErrormessageAndDescription.Split('|');

                        WriteFileLogFile("Job failure ErrorDescription :" + strErrormessageAndDescription[0]);
                        WriteFileLogFile("Job failure ErrorMessage :" + strErrormessageAndDescription[1]);
                    }

                }
            }
            return blnUpdateStatus;

        }

        public int GetNDICGenID()
        {
            int NDICGenID = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandTimeout = 7200;
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "NDIC_GetNDICGenID";
                    sqlCmd.Parameters.AddWithValue("@RequestedBy", "NDIC Batch Executable");
                    SqlParameter RuturnValue = new SqlParameter("@NDICGenId", SqlDbType.Int);
                    RuturnValue.Direction = ParameterDirection.Output;
                    sqlCmd.Parameters.Add(RuturnValue);
                    sqlCmd.ExecuteNonQuery();
                    NDICGenID = (int)sqlCmd.Parameters["@NDICGenId"].Value;

                    sqlCmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                WriteFileLogFile("An error occurred on NDIC ID generation");
                WriteFileLogFile(ex.Message);

            }
            return NDICGenID;
        }

        public int GetStatusForNDICGenID(int NDICGenID)
        {
            int fkNDICGenStatusID = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    conn.Open();
                    SqlCommand sqlcmd = new SqlCommand("select fkNDICGenStatusID from NDIC_Gen where ID=@0", conn);
                    sqlcmd.Parameters.Add(new SqlParameter("0", NDICGenID));
                    using (SqlDataReader reader = sqlcmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fkNDICGenStatusID = (int)reader[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteFileLogFile("An Error occured in getting NDICGEN ID status.");
                WriteFileLogFile("Job failure status code :" + fkNDICGenStatusID);
                WriteFileLogFile(ex.Message);
            }
            return fkNDICGenStatusID;
        }

        public string GetErrorDescriptionAndMessageForNDICGenID(int NDICGenID)
        {
            string fkNDICGenErrormessage = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(sSQLServerConnectString))
                {
                    conn.Open();
                    SqlCommand sqlcmd = new SqlCommand("select ErrorDescription + '|'+ ErrorMessage from NDICGen_Error where fkNDICGenID =@0", conn);
                    sqlcmd.Parameters.Add(new SqlParameter("0", NDICGenID));
                    using (SqlDataReader reader = sqlcmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fkNDICGenErrormessage = (string)reader[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteFileLogFile("An error occurred on getting fkNDICGenErrormessage.");
                WriteFileLogFile(ex.Message);
            }
            return fkNDICGenErrormessage;
        }

        public void WriteFileLogFile(string entry)
        {
            string FilePath;
            if (ConfigurationManager.AppSettings["LogFile"] == null)
            {
                FilePath = "c:\\AIS\\AISNDICExtractLogFile";
            }
            else
            {
                FilePath = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\AISNDICExtractLog.txt";
            }
            Directory.CreateDirectory(sAISNDICLogFolder);
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
            catch (Exception)
            {

            }
        }

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
                dd.Subject = ConfigurationManager.AppSettings["AISNDICExtractSuccessSubject"].ToString();
            else
                dd.Subject = ConfigurationManager.AppSettings["AISNDICExtractFailureSubject"].ToString();

            StringBuilder body = new StringBuilder();

            body.AppendLine("");

            if (status)
                body.AppendLine(ConfigurationManager.AppSettings["AISNDICExtractSuccessBody"].ToString());
            else
            {
                body.AppendLine(ConfigurationManager.AppSettings["AISNDICExtractFailureBody"].ToString());
                //body.AppendLine("Please see the below error details");
                //body.AppendLine(ErrorMsg);
            }

            dd.Body = body.ToString();

            dd.SendMail();

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
