using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// Serves for all database functions for Broker contacts
    /// </summary>
    public class BrokerDA : DataAccessor<EXTRNL_ORG,BrokerBE,AISDatabaseLINQDataContext>
    {
        //public IList<BrokerBE> getBrokerData(int brokerID)
        //{
        //    IList<BrokerBE> result = new List<BrokerBE>();

        //    IQueryable<BrokerBE> query = from res in this.Context.EXTRNL_ORGs
        //                                  join lk in this.Context.LKUPs
        //                                  on res.role_id equals lk.lkup_id
        //                                  join lktype in this.Context.LKUP_TYPs
        //                                  on lk.lkup_typ_id equals lktype.lkup_typ_id
        //                                  where res.actv_ind == true &&
        //                                        lktype.actv_ind == true &&
        //                                        lktype.lkup_typ_nm_txt == "CONTACT TYPE"
        //                                  select new BrokerBE
        //                                  {
        //                                      EXTRNL_ORG_ID = res.extrnl_org_id,
        //                                      FULL_NAME = res.full_name,
        //                                      CONTACT_TYPE_ID = res.role_id,
        //                                      UPDATE_USER_ID=res.updt_user_id,
        //                                      UPDATE_DATE=res.updt_dt,
        //                                      CREATE_USER_ID=res.crte_user_id,
        //                                      CREATE_DATE=res.crte_dt,
        //                                      CONTACT_TYPE = lk.lkup_txt

        //                                  };

        //    if (brokerID >= 0)
        //    {
        //        query = query.Where(res => res.EXTRNL_ORG_ID == brokerID);
        //    }

        //    result = query.ToList();
        //    return result;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="BrokerID"></param>
        ///// <returns></returns>
        //public string getBrokerName(int brokerID)
        //{
        //    string result =
        //          (from cdd in this.Context.EXTRNL_ORGs
        //           where cdd.extrnl_org_id == brokerID
        //           select cdd.full_name).Single().ToString();

        //    return result;
        //}

      
        /// <summary>
        /// Invoked to Know if entered Contacts is already existing or not
        /// </summary>
        /// <param name="BrokerName"></param>
        /// <param name="ContactTypeID"></param>
        /// <returns>Treu if already exists, False if notS</returns>
        public bool IsContactNameExists(string brokerName, int contactTypeID,int extOrgID)
        {
             bool Flag = false;
            
            var statusInfo = from cdd in this.Context.EXTRNL_ORGs
                             where (cdd.full_name == brokerName) && (cdd.role_id == contactTypeID) && (cdd.extrnl_org_id != extOrgID) && (cdd.extrnl_org_id != 1000000)
                              select cdd ;

            if (statusInfo.Count() > 0)                           
                Flag = true;            
            else
                Flag=false;

            return Flag;            
        }
        /// <summary>
        /// Invoked to fill the External Contacts Names Listview
        /// </summary>
        /// <returns>List of Contacts Records</returns>
        public IList<BrokerBE> GetBrokers()
        {
            IList<BrokerBE> brokers = new List<BrokerBE>();

            IQueryable<BrokerBE> result = from res in this.Context.EXTRNL_ORGs
                                          join lk in this.Context.LKUPs on res.role_id equals lk.lkup_id
                                          join lktype in this.Context.LKUP_TYPs on lk.lkup_typ_id equals lktype.lkup_typ_id
                                          where lktype.actv_ind == true && lktype.lkup_typ_nm_txt == "CONTACT TYPE"
                                          && lk.attr_1_txt == "E" && res.extrnl_org_id != 1000000
                                          orderby lk.lkup_txt, res.full_name
                                          //where res.actv_ind == true
                                          select new BrokerBE
                                          {
                                             EXTRNL_ORG_ID = res.extrnl_org_id,
                                             FULL_NAME = res.full_name,
                                             CONTACT_TYPE_ID = res.role_id,
                                             UPDATE_DATE = res.updt_dt,
                                             CREATE_USER_ID = res.crte_user_id,
                                             CREATE_DATE = res.crte_dt,
                                             CONTACT_TYPE = lk.lkup_txt,
                                             ACTV_IND=res.actv_ind
                                          };
            brokers = result.ToList();

            return brokers;
        }

        /// <summary>
        /// Invoked to popup the External Contacts DropDown
        /// </summary>
        /// <returns>Collection of External Contacts Records</returns>
        public IList<LookupBE> GetBrokersForLookups()
        {
            IList<LookupBE> brokers = new List<LookupBE>();

            IQueryable<LookupBE> result = from res in this.Context.EXTRNL_ORGs
                                          where  res.extrnl_org_id != 1000000
                                          orderby res.full_name
                                          select new LookupBE
                                          {
                                               LookUpID=res.extrnl_org_id,
                                               LookUpName=res.full_name                                              
                                          };
            brokers = result.ToList();

            return brokers;
        }
        /// <summary>
        /// Invoked to popup the Invoice Inquiry DropDown
        /// </summary>
        /// <returns>IList<BrokerBE></returns>
        public IList<BrokerBE> GetBrokersList()
        {
            IList<BrokerBE> brokers = new List<BrokerBE>();

            IQueryable<BrokerBE> result = from res in this.Context.EXTRNL_ORGs
                                          where  res.extrnl_org_id != 1000000
                                          orderby res.full_name
                                          select new BrokerBE
                                          {
                                              EXTRNL_ORG_ID = res.extrnl_org_id,
                                              FULL_NAME = res.full_name
                                          };
            brokers = result.ToList();

            return brokers;
        }
        public IList<LookupBE> GetOnlyBrokersForLookups()
        {
            int brokerLookUpID = this.Context.LKUPs.Where(o => o.lkup_txt == "BROKER" ).Select(p => p.lkup_id).Single();

            IList<LookupBE> brokers = new List<LookupBE>();

            IQueryable<LookupBE> result = from res in this.Context.EXTRNL_ORGs
                                          where res.role_id == brokerLookUpID && res.extrnl_org_id != 1000000
                                          orderby res.full_name
                                          select new LookupBE
                                          {
                                              LookUpID = res.extrnl_org_id,
                                              LookUpName = res.full_name
                                          };
            brokers = result.ToList();

            return brokers;
        }
    }
}