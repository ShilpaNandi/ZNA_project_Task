/*-----	Page:	PolicyInfo data Accessor Layer
-----
-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	Code to Save or retreive data from the COML_AGMT table.
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 2/16/09 (AS part of Retro Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               06/16/09	Zakir Hussain
-----				Code modified in Policy Info to save the values of the NY Premium amount field and retrieve the data field values from the database.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class PolicyDA : DataAccessor<COML_AGMT, PolicyBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Retrieves the Policy Information-AK
        /// </summary>
        /// <returns>List of PolicyBE</returns>
        public IList<PolicyBE> getPolicyData()
        {
            IQueryable<PolicyBE> query = this.BuildQueryLossInfo();
            return query.ToList();
        }

        /// <summary>
        /// Retrieves the Policy for a specific criteria
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <returns>List of PolicyBE</returns>
        public List<PolicyBE> GetPolicyData(int ProgramPeriodID)
        {
            IQueryable<PolicyBE> query = this.BuildInActiveQuery();

            if (ProgramPeriodID > 0)
            {
                query = query.Where(pol => pol.ProgramPeriodID == ProgramPeriodID).OrderBy(pol=>pol.PolicyID);

            }
            return (query.ToList());
        }

        /// <summary>
        /// Retrieves the Policy for a specific criteria
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="PolicyID"></param>
        /// <returns>List of PolicyBE</returns>
        public List<PolicyBE> getPolicyData(int ProgramPeriodID, int PolicyID)
        {
            IQueryable<PolicyBE> query = this.BuildInActiveQuery();

            if (PolicyID > 0)
            {

                query = query.Where(pol => pol.PolicyID == PolicyID);

            }

            if (ProgramPeriodID > 0)
            {
                query = query.Where(pol => (pol.ProgramPeriodID == ProgramPeriodID) && (pol.ParentPolicyID == null));

            }
            return (query.ToList());
        }
        public List<PolicyBE> getPolicyDataForParentID(int ParentPolicyID)
        {
            IQueryable<PolicyBE> query = from pol in this.Context.COML_AGMTs
                                         orderby pol.pol_sym_txt + pol.pol_nbr_txt + pol.pol_modulus_txt, 
                                         pol.pol_eff_dt, pol.planned_end_date
                                         select new PolicyBE
                                         {
                                             PolicyID = pol.coml_agmt_id,
                                             PlanEndDate = pol.planned_end_date,
                                             PolicyEffectiveDate = pol.pol_eff_dt,                                             
                                             PolicyModulus = pol.pol_modulus_txt,
                                             PolicyNumber = pol.pol_nbr_txt,
                                             PolicySymbol = pol.pol_sym_txt,
                                             ProgramPeriodID = pol.prem_adj_pgm_id,
                                             ParentPolicyID = pol.parnt_coml_agmt_id,
                                             IsActive = pol.actv_ind
                                         };

            query = query.Where(pol => pol.ParentPolicyID == ParentPolicyID);

            return (query.ToList());
        }

        /// <summary>
        /// Retrieves the Policy for a specific criteria
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="cstmrid"></param>
        /// <returns>List of PolicyBE</returns>
        public List<PolicyBE> getPolicyDataforActID(int ProgramPeriodID, int cstmrid)
        {
            IQueryable<PolicyBE> query = this.BuildQuery();

            if (cstmrid > 0)
            {
                query = query.Where(pol => pol.cstmrid == cstmrid);
            }

            if (ProgramPeriodID > 0)
            {
                query = query.Where(pol => pol.ProgramPeriodID == ProgramPeriodID);

            }
            return (query.ToList());
        }

        

        /// <summary>
        /// Retrieves the Policy for a specific criteria
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="cstmrid"></param>
        /// <returns>List of PolicyBE</returns>
        public List<PolicyBE> getPolicyDataforActIDLBA(int ProgramPeriodID, int cstmrid)
        {
            IQueryable<PolicyBE> query = this.BuildQuery();
            int Typ1 = 0;
            int Typ2 = 0;
            int Typ3 = 0;
            
                Typ1 = (from lk in this.Context.LKUPs
                              where lk.lkup_txt == "AH"
                              select lk.lkup_id).First();
            
                Typ2 = (from lk in this.Context.LKUPs
                              where lk.lkup_txt == "WC"
                              select lk.lkup_id).First();
          
                Typ3 = (from lk in this.Context.LKUPs
                              where lk.lkup_txt == "PROF"
                              select lk.lkup_id).First();
           
            if (cstmrid > 0)
            {
                query = query.Where(pol => pol.cstmrid == cstmrid);
            }

            if (ProgramPeriodID > 0)
            {
                query = query.Where(pol => pol.ProgramPeriodID == ProgramPeriodID);

            }
            if (ProgramPeriodID > 0)
            {
                query = query.Where(pol => (pol.CoverageTypeID == Typ1 || pol.CoverageTypeID == Typ2 || pol.CoverageTypeID == Typ3));

            }
           
            return (query.ToList());
        }
        /// <summary>
        /// Retrieves the policy for a specific criteria for NY-SIF screen
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="cstmrid"></param>
        /// <returns></returns>
        public List<PolicyBE> getPolicies(int ProgramPeriodID, int cstmrid)
        {
            if (this.Context == null) this.Initialize();
           
            IQueryable<PolicyBE> query = from pol in this.Context.COML_AGMTs
                                         where pol.actv_ind==true
                                         orderby pol.pol_sym_txt, pol.pol_nbr_txt, pol.pol_modulus_txt
                                         select new PolicyBE
                                         {
                                             cstmrid = pol.custmr_id,
                                             IsActive = pol.actv_ind,
                                             PolicyID = pol.coml_agmt_id,
                                             ProgramPeriodID = pol.prem_adj_pgm_id,
                                             PolicyNumber = pol.pol_sym_txt + " " + pol.pol_nbr_txt + " " + pol.pol_modulus_txt
                                              
                                         };

            if (cstmrid > 0)
            {
                query = query.Where(pol => pol.cstmrid == cstmrid);
            }

            if (ProgramPeriodID > 0)
            {
                query = query.Where(pol => pol.ProgramPeriodID == ProgramPeriodID);

            }
            return(query.ToList());
        }
        private IQueryable<PolicyBE> BuildQuery()
        {
            IQueryable<PolicyBE> query = from pol in this.Context.COML_AGMTs
                                         orderby pol.pol_nbr_txt
                                         //, pol.pol_eff_dt
                                         where pol.actv_ind==true
                                         select new PolicyBE
                                         {
                                             AdjusmentTypeID = pol.adj_typ_id,
                                             ALAECappedAmount = pol.aloc_los_adj_exps_capped_amt,
                                             ALAETypeID = pol.aloc_los_adj_exps_typ_id,
                                             CoverageTypeID = pol.covg_typ_id,
                                             cstmrid = pol.custmr_id,
                                             DedTblPolicyLimitAmount = pol.dedtbl_pol_lim_amt,
                                             DedtblProtPolicyStID = pol.dedtbl_prot_pol_st_id,
                                             DedtblProtPolMaxAmount = pol.dedtbl_prot_pol_max_amt,
                                             IBNRFactor = pol.incur_but_not_rptd_fctr_rt,
                                             LDFFactor = pol.los_dev_fctr_rt,
                                             IsActive = pol.actv_ind,
                                             LDFIncurredNO63740 = pol.los_dev_fctr_incur_but_not_rptd_step_ind,
                                             LDFIncurredNotReport = pol.los_dev_fctr_incur_but_not_rptd_incld_lim_ind,
                                             LossSystemSourceID = pol.los_sys_src_id,
                                             NonConversionAmount = pol.nonconv_amt,
                                             OtherPolicyAdjustmentAmount = pol.othr_pol_adj_amt,
                                             OverrideDedtblLimitAmount = pol.overrid_dedtbl_lim_amt,
                                             PlanEndDate = pol.planned_end_date,
                                             PolicyEffectiveDate = pol.pol_eff_dt,
                                             PolicyID = pol.coml_agmt_id,
                                             PolicyModulus = pol.pol_modulus_txt,
                                             PolicyNumber = pol.pol_nbr_txt,
                                             PolicySymbol = pol.pol_sym_txt,
                                             ProgramPeriodID = pol.prem_adj_pgm_id,
                                             TPADirectIndicator = pol.thrd_pty_admin_dir_ind,
                                             TPAIndicator = pol.thrd_pty_admin_dir_ind,
                                             UnlimDedtblPolLimitIndicator = pol.unlim_dedtbl_pol_lim_ind,
                                             UnlimOverrideDedtblLimitIndicator = pol.unlim_overrid_dedtbl_lim_ind,
                                             ParentPolicyID = pol.parnt_coml_agmt_id,
                                             ISMasterPEOPolicy=pol.mstr_peo_pol_ind,
                                             NYPremiumDiscAmount = pol.ny_prem_disc_amt
                                         };

            return query;
        }

        private IQueryable<PolicyBE> BuildInActiveQuery()
        {
            IQueryable<PolicyBE> query = from pol in this.Context.COML_AGMTs
                                         orderby pol.pol_sym_txt, pol.pol_nbr_txt, pol.pol_modulus_txt
                                         //, pol.pol_eff_dt
                                         select new PolicyBE
                                         {
                                             AdjusmentTypeID = pol.adj_typ_id,
                                             ALAECappedAmount = pol.aloc_los_adj_exps_capped_amt,
                                             ALAETypeID = pol.aloc_los_adj_exps_typ_id,
                                             CoverageTypeID = pol.covg_typ_id,
                                             cstmrid = pol.custmr_id,
                                             DedTblPolicyLimitAmount = pol.dedtbl_pol_lim_amt,
                                             DedtblProtPolicyStID = pol.dedtbl_prot_pol_st_id,
                                             DedtblProtPolMaxAmount = pol.dedtbl_prot_pol_max_amt,
                                             IBNRFactor = pol.incur_but_not_rptd_fctr_rt,
                                             LDFFactor = pol.los_dev_fctr_rt,
                                             IsActive = pol.actv_ind,
                                             LDFIncurredNO63740 = pol.los_dev_fctr_incur_but_not_rptd_step_ind,
                                             LDFIncurredNotReport = pol.los_dev_fctr_incur_but_not_rptd_incld_lim_ind,
                                             LossSystemSourceID = pol.los_sys_src_id,
                                             NonConversionAmount = pol.nonconv_amt,
                                             OtherPolicyAdjustmentAmount = pol.othr_pol_adj_amt,
                                             OverrideDedtblLimitAmount = pol.overrid_dedtbl_lim_amt,
                                             PlanEndDate = pol.planned_end_date,
                                             PolicyEffectiveDate = pol.pol_eff_dt,
                                             PolicyID = pol.coml_agmt_id,
                                             PolicyModulus = pol.pol_modulus_txt,
                                             PolicyNumber = pol.pol_nbr_txt,
                                             PolicySymbol = pol.pol_sym_txt,
                                             ProgramPeriodID = pol.prem_adj_pgm_id,
                                             TPADirectIndicator = pol.thrd_pty_admin_dir_ind,
                                             TPAIndicator = pol.thrd_pty_admin_dir_ind,
                                             UnlimDedtblPolLimitIndicator = pol.unlim_dedtbl_pol_lim_ind,
                                             UnlimOverrideDedtblLimitIndicator = pol.unlim_overrid_dedtbl_lim_ind,
                                             ParentPolicyID = pol.parnt_coml_agmt_id,
                                             ISMasterPEOPolicy = pol.mstr_peo_pol_ind,
                                             NYPremiumDiscAmount = pol.ny_prem_disc_amt
                                         };

            return query;
        }

        //AK
        private IQueryable<PolicyBE> BuildQueryLossInfo()
        {
            IQueryable<PolicyBE> query = from pol in this.Context.COML_AGMTs
                                         where pol.actv_ind == true
                                         select new PolicyBE
                                         {

                                             POLICY_EFF_DATE = DateTime.Parse(pol.pol_eff_dt.ToString()).ToShortDateString(),
                                             POLICY_END_DATE = DateTime.Parse(pol.pol_eff_dt.ToString()).ToShortDateString(),
                                             PolicyID = pol.coml_agmt_id,
                                             PolicyModulus = pol.pol_modulus_txt,
                                             PolicyNumber = pol.pol_sym_txt.Trim() + pol.pol_nbr_txt.Trim() + pol.pol_modulus_txt.Trim(),
                                             PolicySymbol = pol.pol_sym_txt,

                                         };

            return query;
        }
        /// <summary>
        /// Retrieves Policy Effective Date based on Prg Id and Account No.-AK
        /// </summary>
        /// <returns>List of Policy Eff. date</returns>
        public IList<PolicyBE> getPolicyExpirationDate(int ProgramPeriodID, int cstmrid)
        {
            IList<PolicyBE> result = new List<PolicyBE>();
            IQueryable<PolicyBE> query =
            (from ams in this.Context.COML_AGMTs
             where ams.prem_adj_pgm_id == ProgramPeriodID && ams.custmr_id == cstmrid
             orderby ams.planned_end_date ascending
             select new PolicyBE {   POLICY_END_DATE = DateTime.Parse(ams.planned_end_date.ToString()).ToShortDateString() }).Distinct();
            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Retrieves Policy Effective Date based on Prg Id and Account No.-AK
        /// </summary>
        /// <returns>List of Policy Eff. date</returns>
        public IList<PolicyBE> getPolicyLookUpData(int ProgramPeriodID, int cstmrid)
        {
            IList<PolicyBE> result = new List<PolicyBE>();
            IQueryable<PolicyBE> query =
            (from ams in this.Context.COML_AGMTs
             where ams.prem_adj_pgm_id == ProgramPeriodID && ams.custmr_id == cstmrid
             orderby ams.pol_eff_dt ascending
             select new PolicyBE { POLICY_EFF_DATE = DateTime.Parse(ams.pol_eff_dt.ToString()).ToShortDateString() }).Distinct();
            result = query.ToList();
            return result;
        }



        /// <summary>
        /// Retrieves Policy based on LOB and Prg ID-AK
        /// </summary>
        /// <returns>List of Policy Data</returns>
        /// Kartik/Zakir 10/19/2010 Modified the method to fix the LOB issue. 
        public IList<PolicyBE> getLOBPolData(String LOB, int PrgID)
        {

            // int CovAHID = (from lk in this.Context.LKUPs
            //                 where lk.lkup_txt == "AH"
            //                 select lk.lkup_id).First();
            // int CovWCID = (from lk in this.Context.LKUPs
            //                where lk.lkup_txt == "WC"
            //                select lk.lkup_id).First();
            //int CovGLID = (from lk in this.Context.LKUPs
            //                where lk.lkup_txt == "GL"
            //                select lk.lkup_id).First();
            // int CovGLPRODID = (from lk in this.Context.LKUPs
            //                where lk.lkup_txt == "GL/PROD"
            //                select lk.lkup_id).First();
            // int CovPRODID = (from lk in this.Context.LKUPs
            //                    where lk.lkup_txt == "PROD"
            //                    select lk.lkup_id).First();
            // int CovPROFID = (from lk in this.Context.LKUPs
            //                    where lk.lkup_txt == "PROF"
            //                    select lk.lkup_id).First();
            // int CovAUTOID = (from lk in this.Context.LKUPs
            //                  where lk.lkup_txt == "AUTO"
            //                  select lk.lkup_id).First();
            // int CovALPDID = (from lk in this.Context.LKUPs
            //                  where lk.lkup_txt == "AL/PD"
            //                  select lk.lkup_id).First();

            List<int> l = buildlist(LOB);



            IList<PolicyBE> result = new List<PolicyBE>();
            if (LOB == "WC")
            {


                IQueryable<PolicyBE> query =
                (from pol in this.Context.COML_AGMTs
                 // where (pol.covg_typ_id == CovAHID || pol.covg_typ_id == CovWCID) 
                 where l.Contains(pol.covg_typ_id.Value)
                 && pol.prem_adj_pgm_id == PrgID
                 && pol.actv_ind == true
                 select new PolicyBE
                 {

                     PolicyID = pol.coml_agmt_id,
                     PolicyNumber = pol.pol_sym_txt.Trim() + pol.pol_nbr_txt.Trim() + pol.pol_modulus_txt.Trim(),
                     //PolicySymbol = pol.pol_sym_txt,
                     PolicySymbol = "WC",

                 });
                result = query.ToList();
            }
            if (LOB == "GL")
            {
                IQueryable<PolicyBE> query =
                (from pol in this.Context.COML_AGMTs
                 // where (pol.covg_typ_id == CovGLID || pol.covg_typ_id == CovGLPRODID || pol.covg_typ_id == CovPRODID || pol.covg_typ_id == CovPROFID) && pol.prem_adj_pgm_id == PrgID
                 where l.Contains(pol.covg_typ_id.Value) && pol.prem_adj_pgm_id == PrgID
                 && pol.actv_ind == true
                 select new PolicyBE
                 {

                     PolicyID = pol.coml_agmt_id,
                     PolicyNumber = pol.pol_sym_txt.Trim() + pol.pol_nbr_txt.Trim() + pol.pol_modulus_txt.Trim(),
                     //PolicySymbol = pol.pol_sym_txt,
                     PolicySymbol = "GL",

                 });
                result = query.ToList();
                // String temp = "88-89-90-91";
                //  List<int?> l = new List<int?>();
                //  l.Add(88);
                //  l.Add(89);
                //  l.Add(90);
                //  l.Add(91);
                //  IQueryable<PolicyBE> query =
                // (from pol in this.Context.COML_AGMTs
                //  where l.Contains(pol.covg_typ_id)== true && pol.prem_adj_pgm_id == PrgID
                //  && pol.actv_ind == true
                ////  (pol.covg_typ_id == CovGLID || pol.covg_typ_id == CovGLPRODID || pol.covg_typ_id == CovPRODID || pol.covg_typ_id == CovPROFID) && pol.prem_adj_pgm_id == PrgID
                // // && pol.actv_ind == true
                //  select new PolicyBE
                //  {

                //      PolicyID = pol.coml_agmt_id,
                //      PolicyNumber = pol.pol_sym_txt.Trim() + pol.pol_nbr_txt.Trim() + pol.pol_modulus_txt.Trim(),
                //      //PolicySymbol = pol.pol_sym_txt,
                //      PolicySymbol = "GL",

                //  });
                //result = query.ToList();

            }
            if (LOB == "AUTO")
            {
                IQueryable<PolicyBE> query =
                (from pol in this.Context.COML_AGMTs
                 where l.Contains(pol.covg_typ_id.Value) == true && pol.prem_adj_pgm_id == PrgID
                 && pol.actv_ind == true
                 select new PolicyBE
                 {

                     PolicyID = pol.coml_agmt_id,
                     PolicyNumber = pol.pol_sym_txt.Trim() + pol.pol_nbr_txt.Trim() + pol.pol_modulus_txt.Trim(),
                     //PolicySymbol = pol.pol_sym_txt,
                     PolicySymbol = "AUTO",

                 });
                result = query.ToList();
            }
            return result;
        }

        private List<int> buildlist(string LOB)
        {
            List<int> result = new List<int>();

            var query = (from lkp in this.Context.LKUPs
                         where lkp.attr_1_txt == LOB
                         select lkp.lkup_id);
            result = query.ToList();
            return result;

        }
        /// <summary>
        ///  Retrives PolicyID and Number for ILRF Setup
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="cstmrid"></param>
        /// <returns></returns>
        public List<LookupBE> getPolicyDataforLookups(int programPeriodID, int cstmrid)
        {
            IQueryable<LookupBE> query = from pol in this.Context.COML_AGMTs
                                         orderby pol.pol_sym_txt
                                         where pol.prem_adj_pgm_id == programPeriodID && pol.custmr_id == cstmrid
                                         && pol.actv_ind == true
                                         orderby pol.pol_nbr_txt ascending
                                         select new LookupBE
                                         {
                                             LookUpID = pol.coml_agmt_id,
                                             LookUpName = pol.pol_sym_txt + " " + pol.pol_nbr_txt + " " + pol.pol_modulus_txt
                                         };
            return (query.ToList());
        }


        public List<LookupBE> getPolicyDataforLookups(string programPeriodIDs, int cstmrid)
        {
            var query = from pol in this.Context.COML_AGMTs
                               orderby pol.pol_sym_txt
                               where   pol.custmr_id == cstmrid
                               && pol.actv_ind == true                               
                               select new
                               {
                                   LookUpID = pol.coml_agmt_id,
                                   LookUpName = pol.pol_sym_txt + " " + pol.pol_nbr_txt + " " + pol.pol_modulus_txt,
                                   prem_adj_pgm_id = pol.prem_adj_pgm_id
                               };

            //List<LookupBE> lookups = (List<LookupBE>)query.ToList().FindAll(p => programPeriodIDs.Contains(p.prem_adj_pgm_id.ToString())).ToList();

            var query1 = query.ToList();
            List<LookupBE> lookups = (from q in query1
                                      where programPeriodIDs.Contains(q.prem_adj_pgm_id.ToString())
                                      select new LookupBE
                                      {
                                          LookUpID = q.LookUpID,
                                          LookUpName = q.LookUpName
                                      }).ToList();

            return lookups.OrderBy(p => p.LookUpName).ToList();
        }



        /// <summary>
        /// Checks if a Policy is already exists with given parameters
        /// </summary>
        /// <param name="polID"></param>        
        /// <param name="polNo"></param>        
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>if exsits return true, else false</returns>
        public bool isPolicyAlreadyExist(int polID,  string polNo, DateTime polEffDate, int programPeriodID)
        {            
            if (this.Context == null) this.Initialize();

            int recCount = (from pol in this.Context.COML_AGMTs
                        where pol.pol_nbr_txt == polNo && pol.pol_eff_dt == polEffDate
                        && pol.prem_adj_pgm_id == programPeriodID && pol.coml_agmt_id != polID
                        select pol).Count();

            if (recCount == 0) return false;
            else return true;
        }
        public bool isPolicyAlreadyExistAndDisabled(int polID, string polNo, DateTime polEffDate, int programPeriodID)
        {
            if (this.Context == null) this.Initialize();

            int recCount = (from pol in this.Context.COML_AGMTs
                            where pol.pol_nbr_txt == polNo && pol.pol_eff_dt == polEffDate
                            && pol.prem_adj_pgm_id == programPeriodID && pol.coml_agmt_id != polID && pol.actv_ind == false
                            select pol).Count();

            if (recCount == 0) return false;
            else return true;
        }
        public bool isPolExistsInAnyAcct(int polID, string PolicySymbol, string polNo, string PolicyModulus, DateTime polEffDate, DateTime polExpDate, int customerID)        
        {            
            if (this.Context == null) this.Initialize();

            int recCount = (from pol in this.Context.COML_AGMTs
                            where pol.pol_sym_txt == PolicySymbol && pol.pol_nbr_txt == polNo && pol.pol_modulus_txt == PolicyModulus
                            && pol.pol_eff_dt == polEffDate && pol.pol_eff_dt == polEffDate && pol.planned_end_date == polExpDate && pol.custmr_id != customerID
                            && pol.coml_agmt_id!=polID && pol.actv_ind == true
                        select pol).Count();

            if (recCount == 0) return false;
            else return true;
        }


        public string isPolExistsInAnyAcctCustName(int polID, string PolicySymbol, string polNo, string PolicyModulus, DateTime polEffDate, DateTime polExpDate, int customerID)
        {
            if (this.Context == null) this.Initialize();

            string custName = (from pol in this.Context.COML_AGMTs
                               join cst in this.Context.CUSTMRs on pol.custmr_id equals cst.custmr_id
                            where pol.pol_sym_txt == PolicySymbol && pol.pol_nbr_txt == polNo && pol.pol_modulus_txt == PolicyModulus
                            && pol.pol_eff_dt == polEffDate && pol.pol_eff_dt == polEffDate && pol.planned_end_date == polExpDate && pol.custmr_id != customerID
                            && pol.coml_agmt_id != polID && pol.actv_ind == true
                            select cst.full_nm).FirstOrDefault();

            return custName;

            
        }


        /// <summary>
        /// to get the policy id 
        /// </summary>           
        /// <param name="polNo"></param>        
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>returns the policy ID</returns>
        public int getPolicyID(string PolicySymbol, string polNo, string PolicyModulus, DateTime polEffDate, DateTime polExpDate, int customerID)
        {
            if (this.Context == null) this.Initialize();

            var policyID = (from pol in this.Context.COML_AGMTs
                            where pol.pol_sym_txt == PolicySymbol && pol.pol_nbr_txt == polNo && pol.pol_modulus_txt == PolicyModulus
                            && pol.pol_eff_dt == polEffDate && pol.planned_end_date == polExpDate
                            && pol.custmr_id == customerID
                            select pol.coml_agmt_id).FirstOrDefault();


            return Convert.ToInt32(policyID);
        }


        /// <summary>
        /// to get the policy id 
        /// </summary>           
        /// <param name="polNo"></param>        
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>returns the policy ID</returns>
        public int getPolicyID(string polNo, DateTime polEffDate, int programPeriodID)
        {
            if (this.Context == null) this.Initialize();

            var policyID = (from pol in this.Context.COML_AGMTs
                            where pol.pol_nbr_txt == polNo && pol.pol_eff_dt == polEffDate
                            && pol.prem_adj_pgm_id == programPeriodID
                            select pol.coml_agmt_id).FirstOrDefault();

            return Convert.ToInt32(policyID);
        }

        /// <summary>
        /// to get the program period id 
        /// </summary>           
        /// <param name="polNo"></param>        
        /// <param name="polEffDate"></param>
        /// <param name="programPeriodID"></param>
        /// <returns>returns the program period ID</returns>
        public int getProgramPeriodID(DateTime polEffDate, DateTime polExpDate,int custmrID)
        {
            if (this.Context == null) this.Initialize();

            var prgmPrdID = (from pol in this.Context.COML_AGMTs
                            where pol.pol_eff_dt == polEffDate
                            && pol.planned_end_date == polExpDate
                            && pol.custmr_id == custmrID
                            select pol.prem_adj_pgm_id).FirstOrDefault();

            return Convert.ToInt32(prgmPrdID);
        }


        /// <summary>
        /// Retrieves the Policy for a specific criteria
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="cstmrid"></param>
        /// <param name="AdjID"></param>
        /// <returns>List of PolicyBE</returns>
        public List<PolicyBE> getPolicyDataforActID(int ProgramPeriodID, int cstmrid,int AdjID)
        {
            IQueryable<PolicyBE> query = from pol in this.Context.COML_AGMTs
                                         orderby pol.pol_nbr_txt
                                         //, pol.pol_eff_dt
                                         where pol.actv_ind == true
                                         select new PolicyBE
                                         {
                                             AdjusmentTypeID = pol.adj_typ_id,
                                             cstmrid = pol.custmr_id,
                                             PolicyID = pol.coml_agmt_id,
                                             PolicyNumber = pol.pol_nbr_txt,
                                             PolicySymbol = pol.pol_sym_txt,
                                             PolicyModulus = pol.pol_modulus_txt,
                                             
                                             ProgramPeriodID = pol.prem_adj_pgm_id,
                                              
                                         };

            if (cstmrid > 0)
            {
                query = query.Where(pol => pol.cstmrid == cstmrid);
            }

            if (ProgramPeriodID > 0)
            {
                query = query.Where(pol => pol.ProgramPeriodID == ProgramPeriodID);
            }
            if (AdjID > 0)
            {
                query = query.Where(pol => pol.AdjusmentTypeID == AdjID);
            }
            return (query.ToList());
        }


    }
}
