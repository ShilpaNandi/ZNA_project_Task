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
    /// <summary>
    /// This clas is used to interact with Person Table
    /// </summary>
    public class PersonBS : BusinessServicesBase<PersonBE, PersonDA>
    {
        #region InteralContacts
        /// <summary>
        /// Retrieves the row from Person Table based on PersonID
        /// </summary>
        /// <param name="PersonID">Person ID PrimaryKey</param>
        /// <returns>PersonBE based on PersonID</returns>
        public PersonBE getPersonRow(int PersonID)
        {
            PersonBE personBE = new PersonBE();
            personBE = DA.Load(PersonID);
            return personBE;
        }

        public IList<PersonBE> FillPersons()
        {
            IList<PersonBE> list = new List<PersonBE>();
            PersonDA Person = new PersonDA();

            try
            {
                list = Person.getPersonsList();
                PersonBE personBE = new PersonBE();
                personBE.PERSON_ID = 0;
                personBE.FULLNAME = "(Select)";
                list.Insert(0, personBE);
                
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        public IList<PersonBE> FillCRMusers()
        {

            IList<PersonBE> list = new List<PersonBE>();
            PersonDA Person = new PersonDA();

            try
            {
                list = Person.getCRMusers();
                PersonBE personBE = new PersonBE();
                personBE.PERSON_ID = 0;
                personBE.FULLNAME = "(Select)";
                list.Insert(0, personBE);

            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        
        }
        /// <summary>
        /// Retrieves Person Table List
        /// </summary>
        /// <param name=""></param>
        /// <returns>IList of type PersonBE Person Tables results</returns>
        public IList<PersonBE> getPersonsList(string strLookupType)
        {
            IList<PersonBE> list = new List<PersonBE>();
            PersonDA Person = new PersonDA();
            PersonBE PerBE = new PersonBE();

            try
            {
                list = Person.getPersonsList(strLookupType);
                //PerBE.FULLNAME = "(Select)";
               // list.Insert(0, PerBE);
            }
             
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.PERSON_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PERSON_ID;
            return list;
        }
        /// <summary>
        /// Retrieves Person Table List
        /// </summary>
        /// <param name=""></param>
        /// <returns>IList of type PersonBE Person Tables results</returns>
        public IList<PersonBE> getPersonsList()
        {
            IList<PersonBE> list = new List<PersonBE>();
            PersonDA Person = new PersonDA();
            PersonBE PerBE = new PersonBE();

            try
            {
                list = Person.getPersonsList();
                //PerBE.FULLNAME = "(Select)";
                // list.Insert(0, PerBE);
            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.PERSON_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PERSON_ID;
            return list;
        }
        /// <summary>
        /// update the information in person table
        /// </summary>
        /// <param name="PerBE">Person BE</param>
        /// <returns>Bool (True/false) i.e., updated or not </returns>
        public bool Update(PersonBE PerBE)
        {
            bool succeed = false;
            try
            {
                if (PerBE.PERSON_ID > 0) //On Update
                {
                    succeed = this.Save(PerBE);
                }
                else //On Insert
                {
                    //PerBE.PERSON_ID = this.DA.GetNextPkID().Value;
                    succeed = DA.Add(PerBE);
                }
            }
            catch (Exception ee)
            {
                return succeed;
            }
            return succeed;
        }
        #endregion
        public IList<LookupBE> getExternalContactTypeLookUp()
        {
            BLAccess BLAcc = new BLAccess();
            IList<LookupBE> LookupBEs = new List<LookupBE>();

            LookupBEs = BLAcc.GetLookUpActiveData("CONTACT TYPE", "E");

            IEnumerable<LookupBE> query = (from kj in LookupBEs
                                           where (kj.LookUpName != ExternalContactType.INSURED)
                                           select new LookupBE() { LookUpID = kj.LookUpID, LookUpName = kj.LookUpName });

            return query.ToList();
        }
      
        /// <summary>
        /// Retrieves the Current Logged-In User
        /// </summary>
        /// <param name="externalReference">External Reference Text (AZCORP ID)</param>
        /// <returns>current Logged-In User</returns>
        public PersonBE GetUser(string externalReference)
        {
            PersonDA persDA = new PersonDA();
            return persDA.GetUser(externalReference);
        }
        /// <summary>
        /// checks for Duplicate Record of PersonTable Data
        /// </summary>
        /// <param name="forename"></param>
        /// <param name="surname"></param>
        /// <param name="lookupName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="personID"></param>
        /// <returns>bool True/False</returns>
        public bool IsExistsInternalContact(string forename, string surname, string phone, string email, int personID)
        {
             PersonDA Person = new PersonDA();
             return Person.IsExistsInternalContact(forename, surname, phone, email,personID);
        }
       
        
        #region External Contacts
        public bool IsExistsExtContact(string forename, string surname, int personID, int contTypeID, int contNameID)
        {
            PersonDA Person = new PersonDA();
            if (contTypeID == 236)
                return Person.IsExistsInsuredExtContact(forename, surname, personID, contTypeID, contNameID);
            else
                return Person.IsExistsExtContact(forename, surname, personID, contTypeID, contNameID);
        }

        //}
        /// <summary>
        /// Invoked to Display all external contacts in Listview
        /// </summary>
        /// <returns>List of Person Details along with Post Address Details/Ext Contact Names</returns>
        public IList<PersonBE> getExtContactList(int contactType,string strName)
        {
            IList<PersonBE> list = new List<PersonBE>();
            PersonDA Person = new PersonDA();

            try
            {
                list = Person.getExtContactList(contactType,strName);
            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return list;
        }
        /// <summary>
        /// Invoked to Display all Insured contact Type Names 
        /// </summary>
        /// <returns>List of Insured contact Type Names </returns>
        public IList<PersonBE> getInsuredNames(int contactType)
        {
            IList<PersonBE> list = new List<PersonBE>();
            PersonDA Person = new PersonDA();

            try
            {
                list = Person.getInsuredNames(contactType);
            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return list;
        }

        /// <summary>
        /// Retrives external Organizations/Customer Names, to popup a dropdown
        /// </summary>
        /// <param name="ContactTypeID"></param>
        /// <param name="ContactType"></param>
        /// <returns></returns>
        public IList<LookupBE> getNamesList(int contactTypeID, string contactType)
        {
            PersonDA Person = new PersonDA();
            IList<LookupBE> lkp = new List<LookupBE>();
            try
            {
                //if the Selected Contact Type is INSURED then retrive the contacts list from Customer table
                if (contactType == ExternalContactType.INSURED)
                {
                    lkp = Person.getInsuredNamesList();
                }
                //else (BROKER,CAPTIVE,TPA retrive from External Organization table)
                else
                {
                    lkp = Person.getNamesList(contactTypeID);
                }
                //Adding a new row (Select) as first row
                LookupBE sellkupBE = new LookupBE();
                sellkupBE.LookUpID = 0;
                sellkupBE.LookUpName = "(Select)";
                lkp.Insert(0, sellkupBE);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }


            return lkp;
        }

        #endregion
        #region Program Period
        /// <summary>
        /// Retrives a List of Contacts basing on ExtOrgID , for poupop DropDown
        /// </summary>
        /// <param name="ExtOrgID">External Org ID</param>
        /// <returns></returns>
        public IList<LookupBE> getContactsByExtOrg(int extOrgID)
        {
            PersonDA Person = new PersonDA();
            IList<LookupBE> lkp = new List<LookupBE>();
            try
            {
                lkp = Person.getContactsByExtOrg(extOrgID);

                //Adding a new row (Select) as first row
                LookupBE sellkupBE = new LookupBE();
                sellkupBE.LookUpID = 0;
                sellkupBE.LookUpName = "(Select)";
                lkp.Insert(0, sellkupBE);

                //Adding a new row (Not Applicable) as Second row
                LookupBE sellkupBE1 = new LookupBE();
                sellkupBE1.LookUpID =  1000000;
                sellkupBE1.LookUpName = "Not Applicable";
                lkp.Insert(1, sellkupBE1);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            

            return lkp;
        }

        #endregion
    }
}
