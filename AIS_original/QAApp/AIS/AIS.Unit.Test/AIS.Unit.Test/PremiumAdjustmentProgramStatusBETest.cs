using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentProgramStatusBETest and is intended
    ///to contain all PremiumAdjustmentProgramStatusBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentProgramStatusBETest
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
        ///A test for UPDATEUSER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATEUSER_IDTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATEUSER_ID = expected;
            actual = target.UPDATEUSER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDATE
        ///</summary>
        [TestMethod()]
        public void UPDATEDATETest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDATE = expected;
            actual = target.UPDATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STS_CHK_IND
        ///</summary>
        [TestMethod()]
        public void STS_CHK_INDTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.STS_CHK_IND = expected;
            actual = target.STS_CHK_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMUMADJPROG_STS_ID
        ///</summary>
        [TestMethod()]
        public void PREMUMADJPROG_STS_IDTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREMUMADJPROG_STS_ID = expected;
            actual = target.PREMUMADJPROG_STS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PGM_STATUS_ID
        ///</summary>
        [TestMethod()]
        public void PGM_STATUS_IDTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PGM_STATUS_ID = expected;
            actual = target.PGM_STATUS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDUSER_ID
        ///</summary>
        [TestMethod()]
        public void CREATEDUSER_IDTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATEDUSER_ID = expected;
            actual = target.CREATEDUSER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDDATE
        ///</summary>
        [TestMethod()]
        public void CREATEDDATETest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDDATE = expected;
            actual = target.CREATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmentProgramStatusBE Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmentProgramStatusBEConstructorTest()
        {
            PremiumAdjustmentProgramStatusBE target = new PremiumAdjustmentProgramStatusBE();
            
        }
    }
}
