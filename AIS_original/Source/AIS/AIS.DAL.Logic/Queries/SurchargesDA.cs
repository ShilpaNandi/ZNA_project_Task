/*-----	Page:	Surcharges Setup data Accessor Layer
-----
-----	Created:		CSC (Zakir Hussain)

-----
-----	Description:	Code to Save or retreive data from the SURCHRG_ASSES_SETUP table.
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 6/18/2010 (AS part of Surcharges Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class SurchargesDA : DataAccessor<SURCHRG_ASSES_SETUP, SurchargesBE, AISDatabaseLINQDataContext>
    {
        
        public IList<SurchargesBE> SelectData()
        {

            IList<SurchargesBE> result = new List<SurchargesBE>();

            IQueryable<SurchargesBE> query =
                from staxes in this.Context.SURCHRG_ASSES_SETUPs
                join lkp in this.Context.LKUPs
                on staxes.st_id equals lkp.lkup_id
                join lkpupTax in this.Context.LKUPs
                on staxes.surchrg_typ_id equals lkpupTax.lkup_id
                join lkupCodeSur in this.Context.LKUPs
                on staxes.surchrg_cd_id equals lkupCodeSur.lkup_id
                join lkupTaxCovgTyp in this.Context.LKUPs
                on staxes.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
                join lkupSurFact in this.Context.LKUPs
                on staxes.surchrg_fctr_id equals lkupSurFact.lkup_id
                where lkpupTax.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "SURCHARGES AND ASSESSMENTS" && cond.actv_ind == true).First().lkup_typ_id
                                 && lkupCodeSur.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "SURCHARGE ASSESSMENT CODE" && cond.actv_ind == true).First().lkup_typ_id
                                 && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "LOB" && cond.actv_ind == true).First().lkup_typ_id
                                 && lkupSurFact.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "SURCHARGE DATE INDICATOR" && cond.actv_ind == true).First().lkup_typ_id
                                 && lkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "STATE" && cond.actv_ind == true).First().lkup_typ_id
                orderby lkp.lkup_txt ascending,lkupCodeSur.lkup_txt ascending, staxes.surchrg_eff_dt descending
                select new SurchargesBE
                {
                    SURCHARGE_ASSESS_SETUP_ID = staxes.surchrg_asses_setup_id,
                    LN_OF_BSN_ID = staxes.ln_of_bsn_id,
                    LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt,
                    ST_ID = staxes.st_id,
                    STATEDESCRIPTION = lkp.lkup_txt,
                    LOOKUPID = lkpupTax.lkup_id,
                    SURCHARGE_TYPE_ID = staxes.surchrg_typ_id,
                    SURCHARGE_DESCRIPTION = lkpupTax.lkup_txt,
                    SURCHARGE_EFF_DT = staxes.surchrg_eff_dt,
                    SURCHARGE_RATE = staxes.surchrg_rt,
                    SURCHARGE_CODE_ID = staxes.surchrg_cd_id,
                    SURCHARGE_FACTOR_ID = staxes.surchrg_fctr_id,
                    SURCHARGE_CODE = lkupCodeSur.lkup_txt,
                    STATE_SURCHARGE_CODE=lkp.attr_1_txt.Trim()+'-'+lkupCodeSur.lkup_txt,
                    SURCHARGE_FACTOR = lkupSurFact.lkup_txt,
                    UPDT_USER_ID = staxes.updt_user_id,
                    UPDT_DT = staxes.updt_dt,
                    CRTE_USER_ID = staxes.crte_user_id,
                    CRTE_DT = staxes.crte_dt,
                    ACTV_IND = staxes.actv_ind
                };
            result = query.ToList();
            return result;
        }
        /// <summary>
        ///GetStates() method is used to pull the states information from the Lookup table.
        /// </summary>
        public IList<LookupBE> GetStates()
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             where lk.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "STATE"
             && lk.actv_ind == true
             && lk.lkup_txt != "All Other"
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
        /// <summary>
        ///GetStatesForEdit() method is used to pull the states information from the Lookup table when trying to edit an already existing record.
        /// </summary>
        public IList<LookupBE> GetStatesForEdit(int iLkupId)
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             where lk.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "STATE"
             && lk.actv_ind == true
             && lk.lkup_txt != "All Other"
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
        /// <summary>
        ///FactorDescription() method is used to pull the Factors from the Lookup table.
        /// </summary>
        public IList<LookupBE> FactorDescription()
        {
            IList<LookupBE> result = new List<LookupBE>();

            IQueryable<LookupBE> query =
                from lkp in this.Context.LKUPs
                where lkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGE DATE INDICATOR").First().lkup_typ_id
                && lkp.actv_ind == true
                orderby lkp.lkup_txt
                select new LookupBE
                {
                    LookUpID = lkp.lkup_id,
                    LookUpName = lkp.lkup_txt
                };
            if (query.Count() > 0)
                result = query.ToList();
            LookupBE lkSelect = new LookupBE();
            lkSelect.LookUpName = "(Select)";
            //lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }
        /// <summary>
        /// FactorDescriptionForEdit() method is used to pull the Factors from the Lookup table when trying to edit an already existing record.
        /// </summary>
        public IList<LookupBE> FactorDescriptionForEdit(int iFactorId)
        {
            IList<LookupBE> result = new List<LookupBE>();

            IQueryable<LookupBE> query =
                (from lkp in this.Context.LKUPs
                 where lkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGE DATE INDICATOR").First().lkup_typ_id
                 && lkp.actv_ind == true
                 orderby lkp.lkup_txt
                 select new LookupBE
                 {
                     LookUpID = lkp.lkup_id,
                     LookUpName = lkp.lkup_txt
                 }).Union(from lkpComp in this.Context.LKUPs
                          where lkpComp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGE DATE INDICATOR").First().lkup_typ_id
                          && lkpComp.lkup_id == iFactorId
                          orderby lkpComp.lkup_txt
                          select new LookupBE()
                          {
                              LookUpID = lkpComp.lkup_id,
                              LookUpName = lkpComp.lkup_txt,
                          });
            if (query.Count() > 0)
                result = query.OrderBy(lkp => lkp.LookUpName).ToList();
            LookupBE lkSelect = new LookupBE();
            lkSelect.LookUpName = "(Select)";
            
            result.Insert(0, lkSelect);
            return result;
        }

        public SurchargesBE SelectDataOnSetupId(int sTaxesSetupId)
        {
            SurchargesBE query =
                (from staxes in this.Context.SURCHRG_ASSES_SETUPs
                 join lkp in this.Context.LKUPs
                 on staxes.st_id equals lkp.lkup_id
                 join lkpupTax in this.Context.LKUPs
                 on staxes.surchrg_typ_id equals lkpupTax.lkup_id
                 join lkupCodeSur in this.Context.LKUPs
                 on staxes.surchrg_cd_id equals lkupCodeSur.lkup_id
                 join lkupTaxCovgTyp in this.Context.LKUPs
                 on staxes.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
                 join lkupSurFact in this.Context.LKUPs
                 on staxes.surchrg_fctr_id equals lkupSurFact.lkup_id
                 where lkpupTax.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGES AND ASSESSMENTS" && cond.actv_ind == true).First().lkup_typ_id
                                  && lkupCodeSur.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGE ASSESSMENT CODE" && cond.actv_ind == true).First().lkup_typ_id
                                  && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
                                  && lkupSurFact.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGE DATE INDICATOR" && cond.actv_ind == true).First().lkup_typ_id
                                  && staxes.surchrg_asses_setup_id==sTaxesSetupId
                 orderby lkp.lkup_txt ascending, staxes.surchrg_eff_dt descending
                 select new SurchargesBE
                 {
                     SURCHARGE_ASSESS_SETUP_ID = staxes.surchrg_asses_setup_id,
                     LN_OF_BSN_ID = staxes.ln_of_bsn_id,
                     LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt,
                     ST_ID = staxes.st_id,
                     STATEDESCRIPTION = lkp.lkup_txt,
                     LOOKUPID = lkpupTax.lkup_id,
                     SURCHARGE_TYPE_ID = staxes.surchrg_typ_id,
                     SURCHARGE_DESCRIPTION = lkpupTax.lkup_txt,
                     SURCHARGE_EFF_DT = staxes.surchrg_eff_dt,
                     SURCHARGE_RATE = staxes.surchrg_rt,
                     SURCHARGE_CODE_ID = staxes.surchrg_cd_id,
                     SURCHARGE_FACTOR_ID = staxes.surchrg_fctr_id,
                     SURCHARGE_CODE = lkupCodeSur.lkup_txt,
                     SURCHARGE_FACTOR = lkupSurFact.lkup_txt,
                     UPDT_USER_ID = staxes.updt_user_id,
                     UPDT_DT = staxes.updt_dt,
                     CRTE_USER_ID = staxes.crte_user_id,
                     CRTE_DT = staxes.crte_dt,
                     ACTV_IND = staxes.actv_ind
                 }).First();

           
            return query;
        }
        /// <summary>
        /// Surcharges:Checks if a Surcharges Setup is already exists with given parameters
        /// </summary>
        public int isSurAlreadyExist(int intSurSetupID, int ln_of_bsn_id, int intStateID, int intSurCode, DateTime dtSurEffDate)
        {
            //retrun values descriptions
            //0-no duplicate entries
            //1-Active duplicate
            //2-Inactive Duplicate
            if (this.Context == null) this.Initialize();

            int recCount = (from sTaxes in this.Context.SURCHRG_ASSES_SETUPs
                            where sTaxes.ln_of_bsn_id == ln_of_bsn_id && sTaxes.st_id == intStateID 
                            && sTaxes.surchrg_cd_id == intSurCode && sTaxes.surchrg_eff_dt == dtSurEffDate && sTaxes.surchrg_asses_setup_id != intSurSetupID
                            select sTaxes).Count();

            if (recCount == 0)
                return 0;
            else
            {
                int intActiveduplicate = (from sTaxes in this.Context.SURCHRG_ASSES_SETUPs
                                          where sTaxes.ln_of_bsn_id == ln_of_bsn_id && sTaxes.st_id == intStateID 
                                          && sTaxes.surchrg_cd_id == intSurCode && sTaxes.surchrg_eff_dt == dtSurEffDate && sTaxes.surchrg_asses_setup_id != intSurSetupID && sTaxes.actv_ind == true
                                          select sTaxes).Count();
                if (intActiveduplicate == 0)
                    return 2;
                else
                    return 1;

            }
        }
        /// <summary>
        /// Surcharges: Method is used to pull the Codes from the Lookup table depending upon the selected state.
        /// </summary>
        public IList<LookupBE> GetCode(string Attribute1, string lkupTypeName)
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             where lk.LKUP_TYP.lkup_typ_nm_txt == lkupTypeName
             && lk.attr_1_txt == Attribute1
             && lk.actv_ind == true
             orderby lk.lkup_txt
             select new LookupBE()
             {
                 LookUpID = lk.lkup_id,
                 LookUpTypeName = lk.lkup_txt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            LookupBE lkSelect = new LookupBE();
            lkSelect.LookUpTypeName = "(Select)";
            lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }
        /// <summary>
        /// Surcharges: Method is used to pull the Codes from the Lookup table for a particular state when trying to edit an already existing record.
        /// </summary>
        public IList<LookupBE> GetCodeForEdit(int iCodeId, string Attribute1, string lkupTypeName)
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             where lk.LKUP_TYP.lkup_typ_nm_txt == lkupTypeName
             && lk.attr_1_txt == Attribute1
             && lk.actv_ind == true
             orderby lk.lkup_txt
             select new LookupBE()
             {
                 LookUpID = lk.lkup_id,
                 LookUpTypeName = lk.lkup_txt,
             }).Union(from desc in this.Context.LKUPs
                      where desc.LKUP_TYP.lkup_typ_nm_txt == lkupTypeName
                      && desc.attr_1_txt == Attribute1
                      && desc.lkup_id == iCodeId
                      orderby desc.lkup_txt
                      select new LookupBE()
                      {
                          LookUpID = desc.lkup_id,
                          LookUpTypeName = desc.lkup_txt,
                      });
            if (query.Count() > 0)
                result = query.ToList();
            LookupBE lkSelect = new LookupBE();
            lkSelect.LookUpTypeName = "(Select)";
            lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }
        /// <summary>
        /// Surcharges: Method is used to pull the Description for a particular State.
        /// </summary>
        public LookupBE GetDescription(string lkupTypeName)
        {
            LookupBE SurchargeDescriptionBE=new LookupBE();
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             join lkp in this.Context.LKUPs
             on lk.lkup_txt equals lkp.attr_1_txt
             where lk.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGE ASSESSMENT CODE").First().lkup_typ_id
             && lkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "SURCHARGES AND ASSESSMENTS").First().lkup_typ_id
             && lkp.attr_1_txt == lkupTypeName
             && lkp.actv_ind == true
             select new LookupBE()
             {
                 LookUpID = lkp.lkup_id,
                 LookUpName = lkp.lkup_txt,
             });
            if (query.Count() > 0)
                SurchargeDescriptionBE = query.ToList()[0];
            return SurchargeDescriptionBE;
        }


      

      
    }
}

