using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{
    public class AssignERPFormulaDA : DataAccessor<PREM_ADJ_PGM, ProgramPeriodBE, AISDatabaseLINQDataContext>
    {
        public IList<ProgramPeriodBE> getProgramPeriodData(int ProgramID)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<ProgramPeriodBE> query =
            (from cdd in this.Context.PREM_ADJ_PGMs
             select new ProgramPeriodBE()
             {
                 PREM_ADJ_PGM_ID = cdd.prem_adj_pgm_id,
                 MSTR_ERND_RETRO_PREM_FRMLA_ID= cdd.mstr_ernd_retro_prem_frmla_id
             });

            /// Get a specific account record
            if (ProgramID > 0)
            {
                query = query.Where(cdd => cdd.PREM_ADJ_PGM_ID == ProgramID);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
    }
}
