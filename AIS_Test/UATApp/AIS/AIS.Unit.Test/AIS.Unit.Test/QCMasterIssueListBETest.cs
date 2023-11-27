using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for QCMasterIssueListBETest and is intended
    ///to contain all QCMasterIssueListBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class QCMasterIssueListBETest
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
        ///A test for UpdatedUserID
        ///</summary>
        [TestMethod()]
        public void UpdatedUserIDTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UpdatedUserID = expected;
            actual = target.UpdatedUserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UpdatedDate
        ///</summary>
        [TestMethod()]
        public void UpdatedDateTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Str_Nbr
        ///</summary>
        [TestMethod()]
        public void Str_NbrTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.Str_Nbr = expected;
            actual = target.Str_Nbr;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QualityCntrlMstrIsslstID
        ///</summary>
        [TestMethod()]
        public void QualityCntrlMstrIsslstIDTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.QualityCntrlMstrIsslstID = expected;
            actual = target.QualityCntrlMstrIsslstID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IssueText
        ///</summary>
        [TestMethod()]
        public void IssueTextTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.IssueText = expected;
            actual = target.IssueText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IssueCategory
        ///</summary>
        [TestMethod()]
        public void IssueCategoryTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.IssueCategory = expected;
            actual = target.IssueCategory;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IssCatgID
        ///</summary>
        [TestMethod()]
        public void IssCatgIDTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.IssCatgID = expected;
            actual = target.IssCatgID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FinancialIndicator
        ///</summary>
        [TestMethod()]
        public void FinancialIndicatorTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.FinancialIndicator = expected;
            actual = target.FinancialIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUserID
        ///</summary>
        [TestMethod()]
        public void CreatedUserIDTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
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
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTIVE
        ///</summary>
        [TestMethod()]
        public void ACTIVETest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTIVE = expected;
            actual = target.ACTIVE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QCMasterIssueListBE Constructor
        ///</summary>
        [TestMethod()]
        public void QCMasterIssueListBEConstructorTest()
        {
            QCMasterIssueListBE target = new QCMasterIssueListBE();
            
        }
    }
}
