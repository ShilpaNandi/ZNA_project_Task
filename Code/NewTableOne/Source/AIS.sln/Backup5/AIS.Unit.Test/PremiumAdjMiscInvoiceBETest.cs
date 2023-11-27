using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjMiscInvoiceBETest and is intended
    ///to contain all PremiumAdjMiscInvoiceBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjMiscInvoiceBETest
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
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PERD_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PERD_IDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PERD_ID = expected;
            actual = target.PREM_ADJ_PERD_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_MISC_INVC_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_MISC_INVC_IDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_MISC_INVC_ID = expected;
            actual = target.PREM_ADJ_MISC_INVC_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POSTTRNSTYPE
        ///</summary>
        [TestMethod()]
        public void POSTTRNSTYPETest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POSTTRNSTYPE = expected;
            actual = target.POSTTRNSTYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PostTrnsTyp
        ///</summary>
        [TestMethod()]
        public void PostTrnsTypTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            PostingTransactionTypeBE actual;
            actual = target.PostTrnsTyp;
            
        }

        /// <summary>
        ///A test for POST_TRANS_TYP_ID
        ///</summary>
        [TestMethod()]
        public void POST_TRANS_TYP_IDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.POST_TRANS_TYP_ID = expected;
            actual = target.POST_TRANS_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POST_AMT
        ///</summary>
        [TestMethod()]
        public void POST_AMTTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.POST_AMT = expected;
            actual = target.POST_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICYNUMBER
        ///</summary>
        [TestMethod()]
        public void POLICYNUMBERTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICYNUMBER = expected;
            actual = target.POLICYNUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POL_SYM_TXT
        ///</summary>
        [TestMethod()]
        public void POL_SYM_TXTTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POL_SYM_TXT = expected;
            actual = target.POL_SYM_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POL_REQR_IND
        ///</summary>
        [TestMethod()]
        public void POL_REQR_INDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.POL_REQR_IND = expected;
            actual = target.POL_REQR_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POL_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void POL_NBR_TXTTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POL_NBR_TXT = expected;
            actual = target.POL_NBR_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POL_MODULUS_TXT
        ///</summary>
        [TestMethod()]
        public void POL_MODULUS_TXTTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POL_MODULUS_TXT = expected;
            actual = target.POL_MODULUS_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MISC_POSTS_IND
        ///</summary>
        [TestMethod()]
        public void MISC_POSTS_INDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.MISC_POSTS_IND = expected;
            actual = target.MISC_POSTS_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_SUMRY_POST_FLAG_IND
        ///</summary>
        [TestMethod()]
        public void ADJ_SUMRY_POST_FLAG_INDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ADJ_SUMRY_POST_FLAG_IND = expected;
            actual = target.ADJ_SUMRY_POST_FLAG_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjMiscInvoiceBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjMiscInvoiceBEConstructorTest()
        {
            PremiumAdjMiscInvoiceBE target = new PremiumAdjMiscInvoiceBE();
            
        }
    }
}
