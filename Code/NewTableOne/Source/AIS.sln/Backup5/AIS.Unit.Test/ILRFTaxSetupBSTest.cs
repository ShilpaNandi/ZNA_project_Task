using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System.Data;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ILRFTaxSetupBSTest and is intended
    ///to contain all ILRFTaxSetupBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ILRFTaxSetupBSTest
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
            ILRFTaxSetupBS target = new ILRFTaxSetupBS(); // TODO: Initialize to an appropriate value
            ILRFTaxSetupBE iLRFTaxBE = new ILRFTaxSetupBE(); // TODO: Initialize to an appropriate value
            iLRFTaxBE.ACTV_IND = false;
            iLRFTaxBE.CREATE_DATE = Convert.ToDateTime("12/18/2009 12:04:27 AM");
            iLRFTaxBE.CREATE_USER_ID = 0;
            iLRFTaxBE.CUSTOMER_ID = 106;
            iLRFTaxBE.INCURRED_LOSS_REIM_FUND_TAX_ID = 8;
            iLRFTaxBE.INCURRED_LOSS_REIM_FUND_TAX_TYPE = null;
            iLRFTaxBE.PREM_ADJ_PGM_ID = 707;
            iLRFTaxBE.PREM_ADJ_PGM_SETUP_ID = 11088;
            iLRFTaxBE.TAX_AMT = 3500;
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(iLRFTaxBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for isTaxTypeAlreadyExist
        ///</summary>
        [TestMethod()]
        public void isTaxTypeAlreadyExistTest()
        {
            ILRFTaxSetupBS target = new ILRFTaxSetupBS(); // TODO: Initialize to an appropriate value
            int intILRFTaxSetupID = 0; // TODO: Initialize to an appropriate value
            int intTaxTypeID = 0; // TODO: Initialize to an appropriate value
            int intPrem_adj_pgm_ID = 707; // TODO: Initialize to an appropriate value
            int intLn_Of_Bsn_Id = 0;
            int expected = 1; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.isTaxTypeAlreadyExist(intILRFTaxSetupID, intTaxTypeID, intPrem_adj_pgm_ID, intLn_Of_Bsn_Id);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTaxDescriptionListEditData
        ///</summary>
        [TestMethod()]
        public void getTaxDescriptionListEditDataTest()
        {
            ILRFTaxSetupBS target = new ILRFTaxSetupBS(); // TODO: Initialize to an appropriate value
            int intTaxTypeID = 541; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<ILRFTaxSetupBE> actual;
            actual = target.getTaxDescriptionListEditData(intTaxTypeID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getTaxDescriptionList
        ///</summary>
        [TestMethod()]
        public void getTaxDescriptionListTest()
        {
            ILRFTaxSetupBS target = new ILRFTaxSetupBS(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<ILRFTaxSetupBE> actual;
            actual = target.getTaxDescriptionList();
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getILRFTaxSetupRow
        ///</summary>
        [TestMethod()]
        public void getILRFTaxSetupRowTest()
        {
            ILRFTaxSetupBS target = new ILRFTaxSetupBS(); // TODO: Initialize to an appropriate value
            int iLRFTaxSetupID = 1; // TODO: Initialize to an appropriate value
            ILRFTaxSetupBE expected = null; // TODO: Initialize to an appropriate value
            ILRFTaxSetupBE actual;
            actual = target.getILRFTaxSetupRow(iLRFTaxSetupID);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getILRFTaxSetupList
        ///</summary>
        [TestMethod()]
        public void getILRFTaxSetupListTest()
        {
            ILRFTaxSetupBS target = new ILRFTaxSetupBS(); // TODO: Initialize to an appropriate value
            int intPREM_ADJ_PGM_ID = 707; // TODO: Initialize to an appropriate value
            int intCUSTMR_ID = 106; // TODO: Initialize to an appropriate value
            int expected = 1; // TODO: Initialize to an appropriate value
            IList<ILRFTaxSetupBE> actual;
            actual = target.getILRFTaxSetupList(intPREM_ADJ_PGM_ID, intCUSTMR_ID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for ILRFTaxSetupBS Constructor
        ///</summary>
        [TestMethod()]
        public void ILRFTaxSetupBSConstructorTest()
        {
            ILRFTaxSetupBS target = new ILRFTaxSetupBS();
        }
    }
}
