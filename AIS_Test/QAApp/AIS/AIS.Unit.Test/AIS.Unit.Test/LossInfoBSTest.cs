using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LossInfoBSTest and is intended
    ///to contain all LossInfoBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LossInfoBSTest
    {
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PolicyBE policyBE;
        static PolicyBS policyBS;
        static LossInfoBE losInfoBE;
        static LossInfoBS target;
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            accountBS = new AccountBS();
            AddAccountData();
            programPeriodsBS = new ProgramPeriodsBS();
            AddProgramPeriodData();
            policyBS = new PolicyBS();
            AddPolicyData();
            target = new LossInfoBS();
            AddData();
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{


        //}
        
        #endregion

        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "AK" + System.DateTime.Now;
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }

        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddProgramPeriodData()
        {
            programPeriodBE = new ProgramPeriodBE();
            programPeriodBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            programPeriodBE.CREATE_DATE = System.DateTime.Now;
            programPeriodBE.CREATE_USER_ID = 1;
            programPeriodsBS.Save(programPeriodBE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddPolicyData()
        {
            policyBE = new PolicyBE();
            policyBE.ProgramPeriodID = programPeriodBE.PREM_ADJ_PGM_ID;
            policyBE.cstmrid = accountBE.CUSTMR_ID;
            policyBE.PlanEndDate = System.DateTime.Now;
            policyBE.CREATE_DATE = System.DateTime.Now;
            policyBE.CREATE_USER_ID = 1;
            policyBS.Save(policyBE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddData()
        {
            losInfoBE = new LossInfoBE();
            losInfoBE.ST_ID = 1;
            losInfoBE.COML_AGMT_ID = policyBE.PolicyID;
            losInfoBE.PAID_IDNMTY_AMT = 100;
            losInfoBE.PAID_EXPS_AMT = 100;
            losInfoBE.RESRV_IDNMTY_AMT = 100;
            losInfoBE.RESRV_EXPS_AMT = 100;
            losInfoBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            losInfoBE.VALN_DATE = System.DateTime.Now;
            losInfoBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            losInfoBE.SUPRT_SERV_CUSTMR_GP_ID = "2302849";
            losInfoBE.ACTV_IND = true;
            losInfoBE.SYS_GENRT_IND = false;
            losInfoBE.CREATEDATE = System.DateTime.Now;
            losInfoBE.CREATEUSER = 1;
            target.Update(losInfoBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false; ;
            LossInfoBE losInfBE = new LossInfoBE();
            losInfBE.ST_ID = 1;
            losInfBE.COML_AGMT_ID = policyBE.PolicyID;
            losInfBE.PAID_IDNMTY_AMT = 200;
            losInfBE.PAID_EXPS_AMT = 200;
            losInfBE.RESRV_IDNMTY_AMT = 200;
            losInfBE.RESRV_EXPS_AMT = 200;
            losInfBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            losInfBE.VALN_DATE = System.DateTime.Now;
            losInfBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            losInfBE.SUPRT_SERV_CUSTMR_GP_ID = "2302849";
            losInfBE.ACTV_IND = true;
            losInfBE.SYS_GENRT_IND = false;
            losInfBE.CREATEDATE = System.DateTime.Now;
            losInfBE.CREATEUSER = 1;
            actual = target.Update(losInfBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            LossInfoBS target = new LossInfoBS();
            LossInfoBE losInfoBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(losInfoBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update With Real Data
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            losInfoBE.PAID_IDNMTY_AMT = 200;
            losInfoBE.PAID_EXPS_AMT = 200;
            losInfoBE.RESRV_IDNMTY_AMT = 200;
            losInfoBE.RESRV_EXPS_AMT = 200;
            losInfoBE.UPDATEDDATE = System.DateTime.Now;
            losInfoBE.UPDATEDUSER = 1;
            actual = target.Update(losInfoBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            LossInfoBS target = new LossInfoBS();
            LossInfoBE losInfoBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(losInfoBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getLossInfoRow with Null
        ///</summary>
        [TestMethod()]
        public void getLossInfoRowTestWithNUll()
        {
            LossInfoBS target = new LossInfoBS(); 
            int ArmisLossID = 0; 
            LossInfoBE expected = null; 
            LossInfoBE actual;
            actual = target.getLossInfoRow(ArmisLossID);
            Assert.AreNotEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for getLossInfoRow with Data
        ///</summary>
        [TestMethod()]
        public void getLossInfoRowTestWithData()
        {
            LossInfoBE expected = null; 
            LossInfoBE actual;
            actual = target.getLossInfoRow(losInfoBE.ARMIS_LOS_ID);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getLossInfoDataTest with Data
        ///</summary>
        [TestMethod()]
        public void getLossInfoDataTest()
        {
            LossInfoBE expected = null;
            IList<LossInfoBE> result;
            result = target.getLossInfoData(System.DateTime.Now, losInfoBE.CUSTMR_ID, losInfoBE.PREM_ADJ_PGM_ID);
            Assert.AreNotEqual(expected, result);
        }
        /// <summary>
        ///A test for getLossInfoByPolicyDataTest with Data
        ///</summary>
        [TestMethod()]
        public void getLossInfoByPolicyDataTest()
        {
            LossInfoBE expected = null;
            IList<LossInfoBE> result;
            result = target.getLossInfoByPolicyData(System.DateTime.Now, losInfoBE.CUSTMR_ID, losInfoBE.PREM_ADJ_PGM_ID);
            Assert.AreNotEqual(expected, result);
        }
        /// <summary>
        ///A test for getLossInfoDataHideDisLinesTest with Data
        ///</summary>
        [TestMethod()]
        public void getLossInfoDataHideDisLinesTest()
        {
            LossInfoBE expected = null;
            IList<LossInfoBE> result;
            result = target.getLossInfoDataHideDisLines(System.DateTime.Now, losInfoBE.CUSTMR_ID, losInfoBE.PREM_ADJ_PGM_ID,true);
            Assert.AreNotEqual(expected, result);
        }

        

        /// <summary>
        ///A test for getLOBLookUpData with Data
        ///</summary>
        [TestMethod()]
        public void getLOBLookUpData()
        {
            LookupBE expected = null;
            IList<LookupBE> result;
            result = target.getLOBLookUpData();
            Assert.AreNotEqual(expected, result);
        }
    }
}
