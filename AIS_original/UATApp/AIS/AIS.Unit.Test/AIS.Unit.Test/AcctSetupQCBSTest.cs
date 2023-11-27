using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AcctSetupQCBSTest and is intended
    ///to contain all AcctSetupQCBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AcctSetupQCBSTest
    {
        static AccountBE AcctBE;
        static ProgramPeriodBE papBE;
        static AcctSetupQCBS target;

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
            target = new AcctSetupQCBS();
            AddCustomerData();
            AddCommonData();
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
            AcctBE.FULL_NM = "aaa" + System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            AccountBS AcctBS = new AccountBS();
            AcctBS.Save(AcctBE);
        }

        private static void AddCommonData()
        {
            papBE = new ProgramPeriodBE();
            target = new AcctSetupQCBS();
            
            papBE.CUSTMR_ID = AcctBE.CUSTMR_ID;        
            papBE.CREATE_USER_ID = 1;
            papBE.CREATE_DATE = System.DateTime.Now;
            target.Update(papBE);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            AcctSetupQCBS target = new AcctSetupQCBS();
            ProgramPeriodBE papBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(papBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            ProgramPeriodBE papBE1 = new ProgramPeriodBE();
            AcctSetupQCBS ltarget = new AcctSetupQCBS();
            papBE1.CUSTMR_ID = AcctBE.CUSTMR_ID;
            papBE1.CREATE_USER_ID = 1;
            papBE1.CREATE_DATE = System.DateTime.Now;
            bool expected = true;
            bool actual;
            actual=ltarget.Update(papBE1);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            papBE = new ProgramPeriodBE();
            target = new AcctSetupQCBS();
            papBE.CUSTMR_ID = AcctBE.CUSTMR_ID;
            papBE.CREATE_USER_ID = 1;
            papBE.CREATE_DATE = System.DateTime.Now;
            papBE.UPDATE_DATE = System.DateTime.Now;
            papBE.UPDATE_USER_ID = 1;
            bool expected = true;
            bool actual;
            
           actual=target.Update(papBE);
           Assert.AreEqual(expected, actual);

        }



        /// <summary>
        ///A test for Update
        ///</summary>
        ///
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestwithNull()
        {
            AcctSetupQCBS target = new AcctSetupQCBS();
            ProgramPeriodBE papBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(papBE);
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        ///A test for getRelatedAcctSetupQCItems
        ///</summary>
        [TestMethod()]
        public void getRelatedPrmPrdInfoTest()
        {
            
            int prmPrdID = 1;
            int expected = 1;
            IList<ProgramPeriodBE> actual;
            actual = target.getRelatedPrmPrdInfo(prmPrdID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getPreAdjPgmRow
        ///</summary>
        [TestMethod()]
        public void getPreAdjPgmRowTest()
        {
            
            
            
            ProgramPeriodBE expected = null;
            ProgramPeriodBE actual;
            actual = target.getPreAdjPgmRow(papBE.PREM_ADJ_PGM_ID);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AcctSetupQCBS Constructor
        ///</summary>
        [TestMethod()]
        public void AcctSetupQCBSConstructorTest()
        {
            AcctSetupQCBS target = new AcctSetupQCBS();
        }
    }
}
