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
    public class AdjustmentParameterSetupBE : BusinessEntity<PREM_ADJ_PGM_SETUP>
    {
        public AdjustmentParameterSetupBE()
            : base()
        {

        }
        public int adj_paramet_setup_id { get { return Entity.prem_adj_pgm_setup_id; } set { Entity.prem_adj_pgm_setup_id = value; } }
        public int prem_adj_pgm_id { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public Boolean? incld_ernd_retro_prem_ind { get { return Entity.incld_ernd_retro_prem_ind; } set { Entity.incld_ernd_retro_prem_ind = value; } }
        public decimal? depst_amt { get { return Entity.depst_amt; } set { Entity.depst_amt = value; } }
        public Boolean? actv_ind { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public int? clm_hndl_fee_basis_id { get { return Entity.clm_hndl_fee_basis_id; } set { Entity.clm_hndl_fee_basis_id = value; } }
        public int? lba_Adjustment_typ { get { return Entity.los_base_asses_adj_typ_id; } set { Entity.los_base_asses_adj_typ_id = value; } }
        public Boolean? incld_ibnr_ldf_ind { get { return Entity.incld_incur_but_not_rptd_ind; } set { Entity.incld_incur_but_not_rptd_ind = value; } }
        public int? clm_hnd_fee_basid { get { return Entity.clm_hndl_fee_basis_id; } set { Entity.clm_hndl_fee_basis_id = value; } }
        public decimal? loss_convfact_calimcap { get { return Entity.los_conv_fctr_clm_cap_amt; } set { Entity.los_conv_fctr_clm_cap_amt = value; } }
        public decimal? loss_convfact_aggamt { get { return Entity.los_conv_fctr_aggr_cap_amt; } set { Entity.los_conv_fctr_aggr_cap_amt = value; } }
        public decimal? lay_lossconv_FactInsPay { get { return Entity.los_conv_fctr_lyr_insd_pays_amt; } set { Entity.los_conv_fctr_lyr_insd_pays_amt = value; } }
        public decimal? lay_lossconv_znapayamt { get { return Entity.los_conv_fctr_lyr_zna_pays_amt; } set { Entity.los_conv_fctr_lyr_zna_pays_amt = value; } }

        public int? incur_but_not_rptd_los_dev_fctr_id { get { return Entity.incur_but_not_rptd_los_dev_fctr_id; } set { Entity.incur_but_not_rptd_los_dev_fctr_id = value; } }
        public decimal? incur_los_reim_fund_initl_fund_amt { get { return Entity.incur_los_reim_fund_initl_fund_amt; } set { Entity.incur_los_reim_fund_initl_fund_amt = value; } }
        public Boolean? incur_los_reim_fund_unlim_agmt_lim_ind { get { return Entity.incur_los_reim_fund_unlim_agmt_lim_ind; } set { Entity.incur_los_reim_fund_unlim_agmt_lim_ind = value; } }
        public decimal? incur_los_reim_fund_aggr_lim_amt { get { return Entity.incur_los_reim_fund_aggr_lim_amt; } set { Entity.incur_los_reim_fund_aggr_lim_amt = value; } }
        public Boolean? incur_los_reim_fund_unlim_minimium_lim_ind { get { return Entity.incur_los_reim_fund_unlim_minimium_lim_ind; } set { Entity.incur_los_reim_fund_unlim_minimium_lim_ind = value; } }
        public decimal? incur_los_reim_fund_min_lim_amt { get { return Entity.incur_los_reim_fund_min_lim_amt; } set { Entity.incur_los_reim_fund_min_lim_amt = value; } }
        public Boolean? incur_los_reim_fund_invc_lsi_ind { get { return Entity.incur_los_reim_fund_invc_lsi_ind; } set { Entity.incur_los_reim_fund_invc_lsi_ind = value; } }

        //Added newly as per the work order SR 325928(other amount in ILRF)
        public decimal? incur_los_reim_fund_othr_amt { get { return Entity.incur_los_reim_fund_othr_amt; } set { Entity.incur_los_reim_fund_othr_amt = value; } }

        public int Cstmr_Id { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int? AdjparameterTypeID { get { return Entity.adj_parmet_typ_id; } set { Entity.adj_parmet_typ_id = value; } }
        public int? Escrow_PLMNumber { get { return Entity.escr_paid_los_bil_mms_nbr; } set { Entity.escr_paid_los_bil_mms_nbr = value; } }
        public int? Escrow_Diveser { get { return Entity.escr_dvsr_nbr; } set { Entity.escr_dvsr_nbr = value; } }
        public decimal? Escrow_MnthsHeld { get { return Entity.escr_mms_held_amt; } set { Entity.escr_mms_held_amt = value; } }
        public decimal? Escrow_PrevAmt { get { return Entity.escr_prev_amt; } set { Entity.escr_prev_amt = value; } } 

        public IList<AdjustmentParameterPolicyBE> AdjParametPolBEs { get; set; }
        
        public Boolean HasValue
        {
            get 
            {
                return (Entity != null);
            }
        }

        private EntityRef<LookupBE> _CHFBasisLookup;

        public LookupBE CHFBasisLookup
        {
            get
            {
                if (clm_hndl_fee_basis_id.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _CHFBasisLookup = new EntityRef<LookupBE>(da.Load(clm_hndl_fee_basis_id));
                    return _CHFBasisLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string CHFBasisTypeName
        {
            get
            {
                if (CHFBasisLookup == null)
                    return null;
                else
                    return CHFBasisLookup.LookUpName;
            }
            set { ;}
        }

        private EntityRef<LookupBE> _lbaAdjustmentTypeLookup;
        /// <summary>
        /// Load LBA Adjustment type lookup values
        /// </summary>
        public LookupBE lbaAdjustmentTypelookup
        {
            get
            {
                if (lba_Adjustment_typ.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _lbaAdjustmentTypeLookup = new EntityRef<LookupBE>(da.Load(lba_Adjustment_typ));
                    return _lbaAdjustmentTypeLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string lbaAdjustmentTypeName
        {
            get
            {
                if (lbaAdjustmentTypelookup == null)
                    return null;
                else
                    return lbaAdjustmentTypelookup.LookUpName;
            }
            set { ;}
        }
    }
}
