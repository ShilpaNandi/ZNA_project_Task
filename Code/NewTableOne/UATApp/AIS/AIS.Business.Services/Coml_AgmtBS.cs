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
    public class Coml_AgmtBS : BusinessServicesBase<Coml_AgmtBE, Coml_AgmtDA>
    {

        private IList<Coml_AgmtBE> getCommercialAgreement(int ProgPerdID)
        {
            Coml_AgmtDA comm = new Coml_AgmtDA();
            IList<Coml_AgmtBE> result = comm.getCommAgrList(ProgPerdID);
            return result;
        }
        public IList<Coml_AgmtBE> getCommercialAgreement(int ProgPerdID,int CustomerID)
        {
            Coml_AgmtDA comm = new Coml_AgmtDA();
            IList<Coml_AgmtBE> result = comm.getCommAgrList(ProgPerdID);
            return result;
        }
    }
}
