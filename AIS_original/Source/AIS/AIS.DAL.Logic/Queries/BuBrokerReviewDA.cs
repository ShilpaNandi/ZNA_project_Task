using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.Logic;

///-----------------------------------------------------------------------------------///
///Added By:-Venkata R Kolimi
///Purpouse:-As Part of Bu Broker Review Work Order
///Created Date:-09/08/2009
///Modified By:
///Modified Date:
///Files Used:BuBrokerReview.apsx.cs
///-----------------------------------------------------------------------------------///
namespace ZurichNA.AIS.DAL.Logic
{
    public class BuBrokerReviewDA
    {
        /// <summary>
        /// Data Acessor Method used to update the BU,Broker and Broker Contact  for the adjutement 
        /// and program periods under that adjustment in a single Transaction
        /// </summary>
        /// <param name="objBuBrokerDC"></param>
        /// <param name="intAdjNo"></param>
        /// <param name="intBrokerID"></param>
        /// <param name="intBUOfficeID"></param>
        /// <param name="intBrokerContactID"></param>
        /// <param name="intUserID"></param>
        /// <returns>bool</returns>
        public bool UpdateBuBrokerReviewData(AISDatabaseLINQDataContext objBuBrokerDC, int intAdjNo, int intBrokerID,int intBUOfficeID,int intBrokerContactID, int intUserID)
        {
            try
            {
                var PremAdj = (from cdd in objBuBrokerDC.PREM_ADJs where cdd.prem_adj_id == intAdjNo select cdd).First();
                PremAdj.brkr_id = intBrokerID;
                PremAdj.bu_office_id = intBUOfficeID;
                PremAdj.updt_dt = System.DateTime.Now;
                PremAdj.updt_user_id = intUserID;
                
                var premAdjPerdBE = (from cdd in objBuBrokerDC.PREM_ADJ_PERDs where cdd.prem_adj_id == intAdjNo select cdd).ToList();

                for (int i = 0; i < premAdjPerdBE.Count; i++)
                {
                    var pgmprdBE = (from cdd in objBuBrokerDC.PREM_ADJ_PGMs where cdd.prem_adj_pgm_id == premAdjPerdBE[i].prem_adj_pgm_id select cdd).First();

                   
                    if ((pgmprdBE.brkr_id != intBrokerID) && (intBrokerContactID == 0))
                        pgmprdBE.brkr_conctc_id = null;
                    pgmprdBE.brkr_id = intBrokerID;
                    pgmprdBE.bsn_unt_ofc_id = intBUOfficeID;
                    if(intBrokerContactID!=0)
                        pgmprdBE.brkr_conctc_id = intBrokerContactID;                 
                    pgmprdBE.updt_dt = System.DateTime.Now;
                    pgmprdBE.updt_user_id = intUserID;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
