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
    public class InvoiceInquiryDA : DataAccessor<PREM_ADJ, InvoiceInquiryBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Returns all Invoice Inquiry data that match criteria
        /// </summary>
        /// <param name="intAccountID"></param>
        /// <param name="intProgramTypID"></param>
        /// <param name="strValDate"></param>
        /// <param name="strInvoiceNumber"></param>
        /// <param name="strInvoiceDate"></param>
        /// <param name="intPersonID"></param>
        /// <param name="intExtOrgID"></param>
        /// <param name="intInternalOrgID"></param>
        /// <param name="intAccountNumber"></param>
        /// <returns></returns>
        public DataTable getInvoiceInquiryData(int intAccountID, int intProgramTypID, string strValDate, string strInvoiceNumber, string strInvoiceDate, int intPersonID, int intExtOrgID, int intInternalOrgID, int intAccountNumber)
        {
            string strCustmerNumber = string.Empty;
            if (intAccountID > 0 && intAccountNumber == 0)
            {
                strCustmerNumber = intAccountID.ToString();
            }
            else
            {
                strCustmerNumber = intAccountNumber.ToString();
            }
            DataTable dtInvoiceInquiry = new DataTable();
            SqlConnection sqlconn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandTimeout = 7200;
            sqlconn.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString();
            sqlconn.Open();
            sqlCmd.Connection = sqlconn;
            sqlCmd.CommandText = "GetInvoiceInquiry";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if(strCustmerNumber!="0")
            sqlCmd.Parameters.Add("@ACCOUNTID", SqlDbType.VarChar).Value = strCustmerNumber;
            else
                sqlCmd.Parameters.Add("@ACCOUNTID", SqlDbType.VarChar).Value = string.Empty;
                            
            if (strValDate != null && strValDate != "")
            {
                DateTime dtValDate = Convert.ToDateTime(strValDate);

                string ValDate =dtValDate.ToString("MM/dd/yyyy");
                sqlCmd.Parameters.Add("@VALDT", SqlDbType.VarChar).Value = ValDate;
            }
            else
            {
                sqlCmd.Parameters.Add("@VALDT", SqlDbType.VarChar).Value = string.Empty;
            }
            if (strInvoiceDate != null && strInvoiceDate != "")
            {
                DateTime dtInvoiceDate = Convert.ToDateTime(strInvoiceDate);

                string InvDate = dtInvoiceDate.ToString("MM/dd/yyyy");

                sqlCmd.Parameters.Add("@INVDT", SqlDbType.VarChar).Value = InvDate;
            }
            else
            {
                sqlCmd.Parameters.Add("@INVDT", SqlDbType.VarChar).Value = string.Empty;
            }
            if (strInvoiceNumber != null && strInvoiceNumber != "")
            {
                sqlCmd.Parameters.Add("@INVNO", SqlDbType.VarChar).Value = strInvoiceNumber;
            }
            else
            {
                sqlCmd.Parameters.Add("@INVNO", SqlDbType.VarChar).Value = string.Empty;
            }
            if (intProgramTypID > 0)
            {
                sqlCmd.Parameters.Add("@PGMTYPID", SqlDbType.VarChar).Value = intProgramTypID.ToString();
            }
            else
            {
                sqlCmd.Parameters.Add("@PGMTYPID", SqlDbType.VarChar).Value = string.Empty;
            }
            if (intExtOrgID > 0)
            {
                sqlCmd.Parameters.Add("@EXTORGID", SqlDbType.VarChar).Value = intExtOrgID.ToString();
            }
            else
            {
                sqlCmd.Parameters.Add("@EXTORGID", SqlDbType.VarChar).Value = string.Empty;
            }
            if (intInternalOrgID > 0)
            {
                sqlCmd.Parameters.Add("@INTORGID", SqlDbType.VarChar).Value = intInternalOrgID.ToString();
            }
            else
            {
                sqlCmd.Parameters.Add("@INTORGID", SqlDbType.VarChar).Value = string.Empty;
            }
            if (intPersonID > 0)
            {
                sqlCmd.Parameters.Add("@CFS2PERSONID", SqlDbType.VarChar).Value = intPersonID.ToString();
            }
            else
            {
                sqlCmd.Parameters.Add("@CFS2PERSONID", SqlDbType.VarChar).Value = string.Empty;
            }
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            adapter.Fill(dtInvoiceInquiry);
            if (sqlconn != null)
                sqlconn.Close();
            return dtInvoiceInquiry;

        }
       

              
        /// <summary>
        /// Retrieves the Invoice Details for a specific criteria
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <returns>List of PolicyBE</returns>
        public List<InvoiceInquiryBE> getInvoiceData(int intCustmerID)
        {
            IList<InvoiceInquiryBE> result = new List<InvoiceInquiryBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<InvoiceInquiryBE> drftQuery = (from pol in this.Context.PREM_ADJs
                                                      where pol.reg_custmr_id == intCustmerID && pol.drft_invc_nbr_txt!=null orderby pol.drft_invc_nbr_txt
                                                 select new InvoiceInquiryBE
                                                 {
                                                     DRAFTINVTXT = pol.drft_invc_nbr_txt

                                                 }).Distinct();
            IQueryable<InvoiceInquiryBE> fnlQuery = (from pol in this.Context.PREM_ADJs
                                                     where pol.reg_custmr_id == intCustmerID && pol.fnl_invc_nbr_txt!=null orderby pol.fnl_invc_nbr_txt
                                                 select new InvoiceInquiryBE
                                                 {
                                                     DRAFTINVTXT = pol.fnl_invc_nbr_txt

                                                 }).Distinct();
            result = drftQuery.ToList();
            int j = drftQuery.ToList().Count;
            for (int i = 0; i < fnlQuery.ToList().Count; i++)
            {
                result.Insert(j + i, fnlQuery.ToList()[i]);
            }
            for (int i = 0; i < result.ToList().Count; i++)
            {
                if (result.ToList()[i].DRAFTINVTXT == null || result.ToList()[i].DRAFTINVTXT=="")
                    result.RemoveAt(i);
            }

            return result.ToList();
        }

        /// <summary>
        /// Retrieves the Invoice Details for a specific criteria
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <returns>List of PolicyBE</returns>
        public List<InvoiceInquiryBE> getInvoiceDates(int intCustmerID)
        {
            IList<InvoiceInquiryBE> result = new List<InvoiceInquiryBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<InvoiceInquiryBE> Query = (from pol in this.Context.PREM_ADJs
                                                  where pol.reg_custmr_id == intCustmerID && pol.invc_due_dt != null orderby pol.invc_due_dt
                                                      select new InvoiceInquiryBE
                                                      {
                                                          INVOICEDATE = pol.invc_due_dt.Value.ToShortDateString()

                                                      }).Distinct();

            result = Query.ToList();
            return result.ToList();
        }
       
       
    }
}
