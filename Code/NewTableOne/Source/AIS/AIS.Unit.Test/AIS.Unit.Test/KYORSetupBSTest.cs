using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for KYORSetupBSTest and is intended
    ///to contain all KYORSetupBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KYORSetupBSTest
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
        ///A test for SelectData
        ///</summary>
        [TestMethod()]
        public void SelectDataTest()
        {
            KYORSetupBS target = new KYORSetupBS(); // TODO: Initialize to an appropriate value
            IList<KYORSetupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<KYORSetupBE> actual;
            actual = target.SelectData();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SaveSetupData
        ///</summary>
        [TestMethod()]
        public void SaveSetupDataTest()
        {
            KYORSetupBS target = new KYORSetupBS(); // TODO: Initialize to an appropriate value
            KYORSetupBE koBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SaveSetupData(koBE);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for KYORSetupBS Constructor
        ///</summary>
        [TestMethod()]
        public void KYORSetupBSConstructorTest()
        {
            KYORSetupBS target = new KYORSetupBS();
            
        }

        /// <summary>
        ///A test for LoadData
        ///</summary>
        [TestMethod()]
        public void LoadDataTest()
        {
            KYORSetupBS target = new KYORSetupBS(); // TODO: Initialize to an appropriate value
            int KOSetupId = 0; // TODO: Initialize to an appropriate value
            KYORSetupBE expected = null; // TODO: Initialize to an appropriate value
            KYORSetupBE actual;
            actual = target.LoadData(KOSetupId);
            Assert.AreNotEqual(expected, actual);
           
        } 
    }
}
