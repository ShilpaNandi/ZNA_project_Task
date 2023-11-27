using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class PostingTransactionTypeBE : BusinessEntity<POST_TRNS_TYP>
    {
        public PostingTransactionTypeBE()
            : base()
        {
        }
        public int POST_TRANS_TYP_ID { get { return Entity.post_trns_typ_id; } set { Entity.post_trns_typ_id = value; } }
        public string TRANS_TXT { get { return Entity.trns_nm_txt; } set { Entity.trns_nm_txt = value; } }
        public int? TRNS_TYP_ID { get { return Entity.trns_typ_id; } set { Entity.trns_typ_id = value; } }
        public string MAIN_NBR { get { return Entity.main_nbr_txt; } set { Entity.main_nbr_txt = value; } }
        public string SUB_NBR { get { return Entity.sub_nbr_txt; } set { Entity.sub_nbr_txt = value; } }
        public string COMP_TXT { get { return Entity.comp_txt; } set { Entity.comp_txt = value; } }
        public Boolean? INVOICBL_IND { get { return Entity.invoicbl_ind; } set { Entity.invoicbl_ind = value; } }
        public Boolean? MISC_POSTS_IND { get { return Entity.post_ind; } set { Entity.post_ind = value; } }
        public Boolean? THRD_PTY_ADMIN_MNL_IND { get { return Entity.thrd_pty_admin_mnl_ind; } set { Entity.thrd_pty_admin_mnl_ind = value; } }
        public Boolean? ADJ_SUMRY_NOT_POST_IND { get { return Entity.adj_sumry_ind; } set { Entity.adj_sumry_ind = value; } }
        public Boolean? POL_REQR_IND { get { return Entity.pol_reqr_ind; } set { Entity.pol_reqr_ind = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int Created_UserID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime Created_Date { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }

        private string _TransTyp;
        public string TRANSACTIONTYPE { get { return _TransTyp; } set { _TransTyp = value; } }
        private string _TransName;
        public string TRANSACTIONNAME { get { return _TransName; } set { _TransName = value; } }
        public string _INVOICBL_IND_txt;
        public string INVOICBL_IND_txt { get { if (INVOICBL_IND == true) { return "YES"; } else { return "NO"; } } set { _INVOICBL_IND_txt = value; } }
        public string _MISC_POSTS_IND_txt;
        public string MISC_POSTS_IND_txt { get { if (MISC_POSTS_IND == true) { return "YES"; } else { return "NO"; } } set { _MISC_POSTS_IND_txt = value; } }
        public string _THRD_PTY_ADMIN_MNL_IND_txt;
        public string THRD_PTY_ADMIN_MNL_IND_txt { get { if (THRD_PTY_ADMIN_MNL_IND == true) { return "YES"; } else { return "NO"; } } set { _THRD_PTY_ADMIN_MNL_IND_txt = value; } }
        public string _ADJ_SUMRY_NOT_POST_IND_txt;
        public string ADJ_SUMRY_NOT_POST_IND_txt { get { if (ADJ_SUMRY_NOT_POST_IND == true) { return "YES"; } else { return "NO"; } } set { _ADJ_SUMRY_NOT_POST_IND_txt = value; } }
        public string _POL_REQR_IND_txt;
        public string POL_REQR_IND_txt { get { if (POL_REQR_IND == true) { return "YES"; } else { return "NO"; } } set { _POL_REQR_IND_txt = value; } }

    }
}