<%@ Page Language="C#" MasterPageFile="~/Retro.master" EnableEventValidation="false"
    AutoEventWireup="true" Inherits="AppMgmt_TPAPostingMgmt" CodeBehind="TPAPostingMgmt.aspx.cs" %>

<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="GAILabel" runat="server" Text="TPA Posting Management" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
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
        function testVoid() {
            $get('<%=btnCancel.ClientID%>').click();
            $get('<%=btnOk.ClientID%>').click();
        }
        function testRevise() {
            $get('<%=btnCancelRevisepopup.ClientID%>').click();
            $get('<%=btnOKRevise.ClientID%>').click();
        }
        function VoidConfirmation(str, btn) {
            if (confirm(str)) {
                $get(btn).click();
                return false;
            }
            else {
                return false;
            }

        }
        function ReviseConfirmation(str, btn) {
            if (confirm(str)) {
                $get(btn).click();
                return false;
            }
            else {
                return false;
            }

        }
    </script>

    <asp:UpdatePanel runat="server" ID="upAccountDashboard">
        <ContentTemplate>
            <asp:ValidationSummary ID="valsSearch" runat="server" ValidationGroup="search" BorderColor="Red"
                BorderStyle="Solid" BorderWidth="1" />
            <asp:ObjectDataSource ID="TPANameDataSource" runat="server" SelectMethod="GetPersonNames"
                TypeName="ZurichNA.AIS.DAL.Logic.PersonDA"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="BuOfficeDataSource" runat="server" SelectMethod="GetBUOffForLookups"
                TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="InvoiceTypeDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                <SelectParameters>
                    <asp:Parameter DefaultValue="INVOICE TYPE" Name="lookUpTypeName" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <table>
                <tr>
                    <td>
                        <asp:Panel BorderColor="Black" ID="PnlSearchdtls" BorderWidth="1" Width="910px" runat="server"
                            DefaultButton="btnSearch">
                            <table>
                                <tr>
                                    <td align="right" style="vertical-align: middle">
                                        <asp:Label ID="lblAccountName" Text="Account Name :" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: top">
                                        <AL:AccountList ID="ddlAcctlist" runat="server" AccountType="0" />
                                    </td>
                                    <td style="vertical-align: middle" align="right">
                                        <asp:Label ID="lblTPAName" Text="TPA Name:" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle">
                                        <asp:DropDownList ID="ddlTPAName" runat="server" Width="228px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 30px">
                                    </td>
                                    <td align="right" rowspan="3" valign="bottom">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Height="18px"
                                                        Text="Search" Width="83px" ValidationGroup="search" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClear" runat="server" Height="18px" Text="Clear" Width="83px"
                                                        OnClick="btnClear_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom" align="right">
                                        <asp:Label ID="lblInvNum" Text="Invoice Number :" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;<asp:TextBox ID="txtInvNum" MaxLength="15" ValidationGroup="update" runat="server"
                                            Width="223px"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender TargetControlID="txtInvNum" FilterType="UppercaseLetters, LowercaseLetters, Numbers,Custom"
                                            ValidChars=" " ID="fltrtxtInvNum" runat="server" />
                                    </td>
                                    <td valign="bottom" align="right">
                                        <asp:Label ID="lblBuOffice" Text="BU/Office:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBuOffice" runat="server" DataSourceID="BuOfficeDataSource"
                                            DataTextField="LookUpName" DataValueField="LookUpID" Width="228px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom" align="right">
                                        <asp:Label ID="lblInvType" Text="Invoice Type :" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;<asp:DropDownList ID="ddlInvType" runat="server" DataSourceID="InvoiceTypeDataSource"
                                            DataTextField="LookUpName" DataValueField="LookUpID" Width="228px">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="bottom" align="right">
                                        <asp:Label ID="lblValnDate" Text="Valuation Date:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValnDate" MaxLength="10" runat="server" SkinID="largeTextbox"
                                            TabIndex="4" Width="200px"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtValnDt_CalendarExtender" runat="server" TargetControlID="txtValnDate"
                                            PopupButtonID="imgValndate" Enabled="True" PopupPosition="TopRight" Format="MM/dd/yyyy">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:ImageButton ID="imgValndate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                            CausesValidation="False" />
                                        <ajaxToolkit:MaskedEditExtender ID="msktxtValnDate" runat="server" TargetControlID="txtValnDate"
                                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                            ErrorTooltipEnabled="True" />
                                        <asp:RegularExpressionValidator ID="regExptxtValnDate" runat="server" ControlToValidate="txtValnDate"
                                            ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                            ErrorMessage="Invalid Date" Text="*" ValidationGroup="search"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom" align="right">
                                        <asp:Label ID="lblFromDate" Text="Invoice Date From:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;<asp:TextBox ID="txtFromDate" MaxLength="10" runat="server" SkinID="largeTextbox"
                                            Width="200px" TabIndex="4"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="imgFromdate" Enabled="True" PopupPosition="TopRight" Format="MM/dd/yyyy">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:ImageButton ID="imgFromdate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                            CausesValidation="False" />
                                        <ajaxToolkit:MaskedEditExtender ID="msktxtFromDate" runat="server" TargetControlID="txtFromDate"
                                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                            ErrorTooltipEnabled="True" />
                                        <asp:RegularExpressionValidator ID="regValFromDate" runat="server" ControlToValidate="txtFromDate"
                                            ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                            ErrorMessage="Invalid From Date" Text="*" ValidationGroup="search"></asp:RegularExpressionValidator>
                                    </td>
                                    <td valign="bottom" align="right">
                                        <asp:Label ID="lblToDate" Text="Invoice Date To:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" MaxLength="10" runat="server" SkinID="largeTextbox" TabIndex="4"
                                            Width="200px"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtToDt_CalendarExtender" runat="server" TargetControlID="txtToDate"
                                            PopupButtonID="imgTodate" Enabled="True" PopupPosition="TopRight" Format="MM/dd/yyyy">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:ImageButton ID="imgTodate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                            CausesValidation="False" />
                                        <ajaxToolkit:MaskedEditExtender ID="msktxtToDate" runat="server" TargetControlID="txtToDate"
                                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                            ErrorTooltipEnabled="True" />
                                        <asp:RegularExpressionValidator ID="regvaltxtToDate" runat="server" ControlToValidate="txtToDate"
                                            ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                            ErrorMessage="Invalid To Date" Text="*" ValidationGroup="search"></asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="valFromDate" ControlToCompare="txtToDate" Text="*" runat="server"
                                            ControlToValidate="txtFromDate" Operator="LessThan" Type="Date" ErrorMessage="Invoice From date cannot be greater than Inovice To date"
                                            ValidationGroup="search"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="panTpaInfo" runat="server" Height="250px" Width="910px" ScrollBars="Auto"
                            Visible="true">
                            <asp:AISListView ID="lstTpa" runat="server" OnSorting="lstTpa_Sorting" OnItemCommand="CommandList"
                                OnItemDataBound="lstTpa_DataBound">
                                <LayoutTemplate>
                                    <table id="Table1" class="panelContents" runat="server" width="98%">
                                        <tr class="LayoutTemplate">
                                            <th>
                                                Account Number
                                            </th>
                                            <th>
                                                Account Name
                                            </th>
                                            <th align="center">
                                                <asp:LinkButton ID="lnkValnDate" runat="server" CommandName="Sort" CommandArgument="VALUATIONDATE">Valuation Date</asp:LinkButton>
                                                <asp:Image ID="imgValnDate" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                                    Visible="false" />
                                            </th>
                                            <%--    <th>
                                                Adjustment Number
                                            </th>--%>
                                            <th align="center">
                                                <asp:LinkButton ID="lnkInvNum" runat="server" CommandName="Sort" CommandArgument="INVOICENUMBER">Invoice Number</asp:LinkButton>
                                                <asp:Image ID="imgInvNum" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                                    Visible="false" />
                                            </th>
                                            <th>
                                                TPA Name
                                            </th>
                                            <th align="center">
                                                <asp:LinkButton ID="lnkInvType" runat="server" CommandName="Sort" CommandArgument="INVOICETYPE">Invoice Type</asp:LinkButton>
                                                <asp:Image ID="imgInvType" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                                    Visible="false" />
                                            </th>
                                            <th align="center">
                                                <asp:LinkButton ID="lnkBuOffice" runat="server" CommandName="Sort" CommandArgument="BUOFFICE">BU/Office</asp:LinkButton>
                                                <asp:Image ID="imgBuOffice" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                                    Visible="false" />
                                            </th>
                                            <th align="center">
                                                <asp:LinkButton ID="lnkInvDate" runat="server" CommandName="Sort" CommandArgument="INVOICEDATE">Invoice Date</asp:LinkButton>
                                                <asp:Image ID="imgInvDate" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                                    Visible="false" />
                                            </th>
                                            <th>
                                                Cancel Invoice
                                            </th>
                                            <th>
                                                Revise Invoice
                                            </th>
                                            <th>
                                                Void Invoice
                                            </th>
                                            <th>
                                                Details
                                            </th>
                                        </tr>
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr1" runat="server" class="ItemTemplate">
                                        <td>
                                            <asp:Label ID="lblActNum" Width="20px" runat="server" Text='<%# Bind("CustomerID")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblActName" Width="100px" runat="server" Text='<%# Bind("ACC_NAME")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblValnDate" Width="50px" runat="server" Text='<%# Eval("ValuationDate", "{0:d}")%>'></asp:Label>
                                        </td>
                                        <%--  <td>
                                            <asp:Label ID="lblAdjNumber" Width="50px" runat="server" Text='<%# Bind("ThirdPartyAdminManualInvoiceID")%>'></asp:Label>
                                        </td>--%>
                                        <td align="left">
                                            <asp:Label ID="lblInvoiceNumber" Width="20px" runat="server" Text='<%# Bind("InvoiceNumber")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTpaName" Width="70px" runat="server" Text='<%# Bind("TPA_NAME")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvType" Width="50px" runat="server" Text='<%# Bind("INVOICE_TYPE_TEXT")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBuOffice" Width="50px" runat="server" Text='<%# Bind("BU_OFFICE_TEXT")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvDate" Width="50px" runat="server" Text='<%# Eval("InvoiceDate", "{0:d}")%>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblhdFinalizeInd" Visible="false" runat="server" Text='<%#Bind("FinalizedIndicator") %>'></asp:Label>
                                            <asp:Label ID="lblhdCanInd" runat="server" Text='<%#Bind("CancelIndicator") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblCancel" runat="server" Text="CANCEL"></asp:Label>
                                            <asp:LinkButton ValidationGroup="search" ID="lbnCancelInv" Width="20px" runat="server"
                                                Text="CANCEL" CommandName="CancelInv" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblhdReviseInd" runat="server" Text='<%#Bind("ReviseIndicator") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblRevise" runat="server" Text="REVISE"></asp:Label>
                                            <asp:LinkButton ID="lbnReviseInv" Width="20px" runat="server" Text="REVISE" CommandName="Revise"
                                                CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                            <asp:LinkButton Style="display: none" ID="ReviseLink" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'
                                                Width="0px" runat="server" OnClick="btnReviselink_click"></asp:LinkButton>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblhdVoidInd" runat="server" Text='<%#Bind("VoidIndicator") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblVoid" runat="server" Text="VOID"></asp:Label>
                                            <asp:LinkButton ValidationGroup="search" ID="lblVoidInv" Width="20px" runat="server"
                                                Text="VOID" CommandName="Void" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                            <asp:LinkButton Style="display: none" ID="voidlink" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'
                                                Width="0px" runat="server" OnClick="btnvoidlink_click"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ValidationGroup="search" ID="lbnDetails" Width="20px" runat="server"
                                                Text="DETAILS" CommandName="Details" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr1" runat="server" class="AlternatingItemTemplate">
                                        <td>
                                            <asp:Label ID="lblActNum" Width="20px" runat="server" Text='<%# Bind("CustomerID")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblActName" Width="100px" runat="server" Text='<%# Bind("ACC_NAME")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblValnDate" Width="50px" runat="server" Text='<%# Eval("ValuationDate", "{0:d}")%>'></asp:Label>
                                        </td>
                                        <%--  <td>
                                            <asp:Label ID="lblAdjNumber" Width="20px" runat="server" Text='<%# Bind("ThirdPartyAdminManualInvoiceID")%>'></asp:Label>
                                        </td>--%>
                                        <td align="left">
                                            <asp:Label ID="lblInvoiceNumber" Width="50px" runat="server" Text='<%# Bind("InvoiceNumber")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTpaName" Width="70px" runat="server" Text='<%# Bind("TPA_NAME")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvType" Width="50px" runat="server" Text='<%# Bind("INVOICE_TYPE_TEXT")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBuOffice" Width="50px" runat="server" Text='<%# Bind("BU_OFFICE_TEXT")%>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvDate" Width="50px" runat="server" Text='<%# Eval("InvoiceDate", "{0:d}")%>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblhdFinalizeInd" runat="server" Text='<%#Bind("FinalizedIndicator") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblhdCanInd" runat="server" Text='<%#Bind("CancelIndicator") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblCancel" runat="server" Text="CANCEL"></asp:Label>
                                            <asp:LinkButton ID="lbnCancelInv" Width="20px" runat="server" Text="CANCEL" CommandName="CancelInv"
                                                CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblhdReviseInd" runat="server" Text='<%#Bind("ReviseIndicator") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblRevise" runat="server" Text="REVISE"></asp:Label>
                                            <asp:LinkButton ValidationGroup="search" ID="lbnReviseInv" Width="20px" runat="server"
                                                Text="REVISE" CommandName="Revise" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                            <asp:LinkButton Style="display: none" ID="ReviseLink" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'
                                                Width="0px" runat="server" OnClick="btnReviselink_click"></asp:LinkButton>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblhdVoidInd" runat="server" Text='<%#Bind("VoidIndicator") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblVoid" runat="server" Text="VOID"></asp:Label>
                                            <asp:LinkButton ValidationGroup="search" ID="lblVoidInv" Width="20px" runat="server"
                                                Text="VOID" CommandName="Void" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="voidlink" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'
                                                Style="display: none" runat="server" OnClick="btnvoidlink_click"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ValidationGroup="search" ID="lbnDetails" Width="20px" runat="server"
                                                Text="DETAILS" CommandName="Details" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceID") %>'></asp:LinkButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:AISListView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnVoid" Width="0px" runat="server"></asp:Button>
            <asp:Button ID="btnRevise" Width="0px" runat="server"></asp:Button>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="programmaticModalPopup" TargetControlID="btnVoid"
                PopupControlID="programmaticPopup" BackgroundCssClass="modalBackground" DropShadow="true"
                PopupDragHandleControlID="programmaticPopupDragHandle" RepositionMode="RepositionOnWindowScroll"
                CancelControlID="btnCancel">
            </ajaxToolkit:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="programmaticPopup" Style="border: solid 1px black;
                    display: none; width: 350px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="width: 100%; cursor: move;
                        padding: 0px; background-color: #CCCCCC; height: 20px; border: solid 1px Gray;
                        color: Black; text-align: center; vertical-align: middle">
                        <b style="vertical-align: middle; font-size: 12px">Please Enter Void Comments here</b></asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        <asp:TextBox TextMode="MultiLine" Rows="6" BorderStyle="Solid" BorderColor="Black"
                            BorderWidth="1px" Width="95%" runat="server" ID="txtComments"></asp:TextBox>
                        <br />
                        <asp:Button OnClientClick="testVoid()" ID="btnOk" runat="server" Text="Save" OnClick="btnVoid_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="programmaticModalPopupRevise"
                TargetControlID="btnRevise" PopupControlID="programmaticPopupRevise" BackgroundCssClass="modalBackground"
                DropShadow="true" PopupDragHandleControlID="Panel4" RepositionMode="RepositionOnWindowScroll"
                CancelControlID="btnCancelRevisepopup">
            </ajaxToolkit:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="programmaticPopupRevise" Style="border: solid 1px black;
                    display: none; width: 350px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel4" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                        <b style="vertical-align: middle; font-size: 12px">Please Enter Revision Comments here</b></asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        <asp:TextBox TextMode="MultiLine" Rows="6" BorderStyle="Solid" BorderColor="Black"
                            BorderWidth="1px" Width="95%" runat="server" ID="txtReviseComments"></asp:TextBox>
                        <br />
                        <asp:Button OnClientClick="testRevise()" ID="btnOKRevise" runat="server" Text="Save"
                            OnClick="btnRevise_Click" />
                        <asp:Button ID="btnCancelRevisepopup" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
            <asp:Button Width="0px" runat="server" ID="AriesTemp" />
            <ajaxToolkit:ModalPopupExtender runat="server" ID="modalAries" TargetControlID="AriesTemp"
                PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnClosepopup">
            </ajaxToolkit:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="Panel1" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel2" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        Void has occurred. It will copy the original invoice entries that are being voided
                        and reverse the sign and pass the record to the ARiES transmittal
                        <br />
                        <asp:Button Width="60px" ID="btnClosepopup" runat="server" Text="ok" OnClick="btnVoid_Click" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnOk" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
