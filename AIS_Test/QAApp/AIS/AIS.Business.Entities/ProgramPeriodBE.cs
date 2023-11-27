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
    /// Inherit all properties and method from PGM_PERD class created by 
    /// "LINQ to SQL" tool
    public class ProgramPeriodBE : BusinessEntity<PREM_ADJ_PGM>
    {
        public ProgramPeriodBE()
            : base()
        {

        }

        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int? BRKR_CONCTC_ID { get { return Entity.brkr_conctc_id; } set { Entity.brkr_conctc_id = value; } }
        public DateTime? STRT_DT { get { return Convert.ToDateTime(Entity.strt_dt.Value.ToShortDateString()); } set { Entity.strt_dt = value; } }
        public DateTime? PLAN_END_DT { get { return Entity.plan_end_dt; } set { Entity.plan_end_dt = value; } }
        public DateTime? VALN_MM_DT { get { return Entity.valn_mm_dt; } set { Entity.valn_mm_dt = value; } }
        public int? INCUR_CONV_MMS_CNT { get { return Entity.incur_conv_mms_cnt; } set { Entity.incur_conv_mms_cnt = value; } }
        public int? ADJ_FREQ_MMS_INTVRL_CNT { get { return Entity.adj_freq_mms_intvrl_cnt; } set { Entity.adj_freq_mms_intvrl_cnt = value; } }
        public Boolean? INCLD_CAPTV_PAYKND_IND { get { return Entity.incld_captv_payknd_ind; } set { Entity.incld_captv_payknd_ind = value; } }
        public Boolean? AVG_TAX_MULTI_IND { get { return Entity.avg_tax_multi_ind; } set { Entity.avg_tax_multi_ind = value; } }
        public decimal? TAX_MULTI_FCTR_RT { get { return Entity.tax_multi_fctr_rt; } set { Entity.tax_multi_fctr_rt = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        
        public DateTime? LOS_SENS_INFO_STRT_DT { get { return Entity.los_sens_info_strt_dt; } set { Entity.los_sens_info_strt_dt = value; } }
        public DateTime? LOS_SENS_INFO_END_DT { get { return Entity.los_sens_info_end_dt; } set { Entity.los_sens_info_end_dt = value; } }
        public int? FST_ADJ_MMS_FROM_INCP_CNT { get { return Entity.fst_adj_mms_from_incp_cnt; } set { Entity.fst_adj_mms_from_incp_cnt = value; } }
        public DateTime? FNL_ADJ_DT { get { return Entity.fnl_adj_dt; } set { Entity.fnl_adj_dt = value; } }
        public int? BRKR_ID { get { return Entity.brkr_id; } set { Entity.brkr_id = value; } }
        public DateTime? NXT_VALN_DT { get { return Entity.nxt_valn_dt; } set { Entity.nxt_valn_dt = value; } }
        public DateTime? NXT_VALN_DT_PREM { get { return Entity.nxt_valn_dt; } set { Entity.nxt_valn_dt = value; } }
        public DateTime? PREV_VALN_DT { get { return Entity.prev_valn_dt; } set { Entity.prev_valn_dt = value; } }
        public decimal? COMB_ELEMTS_MAX_AMT { get { return Entity.comb_elemts_max_amt; } set { Entity.comb_elemts_max_amt = value; } }
        public decimal? PEO_PAY_IN_AMT { get { return Entity.peo_pay_in_amt; } set { Entity.peo_pay_in_amt = value; } }
        public int? FST_ADJ_NON_PREM_MMS_CNT { get { return Entity.fst_adj_non_prem_mms_cnt; } set { Entity.fst_adj_non_prem_mms_cnt = value; } }
        public int? FREQ_NON_PREM_MMS_CNT { get { return Entity.freq_non_prem_mms_cnt; } set { Entity.freq_non_prem_mms_cnt = value; } }
        public DateTime? FNL_ADJ_NON_PREM_DT { get { return Entity.fnl_adj_non_prem_dt; } set { Entity.fnl_adj_non_prem_dt = value; } }
        public DateTime? NXT_VALN_DT_NON_PREM_DT { get { return Entity.nxt_valn_dt_non_prem_dt; } set { Entity.nxt_valn_dt_non_prem_dt = value; } }
        public DateTime? PREV_VALN_DT_NON_PREM_DT { get { return Entity.prev_valn_dt_non_prem_dt; } set { Entity.prev_valn_dt_non_prem_dt = value; } }
        public DateTime? FST_ADJ_NON_PREM_DT { get { return Entity.fst_adj_non_prem_dt; } set { Entity.fst_adj_non_prem_dt = value; } }
        public Boolean? ZNA_SERV_COMP_CLM_HNDL_FEE_IND { get { return Entity.zna_serv_comp_clm_hndl_fee_ind; } set { Entity.zna_serv_comp_clm_hndl_fee_ind = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public int? PGM_TYP_ID { get { return Entity.pgm_typ_id; } set { Entity.pgm_typ_id = value; } }
        public int? BNKRPT_BUYOUT_ID { get { return Entity.bnkrpt_buyout_id; } set { Entity.bnkrpt_buyout_id = value; } }
        public DateTime? BNKRPT_BUYOUT_EFF_DT { get { return Entity.bnkrpt_buyout_eff_dt; } set { Entity.bnkrpt_buyout_eff_dt = value; } }
        public int? PAID_INCUR_TYP_ID { get { return Entity.paid_incur_typ_id; } set { Entity.paid_incur_typ_id = value; } }
        public Boolean? AGMT_ALOC_LOS_ADJ_EXPS_IND { get { return Entity.agmt_aloc_los_adj_exps_ind; } set { Entity.agmt_aloc_los_adj_exps_ind = value; } }
        public Boolean? AGMT_UNALOCTD_LOS_ADJ_IND { get { return Entity.agmt_unaloctd_los_adj_ind; } set { Entity.agmt_unaloctd_los_adj_ind = value; } }
        public Boolean? AGMT_LOS_BASE_ASSES_IND { get { return Entity.agmt_los_base_asses_ind; } set { Entity.agmt_los_base_asses_ind = value; } }
        public Boolean? LSI_ALOC_LOS_ADJ_EXPS_IND { get { return Entity.lsi_aloc_los_adj_exps_ind; } set { Entity.lsi_aloc_los_adj_exps_ind = value; } }
        public Boolean? LSI_UNALOCTD_LOS_ADJ_IND { get { return Entity.lsi_unaloctd_los_adj_ind; } set { Entity.lsi_unaloctd_los_adj_ind = value; } }
        public Boolean? LSI_LOS_BASE_ASSES_IND { get { return Entity.lsi_los_base_asses_ind; } set { Entity.lsi_los_base_asses_ind = value; } }
        public Boolean? AGMT_PAID_INCUR_IND { get { return Entity.agmt_paid_incur_ind; } set { Entity.agmt_paid_incur_ind = value; } }
        public DateTime? QLTY_CNTRL_DT { get { return Entity.qlty_cntrl_dt; } set { Entity.qlty_cntrl_dt = value; } }
        public string QLTY_CMMNT_TXT { get { return Entity.qlty_cmmnt_txt; } set { Entity.qlty_cmmnt_txt = value; } }
        public int? MSTR_ERND_RETRO_PREM_FRMLA_ID { get { return Entity.mstr_ernd_retro_prem_frmla_id; } set { Entity.mstr_ernd_retro_prem_frmla_id = value; } }
        public int? BSN_UNT_OFC_ID { get { return Entity.bsn_unt_ofc_id; } set { Entity.bsn_unt_ofc_id = value; ;} }
        public Boolean? INCLD_LEGEND_IND { get { return Entity.incld_legend_ind; } set { Entity.incld_legend_ind = value; } }
        public DateTime? FST_ADJ_DT { get { return Entity.fst_adj_dt; } set { Entity.fst_adj_dt = value; } }
        public DateTime? LSI_RETRIEVE_FROM_DT { get { return Entity.lsi_retrieve_from_dt; } set { Entity.lsi_retrieve_from_dt = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int? QUALITYCONTROL_PERSON_ID { get { return Entity.qlty_cntrl_pers_id; } set { Entity.qlty_cntrl_pers_id = value; } }
        public string PersonName { get; set; }
        public bool? CUSTMR_ACTIVE { get; set; }
        public Boolean? VOID_IND { get; set; }
        public Boolean? RSVN_IND { get; set; }
        //this is used in Invvoice Driver to update
        public int? PRIOR_PREM_ADJ_ID { get { return Entity.prior_prem_adj_id; } set { Entity.prior_prem_adj_id = value; } }
        //This is used for Account assignment
        public string CUSTMR_NAME
        { get; set; }
        public int? PERS_ID
        { get; set; }
        //used in Adjustment Invoicing Dashboard
        public int? ROL_ID { get; set; }
        public int? PREMIUMADJNUM { get; set; }
        public int? PREMIUMADJNUMLOSS { get; set; }
        public string CALC_ADJ_STS_CODE { get; set; }
        public int? LSI_Custmr_Count { get; set; }
        public int? ILRF_Setup_Count { get; set; }
        public string PROGRAMTYPE { get; set; }
        public int? PREM_ADJ_PER_ID { get; set; }
        public string BROKER { get; set; }
        public string BUOFFFICE { get; set; }
        public string PROGRAMSTATUS { get; set; }
        public string PROGRAMSTATUSID { get; set; }
        public string PROGRAMPERIOD_ST_DATE { get; set; }
        public string PROGRAMPERIOD_END_DATE { get; set; }
        public string VALUATION_DATE { get; set; }
        public string PROGRAMPERIOD_ST_END_DATE { get; set; }

        public int? CUSTMR_PERS_REL_pers_id { get; set; }
        public int? CUSTMR_PERS_REL_role_id { get; set; }
        
        public string STARTDATE_ENDDATE
        {
            get
            {
                if (Entity.strt_dt == null || Entity.plan_end_dt == null) return null;
                else return (Entity.strt_dt.Value.ToShortDateString() + " - " + Entity.plan_end_dt.Value.ToShortDateString());
            }
            set { ;}
        }
        public string PREMADJSTS { get; set; }
        private EntityRef<BusinessUnitOfficeBE> _IntlOrganization;
        /// <summary>
        /// Lazy loaded Internal Organization of record business entity.
        /// </summary>
        public BusinessUnitOfficeBE IntlOrganization
        {
            get
            {
                if (BSN_UNT_OFC_ID.HasValue)
                {

                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                       INT_ORG, BusinessUnitOfficeBE, AISDatabaseLINQDataContext> da =
                       new ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                           INT_ORG, BusinessUnitOfficeBE, AISDatabaseLINQDataContext>();
                    _IntlOrganization = new EntityRef<BusinessUnitOfficeBE>(da.Load(BSN_UNT_OFC_ID));
                    return _IntlOrganization.Entity;
                }
                else
                    return null;
            }
        }

        private EntityRef<BrokerBE> _Broker;
        /// <summary>
        /// Lazy loaded broker of record business entity.
        /// </summary>
        public BrokerBE Broker
        {
            get
            {
                if (BRKR_ID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        EXTRNL_ORG, BrokerBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<EXTRNL_ORG, BrokerBE, AISDatabaseLINQDataContext>();
                    _Broker = new EntityRef<BrokerBE>(da.Load(BRKR_ID));
                    return _Broker.Entity;
                }
                else
                    return null;
            }
        }

        private EntityRef<LookupBE> _Lookup;
        /// <summary>
        /// Lazy loaded lookup of record business entity.
        /// </summary>
        public LookupBE Lookup
        {
            get
            {
                if (PGM_TYP_ID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _Lookup = new EntityRef<LookupBE>(da.Load(PGM_TYP_ID));
                    return _Lookup.Entity;
                }
                else
                    return null;
            }
        }

        //private EntityRef<PersonBE> _PERSON;
        ///// <summary>
        ///// Lazy loaded lookup of record business entity.
        ///// </summary>
        //public PersonBE PERSON
        //{
        //    get
        //    {
        //        if (q.HasValue)
        //        {
        //            ZurichNA.LSP.Framework.DataAccess.DataAccessor<
        //                EXTRNL_ORG, BrokerBE, AISDatabaseLINQDataContext> da =
        //                new ZurichNA.LSP.Framework.DataAccess.DataAccessor<EXTRNL_ORG, BrokerBE, AISDatabaseLINQDataContext>();
        //            _Lookup = new EntityRef<BrokerBE>(da.Load(PGM_TYP_ID));
        //            return _Lookup.Entity;
        //        }
        //        else
        //            return null;
        //    }
        //}
        // Additional properties to further define the Program Period class.
        public string BUSINESSUNITNAME
        {
            get
            {
                if (IntlOrganization == null) return null;
                else return (IntlOrganization.FULL_NAME + "/" + IntlOrganization.CITY_NM).Trim();

            }
            set { ;}
        }

        public string PROGRAMPERIOD
        {
            get
            {
                if (Entity.strt_dt.Value.ToShortDateString() == null || Entity.plan_end_dt.Value.ToShortDateString() == null) return null;
                else return (Entity.strt_dt.Value.ToShortDateString() + " - " + Entity.plan_end_dt.Value.ToShortDateString());

            }
            set { ;}
        }

        public string VALUATIONDATE
        {
            get
            {
                if (Entity.valn_mm_dt.Value.ToShortDateString() == null) return null;
                else return (Entity.valn_mm_dt.Value.ToShortDateString());

            }
            set { ;}
        }


        //public string QUALITYREVIEWERNAME
        //{
        //    get
        //    {
        //        if (Entity.LKUP == null) return null;
        //        else return (Entity.PER.surname + "/" +  Entity.PER.forename);
        //    }
        //    set { ;}
        //}

        public string BROKERNAME
        {
            get
            {
                if (Broker == null) return null;
                else
                    return Broker.FULL_NAME;
            }
            set { ;}
        }

        public string PROGRAMTYPENAME
        {
            get
            {
                if (Lookup == null)
                    return null;
                else
                    return Lookup.LookUpName;
            }
            set { ;}
        }


    }

    public class ProgramPeriodListBE : BusinessEntity<PREM_ADJ_PGM>
    {
        public ProgramPeriodListBE()
            : base()
        {

        }

        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public DateTime? STRT_DT { get { return Convert.ToDateTime(Entity.strt_dt.Value.ToShortDateString()); } set { Entity.strt_dt = value; } }
        public DateTime? PLAN_END_DT { get { return Convert.ToDateTime(Entity.plan_end_dt.Value.ToShortDateString()); } set { Entity.plan_end_dt = value; } }
        public DateTime? VALN_MM_DT { get { return Convert.ToDateTime(Entity.valn_mm_dt.Value.ToShortDateString()); } set { Entity.valn_mm_dt = value; } }
        public int? BRKR_ID { get { return Entity.brkr_id; } set { Entity.brkr_id = value; } }
        public int? PGM_TYP_ID { get { return Entity.pgm_typ_id; } set { Entity.pgm_typ_id = value; } }
        public int? BSN_UNT_OFC_ID { get { return Entity.bsn_unt_ofc_id; } set { Entity.bsn_unt_ofc_id = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }

        private EntityRef<BusinessUnitOfficeBE> _IntlOrganization;
        /// <summary>
        /// Lazy loaded Internal Organization of record business entity.
        /// </summary>
        public BusinessUnitOfficeBE IntlOrganization
        {
            get
            {
                if (BSN_UNT_OFC_ID.HasValue)
                {

                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                       INT_ORG, BusinessUnitOfficeBE, AISDatabaseLINQDataContext> da =
                       new ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                           INT_ORG, BusinessUnitOfficeBE, AISDatabaseLINQDataContext>();
                    _IntlOrganization = new EntityRef<BusinessUnitOfficeBE>(da.Load(BSN_UNT_OFC_ID));
                    return _IntlOrganization.Entity;
                }
                else
                    return null;
            }
        }

        private EntityRef<BrokerBE> _Broker;
        /// <summary>
        /// Lazy loaded broker of record business entity.
        /// </summary>
        public BrokerBE Broker
        {
            get
            {
                if (BRKR_ID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        EXTRNL_ORG, BrokerBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<EXTRNL_ORG, BrokerBE, AISDatabaseLINQDataContext>();
                    _Broker = new EntityRef<BrokerBE>(da.Load(BRKR_ID));
                    return _Broker.Entity;
                }
                else
                    return null;
            }
        }

        private EntityRef<LookupBE> _Lookup;
        /// <summary>
        /// Lazy loaded lookup of record business entity.
        /// </summary>
        public LookupBE Lookup
        {
            get
            {
                if (PGM_TYP_ID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _Lookup = new EntityRef<LookupBE>(da.Load(PGM_TYP_ID));
                    return _Lookup.Entity;
                }
                else
                    return null;
            }
        }

        // Additional properties to further define the Program Period class.
        public string BUSINESSUNITNAME
        {
            get
            {
                if (IntlOrganization == null) return null;
                else return (IntlOrganization.FULL_NAME + "/" + IntlOrganization.CITY_NM);

            }
            set { ;}
        }

        public string BROKERNAME
        {
            get
            {
                if (Broker == null) return null;
                else
                    return Broker.FULL_NAME;
            }
            set { ;}
        }

        public string PROGRAMTYPENAME
        {
            get
            {
                if (Lookup == null)
                    return null;
                else
                    return Lookup.LookUpName;
            }
            set { ;}
        }
        public string VALUATIONDATE
        {
            get
            {
                if (Entity.valn_mm_dt.Value.ToShortDateString() == null) return null;
                else return (Entity.valn_mm_dt.Value.ToShortDateString());

            }
            set { ;}
        }

    }
    //used in AdjustmentReviewSearch User Control
    public class ProgramPeriodSearchListBE : BusinessEntity<PREM_ADJ_PGM>
    {
        public ProgramPeriodSearchListBE()
            : base()
        {

        }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int PREM_ADJ_ID { get; set; }
        public DateTime? STRT_DT { get { return Convert.ToDateTime(Entity.strt_dt.Value.ToShortDateString()); } set { Entity.strt_dt = value; } }
        public DateTime? PLAN_END_DT { get { return Convert.ToDateTime(Entity.plan_end_dt.Value.ToShortDateString()); } set { Entity.plan_end_dt = value; } }
        public DateTime? VALN_MM_DT { get { return Convert.ToDateTime(Entity.valn_mm_dt.Value.ToShortDateString()); } set { Entity.valn_mm_dt = value; } }
        public string VALUATIONDATE { get; set; }
        public string STARTDATE_ENDDATE
        {
            get
            {
                if (Entity.strt_dt == null || Entity.plan_end_dt == null) return null;
                else return (Entity.strt_dt.Value.ToShortDateString() + " : " + Entity.plan_end_dt.Value.ToShortDateString());
            }
            set { ;}
        }
        public int? ADJ_STS_TYP_ID { get; set; }
        public string PGMTYP { get; set; }
        public string STARTDATE_ENDDATE_PGMTYP
        {
            get;
            set;

        }

    }
}
