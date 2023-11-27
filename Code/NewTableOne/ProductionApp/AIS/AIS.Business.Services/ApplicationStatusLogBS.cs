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
    public class ApplicationStatusLogBS : BusinessServicesBase<ApplicationStatusLogBE, ApplicationStatusLogDA>
    {
        public ApplicationStatusLogBS()
        {

        }
        public void WriteLog(int intPremAdjID,string strSrcTxt, string strSevCD, string strShortDescText, string strFullDescTest, int intCreatedUserID)
        {
            ApplicationStatusLogBE AppStatusLogBE = null;

            try
            {
                AppStatusLogBE = new ApplicationStatusLogBE();
                AppStatusLogBE.PREM_ADJ_ID = intPremAdjID;
                AppStatusLogBE.SRC_TXT = strSrcTxt;
                AppStatusLogBE.SEV_CD = strSevCD;
                AppStatusLogBE.SHORT_DESC_TXT = strShortDescText;
                AppStatusLogBE.FULL_DESC_TXT = strFullDescTest;
                AppStatusLogBE.CREATE_USER_ID = intCreatedUserID;
                AppStatusLogBE.CREATE_DATE = System.DateTime.Now;
                DA.Add(AppStatusLogBE);
            }
            catch (Exception ex)
            {
                throw new RetroBaseException(ex.Message);
            }
        }
        public void WriteLog(string strSrcTxt, string strSevCD, string strShortDescText, string strFullDescTest,int intCustmrID, int intCreatedUserID)
        {
            ApplicationStatusLogBE AppStatusLogBE = null;

            try
            {
                AppStatusLogBE = new ApplicationStatusLogBE();
                AppStatusLogBE.SRC_TXT = strSrcTxt;
                AppStatusLogBE.SEV_CD = strSevCD;
                AppStatusLogBE.SHORT_DESC_TXT = strShortDescText;
                AppStatusLogBE.FULL_DESC_TXT = strFullDescTest;
                AppStatusLogBE.CUSTMR_ID = intCustmrID;
                AppStatusLogBE.CREATE_USER_ID = intCreatedUserID;
                AppStatusLogBE.CREATE_DATE = System.DateTime.Now;
                DA.Add(AppStatusLogBE);
            }
            catch (Exception ex)
            {
                throw new RetroBaseException(ex.Message);
            }
        }

        public IList<ApplicationStatusLogBE> getLogData(int AcctNumber, string InterfaceType, string fromDate, string toDate)
        {
            IList<ApplicationStatusLogBE> list = new List<ApplicationStatusLogBE>();
            ApplicationStatusLogDA APLDA = new ApplicationStatusLogDA();

            try
            {
                list = APLDA.getLogList(AcctNumber, InterfaceType, fromDate, toDate);
            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
    }
}
