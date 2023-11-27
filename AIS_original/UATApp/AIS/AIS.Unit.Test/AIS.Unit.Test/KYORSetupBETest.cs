using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for KYORSetupBETest and is intended
    ///to contain all KYORSetupBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class KYORSetupBETest
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
        ///A test for UPDT_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDT_USER_IDTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDT_USER_ID = expected;
            actual = target.UPDT_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDT_DT
        ///</summary>
        [TestMethod()]
        public void UPDT_DTTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDT_DT = expected;
            actual = target.UPDT_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for OR_FCTR_RT
        ///</summary>
        [TestMethod()]
        public void OR_FCTR_RTTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.OR_FCTR_RT = expected;
            actual = target.OR_FCTR_RT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for KY_OR_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void KY_OR_SETUP_IDTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.KY_OR_SETUP_ID = expected;
            actual = target.KY_OR_SETUP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for KY_FCTR_RT
        ///</summary>
        [TestMethod()]
        public void KY_FCTR_RTTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.KY_FCTR_RT = expected;
            actual = target.KY_FCTR_RT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EFF_DT
        ///</summary>
        [TestMethod()]
        public void EFF_DTTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.EFF_DT = expected;
            actual = target.EFF_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CRTE_USER_IDTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CRTE_USER_ID = expected;
            actual = target.CRTE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_DT
        ///</summary>
        [TestMethod()]
        public void CRTE_DTTest()
        {
            KYORSetupBE target = new KYORSetupBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CRTE_DT = expected;
            actual = target.CRTE_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for KYORSetupBE Constructor
        ///</summary>
        [TestMethod()]
        public void KYORSetupBEConstructorTest()
        {
            KYORSetupBE target = new KYORSetupBE();
            
        }
    }
}
