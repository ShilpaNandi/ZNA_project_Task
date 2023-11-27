<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="TPAorManual_TPAManualPosting"
    CodeBehind="TPAManualPosting.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="GAILabel" runat="server" Text="TPA Manual Posting" CssClass="h1"></asp:Label>
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
        function finalizePopup() {
            $get('<%=btnFinalizeClose.ClientID%>').click();
            $get('<%=btnFinalizepopup.ClientID%>').click();
        }
        function TestAmount(control) {
            var txt = control.value;
            txt = txt.replace(/_/g, '');
            txt = txt.replace(/,/g, '');

            count = txt.split('.');
            if (txt.indexOf('.') == txt.length - 1) {
                txt += "00";

            }
            control.value = txt;
            alert(txt);
        }
        var scrollTop1;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=pnlTPADtl.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=pnlTPADtl.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;
        }

        function EnableDisable(reqsym, reqnbr, reqmod, HidInd) {

            var ind = $get(HidInd).value;
            if (ind == "True") {
                ValidatorEnable($get(reqsym), true);
                ValidatorEnable($get(reqnbr), true);
                ValidatorEnable($get(reqmod), true);

            }
            else {
                ValidatorEnable($get(reqsym), false);
                ValidatorEnable($get(reqnbr), false);
                ValidatorEnable($get(reqmod), false);
            }
        }     
    
    </script>

    <AI:AccountInfoHeader ID="ucAccountInfoHeader1" runat="server" />
    <asp:ValidationSummary ID="ValSumTPASave" ValidationGroup="Save" runat="server">
    </asp:ValidationSummary>
    <asp:ValidationSummary ID="valSumTPAdtlUpdate" ValidationGroup="updateDetails" runat="server">
    </asp:ValidationSummary>
    <asp:ValidationSummary ID="valSumTPAdtlSave" ValidationGroup="saveDetails" runat="server">
    </asp:ValidationSummary>
    <asp:UpdatePanel ID="upnlTPApostings" runat="server">
        <ContentTemplate>
            <br />
            <asp:Panel BorderWidth="1px" runat="server" Width="910px" DefaultButton="btnDefault">
                <table width="910px">
                    <tr>
                        <td style="width: 100px" nowrap>
                            Invoice Number
                        </td>
                        <td style="width: 80px">
                            <asp:TextBox ReadOnly="true" runat="server" ID="txtInvoiceNbr" Width="150px" MaxLength="20"
                                ContentEditable="False"></asp:TextBox>
                        </td>
                        <td style="width: 80px;" nowrap>
                            Invoice Type
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="ObjInvoiceType" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="INVOICE TYPE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlInvoiceType" runat="server" DataSourceID="ObjInvoiceType"
                                DataTextField="LookUpName" ValidationGroup="Save" DataValueField="LookUpID" Width="156px">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="compInvoiceType" runat="server" ControlToValidate="ddlInvoiceType"
                                ValidationGroup="Save" ValueToCompare="0" Text="*" ErrorMessage="Please select Invoice Type"
                                Operator="NotEqual"></asp:CompareValidator>
                        </td>
                        <td style="width: 100px" nowrap>
                            Val Date
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtValuationDate" ValidationGroup="Save" Width="125px"></asp:TextBox>
                            <AjaxToolKit:MaskedEditExtender ID="mskValDate" runat="server" TargetControlID="txtValuationDate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <AjaxToolKit:CalendarExtender ID="calValDate" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtValuationDate" PopupButtonID="imgValDate" />
                            <asp:ImageButton ID="imgValDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regValuationDate" runat="server" ControlToValidate="txtValuationDate"
                                Display="Dynamic" ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                ErrorMessage="Invalid Valuation Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="reqValDate" runat="server" ErrorMessage="Please enter Valuation Date"
                                ValidationGroup="Save" ControlToValidate="txtValuationDate" Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Invoice Date
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtInvoiceDate" ValidationGroup="Save" Width="125px"></asp:TextBox>
                            <AjaxToolKit:MaskedEditExtender ID="mskInvoiceDate" runat="server" TargetControlID="txtInvoiceDate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <AjaxToolKit:CalendarExtender ID="calInvoiceDate" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtInvoiceDate" PopupButtonID="imgInvoiceDate" />
                            <asp:ImageButton ID="imgInvoiceDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regInvoiceDate" runat="server" ControlToValidate="txtInvoiceDate"
                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                ErrorMessage="Invalid Invoice Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="reqInvoiceDate" runat="server" ErrorMessage="Please enter Invoice Date"
                                ValidationGroup="Save" ControlToValidate="txtInvoiceDate" Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Due Date
                        </td>
                        <td>
                            <asp:TextBox ValidationGroup="Save" runat="server" ID="txtDueDate" Width="125px"></asp:TextBox>
                            <AjaxToolKit:MaskedEditExtender ID="mskDueDate" runat="server" TargetControlID="txtDueDate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <AjaxToolKit:CalendarExtender ID="calDuedate" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtDueDate" PopupButtonID="imgDueDate" />
                            <asp:ImageButton ID="imgDueDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regDueDate" runat="server" ControlToValidate="txtDueDate"
                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                ErrorMessage="Invalid Due Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            End Date
                        </td>
                        <td>
                            <asp:TextBox ValidationGroup="Save" runat="server" ID="txtEndDate" Width="125px"></asp:TextBox>
                            <AjaxToolKit:MaskedEditExtender ID="mskEndDate" runat="server" TargetControlID="txtEndDate"
                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                ErrorTooltipEnabled="True" />
                            <AjaxToolKit:CalendarExtender ID="calEndDate" runat="server" PopupPosition="TopRight"
                                TargetControlID="txtEndDate" PopupButtonID="imgEndDate" />
                            <asp:ImageButton ID="imgEndDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                CausesValidation="False" />
                            <asp:RegularExpressionValidator ID="regEndDate" runat="server" ControlToValidate="txtEndDate"
                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                ErrorMessage="Invalid End Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Source of Loss
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objLossSource" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="LOSS SOURCE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlSourceofLoss" runat="server" DataSourceID="objLossSource"
                                DataTextField="LookUpName" DataValueField="LookUpID" Width="156px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            BU/Office
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="BUOfficeDataSource" runat="server" SelectMethod="GetBUOffForLookups"
                                TypeName="ZurichNA.AIS.Business.Logic.BusinessUnitOfficeBS"></asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlBuOffice" runat="server" DataSourceID="BUOfficeDataSource"
                                DataTextField="LookupName" DataValueField="LookUpID" Width="156px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Billing Cycle
                        </td>
                        <td>
                            <asp:ObjectDataSource ID="objBilling" runat="server" SelectMethod="GetLookUpActiveData"
                                TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="TPA BILLING CYCLE" Name="lookUpTypeName" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DropDownList ID="ddlBillingCycle" runat="server" DataSourceID="objBilling" DataTextField="LookUpName"
                                ValidationGroup="Save" DataValueField="LookUpID" Width="156px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            TPA Name
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTPAName" runat="server" Width="156px" ValidationGroup="ExternalSaveGroup">
                                <asp:ListItem Value="0">(Select)</asp:ListItem>
                            </asp:DropDownList>
                            <%--<asp:CompareValidator ID="compddlTPAName" runat="server" ControlToValidate="ddlTPAName"
                                ValidationGroup="Save" ValueToCompare="0" Text="*" ErrorMessage="Please select TPA Name"
                                Operator="NotEqual"></asp:CompareValidator>--%>
                        </td>
                        <td>
                            Invoice Amount
                        </td>
                        <td>
                            <%-- <asp:TextBox ID="txtAmount" runat="server" MaxLength="14" Width="150px" onblur="FormatNumWithDecAmt(this,11)"></asp:TextBox>
                            <AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtAmount" FilterType="Custom"
                                ValidChars="-0123456789,." ID="fltrAmount">
                            </AjaxToolKit:FilteredTextBoxExtender>--%>
                            <asp:AISAmountTextbox ID="txtAmount" runat="server" Width="150px" AllowDecimal="true" AllowNegetive="true">
                            </asp:AISAmountTextbox>
                        </td>
                        <td>
                            Policy Years
                        </td>
                        <td>
                            <asp:TextBox Width="150px" runat="server" ID="txtPolicyYears" MaxLength="3"></asp:TextBox>
                            <AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtPolicyYears"
                                FilterType="Numbers" ID="fltrPlicyYears">
                            </AjaxToolKit:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Comments
                        </td>
                        <td colspan="5">
                            <asp:TextBox runat="server" MaxLength="4000" onpaste="doPaste(this,4000);" onkeypress="doKeypress(this,4000);"
                                onbeforepaste="doBeforePaste(this,4000);" ID="txtComments" TextMode="MultiLine"
                                Rows="3" Width="460px"></asp:TextBox>
                            <asp:Button ID="btnDefault" runat="server" Height="0px" Width="0px" />
                            <asp:TextBox ID="txtTPAFund" runat="server" Text="No" Style="display: none"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Label ID="lblDetails" runat="server" Visible="false" CssClass="h2" Text="TPA/Manual Posting details"></asp:Label>
            <br />
            <asp:Panel ID="pnlTPADtl" runat="server" Height="190px" ScrollBars="Auto" Width="910px">
                <asp:AISListView ID="lstTPADtl" runat="server" InsertItemPosition="FirstItem" DataKeyNames="ThirdPartyAdminManualInvoiceDtlID"
                    OnItemCommand="CommandList" OnItemDataBound="DataBoundList" OnItemEditing="EditList"
                    OnItemUpdating="UpdateList" OnItemCanceling="CancelList" OnSorting="lstTPADtl_Sorting">
                    <LayoutTemplate>
                        <table id="Table1" class="panelContents" runat="server" width="98%">
                            <tr class="LayoutTemplate">
                                <th style="width: 40px">
                                </th>
                                <th style="width: 210px">
                                    <asp:LinkButton ID="lnkTransaction" runat="server" CommandName="Sort" CommandArgument="PostTransactionText">Transaction</asp:LinkButton>
                                    <asp:Image ID="imgTransaction" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                        Visible="false" />
                                </th>
                                <th>
                                    Main
                                </th>
                                <th>
                                    Sub
                                </th>
                                <th>
                                    Company
                                </th>
                                <th style="width: 165px">
                                    <asp:LinkButton ID="lnkPolicy" runat="server" CommandName="Sort" CommandArgument="PolicyNbrText">Policy #</asp:LinkButton>
                                    <asp:Image ID="imgPolicy" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                        Visible="false" />
                                </th>
                                <th style="width: 110px">
                                    <asp:LinkButton ID="lnkEffctDate" runat="server" CommandName="Sort" CommandArgument="EffectiveDate">Effective Date</asp:LinkButton>
                                    <asp:Image ID="imgEffctDate" runat="server" ImageUrl="~/images/Ascending.gif" ToolTip="Ascending"
                                        Visible="false" />
                                </th>
                                <th>
                                    Expiration Date
                                </th>
                                <th>
                                    Amount
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="ItemTemplate" id="trItemTemplate" runat="server">
                            <td align="left">
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit"></asp:LinkButton>
                            </td>
                            <td align="left">
                                <%# Eval("PostTransactionText")%>
                            </td>
                            <td align="left">
                                <%# Eval("AriesMainNbr", "{0:0}")%>
                            </td>
                            <td align="left">
                                <%# Eval("AriesSubNbr", "{0:0}")%>
                            </td>
                            <td align="left">
                                <%# Eval("CompanyCode")%>
                            </td>
                            <td align="left">
                                <%# Eval("PolicySymbolText")+" "+Eval("PolicyNbrText")+" "+Eval("PolicyModText")%>
                            </td>
                            <td align="left">
                                <%# Eval("EffectiveDate", "{0:d}")%>
                            </td>
                            <td align="left">
                                <%# Eval("ExpiryDate", "{0:d}")%>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblAmount" runat="server" Text='<%# (Eval("ThirdPartyAdminAmt") != null && (Eval("ThirdPartyAdminAmt").ToString() != "")) ?(decimal.Parse(Eval("ThirdPartyAdminAmt").ToString())).ToString("#,##.00") : ""%>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="AlternatingItemTemplate" id="trItemTemplate" runat="server">
                            <td align="left">
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit"></asp:LinkButton>
                            </td>
                            <td align="left">
                                <%# Eval("PostTransactionText")%>
                            </td>
                            <td align="left">
                                <%# Eval("AriesMainNbr", "{0:0}")%>
                            </td>
                            <td align="left">
                                <%# Eval("AriesSubNbr", "{0:0}")%>
                            </td>
                            <td align="left">
                                <%# Eval("CompanyCode")%>
                            </td>
                            <td align="left">
                                <%# Eval("PolicySymbolText")+" "+Eval("PolicyNbrText")+" "+Eval("PolicyModText")%>
                            </td>
                            <td align="left">
                                <%# Eval("EffectiveDate", "{0:d}")%>
                            </td>
                            <td align="left">
                                <%# Eval("ExpiryDate", "{0:d}")%>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblAmount" runat="server" Text='<%# (Eval("ThirdPartyAdminAmt") != null && (Eval("ThirdPartyAdminAmt").ToString() != "")) ?(decimal.Parse(Eval("ThirdPartyAdminAmt").ToString())).ToString("#,##.00") : ""%>'></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EditItemTemplate>
                        <tr class="ItemTemplate">
                            <td align="left">
                                <asp:LinkButton ID="lnkUpdate" ValidationGroup="updateDetails" CommandArgument='<%# Bind("ThirdPartyAdminManualInvoiceDtlID") %>'
                                    CommandName="Update" runat="server" Text="UPDATE"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="CANCEL"></asp:LinkButton>
                            </td>
                            <td align="left" width="195px">
                                <asp:HiddenField ID="hidTransID" runat="server" Value='<%# Bind("PostingTrnsTypID")%>' />
                                <asp:HiddenField ID="hidTransText" runat="server" Value='<%# Bind("PostTransactionText")%>' />
                                <asp:DropDownList AutoPostBack="true" ID="ddlTransaction" Width="180px" runat="server"
                                    OnSelectedIndexChanged="ddlEditTransaction">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="ComphidTransID" runat="server" ControlToValidate="ddlTransaction"
                                    ValidationGroup="updateDetails" ValueToCompare="0" Text="*" ErrorMessage="Please select Transaction"
                                    Operator="NotEqual"></asp:CompareValidator>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblMain" Text='<%# Bind("AriesMainNbr","{0:0}")%>' runat="server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblSub" Text='<%# Bind("AriesSubNbr","{0:0}")%>' runat="server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCompany" Text='<%# Bind("CompanyCode")%>' runat="server"></asp:Label>
                                <asp:HiddenField ID="hidIndicator" runat="server" />
                            </td>
                            <td align="left" nowrap>
                                <asp:TextBox ID="txtpolSymbol" Width="28px" MaxLength="3" Text='<%# Bind("PolicySymbolText")%>'
                                    runat="server" ValidationGroup="updateDetails" onkeypress="return toUpperCase(event,this)" />
                                <AjaxToolKit:FilteredTextBoxExtender TargetControlID="txtpolSymbol" FilterType="LowercaseLetters,UppercaseLetters"
                                    ID="ftePolicySymbol" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredPolicySymbol" runat="server" ControlToValidate="txtpolSymbol"
                                    Enabled="false" ErrorMessage="Please Enter Policy Symbol" Text="*" Display="Dynamic"
                                    ValidationGroup="updateDetails" />
                                <asp:RegularExpressionValidator ID="regPolicySymbol" runat="server" ControlToValidate="txtpolSymbol"
                                    Text="*" Display="Dynamic" ValidationGroup="updateDetails" ErrorMessage="Please enter minimum two alpha characters for Policy Symbol"
                                    ValidationExpression=".{2}.*" />
                                <asp:TextBox ID="txtpolNumber" Width="60px" Text='<%# Bind("PolicyNbrText")%>' MaxLength="8"
                                    runat="server" ValidationGroup="updateDetails" />
                                <asp:RequiredFieldValidator ID="requiredPolicyNumber" runat="server" ControlToValidate="txtpolNumber"
                                    Enabled="false" ErrorMessage="Please Enter Policy Number" Text="*" Display="Dynamic"
                                    ValidationGroup="updateDetails" />
                                <asp:RegularExpressionValidator ID="regulartxtpolNumber" runat="server" ControlToValidate="txtpolNumber"
                                    Text="*" Display="Dynamic" ValidationGroup="updateDetails" ErrorMessage="Please enter minimum seven characters for Policy Number"
                                    ValidationExpression=".{7}.*" />
                                <AjaxToolKit:FilteredTextBoxExtender TargetControlID="txtpolNumber" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                    ID="ftePolicyNumber" runat="server" />
                                <asp:TextBox ID="txtpolmodules" Width="15px" Text='<%# Bind("PolicyModText")%>' MaxLength="2"
                                    runat="server" ValidationGroup="updateDetails" />
                                <asp:RequiredFieldValidator ID="requiredPolicyModulus" runat="server" ControlToValidate="txtpolmodules"
                                    Enabled="false" ErrorMessage="Please Enter Policy Module" Text="*" Display="Dynamic"
                                    ValidationGroup="updateDetails" />
                                <AjaxToolKit:FilteredTextBoxExtender TargetControlID="txtpolmodules" FilterType="Numbers"
                                    ID="ftePolicyModulus" runat="server" />
                                <asp:RegularExpressionValidator ID="RegularPolicyMod" runat="server" ControlToValidate="txtpolmodules"
                                    Text="*" Display="Dynamic" ValidationGroup="updateDetails" ErrorMessage="Please enter minimum two numeric digits for Policy Module"
                                    ValidationExpression=".{2}.*" />
                            </td>
                            <td align="left" nowrap>
                                <asp:TextBox Text='<%# Bind("EffectiveDate","{0:d}")%>' ValidationGroup="updateDetails"
                                    ID="txtEffectiveDate" runat="server" Width="70px"></asp:TextBox>
                                <AjaxToolKit:CalendarExtender ID="caleffDate" runat="server" PopupPosition="TopRight"
                                    TargetControlID="txtEffectiveDate" PopupButtonID="imgEffDate" />
                                <asp:ImageButton ID="imgEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <AjaxToolKit:MaskedEditExtender ID="mskEffectiveDate" runat="server" TargetControlID="txtEffectiveDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <asp:RegularExpressionValidator ID="regEffectiveDate" runat="server" ControlToValidate="txtEffectiveDate"
                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                    ErrorMessage="Invalid Effective Date" Text="*" ValidationGroup="updateDetails"></asp:RegularExpressionValidator>
                            </td>
                            <td align="left">
                                <asp:TextBox Text='<%# Bind("ExpiryDate","{0:d}")%>' ValidationGroup="updateDetails"
                                    ID="txtExpiryDate" runat="server" Width="70px"></asp:TextBox>
                                <AjaxToolKit:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="TopRight"
                                    TargetControlID="txtExpiryDate" PopupButtonID="imgExpDate" />
                                <asp:ImageButton ID="imgExpDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <AjaxToolKit:MaskedEditExtender ID="mskExpiryDate" runat="server" TargetControlID="txtExpiryDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <asp:CompareValidator ID="compExpDate" ValidationGroup="updateDetails" runat="server"
                                    ControlToCompare="txtEffectiveDate" ControlToValidate="txtExpiryDate" Operator="GreaterThan"
                                    Type="Date" ErrorMessage="Expiry date should be greater than Effective date">*</asp:CompareValidator>
                                <asp:RegularExpressionValidator ID="regExpiryDate" runat="server" ControlToValidate="txtExpiryDate"
                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                    ErrorMessage="Invalid Expiry Date" Text="*" ValidationGroup="updateDetails"></asp:RegularExpressionValidator>
                            </td>
                            <td align="left">
                                <asp:AISAmountTextbox AllowDecimal="true" AllowNegetive="true" Text='<%# Bind("ThirdPartyAdminAmt")%>' ID="txtAmount" runat="server"
                                    Width="99px">
                                </asp:AISAmountTextbox>
                                <%--<AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtAmount" FilterType="Custom"
                                    ValidChars="-0123456789.," ID="FltrAmount">
                                </AjaxToolKit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <tr class="ItemTemplate" align="left">
                            <td align="left">
                                <asp:LinkButton ValidationGroup="saveDetails" ID="lnkSave" CommandName="Save" runat="server"
                                    Text="Save"></asp:LinkButton>
                            </td>
                            <td align="left" width="195px">
                                <asp:DropDownList ID="ddlTransaction" runat="server" AutoPostBack="true" Width="180px"
                                    OnSelectedIndexChanged="ddlInsertTransaction">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="ComphidTransID" runat="server" ControlToValidate="ddlTransaction"
                                    ValidationGroup="saveDetails" ValueToCompare="0" Text="*" ErrorMessage="Please select Transaction"
                                    Operator="NotEqual"></asp:CompareValidator>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblMain" runat="server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblSub" runat="server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblCompany" runat="server"></asp:Label>
                                <asp:HiddenField ID="hidIndicator" runat="server" />
                            </td>
                            <td align="left" nowrap>
                                <asp:TextBox ID="txtpolSymbol" Width="28px" onkeypress="return toUpperCase(event,this)"
                                    MaxLength="3" runat="server" ValidationGroup="saveDetails" />
                                <AjaxToolKit:FilteredTextBoxExtender TargetControlID="txtpolSymbol" FilterType="LowercaseLetters,UppercaseLetters"
                                    ID="ftePolicySymbol" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredPolicySymbol" runat="server" ControlToValidate="txtpolSymbol"
                                    Enabled="false" ErrorMessage="Please Enter Policy Symbol" Text="*" Display="Dynamic"
                                    ValidationGroup="saveDetails" />
                                <asp:RegularExpressionValidator ID="regPolicySymbol" runat="server" ControlToValidate="txtpolSymbol"
                                    Text="*" Display="Dynamic" ValidationGroup="saveDetails" ErrorMessage="Please enter minimum two alpha characters for Policy Symbol"
                                    ValidationExpression=".{2}.*" />
                                <asp:TextBox ID="txtpolNumber" Width="60px" MaxLength="8" runat="server" />
                                <asp:RequiredFieldValidator ID="requiredPolicyNumber" runat="server" ControlToValidate="txtpolNumber"
                                    Enabled="false" ErrorMessage="Please Enter Policy Number" Text="*" Display="Dynamic"
                                    ValidationGroup="saveDetails" />
                                <asp:RegularExpressionValidator ID="regulartxtpolNumber" runat="server" ControlToValidate="txtpolNumber"
                                    Text="*" Display="Dynamic" ValidationGroup="saveDetails" ErrorMessage="Please enter minimum seven characters for Policy Number"
                                    ValidationExpression=".{7}.*" />
                                <AjaxToolKit:FilteredTextBoxExtender TargetControlID="txtpolNumber" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                    ID="ftePolicyNumber" runat="server" />
                                <asp:TextBox ID="txtpolmodules" Width="15px" MaxLength="2" runat="server" />
                                <asp:RequiredFieldValidator ID="requiredPolicyModulus" runat="server" ControlToValidate="txtpolmodules"
                                    Enabled="false" ErrorMessage="Please Enter Policy Module" Text="*" Display="Dynamic"
                                    ValidationGroup="saveDetails" />
                                <AjaxToolKit:FilteredTextBoxExtender TargetControlID="txtpolmodules" FilterType="Numbers"
                                    ID="ftePolicyModulus" runat="server" />
                                <asp:RegularExpressionValidator ID="RegularPolicyMod" runat="server" ControlToValidate="txtpolmodules"
                                    Text="*" Display="Dynamic" ValidationGroup="saveDetails" ErrorMessage="Please enter minimum two numeric digits for Policy Module"
                                    ValidationExpression=".{2}.*" />
                            </td>
                            <td align="left" nowrap>
                                <asp:TextBox ValidationGroup="saveDetails" ID="txtEffectiveDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="imgEffDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <AjaxToolKit:MaskedEditExtender ID="mskEffectiveDate" runat="server" TargetControlID="txtEffectiveDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <AjaxToolKit:CalendarExtender ID="calEffectiveDate" runat="server" PopupPosition="TopLeft"
                                    TargetControlID="txtEffectiveDate" PopupButtonID="imgEffDate" />
                                <asp:RegularExpressionValidator ID="regEffectiveDate" runat="server" ControlToValidate="txtEffectiveDate"
                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                    ErrorMessage="Invalid Effective Date" Text="*" ValidationGroup="saveDetails"></asp:RegularExpressionValidator>
                            </td>
                            <td align="left" nowrap>
                                <asp:TextBox ValidationGroup="saveDetails" ID="txtExpiryDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="imgExpDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                    CausesValidation="False" />
                                <AjaxToolKit:MaskedEditExtender ID="mskExpiryDate" runat="server" TargetControlID="txtExpiryDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <asp:CompareValidator ID="compEffDate" ValidationGroup="saveDetails" runat="server"
                                    ControlToValidate="txtEffectiveDate" ControlToCompare="txtExpiryDate" Operator="LessThan"
                                    Type="Date" ErrorMessage="Effective date should be less than Expiry date">*</asp:CompareValidator>
                                <AjaxToolKit:CalendarExtender ID="calExpiryDate" runat="server" PopupPosition="TopLeft"
                                    TargetControlID="txtExpiryDate" PopupButtonID="imgExpDate" />
                                <asp:RegularExpressionValidator ID="regValidationGroup" runat="server" ControlToValidate="txtExpiryDate"
                                    ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                    ErrorMessage="Invalid Expiry Date" Text="*" ValidationGroup="saveDetails"></asp:RegularExpressionValidator>
                            </td>
                            <td align="left">
                                <asp:AISAmountTextbox AllowDecimal="true" AllowNegetive="true" Text='<%# Bind("ThirdPartyAdminAmt")%>' ID="txtAmount" runat="server"
                                    Width="99px" ></asp:AISAmountTextbox>
                                <%--   <AjaxToolKit:FilteredTextBoxExtender runat="server" TargetControlID="txtAmount" FilterType="Custom"
                                    ValidChars="-0123456789.," ID="FltrAmount">
                                </AjaxToolKit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                    </InsertItemTemplate>
                </asp:AISListView>
            </asp:Panel>
            <div style="text-align: right">
                <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnFinalize" ValidationGroup="Save" runat="server" Text="Finalize"
                    OnClick="btnFinalize_Click" />
            </div>
            <asp:Button Width="0px" runat="server" ID="AriesTemp" />
            <AjaxToolKit:ModalPopupExtender runat="server" ID="modalSave" TargetControlID="AriesTemp"
                PopupControlID="pnlSavePopup" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnClosepopup">
            </AjaxToolKit:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlSavePopup" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel1" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        Invoice details will not post to Aries until Finalize invoice is selected.
                        <br />
                        <br />
                        <asp:Button Width="60px" ID="btnClosepopup" runat="server" Text="ok" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
            <asp:Button Width="0px" runat="server" ID="btnFinalTemp" />
            <AjaxToolKit:ModalPopupExtender runat="server" ID="modalFinalze" TargetControlID="btnFinalTemp"
                PopupControlID="pnlFinalizePopup" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnFinalizeClose">
            </AjaxToolKit:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlFinalizePopup" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel4" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        This will generate the posting to Aries and generate invoice number. Do you want
                        to Proceed?
                        <br />
                        <br />
                        <asp:Button Width="60px" OnClientClick="finalizePopup()" ID="btnFinalizepopup" runat="server"
                            Text="Yes" OnClick="btnFinalizePopup_Click" />
                        <asp:Button Width="60px" ID="btnFinalizeClose" runat="server" Text="No" />
                        <br />
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
