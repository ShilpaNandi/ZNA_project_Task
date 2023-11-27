using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{    
    /// <summary>
    /// BusinessService for CustomerComments
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public class CustomerCommentsBS : BusinessServicesBase<CustomerCommentsBE, CustomerCommentsDA>
    {
        public CustomerCommentsBS()
        {
            
        }
        /// <summary>
        /// Function for retrieving Comments
        /// </summary>
        /// <param name="AccountID">AccontID</param>
        /// <returns>CustomerComments Business Entity</returns>
        public IList<CustomerCommentsBE> getRelatedComments(int accountID)
        {
            IList<CustomerCommentsBE> list = new List<CustomerCommentsBE>();
            CustomerCommentsDA commentDA = new CustomerCommentsDA();

            try
            {
                list = commentDA.getRelatedCommentInfo(accountID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
        /// <summary>
        /// Function for retrieving a particular comment corresponding to CommentID
        /// </summary>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        public CustomerCommentsBE getAssignComments(int commentID)
        {
            CustomerCommentsBE commentBE = new CustomerCommentsBE();
            commentBE = DA.Load(commentID);
            return commentBE;
        }
        /// <summary>
        /// Function for Updating the CustomerComments Business Entity
        /// </summary>
        /// <param name="COMMENTBE">CustomerCommentsBE</param>
        /// <returns>CustomerCommentsBE</returns>
        public bool Update(CustomerCommentsBE commntbe)
        {
            bool succeed = false;
            if (commntbe.CommentID > 0)
            {
                succeed = this.Save(commntbe);
            }
            else //On Insert
            {
                //PerBE.PERSON_ID = this.DA.GetNextPkID().Value;
                succeed = DA.Add(commntbe);
            }
            return succeed;
        }
    }
}
