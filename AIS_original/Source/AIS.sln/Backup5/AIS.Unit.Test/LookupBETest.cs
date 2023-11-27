using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LookupBETest and is intended
    ///to contain all LookupBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class LookupBETest
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
        ///A test for LookUpTypeName
        ///</summary>
        [TestMethod()]
        public void LookUpTypeNameTest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LookUpTypeName = expected;
            actual = target.LookUpTypeName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LookUpTypeID
        ///</summary>
        [TestMethod()]
        public void LookUpTypeIDTest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LookUpTypeID = expected;
            actual = target.LookUpTypeID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LookUpName
        ///</summary>
        [TestMethod()]
        public void LookUpNameTest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LookUpName = expected;
            actual = target.LookUpName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LookUpID
        ///</summary>
        [TestMethod()]
        public void LookUpIDTest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LookUpID = expected;
            actual = target.LookUpID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Effective_Date
        ///</summary>
        [TestMethod()]
        public void Effective_DateTest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.Effective_Date = expected;
            actual = target.Effective_Date;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Created_UserID
        ///</summary>
        [TestMethod()]
        public void Created_UserIDTest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Created_UserID = expected;
            actual = target.Created_UserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Created_Date
        ///</summary>
        [TestMethod()]
        public void Created_DateTest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.Created_Date = expected;
            actual = target.Created_Date;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Attribute1
        ///</summary>
        [TestMethod()]
        public void Attribute1Test()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Attribute1 = expected;
            actual = target.Attribute1;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTIVE
        ///</summary>
        [TestMethod()]
        public void ACTIVETest()
        {
            LookupBE target = new LookupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTIVE = expected;
            actual = target.ACTIVE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LookupBE Constructor
        ///</summary>
        [TestMethod()]
        public void LookupBEConstructorTest()
        {
            LookupBE target = new LookupBE();
            
        }
    }
}
