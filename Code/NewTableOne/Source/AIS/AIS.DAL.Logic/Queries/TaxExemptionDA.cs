using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.Logic;

//Texas Tax:Data accesser class for Tax Exemption
#region Texas Tax DAL
namespace ZurichNA.AIS.DAL.Logic
{
    public class TaxExemptionDA : DataAccessor<TAX_EXMP_SETUP, TaxExemptionBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Texas Tax:Used to get the Tax Exempt Setup Data based on the parameters
        /// </summary>
        /// <param name="intPREM_ADJ_PGM_ID"></param>
        /// <param name="intCUSTMR_ID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<TaxExemptionBE> getTaxExemptSetupList(int intPREM_ADJ_PGM_ID, int intCUSTMR_ID)
        {
            IList<TaxExemptionBE> result = new List<TaxExemptionBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<TaxExemptionBE> query =
            (from cdd in this.Context.TAX_EXMP_SETUPs
             join lkup in this.Context.LKUPs on cdd.st_id equals lkup.lkup_id
             where cdd.prem_adj_pgm_id == intPREM_ADJ_PGM_ID && cdd.custmr_id == intCUSTMR_ID
             orderby cdd.st_id ascending
                   //&& cdd.actv_ind==true
             select new TaxExemptionBE()
             {
                 TAX_EXMP_SETUP_ID = cdd.tax_exmp_setup_id,
                 STATE_NAME = lkup.lkup_txt,
                 ACTV_IND = cdd.actv_ind

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }


        /// <summary>
        /// Texas Tax:Checks if a Tax Exempt for this state is already exists with given parameters
        /// </summary>
        public int isTaxExemptAlreadyExist(int intTaxExemptSetupID, int st_id, int intPrem_adj_pgm_ID)
        {
            //retrun values descriptions
            //0-no duplicate entries
            //1-Active duplicate
            //2-Inactive Duplicate
            if (this.Context == null) this.Initialize();

            int recCount = (from cdd in this.Context.TAX_EXMP_SETUPs
                            where cdd.prem_adj_pgm_id == intPrem_adj_pgm_ID && cdd.st_id == st_id
                            && cdd.tax_exmp_setup_id != intTaxExemptSetupID
                            select cdd).Count();

            if (recCount == 0)
                return 0;
            else
            {
                int intActiveduplicate = (from cdd in this.Context.TAX_EXMP_SETUPs
                                          where cdd.prem_adj_pgm_id == intPrem_adj_pgm_ID && cdd.st_id == st_id
                                          && cdd.tax_exmp_setup_id != intTaxExemptSetupID && cdd.actv_ind == true
                                          select cdd).Count();
                if (intActiveduplicate == 0)
                    return 2;
                else
                    return 1;

            }

        }

        public IList<LookupBE> GetStates()
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             join lku in this.Context.LKUPs on lk.attr_1_txt equals lku.attr_1_txt
             where lk.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "STATE"
             && lk.actv_ind == true && lku.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "TAX TYPE" && lku.actv_ind==true
             orderby lk.lkup_txt
             select new LookupBE()
             {
                 LookUpID = lk.lkup_id,
                 LookUpName = lk.lkup_txt,
                 Attribute1 = lk.attr_1_txt
             });
            if (query.Count() > 0)
                result = query.ToList();
            LookupBE lkSelect = new LookupBE();
            lkSelect.Attribute1 = "(Select)";
            lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }
        public IList<LookupBE> GetStatesForEdit(int iLkupId)
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             where lk.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "STATE"
             && lk.actv_ind == true
             orderby lk.lkup_txt
             select new LookupBE()
             {
                 LookUpID = lk.lkup_id,
                 LookUpName = lk.lkup_txt,
                 Attribute1 = lk.attr_1_txt
             }).Union(from stat in this.Context.LKUPs
                      where stat.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "STATE"
                      && stat.lkup_id == iLkupId
                      orderby stat.lkup_txt
                      select new LookupBE()
                      {
                          LookUpID = stat.lkup_id,
                          LookUpName = stat.lkup_txt,
                          Attribute1 = stat.attr_1_txt
                      });
            if (query.Count() > 0)
                result = query.ToList();
            LookupBE lkSelect = new LookupBE();
            lkSelect.Attribute1 = "(Select)";
            lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }

        public int getTaxTypID(int st_id)
        {
            if (this.Context == null) this.Initialize();
            int intTaxTypID =0;
            
            string strStateSymbol = this.Context.LKUPs.Where(cdd => cdd.lkup_id == st_id).FirstOrDefault().attr_1_txt;

            IList<LookupBE> result = new List<LookupBE>();

            result =
                (from lkup in this.Context.LKUPs
                 from lkuptype in this.Context.LKUP_TYPs
                 where lkuptype.lkup_typ_nm_txt.Trim().ToUpper() == "TAX TYPE"
                 && lkuptype.lkup_typ_id == lkup.lkup_typ_id
                 && lkup.attr_1_txt.Trim() == strStateSymbol.Trim()
                 orderby lkup.lkup_txt
                 select new LookupBE
                 {

                     LookUpName = lkup.lkup_txt,
                     LookUpID = lkup.lkup_id,
                     Attribute1 = lkup.attr_1_txt,
                     ACTIVE = lkup.actv_ind,
                     Updated_Date = lkup.updt_dt
                 }
                 ).ToList();

            if (result.Count > 0)
                return result[0].LookUpID;
            else
                return intTaxTypID;
        }
        
    }
}
#endregion