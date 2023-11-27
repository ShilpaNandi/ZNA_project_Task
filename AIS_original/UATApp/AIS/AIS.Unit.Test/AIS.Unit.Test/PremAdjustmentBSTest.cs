using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremAdjustmentBSTest and is intended
    ///to contain all PremAdjustmentBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremAdjustmentBSTest
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
        ///A test for GetAdjustmentInfo
        ///</summary>
        [TestMethod()]
        public void GetAdjustmentInfoTest()
        {
            PremAdjustmentBS target = new PremAdjustmentBS(); // TODO: Initialize to an appropriate value
            int accountID = 0; // TODO: Initialize to an appropriate value
            int statusID = 0; // TODO: Initialize to an appropriate value
            int personID = 0; // TODO: Initialize to an appropriate value
            string pending = "All"; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentBE> actual;
            actual = target.GetAdjustmentInfo(accountID, statusID, personID, pending);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetInvoiceInfo
        ///</summary>
        [TestMethod()]
        public void GetInvoiceInfoTest()
        {
            PremAdjustmentBS target = new PremAdjustmentBS(); // TODO: Initialize to an appropriate value
            int accountID = 0; // TODO: Initialize to an appropriate value
            int personID = 0; // TODO: Initialize to an appropriate value
            string qcComplete = string.Empty; // TODO: Initialize to an appropriate value
            string ariesClrng = string.Empty; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentBE> actual;
            actual = target.GetInvoiceInfo(accountID, personID, qcComplete, ariesClrng,false);
            Assert.AreEqual(expected, actual);
            
        }
    }
}
