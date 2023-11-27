using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Entities
{
    public class LookupTypeBE : BusinessEntity<LKUP_TYP>
    {
        public LookupTypeBE()
        {

        }
        public int LOOKUPTYPE_ID { get { return Entity.lkup_typ_id; } set { Entity.lkup_typ_id = value; } }
        public string LOOKUPTYPE_NAME { get { return Entity.lkup_typ_nm_txt; } set { Entity.lkup_typ_nm_txt = value; } }
        public bool? ACTIVE { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public DateTime CREATED_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CREATED_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime? UPDATED_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATED_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }

    }
}
