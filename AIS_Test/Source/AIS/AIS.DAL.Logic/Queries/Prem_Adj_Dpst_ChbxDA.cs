using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic.Queries
{
    public class Prem_Adj_Dpst_ChbxDA : DataAccessor<PREM_ADJ_DPST_CHBX, Prem_Adj_Dpst_ChbxBE, AISDatabaseLINQDataContext>
    {
        public IList<Prem_Adj_Dpst_ChbxBE> getAdjDpstChbx(int PremAdjPgmSetupID, int PremAdjPgmID, int CstmrID)
        {
            IList<Prem_Adj_Dpst_ChbxBE> resultl = new List<Prem_Adj_Dpst_ChbxBE>();

            IQueryable<Prem_Adj_Dpst_ChbxBE> query = this.BuildQuery();

            if (PremAdjPgmSetupID > 0 || CstmrID > 0 || PremAdjPgmID > 0)
            {
                query = query.Where(PremAdjDpstChbx => PremAdjDpstChbx.prem_adj_pgm_setup_id == PremAdjPgmSetupID);
                query = query.Where(PremAdjDpstChbx => PremAdjDpstChbx.prem_adj_pgm_id == PremAdjPgmID);
                query = query.Where(PremAdjDpstChbx => PremAdjDpstChbx.Cstmr_Id == CstmrID);
                if (query.Count() > 0)
                    resultl = query.ToList();
            }
            else
            {
                resultl = null;
            }

            return resultl;
        }
        private IQueryable<Prem_Adj_Dpst_ChbxBE> BuildQuery()
        {
            if (this.Context == null)
                this.Initialize();
            IQueryable<Prem_Adj_Dpst_ChbxBE> query = from PremAdjDpstchbx in this.Context.PREM_ADJ_DPST_CHBXs
                                                     select new Prem_Adj_Dpst_ChbxBE
                                                     {
                                                               prem_adj_dpst_chbx_id = PremAdjDpstchbx.prem_adj_dpst_chbx_id,
                                                               prem_adj_pgm_setup_id = PremAdjDpstchbx.prem_adj_pgm_setup_id,                                                               
                                                               Cstmr_Id = PremAdjDpstchbx.custmr_id,                                                               
                                                               AdjparameterTypeID = PremAdjDpstchbx.adj_parmet_typ_id,
                                                               dpst_ind = PremAdjDpstchbx.dpst_ind,
                                                               CREATE_DATE = PremAdjDpstchbx.crte_dt,
                                                               CREATE_USER_ID = PremAdjDpstchbx.crte_user_id,
                                                               UPDATE_DATE = PremAdjDpstchbx.updt_dt,
                                                               UPDATE_USER_ID = PremAdjDpstchbx.updt_user_id
                                                     };
            return query;
        }

        //public string getAdjParamResult(int adjPgmSetupID, int programPeriodID, int cstmrID, int adjParamTypeID, bool strDpstInd)
        //{
        //    Prem_Adj_Dpst_ChbxBE result = new Prem_Adj_Dpst_ChbxBE();

        //    IQueryable<Prem_Adj_Dpst_ChbxBE> query = (from aps in this.Context.PREM_ADJ_DPST_CHBXs
        //                                              where aps.prem_adj_pgm_setup_id == adjPgmSetupID && aps.prem_adj_pgm_id == programPeriodID && aps.custmr_id == cstmrID && aps.adj_parmet_typ_id == adjParamTypeID && aps.dpst_ind == strDpstInd
        //                                              select new Prem_Adj_Dpst_ChbxBE
        //                                                    {
        //                                                        prem_adj_pgm_id = aps.prem_adj_pgm_id
        //                                                    });
        //    if (query.Count() > 0)
        //    {
        //        return "true";
        //    }
        //    else
        //        return "false";

        //}
        public int getAdjDpstChbxResultOther(int adjPgmSetupID, int premAdjPgmID, int cstmrID, int? adjParamTypeID)
        {
            Prem_Adj_Dpst_ChbxBE result = new Prem_Adj_Dpst_ChbxBE();

            IQueryable<Prem_Adj_Dpst_ChbxBE> query = (from aps in this.Context.PREM_ADJ_DPST_CHBXs
                                                      where aps.prem_adj_pgm_setup_id == adjPgmSetupID && aps.prem_adj_pgm_id == premAdjPgmID && aps.custmr_id == cstmrID && aps.adj_parmet_typ_id == adjParamTypeID
                                                      select new Prem_Adj_Dpst_ChbxBE
                                                      {
                                                          prem_adj_dpst_chbx_id = aps.prem_adj_dpst_chbx_id,
                                                          //prem_adj_pgm_setup_id = aps.prem_adj_pgm_setup_id
                                                          //prem_adj_pgm_id = aps.prem_adj_pgm_id
                                                      });
            if (query.Count() > 0)
            {
                return query.FirstOrDefault().prem_adj_dpst_chbx_id;
            }
            else
                return 0;

        }
        //public string getAdjDpstChbxResultOther(int adjPgmSetupID, int premAdjPgmID, int cstmrID, int? adjParamTypeID)
        //{
        //    Prem_Adj_Dpst_ChbxBE result = new Prem_Adj_Dpst_ChbxBE();

        //    IQueryable<Prem_Adj_Dpst_ChbxBE> query = (from aps in this.Context.PREM_ADJ_DPST_CHBXs
        //                                              where aps.prem_adj_pgm_setup_id == adjPgmSetupID && aps.prem_adj_pgm_id == premAdjPgmID && aps.custmr_id == cstmrID && aps.adj_parmet_typ_id == adjParamTypeID
        //                                              select new Prem_Adj_Dpst_ChbxBE
        //                                              {
        //                                                  prem_adj_pgm_id = aps.prem_adj_pgm_id
        //                                              });
        //    if (query.Count() > 0)
        //    {
        //        return "true";
        //    }
        //    else
        //        return "false";

        //}
    }
}
