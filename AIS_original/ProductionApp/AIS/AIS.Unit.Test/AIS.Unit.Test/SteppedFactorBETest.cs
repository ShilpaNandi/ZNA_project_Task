using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for SteppedFactorBETest and is intended
    ///to contain all SteppedFactorBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class SteppedFactorBETest
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
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
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
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STEPPED_FACTOR_ID
        ///</summary>
        [TestMethod()]
        public void STEPPED_FACTOR_IDTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.STEPPED_FACTOR_ID = expected;
            actual = target.STEPPED_FACTOR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY_ID
        ///</summary>
        [TestMethod()]
        public void POLICY_IDTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.POLICY_ID = expected;
            actual = target.POLICY_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MONTHS_TO_VAL
        ///</summary>
        [TestMethod()]
        public void MONTHS_TO_VALTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.MONTHS_TO_VAL = expected;
            actual = target.MONTHS_TO_VAL;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LDF_FACTOR
        ///</summary>
        [TestMethod()]
        public void LDF_FACTORTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.LDF_FACTOR = expected;
            actual = target.LDF_FACTOR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ISACTIVE
        ///</summary>
        [TestMethod()]
        public void ISACTIVETest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ISACTIVE = expected;
            actual = target.ISACTIVE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IBNR_FACTOR
        ///</summary>
        [TestMethod()]
        public void IBNR_FACTORTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.IBNR_FACTOR = expected;
            actual = target.IBNR_FACTOR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
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
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
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
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTIVE_CHECK
        ///</summary>
        [TestMethod()]
        public void ACTIVE_CHECKTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ACTIVE_CHECK;
            
        }

        /// <summary>
        ///A test for Validate
        ///</summary>
        [TestMethod()]
        public void ValidateTest()
        {
            SteppedFactorBE target = new SteppedFactorBE(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Validate();
            Assert.AreEqual(expected, actual);
            
        }

         /// <summary>
        ///A test for SteppedFactorBE Constructor
        ///</summary>
        [TestMethod()]
        public void SteppedFactorBEConstructorTest()
        {
            SteppedFactorBE target = new SteppedFactorBE();
            
        }
    }
}
