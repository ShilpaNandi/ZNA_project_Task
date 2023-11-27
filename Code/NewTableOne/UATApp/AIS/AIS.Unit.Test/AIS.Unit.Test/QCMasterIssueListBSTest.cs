using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for QCMasterIssueListBSTest and is intended
    ///to contain all QCMasterIssueListBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QCMasterIssueListBSTest
    {


        private TestContext testContextInstance;
        static QCMasterIssueListBE qltyMasterIssueBE;
        static QCMasterIssueListBS target;
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
            //Quality issue Table
            target = new QCMasterIssueListBS();
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
            qltyMasterIssueBE = new QCMasterIssueListBE();
            qltyMasterIssueBE.IssueText = "IssueList"+System.DateTime.Now;
            qltyMasterIssueBE.CreatedDate = System.DateTime.Now;
            qltyMasterIssueBE.CreatedUserID = 1;
            target.Update(qltyMasterIssueBE);
        }
        /// <summary>
        /// a Test for adding a Record  With Real Data in to Quality Master Issue list Table
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            QCMasterIssueListBE qmilBE = new QCMasterIssueListBE();
            qmilBE.IssueText = "IssueList" + System.DateTime.Now;
            qmilBE.CreatedDate = System.DateTime.Now;
            qmilBE.CreatedUserID = 1;
            actual = (new QCMasterIssueListBS()).Update(qmilBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for adding New Record With NULL(with out Passing Any Data)
        /// the Add Method Should Rise an Exception
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            QCMasterIssueListBE QualityBE = null;
            bool expected = false;
            bool actual;
            actual = new QCMasterIssueListBS().Update(QualityBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update a Record With Real Data into Quality Master Issue list Table
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            QCMasterIssueListBE QCBE = (new QCMasterIssueListBS()).getQCMasterIssueRow(qltyMasterIssueBE.QualityCntrlMstrIsslstID);
            QCBE.UpdatedDate = System.DateTime.Now;
            QCBE.UpdatedUserID = 1;
            actual = (new QCMasterIssueListBS()).Update(QCBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update a record With NULL(i.e With out Passing any Values)
        ///it should Rise Exception
        ///</summary>
        [TestMethod()]
        public void UpdateTestWITHNULL()
        {
            QCMasterIssueListBE QCBE = null;
            bool expected = false;
            bool actual;
            actual = new QCMasterIssueListBS().Update(QCBE);
            Assert.AreEqual(expected, actual);


        }

        /// <summary>
        ///A test for IsExistsIssueName
        ///</summary>
        [TestMethod()]
        public void IsExistsIssueNameTest()
        {
            QCMasterIssueListBS target = new QCMasterIssueListBS(); 
            int LookupID = 0; 
            int intQCMasterIssueID = 0; 
            string strIssueName = string.Empty; 
            bool expected = false; 
            bool actual;
            actual = new QCMasterIssueListBS().IsExistsIssueName(LookupID, intQCMasterIssueID, strIssueName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getQCMasterIssueRow
        ///</summary>
        [TestMethod()]
        public void getQCMasterIssueRowTestWithData()
        {
            QCMasterIssueListBE expected = null; 
            QCMasterIssueListBE actual;
            actual = new QCMasterIssueListBS().getQCMasterIssueRow(qltyMasterIssueBE.QualityCntrlMstrIsslstID);
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for getQCMasterIssueRow
        ///</summary>
        [TestMethod()]
        public void getQCMasterIssueRowTestWithNULL()
        {
            int QltyIssueID = 0; 
            QCMasterIssueListBE expected = null;
            QCMasterIssueListBE actual;
            actual = new QCMasterIssueListBS().getQCMasterIssueRow(QltyIssueID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }

       
        /// <summary>
        ///A test for getIssuesList
        ///</summary>
        [TestMethod()]
        public void getIssuesListTest()
        {
            int IssCatID = 0;
            int expected = 0;
            IList<QCMasterIssueListBE> actual;
            actual = new QCMasterIssueListBS().getIssuesList(IssCatID);
            Assert.AreEqual(expected, actual.Count);
        }

        
    }
}
