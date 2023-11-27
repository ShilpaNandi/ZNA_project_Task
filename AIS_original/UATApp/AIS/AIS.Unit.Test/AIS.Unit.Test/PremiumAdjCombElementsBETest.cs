using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjCombElementsBETest and is intended
    ///to contain all PremiumAdjCombElementsBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjCombElementsBETest
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
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RETRO_TAX_MULTI_RT
        ///</summary>
        [TestMethod()]
        public void RETRO_TAX_MULTI_RTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RETRO_TAX_MULTI_RT = expected;
            actual = target.RETRO_TAX_MULTI_RT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RETRO_SUBTOT_AMT
        ///</summary>
        [TestMethod()]
        public void RETRO_SUBTOT_AMTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RETRO_SUBTOT_AMT = expected;
            actual = target.RETRO_SUBTOT_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RETRO_LOS_FCTR_AMT
        ///</summary>
        [TestMethod()]
        public void RETRO_LOS_FCTR_AMTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RETRO_LOS_FCTR_AMT = expected;
            actual = target.RETRO_LOS_FCTR_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RETRO_BASIC_PREM_AMT
        ///</summary>
        [TestMethod()]
        public void RETRO_BASIC_PREM_AMTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RETRO_BASIC_PREM_AMT = expected;
            actual = target.RETRO_BASIC_PREM_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PERD_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PERD_IDTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PERD_ID = expected;
            actual = target.PREM_ADJ_PERD_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_COMB_ELEMNTS_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_COMB_ELEMNTS_IDTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_COMB_ELEMNTS_ID = expected;
            actual = target.PREM_ADJ_COMB_ELEMNTS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DEDTBL_SUBTOT_AMT
        ///</summary>
        [TestMethod()]
        public void DEDTBL_SUBTOT_AMTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.DEDTBL_SUBTOT_AMT = expected;
            actual = target.DEDTBL_SUBTOT_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DEDTBL_MIN_AMT
        ///</summary>
        [TestMethod()]
        public void DEDTBL_MIN_AMTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.DEDTBL_MIN_AMT = expected;
            actual = target.DEDTBL_MIN_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DEDTBL_MAX_LESS_AMT
        ///</summary>
        [TestMethod()]
        public void DEDTBL_MAX_LESS_AMTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.DEDTBL_MAX_LESS_AMT = expected;
            actual = target.DEDTBL_MAX_LESS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DEDTBL_MAX_AMT
        ///</summary>
        [TestMethod()]
        public void DEDTBL_MAX_AMTTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.DEDTBL_MAX_AMT = expected;
            actual = target.DEDTBL_MAX_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjCombElementsBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjCombElementsBEConstructorTest()
        {
            PremiumAdjCombElementsBE target = new PremiumAdjCombElementsBE();
            
        }
    }
}
