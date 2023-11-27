using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.Logic;


namespace ZurichNA.AIS.DAL.Logic
{
  public  class NonAisCusotmerDA : DataAccessor<NONAIS_CUSTMR,NonAisCustomerBE,AISDatabaseLINQDataContext>
    {
        
      /// <summary>
      /// This method returns the non ais customerlist.
      /// </summary>
      /// <returns></returns>
      public IList<NonAisCustomerBE> getNonaisCustomerlist(string[] RangeSearch)
        {
            IList<NonAisCustomerBE> result = new List<NonAisCustomerBE>();

            if (this.Context == null)
                this.Initialize();
            var predicate = PredicateBuilder.False<NonAisCustomerBE>();
            IQueryable<NonAisCustomerBE> query =
                       (from cus in this.Context.NONAIS_CUSTMRs
                        select new NonAisCustomerBE()
                        {
                             Nonaiscustmrid=cus.nonais_custmr_id,
                             fullname=cus.full_nm
                        });

            foreach (string searchTerm in RangeSearch)
            {
                string temp = searchTerm;
                predicate = predicate.Or(p => p.fullname.StartsWith(temp));
            }
            query = query.Where(predicate);
            query = query.OrderBy(cdd => cdd.fullname);
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

       
    }
}
