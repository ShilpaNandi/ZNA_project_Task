using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AssignContactsBETest and is intended
    ///to contain all AssignContactsBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class AssignContactsBETest
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
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
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
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDT_DT = expected;
            actual = target.UPDT_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SND_INVC_IND_txt
        ///</summary>
        [TestMethod()]
        public void SND_INVC_IND_txtTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SND_INVC_IND_txt = expected;
            actual = target.SND_INVC_IND_txt;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SND_INVC_IND
        ///</summary>
        [TestMethod()]
        public void SND_INVC_INDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.SND_INVC_IND = expected;
            actual = target.SND_INVC_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_PERS_REL_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_PERS_REL_IDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_PERS_REL_ID = expected;
            actual = target.PREM_ADJ_PGM_PERS_REL_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PREM_ADJ_PGM_ID
        ///</summary>
        [TestMethod()]
        public void PREM_ADJ_PGM_IDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PREM_ADJ_PGM_ID = expected;
            actual = target.PREM_ADJ_PGM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERS_ID
        ///</summary>
        [TestMethod()]
        public void PERS_IDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PERS_ID = expected;
            actual = target.PERS_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LastName
        ///</summary>
        [TestMethod()]
        public void LastNameTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LastName = expected;
            actual = target.LastName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        public void FullNameTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FullName = expected;
            actual = target.FullName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FirstName
        ///</summary>
        [TestMethod()]
        public void FirstNameTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FirstName = expected;
            actual = target.FirstName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CUSTMR_ID
        ///</summary>
        [TestMethod()]
        public void CUSTMR_IDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CUSTMR_ID = expected;
            actual = target.CUSTMR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CRTE_USER_ID
        ///</summary>
        [TestMethod()]
        public void CRTE_USER_IDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
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
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CRTE_DT = expected;
            actual = target.CRTE_DT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ContTypID
        ///</summary>
        [TestMethod()]
        public void ContTypIDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ContTypID = expected;
            actual = target.ContTypID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ContTyp
        ///</summary>
        [TestMethod()]
        public void ContTypTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ContTyp = expected;
            actual = target.ContTyp;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ComTypText
        ///</summary>
        [TestMethod()]
        public void ComTypTextTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ComTypText = expected;
            actual = target.ComTypText;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for COMMU_MEDUM_ID
        ///</summary>
        [TestMethod()]
        public void COMMU_MEDUM_IDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.COMMU_MEDUM_ID = expected;
            actual = target.COMMU_MEDUM_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACTV_IND
        ///</summary>
        [TestMethod()]
        public void ACTV_INDTest()
        {
            AssignContactsBE target = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTV_IND = expected;
            actual = target.ACTV_IND;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AssignContactsBE Constructor
        ///</summary>
        [TestMethod()]
        public void AssignContactsBEConstructorTest()
        {
            AssignContactsBE target = new AssignContactsBE();
            
        }
    }
}
