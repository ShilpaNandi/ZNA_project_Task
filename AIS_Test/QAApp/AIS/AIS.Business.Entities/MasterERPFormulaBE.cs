using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;


namespace ZurichNA.AIS.Business.Entities
{
    public class MasterERPFormulaBE : BusinessEntity<MSTR_ERND_RETRO_PREM_FRMLA>
    {
        public int FormulaID { get { return Entity.mstr_ernd_retro_prem_frmla_id; } set { Entity.mstr_ernd_retro_prem_frmla_id = value; } }
        public string FormulaOneText { get { return Entity.ernd_retro_prem_frmla_one_txt; } set { Entity.ernd_retro_prem_frmla_one_txt = value; } }
        public string FormulaTwoText { get { return Entity.ernd_retro_prem_frmla_two_txt; } set { Entity.ernd_retro_prem_frmla_two_txt = value; } }
        public string FormulaDescription { get { return Entity.ernd_retro_prem_frmla_desc_txt; } set { Entity.ernd_retro_prem_frmla_desc_txt = value; } }
        public Boolean? IsActive { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public DateTime? UPDATED_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATED_USERID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CREATED_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CREATED_USERID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }

    }
}
