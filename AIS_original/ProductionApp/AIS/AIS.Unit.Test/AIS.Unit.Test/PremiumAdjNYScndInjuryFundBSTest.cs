using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjNYScndInjuryFundBSTest and is intended
    ///to contain all PremiumAdjNYScndInjuryFundBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjNYScndInjuryFundBSTest
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
        static PremiumAdjNYScndInjuryFundBE PreAdjNYSIFBE;
        static PremiumAdjNYScndInjuryFundBS target;
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
            //Prem Adj PremAdjNYSIF Table
            target = new PremiumAdjNYScndInjuryFundBS();
            AddNYSIFCommonData();
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
        /// To meet F.key in PremAdjNYSIF Table 
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
        /// To meet F.key in PremAdjNYSIF Table 
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
        /// To meet F.key in PremAdjNYSIF Table 
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
        /// a method for adding New Record when the class is initiated into PremAdjNYSIF Table
        /// </summary>
        private static void AddNYSIFCommonData()
        {
            PreAdjNYSIFBE = new PremiumAdjNYScndInjuryFundBE();
            PreAdjNYSIFBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            PreAdjNYSIFBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            PreAdjNYSIFBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            PreAdjNYSIFBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            PreAdjNYSIFBE.INCUR_LOS_AMT = 1000;
            PreAdjNYSIFBE.LOS_CONV_FCTR_RT = 1;
            PreAdjNYSIFBE.CNVT_LOS_AMT = 1000;
            PreAdjNYSIFBE.BASIC_DEDTBL_PREM_AMT = 1000;
            PreAdjNYSIFBE.TAX_MULTI_RT = 1;
            PreAdjNYSIFBE.CNVT_TOT_LOS_AMT = 1000;
            PreAdjNYSIFBE.NY_PREM_DISC_AMT = 100;
            PreAdjNYSIFBE.NY_SCND_INJR_FUND_AUDT_AMT = 2000;
            PreAdjNYSIFBE.NY_TAX_DUE_AMT = 200;
            PreAdjNYSIFBE.PREV_RSLT_AMT = 3000;
            PreAdjNYSIFBE.CURR_ADJ_AMT = 2000;
            PreAdjNYSIFBE.BASIC_CNVT_LOS_AMT = 4000;
            PreAdjNYSIFBE.NY_ERND_RETRO_PREM_AMT = 200;
            PreAdjNYSIFBE.NY_SCND_INJR_FUND_RT = 2;
            PreAdjNYSIFBE.REVD_NY_SCND_INJR_FUND_AMT = 2000;
            PreAdjNYSIFBE.CREATE_USER_ID = 1;
            PreAdjNYSIFBE.CREATE_DATE = System.DateTime.Now;
            target.Update(PreAdjNYSIFBE);
        }
        /// <summary>
        /// a Test for adding a Record  With Real Data in to PremAdjNYSIF Table
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            PremiumAdjNYScndInjuryFundBE PANYSIFBE = new PremiumAdjNYScndInjuryFundBE();
            PANYSIFBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            PANYSIFBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            PANYSIFBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            PANYSIFBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            PANYSIFBE.INCUR_LOS_AMT = 1000;
            PANYSIFBE.LOS_CONV_FCTR_RT = 1;
            PANYSIFBE.CNVT_LOS_AMT = 1000;
            PANYSIFBE.BASIC_DEDTBL_PREM_AMT = 1000;
            PANYSIFBE.TAX_MULTI_RT = 1;
            PANYSIFBE.CNVT_TOT_LOS_AMT = 1000;
            PANYSIFBE.NY_PREM_DISC_AMT = 100;
            PANYSIFBE.NY_SCND_INJR_FUND_AUDT_AMT = 2000;
            PANYSIFBE.NY_TAX_DUE_AMT = 200;
            PANYSIFBE.PREV_RSLT_AMT = 3000;
            PANYSIFBE.CURR_ADJ_AMT = 2000;
            PANYSIFBE.BASIC_CNVT_LOS_AMT = 4000;
            PANYSIFBE.NY_ERND_RETRO_PREM_AMT = 200;
            PANYSIFBE.NY_SCND_INJR_FUND_RT = 2;
            PANYSIFBE.REVD_NY_SCND_INJR_FUND_AMT = 2000;
            PANYSIFBE.CREATE_USER_ID = 1;
            PANYSIFBE.CREATE_DATE = System.DateTime.Now;
            actual = (new PremiumAdjNYScndInjuryFundBS()).Update(PANYSIFBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for adding New Record With NULL(with out Passing Any Data)
        /// the Add Method Should Rise an Exception
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            PremiumAdjNYScndInjuryFundBS PANYSIFBS = new PremiumAdjNYScndInjuryFundBS();
            PremiumAdjNYScndInjuryFundBE PANYSIFBE = null;
            bool expected = false;
            bool actual;
            actual = PANYSIFBS.Update(PANYSIFBE);
            Assert.AreEqual(expected, actual);

        }
       
        /// <summary>
        ///A test for Update a record With NULL(i.e With out Passing any Values)
        ///it should Rise Exception
        ///</summary>
        [TestMethod()]
        public void UpdateTestWITHNULL()
        {
            PremiumAdjNYScndInjuryFundBS PANYSIFBS = new PremiumAdjNYScndInjuryFundBS();
            PremiumAdjNYScndInjuryFundBE PANYSIFBE = null;
            bool expected = false;
            bool actual;
            actual = PANYSIFBS.Update(PANYSIFBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update a Record With Real Data into PremAdjNYSIF Table
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            PremiumAdjNYScndInjuryFundBE PANSIF = new PremiumAdjNYScndInjuryFundBS().getPremiumAdjustmentRow(PreAdjNYSIFBE.PREM_ADJ_NY_SCND_INJR_FUND_ID);
            PANSIF.INCUR_LOS_AMT = 2000;
            PANSIF.NY_ERND_RETRO_PREM_AMT = 5000;
            PANSIF.NY_SCND_INJR_FUND_RT = 5;
            PANSIF.NY_PREM_DISC_AMT = 300;
            PANSIF.UPDATE_DATE = System.DateTime.Now;
            PANSIF.UPDATE_USER_ID = 1;
            actual = (new PremiumAdjNYScndInjuryFundBS()).Update(PANSIF);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getPremiumAdjustmentRow
        ///</summary>
        [TestMethod()]
        public void getPremiumAdjustmentRowTestWITHData()
        {
            PremiumAdjNYScndInjuryFundBE expected = null;
            PremiumAdjNYScndInjuryFundBE actual;
            actual = target.getPremiumAdjustmentRow(PreAdjNYSIFBE.PREM_ADJ_NY_SCND_INJR_FUND_ID);
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for getPremiumAdjustmentRow
        ///</summary>
        [TestMethod()]
        public void getPremiumAdjustmentRowTest()
        {
            int ID = 0;
            PremiumAdjNYScndInjuryFundBE expected = null;
            PremiumAdjNYScndInjuryFundBE actual;
            actual = target.getPremiumAdjustmentRow(ID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for getPremAdjNYSIFRow
        ///</summary>
        [TestMethod()]
        public void getPremAdjNYSIFRowTestWithData()
        {
            PremiumAdjNYScndInjuryFundBE expected = null;
            PremiumAdjNYScndInjuryFundBE actual;
            actual = target.getPremAdjNYSIFRow(PreAdjNYSIFBE.PREM_ADJ_NY_SCND_INJR_FUND_ID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPremAdjNYSIFRow
        ///</summary>
        [TestMethod()]
        public void getPremAdjNYSIFRowTest()
        {
            int intPremAdjNYSIFID = 0;
            PremiumAdjNYScndInjuryFundBE expected = null;
            PremiumAdjNYScndInjuryFundBE actual;
            actual = target.getPremAdjNYSIFRow(intPremAdjNYSIFID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getPremAdjNYSIFRow
        ///</summary>
        [TestMethod()]
        public void getPremAdjNYSIFRowTestWithNull()
        {
            int intPremAdjNYSIFID = 0;
            PremiumAdjNYScndInjuryFundBE expected = null;
            PremiumAdjNYScndInjuryFundBE actual;
            actual = target.getPremAdjNYSIFRow(intPremAdjNYSIFID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPremAdjNYSIF
        ///</summary>
        [TestMethod()]
        public void getPremAdjNYSIFTestWithData()
        {
            int expected = 0;
            IList<PremiumAdjNYScndInjuryFundBE> actual;
            actual = target.getPremAdjNYSIF(accountBE.CUSTMR_ID, premAdjBE.PREMIUM_ADJ_ID, premAdjPeriodBE.PREM_ADJ_PERD_ID, programPeriodBE.PREM_ADJ_PGM_ID);
            Assert.AreNotEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for getPremAdjNYSIF
        ///</summary>
        [TestMethod()]
        public void getPremAdjNYSIFTestWithNULL()
        {
            int intAccountID = 0;
            int intPremAdjID = 0;
            int intPremAdjPerdID = 0;
            int intPremAdjPgrID = 0;
            int expected = 0;
            IList<PremiumAdjNYScndInjuryFundBE> actual;
            actual = target.getPremAdjNYSIF(intAccountID, intPremAdjID, intPremAdjPerdID,intPremAdjPgrID);
            Assert.AreEqual(expected, actual.Count);

        }
               
    }
}
