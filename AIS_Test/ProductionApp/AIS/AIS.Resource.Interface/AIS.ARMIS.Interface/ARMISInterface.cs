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


namespace ZurichNA.AIS.ARMIS.Interface
{
    public class ARMISInterface
    {
        //1. Read the Retro Configuration file for the File Locations
        //Move to the main Config manager class...
        string sSQLServerConnectString = ConfigurationManager.ConnectionStrings["SQLServerConnectString"].ToString();
        string sARMISInboundFolder = ConfigurationManager.AppSettings["ARMISInboundFolder"].ToString();
        string sARMISArchiveFolder = ConfigurationManager.AppSettings["ARMISArchiveFolder"].ToString();
        string ARMISFile37 = ConfigurationManager.AppSettings["ARMISFile37"].ToString();
        string ARMISFile38 = ConfigurationManager.AppSettings["ARMISFile38"].ToString();
        string ARMISFile39 = ConfigurationManager.AppSettings["ARMISFile39"].ToString();
        string sARMISHeaderTag = ConfigurationManager.AppSettings["ARMISHeaderTag"].ToString();
        string sARMISFooterTag = ConfigurationManager.AppSettings["ARMISFooterTag"].ToString();
        string strEmailIDs = ConfigurationManager.AppSettings["NotificationEmailIDs"].ToString();
        string strDate = DateTime.Now.ToString("MM-dd-yyyy HH:mm").Replace(":", "-");
       

        public void LoadARMISData()
        {
            //If the process of loading the ARMIS files into the holding tables does not error out,
            //then proceed to load them in the 2 LOSS tables...a false returned value indicates that no errors were encountered
            if (LoadARMISFiles() == false)
            {
                if (LoadARMISLossData()) //Loads ARMIS data into the two loss tables to be used for limiting.
                {

                    //Since import was successful, move the files to the archive folder and delete the files in daily basket.
                    ArchiveFiles();

                    //Write to log table
                    WriteLog("ARMIS Interface", "Inf", "ARMIS data successfully loaded!", "ARMIS files successfully loaded!", 1);
                    EMailNotification(true,"");
                    
                }
                else
                {
                    //On failure, move all the files to the Archive folder 
                    ArchiveFiles();
                    WriteLog("ARMIS Interface", "Inf", "LoadARMISLossData method failed", "LoadARMISLossData method failed", 1);
                    EMailNotification(false, "LoadARMISLossData method failed");
                }
            }
            //else
            //{
            //    //File not found error, file not openable, etc
            //    WriteLog("ARMIS Interface", "War", "ARMIS File not found!", "ARMIS File not found!", 1);
            //}
        }

        public void ArchiveFiles()
        {
            //Create Archive folder, overwrite if exists already
           
            //Directory.CreateDirectory(sARMISArchiveFolder + "\\" + DateTime.Today.ToString("MM-dd-yyyy"));
            Directory.CreateDirectory(sARMISArchiveFolder + "\\" + strDate);
            foreach (String Files in Directory.GetFiles(@sARMISInboundFolder))
            {
                FileInfo orgFile = new FileInfo(Files);
                //FileInfo desFile = new FileInfo(Files.Replace(sARMISInboundFolder, sARMISArchiveFolder + "\\" + DateTime.Today.ToString("MM-dd-yyyy")));
                FileInfo desFile = new FileInfo(Files.Replace(sARMISInboundFolder, sARMISArchiveFolder + "\\" + strDate));
                File.Copy(orgFile.FullName, desFile.FullName, true);
                File.Delete(orgFile.FullName);
            }
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

        private bool LoadARMISFiles()
        {
            #region COMMENTS_LoadARMISFile
            //5a. If only one of the 2 files exist then
            //Record this in the ARMIS Log table, with the datetime stamp, 
            //Mark the Processing Complete - ERROR  - ERRInterface_001
            //No data to Load



            //5b. [A.00.560.0] Read through the file ARMIS 37 and ARMIS 38, and 
            //if both have just the Header and Footer rows, 
            //Record this in the ARMIS Log table, with the datetime stamp.        
            //Mark the Processing Complete - SUCCESS
            //No data to Load

            //5c. [A.00.570.0] If File 37 has header and Footer but the File 38 
            //has the Footer missing
            //Record this in the ARMIS Log table, with the datetime stamp, 
            //Mark the Processing Complete - ERROR - ERRInterface_002
            //No data to Load

            #endregion

            //*****************************************************            
            //Remove all data from the interface tables ARMIS_INTRFC37 and ARMIS_INTRFC38 
            //before you start with ARMIS data upload.
            //*****************************************************
            try
            {
                SqlConnection conn = new SqlConnection();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = 7200;
                conn.ConnectionString = sSQLServerConnectString;
                conn.Open();
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "Delete from dbo.ARMIS_INTRFC37";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "Delete from dbo.ARMIS_INTRFC38";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
            }
            //*****************************************************

            catch (Exception ex)
            {
                this.WriteFileLogFile("LoadARMISFiles method failed");
                this.WriteFileLogFile(ex.Message);
            }

            //Variable Declaration
            bool blnFile37Exists = false;
            bool blnFile38Exists = false;
            //bool blnFile39Exists = false;
            bool blnARMISFileError = true;


            //Using the location (e.g. //INBOUND/ARMIS/Date/FileName ) from the config file, get all the files available
            //File Existance Check...if any one is missing then cancel the load process...
            if (File.Exists(sARMISInboundFolder + "\\" + ARMISFile37))
            {
                blnFile37Exists = true;
            }
            else
            {
                //Write to log table
                this.WriteFileLogFile("ARMIS file #37 not found!");
                WriteLog("ARMIS Interface", "Inf", "ARMIS file #37 not found!", "ARMIS file #37 not found!", 1);
                EMailNotification(false, "ARMIS file #37 not found!");
                return blnARMISFileError;
            }
            if (File.Exists(sARMISInboundFolder + "\\" + ARMISFile38))
            {
                blnFile38Exists = true;
            }
            else
            {
                //Write to log table
                this.WriteFileLogFile("ARMIS file #38 not found!");
                WriteLog("ARMIS Interface", "Inf", "ARMIS file #38 not found!", "ARMIS file #38 not found!", 1);
                EMailNotification(false, "ARMIS file #38 not found!");
                return blnARMISFileError;
            }

            if (blnFile37Exists && blnFile38Exists)
            {
                if (PreCheckFileData(sARMISInboundFolder + "\\" + ARMISFile37, sARMISInboundFolder + "\\" + ARMISFile38) != 0)
                {
                    //Write Error Log MessageBox.Show("fnPreCheckData Failed the conditions...");
                    return blnARMISFileError;
                }
                //Comparing All Amount Totals and No Of Records with Trailer Record 
                if (CompareWithTrailer(sARMISInboundFolder + "\\" + ARMISFile37, sARMISInboundFolder + "\\" + ARMISFile38) == false)
                {
                    return blnARMISFileError;
                }
                //call function to process the data
                if (ProcessARMISData("37", sARMISInboundFolder + "\\" + ARMISFile37) == true)
                {
                    if (ProcessARMISData("38", sARMISInboundFolder + "\\" + ARMISFile38) == true)
                    {
                        blnARMISFileError = false;
                    }
                    else
                    {
                        blnARMISFileError = true;
                    }
                }
                else { blnARMISFileError = true; }
            }
            else
            {
                blnARMISFileError = true;
                //Insert Error Log Row
            }

            return blnARMISFileError;
        }

        private int PreCheckFileData(string sARMISFile37, string sARMISFile38)
        {
            int nParseStatus = 9;
            TextReader tr37 = new StreamReader(sARMISFile37);
            TextReader tr38 = new StreamReader(sARMISFile38);

            /*Status;
             * 0=Valid Data Exists
             * 1=Header & Footer Found,No Details Valid
             * 7=Missing Header or Footer
             * 8=Empty File
             * 9=Error
            */

            //Processing File 37            
            string sDataLine37 = tr37.ReadLine();
            if (sDataLine37 != null)
            {
                if (sDataLine37.Substring(0, 15) == sARMISHeaderTag)
                {
                    sDataLine37 = tr37.ReadLine();//Read the 2nd line, if END Processing, then OVER
                    if (sDataLine37 != null)
                    //if (tr37.ReadLine() != null)//Read the 2nd line, if END Processing, then OVER
                    {
                        if (sDataLine37.Substring(0, 3) == sARMISFooterTag)
                        {
                            nParseStatus = 1; //Valid End of File Found w/o data
                        }
                        else
                        {
                            sDataLine37 = tr37.ReadToEnd();
                            if (sDataLine37.Contains(sARMISFooterTag))
                            {
                                nParseStatus = 0; //Valid End of File Found with data
                                if (sDataLine37.Substring(sDataLine37.IndexOf(sARMISFooterTag) + sARMISFooterTag.Length, 29).Trim() == "")
                                {
                                    this.WriteFileLogFile("No Trailer Record in File #37!");
                                    WriteLog("ARMIS Interface", "Inf", "No Trailer Record in File #37!", "No Trailer Record in File #37!", 1);
                                    nParseStatus = 8; //7
                                }
                            }
                            else
                            {
                                this.WriteFileLogFile("No footer in File #37!");
                                WriteLog("ARMIS Interface", "Inf", "No footer in File #37!", "No footer in File #37!", 1);
                                nParseStatus = 8; //7   Error
                            }
                        }
                    }
                    else
                    {
                        //Either no Detail Lines were found or No Footer Data was found...either way, it's an Error
                        this.WriteFileLogFile("No Detail Lines or footer in File  #37!");
                        WriteLog("ARMIS Interface", "Inf", "No Detail Lines or footer in File  #37!", "No Detail Lines or footer in File #37!", 1);
                        nParseStatus = 9;
                    }
                }
                else
                {
                    this.WriteFileLogFile("Invalid header in File #37!");
                    WriteLog("ARMIS Interface", "Inf", "Invalid header in File #37!", "Invalid header in File #37!", 1);
                    nParseStatus = 7;
                }//Error...No Header Found

            }
            else
            {
                this.WriteFileLogFile("File #37 is Empty!");
                WriteLog("ARMIS Interface", "Inf", "File #37 is Empty!", "File #37 is Empty!", 1);
                nParseStatus = 8;

            } //Error-Empty File


            //if (nParseStatus == 9) return nParseStatus;
            //No need to process 38...since the primary file had errors in it...


            //Processing File 38            
            string sDataLine38 = tr38.ReadLine();
            if (sDataLine38 != null)
            {
                if (sDataLine38.Substring(0, 15) == sARMISHeaderTag)
                {
                    sDataLine38 = tr38.ReadLine();//Read the 2nd line, if END Processing, then OVER
                    if (sDataLine38 != null)
                    //if (tr38.ReadLine() != null)//Read the 2nd line, if END Processing, then OVER
                    {
                        if (sDataLine38.Substring(0, 3) == sARMISFooterTag)
                        {
                            nParseStatus = 0;//Valid End of File Found w/o data
                        }
                        else
                        {
                            sDataLine38 = tr38.ReadToEnd();
                            if (sDataLine38.Contains(sARMISFooterTag))
                            {
                                nParseStatus = 0; //Valid End of File Found with data
                                if (sDataLine38.Substring(sDataLine38.IndexOf(sARMISFooterTag) + sARMISFooterTag.Length, 29).Trim() == "")
                                {
                                    this.WriteFileLogFile("No Trailer Record in File #38!");
                                    WriteLog("ARMIS Interface", "Inf", "No Trailer Record in File #38!", "No Trailer Record in File #38!", 1);
                                    nParseStatus = 8; //7
                                }
                            }
                            else
                            {
                                this.WriteFileLogFile("No footer in File #38!");
                                WriteLog("ARMIS Interface", "Inf", "No footer in File #38!", "No footer in File #37!", 1);
                                nParseStatus = 8; //7
                            }//Error
                        }
                    }
                    else
                    {
                        //Either no Detail Lines were found or No Footer Data was found...either way, it's an Error
                        this.WriteFileLogFile("No Detail Lines or footer in File #38!");
                        WriteLog("ARMIS Interface", "Inf", "No Detail Lines or footer in File #38!", "No Detail Lines or footer in File #38!", 1);
                        nParseStatus = 9;
                    }
                }
                else
                {
                    this.WriteFileLogFile("Invalid header in File #38!");
                    WriteLog("ARMIS Interface", "Inf", "Invalid header in File #38!", "Invalid header in File #38!", 1);
                    nParseStatus = 7;
                }//Error...No Header Found
            }
            else
            {
                this.WriteFileLogFile("File #38 is Empty!");
                WriteLog("ARMIS Interface", "Inf", "File #38 is Empty!", "File #38 is Empty!", 1);
                nParseStatus = 8;
            } //Error-Empty File

            tr37.Close();
            tr38.Close();

            return nParseStatus;


        }

        private bool CompareWithTrailer(string sARMISFile37, string sARMISFile38)
        {
            TextReader tr37 = new StreamReader(sARMISFile37);
            TextReader tr38 = new StreamReader(sARMISFile38);
            #region Load37

            //Parse the Rows
            string sDataLine37 = tr37.ReadLine();

            int iRecordCount37 = 0;
            int iTrailerRecordCount37 = 0;

            decimal dAmtTotal37 = 0;
            decimal dTrailerTotal37 = 0;

            //skipping the first row as it Header

            while (sDataLine37 != null)
            {
                sDataLine37 = tr37.ReadLine();
                if (sDataLine37 == null) { break; }
                if (sDataLine37.Substring(0, 3) == sARMISFooterTag)                               //Ex: END0000000300000000000015000000
                {
                    iTrailerRecordCount37 = Convert.ToInt32(sDataLine37.Substring(3, 9));         //Record Count 9 Positions
                    dTrailerTotal37 = Convert.ToDecimal(sDataLine37.Substring(12, 20));           //Grand Total 20 position 
                    break;
                }
                //Parse the Line and Summing the 
                dAmtTotal37 = dAmtTotal37 + (Convert.ToDecimal(sDataLine37.Substring(79, 16)) / 100) + (Convert.ToDecimal(sDataLine37.Substring(95, 16)) / 100) + (Convert.ToDecimal(sDataLine37.Substring(111, 16)) / 100) + (Convert.ToDecimal(sDataLine37.Substring(127, 16)) / 100) + (Convert.ToDecimal(sDataLine37.Substring(143, 16)) / 100) + (Convert.ToDecimal(sDataLine37.Substring(159, 16)) / 100);
                iRecordCount37++;
            }
            tr37.Close();

            if (iTrailerRecordCount37 != iRecordCount37)
            {
                //No of records and Trailer record record numbers are differing
                WriteLog("ARMIS Interface", "Inf", "ARMIS file #37 No of Records did not match with Trailer Record!", "Incorrect Number of Records #37!", 1);
                return false;
            }
            if (dTrailerTotal37 != dAmtTotal37)
            {
                //Amount Totals and Trailer totals are differing
                WriteLog("ARMIS Interface", "Inf", "ARMIS file #37 Amount Totals did not match with Trailer Record!", "Incorrect Amount Totals #37!", 1);
                return false;
            }

            #endregion

            #region Load38

            //Parse the Rows
            string sDataLine38 = tr38.ReadLine();

            int iRecordCount38 = 0;
            int iTrailerRecordCount38 = 0;

            decimal dAmtTotal38 = 0;
            decimal dTrailerTotal38 = 0;

            //skipping the first row as it Header                
            while (sDataLine38 != null)
            {

                sDataLine38 = tr38.ReadLine();
                if (sDataLine38 == null) { break; }
                if (sDataLine38.Substring(0, 3) == sARMISFooterTag)                               //Ex: END0000000300000000000015000000
                {
                    iTrailerRecordCount38 = Convert.ToInt32(sDataLine38.Substring(3, 9));         //Record Count 9 Positions
                    dTrailerTotal38 = Convert.ToDecimal(sDataLine38.Substring(12, 20));           //Grand Total 20 position 
                    break;
                }
                //Parse the Line and Summing the 
                dAmtTotal38 = dAmtTotal38 + (Convert.ToDecimal(sDataLine38.Substring(130, 14)) / 100) + (Convert.ToDecimal(sDataLine38.Substring(144, 14)) / 100) + (Convert.ToDecimal(sDataLine38.Substring(158, 14)) / 100) + (Convert.ToDecimal(sDataLine38.Substring(172, 14)) / 100) + (Convert.ToDecimal(sDataLine38.Substring(186, 14)) / 100) + (Convert.ToDecimal(sDataLine38.Substring(200, 14)) / 100);
                iRecordCount38++;
            }
            tr38.Close();

            if (iTrailerRecordCount38 != iRecordCount38)
            {
                //No of records and Trailer record numbers are differing
                WriteLog("ARMIS Interface", "Inf", "ARMIS file #38 No of Records did not match with Trailer Record!", "Incorrect Number of Records #38!", 1);
                return false;
            }

            if (dTrailerTotal38 != dAmtTotal38)
            {
                //Amount Totals and Trailer totals are differing
                WriteLog("ARMIS Interface", "Inf", "ARMIS file #38 Amount Totals did not match with Trailer Record!", "Incorrect Amount Totals #38!", 1);
                return false;

            }
            #endregion
            return true;
        }


        private bool ProcessARMISData(string sFileType, string sARMIsFile)
        {
            #region fnProcessARMISData_comment
            //This function will load the ARMIS text files (3 nos.) 
            //into the Retro Temporary tables.
            #endregion

            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandTimeout = 7200;
            //string sSQLServerConnectString = "Data Source=SPINTO2-1\\SQLExpress;Initial Catalog=Retro;Integrated Security=True";
            conn.ConnectionString = sSQLServerConnectString;
            conn.Open();
            sqlCmd.Connection = conn;

            #region Load37
            if (sFileType == "37")
            {
                TextReader tr37 = new StreamReader(sARMIsFile);

                //Parse the Rows
                string sDataLine = tr37.ReadLine();
                int counter = 0;
                //Header
                string sHeader = sDataLine;
                while (sDataLine != null)
                {
                    counter++;
                    sDataLine = tr37.ReadLine();
                    if (sDataLine == null) { break; }
                    if (sDataLine.Substring(0, 3) == sARMISFooterTag) { break; }
                    //Parse the Line of data and store it into the table
                    string sDataLine0 = "'" + sDataLine.Substring(0, 10) + "','" + sDataLine.Substring(10, 10);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(20, 3) + "','" + sDataLine.Substring(23, 4) + "','" + sDataLine.Substring(27, 5) + "','" + sDataLine.Substring(32, 4);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(36, 10) + "','" + sDataLine.Substring(46, 10);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(56, 6) + "','" + ((sDataLine.Substring(62, 15).Trim().Length == 7) ? "0" + sDataLine.Substring(62, 15).Trim() : sDataLine.Substring(62, 15).Trim()) + "','" + sDataLine.Substring(77, 2);
                    sDataLine0 = sDataLine0 + "'," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(79, 16)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(95, 16)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(111, 16)) / 100));
                    sDataLine0 = sDataLine0 + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(127, 16)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(143, 16)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(159, 16)) / 100));
                    //sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(175, 1) + "'";
                    sDataLine0 = sDataLine0 + ",'Y'," + " Getdate()";

                    string sInsertData = "Insert into ARMIS_INTRFC37 values ( " + sDataLine0 + ")";

                    //Replacing '0001-01-01' with NULL
                    sInsertData = sInsertData.Replace("'0001-01-01'", "NULL");

                    //DataWrite
                    sqlCmd.CommandText = sInsertData;
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.Dispose();
                }
                tr37.Close();
            }

            #endregion
            #region Load38
            if (sFileType == "38")
            {
                TextReader tr38 = new StreamReader(sARMIsFile);

                //Parse the Rows
                string sDataLine = tr38.ReadLine();
                int counter = 0;
                //Header
                string sHeader = sDataLine;
                while (sDataLine != null)
                {
                    counter++;
                    sDataLine = tr38.ReadLine();
                    if (sDataLine == null) { break; }
                    if (sDataLine.Substring(0, 3) == sARMISFooterTag) { break; }

                    //Parse the Line of data and store it into the table
                    string sDataLine0 = "'" + sDataLine.Substring(0, 10) + "','" + sDataLine.Substring(10, 10);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(20, 3) + "','" + sDataLine.Substring(23, 4) + "','" + sDataLine.Substring(27, 5) + "','" + sDataLine.Substring(32, 4);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(36, 10) + "','" + sDataLine.Substring(46, 10);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(56, 3) + "','" + sDataLine.Substring(59, 20) + "','" + sDataLine.Substring(79, 15);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(94, 6) + "','" + sDataLine.Substring(100, 5);
                    sDataLine0 = sDataLine0 + "','" + ((sDataLine.Substring(105, 15).Trim().Length == 7) ? "0" + sDataLine.Substring(105, 15).Trim() : sDataLine.Substring(105, 15).Trim()) + "','" + sDataLine.Substring(120, 10);
                    sDataLine0 = sDataLine0 + "'," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(130, 14)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(144, 14)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(158, 14)) / 100));
                    sDataLine0 = sDataLine0 + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(172, 14)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(186, 14)) / 100)) + "," + Convert.ToString((Convert.ToDecimal(sDataLine.Substring(200, 14)) / 100));
                    sDataLine0 = sDataLine0 + ",'" + sDataLine.Substring(214, 64).Replace("'", "''") + "','" + sDataLine.Substring(278, 2) + "','" + sDataLine.Substring(280, 1);
                    sDataLine0 = sDataLine0 + "','" + sDataLine.Substring(281, 10) + "','" + sDataLine.Substring(291, 10) + "','" + sDataLine.Substring(301, 10) + "'";
                    sDataLine0 = sDataLine0 + ", Getdate()";

                    string sInsertData = "Insert into ARMIS_INTRFC38 values ( " + sDataLine0 + ")";

                    //Replacing '0001-01-01' with NULL
                    sInsertData = sInsertData.Replace("'0001-01-01'", "NULL");

                    //DataWrite
                    sqlCmd.CommandText = sInsertData;
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.Dispose();
                }

                tr38.Close();
            }
            #endregion

            conn.Close();
            sqlCmd.Dispose();
            return true;
        }

        private bool LoadARMISLossData()
        {
            #region fnLoadARMISData_comment
            //This function will UPLOAD ARMIS data from temporary holding tables to the 
            //main Retro Processing tables
            #endregion
            try
            {
                SqlConnection conn = new SqlConnection();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = 7200;
                conn.ConnectionString = sSQLServerConnectString;
                conn.Open();
                sqlCmd.Connection = conn;

                sqlCmd.CommandText = "ModAIS_LoadARMISData";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                this.WriteFileLogFile("LoadARMISLossData method failed");
                this.WriteFileLogFile(ex.Message);
                return false;
            }
            
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
                FilePath = "d:\\ARMISLog.txt";
            }
            else
            {
                FilePath = ConfigurationManager.AppSettings["LogFile"].ToString() + "\\ARMISLog.txt";
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
        private void EMailNotification(bool status,string strMessage)
        {
            SMTPMailer dd = new SMTPMailer();
            dd.HasAttachment = true;
            System.Net.Mail.MailAddressCollection objMailList = new System.Net.Mail.MailAddressCollection();
            dd.lstTomailAddress = objMailList;
            
            string[] strEmailList = strEmailIDs.Split(';');
            for (int i = 0; i < strEmailList.Count(); i++)
            {
                objMailList.Add(strEmailList[i].ToString());
               
            }

            if (status == true)
            {
                
                SqlConnection conn = new SqlConnection(sSQLServerConnectString);
                //ARMIS_INTRFC37_ERR
                string strQuery37 = "SELECT convert(varchar(10),valn_dt,110) as 'Val.Date',suprt_serv_custmr_gp_id,cls1_txt,cls2_txt,cls3_txt,cls4_txt,"
                                + " convert(varchar(10),pol_eff_dt,110) as 'Pol Eff.Date',convert(varchar(10),pol_expiry_dt,110) as 'Pol Exp.Date',"
                                + " ln_of_bsn_txt as 'LOB',pol_nbr_txt,st_txt as 'State',paid_exps_amt,paid_los_bimed_amt,paid_los_lt_amt,resrvd_bimed_amt,"
                                + " resrvd_lt_amt,resrvd_exps_amt,wcd_undrln_ind,err_txt as 'Error',convert(varchar(10),crte_dt,110) as 'Created Date'"
                                + " FROM [dbo].[ARMIS_INTRFC37_ERR] where cast(convert(varchar(10),crte_dt,112) as datetime)= cast(convert(varchar(10),getdate(),112) as datetime)";
                //ARMIS_INTRFC38_ERR
                string strQuery38 = " SELECT convert(varchar(10),valn_dt,110) as 'Val.Date',suprt_serv_custmr_gp_id,cls1_txt,cls2_txt,cls3_txt,cls4_txt, "
                                    + " convert(varchar(10),pol_eff_dt,110) as 'Pol Eff.Date',convert(varchar(10),pol_expiry_dt,110) as 'Pol Exp.Date', "
                                    + " data_src_txt,clm_nbr_txt,site_cd_txt,ln_of_bsn_txt as 'LOB',covg_txt,pol_nbr_txt,convert(varchar(10),pol_eff_2_dt,110) as 'Pol Eff_2.Date',"
                                    + " paid_los_bimed_amt,paid_los_lt_amt,paid_exps_amt,resrvd_exps_amt,resrvd_los_bimed_amt,resrv_los_lt_amt,clmt_nm,"
                                    + " st_txt as 'state',clm_sts_txt,convert(varchar(10),covg_trigr_dt,110) as 'covg_trigr_dt',orgin_clm_nbr_txt,"
                                    + " reop_clm_nbr_txt,err_txt as 'Error',convert(varchar(10),crte_dt,110) as 'Created Date'"
                                    + " FROM [dbo].[ARMIS_INTRFC38_ERR] where cast(convert(varchar(10),crte_dt,112) as datetime)= cast(convert(varchar(10),getdate(),112) as datetime)";

                //ARMIS_INTRFC37_ERR
                SqlDataAdapter daArmis37 = new SqlDataAdapter(strQuery37, conn);
                DataSet dsArmis37 = new DataSet();
                daArmis37.Fill(dsArmis37, "Armis37");

                //ARMIS_INTRFC38_ERR
                SqlDataAdapter daArmis38 = new SqlDataAdapter(strQuery38, conn);
                DataSet dsArmis38 = new DataSet();
                daArmis38.Fill(dsArmis38, "Armis38");

                                
                //ARMIS_INTRFC37_ERR
                string strArmis37FileName = "ARMIS37_ERROR_" + DateTime.Today.ToString("MM-dd-yyyy") + ".xls";
                string strArmis37FilePath = sARMISArchiveFolder + "\\" + strDate + "\\" + strArmis37FileName;
                FastExportingMethod.ExportToExcel(dsArmis37, strArmis37FilePath);

                //ARMIS_INTRFC38_ERR
                string strArmis38FileName = "ARMIS38_ERROR_" + DateTime.Today.ToString("MM-dd-yyyy") + ".xls";
                string strArmis38FilePath = sARMISArchiveFolder + "\\" + strDate + "\\" + strArmis38FileName;
                FastExportingMethod.ExportToExcel(dsArmis38, strArmis38FilePath);

                //ARMIS_INTRFC37_ERR
                if (dsArmis37.Tables[0].Rows.Count > 0)
                {
                    ZAAttachment atArmis37 = new ZAAttachment();
                    atArmis37.AttachmentName = strArmis37FileName;
                    atArmis37.AttachmentType = SMTPMailer.Excel;
                    atArmis37.FullAttachmentPath = strArmis37FilePath;
                    dd.AttachmentList.Add(atArmis37);
                }

                //ARMIS_INTRFC38_ERR
                if (dsArmis38.Tables[0].Rows.Count > 0)
                {
                    ZAAttachment atArmis38 = new ZAAttachment();
                    atArmis38.AttachmentName = strArmis38FileName;
                    atArmis38.AttachmentType = SMTPMailer.Excel;
                    atArmis38.FullAttachmentPath = strArmis38FilePath;
                    dd.AttachmentList.Add(atArmis38);
                }
                string strSubject = "Reg:ARMIS ERROR LOG @" + DateTime.Today.ToString("MM-dd-yyyy");
                string strBody = "Hi All,";
                strBody = strBody + "\r\n";
                strBody = strBody + "\r\n";
                strBody = strBody + "ARMIS job ran successfully on " + DateTime.Today.ToString("MM-dd-yyyy") + "\r\n";
                if (dsArmis37.Tables[0].Rows.Count > 0 || dsArmis38.Tables[0].Rows.Count > 0)
                {
                    strBody = strBody + "Please find the  Error log files along with this mail." + "\r\n";
                }
                else
                {
                    strBody = strBody + "No entries logged into error log tables" + "\r\n";
                }

                strBody = strBody + "\r\n";
                strBody = strBody + "\r\n";

                dd.Subject = strSubject;
                dd.Body = strBody;
                dd.SendMail();
            }
            else
            {
                string strSubject = "Reg:ARMIS JOB FAILED @" + DateTime.Today.ToString("MM-dd-yyyy");
                string strBody = "Hi All,";
                strBody = strBody + "\r\n";
                strBody = strBody + "\r\n";
                strBody = strBody + "ARMIS job failed to run on " + DateTime.Today.ToString("MM-dd-yyyy") + "\r\n";
                strBody = strBody + "because of " + strMessage + "\r\n";
                strBody = strBody + "Please contact application support team for resolution." + "\r\n";
                strBody = strBody + "\r\n";
                strBody = strBody + "\r\n";

                dd.Subject = strSubject;
                dd.Body = strBody;
                dd.SendMail();
            }
        }
       

        #endregion

    }
}
