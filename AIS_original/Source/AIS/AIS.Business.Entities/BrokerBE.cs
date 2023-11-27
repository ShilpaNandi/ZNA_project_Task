using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class BrokerBE : BusinessEntity< EXTRNL_ORG>
    {
        public BrokerBE()
        {

        }

        public int EXTRNL_ORG_ID {get{return Entity.extrnl_org_id;} set{Entity.extrnl_org_id=value;}}
        public string FULL_NAME {get{return Entity.full_name;} set{Entity.full_name=value;}}
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public int CONTACT_TYPE_ID { get { return Entity.role_id; } set { Entity.role_id = value; } }

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }


        public string CONTACT_TYPE { get; set; }

    }
}
