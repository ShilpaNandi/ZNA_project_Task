using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{
    public class AcctSetupQCDA : DataAccessor<PREM_ADJ_PGM, ProgramPeriodBE, AISDatabaseLINQDataContext>
    {
        public IList<ProgramPeriodBE> getRelatedPrmPrdInfo(int prmPrdID)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<ProgramPeriodBE> query =
             (from pap in this.Context.PREM_ADJ_PGMs
              join cust in this.Context.CUSTMRs
             on pap.custmr_id equals cust.custmr_id
              //join pers in this.Context.PERs
            
              //  on pap.qlty_cntrl_pers_id equals pers.pers_id
              where pap.prem_adj_pgm_id == prmPrdID
              select new ProgramPeriodBE()
              {
                  PREM_ADJ_PGM_ID = pap.prem_adj_pgm_id,
                  CUSTMR_ID = pap.custmr_id,
                  QLTY_CNTRL_DT = pap.qlty_cntrl_dt,
                  QLTY_CMMNT_TXT = pap.qlty_cmmnt_txt,
                  QUALITYCONTROL_PERSON_ID = pap.qlty_cntrl_pers_id,
                  PersonName = cust.full_nm,
                  //PersonName = cust.full_nm,
                  //PersonName = pers.surname.Substring(0, 1) + ". " + pers.forename,
                  MSTR_ERND_RETRO_PREM_FRMLA_ID=pap.MSTR_ERND_RETRO_PREM_FRMLA.mstr_ernd_retro_prem_frmla_id,
                  UPDATE_DATE=pap.updt_dt,
                  UPDATE_USER_ID=pap.updt_user_id
              });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
    }
}