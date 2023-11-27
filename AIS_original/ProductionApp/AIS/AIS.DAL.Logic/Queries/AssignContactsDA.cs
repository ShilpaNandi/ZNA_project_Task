using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;


namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// AssignContactsDA class contains methods to retrieve and manipulate the contacts data
    /// </summary>
    public class AssignContactsDA : DataAccessor<PREM_ADJ_PGM_PERS_REL, AssignContactsBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Retrieves existing Contacts data for particular ProgramPeriod
        /// </summary>
        /// <param name="programPeriodId"></param>
        /// <returns>List of Contacts for a given ProgramPeriod</returns>
        public IList<AssignContactsBE> GetContactsData(int programPeriodId)
        {
            IList<AssignContactsBE> result = new List<AssignContactsBE>();

            IQueryable<AssignContactsBE> query =
                (from pappr in this.Context.PREM_ADJ_PGM_PERS_RELs
                 join pap in this.Context.PREM_ADJ_PGMs
                 on pappr.prem_adj_pgm_id equals pap.prem_adj_pgm_id
                 join per in this.Context.PERs
                 on pappr.pers_id equals per.pers_id
                 where pap.prem_adj_pgm_id == programPeriodId 
                 

                 select new AssignContactsBE
                 {
                     PREM_ADJ_PGM_PERS_REL_ID = pappr.prem_adj_pgm_pers_rel_id,
                     PREM_ADJ_PGM_ID = pappr.prem_adj_pgm_id,
                     CUSTMR_ID = pappr.custmr_id,
                     PERS_ID = pappr.pers_id,
                     SND_INVC_IND = pappr.snd_invc_ind,
                     COMMU_MEDUM_ID = pappr.commu_medum_id,
                     ComTypText=(from lkp in Context.LKUPs 
                     where lkp.lkup_id == pappr.commu_medum_id
                     select lkp.lkup_txt ).First().ToString(),
                     UPDT_DT = pappr.updt_dt,
                     UPDT_USER_ID = pappr.updt_user_id,
                     CRTE_USER_ID = pappr.crte_user_id,
                     CRTE_DT = pappr.crte_dt,
                     ACTV_IND = pappr.actv_ind,
                     FirstName = per.forename,
                     LastName = per.surname,
                     FullName = per.surname + ", " + per.forename,
                     ContTypID = per.conctc_typ_id,
                     ContTyp=(from lkp in Context.LKUPs 
                     where lkp.lkup_id == per.conctc_typ_id
                     select lkp.lkup_txt ).First().ToString()
                 });
            query = query.OrderBy(o => o.FullName).OrderBy(o => o.ContTyp);
            result = query.ToList();

            return result;
        }
        /// <summary>
        /// Retrieves Person Names for a given Contact Type
        /// </summary>
        /// <param name="ContTypId"></param>
        /// <returns>List of Person Names</returns>
        public IList<PersonBE> GetPersonNames(int contTypId,int custmrID)
        {
            IList<PersonBE> result = new List<PersonBE>();
            if (contTypId > 0 && contTypId!=236)
            {
                IQueryable<PersonBE> query =
                    (from per in this.Context.PERs
                     where per.conctc_typ_id == contTypId && per.pers_id != 1000000
                     orderby per.surname,per.forename
                     where per.actv_ind == true
                     select new PersonBE
                     {
                         PERSON_ID = per.pers_id,
                         FORENAME = per.forename,
                         SURNAME = per.surname,
                         FULLNAME = per.surname + ", " + per.forename,
                         CONCTACT_TYPE_ID = per.conctc_typ_id
                     });
                result = query.ToList();
            }
            else if (contTypId == 236)
            {
                IQueryable<PersonBE> queryInsured =
                       (from per in this.Context.PERs
                        join custrel in this.Context.CUSTMR_PERS_RELs on per.pers_id equals custrel.pers_id
                        where per.conctc_typ_id == contTypId && per.pers_id != 1000000 && custrel.custmr_id==custmrID
                        orderby per.surname, per.forename
                        select new PersonBE
                        {
                            PERSON_ID = per.pers_id,
                            FORENAME = per.forename,
                            SURNAME = per.surname,
                            FULLNAME = per.surname + ", " + per.forename,
                            CONCTACT_TYPE_ID = per.conctc_typ_id
                        });
                result = queryInsured.ToList();
            
            }

            return result;
        }
        public IList<PersonBE> GetPersonNames(int contTypId)
        {
            IList<PersonBE> result = new List<PersonBE>();
            if (contTypId > 0)
            {
                IQueryable<PersonBE> query =
                    (from per in this.Context.PERs
                     where per.conctc_typ_id == contTypId && per.pers_id != 1000000
                     orderby per.surname, per.forename
                     where per.actv_ind == true
                     select new PersonBE
                     {
                         PERSON_ID = per.pers_id,
                         FORENAME = per.forename,
                         SURNAME = per.surname,
                         FULLNAME = per.surname + ", " + per.forename,
                         CONCTACT_TYPE_ID = per.conctc_typ_id
                     });
                result = query.ToList();
               
            }
            return result;
        }


       


    }
}
