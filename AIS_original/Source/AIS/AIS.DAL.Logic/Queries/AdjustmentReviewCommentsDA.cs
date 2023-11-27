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
                    case "REMITTANCE ADVICE":
                        strStoredProcedure = "GetAdjustmentInvoice";
                        break;
                    case "CHF":
                        strStoredProcedure = "GetCHF";
                        break;
                    case "LOSS FUND":
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
                    //case "NY-SIF":
                    //    strStoredProcedure = "GetAdjNYSecInjFund";
                    //    break;
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
                    case "RETRO PREMIUM BASED SURCHARGES & ASSESSMENTS":
                        strStoredProcedure = "GetSurchargesAssessments";
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
                    case "PAID LOSS BILLINGS FOR CURRENT ADJUSTMENT PERIOD":
                        strStoredProcedure = "GetCumTotWkSheet";
                        break;
                    case "STATE SALES & SERVICE TAX EXHIBIT (EXTERNAL)":
                        strStoredProcedure = "GetTexasTax";
                        break;
                    case "STATE SALES & SERVICE TAX EXHIBIT (INTERNAL)":
                        strStoredProcedure = "GetTexasTax";
                        break;

                }
                DataTable dtReportData = new DataTable();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = 7200;
                sqlCmd.Connection = sqlconn;
                sqlCmd.CommandText = strStoredProcedure;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@ADJNO", SqlDbType.Int).Value = intPremAdjID;
                sqlCmd.Parameters.Add("@FLAG", SqlDbType.Int).Value = IFlag;
                sqlCmd.Parameters.Add("@HISTFLAG", SqlDbType.Bit).Value = false;
                if (strDocName.ToUpper() == "RETRO PREMIUM BASED SURCHARGES & ASSESSMENTS")
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
                if (strDocName.ToUpper() == "REMITTANCE ADVICE")
                {
                    sqlCmd.Parameters.Add("@CMTCATGID", SqlDbType.Int).Value = 318;
                }
                if (strDocName.ToUpper() == "STATE SALES & SERVICE TAX EXHIBIT (EXTERNAL)")
                {
                    sqlCmd.Parameters.Add("@CMTCATGID", SqlDbType.Int).Value = 533;
                }
                if (strDocName.ToUpper() == "STATE SALES & SERVICE TAX EXHIBIT (INTERNAL)")
                {
                    sqlCmd.Parameters.Add("@CMTCATGID", SqlDbType.Int).Value = 534;
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

        /// <summary>
        ///This method is to verify the duplicate comments for this adjustment for the summaryInvoice
        ///Users are allowed to enter the comments for only one program period under
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateComment(int premAdjID, int custmrID, int cmmntCatgID)
        {
            try
            {
                if (this.Context == null)
                    this.Initialize();

                /// Generate query to retrieve Adjustment Review Comments information
                /// and project it into AdjustmentReviewComments Business Entity
                IQueryable<AdjustmentReviewCommentsBE> query =
                   (from prmadjcmmnt in this.Context.PREM_ADJ_CMMNTs
                    join prmadjprd in this.Context.PREM_ADJ_PERDs
                    on prmadjcmmnt.prem_adj_perd_id equals prmadjprd.prem_adj_perd_id
                    where prmadjcmmnt.cmmnt_catg_id == cmmntCatgID && prmadjprd.custmr_id == custmrID
                    && prmadjprd.prem_adj_id == premAdjID
                    select new AdjustmentReviewCommentsBE()
                    {
                        PREM_ADJ_CMMNT_ID = prmadjcmmnt.prem_adj_cmmnt_id
                        
                    }
                );

                /// Force an enumeration so that the SQL is only
                /// executed in this method

                if (query.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
               return false;
            }

        }

        /// <summary>
        ///This method is to get the Program Period for which comments already exists for this adjustment for the summaryInvoice
        ///Users are allowed to enter the comments for only one program period 
        /// </summary>
        /// <returns></returns>
       public ProgramPeriodSearchListBE getProgramPeriod(int premAdjID, int custmrID, int cmmntCatgID)
        {
            ProgramPeriodSearchListBE result = new ProgramPeriodSearchListBE();
            try
            {
                if (this.Context == null)
                    this.Initialize();

                /// Generate query to retrieve Adjustment Review Comments information
                /// and project it into AdjustmentReviewComments Business Entity
                ProgramPeriodSearchListBE query =
                   (from prmadjcmmnt in this.Context.PREM_ADJ_CMMNTs
                    join prmadjprd in this.Context.PREM_ADJ_PERDs
                    on prmadjcmmnt.prem_adj_perd_id equals prmadjprd.prem_adj_perd_id
                    where prmadjcmmnt.cmmnt_catg_id == cmmntCatgID && prmadjprd.custmr_id == custmrID
                    && prmadjprd.prem_adj_id == premAdjID
                    select new ProgramPeriodSearchListBE()
                    {
                        PREM_ADJ_PGM_ID = prmadjprd.PREM_ADJ_PGM.prem_adj_pgm_id,
                        STRT_DT = prmadjprd.PREM_ADJ_PGM.strt_dt,
                        PLAN_END_DT = prmadjprd.PREM_ADJ_PGM.plan_end_dt,
                        STARTDATE_ENDDATE = prmadjprd.PREM_ADJ_PGM.strt_dt + ":" + prmadjprd.PREM_ADJ_PGM.plan_end_dt
                        
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
      
    }
}
