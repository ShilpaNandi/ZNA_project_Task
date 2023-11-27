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
    public class CustomerContactBE : BusinessEntity<CUSTMR_PERS_REL>
    {
        public CustomerContactBE()
            : base()
        {

        }

        public int CUSTOMER_CONTACT_ID { get { return Entity.custmr_pers_rel_id; } set { Entity.custmr_pers_rel_id = value; } }
        public int CUSTOMER_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? PERSON_ID { get { return Entity.pers_id; } set { Entity.pers_id = value; } }
        public int? ROLE_ID { get { return Entity.rol_id; } set { Entity.rol_id = value; } }

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }

        public string FirstName { get { return PER.FORENAME; } set { ; } }
        public string LastName { get { return PER.SURNAME; } set { ;} }
        public string FULLNAME { get { return PER.SURNAME.Trim() + ", " + PER.FORENAME.Trim(); } set { ;} }
        public string POSTALADDRESS
        {
            get
            {
                string strAddr = string.Empty;
                if (POST_ADD_BE!=null)
                    strAddr = (ADDRESS1??"") + "-" + (ADDRESS2??"")
                        + (CITY??"") + "-" + (ZIP_CODE??"");
                return strAddr;
            }
        }

        public string DISPLAYTEXT { 
            get { 
                return string.Format("{0},{1}|{2},{3}", 
                    PER.SURNAME.Trim(), 
                    PER.FORENAME.Trim().PadRight(10, '\u00A0'),
                    ((CITY != null) ? CITY.Trim() : ""),
                    ((STATE_TXT != null) ? STATE_TXT.Trim() : ""));
            }
        }
        public string ACCOUNTNAME { get { return ACC.FULL_NM; } set { ;} }
        public string RESP_NAME { get { return Resplookup.LookUpName; } set { ;} }
        public string LOOKUPTYPE { get { return Resplookup.LookUpTypeName ; } set { ;} }

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

        public string ZIP_CODE
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
        
        //Anil 08/12/2008
        private EntityRef<PersonBE> _PER;
        public PersonBE PER
        {
            get
            {
                if (_PER.HasLoadedOrAssignedValue && _PER.Entity.PERSON_ID == this.PERSON_ID  && _PER.Entity.ACTIVE == true)
                {
                    return _PER.Entity;
                }
                else
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<PER, PersonBE, AISDatabaseLINQDataContext>
                        cc = new ZurichNA.LSP.Framework.DataAccess.DataAccessor<PER, PersonBE, AISDatabaseLINQDataContext>();
                    _PER = new EntityRef<PersonBE>(cc.Load(this.PERSON_ID));
                }
                return _PER.Entity;
            }
        }

        //Anil 08/12/2008
        private EntityRef<AccountBE> _ACC;
        public AccountBE ACC
        {
            get
            {
                if (_ACC.HasLoadedOrAssignedValue && _ACC.Entity.CUSTMR_ID  == this.CUSTOMER_ID)
                {
                    return _ACC.Entity;
                }
                else
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<CUSTMR, AccountBE, AISDatabaseLINQDataContext>
                        cc = new ZurichNA.LSP.Framework.DataAccess.DataAccessor<CUSTMR, AccountBE, AISDatabaseLINQDataContext>();
                    _ACC = new EntityRef<AccountBE>(cc.Load(this.CUSTOMER_ID));
                }
                return _ACC.Entity;
            }
        }

        //Anil 08/12/2008
        private EntityRef<LookupBE> _RespLookup;
        public LookupBE Resplookup
        {
            get
            {
                if (_RespLookup.HasLoadedOrAssignedValue && _RespLookup.Entity.LookUpID == this.ROLE_ID)
                {
                    return _RespLookup.Entity;
                }
                else
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>
                        cc = new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _RespLookup = new EntityRef<LookupBE>(cc.Load(this.ROLE_ID));
                }
                return _RespLookup.Entity;
            }
        }
    }
}

