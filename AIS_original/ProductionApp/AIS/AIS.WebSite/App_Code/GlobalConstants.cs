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
using System.Collections;

/// <summary>
/// Summary description for GlobalConstants
/// </summary>
public static class GlobalConstants
{
    /// <summary>
    /// Error Messages
    /// </summary>
    public struct ErrorMessage
    {
        public const string RowNotFoundOrChanged = 
            "Your update was not saved to the database, another user may be updating this page at this time."+
                                "\n Please refresh the page and try again";
        public const string ServerTooBusy = "Server is too busy. Please try after refreshing page.";

    }
    /// <summary>
    /// Constant values for Program Status
    /// </summary>
    public struct ProgramStatus
    {
        public const string Initial = "Initial";
        public const string SubmittedForSetupQC = "Submitted For Setup QC";
        public const string SetupQCCompleted = "Setup QC Completed";
        public const string Active = "Active";
    }
    /// <summary>
    /// Constant values for Application Security Groups
    /// </summary>
    public struct ApplicationSecurityGroup
    {
        public const string Inquiry = "Inquiry Only";
        public const string SetupRecon = "Setup & Recon";
        public const string AdjSpecialist = "Adjustment Specialist";
        public const string Manager = "Manager";
        public const string SystemAdmin = "System Admin";
    }

    /// <summary>
    /// Constant values for Adjustment Status
    /// </summary>
    public struct AdjustmentStatus
    {
        public const string Calc = "CALC";
        public const string DraftInvd = "DRAFT-INVOICE";
        public const string QCdDraftInv = "QC-DRAFT INVOICE";
        public const string ReconReviewed = "RECON REVIEW";
        public const string UWReviewed = "UW REVIEW";
        public const string FinalInvd = "FINAL INVOICE";
        public const string Cancelled = "CANCELLED";
        public const string Transmitted = "TRANSMITTED";
        public const int Cancelled_Code = 347;
        public const string AdjustmentStatuses = "ADJUSTMENT STATUSES";


    }

    /// <summary>
    /// List of LookUp Types
    /// </summary>
    public struct LookUpType
    {
        public const string STATE = "STATE";
        public const string DELIVERY_METHOD = "DELIVERY METHOD";
        public const string ADJUSTMENT_TYPE = "ADJUSTMENT TYPE";
        public const string ALAE_TYPE = "ALAE TYPE";
        public const string LOSS_SOURCE = "LOSS SOURCE";
        public const string LOB_COVERAGE = "LOB COVERAGE";
        public const string CHF_LOSSTYPE = "CHF LOSSTYPE";
        public const string PROGRAM_TYPE = "CHF LOSSTYPE";
        public const string TITLE = "TITLE";
        public const string EXPOSURE_TYPE = "EXPOSURE TYPE";
        public const string NON_SUBJECT_AUDIT_PREMIUM = "NON-SUBJECT AUDIT PREMIUM";
        public const string ACCOUNT_SEARCH = "ACCOUNT SEARCH";
        public const string INVOICE_SEARCH = "INVOICE SEARCH";
        public const string POLICY_SEARCH = "POLICY SEARCH";
        public const string CONTACT_TYPE = "CONTACT TYPE";
        public const string TRACKING_ISSUES = "TRACKING ISSUES";
        public const string TRACKING_FORMS = "TRACKING FORMS";
        public const string COMMENT_CATEGORY = "COMMENT CATEGORY";
        public const string TPA = "TPA";
        public const string LBA_ADJUSTMENT_TYPE = "PAID/INCURRED (LBA ADJUSTMENT TYPE)";
        public const string CHF_BASIS = "CHF BASIS";
        public const string RETRO_FORMULA_COMPONENTS = "RETRO FORMULA COMPONENTS";
        public const string ADJUSTMENT_ISSUES = "ADJUSTMENT ISSUES";
        public const string ADJUSTMENT_DOCUMENT = "ADJUSTMENT DOCUMENT";
        public const string INVOICE_TYPE = "INVOICE TYPE";
        public const string TRANSACTION_TYPE = "TRANSACTION TYPE";
        public const string BKTCY_BUYOUT = "BKTCY/BUYOUT";
        public const string PER = "PER";
        public const string PAID_INCURRED = "PAID/INCURRED";
        public const string AM_DASHBOARD_CRITERIA = "AM DASHBOARD CRITERIA";
        public const string PROGRAM_STATUSES = "PROGRAM STATUSES";
        public const string ADJUSTMENT_STATUSES = "ADJUSTMENT STATUSES";
        public const string ROLES = "ROLES";
        public const string RESPONSIBILITY = "RESPONSIBILITY";
        public const string ACCOUNT_SETUP_PROCESSING_CHECKLIST_ITEMS = "ACCOUNT SETUP - PROCESSING CHECKLIST ITEMS";
        public const string ACCOUNT_SETUP_ISSUES = "ACCOUNT SETUP ISSUES";
        public const string ADJUSTMENT_PROCESSING_CHECKLIST_ITEMS = "ADJUSTMENT - PROCESSING CHECKLIST ITEMS";
        public const string TPA_BILLING_CYCLE = "TPA BILLING CYCLE";
        public const string TPA_LIST = "TPA LIST";
        public const string CLAIM_STATUS = "CLAIM STATUS";
        public const string PENDING_REASON_CODES = "PENDING REASON CODES";
        public const string UNDERWRITER_RESPONSE = "UNDERWRITER RESPONSE";
        public const string SECURITY_GROUP = "SECURITY GROUP";
        public const string PRIMARY_CONTACT = "PRIMARY CONTACT";
    }

    /// <summary>
    /// Constant Values for Comment Category
    /// </summary>
    public struct CommentCategory
    {
        public const string ADJUSTMENT_SETUP = "Adjustment Setup";
        public const string AUDIT_ENTRY = "Audit Entry";
        public const string ILRF_SETUP = "ILRF Setup";
    }
    public struct AuditExposureType
    {
        public const string BASIC_PREMIUM = "Basic Premium";
        public const string EMPLOYEES = "Employees";
        public const string PAYROLL = "Payroll";
        public const string POWER_UNITS = "Power Units";
        public const string REVENUE = "Revenue";
        public const string SALES = "Sales";
        public const string STANDARD_PREMIUM = "Standard Premium";
        public const string MANUAL_PREMIUM = "Manual Premium";
        public const string COMBINED_AGGREGATE = "Combined Elements";
    }
    public struct DeliveryMethodType
    {
        public const string EMAIL = "E-Mail";
        public const string FAX = "Fax";
        public const string INTERNAL = "Internal";
        public const string UPS = "UPS";
        public const string USPS = "USPS";
    }

    public struct AuditingWebPage
    {
        public const string AccountInfo = "ACCOUNT INFO";
        public const string AccountInfoRelatedRetro = "ACCOUNT INFO - RELATED RETRO";
        public const string AccountInfoRelatedLSI = "ACCOUNT INFO - RELATED LSI";
        public const string PolicyInfo = "POLICY INFO";
        public const string ProgramPeriodSetup = "PROGRAM PERIOD SETUP";
        public const string LBASetup = "LBA SETUP";
        public const string LCFSetup = "LCF SETUP";
        public const string TaxMultiplierSetup = "TAX MULTIPLIER SETUP";
        public const string CHFSetup = "CHF SETUP";
        public const string RMLSetup = "RML SETUP";
        public const string RetroInfo = "RETRO INFO";
        public const string AssignERPFormula = "ASSIGN ERP FORMULA";
        public const string EscrowSetup = "ESCROW SETUP";
        public const string ILRFSetup = "ILRF SETUP";
        public const string InternalMasters = "INTERNAL MASTERS";
        public const string AuditInormation = "AUDITINFORMATION";

        public const string ARIESClearing = "ARIES CLEARING";

    }
    public struct InternalContacts
    {
        public const string Crm = "C & RM";
        public const string UnderWriter = "UNDER WRITER";

    }
}
/// <summary>
/// enumerated Data for Dependent Menu Levels
/// </summary>
public enum DependentMenuLevel
{
    Account,
    ProgramPeriod,
    Policy
}

/// <summary>
/// This class is used to hold Session Level Master Values such as
/// Account Details, Program Period Details
/// </summary>
public class MasterEntities
{
    private string accountName;
    private Boolean masterAccount;
    private int accountNumber;
    private int personIdAries;
    private int masterAccountNumber;
    private string sSCGID;
    private bool? accountStatus;
    private string premiumAdjProgramID;
    private DateTime premiumAdjProgramStartDate;
    private DateTime premiumAdjProgramEndState;
    private string bpnumber;

    private DateTime valuationDate;
    private int adjusmentNumber;
    private DateTime adjustmentDate;
    private string adjustmentStatus;

    private string invoiceNumber;
    private DateTime invoiceDate;

    public MasterEntities()
    {
        accountName = String.Empty;
        masterAccount = false;
        accountNumber = 0;
        personIdAries = 0;
        masterAccountNumber = 0;
        sSCGID = String.Empty;
        premiumAdjProgramID = String.Empty;
        premiumAdjProgramStartDate = new DateTime(1000, 1, 1);
        premiumAdjProgramEndState = new DateTime(1000, 1, 1);
        bpnumber = string.Empty;
        accountStatus = false;
        valuationDate = new DateTime(1000, 1, 1);
        adjusmentNumber = 0;
        adjustmentDate = new DateTime(1000, 1, 1);
        adjustmentStatus = String.Empty;

        invoiceNumber = string.Empty;
        invoiceDate = new DateTime(1000, 1, 1);

        ExcessLoss = new excessLoss();
    }

    public string AccountName
    {
        get { return accountName; }
        set { accountName = value; }
    }
    public int PersonIdAries
    {
        get { return personIdAries; }
        set { personIdAries = value; }
    }
    public bool MasterAccount
    {
        get { return masterAccount; }
        set { masterAccount = value; }
    }

    public string Bpnumber
    {
        get { return bpnumber; }
        set { bpnumber = value; }
    }

    public int AccountNumber
    {
        get { return accountNumber; }
        set { accountNumber = value; }
    }

    public int MasterAccountNumber
    {
        get { return masterAccountNumber; }
        set { masterAccountNumber = value; }
    }

    public string SSCGID
    {
        get { return sSCGID; }
        set { sSCGID = value; }

    }
    public bool? AccountStatus
    {
        get { return accountStatus; }
        set { accountStatus = value; }

    }
    public string PremiumAdjProgramID
    {
        get { return premiumAdjProgramID; }
        set { premiumAdjProgramID = value; }

    }

    public DateTime PremAdjProgramStartDate
    {
        get { return premiumAdjProgramStartDate; }
        set { premiumAdjProgramStartDate = value; }

    }

    public DateTime PremAdjProgramEndState
    {
        get { return premiumAdjProgramEndState; }
        set { premiumAdjProgramEndState = value; }

    }

    public DateTime ValuationDate
    {
        get
        {
            return valuationDate;
        }
        set
        {
            valuationDate = value;
        }
    }

    public int AdjusmentNumber
    {
        get
        {
            return adjusmentNumber;
        }
        set
        {
            adjusmentNumber = value;
        }
    }

    public DateTime AdjustmentDate
    {
        get
        {
            return adjustmentDate;
        }
        set
        {
            adjustmentDate = value;
        }
    }

    public string AdjustmentStatus
    {
        get { return adjustmentStatus; }
        set { adjustmentStatus = value; }
    }

    public string InvoiceNumber
    {
        get { return invoiceNumber; }
        set { invoiceNumber = value; }
    }

    public DateTime InvoiceDate
    {
        get { return invoiceDate; }
        set { invoiceDate = value; }
    }

    #region Used for Invoice Dashboard and Adjustment Review
    private int invoiceDashboardAccountNumber;
    public int InvoiceDashboardAccountNumber
    {
        get { return invoiceDashboardAccountNumber; }
        set { invoiceDashboardAccountNumber = value; }
    }
    #endregion
    #region Used for Loss Information Pages
    public class excessLoss
    {
        public string PolicyEffDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string LOB { get; set; }
        public string State { get; set; }
        public string PolicyNumber { get; set; }
        public int ARMISLossID { get; set; }
        public int ARMISLOssExcID { get; set; }
        public DateTime EffectiveDate1 { get; set; }
        public DateTime EffectiveDate2 { get; set; }
        public int ComlAgmtID { get; set; }
        public string AdjStatus { get; set; }
        //Added to Meet Policy Dates to program Period Dates
        public string ProgramPeriod { get; set; }
        public int PremiumAdjPgmID { get; set; }
        public int PremiumAdjID { get; set; }


    }
    public excessLoss ExcessLoss;
    #endregion

}

/// <summary>
/// Constant Values for External Contact Type
/// </summary>
public struct ExternalContactType
{
    public const string TPA = "TPA";
    public const string INSURED = "INSURED";
    public const string BROKER = "BROKER";
    public const string CAPTIVE = "CAPTIVE";
}

