using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjustmentParameterPolicyBETest and is intended
    ///to contain all AdjustmentParameterPolicyBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjustmentParameterPolicyBETest
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
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
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
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PrmadjPRgmID
        ///</summary>
        [TestMethod()]
        public void PrmadjPRgmIDTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PrmadjPRgmID = expected;
            actual = target.PrmadjPRgmID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyPerfectNumber
        ///</summary>
        [TestMethod()]
        public void PolicyPerfectNumberTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicyPerfectNumber = expected;
            actual = target.PolicyPerfectNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for custmrID
        ///</summary>
        [TestMethod()]
        public void custmrIDTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.custmrID = expected;
            actual = target.custmrID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
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
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for coml_agmt_id
        ///</summary>
        [TestMethod()]
        public void coml_agmt_idTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.coml_agmt_id = expected;
            actual = target.coml_agmt_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for adj_paramet_setup_id
        ///</summary>
        [TestMethod()]
        public void adj_paramet_setup_idTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.adj_paramet_setup_id = expected;
            actual = target.adj_paramet_setup_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for adj_paramet_pol_id
        ///</summary>
        [TestMethod()]
        public void adj_paramet_pol_idTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.adj_paramet_pol_id = expected;
            actual = target.adj_paramet_pol_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustmentParameterPolicyBE Constructor
        ///</summary>
        [TestMethod()]
        public void AdjustmentParameterPolicyBEConstructorTest()
        {
            AdjustmentParameterPolicyBE target = new AdjustmentParameterPolicyBE();
            
        }
    }
}
