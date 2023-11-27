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
    /// This Class is used to interact with Master Earned Retro Formula Table
    /// </summary>
    public class MasterERPFormulaBS : BusinessServicesBase<MasterERPFormulaBE, MasterERPFormulaDA>
    {
        /// <summary>
        /// Function For retrieving ERP formulas
        /// </summary>
        /// <returns>MasterERPFormulaBE</returns>
        public IList<MasterERPFormulaBE> getERPFormulas(int prmPrdID)
        {
            IList<MasterERPFormulaBE> list = new List<MasterERPFormulaBE>();
            MasterERPFormulaDA account = new MasterERPFormulaDA();

            try
            {
                list = account.getERPFormulas(prmPrdID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }


            return list;
        }
        public IList<MasterERPFormulaBE> getERPFormulas()
        {
            IList<MasterERPFormulaBE> list = new List<MasterERPFormulaBE>();
            MasterERPFormulaDA account = new MasterERPFormulaDA();

            try
            {
                list = account.getERPFormulas();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }


            return list;
        }

        public IList<MasterERPFormulaBE> getERPFormulasWithOrder()
        {
            IList<MasterERPFormulaBE> list = new List<MasterERPFormulaBE>();
            MasterERPFormulaDA account = new MasterERPFormulaDA();

            try
            {
                list = account.getERPFormulasWithOrder();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }


            return list;
        }
        
        /// <summary>
        /// Function to fetch the MasterERP Record based on FormulaID
        /// </summary>
        /// <param name="FormulaID"></param>
        /// <returns>MasterERPFormulaBE</returns>
        public MasterERPFormulaBE getERPFormulaRow(int FormulaID)
        {
            MasterERPFormulaBE result = new MasterERPFormulaBE();
            result = DA.Load(FormulaID);
            return result;
        }
        /// <summary>
        /// Function to Update the Master ERPFormula
        /// </summary>
        /// <param name="ERPBE"></param>
        /// <returns>bool True/False(saved or not)</returns>
        public bool UpdateFormula(MasterERPFormulaBE ERPBE)
        {
            bool succeed = false;

            if (ERPBE.FormulaID > 0) //On Update
            {
                succeed = this.Save(ERPBE);
            }
            else //On Insert
            {
                succeed = DA.Add(ERPBE);
            }

            return succeed;
        }
    }
}
