using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Coml_Agmt_AuDtBETest and is intended
    ///to contain all Coml_Agmt_AuDtBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class Coml_Agmt_AuDtBETest
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
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
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
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Sub_Dep_Prm_Amt
        ///</summary>
        [TestMethod()]
        public void Sub_Dep_Prm_AmtTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Sub_Dep_Prm_Amt = expected;
            actual = target.Sub_Dep_Prm_Amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Sub_Aud_Prm_Amt
        ///</summary>
        [TestMethod()]
        public void Sub_Aud_Prm_AmtTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Sub_Aud_Prm_Amt = expected;
            actual = target.Sub_Aud_Prm_Amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for StartDate
        ///</summary>
        [TestMethod()]
        public void StartDateTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.StartDate = expected;
            actual = target.StartDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Prem_Adj_Prg_ID
        ///</summary>
        [TestMethod()]
        public void Prem_Adj_Prg_IDTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Prem_Adj_Prg_ID = expected;
            actual = target.Prem_Adj_Prg_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY
        ///</summary>
        [TestMethod()]
        public void POLICYTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY = expected;
            actual = target.POLICY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Non_Sub_Dep_Prm_Amt
        ///</summary>
        [TestMethod()]
        public void Non_Sub_Dep_Prm_AmtTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Non_Sub_Dep_Prm_Amt = expected;
            actual = target.Non_Sub_Dep_Prm_Amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Non_Sub_Aud_Prm_Amt
        ///</summary>
        [TestMethod()]
        public void Non_Sub_Aud_Prm_AmtTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Non_Sub_Aud_Prm_Amt = expected;
            actual = target.Non_Sub_Aud_Prm_Amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ExposureAmt
        ///</summary>
        [TestMethod()]
        public void ExposureAmtTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ExposureAmt = expected;
            actual = target.ExposureAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Def_Dep_prm_Amt
        ///</summary>
        [TestMethod()]
        public void Def_Dep_prm_AmtTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Def_Dep_prm_Amt = expected;
            actual = target.Def_Dep_prm_Amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Customer_ID
        ///</summary>
        [TestMethod()]
        public void Customer_IDTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Customer_ID = expected;
            actual = target.Customer_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUser_ID
        ///</summary>
        [TestMethod()]
        public void CreatedUser_IDTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
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
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Comm_Agr_ID
        ///</summary>
        [TestMethod()]
        public void Comm_Agr_IDTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Comm_Agr_ID = expected;
            actual = target.Comm_Agr_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Comm_Agr_Audit_ID
        ///</summary>
        [TestMethod()]
        public void Comm_Agr_Audit_IDTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Comm_Agr_Audit_ID = expected;
            actual = target.Comm_Agr_Audit_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Audit_Reslt_Amt
        ///</summary>
        [TestMethod()]
        public void Audit_Reslt_AmtTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Audit_Reslt_Amt = expected;
            actual = target.Audit_Reslt_Amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Aud_Rev_Status
        ///</summary>
        [TestMethod()]
        public void Aud_Rev_StatusTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.Aud_Rev_Status = expected;
            actual = target.Aud_Rev_Status;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustmentIndicator
        ///</summary>
        [TestMethod()]
        public void AdjustmentIndicatorTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.AdjustmentIndicator = expected;
            actual = target.AdjustmentIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Coml_Agmt_AuDtBE Constructor
        ///</summary>
        [TestMethod()]
        public void Coml_Agmt_AuDtBEConstructorTest()
        {
            Coml_Agmt_AuDtBE target = new Coml_Agmt_AuDtBE();
            
        }
    }
}
