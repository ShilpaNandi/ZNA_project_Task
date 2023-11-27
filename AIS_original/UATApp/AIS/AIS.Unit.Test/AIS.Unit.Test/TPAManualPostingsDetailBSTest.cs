using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for TPAManualPostingsDetailBSTest and is intended
    ///to contain all TPAManualPostingsDetailBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TPAManualPostingsDetailBSTest
    {

        static PersonBE PerBE;
        static BrokerBE brokerBE;
        static TPAManualPostingsBE TPABE;
        static PostingTransactionTypeBE PostBE;
        static AccountBE AcctBE;
        static TPAManualPostingsDetailBE TPADtlBE;
        static TPAManualPostingsBS TPABS;
        static TPAManualPostingsDetailBS target;
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
            target = new TPAManualPostingsDetailBS();
            AddCustomerData();
            AddBrokerData();
            AddPersonData();
            AddTPAData();
            AddPostingData();
            AddTPADtlData();
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
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNull()
        {

            TPAManualPostingsDetailBE TPADtlBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(TPADtlBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDtlRow
        ///</summary>
        [TestMethod()]
        public void getTPAPostDtlRowTestWithNull()
        {

            int TpaPostDtlID = 0; // TODO: Initialize to an appropriate value
            TPAManualPostingsDetailBE expected = null; // TODO: Initialize to an appropriate value
            TPAManualPostingsDetailBE actual;
            actual = target.getTPAPostDtlRow(TpaPostDtlID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDltsList
        ///</summary>
        [TestMethod()]
        public void getTPAPostDltsListTestWithNull()
        {

            IList<TPAManualPostingsDetailBE> expected = null; // TODO: Initialize to an appropriate value
            IList<TPAManualPostingsDetailBE> actual;
            actual = target.getTPAPostDltsList(0);
            if (actual.Count == 0)
            {
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
            AcctBE.FULL_NM = "aaa" + System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            AccountBS AcctBS = new AccountBS();
            AcctBS.Save(AcctBE);
        }
        /// <summary>
        /// a method for adding New Record in EXTRNL_ORG Table when the classis initiated 
        /// </summary>
        private static void AddBrokerData()
        {
            brokerBE = new BrokerBE();
            brokerBE.FULL_NAME = "Broker Test";
            brokerBE.CONTACT_TYPE_ID = 233;
            brokerBE.CREATE_DATE = System.DateTime.Now;
            brokerBE.CREATE_USER_ID = 1;

            BrokerBS brokerBS = new BrokerBS();
            brokerBS.Update(brokerBE);
        }
        /// <summary>
        /// a method for adding New Record in Person Table when the classis initiated 
        /// </summary>
        private static void AddPersonData()
        {
            PerBE = new PersonBE();
            PerBE.FORENAME = "aaa";
            PerBE.SURNAME = "bbb";
            PerBE.EXTERNAL_ORGN_ID = brokerBE.EXTRNL_ORG_ID;
            PerBE.CREATEDDATE = System.DateTime.Now;
            PerBE.CREATEDUSER_ID = 1;
            PersonBS perBS = new PersonBS();
            perBS.Update(PerBE);
        }
        /// <summary>
        /// a method for adding New Record in THRD_PTY_ADMIN_MNL_INVC table when the classis initiated
        /// </summary>
        private static void AddTPAData()
        {
            TPABE = new TPAManualPostingsBE();
            TPABE.CustomerID = AcctBE.CUSTMR_ID;
            TPABE.ThirdPartyAdminID = brokerBE.EXTRNL_ORG_ID;
            TPABE.CreatedDate = System.DateTime.Now;
            TPABE.CreatedUserID = 1;
            TPABS.Update(TPABE);
        }
        /// <summary>
        /// a method for adding New Record in POST_TRNS_TYP table when the classis initiated
        /// </summary>
        private static void AddPostingData()
        {
            PostBE = new PostingTransactionTypeBE();
            PostBE.Created_Date = System.DateTime.Now;
            PostBE.Created_UserID = 1;
            PostingTransactionTypeBS postBS = new PostingTransactionTypeBS();
            postBS.SaveSetupData(PostBE);
        }
        /// <summary>
        /// a method for adding New Record in THRD_PTY_ADMIN_MNL_INVC table when the classis initiated
        /// </summary>
        private static void AddTPADtlData()
        {
            TPADtlBE = new TPAManualPostingsDetailBE();
            TPADtlBE.ThirdPartyAdminManualInvoiceID = TPABE.ThirdPartyAdminManualInvoiceID;
            TPADtlBE.CustomerID = AcctBE.CUSTMR_ID;
            TPADtlBE.PostingTrnsTypID =PostBE.POST_TRANS_TYP_ID;
            TPADtlBE.CreatedDate = System.DateTime.Now;
            TPADtlBE.CreatedUserID = 1;
            target.Update(TPADtlBE);
        }
        /// <summary>
        ///A test for adding Record in  THRD_PTY_ADMIN_MNL_INVC_DTL Table 
        /// </summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual;
            TPADtlBE = new TPAManualPostingsDetailBE();
            TPADtlBE.ThirdPartyAdminManualInvoiceID = TPABE.ThirdPartyAdminManualInvoiceID;
            TPADtlBE.CustomerID = AcctBE.CUSTMR_ID;
            TPADtlBE.PostingTrnsTypID = PostBE.POST_TRANS_TYP_ID; ;
            TPADtlBE.CreatedDate = System.DateTime.Now;
            TPADtlBE.CreatedUserID = 1;
            actual = target.Update(TPADtlBE);
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
                     
            TPADtlBE.UpdatedDate = System.DateTime.Now;
            TPADtlBE.UpdatedUserID = 1;
           actual= target.Update(TPADtlBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDtlRow with Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostDtlRowTestWithData()
        {
            TPAManualPostingsDetailBE expected = null; // TODO: Initialize to an appropriate value
            TPAManualPostingsDetailBE actual;
            actual = target.getTPAPostDtlRow(TPADtlBE.ThirdPartyAdminManualInvoiceDtlID);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTPAPostDltsList With Data
        ///</summary>
        [TestMethod()]
        public void getTPAPostDltsListTestWithData()
        {

            IList<TPAManualPostingsDetailBE> expected = null; // TODO: Initialize to an appropriate value
            IList<TPAManualPostingsDetailBE> actual;
            actual = target.getTPAPostDltsList(TPABE.ThirdPartyAdminManualInvoiceID);
            if (actual.Count == 0) actual = null;
            Assert.AreNotEqual(expected, actual);
        }



    }
}
