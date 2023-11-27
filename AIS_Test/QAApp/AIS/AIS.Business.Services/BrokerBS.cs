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
    /// <summary>
    /// Serves for all Broker Contacts funtions as a Business service
    /// </summary>
    public class BrokerBS : BusinessServicesBase<BrokerBE, BrokerDA>
    {
        /// <summary>
        /// Invoked to fill the External Contacts Names Listview
        /// </summary>
        /// <returns>List of Contacts Records</returns>
        public IList<BrokerBE> GetBrokerData()
        {
            IList<BrokerBE> BrokerList = new List<BrokerBE>();
            BrokerDA Brokers = new BrokerDA();
            try
            {                
                BrokerList = Brokers.GetBrokers();
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            
            return BrokerList;
        }
     
        /// <summary>
        /// Used to Update and New record Save
        /// </summary>
        /// <param name="brkBE">Broker BE </param>
        /// <returns>True if saved , False if not saved</returns>
        public bool Update(BrokerBE brkBE)
        {
            IList<BrokerBE> BrokerList = new List<BrokerBE>();
            bool suceeded = false;
            try
            {
                //If extnlOrg Id is Greater then 0 then it is Edits record(using Frame work) using DA.Update will be invoked
                if (brkBE.EXTRNL_ORG_ID > 0)
                {
                    suceeded = DA.Update(brkBE);
                }
                //If extnlOrg Id is equals to 0 then it is new record(using Frame work) DA.Add will be invoked
                else
                {
                    suceeded = DA.Add(brkBE);
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            
            return suceeded;
        }
        /// <summary>
        /// Invoked to Know if entered Contacts is already existing or not
        /// </summary>
        /// <param name="BrokerName"></param>
        /// <param name="ContactTypeID"></param>
        /// <returns>Treu if already exists, False if notS</returns>
        public bool IsContactNameExists(string contactName, int contactTypeID,int extOrgID)
        {
            BrokerDA BrokerDA = new BrokerDA();
            try
            {
                return BrokerDA.IsContactNameExists(contactName, contactTypeID, extOrgID);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }            
            
        }

        /// <summary>
        /// Invoked to load a spesific ExtContact row using Frame work
        /// </summary>
        /// <param name="BrokerID">External Contacts ID</param>
        /// <returns>Returns a Record</returns>
        public BrokerBE getBroker(int brokerID)
        {
            BrokerBE Brk = new BrokerBE();
            Brk = DA.Load(brokerID);
            return Brk;
        }
        /// <summary>
        /// Invoked to popup the External Contacts DropDown
        /// </summary>
        /// <returns>Collection of External Contacts Records</returns>
        public IList<LookupBE> GetBrokersForLookups()
        {
            IList<LookupBE> BrokerList = new List<LookupBE>();
            BrokerDA Brokers = new BrokerDA();

            try
            {
                BrokerList = Brokers.GetBrokersForLookups();

                LookupBE brkBE = new LookupBE();
                brkBE.LookUpTypeID = 0;
                brkBE.LookUpName = "(Select)";
                BrokerList.Insert(0, brkBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }            

            return BrokerList;
        }
        public IList<LookupBE> GetOnlyBrokersForLookups()
        {
            IList<LookupBE> BrokerList = new List<LookupBE>();
            BrokerDA Brokers = new BrokerDA();

            try
            {
                BrokerList = Brokers.GetOnlyBrokersForLookups();

                //Adding a new row (Select) as first row
                LookupBE sellkupBE = new LookupBE();
                sellkupBE.LookUpTypeID = 0;
                sellkupBE.LookUpName = "(Select)";
                BrokerList.Insert(0, sellkupBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return BrokerList;
        }
        public IList<LookupBE> GetOnlyBrokersForLookupsNA()
        {
            IList<LookupBE> BrokerList = new List<LookupBE>();
            BrokerDA Brokers = new BrokerDA();

            try
            {
                BrokerList = Brokers.GetOnlyBrokersForLookups();

                //Adding a new row (Select) as first row
                LookupBE sellkupBE = new LookupBE();
                sellkupBE.LookUpTypeID = 0;
                sellkupBE.LookUpName = "(Select)";
                BrokerList.Insert(0, sellkupBE);

                //Adding a new row (Not Applicable) as Second row
                LookupBE sellkupBE1 = new LookupBE();
                sellkupBE1.LookUpID = 1000000;
                sellkupBE1.LookUpName = "Not Applicable";
                BrokerList.Insert(1, sellkupBE1);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return BrokerList;
        }
        /// <summary>
        /// Invoked to Popup the Invoice Inquiry DropDown
        /// </summary>
        /// <returns>IList<BrokerBE></returns>
        public IList<BrokerBE> GetBrokersList()
        {
            IList<BrokerBE> BrokerList = new List<BrokerBE>();
            BrokerDA Brokers = new BrokerDA();

            try
            {
                BrokerList = Brokers.GetBrokersList();

                BrokerBE brkBE = new BrokerBE();
                brkBE.EXTRNL_ORG_ID = 0;
                brkBE.FULL_NAME = "(Select)";
                BrokerList.Insert(0, brkBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return BrokerList;
        }
    }
}
