using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using System.Data;

namespace ZurichNA.AIS.Business.Logic
{
    public class Adj_paramet_DtlBS: BusinessServicesBase<AdjustmentParameterDetailBE, Adjt_paramet_DtlDA> 
    {


        /// <summary>
        /// This method is used to get the list of Adjustment Parameter Details for a given
        /// Program Period ID, Account ID\Customer ID and Adjustment Parameter Setup ID
        /// </summary>
        /// <param name="prgprmID"></param>
        /// <param name="AdjParameterSetupID"></param>
        /// <param name="Actid"></param>
        /// <returns></returns>
        public IList<AdjustmentParameterDetailBE> getLBAAdjParamtrDtls(int prgprmID, int AdjParameterSetupID, int Actid)
        {
            IList<AdjustmentParameterDetailBE> AdjParameterSetupDetails;
            try
            {
                AdjParameterSetupDetails = new Adjt_paramet_DtlDA().getLBAAdjParamtrDtls(prgprmID, AdjParameterSetupID, Actid);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //IList<AdjustmentParameterDetailBE> AdjParameterDtlBE = this.buildList(AdjParameterSetupDetails);

            return (AdjParameterSetupDetails);
        }
        public bool deleteAdjParamtrDtls(int ProgramPrdID, int AdjParameterSetupID, int AccntID)
        {
            bool AdjParamtrDtlsDelete;
            try
            {
                AdjParamtrDtlsDelete = new Adjt_paramet_DtlDA().deleteAdjParamtrDtls(ProgramPrdID, AdjParameterSetupID, AccntID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (AdjParamtrDtlsDelete);

        }
        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name 
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE</returns>
        public IList<AdjustmentParameterDetailBE> getAdjustprmState(int prgmprmID, int AdjParterSetupID, int Acntid)
        {
            IList<AdjustmentParameterDetailBE> Adjparmetstatelst;
            try
            {
                Adjparmetstatelst = new Adjt_paramet_DtlDA().getAdjustprmState(prgmprmID, AdjParterSetupID, Acntid);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //IList<AdjustmentParameterDetailBE> AdjParameterDtlBE = this.buildList(AdjParameterSetupDetails);

            return (Adjparmetstatelst);
        }

        /// <summary>
        /// Method used to load a single record for adjustment Parameter Details Business entity
        /// this method is used when we want to update an exisiting record in database
        /// </summary>
        /// <param name="adjPrmDtlID"></param>
        /// <returns></returns>
        public AdjustmentParameterDetailBE getAdjParamDtlRow(int adjPrmDtlID)
        {
            AdjustmentParameterDetailBE AdjParaDtlRow = new AdjustmentParameterDetailBE();
            AdjParaDtlRow = DA.Load(adjPrmDtlID);

            return AdjParaDtlRow;
        }

        /// <summary>
        /// Update and Save single Adjustment Parameter Detail row in a list view
        /// </summary>
        /// <param name="LBAadjParmtdetinfo"></param>
        /// <returns></returns>
        public bool Update(AdjustmentParameterDetailBE LBAadjParmtdetinfo)
        {
            bool suceeded = false;
            if (LBAadjParmtdetinfo.prem_adj_pgm_dtl_id > 0)
            {   
                suceeded = DA.Update(LBAadjParmtdetinfo);
            }
            else
            {
                LBAadjParmtdetinfo.act_ind = true;
                suceeded = DA.Add(LBAadjParmtdetinfo);
            }
            return suceeded;
        }

        public DataTable GetLSI_CHF_Interface(int custmr_id, int prem_adj_pgm_id, int erp_type)
        {
            return new Adjt_paramet_DtlDA().GetLSI_CHF_Interface(custmr_id, prem_adj_pgm_id, erp_type);
        }

        public int Getprem_adj_pgm_setup_pol_id(int custmr_id, int prem_adj_pgm_id, bool? incld_ernd_retro_prem_ind, int coml_agmt_id)
        {
            return new Adjt_paramet_DtlDA().Getprem_adj_pgm_setup_pol_id(custmr_id, prem_adj_pgm_id, incld_ernd_retro_prem_ind, coml_agmt_id);
        }

        public string GetPolicyNum(int custmr_id, int prem_adj_pgm_id, bool? incld_ernd_retro_prem_ind, int prem_adj_pgm_setup_pol_id)
        {
            return new Adjt_paramet_DtlDA().GetPolicyNum(custmr_id, prem_adj_pgm_id, incld_ernd_retro_prem_ind, prem_adj_pgm_setup_pol_id);
        }
    }
}
