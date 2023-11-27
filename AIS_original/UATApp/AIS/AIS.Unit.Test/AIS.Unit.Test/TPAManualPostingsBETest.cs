using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for TPAManualPostingsBETest and is intended
    ///to contain all TPAManualPostingsBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class TPAManualPostingsBETest
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
        ///A test for VoidIndicator
        ///</summary>
        [TestMethod()]
        public void VoidIndicatorTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.VoidIndicator = expected;
            actual = target.VoidIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ValuationDate
        ///</summary>
        [TestMethod()]
        public void ValuationDateTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ValuationDate = expected;
            actual = target.ValuationDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UpdatedUserID
        ///</summary>
        [TestMethod()]
        public void UpdatedUserIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
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
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TPA_NAME
        ///</summary>
        [TestMethod()]
        public void TPA_NAMETest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TPA_NAME = expected;
            actual = target.TPA_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ThirdPartyAdminManualInvoiceID
        ///</summary>
        [TestMethod()]
        public void ThirdPartyAdminManualInvoiceIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ThirdPartyAdminManualInvoiceID = expected;
            actual = target.ThirdPartyAdminManualInvoiceID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ThirdPartyAdminLossSrcID
        ///</summary>
        [TestMethod()]
        public void ThirdPartyAdminLossSrcIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ThirdPartyAdminLossSrcID = expected;
            actual = target.ThirdPartyAdminLossSrcID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ThirdPartyAdminInvoiceTypID
        ///</summary>
        [TestMethod()]
        public void ThirdPartyAdminInvoiceTypIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ThirdPartyAdminInvoiceTypID = expected;
            actual = target.ThirdPartyAdminInvoiceTypID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ThirdPartyAdminID
        ///</summary>
        [TestMethod()]
        public void ThirdPartyAdminIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int? actual;
            target.ThirdPartyAdminID = expected;
            actual = target.ThirdPartyAdminID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ReviseIndicator
        ///</summary>
        [TestMethod()]
        public void ReviseIndicatorTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ReviseIndicator = expected;
            actual = target.ReviseIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for RelatedInvoiceID
        ///</summary>
        [TestMethod()]
        public void RelatedInvoiceIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.RelatedInvoiceID = expected;
            actual = target.RelatedInvoiceID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyYearNumber
        ///</summary>
        [TestMethod()]
        public void PolicyYearNumberTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PolicyYearNumber = expected;
            actual = target.PolicyYearNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for InvoiceNumber
        ///</summary>
        [TestMethod()]
        public void InvoiceNumberTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.InvoiceNumber = expected;
            actual = target.InvoiceNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for InvoiceDate
        ///</summary>
        [TestMethod()]
        public void InvoiceDateTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.InvoiceDate = expected;
            actual = target.InvoiceDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVOICE_TYPE_TEXT
        ///</summary>
        [TestMethod()]
        public void INVOICE_TYPE_TEXTTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.INVOICE_TYPE_TEXT = expected;
            actual = target.INVOICE_TYPE_TEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for InoiceAmt
        ///</summary>
        [TestMethod()]
        public void InoiceAmtTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.InoiceAmt = expected;
            actual = target.InoiceAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FinalizedIndicator
        ///</summary>
        [TestMethod()]
        public void FinalizedIndicatorTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.FinalizedIndicator = expected;
            actual = target.FinalizedIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EndDate
        ///</summary>
        [TestMethod()]
        public void EndDateTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.EndDate = expected;
            actual = target.EndDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DueDate
        ///</summary>
        [TestMethod()]
        public void DueDateTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
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
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
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
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
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
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CommentText
        ///</summary>
        [TestMethod()]
        public void CommentTextTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CommentText = expected;
            actual = target.CommentText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CancelIndicator
        ///</summary>
        [TestMethod()]
        public void CancelIndicatorTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.CancelIndicator = expected;
            actual = target.CancelIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BusinessUnitOfficeID
        ///</summary>
        [TestMethod()]
        public void BusinessUnitOfficeIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.BusinessUnitOfficeID = expected;
            actual = target.BusinessUnitOfficeID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BU_OFFICE_TEXT
        ///</summary>
        [TestMethod()]
        public void BU_OFFICE_TEXTTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BU_OFFICE_TEXT = expected;
            actual = target.BU_OFFICE_TEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BillingCycleID
        ///</summary>
        [TestMethod()]
        public void BillingCycleIDTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.BillingCycleID = expected;
            actual = target.BillingCycleID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_NUMBER
        ///</summary>
        [TestMethod()]
        public void ADJ_NUMBERTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJ_NUMBER = expected;
            actual = target.ADJ_NUMBER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Active
        ///</summary>
        [TestMethod()]
        public void ActiveTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.Active = expected;
            actual = target.Active;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACC_NAME
        ///</summary>
        [TestMethod()]
        public void ACC_NAMETest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ACC_NAME = expected;
            actual = target.ACC_NAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TPAManualPostingsBE Constructor
        ///</summary>
        [TestMethod()]
        public void TPAManualPostingsBEConstructorTest()
        {
            TPAManualPostingsBE target = new TPAManualPostingsBE();
            
        }
    }
}
