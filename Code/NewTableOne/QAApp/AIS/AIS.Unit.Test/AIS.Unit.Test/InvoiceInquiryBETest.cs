using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for InvoiceInquiryBETest and is intended
    ///to contain all InvoiceInquiryBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvoiceInquiryBETest
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.VALUATIONDATE = expected;
            actual = target.VALUATIONDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for VALUATION_DATE
        ///</summary>
        [TestMethod()]
        public void VALUATION_DATETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.VAL_DATE = expected;
            actual = target.VAL_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEUSERID
        ///</summary>
        [TestMethod()]
        public void UPDATEUSERIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATEUSERID = expected;
            actual = target.UPDATEUSERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDATE
        ///</summary>
        [TestMethod()]
        public void UPDATEDATETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDATE = expected;
            actual = target.UPDATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TWENTYPCTQLTYCNTRLPERSID
        ///</summary>
        [TestMethod()]
        public void TWENTYPCTQLTYCNTRLPERSIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.TWENTYPCTQLTYCNTRLPERSID = expected;
            actual = target.TWENTYPCTQLTYCNTRLPERSID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TWENTYPCTQLTYCNTRLIND
        ///</summary>
        [TestMethod()]
        public void TWENTYPCTQLTYCNTRLINDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.TWENTYPCTQLTYCNTRLIND = expected;
            actual = target.TWENTYPCTQLTYCNTRLIND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TWENTYPCTQLTYCNTRLDT
        ///</summary>
        [TestMethod()]
        public void TWENTYPCTQLTYCNTRLDTTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.TWENTYPCTQLTYCNTRLDT = expected;
            actual = target.TWENTYPCTQLTYCNTRLDT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ROLE_ID
        ///</summary>
        [TestMethod()]
        public void ROLE_IDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PROGRAMTYP = expected;
            actual = target.PROGRAMTYP;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMADJID
        ///</summary>
        [TestMethod()]
        public void PREMADJIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREMADJID = expected;
            actual = target.PREMADJID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.INVOICE_AMOUNT = expected;
            actual = target.INVOICE_AMOUNT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVDUEDT
        ///</summary>
        [TestMethod()]
        public void INVDUEDTTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.INVDUEDT = expected;
            actual = target.INVDUEDT;
            Assert.AreEqual(expected, actual);
            
        }


        /// <summary>
        ///A test for INTORGID
        ///</summary>
        [TestMethod()]
        public void INTORGIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_CW_KEY = expected;
            actual = target.FNL_CW_KEY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINALMAILEDUWDT
        ///</summary>
        [TestMethod()]
        public void FINALMAILEDUWDTTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.FINALMAILEDUWDT = expected;
            actual = target.FINALMAILEDUWDT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINALINVTXT
        ///</summary>
        [TestMethod()]
        public void FINALINVTXTTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FINALINVTXT = expected;
            actual = target.FINALINVTXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINALINVDT
        ///</summary>
        [TestMethod()]
        public void FINALINVDTTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.FINALINVDT = expected;
            actual = target.FINALINVDT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINAL_INVOICEDATE
        ///</summary>
        [TestMethod()]
        public void FINAL_INVOICEDATETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRFT_INVOICE_NUMBER = expected;
            actual = target.DRFT_INVOICE_NUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFTMAILEDUWDT
        ///</summary>
        [TestMethod()]
        public void DRAFTMAILEDUWDTTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DRAFTMAILEDUWDT = expected;
            actual = target.DRAFTMAILEDUWDT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFTINVTXT
        ///</summary>
        [TestMethod()]
        public void DRAFTINVTXTTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRAFTINVTXT = expected;
            actual = target.DRAFTINVTXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFTINVDATE
        ///</summary>
        [TestMethod()]
        public void DRAFTINVDATETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DRAFTINVDATE = expected;
            actual = target.DRAFTINVDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_INVOICEDATE
        ///</summary>
        [TestMethod()]
        public void DRAFT_INVOICEDATETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_ID = expected;
            actual = target.CUSTOMER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMERID
        ///</summary>
        [TestMethod()]
        public void CUSTMERIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMERID = expected;
            actual = target.CUSTMERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEUSERID
        ///</summary>
        [TestMethod()]
        public void CREATEUSERIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATEUSERID = expected;
            actual = target.CREATEUSERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDATE
        ///</summary>
        [TestMethod()]
        public void CREATEDATETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDATE = expected;
            actual = target.CREATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CFS2_NAME
        ///</summary>
        [TestMethod()]
        public void CFS2_NAMETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BROKER = expected;
            actual = target.BROKER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJVOIDRSNID
        ///</summary>
        [TestMethod()]
        public void ADJVOIDRSNIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJVOIDRSNID = expected;
            actual = target.ADJVOIDRSNID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJUSTMENTDATE
        ///</summary>
        [TestMethod()]
        public void ADJUSTMENTDATETest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ADJUSTMENT_DATE = expected;
            actual = target.ADJUSTMENT_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJRSNRSNID
        ///</summary>
        [TestMethod()]
        public void ADJRSNRSNIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJRSNRSNID = expected;
            actual = target.ADJRSNRSNID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJPENDRSNID
        ///</summary>
        [TestMethod()]
        public void ADJPENDRSNIDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJPENDRSNID = expected;
            actual = target.ADJPENDRSNID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJPENDNGIND
        ///</summary>
        [TestMethod()]
        public void ADJPENDNGINDTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ADJPENDNGIND = expected;
            actual = target.ADJPENDNGIND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACCOUNT_NUMBER
        ///</summary>
        [TestMethod()]
        public void ACCOUNT_NUMBERTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
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
            InvoiceInquiryBE target = new InvoiceInquiryBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ACCOUNT_NAME = expected;
            actual = target.ACCOUNT_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for InvoiceInquiryBE Constructor
        ///</summary>
        [TestMethod()]
        public void InvoiceInquiryBEConstructorTest()
        {
            InvoiceInquiryBE target = new InvoiceInquiryBE();
            
        }
    }
}
