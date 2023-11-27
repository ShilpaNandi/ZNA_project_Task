using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for MasterERPFormulaBETest and is intended
    ///to contain all MasterERPFormulaBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class MasterERPFormulaBETest
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
        ///A test for UPDATED_USERID
        ///</summary>
        [TestMethod()]
        public void UPDATED_USERIDTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATED_USERID = expected;
            actual = target.UPDATED_USERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATED_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATED_DATETest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATED_DATE = expected;
            actual = target.UPDATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IsActive
        ///</summary>
        [TestMethod()]
        public void IsActiveTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.IsActive = expected;
            actual = target.IsActive;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FormulaTwoText
        ///</summary>
        [TestMethod()]
        public void FormulaTwoTextTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FormulaTwoText = expected;
            actual = target.FormulaTwoText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FormulaOneText
        ///</summary>
        [TestMethod()]
        public void FormulaOneTextTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FormulaOneText = expected;
            actual = target.FormulaOneText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FormulaID
        ///</summary>
        [TestMethod()]
        public void FormulaIDTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.FormulaID = expected;
            actual = target.FormulaID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FormulaDescription
        ///</summary>
        [TestMethod()]
        public void FormulaDescriptionTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FormulaDescription = expected;
            actual = target.FormulaDescription;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_USERID
        ///</summary>
        [TestMethod()]
        public void CREATED_USERIDTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATED_USERID = expected;
            actual = target.CREATED_USERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATED_DATE
        ///</summary>
        [TestMethod()]
        public void CREATED_DATETest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATED_DATE = expected;
            actual = target.CREATED_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MasterERPFormulaBE Constructor
        ///</summary>
        [TestMethod()]
        public void MasterERPFormulaBEConstructorTest()
        {
            MasterERPFormulaBE target = new MasterERPFormulaBE();
            
        }
    }
}
