using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// DataAccessor for Account details
    /// </summary>
    public class AccountDA : DataAccessor<CUSTMR, AccountBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Returns all accounts that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getAccountData()
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all accounts that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getAccountData(string[] RangeSearch)
        {

            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();
            var predicate = PredicateBuilder.False<AccountBE>();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 ACTV_IND = cdd.actv_ind,
                 FINC_PTY_ID = cdd.finc_pty_id
             });

            query = query.Where(cdd => cdd.ACTV_IND == true);

            if (RangeSearch != null)
            {
                foreach (string searchTerm in RangeSearch)
                {
                    string temp = searchTerm;
                    predicate = predicate.Or(p => p.FULL_NM.StartsWith(temp));
                }
                query = query.Where(predicate);
            }
            
            query = query.OrderBy(cdd => cdd.FULL_NM);

            result = query.ToList();
            return result;
        }


        /// <summary>
        /// Used to retrive the Account Names 
        /// </summary>
        /// <param name="perId"></param>
        /// <returns>Account Names</returns>
        public IList<AccountBE> getAccountNames(int perId)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             join cpr in this.Context.CUSTMR_PERS_RELs
             on cdd.custmr_id equals cpr.custmr_id
             where cpr.pers_id == perId
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id
             }).Distinct();
            query = query.OrderBy(cdd => cdd.FULL_NM);
            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all accounts that match criteria and accounts that are non-master
        /// Anil 07/29/2008
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getNonMasterAccounts()
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             //where cdd.actv_ind  == true  
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 ACTV_IND = cdd.actv_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id
             });

            query = query.Where(cdd => cdd.MSTR_ACCT_IND != true && cdd.ACTV_IND == true && cdd.CUSTMR_REL_ID == null);

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all accounts that match criteria and accounts that are non-master
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getNonMasterAccounts(string[] RangeSearch)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();
            var predicate = PredicateBuilder.False<AccountBE>();


            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             //where cdd.actv_ind  == true  
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 ACTV_IND = cdd.actv_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id
             });

            query = query.Where(cdd => (cdd.MSTR_ACCT_IND != true || cdd.MSTR_ACCT_IND==null) && (cdd.ACTV_IND == true) && (cdd.CUSTMR_REL_ID == null));

            foreach (string searchTerm in RangeSearch)
            {
                string temp = searchTerm;
                predicate = predicate.Or(p => p.FULL_NM.StartsWith(temp));
            }
            query = query.Where(predicate);
            query = query.OrderBy(cdd => cdd.FULL_NM);
            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }


        /// <summary>
        /// Returns all accounts that match criteria and accounts that are non-master
        /// Anil 07/29/2008
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getMasterAccounts()
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             //where cdd.actv_ind  == true  
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 ACTV_IND = cdd.actv_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id
             });

            query = query.Where(cdd => cdd.CUSTMR_REL_ID == null && cdd.ACTV_IND == true);

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all accounts that match criteria and accounts that are non-master
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getMasterAccounts(string[] RangeSearch)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();
            var predicate = PredicateBuilder.False<AccountBE>();


            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             //where cdd.actv_ind  == true  
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 ACTV_IND = cdd.actv_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id
             });

            query = query.Where(cdd => cdd.CUSTMR_REL_ID == null && cdd.ACTV_IND == true);
            if (RangeSearch != null)
            {
                foreach (string searchTerm in RangeSearch)
                {
                    string temp = searchTerm;
                    predicate = predicate.Or(p => p.FULL_NM.StartsWith(temp));
                }
                query = query.Where(predicate);
            }
            query = query.OrderBy(cdd => cdd.FULL_NM);
            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Retrieves all active Master And Regular Accounts
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getMasterandRegularAccounts(string[] RangeSearch)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();
            var predicate = PredicateBuilder.False<AccountBE>();


            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true
             && (cdd.custmr_rel_actv_ind == null || cdd.custmr_rel_actv_ind == false)
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 ACTV_IND = cdd.actv_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id
             });
            if (RangeSearch != null)
            {
                foreach (string searchTerm in RangeSearch)
                {
                    string temp = searchTerm;
                    predicate = predicate.Or(p => p.FULL_NM.StartsWith(temp));
                }
                query = query.Where(predicate);
            }
            query = query.OrderBy(cdd => cdd.FULL_NM);
            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns MasterAccount and its child accounts for a given Master Account
        /// Naresh Kumar 10/23/2008
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getMasterWithChildAccounts(int MasterAccountID)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true
             && (cdd.custmr_id == MasterAccountID || (cdd.custmr_rel_id == MasterAccountID && cdd.custmr_rel_actv_ind == true))
             orderby cdd.full_nm
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 ACTV_IND = cdd.actv_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id
             });

            //query = query.Where(cdd => cdd.MSTR_ACCT_IND != true && cdd.CUSTMR_REL_ID == MasterAccountID && cdd.ACTV_IND == true );

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        public IList<AccountBE> getMasterWithChildAccounts(int MasterAccountID, string[] RangeSearch)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            var predicate = PredicateBuilder.False<AccountBE>();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true
             && cdd.custmr_id == MasterAccountID || (cdd.custmr_rel_id == MasterAccountID && cdd.custmr_rel_actv_ind == true)
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 ACTV_IND = cdd.actv_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id
             });


            foreach (string searchTerm in RangeSearch)
            {
                string temp = searchTerm;
                predicate = predicate.Or(p => p.FULL_NM.StartsWith(temp));
            }
            query = query.Where(predicate);
            query = query.OrderBy(cdd => cdd.FULL_NM);
            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all accounts that match criteria
        /// </summary>
        /// <param name="AccountName"></param>
        /// <param name="BPNumber"></param>
        /// <param name="AccountNumber"></param>
        /// <param name="SSCGID"></param>
        /// <returns></returns>
        public IList<AccountBE> getAccountData(string AccountName, string BPNumber, int AccountNumber, string SSCGID)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id
             });

            /// Get a specific account record
            if (AccountName.Trim().Length > 0)
                query = query.Where(cdd => (cdd.FULL_NM.Contains(AccountName.Trim())));

            if (BPNumber.Trim().Length > 0)
                query = query.Where(cdd => (cdd.FINC_PTY_ID.Trim().Equals(BPNumber)));

            if (AccountNumber > 0)
                query = query.Where(cdd => (cdd.CUSTMR_ID == AccountNumber));

            if (SSCGID.Trim().Length > 0)
                query = query.Where(cdd => cdd.SUPRT_SERV_CUSTMR_GP_ID.Trim().Equals(SSCGID.Trim()));


            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all accounts that match criteria ... Anil 07/28/08
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<AccountBE> getRelatedAccountData(int AccountID)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             orderby cdd.mstr_acct_ind descending, cdd.full_nm, cdd.actv_ind
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 CUSTMR_REL_ID = cdd.custmr_rel_id,
                 CUSTMR_REL_ACTV_IND = cdd.custmr_rel_actv_ind,
                 UPDATE_DATE = cdd.updt_dt,
                 UPDATE_USER_ID = cdd.updt_user_id
             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.CUSTMR_REL_ID == AccountID);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all related account data that match criteria ... 
        /// Anil 09/02/2008
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<AccountBE> getRelatedAccountDataRO(int AccountID, int MasterAccountID)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             orderby cdd.mstr_acct_ind descending, cdd.full_nm, cdd.actv_ind
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id,
                 CUSTMR_REL_ID = cdd.custmr_rel_id,
                 CUSTMR_REL_ACTV_IND = cdd.custmr_rel_actv_ind,
                 ACTV_IND = cdd.actv_ind,
                 MSTR_ACCT_IND = cdd.mstr_acct_ind

             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => (cdd.CUSTMR_ID == AccountID || cdd.CUSTMR_ID == MasterAccountID) && cdd.ACTV_IND == true);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        /// <summary>
        ///  Save method performs inserts and updates into the ACCT table
        /// </summary>
        /// <param name="acct"></param>
        /// <returns></returns>
        public bool Save(CUSTMR acct)
        {

            if (acct.custmr_id > 0)
            {
                /// Perform an upate
                this.Context.CUSTMRs.Attach(acct, true);
            }
            else
            {
                /// Perform an Insert 
                this.Context.CUSTMRs.InsertOnSubmit(acct);
            }

            //            if (singleTransaction)
            {
                try
                /// Save is not part of a larger transaction
                {   /// Commit changes to database now
                    this.Context.SubmitChanges();
                }
                catch (System.Data.DBConcurrencyException ex)
                {
                    this.Context.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepCurrentValues);
                }
            }
            return true;
        }

        public IList<AccountBE> getaccountdataddl()
        {
            if (this.Context == null)
                this.Initialize();

            IList<AccountBE> result = new List<AccountBE>();

            IQueryable<AccountBE> query =
               (from cdd in this.Context.CUSTMRs
                select new AccountBE()
                {
                    CUSTMR_ID = cdd.custmr_id,
                    FULL_NM = cdd.full_nm,
                    SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                    FINC_PTY_ID = cdd.finc_pty_id,
                });

            result = query.ToList();
            return result;


        }

        public bool CheckDuplicateAccountName(string Name)
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<AccountBE> query =
                  (from cdd in this.Context.CUSTMRs
                   where cdd.full_nm == Name
                   select new AccountBE
                   {
                       CUSTMR_ID = cdd.custmr_id
                   });

            if (query.ToList().Count > 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Checks wether PEO Indicater is set for a given account. ProgramPeriod Screen purpose
        /// Sreedhar 16th Oct 2008
        /// </summary>
        /// <param name="accountID">AccountID</param>
        /// <returns>retuns true if PEO is set, False if not set</returns>
        public bool isAccountSetUPasPEO(int accountID)
        {
            var isPEO = false;
            if (this.Context == null) this.Initialize();

            if (accountID > 0)
            {
                isPEO = (from cst in this.Context.CUSTMRs
                         where cst.custmr_id == accountID
                         select ((cst.peo_ind == null) ? false : Convert.ToBoolean(cst.peo_ind))).First();
            }
            if (isPEO != null)
                return Convert.ToBoolean(isPEO);
            else
                return false;
        }
        /// <summary>
        /// Returns all accounts that match criteria-Adjustment Review
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> getAccountInfo(int intAccNo)
        {
            IList<AccountBE> result = new List<AccountBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true
             select new AccountBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 CUSTMR_REL_ID = cdd.custmr_rel_id,
                 SUPRT_SERV_CUSTMR_GP_ID = cdd.suprt_serv_custmr_gp_id,
                 FINC_PTY_ID = cdd.finc_pty_id
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            query = query.Where(acct => acct.CUSTMR_ID == intAccNo || acct.CUSTMR_REL_ID == intAccNo);
            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Sets parent_id null for child accounts
        /// </summary>
        /// <param name="accountID">Parent Account ID</param>
        public void SetZeroChildAccounts(int accountID)
        {
            if (this.Context == null)
                this.Initialize();
            this.Context.ExecuteCommand("update custmr set custmr_rel_id=null, custmr_rel_actv_ind=null "
                    + " where custmr_id in (select custmr_id from custmr where custmr_rel_id=" +
                     accountID.ToString() + ")");
        }

    }


    /// <summary>
    /// This is class is used for holding Account Details for the Searched Accounts
    /// </summary>
    public class AccountSearchDA : DataAccessor<CUSTMR, AccountSearchBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Returns all accounts that match criteria
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<AccountSearchBE> getAccountData(int AccountID)
        {
            IList<AccountSearchBE> result = new List<AccountSearchBE>();

            if (this.Context == null)
                this.Initialize();

            // Generate query to retrieve account information
            // and project it into Account Business Entity
            IQueryable<AccountSearchBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true && cdd.custmr_id == AccountID
             orderby cdd.full_nm
             select new AccountSearchBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = Convert.ToInt64((cdd.suprt_serv_custmr_gp_id == null) ? "0" : cdd.suprt_serv_custmr_gp_id),
                 FINC_PTY_ID = Convert.ToInt64((cdd.finc_pty_id == null) ? "0" : cdd.finc_pty_id),
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
             }).Take(25);

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();

            //Sets Postal Address
            SetPostalAddress(result);

            return result;
        }


        /// <summary>
        /// Returns all accounts that match criteria
        /// </summary>
        /// <param name="AccountName"></param>
        /// <param name="BPNumber"></param>
        /// <param name="AccountNumber"></param>
        /// <param name="SSCGID"></param>
        /// <returns></returns>
        public IList<AccountSearchBE> getAccountData(string AccountName, string BPNumber, int AccountNumber, string SSCGID)
        {
            IList<AccountSearchBE> result = new List<AccountSearchBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountSearchBE> query =
            (from cdd in this.Context.CUSTMRs
             where cdd.actv_ind == true
             orderby cdd.full_nm
             select new AccountSearchBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
                 SUPRT_SERV_CUSTMR_GP_ID = Convert.ToInt64((cdd.suprt_serv_custmr_gp_id == null) ? "0" : cdd.suprt_serv_custmr_gp_id),
                 FINC_PTY_ID = Convert.ToInt64((cdd.finc_pty_id == null) ? "0" : cdd.finc_pty_id),
                 MSTR_ACCT_IND = cdd.mstr_acct_ind,
                 CUSTMR_REL_ID = cdd.custmr_rel_id,
                 ACTV_IND = cdd.actv_ind
             });

            /// Get a specific account record
            if (AccountName.Trim().Length > 0)
                query = query.Where(cdd => (cdd.FULL_NM.Contains(AccountName.Trim())));

            if (BPNumber.Trim().Length > 0)
                query = query.Where(cdd => (cdd.FINC_PTY_ID == Convert.ToInt64(BPNumber)));

            if (AccountNumber > 0)
                query = query.Where(cdd => (cdd.CUSTMR_ID == AccountNumber));

            if (SSCGID.Trim().Length > 0)
                query = query.Where(cdd => cdd.SUPRT_SERV_CUSTMR_GP_ID == Convert.ToInt64(SSCGID));


            /// Force an enumeration so that the SQL is only
            /// executed in this method
            query = query.Take(25);
            result = query.ToList();

            //Sets Postal Address
            SetPostalAddress(result);

            return result;
        }

        /// <summary>
        /// Set Postal Address to List of Account Search BE
        /// </summary>
        /// <param name="acctSrchList">List of Account Search BE</param>
        private void SetPostalAddress(IList<AccountSearchBE> acctSrchList)
        {
            int pcLkUpID = (from lk in this.Context.LKUPs
                            join lktp in this.Context.LKUP_TYPs
                            on lk.lkup_typ_id equals lktp.lkup_typ_id
                            where lktp.lkup_typ_nm_txt == "PRIMARY CONTACT"
                            select lk.lkup_id).First();

            foreach (AccountSearchBE asbe in acctSrchList)
            {
                IList<PostalAddressBE> postAddrs = (from cpr in this.Context.CUSTMR_PERS_RELs
                                                    join pr in this.Context.PERs
                                                    on cpr.pers_id equals pr.pers_id
                                                    join pa in this.Context.POST_ADDRs
                                                    on pr.pers_id equals pa.pers_id
                                                    where cpr.rol_id == pcLkUpID
                                                    && cpr.custmr_id == asbe.CUSTMR_ID
                                                    select new PostalAddressBE
                                                    {
                                                        ADDRESS1 = pa.addr_ln_1_txt,
                                                        ADDRESS2 = pa.addr_ln_2_txt,
                                                        CITY = pa.city_txt,
                                                        ZIP_CODE = pa.post_cd_txt,
                                                        STATE_CODE = pa.LKUP.attr_1_txt,
                                                    }).ToList();
                if (postAddrs.Count > 0)
                    asbe.POSTAL_ADDRESS = postAddrs[0].ADDRESS1 + ", " + postAddrs[0].ADDRESS2 + " <br>" +
                                             postAddrs[0].CITY + ", " + postAddrs[0].STATE_CODE + " " + postAddrs[0].ZIP_CODE;
                //CustomerContactBE cust = (new CustomerContactDA()).getPrimaryContactData(asbe.CUSTMR_ID);
                //if(!cust.IsNull())
                //asbe.POSTAL_ADDRESS = cust.POSTALADDRESS;
            }
        }
        //Invoice Inquiry
        public IList<AccountSearchBE> getRelatedAccountNo(int AccountID)
        {
            IList<AccountSearchBE> result = new List<AccountSearchBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<AccountSearchBE> query =
            (from cdd in this.Context.CUSTMRs
             where (cdd.custmr_rel_id == AccountID || cdd.custmr_id == AccountID) && cdd.actv_ind == true
             orderby cdd.mstr_acct_ind ascending, cdd.full_nm, cdd.actv_ind
             select new AccountSearchBE()
             {
                 CUSTMR_ID = cdd.custmr_id,
                 FULL_NM = cdd.full_nm,
             });
            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

    }

}
