using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AssignContactsBSTest and is intended
    ///to contain all AssignContactsBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AssignContactsBSTest
    {


        private TestContext testContextInstance;
        static AccountBE AcctBE;
        static ProgramPeriodBE ProgPerdBE;
        static AssignContactsBE contactsBE;
        static PersonBE personBE;
        static AssignContactsBS target;
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

            target = new AssignContactsBS();
            AddCustomerData();
            AddProgPerdData();
            
            AddPersonData();
            AddContactsData();
           
        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            AddCustomerData();
            AddProgPerdData();
            AddPersonData();
            AddContactsData();
        }
        
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        

        /// <summary>
        /// a method for adding New Record in Customer when the classis initiated 
        /// </summary>
        private static void AddCustomerData()
        {
            AcctBE = new AccountBE();
            AcctBE.FULL_NM = "Test Account" + System.DateTime.Now.ToLongTimeString();
            AcctBE.CREATE_DATE = System.DateTime.Now;
            AcctBE.CREATE_USER_ID = 1;
            (new AccountBS()).Save(AcctBE);
        }
        /// <summary>
        /// a method for adding New Record in Premium Adjustment program table when the classis initiated 
        /// </summary>
        private static void AddProgPerdData()
        {
            ProgPerdBE = new ProgramPeriodBE();
            ProgPerdBE.CUSTMR_ID = AcctBE.CUSTMR_ID;
            ProgPerdBE.CREATE_DATE = System.DateTime.Now;
            ProgPerdBE.CREATE_USER_ID = 1;
            (new ProgramPeriodsBS()).Save(ProgPerdBE);
        }

        /// <summary>
        /// a method for adding New Record in PERS table when the class is initiated
        /// </summary>
        private static void AddPersonData()
        {
            personBE = new PersonBE();
            personBE.SURNAME = "TestSurName";
            
            personBE.FORENAME = "TestForeName";
            personBE.USERID = "XCS0672";
            personBE.CREATEDDATE = System.DateTime.Now;
            personBE.CREATEDUSER_ID = 1;
            (new PersonBS()).Save(personBE);
        }
        /// <summary>
        /// To add a new record in PREM_ADJ_PGM_PERS_REL Table when the class is initiated 
        /// </summary>
        private static void AddContactsData()
        {
            contactsBE = new AssignContactsBE();
            contactsBE.PREM_ADJ_PGM_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            contactsBE.CUSTMR_ID = ProgPerdBE.CUSTMR_ID;
            contactsBE.PERS_ID = personBE.PERSON_ID;
            contactsBE.COMMU_MEDUM_ID = 60;
            contactsBE.CRTE_DT = System.DateTime.Now;
            contactsBE.CRTE_USER_ID = 1;
            target.SaveContactsData(contactsBE);
        }
        
        /// <summary>
        ///A null test for SaveContactsData
        ///</summary>
        [TestMethod()]
        public void SaveContactsDataNullTest()
        {
            
            contactsBE = new AssignContactsBE(); // TODO: Initialize to an appropriate value
            contactsBE = null;
            target = new AssignContactsBS();
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            //contactsBE.PREM_ADJ_PGM_PERS_REL_ID = 0;
            actual = target.SaveContactsData(contactsBE);
            Assert.AreNotEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for SaveContactsData with Data
        ///</summary>
        [TestMethod()]
        public void SaveContactsDataTest()
        {

            contactsBE = new AssignContactsBE();
            contactsBE.PREM_ADJ_PGM_ID = ProgPerdBE.PREM_ADJ_PGM_ID;
            contactsBE.CUSTMR_ID = ProgPerdBE.CUSTMR_ID;
            contactsBE.PERS_ID = personBE.PERSON_ID;
            contactsBE.COMMU_MEDUM_ID = 60;
            contactsBE.CRTE_DT = System.DateTime.Now;
            contactsBE.CRTE_USER_ID= 1;
            contactsBE.PREM_ADJ_PGM_PERS_REL_ID = 0;
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SaveContactsData(contactsBE);
            Assert.AreEqual(expected, actual);

        }
        // <summary>
        ///A test for LoadData
        ///</summary>
        [TestMethod()]
        public void LoadDataTest()
        {
            AssignContactsBS target = new AssignContactsBS(); // TODO: Initialize to an appropriate value
            int PreAdjPgmRelId = contactsBE.PREM_ADJ_PGM_PERS_REL_ID; // TODO: Initialize to an appropriate value
            AssignContactsBE expected = null;// target.LoadData(PreAdjPgmRelId);  // TODO: Initialize to an appropriate value
            AssignContactsBE actual = new AssignContactsBE();
            actual = target.LoadData(PreAdjPgmRelId);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreNotEqual(expected, actual);

        }

       
        /// <summary>
        ///A test for getPersonNames
        ///</summary>
        [TestMethod()]
        public void getPersonNamesTest()
        {
            AssignContactsBS target = new AssignContactsBS(); // TODO: Initialize to an appropriate value
            int ContTypId = 0; // TODO: Initialize to an appropriate value
            IList<PersonBE> test = new List<PersonBE>(); // TODO: Initialize to an appropriate value
            int expected = test.Count;
            IList<PersonBE> result;
            result = target.GetPersonNames(ContTypId,0);
            int actual = result.Count;
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getContactsData
        ///</summary>
        [TestMethod()]
        public void getContactsDataTest()
        {
            AssignContactsBS target = new AssignContactsBS(); // TODO: Initialize to an appropriate value
            int ProgramPeriodID = 0; // TODO: Initialize to an appropriate value
            IList<AssignContactsBE> test = new List<AssignContactsBE>();  // TODO: Initialize to an appropriate value
            int expected = test.Count;
            IList<AssignContactsBE> result;
            result = target.GetContactsData(ProgramPeriodID);
            int actual = result.Count;
            Assert.AreEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for LoadData with Invalid Data
        ///</summary>
        [TestMethod()]
        public void LoadDataNullTest()
        {
            AssignContactsBS target = new AssignContactsBS(); // TODO: Initialize to an appropriate value
            int PreAdjPgmRelId = 0; // TODO: Initialize to an appropriate value
            AssignContactsBE expected = null;// target.LoadData(PreAdjPgmRelId);  // TODO: Initialize to an appropriate value
            AssignContactsBE actual = new AssignContactsBE();
            actual = target.LoadData(PreAdjPgmRelId);
            if (actual.IsNull())
            {
                actual = null;
            }
            Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for AssignContactsBS Constructor
        ///</summary>
        [TestMethod()]
        public void AssignContactsBSConstructorTest()
        {
            AssignContactsBS target = new AssignContactsBS();
            
        }

        
        

        /// <summary>
        ///A test for AssignContactsBS Constructor
        ///</summary>
        /*[TestMethod()]
        public void AssignContactsBSConstructorTest1()
        {
            AssignContactsBS target = new AssignContactsBS();
            
        }*/
    }
}
