using ZurichNA.AIS.Business.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PersonBETest and is intended
    ///to contain all PersonBETest Unit Tests
    ///</summary>
    [TestClass()]
    public class PersonBETest
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
        ///A test for ZIPCODE
        ///</summary>
        [TestMethod()]
        public void ZIPCODETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ZIPCODE;
            
        }

        /// <summary>
        ///A test for USERID
        ///</summary>
        [TestMethod()]
        public void USERIDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.USERID = expected;
            actual = target.USERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDUSER_ID
        ///</summary>
        [TestMethod()]
        public void UPDATEDUSER_IDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.UPDATEDUSER_ID = expected;
            actual = target.UPDATEDUSER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for UPDATEDDATE
        ///</summary>
        [TestMethod()]
        public void UPDATEDDATETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.UPDATEDDATE = expected;
            actual = target.UPDATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TitleLookUp
        ///</summary>
        [TestMethod()]
        public void TitleLookUpTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            LookupBE actual;
            actual = target.TitleLookUp;
            
        }

        /// <summary>
        ///A test for TITLE_ID
        ///</summary>
        [TestMethod()]
        public void TITLE_IDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.TITLE_ID = expected;
            actual = target.TITLE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TITLE
        ///</summary>
        [TestMethod()]
        public void TITLETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.TITLE;
            
        }

        /// <summary>
        ///A test for TELEPHONE2
        ///</summary>
        [TestMethod()]
        public void TELEPHONE2Test()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TELEPHONE2 = expected;
            actual = target.TELEPHONE2;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for TELEPHONE1
        ///</summary>
        [TestMethod()]
        public void TELEPHONE1Test()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TELEPHONE1 = expected;
            actual = target.TELEPHONE1;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for SURNAME
        ///</summary>
        [TestMethod()]
        public void SURNAMETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SURNAME = expected;
            actual = target.SURNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for StateName
        ///</summary>
        [TestMethod()]
        public void StateNameTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.StateName = expected;
            actual = target.StateName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for STATE_TXT
        ///</summary>
        [TestMethod()]
        public void STATE_TXTTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.STATE_TXT;
            
        }

        /// <summary>
        ///A test for STATE
        ///</summary>
        [TestMethod()]
        public void STATETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.STATE;
            
        }

        /// <summary>
        ///A test for selPOSTALADDRESSID
        ///</summary>
        [TestMethod()]
        public void selPOSTALADDRESSIDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.selPOSTALADDRESSID = expected;
            actual = target.selPOSTALADDRESSID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POSTALADDRESSID
        ///</summary>
        [TestMethod()]
        public void POSTALADDRESSIDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.POSTALADDRESSID;
            
        }

        /// <summary>
        ///A test for POST_ADDR_ID
        ///</summary>
        [TestMethod()]
        public void POST_ADDR_IDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.POST_ADDR_ID = expected;
            actual = target.POST_ADDR_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for POST_ADD_BE
        ///</summary>
        [TestMethod()]
        public void POST_ADD_BETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            PostalAddressBE expected = null; // TODO: Initialize to an appropriate value
            PostalAddressBE actual;
            target.POST_ADD_BE = expected;
            actual = target.POST_ADD_BE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PERSON_ID
        ///</summary>
        [TestMethod()]
        public void PERSON_IDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.PERSON_ID = expected;
            actual = target.PERSON_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MANAGERID
        ///</summary>
        [TestMethod()]
        public void MANAGERIDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.MANAGERID = expected;
            actual = target.MANAGERID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for MANAGER
        ///</summary>
        [TestMethod()]
        public void MANAGERTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.MANAGER = expected;
            actual = target.MANAGER;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FULLNAME
        ///</summary>
        [TestMethod()]
        public void FULLNAMETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULLNAME = expected;
            actual = target.FULLNAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FORENAME
        ///</summary>
        [TestMethod()]
        public void FORENAMETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FORENAME = expected;
            actual = target.FORENAME;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FAX
        ///</summary>
        [TestMethod()]
        public void FAXTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FAX = expected;
            actual = target.FAX;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXTERNAL_ORGN_TXT
        ///</summary>
        [TestMethod()]
        public void EXTERNAL_ORGN_TXTTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.EXTERNAL_ORGN_TXT = expected;
            actual = target.EXTERNAL_ORGN_TXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXTERNAL_ORGN_ID
        ///</summary>
        [TestMethod()]
        public void EXTERNAL_ORGN_IDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.EXTERNAL_ORGN_ID = expected;
            actual = target.EXTERNAL_ORGN_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EXPIRYDATE
        ///</summary>
        [TestMethod()]
        public void EXPIRYDATETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.EXPIRYDATE = expected;
            actual = target.EXPIRYDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EMAIL
        ///</summary>
        [TestMethod()]
        public void EMAILTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.EMAIL = expected;
            actual = target.EMAIL;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for EFFECTIVEDATE
        ///</summary>
        [TestMethod()]
        public void EFFECTIVEDATETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.EFFECTIVEDATE = expected;
            actual = target.EFFECTIVEDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDUSER_ID
        ///</summary>
        [TestMethod()]
        public void CREATEDUSER_IDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.CREATEDUSER_ID = expected;
            actual = target.CREATEDUSER_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CREATEDDATE
        ///</summary>
        [TestMethod()]
        public void CREATEDDATETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.CREATEDDATE = expected;
            actual = target.CREATEDDATE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CONTACTTYPE
        ///</summary>
        [TestMethod()]
        public void CONTACTTYPETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CONTACTTYPE = expected;
            actual = target.CONTACTTYPE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CONCTACT_TYPE_ID
        ///</summary>
        [TestMethod()]
        public void CONCTACT_TYPE_IDTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.CONCTACT_TYPE_ID = expected;
            actual = target.CONCTACT_TYPE_ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CityName
        ///</summary>
        [TestMethod()]
        public void CityNameTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CityName = expected;
            actual = target.CityName;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CITY
        ///</summary>
        [TestMethod()]
        public void CITYTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.CITY;
            
        }

        /// <summary>
        ///A test for ADJUSTMENT_QC
        ///</summary>
        [TestMethod()]
        public void ADJUSTMENT_QCTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ADJUSTMENT_QC = expected;
            actual = target.ADJUSTMENT_QC;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADJSETUPQCTEXT
        ///</summary>
        [TestMethod()]
        public void ADJSETUPQCTEXTTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ADJSETUPQCTEXT = expected;
            actual = target.ADJSETUPQCTEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ADDRESS2
        ///</summary>
        [TestMethod()]
        public void ADDRESS2Test()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ADDRESS2;
            
        }

        /// <summary>
        ///A test for ADDRESS1
        ///</summary>
        [TestMethod()]
        public void ADDRESS1Test()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ADDRESS1;
            
        }

        /// <summary>
        ///A test for ACTIVE
        ///</summary>
        [TestMethod()]
        public void ACTIVETest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<bool> expected = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            Nullable<bool> actual;
            target.ACTIVE = expected;
            actual = target.ACTIVE;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACCTSETUPQCTEXT
        ///</summary>
        [TestMethod()]
        public void ACCTSETUPQCTEXTTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ACCTSETUPQCTEXT = expected;
            actual = target.ACCTSETUPQCTEXT;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ACCTSETUP_QC
        ///</summary>
        [TestMethod()]
        public void ACCTSETUP_QCTest()
        {
            PersonBE target = new PersonBE(); // TODO: Initialize to an appropriate value
            Nullable<int> expected = new Nullable<int>(); // TODO: Initialize to an appropriate value
            Nullable<int> actual;
            target.ACCTSETUP_QC = expected;
            actual = target.ACCTSETUP_QC;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PersonBE Constructor
        ///</summary>
        [TestMethod()]
        public void PersonBEConstructorTest()
        {
            PersonBE target = new PersonBE();
            
        }
    }
}
