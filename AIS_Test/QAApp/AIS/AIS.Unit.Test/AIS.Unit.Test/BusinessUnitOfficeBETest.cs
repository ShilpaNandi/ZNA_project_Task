using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for BusinessUnitOfficeBETest and is intended
    ///to contain all BusinessUnitOfficeBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessUnitOfficeBETest
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
        ///A test for INTRNL_ORG_ID
        ///</summary>
        [TestMethod()]
        public void INTRNL_ORG_IDTest()
        {
            BusinessUnitOfficeBE target = new BusinessUnitOfficeBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.INTRNL_ORG_ID = expected;
            actual = target.INTRNL_ORG_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FULL_NAME
        ///</summary>
        [TestMethod()]
        public void FULL_NAMETest()
        {
            BusinessUnitOfficeBE target = new BusinessUnitOfficeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULL_NAME = expected;
            actual = target.FULL_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CITY_NM
        ///</summary>
        [TestMethod()]
        public void CITY_NMTest()
        {
            BusinessUnitOfficeBE target = new BusinessUnitOfficeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CITY_NM = expected;
            actual = target.CITY_NM;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BSN_UNT_CD
        ///</summary>
        [TestMethod()]
        public void BSN_UNT_CDTest()
        {
            BusinessUnitOfficeBE target = new BusinessUnitOfficeBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BSN_UNT_CD = expected;
            actual = target.BSN_UNT_CD;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BusinessUnitOfficeBE Constructor
        ///</summary>
        [TestMethod()]
        public void BusinessUnitOfficeBEConstructorTest()
        {
            BusinessUnitOfficeBE target = new BusinessUnitOfficeBE();
            
        }
    }
}
