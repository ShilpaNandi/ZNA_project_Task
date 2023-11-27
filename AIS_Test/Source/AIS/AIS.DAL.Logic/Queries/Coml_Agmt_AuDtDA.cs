using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class Coml_Agmt_AuDtDA : DataAccessor<COML_AGMT_AUDT, Coml_Agmt_AuDtBE, AISDatabaseLINQDataContext>
    {
        public IList<Coml_Agmt_AuDtBE> getCommAgrAuditList(int intProgPeriodID)
        {
            IList<Coml_Agmt_AuDtBE> result = new List<Coml_Agmt_AuDtBE>();
            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Coml_Agmt_AuDtBE> query =
            (from que in this.Context.COML_AGMT_AUDTs
             where que.prem_adj_pgm_id == intProgPeriodID
             orderby que.COML_AGMT.pol_sym_txt ascending ,que.COML_AGMT.pol_nbr_txt ascending, que.strt_dt descending
             select new Coml_Agmt_AuDtBE()
             {
                 Comm_Agr_Audit_ID = que.coml_agmt_audt_id,
                 Comm_Agr_ID = que.coml_agmt_id,
                 Prem_Adj_Prg_ID = que.prem_adj_pgm_id,
                 Customer_ID = que.custmr_id,
                 StartDate = que.strt_dt,
                 Sub_Aud_Prm_Amt = que.subj_audt_prem_amt,
                 Non_Sub_Aud_Prm_Amt = que.non_subj_audt_prem_amt,
                 Def_Dep_prm_Amt = que.defr_depst_prem_amt,
                 Sub_Dep_Prm_Amt = que.subj_depst_prem_amt,
                 Non_Sub_Dep_Prm_Amt = que.non_subj_depst_prem_amt,
                 Audit_Reslt_Amt = que.audt_rslt_amt,
                 ExposureAmt = que.expo_amt,
                 Aud_Rev_Status = que.audt_revd_sts_ind,
                 POLICY = que.COML_AGMT.pol_sym_txt.Trim() + " " + que.COML_AGMT.pol_nbr_txt.Trim() + " " + que.COML_AGMT.pol_modulus_txt.Trim(),
                 AdjustmentIndicator=que.adj_ind,
                 UpdatedDate=que.updt_dt,
                 Pol_Ind=que.COML_AGMT.actv_ind,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }



        public IList<Coml_Agmt_AuDtBE> getCommAgrAuditList(int AccountNumber, int intProgPeriodID)
        {
            IList<Coml_Agmt_AuDtBE> result = new List<Coml_Agmt_AuDtBE>();
            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Coml_Agmt_AuDtBE> query =
            (from que in this.Context.COML_AGMT_AUDTs
             where que.prem_adj_pgm_id == intProgPeriodID && 
             que.custmr_id == AccountNumber
             orderby que.COML_AGMT.pol_sym_txt ascending,que.COML_AGMT.pol_nbr_txt ascending, que.strt_dt descending
             select new Coml_Agmt_AuDtBE()
             {
                 Comm_Agr_Audit_ID = que.coml_agmt_audt_id,
                 Comm_Agr_ID = que.coml_agmt_id,
                 Prem_Adj_Prg_ID = que.prem_adj_pgm_id,
                 Customer_ID = que.custmr_id,
                 StartDate = que.strt_dt,
                 Sub_Aud_Prm_Amt = que.subj_audt_prem_amt,
                 Non_Sub_Aud_Prm_Amt = que.non_subj_audt_prem_amt,
                 Def_Dep_prm_Amt = que.defr_depst_prem_amt,
                 Sub_Dep_Prm_Amt = que.subj_depst_prem_amt,
                 Non_Sub_Dep_Prm_Amt = que.non_subj_depst_prem_amt,
                 Audit_Reslt_Amt = que.audt_rslt_amt,
                 ExposureAmt = que.expo_amt,
                 Aud_Rev_Status = que.audt_revd_sts_ind,
                 POLICY = que.COML_AGMT.pol_sym_txt.Trim() + " " + que.COML_AGMT.pol_nbr_txt.Trim() + " " + que.COML_AGMT.pol_modulus_txt.Trim(),
                 AdjustmentIndicator = que.adj_ind,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

    }
}
