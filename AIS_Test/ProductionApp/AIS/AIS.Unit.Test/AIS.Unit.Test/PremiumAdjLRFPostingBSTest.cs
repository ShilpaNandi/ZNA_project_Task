using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System.Data;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjLRFPostingBSTest and is intended
    ///to contain all PremiumAdjLRFPostingBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjLRFPostingBSTest
    {
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PremiumAdjustmentBE premAdjBE;
        static PremAdjustmentBS premAdjBS;
        static PremiumAdjustmentPeriodBE premAdjPeriodBE;
        static PremiumAdjustmentPeriodBS premAdjPeriodBS;
        static PremiumAdjLRFPostingBE paLRFBE;
        static PremiumAdjLRFPostingBS target;

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
            
            //Prem Adj Misc Invoce Table
            target = new PremiumAdjLRFPostingBS();
            AddLRFPostingCommonData();
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
            accountBE.FULL_NM = "Pralaya" + System.DateTime.Now.ToString();
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
        /// To meet F.key in PremAdjMIscInvoice Table 
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
        /// To meet F.key in PremAdjMIscInvoice Table 
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
        /// a method for adding New Record when the class is initiated into Prem Adj Misc Invoice Table
        /// </summary>
        private static void AddLRFPostingCommonData()
        {

            paLRFBE = new PremiumAdjLRFPostingBE();
            paLRFBE.PremAdjID = premAdjBE.PREMIUM_ADJ_ID;
            paLRFBE.PremAdjPerdID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            paLRFBE.CustomerID = accountBE.CUSTMR_ID;
            //paLRFBE.POST_TRANS_TYP_ID = postTransTypBE.POST_TRANS_TYP_ID;

            paLRFBE.CreatedDate = System.DateTime.Now;
            paLRFBE.CreatedUserID = 1;
            target.Update(paLRFBE);


        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            paLRFBE = new PremiumAdjLRFPostingBE();
            paLRFBE.PremAdjID = premAdjBE.PREMIUM_ADJ_ID;
            paLRFBE.PremAdjPerdID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            paLRFBE.CustomerID = accountBE.CUSTMR_ID;
            //paLRFBE.POST_TRANS_TYP_ID = postTransTypBE.POST_TRANS_TYP_ID;

            paLRFBE.CreatedDate = System.DateTime.Now;
            paLRFBE.CreatedUserID = 1;
            target.Update(paLRFBE);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNULL()
        {
            PremiumAdjLRFPostingBS target = new PremiumAdjLRFPostingBS();
            PremiumAdjLRFPostingBE paLRFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(paLRFBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {

            paLRFBE = new PremiumAdjLRFPostingBE();
            paLRFBE.PremAdjID = premAdjBE.PREMIUM_ADJ_ID;
            paLRFBE.PremAdjPerdID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            paLRFBE.CustomerID = accountBE.CUSTMR_ID;
            //paLRFBE.POST_TRANS_TYP_ID = postTransTypBE.POST_TRANS_TYP_ID;

            paLRFBE.CreatedDate = System.DateTime.Now;
            paLRFBE.CreatedUserID = 1;
            paLRFBE.UpdatedDate = System.DateTime.Now;
            paLRFBE.UpdatedUser_ID = 1;
            bool expected = true;
            bool actual;
            actual = target.Update(paLRFBE);
            Assert.AreEqual(expected, actual);


        }
        /// <summary>
        ///A test for getRelatedComments with data
        ///</summary>
        [TestMethod()]
        public void getRelatedCommentsTestWithData()
        {
           
        }



        /// <summary>
        ///A test for getPreAdjLRFRow
        ///</summary>
        [TestMethod()]
        public void getPreAdjLRFRowTest()
        {
            PremiumAdjLRFPostingBE expected = null;
            PremiumAdjLRFPostingBE actual;
            actual = target.getPreAdjLRFRow(paLRFBE.Prem_Adj_Los_Reim_Fund_Post_ID);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);

            
        }
        
            /// <summary>
        ///A test for getLRFData
        ///</summary>
        [TestMethod()]
        public void getLRFDataTest()
        {
            int acctID = 1; 
            int prmAdjID = 1; 
            int prmAdjPrdID = 1; 
            int expected = 1;
            DataTable actual;
            //IList<PremiumAdjLRFPostingBE> actual;
            actual = target.getLRFData(prmAdjID, prmAdjPrdID, acctID);
            Assert.AreNotEqual(expected, actual.Rows.Count);
            
            
            
        }

        /// <summary>
        ///A test for getLRFList
        ///</summary>
        [TestMethod()]
        public void getLRFListTest()
        {
            int acctID = 1; 
            int prmAdjID = 1; 
            int prmAdjPrdID = 1; 
            int expected = 1;
            IList<PremiumAdjLRFPostingBE> actual;
            actual = target.getLRFList(acctID, prmAdjID, prmAdjPrdID);
            Assert.AreNotEqual(expected, actual.Count);
            
            
            
        }

        /// <summary>
        ///A test for getList
        ///</summary>
        [TestMethod()]
        public void getListTest()
        {
            int expected = 1;
            IList<PremiumAdjLRFPostingBE> actual;
            actual = target.getList();
            Assert.AreNotEqual(expected, actual.Count);
            
        }

        /// <summary>
        ///A test for PremiumAdjLRFPostingBS Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjLRFPostingBSConstructorTest()
        {
            PremiumAdjLRFPostingBS target = new PremiumAdjLRFPostingBS();
            
        }
    }
}
