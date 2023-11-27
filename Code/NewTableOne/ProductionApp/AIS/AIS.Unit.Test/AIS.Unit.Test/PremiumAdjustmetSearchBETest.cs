using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmetSearchBETest and is intended
    ///to contain all PremiumAdjustmetSearchBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmetSearchBETest
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
            PremiumAdjustmetSearchBE target = new PremiumAdjustmetSearchBE(); // TODO: Initialize to an appropriate value
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
            PremiumAdjustmetSearchBE target = new PremiumAdjustmetSearchBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.VALN_MM_DT = expected;
            actual = target.VALN_MM_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_IDTest()
        {
            PremiumAdjustmetSearchBE target = new PremiumAdjustmetSearchBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_ID = expected;
            actual = target.PREM_ADJ_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmetSearchBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmetSearchBEConstructorTest()
        {
            PremiumAdjustmetSearchBE target = new PremiumAdjustmetSearchBE();
            
        }
    }
}
