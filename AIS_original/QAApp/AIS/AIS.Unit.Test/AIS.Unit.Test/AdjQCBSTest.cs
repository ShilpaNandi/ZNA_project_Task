using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjQCBSTest and is intended
    ///to contain all AdjQCBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjQCBSTest
    {
        static AccountBE AcctBE;
        static PremiumAdjustmentBE premadjBE;
        static PremiumAdjustmentStatusBE pasBE;
        static AdjQCBS target;



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
            target = new AdjQCBS();
            AddCustomerData();
            AddPremiumAdjData();
            AddCommonData();
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
        /// a method for adding New Record in Customer when the classis initiated 
        /// </summary>
        private static void AddCustomerData()
        {
            AcctBE = new AccountBE();
            AcctBE.FULL_NM = "aaa" + System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            AccountBS AcctBS = new AccountBS();
            AcctBS.Save(AcctBE);
        }
        /// <summary>
        /// a method for adding New Record in PremiumAdjustment Table when the classis initiated 
        /// </summary>
        private static void AddPremiumAdjData()
        {
            premadjBE = new PremiumAdjustmentBE();
            premadjBE.CUSTOMERID = AcctBE.CUSTMR_ID;
            premadjBE.VALN_DT = System.DateTime.Now;
            premadjBE.CRTE_DT = System.DateTime.Now;
            premadjBE.CRTE_USER_ID = 1;
            (new PremAdjustmentBS()).Save(premadjBE);
        }
        
        private static void AddCommonData()
        {
            pasBE = new PremiumAdjustmentStatusBE();
            pasBE.PremumAdj_ID = premadjBE.PREMIUM_ADJ_ID;
            pasBE.CustomerID = AcctBE.CUSTMR_ID;
            pasBE.EffectiveDate = System.DateTime.Now;
            pasBE.CreatedDate = System.DateTime.Now;
            pasBE.CreatedUser_ID = 1;
            target.Save(pasBE);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            AdjQCBS target = new AdjQCBS();
            PremiumAdjustmentStatusBE pasBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(pasBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            
            PremiumAdjustmentStatusBE pasBE1 = new PremiumAdjustmentStatusBE();
            AdjQCBS ltarget = new AdjQCBS();
            bool expected = true;
            bool actual;
            pasBE1.CustomerID = AcctBE.CUSTMR_ID;
            pasBE1.PremumAdj_ID = premadjBE.PREMIUM_ADJ_ID;
            pasBE1.EffectiveDate = System.DateTime.Now;


            pasBE1.CreatedUser_ID = 1;
            pasBE1.CreatedDate = System.DateTime.Now;
            actual=ltarget.Update(pasBE1);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            pasBE = new PremiumAdjustmentStatusBE();
            target = new AdjQCBS();
            bool expected = true;
            bool actual;
            
            pasBE.CustomerID = AcctBE.CUSTMR_ID;
            pasBE.PremumAdj_ID = premadjBE.PREMIUM_ADJ_ID;
            pasBE.EffectiveDate = System.DateTime.Now;


            pasBE.CreatedUser_ID = 1;
            pasBE.CreatedDate = System.DateTime.Now;
            actual=target.Update(pasBE);
            Assert.AreEqual(expected, actual);

        }



        /// <summary>
        ///A test for Update
        ///</summary>
        ///
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestwithNull()
        {
            AdjQCBS target = new AdjQCBS();
            PremiumAdjustmentStatusBE pasBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(pasBE);
            Assert.AreEqual(expected, actual);

        }


       

       
        /// <summary>
        ///A test for getRelatedAdjSetupQCItems
        ///</summary>
        [TestMethod()]
        public void getRelatedAdjSetupQCItemsTest()
        {
            //int adjID = 1;
            int expected = 1;
            IList<PremiumAdjustmentStatusBE> actual;
            actual = target.getRelatedAdjSetupQCItems(pasBE.PremumAdj_ID);
            Assert.AreNotEqual(expected, actual.Count);
            
        }

        /// <summary>
        ///A test for getPreAdjStatusList
        ///</summary>
        [TestMethod()]
        public void getPreAdjStatusListTest()
        {
            IList<PremiumAdjustmentStatusBE> expected = null; 
            IList<PremiumAdjustmentStatusBE> actual;
            actual = target.getPreAdjStatusList(pasBE.PremumAdj_ID);
            Assert.AreNotEqual(expected, actual.Count);

        }

        /// <summary>
        ///A test for getPreAdjPgmRow
        ///</summary>
        [TestMethod()]
        public void getPreAdjStatusRowTest()
        {
            
           // AdjQCBS target = new AdjQCBS();
            
            PremiumAdjustmentStatusBE expected = null;
            PremiumAdjustmentStatusBE actual;
            actual = target.getPreAdjStatusRow(pasBE.PremumAdj_sts_ID);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);
            

            
        }
        /// <summary>
        ///A test for getPreAdjPgmRow
        ///</summary>
        [TestMethod()]
        public void getPreAdjStatusList()
        {

            //int adjID = 1;
            int expected = 1;
            IList<PremiumAdjustmentStatusBE> actual;
            actual = target.getRelatedAdjSetupQCItems(pasBE.PremumAdj_ID);
            Assert.AreNotEqual(expected, actual.Count);



        }

        /// <summary>
        ///A test for AdjQCBS Constructor
        ///</summary>
        [TestMethod()]
        public void AdjQCBSConstructorTest()
        {
            AdjQCBS target = new AdjQCBS();
        }
    }
}
