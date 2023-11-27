using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using System.Data.Linq;

namespace ZurichNA.AIS.Business.Entities
{
    public class Coml_AgmtBE : BusinessEntity<COML_AGMT>
    {
        public Coml_AgmtBE()
            : base()
        {
        }

        public int Comm_Agr_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int Prem_Adj_Prg_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int Customer_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public string Pol_Sym_Txt { get { return Entity.pol_sym_txt; } set { Entity.pol_sym_txt = value; } }
        public string Pol_Nbr_Txt { get { return Entity.pol_nbr_txt; } set { Entity.pol_nbr_txt = value; } }
        public string Pol_Mod_Txt { get { return Entity.pol_modulus_txt; } set { Entity.pol_modulus_txt = value; } }
        public string POLICY { get; set; }
        public int? ADJ_TYP_ID { get { return Entity.adj_typ_id; } set { Entity.adj_typ_id = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime PlannedEndDate { get { return Entity.planned_end_date; } set { Entity.planned_end_date = value; } }
        public int CreatedUserID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
    }
}
