using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data;

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
                 COPY_IND = ams.copy_ind,
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
                               where pap.prem_adj_id == ams.prem_adj_id && pap.prem_adj_id == adjNo
                               select lkup.lkup_txt).First().ToString(),
                 Incurred = ams.paid_idnmty_amt + ams.paid_exps_amt + ams.resrv_idnmty_amt + ams.resrv_exps_amt,
                 EXC_NON_BIL = (from aldet in this.Context.ARMIS_LOS_EXCs
                                where aldet.armis_los_pol_id == ams.armis_los_pol_id
                                select "Yes").First().ToString(),
                 UPDATEDDATE = ams.updt_dt,
                 UPDATEDUSER = ams.updt_user_id,
                 COPY_IND = ams.copy_ind
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
                 //added as per the general error bug,as we are not receiving val date previously while filtering it is giving error
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
                 VALN_DATE=ams.valn_dt,
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
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == null && ams.valn_dt== ValDate
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, ca.coml_agmt_id, ams.valn_dt  } into grouping
             select new LossInfoBE()
             {
                 VALN_DATE = grouping.Key.valn_dt,
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
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ca.coml_agmt_id == Coml && ams.prem_adj_id == null && ams.valn_dt== ValDate
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
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == null && ams.valn_dt == ValDate
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, luk.attr_1_txt, ams.valn_dt } into grouping
             select new LossInfoBE()
             {
                 VALN_DATE = grouping.Key.valn_dt,
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
             where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == null && ams.valn_dt== ValDate
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


        public IList<LossInfoBE> getLossInfoDetails(string valDate,int adjNo, int prgID, string comlgIDs,int sysGen)
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
             join pgm in this.Context.PREM_ADJ_PGMs
             on ams.prem_adj_pgm_id equals pgm.prem_adj_pgm_id
             //where ca.custmr_id == custmrID && ca.prem_adj_pgm_id == prgID && ams.actv_ind == true && ams.prem_adj_id == null && ams.valn_dt == ValDate
             orderby (ams.COML_AGMT.pol_sym_txt.Trim() + ams.COML_AGMT.pol_nbr_txt.Trim() + ams.COML_AGMT.pol_modulus_txt.Trim()), ams.COML_AGMT.pol_sym_txt.Trim() ascending
             group ams by new { ams.actv_ind, ca.coml_agmt_id, ams.valn_dt,pgm.prem_adj_pgm_id } into grouping
             select new LossInfoBE()
             {
                 VALN_DATE = grouping.Key.valn_dt,
                 ACTV_IND = grouping.Key.actv_ind,
                 COML_AGMT_ID = grouping.Key.coml_agmt_id,
                 PREM_ADJ_ID = grouping.First().prem_adj_id,
                 PREM_ADJ_PGM_ID = grouping.First().prem_adj_pgm_id,
                 SYS_GENRT_IND = grouping.First().sys_genrt_ind,
                 CUSTMR_ID =grouping.First().custmr_id,
                 PGM_PRD_STRT_DT = grouping.First().COML_AGMT.PREM_ADJ_PGM.strt_dt,
                 PGM_PRD_END_DT = grouping.First().COML_AGMT.PREM_ADJ_PGM.plan_end_dt,
                 PGM_TYP_ID = grouping.First().COML_AGMT.PREM_ADJ_PGM.pgm_typ_id,
                 POL_STRT_DT = grouping.First().COML_AGMT.pol_eff_dt,
                 POL_END_DT = grouping.First().COML_AGMT.planned_end_date,
                 ST_ID = grouping.First().st_id ,
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
                 POLICY = grouping.First().COML_AGMT.pol_sym_txt.Trim() + ' ' + grouping.First().COML_AGMT.pol_nbr_txt.Trim() + ' ' + grouping.First().COML_AGMT.pol_modulus_txt.Trim(),
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
            if (valDate != "")
            {
                query = query.Where(pg => Convert.ToDateTime(pg.VALN_DATE) == Convert.ToDateTime(valDate));
            }
            //if (adjNo > 0)
            //{
            //    query = query.Where(pg => pg.PREM_ADJ_ID == adjNo);
            //}
            if (prgID > 0)
            {
                query = query.Where(pg => pg.PREM_ADJ_PGM_ID == prgID);
            }
            if (sysGen == 0 || sysGen == 1)
            {
                query = query.Where(pg => pg.SYS_GENRT_IND == Convert.ToBoolean(sysGen));
            }
            //if (!string.IsNullOrWhiteSpace(comlgIDs))
            //{                
            //    string[] str= comlgIDs.Split(',');
            //    query = query.Where(pg => str.Contains(pg.COML_AGMT_ID.ToString()));
            //}
            result = query.ToList();
            return result;
        }


        public DataTable GetArmisLossDetails(string valDate, string adjNo, string prgID, string comlgIDs, string sysGen, string custmrID)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandText = "GetARMSLossesDetails";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Connection = conn;
            sqlCmd.Parameters.AddWithValue("@VALDT", valDate);
            sqlCmd.Parameters.AddWithValue("@ADJNO", adjNo);
            sqlCmd.Parameters.AddWithValue("@PREM_ADJ_PGM_ID", prgID);
            sqlCmd.Parameters.AddWithValue("@COML_AGM_IDS", comlgIDs);
            sqlCmd.Parameters.AddWithValue("@SYS_GEN", sysGen);
            sqlCmd.Parameters.AddWithValue("@CUSTMR_ID", custmrID);
            da.SelectCommand = sqlCmd;
            da.Fill(dt);
            return dt;
        }

        public DataTable GetExcessLossDetails(string valDate, string adjNo, string prgID, string comlgIDs, string sysGen, string custmrID)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandText = "GetExcessLossesDetails";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Connection = conn;
            sqlCmd.Parameters.AddWithValue("@VALDT", valDate);
            sqlCmd.Parameters.AddWithValue("@ADJNO", adjNo);
            sqlCmd.Parameters.AddWithValue("@PREM_ADJ_PGM_ID", prgID);
            sqlCmd.Parameters.AddWithValue("@COML_AGM_IDS", comlgIDs);
            sqlCmd.Parameters.AddWithValue("@SYS_GEN", sysGen);
            sqlCmd.Parameters.AddWithValue("@CUSTMR_ID", custmrID);
            da.SelectCommand = sqlCmd;
            da.Fill(dt);
            return dt;
        }
        private static void OnSqlRowsTransfer(object sender, SqlRowsCopiedEventArgs e)
        {
            //Console.WriteLine("Copied {0} so far...", e.RowsCopied);
        }

        public bool LossesUploadStage(DataTable dt)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            con.Open();
            using (SqlBulkCopy bulkCopy =
                 new SqlBulkCopy(con))
            {
                bulkCopy.SqlRowsCopied +=
                    new SqlRowsCopiedEventHandler(OnSqlRowsTransfer);
                bulkCopy.NotifyAfter = 100;
                bulkCopy.BatchSize = 100;
                bulkCopy.ColumnMappings.Add("Valuation Date", "Valuation_Date");
                bulkCopy.ColumnMappings.Add("LOB", "LOB");
                bulkCopy.ColumnMappings.Add("Policy No", "POLICY_NO");
                bulkCopy.ColumnMappings.Add("Customer ID", "CUSTMR_ID");
                bulkCopy.ColumnMappings.Add("Program Period Eff Date", "PGM_EFF_DT");
                bulkCopy.ColumnMappings.Add("Program Period Exp Date", "PGM_EXP_DT");
                bulkCopy.ColumnMappings.Add("Program Type", "PGM_TYPE");
                bulkCopy.ColumnMappings.Add("State", "STATE");
                bulkCopy.ColumnMappings.Add("Policy Eff Date", "POL_EFF_DT");
                bulkCopy.ColumnMappings.Add("Policy Exp Date", "POL_EXP_DT");
                bulkCopy.ColumnMappings.Add("SCGID", "SCGID");
                bulkCopy.ColumnMappings.Add("Total Paid Indemnity", "PAID_IDNMTY_AMT");
                bulkCopy.ColumnMappings.Add("Total Paid Expense", "PAID_EXPS_AMT");
                bulkCopy.ColumnMappings.Add("Total Reserved Indemnity", "RESRV_IDNMTY_AMT");
                bulkCopy.ColumnMappings.Add("Total Reserved Expense", "RESRV_EXPS_AMT");
                bulkCopy.ColumnMappings.Add("System Generated", "SYS_GENRT_IND");
                bulkCopy.ColumnMappings.Add("crte_usr_id", "CRTE_USER_ID");
                bulkCopy.ColumnMappings.Add("crte_dt", "CRTE_DT");
                bulkCopy.ColumnMappings.Add("validate", "VALIDATE");
                bulkCopy.DestinationTableName = "dbo.LOSS_INFO_COPY_STAGE_POL";
                bulkCopy.WriteToServer(dt);
            }
            return true;
        }

        public bool ExcessLossesUploadStage(DataTable dt)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            con.Open();
            using (SqlBulkCopy bulkCopy =
                 new SqlBulkCopy(con))
            {
                bulkCopy.SqlRowsCopied +=
                    new SqlRowsCopiedEventHandler(OnSqlRowsTransfer);
                bulkCopy.NotifyAfter = 100;
                bulkCopy.BatchSize = 100;
                bulkCopy.ColumnMappings.Add("Valuation Date", "Valuation_Date");
                bulkCopy.ColumnMappings.Add("LOB", "LOB");
                bulkCopy.ColumnMappings.Add("Policy No", "POLICY_NO");
                bulkCopy.ColumnMappings.Add("Customer ID", "CUSTMR_ID");
                bulkCopy.ColumnMappings.Add("Program Period Eff Date", "PGM_EFF_DT");
                bulkCopy.ColumnMappings.Add("Program Period Exp Date", "PGM_EXP_DT");
                bulkCopy.ColumnMappings.Add("Program Type", "PGM_TYPE");
                bulkCopy.ColumnMappings.Add("State", "STATE");
                bulkCopy.ColumnMappings.Add("Policy Eff Date", "POL_EFF_DT");
                bulkCopy.ColumnMappings.Add("Policy Exp Date", "POL_EXP_DT");
                bulkCopy.ColumnMappings.Add("Claim No", "CLM_NBR_TXT");
                bulkCopy.ColumnMappings.Add("Additional Claim Ind", "ADDN_CLM_IND");
                bulkCopy.ColumnMappings.Add("Additional Claim", "ADDN_CLM_TXT");
                bulkCopy.ColumnMappings.Add("Claimant Name", "CLMT_NM");
                bulkCopy.ColumnMappings.Add("Claim Status", "CLM_STS_ID");
                bulkCopy.ColumnMappings.Add("Coverage Trigger Date", "COVG_TRIGR_DT");
                bulkCopy.ColumnMappings.Add("Limit 2", "LIM2_AMT");
                bulkCopy.ColumnMappings.Add("Total Paid Indemnity", "LOS_PAID_IDNMTY_AMT");
                bulkCopy.ColumnMappings.Add("Total Paid Expense", "LOS_PAID_EXPS_AMT");
                bulkCopy.ColumnMappings.Add("Total Reserved Indemnity", "LOS_RESRV_IDNMTY_AMT");
                bulkCopy.ColumnMappings.Add("Total Reserved Expense", "LOS_RESRV_EXPS_AMT");
                bulkCopy.ColumnMappings.Add("NonBillable Paid Indemnity", "NON_BILABL_PAID_IDNMTY_AMT");
                bulkCopy.ColumnMappings.Add("NonBillable Paid Expense", "NON_BILABL_PAID_EXPS_AMT");
                bulkCopy.ColumnMappings.Add("NonBillable Reserved Indemnity", "NON_BILABL_RESRVD_IDNMTY_AMT");
                bulkCopy.ColumnMappings.Add("NonBillable Reserved Expense", "NON_BILABL_RESRVD_EXPS_AMT");
                bulkCopy.ColumnMappings.Add("System Generated", "LOS_SYS_GENRT_IND");
                bulkCopy.ColumnMappings.Add("crte_usr_id", "CRTE_USER_ID");
                bulkCopy.ColumnMappings.Add("crte_dt", "CRTE_DT");
                bulkCopy.ColumnMappings.Add("validate", "VALIDATE");
                bulkCopy.DestinationTableName = "dbo.LOSS_INFO_COPY_STAGE_EXEC";
                bulkCopy.WriteToServer(dt);
            }
            return true;
        }

        public string ModAISLossInfoCopyStageUploads(string userID, DateTime dtUploadDateTime)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandText = "ModAIS_Process_Copy_Losses_Policy_Upload";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Connection = conn;
            sqlCmd.Parameters.AddWithValue("@create_user_id", userID);
            sqlCmd.Parameters.AddWithValue("@dtUploadDateTime", dtUploadDateTime);
            sqlCmd.Parameters.AddWithValue("@debug", 0); 
            SqlParameter parmErr = new SqlParameter("@err_msg_op", SqlDbType.VarChar);
            parmErr.Direction = ParameterDirection.Output;
            parmErr.Size = -1;
            sqlCmd.Parameters.Add(parmErr);
            conn.Open();
            sqlCmd.ExecuteNonQuery();
            conn.Close();
            return Convert.ToString(sqlCmd.Parameters["@err_msg_op"].Value);
        }

        public DataTable LossesUploadsError(string userID, DateTime dtUploadDateTime)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            SqlCommand sqlCmd = new SqlCommand();
            string strCmd = @"select Valuation_Date [Valuation Date],[LOB] LOB,[POLICY_NO] 'Policy No',[CUSTMR_ID] 'Customer ID'
                              ,PGM_EFF_DT 'Program Period Eff Date',PGM_EXP_DT 'Program Period Exp Date'
                              ,[PGM_TYPE] 'Program Type',[STATE] 'State',POL_EFF_DT 'Policy Eff Date'
                              ,POL_EXP_DT 'Policy Exp Date',[SCGID] SCGID,[PAID_IDNMTY_AMT] 'Total Paid Indemnity'
                              ,[PAID_EXPS_AMT] 'Total Paid Expense',[RESRV_IDNMTY_AMT] 'Total Reserved Indemnity'
                              ,[RESRV_EXPS_AMT] 'Total Reserved Expense',[SYS_GENRT_IND] 'System Generated'     
                              ,[TXTERRORDESC] 'Comments'  from LOSS_INFO_COPY_STAGE_POL_STATUSLOG where crte_user_id= @userID and  crte_dt = @dtUploadDateTime";
            sqlCmd = new SqlCommand(strCmd);
            sqlCmd.CommandTimeout = 0;
            sqlCmd.Connection = conn;
            sqlCmd.Parameters.Add(new SqlParameter("dtUploadDateTime", dtUploadDateTime));
            sqlCmd.Parameters.Add(new SqlParameter("userID", userID));
            da.SelectCommand = sqlCmd;
            da.Fill(dt);
            return dt;
        }


        public string ModAISLossInfoCopyStageExcessUploads(string userID, DateTime dtUploadDateTime)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            sqlCmd.CommandTimeout = 0;
            sqlCmd.CommandText = "ModAIS_Process_Copy_Losses_Excess_Upload";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Connection = conn;
            sqlCmd.Parameters.AddWithValue("@create_user_id", userID);
            sqlCmd.Parameters.AddWithValue("@dtUploadDateTime", dtUploadDateTime);
            SqlParameter parmErr = new SqlParameter("@err_msg_op", SqlDbType.VarChar);
            parmErr.Direction = ParameterDirection.Output;
            parmErr.Size = -1;
            sqlCmd.Parameters.Add(parmErr);
            conn.Open();
            sqlCmd.ExecuteNonQuery();
            conn.Close();
            return Convert.ToString(sqlCmd.Parameters["@err_msg_op"].Value);
        }

        public DataTable ExcessLossesUploadsError(string userID, DateTime dtUploadDateTime)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            SqlCommand sqlCmd = new SqlCommand();
            string strCmd = @"select Valuation_Date [Valuation Date],[LOB] LOB,[POLICY_NO] 'Policy No',[CUSTMR_ID] 'Customer ID'
                              ,PGM_EFF_DT 'Program Period Eff Date',PGM_EXP_DT 'Program Period Exp Date',[PGM_TYPE] 'Program Type'
                              ,[STATE] 'State',POL_EFF_DT 'Policy Eff Date',POL_EXP_DT 'Policy Exp Date',[CLM_NBR_TXT] 'Claim No'
                              ,[ADDN_CLM_IND] 'Additional Claim Ind',[ADDN_CLM_TXT] 'Additional Claim',[CLMT_NM] 'Claimant Name'
                              ,[CLM_STS_ID] 'Claim Status',COVG_TRIGR_DT 'Coverage Trigger Date',[LIM2_AMT] 'Limit 2'
                              ,[LOS_PAID_IDNMTY_AMT] 'Total Paid Indemnity',[LOS_PAID_EXPS_AMT] 'Total Paid Expense'
                              ,[LOS_RESRV_IDNMTY_AMT] 'Total Reserved Indemnity',[LOS_RESRV_EXPS_AMT] 'Total Reserved Expense'
                              ,[NON_BILABL_PAID_IDNMTY_AMT] 'NonBillable Paid Indemnity',[NON_BILABL_PAID_EXPS_AMT] 'NonBillable Paid Expense'
                              ,[NON_BILABL_RESRVD_IDNMTY_AMT] 'NonBillable Reserved Indemnity',[NON_BILABL_RESRVD_EXPS_AMT] 'NonBillable Reserved Expense'
                              ,[LOS_SYS_GENRT_IND] 'System Generated',[TXTERRORDESC] 'Comments'  from LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG where crte_user_id= @userID and  
                              crte_dt = @dtUploadDateTime";
            sqlCmd = new SqlCommand(strCmd);
            sqlCmd.CommandTimeout = 0;
            sqlCmd.Connection = conn;
            sqlCmd.Parameters.Add(new SqlParameter("dtUploadDateTime", dtUploadDateTime));
            sqlCmd.Parameters.Add(new SqlParameter("userID", userID));
            da.SelectCommand = sqlCmd;
            da.Fill(dt);
            return dt;
        }
    }
}
