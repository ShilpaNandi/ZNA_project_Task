using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for NonAisCustomerBETest and is intended
    ///to contain all NonAisCustomerBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class NonAisCustomerBETest
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
        ///A test for Updateduserid
        ///</summary>
        [TestMethod()]
        public void UpdateduseridTest()
        {
            NonAisCustomerBE target = new NonAisCustomerBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.Updateduserid = expected;
            actual = target.Updateduserid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Updateddate
        ///</summary>
        [TestMethod()]
        public void UpdateddateTest()
        {
            NonAisCustomerBE target = new NonAisCustomerBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.Updateddate = expected;
            actual = target.Updateddate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Nonaiscustmrid
        ///</summary>
        [TestMethod()]
        public void NonaiscustmridTest()
        {
            NonAisCustomerBE target = new NonAisCustomerBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Nonaiscustmrid = expected;
            actual = target.Nonaiscustmrid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for fullname
        ///</summary>
        [TestMethod()]
        public void fullnameTest()
        {
            NonAisCustomerBE target = new NonAisCustomerBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.fullname = expected;
            actual = target.fullname;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Creteduserid
        ///</summary>
        [TestMethod()]
        public void CreteduseridTest()
        {
            NonAisCustomerBE target = new NonAisCustomerBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Creteduserid = expected;
            actual = target.Creteduserid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Createddate
        ///</summary>
        [TestMethod()]
        public void CreateddateTest()
        {
            NonAisCustomerBE target = new NonAisCustomerBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.Createddate = expected;
            actual = target.Createddate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NonAisCustomerBE Constructor
        ///</summary>
        [TestMethod()]
        public void NonAisCustomerBEConstructorTest()
        {
            NonAisCustomerBE target = new NonAisCustomerBE();
            
        }
    }
}
