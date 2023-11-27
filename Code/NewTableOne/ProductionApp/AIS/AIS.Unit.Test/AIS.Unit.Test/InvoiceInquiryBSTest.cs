using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System.Data;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for InvoiceInquiryBSTest and is intended
    ///to contain all InvoiceInquiryBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvoiceInquiryBSTest
    {


        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static PremiumAdjustmentBE premAdjBE;
        static PremAdjustmentBS premAdjBS;
        static InvoiceInquiryBE invBE;
        static InvoiceInquiryBS target;
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
            //Customet Table
            accountBS = new AccountBS();
            AddAccountData();
            //Prem Adj Table
            premAdjBS = new PremAdjustmentBS();
            AddPremiumAdjustmentData();
            //Invoice Inquiry
            target = new InvoiceInquiryBS();

        }

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
        /// a method for adding New Record when the class is initiated Into Customer Table 
        /// To meet F.key in PremAdj Table 
        /// </summary>
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "venkat" + System.DateTime.Now.ToString();
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }
        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj   Table 
        /// To meet Invoice Inquiry records 
        /// </summary>
        private static void AddPremiumAdjustmentData()
        {
            premAdjBE = new PremiumAdjustmentBE();
            premAdjBE.CUSTOMERID = accountBE.CUSTMR_ID;
            premAdjBE.VALN_DT = System.DateTime.Now;
            premAdjBE.INVC_DUE_DT = System.DateTime.Now;
            premAdjBE.FNL_INVC_NBR_TXT = "RTI000000101";
            premAdjBE.CRTE_DT = System.DateTime.Now;
            premAdjBE.CRTE_USER_ID = 1;
            premAdjBS.Update(premAdjBE);

        }


        /// <summary>
        ///A test for getInvoiceDates
        ///</summary>
        [TestMethod()]
        public void getInvoiceDatesTest()
        {
            int expected = 0;
            IList<InvoiceInquiryBE> actual;
            actual = target.getInvoiceDates(accountBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual.Count);
        }


        /// <summary>
        ///A test for getInvoiceInquiryData
        ///</summary>
        [TestMethod()]
        public void getInvoiceInquiryDataTest()
        {
            int intProgramTypID = 0;
            string strValDate = string.Empty;
            string strInvoiceNumber = string.Empty;
            string strInvoiceDate = string.Empty;
            int intPersonID = 0;
            int intExtOrgID = 0;
            int intInternalOrgID = 0;
            int intAccountNumber = 0;
            int intcustmerID = 0;
            int expected = 0;
            if (accountBE.CUSTMR_ID != null)
            { 
            intcustmerID=int.Parse(accountBE.CUSTMR_ID.ToString());
            }
            DataTable actual;
            actual = target.getInvoiceInquiryData(intcustmerID, intProgramTypID, strValDate, strInvoiceNumber, strInvoiceDate, intPersonID, intExtOrgID, intInternalOrgID, intAccountNumber);
            Assert.AreNotEqual(expected, actual.Rows.Count);
        }
        /// <summary>
        ///A test for getInvoiceInquiryData by CustmerID
        ///</summary>
        [TestMethod()]
        public void getInvoiceInquiryDataTestWithCustmerID()
        {
            int intProgramTypID = 0;
            string strValDate = string.Empty;
            string strInvoiceNumber = string.Empty;
            string strInvoiceDate = string.Empty;
            int intPersonID = 0;
            int intExtOrgID = 0;
            int intInternalOrgID = 0;
            int intAccountNumber = 0;
            int intcustmerID = 0;
            int expected = 0;
            DataTable actual;
            if (accountBE.CUSTMR_ID != null)
            {
                intcustmerID = int.Parse(accountBE.CUSTMR_ID.ToString());
            }
            actual = target.getInvoiceInquiryData(intcustmerID, intProgramTypID, strValDate, strInvoiceNumber, strInvoiceDate, intPersonID, intExtOrgID, intInternalOrgID, intAccountNumber);
            Assert.AreNotEqual(expected, actual.Rows.Count);
        }
        /// <summary>
        ///A test for getInvoiceInquiryData by ValDate
        ///</summary>
        [TestMethod()]
        public void getInvoiceInquiryDataTestWithValDate()
        {
            int intProgramTypID = 0;
            string strValDate = string.Empty;
            string strInvoiceNumber = string.Empty;
            string strInvoiceDate = string.Empty;
            int intPersonID = 0;
            int intExtOrgID = 0;
            int intInternalOrgID = 0;
            int intAccountNumber = 0;
            int intcustmerID = 0;
            int expected = 0;
            intcustmerID = int.Parse(accountBE.CUSTMR_ID.ToString());
            
            if (premAdjBE.VALN_DT != null)
            {
                strValDate = premAdjBE.VALN_DT.ToString();
            }
            DataTable actual;
            actual = target.getInvoiceInquiryData(intcustmerID, intProgramTypID, strValDate, strInvoiceNumber, strInvoiceDate, intPersonID, intExtOrgID, intInternalOrgID, intAccountNumber);
            Assert.AreNotEqual(expected, actual.Rows.Count);
        }
        /// <summary>
        ///A test for getInvoiceDates
        ///</summary>
        [TestMethod()]
        public void getInvoiceDates()
        {

            int expected = 0;
            IList<InvoiceInquiryBE> actual;
            actual = target.getInvoiceDates(accountBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual.Count);
        }
        /// <summary>
        ///A test for getInvoiceInquiryData 
        ///</summary>
        [TestMethod()]
        public void getInvoiceInquiry()
        {
            
            int expected = 0;
            int intcustmerID = int.Parse(accountBE.CUSTMR_ID.ToString());
            IList<InvoiceInquiryBE> actual;
            actual = target.getInvoiceData(intcustmerID);
            Assert.AreNotEqual(expected, actual.Count);
        }
       
    }
}
