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
    public class PremAdjCommentBS : BusinessServicesBase<PremiumAdjustmetCommentBE, PremAdjCommentDA>
    {
        public bool Update(PremiumAdjustmetCommentBE PerAdjCmtBE)
        {
            bool succeed = false;
            if (PerAdjCmtBE.PremumAdj_Commnt_ID > 0) //On Update
            {
                succeed = this.Save(PerAdjCmtBE);
            }
            else //On Insert
            {
                PerAdjCmtBE.PremumAdj_Commnt_ID = this.DA.GetNextPkID().Value;
                succeed = DA.Add(PerAdjCmtBE);
            }
            return succeed;
        }
        public PremiumAdjustmetCommentBE getPreAdjCmtRow(int PreAdjcmtID)
        {
            PremiumAdjustmetCommentBE PreAdjCmtBE = new PremiumAdjustmetCommentBE();
            PreAdjCmtBE = DA.Load(PreAdjcmtID);
            return PreAdjCmtBE;
        }
    }

}

