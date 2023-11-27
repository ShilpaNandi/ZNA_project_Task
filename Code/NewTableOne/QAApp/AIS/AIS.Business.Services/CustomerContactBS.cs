using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    //public class CustomerContactBS : BaseBS<CustomerContactBE , CustomerContactDA>
    public class CustomerContactBS : BusinessServicesBase<CustomerContactBE, CustomerContactDA>
    {
        public CustomerContactBS():base()
        { }

        public IList<CustomerContactBE> getInsuredContactData(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();

            CustomerContactDA custmrCtDA = new CustomerContactDA();
            result = custmrCtDA.getInsuredContactData(AccountID);
            return result;
        }

        public CustomerContactBE getPrimaryContactData(int AccountID)
        {
            CustomerContactBE result = new CustomerContactBE();

            CustomerContactDA custmrCtDA = new CustomerContactDA();
            result = custmrCtDA.getPrimaryContactData(AccountID);
            return result;
        }
/// <summary>
/// This method is used to get the Account Responsibilities.
/// </summary>
/// <param name="AccountID"></param>
/// <returns></returns>
        public IList<CustomerContactBE> getAccountResponsibilities(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();
            CustomerContactDA custmrCtDA = new CustomerContactDA();

            try
            {
                result = custmrCtDA.getAccountResponsibilities(AccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (result);
        }
       
        public IList<CustomerContactBE> getLSSAnalystName(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();
            CustomerContactDA custmrCtDA = new CustomerContactDA();

            try
            {
                result = custmrCtDA.getLSSAnalystName(AccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (result);
        }
        //Invoice Inquiry
        public IList<CustomerContactBE> getCFS2Names(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();

            try
            {
                result = new CustomerContactDA().getCFS2Names(AccountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (result);
        }
        public CustomerContactBE getInsuredContactData(int AccountID, int personID)
        {
            CustomerContactBE result = new CustomerContactBE();

            CustomerContactDA custmrCtDA = new CustomerContactDA();
            result = custmrCtDA.getInsuredContactData(AccountID,personID);
            result = DA.Load(result.CUSTOMER_CONTACT_ID);
            return result;
        }
        /// <summary>
        /// This method is used to assign responsibilites for a particular account.
        /// </summary>
        /// <param name="Customerid"></param>
        /// <param name="responsibilities"></param>
        /// <param name="Personid"></param>
        public bool AssignResponsibilities(int Customerid, ArrayList responsibilities, 
            ArrayList Personid, out string errorMessage, IList<CustomerContactBE> actresps,
            bool handleConcurrency, int UserID)
        {
            bool status;
            CustomerContactDA custmrCtDA = new CustomerContactDA();
            status = custmrCtDA.Assignresponsibilities(Customerid, responsibilities, Personid,
                        out errorMessage, actresps, handleConcurrency, UserID);
            return status;
        }

        public CustomerContactBE AriesPerson(int customerid, int roleid)
        {
            
            CustomerContactDA custmrCtDA = new CustomerContactDA();
       // fullname=custmrCtDA.GetAriesperson(customerid,roleid);
           CustomerContactBE CustomercontactBE = custmrCtDA.GetAriesperson(customerid, roleid);
           return CustomercontactBE;
        
        }
#region External Contacts

        
        public CustomerContactBE getCustmerContact(int PERSON_ID)
        {
            CustomerContactDA CusCtDA=new CustomerContactDA();
            return CusCtDA.getCustmerContact(PERSON_ID);
            
        }

        public CustomerContactBE getCustmerContactData(int CUSTOMER_CONTACT_ID)
        {
            CustomerContactBE custCt = new CustomerContactBE();
            custCt = DA.Load(CUSTOMER_CONTACT_ID);
            return custCt;
        }



        public bool Update(CustomerContactBE CusCt)
        {            
            bool suceeded = false;

            if (CusCt.CUSTOMER_CONTACT_ID > 0)
            {   
                suceeded = DA.Update(CusCt);
            }
            else
            {  
                suceeded = DA.Add(CusCt);
            }
            return suceeded;
        }
         public bool Delete(CustomerContactBE CusCt)
        {            
            bool suceeded = false;
            suceeded = DA.Delete(CusCt);
            return suceeded;
        }

#endregion

    }
}
