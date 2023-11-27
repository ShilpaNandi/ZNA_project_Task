using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LookupTypeBETest and is intended
    ///to contain all LookupTypeBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class LookupTypeBETest
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
        ///A test for UPDATED_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATED_USER_IDTest()
        {
            LookupTypeBE target = new LookupTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATED_USER_ID = expected;
            actual = target.UPDATED_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATED_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATED_DATETest()
        {
            LookupTypeBE target = new LookupTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATED_DATE = expected;
            actual = target.UPDATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOOKUPTYPE_NAME
        ///</summary>
        [TestMethod()]
        public void LOOKUPTYPE_NAMETest()
        {
            LookupTypeBE target = new LookupTypeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LOOKUPTYPE_NAME = expected;
            actual = target.LOOKUPTYPE_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOOKUPTYPE_ID
        ///</summary>
        [TestMethod()]
        public void LOOKUPTYPE_IDTest()
        {
            LookupTypeBE target = new LookupTypeBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LOOKUPTYPE_ID = expected;
            actual = target.LOOKUPTYPE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATED_USER_IDTest()
        {
            LookupTypeBE target = new LookupTypeBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATED_USER_ID = expected;
            actual = target.CREATED_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_DATE
        ///</summary>
        [TestMethod()]
        public void CREATED_DATETest()
        {
            LookupTypeBE target = new LookupTypeBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATED_DATE = expected;
            actual = target.CREATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTIVE
        ///</summary>
        [TestMethod()]
        public void ACTIVETest()
        {
            LookupTypeBE target = new LookupTypeBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTIVE = expected;
            actual = target.ACTIVE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LookupTypeBE Constructor
        ///</summary>
        [TestMethod()]
        public void LookupTypeBEConstructorTest()
        {
            LookupTypeBE target = new LookupTypeBE();
            
        }
    }
}
