using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for PostingTransactionTypeBSTest and is intended
    ///to contain all PostingTransactionTypeBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PostingTransactionTypeBSTest
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
        ///A test for SaveSetupData
        ///</summary>
        [TestMethod()]
        public void SaveSetupDataTest()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            PostingTransactionTypeBE Data = new PostingTransactionTypeBE();  // TODO: Initialize to an appropriate value
            Data.POST_TRANS_TYP_ID = 1;
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SaveSetupData(Data);
            Assert.AreEqual(expected, actual);
            
        }
        /// <summary>
        ///A test for SaveSetupData with 0
        ///</summary>
        [TestMethod()]
        public void SaveSetupDataTest2()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            PostingTransactionTypeBE Data = new PostingTransactionTypeBE();  // TODO: Initialize to an appropriate value
            Data.POST_TRANS_TYP_ID = 0;
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SaveSetupData(Data);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for LoadData
        ///</summary>
        [TestMethod()]
        public void LoadDataTest()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            int PostTransSetupId = 0; // TODO: Initialize to an appropriate value
            PostingTransactionTypeBE expected = null; // TODO: Initialize to an appropriate value
            PostingTransactionTypeBE actual;
            actual = target.LoadData(PostTransSetupId);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IsPolicyRequires
        ///</summary>
        [TestMethod()]
        public void IsPolicyRequiresTest()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            int intPostTransTypID = 1; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsPolicyRequires(intPostTransTypID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPremAdjMiscInvoiceEditData
        ///</summary>
        [TestMethod()]
        public void getPremAdjMiscInvoiceEditDataTest()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            int intPostTransTypID = 0; // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> actual;
            actual = target.getPremAdjMiscInvoiceEditData(intPostTransTypID);
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getPremAdjMiscInvoiceData
        ///</summary>
        [TestMethod()]
        public void getPremAdjMiscInvoiceDataTest()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> actual;
            actual = target.getPremAdjMiscInvoiceData();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for getList
        ///</summary>
        [TestMethod()]
        public void getListTest()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> actual;
            actual = target.getList();
            Assert.AreNotEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for PostingTransactionTypeBS Constructor
        ///</summary>
        [TestMethod()]
        public void PostingTransactionTypeBSConstructorTest()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS();
            
        }
         /// <summary>
        ///A test for getTPAList
        ///</summary>
        [TestMethod()]
        public void getTPAList()
        {
            PostingTransactionTypeBS target = new PostingTransactionTypeBS(); // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> expected = null; // TODO: Initialize to an appropriate value
            IList<PostingTransactionTypeBE> actual;
            actual = target.getTPAList();
            Assert.AreNotEqual(expected, actual);
            
        }
    }
}
