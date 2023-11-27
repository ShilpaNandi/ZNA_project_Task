using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ILRFFormulaBETest and is intended
    ///to contain all ILRFFormulaBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ILRFFormulaBETest
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
        ///A test for USE_RESERVE_LOSS_INDICATOR
        ///</summary>
        [TestMethod()]
        public void USE_RESERVE_LOSS_INDICATORTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.USE_RESERVE_LOSS_INDICATOR = expected;
            actual = target.USE_RESERVE_LOSS_INDICATOR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR
        ///</summary>
        [TestMethod()]
        public void USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATORTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = expected;
            actual = target.USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for USE_PAID_LOSS_INDICATOR
        ///</summary>
        [TestMethod()]
        public void USE_PAID_LOSS_INDICATORTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.USE_PAID_LOSS_INDICATOR = expected;
            actual = target.USE_PAID_LOSS_INDICATOR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR
        ///</summary>
        [TestMethod()]
        public void USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATORTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = expected;
            actual = target.USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
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
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGRAMPERIOD_ID
        ///</summary>
        [TestMethod()]
        public void PROGRAMPERIOD_IDTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PROGRAMPERIOD_ID = expected;
            actual = target.PROGRAMPERIOD_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOSS_REIM_FUND_FACTOR_TYPE_ID
        ///</summary>
        [TestMethod()]
        public void LOSS_REIM_FUND_FACTOR_TYPE_IDTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LOSS_REIM_FUND_FACTOR_TYPE_ID = expected;
            actual = target.LOSS_REIM_FUND_FACTOR_TYPE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOSS_REIM_FUND_FACTOR_TYPE
        ///</summary>
        [TestMethod()]
        public void LOSS_REIM_FUND_FACTOR_TYPETest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LOSS_REIM_FUND_FACTOR_TYPE = expected;
            actual = target.LOSS_REIM_FUND_FACTOR_TYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INCURRED_LOSS_REIM_FUND_FRMLA_ID
        ///</summary>
        [TestMethod()]
        public void INCURRED_LOSS_REIM_FUND_FRMLA_IDTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.INCURRED_LOSS_REIM_FUND_FRMLA_ID = expected;
            actual = target.INCURRED_LOSS_REIM_FUND_FRMLA_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_IDTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
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
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
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
            ILRFFormulaBE target = new ILRFFormulaBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ILRFFormulaBE Constructor
        ///</summary>
        [TestMethod()]
        public void ILRFFormulaBEConstructorTest()
        {
            ILRFFormulaBE target = new ILRFFormulaBE();
            
        }
    }
}
