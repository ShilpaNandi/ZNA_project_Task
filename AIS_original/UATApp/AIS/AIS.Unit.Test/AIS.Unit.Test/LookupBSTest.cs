using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for LookupBSTest and is intended
    ///to contain all LookupBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LookupBSTest
    {


        private TestContext testContextInstance;
        static LookupBE lkBE;
        static LookupBS lkBS;

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
            lkBS = new LookupBS();
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
        ///A test for States
        ///</summary>
        [TestMethod()]
        public void StatesTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.States;
        }
        /// <summary>
        ///A test for ProgramType
        ///</summary>
        [TestMethod()]
        public void ProgramTypeTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.ProgramType;
        }
        /// <summary>
        ///A test for PrimaryContact
        ///</summary>
        [TestMethod()]
        public void PrimaryContactTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.PrimaryContact;
        }
        /// <summary>
        ///A test for LossSource
        ///</summary>
        [TestMethod()]
        public void LossSourceTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.LossSource;
        }
        /// <summary>
        ///A test for LBAAdjustmentType
        ///</summary>
        [TestMethod()]
        public void LBAAdjustmentTypeTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.LBAAdjustmentType;
        }
        /// <summary>
        ///A test for CoverageType
        ///</summary>
        [TestMethod()]
        public void CoverageTypeTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.CoverageType;
        }
        /// <summary>
        ///A test for BktcyBuyout
        ///</summary>
        [TestMethod()]
        public void BktcyBuyoutTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.BktcyBuyout;
        }
        /// <summary>
        ///A test for ALAEType
        ///</summary>
        [TestMethod()]
        public void ALAETypeTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.ALAEType;
        }
        /// <summary>
        ///A test for AdjustmentType
        ///</summary>
        [TestMethod()]
        public void AdjustmentTypeTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.AdjustmentType;
        }
        /// <summary>
        ///A test for Update
        ///</summary>
        //[TestMethod()]
        //[ExpectedException(typeof(System.NullReferenceException))]
        //public void UpdateTest()
        //{
        //    LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
        //    LookupBE lkupBE = null; // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = target.Update(lkupBE);
        //    Assert.AreEqual(expected, actual);
        //}
        /// <summary>
        ///A test for IsExistsLookupName
        ///</summary>
        [TestMethod()]
        public void IsExistsLookupNameTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            int LookupID = 0; // TODO: Initialize to an appropriate value
            int LookupTypeID = 0; // TODO: Initialize to an appropriate value
            string lookupName = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsExistsLookupName(LookupID, LookupTypeID, lookupName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getLookupName
        ///</summary>
        [TestMethod()]
        public void getLookupNameTest()
        {
            object Key = null; // TODO: Initialize to an appropriate value
            Dictionary<int, string> Table = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = LookupBS.getLookupName(Key, Table);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getLookupData
        ///</summary>
        [TestMethod()]
        public void getLookupDataTest1()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.getLookupData();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getLookupData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ZurichNA.AIS.Business.Logic.dll")]
        public void getLookupDataTest()
        {
            LookupBS_Accessor target = new LookupBS_Accessor(); // TODO: Initialize to an appropriate value
            string LookupTypeName = string.Empty; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.getLookupData(LookupTypeName);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getLkupRow
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void getLkupRowTest()
        {
            LookupBS target = new LookupBS(); // TODO: Initialize to an appropriate value
            int LookupID = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.getLkupRow(LookupID);
            Assert.AreEqual(expected, actual.LookUpID);
        }

        /// <summary>
        ///A test for LookupBS Constructor
        ///</summary>
        [TestMethod()]
        public void LookupBSConstructorTest()
        {
            LookupBS target = new LookupBS();
        }
        /// <summary>
        /// Function to add a new Record in to Lkup Table
        /// </summary>
        [TestMethod()]
        public void AddLookup()
        {
            bool expected = true;
            bool actual;
            lkBE = new LookupBE();
            lkBE.LookUpName = "aaa";
            lkBE.LookUpTypeID = 1;
            lkBE.Attribute1 = "I";
            lkBE.Created_Date = System.DateTime.Now;
            lkBE.Created_UserID = 1;
            actual = lkBS.Update(lkBE);
            Assert.AreEqual(expected, actual);
           
        }
        /// <summary>
        /// Function to Update a Record in Lkup Table
        /// </summary>
        [TestMethod()]
        public void UpdateLookup()
        {
            bool expected = true;
            bool actual;
            LookupBE lkBE1 = new LookupBE();
            lkBE1.LookUpName = "aaa";
            lkBE1.LookUpTypeID = 1;
            lkBE1.Attribute1 = "I";
            lkBE1.Created_Date = System.DateTime.Now;
            lkBE1.Created_UserID = 1;
            actual = lkBS.Update(lkBE1);
            Assert.AreEqual(expected, actual);

        }
    }
}
