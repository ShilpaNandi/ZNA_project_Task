using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerDocumentIssuesBETest and is intended
    ///to contain all CustomerDocumentIssuesBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerDocumentIssuesBETest
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
        ///A test for updateduserid
        ///</summary>
        [TestMethod()]
        public void updateduseridTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.updateduserid = expected;
            actual = target.updateduserid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for updateddate
        ///</summary>
        [TestMethod()]
        public void updateddateTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.updateddate = expected;
            actual = target.updateddate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for tracking_issue_id
        ///</summary>
        [TestMethod()]
        public void tracking_issue_idTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.tracking_issue_id = expected;
            actual = target.tracking_issue_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for custmr_doc_issueid
        ///</summary>
        [TestMethod()]
        public void custmr_doc_issueidTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.custmr_doc_issueid = expected;
            actual = target.custmr_doc_issueid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for custmr_doc_id
        ///</summary>
        [TestMethod()]
        public void custmr_doc_idTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.custmr_doc_id = expected;
            actual = target.custmr_doc_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for cretateddate
        ///</summary>
        [TestMethod()]
        public void cretateddateTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.cretateddate = expected;
            actual = target.cretateddate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for createduserid
        ///</summary>
        [TestMethod()]
        public void createduseridTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.createduserid = expected;
            actual = target.createduserid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CustomerDocumentIssuesBE Constructor
        ///</summary>
        [TestMethod()]
        public void CustomerDocumentIssuesBEConstructorTest()
        {
            CustomerDocumentIssuesBE target = new CustomerDocumentIssuesBE();
            
        }
    }
}
