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
    public class AdjQCBS : BusinessServicesBase<PremiumAdjustmentStatusBE, AdjQCDA>
    {
        public AdjQCBS()
        { }
        public IList<PremiumAdjustmentStatusBE> getPreAdjStatusList(int PremAdjID)
        {
            IList<PremiumAdjustmentStatusBE> list = new List<PremiumAdjustmentStatusBE>();
            PremumAdjustdmentStatusDA PremumAdjustmentStatus = new PremumAdjustdmentStatusDA();

            try
            {
                list = PremumAdjustmentStatus.getPremumAdjustmentStatusList(PremAdjID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.PremumAdjustmentStatus_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PremumAdjustmentStatus_ID;
            return list;
        }

        public PremiumAdjustmentStatusBE getPreAdjStatusRow(int PreAdjStaID)
        {
            PremiumAdjustmentStatusBE PreAdjBE = new PremiumAdjustmentStatusBE();
            PreAdjBE = DA.Load(PreAdjStaID);
            return PreAdjBE;
        }
        //public PremiumAdjustmentStatusBE getPreAdjPgmRow(int ProgPrdID)
        //{
        //    PremiumAdjustmentStatusBE assignERP = new PremiumAdjustmentStatusBE();
        //    assignERP = DA.Load(ProgPrdID);
        //    return assignERP;
        //}

        public IList<PremiumAdjustmentStatusBE> getRelatedAdjSetupQCItems(int adjID)
        {
            IList<PremiumAdjustmentStatusBE> list = new List<PremiumAdjustmentStatusBE>();
            AdjQCDA accQC = new AdjQCDA();

            try
            {
                list = accQC.adjSetupQCItemInfo(adjID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
       
        public bool Update(PremiumAdjustmentStatusBE prmAdjSt)
        {
            bool succeed = false;
            if (prmAdjSt.PremumAdj_sts_ID > 0) //On Update
            {
                succeed = this.Save(prmAdjSt);
            }
            else //On Insert
            {
                prmAdjSt.PremumAdj_sts_ID = DA.GetNextPkID().Value;
                succeed = DA.Add(prmAdjSt);
            }
            return succeed;

        }
        
     
    }
}
