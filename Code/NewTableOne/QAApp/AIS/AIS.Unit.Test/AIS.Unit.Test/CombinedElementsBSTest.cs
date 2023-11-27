using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CombinedElementsBSTest and is intended
    ///to contain all CombinedElementsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CombinedElementsBSTest
    {
        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
        static ProgramPeriodBE programPeriodBE;
        static ProgramPeriodsBS programPeriodsBS;
        static PolicyBS polBS;
        static PolicyBE polBE;
        static CombinedElementsBE combElemntsBE;
        static CombinedElementsBS combElemntsBS;
        

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
            //Commercial agreement Table
            polBS = new PolicyBS();
            AddPolicy();
            //Combined Elements
            combElemntsBS = new CombinedElementsBS();
            AddCombElementsCommonData();
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
        /// To meet F.key in PremAdjCombElemnts Table 
        /// </summary>
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "Praveen" + System.DateTime.Now.ToString();
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
        /// 
        /// </summary>
        private static void AddPolicy()
        {
            polBE = new PolicyBE();
            polBE.cstmrid = accountBE.CUSTMR_ID;
            polBE.ProgramPeriodID = programPeriodBE.PREM_ADJ_PGM_ID;
            polBE.PolicySymbol = "EDF";
            polBE.PolicyNumber = "23456679";
            polBE.PolicyModulus = "43";
            polBE.PolicyEffectiveDate = System.DateTime.Parse("01/01/2007");
            polBE.PlanEndDate = System.DateTime.Parse("02/02/2008");
            polBE.UnlimDedtblPolLimitIndicator = true;
            polBE.OverrideDedtblLimitAmount = 10;
            polBE.UnlimOverrideDedtblLimitIndicator = true;
            polBE.DedTblPolicyLimitAmount = System.Convert.ToDecimal(13.5);
            polBE.LDFIncurredNotReport = true;
            polBE.LDFIncurredNO63740 = true;
            polBE.LDFFactor = System.Convert.ToDecimal(12.10);
            polBE.IBNRFactor = System.Convert.ToDecimal(14.5);
            polBE.DedtblProtPolMaxAmount = 100;
            polBE.TPAIndicator = true;
            polBE.TPADirectIndicator = true;
            polBE.CREATE_DATE = System.DateTime.Now;
            polBE.CREATE_USER_ID = 1;
            polBE.IsActive = true;
            polBS.Update(polBE);
        }
        private static void AddCombElementsCommonData()
        {
            combElemntsBE = new CombinedElementsBE();
            combElemntsBE.COML_AGMT_ID = polBE.PolicyID;
            combElemntsBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            combElemntsBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            combElemntsBE.CRTE_DT = System.DateTime.Now;
            combElemntsBE.CRTE_USR_ID = 1;
            combElemntsBS.Update(combElemntsBE);

        }
        [TestMethod()]
        public void AddCombElementsWithData()
        {
            bool expected = true;
            bool actual = false;
            CombinedElementsBE combBE = new CombinedElementsBE();
            combBE.COML_AGMT_ID = polBE.PolicyID;
            combBE.PREM_ADJ_PGM_ID = programPeriodBE.PREM_ADJ_PGM_ID;
            combBE.CUSTMR_ID = accountBE.CUSTMR_ID;
            combBE.CRTE_DT = System.DateTime.Now;
            combBE.CRTE_USR_ID = 1;
            actual=combElemntsBS.Update(combBE);
            Assert.AreEqual(expected, actual);

        }
        [TestMethod()]
        public void AddTestWithNULL()
        {
            CombinedElementsBS CombBS = new CombinedElementsBS();
            CombinedElementsBE CombBE = null;
            bool expected = false;
            bool actual;
            actual = CombBS.Update(CombBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            CombinedElementsBE CombBE = (new CombinedElementsBS()).getCombelemID(combElemntsBE.COMB_ELEMTS_SETUP_ID);
            CombBE.TOT_AMT = 1000;
            CombBE.UPDT_DATE = System.DateTime.Now;
            CombBE.UPDT_USER_ID = 1;
            actual = new CombinedElementsBS().Update(CombBE);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            CombinedElementsBS CombBS = new CombinedElementsBS();
            CombinedElementsBE CombBE = null;
            bool expected = false;
            bool actual;
            actual = CombBS.Update(CombBE);
            Assert.AreEqual(expected, actual);
            

        }
        /// <summary>
        ///A test for GetCombinedElements
        ///</summary>
        [TestMethod()]
        public void GetCombinedElementsTest1()
        {
            CombinedElementsBS target = new CombinedElementsBS();
            int intProgramperiodID = programPeriodBE.PREM_ADJ_PGM_ID;
            int intAccountID = accountBE.CUSTMR_ID;
            int expected = 0; 
            IList<CombinedElementsBE> actual;
            actual = target.GetCombinedElements(intProgramperiodID, intAccountID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetCombinedElements
        ///</summary>
        [TestMethod()]
        public void GetCombinedElementsTest()
        {
            CombinedElementsBS target = new CombinedElementsBS();
            int Programperiod = programPeriodBE.PREM_ADJ_PGM_ID; 
            int expected = 0; 
            IList<CombinedElementsBE> actual;
            actual = target.GetCombinedElements(Programperiod);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getCombelemID
        ///</summary>
        [TestMethod()]
        public void getCombelemIDTest()
        {
            CombinedElementsBS target = new CombinedElementsBS();
            int CombElemID = combElemntsBE.COMB_ELEMTS_SETUP_ID; 
            CombinedElementsBE expected = null; 
            CombinedElementsBE actual;
            actual = target.getCombelemID(CombElemID);
            Assert.AreNotEqual(expected, actual);
        }

        
    }
}
