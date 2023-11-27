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
    public class Adj_paramet_DtlCHFBSTest
    {

        private TestContext testContextInstance;
        static AdjustmentParameterDetailBE AdjDtlCHFBE;
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
            AddDtlDataCHF();
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
        private static void AddDtlDataCHF()
        {
            AdjDtlCHFBE = new AdjustmentParameterDetailBE();
            AdjDtlCHFBE.PrgmPerodID = 9;
            AdjDtlCHFBE.AccountID = 2;
            AdjDtlCHFBE.st_id = 12;
            AdjDtlCHFBE.adj_paramet_id = 39;
            AdjDtlCHFBE.clm_hndl_fee_los_typ_id = 108;
            AdjDtlCHFBE.CHF_CLMT_NUMBER = 20;
            AdjDtlCHFBE.Clm_hndlfee_clmrate = 99100; 
            AdjDtlCHFBE.act_ind = true;
            AdjDtlCHFBE.CRTE_DATE = System.DateTime.Now;
            AdjDtlCHFBE.CRTE_USER_ID = 1;
            target.Update(AdjDtlCHFBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestCHFDtlWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterDetailBE AdjprmDtlCHFBE = new AdjustmentParameterDetailBE();
            AdjprmDtlCHFBE.PrgmPerodID = 6;
            AdjprmDtlCHFBE.AccountID = 1;
            AdjprmDtlCHFBE.st_id = 8;
            AdjprmDtlCHFBE.adj_paramet_id = 40;
            AdjprmDtlCHFBE.clm_hndl_fee_los_typ_id = 102;
            AdjprmDtlCHFBE.CHF_CLMT_NUMBER = 2;
            AdjprmDtlCHFBE.Clm_hndlfee_clmrate = 241; 
            AdjprmDtlCHFBE.act_ind = true;
            AdjprmDtlCHFBE.CRTE_DATE = System.DateTime.Now;
            AdjprmDtlCHFBE.CRTE_USER_ID = 1;
            actual = target.Update(AdjprmDtlCHFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestCHFDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlCHFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlCHFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestCHFDtlswithData()
        {
            bool expected = true;
            bool actual;
            AdjDtlCHFBE.PrgmPerodID = 9;
            AdjDtlCHFBE.AccountID = 2;
            AdjDtlCHFBE.st_id = 7;
            AdjDtlCHFBE.adj_paramet_id = 39;
            AdjDtlCHFBE.clm_hndl_fee_los_typ_id = 110;
            AdjDtlCHFBE.CHF_CLMT_NUMBER = 5;
            AdjDtlCHFBE.Clm_hndlfee_clmrate = 487; 
            AdjDtlCHFBE.UPDTE_DATE = System.DateTime.Now;
            AdjDtlCHFBE.UPDTE_USER_ID = 10;
            actual = target.Update(AdjDtlCHFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestCHFDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlCHFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlCHFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getCHFAdjParamtrDtls with Null Data
        ///</summary>
        [TestMethod()]
        public void getCHFAdjParamtrDtlsNullTest()
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
        ///A test for getCHFAdjParamtrDtls with Real Data
        ///</summary>
        [TestMethod()]
        public void getCHFAdjParamtrDtlsTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            int expected = 0;
            int AdjParameterSetupID = 39;
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
        public void getAdjCHFParamDtlRowTest()
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
        public void Adj_paramet_DtlCHFBSConstructorTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            //
        }
    }
}
