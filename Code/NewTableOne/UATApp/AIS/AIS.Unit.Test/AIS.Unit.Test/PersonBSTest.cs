using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for PersonBSTest and is intended
    ///to contain all PersonBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PersonBSTest
    {


        private TestContext testContextInstance;
        static PersonBE personBE;
        static PersonBS target;

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
            target = new PersonBS();
            AddData();
        }

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
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddData()
        {
            personBE = new PersonBE();
            personBE.SURNAME = "aaaa";
            personBE.FORENAME = "bbbbb";
            personBE.USERID = "XCSNQK5";
            personBE.CREATEDDATE =System.DateTime.Now;
            personBE.CREATEDUSER_ID = 1;
            target.Update(personBE);

          
        }
        /// <summary>
        ///A test for Add with Data
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            bool expected = true; 
            bool actual;
            personBE = new PersonBE();
            personBE.SURNAME = "ccccc";
            personBE.FORENAME = "ddddddddd";
            personBE.USERID = "XCSNQK5";
            personBE.CREATEDDATE = System.DateTime.Now;
            personBE.CREATEDUSER_ID = 1;
            actual = (new PersonBS()).Update(personBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Add with Null Data
        ///</summary>
        [TestMethod()]
        public void AddTestWithNULL()
        {
            // personBE = new PersonBE();
            bool expected = true;
            bool actual;
            PersonBE personBE = null;
            actual = target.Update(personBE);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update with Data
        ///</summary>
        [TestMethod()]
        public void UpdatePersonTest()
        {
            bool expected = true; 
            bool actual;
            personBE.UPDATEDDATE =System.DateTime.Now;
            personBE.UPDATEDUSER_ID = 1;
            actual = target.Update(personBE);
            Assert.AreEqual(expected, actual);
        } 
        /// <summary>
        ///A test for Update with Null Data
        ///</summary>
        [TestMethod()]
        public void UpdatePersonTestWithNULL()
        {
            bool expected = false;
            bool actual;
            PersonBE personBE = null;
            actual = target.Update(personBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for IsExistsInternalContact with Data
        ///</summary>
        [TestMethod()]
        public void IsExistsInternalContactTest()
        {
            string forename = "Naresh Kumar"; 
            string surname = "Masetti"; 
            string phone = "9866198168"; 
            string email = "nmasetti@csc.com"; 
            bool expected = false; 
            bool actual;
            actual = target.IsExistsInternalContact(forename, surname, phone, email, personBE.PERSON_ID);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for IsExistsInternalContact with Null Data
        ///</summary>
        [TestMethod()]
        public void IsExistsInternalContactTestWithNull()
        {
            string forename = string.Empty; 
            string surname = string.Empty; 
            string phone = string.Empty; 
            string email = string.Empty; 
            bool expected = false; 
            bool actual;
            actual = target.IsExistsInternalContact(forename, surname, phone, email, 0);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for GetUser with Data
        ///</summary>
        [TestMethod()]
        public void GetUserTest()
        {
            string externalReference = "XCSNQK5"; 
            PersonBE expected = null; 
            PersonBE actual;
            actual = target.GetUser(externalReference);
            if (actual.PERSON_ID == 0) actual = null;
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for GetUser with Null Data
        ///</summary>
        [TestMethod()]
        public void GetUserTestWithNULL()
        {
            string externalReference = string.Empty; 
            PersonBE expected = null; 
            PersonBE actual;
            actual = target.GetUser(externalReference);
            if (actual.PERSON_ID == 0) actual = null;
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for getPersonsList
        ///</summary>
        [TestMethod()]
        public void getPersonsListTest()
        {

            //int accountID = 0;
            //int expected = 0;
            //IList<CustomerCommentsBE> actual;
            //actual = target.getRelatedComments(accountID);

            //Assert.AreEqual(expected, actual.Count);
            //PersonBS target = new PersonBS(); 
            int expected = 0; 
            IList<PersonBE> actual;
            actual = target.getPersonsList();
            Assert.AreEqual(expected, actual.Count);
        }
        /// <summary>
        ///A test for getPersonsList
        ///</summary>
        [TestMethod()]
        public void getPersonsListTestwithParam()
        {
            //PersonBS target = new PersonBS();
            int expected = 0;
            string lookupType = string.Empty;
            IList<PersonBE> actual;
            actual = target.getPersonsList(lookupType);
            Assert.AreEqual(expected, actual.Count);
        }
        /// <summary>
        ///A test for getPersonRow with Data
        ///</summary>
        [TestMethod()]
        public void getPersonRowTest()
        {
            PersonBE expected = null;
            PersonBE actual;
            actual = target.getPersonRow(personBE.PERSON_ID);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for getPersonRow with Null data
        ///</summary>
        [TestMethod()]
        public void getPersonRowTestWithNull()
        {
            int PersonID = 0; 
            PersonBE expected = null; 
            PersonBE actual;
            actual = target.getPersonRow(PersonID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }
        /////////


        /// <summary>
        ///A test for getNamesList
        ///</summary>
        [TestMethod()]
        public void getNamesListTest()
        {
            //PersonBS target = new PersonBS();
            int contactTypeID = 1;
            string contactType = "PP";
            IList<LookupBE> expected = null;
            IList<LookupBE> actual;
            actual = target.getNamesList(contactTypeID, contactType);
            Assert.AreEqual(expected, actual.Count);

        }

        /// <summary>
        ///A test for getExternalContactTypeLookUp
        ///</summary>
        [TestMethod()]
        public void getExternalContactTypeLookUpTest()
        {
            //PersonBS target = new PersonBS();
            IList<LookupBE> expected = null;
            IList<LookupBE> actual;
            actual = target.getExternalContactTypeLookUp();
            Assert.AreEqual(expected, actual.Count);

        }

        /// <summary>
        ///A test for getExtContactList
        ///</summary>
        [TestMethod()]
        public void getExtContactListTest()
        {
            //PersonBS target = new PersonBS();
            int contactType = 0;
            string strName = string.Empty;
            IList<PersonBE> expected = null;
            IList<PersonBE> actual;
            actual = target.getExtContactList(contactType, strName);
            Assert.AreEqual(expected, actual.Count);

           
        }

        /// <summary>
        ///A test for getContactsByExtOrg
        ///</summary>
        [TestMethod()]
        public void getContactsByExtOrgTest()
        {
            //PersonBS target = new PersonBS();
            int extOrgID = 0;
            IList<LookupBE> expected = null;
            IList<LookupBE> actual;
            actual = target.getContactsByExtOrg(extOrgID);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for FillPersons
        ///</summary>
        [TestMethod()]
        public void FillPersonsTest()
        {
           // PersonBS target = new PersonBS();
            IList<PersonBE> expected = null;
            IList<PersonBE> actual;
            actual = target.FillPersons();
            Assert.AreEqual(expected, actual);

        }

    }
}
