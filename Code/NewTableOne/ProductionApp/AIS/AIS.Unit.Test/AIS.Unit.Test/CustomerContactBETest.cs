using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerContactBETest and is intended
    ///to contain all CustomerContactBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerContactBETest
    {


        private TestContext testContextInstance;

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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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


        /// <summary>
        ///A test for ZIP_CODE
        ///</summary>
        [TestMethod()]
        public void ZIP_CODETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ZIP_CODE;
            
        }

        /// <summary>
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATE_USER_ID = expected;
            actual = target.UPDATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATE_DATETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STATE_TXT
        ///</summary>
        [TestMethod()]
        public void STATE_TXTTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.STATE_TXT;
            
        }

        /// <summary>
        ///A test for ROLE_ID
        ///</summary>
        [TestMethod()]
        public void ROLE_IDTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ROLE_ID = expected;
            actual = target.ROLE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Resplookup
        ///</summary>
        [TestMethod()]
        public void ResplookupTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.Resplookup;
            
        }

        /// <summary>
        ///A test for RESP_NAME
        ///</summary>
        [TestMethod()]
        public void RESP_NAMETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.RESP_NAME = expected;
            actual = target.RESP_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POSTALADDRESS
        ///</summary>
        [TestMethod()]
        public void POSTALADDRESSTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.POSTALADDRESS;
            
        }

        /// <summary>
        ///A test for POST_ADD_BE
        ///</summary>
        [TestMethod()]
        public void POST_ADD_BETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            PostalAddressBE expected = null; // TODO: Initialize to an appropriate value
            PostalAddressBE actual;
            target.POST_ADD_BE = expected;
            actual = target.POST_ADD_BE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERSON_ID
        ///</summary>
        [TestMethod()]
        public void PERSON_IDTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PERSON_ID = expected;
            actual = target.PERSON_ID.Value;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PER
        ///</summary>
        [TestMethod()]
        public void PERTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            PersonBE actual;
            actual = target.PER;
            
        }

        /// <summary>
        ///A test for LOOKUPTYPE
        ///</summary>
        [TestMethod()]
        public void LOOKUPTYPETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LOOKUPTYPE = expected;
            actual = target.LOOKUPTYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LastName
        ///</summary>
        [TestMethod()]
        public void LastNameTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LastName = expected;
            actual = target.LastName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FULLNAME
        ///</summary>
        [TestMethod()]
        public void FULLNAMETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULLNAME = expected;
            actual = target.FULLNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FirstName
        ///</summary>
        [TestMethod()]
        public void FirstNameTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FirstName = expected;
            actual = target.FirstName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DISPLAYTEXT
        ///</summary>
        [TestMethod()]
        public void DISPLAYTEXTTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DISPLAYTEXT;
            
        }

        /// <summary>
        ///A test for CUSTOMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_IDTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_ID = expected;
            actual = target.CUSTOMER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMER_CONTACT_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_CONTACT_IDTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_CONTACT_ID = expected;
            actual = target.CUSTOMER_CONTACT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATE_USER_ID = expected;
            actual = target.CREATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_DATE
        ///</summary>
        [TestMethod()]
        public void CREATE_DATETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CITY
        ///</summary>
        [TestMethod()]
        public void CITYTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.CITY;
            
        }

        /// <summary>
        ///A test for ADDRESS2
        ///</summary>
        [TestMethod()]
        public void ADDRESS2Test()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ADDRESS2;
            
        }

        /// <summary>
        ///A test for ADDRESS1
        ///</summary>
        [TestMethod()]
        public void ADDRESS1Test()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ADDRESS1;
            
        }

        /// <summary>
        ///A test for ACCOUNTNAME
        ///</summary>
        [TestMethod()]
        public void ACCOUNTNAMETest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ACCOUNTNAME = expected;
            actual = target.ACCOUNTNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACC
        ///</summary>
        [TestMethod()]
        public void ACCTest()
        {
            CustomerContactBE target = new CustomerContactBE(); // TODO: Initialize to an appropriate value
            AccountBE actual;
            actual = target.ACC;
            
        }

        /// <summary>
        ///A test for CustomerContactBE Constructor
        ///</summary>
        [TestMethod()]
        public void CustomerContactBEConstructorTest()
        {
            CustomerContactBE target = new CustomerContactBE();
            
        }
    }
}
