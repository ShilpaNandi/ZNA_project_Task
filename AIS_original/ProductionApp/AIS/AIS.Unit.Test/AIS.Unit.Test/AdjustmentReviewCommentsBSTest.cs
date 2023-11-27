using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjustmentReviewCommentsBSTest and is intended
    ///to contain all AdjustmentReviewCommentsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjustmentReviewCommentsBSTest
    {

        static AccountBE accountBE;
        static AccountBS accountBS;
        static PremiumAdjustmentBE premiumAdjustmentBE;
        static PremAdjustmentBS premAdjustmentBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PremiumAdjustmentPeriodBE premiumAdjustmentPeriodBE;
        static PremiumAdjustmentPeriodBS premiumAdjustmentPeriodBS;
        static AdjustmentReviewCommentsBE adjrevcmmntBE;
        static AdjustmentReviewCommentsBS target;

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
            premAdjustmentBS = new PremAdjustmentBS();
            AddPremiumAdjData();
            programPeriodsBS = new ProgramPeriodsBS();
            AddProgramPeriodData();
            premiumAdjustmentPeriodBS = new PremiumAdjustmentPeriodBS();
            AddPremiumAdjPeriodData();
            target = new AdjustmentReviewCommentsBS();
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
            accountBE.FULL_NM = "Janny1" + System.DateTime.Now;
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }

        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddPremiumAdjData()
        {
            premiumAdjustmentBE = new PremiumAdjustmentBE();
            premiumAdjustmentBE.CUSTOMERID = accountBE.CUSTMR_ID;
            premiumAdjustmentBE.VALN_DT = System.DateTime.Now;
            premiumAdjustmentBE.CRTE_DT = System.DateTime.Now;
            premiumAdjustmentBE.CRTE_USER_ID = 1;
            premAdjustmentBS.Save(premiumAdjustmentBE);
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
        private static void AddPremiumAdjPeriodData()
        {
            premiumAdjustmentPeriodBE = new PremiumAdjustmentPeriodBE();
            premiumAdjustmentPeriodBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            premiumAdjustmentPeriodBE.PREM_ADJ_ID = premiumAdjustmentBE.PREMIUM_ADJ_ID;
            premiumAdjustmentPeriodBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            premiumAdjustmentPeriodBE.CREATE_DATE = System.DateTime.Now;
            premiumAdjustmentPeriodBE.CREATE_USER_ID = 1;
            premiumAdjustmentPeriodBS.Save(premiumAdjustmentPeriodBE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddData()
        {
            adjrevcmmntBE = new AdjustmentReviewCommentsBE();
            adjrevcmmntBE.PREM_ADJ_ID = premiumAdjustmentBE.PREMIUM_ADJ_ID;
            adjrevcmmntBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            adjrevcmmntBE.PREM_ADJ_PERD_ID = premiumAdjustmentPeriodBE.PREM_ADJ_PERD_ID;
            adjrevcmmntBE.CMMNT_CATG_ID= 315;
            adjrevcmmntBE.CMMNT_TXT = "Hello";
            adjrevcmmntBE.CREATEDATE = System.DateTime.Now;
            adjrevcmmntBE.CREATEUSER = 1;
            target.Save(adjrevcmmntBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false; ;
            AdjustmentReviewCommentsBE adjrevcmmntBE = new AdjustmentReviewCommentsBE();
            adjrevcmmntBE.PREM_ADJ_ID = premiumAdjustmentBE.PREMIUM_ADJ_ID;
            adjrevcmmntBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            adjrevcmmntBE.PREM_ADJ_PERD_ID = premiumAdjustmentPeriodBE.PREM_ADJ_PERD_ID;
            adjrevcmmntBE.CMMNT_CATG_ID = 319;
            adjrevcmmntBE.CMMNT_TXT = "Hello!";
            adjrevcmmntBE.CREATEDATE = System.DateTime.Now;
            adjrevcmmntBE.CREATEUSER = 1;
            actual = target.Save(adjrevcmmntBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            bool expected = true;
            bool actual;
            adjrevcmmntBE.CMMNT_TXT = "Hello! updated";
            adjrevcmmntBE.UPDATEDDATE = System.DateTime.Now;
            adjrevcmmntBE.UPDATEDUSER = 1;
            actual = target.Update(adjrevcmmntBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            AdjustmentReviewCommentsBS target = new AdjustmentReviewCommentsBS();
            AdjustmentReviewCommentsBE adjrewcmmntBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(adjrewcmmntBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getAdjustmentReviewCommentsRowTest with Null
        ///</summary>
        [TestMethod()]
        public void getAdjustmentReviewCommentsRowTestWithNUll()
        {
            AdjustmentReviewCommentsBS target = new AdjustmentReviewCommentsBS();
            int ID = 0;
            AdjustmentReviewCommentsBE expected = null;
            AdjustmentReviewCommentsBE actual;
            actual = target.getAdjustmentReviewCommentsRow(ID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getAdjustmentReviewCommentsRow
        ///</summary>
        [TestMethod()]
        public void getAdjustmentReviewCommentsRowTest()
        {
            AdjustmentReviewCommentsBE expected = null;
            AdjustmentReviewCommentsBE actual;
            actual = target.getAdjustmentReviewCommentsRow(adjrevcmmntBE.PREM_ADJ_CMMNT_ID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getAdjReviewCmmntINVOICEData
        ///</summary>
        [TestMethod()]
        public void getAdjReviewCmmntINVOICEDataTest()
        {
           // AdjustmentReviewCommentsBS target = new AdjustmentReviewCommentsBS(); 
            int cmmntCatgID = 319; // TODO: Initialize to an appropriate value
            int PrgAdjID = premiumAdjustmentBE.PREMIUM_ADJ_ID; // TODO: Initialize to an appropriate value
            int custmrID = accountBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            AdjustmentReviewCommentsBE expected = null; // TODO: Initialize to an appropriate value
            AdjustmentReviewCommentsBE actual;
            actual = target.getAdjReviewCmmntINVOICEData(cmmntCatgID, PrgAdjID, custmrID);
            Assert.AreNotEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for getAdjReviewCmmntALLData
        ///</summary>
        [TestMethod()]
        public void getAdjReviewCmmntALLDataTest()
        {
            //AdjustmentReviewCommentsBS target = new AdjustmentReviewCommentsBS();
            int cmmntCatgID = 319; // TODO: Initialize to an appropriate value
            int PrgPrdID = premiumAdjustmentPeriodBE.PREM_ADJ_PERD_ID; // TODO: Initialize to an appropriate value
            int custmrID = accountBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            AdjustmentReviewCommentsBE expected = null; // TODO: Initialize to an appropriate value
            AdjustmentReviewCommentsBE actual;
            actual = target.getAdjReviewCmmntALLData(cmmntCatgID, PrgPrdID, custmrID);
            Assert.AreNotEqual(expected, actual);
            
        }

     }
}
