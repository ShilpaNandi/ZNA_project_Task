using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;
namespace ZurichNA.AIS.Business.Logic
{
   public class NonAisCustomerBS:BusinessServicesBase<NonAisCustomerBE,NonAisCusotmerDA>
    {
/// <summary>
/// This method is used to get the non ais customer list.
/// </summary>
/// <returns></returns>
       public IList<NonAisCustomerBE> getNonaisCustomerlist(string[] query)

       {
           IList<NonAisCustomerBE> lstNonaiscustomer = new List<NonAisCustomerBE>();
           NonAisCusotmerDA objNonaiscustomer = new NonAisCusotmerDA();

           try
           {
               lstNonaiscustomer = objNonaiscustomer.getNonaisCustomerlist(query);
               NonAisCustomerBE nonaisBE = new NonAisCustomerBE();
              // nonaisBE.Nonaiscustmrid = 0;
             //  nonaisBE.fullname = "(Select)";
              // lstNonaiscustomer.Insert(0, nonaisBE);
           }

           catch (System.Data.SqlClient.SqlException ex)
           {
               RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
               throw myException;
           }
           return (lstNonaiscustomer);
       }
       
       
       /// <summary>
       /// This method is used to save or update the nonais customer details.
       /// </summary>
       /// <param name="nonaicustomerBE"></param>
       /// <returns></returns>
       public bool save(NonAisCustomerBE  nonaicustomerBE)
       {
           bool succeed = false;
           try
           {


              
                   succeed = this.DA.Add(nonaicustomerBE);
              


               return succeed;
           }
           catch (Exception ex)
           {
               RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
               throw myException;
           }

       }


     
    }
}
