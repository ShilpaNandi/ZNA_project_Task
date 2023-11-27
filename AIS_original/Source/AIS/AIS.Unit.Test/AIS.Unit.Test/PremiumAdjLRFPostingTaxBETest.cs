using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjLRFPostingTaxBETest and is intended
    ///to contain all PremiumAdjLRFPostingTaxBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjLRFPostingTaxBETest
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
        ///A test for UPDT_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDT_USER_IDTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            expected = null;
            target.UPDT_USER_ID = expected;
            actual = target.UPDT_USER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UPDT_DT
        ///</summary>
        [TestMethod()]
        public void UPDT_DTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            expected = Convert.ToDateTime("12/18/2009 12:31:24 AM");
            target.UPDT_DT = expected;
            actual = target.UPDT_DT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAXTYPE
        ///</summary>
        [TestMethod()]
        public void TAXTYPETest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TAXTYPE = expected;
            actual = target.TAXTYPE;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAX_TYP_ID
        ///</summary>
        [TestMethod()]
        public void TAX_TYP_IDTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            expected = 541;
            target.TAX_TYP_ID = expected;
            actual = target.TAX_TYP_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PRIOR_YY_AMT
        ///</summary>
        [TestMethod()]
        public void PRIOR_YY_AMTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = 4000;
            target.PRIOR_YY_AMT = expected;
            actual = target.PRIOR_YY_AMT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PREM_ADJ_PERD_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PERD_IDTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            int expected = 20302; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PERD_ID = expected;
            actual = target.PREM_ADJ_PERD_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_LOS_REIM_FUND_POST_TAX_IDTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            int expected = 2; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID = expected;
            actual = target.PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            int expected = 600002522; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for POST_AMT
        ///</summary>
        [TestMethod()]
        public void POST_AMTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = 3990;
            target.POST_AMT = expected;
            actual = target.POST_AMT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LIM_AMT
        ///</summary>
        [TestMethod()]
        public void LIM_AMTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = null;
            target.LIM_AMT = expected;
            actual = target.LIM_AMT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CURR_AMT
        ///</summary>
        [TestMethod()]
        public void CURR_AMTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = 0;
            target.CURR_AMT = expected;
            actual = target.CURR_AMT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CRTE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CRTE_USER_IDTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            int expected = 4; // TODO: Initialize to an appropriate value
            int actual;
            target.CRTE_USER_ID = expected;
            actual = target.CRTE_USER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CRTE_DT
        ///</summary>
        [TestMethod()]
        public void CRTE_DTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            expected = Convert.ToDateTime("12/18/2009 12:31:24 AM");
            target.CRTE_DT = expected;
            actual = target.CRTE_DT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for AGGR_AMT
        ///</summary>
        [TestMethod()]
        public void AGGR_AMTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = 4000;
            target.AGGR_AMT = expected;
            actual = target.AGGR_AMT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ADJ_PRIOR_YY_AMT
        ///</summary>
        [TestMethod()]
        public void ADJ_PRIOR_YY_AMTTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = 3564;
            target.ADJ_PRIOR_YY_AMT = expected;
            actual = target.ADJ_PRIOR_YY_AMT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PremiumAdjLRFPostingTaxBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjLRFPostingTaxBEConstructorTest()
        {
            PremiumAdjLRFPostingTaxBE target = new PremiumAdjLRFPostingTaxBE();
        }
    }
}
