using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for Adj_Parameter_SetupCHFBSTest and is intended
    ///to contain all Adj_Parameter_SetupCHFBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Adj_Parameter_SetupBSCHFTest
    {


        private TestContext testContextInstance;
        static AdjustmentParameterSetupBE AdjCHFBE;
        static Adj_Parameter_SetupBS target;

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
            target = new Adj_Parameter_SetupBS();
            AddDataCHF();
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
        private static void AddDataCHF()
        {
            AdjCHFBE = new AdjustmentParameterSetupBE();
            AdjCHFBE.prem_adj_pgm_id = 9;
            AdjCHFBE.Cstmr_Id = 2;
            AdjCHFBE.depst_amt = 999;
            AdjCHFBE.clm_hndl_fee_basis_id = 299;
            AdjCHFBE.incld_ernd_retro_prem_ind = false;
            AdjCHFBE.actv_ind = true;
            AdjCHFBE.AdjparameterTypeID = 398;
            AdjCHFBE.CREATE_DATE = System.DateTime.Now;
            AdjCHFBE.CREATE_USER_ID = 1;
            target.Update(AdjCHFBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestCHFWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterSetupBE AdjprmCHFBE = new AdjustmentParameterSetupBE();
            AdjprmCHFBE.prem_adj_pgm_id = 6;
            AdjprmCHFBE.Cstmr_Id = 1;
            AdjprmCHFBE.depst_amt = 888;
            AdjprmCHFBE.clm_hndl_fee_basis_id = 300;
            AdjprmCHFBE.incld_ernd_retro_prem_ind = true;
            AdjprmCHFBE.actv_ind = true;
            AdjprmCHFBE.AdjparameterTypeID = 398;
            AdjprmCHFBE.CREATE_DATE = System.DateTime.Now;
            AdjprmCHFBE.CREATE_USER_ID = 1;
            actual = target.Update(AdjprmCHFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestCHFWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmCHFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmCHFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestCHFwithData()
        {
            bool expected = true;
            bool actual;
            AdjCHFBE.prem_adj_pgm_id = 9;
            AdjCHFBE.Cstmr_Id = 2;
            AdjCHFBE.depst_amt = 777;
            AdjCHFBE.clm_hndl_fee_basis_id = 300;
            AdjCHFBE.incld_ernd_retro_prem_ind = true;
            AdjCHFBE.actv_ind = true;
            AdjCHFBE.AdjparameterTypeID = 398;
            AdjCHFBE.UPDATE_DATE = System.DateTime.Now;
            AdjCHFBE.UPDATE_USER_ID = 10;
            actual = target.Update(AdjCHFBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestCHFWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmCHFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmCHFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getAdjParamtr with Null Data
        ///</summary>
        [TestMethod()]
        public void getAdjCHFNullParamtrTest()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            int ProgramPeriodID = 0;
            int CstmrID = 0;
            IList<AdjustmentParameterSetupBE> expected = null;
            IList<AdjustmentParameterSetupBE> actual;
            actual = target.getAdjParamtr(ProgramPeriodID, CstmrID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getAdjParamtr with real Data
        ///</summary>
        [TestMethod()]
        public void getAdjCHFParamtrTest()
        {
            int expected = 0;
            int ProgramPeriodID = 9;
            int CstmrID = 2;
            IList<AdjustmentParameterSetupBE> actual;
            actual = target.getAdjParamtr(ProgramPeriodID, CstmrID);
            Assert.AreNotEqual(expected, actual.Count);

        }
        /// <summary>
        ///A test for getAdjParamRow with real data
        ///</summary>
        [TestMethod()]
        public void getAdjParamRowTest()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            int adjPrmsetupID = 0;
            AdjustmentParameterSetupBE expected = null;
            AdjustmentParameterSetupBE actual = new AdjustmentParameterSetupBE();
            actual = target.getAdjParamRow(adjPrmsetupID);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for Adj_Parameter_SetupBS Constructor
        ///</summary>
        [TestMethod()]
        public void Adj_Parameter_SetupBSCHFConstructorTest()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            //
        }

    }
}
