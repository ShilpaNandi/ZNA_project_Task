using System;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class Prem_Adj_Dpst_ChbxBE : BusinessEntity<PREM_ADJ_DPST_CHBX>
    {
        public Prem_Adj_Dpst_ChbxBE()
            : base()
        {

        }
        public int prem_adj_dpst_chbx_id { get { return Entity.prem_adj_dpst_chbx_id; } set { Entity.prem_adj_dpst_chbx_id = value; } }        
        public int prem_adj_pgm_setup_id { get { return Entity.prem_adj_pgm_setup_id; } set { Entity.prem_adj_pgm_setup_id = value; } }
        public int prem_adj_pgm_id { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public Boolean? dpst_ind { get { return Entity.dpst_ind; } set { Entity.dpst_ind = value; } }
        public int Cstmr_Id { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int? AdjparameterTypeID { get { return Entity.adj_parmet_typ_id; } set { Entity.adj_parmet_typ_id = value; } }
               
    }
}
