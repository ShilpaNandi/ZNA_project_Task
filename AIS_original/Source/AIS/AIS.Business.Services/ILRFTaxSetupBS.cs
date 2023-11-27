using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;


//Texas Tax:Business Service Class 
#region Texas Tax Business Srevice Class
namespace ZurichNA.AIS.Business.Logic
{
    public class ILRFTaxSetupBS : BusinessServicesBase<ILRFTaxSetupBE, ILRFTaxSetupDA>
    {

        /// <summary>
        /// Texas Tax:Save and Update ILRF Tax Setup data using Framwork
        /// </summary>
        /// <param name="iLRFFormulaBE"></param>
        /// <returns>returns tru if save success, else false</returns>
        public bool Update(ILRFTaxSetupBE iLRFTaxBE)
        {
            bool succeed = false;
            if (iLRFTaxBE.INCURRED_LOSS_REIM_FUND_TAX_ID > 0)
            {
                succeed = this.Save(iLRFTaxBE);
            }
            else
            {
                succeed = this.DA.Add(iLRFTaxBE);
            }
            return succeed;
        }

        /// <summary>
        /// Texas Tax:uSed to get the Tax Type lookup data
        /// </summary>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getTaxDescriptionList()
        {
            IList<ILRFTaxSetupBE> TaxBEList = new List<ILRFTaxSetupBE>();

            ILRFTaxSetupDA TaxDA = new ILRFTaxSetupDA();
            try
            {
                TaxBEList = TaxDA.getTaxDescriptionList();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            ILRFTaxSetupBE selILRFTaxSetupBE = new ILRFTaxSetupBE();
            selILRFTaxSetupBE.INCURRED_LOSS_REIM_FUND_TAX_ID = 0;
            selILRFTaxSetupBE.INCURRED_LOSS_REIM_FUND_TAX_TYPE = "(Select)";
            TaxBEList.Insert(0, selILRFTaxSetupBE);
            return TaxBEList;
        }


        /// <summary>
        /// Texas Tax:To retrieve All Active TaxTypes union Used type(Active/Inactive) in EditMode 
        /// </summary>
        /// <param name="intTaxTypeID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getTaxDescriptionListEditData(int intTaxTypeID)
        {
            IList<ILRFTaxSetupBE> list = new List<ILRFTaxSetupBE>();
            ILRFTaxSetupDA TaxSetupDA = new ILRFTaxSetupDA();

            try
            {
                list = TaxSetupDA.getTaxDescriptionListEditData(intTaxTypeID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            ILRFTaxSetupBE selILRFTaxSetupBE = new ILRFTaxSetupBE();
            selILRFTaxSetupBE.INCURRED_LOSS_REIM_FUND_TAX_ID = 0;
            selILRFTaxSetupBE.INCURRED_LOSS_REIM_FUND_TAX_TYPE = "(Select)";
            list.Insert(0, selILRFTaxSetupBE);
            return list;
        }

        /// <summary>
        /// Texas Tax:USed to get Tax Setup records based on program period and custmer
        /// </summary>
        /// <param name="intPREM_ADJ_PGM_ID"></param>
        /// <param name="intCUSTMR_ID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getILRFTaxSetupList(int intPREM_ADJ_PGM_ID, int intCUSTMR_ID)
        {
            IList<ILRFTaxSetupBE> ILRFTaxBEList = new List<ILRFTaxSetupBE>();

            ILRFTaxSetupDA ILRFTaxDA = new ILRFTaxSetupDA();
            try
            {
                ILRFTaxBEList = ILRFTaxDA.getILRFTaxSetupList(intPREM_ADJ_PGM_ID, intCUSTMR_ID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return ILRFTaxBEList;
        }

         /// <summary>
        /// Texas Tax:USed to get Tax Setup records based on program period and custmer
        /// </summary>
        /// <param name="intPREM_ADJ_PGM_ID"></param>
        /// <param name="intCUSTMR_ID"></param>
        /// <returns>IList<ILRFTaxSetupBE></returns>
        public IList<ILRFTaxSetupBE> getILRFTaxSetupListData(int intPREM_ADJ_PGM_ID, int intST_ID)
        {
            IList<ILRFTaxSetupBE> ILRFTaxBEList = new List<ILRFTaxSetupBE>();

            ILRFTaxSetupDA ILRFTaxDA = new ILRFTaxSetupDA();
            try
            {
                ILRFTaxBEList = ILRFTaxDA.getILRFTaxSetupListData(intPREM_ADJ_PGM_ID, intST_ID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return ILRFTaxBEList;
        }
        
        /// <summary>
        /// Texas Tax:Loads ILRF Tax Setup row using Framework
        /// </summary>
        /// <param name="iLRFFormulaSetupID"></param>
        /// <returns>ILRFTaxSetupBE row</returns>
        public ILRFTaxSetupBE getILRFTaxSetupRow(int iLRFTaxSetupID)
        {
            ILRFTaxSetupBE iLRFTaxBE = new ILRFTaxSetupBE();
            try
            {
                iLRFTaxBE = DA.Load(iLRFTaxSetupID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return iLRFTaxBE;

        }

        /// <summary>
        /// Texas Tax:Checks if a State is tax exempted with given parameters
        /// </summary>
        public int isTaxExemptedState(int intTaxTypeID, int intPrem_adj_pgm_ID)
        {
            int intTaxTypeCount;
            try
            {
                intTaxTypeCount = new ILRFTaxSetupDA().isTaxExemptedState(intTaxTypeID, intPrem_adj_pgm_ID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (intTaxTypeCount);
        }

        /// <summary>
        /// Texas Tax:Checks if a Tax Type is already exists with given parameters
        /// </summary>
        public int isTaxTypeAlreadyExist(int intILRFTaxSetupID, int intTaxTypeID, int intPrem_adj_pgm_ID, int ln_of_bsn_id)
        {
            int intTaxTypeCount;
            try
            {
                intTaxTypeCount = new ILRFTaxSetupDA().isTaxTypeAlreadyExist(intILRFTaxSetupID, intTaxTypeID,ln_of_bsn_id, intPrem_adj_pgm_ID);
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
#endregion
