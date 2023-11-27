using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using System.Data.Linq;

namespace ZurichNA.AIS.Business.Entities
{
    public class LookupBE : BusinessEntity<LKUP>
    {
        public LookupBE()
            : base()
        {
        }

        public int LookUpID { get { return Entity.lkup_id; } set { Entity.lkup_id = value; } }
        public string LookUpName { get { return Entity.lkup_txt; } set { Entity.lkup_txt = value; } }
        public int LookUpTypeID { get { return Entity.lkup_typ_id; } set { Entity.lkup_typ_id = value; } }
        public string Attribute1 { get { return Entity.attr_1_txt; } set { Entity.attr_1_txt = value; } }
        public bool? ACTIVE { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        private string _LookUpTypeName;
        public string LookUpTypeName { get { return _LookUpTypeName; } set { _LookUpTypeName = value; } }
        public DateTime Created_Date { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? Updated_Date { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int Created_UserID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime Effective_Date { get { return Entity.eff_dt; } set { Entity.eff_dt = value; } }
    }
}
