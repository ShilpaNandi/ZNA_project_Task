using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerContactBSTest and is intended
    ///to contain all CustomerContactBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerContactBSTest
    {
        private TestContext testContextInstance;
        static CustomerContactBE CustomerContactBE;
        static CustomerContactBS target;
        static PersonBE personBE;
        static PersonBS personBS;
        static AccountBE accountBE;
        static AccountBS accountBS;

        

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

       

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            
            personBS = new PersonBS();
            AddPersonData();
            accountBS = new AccountBS();
            AddAccountData();
            target = new CustomerContactBS();
            AddCommonData();
        }

       
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        private static void AddPersonData()
        {
            personBE = new PersonBE();
            personBE.SURNAME = "aaaa";
            personBE.FORENAME = "bbbbb";
            personBE.USERID = "XCSNQK5";
            personBE.CREATEDDATE = System.DateTime.Now;
            personBE.CREATEDUSER_ID = 1;
            personBS.Update(personBE);


        }
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "Pralaya" + System.DateTime.Now.ToString();
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }

        private static void AddCommonData()
        {
            CustomerContactBE = new CustomerContactBE();
            CustomerContactBE.CUSTOMER_ID = accountBE.CUSTMR_ID;
            CustomerContactBE.PERSON_ID = personBE.PERSON_ID;
            CustomerContactBE.ROLE_ID = 360;
            CustomerContactBE.CREATE_DATE = System.DateTime.Now;
            CustomerContactBE.CREATE_USER_ID = 1;
            target.Update(CustomerContactBE);

        }
        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            CustomerContactBS Newtarget = new CustomerContactBS();
            CustomerContactBE NewccBE = new CustomerContactBE();
            NewccBE.CUSTOMER_ID = accountBE.CUSTMR_ID;
            NewccBE.PERSON_ID = personBE.PERSON_ID;
            NewccBE.ROLE_ID = 397;
            NewccBE.CREATE_DATE = System.DateTime.Now;
            NewccBE.CREATE_USER_ID = 1;
            actual = Newtarget.Update(NewccBE);
            Assert.AreEqual(expected, actual);

        }
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            

            CustomerContactBS target = new CustomerContactBS();
            CustomerContactBE ccBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(ccBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
             target = new CustomerContactBS();
            CustomerContactBE ccBE = new CustomerContactBE();
            ccBE.CUSTOMER_ID = accountBE.CUSTMR_ID;
            ccBE.PERSON_ID = personBE.PERSON_ID;
            ccBE.ROLE_ID = 397;
            ccBE.UPDATE_DATE = System.DateTime.Now;
            ccBE.UPDATE_USER_ID = 1;
            bool expected = true;
            bool actual;
            actual = target.Update(ccBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPrimaryContactData
        ///</summary>
        [TestMethod()]
        public void getPrimaryContactDataTest()
        {
            int accountID = 1;
            int expected = 1;
            CustomerContactBE actual;
            actual = target.getPrimaryContactData(accountID);
            Assert.AreEqual(expected, actual.PERSON_ID); 

            ////
            //CustomerContactBE expected = null;
            //CustomerContactBE actual;
            //actual = target.getPrimaryContactData(personBE.PERSON_ID);
            //if (actual.IsNull()) actual = null;
            //Assert.AreNotEqual(expected, actual);

            
        }

        /// <summary>
        ///A test for getAccountResponsibilities
        ///</summary>
        [TestMethod()]
        public void getAccountResponsibilitiesTest()
        {
            int accountID = 1;
            int expected = 1;
            IList<CustomerContactBE> actual;
            actual = target.getAccountResponsibilities(accountID);
            Assert.AreNotEqual(expected, actual.Count);

        }

        /// <summary>
        ///A test for getLSSAnalystName
        ///</summary>
        [TestMethod()]
        public void getLSSAnalystNameTest()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> expected = null; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> actual;
            actual = target.getLSSAnalystName(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getInsuredContactData
        ///</summary>
        [TestMethod()]
        public void getInsuredContactDataTest1()
        {

            int accountID = 1;
            int expected = 1;
            IList<CustomerContactBE> actual;
            actual = target.getInsuredContactData(accountID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getInsuredContactData
        ///</summary>
        [TestMethod()]
        public void getInsuredContactDataTest()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            int personID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getInsuredContactData(AccountID, personID);
            Assert.AreEqual(expected, actual);
            
        }
        

            /// <summary>
        ///A test for getCustmerContactData
        ///</summary>
        [TestMethod()]
        public void AriesPersonTest()
        {

            int accountID = 1;
            int roleID = 1;
            int expected = 1;
            CustomerContactBE actual;
            actual = target.AriesPerson(accountID, roleID);
            Assert.AreEqual(expected, actual.PERSON_ID); 

            
        }
        
                /// <summary>
        ///A test for getCustmerContactData
        ///</summary>
        [TestMethod()]
        public void getCFS2NamesTest()
        {
            int accountID = 1;
            int expected = 1;
            IList<CustomerContactBE> actual;
            actual = target.getCFS2Names(accountID);
            Assert.AreNotEqual(expected, actual.Count);
            
        }
        /// <summary>
        ///A test for getCustmerContactData
        ///</summary>
        [TestMethod()]
        public void getCustmerContactDataTest()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int CUSTOMER_CONTACT_ID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getCustmerContactData(CUSTOMER_CONTACT_ID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getCustmerContact
        ///</summary>
        [TestMethod()]
        public void getCustmerContactTest()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int PERSON_ID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getCustmerContact(PERSON_ID);
            Assert.AreEqual(expected, actual);
            
        }


        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            CustomerContactBE CusCt = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Delete(CusCt);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AssignResponsibilities
        ///</summary>
        [TestMethod()]
        public void AssignResponsibilitiesTest()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int Customerid = 0; // TODO: Initialize to an appropriate value
            ArrayList responsibilities = null; // TODO: Initialize to an appropriate value
            ArrayList Personid = null; // TODO: Initialize to an appropriate value
            string errorMessage = string.Empty;
            IList<CustomerContactBE> customerContacts = null;
            target.AssignResponsibilities(Customerid, responsibilities, Personid, 
                    out errorMessage, customerContacts,false,1);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CustomerContactBS Constructor
        ///</summary>
        [TestMethod()]
        public void CustomerContactBSConstructorTest()
        {
            CustomerContactBS target = new CustomerContactBS();
            
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest1()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            CustomerContactBE CusCt = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(CusCt);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPrimaryContactData
        ///</summary>
        [TestMethod()]
        public void getPrimaryContactDataTest1()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getPrimaryContactData(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getLSSAnalystName
        ///</summary>
        [TestMethod()]
        public void getLSSAnalystNameTest1()
        {
            int accountID = 1;
            int expected = 1;
            IList<CustomerContactBE> actual;
            actual = target.getLSSAnalystName(accountID);
            Assert.AreNotEqual(expected, actual.Count);
            
            
        }

        /// <summary>
        ///A test for getInsuredContactData
        ///</summary>
        [TestMethod()]
        public void getInsuredContactDataTest3()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> expected = null; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> actual;
            actual = target.getInsuredContactData(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getInsuredContactData
        ///</summary>
        [TestMethod()]
        public void getInsuredContactDataTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            int personID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getInsuredContactData(AccountID, personID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getCustmerContactData
        ///</summary>
        [TestMethod()]
        public void getCustmerContactDataTest1()
        {
            CustomerContactBS target = new CustomerContactBS(); 
            int CUSTOMER_CONTACT_ID = 0; 
            CustomerContactBE expected = null; 
            CustomerContactBE actual;
            actual = target.getCustmerContactData(CUSTOMER_CONTACT_ID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getCustmerContact
        ///</summary>
        [TestMethod()]
        public void getCustmerContactTest1()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int PERSON_ID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getCustmerContact(PERSON_ID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getAccountResponsibilities
        ///</summary>
        [TestMethod()]
        public void getAccountResponsibilitiesTest1()
        {
            int accountID = 1;
            int expected = 1;
            IList<CustomerContactBE> actual;
            actual = target.getAccountResponsibilities(accountID);
            Assert.AreNotEqual(expected, actual.Count);
            
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest1()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            CustomerContactBE CusCt = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Delete(CusCt);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AssignResponsibilities
        ///</summary>
        [TestMethod()]
        public void AssignResponsibilitiesTest1()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int Customerid = 0; // TODO: Initialize to an appropriate value
            ArrayList responsibilities = null; // TODO: Initialize to an appropriate value
            ArrayList Personid = null; // TODO: Initialize to an appropriate value
            string errorMessage = string.Empty;
            IList<CustomerContactBE> custmrContacts = null;
            target.AssignResponsibilities(Customerid, responsibilities, Personid,
                out errorMessage, custmrContacts, false,1);
           // Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        ///// <summary>
        /////A test for CustomerContactBS Constructor
        /////</summary>
        //[TestMethod()]
        //public void CustomerContactBSConstructorTest1()
        //{
        //    CustomerContactBS target = new CustomerContactBS();
            
        //}

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            CustomerContactBE CusCt = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(CusCt);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPrimaryContactData
        ///</summary>
        [TestMethod()]
        public void getPrimaryContactDataTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getPrimaryContactData(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getLSSAnalystName
        ///</summary>
        [TestMethod()]
        public void getLSSAnalystNameTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> expected = null; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> actual;
            actual = target.getLSSAnalystName(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getInsuredContactData
        ///</summary>
        [TestMethod()]
        public void getInsuredContactDataTest5()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> expected = null; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> actual;
            actual = target.getInsuredContactData(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getInsuredContactData
        ///</summary>
        [TestMethod()]
        public void getInsuredContactDataTest4()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            int personID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getInsuredContactData(AccountID, personID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getCustmerContactData
        ///</summary>
        [TestMethod()]
        public void getCustmerContactDataTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int CUSTOMER_CONTACT_ID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getCustmerContactData(CUSTOMER_CONTACT_ID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getCustmerContact
        ///</summary>
        [TestMethod()]
        public void getCustmerContactTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int PERSON_ID = 0; // TODO: Initialize to an appropriate value
            CustomerContactBE expected = null; // TODO: Initialize to an appropriate value
            CustomerContactBE actual;
            actual = target.getCustmerContact(PERSON_ID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getAccountResponsibilities
        ///</summary>
        [TestMethod()]
        public void getAccountResponsibilitiesTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> expected = null; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> actual;
            actual = target.getAccountResponsibilities(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            CustomerContactBE CusCt = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Delete(CusCt);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AssignResponsibilities
        ///</summary>
        [TestMethod()]
        public void AssignResponsibilitiesTest2()
        {
            CustomerContactBS target = new CustomerContactBS(); // TODO: Initialize to an appropriate value
            int Customerid = 0; // TODO: Initialize to an appropriate value
            ArrayList responsibilities = null; // TODO: Initialize to an appropriate value
            ArrayList Personid = null;
            string errorMessage = string.Empty;
            IList<CustomerContactBE> customerContacts = null;
            target.AssignResponsibilities(Customerid, responsibilities, Personid, 
                out errorMessage, customerContacts, false,1);
        }

        /// <summary>
        ///A test for CustomerContactBS Constructor
        ///</summary>
        [TestMethod()]
        public void CustomerContactBSConstructorTest2()
        {
            CustomerContactBS target = new CustomerContactBS();
            
        }
    }
}
