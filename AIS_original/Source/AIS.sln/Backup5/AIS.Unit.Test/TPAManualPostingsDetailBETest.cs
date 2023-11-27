using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for TPAManualPostingsDetailBETest and is intended
    ///to contain all TPAManualPostingsDetailBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class TPAManualPostingsDetailBETest
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
        ///A test for UpdatedUserID
        ///</summary>
        [TestMethod()]
        public void UpdatedUserIDTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UpdatedUserID = expected;
            actual = target.UpdatedUserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UpdatedDate
        ///</summary>
        [TestMethod()]
        public void UpdatedDateTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ThirdPartyAdminManualInvoiceID
        ///</summary>
        [TestMethod()]
        public void ThirdPartyAdminManualInvoiceIDTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ThirdPartyAdminManualInvoiceID = expected;
            actual = target.ThirdPartyAdminManualInvoiceID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ThirdPartyAdminManualInvoiceDtlID
        ///</summary>
        [TestMethod()]
        public void ThirdPartyAdminManualInvoiceDtlIDTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ThirdPartyAdminManualInvoiceDtlID = expected;
            actual = target.ThirdPartyAdminManualInvoiceDtlID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ThirdPartyAdminAmt
        ///</summary>
        [TestMethod()]
        public void ThirdPartyAdminAmtTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ThirdPartyAdminAmt = expected;
            actual = target.ThirdPartyAdminAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PostTransactionText
        ///</summary>
        [TestMethod()]
        public void PostTransactionTextTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PostTransactionText = expected;
            actual = target.PostTransactionText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PostingTrnsTypID
        ///</summary>
        [TestMethod()]
        public void PostingTrnsTypIDTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PostingTrnsTypID = expected;
            actual = target.PostingTrnsTypID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicySymbolText
        ///</summary>
        [TestMethod()]
        public void PolicySymbolTextTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicySymbolText = expected;
            actual = target.PolicySymbolText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyNbrText
        ///</summary>
        [TestMethod()]
        public void PolicyNbrTextTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicyNbrText = expected;
            actual = target.PolicyNbrText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyModText
        ///</summary>
        [TestMethod()]
        public void PolicyModTextTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicyModText = expected;
            actual = target.PolicyModText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY
        ///</summary>
        [TestMethod()]
        public void POLICYTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY = expected;
            actual = target.POLICY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ExpiryDate
        ///</summary>
        [TestMethod()]
        public void ExpiryDateTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ExpiryDate = expected;
            actual = target.ExpiryDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EffectiveDate
        ///</summary>
        [TestMethod()]
        public void EffectiveDateTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.EffectiveDate = expected;
            actual = target.EffectiveDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DueDate
        ///</summary>
        [TestMethod()]
        public void DueDateTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DueDate = expected;
            actual = target.DueDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CustomerID
        ///</summary>
        [TestMethod()]
        public void CustomerIDTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CustomerID = expected;
            actual = target.CustomerID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUserID
        ///</summary>
        [TestMethod()]
        public void CreatedUserIDTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CreatedUserID = expected;
            actual = target.CreatedUserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedDate
        ///</summary>
        [TestMethod()]
        public void CreatedDateTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CompanyCode
        ///</summary>
        [TestMethod()]
        public void CompanyCodeTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CompanyCode = expected;
            actual = target.CompanyCode;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AriesSubNbr
        ///</summary>
        [TestMethod()]
        public void AriesSubNbrTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.AriesSubNbr = expected;
            actual = target.AriesSubNbr;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AriesMainNbr
        ///</summary>
        [TestMethod()]
        public void AriesMainNbrTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.AriesMainNbr = expected;
            actual = target.AriesMainNbr;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TPAManualPostingsDetailBE Constructor
        ///</summary>
        [TestMethod()]
        public void TPAManualPostingsDetailBEConstructorTest()
        {
            TPAManualPostingsDetailBE target = new TPAManualPostingsDetailBE();
            
        }
    }
}
