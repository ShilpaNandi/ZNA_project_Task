using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;

namespace ZurichNA.AIS.DAL.Logic
{
    public class AppWebPageAuthDA : DataAccessor<APLCTN_MENU, ApplMenuBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Constructor for AppWebPageAuthDA
        /// </summary>
        public AppWebPageAuthDA()
        {
 
        }

        /// <summary>
        /// Retrieves Authorized Web Pages
        /// </summary>
        /// <param name="rolename">Role Name</param>
        /// <returns>List of ApplMenuBE</returns>
        public IList<ApplMenuBE> RetrieveAuthWebPages(string rolename)
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<ApplMenuBE> result = (from mnu in Context.APLCTN_MENUs
                                             join auth in Context.APLCTN_WEB_PAGE_AUTHs on mnu.aplctn_menu_id equals auth.aplctn_menu_id
                                             join lk in Context.LKUPs on auth.secur_gp_id equals lk.lkup_id
                                             join lkTyp in Context.LKUP_TYPs on lk.lkup_typ_id equals lkTyp.lkup_typ_id
                                             where lk.lkup_txt == rolename && auth.authd_ind == true
                                                    && lkTyp.lkup_typ_nm_txt == "ROLES"
                                                    && mnu.web_page_txt != ""
                                                    //&& mnu.actv_ind == true 
                                                    && lkTyp.actv_ind == true
                                             select new ApplMenuBE()
                                             {
                                                 web_page_txt = mnu.web_page_txt,
                                                 InquiryOrFull_Access_ind = auth.inquiry_full_acss_ind_cd,
                                                 depnd_txt=mnu.depnd_txt,
                                             });

            return result.ToList();
        }

    }
}
