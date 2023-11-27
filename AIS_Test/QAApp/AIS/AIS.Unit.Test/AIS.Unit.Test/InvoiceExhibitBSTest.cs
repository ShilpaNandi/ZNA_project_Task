using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for InvoiceExhibitBSTest and is intended
    ///to contain all InvoiceExhibitBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvoiceExhibitBSTest
    {

        static InvoiceExhibitBE invoiceExhibitBE;
        static InvoiceExhibitBS target;
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            target = new InvoiceExhibitBS();
            AddData();
        }
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
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddData()
        {
            invoiceExhibitBE = new InvoiceExhibitBE();
            invoiceExhibitBE.ATCH_CD = "INV1";
            invoiceExhibitBE.ATCH_NM = "Adjustment Invoice";
            invoiceExhibitBE.STS_IND = true;
            invoiceExhibitBE.SEQ_NBR = 1;
            invoiceExhibitBE.INTRNL_FLAG_IND = 'I';
            invoiceExhibitBE.CESAR_CD_IND = true;
            invoiceExhibitBE.CREATEDATE = System.DateTime.Now;
            invoiceExhibitBE.CREATEUSER = 1;
            target.Update(invoiceExhibitBE);
        }
        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false; ;
            InvoiceExhibitBE invoiceExhibitBE = new InvoiceExhibitBE();
            invoiceExhibitBE.ATCH_CD = "INV2";
            invoiceExhibitBE.ATCH_NM = "Adjustment Invoice1";
            invoiceExhibitBE.STS_IND = true;
            invoiceExhibitBE.SEQ_NBR = 2;
            invoiceExhibitBE.INTRNL_FLAG_IND = 'I';
            invoiceExhibitBE.CESAR_CD_IND = true;
            invoiceExhibitBE.CREATEDATE = System.DateTime.Now;
            invoiceExhibitBE.CREATEUSER = 1;
            actual = target.Update(invoiceExhibitBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            InvoiceExhibitBS target = new InvoiceExhibitBS();
            InvoiceExhibitBE invoiceExhibitBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(invoiceExhibitBE);
            Assert.AreEqual(expected, actual);

        }
       

        /// <summary>
        ///A test for Update with Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            bool expected = true;
            bool actual;
            invoiceExhibitBE.ATCH_CD = "INV3";
            invoiceExhibitBE.ATCH_NM = "Adjustment Invoice2";
            invoiceExhibitBE.STS_IND = true;
            invoiceExhibitBE.SEQ_NBR = 3;
            invoiceExhibitBE.INTRNL_FLAG_IND = 'I'; ;
            invoiceExhibitBE.CESAR_CD_IND = false;
            invoiceExhibitBE.UPDATEDDATE = System.DateTime.Now;
            invoiceExhibitBE.UPDATEDUSER = 1;
            actual = target.Update(invoiceExhibitBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            InvoiceExhibitBS target = new InvoiceExhibitBS();
            InvoiceExhibitBE invoiceExhibitBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(invoiceExhibitBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getInvoiceExhibitRowTest with Null
        ///</summary>
        [TestMethod()]
        public void getInvoiceExhibitRowTestWithNUll()
        {
            InvoiceExhibitBS target = new InvoiceExhibitBS();
            int ExhibitID = 0;
            InvoiceExhibitBE expected = null;
            InvoiceExhibitBE actual;
            actual = target.getInvoiceExhibitRow(ExhibitID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getInvoiceExhibitRow
        ///</summary>
        [TestMethod()]
        public void getInvoiceExhibitRowTest()
        {
            InvoiceExhibitBE expected = null;
            InvoiceExhibitBE actual;
            actual = target.getInvoiceExhibitRow(invoiceExhibitBE.INVC_EXHIBIT_SETUP_ID);
            Assert.AreNotEqual(expected, actual);
        }

        
    }
}
