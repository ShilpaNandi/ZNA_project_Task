using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class PremAdjCommentDA : DataAccessor<PREM_ADJ_CMMNT, PremiumAdjustmetCommentBE, AISDatabaseLINQDataContext>
    {
    }
}
