using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for SubjectAuditPremiumBETest and is intended
    ///to contain all SubjectAuditPremiumBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class SubjectAuditPremiumBETest
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
        ///A test for UpdatedUser_ID
        ///</summary>
        [TestMethod()]
        public void UpdatedUser_IDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UpdatedUser_ID = expected;
            actual = target.UpdatedUser_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UpdatedDate
        ///</summary>
        [TestMethod()]
        public void UpdatedDateTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Sub_Prem_Aud_ID
        ///</summary>
        [TestMethod()]
        public void Sub_Prem_Aud_IDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Sub_Prem_Aud_ID = expected;
            actual = target.Sub_Prem_Aud_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for StateID
        ///</summary>
        [TestMethod()]
        public void StateIDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.StateID = expected;
            actual = target.StateID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STATE
        ///</summary>
        [TestMethod()]
        public void STATETest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.STATE = expected;
            actual = target.STATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Prem_Amt
        ///</summary>
        [TestMethod()]
        public void Prem_AmtTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Prem_Amt = expected;
            actual = target.Prem_Amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Prem_Adj_Pgm_ID
        ///</summary>
        [TestMethod()]
        public void Prem_Adj_Pgm_IDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Prem_Adj_Pgm_ID = expected;
            actual = target.Prem_Adj_Pgm_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Custmr_ID
        ///</summary>
        [TestMethod()]
        public void Custmr_IDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Custmr_ID = expected;
            actual = target.Custmr_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUser_ID
        ///</summary>
        [TestMethod()]
        public void CreatedUser_IDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CreatedUser_ID = expected;
            actual = target.CreatedUser_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedDate
        ///</summary>
        [TestMethod()]
        public void CreatedDateTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Coml_Agmt_ID
        ///</summary>
        [TestMethod()]
        public void Coml_Agmt_IDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Coml_Agmt_ID = expected;
            actual = target.Coml_Agmt_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Coml_Agmt_Audt_ID
        ///</summary>
        [TestMethod()]
        public void Coml_Agmt_Audt_IDTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Coml_Agmt_Audt_ID = expected;
            actual = target.Coml_Agmt_Audt_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Active
        ///</summary>
        [TestMethod()]
        public void ActiveTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.Active = expected;
            actual = target.Active;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SubjectAuditPremiumBE Constructor
        ///</summary>
        [TestMethod()]
        public void SubjectAuditPremiumBEConstructorTest()
        {
            SubjectAuditPremiumBE target = new SubjectAuditPremiumBE();
            
        }
    }
}
