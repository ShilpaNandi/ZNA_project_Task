using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class Adj_paramet_PolDA : DataAccessor<PREM_ADJ_PGM_SETUP_POL, AdjustmentParameterPolicyBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Retrieves the Adjustment Parameter PolicyDetails Information
        /// for a particular AdjParameterSetupID
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <returns>List of AdjustmentParameterPolicyBE</returns>
        public IList<AdjustmentParameterPolicyBE> getAdjParamtrPol(int AdjParmSetupID)
        {
            IList<AdjustmentParameterPolicyBE> resultl = new List<AdjustmentParameterPolicyBE>();

           
            IQueryable<AdjustmentParameterPolicyBE> query = this.BuildQuery();

            if (AdjParmSetupID > 0)
            {
                query = query.Where(AdjPrmtrPolDtls => AdjPrmtrPolDtls.adj_paramet_setup_id == AdjParmSetupID);
                if (query.Count() > 0)
                    resultl = query.ToList();
            }
            else
            {
                resultl = null;
            }
            
            return resultl;
            //return (query.ToList());
        }

        private IQueryable<AdjustmentParameterPolicyBE> BuildQuery()
        {

            if (this.Context == null)
                this.Initialize();
            IQueryable<AdjustmentParameterPolicyBE> query = from AdjPrmtrPolDtls in this.Context.PREM_ADJ_PGM_SETUP_POLs 
                                                  from AdjPolicy in this.Context.COML_AGMTs  
                                                  where AdjPolicy.coml_agmt_id == AdjPrmtrPolDtls.coml_agmt_id
                                                  //orderby AdjPolicy.
                                                  select new AdjustmentParameterPolicyBE
                                                      {
                                                          PolicyPerfectNumber = AdjPolicy.pol_sym_txt+AdjPolicy.pol_nbr_txt+AdjPolicy.pol_modulus_txt,
                                                          adj_paramet_pol_id = AdjPrmtrPolDtls.prem_adj_pgm_setup_pol_id,
                                                          adj_paramet_setup_id = AdjPrmtrPolDtls.prem_adj_pgm_setup_id,
                                                          coml_agmt_id = AdjPrmtrPolDtls.coml_agmt_id,
                                                          custmrID = AdjPrmtrPolDtls.custmr_id, 
                                                          PrmadjPRgmID = AdjPrmtrPolDtls.prem_adj_pgm_id,
                                                          
                                                      };
            return query;
        }


        /// <summary>
        /// Delete all child rows for Adj_Paramt_Setup details
        /// </summary>
        /// <param name="adjParmSetupID"></param>
        public bool deleteAdjParmetPol(int custmrID, int adjParmSetupID)
        {
            bool returnvalue;
            if (custmrID > 0 && adjParmSetupID > 0)
            {
                var pols = from pol in this.Context.PREM_ADJ_PGM_SETUP_POLs
                           where pol.custmr_id == custmrID && pol.prem_adj_pgm_setup_id == adjParmSetupID
                           select pol;

                this.Context.PREM_ADJ_PGM_SETUP_POLs.DeleteAllOnSubmit(pols);

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
        /// <summary>
        ///  deletes all Prem adj Prg Setup Policies for ILRFSetup
        /// </summary>
        /// <param name="custmrID"></param>
        /// <param name="programPeriodID"></param>
        /// <param name="adjParmSetupID"></param>
        /// <returns>tru false</returns>
        public bool DeleteAdjPrmPol(int custmrID, int programPeriodID, int adjParmSetupID)
        {
            bool returnvalue;
            if (custmrID > 0 && programPeriodID > 0 && adjParmSetupID > 0)
            {
                var pols = (from pol in this.Context.PREM_ADJ_PGM_SETUP_POLs
                            where pol.custmr_id == custmrID && pol.prem_adj_pgm_setup_id == adjParmSetupID
                            && pol.prem_adj_pgm_id == programPeriodID
                            select pol);

                this.Context.PREM_ADJ_PGM_SETUP_POLs.DeleteAllOnSubmit(pols);

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

    }
}
