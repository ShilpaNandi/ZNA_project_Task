using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using log4net;

namespace ZurichNA.AIS.Business.Entities
{
    public class PersonBE : BusinessEntity<PER>
    {
        public PersonBE()
            : base()
        {

        }

        public int PERSON_ID { get { return Entity.pers_id; } set { Entity.pers_id = value; } }
        public string FORENAME { get { return Entity.forename; } set { Entity.forename = value; } }
        public string SURNAME { get { return Entity.surname; } set { Entity.surname = value; } }
        public string FULLNAME { get; set; }
        public int? TITLE_ID { get { return Entity.prefx_ttl_id; } set { Entity.prefx_ttl_id = value; } }
        public string TELEPHONE1 { get { return Entity.phone_nbr_1_txt; } set { Entity.phone_nbr_1_txt = value; } }
        public string TELEPHONE2 { get { return Entity.phone_nbr_2_txt; } set { Entity.phone_nbr_2_txt = value; } }
        public string FAX { get { return Entity.fax_nbr_txt; } set { Entity.fax_nbr_txt = value; } }
        public string EMAIL { get { return Entity.email_txt; } set { Entity.email_txt = value; } }
        public string USERID { get { return Entity.external_reference; } set { Entity.external_reference = value; } }
        public int? ACCTSETUP_QC { get { return Entity.acct_qlty_cntrl_pct_id; } set { Entity.acct_qlty_cntrl_pct_id = value; } }
        public int? ADJUSTMENT_QC { get { return Entity.adj_qlty_cntrl_pct_id; } set { Entity.adj_qlty_cntrl_pct_id = value; } }
        public int? ARIES_QC { get { return Entity.aries_qlty_cntrl_pct_id; } set { Entity.aries_qlty_cntrl_pct_id = value; } }
        public int? MANAGERID { get { return Entity.mgr_id; } set { Entity.mgr_id = value; } }
        public string MANAGER { get { return Entity.forename; } set { Entity.forename = value; } }
        public DateTime? EXPIRYDATE { get { return Entity.expi_dt; } set { Entity.expi_dt = value; } }
        public DateTime? EFFECTIVEDATE { get { return Entity.eff_dt; } set { Entity.eff_dt = value; } }
        public DateTime CREATEDDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? UPDATEDDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? EXTERNAL_ORGN_ID { get { return Entity.extrnl_org_id; } set { Entity.extrnl_org_id = value; } }
        public string EXTERNAL_ORGN_TXT { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public int? selPOSTALADDRESSID { get; set; }
        public int CREATEDUSER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int? UPDATEDUSER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int? CONCTACT_TYPE_ID { get { return Entity.conctc_typ_id; } set { Entity.conctc_typ_id = value; } }
        public Boolean? ACTIVE { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public string CONTACTTYPE { get; set; }
        public string TITLE { get { return ((TitleLookUp != null) ? TitleLookUp.LookUpName : String.Empty); } }
        //public string STATE_TEXT { get { return StateLookUp.LookUpName; } }
        public string ACCTSETUPQCTEXT { get; set; }
        public string ADJSETUPQCTEXT { get; set; }
        public int POST_ADDR_ID{ get; set; }
        public string TELEPHONE1_EXTNS { get { return Entity.phone_nbr_1_extns; } set { Entity.phone_nbr_1_extns = value; } }

       public PostalAddressBE POST_ADD_BE { get; set; }

       public string ADDRESS1
       {
           get
           {
               if (POST_ADD_BE != null)
               {
                   return POST_ADD_BE.ADDRESS1;
               }
               else
                   return null;
           }
       }

       public string ADDRESS2
       {
           get
           {
               if (POST_ADD_BE != null)
               {
                   return POST_ADD_BE.ADDRESS2;
               }
               else
                   return null;
           }
       }
       public string CITY
       {
           get
           {
               if (POST_ADD_BE != null)
               {
                   return POST_ADD_BE.CITY;
               }
               else
                   return null;
           }
       }


       public string ZIPCODE
       {
           get
           {
               if (POST_ADD_BE != null)
               {
                   return POST_ADD_BE.ZIP_CODE;
               }
               else
                   return null;
           }

       }
       public int STATE
       {
           get
           {
               if (POST_ADD_BE != null)
               {
                   return POST_ADD_BE.STATE_ID;
               }
               else
                   return 0;
           }
       }
       public string STATE_TXT
       {
           get
           {
               if (POST_ADD_BE != null)
               {
                   return POST_ADD_BE.STATE_TXT;
               }
               else
                   return null;
           }
       }
       public int POSTALADDRESSID
       {
           get
           {
               if (POST_ADD_BE != null)
               {
                   return POST_ADD_BE.POSTALADDRESSID;
               }
               else
                   return 0;
           }
       }




       //////private EntityRef<LookupBE> _stateLookUp;
       //////public LookupBE StateLookUp
       //////{
       //////    get
       //////    {
       //////        if (STATE_ID > 0)
       //////        {
       //////            ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext> da =
       //////                new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
       //////            _stateLookUp = new EntityRef<LookupBE>(da.Load(STATE_ID));
       //////        }
       //////        return _stateLookUp.Entity;
       //////    }
       //////}

        private EntityRef<LookupBE> _TitleLookUp;
        public LookupBE TitleLookUp
        {
            get
            {
                if (TITLE_ID > 0)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _TitleLookUp = new EntityRef<LookupBE>(da.Load(TITLE_ID));
                }
                return _TitleLookUp.Entity;
            }
        }

    }
}


