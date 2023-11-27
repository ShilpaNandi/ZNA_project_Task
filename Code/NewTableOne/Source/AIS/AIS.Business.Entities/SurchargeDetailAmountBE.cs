/*
             File Name          : SurchargeDetailAmountBE.cs
 *           Description        : code having logic to handle events and bussisness logic for surcharge
 *                                Assesment
 *           Author             : Phani Neralla
 *           Team Name          : FinSol/AIS
 *           Creation Date      : 18-Jun-2010
 *           Last Modified By   : 
 *           Last Modified Date :
*/
#region SurchargeDetailsAmountBE Class
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
{/*
    public class SurchargeDetailAmountBE:BusinessEntity<PREM_ADJ_SURCHRG_DTL_AMT>
    {
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
        public decimal? other_surchrg_amt
        { 
            get { return Entity.other_surchrg_amt; }
            set { Entity.other_surchrg_amt = value; }
        }
    }*/
}
#endregion
#endregion
