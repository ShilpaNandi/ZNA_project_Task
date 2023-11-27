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
using ZurichNA.AIS.DAL.LINQ;

///-----------------------------------------------------------------------------------///
///Added By:-Venkata R Kolimi
///Purpouse:-As Part of Bu Broker Review Work Order
///Created Date:-09/08/2009
///Modified By:
///Modified Date:
///Files Used:BuBrokerReview.apsx.cs
///-----------------------------------------------------------------------------------///
namespace ZurichNA.AIS.Business.Logic
{
    public class BuBrokerReviewBS
    {
        public BuBrokerReviewBS()
        { }
        /// <summary>
        /// Business service Method used to update the BU,Broker and Broker Contact  for the adjutement 
        /// and program periods under that adjustment in a single Transaction
        /// </summary>
        /// <param name="objBuBrokerDC"></param>
        /// <param name="intAdjNo"></param>
        /// <param name="intBrokerID"></param>
        /// <param name="intBUOfficeID"></param>
        /// <param name="intBrokerContactID"></param>
        /// <param name="intUserID"></param>
        /// <returns>bool</returns>
        public bool UpdateBuBrokerReviewData(AISDatabaseLINQDataContext objBuBrokerDC, int intAdjNo, int intBrokerID, int intBUOfficeID, int intBrokerContactID, int intUserID)
        {
            BuBrokerReviewDA objBuBrokerReviewDA = new BuBrokerReviewDA();
            return (objBuBrokerReviewDA.UpdateBuBrokerReviewData(objBuBrokerDC, intAdjNo, intBrokerID,intBUOfficeID,intBrokerContactID, intUserID));
        }
    }
}
