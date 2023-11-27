using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjustmentParameterDetailBETest and is intended
    ///to contain all AdjustmentParameterDetailBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjustmentParameterDetailBETest
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
        ///A test for UPDTE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDTE_USER_IDTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDTE_USER_ID = expected;
            actual = target.UPDTE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDTE_DATE
        ///</summary>
        [TestMethod()]
        public void UPDTE_DATETest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDTE_DATE = expected;
            actual = target.UPDTE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for st_id
        ///</summary>
        [TestMethod()]
        public void st_idTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.st_id = expected;
            actual = target.st_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PrgParameterStateName
        ///</summary>
        [TestMethod()]
        public void PrgParameterStateNameTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PrgParameterStateName = expected;
            actual = target.PrgParameterStateName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PrgParameterStateLookupType
        ///</summary>
        [TestMethod()]
        public void PrgParameterStateLookupTypeTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.PrgParameterStateLookupType;
            
        }

        /// <summary>
        ///A test for PrgparameterStateFullname
        ///</summary>
        [TestMethod()]
        public void PrgparameterStateFullnameTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PrgparameterStateFullname = expected;
            actual = target.PrgparameterStateFullname;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PrgParameterLOBName
        ///</summary>
        [TestMethod()]
        public void PrgParameterLOBNameTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PrgParameterLOBName = expected;
            actual = target.PrgParameterLOBName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PrgParameterLOBLookupType
        ///</summary>
        [TestMethod()]
        public void PrgParameterLOBLookupTypeTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.PrgParameterLOBLookupType;
            
        }

        /// <summary>
        ///A test for PrgParameterCHFLName
        ///</summary>
        [TestMethod()]
        public void PrgParameterCHFLNameTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PrgParameterCHFLName = expected;
            actual = target.PrgParameterCHFLName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PrgParameterCHFLLookupType
        ///</summary>
        [TestMethod()]
        public void PrgParameterCHFLLookupTypeTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.PrgParameterCHFLLookupType;
            
        }

        /// <summary>
        ///A test for PrgmPerodID
        ///</summary>
        [TestMethod()]
        public void PrgmPerodIDTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PrgmPerodID = expected;
            actual = target.PrgmPerodID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremAssementAmt
        ///</summary>
        [TestMethod()]
        public void PremAssementAmtTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.PremAssementAmt = expected;
            actual = target.PremAssementAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for prem_adj_pgm_dtl_id
        ///</summary>
        [TestMethod()]
        public void prem_adj_pgm_dtl_idTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.prem_adj_pgm_dtl_id = expected;
            actual = target.prem_adj_pgm_dtl_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ln_of_bsn_id
        ///</summary>
        [TestMethod()]
        public void ln_of_bsn_idTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ln_of_bsn_id = expected;
            actual = target.ln_of_bsn_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for fnl_overrid_amt
        ///</summary>
        [TestMethod()]
        public void fnl_overrid_amtTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.fnl_overrid_amt = expected;
            actual = target.fnl_overrid_amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CRTE_USER_IDTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CRTE_USER_ID = expected;
            actual = target.CRTE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_DATE
        ///</summary>
        [TestMethod()]
        public void CRTE_DATETest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CRTE_DATE = expected;
            actual = target.CRTE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for cmmnt_txt
        ///</summary>
        [TestMethod()]
        public void cmmnt_txtTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.cmmnt_txt = expected;
            actual = target.cmmnt_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Clm_hndlfee_clmrate
        ///</summary>
        [TestMethod()]
        public void Clm_hndlfee_clmrateTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Clm_hndlfee_clmrate = expected;
            actual = target.Clm_hndlfee_clmrate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for clm_hndl_fee_los_typ_id
        ///</summary>
        [TestMethod()]
        public void clm_hndl_fee_los_typ_idTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.clm_hndl_fee_los_typ_id = expected;
            actual = target.clm_hndl_fee_los_typ_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CHF_CLMT_NUMBER
        ///</summary>
        [TestMethod()]
        public void CHF_CLMT_NUMBERTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.CHF_CLMT_NUMBER = expected;
            actual = target.CHF_CLMT_NUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for adj_paramet_id
        ///</summary>
        [TestMethod()]
        public void adj_paramet_idTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.adj_paramet_id = expected;
            actual = target.adj_paramet_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for adj_fctr_rt
        ///</summary>
        [TestMethod()]
        public void adj_fctr_rtTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.adj_fctr_rt = expected;
            actual = target.adj_fctr_rt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for act_ind
        ///</summary>
        [TestMethod()]
        public void act_indTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.act_ind = expected;
            actual = target.act_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AccountID
        ///</summary>
        [TestMethod()]
        public void AccountIDTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.AccountID = expected;
            actual = target.AccountID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustmentParameterDetailBE Constructor
        ///</summary>
        [TestMethod()]
        public void AdjustmentParameterDetailBEConstructorTest()
        {
            AdjustmentParameterDetailBE target = new AdjustmentParameterDetailBE();
            
        }
    }
}
