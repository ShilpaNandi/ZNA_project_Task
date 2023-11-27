using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class AdjustmentManagementDA : DataAccessor<PREM_ADJ, AdjustmentManagementBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// get method used to retrieve Adjustment managemet list from a selected Search criteria
        /// </summary>
        /// <param name="AcctNameID"></param>
        /// <param name="AdjStatusID"></param>
        /// <param name="InvNmbr"></param>
        /// <param name="PersnID"></param>
        /// <param name="InvFrmDt"></param>
        /// <param name="InvToDt"></param>
        /// <param name="ValutnDt"></param>
        /// <returns></returns>
        public IList<AdjustmentManagementBE> getAdjManagement(int AcctNameID, int AdjStatusID, string InvNmbr, int PersnID, DateTime InvFrmDt, DateTime InvToDt, DateTime ValutnDt)
        {
            IList<AdjustmentManagementBE> resultl = new List<AdjustmentManagementBE>();

            IQueryable<AdjustmentManagementBE> query = this.BuildQuery();

            if (AcctNameID > 0)
            {
                query = query.Where(AdjManagement => AdjManagement.custmrID == AcctNameID);
            }
            if (AdjStatusID > 0)
            {
                query = query.Where(AdjManagement => AdjManagement.ADJ_STATUS_TYP_ID == AdjStatusID);
            }
            if (InvNmbr.Length > 0)
            {
                query = query.Where(AdjManagement => AdjManagement.DrftInvoicenmr == Convert.ToString(InvNmbr));
            }
            if (PersnID > 0)
            {
                //qualifier = true;
                var custQuery =
            (from custpers in this.Context.CUSTMR_PERS_RELs
             where custpers.pers_id == PersnID
             select custpers.custmr_id);

                IEnumerable<int> custList = new List<int>();
                custList = custQuery.ToList();
                query = from cu in query
                        where custList.Contains(cu.custmrID)
                        select cu;
                //query = query.Where(AdjManagement => AdjManagement.CREATE_USER_ID == PersnID);
            }
            if (ValutnDt != DateTime.Parse("1/1/0001 12:00:00 AM"))
            {
                query = query.Where(AdjManagement => AdjManagement.ValtnDate == ValutnDt);
            }
            if (InvFrmDt != DateTime.Parse("1/1/0001 12:00:00 AM") && InvToDt != DateTime.Parse("1/1/0001 12:00:00 AM"))
            {
                query = query.Where(AdjManagement => (AdjManagement.DrftInvoiceDate >= InvFrmDt && AdjManagement.DrftInvoiceDate <= InvToDt.AddDays(1)));
            }
            /*  else if(InvFrmDt != DateTime.Parse("1/1/0001 12:00:00 AM"))
              {
                  query = query.Where(AdjManagement => AdjManagement.DrftInvoiceDate >= InvFrmDt);
              }
              else if (InvToDt != DateTime.Parse("1/1/0001 12:00:00 AM"))
              {
                  query = query.Where(AdjManagement => AdjManagement.DrftInvoiceDate <= InvToDt);
              }*/

            if (query.Count() > 0)
            {
                resultl = query.ToList();

                //foreach (AdjustmentManagementBE setupBE in resultl)
                //{
                //    setupBE.PremAdjStatsBE = (new PremumAdjustdmentStatusDA()).getPremumAdjustmentStatusList(setupBE.prem_adjID);
                //}

            }
            else
            {
                resultl = null;
            }

            return resultl;
        }


        private IQueryable<AdjustmentManagementBE> BuildQuery()
        {

            if (this.Context == null)
                this.Initialize();
            IQueryable<AdjustmentManagementBE> query = from AdjMgmtDtls in this.Context.PREM_ADJs
                                                       orderby AdjMgmtDtls.CUSTMR.full_nm ascending, AdjMgmtDtls.valn_dt descending, ((AdjMgmtDtls.fnl_invc_nbr_txt == null) ? AdjMgmtDtls.drft_invc_nbr_txt : AdjMgmtDtls.fnl_invc_nbr_txt) ascending

                                                       // orderby CustomerFullName ascending, ((AdjMgmtDtls.fnl_invc_nbr_txt == null) ? AdjMgmtDtls.drft_invc_nbr_txt : AdjMgmtDtls.fnl_invc_nbr_txt) ascending,
                                                       //AdjMgmtDtls.valn_dt descending 
                                                       //join AcctDtls in this.Context.CUSTMRs 
                                                       //on AdjMgmtDtls.custmr_id equals AcctDtls.custmr_id 
                                                       select new AdjustmentManagementBE
                                                       {

                                                           prem_adjID = AdjMgmtDtls.prem_adj_id,
                                                           custmrID = AdjMgmtDtls.reg_custmr_id,
                                                           ValtnDate = AdjMgmtDtls.valn_dt,
                                                           DrftInvoicenmr = ((AdjMgmtDtls.fnl_invc_nbr_txt == null) ? AdjMgmtDtls.drft_invc_nbr_txt : AdjMgmtDtls.fnl_invc_nbr_txt),
                                                           DrftInvoiceDate = ((AdjMgmtDtls.fnl_invc_dt == null) ? AdjMgmtDtls.drft_invc_dt : AdjMgmtDtls.fnl_invc_dt),
                                                           AdjPendingIndctor = AdjMgmtDtls.adj_pendg_ind,
                                                           AdjPendingRsnID = AdjMgmtDtls.adj_pendg_rsn_id,
                                                           twntyqtrlind = AdjMgmtDtls.twenty_pct_qlty_cntrl_ind,
                                                           CREATE_DATE = AdjMgmtDtls.crte_dt,
                                                           CREATE_USER_ID = AdjMgmtDtls.crte_user_id,
                                                           UPDATE_DATE = AdjMgmtDtls.updt_dt,
                                                           UPDATE_USER_ID = AdjMgmtDtls.updt_user_id,
                                                           ReviseReasonIndc = AdjMgmtDtls.adj_rrsn_ind,
                                                           ReviseReasonID = AdjMgmtDtls.adj_rrsn_rsn_id,
                                                           VoidReasonIndc = AdjMgmtDtls.adj_can_ind,
                                                           VoidReasonID = AdjMgmtDtls.adj_void_rsn_id,
                                                           ADJ_STATUS_TYP_ID = (from pap in this.Context.PREM_ADJ_STs
                                                                                orderby pap.prem_adj_sts_id descending
                                                                                where pap.prem_adj_id == AdjMgmtDtls.prem_adj_id
                                                                                select pap.adj_sts_typ_id).First(),
                                                           Adjuststatus = (from pap in this.Context.PREM_ADJ_STs
                                                                           orderby pap.prem_adj_sts_id descending
                                                                           where pap.prem_adj_id == AdjMgmtDtls.prem_adj_id
                                                                           select pap.LKUP.lkup_txt).First(),
                                                           AdjMgmtStatusNumber = (from pap in this.Context.PREM_ADJ_STs
                                                                                  orderby pap.prem_adj_sts_id descending
                                                                                  where pap.prem_adj_id == AdjMgmtDtls.prem_adj_id
                                                                                  select pap.prem_adj_sts_id).First(),
                                                           PREM_ADJ_PGM_ID = (from pgm in this.Context.PREM_ADJ_PGMs
                                                                              join perd in this.Context.PREM_ADJ_PERDs
                                                                              on pgm.prem_adj_pgm_id equals perd.prem_adj_pgm_id
                                                                              where perd.prem_adj_id == AdjMgmtDtls.prem_adj_id
                                                                              select pgm.prem_adj_pgm_id).First(),

                                                       };

            return query;
        }


    }
}
