using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Adj_Paramet_PolBSTest and is intended
    ///to contain all Adj_Paramet_PolBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Adj_Paramet_PolBSTest
    {


        private TestContext testContextInstance;
        static AdjustmentParameterPolicyBE AdjPolBE;
        static Adj_Paramet_PolBS target;

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
            target = new Adj_Paramet_PolBS();
            AddPolData();
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
        /// a method for adding New Record when the class is initiated
        /// </summary>
        private static void AddPolData()
        {
            AdjPolBE = new AdjustmentParameterPolicyBE();
            AdjPolBE.PrmadjPRgmID = 9;
            AdjPolBE.custmrID = 2;
            AdjPolBE.adj_paramet_setup_id = 37;
            AdjPolBE.coml_agmt_id = 2;
            AdjPolBE.CREATE_DATE = System.DateTime.Now;
            AdjPolBE.CREATE_USER_ID = 1;
            target.Update(AdjPolBE);
        }

        /// <summary>
        /// a Test for add With Real Data
        /// </summary>
        [TestMethod()]
        public void AddTestPolWithData()
        {
            bool expected = true;
            bool actual = false;
            AdjustmentParameterPolicyBE AdjParmPolBE = new AdjustmentParameterPolicyBE();
            AdjParmPolBE.PrmadjPRgmID = 6;
            AdjParmPolBE.custmrID = 1;
            AdjParmPolBE.adj_paramet_setup_id = 38;
            AdjParmPolBE.coml_agmt_id = 4;
            AdjParmPolBE.CREATE_DATE = System.DateTime.Now;
            AdjParmPolBE.CREATE_USER_ID = 1;
            actual = target.Update(AdjParmPolBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// a Test for add With NULL Data
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestPolWithNULL()
        {
            Adj_Paramet_PolBS target = new Adj_Paramet_PolBS();
            AdjustmentParameterPolicyBE AdjParmPolBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjParmPolBE);
            Assert.AreEqual(expected, actual);

        }

       
        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestPolWithNULL()
        {
            Adj_Paramet_PolBS target = new Adj_Paramet_PolBS();
            AdjustmentParameterPolicyBE AdjParmPolBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(AdjParmPolBE);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getAdjParamtrPol with Null Data
        ///</summary>
        [TestMethod()]
        public void getAdjParamtrPolNullTest()
        {
            Adj_Paramet_PolBS target = new Adj_Paramet_PolBS();
            int AdjParmSetupID = 0;
            IList<AdjustmentParameterPolicyBE> expected = null;
            IList<AdjustmentParameterPolicyBE> actual;
            actual = target.getAdjParamtrPol(AdjParmSetupID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getLBAAdjParamtrDtls with Real Data
        ///</summary>
        [TestMethod()]
        public void getAdjParamtrPolTest()
        {
            Adj_Paramet_PolBS target = new Adj_Paramet_PolBS();
            int expected = 0;
            int AdjParameterSetupID = 37;
            IList<AdjustmentParameterPolicyBE> actual;
            actual = target.getAdjParamtrPol(AdjParameterSetupID);
            Assert.AreNotEqual(expected, actual.Count);
            //
        }


        /// <summary>
        ///A test for deletePol with Null data
        ///</summary>
        [TestMethod()]
        public void deletePolNullTest()
        {
            Adj_Paramet_PolBS target = new Adj_Paramet_PolBS();
            int custmrID = 0;
            int adjParmSetupID = 0;
            bool expected = false;
            bool actual;
            actual = target.deletePol(custmrID, adjParmSetupID);
            Assert.AreEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for deletePol with real data
        ///</summary>
        [TestMethod()]
        public void deletePolTest()
        {
            Adj_Paramet_PolBS target = new Adj_Paramet_PolBS(); 
            int custmrID = 6; 
            int adjParmSetupID = 38; 
            bool expected = true; 
            bool actual;
            actual = target.deletePol(custmrID, adjParmSetupID);
            Assert.AreEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for Adj_Paramet_PolBS Constructor
        ///</summary>
        [TestMethod()]
        public void Adj_Paramet_PolBSConstructorTest()
        {
            Adj_Paramet_PolBS target = new Adj_Paramet_PolBS();
            //
        }
    }
}
