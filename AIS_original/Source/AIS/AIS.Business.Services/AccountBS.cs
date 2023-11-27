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
    //public class AccountBS : BaseBS<AccountBE, AccountDA>

    public class AccountBS : BusinessServicesBase<AccountBE, AccountDA>
    {

        public AccountBS()
        {

        }

        /// <summary>
        /// Retrieves all active accounts
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getAccounts()
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getAccountData();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            foreach (AccountBE acct in list)
            {
                CustomerContactBE custContBE = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID);
                if (custContBE.PERSON_ID != null)
                    acct.PERSON_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PERSON_ID.Value;                
            }
            return list;
        }
        /// <summary>
        /// Retrieves all active accounts
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getAccounts(string[] query)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getAccountData(query);

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return list;
        }

        /// <summary>
        /// Retrieves all active accounts and accounts that are master
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getMasterAccounts(string[] query)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getMasterAccounts(query);

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }

        /// <summary>
        /// Retrieves all active Master And Regular Accounts
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getMasterandRegularAccounts(string[] query)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getMasterandRegularAccounts(query);

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
       
        /// <summary>
        /// Retrieves all active accounts and accounts that are non-master
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getNonMasterAccounts()
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getNonMasterAccounts();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }

        /// <summary>
        /// Retrieves all active accounts and accounts that are non-master
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getNonMasterAccounts(string [] query)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getNonMasterAccounts(query);
             }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        /// <summary>
        /// Retrieves all active accounts and accounts that are master
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getMasterAccounts()
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();
            AccountBE ActBE = new AccountBE();

            try
            {
                list = account.getMasterAccounts();
                ActBE.FULL_NM = "(Select)";
                ActBE.CUSTMR_ID = 0;
                list.Insert(0, ActBE);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        /// <summary>
        /// returns Master Account along with child accounts
        /// used in QC Pages
        /// </summary>
        /// <param name="MasterAccountID"></param>
        /// <returns></returns>
        public IList<AccountBE> getMasterWithChildAccounts(int MasterAccountID)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getMasterWithChildAccounts(MasterAccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }

        /// <summary>
        /// returns Master Account along with child accounts
        /// used in QC Pages
        /// </summary>
        /// <param name="MasterAccountID"></param>
        /// <returns></returns>
        public IList<AccountBE> getMasterWithChildAccounts(int MasterAccountID, string [] query)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getMasterWithChildAccounts(MasterAccountID, query);

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        public bool CheckDuplicateAccountName(string Name)
        {
            AccountDA account = new AccountDA();
            return account.CheckDuplicateAccountName(Name);
        }

        // returns the sum of the two numbers. This is a test function to test unit testing functionality. Plese remove. Anil
        public int DivTest(int a , int b ) 
        {
            return (a / b);
        }


        /// <summary>
        /// Retrieve a single account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public AccountBE getAccount(int AccountID)
        {
            AccountBE acct = new AccountBE();
            acct = DA.Load(AccountID);            
            CustomerContactBE custContBE = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID);
            if (custContBE.PERSON_ID != null) 
                acct.PERSON_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PERSON_ID.Value;

            return acct;
        }

        /// <summary>
        /// Retrieve related accounts corresponding to a master account...Anil 07/28/08
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<AccountBE> getRelatedAccounts(int AccountID)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getRelatedAccountData (AccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }

        /// <summary>
        /// Retrieve related accounts corresponding to a master account...
        /// Anil 09/02/2008
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        
        public IList<AccountBE> getRelatedAccountsRO(int AccountID, int MasterAccountID)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getRelatedAccountDataRO(AccountID, MasterAccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }

        /// <summary>
        /// Method to update related RETRo accounts. 
        /// Custmr_ID is updated against the Custmr_rel_id field
        /// to set up the relationship
        /// Anil 08/20/08
        /// </summary>
        /// <param name="accountBE"></param>
        /// <returns></returns>
        public bool Update(AccountBE accountBE)
        {
            bool succeed = false;
            if (accountBE.CUSTMR_ID > 0)
            {
                succeed = this.Save(accountBE);
            }
            return succeed;
        }


        /// <summary>
        /// Method to update related accounts. 
        /// Custmr_ID is updated against the Custmr_rel_id field
        /// to set up the relationship
        /// /// Anil 08/12/08
        /// </summary>
        /// <param name="accountBE"></param>
        /// <returns></returns>
        public bool Update(AccountBE accountBE, CustomerContactBE custmrContactBE,int personID)
        {
            bool succeed = false;
            CustomerContactBE tempCustmrCtc = null;
            if (accountBE.CUSTMR_ID > 0) //On Update
            {
                succeed = this.Save(accountBE);
                if (succeed && (custmrContactBE.PERSON_ID > 0) && personID != custmrContactBE.PERSON_ID)
                {
                    //Should run only on update
                    CustomerContactBS custmrcontactBS = new CustomerContactBS();
                    int roleID = Convert.ToInt32(custmrContactBE.ROLE_ID);
                    custmrContactBE = custmrcontactBS.getInsuredContactData(
                                        custmrContactBE.CUSTOMER_ID, custmrContactBE.PERSON_ID.Value);
                    custmrContactBE.ROLE_ID = roleID;
                    succeed = custmrcontactBS.Save(custmrContactBE);
                    if (succeed && personID > 0)
                    {
                        tempCustmrCtc = custmrcontactBS.getInsuredContactData(custmrContactBE.CUSTOMER_ID, personID);
                        /// This is now a nullable field, Anil 09/08/2008
                        tempCustmrCtc.ROLE_ID = (Nullable<int>)null;
                        succeed = custmrcontactBS.Save(tempCustmrCtc);
                    }
                }
            }
            else //On Insert
            {
                accountBE.CUSTMR_ID = DA.GetNextPkID().Value;
                succeed = DA.Add(accountBE);
            }
            
            return succeed;

        }

        public IList<AccountBE> getAccountNames(int perId)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();
            list = account.getAccountNames(perId);
            AccountBE selAccountBE = new AccountBE();
            selAccountBE.CUSTMR_ID = 0;
            selAccountBE.FULL_NM= "(Select)";
            list.Insert(0, selAccountBE);
            return list;
        }
        /// <summary>
        ///  Retrieve a list of account records
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getAccounts(string AccountName, string BPNumber, int AccountNumber, string SSCGID)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();
            try
            {
                list = account.getAccountData(AccountName, BPNumber, AccountNumber, SSCGID);

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
        /// <summary>
        /// Checks wether PEO Indicater is set for a given account. ProgramPeriod Screen purpose
        /// Sreedhar 16th Oct 2008
        /// </summary>
        /// <param name="accountID">AccountID</param>
        /// <returns>retuns true if PEO is set, False if not set</returns>
        public bool isAccountSetUPasPEO(int accountID)
        {
            AccountDA account = new AccountDA();
            bool isPEO = false;
            try
            {
                isPEO = account.isAccountSetUPasPEO(accountID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return isPEO;
        }
        /// <summary>
        /// Retrieves all active accounts
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getAccountsInfo(int intAccNo)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getAccountInfo(intAccNo);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            
            return list;
        }

        /// <summary>
        /// Sets parent_id null for child accounts
        /// </summary>
        /// <param name="accountID">Parent Account ID</param>
        public void SetZeroChildAccounts(int accountID)
        {
            (new AccountDA()).SetZeroChildAccounts(accountID);
        }

        /// <summary>
        /// Retrieves all active accounts
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getBPNumberDetails(string bpnumber)
        {
            IList<AccountBE> list = new List<AccountBE>();
            AccountDA account = new AccountDA();

            try
            {
                list = account.getBPNumberDetails(bpnumber);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return list;
        }


        /// <summary>
        /// Update transmittal for test account
        /// </summary>
        /// <param name=""></param>
        public void UpdateTransmittalHistoryForTestAcct()
        {
            (new AccountDA()).UpdateTransmittalHistoryForTestAcct();
        }

        //Check Account belongs to Canada or Not (Added as part of AIS C2Z Project)
        public bool IsCanadaAccount(int intAdjNo)
        {
            return (new AccountDA()).IsCanadaAccount(intAdjNo);
        }

        //Check Account belongs to Canada or Not (Added as part of AIS C2Z Project)
        public bool IsCanadaAccountbyZDWKey(string zdwKey)
        {
            return (new AccountDA()).IsCanadaAccountbyZDWKey(zdwKey);
        }

        //Check Account belongs to Canada or Not by Invocie Number(Added as part of AIS C2Z Project)
        public bool IsCanadaAccount(string invNumber)
        {
            return (new AccountDA()).IsCanadaAccount(invNumber);
        }

    }

    public class AccountSearchBS : BusinessServicesBase<AccountSearchBE, AccountSearchDA>
    {
        /// <summary>
        /// Retrieve a single account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<AccountSearchBE> getAccounts(int AccountID)
        {
            IList<AccountSearchBE> list = new List<AccountSearchBE>();
            AccountSearchDA account = new AccountSearchDA();

            try
            {
                list = account.getAccountData(AccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }


        /// <summary>
        ///  Retrieve a list of account records
        /// </summary>
        /// <returns></returns>
        public IList<AccountSearchBE> getAccounts(string AccountName, string BPNumber, int AccountNumber, string SSCGID,string polNo)
        {
            IList<AccountSearchBE> list = new List<AccountSearchBE>();
            AccountSearchDA account = new AccountSearchDA();
            try
            {
                list = account.getAccountData(AccountName, BPNumber, AccountNumber, SSCGID, polNo);

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
        ///Invoice Inquiry
        public IList<AccountSearchBE> getRelatedAccountNo(int AccountID)
        {
            IList<AccountSearchBE> list = new List<AccountSearchBE>();

            try
            {
                list = new AccountSearchDA().getRelatedAccountNo(AccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        } 
    }

}
