using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{
    public class Qtly_Cntrl_ChklistDA : DataAccessor<QLTY_CNTRL_LIST, Qtly_Cntrl_ChklistBE, AISDatabaseLINQDataContext>
    {
        public IList<Qtly_Cntrl_ChklistBE> getQtlychklistList()
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             //join cust in this.Context.CUSTMRs
             //on qlty.revr_pers_id equals cust.custmr_id
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = qlty.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = qlty.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = qlty.chk_list_itm_id,
                 //QUALITYCONTROLTYPE_ID = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qlty.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qlty.prem_adj_aries_clring_id,
                 CUSTOMER_ID = qlty.custmr_id,
                 ACTIVE = qlty.actv_ind,
                 PREMIUMADJUSTMENT_ID = qlty.prem_adj_id,

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        public IList<Qtly_Cntrl_ChklistBE> getAccQtlychklistList(int QualityCntlTypID, int PremAdjPgmID, int customerID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             join iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs on qlty.chk_list_itm_id equals iss.qlty_cntrl_mstr_issu_list_id
             where iss.issu_catg_id == QualityCntlTypID
             && qlty.prem_adj_pgm_id == PremAdjPgmID
             && qlty.custmr_id == customerID

             //join cust in this.Context.CUSTMRs
             //on qlty.revr_pers_id equals cust.custmr_id
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = qlty.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = qlty.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = qlty.chk_list_itm_id,
                 CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 QUALITYCONTROLTYPE_ID = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qlty.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qlty.prem_adj_aries_clring_id,
                 CUSTOMER_ID = qlty.custmr_id,
                 PREMIUMADJUSTMENT_ID = qlty.prem_adj_id,
                 ACTIVE = qlty.actv_ind,
                 ENABLED=iss.actv_ind,

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        public IList<Qtly_Cntrl_ChklistBE> getQtlychklistList(int QualityCntlTypID, int PremAdjStsID, int customerID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             join iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs on qlty.chk_list_itm_id equals iss.qlty_cntrl_mstr_issu_list_id
             where iss.issu_catg_id == QualityCntlTypID
             && qlty.prem_adj_sts_id == PremAdjStsID
             && qlty.custmr_id == customerID
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = qlty.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = qlty.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = qlty.chk_list_itm_id,
                 CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 QUALITYCONTROLTYPE_ID = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qlty.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qlty.prem_adj_aries_clring_id,
                 CUSTOMER_ID = qlty.custmr_id,
                 PREMIUMADJUSTMENT_ID = qlty.prem_adj_id,
                 ACTIVE = qlty.actv_ind,
                 ENABLED = iss.actv_ind,
                 ProgramPeriodStDate=qlty.PREM_ADJ_PGM.strt_dt,
                 ProgramPeriodEndDate = qlty.PREM_ADJ_PGM.plan_end_dt,
                 //AccountName=qlty.C
                 AccountName = (from cust in Context.CUSTMRs
                                where cust.custmr_id == qlty.custmr_id
                                select cust.full_nm).First().ToString(),
                 Reg_AccountName = (from cust in Context.CUSTMRs
                                where cust.custmr_id == qlty.custmr_rel_id
                                select cust.full_nm).First().ToString(),
                
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        public IList<Qtly_Cntrl_ChklistBE> getAriesdetailslist(int QualityCntlTypID, int PremAdjClrgID, int customerID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             join iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs on qlty.chk_list_itm_id equals iss.qlty_cntrl_mstr_issu_list_id
             where iss.issu_catg_id == QualityCntlTypID
             && qlty.prem_adj_aries_clring_id == PremAdjClrgID
             && qlty.custmr_id == customerID
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = qlty.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = qlty.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = qlty.chk_list_itm_id,
                 CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 QUALITYCONTROLTYPE_ID = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qlty.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qlty.prem_adj_aries_clring_id,
                 CUSTOMER_ID = qlty.custmr_id,
                 PREMIUMADJUSTMENT_ID = qlty.prem_adj_id,
                 ACTIVE = qlty.actv_ind,

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        public bool IsExistsIssue(int? premAdjStsID, int ChkListItemID, int? CustomerID, int QltyID)
        {
            bool Flag = false;
            var Qlty = from cdd in this.Context.QLTY_CNTRL_LISTs
                       where (cdd.prem_adj_sts_id == premAdjStsID && cdd.chk_list_itm_id == ChkListItemID && cdd.custmr_id == CustomerID && cdd.qlty_cntrl_list_id != QltyID)
                       select new { cdd.chk_list_itm_id };
            if (Qlty.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }
        public bool IsExistsIssueQCDetails(int? premAdjStsID, int? ProgramPerioDID, int ChkListItemID, int? CustomerID, int QltyID)
        {
            bool Flag = false;
            var Qlty = from cdd in this.Context.QLTY_CNTRL_LISTs
                       where (cdd.prem_adj_sts_id == premAdjStsID && cdd.prem_adj_pgm_id == ProgramPerioDID &&  cdd.chk_list_itm_id == ChkListItemID && cdd.custmr_id == CustomerID && cdd.qlty_cntrl_list_id != QltyID)
                       select new { cdd.chk_list_itm_id };
            if (Qlty.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }
        public bool IsExistsAcctQCIssue(int? premAdjPgmID, int ChkListItemID, int? CustomerID, int QltyID)
        {
            bool Flag = false;
            var Qlty = from cdd in this.Context.QLTY_CNTRL_LISTs
                       where (cdd.prem_adj_pgm_id == premAdjPgmID && cdd.chk_list_itm_id == ChkListItemID && cdd.custmr_id == CustomerID && cdd.qlty_cntrl_list_id != QltyID)
                       select new { cdd.chk_list_itm_id };
            if (Qlty.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }
        public bool IsExistsAriesQCIssue(int? ariesclrngid, int ChkListItemID, int? CustomerID, int QltyID)
        {
            bool Flag = false;
            var Qlty = from cdd in this.Context.QLTY_CNTRL_LISTs
                       where (cdd.prem_adj_aries_clring_id == ariesclrngid && cdd.chk_list_itm_id == ChkListItemID && cdd.custmr_id == CustomerID && cdd.qlty_cntrl_list_id != QltyID)
                       select new { cdd.chk_list_itm_id };
            if (Qlty.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }

        public IList<Qtly_Cntrl_ChklistBE> getQCDetailsLst(int QualityCntlTypID, int PremiumAdjustmentProgramID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             where qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_catg_id == QualityCntlTypID
             && qlty.prem_adj_pgm_id == PremiumAdjustmentProgramID
             //join cust in this.Context.CUSTMRs
             //on qlty.revr_pers_id equals cust.custmr_id
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = qlty.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = qlty.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = qlty.chk_list_itm_id,
                 QUALITYCONTROLTYPE_ID = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qlty.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qlty.prem_adj_aries_clring_id,
                 CUSTOMER_ID = qlty.custmr_id,
                 PREMIUMADJUSTMENT_ID = qlty.prem_adj_id,

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }


    }
}
