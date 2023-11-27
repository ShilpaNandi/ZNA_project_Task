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
    public class PremAdjustmentBS : BusinessServicesBase<PremiumAdjustmentBE, PremAdjustmentDA>
    {

        public IList<PremiumAdjustmentBE> getpremAdjList()
        {
            IList<PremiumAdjustmentBE> list = new List<PremiumAdjustmentBE>();
            PremAdjustmentDA prem = new PremAdjustmentDA();

            try
            {
                list = prem.getList();
            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.PERSON_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PERSON_ID;
            return list;
        }
      
        /// <summary>
        /// To retrieve the Adjustment Info for the AccountManagementDashboard initially
        /// </summary>
        /// <param name="personID"></param>
        /// <returns>Returns the list of Adjustment Information for the given search criteria</returns>

        public IList<PremiumAdjustmentBE> GetAdjustmentInfo(int personID)
        {
            return (new PremAdjustmentDA()).GetAdjustmentInfo(personID);
        }

        /// <summary>
        /// To retrieve the Adjustment Info for AccountManagementDashboard
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="statusID"></param>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Returns the list of Adjustment Information for the given search criteria</returns>
        public IList<PremiumAdjustmentBE> GetAdjustmentInfo(int accountID, int statusID, int personID, string pending)
        {
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();
            PremAdjustmentDA prem = new PremAdjustmentDA();
            try
            {
                result = prem.GetAdjustmentInfo(accountID, statusID, personID, pending);
            }
            catch (Exception ex)
            {
                RetroBaseException myException = new RetroBaseException(ex.Message, ex);
                throw myException;
            }
            return result;
        }


        /// <summary>
        /// To retrieve the Invoice Info for the AccountManagementDashboard
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="statusID"></param>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Returns the list of Invoice Information for the given search criteria</returns>
        public IList<PremiumAdjustmentBE> GetInvoiceInfo(int accountID, int personID, string qcComplete, string ariesClrng,bool historical)
        {
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();
            PremAdjustmentDA prem = new PremAdjustmentDA();
            try
            {
                result = prem.GetInvoiceInfo(accountID, personID, qcComplete, ariesClrng, historical);
            }
            catch (Exception ex)
            {
                RetroBaseException myException = new RetroBaseException(ex.Message, ex);
                throw myException;
            }
            return result;
        }
        //written By Venkat
        public IList<PremiumAdjustmetSearchBE> GetValDateSearch(string straccountID)
        {
            int intaccountID = 0;
            if (straccountID != "(Select)")
            {
                intaccountID = int.Parse(straccountID);
            }

            IList<PremiumAdjustmetSearchBE> PremAdjSearchBE;
            PremAdjSearchBE = new PremAdjustmentDA().GetValDateSearch(intaccountID);
            PremiumAdjustmetSearchBE pasBE = new PremiumAdjustmetSearchBE();
            pasBE.VALUATIONDATE = "(Select)";
            PremAdjSearchBE.Insert(0, pasBE);
            return (PremAdjSearchBE);

        }

        //written By Venkat
        public IList<PremiumAdjustmetSearchBE> GetAdjNumberSearch(string straccountID,int intPremAdjPgmID, string strValDate)
        {
            int intaccountID = 0;
            if (straccountID != "(Select)")
            {
                intaccountID = int.Parse(straccountID);
            }

            DateTime dtValDate = DateTime.Parse(strValDate);

            IList<PremiumAdjustmetSearchBE> PremAdjSearchBE;
            PremAdjSearchBE = new PremAdjustmentDA().GetAdjNumberSearch(intaccountID,intPremAdjPgmID, dtValDate);
            return (PremAdjSearchBE);

        }

        //written By Venkat for adjustment Review Search
        public IList<PremiumAdjustmetSearchBE> GetARAdjNumberSearch(string straccountID, string strValDate)
        {
            int intaccountID = 0;
            if (straccountID != "(Select)")
            {
                intaccountID = Convert.ToInt32(straccountID);
            }

            DateTime dtValDate = DateTime.Parse(strValDate);

            IList<PremiumAdjustmetSearchBE> PremAdjSearchBE;
            PremAdjSearchBE = new PremAdjustmentDA().GetARAdjNumberSearch(intaccountID, dtValDate);
           
            return (PremAdjSearchBE);

        }


        //written By Venkat for Invoice E-Mail Notification
        public IList<PremiumAdjustmentBE> GetEMailInfo(int intAdjNo)
        {
            IList<PremiumAdjustmentBE> PremAdjSearchBE;
            PremAdjSearchBE = new PremAdjustmentDA().GetEMailInfo(intAdjNo);
            return (PremAdjSearchBE);

        }
        /// <summary>
        /// update Prem Adj-AK-For AdjustmentReview Comments Screen
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(PremiumAdjustmentBE prmadjBE)
        {
            bool succeed = false;
            try
            {
                if (prmadjBE.PREMIUM_ADJ_ID > 0) //On Update
                {
                    succeed = this.Save(prmadjBE);

                }
                else //On Insert
                {

                    succeed = DA.Add(prmadjBE);
                }
              
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;

        }
        /// <summary>
        /// Retrieves PremiumAdjustment -AK-For AdjustmentReview Comments Screen
        /// </summary>
        /// <returns>List of PremiumAdjustmentBE</returns>
        public PremiumAdjustmentBE getPremiumAdjustmentRow(int ID)
        {
            PremiumAdjustmentBE PremiumAdjustmentBE = new PremiumAdjustmentBE();
            PremiumAdjustmentBE = DA.Load(ID);
            return PremiumAdjustmentBE;
        }
        /// <summary>
        /// Get the Voided Prem Adj ID
        /// </summary>
        /// <param name="intPremAdjID"></param>
        /// <returns></returns>
        public PremiumAdjustmentBE getVoidedAdjustmentRow(int intPremAdjID)
        {
            PremiumAdjustmentBE result = new PremiumAdjustmentBE();
            result = new PremAdjustmentDA().getVoidedAdjustmentRow(intPremAdjID);
            return result;

        }
        public IList<PremiumAdjustmentBE> GetPremAdjSearch(int intaccountID)
        {
            IList<PremiumAdjustmentBE> PremAdjBE;
            PremAdjBE = new PremAdjustmentDA().GetAdjNumberSearch(intaccountID);
            return (PremAdjBE);

        }
        public IList<PremiumAdjustmentBE> GetPremAdjSearchDates(int intaccountID)
        {
            IList<PremiumAdjustmentBE> PremAdjBE;
            PremAdjBE = new PremAdjustmentDA().GetAdjNumberSearchDates(intaccountID);
            return (PremAdjBE);

        }
        //Used in Invoice Driver
        public string getZDWKey(int intAdjNo, char cKeyType, int IFlag)
        {
            PremAdjustmentDA objPremAdjDA=new PremAdjustmentDA();
            return objPremAdjDA.getZDWKey(intAdjNo, cKeyType, IFlag);
        }
        public IList<string> getEmailIDS(string strAdjStage, bool AdjAction, int CustmrID)
        {
            PremAdjustmentDA objPremAdjDA = new PremAdjustmentDA();
            return objPremAdjDA.getEmailIDS(strAdjStage, AdjAction, CustmrID);
        }
       
    }

}

