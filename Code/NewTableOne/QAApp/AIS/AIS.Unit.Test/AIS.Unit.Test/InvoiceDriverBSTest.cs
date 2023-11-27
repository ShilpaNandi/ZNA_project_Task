using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.Business.Entities;
using System.Configuration;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for InvoiceDriverBSTest and is intended
    ///to contain all InvoiceDriverBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvoiceDriverBSTest
    {


        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static PremiumAdjustmentBE premAdjBE;
        static PremAdjustmentBS premAdjBS;
        static InvoiceDriverBS target;

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
        /// a method for adding New Record when the class is initiated Into Customer Table 
        /// 
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
        /// 
        /// </summary>
        private static void AddPremiumAdjustmentData()
        {
            premAdjBE = new PremiumAdjustmentBE();
            premAdjBE.CUSTOMERID = accountBE.CUSTMR_ID;
            premAdjBE.VALN_DT = System.DateTime.Now;
            premAdjBE.CRTE_DT = System.DateTime.Now;
            premAdjBE.CRTE_USER_ID = 1;
            premAdjBS.Update(premAdjBE);

        }
        /// <summary>
        ///A test for UpdatePremAdjutmentFinalInvoiceData
        ///</summary>
        [TestMethod()]
        public void UpdatePremAdjutmentFinalInvoiceDataTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS(); 
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID;
            string strInvoiceNo = "RTI000000010"; 
            int intUserID = 1; 
            bool expected = true;
            bool actual;
            actual = target.UpdatePremAdjutmentFinalInvoiceData(objDC, intAdjNo, strInvoiceNo, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UpdatePremAdjutmentDraftInvoiceData
        ///</summary>
        [TestMethod()]
        public void UpdatePremAdjutmentDraftInvoiceDataTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS();
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID; ;
            string strInvoiceNo = "RTD00000010";
            int intUserID = 1; 
            bool expected = true; 
            bool actual;
            actual = target.UpdatePremAdjutmentDraftInvoiceData(objDC, intAdjNo, strInvoiceNo, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UpdateFinalZDWKeys
        ///</summary>
        [TestMethod()]
        public void UpdateFinalZDWInternalKeysTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS();
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID;
            string strZDWKey = "01E21A240"; 
            char cKeyType = 'I';
            int intUserID = 1; 
            bool expected = true; 
            bool actual;
            actual = target.UpdateFinalZDWKeys(objDC, intAdjNo, strZDWKey, cKeyType, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateFinalZDWKeys
        ///</summary>
        [TestMethod()]
        public void UpdateFinalZDWExternalKeysTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS();
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID;
            string strZDWKey = "01E21A240";
            char cKeyType = 'E';
            int intUserID = 1;
            bool expected = true;
            bool actual;
            actual = target.UpdateFinalZDWKeys(objDC, intAdjNo, strZDWKey, cKeyType, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateFinalZDWKeys
        ///</summary>
        [TestMethod()]
        public void UpdateFinalZDWCDWSKeysTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS();
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID;
            string strZDWKey = "01E21A240";
            char cKeyType = 'C';
            int intUserID = 1;
            bool expected = true;
            bool actual;
            actual = target.UpdateFinalZDWKeys(objDC, intAdjNo, strZDWKey, cKeyType, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateDraftZDWKeys
        ///</summary>
        [TestMethod()]
        public void UpdateDraftZDWInternalKeysTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS();
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID;
            string strZDWKey = "01E21A240"; 
            char cKeyType = 'I'; 
            int intUserID = 1;
            bool expected = true; 
            bool actual;
            actual = target.UpdateDraftZDWKeys(objDC, intAdjNo, strZDWKey, cKeyType, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateDraftZDWKeys
        ///</summary>
        [TestMethod()]
        public void UpdateDraftZDWExternalKeysTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS();
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID;
            string strZDWKey = "01E21A240";
            char cKeyType = 'E';
            int intUserID = 1;
            bool expected = true;
            bool actual;
            actual = target.UpdateDraftZDWKeys(objDC, intAdjNo, strZDWKey, cKeyType, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateDraftZDWKeys
        ///</summary>
        [TestMethod()]
        public void UpdateDraftZDWCDWSKeysTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS();
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID;
            string strZDWKey = "01E21A240";
            char cKeyType = 'C';
            int intUserID = 1;
            bool expected = true;
            bool actual;
            actual = target.UpdateDraftZDWKeys(objDC, intAdjNo, strZDWKey, cKeyType, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for InsertPremAdjutmentStatusData
        ///</summary>
        [TestMethod()]
        public void InsertPremAdjutmentStatusDataTest()
        {
            InvoiceDriverBS target = new InvoiceDriverBS(); 
            AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            PREM_ADJ_ST premAdjStsNew = new PREM_ADJ_ST()
            {
                prem_adj_id = premAdjBE.PREMIUM_ADJ_ID,
                custmr_id =accountBE.CUSTMR_ID,
                eff_dt = System.DateTime.Now,
                adj_sts_typ_id = 346,
                cmmnt_txt = "Final",
                crte_dt = System.DateTime.Now,
                crte_user_id = 1
            };
            objDC.PREM_ADJ_STs.InsertOnSubmit(premAdjStsNew);
            objDC.SubmitChanges();
            int intAdjNo = premAdjBE.PREMIUM_ADJ_ID; 
            int intInvoiceStatus = 348; 
            int intUserID = 1; 
            bool expected = true; 
            bool actual;
            string strComments = "Final";
            actual = target.InsertPremAdjutmentStatusData(objDC, intAdjNo, intInvoiceStatus,strComments, intUserID);
            objDC.SubmitChanges();
            Assert.AreEqual(expected, actual);
        }

    }
}
