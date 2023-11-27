using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Entities
{
    public class ApplMenuBE:BusinessEntity<APLCTN_MENU>
    {
        public ApplMenuBE()
            :base()
        { 

        }

        public int menu_id { get { return Entity.aplctn_menu_id; } set { Entity.aplctn_menu_id = value; } }
        public string menu_nm_txt { get { return Entity.menu_nm_txt; } set { Entity.menu_nm_txt = value; } }
        public int? parnt_id { get { return Entity.parnt_id; } set { Entity.parnt_id = value; } }
        public string web_page_txt { get { return Entity.web_page_txt; } set { Entity.web_page_txt = value; } }
        public string menu_tooltip_txt { get { return Entity.menu_tooltip_txt; } set { Entity.menu_tooltip_txt = value; } }
        public bool? actv_ind { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public string depnd_txt { get { return Entity.depnd_txt; } set { Entity.depnd_txt = value; } }

        public char? InquiryOrFull_Access_ind { get; set; }
    }


}
