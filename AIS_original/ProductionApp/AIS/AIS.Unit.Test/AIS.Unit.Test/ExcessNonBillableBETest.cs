using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ExcessNonBillableBETest and is intended
    ///to contain all ExcessNonBillableBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExcessNonBillableBETest
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
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
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
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDDATE = expected;
            actual = target.UPDATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TOTAL_INCURRED
        ///</summary>
        [TestMethod()]
        public void TOTAL_INCURREDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.TOTAL_INCURRED = expected;
            actual = target.TOTAL_INCURRED;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SYS_GENRT_IND
        ///</summary>
        [TestMethod()]
        public void SYS_GENRT_INDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.SYS_GENRT_IND = expected;
            actual = target.SYS_GENRT_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUBJ_RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void SUBJ_RESRV_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.SUBJ_RESRV_IDNMTY_AMT = expected;
            actual = target.SUBJ_RESRV_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUBJ_RESRV_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void SUBJ_RESRV_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.SUBJ_RESRV_EXPS_AMT = expected;
            actual = target.SUBJ_RESRV_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUBJ_PAID_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void SUBJ_PAID_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.SUBJ_PAID_IDNMTY_AMT = expected;
            actual = target.SUBJ_PAID_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUBJ_PAID_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void SUBJ_PAID_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.SUBJ_PAID_EXPS_AMT = expected;
            actual = target.SUBJ_PAID_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUBJ_INCURRED
        ///</summary>
        [TestMethod()]
        public void SUBJ_INCURREDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.SUBJ_INCURRED = expected;
            actual = target.SUBJ_INCURRED;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SITE_CD_TXT
        ///</summary>
        [TestMethod()]
        public void SITE_CD_TXTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SITE_CD_TXT = expected;
            actual = target.SITE_CD_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void RESRV_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RESRV_IDNMTY_AMT = expected;
            actual = target.RESRV_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RESRV_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void RESRV_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RESRV_EXPS_AMT = expected;
            actual = target.RESRV_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for REOPEN_CLAIMANT_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void REOPEN_CLAIMANT_NBR_TXTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.REOPEN_CLAIMANT_NBR_TXT = expected;
            actual = target.REOPEN_CLAIMANT_NBR_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY_AMT
        ///</summary>
        [TestMethod()]
        public void POLICY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.POLICY_AMT = expected;
            actual = target.POLICY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY
        ///</summary>
        [TestMethod()]
        public void POLICYTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY = expected;
            actual = target.POLICY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PAID_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void PAID_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.PAID_IDNMTY_AMT = expected;
            actual = target.PAID_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PAID_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void PAID_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.PAID_EXPS_AMT = expected;
            actual = target.PAID_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ORGIN_CLAIM_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void ORGIN_CLAIM_NBR_TXTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ORGIN_CLAIM_NBR_TXT = expected;
            actual = target.ORGIN_CLAIM_NBR_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NON_BILABL_RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void NON_BILABL_RESRV_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NON_BILABL_RESRV_IDNMTY_AMT = expected;
            actual = target.NON_BILABL_RESRV_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NON_BILABL_RESRV_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void NON_BILABL_RESRV_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NON_BILABL_RESRV_EXPS_AMT = expected;
            actual = target.NON_BILABL_RESRV_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NON_BILABL_PAID_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void NON_BILABL_PAID_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NON_BILABL_PAID_IDNMTY_AMT = expected;
            actual = target.NON_BILABL_PAID_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NON_BILABL_PAID_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void NON_BILABL_PAID_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NON_BILABL_PAID_EXPS_AMT = expected;
            actual = target.NON_BILABL_PAID_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NON_BILABL_INCURRED
        ///</summary>
        [TestMethod()]
        public void NON_BILABL_INCURREDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NON_BILABL_INCURRED = expected;
            actual = target.NON_BILABL_INCURRED;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LIMIT2_AMT
        ///</summary>
        [TestMethod()]
        public void LIMIT2_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.LIMIT2_AMT = expected;
            actual = target.LIMIT2_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void EXC_RESRV_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EXC_RESRV_IDNMTY_AMT = expected;
            actual = target.EXC_RESRV_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_RESRV_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void EXC_RESRV_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EXC_RESRV_EXPS_AMT = expected;
            actual = target.EXC_RESRV_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_PAID_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void EXC_PAID_IDNMTY_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EXC_PAID_IDNMTY_AMT = expected;
            actual = target.EXC_PAID_IDNMTY_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_PAID_EXPS_AMT
        ///</summary>
        [TestMethod()]
        public void EXC_PAID_EXPS_AMTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EXC_PAID_EXPS_AMT = expected;
            actual = target.EXC_PAID_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_INCURRED
        ///</summary>
        [TestMethod()]
        public void EXC_INCURREDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EXC_INCURRED = expected;
            actual = target.EXC_INCURRED;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
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
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
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
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDATE = expected;
            actual = target.CREATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COVG_TRIGGER_DATE
        ///</summary>
        [TestMethod()]
        public void COVG_TRIGGER_DATETest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.COVG_TRIGGER_DATE = expected;
            actual = target.COVG_TRIGGER_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COML_AGMT_ID
        ///</summary>
        [TestMethod()]
        public void COML_AGMT_IDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.COML_AGMT_ID = expected;
            actual = target.COML_AGMT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CLAIMSTATUS
        ///</summary>
        [TestMethod()]
        public void CLAIMSTATUSTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CLAIMSTATUS = expected;
            actual = target.CLAIMSTATUS;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CLAIMANT_NM
        ///</summary>
        [TestMethod()]
        public void CLAIMANT_NMTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CLAIMANT_NM = expected;
            actual = target.CLAIMANT_NM;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CLAIM_STS_ID
        ///</summary>
        [TestMethod()]
        public void CLAIM_STS_IDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CLAIM_STS_ID = expected;
            actual = target.CLAIM_STS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CLAIM_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void CLAIM_NBR_TXTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CLAIM_NBR_TXT = expected;
            actual = target.CLAIM_NBR_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ARMIS_LOS_ID
        ///</summary>
        [TestMethod()]
        public void ARMIS_LOS_IDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ARMIS_LOS_ID = expected;
            actual = target.ARMIS_LOS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ARMIS_LOS_EXC_ID
        ///</summary>
        [TestMethod()]
        public void ARMIS_LOS_EXC_IDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ARMIS_LOS_EXC_ID = expected;
            actual = target.ARMIS_LOS_EXC_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ALAE_TYP
        ///</summary>
        [TestMethod()]
        public void ALAE_TYPTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ALAE_TYP = expected;
            actual = target.ALAE_TYP;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ALAE_CAP
        ///</summary>
        [TestMethod()]
        public void ALAE_CAPTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ALAE_CAP = expected;
            actual = target.ALAE_CAP;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADD_CLAIM_TXT
        ///</summary>
        [TestMethod()]
        public void ADD_CLAIM_TXTTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADD_CLAIM_TXT = expected;
            actual = target.ADD_CLAIM_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ExcessNonBillableBE Constructor
        ///</summary>
        [TestMethod()]
        public void ExcessNonBillableBEConstructorTest()
        {
            ExcessNonBillableBE target = new ExcessNonBillableBE();
            
        }
    }
}
