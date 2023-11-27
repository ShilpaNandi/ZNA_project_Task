<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctSetup_ReconReview"
    Title="Untitled Page" CodeBehind="ReconReview.aspx.cs" %>
<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="lblReconreview" runat="server" Text="Reconciliation Review" CssClass="h1"></asp:Label>
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
    function doPaste(control, max) {
        maxLength = parseInt(max);
        value = control.value;
        if (maxLength) {
            event.returnValue = false;
            maxLength = parseInt(maxLength);
            var oTR = control.document.selection.createRange();
            var iInsertLength = maxLength - value.length + oTR.text.length;
            var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
            oTR.text = sData;
        }
    }

</script>
    <AI:AccountInfoHeader ID="ucAccountInfoHeader1" runat="server" />
    <br />
    <asp:ValidationSummary ID="ValSumSave" ValidationGroup="Save" runat="server"></asp:ValidationSummary>
    <asp:ValidationSummary ID="valSumUpdate" ValidationGroup="Update" runat="server">
    </asp:ValidationSummary>
    <asp:ValidationSummary ID="valSumSaveDetail" ValidationGroup="SaveDetail" runat="server">
    </asp:ValidationSummary>
    <asp:ValidationSummary ID="valSummarysearch" CssClass="ValidationSummary" ValidationGroup="search"
        runat="server"></asp:ValidationSummary>
    <asp:DropDownList ID="ddlQCDate" ValidationGroup="search" runat="server" Visible="false">
    </asp:DropDownList>
    <asp:CompareValidator ID="compqcdate" runat="server" ControlToValidate="ddlQCDate"
        ValidationGroup="search" ValueToCompare="0" Text="*" ErrorMessage="Please select date"
        Operator="NotEqual"></asp:CompareValidator>
    <asp:Button ID="btnDisplay" ValidationGroup="search" runat="server" Visible="false"
        Text="Search" OnClick="btnDisplay_click" />
    <br />
    <br />
    <asp:Panel ID="pnlReconReview" DefaultButton="btnSave" runat="server" BorderWidth="1px" ScrollBars="Auto"
        Width="910px">
        <table width="100%">
            <tr>
                <td align="right" style="width: 120px; padding-right: 10px">
                    Review Date:
                </td>
                <td>
                    <asp:TextBox ID="txtReviewDate" runat="server" ValidationGroup="Save"></asp:TextBox>
                    <AjaxToolKit:MaskedEditExtender ID="mskReviewDate" runat="server" TargetControlID="txtReviewDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                        ErrorTooltipEnabled="True" />
                    <asp:ImageButton ID="imgValidTo" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        CausesValidation="False" />
                    <asp:RequiredFieldValidator ID="reqDate" runat="server" ErrorMessage="Please enter Review Date"
                        ValidationGroup="Save" ControlToValidate="txtReviewDate" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regReviewdate" runat="server" ControlToValidate="txtReviewDate"
                        ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                        ErrorMessage="Invalid Review Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <AjaxToolKit:CalendarExtender ID="calReviewDate" runat="server" PopupPosition="TopRight"
                        TargetControlID="txtReviewDate" PopupButtonID="imgValidTo" />
                </td>
                <td rowspan="2" style="vertical-align: top">
                    Comments:
                </td>
                <td rowspan="2" valign="top">
                    <asp:TextBox ID="txtComments" Width="350px" runat="server" TextMode="MultiLine" Rows="3"
                     onpaste="doPaste(this,500);" MaxLength="500" onkeypress="doKeypress(this,500);"
                                            onbeforepaste="doBeforePaste(this,500);">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 10px">
                    Review By:
                </td>
                <td>
                    <asp:Label ID="lblReviewBy" runat="server"></asp:Label>
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
    <asp:Panel ID="pnlDetails" runat="server" Height="175px" Visible="false" ScrollBars="Auto"
        Width="500px">
        <asp:ObjectDataSource ID="lookupDataSource" runat="server" SelectMethod="GetLookUpActiveData"
            TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
            <SelectParameters>
                <asp:Parameter DefaultValue="RECON ISSUE REVIEW" Name="lookUpTypeName" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Label ID="lblAdjQCDetails" runat="server" Text="Reconciliation Review Details"
            CssClass="h3"></asp:Label>
        <asp:AISListView ID="lstReviewDetails" runat="server" InsertItemPosition="FirstItem"
            DataKeyNames="LookUpID" OnItemCommand="CommandList" OnItemDataBound="DataBoundList">
            <LayoutTemplate>
                <table id="Table1" class="panelContents" runat="server" width="98%">
                    <tr class="LayoutTemplate">
                        <th>
                        </th>
                        <th>
                            Recon Issues
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
                    <td align="left">
                        <asp:HiddenField ID="HidQltyChkID" runat="server" />
                        <%# Eval("CHKLISTNAME")%>
                    </td>
                    <td>
                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                            runat="server" Enabled='<%# Eval("ENABLED")%>' ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                        </asp:ImageButton>
                    </td>
                    <tr>
                    </tr>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="AlternatingItemTemplate">
                    <td align="left">
                    </td>
                    <td align="left">
                        <asp:HiddenField ID="HidQltyChkID" runat="server" />
                        <%# Eval("CHKLISTNAME")%>
                    </td>
                    <td>
                        <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                        <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                            runat="server" Enabled='<%# Eval("ENABLED")%>' ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                        </asp:ImageButton>
                    </td>
                    <tr>
                    </tr>
                </tr>
            </AlternatingItemTemplate>
            <InsertItemTemplate>
                <tr class="ItemTemplate">
                    <td align="left">
                        <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="SaveDetail" CommandName="Save"
                            Text="SAVE"></asp:LinkButton>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlIssue" runat="server" ValidationGroup="SaveDetail">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="compIssue" runat="server" ControlToValidate="ddlIssue"
                            ValidationGroup="SaveDetail" ValueToCompare="0" Text="*" ErrorMessage="Please select an Item"
                            Operator="NotEqual"></asp:CompareValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                </tr>
            </InsertItemTemplate>
        </asp:AISListView>
        <div id="divBtnSave" style="text-align: center; width: 500px" runat="server">
            <asp:Button ID="btnApprove" runat="server" Text="APPROVE" OnClick="btnApprove_Click" />
            <asp:Button ID="btnReject" runat="server" Text="REJECT" OnClick="btnReject_Click" />
        </div>
    </asp:Panel>
</asp:Content>
