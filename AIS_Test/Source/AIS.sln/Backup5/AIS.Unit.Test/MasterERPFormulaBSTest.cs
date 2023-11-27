using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for MasterERPFormulaBSTest and is intended
    ///to contain all MasterERPFormulaBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MasterERPFormulaBSTest
    {


        private TestContext testContextInstance;
         static MasterERPFormulaBS target;
         static MasterERPFormulaBE ERPBE;

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
            ERPBE = new MasterERPFormulaBE();
            target = new MasterERPFormulaBS();
            AddData();
        }
        
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
        private static void AddData()
        {
            bool actual;
            ERPBE.FormulaOneText = "Basic Premium1";
            ERPBE.FormulaTwoText = "Basic Premium+TM+LCF1";
            ERPBE.FormulaDescription = "Basic Premium1";
            ERPBE.CREATED_DATE = System.DateTime.Now;
            ERPBE.CREATED_USERID = 1;
            actual = target.UpdateFormula(ERPBE);
        }
        /// <summary>
        ///A test for AddFormula with Real Data
        ///</summary>
        [TestMethod()]
        public void AddFormulaTestWithData()
        {
            bool expected = true;
            bool actual;
            ERPBE.FormulaOneText = "Basic Premium1";
            ERPBE.FormulaTwoText = "Basic Premium+TM+LCF1";
            ERPBE.FormulaDescription = "Basic Premium1";
            ERPBE.CREATED_DATE = System.DateTime.Now;
            ERPBE.CREATED_USERID = 1;
            actual = target.UpdateFormula(ERPBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for AddFormula With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddFormulaTestWithNULL()
        {
            MasterERPFormulaBE ERPBE1 = null;
            bool expected = false;
            bool actual;
            actual = target.UpdateFormula(ERPBE1);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UpdateFormula With Real Data
        ///</summary>
        [TestMethod()]
        public void UpdateFormulaTestWithData()
        {
            bool expected = true;
            bool actual;
            ERPBE.FormulaOneText = "Basic Premium";
            ERPBE.FormulaTwoText = "Basic Premium+TM+LCF";
            ERPBE.FormulaDescription = "Basic Premium";
            ERPBE.UPDATED_DATE = System.DateTime.Now;
            ERPBE.UPDATED_USERID = 1;
            actual = target.UpdateFormula(ERPBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for UpdateFormula With Null
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateFormulaTestWithNULL()
        {
            MasterERPFormulaBE ERPBE1 = null;
            bool expected = false;
            bool actual;
            actual = target.UpdateFormula(ERPBE1);
            Assert.AreEqual(expected, actual);
        }
       
        /// <summary>
        ///A test for getERPFormulas With Real Data
        ///</summary>
        [TestMethod()]
        public void getERPFormulasTestWithData()
        {
            MasterERPFormulaBS target = new MasterERPFormulaBS();
            int expected = 0;
            IList<MasterERPFormulaBE> actual;
            actual = target.getERPFormulas();
            Assert.AreNotEqual(expected, actual.Count);
        }
      
        /// <summary>
        ///A test for getERPFormulaRow
        ///</summary>
        [TestMethod()]
        public void getERPFormulaRowTest()
        {
            int FormulaID = 0; 
            MasterERPFormulaBE expected = null;
            MasterERPFormulaBE actual;
            actual = target.getERPFormulaRow(FormulaID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }
       
    }
}
