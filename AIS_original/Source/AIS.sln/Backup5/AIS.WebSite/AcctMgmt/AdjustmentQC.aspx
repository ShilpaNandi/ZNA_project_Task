<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AcctMgmt_AdjustmentQC"
    Title="AdjustmentQC" CodeBehind="AdjustmentQC.aspx.cs" %>

<%@ Register Src="../App_Shared/AccountInfoHeader.ascx" TagName="AccountInfoHeader"
    TagPrefix="AI" %>
<%@ Register Src="~/App_Shared/AccountList.ascx" TagPrefix="AL" TagName="AccountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="../App_Shared/SaveCancel.ascx" TagName="SaveCancel" TagPrefix="SC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <table width="910" style="text-align: right">
        <tr>
            <td align="left">
                <asp:Label ID="lblAdjProcChklst" runat="server" Text="Adjustment QC" CssClass="h1"></asp:Label>
            </td>
            <%--<td align="right">
                <div id="UCSave">
                    <SC:SaveCancel ID="ucSaveCancel" runat="server" Visible="true" />
                </div>
            </td>--%>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
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
        function HideButtons()
        {
        $get('UCSave').style.display="none";
        
        }
        function ShowButtons()
        {
        $get('UCSave').style.display="block";
        
        }

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

        function PageNavigation() 
        {
            window.location.href = "../AcctMgmt/AccountDashboard.aspx?tab=1&wID=<%= WindowName%>";
        }
    </script>

    <table>
        <tr>
            <td>
                <AI:AccountInfoHeader ID="ucAccountInfoHeader1" runat="server" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValSumSave" ValidationGroup="Save" CssClass="ValidationSummary"
        runat="server"></asp:ValidationSummary>
    <asp:ValidationSummary ID="ValSumSaveDetail" CssClass="ValidationSummary" ValidationGroup="QCDetailSave"
        runat="server"></asp:ValidationSummary>
    <asp:ValidationSummary ID="valSummarysearch" CssClass="ValidationSummary" ValidationGroup="search"
        runat="server"></asp:ValidationSummary>
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Adjustment QC" CssClass="h3"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlQCDate" ValidationGroup="search" runat="server" Visible="false">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="compqcdate" runat="server" ControlToValidate="ddlQCDate"
                                ValidationGroup="search" ValueToCompare="0" Text="*" ErrorMessage="Please select date"
                                Operator="NotEqual"></asp:CompareValidator>
                            <asp:Button ID="btnDisplay" ValidationGroup="search" runat="server" Visible="false"
                                Text="Search" OnClick="btnDisplay_click" />
                            <br />
                            <br />
                            <asp:Panel ID="pnlAdjQC" runat="server" Height="100px" BorderColor="Black" BorderWidth="1"
                                ScrollBars="Auto" Width="910px" DefaultButton="btnSave">
                                <table width="100%">
                                    <tr>
                                        <td align="right" style="width: 120px; padding-right: 10px">
                                            QC Date:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtQCDate" ValidationGroup="Save" />
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgQCDate"
                                                TargetControlID="txtQCDate" PopupPosition="TopRight" />
                                            <asp:ImageButton ID="imgQCDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                                CausesValidation="False" />
                                            <asp:RequiredFieldValidator ID="reqDate" runat="server" ErrorMessage="Please enter Date"
                                                ValidationGroup="Save" ControlToValidate="txtQCDate" Text="*"></asp:RequiredFieldValidator>
                                            <AjaxToolkit:MaskedEditExtender ID="mskReviewDate" runat="server" TargetControlID="txtQCDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" AcceptNegative="Left" ErrorTooltipEnabled="True" />
                                            <asp:RegularExpressionValidator ID="regtxtQCDate" runat="server" ControlToValidate="txtQCDate"
                                                ValidationExpression="(^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:(1[8-9]|[2-8]\d)\d\d)$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$)"
                                                ErrorMessage="Invalid QC Date" Text="*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                        </td>
                                        <td rowspan="2" valign="top">
                                            Comments:
                                        </td>
                                        <td rowspan="2" valign="top">
                                            <asp:TextBox ID="txtComment" Width="350px" runat="server" TextMode="MultiLine" Rows="3"
                                            onpaste="doPaste(this,500);" MaxLength="500" onkeypress="doKeypress(this,500);"
                                            onbeforepaste="doBeforePaste(this,500);" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding-right: 10px">
                                            QC BY:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblQCBy" runat="server" Text='<%# Bind("PersonName") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <asp:Button ID="btnSave" ValidationGroup="Save" Text="Save" OnClick="btnSave_Click"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblAdjQCDetails" Visible="false" runat="server" Text="Adjustment QC Details"
                                CssClass="h3"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlAdjQCDetails" runat="server" Height="175px" Visible="false" ScrollBars="Auto"
                                Width="910px">
                                <asp:AISListView ID="lstAdjQCDetails" runat="server" InsertItemPosition="FirstItem"
                                    DataKeyNames="LookUpID" OnItemCommand="CommandList" OnItemDataBound="DataBoundList">
                                    <LayoutTemplate>
                                        <table id="Table1" class="panelContents" runat="server" width="98%">
                                            <tr class="LayoutTemplate">
                                                <th>
                                                </th>
                                                <th>
                                                    Account
                                                </th>
                                                <th>
                                                    ProgramPeriod
                                                </th>
                                                <th>
                                                    QC Issues
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
                                        <tr id="Tr1" runat="server" class="ItemTemplate">
                                            <td align="left">
                                            </td>
                                            <td>
                                                <%# Eval("Reg_AccountName")%>
                                            </td>
                                            <td>
                                                <%# Eval("ProgramPeriodStDate", "{0:d}") + ":" + Eval("ProgramPeriodEndDate", "{0:d}")%>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidLkupid" runat="server" Value='<%# Eval("LookUpID")%>' />
                                                <asp:HiddenField ID="hidQltyChkID" runat="server" />
                                                <%# Eval("CHKLISTNAME")%>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                                                    runat="server" Enabled=' <%#(Eval("ENABLED")==null)?false:(Eval("ENABLED"))%>' ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr2" runat="server" class="AlternatingItemTemplate">
                                            <td align="left">
                                            </td>
                                            <td>
                                                <%# Eval("Reg_AccountName")%>
                                            </td>
                                            <td>
                                                <%# Eval("ProgramPeriodStDate", "{0:d}") + ":" + Eval("ProgramPeriodEndDate", "{0:d}")%>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidLkupid" runat="server" Value='<%# Eval("LookUpID")%>' />
                                                <asp:HiddenField ID="hidQltyChkID" runat="server" />
                                                <%# Eval("CHKLISTNAME")%>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Bind("ACTIVE")%>' />
                                                <asp:ImageButton ID="imgDisable" CommandArgument='<%# Bind("QualityControlChklst_ID") %>'
                                                    runat="server" Enabled=' <%#(Eval("ENABLED")==null)?false:(Eval("ENABLED"))%>' ImageUrl='<%# Eval("ACTIVE").ToString()=="True"?"~/images/disabled.GIF":"~/images/enabled.GIF" %>'>
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <InsertItemTemplate>
                                        <tr class="ItemTemplate">
                                            <td align="left" style="vertical-align: bottom; padding-bottom: 5px">
                                                <asp:LinkButton ID="LinkButton1" runat="server" ValidationGroup="QCDetailSave" CommandName="Save"
                                                    Text="SAVE"></asp:LinkButton>
                                            </td>
                                            <td style="vertical-align: top">
                                                <%-- <asp:Panel ID="pnlLinks" runat="server">
                                                    <AL:AccountList ID="ddlAcctlist" runat="server" AccountType="2" MasterAccount='<%#AISMasterEntities.AccountNumber %>' />
                                                </asp:Panel>--%>
                                                <asp:DropDownList ID="ddlAccountlist" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAcctlist_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlAccountlist"
                                                    ValidationGroup="QCDetailSave" ValueToCompare="0" Text="*" ErrorMessage="Please select Account"
                                                    Operator="NotEqual"></asp:CompareValidator>
                                            </td>
                                            <td style="vertical-align: bottom; padding-bottom: 5px">
                                                <asp:DropDownList ID="ddlProgramPeriods" runat="server" Width="150px">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="compProgPerd" runat="server" ControlToValidate="ddlProgramPeriods"
                                                    ValidationGroup="QCDetailSave" ValueToCompare="0" Text="*" ErrorMessage="Please select ProgramPeriod"
                                                    Operator="NotEqual"></asp:CompareValidator>
                                            </td>
                                            <td align="left" style="vertical-align: bottom; padding-bottom: 5px">
                                                <asp:ObjectDataSource ID="lookupDataSource" runat="server" SelectMethod="GetLookUpActiveData"
                                                    TypeName="ZurichNA.AIS.Business.Logic.BLAccess">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="AdjustmentQC" Name="lookUpTypeName" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:DropDownList ID="ddlQCIssue" runat="server" ValidationGroup="QCDetailSave">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="compIssue" runat="server" ControlToValidate="ddlQCIssue"
                                                    ValidationGroup="QCDetailSave" ValueToCompare="0" Text="*" ErrorMessage="Please select an Item"
                                                    Operator="NotEqual"></asp:CompareValidator>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                </asp:AISListView>
                            </asp:Panel>
                            <div id="divBtnSave" visible="false" style="text-align: center; width: 910px" runat="server">
                                <asp:Button ID="btnApprove" runat="server" Text="APPROVE" OnClick="btnApprove_Click" />
                                <asp:Button ID="btnReject" runat="server" Text="REJECT" OnClick="btnReject_Click" />
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <%--<div id="divBtnSave" style="text-align: center; width: 500px" runat="server">
                                <asp:Button ID="btnApprove" runat="server" Text="APPROVE" OnClick="btnApprove_Click" />
                                <asp:Button ID="btnReject" runat="server" Text="REJECT" OnClick="btnReject_Click" />
                            </div>--%>
            </td>
        </tr>
    </table>
    <%-- <table>
        <tr>
            <td align="center">
                <asp:Button ID="btnApprove" runat="server" Text="APPROVE" />
                <asp:Button ID="btnReject" runat="server" Text="REJECT" OnClick="btnReject_Click"/>
            </td>
        </tr>
    </table>--%>
    <asp:UpdatePanel runat="server" ID="upAdjustmentQC">
        <ContentTemplate>
    <asp:Button Width="0px" runat="server" ID="QCTemp" Style="display: none" />
            <AjaxToolkit:ModalPopupExtender runat="server" ID="modalAdjustmentQC" TargetControlID="QCTemp"
                PopupControlID="pnlAdjustmentQC" BackgroundCssClass="modalBackground" DropShadow="true"
                CancelControlID="btnClosepopup">
            </AjaxToolkit:ModalPopupExtender>
            <div style="float: left;">
                <asp:Panel runat="server" CssClass="modalPopup" ID="pnlAdjustmentQC" Style="border: solid 1px black;
                    display: none; width: 650px; padding: 0px" HorizontalAlign="Center">
                    <asp:Panel runat="Server" ID="Panel2" Style="width: 100%; cursor: move; padding: 0px;
                        background-color: #CCCCCC; height: 20px; border: solid 1px Gray; color: Black;
                        text-align: center; vertical-align: middle">
                    </asp:Panel>
                    <div style="text-align: center; width: 100%; padding-bottom: 5px; background-color: White;">
                        <br />
                        Your update was not saved to the database, another user may be updating this page at this time.
                        <br />
                        <asp:Button Width="60px" ID="btnClosepopup" runat="server" Text="ok" OnClientClick="PageNavigation();"  />
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
