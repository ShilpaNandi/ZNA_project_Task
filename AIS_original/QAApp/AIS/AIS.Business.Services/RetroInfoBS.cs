using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;

namespace ZurichNA.AIS.Business.Logic
{
    public class RetroInfoBS : BusinessServicesBase<RetroInfoBE, RetroInfoDA>
    {
        /// <summary>
        /// Retrieve Retro information for the given Program Period
        /// </summary>
        /// <param name = "programPeriodID"></param>
        /// <returns></returns>
        public IList<RetroInfoBE> GetRetroInfo(int programPeriodID)
        {
            IList<LookupBE> lkpElemList = new List<LookupBE>();
            BLAccess blAccess = new BLAccess();
            IList<RetroInfoBE> info = new List<RetroInfoBE>();
            RetroInfoDA retroDA = new RetroInfoDA();
            try
            {
                info = retroDA.GetRetroInfo(programPeriodID);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return null;
            }
            /*foreach (RetroInfoBE item in info)
            {
                int? retroElemTypeId = item.RETRO_ELEMT_TYP_ID;
                lkpElemList = blAccess.GetLookUpActiveData("RETROSPECTIVE ELEMENT TYPE");

                foreach (LookupBE lkupitem in lkpElemList)
                {
                    if (lkupitem.LookUpID == retroElemTypeId)
                    {
                        item.RETRO_ELEMT = lkupitem.LookUpName;
                    }
                }
                
            }*/
            
            return info;
        }
        /// <summary>
        /// Method to get Max And Min Amount Values in PremAdjCombElemnts Screen
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <returns>IList<RetroInfoBE></returns>
        public IList<RetroInfoBE> getCombElemntsMaxAndMinAmounts(int intProgramPeriodID, int intCustmorID, int intRetroElemtTypID)
        {
            IList<RetroInfoBE> info = new List<RetroInfoBE>();
            RetroInfoDA retroDA = new RetroInfoDA();
            try
            {
                info = retroDA.getCombElemntsMaxAndMinAmounts(intProgramPeriodID, intCustmorID, intRetroElemtTypID);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return info;
        }
        /// <summary>
        /// Retrieves AuditExposure amount for  Standard Premium ExposureType
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <returns></returns>

        public decimal GetStandardAuditExp(int programPeriodID,int custmorId)
        {
            IList<RetroInfoBE> info = new List<RetroInfoBE>();
            AuditExpDA expDA = new AuditExpDA();
            decimal totAud = 0.0m;
            try
            {
                info = expDA.GetStandardAuditExp(programPeriodID, custmorId);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            foreach (RetroInfoBE item in info)
            {
                if (item.PREM_ADJ_PGM_ID == programPeriodID)
                totAud = Convert.ToDecimal(item.TOT_SUBJ_AUDT_PREM_AMT);
            }
            
            return totAud;
        }
        /// <summary>
        /// Retrieves AuditExposure amount for Payroll ExposureType 
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <returns></returns>

        public decimal GetPayrollAuditExp(int programPeriodID, int custmorId)
        {
            IList<RetroInfoBE> info = new List<RetroInfoBE>();
            AuditExpDA expDA = new AuditExpDA();
            decimal totAud = 0.0m;
            try
            {
                info = expDA.GetPayrollAuditExp(programPeriodID,custmorId);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            foreach (RetroInfoBE item in info)
            {
                if (item.PREM_ADJ_PGM_ID == programPeriodID)
                    totAud = Convert.ToDecimal(item.TOT_SUBJ_AUDT_PREM_AMT);
            }
            
            return totAud;
        }
        /// <summary>
        /// Retrieves AuditExposure amount for “Combined Aggregate ExposureType 
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <returns></returns>
        public decimal GetCombinedAuditExp(int programPeriodID, int custmorId)
        {
            IList<RetroInfoBE> info = new List<RetroInfoBE>();
            AuditExpDA expDA = new AuditExpDA();
            decimal totAud = 0.0m;
            try
            {
                info = expDA.GetCombinedAuditExp(programPeriodID,custmorId);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            foreach (RetroInfoBE item in info)
            {
                if (item.PREM_ADJ_PGM_ID == programPeriodID)
                    totAud = Convert.ToDecimal(item.TOT_SUBJ_AUDT_PREM_AMT);
            }
            
            return totAud;
        }

        /// <summary>
        /// Retrieves AuditExposure amount for Exposure Type other than Standard Premium, Payroll or Manual Premium,
        /// </summary>
        /// <returns></returns>      

        public Decimal GetOtherAuditExp(int programPeriodID, int custmorId)
        {
            IList<RetroInfoBE> info = new List<RetroInfoBE>();
            AuditExpDA expDA = new AuditExpDA();
            Decimal totAud = 0;
            try
            {
                info = expDA.GetOtherAuditExp(programPeriodID,custmorId);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            foreach (RetroInfoBE item in info)
            {
                totAud = totAud  + Convert.ToDecimal(item.TOT_SUBJ_AUDT_PREM_AMT);
            }
            
            return totAud;
        }

        /// <summary>
        /// Retrieves particular Retro Information record
        /// </summary>
        /// <param name="adjRetroInfoId"></param>
        /// <returns></returns>

        public RetroInfoBE LoadData(int adjRetroInfoId)
        {
            RetroInfoBE data = new RetroInfoBE();
            try
            {
                data = DA.Load(adjRetroInfoId);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return data;
        }

        /// <summary>
        /// Saves or Edits to database using FrameWork
        /// </summary>
        /// <param name="RetroInfoBE"></param>
        /// <returns>true if save, False if failed to save</returns>

        public bool SaveRetroData(RetroInfoBE retroInfoBE)
        {
            try
            {
                if (retroInfoBE.ADJ_RETRO_INFO_ID > 0)
                {
                    DA.Update(retroInfoBE);
                }
                else
                {
                    DA.Add(retroInfoBE);
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        /// <summary>
        /// Retriving RetroInfo records. Used for ProgramPeriod Copy Dependies
        /// This will not retrive records Using Lookup, if there are no records in RetroInfo table. unlike "GetRetroInfo" method
        /// Sreedhar 4th Nov 2008
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <returns></returns>
        public IList<RetroInfoBE> getRetroInfoForCopy(int programPeriodID)
        {
            IList<RetroInfoBE> info = new List<RetroInfoBE>();
            RetroInfoDA retroDA = new RetroInfoDA();
            try
            {
                info = retroDA.getRetroInfoForCopy(programPeriodID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return info;
        }


    }
}
