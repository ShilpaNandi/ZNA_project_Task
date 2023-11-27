using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for NonAisCustomerBSTest and is intended
    ///to contain all NonAisCustomerBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NonAisCustomerBSTest
    {


        private TestContext testContextInstance;
        static NonAisCustomerBE nAisCustBE;
        static NonAisCustomerBS target;
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
            target = new NonAisCustomerBS();
            AddCommonData();
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

        private static void AddCommonData()
        {
            nAisCustBE = new NonAisCustomerBE();
            nAisCustBE.fullname = "Praveentest";
            nAisCustBE.Createddate = System.DateTime.Now;
            nAisCustBE.Creteduserid = 1;
            target.save(nAisCustBE);
        }
        /// <summary>
        ///A test for save
        ///</summary>
        [TestMethod()]
        public void saveTestWithData()
        {
            bool expected = true;
            bool actual = false;
            NonAisCustomerBE NAISCBE = new NonAisCustomerBE();
            NAISCBE.fullname = "Praveen";
            NAISCBE.Createddate = System.DateTime.Now;
            NAISCBE.Creteduserid = 1;
            actual = target.save(NAISCBE);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for save
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void saveTestWithNULL()
        {
            target = new NonAisCustomerBS(); 
            NonAisCustomerBE nonaicustomerBE = null; 
            bool expected = false; 
            bool actual;
            actual = target.save(nonaicustomerBE);
            
        }
        /// <summary>
        ///A test for getNonaisCustomerlist
        ///</summary>
        [TestMethod()]
        public void getNonaisCustomerlistTest()
        {
            IList<NonAisCustomerBE> expected = null;
            IList<NonAisCustomerBE> actual;
           // actual = target.getNonaisCustomerlist();
           // Assert.AreNotEqual(expected, actual);

        }
       

      
    }
}
