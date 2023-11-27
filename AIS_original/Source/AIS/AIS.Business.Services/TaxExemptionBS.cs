using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;

namespace ZurichNA.AIS.Business.Logic
{
    public class TaxExemptionBS : BusinessServicesBase<TaxExemptionBE, TaxExemptionDA>
    {
        /// <summary>
        /// Texas Tax:Save and Update Tax Exempt Setup data using Framwork
        /// </summary>
        /// <param name="TaxExemptBE"></param>
        /// <returns>returns tru if save success, else false</returns>
        public bool Update(TaxExemptionBE TaxExemptBE)
        {
            bool succeed = false;
            if (TaxExemptBE.TAX_EXMP_SETUP_ID > 0)
            {
                succeed = this.Save(TaxExemptBE);
            }
            else
            {
                succeed = this.DA.Add(TaxExemptBE);
            }
            return succeed;
        }

        /// <summary>
        /// Texas Tax:Loads  Tax Exempt Setup row using Framework
        /// </summary>
        /// <param name="TaxExemptSetupID"></param>
        /// <returns>TaxExemptionBE row</returns>
        public TaxExemptionBE getTaxExemptSetupRow(int TaxExemptSetupID)
        {
            TaxExemptionBE TaxExemptBE = new TaxExemptionBE();
            try
            {
                TaxExemptBE = DA.Load(TaxExemptSetupID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return TaxExemptBE;

        }

        /// <summary>
        /// Texas Tax:USed to get Tax Exempt Setup records based on program period and custmer
        /// </summary>
        /// <param name="intPREM_ADJ_PGM_ID"></param>
        /// <param name="intCUSTMR_ID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<TaxExemptionBE> getILRFTaxSetupList(int intPREM_ADJ_PGM_ID, int intCUSTMR_ID)
        {
            IList<TaxExemptionBE> TaxExemptBEList = new List<TaxExemptionBE>();

            TaxExemptionDA TaxExemptDA = new TaxExemptionDA();
            try
            {
                TaxExemptBEList = TaxExemptDA.getTaxExemptSetupList(intPREM_ADJ_PGM_ID, intCUSTMR_ID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return TaxExemptBEList;
        }

        /// <summary>
        /// Texas Tax:Checks if a Tax Exempt is already exists with given parameters
        /// </summary>
        public int isTaxExemptAlreadyExist(int intTaxExemptSetupID, int st_id, int intPrem_adj_pgm_ID)
        {
            int intTaxExemptCount;
            try
            {
                intTaxExemptCount = new TaxExemptionDA().isTaxExemptAlreadyExist(intTaxExemptSetupID, st_id,intPrem_adj_pgm_ID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (intTaxExemptCount);
        }

        public IList<LookupBE> GetStates()
        {
            IList<LookupBE> result = new List<LookupBE>();
            TaxExemptionDA dtaxesDA = new TaxExemptionDA();
            try
            {
                result = dtaxesDA.GetStates();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> GetStatesForEdit(int iLkupId)
        {
            IList<LookupBE> result = new List<LookupBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.GetStatesForEdit(iLkupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }

        public int getTaxTypID(int st_id)
        {
            int intTaxTypeID;
            try
            {
                intTaxTypeID = new TaxExemptionDA().getTaxTypID(st_id);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (intTaxTypeID);
        }

    }
}