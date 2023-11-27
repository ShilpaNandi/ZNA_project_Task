using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LSIAllCustomersBETest and is intended
    ///to contain all LSIAllCustomersBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class LSIAllCustomersBETest
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
        ///A test for NAME_ACCOUNTNO
        ///</summary>
        [TestMethod()]
        public void NAME_ACCOUNTNOTest()
        {
            LSIAllCustomersBE target = new LSIAllCustomersBE(); // TODO: Initialize to an appropriate value
            string expected = " - (0)"; // TODO: Initialize to an appropriate value
            string actual;
            target.NAME_ACCOUNTNO = expected;
            actual = target.NAME_ACCOUNTNO;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSI_ACCT_ID
        ///</summary>
        [TestMethod()]
        public void LSI_ACCT_IDTest()
        {
            LSIAllCustomersBE target = new LSIAllCustomersBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LSI_ACCT_ID = expected;
            actual = target.LSI_ACCT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FULL_NAME
        ///</summary>
        [TestMethod()]
        public void FULL_NAMETest()
        {
            LSIAllCustomersBE target = new LSIAllCustomersBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULL_NAME = expected;
            actual = target.FULL_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LSIAllCustomersBE Constructor
        ///</summary>
        [TestMethod()]
        public void LSIAllCustomersBEConstructorTest()
        {
            LSIAllCustomersBE target = new LSIAllCustomersBE();
            
        }
    }
}
