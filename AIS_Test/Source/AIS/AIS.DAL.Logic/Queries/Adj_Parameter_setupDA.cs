using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class Adj_Parameter_setupDA : DataAccessor<PREM_ADJ_PGM_SETUP, AdjustmentParameterSetupBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Retrieves the Adjustment Parameter setup Information
        /// for a particular ProgramPeriodID
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <returns>List of Adj_Parameter_setupBE</returns>
        public IList<AdjustmentParameterSetupBE> getAdjParamtr(int ProgramPeriodID, int CstmrID)
        {
            IList<AdjustmentParameterSetupBE> resultl = new List<AdjustmentParameterSetupBE>();

            IQueryable<AdjustmentParameterSetupBE> query = this.BuildQuery();

            if (ProgramPeriodID > 0 || CstmrID > 0)
            {
                query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.prem_adj_pgm_id == ProgramPeriodID);
                query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.Cstmr_Id == CstmrID);
                if (query.Count() > 0)
                    resultl = query.ToList();

                foreach (AdjustmentParameterSetupBE setupBE in resultl)
                {
                    setupBE.AdjParametPolBEs = (new Adj_paramet_PolDA()).getAdjParamtrPol(setupBE.adj_paramet_setup_id);
                }

            }
            else
            {
                resultl = null;
            }

            //if (CstmrID > 0)
            //{
            //    query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.Cstmr_Id == CstmrID);
            //}
            //if (query.Count() > 0)
            //resultl = query.ToList();

            //foreach (AdjustmentParameterSetupBE setupBE in resultl)
            //{
            //    setupBE.AdjParametPolBEs = (new Adj_paramet_PolDA()).getAdjParamtrPol(setupBE.adj_paramet_setup_id);
            //}

            return resultl;
            //return (query.ToList());
        }
        /// <summary>
        /// retrives Adjparameter setup details for ILRF Setup
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="cstmrID"></param>
        /// <param name="adjParamTypeID"></param>
        /// <returns>List of AdjParameters</returns>
        public AdjustmentParameterSetupBE getAdjParamsforILRF(int programPeriodID, int cstmrID, int adjParamTypeID)
        {
            AdjustmentParameterSetupBE result = new AdjustmentParameterSetupBE();

            IQueryable<AdjustmentParameterSetupBE> query = (from aps in this.Context.PREM_ADJ_PGM_SETUPs
                                                            where aps.prem_adj_pgm_id == programPeriodID && aps.custmr_id == cstmrID && aps.adj_parmet_typ_id == adjParamTypeID
                                                            select new AdjustmentParameterSetupBE
                                                            {
                                                                prem_adj_pgm_id = aps.prem_adj_pgm_id,
                                                                adj_paramet_setup_id = aps.prem_adj_pgm_setup_id,
                                                                lba_Adjustment_typ = aps.los_base_asses_adj_typ_id,
                                                                incur_but_not_rptd_los_dev_fctr_id = aps.incur_but_not_rptd_los_dev_fctr_id,
                                                                incur_los_reim_fund_initl_fund_amt = aps.incur_los_reim_fund_initl_fund_amt,
                                                                incur_los_reim_fund_unlim_agmt_lim_ind = aps.incur_los_reim_fund_unlim_agmt_lim_ind,
                                                                incur_los_reim_fund_aggr_lim_amt = aps.incur_los_reim_fund_aggr_lim_amt,
                                                                incur_los_reim_fund_unlim_minimium_lim_ind = aps.incur_los_reim_fund_unlim_minimium_lim_ind,
                                                                incur_los_reim_fund_min_lim_amt = aps.incur_los_reim_fund_min_lim_amt,
                                                                incur_los_reim_fund_invc_lsi_ind = aps.incur_los_reim_fund_invc_lsi_ind,
                                                                //SR 325928(added newly)
                                                                incur_los_reim_fund_othr_amt=aps.incur_los_reim_fund_othr_amt,
                                                                Escrow_PLMNumber = aps.escr_paid_los_bil_mms_nbr,
                                                                Escrow_Diveser = aps.escr_dvsr_nbr,
                                                                Escrow_MnthsHeld = aps.escr_mms_held_amt,
                                                                Escrow_PrevAmt = aps.escr_prev_amt,

                                                                use_dpst_ind = aps.use_dpst_ind == null ? false : aps.use_dpst_ind,

                                                                actv_ind = aps.actv_ind,                                                                
                                                                CREATE_DATE = aps.crte_dt,
                                                                CREATE_USER_ID = aps.crte_user_id,
                                                                UPDATE_DATE = aps.updt_dt,
                                                                UPDATE_USER_ID = aps.updt_user_id
                                                            });
            if (query.Count() > 0)
            {
                result = query.Single();

                result.AdjParametPolBEs = (new Adj_paramet_PolDA()).getAdjParamtrPol(result.adj_paramet_setup_id);
                return result;
            }
            else
                return null;

        }
        //public IList<AdjustmentParameterSetupBE> getAdjParamtrByPrPID(int ProgramPeriodID)
        //{            
        //    IList<AdjustmentParameterSetupBE> result = new List<AdjustmentParameterSetupBE>();

        //    IQueryable<AdjustmentParameterSetupBE> query = this.BuildQuery();

        //    result = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.prem_adj_pgm_id == ProgramPeriodID);

        //        if (query.Count() > 0) result = query.ToList();

        //        foreach (AdjustmentParameterSetupBE setupBE in resultl)
        //        {
        //            setupBE.AdjParametPolBEs = (new Adj_paramet_PolDA()).getAdjParamtrPol(setupBE.adj_paramet_setup_id);
        //        }
        //        //result.AdjParametPolBEs = (new Adj_paramet_PolDA()).getAdjParamtrPol(result.adj_paramet_setup_id);

        //    return resultl;            
        //}


        private IQueryable<AdjustmentParameterSetupBE> BuildQuery()
        {

            if (this.Context == null)
                this.Initialize();
            IQueryable<AdjustmentParameterSetupBE> query = from AdjPrmtrSetup in this.Context.PREM_ADJ_PGM_SETUPs
                                                           select new AdjustmentParameterSetupBE
                                                      {
                                                          prem_adj_pgm_id = AdjPrmtrSetup.prem_adj_pgm_id,
                                                          adj_paramet_setup_id = AdjPrmtrSetup.prem_adj_pgm_setup_id,
                                                          lba_Adjustment_typ = AdjPrmtrSetup.los_base_asses_adj_typ_id,
                                                          incld_ernd_retro_prem_ind = AdjPrmtrSetup.incld_ernd_retro_prem_ind,
                                                          depst_amt = AdjPrmtrSetup.depst_amt,
                                                          Cstmr_Id = AdjPrmtrSetup.custmr_id,
                                                          incld_ibnr_ldf_ind = AdjPrmtrSetup.incld_incur_but_not_rptd_ind,
                                                          AdjparameterTypeID = AdjPrmtrSetup.adj_parmet_typ_id,
                                                          loss_convfact_calimcap = AdjPrmtrSetup.los_conv_fctr_clm_cap_amt,
                                                          loss_convfact_aggamt = AdjPrmtrSetup.los_conv_fctr_aggr_cap_amt,
                                                          lay_lossconv_FactInsPay = AdjPrmtrSetup.los_conv_fctr_lyr_insd_pays_amt,
                                                          lay_lossconv_znapayamt = AdjPrmtrSetup.los_conv_fctr_lyr_zna_pays_amt,
                                                          clm_hndl_fee_basis_id = AdjPrmtrSetup.clm_hndl_fee_basis_id,
                                                          Escrow_PLMNumber = AdjPrmtrSetup.escr_paid_los_bil_mms_nbr,
                                                          Escrow_Diveser = AdjPrmtrSetup.escr_dvsr_nbr,
                                                          Escrow_MnthsHeld = AdjPrmtrSetup.escr_mms_held_amt,
                                                          Escrow_PrevAmt = AdjPrmtrSetup.escr_prev_amt,

                                                          incur_but_not_rptd_los_dev_fctr_id = AdjPrmtrSetup.incur_but_not_rptd_los_dev_fctr_id,
                                                          incur_los_reim_fund_initl_fund_amt = AdjPrmtrSetup.incur_los_reim_fund_initl_fund_amt,
                                                          incur_los_reim_fund_unlim_agmt_lim_ind = AdjPrmtrSetup.incur_los_reim_fund_unlim_agmt_lim_ind,
                                                          incur_los_reim_fund_aggr_lim_amt = AdjPrmtrSetup.incur_los_reim_fund_aggr_lim_amt,
                                                          incur_los_reim_fund_unlim_minimium_lim_ind = AdjPrmtrSetup.incur_los_reim_fund_unlim_minimium_lim_ind,
                                                          incur_los_reim_fund_min_lim_amt = AdjPrmtrSetup.incur_los_reim_fund_min_lim_amt,
                                                          incur_los_reim_fund_invc_lsi_ind = AdjPrmtrSetup.incur_los_reim_fund_invc_lsi_ind,
                                                          //Added for SR 325928
                                                          incur_los_reim_fund_othr_amt=AdjPrmtrSetup.incur_los_reim_fund_othr_amt,

                                                          use_dpst_ind = AdjPrmtrSetup.use_dpst_ind == null? false : AdjPrmtrSetup.use_dpst_ind,

                                                          actv_ind = AdjPrmtrSetup.actv_ind,
                                                          CREATE_DATE = AdjPrmtrSetup.crte_dt,
                                                          CREATE_USER_ID = AdjPrmtrSetup.crte_user_id,
                                                          UPDATE_DATE = AdjPrmtrSetup.updt_dt,
                                                          UPDATE_USER_ID = AdjPrmtrSetup.updt_user_id
                                                      };
            return query;
        }

        ///// <summary>
        ///// Retrieves the Adjustment Parameter setup Information LCF
        ///// for a particular ProgramPeriodID
        ///// </summary>
        ///// <param name="ProgramPeriodID"></param>
        ///// <returns>List of Adj_Parameter_setupBE</returns>
        //public IList<AdjustmentParameterSetupBE> getAdjParamtrLCF(int ProgramPeriodID, int CstmrID)
        //{
        //    IList<AdjustmentParameterSetupBE> resultlcf = new List<AdjustmentParameterSetupBE>();

        //    IQueryable<AdjustmentParameterSetupBE> query = this.BuildQueryLCF();

        //    if (ProgramPeriodID > 0)
        //    {
        //        query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.prem_adj_pgm_id == ProgramPeriodID);
        //    }

        //    if (CstmrID > 0)
        //    {
        //        query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.Cstmr_Id == CstmrID);
        //    }
        //    if (query.Count() > 0)
        //        resultlcf = query.ToList();

        //    foreach (AdjustmentParameterSetupBE setupBE in resultlcf)
        //    {
        //        setupBE.AdjParametPolBEs = (new Adj_paramet_PolDA()).getAdjParamtrPol(setupBE.adj_paramet_setup_id);
        //    }

        //    return resultlcf;
        //    //return (query.ToList());
        //}

        //private IQueryable<AdjustmentParameterSetupBE> BuildQueryLCF()
        //{

        //    if (this.Context == null)
        //        this.Initialize();
        //    IQueryable<AdjustmentParameterSetupBE> query = from AdjPrmtrSetup in this.Context.PREM_ADJ_PGM_SETUPs
        //                                                   where AdjPrmtrSetup.actv_ind == true
        //                                                   select new AdjustmentParameterSetupBE
        //                                                   {
        //                                                       prem_adj_pgm_id = AdjPrmtrSetup.prem_adj_pgm_id,
        //                                                       adj_paramet_setup_id = AdjPrmtrSetup.prem_adj_pgm_setup_id,
        //                                                       Cstmr_Id = AdjPrmtrSetup.custmr_id,
        //                                                       loss_convfact_calimcap = AdjPrmtrSetup.los_conv_fctr_clm_cap_amt,
        //                                                       loss_convfact_aggamt = AdjPrmtrSetup.los_conv_fctr_aggr_cap_amt,
        //                                                       lay_lossconv_FactInsPay = AdjPrmtrSetup.los_conv_fctr_lyr_insd_pays_amt,
        //                                                       lay_lossconv_znapayamt = AdjPrmtrSetup.los_conv_fctr_lyr_zna_pays_amt
        //                                                   };
        //    return query;
        //}

        /// <summary>
        /// Retrieves the Adjustment Parameter setup Information
        /// for a particular ProgramPeriodID and ERP flag true
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <returns>List of Adj_Parameter_setupBE</returns>
        public IList<AdjustmentParameterSetupBE> getAdjParamtERPTrue(int ProgramPeriodID, int CstmrID, Boolean IncldERP)
        {
            IList<AdjustmentParameterSetupBE> resultl = new List<AdjustmentParameterSetupBE>();

            IQueryable<AdjustmentParameterSetupBE> query = this.BuildQuery();

            if (ProgramPeriodID > 0)
            {
                query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.prem_adj_pgm_id == ProgramPeriodID);
            }
            if (CstmrID > 0)
            {
                query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.Cstmr_Id == CstmrID);
            }
            if (IncldERP)
            {
                query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.incld_ernd_retro_prem_ind == true);

            }
            else
            {
                query = query.Where(AdjPrmtrSetup => AdjPrmtrSetup.incld_ernd_retro_prem_ind == false);
            }

            if (query.Count() > 0)
                resultl = query.ToList();



            foreach (AdjustmentParameterSetupBE setupBE in resultl)
            {
                setupBE.AdjParametPolBEs = (new Adj_paramet_PolDA()).getAdjParamtrPol(setupBE.adj_paramet_setup_id);
            }
            return resultl;
            //return (query.ToList());
        }


        /// <summary>
        /// retrives Adjparameter setup details 
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="cstmrID"></param>
        /// <param name="adjParamTypeID"></param>
        /// <returns>List of AdjParameters</returns>
        public string getAdjParamResult(int programPeriodID, int cstmrID, int adjParamTypeID, bool strIncluded)
        {
            AdjustmentParameterSetupBE result = new AdjustmentParameterSetupBE();

            IQueryable<AdjustmentParameterSetupBE> query = (from aps in this.Context.PREM_ADJ_PGM_SETUPs
                                                            where aps.prem_adj_pgm_id == programPeriodID && aps.custmr_id == cstmrID && aps.adj_parmet_typ_id == adjParamTypeID && aps.incld_ernd_retro_prem_ind == strIncluded
                                                            select new AdjustmentParameterSetupBE
                                                            {
                                                                prem_adj_pgm_id = aps.prem_adj_pgm_id,
                                                               
                                                            });
            if (query.Count() > 0)
            {
               return "true";
            }
            else
                return "false";

        }
        /// <summary>
        /// retrives Adjparameter setup details 
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="cstmrID"></param>
        /// <param name="adjParamTypeID"></param>
        /// <returns>List of AdjParameters</returns>
        public string getAdjParamResultOther(int programPeriodID, int cstmrID, int adjParamTypeID)
        {
            AdjustmentParameterSetupBE result = new AdjustmentParameterSetupBE();

            IQueryable<AdjustmentParameterSetupBE> query = (from aps in this.Context.PREM_ADJ_PGM_SETUPs
                                                            where aps.prem_adj_pgm_id == programPeriodID && aps.custmr_id == cstmrID && aps.adj_parmet_typ_id == adjParamTypeID 
                                                            select new AdjustmentParameterSetupBE
                                                            {
                                                                prem_adj_pgm_id = aps.prem_adj_pgm_id,

                                                            });
            if (query.Count() > 0)
            {
                return "true";
            }
            else
                return "false";

        }

    }
}
