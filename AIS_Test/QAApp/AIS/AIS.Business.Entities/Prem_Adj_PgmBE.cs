using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
namespace ZurichNA.AIS.Business.Entities
{
    public class Prem_Adj_PgmBE : BusinessEntity<PREM_ADJ_PGM>
    {
        public int PremiumAdjustmentProgramID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int? MasterEarnedRetroPremiumFormulaID { get { return Entity.mstr_ernd_retro_prem_frmla_id; } set { Entity.mstr_ernd_retro_prem_frmla_id = value; } }
        public DateTime? QUALITYCONTROL_DATE { get { return Entity.qlty_cntrl_dt; } set { Entity.qlty_cntrl_dt = value; } }
        public string QUALITYCOMMENT_TEXT { get { return Entity.qlty_cmmnt_txt; } set { Entity.qlty_cmmnt_txt = value; } }
        public int? QUALITYCONTROL_PERSON_ID { get { return Entity.qlty_cntrl_pers_id; } set { Entity.qlty_cntrl_pers_id = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CreatedUser_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int CUSTOMERID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? UpdatedUser_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public string PersonName { get; set; }
    }
}
