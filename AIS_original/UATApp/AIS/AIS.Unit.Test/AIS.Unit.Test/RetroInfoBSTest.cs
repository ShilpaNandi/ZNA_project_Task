using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for RetroInfoBSTest and is intended
    ///to contain all RetroInfoBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RetroInfoBSTest
    {
        private TestContext testContextInstance;
        static AccountBE AcctBE;
        static ProgramPeriodBE ProgPerdBE;
        static Coml_AgmtBE CommlAgtBE;
        static Coml_Agmt_AuDtBE ComAgrAudBE;
        static Coml_Agmt_AuDtBS ComAgrAudBS;
        static RetroInfoBS retroBS;
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
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
       

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            retroBS= new RetroInfoBS();
            AddCustomerData();
            AddProgPerdData();
            AddCommAgrData();
            AddCommrAgrAudData();
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
        /// a method for adding New Record in Customer when the classis initiated 
        /// </summary>
        private static void AddCustomerData()
        {
            AcctBE = new AccountBE();
            AcctBE.FULL_NM = "retroTest" + System.DateTime.Now.ToLongTimeString();
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
            CommlAgtBE = new Coml_AgmtBE();
            CommlAgtBE.Prem_Adj_Prg_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            CommlAgtBE.Customer_ID = AcctBE.CUSTMR_ID;
            CommlAgtBE.ADJ_TYP_ID = 65;
            CommlAgtBE.PlannedEndDate = System.DateTime.Now;
            CommlAgtBE.CreatedDate = System.DateTime.Now;
            CommlAgtBE.CreatedUserID = 1;
            (new Coml_AgmtBS()).Save(CommlAgtBE);
        }
        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddCommrAgrAudData()
        {

            ComAgrAudBE = new Coml_Agmt_AuDtBE();
            ComAgrAudBE.Comm_Agr_ID = CommlAgtBE.Comm_Agr_ID;
            ComAgrAudBE.Customer_ID = AcctBE.CUSTMR_ID;
            ComAgrAudBE.Prem_Adj_Prg_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            ComAgrAudBE.ExposureAmt = 100;
            
            ComAgrAudBE.StartDate = System.DateTime.Now;
            ComAgrAudBE.CreatedDate = System.DateTime.Now;
            ComAgrAudBE.CreatedUser_ID = 1;
            (new Coml_Agmt_AuDtBS()).Update(ComAgrAudBE);
        }

        /// <summary>
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddRetroInfoData()
        {

            RetroInfoBE retroBE = new RetroInfoBE();
            retroBE.CUSTMR_ID = AcctBE.CUSTMR_ID;            
            retroBE.PREM_ADJ_PGM_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            retroBE.RETRO_ELEMT_TYP_ID = 334;
            
            retroBE.CRTE_DT = System.DateTime.Now;
            retroBE.CRTE_USER_ID = 1;
            



            retroBS.SaveRetroData(retroBE);
        }

        /// <summary>
        ///A test for SaveRetroData
        ///</summary>
        [TestMethod()]
        public void SaveRetroDataTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            RetroInfoBE retroInfoBE = new RetroInfoBE(); // TODO: Initialize to an appropriate value
            retroInfoBE.ADJ_RETRO_INFO_ID = 0;
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            if (retroInfoBE.ADJ_RETRO_INFO_ID > 0)
            {
                actual = target.SaveRetroData(retroInfoBE);
            }
            else
            {
                actual = target.SaveRetroData(retroInfoBE);
            }
            
            actual = target.SaveRetroData(retroInfoBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SaveRetroData method
        ///</summary>
        [TestMethod()]
        public void SaveRetroDataUpdateTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            RetroInfoBE retroInfoBE = new RetroInfoBE(); // TODO: Initialize to an appropriate value
            retroInfoBE.ADJ_RETRO_INFO_ID = 1;
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            if (retroInfoBE.ADJ_RETRO_INFO_ID > 0)
            {
                actual = target.SaveRetroData(retroInfoBE);
            }
            else
            {
                actual = target.SaveRetroData(retroInfoBE);
            }

            actual = target.SaveRetroData(retroInfoBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for LoadData
        ///</summary>
        [TestMethod()]
        public void LoadDataTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int adjRetroInfoId = 0; // TODO: Initialize to an appropriate value
            RetroInfoBE expected = null; // TODO: Initialize to an appropriate value
            RetroInfoBE actual;
            actual = target.LoadData(adjRetroInfoId);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetStandardAuditExp
        ///</summary>
        [TestMethod()]
        public void GetStandardAuditExpTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = ProgPerdBE.PREM_ADJ_PGM_ID; // TODO: Initialize to an appropriate value
            int custmorId = AcctBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            decimal expected = 0.0m; // TODO: Initialize to an appropriate value
            decimal actual;
            actual = target.GetStandardAuditExp(programPeriodID, custmorId);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetRetroInfo
        ///</summary>
        [TestMethod()]
        public void GetRetroInfoTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            IList<RetroInfoBE> expected = null; // TODO: Initialize to an appropriate value
            IList<RetroInfoBE> actual;
            actual = target.GetRetroInfo(programPeriodID);
            Assert.AreNotEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for getRetroInfoForCopy
        ///</summary>
        [TestMethod()]
        public void getRetroInfoForCopy()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            IList<RetroInfoBE> expected = null; // TODO: Initialize to an appropriate value
            IList<RetroInfoBE> actual;
            actual = target.getRetroInfoForCopy(programPeriodID);
            Assert.AreNotEqual(expected, actual);

        }


        /// <summary>
        ///A test for GetPayrollAuditExp
        ///</summary>
        [TestMethod()]
        public void GetPayrollAuditExpTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = ProgPerdBE.PREM_ADJ_PGM_ID; // TODO: Initialize to an appropriate value
            int custmorId = AcctBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            decimal expected = 0; // TODO: Initialize to an appropriate value
            decimal actual;
            actual = target.GetPayrollAuditExp(programPeriodID, custmorId);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetCombinedAuditExp
        ///</summary>
        [TestMethod()]
        public void GetCombinedAuditExpTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = ProgPerdBE.PREM_ADJ_PGM_ID; // TODO: Initialize to an appropriate value
            int custmorId = AcctBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            decimal expected = 0; // TODO: Initialize to an appropriate value
            decimal actual;
            actual = target.GetCombinedAuditExp(programPeriodID, custmorId);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetOtherAuditExp
        ///</summary>
        [TestMethod()]
        public void GetOtherAuditExpTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = ProgPerdBE.PREM_ADJ_PGM_ID; // TODO: Initialize to an appropriate value
            int custmorId = AcctBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            Decimal expected = new Decimal(); // TODO: Initialize to an appropriate value
            Decimal actual;
            actual = target.GetOtherAuditExp(programPeriodID, custmorId);
            Assert.AreNotEqual(expected, actual);
            
        }


        /// <summary>
        ///A test for GetOtherAuditExp with data
        ///</summary>
        [TestMethod()]
        public void GetOtherAuditExpDataTest()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = 76; // TODO: Initialize to an appropriate value
            int custmorId = 18; // TODO: Initialize to an appropriate value
            Decimal expected = new Decimal(); // TODO: Initialize to an appropriate value
            Decimal actual;
            actual = target.GetOtherAuditExp(programPeriodID, custmorId);
            Assert.AreEqual(expected, actual);

        }

         /// <summary>
        ///A test for getCombElemntsMaxAndMinAmounts
        ///</summary>
        [TestMethod()]
        public void getCombElemntsMaxAndMinAmounts()
        {
            RetroInfoBS target = new RetroInfoBS(); // TODO: Initialize to an appropriate value
            int programPeriodID = 0; // TODO: Initialize to an appropriate value
            int custmorId = 0; // TODO: Initialize to an appropriate value
            int intRetroElemtTypID = 0;
            IList<RetroInfoBE> expected = null; // TODO: Initialize to an appropriate value
            IList<RetroInfoBE> actual;
            actual = target.getCombElemntsMaxAndMinAmounts(programPeriodID, custmorId,intRetroElemtTypID);
            Assert.AreNotEqual(expected, actual);
            
        }
        
    }
}
