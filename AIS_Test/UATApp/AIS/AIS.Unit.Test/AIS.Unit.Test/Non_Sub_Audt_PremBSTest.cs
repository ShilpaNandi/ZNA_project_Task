using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for Non_Sub_Audt_PremBSTest and is intended
    ///to contain all Non_Sub_Audt_PremBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Non_Sub_Audt_PremBSTest
    {


        private TestContext testContextInstance;
        static AccountBE AcctBE;
        static ProgramPeriodBE ProgPerdBE;
        static Coml_AgmtBE CommlAgtBE;
        static Coml_Agmt_AuDtBE ComAgrAudBE;
        static NonSubjectAuditPremiumBE nonSubAuditPremBE;
        static Non_Sub_Audt_PremBS target;
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
            target = new Non_Sub_Audt_PremBS();
            AddCustomerData();
            AddProgPerdData();
            AddCommAgrData();
            AddCommrAgrAudData();
            AddNonSubAudPremData();
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
        /// a method for adding New Record in NonSubAudPrem Table when the classis initiated 
        /// </summary>

        private static void AddNonSubAudPremData()
        {
            nonSubAuditPremBE = new NonSubjectAuditPremiumBE();
            nonSubAuditPremBE.Coml_Agmt_Audt_ID = ComAgrAudBE.Comm_Agr_Audit_ID;
            nonSubAuditPremBE.Coml_Agmt_ID = ComAgrAudBE.Comm_Agr_ID;
            nonSubAuditPremBE.Prem_Adj_Pgm_ID = ComAgrAudBE.Prem_Adj_Prg_ID;
            nonSubAuditPremBE.Custmr_ID = ComAgrAudBE.Customer_ID;
            nonSubAuditPremBE.Nsa_Typ_ID = 1;
            nonSubAuditPremBE.Non_Subj_Audt_Prem_Amt = 1000;
            nonSubAuditPremBE.CreatedDate = System.DateTime.Now;
            nonSubAuditPremBE.CreatedUser_ID = 1;
            target.Update(nonSubAuditPremBE);

        }
        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            NonSubjectAuditPremiumBE nsapBE = new NonSubjectAuditPremiumBE();
            nsapBE.Coml_Agmt_Audt_ID = ComAgrAudBE.Comm_Agr_Audit_ID;
            nsapBE.Coml_Agmt_ID = ComAgrAudBE.Comm_Agr_ID;
            nsapBE.Prem_Adj_Pgm_ID = ComAgrAudBE.Prem_Adj_Prg_ID;
            nsapBE.Custmr_ID = ComAgrAudBE.Customer_ID;
            nsapBE.Nsa_Typ_ID = 1;
            nsapBE.Non_Subj_Audt_Prem_Amt = 1000;
            nsapBE.CreatedDate = System.DateTime.Now;
            nsapBE.CreatedUser_ID = 1;
            actual = target.Update(nsapBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            target = new Non_Sub_Audt_PremBS();
            NonSubjectAuditPremiumBE nonSubAuditPremBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(nonSubAuditPremBE);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update With Real Data
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            nonSubAuditPremBE.Non_Subj_Audt_Prem_Amt = 2000;
            nonSubAuditPremBE.UpdatedDate = System.DateTime.Now;
            nonSubAuditPremBE.UpdatedUser_ID = 1;
            actual = target.Update(nonSubAuditPremBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNULL()
        {
            Non_Sub_Audt_PremBS target = new Non_Sub_Audt_PremBS();
            NonSubjectAuditPremiumBE nonsubAuditPremBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(nonsubAuditPremBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getsubPremAudRow
        ///</summary>
        [TestMethod()]
        public void getsubPremAudRowTestWithData()
        {

            NonSubjectAuditPremiumBE expected = null;
            NonSubjectAuditPremiumBE actual;
            actual = target.getsubPremAudRow(nonSubAuditPremBE.N_Sub_Prem_Aud_ID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getsubPremAudRow
        ///</summary>
        [TestMethod()]
        public void getsubPremAudRowTestNULL()
        {
            int nonsubPremAudID = 0;
            NonSubjectAuditPremiumBE expected = null;
            NonSubjectAuditPremiumBE actual;
            actual = target.getsubPremAudRow(nonsubPremAudID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for getNonsubPremAudList
        ///</summary>
        [TestMethod()]
        public void getNonsubPremAudListTestWithData()
        {
            IList<NonSubjectAuditPremiumBE> expected = null;
            IList<NonSubjectAuditPremiumBE> actual;
            actual = target.getNonsubPremAudList(nonSubAuditPremBE.Coml_Agmt_ID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getNonsubPremAudList
        ///</summary>
        [TestMethod()]
        public void getNonsubPremAudListTestNULL()
        {
            int commAgrID = 0;
            int expected = 0;
            IList<NonSubjectAuditPremiumBE> actual;
            actual = target.getNonsubPremAudList(commAgrID);
            Assert.AreEqual(expected, actual.Count);

        }


    }
}
