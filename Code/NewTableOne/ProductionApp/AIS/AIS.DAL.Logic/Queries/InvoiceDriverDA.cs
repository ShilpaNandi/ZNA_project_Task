using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.Logic;


namespace ZurichNA.AIS.DAL.Logic
{
    public class InvoiceDriverDA
    {
        public bool UpdatePremAdjutmentDraftInvoiceData(AISDatabaseLINQDataContext objDC, int intAdjNo, string strInvoiceNo, int intUserID)
        {
            try
            {
                var PremAdj = (from cdd in objDC.PREM_ADJs where cdd.prem_adj_id == intAdjNo select cdd).First();
                PremAdj.drft_invc_nbr_txt = strInvoiceNo;
                PremAdj.adj_sts_typ_id = 348;
                PremAdj.adj_sts_eff_dt = DateTime.Now;
                PremAdj.drft_invc_dt = System.DateTime.Now;
                PremAdj.updt_dt = System.DateTime.Now;
                PremAdj.updt_user_id = intUserID;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            

        }
        public bool InsertPremAdjutmentStatusData(AISDatabaseLINQDataContext objDC, int intAdjNo, int intInvoiceStatus,string strComments, int intUserID)
        {
            try
            {
                var PremAdjStatus = (from cdd in objDC.PREM_ADJ_STs where cdd.prem_adj_id == intAdjNo orderby cdd.prem_adj_sts_id descending select cdd).First();
                PremAdjStatus.expi_dt = System.DateTime.Now;
                PremAdjStatus.updt_dt = System.DateTime.Now;
                PremAdjStatus.updt_user_id = intUserID;
                PREM_ADJ_ST premAdjStsNew = new PREM_ADJ_ST()
                {
                    prem_adj_id = intAdjNo,
                    custmr_id = PremAdjStatus.custmr_id,
                    eff_dt = System.DateTime.Now,
                    cmmnt_txt=strComments,
                    adj_sts_typ_id = intInvoiceStatus,
                    crte_dt = System.DateTime.Now,
                    crte_user_id = intUserID
                };
                objDC.PREM_ADJ_STs.InsertOnSubmit(premAdjStsNew);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateDraftZDWKeys(AISDatabaseLINQDataContext objDC, int intAdjNo, string strZDWKey,char cKeyType, int intUserID)
        {
            try
            {
                var PremAdj = (from cdd in objDC.PREM_ADJs where cdd.prem_adj_id == intAdjNo select cdd).First();
                switch (cKeyType)
                { 
                    case 'I':
                        PremAdj.drft_intrnl_pdf_zdw_key_txt = strZDWKey;
                        break;
                    case 'E':
                        PremAdj.drft_extrnl_pdf_zdw_key_txt = strZDWKey;
                        break;
                    case 'C':
                        PremAdj.drft_cd_wrksht_pdf_zdw_key_txt = strZDWKey;
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool UpdatePremAdjutmentFinalInvoiceData(AISDatabaseLINQDataContext objDC, int intAdjNo, string strInvoiceNo, int intUserID)
        {
            try
            {
                var PremAdj = (from cdd in objDC.PREM_ADJs where cdd.prem_adj_id == intAdjNo select cdd).First();
                PremAdj.fnl_invc_nbr_txt = strInvoiceNo;
                PremAdj.adj_sts_typ_id = 349;
                PremAdj.adj_sts_eff_dt = DateTime.Now;
                PremAdj.fnl_invc_dt = System.DateTime.Now;
                PremAdj.invc_due_dt = System.DateTime.Now.AddDays(20);
                PremAdj.updt_dt = System.DateTime.Now;
                PremAdj.updt_user_id = intUserID;

               //

                        var premAdjPerdBE = (from cdd in objDC.PREM_ADJ_PERDs where cdd.prem_adj_id == intAdjNo select cdd).ToList();
                        for (int i = 0; i < premAdjPerdBE.Count; i++)
                        {
                            var pgmprdBE = (from cdd in objDC.PREM_ADJ_PGMs where cdd.prem_adj_pgm_id == premAdjPerdBE[i].prem_adj_pgm_id select cdd).First();
                            // Non-Premium next valuation dates match the adjustment valuation date and does not match with Premium next valuation date
                            // If Adjustment's Val Date is equal to NextValnDt(NP) but not equal to nextvalnddt(P)
                            if ((PremAdj.valn_dt == pgmprdBE.nxt_valn_dt_non_prem_dt) && (PremAdj.valn_dt != pgmprdBE.nxt_valn_dt))
                            {
                                DateTime dtNxtValDateNonPrem = new DateTime();
                                int intFrequencyNonPrem = 0;
                                pgmprdBE.prev_valn_dt_non_prem_dt = pgmprdBE.nxt_valn_dt_non_prem_dt;
                                pgmprdBE.lsi_retrieve_from_dt = pgmprdBE.nxt_valn_dt_non_prem_dt;
                                if (pgmprdBE.nxt_valn_dt_non_prem_dt != null)
                                {
                                    dtNxtValDateNonPrem = Convert.ToDateTime(pgmprdBE.nxt_valn_dt_non_prem_dt);
                                    if (pgmprdBE.freq_non_prem_mms_cnt != null)
                                        intFrequencyNonPrem = Convert.ToInt32(pgmprdBE.freq_non_prem_mms_cnt);
                                    pgmprdBE.nxt_valn_dt_non_prem_dt = dtNxtValDateNonPrem.AddMonths(intFrequencyNonPrem);
                                    pgmprdBE.nxt_valn_dt_non_prem_dt= pgmprdBE.nxt_valn_dt_non_prem_dt.Value.AddDays(
                                        (DateTime.DaysInMonth(pgmprdBE.nxt_valn_dt_non_prem_dt.Value.Year,
                                        pgmprdBE.nxt_valn_dt_non_prem_dt.Value.Month)-pgmprdBE.nxt_valn_dt_non_prem_dt.Value.Day));
                                }
                                pgmprdBE.prior_prem_adj_id = pgmprdBE.prior_prem_adj_id;
                                pgmprdBE.updt_dt = System.DateTime.Now;
                                pgmprdBE.updt_user_id = intUserID;
                            }

                            // Premium next valuation dates match the adjustment valuation date and does not match with the Non-Premium next valuation date
                            // If Adjustment's Val Date is equal to NextValnDt(P) but not equal to nextvalnddt(NP)
                            if ((PremAdj.valn_dt != pgmprdBE.nxt_valn_dt_non_prem_dt) && (PremAdj.valn_dt == pgmprdBE.nxt_valn_dt))
                            {
                                pgmprdBE.prev_valn_dt = pgmprdBE.nxt_valn_dt;
                                pgmprdBE.lsi_retrieve_from_dt = pgmprdBE.nxt_valn_dt;
                                DateTime dtNxtValDate = new DateTime();
                                int intFrequency = 0;
                                if (pgmprdBE.nxt_valn_dt != null)
                                {
                                    dtNxtValDate = Convert.ToDateTime(pgmprdBE.nxt_valn_dt);
                                    if (pgmprdBE.adj_freq_mms_intvrl_cnt != null)
                                        intFrequency = Convert.ToInt32(pgmprdBE.adj_freq_mms_intvrl_cnt);
                                    pgmprdBE.nxt_valn_dt = dtNxtValDate.AddMonths(intFrequency);
                                    pgmprdBE.nxt_valn_dt = pgmprdBE.nxt_valn_dt.Value.AddDays(
                                       (DateTime.DaysInMonth(pgmprdBE.nxt_valn_dt.Value.Year,
                                       pgmprdBE.nxt_valn_dt.Value.Month) - pgmprdBE.nxt_valn_dt.Value.Day));
                                }

                                pgmprdBE.prior_prem_adj_id = intAdjNo;
                                pgmprdBE.updt_dt = System.DateTime.Now;
                                pgmprdBE.updt_user_id = intUserID;

                                var comauditlist = (from cdd in objDC.COML_AGMT_AUDTs where cdd.prem_adj_pgm_id == pgmprdBE.prem_adj_pgm_id && cdd.audt_revd_sts_ind != true && cdd.adj_ind!=true select cdd).ToList();
                                for (int j = 0; j < comauditlist.Count; j++)
                                {
                                    var comaudit = (from cdd in objDC.COML_AGMT_AUDTs where cdd.coml_agmt_audt_id == comauditlist[j].coml_agmt_audt_id select cdd).Single();
                                    comaudit.adj_ind = true;
                                    comaudit.prem_adj_id = intAdjNo;

                                }
                                
                            }
                            // Both Premium and Non-Premium next valuation dates match with the adjustment valuation date
                            // If Adjustment's Val Date is equal to NextValnDt(P) and equal to nextvalnddt(NP)
                            if ((PremAdj.valn_dt == pgmprdBE.nxt_valn_dt_non_prem_dt) && (PremAdj.valn_dt == pgmprdBE.nxt_valn_dt))
                            {
                                pgmprdBE.prev_valn_dt = pgmprdBE.nxt_valn_dt;
                                pgmprdBE.lsi_retrieve_from_dt = pgmprdBE.nxt_valn_dt;
                                DateTime dtNxtValDate = new DateTime();
                                int intFrequency = 0;
                                DateTime dtNxtValDateNonPrem = new DateTime();
                                int intFrequencyNonPrem = 0;
                                if (pgmprdBE.nxt_valn_dt != null)
                                {
                                    dtNxtValDate = Convert.ToDateTime(pgmprdBE.nxt_valn_dt);
                                    if (pgmprdBE.adj_freq_mms_intvrl_cnt != null)
                                        intFrequency = Convert.ToInt32(pgmprdBE.adj_freq_mms_intvrl_cnt);
                                    pgmprdBE.nxt_valn_dt = dtNxtValDate.AddMonths(intFrequency);
                                    pgmprdBE.nxt_valn_dt = pgmprdBE.nxt_valn_dt.Value.AddDays(
                                       (DateTime.DaysInMonth(pgmprdBE.nxt_valn_dt.Value.Year,
                                       pgmprdBE.nxt_valn_dt.Value.Month) - pgmprdBE.nxt_valn_dt.Value.Day));
                                }
                                pgmprdBE.prev_valn_dt_non_prem_dt = pgmprdBE.nxt_valn_dt_non_prem_dt;
                                if (pgmprdBE.nxt_valn_dt_non_prem_dt != null)
                                {
                                    dtNxtValDateNonPrem = Convert.ToDateTime(pgmprdBE.nxt_valn_dt_non_prem_dt);
                                    if (pgmprdBE.freq_non_prem_mms_cnt != null)
                                        intFrequencyNonPrem = Convert.ToInt32(pgmprdBE.freq_non_prem_mms_cnt);
                                    pgmprdBE.nxt_valn_dt_non_prem_dt = dtNxtValDateNonPrem.AddMonths(intFrequencyNonPrem);
                                    pgmprdBE.nxt_valn_dt_non_prem_dt = pgmprdBE.nxt_valn_dt_non_prem_dt.Value.AddDays(
                                      (DateTime.DaysInMonth(pgmprdBE.nxt_valn_dt_non_prem_dt.Value.Year,
                                      pgmprdBE.nxt_valn_dt_non_prem_dt.Value.Month) - pgmprdBE.nxt_valn_dt_non_prem_dt.Value.Day));
                                }
                                pgmprdBE.prior_prem_adj_id = intAdjNo;
                                pgmprdBE.updt_user_id = intUserID;
                                pgmprdBE.updt_dt = System.DateTime.Now;


                                var comauditlist = (from cdd in objDC.COML_AGMT_AUDTs where cdd.prem_adj_pgm_id == pgmprdBE.prem_adj_pgm_id && cdd.audt_revd_sts_ind != true && cdd.adj_ind != true select cdd).ToList();
                                for (int j = 0; j < comauditlist.Count; j++)
                                {
                                    var comaudit = (from cdd in objDC.COML_AGMT_AUDTs where cdd.coml_agmt_audt_id == comauditlist[j].coml_agmt_audt_id select cdd).Single();
                                    comaudit.adj_ind = true;
                                    comaudit.prem_adj_id = intAdjNo;

                                }
                               
                            }


                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateFinalZDWKeys(AISDatabaseLINQDataContext objDC, int intAdjNo, string strZDWKey,char cKeyType, int intUserID)
        {
            try
            {
                var PremAdj = (from cdd in objDC.PREM_ADJs where cdd.prem_adj_id == intAdjNo select cdd).First();
                switch (cKeyType)
                {
                    case 'I':
                        PremAdj.drft_intrnl_pdf_zdw_key_txt = strZDWKey;
                        PremAdj.fnl_intrnl_pdf_zdw_key_txt = strZDWKey;
                        break;
                    case 'E':
                        PremAdj.drft_extrnl_pdf_zdw_key_txt = strZDWKey;
                        PremAdj.fnl_extrnl_pdf_zdw_key_txt = strZDWKey;
                        break;
                    case 'C':
                        PremAdj.drft_cd_wrksht_pdf_zdw_key_txt = strZDWKey;
                        PremAdj.fnl_cd_wrksht_pdf_zdw_key_txt = strZDWKey;
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        //Used in Invoice Driver
        public string getZDWKey(AISDatabaseLINQDataContext objDC,int intAdjNo, char cKeyType, int IFlag)
        {
            string strZDWKey = string.Empty;

            
            var querry = (from cdd in objDC.PREM_ADJs
                          where cdd.prem_adj_id == intAdjNo
                          select cdd).First();
            if (IFlag == 1)
            {
                switch (cKeyType)
                {
                    case 'I':
                        strZDWKey = querry.drft_intrnl_pdf_zdw_key_txt;
                        break;
                    case 'E':
                        strZDWKey = querry.drft_extrnl_pdf_zdw_key_txt;
                        break;
                    case 'C':
                        strZDWKey = querry.drft_cd_wrksht_pdf_zdw_key_txt;
                        break;

                }

            }
            if (IFlag == 2)
            {
                switch (cKeyType)
                {
                    case 'I':
                        strZDWKey = querry.drft_intrnl_pdf_zdw_key_txt;
                        break;
                    case 'E':
                        strZDWKey = querry.drft_extrnl_pdf_zdw_key_txt;
                        break;
                    case 'C':
                        strZDWKey = querry.drft_cd_wrksht_pdf_zdw_key_txt;
                        break;

                }

            }

            return strZDWKey;
        }
    }
}
