using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for SteppedFactorBSTest and is intended
    ///to contain all SteppedFactorBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SteppedFactorBSTest
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
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            SteppedFactorBS target = new SteppedFactorBS(); // TODO: Initialize to an appropriate value
            SteppedFactorBE steppedFactor = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(steppedFactor);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetSteppedFactorData
        ///</summary>
        [TestMethod()]
        public void GetSteppedFactorDataTest()
        {
            SteppedFactorBS target = new SteppedFactorBS(); // TODO: Initialize to an appropriate value
            int policyID = 0; // TODO: Initialize to an appropriate value
            IList<SteppedFactorBE> expected = null; // TODO: Initialize to an appropriate value
            IList<SteppedFactorBE> actual;
            actual = target.GetSteppedFactorData(policyID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getGetSteppedFactor
        ///</summary>
        [TestMethod()]
        public void getGetSteppedFactorTest()
        {
            SteppedFactorBS target = new SteppedFactorBS(); // TODO: Initialize to an appropriate value
            int STEPPED_FACTOR_ID = 0; // TODO: Initialize to an appropriate value
            SteppedFactorBE expected = null; // TODO: Initialize to an appropriate value
            SteppedFactorBE actual;
            actual = target.getGetSteppedFactor(STEPPED_FACTOR_ID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SteppedFactorBS Constructor
        ///</summary>
        [TestMethod()]
        public void SteppedFactorBSConstructorTest()
        {
            SteppedFactorBS target = new SteppedFactorBS();
            
        }
    }
}
