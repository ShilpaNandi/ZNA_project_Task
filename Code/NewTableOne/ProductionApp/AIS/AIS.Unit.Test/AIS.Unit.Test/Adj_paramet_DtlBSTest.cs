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
    public class Adj_paramet_DtlBSTest
    {

        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static AdjustmentParameterSetupBE AdjLBABE;
        static Adj_Parameter_SetupBS AdjLBABS;
        static AdjustmentParameterDetailBE AdjDtlLBABE;
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
            //Customet Table
            accountBS = new AccountBS();
            AddAccountData();
            //Prem Adj Program Table
            programPeriodsBS = new ProgramPeriodsBS();
            AddProgramPeriodData();
            //Parameter Table
            AdjLBABS = new Adj_Parameter_SetupBS();
            AddDataLBA();

            //Parameter Detail
            target = new Adj_paramet_DtlBS();
            AddDtlDataLBA();
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
        /// a method for adding New Record when the class is initiated Into Customer Table 
        /// To meet F.key in PremAdjNYSIF Table 
        /// </summary>
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "venkat" + System.DateTime.Now.ToString();
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }

        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj Program  Table 
        /// To meet F.key in PremAdjPeriod Table 
        /// </summary>
        private static void AddProgramPeriodData()
        {
            programPeriodBE = new ProgramPeriodBE();
            programPeriodBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            programPeriodBE.CREATE_DATE = System.DateTime.Now;
            programPeriodBE.CREATE_USER_ID = 1;
            programPeriodsBS.Update(programPeriodBE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddDataLBA()
        {
            AdjLBABE = new AdjustmentParameterSetupBE();
            AdjLBABE.prem_adj_pgm_id = programPeriodBE.PREM_ADJ_PGM_ID;
            AdjLBABE.Cstmr_Id = accountBE.CUSTMR_ID;
            AdjLBABE.incld_ernd_retro_prem_ind = true;
            AdjLBABE.incld_ibnr_ldf_ind = false;
            AdjLBABE.depst_amt = 369;
            AdjLBABE.lba_Adjustment_typ = 63;
            AdjLBABE.actv_ind = true;
            AdjLBABE.AdjparameterTypeID = 401;
            AdjLBABE.CREATE_DATE = System.DateTime.Now;
            AdjLBABE.CREATE_USER_ID = 1;
            AdjLBABS.Update(AdjLBABE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddDtlDataLBA()
        {
            AdjDtlLBABE = new AdjustmentParameterDetailBE();
            AdjDtlLBABE.PrgmPerodID = programPeriodBE.PREM_ADJ_PGM_ID;
            AdjDtlLBABE.AccountID = accountBE.CUSTMR_ID;
            AdjDtlLBABE.st_id = 1;
            AdjDtlLBABE.adj_paramet_id = AdjLBABE.adj_paramet_setup_id; 
            AdjDtlLBABE.adj_fctr_rt = decimal.Parse("9.98");
            AdjDtlLBABE.fnl_overrid_amt = 698;
            AdjDtlLBABE.cmmnt_txt = "Please Switch off your Monitor"; 
            AdjDtlLBABE.act_ind = true;
            AdjDtlLBABE.CRTE_DATE = System.DateTime.Now;
            AdjDtlLBABE.CRTE_USER_ID = 1;
            target.Update(AdjDtlLBABE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestLBADtlWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterDetailBE AdjprmDtlLBABE = new AdjustmentParameterDetailBE();
            AdjprmDtlLBABE.PrgmPerodID = programPeriodBE.PREM_ADJ_PGM_ID;
            AdjprmDtlLBABE.AccountID = accountBE.CUSTMR_ID;
            AdjprmDtlLBABE.st_id = 7;
            AdjprmDtlLBABE.adj_paramet_id = AdjLBABE.adj_paramet_setup_id; 
            AdjprmDtlLBABE.adj_fctr_rt = decimal.Parse("8.65");
            AdjprmDtlLBABE.fnl_overrid_amt = 985;
            AdjprmDtlLBABE.cmmnt_txt = "Invalid Insurance Policy";
            AdjprmDtlLBABE.act_ind = true;
            AdjprmDtlLBABE.CRTE_DATE = System.DateTime.Now;
            AdjprmDtlLBABE.CRTE_USER_ID = 1;
            actual = target.Update(AdjprmDtlLBABE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestLBADtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlLBABE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlLBABE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestLBADtlswithData()
        {
            bool expected = true;
            bool actual;
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlLBABE = target.getAdjParamDtlRow(AdjDtlLBABE.prem_adj_pgm_dtl_id);
            
            AdjDtlLBABE.adj_fctr_rt = decimal.Parse("3.65");
            AdjDtlLBABE.fnl_overrid_amt = 985;
            AdjDtlLBABE.cmmnt_txt = "Invalid Insurance Policy";
            AdjDtlLBABE.UPDTE_DATE = System.DateTime.Now;
            AdjDtlLBABE.UPDTE_USER_ID  = 1;
            actual = target.Update(AdjDtlLBABE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestLBADtlsWithNULL()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            AdjustmentParameterDetailBE AdjprmDtlLBABE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjprmDtlLBABE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getLBAAdjParamtrDtls with Null Data
        ///</summary>
        [TestMethod()]
        public void getLBAAdjParamtrDtlsNullTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            IList<AdjustmentParameterDetailBE> expected = null;
            IList<AdjustmentParameterDetailBE> actual;
            actual = target.getLBAAdjParamtrDtls(programPeriodBE.PREM_ADJ_PGM_ID, AdjLBABE.adj_paramet_setup_id, accountBE.CUSTMR_ID);
            Assert.AreEqual(expected, actual);
        }

        
        /// <summary>
        ///A test for getLBAAdjParamtrDtls with Real Data
        ///</summary>
        [TestMethod()]
        public void getLBAAdjParamtrDtlsTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS();
            int expected = 0;
            IList<AdjustmentParameterDetailBE> actual;
            actual = target.getLBAAdjParamtrDtls(programPeriodBE.PREM_ADJ_PGM_ID, AdjLBABE.adj_paramet_setup_id, accountBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual.Count);
            //
        }

       
        /// <summary>
        ///A test for getAdjParamDtlRow
        ///</summary>
        [TestMethod()]
        public void getAdjParamDtlRowTest()
        {
            Adj_paramet_DtlBS target = new Adj_paramet_DtlBS(); 
            AdjustmentParameterDetailBE expected = null; 
            AdjustmentParameterDetailBE actual = new AdjustmentParameterDetailBE(); ;
            actual = target.getAdjParamDtlRow(AdjDtlLBABE.prem_adj_pgm_dtl_id);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreNotEqual(expected, actual);
            
        }

       
    }
}
