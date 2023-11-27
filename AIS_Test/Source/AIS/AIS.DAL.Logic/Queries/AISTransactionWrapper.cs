using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework;

namespace ZurichNA.AIS.DAL.Logic
{
    public class AISTransactionWrapper : TransactionWrapper<AISDatabaseLINQDataContext>
    {
    }
}
