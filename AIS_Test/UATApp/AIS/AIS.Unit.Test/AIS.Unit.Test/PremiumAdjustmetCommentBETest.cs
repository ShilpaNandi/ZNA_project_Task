using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmetCommentBETest and is intended
    ///to contain all PremiumAdjustmetCommentBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmetCommentBETest
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
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremumAdj_Commnt_ID
        ///</summary>
        [TestMethod()]
        public void PremumAdj_Commnt_IDTest()
        {
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PremumAdj_Commnt_ID = expected;
            actual = target.PremumAdj_Commnt_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUser_ID
        ///</summary>
        [TestMethod()]
        public void CreatedUser_IDTest()
        {
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Commnt_Catg_ID
        ///</summary>
        [TestMethod()]
        public void Commnt_Catg_IDTest()
        {
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.Commnt_Catg_ID = expected;
            actual = target.Commnt_Catg_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentText
        ///</summary>
        [TestMethod()]
        public void CommentTextTest()
        {
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CommentText = expected;
            actual = target.CommentText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmetCommentBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmetCommentBEConstructorTest()
        {
            PremiumAdjustmetCommentBE target = new PremiumAdjustmetCommentBE();
            
        }
    }
}
