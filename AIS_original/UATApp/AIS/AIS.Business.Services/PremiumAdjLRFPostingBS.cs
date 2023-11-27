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
    public class PremiumAdjLRFPostingBS : BusinessServicesBase<PremiumAdjLRFPostingBE, PremiumAdjLRFPostingDA>
    {
        public PremiumAdjLRFPostingBE getPreAdjLRFRow(int PreAdjPgmID)
        {
            PremiumAdjLRFPostingBE PreAdjLRFBE = new PremiumAdjLRFPostingBE();
            PreAdjLRFBE = DA.Load(PreAdjPgmID);
            return PreAdjLRFBE;
        }
        public int getPreAdjLRFReserveID(int intPremAdjPerdID)
        {
            PremiumAdjLRFPostingDA PreAdjLRFDA = new PremiumAdjLRFPostingDA();
            return PreAdjLRFDA.getPreAdjLRFReserveID(intPremAdjPerdID);
        }
     public IList<PremiumAdjLRFPostingBE> getList()
     {
         IList<PremiumAdjLRFPostingBE> List = new List<PremiumAdjLRFPostingBE>();
         PremiumAdjLRFPostingDA palpDA = new PremiumAdjLRFPostingDA();
         try
         {
             List = palpDA.getList();
         }
         catch (Exception ex)
         {

             RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
             throw myException;
         }

         return List;
     }
     public IList<PremiumAdjLRFPostingBE> getLRFList(int acctID,int prmAdjID,int prmAdjPrdID)
     {
         IList<PremiumAdjLRFPostingBE> List = new List<PremiumAdjLRFPostingBE>();
         PremiumAdjLRFPostingDA palpDA = new PremiumAdjLRFPostingDA();
         try
         {
             List = palpDA.getLRFList(acctID, prmAdjID, prmAdjPrdID);
         }
         catch (Exception ex)
         {

             RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
             throw myException;
         }

         return List;
     }

     public DataTable getLRFData(int intPremAdjID,int intPremAdjPerdID,int intCustmrID)
     {
         PremiumAdjLRFPostingDA palpDA = new PremiumAdjLRFPostingDA();
         return palpDA.getLRFData(intPremAdjID, intPremAdjPerdID, intCustmrID);
     }
     public bool Update(PremiumAdjLRFPostingBE paLRFBE)
     {
         bool succeed = false;
         if (paLRFBE.Prem_Adj_Los_Reim_Fund_Post_ID > 0)
         {
             succeed = this.Save(paLRFBE);
         }
         else //On Insert
         {
             //papBE.PremiumAdjustmentProgramID = DA.GetNextPkID().Value;
             succeed = DA.Add(paLRFBE);
         }
         return succeed;
     }
    }
}
