using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Entities
{
    public class SteppedFactorBE : BusinessEntity<STEPPED_FCTR>
    {
        public SteppedFactorBE()
            : base()
        {

        }

        public int STEPPED_FACTOR_ID { get { return Entity.stepped_fctr_id; } set { Entity.stepped_fctr_id = value; } }
        public int? MONTHS_TO_VAL { get { return Entity.mms_to_valn_nbr; } set { Entity.mms_to_valn_nbr = value; } }
        public decimal? LDF_FACTOR { get { return Entity.los_dev_fctr_rt; } set { Entity.los_dev_fctr_rt = value; } }
        public decimal? IBNR_FACTOR { get { return Entity.incur_but_not_rptd_fctr_rt; } set { Entity.incur_but_not_rptd_fctr_rt = value; } }
        public Boolean? ISACTIVE { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public int POLICY_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }

        public bool ACTIVE_CHECK { get { return (Entity.actv_ind == true); } }


        /// <summary>
        /// Checking Validation Errors
        /// </summary>
        protected override void CheckValidationRules()
        {
            if (!this.MONTHS_TO_VAL.HasValue)
                this.ValidationErrors.Add("The Months to valuation is required.");
            if (!this.LDF_FACTOR.HasValue)
                this.ValidationErrors.Add("The LDF Factor is required.");
            if (!this.IBNR_FACTOR.HasValue)
                this.ValidationErrors.Add("The IBNR Factor is required.");
            if (this.POLICY_ID >= 0)
                this.ValidationErrors.Add("The Policy is required.");
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
