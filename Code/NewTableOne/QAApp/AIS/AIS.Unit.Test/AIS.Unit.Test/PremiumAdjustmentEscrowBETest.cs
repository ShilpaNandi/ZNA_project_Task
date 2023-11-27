using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentEscrowBETest and is intended
    ///to contain all PremiumAdjustmentEscrowBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentEscrowBETest
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
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TOT_AMT
        ///</summary>
        [TestMethod()]
        public void TOT_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.TOT_AMT = expected;
            actual = target.TOT_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_SETUP_IDTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PERD_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PERD_IDTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PERD_ID = expected;
            actual = target.PREM_ADJ_PERD_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PARMET_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PARMET_SETUP_IDTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PARMET_SETUP_ID = expected;
            actual = target.PREM_ADJ_PARMET_SETUP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOS_BASE_ASSES_PREV_BILED_AMT
        ///</summary>
        [TestMethod()]
        public void LOS_BASE_ASSES_PREV_BILED_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.LOS_BASE_ASSES_PREV_BILED_AMT = expected;
            actual = target.LOS_BASE_ASSES_PREV_BILED_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOS_BASE_ASSES_DEPST_AMT
        ///</summary>
        [TestMethod()]
        public void LOS_BASE_ASSES_DEPST_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.LOS_BASE_ASSES_DEPST_AMT = expected;
            actual = target.LOS_BASE_ASSES_DEPST_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOS_BASE_ASSES_AMT
        ///</summary>
        [TestMethod()]
        public void LOS_BASE_ASSES_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.LOS_BASE_ASSES_AMT = expected;
            actual = target.LOS_BASE_ASSES_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INCUR_LOS_REIM_FUND_PREVIOUSLY_BILED_AMT
        ///</summary>
        [TestMethod()]
        public void INCUR_LOS_REIM_FUND_PREVIOUSLY_BILED_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.INCUR_LOS_REIM_FUND_PREVIOUSLY_BILED_AMT = expected;
            actual = target.INCUR_LOS_REIM_FUND_PREVIOUSLY_BILED_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INCUR_LOS_REIM_FUND_LIM_AMT
        ///</summary>
        [TestMethod()]
        public void INCUR_LOS_REIM_FUND_LIM_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.INCUR_LOS_REIM_FUND_LIM_AMT = expected;
            actual = target.INCUR_LOS_REIM_FUND_LIM_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INCUR_LOS_REIM_FUND_AMT
        ///</summary>
        [TestMethod()]
        public void INCUR_LOS_REIM_FUND_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.INCUR_LOS_REIM_FUND_AMT = expected;
            actual = target.INCUR_LOS_REIM_FUND_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ESCR_PREVIOUSULY_BILED_AMT
        ///</summary>
        [TestMethod()]
        public void ESCR_PREVIOUSULY_BILED_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ESCR_PREVIOUSULY_BILED_AMT = expected;
            actual = target.ESCR_PREVIOUSULY_BILED_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ESCR_AMT
        ///</summary>
        [TestMethod()]
        public void ESCR_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ESCR_AMT = expected;
            actual = target.ESCR_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ESCR_ADJ_PAID_LOS_AMT
        ///</summary>
        [TestMethod()]
        public void ESCR_ADJ_PAID_LOS_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ESCR_ADJ_PAID_LOS_AMT = expected;
            actual = target.ESCR_ADJ_PAID_LOS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ESCR_ADJ_AMT
        ///</summary>
        [TestMethod()]
        public void ESCR_ADJ_AMTTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ESCR_ADJ_AMT = expected;
            actual = target.ESCR_ADJ_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_PARMET_TYP_ID
        ///</summary>
        [TestMethod()]
        public void ADJ_PARMET_TYP_IDTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJ_PARMET_TYP_ID = expected;
            actual = target.ADJ_PARMET_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmentEscrowBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmentEscrowBEConstructorTest()
        {
            PremiumAdjustmentEscrowBE target = new PremiumAdjustmentEscrowBE();
            
        }
    }
}
