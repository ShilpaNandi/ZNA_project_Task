using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentEscrowBSTest and is intended
    ///to contain all PremiumAdjustmentEscrowBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentEscrowBSTest
    {


        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PremiumAdjustmentBE premAdjBE;
        static PremAdjustmentBS premAdjBS;
        static PremiumAdjustmentPeriodBE premAdjPeriodBE;
        static PremiumAdjustmentPeriodBS premAdjPeriodBS;
        static AdjustmentParameterSetupBE adjParameterSetupBE;
        static Adj_Parameter_SetupBS adjParameterSetupBS;
        static PremiumAdjustmentEscrowBE premadjEscrowBE;
        static PremiumAdjustmentEscrowBS target;
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
            //Prem Adj Table
            premAdjBS = new PremAdjustmentBS();
            AddPremiumAdjustmentData();
            //Prem Adj Period Table
            premAdjPeriodBS = new PremiumAdjustmentPeriodBS();
            AddPremiumAdjustmentPeriodData();
            //Prem Adj Pgm Setup Table
            adjParameterSetupBS = new Adj_Parameter_SetupBS();
            AddPremiumAdjProgramSetupData();
            // PREM_ADJ_PARMET_SETUP Table
            target = new PremiumAdjustmentEscrowBS();
            AddPremiumAdjustmentEscrowCommonData();
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
        /// To meet F.key in PREM_ADJ_PARMET_SETUP Table 
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
        ///  a method for adding New Record when the class is initiated Into Prem Adj   Table 
        /// To meet F.key in PREM_ADJ_PARMET_SETUP Table 
        /// </summary>
        private static void AddPremiumAdjustmentData()
        {
            premAdjBE = new PremiumAdjustmentBE();
            premAdjBE.CUSTOMERID = accountBE.CUSTMR_ID;
            premAdjBE.VALN_DT = System.DateTime.Now;
            premAdjBE.CRTE_DT = System.DateTime.Now;
            premAdjBE.CRTE_USER_ID = 1;
            premAdjBS.Save(premAdjBE);

        }
        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj Period  Table 
        /// To meet F.key in PREM_ADJ_PARMET_SETUP Table 
        /// </summary>
        private static void AddPremiumAdjustmentPeriodData()
        {
            premAdjPeriodBE = new PremiumAdjustmentPeriodBE();
            premAdjPeriodBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            premAdjPeriodBE.REG_CUSTMR_ID = accountBE.CUSTMR_ID;
            premAdjPeriodBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            premAdjPeriodBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            premAdjPeriodBE.CREATE_USER_ID = 1;
            premAdjPeriodBE.CREATE_DATE = System.DateTime.Now;
            premAdjPeriodBS.Save(premAdjPeriodBE);
        }
        /// <summary>
        ///  a method for adding New Record when the class is initiated Into PREM_ADJ_PGM_SETUP  Table 
        /// To meet F.key in PREM_ADJ_PARMET_SETUP Table 
        /// </summary>
        private static void AddPremiumAdjProgramSetupData()
        {
            adjParameterSetupBE = new AdjustmentParameterSetupBE();
            adjParameterSetupBE.prem_adj_pgm_id = programPeriodBE.PREM_ADJ_PGM_ID;
            adjParameterSetupBE.Cstmr_Id = accountBE.CUSTMR_ID;
            adjParameterSetupBE.CREATE_USER_ID = 1;
            adjParameterSetupBE.CREATE_DATE = System.DateTime.Now;
            adjParameterSetupBS.Update(adjParameterSetupBE);

        }
        /// <summary>
        /// a method for adding New Record when the class is initiated into PREM_ADJ_PARMET_SETUP Table
        /// </summary>
        private static void AddPremiumAdjustmentEscrowCommonData()
        {
            premadjEscrowBE = new PremiumAdjustmentEscrowBE();
            premadjEscrowBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            premadjEscrowBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            premadjEscrowBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            premadjEscrowBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            premadjEscrowBE.PREM_ADJ_PGM_SETUP_ID = adjParameterSetupBE.adj_paramet_setup_id;
            premadjEscrowBE.ESCR_ADJ_AMT = 1000;
            premadjEscrowBE.ESCR_ADJ_PAID_LOS_AMT = 2000;
            premadjEscrowBE.ESCR_AMT = 3000;
            premadjEscrowBE.ESCR_PREVIOUSULY_BILED_AMT = 4000;
            premadjEscrowBE.CREATE_USER_ID = 1;
            premadjEscrowBE.CREATE_DATE = System.DateTime.Now;
            target.Update(premadjEscrowBE);

        }
        /// <summary>
        /// a Test for adding a Record  With Real Data in to PREM_ADJ_PARMET_SETUP Table
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            PremiumAdjustmentEscrowBE PAEscrowBE = new PremiumAdjustmentEscrowBE();
            PAEscrowBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            PAEscrowBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            PAEscrowBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            PAEscrowBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            PAEscrowBE.PREM_ADJ_PGM_SETUP_ID = adjParameterSetupBE.adj_paramet_setup_id;
            PAEscrowBE.ESCR_ADJ_AMT = 1000;
            PAEscrowBE.ESCR_ADJ_PAID_LOS_AMT = 2000;
            PAEscrowBE.ESCR_AMT = 3000;
            PAEscrowBE.ESCR_PREVIOUSULY_BILED_AMT = 4000;
            PAEscrowBE.CREATE_USER_ID = 1;
            PAEscrowBE.CREATE_DATE = System.DateTime.Now;
            actual = (new PremiumAdjustmentEscrowBS()).Update(PAEscrowBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for adding New Record With NULL(with out Passing Any Data)
        /// the Add Method Should Rise an Exception
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            PremiumAdjustmentEscrowBS PAEscrowBS = new PremiumAdjustmentEscrowBS();
            PremiumAdjustmentEscrowBE PAEscrowBE = null;
            bool expected = false;
            bool actual;
            actual = PAEscrowBS.Update(PAEscrowBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update a Record With Real Data into PREM_ADJ_PARMET_SETUP Table
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            PremiumAdjustmentEscrowBE PAEBE = new PremiumAdjustmentEscrowBS().getPremAdjEscrowRow(premadjEscrowBE.PREM_ADJ_PARMET_SETUP_ID);
            PAEBE.ESCR_ADJ_AMT = 2000;
            PAEBE.ESCR_ADJ_PAID_LOS_AMT = 3000;
            PAEBE.ESCR_AMT = 4000;
            PAEBE.ESCR_PREVIOUSULY_BILED_AMT = 5000;
            PAEBE.CREATE_USER_ID = 1;
            PAEBE.CREATE_DATE = System.DateTime.Now;
            actual = target.Update(PAEBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update a record With NULL(i.e With out Passing any Values)
        ///it should Rise Exception
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            PremiumAdjustmentEscrowBS target = new PremiumAdjustmentEscrowBS();
            PremiumAdjustmentEscrowBE PreAdjEscrowBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(PreAdjEscrowBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPremAdjEscrowRow With Actual Parameter
        ///</summary>
        [TestMethod()]
        public void getPremAdjEscrowRowTestWithData()
        {
            PremiumAdjustmentEscrowBE expected = null;
            PremiumAdjustmentEscrowBE actual;
            actual = target.getPremAdjEscrowRow(premadjEscrowBE.PREM_ADJ_PARMET_SETUP_ID);
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for getPremAdjEscrowRow with Actual Parameter
        ///</summary>
        [TestMethod()]
        public void getPremAdjEscrowRowTest()
        {
            int intPremAdjParmsetupID = 0;
            PremiumAdjustmentEscrowBE expected = null;
            PremiumAdjustmentEscrowBE actual;
            actual = target.getPremAdjEscrowRow(intPremAdjParmsetupID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for GetPremAdjEscrowInfo
        ///</summary>
        [TestMethod()]
        public void GetPremAdjEscrowInfoTestWithData()
        {
            int expected = 0;
            IList<PremiumAdjustmentEscrowBE> actual;
            actual = target.GetPremAdjEscrowInfo(accountBE.CUSTMR_ID, premAdjPeriodBE.PREM_ADJ_PERD_ID, premAdjBE.PREMIUM_ADJ_ID, programPeriodBE.PREM_ADJ_PGM_ID, adjParameterSetupBE.adj_paramet_setup_id);
            Assert.AreNotEqual(expected, actual.Count);
        }
        /// <summary>
        ///A test for GetPremAdjEscrowInfo
        ///</summary>
        [TestMethod()]
        public void GetPremAdjEscrowInfoTest()
        {
            int intaccountID = 0;
            int intPremAdjPerdID = 0;
            int intPremAdjID = 0;
            int intPremAdjPgrID = 0;
            int intPremAdjPgrSetupID = 0;
            int expected = 0;
            IList<PremiumAdjustmentEscrowBE> actual;
            actual = target.GetPremAdjEscrowInfo(intaccountID, intPremAdjPerdID, intPremAdjID, intPremAdjPgrID, intPremAdjPgrSetupID);
            Assert.AreEqual(expected, actual.Count);
        }

      
    }
}
