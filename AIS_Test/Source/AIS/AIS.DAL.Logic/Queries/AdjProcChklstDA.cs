using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
namespace ZurichNA.AIS.DAL.Logic
{
    public class AdjProcChklstDA : DataAccessor<QLTY_CNTRL_LIST, Qtly_Cntrl_ChklistBE, AISDatabaseLINQDataContext>
    {
        public IList<Qtly_Cntrl_ChklistBE> getAdjProcItemInfo()
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qc in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             join lk in this.Context.LKUPs
             on qc.issu_catg_id equals lk.lkup_id
             join lktype in this.Context.LKUP_TYPs
             on lk.lkup_typ_id equals lktype.lkup_typ_id
             where lktype.lkup_typ_nm_txt == "MASTER ISSUE CHECKLIST TYPE" 
             && lk.lkup_txt == "Adjustment Processing Checklist"
             && qc.actv_ind == true
             orderby qc.srt_nbr
             select new Qtly_Cntrl_ChklistBE()
             {
                 
                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 AdjChkLstItems = qc.issu_txt,
                 LOOKUPID = qc.qlty_cntrl_mstr_issu_list_id,
                 //ACTIVE=qc.actv_ind
                 ACTIVE=false,
                 UpdatedDate=qc.updt_dt

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;

            
        }
        public IList<Qtly_Cntrl_ChklistBE> getRelatedChklstItems_old(int ChklstID, int prmAdjID, int custID, int prgPrdID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             join iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs on qlty.chk_list_itm_id equals iss.qlty_cntrl_mstr_issu_list_id
             where iss.issu_catg_id == ChklstID
             where qlty.prem_adj_id == prmAdjID
             && qlty.custmr_id == custID
             && qlty.prem_adj_pgm_id == prgPrdID  
             && iss.actv_ind == true
             orderby iss.srt_nbr
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
                 ACTIVE = qlty.actv_ind,
                 CHKLIST_STS_CD=qlty.chklist_sts_cd,
                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 AdjChkLstItems = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 LOOKUPID = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.qlty_cntrl_mstr_issu_list_id,
                 UpdatedDate=qlty.updt_dt
               

             });
            IQueryable<Qtly_Cntrl_ChklistBE> query1 = (from iss1 in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
                                                       where iss1.issu_catg_id == ChklstID
                                                        && iss1.actv_ind == true
                                                       
                                                       && (from qlt in this.Context.QLTY_CNTRL_LISTs
                                                           where qlt.prem_adj_pgm_id == prgPrdID && qlt.prem_adj_id == prmAdjID
                                                           select qlt.chk_list_itm_id).Contains(iss1.qlty_cntrl_mstr_issu_list_id) == false
                                                       orderby iss1.srt_nbr
                                                       select new Qtly_Cntrl_ChklistBE()
                                                       {
                                                           QualityControlChklst_ID = 0,
                                                           PremumAdj_Pgm_ID = prgPrdID,
                                                           CHECKLISTITEM_ID = 0,
                                                           //CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                                                           QUALITYCONTROLTYPE_ID = iss1.issu_catg_id,
                                                           PREMIUMADJ_STATUS_ID = null,
                                                           PREMIUMADJ_ARIES_CLR_ID = null,
                                                           CUSTOMER_ID = custID,
                                                           PREMIUMADJUSTMENT_ID = prmAdjID,
                                                           ACTIVE = false,
                                                           CHKLIST_STS_CD = null,
                                                           //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                                                           //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                                                           //CUSTOMER_ID = qcc.custmr_id,
                                                           AdjChkLstItems = iss1.issu_txt,
                                                           LOOKUPID = iss1.qlty_cntrl_mstr_issu_list_id,
                                                           UpdatedDate=null
                                                           
                                                       });

            if (query.Count() > 0)
                result = query.ToList();

            if (query1.Count() > 0)
            {
                IList<Qtly_Cntrl_ChklistBE> result2 = query1.ToList();
                foreach (Qtly_Cntrl_ChklistBE qcl in result2)
                    result.Add(qcl);
            }
            return result;

           
        }

        public IList<Qtly_Cntrl_ChklistBE> getRelatedChklstItems(int ChklstID, int prmAdjID, int custID, int prgPrdID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             join iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs on qlty.chk_list_itm_id equals iss.qlty_cntrl_mstr_issu_list_id
             where iss.issu_catg_id == ChklstID
             where qlty.prem_adj_id == prmAdjID
             && qlty.custmr_id == custID
             && qlty.prem_adj_pgm_id == prgPrdID
             && iss.actv_ind == true
             
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
                 ACTIVE = qlty.actv_ind,
                 CHKLIST_STS_CD = qlty.chklist_sts_cd,
                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 AdjChkLstItems = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 LOOKUPID = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.qlty_cntrl_mstr_issu_list_id,
                 UpdatedDate = qlty.updt_dt,
                 SortNumber = Convert.ToInt32(iss.srt_nbr)

             });
            IQueryable<Qtly_Cntrl_ChklistBE> query1 = (from iss1 in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
                                                       where iss1.issu_catg_id == ChklstID && iss1.actv_ind == true


                                                       && (from qlt in this.Context.QLTY_CNTRL_LISTs
                                                           where qlt.prem_adj_pgm_id == prgPrdID && qlt.prem_adj_id == prmAdjID
                                                           select qlt.chk_list_itm_id).Contains(iss1.qlty_cntrl_mstr_issu_list_id) == false

                                                       select new Qtly_Cntrl_ChklistBE()
                                                       {
                                                           QualityControlChklst_ID = 0,
                                                           PremumAdj_Pgm_ID = prgPrdID,
                                                           CHECKLISTITEM_ID = 0,
                                                           //CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                                                           QUALITYCONTROLTYPE_ID = iss1.issu_catg_id,
                                                           PREMIUMADJ_STATUS_ID = null,
                                                           PREMIUMADJ_ARIES_CLR_ID = null,
                                                           CUSTOMER_ID = custID,
                                                           PREMIUMADJUSTMENT_ID = prmAdjID,
                                                           ACTIVE = false,
                                                           CHKLIST_STS_CD = null,
                                                           //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                                                           //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                                                           //CUSTOMER_ID = qcc.custmr_id,
                                                           AdjChkLstItems = iss1.issu_txt,
                                                           LOOKUPID = iss1.qlty_cntrl_mstr_issu_list_id,
                                                           UpdatedDate = null,
                                                           SortNumber = Convert.ToInt32(iss1.srt_nbr)
                                                       });

            if (query.Count() > 0)
                result = query.ToList();

            if (query1.Count() > 0)
            {
                IList<Qtly_Cntrl_ChklistBE> result2 = query1.ToList();
                foreach (Qtly_Cntrl_ChklistBE qcl in result2)
                    result.Add(qcl);
            }

            IEnumerable<Qtly_Cntrl_ChklistBE> sortedEnum = result.OrderBy(f => f.SortNumber);
            IList<Qtly_Cntrl_ChklistBE> sortedList = sortedEnum.ToList();

            return sortedList;


        }

        public IList<Qtly_Cntrl_ChklistBE> getApprovedInvItemInfo()
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qc in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             join lk in this.Context.LKUPs
             on qc.issu_catg_id equals lk.lkup_id
             join lktype in this.Context.LKUP_TYPs
             on lk.lkup_typ_id equals lktype.lkup_typ_id
             where lktype.lkup_typ_nm_txt == "MASTER ISSUE CHECKLIST TYPE" && lk.lkup_txt == "Adjustment Approved Invoice to Underwriting Checklist"

             select new Qtly_Cntrl_ChklistBE()
             {

                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 AdjChkLstItems = qc.issu_txt,
                 LOOKUPID = qc.qlty_cntrl_mstr_issu_list_id,
                 //ACTIVE = qc.actv_ind
                 ACTIVE=false,
                 UpdatedDate=qc.updt_dt


             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;


        }
      
    }
}
