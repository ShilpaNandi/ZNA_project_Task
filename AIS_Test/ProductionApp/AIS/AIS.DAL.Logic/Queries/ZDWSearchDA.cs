using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.Business.Entities;
using System.Data;
using ZurichNA.AIS.ExceptionHandling;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
namespace ZurichNA.AIS.DAL.Logic
{
    public class ZDWSearchDA : DataAccessor<PREM_ADJ, ZDWSearchBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Returns all ZDW Search data that match criteria
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public string getKeysData(string AccountName, string ProgramPrdEffDt, string ProgramPrdExpDt, string strValDate, string InvoiceNumber, string FinalInvoiceDt, string AccountNumber, string BrokerName, string BU, string AnalystName, string ProgramType, string PolicyNumber)
        {
            string ProgramTyp = string.Empty;
            if (ProgramType.ToUpper() == "DEP")
            {
                ProgramTyp = (from lk in this.Context.LKUPs
                              where lk.lkup_txt == "DEP"
                              select lk.lkup_id).First().ToString();
            }
            else if (ProgramType.ToUpper() == "NON-DEP")
            {
                ProgramTyp = (from lk in this.Context.LKUPs
                              where lk.lkup_txt == "NON-DEP"
                              select lk.lkup_id).First().ToString();
            }
           
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString();
            conn.Open();
            sqlCmd.Connection = conn;
            sqlCmd.CommandText = "GetZDWSearch";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@ACCOUNTNAME", SqlDbType.VarChar).Value =AccountName;
            sqlCmd.Parameters.Add("@POLICYNO", SqlDbType.VarChar).Value=PolicyNumber;
            sqlCmd.Parameters.Add("@VALDT", SqlDbType.VarChar).Value = strValDate;
            sqlCmd.Parameters.Add("@INVDT", SqlDbType.VarChar).Value = FinalInvoiceDt;
            sqlCmd.Parameters.Add("@ACCOUNTNO", SqlDbType.VarChar).Value = AccountNumber;
            sqlCmd.Parameters.Add("@BROKER", SqlDbType.VarChar).Value = BrokerName;
            sqlCmd.Parameters.Add("@BU", SqlDbType.VarChar).Value = BU;
            sqlCmd.Parameters.Add("@ANALYSTNAME", SqlDbType.VarChar).Value = AnalystName;
            sqlCmd.Parameters.Add("@INVNO", SqlDbType.VarChar).Value = InvoiceNumber;
            sqlCmd.Parameters.Add("@PGMTYP", SqlDbType.VarChar).Value = ProgramTyp;
            sqlCmd.Parameters.Add("@PGMEFFDT", SqlDbType.VarChar).Value = ProgramPrdEffDt;
            sqlCmd.Parameters.Add("@PGMEXPDT", SqlDbType.VarChar).Value = ProgramPrdExpDt;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            adapter.Fill(dt);
            string Result = string.Empty;
             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 string InvNumber=string.Empty;
                 string InvDate = string.Empty;
                 string DocKeyInternal = string.Empty;
                 string DocKeyExternal = string.Empty;
                 string DocKeyCW = string.Empty;
                 if(dt.Rows[i][4].ToString()!="")
                 {
                     InvNumber=dt.Rows[i][4].ToString();
                     InvDate = dt.Rows[i][5].ToString();
                 }
                 else
                 {
                    InvNumber=dt.Rows[i][3].ToString();
                    InvDate = dt.Rows[i][6].ToString();
                 }
                 
                 if (dt.Rows[i][10].ToString() != "")
                     DocKeyInternal = dt.Rows[i][10].ToString();
                 if (dt.Rows[i][11].ToString() != "")
                     DocKeyExternal = dt.Rows[i][11].ToString();
                 if (dt.Rows[i][12].ToString() != "")
                     DocKeyCW = dt.Rows[i][12].ToString();
                 if (DocKeyCW == "" && DocKeyInternal == "" && DocKeyExternal == "")
                 {
                     if (dt.Rows[i][7].ToString() != "")
                         DocKeyInternal = dt.Rows[i][7].ToString();
                     if (dt.Rows[i][8].ToString() != "")
                         DocKeyExternal = dt.Rows[i][8].ToString();
                     if (dt.Rows[i][9].ToString() != "")
                         DocKeyCW = dt.Rows[i][9].ToString();
                 }
                 if (DocKeyCW == "" && DocKeyInternal == "" && DocKeyExternal == "")
                 {
                 }
                 else
                 {
                     Result += dt.Rows[i][0].ToString() + "|" + dt.Rows[i][2].ToString() + "|" + InvNumber + "|"
                                 + InvDate + "|" + dt.Rows[i][17].ToString() + "|" + DocKeyInternal + "|"
                                 + DocKeyExternal + "|" + DocKeyCW + "~";
                 }
                     
              }
             if (conn != null)
                 conn.Close();
             return Result;
        }
    }
}
