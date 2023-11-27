using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.DAL.Logic
{
  public class AriesClearingDA :DataAccessor<PREM_ADJ_ARIES_CLRING,AriesClearingBE,AISDatabaseLINQDataContext>
    {
      public AriesClearingDA()
      { }
/// <summary>
/// This method returns the aries clearing details list.
/// </summary>
/// <param name="Premadjustid"></param>
/// <param name="customerid"></param>
/// <returns></returns>
      public IList<AriesClearingBE> GetAriesClearingDetails(int Premadjustid, int customerid)
      {
          IList<AriesClearingBE> result = new List<AriesClearingBE>();
          IQueryable<AriesClearingBE> query = (from aries in this.Context.PREM_ADJ_ARIES_CLRINGs
                                               where aries.prem_adj_id == Premadjustid && aries.custmr_id == customerid

                                               select new AriesClearingBE
                                               {
                                                     PREMIUM_ADJUST_CLEARING_ID=aries.prem_adj_aries_clring_id ,
                                                     PREMIUM_ADJUSTMENT_ID=aries.prem_adj_id,
                                                     CUSTOMER_ID=aries.custmr_id,
                                                     QULAITY_PERSON_ID=aries.qlty_cntrl_pers_id,
                                                      RECON_DUE_DATE=aries.recon_due_dt,
                                                      RECON_DATE=aries.recon_dt,
                                                      QUALITY_CONTROL_DATE=aries.qlty_cntrl_dt,
                                                      ARIES_POST_DATE=aries.aries_post_dt,
                                                      CHECK_NUMBER_TEXT=aries.chk_nbr_txt,
                                                      ARIES_PAYMENT_AMOUNT=aries.aries_paymt_amt,
                                                      BILLED_ITEM_CLEAR_DATE=aries.biled_itm_clring_dt,
                                                    //PREM_ADJUSTMENT_COMMENT_ID=aries.prem_adj_cmmnt_id,
                                                    COMMENTS_TEXT=aries.cmmnt_txt,
                                                      QUALITY_CTRL_IND=aries.qlty_cntrl_ind,
                                                      UPDATED_USR_ID=aries.updt_user_id,
                                                      UPDATED_DATE=aries.updt_dt,
                                                      CREATED_USER_ID=aries.crte_user_id,
                                                       CREATED_DATE=aries.crte_dt,
                                                       ARIES_COMPL_IND=aries.aries_cmplt_ind
                                               });
            result = query.ToList();
          return result;
      
      
      }
    }
}
