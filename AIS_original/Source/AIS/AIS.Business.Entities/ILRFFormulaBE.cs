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
    public class ILRFFormulaBE : BusinessEntity<INCUR_LOS_REIM_FUND_FRMLA>
    {
        public ILRFFormulaBE()
        {

        }
        public int INCURRED_LOSS_REIM_FUND_FRMLA_ID { get { return Entity.incur_los_reim_fund_frmla_id; } set { Entity.incur_los_reim_fund_frmla_id = value; } }
        public int PROGRAMPERIOD_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTOMER_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int LOSS_REIM_FUND_FACTOR_TYPE_ID { get { return Entity.los_reim_fund_fctr_typ_id; } set { Entity.los_reim_fund_fctr_typ_id = value; } }

        public bool? USE_PAID_LOSS_INDICATOR { get { return Entity.use_paid_los_ind; } set { Entity.use_paid_los_ind = value; } }
        public bool? USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR { get { return Entity.use_paid_aloc_los_adj_exps_ind; } set { Entity.use_paid_aloc_los_adj_exps_ind = value; } }
        public bool? USE_RESERVE_LOSS_INDICATOR { get { return Entity.use_resrv_los_ind; } set { Entity.use_resrv_los_ind = value; } }
        public bool? USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR { get { return Entity.use_resrv_aloc_los_adj_exps_ind; } set { Entity.use_resrv_aloc_los_adj_exps_ind = value; } }

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }

        public string LOSS_REIM_FUND_FACTOR_TYPE { get; set; }
    }
}
