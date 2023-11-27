using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using ZurichNA.AIS.ExceptionHandling;
namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AriesClearingBSTest and is intended
    ///to contain all AriesClearingBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AriesClearingBSTest
    {
        static AccountBE accountBE;
        static AccountBS accountBS;
        static PremAdjustmentBS premadjBS;
        static PremiumAdjustmentBE premadjBE;


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
            premadjBS = new PremAdjustmentBS();
            AddCustomerData();
            AddPreiumadjustmentData();
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

        public static void AddPreiumadjustmentData()
        {
            premadjBE = new PremiumAdjustmentBE();
            premadjBE.PREMIUM_ADJ_ID = accountBE.CUSTMR_ID;
            premadjBE.CRTE_USER_ID = 1;
            premadjBE.CRTE_DT = System.DateTime.Now;
            premadjBS.Save(premadjBE);
        
        }
        public static void AddCustomerData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "PraveenAC" + System.DateTime.Now.ToString();
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }
        /// <summary>
        ///A test for save
        ///</summary>
        [TestMethod()]
        [ExpectedException (typeof(RetroDatabaseException))]
        public void saveTest()
        {
            AriesClearingBS target = new AriesClearingBS(); // TODO: Initialize to an appropriate value
            AriesClearingBE ariesclrgBE = new AriesClearingBE(); // TODO: Initialize to an appropriate value
            ariesclrgBE.PREMIUM_ADJUST_CLEARING_ID = 2;
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.save(ariesclrgBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetAriesDetails
        ///</summary>
        [TestMethod()]
        public void GetAriesDetailsTest()
        {
            AriesClearingBS target = new AriesClearingBS(); // TODO: Initialize to an appropriate value
            int Premadjustid = 0; // TODO: Initialize to an appropriate value
            int Customerid = 0; // TODO: Initialize to an appropriate value
            IList<AriesClearingBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AriesClearingBE> actual;
            actual = target.GetAriesDetails(Premadjustid, Customerid);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetAriesDetails
        ///</summary>
        [TestMethod()]
        public void GetAriesDetailsTest1()
        {
            AriesClearingBS target = new AriesClearingBS(); // TODO: Initialize to an appropriate value
            AriesClearingBE expected = null; // TODO: Initialize to an appropriate value
            AriesClearingBE actual;
            actual = target.GetAriesDetails(0);
            Assert.AreNotEqual(expected, actual);

        }

        
    }
}
