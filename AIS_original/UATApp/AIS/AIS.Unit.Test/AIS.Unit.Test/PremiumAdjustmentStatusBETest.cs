using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentStatusBETest and is intended
    ///to contain all PremiumAdjustmentStatusBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentStatusBETest
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
        ///A test for UpdtUserID
        ///</summary>
        [TestMethod()]
        public void UpdtUserIDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UpdtUserID = expected;
            actual = target.UpdtUserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UpdtDate
        ///</summary>
        [TestMethod()]
        public void UpdtDateTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdtDate = expected;
            actual = target.UpdtDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ReviewPerson_ID
        ///</summary>
        [TestMethod()]
        public void ReviewPerson_IDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ReviewPerson_ID = expected;
            actual = target.ReviewPerson_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Review_Cmplt_Date
        ///</summary>
        [TestMethod()]
        public void Review_Cmplt_DateTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.Review_Cmplt_Date = expected;
            actual = target.Review_Cmplt_Date;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremumAdj_sts_ID
        ///</summary>
        [TestMethod()]
        public void PremumAdj_sts_IDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PremumAdj_sts_ID = expected;
            actual = target.PremumAdj_sts_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremumAdj_ID
        ///</summary>
        [TestMethod()]
        public void PremumAdj_IDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PremumAdj_ID = expected;
            actual = target.PremumAdj_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjStatus
        ///</summary>
        [TestMethod()]
        public void PremiumAdjStatusTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PremiumAdjStatus = expected;
            actual = target.PremiumAdjStatus;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PersonName
        ///</summary>
        [TestMethod()]
        public void PersonNameTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PersonName = expected;
            actual = target.PersonName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXPIRYDATE
        ///</summary>
        [TestMethod()]
        public void EXPIRYDATETest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.EXPIRYDATE = expected;
            actual = target.EXPIRYDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EffectiveDate
        ///</summary>
        [TestMethod()]
        public void EffectiveDateTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.EffectiveDate = expected;
            actual = target.EffectiveDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CustomerID
        ///</summary>
        [TestMethod()]
        public void CustomerIDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CustomerID = expected;
            actual = target.CustomerID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUser_ID
        ///</summary>
        [TestMethod()]
        public void CreatedUser_IDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CreatedUser_ID = expected;
            actual = target.CreatedUser_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedDate
        ///</summary>
        [TestMethod()]
        public void CreatedDateTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentText
        ///</summary>
        [TestMethod()]
        public void CommentTextTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CommentText = expected;
            actual = target.CommentText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COMMENTID
        ///</summary>
        [TestMethod()]
        public void COMMENTIDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.COMMENTID = expected;
            actual = target.COMMENTID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for APPROVEINDICATOR
        ///</summary>
        [TestMethod()]
        public void APPROVEINDICATORTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.APPROVEINDICATOR = expected;
            actual = target.APPROVEINDICATOR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_STS_TYP_ID
        ///</summary>
        [TestMethod()]
        public void ADJ_STS_TYP_IDTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJ_STS_TYP_ID = expected;
            actual = target.ADJ_STS_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmentStatusBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmentStatusBEConstructorTest()
        {
            PremiumAdjustmentStatusBE target = new PremiumAdjustmentStatusBE();
            
        }
    }
}
