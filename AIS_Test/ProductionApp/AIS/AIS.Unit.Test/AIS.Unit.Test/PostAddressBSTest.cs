using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for PostAddressBSTest and is intended
    ///to contain all PostAddressBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PostAddressBSTest
    {

        static PersonBE personBE;
        static PostalAddressBE PostBE;
        static PersonBS target;
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
            target = new PersonBS();
            AddPersonData();
            AddPostalData();
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
        /// a method for adding New Record in Person Table when the classis initiated 
        /// </summary>
        private static void AddPersonData()
        {
            personBE = new PersonBE();
            personBE.SURNAME = "aaaa";
            personBE.FORENAME = "bbbbb";
            personBE.USERID = "XCSNQK5";
            personBE.CREATEDDATE = System.DateTime.Now;
            personBE.CREATEDUSER_ID = 1;
            (new PersonBS()).Update(personBE);
        }
        /// <summary>
        /// a method for adding New Record in Person Table when the classis initiated 
        /// </summary>
        private static void AddPostalData()
        {
            PostBE = new PostalAddressBE();
            PostBE.PERSON_ID = personBE.PERSON_ID;
            PostBE.CITY = "Hyderabad";
            PostBE.ADDRESS1 = "MindSpace";
            PostBE.ZIP_CODE = "5200005";
            PostBE.STATE_ID = 1;
            PostBE.CREATEDDATE = System.DateTime.Now;
            PostBE.CREATEDUSER = 1;
            (new PersonBS()).Update(personBE);
        }
        /// <summary>
        ///A test for Update with Null Data
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithNULL()
        {
            PostalAddressBE postBE = null; // TODO: Initialize to an appropriate value
            int PerID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = (new PostAddressBS()).Update(postBE, PerID);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Update with data
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            PostAddressBS target = new PostAddressBS(); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(PostBE, personBE.PERSON_ID);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for getPostAddrRow with NULL data
        ///</summary>
        [TestMethod()]

        public void getPostAddrRowTestWithNULL()
        {
            int PostAddrID = 0; // TODO: Initialize to an appropriate value
            PostalAddressBE expected = null; // TODO: Initialize to an appropriate value
            PostalAddressBE actual;
            actual = (new PostAddressBS()).getPostAddrRow(PostAddrID);
            if (actual.IsNull()) actual = null;
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for getPostAddrRow with data
        ///</summary>
        [TestMethod()]
        public void getPostAddrRowTestwithData()
        {
            PostAddressBS target = new PostAddressBS(); // TODO: Initialize to an appropriate value
            PostalAddressBE expected = null; // TODO: Initialize to an appropriate value
            PostalAddressBE actual;
            actual = target.getPostAddrRow(PostBE.POSTALADDRESSID);
            Assert.AreNotEqual(expected, actual);
        }


    }
}
