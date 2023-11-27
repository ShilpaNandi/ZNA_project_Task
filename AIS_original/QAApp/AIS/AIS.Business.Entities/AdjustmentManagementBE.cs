using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class AdjustmentManagementBE: BusinessEntity<PREM_ADJ> 
    {
        public int prem_adjID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int? Rel_prem_adjID { get { return Entity.rel_prem_adj_id; } set { Entity.rel_prem_adj_id = value; } }
///        public string PREM_NON_PREM_CODE { get { return Entity.prem_non_prem_cd; } set { Entity.prem_non_prem_cd = value; } }
        public int custmrID { get { return Entity.reg_custmr_id; } set { Entity.reg_custmr_id = value; } }
        public DateTime ValtnDate { get { return Entity.valn_dt; } set { Entity.valn_dt = value; } }
        public string DrftInvoicenmr { get { return Entity.drft_invc_nbr_txt; } set { Entity.drft_invc_nbr_txt = value; } }
        public string FinalInvoicenmr { get { return Entity.fnl_invc_nbr_txt; } set { Entity.fnl_invc_nbr_txt = value; } }
        public DateTime? DrftInvoiceDate { get { return Entity.drft_invc_dt; } set { Entity.drft_invc_dt = value; } }
        public DateTime? FinalInvoiceDate { get { return Entity.fnl_invc_dt; } set { Entity.fnl_invc_dt = value; } }
        public Boolean? AdjPendingIndctor { get { return Entity.adj_pendg_ind; } set { Entity.adj_pendg_ind = value; } }
        public int? AdjPendingRsnID { get { return Entity.adj_pendg_rsn_id; } set { Entity.adj_pendg_rsn_id = value; } }
        public Boolean? twntyqtrlind { get { return Entity.twenty_pct_qlty_cntrl_ind; } set { Entity.twenty_pct_qlty_cntrl_ind = value; } }
        public Boolean? TwentyReqIND { get { return Entity.twenty_pct_qlty_cntrl_reqr_ind; } set { Entity.twenty_pct_qlty_cntrl_reqr_ind = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public Boolean? ReviseReasonIndc { get { return Entity.adj_rrsn_ind; } set { Entity.adj_rrsn_ind = value; } } 
        public int? ReviseReasonID { get { return Entity.adj_rrsn_rsn_id; } set { Entity.adj_rrsn_rsn_id = value; } }
        public Boolean? VoidReasonIndc { get { return Entity.adj_void_ind; } set { Entity.adj_void_ind = value; } } 
        public int? VoidReasonID { get { return Entity.adj_void_rsn_id; } set { Entity.adj_void_rsn_id = value; } } 
        public string Adjuststatus { get; set; }
        public int? ADJ_STATUS_TYP_ID { get; set; }
        public int? AdjMgmtStatusNumber { get; set; }
        public int? PREM_ADJ_PGM_ID { get; set; }
        public int? BROKER_ID { get { return Entity.brkr_id; } set { Entity.brkr_id = value; } }
        public int? BU_OFF_ID { get { return Entity.bu_office_id; } set { Entity.bu_office_id = value; } }
        
        public Boolean? HistoricalIndc { get { return Entity.historical_adj_ind; } set { Entity.historical_adj_ind= value; } }
        
        //public IList<PremiumAdjustmentStatusBE> PremAdjStatsBE { get; set; }
        
        private EntityRef<AccountBE> _CustomerName;

        public AccountBE CustomerName
        {
            get
            {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        CUSTMR, AccountBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<CUSTMR, AccountBE, AISDatabaseLINQDataContext>();
                    _CustomerName = new EntityRef<AccountBE>(da.Load(custmrID));
                    return _CustomerName.Entity;
             
            }
        }

        public string CustomerFullName
        {
            get
            {
                if (CustomerName == null)
                    return null;
                else
                    return CustomerName.FULL_NM; 
            }
            set { ;}
        }

        private EntityRef<LookupBE> _PendingLookup;

        
        public LookupBE PendingLookup
        {
            get
            {
                if (AdjPendingRsnID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _PendingLookup = new EntityRef<LookupBE>(da.Load(AdjPendingRsnID));
                    return _PendingLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string PendingTypeName
        {
            get
            {
                if (PendingLookup == null)
                    return null;
                else
                    return PendingLookup.LookUpName;
            }
            set { ;}
        }


        
    }
}
