using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

using ZurichNA.AIS.DAL.Logic;


namespace ZurichNA.AIS.DAL.Logic
{
    public class PersonDA : DataAccessor<PER, PersonBE, AISDatabaseLINQDataContext>
    {
        #region internalcontacts
        public IList<PersonBE> getPersonsList(string strLookupType)
        {
            IList<PersonBE> result = new List<PersonBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PersonBE> query =
            (from cdd in this.Context.PERs
             join lk in this.Context.LKUPs on cdd.conctc_typ_id equals lk.lkup_id
             join padd in this.Context.POST_ADDRs on cdd.pers_id equals padd.pers_id
             where lk.attr_1_txt == "I" && lk.lkup_txt == strLookupType && cdd.pers_id != 1000000
             orderby cdd.conctc_typ_id, cdd.surname, cdd.forename
             select new PersonBE()
             {
                 PERSON_ID = cdd.pers_id,
                 FORENAME = cdd.forename,
                 SURNAME = cdd.surname,
                 FULLNAME = cdd.surname + "," + cdd.forename,
                 USERID = cdd.external_reference,
                 TITLE_ID = cdd.prefx_ttl_id,
                 TELEPHONE1 = cdd.phone_nbr_1_txt,
                 TELEPHONE2 = cdd.phone_nbr_2_txt,
                 FAX = cdd.fax_nbr_txt,
                 EMAIL = cdd.email_txt,
                 CONTACTTYPE = lk.lkup_txt,
                 EFFECTIVEDATE = cdd.eff_dt,
                 EXPIRYDATE = cdd.expi_dt,
                 ACTIVE = cdd.actv_ind,
                 POST_ADDR_ID = padd.post_addr_id
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        public IList<PersonBE> getPersonsList()
        {
            IList<PersonBE> result = new List<PersonBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PersonBE> query =
            (from cdd in this.Context.PERs
             join lk in this.Context.LKUPs on cdd.conctc_typ_id equals lk.lkup_id
             join padd in this.Context.POST_ADDRs on cdd.pers_id equals padd.pers_id
             where lk.attr_1_txt == "I" && cdd.pers_id != 1000000 && cdd.actv_ind==true
             orderby (cdd.surname + "," + cdd.forename)//cdd.conctc_typ_id, cdd.surname, cdd.forename
             select new PersonBE()
             {
                 PERSON_ID = cdd.pers_id,
                 FORENAME = cdd.forename,
                 SURNAME = cdd.surname,
                 FULLNAME = cdd.surname + "," + cdd.forename,
                 USERID = cdd.external_reference,
                 TITLE_ID = cdd.prefx_ttl_id,
                 TELEPHONE1 = cdd.phone_nbr_1_txt,
                 TELEPHONE2 = cdd.phone_nbr_2_txt,
                 FAX = cdd.fax_nbr_txt,
                 EMAIL = cdd.email_txt,
                 CONTACTTYPE = lk.lkup_txt,
                 EFFECTIVEDATE = cdd.eff_dt,
                 EXPIRYDATE = cdd.expi_dt,
                 ACTIVE = cdd.actv_ind,
                 POST_ADDR_ID = padd.post_addr_id
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        public IList<PersonBE> getCRMusers()
        {
            IList<PersonBE> result = new List<PersonBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PersonBE> query =
            (from cdd in this.Context.PERs
             join lk in this.Context.LKUPs on cdd.conctc_typ_id equals lk.lkup_id
             join padd in this.Context.POST_ADDRs on cdd.pers_id equals padd.pers_id
             where lk.lkup_id  == 234 && cdd.pers_id != 1000000 && cdd.actv_ind == true
             orderby cdd.conctc_typ_id, cdd.surname, cdd.forename
             select new PersonBE()
             {
                 PERSON_ID = cdd.pers_id,
                 FORENAME = cdd.forename,
                 SURNAME = cdd.surname,
                 FULLNAME = cdd.surname + "," + cdd.forename,
                 USERID = cdd.external_reference,
                 TITLE_ID = cdd.prefx_ttl_id,
                 TELEPHONE1 = cdd.phone_nbr_1_txt,
                 TELEPHONE2 = cdd.phone_nbr_2_txt,
                 FAX = cdd.fax_nbr_txt,
                 EMAIL = cdd.email_txt,
                 CONTACTTYPE = lk.lkup_txt,
                 EFFECTIVEDATE = cdd.eff_dt,
                 EXPIRYDATE = cdd.expi_dt,
                 ACTIVE = cdd.actv_ind,
                 POST_ADDR_ID = padd.post_addr_id
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        //To check whether the contact is manager to other active contacts
        public bool IsMgrMappedWithAnyContact(int mgrID)
        {
            bool Flag = false;
            var result = from contacts in this.Context.PERs
                         where contacts.mgr_id == mgrID && contacts.actv_ind == true
                         select contacts;
            if (result.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }

        #endregion
        public IList<PersonBE> GetPersonNames()
        {
            IList<PersonBE> result = new List<PersonBE>();
            IQueryable<PersonBE> query =
                (from per in this.Context.PERs
                 where per.actv_ind == true &&  per.pers_id != 1000000
                 select new PersonBE
                 {
                     PERSON_ID = per.pers_id,
                     FORENAME = per.forename,
                     SURNAME = per.surname,
                     FULLNAME = per.surname + ", " + per.forename,

                 });


            result = query.ToList();
            PersonBE perBE = new PersonBE();
            perBE.PERSON_ID = 0;
            perBE.FULLNAME = "(Select)";
            result.Insert(0, perBE);

            return result;
        }
        public bool IsExistsInternalContact(string forename, string surname, string phone, string email, int personID)
        {
            bool Flag = false;
            var Internal = from cdd in this.Context.PERs
                           where (cdd.forename == forename && cdd.surname == surname && cdd.phone_nbr_1_txt == phone && cdd.email_txt == email && cdd.pers_id != personID)
                           select new { cdd.forename };
            if (Internal.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }

    

        #region External Contacts
        public bool IsExistsExtContact(string forename, string surname, int personID, int contTypeID,int contNameID)
        {
            bool Flag = false;
            var Internal = from cdd in this.Context.PERs
                           where (cdd.forename == forename && cdd.surname == surname && cdd.conctc_typ_id == contTypeID
                           && cdd.extrnl_org_id == contNameID && cdd.pers_id != personID)
                           select new { cdd.forename };
            if (Internal.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }
        public bool IsExistsInsuredExtContact(string forename, string surname, int personID, int contTypeID, int contNameID)
        {
            bool Flag = false;
            var Internal = from cdd in this.Context.PERs
                           join cusper in this.Context.CUSTMR_PERS_RELs on cdd.pers_id equals cusper.pers_id
                           where (cdd.forename == forename && cdd.surname == surname && cdd.conctc_typ_id == contTypeID
                           && cusper.custmr_id == contNameID && cdd.pers_id != personID)
                           select new { cdd.forename };
            if (Internal.Count() > 0)
            {
                Flag = true;
            }
            return Flag;
        }

        /// <summary>
        /// Invoked to Display all external contacts in Listview
        /// </summary>
        /// <returns>List of Person Details along with Post Address Details/Ext Contact Names</returns>
        public IList<PersonBE> getExtContactList(int ContactTypeId,string strName)
        {
            IList<PersonBE> result = new List<PersonBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            /// 236 - Insured
            if (ContactTypeId == 236)
            {
                //Finding 'Primary Contact' Type ID from Lookup table
                int PrimaryContactID = this.Context.LKUPs.Where(o => o.lkup_txt == "PRIMARY CONTACT").Select(p => p.lkup_id).Single();

                IQueryable<PersonBE> query =
                    (from pers in this.Context.PERs
                     join cusper in this.Context.CUSTMR_PERS_RELs on pers.pers_id equals cusper.pers_id
                     join custmr in this.Context.CUSTMRs on cusper.custmr_id equals custmr.custmr_id
                     join pa in this.Context.POST_ADDRs on pers.pers_id equals pa.pers_id into tempPA
                     from x in tempPA.DefaultIfEmpty()
                     join lk in this.Context.LKUPs on x.st_id equals lk.lkup_id into tempLK
                     from z in tempLK.DefaultIfEmpty()
                     where pers.conctc_typ_id.Value == ContactTypeId && custmr.full_nm == strName && pers.pers_id!= 1000000 &&
                    (cusper.rol_id == PrimaryContactID /// -  PRIMARY CONTACT/PRIMARY CONTACT
                       || cusper.rol_id == null)
                     orderby ExternalContactType.INSURED,pers.surname,pers.forename,custmr.full_nm
                     select new PersonBE()
                     {
                         PERSON_ID = pers.pers_id,
                         FORENAME = pers.forename,
                         SURNAME = pers.surname,
                         EXTERNAL_ORGN_ID = pers.extrnl_org_id,
                         CONTACTTYPE = ExternalContactType.INSURED,
                         EXTERNAL_ORGN_TXT = custmr.full_nm,
                         CityName = x.city_txt,
                         StateName = z.lkup_txt,
                         selPOSTALADDRESSID = x.post_addr_id,
                         ACTIVE = pers.actv_ind
                     });

                if (query.Count() > 0)
                    result = query.ToList();
            }
            else
            {

                IQueryable<PersonBE> query =
                (from pers in this.Context.PERs
                 join pa in this.Context.POST_ADDRs on pers.pers_id equals pa.pers_id into tempPA
                 from x in tempPA.DefaultIfEmpty( )
                 join lk in this.Context.LKUPs on x.st_id equals lk.lkup_id  into tempLK
                 from z in tempLK.DefaultIfEmpty( )
                 where pers.conctc_typ_id.Value == ContactTypeId && pers.EXTRNL_ORG.full_name == strName &&  pers.pers_id != 1000000
                 orderby pers.LKUP.lkup_txt, pers.surname, pers.forename,pers.EXTRNL_ORG.full_name 
                 select new PersonBE()
                 {
                     PERSON_ID = pers.pers_id,
                     FORENAME = pers.forename,
                     SURNAME = pers.surname,
                     EXTERNAL_ORGN_ID = pers.extrnl_org_id,
                     CONTACTTYPE = pers.LKUP.lkup_txt,
                     EXTERNAL_ORGN_TXT = pers.EXTRNL_ORG.full_name,
                     CityName = x.city_txt,
                     StateName =  z.lkup_txt,
                     selPOSTALADDRESSID = x.post_addr_id,
                     ACTIVE = pers.actv_ind
                 });
                if (query.Count() > 0)
                    result = query.ToList();
            }

            //Checking for each row 
            //foreach (PersonBE pers in result)
            //{

            //    //Checking for each row with address table to retrive City and State
            //    IList<PostalAddressBE> pAddBEs = new List<PostalAddressBE>();
            //    pAddBEs = (from pa in this.Context.POST_ADDRs
            //               join look in this.Context.LKUPs on pa.st_id equals look.lkup_id
            //               join pa in this.Context.POST_ADDRs on cdd.pers_id equals pa.pers_id
            //               join look in this.Context.LKUPs on pa.st_id equals look.lkup_id
            //               where pa.pers_id == pers.PERSON_ID
            //               select new PostalAddressBE
            //               {
            //                   ADDRESS1 = pa.addr_ln_1_txt,
            //                   ADDRESS2 = pa.addr_ln_2_txt,
            //                   CITY = pa.city_txt,
            //                   POSTALADDRESSID = pa.post_addr_id,
            //                   STATE_ID = pa.st_id,
            //                   ZIP_CODE = pa.post_cd_txt,
            //                   STATE_TXT = look.lkup_txt
            //               }).ToList();
            //    if (pAddBEs.Count > 0)
            //        pers.POST_ADD_BE = pAddBEs[0];
            //    else
            //        pers.POST_ADD_BE = null;
            //}
            return result;
        }
        /// <summary>
        /// Invoked to Display all Insured contact Type Names 
        /// </summary>
        /// <returns>List of Insured Contact Type Names</returns>
        public IList<PersonBE> getInsuredNames(int ContactTypeId)
        {
            IList<PersonBE> result = new List<PersonBE>();
            if (this.Context == null)
                this.Initialize();
                IQueryable<PersonBE> query =
                    (from pers in this.Context.PERs
                     join cusper in this.Context.CUSTMR_PERS_RELs on pers.pers_id equals cusper.pers_id
                     join custmr in this.Context.CUSTMRs on cusper.custmr_id equals custmr.custmr_id
                     where pers.conctc_typ_id.Value == ContactTypeId &&  pers.pers_id != 1000000
                     orderby custmr.full_nm
                     select new PersonBE()
                     {
                         PERSON_ID = pers.pers_id,
                         EXTERNAL_ORGN_TXT = custmr.full_nm,
                     });

                if (query.Count() > 0)
                    result = query.ToList();
            return result;
        }

        /// <summary>
        /// To retrive External Orgnanization names list to popup the Dropdown
        /// </summary>
        /// <param name="ContactTypeID"></param>
        /// <returns>Retuns List with Ext Contacts and IDs</returns>
        public IList<LookupBE> getNamesList(int contactTypeID)
        {
            IList<LookupBE> result = new List<LookupBE>();
            IQueryable<LookupBE> query =
                       (from exorg in this.Context.EXTRNL_ORGs
                        where exorg.role_id == contactTypeID
                        && exorg.extrnl_org_id != 1000000
                        orderby exorg.full_name
                        select new LookupBE()
                       {
                           LookUpID = exorg.extrnl_org_id,
                           LookUpName = exorg.full_name
                       });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns Customer Names To popup the dropdown
        /// </summary>
        /// <returns>Retuns List with CustomerNames and IDs</returns>
        public IList<LookupBE> getInsuredNamesList()
        {
            IList<LookupBE> result = new List<LookupBE>();
            IQueryable<LookupBE> query =
                       (from cus in this.Context.CUSTMRs
                        orderby cus.full_nm
                        select new LookupBE()
                        {
                            LookUpID = cus.custmr_id,
                            LookUpName = cus.full_nm
                        });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        #endregion
        #region Program Periods
        /// <summary>
        /// Invokes Contact Names for selected External Organization
        /// </summary>
        /// <param name="ExtOrgID">External Organization ID</param>
        /// <returns></returns>
        public IList<LookupBE> getContactsByExtOrg(int extOrgID)
        {
            IList<LookupBE> result = new List<LookupBE>();
            IQueryable<LookupBE> query =
                       (from pe in this.Context.PERs
                        where pe.extrnl_org_id == extOrgID
                        && pe.extrnl_org_id != 1000000
                        orderby pe.forename + " " + pe.surname
                        select new LookupBE()
                        {
                            LookUpID = pe.pers_id,
                            LookUpName = pe.forename + " " + pe.surname
                        });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        #endregion

        /// <summary>
        /// Retrieves the Current Logged-In User
        /// </summary>
        /// <param name="externalReference">External Reference Text (AZCORP ID)</param>
        /// <returns>current Logged-In User</returns>
        public PersonBE GetUser(string externalReference)
        {
            PersonBE result = new PersonBE();
            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PersonBE> query =
            (from pr in this.Context.PERs
             where pr.external_reference.Trim().ToUpper() == externalReference.Trim().ToUpper()
             && pr.pers_id != 1000000
             select new PersonBE()
             {
                 PERSON_ID = pr.pers_id,
                 FORENAME = pr.forename,
                 SURNAME = pr.surname,
                 FULLNAME = pr.surname + "," + pr.forename,
                 TITLE_ID = pr.prefx_ttl_id,
                 TELEPHONE1 = pr.phone_nbr_1_txt,
                 TELEPHONE2 = pr.phone_nbr_2_txt,
                 FAX = pr.fax_nbr_txt,
                 EMAIL = pr.email_txt
             });
            if (query.Count() > 0)
            {
                result = query.ToList()[0];
            }
            return result;

        }


    }
}
