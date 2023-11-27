using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjMiscInvoiceBSTest and is intended
    ///to contain all PremiumAdjMiscInvoiceBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjMiscInvoiceBSTest
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
        static PostingTransactionTypeBE postTransTypBE;
        static PostingTransactionTypeBS postTransTypBS;
        static PremiumAdjMiscInvoiceBE PreAdjMiscInvoiceBE;
        static PremiumAdjMiscInvoiceBS target;
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
            //Post Trans Typ Table
            postTransTypBS = new PostingTransactionTypeBS();
            AddPostTransTypData();
            //Prem Adj Misc Invoce Table
            target = new PremiumAdjMiscInvoiceBS();
            AddMiscInvoiceCommonData();
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
            accountBE.FULL_NM = "venkat"+System.DateTime.Now.ToString();
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
            premAdjBE.CUSTOMERID= accountBE.CUSTMR_ID;
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
        /// a method for adding New Record when the class is initiated into Post Trans Type Table
        /// </summary>
        private static void AddPostTransTypData()
        {
             postTransTypBE = new PostingTransactionTypeBE();
             postTransTypBE.TRANS_TXT = "abc";
             postTransTypBE.Created_UserID = 1;
             postTransTypBE.Created_Date = System.DateTime.Now;
             postTransTypBS.Save(postTransTypBE);
        }
        /// <summary>
        /// a method for adding New Record when the class is initiated into Prem Adj Misc Invoice Table
        /// </summary>
        private static void AddMiscInvoiceCommonData()
        {

            PreAdjMiscInvoiceBE = new PremiumAdjMiscInvoiceBE();
            PreAdjMiscInvoiceBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            PreAdjMiscInvoiceBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            PreAdjMiscInvoiceBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            PreAdjMiscInvoiceBE.POST_TRANS_TYP_ID = postTransTypBE.POST_TRANS_TYP_ID;
            PreAdjMiscInvoiceBE.POST_AMT = 1000;
            PreAdjMiscInvoiceBE.POL_SYM_TXT = "WCQ";
            PreAdjMiscInvoiceBE.POL_NBR_TXT = "120000";
            PreAdjMiscInvoiceBE.POL_MODULUS_TXT = "01";
            PreAdjMiscInvoiceBE.CREATE_DATE = System.DateTime.Now;
            PreAdjMiscInvoiceBE.CREATE_USER_ID = 1;
            target.Update(PreAdjMiscInvoiceBE);


        }
        /// <summary>
        /// a Test for adding a Record  With Real Data in to Prem Adj Misc Invoice Table
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            PremiumAdjMiscInvoiceBE PAMIBE = new PremiumAdjMiscInvoiceBE();
            PAMIBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            PAMIBE.PREM_ADJ_PERD_ID = premAdjPeriodBE.PREM_ADJ_PERD_ID;
            PAMIBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            PAMIBE.POST_TRANS_TYP_ID = postTransTypBE.POST_TRANS_TYP_ID;
            PAMIBE.POST_AMT = 1000;
            PAMIBE.POL_SYM_TXT = "WCQ";
            PAMIBE.POL_NBR_TXT = "120000";
            PAMIBE.POL_MODULUS_TXT = "01";
            PAMIBE.CREATE_DATE = System.DateTime.Now;
            PAMIBE.CREATE_USER_ID = 1;
            actual = (new PremiumAdjMiscInvoiceBS()).Update(PAMIBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for adding New Record With NULL(with out Passing Any Data)
        /// the Add Method Should Rise an Exception
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            PremiumAdjMiscInvoiceBS PAMIBS = new PremiumAdjMiscInvoiceBS();
            PremiumAdjMiscInvoiceBE PAMIBE = null;
            bool expected = false;
            bool actual;
            actual = PAMIBS.Update(PAMIBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update a Record With Real Data into Prem Adj Misc Invoice Table
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            PremiumAdjMiscInvoiceBE PAMIBE = (new PremiumAdjMiscInvoiceBS()).getMiscInvoiceRow(PreAdjMiscInvoiceBE.PREM_ADJ_MISC_INVC_ID);
            PAMIBE.POST_AMT = 2000;
            PAMIBE.UPDATE_DATE = System.DateTime.Now;
            PAMIBE.UPDATE_USER_ID = 1;
            actual = (new PremiumAdjMiscInvoiceBS()).Update(PAMIBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update a record With NULL(i.e With out Passing any Values)
        ///it should Rise Exception
        ///</summary>
        [TestMethod()]
        public void UpdateTestWITHNULL()
        {
            PremiumAdjMiscInvoiceBS target = new PremiumAdjMiscInvoiceBS();
            PremiumAdjMiscInvoiceBE PreAdjMiscInvoiceBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(PreAdjMiscInvoiceBE);
            Assert.AreEqual(expected, actual);
            

        }

       
        /// <summary>
        ///A test for Retrieving Records from PremAdj Misc Invoice Table Uisng getMiscInvoiceRow Method
        ///By passing PREM_ADJ_MISC_INVC_ID
        ///</summary>
        [TestMethod()]
        public void getMiscInvoiceRowTestWithData()
        {
            PremiumAdjMiscInvoiceBE expected = null;
            PremiumAdjMiscInvoiceBE actual;
            actual = target.getMiscInvoiceRow(PreAdjMiscInvoiceBE.PREM_ADJ_MISC_INVC_ID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for for Retrieving Records from PremAdj Misc Invoice Table Uisng GetMiscInvoicelist Method 
        ///By Passing CUSTMR_ID,PREM_ADJ_PERD_ID,PREMIUM_ADJ_ID as Zero's
        ///</summary>
        [TestMethod()]
        public void GetMiscInvoicelistTest()
        {
            int intaccountID = 0;
            int intpremperdID = 0;
            int intpremadjID = 0;
            int expected = 0;
            IList<PremiumAdjMiscInvoiceBE> actual;
            actual = target.GetMiscInvoicelist(intaccountID, intpremperdID, intpremadjID);
            Assert.AreEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for for Retrieving Records from PremAdj Misc Invoice Table Uisng getMiscInvoiceRow Method
        ///By Passing zero
        ///The method should not Retrive any Records
        ///</summary>
        [TestMethod()]
        public void getMiscInvoiceRowTest()
        {
            int intPremAdjMiscInvoiceID = 0;
            PremiumAdjMiscInvoiceBE expected = null;
            PremiumAdjMiscInvoiceBE actual;
            actual = target.getMiscInvoiceRow(intPremAdjMiscInvoiceID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for for Retrieving Records from PremAdj Misc Invoice Table Uisng GetMiscInvoicelist Method 
        ///By Passing CUSTMR_ID,PREM_ADJ_PERD_ID,PREMIUM_ADJ_ID Values
        ///</summary>
        [TestMethod()]
        public void GetMiscInvoicelistTestWithData()
        {
            int expected = 0;
            IList<PremiumAdjMiscInvoiceBE> actual;
            actual = target.GetMiscInvoicelist(accountBE.CUSTMR_ID, premAdjPeriodBE.PREM_ADJ_PERD_ID, premAdjBE.PREMIUM_ADJ_ID);
            Assert.AreNotEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for for Retrieving Records from PremAdj Misc Invoice Table Uisng GetMiscInvoicelist Method 
        ///By Passing CUSTMR_ID,PREM_ADJ_PERD_ID,PREMIUM_ADJ_ID as Zero's
        ///</summary>
        [TestMethod()]
        public void GetMiscInvoicelistTestWITHNULL()
        {
            int intaccountID = 0;
            int intpremperdID = 0;
            int intpremadjID = 0;
            int expected = 0;
            IList<PremiumAdjMiscInvoiceBE> actual;
            actual = target.GetMiscInvoicelist(intaccountID, intpremperdID, intpremadjID);
            Assert.AreEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for for Retrieving Records from PremAdj Misc Invoice Table 
        ///
        ///</summary>
        //[TestMethod()]
        //public void getPremAdjMiscInvoiceTypeLookUp()
        //{
        //    int expected = 0;
        //    IList<LookupBE> actual;
        //    actual = target.getPremAdjMiscInvoiceTypeLookUp();
        //    Assert.AreNotEqual(expected, actual.Count);
        //}
       
    }
}
