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
    public class PostingTransactionTypeDA : DataAccessor<POST_TRNS_TYP, PostingTransactionTypeBE, AISDatabaseLINQDataContext>
    {
        public IList<PostingTransactionTypeBE> getList()
        {
            IList<PostingTransactionTypeBE> result = new List<PostingTransactionTypeBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PostingTransactionTypeBE> query =
            (from cdd in this.Context.POST_TRNS_TYPs
             join lkp in this.Context.LKUPs
             on cdd.trns_typ_id equals lkp.lkup_id
             orderby lkp.lkup_txt ascending, cdd.trns_nm_txt ascending, cdd.main_nbr_txt ascending, cdd.sub_nbr_txt ascending
             select new PostingTransactionTypeBE()
             {
                 POST_TRANS_TYP_ID = cdd.post_trns_typ_id,
                 TRNS_TYP_ID = cdd.trns_typ_id,
                 TRANS_TXT = cdd.trns_nm_txt,
                 MAIN_NBR = cdd.main_nbr_txt,
                 SUB_NBR = cdd.sub_nbr_txt,
                 COMP_TXT = cdd.comp_txt,
                 INVOICBL_IND = cdd.invoicbl_ind,
                 MISC_POSTS_IND = cdd.post_ind,
                 THRD_PTY_ADMIN_MNL_IND = cdd.thrd_pty_admin_mnl_ind,
                 ADJ_SUMRY_NOT_POST_IND = cdd.adj_sumry_ind,
                 POL_REQR_IND = cdd.pol_reqr_ind,
                 ACTV_IND = cdd.actv_ind,
                 Created_Date = cdd.crte_dt,
                 Created_UserID = cdd.crte_user_id,
                 TRANSACTIONTYPE = lkp.lkup_txt,
                 UPDATE_DATE=cdd.updt_dt

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        public IList<PostingTransactionTypeBE> getTPAList()
        {
            IList<PostingTransactionTypeBE> result = new List<PostingTransactionTypeBE>();
            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PostingTransactionTypeBE> query =
            (from cdd in this.Context.POST_TRNS_TYPs
             join lkp in this.Context.LKUPs
             on cdd.trns_typ_id equals lkp.lkup_id
             orderby cdd.trns_nm_txt ascending, cdd.main_nbr_txt ascending, cdd.sub_nbr_txt ascending
             where cdd.trns_typ_id == 460//TPA Manual
             && cdd.actv_ind == true
             //where cdd.thrd_pty_admin_mnl_ind == true
             select new PostingTransactionTypeBE()
             {
                 POST_TRANS_TYP_ID = cdd.post_trns_typ_id,
                 TRNS_TYP_ID = cdd.trns_typ_id,
                 TRANS_TXT = cdd.trns_nm_txt,
                 MAIN_NBR = cdd.main_nbr_txt,
                 SUB_NBR = cdd.sub_nbr_txt,
                 COMP_TXT = cdd.comp_txt,
                 INVOICBL_IND = cdd.invoicbl_ind,
                 MISC_POSTS_IND = cdd.post_ind,
                 THRD_PTY_ADMIN_MNL_IND = cdd.thrd_pty_admin_mnl_ind,
                 ADJ_SUMRY_NOT_POST_IND = cdd.adj_sumry_ind,
                 POL_REQR_IND = cdd.pol_reqr_ind,
                 ACTV_IND = cdd.actv_ind,
                 Created_Date = cdd.crte_dt,
                 Created_UserID = cdd.crte_user_id,
                 TRANSACTIONTYPE = lkp.lkup_txt,
                 UPDATE_DATE=cdd.updt_dt

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        public IList<PostingTransactionTypeBE> getPremAdjMiscInvoiceData()
        {
            IList<PostingTransactionTypeBE> result = new List<PostingTransactionTypeBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PostingTransactionTypeBE> query =
            (from cdd in this.Context.POST_TRNS_TYPs
             where cdd.trns_typ_id == 444 && cdd.actv_ind==true
             orderby cdd.trns_nm_txt descending
             select new PostingTransactionTypeBE()
             {
                 POST_TRANS_TYP_ID = cdd.post_trns_typ_id,
                 COMP_TXT = cdd.comp_txt,
                 TRANS_TXT = cdd.trns_nm_txt
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// To retrieve All Active Post TransactionTypes union Used type(Active/Inactive) in EditMode in AC-MiscInvoicing Screen
        /// </summary>
        /// <param name="intPostTransTypID"></param>
        /// <returns></returns>
        public IList<PostingTransactionTypeBE> getPremAdjMiscInvoiceEditData(int intPostTransTypID)
        {
            IList<PostingTransactionTypeBE> result = new List<PostingTransactionTypeBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PostingTransactionTypeBE> query =
            (from cdd in this.Context.POST_TRNS_TYPs
             where cdd.actv_ind == true && cdd.trns_typ_id == 444
             orderby cdd.trns_nm_txt descending
             select new PostingTransactionTypeBE()
             {
                 POST_TRANS_TYP_ID = cdd.post_trns_typ_id,
                 COMP_TXT = cdd.comp_txt,
                 TRANS_TXT = cdd.trns_nm_txt
             }).Union(from post in this.Context.POST_TRNS_TYPs
                      where post.post_trns_typ_id == intPostTransTypID
                      select new PostingTransactionTypeBE()
                      {
                          POST_TRANS_TYP_ID = post.post_trns_typ_id,
                          COMP_TXT = post.comp_txt,
                          TRANS_TXT = post.trns_nm_txt
                      }
                      );

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool IsPolicyRequires(int intPostTransTypID)
        {
            bool isPolReq = false;
            if (this.Context == null)
                this.Initialize();

            if (intPostTransTypID > 0)
            {
                isPolReq = Convert.ToBoolean((from cdd in this.Context.POST_TRNS_TYPs
                                              where cdd.post_trns_typ_id == intPostTransTypID
                                              select cdd.pol_reqr_ind).First());
            }

            return isPolReq;

        }

        public bool IsMainSubExits(string strTransactionText, string strTransactionTypeText)
        {
            IList<PostingTransactionTypeBE> result = new List<PostingTransactionTypeBE>();

            if (this.Context == null)
                this.Initialize();
           
          IQueryable<PostingTransactionTypeBE> query =
          (from cdd in this.Context.POST_TRNS_TYPs
           join lkp in this.Context.LKUPs
           on cdd.trns_typ_id equals lkp.lkup_id
           where cdd.trns_nm_txt.Trim()==strTransactionText.Trim()
           && lkp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "TRANSACTION TYPE" && cond.actv_ind == true).First().lkup_typ_id
           && cdd.trns_typ_id == this.Context.LKUPs.Where(cond => cond.lkup_txt.Trim() == strTransactionTypeText.Trim() && cond.actv_ind == true).First().lkup_id
           && cdd.actv_ind==true
           select new PostingTransactionTypeBE()
           {
               POST_TRANS_TYP_ID = cdd.post_trns_typ_id,
               TRNS_TYP_ID = cdd.trns_typ_id,
               TRANS_TXT = cdd.trns_nm_txt,
               MAIN_NBR = cdd.main_nbr_txt,
               SUB_NBR = cdd.sub_nbr_txt,
               COMP_TXT = cdd.comp_txt,
               
           });

            
            if (query.Count() > 0)
               return true;
            else
                return false;

             
           
        }
    }
}

