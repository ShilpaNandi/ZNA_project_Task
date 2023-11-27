using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using log4net;


namespace ZurichNA.AIS.Business.Entities
{
 public class PremiumAdjLRFPostingBE : BusinessEntity<PREM_ADJ_LOS_REIM_FUND_POST>
    {

     public PremiumAdjLRFPostingBE()
            : base()
        {

        }
     public int Prem_Adj_Los_Reim_Fund_Post_ID { get { return Entity.prem_adj_los_reim_fund_post_id; } set { Entity.prem_adj_los_reim_fund_post_id = value; } }
     public int PremAdjPerdID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
     public int PremAdjID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
     public int CustomerID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
     public int? ReceivableTypID { get { return Entity.recv_typ_id; } set { Entity.recv_typ_id = value; } }
     public decimal? CurrntAmount { get { return Entity.curr_amt; } set { Entity.curr_amt = value; } }
     public decimal? AggrgateAmt { get { return Entity.aggr_amt; } set { Entity.aggr_amt = value; } }
     public decimal? PriorYrAmt { get { return Entity.prior_yy_amt; } set { Entity.prior_yy_amt = value; } }
     public decimal? AdjustPriorYrAmt { get { return Entity.adj_prior_yy_amt; } set { Entity.adj_prior_yy_amt = value; } }
     public decimal? PostedAmt { get { return Entity.post_amt; } set { Entity.post_amt = value; } }
     public decimal? lim_amt { get { return Entity.lim_amt; } set { Entity.lim_amt = value; } }
     public string RecvType { get; set; }
     public int? UpdatedUser_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
     public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
     public int CreatedUserID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
     public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        
    }
}
