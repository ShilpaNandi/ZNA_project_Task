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
    /// DataAccessor for LSICustomer details
    /// </summary>
    public class LSICustomersDA : DataAccessor<LSI_CUSTMR, LSICustomerBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Returns all LSI accounts that match criteria
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public IList<LSIAllCustomersBE> getLSIAccountData()
        {
            if (this.Context == null)
                this.Initialize();

            IEnumerable<LSIAllCustomersBE> query =
                (from cdd in this.Context.GetAIS_LSIAccount_Data()
                 select new LSIAllCustomersBE()
                 {
                     FULL_NAME = cdd.AccountName,
                     LSI_ACCT_ID = cdd.AccountID
                 });

            return query.ToList();
        }

        /// <summary>
        /// Returns all LSI accounts that match criteria
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public IList<LSIAllCustomersBE> getLSIAccountData(string[] RangeSearch)
        {
            var predicate = PredicateBuilder.False<LSIAllCustomersBE>();
            IList<LSIAllCustomersBE> result = new List<LSIAllCustomersBE>();

            if (this.Context == null)
                this.Initialize();

            IQueryable<LSIAllCustomersBE> query =
                (from cdd in this.Context.GetAIS_LSIAccount_Data()
                 select new LSIAllCustomersBE()
                 {
                     FULL_NAME = cdd.AccountName,
                     LSI_ACCT_ID = cdd.AccountID
                 }).AsQueryable < LSIAllCustomersBE>();

            foreach (string searchTerm in RangeSearch)
            {
                string temp = searchTerm;
                predicate = predicate.Or(p => p.FULL_NAME.StartsWith(temp));
            }

            query = query.Where(predicate);
             query = query.OrderBy(cdd => cdd.FULL_NAME);
             result = query.ToList();
             return result;
        }

        /// <summary>
        /// Returns all related LSI accounts that match criteria ... 
        /// Anil 08/21/08
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<LSICustomerBE> getRelatedLSIAccountData(int AccountID)
        {
            IList<LSICustomerBE> result = new List<LSICustomerBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<LSICustomerBE> query =
            (from cdd in this.Context.LSI_CUSTMRs
             orderby cdd.full_nm
             select new LSICustomerBE()
             {
                 LSI_ACCT_ID = cdd.lsi_acct_id,
                 CUSTMR_ID = cdd.custmr_id,
                 LSI_CUSTMR_ID = cdd.lsi_custmr_id,
                 FULL_NAME = cdd.full_nm,
                 ACTV_IND = cdd.actv_ind,
                 PRIM_IND = cdd.prim_ind,
                 UPDATE_DATE = cdd.updt_dt,
                 UPDATE_USER_ID = cdd.updt_user_id
             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.CUSTMR_ID == AccountID);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        public bool getDuplLSIPrimaryAccount(bool isActive, bool Primselected, int AccountID)
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<LSICustomerBE> query =
              (from cdd in this.Context.LSI_CUSTMRs
               where cdd.prim_ind == true && cdd.actv_ind == true && cdd.custmr_id == AccountID
               select new LSICustomerBE
               {
                   LSI_CUSTMR_ID = cdd.lsi_custmr_id
               });

            if (query.ToList().Count > 0 && Primselected && isActive)
                return true;
            else
                return false;
        }

        public bool CheckDuplicateLSIAccountName(int LSIAccountID, int AccountID)
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<LSICustomerBE> query =
              (from cdd in this.Context.LSI_CUSTMRs
               where cdd.custmr_id == AccountID && cdd.lsi_acct_id == LSIAccountID
               select new LSICustomerBE
               {
                   LSI_CUSTMR_ID = cdd.lsi_custmr_id
               });

            if (query.ToList().Count > 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// This method is written to verify Permissions on LSI Database for AIS user
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckLSIPermissions()
        {
            if (this.Context == null)
                this.Initialize();
            try
            {
                IEnumerable<LSIAllCustomersBE> query =
                    (from cdd in this.Context.GetAIS_LSIAccount_Data()
                     select new LSIAllCustomersBE()
                     {
                         FULL_NAME = cdd.AccountName,
                         LSI_ACCT_ID = cdd.AccountID
                     });

                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

    }
}
