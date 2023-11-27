using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CustomerDocumentBSTest and is intended
    ///to contain all CustomerDocumentBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerDocumentBSTest
    {


        private TestContext testContextInstance;
        static CustomerDocumentBE cdBE;
        static CustomerDocumentBS target;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static NonAisCustomerBE nonaisBE;
        static NonAisCustomerBS nonaisBS;
        static PersonBE perBE;
        static PersonBS perBS;
        static CustomerDocumentIssuesBE custmrissuesBE;
        static CustomerDocumentIssuesBS custmrissuesBS;
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
            AddCustomerData();
            nonaisBS = new NonAisCustomerBS();
            AddNonAisCustomerData();
            perBS = new PersonBS();
            AddPersonData();
            target = new CustomerDocumentBS();
            AddDocumentsData();
            custmrissuesBS = new CustomerDocumentIssuesBS();
            AddIssuesData();
           // target = new CustomerDocumentBS();
           // AddCommonData();

        }

        private static void AddPersonData()
        {
            perBE = new PersonBE();
            perBE.FORENAME = "Praveen" + System.DateTime.Now.ToString();
            perBE.SURNAME = "Konduru" + System.DateTime.Now.ToString();
            perBE.CREATEDDATE = System.DateTime.Now;
            perBE.CREATEDUSER_ID = 1;
            perBS.Update(perBE);
        }
        public static void AddCustomerData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "Praveen" + System.DateTime.Now.ToString();
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }
        public static void AddNonAisCustomerData()
        {
            nonaisBE = new NonAisCustomerBE();
            nonaisBE.fullname = "Praveen" + System.DateTime.Now.ToString();
            nonaisBE.Creteduserid = 1;
            nonaisBE.Createddate = System.DateTime.Now;
            nonaisBS.Save(nonaisBE);
        }
        public static void AddIssuesData()
        {
            custmrissuesBE = new CustomerDocumentIssuesBE();
            custmrissuesBE.custmr_doc_id = cdBE.CUSTOMER_DOCUMENT_ID;
            custmrissuesBE.tracking_issue_id = 239;
            custmrissuesBE.createduserid = 1;
            custmrissuesBS.Save(custmrissuesBE);

        }
        public static void AddDocumentsData()
        {
            cdBE = new CustomerDocumentBE();
            cdBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            cdBE.FORM_ID = 264;
            cdBE.RECEVD_DATE = System.DateTime.Now;
            cdBE.PROGM_EFF_DATE = System.DateTime.Now;
            cdBE.PROG_EXP_DATE = System.DateTime.Now;
            cdBE.VALUATION_DATE = System.DateTime.Now;
            cdBE.QUALITY_CNTRL_DATE = System.DateTime.Now;
            cdBE.RETRO_ADJ_AMOUNT = 1000;
            cdBE.RESPONSIBLE_PERS_ID = perBE.PERSON_ID;
            cdBE.cash_flw_spl_id = perBE.PERSON_ID;
            cdBE.Non_Ais_id = nonaisBE.Nonaiscustmrid;
            cdBE.CREATED_DATE = System.DateTime.Now;
            cdBE.CREATED_USR_ID = 1;
            target.save(cdBE);
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

        private static void AddCommonData()
        {
            cdBE = new CustomerDocumentBE();
            cdBE.CUSTMR_ID = accountBE.CUSTMR_ID; 
            cdBE.FORM_ID = 264;
            cdBE.RECEVD_DATE = System.DateTime.Now;
            cdBE.PROGM_EFF_DATE = System.DateTime.Now;
            cdBE.PROG_EXP_DATE = System.DateTime.Now;
            cdBE.VALUATION_DATE = System.DateTime.Now;
            cdBE.QUALITY_CNTRL_DATE = System.DateTime.Now;
            cdBE.RETRO_ADJ_AMOUNT = 1000;
            cdBE.RESPONSIBLE_PERS_ID = perBE.PERSON_ID;
            cdBE.cash_flw_spl_id = perBE.PERSON_ID;
            cdBE.Non_Ais_id = nonaisBE.Nonaiscustmrid;
            cdBE.CREATED_DATE = System.DateTime.Now;
            cdBE.CREATED_USR_ID = 1;
            target.save(cdBE);
        }
        [TestMethod()]
        public void SaveTestWithData()
        {
            bool expected = true;
            bool actual = false;
            CustomerDocumentBE cdocBE = new CustomerDocumentBE();
            cdocBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            cdocBE.FORM_ID = 264;
            cdocBE.RECEVD_DATE = System.DateTime.Now;
            cdocBE.PROGM_EFF_DATE = System.DateTime.Now;
            cdocBE.PROG_EXP_DATE = System.DateTime.Now;
            cdocBE.VALUATION_DATE = System.DateTime.Now;
            cdocBE.QUALITY_CNTRL_DATE = System.DateTime.Now;
            cdocBE.RETRO_ADJ_AMOUNT = 1000;
            cdocBE.RESPONSIBLE_PERS_ID = perBE.PERSON_ID;
            cdocBE.cash_flw_spl_id = perBE.PERSON_ID;
            cdocBE.Non_Ais_id = nonaisBE.Nonaiscustmrid;
            cdocBE.CREATED_DATE = System.DateTime.Now;
            cdocBE.CREATED_USR_ID = 1;
            actual = target.save(cdocBE);
            Assert.AreEqual(expected, actual);

        }
        

        /// <summary>
        ///A test for save
        ///</summary>
        [TestMethod()]
        public void saveTestWithNULL()
        {
            target = new CustomerDocumentBS(); 
            CustomerDocumentBE customerdocBE = null; 
            bool expected = false; 
            bool actual;
            actual = target.save(customerdocBE);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void SaveTestWithParameter()
        {
            target = new CustomerDocumentBS();
            CustomerDocumentBE customerdocBE = null;
            bool expected = false;
            bool actual;
            ArrayList issues = new ArrayList();
            actual = target.save(customerdocBE,issues);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void UpdateTestWithParameter()
        {
            target = new CustomerDocumentBS();
            CustomerDocumentBE customerdocBE = null;
            bool expected = false;
            bool actual;
            ArrayList issues = new ArrayList();
            actual = target.update(customerdocBE, issues);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void UpdateTestwithData()
        {
            bool expected = true;
            bool actual;
            cdBE.RECEVD_DATE = System.DateTime.Now;
            cdBE.PROGM_EFF_DATE = System.DateTime.Now;
            cdBE.PROG_EXP_DATE = System.DateTime.Now;
            cdBE.VALUATION_DATE = System.DateTime.Now;
            cdBE.QUALITY_CNTRL_DATE = System.DateTime.Now;
            cdBE.RETRO_ADJ_AMOUNT = 1000;
            actual = target.Save(cdBE);
            Assert.AreEqual(expected, actual);
            
        }
        [TestMethod()]
        // This method is not required which is same as save wtih null.
        public void UpdateTestwithNull()
        {
            bool expected = false;
            bool actual;
            target = new CustomerDocumentBS();
            CustomerDocumentBE customerdocBE = null;
            actual = target.save(customerdocBE);
            Assert.AreEqual(expected, actual);
        
        }
        /// <summary>
        ///A test for getcustomerdocuments
        ///</summary>
        [TestMethod()]
        public void getcustomerdocumentsTestData()
        {
            IList<CustomerDocumentBE> expected = null; 
            IList<CustomerDocumentBE> actual;
            actual = target.getcustomerdocuments(cdBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getcustomerdocuments
        ///</summary>
        [TestMethod()]
        public void getcustomerdocumentsTest()
        {
          
            int AccountId = 0; 
            IList<CustomerDocumentBE> expected = null; 
            IList<CustomerDocumentBE> actual;
            actual = target.getcustomerdocuments(AccountId);
            if (actual.Count==0) actual = null;
            Assert.AreEqual(expected, actual);
           
        }
        /// <summary>
        ///A test for getcustomerdocuments
        ///</summary>
        [TestMethod()]
        public void getNonaiscustomerdocumentsTest()
        {

            int NonaisAccountId = 0;
            IList<CustomerDocumentBE> expected = null;
            IList<CustomerDocumentBE> actual;
            actual = target.getNonaiscustomerdocuments(NonaisAccountId);
            if (actual.Count == 0) actual = null;
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test to getCustomerDocid
        ///</summary>
        [TestMethod()]
        public void getCustomerDocidTestData()
        {
            CustomerDocumentBE expected = null; 
            CustomerDocumentBE actual;
            actual = target.getCustomerDocid(cdBE.CUSTOMER_DOCUMENT_ID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getCustomerDocid
        ///</summary>
        [TestMethod()]
        public void getCustomerDocidTest()
        {
            int CustdocId = 0; 
            CustomerDocumentBE expected = null; 
            CustomerDocumentBE actual;
            actual = target.getCustomerDocid(CustdocId);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
           
        }

       
    }
}
