using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjLRFPostingTaxBSTest and is intended
    ///to contain all PremiumAdjLRFPostingTaxBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjLRFPostingTaxBSTest
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
            PremiumAdjLRFPostingTaxBS target = new PremiumAdjLRFPostingTaxBS(); // TODO: Initialize to an appropriate value
            PremiumAdjLRFPostingTaxBE paLRFBE = new PremiumAdjLRFPostingTaxBE(); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            paLRFBE = target.LoadLRFTaxData(2);
            paLRFBE.ADJ_PRIOR_YY_AMT = 10;
            paLRFBE.POST_AMT = 3990;
            actual = target.Update(paLRFBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LoadLRFTaxData
        ///</summary>
        [TestMethod()]
        public void LoadLRFTaxDataTest()
        {
            PremiumAdjLRFPostingTaxBS target = new PremiumAdjLRFPostingTaxBS(); // TODO: Initialize to an appropriate value
            int LRFTaxSetupId = 2; // TODO: Initialize to an appropriate value
            PremiumAdjLRFPostingTaxBE expected = null; // TODO: Initialize to an appropriate value
            PremiumAdjLRFPostingTaxBE actual;
            actual = target.LoadLRFTaxData(LRFTaxSetupId);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for LoadData
        ///</summary>
        [TestMethod()]
        public void LoadDataTest()
        {
            PremiumAdjLRFPostingTaxBS target = new PremiumAdjLRFPostingTaxBS(); // TODO: Initialize to an appropriate value
            int prmAdjLRFPostingTaxId = 2; // TODO: Initialize to an appropriate value
            PremiumAdjLRFPostingTaxBE expected = null; // TODO: Initialize to an appropriate value
            PremiumAdjLRFPostingTaxBE actual;
            actual = target.LoadData(prmAdjLRFPostingTaxId);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetPrmAdjLRFPostingTax
        ///</summary>
        [TestMethod()]
        public void GetPrmAdjLRFPostingTaxTest()
        {
            PremiumAdjLRFPostingTaxBS target = new PremiumAdjLRFPostingTaxBS(); // TODO: Initialize to an appropriate value
            int prmAdjmtId = 600002522; // TODO: Initialize to an appropriate value
            int prem_adj_perd_id = 20302; // TODO: Initialize to an appropriate value
            int prem_adj_pgm_id = 707; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<PremiumAdjLRFPostingTaxBE> actual;
            actual = target.GetPrmAdjLRFPostingTax(prmAdjmtId, prem_adj_perd_id, prem_adj_pgm_id);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for PremiumAdjLRFPostingTaxBS Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjLRFPostingTaxBSConstructorTest()
        {
            PremiumAdjLRFPostingTaxBS target = new PremiumAdjLRFPostingTaxBS();
        }
    }
}
