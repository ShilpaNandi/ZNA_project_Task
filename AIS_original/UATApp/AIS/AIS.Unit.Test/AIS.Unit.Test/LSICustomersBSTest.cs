using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using ZurichNA.AIS.ExceptionHandling;


namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LSICustomersBSTest and is intended
    ///to contain all LSICustomersBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LSICustomersBSTest
    {

        static LSICustomersBS LSICustomerBS;
        static LSICustomerBE LSICustmrBE;
        static AccountBE AcctBE;
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            
            LSICustomerBS = new LSICustomersBS();
            AddCustomerData();
            AddLSICustmrData();
            
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
        /// a method for adding New Record in Customer when the classis initiated 
        /// </summary>
        private static void AddCustomerData()
        {
            AcctBE = new AccountBE();
            AcctBE.FULL_NM = "LSICustmrTest" + System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            (new AccountBS()).Save(AcctBE);
        }
        
        /// <summary>
        /// a method for adding New Record in Customer when the classis initiated 
        /// </summary>
        private static void AddLSICustmrData()
        {
            LSICustmrBE = new LSICustomerBE();
            LSICustmrBE.CUSTMR_ID= AcctBE.CUSTMR_ID;
            LSICustmrBE.ACTV_IND= true;
            LSICustmrBE.PRIM_IND = true;
            LSICustmrBE.FULL_NAME = "LSICustmrTest" + System.DateTime.Now.ToLongTimeString();
            LSICustmrBE.CREATE_DATE = System.DateTime.Now;
            LSICustmrBE.CREATE_USER_ID = 1;
            (new AccountBS()).Save(AcctBE);
        }

        /// <summary>
        ///A test for Update with new recoed
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            LSICustomerBE LSIBE = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            LSIBE.CUSTMR_ID = AcctBE.CUSTMR_ID;
            LSIBE.LSI_ACCT_ID = 1;
            LSIBE.CREATE_DATE = System.DateTime.Now;
            LSIBE.CREATE_USER_ID = 1;
            LSIBE.LSI_CUSTMR_ID = 1;
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(LSIBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Update with existing recoed
        ///</summary>
        [TestMethod()]
        public void UpdateTest2()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            LSICustmrBE.FULL_NAME = "TestUpdate" + System.DateTime.Now.ToLongTimeString();
            
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(LSICustmrBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for getRelatedLSIAccounts with 0
        ///</summary>
        [TestMethod()]
        public void getRelatedLSIAccountsTest()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<LSICustomerBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LSICustomerBE> actual;
            actual = target.getRelatedLSIAccounts(AccountID);
            Assert.AreNotEqual(expected, actual);
            
        }

       

        /// <summary>
        ///A test for getRelatedLSIAccounts
        ///</summary>
        [TestMethod()]
        public void getRelatedLSIAccountsTest2()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            int AccountID = AcctBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            int expected =0 ; // TODO: Initialize to an appropriate value
            IList<LSICustomerBE> actual; 
            actual = target.getRelatedLSIAccounts(AccountID);
            if (actual.Count > 0)
                expected = actual.Count;
            Assert.AreEqual(expected, actual.Count);

        }

        /// <summary>
        ///A test for getLSIAccounts
        ///</summary>
        [TestMethod()]
        public void getLSIAccountsTest1()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            string[] query = new string[1]; // TODO: Initialize to an appropriate value
            query[0] = "L";
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LSIAllCustomersBE> actual;
            actual = target.getLSIAccounts(query);
            if (actual.Count > 0)
                expected = actual.Count;
            Assert.AreEqual(expected, actual.Count);
            
        }

        /// <summary>
        ///A test for getLSIAccounts
        ///</summary>
        [TestMethod()]
        public void getLSIAccountsTest()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            IList<LSIAllCustomersBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LSIAllCustomersBE> actual;
            actual = target.getLSIAccounts();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getLSIAccount
        ///</summary>
        [TestMethod()]
        public void getLSIAccountTest()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            int LSIAccountID = 0; // TODO: Initialize to an appropriate value
            LSICustomerBE expected = null; // TODO: Initialize to an appropriate value
            LSICustomerBE actual;
            actual = target.getLSIAccount(LSIAccountID);
            Assert.AreNotEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for getDuplLSIPrimaryAccount
        ///</summary>
        [TestMethod()]
        public void getDuplLSIPrimaryAccountTest()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            bool isActive = false; // TODO: Initialize to an appropriate value
            bool Primselected = false; // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.getDuplLSIPrimaryAccount(isActive, Primselected, AccountID);
            Assert.AreEqual(expected, actual);
           
        }
        /// <summary>
        ///A test for getDuplLSIPrimaryAccount with data
        ///</summary>
        [TestMethod()]
        public void getDuplLSIPrimaryAccountTestwithdata()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            bool isActive = true; // TODO: Initialize to an appropriate value
            bool Primselected = true; // TODO: Initialize to an appropriate value
            int AccountID = AcctBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.getDuplLSIPrimaryAccount(isActive, Primselected, AccountID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for CheckDuplicateLSIAccountName
        ///</summary>
        [TestMethod()]
        public void CheckDuplicateLSIAccountNameTest()
        {
            LSICustomersBS target = new LSICustomersBS(); // TODO: Initialize to an appropriate value
            int LSIAccountID = 0; // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CheckDuplicateLSIAccountName(LSIAccountID, AccountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSICustomersBS Constructor
        ///</summary>
        [TestMethod()]
        public void LSICustomersBSConstructorTest()
        {
            LSICustomersBS target = new LSICustomersBS();
            
        }
    }
}
