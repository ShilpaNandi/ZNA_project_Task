using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for ILRFFormulaBSTest and is intended
    ///to contain all ILRFFormulaBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ILRFFormulaBSTest
    {


        private TestContext testContextInstance;
        
        

        static AccountBE AcctBE;
        static ProgramPeriodBE ProgPerdBE;
        static PolicyBE polBE;

        static AdjustmentParameterSetupBE ILRFParamBE;
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
            try
            {

            
            polBE = new PolicyBE();
            polBE.PolicyID= ProgPerdBE.PREM_ADJ_PGM_ID;
            polBE.cstmrid = AcctBE.CUSTMR_ID;
            polBE.PlanEndDate = System.DateTime.Now;
            polBE.CREATE_DATE = System.DateTime.Now;
            polBE.CREATE_USER_ID = 1;
            (new PolicyBS()).Save(polBE);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        private static void AddILRF()
        {
            ILRFParamBE = new AdjustmentParameterSetupBE();
            Adj_Parameter_SetupBS AdjPrmStupBS = new Adj_Parameter_SetupBS();
            ILRFParamBE.AdjparameterTypeID = 400;
            ILRFParamBE.Cstmr_Id = AcctBE.CUSTMR_ID;
            ILRFParamBE.prem_adj_pgm_id = ProgPerdBE.PREM_ADJ_PGM_ID;
            ILRFParamBE.incur_los_reim_fund_initl_fund_amt = 100;
            ILRFParamBE.incur_los_reim_fund_unlim_agmt_lim_ind = true;
            ILRFParamBE.incur_los_reim_fund_aggr_lim_amt = 200;
            ILRFParamBE.incur_los_reim_fund_unlim_minimium_lim_ind = true;
            ILRFParamBE.incur_los_reim_fund_min_lim_amt = 300;
            ILRFParamBE.incur_los_reim_fund_invc_lsi_ind = true;
            ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id = 419;
            ILRFParamBE.CREATE_DATE = System.DateTime.Now;
            ILRFParamBE.CREATE_USER_ID = 1;
            ILRFParamBE.actv_ind = true;
            AdjPrmStupBS.Update(ILRFParamBE);

            AdjustmentParameterPolicyBE ILRFPrmPolBE = new AdjustmentParameterPolicyBE();
            Adj_Paramet_PolBS AdjPrmPolBS = new Adj_Paramet_PolBS();
            ILRFPrmPolBE.adj_paramet_setup_id = ILRFParamBE.adj_paramet_setup_id;
            ILRFPrmPolBE.custmrID = AcctBE.CUSTMR_ID;
            ILRFPrmPolBE.PrmadjPRgmID = ProgPerdBE.PREM_ADJ_PGM_ID;
            ILRFPrmPolBE.coml_agmt_id = polBE.PolicyID;
            ILRFPrmPolBE.CREATE_USER_ID = 1;
            ILRFPrmPolBE.CREATE_DATE = System.DateTime.Now;
            AdjPrmPolBS.Update(ILRFPrmPolBE);
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            AddCustomerData();
            AddProgPerdData();
            AddCommAgrData();
            AddILRF();
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
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            ILRFFormulaBS target = new ILRFFormulaBS(); // TODO: Initialize to an appropriate value
            ILRFFormulaBE iLRFFormulaBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(iLRFFormulaBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getILRFFormulas
        ///</summary>
        [TestMethod()]
        public void getILRFFormulasTest()
        {
            ILRFFormulaBS target = new ILRFFormulaBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            int customerID = 0; // TODO: Initialize to an appropriate value
            string iBNRLDF = string.Empty; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<ILRFFormulaBE> actual;
            actual = target.getILRFFormulas(programPeriodID, customerID, iBNRLDF);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getILRFFormulaRow
        ///</summary>
        [TestMethod()]
        public void getILRFFormulaRowTest()
        {
            ILRFFormulaBS target = new ILRFFormulaBS(); // TODO: Initialize to an appropriate value
            int iLRFFormulaSetupID = 0; // TODO: Initialize to an appropriate value
            ILRFFormulaBE expected = null; // TODO: Initialize to an appropriate value
            ILRFFormulaBE actual;
            actual = target.getILRFFormulaRow(iLRFFormulaSetupID);
            Assert.AreNotEqual(expected, actual);
        }


    }
}
