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
    public class KYORSetupBS : BusinessServicesBase<KYORSetupBE, KYORSetupDA>
    {
        /// <summary>
        /// Retrieve KY & OR setup information
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public IList<KYORSetupBE> SelectData()
        {
            IList<KYORSetupBE> result = new List<KYORSetupBE>();
            KYORSetupDA kyorDA = new KYORSetupDA();
            try
            {
                result = kyorDA.SelectData();
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return result;
        }

        /// <summary>
        /// Retrieve particular KY & OR setup information record
        /// </summary>
        /// <param name="KOSetupId"></param>
        /// <returns></returns>
        public KYORSetupBE LoadData(int KOSetupId)
        {
            KYORSetupBE Data = new KYORSetupBE();
            try
            {
                Data = DA.Load(KOSetupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Data;
        }

        /// <summary>
        /// Saves or Edits to database using FrameWork
        /// </summary>
        /// <param name="KYORSetupBE"></param>
        /// <returns>true if save, False if failed to save</returns>
        public bool SaveSetupData(KYORSetupBE koBE)
        {
            try
            {
                if (koBE.KY_OR_SETUP_ID > 0)
                {
                    DA.Update(koBE);
                }
                else
                {
                    DA.Add(koBE);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
