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
    public class RetroInfoBE : BusinessEntity<PREM_ADJ_PGM_RETRO>
    {
        public RetroInfoBE()
            : base()
        {
        }
        
        public int ADJ_RETRO_INFO_ID { get { return Entity.prem_adj_pgm_retro_id; } set { Entity.prem_adj_pgm_retro_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public Boolean? RETRO_ADJ_FCTR_APLCBL_IND { get { return Entity.retro_adj_fctr_aplcbl_ind; } set { Entity.retro_adj_fctr_aplcbl_ind = value; } }
        public Boolean? NO_LIM_IND { get { return Entity.no_lim_ind; } set { Entity.no_lim_ind = value; } }
        public int? EXPO_TYP_ID { get { return Entity.expo_typ_id; } set { Entity.expo_typ_id = value; } }
        public decimal? EXPO_AGMT_AMT { get { return Entity.expo_agmt_amt; } set { Entity.expo_agmt_amt = value; } }
        public decimal? RETRO_ADJ_FCTR_RT { get { return Entity.retro_adj_fctr_rt; } set { Entity.retro_adj_fctr_rt = value; } }
        public decimal? TOT_AGMT_AMT { get { return Entity.tot_agmt_amt; } set { Entity.tot_agmt_amt = value; } }
        public decimal? AUDT_EXPO_AMT { get { return Entity.audt_expo_amt; } set { Entity.audt_expo_amt = value; } }
        public decimal? AGGR_FCTR_PCT { get { return Entity.aggr_fctr_pct; } set { Entity.aggr_fctr_pct = value; } }
        public decimal? TOT_AUDT_AMT { get { return Entity.tot_audt_amt; } set { Entity.tot_audt_amt = value; } }
        public int? RETRO_ELEMT_TYP_ID { get { return Entity.retro_elemt_typ_id; } set { Entity.retro_elemt_typ_id = value; } }
        public int? EXPO_TYP_INCREMNT_NBR_ID { get { return Entity.expo_typ_incremnt_nbr_id; } set { Entity.expo_typ_incremnt_nbr_id = value; } }
        public int? UPDT_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDT_DT { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DT { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        private string _Retro_Elemt;
        public int? SEQ_NO { get; set; }
        public string EXP_BASIS { get; set; }
        public string RETRO_ELEMT { get { return _Retro_Elemt; } set { _Retro_Elemt = value; } }
        private Decimal? _tot_subj_audt_prem_amt;
        public Decimal? TOT_SUBJ_AUDT_PREM_AMT { get { return _tot_subj_audt_prem_amt; } set { _tot_subj_audt_prem_amt = value; } }



    }
}
