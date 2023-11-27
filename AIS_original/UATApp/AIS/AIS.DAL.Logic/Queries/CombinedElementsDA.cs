using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;
namespace ZurichNA.AIS.DAL.Logic
{
    public class CombinedElementsDA:DataAccessor<COMB_ELEMT,CombinedElementsBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// This function is used to retrieve the combined elements 
        /// information based on the programperiodid.
        /// </summary>
        /// <param name="Programperiod">Programperiod</param>
        /// <returns>List</returns>
        public IList<CombinedElementsBE> getCombinedElements(int Prgmprdid)
        {

            IList<CombinedElementsBE> result = new List<CombinedElementsBE>();
            IQueryable<CombinedElementsBE> query = this.BuildQueryList();
            //if (Prgmprdid > 0)

                query = query.Where(ce => ce.PREM_ADJ_PGM_ID == Prgmprdid);
            result = query.ToList();
            return result;
        }

        public IList<CombinedElementsBE> getCombelemsPolicylist(int ProgramPeriodID)
        {
             CombinedElementsBE lstPolicy = new CombinedElementsBE();
             lstPolicy.COML_AGMT_ID = 0;
             lstPolicy.Policy = "(Select)";
           
            IList<CombinedElementsBE> result = new List<CombinedElementsBE>();
            if (this.Context == null)
                this.Initialize();


            var firstquery = from first in this.Context.COMB_ELEMTs 
                             select first.coml_agmt_id;

            IQueryable<CombinedElementsBE> query =
            (from que in this.Context.COML_AGMTs
             where que.prem_adj_pgm_id == ProgramPeriodID
              && !firstquery.Contains(que.coml_agmt_id)
              && que.actv_ind == true
             orderby que.pol_sym_txt + que.pol_nbr_txt + que.pol_modulus_txt
             select new CombinedElementsBE()
             {
                 COML_AGMT_ID =que.coml_agmt_id,
                 Policy = que.pol_sym_txt.Trim()+ que.pol_nbr_txt.Trim()+que.pol_modulus_txt,
             });
            if (query.Count() > 0)
                result = query.ToList();
          
            return result;
        }
        
       
        /// <summary>
        /// This function is used to build the query and return the resulsset.
        /// </summary>
        /// <returns></returns>
        /// 
       private IQueryable<CombinedElementsBE> BuildQueryList()
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<CombinedElementsBE> query = from ce in Context.COMB_ELEMTs
                       
                        //where ce.actv_ind==true
                        select new CombinedElementsBE
                         {
                            COMB_ELEMTS_SETUP_ID =ce.comb_elemts_id,
                            PREM_ADJ_PGM_ID = ce.prem_adj_pgm_id,
                            COML_AGMT_ID = ce.coml_agmt_id,
                            AUDIT_EXPO_AMT = ce.audt_expo_amt,
                            EXPO_TYP_ID=ce.expo_typ_id,
                            DVSR_NBR_ID=ce.dvsr_nbr_id,
                            PERTEXT=(from lkp in Context.LKUPs
                                     where lkp.lkup_id == ce.dvsr_nbr_id
                                     select lkp.lkup_txt).First().ToString(),                                       
                            ADJ_RT = ce.adj_rt,
                            TOT_AMT = ce.tot_amt,
                            ACTV_IND = ce.actv_ind,
                             UPDT_DATE=ce.updt_dt ,
                            PerfectPolicyNumber = (from ca in this.Context.COML_AGMTs
                                           where ca.coml_agmt_id == ce.coml_agmt_id
                                           select ca.pol_sym_txt.Trim() + " "+ 
                                                  ca.pol_nbr_txt.Trim() +" "+ 
                                                  ca.pol_modulus_txt.Trim()).First()


                                                 
                        };
            return query;

        
        }

       public IList<CombinedElementsBE> getCombinedElements(int intPrgmprdID,int intAccountID)
       {

           IList<CombinedElementsBE> result = new List<CombinedElementsBE>();
           if (this.Context == null)
               this.Initialize();

           IQueryable<CombinedElementsBE> query = from ce in Context.COMB_ELEMTs
                                                  where ce.prem_adj_pgm_id==intPrgmprdID && ce.custmr_id==intAccountID
                                                  select new CombinedElementsBE
                                                  {
                                                      COMB_ELEMTS_SETUP_ID = ce.comb_elemts_id,
                                                      PREM_ADJ_PGM_ID = ce.prem_adj_pgm_id,
                                                      COML_AGMT_ID = ce.coml_agmt_id,
                                                      AUDIT_EXPO_AMT = ce.audt_expo_amt,
                                                      EXPO_TYP_ID = ce.expo_typ_id,
                                                      DVSR_NBR_ID = ce.dvsr_nbr_id,
                                                      PERTEXT = (from lkp in Context.LKUPs
                                                                 where lkp.lkup_id == ce.dvsr_nbr_id
                                                                 select lkp.lkup_txt).First().ToString(),
                                                      ADJ_RT = ce.adj_rt,
                                                      TOT_AMT = ce.tot_amt,
                                                      ACTV_IND = ce.actv_ind

                                                  };
           result=query.ToList();
           
           return result;
       }

       public IList<CombinedElementsBE> getPolicyList(int Prgmprdid)
       {

           IList<CombinedElementsBE> result = new List<CombinedElementsBE>();
            IQueryable<CombinedElementsBE> query = this.BuildQueryList();
            //if (Prgmprdid > 0)
             query = query.Where(ce => ce.PREM_ADJ_PGM_ID == Prgmprdid);
            result = query.ToList();
            return result;
        }

       private IQueryable<CombinedElementsBE> BuildPolicylist()
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<CombinedElementsBE> query = from res in this.Context.COML_AGMTs                                         
                                        where res.coml_agmt_id != res.coml_agmt_id && res.actv_ind==true
                       
                        
                        select new CombinedElementsBE
                         {
                           
                            COML_AGMT_ID = res.coml_agmt_id,
                            

                        };
            return query;

        
        }
          
       }

    }

