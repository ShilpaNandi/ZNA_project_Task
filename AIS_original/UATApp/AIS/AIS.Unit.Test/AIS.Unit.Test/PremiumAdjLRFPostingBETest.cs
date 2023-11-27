using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjLRFPostingBETest and is intended
    ///to contain all PremiumAdjLRFPostingBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjLRFPostingBETest
    {


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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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
        ///A test for UpdatedUser_ID
        ///</summary>
        [TestMethod()]
        public void UpdatedUser_IDTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UpdatedUser_ID = expected;
            actual = target.UpdatedUser_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UpdatedDate
        ///</summary>
        [TestMethod()]
        public void UpdatedDateTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RecvType
        ///</summary>
        [TestMethod()]
        public void RecvTypeTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.RecvType = expected;
            actual = target.RecvType;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ReceivableTypID
        ///</summary>
        [TestMethod()]
        public void ReceivableTypIDTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ReceivableTypID = expected;
            actual = target.ReceivableTypID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PriorYrAmt
        ///</summary>
        [TestMethod()]
        public void PriorYrAmtTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.PriorYrAmt = expected;
            actual = target.PriorYrAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremAdjPerdID
        ///</summary>
        [TestMethod()]
        public void PremAdjPerdIDTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PremAdjPerdID = expected;
            actual = target.PremAdjPerdID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremAdjID
        ///</summary>
        [TestMethod()]
        public void PremAdjIDTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PremAdjID = expected;
            actual = target.PremAdjID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Prem_Adj_Los_Reim_Fund_Post_ID
        ///</summary>
        [TestMethod()]
        public void Prem_Adj_Los_Reim_Fund_Post_IDTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Prem_Adj_Los_Reim_Fund_Post_ID = expected;
            actual = target.Prem_Adj_Los_Reim_Fund_Post_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PostedAmt
        ///</summary>
        [TestMethod()]
        public void PostedAmtTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.PostedAmt = expected;
            actual = target.PostedAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CustomerID
        ///</summary>
        [TestMethod()]
        public void CustomerIDTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CustomerID = expected;
            actual = target.CustomerID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CurrntAmount
        ///</summary>
        [TestMethod()]
        public void CurrntAmountTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.CurrntAmount = expected;
            actual = target.CurrntAmount;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUserID
        ///</summary>
        [TestMethod()]
        public void CreatedUserIDTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CreatedUserID = expected;
            actual = target.CreatedUserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedDate
        ///</summary>
        [TestMethod()]
        public void CreatedDateTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AggrgateAmt
        ///</summary>
        [TestMethod()]
        public void AggrgateAmtTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.AggrgateAmt = expected;
            actual = target.AggrgateAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustPriorYrAmt
        ///</summary>
        [TestMethod()]
        public void AdjustPriorYrAmtTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.AdjustPriorYrAmt = expected;
            actual = target.AdjustPriorYrAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjLRFPostingBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjLRFPostingBEConstructorTest()
        {
            PremiumAdjLRFPostingBE target = new PremiumAdjLRFPostingBE();
            
        }
    }
}
