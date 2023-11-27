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
    public class AdjustmentParameterDetailBE : BusinessEntity<PREM_ADJ_PGM_DTL>
    {

        public int prem_adj_pgm_dtl_id { get { return Entity.prem_adj_pgm_dtl_id; } set { Entity.prem_adj_pgm_dtl_id = value; } }
        public int? st_id { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public int? clm_hndl_fee_los_typ_id { get { return Entity.clm_hndl_fee_los_typ_id; } set { Entity.clm_hndl_fee_los_typ_id = value; } }
        public decimal? CHF_CLMT_NUMBER { get { return Entity.clm_hndl_fee_clmt_nbr; } set { Entity.clm_hndl_fee_clmt_nbr = value; } }
        public int adj_paramet_id
        {
            get { return Entity.prem_adj_pgm_setup_id; }
            set { Entity.prem_adj_pgm_setup_id = value; }
        }
        public int PrgmPerodID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int AccountID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public decimal? adj_fctr_rt { get { return Entity.adj_fctr_rt ; } set { Entity.adj_fctr_rt = value; } }
        public string cmmnt_txt { get { return Entity.cmmnt_txt; } set { Entity.cmmnt_txt = value; } }
        public decimal? fnl_overrid_amt { get { return Entity.fnl_overrid_amt; } set { Entity.fnl_overrid_amt = value; } }
        public int? ln_of_bsn_id { get { return Entity.ln_of_bsn_id; } set { Entity.ln_of_bsn_id = value; } }
        public int? UPDTE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDTE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public Boolean? act_ind { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public decimal? PremAssementAmt { get { return Entity.prem_asses_rt; } set { Entity.prem_asses_rt = value; } }
        public decimal? Clm_hndlfee_clmrate { get { return Entity.clm_hndl_fee_clm_rt_nbr; } set { Entity.clm_hndl_fee_clm_rt_nbr = value; } }
        public int? prem_adj_pgm_setup_pol_id { get { return Entity.prem_adj_pgm_setup_pol_id; } set { Entity.prem_adj_pgm_setup_pol_id = value; } }

        private EntityRef<LookupBE> _PrgParameterStateLookupType;
        /// <summary>
        /// Load Program Parameter States lookup 
        /// </summary>
        public LookupBE PrgParameterStateLookupType
        {
            get 
            {
                if (st_id.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                    LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                    new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _PrgParameterStateLookupType = new EntityRef<LookupBE>(da.Load(st_id));
                    return _PrgParameterStateLookupType.Entity;

                }
                else
                    return null;
            }
        }
    
        public string PrgParameterStateName
        {
           get
           {
              if (PrgParameterStateLookupType == null)
                 return null;
              else
                 return PrgParameterStateLookupType.Attribute1; 
              
           }
           set{ ;}
      
        }

        public string PrgparameterStateFullname
        {
            get
            {
                if (PrgParameterStateLookupType == null)
                    return null;
                else
                    return PrgParameterStateLookupType.LookUpName;

            }
            set { ;}
        
        }


        private EntityRef<LookupBE> _PrgParameterLOBLookupType;
        /// <summary>
        /// Load Program parameter Line Of Business lookup values
        /// </summary>
        public LookupBE PrgParameterLOBLookupType
        {
            get
            {
                if (ln_of_bsn_id.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                    LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                    new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _PrgParameterLOBLookupType = new EntityRef<LookupBE>(da.Load(ln_of_bsn_id));
                    return _PrgParameterLOBLookupType.Entity;

                }
                else
                    return null;
            }
        }

        public string PrgParameterLOBName
        {
            get
            {
                if (PrgParameterLOBLookupType == null)
                    return null;
                else
                    return PrgParameterLOBLookupType.LookUpName;

            }
            set { ;}

        }

        private EntityRef<LookupBE> _PrgParameterCHFLLookupType;
        /// <summary>
        /// Load Program parameter Claims handeling Fees lookup values
        /// </summary>
        public LookupBE PrgParameterCHFLLookupType
        {
            get
            {
                if (clm_hndl_fee_los_typ_id.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                    LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                    new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _PrgParameterCHFLLookupType = new EntityRef<LookupBE>(da.Load(clm_hndl_fee_los_typ_id));
                    return _PrgParameterCHFLLookupType.Entity;

                }
                else
                    return null;
            }
        }

        public string PrgParameterCHFLName
        {
            get
            {
                if (PrgParameterCHFLLookupType == null)
                    return null;
                else
                    return PrgParameterCHFLLookupType.LookUpName;

            }
            set { ;}

        }
    }
}