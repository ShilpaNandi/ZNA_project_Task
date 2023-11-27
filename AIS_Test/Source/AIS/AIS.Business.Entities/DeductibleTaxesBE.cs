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
    public class DeductibleTaxesBE : BusinessEntity<DEDTBL_TAX_SETUP>
    {
        public DeductibleTaxesBE()
            : base()
        {
        }
        public int DED_TAXES_SETUP_ID
        {
            get
            {
                return Entity.dedtbl_tax_setup_id;
            }
            set
            {
                Entity.dedtbl_tax_setup_id = value;
            }
        }
        public int LN_OF_BSN_ID
        {
            get
            {
                return Entity.ln_of_bsn_id;
            }
            set
            {
                Entity.ln_of_bsn_id = value;
            }
        }
        private string _lineOfBusiness;
        public string LN_OF_BSN_TXT
        {
            get
            {
                return _lineOfBusiness;
            }
            set
            {
                _lineOfBusiness = value;
            }
        }

        public int ST_ID
        {
            get
            {
                return Entity.st_id;
            }
            set
            {
                Entity.st_id = value;
            }
        }
        private string _stateDescription;
        public string STATEDESCRIPTION
        {
            get
            {
                return _stateDescription;
            }
            set
            {
                _stateDescription = value;
            }
        }
        public int TAX_TYP_ID
        {
            get
            {
                return Entity.tax_typ_id;
            }
            set
            {
                Entity.tax_typ_id = value;
            }
        }
        public decimal? TAX_RATE
        {
            get
            {
                return Entity.tax_rt;
            }
            set
            {
                Entity.tax_rt = value;
            }
        }
        public DateTime? POL_EFF_DT
        {
            get
            {
                return Entity.pol_eff_dt;
            }
            set
            {
                Entity.pol_eff_dt = value;
            }
        }
        public int? DED_TAX_COMPONENT_ID
        {
            get
            {
                return Entity.dedtbl_tax_cmpnt_id;
            }
            set
            {
                Entity.dedtbl_tax_cmpnt_id = value;
            }
        }
        public DateTime? TAX_END_DT
        {
            get
            {
                return Entity.tax_end_dt;
            }
            set
            {
                Entity.tax_end_dt = value;
            }
        }
        public string MAIN_NBR_TXT
        {
            get
            {
                return Entity.main_nbr_txt;
            }
            set
            {
                Entity.main_nbr_txt = value;
            }
        }
        public string SUB_NBR_TXT
        {
            get
            {
                return Entity.sub_nbr_txt;
            }
            set
            {
                Entity.sub_nbr_txt = value;
            }
        }
        public int? UPDT_USER_ID
        {
            get
            {
                return Entity.updt_user_id;
            }
            set
            {
                Entity.updt_user_id = value;
            }
        }
        public DateTime? UPDT_DT
        {
            get
            {
                return Entity.updt_dt;
            }
            set
            {
                Entity.updt_dt = value;
            }
        }
        public int CRTE_USER_ID
        {
            get
            {
                return Entity.crte_user_id;
            }
            set
            {
                Entity.crte_user_id = value;
            }
        }
        public DateTime CRTE_DT
        {
            get
            {
                return Entity.crte_dt;
            }
            set
            {
                Entity.crte_dt = value;
            }
        }
        public Boolean? ACTV_IND
        {
            get
            {
                return Entity.actv_ind;
            }
            set
            {
                Entity.actv_ind = value;
            }
        }

        private string _DeductibleDescription;
        public string DTAXDESCRIPTION
        {
            get
            {
                return _DeductibleDescription;
            }
            set
            {
                _DeductibleDescription = value;
            }
        }
        private string _ComponentDescription;
        public string DTAXCOMDESCRIPTION
        {
            get
            {
                return _ComponentDescription;
            }
            set
            {
                _ComponentDescription = value;
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
