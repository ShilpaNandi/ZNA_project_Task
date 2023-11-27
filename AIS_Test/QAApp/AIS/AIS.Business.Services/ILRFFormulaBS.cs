using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

namespace ZurichNA.AIS.Business.Logic
{
    public class ILRFFormulaBS : BusinessServicesBase<ILRFFormulaBE, ILRFFormulaDA>
    {
        /// <summary>
        /// Retrives all formulas info to fill ILRF Formula Setup Listview
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="customerID"></param>
        /// <param name="iBNRLDF"></param>
        /// <returns>List of ILRF Formula Setups</returns>
        public IList<ILRFFormulaBE> getILRFFormulas(int programPeriodID, int customerID, string iBNRLDF)
        {
            IList<ILRFFormulaBE> Formulas = new List<ILRFFormulaBE>();

            ILRFFormulaDA FormulaInfo = new ILRFFormulaDA();
            try
            {
                Formulas = FormulaInfo.getILRFFormulas(programPeriodID, customerID, iBNRLDF);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Formulas;
        }

        public IList<ILRFFormulaBE> getILRFFormulasForProgPerCopy(int programPeriodID, int customerID, string iBNRLDF)
        {
            IList<ILRFFormulaBE> Formulas = new List<ILRFFormulaBE>();

            ILRFFormulaDA FormulaInfo = new ILRFFormulaDA();
            try
            {
                Formulas = FormulaInfo.getILRFFormulasForProgPerCopy(programPeriodID, customerID, iBNRLDF);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Formulas;
        }
        /// <summary>
        /// Loads ILRF Formula Setups row using Framework
        /// </summary>
        /// <param name="iLRFFormulaSetupID"></param>
        /// <returns>ILRFFormulaSetuo row</returns>
        public ILRFFormulaBE getILRFFormulaRow(int iLRFFormulaSetupID)
        {
            ILRFFormulaBE iLRFFormulaBE = new ILRFFormulaBE();
            try
            {
                iLRFFormulaBE = DA.Load(iLRFFormulaSetupID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return iLRFFormulaBE;

        }
        /// <summary>
        /// Save and Update ILRF Formula Setup data using Framwork
        /// </summary>
        /// <param name="iLRFFormulaBE"></param>
        /// <returns>returns tru if save success, else false</returns>
        public bool Update(ILRFFormulaBE iLRFFormulaBE)
        {
            bool succeed = false;
            if (iLRFFormulaBE.INCURRED_LOSS_REIM_FUND_FRMLA_ID > 0)
            {
                succeed = this.Save(iLRFFormulaBE);
            }
            else
            {
                succeed = this.DA.Add(iLRFFormulaBE);
            }
            return succeed;
        }
        /// <summary>
        /// Invoked to delete all Factoers
        /// </summary>
        /// <param name="adjParmtSetupID"></param>
        /// <returns></returns>
        public bool DeleteFactors(int programPeriodID)
        {
            bool succeed = false;
            ILRFFormulaDA FormulaInfo = new ILRFFormulaDA();
            try
            {
                FormulaInfo.DeleteFactors(programPeriodID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return succeed;
        }

        public IList<ILRFFormulaBE> GetDefaultILRFFormulas(int programPeriodID, int customerID, string iBNRLDF)
        {
            IList<ILRFFormulaBE> result = new List<ILRFFormulaBE>();
            result = (new ILRFFormulaDA()).GetDefaultILRFFormulas(programPeriodID, customerID, iBNRLDF);
            return result;
        }
    }

}
