using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{
    public class PremumAdjustdmentStatusDA : DataAccessor<PREM_ADJ_ST, PremiumAdjustmentStatusBE, AISDatabaseLINQDataContext>
    {
        public IList<PremiumAdjustmentStatusBE> getPremumAdjustmentStatusList(int PremAdjID)
        {
            IList<PremiumAdjustmentStatusBE> result = new List<PremiumAdjustmentStatusBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PremiumAdjustmentStatusBE> query =
            (from pas in this.Context.PREM_ADJ_STs
             join cust in this.Context.CUSTMRs
             on pas.custmr_id equals cust.custmr_id
             where pas.prem_adj_id == PremAdjID
             orderby pas.prem_adj_sts_id descending
             select new PremiumAdjustmentStatusBE()
             {
                 PremumAdj_sts_ID = pas.prem_adj_sts_id,
                 PremumAdj_ID = pas.prem_adj_id,
                 Review_Cmplt_Date = pas.qlty_cntrl_dt,
                 CommentText = pas.cmmnt_txt,
                 ReviewPerson_ID = pas.qlty_cntrl_pers_id,
                 PersonName = cust.full_nm,
                 //                 COMMENTID=pas.PREM_ADJ_CMMNT.prem_adj_cmmnt_id,
                 APPROVEINDICATOR = pas.aprv_ind,
                 ADJ_STS_TYP_ID = pas.adj_sts_typ_id,
                 UpdtUserID=pas.updt_user_id,
                 UpdtDate=pas.updt_dt,

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        public string getPremiumAdjstmentStatus(int intPremAdjID)
        {

            string result
               = (from cdd in this.Context.PREM_ADJ_STs
                  where cdd.prem_adj_id == intPremAdjID
                  orderby cdd.prem_adj_sts_id descending
                  select cdd.LKUP.lkup_txt).First();

            return result;

        }

        //public string getPremiumAdjstmentStatusOld(int intPremAdjID)
        //{

        //    string result
        //       = (from cdd in this.Context.PREM_ADJs
        //          where cdd.prem_adj_id == intPremAdjID
        //          select cdd.adj_sts_typ_id).First().ToString();

        //    return result;

        //}

        /// <summary>
        /// retrives all Review Feedbacks of Adjustment Status.
        /// </summary>
        /// <param name="AdjID"></param>
        /// <param name="ReviewStage">AdjQC=1 and Recon =2</param>
        /// <returns></returns>
        public DataTable getReviewFeedback(int AdjID, int ReviewStage)
        {
            DataTable dtReviewFeedback = new DataTable();
            SqlConnection sqlconn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            sqlconn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString();
            sqlconn.Open();
            sqlCmd.Connection = sqlconn;
            sqlCmd.CommandText = "GetReviewFeedback";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@ADJNO", SqlDbType.Int).Value = AdjID;
            sqlCmd.Parameters.Add("@ReviewType", SqlDbType.Int).Value = ReviewStage;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            adapter.Fill(dtReviewFeedback);
            if (sqlconn != null)
                sqlconn.Close();
            return dtReviewFeedback;

        }

    }
}
