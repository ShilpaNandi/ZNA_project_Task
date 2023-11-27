using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Entities
{
    public class ApplWebPageAuthBE: BusinessEntity<APLCTN_WEB_PAGE_AUTH>
    {
        public ApplWebPageAuthBE()
        {

        }

        public int web_page_auth_id { get { return Entity.aplctn_web_page_auth_id; } set { Entity.aplctn_web_page_auth_id = value; } }
        public int menu_id { get { return Entity.aplctn_menu_id; } set { Entity.aplctn_menu_id = value; } }
        public int secur_gp_id { get { return Entity.secur_gp_id; } set { Entity.secur_gp_id = value; } }
        public Boolean? authd_ind { get { return Entity.authd_ind; } set { Entity.authd_ind = value; } }

        public string menu_nm_txt { get { return Entity.APLCTN_MENU.menu_nm_txt; } set { ; } }
        public string menu_tooltip_txt { get { return Entity.APLCTN_MENU.menu_tooltip_txt; } set { ;} }
        public int? parnt_id { get { return Entity.APLCTN_MENU.parnt_id; } set { ; } }
        public string web_page_txt { get { return Entity.APLCTN_MENU.web_page_txt; } set { ; } }
    }
}
