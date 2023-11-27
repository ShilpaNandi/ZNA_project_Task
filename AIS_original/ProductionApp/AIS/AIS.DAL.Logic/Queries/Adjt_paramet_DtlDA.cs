using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class Adjt_paramet_DtlDA : DataAccessor<PREM_ADJ_PGM_DTL, AdjustmentParameterDetailBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Get all state from look up table and fitler out only those states that 
        /// are remove those States from the list that are not not available in te listview
        /// </summary>
        /// <returns></returns>
        public IList<AdjustmentParameterDetailBE> getAdjustprmState(int ProgramPrdID, int AdjParameterSetupID, int AccntID)
        {

            IQueryable<AdjustmentParameterDetailBE> Stateresult = this.BuildQueryState(ProgramPrdID, AdjParameterSetupID, AccntID);
            return (Stateresult.ToList());

        }

        private IQueryable<AdjustmentParameterDetailBE> BuildQueryState(int PgrPrdID, int AdjPrmSetupID, int AcntID)
        { 
            /// Generate query to retrieve all states that
            /// have not present in the existing listdetails

            
            int[] Statequery = (from lk in this.Context.LKUPs
                                join adjparmdtls in this.Context.PREM_ADJ_PGM_DTLs
                                                                 on lk.lkup_id equals Convert.ToInt32(adjparmdtls.st_id)
                                                                 where  
                                                                 lk.lkup_typ_id == 1 &&
                                                                 adjparmdtls.prem_adj_pgm_setup_id == AdjPrmSetupID 
                                                                 && adjparmdtls.prem_adj_pgm_id == PgrPrdID && 
                                                                 adjparmdtls.custmr_id == AcntID  
                                                                 select
 
                                                                   lk.lkup_id 
                                                                     
                                                                 ).ToArray();

            IQueryable<AdjustmentParameterDetailBE> lookupstates = from lookup in this.Context.LKUPs
                                                                   where lookup.lkup_typ_id == 1 &&
                                                                   !Statequery.Contains(lookup.lkup_id) 
                                                                   select new AdjustmentParameterDetailBE
                                                                   {
                                                                       st_id = lookup.lkup_id
                                                                   };

            return lookupstates;
        }
        
        /// <summary>
        /// Retrieves the Adjustment Parametersetup details 
        /// </summary>
        /// <returns>List of Adj_Paramet_DltBE</returns>
        public IList<AdjustmentParameterDetailBE> getLBAAdjParamtrDtls()
        {
            IQueryable<AdjustmentParameterDetailBE> query = this.BuildQuery();
            return query.ToList();
        }

        /// <summary>
        /// Retrieves the Adjustment Parametersetup  details 
        /// for a particular Adjustment Parameter Setup ID
        /// </summary>
        /// <param name="AdjParameterSetupID"></param>
        /// <returns>List of Adjt_Paramet_DtlBE</returns>
        public List<AdjustmentParameterDetailBE> getLBAAdjParamtrDtls(int ProgramPrdID, int AdjParameterSetupID, int AccntID)
        {
            List<AdjustmentParameterDetailBE> resultAdjDtl = new List<AdjustmentParameterDetailBE>();  
            IQueryable<AdjustmentParameterDetailBE> query = this.BuildQuery();
            if (ProgramPrdID > 0 || AdjParameterSetupID > 0 || AccntID > 0)
            {
                query = query.Where(LBAAdjPrmtrSetupDetails => LBAAdjPrmtrSetupDetails.PrgmPerodID == ProgramPrdID);
                query = query.Where(LBAAdjPrmtrSetupDetails => LBAAdjPrmtrSetupDetails.adj_paramet_id == AdjParameterSetupID);
                query = query.Where(LBAAdjPrmtrSetupDetails => LBAAdjPrmtrSetupDetails.AccountID == AccntID);
                resultAdjDtl = query.ToList();
            }
            else 
            {
                resultAdjDtl = null;
            }

            //if (AdjParameterSetupID > 0)
            //{
            //    query = query.Where(LBAAdjPrmtrSetupDetails => LBAAdjPrmtrSetupDetails.adj_paramet_id == AdjParameterSetupID);
            //}

            //if (AccntID > 0)
            //{
            //    query = query.Where(LBAAdjPrmtrSetupDetails => LBAAdjPrmtrSetupDetails.AccountID == AccntID);

            //}
            return resultAdjDtl;
        }
        public bool deleteAdjParamtrDtls(int ProgramPrdID, int AdjParameterSetupID, int AccntID)
        {
            bool returnvalue;
            if (AccntID > 0 && AdjParameterSetupID > 0 && ProgramPrdID > 0)
            {
                var dtl = from dtls in this.Context.PREM_ADJ_PGM_DTLs
                           where dtls.custmr_id == AccntID && dtls.prem_adj_pgm_setup_id == AdjParameterSetupID && dtls.prem_adj_pgm_id == ProgramPrdID
                           select dtls;

                this.Context.PREM_ADJ_PGM_DTLs.DeleteAllOnSubmit(dtl);

                try
                {
                    this.Context.SubmitChanges();
                }
                catch
                {
                }
                returnvalue = true;
            }
            else 
            {
                returnvalue = false;
            }

            return returnvalue; 
        }

        private IQueryable<AdjustmentParameterDetailBE> BuildQuery()
        {
            IQueryable<AdjustmentParameterDetailBE> query = from LBAAdjPrmtrSetupDetails in this.Context.PREM_ADJ_PGM_DTLs 
                                                            orderby LBAAdjPrmtrSetupDetails.ln_of_bsn_id, LBAAdjPrmtrSetupDetails.st_id,
                                                             LBAAdjPrmtrSetupDetails.clm_hndl_fee_los_typ_id 
                                                           select new AdjustmentParameterDetailBE
                                                      {
                                                          prem_adj_pgm_dtl_id = LBAAdjPrmtrSetupDetails.prem_adj_pgm_dtl_id,
                                                          adj_paramet_id = LBAAdjPrmtrSetupDetails.prem_adj_pgm_setup_id,
                                                          PrgmPerodID = LBAAdjPrmtrSetupDetails.prem_adj_pgm_id,
                                                          AccountID = LBAAdjPrmtrSetupDetails.custmr_id,
                                                          st_id =LBAAdjPrmtrSetupDetails.st_id, 
                                                          fnl_overrid_amt = LBAAdjPrmtrSetupDetails.fnl_overrid_amt, 
                                                          adj_fctr_rt = LBAAdjPrmtrSetupDetails.adj_fctr_rt,
                                                          cmmnt_txt = LBAAdjPrmtrSetupDetails.cmmnt_txt, 
                                                          ln_of_bsn_id = LBAAdjPrmtrSetupDetails.ln_of_bsn_id,
                                                          PremAssementAmt = LBAAdjPrmtrSetupDetails.prem_asses_rt,
                                                          Clm_hndlfee_clmrate = LBAAdjPrmtrSetupDetails.clm_hndl_fee_clm_rt_nbr,
                                                          clm_hndl_fee_los_typ_id = LBAAdjPrmtrSetupDetails.clm_hndl_fee_los_typ_id,
                                                          CHF_CLMT_NUMBER = LBAAdjPrmtrSetupDetails.clm_hndl_fee_clmt_nbr ,
                                                          act_ind = LBAAdjPrmtrSetupDetails.actv_ind,
                                                          CRTE_DATE = LBAAdjPrmtrSetupDetails.crte_dt,
                                                          CRTE_USER_ID = LBAAdjPrmtrSetupDetails.crte_user_id,
                                                          UPDTE_DATE = LBAAdjPrmtrSetupDetails.updt_dt,
                                                          UPDTE_USER_ID = LBAAdjPrmtrSetupDetails.updt_user_id 
                                                          
                                                      };
            return query;
        }
       
    }

    
}
