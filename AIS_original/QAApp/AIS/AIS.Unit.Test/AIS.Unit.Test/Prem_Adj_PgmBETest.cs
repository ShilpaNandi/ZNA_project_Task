using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for Prem_Adj_PgmBETest and is intended
    ///to contain all Prem_Adj_PgmBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class Prem_Adj_PgmBETest
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
        ///A test for UpdatedUser_ID
        ///</summary>
        [TestMethod()]
        public void UpdatedUser_IDTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UpdatedUser_ID = expected;
            actual = target.UpdatedUser_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UpdatedDate
        ///</summary>
        [TestMethod()]
        public void UpdatedDateTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UpdatedDate = expected;
            actual = target.UpdatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QUALITYCONTROL_PERSON_ID
        ///</summary>
        [TestMethod()]
        public void QUALITYCONTROL_PERSON_IDTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.QUALITYCONTROL_PERSON_ID = expected;
            actual = target.QUALITYCONTROL_PERSON_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QUALITYCONTROL_DATE
        ///</summary>
        [TestMethod()]
        public void QUALITYCONTROL_DATETest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.QUALITYCONTROL_DATE = expected;
            actual = target.QUALITYCONTROL_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for QUALITYCOMMENT_TEXT
        ///</summary>
        [TestMethod()]
        public void QUALITYCOMMENT_TEXTTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.QUALITYCOMMENT_TEXT = expected;
            actual = target.QUALITYCOMMENT_TEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PremiumAdjustmentProgramID
        ///</summary>
        [TestMethod()]
        public void PremiumAdjustmentProgramIDTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PremiumAdjustmentProgramID = expected;
            actual = target.PremiumAdjustmentProgramID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PersonName
        ///</summary>
        [TestMethod()]
        public void PersonNameTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PersonName = expected;
            actual = target.PersonName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MasterEarnedRetroPremiumFormulaID
        ///</summary>
        [TestMethod()]
        public void MasterEarnedRetroPremiumFormulaIDTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.MasterEarnedRetroPremiumFormulaID = expected;
            actual = target.MasterEarnedRetroPremiumFormulaID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTOMERID
        ///</summary>
        [TestMethod()]
        public void CUSTOMERIDTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTOMERID = expected;
            actual = target.CUSTOMERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedUser_ID
        ///</summary>
        [TestMethod()]
        public void CreatedUser_IDTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CreatedUser_ID = expected;
            actual = target.CreatedUser_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CreatedDate
        ///</summary>
        [TestMethod()]
        public void CreatedDateTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CreatedDate = expected;
            actual = target.CreatedDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Prem_Adj_PgmBE Constructor
        ///</summary>
        [TestMethod()]
        public void Prem_Adj_PgmBEConstructorTest()
        {
            Prem_Adj_PgmBE target = new Prem_Adj_PgmBE();
            
        }
    }
}
