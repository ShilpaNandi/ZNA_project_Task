using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ExcessNonBillableBSTest and is intended
    ///to contain all ExcessNonBillableBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExcessNonBillableBSTest
    {
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PolicyBE policyBE;
        static PolicyBS policyBS;
        static LossInfoBE losInfoBE;
        static LossInfoBS losInfoBS;
        static ExcessNonBillableBE ExcessNonBilBE;
        static ExcessNonBillableBS target;
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
            losInfoBS = new LossInfoBS();
            AddLossInfoData();
            target = new ExcessNonBillableBS();
            AddData();
        }
        
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
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "Joseph" + System.DateTime.Now;
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
        private static void AddLossInfoData()
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
            losInfoBS.Save(losInfoBE);
        }

        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddData()
        {
            ExcessNonBilBE = new ExcessNonBillableBE();
            ExcessNonBilBE.ARMIS_LOS_ID = losInfoBE.ARMIS_LOS_ID;
            ExcessNonBilBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            ExcessNonBilBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            ExcessNonBilBE.COML_AGMT_ID = policyBE.PolicyID;
            ExcessNonBilBE.CLAIM_NBR_TXT = "22";
            ExcessNonBilBE.CLAIMANT_NM = "John";
            ExcessNonBilBE.COVG_TRIGGER_DATE = System.DateTime.Now;
            ExcessNonBilBE.PAID_IDNMTY_AMT = 100;
            ExcessNonBilBE.PAID_EXPS_AMT = 100;
            ExcessNonBilBE.RESRV_IDNMTY_AMT = 100;
            ExcessNonBilBE.RESRV_EXPS_AMT = 100;
            ExcessNonBilBE.NON_BILABL_PAID_IDNMTY_AMT = 100;
            ExcessNonBilBE.NON_BILABL_PAID_EXPS_AMT = 100;
            ExcessNonBilBE.NON_BILABL_RESRV_IDNMTY_AMT = 100;
            ExcessNonBilBE.NON_BILABL_RESRV_EXPS_AMT = 100;
            ExcessNonBilBE.ADD_CLAIM_TXT = "More Claims";
            ExcessNonBilBE.LIMIT2_AMT = 100;
            ExcessNonBilBE.SYS_GENRT_IND = false;
            ExcessNonBilBE.ACTV_IND = true;
            ExcessNonBilBE.CREATEDATE = System.DateTime.Now;
            ExcessNonBilBE.CREATEUSER = 1;
            target.Update(ExcessNonBilBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = true; ;
            ExcessNonBillableBE ExcNonBilBE = new ExcessNonBillableBE();
            ExcNonBilBE.ARMIS_LOS_ID = policyBE.PolicyID;
            ExcNonBilBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            ExcNonBilBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            ExcNonBilBE.COML_AGMT_ID = policyBE.PolicyID;
            ExcNonBilBE.CLAIM_NBR_TXT = "22";
            ExcNonBilBE.CLAIMANT_NM = "John";
            ExcNonBilBE.COVG_TRIGGER_DATE = System.DateTime.Now;
            ExcNonBilBE.PAID_IDNMTY_AMT = 100;
            ExcNonBilBE.PAID_EXPS_AMT = 100;
            ExcessNonBilBE.RESRV_IDNMTY_AMT = 100;
            ExcNonBilBE.RESRV_EXPS_AMT = 100;
            ExcNonBilBE.NON_BILABL_PAID_IDNMTY_AMT = 100;
            ExcNonBilBE.NON_BILABL_PAID_EXPS_AMT = 100;
            ExcNonBilBE.NON_BILABL_RESRV_IDNMTY_AMT = 100;
            ExcNonBilBE.NON_BILABL_RESRV_EXPS_AMT = 100;
            ExcNonBilBE.ADD_CLAIM_TXT = "More Claims";
            ExcNonBilBE.LIMIT2_AMT = 100;
            ExcNonBilBE.SYS_GENRT_IND = false;
            ExcNonBilBE.ACTV_IND = true;
            ExcNonBilBE.CREATEDATE = System.DateTime.Now;
            ExcNonBilBE.CREATEUSER = 1;
            actual = target.Update(ExcNonBilBE);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS();
            ExcessNonBillableBE ExcessNonBilBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(ExcessNonBilBE);
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
            ExcessNonBilBE.PAID_IDNMTY_AMT = 200;
            ExcessNonBilBE.PAID_EXPS_AMT = 200;
            ExcessNonBilBE.RESRV_IDNMTY_AMT = 200;
            ExcessNonBilBE.RESRV_EXPS_AMT = 200;
            ExcessNonBilBE.NON_BILABL_PAID_IDNMTY_AMT = 200;
            ExcessNonBilBE.NON_BILABL_PAID_EXPS_AMT = 200;
            ExcessNonBilBE.NON_BILABL_RESRV_IDNMTY_AMT = 200;
            ExcessNonBilBE.NON_BILABL_RESRV_EXPS_AMT = 200;
            ExcessNonBilBE.UPDATEDDATE = System.DateTime.Now;
            ExcessNonBilBE.UPDATEDUSER = 1;
            actual = target.Update(ExcessNonBilBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS();
            ExcessNonBillableBE ExcessNonBilBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(ExcessNonBilBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for getExcNonBillableDataHideDisLines with Parameters as Null
        ///</summary>
        [TestMethod()]
        public void getExcNonBillableDataHideDisLinesTestWithNull()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS();
            int armsID = 0;
            int comAmgtID = 0;
            int custmrID = 0;
            int prgID = 0;
            bool flag = false;
            int expected = 0;
            IList<ExcessNonBillableBE> actual;
            actual = target.getExcNonBillableDataHideDisLines(armsID, comAmgtID, custmrID, prgID, flag);
            Assert.AreEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for getExcNonBillableDataHideDisLines with Parameters
        ///</summary>
        [TestMethod()]
        public void getExcNonBillableDataHideDisLinesTest()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS(); 
            int armsID = ExcessNonBilBE.ARMIS_LOS_ID; 
            int comAmgtID =ExcessNonBilBE.COML_AGMT_ID; 
            int custmrID = ExcessNonBilBE.CUSTMR_ID;
            int prgID = ExcessNonBilBE.PREM_ADJ_PGM_ID; 
            bool flag = true; 
            int expected = 0; 
            IList<ExcessNonBillableBE> actual;
            actual = target.getExcNonBillableDataHideDisLines(armsID, comAmgtID, custmrID, prgID, flag);
            Assert.AreNotEqual(expected, actual.Count);
            
        }
        /// <summary>
        ///A test for getExcNonBillableData with Parameters as Null
        ///</summary>
        [TestMethod()]
        public void getExcNonBillableDataTest1WithNull()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS();
            int armsID = 0;
            int comAmgtID = 0;
            int custmrID = 0;
            int prgID = 0;
            int expected = 0;
            IList<ExcessNonBillableBE> actual;
            actual = target.getExcNonBillableData(armsID, comAmgtID, custmrID, prgID);
            Assert.AreEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for getExcNonBillableData with Parameters
        ///</summary>
        [TestMethod()]
        public void getExcNonBillableDataTest1()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS(); 
            int armsID = ExcessNonBilBE.ARMIS_LOS_ID; 
            int comAmgtID =ExcessNonBilBE.COML_AGMT_ID; 
            int custmrID = ExcessNonBilBE.CUSTMR_ID;
            int prgID = ExcessNonBilBE.PREM_ADJ_PGM_ID; 
            int expected = 0; 
            IList<ExcessNonBillableBE> actual;
            actual = target.getExcNonBillableData(armsID, comAmgtID, custmrID, prgID);
            Assert.AreNotEqual(expected, actual.Count);
            
        }
        /// <summary>
        ///A test for getExcNonBillableData with Null
        ///</summary>
        [TestMethod()]
        public void getExcNonBillableDataTestWithNull()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS();
            int intarmsLossExcID = 0; 
            int  expected = 0;
            IList<ExcessNonBillableBE> actual;
            actual = target.getExcNonBillableData(intarmsLossExcID);
            Assert.AreEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for getExcNonBillableData with Data
        ///</summary>
        [TestMethod()]
        public void getExcNonBillableDataTest()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS(); 
            //int intarmsLossExcID = 1; 
            int expected = 0;
            IList<ExcessNonBillableBE> actual;
            actual = target.getExcNonBillableData(ExcessNonBilBE.ARMIS_LOS_EXC_ID);
            Assert.AreNotEqual(expected, actual.Count);
            
        }
        /// <summary>
        ///A test for getExcessNonBilRow with Null
        ///</summary>
        [TestMethod()]
        public void getExcessNonBilRowTestWithNull()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS(); 
            int ArmisLossExcID = 0;
            ExcessNonBillableBE expected = null; 
            ExcessNonBillableBE actual;
            actual = target.getExcessNonBilRow(ArmisLossExcID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getExcessNonBilRow with Data
        ///</summary>
        [TestMethod()]
        public void getExcessNonBilRowTest()
        {
            ExcessNonBillableBS target = new ExcessNonBillableBS();
            //int ArmisLossExcID = 1; 
            ExcessNonBillableBE expected = null; 
            ExcessNonBillableBE actual;
            actual = target.getExcessNonBilRow(ExcessNonBilBE.ARMIS_LOS_EXC_ID);
            Assert.AreNotEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for ExcessNonBillableBS Constructor
        ///</summary>
        //[TestMethod()]
        //public void ExcessNonBillableBSConstructorTest()
        //{
        //    ExcessNonBillableBS target = new ExcessNonBillableBS();
        //    
        //}
    }
}
