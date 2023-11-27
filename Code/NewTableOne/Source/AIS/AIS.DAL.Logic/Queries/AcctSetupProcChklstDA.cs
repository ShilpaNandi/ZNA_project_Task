using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Configuration;
namespace ZurichNA.AIS.DAL.Logic
{
    public class AcctSetupProcChklstDA : DataAccessor<QLTY_CNTRL_LIST, Qtly_Cntrl_ChklistBE, AISDatabaseLINQDataContext>
    {
        public IList<Qtly_Cntrl_ChklistBE> getAllChklstItems()
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
             && lk.lkup_txt == "Account Setup Processing Checklist"
             && qc.actv_ind == true
             orderby qc.srt_nbr
             select new Qtly_Cntrl_ChklistBE()
             {
                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 ChkLstItems = qc.issu_txt,
                 LOOKUPID = qc.qlty_cntrl_mstr_issu_list_id,
                 ACTIVE = false

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;


        }

        public IList<Qtly_Cntrl_ChklistBE> getRelatedChklstItems(int ChklstID, int prmPrdID, int custID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             join iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             on qlty.chk_list_itm_id equals iss.qlty_cntrl_mstr_issu_list_id
             where iss.issu_catg_id == ChklstID
            && qlty.prem_adj_pgm_id == prmPrdID
            && qlty.custmr_id == custID
            && iss.actv_ind == true


             orderby iss.srt_nbr
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = qlty.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = qlty.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = qlty.chk_list_itm_id,
                 //CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 QUALITYCONTROLTYPE_ID = iss.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qlty.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qlty.prem_adj_aries_clring_id,
                 CUSTOMER_ID = qlty.custmr_id,
                 PREMIUMADJUSTMENT_ID = qlty.prem_adj_id,
                 ACTIVE = qlty.actv_ind,
                 CHKLIST_STS_CD = qlty.chklist_sts_cd,
                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 ChkLstItems = iss.issu_txt,
                 LOOKUPID = iss.qlty_cntrl_mstr_issu_list_id,
                 SortNumber = Convert.ToInt32(iss.srt_nbr)


             });
            IQueryable<Qtly_Cntrl_ChklistBE> query1 = (from iss1 in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
                                                       where iss1.issu_catg_id == ChklstID &&  iss1.actv_ind == true

                                                       && (from qlt in this.Context.QLTY_CNTRL_LISTs
                                                           where qlt.prem_adj_pgm_id == prmPrdID
                                                           select qlt.chk_list_itm_id).Contains(iss1.qlty_cntrl_mstr_issu_list_id) == false
                                                       select new Qtly_Cntrl_ChklistBE()
                                                       {
                                                           QualityControlChklst_ID = 0,
                                                           PremumAdj_Pgm_ID = prmPrdID,
                                                           CHECKLISTITEM_ID = 0,
                                                           //CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                                                           QUALITYCONTROLTYPE_ID = iss1.issu_catg_id,
                                                           PREMIUMADJ_STATUS_ID = null,
                                                           PREMIUMADJ_ARIES_CLR_ID = null,
                                                           CUSTOMER_ID = custID,
                                                           PREMIUMADJUSTMENT_ID = null,
                                                           ACTIVE = iss1.actv_ind,
                                                           CHKLIST_STS_CD = null,
                                                           //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                                                           //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                                                           //CUSTOMER_ID = qcc.custmr_id,
                                                           ChkLstItems = iss1.issu_txt,
                                                           LOOKUPID = iss1.qlty_cntrl_mstr_issu_list_id,
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
            //return result;

            IEnumerable<Qtly_Cntrl_ChklistBE> sortedEnum = result.OrderBy(f => f.SortNumber);
            IList<Qtly_Cntrl_ChklistBE> sortedList = sortedEnum.ToList();

            return sortedList;
        }

        public IList<Qtly_Cntrl_ChklistBE> getRelatedChklstItems_Newest(int ChklstID, int prmPrdID, int custID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from qlty in this.Context.QLTY_CNTRL_LISTs
             join iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             on qlty.chk_list_itm_id equals iss.qlty_cntrl_mstr_issu_list_id
             where iss.issu_catg_id == ChklstID
            && qlty.prem_adj_pgm_id == prmPrdID
            && qlty.custmr_id == custID

            && iss.actv_ind == true
            && qlty.actv_ind == true
             orderby iss.srt_nbr
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = qlty.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = qlty.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = qlty.chk_list_itm_id,
                 //CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 QUALITYCONTROLTYPE_ID = iss.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qlty.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qlty.prem_adj_aries_clring_id,
                 CUSTOMER_ID = qlty.custmr_id,
                 PREMIUMADJUSTMENT_ID = qlty.prem_adj_id,
                 ACTIVE = qlty.actv_ind,
                 CHKLIST_STS_CD = qlty.chklist_sts_cd,
                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 ChkLstItems = iss.issu_txt,
                 LOOKUPID = iss.qlty_cntrl_mstr_issu_list_id,
                 SortNumber = Convert.ToInt32(iss.srt_nbr),


             });
            IQueryable<Qtly_Cntrl_ChklistBE> query1 = (from iss1 in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
                                                       where iss1.issu_catg_id == ChklstID && iss1.actv_ind == true

                      && (from qlt in this.Context.QLTY_CNTRL_LISTs
                          where qlt.prem_adj_pgm_id == prmPrdID
                          && qlt.actv_ind == true
                          select qlt.chk_list_itm_id).Contains(iss1.qlty_cntrl_mstr_issu_list_id) == false
                                                       orderby iss1.srt_nbr
                                                       select new Qtly_Cntrl_ChklistBE()
                                                       {
                                                           QualityControlChklst_ID = 0,
                                                           PremumAdj_Pgm_ID = prmPrdID,
                                                           CHECKLISTITEM_ID = 0,
                                                           //CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                                                           QUALITYCONTROLTYPE_ID = iss1.issu_catg_id,
                                                           PREMIUMADJ_STATUS_ID = null,
                                                           PREMIUMADJ_ARIES_CLR_ID = null,
                                                           CUSTOMER_ID = custID,
                                                           PREMIUMADJUSTMENT_ID = null,
                                                           ACTIVE = iss1.actv_ind,
                                                           CHKLIST_STS_CD = null,
                                                           //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                                                           //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                                                           //CUSTOMER_ID = qcc.custmr_id,
                                                           ChkLstItems = iss1.issu_txt,
                                                           LOOKUPID = iss1.qlty_cntrl_mstr_issu_list_id,
                                                           SortNumber = Convert.ToInt32(iss1.srt_nbr),
                                                       });

            if (query.Count() > 0)
                result = query.ToList();

            if (query1.Count() > 0)
            {
                IList<Qtly_Cntrl_ChklistBE> result2 = query1.ToList();
                foreach (Qtly_Cntrl_ChklistBE qcl in result2)
                    result.Add(qcl);
            }


            //var resultList = result.OrderBy(r=>r.SortNumber);
            //resultList = resultList.OrderBy(r => r.SortNumber);
            //result = (IList<Qtly_Cntrl_ChklistBE>)resultList;

            //return result.OrderBy(r => r.SortNumber).ToList<Qtly_Cntrl_ChklistBE>();

            IEnumerable<Qtly_Cntrl_ChklistBE> sortedEnum = result.OrderBy(f => f.SortNumber);
            IList<Qtly_Cntrl_ChklistBE> sortedList = sortedEnum.ToList();

            return sortedList;
        }





        public IList<Qtly_Cntrl_ChklistBE> getRelatedChklstItems_New(int ChklstID, int prmPrdID, int custID)
        {
            IList<Qtly_Cntrl_ChklistBE> result = new List<Qtly_Cntrl_ChklistBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Qtly_Cntrl_ChklistBE> query =
            (from iss in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             join qlty in this.Context.QLTY_CNTRL_LISTs
             on iss.qlty_cntrl_mstr_issu_list_id equals qlty.chk_list_itm_id into t
             from qltys in t.Where(q => q.prem_adj_pgm_id == prmPrdID
                 && q.custmr_id == custID
                 && q.actv_ind == true).DefaultIfEmpty()
             where iss.issu_catg_id == ChklstID
             && iss.actv_ind == true
             orderby iss.srt_nbr
             select new Qtly_Cntrl_ChklistBE()
             {
                 QualityControlChklst_ID = (qltys.qlty_cntrl_list_id == null) ? 0 : qltys.qlty_cntrl_list_id,
                 PremumAdj_Pgm_ID = (qltys.prem_adj_pgm_id == null) ? prmPrdID : qltys.prem_adj_pgm_id,
                 CHECKLISTITEM_ID = (qltys.chk_list_itm_id == null) ? 0 : qltys.chk_list_itm_id,
                 //CHKLISTNAME = qlty.QLTY_CNTRL_MSTR_ISSU_LIST.issu_txt,
                 QUALITYCONTROLTYPE_ID = iss.issu_catg_id,
                 PREMIUMADJ_STATUS_ID = qltys.prem_adj_sts_id,
                 PREMIUMADJ_ARIES_CLR_ID = qltys.prem_adj_aries_clring_id,
                 CUSTOMER_ID = (qltys.custmr_id == null) ? custID : qltys.custmr_id,
                 PREMIUMADJUSTMENT_ID = qltys.prem_adj_id,
                 ACTIVE = (qltys.actv_ind == null) ? iss.actv_ind : qltys.actv_ind,
                 CHKLIST_STS_CD = qltys.chklist_sts_cd,
                 //QualityControlChklst_ID = qcc.qlty_cntrl_chklist_id,
                 //CHECKLISTITEM_ID = qcc.chk_list_itm_id,
                 //CUSTOMER_ID = qcc.custmr_id,
                 ChkLstItems = iss.issu_txt,
                 LOOKUPID = iss.qlty_cntrl_mstr_issu_list_id,
                 SortNumber = Convert.ToInt32(iss.srt_nbr),
                 CreatedDate = iss.crte_dt

             });


            if (query.Count() > 0)
                result = query.ToList();

            //var resultList = result.OrderBy(r=>r.SortNumber);
            //resultList = resultList.OrderBy(r => r.SortNumber);
            //result = (IList<Qtly_Cntrl_ChklistBE>)resultList;

            //List<Qtly_Cntrl_ChklistBE> ra = result.ToList();

            //ra.Sort(delegate(Qtly_Cntrl_ChklistBE x, Qtly_Cntrl_ChklistBE y)
            //{
            //    return x.SortNumber.CompareTo(y.SortNumber);
            //});

            //return ra;

            //return result.OrderBy(r => r.SortNumber).ToList();
            return result;

        }


        public void UpdateQltycntrlchklst(Qtly_Cntrl_ChklistBE qltycntrlchklstBE)
        {
            try
            {
                int act_ind = (Convert.ToBoolean(qltycntrlchklstBE.ACTIVE) == true) ? 1 : 0;
                SqlCommand sqlCmd = new SqlCommand();
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
                sqlCmd.CommandTimeout = 0;
                string strQuery = @"update QLTY_CNTRL_LIST set CHKLIST_STS_CD='" + qltycntrlchklstBE.CHKLIST_STS_CD + "',actv_ind=" + act_ind + ",prem_adj_pgm_id=" + qltycntrlchklstBE.PremumAdj_Pgm_ID + ",chk_list_itm_id=" + qltycntrlchklstBE.CHECKLISTITEM_ID + ",updt_dt=GETDATE(),updt_user_id=" + qltycntrlchklstBE.UpdatedUserID + " where qlty_cntrl_list_id=" + qltycntrlchklstBE.QualityControlChklst_ID + "";
                sqlCmd.CommandText = strQuery;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = conn;
                conn.Open();
                sqlCmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
    
}
