using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for PremiumAdjustmentStatusBSTest and is intended
    ///to contain all PremiumAdjustmentStatusBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentStatusBSTest
    {

        static AccountBE AcctBE;
        static PremiumAdjustmentBE premadjBE;
        private TestContext testContextInstance;
        static PremiumAdjustmentStatusBE PremAdjStsBE;
        static PremiumAdjustmentStatusBS PremAdjStsBS;

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
            PremAdjStsBS = new PremiumAdjustmentStatusBS();
            AddCustomerData();
            AddPremiumAdjData();
            AddPremiumAdjStsData();
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
        /// <summary>
        /// a method for adding New Record in PremiumAdjSts when the classis initiated 
        /// </summary>
        private static void AddPremiumAdjStsData()
        {
            PremAdjStsBE = new PremiumAdjustmentStatusBE();
            PremAdjStsBE.PremumAdj_ID = premadjBE.PREMIUM_ADJ_ID;
            PremAdjStsBE.CustomerID = AcctBE.CUSTMR_ID;
            PremAdjStsBE.EffectiveDate = System.DateTime.Now;
            PremAdjStsBE.CreatedDate = System.DateTime.Now;
            PremAdjStsBE.CreatedUser_ID = 1;
            PremAdjStsBS.Save(PremAdjStsBE);
        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNull()
        {
            PremiumAdjustmentStatusBS target = new PremiumAdjustmentStatusBS(); // TODO: Initialize to an appropriate value
            PremiumAdjustmentStatusBE PremBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(PremBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPreAdjStatusRow
        ///</summary>
        [TestMethod()]
        public void getPreAdjStatusRowTest()
        {
            PremiumAdjustmentStatusBS target = new PremiumAdjustmentStatusBS(); // TODO: Initialize to an appropriate value
            int PreAdjStaID = 0; // TODO: Initialize to an appropriate value
            PremiumAdjustmentStatusBE expected = null; // TODO: Initialize to an appropriate value
            PremiumAdjustmentStatusBE actual;
            actual = target.getPreAdjStatusRow(PreAdjStaID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPreAdjStatusList
        ///</summary>
        [TestMethod()]
        public void getPreAdjStatusListTest()
        {
            PremiumAdjustmentStatusBS target = new PremiumAdjustmentStatusBS(); // TODO: Initialize to an appropriate value
            int PremAdjID = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentStatusBE> actual;
            actual = target.getPreAdjStatusList(PremAdjID);
            Assert.AreEqual(expected, actual.Count);
        }

       
        /// <summary>
        /// function to add new record in to Premium Adjustment Status Table
        /// </summary>
        [TestMethod()]
        public void AddPremAdjSts()
        {
            bool expected = true;
            bool actual;
            PremAdjStsBE = new PremiumAdjustmentStatusBE();
            PremAdjStsBE.CreatedDate = System.DateTime.Now;
            PremAdjStsBE.CreatedUser_ID = 1;
            PremAdjStsBE.PremumAdj_ID = premadjBE.PREMIUM_ADJ_ID;
            PremAdjStsBE.CustomerID = AcctBE.CUSTMR_ID;
            PremAdjStsBE.EffectiveDate =System.DateTime.Now;
            actual = PremAdjStsBS.Update(PremAdjStsBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// function to update record in Premium Adjustment Status Table
        /// </summary>
        [TestMethod()]
        public void UpdatePremAdjSts()
        {
            bool expected = true;
            bool actual;
            PremAdjStsBE.UpdtDate = System.DateTime.Now;
            PremAdjStsBE.UpdtUserID= 1;
            actual = PremAdjStsBS.Update(PremAdjStsBE);
            Assert.AreEqual(expected, actual);
        }
    }
}
