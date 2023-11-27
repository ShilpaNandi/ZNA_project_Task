using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class PremiumAdjustmentProgramStatusDA : DataAccessor<PREM_ADJ_PGM_ST, PremiumAdjustmentProgramStatusBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="progPerID"></param>
        /// <returns></returns>
        public IList<PremiumAdjustmentProgramStatusBE> GetProgramStatusList(int accountID, int progPerID)
        {
            IList<PremiumAdjustmentProgramStatusBE> PremAdjProgStsBEs =new List<PremiumAdjustmentProgramStatusBE>();

            IQueryable<PremiumAdjustmentProgramStatusBE> query = from prgSts in this.Context.PREM_ADJ_PGM_STs
                                where (prgSts.custmr_id == accountID && prgSts.prem_adj_pgm_id == progPerID)
                                select new PremiumAdjustmentProgramStatusBE 
                                {
                                    PREMUMADJPROG_STS_ID = prgSts.prem_adj_pgm_sts_id,
                                    PGM_STATUS_ID = prgSts.pgm_perd_sts_typ_id,
                                    STS_CHK_IND = prgSts.sts_chk_ind,
                                    CREATEDDATE = prgSts.crte_dt,
                                    CREATEDUSER_ID = prgSts.crte_user_id,
                                    UPDATEUSER_ID = prgSts.updt_user_id,
                                    UPDATEDATE = prgSts.updt_dt  
                                };

            PremAdjProgStsBEs = query.ToList();
            return PremAdjProgStsBEs;
        }

    }
}
