using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using System.Data;
using ZurichNA.AIS.ExceptionHandling;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// DataAccessor for Adjustment Review Comments
    /// </summary>
    public class AdjustmentReviewCommentsDA : DataAccessor<PREM_ADJ_CMMNT, AdjustmentReviewCommentsBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Returns all Adjustment Review Comments that match criteria
        /// </summary>
        /// <returns></returns>
        public AdjustmentReviewCommentsBE getAdjReviewCmmntINVOICEData(int cmmntCatgID, int PrgAdjID, int custmrID)
        {
            AdjustmentReviewCommentsBE result = new AdjustmentReviewCommentsBE();
            try
            {
                if (this.Context == null)
                    this.Initialize();

                /// Generate query to retrieve Adjustment Review Comments information
                /// and project it into AdjustmentReviewComments Business Entity
                AdjustmentReviewCommentsBE query =
                (from prmadjcmmnt in this.Context.PREM_ADJ_CMMNTs
                 join prmadj in this.Context.PREM_ADJs
                 on prmadjcmmnt.prem_adj_id equals prmadj.prem_adj_id
                 where prmadjcmmnt.cmmnt_catg_id == cmmntCatgID &&
                 prmadj.prem_adj_id == PrgAdjID
                 && prmadj.reg_custmr_id == custmrID
                 select new AdjustmentReviewCommentsBE()
                 {
                     PREM_ADJ_CMMNT_ID = prmadjcmmnt.prem_adj_cmmnt_id,
                     CMMNT_CATG_ID = prmadjcmmnt.cmmnt_catg_id,
                     CMMNT_TXT = prmadjcmmnt.cmmnt_txt,
                     CREATEDATE = prmadjcmmnt.crte_dt,
                     CREATEUSER = prmadjcmmnt.crte_user_id,
                     UPDATEDDATE=prmadjcmmnt.updt_dt

                 }
                ).Single();

                /// Force an enumeration so that the SQL is only
                /// executed in this method

                result = query;
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }

        }
        /// <summary>
        /// Returns all Adjustment Review Comments that match criteria
        /// </summary>
        /// <returns></returns>
        public AdjustmentReviewCommentsBE getAdjReviewCmmntALLData(int cmmntCatgID, int pgmPrdID, int custmrID)
        {
            AdjustmentReviewCommentsBE result = new AdjustmentReviewCommentsBE();
            try
            {
                if (this.Context == null)
                    this.Initialize();

                /// Generate query to retrieve Adjustment Review Comments information
                /// and project it into AdjustmentReviewComments Business Entity
                AdjustmentReviewCommentsBE query =
                   (from prmadjcmmnt in this.Context.PREM_ADJ_CMMNTs
                    join prmadjprd in this.Context.PREM_ADJ_PERDs
                    on prmadjcmmnt.prem_adj_perd_id equals prmadjprd.prem_adj_perd_id
                    where prmadjcmmnt.cmmnt_catg_id == cmmntCatgID && prmadjprd.custmr_id == custmrID
                    && prmadjprd.prem_adj_perd_id==pgmPrdID
                    select new AdjustmentReviewCommentsBE()
                    {
                        PREM_ADJ_CMMNT_ID = prmadjcmmnt.prem_adj_cmmnt_id,
                        CMMNT_CATG_ID = prmadjcmmnt.cmmnt_catg_id,
                        CMMNT_TXT = prmadjcmmnt.cmmnt_txt,
                        CREATEDATE = prmadjcmmnt.crte_dt,
                        CREATEUSER = prmadjcmmnt.crte_user_id,
                        UPDATEDDATE=prmadjcmmnt.updt_dt

                    }
                ).Single();

                /// Force an enumeration so that the SQL is only
                /// executed in this method

                result = query;
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }

        }

        /// <summary>
        /// Returns whether preview is available or not
        /// </summary>
        /// <param name="intPremAdjID"></param>
        /// <param name="IFlag"></param>
        /// <param name="strDocName"></param>
        /// <returns></returns>
        public bool IsReportAvialable(int intPremAdjID, int IFlag, string strDocName)
        {
            SqlConnection sqlconn = new SqlConnection();
            sqlconn.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString();
            sqlconn.Open();
            try
            {
                string strStoredProcedure = string.Empty;
                switch (strDocName.ToUpper())
                {

                    case "SUMMARY INVOICE":
                        strStoredProcedure = "GetAdjustmentInvoice";
                        break;
                    case "CHF":
                        strStoredProcedure = "GetCHF";
                        break;
                    case "ESCROW":
                        strStoredProcedure = "GetEscrow";
                        break;
                    case "ILRF (INTERNAL)":
                        strStoredProcedure = "GetILRF";
                        break;
                    case "ILRF (EXTERNAL)":
                        strStoredProcedure = "GetILRF";
                        break;
                    case "LBA":
                        strStoredProcedure = "GetLBA";
                        break;
                    case "COMBINED ELEMENTS":
                        strStoredProcedure = "GetCombinedElements";
                        break;
                    case "NY-SIF":
                        strStoredProcedure = "GetAdjNYSecInjFund";
                        break;
                    case "WA":

                        break;
                    case "RML":
                        strStoredProcedure = "GetResidualMSC";
                        break;
                    case "EXCESS LOSS":
                        strStoredProcedure = "GetExcessLossExhibit";
                        break;
                    case "RETROSPECTIVE PREMIUM ADJUSTMENT":
                        strStoredProcedure = "GetRetroSummary";
                        break;
                    case "RETROSPECTIVE ADJUSTMENT LEGEND":
                        strStoredProcedure = "GetLegendPgmInfo";
                        break;
                    case "BROKER LETTER":
                        strStoredProcedure = "GetAdjustmentInvoice";
                        break;
                    case "KY & OR TAXES":
                        strStoredProcedure = "GetWorkCompTxAsseKeOr";
                        break;
                    case "PROCESSING CHECKLIST":
                        strStoredProcedure = "GetProcessingChecklist";
                        break;
                    case "ARIES POSTING DETAILS":
                        strStoredProcedure = "GetARiESPostingDetails";
                        break;
                    case "CESAR CODING WORKSHEET":
                        strStoredProcedure = "GetCesarCodingWorksheet";
                        break;
                    case "CUMULATIVE TOTALS":
                        strStoredProcedure = "GetCumTotWkSheet";
                        break;
                }
                DataTable dtReportData = new DataTable();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlconn;
                sqlCmd.CommandText = strStoredProcedure;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@ADJNO", SqlDbType.Int).Value = intPremAdjID;
                sqlCmd.Parameters.Add("@FLAG", SqlDbType.Int).Value = IFlag;
                sqlCmd.Parameters.Add("@HISTFLAG", SqlDbType.Bit).Value = false;
                if (strDocName.ToUpper() == "NY-SIF" || strDocName.ToUpper() == "KY & OR TAXES")
                {
                    sqlCmd.Parameters.Add("@REVFLAGPREV", SqlDbType.Bit).Value = 0;
                }
                if (strDocName.ToUpper() == "ILRF (INTERNAL)")
                {
                    sqlCmd.Parameters.Add("@CMTCATGID", SqlDbType.Int).Value = 318;
                }
                if (strDocName.ToUpper() == "RML")
                {
                    sqlCmd.Parameters.Add("@REVFLAGPREV", SqlDbType.Int).Value = 0;
                }
                if (strDocName.ToUpper() == "ILRF (EXTERNAL)")
                {
                    sqlCmd.Parameters.Add("@CMTCATGID", SqlDbType.Int).Value = 375;
                }
                if (strDocName.ToUpper() == "BROKER LETTER")
                {
                    sqlCmd.Parameters.Add("@CMTCATGID", SqlDbType.Int).Value = 339;
                }
                if (strDocName.ToUpper() == "SUMMARY INVOICE")
                {
                    sqlCmd.Parameters.Add("@CMTCATGID", SqlDbType.Int).Value = 319;
                }
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                adapter.Fill(dtReportData);

                if (dtReportData.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlconn != null)
                    sqlconn.Close();
            }

        }
      
    }
}
