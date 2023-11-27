using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LossInfoBETest and is intended
    ///to contain all LossInfoBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class LossInfoBETest
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
        ///A test for VALN_DATE
        ///</summary>
        [TestMethod()]
        public void VALN_DATETest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.VALN_DATE = expected;
            actual = target.VALN_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDUSER
        ///</summary>
        [TestMethod()]
        public void UPDATEDUSERTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = false; // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.SYS_GENRT_IND = expected;
            actual = target.SYS_GENRT_IND;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SUPRT_SERV_CUSTMR_GP_ID
        ///</summary>
        [TestMethod()]
        public void SUPRT_SERV_CUSTMR_GP_IDTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SUPRT_SERV_CUSTMR_GP_ID = expected;
            actual = target.SUPRT_SERV_CUSTMR_GP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUBJ_RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void SUBJ_RESRV_IDNMTY_AMTTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.SUBJ_INCURRED = expected;
            actual = target.SUBJ_INCURRED;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STATETYPE
        ///</summary>
        [TestMethod()]
        public void STATETYPETest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.STATETYPE = expected;
            actual = target.STATETYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Statelookuptype
        ///</summary>
        [TestMethod()]
        public void StatelookuptypeTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.Statelookuptype;
            
        }

        /// <summary>
        ///A test for ST_ID
        ///</summary>
        [TestMethod()]
        public void ST_IDTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ST_ID = expected;
            actual = target.ST_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void RESRV_IDNMTY_AMTTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.RESRV_EXPS_AMT = expected;
            actual = target.RESRV_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICYSYMBOL = expected;
            actual = target.POLICYSYMBOL;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyNumber
        ///</summary>
        [TestMethod()]
        public void PolicyNumberTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicyNumber = expected;
            actual = target.PolicyNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY_AMT
        ///</summary>
        [TestMethod()]
        public void POLICY_AMTTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY = expected;
            actual = target.POLICY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PerfectPolicynum
        ///</summary>
        [TestMethod()]
        public void PerfectPolicynumTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            PolicyBE actual;
            actual = target.PerfectPolicynum;
            
        }

        /// <summary>
        ///A test for PAID_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void PAID_IDNMTY_AMTTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.PAID_EXPS_AMT = expected;
            actual = target.PAID_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NON_BILABL_RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void NON_BILABL_RESRV_IDNMTY_AMTTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NON_BILABL_INCURRED = expected;
            actual = target.NON_BILABL_INCURRED;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOBTYPE
        ///</summary>
        [TestMethod()]
        public void LOBTYPETest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LOBTYPE = expected;
            actual = target.LOBTYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Loblookuptype
        ///</summary>
        [TestMethod()]
        public void LoblookuptypeTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.Loblookuptype;
            
        }

        /// <summary>
        ///A test for Incurred
        ///</summary>
        [TestMethod()]
        public void IncurredTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Incurred = expected;
            actual = target.Incurred;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_RESRV_IDNMTY_AMT
        ///</summary>
        [TestMethod()]
        public void EXC_RESRV_IDNMTY_AMTTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EXC_PAID_EXPS_AMT = expected;
            actual = target.EXC_PAID_EXPS_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_NON_BIL
        ///</summary>
        [TestMethod()]
        public void EXC_NON_BILTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.EXC_NON_BIL = expected;
            actual = target.EXC_NON_BIL;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXC_INCURRED
        ///</summary>
        [TestMethod()]
        public void EXC_INCURREDTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
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
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.COML_AGMT_ID = expected;
            actual = target.COML_AGMT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ARMIS_LOS_ID
        ///</summary>
        [TestMethod()]
        public void ARMIS_LOS_IDTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ARMIS_LOS_ID = expected;
            actual = target.ARMIS_LOS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ALAE_TYP
        ///</summary>
        [TestMethod()]
        public void ALAE_TYPTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ALAE_TYP = expected;
            actual = target.ALAE_TYP;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_STATUS
        ///</summary>
        [TestMethod()]
        public void ADJ_STATUSTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJ_STATUS = expected;
            actual = target.ADJ_STATUS;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            LossInfoBE target = new LossInfoBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LossInfoBE Constructor
        ///</summary>
        [TestMethod()]
        public void LossInfoBEConstructorTest()
        {
            LossInfoBE target = new LossInfoBE();
            
        }
    }
}
