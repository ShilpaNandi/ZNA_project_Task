using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    //DataAccessor for PremiumAdj MiscInvoice details
    public class PremiumAdjMiscInvoiceDA : DataAccessor<PREM_ADJ_MISC_INVC, PremiumAdjMiscInvoiceBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        ///Method To Get the Records from Prem Adj Misc Invoicing Object
        /// </summary>
        /// <param name="intaccountID"></param>
        /// <param name="intpremperdID"></param>
        /// <param name="intpremadjID"></param>
        /// <returns>IList<PremiumAdjMiscInvoiceBE></returns>
        public IList<PremiumAdjMiscInvoiceBE> GetMiscInvoicelist(int intaccountID, int intpremperdID, int intpremadjID)
        {
            IList<PremiumAdjMiscInvoiceBE> result = new List<PremiumAdjMiscInvoiceBE>();

            IQueryable<PremiumAdjMiscInvoiceBE> query =
            this.BuildMiscInvoiceList(intaccountID, intpremperdID, intpremadjID);
            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Method to Buid the Query to retrieve Misc Invoice information
        /// </summary>
        /// <param name="intaccountID"></param>
        /// <param name="intpremperdID"></param>
        /// <param name="intpremadjID"></param>
        /// <returns>IQueryable<PremiumAdjMiscInvoiceBE></returns>
        private IQueryable<PremiumAdjMiscInvoiceBE> BuildMiscInvoiceList(int intaccountID, int intpremperdID, int intpremadjID)
        {
            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve Misc Invoice information
            /// and project it into Misc Invoice Business Entity
            IQueryable<PremiumAdjMiscInvoiceBE> MiscInvoiceList = from MiscInvoice in this.Context.PREM_ADJ_MISC_INVCs
                                                                  join post in this.Context.POST_TRNS_TYPs on MiscInvoice.post_trns_typ_id equals post.post_trns_typ_id
                                                                  where MiscInvoice.custmr_id == intaccountID
                                                                  && MiscInvoice.prem_adj_perd_id == intpremperdID
                                                                  && MiscInvoice.prem_adj_id == intpremadjID
                                                                  orderby MiscInvoice.POST_TRNS_TYP.trns_nm_txt ascending
                                                                  select new PremiumAdjMiscInvoiceBE
                                                                  {
                                                                      PREM_ADJ_MISC_INVC_ID = MiscInvoice.prem_adj_misc_invc_id,
                                                                      POSTTRNSTYPE = post.trns_nm_txt,
                                                                      POST_TRANS_TYP_ID = MiscInvoice.post_trns_typ_id,
                                                                      POST_AMT = MiscInvoice.post_amt,
                                                                      POL_MODULUS_TXT = MiscInvoice.pol_modulus_txt,
                                                                      POL_NBR_TXT = MiscInvoice.pol_nbr_txt,
                                                                      POL_SYM_TXT = MiscInvoice.pol_sym_txt,
                                                                      ACTV_IND = MiscInvoice.actv_ind,
                                                                      MISC_POSTS_IND = post.post_ind,
                                                                      POL_REQR_IND=post.pol_reqr_ind,
                                                                      ADJ_SUMRY_POST_FLAG_IND = post.adj_sumry_ind,
                                                                      POLICYNUMBER = MiscInvoice.pol_sym_txt + " " + MiscInvoice.pol_nbr_txt + " " + MiscInvoice.pol_modulus_txt,
                                                                      UPDATE_DATE=MiscInvoice.updt_dt,
                                                                  };
            return MiscInvoiceList;
        }


    }
}
