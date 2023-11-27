using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjNYScndInjuryFundBETest and is intended
    ///to contain all PremiumAdjNYScndInjuryFundBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjNYScndInjuryFundBETest
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
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TAX_MULTI_RT
        ///</summary>
        [TestMethod()]
        public void TAX_MULTI_RTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.TAX_MULTI_RT = expected;
            actual = target.TAX_MULTI_RT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for REVD_NY_SCND_INJR_FUND_AMT
        ///</summary>
        [TestMethod()]
        public void REVD_NY_SCND_INJR_FUND_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.REVD_NY_SCND_INJR_FUND_AMT = expected;
            actual = target.REVD_NY_SCND_INJR_FUND_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREV_RSLT_AMT
        ///</summary>
        [TestMethod()]
        public void PREV_RSLT_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.PREV_RSLT_AMT = expected;
            actual = target.PREV_RSLT_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
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
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PERD_ID = expected;
            actual = target.PREM_ADJ_PERD_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_NY_SCND_INJR_FUND_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_NY_SCND_INJR_FUND_IDTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_NY_SCND_INJR_FUND_ID = expected;
            actual = target.PREM_ADJ_NY_SCND_INJR_FUND_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NY_TAX_DUE_AMT
        ///</summary>
        [TestMethod()]
        public void NY_TAX_DUE_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NY_TAX_DUE_AMT = expected;
            actual = target.NY_TAX_DUE_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NY_SCND_INJR_FUND_RT
        ///</summary>
        [TestMethod()]
        public void NY_SCND_INJR_FUND_RTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NY_SCND_INJR_FUND_RT = expected;
            actual = target.NY_SCND_INJR_FUND_RT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NY_SCND_INJR_FUND_AUDT_AMT
        ///</summary>
        [TestMethod()]
        public void NY_SCND_INJR_FUND_AUDT_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NY_SCND_INJR_FUND_AUDT_AMT = expected;
            actual = target.NY_SCND_INJR_FUND_AUDT_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NY_PREM_DISC_AMT
        ///</summary>
        [TestMethod()]
        public void NY_PREM_DISC_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NY_PREM_DISC_AMT = expected;
            actual = target.NY_PREM_DISC_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NY_ERND_RETRO_PREM_AMT
        ///</summary>
        [TestMethod()]
        public void NY_ERND_RETRO_PREM_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NY_ERND_RETRO_PREM_AMT = expected;
            actual = target.NY_ERND_RETRO_PREM_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOS_CONV_FCTR_RT
        ///</summary>
        [TestMethod()]
        public void LOS_CONV_FCTR_RTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.LOS_CONV_FCTR_RT = expected;
            actual = target.LOS_CONV_FCTR_RT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INCUR_LOS_AMT
        ///</summary>
        [TestMethod()]
        public void INCUR_LOS_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.INCUR_LOS_AMT = expected;
            actual = target.INCUR_LOS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CURR_ADJ_AMT
        ///</summary>
        [TestMethod()]
        public void CURR_ADJ_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.CURR_ADJ_AMT = expected;
            actual = target.CURR_ADJ_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COML_AGMT_ID
        ///</summary>
        [TestMethod()]
        public void COML_AGMT_IDTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.COML_AGMT_ID = expected;
            actual = target.COML_AGMT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CNVT_TOT_LOS_AMT
        ///</summary>
        [TestMethod()]
        public void CNVT_TOT_LOS_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.CNVT_TOT_LOS_AMT = expected;
            actual = target.CNVT_TOT_LOS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CNVT_LOS_AMT
        ///</summary>
        [TestMethod()]
        public void CNVT_LOS_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.CNVT_LOS_AMT = expected;
            actual = target.CNVT_LOS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BASIC_DEDTBL_PREM_AMT
        ///</summary>
        [TestMethod()]
        public void BASIC_DEDTBL_PREM_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.BASIC_DEDTBL_PREM_AMT = expected;
            actual = target.BASIC_DEDTBL_PREM_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BASIC_CNVT_LOS_AMT
        ///</summary>
        [TestMethod()]
        public void BASIC_CNVT_LOS_AMTTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.BASIC_CNVT_LOS_AMT = expected;
            actual = target.BASIC_CNVT_LOS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjNYScndInjuryFundBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjNYScndInjuryFundBEConstructorTest()
        {
            PremiumAdjNYScndInjuryFundBE target = new PremiumAdjNYScndInjuryFundBE();
            
        }
    }
}
