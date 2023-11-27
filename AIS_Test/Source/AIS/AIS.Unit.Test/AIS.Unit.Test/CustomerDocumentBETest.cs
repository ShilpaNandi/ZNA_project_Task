using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerDocumentBETest and is intended
    ///to contain all CustomerDocumentBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerDocumentBETest
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
        ///A test for VALUATION_DATE
        ///</summary>
        [TestMethod()]
        public void VALUATION_DATETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.VALUATION_DATE = expected;
            actual = target.VALUATION_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATED_USR_ID
        ///</summary>
        [TestMethod()]
        public void UPDATED_USR_IDTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
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
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATED_DATE = expected;
            actual = target.UPDATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TWENTY_PER_QC
        ///</summary>
        [TestMethod()]
        public void TWENTY_PER_QCTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.TWENTY_PER_QC = expected;
            actual = target.TWENTY_PER_QC;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RETRO_ADJ_AMOUNT
        ///</summary>
        [TestMethod()]
        public void RETRO_ADJ_AMOUNTTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RETRO_ADJ_AMOUNT = expected;
            actual = target.RETRO_ADJ_AMOUNT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RESPONSIBLE_PERS_ID
        ///</summary>
        [TestMethod()]
        public void RESPONSIBLE_PERS_IDTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.RESPONSIBLE_PERS_ID = expected;
            actual = target.RESPONSIBLE_PERS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RECEVD_DATE
        ///</summary>
        [TestMethod()]
        public void RECEVD_DATETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.RECEVD_DATE = expected;
            actual = target.RECEVD_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QUALITY_CNTRL_DATE
        ///</summary>
        [TestMethod()]
        public void QUALITY_CNTRL_DATETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.QUALITY_CNTRL_DATE = expected;
            actual = target.QUALITY_CNTRL_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGM_EFF_DATE
        ///</summary>
        [TestMethod()]
        public void PROGM_EFF_DATETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PROGM_EFF_DATE = expected;
            actual = target.PROGM_EFF_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROG_EXP_DATE
        ///</summary>
        [TestMethod()]
        public void PROG_EXP_DATETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PROG_EXP_DATE = expected;
            actual = target.PROG_EXP_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Non_Ais_id
        ///</summary>
        [TestMethod()]
        public void Non_Ais_idTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.Non_Ais_id = expected;
            actual = target.Non_Ais_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FORM_NAME
        ///</summary>
        [TestMethod()]
        public void FORM_NAMETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FORM_NAME = expected;
            actual = target.FORM_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FORM_ID
        ///</summary>
        [TestMethod()]
        public void FORM_IDTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.FORM_ID = expected;
            actual = target.FORM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ENTRY_DATE
        ///</summary>
        [TestMethod()]
        public void ENTRY_DATETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ENTRY_DATE = expected;
            actual = target.ENTRY_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMER_DOCUMENT_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_DOCUMENT_IDTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_DOCUMENT_ID = expected;
            actual = target.CUSTOMER_DOCUMENT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_USR_ID
        ///</summary>
        [TestMethod()]
        public void CREATED_USR_IDTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATED_USR_ID = expected;
            actual = target.CREATED_USR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_DATE
        ///</summary>
        [TestMethod()]
        public void CREATED_DATETest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATED_DATE = expected;
            actual = target.CREATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COMMENTS
        ///</summary>
        [TestMethod()]
        public void COMMENTSTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.COMMENTS = expected;
            actual = target.COMMENTS;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for cash_flw_spl_id
        ///</summary>
        [TestMethod()]
        public void cash_flw_spl_idTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.cash_flw_spl_id = expected;
            actual = target.cash_flw_spl_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for bu_off_id
        ///</summary>
        [TestMethod()]
        public void bu_off_idTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.bu_off_id = expected;
            actual = target.bu_off_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CustomerDocumentBE Constructor
        ///</summary>
        [TestMethod()]
        public void CustomerDocumentBEConstructorTest()
        {
            CustomerDocumentBE target = new CustomerDocumentBE();
            
        }
    }
}
