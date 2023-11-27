using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Logic
{
    public class PremiumAdjustmentProgramStatusBS : BusinessServicesBase<PremiumAdjustmentProgramStatusBE, PremiumAdjustmentProgramStatusDA>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="premAdjProgStsID"></param>
        /// <returns></returns>
        public PremiumAdjustmentProgramStatusBE getPremAdjProgSt(int premAdjProgStsID)
        {
            PremiumAdjustmentProgramStatusBE PremAdjProgStsBE = new PremiumAdjustmentProgramStatusBE();
            try
            {
                PremAdjProgStsBE = DA.Load(premAdjProgStsID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return PremAdjProgStsBE;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PremAdjProgStsBE"></param>
        /// <returns></returns>
        public bool Update(PremiumAdjustmentProgramStatusBE premAdjProgStsBE)
        {
            bool succeed = false;
            if (premAdjProgStsBE.PREMUMADJPROG_STS_ID > 0)
            {
                succeed = this.DA.Update(premAdjProgStsBE);
            }
            else
            {
                succeed = this.DA.Add(premAdjProgStsBE);
            }
            return succeed;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="premAdjProgStsBE"></param>
        /// <returns></returns>
        public bool Delete(PremiumAdjustmentProgramStatusBE premAdjProgStsBE)
        {
            bool succeed = false;
            try
            {
                succeed = this.DA.Delete(premAdjProgStsBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return succeed;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="progPerID"></param>
        /// <returns></returns>
        public IList<PremiumAdjustmentProgramStatusBE> GetProgramStatusList(int accountID, int progPerID)
        {
            IList<PremiumAdjustmentProgramStatusBE> PremAdjProgStsBEs;
            try
            {
                PremAdjProgStsBEs = new PremiumAdjustmentProgramStatusDA().GetProgramStatusList(accountID, progPerID);                
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (PremAdjProgStsBEs);
        }

    }
}
