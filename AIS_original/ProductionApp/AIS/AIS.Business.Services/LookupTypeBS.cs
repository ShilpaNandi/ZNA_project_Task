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
    /// <summary>
    /// This Class is used to interact with LookupType Table
    /// </summary>
    public class LookupTypeBS : BusinessServicesBase<LookupTypeBE, LookupTypeDA>
    {

        /// <summary>
        /// Retrieves all LookupType Data 
        /// </summary>
        /// <returns>LookupTypeBE</returns>
        public IList<LookupTypeBE> getLookupTypeData()
        {
            LookupTypeDA lookupType = new LookupTypeDA();
            IList<LookupTypeBE> result = lookupType.getLookUpTypeData();
            return result;
        }
        /// <summary>
        /// Retrieves all LookupType Data based on LookupTypeID
        /// </summary>
        /// <param name="LookupTypeID"></param>
        /// <returns>LookupTypeBE</returns>
        public LookupTypeBE getLkupTypeRow(int LookupTypeID)
        {
            LookupTypeBE lkupTyBE = new LookupTypeBE();
            lkupTyBE = DA.Load(LookupTypeID);
            return lkupTyBE;
        }
        /// <summary>
        /// Function to update the LookuptypeBE
        /// </summary>
        /// <param name="lkupTypeBE"></param>
        /// <returns>bool  True/False</returns>
        public bool Update(LookupTypeBE lkupTypeBE)
        {
            bool succeed = false;
            try
            {
                if (lkupTypeBE.LOOKUPTYPE_ID > 0) //On Update
                {
                    succeed = this.Save(lkupTypeBE);

                }
                else //On Insert
                {
                    lkupTypeBE.LOOKUPTYPE_ID = DA.GetNextPkID().Value;
                    succeed = DA.Add(lkupTypeBE);
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }

            return succeed;

        }
        /// <summary>
        /// checks for Duplicate Record of LookupTypeData
        /// </summary>
        /// <param name="lookupTypeName"></param>
        /// <param name="LookupTypeID"></param>
        /// <returns>bool True/False</returns>
        public bool IsExistsLookupTypeName(string lookupTypeName, int LookupTypeID)
        {
            LookupTypeDA lookDA = new LookupTypeDA();
            return lookDA.IsExistsLookupTypeName(lookupTypeName, LookupTypeID);
        }
    }
}
