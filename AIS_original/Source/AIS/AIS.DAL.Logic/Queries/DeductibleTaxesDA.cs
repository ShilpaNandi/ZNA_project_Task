using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class DeductibleTaxesDA : DataAccessor<DEDTBL_TAX_SETUP, DeductibleTaxesBE, AISDatabaseLINQDataContext>
    {
        public IList<DeductibleTaxesBE> SelectData()
        {
            //&& lkp.LKUP_TYP.lkup_typ_nm_txt == "TAX TYPE"
            //&& lkp.attr_1_txt == "TEXAS"
            //---join lkpTyp in this.Context.LKUP_TYPs
            //on lkp.lkup_typ_id equals lkpTyp.lkup_typ_id
            IList<DeductibleTaxesBE> result = new List<DeductibleTaxesBE>();

            IQueryable<DeductibleTaxesBE> query =
                from dtaxes in this.Context.DEDTBL_TAX_SETUPs
                join lkp in this.Context.LKUPs
                on dtaxes.st_id equals lkp.lkup_id
                join lkpupTax in this.Context.LKUPs
                on dtaxes.tax_typ_id equals lkpupTax.lkup_id
                join lkupTaxtDed in this.Context.LKUPs
                on dtaxes.dedtbl_tax_cmpnt_id equals lkupTaxtDed.lkup_id
                join lkupTaxCovgTyp in this.Context.LKUPs
                on dtaxes.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
                where lkpupTax.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "TAX TYPE" && cond.actv_ind == true).First().lkup_typ_id
                                 && lkupTaxtDed.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "DEDUCTIBLE TAX COMPONENT" && cond.actv_ind == true).First().lkup_typ_id
                                 && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
                orderby lkp.lkup_txt ascending,dtaxes.pol_eff_dt descending
                select new DeductibleTaxesBE
                {
                    DED_TAXES_SETUP_ID = dtaxes.dedtbl_tax_setup_id,
                    LN_OF_BSN_ID = lkupTaxCovgTyp.lkup_typ_id,
                    LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt,
                    ST_ID = dtaxes.st_id,
                    STATEDESCRIPTION = lkp.lkup_txt,
                    LOOKUPID = lkpupTax.lkup_id,
                    DTAXDESCRIPTION = lkpupTax.lkup_txt,
                    TAX_TYP_ID = dtaxes.tax_typ_id,
                    TAX_RATE = dtaxes.tax_rt,
                    POL_EFF_DT = dtaxes.pol_eff_dt,
                    DED_TAX_COMPONENT_ID = dtaxes.dedtbl_tax_cmpnt_id,
                    DTAXCOMDESCRIPTION = lkupTaxtDed.lkup_txt,
                    TAX_END_DT = dtaxes.tax_end_dt,
                    MAIN_NBR_TXT = dtaxes.main_nbr_txt,
                    SUB_NBR_TXT = dtaxes.sub_nbr_txt,
                    UPDT_USER_ID = dtaxes.updt_user_id,
                    UPDT_DT = dtaxes.updt_dt,
                    CRTE_USER_ID = dtaxes.crte_user_id,
                    CRTE_DT = dtaxes.crte_dt,
                    ACTV_IND = dtaxes.actv_ind
                };
            result = query.ToList();
            return result;
        }
        public IList<LookupBE> GetStates()
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
        public IList<LookupBE> GetDescription(string Attribute1, string lkupTypeName)
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
        public IList<LookupBE> GetDescriptionForEdit(int iDescriptionId,string Attribute1, string lkupTypeName)
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
                      && desc.lkup_id == iDescriptionId
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
        public string GetMainSub(string trns_nm_txt)
        {
            string mainSub = string.Empty;
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<PostingTransactionTypeBE> query =
            (from ptrns in this.Context.POST_TRNS_TYPs
             where ptrns.trns_nm_txt == trns_nm_txt
             && ptrns.actv_ind == true
             select new PostingTransactionTypeBE()
             {
                 MAIN_NBR = ptrns.main_nbr_txt,
                 SUB_NBR = ptrns.sub_nbr_txt,
             });
            if (query.Count() > 0)
                mainSub = query.First().MAIN_NBR + "/" + query.First().SUB_NBR;
            return mainSub;
        }

        public IList<DeductibleTaxesBE> SelectDataOnTransMainSub(string TransNmTxt, string strMain, string strSub)
        {
            //&& lkp.LKUP_TYP.lkup_typ_nm_txt == "TAX TYPE"
            //&& lkp.attr_1_txt == "TEXAS"
            //---join lkpTyp in this.Context.LKUP_TYPs
            //on lkp.lkup_typ_id equals lkpTyp.lkup_typ_id
            //IList<DeductibleTaxesBE> result = new List<DeductibleTaxesBE>();

            IQueryable<DeductibleTaxesBE> query =
                (from dtaxes in this.Context.DEDTBL_TAX_SETUPs
                 join lkp in this.Context.LKUPs
                 on dtaxes.st_id equals lkp.lkup_id
                 join lkpupTax in this.Context.LKUPs
                 on dtaxes.tax_typ_id equals lkpupTax.lkup_id
                 join lkupTaxtDed in this.Context.LKUPs
                 on dtaxes.dedtbl_tax_cmpnt_id equals lkupTaxtDed.lkup_id
                 join lkupTaxCovgTyp in this.Context.LKUPs
                 on dtaxes.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
                 where lkpupTax.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "TAX TYPE" && cond.actv_ind == true).First().lkup_typ_id
                  && lkupTaxtDed.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "DEDUCTIBLE TAX COMPONENT" && cond.actv_ind == true).First().lkup_typ_id
                  && lkpupTax.lkup_txt == TransNmTxt
                  && dtaxes.main_nbr_txt == strMain
                  && dtaxes.sub_nbr_txt == strSub
                  && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
                 select new DeductibleTaxesBE
                 {
                     DED_TAXES_SETUP_ID = dtaxes.dedtbl_tax_setup_id,
                     LN_OF_BSN_ID = lkupTaxCovgTyp.lkup_typ_id,
                     LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt,
                     ST_ID = dtaxes.st_id,
                     STATEDESCRIPTION = lkp.lkup_txt,
                     LOOKUPID = lkpupTax.lkup_id,
                     DTAXDESCRIPTION = lkpupTax.lkup_txt,
                     TAX_TYP_ID = dtaxes.tax_typ_id,
                     TAX_RATE = dtaxes.tax_rt,
                     POL_EFF_DT = dtaxes.pol_eff_dt,
                     DED_TAX_COMPONENT_ID = dtaxes.dedtbl_tax_cmpnt_id,
                     DTAXCOMDESCRIPTION = lkupTaxtDed.lkup_txt,
                     TAX_END_DT = dtaxes.tax_end_dt,
                     MAIN_NBR_TXT = dtaxes.main_nbr_txt,
                     SUB_NBR_TXT = dtaxes.sub_nbr_txt,
                     UPDT_USER_ID = dtaxes.updt_user_id,
                     UPDT_DT = dtaxes.updt_dt,
                     CRTE_USER_ID = dtaxes.crte_user_id,
                     CRTE_DT = dtaxes.crte_dt,
                     ACTV_IND = dtaxes.actv_ind
                 });
            //result = query.ToList();
            return query.ToList();
        }

        public DeductibleTaxesBE SelectDataOnSetupId(int dTaxesSetupId)
        {
            //&& lkp.LKUP_TYP.lkup_typ_nm_txt == "TAX TYPE"
            //&& lkp.attr_1_txt == "TEXAS"
            //---join lkpTyp in this.Context.LKUP_TYPs
            //on lkp.lkup_typ_id equals lkpTyp.lkup_typ_id
            //IList<DeductibleTaxesBE> result = new List<DeductibleTaxesBE>();

            DeductibleTaxesBE query =
                (from dtaxes in this.Context.DEDTBL_TAX_SETUPs
                 join lkp in this.Context.LKUPs
                 on dtaxes.st_id equals lkp.lkup_id
                 join lkpupTax in this.Context.LKUPs
                 on dtaxes.tax_typ_id equals lkpupTax.lkup_id
                 join lkupTaxtDed in this.Context.LKUPs
                 on dtaxes.dedtbl_tax_cmpnt_id equals lkupTaxtDed.lkup_id
                 join lkupTaxCovgTyp in this.Context.LKUPs
                 on dtaxes.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
                 where lkpupTax.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "TAX TYPE" && cond.actv_ind == true).First().lkup_typ_id
                  && lkupTaxtDed.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "DEDUCTIBLE TAX COMPONENT" && cond.actv_ind == true).First().lkup_typ_id
                  && dtaxes.dedtbl_tax_setup_id == dTaxesSetupId
                  && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
                 select new DeductibleTaxesBE
                 {
                     DED_TAXES_SETUP_ID = dtaxes.dedtbl_tax_setup_id,
                     LN_OF_BSN_ID = lkupTaxCovgTyp.lkup_typ_id,
                     LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt,
                     ST_ID = dtaxes.st_id,
                     STATEDESCRIPTION = lkp.lkup_txt,
                     LOOKUPID = lkpupTax.lkup_id,
                     DTAXDESCRIPTION = lkpupTax.lkup_txt,
                     TAX_TYP_ID = dtaxes.tax_typ_id,
                     TAX_RATE = dtaxes.tax_rt,
                     POL_EFF_DT = dtaxes.pol_eff_dt,
                     DED_TAX_COMPONENT_ID = dtaxes.dedtbl_tax_cmpnt_id,
                     DTAXCOMDESCRIPTION = lkupTaxtDed.lkup_txt,
                     TAX_END_DT = dtaxes.tax_end_dt,
                     MAIN_NBR_TXT = dtaxes.main_nbr_txt,
                     SUB_NBR_TXT = dtaxes.sub_nbr_txt,
                     UPDT_USER_ID = dtaxes.updt_user_id,
                     UPDT_DT = dtaxes.updt_dt,
                     CRTE_USER_ID = dtaxes.crte_user_id,
                     CRTE_DT = dtaxes.crte_dt,
                     ACTV_IND = dtaxes.actv_ind
                 }).First();
            //result = query.ToList();
            return query;
        }
        public IList<LookupBE> GetComponentDescription()
        {
            IList<LookupBE> result = new List<LookupBE>();

            IQueryable<LookupBE> query =
                from lkp in this.Context.LKUPs
                where lkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "DEDUCTIBLE TAX COMPONENT").First().lkup_typ_id
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
        public IList<LookupBE> GetComponentDescriptionForEdit(int iComponentId)
        {
            IList<LookupBE> result = new List<LookupBE>();

            IQueryable<LookupBE> query =
                (from lkp in this.Context.LKUPs
                where lkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "DEDUCTIBLE TAX COMPONENT").First().lkup_typ_id
                orderby lkp.lkup_txt
                select new LookupBE
                {
                    LookUpID = lkp.lkup_id,
                    LookUpName = lkp.lkup_txt
                }).Union(from lkpComp in this.Context.LKUPs
                         where lkpComp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "DEDUCTIBLE TAX COMPONENT").First().lkup_typ_id
                         && lkpComp.lkup_id == iComponentId
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
            //lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }
        /// <summary>
        /// Texas Tax:Checks if a Tax Type is already exists with given parameters
        /// </summary>
        public int isDTaxAlreadyExist(int intDTaxSetupID, int ln_of_bsn_id, int intStateID, int intTaxTypeID, int intDTaxCompID, DateTime dtPolicyEffDate)
        {
            //retrun values descriptions
            //0-no duplicate entries
            //1-Active duplicate
            //2-Inactive Duplicate
            if (this.Context == null) this.Initialize();

            int recCount = (from dTaxes in this.Context.DEDTBL_TAX_SETUPs
                            where dTaxes.ln_of_bsn_id==ln_of_bsn_id && dTaxes.st_id == intStateID && dTaxes.tax_typ_id == intTaxTypeID
                            && dTaxes.dedtbl_tax_cmpnt_id == intDTaxCompID && dTaxes.pol_eff_dt == dtPolicyEffDate && dTaxes.dedtbl_tax_setup_id != intDTaxSetupID
                            select dTaxes).Count();

            if (recCount == 0)
                return 0;
            else
            {
                int intActiveduplicate = (from dTaxes in this.Context.DEDTBL_TAX_SETUPs
                                          where dTaxes.ln_of_bsn_id == ln_of_bsn_id && dTaxes.st_id == intStateID && dTaxes.tax_typ_id == intTaxTypeID
                                          && dTaxes.dedtbl_tax_cmpnt_id == intDTaxCompID && dTaxes.pol_eff_dt == dtPolicyEffDate && dTaxes.dedtbl_tax_setup_id != intDTaxSetupID && dTaxes.actv_ind == true
                                          select dTaxes).Count();
                if (intActiveduplicate == 0)
                    return 2;
                else
                    return 1;

            }
        }
    }
}
