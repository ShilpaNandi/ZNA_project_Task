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
    public class AcctSetupQCBS : BusinessServicesBase<ProgramPeriodBE, AcctSetupQCDA>
    {
        public AcctSetupQCBS()
        {
            // AccountComments = new CustomerCommentsBS();
        }
        public ProgramPeriodBE getPreAdjPgmRow(int PreAdjPgmID)
        {
            ProgramPeriodBE PreAdjPgmBE = new ProgramPeriodBE();
            PreAdjPgmBE = DA.Load(PreAdjPgmID);
            return PreAdjPgmBE;
        }
        //public Prem_Adj_PgmBE getPreAdjPgmRow(int ProgPrdID)
        //{
        //    Prem_Adj_PgmBE assignERP = new Prem_Adj_PgmBE();
        //    assignERP = DA.Load(ProgPrdID);
        //    return assignERP;
        //}


        public IList<ProgramPeriodBE> getRelatedPrmPrdInfo(int prmPrdID)
        {
            IList<ProgramPeriodBE> list = new List<ProgramPeriodBE>();
            AcctSetupQCDA accQC = new AcctSetupQCDA();

            try
            {
                list = accQC.getRelatedPrmPrdInfo(prmPrdID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
        public bool Update(ProgramPeriodBE papBE)
        {
            bool succeed = false;
            if (papBE.PREM_ADJ_PGM_ID > 0)
            {
                succeed = this.Save(papBE);
            }
            else //On Insert
            {
                //papBE.PremiumAdjustmentProgramID = DA.GetNextPkID().Value;
                succeed = DA.Add(papBE);
            }
            return succeed;
        }
    }
}
