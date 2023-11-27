using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LSICustomerBETest and is intended
    ///to contain all LSICustomerBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class LSICustomerBETest
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
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
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
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PRIM_IND
        ///</summary>
        [TestMethod()]
        public void PRIM_INDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.PRIM_IND = expected;
            actual = target.PRIM_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSI_CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void LSI_CUSTMR_IDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LSI_CUSTMR_ID = expected;
            actual = target.LSI_CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSI_ACCT_ID
        ///</summary>
        [TestMethod()]
        public void LSI_ACCT_IDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LSI_ACCT_ID = expected;
            actual = target.LSI_ACCT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IS_PRIM_IND
        ///</summary>
        [TestMethod()]
        public void IS_PRIM_INDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.IS_PRIM_IND;
            
        }

        /// <summary>
        ///A test for IS_ACTV_IND
        ///</summary>
        [TestMethod()]
        public void IS_ACTV_INDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.IS_ACTV_IND;
            
        }

        /// <summary>
        ///A test for FULL_NAME
        ///</summary>
        [TestMethod()]
        public void FULL_NAMETest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULL_NAME = expected;
            actual = target.FULL_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
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
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACC
        ///</summary>
        [TestMethod()]
        public void ACCTest()
        {
            LSICustomerBE target = new LSICustomerBE(); // TODO: Initialize to an appropriate value
            AccountBE actual;
            actual = target.ACC;
            
        }

        /// <summary>
        ///A test for LSICustomerBE Constructor
        ///</summary>
        [TestMethod()]
        public void LSICustomerBEConstructorTest()
        {
            LSICustomerBE target = new LSICustomerBE();
            
        }
    }
}
