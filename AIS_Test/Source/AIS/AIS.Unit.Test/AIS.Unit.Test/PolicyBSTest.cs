using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PolicyBSTest and is intended
    ///to contain all PolicyBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PolicyBSTest
    {

        static AccountBE AcctBE;
        static ProgramPeriodBE progPerBE;
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            
            AddAccount();
            AddProgramPeriod();
            AddPolicy();
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

        private static void AddAccount()
        {
            AcctBE = new AccountBE();
            AcctBE.FULL_NM = "aaa" + System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            (new AccountBS()).Save(AcctBE);
        }

        private static void AddProgramPeriod()
        {
            ProgramPeriodsBS prpgPerBS = new ProgramPeriodsBS();
            progPerBE = new ProgramPeriodBE();
            progPerBE.CUSTMR_ID = AcctBE.CUSTMR_ID;
            progPerBE.STRT_DT = System.DateTime.Parse("10/10/2008");
            progPerBE.PLAN_END_DT = System.DateTime.Parse("11/11/2008");
            progPerBE.LOS_SENS_INFO_END_DT = System.DateTime.Parse("3/3/2008");
            progPerBE.LOS_SENS_INFO_STRT_DT = System.DateTime.Parse("5/5/2008");
            //progPerBE.BRKR_ID = 1;
            //progPerBE.BRKR_CONCTC_ID = 1;
            //progPerBE.PGM_TYP_ID = 114;
            //progPerBE.BSN_UNT_OFC_ID = 2;
            //progPerBE.PAID_INCUR_TYP_ID = 297;
            //progPerBE.INCUR_CONV_MMS_CNT = 23;
            //progPerBE.FST_ADJ_MMS_FROM_INCP_CNT = 14;
            progPerBE.VALN_MM_DT = System.DateTime.Parse("2/29/2008");
            //progPerBE.ADJ_FREQ_MMS_INTVRL_CNT = 22;
            //progPerBE.BNKRPT_BUYOUT_ID = 330;
            progPerBE.BNKRPT_BUYOUT_EFF_DT = System.DateTime.Parse("10/12/2008");
            progPerBE.FNL_ADJ_DT = System.DateTime.Parse("1/12/2008");
            progPerBE.INCLD_CAPTV_PAYKND_IND = true;
            progPerBE.AVG_TAX_MULTI_IND = true;
            //progPerBE.COMB_ELEMTS_MAX_AMT = 230;
            progPerBE.ZNA_SERV_COMP_CLM_HNDL_FEE_IND = true;
            //progPerBE.PEO_PAY_IN_AMT = 12;
            //progPerBE.FST_ADJ_NON_PREM_MMS_CNT = 140;
            //progPerBE.FREQ_NON_PREM_MMS_CNT = 230;
            progPerBE.FNL_ADJ_NON_PREM_DT = System.DateTime.Parse("2/20/2008");
            //progPerBE.TAX_MULTI_FCTR_RT = 120;
            progPerBE.INCLD_LEGEND_IND = true;
            progPerBE.FST_ADJ_DT = System.DateTime.Parse("3/3/2008");
            progPerBE.LSI_RETRIEVE_FROM_DT = System.DateTime.Parse("2/5/2007");
            progPerBE.CREATE_USER_ID = 1;
            progPerBE.CREATE_DATE = System.DateTime.Now;
            progPerBE.ACTV_IND = true;
            prpgPerBS.Update(progPerBE);

        }

        private static void AddPolicy()
        {
            PolicyBS target = new PolicyBS();
            PolicyBE polBE = new PolicyBE();
            polBE.cstmrid = AcctBE.CUSTMR_ID;
            polBE.ProgramPeriodID = progPerBE.PREM_ADJ_PGM_ID;
            polBE.PolicySymbol = "EDF";
            polBE.PolicyNumber = "23456679";
            polBE.PolicyModulus = "43";
            polBE.PolicyEffectiveDate = System.DateTime.Parse("01/01/2007");
            polBE.PlanEndDate = System.DateTime.Parse("02/02/2008");
            //polBE.CoverageTypeID = 85;
            //polBE.ALAETypeID = 76;
            //polBE.AdjusmentTypeID = 63;
            //polBE.ALAECappedAmount = 20;
            polBE.UnlimDedtblPolLimitIndicator = true;
            polBE.OverrideDedtblLimitAmount = 10;
            polBE.UnlimOverrideDedtblLimitIndicator = true;
            polBE.DedTblPolicyLimitAmount = System.Convert.ToDecimal(13.5);
            polBE.LDFIncurredNotReport = true;
            polBE.LDFIncurredNO63740 = true;
            polBE.LDFFactor = System.Convert.ToDecimal(12.10);
            polBE.IBNRFactor = System.Convert.ToDecimal(14.5);
            //polBE.DedtblProtPolicyStID = 1;
            polBE.DedtblProtPolMaxAmount = 100;
            polBE.TPAIndicator = true;
            polBE.TPADirectIndicator = true;
            //polBE.LossSystemSourceID = 81;
            //polBE.NonConversionAmount = 12;
            //polBE.OtherPolicyAdjustmentAmount = 15;
            polBE.CREATE_DATE = System.DateTime.Now;
            polBE.CREATE_USER_ID = 1;
            polBE.IsActive = true;

            target.Update(polBE);
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest()
        {
            PolicyBS target = new PolicyBS();
            bool expected = true;
            bool actual;
            PolicyBE polBE = new PolicyBE();
            polBE.cstmrid = 1;
            polBE.ProgramPeriodID = 1;
            polBE.PolicySymbol = "BBC";
            polBE.PolicyNumber = "23434679";
            polBE.PolicyModulus = "34";
            polBE.PolicyEffectiveDate = System.DateTime.Parse("02/02/2007");
            polBE.PlanEndDate = System.DateTime.Parse("01/01/2008");
            polBE.CoverageTypeID = 85;
            polBE.ALAETypeID = 76;
            polBE.AdjusmentTypeID = 63;
            polBE.ALAECappedAmount = 20;
            polBE.UnlimDedtblPolLimitIndicator = true;
            polBE.OverrideDedtblLimitAmount = 10;
            polBE.UnlimOverrideDedtblLimitIndicator = true;
            polBE.DedTblPolicyLimitAmount = 123;
            polBE.LDFIncurredNotReport = true;
            polBE.LDFIncurredNO63740 = true;
            polBE.LDFFactor = System.Convert.ToDecimal(12.10);
            polBE.IBNRFactor = System.Convert.ToDecimal(14.5);
            polBE.DedtblProtPolicyStID = 1;
            polBE.DedtblProtPolMaxAmount = 100;
            polBE.TPAIndicator = true;
            polBE.TPADirectIndicator = true;
            polBE.LossSystemSourceID = 81;
            polBE.NonConversionAmount = 12;
            polBE.OtherPolicyAdjustmentAmount = 15;
            polBE.CREATE_DATE = System.DateTime.Now;
            polBE.CREATE_USER_ID = 1;
            polBE.IsActive = true;
            target.Update(polBE);
            actual = target.Update(polBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Save with null
        ///</summary>
        [TestMethod()]
        public void SaveTestWithNULL()
        {
            PolicyBS target = new PolicyBS();
            bool expected = false;
            bool actual;
            PolicyBE polBE = new PolicyBE();
            actual = target.Update(polBE);
            Assert.AreNotEqual(expected, actual);
            
        }

        [TestMethod()]
        public void UpdateTest()
        {
            PolicyBS target = new PolicyBS();
            bool expected = true;
            bool actual;
            PolicyBE polBE = new PolicyBE();
            polBE.PolicyID = 1;
            polBE.cstmrid = 1;
            polBE.ProgramPeriodID = 1;
            polBE.PolicySymbol = "BBC";
            polBE.PolicyNumber = "23434679";
            polBE.PolicyModulus = "34";
            polBE.PolicyEffectiveDate = System.DateTime.Parse("02/02/2007");
            polBE.PlanEndDate = System.DateTime.Parse("01/01/2008");
            polBE.CoverageTypeID = 85;
            polBE.ALAETypeID = 76;
            polBE.AdjusmentTypeID = 63;
            polBE.ALAECappedAmount = 20;
            polBE.UnlimDedtblPolLimitIndicator = true;
            polBE.OverrideDedtblLimitAmount = 10;
            polBE.UnlimOverrideDedtblLimitIndicator = true;
            polBE.DedTblPolicyLimitAmount = 125;
            polBE.LDFIncurredNotReport = true;
            polBE.LDFIncurredNO63740 = true;
            polBE.LDFFactor = System.Convert.ToDecimal(12.10);
            polBE.IBNRFactor = System.Convert.ToDecimal(14.5);
            polBE.DedtblProtPolicyStID = 1;
            polBE.DedtblProtPolMaxAmount = 100;
            polBE.TPAIndicator = true;
            polBE.TPADirectIndicator = true;
            polBE.LossSystemSourceID = 81;
            polBE.NonConversionAmount = 12;
            polBE.OtherPolicyAdjustmentAmount = 15;
            polBE.UPDATE_DATE = System.DateTime.Now;
            polBE.UPDATE_USER_ID = 1;
            polBE.IsActive = true;

            actual = target.Update(polBE);
            Assert.AreEqual(expected, actual);
            

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestwithNull()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            PolicyBE PolBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(PolBE);
            Assert.AreEqual(expected, actual);
            
        }



        /// <summary>
        ///A test for isPolicyAlreadyExist
        ///</summary>
        [TestMethod()]
        public void isPolicyAlreadyExistTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int polID = 0; // TODO: Initialize to an appropriate value            
            string polNo = string.Empty; // TODO: Initialize to an appropriate value            
            DateTime polEffDate = new DateTime(); // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.isPolicyAlreadyExist(polID,  polNo, polEffDate, programPeriodID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyLookUpData
        ///</summary>
        [TestMethod()]
        public void getPolicyLookUpDataTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            int cstmrid = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.getPolicyLookUpData(ProgramPeriodID, cstmrid);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyInfo
        ///</summary>
        [TestMethod()]
        public void getPolicyInfoTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int PolicyID = 0; // TODO: Initialize to an appropriate value
            PolicyBE expected = null; // TODO: Initialize to an appropriate value
            PolicyBE actual;
            actual = target.getPolicyInfo(PolicyID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyDataWithActID
        ///</summary>
        [TestMethod()]
        public void getPolicyDataWithActIDTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.getPolicyDataWithActID(ProgramPeriodID, AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyDataForParentID
        ///</summary>
        [TestMethod()]
        public void getPolicyDataForParentIDTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int ParentPolicyID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.getPolicyDataForParentID(ParentPolicyID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyDataforLookups
        ///</summary>
        [TestMethod()]
        public void getPolicyDataforLookupsTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            int cstmrid = 0; // TODO: Initialize to an appropriate value
            List<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            List<LookupBE> actual;
            actual = target.getPolicyDataforLookups(programPeriodID, cstmrid);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyData
        ///</summary>
        [TestMethod()]
        public void getPolicyDataTest2()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.getPolicyData();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyData
        ///</summary>
        [TestMethod()]
        public void getPolicyDataTest1()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.getPolicyData(ProgramPeriodID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPolicyData
        ///</summary>
        [TestMethod()]
        public void getPolicyDataTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            int PolicyID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.getPolicyData(ProgramPeriodID, PolicyID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getLookupName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ZurichNA.AIS.Business.Logic.dll")]
        public void getLookupNameTest()
        {
            PolicyBS_Accessor target = new PolicyBS_Accessor(); // TODO: Initialize to an appropriate value
            int Key = 0; // TODO: Initialize to an appropriate value
            Dictionary<int, string> Table = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getLookupName(Key, Table);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getLOBPolData
        ///</summary>
        [TestMethod()]
        public void getLOBPolDataTest()
        {
            PolicyBS target = new PolicyBS(); // TODO: Initialize to an appropriate value
            string LOB = string.Empty; // TODO: Initialize to an appropriate value
            int PrgID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.getLOBPolData(LOB, PrgID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for buildList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ZurichNA.AIS.Business.Logic.dll")]
        public void buildListTest()
        {
            PolicyBS_Accessor target = new PolicyBS_Accessor(); // TODO: Initialize to an appropriate value
            IList<PolicyBE> policy = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.buildList(policy);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyBS Constructor
        ///</summary>
        [TestMethod()]
        public void PolicyBSConstructorTest()
        {
            PolicyBS target = new PolicyBS();
            
        }
    }
}
