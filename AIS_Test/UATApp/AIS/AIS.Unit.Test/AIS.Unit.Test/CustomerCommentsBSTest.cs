using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerCommentsBSTest and is intended
    ///to contain all CustomerCommentsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerCommentsBSTest
    {
        

        private TestContext testContextInstance;
        
        static AccountBE AcctBE;
        static CustomerCommentsBE commntbe;
        static CustomerCommentsBS target;
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
            target = new CustomerCommentsBS();
            AddCustomerData();
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
        
        private static void AddCommonData()
        {
            commntbe = new CustomerCommentsBE();
            target = new CustomerCommentsBS();
            commntbe.CustomerID = AcctBE.CUSTMR_ID;
            commntbe.CommentCategoryID = 295;
            commntbe.CommentDate = System.DateTime.Now;
            commntbe.CommentBY = 1;
            target.Update(commntbe);
        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            CustomerCommentsBS target = new CustomerCommentsBS();
            CustomerCommentsBE commntbe = null;
            bool expected = false;
            bool actual;
            actual = target.Update(commntbe);
          Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            CustomerCommentsBS ltarget = new CustomerCommentsBS();
            CustomerCommentsBE commntbe1 = new CustomerCommentsBE();
            commntbe1.CustomerID = AcctBE.CUSTMR_ID;
            commntbe1.CommentCategoryID = 295;
            commntbe1.CommentDate = System.DateTime.Now;
            commntbe1.CommentBY = 1;
            bool expected = true; 
            bool actual;
            actual = ltarget.Update(commntbe1);
            Assert.AreEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNULL()
        {
            CustomerCommentsBS target = new CustomerCommentsBS();
            CustomerCommentsBE commntbe = null;
            bool expected = false;
            bool actual;
            actual = target.Update(commntbe);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {

            commntbe.CustomerID = AcctBE.CUSTMR_ID;
            commntbe.CommentCategoryID = 295;
            commntbe.CommentDate = System.DateTime.Now;
            commntbe.CommentBY = 1;
            commntbe.CommntUpdatedDate = System.DateTime.Now;
            commntbe.CommntUpdatedUserID = 1;
            bool expected = true; 
            bool actual;
            actual = target.Update(commntbe);
            Assert.AreEqual(expected, actual);
            

        }

        /// <summary>
        ///A test for getRelatedComments without data
        ///</summary>
        [TestMethod()]
        public void getRelatedCommentsTestWithNULL()
        {
            int accountID = 0; 
            int expected = 0; 
            IList<CustomerCommentsBE> actual;
            actual = target.getRelatedComments(accountID);
            
            Assert.AreEqual(expected, actual.Count);
            
        }
        /// <summary>
        ///A test for getRelatedComments with data
        ///</summary>
        [TestMethod()]
        public void getRelatedCommentsTestWithData()
        {
            int accountID = 1;
            int expected = 1; 
            IList<CustomerCommentsBE> actual;
            actual = target.getRelatedComments(accountID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getAssignComments without data
        ///</summary>
        [TestMethod()]
        public void getAssignCommentsTestWithNULL()
        {
            int commentID = 0;
            CustomerCommentsBE expected = null;
            CustomerCommentsBE actual;
            actual = target.getAssignComments(commentID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for getAssignComments with data
        ///</summary>
        [TestMethod()]
        public void getAssignCommentsTestWithData()
        {
           
            CustomerCommentsBE expected = null; 
            CustomerCommentsBE actual;
            actual = target.getAssignComments(commntbe.CommentID);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for CustomerCommentsBS Constructor
        ///</summary>
        [TestMethod()]
        public void CustomerCommentsBSConstructorTest()
        {
            CustomerCommentsBS target = new CustomerCommentsBS();
        }
    }
}
