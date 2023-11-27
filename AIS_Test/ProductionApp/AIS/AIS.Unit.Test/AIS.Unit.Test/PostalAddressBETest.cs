using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PostalAddressBETest and is intended
    ///to contain all PostalAddressBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PostalAddressBETest
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
        ///A test for ZIP_CODE
        ///</summary>
        [TestMethod()]
        public void ZIP_CODETest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ZIP_CODE = expected;
            actual = target.ZIP_CODE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDUSER
        ///</summary>
        [TestMethod()]
        public void UPDATEDUSERTest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATEDUSER = expected;
            actual = target.UPDATEDUSER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDDATE
        ///</summary>
        [TestMethod()]
        public void UPDATEDDATETest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDDATE = expected;
            actual = target.UPDATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STATE_TXT
        ///</summary>
        [TestMethod()]
        public void STATE_TXTTest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.STATE_TXT = expected;
            actual = target.STATE_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STATE_ID
        ///</summary>
        [TestMethod()]
        public void STATE_IDTest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.STATE_ID = expected;
            actual = target.STATE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POSTALADDRESSID
        ///</summary>
        [TestMethod()]
        public void POSTALADDRESSIDTest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.POSTALADDRESSID = expected;
            actual = target.POSTALADDRESSID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERSON_ID
        ///</summary>
        [TestMethod()]
        public void PERSON_IDTest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PERSON_ID = expected;
            actual = target.PERSON_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDUSER
        ///</summary>
        [TestMethod()]
        public void CREATEDUSERTest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATEDUSER = expected;
            actual = target.CREATEDUSER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDDATE
        ///</summary>
        [TestMethod()]
        public void CREATEDDATETest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDDATE = expected;
            actual = target.CREATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CITY
        ///</summary>
        [TestMethod()]
        public void CITYTest()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CITY = expected;
            actual = target.CITY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADDRESS2
        ///</summary>
        [TestMethod()]
        public void ADDRESS2Test()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADDRESS2 = expected;
            actual = target.ADDRESS2;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADDRESS1
        ///</summary>
        [TestMethod()]
        public void ADDRESS1Test()
        {
            PostalAddressBE target = new PostalAddressBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADDRESS1 = expected;
            actual = target.ADDRESS1;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PostalAddressBE Constructor
        ///</summary>
        [TestMethod()]
        public void PostalAddressBEConstructorTest()
        {
            PostalAddressBE target = new PostalAddressBE();
            
        }
    }
}
