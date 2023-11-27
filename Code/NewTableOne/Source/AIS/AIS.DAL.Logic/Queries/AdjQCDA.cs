using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ZurichNA.AIS.DAL.Logic
{
    public class AdjQCDA : DataAccessor<PREM_ADJ_ST, PremiumAdjustmentStatusBE, AISDatabaseLINQDataContext>
    {
        public IList<PremiumAdjustmentStatusBE> adjSetupQCItemInfo(int adjID)
        {
            IList<PremiumAdjustmentStatusBE> result = new List<PremiumAdjustmentStatusBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PremiumAdjustmentStatusBE> query =
                ( from pas in this.Context.PREM_ADJ_STs
                 join cust in this.Context.CUSTMRs
                 on pas.custmr_id equals cust.custmr_id
                 where pas.prem_adj_id == adjID
                  orderby pas.eff_dt descending
            

              select new PremiumAdjustmentStatusBE()
              {
                  PremumAdj_sts_ID = pas.prem_adj_sts_id,
                  PremumAdj_ID = pas.prem_adj_id,
                  Review_Cmplt_Date = pas.qlty_cntrl_dt,
                  CommentText = pas.cmmnt_txt,
                  ReviewPerson_ID = pas.qlty_cntrl_pers_id,
                  
                  PersonName = cust.full_nm,
//                  COMMENTID = pas.PREM_ADJ_CMMNT.prem_adj_cmmnt_id,
                  APPROVEINDICATOR = pas.aprv_ind,
                  ADJ_STS_TYP_ID = pas.adj_sts_typ_id,
                  UpdtDate=pas.updt_dt,
                  UpdtUserID=pas.updt_user_id,
                  
              });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
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

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        public string  ModAISProcessReconReview(int adjID, int usrID)
        {
            //bool status = false;
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandText = "ModAIS_Process_ReconReview";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Connection = conn;
            sqlCmd.Parameters.AddWithValue("@prem_adj_id", adjID);
            sqlCmd.Parameters.AddWithValue("@create_user_id", usrID);
            sqlCmd.Parameters.AddWithValue("@create_date_time", DateTime.Now);
            SqlParameter parmErr = new SqlParameter("@err_msg_output", SqlDbType.VarChar);
            parmErr.Direction = ParameterDirection.Output;
            parmErr.Size = -1;
            sqlCmd.Parameters.Add(parmErr);
            conn.Open();
            int i = sqlCmd.ExecuteNonQuery();
            conn.Close();
            string strError = Convert.ToString(sqlCmd.Parameters["@err_msg_output"].Value);
            return strError;
            //if (!string.IsNullOrWhiteSpace(strError))
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
        }
    }
}
