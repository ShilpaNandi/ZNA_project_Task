using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using log4net;

namespace ZurichNA.AIS.Business.Entities
{
    public class PostalAddressBE : BusinessEntity<POST_ADDR>
    {
        public int POSTALADDRESSID { get { return Entity.post_addr_id; } set { Entity.post_addr_id = value; } }
        public int PERSON_ID { get { return Entity.pers_id; } set { Entity.pers_id = value; } }
        public string ADDRESS1 { get { return Entity.addr_ln_1_txt; } set { Entity.addr_ln_1_txt = value; } }
        public string ADDRESS2 { get { return Entity.addr_ln_2_txt; } set { Entity.addr_ln_2_txt = value; } }
        public string CITY { get { return Entity.city_txt; } set { Entity.city_txt = value; } }        
        public int STATE_ID { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public string ZIP_CODE { get { return Entity.post_cd_txt; } set { Entity.post_cd_txt = value; } }
        public int CREATEDUSER { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATEDDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int? UPDATEDUSER { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATEDDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public string STATE_TXT { get; set; }
        public string STATE_CODE { get; set; }
    }
}
