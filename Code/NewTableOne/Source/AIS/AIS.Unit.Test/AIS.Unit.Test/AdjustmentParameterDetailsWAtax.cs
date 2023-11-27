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
    public class Adj_paramet_DtlTMBSTest
    {

        private TestContext testContextInstance;
        static AdjustmentParameterDetailBE AdjDtlTMBE;
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
            AddDtlDataTM();
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
        private static void AddDtlDataTM()
        {
            AdjDtlTMBE = new AdjustmentParameterDetailBE();
            AdjDtlTMBE.PrgmPerodID = 9;
            AdjDtlTMBE.AccountID = 2;
            AdjDtlTMBE.st_id = 6;
            AdjDtlTMBE.adj_paramet_id = 35;
            AdjDtlTMBE.adj_fctr_rt = decimal.Parse("1.08");
            AdjDtlTMBE.PremAssementAmt = decimal.Parse("5.21");
            AdjDtlTMBE.act_ind = true;
            AdjDtlTMBE.CRTE_DATE = System.DateTime.Now;
            AdjDtlTMBE.CRTE_USER_ID = 1;
            target.Update(AdjDtlTMBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestTMDtlWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterDetailBE AdjprmDtlTMBE = new AdjustmentParameterDetailBE();
            AdjprmDtlTMBE.PrgmPerodID = 6;
            AdjprmDtlTMBE.AccountID = 1;
            AdjprmDtlTMBE.st_id = 10;
            AdjprmDtlTMBE.adj_paramet_id = 36;
            AdjprmDtlTMBE.adj_fctr_rt = decimal.Parse("9.05");
            AdjprmDtlTMBE.PremAssementAmt = decimal.Parse("6.65");
            AdjprmDtlTMBE.act_ind = true;
            AdjprmDtlTMBE.CRTE_DATE = System.DateTime.Now;
            AdjprmDtlTMBE.CRTE_USER_ID = 1;
            actual = target.Update(AdjprmDtlTMBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestTMDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlTMBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlTMBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestTMDtlswithData()
        {
            bool expected = true;
            bool actual;
            AdjDtlTMBE.PrgmPerodID = 9;
            AdjDtlTMBE.AccountID = 2;
            AdjDtlTMBE.st_id = 7;
            AdjDtlTMBE.adj_paramet_id = 35;
            AdjDtlTMBE.adj_fctr_rt = decimal.Parse("8.24");
            AdjDtlTMBE.PremAssementAmt = decimal.Parse("0.54");
            AdjDtlTMBE.UPDTE_DATE = System.DateTime.Now;
            AdjDtlTMBE.UPDTE_USER_ID = 10;
            actual = target.Update(AdjDtlTMBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestTMDtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlTMBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlTMBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getTMAdjParamtrDtls with Null Data
        ///</summary>
        [TestMethod()]
        public void getTMAdjParamtrDtlsNullTest()
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
        ///A test for getTMAdjParamtrDtls with Real Data
        ///</summary>
        [TestMethod()]
        public void getTMAdjParamtrDtlsTest()
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
        public void getAdjTMParamDtlRowTest()
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
        public void Adj_paramet_DtlTMBSConstructorTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            //
        }
    }
}
