using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Coml_AgmtBSTest and is intended
    ///to contain all Coml_AgmtBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Coml_AgmtBSTest
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
        ///A test for getCommercialAgreement
        ///</summary>
        [TestMethod()]
        public void getCommercialAgreementTest1()
        {
            Coml_AgmtBS target = new Coml_AgmtBS(); // TODO: Initialize to an appropriate value
            int ProgPerdID = 0; // TODO: Initialize to an appropriate value
            int CustomerID = 0; // TODO: Initialize to an appropriate value
            IList<Coml_AgmtBE> expected = null; // TODO: Initialize to an appropriate value
            IList<Coml_AgmtBE> actual;
            actual = target.getCommercialAgreement(ProgPerdID, CustomerID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getCommercialAgreement
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ZurichNA.AIS.Business.Logic.dll")]
        public void getCommercialAgreementTest()
        {
            Coml_AgmtBS_Accessor target = new Coml_AgmtBS_Accessor(); // TODO: Initialize to an appropriate value
            int ProgPerdID = 0; // TODO: Initialize to an appropriate value
            IList<Coml_AgmtBE> expected = null; // TODO: Initialize to an appropriate value
            IList<Coml_AgmtBE> actual;
            actual = target.getCommercialAgreement(ProgPerdID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Coml_AgmtBS Constructor
        ///</summary>
        [TestMethod()]
        public void Coml_AgmtBSConstructorTest()
        {
            Coml_AgmtBS target = new Coml_AgmtBS();
            
        }
    }
}
