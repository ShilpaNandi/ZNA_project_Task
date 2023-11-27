using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for Adj_Parameter_SetupLCFBSTest and is intended
    ///to contain all Adj_Parameter_SetupLCFBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Adj_Parameter_SetupBSLCFTest
    {


        private TestContext testContextInstance;
        static AdjustmentParameterSetupBE AdjLCFBE;
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
            AddDataLCF();
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
        private static void AddDataLCF()
        {
            AdjLCFBE = new AdjustmentParameterSetupBE();
            AdjLCFBE.prem_adj_pgm_id = 9;
            AdjLCFBE.Cstmr_Id = 2;
            AdjLCFBE.loss_convfact_aggamt = 1000;
            AdjLCFBE.loss_convfact_calimcap = 2326;
            AdjLCFBE.lay_lossconv_FactInsPay = 598;
            AdjLCFBE.lay_lossconv_znapayamt = 354;
            AdjLCFBE.actv_ind = true;
            AdjLCFBE.AdjparameterTypeID = 402;
            AdjLCFBE.CREATE_DATE = System.DateTime.Now;
            AdjLCFBE.CREATE_USER_ID = 1;
            target.Update(AdjLCFBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestLCFWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterSetupBE AdjprmLCFBE = new AdjustmentParameterSetupBE();
            AdjprmLCFBE.prem_adj_pgm_id = 6;
            AdjprmLCFBE.Cstmr_Id = 1;
            AdjprmLCFBE.loss_convfact_aggamt = 7000;
            AdjprmLCFBE.loss_convfact_calimcap = 7326;
            AdjprmLCFBE.lay_lossconv_FactInsPay = 798;
            AdjprmLCFBE.lay_lossconv_znapayamt = 754;
            AdjprmLCFBE.actv_ind = true;
            AdjprmLCFBE.AdjparameterTypeID = 402;
            AdjprmLCFBE.CREATE_DATE = System.DateTime.Now;
            AdjprmLCFBE.CREATE_USER_ID = 1;
            actual = target.Update(AdjprmLCFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestLCFWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmLCFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmLCFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestLCFwithData()
        {
            bool expected = true;
            bool actual;
            AdjLCFBE.prem_adj_pgm_id = 9;
            AdjLCFBE.Cstmr_Id = 2;
            AdjLCFBE.loss_convfact_aggamt = 547;
            AdjLCFBE.loss_convfact_calimcap = 32600;
            AdjLCFBE.lay_lossconv_FactInsPay = 7980;
            AdjLCFBE.lay_lossconv_znapayamt = 1540;
            AdjLCFBE.actv_ind = true;
            AdjLCFBE.AdjparameterTypeID = 402;
            AdjLCFBE.UPDATE_DATE = System.DateTime.Now;
            AdjLCFBE.UPDATE_USER_ID = 10;
            actual = target.Update(AdjLCFBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestLCFWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmLCFBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmLCFBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getAdjParamtr with Null Data
        ///</summary>
        [TestMethod()]
        public void getAdjLCFNullParamtrTest()
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
        public void getAdjLCFParamtrTest()
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
        public void Adj_Parameter_SetupBSLCFConstructorTest()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            //
        }

    }
}
