using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for Coml_Agmt_AuDtBSTest and is intended
    ///to contain all Coml_Agmt_AuDtBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Coml_Agmt_AuDtBSTest
    {
        private TestContext testContextInstance;
        static AccountBE AcctBE;
        static ProgramPeriodBE ProgPerdBE;
        static Coml_AgmtBE CommlAgtBE;
        static Coml_Agmt_AuDtBE ComAgrAudBE;
        static Coml_Agmt_AuDtBS ComAgrAudBS;
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
            ComAgrAudBS = new Coml_Agmt_AuDtBS();
            AddCustomerData();
            AddProgPerdData();
            AddCommAgrData();
            AddCommrAgrAudData();
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
            ComAgrAudBS.Update(ComAgrAudBE);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNull()
        {
            Coml_Agmt_AuDtBS target = new Coml_Agmt_AuDtBS(); // TODO: Initialize to an appropriate value
            Coml_Agmt_AuDtBE commlBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(commlBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for getCommAgrRow
        ///</summary>
        [TestMethod()]
        public void getCommAgrRowTestWithNull()
        {
            Coml_Agmt_AuDtBS target = new Coml_Agmt_AuDtBS(); // TODO: Initialize to an appropriate value
            int CommAgrID = 0; // TODO: Initialize to an appropriate value
            Coml_Agmt_AuDtBE expected = null; // TODO: Initialize to an appropriate value
            Coml_Agmt_AuDtBE actual;
            actual = target.getCommAgrRow(CommAgrID);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for getCommAgrAuditList
        ///</summary>
        [TestMethod()]
        public void getCommAgrAuditListTestWithNull()
        {
            Coml_Agmt_AuDtBS target = new Coml_Agmt_AuDtBS(); // TODO: Initialize to an appropriate value
            int progPeriodID = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<Coml_Agmt_AuDtBE> actual;
            actual = target.getCommAgrAuditList(progPeriodID);
            Assert.AreEqual(expected, actual.Count);
        }
        /// <summary>
        ///A test for Coml_Agmt_AuDtBS Constructor
        ///</summary>
        [TestMethod()]
        public void Coml_Agmt_AuDtBSConstructorTestWithNull()
        {
            Coml_Agmt_AuDtBS target = new Coml_Agmt_AuDtBS();
        }
       
        /// <summary>
        /// Function to Add new Record in to Coml_Agmt_Audt Table
        /// </summary>
        [TestMethod]
        public void AddComlAgmtAuditTestWithData()
        {
            bool expected = true;
            bool actual;
            Coml_Agmt_AuDtBS target = new Coml_Agmt_AuDtBS();
            Coml_Agmt_AuDtBE ComAgrAudBE1 = new Coml_Agmt_AuDtBE();
            ComAgrAudBE1.Comm_Agr_ID = CommlAgtBE.Comm_Agr_ID;
            ComAgrAudBE1.Customer_ID = AcctBE.CUSTMR_ID;
            ComAgrAudBE1.Prem_Adj_Prg_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            ComAgrAudBE1.StartDate = System.DateTime.Now;
            ComAgrAudBE1.CreatedDate = System.DateTime.Now;
            ComAgrAudBE1.CreatedUser_ID = 1;
            actual = target.Update(ComAgrAudBE1);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// A Test to update Record in Coml_Agmt_Audt Table
        /// </summary>
        [TestMethod]
        public void UpdateComlAgmtAuditTestWithData()
        {
            bool expected = false;
            bool actual;
            ComAgrAudBE.UpdatedDate = System.DateTime.Now;
            ComAgrAudBE.UpdatedUser_ID = 1;
            actual = ComAgrAudBS.Update(ComAgrAudBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// A Test to get the row from Coml_agmt_audt Table based on ComlAgrAuditID
        /// </summary>
        [TestMethod]
        public void getCommAgrRowTestWithData()
        {
            Coml_Agmt_AuDtBE expected = null;
            Coml_Agmt_AuDtBE actual;
            actual = ComAgrAudBS.getCommAgrRow(ComAgrAudBE.Comm_Agr_ID);
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A Test to get the records from Coml_agmt_audt Table based on ProgramPeriodID
        ///</summary>
        [TestMethod()]
        public void getCommAgrAuditListTestWithData()
        {
            Coml_Agmt_AuDtBE expected = null;
            Coml_Agmt_AuDtBE actual;
            actual = ComAgrAudBS.getCommAgrRow(ComAgrAudBE.Prem_Adj_Prg_ID);
            Assert.AreNotEqual(expected, actual);
        }
    }
}
