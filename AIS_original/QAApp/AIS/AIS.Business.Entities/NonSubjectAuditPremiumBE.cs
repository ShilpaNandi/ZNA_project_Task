using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Entities
{
    public class NonSubjectAuditPremiumBE : BusinessEntity<NON_SUBJ_PREM_AUDT>
    {
        public int N_Sub_Prem_Aud_ID { get { return Entity.non_subj_prem_audt_id; } set { Entity.non_subj_prem_audt_id = value; } }
        public int Coml_Agmt_Audt_ID { get { return Entity.coml_agmt_audt_id; } set { Entity.coml_agmt_audt_id = value; } }
        public int Coml_Agmt_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int Prem_Adj_Pgm_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int Nsa_Typ_ID { get { return Entity.nsa_typ_id; } set { Entity.nsa_typ_id = value; } }
        public int Custmr_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public decimal? Non_Subj_Audt_Prem_Amt { get { return Entity.non_subj_audt_prem_amt; } set { Entity.non_subj_audt_prem_amt = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CreatedUser_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UpdatedUser_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public bool? Active { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public string NSATYPE { get; set; }
    }
}