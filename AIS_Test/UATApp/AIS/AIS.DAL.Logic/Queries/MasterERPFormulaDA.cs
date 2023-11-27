using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{   //DataAccessor for ERP Formula details
    public class MasterERPFormulaDA : DataAccessor<MSTR_ERND_RETRO_PREM_FRMLA, MasterERPFormulaBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Function for retrieving All ERP Formulas
        /// </summary>
        /// <returns>MasterERPFormulaBE</returns>
        public IList<MasterERPFormulaBE> getERPFormulas(int prmPrdID)
        {
            IList<MasterERPFormulaBE> result = new List<MasterERPFormulaBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve ERP Formulas
            ///  into MasterERPFormula Business Entity
            IQueryable<MasterERPFormulaBE> query =
            (from cdd in this.Context.MSTR_ERND_RETRO_PREM_FRMLAs
             join pap in this.Context.PREM_ADJ_PGMs
             on cdd.mstr_ernd_retro_prem_frmla_id equals pap.mstr_ernd_retro_prem_frmla_id
             where pap.prem_adj_pgm_id==prmPrdID
             select new MasterERPFormulaBE()
             {
                 FormulaID = cdd.mstr_ernd_retro_prem_frmla_id,
                 FormulaOneText = cdd.ernd_retro_prem_frmla_one_txt,
                 FormulaTwoText = cdd.ernd_retro_prem_frmla_two_txt,
                 FormulaDescription = cdd.ernd_retro_prem_frmla_desc_txt,
                 IsActive=cdd.actv_ind,

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        public IList<MasterERPFormulaBE> getERPFormulas()
        {
            IList<MasterERPFormulaBE> result = new List<MasterERPFormulaBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve ERP Formulas
            ///  into MasterERPFormula Business Entity
            IQueryable<MasterERPFormulaBE> query =
            (from cdd in this.Context.MSTR_ERND_RETRO_PREM_FRMLAs
             orderby cdd.ernd_retro_prem_frmla_desc_txt ascending
             select new MasterERPFormulaBE()
             {
                 FormulaID = cdd.mstr_ernd_retro_prem_frmla_id,
                 FormulaOneText = cdd.ernd_retro_prem_frmla_one_txt,
                 FormulaTwoText = cdd.ernd_retro_prem_frmla_two_txt,
                 FormulaDescription = cdd.ernd_retro_prem_frmla_desc_txt.Substring(4),
                 IsActive = cdd.actv_ind,

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        public IList<MasterERPFormulaBE> getERPFormulasWithOrder()
        {
            IList<MasterERPFormulaBE> result = new List<MasterERPFormulaBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve ERP Formulas
            ///  into MasterERPFormula Business Entity
            IQueryable<MasterERPFormulaBE> query =
            (from cdd in this.Context.MSTR_ERND_RETRO_PREM_FRMLAs
             orderby cdd.ernd_retro_prem_frmla_desc_txt ascending
             select new MasterERPFormulaBE()
             {
                 FormulaID = cdd.mstr_ernd_retro_prem_frmla_id,
                 FormulaOneText = cdd.ernd_retro_prem_frmla_one_txt,
                 FormulaTwoText = cdd.ernd_retro_prem_frmla_two_txt,
                 FormulaDescription = cdd.ernd_retro_prem_frmla_desc_txt,
                 IsActive = cdd.actv_ind,

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
    }
}
