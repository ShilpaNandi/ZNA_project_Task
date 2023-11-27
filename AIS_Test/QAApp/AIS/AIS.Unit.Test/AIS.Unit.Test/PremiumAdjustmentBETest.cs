using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentBETest and is intended
    ///to contain all PremiumAdjustmentBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentBETest
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
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.VALUATIONDATE = expected;
            actual = target.VALUATIONDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for VALN_DT
        ///</summary>
        [TestMethod()]
        public void VALN_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.VALN_DT = expected;
            actual = target.VALN_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDT_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDT_USER_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDT_USER_ID = expected;
            actual = target.UPDT_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDT_DT
        ///</summary>
        [TestMethod()]
        public void UPDT_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDT_DT = expected;
            actual = target.UPDT_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TWENTY_PCT_QLTY_CNTRL_PERS_ID
        ///</summary>
        [TestMethod()]
        public void TWENTY_PCT_QLTY_CNTRL_PERS_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.TWENTY_PCT_QLTY_CNTRL_PERS_ID = expected;
            actual = target.TWENTY_PCT_QLTY_CNTRL_PERS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TWENTY_PCT_QLTY_CNTRL_IND
        ///</summary>
        [TestMethod()]
        public void TWENTY_PCT_QLTY_CNTRL_INDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.TWENTY_PCT_QLTY_CNTRL_IND = expected;
            actual = target.TWENTY_PCT_QLTY_CNTRL_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TWENTY_PCT_QLTY_CNTRL_DT
        ///</summary>
        [TestMethod()]
        public void TWENTY_PCT_QLTY_CNTRL_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.TWENTY_PCT_QLTY_CNTRL_DT = expected;
            actual = target.TWENTY_PCT_QLTY_CNTRL_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for REL_PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void REL_PREM_ADJ_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.REL_PREM_ADJ_ID = expected;
            actual = target.REL_PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RECON_COUNT
        ///</summary>
        //[TestMethod()]
        //public void RECON_COUNTTest()
        //{
        //    PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
        //    Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
        //    Nullable<int> actual;
        //    target.RECON_COUNT = expected;
        //    actual = target.RECON_COUNT;
        //    Assert.AreEqual(expected, actual);
            
        //}

        /// <summary>
        ///A test for QLTY_CNTRL_DT
        ///</summary>
        [TestMethod()]
        public void QLTY_CNTRL_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.QLTY_CNTRL_DT = expected;
            actual = target.QLTY_CNTRL_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGRAMPERIOD_STDT
        ///</summary>
        [TestMethod()]
        public void PROGRAMPERIOD_STDTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PROGRAMPERIOD_STDT = expected;
            actual = target.PROGRAMPERIOD_STDT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGRAMPERIOD_ENDT
        ///</summary>
        [TestMethod()]
        public void PROGRAMPERIOD_ENDTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PROGRAMPERIOD_ENDT = expected;
            actual = target.PROGRAMPERIOD_ENDT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMIUM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREMIUM_ADJ_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREMIUM_ADJ_ID = expected;
            actual = target.PREMIUM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERS_ID
        ///</summary>
        [TestMethod()]
        public void PERS_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PERS_ID = expected;
            actual = target.PERS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVC_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void INVC_NBR_TXTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.INVC_NBR_TXT = expected;
            actual = target.INVC_NBR_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVC_DUE_DT
        ///</summary>
        [TestMethod()]
        public void INVC_DUE_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.INVC_DUE_DT = expected;
            actual = target.INVC_DUE_DT;
            Assert.AreEqual(expected, actual);
            
        }

 

        /// <summary>
        ///A test for HISTORICAL_ADJ_IND
        ///</summary>
        [TestMethod()]
        public void HISTORICAL_ADJ_INDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.HISTORICAL_ADJ_IND = expected;
            actual = target.HISTORICAL_ADJ_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_INVC_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void FNL_INVC_NBR_TXTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_INVC_NBR_TXT = expected;
            actual = target.FNL_INVC_NBR_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_INVC_DT
        ///</summary>
        [TestMethod()]
        public void FNL_INVC_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.FNL_INVC_DT = expected;
            actual = target.FNL_INVC_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_INTRNL_PDF_ZDW_KEY_TXT
        ///</summary>
        [TestMethod()]
        public void FNL_INTRNL_PDF_ZDW_KEY_TXTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_INTRNL_PDF_ZDW_KEY_TXT = expected;
            actual = target.FNL_INTRNL_PDF_ZDW_KEY_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_EXTRNL_PDF_ZDW_KEY_TXT
        ///</summary>
        [TestMethod()]
        public void FNL_EXTRNL_PDF_ZDW_KEY_TXTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_EXTRNL_PDF_ZDW_KEY_TXT = expected;
            actual = target.FNL_EXTRNL_PDF_ZDW_KEY_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FNL_CD_WRKSHT_PDF_ZDW_KEY_TXT
        ///</summary>
        [TestMethod()]
        public void FNL_CD_WRKSHT_PDF_ZDW_KEY_TXTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FNL_CD_WRKSHT_PDF_ZDW_KEY_TXT = expected;
            actual = target.FNL_CD_WRKSHT_PDF_ZDW_KEY_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINALINVNO
        ///</summary>
        [TestMethod()]
        public void FINALINVNOTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FINALINVNO = expected;
            actual = target.FINALINVNO;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EFF_DT
        ///</summary>
        [TestMethod()]
        public void EFF_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.EFF_DT = expected;
            actual = target.EFF_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRFT_INVC_DT
        ///</summary>
        [TestMethod()]
        public void DRFT_INVC_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DRFT_INVC_DT = expected;
            actual = target.DRFT_INVC_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRFT_INTRNL_PDF_ZDW_KEY_TXT
        ///</summary>
        [TestMethod()]
        public void DRFT_INTRNL_PDF_ZDW_KEY_TXTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT = expected;
            actual = target.DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DRAFTINVNO
        ///</summary>
        [TestMethod()]
        public void DRAFTINVNOTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DRAFTINVNO = expected;
            actual = target.DRAFTINVNO;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMERID
        ///</summary>
        [TestMethod()]
        public void CUSTOMERIDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMERID = expected;
            actual = target.CUSTOMERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_NAME
        ///</summary>
        [TestMethod()]
        public void CUSTMR_NAMETest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CUSTMR_NAME = expected;
            actual = target.CUSTMR_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CRTE_USER_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CRTE_USER_ID = expected;
            actual = target.CRTE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_DT
        ///</summary>
        [TestMethod()]
        public void CRTE_DTTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CRTE_DT = expected;
            actual = target.CRTE_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CALC_ADJ_STS_CODE
        ///</summary>
        [TestMethod()]
        public void CALC_ADJ_STS_CODETest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CALC_ADJ_STS_CODE = expected;
            actual = target.CALC_ADJ_STS_CODE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BUNAME
        ///</summary>
        [TestMethod()]
        public void BUNAMETest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BUNAME = expected;
            actual = target.BUNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BROKERNAME
        ///</summary>
        [TestMethod()]
        public void BROKERNAMETest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BROKERNAME = expected;
            actual = target.BROKERNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ARIES_CMPLT_IND
        ///</summary>
        [TestMethod()]
        public void ARIES_CMPLT_INDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ARIES_CMPLT_IND = expected;
            actual = target.ARIES_CMPLT_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJQC_COUNT
        ///</summary>
        //[TestMethod()]
        //public void ADJQC_COUNTTest()
        //{
        //    PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
        //    Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
        //    Nullable<int> actual;
        //    target.ADJQC_COUNT = expected;
        //    actual = target.ADJQC_COUNT;
        //    Assert.AreEqual(expected, actual);
            
        //}

        /// <summary>
        ///A test for ADJ_VOID_IND
        ///</summary>
        [TestMethod()]
        public void ADJ_VOID_INDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ADJ_VOID_IND = expected;
            actual = target.ADJ_VOID_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_STATUS_TYP_ID
        ///</summary>
        [TestMethod()]
        public void ADJ_STATUS_TYP_IDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJ_STATUS_TYP_ID = expected;
            actual = target.ADJ_STATUS_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_STATUS
        ///</summary>
        [TestMethod()]
        public void ADJ_STATUSTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJ_STATUS = expected;
            actual = target.ADJ_STATUS;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_RRSN_IND
        ///</summary>
        [TestMethod()]
        public void ADJ_RRSN_INDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ADJ_RRSN_IND = expected;
            actual = target.ADJ_RRSN_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_PENDG_IND_DESC
        ///</summary>
        [TestMethod()]
        public void ADJ_PENDG_IND_DESCTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJ_PENDG_IND_DESC = expected;
            actual = target.ADJ_PENDG_IND_DESC;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_PENDG_IND
        ///</summary>
        [TestMethod()]
        public void ADJ_PENDG_INDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ADJ_PENDG_IND = expected;
            actual = target.ADJ_PENDG_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_CAN_IND
        ///</summary>
        [TestMethod()]
        public void ADJ_CAN_INDTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ADJ_CAN_IND = expected;
            actual = target.ADJ_CAN_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmentBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmentBEConstructorTest()
        {
            PremiumAdjustmentBE target = new PremiumAdjustmentBE();
            
        }
    }
}
