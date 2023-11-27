using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ZDWSearchBETest and is intended
    ///to contain all ZDWSearchBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZDWSearchBETest
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
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.VALUATION_DATE = expected;
            actual = target.VALUATION_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for VAL_DATE
        ///</summary>
        [TestMethod()]
        public void VAL_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.VAL_DATE = expected;
            actual = target.VAL_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ROLE_ID
        ///</summary>
        [TestMethod()]
        public void ROLE_IDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ROLE_ID = expected;
            actual = target.ROLE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QCD_DATE
        ///</summary>
        [TestMethod()]
        public void QCD_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.QCD_DATE = expected;
            actual = target.QCD_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGRAMTYP_ID
        ///</summary>
        [TestMethod()]
        public void PROGRAMTYP_IDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PROGRAMTYP_ID = expected;
            actual = target.PROGRAMTYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGRAMTYP
        ///</summary>
        [TestMethod()]
        public void PROGRAMTYPTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PROGRAMTYP = expected;
            actual = target.PROGRAMTYP;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY_NO
        ///</summary>
        [TestMethod()]
        public void POLICY_NOTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY_NO = expected;
            actual = target.POLICY_NO;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PGM_EXP_DATE
        ///</summary>
        [TestMethod()]
        public void PGM_EXP_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PGM_EXP_DATE = expected;
            actual = target.PGM_EXP_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PGM_EFF_DATE
        ///</summary>
        [TestMethod()]
        public void PGM_EFF_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PGM_EFF_DATE = expected;
            actual = target.PGM_EFF_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERSON_ID
        ///</summary>
        [TestMethod()]
        public void PERSON_IDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PERSON_ID = expected;
            actual = target.PERSON_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICEDATE
        ///</summary>
        [TestMethod()]
        public void INVOICEDATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.INVOICEDATE = expected;
            actual = target.INVOICEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICE_NUMBER
        ///</summary>
        [TestMethod()]
        public void INVOICE_NUMBERTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.INVOICE_NUMBER = expected;
            actual = target.INVOICE_NUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICE_INQUIRY_NO
        ///</summary>
        [TestMethod()]
        public void INVOICE_INQUIRY_NOTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.INVOICE_INQUIRY_NO = expected;
            actual = target.INVOICE_INQUIRY_NO;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICE_DATE
        ///</summary>
        [TestMethod()]
        public void INVOICE_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.INVOICE_DATE = expected;
            actual = target.INVOICE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICE_AMOUNT
        ///</summary>
        [TestMethod()]
        public void INVOICE_AMOUNTTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.INVOICE_AMOUNT = expected;
            actual = target.INVOICE_AMOUNT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INTORGID
        ///</summary>
        [TestMethod()]
        public void INTORGIDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.INTORGID = expected;
            actual = target.INTORGID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INSURED_CONTACT
        ///</summary>
        [TestMethod()]
        public void INSURED_CONTACTTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.INSURED_CONTACT = expected;
            actual = target.INSURED_CONTACT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_INVOICE_NUMBER
        ///</summary>
        [TestMethod()]
        public void FNL_INVOICE_NUMBERTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_INVOICE_NUMBER = expected;
            actual = target.FNL_INVOICE_NUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_INTERNAL_KEY
        ///</summary>
        [TestMethod()]
        public void FNL_INTERNAL_KEYTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_INTERNAL_KEY = expected;
            actual = target.FNL_INTERNAL_KEY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_EXTERNAL_KEY
        ///</summary>
        [TestMethod()]
        public void FNL_EXTERNAL_KEYTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_EXTERNAL_KEY = expected;
            actual = target.FNL_EXTERNAL_KEY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_CW_KEY
        ///</summary>
        [TestMethod()]
        public void FNL_CW_KEYTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_CW_KEY = expected;
            actual = target.FNL_CW_KEY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINAL_INVOICEDATE
        ///</summary>
        [TestMethod()]
        public void FINAL_INVOICEDATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FINAL_INVOICEDATE = expected;
            actual = target.FINAL_INVOICEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINAL_INVOICE_DATE
        ///</summary>
        [TestMethod()]
        public void FINAL_INVOICE_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.FINAL_INVOICE_DATE = expected;
            actual = target.FINAL_INVOICE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXTORGID
        ///</summary>
        [TestMethod()]
        public void EXTORGIDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.EXTORGID = expected;
            actual = target.EXTORGID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRFT_INVOICE_NUMBER
        ///</summary>
        [TestMethod()]
        public void DRFT_INVOICE_NUMBERTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRFT_INVOICE_NUMBER = expected;
            actual = target.DRFT_INVOICE_NUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_INVOICEDATE
        ///</summary>
        [TestMethod()]
        public void DRAFT_INVOICEDATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRAFT_INVOICEDATE = expected;
            actual = target.DRAFT_INVOICEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_INVOICE_DATE
        ///</summary>
        [TestMethod()]
        public void DRAFT_INVOICE_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DRAFT_INVOICE_DATE = expected;
            actual = target.DRAFT_INVOICE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_INTERNAL_KEY
        ///</summary>
        [TestMethod()]
        public void DRAFT_INTERNAL_KEYTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRAFT_INTERNAL_KEY = expected;
            actual = target.DRAFT_INTERNAL_KEY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_EXTERNAL_KEY
        ///</summary>
        [TestMethod()]
        public void DRAFT_EXTERNAL_KEYTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRAFT_EXTERNAL_KEY = expected;
            actual = target.DRAFT_EXTERNAL_KEY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_CW_KEY
        ///</summary>
        [TestMethod()]
        public void DRAFT_CW_KEYTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRAFT_CW_KEY = expected;
            actual = target.DRAFT_CW_KEY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_IDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_ID = expected;
            actual = target.CUSTOMER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CFS2_NAME
        ///</summary>
        [TestMethod()]
        public void CFS2_NAMETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CFS2_NAME = expected;
            actual = target.CFS2_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BU_OFFICE
        ///</summary>
        [TestMethod()]
        public void BU_OFFICETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BU_OFFICE = expected;
            actual = target.BU_OFFICE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BROKER_ID
        ///</summary>
        [TestMethod()]
        public void BROKER_IDTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.BROKER_ID = expected;
            actual = target.BROKER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BROKER_CONTACT
        ///</summary>
        [TestMethod()]
        public void BROKER_CONTACTTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BROKER_CONTACT = expected;
            actual = target.BROKER_CONTACT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BROKER
        ///</summary>
        [TestMethod()]
        public void BROKERTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BROKER = expected;
            actual = target.BROKER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJUSTMENTDATE
        ///</summary>
        [TestMethod()]
        public void ADJUSTMENTDATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJUSTMENTDATE = expected;
            actual = target.ADJUSTMENTDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJUSTMENT_STATUS
        ///</summary>
        [TestMethod()]
        public void ADJUSTMENT_STATUSTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJUSTMENT_STATUS = expected;
            actual = target.ADJUSTMENT_STATUS;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJUSTMENT_DATE
        ///</summary>
        [TestMethod()]
        public void ADJUSTMENT_DATETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ADJUSTMENT_DATE = expected;
            actual = target.ADJUSTMENT_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACCOUNT_NUMBER
        ///</summary>
        [TestMethod()]
        public void ACCOUNT_NUMBERTest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ACCOUNT_NUMBER = expected;
            actual = target.ACCOUNT_NUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACCOUNT_NAME
        ///</summary>
        [TestMethod()]
        public void ACCOUNT_NAMETest()
        {
            ZDWSearchBE target = new ZDWSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ACCOUNT_NAME = expected;
            actual = target.ACCOUNT_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ZDWSearchBE Constructor
        ///</summary>
        [TestMethod()]
        public void ZDWSearchBEConstructorTest()
        {
            ZDWSearchBE target = new ZDWSearchBE();
            
        }
    }
}
