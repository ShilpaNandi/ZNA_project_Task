using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using log4net;

namespace ZurichNA.AIS.Business.Entities
{
    public class LossInfoBE : BusinessEntity<ARMIS_LOS_POL>
    {
        public LossInfoBE()
            : base()
        {

        }

        public int ARMIS_LOS_ID { get { return Entity.armis_los_pol_id; } set { Entity.armis_los_pol_id = value; } }
        public int COML_AGMT_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int ST_ID { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public DateTime? VALN_DATE { get { return Entity.valn_dt; } set { Entity.valn_dt = value; } }
        public string SUPRT_SERV_CUSTMR_GP_ID { get { return Entity.suprt_serv_custmr_gp_id; } set { Entity.suprt_serv_custmr_gp_id = value; } }
        public Decimal? PAID_IDNMTY_AMT { get { return Entity.paid_idnmty_amt; } set { Entity.paid_idnmty_amt = value; } }
        public Decimal? PAID_EXPS_AMT { get { return Entity.paid_exps_amt; } set { Entity.paid_exps_amt = value; } }
        public Decimal? RESRV_IDNMTY_AMT { get { return Entity.resrv_idnmty_amt; } set { Entity.resrv_idnmty_amt = value; } }
        public Decimal? RESRV_EXPS_AMT { get { return Entity.resrv_exps_amt; } set { Entity.resrv_exps_amt = value; } }
        public Decimal? NON_BILABL_PAID_IDNMTY_AMT { get { return Entity.non_bilabl_paid_idnmty_amt; } set { Entity.non_bilabl_paid_idnmty_amt = value; } }
        public Decimal? NON_BILABL_PAID_EXPS_AMT { get { return Entity.non_bilabl_paid_exps_amt; } set { Entity.non_bilabl_paid_exps_amt = value; } }
        public Decimal? NON_BILABL_RESRV_IDNMTY_AMT { get { return Entity.non_bilabl_resrv_idnmty_amt; } set { Entity.non_bilabl_resrv_idnmty_amt = value; } }
        public Decimal? NON_BILABL_RESRV_EXPS_AMT { get { return Entity.non_bilabl_resrv_exps_amt; } set { Entity.non_bilabl_resrv_exps_amt = value; } }
        public Decimal? SUBJ_PAID_IDNMTY_AMT { get { return Entity.subj_paid_idnmty_amt; } set { Entity.subj_paid_idnmty_amt = value; } }
        public Decimal? SUBJ_PAID_EXPS_AMT { get { return Entity.subj_paid_exps_amt; } set { Entity.subj_paid_exps_amt = value; } }
        public Decimal? SUBJ_RESRV_IDNMTY_AMT { get { return Entity.subj_resrv_idnmty_amt; } set { Entity.subj_resrv_idnmty_amt = value; } }
        public Decimal? SUBJ_RESRV_EXPS_AMT { get { return Entity.subj_resrv_exps_amt; } set { Entity.subj_resrv_exps_amt = value; } }
        public Decimal? EXC_PAID_IDNMTY_AMT { get { return Entity.exc_paid_idnmty_amt; } set { Entity.exc_paid_idnmty_amt = value; } }
        public Decimal? EXC_PAID_EXPS_AMT { get { return Entity.exc_paid_exps_amt; } set { Entity.exc_paid_exps_amt = value; } }
        public Decimal? EXC_RESRV_IDNMTY_AMT { get { return Entity.exc_resrvd_idnmty_amt; } set { Entity.exc_resrvd_idnmty_amt = value; } }
        public Decimal? EXC_RESRV_EXPS_AMT { get { return Entity.exc_resrv_exps_amt; } set { Entity.exc_resrv_exps_amt = value; } }
        public Boolean? SYS_GENRT_IND { get { return Entity.sys_genrt_ind??false; } set { Entity.sys_genrt_ind = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int? PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public string POLICYSYMBOL { get; set; }
        public string POLICY { get; set; }
        public string ADJ_STATUS { get; set; }
        public string EXC_NON_BIL { get; set; }
        public Decimal? Incurred { get; set; }
        public int CREATEUSER { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int? UPDATEDUSER { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CREATEDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? UPDATEDDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public Decimal? TOTAL_INCURRED { get; set; }
        public Decimal? NON_BILABL_INCURRED { get; set; }
        public Decimal? SUBJ_INCURRED { get; set; }
        public Decimal? EXC_INCURRED { get; set; }
        public string ALAE_TYP { get; set; }
        public Decimal? POLICY_AMT { get; set; }
        public Decimal? LIMIT2_AMT { get; set; }
        public int? COVG_ID { get; set; }
        public DateTime? PGM_PRD_STRT_DT { get; set; }
        public DateTime? PGM_PRD_END_DT { get; set; }
        public int? PGM_TYP_ID { get; set; }
        public DateTime? POL_STRT_DT { get; set; }
        public DateTime? POL_END_DT { get; set; }
        public Boolean? COPY_IND { get { return Entity.copy_ind ?? false; } set { Entity.copy_ind = value; } }

        private EntityRef<LookupBE> _StateLookup;
        public LookupBE Statelookuptype
        {
            get
            {
                if (ST_ID > 0)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _StateLookup = new EntityRef<LookupBE>(da.Load(ST_ID));
                    return _StateLookup.Entity;

                }
                else
                    return null;
            }
        }

        private EntityRef<LookupBE> _LOBLookup;
        public LookupBE Loblookuptype
        {
            get
            {
                if (COML_AGMT_ID > 0)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _LOBLookup = new EntityRef<LookupBE>(da.Load(COML_AGMT_ID));
                    return _LOBLookup.Entity;

                }
                else
                    return null;
            }
        }

        private EntityRef<PolicyBE> _Policy;
        public PolicyBE PerfectPolicynum
        {
            get
            {

                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        COML_AGMT, PolicyBE, AISDatabaseLINQDataContext> da =
                         new ZurichNA.LSP.Framework.DataAccess.DataAccessor<COML_AGMT, PolicyBE, AISDatabaseLINQDataContext>();
                    _Policy = new EntityRef<PolicyBE>(da.Load(COML_AGMT_ID));
                    return _Policy.Entity;


                }

            }

        }

        private EntityRef<LookupBE> _PgmType;
        public LookupBE PgmTypeLookup
        {
            get
            {
                if (PGM_TYP_ID > 0)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _StateLookup = new EntityRef<LookupBE>(da.Load(PGM_TYP_ID));
                    return _StateLookup.Entity;

                }
                else
                    return null;
            }
        }

        public string STATETYPE
        {


            get
            {
                if (Statelookuptype == null)
                    return null;
                else
                    return Statelookuptype.Attribute1;


            }
            set { ;}
        }

        public string LOBTYPE
        {


            get
            {
                if (Loblookuptype == null)
                    return null;
                else
                    return Loblookuptype.LookUpName;


            }
            set { ;}
        }
        public string PolicyNumber
        {

            get
            {

                if (PerfectPolicynum == null)
                    return null;
                else
                    return PerfectPolicynum.PolicyPerfectNumber;


            }
            set { ;}
        }

        public string PGMTYPE
        {


            get
            {
                if (PgmTypeLookup == null)
                    return null;
                else
                    return PgmTypeLookup.Attribute1;


            }
            set { ;}
        }
    }
}
