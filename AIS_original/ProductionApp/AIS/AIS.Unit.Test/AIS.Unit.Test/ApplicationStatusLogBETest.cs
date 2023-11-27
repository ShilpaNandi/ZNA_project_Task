using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ApplicationStatusLogBETest and is intended
    ///to contain all ApplicationStatusLogBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApplicationStatusLogBETest
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
        ///A test for SRC_TXT
        ///</summary>
        [TestMethod()]
        public void SRC_TXTTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SRC_TXT = expected;
            actual = target.SRC_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SHORT_DESC_TXT
        ///</summary>
        [TestMethod()]
        public void SHORT_DESC_TXTTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SHORT_DESC_TXT = expected;
            actual = target.SHORT_DESC_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SEV_CD
        ///</summary>
        [TestMethod()]
        public void SEV_CDTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SEV_CD = expected;
            actual = target.SEV_CD;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FULL_DESC_TXT
        ///</summary>
        [TestMethod()]
        public void FULL_DESC_TXTTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULL_DESC_TXT = expected;
            actual = target.FULL_DESC_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATE_USER_ID = expected;
            actual = target.CREATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_DATE_string
        ///</summary>
        [TestMethod()]
        public void CREATE_DATE_stringTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CREATE_DATE_string = expected;
            actual = target.CREATE_DATE_string;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_DATE
        ///</summary>
        [TestMethod()]
        public void CREATE_DATETest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for APPLICATION_STS_LOG_ID
        ///</summary>
        [TestMethod()]
        public void APPLICATION_STS_LOG_IDTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.APPLICATION_STS_LOG_ID = expected;
            actual = target.APPLICATION_STS_LOG_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ApplicationStatusLogBE Constructor
        ///</summary>
        [TestMethod()]
        public void ApplicationStatusLogBEConstructorTest()
        {
            ApplicationStatusLogBE target = new ApplicationStatusLogBE();
            
        }
    }
}
