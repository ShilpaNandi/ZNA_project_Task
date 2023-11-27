using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for ProgramPeriodsBSTest and is intended
    ///to contain all ProgramPeriodsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramPeriodsBSTest
    {
        static PersonBE PerBE;
        static BrokerBE brokerBE;
        static AccountBE AccBE;
        static ProgramPeriodBE progPerBE;
        static PremiumAdjustmentBE premAdjBE;
        static PremAdjustmentBS premAdjBS;
        static PremiumAdjustmentPeriodBE premAdjPeriodBE;
        static PremiumAdjustmentPeriodBS premAdjPeriodBS;
        
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
            premAdjBS = new PremAdjustmentBS();
            premAdjPeriodBS = new PremiumAdjustmentPeriodBS();
            AddBrokerData();
            AddPersonData();
            AddAccount();
            AddProgramPeriod();
            AddPremiumAdjustmentData();
            AddPremiumAdjustmentPeriodData();

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
        /// a method for adding New Record in EXTRNL_ORG Table when the classis initiated 
        /// </summary>
        private static void AddBrokerData()
        {
            brokerBE = new BrokerBE();
            brokerBE.FULL_NAME = "Broker_Prgm_perd_Test";
            brokerBE.CONTACT_TYPE_ID = 233;
            brokerBE.CREATE_DATE = System.DateTime.Now;
            brokerBE.CREATE_USER_ID = 1;

            BrokerBS brokerBS = new BrokerBS();
            brokerBS.Update(brokerBE);
        }
        /// <summary>
        /// a method for adding New Record in Person Table when the classis initiated 
        /// </summary>
        private static void AddPersonData()
        {
            PerBE = new PersonBE();
            PerBE.FORENAME = "aaa";
            PerBE.SURNAME = "bbb";
            PerBE.EXTERNAL_ORGN_ID = brokerBE.EXTRNL_ORG_ID;
            PerBE.CREATEDDATE = System.DateTime.Now;
            PerBE.CREATEDUSER_ID = 1;
            PersonBS perBS = new PersonBS();
            perBS.Update(PerBE);
        }
        private static void AddAccount()
        {
            AccountBS AccBS = new AccountBS();
            AccBE = new AccountBE();
            AccBE.BKTCYBUYOUT_DATE = System.DateTime.Now;
            AccBE.BKTCYBUYOUT_ID = 1;
            
            AccBE.FINC_PTY_ID = "1234567890";
            AccBE.FULL_NM = "UnitTestingNEW"+ System.DateTime.Now;
            AccBE.MARYLAND_RETRO_IND = true;
            AccBE.MSTR_ACCT_IND = false;
            AccBE.PERSON_ID = PerBE.PERSON_ID;
            AccBE.PEO_IND = true;
            AccBE.SUPRT_SERV_CUSTMR_GP_ID = "0123456789";
            AccBE.TPA_FUNDED_IND = true;
            AccBE.ACTV_IND = true;
            AccBE.CREATE_DATE = System.DateTime.Now;
            AccBE.CREATE_USER_ID = 1;
            AccBS.Save(AccBE);
        }

        private static void AddProgramPeriod()
        {
            ProgramPeriodsBS prpgPerBS = new ProgramPeriodsBS();
            progPerBE = new ProgramPeriodBE();
            progPerBE.CUSTMR_ID = AccBE.CUSTMR_ID;
            progPerBE.STRT_DT = Convert.ToDateTime("10/10/2008");
            progPerBE.PLAN_END_DT = System.DateTime.Parse("11/11/2008");
            progPerBE.LOS_SENS_INFO_END_DT = System.DateTime.Parse("3/3/2008");
            progPerBE.LOS_SENS_INFO_STRT_DT = System.DateTime.Parse("5/5/2008");
            progPerBE.BRKR_ID = brokerBE.EXTRNL_ORG_ID;
            progPerBE.BRKR_CONCTC_ID = PerBE.PERSON_ID;
            progPerBE.PGM_TYP_ID = 114;
            progPerBE.BSN_UNT_OFC_ID = 2;
            progPerBE.PAID_INCUR_TYP_ID = 297;
            progPerBE.INCUR_CONV_MMS_CNT = 23;
            progPerBE.FST_ADJ_MMS_FROM_INCP_CNT = 14;
            progPerBE.VALN_MM_DT = System.DateTime.Parse("2/29/2008");
            progPerBE.ADJ_FREQ_MMS_INTVRL_CNT = 22;
            progPerBE.BNKRPT_BUYOUT_ID = 330;
            progPerBE.BNKRPT_BUYOUT_EFF_DT = System.DateTime.Parse("10/12/2008");
            progPerBE.FNL_ADJ_DT = System.DateTime.Parse("1/12/2008");
            progPerBE.INCLD_CAPTV_PAYKND_IND = true;
            progPerBE.AVG_TAX_MULTI_IND = true;
            progPerBE.COMB_ELEMTS_MAX_AMT = 230;
            progPerBE.ZNA_SERV_COMP_CLM_HNDL_FEE_IND = true;
            progPerBE.PEO_PAY_IN_AMT = 12;
            progPerBE.FST_ADJ_NON_PREM_MMS_CNT = 140;
            progPerBE.FREQ_NON_PREM_MMS_CNT = 230;
            progPerBE.FNL_ADJ_NON_PREM_DT = System.DateTime.Parse("2/20/2008");
            progPerBE.TAX_MULTI_FCTR_RT = 120;
            progPerBE.INCLD_LEGEND_IND = true;
            progPerBE.FST_ADJ_DT = System.DateTime.Parse("3/3/2008");
            progPerBE.LSI_RETRIEVE_FROM_DT = System.DateTime.Parse("2/5/2007");
            progPerBE.CREATE_USER_ID = 1;
            progPerBE.CREATE_DATE = System.DateTime.Now;
            progPerBE.ACTV_IND = true;
            prpgPerBS.Update(progPerBE);

        }

        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj   Table 
        /// To meet F.key in PremAdjNYSIF Table 
        /// </summary>
        private static void AddPremiumAdjustmentData()
        {
            premAdjBE = new PremiumAdjustmentBE();
            premAdjBE.CUSTOMERID = AccBE.CUSTMR_ID;
            premAdjBE.VALN_DT = System.DateTime.Now;
            premAdjBE.CRTE_DT = System.DateTime.Now;
            premAdjBE.CRTE_USER_ID = 1;
            premAdjBS.Save(premAdjBE);

        }
        /// <summary>
        ///  a method for adding New Record when the class is initiated Into Prem Adj Period  Table 
        /// To meet F.key in PremAdjNYSIF Table 
        /// </summary>
        private static void AddPremiumAdjustmentPeriodData()
        {
            premAdjPeriodBE = new PremiumAdjustmentPeriodBE();
            premAdjPeriodBE.CUSTMR_ID = AccBE.CUSTMR_ID;
            premAdjPeriodBE.REG_CUSTMR_ID = AccBE.CUSTMR_ID;
            premAdjPeriodBE.PREM_ADJ_ID = premAdjBE.PREMIUM_ADJ_ID;
            premAdjPeriodBE.PREM_ADJ_PGM_ID = progPerBE.PREM_ADJ_PGM_ID;
            premAdjPeriodBE.CREATE_USER_ID = 1;
            premAdjPeriodBE.CREATE_DATE = System.DateTime.Now;
            premAdjPeriodBS.Save(premAdjPeriodBE);
        }
        public void AddNewProgramPeriod()
        {
            bool expected = true; // TODO: Initialize to an appropriate value
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            ProgramPeriodBE progPerBE = new ProgramPeriodBE();
            progPerBE.CUSTMR_ID = AccBE.CUSTMR_ID;
            progPerBE.STRT_DT = System.DateTime.Parse("10/10/2008");
            progPerBE.PLAN_END_DT = System.DateTime.Parse("11/11/2008");
            progPerBE.LOS_SENS_INFO_END_DT = System.DateTime.Parse("3/3/2008");
            progPerBE.LOS_SENS_INFO_STRT_DT = System.DateTime.Parse("5/5/2008");
            progPerBE.BRKR_ID = 1;
            progPerBE.BRKR_CONCTC_ID = 1;
            progPerBE.PGM_TYP_ID = 114;
            progPerBE.BSN_UNT_OFC_ID = 2;
            progPerBE.PAID_INCUR_TYP_ID = 297;
            progPerBE.INCUR_CONV_MMS_CNT = 23;
            progPerBE.FST_ADJ_MMS_FROM_INCP_CNT = 14;
            progPerBE.VALN_MM_DT = System.DateTime.Parse("2/29/2008");
            progPerBE.ADJ_FREQ_MMS_INTVRL_CNT = 22;
            progPerBE.BNKRPT_BUYOUT_ID = 330;
            progPerBE.BNKRPT_BUYOUT_EFF_DT = System.DateTime.Parse("10/12/2008");
            progPerBE.FNL_ADJ_DT = System.DateTime.Parse("1/12/2008");
            progPerBE.INCLD_CAPTV_PAYKND_IND = true;
            progPerBE.AVG_TAX_MULTI_IND = true;
            progPerBE.COMB_ELEMTS_MAX_AMT = 230;
            progPerBE.ZNA_SERV_COMP_CLM_HNDL_FEE_IND = true;
            progPerBE.PEO_PAY_IN_AMT = 12;
            progPerBE.FST_ADJ_NON_PREM_MMS_CNT = 140;
            progPerBE.FREQ_NON_PREM_MMS_CNT = 230;
            progPerBE.FNL_ADJ_NON_PREM_DT = System.DateTime.Parse("2/20/2008");
            progPerBE.TAX_MULTI_FCTR_RT = 120;
            progPerBE.INCLD_LEGEND_IND = true;
            progPerBE.FST_ADJ_DT = System.DateTime.Parse("3/3/2008");
            progPerBE.LSI_RETRIEVE_FROM_DT = System.DateTime.Parse("2/5/2007");
            progPerBE.CREATE_USER_ID = 1;
            progPerBE.CREATE_DATE = System.DateTime.Now;
            progPerBE.ACTV_IND = true;
            
            bool actual;
            actual = target.Update(progPerBE);
            Assert.AreEqual(expected, actual);
            
        }

        [TestMethod()]
        public void UpdateProgramPeriod()
        {
            bool expected = true; // TODO: Initialize to an appropriate value
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            //ProgramPeriodBE progPerBE = new ProgramPeriodBE();
            //progPerBE = target.getProgramPeriodInfo(progPerBE.);
            progPerBE.INCLD_LEGEND_IND = true;
            bool actual;
            actual = target.Update(progPerBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
    [ExpectedException (typeof(System.NullReferenceException))]
        public void UpdateTestwithNull()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            ProgramPeriodBE prgmperdBE = new ProgramPeriodBE(); // TODO: Initialize to an appropriate value
            prgmperdBE = null;
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(prgmperdBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IsProgramPeriodExits
        ///</summary>
        [TestMethod()]
        public void IsProgramPeriodExitsTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int progPerID = progPerBE.PREM_ADJ_PGM_ID; // TODO: Initialize to an appropriate value
            int brokerID = brokerBE.EXTRNL_ORG_ID; // TODO: Initialize to an appropriate value
            int buOffice = Convert.ToInt32(progPerBE.BSN_UNT_OFC_ID); // TODO: Initialize to an appropriate value
            string StartDate = Convert.ToDateTime(progPerBE.STRT_DT).ToShortDateString();// TODO: Initialize to an appropriate value
            string EndDate = Convert.ToDateTime(progPerBE.PLAN_END_DT).ToShortDateString(); ; // TODO: Initialize to an appropriate value
            string ValDate = Convert.ToDateTime(progPerBE.VALN_MM_DT).ToShortDateString(); ; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsProgramPeriodExits(progPerID, brokerID, buOffice, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), AccBE.CUSTMR_ID, Convert.ToDateTime(ValDate), Convert.ToInt32(progPerBE.PGM_TYP_ID));
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getUserNames
        ///</summary>
        [TestMethod()]
        public void getUserNamesTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            IList<PersonBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PersonBE> actual;
            actual = target.getUserNames();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getrangeprogramperiods
        ///</summary>
        [TestMethod()]
        public void getrangeprogramperiodsTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int buOfficeID = 0; // TODO: Initialize to an appropriate value
            int brokerID = 0; // TODO: Initialize to an appropriate value
            char stChr = '\0'; // TODO: Initialize to an appropriate value
            char edChr = '\0'; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.getrangeprogramperiods(buOfficeID, brokerID, stChr, edChr);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetProgramPeriodSearchDashboard
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodSearchDashboardTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int AccNo = 0; // TODO: Initialize to an appropriate value
            int PrgPrdTypID = 1; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetProgramPeriodSearchDashboard(AccNo, PrgPrdTypID);
            Assert.AreNotEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for GetProgramPeriodSearchDashboard
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodSearchDashboard2Test()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int AccNo = 0; // TODO: Initialize to an appropriate value
            int PrgTypID = 1; // TODO: Initialize to an appropriate value
            string valDate = "";
            int adjNo =0;
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetProgramPeriodSearchDashboard(AccNo, PrgTypID,adjNo,valDate);
            Assert.AreNotEqual(expected, actual);

        }


        /// <summary>
        ///A test for AdjustmentRevision
        ///</summary>
        [TestMethod()]
        
        public void AdjustmentRevision()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            AccBE.CUSTMR_REL_ID = AccBE.CUSTMR_ID;
            IList<PremiumAdjustmentBE> premadjBE = (new PremAdjustmentBS()).GetPremAdjSearch(AccBE.CUSTMR_ID);
            int adjno = 0;
            if (premadjBE.Count>0)
            adjno = premadjBE[0].PREMIUM_ADJ_ID;
            string actual;
            actual = target.AdjustmentRevision(adjno, AccBE.CUSTMR_ID, Convert.ToInt32(PerBE.PERSON_ID));
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for AdjustmentCancel
        ///</summary>
        [TestMethod()]
        public void AdjustmentCancel()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentBE> premadjBE = (new PremAdjustmentBS()).GetPremAdjSearch(AccBE.CUSTMR_ID);
            int adjno = 0;
            if (premadjBE.Count > 0)
                adjno = premadjBE[0].PREMIUM_ADJ_ID;
            string actual;
            actual = target.AdjustmentCancel(adjno);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for AdjustmentVoid
        ///</summary>
        [TestMethod()]
        
        public void AdjustmentVoid()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            IList<PremiumAdjustmentBE> premadjBE = (new PremAdjustmentBS()).GetPremAdjSearch(AccBE.CUSTMR_ID);
            int adjno = 0;
            if (premadjBE.Count > 0)
                adjno = premadjBE[0].PREMIUM_ADJ_ID;
            
            string actual;
            actual = target.AdjustmentVoid(adjno, AccBE.CUSTMR_ID, 1);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for CalcDriver
        ///</summary>
        [TestMethod()]
        public void CalcDriver()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.CalcDriver(AccBE.CUSTMR_ID, progPerBE.PREM_ADJ_PGM_ID.ToString(), "", false,true, 1);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetProgramPeriodSearch
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodSearchTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            string straccountID = AccBE.CUSTMR_ID.ToString(); // TODO: Initialize to an appropriate value
            string strValDate = Convert.ToDateTime(progPerBE.VALN_MM_DT).ToShortDateString(); // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> actual;
            actual = target.GetProgramPeriodSearch(straccountID, strValDate);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetProgramPeriodsList
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodsList()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> actual;
            actual = target.GetProgramPeriodsList(progPerBE.PREM_ADJ_PGM_ID.ToString());
            Assert.AreNotEqual(expected, actual);

        }



        /// <summary>
        ///A test for GetProgramPeriodSearch
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodsByPremAdjID()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            string strValDate = Convert.ToDateTime(progPerBE.VALN_MM_DT).ToShortDateString(); // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> actual;
            actual = target.GetProgramPeriodsByPremAdjID("1", Convert.ToDateTime(strValDate));
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetProgramPeriods
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodsTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int accountID = AccBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetProgramPeriods(accountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetProgramPeriodList
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodListTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int accountID = AccBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodListBE> actual;
            actual = target.GetProgramPeriodList(accountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getProgramPeriodInfo
        ///</summary>
        [TestMethod()]
        public void getProgramPeriodInfoTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int prgPeriodID = progPerBE.PREM_ADJ_PGM_ID; // TODO: Initialize to an appropriate value
            ProgramPeriodBE expected = null; // TODO: Initialize to an appropriate value
            ProgramPeriodBE actual;
            actual = target.getProgramPeriodInfo(prgPeriodID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetProgramPeriodData
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodDataTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int accountID = AccBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            int statusID = 343; // TODO: Initialize to an appropriate value
            int personID = PerBE.PERSON_ID; // TODO: Initialize to an appropriate value
            string startDate = Convert.ToDateTime(progPerBE.STRT_DT).ToShortDateString(); ; // TODO: Initialize to an appropriate value
            string endDate = Convert.ToDateTime(progPerBE.STRT_DT).ToShortDateString(); ; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetProgramPeriodData(accountID, statusID, personID, startDate, endDate);
            Assert.AreNotEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for GetProgramPeriodData
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodData2Test()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int statusID = 343; // TODO: Initialize to an appropriate value
            int personID = PerBE.PERSON_ID; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetProgramPeriodData(statusID, personID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for GetProgramPeriodID
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodID()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int personID = PerBE.PERSON_ID; // TODO: Initialize to an appropriate value
            string StartDate = Convert.ToDateTime(progPerBE.STRT_DT).ToShortDateString();// TODO: Initialize to an appropriate value
            string EndDate = Convert.ToDateTime(progPerBE.PLAN_END_DT).ToShortDateString(); // TODO: Initialize to an appropriate value
            string valDate = Convert.ToDateTime(progPerBE.VALN_MM_DT).ToShortDateString();  // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> actual;
            actual = target.GetProgramPeriodID(AccBE.CUSTMR_ID, Convert.ToDateTime(valDate), Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetProgramPeriodsByCustmrID
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodsByCustmrID()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> actual;
            actual = target.GetProgramPeriodsByCustmrID(AccBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetCustomerList
        ///</summary>
        [TestMethod()]
        public void GetCustomerListTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int customerid = AccBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            int buID = Convert.ToInt32(progPerBE.BSN_UNT_OFC_ID); // TODO: Initialize to an appropriate value
            int brokerID = Convert.ToInt32(progPerBE.BRKR_ID); // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetCustomerList(customerid, buID, brokerID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetActiveInActiveProgramPeriods
        ///</summary>
        [TestMethod()]
        public void GetActiveInActiveProgramPeriodsTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            int accountID = AccBE.CUSTMR_ID; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodListBE> actual;
            actual = target.GetActiveInActiveProgramPeriods(accountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        
       
        /// <summary>
        ///A test for ProgramPeriodsBS Constructor
        ///</summary>
        [TestMethod()]
        public void ProgramPeriodsBSConstructorTest()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS();
            
        }

        public void GetRangeProgramperiodsTest()
        { 
        
        }
        /// <summary>
        ///A test for getProgramPeriodRow
        ///</summary>
        [TestMethod()]
        public void getProgramPeriodRow()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            ProgramPeriodBE expected = new ProgramPeriodBE();  // TODO: Initialize to an appropriate value
           ProgramPeriodBE actual = new ProgramPeriodBE(); 
            actual = target.getProgramPeriodRow(progPerBE.PREM_ADJ_PGM_ID);
            Assert.AreNotEqual(expected, actual);
            
            
        }

        /// <summary>
        ///A test for GetRoles
        ///</summary>
        [TestMethod()]
        public void GetRoles()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetRoles(AccBE.CUSTMR_ID,PerBE.PERSON_ID);
            Assert.AreNotEqual(expected, actual);

        }


        /// <summary>
        ///A test for GetValDate
        ///</summary>
        [TestMethod()]
        public void GetValDate()
        {
            ProgramPeriodsBS target = new ProgramPeriodsBS(); // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodSearchListBE> actual;
            actual = target.GetValDate(AccBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual);

        }
    }
}
