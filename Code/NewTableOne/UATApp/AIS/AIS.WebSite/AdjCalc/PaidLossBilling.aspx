<%@ Page Language="C#" MasterPageFile="~/Retro.master" AutoEventWireup="true" Inherits="AdjCalc_PaidLossBilling"
    Title="Untitled Page" CodeBehind="PaidLossBilling.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/App_Shared/AdjustmentReviewSearch.ascx" TagPrefix="ARS" TagName="AdjustmentReviewSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="Server">
    <asp:Label ID="GAILabel" runat="server" Text="Adjustment Review" CssClass="h1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="Server">

    <script src="../JavaScript/RetroScript.js" type="text/javascript">
    </script>

    <script language="javascript" type="text/javascript">
        //Function to Redirect to differenet Tabs
        function AddAdjustedAmount(txtAdjIndem, txtAdjExps, txtAdjustedAmt) {
            var myRegExp = /,|_/g;
            var AdjIndem = $get(txtAdjIndem).value.replace(myRegExp, '');
            var AdjExps = $get(txtAdjExps).value.replace(myRegExp, '');
            var result = 0;
            if (AdjExps != "") {
                result += parseFloat(AdjExps);
            }
            if (AdjIndem != "") {
                result += parseFloat(AdjIndem);
            }
            $get(txtAdjustedAmt).value = fix(result);

            FormatNumWithDecAmt($get(txtAdjustedAmt), 11);
        }
        //Function to round the given decimal number
        //Math.pow(10,N), Based on N Value that many Places this will round off
        function fix(fixNumber) {
            var div = Math.pow(10, 2);
            fixNumber = Math.round(fixNumber * div) / div;
            return fixNumber;
        }

        var scrollTop1;

        if (Sys.WebForms.PageRequestManager != null) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function BeginRequestHandler(sender, args) {
            var mef = $get('<%=pnlplb.ClientID%>');
            if (mef != null)
                scrollTop1 = mef.scrollTop;
        }

        function EndRequestHandler(sender, args) {
            var mef = $get('<%=pnlplb.ClientID%>');
            if (mef != null)
                mef.scrollTop = scrollTop1;
        }

        function Tabnavigation(Pagename) {
            var selectedValues = $get('<%=hidSelectedValues.ClientID%>');
            var strURL = "../AdjCalc/";
            if (Pagename == "ARC") {
                strURL += "AdjustmentReview.aspx";
            }
            else if (Pagename == "ARPLB") {
                strURL += "PaidLossBilling.aspx";
            }
            else if (Pagename == "AREA") {
                strURL += "EscrowAdjustment.aspx";
            }
            else if (Pagename == "ARCE") {
                strURL += "CombinedElements.aspx";
            }
            else if (Pagename == "ARNYSIF") {
                strURL += "NY-SIF.aspx";
            }
            else if (Pagename == "ARMI") {
                strURL += "MiscInvoicing.aspx";
            }
            else if (Pagename == "ARLRFP") {
                strURL += "LRFPostingDetails.aspx";
            }
            else if (Pagename == "ARAPCL") {
                strURL += "AdjustProcessingChklst.aspx";
            }
            else if (Pagename == "ARANTM") {
                strURL += "AdjustmentNumberTextMaintenance.aspx";
            }
            if (selectedValues.value != "") {
                strURL += "?SelectedValues=" + selectedValues.value;
            }
            window.location.href = strURL;
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

    </script>

    <table>
        <tr>
            <td>
                <cc1:TabContainer ID="TabContainer2" runat="server" CssClass="VariableTabs" SkinID="tabVariable"
                    ActiveTabIndex="1">
                    <cc1:TabPanel runat="server" ID="TabPanel1">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARC')">
                                Review Comments
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel2">
                        <HeaderTemplate>
                            Adj. PLB
                        </HeaderTemplate>
                        <ContentTemplate>
                            <br />
                            <asp:ObjectDataSource ID="odsAdjNumber" runat="server" SelectMethod="GetAdjNumberSearch"
                                TypeName="ZurichNA.AIS.Business.Logic.PremAdjustmentBS">
                                <SelectParameters>
                                    <asp:Parameter Name="straccountID" Type="String" />
                                    <asp:Parameter Name="strValDate" Type="String" />
                                    <asp:Parameter Name="intPremAdjPgmID" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlSelectionHeader" BorderColor="Black" BorderWidth="1px" Width="60%"
                                        runat="server" class="panelExtContents">
                                        <asp:ValidationSummary ID="valCombinedElems" runat="server" ValidationGroup="Update"
                                            BorderColor="Red" BorderWidth="1" BorderStyle="Solid" />
                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                            <tr style="background-color: #608CC8; color: White">
                                                <td width="26%" height="20" align="center" valign="top">
                                                    <asp:Label ID="lblselectMessage" Font-Bold="true" Font-Size="Small" runat="server"
                                                        Text="Please make selection" Style="font-family: Verdana; font-size: 11px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSearch" runat="server" BorderColor="Black" BorderWidth="1px" Width="60%">
                                        <ARS:AdjustmentReviewSearch ID="ARS" runat="server" />
                                        <asp:HiddenField ID="hidSelectedValues" runat="server" />
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlplb" runat="server" Width="910px" Height="210px" ScrollBars="Auto">
                                        <asp:Label ID="lblPLSBills" runat="server" CssClass="h3"></asp:Label>
                                        <asp:AISListView ID="lsvplb" runat="server" DataKeyNames="PREM_ADJ_PAID_LOS_BIL_ID"
                                            OnItemCanceling="CancelList" OnItemEditing="EditList" OnItemUpdating="UpdateList"
                                            OnItemDataBound="lsvplb_DataBoundList">
                                            <LayoutTemplate>
                                                <table id="Table1" class="panelContents" runat="server" width="98%">
                                                    <tr class="LayoutTemplate">
                                                        <th>
                                                        </th>
                                                        <th>
                                                            Valuation
                                                            <br />
                                                            Date
                                                        </th>
                                                        <th>
                                                            Policy Number
                                                        </th>
                                                        <th>
                                                            COV
                                                        </th>
                                                        <th>
                                                            Adjustment
                                                            <br />
                                                            Type
                                                        </th>
                                                        <th>
                                                            Indemnity
                                                        </th>
                                                        <th>
                                                            Adjusted<br />
                                                            Indemnity
                                                        </th>
                                                        <th>
                                                            Expenses
                                                        </th>
                                                        <th>
                                                            Adjusted<br />
                                                            Expenses
                                                        </th>
                                                        <th>
                                                            Total<br />
                                                            PLB Amount
                                                        </th>
                                                        <th>
                                                            Adjusted<br />
                                                            Amount
                                                        </th>
                                                        <th>
                                                            Comment
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trItemTemplate" runat="server" class="AlternatingItemTemplate">
                                                    <td align="center">
                                                        <asp:HiddenField ID="hidplbID" runat="server" Value='<%# Bind("PREM_ADJ_PAID_LOS_BIL_ID") %>' />
                                                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblValDate" runat="server" Text='<%# Bind("LSI_VALN_DATE","{0:d}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPolicy" runat="server" Text='<%# Bind("POLICYSYMBOL") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjTyp" runat="server" Text='<%# Bind("LSI_PGM_TYP") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblIndem" runat="server" Text='<%# Eval("IDNMTY_AMT") != null ? (Eval("IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("IDNMTY_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjIndem" runat="server" Text='<%# Eval("ADJ_IDNMTY_AMT") != null ? (Eval("ADJ_IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_IDNMTY_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblExps" runat="server" Text='<%# Eval("EXPS_AMT") != null ? (Eval("EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("EXPS_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjExps" runat="server" Text='<%# Eval("ADJ_EXPS_AMT") != null ? (Eval("ADJ_EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_EXPS_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblTotPLSAmt" runat="server" Text='<%# Eval("TOT_PAID_LOS_BIL_AMT") != null ? (Eval("TOT_PAID_LOS_BIL_AMT").ToString() != "" ?(decimal.Parse(Eval("TOT_PAID_LOS_BIL_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjustedAmt" runat="server" Text='<%# Eval("ADJ_TOT_PAID_LOS_BIL_AMT") != null ? (Eval("ADJ_TOT_PAID_LOS_BIL_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_TOT_PAID_LOS_BIL_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="txtComment" runat="server" Text='<%# Bind("CMMNT_TXT") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EditItemTemplate>
                                                <tr class="ItemTemplate">
                                                    <td align="center">
                                                        <asp:HiddenField ID="hidplbID" runat="server" Value='<%# Bind("PREM_ADJ_PAID_LOS_BIL_ID") %>' />
                                                        <asp:LinkButton ID="lnkUpdate" ValidationGroup="Update" CommandArgument='<%# Bind("PREM_ADJ_PAID_LOS_BIL_ID") %>'
                                                            CommandName="Update" runat="server" Text="UPDATE"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="CANCEL"></asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblValDate" runat="server" Text='<%# Bind("LSI_VALN_DATE","{0:d}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPolicy" runat="server" Text='<%# Bind("POLICYSYMBOL") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjTyp" runat="server" Text='<%# Bind("LSI_PGM_TYP") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblIndem" runat="server" Text='<%# Eval("IDNMTY_AMT") != null ? (Eval("IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("IDNMTY_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:AISAmountTextbox ID="txtAdjIndem" ValidationGroup="Update" runat="server" Text='<%# Eval("ADJ_IDNMTY_AMT")!= null ? (Eval("ADJ_IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_IDNMTY_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'
                                                            AllowDecimal="true" AllowNegetive="true" Width="105px" Enabled='<%# (Eval("LSI_PGM_TYP").ToString()=="Incurred Underlayer"  ||  Eval("LSI_PGM_TYP").ToString()=="Paid Loss Underlayer" || Eval("LSI_PGM_TYP").ToString()=="Incurred Loss WA" || Eval("LSI_PGM_TYP").ToString()=="Paid Loss WA")?true:false %>' />
                                                        <asp:RequiredFieldValidator ID="reqAdjIndem" runat="server" ErrorMessage="Please enter Adjusted Indemnity."
                                                            Text="*" ValidationGroup="Update" ControlToValidate="txtAdjIndem"></asp:RequiredFieldValidator>
                                                        <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtAdjIndem" FilterType="Custom"
                                                            ValidChars="-0123456789,." ID="fltrAdjIndem">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                        <%--<cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtAdjIndem"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AutoComplete="false" />
                                                                --%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblExps" runat="server" Text='<%# Eval("EXPS_AMT") != null ? (Eval("EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("EXPS_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:AISAmountTextbox ID="txtAdjExps" ValidationGroup="Update" AllowDecimal="true"
                                                            AllowNegetive="true" runat="server" Width="105px" Text='<%# Eval("ADJ_EXPS_AMT")!= null ? (Eval("ADJ_EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_EXPS_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'
                                                            Enabled='<%# (Eval("LSI_PGM_TYP").ToString()=="Incurred Underlayer"  ||  Eval("LSI_PGM_TYP").ToString()=="Paid Loss Underlayer" || Eval("LSI_PGM_TYP").ToString()=="Incurred Loss WA" || Eval("LSI_PGM_TYP").ToString()=="Paid Loss WA")?true:false %>' />
                                                        <asp:RequiredFieldValidator ID="reqAdjExps" runat="server" ErrorMessage="Please enter Adjusted Expenses."
                                                            Text="*" ValidationGroup="Update" ControlToValidate="txtAdjExps"></asp:RequiredFieldValidator>
                                                        <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtAdjExps" FilterType="Custom"
                                                            ValidChars="-0123456789,." ID="fltrAdjExps">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                        <%--<cc1:MaskedEditExtender ID="MaskedtxttxtAdjExps" runat="server" TargetControlID="txtAdjExps"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AutoComplete="false" />--%>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblTotPLSAmt" runat="server" Text='<%# Eval("TOT_PAID_LOS_BIL_AMT") != null ? (Eval("TOT_PAID_LOS_BIL_AMT").ToString() != "" ?(decimal.Parse(Eval("TOT_PAID_LOS_BIL_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <%--<asp:Label ID="lblAdjustedAmt" runat="server" Text='<%# Bind("ADJ_TOT_PAID_LOS_BIL_AMT","{0:0}") %>'></asp:Label>--%>
                                                        <asp:AISAmountTextbox ID="txtAdjustedAmt" AllowDecimal="true"
                                                            AllowNegetive="true" runat="server" Width="105px" Text='<%# Eval("ADJ_TOT_PAID_LOS_BIL_AMT") != null ? (Eval("ADJ_TOT_PAID_LOS_BIL_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_TOT_PAID_LOS_BIL_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:AISAmountTextbox>
                                                        <%--<cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtAdjustedAmt" FilterType="Custom"
                                                            ValidChars="-0123456789,." ID="fltrAdjustedAmt">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                        <%--<cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtAdjustedAmt"
                                                                Mask="99,999,999,999.99" MessageValidatorTip="true" MaskType="Number" DisplayMoney="None"
                                                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft"
                                                                AutoComplete="false" />--%>
                                                    </td>
                                                    <td align="center">
                                                        <div style="width: 67px; height: 50px; background-color: White; vertical-align: middle;">
                                                            <asp:TextBox ID="txtComment" ValidationGroup="Update" MaxLength="4000" runat="server"
                                                                Text='<%# Bind("CMMNT_TXT") %>' Width="66px" Height="50px" Wrap="false" TextMode="MultiLine"
                                                                onpaste="doPaste(this,4000);" onkeypress="doKeypress(this,4000);" onbeforepaste="doBeforePaste(this,4000);"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <tr id="trItemTemplate" runat="server" class="ItemTemplate">
                                                    <td align="center">
                                                        <asp:HiddenField ID="hidplbID" runat="server" Value='<%# Bind("PREM_ADJ_PAID_LOS_BIL_ID") %>' />
                                                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="EDIT"></asp:LinkButton>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblValDate" runat="server" Text='<%# Bind("LSI_VALN_DATE","{0:d}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPolicy" runat="server" Text='<%# Bind("POLICYSYMBOL") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjTyp" runat="server" Text='<%# Bind("LSI_PGM_TYP") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblIndem" runat="server" Text='<%# Eval("IDNMTY_AMT") != null ? (Eval("IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("IDNMTY_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAdjIndem" Width="80px" runat="server" Text='<%# Eval("ADJ_IDNMTY_AMT") != null ? (Eval("ADJ_IDNMTY_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_IDNMTY_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblExps" runat="server" Text='<%# Eval("EXPS_AMT") != null ? (Eval("EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("EXPS_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAdjExps" runat="server" Width="80px" Text='<%# Eval("ADJ_EXPS_AMT") != null ? (Eval("ADJ_EXPS_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_EXPS_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTotPLSAmt" runat="server" Text='<%# Eval("TOT_PAID_LOS_BIL_AMT") != null ? (Eval("TOT_PAID_LOS_BIL_AMT").ToString() != "" ?(decimal.Parse(Eval("TOT_PAID_LOS_BIL_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAdjustedAmt" Width="80px" runat="server" Text='<%# Eval("ADJ_TOT_PAID_LOS_BIL_AMT") != null ? (Eval("ADJ_TOT_PAID_LOS_BIL_AMT").ToString() != "" ?(decimal.Parse(Eval("ADJ_TOT_PAID_LOS_BIL_AMT").ToString())).ToString("#,##0.00") : "0"): "0"%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="txtComment" runat="server" Text='<%# Bind("CMMNT_TXT") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:AISListView>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlAdjReviewEmptyData" runat="server" Visible="false">
                                        <table id="Table1" visible="true" runat="server" class="panelContents" width="900px">
                                            <tr align="center">
                                                <th>
                                                    Valuation
                                                    <br />
                                                    Date
                                                </th>
                                                <th>
                                                    Policy Number
                                                </th>
                                                <th>
                                                    COV
                                                </th>
                                                <th>
                                                    Adjustment
                                                    <br />
                                                    Type
                                                </th>
                                                <th>
                                                    Indemnity
                                                </th>
                                                <th>
                                                    Adjusted<br />
                                                    Indemnity
                                                </th>
                                                <th>
                                                    Expenses
                                                </th>
                                                <th>
                                                    Adjusted<br />
                                                    Expenses
                                                </th>
                                                <th>
                                                    Total<br />
                                                    PLB Amount
                                                </th>
                                                <th>
                                                    Adjusted<br />
                                                    Amount
                                                </th>
                                                <th>
                                                    Comment
                                                </th>
                                            </tr>
                                        </table>
                                        <table width="900px">
                                            <tr id="Tr1" runat="server" class="ItemTemplate">
                                                <td align="center">
                                                    <asp:Label ID="lblEmptyMessage" Text="---PLB Not Done for above Search ---" Font-Bold="true"
                                                        runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel3">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('AREA')">
                                Escrow Adjustment
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel4">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARCE')">
                                Combined Elements
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel5">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARNYSIF')">
                                NY-SIF
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel6">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARMI')">
                                Misc. Invoicing
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlLRF">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARLRFP')">
                                LRF Posting
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAdjchklist">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARAPCL')">
                                Adj. Checklist
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tblpnlAdjNumberText">
                        <HeaderTemplate>
                            <div onclick="Tabnavigation('ARANTM')">
                                Adj. Number
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
