using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.Logic;

//Texas Tax:Data accesser class
#region Texas Tax DAL
namespace ZurichNA.AIS.DAL.Logic
{
    public class ILRFTaxSetupDA : DataAccessor<INCUR_LOS_REIM_FUND_TAX_SETUP, ILRFTaxSetupBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Texas Tax:Used to retrieve Tax type data from lkup
        /// </summary>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getTaxDescriptionList()
        {
            IList<ILRFTaxSetupBE> result = new List<ILRFTaxSetupBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<ILRFTaxSetupBE> query =
            (from cdd in this.Context.LKUPs
             join ded in this.Context.DEDTBL_TAX_SETUPs on cdd.lkup_id equals ded.tax_typ_id
             where cdd.actv_ind == true && cdd.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "TAX TYPE").First().lkup_typ_id
             && ded.actv_ind==true
             orderby cdd.lkup_txt ascending
             select new ILRFTaxSetupBE()
             {
                 INCURRED_LOSS_REIM_FUND_TAX_ID = cdd.lkup_id,
                 INCURRED_LOSS_REIM_FUND_TAX_TYPE = cdd.lkup_txt,
                                  
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
            result = query.ToList().Distinct(new DistinctTaxType()).ToList();
            return result;
        }


        public class DistinctTaxType : IEqualityComparer<ILRFTaxSetupBE>
        {
            public bool Equals(ILRFTaxSetupBE x, ILRFTaxSetupBE y)
            {
                return x.INCURRED_LOSS_REIM_FUND_TAX_TYPE.Equals(y.INCURRED_LOSS_REIM_FUND_TAX_TYPE);
            }

            public int GetHashCode(ILRFTaxSetupBE obj)
            {
                return obj.INCURRED_LOSS_REIM_FUND_TAX_TYPE.GetHashCode();
            }
        }
        /// <summary>
        /// Texas Tax:To retrieve All Active TaxTypes union Used type(Active/Inactive) in EditMode.
        /// </summary>
        /// <param name="intTaxTypID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getTaxDescriptionListEditData(int intTaxTypID)
        {
            IList<ILRFTaxSetupBE> result = new List<ILRFTaxSetupBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<ILRFTaxSetupBE> query =
            (from cdd in this.Context.LKUPs
             join ded in this.Context.DEDTBL_TAX_SETUPs on cdd.lkup_id equals ded.tax_typ_id
             where cdd.actv_ind == true && cdd.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "TAX TYPE").First().lkup_typ_id
             && ded.actv_ind == true
             orderby cdd.lkup_txt ascending
             select new ILRFTaxSetupBE()
             {
                 INCURRED_LOSS_REIM_FUND_TAX_ID = cdd.lkup_id,
                 INCURRED_LOSS_REIM_FUND_TAX_TYPE = cdd.lkup_txt,

             }).Union(from post in this.Context.LKUPs
                      where post.lkup_id == intTaxTypID
                      select new ILRFTaxSetupBE()
                      {
                          INCURRED_LOSS_REIM_FUND_TAX_ID = post.lkup_id,
                          INCURRED_LOSS_REIM_FUND_TAX_TYPE = post.lkup_txt,
                      }
                      );

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            //result = query.ToList().Distinct(new DistinctTaxType()).ToList();
            result = query.OrderBy(cdd=>cdd.INCURRED_LOSS_REIM_FUND_TAX_TYPE).ToList().Distinct(new DistinctTaxType()).ToList();
            return result;
        }

        /// <summary>
        /// Texas Tax:Used to get the Tax Setup Data based on the parameters
        /// LOB added to the screen
        /// </summary>
        /// <param name="intPREM_ADJ_PGM_ID"></param>
        /// <param name="intCUSTMR_ID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getILRFTaxSetupList(int intPREM_ADJ_PGM_ID,int intCUSTMR_ID)
        {
            IList<ILRFTaxSetupBE> result = new List<ILRFTaxSetupBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<ILRFTaxSetupBE> query =
            (from cdd in this.Context.INCUR_LOS_REIM_FUND_TAX_SETUPs
             join lkup in this.Context.LKUPs on cdd.tax_typ_id equals lkup.lkup_id
             join lkupTaxCovgTyp in this.Context.LKUPs
                on cdd.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
             where cdd.prem_adj_pgm_id == intPREM_ADJ_PGM_ID && cdd.custmr_id==intCUSTMR_ID
                    && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
                    orderby lkup.lkup_txt ascending,cdd.ln_of_bsn_id ascending
                    
             select new ILRFTaxSetupBE()
             {
                 INCURRED_LOSS_REIM_FUND_TAX_ID = cdd.incur_los_reim_fund_tax_id,
                 INCURRED_LOSS_REIM_FUND_TAX_TYPE = lkup.lkup_txt,
                 LN_OF_BSN_ID = lkupTaxCovgTyp.lkup_id,
                 LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt,
                 TAX_TYP_ID=cdd.tax_typ_id,
                 TAX_AMT=cdd.tax_amt,
                 ACTV_IND=cdd.actv_ind,

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        /// <summary>
        /// Texas Tax:Checks if a Tax Type is already exists with given parameters
        /// </summary>
        public int isTaxTypeAlreadyExist(int intILRFTaxSetupID,int intTaxTypeID, int ln_of_bsn_id, int intPrem_adj_pgm_ID)
        {
            //retrun values descriptions
            //0-no duplicate entries
            //1-Active duplicate
            //2-Inactive Duplicate
            if (this.Context == null) this.Initialize();

            int recCount = (from cdd in this.Context.INCUR_LOS_REIM_FUND_TAX_SETUPs
                            where cdd.prem_adj_pgm_id == intPrem_adj_pgm_ID && cdd.tax_typ_id == intTaxTypeID
                            && cdd.ln_of_bsn_id == ln_of_bsn_id  && cdd.incur_los_reim_fund_tax_id != intILRFTaxSetupID
                            select cdd).Count();

            if (recCount == 0) 
                return 0;
            else 
            {
                int intActiveduplicate = (from cdd in this.Context.INCUR_LOS_REIM_FUND_TAX_SETUPs
                                where cdd.prem_adj_pgm_id == intPrem_adj_pgm_ID && cdd.tax_typ_id == intTaxTypeID
                                && cdd.ln_of_bsn_id == ln_of_bsn_id && cdd.incur_los_reim_fund_tax_id != intILRFTaxSetupID && cdd.actv_ind == true
                                select cdd).Count();
                if (intActiveduplicate == 0)
                    return 2;
                else
                    return 1;
            
            }
                
        }

        /// <summary>
        /// Texas Tax:Used to get the Tax Setup Data based on the parameters
        /// LOB added to the screen
        /// </summary>
        /// <param name="intPREM_ADJ_PGM_ID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getILRFTaxSetupListData(int intPREM_ADJ_PGM_ID, int intST_ID)
        {
            IList<ILRFTaxSetupBE> result = new List<ILRFTaxSetupBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<ILRFTaxSetupBE> query =
            (from cdd in this.Context.INCUR_LOS_REIM_FUND_TAX_SETUPs
             join lkup in this.Context.LKUPs on cdd.tax_typ_id equals lkup.lkup_id
             join lkupTaxCovgTyp in this.Context.LKUPs
                on cdd.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
             where cdd.prem_adj_pgm_id == intPREM_ADJ_PGM_ID 
                    && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
                    //&& cdd.actv_ind==true
                    && lkup.attr_1_txt==this.Context.LKUPs.Where(val=>val.lkup_id==intST_ID).FirstOrDefault().attr_1_txt
             select new ILRFTaxSetupBE()
             {
                 INCURRED_LOSS_REIM_FUND_TAX_ID = cdd.incur_los_reim_fund_tax_id,
                 INCURRED_LOSS_REIM_FUND_TAX_TYPE = lkup.lkup_txt,
                 LN_OF_BSN_ID = lkupTaxCovgTyp.lkup_id,
                 LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt,
                 TAX_TYP_ID = cdd.tax_typ_id,
                 TAX_AMT = cdd.tax_amt,
                 ACTV_IND = cdd.actv_ind,

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        /// <summary>
        /// Texas Tax:Checks if a State is tax exempted with given parameters
        /// </summary>
        public int isTaxExemptedState(int intTaxTypeID, int intPrem_adj_pgm_ID)
        {
            //retrun values descriptions
            //0-not Tax Exempted State
            //1-Tax Exempted State
            
            if (this.Context == null) this.Initialize();

            string strStateSymbol = this.Context.LKUPs.Where(cdd => cdd.lkup_id == intTaxTypeID).FirstOrDefault().attr_1_txt;

            int intST_ID = 0;


            IList<LookupBE> result = new List<LookupBE>();

            result =
                (from lkup in this.Context.LKUPs
                 from lkuptype in this.Context.LKUP_TYPs
                 where lkuptype.lkup_typ_nm_txt.Trim().ToUpper() == "STATE"
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
                intST_ID=result[0].LookUpID;

            int recCount = (from cdd in this.Context.TAX_EXMP_SETUPs
                            where cdd.prem_adj_pgm_id == intPrem_adj_pgm_ID && cdd.st_id == intST_ID
                            && cdd.actv_ind == true select cdd).Count();


            if (recCount > 0)
                return 1;
            else
                return 0;

            

        }


        
    }
}
#endregion
