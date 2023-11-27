using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerCommentsBETest and is intended
    ///to contain all CustomerCommentsBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerCommentsBETest
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
        ///A test for CustomerID
        ///</summary>
        [TestMethod()]
        public void CustomerIDTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CustomerID = expected;
            actual = target.CustomerID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommntUpdatedUserID
        ///</summary>
        [TestMethod()]
        public void CommntUpdatedUserIDTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CommntUpdatedUserID = expected;
            actual = target.CommntUpdatedUserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommntUpdatedDate
        ///</summary>
        [TestMethod()]
        public void CommntUpdatedDateTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.CommntUpdatedDate = expected;
            actual = target.CommntUpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentUserName
        ///</summary>
        [TestMethod()]
        public void CommentUserNameTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CommentUserName = expected;
            actual = target.CommentUserName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentText
        ///</summary>
        [TestMethod()]
        public void CommentTextTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CommentText = expected;
            actual = target.CommentText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentID
        ///</summary>
        [TestMethod()]
        public void CommentIDTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CommentID = expected;
            actual = target.CommentID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentDate
        ///</summary>
        [TestMethod()]
        public void CommentDateTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CommentDate = expected;
            actual = target.CommentDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentCategoryName
        ///</summary>
        [TestMethod()]
        public void CommentCategoryNameTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CommentCategoryName = expected;
            actual = target.CommentCategoryName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentCategoryID
        ///</summary>
        [TestMethod()]
        public void CommentCategoryIDTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CommentCategoryID = expected;
            actual = target.CommentCategoryID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentBY
        ///</summary>
        [TestMethod()]
        public void CommentBYTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CommentBY = expected;
            actual = target.CommentBY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CustomerCommentsBE Constructor
        ///</summary>
        [TestMethod()]
        public void CustomerCommentsBEConstructorTest()
        {
            CustomerCommentsBE target = new CustomerCommentsBE();
            
        }
    }
}
