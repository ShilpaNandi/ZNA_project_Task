using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Entities
{
   public class CombinedElementsBE : BusinessEntity<COMB_ELEMT>
    {
       public CombinedElementsBE()
            : base()
        {

        }
# region Properties
        public int COMB_ELEMTS_SETUP_ID
       { get { return Entity.comb_elemts_id; } set { Entity.comb_elemts_id = value; } }
        public int COML_AGMT_ID
        { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int PREM_ADJ_PGM_ID
        { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTMR_ID
        { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? DVSR_NBR_ID
        { get { return Entity.dvsr_nbr_id; } set { Entity.dvsr_nbr_id = value; } }
       
        public int? EXPO_TYP_ID
        { get { return Entity.expo_typ_id; } set { Entity.expo_typ_id = value; } }
        public Boolean? ACTV_IND
        { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public decimal? TOT_AMT
        { get { return Entity.tot_amt; } set { Entity.tot_amt = value; } }
        public decimal? ADJ_RT
        { get { return Entity.adj_rt; } set { Entity.adj_rt = value; } }
        public decimal? AUDIT_EXPO_AMT
        { get { return Entity.audt_expo_amt; } set { Entity.audt_expo_amt = value; } }
        public int? UPDT_USER_ID
        { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDT_DATE
        { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USR_ID
        { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }

       public string PerfectPolicyNumber { get; set; }

       public DateTime CRTE_DT
       {
           get { return Entity.crte_dt; }
           set { Entity.crte_dt = value; }
       }
       public string Policy
       {
           get;
           set;
       }
       public string PERTEXT
       {
           get;
           set;
       }
#endregion

    
        private EntityRef<LookupBE> _ExpoLookup;
        public LookupBE Expolookuptype
        {
            get
            {
                if (EXPO_TYP_ID.HasValue)
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                        new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _ExpoLookup = new EntityRef<LookupBE>(da.Load(EXPO_TYP_ID));
                    return _ExpoLookup.Entity;

                }
                else
                    return null;
            }
        }


        private EntityRef<PolicyBE> _Policy;
        public PolicyBE PerfectPolicynum
        {
            get {
                
                {
                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                        COML_AGMT, PolicyBE, AISDatabaseLINQDataContext> da =
                         new ZurichNA.LSP.Framework.DataAccess.DataAccessor<COML_AGMT, PolicyBE, AISDatabaseLINQDataContext>();
                   _Policy = new EntityRef<PolicyBE >(da.Load(COML_AGMT_ID));
                   return _Policy.Entity;

                
                }
              
            }
        
        }
        private EntityRef<LookupBE> _PerLookup;
        public LookupBE Perlookuptype
        {
            get
            {

                if (DVSR_NBR_ID.HasValue)
                {

                    ZurichNA.LSP.Framework.DataAccess.DataAccessor<
                          LKUP, LookupBE, AISDatabaseLINQDataContext> da =
                          new ZurichNA.LSP.Framework.DataAccess.DataAccessor<LKUP, LookupBE, AISDatabaseLINQDataContext>();
                    _PerLookup = new EntityRef<LookupBE>(da.Load(DVSR_NBR_ID));
                    return _PerLookup.Entity;
                }
                else
                    return null;
            }

        }

        public string EXPOSURETYPE
        {


            get
            {
                if (Expolookuptype == null)
                    return null;
                else
                    return Expolookuptype.LookUpName;


            }
            set { ;}
        }

        public string PER
        {
            get
            {
                if (Perlookuptype == null)
                    return null;
                else
                    return Perlookuptype.LookUpTypeName;
            }
            set { ;}
        }
       
        public string PolicyNumber
        {

            get {

                if (PerfectPolicynum == null)
                    return null;
                else
                    return PerfectPolicynum.PolicyPerfectNumber;  
    
            
                }
            set { ;}
        }
    }
}
