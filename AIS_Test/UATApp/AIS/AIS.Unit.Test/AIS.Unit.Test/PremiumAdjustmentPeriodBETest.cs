using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentPeriodBETest and is intended
    ///to contain all PremiumAdjustmentPeriodBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentPeriodBETest
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
        ///A test for VALUATIONDATE
        ///</summary>
        [TestMethod()]
        public void VALUATIONDATETest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.VALUATIONDATE = expected;
            actual = target.VALUATIONDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for START_DATE
        ///</summary>
        [TestMethod()]
        public void START_DATETest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.START_DATE = expected;
            actual = target.START_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for REG_CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void REG_CUSTMR_IDTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.REG_CUSTMR_ID = expected;
            actual = target.REG_CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGRAM_PERIOD
        ///</summary>
        [TestMethod()]
        public void PROGRAM_PERIODTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PROGRAM_PERIOD = expected;
            actual = target.PROGRAM_PERIOD;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for END_DATE
        ///</summary>
        [TestMethod()]
        public void END_DATETest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.END_DATE = expected;
            actual = target.END_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_NAME
        ///</summary>
        [TestMethod()]
        public void CUSTMR_NAMETest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CUSTMR_NAME = expected;
            actual = target.CUSTMR_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_TYPE
        ///</summary>
        [TestMethod()]
        public void ADJ_TYPETest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJ_TYPE = expected;
            actual = target.ADJ_TYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void ADJ_NBR_TXTTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJ_NBR_TXT = expected;
            actual = target.ADJ_NBR_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_NBR
        ///</summary>
        [TestMethod()]
        public void ADJ_NBRTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJ_NBR = expected;
            actual = target.ADJ_NBR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmentPeriodBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmentPeriodBEConstructorTest()
        {
            PremiumAdjustmentPeriodBE target = new PremiumAdjustmentPeriodBE();
            
        }
    }
}
