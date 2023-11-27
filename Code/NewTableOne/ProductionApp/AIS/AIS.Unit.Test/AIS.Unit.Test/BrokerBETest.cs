using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for BrokerBETest and is intended
    ///to contain all BrokerBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class BrokerBETest
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
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
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
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FULL_NAME
        ///</summary>
        [TestMethod()]
        public void FULL_NAMETest()
        {
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULL_NAME = expected;
            actual = target.FULL_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXTRNL_ORG_ID
        ///</summary>
        [TestMethod()]
        public void EXTRNL_ORG_IDTest()
        {
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.EXTRNL_ORG_ID = expected;
            actual = target.EXTRNL_ORG_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
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
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CONTACT_TYPE_ID
        ///</summary>
        [TestMethod()]
        public void CONTACT_TYPE_IDTest()
        {
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CONTACT_TYPE_ID = expected;
            actual = target.CONTACT_TYPE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CONTACT_TYPE
        ///</summary>
        [TestMethod()]
        public void CONTACT_TYPETest()
        {
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CONTACT_TYPE = expected;
            actual = target.CONTACT_TYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            BrokerBE target = new BrokerBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BrokerBE Constructor
        ///</summary>
        [TestMethod()]
        public void BrokerBEConstructorTest()
        {
            BrokerBE target = new BrokerBE();
            
        }
    }
}
