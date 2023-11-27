/*-----	Page:	MasterAdjustmentInfo
-----
-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	Business layer Access.
-----
-----	
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               06/13/09	Zakir Hussain
-----				New Method GetLookUpLOBActiveData added to retrieve LOB Data for a LookUp Table.

*/
using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;

namespace ZurichNA.AIS.Business.Logic
{


    //namespace ZurichNA.AIS.WebSite
    //{

    /// <summary>
    /// Summary description for BLAccess
    /// </summary>
    public class BLAccess
    {
        public BLAccess()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// This method is a wrapper to the BA Class
        /// If the List return is empty or Count =0, we need to add a
        /// Dummy PolicyBE, so that in the GridView a footer row is
        /// displayed. Otherwise the GridView will show empty and nothing will 
        /// be displayed.
        /// </summary>
        /// <returns></returns>
        public IList<PolicyBE> GetPolicyData(int ProgramPeriodID)
        {
            PolicyBS PolicyBS = new PolicyBS();
            IList<PolicyBE> PolicyBEList = new List<PolicyBE>(); ;
            //ToDo:When a program period ID of -1 is passes the DAL throws an error
            //PolicyBS public IList<PolicyBE> getPolicyData(int ProgramPeriodID, int PolicyID) method


            try
            {
                PolicyBEList = PolicyBS.getPolicyData(ProgramPeriodID);
            }
            catch (RetroDatabaseException ex)
            {
                throw ex;
                //Session["RetroException"] = ex;
                //Response.Redirect("~/GeneralError.aspx");
            }

            if (PolicyBEList.Count == 0)
            {
                PolicyBE PolicyBE = new PolicyBE();
                PolicyBE.PolicyID = -1;
                PolicyBEList.Add(PolicyBE);
            }


            return PolicyBEList;
        }

        public IList<PolicyBE> ListviewGetPolicyDataforCust(int ProgramPeriodID, int AccountID)
        {
            PolicyBS PolicyBS = new PolicyBS();
            IList<PolicyBE> PolicyBEList = new List<PolicyBE>(); ;
            //ToDo:When a program period ID of -1 is passes the DAL throws an error
            //PolicyBS public IList<PolicyBE> getPolicyData(int ProgramPeriodID, int PolicyID) method

            try
            {
                PolicyBEList = PolicyBS.getPolicyDataWithActID(ProgramPeriodID, AccountID);
            }
            catch (Exception ex)
            {
                throw ex;
                //Session["RetroException"] = ex;
                //Response.Redirect("~/GeneralError.aspx");                    
            }


            if (PolicyBEList.Count == 0)
            {

            }


            return PolicyBEList;
        }

        public IList<PolicyBE> ListviewGetPolicyDataforCust(int ProgramPeriodID, int AccountID,int adjID)
        {
            PolicyBS PolicyBS = new PolicyBS();
            IList<PolicyBE> PolicyBEList = new List<PolicyBE>(); ;
            //ToDo:When a program period ID of -1 is passes the DAL throws an error
            //PolicyBS public IList<PolicyBE> getPolicyData(int ProgramPeriodID, int PolicyID) method

            try
            {
                PolicyBEList = PolicyBS.getPolicyDataWithActID(ProgramPeriodID, AccountID, adjID);
            }
            catch (Exception ex)
            {
                throw ex;
                //Session["RetroException"] = ex;
                //Response.Redirect("~/GeneralError.aspx");                    
            }


            if (PolicyBEList.Count == 0)
            {

            }


            return PolicyBEList;
        }

        public IList<PolicyBE> ListviewGetPolicyDataforCustLBA(int ProgramPeriodID, int AccountID)
        {
            PolicyBS PolicyBS = new PolicyBS();
            IList<PolicyBE> PolicyBEList = new List<PolicyBE>(); ;
            //ToDo:When a program period ID of -1 is passes the DAL throws an error
            //PolicyBS public IList<PolicyBE> getPolicyData(int ProgramPeriodID, int PolicyID) method

            try
            {
                PolicyBEList = PolicyBS.getPolicyDataWithActIDLBA(ProgramPeriodID, AccountID);
            }
            catch (Exception ex)
            {
                throw ex;
                //Session["RetroException"] = ex;
                //Response.Redirect("~/GeneralError.aspx");                    
            }


            if (PolicyBEList.Count == 0)
            {

            }


            return PolicyBEList;
        }
        public IList<PolicyBE> ListviewGetPolicyData(int ProgramPeriodID)
        {
            PolicyBS PolicyBS = new PolicyBS();
            IList<PolicyBE> PolicyBEList = new List<PolicyBE>(); ;
            //ToDo:When a program period ID of -1 is passes the DAL throws an error
            //PolicyBS public IList<PolicyBE> getPolicyData(int ProgramPeriodID, int PolicyID) method

            try
            {
                PolicyBEList = PolicyBS.getPolicyData(ProgramPeriodID);
            }
            catch (Exception ex)
            {
                throw ex;
                //Session["RetroException"] = ex;
                //Response.Redirect("~/GeneralError.aspx");
            }
            //catch (RetroDatabaseException ex)
            //{
            //    throw ex;
            //    //Session["RetroException"] = ex;
            //    //Response.Redirect("~/GeneralError.aspx");
            //}

            if (PolicyBEList.Count == 0)
            {
                //PolicyBE PolicyBE = new PolicyBE();
                //PolicyBE.POL_ID = -1;
                //PolicyBEList.Add(PolicyBE);
            }


            return PolicyBEList;
        }

        public IList<CustomerContactBE> GetInsContact(int AccountID)
        {
            CustomerContactBS CustomerContactBS = new CustomerContactBS();
            IList<CustomerContactBE> CustomerContactBEList = new List<CustomerContactBE>(); ;

            try
            {
                CustomerContactBEList = CustomerContactBS.getInsuredContactData(AccountID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CustomerContactBEList;
        }

        /// <summary>
        /// If the List return is empty or Count =0, we need to add a
        /// Dummy AccountBE, so that in the GridView a footer row is
        /// displayed. Otherwise the GridView will show empty and nothing will 
        /// be displayed. Anil 07/28/08
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> GetRelatedAccountData(int AccountID)
        {
            AccountBS AccountBS = new AccountBS();
            IList<AccountBE> AccountBEList = new List<AccountBE>(); ;
            try
            {
                AccountBEList = AccountBS.getRelatedAccounts(AccountID);
            }
            catch (RetroDatabaseException ex)
            {
                throw ex;
            }

            if (AccountBEList.Count == 0)
            {
                AccountBE AccountBE = new AccountBE();
                AccountBE.CUSTMR_ID = -1;
                AccountBEList.Add(AccountBE);
            }


            return AccountBEList;
        }

        /// <summary>
        /// Returns all the active and non-master accounts 
        /// to be setup as related accounts.
        /// </summary>
        /// <returns></returns>
        public IList<AccountBE> GetAccountData()
        {
            AccountBS AccountBS = new AccountBS();
            IList<AccountBE> AccountBEList = new List<AccountBE>(); ;

            try
            {
                AccountBEList = AccountBS.getNonMasterAccounts();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return AccountBEList;
        }

        /// <summary>
        /// Returns all the active LSI accounts 
        /// to be setup as related LSI accounts.
        /// </summary>
        /// <returns></returns>
        public IList<LSIAllCustomersBE> GetAllLSICustomers()
        {
            LSICustomersBS LSICustomersBS = new LSICustomersBS();
            IList<LSIAllCustomersBE> LSIAllCustomersBEList = new List<LSIAllCustomersBE>(); ;

            try
            {
                LSIAllCustomersBEList = LSICustomersBS.getLSIAccounts();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return LSIAllCustomersBEList;
        }


        public Dictionary<int, string> GetAdjustmentTypes()
        {
            LookupBS lookup = new LookupBS();

            return lookup.AdjustmentType;

        }
        public Dictionary<int, string> GetLBAAdjustmentTypes()
        {
            LookupBS lookup = new LookupBS();
            return lookup.LBAAdjustmentType;

        }
        public Dictionary<int, string> GetCoverageTypes()
        {
            LookupBS lookup = new LookupBS();

            return lookup.CoverageType;

        }

        public Dictionary<int, string> GetDepState()
        {
            LookupBS lookup = new LookupBS();
            return lookup.States;

        }



        public Dictionary<int, string> GetLossSource()
        {
            LookupBS lookup = new LookupBS();
            return lookup.LossSource;

        }

        public Dictionary<int, string> GetALAEType()
        {
            LookupBS lookup = new LookupBS();
            return lookup.ALAEType;

        }

        public Dictionary<int, string> GetBktcyBuyout()
        {
            LookupBS lookup = new LookupBS();
            return lookup.BktcyBuyout;
        }

        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpData(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];

            lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper().Trim() == lookUpTypeName.ToUpper().Trim()).ToList();
            LookupBE sellkupBE = new LookupBE();
            sellkupBE.LookUpID = 0;
            if (lookUpTypeName != "TRACKING ISSUES")
            {
                sellkupBE.LookUpName = "(Select)";
                lookups.Insert(0, sellkupBE);
            }

            if (lookUpTypeName == "CLAIM STATUS")
            {
                lookups = lookups.Where(lk => lk.LookUpTypeID == 42).ToList();
            }

            //changed by pk as part of bug fix 8367.
            if (lookUpTypeName == "ADJUSTMENT STATUSES")
            {
                lookups = lookups.Where(lk => lk.LookUpID != 347 && lk.LookUpID != 349 && lk.LookUpID != 352).ToList();

            }
            return lookups;
        }

        /// <summary>
        /// Returns LookUP Data with Select Option
        /// </summary>
        /// <param name="lookUpTypeName"></param>
        /// <returns></returns>
        public IList<LookupBE> GetLookUpDataWithSelect(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];

            lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper().Trim()
                    == lookUpTypeName.ToUpper().Trim()).ToList();
            LookupBE sellkupBE = new LookupBE();
            sellkupBE.LookUpID = 0;
            sellkupBE.Attribute1 = "(Select)";
            lookups.Insert(0, sellkupBE);

            return lookups;
        }

        /// <summary>
        /// Returns LookUP Data with Select Option
        /// </summary>
        /// <param name="lookUpTypeName"></param>
        /// <returns></returns>
        public IList<LookupBE> GetLookUpActiveDataWithSelect(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];

            lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper().Trim()
                    == lookUpTypeName.ToUpper().Trim() && lk.ACTIVE == true).ToList();
            lookups = lookups.OrderBy(lk => lk.Attribute1).ToList();
            LookupBE sellkupBE = new LookupBE();
            sellkupBE.LookUpID = 0;
            sellkupBE.Attribute1 = "(Select)";
            lookups.Insert(0, sellkupBE);

            return lookups;
        }

        public IList<LookupBE> GetLookUpDataActive(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];

            lookups = lookups.Where(lk => lk.ACTIVE == true && (lk.LookUpTypeName.ToUpper().Trim() == lookUpTypeName.ToUpper().Trim())).ToList();
            LookupBE sellkupBE = new LookupBE();
            sellkupBE.LookUpID = 0;
            return lookups;
        }

        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpLOBActiveData(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper().Trim() == lookUpTypeName.ToUpper().Trim()
                                    && lk.ACTIVE == true && lk.LookUpID != 347).ToList(); //347 = Cancelled
            lookups = (lookups.OrderBy(lk => lk.LookUpName).ToList());
            LookupBE sellkupBE = new LookupBE();
            sellkupBE.LookUpID = 0;
            sellkupBE.LookUpName = "(Select)";
            lookups.Insert(0, sellkupBE);
            return lookups;
        }
        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpActiveData(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper().Trim() == lookUpTypeName.ToUpper().Trim()
                                    && lk.ACTIVE == true && lk.LookUpID != 347).ToList(); //347 = Cancelled
            lookups = (lookups.OrderBy(lk => lk.LookUpName).ToList());
            LookupBE sellkupBE = new LookupBE();
            sellkupBE.LookUpID = 0;
            if (lookUpTypeName != "TRACKING ISSUES")
            {
                sellkupBE.LookUpName = "(Select)";
                lookups.Insert(0, sellkupBE);
            }
            return lookups;
        }
        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpActiveDataDashboard(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper().Trim() == lookUpTypeName.ToUpper().Trim()
                                    && lk.ACTIVE == true && lk.LookUpID != 347 && lk.LookUpID != 349 && lk.LookUpID != 352).ToList();//347 = Cancelled; 349= FINAL INVOICE; 352 =TRANSMITTED
            lookups = (lookups.OrderBy(lk => lk.LookUpName).ToList());
            LookupBE sellkupBE = new LookupBE();
            sellkupBE.LookUpID = 0;
            sellkupBE.LookUpName = "(Select)";
            lookups.Insert(0, sellkupBE);
            return lookups;
        }

        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name without (Select) Option
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE without (Select) Option</returns>
        public IList<LookupBE> GetLookUpDataWithoutSelect(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName == lookUpTypeName).ToList();
            return lookups;
        }

        /// <summary>
        /// Retrieves Active LookUp Data for a LookUp Type Name without (Select) Option
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE without (Select) Option</returns>
        public IList<LookupBE> GetLookUpActiveDataWithoutSelect(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName == lookUpTypeName && lk.ACTIVE == true).ToList();
            return lookups;
        }

        /// <summary>
        /// Retrieves LookUp Dictionary for a LookUp Type Name
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name Value</param>
        /// <returns>Dictionary List</returns>

        public IDictionary<int, string> GetLookUpDictionary(string lookUpTypeName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            IDictionary<int, string> lookupDict = new Dictionary<int, string>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName == lookUpTypeName).ToList();

            lookupDict.Add(0, "(Select)");
            foreach (LookupBE lkup in lookups)
                lookupDict.Add(lkup.LookUpID, lkup.LookUpName);
            return lookupDict;
        }


        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name with Attribute
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <param name="attribute">Attribute</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpData(string lookUpTypeName, string attribute)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName == lookUpTypeName && lk.Attribute1 == (attribute == null ? "" : attribute.Trim())).ToList();
            lookups = (lookups.OrderBy(lk => lk.LookUpName)).ToList();
            lookups.Insert(0, (new LookupBE() { Attribute1 = "", LookUpID = 0, LookUpName = "(Select)" }));
            return lookups;
        }

        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name with Attribute
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <param name="attribute">Attribute</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpActiveData(string lookUpTypeName, string attribute)
        {

            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName == lookUpTypeName
                && lk.Attribute1 == (attribute == null ? "" : attribute)
                && lk.ACTIVE == true).ToList();
            lookups = (lookups.OrderBy(lk => lk.LookUpName)).ToList();
            lookups.Insert(0, (new LookupBE() { Attribute1 = "", LookUpID = 0, LookUpName = "(Select)" }));
            return lookups;
        }
        /// <summary>
        /// Retrieves LookUp Dictionary for a LookUp Type Name with Attribute
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name Value</param>
        /// <param name="attribute">Attribute</param>
        /// <returns>Dictionary List</returns>

        public IDictionary<int, string> GetLookUpDictionary(string lookUpTypeName, string attribute)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            IDictionary<int, string> lookupDict = new Dictionary<int, string>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpTypeName == lookUpTypeName).ToList();
            lookupDict.Add(0, "(Select)");
            foreach (LookupBE lkup in lookups)
                lookupDict.Add(lkup.LookUpID, lkup.LookUpName);
            return lookupDict;
        }


        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type ID
        /// </summary>
        /// <param name="LookUpTypeID">LookUp Type ID value</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpData(int lookUpTypeID)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = (lookups.Where(lk => lk.LookUpTypeID == lookUpTypeID).ToList());
            lookups = (lookups.OrderBy(lk => lk.LookUpName).ToList());
            return lookups;
        }

        /// <summary>
        /// Retrieves Active LookUp Data for a LookUp Type ID
        /// </summary>
        /// <param name="LookUpTypeID">LookUp Type ID value</param>
        /// <returns>List of LookUpBE</returns>
        public IList<LookupBE> GetLookUpActiveData(int lookUpTypeID)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = (lookups.Where(lk => lk.LookUpTypeID == lookUpTypeID && lk.ACTIVE == true).ToList());
            lookups = (lookups.OrderBy(lk => lk.LookUpName).ToList());
            return lookups;
        }


        /// <summary>
        /// Retrieves LookUp Data for a LookUp Name
        /// </summary>
        /// <param name="LookUpID">LookUp ID value</param>
        /// <returns>LookUpBE</returns>
        public int GetLookUpID(string lookUpName)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (IList<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName.ToUpper().Trim() == lookUpName.ToUpper().Trim()).ToList();
            lookups = (lookups.OrderBy(lk => lk.LookUpName).ToList());
            if (lookups.Count > 0)
                return lookups[0].LookUpID;
            else
                return 0;
        }


        /// <summary>
        /// Retrieves LookUp Data for a LookUp Type Name
        /// </summary>
        /// <param name="LookUpTypeName">LookUp Type Name value</param>
        /// <returns>List of LookUpBE</returns>
        //public IList<LookupBE> GetLookUpLOBActiveData(string lookUpTypeName)
        //{
        //    IList<LookupBE> lookups = new List<LookupBE>();
        //    lookups = (List<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
        //    lookups = lookups.Where(lk => lk.LookUpTypeName.ToUpper().Trim() == lookUpTypeName.ToUpper().Trim()
        //                            && lk.ACTIVE == true && lk.LookUpID != 347).ToList(); //347 = Cancelled
        //    lookups = (lookups.OrderBy(lk => lk.LookUpName).ToList());
        //    LookupBE sellkupBE = new LookupBE();
        //    sellkupBE.LookUpID = 0;
        //    sellkupBE.LookUpName = "(Select)";
        //    lookups.Insert(0, sellkupBE);
        //    return lookups;
        //}

        /// <summary>
        /// Retrieves LookUp Data for a LookUp Name and Lookup Type
        /// </summary>
        /// <param name="LookUpID">LookUp ID value</param>
        /// <returns>LookUpBE</returns>
        public int GetLookUpID(string lookUpName, string lookUpType)
        {
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (IList<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName.ToUpper().Trim() == lookUpName.ToUpper().Trim()
                        && lk.LookUpTypeName.ToUpper().Trim() == lookUpType.ToUpper().Trim()).ToList();
            if (lookups.Count > 0)
                return lookups[0].LookUpID;
            else
                return 0;
        }

        /// <summary>
        /// Returns Lookup Name for a given Lookup ID
        /// </summary>
        /// <param name="lookUpID">Lookup ID</param>
        /// <returns>Lookup Name</returns>
        public string GetLookupName(int lookUpID)
        {
            string retValue = String.Empty;
            IList<LookupBE> lookups = new List<LookupBE>();
            lookups = (IList<LookupBE>)System.Web.HttpContext.Current.Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpID == lookUpID).ToList();
            if (lookups.Count > 0)
                retValue = lookups[0].LookUpName;
            return retValue;
        }
    }

}


