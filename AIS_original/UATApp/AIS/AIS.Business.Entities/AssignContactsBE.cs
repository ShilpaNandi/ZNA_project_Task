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
    public class AssignContactsBE : BusinessEntity<PREM_ADJ_PGM_PERS_REL>
    {
        public AssignContactsBE()
            : base()
        {

        }

        public int PREM_ADJ_PGM_PERS_REL_ID { get { return Entity.prem_adj_pgm_pers_rel_id; } set { Entity.prem_adj_pgm_pers_rel_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int PERS_ID { get { return Entity.pers_id; } set { Entity.pers_id = value; } }
        public Boolean? SND_INVC_IND { get { return Entity.snd_invc_ind; } set { Entity.snd_invc_ind = value; } }
        public int COMMU_MEDUM_ID { get { return Entity.commu_medum_id; } set { Entity.commu_medum_id = value; } }
        public DateTime? UPDT_DT { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDT_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DT { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        private string _FirstName;
        public string FirstName { get { return _FirstName; } set { _FirstName = value; } }
        private string _LastName;
        public string LastName { get { return _LastName; } set { _LastName = value; } }
        public string _SND_INVC_IND_txt;

        public string SND_INVC_IND_txt { get { if (SND_INVC_IND == true) { return "YES"; } else { return "NO"; } } set { _SND_INVC_IND_txt = value; } }
        private string _ComTypText;
        public string ComTypText { get { return _ComTypText; } set { _ComTypText = value; } }
        private string _FullName;
        public string FullName { get { return (LastName + ", " + FirstName); } set { _FullName = value; } }
        private int? _ContTypID;
        public int? ContTypID { get { return _ContTypID; } set { _ContTypID = value; } }
        private string _ContTyp;
        public string ContTyp { get { return _ContTyp; } set { _ContTyp = value; } }
    }
}
