using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Reflection;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class ApplWebPageAudtBS : BusinessServicesBase<ApplWebPageAudtBE, ApplWebPageAudtDA>
    {
      /// <summary>
        /// Constructor for ApplWebPageAudtBS
      /// </summary>
       public ApplWebPageAudtBS()
        { 
        }

       public void Save(string webPageName, int userID)
       {
           ApplWebPageAudtBE auditTrail = null;

           try
           {
               int webPageID = (new BLAccess()).GetLookUpID(webPageName);
               auditTrail = new ApplWebPageAudtBE();
               auditTrail.WEB_PAGE_ID = webPageID;
               auditTrail.CREATE_DATE = DateTime.Now;
               auditTrail.CREATE_USER_ID = userID;
               DA.Add(auditTrail);
           }
           catch (Exception ex)
           {
               throw new RetroBaseException(ex.Message);
           }
       }

       public void Save(string webPageName, int userID, string updatedColumn)
       {
           ApplWebPageAudtBE auditTrail = null;

           try
           {
               int webPageID = (new BLAccess()).GetLookUpID(webPageName);
               auditTrail = new ApplWebPageAudtBE();
               auditTrail.WEB_PAGE_ID = webPageID;
               auditTrail.WEB_PAGE_CNTRL_TXT = updatedColumn;
               auditTrail.CREATE_DATE = DateTime.Now;
               auditTrail.CREATE_USER_ID = userID;
               DA.Add(auditTrail);
           }
           catch (Exception ex)
           {
               throw new RetroBaseException(ex.Message);
           }
       }

        public void Save(int custmr_ID, string webPageName,  int userID, string updatedColumn)
       {
           ApplWebPageAudtBE auditTrail = null;

           try
           {
               int webPageID = (new BLAccess()).GetLookUpID(webPageName);
               auditTrail = new ApplWebPageAudtBE();
               auditTrail.CUSTMR_ID = custmr_ID;
               auditTrail.WEB_PAGE_ID = webPageID;
               auditTrail.WEB_PAGE_CNTRL_TXT = updatedColumn;
               auditTrail.CREATE_DATE = DateTime.Now;
               auditTrail.CREATE_USER_ID = userID;
               DA.Add(auditTrail);
           }
           catch (Exception ex)
           {
               throw new RetroBaseException(ex.Message);
           }
       }

       public void Save(int custmr_ID, int programPeriodID, string webPageName, 
           int userID, string updatedColumn)
       {
           ApplWebPageAudtBE auditTrail = null;

           try
           {
               int webPageID = (new BLAccess()).GetLookUpID(webPageName);
               auditTrail = new ApplWebPageAudtBE();
               auditTrail.CUSTMR_ID = custmr_ID;
               auditTrail.PREM_ADJ_PGM_ID = programPeriodID;
               auditTrail.WEB_PAGE_ID = webPageID;
               auditTrail.WEB_PAGE_CNTRL_TXT = updatedColumn;
               auditTrail.CREATE_DATE = DateTime.Now;
               auditTrail.CREATE_USER_ID = userID;
               DA.Add(auditTrail);
           }
           catch (Exception ex)
           {
               throw new RetroBaseException(ex.Message);
           }
       }

       public void Save(int custmr_ID, string webPageName, int userID)
       {
           ApplWebPageAudtBE auditTrail = null;

           try
           {
               int webPageID = (new BLAccess()).GetLookUpID(webPageName);
               auditTrail = new ApplWebPageAudtBE();
               auditTrail.CUSTMR_ID = custmr_ID;
               auditTrail.WEB_PAGE_ID = webPageID;
               auditTrail.CREATE_DATE = DateTime.Now;
               auditTrail.CREATE_USER_ID = userID;
               DA.Add(auditTrail);
           }
           catch (Exception ex)
           {
               throw new RetroBaseException(ex.Message);
           }
       }

       public void Save(int custmr_ID, int programPeriodID, string webPageName,  int userID)
       {
           ApplWebPageAudtBE auditTrail = null;

           try
           {
               int webPageID = (new BLAccess()).GetLookUpID(webPageName);
               auditTrail = new ApplWebPageAudtBE();
               auditTrail.CUSTMR_ID = custmr_ID;
               auditTrail.PREM_ADJ_PGM_ID = programPeriodID;
               auditTrail.WEB_PAGE_ID = webPageID;
               auditTrail.CREATE_DATE = DateTime.Now;
               auditTrail.CREATE_USER_ID = userID;
               DA.Add(auditTrail);
           }
           catch (Exception ex)
           {
               throw new RetroBaseException(ex.Message);
           }
       }

   }
   
}
