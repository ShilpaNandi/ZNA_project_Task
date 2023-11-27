using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using ZurichNA.AIS.DAL.Logic;
using System.Data;


namespace ZurichNA.AIS.DAL.Logic
{

    public class PremiumAdjLRFPostingDA : DataAccessor<PREM_ADJ_LOS_REIM_FUND_POST, PremiumAdjLRFPostingBE, AISDatabaseLINQDataContext>
    {
        public IList<PremiumAdjLRFPostingBE> getList()
        {
            IList<PremiumAdjLRFPostingBE> result = new List<PremiumAdjLRFPostingBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PremiumAdjLRFPostingBE> query =
            (from cdd in this.Context.PREM_ADJ_LOS_REIM_FUND_POSTs
             join prm in this.Context.PREM_ADJs on cdd.prem_adj_id equals prm.prem_adj_id
             join prd in this.Context.PREM_ADJ_PERDs on prm.prem_adj_id equals prd.prem_adj_id
             join trns in this.Context.POST_TRNS_TYPs on cdd.recv_typ_id equals trns.post_trns_typ_id
             join lk in this.Context.LKUPs on trns.trns_typ_id equals lk.lkup_id
             where trns.actv_ind == true && lk.lkup_txt.Trim().ToUpper()== "ILRF DEDUCTIBLE"
             select new PremiumAdjLRFPostingBE()
             {

                 Prem_Adj_Los_Reim_Fund_Post_ID = cdd.prem_adj_los_reim_fund_post_id,
                 PremAdjPerdID = cdd.prem_adj_perd_id,
                 PremAdjID = cdd.prem_adj_id,
                 CustomerID = cdd.custmr_id,
                 ReceivableTypID = cdd.recv_typ_id,
                 CurrntAmount = cdd.curr_amt,
                 AggrgateAmt = cdd.aggr_amt,
                 PriorYrAmt = cdd.prior_yy_amt,
                 AdjustPriorYrAmt = cdd.adj_prior_yy_amt,
                 PostedAmt = cdd.curr_amt + cdd.aggr_amt - cdd.adj_prior_yy_amt,
                 RecvType = trns.trns_nm_txt
                 //RecvType=(from cdd1 in this.Context.PREM_ADJ_LOS_REIM_FUND_POSTs
                 // join trns in this.Context.POST_TRNS_TYPs on cdd1.recv_typ_id equals trns.post_trns_typ_id
                 // select 
                 // trns.trns_nm_txt).Distinct().ToString()

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }


        public int getPreAdjLRFReserveID(int intPremAdjPerdID)
        {
           

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PremiumAdjLRFPostingBE> query =
            (from cdd in this.Context.PREM_ADJ_LOS_REIM_FUND_POSTs
             where cdd.recv_typ_id == 71 && cdd.prem_adj_perd_id==intPremAdjPerdID
             select new PremiumAdjLRFPostingBE()
             {
                 Prem_Adj_Los_Reim_Fund_Post_ID = cdd.prem_adj_los_reim_fund_post_id

             });

            if (query.Count() > 0)
            {
                return query.ToList()[0].Prem_Adj_Los_Reim_Fund_Post_ID;
            }
            else
            {
                return 0;
            }
        }


        public IList<PremiumAdjLRFPostingBE> getLRFList(int acctID, int prmAdjID, int prmAdjPrdID)
     {
         IList<PremiumAdjLRFPostingBE> result = new List<PremiumAdjLRFPostingBE>();

         if (this.Context == null)
             this.Initialize();

         /// Generate query to retrieve account information
         /// and project it into Account Business Entity
         IQueryable<PremiumAdjLRFPostingBE> query =
         (from cdd in this.Context.PREM_ADJ_LOS_REIM_FUND_POSTs
          join prm in this.Context.PREM_ADJs on cdd.prem_adj_id equals prm.prem_adj_id
          join prd in this.Context.PREM_ADJ_PERDs on prm.prem_adj_id equals prd.prem_adj_id 
          join trns in this.Context.POST_TRNS_TYPs on cdd.recv_typ_id equals trns.post_trns_typ_id
          join lk in this.Context.LKUPs on trns.trns_typ_id equals lk.lkup_id
          where trns.actv_ind == true && lk.lkup_txt.Trim().ToUpper() == "ILRF ADJUSTMENT" 
          && cdd.prem_adj_id == prmAdjID
          && cdd.prem_adj_perd_id == prd.prem_adj_perd_id
          //          && cdd.prem_adj_perd_id == prmAdjPrdID && cdd.custmr_id == acctID
          select new PremiumAdjLRFPostingBE()
          {

              Prem_Adj_Los_Reim_Fund_Post_ID = cdd.prem_adj_los_reim_fund_post_id,
              PremAdjPerdID = cdd.prem_adj_perd_id,
              PremAdjID = cdd.prem_adj_id,
              CustomerID = cdd.custmr_id,
              ReceivableTypID = cdd.recv_typ_id,
              CurrntAmount = cdd.curr_amt,
              AggrgateAmt = cdd.aggr_amt,
              PriorYrAmt = cdd.prior_yy_amt,
              AdjustPriorYrAmt = cdd.adj_prior_yy_amt,
              
              RecvType = trns.trns_nm_txt,
              PostedAmt = cdd.post_amt
              
              
              //PostedAmt = Convert.ToDecimal(cdd.curr_amt) + Convert.ToDecimal(cdd.aggr_amt == null ? 0 : cdd.aggr_amt) - Convert.ToDecimal(cdd.adj_prior_yy_amt)
              // join trns in this.Context.POST_TRNS_TYPs on cdd1.recv_typ_id equals trns.post_trns_typ_id
              // select 
              // trns.trns_nm_txt).Distinct().ToString()

          });

         if (query.Count() > 0)
         {
             result = query.ToList();

             for (int i = 0; i < result.Count; i++)
             {
                 if (result[i].PostedAmt != null)
                 {
                     result[i].PostedAmt = result[i].CurrntAmount + (result[i].AggrgateAmt == null ? 0 : result[i].AggrgateAmt) - result[i].AdjustPriorYrAmt;
                 }
             }
             
         }
         return result;
         
     }

        public DataTable getLRFData(int intPremAdjID,int intPremAdjPerdID,int intCustmrID)
        {
            DataTable dtLRF = new DataTable();
            SqlConnection sqlconn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            sqlconn.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString();
            sqlconn.Open();
            sqlCmd.Connection = sqlconn;
            sqlCmd.CommandText = "GetAdjReviewILRFPostFund";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@ADJNO", SqlDbType.Int).Value = intPremAdjID;
            sqlCmd.Parameters.Add("@PREMADJPERDID", SqlDbType.Int).Value = intPremAdjPerdID;
            sqlCmd.Parameters.Add("@CUSTMRID", SqlDbType.Int).Value = intCustmrID;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            adapter.Fill(dtLRF);
            if (sqlconn != null)
                sqlconn.Close();
            return dtLRF;

            if (dtLRF.Rows.Count > 0)
            {
                for (int i = 0; i < dtLRF.Rows.Count; i++)
                {
                    if (dtLRF.Rows[i]["POST AMT"] != null && dtLRF.Rows[i]["POST AMT"].ToString() !="")
                    {
                        //dtLRF.Rows[i]["POST AMT"] = Convert.ToDecimal(dtLRF.Rows[i]["CURRENT AMOUNT"]) + (dtLRF.Rows[i]["AGGR AMT"] == null ? 0 : Convert.ToDecimal(dtLRF.Rows[i]["AGGR AMT"])) - Convert.ToDecimal(dtLRF.Rows[i]["ADJ PRIOR YY AMT"]);
                        decimal currentAmount = 0.0m;
                        decimal AggrAmount = 0.0m;
                        decimal AdjPriorYYAmount = 0.0m;
                        if (dtLRF.Rows[i]["CURRENT AMOUNT"] != null && dtLRF.Rows[i]["CURRENT AMOUNT"].ToString() != "")
                        {
                            currentAmount = Convert.ToDecimal(dtLRF.Rows[i]["CURRENT AMOUNT"]);
                        }
                        if (dtLRF.Rows[i]["AGGR AMT"] != null && dtLRF.Rows[i]["AGGR AMT"].ToString() !="")
                        {
                            AggrAmount = Convert.ToDecimal(dtLRF.Rows[i]["AGGR AMT"]);
                        }
                        if(dtLRF.Rows[i]["ADJ PRIOR YY AMT"]!=null && dtLRF.Rows[i]["ADJ PRIOR YY AMT"].ToString()!="")
                        {
                            AdjPriorYYAmount=Convert.ToDecimal(dtLRF.Rows[i]["ADJ PRIOR YY AMT"]);
                        }
                        dtLRF.Rows[i]["POST AMT"] = currentAmount + AggrAmount - AdjPriorYYAmount;
                    }
                }
            
            }
        }
    }
}
