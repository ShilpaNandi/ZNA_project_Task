/*-----	Page:	Surcharges Business entity
-----
-----	Created:		CSC (Zakir Hussain)

-----
-----	Description:	Replicaton of SURCHRG_ASSES_SETUP table
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 6/16/2010 (AS part of Surcharges Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
             

*/
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
    public class SurchargesBE : BusinessEntity<SURCHRG_ASSES_SETUP>
    {
        public SurchargesBE()
        {

        }

        public int SURCHARGE_ASSESS_SETUP_ID { get { return Entity.surchrg_asses_setup_id; } set { Entity.surchrg_asses_setup_id = value; } }
        public int ST_ID { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public int? LN_OF_BSN_ID { get { return Entity.ln_of_bsn_id; } set { Entity.ln_of_bsn_id = value; } }
        public int SURCHARGE_TYPE_ID { get { return Entity.surchrg_typ_id; } set { Entity.surchrg_typ_id = value; } }
        public decimal SURCHARGE_RATE
        {
            get
            {
                return Entity.surchrg_rt;
            }
            set
            {
                Entity.surchrg_rt = value;
            }
        }
        public DateTime SURCHARGE_EFF_DT
        {
            get
            {
                return Entity.surchrg_eff_dt;
            }
            set
            {
                Entity.surchrg_eff_dt = value;
            }
        }
        public int SURCHARGE_CODE_ID { get { return Entity.surchrg_cd_id; } set { Entity.surchrg_cd_id = value; } }
        public int? SURCHARGE_FACTOR_ID { get { return Entity.surchrg_fctr_id; } set { Entity.surchrg_fctr_id = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }

        public int? UPDT_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDT_DT { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DT { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public string STATE_SURCHARGE_CODE { get; set; }
        

        private string _StateDescription;
        public string STATEDESCRIPTION
        {
            get
            {
                return _StateDescription;
            }
            set
            {
                _StateDescription = value;
            }
        }
        private string _lnofbsntxt;
        public string LN_OF_BSN_TXT
        {
            get
            {
                return _lnofbsntxt;
            }
            set
            {
                _lnofbsntxt = value;
            }
        }
        private string _SurchargeDescription;
        public string SURCHARGE_DESCRIPTION
        {
            get
            {
                return _SurchargeDescription;
            }
            set
            {
                _SurchargeDescription = value;
            }
        }
        private string _CodeDescription;
        public string SURCHARGE_CODE
        {
            get
            {
                return _CodeDescription;
            }
            set
            {
                _CodeDescription = value;
            }
        }
        
            
        
        
        
        private string _SurchargeFactor;
        public string SURCHARGE_FACTOR
        {
            get
            {
                return _SurchargeFactor;
            }
            set
            {
                _SurchargeFactor = value;
            }
        }
        private int _lookUpId;
        public int LOOKUPID
        {
            get
            {
                return _lookUpId;
            }
            set
            {
                _lookUpId = value;
            }
        }


    }
}