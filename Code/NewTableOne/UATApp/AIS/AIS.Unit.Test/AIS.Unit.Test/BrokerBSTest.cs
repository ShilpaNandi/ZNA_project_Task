using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for BrokerBSTest and is intended
    ///to contain all BrokerBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BrokerBSTest
    {


        private TestContext testContextInstance;
        static BrokerBE brkBE;
        

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

        private static void AddBroker()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            brkBE = new BrokerBE();
            brkBE.CONTACT_TYPE_ID = 233;
            brkBE.FULL_NAME = "TestBrokerIni";
            brkBE.CREATE_USER_ID = 1;
            brkBE.CREATE_DATE = System.DateTime.Now;
            brkBE.ACTV_IND = true;
            target.Update(brkBE);        

        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            AddBroker();
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
        ///A test for Add with Data
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            bool expected = true;
            bool actual;
            BrokerBE BrkBE = new BrokerBE();
            BrkBE.CONTACT_TYPE_ID = 233;
            BrkBE.FULL_NAME = "TestBroker";
            BrkBE.CREATE_USER_ID = 1;
            BrkBE.CREATE_DATE = System.DateTime.Now;
            BrkBE.ACTV_IND = true;
            actual = (new BrokerBS()).Update(BrkBE);
            Assert.AreEqual(expected, actual);


        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value           
            bool expected = true; // TODO: Initialize to an appropriate value
                        
            brkBE.CONTACT_TYPE_ID = 233;
            brkBE.FULL_NAME = "TestBroker1";
            brkBE.UPDATE_USER_ID = 1;
            brkBE.UPDATE_DATE = System.DateTime.Now;
            bool actual;
            actual = target.Update(brkBE);
            Assert.AreEqual(expected, actual);            
        }

        /// <summary>
        ///A test for IsContactNameExists for Existing Record
        ///</summary>
        [TestMethod()]
        public void IsContactNameExistsTest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            string contactName = "TestBroker"; // TODO: Initialize to an appropriate value
            int contactTypeID = 233; // TODO: Initialize to an appropriate value
            int extOrg = 1;
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsContactNameExists(contactName, contactTypeID, extOrg);
            Assert.AreEqual(expected, actual);            
        }

        /// <summary>
        ///A test for IsContactNameExists for no Existing Record
        ///</summary>
        [TestMethod()]
        public void IsContactNameExistsTestWithNULL()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            string contactName = string.Empty; // TODO: Initialize to an appropriate value
            int contactTypeID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsContactNameExists(contactName, contactTypeID,0);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetOnlyBrokersForLookups
        ///</summary>
        [TestMethod()]
        public void GetOnlyBrokersForLookupsTest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetOnlyBrokersForLookups();
            Assert.AreNotEqual(expected, actual.Count);           
        }

        /// <summary>
        ///A test for GetBrokersForLookups
        ///</summary>
        [TestMethod()]
        public void GetBrokersForLookupsTest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetBrokersForLookups();
            Assert.AreNotEqual(expected, actual.Count);           
        }

        /// <summary>
        ///A test for GetBrokerData
        ///</summary>
        [TestMethod()]
        public void GetBrokerDataTest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<BrokerBE> actual;
            actual = target.GetBrokerData();
            Assert.AreNotEqual(expected, actual.Count);            
        }

        /// <summary>
        ///A test for getBroker
        ///</summary>
        [TestMethod()]
        public void getBrokerTest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            int brokerID = 0; // TODO: Initialize to an appropriate value
            BrokerBE expected = null; // TODO: Initialize to an appropriate value
            BrokerBE actual;
            actual = target.getBroker(brokerID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetBrokersList
        ///</summary>
        [TestMethod()]
        public void GetBrokersListTest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            IList<BrokerBE> expected = null; // TODO: Initialize to an appropriate value
            IList<BrokerBE> actual;
            actual = target.GetBrokersList();
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetOnlyBrokersForLookupsNA
        ///</summary>
        [TestMethod()]
        public void GetOnlyBrokersForLookupsNATest()
        {
            BrokerBS target = new BrokerBS(); // TODO: Initialize to an appropriate value
            IList<LookupBE > expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetOnlyBrokersForLookupsNA();
            Assert.AreNotEqual(expected, actual);

        }
       
    }
}
