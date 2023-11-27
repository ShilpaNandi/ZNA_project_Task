using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjustmentManagementBSTest and is intended
    ///to contain all AdjustmentManagementBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjustmentManagementBSTest
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
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            AdjustmentManagementBS target = new AdjustmentManagementBS(); 
            AdjustmentManagementBE prmadjBE = null;
            bool expected = false; 
            bool actual;
            actual = target.Update(prmadjBE);
            Assert.AreEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for getPremMgmtRow
        ///</summary>
        [TestMethod()]
        public void getPremMgmtRowTest()
        {
            AdjustmentManagementBS target = new AdjustmentManagementBS(); 
            int PremAdjMgmtID = 0; 
            AdjustmentManagementBE expected = null; 
            AdjustmentManagementBE actual;
            actual = target.getPremMgmtRow(PremAdjMgmtID);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for getAdjManagement
        ///</summary>
        [TestMethod()]
        public void getAdjManagementTest()
        {
            AdjustmentManagementBS target = new AdjustmentManagementBS(); 
            int AcctNameID = 17; 
            int AdjStatusID = 0; 
            string InvNmbr = ""; 
            int PersnID = 0;
            DateTime InvFrmDt = Convert.ToDateTime("1/1/0001 12:00:00 AM");
            DateTime InvToDt = Convert.ToDateTime("1/1/0001 12:00:00 AM");
            DateTime ValutnDt = Convert.ToDateTime("1/1/0001 12:00:00 AM"); 
            IList<AdjustmentManagementBE> expected = null; 
            IList<AdjustmentManagementBE> actual;
            actual = target.getAdjManagement(AcctNameID, AdjStatusID, InvNmbr, PersnID, InvFrmDt, InvToDt, ValutnDt);
            Assert.AreNotEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for AdjustmentManagementBS Constructor
        ///</summary>
        [TestMethod()]
        public void AdjustmentManagementBSConstructorTest()
        {
            AdjustmentManagementBS target = new AdjustmentManagementBS();
            //
        }
    }
}
