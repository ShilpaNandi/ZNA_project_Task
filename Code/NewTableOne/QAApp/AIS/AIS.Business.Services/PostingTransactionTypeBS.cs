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
using ZurichNA.AIS.ExceptionHandling;
namespace ZurichNA.AIS.Business.Logic
{
    public class PostingTransactionTypeBS : BusinessServicesBase<PostingTransactionTypeBE, PostingTransactionTypeDA>
    {
        /// <summary>
        /// Retrieve Posting Transaction Type setup information
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public IList<PostingTransactionTypeBE> getList()
        {
            IList<PostingTransactionTypeBE> List = new List<PostingTransactionTypeBE>();
            PostingTransactionTypeDA PostTranDA = new PostingTransactionTypeDA();
            try
            {
                List = PostTranDA.getList();
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return List;
        }

        /// <summary>
        /// Retrieve Posting Transaction Type setup information(TPA only types)
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public IList<PostingTransactionTypeBE> getTPAList()
        {
            IList<PostingTransactionTypeBE> List = new List<PostingTransactionTypeBE>();
            PostingTransactionTypeDA PostTranDA = new PostingTransactionTypeDA();
            try
            {
                List = PostTranDA.getTPAList();
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return List;
        }
        public IList<PostingTransactionTypeBE> getPremAdjMiscInvoiceData()
        {
            IList<PostingTransactionTypeBE> list = new List<PostingTransactionTypeBE>();
            PostingTransactionTypeDA PremAdjMiscInvoice = new PostingTransactionTypeDA();

            try
            {
                list = PremAdjMiscInvoice.getPremAdjMiscInvoiceData();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            PostingTransactionTypeBE selPostingTransactionTypeBE = new PostingTransactionTypeBE();
            selPostingTransactionTypeBE.POST_TRANS_TYP_ID = 0;
            selPostingTransactionTypeBE.TRANS_TXT = "(Select)";
            list.Insert(0, selPostingTransactionTypeBE);
            return list;
        }
        /// <summary>
        /// Retrieves particular Posting Transaction Type setup information record
        /// </summary>
        /// <param name="KOSetupId"></param>
        /// <returns></returns>
        public PostingTransactionTypeBE LoadData(int PostTransSetupId)
        {
            PostingTransactionTypeBE Data = new PostingTransactionTypeBE();
             try
            {
                Data = DA.Load(PostTransSetupId);
            }
             catch (Exception ex)
             {

                 RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                 throw myException;
             }
            return Data;
        }

        /// <summary>
        /// Saves or Edits to database using FrameWork
        /// </summary>
        /// <param name="PostingTransactionTypeBE"></param>
        /// <returns>true if save, False if failed to save</returns>
        public bool SaveSetupData(PostingTransactionTypeBE Data)
        {
            try
            {
                if (Data.POST_TRANS_TYP_ID > 0)
                {
                    DA.Update(Data);
                }
                else
                {
                    DA.Add(Data);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// To retrieve All Active Post TransactionTypes union Used type(Active/Inactive) in EditMode in AC-MiscInvoicing Screen
        /// </summary>
        /// <param name="intPostTransTypID"></param>
        /// <returns></returns>
        public IList<PostingTransactionTypeBE> getPremAdjMiscInvoiceEditData(int intPostTransTypID)
        {
            IList<PostingTransactionTypeBE> list = new List<PostingTransactionTypeBE>();
            PostingTransactionTypeDA PremAdjMiscInvoice = new PostingTransactionTypeDA();

            try
            {
                list = PremAdjMiscInvoice.getPremAdjMiscInvoiceEditData(intPostTransTypID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            PostingTransactionTypeBE selPostingTransactionTypeBE = new PostingTransactionTypeBE();
            selPostingTransactionTypeBE.POST_TRANS_TYP_ID = 0;
            selPostingTransactionTypeBE.TRANS_TXT = "(Select)";
            list.Insert(0, selPostingTransactionTypeBE);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intPostTransTypID"></param>
        /// <returns></returns>
        public bool IsPolicyRequires(int intPostTransTypID)
        {
            PostingTransactionTypeDA postTrans = new PostingTransactionTypeDA();
            bool isPolReq = false;
            try
            {
                isPolReq = postTrans.IsPolicyRequires(intPostTransTypID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return isPolReq;
        }
    }

}

