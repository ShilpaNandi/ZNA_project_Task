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
    public class PolicyBE : BusinessEntity<COML_AGMT>
    {

        public PolicyBE()
            : base()
        {
            base.AutoValidate = true;
        }

        public int PolicyID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public string PolicySymbol { get { return Entity.pol_sym_txt; } set { Entity.pol_sym_txt = value; } }
        public string PolicyNumber { get { return Entity.pol_nbr_txt; } set { Entity.pol_nbr_txt = value; } }
        public string PolicyModulus { get { return Entity.pol_modulus_txt; } set { Entity.pol_modulus_txt = value; } }
        public DateTime? PolicyEffectiveDate { get { return Entity.pol_eff_dt; } set { Entity.pol_eff_dt = value; } }
        public DateTime PlanEndDate { get { return Entity.planned_end_date; } set { Entity.planned_end_date = value; } }
        public int? CoverageTypeID { get { return Entity.covg_typ_id; } set { Entity.covg_typ_id = value; } }
        public int? AdjusmentTypeID { get { return Entity.adj_typ_id; } set { Entity.adj_typ_id = value; } }
        public int? ALAETypeID { get { return Entity.aloc_los_adj_exps_typ_id; } set { Entity.aloc_los_adj_exps_typ_id = value; } }
        public int ProgramPeriodID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public decimal? DedTblPolicyLimitAmount { get { return Entity.dedtbl_pol_lim_amt; } set { Entity.dedtbl_pol_lim_amt = value; } }
        public decimal? NonConversionAmount { get { return Entity.nonconv_amt; } set { Entity.nonconv_amt = value; } }
        public decimal? LDFFactor { get { return Entity.los_dev_fctr_rt; } set { Entity.los_dev_fctr_rt = value; } }
        public decimal? IBNRFactor { get { return Entity.incur_but_not_rptd_fctr_rt; } set { Entity.incur_but_not_rptd_fctr_rt = value; } }
        public decimal? ALAECappedAmount { get { return Entity.aloc_los_adj_exps_capped_amt; } set { Entity.aloc_los_adj_exps_capped_amt = value; } }
        public Boolean? UnlimDedtblPolLimitIndicator { get { return Entity.unlim_dedtbl_pol_lim_ind; } set { Entity.unlim_dedtbl_pol_lim_ind = value; } }
        public Boolean? UnlimOverrideDedtblLimitIndicator { get { return Entity.unlim_overrid_dedtbl_lim_ind; } set { Entity.unlim_overrid_dedtbl_lim_ind = value; } }
        public decimal? OverrideDedtblLimitAmount { get { return Entity.overrid_dedtbl_lim_amt; } set { Entity.overrid_dedtbl_lim_amt = value; } }
        public Boolean? LDFIncurredNotReport { get { return Entity.los_dev_fctr_incur_but_not_rptd_incld_lim_ind; } set { Entity.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = value; } }
        public Boolean? LDFIncurredNO63740 { get { return Entity.los_dev_fctr_incur_but_not_rptd_step_ind; } set { Entity.los_dev_fctr_incur_but_not_rptd_step_ind = value; } }
        public decimal? DedtblProtPolMaxAmount { get { return Entity.dedtbl_prot_pol_max_amt; } set { Entity.dedtbl_prot_pol_max_amt = value; } }
        public Boolean? TPAIndicator { get { return Entity.thrd_pty_admin_ind; } set { Entity.thrd_pty_admin_ind = value; } }
        public int? LossSystemSourceID { get { return Entity.los_sys_src_id; } set { Entity.los_sys_src_id = value; } }
        public Boolean? TPADirectIndicator { get { return Entity.thrd_pty_admin_dir_ind; } set { Entity.thrd_pty_admin_dir_ind = value; } }
        public decimal? OtherPolicyAdjustmentAmount { get { return Entity.othr_pol_adj_amt; } set { Entity.othr_pol_adj_amt = value; } }
        public int? DedtblProtPolicyStID { get { return Entity.dedtbl_prot_pol_st_id; } set { Entity.dedtbl_prot_pol_st_id = value; } }
        public Boolean? IsActive { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public int cstmrid { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? ParentPolicyID { get { return Entity.parnt_coml_agmt_id; } set { Entity.parnt_coml_agmt_id = value; } }
        public Boolean? ISMasterPEOPolicy { get { return Entity.mstr_peo_pol_ind; } set { Entity.mstr_peo_pol_ind = value; } }
        

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public string POLICY_EFF_DATE { get; set;}
        public string POLICY_END_DATE { get; set; }
        //public string AdjustmentTypeName { get; set; }
        //public string ALAETypeName { get; set; }
        //public string CoverageTypeName { get; set; }
        //public string LossSourceName { get; set; }
        //public string StatesName { get; set; }


        private EntityRef<LookupBE> _LossSourceTypeLookup;
        /// <summary>
        /// Load Loss Source Type lookup values
        /// </summary>
        public LookupBE LossSourceTypelookup
        {
            get
            {
                if (LossSystemSourceID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _LossSourceTypeLookup = new EntityRef<LookupBE>(da.Load(LossSystemSourceID));
                    return _LossSourceTypeLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string LossSourceName
        {
            get
            {
                if (LossSourceTypelookup == null)
                    return null;
                else
                    return LossSourceTypelookup.LookUpName;
            }
            set { ;}
        }

        private EntityRef<LookupBE> _StateTypeLookup;
        /// <summary>
        /// Load State type lookup values
        /// </summary>
        public LookupBE StateTypelookup
        {
            get
            {
                if (DedtblProtPolicyStID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _StateTypeLookup = new EntityRef<LookupBE>(da.Load(DedtblProtPolicyStID));
                    return _StateTypeLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string StatesName
        {
            get
            {
                if (StateTypelookup == null)
                    return null;
                else
                    return StateTypelookup.LookUpName;
            }
            set { ;}
        }

        private EntityRef<LookupBE> _CoverageTypeLookup;
        /// <summary>
        /// Load Coverage type lookup values
        /// </summary>
        public LookupBE CoverageTypelookup
        {
            get
            {
                if (CoverageTypeID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _CoverageTypeLookup = new EntityRef<LookupBE>(da.Load(CoverageTypeID));
                    return _CoverageTypeLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string CoverageTypeName
        {
            get
            {
                if (CoverageTypelookup == null)
                    return null;
                else
                    return CoverageTypelookup.LookUpName;
            }
            set { ;}
        }


        private EntityRef<LookupBE> _ALAETypeLookup;
        /// <summary>
        /// Load ALAE type lookup values
        /// </summary>
        public LookupBE ALAETypelookup
        {
            get
            {
                if (ALAETypeID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _ALAETypeLookup = new EntityRef<LookupBE>(da.Load(ALAETypeID));
                    return _ALAETypeLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string ALAETypeName
        {
            get
            {
                if (ALAETypelookup == null)
                    return null;
                else
                    return ALAETypelookup.LookUpName;
            }
            set { ;}
        }


        private EntityRef<LookupBE> _AdjustmentTypeLookup;
        /// <summary>
        /// Load Adjustment type lookup values
        /// </summary>
        public LookupBE AdjustmentTypelookup
        {
            get
            {
                if (AdjusmentTypeID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _AdjustmentTypeLookup = new EntityRef<LookupBE>(da.Load(AdjusmentTypeID));
                    return _AdjustmentTypeLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string AdjustmentTypeName
        {
            get
            {
                if (AdjustmentTypelookup == null)
                    return null;
                else
                    return AdjustmentTypelookup.LookUpName;
            }
            set { ;}
        }

        public string PolicyPerfectNumber { get { return Entity.pol_sym_txt.Trim() + " "+ Entity.pol_nbr_txt.Trim() +" "+ Entity.pol_modulus_txt.Trim(); } }

        /// <summary>
        /// Checking Validation Errors
        /// </summary>
        protected override void CheckValidationRules()
        {
            if (string.IsNullOrEmpty(this.PolicySymbol))
                this.ValidationErrors.Add("The Policy Symbol is required.");
            if (string.IsNullOrEmpty(this.PolicyNumber))
                this.ValidationErrors.Add("The Policy Number is required.");
            if (string.IsNullOrEmpty(this.PolicyModulus))
                this.ValidationErrors.Add("The Policy Modulus is required.");
            if (!this.PolicyEffectiveDate.HasValue)
                this.ValidationErrors.Add("The Policy Effective Date is required.");
            if (!this.PlanEndDate.Equals((new DateTime(1900, 1, 1))))
                this.ValidationErrors.Add("The Policy End Date is required.");
            if (!this.CoverageTypeID.HasValue)
                this.ValidationErrors.Add("The Coverage Type is required.");
            if (!this.AdjusmentTypeID.HasValue)
                this.ValidationErrors.Add("Adjustment Type is required.");
            if (!this.ALAETypeID.HasValue)
                this.ValidationErrors.Add("ALAE Type is required");
        }

        /// <summary>
        /// Validates business rules.
        /// </summary>
        /// <returns>Returns true if successful, false otherwise.</returns>
        public override bool Validate()
        {
            this.CheckValidationRules(); //check validation errors

            //if there are validation errors, return false
            if (this.ValidationErrors.Count > 0)
                return false;
            else //otherwise return true
                return true;
        }

    }
}
