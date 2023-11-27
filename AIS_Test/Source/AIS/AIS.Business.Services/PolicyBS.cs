using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Configuration;
using Microsoft.VisualBasic.FileIO;



namespace ZurichNA.AIS.Business.Logic
{
    public class PolicyBS : BusinessServicesBase<PolicyBE, PolicyDA>
    {
        private LookupBS lookup;

        public PolicyBS()
        {
            lookup = new LookupBS();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <returns></returns>
        public IList<PolicyBE> getPolicyData(int ProgramPeriodID)
        {
            IList<PolicyBE> policy;

            try
            {
                policy = new PolicyDA().getPolicyData(ProgramPeriodID, 0);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            //IList<PolicyBE> PolicyBE = this.buildList(policy);
            //return (PolicyBE);
            return (policy);
        }
        /// <summary>
        /// This will retrive all Policies along with underlaying policies i.e ParentPolicy!=null
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <returns></returns>
        public IList<PolicyBE> getAllPolicies(int ProgramPeriodID)
        {
            IList<PolicyBE> policy;

            try
            {
                policy = new PolicyDA().GetPolicyData(ProgramPeriodID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            //IList<PolicyBE> PolicyBE = this.buildList(policy);
            //return (PolicyBE);
            return (policy);
        }
        public IList<PolicyBE> getPolicyDataForParentID(int ParentPolicyID)
        {
            IList<PolicyBE> policy;

            try
            {
                policy = new PolicyDA().getPolicyDataForParentID(ParentPolicyID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            //IList<PolicyBE> PolicyBE = this.buildList(policy);
            //return (PolicyBE);
            return (policy);
        }
        /// <summary>
        /// Retrieves Policy Data
        /// </summary>
        /// <returns>List of Policy Data</returns>
        public IList<PolicyBE> getPolicyData()
        {
            IList<PolicyBE> policy;

            try
            {
                policy = new PolicyDA().getPolicyData();
                PolicyBE polBE = new PolicyBE();
                polBE.PolicyID = 0;
                polBE.PolicyNumber = "(Select)";
                policy.Insert(0, polBE);
                return policy;

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

        }
        /// <summary>
        /// Retrieves Policy Effective Date based on Prg Id and Account No.
        /// </summary>
        /// <returns>List of Policy Eff. date</returns>
        public IList<PolicyBE> getPolicyLookUpData(int ProgramPeriodID, int cstmrid)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyLookUpData(ProgramPeriodID, cstmrid);
                PolicyBE polBE = new PolicyBE();
                //polBE.PolicyID = 0;
                polBE.POLICY_EFF_DATE = "(Select)";
                policy.Insert(0, polBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return policy;
        }

        /// <summary>
        /// Retrieves Policy Effective Date based on Prg Id and Account No.
        /// </summary>
        /// <returns>List of Policy Eff. date</returns>
        public IList<PolicyBE> getPolicyExpirationDate(int ProgramPeriodID, int cstmrid)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyExpirationDate(ProgramPeriodID, cstmrid);
                PolicyBE polBE = new PolicyBE();
                //polBE.PolicyID = 0;
                polBE.POLICY_END_DATE = "(Select)";
                policy.Insert(0, polBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return policy;
        }

        /// <summary>
        /// Retrieves Policy based on LOB and Prg Id
        /// </summary>
        /// <returns>List of Policy Data</returns>
        public IList<PolicyBE> getLOBPolData(String LOB, int PrgID)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getLOBPolData(LOB, PrgID);
                PolicyBE polBE = new PolicyBE();
                polBE.PolicyID = 0;
                polBE.PolicyNumber = "(Select)";
                policy.Insert(0, polBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return policy;
        }

        public PolicyBE getPolicyInfo(int PolicyID)
        {
            PolicyBE policyInfo = new PolicyBE();
            policyInfo = DA.Load(PolicyID);
            return policyInfo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="PolicyID"></param>
        /// <returns></returns>
        public IList<PolicyBE> getPolicyData(int ProgramPeriodID, int PolicyID)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyData(ProgramPeriodID, PolicyID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            IList<PolicyBE> PolicyBE = this.buildList(policy);
            return (PolicyBE);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="PolicyID"></param>
        /// <returns></returns>
        public IList<PolicyBE> getPolicyDataWithActID(int ProgramPeriodID, int AccountID)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyDataforActID(ProgramPeriodID, AccountID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            //IList<PolicyBE> PolicyBE = this.buildList(policy);
            //return (PolicyBE);
            return (policy);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="PolicyID"></param>
        /// /// <param name="AdjID"></param>
        /// <returns></returns>
        public IList<PolicyBE> getPolicyDataWithActID(int ProgramPeriodID, int AccountID, int AdjID)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyDataforActID(ProgramPeriodID, AccountID, AdjID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            //IList<PolicyBE> PolicyBE = this.buildList(policy);
            //return (PolicyBE);
            return (policy);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="PolicyID"></param>
        /// <returns></returns>
        public IList<PolicyBE> getPolicyDataWithActIDLBA(int ProgramPeriodID, int AccountID)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyDataforActIDLBA(ProgramPeriodID, AccountID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            //IList<PolicyBE> PolicyBE = this.buildList(policy);
            //return (PolicyBE);
            return (policy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        private IList<PolicyBE> buildList(IList<PolicyBE> policy)
        {
            IList<PolicyBE> PolicyList = new List<PolicyBE>();
            foreach (PolicyBE value in policy)
            {
                value.AdjustmentTypeName = LookupBS.getLookupName(value.AdjusmentTypeID.Value, lookup.AdjustmentType);
                value.ALAETypeName = LookupBS.getLookupName(value.ALAETypeID.Value, lookup.ALAEType);
                value.CoverageTypeName = LookupBS.getLookupName("DEP", lookup.CoverageType);
                value.LossSourceName = LookupBS.getLookupName(value.LossSystemSourceID, lookup.LossSource);
                value.StatesName = LookupBS.getLookupName(value.DedtblProtPolicyStID, lookup.States);
                PolicyList.Add(value);
            }
            return (PolicyList);

        }
        /// <summary>
        /// To retrieve the policies for NY-SIF Screen
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<PolicyBE> getPolicies(int ProgramPeriodID, int AccountID)
        {
            IList<PolicyBE> policy;
            try
            {
                policy = new PolicyDA().getPolicies(ProgramPeriodID, AccountID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            //IList<PolicyBE> PolicyBE = this.buildList(policy);
            //return (PolicyBE);
            return (policy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private string getLookupName(int Key, Dictionary<int, string> Table)
        {
            string name = (from c in Table
                           where c.Key == Key
                           select c.Value).First();

            return name;
        }

        /// <summary>
        /// Perform a save - this could be either an insert or update.
        /// </summary>
        /// <param name="acct"></param>
        /// <returns></returns>
        public bool Save(PolicyBE source)
        {
            bool success = false;

            try
            {
                success = DA.Update(source);
            }
            catch (Exception ex)
            {
                source.SetError(ex.Message);
                success = false;
            }
            return success;
        }
        public bool Update(PolicyBE PolBE)
        {
            bool succeed = false;
            if (PolBE.PolicyID > 0)
            {
                try
                {
                    succeed = this.DA.Update(PolBE);
                }
                catch (Exception ex)
                {
                    PolBE.SetError(ex.Message);
                    succeed = false;
                }
            }
            else
            {               
                succeed = this.DA.Add(PolBE);
            }
            return succeed;
        }
        /// <summary>
        /// Retrives PolicyID and Number for ILRF Setup
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="cstmrid"></param>
        /// <returns>List of Policy data</returns>
        public List<LookupBE> getPolicyDataforLookups(int programPeriodID, int cstmrid)
        {
            IList<LookupBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyDataforLookups(programPeriodID, cstmrid);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (policy.ToList());
        }


        public List<LookupBE> getPolicyDataforLookups(string programPeriodIDs, int cstmrid)
        {
            IList<LookupBE> policy;
            try
            {
                policy = new PolicyDA().getPolicyDataforLookups(programPeriodIDs, cstmrid);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (policy.ToList());
        }

        /// <summary>
        /// Checks if a Policy is already exists with given parameters
        /// </summary>
        /// <param name="polID"></param>
        /// <param name="polMod"></param>
        /// <param name="polNo"></param>
        /// <param name="polSyb"></param>
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>if exsits return true, else false</returns>
        public bool isPolicyAlreadyExist(int polID,  string polNo, DateTime polEffDate, int programPeriodID)
        {
            bool isPolExists;
            try
            {
                isPolExists = new PolicyDA().isPolicyAlreadyExist(polID, polNo, polEffDate, programPeriodID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (isPolExists);
        }
        public bool isPolicyAlreadyExistAndDisabled(int polID, string polNo, DateTime polEffDate, int programPeriodID)
        {
            bool isPolExists;
            try
            {
                isPolExists = new PolicyDA().isPolicyAlreadyExistAndDisabled(polID, polNo, polEffDate, programPeriodID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (isPolExists);
        }

        public bool isPolExistsInAnyAcct(int polID, string PolicySymbol, string polNo, string PolicyModulus, DateTime polEffDate, DateTime polExpDate, int customerID)
        {
            bool isPolExistsInAnyAcct;
            try
            {
                isPolExistsInAnyAcct = new PolicyDA().isPolExistsInAnyAcct(polID, PolicySymbol, polNo, PolicyModulus, polEffDate, polExpDate, customerID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (isPolExistsInAnyAcct);
        }

        public string isPolExistsInAnyAcctCustName(int polID, string PolicySymbol, string polNo, string PolicyModulus, DateTime polEffDate, DateTime polExpDate, int customerID)
        {
            string  custName;
            try
            {
                custName = new PolicyDA().isPolExistsInAnyAcctCustName(polID, PolicySymbol, polNo, PolicyModulus, polEffDate, polExpDate, customerID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (custName);
        }


        /// <summary>
        /// to get the policy ID
        /// </summary>
        /// <param name="polMod"></param>
        /// <param name="polNo"></param>
        /// <param name="polSyb"></param>
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>returns the policy ID</returns>
        public int getPolicyID(string polNo, DateTime polEffDate, int programPeriodID)
        {
            int policyID;
            try
            {
                policyID = new PolicyDA().getPolicyID(polNo, polEffDate, programPeriodID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return policyID;
        }


        /// <summary>
        /// to get the policy ID
        /// </summary>
        /// <param name="polMod"></param>
        /// <param name="polNo"></param>
        /// <param name="polSyb"></param>
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>returns the policy ID</returns>
        public int getPolicyID(string PolicySymbol, string polNo, string PolicyModulus, DateTime polEffDate, DateTime polExpDate, int customerID)
        {
            int policyID;
            try
            {
                policyID = new PolicyDA().getPolicyID(PolicySymbol, polNo, PolicyModulus, polEffDate, polExpDate, customerID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return policyID;
        }

        /// <summary>
        /// to get the program period ID
        /// </summary>
        /// <param name="polMod"></param>
        /// <param name="polNo"></param>
        /// <param name="polSyb"></param>
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>returns the program period ID</returns>
        public int getProgramPeriodID(DateTime polEffDate, DateTime polExpDate, int custmrID)
        {
            int prgmPrdID;
            try
            {
                prgmPrdID = new PolicyDA().getProgramPeriodID(polEffDate, polExpDate, custmrID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return prgmPrdID;
        }


        // Gets data from excel sheet
        public DataTable GetDataTableExcel(string strFileName)
        {
            try
            {
                //Getting workbook from given path
                SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(strFileName);
                //Getting active worksheet from workbook
                SpreadsheetGear.IWorksheet worksheet = workbook.ActiveWorksheet;
                //Getting cells from worksheet
                SpreadsheetGear.IRange cells = worksheet.Cells;

                //Getting used range of row count & column count from worksheet
                int rows = worksheet.UsedRange.RowCount;
                int cols = worksheet.UsedRange.ColumnCount;

                //Data table initialization
                System.Data.DataTable datatable = new DataTable();

                //For Column Headers
                for (int c = 0; c < cols; c++)
                {
                    datatable.Columns.Add(cells[0, c].Text);
                }

                //For rows and columns
                for (int r = 1; r < rows; r++)
                {
                    DataRow dr = datatable.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[c] = cells[r, c].Text;
                    }
                    datatable.Rows.Add(dr);
                }
                bool isEmpty = true;

                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    isEmpty = true;

                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        if (string.IsNullOrEmpty(datatable.Rows[i][j].ToString()) == false)
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty == true)
                    {
                        datatable.Rows.RemoveAt(i);
                        i--;
                    }
                }

                return datatable;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        // Gets data from excel sheet for Stepped Factor
        public DataTable GetDataTableExcelSF(string strFileName)
        {
            try
            {
                //Getting workbook from given path
                SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(strFileName);
                //Getting active worksheet from workbook
                SpreadsheetGear.IWorksheet worksheet = workbook.ActiveWorksheet;
                //Getting cells from worksheet
                SpreadsheetGear.IRange cells = worksheet.Cells;

                //Getting used range of row count & column count from worksheet
                int rows = worksheet.UsedRange.RowCount;
                int cols = worksheet.UsedRange.ColumnCount;

                //Data table initialization
                System.Data.DataTable datatable = new DataTable();

                //For Column Headers
                for (int c = 0; c < cols; c++)
                {
                    datatable.Columns.Add(cells[0, c].Text);
                }

                //For rows and columns
                for (int r = 1; r < rows; r++)
                {
                    DataRow dr = datatable.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[c] = cells[r, c].Text;
                    }
                    datatable.Rows.Add(dr);
                }
                bool isEmpty = true;

                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    isEmpty = true;

                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        if (string.IsNullOrEmpty(datatable.Rows[i][j].ToString()) == false)
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty == true)
                    {
                        datatable.Rows.RemoveAt(i);
                        i--;
                    }
                }

                return datatable;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        //Gets data from excel sheet without oledb
        public DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            string tableDelim = ",";
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[]
                    {
                        tableDelim
                    });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == string.Empty)
                            {
                                fieldData[i] = string.Empty; //fieldData[i] = null
                            }
                            //Skip rows that have any csv header information or blank rows in them
                            if (fieldData[0].Contains("Disclaimer") || string.IsNullOrEmpty(fieldData[0]))
                            {
                                continue;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return csvData;
        }
        public DataTable GetDataTableExcelSSG(string strFileName,string strSheetName)
        {
            try
            {
                SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(strFileName);
                DataSet dataSet = new DataSet();
                dataSet = workbook.GetDataSet(strSheetName, SpreadsheetGear.Data.GetDataFlags.FormattedText);
                return dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Gets data from excel sheet
        public DataTable GetDataTableExcelWithOutOledb(string strFileName,string strSheetName)
        {
            try
            {
                //Getting workbook from given path
                SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(strFileName);
                //Getting active worksheet from workbook
                SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[strSheetName];
                //Getting cells from worksheet
                SpreadsheetGear.IRange cells = worksheet.Cells;

                //Getting used range of row count & column count from worksheet
                int rows = worksheet.UsedRange.RowCount;
                int cols = worksheet.UsedRange.ColumnCount;

                //Data table initialization
                System.Data.DataTable datatable = new DataTable();

                //For Column Headers
                for (int c = 0; c < cols; c++)
                {
                    datatable.Columns.Add(cells[0, c].Text);
                }

                //For rows and columns
                for (int r = 1; r < rows; r++)
                {
                    DataRow dr = datatable.NewRow();
                    for (int c = 0; c < cols; c++)
                    {
                        dr[c] = cells[r, c].Text;
                    }
                    datatable.Rows.Add(dr);
                }
                bool isEmpty = true;

                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    isEmpty = true;

                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        if (string.IsNullOrEmpty(datatable.Rows[i][j].ToString()) == false)
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty == true)
                    {
                        datatable.Rows.RemoveAt(i);
                        i--;
                    }
                }

                return datatable;
            }

            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
