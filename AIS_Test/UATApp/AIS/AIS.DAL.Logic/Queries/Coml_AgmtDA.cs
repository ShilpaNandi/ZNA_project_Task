using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class Coml_AgmtDA : DataAccessor<COML_AGMT, Coml_AgmtBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Function return the Coml_AgreementBE based on Program PeriodID
        /// </summary>
        /// <param name="intProgPerdID"></param>
        /// <returns>Coml_AgmtBE  List</returns>
        public IList<Coml_AgmtBE> getCommAgrList(int intProgPerdID)
        {
            IList<Coml_AgmtBE> result = new List<Coml_AgmtBE>();

            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<Coml_AgmtBE> query =
            (from que in this.Context.COML_AGMTs
             where que.prem_adj_pgm_id == intProgPerdID
             && que.actv_ind == true
             select new Coml_AgmtBE()
             {
                 Comm_Agr_ID = que.coml_agmt_id,
                 Prem_Adj_Prg_ID = que.prem_adj_pgm_id,
                 Customer_ID = que.custmr_id,
                 Pol_Sym_Txt = que.pol_sym_txt,
                 Pol_Nbr_Txt = que.pol_nbr_txt,
                 Pol_Mod_Txt = que.pol_modulus_txt,
                 POLICY = que.pol_sym_txt + " " + que.pol_nbr_txt + " " + que.pol_modulus_txt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        /// <summary>
        /// Function return the Coml_AgreementBE based on Program PeriodID and CustomerID
        /// </summary>
        /// <param name="intProgPerdID"></param>
        /// <param name="CustomerID"></param>
        /// <returns>Commercial Agreement List</returns>
        public IList<Coml_AgmtBE> getCommAgrList(int intProgPerdID, int CustomerID)
        {
            IList<Coml_AgmtBE> result = new List<Coml_AgmtBE>();
            if (this.Context == null)
                this.Initialize();
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            var firstquery = from first in this.Context.COML_AGMT_AUDTs
                             select first.coml_agmt_id;
            IQueryable<Coml_AgmtBE> query =
            (from que in this.Context.COML_AGMTs
             where que.prem_adj_pgm_id == intProgPerdID
             && que.custmr_id == CustomerID && !firstquery.Contains(que.coml_agmt_id)
             select new Coml_AgmtBE()
             {
                 Comm_Agr_ID = que.coml_agmt_id,
                 Prem_Adj_Prg_ID = que.prem_adj_pgm_id,
                 Customer_ID = que.custmr_id,
                 Pol_Sym_Txt = que.pol_sym_txt,
                 Pol_Nbr_Txt = que.pol_nbr_txt,
                 Pol_Mod_Txt = que.pol_modulus_txt,
                 POLICY = que.pol_sym_txt + " " + que.pol_nbr_txt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
    }
}
