using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjustmentParameterSetupBETest and is intended
    ///to contain all AdjustmentParameterSetupBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjustmentParameterSetupBETest
    {


        private TestContext testContextInstance;

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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATE_USER_ID = expected;
            actual = target.UPDATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATE_DATETest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for prem_adj_pgm_id
        ///</summary>
        [TestMethod()]
        public void prem_adj_pgm_idTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.prem_adj_pgm_id = expected;
            actual = target.prem_adj_pgm_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for loss_convfact_calimcap
        ///</summary>
        [TestMethod()]
        public void loss_convfact_calimcapTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.loss_convfact_calimcap = expected;
            actual = target.loss_convfact_calimcap;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for loss_convfact_aggamt
        ///</summary>
        [TestMethod()]
        public void loss_convfact_aggamtTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.loss_convfact_aggamt = expected;
            actual = target.loss_convfact_aggamt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for lbaAdjustmentTypeName
        ///</summary>
        [TestMethod()]
        public void lbaAdjustmentTypeNameTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.lbaAdjustmentTypeName = expected;
            actual = target.lbaAdjustmentTypeName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for lbaAdjustmentTypelookup
        ///</summary>
        [TestMethod()]
        public void lbaAdjustmentTypelookupTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.lbaAdjustmentTypelookup;
            
        }

        /// <summary>
        ///A test for lba_Adjustment_typ
        ///</summary>
        [TestMethod()]
        public void lba_Adjustment_typTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.lba_Adjustment_typ = expected;
            actual = target.lba_Adjustment_typ;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for lay_lossconv_znapayamt
        ///</summary>
        [TestMethod()]
        public void lay_lossconv_znapayamtTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.lay_lossconv_znapayamt = expected;
            actual = target.lay_lossconv_znapayamt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for lay_lossconv_FactInsPay
        ///</summary>
        [TestMethod()]
        public void lay_lossconv_FactInsPayTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.lay_lossconv_FactInsPay = expected;
            actual = target.lay_lossconv_FactInsPay;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incur_los_reim_fund_unlim_minimium_lim_ind
        ///</summary>
        [TestMethod()]
        public void incur_los_reim_fund_unlim_minimium_lim_indTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.incur_los_reim_fund_unlim_minimium_lim_ind = expected;
            actual = target.incur_los_reim_fund_unlim_minimium_lim_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incur_los_reim_fund_unlim_agmt_lim_ind
        ///</summary>
        [TestMethod()]
        public void incur_los_reim_fund_unlim_agmt_lim_indTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.incur_los_reim_fund_unlim_agmt_lim_ind = expected;
            actual = target.incur_los_reim_fund_unlim_agmt_lim_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incur_los_reim_fund_min_lim_amt
        ///</summary>
        [TestMethod()]
        public void incur_los_reim_fund_min_lim_amtTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.incur_los_reim_fund_min_lim_amt = expected;
            actual = target.incur_los_reim_fund_min_lim_amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incur_los_reim_fund_invc_lsi_ind
        ///</summary>
        [TestMethod()]
        public void incur_los_reim_fund_invc_lsi_indTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.incur_los_reim_fund_invc_lsi_ind = expected;
            actual = target.incur_los_reim_fund_invc_lsi_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incur_los_reim_fund_initl_fund_amt
        ///</summary>
        [TestMethod()]
        public void incur_los_reim_fund_initl_fund_amtTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.incur_los_reim_fund_initl_fund_amt = expected;
            actual = target.incur_los_reim_fund_initl_fund_amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incur_los_reim_fund_aggr_lim_amt
        ///</summary>
        [TestMethod()]
        public void incur_los_reim_fund_aggr_lim_amtTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.incur_los_reim_fund_aggr_lim_amt = expected;
            actual = target.incur_los_reim_fund_aggr_lim_amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incur_but_not_rptd_los_dev_fctr_id
        ///</summary>
        [TestMethod()]
        public void incur_but_not_rptd_los_dev_fctr_idTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.incur_but_not_rptd_los_dev_fctr_id = expected;
            actual = target.incur_but_not_rptd_los_dev_fctr_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incld_ibnr_ldf_ind
        ///</summary>
        [TestMethod()]
        public void incld_ibnr_ldf_indTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.incld_ibnr_ldf_ind = expected;
            actual = target.incld_ibnr_ldf_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for incld_ernd_retro_prem_ind
        ///</summary>
        [TestMethod()]
        public void incld_ernd_retro_prem_indTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.incld_ernd_retro_prem_ind = expected;
            actual = target.incld_ernd_retro_prem_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for HasValue
        ///</summary>
        [TestMethod()]
        public void HasValueTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.HasValue;
            
        }

        /// <summary>
        ///A test for Escrow_PrevAmt
        ///</summary>
        [TestMethod()]
        public void Escrow_PrevAmtTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Escrow_PrevAmt = expected;
            actual = target.Escrow_PrevAmt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Escrow_PLMNumber
        ///</summary>
        [TestMethod()]
        public void Escrow_PLMNumberTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.Escrow_PLMNumber = expected;
            actual = target.Escrow_PLMNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Escrow_MnthsHeld
        ///</summary>
        [TestMethod()]
        public void Escrow_MnthsHeldTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.Escrow_MnthsHeld = expected;
            actual = target.Escrow_MnthsHeld;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Escrow_Diveser
        ///</summary>
        [TestMethod()]
        public void Escrow_DiveserTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.Escrow_Diveser = expected;
            actual = target.Escrow_Diveser;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for depst_amt
        ///</summary>
        [TestMethod()]
        public void depst_amtTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.depst_amt = expected;
            actual = target.depst_amt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Cstmr_Id
        ///</summary>
        [TestMethod()]
        public void Cstmr_IdTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Cstmr_Id = expected;
            actual = target.Cstmr_Id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATE_USER_ID = expected;
            actual = target.CREATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_DATE
        ///</summary>
        [TestMethod()]
        public void CREATE_DATETest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for clm_hndl_fee_basis_id
        ///</summary>
        [TestMethod()]
        public void clm_hndl_fee_basis_idTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.clm_hndl_fee_basis_id = expected;
            actual = target.clm_hndl_fee_basis_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for clm_hnd_fee_basid
        ///</summary>
        [TestMethod()]
        public void clm_hnd_fee_basidTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.clm_hnd_fee_basid = expected;
            actual = target.clm_hnd_fee_basid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CHFBasisTypeName
        ///</summary>
        [TestMethod()]
        public void CHFBasisTypeNameTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CHFBasisTypeName = expected;
            actual = target.CHFBasisTypeName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CHFBasisLookup
        ///</summary>
        [TestMethod()]
        public void CHFBasisLookupTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.CHFBasisLookup;
            
        }

        /// <summary>
        ///A test for AdjParametPolBEs
        ///</summary>
        [TestMethod()]
        public void AdjParametPolBEsTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            IList<AdjustmentParameterPolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AdjustmentParameterPolicyBE> actual;
            target.AdjParametPolBEs = expected;
            actual = target.AdjParametPolBEs;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjparameterTypeID
        ///</summary>
        [TestMethod()]
        public void AdjparameterTypeIDTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.AdjparameterTypeID = expected;
            actual = target.AdjparameterTypeID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for adj_paramet_setup_id
        ///</summary>
        [TestMethod()]
        public void adj_paramet_setup_idTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.adj_paramet_setup_id = expected;
            actual = target.adj_paramet_setup_id;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for actv_ind
        ///</summary>
        [TestMethod()]
        public void actv_indTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.actv_ind = expected;
            actual = target.actv_ind;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustmentParameterSetupBE Constructor
        ///</summary>
        [TestMethod()]
        public void AdjustmentParameterSetupBEConstructorTest()
        {
            AdjustmentParameterSetupBE target = new AdjustmentParameterSetupBE();
            
        }
    }
}
