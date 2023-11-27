using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PaidLossBillingBETest and is intended
    ///to contain all PaidLossBillingBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PaidLossBillingBETest
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
        ///A test for UPDATEDUSER
        ///</summary>
        [TestMethod()]
        public void UPDATEDUSERTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATEDUSER = expected;
            actual = target.UPDATEDUSER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDDATE
        ///</summary>
        [TestMethod()]
        public void UPDATEDDATETest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDDATE = expected;
            actual = target.UPDATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TOT_PAID_LOS_BIL_AMT
        ///</summary>
        [TestMethod()]
        public void TOT_PAID_LOS_BIL_AMTTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.TOT_PAID_LOS_BIL_AMT = expected;
            actual = target.TOT_PAID_LOS_BIL_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PERD_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PERD_IDTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREM_ADJ_PERD_ID = expected;
            actual = target.PREM_ADJ_PERD_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PAID_LOS_BIL_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PAID_LOS_BIL_IDTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PAID_LOS_BIL_ID = expected;
            actual = target.PREM_ADJ_PAID_LOS_BIL_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICYSYMBOL
        ///</summary>
        [TestMethod()]
        public void POLICYSYMBOLTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICYSYMBOL = expected;
            actual = target.POLICYSYMBOL;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSI_VALN_DATE
        ///</summary>
        [TestMethod()]
        public void LSI_VALN_DATETest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.LSI_VALN_DATE = expected;
            actual = target.LSI_VALN_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSI_SRC
        ///</summary>
        [TestMethod()]
        public void LSI_SRCTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.LSI_SRC = expected;
            actual = target.LSI_SRC;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSI_PGM_TYP
        ///</summary>
        [TestMethod()]
        public void LSI_PGM_TYPTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LSI_PGM_TYP = expected;
            actual = target.LSI_PGM_TYP;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOB_ID
        ///</summary>
        [TestMethod()]
        public void LOB_IDTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.LOB_ID = expected;
            actual = target.LOB_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOB
        ///</summary>
        [TestMethod()]
        public void LOBTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LOB = expected;
            actual = target.LOB;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void IDNMTY_AMTTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.IDNMTY_AMT = expected;
            actual = target.IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void EXPS_AMTTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EXPS_AMT = expected;
            actual = target.EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEUSER
        ///</summary>
        [TestMethod()]
        public void CREATEUSERTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATEUSER = expected;
            actual = target.CREATEUSER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDATE
        ///</summary>
        [TestMethod()]
        public void CREATEDATETest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDATE = expected;
            actual = target.CREATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COML_AGMT_ID
        ///</summary>
        [TestMethod()]
        public void COML_AGMT_IDTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.COML_AGMT_ID = expected;
            actual = target.COML_AGMT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CMMNT_TXT
        ///</summary>
        [TestMethod()]
        public void CMMNT_TXTTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CMMNT_TXT = expected;
            actual = target.CMMNT_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_TOT_PAID_LOS_BIL_AMT
        ///</summary>
        [TestMethod()]
        public void ADJ_TOT_PAID_LOS_BIL_AMTTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ADJ_TOT_PAID_LOS_BIL_AMT = expected;
            actual = target.ADJ_TOT_PAID_LOS_BIL_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void ADJ_IDNMTY_AMTTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ADJ_IDNMTY_AMT = expected;
            actual = target.ADJ_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void ADJ_EXPS_AMTTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ADJ_EXPS_AMT = expected;
            actual = target.ADJ_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PaidLossBillingBE Constructor
        ///</summary>
        [TestMethod()]
        public void PaidLossBillingBEConstructorTest()
        {
            PaidLossBillingBE target = new PaidLossBillingBE();
            
        }
    }
}
