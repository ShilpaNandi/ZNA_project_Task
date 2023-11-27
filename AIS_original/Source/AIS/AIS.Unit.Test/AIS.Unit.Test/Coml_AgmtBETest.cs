using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Coml_AgmtBETest and is intended
    ///to contain all Coml_AgmtBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class Coml_AgmtBETest
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
        ///A test for Prem_Adj_Prg_ID
        ///</summary>
        [TestMethod()]
        public void Prem_Adj_Prg_IDTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Prem_Adj_Prg_ID = expected;
            actual = target.Prem_Adj_Prg_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY
        ///</summary>
        [TestMethod()]
        public void POLICYTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY = expected;
            actual = target.POLICY;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Pol_Sym_Txt
        ///</summary>
        [TestMethod()]
        public void Pol_Sym_TxtTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Pol_Sym_Txt = expected;
            actual = target.Pol_Sym_Txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Pol_Nbr_Txt
        ///</summary>
        [TestMethod()]
        public void Pol_Nbr_TxtTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Pol_Nbr_Txt = expected;
            actual = target.Pol_Nbr_Txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Pol_Mod_Txt
        ///</summary>
        [TestMethod()]
        public void Pol_Mod_TxtTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Pol_Mod_Txt = expected;
            actual = target.Pol_Mod_Txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PlannedEndDate
        ///</summary>
        [TestMethod()]
        public void PlannedEndDateTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.PlannedEndDate = expected;
            actual = target.PlannedEndDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Customer_ID
        ///</summary>
        [TestMethod()]
        public void Customer_IDTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Customer_ID = expected;
            actual = target.Customer_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUserID
        ///</summary>
        [TestMethod()]
        public void CreatedUserIDTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CreatedUserID = expected;
            actual = target.CreatedUserID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedDate
        ///</summary>
        [TestMethod()]
        public void CreatedDateTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Comm_Agr_ID
        ///</summary>
        [TestMethod()]
        public void Comm_Agr_IDTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Comm_Agr_ID = expected;
            actual = target.Comm_Agr_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Coml_AgmtBE Constructor
        ///</summary>
        [TestMethod()]
        public void Coml_AgmtBEConstructorTest()
        {
            Coml_AgmtBE target = new Coml_AgmtBE();
            
        }
    }
}
