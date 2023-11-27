using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for BLAccessTest and is intended
    ///to contain all BLAccessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BLAccessTest
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
        ///A test for ListviewGetPolicyDataforCustLBA
        ///</summary>
        [TestMethod()]
        public void ListviewGetPolicyDataforCustLBATest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.ListviewGetPolicyDataforCustLBA(ProgramPeriodID, AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ListviewGetPolicyDataforCust
        ///</summary>
        [TestMethod()]
        public void ListviewGetPolicyDataforCustTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.ListviewGetPolicyDataforCust(ProgramPeriodID, AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ListviewGetPolicyData
        ///</summary>
        [TestMethod()]
        public void ListviewGetPolicyDataTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.ListviewGetPolicyData(ProgramPeriodID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetRelatedAccountData
        ///</summary>
        [TestMethod()]
        public void GetRelatedAccountDataTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.GetRelatedAccountData(AccountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetPolicyData
        ///</summary>
        [TestMethod()]
        public void GetPolicyDataTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            IList<PolicyBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PolicyBE> actual;
            actual = target.GetPolicyData(ProgramPeriodID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetLossSource
        ///</summary>
        [TestMethod()]
        public void GetLossSourceTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.GetLossSource();
            Assert.AreNotEqual(0, actual.Count);
            
        }

        /// <summary>
        ///A test for GetLookUpID
        ///</summary>
        [TestMethod()]
        public void GetLookUpIDTest1()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpName = string.Empty; // TODO: Initialize to an appropriate value
            string lookUpType = string.Empty; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.GetLookUpID(lookUpName, lookUpType);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetLookUpID
        ///</summary>
        [TestMethod()]
        public void GetLookUpIDTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpName = string.Empty; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.GetLookUpID(lookUpName);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetLookUpID
        ///</summary>
        [TestMethod()]
        public void GetLookUpIDTestwithData()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpName = "C & RM"; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.GetLookUpID(lookUpName);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetLookUpDictionary
        ///</summary>
        [TestMethod()]
        public void GetLookUpDictionaryTest1WithNull()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpTypeName = string.Empty; // TODO: Initialize to an appropriate value
            IDictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, string> actual;
            actual = target.GetLookUpDictionary(lookUpTypeName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetLookUpDictionary
        ///</summary>
        [TestMethod()]
        public void GetLookUpDictionaryTest1WithData()
        {
            BLAccess target = new BLAccess();
            string lookUpTypeName = "STATE";
            IDictionary<int, string> actual;
            actual = target.GetLookUpDictionary(lookUpTypeName);
            Assert.AreNotEqual(0, actual.Count);
        }

        /// <summary>
        ///A test for GetLookUpDictionary
        ///</summary>
        [TestMethod()]
        public void GetLookUpDictionaryTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpTypeName = string.Empty; // TODO: Initialize to an appropriate value
            string attribute = string.Empty; // TODO: Initialize to an appropriate value
            IDictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, string> actual;
            actual = target.GetLookUpDictionary(lookUpTypeName, attribute);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetLookUpDictionary
        ///</summary>
        [TestMethod()]
        public void GetLookUpDictionaryTestWithData()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpTypeName = "CONTACT TYPE"; // TODO: Initialize to an appropriate value
            string attribute = "I"; // TODO: Initialize to an appropriate value
            IDictionary<int, string> actual;
            actual = target.GetLookUpDictionary(lookUpTypeName, attribute);
            Assert.AreNotEqual(0, actual.Count);

        }

        
        /// <summary>
        ///A test for GetLookUpDataWithoutSelect
        ///</summary>
        [TestMethod()]
        public void GetLookUpDataWithoutSelectTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpTypeName = string.Empty; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetLookUpDataWithoutSelect(lookUpTypeName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetLookUpDataWithoutSelect
        ///</summary>
        [TestMethod()]
        public void GetLookUpDataWithoutSelectTestWithData()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpTypeName = "CONTACT TYPE"; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetLookUpDataWithoutSelect(lookUpTypeName);
            Assert.AreNotEqual(0, actual.Count);
        }

        /// <summary>
        ///A test for GetLookUpData
        ///</summary>
        [TestMethod()]
        public void GetLookUpDataTest2()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int lookUpTypeID = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetLookUpData(lookUpTypeID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetLookUpData
        ///</summary>
        [TestMethod()]
        public void GetLookUpDataTest2WithData()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int lookUpTypeID = 17; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetLookUpData(lookUpTypeID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetLookUpData
        ///</summary>
        [TestMethod()]
        public void GetLookUpDataTest1()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpTypeName = string.Empty; // TODO: Initialize to an appropriate value
            string attribute = string.Empty; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetLookUpData(lookUpTypeName, attribute);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetLookUpData
        ///</summary>
        [TestMethod()]
        public void GetLookUpDataTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            string lookUpTypeName = string.Empty; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetLookUpData(lookUpTypeName);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetLBAAdjustmentTypes
        ///</summary>
        [TestMethod()]
        public void GetLBAAdjustmentTypesTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.GetLBAAdjustmentTypes();
            Assert.AreNotEqual(expected, actual);
            
        }

            /// <summary>
        ///A test for GetInsContact
        ///</summary>
        [TestMethod()]
        public void GetInsContactTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> expected = null; // TODO: Initialize to an appropriate value
            IList<CustomerContactBE> actual;
            actual = target.GetInsContact(AccountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetDepState
        ///</summary>
        [TestMethod()]
        public void GetDepStateTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.GetDepState();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetCoverageTypes
        ///</summary>
        [TestMethod()]
        public void GetCoverageTypesTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.GetCoverageTypes();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetBktcyBuyout
        ///</summary>
        [TestMethod()]
        public void GetBktcyBuyoutTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.GetBktcyBuyout();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetAllLSICustomers
        ///</summary>
        [TestMethod()]
        public void GetAllLSICustomersTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            IList<LSIAllCustomersBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LSIAllCustomersBE> actual;
            actual = target.GetAllLSICustomers();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetALAEType
        ///</summary>
        [TestMethod()]
        public void GetALAETypeTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.GetALAEType();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetAdjustmentTypes
        ///</summary>
        [TestMethod()]
        public void GetAdjustmentTypesTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            Dictionary<int, string> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<int, string> actual;
            actual = target.GetAdjustmentTypes();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetAccountData
        ///</summary>
        [TestMethod()]
        public void GetAccountDataTest()
        {
            BLAccess target = new BLAccess(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.GetAccountData();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BLAccess Constructor
        ///</summary>
        [TestMethod()]
        public void BLAccessConstructorTest()
        {
            BLAccess target = new BLAccess();
            
        }
    }
}
