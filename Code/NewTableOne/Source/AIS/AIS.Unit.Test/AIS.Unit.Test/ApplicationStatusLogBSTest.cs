using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ApplicationStatusLogBSTest and is intended
    ///to contain all ApplicationStatusLogBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApplicationStatusLogBSTest
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
        ///A test for WriteLog
        ///</summary>
        [TestMethod()]
        public void WriteLogTest()
        {
            ApplicationStatusLogBS target = new ApplicationStatusLogBS(); // TODO: Initialize to an appropriate value
            int intPremAdjID = 0; // TODO: Initialize to an appropriate value
            string strSrcTxt = string.Empty; // TODO: Initialize to an appropriate value
            string strSevCD = string.Empty; // TODO: Initialize to an appropriate value
            string strShortDescText = string.Empty; // TODO: Initialize to an appropriate value
            string strFullDescTest = string.Empty; // TODO: Initialize to an appropriate value
            int intCreatedUserID = 0; // TODO: Initialize to an appropriate value
            target.WriteLog(intPremAdjID, strSrcTxt, strSevCD, strShortDescText, strFullDescTest, intCreatedUserID);
        }

        /// <summary>
        ///A test for getLogData
        ///</summary>
        [TestMethod()]
        public void getLogDataTest()
        {
            ApplicationStatusLogBS target = new ApplicationStatusLogBS(); 
            int AcctNumber = 0; 
            string InterfaceType = string.Empty; 
            string fromDate = string.Empty; 
            string toDate = string.Empty; 
            IList<ApplicationStatusLogBE> expected = null; 
            IList<ApplicationStatusLogBE> actual;
            actual = target.getLogData(AcctNumber, InterfaceType, fromDate, toDate);
            Assert.AreEqual(0, actual.Count);
        }

        /// <summary>
        ///A test for ApplicationStatusLogBS Constructor
        ///</summary>
        [TestMethod()]
        public void ApplicationStatusLogBSConstructorTest()
        {
            ApplicationStatusLogBS target = new ApplicationStatusLogBS();
            Assert.IsInstanceOfType(target, typeof(ApplicationStatusLogBS));
        }
    }
}
