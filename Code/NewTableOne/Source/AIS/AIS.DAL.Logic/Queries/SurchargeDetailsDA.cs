/*
             File Name          : SurchargeDetailsDA.cs
 *           Description        : code for retriving data and updating data for surcharge assesment
 *           Author             : Phani Neralla
 *           Team Name          : FinSol/AIS
 *           Creation Date      : 18-Jun-2010
 *           Last Modified By   : 
 *           Last Modified Date :
*/
#region Surcharge Details Data Accesor
#region namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
//AIS specfic custome namespaces
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
#endregion

#region code
namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// class to access the PREM_ADJ_SURCHRG_DTL table, to modify and update the data.
    /// </summary>
    public class SurchargeDetailsDA : DataAccessor<PREM_ADJ_SURCHRG_DTL_AMT, SurchargeDetailAmountBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SurchargeDetailsDA()
        {

        }

        /// <summary>
        /// This method returns the state level details in Surcharge Assesment Webpage
        /// </summary>
        /// <param name="intPremAdjID">prem adjustment id</param>
        /// <param name="intPremAdjPgmID">prem adj program id</param>
        /// <returns>surcharge list at state level</returns>
        public IList<SurchargeDetailAmountBE> GetSurchargeDetailsList(int intPremAdjID_in, int intPremAdjPgmID_in)
        {

            IList<SurchargeDetailAmountBE> result = new List<SurchargeDetailAmountBE>();
            IList<string> descResult = new List<string>();
            IQueryable<SurchargeDetailAmountBE> query =
                 from surchrge in
                     (from premsurchdetailamt in this.Context.PREM_ADJ_SURCHRG_DTL_AMTs
                      join Statelkp in this.Context.LKUPs on
                      premsurchdetailamt.st_id equals Statelkp.lkup_id
                      join lkpupTax in this.Context.LKUPs
                      on premsurchdetailamt.surchrg_typ_id equals lkpupTax.lkup_id
                      join lkupCodeSur in this.Context.LKUPs
                      on premsurchdetailamt.surchrg_cd_id equals lkupCodeSur.lkup_id
                      where lkpupTax.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "SURCHARGES AND ASSESSMENTS" && cond.actv_ind == true).First().lkup_typ_id
                       && lkupCodeSur.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "SURCHARGE ASSESSMENT CODE" && cond.actv_ind == true).First().lkup_typ_id
                       && Statelkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt.Trim().ToUpper() == "STATE" && cond.actv_ind == true).First().lkup_typ_id
                       && premsurchdetailamt.prem_adj_id == intPremAdjID_in
                       && premsurchdetailamt.prem_adj_pgm_id == intPremAdjPgmID_in
                      orderby Statelkp.attr_1_txt, premsurchdetailamt.surchrg_cd_id ascending
                      select new SurchargeDetailAmountBE
                      {
                          other_surchrg_amt = premsurchdetailamt.other_surchrg_amt,
                          st_id = premsurchdetailamt.st_id,
                          surchrg_cd_id = premsurchdetailamt.surchrg_cd_id,
                          surchrg_type_id = premsurchdetailamt.surchrg_typ_id,
                          Surcharge_Code = Statelkp.attr_1_txt.Trim() + "-" + lkupCodeSur.lkup_txt,
                          Surcharge_Desc = lkpupTax.lkup_txt
                      })
                 group surchrge by
                 new { surchrge.st_id, surchrge.surchrg_cd_id, surchrge.surchrg_type_id, surchrge.Surcharge_Code, surchrge.Surcharge_Desc } into g
                 orderby g.Key.Surcharge_Code
                 select new SurchargeDetailAmountBE
                 {
                     st_id = g.Key.st_id,
                     surchrg_cd_id = g.Key.surchrg_cd_id,
                     surchrg_type_id = g.Key.surchrg_type_id,
                     Surcharge_Code = g.Key.Surcharge_Code,
                     Surcharge_Desc = g.Key.Surcharge_Desc,
                     other_surchrg_amt = g.Sum(sur => sur.other_surchrg_amt)
                 };

            result = query.ToList();

            return result;
        }

        /// <summary>
        /// This method returns the policy level details in Surcharge Assesment Webpage
        /// </summary>
        /// <param name="intPremAdjID"></param>
        /// <param name="intPremAdjPgmID"></param>
        /// <returns></returns>
        public IList<SurchargeDetailAmountBE> GetSurchargePolLvlDetailsList(int intPremAdjID_in, int intPremAdjPgmID_in
                                                                            , int surCode_in, int stateID_in)
        {

            IList<SurchargeDetailAmountBE> result = new List<SurchargeDetailAmountBE>();

            IQueryable<SurchargeDetailAmountBE> query =
                from premsurchdetailamt in this.Context.PREM_ADJ_SURCHRG_DTL_AMTs
                join premsurchdetail in this.Context.PREM_ADJ_SURCHRG_DTLs
                on premsurchdetailamt.coml_agmt_id equals premsurchdetail.coml_agmt_id
                join comlagmt in this.Context.COML_AGMTs on
                   premsurchdetailamt.coml_agmt_id equals comlagmt.coml_agmt_id
                where premsurchdetailamt.prem_adj_id == intPremAdjID_in &&
                   premsurchdetail.prem_adj_id == intPremAdjID_in &&
                   premsurchdetailamt.prem_adj_pgm_id == intPremAdjPgmID_in &&
                   premsurchdetail.surchrg_cd_id == surCode_in &&
                   premsurchdetailamt.surchrg_cd_id == surCode_in &&
                   premsurchdetail.st_id == stateID_in


                select new SurchargeDetailAmountBE
                {

                    prem_adj_surchrg_dtl_id = premsurchdetailamt.prem_adj_surchrg_dtl_amt_id,
                    PrgmPerodID = premsurchdetailamt.prem_adj_pgm_id,
                    prem_adj_id = premsurchdetailamt.prem_adj_id,
                    other_surchrg_amt = premsurchdetailamt.other_surchrg_amt,
                    surchrg_rt = premsurchdetail.surchrg_rt,
                    st_id = premsurchdetailamt.st_id,
                    surchrg_cd_id = premsurchdetailamt.surchrg_cd_id,
                    surchrg_type_id = premsurchdetailamt.surchrg_typ_id,
                    UPDTE_DATE = premsurchdetailamt.updt_dt,
                    pol_name = comlagmt.pol_sym_txt + "  " + comlagmt.pol_nbr_txt + " " + comlagmt.pol_modulus_txt,
                    tot_surchrg_asses_base = premsurchdetail.tot_surchrg_asses_base,
                    tot_addn_rtn = premsurchdetail.tot_addn_rtn,

                };

            result = query.ToList();
            return result;
        }


        public void CalcSurchargeReview(int premAdjPrdId_in, int premAdjId_in, int custId_in, int premAdjPrgId_in,
            decimal addnRtn932_in, int stateId_in, int lobId_in, int comagmtid_in, int surCodeid_in, int surTypeId_in, int createUserId_in, bool debug_in)
        {
            string ErroMsg;
            ErroMsg = string.Empty;

            if (this.Context == null)
                this.Initialize();
            this.Context.ModAISCalcSurchargeReview(premAdjPrdId_in, premAdjId_in, custId_in, premAdjPrgId_in, addnRtn932_in,
                stateId_in, lobId_in, comagmtid_in, surCodeid_in, surTypeId_in, createUserId_in, ref ErroMsg, debug_in);
        }
    }
}
#endregion
#endregion
