using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for TaxExemptionBETest and is intended
    ///to contain all TaxExemptionBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class TaxExemptionBETest
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


        /// <summary>
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            expected = 0;
            target.UPDATE_USER_ID = expected;
            actual = target.UPDATE_USER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UPDATE_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATE_DATETest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            expected = Convert.ToDateTime("12/29/2009 5:16:30 AM");
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TAX_EXMP_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void TAX_EXMP_SETUP_IDTest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            int expected = 1; // TODO: Initialize to an appropriate value
            int actual;
            target.TAX_EXMP_SETUP_ID = expected;
            actual = target.TAX_EXMP_SETUP_ID;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            int expected = 707; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for STATE_ID
        ///</summary>
        [TestMethod()]
        public void STATE_IDTest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            int expected = 47; // TODO: Initialize to an appropriate value
            int actual;
            target.ST_ID = expected;
            actual = target.ST_ID;
            Assert.AreEqual(expected, actual);

        }



        /// <summary>
        ///A test for CUSTOMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_IDTest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            int expected = 106; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMER_ID = expected;
            actual = target.CUSTOMER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATE_USER_ID = expected;
            actual = target.CREATE_USER_ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CREATE_DATE
        ///</summary>
        [TestMethod()]
        public void CREATE_DATETest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            expected = Convert.ToDateTime("12/18/2009 12:04:27 AM");
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            TaxExemptionBE target = new TaxExemptionBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            expected = true;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
        }



        /// <summary>
        ///A test for TaxExemptionBE Constructor
        ///</summary>
        [TestMethod()]
        public void TaxExemptionBEConstructorTest()
        {
            TaxExemptionBE target = new TaxExemptionBE();
        }
    }
}

