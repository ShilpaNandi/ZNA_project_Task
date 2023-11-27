using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class BusinessUnitOfficeDA : DataAccessor<INT_ORG, BusinessUnitOfficeBE, AISDatabaseLINQDataContext>
    {

        public IList<BusinessUnitOfficeBE> GetBusinessUnitList()
        {
            IList<BusinessUnitOfficeBE> BusinessUnits = new List<BusinessUnitOfficeBE>();

            IQueryable<BusinessUnitOfficeBE> result = from res in this.Context.INT_ORGs
                                                      orderby res.full_name, res.city_nm, res.ofc_cd
                                                      select new BusinessUnitOfficeBE
                                                      {
                                                          INTRNL_ORG_ID = res.int_org_id,
                                                          FULL_NAME = res.full_name,
                                                          BSN_UNT_CD = res.bsn_unt_cd,
                                                          CITY_NM = (res.city_nm == null ? "" : res.city_nm),
                                                          OFC_CD = res.ofc_cd,
                                                          CREATED_DATE = res.crte_dt,
                                                          CREATED_USERID = res.crte_user_id,
                                                          UPDATED_DATE = res.updt_dt,
                                                          UPDATED_USERID = res.updt_user_id,
                                                          ACTV_IND = res.actv_ind
                                                      };
            BusinessUnits = result.ToList();

            return BusinessUnits;
        }

        public IList<BusinessUnitOfficeBE> GetBusinessUnits()
        {
            IList<BusinessUnitOfficeBE> BusinessUnits = new List<BusinessUnitOfficeBE>();

            IQueryable<BusinessUnitOfficeBE> result = from res in this.Context.INT_ORGs
                                                      orderby res.full_name, res.city_nm, res.ofc_cd
                                                      where res.actv_ind == true
                                                      select new BusinessUnitOfficeBE
                                                  {
                                                      INTRNL_ORG_ID = res.int_org_id,
                                                      FULL_NAME = res.full_name,
                                                      BSN_UNT_CD = res.bsn_unt_cd,
                                                      CITY_NM = (res.city_nm == null ? "" : res.city_nm),
                                                      OFC_CD = res.ofc_cd,
                                                      CREATED_DATE = res.crte_dt,
                                                      CREATED_USERID = res.crte_user_id,
                                                      UPDATED_DATE = res.updt_dt,
                                                      UPDATED_USERID = res.updt_user_id,
                                                      ACTV_IND = res.actv_ind
                                                  };
            BusinessUnits = result.ToList();

            return BusinessUnits;
        }
        public IList<LookupBE> GetBUOffForLookups()
        {
            IList<LookupBE> BusinessUnits = new List<LookupBE>();

            IQueryable<LookupBE> result = from res in this.Context.INT_ORGs
                                          where ((res.bsn_unt_cd + "/" + (res.city_nm == null ? "" : res.city_nm))!="N /A")
                                          orderby (res.bsn_unt_cd + "/" + (res.city_nm == null ? "" : res.city_nm))
                                          where res.actv_ind == true
                                          select new LookupBE
                                                      {
                                                          LookUpID = res.int_org_id,
                                                          LookUpName = res.bsn_unt_cd + "/" + (res.city_nm == null ? "" : res.city_nm)
                                                      };
            BusinessUnits = result.ToList();

            return BusinessUnits;
        }
        /// <summary>
        /// Invoked to fill the InvoiceInquiry DropDown
        /// </summary>
        /// <returns>IList<BusinessUnitOfficeBE></returns>
        public IList<BusinessUnitOfficeBE> GetBUOffList()
        {
            IList<BusinessUnitOfficeBE> BusinessUnits = new List<BusinessUnitOfficeBE>();

            IQueryable<BusinessUnitOfficeBE> result = from res in this.Context.INT_ORGs
                                                      where ((res.bsn_unt_cd + "/" + (res.city_nm == null ? "" : res.city_nm)) != "N /A")
                                                      orderby (res.bsn_unt_cd + "/" + (res.city_nm == null ? "" : res.city_nm))
                                                      where res.actv_ind == true
                                                      select new BusinessUnitOfficeBE
                                                      {
                                                          INTRNL_ORG_ID = res.int_org_id,
                                                          FULL_NAME = res.bsn_unt_cd + "/" + (res.city_nm == null ? "" : res.city_nm)
                                                      };
            BusinessUnits = result.ToList();

            return BusinessUnits;
        }
    }
}
