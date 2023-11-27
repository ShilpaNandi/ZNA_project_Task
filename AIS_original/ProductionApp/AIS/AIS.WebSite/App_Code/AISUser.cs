using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Principal;
using System.Collections;
using System.Collections.Generic;

using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;

/// <summary>
/// Custom Principal Class for Retro Application
/// </summary>
public class AISUser : IPrincipal
{

    IList<ApplMenuBE> authWebPages = new List<ApplMenuBE>();
    string role = string.Empty;

    IPrincipal lPrincipal;

    string userID = string.Empty;
    int personID = 0;
    string fullName = String.Empty;
    string fullNameWithDot = String.Empty;

    /// <summary>
    /// Holds User Id (Azcorp ID)
    /// </summary>
    public string UserID
    {
        get { return userID; }
    }

    public int PersonID
    {
        get { return personID; }
    }

    public string FullName
    {
        get
        {
            return fullName;
        }
    }

    public string FullNameWithDot
    {
        get
        {
            return fullNameWithDot;
        }
    }
    /// <summary>
    /// Retrieves authorized web pages for the current Role
    /// </summary>
    private void RetrievePrimes()
    {
        authWebPages = (new AppWebPageAuthBS()).RetrieveAuthWebPages(this.Role);
    }
    /// <summary>
    /// Determine type of permission to given page
    /// "I" - Inquiry Only
    /// "F" - Fully permissions
    /// "X" - No Access
    /// </summary>
    /// <param name="webpage"></param>
    /// <returns>either 'I' or 'F'</returns>
    public char AccessPermission(string webpage)
    {
        char strAccess = 'X';
        ApplMenuBE apMenuBE = new ApplMenuBE();
        apMenuBE = authWebPages.Where(awp => awp.web_page_txt.ToUpper() == webpage.ToUpper()).Count() > 0 ? authWebPages.Where(awp => awp.web_page_txt.ToUpper() == webpage.ToUpper()).First() : null;
        if (apMenuBE != null)
        {
            if (apMenuBE.InquiryOrFull_Access_ind != null)
                strAccess = apMenuBE.InquiryOrFull_Access_ind.Value;
        }
        return strAccess;
    }
    /// <summary>
    /// True - The current Webpage is Account Dependent Webpage
    /// False - The current Webpage is not Account Dependent Webpage
    /// </summary>
    /// <param name="webpage"></param>
    /// <returns></returns>
    public bool ISAccountDependPage(string webpage)
    {
        bool Flag = false;
        ApplMenuBE apMenuBE = new ApplMenuBE();
        apMenuBE = authWebPages.Where(awp => awp.web_page_txt.ToUpper() == webpage.ToUpper()).Count() > 0 ? authWebPages.Where(awp => awp.web_page_txt.ToUpper() == webpage.ToUpper()).First() : null;
        if (apMenuBE != null)
        {
            if (apMenuBE.depnd_txt !=null && apMenuBE.depnd_txt.ToUpper() == "ACCOUNT")
                Flag = true;
        }
        return Flag;
    }
    /// <summary>
    /// Retrieves User Details
    /// </summary>
    /// <param name="oPrincipal"></param>
    public AISUser(IPrincipal oPrincipal)
    {
        lPrincipal = oPrincipal;
        ProcessSecurity();
    }

    /// <summary>
    /// Retrieves User Details
    /// </summary>
    /// <param name="oIdentity"></param>
    public AISUser(IIdentity oIdentity)
    {
        lPrincipal = new WindowsPrincipal((WindowsIdentity)oIdentity);


        ProcessSecurity();
    }
    /// <summary>
    /// Identify the name of the logged-in user 
    /// </summary>
    private void ProcessSecurity()
    {
        userID = (lPrincipal.Identity.Name.Contains('\\')) ?
                lPrincipal.Identity.Name.Split('\\')[1] : lPrincipal.Identity.Name;


        //userID = "AISINQUIRY1";
        //role = GlobalConstants.ApplicationSecurityGroup.Inquiry;

        //userID = "AISADJSPECIALIST1";
        //role = GlobalConstants.ApplicationSecurityGroup.AdjSpecialist;

        //userID = "AISSETUPRECON1";
        //role = GlobalConstants.ApplicationSecurityGroup.SetupRecon;

        //userID = "AISMANAGER1";
       // role = GlobalConstants.ApplicationSecurityGroup.Manager;

        //userID = "AISSYSTEMADMIN1";
        //role = GlobalConstants.ApplicationSecurityGroup.SystemAdmin;
        
        PersonBS persBS = new PersonBS();
        PersonBE persBE;
        //Retrieves Current User ID's Person ID
        persBE = persBS.GetUser(userID);
        if (persBE.PERSON_ID != 0)
        {
            //userID = persBE.USERID;
            personID = persBE.PERSON_ID;
            fullName = persBE.SURNAME + "," + persBE.FORENAME;
            fullNameWithDot = persBE.SURNAME.Substring(0, 1) + ". " + persBE.FORENAME;
        }
        else
        {
            personID = 0;
            fullName = String.Empty;
            fullNameWithDot = string.Empty;
        }
        this.RetrievePrimes();

    }

    public IIdentity Identity
    {
        get
        {
            return lPrincipal.Identity;
        }
    }

    public bool IsInRole(string roleName)
    {
        return (role.Length >= 0);
    }
    /// <summary>
    /// Determine the role of the current user
    /// During system test we have test groups in 
    /// Development and QA web servers
    /// </summary>
    public string Role
    {
        get
        {
            if (role.Length == 0)
            {
                role = String.Empty;
                string[] roles = new string[10];
                SortedList tblRoles = new SortedList(10);
                roles[0] = ConfigurationManager.AppSettings["Inquiry"].ToString();
                tblRoles.Add(roles[0], GlobalConstants.ApplicationSecurityGroup.Inquiry);
                roles[1] = ConfigurationManager.AppSettings["SetupRecon"].ToString();
                tblRoles.Add(roles[1], GlobalConstants.ApplicationSecurityGroup.SetupRecon);
                roles[2] = ConfigurationManager.AppSettings["AdjustmentSpecialist"].ToString();
                tblRoles.Add(roles[2], GlobalConstants.ApplicationSecurityGroup.AdjSpecialist);
                roles[3] = ConfigurationManager.AppSettings["Manager"].ToString();
                tblRoles.Add(roles[3], GlobalConstants.ApplicationSecurityGroup.Manager);
                roles[4] = ConfigurationManager.AppSettings["SystemAdmin"].ToString();
                tblRoles.Add(roles[4], GlobalConstants.ApplicationSecurityGroup.SystemAdmin);

                /// Test groups do not exist in Production environment
                if (ConfigurationManager.AppSettings["Inquiry_Test"] != null)
                {
                    roles[5] = ConfigurationManager.AppSettings["Inquiry_Test"].ToString();
                    tblRoles.Add(roles[5], GlobalConstants.ApplicationSecurityGroup.Inquiry);
                    roles[6] = ConfigurationManager.AppSettings["SetupRecon_Test"].ToString();
                    tblRoles.Add(roles[6], GlobalConstants.ApplicationSecurityGroup.SetupRecon);
                    roles[7] = ConfigurationManager.AppSettings["AdjustmentSpecialist_Test"].ToString();
                    tblRoles.Add(roles[7], GlobalConstants.ApplicationSecurityGroup.AdjSpecialist);
                    roles[8] = ConfigurationManager.AppSettings["Manager_Test"].ToString();
                    tblRoles.Add(roles[8], GlobalConstants.ApplicationSecurityGroup.Manager);
                    roles[9] = ConfigurationManager.AppSettings["SystemAdmin_Test"].ToString();
                    tblRoles.Add(roles[9], GlobalConstants.ApplicationSecurityGroup.SystemAdmin);
                }
                foreach (string lvRole in roles)
                    if (lPrincipal.IsInRole(lvRole))
                    {
                        role = tblRoles[lvRole].ToString();
                        return role;
                    }
            }
            return role;
        }
    }

    /// <summary>
    /// Validate if current user is authorized to access current Web Page
    /// </summary>
    /// <param name="currentWebPage"></param>
    /// <returns></returns>
    public bool IsAuthorized(string currentWebPage)
    {
        bool authorized = false;
        if (this.IsAuthorized())
        {
            var mnus =
                authWebPages.Where(ap => ap.web_page_txt.ToUpper() == currentWebPage.ToUpper());
            authorized = (mnus.Count() > 0);
        }
        return (authorized);
    }
    /// <summary>
    /// Validate if current user is authorized to access current Web Page
    /// </summary>
    /// <returns></returns>
    public bool IsAuthorized()
    {
        string userdomain = lPrincipal.Identity.Name.Split(new char[] { '\\' })[0];
        string actualdomain = ConfigurationManager.AppSettings["Inquiry"].ToString().Split(new char[] { '\\' })[0];
        return (this.Role.Length > 0 && (userdomain == actualdomain || userdomain.ToUpper().IndexOf("WB") >= 0));
    }

}
