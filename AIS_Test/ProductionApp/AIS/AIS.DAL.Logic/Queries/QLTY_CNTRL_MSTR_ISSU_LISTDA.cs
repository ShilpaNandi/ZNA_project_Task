using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{
    public class QLTY_CNTRL_MSTR_ISSU_LISTDA : DataAccessor<QLTY_CNTRL_MSTR_ISSU_LIST, QCMasterIssueListBE, AISDatabaseLINQDataContext>
    {
        public IList<QCMasterIssueListBE> getQltyIssueList(int issueCateID)
        {
            IList<QCMasterIssueListBE> result = new List<QCMasterIssueListBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<QCMasterIssueListBE> query =
            (from cdd in this.Context.QLTY_CNTRL_MSTR_ISSU_LISTs
             where cdd.issu_catg_id == issueCateID
             select new QCMasterIssueListBE()
             {
                 QualityCntrlMstrIsslstID = cdd.qlty_cntrl_mstr_issu_list_id,
                 IssCatgID= cdd.issu_catg_id,
                 IssueText=cdd.issu_txt,
             });
            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
    }
}
