using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using System.Collections;

namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// DataAccessor for Customer Contact details
    /// </summary>
    public class CustomerContactDA : DataAccessor<CUSTMR_PERS_REL, CustomerContactBE, AISDatabaseLINQDataContext>
    {

        public CustomerContactDA()
            : base()
        { }
        /// <summary>
        /// Returns all Insured Contacts corresponding to an account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<CustomerContactBE> getInsuredContactData(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();
            //PersonBE persBE = new PersonBE();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Contact information
            /// and project it into Account Business Entity
            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             //where cdd.LKUP.lkup_txt.ToString().Trim() == "PRIMARY CONTACT" && cdd.PER.actv_ind == true
             where cdd.PER.actv_ind == true
             select new CustomerContactBE()
             {
                 CUSTOMER_CONTACT_ID = cdd.custmr_pers_rel_id,
                 PERSON_ID = cdd.pers_id,
                 ROLE_ID = cdd.rol_id,
                 CUSTOMER_ID = cdd.custmr_id
             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.CUSTOMER_ID == AccountID && (cdd.ROLE_ID == 397 || cdd.ROLE_ID == null));
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();

            foreach (CustomerContactBE cContact in result)
            {
                IList<PostalAddressBE> pAddBEs = new List<PostalAddressBE>();
                pAddBEs = (from pa in this.Context.POST_ADDRs
                           join look in this.Context.LKUPs on pa.st_id equals look.lkup_id
                           join pers in this.Context.PERs on pa.pers_id equals pers.pers_id
                           where pa.pers_id == cContact.PERSON_ID
                           select new PostalAddressBE
                           {
                               ADDRESS1 = pa.addr_ln_1_txt,
                               ADDRESS2 = pa.addr_ln_2_txt,
                               CITY = pa.city_txt,
                               POSTALADDRESSID = pa.post_addr_id,
                               STATE_ID = pa.st_id,
                               ZIP_CODE = pa.post_cd_txt,
                               STATE_TXT = look.lkup_txt
                           }).ToList();
                if (pAddBEs.Count > 0)
                    cContact.POST_ADD_BE = pAddBEs[0];
                else
                    cContact.POST_ADD_BE = null;
            }

            return result;
        }


        //public 
        /// <summary>
        /// Returns all Insured Contacts corresponding to an account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="personID"</param>
        /// <returns></returns>
        public CustomerContactBE getInsuredContactData(int AccountID, int personID)
        {
            CustomerContactBE result = new CustomerContactBE();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Contact information
            /// and project it into Account Business Entity
            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             select new CustomerContactBE()
             {
                 CUSTOMER_CONTACT_ID = cdd.custmr_pers_rel_id,
                 PERSON_ID = cdd.pers_id,
                 ROLE_ID = cdd.rol_id,
                 CUSTOMER_ID = cdd.custmr_id
             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.CUSTOMER_ID == AccountID && cdd.PERSON_ID == personID);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            if (query.Count() > 0)
                result = query.ToList()[0];

            return result;
        }
      //  public CustomerContactBE getAccountResponsibilities

        /// <summary>
        /// Returns all Insured Contacts corresponding to an account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public CustomerContactBE getPrimaryContactData(int AccountID)
        {
            CustomerContactBE result = new CustomerContactBE();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Contact information
            /// and project it into Account Business Entity
            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             join lk in this.Context.LKUPs
             on cdd.rol_id equals lk.lkup_id
             where lk.lkup_txt == "PRIMARY CONTACT"
             select new CustomerContactBE()
             {
                 CUSTOMER_CONTACT_ID = cdd.custmr_pers_rel_id,
                 PERSON_ID = cdd.pers_id,
                 ROLE_ID = cdd.rol_id,
                 CUSTOMER_ID = cdd.custmr_id
             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.CUSTOMER_ID == AccountID && cdd.ROLE_ID != null);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            if (query.Count() > 0)
                result = query.ToList()[0];

            IList<PostalAddressBE> pAddBEs = new List<PostalAddressBE>();
            pAddBEs = (from pa in this.Context.POST_ADDRs
                      join look in this.Context.LKUPs on pa.st_id equals look.lkup_id
                      join pers in this.Context.PERs on pa.pers_id equals pers.pers_id
                      where pa.pers_id == result.PERSON_ID
                      select new PostalAddressBE
                      {
                          ADDRESS1 = pa.addr_ln_1_txt,
                          ADDRESS2 = pa.addr_ln_2_txt,
                          CITY = pa.city_txt,
                          POSTALADDRESSID = pa.post_addr_id,
                          STATE_ID = pa.st_id,
                          ZIP_CODE = pa.post_cd_txt,
                          STATE_TXT = look.lkup_txt
                      }).ToList();

            if (pAddBEs.Count>0)
                result.POST_ADD_BE = pAddBEs[0];
            else
                result.POST_ADD_BE = null;
            
            return result;
        }

        /// <summary>
        /// Returns all responsibilities corresponding to an account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<CustomerContactBE> getAccountResponsibilities(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Contact information
            /// and project it into Account Business Entity
            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             where cdd.LKUP.lkup_txt.ToString().Trim() != "PRIMARY CONTACT"
             orderby cdd.LKUP.lkup_id.ToString().Trim()
             select new CustomerContactBE()
             {
                 CUSTOMER_CONTACT_ID = cdd.custmr_pers_rel_id,
                 PERSON_ID = cdd.pers_id,
                 ROLE_ID = cdd.rol_id,
                 CUSTOMER_ID = cdd.custmr_id,
                 FULLNAME = cdd.PER.surname + " " + cdd.PER.forename
                 
             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.CUSTOMER_ID == AccountID && cdd.ROLE_ID != null);
            }
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.PERSON_ID > 0);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }


        /// <summary>
        /// Returns all responsibilities corresponding to an account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public IList<CustomerContactBE> getLSSAnalystName(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();

            if (this.Context == null)
                this.Initialize();

            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             where cdd.LKUP.lkup_txt.ToString().Trim() != "CFS2"
             orderby cdd.LKUP.lkup_id.ToString().Trim()
             select new CustomerContactBE()
             {
                 CUSTOMER_CONTACT_ID = cdd.custmr_pers_rel_id,
                 PERSON_ID = cdd.pers_id,
                 ROLE_ID = cdd.rol_id,
                 CUSTOMER_ID = cdd.custmr_id
             });

            /// Get a specific account record
            if (AccountID > 0)
            {
                query = query.Where(cdd => cdd.CUSTOMER_ID == AccountID && cdd.ROLE_ID != null);
            }

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        //Invoice Inquiry
        public IList<CustomerContactBE> getCFS2Names(int AccountID)
        {
            IList<CustomerContactBE> result = new List<CustomerContactBE>();

            if (this.Context == null)
                this.Initialize();

            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             join look in this.Context.LKUPs on cdd.rol_id equals look.lkup_id
             join pers in this.Context.PERs on cdd.pers_id equals pers.pers_id
             where cdd.custmr_id == AccountID
             && cdd.rol_id == 366  // 366 - CFS2
             orderby cdd.PER.surname, cdd.PER.forename
             select new CustomerContactBE()
             {
                 FULLNAME = cdd.PER.forename + "," + cdd.PER.surname,
                 PERSON_ID = cdd.PER.pers_id
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        # region AccountAssignment
        public bool Assignresponsibilities(int intCustomerid, ArrayList responsibilitiesid, 
            ArrayList Personid, out string errorMessage, IList<CustomerContactBE> actresps, 
            bool handleConcurrency, int UserID)
        {
            bool success = true;
            CustomerContactBE result = null;
            int responsibleid;

            errorMessage = string.Empty;
            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Contact information
            /// and project it into Account Business Entity

            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             select new CustomerContactBE()
             {
                 CUSTOMER_CONTACT_ID = cdd.custmr_pers_rel_id,
                 PERSON_ID = cdd.pers_id,
                 ROLE_ID = cdd.rol_id,
                 CUSTOMER_ID = cdd.custmr_id
             });

            if (handleConcurrency)
            {
                try
                {
                    IList<CustomerContactBE> lstresp = getAccountResponsibilities(intCustomerid);
                    int loop = 0;
                    if (actresps != null)
                    {
                        if (actresps.Count() == lstresp.Count())
                        {
                            foreach (CustomerContactBE cct in actresps)
                                if (cct.ROLE_ID == lstresp[loop].ROLE_ID && cct.PERSON_ID != lstresp[loop++].PERSON_ID)
                                {
                                    success = false;
                                    errorMessage = "Row Not Found or Changed";
                                    break;
                                }
                        }
                        else
                        {
                            success = false;
                            errorMessage = "Row Not Found or Changed";
                        }

                        if (!success)
                            return false;
                    }
                }
                catch { }
            }
                /// Get a specific account record
                if (intCustomerid > 0)
                {

                    for (int intcount = 0; intcount < responsibilitiesid.Count; intcount++)
                    {
                        responsibleid = Convert.ToInt32(responsibilitiesid[intcount]);
                        //query = query.Where(cdd => cdd.CUSTOMER_ID == intCustomerid &&  cdd.ROLE_ID ==(int)responsibilitiesid[intcount]);
                        query = query.Where(cdd => cdd.CUSTOMER_ID == intCustomerid && cdd.ROLE_ID == responsibleid);
                        if (query.ToList().Count > 0)
                        {

                            result = query.ToList()[0];
                            result = this.Load(result.CUSTOMER_CONTACT_ID);

                            if ((int)Personid[intcount] > 0)
                            {
                                result.PERSON_ID = (int)Personid[intcount];
                                //else
                                // result.PERSON_ID = null;


                                success = Saveresponsibilities(result, UserID);
                                errorMessage = result.ErrorMessage;
                            }
                            else
                                result.PERSON_ID = null;

                        }
                        else
                        {
                            result = new CustomerContactBE();
                            result.CUSTOMER_ID = intCustomerid;
                            result.ROLE_ID = (int)responsibilitiesid[intcount];

                            if ((int)Personid[intcount] > 0)
                            {
                                result.PERSON_ID = (int)Personid[intcount];

                                success = Saveresponsibilities(result, UserID);
                                errorMessage = result.ErrorMessage;
                            }
                             else
                             result.PERSON_ID = null;
                        }


                    }
                }
            return success;
        }
        /// <summary>
        /// This method is used to return the aries person id based on the role.
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public CustomerContactBE GetAriesperson(int customerid, int roleid)
        {
            CustomerContactBE result = new CustomerContactBE();
            string strFullname;
            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Contact information
            /// and project it into Account Business Entity
            IQueryable<CustomerContactBE> query =
            (from cdd in this.Context.CUSTMR_PERS_RELs
             join per in this.Context.PERs
             on cdd.pers_id equals per.pers_id
             where cdd.custmr_id == customerid && cdd.rol_id == roleid
             select new CustomerContactBE 
             {
                 PERSON_ID = per.pers_id,
             }
             );

            /// Get a specific account record
            

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            if (query.Count() > 0)
                result = query.ToList()[0];
            return result;

        
        
        }
     /*   public IList<CustomerContactBE> Getresponsibilities(int Customerid)
        {
            IList<CustomerContactBE> lstResponsibilitiess = new List<CustomerContactBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<CustomerContactBE> query = from cdd in Context.CUSTMR_PERS_RELs
                                                  where cdd.custmr_id = Customerid
                                                  select new CustomerContactBE
                                                  {
                                                      FULLNAME = cdd.PER.surname + " " + cdd.PER.forename,
                                                      RESP_NAME = cdd.LKUP

                                                  };

        
        }*/

        public bool Saveresponsibilities(CustomerContactBE CusCt, int UserID)
        {
            bool suceeded = false;

            if (CusCt != null)
            {
                if (CusCt.CUSTOMER_CONTACT_ID > 0)
                {
                    CusCt.UPDATE_USER_ID = UserID;
                    CusCt.UPDATE_DATE = DateTime.Now;
                    
                    suceeded = this.Update(CusCt);
                }
                else
                {
                    CusCt.CREATE_USER_ID = UserID;
                    CusCt.CREATE_DATE = DateTime.Now;

                    suceeded = this.Add(CusCt);
                }
            }
            return suceeded;
        }
        #endregion
        #region External Contacts
        public CustomerContactBE getCustmerContact(int PERSON_ID)
        {
            CustomerContactBE cstCt = new CustomerContactBE();

            IQueryable<CustomerContactBE> result = from res in this.Context.CUSTMR_PERS_RELs
                                                   where res.pers_id == PERSON_ID
                                                   select new CustomerContactBE
                                                   {
                                                       CUSTOMER_CONTACT_ID = res.custmr_pers_rel_id,
                                                       PERSON_ID = res.pers_id,
                                                       CUSTOMER_ID = res.custmr_id
                                                   };
            cstCt = result.Single();

            return cstCt;

        }
        #endregion
    }
}
