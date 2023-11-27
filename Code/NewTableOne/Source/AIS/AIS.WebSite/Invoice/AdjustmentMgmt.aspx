<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="Invoice_AdjustmentMgmt"
    CodeBehind="AdjustmentMgmt.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="GAILabel" runat="server" Text="Adjustment Management" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server"  >
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup
        {
            width: 250px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        
var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager != null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    
    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=pnlAdjMgmtDtls.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=pnlAdjMgmtDtls.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    }      
    
        function disableRevisionVoidCheck(spanChk) {
            if (spanChk == 'Revision') {
                var ddlRev = $get('<%=ddlRevisionID.ClientID %>');
                var ddlRevtext = ddlRev.options[ddlRev.selectedIndex].text;
                var theBox = $get('<%=chkBxReavisionReasonInd.ClientID %>');
                xState = theBox.checked;
                if (xState == true || ddlRevtext != "(Select)" ) {
                    $get('<%=chkBxVoidReasonInd.ClientID %>').disabled = true;
                    $get('<%=ddVoidReason.ClientID %>').disabled = true;
                }
                else {
                    $get('<%=chkBxVoidReasonInd.ClientID %>').disabled = false;
                    $get('<%=ddVoidReason.ClientID %>').disabled = false;
                }
            }
            else {
                var theBox = $get('<%=chkBxVoidReasonInd.ClientID %>');
                xState = theBox.checked;
                var ddlVoid = $get('<%=ddVoidReason.ClientID %>');
                var ddlVoidtext = ddlVoid.options[ddlVoid.selectedIndex].text;
                if (ddlVoidtext != "(Select)" || xState == true) {
                    $get('<%=chkBxReavisionReasonInd.ClientID %>').disabled = true;
                    $get('<%=ddlRevisionID.ClientID %>').disabled = true;
                }

                else {
                    $get('<%=chkBxReavisionReasonInd.ClientID %>').disabled = false;
                    $get('<%=ddlRevisionID.ClientID %>').disabled = false;
                }
            }

        }
        
 function CofirmAction(operation)
        {
        var invoicenumber=$get('<%=txtInvNumber.ClientID %>').value;
        var customer=$get('<%=txtAcctName.ClientID %>').value;
        var str='Do you want to '+operation+' Invoice '+invoicenumber+' for '+customer+' ?';
        var strConfirm;
        //SR-321581
        if (operation == 'Revise')
        {
            strConfirm='Revision';
        }
        else if (operation == 'Void')
        {
            strConfirm='Void';
        } 
                if(confirm(str)) 
                {
                     //SR-321581
                     if ($get('<% =hidLaunch.ClientID%>').value > 0 && operation!='Cancel')
                     {
                     
                    if(confirm(''+ strConfirm +' will not be performed for this account as this account is not set with BP Number. Do you want to proceed to Account Info screen to update the BP Number now?'))
                    {
                    window.location.href="../AcctSetup/AcctInfo.aspx?wID=<%= WindowName%>";
                    return false;
                    }
                    else
                    {
                     return false;
                    }
                    }
                    else
                    {
                    document.getElementById('ctl00_hdnControlDirty').value = 0;
                    if (operation == 'Void') {
                    <%=Page.GetPostBackEventReference(btnVoidInv)%>
                        
                    }
                    else if (operation == 'Revise') {
                    <%=Page.GetPostBackEventReference(btnRevseInv)%>
                    
                    }
                    else if (operation == 'Cancel') {
                    <%=Page.GetPostBackEventReference(btnCnclinvoice)%>
                    }
                    $get('<%=btnVoidInv.ClientID %>').disabled = true;
                    $get('<%=btnRevseInv.ClientID %>').disabled = true;
                    $get('<%=btnCnclinvoice.ClientID %>').disabled = true;
                    document.body.style.cursor = 'wait';
                    var x=document.getElementsByTagName("input");
                    for (var i=0;i<x.length;i++)
                    {	x[i].style.cursor='wait';}
                    if(document.getElementById('<%=chkBxReavisionReasonInd.ClientID %>').checked)
                    return true;
                    }
                }
                else
                {
                return false;
                }
            return true;
        }

  

 



    
function CheckFile()
{
var file = document.getElementById('<%=uploadZDW.ClientID %>');
var len=file.value.length;
$get('<%=hidfiletxt.ClientID%>').value=file.value;
var ext=file.value;
    if(len ==0)
    {
    alert('Please select a file to import');
    document.getElementById('<%=uploadZDW.ClientID %>').focus();
    return false;
    }
    else if(ext.substr(len-3,len)!="pdf" && ext.substr(len-3,len)!="xls" && ext.substr(len-3,len)!="doc" && ext.substr(len-4,len)!="xlsx" && ext.substr(len-4,len)!="docx"  )
    {
    alert("Please select a doc or xls or pdf file ");
    return false;
    } 
}
    function Validate()
    {
    
    
    }
    var oldgridSelectedColor;
    var oldgridClickedColor;
    var oldElement;

    function setMouseOverColor(element)
    {
        oldgridSelectedColor = element.style.backgroundColor;
        element.style.backgroundColor='lightblue';
        element.style.cursor='hand';
        element.style.textDecoration='underline';
    }

    function setMouseOutColor(element)
    {
        element.style.backgroundColor=oldgridSelectedColor;
        element.style.textDecoration='none';
    }
    function SetMouseClickColor(element)
    {
        if(oldElement != null)
        {
        oldElement.style.backgroundColor=oldgridSelectedColor;
        }
        oldElement=element;
        oldgridSelectedColor= element.style.backgroundColor;
        element.style.backgroundColor='yellow';
        element.style.cursor='hand';
    }
    function validateSearch()
    {
    txtfrom=$get('<%=txtInvoiceDateFrm.ClientID%>');
    txtTo=$get('<%=txtInvoiceDateTo.ClientID%>');
    v1=$get('<%=reqFromDate.ClientID %>');
    v2=$get('<%=reqvalTo.ClientID %>');
    ValidatorEnable(v1,false);
    ValidatorEnable(v2,false);
    if(txtfrom.value =="" && txtTo.value =="")
    {
    }
    else if(txtfrom.value != "" && txtfrom.value !="__/__/____" && (txtTo.value == "" || txtTo.value == "__/__/____")  )
    {
    ValidatorEnable(v2,true);
    }
    else if((txtfrom.value == "" || txtfrom.value == "__/__/____") && txtTo.value !="__/__/____" && txtTo.value != "" )
    {
    ValidatorEnable(v1,true);
    }
    }
     
    </script>

    <asp:ValidationSummary ID="valSave" runat="server" ValidationGroup="Search" BorderColor="Red"
        BorderStyle="Solid" BorderWidth="1" />
    <asp:ValidationSummary ID="valSumRevise" runat="server" ValidationGroup="Revise"
        BorderColor="Red" BorderStyle="Solid" BorderWidth="1" />
    <asp:ValidationSummary ID="ValSumVoid" runat="server" ValidationGroup="Void" BorderColor="Red"
        BorderStyle="Solid" BorderWidth="1" />
        <br />
    <asp:Panel BorderColor="Black" ID="PnlSearchdtls" BorderWidth="1" Width="910px" runat="server" DefaultButton="btnAdjMgmtSearch">
        <table>
            <tr>
                <td style="width: 102px; vertical-align: middle">
                    Account Name :
                </td>
                <td style="vertical-align: top">
                    <AL:AccountList ID="ddlAcctlist" runat="server" />
                </td>
                <td style="width: 150px; vertical-align: middle">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Adjustment Status:
                </td>
                <td style="vertical-align: middle">
                    <asp:ObjectDataSource ID="ObjAdjStatus" runat="server" SelectMethod="GetLookUpActiveData"
                        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="ADJUSTMENT STATUSES" Name="lookUpTypeName" Type="String" />
                            <asp:Parameter DefaultValue="" Name="attribute" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:DropDownList ID="ddAdjStatus" runat="server" Width="231px" DataSourceID="ObjAdjStatus"
                        DataTextField="LookUpName" DataValueField="LookUpID">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Invoice Number :
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txtInvoiceNmber" runat="server" Width="220px" MaxLength="15"></asp:TextBox>
                    <AjaxToolKit:FilteredTextBoxExtender ID="flttxtInvoiceNmber" runat="server" TargetControlID="txtInvoiceNmber"
                        FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" />
                </td>
                <td style="width: 150px;">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;User :
                </td>
                <td>
                    <!-- <asp:ObjectDataSource ID="objUserLst" runat="server" SelectMethod="getPersonsList"
                    TypeName="ZurichNA.AIS.Business.Logic.PersonBS"></asp:ObjectDataSource>-->
                    <asp:DropDownList ID="ddUserLst" runat="server" Width="231px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 102px; vertical-align: middle">
                    Invoice Date From :
                </td>
                <td>
                    &nbsp;<asp:TextBox ValidationGroup="Search" runat="server" ID="txtInvoiceDateFrm"
                        MaxLength="10" Width="100px" onblur="validateSearch();"></asp:TextBox>
                    <AjaxToolKit:CalendarExtender ID="calInvoiceDateFrm" runat="server" PopupPosition="BottomRight"
                        TargetControlID="txtInvoiceDateFrm"  PopupButtonID="imgInvoiceDateFrm" />
                    <asp:ImageButton ID="imgInvoiceDateFrm" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <asp:RegularExpressionValidator ID="regFromDate" runat="server" ControlToValidate="txtInvoiceDateFrm"
                        ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                        ErrorMessage="Please enter valid Invoice From Date" Text="*" ValidationGroup="Search"></asp:RegularExpressionValidator>
                    <AjaxToolKit:MaskedEditExtender ID="mskFromDate" runat="server" TargetControlID="txtInvoiceDateFrm"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:RequiredFieldValidator ID="reqFromDate" runat="server" ErrorMessage="Please enter Invoice Date From"
                        ValidationGroup="Search" ControlToValidate="txtInvoiceDateFrm" Enabled="false"
                        Text="*"></asp:RequiredFieldValidator>
                </td>
                <td style="vertical-align: middle">
                    &nbsp;&nbsp;&nbsp;&nbsp;Invoice Date To :
                </td>
                <td>
                    <asp:TextBox ValidationGroup="Search" runat="server" ID="txtInvoiceDateTo" MaxLength="10"
                        Width="100px" onblur="validateSearch();"></asp:TextBox>
                    <AjaxToolKit:CalendarExtender ID="calInvoiceDateTo" runat="server" PopupPosition="BottomRight"
                        TargetControlID="txtInvoiceDateTo" PopupButtonID="imgInvoiceDateTo" />
                    <asp:ImageButton ID="imgInvoiceDateTo" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <asp:RequiredFieldValidator ID="reqvalTo" runat="server" ErrorMessage="Please enter valid Invoice To Date"
                        ValidationGroup="Search" ControlToValidate="txtInvoiceDateTo" Enabled="false"
                        Text="*"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="compFromDate" ValidationGroup="Search" runat="server" ControlToCompare="txtInvoiceDateFrm"
                        ControlToValidate="txtInvoiceDateTo" Operator="GreaterThan" Type="Date" ErrorMessage="To date should be greater than From date">*</asp:CompareValidator>
                    <asp:RegularExpressionValidator ID="regExpiryDate" runat="server" ControlToValidate="txtInvoiceDateTo"
                        ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                        ErrorMessage="Please enter valid Invalid To Date" Text="*" ValidationGroup="Search"></asp:RegularExpressionValidator>
                    <AjaxToolKit:MaskedEditExtender ID="mskToDate" runat="server" TargetControlID="txtInvoiceDateTo"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAdjMgmtSearch" runat="server" Width="90px"
                        Text="Search" ValidationGroup="Search" OnClick="btnAdjMgmtSearch_Click" />
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAdjMgmtClear" runat="server" Width="90px"
                        Text="Clear" OnClick="btnAdjMgmtClear_Click" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 102px;">
                    Valuation Date :
                </td>
                <td>
                    &nbsp;<asp:TextBox runat="server" ID="txtValuationDate" MaxLength="10" Width="100px"></asp:TextBox>
                    <AjaxToolKit:CalendarExtender ID="caltxtValuationDate" runat="server" PopupPosition="BottomRight"
                        TargetControlID="txtValuationDate" PopupButtonID="imgValuationDate" />
                    <asp:ImageButton ID="imgValuationDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <asp:RegularExpressionValidator ID="regVandate" runat="server" ControlToValidate="txtValuationDate"
                        ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                        ErrorMessage="Invalid Valuation Date" Text="*" ValidationGroup="Search"></asp:RegularExpressionValidator>
                    <AjaxToolKit:MaskedEditExtender ID="mskvalndate" runat="server" TargetControlID="txtValuationDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="DataSrcePendingType" runat="server" SelectMethod="GetLookUpActiveData"
        TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
        <SelectParameters>
            <asp:Parameter Name="lookUpTypeName" Type="String" DefaultValue="PENDING REASON CODES" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%--<asp:UpdatePanel runat="server" ID="updInternal">
        <ContentTemplate>--%>
    <asp:Panel ID="pnlAdjMgmtDtls" runat="server" Height="100px" ScrollBars="Auto" Width="910px">
        <asp:AISListView ID="lstAdjMgmtDtl" runat="server" OnSelectedIndexChanging="lstAdjMgmtDtl_SelectedIndexChanging"
            OnItemDataBound="lstAdjMgmtDtl_DataBoundList" OnSorting="lstAdjMgmtDtl_Sorting"
            OnItemCommand="lstAdjMgmtDtl_ItemCommand">
            <LayoutTemplate>
                <table id="lstviewtable" class="panelContents" runat="server" width="98%">
                    <tr class="LayoutTemplate">
                        <th>
                        </th>
                        <th>
                            Acct. Num.
                        </th>
                        <th>
                            Account Name
                        </th>
                        <th>
                            <asp:LinkButton ID="hlValdtsrt" runat="server" CommandName="Sort" CommandArgument="ValtnDate">
                            Valuation Date
                            </asp:LinkButton>
                            <asp:ImageButton ID="imgValDtSrt" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                runat="server" />
                        </th>
                        <th>
                            Adjust. Number
                        </th>
                        <th>
                            <asp:LinkButton ID="hlInvNmber" runat="server" CommandName="Sort" CommandArgument="DrftInvoicenmr">
                            Invoice Number
                            </asp:LinkButton>
                            <asp:ImageButton ID="imgDrftInvNum" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                runat="server" />
                        </th>
                        <th>
                            <asp:LinkButton ID="hlInvDate" runat="server" CommandName="Sort" CommandArgument="DrftInvoiceDate">
                            Invoice Date
                            </asp:LinkButton>
                            <asp:ImageButton ID="imgDrftInvDate" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                runat="server" />
                        </th>
                        <th>
                            <asp:LinkButton ID="hlAdjustStatus" runat="server" CommandName="Sort" CommandArgument="Adjuststatus">
                            Adj. Status
                            </asp:LinkButton>
                            <asp:ImageButton ID="imgAdjustStatus" Visible="false" ToolTip="Ascending" ImageUrl="~/images/ascending.gif"
                                runat="server" />
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                    <td>
                        <asp:HiddenField ID="HidAdjMgmtID" runat="server" Value='<%# Bind("prem_adjID") %>' />
                        <asp:LinkButton ID="lnkSelect" CommandName="SELECT" runat="server" Text="Select"></asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("custmrID")%>
                    </td>
                    <td>
                        <%# Eval("CustomerFullName") %>
                    </td>
                    <td>
                        <%# Eval("ValtnDate", "{0:d}") %>
                    </td>
                    <td>
                        <asp:HiddenField ID="hdAdjMgmtNumb" runat="server" Value='<%# Bind("AdjMgmtStatusNumber") %>' />
                        <%# Eval("prem_adjID")%>
                    </td>
                    <td>
                        <asp:Label ID="lblInvNumber" runat="server" Text='<%# Eval("DrftInvoicenmr") %>' />
                    </td>
                    <td>
                        <%# Eval("DrftInvoiceDate","{0:d}") %>
                    </td>
                    <td>
                        <asp:Label ID="lblAdjStatus" Visible="false" Text='<%# Bind("Adjuststatus") %>' runat="server"></asp:Label>
                        <%# Eval("Adjuststatus") %>
                        <asp:Label ID="lblPGMID" Visible="false" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                    <td>
                        <asp:HiddenField ID="HidAdjMgmtID" runat="server" Value='<%# Bind("prem_adjID") %>' />
                        <asp:LinkButton ID="lnkSelect" CommandName="SELECT" runat="server" Text="Select"></asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("custmrID") %>
                    </td>
                    <td>
                        <%# Eval("CustomerFullName") %>
                    </td>
                    <td>
                        <%# Eval("ValtnDate","{0:d}") %>
                    </td>
                    <td>
                        <asp:HiddenField ID="hdAdjMgmtNumb" runat="server" Value='<%# Bind("AdjMgmtStatusNumber") %>' />
                        <%# Eval("prem_adjID")%>
                    </td>
                    <td>
                        <asp:Label ID="lblInvNumber" runat="server" Text='<%# Eval("DrftInvoicenmr") %>' />
                    </td>
                    <td>
                        <%# Eval("DrftInvoiceDate","{0:d}") %>
                    </td>
                    <td>
                        <asp:Label ID="lblAdjStatus" Visible="false" Text='<%# Bind("Adjuststatus") %>' runat="server"></asp:Label>
                        <%# Eval("Adjuststatus") %>
                        <asp:Label ID="lblPGMID" Visible="false" runat="server" Text='<%# Bind("PREM_ADJ_PGM_ID") %>'></asp:Label>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:AISListView>
    </asp:Panel>
    <div style="padding-top: 5px">
        <asp:Label ID="lblAdjustmentMgmt" Visible="false" runat="server" Text="Adjustment Management Details"
            CssClass="h2"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkClose" runat="server"
                Visible="false" Text="Close" OnClick="lnkClose_Click"></asp:LinkButton>
    </div>
    <asp:Panel BorderColor="Black" ID="pnlDetails" Visible="false" BorderWidth="1" Width="910px"
        runat="server">
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:HiddenField ID="hidindex" runat="server" Value="-1" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    Account Number:
                </td>
                <td>
                    <asp:TextBox ID="txtAcctNumber" runat="server" ReadOnly="true" Width="110px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Account Name:
                </td>
                <td>
                    <asp:TextBox ID="txtAcctName" runat="server" ReadOnly="true" Width="286px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Invoice Number:
                </td>
                <td>
                    <asp:TextBox ID="txtInvNumber" runat="server" ReadOnly="true" Width="110px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Invoice Date:
                </td>
                <td>
                    <asp:TextBox ID="txtInvDate" runat="server" ReadOnly="true" Width="110px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Adjustment Number:
                </td>
                <td>
                    <asp:TextBox ID="txtAdjmtNumber" runat="server" ReadOnly="true" Width="110px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Adjustment Status:
                </td>
                <td>
                    <asp:TextBox ID="txtAdjmtStuts" runat="server" ReadOnly="true" Width="110px"></asp:TextBox>
                </td>
            </tr>
            <table width="800px">
                <tr>
                    <td style="width: 150px">
                        Revise Indicator:
                    </td>
                    <td>
                        <asp:CheckBox ValidationGroup="Revise" onclick="javascript:disableRevisionVoidCheck('Revision');" ID="chkBxReavisionReasonInd" runat="server"
                            Enabled="false"></asp:CheckBox>
                    </td>
                    <td style="width: 150px">
                        Revise Reason:
                    </td>
                    <td align="left">
                        <asp:ObjectDataSource ID="RevisionTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="REVISION REASON" Name="lookUpTypeName" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:DropDownList ID="ddlRevisionID" runat="server" Enabled="false" Width="200px"
                            DataSourceID="RevisionTypeDataSource" ValidationGroup="Revise" DataTextField="LookUpName"
                            DataValueField="LookUpID" OnChange="javascript:disableRevisionVoidCheck('Revision');">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Valuation Date:
                    </td>
                    <td>
                        <asp:TextBox ID="txtValutiondtlsDate" runat="server" ReadOnly="true" Width="80px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">
                        Void Indicator:
                    </td>
                    <td>
                        <asp:CheckBox  ValidationGroup="Void" onclick="javascript:disableRevisionVoidCheck('Void');" ID="chkBxVoidReasonInd" runat="server" Enabled="false">
                        </asp:CheckBox>
                    </td>
                    <td style="width: 150px">
                        Void Reason:
                    </td>
                    <td align="left">
                        <asp:ObjectDataSource ID="objVoidReason" runat="server" SelectMethod="GetLookUpActiveData"
                            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="VOID REASON" Name="lookUpTypeName" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:DropDownList ID="ddVoidReason" Enabled="false" runat="server" Width="200px"
                            DataSourceID="objVoidReason" ValidationGroup="Void" DataTextField="LookUpName"
                            DataValueField="LookUpID" OnChange="javascript:disableRevisionVoidCheck('Void');">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp; Historical Indicator:
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHistrorical" runat="server" Enabled="false"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px">
                        Pending Indicator:
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkBkPendingInv" Enabled="false" runat="server"></asp:CheckBox>
                    </td>
                    <td style="width: 150px">
                        Pending Reason:
                    </td>
                    <td align="left">
                        <asp:ObjectDataSource ID="objPndReasonID" runat="server" SelectMethod="GetLookUpActiveData"
                            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="PENDING REASON CODES" Name="lookUpTypeName" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:DropDownList ID="ddPndReason" Enabled="false" Width="200px" runat="server" DataSourceID="objPndReasonID"
                            ValidationGroup="Save" DataTextField="LookUpName" DataValueField="LookUpID">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Set 20% QC:
                    </td>
                    <td>
                        <asp:CheckBox ID="chkbxSetQc" runat="server"></asp:CheckBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="text-align: center; padding-left: 80px;">
                    <asp:HiddenField runat="server" ID="hidLaunch" Value="0"/>
                        <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" ToolTip="Please click here to save Adjustment Management Details" />
                        <asp:Button ID="btnCancel" ToolTip="Please click here to Cancel" runat="server" Text="Cancel"
                            OnClick="btnCancel_Click" />
                        <asp:Button ID="btnCnclinvoice" ToolTip="Please click here to cancel Invoice" runat="server"
                            Text="Cancel Invoice" OnClientClick="return CofirmAction('Cancel');"
                            OnClick="btnCanclInvoice_Click" />
                        <asp:Button ID="btnRevseInv" ToolTip="Please click here to Revise Invoice" runat="server"
                            Text="Revise Invoice"  OnClick="btnRevisInvoice_Click"
                            OnClientClick="return CofirmAction('Revise'); " />
                        <asp:Button ID="btnVoidInv" ToolTip="Please click here to Void Invoice" runat="server"
                            Text="Void Invoice" Enabled="false"  OnClick="btnVoidInvoice_Click" 
                            OnClientClick="return CofirmAction('Void'); " />
                        <asp:Button ID="btnDetails" ToolTip="Please click here to see Invoicing Dashboard"
                            runat="server" Text="Details" OnClick="btnDetails_Click" />
                        <asp:Button ID="btnlssinfo" ToolTip="Please click here to see Loss Info details"
                            runat="server" Text="Loss Info" OnClick="btnLossInfo_Click" />
                        <asp:Button ID="btnUploadZDW" ToolTip="Please click here to Upload addition documents to ZDW"
                            runat="server" Text="Upload to ZDW" />
                        <AjaxToolKit:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                            TargetControlID="btnUploadZDW" PopupControlID="pnlZDW" BackgroundCssClass="modalBackground"
                            DropShadow="true" PopupDragHandleControlID="programmaticPopupDragHandle" RepositionMode="RepositionOnWindowScroll"
                            CancelControlID="btnZdwCancel">
                        </AjaxToolKit:ModalPopupExtender>
                        <div style="float: left;">
                            <asp:Panel runat="server" CssClass="modalPopup" ID="pnlZDW" Style="border: solid 1px black;
                                display: none; width: 350px; padding: 0px" HorizontalAlign="Center">
                                <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="width: 100%; cursor: move;
                                    padding: 0px; background-color: #CCCCCC; height: 20px; border: solid 1px Gray;
                                    color: Black; text-align: center; vertical-align: middle">
                                    <b style="vertical-align: middle; font-size: 12px">Please select file to upload to ZDW</b></asp:Panel>
                                <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                                    <br />
                                    <br />
                                    
                                    <asp:FileUpload runat="server" ID="uploadZDW"></asp:FileUpload>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnUploadOk" OnClientClick="javascript:return CheckFile();" runat="server"
                                        Text="UpLoad" OnClick="btnUploadZDW_Click" />
                                    <asp:Button ID="btnZdwCancel" runat="server" Text="Cancel" />
                                    <asp:HiddenField ID="hidfiletxt" runat="server" />
                                    <br />
                                </div>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </table>
    </asp:Panel>
   <%-- </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnUploadOk" />
        <asp:PostBackTrigger ControlID="btnUploadZDW" />
        </Triggers>
    </asp:UpdatePanel>--%>
   
</asp:Content>
