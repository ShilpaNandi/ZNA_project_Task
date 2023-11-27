using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Qtly_Cntrl_ChklistBETest and is intended
    ///to contain all Qtly_Cntrl_ChklistBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class Qtly_Cntrl_ChklistBETest
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
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
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
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QUALITYCONTROLTYPE_ID
        ///</summary>
        [TestMethod()]
        public void QUALITYCONTROLTYPE_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.QUALITYCONTROLTYPE_ID = expected;
            actual = target.QUALITYCONTROLTYPE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QualityControlChklst_ID
        ///</summary>
        [TestMethod()]
        public void QualityControlChklst_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.QualityControlChklst_ID = expected;
            actual = target.QualityControlChklst_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ProgramPeriodStDate
        ///</summary>
        [TestMethod()]
        public void ProgramPeriodStDateTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ProgramPeriodStDate = expected;
            actual = target.ProgramPeriodStDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ProgramPeriodEndDate
        ///</summary>
        [TestMethod()]
        public void ProgramPeriodEndDateTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.ProgramPeriodEndDate = expected;
            actual = target.ProgramPeriodEndDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremumAdj_Pgm_ID
        ///</summary>
        [TestMethod()]
        public void PremumAdj_Pgm_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PremumAdj_Pgm_ID = expected;
            actual = target.PremumAdj_Pgm_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMIUMADJUSTMENT_ID
        ///</summary>
        [TestMethod()]
        public void PREMIUMADJUSTMENT_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREMIUMADJUSTMENT_ID = expected;
            actual = target.PREMIUMADJUSTMENT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMIUMADJ_STATUS_ID
        ///</summary>
        [TestMethod()]
        public void PREMIUMADJ_STATUS_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREMIUMADJ_STATUS_ID = expected;
            actual = target.PREMIUMADJ_STATUS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREMIUMADJ_ARIES_CLR_ID
        ///</summary>
        [TestMethod()]
        public void PREMIUMADJ_ARIES_CLR_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.PREMIUMADJ_ARIES_CLR_ID = expected;
            actual = target.PREMIUMADJ_ARIES_CLR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LOOKUPID
        ///</summary>
        [TestMethod()]
        public void LOOKUPIDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.LOOKUPID = expected;
            actual = target.LOOKUPID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMER_ID
        ///</summary>
        [TestMethod()]
        public void CUSTOMER_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CUSTOMER_ID = expected;
            actual = target.CUSTOMER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUST_RELID
        ///</summary>
        [TestMethod()]
        public void CUST_RELIDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CUST_RELID = expected;
            actual = target.CUST_RELID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUser_ID
        ///</summary>
        [TestMethod()]
        public void CreatedUser_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
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
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ChkLstItems
        ///</summary>
        [TestMethod()]
        public void ChkLstItemsTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ChkLstItems = expected;
            actual = target.ChkLstItems;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CHKLISTNAME
        ///</summary>
        [TestMethod()]
        public void CHKLISTNAMETest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CHKLISTNAME = expected;
            actual = target.CHKLISTNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CHECKLISTITEM_ID
        ///</summary>
        [TestMethod()]
        public void CHECKLISTITEM_IDTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CHECKLISTITEM_ID = expected;
            actual = target.CHECKLISTITEM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjChkLstItems
        ///</summary>
        [TestMethod()]
        public void AdjChkLstItemsTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.AdjChkLstItems = expected;
            actual = target.AdjChkLstItems;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTIVE
        ///</summary>
        [TestMethod()]
        public void ACTIVETest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTIVE = expected;
            actual = target.ACTIVE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AccountName
        ///</summary>
        [TestMethod()]
        public void AccountNameTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.AccountName = expected;
            actual = target.AccountName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Qtly_Cntrl_ChklistBE Constructor
        ///</summary>
        [TestMethod()]
        public void Qtly_Cntrl_ChklistBEConstructorTest()
        {
            Qtly_Cntrl_ChklistBE target = new Qtly_Cntrl_ChklistBE();
            
        }
    }
}
