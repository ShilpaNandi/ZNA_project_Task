using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for DeductibleTaxesBSTest and is intended
    ///to contain all DeductibleTaxesBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DeductibleTaxesBSTest
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
        ///A test for SelectDataOnSetupId
        ///</summary>
        [TestMethod()]
        public void SelectDataOnSetupIdTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int dTaxesSetupId = 2; // TODO: Initialize to an appropriate value
            DeductibleTaxesBE expected = null;
            DeductibleTaxesBE actual = target.SelectDataOnSetupId(dTaxesSetupId);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for SelectData
        ///</summary>
        [TestMethod()]
        public void SelectDataTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS();
            int expected = 2;
            IList<DeductibleTaxesBE> actual;
            actual = target.SelectData();
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for SaveDeductibleTaxes
        ///</summary>
        [TestMethod()]
        public void SaveDeductibleTaxesTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            DeductibleTaxesBE dTaxesBE = new DeductibleTaxesBE();// TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            dTaxesBE.DTAXCOMDESCRIPTION = "LDF";
            dTaxesBE.DTAXDESCRIPTION = "CA Sales & Use Tax";
            dTaxesBE.LN_OF_BSN_TXT = "WC";
            dTaxesBE.LOOKUPID = 542;
            dTaxesBE.STATEDESCRIPTION = "California ";
            dTaxesBE.ACTV_IND = false;
            dTaxesBE.CRTE_DT = DateTime.Now;
            dTaxesBE.CRTE_USER_ID = 4;
            dTaxesBE.DED_TAX_COMPONENT_ID = 546;
            //dTaxesBE.DED_TAXES_SETUP_ID = 2;
            dTaxesBE.LN_OF_BSN_ID = 51;
            dTaxesBE.LN_OF_BSN_TXT = "WC";
            dTaxesBE.MAIN_NBR_TXT = "8000 ";
            dTaxesBE.POL_EFF_DT = Convert.ToDateTime("1/1/2004 12:00:00 AM");
            dTaxesBE.ST_ID = 6;
            dTaxesBE.SUB_NBR_TXT = "5502 ";
            dTaxesBE.TAX_END_DT = Convert.ToDateTime(" 12/13/2005 12:00:00 AM");
            dTaxesBE.TAX_RATE = decimal.Parse("0.0825");
            dTaxesBE.TAX_TYP_ID = 542;
            //dTaxesBE.UPDT_DT = Convert.ToDateTime("12/17/2009 6:52:02 AM");
            //dTaxesBE.UPDT_USER_ID = 0;
            actual = target.SaveDeductibleTaxes(dTaxesBE);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadData
        ///</summary>
        [TestMethod()]
        public void LoadDataTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int dTaxesSetupId = 2; // TODO: Initialize to an appropriate value
            DeductibleTaxesBE expected = null; // TODO: Initialize to an appropriate value
            DeductibleTaxesBE actual;
            actual = target.LoadData(dTaxesSetupId);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for isTaxTypeAlreadyExist
        ///</summary>
        [TestMethod()]
        public void isTaxTypeAlreadyExistTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int intDTaxSetupID = 2; // TODO: Initialize to an appropriate value
            int intLnOfBsnID = 51;
            int intStateID = 6; // TODO: Initialize to an appropriate value
            int intTaxTypeID = 542; // TODO: Initialize to an appropriate value
            int intDTaxCompID = 546; // TODO: Initialize to an appropriate value
            DateTime dtPolicyEffDate = Convert.ToDateTime("1/1/2004 12:00:00 AM"); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.isTaxTypeAlreadyExist(intDTaxSetupID,intLnOfBsnID, intStateID, intTaxTypeID, intDTaxCompID, dtPolicyEffDate);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetStatesForEdit
        ///</summary>
        [TestMethod()]
        public void GetStatesForEditTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int iLkupId = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetStatesForEdit(iLkupId);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetStates
        ///</summary>
        [TestMethod()]
        public void GetStatesTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetStates();
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetMainSub
        ///</summary>
        [TestMethod()]
        public void GetMainSubTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            string trns_nm_txt = "TPA Direct Paid Losses"; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetMainSub(trns_nm_txt);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetDescriptionForEdit
        ///</summary>
        [TestMethod()]
        public void GetDescriptionForEditTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int iDescriptionId = 542; // TODO: Initialize to an appropriate value
            string Attribute1 = "CA"; // TODO: Initialize to an appropriate value
            string lkupTypeName = "TAX TYPE"; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetDescriptionForEdit(iDescriptionId, Attribute1, lkupTypeName);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetDescription
        ///</summary>
        [TestMethod()]
        public void GetDescriptionTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            string Attribute1 = "CA"; // TODO: Initialize to an appropriate value
            string lkupTypeName = "TAX TYPE"; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetDescription(Attribute1, lkupTypeName);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetComponentDescriptionForEdit
        ///</summary>
        [TestMethod()]
        public void GetComponentDescriptionForEditTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int iComponentId = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetComponentDescriptionForEdit(iComponentId);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetComponentDescription
        ///</summary>
        [TestMethod()]
        public void GetComponentDescriptionTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetComponentDescription();
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for DeductibleTaxesBS Constructor
        ///</summary>
        [TestMethod()]
        public void DeductibleTaxesBSConstructorTest()
        {
            DeductibleTaxesBS target = new DeductibleTaxesBS();
        }
    }
}
