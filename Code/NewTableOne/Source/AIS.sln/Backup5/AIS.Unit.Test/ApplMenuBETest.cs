using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ApplMenuBETest and is intended
    ///to contain all ApplMenuBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApplMenuBETest
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
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.web_page_txt = expected;
            actual = target.web_page_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for parnt_id
        ///</summary>
        [TestMethod()]
        public void parnt_idTest()
        {
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
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
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
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
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
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
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.menu_id = expected;
            actual = target.menu_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for InquiryOrFull_Access_ind
        ///</summary>
        [TestMethod()]
        public void InquiryOrFull_Access_indTest()
        {
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
            Nullable<char> expected = new Nullable<char>(); // TODO: Initialize to an appropriate value
            Nullable<char> actual;
            target.InquiryOrFull_Access_ind = expected;
            actual = target.InquiryOrFull_Access_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for depnd_txt
        ///</summary>
        [TestMethod()]
        public void depnd_txtTest()
        {
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.depnd_txt = expected;
            actual = target.depnd_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for actv_ind
        ///</summary>
        [TestMethod()]
        public void actv_indTest()
        {
            ApplMenuBE target = new ApplMenuBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.actv_ind = expected;
            actual = target.actv_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ApplMenuBE Constructor
        ///</summary>
        [TestMethod()]
        public void ApplMenuBEConstructorTest()
        {
            ApplMenuBE target = new ApplMenuBE();
            
        }
    }
}
