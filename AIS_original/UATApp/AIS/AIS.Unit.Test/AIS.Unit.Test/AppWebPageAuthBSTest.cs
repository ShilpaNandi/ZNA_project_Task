using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AppWebPageAuthBSTest and is intended
    ///to contain all AppWebPageAuthBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AppWebPageAuthBSTest
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
        ///A test for RetrieveAuthWebPages
        ///</summary>
        [TestMethod()]
        public void RetrieveAuthWebPagesTest()
        {
            AppWebPageAuthBS target = new AppWebPageAuthBS(); // TODO: Initialize to an appropriate value
            string rolename = "SystemAdmin"; // TODO: Initialize to an appropriate value
            IList<ApplMenuBE> actual;
            actual = target.RetrieveAuthWebPages(rolename);
            Assert.AreNotEqual(0,actual.Count);
        }

        /// <summary>
        ///A test for AppWebPageAuthBS Constructor
        ///</summary>
        [TestMethod()]
        public void AppWebPageAuthBSConstructorTest()
        {
            AppWebPageAuthBS target = new AppWebPageAuthBS();
            Assert.IsInstanceOfType(target, typeof(AppWebPageAuthBS));
        }
    }
}
