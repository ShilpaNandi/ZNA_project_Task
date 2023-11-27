using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System.Linq;
using System; 

namespace AIS.Unit.Test
{
    /// <summary>
    ///This is a test class for AccountBSTest and is intended
    ///to contain all AccountBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AccountBSTest
    {

        private TestContext testContextInstance;
        static AccountBE accountBE;
        static AccountBS accountBS;
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


        private AccountBS accountService;

        private AccountBS AccountService
        {
            get
            {
                if (accountService == null)
                {
                    accountService = new AccountBS();
                }
                return accountService;
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
            //Customet Table
            accountBS = new AccountBS();
            AddAccountData();
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
        /// a method for adding New Record when the class is initiated Into Customer Table 
        /// </summary>
        private static void AddAccountData()
        {
            accountBE = new AccountBE();
            accountBE.FULL_NM = "siva" + System.DateTime.Now.ToString();
            accountBE.CREATE_DATE = System.DateTime.Now;
            accountBE.CREATE_USER_ID = 1;
            accountBS.Save(accountBE);
        }
        private AccountBE OriginalBE;
        private AccountBE UpdatedBE;
        private AccountBE UpdateComparisonBE;

        #region AccountBS Testing Methods
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            bool expected = true;
            bool actual;
            AccountBS target = new AccountBS();
            AccountBE accBE = target.Retrieve(accountBE.CUSTMR_ID);
            actual = target.Update(accBE);
            Assert.AreEqual(expected, actual);
        }

       

        /// <summary>
        ///A test for getRelatedAccountsRO
        ///</summary>
        [TestMethod()]
        public void getRelatedAccountsROTest()
        {
            int AccountID = 1; 
            int MasterAccountID = 1; 
            int Expected = 9;
            int Delta = 50; 
            IList<AccountBE> actual;
            actual = AccountService.getRelatedAccountsRO(AccountID, MasterAccountID);
            Assert.AreEqual(Expected, actual.Count, Delta, "Expected " + Expected.ToString() + ".   Received " + actual.Count.ToString() + ". Acceptable Delta of " + Delta.ToString() + "."); 
        }

        /// <summary>
        ///A test for getRelatedAccounts
        ///</summary>
        [TestMethod()]
        public void getRelatedAccountsTest()
        {
            int AccountID = 1; 
            int Expected = 2;
            int Delta = 50; 
            IList<AccountBE> actual;
            actual = AccountService.getRelatedAccounts(AccountID); //target.getRelatedAccounts(AccountID);
            Assert.AreEqual(Expected, actual.Count, Delta, "Expected " + Expected.ToString() + ".   Received " + actual.Count.ToString() + ". Acceptable Delta of " + Delta.ToString() + "."); 
        }

        
        /// <summary>
        ///A test for getNonMasterAccounts
        ///</summary>
        [TestMethod()]
        public void getNonMasterAccountsTest()
        {
            int Expected = 2;
            int Delta = 50;
            IList<AccountBE> actual;
            actual = AccountService.getNonMasterAccounts(); 
            Assert.AreEqual(Expected, actual.Count, Delta, "Expected " + Expected.ToString() + ".   Received " + actual.Count.ToString() + ". Acceptable Delta of " + Delta.ToString() + ".");
        }

        /// <summary>
        ///A test for getAccounts
        ///</summary>
        [TestMethod()]
        public void getAccountsTest1()
        {
            AccountBS target = new AccountBS();
            string AccountName = "aaa9:06:35 AM";
            string BPNumber = string.Empty;  
            int AccountNumber = 0;  
            string SSCGID = string.Empty; 
            IList<AccountBE> expected = null; 
            IList<AccountBE> actual;
            actual = target.getAccounts(AccountName, BPNumber, AccountNumber, SSCGID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getAccounts
        ///</summary>
        [TestMethod()]
        public void getAccountsTest()
        {
            int NotExpected = 0;
            IList<AccountBE> actual;
            actual = AccountService.getAccounts();
            Assert.AreNotSame(NotExpected, actual.Count);
      }

        /// <summary>
        ///A test for getAccount
        ///</summary>
        [TestMethod()]
        public void getAccountTest()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            int AccountID = 701; // TODO: Initialize to an appropriate value
            AccountBE expected = null; // TODO: Initialize to an appropriate value
            AccountBE actual;
            actual = target.getAccount(AccountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DivTest
        ///</summary>
        [TestMethod()]
        public void DivTestTest()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            int a = 6; // TODO: Initialize to an appropriate value
            int b = 1; // TODO: Initialize to an appropriate value
            int expected = 6; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.DivTest(a, b);
            Assert.AreEqual(expected, actual);
            //
        }

        /// <summary>
        ///A test for CheckDuplicateAccountName
        ///</summary>
        [TestMethod()]
        public void CheckDuplicateAccountNameTest()
        {
            string Name = "L'oreal";
            bool Expected = true;
            bool actual;
            actual = AccountService.CheckDuplicateAccountName(Name);
            Assert.AreEqual(Expected, actual);
        }

       
        #endregion

        #region Save and Retrieve Account Data

        [TestMethod()]
        public void Add_And_Retrieve_AccountInfoData()
        {
            OriginalBE = new AccountBE();

            // Test Save with missing required fields 
            Assert.IsTrue(!AccountService.Save(OriginalBE), "Add was successful with missing data.");

            // Set values 
            PopulateEntity(OriginalBE);

            // Test Add with all of the required fields 
            Assert.IsTrue(AccountService.Save(OriginalBE), "Add Entity Failed.  Error: " + OriginalBE.ValidationErrors);

            // Use Primary Key of record just added 
            UpdatedBE = AccountService.getAccount(OriginalBE.CUSTMR_ID);

            // Ensure that the result is not null 
            Assert.IsNotNull(UpdatedBE, "Retrieve Entity Failed.");

            // Test every field for successful entry to the database. 
            Assert.IsTrue(OriginalBE.BKTCYBUYOUT_DATE == UpdatedBE.BKTCYBUYOUT_DATE);
            Assert.IsTrue(OriginalBE.BKTCYBUYOUT_ID == UpdatedBE.BKTCYBUYOUT_ID);
            Assert.IsTrue(OriginalBE.ACTV_IND == UpdatedBE.ACTV_IND);
            Assert.IsTrue(OriginalBE.CREATE_DATE == DateTime.Parse(DateTime.Now.ToShortDateString()));
            Assert.IsTrue(OriginalBE.CREATE_USER_ID == UpdatedBE.CREATE_USER_ID);
            Assert.IsTrue(OriginalBE.FINC_PTY_ID == UpdatedBE.FINC_PTY_ID);
            Assert.IsTrue(OriginalBE.FULL_NM == UpdatedBE.FULL_NM);
            Assert.IsTrue(OriginalBE.MARYLAND_RETRO_IND == UpdatedBE.MARYLAND_RETRO_IND);
            Assert.IsTrue(OriginalBE.MSTR_ACCT_IND == UpdatedBE.MSTR_ACCT_IND);
            //Assert.IsTrue(OriginalBE.PERSON_ID == UpdatedBE.PERSON_ID);
            Assert.IsTrue(OriginalBE.PEO_IND == UpdatedBE.PEO_IND);
            Assert.IsTrue(OriginalBE.SUPRT_SERV_CUSTMR_GP_ID == UpdatedBE.SUPRT_SERV_CUSTMR_GP_ID);
            Assert.IsTrue(OriginalBE.TPA_FUNDED_IND == UpdatedBE.TPA_FUNDED_IND);
            Assert.IsTrue(OriginalBE.UPDATE_DATE == UpdatedBE.UPDATE_DATE);
            Assert.IsTrue(OriginalBE.UPDATE_USER_ID == UpdatedBE.UPDATE_USER_ID);  

            // Update all of the fields of the just retrieved record. 
            UpdatedBE.BKTCYBUYOUT_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            UpdatedBE.BKTCYBUYOUT_ID = 1;
            UpdatedBE.ACTV_IND = true;
            UpdatedBE.CREATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            UpdatedBE.CREATE_USER_ID = 1;
            UpdatedBE.FINC_PTY_ID = "1234567890";
            UpdatedBE.FULL_NM = "UnitTestingUPD";
            UpdatedBE.MARYLAND_RETRO_IND = true;
            UpdatedBE.MSTR_ACCT_IND = false;
            //UpdatedBE.PERSON_ID = 1;
            UpdatedBE.PEO_IND = true;
            UpdatedBE.SUPRT_SERV_CUSTMR_GP_ID = "0123456789";
            UpdatedBE.TPA_FUNDED_IND = true;
            UpdatedBE.UPDATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            UpdatedBE.UPDATE_USER_ID = 1;


            // Test Update Service for updating AE 
            Assert.IsTrue(AccountService.Save(UpdatedBE), "Update Entity Failed.  Error: " + OriginalBE.ValidationErrors);

            // Re-retrieve the entity - it should now have all of the updated fields. 
            UpdateComparisonBE = AccountService.getAccount(OriginalBE.CUSTMR_ID);

            // Make sure we received a record 
            Assert.IsNotNull(UpdateComparisonBE, "Retrieve Entity Failed.");
            // Test all of the fields again to check that they match the expected "updated" value. 
            Assert.IsTrue(UpdateComparisonBE.BKTCYBUYOUT_DATE == UpdatedBE.BKTCYBUYOUT_DATE);
            Assert.IsTrue(UpdateComparisonBE.BKTCYBUYOUT_ID == UpdatedBE.BKTCYBUYOUT_ID);
            Assert.IsTrue(UpdateComparisonBE.ACTV_IND == UpdatedBE.ACTV_IND);
            Assert.IsTrue(UpdateComparisonBE.CREATE_DATE == DateTime.Parse(DateTime.Now.ToShortDateString()));
            Assert.IsTrue(UpdateComparisonBE.CREATE_USER_ID == UpdatedBE.CREATE_USER_ID);
            Assert.IsTrue(UpdateComparisonBE.FINC_PTY_ID == UpdatedBE.FINC_PTY_ID);
            Assert.IsTrue(UpdateComparisonBE.FULL_NM == UpdatedBE.FULL_NM);
            Assert.IsTrue(UpdateComparisonBE.MARYLAND_RETRO_IND == UpdatedBE.MARYLAND_RETRO_IND);
            Assert.IsTrue(UpdateComparisonBE.MSTR_ACCT_IND == UpdatedBE.MSTR_ACCT_IND);
            //Assert.IsTrue(UpdateComparisonBE.PERSON_ID == UpdatedBE.PERSON_ID);
            Assert.IsTrue(UpdateComparisonBE.PEO_IND == UpdatedBE.PEO_IND);
            Assert.IsTrue(UpdateComparisonBE.SUPRT_SERV_CUSTMR_GP_ID == UpdatedBE.SUPRT_SERV_CUSTMR_GP_ID);
            Assert.IsTrue(UpdateComparisonBE.TPA_FUNDED_IND == UpdatedBE.TPA_FUNDED_IND);
            Assert.IsTrue(UpdateComparisonBE.UPDATE_DATE == UpdatedBE.UPDATE_DATE);
            Assert.IsTrue(UpdateComparisonBE.UPDATE_USER_ID == UpdatedBE.UPDATE_USER_ID); 
        } 

            private void PopulateEntity(AccountBE be)
            {
                be.BKTCYBUYOUT_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
                be.BKTCYBUYOUT_ID = 1;
                be.ACTV_IND = true;
                be.CREATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
                be.CREATE_USER_ID = 1;
                be.FINC_PTY_ID = "1234567890";
                be.FULL_NM = "UnitTestingNEW";
                be.MARYLAND_RETRO_IND = true;
                be.MSTR_ACCT_IND = false;
                //be.PERSON_ID = 1;
                be.PEO_IND = true;
                be.SUPRT_SERV_CUSTMR_GP_ID = "0123456789";
                be.TPA_FUNDED_IND = true;
                be.UPDATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
                be.UPDATE_USER_ID = 1;
            } 
        #endregion

        #region Save and Retrieve Related Retro Account Data

        [TestMethod()]
        public void Add_And_Retrieve_RelatedRetroAccountData()
        {
            OriginalBE = new AccountBE();

            // Test Save with missing required fields 
            Assert.IsTrue(!AccountService.Save(OriginalBE), "Add was successful with missing data.");

            // Set values 
            PopulateAccount(OriginalBE);

            // Test Add with all of the required fields 
            Assert.IsTrue(AccountService.Save(OriginalBE), "Add Entity Failed.  Error: " + OriginalBE.ValidationErrors);

            // Use Primary Key of record just added 
            UpdatedBE = AccountService.getAccount(OriginalBE.CUSTMR_ID);

            // Ensure that the result is not null 
            Assert.IsNotNull(UpdatedBE, "Retrieve Entity Failed.");

            // Test every field for successful entry to the database. 
            Assert.IsTrue(OriginalBE.BKTCYBUYOUT_DATE == UpdatedBE.BKTCYBUYOUT_DATE);
            Assert.IsTrue(OriginalBE.BKTCYBUYOUT_ID == UpdatedBE.BKTCYBUYOUT_ID);
            Assert.IsTrue(OriginalBE.ACTV_IND == UpdatedBE.ACTV_IND);
            Assert.IsTrue(OriginalBE.CREATE_DATE == DateTime.Parse(DateTime.Now.ToShortDateString()));
            Assert.IsTrue(OriginalBE.CREATE_USER_ID == UpdatedBE.CREATE_USER_ID);
            Assert.IsTrue(OriginalBE.FINC_PTY_ID == UpdatedBE.FINC_PTY_ID);
            Assert.IsTrue(OriginalBE.FULL_NM == UpdatedBE.FULL_NM);
            Assert.IsTrue(OriginalBE.MARYLAND_RETRO_IND == UpdatedBE.MARYLAND_RETRO_IND);
            Assert.IsTrue(OriginalBE.MSTR_ACCT_IND == UpdatedBE.MSTR_ACCT_IND);
            //Assert.IsTrue(OriginalBE.PERSON_ID == UpdatedBE.PERSON_ID);
            Assert.IsTrue(OriginalBE.PEO_IND == UpdatedBE.PEO_IND);
            Assert.IsTrue(OriginalBE.SUPRT_SERV_CUSTMR_GP_ID == UpdatedBE.SUPRT_SERV_CUSTMR_GP_ID);
            Assert.IsTrue(OriginalBE.TPA_FUNDED_IND == UpdatedBE.TPA_FUNDED_IND);
            Assert.IsTrue(OriginalBE.UPDATE_DATE == UpdatedBE.UPDATE_DATE);
            Assert.IsTrue(OriginalBE.UPDATE_USER_ID == UpdatedBE.UPDATE_USER_ID);

            // Update all of the fields of the just retrieved record. 
            UpdatedBE.BKTCYBUYOUT_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            UpdatedBE.BKTCYBUYOUT_ID = 1;
            UpdatedBE.ACTV_IND = true;
            UpdatedBE.CREATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            UpdatedBE.CREATE_USER_ID = 1;
            UpdatedBE.CUSTMR_REL_ACTV_IND = true;
            UpdatedBE.CUSTMR_REL_ID = 1;
            UpdatedBE.FINC_PTY_ID = "1234567890";
            UpdatedBE.FULL_NM = "UnitTestingUPD";
            UpdatedBE.MARYLAND_RETRO_IND = true;
            UpdatedBE.MSTR_ACCT_IND = false;
            //UpdatedBE.PERSON_ID = 1;
            UpdatedBE.PEO_IND = true;
            UpdatedBE.SUPRT_SERV_CUSTMR_GP_ID = "0123456789";
            UpdatedBE.TPA_FUNDED_IND = true;
            UpdatedBE.UPDATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            UpdatedBE.UPDATE_USER_ID = 1;


            // Test Update Service for updating AE 
            Assert.IsTrue(AccountService.Save(UpdatedBE), "Update Entity Failed.  Error: " + OriginalBE.ValidationErrors);

            // Re-retrieve the entity - it should now have all of the updated fields. 
            UpdateComparisonBE = AccountService.getAccount(OriginalBE.CUSTMR_ID);

            // Make sure we received a record 
            Assert.IsNotNull(UpdateComparisonBE, "Retrieve Entity Failed.");
            // Test all of the fields again to check that they match the expected "updated" value. 
            Assert.IsTrue(UpdateComparisonBE.BKTCYBUYOUT_DATE == UpdatedBE.BKTCYBUYOUT_DATE);
            Assert.IsTrue(UpdateComparisonBE.BKTCYBUYOUT_ID == UpdatedBE.BKTCYBUYOUT_ID);
            Assert.IsTrue(UpdateComparisonBE.ACTV_IND == UpdatedBE.ACTV_IND);
            Assert.IsTrue(UpdateComparisonBE.CREATE_DATE == DateTime.Parse(DateTime.Now.ToShortDateString()));
            Assert.IsTrue(UpdateComparisonBE.CREATE_USER_ID == UpdatedBE.CREATE_USER_ID);
            Assert.IsTrue(UpdateComparisonBE.CUSTMR_REL_ACTV_IND == UpdatedBE.CUSTMR_REL_ACTV_IND);
            Assert.IsTrue(UpdateComparisonBE.CUSTMR_REL_ID == UpdatedBE.CUSTMR_REL_ID);
            Assert.IsTrue(UpdateComparisonBE.FINC_PTY_ID == UpdatedBE.FINC_PTY_ID);
            Assert.IsTrue(UpdateComparisonBE.FULL_NM == UpdatedBE.FULL_NM);
            Assert.IsTrue(UpdateComparisonBE.MARYLAND_RETRO_IND == UpdatedBE.MARYLAND_RETRO_IND);
            Assert.IsTrue(UpdateComparisonBE.MSTR_ACCT_IND == UpdatedBE.MSTR_ACCT_IND);
            //Assert.IsTrue(UpdateComparisonBE.PERSON_ID == UpdatedBE.PERSON_ID);
            Assert.IsTrue(UpdateComparisonBE.PEO_IND == UpdatedBE.PEO_IND);
            Assert.IsTrue(UpdateComparisonBE.SUPRT_SERV_CUSTMR_GP_ID == UpdatedBE.SUPRT_SERV_CUSTMR_GP_ID);
            Assert.IsTrue(UpdateComparisonBE.TPA_FUNDED_IND == UpdatedBE.TPA_FUNDED_IND);
            Assert.IsTrue(UpdateComparisonBE.UPDATE_DATE == UpdatedBE.UPDATE_DATE);
            Assert.IsTrue(UpdateComparisonBE.UPDATE_USER_ID == UpdatedBE.UPDATE_USER_ID);
        }

        private void PopulateAccount(AccountBE be)
        {
            be.BKTCYBUYOUT_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            be.BKTCYBUYOUT_ID = 1;
            be.ACTV_IND = true;
            be.CREATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            be.CREATE_USER_ID = 1;
            be.FINC_PTY_ID = "1234567890";
            be.FULL_NM = "UnitTestingNEW";
            be.MARYLAND_RETRO_IND = true;
            be.MSTR_ACCT_IND = false;
            //be.PERSON_ID = 1;
            be.PEO_IND = true;
            be.SUPRT_SERV_CUSTMR_GP_ID = "0123456789";
            be.TPA_FUNDED_IND = true;
            be.UPDATE_DATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            be.UPDATE_USER_ID = 1;
        }
        #endregion

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest3()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            AccountBE accountBE = null; // TODO: Initialize to an appropriate value
            CustomerContactBE custmrContactBE = null; // TODO: Initialize to an appropriate value
            int personID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(accountBE, custmrContactBE, personID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest2()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            AccountBE accountBE = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(accountBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getRelatedAccountsRO
        ///</summary>
        [TestMethod()]
        public void getRelatedAccountsROTest1()
        {
            AccountBS target = new AccountBS(); 
            int AccountID = 0; 
            int MasterAccountID = 0; 
            IList<AccountBE> expected = null; 
            IList<AccountBE> actual;
            actual = target.getRelatedAccountsRO(AccountID, MasterAccountID);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for getAccountNames
        ///</summary>
        [TestMethod()]
        public void getAccountNamesTest()
        {
            AccountBS target = new AccountBS();
            IList<AccountBE> expected = null;
            IList<AccountBE> actual;
            actual = target.getAccountNames(2);
            Assert.AreNotEqual(expected, actual);
        }


        /// <summary>
        ///A test for getRelatedAccounts
        ///</summary>
        [TestMethod()]
        public void getRelatedAccountsTest1()
        {
            AccountBS target = new AccountBS(); 
            int AccountID = 0; 
            IList<AccountBE> expected = null; 
            IList<AccountBE> actual;
            actual = target.getRelatedAccounts(AccountID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getNonMasterAccounts
        ///</summary>
        [TestMethod()]
        public void getNonMasterAccountsTest1()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.getNonMasterAccounts();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getNonMasterAccounts
        ///</summary>
        [TestMethod()]
        public void isAccountSetUPasPEOTest()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            
            Boolean actual;
            actual = target.isAccountSetUPasPEO(accountBE.CUSTMR_ID);
            Assert.AreEqual(false, actual);

        }

        /// <summary>
        ///A test for getAccounts
        ///</summary>
        [TestMethod()]
        public void getAccountsTest3()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            string AccountName = string.Empty; // TODO: Initialize to an appropriate value
            string BPNumber = string.Empty; // TODO: Initialize to an appropriate value
            int AccountNumber = 0; // TODO: Initialize to an appropriate value
            string SSCGID = string.Empty; // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.getAccounts(AccountName, BPNumber, AccountNumber, SSCGID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getAccounts
        ///</summary>
        [TestMethod()]
        public void getAccountsTest2()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.getAccounts();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getAccounts
        ///</summary>
        [TestMethod()]
        public void getAccountsTest4()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            string[] input = new string[2];
            input[0] = "a";
            input[1] = "b";
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.getAccounts(input);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getAccountsInfo
        ///</summary>
        [TestMethod()]
        public void getAccountsInfoTest()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.getAccountsInfo(accountBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getMasterAccounts
        ///</summary>
        [TestMethod()]
        public void getMasterAccountsTest()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.getMasterAccounts();
            Assert.AreNotEqual(expected, actual);

        }
        /// <summary>
        ///A test for getMasterAccounts
        ///</summary>
        [TestMethod()]
        public void getMasterAccountsTest1()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            string[] input = new string[2];
            input[0] = "a";
            input[1] = "b";
            IList<AccountBE> actual;
            actual = target.getMasterAccounts(input);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getMasterWithChildAccounts
        ///</summary>
        [TestMethod()]
        public void getMasterWithChildAccountsTest()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            actual = target.getMasterWithChildAccounts(2);
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for getMasterWithChildAccounts
        ///</summary>
        [TestMethod()]
        public void getMasterWithChildAccountsTest1()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            IList<AccountBE> expected = null; // TODO: Initialize to an appropriate value
            IList<AccountBE> actual;
            string[] input = new string[2];
            input[0] = "a";
            input[1] = "b";
            actual = target.getMasterWithChildAccounts(2,input);
            Assert.AreNotEqual(expected, actual);

        }


        /// <summary>
        ///A test for getAccount
        ///</summary>
        [TestMethod()]
        public void getAccountTest1()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            int AccountID = 0; // TODO: Initialize to an appropriate value
            AccountBE expected = null; // TODO: Initialize to an appropriate value
            AccountBE actual;
            actual = target.getAccount(AccountID);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for DivTest
        ///</summary>
        [TestMethod()]
        public void DivTestTest1()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            int a = 1; // TODO: Initialize to an appropriate value
            int b = 1; // TODO: Initialize to an appropriate value
            int expected = 1; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.DivTest(a, b);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for CheckDuplicateAccountName
        ///</summary>
        [TestMethod()]
        public void CheckDuplicateAccountNameTest1()
        {
            AccountBS target = new AccountBS(); // TODO: Initialize to an appropriate value
            string Name = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = true ; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.CheckDuplicateAccountName(Name);
            Assert.AreEqual(expected, actual);
            
        }

       
    }
}
