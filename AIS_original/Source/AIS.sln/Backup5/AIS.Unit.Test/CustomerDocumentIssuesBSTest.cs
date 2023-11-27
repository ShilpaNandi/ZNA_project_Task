using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerDocumentIssuesBSTest and is intended
    ///to contain all CustomerDocumentIssuesBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerDocumentIssuesBSTest
    {


        private TestContext testContextInstance;
        static CustomerDocumentIssuesBE cdiBE;
        static CustomerDocumentIssuesBS target;
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
            target = new CustomerDocumentIssuesBS();
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

        private static void AddCommonData()
        {
            cdiBE = new CustomerDocumentIssuesBE();
            cdiBE.custmr_doc_id = 122;
            cdiBE.tracking_issue_id = 240;
            cdiBE.cretateddate = System.DateTime.Now;
            cdiBE.createduserid = 1;
            target.save(cdiBE);
        }
        /// <summary>
        ///A test for save
        ///</summary>
        [TestMethod()]
        public void saveTestWithData()
        {
            bool expected = true; 
            bool actual;
            CustomerDocumentIssuesBE cdocIssueBE = new CustomerDocumentIssuesBE();
            cdocIssueBE.custmr_doc_id = 122;
            cdocIssueBE.tracking_issue_id = 240;
            cdocIssueBE.cretateddate = System.DateTime.Now;
            cdocIssueBE.createduserid = 1;
            actual = target.save(cdocIssueBE);
            Assert.AreEqual(expected, actual);
            
        }
        
            
         
        /// <summary>
        ///A test for save
        ///</summary>
        [TestMethod()]
        //[ExpectedException(typeof(System.NullReferenceException))]
        public void saveTestWithNULL()
        {
            CustomerDocumentIssuesBS target = new CustomerDocumentIssuesBS();
            CustomerDocumentIssuesBE cdocIsuueBE = null;
            bool expected = false;
            bool actual = target.save(cdocIsuueBE);
            Assert.AreEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for getIssuedetails
        ///</summary>
        [TestMethod()]
        public void getIssuedetailsTestWithData()
        {
            IList<CustomerDocumentIssuesBE> expected = null; 
            IList<CustomerDocumentIssuesBE> actual;
            actual = target.getIssuedetails(cdiBE.custmr_doc_id);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getIssuedetails
        ///</summary>
        [TestMethod()]
        public void getIssuedetailsTestWithNULL()
        {
           
            int customerid = 0; 
            IList<CustomerDocumentIssuesBE> expected = null; 
            IList<CustomerDocumentIssuesBE> actual;
            actual = target.getIssuedetails(customerid);
            if (actual.Count == 0) actual = null;
            Assert.AreEqual(expected, actual);
          
        }
        /// <summary>
        ///A test for deleteCustmrissues
        ///</summary>
        [TestMethod()]
        public void deleteCustmrissuesTestWithData()
        {
           
            bool expected = false; 
            bool actual;
            actual = target.deleteCustmrissues(cdiBE.custmr_doc_id);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for deleteCustmrissues
        ///</summary>
        [TestMethod()]
        public void deleteCustmrissuesTestWithNULL()
        {
           
            int custmrdocID = 0; 
            bool expected = false; 
            bool actual;
            actual = target.deleteCustmrissues(custmrdocID);
            Assert.AreEqual(expected, actual);
           
        }

       
    }
}
