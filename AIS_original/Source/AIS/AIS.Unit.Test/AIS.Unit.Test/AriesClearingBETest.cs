using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AriesClearingBETest and is intended
    ///to contain all AriesClearingBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AriesClearingBETest
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
        ///A test for UPDATED_USR_ID
        ///</summary>
        [TestMethod()]
        public void UPDATED_USR_IDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATED_USR_ID = expected;
            actual = target.UPDATED_USR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATED_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATED_DATETest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATED_DATE = expected;
            actual = target.UPDATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RECON_DUE_DATE
        ///</summary>
        [TestMethod()]
        public void RECON_DUE_DATETest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.RECON_DUE_DATE = expected;
            actual = target.RECON_DUE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RECON_DATE
        ///</summary>
        [TestMethod()]
        public void RECON_DATETest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.RECON_DATE = expected;
            actual = target.RECON_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QULAITY_PERSON_ID
        ///</summary>
        [TestMethod()]
        public void QULAITY_PERSON_IDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.QULAITY_PERSON_ID = expected;
            actual = target.QULAITY_PERSON_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QUALITY_CTRL_IND
        ///</summary>
        [TestMethod()]
        public void QUALITY_CTRL_INDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.QUALITY_CTRL_IND = expected;
            actual = target.QUALITY_CTRL_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QUALITY_CONTROL_DATE
        ///</summary>
        [TestMethod()]
        public void QUALITY_CONTROL_DATETest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.QUALITY_CONTROL_DATE = expected;
            actual = target.QUALITY_CONTROL_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMIUM_ADJUSTMENT_ID
        ///</summary>
        [TestMethod()]
        public void PREMIUM_ADJUSTMENT_IDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREMIUM_ADJUSTMENT_ID = expected;
            actual = target.PREMIUM_ADJUSTMENT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMIUM_ADJUST_CLEARING_ID
        ///</summary>
        [TestMethod()]
        public void PREMIUM_ADJUST_CLEARING_IDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREMIUM_ADJUST_CLEARING_ID = expected;
            actual = target.PREMIUM_ADJUST_CLEARING_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_IDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_ID = expected;
            actual = target.CUSTOMER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATED_USER_IDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATED_USER_ID = expected;
            actual = target.CREATED_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_DATE
        ///</summary>
        [TestMethod()]
        public void CREATED_DATETest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATED_DATE = expected;
            actual = target.CREATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COMMENTS_TEXT
        ///</summary>
        [TestMethod()]
        public void COMMENTS_TEXTTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.COMMENTS_TEXT = expected;
            actual = target.COMMENTS_TEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CHECK_NUMBER_TEXT
        ///</summary>
        [TestMethod()]
        public void CHECK_NUMBER_TEXTTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CHECK_NUMBER_TEXT = expected;
            actual = target.CHECK_NUMBER_TEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BILLED_ITEM_CLEAR_DATE
        ///</summary>
        [TestMethod()]
        public void BILLED_ITEM_CLEAR_DATETest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.BILLED_ITEM_CLEAR_DATE = expected;
            actual = target.BILLED_ITEM_CLEAR_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ARIES_POST_DATE
        ///</summary>
        [TestMethod()]
        public void ARIES_POST_DATETest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ARIES_POST_DATE = expected;
            actual = target.ARIES_POST_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ARIES_PAYMENT_AMOUNT
        ///</summary>
        [TestMethod()]
        public void ARIES_PAYMENT_AMOUNTTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ARIES_PAYMENT_AMOUNT = expected;
            actual = target.ARIES_PAYMENT_AMOUNT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ARIES_COMPL_IND
        ///</summary>
        [TestMethod()]
        public void ARIES_COMPL_INDTest()
        {
            AriesClearingBE target = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ARIES_COMPL_IND = expected;
            actual = target.ARIES_COMPL_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AriesClearingBE Constructor
        ///</summary>
        [TestMethod()]
        public void AriesClearingBEConstructorTest()
        {
            AriesClearingBE target = new AriesClearingBE();
            
        }
    }
}
