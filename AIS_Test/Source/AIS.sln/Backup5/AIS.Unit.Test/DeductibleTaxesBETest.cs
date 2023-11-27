using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for DeductibleTaxesBETest and is intended
    ///to contain all DeductibleTaxesBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class DeductibleTaxesBETest
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
        ///A test for UPDT_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDT_USER_IDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            expected = 0;
            target.UPDT_USER_ID = expected;
            actual = target.UPDT_USER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UPDT_DT
        ///</summary>
        [TestMethod()]
        public void UPDT_DTTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            expected = Convert.ToDateTime("12/17/2009 6:52:02 AM");
            target.UPDT_DT = expected;
            actual = target.UPDT_DT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAX_TYP_ID
        ///</summary>
        [TestMethod()]
        public void TAX_TYP_IDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            int expected = 542; // TODO: Initialize to an appropriate value
            int actual;
            target.TAX_TYP_ID = expected;
            actual = target.TAX_TYP_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAX_RATE
        ///</summary>
        [TestMethod()]
        public void TAX_RATETest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            expected = decimal.Parse("0.0825");
            target.TAX_RATE = expected;
            actual = target.TAX_RATE;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAX_END_DT
        ///</summary>
        [TestMethod()]
        public void TAX_END_DTTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            expected = Convert.ToDateTime("12/13/2005 12:00:00 AM");
            target.TAX_END_DT = expected;
            actual = target.TAX_END_DT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SUB_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void SUB_NBR_TXTTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            string expected = "5502 "; // TODO: Initialize to an appropriate value
            string actual;
            target.SUB_NBR_TXT = expected;
            actual = target.SUB_NBR_TXT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for STATEDESCRIPTION
        ///</summary>
        [TestMethod()]
        public void STATEDESCRIPTIONTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            string expected = "California "; // TODO: Initialize to an appropriate value
            string actual;
            target.STATEDESCRIPTION = expected;
            actual = target.STATEDESCRIPTION;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ST_ID
        ///</summary>
        [TestMethod()]
        public void ST_IDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            int expected = 6; // TODO: Initialize to an appropriate value
            int actual;
            target.ST_ID = expected;
            actual = target.ST_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for POL_EFF_DT
        ///</summary>
        [TestMethod()]
        public void POL_EFF_DTTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.POL_EFF_DT = expected;
            actual = target.POL_EFF_DT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MAIN_NBR_TXT
        ///</summary>
        [TestMethod()]
        public void MAIN_NBR_TXTTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            string expected = "8000 "; // TODO: Initialize to an appropriate value
            string actual;
            target.MAIN_NBR_TXT = expected;
            actual = target.MAIN_NBR_TXT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LOOKUPID
        ///</summary>
        [TestMethod()]
        public void LOOKUPIDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            int expected = 542; // TODO: Initialize to an appropriate value
            int actual;
            target.LOOKUPID = expected;
            actual = target.LOOKUPID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LN_OF_BSN_TXT
        ///</summary>
        [TestMethod()]
        public void LN_OF_BSN_TXTTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            string expected = "WC"; // TODO: Initialize to an appropriate value
            string actual;
            target.LN_OF_BSN_TXT = expected;
            actual = target.LN_OF_BSN_TXT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LN_OF_BSN_ID
        ///</summary>
        [TestMethod()]
        public void LN_OF_BSN_IDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            int expected = 51; // TODO: Initialize to an appropriate value
            int actual;
            target.LN_OF_BSN_ID = expected;
            actual = target.LN_OF_BSN_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DTAXDESCRIPTION
        ///</summary>
        [TestMethod()]
        public void DTAXDESCRIPTIONTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            string expected = "CA Sales & Use Tax"; // TODO: Initialize to an appropriate value
            string actual;
            target.DTAXDESCRIPTION = expected;
            actual = target.DTAXDESCRIPTION;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DTAXCOMDESCRIPTION
        ///</summary>
        [TestMethod()]
        public void DTAXCOMDESCRIPTIONTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            string expected = "LDF"; // TODO: Initialize to an appropriate value
            string actual;
            target.DTAXCOMDESCRIPTION = expected;
            actual = target.DTAXCOMDESCRIPTION;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DED_TAXES_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void DED_TAXES_SETUP_IDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            int expected = 2; // TODO: Initialize to an appropriate value
            int actual;
            target.DED_TAXES_SETUP_ID = expected;
            actual = target.DED_TAXES_SETUP_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DED_TAX_COMPONENT_ID
        ///</summary>
        [TestMethod()]
        public void DED_TAX_COMPONENT_IDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            expected = 546;
            target.DED_TAX_COMPONENT_ID = expected;
            actual = target.DED_TAX_COMPONENT_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CRTE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CRTE_USER_IDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            int expected = 4; // TODO: Initialize to an appropriate value
            int actual;
            target.CRTE_USER_ID = expected;
            actual = target.CRTE_USER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CRTE_DT
        ///</summary>
        [TestMethod()]
        public void CRTE_DTTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            expected = Convert.ToDateTime("12/21/2009 7:14:26 AM");
            target.CRTE_DT = expected;
            actual = target.CRTE_DT;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            expected = false;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DeductibleTaxesBE Constructor
        ///</summary>
        [TestMethod()]
        public void DeductibleTaxesBEConstructorTest()
        {
            DeductibleTaxesBE target = new DeductibleTaxesBE();
        }
    }
}
