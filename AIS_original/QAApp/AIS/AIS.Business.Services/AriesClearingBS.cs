using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;

namespace ZurichNA.AIS.Business.Logic
{
    public class AriesClearingBS :BusinessServicesBase<AriesClearingBE,AriesClearingDA>
    {
/// <summary>
/// This method is used to add or update the aries clearing details.
/// </summary>
/// <param name="ariesclrgBE"></param>
/// <returns></returns>
        public bool save(AriesClearingBE ariesclrgBE)
        {
            bool succeed = false;
            try
            {

                if (ariesclrgBE.PREMIUM_ADJUST_CLEARING_ID > 0)
                {
                    ariesclrgBE.UPDATED_DATE = DateTime.Now;
                    succeed = this.DA.Update(ariesclrgBE);
                }
                else
                {
                    ariesclrgBE.CREATED_DATE = DateTime.Now;
                    succeed = this.DA.Add(ariesclrgBE);
                }
                    return succeed;
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

        }
/// <summary>
/// This method is used to fill the aries details list.
/// </summary>
/// <param name="Premadjustid"></param>
/// <param name="Customerid"></param>
/// <returns></returns>
        public IList<AriesClearingBE> GetAriesDetails(int Premadjustid, int Customerid)
        {
            IList<AriesClearingBE> lstarieslist;
            lstarieslist = new AriesClearingDA().GetAriesClearingDetails(Premadjustid, Customerid);
            return lstarieslist;

        
        }
        public AriesClearingBE GetAriesDetails(int Ariesid)
        {
            AriesClearingBE ariesBE = new AriesClearingBE();
            ariesBE = DA.Load(Ariesid);
            return ariesBE;
        }

    }
}
