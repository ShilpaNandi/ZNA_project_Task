using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for InvoiceExhibitBETest and is intended
    ///to contain all InvoiceExhibitBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvoiceExhibitBETest
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
        ///A test for UPDATEDUSER
        ///</summary>
        [TestMethod()]
        public void UPDATEDUSERTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATEDUSER = expected;
            actual = target.UPDATEDUSER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDDATE
        ///</summary>
        [TestMethod()]
        public void UPDATEDDATETest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDDATE = expected;
            actual = target.UPDATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STS_IND
        ///</summary>
        [TestMethod()]
        public void STS_INDTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.STS_IND = expected;
            actual = target.STS_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SEQ_NBR
        ///</summary>
        [TestMethod()]
        public void SEQ_NBRTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.SEQ_NBR = expected;
            actual = target.SEQ_NBR;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INVC_EXHIBIT_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void INVC_EXHIBIT_SETUP_IDTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.INVC_EXHIBIT_SETUP_ID = expected;
            actual = target.INVC_EXHIBIT_SETUP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for INTRNL_FLAG_IND
        ///</summary>
        [TestMethod()]
        public void INTRNL_FLAG_INDTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            Nullable<char> expected = new Nullable<char>(); // TODO: Initialize to an appropriate value
            Nullable<char> actual;
            target.INTRNL_FLAG_IND = expected;
            actual = target.INTRNL_FLAG_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEUSER
        ///</summary>
        [TestMethod()]
        public void CREATEUSERTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATEUSER = expected;
            actual = target.CREATEUSER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDATE
        ///</summary>
        [TestMethod()]
        public void CREATEDATETest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDATE = expected;
            actual = target.CREATEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CESAR_CD_IND
        ///</summary>
        [TestMethod()]
        public void CESAR_CD_INDTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.CESAR_CD_IND = expected;
            actual = target.CESAR_CD_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ATCH_NM
        ///</summary>
        [TestMethod()]
        public void ATCH_NMTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ATCH_NM = expected;
            actual = target.ATCH_NM;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ATCH_CD
        ///</summary>
        [TestMethod()]
        public void ATCH_CDTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ATCH_CD = expected;
            actual = target.ATCH_CD;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for InvoiceExhibitBE Constructor
        ///</summary>
        [TestMethod()]
        public void InvoiceExhibitBEConstructorTest()
        {
            InvoiceExhibitBE target = new InvoiceExhibitBE();
            
        }
    }
}
