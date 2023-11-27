using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class DeductibleTaxesBS:BusinessServicesBase<DeductibleTaxesBE,DeductibleTaxesDA>
    {
        /// <summary>
        /// Retrieve Deductible Taxes information
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public IList<DeductibleTaxesBE> SelectData()
        {
            IList<DeductibleTaxesBE> result = new List<DeductibleTaxesBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.SelectData();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> GetStates()
        {
            IList<LookupBE> result = new List<LookupBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
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
        public IList<LookupBE> GetDescription(string Attribute1, string lkupTypeName)
        {
            IList<LookupBE> result = new List<LookupBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.GetDescription(Attribute1, lkupTypeName);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> GetDescriptionForEdit(int iDescriptionId,string Attribute1, string lkupTypeName)
        {
            IList<LookupBE> result = new List<LookupBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.GetDescriptionForEdit(iDescriptionId,Attribute1, lkupTypeName);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public string GetMainSub(string trns_nm_txt)
        {
            string mainSub = string.Empty;
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                mainSub = dtaxesDA.GetMainSub(trns_nm_txt);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return mainSub;
        }
        /// <summary>
        /// Saves or Edits to database using FrameWork
        /// </summary>
        /// <param name="DeductibleTaxesBE"></param>
        /// <returns>true if save, False if failed to save</returns>
        public bool SaveDeductibleTaxes(DeductibleTaxesBE dTaxesBE)
        {
            bool succeed = false;
            try
            {
                if (dTaxesBE.DED_TAXES_SETUP_ID > 0)
                {
                    succeed = this.Save(dTaxesBE);
                }
                else //On Insert
                {
                    succeed = DA.Add(dTaxesBE);
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;
        }

        public IList<DeductibleTaxesBE> SelectDataOnTransMainSub(string TransNmTxt, string strMain, string strSub)
        {
            IList<DeductibleTaxesBE> result = new List<DeductibleTaxesBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.SelectDataOnTransMainSub(TransNmTxt, strMain, strSub);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }

        public DeductibleTaxesBE SelectDataOnSetupId(int dTaxesSetupId)
        {
            DeductibleTaxesBE result = new DeductibleTaxesBE();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.SelectDataOnSetupId(dTaxesSetupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> GetComponentDescription()
        {
            IList<LookupBE> result = new List<LookupBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.GetComponentDescription();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> GetComponentDescriptionForEdit(int iComponentId)
        {
            IList<LookupBE> result = new List<LookupBE>();
            DeductibleTaxesDA dtaxesDA = new DeductibleTaxesDA();
            try
            {
                result = dtaxesDA.GetComponentDescriptionForEdit(iComponentId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        /// <summary>
        /// Retrieve particular deductibleTaxessetup information record
        /// </summary>
        /// <param name="KOSetupId"></param>
        /// <returns></returns>
        public DeductibleTaxesBE LoadData(int dTaxesSetupId)
        {
            DeductibleTaxesBE Data = new DeductibleTaxesBE();
            try
            {
                Data = DA.Load(dTaxesSetupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Data;
        }
        /// <summary>
        /// Texas Tax:Checks if a Deductible Tax Type is already exists with given parameters
        /// </summary>
        public int isTaxTypeAlreadyExist(int intDTaxSetupID, int ln_of_bsn_id, int intStateID, int intTaxTypeID, int intDTaxCompID, DateTime dtPolicyEffDate)
        {
            int intTaxTypeCount;
            try
            {
                intTaxTypeCount = new DeductibleTaxesDA().isDTaxAlreadyExist(intDTaxSetupID,ln_of_bsn_id,  intStateID,  intTaxTypeID,  intDTaxCompID, dtPolicyEffDate);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (intTaxTypeCount);
        }
        
    }
}
