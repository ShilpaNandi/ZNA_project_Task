/*
             File Name          : SurchargeDetailsBE.cs
 *           Description        : code having logic to handle events and bussisness logic for surcharge
 *                                Assesment
 *           Author             : Phani Neralla
 *           Team Name          : FinSol/AIS
 *           Creation Date      : 18-Jun-2010
 *           Last Modified By   : 
 *           Last Modified Date :
*/

#region SurchargeDetailsBE Class
#region namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
//AIS Specfic custom namespaces
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using log4net;
#endregion

#region Class code
namespace ZurichNA.AIS.Business.Entities
{
    /// <summary>
    /// This class contains the identical copy of the fields in the database.
    /// This is mapped to PREM_ADJ_SURCHRG_DTL table in our database
    /// </summary>
    public class SurchargeDetailsBE : BusinessEntity<PREM_ADJ_SURCHRG_DTL>
    {
       
        
        
        /// <summary>
        /// default constructor in the class.
        /// this is will inturn call the base constructor
        /// </summary>
        public SurchargeDetailsBE()
            : base()
        {
        }

        public int prem_adj_surchrg_dtl_id
        {
            get { return Entity.prem_adj_surchrg_dtl_id; }
            set { Entity.prem_adj_surchrg_dtl_id = value; }
        }
        public int prem_adj_perd_id
        {
            get { return Entity.prem_adj_perd_id; }
            set { Entity.prem_adj_perd_id = value; }
        }
        public int prem_adj_id
        {
            get { return Entity.prem_adj_id; }
            set { Entity.prem_adj_id= value; }
        }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int coml_agmt_id
        {
            get { return Entity.coml_agmt_id; }
            set { Entity.coml_agmt_id = value; }
        }
        public int PrgmPerodID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int? st_id { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public int? ln_of_bsn_id { get { return Entity.ln_of_bsn_id; } set { Entity.ln_of_bsn_id = value; } }
        public int? surchrg_cd_id { get { return Entity.surchrg_cd_id; } set { Entity.surchrg_cd_id = value; } }
        public int? surchrg_type_id { get { return Entity.surchrg_typ_id; } set { Entity.surchrg_typ_id = value; } }
        public int? UPDTE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDTE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public decimal? subj_paid_idnmty_amt { 
            get { return Entity.subj_paid_idnmty_amt; }
            set { Entity.subj_paid_idnmty_amt = value; }
        }
        public decimal? subj_paid_exps_amt
        {
            get { return Entity.subj_paid_exps_amt; }
            set { Entity.subj_paid_exps_amt = value; }
        }
        public decimal? subj_resrv_idnmty_amt
        {
            get { return Entity.subj_resrv_idnmty_amt; }
            set { Entity.subj_resrv_idnmty_amt = value; }
        }
        public decimal? subj_resrv_exps_amt
        {
            get { return Entity.subj_resrv_exps_amt; }
            set { Entity.subj_resrv_exps_amt = value; }
        }
        public decimal? basic_amt
        {
            get { return Entity.basic_amt; }
            set { Entity.basic_amt = value; }
        }
        public decimal? std_subj_prem_amt
        {
            get { return Entity.std_subj_prem_amt; }
            set { Entity.std_subj_prem_amt = value; }
        }
        public decimal? tot_surchrg_asses_base
        {
            get { return Entity.tot_surchrg_asses_base; }
            set { Entity.tot_surchrg_asses_base = value; }
        }
        public decimal? surchrg_rt { get { return Entity.surchrg_rt; } set { Entity.surchrg_rt = value; } }
        public decimal? addn_rtn
        {
            get { return Entity.addn_rtn; }
            set { Entity.addn_rtn = value; }
        }
        public decimal? ernd_retro_prem_amt
        {
            get { return Entity.ernd_retro_prem_amt; }
            set { Entity.ernd_retro_prem_amt = value; }
        }
        //public decimal? biled_ernd_retro_prem_amt
        //{
        //    get { return Entity.biled_ernd_retro_prem_amt; }
        //    set { Entity.biled_ernd_retro_prem_amt = value; }
        //}
        public decimal? prev_biled_ernd_retro_prem_amt
        {
            get { return Entity.prev_biled_ernd_retro_prem_amt; }
            set { Entity.prev_biled_ernd_retro_prem_amt = value; }
        }
        //public decimal? prev_std_subj_prem_amt
        //{
        //    get { return Entity.prev_std_subj_prem_amt; }
        //    set { Entity.prev_std_subj_prem_amt = value; }
        //}
        public decimal? retro_rslt
        {
            get { return Entity.retro_rslt; }
            set { Entity.retro_rslt = value; }
        }
        public decimal? addn_surchrg_asses_cmpnt
        {
            get { return Entity.addn_surchrg_asses_cmpnt; }
            set { Entity.addn_surchrg_asses_cmpnt = value; }
        }
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
            set { ;}

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
        public string pol_name
        {
            get;
            set;
        }
    }

    /// <summary>
    /// /// This class contains the identical copy of the fields in the database.
    /// This is mapped to PREM_ADJ_SURCHRG_DTL_AMT table in our database
    /// </summary>
    public class SurchargeDetailAmountBE : BusinessEntity<PREM_ADJ_SURCHRG_DTL_AMT>
    {

        private EntityRef<PREM_ADJ_SURCHRG_DTL_AMT> _SurDTL;
        /// <summary>
        /// Lazy loaded broker of record business entity.
        /// </summary>
        public PREM_ADJ_SURCHRG_DTL_AMT SurDTL
        {
            get
            {
                if (prem_adj_surchrg_dtl_id > null)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        PREM_ADJ_SURCHRG_DTL_AMT, SurchargeDetailAmountBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<PREM_ADJ_SURCHRG_DTL_AMT, SurchargeDetailAmountBE, AISDatabaseLINQDataContext>();
                    _SurDTL = new EntityRef<PREM_ADJ_SURCHRG_DTL_AMT>();
                    return _SurDTL.Entity;
                }
                else
                    return null;
            }
        }
        /// <summary>
        /// default constructor in the class.
        /// this is will inturn call the base constructor
        /// </summary>
        public SurchargeDetailAmountBE()
            : base()
        {
        }
        public int prem_adj_surchrg_dtl_id
        {
            get { return Entity.prem_adj_surchrg_dtl_amt_id; }
            set { Entity.prem_adj_surchrg_dtl_amt_id = value; }
        }
        public int prem_adj_perd_id
        {
            get { return Entity.prem_adj_perd_id; }
            set { Entity.prem_adj_perd_id = value; }
        }
        public int prem_adj_id
        {
            get { return Entity.prem_adj_id; }
            set { Entity.prem_adj_id = value; }
        }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int coml_agmt_id
        {
            get { return Entity.coml_agmt_id; }
            set { Entity.coml_agmt_id = value; }
        }
        public int PrgmPerodID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int? st_id { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public int? ln_of_bsn_id { get { return Entity.ln_of_bsn_id; } set { Entity.ln_of_bsn_id = value; } }
        public int? surchrg_cd_id { get { return Entity.surchrg_cd_id; } set { Entity.surchrg_cd_id = value; } }
        public int? surchrg_type_id { get { return Entity.surchrg_typ_id; } set { Entity.surchrg_typ_id = value; } }
        public int? UPDTE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDTE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public decimal? other_surchrg_amt
        {
            get { return Entity.other_surchrg_amt; }
            set { Entity.other_surchrg_amt = value; }
        }
        public string pol_name
        {
            get;
            set;
        }
        public decimal? surchrg_rt { get; set; }
        public decimal? tot_surchrg_asses_base
        {
            get;
            set;
        }
        public string ProgPeriodCmmnts { get; set; }
        public string Surcharge_Desc { get; set; }
        public string Surcharge_Code { get; set; }
        public decimal? tot_addn_rtn { get; set; }
       
    }
}
#endregion

#endregion