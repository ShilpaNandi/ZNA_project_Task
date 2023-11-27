using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ApplWebPageAuthBETest and is intended
    ///to contain all ApplWebPageAuthBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApplWebPageAuthBETest
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
        ///A test for web_page_txt
        ///</summary>
        [TestMethod()]
        public void web_page_txtTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.web_page_txt = expected;
            actual = target.web_page_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for web_page_auth_id
        ///</summary>
        [TestMethod()]
        public void web_page_auth_idTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.web_page_auth_id = expected;
            actual = target.web_page_auth_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for secur_gp_id
        ///</summary>
        [TestMethod()]
        public void secur_gp_idTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.secur_gp_id = expected;
            actual = target.secur_gp_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for parnt_id
        ///</summary>
        [TestMethod()]
        public void parnt_idTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.parnt_id = expected;
            actual = target.parnt_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for menu_tooltip_txt
        ///</summary>
        [TestMethod()]
        public void menu_tooltip_txtTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.menu_tooltip_txt = expected;
            actual = target.menu_tooltip_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for menu_nm_txt
        ///</summary>
        [TestMethod()]
        public void menu_nm_txtTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.menu_nm_txt = expected;
            actual = target.menu_nm_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for menu_id
        ///</summary>
        [TestMethod()]
        public void menu_idTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.menu_id = expected;
            actual = target.menu_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for authd_ind
        ///</summary>
        [TestMethod()]
        public void authd_indTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.authd_ind = expected;
            actual = target.authd_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ApplWebPageAuthBE Constructor
        ///</summary>
        [TestMethod()]
        public void ApplWebPageAuthBEConstructorTest()
        {
            ApplWebPageAuthBE target = new ApplWebPageAuthBE();
            
        }
    }
}
