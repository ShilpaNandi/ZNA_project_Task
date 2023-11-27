using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AccountBETest and is intended
    ///to contain all AccountBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AccountBETest
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
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATE_USER_ID = expected;
            actual = target.UPDATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATE_DATETest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TPA_FUNDED_IND
        ///</summary>
        [TestMethod()]
        public void TPA_FUNDED_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.TPA_FUNDED_IND = expected;
            actual = target.TPA_FUNDED_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for THIRD_PARTY_ADMIN_FUNDED_DATE
        ///</summary>
        [TestMethod()]
        public void THIRD_PARTY_ADMIN_FUNDED_DATETest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.THIRD_PARTY_ADMIN_FUNDED_DATE = expected;
            actual = target.THIRD_PARTY_ADMIN_FUNDED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SUPRT_SERV_CUSTMR_GP_ID
        ///</summary>
        [TestMethod()]
        public void SUPRT_SERV_CUSTMR_GP_IDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SUPRT_SERV_CUSTMR_GP_ID = expected;
            actual = target.SUPRT_SERV_CUSTMR_GP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERSON_ID
        ///</summary>
        [TestMethod()]
        public void PERSON_IDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PERSON_ID = expected;
            actual = target.PERSON_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PEO_IND
        ///</summary>
        [TestMethod()]
        public void PEO_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.PEO_IND = expected;
            actual = target.PEO_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MSTR_ACCT_IND
        ///</summary>
        [TestMethod()]
        public void MSTR_ACCT_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.MSTR_ACCT_IND = expected;
            actual = target.MSTR_ACCT_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MARYLAND_RETRO_IND
        ///</summary>
        [TestMethod()]
        public void MARYLAND_RETRO_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.MARYLAND_RETRO_IND = expected;
            actual = target.MARYLAND_RETRO_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IS_MSTR_ACCT_IND
        ///</summary>
        [TestMethod()]
        public void IS_MSTR_ACCT_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.IS_MSTR_ACCT_IND;
            
        }

        /// <summary>
        ///A test for IS_CUSTMR_REL_ACTV_IND
        ///</summary>
        [TestMethod()]
        public void IS_CUSTMR_REL_ACTV_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.IS_CUSTMR_REL_ACTV_IND;
            
        }

        /// <summary>
        ///A test for FULL_NM
        ///</summary>
        [TestMethod()]
        public void FULL_NMTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
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
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
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
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CUSTMR_REL_ID = expected;
            actual = target.CUSTMR_REL_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_REL_ACTV_IND
        ///</summary>
        [TestMethod()]
        public void CUSTMR_REL_ACTV_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.CUSTMR_REL_ACTV_IND = expected;
            actual = target.CUSTMR_REL_ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATE_USER_ID = expected;
            actual = target.CREATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_DATE
        ///</summary>
        [TestMethod()]
        public void CREATE_DATETest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BKTCYBUYOUT_ID
        ///</summary>
        [TestMethod()]
        public void BKTCYBUYOUT_IDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.BKTCYBUYOUT_ID = expected;
            actual = target.BKTCYBUYOUT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BKTCYBUYOUT_DATE
        ///</summary>
        [TestMethod()]
        public void BKTCYBUYOUT_DATETest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.BKTCYBUYOUT_DATE = expected;
            actual = target.BKTCYBUYOUT_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            AccountBE target = new AccountBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AccountBE Constructor
        ///</summary>
        [TestMethod()]
        public void AccountBEConstructorTest()
        {
            AccountBE target = new AccountBE();
            
        }
    }
}
