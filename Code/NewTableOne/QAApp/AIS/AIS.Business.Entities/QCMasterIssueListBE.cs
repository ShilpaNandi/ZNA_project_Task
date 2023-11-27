using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using System.Data.Linq;
namespace ZurichNA.AIS.Business.Entities
{
    public class QCMasterIssueListBE : BusinessEntity<QLTY_CNTRL_MSTR_ISSU_LIST>
    {
        public QCMasterIssueListBE()
            : base()
        {

        }
        public int QualityCntrlMstrIsslstID { get { return Entity.qlty_cntrl_mstr_issu_list_id; } set { Entity.qlty_cntrl_mstr_issu_list_id = value; } }
        public int? IssCatgID { get { return Entity.issu_catg_id; } set { Entity.issu_catg_id = value; } }
        public int? Str_Nbr { get { return Entity.srt_nbr; } set { Entity.srt_nbr = value; } }
        public string IssueText { get { return Entity.issu_txt; } set { Entity.issu_txt = value; } }
        public bool? FinancialIndicator { get { return Entity.finc_ind; } set { Entity.finc_ind = value; } }
        public Boolean? ACTIVE { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UpdatedUserID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CreatedUserID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }

        public string IssueCategory { get; set; } 
    }
}
