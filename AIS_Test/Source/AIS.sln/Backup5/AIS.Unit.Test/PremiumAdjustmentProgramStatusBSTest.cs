using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PremiumAdjustmentProgramStatusBSTest and is intended
    ///to contain all PremiumAdjustmentProgramStatusBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PremiumAdjustmentProgramStatusBSTest
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
            PremiumAdjustmentProgramStatusBS target = new PremiumAdjustmentProgramStatusBS(); // TODO: Initialize to an appropriate value
            PremiumAdjustmentProgramStatusBE premAdjProgStsBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(premAdjProgStsBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetProgramStatusList
        ///</summary>
        [TestMethod()]
        public void GetProgramStatusListTest()
        {
            PremiumAdjustmentProgramStatusBS target = new PremiumAdjustmentProgramStatusBS(); // TODO: Initialize to an appropriate value
            int accountID = 0; // TODO: Initialize to an appropriate value
            int progPerID = 0; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentProgramStatusBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentProgramStatusBE> actual;
            actual = target.GetProgramStatusList(accountID, progPerID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPremAdjProgSt
        ///</summary>
        [TestMethod()]
        public void getPremAdjProgStTest()
        {
            PremiumAdjustmentProgramStatusBS target = new PremiumAdjustmentProgramStatusBS(); // TODO: Initialize to an appropriate value
            int premAdjProgStsID = 0; // TODO: Initialize to an appropriate value
            PremiumAdjustmentProgramStatusBE expected = null; // TODO: Initialize to an appropriate value
            PremiumAdjustmentProgramStatusBE actual;
            actual = target.getPremAdjProgSt(premAdjProgStsID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            PremiumAdjustmentProgramStatusBS target = new PremiumAdjustmentProgramStatusBS(); // TODO: Initialize to an appropriate value
            PremiumAdjustmentProgramStatusBE premAdjProgStsBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Delete(premAdjProgStsBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmentProgramStatusBS Constructor
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmentProgramStatusBSConstructorTest()
        {
            PremiumAdjustmentProgramStatusBS target = new PremiumAdjustmentProgramStatusBS();
            
        }
    }
}
