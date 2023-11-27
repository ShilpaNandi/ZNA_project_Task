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
    public class PaidLossBillingBE : BusinessEntity<PREM_ADJ_PAID_LOS_BIL>
    {
        public PaidLossBillingBE()
            : base()
        {

        }
        public int PREM_ADJ_PAID_LOS_BIL_ID { get { return Entity.prem_adj_paid_los_bil_id; } set { Entity.prem_adj_paid_los_bil_id = value; } }
        public int? PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public int? PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int? CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public string LSI_PGM_TYP { get { return Entity.lsi_pgm_typ_txt; } set { Entity.lsi_pgm_typ_txt = value; } }
        public Boolean? LSI_SRC { get { return Entity.lsi_src; } set { Entity.lsi_src = value; } }
        public DateTime? LSI_VALN_DATE { get { return Entity.lsi_valn_dt; } set { Entity.lsi_valn_dt = value; } }
        public Decimal? IDNMTY_AMT { get { return Entity.idnmty_amt; } set { Entity.idnmty_amt = value; } }
        public Decimal? ADJ_IDNMTY_AMT { get { return Entity.adj_idnmty_amt; } set { Entity.adj_idnmty_amt = value; } }
        public Decimal? EXPS_AMT { get { return Entity.exps_amt; } set { Entity.exps_amt = value; } }
        public Decimal? ADJ_EXPS_AMT { get { return Entity.adj_exps_amt; } set { Entity.adj_exps_amt = value; } }
        public Decimal? TOT_PAID_LOS_BIL_AMT { get { return Entity.tot_paid_los_bil_amt; } set { Entity.tot_paid_los_bil_amt = value; } }
        public Decimal? ADJ_TOT_PAID_LOS_BIL_AMT { get { return Entity.adj_tot_paid_los_bil_amt; } set { Entity.adj_tot_paid_los_bil_amt = value; } }
        public string CMMNT_TXT { get { return Entity.cmmnt_txt; } set { Entity.cmmnt_txt = value; } }
        public int? LOB_ID { get { return Entity.ln_of_bsn_id; } set { Entity.ln_of_bsn_id = value; } }
        public int? COML_AGMT_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int? PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CREATEUSER { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int? UPDATEDUSER { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CREATEDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? UPDATEDDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public string POLICYSYMBOL { get; set; }
        public string LOB { get; set; }
       
    }
}
