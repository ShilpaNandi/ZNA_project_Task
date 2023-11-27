using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Adj_Parameter_SetupBSTest and is intended
    ///to contain all Adj_Parameter_SetupBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Adj_Parameter_SetupBSTest
    {


        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static AdjustmentParameterSetupBE  AdjLBABE;
        static Adj_Parameter_SetupBS target;

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
            //Customet Table
            accountBS = new AccountBS();
            AddAccountData();
            //Prem Adj Program Table
            programPeriodsBS = new ProgramPeriodsBS();
            AddProgramPeriodData();
            //Parameter Table
            target = new Adj_Parameter_SetupBS();
            AddDataLBA();
        }
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
        /// a method for adding New Record when the class is initiated Into Customer Table 
        /// To meet F.key in PremAdjMIscInvoice Table 
        /// </summary>
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "venkat" + System.DateTime.Now.ToString();
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }

        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj Program  Table 
        /// To meet F.key in PremAdjPeriod Table 
        /// </summary>
        private static void AddProgramPeriodData()
        {
            programPeriodBE = new ProgramPeriodBE();
            programPeriodBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            programPeriodBE.CREATE_DATE = System.DateTime.Now;
            programPeriodBE.CREATE_USER_ID = 1;
            programPeriodsBS.Update(programPeriodBE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddDataLBA()
        {
            AdjLBABE = new AdjustmentParameterSetupBE();
            AdjLBABE.prem_adj_pgm_id = programPeriodBE.PREM_ADJ_PGM_ID;
            AdjLBABE.Cstmr_Id = accountBE.CUSTMR_ID;
            AdjLBABE.incld_ernd_retro_prem_ind = true;
            AdjLBABE.incld_ibnr_ldf_ind = false;
            AdjLBABE.depst_amt = 369;
            AdjLBABE.lba_Adjustment_typ = 63;
            AdjLBABE.actv_ind = true;
            AdjLBABE.AdjparameterTypeID = 401;
            AdjLBABE.CREATE_DATE  = System.DateTime.Now;
            AdjLBABE.CREATE_USER_ID = 1;
            target.Update(AdjLBABE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestLBAWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterSetupBE AdjprmLBABE = new AdjustmentParameterSetupBE();
            AdjprmLBABE.prem_adj_pgm_id = programPeriodBE.PREM_ADJ_PGM_ID;
            AdjprmLBABE.Cstmr_Id = accountBE.CUSTMR_ID;
            AdjprmLBABE.incld_ernd_retro_prem_ind = false;
            AdjprmLBABE.incld_ibnr_ldf_ind = true;
            AdjprmLBABE.depst_amt = 550;
            AdjprmLBABE.lba_Adjustment_typ = 68;
            AdjprmLBABE.actv_ind = true;
            AdjprmLBABE.AdjparameterTypeID = 401;
            AdjprmLBABE.CREATE_DATE = System.DateTime.Now;
            AdjprmLBABE.CREATE_USER_ID = 1;
            actual = target.Update(AdjprmLBABE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestLBAWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmLBABE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmLBABE);
            Assert.AreEqual(expected, actual);

        }
       
        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestLBAwithData()
        {
            bool expected = true;
            bool actual;
            AdjustmentParameterSetupBE adjPSBE=(new Adj_Parameter_SetupBS()).getAdjParamRow(AdjLBABE.adj_paramet_setup_id);
            adjPSBE.depst_amt = 400;
            adjPSBE.AdjparameterTypeID = 401;
            adjPSBE.UPDATE_DATE  = System.DateTime.Now;
            adjPSBE.UPDATE_USER_ID = 10;
            actual = (new Adj_Parameter_SetupBS()).Update(adjPSBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestLBAWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmLBABE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmLBABE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getAdjParamtr with real Data
        ///</summary>
        [TestMethod()]
        public void getAdjLBAParamtrTest()
        {
            int expected = 0;
            int ProgramPeriodID = programPeriodBE.PREM_ADJ_PGM_ID;
            int CstmrID = accountBE.CUSTMR_ID;
            IList<AdjustmentParameterSetupBE> actual;
            actual = target.getAdjParamtr(ProgramPeriodID, CstmrID);
            Assert.AreNotEqual(expected, actual.Count);
          
        }


        /// <summary>
        ///A test for getAdjParamtr with real Data
        ///</summary>
        [TestMethod()]
        public void getAdjReviewEscrow()
        {
            int expected = 0;
            int ProgramPeriodID = programPeriodBE.PREM_ADJ_PGM_ID;
            int CstmrID =accountBE.CUSTMR_ID;
            IList<AdjustmentParameterSetupBE> actual;
            actual = target.getAdjReviewEscrow(ProgramPeriodID, CstmrID, 401);
            Assert.AreNotEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for getAdjParamtr with real Data
        ///</summary>
        [TestMethod()]
        public void getAdjParamsforILRF()
        {
           
            int ProgramPeriodID = programPeriodBE.PREM_ADJ_PGM_ID;
            int CstmrID = accountBE.CUSTMR_ID;
            AdjustmentParameterSetupBE expected = null; 
            AdjustmentParameterSetupBE actual;
            actual = target.getAdjParamsforILRF(ProgramPeriodID, CstmrID, 401);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getAdjParamtr with real Data
        ///</summary>
        [TestMethod()]
        public void getAdjParamtERPTrue()
        {
            int expected = 0;
            int ProgramPeriodID = programPeriodBE.PREM_ADJ_PGM_ID;
            int CstmrID = accountBE.CUSTMR_ID;
            IList<AdjustmentParameterSetupBE> actual;
            actual = target.getAdjParamtERPTrue(ProgramPeriodID, CstmrID, true);
            Assert.AreNotEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for getAdjParamtr with Null Data
        ///</summary>
        [TestMethod()]
        public void getAdjLBANullParamtrTest()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            int ProgramPeriodID = 0;
            int CstmrID = 0;
            IList<AdjustmentParameterSetupBE> expected = null;
            IList<AdjustmentParameterSetupBE> actual;
            actual = target.getAdjParamtr(ProgramPeriodID, CstmrID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getAdjParamRow with real data
        ///</summary>
        [TestMethod()]
        public void getAdjParamRowTest()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS(); 
            int adjPrmsetupID = 0; 
            AdjustmentParameterSetupBE expected = null; 
            AdjustmentParameterSetupBE actual = new AdjustmentParameterSetupBE();
            actual = target.getAdjParamRow(adjPrmsetupID);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreEqual(expected, actual);
            
        }

       

    }
}
