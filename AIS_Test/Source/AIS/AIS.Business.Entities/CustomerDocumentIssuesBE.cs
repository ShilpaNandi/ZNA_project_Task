using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class CustomerDocumentIssuesBE : BusinessEntity<CUSTMR_DOC_ISSUS>
    {
        public CustomerDocumentIssuesBE()
        { }

        public int custmr_doc_issueid
        {
            get { return Entity.custmr_doc_issus_id; }
            set { Entity.custmr_doc_issus_id = value; }
        }
            public int custmr_doc_id
            {
             get{return Entity.custmr_doc_id;}
                set{Entity.custmr_doc_id=value;}

            }
        public int tracking_issue_id
        {
           get{return Entity.traking_issu_id;}
            set{Entity.traking_issu_id=value;}
       }

        public DateTime? updateddate
        {
            get { return Entity.updt_dt; }
            set { Entity.updt_dt = value; }
        }
        public int? updateduserid
        {
            get { return Entity.updt_user_id;}
            set { Entity.updt_user_id = value; }
        }

        public int createduserid
        {
            get { return Entity.crte_user_id; }
            set { Entity.crte_user_id = value; }
        
        }
        public DateTime cretateddate
        {
            get { return Entity.crte_dt; }
            set { Entity.crte_dt = value; }
        
        }

    }
}
