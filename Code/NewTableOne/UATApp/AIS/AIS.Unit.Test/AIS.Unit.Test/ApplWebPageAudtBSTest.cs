using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ApplWebPageAudtBSTest and is intended
    ///to contain all ApplWebPageAudtBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApplWebPageAudtBSTest
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
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest5()
        {
            ApplWebPageAudtBS target = new ApplWebPageAudtBS(); 
            int custmr_ID = 1;
            string webPageName = "Policy Info"; 
            int userID = 1; 
            string updatedColumn = "Valuation Date"; 
            target.Save(custmr_ID, webPageName, userID, updatedColumn);
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest4()
        {
            ApplWebPageAudtBS target = new ApplWebPageAudtBS(); 
            string webPageName = string.Empty; 
            int userID = 0; 
            string updatedColumn = string.Empty; 
            target.Save(webPageName, userID, updatedColumn);
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest3()
        {
            ApplWebPageAudtBS target = new ApplWebPageAudtBS(); // TODO: Initialize to an appropriate value
            string webPageName = string.Empty; // TODO: Initialize to an appropriate value
            int userID = 0; // TODO: Initialize to an appropriate value
            target.Save(webPageName, userID);
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest2()
        {
            ApplWebPageAudtBS target = new ApplWebPageAudtBS(); // TODO: Initialize to an appropriate value
            int custmr_ID = 0; // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            string webPageName = string.Empty; // TODO: Initialize to an appropriate value
            int userID = 0; // TODO: Initialize to an appropriate value
            target.Save(custmr_ID, programPeriodID, webPageName, userID);
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest1()
        {
            ApplWebPageAudtBS target = new ApplWebPageAudtBS(); // TODO: Initialize to an appropriate value
            int custmr_ID = 0; // TODO: Initialize to an appropriate value
            string webPageName = string.Empty; // TODO: Initialize to an appropriate value
            int userID = 0; // TODO: Initialize to an appropriate value
            target.Save(custmr_ID, webPageName, userID);
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest()
        {
            ApplWebPageAudtBS target = new ApplWebPageAudtBS(); // TODO: Initialize to an appropriate value
            int custmr_ID = 0; // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            string webPageName = string.Empty; // TODO: Initialize to an appropriate value
            int userID = 0; // TODO: Initialize to an appropriate value
            string updatedColumn = string.Empty; // TODO: Initialize to an appropriate value
            target.Save(custmr_ID, programPeriodID, webPageName, userID, updatedColumn);
        }

        /// <summary>
        ///A test for ApplWebPageAudtBS Constructor
        ///</summary>
        [TestMethod()]
        public void ApplWebPageAudtBSConstructorTest()
        {
            ApplWebPageAudtBS target = new ApplWebPageAudtBS();
            Assert.IsInstanceOfType(target, typeof(ApplWebPageAudtBS));
        }
    }
}
