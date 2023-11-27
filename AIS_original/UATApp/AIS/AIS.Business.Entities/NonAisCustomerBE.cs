using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
  public   class NonAisCustomerBE : BusinessEntity<NONAIS_CUSTMR>
    {

        public NonAisCustomerBE()
            : base()
        {

        }
        public int Nonaiscustmrid
        { get { return Entity.nonais_custmr_id; } set { Entity.nonais_custmr_id = value; } }
        public string fullname
        { get { return Entity.full_nm; } set { Entity.full_nm = value; } }
        public int? Updateduserid
        {
            get { return Entity.updt_user_id; }
            set{Entity.updt_user_id=value;}
        }
        public DateTime? Updateddate
        {
            get { return Entity.updt_dt; }
            set{Entity.updt_dt=value;}
        }

        public int Creteduserid
        {
            get { return Entity.crte_user_id; }
            set { Entity.crte_user_id = value; }
        }

        public DateTime Createddate
        {
            get { return Entity.crte_dt; }
            set { Entity.crte_dt = value; }
        }
        
       
        
    }
}
