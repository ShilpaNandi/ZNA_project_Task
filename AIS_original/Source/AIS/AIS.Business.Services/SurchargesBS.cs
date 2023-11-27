/*-----	Page:	Surcharges Setup Business Service Layer
-----
-----	Created:		CSC (Zakir Hussain)

-----
-----	Description:	The business logic for Surcharges Setup is present here and it interacts with the data accessor layer to retrieve data from the database.
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 6/18/2010 (AS part of Surcharges Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

*/

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
    public class SurchargesBS : BusinessServicesBase<SurchargesBE, SurchargesDA>
    {
        /// <summary>
        /// Retrieve Surcharges information
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public IList<SurchargesBE> SelectData()
        {
            IList<SurchargesBE> result = new List<SurchargesBE>();
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.SelectData();
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
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.GetStates();
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
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.GetStatesForEdit(iLkupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        
        /// <summary>
        /// Saves or Edits to database using FrameWork
        /// </summary>
        /// <param name="DeductibleTaxesBE"></param>
        /// <returns>true if save, False if failed to save</returns>
        public bool SaveSurcharges(SurchargesBE sTaxesBE)
        {
            bool succeed = false;
            try
            {
                if (sTaxesBE.SURCHARGE_ASSESS_SETUP_ID > 0)
                {
                    succeed = this.Save(sTaxesBE);
                }
                else //On Insert
                {
                    succeed = DA.Add(sTaxesBE);
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;
        }

        
        public SurchargesBE SelectDataOnSetupId(int sTaxesSetupId)
        {
            SurchargesBE result = new SurchargesBE();
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.SelectDataOnSetupId(sTaxesSetupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> FactorDescription()
        {
            IList<LookupBE> result = new List<LookupBE>();
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.FactorDescription();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> FactorDescriptionForEdit(int iFactorId)
        {
            IList<LookupBE> result = new List<LookupBE>();
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.FactorDescriptionForEdit(iFactorId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        /// <summary>
        /// Retrieve particular Surcharges information record
        /// </summary>
        /// <param name="KOSetupId"></param>
        /// <returns></returns>
        public SurchargesBE LoadData(int sTaxesSetupId)
        {
            SurchargesBE Data = new SurchargesBE();
            try
            {
                Data = DA.Load(sTaxesSetupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Data;
        }
        /// <summary>
        /// Surcharges:Checks if a Surcharge  already exists with given parameters
        /// </summary>
        public int isSurAlreadyExist(int intSurSetupID, int ln_of_bsn_id, int intStateID, int intSurCode, DateTime dtSurEffDate)
        {
            int intTaxTypeCount;
            try
            {
                intTaxTypeCount = new SurchargesDA().isSurAlreadyExist(intSurSetupID, ln_of_bsn_id, intStateID, intSurCode, dtSurEffDate );
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (intTaxTypeCount);
        }

        public IList<LookupBE> GetCode(string Attribute1, string lkupTypeName)
        {
            IList<LookupBE> result = new List<LookupBE>();
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.GetCode(Attribute1, lkupTypeName);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public IList<LookupBE> GetCodeForEdit(int iCodeId, string Attribute1, string lkupTypeName)
        {
            IList<LookupBE> result = new List<LookupBE>();
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                result = staxesDA.GetCodeForEdit(iCodeId, Attribute1, lkupTypeName);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }
        public LookupBE GetDescription(string lkupTypeName)
        {
            LookupBE SurchargeDescriptionBE;
            SurchargesDA staxesDA = new SurchargesDA();
            try
            {
                SurchargeDescriptionBE = staxesDA.GetDescription(lkupTypeName);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return SurchargeDescriptionBE;
        }
        

    }
}
