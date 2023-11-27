using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Entities
{
    public class BusinessUnitOfficeBE:BusinessEntity<INT_ORG>
    {
        public BusinessUnitOfficeBE()
            : base()
        { }

        public int  INTRNL_ORG_ID {get{return Entity.int_org_id;} set{Entity.int_org_id=value;}}
        public string FULL_NAME {get{return Entity.full_name;} set{Entity.full_name=value;}}
        public string BSN_UNT_CD {get{return Entity.bsn_unt_cd;}set{Entity.bsn_unt_cd=value;}}
        public string CITY_NM { get { return Entity.city_nm; } set { Entity.city_nm = value; } }
        public string OFC_CD { get { return Entity.ofc_cd; } set { Entity.ofc_cd = value; } }
        public bool ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public DateTime CREATED_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CREATED_USERID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime? UPDATED_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATED_USERID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
    }
}
