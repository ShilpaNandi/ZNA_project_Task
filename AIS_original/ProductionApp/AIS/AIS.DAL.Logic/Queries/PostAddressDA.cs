using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{
    public class PostAddressDA : DataAccessor<POST_ADDR, PostalAddressBE, AISDatabaseLINQDataContext>
    {

        public PostalAddressBE getPersonsList(int PersonID)
        {
            PostalAddressBE result = new PostalAddressBE();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PostalAddressBE> query =
            (from cdd in this.Context.POST_ADDRs
             select new PostalAddressBE()
             {
                 POSTALADDRESSID = cdd.post_addr_id,
                 PERSON_ID = cdd.pers_id,
                 ADDRESS1 = cdd.addr_ln_1_txt,
                 ADDRESS2 = cdd.addr_ln_2_txt,
                 CITY = cdd.city_txt,
                 STATE_ID = cdd.st_id,
                 ZIP_CODE = cdd.post_cd_txt,
                 CREATEDDATE=cdd.crte_dt,
                 UPDATEDDATE=cdd.updt_dt,
                 UPDATEDUSER=cdd.updt_user_id,
                 CREATEDUSER=cdd.crte_user_id,
             });

            /// Get a specific account record
            if (PersonID > 0)
            {
                query = query.Where(cdd => cdd.PERSON_ID == PersonID);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList()[0];
            return result;
        }
    }
}
