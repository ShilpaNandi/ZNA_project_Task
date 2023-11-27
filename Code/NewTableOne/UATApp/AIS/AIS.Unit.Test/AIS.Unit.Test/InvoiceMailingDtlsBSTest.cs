using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for InvoiceMailingDtlsBSTest and is intended
    ///to contain all InvoiceMailingDtlsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvoiceMailingDtlsBSTest
    {
        static InvoiceMailingDtlsBE invMailingDtlsBE;
        static InvoiceMailingDtlsBS target;
        static AccountBE accountBE;
        static AccountBS accountBS;
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
            accountBS = new AccountBS();
            AddAccountData();
            target = new InvoiceMailingDtlsBS();
            AddPremiumAdjData();
           
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
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "Sank" + System.DateTime.Now;
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }

        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddPremiumAdjData()
        {
            invMailingDtlsBE = new InvoiceMailingDtlsBE();
            invMailingDtlsBE.CUSTMER_ID = accountBE.CUSTMR_ID;
            invMailingDtlsBE.FINAL_INV_TXT = "RTI120000000005";
            invMailingDtlsBE.DRAFT_INV_TXT = "RTD120000000005";
            invMailingDtlsBE.VALUATION_DATE = System.DateTime.Parse("10/10/2006");
            invMailingDtlsBE.INV_DUE_DT = System.DateTime.Parse("10/10/2006");
            invMailingDtlsBE.VALUATION_DATE = System.DateTime.Now;
            invMailingDtlsBE.CREATEDATE = System.DateTime.Now;
            invMailingDtlsBE.CREATEUSERID = 1;
            target.Save(invMailingDtlsBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false; ;
            InvoiceMailingDtlsBE invMailingDtlsBE = new InvoiceMailingDtlsBE();
            invMailingDtlsBE.CUSTMER_ID = accountBE.CUSTMR_ID;
            invMailingDtlsBE.FINAL_INV_TXT = "RTI120000000005";
            invMailingDtlsBE.DRAFT_INV_TXT = "RTD120000000005";
            invMailingDtlsBE.VALUATION_DATE =System.DateTime.Parse("10/10/2006");
            invMailingDtlsBE.INV_DUE_DT = System.DateTime.Parse("10/10/2006");
            invMailingDtlsBE.CREATEDATE = System.DateTime.Now;
            invMailingDtlsBE.CREATEUSERID = 1;
            actual = target.Save(invMailingDtlsBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            bool expected = true;
            bool actual;
            invMailingDtlsBE.FINAL_EMAIL_DT = System.DateTime.Now;
            invMailingDtlsBE.FINAL_MAILED_UW_DT = System.DateTime.Now;
            invMailingDtlsBE.DRAFT_MAILED_UW_DT = System.DateTime.Now;
            invMailingDtlsBE.UPDATEDATE = System.DateTime.Now;
            invMailingDtlsBE.UPDATEUSERID = 1;
            actual = target.Update(invMailingDtlsBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            InvoiceMailingDtlsBS target = new InvoiceMailingDtlsBS();
            InvoiceMailingDtlsBE invMailingDtlsBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(invMailingDtlsBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getInvoiceMailingDtlsRow
        ///</summary>
        [TestMethod()]
        public void getInvoiceMailingDtlsRowTest()
        {
            InvoiceMailingDtlsBS target = new InvoiceMailingDtlsBS();
            int ID = invMailingDtlsBE.PREM_ADJ_ID; 
            InvoiceMailingDtlsBE expected = null; 
            InvoiceMailingDtlsBE actual;
            actual = target.getInvoiceMailingDtlsRow(ID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getInvMailingFinalData
        ///</summary>
        [TestMethod()]
        public void getInvMailingFinalDataTest()
        {
            InvoiceMailingDtlsBS target = new InvoiceMailingDtlsBS();
            int custmrID = accountBE.CUSTMR_ID; 
            string fnlinvnbr = "RTI120000000005";
            string valndtfrm ="10/10/2006";
            string valndtto = "10/10/2008";
            string invduedtfrm = "10/10/2006";
            string invduedtto = "10/10/2008"; 
            IList<InvoiceMailingDtlsBE> expected = null; 
            IList<InvoiceMailingDtlsBE> actual;
            actual = target.getInvMailingFinalData(custmrID, fnlinvnbr, valndtfrm, valndtto, invduedtfrm, invduedtto);
            Assert.AreNotEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for getInvMailingDraftData
        ///</summary>
        [TestMethod()]
        public void getInvMailingDraftDataTest()
        {
            InvoiceMailingDtlsBS target = new InvoiceMailingDtlsBS();
            int custmrID = accountBE.CUSTMR_ID; 
            string dftinvnbr = "RTD120000000005";
            string valndtfrm = "10/10/2006";
            string valndtto = "10/10/2008";
            string invduedtfrm = "10/10/2006";
            string invduedtto = "10/10/2008"; 
            IList<InvoiceMailingDtlsBE> expected = null; 
            IList<InvoiceMailingDtlsBE> actual;
            actual = target.getInvMailingDraftData(custmrID, dftinvnbr, valndtfrm, valndtto, invduedtfrm, invduedtto);
            Assert.AreNotEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for getInvMailingDataRow
        ///</summary>
        //[TestMethod()]
        //public void getInvMailingDataRowTest()
        //{
        //    InvoiceMailingDtlsBS target = new InvoiceMailingDtlsBS(); 
        //    int ID = invMailingDtlsBE.PREM_ADJ_ID;
        //    InvoiceMailingDtlsBE expected = null; 
        //    InvoiceMailingDtlsBE actual;
        //    actual = target.getInvMailingDataRow(ID);
        //    Assert.AreNotEqual(expected, actual);
           
        //}

      
    }
}
