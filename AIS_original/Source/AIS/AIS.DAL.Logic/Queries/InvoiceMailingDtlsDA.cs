using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class InvoiceMailingDtlsDA : DataAccessor<PREM_ADJ, InvoiceMailingDtlsBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Returns all Inv. Mailing Details that match criteria based on Final Inv. No.
        /// </summary>
        /// <returns></returns>
        public IList<InvoiceMailingDtlsBE> getInvMailingFinalData(int custmrID, string fnlinvnbr)
        {
            IList<InvoiceMailingDtlsBE> result = new List<InvoiceMailingDtlsBE>();
            int recRevID = (from lk in this.Context.LKUPs 
                            where lk.lkup_txt=="RECON REVIEW" select lk.lkup_id).First();
            int uwRevID = (from lk in this.Context.LKUPs
                           where lk.lkup_txt == "UW REVIEW"
                           select lk.lkup_id).First();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Inv. Mailing Details
            /// and project it into Inv. Mailing Details Business Entity
            IQueryable<InvoiceMailingDtlsBE> query =
            (from prmadj in this.Context.PREM_ADJs
             join prmadjStatus in this.Context.PREM_ADJ_STs
              on prmadj.prem_adj_id equals prmadjStatus.prem_adj_id
             where prmadj.reg_custmr_id == custmrID && prmadj.fnl_invc_nbr_txt == fnlinvnbr
             //&& prmadj.valn_dt != null && prmadj.invc_due_dt != null
             orderby prmadj.valn_dt descending
             select new InvoiceMailingDtlsBE()
             {
                 INVC_AMT = this.Context.fn_GetTotalforAdjInv(prmadj.prem_adj_id).Value.ToString(),
                 PREM_ADJ_ID = prmadj.prem_adj_id,
                 VALUATION_DATE = prmadj.valn_dt,
                 CUSTMER_ID = prmadj.reg_custmr_id,
                 CUSTMRNM = prmadj.CUSTMR.full_nm,
                 BROKER = prmadj.EXTRNL_ORG.full_name,
                 BUOFC = prmadj.INT_ORG.full_name,
                 FINAL_INV_TXT = prmadj.fnl_invc_nbr_txt,
                 INV_DUE_DT = prmadj.invc_due_dt,
                 CREATEDATE = prmadj.crte_dt,
                 FINAL_EMAIL_DT = prmadj.fnl_mailed_brkr_dt,
                 DRAFT_INV_DATE = prmadj.drft_invc_dt,
                 FINAL_INV_DT = prmadj.fnl_invc_dt,
                 DRAFT_MAILED_UW_DT = prmadj.drft_mailed_undrwrt_dt,
                 FINAL_MAILED_UW_DT = prmadj.fnl_mailed_undrwrt_dt,
                 ADJ_STS_TYP_ID=prmadjStatus.adj_sts_typ_id,
                 //Below 3 fileds are added to meet PDF linkbutton requirements
                 DRFT_INTRNL_PDF_ZDW_KEY_TXT = prmadj.drft_intrnl_pdf_zdw_key_txt,
                 DRFT_EXTRNL_PDF_ZDW_KEY_TXT = prmadj.drft_extrnl_pdf_zdw_key_txt,
                 DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT = prmadj.drft_cd_wrksht_pdf_zdw_key_txt
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            result = result.Where(prmadjsts => (prmadjsts.ADJ_STS_TYP_ID == recRevID || prmadjsts.ADJ_STS_TYP_ID == uwRevID)).ToList();
            return result;
        }

        /// <summary>
        /// Returns all Inv. Mailing Details that match criteria based on Draft Inv. No.
        /// </summary>
        /// <returns></returns>
        public IList<InvoiceMailingDtlsBE> getInvMailingDraftData(int custmrID, string dftinvnbr)
        {
            IList<InvoiceMailingDtlsBE> result = new List<InvoiceMailingDtlsBE>();
            int recRevID = (from lk in this.Context.LKUPs
                            where lk.lkup_txt == "RECON REVIEW"
                            select lk.lkup_id).First();
            int uwRevID = (from lk in this.Context.LKUPs
                           where lk.lkup_txt == "UW REVIEW"
                           select lk.lkup_id).First();
            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Inv. Mailing Details
            /// and project it into Inv. Mailing Details Business Entity
            IQueryable<InvoiceMailingDtlsBE> query =
            (from prmadj in this.Context.PREM_ADJs
             join prmadjStatus in this.Context.PREM_ADJ_STs
              on prmadj.prem_adj_id equals prmadjStatus.prem_adj_id
             where prmadj.reg_custmr_id == custmrID && prmadj.drft_invc_nbr_txt == dftinvnbr
             //&& prmadj.valn_dt != null && prmadj.invc_due_dt != null
             orderby prmadj.valn_dt descending
             select new InvoiceMailingDtlsBE()
             {
                 INVC_AMT = this.Context.fn_GetTotalforAdjInv(prmadj.prem_adj_id).Value.ToString(),
                 PREM_ADJ_ID = prmadj.prem_adj_id,
                 VALUATION_DATE = prmadj.valn_dt,
                 CUSTMER_ID = prmadj.reg_custmr_id,
                 CUSTMRNM = prmadj.CUSTMR.full_nm,
                 BROKER = prmadj.EXTRNL_ORG.full_name,
                 BUOFC = prmadj.INT_ORG.full_name,
                 DRAFT_INV_TXT = prmadj.drft_invc_nbr_txt,
                 INV_DUE_DT = prmadj.invc_due_dt,
                 CREATEDATE = prmadj.crte_dt,
                 FINAL_EMAIL_DT = prmadj.fnl_mailed_brkr_dt,
                 DRAFT_INV_DATE = prmadj.drft_invc_dt,
                 FINAL_INV_DT = prmadj.fnl_invc_dt,
                 DRAFT_MAILED_UW_DT = prmadj.drft_mailed_undrwrt_dt,
                 FINAL_MAILED_UW_DT = prmadj.fnl_mailed_undrwrt_dt,
                 ADJ_STS_TYP_ID = prmadjStatus.adj_sts_typ_id,
                 //Below 3 fileds are added to meet PDF linkbutton requirements
                 DRFT_INTRNL_PDF_ZDW_KEY_TXT=prmadj.drft_intrnl_pdf_zdw_key_txt,
                 DRFT_EXTRNL_PDF_ZDW_KEY_TXT=prmadj.drft_extrnl_pdf_zdw_key_txt,
                 DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT=prmadj.drft_cd_wrksht_pdf_zdw_key_txt
                 
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            result = result.Where(prmadjsts => (prmadjsts.ADJ_STS_TYP_ID == recRevID || prmadjsts.ADJ_STS_TYP_ID == uwRevID)).ToList();
            return result;
        }
      /// <summary>
        /// Returns all Inv. Mailing Details that match criteria 
        /// </summary>
        /// <returns></returns>
        public InvoiceMailingDtlsBE getInvMailingDataRow(int ID)
        {
            int calcID = (from lk in this.Context.LKUPs
                            where lk.lkup_txt == "CALC"
                            select lk.lkup_id).First();
            int uwRevID = (from lk in this.Context.LKUPs
                           where lk.lkup_txt == "UW REVIEW"
                           select lk.lkup_id).First();

            InvoiceMailingDtlsBE result = new InvoiceMailingDtlsBE();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Inv. Mailing Details
            /// and project it into Inv. Mailing Details Business Entity
            IQueryable<InvoiceMailingDtlsBE> query =
            (from prmadj in this.Context.PREM_ADJs
             join prmadjStatus in (this.Context.PREM_ADJ_STs.OrderByDescending(prm=>prm.eff_dt)
             .Where(prm=>prm.prem_adj_id==ID).Take(1))
             on prmadj.prem_adj_id equals prmadjStatus.prem_adj_id
             where prmadj.prem_adj_id==ID 
             select new InvoiceMailingDtlsBE()
             {
                 UW_NOT_REQ=prmadj.undrwrt_not_reqr_ind,
                 HistoricalInd=prmadj.historical_adj_ind,
                 INVC_AMT = this.Context.fn_GetTotalforAdjInv(prmadj.prem_adj_id).Value.ToString(),
                PREM_ADJ_ID=prmadj.prem_adj_id,
                VALUATION_DATE = prmadj.valn_dt,
                CUSTMER_ID = prmadj.reg_custmr_id,
                CUSTMRNM=prmadj.CUSTMR.full_nm,
                BROKER = prmadj.EXTRNL_ORG.full_name,
                BUOFC = prmadj.INT_ORG.full_name,
                FINAL_INV_TXT=prmadj.fnl_invc_nbr_txt,
                INV_DUE_DT=prmadj.invc_due_dt,
                CREATEDATE=prmadj.crte_dt,
                FINAL_EMAIL_DT=prmadj.fnl_mailed_brkr_dt,
                DRAFT_INV_DATE=prmadj.drft_invc_dt,
                FINAL_INV_DT=prmadj.fnl_invc_dt,
                DRAFT_MAILED_UW_DT=prmadj.drft_mailed_undrwrt_dt,
                FINAL_MAILED_UW_DT=prmadj.fnl_mailed_undrwrt_dt,
                CMMNT_TXT=prmadjStatus.cmmnt_txt,
                ADJ_STS_ID=prmadjStatus.prem_adj_sts_id,
                UW_RESP=(from prmadjSts in this.Context.PREM_ADJ_STs
                         join lk in this.Context.LKUPs
                         on prmadjSts.adj_sts_typ_id equals lk.lkup_id
                         where prmadjSts.prem_adj_sts_id == prmadjStatus.prem_adj_sts_id
                         select lk.lkup_txt).First().ToString(),
                UW_RESP_DT=(from premadjStatus in this.Context.PREM_ADJ_STs
                            where premadjStatus.prem_adj_id == ID && (premadjStatus.adj_sts_typ_id == uwRevID || premadjStatus.adj_sts_typ_id == calcID)
                            orderby premadjStatus.prem_adj_sts_id descending
                            select premadjStatus.qlty_cntrl_dt).First(),
                CALC_ID=calcID,
                UW_REV_ID=uwRevID
               

             }).Distinct();

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList()[0];
            return result;
        }



        /// <summary>
        /// Returns all Inv. Mailing Details that match criteria -written By Venkat to Fix the bug 7804
        /// </summary>
        /// <returns></returns>
        public IList<InvoiceMailingDtlsBE> getInvMailingData(int custmrID, string invnbr, string valndtfrm, string valndtto, string invduedtfrm, string invduedtto)
        {
            IList<InvoiceMailingDtlsBE> result = new List<InvoiceMailingDtlsBE>();
            int recRevID = (from lk in this.Context.LKUPs
                            where lk.lkup_txt == "RECON REVIEW"
                            select lk.lkup_id).First();
            int uwRevID = (from lk in this.Context.LKUPs
                           where lk.lkup_txt == "UW REVIEW"
                           select lk.lkup_id).First();
            int FinalID = (from lk in this.Context.LKUPs
                           where lk.lkup_txt == "FINAL INVOICE"
                           select lk.lkup_id).First();
            int TransmittedID = (from lk in this.Context.LKUPs
                                 where lk.lkup_txt == "TRANSMITTED"
                           select lk.lkup_id).First();
            int CancelID = (from lk in this.Context.LKUPs
                            where lk.lkup_txt == "CANCELLED"
                                 select lk.lkup_id).First();
            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Inv. Mailing Details
            /// and project it into Inv. Mailing Details Business Entity
            IQueryable<InvoiceMailingDtlsBE> query =
            (from prmadj in this.Context.PREM_ADJs
             //where prmadj.reg_custmr_id == custmrID //It is commented, because search can be any field in the screen.
             orderby prmadj.valn_dt descending
             select new InvoiceMailingDtlsBE()
             {
                 INVC_AMT = this.Context.fn_GetTotalforAdjInv(prmadj.prem_adj_id).Value.ToString(),
                 
                 PREM_ADJ_ID = prmadj.prem_adj_id,
                 VALUATION_DATE = prmadj.valn_dt,
                 CUSTMER_ID = prmadj.reg_custmr_id,
                 CUSTMRNM = prmadj.CUSTMR.full_nm,
                 BROKER = prmadj.EXTRNL_ORG.full_name,
                 BUOFC = prmadj.INT_ORG.full_name,
                 DRAFT_INV_TXT = prmadj.drft_invc_nbr_txt,
                 FINAL_INV_TXT = prmadj.fnl_invc_nbr_txt,
                 INV_DUE_DT = prmadj.invc_due_dt,
                 CREATEDATE = prmadj.crte_dt,
                 UPDATEDATE=prmadj.updt_dt,
                 FINAL_EMAIL_DT = prmadj.fnl_mailed_brkr_dt,
                 DRAFT_INV_DATE = prmadj.drft_invc_dt,
                 FINAL_INV_DT = prmadj.fnl_invc_dt,
                 DRAFT_MAILED_UW_DT = prmadj.drft_mailed_undrwrt_dt,
                 FINAL_MAILED_UW_DT = prmadj.fnl_mailed_undrwrt_dt,
                 HistoricalInd=prmadj.historical_adj_ind,
                 //Below 3 fileds are added to meet PDF linkbutton requirements
                 DRFT_INTRNL_PDF_ZDW_KEY_TXT=prmadj.drft_intrnl_pdf_zdw_key_txt,
                 DRFT_EXTRNL_PDF_ZDW_KEY_TXT=prmadj.drft_extrnl_pdf_zdw_key_txt,
                 DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT=prmadj.drft_cd_wrksht_pdf_zdw_key_txt,
                 DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT="",// phase -3
                 Historical=prmadj.historical_adj_ind,
                 ADJ_STS_TYP_ID = Convert.ToInt32((from pap in this.Context.PREM_ADJ_STs
                                                   orderby pap.prem_adj_sts_id descending
                                                   where pap.prem_adj_id == prmadj.prem_adj_id
                                                   select pap.adj_sts_typ_id).First().ToString())
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            result = result.Where(prmadjsts => (prmadjsts.ADJ_STS_TYP_ID == recRevID || prmadjsts.ADJ_STS_TYP_ID == uwRevID || prmadjsts.ADJ_STS_TYP_ID == FinalID || (prmadjsts.ADJ_STS_TYP_ID != FinalID && prmadjsts.ADJ_STS_TYP_ID != TransmittedID && prmadjsts.ADJ_STS_TYP_ID != CancelID && prmadjsts.Historical == true))).ToList();
            if (custmrID != 0)
            {
                result = result.Where(prmadj => prmadj.CUSTMER_ID == custmrID).ToList();
            }
            if (invnbr.ToString() != "" && invnbr.ToString() != "")
            {
                result = result.Where(prmadj => prmadj.DRAFT_INV_TXT == invnbr || prmadj.FINAL_INV_TXT == invnbr).ToList();
            }
            if (valndtfrm.ToString() != "" && valndtto.ToString() != "")
            {
                result = result.Where(prmadj => prmadj.VALUATION_DATE >= DateTime.Parse(valndtfrm) && prmadj.VALUATION_DATE <= DateTime.Parse(valndtto)).ToList();
            }
            if (invduedtfrm.ToString() != "" && invduedtto.ToString() != "")
            {
                result = result.Where(prmadj => prmadj.INV_DUE_DT >= DateTime.Parse(invduedtfrm) && prmadj.INV_DUE_DT <= DateTime.Parse(invduedtto)).ToList();
            }
            return result;
        }
    }
}
