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
   public class ApplWebPageAudtBE:BusinessEntity<APLCTN_WEB_PAGE_AUDT>
    {
       public ApplWebPageAudtBE()
            : base()
        { 
        
        }
       public int APLCTN_WEB_PAGE_AUDT_ID
       { get { return Entity.aplctn_web_page_audt_id; } set { Entity.aplctn_web_page_audt_id = value; } }
        public int? CUSTMR_ID
        { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? PREM_ADJ_PGM_ID
        { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int WEB_PAGE_ID
        { get { return Entity.web_page_id; } set { Entity.web_page_id = value; } }
        public string WEB_PAGE_CNTRL_TXT
        { get { return Entity.web_page_cntrl_txt; } set { Entity.web_page_cntrl_txt = value; } }
        public DateTime CREATE_DATE
        { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CREATE_USER_ID
        { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }

    }
}
