using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Sub_Audt_PremBSTest and is intended
    ///to contain all Sub_Audt_PremBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Sub_Audt_PremBSTest
    {

                      
        private TestContext testContextInstance;
        static AccountBE AcctBE;
        static ProgramPeriodBE ProgPerdBE;
        static Coml_AgmtBE CommlAgtBE;
        static Coml_Agmt_AuDtBE ComAgrAudBE;
        static SubjectAuditPremiumBE subAuditPremBE;
        static Sub_Audt_PremBS target;

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
            target = new Sub_Audt_PremBS();
            AddCustomerData();
            AddProgPerdData();
            AddCommAgrData();
            AddCommrAgrAudData();
            AddSubAudPremData();
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
        /// a method for adding New Record in Customer when the classis initiated 
        /// </summary>
        private static void AddCustomerData()
        {
            AcctBE = new AccountBE();
            AcctBE.FULL_NM = "aaa" + System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            (new AccountBS()).Save(AcctBE);
        }
        /// <summary>
        /// a method for adding New Record in Premium Adjustment program table when the classis initiated 
        /// </summary>
        private static void AddProgPerdData()
        {
            ProgPerdBE = new ProgramPeriodBE();
            ProgPerdBE.CUSTMR_ID = AcctBE.CUSTMR_ID;
            ProgPerdBE.CREATE_DATE = System.DateTime.Now;
            ProgPerdBE.CREATE_USER_ID = 1;
            (new ProgramPeriodsBS()).Save(ProgPerdBE);
        }
        /// <summary>
        /// a method for adding New Record in Commercial Agreement table when the classis initiated 
        /// </summary>
        private static void AddCommAgrData()
        {
            CommlAgtBE = new Coml_AgmtBE();
            CommlAgtBE.Prem_Adj_Prg_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            CommlAgtBE.Customer_ID = AcctBE.CUSTMR_ID;
            CommlAgtBE.PlannedEndDate = System.DateTime.Now;
            CommlAgtBE.CreatedDate = System.DateTime.Now;
            CommlAgtBE.CreatedUserID = 1;
            (new Coml_AgmtBS()).Save(CommlAgtBE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddCommrAgrAudData()
        {

            ComAgrAudBE = new Coml_Agmt_AuDtBE();
            ComAgrAudBE.Comm_Agr_ID = CommlAgtBE.Comm_Agr_ID;
            ComAgrAudBE.Customer_ID = AcctBE.CUSTMR_ID;
            ComAgrAudBE.Prem_Adj_Prg_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            ComAgrAudBE.StartDate = System.DateTime.Now;
            ComAgrAudBE.CreatedDate = System.DateTime.Now;
            ComAgrAudBE.CreatedUser_ID = 1;
            (new Coml_Agmt_AuDtBS()).Update(ComAgrAudBE);
        }
        /// <summary>
        /// a method for adding New Record in subAudPrem Table when the classis initiated 
        /// </summary>
        private static void AddSubAudPremData()
        {
            subAuditPremBE = new SubjectAuditPremiumBE();
            subAuditPremBE.Coml_Agmt_Audt_ID = ComAgrAudBE.Comm_Agr_Audit_ID;
            subAuditPremBE.Coml_Agmt_ID = ComAgrAudBE.Comm_Agr_ID;
            subAuditPremBE.Prem_Adj_Pgm_ID = ComAgrAudBE.Prem_Adj_Prg_ID;
            subAuditPremBE.Custmr_ID = ComAgrAudBE.Customer_ID;
            subAuditPremBE.StateID = 1;
            subAuditPremBE.Prem_Amt = 1000;
            subAuditPremBE.CreatedDate = System.DateTime.Now;
            subAuditPremBE.CreatedUser_ID = 1;
            target.Update(subAuditPremBE);
        }
        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            subAuditPremBE.Coml_Agmt_Audt_ID = ComAgrAudBE.Comm_Agr_Audit_ID;
            subAuditPremBE.Coml_Agmt_ID = ComAgrAudBE.Comm_Agr_ID;
            subAuditPremBE.Prem_Adj_Pgm_ID = ComAgrAudBE.Prem_Adj_Prg_ID;
            subAuditPremBE.Custmr_ID = ComAgrAudBE.Customer_ID;
            subAuditPremBE.StateID = 1;
            subAuditPremBE.Prem_Amt = 1000;
            subAuditPremBE.CreatedDate = System.DateTime.Now;
            subAuditPremBE.CreatedUser_ID = 1;
            actual = target.Update(subAuditPremBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            target = new Sub_Audt_PremBS();
            SubjectAuditPremiumBE subAuditPremBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(subAuditPremBE);
           

        }

        /// <summary>
        /// A test for Update With Real Data
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            subAuditPremBE.Prem_Amt = 2000;
            subAuditPremBE.UpdatedDate = System.DateTime.Now;
            subAuditPremBE.UpdatedUser_ID = 1;
            actual = target.Update(subAuditPremBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNULL()
        {
            Sub_Audt_PremBS target = new Sub_Audt_PremBS();
            SubjectAuditPremiumBE subAuditPremBE = null; 
            bool expected = false;
            bool actual;
            actual = target.Update(subAuditPremBE);
           

        }
         
        /// <summary>
        ///A test for getsubPremAudRow with Data
        ///</summary>
        [TestMethod()]
        public void getsubPremAudRowTestWithData()
        {
           
            SubjectAuditPremiumBE expected = null;
            SubjectAuditPremiumBE actual;
            actual = target.getsubPremAudRow(subAuditPremBE.Sub_Prem_Aud_ID);
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for getsubPremAudRow
        ///</summary>
        [TestMethod()]
        public void getsubPremAudRowTestWithNULL()
        {
            int subPremAudID = 0;
            SubjectAuditPremiumBE expected = null;
            SubjectAuditPremiumBE actual;
            actual = target.getsubPremAudRow(subPremAudID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
          
        }
        /// <summary>
        ///A test for getsubPremAudList
        ///</summary>
        [TestMethod()]
        public void getsubPremAudListTestWithData()
        {
           
            IList<SubjectAuditPremiumBE> expected = null;
            IList<SubjectAuditPremiumBE> actual;
            actual = target.getsubPremAudList(subAuditPremBE.Coml_Agmt_Audt_ID);
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for getsubPremAudList
        ///</summary>
        [TestMethod()]
        public void getsubPremAudListTestNULL()
        {
            Sub_Audt_PremBS target = new Sub_Audt_PremBS();
            int commAgrID = 0;
            int expected = 0;
            IList<SubjectAuditPremiumBE> actual;
            actual = target.getsubPremAudList(commAgrID);
            Assert.AreEqual(expected, actual.Count);
          
        }

       
    }
}
