using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.Business.Entities;
using System.Data;
using ZurichNA.AIS.ExceptionHandling;

namespace ZurichNA.AIS.DAL.Logic
{
    public class MenuDA:DataAccessor<APLCTN_MENU,ApplMenuBE,AISDatabaseLINQDataContext>
    {

        public MenuDA()
        {
 
        }


        public DataTable FillDataTable(string Rolename)
        {
            if (this.Context == null)
                this.Initialize();

            int securGpID = (from lk in Context.LKUPs
                             join lkTyp in Context.LKUP_TYPs on lk.lkup_typ_id equals lkTyp.lkup_typ_id
                             where lk.lkup_txt == Rolename && lkTyp.lkup_typ_nm_txt == "ROLES"
                                  && lkTyp.actv_ind == true
                             select lk.lkup_id).First();
           

            // IEnumerable<DataTable> 
            var result = (from mnu in Context.APLCTN_MENUs
                          join auth in Context.APLCTN_WEB_PAGE_AUTHs on mnu.aplctn_menu_id equals auth.aplctn_menu_id
                          where auth.secur_gp_id == securGpID
                                 && auth.authd_ind == true 
                                 && ((from sbmnu in Context.APLCTN_MENUs
                                      join sbauth in Context.APLCTN_WEB_PAGE_AUTHs
                                      on sbmnu.aplctn_menu_id equals sbauth.aplctn_menu_id
                                      where sbauth.authd_ind == true && sbauth.secur_gp_id == securGpID
                                        && sbmnu.aplctn_menu_id == mnu.parnt_id
                                      select sbauth.aplctn_menu_id).Count() > 0 || mnu.parnt_id == null)
                                 && mnu.page_catg_cd == "M"
                          select new ApplMenuBE()
                          {
                              menu_id = mnu.aplctn_menu_id,
                              menu_nm_txt = mnu.menu_nm_txt,
                              parnt_id = mnu.parnt_id,
                              web_page_txt = mnu.web_page_txt,
                              menu_tooltip_txt = mnu.menu_tooltip_txt,
                              actv_ind = mnu.actv_ind,
                              depnd_txt = mnu.depnd_txt
                          });

            DataTable dt = ToDataTable(Context, result);
            return dt;
        
        }

        public DataTable ToDataTable(System.Data.Linq.DataContext ctx,object query)
        {
            if (query == null)
            {
               // throw new ArgumentNullException("query");
                throw new RetroBaseException("Null Error - The object does not contain any value");
            }

            IDbCommand cmd = ctx.GetCommand(query as IQueryable);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = (SqlCommand)cmd;
            DataTable dt = new DataTable("sd");

            try
            {
                cmd.Connection.Open();
                //adapter.FillSchema(dt, SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }


    
    
    }

}
