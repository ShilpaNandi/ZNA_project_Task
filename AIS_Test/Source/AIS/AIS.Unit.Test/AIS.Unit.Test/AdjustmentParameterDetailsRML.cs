using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for Adj_paramet_DtlBSTest and is intended
    ///to contain all Adj_paramet_DtlBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Adj_paramet_DtlRMLBSTest
    {

        private TestContext testContextInstance;
        static AdjustmentParameterDetailBE AdjDtlRMLBE;
        static Adj_paramet_DtlBS target;

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
            target = new Adj_paramet_DtlBS();
            AddDtlDataRML();
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
        private static void AddDtlDataRML()
        {
            AdjDtlRMLBE = new AdjustmentParameterDetailBE();
            AdjDtlRMLBE.PrgmPerodID = 9;
            AdjDtlRMLBE.AccountID = 2;
            AdjDtlRMLBE.st_id = 15;
            AdjDtlRMLBE.ln_of_bsn_id = 92;   
            AdjDtlRMLBE.adj_paramet_id = 35;
            AdjDtlRMLBE.adj_fctr_rt = decimal.Parse("5.989876");
            AdjDtlRMLBE.fnl_overrid_amt = 3567;
            AdjDtlRMLBE.cmmnt_txt = "Please check the Policy details"; 
            AdjDtlRMLBE.act_ind = true;
            AdjDtlRMLBE.CRTE_DATE = System.DateTime.Now;
            AdjDtlRMLBE.CRTE_USER_ID = 1;
            target.Update(AdjDtlRMLBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestRMLDtlWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterDetailBE AdjprmDtlRMLBE = new AdjustmentParameterDetailBE();
            AdjprmDtlRMLBE.PrgmPerodID = 6;
            AdjprmDtlRMLBE.AccountID = 1;
            AdjprmDtlRMLBE.st_id = 10;
            AdjprmDtlRMLBE.adj_paramet_id = 36;
            AdjprmDtlRMLBE.ln_of_bsn_id = 92;
            AdjprmDtlRMLBE.adj_fctr_rt = decimal.Parse("9.0548");
            AdjprmDtlRMLBE.fnl_overrid_amt = 487;
            AdjprmDtlRMLBE.cmmnt_txt = "Please check the Program prd details"; 
            AdjprmDtlRMLBE.act_ind = true;
            AdjprmDtlRMLBE.CRTE_DATE = System.DateTime.Now;
            AdjprmDtlRMLBE.CRTE_USER_ID = 1;
            actual = target.Update(AdjprmDtlRMLBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestRMLDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlRMLBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlRMLBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestRMLDtlswithData()
        {
            bool expected = true;
            bool actual;
            AdjDtlRMLBE.PrgmPerodID = 9;
            AdjDtlRMLBE.AccountID = 2;
            AdjDtlRMLBE.st_id = 20;
            AdjDtlRMLBE.adj_paramet_id = 35;
            AdjDtlRMLBE.ln_of_bsn_id = 92;
            AdjDtlRMLBE.adj_fctr_rt = decimal.Parse("8.2848");
            AdjDtlRMLBE.fnl_overrid_amt = 7812;
            AdjDtlRMLBE.cmmnt_txt = "PAss"; 
            AdjDtlRMLBE.UPDTE_DATE = System.DateTime.Now;
            AdjDtlRMLBE.UPDTE_USER_ID = 10;
            actual = target.Update(AdjDtlRMLBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestRMLDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlRMLBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlRMLBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getRMLAdjParamtrDtls with Null Data
        ///</summary>
        [TestMethod()]
        public void getRMLAdjParamtrDtlsNullTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            int ProgramPeriodID = 0;
            int AdjParameterSetupID = 0;
            int Actid = 0;
            IList<AdjustmentParameterDetailBE> expected = null;
            IList<AdjustmentParameterDetailBE> actual;
            actual = target.getLBAAdjParamtrDtls(ProgramPeriodID, AdjParameterSetupID, Actid);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for getRMLAdjParamtrDtls with Real Data
        ///</summary>
        [TestMethod()]
        public void getRMLAdjParamtrDtlsTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            int expected = 0;
            int AdjParameterSetupID = 35;
            int prgprmID = 9;
            int Actid = 2;
            IList<AdjustmentParameterDetailBE> actual;
            actual = target.getLBAAdjParamtrDtls(prgprmID, AdjParameterSetupID, Actid);
            Assert.AreNotEqual(expected, actual.Count);
            //
        }


        /// <summary>
        ///A test for getAdjParamDtlRow
        ///</summary>
        [TestMethod()]
        public void getAdjRMLParamDtlRowTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            int adjPrmDtlID = 0;
            AdjustmentParameterDetailBE expected = null;
            AdjustmentParameterDetailBE actual = new AdjustmentParameterDetailBE(); ;
            actual = target.getAdjParamDtlRow(adjPrmDtlID);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for Adj_paramet_DtlBS Constructor
        ///</summary>
        [TestMethod()]
        public void Adj_paramet_DtlRMLBSConstructorTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            //
        }
    }
}
