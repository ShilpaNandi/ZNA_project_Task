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
    public class AccountBE : BusinessEntity<CUSTMR>
    {
        public AccountBE()
            : base()
        {

        }

        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? CUSTMR_REL_ID { get { return Entity.custmr_rel_id; } set { Entity.custmr_rel_id = value; } }
        public string FULL_NM { get { return Entity.full_nm; } set { Entity.full_nm = value; } }
        public string FINC_PTY_ID { get { return Entity.finc_pty_id; } set { Entity.finc_pty_id = value; } }
        public string SUPRT_SERV_CUSTMR_GP_ID { get { return Entity.suprt_serv_custmr_gp_id; } set { Entity.suprt_serv_custmr_gp_id  = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public Boolean? MSTR_ACCT_IND { get { return Entity.mstr_acct_ind; } set { Entity.mstr_acct_ind = value; } }
        public Boolean? PEO_IND { get { return Entity.peo_ind; } set { Entity.peo_ind = value; } }
        public Boolean? TPA_FUNDED_IND { get { return Entity.thrd_pty_admin_funded_ind; } set { Entity.thrd_pty_admin_funded_ind = value; } }
        public DateTime? THIRD_PARTY_ADMIN_FUNDED_DATE { get { return Entity.thrd_pty_admin_funded_dt; } set { Entity.thrd_pty_admin_funded_dt = value; } }
        public Boolean? MARYLAND_RETRO_IND { get { return Entity.md_retro_adj_ind; } set { Entity.md_retro_adj_ind = value; } }
        public DateTime? BKTCYBUYOUT_DATE { get { return Entity.bnkrpt_buyout_eff_dt; } set { Entity.bnkrpt_buyout_eff_dt = value; } }
        public int? BKTCYBUYOUT_ID { get { return Entity.bnkrpt_buyout_id; } set { Entity.bnkrpt_buyout_id = value; } }
        public Boolean? CUSTMR_REL_ACTV_IND { get { return Entity.custmr_rel_actv_ind; } set { Entity.custmr_rel_actv_ind = value; } }
        public string IS_CUSTMR_REL_ACTV_IND { get { return ((Entity.custmr_rel_actv_ind == true) ? "YES" : "NO"); } }
        public string IS_MSTR_ACCT_IND { get { return ((Entity.mstr_acct_ind == true) ? "YES" : "NO"); } }

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }

        private int _PERSON_ID;
        public int PERSON_ID { get { return _PERSON_ID; } set { _PERSON_ID = value;} }
    }


    public class AccountSearchBE : BusinessEntity<CUSTMR>
    {
        public AccountSearchBE()
            : base()
        {

        }

        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value;} }
        public int? CUSTMR_REL_ID { get { return Entity.custmr_rel_id; } set { Entity.custmr_rel_id = value; } }
        public string FULL_NM { get { return Entity.full_nm; } set { Entity.full_nm=value;} }
        public Boolean? MSTR_ACCT_IND { get { return Entity.mstr_acct_ind; } set { Entity.mstr_acct_ind = value; } }
        public string IS_MSTR_ACCT { get { return ((Entity.mstr_acct_ind == true) ? "YES" : "NO"); } }
        public long FINC_PTY_ID { get {
            return ((Entity.finc_pty_id==null ||Entity.finc_pty_id.Trim().Length == 0) ? 0 : long.Parse(Entity.finc_pty_id));
        }
            set { Entity.finc_pty_id = value.ToString(); }
        }
        public long SUPRT_SERV_CUSTMR_GP_ID { get {
            return ((Entity.suprt_serv_custmr_gp_id== null||Entity.suprt_serv_custmr_gp_id.Trim().Length == 0) ? 0 : long.Parse(Entity.suprt_serv_custmr_gp_id));
        }
            set { Entity.suprt_serv_custmr_gp_id = value.ToString(); }
        }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; ;} }
        public string POSTAL_ADDRESS { get; set; }
    }

  
}