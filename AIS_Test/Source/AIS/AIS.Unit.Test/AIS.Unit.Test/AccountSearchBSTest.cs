using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System;

namespace AIS.Unit.Test
{
    /// <summary>
    ///This is a test class for AccountSearchBSTest and is intended
    ///to contain all AccountSearchBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AccountSearchBSTest
    {
        static AccountBE acctBE;
        static AccountBS acctBS;
        static AccountSearchBS target; 

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
            acctBE = new AccountBE();
            acctBS = new AccountBS();
            target = new AccountSearchBS();
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {

        }
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string full_nm = "L'oreal USA Inc";
            string finc_pty_id = "23424";
            string suprt_serv_custmr_gp_id = "234234243";
            bool mstr_acct_ind = false;
            int crte_user_id = 1;
            DateTime crte_dt = DateTime.Now;
            bool actv_ind = true;

            acctBE.FULL_NM = full_nm;
            acctBE.FINC_PTY_ID = finc_pty_id;
            acctBE.SUPRT_SERV_CUSTMR_GP_ID = suprt_serv_custmr_gp_id;
            acctBE.MSTR_ACCT_IND = mstr_acct_ind;
            acctBE.MARYLAND_RETRO_IND = false;
            acctBE.PEO_IND = false;
            acctBE.TPA_FUNDED_IND = true;
            acctBE.CREATE_DATE = crte_dt;
            acctBE.CREATE_USER_ID = crte_user_id;
            acctBE.ACTV_IND = actv_ind;

            acctBS.Update(acctBE);
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }
        
        #endregion


        /// <summary>
        ///A test for AccountSearchBS Constructor
        ///</summary>
        [TestMethod()]
        public void AccountSearchBSConstructorTest()
        {
            AccountSearchBS target = new AccountSearchBS();
            Assert.AreNotEqual(null, target);
        }

        /// <summary>
        ///A test for getAccounts
        ///</summary>
        [TestMethod()]
        public void getAccountsTestWithData()
        {
            IList<AccountSearchBE> expected = null;
            IList<AccountSearchBE> actual;
            actual = target.getAccounts(acctBE.FULL_NM, acctBE.FINC_PTY_ID, acctBE.CUSTMR_ID,
                                            acctBE.SUPRT_SERV_CUSTMR_GP_ID, "");
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getRelatedAccountNo
        ///</summary>
        [TestMethod()]
        public void getRelatedAccountNoData()
        {
            IList<AccountSearchBE> expected = null;
            IList<AccountSearchBE> actual;
            actual = target.getRelatedAccountNo(acctBE.CUSTMR_ID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getAccounts
        ///</summary>
        [TestMethod()]
        public void getAccountsTestWithNull()
        {
            AccountSearchBS target = new AccountSearchBS(); 
            int AccountID = 0; 
            int expected = 0; 
            IList<AccountSearchBE> actual;
            actual = target.getAccounts(AccountID);
            Assert.AreEqual(expected, actual.Count);
        }
    }
}
