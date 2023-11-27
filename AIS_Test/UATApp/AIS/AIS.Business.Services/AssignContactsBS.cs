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
    /// AssignContactsBS class contains methods to get and update contacts Data
    /// </summary>
    public class AssignContactsBS : BusinessServicesBase<AssignContactsBE, AssignContactsDA>
    {
        /// <summary>
        /// Retrieves existing Contacts data for the perticular Program Period ID
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <returns>Contacts Data </returns>
        public IList<AssignContactsBE> GetContactsData(int programPeriodID)
        {
            IList<AssignContactsBE> Contacts = new List<AssignContactsBE>();
            
            BLAccess blAccess = new BLAccess();
            AssignContactsDA contactsDA = new AssignContactsDA();
            try
            {
                Contacts = contactsDA.GetContactsData(programPeriodID);
            }
            catch (System.Exception ex)
            {
                
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
               
            }
            


            return Contacts;
        }
        public IList<PersonBE> GetPersonNames(int contTypId)
        {
            IList<PersonBE> Details = new List<PersonBE>();
            AssignContactsDA contactsDA = new AssignContactsDA();
            try
            {
                Details = contactsDA.GetPersonNames(contTypId);
            }
            catch (System.Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            PersonBE selperBE = new PersonBE();
            selperBE.PERSON_ID = 0;
            selperBE.FULLNAME = "(Select)";
            Details.Insert(0, selperBE);
            return Details;
        }

        /// <summary>
        /// Retrieves Person Names for particular Contact Type ID
        /// </summary>
        /// <param name="ContTypId">Contact Type ID</param>
        /// <returns>Returns the person names list for the given Contact Type ID </returns>
        public IList<PersonBE> GetPersonNames(int contTypId, int custmrID)
        {
            IList<PersonBE> Details = new List<PersonBE>();
            AssignContactsDA contactsDA = new AssignContactsDA();
            try
            {
                Details = contactsDA.GetPersonNames(contTypId,custmrID);
            }
            catch (System.Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            PersonBE selperBE = new PersonBE();
            selperBE.PERSON_ID= 0;
            selperBE.FULLNAME = "(Select)";
            Details.Insert(0, selperBE);
            return Details;
        }
        /// <summary>
        /// Retrieves particular Contact record
        /// </summary>
        /// <param name="PreAdjPgmRelId">Primary Key in Contacts Table</param>
        /// <returns>Returns Contact data</returns>
        public AssignContactsBE LoadData(int preAdjPgmRelId)
        {
            AssignContactsBE Data = new AssignContactsBE();
            try
            {
            Data = DA.Load(preAdjPgmRelId);
            }
            catch (System.Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Data;
        }
        /// <summary>
        /// Updates the existing record / Adds a new record if there are none.
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns>Returns whethew the save/update was successful</returns>
        public bool SaveContactsData(AssignContactsBE contacts)
        {
            try
            {
            if (contacts.PREM_ADJ_PGM_PERS_REL_ID > 0)
            {
                return DA.Update(contacts);
            }
            else
            {
               return DA.Add(contacts);
            }
            }
            catch (System.Exception ex)
            {
                 
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return false;
                //throw myException;
            }
             
        }

    }
}