using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentPeriodBSTest and is intended
    ///to contain all PremiumAdjustmentPeriodBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentPeriodBSTest
    {


        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PremiumAdjustmentBE premAdjBE;
        static PremAdjustmentBS premAdjBS;
        static PremiumAdjustmentPeriodBE premAdjPeriodBE;
        static PremiumAdjustmentPeriodBS target;
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
            target = new PremiumAdjustmentPeriodBS();
            AddPremiumAdjustmentPeriodData();
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
        /// To meet F.key in PremAdjPerd Table 
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
            programPeriodBE.STRT_DT = System.DateTime.Now;
            programPeriodBE.PLAN_END_DT = System.DateTime.Now;
            programPeriodBE.CREATE_DATE = System.DateTime.Now;
            programPeriodBE.CREATE_USER_ID = 1;
            programPeriodsBS.Update(programPeriodBE);
        }
        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj   Table 
        /// To meet F.key in PremAdjPerd Table 
        /// </summary>
        private static void AddPremiumAdjustmentData()
        {
            premAdjBE = new PremiumAdjustmentBE();
            premAdjBE.CUSTOMERID = accountBE.CUSTMR_ID;
            premAdjBE.VALN_DT = System.DateTime.Now;
            premAdjBE.CRTE_DT = System.DateTime.Now;
            premAdjBE.CRTE_USER_ID = 1;
            premAdjBS.Update(premAdjBE);

        }
        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj Period  Table 
        /// To meet F.key in PremAdjPerd Table 
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
            target.Save(premAdjPeriodBE);
        }
        /// <summary>
        /// a Test for adding a Record  With Real Data 
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            PremiumAdjustmentPeriodBE premAdjPerdBE = new PremiumAdjustmentPeriodBE();
            premAdjPerdBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            premAdjPerdBE.REG_CUSTMR_ID = accountBE.CUSTMR_ID;
            premAdjPerdBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            premAdjPerdBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            premAdjPerdBE.CREATE_USER_ID = 1;
            premAdjPerdBE.CREATE_DATE = System.DateTime.Now;
            actual=(new PremiumAdjustmentPeriodBS()).Save(premAdjPeriodBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for adding New Record With NULL(with out Passing Any Data)
        /// the Add Method Should Rise an Exception
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            PremiumAdjustmentPeriodBS PAMIBS = new PremiumAdjustmentPeriodBS();
            PremiumAdjustmentPeriodBE PAMIBE = null;
            bool expected = false;
            bool actual;
            actual = PAMIBS.Update(PAMIBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update a Record With Real Data 
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            PremiumAdjustmentPeriodBE premAdjPerdBE = (new PremiumAdjustmentPeriodBS()).getPremAdjPerdRow(premAdjPeriodBE.PREM_ADJ_PERD_ID);
            premAdjPerdBE.ADJ_NBR_TXT = "Second Adjustment Number";
            premAdjPerdBE.UPDATE_DATE = System.DateTime.Now;
            premAdjPerdBE.UPDATE_USER_ID = 1;
            actual = (new PremiumAdjustmentPeriodBS()).Update(premAdjPerdBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update a record With NULL(i.e With out Passing any Values)
        ///it should Rise Exception
        ///</summary>
        [TestMethod()]
        public void UpdateTestWITHNULL()
        {
            PremiumAdjustmentPeriodBS target = new PremiumAdjustmentPeriodBS();
            PremiumAdjustmentPeriodBE premAdjPerdBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(premAdjPerdBE);
            Assert.AreEqual(expected, actual);


        }

        /// <summary>
        ///A test for GetProgramPeriods
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodsTest()
        {
            PremiumAdjustmentPeriodBS target = new PremiumAdjustmentPeriodBS();
            int intPremAdjID = premAdjBE.PREMIUM_ADJ_ID; 
            int expected = 0; 
            IList<PremiumAdjustmentPeriodBE> actual;
            actual = target.GetProgramPeriods(intPremAdjID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getPremAdjPerdRow
        ///</summary>
        [TestMethod()]
        public void getPremAdjPerdRowTest()
        {
            PremiumAdjustmentPeriodBS target = new PremiumAdjustmentPeriodBS();
            int intPremAdjPerdID = premAdjPeriodBE.PREM_ADJ_PERD_ID; 
            PremiumAdjustmentPeriodBE expected = null; 
            PremiumAdjustmentPeriodBE actual;
            actual = target.getPremAdjPerdRow(intPremAdjPerdID);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPremAdjPerdID
        ///</summary>
        [TestMethod()]
        public void getPremAdjPerdIDTest()
        {
            PremiumAdjustmentPeriodBS target = new PremiumAdjustmentPeriodBS();
            int intCustmerID = accountBE.CUSTMR_ID;
            int intPremAdjID = premAdjBE.PREMIUM_ADJ_ID;
            int intPremAdjPgrmID = programPeriodBE.PREM_ADJ_PGM_ID; 
            int expected = 0; 
            IList<PremiumAdjustmentPeriodBE> actual;
            actual = target.getPremAdjPerdID(intCustmerID, intPremAdjID, intPremAdjPgrmID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetAdjNumberList
        ///</summary>
        [TestMethod()]
        public void GetAdjNumberListTest()
        {
            PremiumAdjustmentPeriodBS target = new PremiumAdjustmentPeriodBS();
            int intPremAdjID = premAdjBE.PREMIUM_ADJ_ID; 
            int expected = 0; 
            IList<PremiumAdjustmentPeriodBE> actual;
            actual = target.GetAdjNumberList(intPremAdjID, premAdjPeriodBE.PREM_ADJ_PGM_ID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        
    }
}
