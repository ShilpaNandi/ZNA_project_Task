using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ApplWebPageAudtBETest and is intended
    ///to contain all ApplWebPageAudtBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApplWebPageAudtBETest
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
        ///A test for WEB_PAGE_ID
        ///</summary>
        [TestMethod()]
        public void WEB_PAGE_IDTest()
        {
            ApplWebPageAudtBE target = new ApplWebPageAudtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.WEB_PAGE_ID = expected;
            actual = target.WEB_PAGE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for WEB_PAGE_CNTRL_TXT
        ///</summary>
        [TestMethod()]
        public void WEB_PAGE_CNTRL_TXTTest()
        {
            ApplWebPageAudtBE target = new ApplWebPageAudtBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.WEB_PAGE_CNTRL_TXT = expected;
            actual = target.WEB_PAGE_CNTRL_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            ApplWebPageAudtBE target = new ApplWebPageAudtBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            ApplWebPageAudtBE target = new ApplWebPageAudtBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
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
            ApplWebPageAudtBE target = new ApplWebPageAudtBE(); // TODO: Initialize to an appropriate value
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
            ApplWebPageAudtBE target = new ApplWebPageAudtBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for APLCTN_WEB_PAGE_AUDT_ID
        ///</summary>
        [TestMethod()]
        public void APLCTN_WEB_PAGE_AUDT_IDTest()
        {
            ApplWebPageAudtBE target = new ApplWebPageAudtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.APLCTN_WEB_PAGE_AUDT_ID = expected;
            actual = target.APLCTN_WEB_PAGE_AUDT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ApplWebPageAudtBE Constructor
        ///</summary>
        [TestMethod()]
        public void ApplWebPageAudtBEConstructorTest()
        {
            ApplWebPageAudtBE target = new ApplWebPageAudtBE();
            
        }
    }
}
