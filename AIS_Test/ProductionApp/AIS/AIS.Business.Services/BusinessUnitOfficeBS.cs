using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;

namespace ZurichNA.AIS.Business.Logic
{
    public class BusinessUnitOfficeBS : BusinessServicesBase<BusinessUnitOfficeBE, BusinessUnitOfficeDA>
    {

        public IList<BusinessUnitOfficeBE> GetBusinessUnitList()
        {
            IList<BusinessUnitOfficeBE> BusinessUnitList = new List<BusinessUnitOfficeBE>();

            BusinessUnitOfficeDA BusinessUnits = new BusinessUnitOfficeDA();

            BusinessUnitList = BusinessUnits.GetBusinessUnitList();

            return BusinessUnitList;
        }

        public IList<BusinessUnitOfficeBE> GetBusinessUnits()
        {
            IList<BusinessUnitOfficeBE> BusinessUnitList = new List<BusinessUnitOfficeBE>();

            BusinessUnitOfficeDA BusinessUnits = new BusinessUnitOfficeDA();

            BusinessUnitList = BusinessUnits.GetBusinessUnits();


            BusinessUnitOfficeBE buBE = new BusinessUnitOfficeBE();
            buBE.INTRNL_ORG_ID  = 0;
            buBE.FULL_NAME = "(Select)";
            BusinessUnitList.Insert(0, buBE);
            
            return BusinessUnitList;
        }


        public IList<LookupBE> GetBUOffForLookups()
        {
            IList<LookupBE> BusinessUnitList = new List<LookupBE>();

            BusinessUnitOfficeDA BusinessUnits = new BusinessUnitOfficeDA();

            BusinessUnitList = BusinessUnits.GetBUOffForLookups();

            LookupBE buBE = new LookupBE();
            LookupBE buBE1 = new LookupBE();
            buBE.LookUpID = 0;
            buBE.LookUpName = "(Select)";
            BusinessUnitList.Insert(0, buBE);
            buBE1.LookUpID = 1;
            buBE1.LookUpName = "Not Applicable";
            BusinessUnitList.Insert(1, buBE1);
            return BusinessUnitList;
        }
        /// <summary>
        /// Invoked to fill the Invoice Inquiry Drop Down
        /// </summary>
        /// <returns></returns>
        public IList<BusinessUnitOfficeBE> GetBUOffList()
        {
            IList<BusinessUnitOfficeBE> BusinessUnitList = new List<BusinessUnitOfficeBE>();

            BusinessUnitOfficeDA BusinessUnits = new BusinessUnitOfficeDA();

            BusinessUnitList = BusinessUnits.GetBUOffList();

            BusinessUnitOfficeBE buBE = new BusinessUnitOfficeBE();
            BusinessUnitOfficeBE buBE1 = new BusinessUnitOfficeBE();
            buBE.INTRNL_ORG_ID = 0;
            buBE.FULL_NAME = "(Select)";
            BusinessUnitList.Insert(0, buBE);
            buBE1.INTRNL_ORG_ID = 1;
            buBE1.FULL_NAME = "Not Applicable";
            BusinessUnitList.Insert(1, buBE1);
            return BusinessUnitList;
        }
    }
}
