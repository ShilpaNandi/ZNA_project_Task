using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{   
    /// <summary>
    /// DataAcessor For CustomerComments details
    /// </summary>
    /// <param name="AccountID">AccontID</param>
    /// <returns></returns>
    public class CustomerCommentsDA : DataAccessor<CUSTMR_CMMNT, CustomerCommentsBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Function for retrieving Comments Corresponding to AccountID
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<CustomerCommentsBE> getRelatedCommentInfo(int accountID)
        {
            IList<CustomerCommentsBE> result = new List<CustomerCommentsBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve comment information
            /// into CustomerComments Business Entity
            IQueryable<CustomerCommentsBE> query =
            (from cdd in this.Context.CUSTMR_CMMNTs
             join lk in this.Context.LKUPs
             on cdd.cmmnt_catg_id equals lk.lkup_id
             //join per in this.Context.PERs
             // on cdd.crte_user_id equals per.pers_id
               where cdd.custmr_id == accountID
//               orderby cdd.crte_dt descending,cdd.crte_user_id ascending
             select new CustomerCommentsBE()
             {
                 CommentID = cdd.custmr_cmmnt_id,
                 CustomerID = cdd.custmr_id,
                 CommentDate = cdd.crte_dt,
                 CommentBY = cdd.crte_user_id,
                 CommentText = cdd.cmmnt_txt,
                 CommentCategoryID = cdd.cmmnt_catg_id,
                 CommentCategoryName = lk.lkup_txt,
                  //CommentUserName = per.surname+", "+per.forename
                  CommentUserName = (from per in this.Context.PERs
                                    where cdd.crte_user_id  == per.pers_id
                                    select per.surname+", "+per.forename
                                     ).First().ToString()
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            result = result.OrderBy(res => res.CommentUserName).OrderByDescending(res => res.CommentDate).ToList();
            return result;
        }
    }
}
