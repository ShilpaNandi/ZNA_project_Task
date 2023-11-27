<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AppMgmt_InterfaceStatus"
    Title="Interface Adjustment Status Page" CodeBehind="InterfaceStatus.aspx.cs"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <asp:Label ID="GAILabel" runat="server" Text="Interface/Adjustment Status" CssClass="h1"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
 <script language="javascript" type="text/javascript">

    var scrollTop1;
    
    if(Sys.WebForms.PageRequestManager!=null)
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function BeginRequestHandler(sender, args) 
    {
        var mef = $get('<%=Panel1.ClientID%>');
        if(mef!=null)
        scrollTop1 = mef.scrollTop;
    }

    function EndRequestHandler(sender, args)
    {
        var mef = $get('<%=Panel1.ClientID%>');
        if(mef!=null)
        mef.scrollTop = scrollTop1;
    } 
</script>

    <table >
        <tr valign="bottom">
            <td align="right" style="vertical-align:bottom" >
                <asp:Label ID="Label1" runat="server" Width="40px"  Text="Account Name:"></asp:Label>
            </td>
            <td style="vertical-align:bottom">
                <asp:Panel ID="Panel2" runat="server">
                    <AL:AccountList ID="ddlAcctlist" runat="server" />
                </asp:Panel>
            </td>
            <td style="vertical-align:bottom">
                <asp:Label ID="Label2" runat="server" Text="Type:"></asp:Label>
            </td>
            <td style="vertical-align:bottom">
                <asp:DropDownList ID="ddlInterfaceType" runat="server" AutoPostBack="false" Width="153px">
                    <asp:ListItem Value="0">(Select)</asp:ListItem>
                    <asp:ListItem Value="1">AIS Calculation Engine</asp:ListItem>
                    <asp:ListItem Value="2">AIS Invoice Driver</asp:ListItem>
                    <asp:ListItem Value="3">ARiES Interface</asp:ListItem>
                    <asp:ListItem Value="4">ARMIS Interface</asp:ListItem>
                    <asp:ListItem Value="5">AIS TPA/Manual</asp:ListItem>
                    <asp:ListItem Value="6">AIS Adjustment Cancel Driver</asp:ListItem>
                    
                </asp:DropDownList>
            </td>
            <td style="vertical-align:bottom" align="right">
                <asp:Label  ID="lblFromDate" Text="Date From:" runat="server" Width="40px"></asp:Label>
            </td>
            <td style="vertical-align:bottom">
                <asp:TextBox ID="txtFromDate" runat="server" ValidationGroup="Save" MaxLength="10" Width="70px"></asp:TextBox>
                <AjaxToolKit:CalendarExtender ID="calValidFrom" runat="server" PopupPosition="TopRight"
                    TargetControlID="txtFromDate" PopupButtonID="imgValidFrom" />
                <asp:ImageButton ID="imgValidFrom" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                    CausesValidation="False" />
                <AjaxToolKit:MaskedEditExtender ID="mskEditFrom" runat="server" TargetControlID="txtFromDate"
                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError" MaskType="Date" CultureName="en-US" DisplayMoney="Left"
                    AcceptNegative="Left" ErrorTooltipEnabled="True" AutoComplete="false" />
                <asp:RegularExpressionValidator ID="regValidFrom" runat="server" ControlToValidate="txtFromDate"
                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                    ErrorMessage="Invalid Valid From Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                <asp:CompareValidator ID="CompareValidator2" ControlToCompare="txtToDate" Text="*"
                    runat="server" ControlToValidate="txtFromDate" Operator="LessThan" Type="Date"
                    ErrorMessage="FROM date cannot be greater than TO date" ValidationGroup="search"></asp:CompareValidator>
            </td>
            <td style="vertical-align:bottom" align="right">
                <asp:Label ID="lblToDate" Text="Date To:" runat="server" Width="40px"></asp:Label>
            </td>
            <td style="vertical-align:bottom">
                <asp:TextBox ID="txtToDate" runat="server" ValidationGroup="Save" Width="70px" MaxLength="10"></asp:TextBox>
                <AjaxToolKit:CalendarExtender ID="calValidTO" runat="server" PopupPosition="TopRight"
                    TargetControlID="txtToDate" PopupButtonID="imgValidTo" />
                <asp:ImageButton ID="imgValidTo" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                    CausesValidation="False" />
                <AjaxToolKit:MaskedEditExtender ID="mskEditTo" runat="server" TargetControlID="txtToDate"
                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Right" AcceptNegative="Left"
                    AutoComplete="false" ErrorTooltipEnabled="True" />
                <asp:RegularExpressionValidator ID="regValidTo" runat="server" ControlToValidate="txtToDate"
                    ValidationExpression="(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](1[3-9]|2[01])\d\d"
                    ErrorMessage="Invalid Valid To Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
            </td>
            <td style="vertical-align:bottom">
                <asp:Button ID="cmdSearch" runat="server" Text="Search" Width="50px" OnClick="btnSearch_Click" />
            </td>
        </tr>
        <br />
    </table>
    <asp:Panel ID="Panel1" Width="910px" runat="server" ScrollBars="Auto" Height="350px">
        <asp:ListView ID="lstInternalMasters" runat="server">
            <LayoutTemplate>
                <table id="tblLayout" class="panelContents" runat="server" width="890px">
                    <tr class="LayoutTemplate">
                        <th>
                            Type
                        </th>
                        <th>
                            Acct Number
                        </th>
                        <th>
                            Sev
                        </th>
                        <th>
                            Short Description
                        </th>
                        <th>
                            Long Description
                        </th>
                        <th>
                            Create Date
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="ItemTemplate" runat="server" id="trItemTemplate">
                    <td runat="server" align="left" id="Td5">
                        <asp:Label ID="Label7" Width="60px" runat="server" Text='<%# Bind("SRC_TXT") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td4">
                        <asp:Label ID="Label6" Width="20px" runat="server" Text='<%# Bind("CUSTMR_ID") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td3">
                        <asp:Label ID="Label5" Width="20px" runat="server" Text='<%# Bind("SEV_CD") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td2">
                        <asp:Label ID="Label4" Width="100px" runat="server" Text='<%# Bind("SHORT_DESC_TXT") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="test2">
                        <asp:Label ID="FullDesc" Width="440px" runat="server" Text='<%# Bind("FULL_DESC_TXT") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td1">
                        <asp:Label ID="Label3" Width="100px" runat="server" Text='<%# Bind("CREATE_DATE") %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="AlternatingItemTemplate" runat="server" id="trItemTemplate">
                    <td runat="server" align="left" id="Td5">
                        <asp:Label ID="Label7" Width="60px" runat="server" Text='<%# Bind("SRC_TXT") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td4">
                        <asp:Label ID="Label6" Width="20px" runat="server" Text='<%# Bind("CUSTMR_ID") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td3">
                        <asp:Label ID="Label5" Width="20px" runat="server" Text='<%# Bind("SEV_CD") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td2">
                        <asp:Label ID="Label4" Width="100px" runat="server" Text='<%# Bind("SHORT_DESC_TXT") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="test2">
                        <asp:Label ID="FullDesc" Width="440px" runat="server" Text='<%# Bind("FULL_DESC_TXT") %>'></asp:Label>
                    </td>
                    <td runat="server" align="left" id="Td1">
                        <asp:Label ID="Label3" Width="100px" runat="server" Text='<%# Bind("CREATE_DATE") %>'></asp:Label>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:ListView>
    </asp:Panel>
</asp:Content>
