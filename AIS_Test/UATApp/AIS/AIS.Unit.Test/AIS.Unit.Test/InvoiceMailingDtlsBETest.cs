using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for InvoiceMailingDtlsBETest and is intended
    ///to contain all InvoiceMailingDtlsBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvoiceMailingDtlsBETest
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
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.VALUATION_DATE = expected;
            actual = target.VALUATION_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UW_REV_ID
        ///</summary>
        [TestMethod()]
        public void UW_REV_IDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.UW_REV_ID = expected;
            actual = target.UW_REV_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UW_RESP_DT
        ///</summary>
        [TestMethod()]
        public void UW_RESP_DTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UW_RESP_DT = expected;
            actual = target.UW_RESP_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UW_RESP
        ///</summary>
        [TestMethod()]
        public void UW_RESPTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.UW_RESP = expected;
            actual = target.UW_RESP;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEUSERID
        ///</summary>
        [TestMethod()]
        public void UPDATEUSERIDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
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
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDATE = expected;
            actual = target.UPDATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }



        /// <summary>
        ///A test for INV_DUE_DT
        ///</summary>
        [TestMethod()]
        public void INV_DUE_DTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.INV_DUE_DT = expected;
            actual = target.INV_DUE_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINAL_MAILED_UW_DT
        ///</summary>
        [TestMethod()]
        public void FINAL_MAILED_UW_DTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.FINAL_MAILED_UW_DT = expected;
            actual = target.FINAL_MAILED_UW_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINAL_INV_TXT
        ///</summary>
        [TestMethod()]
        public void FINAL_INV_TXTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FINAL_INV_TXT = expected;
            actual = target.FINAL_INV_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINAL_INV_DT
        ///</summary>
        [TestMethod()]
        public void FINAL_INV_DTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.FINAL_INV_DT = expected;
            actual = target.FINAL_INV_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINAL_EMAIL_DT
        ///</summary>
        [TestMethod()]
        public void FINAL_EMAIL_DTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.FINAL_EMAIL_DT = expected;
            actual = target.FINAL_EMAIL_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRFT_INTRNL_PDF_ZDW_KEY_TXT
        ///</summary>
        [TestMethod()]
        public void DRFT_INTRNL_PDF_ZDW_KEY_TXTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRFT_INTRNL_PDF_ZDW_KEY_TXT = expected;
            actual = target.DRFT_INTRNL_PDF_ZDW_KEY_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRFT_EXTRNL_PDF_ZDW_KEY_TXT
        ///</summary>
        [TestMethod()]
        public void DRFT_EXTRNL_PDF_ZDW_KEY_TXTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRFT_EXTRNL_PDF_ZDW_KEY_TXT = expected;
            actual = target.DRFT_EXTRNL_PDF_ZDW_KEY_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT
        ///</summary>
        [TestMethod()]
        public void DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT = expected;
            actual = target.DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_MAILED_UW_DT
        ///</summary>
        [TestMethod()]
        public void DRAFT_MAILED_UW_DTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DRAFT_MAILED_UW_DT = expected;
            actual = target.DRAFT_MAILED_UW_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_INV_TXT
        ///</summary>
        [TestMethod()]
        public void DRAFT_INV_TXTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRAFT_INV_TXT = expected;
            actual = target.DRAFT_INV_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFT_INV_DATE
        ///</summary>
        [TestMethod()]
        public void DRAFT_INV_DATETest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DRAFT_INV_DATE = expected;
            actual = target.DRAFT_INV_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMRNM
        ///</summary>
        [TestMethod()]
        public void CUSTMRNMTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CUSTMRNM = expected;
            actual = target.CUSTMRNM;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMER_IDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMER_ID = expected;
            actual = target.CUSTMER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEUSERID
        ///</summary>
        [TestMethod()]
        public void CREATEUSERIDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
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
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDATE = expected;
            actual = target.CREATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CMMNT_TXT
        ///</summary>
        [TestMethod()]
        public void CMMNT_TXTTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CMMNT_TXT = expected;
            actual = target.CMMNT_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CALC_ID
        ///</summary>
        [TestMethod()]
        public void CALC_IDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CALC_ID = expected;
            actual = target.CALC_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BUOFC
        ///</summary>
        [TestMethod()]
        public void BUOFCTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BUOFC = expected;
            actual = target.BUOFC;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BROKER
        ///</summary>
        [TestMethod()]
        public void BROKERTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BROKER = expected;
            actual = target.BROKER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_STS_TYP_ID
        ///</summary>
        [TestMethod()]
        public void ADJ_STS_TYP_IDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJ_STS_TYP_ID = expected;
            actual = target.ADJ_STS_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_STS_ID
        ///</summary>
        [TestMethod()]
        public void ADJ_STS_IDTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ADJ_STS_ID = expected;
            actual = target.ADJ_STS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for InvoiceMailingDtlsBE Constructor
        ///</summary>
        [TestMethod()]
        public void InvoiceMailingDtlsBEConstructorTest()
        {
            InvoiceMailingDtlsBE target = new InvoiceMailingDtlsBE();
            
        }
    }
}
