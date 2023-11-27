using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.Logic;

namespace ZurichNA.AIS.DAL.Logic
{
    public class ILRFFormulaDA : DataAccessor<INCUR_LOS_REIM_FUND_FRMLA, ILRFFormulaBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Retrives all formulas info to fill ILRF Formula Setup Listview
        /// If records not available in table. Retrives empty rows using Lookupdata
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="customerID"></param>
        /// <param name="iBNRLDF"></param>
        /// <returns></returns>
        public IList<ILRFFormulaBE> getILRFFormulas(int programPeriodID, int customerID, string iBNRLDF)
        {
            IList<ILRFFormulaBE> result = new List<ILRFFormulaBE>();

            IQueryable<ILRFFormulaBE> query = from res in this.Context.INCUR_LOS_REIM_FUND_FRMLAs
                                              join lk in this.Context.LKUPs on res.los_reim_fund_fctr_typ_id equals lk.lkup_id
                                              join lktype in this.Context.LKUP_TYPs on lk.lkup_typ_id equals lktype.lkup_typ_id
                                              where lktype.lkup_typ_nm_txt == "LRF FACTOR TYPE" && lktype.actv_ind == true &&
                                              (lk.attr_1_txt == iBNRLDF || lk.attr_1_txt == "") &&
                                              res.prem_adj_pgm_id == programPeriodID && res.custmr_id == customerID
                                              select new ILRFFormulaBE
                                              {
                                                  INCURRED_LOSS_REIM_FUND_FRMLA_ID = res.incur_los_reim_fund_frmla_id,
                                                  LOSS_REIM_FUND_FACTOR_TYPE_ID = res.los_reim_fund_fctr_typ_id,
                                                  LOSS_REIM_FUND_FACTOR_TYPE = lk.lkup_txt,
                                                  USE_PAID_LOSS_INDICATOR = res.use_paid_los_ind,
                                                  USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = res.use_paid_aloc_los_adj_exps_ind,
                                                  USE_RESERVE_LOSS_INDICATOR = res.use_resrv_los_ind,
                                                  USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = res.use_resrv_aloc_los_adj_exps_ind,
                                                  UPDATE_DATE = res.updt_dt
                                              };

            result = query.ToList();
            //If no data available in DB table. Retriving empty rows using Lookup data
            if (result == null || result.Count <= 0)
            {
                IList<LookupBE> lookup = new List<LookupBE>();

                IQueryable<LookupBE> lkpQuery =
                    (from lkp in this.Context.LKUPs
                     join lkptyp in this.Context.LKUP_TYPs on lkp.lkup_typ_id equals lkptyp.lkup_typ_id
                     where lkptyp.lkup_typ_nm_txt == "LRF FACTOR TYPE" && lkptyp.actv_ind == true &&
                     (lkp.attr_1_txt == iBNRLDF || lkp.attr_1_txt == "")
                     select new LookupBE
                     {
                         LookUpName = lkp.lkup_txt,
                         LookUpID = lkp.lkup_id

                     });
                int rsLoop = 0;

                foreach (LookupBE lkupitem in lkpQuery)
                {
                    result.Add(new ILRFFormulaBE());
                    result[rsLoop].LOSS_REIM_FUND_FACTOR_TYPE_ID = lkupitem.LookUpID;
                    result[rsLoop].LOSS_REIM_FUND_FACTOR_TYPE = lkupitem.LookUpName;
                    switch (lkupitem.LookUpName.Trim().ToUpper())
                    { 
                        case "LBA":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            break;
                        case "LCF":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            break;
                        case "LDF":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            break;
                        case "IBNR":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            break;
                        case "IBNR - USE LBA":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = false;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            break;
                        case "IBNR - USE LCF":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            break;
                        case "LDF - USE LBA":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = false;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                            break;
                        case "LDF - USE LCF":
                            result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                            result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                            result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                            break;

                    }
                    

                    rsLoop++;
                }
            }

            return result;
        }
        /// <summary>
        /// This will retive only from database table. Will not retive from Lookups if no records in DB table. 
        /// unlike getILRFFormulas
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="customerID"></param>
        /// <param name="iBNRLDF"></param>
        /// <returns></returns>
        public IList<ILRFFormulaBE> getILRFFormulasForProgPerCopy(int programPeriodID, int customerID, string iBNRLDF)
        {
            IList<ILRFFormulaBE> result = new List<ILRFFormulaBE>();

            IQueryable<ILRFFormulaBE> query = from res in this.Context.INCUR_LOS_REIM_FUND_FRMLAs
                                              join lk in this.Context.LKUPs on res.los_reim_fund_fctr_typ_id equals lk.lkup_id
                                              join lktype in this.Context.LKUP_TYPs on lk.lkup_typ_id equals lktype.lkup_typ_id
                                              where lktype.lkup_typ_nm_txt == "LRF FACTOR TYPE" && lktype.actv_ind == true &&
                                              (lk.attr_1_txt == iBNRLDF || lk.attr_1_txt == "") &&
                                              res.prem_adj_pgm_id == programPeriodID && res.custmr_id == customerID
                                              select new ILRFFormulaBE
                                              {
                                                  INCURRED_LOSS_REIM_FUND_FRMLA_ID = res.incur_los_reim_fund_frmla_id,
                                                  LOSS_REIM_FUND_FACTOR_TYPE_ID = res.los_reim_fund_fctr_typ_id,
                                                  LOSS_REIM_FUND_FACTOR_TYPE = lk.lkup_txt,
                                                  USE_PAID_LOSS_INDICATOR = res.use_paid_los_ind,
                                                  USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = res.use_paid_aloc_los_adj_exps_ind,
                                                  USE_RESERVE_LOSS_INDICATOR = res.use_resrv_los_ind,
                                                  USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = res.use_resrv_aloc_los_adj_exps_ind
                                              };

            result = query.ToList();
            return result;
        }

        public bool DeleteFactors(int programPeriodID)
        {
            bool returnvalue;
            if (programPeriodID > 0)
            {
                var facts = (from fac in this.Context.INCUR_LOS_REIM_FUND_FRMLAs
                             where fac.prem_adj_pgm_id == programPeriodID
                             select fac);

                this.Context.INCUR_LOS_REIM_FUND_FRMLAs.DeleteAllOnSubmit(facts);

                this.Context.SubmitChanges();

                returnvalue = true;
            }
            else
            {
                returnvalue = false;
            }

            return returnvalue;
        }

        public IList<ILRFFormulaBE> GetDefaultILRFFormulas(int programPeriodID, int customerID, string iBNRLDF)
        {
            IList<ILRFFormulaBE> result = new List<ILRFFormulaBE>();

            IList<LookupBE> lookup = new List<LookupBE>();

            IQueryable<LookupBE> lkpQuery =
                (from lkp in this.Context.LKUPs
                 join lkptyp in this.Context.LKUP_TYPs on lkp.lkup_typ_id equals lkptyp.lkup_typ_id
                 where lkptyp.lkup_typ_nm_txt == "LRF FACTOR TYPE" && lkptyp.actv_ind == true &&
                 (lkp.attr_1_txt == iBNRLDF || lkp.attr_1_txt == "")
                 select new LookupBE
                 {
                     LookUpName = lkp.lkup_txt,
                     LookUpID = lkp.lkup_id

                 });
            int rsLoop = 0;

            foreach (LookupBE lkupitem in lkpQuery)
            {
                result.Add(new ILRFFormulaBE());
                result[rsLoop].LOSS_REIM_FUND_FACTOR_TYPE_ID = lkupitem.LookUpID;
                result[rsLoop].LOSS_REIM_FUND_FACTOR_TYPE = lkupitem.LookUpName;
                result[rsLoop].PROGRAMPERIOD_ID = programPeriodID;
                result[rsLoop].CUSTOMER_ID = customerID;

                switch (lkupitem.LookUpName.Trim().ToUpper())
                {
                    case "LBA":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        break;
                    case "LCF":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        break;
                    case "LDF":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        break;
                    case "IBNR":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        break;
                    case "IBNR - USE LBA":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = false;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        break;
                    case "IBNR - USE LCF":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        break;
                    case "LDF - USE LBA":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = false;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = false;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = false;
                        break;
                    case "LDF - USE LCF":
                        result[rsLoop].USE_PAID_LOSS_INDICATOR = true;
                        result[rsLoop].USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_LOSS_INDICATOR = true;
                        result[rsLoop].USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = true;
                        break;
                }
                rsLoop++;
        }

        return result;

        }

    }
}
