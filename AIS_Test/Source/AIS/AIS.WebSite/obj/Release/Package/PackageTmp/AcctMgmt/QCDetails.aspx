<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctSetup_QCDetails"
    Title="Untitled Page" CodeBehind="QCDetails.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblQCdetails" runat="server" Text="20% Adjustment QC " CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

<script type="text/javascript">
    function doKeypress(control, max) {
        maxLength = parseInt(max);
        value = control.value;
        if (maxLength && value.length > maxLength - 1) {
            event.returnValue = false;
            maxLength = parseInt(maxLength);
        }
    }
    // Cancel default behavior
    function doBeforePaste(control, max) {
        maxLength = parseInt(max);
        if (maxLength) {
            event.returnValue = false;
        }
    }
    // Cancel default behavior and create a new paste routine

    //function doPaste(control, max) {      Edge changes - created new function
    //    maxLength = parseInt(max);
    //    value = control.value;
    //    if (maxLength) {
    //        event.returnValue = false;
    //        maxLength = parseInt(maxLength);
    //        var oTR = control.document.selection.createRange();
    //        var iInsertLength = maxLength - value.length + oTR.text.length;
    //        var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
    //        oTR.text = sData;
    //    }
    //}

    function doPaste(control, max) {
        maxLength = parseInt(max);
        var sDataLength = event.clipboardData.getData("Text").length;
        var sData = event.clipboardData.getData("Text");
        if (sDataLength > maxLength) {
            var fData = sData.substring(0, maxLength);
            document.getElementById("<%=txtComments.ClientID%>").value = fData;
                event.returnValue = false;
            }
        }

</script>
    <AI:AccountInfoHeader ID="ucAccountInfoHeader1" runat="server" />
    <br />
    <asp:ValidationSummary ID="ValSumSave" CssClass="ValidationSummary" ValidationGroup="Save"
        runat="server"></asp:ValidationSummary>
    <asp:ValidationSummary ID="valSumUpdate" CssClass="ValidationSummary" ValidationGroup="Update"
        runat="server"></asp:ValidationSummary>
    <asp:ValidationSummary ID="valSumSaveDetail" CssClass="ValidationSummary" ValidationGroup="SaveDetail"
        runat="server"></asp:ValidationSummary>
    <asp:Panel BorderWidth="1px" ID="pnlQC" runat="server" Width="910px" DefaultButton="btnSave">
        <table width="100%">
            <tr>
                <td align="right" style="width: 120px; padding-right: 10px">
                    QC Date:
                </td>
                <td>
                    <asp:TextBox ID="txtQCDate" runat="server" ValidationGroup="Save"></asp:TextBox>
                    <AjaxToolKit:MaskedEditExtender ID="mskQCDate" runat="server" TargetControlID="txtQCDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:ImageButton ID="imgValidTo" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <asp:RegularExpressionValidator ID="regQCDate" runat="server" ControlToValidate="txtQCDate"
                        ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                        ErrorMessage="Invalid QC Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="reqDate" runat="server" ErrorMessage="Please enter QC Date"
                        ValidationGroup="Save" ControlToValidate="txtQCDate" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                    <AjaxToolKit:CalendarExtender ID="calQCDate" runat="server" PopupPosition="TopRight"
                        TargetControlID="txtQCDate" PopupButtonID="imgValidTo" />
                </td>
                <td rowspan="2" style="vertical-align: top">
                    Comments:
                </td>
                <td rowspan="2" valign="top">
                    <asp:TextBox ID="txtComments" Width="350px" runat="server" TextMode="MultiLine" Rows="3"
                     onpaste="doPaste(this,500);" MaxLength="500" onkeypress="doKeypress(this,500);"
                                            onbeforepaste="doBeforePaste(this,500);">
                    ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 10px">
                    QC By:
                </td>
                <td>
                    <asp:Label ID="lblQCby" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="Save" OnClick="btnSave_Click" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblAdjQCDetails" runat="server" Visible="false" Text="20% Adjustment QC Details"
        CssClass="h3"></asp:Label>
    <asp:Panel ID="pnlQCDetails" runat="server" Height="175px" Visible="false" ScrollBars="Auto"
        Width="800px">
        <asp:ObjectDataSource ID="odsProgPeriods" runat="server" SelectMethod="GetProgramPeriodSearchDashboard"
            TypeName="ZurichNA.AIS.Business.Logic.ProgramPeriodsBS">
            <SelectParameters>
                <asp:Parameter Name="AccNo" Type="Int32" />
                <asp:Parameter Name="PrgPrdID" DefaultValue="0" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:AISListView ID="lstQCDetails" runat="server" DataKeyNames="LookUpID" OnItemCommand="CommandList"
            OnItemDataBound="DataBoundList" InsertItemPosition="FirstItem">
            <LayoutTemplate>
                <table id="Table1" class="panelContents" runat="server" width="96%">
                    <tr class="LayoutTemplate">
                        <th>
                        </th>
                        <th>
                            Account
                        </th>
                        <th>
                            Program Period
                        </th>
                        <th>
                            QC Details
                        </th>
                        <th>
                            Disable
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="ItemTemplate">
                    <td align="left">
                    </td>
                    <td>
                        <%# Eval("Reg_AccountName")%>
                    </td>
                    <td>
                        <%# Eval("ProgramPeriodStDate", "{0:d}") + "-" + Eval("ProgramPeriodEndDate", "{0:d}")%>
                    </td>
                    <td align="left">
                        <asp:HiddenField ID="hidQltyID" runat="server" Value='<%# Eval("QualityControlChklst_ID")%>' />
                        <asp:HiddenField ID="HidQltyChkID" runat="server" />
                        <%# Eval("CHKLISTNAME")%>
                    </td>
                    <td>
                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                            runat="server" Enabled=' <%#(Eval("ENABLED")==null)?false:(Eval("ENABLED"))%>'
                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                        </asp:ImageButton>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="AlternatingItemTemplate">
                    <td align="left">
                    </td>
                    <td>
                        <%# Eval("Reg_AccountName")%>
                    </td>
                    <td>
                        <%# Eval("ProgramPeriodStDate", "{0:d}") + "-" + Eval("ProgramPeriodEndDate", "{0:d}")%>
                    </td>
                    <td align="left">
                        <asp:HiddenField ID="hidQltyID" runat="server" Value='<%# Eval("QualityControlChklst_ID")%>' />
                        <asp:HiddenField ID="HidQltyChkID" runat="server" />
                        <%# Eval("CHKLISTNAME")%>
                    </td>
                    <td>
                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                            runat="server" Enabled=' <%#(Eval("ENABLED")==null)?false:(Eval("ENABLED"))%>'
                            ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                        </asp:ImageButton>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <InsertItemTemplate>
                <tr class="ItemTemplate">
                    <td>
                        <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="SaveDetail" CommandName="Save"
                            Text="SAVE"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAccountlist" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAcctlist_OnSelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlAccountlist"
                            ValidationGroup="SaveDetail" ValueToCompare="0" Text="*" ErrorMessage="Please select Account"
                            Operator="NotEqual"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProgramPeriods" runat="server" Width="150px">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="compProgPerd" runat="server" ControlToValidate="ddlProgramPeriods"
                            ValidationGroup="SaveDetail" ValueToCompare="0" Text="*" ErrorMessage="Please select ProgramPeriod"
                            Operator="NotEqual"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:ObjectDataSource ID="lookupDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="TWENTY PERCENT QC REVIEW" Name="lookUpTypeName" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:DropDownList ID="ddlIssue" runat="server" ValidationGroup="SaveDetail">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="compIssue" runat="server" ControlToValidate="ddlIssue"
                            ValidationGroup="SaveDetail" ValueToCompare="0" Text="*" ErrorMessage="Please select a QC Issue"
                            Operator="NotEqual"></asp:CompareValidator>
                    </td>
                    <td>
                    </td>
                </tr>
            </InsertItemTemplate>
        </asp:AISListView>
    </asp:Panel>
    <div visible="false" style="text-align: right; width: 800px; padding-top: 10px" id="divQCComplete"
        runat="server">
        <asp:Button ID="btnQCComplete" runat="server" Text="QC Complete" OnClick="btnQCComplete_Click" />
        &nbsp;&nbsp;&nbsp;</div>
</asp:Content>
