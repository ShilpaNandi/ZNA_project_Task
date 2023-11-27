using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;


namespace ZurichNA.AIS.Business.Entities
{
    public class PremiumAdjustmentPeriodBE : BusinessEntity<PREM_ADJ_PERD>
    {
        public PremiumAdjustmentPeriodBE()
            : base()
        {

        }
        public string PREM_NON_PREM_CODE { get { return Entity.prem_non_prem_cd; } set { Entity.prem_non_prem_cd = value; } }
        public int PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int REG_CUSTMR_ID { get { return Entity.reg_custmr_id; } set { Entity.reg_custmr_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int? ADJ_NBR { get { return Entity.adj_nbr; } set { Entity.adj_nbr = value; } }
        public string ADJ_NBR_TXT { get { return Entity.adj_nbr_txt; } set { Entity.adj_nbr_txt = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int? ADJ_STS_ID { get; set; }
        public int? ADJ_STS_TYP_ID { get; set; }
        public string VALUATIONDATE { get; set; }
        public string CUSTMR_NAME { get; set; }
        public string ADJ_TYPE { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public bool? ADJ_NBR_MNL_OVERRID_IND { get { return Entity.adj_nbr_mnl_overrid_ind; } set { Entity.adj_nbr_mnl_overrid_ind = value; } }
        public bool? ESCR_MNL_OVERRID_IND { get { return Entity.escr_mnl_overrid_ind; } set { Entity.escr_mnl_overrid_ind = value; } }
        public decimal? ESCR_TOT_AMT { get { return Entity.escr_tot_amt; } set { Entity.escr_tot_amt = value; } }
        public string PROGRAM_PERIOD
        {
            get;
            set;
        }

    }

}