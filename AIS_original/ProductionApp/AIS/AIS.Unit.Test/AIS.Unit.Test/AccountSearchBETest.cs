using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AccountSearchBETest and is intended
    ///to contain all AccountSearchBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AccountSearchBETest
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
        ///A test for SUPRT_SERV_CUSTMR_GP_ID
        ///</summary>
        [TestMethod()]
        public void SUPRT_SERV_CUSTMR_GP_IDTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.SUPRT_SERV_CUSTMR_GP_ID = expected;
            actual = target.SUPRT_SERV_CUSTMR_GP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POSTAL_ADDRESS
        ///</summary>
        [TestMethod()]
        public void POSTAL_ADDRESSTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POSTAL_ADDRESS = expected;
            actual = target.POSTAL_ADDRESS;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MSTR_ACCT_IND
        ///</summary>
        [TestMethod()]
        public void MSTR_ACCT_INDTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.MSTR_ACCT_IND = expected;
            actual = target.MSTR_ACCT_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IS_MSTR_ACCT
        ///</summary>
        [TestMethod()]
        public void IS_MSTR_ACCTTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.IS_MSTR_ACCT;
            
        }

        /// <summary>
        ///A test for FULL_NM
        ///</summary>
        [TestMethod()]
        public void FULL_NMTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULL_NM = expected;
            actual = target.FULL_NM;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FINC_PTY_ID
        ///</summary>
        [TestMethod()]
        public void FINC_PTY_IDTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            target.FINC_PTY_ID = expected;
            actual = target.FINC_PTY_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_REL_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_REL_IDTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CUSTMR_REL_ID = expected;
            actual = target.CUSTMR_REL_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            AccountSearchBE target = new AccountSearchBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AccountSearchBE Constructor
        ///</summary>
        [TestMethod()]
        public void AccountSearchBEConstructorTest()
        {
            AccountSearchBE target = new AccountSearchBE();
            
        }
    }
}
