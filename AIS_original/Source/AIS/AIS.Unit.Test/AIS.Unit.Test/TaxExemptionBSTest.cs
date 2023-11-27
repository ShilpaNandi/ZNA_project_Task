using ZurichNA.AIS.Business.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZurichNA.AIS.Business.Entities;
using System.Collections.Generic;
using System.Data;
using System;

namespace AIS.Unit.Test
{


    /// <summary>
    ///This is a test class for TaxexemptionBSTest and is intended
    ///to contain all TaxexemptionBSBSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TaxExemptionBSTest
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




        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            TaxExemptionBS target = new TaxExemptionBS(); // TODO: Initialize to an appropriate value
            TaxExemptionBE TaxExemptBE = new TaxExemptionBE(); // TODO: Initialize to an appropriate value

            TaxExemptBE.CREATE_DATE = Convert.ToDateTime("12/18/2009 12:04:27 AM");
            TaxExemptBE.CREATE_USER_ID = 0;
            TaxExemptBE.CUSTOMER_ID = 106;
            TaxExemptBE.ST_ID = 47;
            //TaxExemptBE.TAX_EXMP_SETUP_ID = 0;
            TaxExemptBE.PREM_ADJ_PGM_ID = 707;
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(TaxExemptBE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getTaxExemptSetupRow
        ///</summary>
        [TestMethod()]
        public void getTaxExemptSetupRow()
        {
            TaxExemptionBS target = new TaxExemptionBS(); // TODO: Initialize to an appropriate value
            int TaxExemptSetupID = 0; // TODO: Initialize to an appropriate value
            TaxExemptionBE expected = null;// TODO: Initialize to an appropriate value
            TaxExemptionBE actual;
            actual = target.getTaxExemptSetupRow(TaxExemptSetupID);
            Assert.AreNotEqual(expected, actual);
        }
        /// <summary>
        ///A test for isTaxExemptAlreadyExist
        ///</summary>
        [TestMethod()]
        public void isTaxExemptAlreadyExistTest()
        {
            TaxExemptionBS target = new TaxExemptionBS(); // TODO: Initialize to an appropriate value
            int intTaxExemptSetupID = 2; // TODO: Initialize to an appropriate value
            int st_id = 47;// TODO: Initialize to an appropriate value
            int intPrem_adj_pgm_ID = 707; // TODO: Initialize to an appropriate value
            int expected = 1; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.isTaxExemptAlreadyExist(intTaxExemptSetupID, st_id, intPrem_adj_pgm_ID);
            Assert.AreNotEqual(expected, actual);
        }


        /// <summary>
        ///A test for getILRFTaxSetupList
        ///</summary>
        [TestMethod()]
        public void getILRFTaxSetupListTest()
        {
            TaxExemptionBS target = new TaxExemptionBS(); // TODO: Initialize to an appropriate value
            int intPREM_ADJ_PGM_ID = 707; // TODO: Initialize to an appropriate value
            int intCUSTMR_ID = 106; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            IList<TaxExemptionBE> actual;
            actual = target.getILRFTaxSetupList(intPREM_ADJ_PGM_ID, intCUSTMR_ID);
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetStatesForEdit
        ///</summary>
        [TestMethod()]
        public void GetStatesForEditTest()
        {
            TaxExemptionBS target = new TaxExemptionBS(); // TODO: Initialize to an appropriate value
            int iLkupId = 0; // TODO: Initialize to an appropriate value
            IList<LookupBE> expected = null; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetStatesForEdit(iLkupId);
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetStates
        ///</summary>
        [TestMethod()]
        public void GetStatesTest()
        {
            TaxExemptionBS target = new TaxExemptionBS(); // TODO: Initialize to an appropriate value
            int expected = 47; // TODO: Initialize to an appropriate value
            IList<LookupBE> actual;
            actual = target.GetStates();
            Assert.AreNotEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for TaxExemption Constructor
        ///</summary>
        [TestMethod()]
        public void TaxExemptionBSConstructorTest()
        {
            TaxExemptionBS target = new TaxExemptionBS();
        }
    }
}
