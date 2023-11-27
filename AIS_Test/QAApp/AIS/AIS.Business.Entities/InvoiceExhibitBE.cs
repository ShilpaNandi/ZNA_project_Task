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
    public class InvoiceExhibitBE : BusinessEntity<INVC_EXHIBIT_SETUP>
    {
        public InvoiceExhibitBE()
            : base()
        {

        }
        public int INVC_EXHIBIT_SETUP_ID { get { return Entity.invc_exhibit_setup_id; } set { Entity.invc_exhibit_setup_id = value; } }
        public string ATCH_CD { get { return Entity.atch_cd; } set { Entity.atch_cd = value; } }
        public string ATCH_NM { get { return Entity.atch_nm; } set { Entity.atch_nm = value; } }
        public Boolean? STS_IND { get { return Entity.sts_ind; } set { Entity.sts_ind = value; } }
        public int? SEQ_NBR { get { return Entity.seq_nbr; } set { Entity.seq_nbr = value; } }
        public char? INTRNL_FLAG_IND { get { return Entity.intrnl_flag_cd; } set { Entity.intrnl_flag_cd = value; } }
        public Boolean? CESAR_CD_IND { get { return Entity.cesar_cd_ind; } set { Entity.cesar_cd_ind = value; } }
        public int CREATEUSER { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int? UPDATEDUSER { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CREATEDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? UPDATEDDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }


    }
}
