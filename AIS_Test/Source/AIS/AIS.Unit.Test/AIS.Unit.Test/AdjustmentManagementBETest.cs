using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjustmentManagementBETest and is intended
    ///to contain all AdjustmentManagementBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjustmentManagementBETest
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
        ///A test for VoidReasonIndc
        ///</summary>
        [TestMethod()]
        public void VoidReasonIndcTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.VoidReasonIndc = expected;
            actual = target.VoidReasonIndc;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for VoidReasonID
        ///</summary>
        [TestMethod()]
        public void VoidReasonIDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.VoidReasonID = expected;
            actual = target.VoidReasonID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ValtnDate
        ///</summary>
        [TestMethod()]
        public void ValtnDateTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.ValtnDate = expected;
            actual = target.ValtnDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
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
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for twntyqtrlind
        ///</summary>
        [TestMethod()]
        public void twntyqtrlindTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.twntyqtrlind = expected;
            actual = target.twntyqtrlind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ReviseReasonIndc
        ///</summary>
        [TestMethod()]
        public void ReviseReasonIndcTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ReviseReasonIndc = expected;
            actual = target.ReviseReasonIndc;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ReviseReasonID
        ///</summary>
        [TestMethod()]
        public void ReviseReasonIDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ReviseReasonID = expected;
            actual = target.ReviseReasonID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for prem_adjID
        ///</summary>
        [TestMethod()]
        public void prem_adjIDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.prem_adjID = expected;
            actual = target.prem_adjID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PendingTypeName
        ///</summary>
        [TestMethod()]
        public void PendingTypeNameTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); 
            string expected = null; 
            string actual;
            target.PendingTypeName = expected;
            actual = target.PendingTypeName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PendingLookup
        ///</summary>
        [TestMethod()]
        public void PendingLookupTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.PendingLookup;
            
        }

        /// <summary>
        ///A test for DrftInvoicenmr
        ///</summary>
        [TestMethod()]
        public void DrftInvoicenmrTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DrftInvoicenmr = expected;
            actual = target.DrftInvoicenmr;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DrftInvoiceDate
        ///</summary>
        [TestMethod()]
        public void DrftInvoiceDateTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.DrftInvoiceDate = expected;
            actual = target.DrftInvoiceDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CustomerName
        ///</summary>
        [TestMethod()]
        public void CustomerNameTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            AccountBE actual;
            actual = target.CustomerName;
            
        }

        /// <summary>
        ///A test for CustomerFullName
        ///</summary>
        [TestMethod()]
        public void CustomerFullNameTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CustomerFullName = expected;
            actual = target.CustomerFullName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for custmrID
        ///</summary>
        [TestMethod()]
        public void custmrIDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.custmrID = expected;
            actual = target.custmrID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
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
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Adjuststatus
        ///</summary>
        [TestMethod()]
        public void AdjuststatusTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Adjuststatus = expected;
            actual = target.Adjuststatus;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjPendingRsnID
        ///</summary>
        [TestMethod()]
        public void AdjPendingRsnIDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.AdjPendingRsnID = expected;
            actual = target.AdjPendingRsnID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjPendingIndctor
        ///</summary>
        [TestMethod()]
        public void AdjPendingIndctorTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.AdjPendingIndctor = expected;
            actual = target.AdjPendingIndctor;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjMgmtStatusNumber
        ///</summary>
        [TestMethod()]
        public void AdjMgmtStatusNumberTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.AdjMgmtStatusNumber = expected;
            actual = target.AdjMgmtStatusNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_STATUS_TYP_ID
        ///</summary>
        [TestMethod()]
        public void ADJ_STATUS_TYP_IDTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJ_STATUS_TYP_ID = expected;
            actual = target.ADJ_STATUS_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustmentManagementBE Constructor
        ///</summary>
        [TestMethod()]
        public void AdjustmentManagementBEConstructorTest()
        {
            AdjustmentManagementBE target = new AdjustmentManagementBE();
            
        }
    }
}
