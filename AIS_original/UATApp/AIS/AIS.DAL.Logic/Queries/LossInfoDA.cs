using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// DataAccessor for Loss Info
    /// </summary>
    public class LossInfoDA : DataAccessor<ARMIS_LOS_POL, LossInfoBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Returns LOB
        /// </summary>
        /// <returns></returns>
        public IList<LookupBE>  getLOBLookUpData()
        {
            IList<LookupBE> result = new List<LookupBE>();
            IQueryable<LookupBE> query =
            (from ams in this.Context.COML_AGMTs
             select new LookupBE{LookUpName= ams.pol_sym_txt}).Distinct();
             result= query.ToList();
             return result;
        }

        /// <summary>
        /// Returns all loss Info that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoData(DateTime ValDate,int custmrID,int prgID)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs 
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ca.custmr_id == custmrID && 
             ca.prem_adj_pgm_id==prgID 
             orderby ca.pol_sym_txt, ca.pol_nbr_txt, ams.st_id ascending
             select new LossInfoBE()
             {
                 ARMIS_LOS_ID=ams.armis_los_pol_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id==Convert.ToInt32(ca.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 POLICY=ams.COML_AGMT.pol_sym_txt.Trim()+ ams.COML_AGMT.pol_nbr_txt.Trim()+ams.COML_AGMT.pol_modulus_txt.Trim(),
                 COML_AGMT_ID=ams.coml_agmt_id,
                 ST_ID=ams.st_id,
                 SUPRT_SERV_CUSTMR_GP_ID = ams.suprt_serv_custmr_gp_id,
                 PAID_IDNMTY_AMT=ams.paid_idnmty_amt,
                 PAID_EXPS_AMT=ams.paid_exps_amt,
                 RESRV_IDNMTY_AMT=ams.resrv_idnmty_amt,
                 RESRV_EXPS_AMT=ams.resrv_exps_amt,
                 SYS_GENRT_IND=ams.sys_genrt_ind,
                 VALN_DATE=ams.valn_dt,
                 CUSTMR_ID = ams.custmr_id,
                 ACTV_IND=ams.actv_ind,
                 PREM_ADJ_ID=ams.prem_adj_id,
                 UPDATEDDATE=ams.updt_dt,
                 ADJ_STATUS = (from pap in this.Context.PREM_ADJ_STs
                               join lkup in this.Context.LKUPs
                               on pap.adj_sts_typ_id equals lkup.lkup_id
                               orderby pap.prem_adj_sts_id descending
                               where pap.prem_adj_id == ams.prem_adj_id
                               select lkup.lkup_txt).First().ToString(),
                 Incurred = ams.paid_idnmty_amt + ams.paid_exps_amt + ams.resrv_idnmty_amt + ams.resrv_exps_amt,
                 EXC_NON_BIL = (from aldet in this.Context.ARMIS_LOS_EXCs
                                where aldet.armis_los_pol_id ==ams.armis_los_pol_id
                                select "Yes").First().ToString()
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            result = result.OrderBy(rs => rs.STATETYPE).OrderBy(rs=>rs.POLICY).OrderBy(rs=>rs.POLICYSYMBOL).ToList();
            return result;

        }
        /// <summary>
        /// Returns all loss Info that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoDataAdjNo(DateTime ValDate, int custmrID, int prgID,int adjNo)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ca.custmr_id == custmrID &&
             ca.prem_adj_pgm_id == prgID && ams.prem_adj_id == adjNo 
             orderby ca.pol_sym_txt, ca.pol_nbr_txt, ams.st_id ascending
             select new LossInfoBE()
             {
                 ARMIS_LOS_ID = ams.armis_los_pol_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(ca.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 POLICY = ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim(),
                 COML_AGMT_ID = ams.coml_agmt_id,
                 ST_ID = ams.st_id,
                 SUPRT_SERV_CUSTMR_GP_ID = ams.suprt_serv_custmr_gp_id,
                 PAID_IDNMTY_AMT = ams.paid_idnmty_amt,
                 PAID_EXPS_AMT = ams.paid_exps_amt,
                 RESRV_IDNMTY_AMT = ams.resrv_idnmty_amt,
                 RESRV_EXPS_AMT = ams.resrv_exps_amt,
                 SYS_GENRT_IND = ams.sys_genrt_ind,
                 CUSTMR_ID = ams.custmr_id,
                 ACTV_IND = ams.actv_ind,
                 PREM_ADJ_ID = ams.prem_adj_id,
                 ADJ_STATUS = (from pap in this.Context.PREM_ADJ_STs
                               join lkup in this.Context.LKUPs
                               on pap.adj_sts_typ_id equals lkup.lkup_id
                               orderby pap.prem_adj_sts_id descending
                               where pap.prem_adj_id == ams.prem_adj_id && pap.prem_adj_id==adjNo
                               select lkup.lkup_txt).First().ToString(),
                 Incurred = ams.paid_idnmty_amt + ams.paid_exps_amt + ams.resrv_idnmty_amt + ams.resrv_exps_amt,
                 EXC_NON_BIL = (from aldet in this.Context.ARMIS_LOS_EXCs
                                where aldet.armis_los_pol_id == ams.armis_los_pol_id
                                select "Yes").First().ToString(),
                 UPDATEDDATE = ams.updt_dt,
                 UPDATEDUSER = ams.updt_user_id
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            result = result.OrderBy(rs => rs.STATETYPE).OrderBy(rs => rs.POLICY).OrderBy(rs => rs.POLICYSYMBOL).ToList();
            return result;

        }
        /// <summary>
        /// Returns all loss Info that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoDataHideDisLinesAdjNo(DateTime ValDate, int custmrID, int prgID, bool flag,int adjNo)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == adjNo
             orderby ca.pol_sym_txt, ca.pol_nbr_txt, ams.st_id ascending
             select new LossInfoBE()
             {
                 ARMIS_LOS_ID = ams.armis_los_pol_id,
                 //POLICYSYMBOL = ams.COML_AGMT.pol_sym_txt,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(ca.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 POLICY = ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim(),
                 COML_AGMT_ID = ams.coml_agmt_id,
                 ST_ID = ams.st_id,
                 SUPRT_SERV_CUSTMR_GP_ID = ams.suprt_serv_custmr_gp_id,
                 PAID_IDNMTY_AMT = ams.paid_idnmty_amt,
                 PAID_EXPS_AMT = ams.paid_exps_amt,
                 RESRV_IDNMTY_AMT = ams.resrv_idnmty_amt,
                 RESRV_EXPS_AMT = ams.resrv_exps_amt,
                 SYS_GENRT_IND = ams.sys_genrt_ind,
                 VALN_DATE = ams.valn_dt,
                 CUSTMR_ID = ams.custmr_id,
                 ACTV_IND = ams.actv_ind,
                 PREM_ADJ_ID = ams.prem_adj_id,
                 UPDATEDDATE = ams.updt_dt,
                 ADJ_STATUS = (from pap in this.Context.PREM_ADJ_STs
                               join lkup in this.Context.LKUPs
                               on pap.adj_sts_typ_id equals lkup.lkup_id
                               orderby pap.prem_adj_sts_id descending
                               where pap.prem_adj_id == ams.prem_adj_id && pap.prem_adj_id == adjNo
                               select lkup.lkup_txt).First().ToString(),
                 Incurred = ams.paid_idnmty_amt + ams.paid_exps_amt + ams.resrv_idnmty_amt + ams.resrv_exps_amt,
                 EXC_NON_BIL = (from aldet in this.Context.ARMIS_LOS_EXCs
                                where aldet.armis_los_pol_id == ams.armis_los_pol_id
                                select "Yes").First().ToString()


             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all loss Info that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoDataHideDisLines(DateTime ValDate, int custmrID, int prgID, bool flag)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true 
             orderby ca.pol_sym_txt, ca.pol_nbr_txt, ams.st_id ascending
             select new LossInfoBE()
             {
                 ARMIS_LOS_ID = ams.armis_los_pol_id,
                 //POLICYSYMBOL = ams.COML_AGMT.pol_sym_txt,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(ca.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 POLICY = ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim(),
                 COML_AGMT_ID = ams.coml_agmt_id,
                 ST_ID = ams.st_id,
                 SUPRT_SERV_CUSTMR_GP_ID = ams.suprt_serv_custmr_gp_id,
                 PAID_IDNMTY_AMT = ams.paid_idnmty_amt,
                 PAID_EXPS_AMT = ams.paid_exps_amt,
                 RESRV_IDNMTY_AMT = ams.resrv_idnmty_amt,
                 RESRV_EXPS_AMT = ams.resrv_exps_amt,
                 SYS_GENRT_IND = ams.sys_genrt_ind,
                 CUSTMR_ID = ams.custmr_id,
                 ACTV_IND = ams.actv_ind,
                 PREM_ADJ_ID = ams.prem_adj_id,
                 UPDATEDDATE = ams.updt_dt,
                 ADJ_STATUS = (from pap in this.Context.PREM_ADJ_STs
                               join lkup in this.Context.LKUPs
                               on pap.adj_sts_typ_id equals lkup.lkup_id
                               orderby pap.prem_adj_sts_id descending
                               where pap.prem_adj_id == ams.prem_adj_id
                               select lkup.lkup_txt).First().ToString(),
                 Incurred = ams.paid_idnmty_amt + ams.paid_exps_amt + ams.resrv_idnmty_amt + ams.resrv_exps_amt,
                 EXC_NON_BIL = (from aldet in this.Context.ARMIS_LOS_EXCs
                                where aldet.armis_los_pol_id == ams.armis_los_pol_id
                                select "Yes").First().ToString()


             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoByPolicyData(DateTime ValDate, int custmrID, int prgID)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == null
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, ca.coml_agmt_id } into grouping
             select new LossInfoBE()
             {
                 ACTV_IND = grouping.Key.actv_ind,
                 COML_AGMT_ID = grouping.Key.coml_agmt_id,
                 PREM_ADJ_ID = grouping.First().prem_adj_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 PAID_IDNMTY_AMT = grouping.Sum(ams => ams.paid_idnmty_amt),
                 PAID_EXPS_AMT = grouping.Sum(ams => ams.paid_exps_amt),
                 RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.resrv_idnmty_amt),
                 RESRV_EXPS_AMT = grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt),
                 NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_paid_exps_amt),
                 NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt),
                 NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.subj_paid_idnmty_amt),
                 SUBJ_PAID_EXPS_AMT = grouping.Sum(ams => ams.subj_paid_exps_amt),
                 SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.subj_resrv_idnmty_amt),
                 SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.exc_paid_idnmty_amt),
                 EXC_PAID_EXPS_AMT = grouping.Sum(ams => ams.exc_paid_exps_amt),
                 EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.exc_resrvd_idnmty_amt),
                 EXC_RESRV_EXPS_AMT = grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                 POLICY = grouping.First().COML_AGMT.pol_sym_txt.Trim() + grouping.First().COML_AGMT.pol_nbr_txt.Trim() + grouping.First().COML_AGMT.pol_modulus_txt.Trim(),
                 LIMIT2_AMT = (from en in this.Context.ARMIS_LOS_EXCs
                               where grouping.First().armis_los_pol_id == en.armis_los_pol_id
                               select en.lim2_amt).First(),
                 POLICY_AMT = grouping.First().COML_AGMT.dedtbl_pol_lim_amt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where grouping.First().COML_AGMT.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString()
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }


        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoByPolicyDataAdjNo(DateTime ValDate, int custmrID, int prgID, int AdjNo)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id 
             where  ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.prem_adj_id == AdjNo  && ams.actv_ind==true
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind,ca.coml_agmt_id } into grouping
             select new LossInfoBE()
             {
                ACTV_IND = grouping.Key.actv_ind,
                COML_AGMT_ID=grouping.Key.coml_agmt_id,
                PREM_ADJ_ID= grouping.First().prem_adj_id,
                POLICYSYMBOL = (from luk in this.Context.LKUPs
                                where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                select luk.attr_1_txt).First().ToString(),
                PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.paid_idnmty_amt),
                PAID_EXPS_AMT = grouping.Sum(ams =>ams.paid_exps_amt),
                RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.resrv_idnmty_amt),
                RESRV_EXPS_AMT = grouping.Sum(ams =>ams.resrv_exps_amt),
                NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.non_bilabl_paid_idnmty_amt),
                NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams =>ams.non_bilabl_paid_exps_amt),
                NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.non_bilabl_resrv_idnmty_amt),
                NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.non_bilabl_resrv_exps_amt),
                SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.subj_paid_idnmty_amt),
                SUBJ_PAID_EXPS_AMT = grouping.Sum(ams =>ams.subj_paid_exps_amt),
                SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.subj_resrv_idnmty_amt),
                SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.subj_resrv_exps_amt),
                EXC_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.exc_paid_idnmty_amt),
                EXC_PAID_EXPS_AMT = grouping.Sum(ams =>ams.exc_paid_exps_amt),
                EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.exc_resrvd_idnmty_amt),
                EXC_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.exc_resrv_exps_amt),
                TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                POLICY = grouping.First().COML_AGMT.pol_sym_txt.Trim() + grouping.First().COML_AGMT.pol_nbr_txt.Trim() + grouping.First().COML_AGMT.pol_modulus_txt.Trim(),
                LIMIT2_AMT = (from en in this.Context.ARMIS_LOS_EXCs
                               where grouping.First().armis_los_pol_id == en.armis_los_pol_id
                               select en.lim2_amt).First(),
                 POLICY_AMT = grouping.First().COML_AGMT.dedtbl_pol_lim_amt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where grouping.First().COML_AGMT.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString()
               
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoDataAdjNoComl(DateTime ValDate, int custmrID, int prgID, int AdjNo, int Coml)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ca.coml_agmt_id == Coml && ams.prem_adj_id==AdjNo
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, ca.coml_agmt_id } into grouping
             select new LossInfoBE()
             {
                 ACTV_IND = grouping.Key.actv_ind,
                 COML_AGMT_ID = grouping.Key.coml_agmt_id,
                 PREM_ADJ_ID = grouping.First().prem_adj_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 PAID_IDNMTY_AMT = grouping.Sum(ams => ams.paid_idnmty_amt),
                 PAID_EXPS_AMT = grouping.Sum(ams => ams.paid_exps_amt),
                 RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.resrv_idnmty_amt),
                 RESRV_EXPS_AMT = grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt),
                 NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_paid_exps_amt),
                 NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt),
                 NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.subj_paid_idnmty_amt),
                 SUBJ_PAID_EXPS_AMT = grouping.Sum(ams => ams.subj_paid_exps_amt),
                 SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.subj_resrv_idnmty_amt),
                 SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.exc_paid_idnmty_amt),
                 EXC_PAID_EXPS_AMT = grouping.Sum(ams => ams.exc_paid_exps_amt),
                 EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.exc_resrvd_idnmty_amt),
                 EXC_RESRV_EXPS_AMT = grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                 POLICY = grouping.First().COML_AGMT.pol_sym_txt.Trim() + grouping.First().COML_AGMT.pol_nbr_txt.Trim() + grouping.First().COML_AGMT.pol_modulus_txt.Trim(),
                 LIMIT2_AMT = (from en in this.Context.ARMIS_LOS_EXCs
                               where grouping.First().armis_los_pol_id == en.armis_los_pol_id
                               select en.lim2_amt).First(),
                 POLICY_AMT = grouping.First().COML_AGMT.dedtbl_pol_lim_amt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where grouping.First().COML_AGMT.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString()
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoDataComl(DateTime ValDate, int custmrID, int prgID, int Coml)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ca.coml_agmt_id == Coml && ams.prem_adj_id == null
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, ca.coml_agmt_id } into grouping
             select new LossInfoBE()
             {
                 ACTV_IND = grouping.Key.actv_ind,
                COML_AGMT_ID=grouping.Key.coml_agmt_id,
                PREM_ADJ_ID= grouping.First().prem_adj_id,
                POLICYSYMBOL = (from luk in this.Context.LKUPs
                                where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                select luk.attr_1_txt).First().ToString(),
                PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.paid_idnmty_amt),
                PAID_EXPS_AMT = grouping.Sum(ams =>ams.paid_exps_amt),
                RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.resrv_idnmty_amt),
                RESRV_EXPS_AMT = grouping.Sum(ams =>ams.resrv_exps_amt),
                NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.non_bilabl_paid_idnmty_amt),
                NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams =>ams.non_bilabl_paid_exps_amt),
                NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.non_bilabl_resrv_idnmty_amt),
                NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.non_bilabl_resrv_exps_amt),
                SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.subj_paid_idnmty_amt),
                SUBJ_PAID_EXPS_AMT = grouping.Sum(ams =>ams.subj_paid_exps_amt),
                SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.subj_resrv_idnmty_amt),
                SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.subj_resrv_exps_amt),
                EXC_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.exc_paid_idnmty_amt),
                EXC_PAID_EXPS_AMT = grouping.Sum(ams =>ams.exc_paid_exps_amt),
                EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.exc_resrvd_idnmty_amt),
                EXC_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.exc_resrv_exps_amt),
                TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                POLICY = grouping.First().COML_AGMT.pol_sym_txt.Trim() + grouping.First().COML_AGMT.pol_nbr_txt.Trim() + grouping.First().COML_AGMT.pol_modulus_txt.Trim(),
                LIMIT2_AMT = (from en in this.Context.ARMIS_LOS_EXCs
                               where grouping.First().armis_los_pol_id == en.armis_los_pol_id
                               select en.lim2_amt).First(),
                 POLICY_AMT = grouping.First().COML_AGMT.dedtbl_pol_lim_amt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where grouping.First().COML_AGMT.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString()
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoByLOBDataAdjNo(DateTime ValDate, int custmrID, int prgID, int AdjNo)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id 
             join luk in this.Context.LKUPs
             on ca.covg_typ_id equals luk.lkup_id
             where  ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.prem_adj_id == AdjNo  && ams.actv_ind==true
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind,luk.attr_1_txt} into grouping
             select new LossInfoBE()
             {
                ACTV_IND = grouping.Key.actv_ind,
                COVG_ID = 1,//grouping.Key.covg_typ_id,
                PREM_ADJ_ID= grouping.First().prem_adj_id,
                POLICYSYMBOL = (from luk in this.Context.LKUPs
                                where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                select luk.attr_1_txt).First().ToString(),
                PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.paid_idnmty_amt),
                PAID_EXPS_AMT = grouping.Sum(ams =>ams.paid_exps_amt),
                RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.resrv_idnmty_amt),
                RESRV_EXPS_AMT = grouping.Sum(ams =>ams.resrv_exps_amt),
                NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.non_bilabl_paid_idnmty_amt),
                NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams =>ams.non_bilabl_paid_exps_amt),
                NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.non_bilabl_resrv_idnmty_amt),
                NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.non_bilabl_resrv_exps_amt),
                SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.subj_paid_idnmty_amt),
                SUBJ_PAID_EXPS_AMT = grouping.Sum(ams =>ams.subj_paid_exps_amt),
                SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.subj_resrv_idnmty_amt),
                SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.subj_resrv_exps_amt),
                EXC_PAID_IDNMTY_AMT = grouping.Sum(ams =>ams.exc_paid_idnmty_amt),
                EXC_PAID_EXPS_AMT = grouping.Sum(ams =>ams.exc_paid_exps_amt),
                EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams =>ams.exc_resrvd_idnmty_amt),
                EXC_RESRV_EXPS_AMT = grouping.Sum(ams =>ams.exc_resrv_exps_amt),
                TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt)
                 
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoByLOBDataAdjNoCovg(DateTime ValDate, int custmrID, int prgID, int AdjNo, string Lob)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             join luk in this.Context.LKUPs
             on ca.covg_typ_id equals luk.lkup_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.prem_adj_id == AdjNo && ams.actv_ind == true //&&  ca.covg_typ_id==Covg
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, luk.attr_1_txt } into grouping
             select new LossInfoBE()
             {
                 ACTV_IND = grouping.Key.actv_ind,
                 COVG_ID = 1,//grouping.Key.covg_typ_id,
                 PREM_ADJ_ID = grouping.First().prem_adj_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 PAID_IDNMTY_AMT = grouping.Sum(ams => ams.paid_idnmty_amt),
                 PAID_EXPS_AMT = grouping.Sum(ams => ams.paid_exps_amt),
                 RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.resrv_idnmty_amt),
                 RESRV_EXPS_AMT = grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt),
                 NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_paid_exps_amt),
                 NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt),
                 NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.subj_paid_idnmty_amt),
                 SUBJ_PAID_EXPS_AMT = grouping.Sum(ams => ams.subj_paid_exps_amt),
                 SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.subj_resrv_idnmty_amt),
                 SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.exc_paid_idnmty_amt),
                 EXC_PAID_EXPS_AMT = grouping.Sum(ams => ams.exc_paid_exps_amt),
                 EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.exc_resrvd_idnmty_amt),
                 EXC_RESRV_EXPS_AMT = grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt)

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            query = query.Where(pol => pol.POLICYSYMBOL == Lob);
            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoByLOBData(DateTime ValDate, int custmrID, int prgID)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             join luk in this.Context.LKUPs
             on ca.covg_typ_id equals luk.lkup_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == null 
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, luk.attr_1_txt } into grouping
             select new LossInfoBE()
             {
                 ACTV_IND = grouping.Key.actv_ind,
                 COVG_ID = 1,//grouping.Key.covg_typ_id,
                 PREM_ADJ_ID = grouping.First().prem_adj_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 PAID_IDNMTY_AMT = grouping.Sum(ams => ams.paid_idnmty_amt),
                 PAID_EXPS_AMT = grouping.Sum(ams => ams.paid_exps_amt),
                 RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.resrv_idnmty_amt),
                 RESRV_EXPS_AMT = grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt),
                 NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_paid_exps_amt),
                 NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt),
                 NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.subj_paid_idnmty_amt),
                 SUBJ_PAID_EXPS_AMT = grouping.Sum(ams => ams.subj_paid_exps_amt),
                 SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.subj_resrv_idnmty_amt),
                 SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.exc_paid_idnmty_amt),
                 EXC_PAID_EXPS_AMT = grouping.Sum(ams => ams.exc_paid_exps_amt),
                 EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.exc_resrvd_idnmty_amt),
                 EXC_RESRV_EXPS_AMT = grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt)

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind Master Listview in LossesReport webPage
        /// </summary>
        /// <param name="startEffDate"></param>
        /// <param name="endEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoByLOBDataCovg(DateTime ValDate, int custmrID, int prgID, string Lob)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             join luk in this.Context.LKUPs
             on ca.covg_typ_id equals luk.lkup_id
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == null
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, luk.attr_1_txt } into grouping
             select new LossInfoBE()
             {
                 ACTV_IND = grouping.Key.actv_ind,
                 COVG_ID = 1,//grouping.Key.covg_typ_id,
                 PREM_ADJ_ID = grouping.First().prem_adj_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(grouping.First().COML_AGMT.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 PAID_IDNMTY_AMT = grouping.Sum(ams => ams.paid_idnmty_amt),
                 PAID_EXPS_AMT = grouping.Sum(ams => ams.paid_exps_amt),
                 RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.resrv_idnmty_amt),
                 RESRV_EXPS_AMT = grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt),
                 NON_BILABL_PAID_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_paid_exps_amt),
                 NON_BILABL_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt),
                 NON_BILABL_RESRV_EXPS_AMT = grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.subj_paid_idnmty_amt),
                 SUBJ_PAID_EXPS_AMT = grouping.Sum(ams => ams.subj_paid_exps_amt),
                 SUBJ_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.subj_resrv_idnmty_amt),
                 SUBJ_RESRV_EXPS_AMT = grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_PAID_IDNMTY_AMT = grouping.Sum(ams => ams.exc_paid_idnmty_amt),
                 EXC_PAID_EXPS_AMT = grouping.Sum(ams => ams.exc_paid_exps_amt),
                 EXC_RESRV_IDNMTY_AMT = grouping.Sum(ams => ams.exc_resrvd_idnmty_amt),
                 EXC_RESRV_EXPS_AMT = grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 TOTAL_INCURRED = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt),
                 NON_BILABL_INCURRED = grouping.Sum(ams => ams.non_bilabl_paid_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_paid_exps_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_idnmty_amt) + grouping.Sum(ams => ams.non_bilabl_resrv_exps_amt),
                 SUBJ_INCURRED = grouping.Sum(ams => ams.subj_paid_idnmty_amt) + grouping.Sum(ams => ams.subj_paid_exps_amt) + grouping.Sum(ams => ams.subj_resrv_idnmty_amt) + grouping.Sum(ams => ams.subj_resrv_exps_amt),
                 EXC_INCURRED = grouping.Sum(ams => ams.exc_paid_idnmty_amt) + grouping.Sum(ams => ams.exc_paid_exps_amt) + grouping.Sum(ams => ams.exc_resrvd_idnmty_amt) + grouping.Sum(ams => ams.exc_resrv_exps_amt),
                 Incurred = grouping.Sum(ams => ams.paid_idnmty_amt) + grouping.Sum(ams => ams.paid_exps_amt) + grouping.Sum(ams => ams.resrv_idnmty_amt) + grouping.Sum(ams => ams.resrv_exps_amt)

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            query = query.Where(pol => pol.POLICYSYMBOL == Lob);
            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all loss Info that match criteria,this method is used to bind child Listview in LossesReport WebPage
        /// </summary>
        /// <param name="intArmisID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoData(int intArmisID)
        {
            IList<LossInfoBE> result = new List<LossInfoBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve loss information
            /// and project it into Loss Info Business Entity
            IQueryable<LossInfoBE> query =
            (from ams in this.Context.ARMIS_LOS_POLs
             join ca in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals ca.coml_agmt_id
             where ams.armis_los_pol_id == intArmisID
             select new LossInfoBE()
             {
                 ARMIS_LOS_ID = ams.armis_los_pol_id,
                 POLICYSYMBOL = (from luk in this.Context.LKUPs
                                 where luk.lkup_id == Convert.ToInt32(ca.covg_typ_id)
                                 select luk.attr_1_txt).First().ToString(),
                 POLICY = ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim(),
                 COML_AGMT_ID = ams.coml_agmt_id,
                 ST_ID = ams.st_id,
                 PREM_ADJ_ID=ams.prem_adj_id,
                 SUPRT_SERV_CUSTMR_GP_ID = ams.suprt_serv_custmr_gp_id,
                 PAID_IDNMTY_AMT = ams.paid_idnmty_amt,
                 PAID_EXPS_AMT = ams.paid_exps_amt,
                 RESRV_IDNMTY_AMT = ams.resrv_idnmty_amt,
                 RESRV_EXPS_AMT = ams.resrv_exps_amt,
                 NON_BILABL_PAID_IDNMTY_AMT = ams.non_bilabl_paid_idnmty_amt,
                 NON_BILABL_PAID_EXPS_AMT = ams.non_bilabl_paid_exps_amt,
                 NON_BILABL_RESRV_IDNMTY_AMT = ams.non_bilabl_resrv_idnmty_amt,
                 NON_BILABL_RESRV_EXPS_AMT = ams.non_bilabl_resrv_exps_amt,
                 SUBJ_PAID_IDNMTY_AMT = ams.subj_paid_idnmty_amt,
                 SUBJ_PAID_EXPS_AMT = ams.subj_paid_exps_amt,
                 SUBJ_RESRV_IDNMTY_AMT = ams.subj_resrv_idnmty_amt,
                 SUBJ_RESRV_EXPS_AMT = ams.subj_resrv_exps_amt,
                 EXC_PAID_IDNMTY_AMT = ams.exc_paid_idnmty_amt,
                 EXC_PAID_EXPS_AMT = ams.exc_paid_exps_amt,
                 EXC_RESRV_IDNMTY_AMT = ams.exc_resrvd_idnmty_amt,
                 EXC_RESRV_EXPS_AMT = ams.exc_resrv_exps_amt,
                 SYS_GENRT_IND = ams.sys_genrt_ind,
                 CUSTMR_ID = ams.custmr_id,
                 ACTV_IND = ams.actv_ind,
                 TOTAL_INCURRED = (ams.paid_idnmty_amt==null?0:ams.paid_idnmty_amt) + (ams.paid_exps_amt==null?0:ams.paid_exps_amt) + (ams.resrv_idnmty_amt==null?0:ams.resrv_idnmty_amt) + (ams.resrv_exps_amt==null?0:ams.resrv_exps_amt),
                 NON_BILABL_INCURRED = (ams.non_bilabl_paid_idnmty_amt==null ? 0: ams.non_bilabl_paid_idnmty_amt) + (ams.non_bilabl_paid_exps_amt==null ?0:ams.non_bilabl_paid_exps_amt) + (ams.non_bilabl_resrv_exps_amt==null?0:ams.non_bilabl_resrv_exps_amt) + (ams.non_bilabl_resrv_idnmty_amt==null?0:ams.non_bilabl_resrv_idnmty_amt),
                 SUBJ_INCURRED = (ams.subj_paid_idnmty_amt==null? 0: ams.subj_paid_idnmty_amt) + (ams.subj_paid_exps_amt==null? 0: ams.subj_paid_exps_amt) + (ams.subj_resrv_exps_amt==null ? 0 :ams.subj_resrv_exps_amt) + (ams.subj_resrv_idnmty_amt==null ? 0 :ams.subj_resrv_idnmty_amt),
                 EXC_INCURRED = (ams.exc_paid_idnmty_amt==null? 0 :ams.exc_paid_idnmty_amt) + (ams.exc_paid_exps_amt==null ? 0 : ams.exc_paid_exps_amt) + (ams.exc_resrv_exps_amt==null ? 0: ams.exc_resrv_exps_amt)  + (ams.exc_resrvd_idnmty_amt==null ? 0 :ams.exc_resrvd_idnmty_amt),
                 ADJ_STATUS = (from pap in this.Context.PREM_ADJ_STs
                               join lkup in this.Context.LKUPs
                               on pap.adj_sts_typ_id equals lkup.lkup_id
                               orderby pap.prem_adj_sts_id descending
                               where pap.prem_adj_id == ams.prem_adj_id 
                               select lkup.lkup_txt).First().ToString(),
                 Incurred = ams.paid_idnmty_amt + ams.paid_exps_amt + ams.resrv_idnmty_amt + ams.resrv_exps_amt,
                 EXC_NON_BIL = (from aldet in this.Context.ARMIS_LOS_EXCs
                                where aldet.armis_los_pol_id == ams.armis_los_pol_id
                                select "Yes").First().ToString(),
                 POLICY_AMT = ca.dedtbl_pol_lim_amt,
                 LIMIT2_AMT = (from en in this.Context.ARMIS_LOS_EXCs
                               where ams.armis_los_pol_id == en.armis_los_pol_id
                               select en.lim2_amt).First(),
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where ca.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString(),
                 UPDATEDDATE = ams.updt_dt,
                 UPDATEDUSER = ams.updt_user_id
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }


        public int? GetAdjustmentNumber(int custmr_id, DateTime valn_dt)
        {
            int? AdjNo = null;
            IList<int> list = null;

            if (this.Context == null)
                this.CreateContext();

            string strQry = "select top 1 pa.prem_adj_id from prem_adj_perd pap inner join prem_adj pa "+ 
                            " on pap.prem_adj_id = pa.prem_adj_id where custmr_id="+custmr_id.ToString()
                            +" and valn_dt = '"+valn_dt.ToShortDateString()+"' and adj_sts_typ_id=346";

            list = this.Context.ExecuteQuery<int>(strQry).ToList();
            if (list.Count() > 0)
                AdjNo = list.First();
            return AdjNo;
        }
    }
}
