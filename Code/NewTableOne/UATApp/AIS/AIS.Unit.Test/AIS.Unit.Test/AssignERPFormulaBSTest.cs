using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AssignERPFormulaBSTest and is intended
    ///to contain all AssignERPFormulaBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AssignERPFormulaBSTest
    {
       
        private TestContext testContextInstance;
        static AccountBE AcctBE;
        static ProgramPeriodBE papBE;
         static AssignERPFormulaBS target;
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
            
            target = new AssignERPFormulaBS();
            AddCustomerData();
            AddCommonData();
            
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        public void MyTestInitialize()
        {
            //papBE = new Prem_Adj_PgmBE();
            
        }
        
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
            papBE.CUSTMR_ID = AcctBE.CUSTMR_ID;
            
            papBE.CREATE_DATE = System.DateTime.Now;
            papBE.CREATE_USER_ID = 1;
            target.Update(papBE);
        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            AssignERPFormulaBS target = new AssignERPFormulaBS();
            ProgramPeriodBE papBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(papBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Adding ERP Formula
        ///</summary>
        [TestMethod()]
        public void AddTestWithData()
        {

            
            ProgramPeriodBE papBE1 = new ProgramPeriodBE();
            AssignERPFormulaBS ltarget = new AssignERPFormulaBS();
            bool expected = true; 
            bool actual;
            papBE1.CUSTMR_ID = AcctBE.CUSTMR_ID;
            papBE1.CREATE_DATE = System.DateTime.Now;
            papBE1.CREATE_USER_ID = 1;
            actual = ltarget.Update(papBE1);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestWithNULL()
        {
            AssignERPFormulaBS target = new AssignERPFormulaBS();
            ProgramPeriodBE papBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(papBE);

        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
           
            bool expected = true; 
            bool actual;
            //papBE.PremiumAdjustmentProgramID = 21;
            papBE.CUSTMR_ID = AcctBE.CUSTMR_ID;
            
            papBE.UPDATE_DATE = System.DateTime.Now;
            papBE.UPDATE_USER_ID = 1;
            actual = target.Update(papBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getAssignERPRow
        ///</summary>
        [TestMethod()]
        public void getAssignERPRowTestWithNUll()
        {
            
            
            int ProgPrdID = 0; 
            ProgramPeriodBE expected = null; 
            ProgramPeriodBE actual;
            actual = target.getAssignERPRow(ProgPrdID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for getAssignERPRow
        ///</summary>
        [TestMethod()]
        public void getAssignERPRowTestWithData()
        {
            
           
            ProgramPeriodBE expected = null;
            ProgramPeriodBE actual;
            actual = target.getAssignERPRow(papBE.PREM_ADJ_PGM_ID);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for AssignERPFormulaBS Constructor
        ///</summary>
        [TestMethod()]
        public void AssignERPFormulaBSConstructorTest()
        {
            AssignERPFormulaBS target = new AssignERPFormulaBS();
            
        }
    }
}
