using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.DAL.Logic
{
    public class QCMasterIssueListDA:DataAccessor<QLTY_CNTRL_MSTR_ISSU_LIST, QCMasterIssueListBE,AISDatabaseLINQDataContext>
    {
        public QCMasterIssueListDA()
        {
 
        }
        /// <summary>
        /// To Retrieve records based on issueCateID
        /// </summary>
        /// <param name="issueCateID"></param>
        /// <returns>IList<QCMasterIssueListBE></returns>
        public IList<QCMasterIssueListBE> getQltyIssueList(int issueCateID)
        {
            IList<QCMasterIssueListBE> result = new List<QCMasterIssueListBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<QCMasterIssueListBE> query =
            (from cdd in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             join lk in this.Context.LKUPs
             on cdd.issu_catg_id equals lk.lkup_id
             where cdd.issu_catg_id == issueCateID 
             && lk.actv_ind == true
             && cdd.actv_ind == true
             select new QCMasterIssueListBE()
             {
                 QualityCntrlMstrIsslstID = cdd.qlty_cntrl_mstr_issu_list_id,
                 IssCatgID = cdd.issu_catg_id,
                 IssueCategory = lk.lkup_txt,
                 Str_Nbr=cdd.srt_nbr,
                 IssueText = cdd.issu_txt,
                 FinancialIndicator = cdd.finc_ind,
                 ACTIVE=cdd.actv_ind,
                 CreatedUserID = cdd.crte_user_id,
                 CreatedDate = cdd.crte_dt,
                 UpdatedUserID = cdd.updt_user_id,
                 UpdatedDate = cdd.updt_dt
             });


            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        public IList<QCMasterIssueListBE> getQltyIssueListALL(int issueCateID)
        {
            IList<QCMasterIssueListBE> result = new List<QCMasterIssueListBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<QCMasterIssueListBE> query =
            (from cdd in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             join lk in this.Context.LKUPs
             on cdd.issu_catg_id equals lk.lkup_id
             where cdd.issu_catg_id == issueCateID
             && lk.actv_ind == true
             select new QCMasterIssueListBE()
             {
                 QualityCntrlMstrIsslstID = cdd.qlty_cntrl_mstr_issu_list_id,
                 IssCatgID = cdd.issu_catg_id,
                 IssueCategory = lk.lkup_txt,
                 Str_Nbr = cdd.srt_nbr,
                 IssueText = cdd.issu_txt,
                 FinancialIndicator = cdd.finc_ind,
                 ACTIVE = cdd.actv_ind,
                 CreatedUserID = cdd.crte_user_id,
                 CreatedDate = cdd.crte_dt,
                 UpdatedUserID = cdd.updt_user_id,
                 UpdatedDate = cdd.updt_dt
             });


            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        /// <summary>
        /// Checks for Duplicate of IssueName
        /// </summary>
        /// <param name="LookupID"></param>
        /// <param name="intQCMasterIssueID"></param>
        /// <param name="IssueName"></param>
        /// <returns>True/False(bool)</returns>
        public bool IsExistsIssueName(int LookupID,int intQCMasterIssueID, string IssueName)
        {
            bool Flag = false;
            var QcMaster = from cdd in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
                           where (cdd.issu_catg_id == LookupID && cdd.issu_txt == IssueName.Trim()&& cdd.qlty_cntrl_mstr_issu_list_id!=intQCMasterIssueID)
                         select new { cdd.issu_txt };
            if (QcMaster.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }
    }
}
