using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PolicyBETest and is intended
    ///to contain all PolicyBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PolicyBETest
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
        ///A test for UPDATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATE_USER_IDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATE_USER_ID = expected;
            actual = target.UPDATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATE_DATE
        ///</summary>
        [TestMethod()]
        public void UPDATE_DATETest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATE_DATE = expected;
            actual = target.UPDATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UnlimOverrideDedtblLimitIndicator
        ///</summary>
        [TestMethod()]
        public void UnlimOverrideDedtblLimitIndicatorTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.UnlimOverrideDedtblLimitIndicator = expected;
            actual = target.UnlimOverrideDedtblLimitIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UnlimDedtblPolLimitIndicator
        ///</summary>
        [TestMethod()]
        public void UnlimDedtblPolLimitIndicatorTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.UnlimDedtblPolLimitIndicator = expected;
            actual = target.UnlimDedtblPolLimitIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TPAIndicator
        ///</summary>
        [TestMethod()]
        public void TPAIndicatorTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.TPAIndicator = expected;
            actual = target.TPAIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TPADirectIndicator
        ///</summary>
        [TestMethod()]
        public void TPADirectIndicatorTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.TPADirectIndicator = expected;
            actual = target.TPADirectIndicator;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for StateTypelookup
        ///</summary>
        [TestMethod()]
        public void StateTypelookupTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.StateTypelookup;
            
        }

        /// <summary>
        ///A test for StatesName
        ///</summary>
        [TestMethod()]
        public void StatesNameTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.StatesName = expected;
            actual = target.StatesName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ProgramPeriodID
        ///</summary>
        [TestMethod()]
        public void ProgramPeriodIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ProgramPeriodID = expected;
            actual = target.ProgramPeriodID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicySymbol
        ///</summary>
        [TestMethod()]
        public void PolicySymbolTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicySymbol = expected;
            actual = target.PolicySymbol;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyPerfectNumber
        ///</summary>
        [TestMethod()]
        public void PolicyPerfectNumberTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.PolicyPerfectNumber;
            
        }

        /// <summary>
        ///A test for PolicyNumber
        ///</summary>
        [TestMethod()]
        public void PolicyNumberTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicyNumber = expected;
            actual = target.PolicyNumber;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyModulus
        ///</summary>
        [TestMethod()]
        public void PolicyModulusTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PolicyModulus = expected;
            actual = target.PolicyModulus;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyID
        ///</summary>
        [TestMethod()]
        public void PolicyIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PolicyID = expected;
            actual = target.PolicyID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PolicyEffectiveDate
        ///</summary>
        [TestMethod()]
        public void PolicyEffectiveDateTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.PolicyEffectiveDate = expected;
            actual = target.PolicyEffectiveDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY_END_DATE
        ///</summary>
        [TestMethod()]
        public void POLICY_END_DATETest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY_END_DATE = expected;
            actual = target.POLICY_END_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POLICY_EFF_DATE
        ///</summary>
        [TestMethod()]
        public void POLICY_EFF_DATETest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.POLICY_EFF_DATE = expected;
            actual = target.POLICY_EFF_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PlanEndDate
        ///</summary>
        [TestMethod()]
        public void PlanEndDateTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.PlanEndDate = expected;
            actual = target.PlanEndDate;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ParentPolicyID
        ///</summary>
        [TestMethod()]
        public void ParentPolicyIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ParentPolicyID = expected;
            actual = target.ParentPolicyID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for OverrideDedtblLimitAmount
        ///</summary>
        [TestMethod()]
        public void OverrideDedtblLimitAmountTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.OverrideDedtblLimitAmount = expected;
            actual = target.OverrideDedtblLimitAmount;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for OtherPolicyAdjustmentAmount
        ///</summary>
        [TestMethod()]
        public void OtherPolicyAdjustmentAmountTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.OtherPolicyAdjustmentAmount = expected;
            actual = target.OtherPolicyAdjustmentAmount;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for NonConversionAmount
        ///</summary>
        [TestMethod()]
        public void NonConversionAmountTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.NonConversionAmount = expected;
            actual = target.NonConversionAmount;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LossSystemSourceID
        ///</summary>
        [TestMethod()]
        public void LossSystemSourceIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.LossSystemSourceID = expected;
            actual = target.LossSystemSourceID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LossSourceTypelookup
        ///</summary>
        [TestMethod()]
        public void LossSourceTypelookupTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.LossSourceTypelookup;
            
        }

        /// <summary>
        ///A test for LossSourceName
        ///</summary>
        [TestMethod()]
        public void LossSourceNameTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LossSourceName = expected;
            actual = target.LossSourceName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LDFIncurredNotReport
        ///</summary>
        [TestMethod()]
        public void LDFIncurredNotReportTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.LDFIncurredNotReport = expected;
            actual = target.LDFIncurredNotReport;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LDFIncurredNO63740
        ///</summary>
        [TestMethod()]
        public void LDFIncurredNO63740Test()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.LDFIncurredNO63740 = expected;
            actual = target.LDFIncurredNO63740;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LDFFactor
        ///</summary>
        [TestMethod()]
        public void LDFFactorTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.LDFFactor = expected;
            actual = target.LDFFactor;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ISMasterPEOPolicy
        ///</summary>
        [TestMethod()]
        public void ISMasterPEOPolicyTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ISMasterPEOPolicy = expected;
            actual = target.ISMasterPEOPolicy;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IsActive
        ///</summary>
        [TestMethod()]
        public void IsActiveTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.IsActive = expected;
            actual = target.IsActive;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IBNRFactor
        ///</summary>
        [TestMethod()]
        public void IBNRFactorTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.IBNRFactor = expected;
            actual = target.IBNRFactor;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DedtblProtPolMaxAmount
        ///</summary>
        [TestMethod()]
        public void DedtblProtPolMaxAmountTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.DedtblProtPolMaxAmount = expected;
            actual = target.DedtblProtPolMaxAmount;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DedtblProtPolicyStID
        ///</summary>
        [TestMethod()]
        public void DedtblProtPolicyStIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.DedtblProtPolicyStID = expected;
            actual = target.DedtblProtPolicyStID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DedTblPolicyLimitAmount
        ///</summary>
        [TestMethod()]
        public void DedTblPolicyLimitAmountTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.DedTblPolicyLimitAmount = expected;
            actual = target.DedTblPolicyLimitAmount;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for cstmrid
        ///</summary>
        [TestMethod()]
        public void cstmridTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.cstmrid = expected;
            actual = target.cstmrid;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CREATE_USER_IDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATE_USER_ID = expected;
            actual = target.CREATE_USER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATE_DATE
        ///</summary>
        [TestMethod()]
        public void CREATE_DATETest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATE_DATE = expected;
            actual = target.CREATE_DATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CoverageTypeName
        ///</summary>
        [TestMethod()]
        public void CoverageTypeNameTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CoverageTypeName = expected;
            actual = target.CoverageTypeName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CoverageTypelookup
        ///</summary>
        [TestMethod()]
        public void CoverageTypelookupTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.CoverageTypelookup;
            
        }

        /// <summary>
        ///A test for CoverageTypeID
        ///</summary>
        [TestMethod()]
        public void CoverageTypeIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CoverageTypeID = expected;
            actual = target.CoverageTypeID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ALAETypeName
        ///</summary>
        [TestMethod()]
        public void ALAETypeNameTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ALAETypeName = expected;
            actual = target.ALAETypeName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ALAETypelookup
        ///</summary>
        [TestMethod()]
        public void ALAETypelookupTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.ALAETypelookup;
            
        }

        /// <summary>
        ///A test for ALAETypeID
        ///</summary>
        [TestMethod()]
        public void ALAETypeIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ALAETypeID = expected;
            actual = target.ALAETypeID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ALAECappedAmount
        ///</summary>
        [TestMethod()]
        public void ALAECappedAmountTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.ALAECappedAmount = expected;
            actual = target.ALAECappedAmount;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustmentTypeName
        ///</summary>
        [TestMethod()]
        public void AdjustmentTypeNameTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.AdjustmentTypeName = expected;
            actual = target.AdjustmentTypeName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AdjustmentTypelookup
        ///</summary>
        [TestMethod()]
        public void AdjustmentTypelookupTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.AdjustmentTypelookup;
            
        }

        /// <summary>
        ///A test for AdjusmentTypeID
        ///</summary>
        [TestMethod()]
        public void AdjusmentTypeIDTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.AdjusmentTypeID = expected;
            actual = target.AdjusmentTypeID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Validate
        ///</summary>
        [TestMethod()]
        public void ValidateTest()
        {
            PolicyBE target = new PolicyBE(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Validate();
            Assert.AreEqual(expected, actual);
            
        }


        /// <summary>
        ///A test for PolicyBE Constructor
        ///</summary>
        [TestMethod()]
        public void PolicyBEConstructorTest()
        {
            PolicyBE target = new PolicyBE();
            
        }
    }
}
