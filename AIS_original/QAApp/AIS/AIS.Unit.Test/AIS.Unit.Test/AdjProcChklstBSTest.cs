using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;

namespace AIS.Unit.Test
{
    
    
    /// <summary>
    ///This is a test class for AdjProcChklstBSTest and is intended
    ///to contain all AdjProcChklstBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdjProcChklstBSTest
    {

        static Qtly_Cntrl_ChklistBE ChklstBE;
        static AdjProcChklstBS target;
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
            target = new AdjProcChklstBS();
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
            ChklstBE = new Qtly_Cntrl_ChklistBE();
            target = new AdjProcChklstBS();
            
            ChklstBE.CHECKLISTITEM_ID = 19;
            
            ChklstBE.CreatedDate = System.DateTime.Now;
            ChklstBE.CreatedUser_ID = 1;
            target.Update(ChklstBE);
        }
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AddTestWithNULL()
        {
            AdjProcChklstBS target = new AdjProcChklstBS();
            Qtly_Cntrl_ChklistBE ChklstBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(ChklstBE);
              Assert.AreEqual(expected, actual);

        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void AddTestWithData()
        {
            Qtly_Cntrl_ChklistBE ChklstBE1 = new Qtly_Cntrl_ChklistBE();
            AdjProcChklstBS ltarget = new AdjProcChklstBS();
            bool expected = true;
            bool actual;
            ChklstBE1.CHECKLISTITEM_ID = 20;
            
            ChklstBE1.CreatedDate = System.DateTime.Now;
            ChklstBE1.CreatedUser_ID = 1;
            actual=ltarget.Update(ChklstBE1);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestWithData()
        {
            ChklstBE = new Qtly_Cntrl_ChklistBE();
            target = new AdjProcChklstBS();
            bool expected = true;
            bool actual;
            ChklstBE.CHECKLISTITEM_ID = 19;

            ChklstBE.CreatedDate = System.DateTime.Now;
            ChklstBE.CreatedUser_ID = 1;
            ChklstBE.UpdatedDate = System.DateTime.Now;
            ChklstBE.UpdatedUserID = 1;
            actual=target.Update(ChklstBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        ///
        [TestMethod()]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void UpdateTestwithNull()
        {
            AdjProcChklstBS target = new AdjProcChklstBS();
            Qtly_Cntrl_ChklistBE ChklstBE = null;
            bool expected = false;
            bool actual;
            actual = target.Update(ChklstBE);
            Assert.AreEqual(expected, actual);
        }

       

        /// <summary>
        ///A test for getRelatedChklstItems
        ///</summary>
        [TestMethod()]
        public void getRelatedChklstItemsTest()
        {
           
            
            
            AdjProcChklstBS target = new AdjProcChklstBS();
            int ChklstID = 1; 
            int prmAdjID = 1; 
            int custID = 1;
            int prgPrdID = 1;
            int expected = 1;

            IList<Qtly_Cntrl_ChklistBE> actual;
            actual = target.getRelatedChklstItems(ChklstID, prmAdjID, custID, prgPrdID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for getQltyCntrlRow
        ///</summary>
        [TestMethod()]
        public void getQltyCntrlRowTest()
        {
            Qtly_Cntrl_ChklistBE expected = null;
            Qtly_Cntrl_ChklistBE actual;
            actual = target.getQltyCntrlRow(ChklstBE.QualityControlChklst_ID);
            if (actual.IsNull()) actual = null;
            Assert.AreNotEqual(expected, actual);
            
            
        }

        /// <summary>
        ///A test for getAllAdjChklstItems
        ///</summary>
        [TestMethod()]
        public void getAllAdjChklstItemsTest()
        {
            int expected = 1;
            IList<Qtly_Cntrl_ChklistBE> actual;
            actual = target.getAllAdjChklstItems();
            Assert.AreNotEqual(expected, actual.Count);
            
        }

        /// <summary>
        ///A test for AdjProcChklstBS Constructor
        ///</summary>
        [TestMethod()]
        public void AdjProcChklstBSConstructorTest()
        {
            AdjProcChklstBS target = new AdjProcChklstBS();
        }
    }
}
