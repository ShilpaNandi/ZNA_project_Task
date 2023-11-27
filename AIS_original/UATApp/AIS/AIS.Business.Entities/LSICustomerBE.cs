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

    public class LSICustomerBE : BusinessEntity<LSI_CUSTMR>
    {
        public LSICustomerBE()
            : base()
        {

        }

        public int LSI_CUSTMR_ID { get { return Entity.lsi_custmr_id; } set { Entity.lsi_custmr_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int LSI_ACCT_ID { get { return Entity.lsi_acct_id; } set { Entity.lsi_acct_id = value; } }
        public string FULL_NAME { get { return Entity.full_nm; } set { Entity.full_nm = value; } }
        public Boolean? PRIM_IND { get { return Entity.prim_ind; } set { Entity.prim_ind = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public string IS_PRIM_IND { get { return ((Entity.prim_ind == true) ? "YES" : "NO"); } }
        public string IS_ACTV_IND { get { return ((Entity.actv_ind == true) ? "YES" : "NO"); } }

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }


        //Anil 08/12/2008
        private EntityRef<AccountBE> _ACC;
        public AccountBE ACC
        {
            get
            {
                if (_ACC.HasLoadedOrAssignedValue && _ACC.Entity.CUSTMR_ID == this.CUSTMR_ID)
                {
                    return _ACC.Entity;
                }
                else
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<CUSTMR, AccountBE, AISDatabaseLINQDataContext>
                        cc = new ZurichNA.LSP.Framework.DataAccess.DataAccessor<CUSTMR, AccountBE, AISDatabaseLINQDataContext>();
                    _ACC = new EntityRef<AccountBE>(cc.Load(this.CUSTMR_ID));
                }
                return _ACC.Entity;
            }
        }
    }

    public class LSIAllCustomersBE
    {
        public LSIAllCustomersBE()
            : base()
        {

        }

        public int LSI_ACCT_ID { get; set; }
        public string FULL_NAME { get; set; }
        public string NAME_ACCOUNTNO { get { return FULL_NAME + " - (" + LSI_ACCT_ID + ")"; } set { ;} }
    }
}
