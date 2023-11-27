using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for CombinedElementsBETest and is intended
    ///to contain all CombinedElementsBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class CombinedElementsBETest
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
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDT_USER_ID = expected;
            actual = target.UPDT_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDT_DATE
        ///</summary>
        [TestMethod()]
        public void UPDT_DATETest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDT_DATE = expected;
            actual = target.UPDT_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TOT_AMT
        ///</summary>
        [TestMethod()]
        public void TOT_AMTTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.TOT_AMT = expected;
            actual = target.TOT_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyNumber
        ///</summary>
        [TestMethod()]
        public void PolicyNumberTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicyNumber = expected;
            actual = target.PolicyNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Policy
        ///</summary>
        [TestMethod()]
        public void PolicyTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Policy = expected;
            actual = target.Policy;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERTEXT
        ///</summary>
        [TestMethod()]
        public void PERTEXTTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PERTEXT = expected;
            actual = target.PERTEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Perlookuptype
        ///</summary>
        [TestMethod()]
        public void PerlookuptypeTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.Perlookuptype;
            
        }

        /// <summary>
        ///A test for PerfectPolicynum
        ///</summary>
        [TestMethod()]
        public void PerfectPolicynumTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            PolicyBE actual;
            actual = target.PerfectPolicynum;
            
        }

        /// <summary>
        ///A test for PER
        ///</summary>
        [TestMethod()]
        public void PERTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PER = expected;
            actual = target.PER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXPOSURETYPE
        ///</summary>
        [TestMethod()]
        public void EXPOSURETYPETest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.EXPOSURETYPE = expected;
            actual = target.EXPOSURETYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Expolookuptype
        ///</summary>
        [TestMethod()]
        public void ExpolookuptypeTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.Expolookuptype;
            
        }

        /// <summary>
        ///A test for EXPO_TYP_ID
        ///</summary>
        [TestMethod()]
        public void EXPO_TYP_IDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.EXPO_TYP_ID = expected;
            actual = target.EXPO_TYP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DVSR_NBR_ID
        ///</summary>
        [TestMethod()]
        public void DVSR_NBR_IDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.DVSR_NBR_ID = expected;
            actual = target.DVSR_NBR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_USR_ID
        ///</summary>
        [TestMethod()]
        public void CRTE_USR_IDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CRTE_USR_ID = expected;
            actual = target.CRTE_USR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_DT
        ///</summary>
        [TestMethod()]
        public void CRTE_DTTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CRTE_DT = expected;
            actual = target.CRTE_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COML_AGMT_ID
        ///</summary>
        [TestMethod()]
        public void COML_AGMT_IDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.COML_AGMT_ID = expected;
            actual = target.COML_AGMT_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COMB_ELEMTS_SETUP_ID
        ///</summary>
        [TestMethod()]
        public void COMB_ELEMTS_SETUP_IDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.COMB_ELEMTS_SETUP_ID = expected;
            actual = target.COMB_ELEMTS_SETUP_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AUDIT_EXPO_AMT
        ///</summary>
        [TestMethod()]
        public void AUDIT_EXPO_AMTTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.AUDIT_EXPO_AMT = expected;
            actual = target.AUDIT_EXPO_AMT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJ_RT
        ///</summary>
        [TestMethod()]
        public void ADJ_RTTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ADJ_RT = expected;
            actual = target.ADJ_RT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            CombinedElementsBE target = new CombinedElementsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CombinedElementsBE Constructor
        ///</summary>
        [TestMethod()]
        public void CombinedElementsBEConstructorTest()
        {
            CombinedElementsBE target = new CombinedElementsBE();
            
        }
    }
}
