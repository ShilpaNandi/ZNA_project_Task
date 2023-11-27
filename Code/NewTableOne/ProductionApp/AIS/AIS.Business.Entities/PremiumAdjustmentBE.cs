using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using log4net;

namespace ZurichNA.AIS.Business.Entities
{
    public class PremiumAdjustmentBE : BusinessEntity<PREM_ADJ>
    {
        public PremiumAdjustmentBE()
            : base()
        {
        }
        public int? REV_RESN_ID { get { return Entity.adj_rrsn_rsn_id; } set { Entity.adj_rrsn_rsn_id = value; } }
        public int PREMIUM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTOMERID { get { return Entity.reg_custmr_id; } set { Entity.reg_custmr_id = value; } }
        public int? REL_PREM_ADJ_ID { get { return Entity.rel_prem_adj_id; } set { Entity.rel_prem_adj_id = value; } }
        public DateTime? INVC_DUE_DT { get { return Entity.invc_due_dt; } set { Entity.invc_due_dt = value; } }
        public DateTime? FNL_INVC_DT { get { return Entity.fnl_invc_dt; } set { Entity.fnl_invc_dt = value; } }
        public DateTime? DRFT_INVC_DT { get { return Entity.drft_invc_dt; } set { Entity.drft_invc_dt = value; } }
        public DateTime VALN_DT { get { return Entity.valn_dt; } set { Entity.valn_dt = value; } }
        public string INVC_NBR_TXT { get { return Entity.drft_invc_nbr_txt; } set { Entity.drft_invc_nbr_txt = value; } }
        public string FNL_INVC_NBR_TXT { get { return Entity.fnl_invc_nbr_txt; } set { Entity.fnl_invc_nbr_txt = value; } }
        public Boolean? TWENTY_PCT_QLTY_CNTRL_IND { get { return Entity.twenty_pct_qlty_cntrl_ind; } set { Entity.twenty_pct_qlty_cntrl_ind = value; } }
        public Boolean? TWENTY_REQ_IND { get { return Entity.twenty_pct_qlty_cntrl_reqr_ind; } set { Entity.twenty_pct_qlty_cntrl_reqr_ind = value; } }
        public Boolean? ADJ_PENDG_IND { get { return Entity.adj_pendg_ind; } set { Entity.adj_pendg_ind = value; } }
        public String ADJ_PENDG_IND_DESC { get { if (ADJ_PENDG_IND == true) { return "YES"; } else { return "NO"; } } set { } }
        public int? UPDT_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDT_DT { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DT { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int? TWENTY_PCT_QLTY_CNTRL_PERS_ID { get { return Entity.twenty_pct_qlty_cntrl_pers_id; } set { Entity.twenty_pct_qlty_cntrl_pers_id = value; } }
        public int? BROKER_ID { get { return Entity.brkr_id; } set { Entity.brkr_id = value; } }
        public int? BU_OFF_ID { get { return Entity.bu_office_id; } set { Entity.bu_office_id = value; } }
        public DateTime? TWENTY_PCT_QLTY_CNTRL_DT { get { return Entity.twenty_pct_qlty_cntrl_dt; } set { Entity.twenty_pct_qlty_cntrl_dt = value; } }
        public int? PREM_ADJ_PGMID { get; set; }
        public int? ADJ_STS_TYP_ID { get { return Entity.adj_sts_typ_id; } set { Entity.adj_sts_typ_id = value; } }
        //        public int? PREM_ADJ_CMMNT_ID { get { return Entity.prem_adj_cmmnt_id; } set { Entity.prem_adj_cmmnt_id = value; } }
        public Boolean? ADJ_CAN_IND { get { return Entity.adj_can_ind; } set { Entity.adj_can_ind = value; } }
        public Boolean? ADJ_RRSN_IND { get { return Entity.adj_rrsn_ind; } set { Entity.adj_rrsn_ind = value; } }
        public Boolean? ADJ_VOID_IND { get { return Entity.adj_void_ind; } set { Entity.adj_void_ind = value; } }
        public Boolean? HISTORICAL_ADJ_IND { get { return Entity.historical_adj_ind; } set { Entity.historical_adj_ind = value; } }
//        public string PREM_NON_PREM_CD { get { return Entity.prem_non_prem_cd; } set { Entity.prem_non_prem_cd = value; } }
        public string CALC_ADJ_STS_CODE { get { return Entity.calc_adj_sts_cd; } set { Entity.calc_adj_sts_cd = value; } }
        //ZDW key's
        public string DRFT_INTRNL_PDF_ZDW_KEY_TXT { get { return Entity.drft_intrnl_pdf_zdw_key_txt; } set { Entity.drft_intrnl_pdf_zdw_key_txt = value; } }
        public string DRFT_EXTRNL_PDF_ZDW_KEY_TXT { get { return Entity.drft_extrnl_pdf_zdw_key_txt; } set { Entity.drft_extrnl_pdf_zdw_key_txt = value; } }
        public string DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT { get { return Entity.drft_cd_wrksht_pdf_zdw_key_txt; } set { Entity.drft_cd_wrksht_pdf_zdw_key_txt = value; } }
        public string FNL_INTRNL_PDF_ZDW_KEY_TXT { get { return Entity.fnl_intrnl_pdf_zdw_key_txt; } set { Entity.fnl_intrnl_pdf_zdw_key_txt = value; } }
        public string FNL_EXTRNL_PDF_ZDW_KEY_TXT { get { return Entity.fnl_extrnl_pdf_zdw_key_txt; } set { Entity.fnl_extrnl_pdf_zdw_key_txt = value; } }
        public string FNL_CD_WRKSHT_PDF_ZDW_KEY_TXT { get { return Entity.fnl_cd_wrksht_pdf_zdw_key_txt; } set { Entity.fnl_cd_wrksht_pdf_zdw_key_txt = value; } }
        public int? PREM_ADJ_PERD { get; set; }
        public string INVC_AMT { get; set; }
        public int? ACCOUNTID { get; set; }
        public string CUSTMR_NAME
        { get; set; }
        public string CHILD_CUSTMR_NAME
        { get; set; }
        public bool? CUSTMR_ACTIVE
        { get; set; }
        public string VALUATIONDATE { get; set; }
        public DateTime? PROGRAMPERIOD_STDT { get; set; }
        public DateTime? PROGRAMPERIOD_ENDT { get; set; }
        public string BROKERNAME { get; set; }
        public string BUNAME { get; set; }
        public string FINALINVNO { get; set; }
        public string DRAFTINVNO { get; set; }
        public string ADJ_STATUS
        { get; set; }
        public int? ADJ_STATUS_TYP_ID
        { get; set; }
        public int PERS_ID
        { get; set; }
        public int PREM_ADJ_PGM_ID
        { get; set; }
        public DateTime? QLTY_CNTRL_DT
        { get; set; }
        public DateTime? EFF_DT
        { get; set; }
        public Boolean? ARIES_CMPLT_IND
        { get; set; }
        // from table CUSTMR_PERS_REL
        public int? CUSTMR_PERS_REL_pers_id
        { get; set; }
        public DateTime? ADJ_STS_EFF_DT { get { return Entity.adj_sts_eff_dt; } set { Entity.adj_sts_eff_dt = value; } }
        public bool? ADJ_QC_IND { get { return Entity.adj_qc_ind; } set { Entity.adj_qc_ind = value; } }
        public bool? RECONCILER_REVW_IND { get { return Entity.reconciler_revw_ind; } set { Entity.reconciler_revw_ind = value; } }
        

    }
    public class PremiumAdjustmetSearchBE : BusinessEntity<PREM_ADJ>
    {
        public PremiumAdjustmetSearchBE()
            : base()
        {

        }
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public DateTime VALN_MM_DT { get { return Convert.ToDateTime(Entity.valn_dt.ToShortDateString()); } set { Entity.valn_dt = value; } }
        public string VALUATIONDATE { get; set; }
        public int? ADJ_STS_TYP_ID { get; set; }

    }
}
