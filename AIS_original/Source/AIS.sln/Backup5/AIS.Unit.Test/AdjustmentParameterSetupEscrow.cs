using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for Adj_Parameter_SetupESCBSTest and is intended
    ///to contain all Adj_Parameter_SetupESCBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Adj_Parameter_SetupBSESCTest
    {


        private TestContext testContextInstance;
        static AdjustmentParameterSetupBE AdjESCBE;
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
            AddDataESC();
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
        private static void AddDataESC()
        {
            AdjESCBE = new AdjustmentParameterSetupBE();
            AdjESCBE.prem_adj_pgm_id = 9;
            AdjESCBE.Cstmr_Id = 2;
            AdjESCBE.Escrow_Diveser = 12;
            AdjESCBE.Escrow_MnthsHeld = decimal.Parse("22.3");
            AdjESCBE.Escrow_PLMNumber = 24;
            AdjESCBE.Escrow_PrevAmt = decimal.Parse("1200");    
            AdjESCBE.actv_ind = true;
            AdjESCBE.AdjparameterTypeID = 399;
            AdjESCBE.CREATE_DATE = System.DateTime.Now;
            AdjESCBE.CREATE_USER_ID = 1;
            target.Update(AdjESCBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestESCWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterSetupBE AdjprmESCBE = new AdjustmentParameterSetupBE();
            AdjprmESCBE.prem_adj_pgm_id = 6;
            AdjprmESCBE.Cstmr_Id = 1;
            AdjprmESCBE.Escrow_Diveser = 10;
            AdjprmESCBE.Escrow_MnthsHeld = decimal.Parse("06.3");
            AdjprmESCBE.Escrow_PLMNumber = 48;
            AdjprmESCBE.Escrow_PrevAmt = decimal.Parse("2548");    
            AdjprmESCBE.actv_ind = true;
            AdjprmESCBE.AdjparameterTypeID = 399;
            AdjprmESCBE.CREATE_DATE = System.DateTime.Now;
            AdjprmESCBE.CREATE_USER_ID = 1;
            actual = target.Update(AdjprmESCBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestESCWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmESCBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmESCBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestESCwithData()
        {
            bool expected = true;
            bool actual;
            AdjESCBE.prem_adj_pgm_id = 9;
            AdjESCBE.Cstmr_Id = 2;
            AdjESCBE.Escrow_Diveser = 300;
            AdjESCBE.Escrow_MnthsHeld = decimal.Parse("36.9");
            AdjESCBE.Escrow_PLMNumber = 42;
            AdjESCBE.Escrow_PrevAmt = decimal.Parse("692");    
            AdjESCBE.actv_ind = true;
            AdjESCBE.AdjparameterTypeID = 399;
            AdjESCBE.UPDATE_DATE = System.DateTime.Now;
            AdjESCBE.UPDATE_USER_ID = 10;
            actual = target.Update(AdjESCBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestESCWithNULL()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            AdjustmentParameterSetupBE AdjprmESCBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmESCBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getAdjParamtr with Null Data
        ///</summary>
        [TestMethod()]
        public void getAdjESCNullParamtrTest()
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
        public void getAdjESCParamtrTest()
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
        public void Adj_Parameter_SetupBSESCConstructorTest()
        {
            Adj_Parameter_SetupBS target = new Adj_Parameter_SetupBS();
            //
        }

    }
}
