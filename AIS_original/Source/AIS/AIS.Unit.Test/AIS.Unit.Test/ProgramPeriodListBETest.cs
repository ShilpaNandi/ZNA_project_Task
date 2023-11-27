using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ProgramPeriodListBETest and is intended
    ///to contain all ProgramPeriodListBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramPeriodListBETest
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
        ///A test for VALUATIONDATE
        ///</summary>
        [TestMethod()]
        public void VALUATIONDATETest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.VALUATIONDATE = expected;
            actual = target.VALUATIONDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for VALN_MM_DT
        ///</summary>
        [TestMethod()]
        public void VALN_MM_DTTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.VALN_MM_DT = expected;
            actual = target.VALN_MM_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STRT_DT
        ///</summary>
        [TestMethod()]
        public void STRT_DTTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.STRT_DT = expected;
            actual = target.STRT_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PROGRAMTYPENAME
        ///</summary>
        [TestMethod()]
        public void PROGRAMTYPENAMETest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PROGRAMTYPENAME = expected;
            actual = target.PROGRAMTYPENAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PLAN_END_DT
        ///</summary>
        [TestMethod()]
        public void PLAN_END_DTTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PLAN_END_DT = expected;
            actual = target.PLAN_END_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PGM_TYP_ID
        ///</summary>
        [TestMethod()]
        public void PGM_TYP_IDTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PGM_TYP_ID = expected;
            actual = target.PGM_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Lookup
        ///</summary>
        [TestMethod()]
        public void LookupTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.Lookup;
            
        }

        /// <summary>
        ///A test for IntlOrganization
        ///</summary>
        [TestMethod()]
        public void IntlOrganizationTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            BusinessUnitOfficeBE actual;
            actual = target.IntlOrganization;
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BUSINESSUNITNAME
        ///</summary>
        [TestMethod()]
        public void BUSINESSUNITNAMETest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BUSINESSUNITNAME = expected;
            actual = target.BUSINESSUNITNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BSN_UNT_OFC_ID
        ///</summary>
        [TestMethod()]
        public void BSN_UNT_OFC_IDTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.BSN_UNT_OFC_ID = expected;
            actual = target.BSN_UNT_OFC_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BROKERNAME
        ///</summary>
        [TestMethod()]
        public void BROKERNAMETest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BROKERNAME = expected;
            actual = target.BROKERNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Broker
        ///</summary>
        [TestMethod()]
        public void BrokerTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            BrokerBE actual;
            actual = target.Broker;
            
        }

        /// <summary>
        ///A test for BRKR_ID
        ///</summary>
        [TestMethod()]
        public void BRKR_IDTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.BRKR_ID = expected;
            actual = target.BRKR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ProgramPeriodListBE Constructor
        ///</summary>
        [TestMethod()]
        public void ProgramPeriodListBEConstructorTest()
        {
            ProgramPeriodListBE target = new ProgramPeriodListBE();
            
        }
    }
}
