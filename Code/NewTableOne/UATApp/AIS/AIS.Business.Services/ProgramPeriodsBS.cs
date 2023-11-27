using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;

using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Logic
{
    /// <summary>
    /// Serves for all busines functions for Program Period Master page
    /// </summary>
    public class ProgramPeriodsBS : BusinessServicesBase<ProgramPeriodBE, ProgramPeriodDA>
    {
        /// <summary>
        /// Retrieves the row from Prem_adj_Pgm Table
        /// </summary>
        /// <param name="PremAdjPgmID">PremAdjPgmID PrimaryKey</param>
        /// <returns>PersonBE based on PremAdjPgmID</returns>
        public ProgramPeriodBE getProgramPeriodRow(int PremAdjPgmID)
        {
            ProgramPeriodBE premAdj = new ProgramPeriodBE();
            premAdj = DA.Load(PremAdjPgmID);
            return premAdj;
        }

        /// <summary>
        /// Retrieve Active Program Period information(Program Period Business Entity) for a given account
        /// Used for Program Period User Control
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetProgramPeriods(int accountID)
        {

            IList<ProgramPeriodBE> programPrdBE;
            try
            {
                programPrdBE = new ProgramPeriodDA().GetProgramPeriodData(accountID);
                LookupBS lookup = new LookupBS();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (programPrdBE);

        }
        //Invoice Inquiry
        public IList<ProgramPeriodSearchListBE> GetValDate(int accountID)
        {

            IList<ProgramPeriodSearchListBE> programPrdBE;
            try
            {
                programPrdBE = new ProgramPeriodDA().GetValDate(accountID);

            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (programPrdBE);

        }

        /// <summary>
        /// To retrieve Program Period Data for initial Display AccountManagementDashBoard screen
        /// </summary>
        /// <param name="statusID"></param>
        /// <param name="personID"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetProgramPeriodData(int statusID, int personID)
        {
            IList<ProgramPeriodBE> lPrgData = (new ProgramPeriodDA()).GetProgramPeriodData(statusID, personID);
            return lPrgData;
        }

        /// <summary>
        /// To retrieve Program Period Data for the given search criteria
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="statusID"></param>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetProgramPeriodData(int accountID, int statusID, int personID, string startDate, string endDate)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            try
            {
                result = new ProgramPeriodDA().GetProgramPeriodData(accountID, statusID, personID, startDate, endDate);

            }
            catch (Exception ex)
            {
                RetroBaseException myException = new RetroBaseException(ex.Message, ex);
                throw myException;
            }
            return result;
        }


        /// <summary>
        /// To get All the Users
        /// </summary>
        /// <returns>List of Users</returns>
        public IList<PersonBE> getUserNames()
        {
            IList<PersonBE> perBE;
            try
            {
                perBE = new ProgramPeriodDA().getUserNames();
            }
            catch (System.Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            PersonBE selperBE = new PersonBE();
            selperBE.PERSON_ID = 0;
            selperBE.FULLNAME = "(Select)";
            perBE.Insert(0, selperBE);
            return perBE;
        }

        /// <summary>
        /// Retives all Program Periods
        /// </summary>
        /// <param name="AccountID">Account ID</param>
        /// <returns>List of Program Period Records</returns>
        public IList<ProgramPeriodListBE> GetActiveInActiveProgramPeriods(int accountID)
        {

            IList<ProgramPeriodListBE> programPrdBE;
            try
            {
                programPrdBE = new ProgramPeriodDA().GetActiveInActiveProgramPeriods(accountID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (programPrdBE);

        }

        public bool IsProgramPeriodExits(int progPerID, int brokerID, int buOffice, DateTime StartDate, DateTime EndDate, int custID, DateTime valDate, int progType)
        {
            bool Results;
            ProgramPeriodDA proPreDA = new ProgramPeriodDA();
            try
            {
                Results = proPreDA.IsProgramPeriodExits(progPerID, brokerID, buOffice, StartDate, EndDate, custID, valDate, progType);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return Results;
        }


        /// <summary>
        /// Retrieve all Program Period information(Program Period Business Entity) for a given account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<ProgramPeriodListBE> GetProgramPeriodList(int accountID)
        {

            IList<ProgramPeriodListBE> programPrdBE;
            try
            {
                programPrdBE = new ProgramPeriodDA().GetProgramPeriodList(accountID);
                LookupBS lookup = new LookupBS();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (programPrdBE);

        }
        /// <summary>
        /// This method is used to return the customerlist.
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="buID"></param>
        /// <param name="brokerID"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetCustomerList(int customerid, int buID, int brokerID)
        {
            IList<ProgramPeriodBE> ProgramPrdBE;
            try
            {
                ProgramPrdBE = new ProgramPeriodDA().GetCustomerDetails(customerid, buID, brokerID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (ProgramPrdBE);

        }
        /// <summary>
        /// This method is used to get hte account details based on the range search.
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="buOfficeID"></param>
        /// <param name="brokerID"></param>
        /// <param name="stChr"></param>
        /// <param name="edChr"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> getrangeprogramperiods(int buOfficeID, int brokerID, char stChr, char edChr)
        {
            IList<ProgramPeriodBE> ProgramPrdBE;
            try
            {

                ProgramPrdBE = new ProgramPeriodDA().getrangeprogramperiods(buOfficeID, brokerID, stChr, edChr);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (ProgramPrdBE);

        }
        /// <summary>
        /// Retrives a Program Period row from database using framework
        /// </summary>
        /// <param name="prgPeriodID">Program Period</param>
        /// <returns>Program Period row entity</returns>
        public ProgramPeriodBE getProgramPeriodInfo(int prgPeriodID)
        {
            ProgramPeriodBE ProgPeriod = new ProgramPeriodBE();
            try
            {
                ProgPeriod = DA.Load(prgPeriodID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return ProgPeriod;

        }
        /// <summary>
        /// Saves or Edits to database using FrameWork
        /// </summary>
        /// <param name="prgmperdBE">Program Period ID</param>
        /// <returns>true if save, False if failed to save</returns>
        public bool Update(ProgramPeriodBE prgmperdBE)
        {
            bool succeed = false;
            if (prgmperdBE.PREM_ADJ_PGM_ID > 0)
            {
                succeed = this.Save(prgmperdBE);
            }
            else
            {
                succeed = this.DA.Add(prgmperdBE);
            }
            return succeed;
        }
        //written By Venkat
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodSearch(string straccountID, string strValDate)
        {
            int intaccountID = 0;
            DateTime dtValDate = new DateTime();
            if (straccountID != "(Select)")
            {
                intaccountID = int.Parse(straccountID);
            }
            if (strValDate != null)
            {
                dtValDate = Convert.ToDateTime(strValDate);
            }
            IList<ProgramPeriodSearchListBE> ProgramPrdSearchBE;
            ProgramPrdSearchBE = new ProgramPeriodDA().GetProgramPeriodSearch(intaccountID, dtValDate);
            return (ProgramPrdSearchBE);

        }
        //written By Venkat for Adjsutment Review Search User control
        public IList<ProgramPeriodSearchListBE> GetARProgramPeriodSearch(string strPremAdjID, string straccountID)
        {
            int intPremAdjID = 0;
            int intaccountID = 0;
            if (strPremAdjID != "(Select)")
            {
                intPremAdjID = int.Parse(strPremAdjID);
            }
            if (straccountID != "(Select)")
            {
                intaccountID = int.Parse(straccountID);
            }
            IList<ProgramPeriodSearchListBE> ProgramPrdSearchBE;
            ProgramPrdSearchBE = new ProgramPeriodDA().GetARProgramPeriodSearch(intPremAdjID, intaccountID);
            for (int row = 0; row < ProgramPrdSearchBE.Count; row++)
            {
                ProgramPrdSearchBE[row].STARTDATE_ENDDATE_PGMTYP = ProgramPrdSearchBE[row].STARTDATE_ENDDATE + " " + ProgramPrdSearchBE[row].PGMTYP;

            }
            return (ProgramPrdSearchBE);

        }
        //written By Venkat
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodID(int intaccountID, DateTime dtValDate, DateTime dtStartDate, DateTime dtEndDate)
        {

            IList<ProgramPeriodSearchListBE> ProgramPrdSearchBE;
            ProgramPrdSearchBE = new ProgramPeriodDA().GetProgramPeriodID(intaccountID, dtValDate, dtStartDate, dtEndDate);
            return (ProgramPrdSearchBE);

        }
        //written By Venkat for lossinfo
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodsList(string strPremAdjPgmID)
        {
            int intPremAdjPgmID = 0;
            if (strPremAdjPgmID != "(Select)")
            {
                intPremAdjPgmID = int.Parse(strPremAdjPgmID);
            }
            IList<ProgramPeriodSearchListBE> ProgramPrdSearchBE;
            ProgramPrdSearchBE = new ProgramPeriodDA().GetProgramPeriods(intPremAdjPgmID);
            for (int row = 0; row < ProgramPrdSearchBE.Count; row++)
            {
                ProgramPrdSearchBE[row].STARTDATE_ENDDATE_PGMTYP = ProgramPrdSearchBE[row].STARTDATE_ENDDATE + " " + ProgramPrdSearchBE[row].PGMTYP;

            }
            return (ProgramPrdSearchBE);

        }
        //written By Venkat for lossinfo
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodsByPremAdjID(string strPremAdjID, DateTime dtValDate)
        {
            int intPremAdjID = 0;
            if (strPremAdjID != "(Select)")
            {
                intPremAdjID = int.Parse(strPremAdjID);
            }
            IList<ProgramPeriodSearchListBE> ProgramPrdSearchBE;
            ProgramPrdSearchBE = new ProgramPeriodDA().GetProgramPeriodsByPremAdjID(intPremAdjID, dtValDate);
            return (ProgramPrdSearchBE);

        }
        //written By Venkat for lossinfo
        //public IList<ProgramPeriodSearchListBE> GetProgramPeriodsByCustmrID(int intCustmrID)
        //{
        //    IList<ProgramPeriodSearchListBE> ProgramPrdSearchBE;
        //    ProgramPrdSearchBE = new ProgramPeriodDA().GetProgramPeriodsByCustmrID(intCustmrID);
        //    return (ProgramPrdSearchBE);

        //}
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodsByCustmrID(int intCustmrID)
        {
            IList<ProgramPeriodSearchListBE> ProgramPrdSearchBE;
            ProgramPrdSearchBE = new ProgramPeriodDA().GetProgramPeriodsByCustmrID(intCustmrID);
            for (int row = 0; row < ProgramPrdSearchBE.Count; row++)
            {
                ProgramPrdSearchBE[row].STARTDATE_ENDDATE_PGMTYP = ProgramPrdSearchBE[row].STARTDATE_ENDDATE + " " + ProgramPrdSearchBE[row].PGMTYP;

            }
            return (ProgramPrdSearchBE);

        }

        public IList<ProgramPeriodBE> GetProgramPeriodSearchDashboard(int AccNo, int PrgTypID, int adjNo, string valDate)
        {
            ProgramPeriodDA prgPRD = new ProgramPeriodDA();
            IList<ProgramPeriodBE> ProgramPrdSearchBE = new List<ProgramPeriodBE>();
            ProgramPrdSearchBE = prgPRD.GetProgramPeriodSearchDashboard(AccNo, PrgTypID, adjNo, valDate);
            return ProgramPrdSearchBE;

        }
        public IList<ProgramPeriodBE> GetProgramPeriodSearchDashboard(int AccNo, int PrgTypID)
        {
            ProgramPeriodDA prgPRD = new ProgramPeriodDA();
            IList<ProgramPeriodBE> ProgramPrdSearchBE = new List<ProgramPeriodBE>();
            ProgramPrdSearchBE = prgPRD.GetProgramPeriodSearchDashboard(AccNo);
            if (PrgTypID != 0)
            {
                ProgramPrdSearchBE = ProgramPrdSearchBE.Where(pg => pg.PGM_TYP_ID == PrgTypID).ToList();
            }
            return ProgramPrdSearchBE;

        }
        public string CalcDriver(int CustomerID, string strPRGIDs, string strPERDIDs, bool Flag, bool ILRFlag, int UserID)
        {
            ProgramPeriodDA prgPRD = new ProgramPeriodDA();
            return prgPRD.CalcDriver(CustomerID, strPRGIDs, strPERDIDs, Flag, ILRFlag, UserID);
        }
        public string AdjustmentRevision(int PremAdjID, int? Custmr_id, int? PersonID)
        {
            ProgramPeriodDA prgPRD = new ProgramPeriodDA();
            return prgPRD.AdjustmentRevision(PremAdjID, Custmr_id, PersonID);
        }
        public string AdjustmentCancel(int PremAdjID)
        {
            try
            {
                ProgramPeriodDA prgPRD = new ProgramPeriodDA();
                return prgPRD.AdjustmentCancel(PremAdjID);
            }
            catch (Exception ex)
            {
                RetroBaseException myException = new RetroBaseException(ex.Message, ex);
                throw myException;
            }
        }

        public string AdjustmentVoid(int PremAdjID, int? Custmr_id, int? PersonID)
        {
            ProgramPeriodDA prgPRD = new ProgramPeriodDA();
            return prgPRD.AdjustmentVoid(PremAdjID, Custmr_id, PersonID);
        }
        public IList<ProgramPeriodBE> GetRoles(int intCustmrID, int intPersonID)
        {
            ProgramPeriodDA prgPRD = new ProgramPeriodDA();
            IList<ProgramPeriodBE> ProgramPrdBE = new List<ProgramPeriodBE>();
            ProgramPrdBE = prgPRD.getRoles(intCustmrID, intPersonID);

            return ProgramPrdBE;

        }
        /// <summary>
        /// Function to check First ADjustment or Not.
        /// If it returns zero then First Adjustment.
        /// </summary>
        /// <param name="PremAdjPGMID"></param>
        /// <returns></returns>
        public int? CheckFirstAdjustment(int PremAdjPGMID)
        {
            ProgramPeriodDA prgPRD = new ProgramPeriodDA();
            return prgPRD.CheckFirstAdjustment(PremAdjPGMID);
        }
        //Method to call when Adjustment is Finalised to Post to ARies
        public void ModAISTransmittalToARiES(int intPremAdjID, int? IND)
        {
            try
            {
                (new ProgramPeriodDA()).ModAISTransmittalToARiES(intPremAdjID, IND);

            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
        }

    }
}
