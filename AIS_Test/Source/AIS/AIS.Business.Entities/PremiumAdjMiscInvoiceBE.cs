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
    public class PremiumAdjMiscInvoiceBE : BusinessEntity<PREM_ADJ_MISC_INVC>
    {
        public PremiumAdjMiscInvoiceBE()
            : base()
        {

        }

        public int PREM_ADJ_MISC_INVC_ID { get { return Entity.prem_adj_misc_invc_id; } set { Entity.prem_adj_misc_invc_id = value; } }
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public decimal? POST_AMT { get { return Entity.post_amt; } set { Entity.post_amt = value; } }
        public string POL_SYM_TXT { get { return Entity.pol_sym_txt; } set { Entity.pol_sym_txt = value; } }
        public string POL_NBR_TXT { get { return Entity.pol_nbr_txt; } set { Entity.pol_nbr_txt = value; } }
        public string POL_MODULUS_TXT { get { return Entity.pol_modulus_txt; } set { Entity.pol_modulus_txt = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
//        public Boolean? MISC_POSTS_IND { get { return Entity.POST_TRNS_TYP.post_ind; } set { Entity.POST_TRNS_TYP.post_ind = value; } }
//        public Boolean? ADJ_SUMRY_POST_FLAG_IND { get { return Entity.POST_TRNS_TYP.adj_sumry_ind; } set { Entity.POST_TRNS_TYP.adj_sumry_ind = value; } }
        public int? POST_TRANS_TYP_ID { get { return Entity.post_trns_typ_id; } set { Entity.post_trns_typ_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public string POLICYNUMBER { get; set; }

        private EntityRef<PostingTransactionTypeBE> _PostTrnsTyp;
        public PostingTransactionTypeBE PostTrnsTyp
        {
            get
            {

                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        POST_TRNS_TYP, PostingTransactionTypeBE, AISDatabaseLINQDataContext> da =
                         new ZurichNA.LSP.Framework.DataAccess.DataAccessor<POST_TRNS_TYP, PostingTransactionTypeBE, AISDatabaseLINQDataContext>();
                    _PostTrnsTyp = new EntityRef<PostingTransactionTypeBE>(da.Load(POST_TRANS_TYP_ID));
                    return _PostTrnsTyp.Entity;


                }

            }

        }
        public string POSTTRNSTYPE
        {

            get
            {

                if (PostTrnsTyp == null)
                    return null;
                else
                    return PostTrnsTyp.TRANS_TXT;


            }
            set { ;}
        }

        public bool? ADJ_SUMRY_POST_FLAG_IND
        {

            get
            {

                if (PostTrnsTyp == null)
                    return null;
                else
                    return PostTrnsTyp.ADJ_SUMRY_NOT_POST_IND;
            }
            set { ;}
        }
        public bool? POL_REQR_IND
        {

            get
            {

                if (PostTrnsTyp == null)
                    return null;
                else
                    return PostTrnsTyp.POL_REQR_IND;
            }
            set { ;}
        }

        public bool? MISC_POSTS_IND
        {

            get
            {
                if (PostTrnsTyp == null)
                    return null;
                else
                    return PostTrnsTyp.MISC_POSTS_IND;
            }
            set { ;}
        }
      
    }
}
