using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Qtly_Cntrl_ChklistBSTest and is intended
    ///to contain all Qtly_Cntrl_ChklistBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Qtly_Cntrl_ChklistBSTest
    {

        
        private TestContext testContextInstance;
        static QCMasterIssueListBE QCMastrIssue;
        static Qtly_Cntrl_ChklistBS target;
        static Qtly_Cntrl_ChklistBE qltyBE;

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
        
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            target = new Qtly_Cntrl_ChklistBS();
            AddMasterIssueData();
            AddQualtiyCntlData();
        }
        
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
        /// a method for adding New Record in QLTY_CNTRL_MSTR_ISSU_LIST table when the classis initiated
        /// </summary>
        private static void AddMasterIssueData()
        {
            QCMastrIssue = new QCMasterIssueListBE();
            QCMastrIssue.IssueText = "MasterIssue";
            QCMastrIssue.CreatedDate = System.DateTime.Now;
            QCMastrIssue.CreatedUserID = 1;
            (new QCMasterIssueListBS()).Update(QCMastrIssue);
        }
        /// <summary>
        /// a method for adding New Record in QLTY_CNTRL_LIST table when the classis initiated
        /// </summary>
        private static void AddQualtiyCntlData()
        {
            qltyBE = new Qtly_Cntrl_ChklistBE();
            qltyBE.CHECKLISTITEM_ID = QCMastrIssue.QualityCntrlMstrIsslstID;
            qltyBE.CreatedDate = System.DateTime.Now;
            qltyBE.CreatedUser_ID = 1;
            (new Qtly_Cntrl_ChklistBS()).Update(qltyBE);
        }
        /// <summary>
        ///A test for AddTest With Data
        ///</summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual;
            qltyBE = new Qtly_Cntrl_ChklistBE();
            qltyBE.CHECKLISTITEM_ID = QCMastrIssue.QualityCntrlMstrIsslstID;
            qltyBE.CreatedDate = System.DateTime.Now;
            qltyBE.CreatedUser_ID = 1;
            actual = target.Update(qltyBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for AddTest With Null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            Qtly_Cntrl_ChklistBS target = new Qtly_Cntrl_ChklistBS();
            Qtly_Cntrl_ChklistBE qltyBE1 = null;
            bool expected = false;
            bool actual;
            actual = target.Update(qltyBE1);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateTest With Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            qltyBE.UpdatedDate = System.DateTime.Now;
            qltyBE.UpdatedUserID = 1;
            actual = target.Update(qltyBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateTest With Null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestNULL()
        {
            Qtly_Cntrl_ChklistBE qltyBE1 = null;
            bool expected = false; 
            bool actual;
            actual = target.Update(qltyBE1);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsExistsIssue
        ///</summary>
        [TestMethod()]
        public void IsExistsIssueTestWithNull()
        {
            Qtly_Cntrl_ChklistBS target = new Qtly_Cntrl_ChklistBS(); 
            int premAdjStsID = 0; 
            int ChkListItemID = 0; 
            int CustomerID = 0; 
            int QltyID = 0; 
            bool expected = false; 
            bool actual;
            actual = target.IsExistsIssue(premAdjStsID, ChkListItemID, CustomerID, QltyID);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for IsExistsIssueName
        ///</summary>
        [TestMethod()]
        public void IsExistsAcctQCIssueTest()
        {
            Qtly_Cntrl_ChklistBS target = new Qtly_Cntrl_ChklistBS();
            int premAdjPgmID = 0;
            int ChkListItemID = 0;
            int CustomerID = 0;
            int QltyID = 0;
            bool expected = false;
            bool actual;
            actual = target.IsExistsAcctQCIssue(premAdjPgmID, ChkListItemID, CustomerID, QltyID);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for IsExistsAriesIssue
        ///</summary>
        [TestMethod()]
        public void IsExistsAriesIssueTest()
        {
            Qtly_Cntrl_ChklistBS target = new Qtly_Cntrl_ChklistBS();
            int ariesClrngid = 0;
            int ChkListItemID = 0;
            int CustomerID = 0;
            int QltyID = 0;
            bool expected = false;
            bool actual;
            actual = target.IsExistsAriesIssue(ariesClrngid, ChkListItemID, CustomerID, QltyID);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for getQualityControlRow
        ///</summary>
        [TestMethod()]
        public void getQualityControlRowTestWithNull()
        {
            int QualityControlChklstID = 0; 
            Qtly_Cntrl_ChklistBE expected = null;
            Qtly_Cntrl_ChklistBE actual;
            actual = target.getQualityControlRow(QualityControlChklstID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }
       

        /// <summary>
        ///A test for getQualityControlRow with Data
        ///</summary>
        [TestMethod()]
        public void getQualityControlRowTestWithData()
        {
            int QualityControlChklstID = 0;
            Qtly_Cntrl_ChklistBE expected = null;
            Qtly_Cntrl_ChklistBE actual;
            actual = target.getQualityControlRow(qltyBE.QualityControlChklst_ID);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getQualityControlList
        ///</summary>
        [TestMethod()]
        public void getQualityControlListTestWithNull()
        {
            Qtly_Cntrl_ChklistBS target = new Qtly_Cntrl_ChklistBS(); 
            IList<Qtly_Cntrl_ChklistBE> expected = null; 
            IList<Qtly_Cntrl_ChklistBE> actual;
            actual = target.getQualityControlList();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getQtlychklistList
        ///</summary>
        [TestMethod()]
        public void getQtlychklistListTestWithNULL()
        {
            int QualityCntlTypID = 0; 
            int PremAdjStsID = 0;
            int CstomerID = 0;
            int expected = 0; 
            IList<Qtly_Cntrl_ChklistBE> actual;
            actual = target.getQtlychklistList(QualityCntlTypID, PremAdjStsID, CstomerID);
            
            Assert.AreEqual(expected, actual.Count);
        }
        /// <summary>
        ///A test for getQtlychklistList
        ///</summary>
        [TestMethod()]
        public void getAccQtlychklistListTestWithNULL()
        {
            int QualityCntlTypID = 0;
            int PremAdjPgmID = 0;
            int CstomerID = 0;
            int expected = 0;
            IList<Qtly_Cntrl_ChklistBE> actual;
            actual = target.getAccQtlychklistList(QualityCntlTypID, PremAdjPgmID, CstomerID);

            Assert.AreEqual(expected, actual.Count);
        }
        /// <summary>
        ///A test for getQtlychklistList
        ///</summary>
        [TestMethod()]
        public void getArieschecklistTestWithNULL()
        {
            int QualityCntlTypID = 0;
            int PremAdjstClrgId = 0;
            int CstomerID = 0;
            int expected = 0;
            IList<Qtly_Cntrl_ChklistBE> actual;
            actual = target.getArieschecklist(QualityCntlTypID, PremAdjstClrgId, CstomerID);

            Assert.AreEqual(expected, actual.Count);
        }

       

    }
}
