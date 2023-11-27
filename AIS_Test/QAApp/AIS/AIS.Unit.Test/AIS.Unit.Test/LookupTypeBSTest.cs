using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for LookupTypeBSTest and is intended
    ///to contain all LookupTypeBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LookupTypeBSTest
    {
        private TestContext testContextInstance;
        static LookupTypeBE lkupTypeBE;
        static LookupTypeBS target;


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
            target = new LookupTypeBS();
            AddData();
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
        /// a method for adding New Record when the classis initiated
        /// </summary>
        private static void AddData()
        {
            lkupTypeBE = new LookupTypeBE();
            lkupTypeBE.LOOKUPTYPE_NAME = "STATE1";
            lkupTypeBE.CREATED_DATE = System.DateTime.Now;
            lkupTypeBE.CREATED_USER_ID = 1;
            target.Update(lkupTypeBE);
        }
        /// <summary>
        /// a Test for add With Real Data
        /// </summary>

        [TestMethod()]
        public void AddTestWithData()
        {
            bool expected = true;
            bool actual = false;
            LookupTypeBE lkBE = new LookupTypeBE();
            lkBE.LOOKUPTYPE_NAME = "STATE2";
            lkBE.CREATED_DATE = System.DateTime.Now;
            lkBE.CREATED_USER_ID = 1;
            actual = target.Update(lkBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// a Test for add With Real NULL
        /// </summary>

        [TestMethod()]
        public void AddTestWithNULL()
        {
            LookupTypeBS target = new LookupTypeBS();
            LookupTypeBE lkupTypeBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(lkupTypeBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for Update With Real Data
        /// </summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            bool expected = true;
            bool actual;
            lkupTypeBE.LOOKUPTYPE_NAME = "STATEUpdate";
            lkupTypeBE.UPDATED_DATE = System.DateTime.Now;
            lkupTypeBE.UPDATED_USER_ID = 1;
            actual = target.Update(lkupTypeBE);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for Update With NULL
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            LookupTypeBS target = new LookupTypeBS();
            LookupTypeBE lkupTypeBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(lkupTypeBE);
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        /// A test for IsExistsLookupTypeName With Real Data
        /// </summary>
        public void IsExistsLookupTypeNameTestWithData()
        {
            string lookupTypeName = "STATE";
            //int LookupTypeID = 1;
            bool expected = true;
            bool actual;
            actual = target.IsExistsLookupTypeName(lookupTypeName, lkupTypeBE.LOOKUPTYPE_ID);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for IsExistsLookupTypeName With Null Values
        ///</summary>
        [TestMethod()]
        public void IsExistsLookupTypeNameTestWithNULL()
        {
            LookupTypeBS target = new LookupTypeBS();
            string lookupTypeName = string.Empty;
            int LookupTypeID = 0;
            bool expected = false;
            bool actual;
            actual = target.IsExistsLookupTypeName(lookupTypeName, LookupTypeID);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for getLookupTypeData With Real Data
        ///</summary>
        [TestMethod()]
        public void getLookupTypeDataTest()
        {
            int expected = 0;
            IList<LookupTypeBE> actual;
            actual = target.getLookupTypeData();
            Assert.AreNotEqual(expected, actual.Count);

        }

        ///// <summary>
        /////A test for getLookupTypeData With NULL
        /////</summary>
        //[TestMethod()]
        //public void getLookupTypeDataTestWithNULL()
        //{
        //    LookupTypeBS target = new LookupTypeBS(); 
        //    int expected = 0; 
        //    IList<LookupTypeBE> actual;
        //    actual = target.getLookupTypeData();
        //    Assert.AreEqual(expected, actual.Count);

        //}

        /// <summary>
        ///A test for getLkupTypeRow With Real Data
        ///</summary>
        [TestMethod()]
        public void getLkupTypeRowTestWithData()
        {
            LookupTypeBE expected = null;
            LookupTypeBE actual;
            actual = target.getLkupTypeRow(lkupTypeBE.LOOKUPTYPE_ID);
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getLkupTypeRow With Null
        ///</summary>
        [TestMethod()]
        public void getLkupTypeRowTestWithNULL()
        {
            LookupTypeBS target = new LookupTypeBS();
            int LookupTypeID = 0;
            LookupTypeBE expected = null;
            LookupTypeBE actual;
            actual = target.getLkupTypeRow(LookupTypeID);
            Assert.AreNotEqual(expected, actual);

        }

    }
}
