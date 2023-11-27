using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for TPAManualPostingsBSTest and is intended
    ///to contain all TPAManualPostingsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TPAManualPostingsBSTest
   { 
        static PersonBE PerBE;
        static BrokerBE brokerBE;
        static TPAManualPostingsBE TPABE;
        static PremiumAdjustmentBE premAdjBE;
        static AccountBE AcctBE;
        static TPAManualPostingsBS TPABS;
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
            TPABS = new TPAManualPostingsBS();
            AddCustomerData();
            AddBrokerData();
            AddPersonData();
            
            AddPremAdjData();
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
        ///A test for Update the record in THRD_PTY_ADMIN_MNL_INVC table with null values
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNull()
        {
            TPAManualPostingsBE TPABE1 = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = TPABS.Update(TPABE1);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDtlRow with Null Data
        ///</summary>
        [TestMethod()]
       // [ExpectedException(typeof(System.NullReferenceException))]
        public void getTPAPostDtlRowTestWithNull()
        {
            int TpaPostID = 0; // TODO: Initialize to an appropriate value
            TPAManualPostingsBE expected = null; // TODO: Initialize to an appropriate value
            TPAManualPostingsBE actual;
            actual = TPABS.getTPAPostRow(TpaPostID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDltsList with Null Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostDltsListTestWithNull()
        {
            TPAManualPostingsBE expected = null; // TODO: Initialize to an appropriate value
            TPAManualPostingsBE actual;
            int CustomerID = 0;
            actual = TPABS.getTPAPostList(CustomerID);
            if (actual.ThirdPartyAdminManualInvoiceID == 0)
            {
                actual = null;
            }
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostSearchResultList with Null Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostSearchResultListTestWithNull()
        {

            IList<TPAManualPostingsBE> expected = new List<TPAManualPostingsBE>();
            IList<TPAManualPostingsBE> actual = new List<TPAManualPostingsBE>();
            expected = null;
            actual = TPABS.getTPAPostSearchResultList(0, 0, "", 0, 0, "", "", "");
            if (actual.Count == 0)
            {
                actual = null;
            }
            else
            {
                if(actual[0].CustomerID >0)
                actual = null;
            }
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// a method for adding New Record in Customer when the classis initiated 
        /// </summary>
        private static void AddCustomerData()
        {
            AcctBE = new AccountBE();
            AcctBE.FULL_NM = "aaa"+System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            AccountBS AcctBS = new AccountBS();
            AcctBS.Save(AcctBE);
        }
        /// <summary>
        /// a method for adding New Record in Person Table when the classis initiated 
        /// </summary>
        private static void AddPersonData()
        {
            PerBE = new PersonBE();
            PerBE.FORENAME = "aaa";
            PerBE.SURNAME = "bbb";
            PerBE.EXTERNAL_ORGN_ID = brokerBE.CONTACT_TYPE_ID;
            PerBE.CREATEDDATE = System.DateTime.Now;
            PerBE.CREATEDUSER_ID = 1;

            PersonBS perBS = new PersonBS();
            perBS.Update(PerBE);
        }

        /// <summary>
        /// a method for adding New Record in EXTRNL_ORG Table when the classis initiated 
        /// </summary>
        private static void AddBrokerData()
        {
            brokerBE  = new BrokerBE();
            brokerBE.FULL_NAME = "Broker Test";
            brokerBE.CONTACT_TYPE_ID = 233;
            brokerBE.CREATE_DATE = System.DateTime.Now;
            brokerBE.CREATE_USER_ID = 1;

            BrokerBS brokerBS = new BrokerBS();
            brokerBS.Update(brokerBE);
        }

        /// <summary>
        /// a method for adding New Record in PREM_ADJ table when the class is initiated
        /// </summary>
        private static void AddPremAdjData()
        {
            premAdjBE = new PremiumAdjustmentBE();
            premAdjBE.CUSTOMERID = AcctBE.CUSTMR_ID;
            premAdjBE.INVC_NBR_TXT = "12345";
            premAdjBE.VALN_DT = System.DateTime.Now;
            premAdjBE.CRTE_DT = System.DateTime.Now;
            premAdjBE.CRTE_USER_ID = 1;
            PremAdjustmentBS premadjBS = new PremAdjustmentBS();
            premadjBS.Update(premAdjBE);
        }

        /// <summary>
        /// a method for adding New Record in THRD_PTY_ADMIN_MNL_INVC table when the classis initiated
        /// </summary>
        private static void AddData()
        {
            TPABE = new TPAManualPostingsBE();
            TPABE.CustomerID = AcctBE.CUSTMR_ID;
            TPABE.ThirdPartyAdminID = brokerBE.EXTRNL_ORG_ID;
            TPABE.CreatedDate = System.DateTime.Now;
            TPABE.CreatedUserID = 1;
            TPABE.InvoiceNumber = "12345";
            TPABE.BusinessUnitOfficeID = 1;
            TPABE.ThirdPartyAdminInvoiceTypID = 324;
            TPABS.Update(TPABE);
        }
        /// <summary>
        ///A test for adding Record in  THRD_PTY_ADMIN_MNL_INVC Table 
        /// </summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual;
            TPABE = new TPAManualPostingsBE();
            TPABE.CustomerID = AcctBE.CUSTMR_ID;
            TPABE.ThirdPartyAdminID = brokerBE.EXTRNL_ORG_ID;
            TPABE.CreatedDate = System.DateTime.Now;
            TPABE.UpdatedUserID = 1;
            actual = TPABS.Update(TPABE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for update the Record in  THRD_PTY_ADMIN_MNL_INVC Table 
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            
            TPABE.UpdatedDate = System.DateTime.Now;
            TPABE.UpdatedUserID = 1;
            actual = TPABS.Update(TPABE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDtlRow with Data
        ///</summary>
        [TestMethod()]
       // [ExpectedException(typeof(System.NullReferenceException))]
        public void getTPAPostDtlRowTestWithData()
        {
            TPAManualPostingsBE expected = null; // TODO: Initialize to an appropriate value
            TPAManualPostingsBE actual;
            actual = TPABS.getTPAPostRow(TPABE.ThirdPartyAdminManualInvoiceID);
            if (actual.ThirdPartyAdminManualInvoiceID == 0) actual = null;
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDltsList with Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostDltsListTestWithData()
        {
            TPAManualPostingsBE expected = null; // TODO: Initialize to an appropriate value
            TPAManualPostingsBE actual;
            actual = TPABS.getTPAPostList(AcctBE.CUSTMR_ID);
            if (actual.ThirdPartyAdminManualInvoiceID ==0) actual = null;
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostSearchResultList with Valid Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostSearchResultListTestWithData()
        {

            IList<TPAManualPostingsBE> expected = new List<TPAManualPostingsBE>();
            IList<TPAManualPostingsBE> actual = new List<TPAManualPostingsBE>();
            expected = null;
            int Busn_Unt_Off_Id = int.Parse(TPABE.BusinessUnitOfficeID.ToString());
            int InvTypId = int.Parse(TPABE.ThirdPartyAdminInvoiceTypID.ToString());
            actual = TPABS.getTPAPostSearchResultList(AcctBE.CUSTMR_ID, TPABE.ThirdPartyAdminID, TPABE.InvoiceNumber, Busn_Unt_Off_Id, InvTypId, TPABE.ValuationDate.ToString(), TPABE.InvoiceDate.ToString(), TPABE.InvoiceDate.ToString());
            if (actual.Count == 0)
                actual = null;
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostBEList with Null Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostBEListTestWithNull()
        {

            IList<TPAManualPostingsBE> expected = new List<TPAManualPostingsBE>();
            IList<TPAManualPostingsBE> actual = new List<TPAManualPostingsBE>();
            expected = null;
            actual = TPABS.getTPAPostBEList(0);
            if (actual.Count == 0)
            {
                actual = null;
            }
            else
            {
                if (actual[0].CustomerID > 0)
                    actual = null;
            }
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostBEList with Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostBEListTestWithData()
        {
            IList<TPAManualPostingsBE> expected = new List<TPAManualPostingsBE>();
            IList<TPAManualPostingsBE> actual = new List<TPAManualPostingsBE>();
            expected = null;
            actual = TPABS.getTPAPostBEList(AcctBE.CUSTMR_ID);
            if (actual.Count == 0)
            {
                actual = null;
            }
            else
            {
                if (actual[0].CustomerID > 0)
                    actual = null;
            }
            Assert.AreEqual(expected, actual);
        }
        

        

    }
}
