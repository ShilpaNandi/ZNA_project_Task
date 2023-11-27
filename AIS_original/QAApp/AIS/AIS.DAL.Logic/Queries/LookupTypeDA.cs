using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class LookupTypeDA : DataAccessor<LKUP_TYP, LookupTypeBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Retrieves Lookup Data for a LookTypeName
        /// </summary>
        /// <param name="LookupTypeName">Lookup Type Name Value</param>
        /// <returns>List of LookupBE</returns>

        public IList<LookupTypeBE> getLookUpTypeData()
        {
            IList<LookupTypeBE> result = new List<LookupTypeBE>();

            result =
                (from lkuptype in this.Context.LKUP_TYPs
                 //where lkuptype.actv_ind == true
                 orderby lkuptype.lkup_typ_nm_txt ascending
                 select new LookupTypeBE
                 {
                     LOOKUPTYPE_ID = lkuptype.lkup_typ_id,
                     LOOKUPTYPE_NAME = lkuptype.lkup_typ_nm_txt,
                     CREATED_DATE = lkuptype.crte_dt,
                     CREATED_USER_ID = lkuptype.crte_user_id,
                     UPDATED_DATE = lkuptype.updt_dt,
                     UPDATED_USER_ID = lkuptype.updt_user_id,
                     ACTIVE = lkuptype.actv_ind,

                 }
                 ).ToList();

            return result;
        }
        
        /// <summary>
        /// Checks for Duplicate of Lookup Type Name
        /// </summary>
        /// <param name="LookupTypeName">Lookup Type Name </param>
        /// <param name="lookupTypeID">Lookup ID</param>
        /// <returns>True/False(bool) </returns>
        public bool IsExistsLookupTypeName(string LookupTypeName, int lookupTypeID)
        {
            bool Flag = false;
            var LookupType = from cdd in this.Context.LKUP_TYPs
                             where (cdd.lkup_typ_nm_txt == LookupTypeName && cdd.lkup_typ_id != lookupTypeID )
                             select new { cdd.lkup_typ_nm_txt };
            if (LookupType.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }
    }
}