using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PaidLossBillingBSTest and is intended
    ///to contain all PaidLossBillingBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PaidLossBillingBSTest
    {

        static PaidLossBillingBE paidLossBillingBE;
        static PaidLossBillingBS target;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static PremiumAdjustmentBE premiumAdjustmentBE;
        static PremAdjustmentBS premAdjustmentBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PremiumAdjustmentPeriodBE premiumAdjustmentPeriodBE;
        static PremiumAdjustmentPeriodBS premiumAdjustmentPeriodBS;
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
            target = new PaidLossBillingBS();
            AddData();
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
            paidLossBillingBE = new PaidLossBillingBE();
            paidLossBillingBE.PREM_ADJ_ID = premiumAdjustmentBE.PREMIUM_ADJ_ID;
            paidLossBillingBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            paidLossBillingBE.PREM_ADJ_PERD_ID = premiumAdjustmentPeriodBE.PREM_ADJ_PERD_ID;
            paidLossBillingBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            paidLossBillingBE.IDNMTY_AMT = 100;
            paidLossBillingBE.EXPS_AMT = 100;
            paidLossBillingBE.ADJ_IDNMTY_AMT = 100;
            paidLossBillingBE.ADJ_EXPS_AMT = 100;
            paidLossBillingBE.TOT_PAID_LOS_BIL_AMT = 200;
            paidLossBillingBE.ADJ_TOT_PAID_LOS_BIL_AMT = 200;
            paidLossBillingBE.CREATEDATE = System.DateTime.Now;
            paidLossBillingBE.CREATEUSER = 1;
            target.Save(paidLossBillingBE);
        }
        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false; ;
            PaidLossBillingBE paidLossBillingBE = new PaidLossBillingBE();
            paidLossBillingBE.PREM_ADJ_ID = premiumAdjustmentBE.PREMIUM_ADJ_ID;
            paidLossBillingBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            paidLossBillingBE.PREM_ADJ_PERD_ID = premiumAdjustmentPeriodBE.PREM_ADJ_PERD_ID;
            paidLossBillingBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            paidLossBillingBE.IDNMTY_AMT = 100;
            paidLossBillingBE.EXPS_AMT = 100;
            paidLossBillingBE.ADJ_IDNMTY_AMT = 100;
            paidLossBillingBE.ADJ_EXPS_AMT = 100;
            paidLossBillingBE.TOT_PAID_LOS_BIL_AMT = 200;
            paidLossBillingBE.ADJ_TOT_PAID_LOS_BIL_AMT = 200;
            paidLossBillingBE.CREATEDATE = System.DateTime.Now;
            paidLossBillingBE.CREATEUSER = 1;
            actual = target.Save(paidLossBillingBE);
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
            paidLossBillingBE.ADJ_IDNMTY_AMT = 200;
            paidLossBillingBE.ADJ_EXPS_AMT = 200;
            paidLossBillingBE.ADJ_TOT_PAID_LOS_BIL_AMT = 400;
            paidLossBillingBE.UPDATEDDATE = System.DateTime.Now;
            paidLossBillingBE.UPDATEDUSER = 1;
            actual = target.Update(paidLossBillingBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            PaidLossBillingBS target = new PaidLossBillingBS();
            PaidLossBillingBE paidLossBillingBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(paidLossBillingBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPaidLossBillingDataRowTest with Null
        ///</summary>
        [TestMethod()]
        public void getPaidLossBillingDataRowTestWithNUll()
        {
            PaidLossBillingBS target = new PaidLossBillingBS();
            int prmadjplbID = 0;
            PaidLossBillingBE expected = null;
            PaidLossBillingBE actual;
            actual = target.getPaidLossBillingDataRow(prmadjplbID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getPaidLossBillingDataRow
        ///</summary>
        [TestMethod()]
        public void getPaidLossBillingDataRowTest()
        {
            PaidLossBillingBE expected = null;
            PaidLossBillingBE actual;
            actual = target.getPaidLossBillingDataRow(paidLossBillingBE.PREM_ADJ_PAID_LOS_BIL_ID);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPaidLossBillingData
        ///</summary>
        //[TestMethod()]
        //public void getPaidLossBillingDataTest()
        //{
        //    PaidLossBillingBS target = new PaidLossBillingBS(); // TODO: Initialize to an appropriate value
        //    int custmrID = accountBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
        //    int prgID = premiumAdjustmentPeriodBE.PREM_ADJ_PERD_ID; // TODO: Initialize to an appropriate value
        //    int PrmAdjID = premiumAdjustmentBE.PREMIUM_ADJ_ID; // TODO: Initialize to an appropriate value
        //    int PrmAdjPrgID = programPeriodBE.PREM_ADJ_PGM_ID; // TODO: Initialize to an appropriate value
        //    IList<PaidLossBillingBE> expected = null; // TODO: Initialize to an appropriate value
        //    IList<PaidLossBillingBE> actual;
        //    actual = target.getPaidLossBillingData(custmrID, prgID, PrmAdjID, PrmAdjPrgID);
        //    Assert.AreEqual(expected, actual);
        //    
        //}

       
    }
}
