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
    public class LSICustomersBS : BusinessServicesBase<LSICustomerBE, LSICustomersDA>
    {
        private LSICustomersDA LSICustomersDA;

        public LSICustomersBS()
        {
            LSICustomersDA = new LSICustomersDA(); // Instantiate DAL object.
        }

        /// <summary>
        /// Retrieves all active LSI accounts from the "LSI" Database.
        /// Anil 08/20/2008
        /// </summary>
        /// <returns></returns>
        public IList<LSIAllCustomersBE> getLSIAccounts()
        {
            return LSICustomersDA.getLSIAccountData();
        }

        /// <summary>
        /// Retrieves all active LSI accounts from the "LSI" Database.
        /// Anil 08/20/2008
        /// </summary>
        /// <returns></returns>
        public IList<LSIAllCustomersBE> getLSIAccounts(string[] query)
        {
            return LSICustomersDA.getLSIAccountData(query);
        }

        /// <summary>
        /// Retrieve related LSI accounts corresponding 
        /// to the selected Retro account...
        /// Anil 07/28/2008
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<LSICustomerBE> getRelatedLSIAccounts(int AccountID)
        {
            IList<LSICustomerBE> list = new List<LSICustomerBE>();
            LSICustomersDA LSIaccount = new LSICustomersDA();

           
                list = LSIaccount.getRelatedLSIAccountData(AccountID);
            
           
            return list;
        }


        
        /// <summary>
        /// Retrieve a single LSI account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public LSICustomerBE getLSIAccount(int LSIAccountID)
        {
            LSICustomerBE LSIacct = new LSICustomerBE();
            LSIacct = DA.Load(LSIAccountID);
            return LSIacct;
        }

        /// <summary>
        /// Method to update related LSI accounts. 
        /// to set up the relationship with the selected retro account
        /// </summary>
        /// <param name="accountBE"></param>
        /// <returns></returns>
        public bool Update(LSICustomerBE lsiCustomerBE)
        {
            bool succeed = false;
            if (lsiCustomerBE.LSI_CUSTMR_ID > 0)//on Update
            {
                succeed = this.Save(lsiCustomerBE);
            }
            else //On Insert
            {
                lsiCustomerBE.LSI_CUSTMR_ID = DA.GetNextPkID().Value;
                succeed = DA.Add(lsiCustomerBE);
            }
    
            return succeed;
        }

        public bool CheckDuplicateLSIAccountName(int LSIAccountID, int AccountID)
        {
            LSICustomersDA LSIaccount = new LSICustomersDA();
            return LSIaccount.CheckDuplicateLSIAccountName(LSIAccountID, AccountID);
        }

        public bool getDuplLSIPrimaryAccount(bool isActive, bool Primselected, int AccountID)
        {
            LSICustomersDA LSIaccount = new LSICustomersDA();
            return LSIaccount.getDuplLSIPrimaryAccount(isActive, Primselected, AccountID);
        }
        /// <summary>
        /// This method is written to verify Permissions on LSI Database for AIS user
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckLSIPermissions()
        {
            LSICustomersDA LSICustomerDA = new LSICustomersDA();
            return LSICustomerDA.CheckLSIPermissions();
        }

    }
}
