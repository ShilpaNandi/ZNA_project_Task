using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for BusinessUnitOfficeBSTest and is intended
    ///to contain all BusinessUnitOfficeBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BusinessUnitOfficeBSTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
          
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
        ///A test for GetBusinessUnits
        ///</summary>
        [TestMethod()]
        public void GetBusinessUnitsTest()
        {
            BusinessUnitOfficeBS target = new BusinessUnitOfficeBS(); // TODO: Initialize to an appropriate value
            IList<BusinessUnitOfficeBE> expected = null; // TODO: Initialize to an appropriate value
            IList<BusinessUnitOfficeBE> actual;
            actual = target.GetBusinessUnits();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetBUOffList
        ///</summary>
        [TestMethod()]
        public void GetBUOffListTest()
        {
            BusinessUnitOfficeBS target = new BusinessUnitOfficeBS(); // TODO: Initialize to an appropriate value
            IList<BusinessUnitOfficeBE> expected = null; // TODO: Initialize to an appropriate value
            IList<BusinessUnitOfficeBE> actual;
            actual = target.GetBUOffList();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetBUOffForLookups
        ///</summary>
        [TestMethod()]
        public void GetBUOffForLookupsTest()
        {
            BusinessUnitOfficeBS target = new BusinessUnitOfficeBS(); // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetBUOffForLookups();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for BusinessUnitOfficeBS Constructor
        ///</summary>
        [TestMethod()]
        public void BusinessUnitOfficeBSConstructorTest()
        {
            BusinessUnitOfficeBS target = new BusinessUnitOfficeBS();
            
        }
    }
}
