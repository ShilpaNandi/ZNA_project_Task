using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
namespace ZurichNA.AIS.Business.Entities
{
    public class AssignERPFormulaBE : BusinessEntity<PREM_ADJ_PGM>
    {
        public int PremiumAdjustmentProgramID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int? MasterEarnedRetroPremiumFormulaID { get { return Entity.mstr_ernd_retro_prem_frmla_id; } set { Entity.mstr_ernd_retro_prem_frmla_id = value; } }
        
    }
}
