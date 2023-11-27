using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjCombElementsBSTest and is intended
    ///to contain all PremiumAdjCombElementsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjCombElementsBSTest
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
        static PremiumAdjCombElementsBE PreAdjCombElementsBE;
        static PremiumAdjCombElementsBS target;
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
            //Prem Adj Comb Elemnts Table
            target = new PremiumAdjCombElementsBS();
            AddPremAdjCombElemntsCommonData();
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
        /// To meet F.key in PremAdjCombElemnts Table 
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
        /// To meet F.key in PremAdjCombElemnts Table 
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
        /// To meet F.key in PremAdjCombElemnts Table 
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
        /// a method for adding New Record when the class is initiated into PremAdjCombElemnts Table
        /// </summary>
        private static void AddPremAdjCombElemntsCommonData()
        {

            PreAdjCombElementsBE = new PremiumAdjCombElementsBE();
            PreAdjCombElementsBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            PreAdjCombElementsBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            PreAdjCombElementsBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            PreAdjCombElementsBE.RETRO_BASIC_PREM_AMT = 1000;
            PreAdjCombElementsBE.RETRO_LOS_FCTR_AMT = 100;
            PreAdjCombElementsBE.RETRO_TAX_MULTI_RT = 5;
            PreAdjCombElementsBE.RETRO_SUBTOT_AMT = 1000;
            PreAdjCombElementsBE.DEDTBL_SUBTOT_AMT = 1000;
            PreAdjCombElementsBE.DEDTBL_MAX_LESS_AMT = 100;
            PreAdjCombElementsBE.DEDTBL_MAX_AMT = 100;
            PreAdjCombElementsBE.DEDTBL_MIN_AMT = 100;
            PreAdjCombElementsBE.CREATE_USER_ID = 1;
            PreAdjCombElementsBE.CREATE_DATE = System.DateTime.Now;
            target.Update(PreAdjCombElementsBE);


        }
        /// <summary>
        /// a Test for adding a Record  With Real Data in to PremAdjCombElemnts Table
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            PremiumAdjCombElementsBE PACEBE = new PremiumAdjCombElementsBE();
            PACEBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            PACEBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            PACEBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            PACEBE.RETRO_BASIC_PREM_AMT = 1000;
            PACEBE.RETRO_LOS_FCTR_AMT = 100;
            PACEBE.RETRO_TAX_MULTI_RT = 5;
            PACEBE.RETRO_SUBTOT_AMT = 1000;
            PACEBE.DEDTBL_SUBTOT_AMT = 1000;
            PACEBE.DEDTBL_MAX_LESS_AMT = 100;
            PACEBE.DEDTBL_MAX_AMT = 100;
            PACEBE.DEDTBL_MIN_AMT = 100;
            PACEBE.CREATE_USER_ID = 1;
            PACEBE.CREATE_DATE = System.DateTime.Now;
            actual = (new PremiumAdjCombElementsBS()).Update(PACEBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for adding New Record With NULL(with out Passing Any Data)
        /// the Add Method Should Rise an Exception
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            PremiumAdjCombElementsBS PACEBS = new PremiumAdjCombElementsBS();
            PremiumAdjCombElementsBE PACEBE = null;
            bool expected = false;
            bool actual;
            actual = PACEBS.Update(PACEBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update a Record With Real Data into PremAdjCombElemnts Table
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            PremiumAdjCombElementsBE PACombBE = (new PremiumAdjCombElementsBS()).getPremAdjCombElementsRow(PreAdjCombElementsBE.PREM_ADJ_COMB_ELEMNTS_ID);
            PACombBE.RETRO_SUBTOT_AMT = 2000;
            PACombBE.RETRO_BASIC_PREM_AMT = 2000;
            PACombBE.UPDATE_DATE = System.DateTime.Now;
            PACombBE.UPDATE_USER_ID = 1;
            actual = target.Update(PACombBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update a record With NULL(i.e With out Passing any Values)
        ///it should Rise Exception
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            PremiumAdjCombElementsBS target = new PremiumAdjCombElementsBS();
            PremiumAdjCombElementsBE PreAdjCombElementsBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(PreAdjCombElementsBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPremAdjCombElementsRow
        ///</summary>
        [TestMethod()]
        public void getPremAdjCombElementsRowTestWithData()
        {
            PremiumAdjCombElementsBE expected = null;
            PremiumAdjCombElementsBE actual;
            actual = target.getPremAdjCombElementsRow(PreAdjCombElementsBE.PREM_ADJ_COMB_ELEMNTS_ID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPremAdjCombElementsRow
        ///</summary>
        [TestMethod()]
        public void getPremAdjCombElementsRowTestWithNULL()
        {
            int intPremAdjCombElementsID = 0;
            PremiumAdjCombElementsBE expected = null;
            PremiumAdjCombElementsBE actual;
            actual = target.getPremAdjCombElementsRow(intPremAdjCombElementsID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPremAdjCombElements
        ///</summary>
        [TestMethod()]
        public void getPremAdjCombElementsTestWithData()
        {
            int expected = 0;
            IList<PremiumAdjCombElementsBE> actual;
            actual = target.getPremAdjCombElements(accountBE.CUSTMR_ID, premAdjBE.PREMIUM_ADJ_ID, premAdjPeriodBE.PREM_ADJ_PERD_ID);
            Assert.AreNotEqual(expected, actual.Count);

        }

        /// <summary>
        ///A test for getPremAdjCombElements
        ///</summary>
        [TestMethod()]
        public void getPremAdjCombElementsTest()
        {
            int intAccountID = 0;
            int intPremAdjID = 0;
            int intPremAdjPerdID = 0;
            int expected = 0;
            IList<PremiumAdjCombElementsBE> actual;
            actual = target.getPremAdjCombElements(intAccountID, intPremAdjID, intPremAdjPerdID);
            Assert.AreEqual(expected, actual.Count);

        }
    }
}
