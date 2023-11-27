using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremAdjCommentBSTest and is intended
    ///to contain all PremAdjCommentBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremAdjCommentBSTest
    {


        private TestContext testContextInstance;
        static PremAdjCommentBS PremCmtBS;
        static PremiumAdjustmetCommentBE PremCmtBE;

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
            PremCmtBS = new PremAdjCommentBS();
            AddComment();
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
        /// A test for Adding PremiumAdjustmentComment
        /// </summary>
        public static void AddComment()
        {
            PremCmtBE = new PremiumAdjustmetCommentBE();
            PremCmtBE.CommentText = "aaa";
            PremCmtBE.Commnt_Catg_ID = 227;
            PremCmtBE.CreatedUser_ID = 1;
            PremCmtBE.CreatedDate =System.DateTime.Now;
            PremCmtBS.Update(PremCmtBE);
        }
        /// <summary>
        /// A test for updating PremiumAdjustmentComment
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void updateComment()
        {
            bool expected = true;
            bool actual;
            int premAdjCmtID=1;
            PremCmtBE = PremCmtBS.getPreAdjCmtRow(premAdjCmtID);
            PremCmtBE.CommentText = "aaa";
            PremCmtBE.Commnt_Catg_ID = 227;
            PremCmtBE.UpdatedUser_ID = 1;
            PremCmtBE.UpdatedDate = System.DateTime.Now;
            actual = PremCmtBS.Update(PremCmtBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTest()
        {
            PremAdjCommentBS target = new PremAdjCommentBS(); // TODO: Initialize to an appropriate value
            PremiumAdjustmetCommentBE PerAdjCmtBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(PerAdjCmtBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPreAdjCmtRow
        ///</summary>
        [TestMethod()]
        public void getPreAdjCmtRowTest()
        {
            PremAdjCommentBS target = new PremAdjCommentBS(); // TODO: Initialize to an appropriate value
            int PreAdjcmtID = 0; // TODO: Initialize to an appropriate value
            PremiumAdjustmetCommentBE expected = null; // TODO: Initialize to an appropriate value
            PremiumAdjustmetCommentBE actual;
            actual = target.getPreAdjCmtRow(PreAdjcmtID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }
    }
}
