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
    public class AdjustmentReviewCommentsBS : BusinessServicesBase<AdjustmentReviewCommentsBE, AdjustmentReviewCommentsDA>
    {
        /// <summary>
        /// Retrieves AdjustmentReviewComments 
        /// </summary>
        /// <returns>AdjustmentReviewCommentsBE</returns>
        public AdjustmentReviewCommentsBE getAdjReviewCmmntINVOICEData(int cmmntCatgID, int PrgAdjID, int custmrID)
        {
            AdjustmentReviewCommentsDA adjRevCmmnt = new AdjustmentReviewCommentsDA();
            AdjustmentReviewCommentsBE result = adjRevCmmnt.getAdjReviewCmmntINVOICEData(cmmntCatgID, PrgAdjID, custmrID);
            return result;
        }
        /// <summary>
        /// Retrieves AdjustmentReviewComments 
        /// </summary>
        /// <returns>AdjustmentReviewCommentsBE</returns>
        public AdjustmentReviewCommentsBE getAdjReviewCmmntALLData(int cmmntCatgID, int PrgPrdID, int custmrID)
        {
            AdjustmentReviewCommentsDA adjRevCmmnt = new AdjustmentReviewCommentsDA();
            AdjustmentReviewCommentsBE result = adjRevCmmnt.getAdjReviewCmmntALLData(cmmntCatgID, PrgPrdID, custmrID);
            return result;
        }
        
        /// <summary>
        /// Retrieves AdjustmentReviewComments 
        /// </summary>
        /// <returns>List of AdjustmentReviewCommentsBE</returns>
        public AdjustmentReviewCommentsBE getAdjustmentReviewCommentsRow(int ID)
        {
            AdjustmentReviewCommentsBE AdjustmentReviewCommentsBE = new AdjustmentReviewCommentsBE();
            AdjustmentReviewCommentsBE = DA.Load(ID);
            return AdjustmentReviewCommentsBE;
        }
        /// <summary>
        /// Insert AdjustmentReviewComments for all Adjustment Review Comments
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(AdjustmentReviewCommentsBE adjRevCmmntBE)
        {
            bool succeed = false;
            try
            {
                if (adjRevCmmntBE.PREM_ADJ_CMMNT_ID > 0) //On Update
                {
                    succeed = this.Save(adjRevCmmntBE);

                }
                else //On Insert
                {
                    adjRevCmmntBE.PREM_ADJ_CMMNT_ID = DA.GetNextPkID().Value;

                    succeed = DA.Add(adjRevCmmntBE);
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;

        }

        public bool IsReportAvialable(int intPremAdjID, int IFlag, string strDocName)
        {
            AdjustmentReviewCommentsDA adjRevCmmnt = new AdjustmentReviewCommentsDA();
            return adjRevCmmnt.IsReportAvialable(intPremAdjID, IFlag, strDocName);
        
        }
        /// <summary>
        ///This method is to verify the duplicate comments for this adjustment for the summaryInvoice
        ///Users are allowed to enter the comments for only one program period under
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateComment(int premAdjID, int custmrID, int cmmntCatgID)
        {
            AdjustmentReviewCommentsDA adjRevCmmnt = new AdjustmentReviewCommentsDA();
            return adjRevCmmnt.IsDuplicateComment(premAdjID, custmrID, cmmntCatgID);

        }
        /// <summary>
        ///This method is to get the Program Period for which comments already exists for this adjustment for the summaryInvoice
        ///Users are allowed to enter the comments for only one program period 
        /// </summary>
        /// <returns></returns>
        public ProgramPeriodSearchListBE getProgramPeriod(int premAdjID, int custmrID, int cmmntCatgID)
        {
            ProgramPeriodSearchListBE ProgramPeriodBE = new ProgramPeriodSearchListBE();
            ProgramPeriodBE = DA.getProgramPeriod(premAdjID, custmrID, cmmntCatgID);
            return ProgramPeriodBE;
        }
    }
}
