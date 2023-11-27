using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PostingTransactionTypeBETest and is intended
    ///to contain all PostingTransactionTypeBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PostingTransactionTypeBETest
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
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATE_USER_ID = expected;
            actual = target.UPDATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATE_DATETest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TRNS_TYP_ID
        ///</summary>
        [TestMethod()]
        public void TRNS_TYP_IDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.TRNS_TYP_ID = expected;
            actual = target.TRNS_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TRANSACTIONTYPE
        ///</summary>
        [TestMethod()]
        public void TRANSACTIONTYPETest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TRANSACTIONTYPE = expected;
            actual = target.TRANSACTIONTYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TRANSACTIONNAME
        ///</summary>
        [TestMethod()]
        public void TRANSACTIONNAMETest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TRANSACTIONNAME = expected;
            actual = target.TRANSACTIONNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TRANS_TXT
        ///</summary>
        [TestMethod()]
        public void TRANS_TXTTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TRANS_TXT = expected;
            actual = target.TRANS_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for THRD_PTY_ADMIN_MNL_IND_txt
        ///</summary>
        [TestMethod()]
        public void THRD_PTY_ADMIN_MNL_IND_txtTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.THRD_PTY_ADMIN_MNL_IND_txt = expected;
            actual = target.THRD_PTY_ADMIN_MNL_IND_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for THRD_PTY_ADMIN_MNL_IND
        ///</summary>
        [TestMethod()]
        public void THRD_PTY_ADMIN_MNL_INDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.THRD_PTY_ADMIN_MNL_IND = expected;
            actual = target.THRD_PTY_ADMIN_MNL_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUB_NBR
        ///</summary>
        [TestMethod()]
        public void SUB_NBRTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SUB_NBR = expected;
            actual = target.SUB_NBR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POST_TRANS_TYP_ID
        ///</summary>
        [TestMethod()]
        public void POST_TRANS_TYP_IDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.POST_TRANS_TYP_ID = expected;
            actual = target.POST_TRANS_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POL_REQR_IND_txt
        ///</summary>
        [TestMethod()]
        public void POL_REQR_IND_txtTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POL_REQR_IND_txt = expected;
            actual = target.POL_REQR_IND_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POL_REQR_IND
        ///</summary>
        [TestMethod()]
        public void POL_REQR_INDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.POL_REQR_IND = expected;
            actual = target.POL_REQR_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MISC_POSTS_IND_txt
        ///</summary>
        [TestMethod()]
        public void MISC_POSTS_IND_txtTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.MISC_POSTS_IND_txt = expected;
            actual = target.MISC_POSTS_IND_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MISC_POSTS_IND
        ///</summary>
        [TestMethod()]
        public void MISC_POSTS_INDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.MISC_POSTS_IND = expected;
            actual = target.MISC_POSTS_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MAIN_NBR
        ///</summary>
        [TestMethod()]
        public void MAIN_NBRTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.MAIN_NBR = expected;
            actual = target.MAIN_NBR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICBL_IND_txt
        ///</summary>
        [TestMethod()]
        public void INVOICBL_IND_txtTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.INVOICBL_IND_txt = expected;
            actual = target.INVOICBL_IND_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICBL_IND
        ///</summary>
        [TestMethod()]
        public void INVOICBL_INDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.INVOICBL_IND = expected;
            actual = target.INVOICBL_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Created_UserID
        ///</summary>
        [TestMethod()]
        public void Created_UserIDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Created_UserID = expected;
            actual = target.Created_UserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Created_Date
        ///</summary>
        [TestMethod()]
        public void Created_DateTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.Created_Date = expected;
            actual = target.Created_Date;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COMP_TXT
        ///</summary>
        [TestMethod()]
        public void COMP_TXTTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.COMP_TXT = expected;
            actual = target.COMP_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_SUMRY_NOT_POST_IND_txt
        ///</summary>
        [TestMethod()]
        public void ADJ_SUMRY_NOT_POST_IND_txtTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJ_SUMRY_NOT_POST_IND_txt = expected;
            actual = target.ADJ_SUMRY_NOT_POST_IND_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_SUMRY_NOT_POST_IND
        ///</summary>
        [TestMethod()]
        public void ADJ_SUMRY_NOT_POST_INDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ADJ_SUMRY_NOT_POST_IND = expected;
            actual = target.ADJ_SUMRY_NOT_POST_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PostingTransactionTypeBE Constructor
        ///</summary>
        [TestMethod()]
        public void PostingTransactionTypeBEConstructorTest()
        {
            PostingTransactionTypeBE target = new PostingTransactionTypeBE();
            
        }
    }
}
