using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ProgramPeriodSearchListBETest and is intended
    ///to contain all ProgramPeriodSearchListBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramPeriodSearchListBETest
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
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE(); // TODO: Initialize to an appropriate value
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
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE(); 
            Nullable<DateTime> expected = new DateTime(2009,1,1); 
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
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE(); 
            Nullable<DateTime> expected = new DateTime(2009,1,1); 
            Nullable<DateTime> actual;
            target.STRT_DT = expected;
            actual = target.STRT_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STARTDATE_ENDDATE
        ///</summary>
        [TestMethod()]
        public void STARTDATE_ENDDATETest()
        {
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE(); 
            string expected = null; 
            string actual;
            target.STARTDATE_ENDDATE = expected;
            actual = target.STARTDATE_ENDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE(); 
            int expected = 0; 
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PLAN_END_DT
        ///</summary>
        [TestMethod()]
        public void PLAN_END_DTTest()
        {
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE(); 
            Nullable<DateTime> expected = new DateTime(2009,1,1); 
            Nullable<DateTime> actual;
            target.PLAN_END_DT = expected;
            actual = target.PLAN_END_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ProgramPeriodSearchListBE Constructor
        ///</summary>
        [TestMethod()]
        public void ProgramPeriodSearchListBEConstructorTest()
        {
            ProgramPeriodSearchListBE target = new ProgramPeriodSearchListBE();
            
        }
    }
}
