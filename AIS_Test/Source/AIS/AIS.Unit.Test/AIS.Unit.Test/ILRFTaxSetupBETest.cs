using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ILRFTaxSetupBETest and is intended
    ///to contain all ILRFTaxSetupBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ILRFTaxSetupBETest
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
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            expected = 4;
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
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            expected = Convert.ToDateTime("12/29/2009 5:16:30 AM");
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAX_TYP_ID
        ///</summary>
        [TestMethod()]
        public void TAX_TYP_IDTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            int expected =541; // TODO: Initialize to an appropriate value
            int actual;
            target.TAX_TYP_ID = expected;
            actual = target.TAX_TYP_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAX_AMT
        ///</summary>
        [TestMethod()]
        public void TAX_AMTTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = 4500;
            target.TAX_AMT = expected;
            actual = target.TAX_AMT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_SETUP_IDTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 8; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_SETUP_ID = expected;
            actual = target.PREM_ADJ_PGM_SETUP_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 707; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for INCURRED_LOSS_REIM_FUND_TAX_TYPE
        ///</summary>
        [TestMethod()]
        public void INCURRED_LOSS_REIM_FUND_TAX_TYPETest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.INCURRED_LOSS_REIM_FUND_TAX_TYPE = expected;
            actual = target.INCURRED_LOSS_REIM_FUND_TAX_TYPE;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for INCURRED_LOSS_REIM_FUND_TAX_ID
        ///</summary>
        [TestMethod()]
        public void INCURRED_LOSS_REIM_FUND_TAX_IDTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 8; // TODO: Initialize to an appropriate value
            int actual;
            target.INCURRED_LOSS_REIM_FUND_TAX_ID = expected;
            actual = target.INCURRED_LOSS_REIM_FUND_TAX_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CUSTOMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_IDTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 106; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_ID = expected;
            actual = target.CUSTOMER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
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
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            expected = Convert.ToDateTime("12/18/2009 12:04:27 AM");
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
            ILRFTaxSetupBE target = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            expected = true;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ILRFTaxSetupBE Constructor
        ///</summary>
        [TestMethod()]
        public void ILRFTaxSetupBEConstructorTest()
        {
            ILRFTaxSetupBE target = new ILRFTaxSetupBE();
        }
    }
}
