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
    public class ApplicationStatusLogBE : BusinessEntity<APLCTN_STS_LOG>
    {
        public ApplicationStatusLogBE()
            : base()
        {

        }

        public int APPLICATION_STS_LOG_ID { get { return Entity.aplctn_sts_log_id; } set { Entity.aplctn_sts_log_id = value; } }
        public string SRC_TXT { get { return Entity.src_txt; } set { Entity.src_txt = value; } }
        public int? CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public string SEV_CD { get { return Entity.sev_cd; } set { Entity.sev_cd = value; } }
        public string SHORT_DESC_TXT { get { return Entity.shrt_desc_txt; } set { Entity.shrt_desc_txt = value; } }
        public string FULL_DESC_TXT { get { return Entity.full_desc_txt; } set { Entity.full_desc_txt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public string CREATE_DATE_string { get; set; }
    }
}
