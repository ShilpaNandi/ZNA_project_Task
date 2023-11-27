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
    public class Adj_paramet_DtlLCFBSTest
    {

        private TestContext testContextInstance;
        static AdjustmentParameterDetailBE AdjDtlLCFBE;
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
            AddDtlDataLCF();
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
        private static void AddDtlDataLCF()
        {
            AdjDtlLCFBE = new AdjustmentParameterDetailBE();
            AdjDtlLCFBE.PrgmPerodID = 9;
            AdjDtlLCFBE.AccountID = 2;
            AdjDtlLCFBE.st_id = 1;
            AdjDtlLCFBE.adj_paramet_id = 39;
            AdjDtlLCFBE.adj_fctr_rt = decimal.Parse("1.08");
            AdjDtlLCFBE.ln_of_bsn_id = 88;
            AdjDtlLCFBE.act_ind = true;
            AdjDtlLCFBE.CRTE_DATE = System.DateTime.Now;
            AdjDtlLCFBE.CRTE_USER_ID = 1;
            target.Update(AdjDtlLCFBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestLCFDtlWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterDetailBE AdjprmDtlLCFBE = new AdjustmentParameterDetailBE();
            AdjprmDtlLCFBE.PrgmPerodID = 6;
            AdjprmDtlLCFBE.AccountID = 1;
            AdjprmDtlLCFBE.st_id = 7;
            AdjprmDtlLCFBE.adj_paramet_id = 40;
            AdjprmDtlLCFBE.adj_fctr_rt = decimal.Parse("4.00");
            AdjprmDtlLCFBE.ln_of_bsn_id = 87;
            AdjprmDtlLCFBE.act_ind = true;
            AdjprmDtlLCFBE.CRTE_DATE = System.DateTime.Now;
            AdjprmDtlLCFBE.CRTE_USER_ID = 1;
            actual = target.Update(AdjprmDtlLCFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestLCFDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlLCFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlLCFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestLCFDtlswithData()
        {
            bool expected = true;
            bool actual;
            AdjDtlLCFBE.PrgmPerodID = 9;
            AdjDtlLCFBE.AccountID = 2;
            AdjDtlLCFBE.st_id = 7;
            AdjDtlLCFBE.adj_paramet_id = 39;
            AdjDtlLCFBE.adj_fctr_rt = decimal.Parse("2.22");
            AdjDtlLCFBE.ln_of_bsn_id = 92;
            AdjDtlLCFBE.UPDTE_DATE = System.DateTime.Now;
            AdjDtlLCFBE.UPDTE_USER_ID = 10;
            actual = target.Update(AdjDtlLCFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestLCFDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlLCFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlLCFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getLCFAdjParamtrDtls with Null Data
        ///</summary>
        [TestMethod()]
        public void getLCFAdjParamtrDtlsNullTest()
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
        ///A test for getLCFAdjParamtrDtls with Real Data
        ///</summary>
        [TestMethod()]
        public void getLCFAdjParamtrDtlsTest()
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
        public void getAdjLCFParamDtlRowTest()
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
        public void Adj_paramet_DtlLCFBSConstructorTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            //
        }
    }
}
