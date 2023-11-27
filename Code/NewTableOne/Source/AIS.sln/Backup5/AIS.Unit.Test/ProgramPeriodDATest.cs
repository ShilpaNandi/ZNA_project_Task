using ZurichNA.AIS.DAL.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Linq;
using System.Collections.Generic;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for ProgramPeriodDATest and is intended
    ///to contain all ProgramPeriodDATest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramPeriodDATest
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
        ///A test for GetProgramPeriodList
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodListTest()
        {
            ProgramPeriodDA target = new ProgramPeriodDA(); // TODO: Initialize to an appropriate value
            int accountID = 0; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodListBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodListBE> actual;
            actual = target.GetProgramPeriodList(accountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetProgramPeriodData
        ///</summary>
        [TestMethod()]
        public void GetProgramPeriodDataTest()
        {
            ProgramPeriodDA target = new ProgramPeriodDA(); // TODO: Initialize to an appropriate value
            int accountID = 0; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> expected = null; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetProgramPeriodData(accountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetCustomerDetails
        ///</summary>
        [TestMethod()]
        public void GetCustomerDetailsTest()
        {
            ProgramPeriodDA target = new ProgramPeriodDA(); // TODO: Initialize to an appropriate value
            int CustomerID = 0; // TODO: Initialize to an appropriate value
            int BUOfficeID = 0; // TODO: Initialize to an appropriate value
            int BrokerID = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<ProgramPeriodBE> actual;
            actual = target.GetCustomerDetails(CustomerID, BUOfficeID, BrokerID);
            Assert.AreEqual(expected, actual.Count);
            
        }

  
  
 

        /// <summary>
        ///A test for ProgramPeriodDA Constructor
        ///</summary>
        [TestMethod()]
        public void ProgramPeriodDAConstructorTest()
        {
            ProgramPeriodDA target = new ProgramPeriodDA();
            
        }
    }
}

