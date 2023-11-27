using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class LookupDA : DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Retrieves Lookup Data for a LookTypeName
        /// </summary>
        /// <param name="LookupTypeName">Lookup Type Name Value</param>
        /// <returns>List of LookupBE</returns>
        public IList<LookupBE> getLookUpData(string LookupTypeName)
        {
            IList<LookupBE> result = new List<LookupBE>();

            result =
                (from lkup in this.Context.LKUPs
                 from lkuptype in this.Context.LKUP_TYPs
                 where lkuptype.lkup_typ_nm_txt.Trim() == LookupTypeName.Trim()
                // && lkup.actv_ind == true
                // && lkuptype.actv_ind == true
                 && lkuptype.lkup_typ_id == lkup.lkup_typ_id
                 orderby lkup.lkup_txt
                 select new LookupBE
                 {
                       
                     LookUpName = lkup.lkup_txt,
                     LookUpID = lkup.lkup_id ,
                     Attribute1 = lkup.attr_1_txt,
                     ACTIVE = lkup.actv_ind,
                     Updated_Date=lkup.updt_dt
                 }
                 ).ToList();

            return result;
        }

        public IList<LookupBE> getLookUpData()
        {
            IList<LookupBE> result = new List<LookupBE>();

            result =
                (from lkup in this.Context.LKUPs
                 from lkuptype in this.Context.LKUP_TYPs
                 where lkuptype.lkup_typ_id == lkup.lkup_typ_id
                 //&& lkup.actv_ind == true
                 //&& lkuptype.actv_ind == true
                 select new LookupBE
                 {
                     LookUpTypeID = lkup.lkup_typ_id,
                     LookUpName = lkup.lkup_txt,
                     LookUpID = lkup.lkup_id,
                     LookUpTypeName = lkuptype.lkup_typ_nm_txt,
                     Attribute1 = lkup.attr_1_txt,
                     ACTIVE=lkup.actv_ind,
                     Updated_Date=lkup.updt_dt

                 }
                 ).ToList().OrderBy(rs=>rs.LookUpName).OrderBy(rs=>rs.LookUpTypeName).ToList();

            return result;
        }


        /// <summary>
        /// Checks for Duplicate of Lookup Name based on LookupTypeID
        /// </summary>
        /// <param name="LookupID">Lookup ID </param>
        /// <param name="LookupTypeID">Lookup Type ID </param>
        /// <param name="LookupName">Lookup Name </param>
        /// <returns>True/False(bool) </returns>
        public bool IsExistsLookupName(int LookupID, int lookupTypeID, string LookupName)
        {
            bool Flag = false;
            var Lookup = from cdd in this.Context.LKUPs
                             where (cdd.lkup_typ_id == lookupTypeID && cdd.lkup_txt == LookupName && cdd.lkup_id != LookupID)
                         select new { cdd.lkup_txt };
            if (Lookup.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }
    }
}